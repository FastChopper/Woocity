using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityNews.Api.Models
{
    public class Actities
    {
        public string Leader { get; set; }
        public string Name { get; set; }

        public string Contents { get; set; }
        public string Arrange { get; set; }
        public string Phone { get; set; }
        public DateTime SignEndTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<FileWAy> FilePath { get; set; }
    }
}