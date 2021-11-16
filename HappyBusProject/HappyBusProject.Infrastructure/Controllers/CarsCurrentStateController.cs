using HappyBusProject.HappyBusProject.DataLayer.InputModels;
using HappyBusProject.HappyBusProject.DataLayer.InputModels.CarStateModels;
using HappyBusProject.HappyBusProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;


namespace HappyBusProject.HappyBusProject.Infrastructure.Controllers
{
    [Route("AppAPI/CarsState")]
    [ApiController]
    public class CarsCurrentStateController : ControllerBase
    {
        private readonly ICarsStateRepository<IActionResult> _repository;
        public CarsCurrentStateController(ICarsStateRepository<IActionResult> carsRepository)
        {
            _repository = carsRepository;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            return new ObjectResult(await _repository.GetAllAsync());
        }

        [HttpGet("{DriverName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(string DriverName)
        {
            return new ObjectResult(await _repository.GetByNameAsync(DriverName));
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
            return await _repository.CreateState(newState);
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
