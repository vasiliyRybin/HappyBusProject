using HappyBusProject.ModelsToReturn;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;


namespace HappyBusProject.Controllers
{
    [Route("AppAPI/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyShuttleBusAppNewDBContext _context;
        public UsersController(MyShuttleBusAppNewDBContext myShuttleBusAppNewDBContext)
        {
            _context = myShuttleBusAppNewDBContext;
        }

        [HttpGet]
        public UsersInfo[] Get()
        {
            try
            {
                var users = _context.Users.ToList();
                UsersInfo[] result = new UsersInfo[users.Count];

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = new UsersInfo { Name = users[i].FullName, Rating = users[i].Rating, PhoneNumber = users[i].PhoneNumber, Email = users[i].Email, IsInBlackList = users[i].IsInBlacklist };
                }

                return result;
            }
            catch (Exception e)
            {
                AppTools.ErrorWriterTpFile(e.Message + " " + "GET Method");
                return new UsersInfo[] { new UsersInfo { ErrorMessage = DateTime.Now + " " + e.Message } };
            }
        }

        [HttpGet("{name}")]
        public UsersInfo[] Get(string name)
        {
            try
            {
                var users = _context.Users.Where(u => u.FullName.Contains(name)).ToList();
                UsersInfo[] result = new UsersInfo[users.Count];

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = new UsersInfo { Name = users[i].FullName, Rating = users[i].Rating, PhoneNumber = users[i].PhoneNumber, Email = users[i].Email, IsInBlackList = users[i].IsInBlacklist };
                }

                return result;
            }
            catch (Exception e)
            {
                AppTools.ErrorWriterTpFile(e.Message + " " + "GET Method");
                return new UsersInfo[] { new UsersInfo { ErrorMessage = DateTime.Now + " " + e.Message } };
            }
        }

        [HttpPost("{name}/{phoneNumber}")]
        public string Post(string name, string phoneNumber, string email)
        {
            string check = AppTools.UsersValuesValidation(name, ref phoneNumber, ref email);

            if (check != "ok") return check;

            try
            {

                User user = new()
                {
                    Id = Guid.NewGuid(),
                    FullName = name,
                    Rating = 5.0,
                    PhoneNumber = phoneNumber,
                    Email = email.ToLower(),
                    IsInBlacklist = false,
                    RegistrationDateTime = DateTime.Now
                };

                _context.Users.Add(user);
                int successUpdate = _context.SaveChanges();
                if (successUpdate > 0) return "User succesfully added";
                return "No changes been made";

            }
            catch (Exception e)
            {
                AppTools.ErrorWriterTpFile(e.Message + " " + "GET Method(param)");
                return e.Message;
            }
        }

        [HttpPut("{name}")]
        public string Put(string name, string phoneNumber, string email)
        {
            string check = AppTools.UsersValuesValidation(name, ref phoneNumber, ref email);
            if (check != "ok") return check;

            try
            {
                var user = _context.Users.FirstOrDefault(c => c.FullName.Contains(name));
                if (user != null)
                {
                    if (!string.IsNullOrWhiteSpace(phoneNumber)) user.PhoneNumber = phoneNumber;
                    if (!string.IsNullOrWhiteSpace(email)) user.Email = email;
                    _context.SaveChanges();
                    return "Info successfully updated";
                }
                return "No changes been made";

            }
            catch (Exception e)
            {
                AppTools.ErrorWriterTpFile(e.Message + " " + "PUT method");
                return e.Message;
            }
        }

        [HttpDelete("{name}")]
        public string Delete(string name)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(c => c.FullName.Contains(name));

                if (user != null)
                {
                    _context.Remove(user);
                    _context.SaveChanges();
                    return "User successfully deleted";
                }
                return "No changes been made";

            }
            catch (Exception e)
            {
                AppTools.ErrorWriterTpFile(e.Message + " " + "DELETE method");
                return e.Message;
            }
        }
    }
}
