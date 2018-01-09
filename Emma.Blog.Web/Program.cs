using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Emma.Blog.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var host = BuildWebHost(args);

            host.Run();

        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
            .UseKestrel()
             
            .UseStartup<Startup>()
            .Build();



    }
}
