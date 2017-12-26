// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace DirectLinkCore
{
    internal class RoutingService : IRoutingService
    {
        private readonly IServiceProvider _provider;
        private readonly IAuthorizationService _authorizationService;
        private readonly IComponentsService _componentsService;
        private readonly Type _entryType;
        private readonly string _notFoundPath;
        private readonly string _notAuthPath;
        private readonly string _errorPath;
        private readonly List<string> _pathBases;

        public RoutingService(
            IServiceProvider provider,
            IAuthorizationService authorizationService,
            IComponentsService componentsService,
            DirectLinkOptionsProvider optionsProvider
        )
        {
            _provider = provider;
            _authorizationService = authorizationService;
            _componentsService = componentsService;

            var options = optionsProvider.Options;
            _entryType = options.EntryType;
            _notFoundPath = options.NotFoundPath;
            _notAuthPath = options.NotAuthPath;
            _errorPath = options.ErrorPath;
            _pathBases = new List<string>(options.PathBases);
        }

        public async Task<DataResponse> GetResponseAsync(DirectLinkContext context)
        {
            try {
                var data = await GetDataAsync(context.Path, context);
                if (_errorPath != null && context.Path.StartsWith(_errorPath)) {
                    var exceptionFeature = context.HttpContext.Features.Get<IExceptionHandlerPathFeature>();
                    if (exceptionFeature != null) {
                        Exception exceptionThatOccurred = exceptionFeature.Error;
                        data.Message = exceptionThatOccurred.Message;
                        return new DataResponse(StatusCodes.Status500InternalServerError, data);
                    }
                }
                if (!data.IsAuthorized || !data.IsRouted) {
                    var _path = !data.IsAuthorized ? _notAuthPath : _notFoundPath;
                    var statusCode = !data.IsAuthorized ? StatusCodes.Status403Forbidden : StatusCodes.Status404NotFound;
                    if (_path == null) {
                        return new DataResponse(statusCode);
                    }
                    var isAuthorized = data.IsAuthorized;
                    var routed = data.IsRouted;
                    data = await GetDataAsync(_path, context);
                    data.IsAuthorized = isAuthorized;
                    data.IsRouted = routed;
                    return new DataResponse(statusCode, data);
                }
                return new DataResponse(StatusCodes.Status200OK, data);
            }
            catch (Exception ex) {
                if (_errorPath == null) {
                    return new DataResponse(StatusCodes.Status500InternalServerError, null, ex.Message);
                }
                try {
                    var data = await GetDataAsync(_errorPath, context);
                    data.Message = ex.Message;
                    return new DataResponse(StatusCodes.Status500InternalServerError, data);
                }
                catch (Exception exception) {
                    return new DataResponse(StatusCodes.Status500InternalServerError, null, exception.Message);
                }
            }
        }

        private async Task<DirectLinkData> GetDataAsync(string path, DirectLinkContext context)
        {
            var name = _componentsService.GetComponentName(_entryType);
            var fullname = name;
            var data = new DirectLinkData(name, _provider, _authorizationService, _componentsService);

            var authorized = await AuthorizationHelper.AuthorizeAsync(_entryType, context.User, _authorizationService);
            if (!authorized) {
                data.IsAuthorized = false;
                data.Title = context.Title;
                return data;
            }

            var link = (IInternalDirectLink<ViewModel>)_provider.GetService(_entryType);
            var args = Array.Empty<object>();
            var viewModel = data.CreateViewModel(link, args, name);

            data.AddAssets(name);
            await data.AddStateAsync(name, fullname, viewModel, context.User);

            var idx = path.IndexOf('#');
            if (idx >= 0) {
                path = path.Substring(0, idx);
            }
            var routedPath = "";
            context.PathBase = "";
            foreach (var pathBase in _pathBases) {
                if (path.StartsWith(pathBase)) {
                    context.PathBase = pathBase;
                    routedPath = pathBase;
                    path = path.Substring(pathBase.Length, path.Length - pathBase.Length);
                }
            }

            var routes = Routes.Get(_entryType);
            if (routes == null) {
                data.IsRouted = true;
                data.Title = context.Title;
                return data;
            }

            var restPath = path;
            while (true) {
                args = null;
                Route nextRoute = null;
                foreach (var route in routes) {
                    if (route.Regex == null) {
                        if (restPath.StartsWith(route.Template) || (restPath == "" || restPath == "/") && route.Default) {
                            nextRoute = route;
                            args = nextRoute.Args;
                            if (route.Default) {
                                routedPath += restPath;
                                restPath = "";
                            }
                            else {
                                routedPath += route.Template;
                                restPath = restPath.Substring(route.Template.Length);
                            }
                            break;
                        }
                    }
                    else {
                        var m = route.Regex.Match(restPath);
                        if (!m.Success) {
                            continue;
                        }
                        if (m.Groups.Count - 1 != route.Parameters.Count) {
                            break;
                        }
                        var values = m.Groups.Skip(1).Select(p => p.Value).ToArray();
                        if (RouteTypesHelper.TryGetArgs(route.Parameters, values, ref args)) {
                            if (route.Args != null) {
                                args = args.Concat(route.Args).ToArray();
                            }
                            nextRoute = route;
                            routedPath += m.Groups[0];
                            restPath = restPath.Substring(m.Value.Length);
                            break;
                        }
                    }
                }

                if (nextRoute == null) {
                    break;
                }

                var routerViewModel = (RouterViewModel)viewModel;
                routerViewModel.OnRouted(nextRoute);

                name = nextRoute.Type != null
                    ? _componentsService.GetComponentName(nextRoute.Type)
                    : nextRoute.Name;
                fullname += $".{name}";
                routerViewModel.Component = new RouteComponent(name, fullname);
                data.AddAssets(name);

                if (nextRoute.Type != null) {
                    authorized = await AuthorizationHelper.AuthorizeAsync(nextRoute.Type, context.User, _authorizationService);
                    if (!authorized) {
                        data.IsAuthorized = false;
                        break;
                    }

                    link = (IInternalDirectLink<ViewModel>)_provider.GetService(nextRoute.Type);
                    args = args ?? Array.Empty<object>();
                    viewModel = data.CreateViewModel(link, args, name);

                    var succeeded = await data.AddStateAsync(name, fullname, viewModel, context.User);
                    if (!succeeded) {
                        data.IsAuthorized = false;
                        break;
                    }

                    routes = Routes.Get(nextRoute.Type);
                    if (routes == null) {
                        if (restPath == "/") {
                            routedPath += "/";
                        }
                        break;
                    }
                }
                else {
                    if (nextRoute.State != null) {
                        data.States.Add(fullname, nextRoute.State);
                    }
                    else {
                        if (args == null) {
                            data.States.Add(fullname, new Dictionary<string, object>{ { "args", null }});
                        }
                        else {
                            var values = new Dictionary<string, object>();
                            for (int i = 0; i < nextRoute.Parameters.Count; ++i) {
                                values.Add(nextRoute.Parameters[i].Name, args[i]);
                            }
                            data.States.Add(fullname, new Dictionary<string, object> { { "args", values } });
                        }
                    }
                    if (restPath == "/") {
                        routedPath += "/";
                    }
                    break;
                }
            }

            data.IsRouted = routedPath == context.PathBase + path;
            data.Title = context.Title;
            return data;
        }
    }
}