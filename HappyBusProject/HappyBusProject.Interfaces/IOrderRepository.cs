using HappyBusProject.HappyBusProject.DataLayer.InputModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.HappyBusProject.Interfaces
{
    public interface IOrderRepository<T> 
        where T : IActionResult
    {
        Task<IActionResult> GetAllOrders();
        Task<T> GetLastOrder(string FullName);
        Task<IActionResult> CreateOrder(OrderInputModel orderInput);
        public void UpdateOrder(string FullName);
        public void DeleteOrder(string FullName);
    }
}
