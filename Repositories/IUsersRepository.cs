using System;

namespace HappyBusProject.Repositories
{
    interface IUsersRepository<T> : IDisposable
        where T : class
    {
        T GetAllUsers();
        T GetUserByName(string value);
        string Create(string name, string phoneNumber, string email);
        string Update(string name, string phoneNumber, string email);
        string Delete(string name);
        void Save();
    }
}
