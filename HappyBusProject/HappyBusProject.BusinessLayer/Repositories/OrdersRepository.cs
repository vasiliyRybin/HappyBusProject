using AutoMapper;
using HappyBusProject.HappyBusProject.DataLayer.InputModels;
using HappyBusProject.HappyBusProject.DataLayer.InputValidators;
using HappyBusProject.HappyBusProject.DataLayer.ViewModels;
using HappyBusProject.HappyBusProject.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.HappyBusProject.BusinessLayer.Repositories
{
    public class OrdersRepository : IOrderRepository<IActionResult>
    {
        private readonly MyShuttleBusAppNewDBContext _repository;
        private readonly IMapper _mapper;

        public OrdersRepository(MyShuttleBusAppNewDBContext myShuttleBusAppNewDBContext, IMapper mapper)
        {
            _repository = myShuttleBusAppNewDBContext ?? throw new ArgumentNullException(nameof(myShuttleBusAppNewDBContext));
            _mapper = mapper;
        }

        public async Task<IActionResult> GetAllAsync()
        {
            var orders = await _repository.Orders.ToListAsync();

            if (orders.Count != 0)
            {
                return new OkObjectResult(orders);
            }

            return new NoContentResult();
        }

        public async Task<IActionResult> GetByNameAsync(string FullName)
        {
            var customer = _repository.Users.FirstOrDefault(u => u.FullName == FullName);
            if (customer != null)
            {
                var customerID = customer.Id;
                var order = await _repository.Orders.Where(o => o.CustomerId == customerID).OrderByDescending(d => d.OrderDateTime).FirstAsync();
                return new ObjectResult(order);
            }

            return new NotFoundResult();
        }

        public async Task<IActionResult> CreateOrder(OrderInputModel orderInput)
        {
            var checkDateTime = DateTime.TryParse(orderInput.DesiredDepartureTime, out DateTime DesiredDepartureTime);

            if (!checkDateTime) return new BadRequestObjectResult("Incorrect DateTime format");

            RetrieveDataForCreatingOrder(orderInput, DesiredDepartureTime, out Guid carIDReadyToOrder, out Guid whoOrdered, out RouteStop startPoint, out RouteStop endPoint, out int availableSeatsNum);
            var check = OrderInputValidation.OrderValuesValidation(orderInput, whoOrdered, startPoint, endPoint, out string errorMessage);
            
            if (check)
            {
                if (availableSeatsNum < orderInput.OrderSeatsNum) return new BadRequestObjectResult("Attempt to order more seats than available");
                var startPointKM = startPoint.RouteLengthKM;
                var endPointKM = endPoint.RouteLengthKM;

                double totalPrice = Math.Round(startPointKM > endPointKM ? (startPointKM - endPointKM) * 0.065 : (endPointKM - startPointKM) * 0.065);

                try
                {
                    var order = _mapper.Map<Order>(orderInput);
                    AssignValuesToOrder(order, startPoint, endPoint, carIDReadyToOrder, whoOrdered, orderInput, DesiredDepartureTime);

                    var view = _mapper.Map<OrderViewModel>(order);
                    _mapper.Map(orderInput, view);
                    view.TotalPrice = totalPrice;

                    await _repository.Orders.AddAsync(order);
                    _repository.SaveChanges();
                    
                    return new ObjectResult(view);
                }
                catch (Exception e)
                {
                    LogWriter.ErrorWriterToFile(e.Message + " " + "POST Method, Orders Repository");
                    return new ObjectResult(e.Message);
                }
            }
            else return new BadRequestObjectResult(errorMessage);
        }

        public void UpdateOrder(string FullName)
        {
            
        }

        public void DeleteOrder(string FullName)
        {
            
        }

        private void AssignValuesToOrder(Order order, RouteStop startPoint, RouteStop endPoint, Guid carIDReadyToOrder, Guid whoOrdered, OrderInputModel orderInput, DateTime DesiredDepartureTime)
        {
            order.StartPointId = startPoint.PointId;
            order.EndPointId = endPoint.PointId;
            order.CarId = carIDReadyToOrder;
            order.CustomerId = whoOrdered;
            order.Id = Guid.NewGuid();
            order.OrderDateTime = DateTime.Now;
            order.OrderType = orderInput.OrderType.ToString();
            order.IsActual = true;
            order.DesiredDepartureTime = DesiredDepartureTime;
        }

        private void RetrieveDataForCreatingOrder(OrderInputModel orderInput, DateTime DesiredDepartureTime, out Guid carIDReadyToOrder, out Guid whoOrdered, out RouteStop startPoint, out RouteStop endPoint, out int availableSeatsNum)
        {
            var carID = GetDriver(DesiredDepartureTime);
            if (carID != Guid.Empty)
            {
                availableSeatsNum = _repository.CarCurrentStates.FirstOrDefault(c => c.Id == carID).FreeSeatsNum;
                whoOrdered = _repository.Users.FirstOrDefaultAsync(u => u.FullName == orderInput.FullName).Result.Id;
                startPoint = _repository.RouteStops.FirstOrDefault(c => c.Name == orderInput.StartPoint);
                endPoint = _repository.RouteStops.FirstOrDefault(c => c.Name == orderInput.EndPoint);
                carIDReadyToOrder = carID;
            }

            availableSeatsNum = default;
            whoOrdered = default;
            startPoint = default;
            endPoint = default;
            carIDReadyToOrder = carID;
        }

        private Guid GetDriver(DateTime desiredDepartureDateTime)
        {
            var freeCar = _repository.CarCurrentStates.FirstOrDefault(c => c.DepartureTime == desiredDepartureDateTime);

            if (freeCar != null)
            {
                return freeCar.Id;
            }
            return Guid.Empty;
        }
    }
}
