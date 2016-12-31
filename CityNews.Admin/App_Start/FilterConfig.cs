using System.Web;
using System.Web.Mvc;
using Jstv.AccessControl;

namespace CityNews.Admin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CheckAccessFilter());
        }
    }
}
