// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace DirectLinkCore
{
    internal class LinkService : ILinkService
    {
        private readonly IServiceProvider _provider;
        private Dictionary<string, List<RoutePathInfo>> _routePaths = new Dictionary<string, List<RoutePathInfo>>();

        public string GetLink<T>(params object[] args)
        {
            return GetLink(typeof(T).Name, args);
        }

        public string GetLink(string name, params object[] args)
        {
            _routePaths.TryGetValue(name, out var paths);
            if (paths == null) {
                return null;
            }
            foreach (var routePathInfo in paths) {
                if (routePathInfo.RouteParameters.Count == args.Length) {
                    var routesArgs = args.Select(arg => arg.ToRouteParameters().ToList()).ToList();
                    var success = true;
                    var url = "";
                    var idx = 0;

                    foreach (var route in routePathInfo.RoutePath) {
                        var template = route.Template;
                        if ((route.Parameters?.Count ?? 0) > 0) {
                            var routeArgs = routesArgs[idx];
                            if (routeArgs.Count == route.Parameters.Count) {
                                for (int i = 0; i < routeArgs.Count && success; ++i) {
                                    var parameter = route.Parameters[i];
                                    if (routeArgs[i].Name != parameter.Name || routeArgs[i].Type != parameter.Type) {
                                        success = false;
                                    }
                                    if (parameter.Format == null) {
                                        template = template.Replace(parameter.Match, routeArgs[i].Value.ToString());
                                    }
                                    else {
                                        template = template.Replace(parameter.Match, ((dynamic)routeArgs[i].Value).ToString(parameter.Format, CultureInfo.InvariantCulture));
                                    }
                                }
                            }
                            idx++;
                        }
                        url += template;
                    }
                    if (success) {
                        return url;
                    }
                }
            }
            return null;
        }

        public LinkService(IServiceScopeFactory scopeFactory, DirectLinkOptionsProvider optionsProvider)
        {
            using (var scope = scopeFactory.CreateScope()) {
                _provider = scope.ServiceProvider;
                var app = _provider.GetService(optionsProvider.Options.EntryType) as IInternalDirectLinkRouter<RouterViewModel>;
                if (app?.Routes != null) {
                    VisitRoutes(app.Routes, new RoutePathInfo());
                }
            }
        }

        private void VisitRoutes(ICollection<Route> routes, RoutePathInfo routePathInfo)
        {
            foreach (var route in routes) {
                if (route.Type != null) {
                    var router = _provider.GetService(route.Type) as IInternalDirectLinkRouter<RouterViewModel>;
                    if (router?.Routes != null) {
                        VisitRoutes(router.Routes, routePathInfo.AddRoute(route));
                    }
                }
                AddOrUpdatePathInfo(routePathInfo.AddRoute(route), route);
            }
        }

        private void AddOrUpdatePathInfo(RoutePathInfo routePathInfo, Route route)
        {
            if (_routePaths.ContainsKey(route.Name)) {
                _routePaths[route.Name].Add(routePathInfo);
            }
            else {
                _routePaths.Add(route.Name, new List<RoutePathInfo> { routePathInfo });
            }
        }

        private class RoutePathInfo
        {
            public ICollection<Route> RoutePath = new List<Route>();

            public ICollection<int> RouteParameters = new List<int>();

            public RoutePathInfo AddRoute(Route route)
            {
                var routePathInfo = new RoutePathInfo {
                    RoutePath = new List<Route>(this.RoutePath),
                    RouteParameters = new List<int>(this.RouteParameters)
                };
                routePathInfo.RoutePath.Add(route);
                if (route.Parameters != null) {
                    routePathInfo.RouteParameters.Add(route.Parameters.Count);
                }
                return routePathInfo;
            }
        }
    }
}