using HappyBusProject.InputModels;
using HappyBusProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HappyBusProject.Controllers
{
    [Route("AppAPI/NewDriversController")]
    [ApiController]
    public class NewDriversController : ControllerBase
    {
        private readonly DriverService _service;

        public NewDriversController(DriverService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDrivers()
        {
            var result = await _service.GetAllAsync();

            if (result != null) return Ok(result);
            return NoContent();
        }

        [HttpGet("{DriverName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDriverByName(string DriverName)
        {
            var result = await _service.GetByNameAsync(DriverName);

            if (result != null) return Ok(result);

            return NotFound();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDriver(DriverCarInputModel newState)
        {
            var result = await _service.CreateAsync(newState);
            if (result != null) return Ok(result);
            return Conflict();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDriverInfo(PutMethodDriverInputModel driverInputModel)
        {
            var result = await _service.UpdateDriver(driverInputModel);
            if (result) return Ok(driverInputModel);
            else return Conflict();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveDriver(string DriverName)
        {
            var result = await _service.DeleteDriver(DriverName);
            if (result) return Ok();
            return Conflict();
        }
    }
}
