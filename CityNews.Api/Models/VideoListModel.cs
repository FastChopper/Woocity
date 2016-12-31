using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityNews.Api.Models
{
    public class VideoListModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Introduce { get; set; }
        public string Name { get; set; }
        public string User { get; set; }

    }
}