using HappyBusProject.ModelsToReturn;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace HappyBusProject.Controllers
{
    [Route("AppAPI/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public UsersInfo[] Get()
        {
            try
            {
                //var test1 = 0;
                //var test2 = 10 / test1;
                using var db = new MyShuttleBusAppNewDBContext();
                var users = db.Users.ToList();

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
                //var test1 = 0;
                //var test2 = 10 / test1;
                using var db = new MyShuttleBusAppNewDBContext();
                var users = db.Users.Where(u => u.FullName.Contains(name)).ToList();

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
            string check = AppTools.ValuesValidation(name, ref phoneNumber, ref email);

            if (check != "ok") return check;

            try
            {
                using var dbContext = new MyShuttleBusAppNewDBContext();

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

                dbContext.Users.Add(user);
                int successUpdate = dbContext.SaveChanges();
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
            string check = AppTools.ValuesValidation(name, ref phoneNumber, ref email);
            if (check != "ok") return check;

            try
            {
                using var db = new MyShuttleBusAppNewDBContext();
                {
                    var user = db.Users.FirstOrDefault(c => c.FullName.Contains(name));
                    if (user != null)
                    {
                        if (!string.IsNullOrWhiteSpace(phoneNumber)) user.PhoneNumber = phoneNumber;
                        if (!string.IsNullOrWhiteSpace(email)) user.Email = email;
                        db.SaveChanges();
                        return "Info successfully updated";
                    }
                    return "No changes been made";
                }
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
                using var db = new MyShuttleBusAppNewDBContext();
                {
                    var user = db.Users.FirstOrDefault(c => c.FullName.Contains(name));

                    if (user != null)
                    {
                        db.Remove(user);
                        db.SaveChanges();
                        return "User successfully deleted";
                    }
                    return "No changes been made";
                }
            }
            catch (Exception e)
            {
                AppTools.ErrorWriterTpFile(e.Message + " " + "DELETE method");
                return e.Message;
            }
        }
    }
}
