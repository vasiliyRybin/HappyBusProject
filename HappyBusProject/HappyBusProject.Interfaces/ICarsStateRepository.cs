using HappyBusProject.HappyBusProject.DataLayer.InputModels;
using HappyBusProject.HappyBusProject.DataLayer.InputModels.CarStateModels;
using HappyBusProject.HappyBusProject.DataLayer.ViewModels;
using HappyBusProject.Interfaces;
using System.Threading.Tasks;

namespace HappyBusProject.HappyBusProject.Interfaces
{
    public interface ICarsStateRepository<T> : IBusAppObject<T>
        where T : CarStateViewModel
    {
        public Task<T> CreateState(CarStatePostModel newState);
        public void UpdateState(string DriverName, CarStateInputModel carCurrentState);
        public void DeleteState(string DriverName);
    }
}
