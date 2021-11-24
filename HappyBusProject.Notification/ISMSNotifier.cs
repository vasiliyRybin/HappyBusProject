using System.Threading.Tasks;

namespace HappyBusProject.Notification
{
    public interface ISMSNotifier
    {
        Task StartNotifierAsync();
    }
}