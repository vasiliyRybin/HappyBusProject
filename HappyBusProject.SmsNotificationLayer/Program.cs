using HappyBusProject.HappyBusProject.BusinessLayer.Notifier;
using HappyBusProject.SmsNotificationLayer.Interfaces;
using System.Threading.Tasks;

namespace HappyBusProject.SmsNotificationLayer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ISmsNotifier sms = new TwilioSMSNotifier();
            await sms.StartNotifierAsync();
        }
    }
}
