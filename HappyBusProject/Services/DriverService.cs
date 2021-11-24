using AutoMapper;
using HappyBusProject.HappyBusProject.DataLayer.Models;
using HappyBusProject.Interfaces;
using HappyBusProject.ModelsToReturn;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HappyBusProject.Web.Services
{
    public class DriverService
    {
        //private readonly MyShuttleBusAppNewDBContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _log;
        IRepository<Car> _carRepository;
        IRepository<Driver> _drRepository;
        IRepository<CarsCurrentState> _stRepository;

        public DriverService(
            //repositories
            IMapper mapper, ILogger log)
        {
            //repositories
            _mapper = mapper;
            _log = log;
        }

        public async Task<DriverViewModel> CreateAsync(DriverCarInputModel driverCar)
        { 
            try
            {
                Car car = _mapper.Map<Car>(driverCar);
                Driver driver = _mapper.Map<Driver>(driverCar);
                driver.CarId = car.CarId;

                DriverViewModel driverInfo = _mapper.Map<DriverViewModel>(driver);
                _mapper.Map(car, driverInfo);

                CarsCurrentState carsCurrent = _mapper.Map<CarsCurrentState>(car);
                _mapper.Map(driverCar, carsCurrent);

                _drRepository.Create(driver);
                _carRepository.Create(car);
                _stRepository.Create(carsCurrent);
                return driverInfo;
            }
            catch (Exception e)
            {
                _log.LogError(e, e.Message);
                return null;
            }
        }

        public async Task<IActionResult> DeleteAsync(string name)
        {
            try
            {
                var driver = _context.Drivers.FirstOrDefault(c => c.DriverName == name);
                var carToRemove = _context.Cars.FirstOrDefault(c => c.CarId == driver.CarId);
                var stateToRemove = _context.CarCurrentStates.FirstOrDefault(s => s.Id == driver.CarId);

                if (driver != null && carToRemove != null)
                {
                    _context.Remove(driver);
                    _context.Remove(carToRemove);
                    _context.Remove(stateToRemove);
                    await _context.SaveChangesAsync();
                    return new OkResult();
                }
                return new NoContentResult();

            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + "\t" + "DELETE method, DriversRepository");
                return new BadRequestObjectResult(e.Message);
            }
        }

        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                //drivers
                //cars 
                var drivers = await _context.Drivers.Join(_context.Cars, d => d.CarId, c => c.CarId, (d, c) => new { d.DriverName, d.DriverAge, d.Rating, c.CarBrand }).ToListAsync();

                if (drivers?.Count != 0)
                {
                    var result = new DriverViewModel[drivers.Count];

                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = new DriverViewModel { DriverName = drivers[i].DriverName, DriverAge = drivers[i].DriverAge, CarBrand = drivers[i].CarBrand, Rating = drivers[i].Rating };
                    }

                    return (result);
                }

                return null;
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + "\t" + "GET Method, DriversRepository");
                return new BadRequestObjectResult(e.Message);
            }
        }

        public async Task<IActionResult> GetByNameAsync(string name)
        {
            try
            {
                var drivers = await _context.Drivers.Where(d => d.DriverName == name)
                                                    .Join(_context.Cars, d => d.CarId, c => c.CarId,
                                                    (d, c) => new { d.DriverName, d.DriverAge, d.Rating, c.CarBrand })
                                                    .ToListAsync();

                if (drivers.Count != 0 && drivers != null)
                {
                    var driver = new DriverViewModel()
                    {
                        DriverName = drivers[0].DriverName,
                        DriverAge = drivers[0].DriverAge,
                        CarBrand = drivers[0].CarBrand,
                        Rating = drivers[0].Rating
                    };

                    return new OkObjectResult(driver);
                }

                return new NotFoundResult();
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "GET Method, DriversRepository");
                return new BadRequestObjectResult(e.Message);
            }
        }

        public async Task<IActionResult> UpdateAsync(DriverCarInputModel driverCar)
        {
            if (!DriversInputValidation.PutMethodInputValidation(driverCar, out string errorMessage)) return new BadRequestResult();

            try
            {
                var driverCarID = _context.Drivers.FirstOrDefault(c => c.DriverName.Contains(driverCar.DriverName)).CarId;
                var car = _context.Cars.FirstOrDefault(c => c.CarId == driverCarID);
                if (car != null)
                {
                    car.CarBrand = driverCar.CarBrand;
                    await _context.SaveChangesAsync();
                    return new OkResult();
                }

                return new NoContentResult();
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "PUT method, DriversRepository");
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
