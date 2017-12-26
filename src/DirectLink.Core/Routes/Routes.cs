// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DirectLinkCore
{
    public static class Routes
    {
        private static readonly ConcurrentDictionary<Type, ICollection<Route>> _routes = new ConcurrentDictionary<Type, ICollection<Route>>();
        private static Dictionary<string, List<RoutePathInfo>> _routePaths = new Dictionary<string, List<RoutePathInfo>>();

        public static void For<T>(Action<ICollection<Route>> setupAction) where T : IDirectLinkRouter<RouterViewModel>
        {
            var routes = new List<Route>();
            setupAction(routes);
            Routes.Set<T>(routes);
        }

        public static void Set<T>(ICollection<Route> routes) where T : IDirectLinkRouter<RouterViewModel>
        {
            Routes.Set(typeof(T), routes);
        }

        internal static void Set(Type type, ICollection<Route> routes)
        {
            routes = routes
                .OrderByDescending(p => p.Priority)
                .ThenByDescending(p => p.Template.Length).ToList();
            _routes[type] = routes;
            foreach (var route in routes) {
                _routePaths.TryGetValue(type.Name, out var parentPaths);
                if (parentPaths != null) {
                    foreach (var routePathInfo in parentPaths) {
                        AddOrUpdatePathInfo(route.Name, routePathInfo.AddRoute(route));
                    }
                }
                else {
                    AddOrUpdatePathInfo(route.Name, new RoutePathInfo().AddRoute(route));
                }
            }
        }

        public static ICollection<Route> Get<T>() where T : IDirectLinkRouter<RouterViewModel>
        {
            _routes.TryGetValue(typeof(T), out var routes);
            return routes;
        }

        public static ICollection<Route> Get(Type type)
        {
            _routes.TryGetValue(type, out var routes);
            return routes;
        }

        public static string GetLink<T>(params object[] args)
        {
            return GetLink(typeof(T).Name, args);
        }

        public static string GetLink(string name, params object[] args)
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
                            else {
                                success = false;
                                continue;
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

        private static void AddOrUpdatePathInfo(string name, RoutePathInfo routePathInfo)
        {
            if (_routePaths.ContainsKey(name)) {
                _routePaths[name].Add(routePathInfo);
            }
            else {
                _routePaths.Add(name, new List<RoutePathInfo> { routePathInfo });
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