using System.Threading.Tasks;

namespace HappyBusProject.Interfaces
{
    public interface ICarsStateRepository<T, T1, T2> : IBusAppObject<T>
        where T : class
        where T1 : class
        where T2 : class
    {
        public Task<T> CreateState(T1 newState);
        public void UpdateState(string DriverName, T2 carCurrentState);
        public void DeleteState(string DriverName);
    }
}
