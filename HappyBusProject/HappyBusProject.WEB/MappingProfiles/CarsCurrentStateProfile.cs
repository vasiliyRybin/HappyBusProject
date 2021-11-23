using AutoMapper;
using HappyBusProject.HappyBusProject.DataLayer.InputModels;
using HappyBusProject.HappyBusProject.DataLayer.InputModels.CarStateModels;
using HappyBusProject.HappyBusProject.DataLayer.Models;
using HappyBusProject.HappyBusProject.DataLayer.ViewModels;
using HappyBusProject.ModelsToReturn;

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
            CreateMap<CarStateInputModel, CarsCurrentState>()
                .ForMember(dest => dest.IsBusyNow, opt => opt.MapFrom(src => 0));
            CreateMap<CarsCurrentState, CarStateViewModel>();
            CreateMap<CarStateInputModel, CarStateViewModel>();
            CreateMap<CarStatePostModel, CarsCurrentState>();
            CreateMap<Driver, CarsCurrentState>();
        }
    }
}
