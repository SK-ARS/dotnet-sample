using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace STP.Web.Filters
{
    public class SessionValidate : ActionFilterAttribute
    {
        public bool Disable { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Disable) return;

            string path = HttpContext.Current.Request.RawUrl;

            var session = filterContext.HttpContext;
            if (session.Session["UserInfo"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(SessionValidateRoute(path));
            }
        }

        private RouteValueDictionary SessionValidateRoute(string returnUrl = "")
        {
            return new RouteValueDictionary
            {
                {"action", "Login"},
                {"controller", "Users"},
                {"B7vy6imTleYsMr6Nlv7VQ", STP.Web.Helpers.EncryptionUtility.Encrypt("returnUrl=" + returnUrl) }
            };
        }
    }
}