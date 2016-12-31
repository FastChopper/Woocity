using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityNews.Admin.Controllers
{
    public class IncludeController : Controller
    {
   
        [ChildActionOnly]
        public ActionResult UploaderJs(string Direcetory)
        {
            ViewBag.Direcetory = Direcetory;
            return PartialView();
        }
    }
}