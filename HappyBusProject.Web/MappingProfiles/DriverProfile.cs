using AutoMapper;
using HappyBusProject.InputModels;
using HappyBusProject.ViewModels;
using System;

namespace HappyBusProject.HappyBusProject.DataLayer.MappingProfiles
{
    public class DriverProfile : Profile
    {
        public DriverProfile()
        {
            CreateMap<Car, DriverViewModel>();
            CreateMap<Driver, DriverViewModel>();
            CreateMap<DriverCarInputModel, Car>()
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => Guid.NewGuid()));
            CreateMap<DriverCarInputModel, Driver>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => 5.0));
        }
    }
}
