using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore
{
    public class WebApplicationBuilder
    {
        public IWebHostEnvironment Environment => default!;
        public IServiceCollection Services => default!;
        public IConfiguration Configuration => default!;
        public ILoggingBuilder Logging => default!;

        // Ability to configure existing web host and host
        public IWebHostBuilder WebHost => default!;
        public IHostBuilder Host => default!;

        public WebApplication Build() => default!;
    }
}
