// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DirectLinkCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDirectLink<T>(this IServiceCollection services,
            Action<TemplateTags> tagsSetupAction = null,
            IEnumerable<Assembly> assemblies = null)
        {
            return services.AddDirectLink(options => options.EntryType = typeof(T), tagsSetupAction, assemblies);
        }

        public static IServiceCollection AddDirectLink<T>(this IServiceCollection services,
            Action<DirectLinkOptions> optionsSetupAction,
            Action<TemplateTags> tagsSetupAction = null,
            IEnumerable<Assembly> assemblies = null)
        {
            return services.AddDirectLink(options => {
                options.EntryType = typeof(T);
                optionsSetupAction(options);
            }, tagsSetupAction, assemblies);
        }

        public static IServiceCollection AddDirectLink(this IServiceCollection services,
            Action<DirectLinkOptions> optionsSetupAction,
            Action<TemplateTags> tagsSetupAction = null,
            IEnumerable<Assembly> assemblies = null)
        {
            services.AddSignalR(options => {
                options.JsonSerializerSettings = new JsonSerializerSettings {
                    ContractResolver = new DefaultContractResolver()
                };
            });

            services.AddSingleton(typeof(DirectLinkOptionsProvider), provider => {
                var options = GetDefaultOptions();
                options.NodeServicesOptions = new NodeServicesOptions(provider) { WatchFileExtensions = new[] { ".js" } };
                options.DirectLinkOutputLogger = provider.GetService<ILoggerFactory>().CreateLogger("DirectLink.Core");

                optionsSetupAction.Invoke(options);

                if (options.EntryType == null) {
                    throw new InvalidOperationException("EntryType is null. You need to specify EntryType: services.AddDirectLink(options => {{ options.EntryType = typeof(...); }})");
                }

                if (!typeof(IDirectLink<ViewModel>).IsAssignableFrom(options.EntryType)) {
                    throw new InvalidOperationException("EntryType is not valid. You need to specify correct EntryType");
                }

                return new DirectLinkOptionsProvider(options);
            });

            services.AddSingleton<HotNodeServicesFactory>();
            services.AddScoped(typeof(IHotNodeService), provider => provider.GetService<HotNodeServicesFactory>().GetHotNodeService());

            if (assemblies == null) {
                assemblies = new[] { Assembly.GetEntryAssembly() };
            }
            var types = assemblies.SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IDirectLink<ViewModel>).IsAssignableFrom(type))
                .ToArray();
            foreach (var type in types) {
                services.AddScoped(type);
            }
            services.AddSingleton(typeof(IComponentsService), provider =>
                new ComponentsService(types, provider.GetService<ILogger<ComponentsService>>(), provider.GetService<DirectLinkOptionsProvider>())
            );

            services.AddScoped<IRoutingService, RoutingService>();
            services.AddSingleton<ILinkService, LinkService>();

            var tags = new TemplateTags();
            tagsSetupAction?.Invoke(tags);
            services.AddScoped(typeof(DirectLinkContext), provider => new DirectLinkContext(provider.GetService<ILinkService>()) {
                    Tags = new TemplateTags(tags),
                    Data = new Dictionary<string, object>()
                }
            );

            return services;
        }

        private static DirectLinkOptions GetDefaultOptions()
        {
            return new DirectLinkOptions {
                MemoryWatcherInterval = TimeSpan.FromSeconds(20),
                FileWatcherDelay = TimeSpan.FromSeconds(1),
                NodeServiceDisposingDelay = TimeSpan.FromSeconds(10),

                WarmupPath = "node_modules/directlink-react/dist/warmup.js",
                IndexTemplatePath = "node_modules/directlink-react/dist/index.template.html",
                AssetsPath = "assets.json",

                ComponentNameConverter = type => type.Name,
                FileNameConverter = name => name.ToLower(),
                ScriptExtension = ".js",
                StyleExtension = ".css",

                NodeInstanceCount = Environment.ProcessorCount,
                NodeInstanceMemoryLimit = 256 * 1024 * 1024,

                RequestsPerSecond = 10,
                QueuedRequestCount = 20,
                MaxLinkCount = 32
            };
        }
    }
}