using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore
{
    public class WebApplication : IHost, IDisposable, IAsyncDisposable, IApplicationBuilder, IEndpointRouteBuilder
    {
        // Top level properties to access common services
        public ILogger Logger => default!;
        public IEnumerable<string> Addresses => default!;
        public IHostApplicationLifetime Lifetime => default!;
        public IServiceProvider Services => default!;
        public IConfiguration Configuration => default!;
        public IWebHostEnvironment Environment => default!;

        // Factory methods
        public static WebApplication Create(string[] args) => default!;
        public static WebApplication Create() => default!;
        public static WebApplicationBuilder CreateBuilder() => default!;
        public static WebApplicationBuilder CreateBuilder(string[] args) => default!;

        // Methods used to start the host
        public void Run(params string[] urls) { }
        public void Run() { }
        public Task RunAsync(CancellationToken cancellationToken = default, params string[] urls) => Task.CompletedTask;
        public Task StartAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task StopAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public void Dispose() { }
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;

        IServiceProvider IApplicationBuilder.ApplicationServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        IFeatureCollection IApplicationBuilder.ServerFeatures => throw new NotImplementedException();

        IDictionary<string, object?> IApplicationBuilder.Properties => throw new NotImplementedException();

        IServiceProvider IEndpointRouteBuilder.ServiceProvider => throw new NotImplementedException();

        ICollection<EndpointDataSource> IEndpointRouteBuilder.DataSources => throw new NotImplementedException();


        IApplicationBuilder IApplicationBuilder.Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            throw new NotImplementedException();
        }

        IApplicationBuilder IApplicationBuilder.New()
        {
            throw new NotImplementedException();
        }

        RequestDelegate IApplicationBuilder.Build()
        {
            throw new NotImplementedException();
        }

        IApplicationBuilder IEndpointRouteBuilder.CreateApplicationBuilder()
        {
            throw new NotImplementedException();
        }
    }
}
