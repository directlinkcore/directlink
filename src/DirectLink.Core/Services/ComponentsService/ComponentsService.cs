// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace DirectLinkCore
{
    internal class ComponentsService : IComponentsService, IDisposable
    {
        public static char Delimiter = '@';

        private readonly IDictionary<string, string> _components = new Dictionary<string, string>();
        private readonly IDictionary<string, ComponentInfo> _infos = new Dictionary<string, ComponentInfo>();

        private readonly ConcurrentDictionary<string, string> _assets;
        private Action<string, ConcurrentDictionary<string, string>> _assetsParser;

        private CancellationTokenSource _cts = new CancellationTokenSource();
        private FileSystemWatcher _watcher;
        private TimeSpan _watcherDelay;
        private Task _parseDueToFilesChangesTask;

        private readonly ILogger<ComponentsService> _logger;
        private readonly Func<Type, string> _componentNameConverter;
        private readonly Func<string, string> _fileNameConverter;
        private readonly string _scriptExtension;
        private readonly string _styleExtension;

        public ComponentsService(Type[] types, ILogger<ComponentsService> logger, DirectLinkOptionsProvider optionsProvider)
        {
            _logger = logger;
            var options = optionsProvider.Options;
            _componentNameConverter = options.ComponentNameConverter;
            _fileNameConverter = options.FileNameConverter;
            _scriptExtension = options.ScriptExtension;
            _styleExtension = options.StyleExtension;

            foreach (var type in types) {
                ProcessType(type);
            }

            _watcherDelay = options.FileWatcherDelay;
            _assetsParser = options.AssetsParser ?? AssetsParser;
            _assets = new ConcurrentDictionary<string, string>();

            var assetsPath = options.AssetsPath;
            if (File.Exists(assetsPath)) {
                _assetsParser(File.ReadAllText(assetsPath), _assets);
                _watcher = BeginFileWatcher(Path.GetDirectoryName(Path.GetFullPath(assetsPath)), Path.GetFileName(assetsPath));
            }
        }

        public string GetComponentName(Type type)
        {
            _components.TryGetValue(type.Name, out var component);
            return component;
        }

        public Type GetType(string component)
        {
            _infos.TryGetValue(component, out var info);
            return info.Type;
        }

        public MethodInfo GetMethodInfo(string component, string method)
        {
            _infos.TryGetValue(component, out var info);
            if (info.MethodInfos == null) {
                return null;
            }
            info.MethodInfos.TryGetValue(method, out var methodInfo);
            return methodInfo;
        }

        public IList<string> GetMethodNames(string component)
        {
            _infos.TryGetValue(component, out var info);
            return info.MethodNames;
        }

        public string GetScriptPath(string component)
        {
            if (_infos.TryGetValue(component, out var info)) {
                return GetAssetPath(info.ScriptName);
            }
            return null;
        }

        public string GetStylePath(string component)
        {
            if (_infos.TryGetValue(component, out var info)) {
                return GetAssetPath(info.StyleName);
            }
            return null;
        }

        public string GetAssetPath(string filename)
        {
            if (!string.IsNullOrWhiteSpace(filename)) {
                if (_assets.TryGetValue(filename, out string result)) {
                    return result;
                }
            }
            return null;
        }

        public void Configure(ConfigurationComponents components)
        {
            foreach (var component in components.GetComponents()) {
                var name = null as string;
                if (component.Type != null) {
                    name = _components[component.Type.Name];
                    if (component.Name != null) {
                        _components[component.Type.Name] = component.Name;
                        _infos.Add(component.Name, _infos[name]);
                        _infos.Remove(name);
                        name = component.Name;
                    }
                }
                else if (!string.IsNullOrWhiteSpace(component.Name)) {
                    name = component.Name;
                }
                else {
                    throw new ArgumentException("Use Map<T> or specify `componentName` parameter");
                }

                var filename = null as string;
                if (component.BaseName != null) {
                    filename = component.BaseName;
                }
                else if (component.Script != null) {
                    filename = Path.GetFileNameWithoutExtension(component.Script);
                }
                else if (component.Style != null) {
                    filename = Path.GetFileNameWithoutExtension(component.Style);
                }
                if (filename != null) {
                    if (!_infos.ContainsKey(name)) {
                        _infos.Add(name, new ComponentInfo { });
                    }
                    _infos[name].ScriptName = filename + _scriptExtension;
                    _infos[name].StyleName = filename + _styleExtension;
                }

                if (component.Script != null) {
                    _assets[_infos[name].ScriptName] = component.Script;
                }
                if (component.Style != null) {
                    _assets[_infos[name].StyleName] = component.Style;
                }
            }
        }

        private void ProcessType(Type type)
        {
            var directlinkAttribute = type.GetCustomAttribute<DirectLinkAttribute>();
            var name = directlinkAttribute?.Component ?? _componentNameConverter(type);
            _components.Add(type.Name, name);
            var info = new ComponentInfo { Type = type };

            var filename = directlinkAttribute?.BaseName ?? _fileNameConverter(name);
            info.ScriptName = filename + _scriptExtension;
            info.StyleName = filename + _styleExtension;
            if (directlinkAttribute?.Script != null) {
                _assets[filename + _scriptExtension] = directlinkAttribute?.Script;
            }
            if (directlinkAttribute?.Style != null) {
                _assets[filename + _styleExtension] = directlinkAttribute?.Style;
            }

            info.MethodInfos = type.GetTypeInfo().GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(methodInfo => !methodInfo.IsSpecialName && methodInfo.DeclaringType != typeof(object) && !methodInfo.IsVirtual)
                .ToDictionary(methodInfo => $"{methodInfo.Name}{Delimiter}{methodInfo.GetParameters().Length}", methodInfo => methodInfo);

            info.MethodNames = info.MethodInfos.Select(pair => {
                var contexted = pair.Value.GetParameters().FirstOrDefault()?.ParameterType == typeof(HubCallerContext);
                return (contexted ? DirectLinkHub.MethodPrefix : "") + pair.Key.Split(Delimiter)[0];
            }).ToList();

            _infos.Add(name, info);
        }

        private FileSystemWatcher BeginFileWatcher(string path, string filename)
        {
            var watcher = new FileSystemWatcher(path, filename) { NotifyFilter = NotifyFilters.LastWrite };
            watcher.Changed += OnFileChanged;
            watcher.EnableRaisingEvents = true;
            return watcher;
        }

        private void OnFileChanged(object source, FileSystemEventArgs e)
        {
            _logger.LogInformation($"{e.FullPath} changed");
            StartParseAssets(e.FullPath);
        }

        private void StartParseAssets(string path)
        {
            if (_parseDueToFilesChangesTask != null && !_parseDueToFilesChangesTask.IsCompleted) {
                _cts.Cancel();
                try {
                    _parseDueToFilesChangesTask.Wait();
                }
                catch (AggregateException ae) {
                    ae.Handle(ex => ex is TaskCanceledException);
                }
                _cts = new CancellationTokenSource();
            }

            _parseDueToFilesChangesTask = Task.Run(async () => {
                await Task.Delay(_watcherDelay, _cts.Token);
                if (!_cts.IsCancellationRequested) {
                    _assetsParser(File.ReadAllText(path), _assets);
                }
            });
        }

        private void AssetsParser(string fileContent, ConcurrentDictionary<string, string> assets)
        {
            try {
                var jobject = JObject.Parse(fileContent);
                foreach (var property in jobject.Properties()) {
                    foreach (var token in property.Values()) {
                        var tokenValue = ((token as JProperty)?.Value as JValue)?.Value.ToString();
                        if (tokenValue != null) {
                            _assets[token.Path] = tokenValue;
                        }
                    }
                }
            }
            catch (Exception ex) {
                _logger.LogError(0, ex, "Exception while parsing assets");
            }
        }

        public void Dispose()
        {
            _watcher?.Dispose();
            _watcher = null;

            if (_parseDueToFilesChangesTask != null) {
                if (!_parseDueToFilesChangesTask.IsCompleted) {
                    _cts.Cancel();
                    try {
                        _parseDueToFilesChangesTask.Wait();
                    }
                    catch (AggregateException ae) {
                        ae.Handle(ex => ex is TaskCanceledException);
                    }
                }
                _cts.Dispose();
            }
        }

        internal class ComponentInfo
        {
            public Type Type { get; set; }
            public IDictionary<string, MethodInfo> MethodInfos { get; set; }
            public IList<string> MethodNames { get; set; }
            public string ScriptName { get; set; }
            public string StyleName { get; set; }
        }
    }
}