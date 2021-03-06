using HappyBusProject.AuthLayer.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HappyBusProject.AuthLayer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();

            var authOptionsCfg = Configuration.GetSection("Auth");
            services.Configure<AuthOptions>(authOptionsCfg);

            services.AddCors
            (
                options =>
                {
                    options.AddDefaultPolicy
                    (
                        builder =>
                        {
                            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                        }
                    );
                }
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
