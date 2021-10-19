using System;

namespace HappyBusProject.Repositories
{
    interface IDriversRepository<T> : IDisposable
        where T : class
    {
        T GetAllDrivers();
        T GetDriverByName(string name);
        string Create(string brand, string seatsNum, string registrationNumPlate, string carAge, string driverName, string driverAge, string examPass = "1900-01-01 00:00:00");
        string Update(string name, string newCarBrand);
        string Delete(string name);
        void Save();
    }
}
