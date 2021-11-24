using AutoMapper;
using HappyBusProject.InputModels;
using HappyBusProject.ViewModels;

namespace HappyBusProject.HappyBusProject.DataLayer.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UsersViewModel>();
            CreateMap<UserInputModel, User>();
        }
    }
}
