// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DirectLinkCore
{
    public class DirectLinkHub : Hub<IDirectLinkHub>
    {
        public static IHubContext<DirectLinkHub> HubContext;
        public static string MethodPrefix = "$";

        private static readonly ConcurrentDictionary<string, int> LinkCount = new ConcurrentDictionary<string, int>();
        private static readonly ConcurrentDictionary<string, long> RequestNextTime = new ConcurrentDictionary<string, long>();
        private static readonly ConcurrentDictionary<string, CancellationTokenSource> TokenSources = new ConcurrentDictionary<string, CancellationTokenSource>();
        private static readonly ConcurrentDictionary<string, Dictionary<string, string>> Links = new ConcurrentDictionary<string, Dictionary<string, string>>();

        private readonly IServiceProvider _provider;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IRoutingService _routingService;
        private readonly IComponentsService _componentsService;
        private readonly ILogger<DirectLinkHub> _logger;

        private int _requestsPerSecond;
        private int _queuedRequestCount;
        private int _maxLinkCount;
        private int _requestDelay;
        private long _requestDelayTicks;

        public DirectLinkHub(IServiceProvider provider)
        {
            _provider = provider;
            _authenticationService = provider.GetService<IAuthenticationService>();
            _authorizationService = provider.GetService<IAuthorizationService>();
            _routingService = provider.GetRequiredService<IRoutingService>();
            _componentsService = provider.GetRequiredService<IComponentsService>();

            var options = provider.GetRequiredService<DirectLinkOptionsProvider>().Options;
            _requestsPerSecond = options.RequestsPerSecond;
            if (_requestsPerSecond > 0) {
                _queuedRequestCount = options.QueuedRequestCount;
                _maxLinkCount = options.MaxLinkCount;
                _requestDelay = 1000 / _requestsPerSecond;
                _requestDelayTicks = _requestDelay * 10000;
            }

            _logger = provider.GetRequiredService<ILogger<DirectLinkHub>>();
            if (HubContext == null) {
                HubContext = provider.GetRequiredService<IHubContext<DirectLinkHub>>();
            }
        }

        public override async Task OnConnectedAsync()
        {
            if (_requestsPerSecond > 0) {
                LinkCount[Context.ConnectionId] = 0;
                TokenSources[Context.ConnectionId] = new CancellationTokenSource();
            }
            if (_authenticationService != null) {
                await _authenticationService.SetPrincipalAsync(Context.ConnectionId, Context.User);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (_authenticationService != null) {
                await _authenticationService.LogoutAsync(Context);
            }
            if (Links.TryRemove(Context.ConnectionId, out Dictionary<string, string> links)) {
                foreach (var link in links) {
                    var fullname = link.Key;
                    var name = link.Value;
                    var dispatcher = _provider.GetService(_componentsService.GetType(name)) as IDirectLinkDispatcher<ViewModel>;
                    if (dispatcher?.Connections.AddOrUpdate(fullname, new HashSet<string>(), (k, v) => {
                        if (v.Contains(Context.ConnectionId)) {
                            v.Remove(Context.ConnectionId);
                        }
                        return v;
                    }).Count == 0) {
                        dispatcher?.Connections.TryRemove(fullname, out var _);
                        dispatcher?.Props.TryRemove(fullname, out var _);
                    }
                    await dispatcher.OnDisconnectedAsync(Context.ConnectionId);
                }
            }

            if (_requestsPerSecond > 0) {
                LinkCount.TryRemove(Context.ConnectionId, out var _);
                RequestNextTime.TryRemove(Context.ConnectionId, out var _);
                TokenSources.TryRemove(Context.ConnectionId, out var _);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task Link(string fullname, string name, object props)
        {
            if (IsLinkRequestLimited()) {
                Context.Connection.Abort();
                return;
            }
            if (string.IsNullOrWhiteSpace(fullname) || string.IsNullOrWhiteSpace(name)) {
                return;
            }
            var type = _componentsService.GetType(name);
            if (type == null) {
                return;
            }

            Links.AddOrUpdate(Context.ConnectionId, new Dictionary<string, string> { { fullname, name } }, (k, v) => {
                if (!v.ContainsKey(fullname)) {
                    v.Add(fullname, name);
                }
                return v;
            });
            var dispatcher = _provider.GetService(type) as IDirectLinkDispatcher<ViewModel>;
            if (dispatcher != null) {
                dispatcher.Props[fullname] = props;
                dispatcher.Connections.AddOrUpdate(fullname, new HashSet<string> { Context.ConnectionId }, (k, v) => {
                    if (!v.Contains(Context.ConnectionId)) {
                        v.Add(Context.ConnectionId);
                    }
                    return v;
                });
            }
            await this.Groups.AddAsync(Context.ConnectionId, fullname);
            await dispatcher?.OnConnectedAsync(Context.ConnectionId);
        }

        public async Task Unlink(string fullname, string name)
        {
            if (IsLinkRequestLimited()) {
                Context.Connection.Abort();
                return;
            }
            if (string.IsNullOrWhiteSpace(fullname) || string.IsNullOrWhiteSpace(name)) {
                return;
            }
            var type = _componentsService.GetType(name);
            if (type == null) {
                return;
            }

            Links.AddOrUpdate(Context.ConnectionId, new Dictionary<string, string>(), (k, v) => {
                if (v.ContainsKey(fullname)) {
                    v.Remove(fullname);
                }
                return v;
            });
            await this.Groups.RemoveAsync(Context.ConnectionId, fullname);
            var dispatcher = _provider.GetService(type) as IDirectLinkDispatcher<ViewModel>;
            if (dispatcher?.Connections.AddOrUpdate(fullname, new HashSet<string>(), (k, v) => {
                if (v.Contains(Context.ConnectionId)) {
                    v.Remove(Context.ConnectionId);
                }
                return v;
            }).Count == 0) {
                dispatcher?.Connections.TryRemove(fullname, out var _);
                dispatcher?.Props.TryRemove(fullname, out var _);
            }
            await dispatcher?.OnDisconnectedAsync(Context.ConnectionId);
        }

        public async Task RequestData(string path, string referer)
        {
            var abortedToken = Context.Connection.ConnectionAbortedToken;
            if (abortedToken.IsCancellationRequested) {
                return;
            }
            if (_requestsPerSecond > 0 && TokenSources[Context.ConnectionId].Token.IsCancellationRequested) {
                return;
            }

            var client = this.Clients.Client(Context.ConnectionId);
            if (await IsRequestLimited()) {
                if (!abortedToken.IsCancellationRequested) {
                    client.DataResponse(new DataResponse(StatusCodes.Status429TooManyRequests));
                }
                if (!TokenSources[Context.ConnectionId].Token.IsCancellationRequested) {
                    TokenSources[Context.ConnectionId].Cancel();
                    Context.Connection.Abort();
                }
                return;
            }

            _logger.LogInformation($"RequestData: {Context.ConnectionId} {path}");
            if (_requestsPerSecond > 0) {
                LinkCount[Context.ConnectionId] = 0;
            }

            var context = await GetDirectLinkContext(path, referer);
            var response = await _routingService.GetResponseAsync(context);
            if (!abortedToken.IsCancellationRequested) {
                client.DataResponse(response);
            }
        }

        private async Task<DirectLinkContext> GetDirectLinkContext(string path, string referer)
        {
            var context = _provider.GetService<DirectLinkContext>();
            context.HubCallerContext = Context;
            context.HttpContext = Context.Connection.GetHttpContext();
            context.Path = path;
            context.HubReferer = referer;
            if (_authenticationService != null) {
                var user = await _authenticationService.GetPrincipalAsync(Context.ConnectionId);
                var authenticationFeature = context.HttpContext.Features.Get<IHttpAuthenticationFeature>();
                authenticationFeature.User = user;
            }
            return context;
        }

        public async Task Invoke(Guid invocationId, string fullname, string name, string method, object[] args)
        {
            var abortedToken = Context.Connection.ConnectionAbortedToken;
            if (abortedToken.IsCancellationRequested) {
                return;
            }

            var connectionId = Context.ConnectionId;
            if (_requestsPerSecond > 0 && TokenSources[connectionId].Token.IsCancellationRequested) {
                return;
            }

            var client = this.Clients.Client(connectionId);
            if (await IsRequestLimited()) {
                if (!abortedToken.IsCancellationRequested) {
                    client.InvokeResult(invocationId, new { StatusCode = StatusCodes.Status429TooManyRequests });
                }
                if (!TokenSources[connectionId].Token.IsCancellationRequested) {
                    TokenSources[connectionId].Cancel();
                    Context.Connection.Abort();
                }
                return;
            }
            if (string.IsNullOrWhiteSpace(fullname) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(method)) {
                if (!abortedToken.IsCancellationRequested) {
                    client.InvokeResult(invocationId, new { StatusCodes.Status400BadRequest });
                }
                return;
            }
            try {
                _logger.LogInformation($"Invoke: {connectionId} {fullname} {method}");
                var link = (IDirectLink<ViewModel>)_provider.GetService(_componentsService.GetType(name));
                if (method.StartsWith(MethodPrefix)) {
                    method = method.Substring(MethodPrefix.Length);
                    args = new object[] { Context }.Concat(args ?? Array.Empty<object>()).ToArray();
                }
                var response = await OnInvoke(link, method, args);
                if (response.HasResult) {
                    if (!abortedToken.IsCancellationRequested) {
                        client.InvokeResult(invocationId, new { response.StatusCode, response.Result });
                    }
                    return;
                }

                if (!abortedToken.IsCancellationRequested) {
                    client.InvokeResult(invocationId, new { response.StatusCode });
                }
            }
            catch (Exception ex) {
                _logger.LogError(0, ex, $"Exception on Invoke: {connectionId} {fullname} {method}");
                if (!abortedToken.IsCancellationRequested) {
                    client.InvokeResult(invocationId, new { StatusCode = StatusCodes.Status500InternalServerError });
                }
            }
        }

        private bool IsLinkRequestLimited()
        {
            if (_requestsPerSecond <= 0) {
                return false;
            }
            if (LinkCount.TryGetValue(Context.ConnectionId, out var count)) {
                if (count > _maxLinkCount) {
                    return true;
                }
            }
            LinkCount.AddOrUpdate(Context.ConnectionId, count + 1, cnt => cnt + 1);
            return false;
        }

        private async Task<bool> IsRequestLimited()
        {
            if (_requestsPerSecond <= 0) {
                return false;
            }
            if (RequestNextTime.TryGetValue(Context.ConnectionId, out var nextTime)) {
                var timespan = nextTime - DateTime.Now.Ticks;
                if (timespan > 0) {
                    var requestCount = timespan / _requestDelayTicks;
                    if (requestCount >= _queuedRequestCount + _requestsPerSecond) {
                        return true;
                    }
                    RequestNextTime.AddOrUpdate(Context.ConnectionId, nextTime + _requestDelayTicks, time => time + _requestDelayTicks);
                    if (requestCount >= _requestsPerSecond - 1) {
                        try {
                            await Task.Delay((int)(timespan / 10000), TokenSources[Context.ConnectionId].Token);
                        }
                        catch (TaskCanceledException) {
                            return true;
                        }
                    }
                    return false;
                }
            }
            RequestNextTime.AddOrUpdate(Context.ConnectionId, DateTime.Now.Ticks + _requestDelayTicks, time => Math.Max(time, DateTime.Now.Ticks) + _requestDelayTicks);
            return false;
        }

        private async Task<InvokeResponse> OnInvoke(object link, string method, object[] args)
        {
            var user = _authenticationService != null ? await _authenticationService.GetPrincipalAsync(Context.ConnectionId) : null;
            var type = link.GetType();
            var authorized = await AuthorizationHelper.AuthorizeAsync(type, user, _authorizationService);
            if (!authorized) {
                return new InvokeResponse(StatusCodes.Status403Forbidden);
            }
            var name = _componentsService.GetComponentName(type);
            var methodInfo = _componentsService.GetMethodInfo(name, $"{method}{ComponentsService.Delimiter}{args?.Length ?? 0}");
            if (methodInfo == null) {
                return new InvokeResponse(StatusCodes.Status404NotFound);
            }
            authorized = await AuthorizationHelper.AuthorizeAsync(methodInfo, user, _authorizationService);
            if (!authorized) {
                return new InvokeResponse(StatusCodes.Status403Forbidden);
            }
            var returnType = methodInfo.ReturnType;
            var parameters = methodInfo.GetParameters();
            if (args != null) {
                for (int j = 0; j < args.Length; ++j) {
                    args[j] = Convert(args[j], parameters[j].ParameterType);
                }
            }
            if (returnType == typeof(void)) {
                // void Method(T1,...,Tn)
                methodInfo.Invoke(link, args);
                return new InvokeResponse(StatusCodes.Status200OK);
            }
            if (returnType == typeof(Task)) {
                // Task Method(T1,...,Tn) => {...}
                var task = (Task)methodInfo.Invoke(link, args);
                await task;
                return new InvokeResponse(StatusCodes.Status200OK);
            }
            if (returnType.GetTypeInfo().BaseType == typeof(Task)) {
                // Task<TResult> Method(T1,...,Tn)
                dynamic task = methodInfo.Invoke(link, args);
                await task;
                return new InvokeResponse(StatusCodes.Status200OK, task.Result, true);
            }
            // TResult Method(T1,...,Tn)
            var result = methodInfo.Invoke(link, args);
            return new InvokeResponse(StatusCodes.Status200OK, result, true);
        }

        private object Convert(object parameter, Type type)
        {
            if (parameter != null) {
                if (type.GetTypeInfo().IsClass) {
                    if (type != typeof(string) && type != parameter.GetType()) {
                        parameter = JsonConvert.DeserializeObject(parameter.ToString(), type);
                    }
                }
                else {
                    var typeConverter = TypeDescriptor.GetConverter(type);
                    if (typeConverter != null) {
                        parameter = typeConverter.ConvertFromString(parameter.ToString());
                    }
                }
            }
            return parameter;
        }
    }
}