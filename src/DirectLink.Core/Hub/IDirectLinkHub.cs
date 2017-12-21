// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace DirectLinkCore
{
    public interface IDirectLinkHub
    {
        void SetState(string fullname, object data);

        void Invoke(string fullname, string method, object[] args);

        void DataResponse(DataResponse dataResponse);

        void InvokeResult(Guid invocationId, object result);

        void AssetsUpdate();
    }
}