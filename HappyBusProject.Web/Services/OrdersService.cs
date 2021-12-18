using AutoMapper;
using HappyBusProject.HappyBusProject.DataLayer.InputValidators;
using HappyBusProject.HappyBusProject.DataLayer.Methods;
using HappyBusProject.InputModels;
using HappyBusProject.Interfaces;
using HappyBusProject.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.Services
{
    public class OrdersService
    {
        public IRepository<Order> OrderRepo { get; }
        private IRepository<RouteStop> RouteRepo { get; }
        private IRepository<User> UserRepo { get; }
        private IRepository<CarsCurrentState> CurrentStateRepo { get; }
        private readonly ILogger _logger;
        public IMapper Mapper { get; }

        public OrdersService(IRepository<Order> orRepository, IRepository<RouteStop> _routeRepo, IRepository<User> _userRepo, IRepository<CarsCurrentState> _currRepo, ILogger<OrdersService> logger, IMapper mapper)
        {
            OrderRepo = orRepository;
            RouteRepo = _routeRepo;
            UserRepo = _userRepo;
            CurrentStateRepo = _currRepo;
            Mapper = mapper;
            _logger = logger;
        }

        public async Task<OrderViewModel[]> GetAllOrdersAsync()
        {
            try
            {
                var orders = await OrderRepo.Get();

                if (orders.Any())
                {
                    var ordersArr = orders.ToArray();
                    var result = new OrderViewModel[orders.Count()];

                    for (int i = 0; i < result.Length; i++)
                    {
                        var customer = await UserRepo.GetFirstOrDefault(c => c.Id == ordersArr[i].CustomerId);
                        var startPoint = await RouteRepo.GetFirstOrDefault(o => o.PointId == ordersArr[i].StartPointId);
                        var endPoint = await RouteRepo.GetFirstOrDefault(o => o.PointId == ordersArr[i].EndPointId);

                        result[i] = Mapper.Map<OrderViewModel>(ordersArr[i]);
                        result[i].StartPoint = startPoint.Name;
                        result[i].EndPoint = endPoint.Name;
                        result[i].CustomerName = customer.FullName;
                    }

                    return result;
                }

                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString() + "\t" + "OrdersService");
                return null;
            }
        }

        public async Task<OrderViewModel> GetOrderByCustomerNameAsync(string FullName)
        {
            var customer = await UserRepo.GetFirstOrDefault(u => u.FullName == FullName);

            if (customer != null)
            {
                try
                {
                    var order = OrderRepo.Get().Result.OrderByDescending(o => o.OrderDateTime).FirstOrDefault(c => c.CustomerId == customer.Id);
                    if (order != null)
                    {
                        var result = Mapper.Map<OrderViewModel>(order);
                        result.StartPoint = RouteRepo.GetFirstOrDefault(o => o.PointId == order.StartPointId).Result.Name;
                        result.EndPoint = RouteRepo.GetFirstOrDefault(o => o.PointId == order.EndPointId).Result.Name;
                        result.CustomerName = customer.FullName;
                        return result;
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString() + "\t" + "OrdersService");
                    return null;
                }
            }

            return null;
        }

        public async Task<OrderViewModel> CreateOrder(OrderInputModel orderInput)
        {
            var CustomerGUID = UserRepo.GetFirstOrDefault(u => u.FullName == orderInput.FullName).Result.Id;
            var startPoint = RouteRepo.GetFirstOrDefault(r => r.Name == orderInput.StartPoint).Result.Name;
            var endPoint = RouteRepo.GetFirstOrDefault(r => r.Name == orderInput.EndPoint).Result.Name;

            var check = OrderInputValidation.OrderValuesValidation(CustomerGUID, startPoint, endPoint, orderInput, out string errorMessage);

            if (check)
            {
                var freeCar = await CurrentStateRepo.GetFirstOrDefault(c => c.DepartureTime.Equals(orderInput.DesiredDepartureTime));
                if (freeCar.FreeSeatsNum < orderInput.OrderSeatsNum) return null;
                var startPointKM = RouteRepo.GetLengthKM(orderInput.StartPoint);
                var endPointKM = RouteRepo.GetLengthKM(orderInput.EndPoint);

                try
                {
                    var order = Mapper.Map<Order>(orderInput);
                    double totalPrice = OrderMethods.CountTotalPrice(startPointKM, endPointKM, order.OrderSeatsNum);

                    OrderMethods.AssignValuesToOrder(order, RouteRepo, freeCar.Id, CustomerGUID, orderInput, totalPrice);

                    var viewResult = Mapper.Map<OrderViewModel>(order);
                    Mapper.Map(orderInput, viewResult);
                    viewResult.CustomerName = orderInput.FullName;


                    var currentcar = await CurrentStateRepo.GetFirstOrDefault(c => c.Id == freeCar.Id);
                    currentcar.FreeSeatsNum -= orderInput.OrderSeatsNum;
                    var updateResult = await CurrentStateRepo.Update(currentcar);

                    if (updateResult)
                    {
                        bool createOrderResult = await OrderRepo.Create(order);
                        if (createOrderResult) return viewResult;
                    }

                    return null;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString() + "\t" + "OrdersService");
                    return null;
                }
            }

            return null;
        }


        public async Task<OrderViewModel> UpdateOrder(OrderInputModelPutMethod putModel)
        {
            try
            {
                var user = await UserRepo.GetFirstOrDefault(u => u.FullName == putModel.FullName);

                if (user != null)
                {
                    var startPoint = RouteRepo.GetFirstOrDefault(r => r.Name == putModel.StartPoint).Result.Name;
                    var endPoint = RouteRepo.GetFirstOrDefault(r => r.Name == putModel.EndPoint).Result.Name;

                    var check = OrderInputValidation.OrderValuesValidation(user.Id, startPoint, endPoint, (OrderInputModel)putModel, out string _);

                    if (check)
                    {
                        Order order = OrderRepo.Get().Result.OrderByDescending(o => o.OrderDateTime).First(o => o.CustomerId == user.Id && o.IsActual);
                        OrderMethods.PutMethodMyMapper(OrderRepo, RouteRepo, putModel, order, user, out bool isPointModified);

                        if (isPointModified)
                        {
                            var startPointKM = RouteRepo.GetLengthKM(order.StartPointId);
                            var endPointKM = RouteRepo.GetLengthKM(order.EndPointId);
                            order.TotalPrice = OrderMethods.CountTotalPrice(startPointKM, endPointKM, order.OrderSeatsNum);
                            order.OrderDateTime = DateTime.Now;


                            var updateResult = await OrderRepo.Update(order);

                            if (updateResult)
                            {
                                var viewUpdatedResult = Mapper.Map<OrderViewModel>(order);
                                viewUpdatedResult.CustomerName = user.FullName;
                                viewUpdatedResult.StartPoint = RouteRepo.GetPointName(order.StartPointId);
                                viewUpdatedResult.EndPoint = RouteRepo.GetPointName(order.EndPointId);
                                return viewUpdatedResult;
                            }
                        }

                        var viewResult = Mapper.Map<OrderViewModel>(order);
                        viewResult.CustomerName = user.FullName;
                        viewResult.StartPoint = RouteRepo.GetPointName(order.StartPointId);
                        viewResult.EndPoint = RouteRepo.GetPointName(order.EndPointId);
                        return viewResult;
                    }
                }

                return null;
            }
            catch (Exception e) 
            {
                _logger.LogError(e.ToString() + "\t" + "OrdersService");
                return null;
            }            
        }

        public async Task<bool> DeleteOrder(string FullName)
        {
            try
            {
                var user = await UserRepo.GetFirstOrDefault(u => u.FullName == FullName);

                if (user != null)
                {
                    Order order = OrderRepo.Get().Result.OrderByDescending(o => o.OrderDateTime).First(o => o.CustomerId == user.Id && o.IsActual);

                    if (order != null)
                    {
                        var removeResult = await OrderRepo.Delete(order);
                        if (removeResult) return true;
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString() + "\t" + "OrdersService");
                return false;
            }
            
        }

    }
}
