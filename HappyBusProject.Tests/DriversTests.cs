using AutoMapper;
using HappyBusProject.InputModels;
using HappyBusProject.Interfaces;
using HappyBusProject.MappingProfiles;
using HappyBusProject.Services;
using HappyBusProject.ViewModels;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace HappyBusProject.Tests
{
    public class DriversTests
    {
        public IMapper Mapper { get; }

        public DriversTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DriverProfile());
                mc.AddProfile(new CarsCurrentStateProfile());
                mc.AddProfile(new CarProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            Mapper = mapper;
        }


        private void InitialiseDriverRelatedRepositories(out Mock<IRepository<Driver>> driverMock, out Mock<IRepository<Car>> carMock, out Mock<IRepository<CarsCurrentState>> currentStateMock, out DriverService controller)
        {
            driverMock = new Mock<IRepository<Driver>>();
            driverMock.Setup(c => c.Get().Result).Returns(MockRepository.GetTestDrivers());

            carMock = new Mock<IRepository<Car>>();
            carMock.Setup(c => c.Get().Result).Returns(MockRepository.GetTestCars());

            currentStateMock = new Mock<IRepository<CarsCurrentState>>();
            currentStateMock.Setup(c => c.Get().Result).Returns(MockRepository.GetTestCarStates());

            controller = new DriverService(driverMock.Object, carMock.Object, currentStateMock.Object, new NullLogger<DriverService>(), Mapper);
        }

        [Fact]
        public void GetDriverByName_DriverFound()
        {
            string DriverName = "Vitalii";
            Guid CarGUID = Guid.Parse("AB68891B-962F-4B4D-9A15-325276F6307F");


            InitialiseDriverRelatedRepositories(out Mock<IRepository<Driver>> driverMock, out Mock<IRepository<Car>> carMock, out Mock<IRepository<CarsCurrentState>> currentStateMock, out DriverService controller);
            driverMock.Setup(s => s.GetFirstOrDefault(It.IsAny<Func<Driver, bool>>()).Result)
                .Returns(MockRepository.GetTestDrivers().FirstOrDefault(d => d.DriverName == DriverName));
            carMock.Setup(s => s.GetFirstOrDefault(It.IsAny<Func<Car, bool>>()).Result)
                .Returns(MockRepository.GetTestCars().FirstOrDefault(d => d.CarId == CarGUID));

            controller = new DriverService(driverMock.Object, carMock.Object, currentStateMock.Object, new NullLogger<DriverService>(), Mapper);

            var result = controller.GetByNameAsync(DriverName).Result;

            Assert.IsType<DriverViewModel>(result);
            Assert.NotNull(result);
        }


        [Fact]
        public void GetAllDrivers_CheckOutputType_InputCountEqualsOutputCount()
        {
            //Arrange
            InitialiseDriverRelatedRepositories(out _, out _, out _, out DriverService controller);

            //Act
            var result = controller.GetAllAsync().Result;

            //Assert
            var viewResult = Assert.IsType<DriverViewModel[]>(result);
            Assert.Equal(MockRepository.GetTestDrivers().Count(), viewResult.Length);
        }

        [Fact]
        public void CreateDriverWrongInput()
        {
            InitialiseDriverRelatedRepositories(out Mock<IRepository<Driver>> driverMock, out Mock<IRepository<Car>> carMock, out Mock<IRepository<CarsCurrentState>> currentStateMock, out DriverService controller);

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

            var result = controller.CreateAsync(newDriver).Result;

            Assert.Null(result); //Fix it!!!
        }

        [Fact]
        public void CreateDriver_Success()
        {
            var carID = Guid.NewGuid();
            var driverID = Guid.NewGuid();
            var newDriver = new DriverCarInputModel()
            {
                DriverName = "Grisha Pumpkin",
                DriverAge = 35,
                CarBrand = "Some Helicopter",
                CarAge = 2,
                SeatsNum = 21,
                MedicalExamPassDate = DateTime.Parse("2021-02-01"),
                RegistrationNumPlate = "5544 PL-5"
            };

            var driver = Mapper.Map<Driver>(newDriver);
            driver.CarId = carID;
            driver.Id = driverID;
            driver.Rating = 5.0;

            var car = Mapper.Map<Car>(driver);
            Mapper.Map(newDriver, car);

            var carState = Mapper.Map<CarsCurrentState>(car);
            carState.DepartureTime = DateTime.Now;
            carState.FreeSeatsNum = carState.SeatsNum;

            InitialiseDriverRelatedRepositories(out Mock<IRepository<Driver>> driverMock, out Mock<IRepository<Car>> carMock, out Mock<IRepository<CarsCurrentState>> currentStateMock, out DriverService controller);
            driverMock.Setup(c => c.Create(It.IsAny<Driver>()).Result).Returns(true);
            carMock.Setup(c => c.Create(It.IsAny<Car>()).Result).Returns(true);
            currentStateMock.Setup(c => c.Create(It.IsAny<CarsCurrentState>()).Result).Returns(true);


            var result = controller.CreateAsync(newDriver).Result;

            Assert.NotNull(result);
            Assert.IsType<DriverViewModel>(result);
        }

        [Fact]
        public void DeleteDriver_DriverFound()
        {
            var someDriver = MockRepository.GetTestDrivers().FirstOrDefault(d => d.DriverName == "Vitalii");
            var someCar = MockRepository.GetTestCars().FirstOrDefault(c => c.CarId == someDriver.CarId);
            var someCarState = MockRepository.GetTestCarStates().FirstOrDefault(s => s.Id == someCar.CarId);

            InitialiseDriverRelatedRepositories(out Mock<IRepository<Driver>> driverMock, out Mock<IRepository<Car>> carMock, out Mock<IRepository<CarsCurrentState>> currentStateMock, out DriverService controller);

            driverMock.Setup(s => s.GetFirstOrDefault(It.IsAny<Func<Driver, bool>>()).Result)
                .Returns(MockRepository.GetTestDrivers().FirstOrDefault(d => d.DriverName == someDriver.DriverName));
            driverMock.Setup(s => s.Delete(It.IsAny<Driver>()).Result).Returns(true);

            carMock.Setup(s => s.GetFirstOrDefault(It.IsAny<Func<Car, bool>>()).Result)
                .Returns(MockRepository.GetTestCars().FirstOrDefault(d => d.CarId == someCar.CarId));
            carMock.Setup(s => s.Delete(It.IsAny<Car>()).Result).Returns(true);

            currentStateMock.Setup(s => s.GetFirstOrDefault(It.IsAny<Func<CarsCurrentState, bool>>()).Result)
                .Returns(MockRepository.GetTestCarStates().FirstOrDefault(d => d.Id == someCar.CarId));
            currentStateMock.Setup(s => s.Delete(It.IsAny<CarsCurrentState>()).Result).Returns(true);

            var result = controller.DeleteDriver(someDriver.DriverName).Result;

            Assert.True(result);
        }
    }
}
