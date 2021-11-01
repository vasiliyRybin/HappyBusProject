using HappyBusProject.HappyBusProject.DataLayer.InputModels;
using HappyBusProject.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HappyBusProject.Repositories
{
    public interface IUsersRepository<T, T1> : IPerson<T, T1>
        where T : class
        where T1 : class
    {
        Task<ActionResult<T1>> CreateAsync(UserInputModel usersInfo);
        Task<IActionResult> UpdateAsync(UserInputModel usersInfo);
        Task<IActionResult> DeleteAsync(string name);
    }
}
