// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DirectLinkCore
{
    public class FaviconMiddleware
    {
        private readonly byte[] faviconBytes;
        private readonly RequestDelegate _next;

        public FaviconMiddleware(RequestDelegate next)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"DirectLinkCore.Content.favicon.ico";

            using (var stream = assembly.GetManifestResourceStream(resourceName)) {
                faviconBytes = new byte[stream.Length];
                stream.Read(faviconBytes, 0, faviconBytes.Length);
            }

            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/favicon.ico") {
                await context.Response.Body.WriteAsync(faviconBytes, 0, faviconBytes.Length);
                return;
            }

            await _next(context);
        }
    }
}