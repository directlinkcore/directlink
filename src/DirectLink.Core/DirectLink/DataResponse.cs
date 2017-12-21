// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace DirectLinkCore
{
    public class DataResponse
    {
        public int StatusCode { get; }

        public IDirectLinkData Data { get; }

        public string Message { get; }

        public DataResponse(int statusCode, IDirectLinkData data = null, string message = null)
        {
            this.StatusCode = statusCode;
            this.Data = data;
            this.Message = message;
        }
    }
}