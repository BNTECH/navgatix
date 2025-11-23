using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.WindowsAzure.Storage.Blob;
using satguruApp.Helpers;
using System.ComponentModel;
using System.Threading.Tasks;
using System.IO;
using System;

namespace satguruApp.Service.Services
{
    public class StorageService
    {
        CloudBlobContainer container;
      //  class BlobStorageService
        //{
            private readonly BlobServiceClient _blobServiceClient;
            private readonly BlobContainerClient _containerClient;

            public StorageService(string connectionString, string containerName)
            {
                _blobServiceClient = new BlobServiceClient(connectionString);
                _containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            }

            // Upload a file to blob container
            public async Task UploadFileAsync(string localFilePath, string blobName)
            {
                BlobClient blobClient = _containerClient.GetBlobClient(blobName);

                using FileStream uploadFileStream = File.OpenRead(localFilePath);
                await blobClient.UploadAsync(uploadFileStream, true);
                Console.WriteLine($"Uploaded file to blob storage as {blobName}");
            }

            // Download a blob to local file
            public async Task DownloadFileAsync(string blobName, string downloadFilePath)
            {
                BlobClient blobClient = _containerClient.GetBlobClient(blobName);

                BlobDownloadInfo download = await blobClient.DownloadAsync();

                using (FileStream downloadFileStream = File.OpenWrite(downloadFilePath))
                {
                    await download.Content.CopyToAsync(downloadFileStream);
                }
                Console.WriteLine($"Downloaded blob {blobName} to {downloadFilePath}");
            }

            // List blobs inside the container
            public async Task ListBlobsAsync()
            {
                await foreach (BlobItem blobItem in _containerClient.GetBlobsAsync())
                {
                    Console.WriteLine($"Blob name: {blobItem.Name}");
                }
            }
        


        //public async Task<CloudBlobContainer> Init(string containerName = "", string account = "", string key = "")
        //{

        //    Microsoft.WindowsAzure.Storage.CloudStorageAccount cloudStorageAccount = ConnectionString.GetConnectionString(account, key);
        //    CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        //    container = cloudBlobClient.GetContainerReference(containerName);

        //    if (await container.CreateIfNotExistsAsync())
        //    {
        //        await container.SetPermissionsAsync(
        //            new BlobContainerPermissions
        //            {
        //                PublicAccess = BlobContainerPublicAccessType.Blob
        //            }
        //            );
        //    }
        //    return container;
        //}
        //public string UploadFile(HttpPostedFileBase fileToUpload)
        //{
        //    return AsyncHelpers.RunSync<string>(() => UploadFileAsync(fileToUpload));
        //}

        public string UploadFile(string folder, string fileName, Byte[] fileBytes)
        {
            return AsyncHelpers.RunSync<string>(() => UploadFileAsync(folder, fileName, fileBytes));
        }

        //public async Task<string> UploadFileAsync(HttpPostedFileBase fileToUpload)
        //{
        //    string folder = "";
        //    if (fileToUpload == null || fileToUpload.ContentLength == 0)
        //    {
        //        return null;
        //    }

        //    var fileBytes = new Byte[fileToUpload.ContentLength];
        //    fileToUpload.InputStream.Read(fileBytes, 0, fileToUpload.ContentLength);

        //    return await UploadFileAsync(folder, fileToUpload.FileName, fileBytes, fileToUpload.ContentType);
        //}


        public async Task<string> UploadFileAsync(string folder, string fileName, Byte[] fileBytes, string contentType = null)
        {
            if (fileBytes == null)
                return null;
            try
            {
                string fileFullPath = null;
                var currentWeekYear = CurrentWeek.GetCurrentWeek() + "/";
                var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                var blob = container.GetBlockBlobReference(folder + currentWeekYear + newFileName);
                blob.Properties.ContentType = contentType == null ? Path.GetExtension(fileName) : contentType;
                await blob.UploadFromByteArrayAsync(fileBytes, 0, fileBytes.Length);
                fileFullPath = folder + currentWeekYear + newFileName;
                return fileFullPath;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //code to map image to cloud
        public async Task<string> UploadFileAsync1(string fileName, Byte[] fileBytes, string contentType = null, string weekyear = null)
        {
            if (fileBytes == null)
                return null;
            try
            {
                string fileFullPath = null;
                var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                var blob = container.GetBlockBlobReference(weekyear + "/" + newFileName);
                blob.Properties.ContentType = contentType == null ? Path.GetExtension(fileName) : contentType;
                await blob.UploadFromByteArrayAsync(fileBytes, 0, fileBytes.Length);
                fileFullPath = weekyear + "/" + newFileName;
                return fileFullPath;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public byte[] DownloadFile(string fileName)
        {
            return AsyncHelpers.RunSync<byte[]>(() => DownloadFileAsync(fileName));
        }
        public async Task<byte[]> DownloadFileAsync(string fileName)
        {
            try
            {
                var blockBlob = container.GetBlockBlobReference(fileName);
                // Read content  
                using (var ms = new MemoryStream())
                {
                    await blockBlob.DownloadToStreamAsync(ms);
                    return ms.ToArray();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}