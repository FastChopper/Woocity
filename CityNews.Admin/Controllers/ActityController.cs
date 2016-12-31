using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CityNews.Admin.Models;
using CityNews.Model;
using CityNews.Model.Common;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace CityNews.Admin.Controllers
{
    public class ActityController : Controller
    {
        CityNewsEntities db = new CityNewsEntities();
        // GET: Actity
        public ActionResult Index()
        {
            int PageIndex = PagerHelper.GetPageIndex();
            int PageSize = 20;
            var rsl = new ResultList<ActityViewModel>
            {
                Msg = "获取失败",
                Code = ResultCode.Error,
                Data = new List<ActityViewModel>(),
            };
            try
            {
                var list = db.CityNews_ActitiesList.Select(i =>
                    new ActityViewModel
                    {
                        ID = i.ID,
                        Contents = i.Contents,
                        FilePath = db.CityNews_File.FirstOrDefault(d => d.Fk_ID == i.FilePath).FilePath,
                        State = i.State,
                        Status = i.Status,
                        StateName = db.CityNews_Dict.FirstOrDefault(d => d.Value == i.State && d.Type == "爆料状态").Text,
                        Leader = i.Leader,
                        Name = i.Name,
                        AddTime = i.AddTime,
                        Phone = i.Phone,
                        StartTime = i.StartTime,
                        EndTime = i.EndTime,

                        OrderIndex = i.OrderIndex
                    }).OrderByDescending(d => d.OrderIndex)
                .Skip((PageIndex - 1) * PageSize)
                    .Take(PageSize).ToList();
                ViewBag.RecordCount = db.CityNews_ActitiesList.Count();
                rsl.Data = list;
                rsl.Msg = "获取成功！";
                rsl.Code = ResultCode.Success;
            }
            catch (Exception ex)
            {
                //
            }
            return View(rsl);
        }

        #region 修改
        [HttpGet]
        public ActionResult Modify(int id)
        {
            var actity = db.CityNews_ActitiesList.FirstOrDefault(i => i.ID == id);
            var brokeStatus = db.CityNews_Dict.Where(i => i.Type == "发布状态").OrderBy(i => i.OrderIndex).ToList();
            var list = new List<SelectListItem>
            {
                new SelectListItem {Text = "通过", Value = "1"},
                new SelectListItem {Text = "不通过", Value = "0"}
            };
            if (actity == null)
            {
                return HttpNotFound();
            }
            var model = new ActityFormModel
            {
                Contents = actity.Contents,
                Leader = actity.Leader,
                FilePath = db.CityNews_File.Where(i => i.Fk_ID == actity.FilePath).ToList(),
                State = actity.State,
                Status = actity.Status,
                EndTime = actity.EndTime,
                Arrange = actity.Arrange,
                Name = actity.Name,
                SignEndTime = actity.SignEndTime,
                StartTime = actity.StartTime
            };
            ViewBag.brokeStatus = new SelectList(brokeStatus, "Value", "Text", model.Status);
            ViewBag.brokeState = new SelectList(list, "Value", "Text", model.State);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modify(int id, ActityFormModel model)
        {
            var actity = db.CityNews_ActitiesList.FirstOrDefault(i => i.ID == id);
            if (actity == null)
            {
                return Json(new Result
                {
                    Code = ResultCode.CreateFailed,
                    Msg = "未更新条目"
                });
            }
            Result rsl = new Result
            {
                Code = ResultCode.CreateFailed
            };
            actity.Contents = model.Contents;
            actity.Arrange = model.Arrange;
            actity.StartTime = model.StartTime;
            actity.EndTime = model.EndTime;
            actity.SignEndTime = model.SignEndTime;
            actity.Leader = model.Leader;
            actity.Name = model.Name;
            actity.State = model.State;
            actity.Status = model.Status;
            try
            {
                db.SaveChanges();
                rsl.Code = ResultCode.Success;
                rsl.Msg = "修改成功";
            }
            catch (Exception ex)
            {
                rsl.Msg = "修改失败!";
                rsl.Code = ResultCode.CreateFailed;
                return Json(rsl);
            }
            return Json(rsl);

        }
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Enable(int id, bool enable)
        {
            var user = db.CityNews_ActitiesList.FirstOrDefault(i => i.ID == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.Status = enable ? 1 : 0;
            try
            {
                db.SaveChanges();
                return Json(new Result
                {
                    Code = ResultCode.Success,
                    Msg = "修改成功"
                });
            }
            catch
            {
                return Json(new Result
                {
                    Code = ResultCode.CreateFailed,
                    Msg = "修改失败"
                });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnableState(int id, bool enable)
        {
            var user = db.CityNews_ActitiesList.FirstOrDefault(i => i.ID == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.State = enable ? 1 : 0;
            try
            {
                db.SaveChanges();
                return Json(new Result
                {
                    Code = ResultCode.Success,
                    Msg = "修改成功"
                });
            }
            catch
            {
                return Json(new Result
                {
                    Code = ResultCode.CreateFailed,
                    Msg = "修改失败"
                });
            }

        }


        public ActionResult SortOrder(int src, int des)
        {
            try
            {
                db.ChangeCityNews_ActitiesIndex(src, des);
                return Json(0);
            }
            catch (Exception)
            {
                return Json(-1);
            }
        }

        [HttpGet]
        public ActionResult Export(int id)
        {

            IWorkbook workbook = null;
            workbook = new HSSFWorkbook();//2003           

            List<CityNews_ActitySign> UserList;
            try
            {
                UserList = db.CityNews_ActitySign.Where(i=>i.Fk_Id==id).ToList();
            }
            catch
            {
                return null;
            }

            ISheet sheet = workbook.CreateSheet("sheet1");
            //设置大标题行   
            int rowCount = 0;


            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 14; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高   
            //设置标题行数据   
            int a = 0;

            IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题列   

            string[] columnHeaders = new string[] {
                "电话", "姓名","报名时间" };

            for (int k = 0; k < columnHeaders.Length; k++)
            {  //将传递过来的字符串表头进行拆分到Excel

                string columnName = columnHeaders[k];
                ICell cell = row1.CreateCell(a);
                cell.SetCellValue(columnName);
                a++;
            }

            //填写UserList数据进excel   
            for (int i = 0; i < UserList.Count; i++) //写行数据   
            {
                IRow row2 = sheet.CreateRow(i + 1);
                row2.CreateCell(0).SetCellValue(UserList[i].Phone);
                row2.CreateCell(1).SetCellValue(UserList[i].Name);
                row2.CreateCell(2).SetCellValue(UserList[i].AddTime.ToString("yyyy-MM-dd HH-mm-ss"));
            }

            //创建excel   
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);

            return File(ms, "application/vnd.ms-excel", "报名信息.xls");
        }


        public ActionResult ActitySign(int id)
        {
            var rsl = new ResultList<CityNews_ActitySign>
            {
                Msg = "获取失败",
                Code = ResultCode.Error,
                Data = new List<CityNews_ActitySign>(),
            };
            try
            {
                var list = db.CityNews_ActitySign.Where(i => i.Fk_Id == id).ToList();
                rsl.Msg = "获取成功";
                rsl.Code = ResultCode.Success;
                rsl.Data = list;
            }
            catch (Exception)
            {
                
                //throw;
            }
            return View(rsl);
        }
    }
}