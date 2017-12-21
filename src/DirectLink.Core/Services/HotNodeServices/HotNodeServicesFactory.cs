// Copyright (c) .NET Foundation. All rights reserved.
// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.AspNetCore.NodeServices.HostingModels;
using Microsoft.Extensions.Logging;

namespace DirectLinkCore
{
    public class HotNodeServicesFactory : IDisposable
    {
        private readonly DirectLinkOptions _options;
        private readonly ILogger _logger;

        private readonly IHotNodeService[] _services;
        private int _currentService;

        private CancellationTokenSource _ctsFileWatcher;
        private FileSystemWatcher _fileWatcher;
        private Task _reloadDueToFilesChangesTask;

        private readonly CancellationTokenSource _ctsCommon;
        private readonly Task _memoryWatcherTask;

        private readonly object _lockObject = new object();

        public HotNodeServicesFactory(IServiceProvider provider)
        {
            _options = ((DirectLinkOptionsProvider)provider.GetService(typeof(DirectLinkOptionsProvider))).Options;
            _logger = _options.DirectLinkOutputLogger;
            _ctsCommon = new CancellationTokenSource();

            var instanceCount = _options.NodeInstanceCount;
            if (instanceCount <= 0) {
                throw new ArgumentNullException(nameof(instanceCount));
            }

            _services = new IHotNodeService[instanceCount];
            CreateNodeInstances(_options.NodeServicesOptions).Wait();

            _ctsFileWatcher = new CancellationTokenSource();
            var projectPath = Directory.GetCurrentDirectory();
            _fileWatcher = BeginFileWatcher(projectPath);

            _memoryWatcherTask = Task.Run(() => TrackMemoryUsage(_ctsCommon.Token), _ctsCommon.Token);
        }

        public IHotNodeService GetHotNodeService()
        {
            _currentService %= _services.Length;
            return _services[_currentService++];
        }

        private async Task CreateNodeInstances(NodeServicesOptions options)
        {
            for (int i = 0; i < _services.Length; ++i) {
                _services[i] = new HotNodeService(CreateNodeInstance(options));
                await WarmupService(_services[i], CancellationToken.None);
            }
        }

        private Task<string> WarmupService(IHotNodeService service, CancellationToken token)
        {
            return service.InvokeExportAsync<string>(CancellationToken.None, _options.WarmupPath, null, File.ReadAllText(_options.IndexTemplatePath), _options.HotModuleReplacement);
        }

        private FileSystemWatcher BeginFileWatcher(string rootDir)
        {
            var watchFileExtensions = _options.NodeServicesOptions.WatchFileExtensions;
            if (watchFileExtensions == null || watchFileExtensions.Length == 0) {
                return null;
            }

            var watcher = new FileSystemWatcher(rootDir) {
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
            };
            watcher.Changed += OnFileChanged;
            watcher.Created += OnFileChanged;
            watcher.Deleted += OnFileChanged;
            watcher.Renamed += OnFileRenamed;
            watcher.EnableRaisingEvents = true;
            return watcher;
        }

        private void OnFileChanged(object source, FileSystemEventArgs e)
        {
            if (IsFilenameBeingWatched(e.FullPath)) {
                _logger.LogInformation($"File {e.FullPath} {e.ChangeType.ToString().ToLower()}");
                StartReloadingServices();
            }
        }

        private void OnFileRenamed(object source, RenamedEventArgs e)
        {
            if (IsFilenameBeingWatched(e.OldFullPath) || IsFilenameBeingWatched(e.FullPath)) {
                _logger.LogInformation($"File {e.OldFullPath} renamed");
                StartReloadingServices();
            }
        }

        private void StartReloadingServices()
        {
            if (_reloadDueToFilesChangesTask != null && !_reloadDueToFilesChangesTask.IsCompleted) {
                _ctsFileWatcher.Cancel();
                try {
                    _reloadDueToFilesChangesTask.Wait();
                }
                catch (AggregateException ae) {
                    ae.Handle(ex => ex is TaskCanceledException);
                }
                _ctsFileWatcher = new CancellationTokenSource();
            }

            _reloadDueToFilesChangesTask = Task.Run(async () => {
                await Task.Delay(_options.FileWatcherDelay, _ctsFileWatcher.Token);
                lock (_lockObject) {
                    for (int i = 0; i < _services.Length; ++i) {
                        if (!_ctsCommon.Token.IsCancellationRequested && !_ctsFileWatcher.IsCancellationRequested) {
                            ReloadService(i, _ctsCommon.Token).Wait();
                        }
                    }
                }
            }, _ctsCommon.Token);
        }

        private bool IsFilenameBeingWatched(string fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath)) {
                return false;
            }
            var actualExtension = Path.GetExtension(fullPath) ?? string.Empty;
            return _options.NodeServicesOptions.WatchFileExtensions.Any(actualExtension.Equals);
        }

        private INodeInstance CreateNodeInstance(NodeServicesOptions options)
        {
            return new HttpNodeInstance(options.ProjectPath, null, options.ApplicationStoppingToken, options.NodeInstanceOutputLogger,
                options.EnvironmentVariables, options.InvocationTimeoutMilliseconds, options.LaunchWithDebugging, options.DebuggingPort, /* port */ 0);
        }

        private void TrackMemoryUsage(CancellationToken token)
        {
            var index = 0;
            while (true) {
                try {
                    token.WaitHandle.WaitOne(_options.MemoryWatcherInterval);
                    if (token.IsCancellationRequested) {
                        return;
                    }
                    if (Monitor.TryEnter(_lockObject)) {
                        try {
                            var usedMemory = _services[index].GetUsedMemory();
                            if (usedMemory > _options.NodeInstanceMemoryLimit) {
                                ReloadService(index, token).Wait();
                            }
                            index++;
                            index %= _services.Length;
                        }
                        finally {
                            Monitor.Exit(_lockObject);
                        }
                    }
                }
                catch (Exception ex) {
                    _logger.LogError(0, ex, "Tracking memory usage threw an exception");
                }
            }
        }

        private async Task ReloadService(int serviceIndex, CancellationToken token)
        {
            _logger.LogInformation($"Reloading NodeService [{serviceIndex}]");
            try {
                var oldService = _services[serviceIndex];

                if (_options.NodeServicesOptions.LaunchWithDebugging) {
                    oldService.Dispose();
                }
                var newService = new HotNodeService(CreateNodeInstance(_options.NodeServicesOptions));
                await WarmupService(newService, token);
                if (_options.HotModuleReplacement && serviceIndex == _services.Length - 1) {
                    if (DirectLinkHub.HubContext != null) {
                        await DirectLinkHub.HubContext.Clients.All.InvokeAsync(nameof(IDirectLinkHub.AssetsUpdate), new object[] { });
                    }
                }
                if (!_options.NodeServicesOptions.LaunchWithDebugging) {
#pragma warning disable 4014
                    Task.Run(async () => {
                        await Task.Delay(_options.NodeServiceDisposingDelay, token);
                        oldService.Dispose();
                    }, token);
#pragma warning restore 4014
                }

                _services[serviceIndex] = newService;
            }
            catch (AggregateException ae) {
                ae.Handle(ex => ex is TaskCanceledException);
            }
            catch (Exception ex) {
                _logger.LogError(0, ex, $"Reloading NodeService [{serviceIndex}] threw an exception");
            }
        }

        public void Dispose()
        {
            try {
                _ctsCommon.Cancel();
                _fileWatcher?.Dispose();
                _fileWatcher = null;

                try {
                    _memoryWatcherTask.Wait();
                }
                catch (AggregateException ae) {
                    ae.Handle(ex => ex is TaskCanceledException);
                }

                if (_reloadDueToFilesChangesTask != null) {
                    if (!_reloadDueToFilesChangesTask.IsCompleted) {
                        _ctsFileWatcher.Cancel();
                        try {
                            _reloadDueToFilesChangesTask.Wait();
                        }
                        catch (AggregateException ae) {
                            ae.Handle(ex => ex is TaskCanceledException);
                        }
                    }
                    _ctsFileWatcher.Dispose();
                }
            }
            catch (Exception ex) {
                _logger.LogError(0, ex, "Task cancellation during disposing threw an exception");
            }
            finally {
                _ctsCommon.Dispose();
            }

            foreach (var service in _services) {
                service.Dispose();
            }
        }
    }
}