// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;

namespace DirectLinkCore
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DirectLinkAttribute : Attribute
    {
        public string Component { get; }

        public string BaseName { get; }

        public string Script { get; }

        public string Style { get; }

        public DirectLinkAttribute(string component = null, string basename = null, string script = null, string style = null)
        {
            this.Component = component;
            this.BaseName = Path.GetFileNameWithoutExtension(basename);
            this.Script = script;
            this.Style = style;
        }
    }
}