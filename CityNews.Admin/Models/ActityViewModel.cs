using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CityNews.Model;

namespace CityNews.Admin.Models
{
    public class ActityViewModel
    {
        public int ID { get; set; }
        public string Leader { get; set; }
        public string Name { get; set; }
        public string Contents { get; set; }
        public string Arrange { get; set; }
        public System.DateTime AddTime { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public string ActityTime { get; set; }
        public int State { get; set; }
        public int OrderIndex { get; set; }
        public string StateName { get; set; }
        public int Status { get; set; }
        public string FilePath { get; set; }
        public string Phone { get; set; }


    }

    public class ActityFormModel
    {
        [Display(Name = "发起人")]
        [MaxLength(20, ErrorMessage="最大20字符")]
        [Required(ErrorMessage = "发起人必填")]
        public string Leader { get; set; }
        [Display(Name = "活动名称")]
        [MaxLength(30, ErrorMessage = "最大30字符")]
        [Required(ErrorMessage = "活动名称必填")]
        public string Name { get; set; }

        [Display(Name = "活动内容")]
        [Required(ErrorMessage = "活动内容必填")]
        public string Contents { get; set; }
        [Display(Name = "活动安排")]
        [Required(ErrorMessage = "活动安排必填")]
        public string Arrange { get; set; }

        [Display(Name = "报名截止时间")]
        [Required(ErrorMessage = "报名截止时间必填")]
        public System.DateTime SignEndTime { get; set; }

        [Display(Name = "活动开始时间")]
        [Required(ErrorMessage = "活动开始时间必填")]
        public System.DateTime StartTime { get; set; }

        [Display(Name = "活动结束时间")]
        [Required(ErrorMessage = "活动开始时间必填")]
        public System.DateTime EndTime { get; set; }
        [Display(Name = "状态")]
       
        public int State { get; set; }
        [Display(Name = "发布状态")]
        public int Status { get; set; }
        public List<CityNews_File> FilePath { get; set; }

    }
}