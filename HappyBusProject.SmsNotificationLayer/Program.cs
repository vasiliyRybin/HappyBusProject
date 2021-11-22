using HappyBusProject.HappyBusProject.BusinessLayer.Notifier;
using System.Threading.Tasks;

namespace HappyBusProject.SmsNotificationLayer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var sms = new TwilioSMSNotifier();
            await sms.StartNotifierAsync();
        }
    }
}
