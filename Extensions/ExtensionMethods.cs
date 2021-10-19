using HappyBusProject.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HappyBusProject.Extensions
{
    public static class ExtensionMethods
    {
        public static void AddEntities(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<UsersRepository>();
            services.AddTransient<DriversRepository>();
        }
    }
}
