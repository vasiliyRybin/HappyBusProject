using System;
using System.Collections.Generic;

namespace HappyBusProject.Repositories
{
    interface IDriversRepository<T> : IDisposable
        where T : class
    {
        T GetAllData();
        T GetDataByValue(string value);
        void Create(string name, string phoneNumber, string email);
        void Update(string name, string phoneNumber, string email);
        void Delete(string name);
        void Save();
    }
}
