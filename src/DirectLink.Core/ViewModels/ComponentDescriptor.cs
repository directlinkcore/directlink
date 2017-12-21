// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DirectLinkCore
{
    public class ComponentDescriptor
    {
        public Type Type { get; }

        public string Name { get; }

        public string Id { get; }

        public object[] Args { get; }

        public Dictionary<string, object> State { get; }

        public ComponentDescriptor(Type type, string id = null, object args = null)
        {
            this.Type = type;
            this.Id = id;
            this.Args = args?.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(prop => prop.GetValue(args, null))
                .ToArray();
        }

        public ComponentDescriptor(string name, string id = null, object state = null)
        {
            this.Name = name;
            this.Id = id;
            this.State = state?.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(state, null));
        }
    }
}