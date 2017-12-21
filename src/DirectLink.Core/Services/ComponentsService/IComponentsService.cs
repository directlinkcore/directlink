// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace DirectLinkCore
{
    public interface IComponentsService
    {
        string GetComponentName(Type type);

        Type GetType(string component);

        MethodInfo GetMethodInfo(string component, string method);

        IList<string> GetMethodNames(string component);

        string GetScriptPath(string component);

        string GetStylePath(string component);

        string GetAssetPath(string filename);

        void Configure(ConfigurationComponents components);
    }
}