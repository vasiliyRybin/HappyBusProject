using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HappyBusProject.Interfaces
{
    public interface IRepository<T>
        where T : class
    {
        Task<IEnumerable<T>> Get();
        Task<T> GetFirstOrDefault(Func<T, bool> predicate);
        Task<bool> Create(T item);
        Task<bool> Update(T ValuesToModify);
        Task<bool> Delete(T item);
    }
}
