#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using STP.Common.Constants;
using STP.Domain;
using System.Web.Mvc;
using STP.Common.Logger;
using STP.Domain.SecurityAndUsers;
using STP.Web.WorkflowProvider;
#endregion

namespace STP.Web.Controllers
{
    public class HelpdeskController : Controller
    {
        // GET: Helpdesk
        public ActionResult Index()
        {
            //if (Session["UserInfo"] == null)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            //if (Convert.ToString(Session["userTypeId"]).Equals("696006"))
            //{
            var sessionValues = (UserInfo)Session["UserInfo"];
            int portalType = sessionValues.UserTypeId;
            switch (portalType)
            {
                case 696001:
                    ViewBag.PortalType = Constants.Haulier;
                    break;
                case 696002:
                    ViewBag.PortalType = Constants.Police;
                    break;
                case 696003:
                    ViewBag.PortalType = Constants.OPS;
                    break;
                case 696004:
                    ViewBag.PortalType = Constants.MIS;
                    break;
                case 696005:
                    ViewBag.PortalType = Constants.PUBLIC;
                    break;
                case 696006:
                    ViewBag.PortalType = Constants.Admin;
                    break;
                case 696007:
                    ViewBag.PortalType = Constants.SOA;
                    break;
                case 696008:
                    ViewBag.PortalType = Constants.SORT;
                    break;
                default:
                    ViewBag.PortalType = "";
                    break;
            }

            if (TempData["SetPreferenceMessage"] != null && (string)TempData["SetPreferenceMessage"] == "1")
            {
                ViewBag.SetPreferenceMessage = "1";
            }

            ViewBag.DaysToExpire = TempData["ExpiryDays"];
            Session["PreferenceNotesBLOB"] = null;
            Session["PreferenceNotes"] = null;


            if (TempData["success"] != null && (string)TempData["success"] == "1")
            {
                ViewBag.saveMsg = "1";
            }

            ViewBag.RedirectToChangePwd = TempData["RedirectToChangePwd"];
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                if (SessionInfo != null)
                {
                    if (SessionInfo.HelpdeskRedirect == "true")
                    {
                        ViewBag.HelpdeskDistRedirect = "true";
                    }
                    else
                    {
                        ViewBag.HelpdeskDistRedirect = "false";
                    }
                }
                else
                {
                    ViewBag.HelpdeskDistRedirect = "false";
                }
            }
            return View();
            //}
            //else
            //{
            //    return RedirectToAction("LogOut", "Account");
            //}
        }
        public ActionResult test()
        {
            return View();
        }
    }
}