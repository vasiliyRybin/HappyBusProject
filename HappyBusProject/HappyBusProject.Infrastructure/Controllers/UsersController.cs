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
        private readonly IUsersRepository<UsersViewModel[], UsersViewModel> _repository;

        public UsersController(IUsersRepository<UsersViewModel[], UsersViewModel> usersRepository)
        {
            _repository = usersRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            return new ObjectResult(await _repository.GetAllAsync());
        }

        [HttpGet("{name}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(string name)
        {
            return new ObjectResult(await _repository.GetByNameAsync(name));
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<UsersViewModel>> Post([FromQuery] UserInputModel userInput)
        {
            UsersInputValidation.AssignEmptyStringsToNullValues(userInput);

            return await _repository.CreateAsync(userInput);
        }

        [HttpPut]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Put([FromBody] UserInputModel userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput.PhoneNumber) && string.IsNullOrWhiteSpace(userInput.Email)) return new BadRequestResult();

            UsersInputValidation.AssignEmptyStringsToNullValues(userInput);

            return await _repository.UpdateAsync(userInput);
        }

        [HttpDelete("{name}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Delete(string name)
        {
            return await _repository.DeleteAsync(name);
        }
    }
}
