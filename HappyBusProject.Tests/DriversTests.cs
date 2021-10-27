using HappyBusProject.ModelsToReturn;
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

        [Fact]
        public async Task GetAllReturnListOfDrivers()
        {
            var testDrivers = await GetTestDrivers();
            int result = testDrivers.Count;

            Assert.Equal(4, result);
        }
    }
}
