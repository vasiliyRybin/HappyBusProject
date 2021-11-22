using HappyBusProject.HappyBusProject.DataLayer.InputModels;
using HappyBusProject.HappyBusProject.DataLayer.InputModels.OrdersInputModels;
using HappyBusProject.HappyBusProject.DataLayer.ViewModels;
using HappyBusProject.HappyBusProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HappyBusProject.Controllers
{
    [Route("AppAPI/Orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository<OrderViewModel> _repository;
        public OrdersController(IOrderRepository<OrderViewModel> repository)
        {
            _repository = repository;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            return new ObjectResult(await _repository.GetAllAsync());
        }


        [HttpGet("{FullName}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Get(string FullName)
        {
            return new ObjectResult(await _repository.GetByNameAsync(FullName));
        }


        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Post([FromBody] OrderInputModel orderInput)
        {
            return new ObjectResult(await _repository.CreateOrder(orderInput));
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        public void Put(OrderInputModelPutMethod putMethod)
        {
            _repository.UpdateOrder(putMethod);
        }


        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public void Delete(string FullName)
        {
            _repository.DeleteOrder(FullName);
        }
    }
}
