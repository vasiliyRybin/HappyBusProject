using HappyBusProject.HappyBusProject.DataLayer.InputModels;
using HappyBusProject.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HappyBusProject.Repositories
{
    public interface IUsersRepository<T> : IObject<T>
        where T : IActionResult
    {
        Task<T> CreateAsync(UserInputModel usersInfo);
        Task<IActionResult> UpdateAsync(UserInputModel usersInfo);
        Task<IActionResult> DeleteAsync(string name);
    }
}
