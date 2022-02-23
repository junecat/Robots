using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;
using System.Net;

namespace GetDocNumbers
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("Logs/GetDocNumbersRobot_.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            Log.Information(new string('=', 60));
            Log.Information("Running GetDocNumbersRobot ver 0.1");

            var host = new WebHostBuilder().UseKestrel(GetKso()).UseContentRoot(Directory.GetCurrentDirectory()).UseStartup<Startup>().UseWebRoot("static").Build();
            host.Run();

        }

        static Action<KestrelServerOptions> GetKso() {
            Action<KestrelServerOptions> ret = options =>
            {
                options.Limits.MaxConcurrentConnections = 100;
                options.Listen(IPAddress.Any, 5002);
            };
            return ret;
        }

    }
}
