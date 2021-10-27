using HappyBusProject.ModelsToReturn;
using HappyBusProject.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HappyBusProject.Extensions
{
    public static class ExtensionMethods
    {
        public static void AddTransientScopedSingletonEntities(this IServiceCollection services)
        {
            services.AddTransient<IDriversRepository<DriverViewModel[], DriverViewModel>, DriversRepository>();
            services.AddTransient<IUsersRepository<UsersViewModel[], UsersViewModel>, UsersRepository>();
        }
    }
}
