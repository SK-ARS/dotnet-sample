using PagedList;
using STP.Common.Logger;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.SecurityAndUsers;
using STP.ServiceAccess.LoggingAndReporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STP.Web.Controllers
{
    public class LoggingController : Controller
    {

        private readonly ILoggingService loggingService;
        public LoggingController(ILoggingService loggingService)
        {
            this.loggingService = loggingService;
        }
        #region NENAuditLogPopup
        public ActionResult NENAuditLogPopup(int pageNum = 1, int pageSize = 10, string NEN_Notif_No = "", int? sortOrder = null, int? sortType = null)
        {
            try
            {
                string messg = "NENNotification/NENAuditLogPopup?pageNum=" + pageNum + "pageSize=" + pageSize;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                ViewBag.pageNum = pageNum;
                ViewBag.pageSize = pageSize;
                ViewBag.NEN_Notif_No = NEN_Notif_No;
                sortOrder = sortOrder != null && sortOrder != 0 ? (int)sortOrder : 1; //date
                sortType = sortType != null ? (int)sortType : 1; // asc
                ViewBag.SortOrder = sortOrder;
                ViewBag.SortType = sortType;
                return PartialView();
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/NENAuditLogPopup, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region NEN Audit Log per NEN notification in Authorized Movement page
        public ActionResult ListNENAuditPerNotification(int? page, int? pageSize, string NEN_Notif_No = "", int? sortOrder = null, int? sortType = null)
        {
            try
            {
                List<NENAuditGridList> GridListObj = new List<NENAuditGridList>();
                //Verifying session
                sortOrder = sortOrder != null && sortOrder != 0 ? (int)sortOrder : 1; //date
                sortType = sortType != null ? (int)sortType : 1; // asc
                ViewBag.SortOrder = sortOrder;
                ViewBag.SortType = sortType;
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (NEN_Notif_No != "" && NEN_Notif_No != null)
                {
                    NEN_Notif_No = NEN_Notif_No.Replace("~", "#");
                }
                if (pageSize == null)
                {
                    pageSize = ((Session["PageSize"] != null) && (Convert.ToString(Session["PageSize"]) != "0")) ? (int)Session["PageSize"] : 10;
                }
                else
                {
                    Session["PageSize"] = pageSize;
                }
                if (NEN_Notif_No == "")
                {
                    NEN_Notif_No = ((Session["NEN_Notif_No"] != null) && (Convert.ToString(Session["NEN_Notif_No"]) != "")) ? (string)Session["NEN_Notif_No"] : "";
                }
                else
                {
                    Session["NEN_Notif_No"] = NEN_Notif_No;
                }


                GridListObj = loggingService.GetAuditlogNEN(page, pageSize, NEN_Notif_No, SessionInfo.OrganisationId,(int)sortOrder,sortType);

                int tempPageCount = 0;
                if (GridListObj != null && GridListObj.Count != 0)
                {
                    tempPageCount = (int)GridListObj[0].RecordCount;
                }
                ViewBag.TotalCount = tempPageCount;



                int pageNumber = page ?? 1;
                
                ViewBag.pageSize = pageSize;
                ViewBag.page = page;
                ViewBag.NEN_Notif_No = NEN_Notif_No;
                var AuditLogPageList = new StaticPagedList<NENAuditGridList>(GridListObj, pageNumber, (int)pageSize, ViewBag.TotalCount);
                return View("ListNENAuditPerNotification", AuditLogPageList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/ListNENAuditPerNotification, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        #endregion
    }
}