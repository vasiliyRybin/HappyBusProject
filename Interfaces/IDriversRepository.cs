using HappyBusProject.Interfaces;
using HappyBusProject.ModelsToReturn;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HappyBusProject.Repositories
{
    public interface IDriversRepository<T> : IPerson<T>
        where T : class
    {
        Task<IActionResult> CreateAsync(DriverCarPreResultModel driverInfo);
        Task<IActionResult> UpdateAsync(DriverCarPreResultModel driverInfo);
        Task<IActionResult> DeleteAsync(string name);
    }
}
