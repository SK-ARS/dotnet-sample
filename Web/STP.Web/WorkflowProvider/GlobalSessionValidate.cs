using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace STP.Web.WorkflowProvider
{
    public class GlobalSessionValidate : ActionFilterAttribute
    {
        public UserInfo SessionInfo = null;

       

        public override void OnActionExecuting(ActionExecutingContext filterContext)
            
        {
        var objSession = filterContext.HttpContext;

            
        
        /// otherwise we redirect the user to the login page
         if (objSession.Session["UserInfo"] == null)
        {
                FormsAuthentication.SignOut();
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new RedirectResult("~/Account/RedirectOnLogin");
                }
                else
                { 
                    filterContext.Result = new RedirectResult("~/Account/Login");
                }
                return;
        }
            base.OnActionExecuting(filterContext);
        }

    public override void OnResultExecuting(ResultExecutingContext filterContext)
    {
        base.OnResultExecuting(filterContext);

        /// we set a field 'IsAjaxRequest' in ViewBag according to the actual request type
        filterContext.Controller.ViewBag.IsAjaxRequest = filterContext.HttpContext.Request.IsAjaxRequest();
    }
    
    }
}