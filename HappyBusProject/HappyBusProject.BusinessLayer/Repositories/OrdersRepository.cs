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
                var result = new OrderViewModel[orders.Count];

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = _mapper.Map<OrderViewModel>(orders[i]);
                    result[i].StartPoint = GetPointName(_repository, orders[i].StartPointId);
                    result[i].EndPoint = GetPointName(_repository, orders[i].EndPointId);
                }

                return new OkObjectResult(result);
            }

            return new NoContentResult();
        }


        /// <summary>
        /// Get last order by input name
        /// </summary>
        /// <param name="FullName"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetByNameAsync(string FullName)
        {
            var customer = await _repository.Users.FirstOrDefaultAsync(u => u.FullName == FullName);

            if (customer != null)
            {
                var order = _repository.Orders.OrderByDescending(o => o.OrderDateTime).FirstOrDefault(c => c.CustomerId == customer.Id);
                if (order != null)
                {
                    var result = _mapper.Map<OrderViewModel>(order);
                    result.StartPoint = GetPointName(_repository, order.StartPointId);
                    result.EndPoint = GetPointName(_repository, order.EndPointId);
                    return new OkObjectResult(result);
                }
            }

            return new NotFoundResult();
        }

        public async Task<IActionResult> CreateOrder(OrderInputModel orderInput)
        {            
            var check = OrderInputValidation.OrderValuesValidation(_repository, orderInput, out string errorMessage);

            if (check)
            {
                RetrieveDataForCreatingOrder(orderInput, out Guid carIDReadyToOrder, out Guid whoOrdered, out int availableSeatsNum);
                if (availableSeatsNum < orderInput.OrderSeatsNum) return new BadRequestObjectResult("Attempt to order more seats than available");
                var startPointKM = GetLengthKM(_repository, orderInput.StartPoint);
                var endPointKM = GetLengthKM(_repository, orderInput.EndPoint);

                try
                {
                    var order = _mapper.Map<Order>(orderInput);
                    double totalPrice = CountTotalPrice(startPointKM, endPointKM, order.OrderSeatsNum);

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
        public void UpdateOrder(OrderInputModelPutMethod putModel)
        {
            var user = _repository.Users.FirstOrDefault(u => u.FullName == putModel.FullName);
            var check = OrderInputValidation.OrderValuesValidation(_repository, (OrderInputModel)putModel, out string _);

            if (user != null && check)
            {
                Order order = _repository.Orders.OrderByDescending(o => o.OrderDateTime).First(o => o.CustomerId == user.Id && o.IsActual);
                PutMethodMyMapper(_repository, putModel, order, user, out bool isPointModified);
                
                if (isPointModified)
                {
                    var startPointKM = GetLengthKM(_repository, order.StartPointId);
                    var endPointKM = GetLengthKM(_repository, order.EndPointId);
                    order.TotalPrice = CountTotalPrice(startPointKM, endPointKM, order.OrderSeatsNum);
                }

                _repository.SaveChanges();
            }
        }

        /// <summary>
        /// This method deletes(cancels) last active user's order
        /// </summary>
        /// <param name="FullName"></param>
        public void DeleteOrder(string FullName)
        {
            var user = _repository.Users.FirstOrDefault(u => u.FullName == FullName);

            if (user != null)
            {
                Order order = _repository.Orders.OrderByDescending(o => o.OrderDateTime).First(o => o.CustomerId == user.Id && o.IsActual);

                if (order != null)
                {
                    _repository.Remove(order);
                    _repository.SaveChanges();
                }
            }            
        }

        #region RetrieveDataForCreatingOrder

        private void RetrieveDataForCreatingOrder(DataLayer.InputModels.OrderInputModel orderInput, out Guid carIDReadyToOrder, out Guid whoOrdered, out int availableSeatsNum)
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

        #endregion

        #region AssignValuesToOrder
        private static void AssignValuesToOrder(Order order, MyShuttleBusAppNewDBContext _repository, Guid carIDReadyToOrder, Guid whoOrdered, DataLayer.InputModels.OrderInputModel orderInput, double totalPrice)
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

        #region PutMethodMyMapper

        private static void PutMethodMyMapper(MyShuttleBusAppNewDBContext _repository, OrderInputModelPutMethod putModel, Order order, User user, out bool isPointsModified)
        {            
            var inputPropertiesArr = putModel.GetType().GetProperties();
            isPointsModified = false;

            try
            {
                foreach (var item in inputPropertiesArr)
                {
                    var PropertyName = item.Name;
                    if (PropertyName != nameof(putModel.FullName))
                    {
                        var fieldValue = item.GetValue(putModel);
                        if (fieldValue is null || string.IsNullOrWhiteSpace(fieldValue.ToString())) continue;
                        if (order is null) order = _repository.Orders.OrderByDescending(o => o.OrderDateTime).First(o => o.CustomerId == user.Id);

                        if (PropertyName.Contains("Point"))
                        {
                            var pointID = GetPointID(_repository, fieldValue.ToString());
                            var pointProperty = order.GetType().GetProperty(PropertyName + "Id");
                            var pointCurrentValue = (int)pointProperty.GetValue(order);
                            if (pointCurrentValue != pointID && !isPointsModified) isPointsModified = true;

                            pointProperty.SetValue(order, pointID);
                            continue;
                        }

                        if (PropertyName == nameof(putModel.OrderType)) fieldValue = fieldValue.ToString();

                        var property = order.GetType().GetProperty(PropertyName);
                        property.SetValue(order, fieldValue);
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        private static string GetPointName(MyShuttleBusAppNewDBContext _repository, int id)
        {
            return _repository.RouteStops.FirstOrDefault(c => c.PointId == id).Name;
        }

        private static int GetPointID(MyShuttleBusAppNewDBContext _repository, string pointName)
        {
            return _repository.RouteStops.FirstOrDefault(c => c.Name == pointName).PointId;
        }

        private static int GetLengthKM(MyShuttleBusAppNewDBContext _repository, string pointName)
        {
            return _repository.RouteStops.FirstOrDefault(c => c.Name == pointName).RouteLengthKM;
        }

        private static int GetLengthKM(MyShuttleBusAppNewDBContext _repository, int id)
        {
            return _repository.RouteStops.FirstOrDefault(c => c.PointId == id).RouteLengthKM;
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

        private static double CountTotalPrice(int startPointKM, int endPointKM, int OrderSeatsNum)
        {
            return Math.Round(startPointKM > endPointKM ? (startPointKM - endPointKM) * 0.065 * OrderSeatsNum : (endPointKM - startPointKM) * 0.065 * OrderSeatsNum);
        }
    }
}
