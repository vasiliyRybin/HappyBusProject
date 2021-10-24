using HappyBusProject.Interfaces;
using HappyBusProject.ModelsToReturn;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HappyBusProject.Repositories
{
    public interface IUsersRepository<T> : IPerson<T>
        where T : class
    {
        Task<IActionResult> CreateAsync(UsersInfo usersInfo);
        Task<IActionResult> UpdateAsync(UsersInfo usersInfo);
        Task<IActionResult> DeleteAsync(string name);
    }
}
