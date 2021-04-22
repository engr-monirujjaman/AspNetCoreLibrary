// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RateLimiterSample
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new IPAggregatedRateLimiter(2, 2));
            services.AddSingleton(new ConcurrencyLimiter(1));
            services.AddSingleton(new RateLimiter(2, 2));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.UseRouting();

            app.UseRateLimiter();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/di", async context =>
                {
                    await Task.Delay(5000);
                    await context.Response.WriteAsync("Hello World!");
                }).EnforceLimit<RateLimiter>();

                endpoints.MapGet("/concurrent", async context =>
                {
                    await Task.Delay(5000);
                    await context.Response.WriteAsync("Wrote!");
                }).EnforceLimit<ConcurrencyLimiter>();

                endpoints.MapGet("/adhoc", async context =>
                {
                    await Task.Delay(5000);
                    await context.Response.WriteAsync("Tested!");
                }).EnforceLimit(requestPerSecond: 2);

                endpoints.MapGet("/ip", async context =>
                {
                    await Task.Delay(5000);
                    await context.Response.WriteAsync("IP limited!");
                }).EnforceAggregatedLimit<IPAggregatedRateLimiter>();
            });
        }
    }
}
