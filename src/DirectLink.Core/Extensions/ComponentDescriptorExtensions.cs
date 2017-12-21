// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace DirectLinkCore
{
    public static class ComponentDescriptorExtensions
    {
        public static ICollection<ComponentDescriptor> Add<T>(this ICollection<ComponentDescriptor> components, string id = null, object args = null) where T : IDirectLink<ViewModel>
        {
            components.Add(new ComponentDescriptor(typeof(T), id, args));
            return components;
        }

        public static ICollection<ComponentDescriptor> Add(this ICollection<ComponentDescriptor> components, string name, string id = null, object args = null)
        {
            components.Add(new ComponentDescriptor(name, id, args));
            return components;
        }
    }
}