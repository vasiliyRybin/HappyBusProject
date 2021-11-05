using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HappyBusProject.Interfaces
{
    public interface IPerson<T, T1>
        where T : class
        where T1 : class
    {
        Task<ActionResult<T>> GetAllAsync();
        Task<ActionResult<T1>> GetByNameAsync(string name);
    }
}
