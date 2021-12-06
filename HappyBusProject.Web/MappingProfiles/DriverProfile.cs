using AutoMapper;
using HappyBusProject.InputModels;
using HappyBusProject.ViewModels;

namespace HappyBusProject.HappyBusProject.DataLayer.MappingProfiles
{
    public class DriverProfile : Profile
    {
        public DriverProfile()
        {
            CreateMap<Car, DriverViewModel>();
            CreateMap<Driver, DriverViewModel>();
            CreateMap<DriverCarInputModel, Driver>();
            CreateMap<PutMethodDriverInputModel, Driver>();
        }
    }
}
