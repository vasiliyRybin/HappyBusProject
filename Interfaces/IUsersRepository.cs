using HappyBusProject.Interfaces;
using HappyBusProject.ModelsToReturn;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HappyBusProject.Repositories
{
    public interface IUsersRepository<T, T1> : IPerson<T, T1>
        where T : class
        where T1 : class
    {
        Task<ActionResult<T1>> CreateAsync(UsersInfo usersInfo);
        Task<IActionResult> UpdateAsync(UsersInfo usersInfo);
        Task<IActionResult> DeleteAsync(string name);
    }
}
