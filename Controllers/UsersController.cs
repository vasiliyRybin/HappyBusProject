using HappyBusProject.ModelsToReturn;
using HappyBusProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HappyBusProject.Controllers
{
    [Route("AppAPI/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository<UsersInfo[], UsersInfo> _repository;

        public UsersController(IUsersRepository<UsersInfo[], UsersInfo> usersRepository)
        {
            _repository = usersRepository;
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

        [HttpPost("{name}/{phoneNumber}")]
        public async Task<IActionResult> Post(string name, string phoneNumber, string email)
        {
            var usersInput = new UsersInfo
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber
            };

            UsersInputValidation.AssignEmptyStringsToNullValues(usersInput);

            return await _repository.CreateAsync(usersInput);
        }

        [HttpPut("{name}")]
        public async Task<IActionResult> Put(string name, string phoneNumber, string email)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) && string.IsNullOrWhiteSpace(email)) return new BadRequestResult();

            var usersInput = new UsersInfo
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber
            };

            UsersInputValidation.AssignEmptyStringsToNullValues(usersInput);

            return await _repository.UpdateAsync(usersInput);
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            return await _repository.DeleteAsync(name);
        }
    }
}
