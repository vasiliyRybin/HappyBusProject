using System.Threading.Tasks;

namespace HappyBusProject.Interfaces
{
    public interface IBusAppObject<T>
        where T : class
    {
        Task<T[]> GetAllAsync();
        Task<T> GetByNameAsync(string name);
    }
}
