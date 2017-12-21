// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace DirectLinkCore
{
    public class DirectLink<T> : IDirectLink<T>, IInternalDirectLink<T> where T : ViewModel
    {
        DirectLinkType IInternalDirectLink<T>.LinkType { get; } = DirectLinkType.Link;

        Type IInternalDirectLink<T>.ViewModelType { get; } = typeof(T);
    }
}