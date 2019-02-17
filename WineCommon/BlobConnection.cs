using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;

namespace WineCommon
{
    public class BlobConnection
    {
        public static CloudBlockBlob GetBlobReference(string connectionString, string containerName, string fileName)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var client = storageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference(containerName);

            return container.GetBlockBlobReference(fileName );
        }
    }
}
