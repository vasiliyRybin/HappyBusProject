using HappyBusProject.InputModels;
using HappyBusProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HappyBusProject.Controllers
{
    [Route("AppAPI/NewCarsState")]
    [ApiController]
    public class NewCarsCurrentStateController : ControllerBase
    {
        private NewCarsCurrentStateService _service { get; }

        public NewCarsCurrentStateController(NewCarsCurrentStateService service)
        {
            _service = service;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetAllStates();

            if (result != null) return Ok(result);
            return NoContent();
        }

        [HttpGet("{DriverName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(string DriverName)
        {
            var result = await _service.GetStateByDriversName(DriverName);

            if (result != null) return Ok(result);
            return NotFound();
        }


        /// <summary>
        /// For Admin usage only! 
        /// Use at your own risk! :D
        /// Use it just for if something went wrong due creating state when new driver been created
        /// </summary>
        /// <param name="newState"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateState(CarStatePostModel newState)
        {
            var result = await _service.CreateState(newState);
            if (result != null) return Ok(result);
            return Conflict();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateState(string DriverName, CarStateInputModel newState)
        {
            var result = await _service.UpdateState(DriverName, newState);
            if (result) return Ok(result);
            return Conflict();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveState(string DriverName)
        {
            var result = await _service.DeleteState(DriverName);
            if (result) return Ok(result);
            return Conflict();
        }
    }
}
