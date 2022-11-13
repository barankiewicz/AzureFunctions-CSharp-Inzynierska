using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

namespace GetVehicleData.Class
{
    public static class StorageManager
    {
        private static BlobContainerClient[] containers = 
            {   
                new BlobContainerClient(Environment.GetEnvironmentVariable("AzureStorageString"), "raw-data"),
                new BlobContainerClient(Environment.GetEnvironmentVariable("AzureStorageString"), "timetables"),
                new BlobContainerClient(Environment.GetEnvironmentVariable("AzureStorageString"), "processed-data"),
                new BlobContainerClient(Environment.GetEnvironmentVariable("AzureStorageString"), "event-log-data"),
                new BlobContainerClient(Environment.GetEnvironmentVariable("AzureStorageString"), "helper-data") 
            };

        public static StreamWriter GetBlobStream(ContainerEnum cont, string fileName)
        {
            var container = containers[(int)cont];
            var blob = container.GetAppendBlobClient(fileName);
            blob.CreateIfNotExists();

  
            Stream str = blob.OpenWrite(false);

            StreamWriter sw = new StreamWriter(str);
            sw.AutoFlush = true;

            return sw;
        }

        public static AppendBlobClient GetRawDataBlob(string fileName)
        {
            var container = containers[0];
            var blob = container.GetAppendBlobClient(fileName);

            if (!blob.Exists().Value)
            {
                blob.Create();
                blob.AppendBlock(StreamGenerator.GenerateStream("Lines,Brigade,VehicleNumber,Lon,Lat,Time,VehicleType\n"));
            }

            return blob;
        }

        public static StreamWriter GetTimetableBlob(string fileName)
        {
            var container = containers[1];
            var blob = container.GetBlobClient(fileName);

            StreamWriter sw = new StreamWriter(blob.OpenWrite(true));

            return sw;
        }

    }
}
