using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ShopApp.Utility
{
    public static class FileManager
    {
        public static async Task Configure()
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=zakupowostorage;AccountKey=/W5lxkmTY0GWaGzb1w6Ev00aVivkwTbLCfgOh/y7aGU4MDYwLlX1TIbrAr/daB5OO93G3BTHz7K9AY64hgKTiA==;EndpointSuffix=core.windows.net";
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("offer-pictures");

            string localPath = "../../App_Files/Other/";
            string fileName = "quickstart" + Guid.NewGuid().ToString() + ".txt";
            string localFilePath = Path.Combine(localPath, fileName);

            // Write text to the file
            File.WriteAllText(localFilePath, "Hello, World!");

            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            // Open the file and upload its data
            using (FileStream uploadFileStream = File.OpenRead(localFilePath))
            {
                await blobClient.UploadAsync(uploadFileStream, true);
                uploadFileStream.Close();
            }

            System.Diagnostics.Debug.WriteLine("Zapisałem plik " + fileName);
        }
    }
}


