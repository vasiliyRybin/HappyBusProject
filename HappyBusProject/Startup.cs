using AutoMapper;
using HappyBusProject.AuthLayer.Common;
using HappyBusProject.Extensions;
using HappyBusProject.MappingProfiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Text.Json.Serialization;

namespace HappyBusProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .AddJsonOptions(x =>
                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            var authOptions = Configuration.GetSection("Auth").Get<AuthOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = authOptions.Audience,
                        ValidateLifetime = true,

                        IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                })
            ;
            services.AddSwaggerGen();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CarsCurrentStateProfile());
                mc.AddProfile(new DriverProfile());
                mc.AddProfile(new OrderProfile());
                mc.AddProfile(new UserProfile());
                mc.AddProfile(new CarProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration, sectionName: "Serilog")
                .CreateLogger();

            services.AddControllersWithViews();
            services.AddDbContext<MyShuttleBusAppNewDBContext>
                (options => options.UseSqlServer(Environment.GetEnvironmentVariable("DBConnectionString", EnvironmentVariableTarget.Machine))
                .EnableSensitiveDataLogging());

            services.AddTransientScopedSingletonEntities(mapper, logger);

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
            services.AddSwaggerJWTTokenAuthentication();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
