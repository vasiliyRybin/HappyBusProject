using HappyBusProject.InputModels;
using System;

namespace HappyBusProject.HappyBusProject.DataLayer.InputValidators
{
    public static class OrderInputValidation
    {
        public static bool OrderValuesValidation(Guid CustomerGUID, string startPoint, string endPoint, OrderInputModel orderInput, out string errorMessage)
        {
            if (CustomerGUID == default)
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
            if (string.IsNullOrWhiteSpace(startPoint) || string.IsNullOrWhiteSpace(endPoint))
            {
                errorMessage = "No such bus stop exists";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
