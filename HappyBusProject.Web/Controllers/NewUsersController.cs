using HappyBusProject.InputModels;
using HappyBusProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HappyBusProject.Controllers
{
    [Route("AppAPI/NewUsersController")]
    [ApiController]
    public class NewUsersController : ControllerBase
    {
        private readonly UsersService _service;

        public NewUsersController(UsersService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDrivers()
        {
            var result = await _service.GetAllUsers();
            if (result != null) return Ok(result);
            return null;
        }

        [HttpGet("{UserName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserByName(string UserName)
        {
            var result = await _service.GetByNameAsync(UserName);
            if (result != null) return Ok(result);
            return NotFound();
        }


        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> CreateUser(UserInputModel newState)
        {
            var result = await _service.CreateAsync(newState);
            if (result != null) return Ok(result);
            return Conflict();
        }

        [HttpPut]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> UpdateDriverInfo(UserInputModel userInputModel)
        {
            var result = await _service.UpdateUserInfo(userInputModel);
            if (result) return Ok(userInputModel);
            else return Conflict();
        }

        [HttpDelete("{UserName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveDriver(string UserName)
        {
            var result = await _service.DeleteUser(UserName);
            if (result) return Ok();
            return Conflict();
        }
    }
}
