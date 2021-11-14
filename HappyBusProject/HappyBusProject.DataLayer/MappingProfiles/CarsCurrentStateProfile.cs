using AutoMapper;
using HappyBusProject.HappyBusProject.DataLayer.Models;
using HappyBusProject.ModelsToReturn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.HappyBusProject.DataLayer.MappingProfiles
{
    public class CarsCurrentStateProfile : Profile
    {
        public CarsCurrentStateProfile()
        {
            CreateMap<DriverCarInputModel, CarsCurrentState>()
                .ForMember(dest => dest.IsBusyNow, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.FreeSeatsNum, opt => opt.MapFrom(src => src.SeatsNum))
                .ForMember(dest => dest.SeatsNum, opt => opt.MapFrom(src => src.SeatsNum));
            CreateMap<Car, CarsCurrentState>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CarId));
        }
    }
}
