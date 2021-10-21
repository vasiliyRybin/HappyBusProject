using HappyBusProject.InputValidators;
using HappyBusProject.ModelsToReturn;
using System;
using System.Linq;

namespace HappyBusProject.Repositories
{
    public class DriversRepository : IDriversRepository<DriverInfo[]>
    {
        private readonly MyShuttleBusAppNewDBContext _context;

        public DriversRepository(MyShuttleBusAppNewDBContext myShuttleBusAppNewDBContext)
        {
            _context = myShuttleBusAppNewDBContext;
        }

        public string Create(DriverCarPreResultModel driverCar)
        {
            var isNotValid = DriversInputValidation.DriversInputValidator(driverCar, out int numSeats, out int carAgeInt, out int driverAgeInt, out DateTime resultExamPass, out string errorMessage);
            if (!isNotValid) return errorMessage;

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

                _context.Drivers.Add(driver);
                _context.Cars.Add(car);
                int successUpdate = _context.SaveChanges();
                if (successUpdate > 0) return "Driver succesfully added";
                else return "No changes been made";

            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message);
                return e.Message;
            }
        }

        public string Delete(string name)
        {
            try
            {
                var driver = _context.Drivers.FirstOrDefault(c => c.Name.Contains(name));
                var carToRemove = _context.Cars.FirstOrDefault(c => c.Id == driver.CarId);

                if (driver != null && carToRemove != null)
                {
                    _context.Remove(driver);
                    _context.Remove(carToRemove);
                    Save();
                    return "Driver successfully deleted";
                }
                return "No changes been made";

            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "DELETE method");
                return e.Message;
            }
        }

        public DriverInfo[] GetAll()
        {
            try
            {
                var drivers = _context.Drivers.Join(_context.Cars, d => d.CarId, c => c.Id, (d, c) => new { d.Name, d.Age, d.Rating, CarBrand = c.Brand }).ToList();

                DriverInfo[] result = new DriverInfo[drivers.Count];

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = new DriverInfo { Name = drivers[i].Name, Age = drivers[i].Age, CarBrand = drivers[i].CarBrand, Rating = drivers[i].Rating };
                }

                return result;
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "GET Method");
                return new DriverInfo[] { new DriverInfo { ErrorMessage = DateTime.Now + " " + e.Message } };
            }
        }

        public DriverInfo[] GetByName(string name)
        {
            try
            {
                var drivers = _context.Drivers.Where(d => d.Name.Contains(name)).Join(_context.Cars, d => d.CarId, c => c.Id, (d, c) => new { d.Name, d.Age, d.Rating, CarBrand = c.Brand }).ToList();

                DriverInfo[] result = new DriverInfo[drivers.Count];

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = new DriverInfo { Name = drivers[i].Name, Age = drivers[i].Age, CarBrand = drivers[i].CarBrand, Rating = drivers[i].Rating };
                }

                return result;
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "GET Method");
                return new DriverInfo[] { new DriverInfo { ErrorMessage = DateTime.Now + " " + e.Message } };
            }
        }

        public string Update(DriverCarPreResultModel driverCar)
        {
            if (!DriversInputValidation.PutMethodInputValidation(driverCar, out string errorMessage)) return errorMessage;

            try
            {
                var driverCarID = _context.Drivers.FirstOrDefault(c => c.Name.Contains(driverCar.DriverName)).CarId;
                var car = _context.Cars.FirstOrDefault(c => c.Id == driverCarID);
                if (car != null)
                {
                    car.Brand = driverCar.CarBrand;
                    Save();
                    return "Info successfully updated";
                }
                return "No changes been made";
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "PUT method");
                return e.Message;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
