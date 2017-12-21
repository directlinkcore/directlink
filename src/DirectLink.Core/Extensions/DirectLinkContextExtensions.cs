// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace DirectLinkCore
{
    public static class DirectLinkContextExtensions
    {
        public static string GetUserAgent(this DirectLinkContext context)
        {
            if (context.HttpContext.Request.Headers.TryGetValue("User-Agent", out var userAgent)) {
                return userAgent;
            }
            return null;
        }

        public static string GetReferer(this DirectLinkContext context)
        {
            var request = context.HttpContext.Request;
            if (context.HubCallerContext == null) {
                if (request.Headers.TryGetValue("Referer", out var referer)) {
                    return referer;
                }
            }
            else {
                if (context.HubReferer != null) {
                    return $"{request.Scheme}://{request.Host}{context.HubReferer}";
                }
            }
            return null;
        }
    }
}