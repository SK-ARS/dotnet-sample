using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace STP.Web.Filters
{
    public class UrlValidate : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute)
                || url.ToLower().Contains("javascript:") || url.ToLower().Contains("javascript%3A")
                || url.ToLower().Contains("<script>") || url.ToLower().Contains("%3Cscript%3E") 
                || url.ToLower().Contains("</script>") || url.ToLower().Contains("%3C/script%3E"))
            {
                filterContext.Result = new RedirectToRouteResult(UrlInvalidRoute());
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }

        private RouteValueDictionary UrlInvalidRoute()
        {
            return new RouteValueDictionary
            {
                {"action", "BadRequest"},
                {"controller", "Error"}
            };
        }
    }
}