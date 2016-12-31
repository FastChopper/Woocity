using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CityNews.Model.Common;

namespace CityNews.Admin.Controllers
{
    public class UploadController : Controller
    {
        [HttpPost]
        public ActionResult Pic(string dir)
        {
            var context = System.Web.HttpContext.Current.Request;
            if (context.Files.Count > 0)
            {
                try
                {
                    var file = context.Files[0];
                    return  Json(Upload.UploadFile(file));
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        Message = ex.Message + "|" + ex.InnerException.Message,
                        Success = false,
                        Url = default(string)
                    });
                }
            }
            return Json(new
            {
                Message = "未上传图片",
                Success = false,
                Url = default(string)
            });
        }

    }
}