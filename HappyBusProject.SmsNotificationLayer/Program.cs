using HappyBusProject.AmazonSMSNotifier;
using HappyBusProject.Notification;
using HappyBusProject.TwilioNotification;
using System;
using System.Threading.Tasks;

namespace HappyBusProject.SmsNotificationLayer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Service.Start(TwilioSMSNotifier);
            ISMSNotifier sms = GetNotifier();
            await sms.StartNotifierAsync();
        }
        private static ISMSNotifier GetNotifier()
        {
#if RELEASEBY
            var accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID", EnvironmentVariableTarget.User);
            var authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN", EnvironmentVariableTarget.User);
            var connectionString = Environment.GetEnvironmentVariable("DBConnectionString", EnvironmentVariableTarget.User);
            return new TwilioSMSNotifier(connectionString,
            accountSid,
            authToken);
#elif RELEASEMD
            return new AmazonSMSNotifier.AmazonSMSNotifier();
#elif RELEASEUA
            var accountSid = AzureKeyVault.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
            var authToken = AzureKeyVault.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
            var connectionString = AzureKeyVault.GetEnvironmentVariable("DBConnectionString");
            return new TwilioSMSNotifier(connectionString, accountSid, authToken);
#elif DEBUG
            return null;//test notification
#endif
        }
    }

    internal class AzureKeyVault
    {
        internal static string GetEnvironmentVariable(string key)
        {
            throw new NotImplementedException();
        }
    }
}
