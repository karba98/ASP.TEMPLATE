using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using PLANTILLA.TEST.DS;

namespace PLANTILLA.TEST.DS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseKestrel(options =>
                    //{

                    //    options.Listen(IPAddress.Any, 5000);  // http:localhost:5000
                    //    //options.Listen(IPAddress.Any, 80);         // http:*:80
                    //    options.Listen(IPAddress.Any, 8080);         // http:*:8080
                    //    //options.Listen(IPAddress.Any, 8880);         // http:*:8880

                    //    options.Listen(IPAddress.Any, 5001, listenOptions => // https
                    //    {
                    //        listenOptions.UseHttps("/etc/pki/tls/certs/vypcert.pfx", "i9*26Ppf2!sFBfXD2#^&@VR2n3%@$ym");
                    //    });
                    //});
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog((hostingContext, loggerConfig) =>
                      loggerConfig.ReadFrom.Configuration(hostingContext.Configuration))
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddHttpClient();
                }).UseConsoleLifetime();

    }
}
