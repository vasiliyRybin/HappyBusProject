using AutoMapper;
using HappyBusProject.HappyBusProject.DataLayer.InputModels;
using HappyBusProject.HappyBusProject.DataLayer.InputModels.OrdersInputModels;
using HappyBusProject.HappyBusProject.DataLayer.InputValidators;
using HappyBusProject.HappyBusProject.DataLayer.ViewModels;
using HappyBusProject.HappyBusProject.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                var result = _mapper.Map<OrderViewModel>(orders);
                return new OkObjectResult(result);
            }

            return new NoContentResult();
        }

        public async Task<IActionResult> GetByNameAsync(string FullName)
        {
            var customer = await _repository.Users.FirstOrDefaultAsync(u => u.FullName == FullName);

            if (customer != null)
            {
                var order = _repository.Orders.FirstOrDefault(c => c.CustomerId == customer.Id);
                var result = _mapper.Map<OrderViewModel>(order);
                return new OkObjectResult(result);
            }

            return new NotFoundResult();
        }

        public async Task<IActionResult> CreateOrder(OrderInputModel orderInput)
        {            
            var check = OrderInputValidation.OrderValuesValidation(_repository, orderInput, out string errorMessage);

            RetrieveDataForCreatingOrder(orderInput, out Guid carIDReadyToOrder, out Guid whoOrdered, out int availableSeatsNum);

            if (check)
            {
                if (availableSeatsNum < orderInput.OrderSeatsNum) return new BadRequestObjectResult("Attempt to order more seats than available");
                var startPointKM = GetLengthKM(_repository, orderInput.StartPoint);
                var endPointKM = GetLengthKM(_repository, orderInput.EndPoint);

                double totalPrice = Math.Round(startPointKM > endPointKM ? (startPointKM - endPointKM) * 0.065 : (endPointKM - startPointKM) * 0.065);

                try
                {
                    var order = _mapper.Map<Order>(orderInput);
                    AssignValuesToOrder(order, _repository, carIDReadyToOrder, whoOrdered, orderInput, totalPrice);

                    var view = _mapper.Map<OrderViewModel>(order);
                    _mapper.Map(orderInput, view);


                    _repository.CarCurrentStates.First(c => c.Id == carIDReadyToOrder).FreeSeatsNum -= orderInput.OrderSeatsNum;
                    await _repository.Orders.AddAsync(order);
                    _repository.SaveChanges();
                    
                    return new OkObjectResult(view);
                }
                catch (Exception e)
                {
                    LogWriter.ErrorWriterToFile(e.Message + " " + e.InnerException + " " + "POST Method, Orders Repository");
                    return new UnprocessableEntityObjectResult(e.Message + " " + e.InnerException);
                }
            }
            else return new BadRequestObjectResult(errorMessage);
        }


        /// <summary>
        /// This method updates last active user's order
        /// </summary>
        /// <param name="FullName"></param>
        public void UpdateOrder(OrdersInputModelPutMethod putModel)
        {
            var user = _repository.Users.FirstOrDefault(u => u.FullName == putModel.FullName);

            if (user != null)
            {
                PutMethodMyMapper(_repository, putModel, user);
            }
        }

        /// <summary>
        /// This method deletes(cancels) last active user's order
        /// </summary>
        /// <param name="FullName"></param>
        public void DeleteOrder(string FullName)
        {

        }

        #region RetrieveDataForCreatingOrder

        private void RetrieveDataForCreatingOrder(OrderInputModel orderInput, out Guid carIDReadyToOrder, out Guid whoOrdered, out int availableSeatsNum)
        {
            var carID = GetDriver(orderInput.DesiredDepartureTime);
            if (carID != Guid.Empty)
            {
                availableSeatsNum = _repository.CarCurrentStates.FirstOrDefault(c => c.Id == carID).FreeSeatsNum;
                whoOrdered = _repository.Users.FirstOrDefaultAsync(u => u.FullName == orderInput.FullName).Result.Id;                
                carIDReadyToOrder = carID;
                return;
            }

            availableSeatsNum = default;
            whoOrdered = default;
            carIDReadyToOrder = carID;
        }

        private Guid GetDriver(DateTime desiredDepartureDateTime)
        {
            var freeCar = _repository.CarCurrentStates.FirstOrDefault(c => c.DepartureTime.Equals(desiredDepartureDateTime));

            if (freeCar != null)
            {
                return freeCar.Id;
            }
            return Guid.Empty;
        }

        #endregion

        #region AssignValuesToOrder
        private static void AssignValuesToOrder(Order order, MyShuttleBusAppNewDBContext _repository, Guid carIDReadyToOrder, Guid whoOrdered, OrderInputModel orderInput, double totalPrice)
        {
            order.StartPointId = GetPointID(_repository, orderInput.StartPoint);
            order.EndPointId = GetPointID(_repository, orderInput.EndPoint);
            order.CarId = carIDReadyToOrder;
            order.CustomerId = whoOrdered;
            order.Id = Guid.NewGuid();
            order.OrderDateTime = DateTime.Now;
            order.OrderType = orderInput.OrderType.ToString();
            order.IsActual = true;
            order.TotalPrice = totalPrice;
        }

        #endregion

        #region MyMapper

        private static void PutMethodMyMapper(MyShuttleBusAppNewDBContext _repository, OrdersInputModelPutMethod putModel, User user)
        {
            Order order = _repository.Orders.OrderByDescending(o => o.OrderDateTime).First(o => o.CustomerId == user.Id);
            var inputPropertiesArr = putModel.GetType().GetProperties();

            try
            {
                foreach (var item in inputPropertiesArr)
                {
                    var PropertyName = item.Name;
                    if (PropertyName == nameof(putModel.FullName)) continue;
                    var fieldValue = item.GetValue(putModel);
                    if (fieldValue is null || string.IsNullOrWhiteSpace(fieldValue.ToString())) continue;
                    if(order is null) order = _repository.Orders.OrderByDescending(o => o.OrderDateTime).First(o => o.CustomerId == user.Id);

                    if (PropertyName.Contains("Point"))
                    {
                        var pointID = GetPointID(_repository, fieldValue.ToString());
                        var pointProperty = order.GetType().GetProperty(PropertyName + "Id");
                        pointProperty.SetValue(order, pointID);
                        continue;
                    }

                    var property = order.GetType().GetProperty(PropertyName);
                    property.SetValue(order, fieldValue);
                }

                _repository.Update(order);
                _repository.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion


        private static int GetPointID(MyShuttleBusAppNewDBContext _repository, string pointName)
        {
            return _repository.RouteStops.FirstOrDefault(c => c.Name == pointName).PointId;
        }

        private static int GetLengthKM(MyShuttleBusAppNewDBContext _repository, string pointName)
        {
            return _repository.RouteStops.FirstOrDefault(c => c.Name == pointName).RouteLengthKM;
        }
    }
}
