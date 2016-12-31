using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CityNews.Api.Models;
using CityNews.Model;
using CityNews.Model.Common;

namespace CityNews.Api.Controllers
{
    [EnableCors("*", "*", "*")]
    public class GetActityController : ApiController
    {
       
        public Result<object> Get()
        {
            CityNewsEntities db=new CityNewsEntities();
            var list = db.CityNews_ActitiesList.Where(i => i.State == 1 && i.Status == 1).Select(i => new ActityViewModel
            {
                ID = i.ID,
                Leader = i.Leader,
                Contents= i.Contents,
                Arrange=  i.Arrange,
                AddTime=i.AddTime,
                FilePath=db.CityNews_File.FirstOrDefault(d=>d.Fk_ID==i.FilePath).FilePath,
                Name = i.Name,
                SignEndTime = i.SignEndTime,
                StartTime = i.StartTime,
                EndTime = i.EndTime,
                OrderIndex = i.OrderIndex
            }).OrderByDescending(i=>i.OrderIndex).ToArray();
            return new Result<object>
            {
                Code = ResultCode.Success,
                Msg = "获取成功",
                Data = list
            };
        }

        [Route("GetActityByID")]
        public Result<object> Get(int id)
        {
            CityNewsEntities db = new CityNewsEntities();
            var list = db.CityNews_ActitiesList.Where(i => i.State == 1 && i.Status == 1&&i.ID==id).Select(i => new ActityViewModel
            {
                ID = i.ID,
                Leader = i.Leader,
                Contents = i.Contents,
                Arrange = i.Arrange,
                AddTime = i.AddTime,
                FilePath = db.CityNews_File.FirstOrDefault(d => d.Fk_ID == i.FilePath).FilePath,
                Name = i.Name,
                SignEndTime = i.SignEndTime,
                StartTime = i.StartTime,
                EndTime = i.EndTime,
                PersonCount = db.CityNews_ActitySign.Count(d=>d.Fk_Id==i.ID)
            }).ToArray();
            return new Result<object>
            {
                Code = ResultCode.Success,
                Msg = "获取成功",
                Data = list
            };
        }
    }
}
