// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace DirectLinkCore
{
    public class DirectLinkController<T> : DirectLinkDispatcher<T>, IDirectLinkController<T>,
        IInternalDirectLink<T> where T : RouterViewModel
    {
        DirectLinkType IInternalDirectLink<T>.LinkType { get; } = DirectLinkType.Controller;

        public void MapRoutes(Action<ICollection<Route>> setupAction)
        {
            var routes = Routes.Get(this.GetType());
            if (routes == null) {
                routes = new List<Route>();
                setupAction(routes);
                Routes.Set(this.GetType(), routes);
            }
        }
    }
}