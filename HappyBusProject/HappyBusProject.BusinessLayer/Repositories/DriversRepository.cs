using AutoMapper;
using HappyBusProject.HappyBusProject.DataLayer.Models;
using HappyBusProject.InputValidators;
using HappyBusProject.ModelsToReturn;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace HappyBusProject.Repositories
{
    public class DriversRepository : IDriversRepository<IActionResult>
    {
        private readonly MyShuttleBusAppNewDBContext _context;
        private readonly IMapper _mapper;

        public DriversRepository(MyShuttleBusAppNewDBContext myShuttleBusAppNewDBContext, IMapper mapper)
        {
            _context = myShuttleBusAppNewDBContext ?? throw new ArgumentNullException(nameof(myShuttleBusAppNewDBContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public async Task<IActionResult> CreateAsync(DriverCarInputModel driverCar)
        {
            var isNotValid = DriversInputValidation.DriversInputValidator(driverCar, out _);
            if (!isNotValid) return new BadRequestResult();

            try
            {
                Car car = _mapper.Map<Car>(driverCar);

                Driver driver = _mapper.Map<Driver>(driverCar);
                driver.CarId = car.CarId;

                DriverViewModel driverInfo = _mapper.Map<DriverViewModel>(driver);
                _mapper.Map(car, driverInfo);

                CarsCurrentState carsCurrent = _mapper.Map<CarsCurrentState>(car);
                _mapper.Map(driverCar, carsCurrent);

                await _context.Drivers.AddAsync(driver);
                await _context.Cars.AddAsync(car);
                await _context.CarCurrentStates.AddAsync(carsCurrent);
                int successUpdate = await _context.SaveChangesAsync();
                if (successUpdate > 0) return new OkObjectResult(driverInfo);
                else return new NoContentResult();
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message);
                return new BadRequestObjectResult(e.Message);
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
                var drivers = await _context.Drivers.Join(_context.Cars, d => d.CarId, c => c.CarId, (d, c) => new { d.DriverName, d.DriverAge, d.Rating, c.CarBrand }).ToListAsync();

                if (drivers.Count != 0 && drivers != null)
                {
                    var result = new DriverViewModel[drivers.Count];

                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = new DriverViewModel { DriverName = drivers[i].DriverName, DriverAge = drivers[i].DriverAge, CarBrand = drivers[i].CarBrand, Rating = drivers[i].Rating };
                    }

                    return new OkObjectResult(result);
                }

                return new NoContentResult();
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
