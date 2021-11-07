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

        public DriversRepository(MyShuttleBusAppNewDBContext myShuttleBusAppNewDBContext)
        {
            _context = myShuttleBusAppNewDBContext ?? throw new ArgumentNullException(nameof(myShuttleBusAppNewDBContext));
        }

        public async Task<IActionResult> CreateAsync(DriverCarInputModel driverCar)
        {
            var isNotValid = DriversInputValidation.DriversInputValidator(driverCar, out int numSeats, out int carAgeInt, out int driverAgeInt, out DateTime resultExamPass, out _);
            if (!isNotValid) return new BadRequestResult();

            try
            {
                var carGuid = Guid.NewGuid();

                Car car = new()
                {
                    Id = carGuid,
                    Brand = driverCar.CarBrand,
                    SeatsNum = numSeats,
                    RegistrationNumPlate = driverCar.RegistrationNumPlate,
                    Age = carAgeInt
                };

                Driver driver = new()
                {
                    Name = driverCar.DriverName,
                    Age = driverAgeInt,
                    Id = Guid.NewGuid(),
                    CarId = carGuid,
                    Rating = 5.0,
                    MedicalExamPassDate = resultExamPass
                };

                DriverViewModel driverInfo = new()
                {
                    Age = driver.Age,
                    Name = driver.Name,
                    CarBrand = car.Brand,
                    Rating = driver.Rating
                };

                await _context.Drivers.AddAsync(driver);
                await _context.Cars.AddAsync(car);
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
                var driver = _context.Drivers.FirstOrDefault(c => c.Name.Contains(name));
                var carToRemove = _context.Cars.FirstOrDefault(c => c.Id == driver.CarId);

                if (driver != null && carToRemove != null)
                {
                    _context.Remove(driver);
                    _context.Remove(carToRemove);
                    await _context.SaveChangesAsync();
                    return new OkResult();
                }
                return new NoContentResult();

            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "DELETE method");
                return new BadRequestObjectResult(e.Message);
            }
        }

        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var drivers = await _context.Drivers.Join(_context.Cars, d => d.CarId, c => c.Id, (d, c) => new { d.Name, d.Age, d.Rating, CarBrand = c.Brand }).ToListAsync();

                if (drivers.Count != 0 && drivers != null)
                {
                    var result = new DriverViewModel[drivers.Count];

                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = new DriverViewModel { Name = drivers[i].Name, Age = drivers[i].Age, CarBrand = drivers[i].CarBrand, Rating = drivers[i].Rating };
                    }

                    return new OkObjectResult(result);
                }

                return new NoContentResult();
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "GET Method");
                return new BadRequestObjectResult(e.Message);
            }
        }

        public async Task<IActionResult> GetByNameAsync(string name)
        {
            try
            {
                var drivers = await _context.Drivers.Where(d => d.Name.Contains(name))
                                                    .Join(_context.Cars, d => d.CarId, c => c.Id,
                                                    (d, c) => new { d.Name, d.Age, d.Rating, CarBrand = c.Brand })
                                                    .ToListAsync();

                if (drivers.Count != 0 && drivers != null)
                {
                    var driver = new DriverViewModel()
                    {
                        Name = drivers[0].Name,
                        Age = drivers[0].Age,
                        CarBrand = drivers[0].CarBrand,
                        Rating = drivers[0].Rating
                    };

                    return new OkObjectResult(driver);
                }

                return new NotFoundResult();
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "GET Method");
                return new BadRequestObjectResult(e.Message);
            }
        }

        public async Task<IActionResult> UpdateAsync(DriverCarInputModel driverCar)
        {
            if (!DriversInputValidation.PutMethodInputValidation(driverCar, out string errorMessage)) return new BadRequestResult();

            try
            {
                var driverCarID = _context.Drivers.FirstOrDefault(c => c.Name.Contains(driverCar.DriverName)).CarId;
                var car = _context.Cars.FirstOrDefault(c => c.Id == driverCarID);
                if (car != null)
                {
                    car.Brand = driverCar.CarBrand;
                    await _context.SaveChangesAsync();
                    return new OkResult();
                }
                return new NoContentResult();
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "PUT method");
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
