using HappyBusProject.ModelsToReturn;
using HappyBusProject.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace HappyBusProject.Controllers
{
    [Route("AppAPI/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository<UsersInfo[]> _db;
        public UsersController(UsersRepository usersRepository)
        {
            _db = usersRepository;
        }

        [HttpGet]
        public UsersInfo[] Get()
        {
            return _db.GetAll();
        }

        [HttpGet("{name}")]
        public UsersInfo[] Get(string name)
        {
            return _db.GetByName(name);
        }

        [HttpPost("{name}/{phoneNumber}")]
        public string Post(string name, string phoneNumber, string email)
        {
            var usersInput = new UsersInfo
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber
            };

            UsersInputValidation.AssignEmptyStringsToNullValues(usersInput);

            return _db.Create(usersInput);
        }

        [HttpPut("{name}")]
        public string Put(string name, string phoneNumber, string email)
        {
            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(name)) return "Both fields are empty";

            var usersInput = new UsersInfo
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber
            };

            UsersInputValidation.AssignEmptyStringsToNullValues(usersInput);

            return _db.Update(usersInput);
        }

        [HttpDelete("{name}")]
        public string Delete(string name)
        {
            return _db.Delete(name);
        }
    }
}
