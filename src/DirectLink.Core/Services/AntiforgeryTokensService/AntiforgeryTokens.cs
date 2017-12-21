// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace DirectLinkCore
{
    public class AntiforgeryTokens
    {
        public string CookieName { get; }

        public string CookieToken { get; }

        public string RequestToken { get; }

        public AntiforgeryTokens(string cookieName, string cookieToken, string requestToken)
        {
            this.RequestToken = requestToken;
            this.CookieName = cookieName;
            this.CookieToken = cookieToken;
        }
    }
}