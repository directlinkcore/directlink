// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace DirectLinkCore
{
    public interface IDirectLinkRouter<out T> : IDirectLink<T> where T : RouterViewModel
    {
        void MapRoutes(Action<ICollection<Route>> setupAction);
    }
}