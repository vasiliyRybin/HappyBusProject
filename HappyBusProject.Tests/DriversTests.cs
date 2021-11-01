using HappyBusProject.InputValidators;
using HappyBusProject.ModelsToReturn;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HappyBusProject.Tests
{
    public class DriversTests
    {
        //TODO: A driver with the same registration plate can't be added twice

        private async Task<List<DriverViewModel>> GetTestDrivers()
        {
            var drivers = new List<DriverViewModel>
            {
                new DriverViewModel{ Name = "Vitalii", Age = 28, Rating = 5.0},
                new DriverViewModel{ Name = "Kate", Age = 25, Rating = 4.5},
                new DriverViewModel{ Name = "Lehonti", Age = 27, Rating = 5.0},
                new DriverViewModel{ Name = "Nikolai", Age = 45, Rating = 4.1}
            };
            return await Task.FromResult(drivers);
        }

        private DriverViewModel CreateDriver(DriverCarInputModel driverCar, out string errorMessage)
        {
            var _context = new List<DriverViewModel>();

            var isNotValid = DriversInputValidation.DriversInputValidator(driverCar, out int numSeats, out int carAgeInt, out int driverAgeInt, out DateTime resultExamPass, out errorMessage);
            if (!isNotValid) return new DriverViewModel();

            try
            {
                DriverViewModel driverInfo = new()
                {
                    Age = driverAgeInt,
                    Name = driverCar.DriverName,
                    CarBrand = driverCar.CarBrand,
                    Rating = 5.0
                };

                if (driverInfo != null) return new DriverViewModel();
                else return new DriverViewModel();
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message);
                return new DriverViewModel();
            }
        }




        [Fact]
        public async Task GetAllReturnListOfDrivers()
        {
            var testDrivers = await GetTestDrivers();
            int result = testDrivers.Count;

            Assert.Equal(4, result);
        }

        [Fact]
        public void CreateDriverWrongInput()
        {
            var newDriver = new DriverCarInputModel()
            {
                DriverName = "Grisha Pumpkin",
                DriverAge = "85",
                CarBrand = "Some Helicopter",
                CarAge = "20",
                SeatsNum = "21",
                ExamPass = "1900-01-01",
                RegistrationNumPlate = "5544 PL-5"
            };

            _ = CreateDriver(newDriver, out string errorMessage);
            Assert.NotEmpty(errorMessage);
        }
    }
}
