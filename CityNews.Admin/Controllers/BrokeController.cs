using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CityNews.Admin.Models;
using CityNews.Model;
using CityNews.Model.Common;

namespace CityNews.Admin.Controllers
{
    public class BrokeController : Controller
    {
        CityNewsEntities db = new CityNewsEntities();
        // GET: Broke
        public ActionResult Index()
        {
            int PageIndex = PagerHelper.GetPageIndex();
            int PageSize = 20;
            var rsl = new ResultList<BrokeViewModel>
            {
                Msg = "获取失败",
                Code = ResultCode.Error,
                Data = new List<BrokeViewModel>(),
            };
            try
            {
                string state = Request.QueryString["state"];
                string type = Request.QueryString["type"];
                var itemList = new List<CityNews_Dict>
                {
                    new CityNews_Dict {Text = "请选择",Value = 6}
                };
                itemList.AddRange(db.CityNews_Dict.Where(i => i.Type == "标识类别").OrderBy(i => i.OrderIndex).ToList());
                var brokeState = new List<CityNews_Dict>
                {
                    new CityNews_Dict {Text = "请选择",Value = 6}
                };
                brokeState.AddRange(db.CityNews_Dict.Where(i => i.Type == "爆料状态").OrderBy(i => i.OrderIndex).ToList());
                var model = db.CityNews_BrokeNews.Select(i => new BrokeViewModel
                {
                    ID = i.ID,
                    Contents = i.Contents,
                    Creatime = i.Creatime,
                    Explain = i.Explain,
                    FilePath = i.FilePath,
                    Phone = i.Phone,
                    State = i.State,
                    Status = i.Status,
                    StatusName = db.CityNews_Dict.FirstOrDefault(d => d.Value == i.Type && d.Type == "标识类别").Text,
                    StateName = db.CityNews_Dict.FirstOrDefault(d => d.Value == i.State && d.Type == "爆料状态").Text,
                    Type = i.Type,
                    OrderIndex = i.OrderIndex
                });
                var recordcount = db.CityNews_BrokeNews.Where(i => true);
                if (!string.IsNullOrEmpty(state) && state != "6")
                {
                    int stateint = Convert.ToInt16(state);
                    model = model.Where(i => i.State == stateint);
                    recordcount = recordcount.Where(i => i.State == stateint);
                }
                if (!string.IsNullOrEmpty(type) && type != "6")
                {
                    int typeint = Convert.ToInt16(type);
                    model = model.Where(i => i.Type == typeint);
                    recordcount = recordcount.Where(i => i.State == typeint);
                }
                ViewBag.RecordCount = recordcount.Count();
                ViewBag.ItemTypeList = new SelectList(itemList, "Value", "Text",type);
                ViewBag.brokeState = new SelectList(brokeState, "Value", "Text",state);
                rsl.Data = model.OrderByDescending(i => i.OrderIndex)
                .Skip((PageIndex - 1) * PageSize)
                    .Take(PageSize).ToList();
                rsl.Msg = "获取成功！";
                rsl.Code = ResultCode.Success;
            }
            catch (Exception ex)
            {
                //
            }
            return View(rsl);
        }

        #region 终极管理员审核
        [HttpGet]
        public ActionResult Modify(int id)
        {
            var broke = db.CityNews_BrokeNews.FirstOrDefault(i => i.ID == id);
            var itemList = db.CityNews_Dict.Where(i => i.Type == "标识类别").OrderBy(i => i.OrderIndex).ToList();
            var brokeState = db.CityNews_Dict.Where(i => i.Type == "爆料状态").OrderBy(i => i.OrderIndex).ToList();
            var brokeStatus = db.CityNews_Dict.Where(i => i.Type == "发布状态").OrderBy(i => i.OrderIndex).ToList();
            if (broke == null)
            {
                return HttpNotFound();
            }
            var model = new BrokeFormModel
            {
                Contents = broke.Contents,
                Explain = broke.Explain,
                FilePath = db.CityNews_File.Where(i => i.Fk_ID == broke.FilePath).ToList(),
                State = broke.State,
                Type = broke.Type,
                Status = broke.Status,
                Title = broke.Title
            };
            ViewBag.ItemTypeList = new SelectList(itemList, "Value", "Text", model.Type);
            ViewBag.brokeState = new SelectList(brokeState, "Value", "Text", model.State);
            ViewBag.brokeStatus = new SelectList(brokeStatus, "Value", "Text", model.Status);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modify(int id, BrokeFormModel model)
        {
            var broke = db.CityNews_BrokeNews.FirstOrDefault(i => i.ID == id);
            if (broke == null)
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
            if (model.State == 1)
            {
                broke.ExplainTime = DateTime.Now;
            }
            else if (model.State == 2)
            {
                if (broke.State == 0)
                {
                    broke.ExplainTime = DateTime.Now;
                    broke.IficationTime = DateTime.Now;
                }
                else if (broke.State == 1)
                {
                    broke.IficationTime = DateTime.Now;
                }
                else if (broke.State == 2)
                {
                    broke.ExplainTime = DateTime.Now;
                }
            }
            broke.Contents = model.Contents;
            broke.Explain = model.Explain ?? "";
            broke.State = model.State;
            broke.Type = model.Type;
            broke.Status = model.Status;
            broke.Title = model.Title;
            try
            {
                db.SaveChanges();
                rsl.Code = ResultCode.Success;
                rsl.Msg = "修改成功";
            }
            catch
            {
                rsl.Msg = "修改失败!";
                rsl.Code = ResultCode.CreateFailed;
                return Json(rsl);
            }
            return Json(rsl);

        }
        #endregion

        #region 审核
        [HttpGet]
        public ActionResult Check(int id)
        {
            var broke = db.CityNews_BrokeNews.FirstOrDefault(i => i.ID == id);
            var itemList = db.CityNews_Dict.Where(i => i.Type == "标识类别").OrderBy(i => i.OrderIndex).ToList();
            //var brokeState = db.CityNews_Dict.Where(i => i.Type == "爆料状态").OrderBy(i => i.OrderIndex).ToList();
            var brokeStatus = db.CityNews_Dict.Where(i => i.Type == "发布状态").OrderBy(i => i.OrderIndex).ToList();
            if (broke == null)
            {
                return HttpNotFound();
            }
            var model = new BrokeFormModel
            {
                Contents = broke.Contents,
                Explain = broke.Explain,
                FilePath = db.CityNews_File.Where(i => i.Fk_ID == broke.FilePath).ToList(),
                State = 2,
                Status = broke.Status,
                Title = broke.Title
            };
            ViewBag.ItemTypeList = new SelectList(itemList, "Value", "Text", model.Type);
            //ViewBag.brokeState = new SelectList(brokeState, "Value", "Text", model.State);
            ViewBag.brokeStatus = new SelectList(brokeStatus, "Value", "Text", model.Status);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Check(int id, BrokeFormModel model)
        {
            var broke = db.CityNews_BrokeNews.FirstOrDefault(i => i.ID == id);
            if (broke == null)
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
            broke.ExplainTime = DateTime.Now;
            broke.Contents = model.Contents;
            broke.Explain = model.Explain ?? "";
            broke.State = model.State;
            broke.Status = model.Status;
            broke.Title = model.Title;
            try
            {
                db.SaveChanges();
                rsl.Code = ResultCode.Success;
                rsl.Msg = "修改成功";
            }
            catch
            {
                rsl.Msg = "修改失败!";
                rsl.Code = ResultCode.CreateFailed;
                return Json(rsl);
            }
            return Json(rsl);

        }
        #endregion

        #region 分类
        [HttpGet]
        public ActionResult IfCation(int id)
        {
            var broke = db.CityNews_BrokeNews.FirstOrDefault(i => i.ID == id);
            var itemList = db.CityNews_Dict.Where(i => i.Type == "标识类别").OrderBy(i => i.OrderIndex).ToList();
            //var brokeState = db.CityNews_Dict.Where(i => i.Type == "爆料状态").OrderBy(i => i.OrderIndex).ToList();
            //var brokeStatus = db.CityNews_Dict.Where(i => i.Type == "发布状态").OrderBy(i => i.OrderIndex).ToList();
            if (broke == null)
            {
                return HttpNotFound();
            }
            var model = new BrokeFormModel
            {
                Contents = broke.Contents,
                Explain = broke.Explain,
                FilePath = db.CityNews_File.Where(i => i.Fk_ID == broke.FilePath).ToList(),
                State = broke.State,
                Type = broke.Type,
                Status = broke.Status,
                Title = broke.Title
            };
            ViewBag.ItemTypeList = new SelectList(itemList, "Value", "Text", model.Type);
            //ViewBag.brokeState = new SelectList(brokeState, "Value", "Text", model.State);
            // ViewBag.brokeStatus = new SelectList(brokeStatus, "Value", "Text", model.Status);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IfCation(int id, BrokeFormModel model)
        {
            var broke = db.CityNews_BrokeNews.FirstOrDefault(i => i.ID == id);
            if (broke == null)
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
            broke.IficationTime = DateTime.Now;
            broke.Type = model.Type;
            broke.State = 1;
            try
            {
                db.SaveChanges();
                rsl.Code = ResultCode.Success;
                rsl.Msg = "修改成功";
            }
            catch
            {
                rsl.Msg = "修改失败!";
                rsl.Code = ResultCode.CreateFailed;
                return Json(rsl);
            }
            return Json(rsl);

        }
        #endregion

        #region 图片和Video
        [HttpGet]
        public ActionResult Pic(int id)
        {
            var broke = db.CityNews_File.FirstOrDefault(i => i.ID == id);
            if (broke == null)
            {
                return HttpNotFound();
            }
            var model = new FileViewModel
            {
                FilePath = broke.FilePath,
                FilePathMin = broke.MinPath
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Pic(int id, FileViewModel file)
        {
            var broke = db.CityNews_File.FirstOrDefault(i => i.ID == id);
            if (broke == null)
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
            broke.FilePath = file.FilePath;
            broke.MinPath = file.FilePathMin;
            try
            {
                db.SaveChanges();
                rsl.Code = ResultCode.Success;
                rsl.Msg = "修改成功";
            }
            catch
            {
                rsl.Msg = "修改失败!";
                rsl.Code = ResultCode.CreateFailed;
                return Json(rsl);
            }
            return Json(rsl);
        }

        [HttpGet]
        public ActionResult Video(int id)
        {
            var broke = db.CityNews_File.FirstOrDefault(i => i.ID == id);
            if (broke == null)
            {
                return HttpNotFound();
            }
            var model = new FileViewModel
            {
                FilePath = broke.FilePath
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Video(int id, FileViewModel file)
        {
            var broke = db.CityNews_File.FirstOrDefault(i => i.ID == id);
            if (broke == null)
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
            broke.FilePath = file.FilePath;
            try
            {
                db.SaveChanges();
                rsl.Code = ResultCode.Success;
                rsl.Msg = "修改成功";
            }
            catch
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
            var user = db.CityNews_BrokeNews.FirstOrDefault(i => i.ID == id);
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


        public ActionResult SortOrder(int src, int des)
        {
            try
            {
                db.ChangeCityNews_BrokeIndex(src, des);
                return Json(0);
            }
            catch (Exception)
            {
                return Json(-1)
                    ;
            }
        }

    }
}