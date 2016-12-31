using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Cors;
using CityNews.Api.Models;
using CityNews.Model;
using Jstv.Web;

namespace CityNews.Api.Controllers
{
    public class ActitySignController : ApiController
    {
      
        public object Post(ActitySign model)
        {
            CityNewsEntities db = new CityNewsEntities();
            Regex reg = new Regex(@"^1[3|4|5|8|7]\d{9}$");

            if (string.IsNullOrEmpty(model.Phone))
            {
                return new
                {
                    Success = false,
                    Msg = "手机号不可为空"
                };
            }
            if (!reg.IsMatch(model.Phone))
            {
                return new
                {
                    success = false,
                    message = "手机号码格式不正确！"
                };
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                return new
                {
                    Success = false,
                    Msg = "姓名不可为空"
                };
            }
            var cityNewsActitiesList = db.CityNews_ActitiesList.FirstOrDefault(d => d.ID == model.Id);
            if (cityNewsActitiesList != null)
            {
                DateTime signTime = cityNewsActitiesList.SignEndTime;
                if (signTime<DateTime.Now.Date)
                {
                    return new
                    {
                        Success=false,
                        Msg="报名时间已结束",
                    };
                }
            }

            int isSign = db.CityNews_ActitySign.Count(d => d.Phone == model.Phone&&d.Fk_Id==model.Id);
            if (isSign>0)
            {
                return new
                {
                    Success = false,
                    Msg = "该手机已报过此活动",
                };
            }
            string IP = IPHelper.IP;
            int ipint;
            if (IP.Contains(":"))
            {
                IP = IP.Remove(IP.IndexOf(":"), (IP.Length - IP.IndexOf(":")) - 1);
                int i = 4;
                ipint = IP.Split('.').Select(item => int.Parse(item) << (--i * 8)).Sum();
            }
            else
            {
                ipint = IPHelper.IPInt;
            }
            CityNews_ActitySign sign=new CityNews_ActitySign
            {
                AddTime = DateTime.Now,
                Fk_Id = model.Id,
                Ip = IPHelper.IP,
                IpInt = ipint,
                Name = model.Name,
                Phone = model.Phone,
            };
            db.CityNews_ActitySign.Add(sign);
            try
            {
                db.SaveChanges();
                return new
                {
                    Success = true,
                    Msg = "报名成功！"
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Success = false,
                    Msg = ex.Message ,
                };
            }
        }
    }
}
