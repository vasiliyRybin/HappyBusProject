using HappyBusProject.Interfaces;
using HappyBusProject.ModelsToReturn;

namespace HappyBusProject.Repositories
{
    interface IDriversRepository<T> : IPerson<T>
        where T : class
    {
        string Create(DriverCarPreResultModel driverInfo);
        string Update(DriverCarPreResultModel driverInfo);
        string Delete(string name);
        void Save();
    }
}
