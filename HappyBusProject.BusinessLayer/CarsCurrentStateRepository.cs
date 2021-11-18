﻿using AutoMapper;
using HappyBusProject.HappyBusProject.DataLayer.InputModels;
using HappyBusProject.HappyBusProject.DataLayer.InputModels.CarStateModels;
using HappyBusProject.HappyBusProject.DataLayer.ViewModels;
using HappyBusProject.HappyBusProject.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.HappyBusProject.BusinessLayer.Repositories
{
    public class CarsCurrentStateRepository : ICarsStateRepository<Car>   
    {
        private readonly MyShuttleBusAppNewDBContext _repository;

        public CarsCurrentStateRepository(MyShuttleBusAppNewDBContext myShuttleBusAppNewDBContext, IMapper mapper)
        {
            _repository = myShuttleBusAppNewDBContext ?? throw new ArgumentNullException(nameof(myShuttleBusAppNewDBContext));
            _mapper = mapper;
        }

        public async Task<IActionResult> CreateState(CarStatePostModel newState)
        {
            var driver = await _repository.Drivers
                .FirstOrDefaultAsync(d => d.DriverName == newState.DriverName);

            if (driver != null)
            {
                try
                {
                    var car = _repository.Cars.First(c => c.CarId == driver.CarId);

                    CarsCurrentState result = new();
                    _mapper.Map(newState, result);
                    _mapper.Map(car, result);
                    result.FreeSeatsNum = result.SeatsNum;

                    var viewResult = _mapper.Map<CarStateViewModel>(result);
                    viewResult.CarBrand = car.CarBrand;
                    viewResult.DriverName = driver.DriverName;

                    _repository.Add(result);
                    if (_repository.SaveChanges() > 0) return new OkObjectResult(viewResult);

                }
                catch (Exception e)
                {
                    LogWriter.ErrorWriterToFile("POST Method, CarsCurrentState Repository" + "\t" + e.Message);
                    return new ConflictObjectResult(e.Message);
                }
            }

            return new EmptyResult();
        }

        public void DeleteState(string DriverName)
        {
            var driver = _repository.Drivers.FirstOrDefault(d => d.DriverName == DriverName);

            if (driver != null)
            {
                var currentCarState = _repository.CarCurrentStates.FirstOrDefault(s => s.Id == driver.CarId);
                if (currentCarState != null)
                {
                    try
                    {
                        _repository.Remove(currentCarState);
                        _repository.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        LogWriter.ErrorWriterToFile("Delete Method, CarsCurrentState Repository" + "\t" + e.Message);
                    }
                }
            }
        }

        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var currentState = await
                (
                    Task.Run
                    (() =>
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
                    return null;
                }

                return currentState;
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile("GET Method, CarsCurrentState Repository" + "\t" + e.Message);
                return new ConflictObjectResult(e.Message);
            }

        }

        public async Task<IActionResult> GetByNameAsync(string DriverName)
        {
            try
            {
                var driver = await _repository.Drivers.FirstOrDefaultAsync(d => d.DriverName == DriverName);

                if (driver != null)
                {
                    var currentState = _repository.CarCurrentStates.FirstOrDefault(s => s.Id == driver.CarId);

                    if (currentState != null)
                    {
                        var result = _mapper.Map<CarStateViewModel>(currentState);
                        result.DriverName = driver.DriverName;
                        result.CarBrand = _repository.Cars.FirstOrDefault(d => d.CarId == currentState.Id).CarBrand;
                        return new OkObjectResult(result);
                    }
                }

                return new NoContentResult();
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile("GET (by name) Method, CarsCurrentState Repository" + "\t" + e.Message);
                return new ConflictObjectResult(e.Message);
            }
        }

        public void UpdateState(string DriverName, CarStateInputModel newState)
        {
            var driver = _repository.Drivers.FirstOrDefault(d => d.DriverName == DriverName);

            if (driver != null)
            {
                var currentCarState = _repository.CarCurrentStates.FirstOrDefault(s => s.Id == driver.CarId);
                if (currentCarState != null)
                {
                    try
                    {
                        _mapper.Map(newState, currentCarState);
                        _repository.Update(currentCarState);
                        _repository.SaveChanges();

                    }
                    catch (Exception e)
                    {
                        LogWriter.ErrorWriterToFile("POST Method, CarsCurrentState Repository" + "\t" + e.Message);
                    }
                }
            }
        }
    }
}