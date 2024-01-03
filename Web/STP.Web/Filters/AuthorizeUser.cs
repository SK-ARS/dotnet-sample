using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using STP.Domain;
using STP.Domain.SecurityAndUsers;

namespace STP.Web.Filters
{
    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class AuthorizeUser : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string path = HttpContext.Current.Request.RawUrl;
            var requiredRoles = Roles.Split(Convert.ToChar(","));
            UserInfo userInfo = null;
            if (HttpContext.Current.Session["UserInfo"] != null)
            {
                userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            }

            if (userInfo == null)
            {
                filterContext.Result = new RedirectToRouteResult(SessionValidateRoute(path));
            }
            
            if (System.Web.HttpContext.Current.Session["MenuAccess"] != null)
            {
                var menuDetails = (MenuAccess)System.Web.HttpContext.Current.Session["MenuAccess"];
                var menuAccessList = new List<string>();
                foreach (var item in menuDetails.MenuAccessInfo)
                {
                    menuAccessList.Add(item.MenuId);
                }
                var matchingMenus = requiredRoles.Intersect(menuAccessList, StringComparer.OrdinalIgnoreCase);
                if (matchingMenus.Count() <= 0)
                {
                    if (!requiredRoles.Contains("70001"))
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "NotFound", controller = "Error" }));
                }
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
