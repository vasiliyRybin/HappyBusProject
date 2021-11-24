using HappyBusProject.Interfaces;
using System.Threading.Tasks;

namespace HappyBusProject.HappyBusProject.Interfaces
{
    public interface IOrderRepository<T> : IBusAppObject<T>
    {
        Task<T> CreateOrder(T entity);
        public void UpdateOrder(T entity);
        public void DeleteOrder(string FullName);
    }
}
