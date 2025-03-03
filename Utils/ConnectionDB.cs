namespace BartenderApp.Utils
{
    public static class ConnectionDB
    {
        public static string ReturnConnectionString(string dbName)
        {
            string connectionString= string.Empty;
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                connectionString = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                connectionString = Path.Combine(connectionString, dbName);
            }
            else if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                connectionString = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                connectionString = Path.Combine(connectionString,"..","Library", dbName);
            }
            return connectionString;
        }
    }
}
