using HibernatingRhinos.Profiler.Appender.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace HappyBusProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            EntityFrameworkProfilerBootstrapper.PreStart();
            EntityFrameworkProfiler.Initialize();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging((ctx, builder) =>
            {
                builder.AddConfiguration(ctx.Configuration.GetSection("Logging"));
                builder.AddFile(o => o.RootPath = ctx.HostingEnvironment.ContentRootPath);
            })
            .ConfigureWebHostDefaults(webBuilder =>
                {

                    webBuilder.UseStartup<Startup>();
                }
                );
    }
}

