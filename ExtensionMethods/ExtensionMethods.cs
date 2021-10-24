using HappyBusProject.ModelsToReturn;
using HappyBusProject.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HappyBusProject.Extensions
{
    public static class ExtensionMethods
    {
        public static void AddTransientScopedSingletonEntities(this IServiceCollection services)
        {
            services.AddTransient<IDriversRepository<DriverInfo[]>, DriversRepository>();
            services.AddTransient<IUsersRepository<UsersInfo[]>, UsersRepository>();
        }
    }
}
