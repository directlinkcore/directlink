// Copyright (c) 2018 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace DirectLinkCore
{
    public interface IDirectLinkHub
    {
        Task SetState(string fullname, object data);

        Task Invoke(string fullname, string method, object[] args);

        Task DataResponse(DataResponse dataResponse);

        Task InvokeResult(Guid invocationId, object result);

        Task AssetsUpdate();
    }
}