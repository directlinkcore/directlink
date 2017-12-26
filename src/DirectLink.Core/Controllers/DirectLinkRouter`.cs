// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace DirectLinkCore
{
    public class DirectLinkRouter<T> : IDirectLinkRouter<T>, IInternalDirectLink<T> where T : RouterViewModel
    {
        DirectLinkType IInternalDirectLink<T>.LinkType { get; } = DirectLinkType.Router;

        Type IInternalDirectLink<T>.ViewModelType { get; } = typeof(T);

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