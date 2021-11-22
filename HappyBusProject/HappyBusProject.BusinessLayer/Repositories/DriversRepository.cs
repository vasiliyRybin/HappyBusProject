using AutoMapper;
using HappyBusProject.HappyBusProject.DataLayer.Models;
using HappyBusProject.InputValidators;
using HappyBusProject.ModelsToReturn;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace HappyBusProject.Repositories
{
    public class DriversRepository : IDriversRepository<DriverViewModel>
    {
        private readonly MyShuttleBusAppNewDBContext _context;
        private readonly IMapper _mapper;

        public DriversRepository(MyShuttleBusAppNewDBContext myShuttleBusAppNewDBContext, IMapper mapper)
        {
            _context = myShuttleBusAppNewDBContext ?? throw new ArgumentNullException(nameof(myShuttleBusAppNewDBContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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

                await _context.Drivers.AddAsync(driver);
                await _context.Cars.AddAsync(car);
                await _context.CarCurrentStates.AddAsync(carsCurrent);
                int successUpdate = await _context.SaveChangesAsync();
                if (successUpdate > 0) return driverInfo;
                return null;
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message);
                return null;
            }
        }


        public async Task DeleteDriver(string name)
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
                }

            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + "\t" + "DELETE method, DriversRepository");
            }
        }

        public async Task<DriverViewModel[]> GetAllAsync()
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

                    return result;
                }

                return null;
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + "\t" + "GET Method, DriversRepository");
                return null;
            }
        }

        public async Task<DriverViewModel> GetByNameAsync(string name)
        {
            try
            {
                var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.DriverName == name);
                Car car = new();
                if (driver != null) car = _context.Cars.FirstOrDefault(c => c.CarId == driver.CarId);

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
                LogWriter.ErrorWriterToFile(e.Message + " " + "GET Method, DriversRepository");
                return null;
            }
        }

        public async Task UpdateDriver(DriverCarInputModel driverCar)
        {
            if (!DriversInputValidation.PutMethodInputValidation(driverCar, out string errorMessage)) return;

            try
            {
                Guid CarID = Guid.Empty;
                var driver = _context.Drivers.FirstOrDefault(c => c.DriverName.Contains(driverCar.DriverName));

                if (driver != null) CarID = driver.CarId;
                else return;

                var car = _context.Cars.FirstOrDefault(c => c.CarId == CarID);
                if (car != null)
                {
                    car.CarBrand = driverCar.CarBrand;
                    await _context.SaveChangesAsync();
                    return;
                }

                return;
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "PUT method, DriversRepository");
                return;
            }
        }
    }
}
