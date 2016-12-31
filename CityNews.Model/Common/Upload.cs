using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Web;
using Jstv.Web;
using Jstv;
using System.Drawing;



namespace CityNews.Model.Common
{
    public class Upload
    {
        public static RequestJson UploadFile(HttpPostedFile file)
        {
            try
            {
                Random rnd = new Random();
                string fileName = string.Empty;
                int length = file.ContentLength;
                string type = file.ContentType;
                string fileExt = Path.GetExtension(file.FileName).ToLower();
                string fileRealName = Path.GetFileName(file.FileName);
                //文件类型
                //string fileFilt = ".gif|.jpg|.php|.jsp|.jpeg|.png|.avi|.rmvb|.rm|.asf|.divx|.mpg|.mpeg|.mpe|.wmv|.mp4|.mkv|.vob ";
                //if (fileFilt.IndexOf(fileExt) <= -1)
                //{
                //    return new RequestJson
                //    {
                //        Msg = "不支持该文件类型",
                //        Success = false,
                //    };
                //}
                string filePic = ".gif|.jpg|.php|.jsp|.jpeg|.png";
                var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("diaospublicblob"));
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference("images");
                fileName = DateTime.Now.ToString().Replace(" ", "").Replace("/", "").Replace(":", "");
                fileName += rnd.Next(10000000, 99999999).ToString();
                string fileMinName = fileName + "min" + fileExt;
                fileName += fileExt;
                var filebyte = file.InputStream.ToByteArray();
                if (filePic.IndexOf(fileExt, StringComparison.Ordinal) >= 0)
                {
                    if (length > 1024 * 1024 * 4)
                    {
                        return new RequestJson
                        {
                            Msg = "图片文件大于4M上传失败",
                            Success = false,
                        };
                    }
                    UploadFileToContainer(container, filebyte, fileName);
                    Image img = Image.FromStream(new MemoryStream(filebyte));
                
                    //container = blobClient.GetContainerReference("images");
                 
                    if (img.Width > 300)
                    {
                        int newW = 300;
                        int newH = (int)((double)newW / img.Width * img.Height);
                        byte[] minFileBytes = ThumbNail(img, newW, newH);
                        UploadFileToContainer(container, minFileBytes, fileMinName);
                    }
                    else
                    {
                        fileMinName = fileName;
                    }
                    return new RequestJson
                    {
                        Success = true,
                        Url = "https://hdstatic.blob.core.chinacloudapi.cn/images/" + fileName,
                        UrlMin = "https://hdstatic.blob.core.chinacloudapi.cn/images/" + fileMinName,
                        FileName = fileRealName
                    };
                }
                else
                {
                    return  new RequestJson
                    {
                        Success = false,
                        Msg = "该文件不是图片",
                    };
                }
                //if (length > 1024 * 1024 * 100)
                //{
                //    return new RequestJson
                //    {
                //        Msg = "视频文件大于100M上传失败",
                //        Success = false,
                //    };
                //}
                //UploadFileToContainer(container, file, fileName);
                //return new RequestJson
                //{
                //    Success = true,
                //    Url = "https://hdstatic.blob.core.chinacloudapi.cn/videos/"+ fileName,
                //    FileName = fileRealName,
                //    Type = "video",
                //};
            }
            catch
            {
                throw new Exception();
            }
        }

        public static void UploadFileToContainer(CloudBlobContainer container, HttpPostedFile file, string uplodedFileName)
        {
            var blockBlob = container.GetBlockBlobReference(uplodedFileName);
            using (var fileStream = file.InputStream)
            {
                blockBlob.UploadFromStream(fileStream);
            }
        }
        public static void UploadFileToContainer(CloudBlobContainer container, byte[] file, string uplodedFileName)
        {
            var blockBlob = container.GetBlockBlobReference(uplodedFileName);
                blockBlob.UploadFromByteArray(file,0,file.Length);
        }

        /// <summary>
        /// 缩略图
        /// </summary>
        /// <param name="img">流图像</param>
        /// <param name="newW">宽</param>
        /// <param name="newH">高</param>
        /// <returns></returns>
        public static byte[] ThumbNail(Image img, int newW, int newH)
        {
            var img_s = ImageCutter.Scale(img, newW, newH);
            MemoryStream ms = new MemoryStream();
            img_s.Save(ms, img.RawFormat);
            byte[] newData = ms.ToArray();
            ms.Close();
            return newData;
        }


    }
}
