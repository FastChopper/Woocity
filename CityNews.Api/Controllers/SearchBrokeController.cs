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
using CityNews.Model.Common;

namespace CityNews.Api.Controllers
{
    public class SearchBrokeController : ApiController
    {
        [EnableCors("*", "*", "*")]
        public Result<object> Post(BrokeSearch model)
        {
            if (string.IsNullOrEmpty(model.Phone) && model.Id == 0)
            {
                return new Result<object>
                {
                    Code = ResultCode.CreateFailed,
                    Msg = "至少填写一个查询项",
                };
            }
            CityNewsEntities db = new CityNewsEntities();
            List<GetBrokeModel> list;
            if (!string.IsNullOrEmpty(model.Phone))
            {
                Regex reg = new Regex(@"^1[3|4|5|8|7]\d{9}$");
                if (!reg.IsMatch(model.Phone))
                {
                    return new Result<object>
                    {
                        Code = ResultCode.CreateFailed,
                        Msg = "手机号码格式不正确！"
                    };
                }
            }
            if (!string.IsNullOrEmpty(model.Phone) && model.Id == 0)
            {
               
                list = db.CityNews_BrokeNews.Where(i => i.Phone == model.Phone).Select(i=>new GetBrokeModel
                {
                    ID = i.ID,
                    Contents = i.Contents,
                    Creatime = i.Creatime,
                    Explain = i.Explain,
                    ExplainTime = i.ExplainTime,
                    FilePath = i.FilePath,
                    IficationTime = i.IficationTime,
                    State = i.State,
                    Phone = i.Phone,
                    Status = i.Status,
                    Type = i.Type,
                }).ToList();
            }
            else if (model.Id != 0 && string.IsNullOrEmpty(model.Phone))
            {
                list = db.CityNews_BrokeNews.Where(i => i.ID == model.Id).Select(i => new GetBrokeModel
                {
                    ID = i.ID,
                    Contents = i.Contents,
                    Creatime = i.Creatime,
                    Explain = i.Explain,
                    ExplainTime = i.ExplainTime,
                    FilePath = i.FilePath,
                    IficationTime = i.IficationTime,
                    State = i.State,
                    Phone = i.Phone,
                    Status = i.Status,
                    Type = i.Type,
                }).OrderByDescending(i => i.Creatime).ToList();
            }
            else
            {
                list = db.CityNews_BrokeNews.Where(i => i.ID == model.Id).Select(i => new GetBrokeModel
                {
                    ID = i.ID,
                    Contents = i.Contents,
                    Creatime = i.Creatime,
                    Explain = i.Explain,
                    ExplainTime = i.ExplainTime,
                    FilePath = i.FilePath,
                    IficationTime = i.IficationTime,
                    State = i.State,
                    Phone = i.Phone,
                    Status = i.Status,
                    Type = i.Type,
                }).ToList();
                var listmodel = db.CityNews_BrokeNews.Where(i => i.ID != model.Id&&i.Phone==model.Phone).Select(i => new GetBrokeModel
                {
                    ID = i.ID,
                    Contents = i.Contents,
                    Creatime = i.Creatime,
                    Explain = i.Explain,
                    ExplainTime = i.ExplainTime,
                    FilePath = i.FilePath,
                    IficationTime = i.IficationTime,
                    State = i.State,
                    Phone = i.Phone,
                    Status = i.Status,
                    Type = i.Type,
                }).OrderByDescending(i => i.Creatime).ToList();
                list.AddRange(listmodel);
            }
            return new Result<object>
            {
                Code = ResultCode.Success,
                Msg = "获取成功",
                Data = list
            };
        }
    }
}
