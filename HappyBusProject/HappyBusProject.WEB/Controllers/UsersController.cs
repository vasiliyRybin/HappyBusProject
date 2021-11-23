using HappyBusProject.HappyBusProject.DataLayer.InputModels;
using HappyBusProject.ModelsToReturn;
using HappyBusProject.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HappyBusProject.Controllers
{
    [Route("AppAPI/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository<UsersViewModel> _repository;

        public UsersController(IUsersRepository<UsersViewModel> usersRepository)
        {
            _repository = usersRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            var result = await _repository.GetAllAsync();
            if (result != null) return Ok(result);
            return NoContent();
        }

        [HttpGet("{name}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(string name)
        {
            var result = await _repository.GetByNameAsync(name);
            if (result != null) return Ok(result);
            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Post([FromQuery] UserInputModel userInput)
        {
            UsersInputValidation.AssignEmptyStringsToNullValues(userInput);

            var result = await _repository.CreateAsync(userInput);

            if (result != null) return Ok();
            return Conflict();
        }

        [HttpPut]
        [Authorize(Roles = "User, Admin")]
        public void Put([FromBody] UserInputModel userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput.PhoneNumber) && string.IsNullOrWhiteSpace(userInput.Email)) return;

            UsersInputValidation.AssignEmptyStringsToNullValues(userInput);

            _repository.UpdateAsync(userInput);
        }

        [HttpDelete("{name}")]
        [Authorize(Roles = "User, Admin")]
        public void Delete(string name)
        {
            _repository.DeleteAsync(name);
        }
    }
}
