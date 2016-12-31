using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using CityNews.Api.Models;
using CityNews.Model;
using Jstv.Web;
using Newtonsoft.Json;

namespace CityNews.Api.Controllers
{
    public class BrokeController : ApiController
    {
       
        public object postBroke(BrokeNews borke)
        {
            CityNewsEntities db=new CityNewsEntities();
            Regex reg = new Regex(@"^1[3|4|5|8|7]\d{9}$");
          
            if (string.IsNullOrEmpty(borke.Phone))
            {
                return new
                {
                    Success = false,
                    Msg = "手机号不可为空"
                };
            }

            if (string.IsNullOrEmpty(borke.Title))
            {
                return new
                {
                    Success = false,
                    Msg = "标题不可为空"
                };
            }
            if (!reg.IsMatch(borke.Phone))
            {
                return new
                {
                    success = false,
                    message = "手机号码格式不正确！"
                };
            }
            if (string.IsNullOrEmpty(borke.Contents))
            {
                return new
                {
                    Success = false,
                    Msg = "爆料内容不可为空"
                };
            }
            if (borke.Contents.Length<10)
            {
                return new
                {
                    Success = false,
                    Msg = "爆料内容不可小于10个字符"
                };
            }
            string newid = Guid.NewGuid().ToString().Replace("-", "");

            try
            {
                if (borke.FilePath.Count > 0)
                {
                    foreach (var item in borke.FilePath)
                    {
                        CityNews_File cityNewsFile = new CityNews_File
                        {
                            FilePath = item.Path,
                            FileRealName = item.FileName,
                            AddTime = DateTime.Now,
                            Fk_ID = newid,
                            MinPath = item.PathMin
                        };
                        db.CityNews_File.Add(cityNewsFile);
                    }
                }
            }
            catch (Exception ex)
            {
                return new
                {
                    Success = false,
                    Msg = ex.Message+"1"
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
            try
            {
                
                CityNews_BrokeNews brokeNews = new CityNews_BrokeNews
                {
                    Contents = borke.Contents,
                    Creatime = DateTime.Now,
                    Explain = "",
                    FilePath = newid,
                    IP = IPHelper.IP,
                    IPInt = ipint,
                    Phone = borke.Phone,
                    State = 0,
                    Status = 0,
                    ExplainTime = DateTime.Now,
                    IficationTime= DateTime.Now,
                    OrderIndex = 1,
                    Title = borke.Title
                };
                db.CityNews_BrokeNews.Add(brokeNews);
                db.SaveChanges();
                brokeNews.OrderIndex = brokeNews.ID;
                db.SaveChanges();
                return new
                {
                    Success = true,
                    Msg = "提交成功！",
                    Id = brokeNews.ID,
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Success = false,
                    Msg = ex.Message + "2",
                };
            }
          
           
        }
    }
}
