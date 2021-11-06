using AutoMapper;
using HappyBusProject.HappyBusProject.DataLayer.InputModels;
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

        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _repository.Orders.ToListAsync();
            return new ObjectResult(orders);
        }

        public async Task<IActionResult> GetLastOrder([FromQuery]string FullName)
        {
            var customerID = _repository.Users.FirstOrDefault(u => u.FullName == FullName).Id;
            if (customerID != Guid.Empty)
            {
                var order = await _repository.Orders.Where(o => o.CustomerId == customerID).OrderByDescending(d => d.OrderDateTime).FirstAsync();
                return new ObjectResult(order);
            }

            return new NotFoundResult();
        }

        public async Task<IActionResult> CreateOrder(OrderInputModel orderInput)
        {
            var freeCars = await _repository.CarCurrentStates.Where(c => c.IsBusyNow != true).ToListAsync();
            var randomCar = new Random().Next(0, freeCars.Count - 1);
            var carIDReadyToOrder = freeCars[randomCar].Id;
            var whoOrdered = _repository.Users.First(u => u.FullName == orderInput.FullName).Id;
            var startPointKM = _repository.RouteStops.First(c => c.Name == orderInput.StartPoint).RouteLengthKM;
            var endPointKM = _repository.RouteStops.First(c => c.Name == orderInput.EndPoint).RouteLengthKM;
            double totalPrice = Math.Round(startPointKM > endPointKM ? (startPointKM - endPointKM) * 0.065 : (endPointKM - startPointKM) * 0.065);

            try
            {
                var order = _mapper.Map<Order>(orderInput);
                order.CarId = carIDReadyToOrder;
                order.CustomerId = whoOrdered;
                order.Id = Guid.NewGuid();
                order.OrderDateTime = DateTime.Now;
                order.OrderType = string.IsNullOrWhiteSpace(orderInput.OrderType) ? "MobileApp" : orderInput.OrderType;

                _repository.Orders.Add(order);
                _repository.SaveChanges();
                return new OkObjectResult(totalPrice);
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "POST Method");
                return new ObjectResult(e.Message);
            }
        }

        public void UpdateOrder(string FullName)
        {
            
        }

        public void DeleteOrder(string FullName)
        {
            
        }
    }
}
