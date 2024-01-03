using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace STP.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AntiForgeryCookie : FilterAttribute, IActionFilter
    {
        public bool SessionCookieReset { get; set; }
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            AntiForgery.GetTokens(null, out string cookieToken, out string _);
            var cookie = new HttpCookie(AntiForgeryConfig.CookieName, cookieToken)
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.MinValue
            };
            filterContext.HttpContext.Response.Cookies.Add(cookie);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}