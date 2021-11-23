using AutoMapper;
using HappyBusProject.HappyBusProject.DataLayer.InputModels;
using HappyBusProject.ModelsToReturn;

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
