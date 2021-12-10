using HappyBusProject.InputModels;
using HappyBusProject.InputValidators;
using HappyBusProject.Interfaces;
using HappyBusProject.Services;
using HappyBusProject.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HappyBusProject.Tests
{
    public class DriversTests
    {


        private static DriverViewModel CreateDriver(DriverCarInputModel driverCar, out string errorMessage)
        {
            var isNotValid = DriversInputValidation.DriversInputValidator(driverCar, out errorMessage);
            if (!isNotValid) return new DriverViewModel();

            try
            {
                DriverViewModel driverInfo = new()
                {
                    DriverAge = driverCar.DriverAge,
                    DriverName = driverCar.DriverName,
                    CarBrand = driverCar.CarBrand,
                    Rating = 5.0
                };

                if (driverInfo != null) return new DriverViewModel();
                else return new DriverViewModel();
            }
            catch (Exception)
            {
                return null;
            }
        }

        [Fact]
        public void GetAllDrivers()
        {
            var driverMock = new Mock<IRepository<Driver>>();
            driverMock.Setup(c => c.Get().Result).Returns(MockRepository.GetTestDrivers());

            var carMock = new Mock<IRepository<Car>>();
            carMock.Setup(c => c.Get().Result).Returns(MockRepository.GetTestCars());

            var currentStateMock = new Mock<IRepository<CarsCurrentState>>();
            currentStateMock.Setup(c => c.Get().Result).Returns(MockRepository.GetTestCarStates());

            var controller = new DriverService(driverMock.Object, carMock.Object, currentStateMock.Object, null, null);

            var result = controller.GetAllAsync().Result;

            var viewResult = Assert.IsType<DriverViewModel[]>(result);
            Assert.Equal(MockRepository.GetTestDrivers().Count(), viewResult.Length);
        }


        [Fact]
        public void GetAllReturnListOfDrivers()
        {
            var testDrivers = MockRepository.GetTestDrivers();
            int result = testDrivers.Count();

            Assert.Equal(4, result);
        }

        [Fact]
        public void CreateDriverWrongInput()
        {
            var newDriver = new DriverCarInputModel()
            {
                DriverName = "Grisha Pumpkin",
                DriverAge = 85,
                CarBrand = "Some Helicopter",
                CarAge = 20,
                SeatsNum = 21,
                MedicalExamPassDate = DateTime.Parse("1900-01-01"),
                RegistrationNumPlate = "5544 PL-5"
            };

            _ = CreateDriver(newDriver, out string errorMessage);
            Assert.NotEmpty(errorMessage);
        }

        [Fact]
        public void DeleteDriver_DriverNotFound()
        {
            var driverToDelete = MockRepository.GetTestDrivers();
            var result = DeleteDriver(driverToDelete, " ");
            Assert.False(result);
        }

        public static bool DeleteDriver(IEnumerable<Driver> drivers, string FullName)
        {
            var driver = drivers.FirstOrDefault(c => c.DriverName == FullName);
            if (driver != null) return true;
            return false;
        }
    }
}
