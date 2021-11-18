using HappyBusProject.HappyBusProject.BusinessLayer.Notifier;
using System.Threading.Tasks;

namespace HappyBusProject.SmsNotificationLayer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ISMSNotifier sms = new AmazonSMSNotifier();
                //new TwilioSMSNotifier();

            await sms.StartNotifierAsync();
        }
    }
}
