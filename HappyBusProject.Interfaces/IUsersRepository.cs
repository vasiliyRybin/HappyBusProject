using System.Threading.Tasks;

namespace HappyBusProject.Interfaces
{
    public interface IUsersRepository<T, T2> : IBusAppObject<T>
        where T : class
        where T2 : class
    {
        Task<T> CreateAsync(T2 usersInfo);
        public void UpdateAsync(T2 usersInfo);
        public void DeleteAsync(string name);
    }
}
