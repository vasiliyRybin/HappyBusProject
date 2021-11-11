using HappyBusProject.HappyBusProject.DataLayer.InputModels;
using HappyBusProject.HappyBusProject.DataLayer.InputModels.OrdersInputModels;
using HappyBusProject.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.HappyBusProject.Interfaces
{
    public interface IOrderRepository<T> : IBusAppObject<T>
        where T : IActionResult
    {
        Task<T> CreateOrder(OrderInputModel orderInput);
        public void UpdateOrder(OrdersInputModelPutMethod putModel);
        public void DeleteOrder(string FullName);
    }
}
