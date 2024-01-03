using STP.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STP.Web.Controllers
{
    [AllowAnonymous]
    [SessionValidate(Disable = true)]
    public class HealthCheckController : Controller
    {
        // GET: HealthCheck
        public ActionResult Index()
        {
            return Json(new { },JsonRequestBehavior.AllowGet);
        }
    }
}