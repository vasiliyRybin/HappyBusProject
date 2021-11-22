using HappyBusProject.Interfaces;
using HappyBusProject.ModelsToReturn;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HappyBusProject.Repositories
{
    public interface IDriversRepository<T> : IBusAppObject<T>
        where T : DriverViewModel
    {
        Task<T> CreateAsync(DriverCarInputModel driverInfo);
        Task UpdateDriver(DriverCarInputModel driverInfo);
        Task DeleteDriver(string name);
    }
}
