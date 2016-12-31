using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityNews.Api.Models
{
    public class ActityViewModel
    {
        public int ID { get; set; }
        public int OrderIndex { get; set; }
        public string Leader { get; set; }
        public string Contents { get; set; }
        public string Arrange { get; set; }
   
        public string Time { get; set; }
        public string FilePath { get; set; }
        public string Name { get; set; }
        public int PersonCount { get; set; }
        public DateTime SignEndTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public DateTime AddTime { get; set; }
    }
}