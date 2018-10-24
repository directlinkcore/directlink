// Copyright (c) 2018 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace DirectLinkCore
{
    public class DirectLinkMiddleware
    {
        private readonly string _moduleName = "node_modules/directlink-react/dist/server.js";
        private readonly string _wwwroot = "../../../wwwroot";

        public DirectLinkMiddleware(RequestDelegate next)
        {
        }

        public async Task Invoke(HttpContext httpContext, DirectLinkContext context, IRoutingService routingService, IHotNodeService nodeService)
        {
            var request = httpContext.Request;
            context.HttpContext = httpContext;
            context.Path = request.PathBase.Value + request.Path.Value + request.QueryString;

            var response = await routingService.GetResponseAsync(context);
            httpContext.Response.StatusCode = response.StatusCode;
            httpContext.Response.Headers["Cache-Control"] = "no-store";
            if (response.Data != null) {
                var dataJson = JsonConvert.SerializeObject(response.Data);
                foreach (var style in response.Data.Styles) {
                    context.Tags.Head.AddLink(style.Value);
                }
                foreach (var script in response.Data.Scripts) {
                    context.Tags.Head.AddScript(script.Value);
                }
                context.Data.Add("title", context.Title);
                context.Data.Add("metaTags", context.Tags.GetMetaTags());
                context.Data.Add("headTags", context.Tags.Head);
                context.Data.Add("contentTags", context.Tags.Content);
                context.Data.Add("bodyTags", context.Tags.Body);
                context.Data.Add("serverTime", DateTime.Now.Ticks.ToString());

                var contextDataJson = JsonConvert.SerializeObject(context.Data);
                var cookiesJson = JsonConvert.SerializeObject(httpContext.Request.Cookies.ToDictionary(k => k.Key, v => v.Value));
                var content = $@"{{""moduleName"":""{_moduleName}"",""args"":[""{_wwwroot}"",{dataJson},{contextDataJson},{cookiesJson}]}}";
                var payload = new StringContent(content, Encoding.UTF8, "application/json");
                var result = await nodeService.InvokeExportAsync<string>(CancellationToken.None, payload);
                await httpContext.Response.WriteAsync(result);
                return;
            }
            if (response.Message != null) {
                await httpContext.Response.WriteAsync(response.Message);
            }
        }
    }
}