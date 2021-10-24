using HappyBusProject.ModelsToReturn;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.Repositories
{
    public class UsersRepository : IUsersRepository<UsersInfo[]>
    {
        private readonly MyShuttleBusAppNewDBContext _context;

        public UsersRepository(MyShuttleBusAppNewDBContext myShuttleBusAppNewDBContext)
        {
            _context = myShuttleBusAppNewDBContext ?? throw new ArgumentNullException(nameof(myShuttleBusAppNewDBContext));
        }

        public async Task<IActionResult> CreateAsync(UsersInfo usersInfo)
        {
            var check = UsersInputValidation.UsersValuesValidation(usersInfo, out string errorMessage);

            if (!check) return new BadRequestObjectResult(errorMessage);

            try
            {
                User user = new()
                {
                    Id = Guid.NewGuid(),
                    FullName = usersInfo.Name,
                    Rating = 5.0,
                    PhoneNumber = usersInfo.PhoneNumber,
                    Email = usersInfo.Email.ToLower(),
                    IsInBlacklist = false,
                    RegistrationDateTime = DateTime.Now
                };

                await _context.Users.AddAsync(user);
                int successUpdate = _context.SaveChanges();
                if (successUpdate > 0) return new OkResult();
                return new NoContentResult();
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "POST Method");
                return new BadRequestObjectResult(e.Message);
            }
        }

        public async Task<IActionResult> DeleteAsync(string name)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(c => c.FullName.Contains(name));

                if (user != null)
                {
                    _context.Remove(user);
                    await _context.SaveChangesAsync();
                    return new OkObjectResult("User successfully deleted");
                }

                return new NoContentResult();

            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "DELETE method");
                return new BadRequestObjectResult(e.Message);
            }
        }

        public async Task<ActionResult<UsersInfo[]>> GetAllAsync()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                UsersInfo[] result = new UsersInfo[users.Count];

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = new UsersInfo { Name = users[i].FullName, Rating = users[i].Rating, PhoneNumber = users[i].PhoneNumber, Email = users[i].Email, IsInBlackList = users[i].IsInBlacklist };
                }

                return result;
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "GET Method");
                return new BadRequestObjectResult(e.Message);
            }
        }

        public async Task<ActionResult<UsersInfo[]>> GetByNameAsync(string value)
        {
            try
            {
                var users = await _context.Users.Where(u => u.FullName.Contains(value)).ToListAsync();
                UsersInfo[] result = new UsersInfo[users.Count];

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = new UsersInfo { Name = users[i].FullName, Rating = users[i].Rating, PhoneNumber = users[i].PhoneNumber, Email = users[i].Email, IsInBlackList = users[i].IsInBlacklist };
                }
                return result;
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "GET Method (by value)");
                return new BadRequestObjectResult(e.Message);
            }
        }

        public async Task<IActionResult> UpdateAsync(UsersInfo usersInfo)
        {
            bool check = UsersInputValidation.UsersValuesValidation(usersInfo, out string errorMessage);
            if (!check) return new BadRequestObjectResult(errorMessage);

            try
            {
                var user = _context.Users.FirstOrDefault(c => c.FullName.Contains(usersInfo.Name));
                if (user != null)
                {
                    if (!string.IsNullOrWhiteSpace(usersInfo.PhoneNumber)) user.PhoneNumber = usersInfo.PhoneNumber;
                    if (!string.IsNullOrWhiteSpace(usersInfo.Email)) user.Email = usersInfo.Email;
                    await _context.SaveChangesAsync();
                    return new OkResult();
                }

                return new NoContentResult();
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "PUT method");
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
