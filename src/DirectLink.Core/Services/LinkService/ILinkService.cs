// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace DirectLinkCore
{
    public interface ILinkService
    {
        string GetLink<T>(params object[] args);

        string GetLink(string name, params object[] args);
    }
}