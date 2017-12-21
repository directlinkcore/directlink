// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace DirectLinkCore
{
    internal interface IInternalDirectLinkRouter<out T> where T : RouterViewModel
    {
        ICollection<Route> Routes { get; }
    }
}