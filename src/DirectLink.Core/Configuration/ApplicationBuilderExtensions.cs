// Copyright (c) 2018 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DirectLinkCore
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDirectLink(this IApplicationBuilder builder, string pathBase)
        {
            if (!string.IsNullOrWhiteSpace(pathBase)) {
                var options = builder.ApplicationServices.GetRequiredService<DirectLinkOptionsProvider>().Options;
                options.PathBases.Add(pathBase);
            }
            return builder.UseMiddleware<DirectLinkMiddleware>();
        }

        public static IApplicationBuilder UseDirectLink(this IApplicationBuilder builder, Action<ConfigurationComponents> configureComponents = null)
        {
            return builder
                .PrepareDirectLink(configureComponents)
                .UseMiddleware<DirectLinkMiddleware>();
        }

        public static IApplicationBuilder PrepareDirectLink(this IApplicationBuilder builder, Action<ConfigurationComponents> configureComponents = null)
        {
            if (configureComponents != null) {
                builder.MapComponents(configureComponents);
            }

            builder.WarmupNodeServices();
            return builder
                .UseMiddleware<Status503Middleware>()
                .UseStaticFiles()
                .UseMiddleware<FaviconMiddleware>()
                .UseWebSockets()
                .UseSignalR(routes => { routes.MapHub<DirectLinkHub>("/directlink"); });
        }

        public static IApplicationBuilder MapComponents(this IApplicationBuilder builder, Action<ConfigurationComponents> configureComponents)
        {
            if (configureComponents != null) {
                var componentsService = builder.ApplicationServices.GetRequiredService<IComponentsService>();
                var components = new ConfigurationComponents();
                configureComponents(components);
                componentsService.Configure(components);
            }
            return builder;
        }

        public static IApplicationBuilder WarmupNodeServices(this IApplicationBuilder builder)
        {
            builder.ApplicationServices.GetRequiredService<HotNodeServicesFactory>();
            return builder;
        }
    }
}