using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CityNews.Model;

namespace CityNews.Api.Models
{
    public class GetBrokeModel
    {

        public int ID { get; set; }
        public int OrderIndex { get; set; }
        public string IP { get; set; }
        public int IPInt { get; set; }
        public string Phone { get; set; }
        public string Contents { get; set; }
        public string Title { get; set; }

        public List<FileWAy> FilePathList { get; set; }
        private string _filePath;
        public string FilePath
        {
            get
            {
                return _filePath;
            }

            set
            {
                _filePath = value;
                CityNewsEntities db = new CityNewsEntities();
                FilePathList = db.CityNews_File.Where(i => i.Fk_ID == _filePath).Select(i => new FileWAy
                {
                    FileName = i.FileRealName,
                    Path = i.FilePath,
                    PathMin = i.MinPath
                }).ToList();
            }
        }
        public int Type { get; set; }
        public int State { get; set; }
        public System.DateTime Creatime { get; set; }
        public int Status { get; set; }
        public string Explain { get; set; }

        public string HmExplainTime { get; set; }
        private System.DateTime _ExplainTime;
        public System.DateTime ExplainTime
        {
            get
            {
                return _ExplainTime;
            }

            set
            {
                _ExplainTime = value;
                HmExplainTime = _ExplainTime.ToString("HH:mm");
            }
        }

        public string HmIficationTime { get; set; }

        private System.DateTime _IficationTime;
        public System.DateTime IficationTime
        {
            get
            {
                return _IficationTime;
            }

            set
            {
                _IficationTime = value;
                HmIficationTime = _IficationTime.ToString("HH:mm");

            }
        }

      
    }
}