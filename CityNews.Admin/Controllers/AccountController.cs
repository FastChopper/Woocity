using Jstv.AccessControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CityNews.Admin.Models;
using CityNews.Model;

namespace CityNews.Admin.Controllers
{
    public class AccountController : Controller
    {
        CityNewsEntities db=new CityNewsEntities();
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            var isChecked = false;
            var userId = 0;
            var userRole = "";
            if (model.NickName == "jsbcadmin" && model.Password == "jsbc2016")
            {
                isChecked = true;
                userId = 1;
                userRole = "1";
            }
            else if (model.NickName == "jsbceditor" && model.Password == "jsbc2016")
            {
                isChecked = true;
                userId = 2;
                userRole = "2";
            }
            else if (model.NickName == "jsbcification" && model.Password == "jsbc2016")
            {
                isChecked = true;
                userId = 3;
                userRole = "3";
            }
            if (isChecked)
            {
                new AccessHelper<int>()
                    .SetAccess(userId, userRole);
                return Json(new
                {
                    Success = true,
                    Message = "登录成功"
                });
            }
            else
            {
                return Json(new
                {
                    Success = false,
                    Message = "用户名或密码错误"
                });
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Logout()
        {
            new AccessHelper<int>()
                    .ClearAccess();
            return Json(new 
            {
                Success = true,
                Message = "登出成功"
            });
        }
    }
}