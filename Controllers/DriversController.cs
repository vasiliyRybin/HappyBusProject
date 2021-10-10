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
        public DriverInfo[] Get()
        {
            try
            {
                using var db = new MyShuttleBusAppDBContext();
                var drivers = db.Drivers.Join(db.Cars, d => d.CarId, c => c.Id, (d, c) => new { d.Name, d.Age, d.Rating, CarBrand = c.Brand }).ToList();

                DriverInfo[] result = new DriverInfo[drivers.Count];

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = new DriverInfo { Name = drivers[i].Name, Age = drivers[i].Age, CarBrand = drivers[i].CarBrand, Rating = drivers[i].Rating };
                }

                return result;
            }
            catch (Exception e)
            {
                AppTools.ErrorWriter(e.Message);
                return Array.Empty<DriverInfo>();
            }
        }

        [HttpGet("{name}")]
        public DriverInfo[] Get(string name)
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

                return result;
            }
            catch (Exception e)
            {
                AppTools.ErrorWriter(e.Message);
                return Array.Empty<DriverInfo>();
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
            if (driverName.Length > 50 || !new Regex(@"\w{1,15}\s\w{1,15}\s\w{1,15}").IsMatch(driverName)) return "Invalid name";
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
                AppTools.ErrorWriter(e.Message);
                return e.Message;
            }
        }

        [HttpPut]
        public string TestPut()
        {
            return "Put Method";
        }

        [HttpDelete]
        public string TestDelete()
        {
            return "Deleted";
        }
    }
}
