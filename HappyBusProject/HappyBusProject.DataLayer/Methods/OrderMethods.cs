using HappyBusProject.HappyBusProject.DataLayer.InputModels;
using HappyBusProject.HappyBusProject.DataLayer.InputModels.OrdersInputModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace HappyBusProject.HappyBusProject.DataLayer.Methods
{
    public static class OrderMethods
    {
        #region RetrieveDataForCreatingOrder

        public static void RetrieveDataForCreatingOrder(MyShuttleBusAppNewDBContext _repository, OrderInputModel orderInput, out Guid carIDReadyToOrder, out Guid whoOrdered, out int availableSeatsNum)
        {
            var carID = GetDriver(_repository, orderInput.DesiredDepartureTime);
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
        public static void AssignValuesToOrder(Order order, MyShuttleBusAppNewDBContext _repository, Guid carIDReadyToOrder, Guid whoOrdered, OrderInputModel orderInput, double totalPrice)
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

        public static void PutMethodMyMapper(MyShuttleBusAppNewDBContext _repository, OrderInputModelPutMethod putModel, Order order, User user, out bool isPointsModified)
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

                        if (fieldValue is null || string.IsNullOrWhiteSpace(fieldValue.ToString()))
                            continue;

                        if (order is null)
                            order = _repository.Orders.OrderByDescending(o => o.OrderDateTime).First(o => o.CustomerId == user.Id);

                        if (PropertyName.Contains("Point"))
                        {
                            var pointID = GetPointID(_repository, fieldValue.ToString());
                            var pointProperty = order.GetType().GetProperty(PropertyName + "Id");
                            var pointCurrentValue = (int)pointProperty.GetValue(order);

                            if (pointCurrentValue != pointID && !isPointsModified) 
                                isPointsModified = true;

                            pointProperty.SetValue(order, pointID);
                            continue;
                        }

                        if (PropertyName == nameof(putModel.OrderType)) 
                            fieldValue = fieldValue.ToString();

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

        public static string GetPointName(MyShuttleBusAppNewDBContext _repository, int id)
        {
            return _repository.RouteStops.FirstOrDefault(c => c.PointId == id).Name;
        }

        public static int GetPointID(MyShuttleBusAppNewDBContext _repository, string pointName)
        {
            return _repository.RouteStops.FirstOrDefault(c => c.Name == pointName).PointId;
        }

        public static int GetLengthKM(MyShuttleBusAppNewDBContext _repository, string pointName)
        {
            return _repository.RouteStops.FirstOrDefault(c => c.Name == pointName).RouteLengthKM;
        }

        public static int GetLengthKM(MyShuttleBusAppNewDBContext _repository, int id)
        {
            return _repository.RouteStops.First(c => c.PointId == id).RouteLengthKM;
        }

        public static Guid GetDriver(MyShuttleBusAppNewDBContext _repository, DateTime desiredDepartureDateTime)
        {
            var freeCar = _repository.CarCurrentStates.FirstOrDefault(c => c.DepartureTime.Equals(desiredDepartureDateTime));

            if (freeCar != null)
            {
                return freeCar.Id;
            }
            return Guid.Empty;
        }

        public static double CountTotalPrice(int startPointKM, int endPointKM, int OrderSeatsNum)
        {
            return Math.Round(startPointKM > endPointKM ? (startPointKM - endPointKM) * 0.065 * OrderSeatsNum : (endPointKM - startPointKM) * 0.065 * OrderSeatsNum);
        }
    }
}
