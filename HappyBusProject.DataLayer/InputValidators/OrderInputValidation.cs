using HappyBusProject.InputModels;
using System.Linq;

namespace HappyBusProject.HappyBusProject.DataLayer.InputValidators
{
    public static class OrderInputValidation
    {
        public static bool OrderValuesValidation(MyShuttleBusAppNewDBContext _repository, OrderInputModel orderInput, out string errorMessage)
        {
            var CustomerGUID = _repository.Users.FirstOrDefault(u => u.FullName == orderInput.FullName);
            var startPoint = _repository.RouteStops.FirstOrDefault(r => r.Name == orderInput.StartPoint);
            var endPoint = _repository.RouteStops.FirstOrDefault(r => r.Name == orderInput.EndPoint);

            if (CustomerGUID is null)
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
