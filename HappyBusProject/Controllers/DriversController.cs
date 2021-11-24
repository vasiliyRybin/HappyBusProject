using HappyBusProject.InputValidators;
using HappyBusProject.ModelsToReturn;
using HappyBusProject.Repositories;
using HappyBusProject.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HappyBusProject.Controllers
{
    [ApiController]
    [Route("AppAPI/Drivers")]
    public class DriversController : ControllerBase
    {
        private readonly DriverService _driverService;

        public DriversController(DriverService driverService)
            => _driverService = driverService;

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            return new JsonResult(await _driverService.GetAllAsync());
        }

        [HttpGet("{name}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(string name)
        {
            return new JsonResult(await _driverService.GetByNameAsync(name));
        }

        [HttpPost]
        [Authorize(Roles = "Driver, Admin")]
        public async Task<IActionResult> PostTest([FromBody] DriverCarInputModel driverCar)
        {
            var isNotEmtpy = DriversInputValidation.IsEmptyInputValues(driverCar);
            var isNotValid = DriversInputValidation.DriversInputValidator(driverCar, out _);

            if (!isNotEmtpy || !isNotValid) return new BadRequestResult();

            var result = await _driverService.CreateAsync(driverCar);
            return new ObjectResult(result);
        }

        [HttpPut("{driverName}/{newCarBrand}")]
        [Authorize(Roles = "Driver, Admin")]
        public async Task<IActionResult> TestPut(string driverName, string newCarBrand)
        {
            DriverCarInputModel driverCar = new()
            {
                DriverName = driverName,
                CarBrand = newCarBrand
            };

            return await _driverService.UpdateAsync(driverCar);
        }

        [HttpDelete("{driverName}")]
        [Authorize(Roles = "Driver, Admin")]
        public async Task<IActionResult> TestDelete(string driverName)
        {
            return await _driverService.DeleteAsync(driverName);
        }
    }
}
