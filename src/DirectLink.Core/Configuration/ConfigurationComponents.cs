// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace DirectLinkCore
{
    public class ConfigurationComponents
    {
        private readonly ICollection<ConfigurationComponent> _components = new List<ConfigurationComponent>();

        public ConfigurationComponents Map(string component = null, string basename = null, string script = null, string style = null)
        {
            _components.Add(new ConfigurationComponent(null, component, basename, script, style));
            return this;
        }

        public ConfigurationComponents Map<T>(string component = null, string basename = null, string script = null, string style = null)
        {
            _components.Add(new ConfigurationComponent(typeof(T), component, basename, script, style));
            return this;
        }

        internal ICollection<ConfigurationComponent> GetComponents()
        {
            return _components;
        }

        internal class ConfigurationComponent
        {
            public Type Type { get; }
            public string Name { get; }
            public string BaseName { get; }
            public string Script { get; }
            public string Style { get; }

            public ConfigurationComponent(Type type, string component, string basename, string script, string style) =>
                (Type, Name, BaseName, Script, Style) = (type, component, basename, script, style);
        }
    }
}