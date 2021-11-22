﻿using HappyBusProject.InputValidators;
using HappyBusProject.ModelsToReturn;
using HappyBusProject.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HappyBusProject.Controllers
{
    [ApiController]
    [Route("AppAPI/Drivers")]
    public class DriversController : ControllerBase
    {
        private readonly IDriversRepository<DriverViewModel> _repository;
        //private Guid UserID => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public DriversController(IDriversRepository<DriverViewModel> driversRepository)
        {
            _repository = driversRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            var result = await _repository.GetAllAsync();
            if (result != null) return Ok(result);
            return NoContent();
        }

        [HttpGet("{name}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(string name)
        {
            var result = await _repository.GetByNameAsync(name);
            if (result != null) return Ok(result);
            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "Driver, Admin")]
        public async Task<IActionResult> PostTest([FromBody] DriverCarInputModel driverCar)
        {
            var isNotEmtpy = DriversInputValidation.IsEmptyInputValues(driverCar);
            if (!isNotEmtpy) return BadRequest();

            var result = await _repository.CreateAsync(driverCar);

            if (result != null) return Ok(result);
            return Conflict();
        }

        [HttpPut("{driverName}/{newCarBrand}")]
        [Authorize(Roles = "Driver, Admin")]
        public void UpdateDriver(string driverName, string newCarBrand)
        {
            DriverCarInputModel driverCar = new()
            {
                DriverName = driverName,
                CarBrand = newCarBrand
            };

            _repository.UpdateDriver(driverCar);
        }

        [HttpDelete("{driverName}")]
        [Authorize(Roles = "Driver, Admin")]
        public void DeleteDriver(string driverName)
        {
            _repository.DeleteDriver(driverName);
        }
    }
}