using System.Threading.Tasks;

namespace HappyBusProject.Interfaces
{
    public interface IPerson<T>
    {
        Task<T> GetAllAsync();
        Task<T> GetByNameAsync(string name);
    }
}
