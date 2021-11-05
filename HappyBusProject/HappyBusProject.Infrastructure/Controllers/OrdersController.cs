using AutoMapper;
using HappyBusProject.HappyBusProject.DataLayer.InputModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HappyBusProject.Controllers
{
    [Route("AppAPI/Orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly MyShuttleBusAppNewDBContext _context;
        private readonly IMapper _mapper;
        public OrdersController(MyShuttleBusAppNewDBContext myShuttleBusAppNewDBContext, IMapper mapper)
        {
            _context = myShuttleBusAppNewDBContext;
            _mapper = mapper;
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }


        [HttpPost]
        public string Post([FromBody]OrderInputModel orderInput)
        {
            var freeCars = _context.Cars.Where(c => c.IsBusyNow != true).ToArray();
            var randomCar = new Random().Next(0, freeCars.Length - 1);
            var carIDReadyToOrder = freeCars[randomCar].Id;
            var whoOrdered = _context.Users.First(u => u.FullName == orderInput.FullName).Id;
            var startPointKM = _context.RouteStops.First(c => c.Name == orderInput.StartPoint).RouteLengthKM;
            var endPointKM = _context.RouteStops.First(c => c.Name == orderInput.EndPoint).RouteLengthKM;
            double totalPrice = Math.Round(startPointKM > endPointKM ? (startPointKM - endPointKM) * 0.065 : (endPointKM - startPointKM) * 0.065);

            try
            {
                var order = _mapper.Map<Order>(orderInput);
                
                //Order order = new()
                //{
                //    CarId = carIDReadyToOrder,
                //    CustomerId = whoOrdered,
                //    Id = Guid.NewGuid(),
                //    OrderDateTime = DateTime.Now,
                //    OrderType = string.IsNullOrWhiteSpace(orderInput.OrderType) ? "MobileApp" : orderInput.OrderType,
                //    StartPointId = _context.RouteStops.First(c => c.Name == orderInput.StartPoint).PointId,
                //    EndPointId = _context.RouteStops.First(c => c.Name == orderInput.EndPoint).PointId,
                //    TotalPrice = totalPrice
                //};

                _context.Orders.Add(order);
                _context.SaveChanges();
                return $"Order successfully created. Total price is { totalPrice } ";
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "POST Method");
                return e.Message;
            }
        }

        
        [HttpPut("{id}")]
        public void Put(int id, string value)
        {

        }

        
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}
