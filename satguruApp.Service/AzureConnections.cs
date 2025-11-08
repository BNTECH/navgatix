using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace satguruApp.Service
{
    public static class AzureConnections
    {
        //static string defaultAccount = Configuration. ConfigurationManager.AppSettings["CDN2Account"];

        //public static string Key(string account)
        //{
        //    return ConfigurationManager.AppSettings[account];
        //}
        //public static CloudStorageAccount GetConnectionString(string account = "", string key = "")
        //{
        //    account = string.IsNullOrWhiteSpace(account) ? defaultAccount : account;
        //    key = string.IsNullOrWhiteSpace(key) ? Key(account) : key;
        //    if (string.IsNullOrEmpty(key))
        //    {
        //        throw new ArgumentNullException("No key found for this account: " + account);
        //    }
        //    string connectionString = string.Format("DefaultEndpointsProtocol=https;EndpointSuffix=core.windows.net;AccountName={0};AccountKey={1}", account, key);
        //    return CloudStorageAccount.Parse(connectionString);
        //}
    }
}
