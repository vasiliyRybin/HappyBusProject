using HappyBusProject.HappyBusProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet("{name}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(string name)
        {
            return new ObjectResult(await _repository.GetByNameAsync(name));
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public void Put(int id, string value)
        {
            //TODO: Implement something there
        }
    }
}
