using HappyBusProject.DB.Models;
using System.Threading.Tasks;

namespace HappyBusProject.Interfaces
{
    public interface IBusAppObject<T>
        where T: IEntity
    {
        Task<T> GetAllAsync();
        Task<T> GetByNameAsync(string name);
    }
}
