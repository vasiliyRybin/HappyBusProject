using HappyBusProject.ModelsToReturn;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HappyBusProject.Controllers
{
    [Route("AppAPI/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public UsersInfo[] Get()
        {
            try
            {
                //var test1 = 0;
                //var test2 = 10 / test1;
                using var db = new MyShuttleBusAppDBContext();
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
                using var db = new MyShuttleBusAppDBContext();
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

        [HttpPost("{name}/{email}/{phoneNumber}")]
        public string Post(string name, string email, string phoneNumber)
        {
            if (name.Length > 50 || !new Regex(pattern: @"(^[a-zA-Z '-]{1,25})|(^[А-Яа-я '-]{1,25})").IsMatch(name)) return "Invalid name";
            if (phoneNumber.Length > 12 || phoneNumber.Any(c => !char.IsDigit(c))) return "Invalid phone number";
            if (email.Length > 30 || !new Regex(pattern: @"^([0-9a-zA-Z_-]{1,20}@[a-zA-Z]{1,10}.[a-zA-Z]{1,3})").IsMatch(email)) return "Invalid E-Mail address type";

            try
            {
                using var dbContext = new MyShuttleBusAppDBContext();

                User user = new()
                {
                    Id = Guid.NewGuid(),
                    FullName = name,
                    Rating = 5.0,
                    PhoneNumber = phoneNumber,
                    Email = email.ToLower(),
                    IsInBlacklist = false
                };

                dbContext.Users.Add(user);
                int successUpdate = dbContext.SaveChanges();
                if (successUpdate > 0) return "User succesfully added";
                else return "No changes been made";

            }
            catch (Exception e)
            {
                AppTools.ErrorWriterTpFile(e.Message + " " + "GET Method(param)");
                return e.Message;
            }
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{name}")]
        public string Delete(string name)
        {
            try
            {
                using var db = new MyShuttleBusAppDBContext();
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
