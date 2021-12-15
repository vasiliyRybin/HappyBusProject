using AutoMapper;
using HappyBusProject.InputModels;

namespace HappyBusProject.MappingProfiles
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
