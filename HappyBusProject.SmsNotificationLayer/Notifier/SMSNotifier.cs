using HappyBusProject.SmsNotificationLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace HappyBusProject.HappyBusProject.BusinessLayer.Notifier
{
    public class SMSNotifier
    {
        private Dictionary<string, string> _usersToNotify = new();
        private readonly string _connectionString;
        private readonly string _accountSid;
        private readonly string _authToken;

        public SMSNotifier()
        {
            _accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID", EnvironmentVariableTarget.User);
            _authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN", EnvironmentVariableTarget.User);
            _connectionString = Environment.GetEnvironmentVariable("DBConnectionString", EnvironmentVariableTarget.User);
        }

        public async Task StartNotifierAsync()
        {
            while (true)
            {
                try
                {
                    await Task.Delay(10000);

                    using SqlConnection connection = new(_connectionString);
                    connection.Open();
                    SqlCommand command = new(Queries.GetAllNotNotifiedUsers(), connection);
                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            _usersToNotify.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                        }

                        foreach (var item in _usersToNotify)
                        {
                            var phoneNumber = item.Value;
                            if (!string.IsNullOrWhiteSpace(phoneNumber))
                            {
                                TwilioClient.Init(_accountSid, _authToken);

                                var message = await MessageResource.CreateAsync(
                                    body: "SMS API Testing",
                                    from: new Twilio.Types.PhoneNumber("+19282725653"),
                                    to: new Twilio.Types.PhoneNumber($"+{phoneNumber}")
                                );

                                if (message.Status == MessageResource.StatusEnum.Queued)
                                {
                                    reader.Close();
                                    string query = Queries.UpdateAfterNotification(_usersToNotify.First(k => k.Value == phoneNumber).Key);
                                    command.CommandText = query;
                                    var test = command.ExecuteNonQuery();
                                }
                            }
                        }

                        reader.Close();
                    }

                }
                catch (Exception e)
                {
                    LogWriter.ErrorWriterToFile("SMS Notifier:\t" + e.Message + "\t" + e.InnerException);
                    Console.WriteLine(e.Message + "\t" + e.InnerException);
                }
            }
        }
    }
}
