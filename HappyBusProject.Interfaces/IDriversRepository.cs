using System.Threading.Tasks;

namespace HappyBusProject.Interfaces
{
    public interface IDriversRepository<T, T2> : IBusAppObject<T>
        where T : class
    {
        Task<T> CreateAsync(T2 driverInfo);
        Task UpdateDriver(T2 driverInfo);
        Task DeleteDriver(string name);
    }
}
