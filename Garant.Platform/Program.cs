using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore;

namespace Garant.Platform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        //public static IWebHostBuilder CreateHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseKestrel()
        //                .UseContentRoot(Directory.GetCurrentDirectory())
        //                .UseUrls("https://*:9991")
        //                .UseStartup<Startup>(); 
        //        });

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls("http://*:9898")
                .UseStartup<Startup>();
    }
}
