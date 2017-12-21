// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace DirectLinkCore
{
    internal interface IInternalDirectLink<out T> where T : ViewModel
    {
        DirectLinkType LinkType { get; }

        Type ViewModelType { get; }
    }
}