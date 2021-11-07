using AutoMapper;
using HappyBusProject.HappyBusProject.DataLayer.InputModels;
using HappyBusProject.ModelsToReturn;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.Repositories
{
    public class UsersRepository : IUsersRepository<IActionResult>
    {
        private readonly MyShuttleBusAppNewDBContext _context;
        private readonly IMapper _mapper;

        public UsersRepository(MyShuttleBusAppNewDBContext myShuttleBusAppNewDBContext, IMapper mapper)
        {
            _context = myShuttleBusAppNewDBContext ?? throw new ArgumentNullException(nameof(myShuttleBusAppNewDBContext));
            _mapper = mapper;
        }

        public async Task<IActionResult> CreateAsync(UserInputModel usersInfo)
        {
            var check = UsersInputValidation.UsersValuesValidation(usersInfo, out string errorMessage);

            if (!check) return new BadRequestObjectResult(errorMessage);

            try
            {
                var user = _mapper.Map<User>(usersInfo);
                user.Id = Guid.NewGuid();
                user.RegistrationDateTime = DateTime.Now;
                user.Rating = 5.0;
                user.IsInBlacklist = false;

                await _context.Users.AddAsync(user);
                int successUpdate = _context.SaveChanges();
                if (successUpdate > 0) return new OkObjectResult(user);
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

                return new NotFoundResult();

            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "DELETE method");
                return new BadRequestObjectResult(e.Message);
            }
        }

        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var users = await _context.Users.ToListAsync();

                if (users.Count != 0 && users != null)
                {
                    UsersViewModel[] result = new UsersViewModel[users.Count];
                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = _mapper.Map<UsersViewModel>(users[i]);
                    }

                    return new OkObjectResult(result);
                }

                return new NoContentResult();                
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "GET Method");
                return new BadRequestObjectResult(e.Message);
            }
        }

        public async Task<IActionResult> GetByNameAsync(string value)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.FullName.Contains(value));
                if (user != null)
                {
                    var userResult = _mapper.Map<UsersViewModel>(user);

                    return new OkObjectResult(userResult);
                }

                return new NotFoundResult();
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "GET Method (by value)");
                return new BadRequestObjectResult(e.Message);
            }
        }

        public async Task<IActionResult> UpdateAsync(UserInputModel usersInfo)
        {
            bool check = UsersInputValidation.UsersValuesValidation(usersInfo, out string errorMessage);
            if (!check) return new BadRequestObjectResult(errorMessage);

            try
            {
                var user = _context.Users.FirstOrDefault(c => c.FullName.Contains(usersInfo.FullName));
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
