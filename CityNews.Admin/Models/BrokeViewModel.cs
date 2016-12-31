using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CityNews.Model;

namespace CityNews.Admin.Models
{
    public class BrokeViewModel
    {
        public int ID { get; set; }
        public string Phone { get; set; }
        public string Contents { get; set; }
        public string FilePath { get; set; }
        public int Type { get; set; }
        public int State { get; set; }
        public System.DateTime Creatime { get; set; }
        public int Status { get; set; }
        public string Explain { get; set; }
        public int OrderIndex { get; set; }

        public string StatusName { get; set; }
        public string StateName { get; set; }
    }

    public class BrokeFormModel
    {
        [Display(Name = "爆料内容")]
        [Required(ErrorMessage = "爆料内容必填")]
        public string Contents { get; set; }
        [Display(Name = "标题")]
        [MaxLength(24,ErrorMessage = "标题最长24个字符")]
        [Required(ErrorMessage = "标题必填")]
        public string Title { get; set; }
        public List<CityNews_File> FilePath { get; set; }

        [Display(Name = "爆料分类")]
        public int Type { get; set; }
        [Display(Name = "状态")]
        public int State { get; set; }
        [Display(Name = "进度反馈")]
        public string Explain { get; set; }
        [Display(Name = "发布状态")]
        public int Status { get; set; }


    }
}