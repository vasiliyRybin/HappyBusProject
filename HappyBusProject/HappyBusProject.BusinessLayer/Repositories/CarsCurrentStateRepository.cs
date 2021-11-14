using AutoMapper;
using HappyBusProject.HappyBusProject.DataLayer.ViewModels;
using HappyBusProject.HappyBusProject.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.HappyBusProject.BusinessLayer.Repositories
{
    public class CarsCurrentStateRepository : ICarsStateRepository<IActionResult>
    {
        private readonly MyShuttleBusAppNewDBContext _repository;
        private readonly IMapper _mapper;

        public CarsCurrentStateRepository(MyShuttleBusAppNewDBContext myShuttleBusAppNewDBContext, IMapper mapper)
        {
            _repository = myShuttleBusAppNewDBContext ?? throw new ArgumentNullException(nameof(myShuttleBusAppNewDBContext));
            _mapper = mapper;
        }

        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var currentState = await
                (
                    Task.Run(() => 
                    _repository.Drivers
                    .Join(_repository.Cars, d => d.CarId, c => c.CarId,
                    (driver, car) => new { driver, car })

                    .Join(_repository.CarCurrentStates,
                    joined => joined.car.CarId,
                    carState => carState.Id,
                    (joined, carState) => new CarStateViewModel
                    {
                        CarBrand = joined.car.CarBrand,
                        DriverName = joined.driver.DriverName,
                        SeatsNum = carState.FreeSeatsNum,
                        FreeSeatsNum = carState.FreeSeatsNum,
                        IsBusyNow = carState.IsBusyNow
                    }))
                );

                if (!currentState.Any())
                {
                    return new NoContentResult();
                }

                return new OkObjectResult(currentState);
            }
            catch (Exception e)
            {
                return new ConflictObjectResult(e.Message);
            }
            
        }

        public async Task<IActionResult> GetByNameAsync(string name)
        {
            var currentState = await
                (
                    Task.Run(() =>
                    _repository.Drivers.Where(d => d.DriverName == name)
                    .Join(_repository.Cars, d => d.CarId, c => c.CarId,
                    (driver, car) => new { driver, car })

                    .Join(_repository.CarCurrentStates,
                    joined => joined.car.CarId,
                    carState => carState.Id,
                    (joined, carState) => new CarStateViewModel
                    {
                        CarBrand = joined.car.CarBrand,
                        DriverName = joined.driver.DriverName,
                        SeatsNum = carState.FreeSeatsNum,
                        FreeSeatsNum = carState.FreeSeatsNum,
                        IsBusyNow = carState.IsBusyNow
                    }))
                );

            if (!currentState.Any())
            {
                return new NoContentResult();
            }

            return new OkObjectResult(currentState);
        }

        public void UpdateState(string FullName)
        {
            throw new NotImplementedException();
        }
    }
}
