using AutoMapper;
using HappyBusProject.InputModels;
using HappyBusProject.Interfaces;
using HappyBusProject.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.Services
{
    public class CarsCurrentStateService
    {
        private IRepository<Driver> DrRepository { get; }
        private IRepository<Car> CarRepository { get; }
        private IRepository<CarsCurrentState> StRepository { get; }
        private readonly ILogger _logger;
        private IMapper Mapper { get; }

        public CarsCurrentStateService(IRepository<Driver> drRepository, IRepository<Car> carRepository, IRepository<CarsCurrentState> stRepository, ILogger<CarsCurrentStateService> logger, IMapper mapper)
        {
            CarRepository = carRepository;
            DrRepository = drRepository;
            StRepository = stRepository;
            _logger = logger;
            Mapper = mapper;
        }

        public async Task<CarStateViewModel[]> GetAllStates()
        {
            try
            {
                var drivers = await DrRepository.Get();
                var cars = await CarRepository.Get();
                var currentStates = await StRepository.Get();

                var result = drivers
                    .Join(cars, d => d.CarId, c => c.CarId,
                     (driver, car) => new { driver, car })

                    .Join(currentStates,
                        joined => joined.car.CarId,
                        carState => carState.Id,
                        (joined, carState) => new CarStateViewModel
                        {
                            CarBrand = joined.car.CarBrand,
                            DriverName = joined.driver.DriverName,
                            SeatsNum = carState.FreeSeatsNum,
                            FreeSeatsNum = carState.FreeSeatsNum,
                            IsBusyNow = carState.IsBusyNow
                        });

                if (result.Any()) return result.ToArray();

                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e + "\t" + "GET Method, CarsCurrentState Service" + "\t" + e.Message + "\n");
                return null;
            }
        }


        public async Task<CarStateViewModel> GetStateByDriversName(string DriverName)
        {
            try
            {
                var driver = await DrRepository.GetFirstOrDefault(d => d.DriverName == DriverName);

                if (driver != null)
                {
                    var currentState = await StRepository.GetFirstOrDefault(s => s.Id == driver.CarId);
                    var carBrand = CarRepository.GetFirstOrDefault(d => d.CarId == currentState.Id).Result.CarBrand;

                    if (currentState != null)
                    {
                        var result = Mapper.Map<CarStateViewModel>(currentState);
                        result.DriverName = driver.DriverName;
                        result.CarBrand = carBrand;
                        return result;
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e + "\t" + "GET (by name) Method, CarsCurrentState Service" + "\t" + e.Message + "\n");
                return null;
            }
        }

        public async Task<CarStateViewModel> CreateState(CarStatePostModel newState)
        {
            var driver = await DrRepository.GetFirstOrDefault(d => d.DriverName == newState.DriverName);

            if (driver != null)
            {
                try
                {
                    var car = await CarRepository.GetFirstOrDefault(c => c.CarId == driver.CarId);

                    CarsCurrentState result = new();
                    Mapper.Map(newState, result);
                    Mapper.Map(car, result);
                    result.FreeSeatsNum = result.SeatsNum;

                    var viewResult = Mapper.Map<CarStateViewModel>(result);
                    viewResult.CarBrand = car.CarBrand;
                    viewResult.DriverName = driver.DriverName;

                    var createResult = await StRepository.Create(result);
                    if (createResult) return viewResult;

                }
                catch (Exception e)
                {
                    _logger.LogError(e + "\t" + "POST Method, CarsCurrentState Service" + "\t" + e.Message + "\n");
                    return null;
                }
            }

            return null;
        }

        public async Task<bool> UpdateState(string DriverName, CarStateInputModel newState)
        {
            var driver = await DrRepository.GetFirstOrDefault(d => d.DriverName == DriverName);

            if (driver != null)
            {
                var currentCarState = await StRepository.GetFirstOrDefault(s => s.Id == driver.CarId);
                if (currentCarState != null)
                {
                    try
                    {
                        Mapper.Map(newState, currentCarState);
                        var result = await StRepository.Update(currentCarState);
                        if (result) return true;

                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e + "\t" + "PUT Method, CarsCurrentState Service" + "\t" + e.Message + "\n");
                        return false;
                    }
                }
            }

            return false;
        }

        public async Task<bool> DeleteState(string DriverName)
        {
            var driver = await DrRepository.GetFirstOrDefault(d => d.DriverName == DriverName);

            if (driver != null)
            {
                var currentCarState = await StRepository.GetFirstOrDefault(s => s.Id == driver.CarId);
                if (currentCarState != null)
                {
                    try
                    {
                        var result = await StRepository.Delete(currentCarState);
                        if (result) return true;
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e + "\t" + "Delete Method, CarsCurrentState Service" + "\t" + e.Message + "\n");
                        return false;
                    }
                }
            }

            return false;
        }
    }
}
