using HappyBusProject;
using HappyBusProject.InputModels;
using HappyBusProject.Interfaces;
using HappyBusProject.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HappyBusProject.Controllers
{
    [Route("AppAPI/CarsState")]
    [ApiController]
    public class CarsCurrentStateController : ControllerBase
    {
        private readonly ICarsStateRepository<CarStateViewModel, CarStatePostModel, CarStateInputModel> _repository;
        public CarsCurrentStateController(ICarsStateRepository<CarStateViewModel, CarStatePostModel, CarStateInputModel> carsRepository)
        {
            _repository = carsRepository;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            var result = await _repository.GetAllAsync();

            if (result != null) return Ok(result);
            return NoContent();
        }

        [HttpGet("{DriverName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(string DriverName)
        {
            var result = await _repository.GetByNameAsync(DriverName);

            if (result != null) return Ok(result);

            return NotFound();
        }


        /// <summary>
        /// For Admin usage only! 
        /// Use at your own risk! :D
        /// Use it just for if something went wrong due creating state when new driver been created
        /// </summary>
        /// <param name="newState"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateState(CarStatePostModel newState)
        {
            var result = await _repository.CreateState(newState);
            if (result != null) return Ok(result);
            return Conflict();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public void UpdateState(string DriverName, CarStateInputModel newState)
        {
            _repository.UpdateState(DriverName, newState);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public void RemoveState(string DriverName)
        {
            _repository.DeleteState(DriverName);
        }
    }
}
