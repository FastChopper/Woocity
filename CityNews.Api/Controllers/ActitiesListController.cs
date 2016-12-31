using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CityNews.Api.Models;
using CityNews.Model;
using Jstv.Web;

namespace CityNews.Api.Controllers
{
    public class ActitiesListController : ApiController
    {

        public object Post(Actities model)
        {
            CityNewsEntities db = new CityNewsEntities();
            if (string.IsNullOrEmpty(model.Leader))
            {
                return new
                {
                    Success = false,
                    Msg = "发起人姓名不可为空"
                };
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                return new
                {
                    Success = false,
                    Msg = "活动名称不可为空"
                };
            }
            if (string.IsNullOrEmpty(model.Contents))
            {
                return new
                {
                    Success = false,
                    Msg = "活动详情不可为空"
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
                string newid = Guid.NewGuid().ToString().Replace("-", "");
                if (model.FilePath.Count > 0)
                {
                    foreach (var item in model.FilePath)
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
                CityNews_ActitiesList actity = new CityNews_ActitiesList
                {
                    AddTime = DateTime.Now,
                    Arrange = model.Arrange ?? "",
                    Contents = model.Contents,
                    FilePath = newid,
                    IP = IPHelper.IP,
                    IPInt = ipint,
                    Leader = model.Leader,
                    Name = model.Name,
                    State = 0,
                    Status = 0,
                    SignEndTime = model.SignEndTime,
                    EndTime = model.EndTime,
                    StartTime = model.StartTime,
                    OrderIndex = 1,
                    Phone = model.Phone
                };
                db.CityNews_ActitiesList.Add(actity);
                db.SaveChanges();
                actity.OrderIndex = actity.ID;
                db.SaveChanges();
                return new
                {
                    Success = true,
                    Msg = "提交成功！",
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Success = false,
                    Msg = ex.Message
                };
            }
        }
    }
}
