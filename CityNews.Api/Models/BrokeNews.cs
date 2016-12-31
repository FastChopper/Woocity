using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityNews.Api.Models
{
    public class BrokeNews
    {
        public string Phone { get; set; }
        public string Contents { get; set; }
        public string Title { get; set; }
        public List<FileWAy> FilePath { get; set; }
      
    }

    public class FileWAy
    {
        public string Path { get; set; }
        public string FileName { get; set; }
        public string PathMin { get; set; }


    }
}