using HappyBusProject.ModelsToReturn;
using HappyBusProject.Repositories;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1806

namespace HappyBusProject.Controllers
{
    [ApiController]
    [Route("AppAPI/Drivers")]
    public class DriversController : ControllerBase
    {
        private readonly IDriversRepository<DriverInfo[]> _db;

        public DriversController(DriversRepository driversRepository)
        {
            _db = driversRepository;
        }

        [HttpGet]
        public DriverInfo[] Get()
        {
            return _db.GetAllDrivers();
        }

        [HttpGet("{name}")]
        public DriverInfo[] Get(string name)
        {
            return _db.GetDriverByName(name);
        }

        [HttpPost("{brand}/{seatsNum}/{registrationNumPlate}/{carAge}/{driverName}/{driverAge}/{examPass}")]
        public string PostTest(string brand, string seatsNum, string registrationNumPlate, string carAge, string driverName, string driverAge, string examPass = "1900-01-01 00:00:00")
        {
            return _db.Create(brand, seatsNum, registrationNumPlate, carAge, driverName, driverAge, examPass);
        }

        [HttpPut("{driverName}/{newCarBrand}")]
        public string TestPut(string driverName, string newCarBrand)
        {
            return _db.Update(driverName, newCarBrand);
        }

        [HttpDelete("{driverName}")]
        public string TestDelete(string driverName)
        {
            return _db.Delete(driverName);
        }
    }
}
