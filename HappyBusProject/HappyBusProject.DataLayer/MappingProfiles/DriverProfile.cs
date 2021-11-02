using AutoMapper;
using HappyBusProject.ModelsToReturn;
using System;

namespace HappyBusProject.HappyBusProject.DataLayer.MappingProfiles
{
    public class DriverProfile : Profile
    {
        public DriverProfile()
        {
            CreateMap<Driver, DriverViewModel>();
            CreateMap<DriverCarInputModel, Car>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.CarBrand))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.CarAge))
                .ForMember(dest => dest.SeatsNum, opt => opt.MapFrom(src => src.SeatsNum))
                .ForMember(dest => dest.RegistrationNumPlate, opt => opt.MapFrom(src => src.RegistrationNumPlate))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.IsBusyNow, opt => opt.MapFrom(src => 0));
            //CreateMap<DriverCarInputModel, Driver>()
            //    .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => Guid.NewGuid()))
            //    .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => Guid.NewGuid()))
        }
    }
}
