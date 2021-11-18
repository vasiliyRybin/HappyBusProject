namespace HappyBusProject.SmsNotificationLayer
{
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
