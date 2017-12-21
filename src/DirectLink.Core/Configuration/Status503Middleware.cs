// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DirectLinkCore
{
    public class Status503Middleware
    {
        private readonly RequestDelegate _next;
        private readonly IApplicationLifetime _applicationLifetime;

        public Status503Middleware(RequestDelegate next, IApplicationLifetime applicationLifetime)
        {
            _next = next;
            _applicationLifetime = applicationLifetime;
        }

        public async Task Invoke(HttpContext context)

        {
            if (_applicationLifetime.ApplicationStopping.IsCancellationRequested || _applicationLifetime.ApplicationStopped.IsCancellationRequested) {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                return;
            }
            await _next(context);
        }
    }
}