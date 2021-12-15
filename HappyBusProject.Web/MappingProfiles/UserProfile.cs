using AutoMapper;
using HappyBusProject.InputModels;
using HappyBusProject.ViewModels;

namespace HappyBusProject.MappingProfiles
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
