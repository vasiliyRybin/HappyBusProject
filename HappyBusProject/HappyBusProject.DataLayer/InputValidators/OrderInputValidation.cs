using HappyBusProject.HappyBusProject.DataLayer.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.HappyBusProject.DataLayer.InputValidators
{
    public static class OrderInputValidation
    {
        public static bool OrderValuesValidation(OrderInputModel orderInput, Guid CustomerGUID, RouteStop startPoint, RouteStop endPoint, out string errorMessage)
        {
            if (CustomerGUID == Guid.Empty)
            {
                errorMessage = "Customer not found";
                return false;
            }
            if (orderInput is null)
            {
                errorMessage = "Order info is null";
                return false;
            }
            if (orderInput.StartPoint == orderInput.EndPoint)
            {
                errorMessage = "Start point same as end point";
                return false;
            }
            if (orderInput.OrderSeatsNum > 5)
            {
                errorMessage = "Ordering more than 5 seats is prohibited";
                return false;
            }
            if (startPoint is null || endPoint is null)
            {
                errorMessage = "No such bus stop exists";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
