// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace DirectLinkCore
{
    public class AntiforgeryTokensService : IAntiforgeryTokensService
    {
        private readonly IAntiforgery _antiforgery;

        public AntiforgeryTokensService(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public AntiforgeryTokens GetTokens(IPrincipal user)
        {
            var httpContext = new DefaultHttpContext { User = user as ClaimsPrincipal };
            var tokens = _antiforgery.GetAndStoreTokens(httpContext);
            var cookieName = httpContext.Response.Headers["Set-Cookie"].ToString().Split('=', ';')[0];
            return new AntiforgeryTokens(cookieName, tokens.CookieToken, tokens.RequestToken);
        }
    }
}