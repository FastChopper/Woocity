using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Management;
using CityNews.Api.Models;
using CityNews.Model.Common;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CityNews.Api.Controllers
{
  
    public class UploadController : ApiController
    {
       
        public object PostUpload()
        {
            var context = HttpContext.Current.Request;
            var file = context.Files[0];
            return Upload.UploadFile(file);
        }

        [Route("Uploader")]
        public object Post(Uploader model)
        {
            Random rnd = new Random();
           
            try
            {
                string baseSting = model.Path;
                byte[] imageBytes = Convert.FromBase64String(baseSting);
                string fileExt = model.FileName.Substring(model.FileName.LastIndexOf(".", StringComparison.Ordinal));
                var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("diaospublicblob"));
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference("images");
                var fileName = DateTime.Now.ToString().Replace(" ", "").Replace("/", "").Replace(":", "");
                fileName += rnd.Next(10000000, 99999999).ToString();
                fileName += fileExt;
                Upload.UploadFileToContainer(container, imageBytes, fileName);
                return new
                {
                    Success = true,
                    Url = "https://hdstatic.blob.core.chinacloudapi.cn/images/" + fileName,
                    FileName = model.FileName.Substring(model.FileName.LastIndexOf("\\", StringComparison.Ordinal)+1)
                };
            }
            catch (Exception ex)
            {
                return new {Success=false,Msg=ex.Message};
            }
        }
    }
}
