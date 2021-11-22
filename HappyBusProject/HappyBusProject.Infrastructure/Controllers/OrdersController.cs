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
            var result = await _repository.GetAllAsync();

            if (result != null) return Ok(result);
            return NoContent();
        }


        [HttpGet("{FullName}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Get(string FullName)
        {
            var result = await _repository.GetByNameAsync(FullName);

            if (result != null) return Ok(result);
            return NotFound();
        }


        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Post([FromBody] OrderInputModel orderInput)
        {
            var result = await _repository.CreateOrder(orderInput);

            if (result != null) return Ok(result);
            return Conflict();
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
