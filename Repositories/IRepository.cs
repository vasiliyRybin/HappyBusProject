using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.Repositories
{
    interface IRepository<T> : IDisposable
        where T : class
    {
        IEnumerable<T> GetAllData();
        T GetDataByValue(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        void Save();
    }
}
