using System.Threading.Tasks;

namespace HappyBusProject.Interfaces
{
    public interface IOrderRepository<T, T2, T3> : IBusAppObject<T>
        where T : class
        where T2 : class
        where T3 : class
    {
        Task<T> CreateOrder(T2 orderInput);
        public void UpdateOrder(T3 putModel);
        public void DeleteOrder(string FullName);
    }
}
