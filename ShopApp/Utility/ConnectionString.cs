using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;

namespace ShopApp.Utility
{
    public static class ConnectionString
    {
        static readonly string account = "zakupowofiles";
        static readonly string key = "RmujwDWlV1pqYlg/9zfO6MUHHsXbznkev3NcOPidHoYUCuouWMXnvfZEDqhXvizEDpsaeqxwCljcBKvHVTsXvw==";
        public static CloudStorageAccount GetConnectionString()
        {
            string connectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", account, key);
            return CloudStorageAccount.Parse(connectionString);
        }
    }
}