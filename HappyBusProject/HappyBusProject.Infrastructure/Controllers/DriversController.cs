using HappyBusProject.InputValidators;
using HappyBusProject.ModelsToReturn;
using HappyBusProject.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HappyBusProject.Controllers
{
    [ApiController]
    [Route("AppAPI/Drivers")]
    public class DriversController : ControllerBase
    {
        private readonly IDriversRepository<DriverViewModel[], DriverViewModel> _repository;
        private Guid UserID => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public DriversController(IDriversRepository<DriverViewModel[], DriverViewModel> driversRepository)
        {
            _repository = driversRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            return new JsonResult(await _repository.GetAllAsync());
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            return new JsonResult(await _repository.GetByNameAsync(name));
        }

        [HttpPost]
        public async Task<IActionResult> PostTest([FromQuery] DriverCarInputModel driverCar)
        {
            var isNotEmtpy = DriversInputValidation.IsEmptyInputValues(driverCar);
            if (!isNotEmtpy) return new BadRequestResult();

            return new ObjectResult(await _repository.CreateAsync(driverCar));
        }

        [HttpPut("{driverName}/{newCarBrand}")]
        public async Task<IActionResult> TestPut(string driverName, string newCarBrand)
        {
            DriverCarInputModel driverCar = new()
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
