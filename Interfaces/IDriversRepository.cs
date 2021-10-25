using HappyBusProject.Interfaces;
using HappyBusProject.ModelsToReturn;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HappyBusProject.Repositories
{
    public interface IDriversRepository<T, T1> : IPerson<T, T1>
        where T : class
        where T1 : class
    {
        Task<IActionResult> CreateAsync(DriverCarPreResultModel driverInfo);
        Task<IActionResult> UpdateAsync(DriverCarPreResultModel driverInfo);
        Task<IActionResult> DeleteAsync(string name);
    }
}
