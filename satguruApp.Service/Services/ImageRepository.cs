using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace satguruApp.Service.Services
{
    public class ImageRepository
    {
 




    //  #region Microsoft Azure Image Upload
    //public async Task<string> UploadImage(string folder, string FileName, Byte[] imageBytes, string contentType, string CDN3Folder, string account, string key)
    //{
    //    try
    //    {
    //        folder = String.IsNullOrWhiteSpace(folder) ? "" : folder + "/";
    //        var _imageService = new StorageService();
    //        //await _imageService.Init(CDN3Folder, account, key);
    //        //return (await _imageService.UploadFileAsync(folder, FileName, imageBytes, contentType));
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}

    //#endregion

    //public async Task<byte[]> DownloadFileAsync(string FileName, string CDN3Folder, string account, string key)
    //{
    //    try
    //    {
    //        string[] stringSeparators = new string[] { CDN3Folder + "/" };
    //        var _imageService = new StorageService();
    //        await _imageService.Init(CDN3Folder, account, key);
    //        string[] urlArray = FileName.Split(stringSeparators, StringSplitOptions.None);
    //        FileName = urlArray[1];
    //        return await _imageService.DownloadFileAsync(FileName);
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}
}
}
