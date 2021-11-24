using System.Collections.Generic;

namespace HappyBusProject.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> Get();
        bool Create(T item);
        bool Delete(T item);
        T Get(T item);
    }
}