// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace DirectLinkCore
{
    public interface IRoutingService
    {
        Task<DataResponse> GetResponseAsync(DirectLinkContext context);
    }
}