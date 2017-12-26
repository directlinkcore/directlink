using System;
using DirectLinkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace RoutingExample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDirectLink<Routing>(
                options => {
                    options.HotModuleReplacement = true;
                    options.NodeInstanceCount = 1;
                    options.FileWatcherDelay = TimeSpan.FromMilliseconds(200);
                },
                tags => tags.AddDefaultTemplateTags(title: "Routing", hmr: true, isDevelopment: true)
                    .AddHeadTag(("https://use.fontawesome.com/releases/v5.0.2/css/all.css", "", "").ToLink())
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseDirectLink(components => components
                .Map("Employee", "employee")
            );

            Routes.For<Routing>(routes =>
                routes.MapDefaultRoute("Home", "/")
                    .MapRoute<Company>("/companies")
                    .MapRoute<Company>("/company/{name:string}")
                    .MapRoute<Company>("/company/{name:string}:{view:string}")
                    .MapRoute<Company>("/company/{id:int}", new { view = "history" })
                    .MapRoute<Company>("/company/{id:int}:{view:string}")
                    .MapRoute("Employee", "/employee")
                    .MapRoute("Employee", "/employee/{id:int}")
                    .MapRoute("Employee", "/employee/{name:string}")
            );
        }
    }
}