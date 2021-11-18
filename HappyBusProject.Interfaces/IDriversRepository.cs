using HappyBusProject.Interfaces;
using HappyBusProject.ModelsToReturn;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HappyBusProject.Repositories
{
    public interface IDriversRepository<T> : IBusAppObject<T>
        where T : IActionResult
    {
        Task<T> CreateAsync(DriverCarInputModel driverInfo);
        Task<IActionResult> UpdateAsync(DriverCarInputModel driverInfo);
        Task<IActionResult> DeleteAsync(string name);
    }
}
