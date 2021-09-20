using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Garant.Platform
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
                webBuilder.UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseUrls("https://localhost:44344")
                    .UseStartup<Startup>();
            });

        //Host.CreateDefaultBuilder(args)
        //.ConfigureWebHostDefaults(webBuilder =>
        //{
        //    webBuilder.UseStartup<Startup>();
        //});
        //Host.CreateDefaultBuilder(args)
        //.ConfigureWebHostDefaults(webBuilder =>
        //{
        //    webBuilder
        //        //.UseKestrel()
        //        //.UseContentRoot(Directory.GetCurrentDirectory())
        //        //.UseUrls("httpz://*:9999")
        //        .UseKestrel(options =>
        //        {
        //            options.Listen(IPAddress.Loopback, 5000);  // http:localhost:5000
        //            options.Listen(IPAddress.Any, 80);         // http:*:80
        //            options.Listen(IPAddress.Loopback, 443, listenOptions =>
        //            {
        //                listenOptions.UseHttps("certificate.pfx", "password");
        //            });
        //        })
        //        .UseStartup<Startup>();
        //});
    }
}
