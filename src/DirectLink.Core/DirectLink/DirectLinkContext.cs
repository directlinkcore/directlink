// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace DirectLinkCore
{
    public class DirectLinkContext
    {
        private readonly ILinkService _linkService;

        public HubCallerContext HubCallerContext { get; internal set; }
        public HttpContext HttpContext { get; internal set; }
        public ClaimsPrincipal User { get => this.HttpContext.User; }

        public string Title { get => this.Tags.Title; set => this.Tags.Title = value; }
        public TemplateTags Tags { get; internal set; }
        public IDictionary<string, object> Data { get; internal set; }

        public string PathBase { get; internal set; }
        public string Path { get; internal set; }
        public string HubReferer { get; internal set; }

        public string GetLink<T>(params object[] args) => this.PathBase + _linkService.GetLink<T>(args);
        public string GetLink(string name, params object[] args) => this.PathBase + _linkService.GetLink(name, args);

        public DirectLinkContext(ILinkService linkService) => _linkService = linkService;
    }
}