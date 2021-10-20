using HappyBusProject.Interfaces;
using System;

namespace HappyBusProject.Repositories
{
    interface IDriversRepository<T> : IDisposable, IPerson<T>
        where T : class
    {
        string Create(string brand, string seatsNum, string registrationNumPlate, string carAge, string driverName, string driverAge, string examPass = "1900-01-01 00:00:00");
        string Update(string name, string newCarBrand);
        string Delete(string name);
        void Save();
    }
}
