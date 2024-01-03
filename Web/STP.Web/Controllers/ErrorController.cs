using STP.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STP.Web.Controllers
{
    [SessionValidate(Disable = true)]
    public class ErrorController : Controller
    {
        // GET: Error
        [UnValidateAntiForgeryToken]
        public ActionResult BadRequest()
        {
            return View();
        }

        [UnValidateAntiForgeryToken]
        public ActionResult UnauthorizedAccess()
        {
            return View();
        }

        [UnValidateAntiForgeryToken]
        public ActionResult NotFound()
        {
            var isAuthenticated = Session != null && Session["UserInfo"] != null;
            if(!isAuthenticated)
                Response.Redirect("/Account/Login");
            //Session.RemoveAll();
            return View();
        }
    }
}