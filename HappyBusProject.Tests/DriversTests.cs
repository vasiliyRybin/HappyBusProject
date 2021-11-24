using HappyBusProject.InputValidators;
using HappyBusProject.ModelsToReturn;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using HappyBusProject.Interfaces;
using HappyBusProject.HappyBusProject.DataLayer.Models;
using HappyBusProject.Web.Services;

namespace HappyBusProject.Tests
{
    public class DriversTests
    {
        //TODO: A driver with the same registration plate can't be added twice

        private async Task<List<DriverViewModel>> GetTestDrivers()
        {
            var drivers = new List<DriverViewModel>
            {
                new DriverViewModel{ DriverName = "Vitalii", DriverAge = 28, Rating = 5.0},
                new DriverViewModel{ DriverName = "Kate", DriverAge = 25, Rating = 4.5},
                new DriverViewModel{ DriverName = "Lehonti", DriverAge = 27, Rating = 5.0},
                new DriverViewModel{ DriverName = "Nikolai", DriverAge = 45, Rating = 4.1}
            };
            return await Task.FromResult(drivers);
        }

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
        public async Task DeleteDriver_DriverNotFoundAsync()
        {
            var driverToDelete = await GetTestDrivers();
            var result = DeleteDriver(driverToDelete, " ");
            Assert.False(result);
        }

        public static bool DeleteDriver(List<DriverViewModel> drivers, string FullName)
        {
            var driver = drivers.Find(c => c.DriverName == FullName);
            if (driver != null) return true;
            return false;
        }

        [Fact]
        public async void CreateDriverTest()
        {
            var carMock = new Mock<IRepository<Car>>();
            IRepository<Driver> _drRepository;// = new Mock<IRepository<Driver>>();
            IRepository<CarsCurrentState> _stRepository;// = new Mock<IRepository<CarsCurrentState>>();

            //repo.Setup(x => x.GetAll()).Returns(Task.FromResult(mock.Object.AsEnumerable()));
            //Create - mock for 3 repositores!!!!
            //carMock.Obj
            var service = new DriverService(null, null/*pass mocks here*/);

            var driverVM = await service.CreateAsync(null/*pass VM here*/);

            Assert.Equal(11, driverVM.DriverAge);
            //all properties of driverVM should be validated
        }

    }
}
