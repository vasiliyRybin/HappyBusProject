using HappyBusProject.Notification;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace HappyBusProject.TwilioNotification
{
    public class TwilioSMSNotifier : ISMSNotifier
    {
        private const int _smsDelay = 10_000;
        private Dictionary<string, string> _usersToNotify = new();
        private readonly string _connectionString;
        private readonly string _accountSid;
        private readonly string _authToken;

        public TwilioSMSNotifier(
            string connectionString,
            string accountSid,
            string authToken)
        //=>
        //  (_connectionString, _accountSid, _authToken) =
        //  (connectionString, accountSid, authToken);
        {
            _accountSid = accountSid;
            _authToken = authToken;
            _connectionString = connectionString;
        }

        public async Task StartNotifierAsync()
        {
            while (true)
            {
                try
                {
                    await Task.Delay(_smsDelay);

                    using SqlConnection connection = new(_connectionString);
                    connection.Open();
                    var allUsers = Queries.GetAllNotNotifiedUsers();
                    SqlCommand command = new(allUsers, connection);
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
                    Console.WriteLine(e.Message + "\t" + e.InnerException);
                }
            }
        }
    }
    public static class Queries
    {
        public static string GetAllNotNotifiedUsers()
        {
            return "SELECT[OrderID], US.PhoneNumber " +
                   "FROM[MyShuttleBusAppNewDB].[dbo].[NotifierState] NTF " +
                   "JOIN(SELECT[CustomerID], [ID] FROM[MyShuttleBusAppNewDB].[dbo].[Orders]) ORD " +
                   "ON ORD.ID = NTF.OrderID " +
                   "JOIN(SELECT[ID], [PhoneNumber] FROM[MyShuttleBusAppNewDB].[dbo].[Users]) US " +
                   "ON US.ID = ORD.CustomerID " +
                   "WHERE IsNotified = 0 " +
                   "AND DATEDIFF(MINUTE, GETDATE(), [DesiredDepartureTime]) BETWEEN 0 AND 60 ";
        }

        public static string UpdateAfterNotification(string orderID)
        {
            return "UPDATE[MyShuttleBusAppNewDB].[dbo].[NotifierState] " +
                   "SET IsNotified = 1 " +
                  $"WHERE OrderID = '{orderID}' ";
        }
    }
}