using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityNews.Api.Models
{
    public class RequestJson
    {
        public bool Success { get; set; }
        public string Msg { get; set; }
        public string Url { get; set; }

        public string FileName { get; set; }
    }
}