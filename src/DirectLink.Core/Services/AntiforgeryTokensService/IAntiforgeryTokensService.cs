// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Principal;

namespace DirectLinkCore
{
    public interface IAntiforgeryTokensService
    {
        AntiforgeryTokens GetTokens(IPrincipal user);
    }
}