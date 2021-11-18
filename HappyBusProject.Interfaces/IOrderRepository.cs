using HappyBusProject.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HappyBusProject.HappyBusProject.Interfaces
{
    public interface IOrderRepository<T> : IBusAppObject<T>
        where T : IActionResult
    {
        Task<T> CreateOrder(DataLayer.InputModels.OrderInputModel orderInput);
        public void UpdateOrder(DataLayer.InputModels.OrdersInputModels.OrderInputModelPutMethod putModel);
        public void DeleteOrder(string FullName);
    }
}
