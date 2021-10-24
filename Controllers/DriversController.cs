using HappyBusProject.InputValidators;
using HappyBusProject.ModelsToReturn;
using HappyBusProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HappyBusProject.Controllers
{
    [ApiController]
    [Route("AppAPI/Drivers")]
    public class DriversController : ControllerBase
    {
        private readonly IDriversRepository<DriverInfo[]> _repository;

        public DriversController(IDriversRepository<DriverInfo[]> driversRepository)
        {
            _repository = driversRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new ObjectResult(await _repository.GetAllAsync());
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            return new ObjectResult(await _repository.GetByNameAsync(name));
        }

        [HttpPost]
        public async Task<IActionResult> PostTest([FromQuery] DriverCarPreResultModel driverCar)
        {
            var isNotEmtpy = DriversInputValidation.IsEmptyInputValues(driverCar);
            if (!isNotEmtpy) return new BadRequestResult();

            return Created(" ", await _repository.CreateAsync(driverCar));
        }

        [HttpPut("{driverName}/{newCarBrand}")]
        public async Task<IActionResult> TestPut(string driverName, string newCarBrand)
        {
            DriverCarPreResultModel driverCar = new()
            {
                DriverName = driverName,
                CarBrand = newCarBrand
            };

            return await _repository.UpdateAsync(driverCar);
        }

        [HttpDelete("{driverName}")]
        public async Task<IActionResult> TestDelete(string driverName)
        {
            return await _repository.DeleteAsync(driverName);
        }
    }
}
