using HappyBusProject.InputModels;
using HappyBusProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HappyBusProject.Controllers
{
    [Route("AppAPI/NewOrders")]
    [ApiController]
    public class NewOrdersController : ControllerBase
    {
        public OrdersService Service { get; }

        public NewOrdersController(OrdersService service)
        {
            Service = service;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            var result = await Service.GetAllOrdersAsync();

            if (result != null) return Ok(result);
            return NoContent();
        }


        [HttpGet("{FullName}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Get(string FullName)
        {
            var result = await Service.GetOrderByCustomerNameAsync(FullName);

            if (result != null) return Ok(result);
            return NotFound();
        }


        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Post([FromBody] OrderInputModel orderInput)
        {
            var result = await Service.CreateOrder(orderInput);

            if (result != null) return Ok(result);
            return Conflict();
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(OrderInputModelPutMethod putMethod)
        {
            var result = await Service.UpdateOrder(putMethod);

            if (result != null) return Ok(result);
            return Conflict();
        }


        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string FullName)
        {
            var result = await Service.DeleteOrder(FullName);

            if (result) return Ok(result);
            return Conflict();
        }
    }
}
