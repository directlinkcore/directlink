// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace DirectLinkCore
{
    internal class InvokeResponse
    {
        public int StatusCode { get; }

        public object Result { get; }

        public bool HasResult { get; }

        public InvokeResponse(int statusCode, object result = null, bool hasResult = false)
        {
            this.StatusCode = statusCode;
            this.Result = result;
            this.HasResult = hasResult;
        }
    }
}