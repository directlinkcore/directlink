// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace DirectLinkCore
{
    public static class RoutingExtensions
    {
        public static ICollection<Route> MapDefaultRoute(this ICollection<Route> routes, string component, string template, object args = null)
        {
            routes.Add(new Route(component, template, args, true));
            return routes;
        }

        public static ICollection<Route> MapRoute(this ICollection<Route> routes, string component, string template, object args = null)
        {
            routes.Add(new Route(component, template, args, false));
            return routes;
        }

        public static ICollection<Route> MapRoute(this ICollection<Route> routes, Type type, string template, object args = null)
        {
            routes.Add(new Route(type, template, args, false));
            return routes;
        }

        public static ICollection<Route> MapDefaultRoute<T>(this ICollection<Route> routes, string template, object args = null) where T : IDirectLink<ViewModel>
        {
            routes.Add(new Route(typeof(T), template, args, true));
            return routes;
        }

        public static ICollection<Route> MapRoute<T>(this ICollection<Route> routes, string template, object args = null) where T : IDirectLink<ViewModel>
        {
            routes.Add(new Route(typeof(T), template, args, false));
            return routes;
        }
    }
}