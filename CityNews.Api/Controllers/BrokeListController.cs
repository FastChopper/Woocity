using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.UI;
using System.Web.WebPages.Html;
using CityNews.Api.Models;
using CityNews.Model;
using CityNews.Model.Common;
using Jstv;

namespace CityNews.Api.Controllers
{
   
    public class BrokeListController : ApiController
    {
       
        public Result<object> Get()
        {
            CityNewsEntities db = new CityNewsEntities();
            var list = db.CityNews_BrokeNews.Where(i => i.Status == 1).Select(i => new GetBrokeModel
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
                Title = i.Title,
                OrderIndex=i.OrderIndex
            }).OrderByDescending(i => i.OrderIndex).ToList();
            return new Result<object>
            {
                Code = ResultCode.Success,
                Msg = "获取成功",
                Data = list
            };
        }

        [Route("BrokeListPage")]
        public ResultList<GetBrokeModel> Get(int pageIndex)
        {
            int pageSize = 10;
            CityNewsEntities db = new CityNewsEntities();
            var list = db.CityNews_BrokeNews.Where(i => i.Status == 1).Select(i => new GetBrokeModel
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
                Title = i.Title
            }).OrderByDescending(i => i.Creatime).Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize).ToList();
            int count = db.CityNews_BrokeNews.Count(i => i.Status == 1);
            return new ResultList<GetBrokeModel>
            {
                Code = ResultCode.Success,
                Msg = "获取成功",
                RecordCount = count,
                Data = list
            };

        }
    }
}
