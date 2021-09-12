using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;
using System.Net;

namespace Pb {
    public class Program {
        public static void Main(string[] args) {

            Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/CompanyIdRobot_.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

            Log.Information(new string('=', 60));
            Log.Information("Running CompanyIdRobot ver 0.1");

            var host = new WebHostBuilder().UseKestrel(GetKso()).UseContentRoot(Directory.GetCurrentDirectory()).UseStartup<Startup>().UseWebRoot("static").Build();
            host.Run();
        }

        static Action<KestrelServerOptions> GetKso() {
            Action<KestrelServerOptions> ret = options =>
            {
                options.Limits.MaxConcurrentConnections = 100;
                options.Listen(IPAddress.Any, 5001);
            };
            return ret;
        }
    }

    public class Startup {

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            app.Run(async (context) => {
                await context.Response.WriteAsync("Hello, world");
            });
        }

    }
}
