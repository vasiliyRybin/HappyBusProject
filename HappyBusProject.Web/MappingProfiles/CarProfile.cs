using AutoMapper;
using HappyBusProject.InputModels;

namespace HappyBusProject.HappyBusProject.DataLayer.Profiles
{
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            CreateMap<PutMethodDriverInputModel, Car>();
            CreateMap<DriverCarInputModel, Car>();
        }
    }
}
