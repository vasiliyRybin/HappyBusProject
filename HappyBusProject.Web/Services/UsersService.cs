using AutoMapper;
using HappyBusProject.InputModels;
using HappyBusProject.Interfaces;
using HappyBusProject.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.Services
{
    public class UsersService
    {
        private readonly IMapper _mapper;
        readonly IRepository<User> _usRepository;
        private ILogger Logger { get; }

        public UsersService(IRepository<User> repository, IMapper mapper, ILogger<UsersService> logger)
        {
            _mapper = mapper;
            _usRepository = repository;
            Logger = logger;
        }

        public async Task<UsersViewModel> GetByNameAsync(string name)
        {
            try
            {
                User user = await Task.Run(() => _usRepository.GetFirstOrDefault(x => x.FullName == name));
                if (user != null)
                {
                    var userView = _mapper.Map<UsersViewModel>(user);
                    return userView;
                }

                return null;

            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString() + "\t" + "UsersService");
                return null;
            }
        }

        public async Task<UsersViewModel[]> GetAllUsers()
        {
            try
            {
                var users = await _usRepository.Get();
                if (users != null)
                {
                    var result = _mapper.Map<UsersViewModel[]>(users.ToList());
                    return result;
                }

                return null;
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString() + "\t" + "UsersService");
                return null;
            }
        }

        public async Task<UsersViewModel> CreateAsync(UserInputModel InputUser)
        {
            const double UserDefaultRating = 5.0;

            try
            {
                UsersInputValidation.AssignEmptyStringsToNullValues(InputUser);
                var check = UsersInputValidation.UsersValuesValidation(InputUser, out _);

                if (check)
                {
                    var user = _mapper.Map<User>(InputUser);
                    user.Id = Guid.NewGuid();
                    user.Rating = UserDefaultRating;
                    user.RegistrationDateTime = DateTime.Now;

                    var result = await _usRepository.Create(user);

                    if (result)
                    {
                        var output = _mapper.Map<UsersViewModel>(user);
                        return output;
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString() + "\t" + "UsersService");
                return null;
            }

        }

        public async Task<bool> UpdateUserInfo(UserInputModel InputUser)
        {
            try
            {
                UsersInputValidation.AssignEmptyStringsToNullValues(InputUser);
                var check = UsersInputValidation.UsersValuesValidation(InputUser, out _);

                if (check)
                {
                    var user = await _usRepository.GetFirstOrDefault(u => u.FullName == InputUser.FullName);

                    if (user != null)
                    {
                        _mapper.Map(InputUser, user);
                        var result = await _usRepository.Update(user);
                        if (result) return true;
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString() + "\t" + "UsersService");
                return false;
            }

        }

        public async Task<bool> DeleteUser(string Name)
        {
            try
            {
                var userToRemove = await _usRepository.GetFirstOrDefault(u => u.FullName == Name);

                if (userToRemove != null)
                {
                    var result = await _usRepository.Delete(userToRemove);

                    if (result)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString() + "\t" + "UsersService");
                return false;
            }
        }
    }
}
