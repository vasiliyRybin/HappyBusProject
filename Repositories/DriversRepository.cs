using HappyBusProject.ModelsToReturn;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace HappyBusProject.Repositories
{
    public class DriversRepository : IDriversRepository<DriverInfo[]>
    {
        private readonly MyShuttleBusAppNewDBContext _context;
        private bool disposedValue = false;

        public DriversRepository(MyShuttleBusAppNewDBContext myShuttleBusAppNewDBContext)
        {
            _context = myShuttleBusAppNewDBContext;
        }

        public string Create(string brand, string seatsNum, string registrationNumPlate, string carAge, string driverName, string driverAge, string examPass = "1900-01-01 00:00:00")
        {
            if (string.IsNullOrWhiteSpace(brand) || string.IsNullOrWhiteSpace(seatsNum) || string.IsNullOrWhiteSpace(registrationNumPlate) || string.IsNullOrWhiteSpace(carAge)) return "Car info fields shall not be empty";
            if (string.IsNullOrWhiteSpace(driverName) || string.IsNullOrWhiteSpace(driverAge)) return "Driver info fields shall not be empty";
            if (brand.Length > 30) return "Invalid brand name";
            if (!int.TryParse(seatsNum, out int numSeats) || numSeats <= 8 || numSeats > 50) return "Invalid number of seats";
            if (registrationNumPlate.Length > 9 || !new Regex(@"\d{4}\s\w{2}.\d{1}").IsMatch(registrationNumPlate)) return "Invalid registration plate number";
            if (!int.TryParse(carAge, out int carAgeInt) || carAgeInt < 0 || carAgeInt > 15) return "Invalid car age or car age is too big";
            if (driverName.Length > 50 || !new Regex(pattern: @"(^[a-zA-Z '-]{1,25})|(^[А-Яа-я '-]{1,25})").IsMatch(driverName)) return "Invalid name";
            if (!int.TryParse(driverAge, out int driverAgeInt) || driverAgeInt < 21 || driverAgeInt > 65) return "Invalid age.";
            DateTime.TryParse(examPass, out DateTime resultExamPass);

            try
            {
                var carGuid = Guid.NewGuid();

                Car car = new()
                {
                    Id = carGuid,
                    Brand = brand,
                    SeatsNum = numSeats,
                    RegistrationNumPlate = registrationNumPlate,
                    Age = carAgeInt
                };

                Driver driver = new()
                {
                    Name = driverName,
                    Age = driverAgeInt,
                    Id = Guid.NewGuid(),
                    CarId = carGuid,
                    Rating = 5.0,
                    MedicalExamPassDate = resultExamPass
                };

                _context.Drivers.Add(driver);
                _context.Cars.Add(car);
                int successUpdate = _context.SaveChanges();
                Dispose();
                if (successUpdate > 0) return "Driver succesfully added";
                else return "No changes been made";

            }
            catch (Exception e)
            {
                AppTools.ErrorWriterTpFile(e.Message);
                Dispose();
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
                    Dispose();
                    return "Driver successfully deleted";
                }
                return "No changes been made";

            }
            catch (Exception e)
            {
                AppTools.ErrorWriterTpFile(e.Message + " " + "DELETE method");
                Dispose();
                return e.Message;
            }
        }

        public DriverInfo[] GetAllDrivers()
        {
            try
            {
                var drivers = _context.Drivers.Join(_context.Cars, d => d.CarId, c => c.Id, (d, c) => new { d.Name, d.Age, d.Rating, CarBrand = c.Brand }).ToList();

                DriverInfo[] result = new DriverInfo[drivers.Count];

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = new DriverInfo { Name = drivers[i].Name, Age = drivers[i].Age, CarBrand = drivers[i].CarBrand, Rating = drivers[i].Rating };
                }

                Dispose();
                return result;
            }
            catch (Exception e)
            {
                AppTools.ErrorWriterTpFile(e.Message + " " + "GET Method");
                Dispose();
                return new DriverInfo[] { new DriverInfo { ErrorMessage = DateTime.Now + " " + e.Message } };
            }
        }

        public DriverInfo[] GetDriverByName(string name)
        {
            try
            {
                var drivers = _context.Drivers.Where(d => d.Name.Contains(name)).Join(_context.Cars, d => d.CarId, c => c.Id, (d, c) => new { d.Name, d.Age, d.Rating, CarBrand = c.Brand }).ToList();

                DriverInfo[] result = new DriverInfo[drivers.Count];

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = new DriverInfo { Name = drivers[i].Name, Age = drivers[i].Age, CarBrand = drivers[i].CarBrand, Rating = drivers[i].Rating };
                }

                Dispose();
                return result;
            }
            catch (Exception e)
            {
                AppTools.ErrorWriterTpFile(e.Message + " " + "GET Method");
                Dispose();
                return new DriverInfo[] { new DriverInfo { ErrorMessage = DateTime.Now + " " + e.Message } };
            }
        }

        public string Update(string name, string newCarBrand)
        {
            try
            {
                var driverCarID = _context.Drivers.FirstOrDefault(c => c.Name.Contains(name)).CarId;
                var car = _context.Cars.FirstOrDefault(c => c.Id == driverCarID);
                if (car != null)
                {
                    car.Brand = newCarBrand;
                    Save();
                    Dispose();
                    return "Info successfully updated";
                }
                return "No changes been made";
            }
            catch (Exception e)
            {
                AppTools.ErrorWriterTpFile(e.Message + " " + "PUT method");
                Dispose();
                return e.Message;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
