using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityNews.Model.Common
{
    public enum ResultCode
    {
        Success = 0,
        Error = 100,
        DatabaseConnError = 101,
        ElementNotFindError = 102,
        AlreadyLiked = 201,
        AlreadyScored = 202,
        CreateFailed = 301,
        UpdateFailed = 302,
        DeleteFailed = 303,
        OrderFailed = 304,
        ModifyFailed = 305,
        RequestForbbiden = 403
    };
    public class Result
    {
        public ResultCode Code { get; set; }
        public string Msg { get; set; }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }
    }

    public class ResultList<T> : Result
    {
        public int RecordCount { get; set; }
        public List<T> Data { get; set; }
    }

    public class RequestJson
    {
        public bool Success { get; set; }
        public string Msg { get; set; }
        public string Url { get; set; }

        public string FileName { get; set; }
        public string UrlMin { get; set; }
    }
}
