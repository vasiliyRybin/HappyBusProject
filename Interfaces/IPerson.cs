using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HappyBusProject.Interfaces
{
    public interface IPerson<T> where T : class
    {
        Task<ActionResult<T>> GetAllAsync();
        Task<ActionResult<T>> GetByNameAsync(string name);
    }
}
