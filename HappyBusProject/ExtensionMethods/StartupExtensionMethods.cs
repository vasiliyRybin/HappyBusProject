using AutoMapper;
using HappyBusProject.HappyBusProject.BusinessLayer.Notifier;
using HappyBusProject.InputModels;
using HappyBusProject.Interfaces;
using HappyBusProject.Repositories;
using HappyBusProject.SmsNotificationLayer.Interfaces;
using HappyBusProject.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace HappyBusProject.Extensions
{
    public static class StartupExtensionMethods
    {
        public static void AddTransientScopedSingletonEntities(this IServiceCollection services, IMapper mapper)
        {
            services.AddSingleton<ISmsNotifier, TwilioSMSNotifier>();
            services.AddTransient<IDriversRepository<DriverViewModel, DriverCarInputModel>, DriversRepository>();
            services.AddTransient<IUsersRepository<UsersViewModel, UserInputModel>, UsersRepository>();
            services.AddTransient<IOrderRepository<OrderViewModel, OrderInputModel, OrderInputModelPutMethod>, OrdersRepository>();
            services.AddTransient<ICarsStateRepository<CarStateViewModel, CarStatePostModel, CarStateInputModel>, CarsCurrentStateRepository>();
            services.AddSingleton(mapper);
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
