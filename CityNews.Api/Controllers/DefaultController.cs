using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CityNews.Api.Controllers
{
    public class DefaultController : ApiController
    {
        public object get()
        {
            var context = HttpContext.Current.Request;
            var filepath = context.Files[0];
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("diaospublicblob"));
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("images");
            UploadFileToContainer(container, @"D:\test.txt", "test.txt");
            return new
            {
                Url = "https://hdstatic.blob.core.chinacloudapi.cn/images/test.txt"
            };
        }
   

        [NonAction]
        private static void UploadFileToContainer(CloudBlobContainer container, string filePath, string uplodedFileName)
        {
            var blockBlob = container.GetBlockBlobReference(uplodedFileName);
            using (var fileStream = System.IO.File.OpenRead(filePath))
            {
                blockBlob.UploadFromStream(fileStream);
            }
        }
    }

   
}
