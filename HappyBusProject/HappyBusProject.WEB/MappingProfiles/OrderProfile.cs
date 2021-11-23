using AutoMapper;
using HappyBusProject.HappyBusProject.DataLayer.InputModels;
using HappyBusProject.HappyBusProject.DataLayer.InputModels.OrdersInputModels;
using HappyBusProject.HappyBusProject.DataLayer.ViewModels;

namespace HappyBusProject.HappyBusProject.DataLayer.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderInputModel, Order>();
            CreateMap<OrderInputModel, OrderViewModel>();
            CreateMap<Order, OrderViewModel>();
            CreateMap<OrderViewModel, Order>();
            CreateMap<OrderInputModelPutMethod, Order>();
        }
    }
}
