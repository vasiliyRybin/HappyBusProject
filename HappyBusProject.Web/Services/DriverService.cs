using AutoMapper;
using HappyBusProject.InputModels;
using HappyBusProject.InputValidators;
using HappyBusProject.Interfaces;
using HappyBusProject.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.Services
{
    public class DriverService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _log;
        readonly IRepository<Car> _carRepository;
        readonly IRepository<Driver> _drRepository;
        readonly IRepository<CarsCurrentState> _stRepository;

        public DriverService(IRepository<Driver> driver, IRepository<Car> car, IRepository<CarsCurrentState> currentState, ILogger<DriverService> logger, IMapper mapper)
        {
            _carRepository = car;
            _drRepository = driver;
            _stRepository = currentState;
            _log = logger;
            _mapper = mapper;
        }

        public async Task<DriverViewModel> GetByNameAsync(string name)
        {
            try
            {
                Driver driver = await Task.Run(() => _drRepository.GetFirstOrDefault(x => x.DriverName == name));
                Car car = default;
                if (driver != default) car = await Task.Run(() => _carRepository.GetFirstOrDefault(x => x.CarId == driver.CarId));

                if (driver != null && car != null)
                {
                    var driverView = new DriverViewModel()
                    {
                        DriverName = driver.DriverName,
                        DriverAge = driver.DriverAge,
                        CarBrand = car.CarBrand,
                        Rating = driver.Rating
                    };

                    return driverView;
                }

                return null;
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "GET Method, NewDriversRepository");
                return null;
            }
        }

        public async Task<DriverViewModel[]> GetAllAsync()
        {
            try
            {
                var drivers = await _drRepository.Get();
                var cars = await _carRepository.Get();
                var preResult = drivers.Join(cars, d => d.CarId, c => c.CarId, (d, c) => new { d.DriverName, d.DriverAge, d.Rating, c.CarBrand }).ToList();

                if (preResult.Count != 0 && drivers != null)
                {
                    var result = new DriverViewModel[preResult.Count];

                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = new DriverViewModel { DriverName = preResult[i].DriverName, DriverAge = preResult[i].DriverAge, CarBrand = preResult[i].CarBrand, Rating = preResult[i].Rating };
                    }

                    return result;
                }

                return null;
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + "\t" + "GET Method, NewDriversRepository");
                return null;
            }
        }

        public async Task<DriverViewModel> CreateAsync(DriverCarInputModel driverCar)
        {
            var isNotValid = DriversInputValidation.DriversInputValidator(driverCar, out _);
            if (!isNotValid) return null;

            try
            {
                Car car = _mapper.Map<Car>(driverCar);

                Driver driver = _mapper.Map<Driver>(driverCar);
                driver.CarId = car.CarId;

                DriverViewModel driverInfo = _mapper.Map<DriverViewModel>(driver);
                _mapper.Map(car, driverInfo);

                CarsCurrentState carsCurrent = _mapper.Map<CarsCurrentState>(car);
                _mapper.Map(driverCar, carsCurrent);

                var drSuccess = await _drRepository.Create(driver);
                var carSuccess = await _carRepository.Create(car);
                var stSuccess = await _stRepository.Create(carsCurrent);

                if (drSuccess && carSuccess && stSuccess) return driverInfo;
                return null;
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message);
                return null;
            }
        }

        public async Task<bool> UpdateDriver(PutMethodDriverInputModel driverInput)
        {
            if (!DriversInputValidation.DriversInputValidator(driverInput, out string errorMessage)) return false;

            try
            {
                Guid CarID = Guid.Empty;
                var driver = await _drRepository.GetFirstOrDefault(c => c.DriverName.Contains(driverInput.DriverName));

                if (driver != null) CarID = driver.CarId;
                else return false;

                var car = await _carRepository.GetFirstOrDefault(c => c.CarId == CarID);
                if (car != null)
                {
                    _mapper.Map(driverInput, car);
                    _mapper.Map(driverInput, driver);
                    var carResult = await _carRepository.Update(car);
                    var driverResult = await _drRepository.Update(driver);
                    if (carResult && driverResult) return true;
                }

                return false;
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "PUT method, NewDriversRepository");
                return false;
            }
        }


        //public async Task DeleteDriver(string name)
        //{
        //    try
        //    {
        //        var driver = _context.Drivers.FirstOrDefault(c => c.DriverName == name);
        //        var carToRemove = _context.Cars.FirstOrDefault(c => c.CarId == driver.CarId);
        //        var stateToRemove = _context.CarCurrentStates.FirstOrDefault(s => s.Id == driver.CarId);

        //        if (driver != null && carToRemove != null)
        //        {
        //            _context.Remove(driver);
        //            _context.Remove(carToRemove);
        //            _context.Remove(stateToRemove);
        //            await _context.SaveChangesAsync();
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        LogWriter.ErrorWriterToFile(e.Message + "\t" + "DELETE method, NewDriversRepository");
        //    }
        //}  
    }
}
