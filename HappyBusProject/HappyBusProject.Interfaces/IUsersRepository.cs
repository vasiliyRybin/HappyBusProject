using HappyBusProject.HappyBusProject.DataLayer.InputModels;
using HappyBusProject.Interfaces;
using HappyBusProject.ModelsToReturn;
using System.Threading.Tasks;

namespace HappyBusProject.Repositories
{
    public interface IUsersRepository<T> : IBusAppObject<T>
        where T : UsersViewModel
    {
        Task<T> CreateAsync(UserInputModel usersInfo);
        public void UpdateAsync(UserInputModel usersInfo);
        public void DeleteAsync(string name);
    }
}
