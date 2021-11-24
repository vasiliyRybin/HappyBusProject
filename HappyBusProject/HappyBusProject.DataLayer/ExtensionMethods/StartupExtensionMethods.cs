using HappyBusProject.BusinessLayer;
using HappyBusProject.HappyBusProject.Interfaces;
using HappyBusProject.Interfaces;
using HappyBusProject.Repositories;
using HappyBusProject.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace HappyBusProject.Extensions
{
    public static class StartupExtensionMethods
    {
        public static void AddTransientScopedSingletonEntities(this IServiceCollection services)
        {
            services.AddTransient<IDriversRepository<IActionResult>, DriversRepository>();
            services.AddTransient<IUsersRepository<IActionResult>, UsersRepository>();
            services.AddTransient<IOrderRepository<IActionResult>, OrdersRepository>();
            services.AddTransient<ICarsStateRepository<IActionResult>, CarsCurrentStateRepository>();
            services.AddTransient<DriverService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        public static void AddSwaggerJWTTokenAuthentication(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My Shuttle Bus App API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    In = ParameterLocation.Header,
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Description = "Please insert JWT token into field",
                    Name = "Authorization"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                         {
                           Reference = new OpenApiReference
                           {
                             Type = ReferenceType.SecurityScheme,
                             Id = "Bearer"
                           }
                          },
                          Array.Empty<string>()
                    }
                  });
            });
        }
    }
}
