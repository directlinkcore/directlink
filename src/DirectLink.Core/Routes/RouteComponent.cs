// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace DirectLinkCore
{
    public class RouteComponent
    {
        public string Name { get; }

        public string FullName { get; }

        public RouteComponent(string name, string fullname = null)
        {
            this.Name = name;
            this.FullName = fullname ?? name;
        }
    }
}