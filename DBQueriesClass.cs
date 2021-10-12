namespace HappyBusProject
{
    public static class DBQueriesClass
    {
        public static string GetDrivers()
        {
            return "SELECT [Name] " +
              ",[CarID] " +
              ",[Rating] " +
              ",[Age] " +
              "FROM[MyShuttleBusAppDB].[dbo].[Drivers] ";
        }

        public static string GetDriverByID(string ID)
        {
            return GetDrivers() + $"WHERE ID = '{ID}'";
        }

        public static string CreateNewDriver(string brand, string seatsNum, string registrationNumPlate, string carAge, string driverName, string driverAge, string examPass)
        {
            return
            "DECLARE @CarID UNIQUEIDENTIFIER = NEWID() " +

            "INSERT INTO[MyShuttleBusAppDB].[dbo].[Cars] " +
            $"VALUES(@CarID, '{brand}', {seatsNum}, '{registrationNumPlate}', {carAge}) " +

            "INSERT INTO[MyShuttleBusAppDB].[dbo].[Drivers] " +
            $"VALUES(NEWID(), '{driverName}', @CarID, 5.0, '{driverAge}', '{examPass}')";
        }

        public static string DeleteFriver()
        {
            return "Deleted!";
        }
    }
}
