using STP.Web.Filters;
using System.Web;
using System.Web.Mvc;

namespace STP.Web
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new UrlValidate());
            filters.Add(new SessionValidate());
            filters.Add(new ContentSecurityPolicy());
        }
    }
}
