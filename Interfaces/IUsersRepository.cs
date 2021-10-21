using HappyBusProject.Interfaces;
using HappyBusProject.ModelsToReturn;

namespace HappyBusProject.Repositories
{
    interface IUsersRepository<T> : IPerson<T>
        where T : class
    {
        string Create(UsersInfo usersInfo);
        string Update(UsersInfo usersInfo);
        string Delete(string name);
        void Save();
    }
}
