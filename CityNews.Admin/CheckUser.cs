using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jstv.AccessControl;

namespace CityNews.Admin
{
    public class CheckUser
    {
        public  static bool Check(string Roles)
        {
            AccessHelper<object> accessHelper = new AccessHelper<object>();
            if (accessHelper.IsAuthed)
            {
                if (string.IsNullOrEmpty(Roles))
                    return true;
                string[] roles = accessHelper.Role.Split(',');
                return Roles.Split(',').Any(i => roles.Contains(i));
            }
            return false;
        }
    }
}