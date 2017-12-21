// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace DirectLinkCore
{
    public class DirectLinkController<T> : DirectLinkDispatcher<T>, IDirectLinkController<T>,
        IInternalDirectLink<T>, IInternalDirectLinkRouter<T> where T : RouterViewModel
    {
        private static ICollection<Route> _routes;

        DirectLinkType IInternalDirectLink<T>.LinkType { get; } = DirectLinkType.Controller;

        ICollection<Route> IInternalDirectLinkRouter<T>.Routes => _routes;

        public void MapRoutes(Action<ICollection<Route>> setupAction)
        {
            if (_routes == null) {
                _routes = new List<Route>();
                setupAction(_routes);
                _routes = _routes
                    .OrderByDescending(p => p.Priority)
                    .ThenByDescending(p => p.Template.Length).ToList();
            }
        }
    }
}