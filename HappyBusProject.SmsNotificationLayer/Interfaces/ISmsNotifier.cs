using System.Threading.Tasks;

namespace HappyBusProject.SmsNotificationLayer.Interfaces
{
    public interface ISmsNotifier
    {
        public Task StartNotifierAsync();
    }
}
