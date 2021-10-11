using HappyBusProject.ModelsToReturn;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HappyBusProject.Controllers
{
    [ApiController]
    [Route("AppAPI/Drivers")]
    public class DriversController : ControllerBase
    {
        private const string connectionString = "Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;";
        private readonly ILogger<DriversController> _logger;

        public DriversController(ILogger<DriversController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public Tuple<DriverInfo[], string> Get()
        {
            try
            {
                //var test1 = 0;
                //var test2 = 10 / test1;
                using var db = new MyShuttleBusAppDBContext();
                var drivers = db.Drivers.Join(db.Cars, d => d.CarId, c => c.Id, (d, c) => new { d.Name, d.Age, d.Rating, CarBrand = c.Brand }).ToList();

                DriverInfo[] result = new DriverInfo[drivers.Count];

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = new DriverInfo { Name = drivers[i].Name, Age = drivers[i].Age, CarBrand = drivers[i].CarBrand, Rating = drivers[i].Rating };
                }

                return new (result, string.Empty);
            }
            catch (Exception e)
            {
                AppTools.ErrorWriterTpFile(e.Message + "GET Method");
                return new(Array.Empty<DriverInfo>(), DateTime.Now + " " + e.Message);
            }
        }

        [HttpGet("{name}")]
        public Tuple<DriverInfo[], string> Get(string name)
        {
            try
            {
                using var db = new MyShuttleBusAppDBContext();
                var drivers = db.Drivers.Where(d => d.Name.Contains(name)).Join(db.Cars, d => d.CarId, c => c.Id, (d, c) => new { d.Name, d.Age, d.Rating, CarBrand = c.Brand }).ToList();

                DriverInfo[] result = new DriverInfo[drivers.Count];

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = new DriverInfo { Name = drivers[i].Name, Age = drivers[i].Age, CarBrand = drivers[i].CarBrand, Rating = drivers[i].Rating };
                }

                return new(result, string.Empty);
            }
            catch (Exception e)
            {
                AppTools.ErrorWriterTpFile(e.Message + "GET Method");
                return new(Array.Empty<DriverInfo>(), DateTime.Now + " " + e.Message);
            }
        }

        [HttpPost("{brand}/{seatsNum}/{registrationNumPlate}/{carAge}/{driverName}/{driverAge}/{examPass}")]
        public string PostTest(string brand, string seatsNum, string registrationNumPlate, string carAge, string driverName, string driverAge, string examPass = "1900-01-01 00:00:00")
        {
            if (string.IsNullOrWhiteSpace(brand) || string.IsNullOrWhiteSpace(seatsNum) || string.IsNullOrWhiteSpace(registrationNumPlate) || string.IsNullOrWhiteSpace(carAge)) return "Car info fields shall not be empty";
            if (string.IsNullOrWhiteSpace(driverName) || string.IsNullOrWhiteSpace(driverAge)) return "Driver info fields shall not be empty";
            if (brand.Length > 30) return "Invalid brand name";
            if (!int.TryParse(seatsNum, out int numSeats) || numSeats <= 8 || numSeats > 50) return "Invalid number of seats";
            if (registrationNumPlate.Length > 9 || !new Regex(@"\d{4}\s\w{2}.\d{1}").IsMatch(registrationNumPlate)) return "Invalid registration plate number";
            if (!int.TryParse(carAge, out int carAgeInt) || carAgeInt < 0 || carAgeInt > 15) return "Invalid car age or car age is too big";
            //if (driverName.Length > 50 || !new Regex(@"\w{1,15}\s\w{1,15}\s\w{1,15}").IsMatch(driverName)) return "Invalid name";
            if (!int.TryParse(driverAge, out int driverAgeInt) || driverAgeInt < 21 || driverAgeInt > 65) return "Invalid age.";
            if (DateTime.TryParse(examPass, out DateTime result)) examPass = result.ToString();

            try
            {
                using var connection = new SqlConnection(connectionString);
                SqlCommand command = new(DBQueriesClass.CreateNewDriver(brand, seatsNum, registrationNumPlate, carAge, driverName, driverAge, examPass), connection)
                {
                    CommandTimeout = 30
                };

                connection.Open();
                var SQLquery = command.ExecuteReader();
                SQLquery.Close();
                return "Driver succesfully added";
            }
            catch (Exception e)
            {
                AppTools.ErrorWriterTpFile(e.Message);
                return e.Message;
            }
        }

        [HttpPut("{driverName}/{newCarBrand}")]
        public string TestPut( string driverName, string newCarBrand)
        {
            try
            {
                using var db = new MyShuttleBusAppDBContext();
                {
                    var driverCarID = db.Drivers.FirstOrDefault(c => c.Name.Contains(driverName)).CarId;
                    var car = db.Cars.FirstOrDefault(c => c.Id == driverCarID);
                    if (car != null)
                    {
                        car.Brand = newCarBrand;
                        db.SaveChanges();
                        return "Info successfully updated";
                    }
                    return "No changes been made";
                }
            }
            catch (Exception e)
            {
                AppTools.ErrorWriterTpFile(e.Message + "PUT method");
                return e.Message;
            }
        }

        [HttpDelete("{driverName}")]
        public string TestDelete(string driverName)
        {
            try
            {
                using var db = new MyShuttleBusAppDBContext();
                {
                    var driver = db.Drivers.FirstOrDefault(c => c.Name.Contains(driverName));
                    var carToRemove = db.Cars.FirstOrDefault(c => c.Id == driver.CarId);

                    if (driver != null && carToRemove != null)
                    {
                        db.Remove(driver);
                        db.Remove(carToRemove);
                        db.SaveChanges();
                        return "Driver successfully deleted";
                    }
                    return "No changes been made";
                }
            }
            catch (Exception e)
            {
                AppTools.ErrorWriterTpFile(e.Message + "DELETE method");
                return e.Message;
            }
        }
    }
}
