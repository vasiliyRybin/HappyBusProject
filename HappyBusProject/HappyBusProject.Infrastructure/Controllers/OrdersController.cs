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
        public OrdersController(MyShuttleBusAppNewDBContext myShuttleBusAppNewDBContext)
        {
            _context = myShuttleBusAppNewDBContext;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }


        [HttpPost("{userName}/{startPoint}/{endPoint}")]
        public string Post(string userName, string startPoint, string endPoint, string orderType)
        {
            var freeCars = _context.Cars.Where(c => c.IsBusyNow != true).ToArray();
            var randomCar = new Random().Next(0, freeCars.Length - 1);
            var carIDReadyToOrder = freeCars[randomCar].Id;
            var whoOrdered = _context.Users.First(u => u.FullName == userName).Id;
            var startPointKM = _context.RouteStops.First(c => c.Name == startPoint).RouteLengthKM;
            var endPointKM = _context.RouteStops.First(c => c.Name == endPoint).RouteLengthKM;
            double totalPrice = Math.Round(startPointKM > endPointKM ? (startPointKM - endPointKM) * 0.065 : (endPointKM - startPointKM) * 0.065);

            try
            {
                Order order = new()
                {
                    CarId = carIDReadyToOrder,
                    CustomerId = whoOrdered,
                    Id = Guid.NewGuid(),
                    OrderDateTime = DateTime.Now,
                    OrderType = string.IsNullOrWhiteSpace(orderType) ? "MobileApp" : orderType,
                    StartPointId = _context.RouteStops.First(c => c.Name == startPoint).PointId,
                    EndPointId = _context.RouteStops.First(c => c.Name == endPoint).PointId,
                    TotalPrice = totalPrice
                };

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

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
