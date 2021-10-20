using HappyBusProject.Interfaces;
using System;

namespace HappyBusProject.Repositories
{
    interface IUsersRepository<T> : IDisposable, IPerson<T>
        where T : class
    {
        string Create(string name, string phoneNumber, string email);
        string Update(string name, string phoneNumber, string email);
        string Delete(string name);
        void Save();
    }
}
