using AutoMapper;
using HappyBusProject.InputModels;
using HappyBusProject.ViewModels;

namespace HappyBusProject.MappingProfiles
{
    public class DriverProfile : Profile
    {
        public DriverProfile()
        {
            CreateMap<Car, DriverViewModel>();
            CreateMap<Driver, DriverViewModel>();
            CreateMap<DriverCarInputModel, Driver>();
            CreateMap<PutMethodDriverInputModel, Driver>();
            CreateMap<Driver, Car>();
        }
    }
}
