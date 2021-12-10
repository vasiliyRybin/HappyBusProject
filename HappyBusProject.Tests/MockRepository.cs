using System;
using System.Collections.Generic;

namespace HappyBusProject.Tests
{
    public static class MockRepository
    {
        const string firstCarID = "AB68891B-962F-4B4D-9A15-325276F6307F";
        const string secondCarID = "F6CA737A-71BE-4B70-8B6B-34903BAC51A9";
        const string thirdCarID = "D6C08780-C4DC-4397-9DE4-57055858A9B6";
        const string fourthCarID = "B684EEB9-0782-4A81-B7B4-88D1EA176BCA";

        const int car1Seats = 10;
        const int car2Seats = 15;
        const int car3Seats = 20;
        const int car4Seats = 25;

        public static IEnumerable<Driver> GetTestDrivers()
        {
            var drivers = new List<Driver>
            {
                new Driver{ DriverName = "Vitalii", DriverAge = 28, Rating = 5.0, CarId = Guid.Parse(firstCarID), Id = Guid.Parse("19CDBF99-75D2-4E11-8A72-055EC5EB8EEA")},
                new Driver{ DriverName = "Kate", DriverAge = 25, Rating = 4.5, CarId = Guid.Parse(secondCarID), Id = Guid.Parse("ABD7E13D-BEAF-4A64-A94D-1466B3CD5387")},
                new Driver{ DriverName = "Lehonti", DriverAge = 27, Rating = 5.0, CarId = Guid.Parse(thirdCarID), Id = Guid.Parse("8B34158E-2E13-48CC-B390-2A907E6BA8A2")},
                new Driver{ DriverName = "Nikolai", DriverAge = 45, Rating = 4.1, CarId = Guid.Parse(fourthCarID), Id = Guid.Parse("BE9D82F5-039C-4BDD-A25F-4F584E1DBC69")}
            };
            return drivers;
        }

        public static IEnumerable<Car> GetTestCars()
        {
            var cars = new List<Car>
            {
                new Car{ CarBrand = "SomeCar1", CarAge = 2, CarId = Guid.Parse(firstCarID), RegistrationNumPlate = "7654 PH-1", SeatsNum = car1Seats},
                new Car{ CarBrand = "SomeCar2", CarAge = 3, CarId = Guid.Parse(secondCarID), RegistrationNumPlate = "7654 PH-1", SeatsNum = car2Seats},
                new Car{ CarBrand = "SomeCar3", CarAge = 4, CarId = Guid.Parse(thirdCarID), RegistrationNumPlate = "7654 PH-1", SeatsNum = car3Seats},
                new Car{ CarBrand = "SomeCar4", CarAge = 5, CarId = Guid.Parse(fourthCarID), RegistrationNumPlate = "7654 PH-1", SeatsNum = car4Seats}
            };
            return cars;
        }

        public static IEnumerable<CarsCurrentState> GetTestCarStates()
        {
            var states = new List<CarsCurrentState>
            {
                new CarsCurrentState{ Id = Guid.Parse(firstCarID), DepartureTime = DateTime.Parse("2021-12-10 17:00:00.0000000"), SeatsNum = car1Seats, FreeSeatsNum = car1Seats, IsBusyNow = false},
                new CarsCurrentState{ Id = Guid.Parse(firstCarID), DepartureTime = DateTime.Parse("2021-12-10 17:00:00.0000000"), SeatsNum = car2Seats, FreeSeatsNum = car2Seats, IsBusyNow = false},
                new CarsCurrentState{ Id = Guid.Parse(firstCarID), DepartureTime = DateTime.Parse("2021-12-10 17:00:00.0000000"), SeatsNum = car3Seats, FreeSeatsNum = car3Seats, IsBusyNow = false},
                new CarsCurrentState{ Id = Guid.Parse(firstCarID), DepartureTime = DateTime.Parse("2021-12-10 17:00:00.0000000"), SeatsNum = car4Seats, FreeSeatsNum = car4Seats, IsBusyNow = false}
            };
            return states;
        }
    }
}
