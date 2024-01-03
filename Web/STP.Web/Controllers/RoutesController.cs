using PagedList;
using STP.Domain.SecurityAndUsers;
using STP.Web.Filters;
using STP.ServiceAccess.LoggingAndReporting;
using STP.ServiceAccess.MovementsAndNotifications;
using STP.ServiceAccess.Routes;
using STP.ServiceAccess.VehiclesAndFleets;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using static STP.Domain.Routes.RouteModel;
using STP.Domain.LoggingAndReporting;
using STP.Common.Constants;
using STP.Domain.Routes;
using STP.Common.Logger;
using static STP.Domain.Routes.RouteModelJson;
using Newtonsoft.Json;
using STP.Web.WorkflowProvider;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Common.Enums;
using STP.ServiceAccess.Applications;
using STP.ServiceAccess.Workflows.ApplicationsNotifications;
using STP.Domain.Workflow;
using System.Dynamic;
using STP.Web.General;
using STP.Domain.Workflow.Models;
using STP.ServiceAccess.RouteAssessment;
using STP.Domain.MovementsAndNotifications.Notification;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace STP.Web.Controllers
{
    //[AuthorizeUser(Roles = "40000,40001,40002,13003,13004,13005,13006,100001,100002,300000")]
    public class RoutesController : Controller
    {
        private readonly IRoutesService routesService;
        private readonly ILoggingService loggingService;
        private readonly IAuditLogService auditLogService;
        private readonly INENNotificationService nenNotificationService;
        private readonly IVehicleConfigService vehicleconfigService;
        private readonly IApplicationNotificationWorkflowService applicationNotificationWorkflowService;
        private readonly IRouteAssessmentService routeAssessmentService;
        public RoutesController(IRoutesService routesService, ILoggingService loggingService, IAuditLogService auditLogService, INENNotificationService nenNotificationService, IVehicleConfigService vehicleconfigService, IApplicationNotificationWorkflowService applicationNotificationWorkflowService, IRouteAssessmentService routeAssessmentService)
        {
            this.routesService = routesService;
            this.loggingService = loggingService;
            this.auditLogService = auditLogService;
            this.nenNotificationService = nenNotificationService;
            this.vehicleconfigService = vehicleconfigService;
            this.applicationNotificationWorkflowService = applicationNotificationWorkflowService;
            this.routeAssessmentService = routeAssessmentService;
        }
        public ActionResult Preferences(string mode)
        {
            int UserId = 0;
            ViewBag.mode = "Get";
            if (Session["UserInfo"] != null)
            {
                var sessionValues = (UserInfo)Session["UserInfo"];
                UserId = Convert.ToInt32(sessionValues.UserId);
            }
            ViewBag.UserId = UserId;

            return PartialView("Preferences");
        }
        public ActionResult A2BPlanning(long routeID)
        {
            return PartialView("A2BPlanning");
        }
        [AuthorizeUser(Roles = "40000,40001,40002,40003,40004")]
        public ActionResult A2BLeftPanel(long routeID = 0, string val = "", int? x = 0, int? y = 0, bool ShowReturnLegFlag = false)
        {
            if (val == "AffectedStructure")
            {
                ViewBag.IsAffectedStructure = true;
                ViewBag.X = x;
                ViewBag.Y = y;
            }
            else
                ViewBag.IsAffectedStructure = false;
            if (Session["RouteFlag"] != null && Session["plannedRouteId"] != null && routeID == 0)
                routeID = Convert.ToInt32(Session["plannedRouteId"].ToString());
            ViewBag.isReplan = false;
            ViewBag.IsLibrary = false;
            if (routeID != 0)
            {
                Session["plannedRouteId"] = routeID;
                ViewBag.IsNewRoute = false;
                #region for broken route
                //code for checking broken routes
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                List<BrokenRouteList> brokenRouteIds;
                GetBrokenRouteList brokenRouteList = new GetBrokenRouteList { UserSchema = SessionInfo.UserSchema };
                if (Session["IsLibrary"] != null && (bool)Session["IsLibrary"])//check for library route
                {
                    int INBOX_ITEM_ID = Convert.ToInt32(Session["NENINBOX_ITEM_ID"]);
                    if (INBOX_ITEM_ID == 0) //check added for indicationg the current route is not nen route(Nen route uses the same logic of library routes but it contains at portal side)
                    {
                        brokenRouteList.LibraryRouteId = routeID;
                        brokenRouteIds = routesService.GetBrokenRouteIds(brokenRouteList);
                        ViewBag.IsLibrary = true;//indication library route
                    }
                    else
                    {
                        brokenRouteList.RoutePartId = routeID;
                        brokenRouteIds = routesService.GetBrokenRouteIds(brokenRouteList);
                    }
                }
                else
                {
                    brokenRouteList.RoutePartId = routeID;
                    brokenRouteIds = routesService.GetBrokenRouteIds(brokenRouteList);
                }
                if (brokenRouteIds.Count > 0 && brokenRouteIds[0].IsBroken > 0 && brokenRouteIds[0].IsReplan <= 1)
                    ViewBag.isReplan = true;     //If the route is broken and is replannable (No special maneouver )Then  show the replan but on UI 
                #endregion
            }
            else
            {
                ViewBag.IsNewRoute = true;
                if (Session["IsLibrary"] != null && (bool)Session["IsLibrary"])//check for library route
                {
                    ViewBag.IsLibrary = true;//indication library route
                }
            }
            ViewBag.routeID = routeID;
            ViewBag.ShowReturnLegFlag = ShowReturnLegFlag;
            return PartialView("A2BLeftPanel");
        }

        [HttpPost]
        public JsonResult IsSessionActive(double rand)
        {
            return Json(new { Success = Session["UserInfo"] != null });
        }

        // [AuthorizeUser(Roles = "40001,40000,13003,13004,13005,13006,300000")]
        [AuthorizeUser(Roles = "40000,40001,40002,40003,40004")]
        public ActionResult RoutePartLibrary(int page = 0, int pageSize = 0, string SearchStrings = null, int flag = 0, bool isApplicationroute = false, long ApplicationRevId = 0, bool isNotifroute = false, int RouteType = 0, bool IsSimpleN = false, int filterFavouritesRoutes = 0, bool importFlag = false, int? sortOrder = null, int? sortType = null, bool clearSearch = false)
        {
            if (clearSearch)
            {
                Session["UserSearchString"] = null;
                SearchStrings = null;
                Session["UserSearchStringPlanMvmt"]=null;
            }
            var sessionValues = (UserInfo)Session["UserInfo"];
            sortOrder = sortOrder != null ? (int)sortOrder : 1; //Route name
            int presetFilter = sortType != null ? (int)sortType : 0; // asc
            ViewBag.SortOrder = sortOrder;
            ViewBag.SortType = presetFilter;
            if (Session["RouteFlag"] != null && Session["RouteFlag"].ToString() == "2" && RouteType != 2)
                RouteType = 0;
            if (importFlag == true)
                RouteType = 2;//outline routes are not needed to be imported in esdal4
            if (flag == 1)
            {
                Session["g_RouteSearch"] = null;
                Session["buttonflag"] = null;
            }
            if (Session["g_RouteSearch"] != null)
                SearchStrings = (string)Session["g_RouteSearch"];
            if (Session["filterFavouritesRoutes"] != null)
            {
                filterFavouritesRoutes = (int)Session["filterFavouritesRoutes"];
                ViewBag.filterFavouritesRoutes = Session["filterFavouritesRoutes"];

            }
            if (importFlag)
            {
                if (Session["UserSearchStringPlanMvmt"] != null)
                {
                    SearchStrings = (string)Session["UserSearchStringPlanMvmt"];
                    ViewBag.UserSearchString = Session["UserSearchStringPlanMvmt"];
                }
            }
            else
            {
                if (Session["UserSearchString"] != null)
                {
                    SearchStrings = (string)Session["UserSearchString"];
                    ViewBag.UserSearchString = Session["UserSearchString"];
                }
            }
            

            if (Session["buttonflag"] != null)
                isApplicationroute = true;
            else
                Session["buttonflag"] = null;
            int pageNumber = 1;
            if (page != 0)
                pageNumber = page;
            if (Session["PageSize"] == null)
                Session["PageSize"] = 10;
            if (pageSize == 0)
                pageSize = (Session["PageSize"] != null) ? (int)Session["PageSize"] : 10;
            else
                Session["PageSize"] = pageSize;
            int organisationID;
            if (sessionValues.UserTypeId == 696008)
                organisationID = (int)Session["SORTOrgID"];
            else
                organisationID = (int)sessionValues.OrganisationId;
            if (filterFavouritesRoutes == 0)
            {
                ViewBag.filterFavouritesRoutes = false;


            }
            else
            {
                ViewBag.filterFavouritesRoutes = true;
            }


            List<RoutePartDetails> GridListObj1 = routesService.LibraryRouteList(organisationID, pageNumber, pageSize, RouteType, SearchStrings, sessionValues.UserSchema, filterFavouritesRoutes, presetFilter, sortOrder);
            int totalCount = 0;
            if (GridListObj1.Count > 0)
            {
                totalCount = GridListObj1[0].TotalRecord;
            }
            var routeListAsIPagedList = new StaticPagedList<RoutePartDetails>(GridListObj1, pageNumber, pageSize, totalCount);
            //if (GridListObj1.Count <= 10 || (Convert.ToString(Session["UserSearchString"]) != SearchString && SearchString != null))
            //   pageNumber = 1;
            //apply for SO
            ViewBag.ApprevisionId = ApplicationRevId;
            ViewBag.isNotifroute = isNotifroute;

            if (isApplicationroute || (Session["RouteFlag"] != null && Session["RouteFlag"].ToString() == "2"))
                ViewBag.IsSOApplication = true;
            else
                ViewBag.IsSOApplication = false;
            if (Session["RouteFlag"] != null && (Session["RouteFlag"].ToString() == "3" || Session["RouteFlag"].ToString() == "1"))
            {
                ViewBag.isNotifroute = true;
                ViewBag.IsSOApplication = false;
            }
            //apply for SO
            if (importFlag)
                Session["UserSearchStringPlanMvmt"] = SearchStrings;
            else
                Session["UserSearchString"] = SearchStrings;
            
            ViewBag.pageSize = pageSize;
            ViewBag.page = pageNumber;
            ViewBag.UserSearchString = SearchStrings;
            ViewBag.IsSimpleN = IsSimpleN;
            ViewBag.ImportFlag = importFlag;
            if (!importFlag)
            {
                ViewBag.isNotifroute = false;
            }

            return View(routeListAsIPagedList);
        }

        #region public ActionResult SaveRouteSearch(string search)
        [AuthorizeUser(Roles = "40000,40001,40002,40003,40004")]
        public ActionResult SaveRouteSearch(string searchString = null, int filterFavouritesRoutes = 0, bool importFlag = false, int? sortOrder = null, int? sortType = null, int page = 0, int pageSize = 0)
        {
            if (searchString == "")
            {
                searchString = null;
            }
            if (importFlag)
                Session["UserSearchStringPlanMvmt"] = searchString;
            else
                Session["UserSearchString"] = searchString;

            return RedirectToAction("RoutePartLibrary", new
            {
                B7vy6imTleYsMr6Nlv7VQ =
                        STP.Web.Helpers.EncryptionUtility.Encrypt("SearchString=" + searchString +
                        "&filterFavouritesRoutes=" + filterFavouritesRoutes +
                        "&importFlag=" + importFlag +
                        "&sortOrder=" + sortOrder +
                        "&sortType=" + sortType +
                        "&page=" + page +
                        "&pageSize=" + pageSize)
            });
        }
        #endregion
        [AuthorizeUser(Roles = "40000,40001,40002,40003,40004")]
        public ActionResult SetMapUsage(int type)
        {
            long val = 0;
            UserInfo SessionInfo = new UserInfo(); ;
            bool ismapsessioncheckflag = true;
            bool isrouteplansessioncheckflag = true;
            bool isroutedisplaysessioncheckflag = true;

            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }

            int organisationId = (int)SessionInfo.OrganisationId;
            int userId = Convert.ToInt32(SessionInfo.UserId);

            if (Session["routedisplayCheckflag"] != null)
            {
                isroutedisplaysessioncheckflag = (bool)Session["routedisplayCheckflag"];
            }

            if (Session["maploadCheckflag"] != null)
            {
                ismapsessioncheckflag = (bool)Session["maploadCheckflag"];
            }
            if (Session["routeplanCheckflag"] != null)
            {
                isrouteplansessioncheckflag = (bool)Session["routeplanCheckflag"];
            }
            if (type == 0 && !ismapsessioncheckflag)
            {
                type = 0;
                Session["maploadCheckflag"] = true;
            }
            else if (type == 1 && !isroutedisplaysessioncheckflag && !isrouteplansessioncheckflag)
            {
                type = 1;
                Session["routedisplayCheckflag"] = true;
            }
            val = routesService.SaveMapUsage(userId, organisationId, type);
            return Json(new { value = val });
        }

        [HttpPost]
        [AuthorizeUser(Roles = "40000,40001,40002,40003,40004")]
        public JsonResult DeleteRoute(long PlannedRouteId)
        {
            UserInfo SessionInfo = new UserInfo();
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }

            int result = routesService.DeleteLibraryRoute(PlannedRouteId, SessionInfo.UserSchema);
            if (result == 1)
            {
                string ErrMsg = string.Empty;
                string sysEventDescp = string.Empty;
                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                movactiontype.UserName = SessionInfo.UserName;
                movactiontype.LibRouteId = (int)PlannedRouteId;
                movactiontype.SystemEventType = SysEventType.Haulier_deleted_library_route;

                sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
            }
            return Json(new { Success = true });
        }

        [AuthorizeUser(Roles = "40000,40001,40002,40003,40004")]
        public ActionResult LibraryRoutePartDetails(string plannedRouteName, long plannedRouteId = 0, string PageFlag = "S", string routeType = "planned", string ApplicationRevId = "0", bool IsTextualRouteType = false, bool IsStartAndEndPointOnly = false, int ShowReturnLeg = 0, string workflowProcess = "", List<AppRouteList> routeLists = null,string flag="false")
        {
            UserInfo SessionInfo = new UserInfo();
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }
            ViewBag.flag = flag;
            Session["plannedRouteId"] = plannedRouteId;
            if (workflowProcess.Length > 0 && WorkflowTaskFinder.FindNextTask(workflowProcess, WorkflowActivityTypes.An_Activity_PlanRouteOnMap, out dynamic workflowPayload) != string.Empty)
            {
                var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                dynamic dataPayload = new ExpandoObject();
                dataPayload.workflowActivityLog = applicationNotificationManagement.SetWorkflowLog(WorkflowActivityTypes.An_Activity_PlanRouteOnMap.ToString());
                WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                {
                    data = dataPayload,
                    workflowData = workflowPayload
                };
                applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);
            }
            if (ApplicationRevId == "0")
            {
                ViewBag.Origin = "Lib";
                Session["RouteFlag"] = null;
                Session["IsLibrary"] = true;//session checks for indicating library route
            }
            else
            {
                ViewBag.Origin = "App";
                Session["ApplicationRevId"] = ApplicationRevId;
                Session["IsLibrary"] = false;
            }
            if (Session["RouteFlag"] == null)
                Session["ApplicationRevId"] = null;
            try
            {
                if (routeType == "outline")
                {
                    RoutePart rp;
                    if (ApplicationRevId == "0")
                        rp = plannedRouteId > 0 ? routesService.GetLibraryRoute(plannedRouteId, SessionInfo.UserSchema) : new RoutePart();
                    else // This is for so app and VR1 app , Notification.
                    {
                        if (Session["RouteFlag"].ToString() == "4")
                            rp = plannedRouteId > 0 ? routesService.GetCandidateOutlineRoute(plannedRouteId, UserSchema.Sort) : new RoutePart();
                        else if (Session["RouteFlag"].ToString() == "2")
                            rp = plannedRouteId > 0 ? routesService.GetApplicationRoutePartGeometry(plannedRouteId, SessionInfo.UserSchema) : new RoutePart();
                        else
                            rp = plannedRouteId > 0 ? routesService.GetApplicationRoutePartGeometry((int)plannedRouteId, SessionInfo.UserSchema) : new RoutePart();
                        ViewBag.rp = rp;
                    }
                    if (plannedRouteId == 0)
                    {
                        rp.RoutePartDetails.RouteId = 0;
                        RoutePath routeP = new RoutePath();
                        routeP.RoutePointList.Add(new RoutePoint());
                        routeP.RoutePointList.Add(new RoutePoint());
                        rp.RoutePathList.Add(routeP);//placeholder for start and end
                    }
                    else
                    {
                        rp.RoutePartDetails.RouteId = plannedRouteId;
                        if (isRoutePresentable(rp.RoutePathList[0].RoutePointList))
                        {
                            ViewBag.routeID = plannedRouteId;
                            ViewBag.plannedRouteName = Request.QueryString["plannedRouteName"];
                            ViewBag.PageFlag = PageFlag;
                            ViewBag.Routetype = routeType;
                            TempData["RouteID"] = plannedRouteId;
                            if (PageFlag == "updateOutline")
                                return PartialView("OutlineRouteDetails", rp);
                            else
                                return View();
                        }
                    }
                    if (PageFlag == "updateOutline")
                    {
                        return PartialView("OutlineRouteDetails", null);
                    }

                    ViewBag.IsTextualRouteType = IsTextualRouteType;
                    ViewBag.IsStartAndEndPointOnly = IsStartAndEndPointOnly;
                    return PartialView("OutlineRouteDetails", rp);
                }
                if (plannedRouteId == 0)
                {
                    ViewBag.IsNewRoute = true;
                }
                else
                {
                    ViewBag.IsNewRoute = false;
                }
                ViewBag.routeID = plannedRouteId;
                ViewBag.plannedRouteName = Request.QueryString["plannedRouteName"];
                ViewBag.PageFlag = PageFlag;
                TempData["RouteID"] = plannedRouteId;
                ViewBag.ShowReturnLeg = ShowReturnLeg;
                if (plannedRouteId == 0)
                {
                    RouteUpdateFlagSessionClear();
                }
                ViewBag.routeList = routeLists;
                return View();
            }
            catch (Exception)
            {
                return View();
            }
        }
        bool isRoutePresentable(List<RoutePoint> routePtList)
        {
            foreach (RoutePoint routePt in routePtList)
            {
                if (routePt.PointGeom == null)
                    return false;
            }

            return true;
        }
        public void RouteUpdateFlagSessionClear()
        {
            Session["URouteID"] = "";
            Session["URouteName"] = "";
            Session["URouteDesc"] = "";
            Session["UFlag"] = "";
        }

        [AuthorizeUser(Roles = "40000,40001,40002,40003,40004")]
        public ActionResult SaveRoute(RoutePart plannedRoutePath1, string PlannedRouteId, long? orgID, long RouteFlag = 0, string VR1ContentRefNo = "", int VR1VersionId = 0, long RouteRevisionId = 0, int Dock_Flag = 0, int NotificationId = 0, int isSimple = 0, bool IsNEN = false, bool IsAutoPlan = false, bool isNENReturnRoute = false, bool IsReturnRoute = false, bool IsAddToLibrary = false, bool IsNENApi = false)
        {
            long NENRturnRouteID = 0, NENMainRouteID = 0;
            var isBroken = plannedRoutePath1.Esdal2Broken;
            var IsRePlanned = plannedRoutePath1.Esdal2Broken;  // set 1 for if the route is replanned
            long OldRtId = Convert.ToInt64(plannedRoutePath1.RoutePartDetails.RouteId);
            long versionId = 0;
            long appRevId = 0;
            string contRefNum = null;
            int dockFlag = 0;
            if (plannedRoutePath1.RoutePartDetails.DockCaution == true)
            {
                dockFlag = 1;
                Dock_Flag = 1;
            }
            long routeRevisonId = 0;
            long routeId = 0;
            string sysEventDescp;
            MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
            string ErrMsg;
            if (IsRePlanned != 1) //check added for if the route is not replanned then identify the existing route is broken if yes then set isbroken field is 1 for new route in routepoints table
            {
                if (RouteFlag == 0) //checking library route
                {
                    //_ = CheckandupdateBrokenRt(OldRtId, "PLANNED", 1);
                }
                else
                {
                    // _ = CheckandupdateBrokenRt(OldRtId, UserSchema.Portal, 1);
                }
            }
            try
            {
                bool res = false;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Route/SaveRoute actionResult method started successfully , Input Parameters for Saving/Editing Route PlannedRouteId: {0},orgID: {1},RouteFlag: {2},VR1ContentRefNo: {3},VR1VersionId: {4},RouteRevisionId: {5},Dock_Flag: {6},NotificationId: {7}", PlannedRouteId, RouteFlag, RouteFlag, VR1ContentRefNo, VR1VersionId, RouteRevisionId, Dock_Flag, NotificationId));
                var sessionValues = (UserInfo)Session["UserInfo"];
                orgID = sessionValues.OrganisationId;
                plannedRoutePath1.UserId = Convert.ToInt32(sessionValues.UserId);//setting user Id
                if (sessionValues.UserTypeId == 696008)
                {
                    plannedRoutePath1.OrgId = (int)Session["SORTOrgID"];
                }
                else
                {
                    plannedRoutePath1.OrgId = Convert.ToInt32(orgID);
                }
                Session["RouteSavingError"] = "";
                if (Session["RouteFlag"] != null)
                {
                    RouteFlag = Convert.ToInt64(Session["RouteFlag"]);
                }
                long NENPre_RouteId = Convert.ToInt32(PlannedRouteId);

                long Route_user = nenNotificationService.GetNENRouteID(Convert.ToInt32(Session["NEN_ID"]), Convert.ToInt32(Session["NENINBOX_ITEM_ID"]), Convert.ToInt32(sessionValues.OrganisationId), 'U');
                int IsUsing = nenNotificationService.VerifyRouteIdWithOtherOrg(Convert.ToInt32(Session["NEN_ID"]), Convert.ToInt32(sessionValues.OrganisationId), Convert.ToInt32(PlannedRouteId));

                if (IsNEN && IsUsing != 0) //common nen route for all users
                {
                    //if IsUsing is not 0 and is equal to the session user id  then new route id should be created since the first user is editing a common route .
                    if (Route_user == Convert.ToInt32(sessionValues.UserId) && !IsAutoPlan)
                        PlannedRouteId = "0";
                    else if (IsAutoPlan)//if the route is getting planned automatically and the route points start or end address has been updated by QAS then a new route has to be saved.
                        PlannedRouteId = "0";
                }

                int listCount = 0;
                for (int i = 0; i < plannedRoutePath1.RoutePathList.Count; i++)
                {
                    listCount = plannedRoutePath1.RoutePathList[i].RouteSegmentList.Count;
                    if (listCount > 0)
                        break;
                }
                if (PlannedRouteId == "0" && isSimple == 1)
                {
                    if (Session["ApplicationRevId"] != null)
                    {
                        contRefNum = Session["ApplicationRevId"].ToString();
                        routeId = routesService.GetRoutePartId(contRefNum, sessionValues.UserSchema);
                        PlannedRouteId = routeId.ToString();
                        if (plannedRoutePath1.RoutePartDetails.RouteName == "NotifReturnRoutePart")
                            PlannedRouteId = "0";
                    }
                    else
                        PlannedRouteId = "0";
                }

                #region Edit Route
                if (PlannedRouteId != "0")
                {
                    plannedRoutePath1.RoutePartDetails.RouteId = Convert.ToInt64(PlannedRouteId);

                    #region SO Application
                    if (RouteFlag == 2)// RouteFlag 2 for SO application
                    {
                        if (Session["ApplicationRevId"] != null)
                            appRevId = Convert.ToInt64(Session["ApplicationRevId"]);
                        TempData["Success"] = "2";
                        if (listCount > 0)
                            plannedRoutePath1.RoutePartDetails.RouteType = "planned";
                        else
                            versionId = 1;
                        routeId = routesService.UpdateApplicationRoute(plannedRoutePath1, versionId, appRevId, contRefNum, dockFlag, routeRevisonId, sessionValues.UserSchema);
                        if (routeId != 0)
                        {
                            plannedRoutePath1.RoutePartDetails.RouteId = routeId;
                            res = true;
                        }
                        #region System Event Log - Haulier_edited_route_for_so_application
                        movactiontype.UserName = sessionValues.UserName;
                        movactiontype.RouteId = (int)plannedRoutePath1.RoutePartDetails.RouteId;
                        movactiontype.RevisionId = (int)appRevId;
                        if (sessionValues.UserSchema == UserSchema.Portal)
                        {
                            movactiontype.SystemEventType = SysEventType.Haulier_edited_route_for_so_application;
                        }
                        else if (sessionValues.UserSchema == UserSchema.Sort)
                        {
                            movactiontype.SystemEventType = SysEventType.Sort_edited_route_for_so_application;
                        }
                        #endregion
                    }
                    #endregion

                    #region VR1 Application
                    else if (RouteFlag == 1)// RouteFlag 1 for VR1 application 
                    {
                        if (Session["ApplicationRevId"] != null)
                            appRevId = Convert.ToInt64(Session["ApplicationRevId"]);
                        if (listCount > 0)
                            plannedRoutePath1.RoutePartDetails.RouteType = "planned";
                        routeId = routesService.UpdateApplicationRoute(plannedRoutePath1, versionId, appRevId, contRefNum, dockFlag, routeRevisonId, sessionValues.UserSchema);
                        if (routeId != 0)
                        {
                            plannedRoutePath1.RoutePartDetails.RouteId = routeId;
                            res = true;
                        }
                        TempData["Success"] = "2";

                        #region System Event Log - Haulier_edited_route_for_vr1_application
                        movactiontype.UserName = sessionValues.UserName;
                        movactiontype.RouteId = (int)plannedRoutePath1.RoutePartDetails.RouteId;
                        movactiontype.RevisionId = (int)appRevId;
                        if (sessionValues.UserSchema == UserSchema.Portal)
                        {
                            movactiontype.SystemEventType = SysEventType.Haulier_edited_route_for_vr1_application;
                        }
                        else if (sessionValues.UserSchema == UserSchema.Sort)
                        {
                            movactiontype.SystemEventType = SysEventType.Sort_edited_route_for_vr1_application;
                        }
                        #endregion
                    }
                    #endregion

                    #region Notification
                    else if (RouteFlag == 3 || IsNEN)// RouteFlag  3 for Notification.
                    {
                        if (Session["ApplicationRevId"] != null)
                            contRefNum = Session["ApplicationRevId"].ToString();
                        if (listCount > 0)
                            plannedRoutePath1.RoutePartDetails.RouteType = "planned";
                        routeId = routesService.UpdateApplicationRoute(plannedRoutePath1, versionId, appRevId, contRefNum, dockFlag, routeRevisonId, sessionValues.UserSchema);
                        if (routeId != 0)
                        {
                            plannedRoutePath1.RoutePartDetails.RouteId = routeId;
                            res = true;
                        }
                        TempData["Success"] = "2";
                        Session["NotiplannedRouteId"] = PlannedRouteId;
                        if (IsNEN)
                        {
                            int IOrgID = Convert.ToInt32(sessionValues.OrganisationId);
                            NENRturnRouteID = nenNotificationService.GetNENReturnRouteID(Convert.ToInt32(Session["NENINBOX_ITEM_ID"]), IOrgID); //added as part of Non esdal release 2 db changes.
                            NENMainRouteID = nenNotificationService.GetNENRouteID(Convert.ToInt32(Session["NEN_ID"]), Convert.ToInt32(Session["NENINBOX_ITEM_ID"]), IOrgID, 'R'); //added additional parameter as part of non esdal release 2 db changes

                            if (IsAutoPlan)
                            {
                                #region NEN auodit logs
                                if (NENPre_RouteId == NENRturnRouteID)
                                    SaveAuditLogs(AuditActionType.soauser_edit_nen_route, "NEN Route (Return)");
                                else
                                    SaveAuditLogs(AuditActionType.soauser_edit_nen_route, plannedRoutePath1.RoutePartDetails.RouteName);
                                #endregion
                            }
                        }
                        if (IsNENApi)
                            SaveAuditLogs(AuditActionType.soauser_edit_nen_route, plannedRoutePath1.RoutePartDetails.RouteName);

                        #region System Event Log - Haulier_edited_route_for_notification
                        if (NotificationId != 0)
                        {
                            movactiontype.UserName = sessionValues.UserName;
                            if (isSimple == 1)
                            {
                                if (plannedRoutePath1.RoutePartDetails.RoutePartNo == 2)
                                {
                                    movactiontype.ReturnRouteId = (int)plannedRoutePath1.RoutePartDetails.RouteId;
                                }
                                else
                                {
                                    movactiontype.RouteId = (int)plannedRoutePath1.RoutePartDetails.RouteId;
                                }
                            }
                            else
                            {
                                movactiontype.RouteId = (int)plannedRoutePath1.RoutePartDetails.RouteId;
                            }
                            movactiontype.ContentRefNo = VR1ContentRefNo;
                            movactiontype.NotificationID = NotificationId;
                            movactiontype.SystemEventType = SysEventType.Haulier_edited_route;
                        }
                        #endregion
                    }
                    #endregion

                    #region Candidate Route
                    else if (RouteFlag == 4)
                    {
                        if (Session["ApplicationRevId"] != null)
                            appRevId = Convert.ToInt64(Session["ApplicationRevId"]);
                        if (listCount > 0)
                        {
                            plannedRoutePath1.RoutePartDetails.RouteType = "planned";
                            routeId = routesService.UpdateApplicationRoute(plannedRoutePath1, versionId, appRevId, contRefNum, dockFlag, routeRevisonId, sessionValues.UserSchema);
                            if (routeId != 0)
                            {
                                plannedRoutePath1.RoutePartDetails.RouteId = routeId;
                                res = true;
                            }
                            //Clear the route assessments.
                            if (res && RouteRevisionId != 0)
                                routeAssessmentService.ClearRouteAssessment(RouteRevisionId, sessionValues.UserSchema);
                        }
                        #region System Event Log - Sort Edited Candidate Route
                        movactiontype.UserName = sessionValues.UserName;
                        movactiontype.RouteId = (int)plannedRoutePath1.RoutePartDetails.RouteId;
                        movactiontype.RevisionId = (int)appRevId;
                        movactiontype.SystemEventType = SysEventType.save_sort_new_candidate_route;
                        #endregion

                        TempData["Success"] = "2";
                    }
                    #endregion

                    #region Library Route
                    else
                    {
                        routeId = Convert.ToInt64(PlannedRouteId);

                        routeId = routesService.UpdateLibraryRoute(plannedRoutePath1, sessionValues.UserSchema);

                        if (routeId != 0)
                        {
                            plannedRoutePath1.RoutePartDetails.RouteId = routeId;
                            res = true;
                        }

                        #region System Event Log - Haulier_edited_libray_route
                        if (res)
                        {
                            movactiontype.UserName = sessionValues.UserName;
                            movactiontype.LibRouteId = (int)plannedRoutePath1.RoutePartDetails.RouteId;
                            movactiontype.SystemEventType = SysEventType.Haulier_edited_library_route;
                        }
                        #endregion

                        TempData["Success"] = "2";
                    }
                    #endregion

                    if (res)
                    {
                        Session["NotiplannedRouteId"] = PlannedRouteId;
                    }

                    sysEventDescp = System_Events.GetSysEventString(sessionValues, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(sessionValues.UserId), sessionValues.UserSchema);
                }

                #endregion

                #region Create Route
                else
                {
                    //#region SO Application
                    if (RouteFlag == 2)
                    {
                        if (Session["ApplicationRevId"] != null)
                            appRevId = Convert.ToInt64(Session["ApplicationRevId"].ToString());
                        if (listCount > 0)
                            plannedRoutePath1.RoutePartDetails.RouteType = "planned";
                        routeId = routesService.SaveApplicationRoute(plannedRoutePath1, versionId, appRevId, contRefNum, dockFlag, routeRevisonId, sessionValues.UserSchema, IsReturnRoute);
                        if (routeId != 0)
                        {
                            plannedRoutePath1.RoutePartDetails.RouteId = routeId;
                            res = true;
                        }

                        #region System Event Log - Haulier_created_new_route_by_map_for_so_application
                        movactiontype.UserName = sessionValues.UserName;
                        movactiontype.RouteId = (int)plannedRoutePath1.RoutePartDetails.RouteId;
                        movactiontype.RevisionId = (int)appRevId;
                        if (plannedRoutePath1.RoutePartDetails.RouteType == "planned")
                        {
                            if (sessionValues.UserSchema == UserSchema.Portal)
                                movactiontype.SystemEventType = SysEventType.Haulier_created_new_route_by_map_for_so_application;
                            else if (sessionValues.UserSchema == UserSchema.Sort)
                                movactiontype.SystemEventType = SysEventType.Sort_created_new_route_by_map_for_sort_so_application;
                        }
                        else
                        {
                            if (plannedRoutePath1.RoutePathList[0].RoutePointList[0].PointDescr == null && plannedRoutePath1.RoutePathList[0].RoutePointList[1].PointDescr == null)
                            {
                                if (sessionValues.UserSchema == UserSchema.Portal)
                                    movactiontype.SystemEventType = SysEventType.Haulier_created_new_route_by_textual_description_for_so_application;
                                else if (sessionValues.UserSchema == UserSchema.Sort)
                                    movactiontype.SystemEventType = SysEventType.Sort_created_new_route_by_textual_description_for_so_application;
                            }
                            else
                            {
                                if (sessionValues.UserSchema == UserSchema.Portal)
                                    movactiontype.SystemEventType = SysEventType.Haulier_created_new_route_by_start_and_end_point_for_so_application;
                                else if (sessionValues.UserSchema == UserSchema.Sort)
                                    movactiontype.SystemEventType = SysEventType.Sort_created_new_route_by_start_and_end_point_for_so_application;
                            }
                        }

                        #endregion
                    }


                    #region VR1 and Notification
                    else if (RouteFlag == 1 || RouteFlag == 3)// RouteFlag 1 for VR1 application and 3 for Notification.
                    {
                        if (Session["ApplicationRevId"] != null)
                        {
                            if (RouteFlag == 3)
                                contRefNum = Session["ApplicationRevId"].ToString();
                            else
                            {
                                contRefNum = VR1ContentRefNo;
                                appRevId = Convert.ToInt32(Session["ApplicationRevId"]);
                            }
                        }
                        if (listCount > 0)
                        {
                            plannedRoutePath1.RoutePartDetails.RouteType = "planned";
                            versionId = VR1VersionId;
                            if (contRefNum == null && appRevId == 0)
                                versionId = 0;
                            routeId = routesService.SaveApplicationRoute(plannedRoutePath1, versionId, appRevId, contRefNum, dockFlag, routeRevisonId, sessionValues.UserSchema, IsReturnRoute);
                            if (routeId != 0)
                            {
                                plannedRoutePath1.RoutePartDetails.RouteId = routeId;
                                res = true;
                            }
                            Session["NotiplannedRouteId"] = plannedRoutePath1.RoutePartDetails.RouteId;
                            ViewBag.NotiplannedRouteId = plannedRoutePath1.RoutePartDetails.RouteId;
                            if (plannedRoutePath1.RoutePartDetails.RouteId != 0)
                            {
                                if (IsNEN)
                                {
                                    bool result = nenNotificationService.InsertInboxEditRouteForNewUser(Convert.ToInt32(Session["NENINBOX_ITEM_ID"]), Convert.ToInt32(Session["NEN_ID"]), Convert.ToInt32(Session["NOTIF_ID"]),
                                    Convert.ToInt32(sessionValues.UserId), NENPre_RouteId, routeId, (long)orgID);
                                    int IOrgID = Convert.ToInt32(sessionValues.OrganisationId);
                                    NENRturnRouteID = nenNotificationService.GetNENReturnRouteID(Convert.ToInt32(Session["NENINBOX_ITEM_ID"]), IOrgID); //added as part of Non esdal release 2 db changes.
                                    NENMainRouteID = nenNotificationService.GetNENRouteID(Convert.ToInt32(Session["NEN_ID"]), Convert.ToInt32(Session["NENINBOX_ITEM_ID"]), IOrgID, 'R'); //added additional parameter as part of non esdal release 2 db changes
                                    if (IsAutoPlan)
                                    {
                                        #region NEN auodit logs
                                        if (NENPre_RouteId == NENRturnRouteID)
                                            SaveAuditLogs(AuditActionType.soauser_edit_nen_route, "NEN Route (Return)");
                                        else
                                            SaveAuditLogs(AuditActionType.soauser_edit_nen_route, plannedRoutePath1.RoutePartDetails.RouteName);
                                        #endregion
                                    }
                                }
                            }
                        }
                        else
                        {
                            routeId = routesService.SaveApplicationRoute(plannedRoutePath1, versionId, appRevId, contRefNum, dockFlag, routeRevisonId, sessionValues.UserSchema, IsReturnRoute);
                            if (routeId != 0)
                            {
                                plannedRoutePath1.RoutePartDetails.RouteId = routeId;
                                res = true;
                            }
                        }

                        #region System Event Log - Haulier_created_new_route_by_map_for_vr1_application
                        if (RouteFlag == 1)
                        {

                            movactiontype.UserName = sessionValues.UserName;
                            movactiontype.RouteId = (int)plannedRoutePath1.RoutePartDetails.RouteId;
                            movactiontype.RevisionId = (int)appRevId;
                            if (sessionValues.UserSchema == UserSchema.Portal)
                            {
                                movactiontype.SystemEventType = SysEventType.Haulier_created_new_route_by_map_for_vr1_application;
                            }
                            else if (sessionValues.UserSchema == UserSchema.Sort)
                            {
                                movactiontype.SystemEventType = SysEventType.Sort_created_a_new_route_by_map_for_vr1_application;
                            }

                        }
                        #endregion

                        #region System Event Log - Haulier_created_new_route_for_notification
                        else if (RouteFlag == 3 && NotificationId != 0)
                        {

                            movactiontype.UserName = sessionValues.UserName;
                            movactiontype.RouteId = (int)plannedRoutePath1.RoutePartDetails.RouteId;
                            movactiontype.ContentRefNo = contRefNum;
                            movactiontype.NotificationID = NotificationId;
                            movactiontype.SystemEventType = SysEventType.Haulier_created_new_route;
                        }
                        #endregion
                    }
                    #endregion

                    #region Candidate Route
                    else if (RouteFlag == 4)//Creation for candidate route
                    {
                        Session["RouteAssessmentFlag"] = "";
                        res = false;
                        if (listCount > 0)
                        {
                            plannedRoutePath1.RoutePartDetails.RouteType = "planned";
                            routeId = routesService.SaveApplicationRoute(plannedRoutePath1, 0, 0, null, Dock_Flag, RouteRevisionId, sessionValues.UserSchema, IsReturnRoute);
                            if (routeId != 0)
                            {
                                plannedRoutePath1.RoutePartDetails.RouteId = routeId;
                                res = true;
                                Session["RouteAssessmentFlag"] = "Completed";
                            }
                            Session["NotiplannedRouteId"] = plannedRoutePath1.RoutePartDetails.RouteId;
                            ViewBag.NotiplannedRouteId = plannedRoutePath1.RoutePartDetails.RouteId;
                            //Clear the route assessments.
                            if (res)
                            {
                                bool routestatus = routeAssessmentService.ClearRouteAssessment(RouteRevisionId, sessionValues.UserSchema);
                            }
                        }
                        #region System Event Log - Sort Edited Candidate Route
                        if (res)
                        {
                            movactiontype.UserName = sessionValues.UserName;
                            movactiontype.RouteId = (int)plannedRoutePath1.RoutePartDetails.RouteId;
                            movactiontype.RevisionId = (int)appRevId;
                            movactiontype.SystemEventType = SysEventType.bind_sort_candidate_route_revisions;
                        }
                        #endregion
                    }
                    #endregion

                    #region Library Route
                    else // This is for Route Library
                    {
                        routeId = routesService.SaveLibraryRoute(plannedRoutePath1, UserSchema.Portal);
                        if (routeId != 0)
                        {
                            plannedRoutePath1.RoutePartDetails.RouteId = routeId;
                            res = true;
                        }
                        if (res)
                        {
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"Library Route created completed successfully ,New RouteId:" + routeId);
                            #region System Event Log - Haulier_Edited_library_route
                            movactiontype.UserName = sessionValues.UserName;
                            movactiontype.LibRouteId = (int)plannedRoutePath1.RoutePartDetails.RouteId;
                            movactiontype.SystemEventType = SysEventType.Haulier_edited_library_route;
                            #endregion
                        }
                    }
                    #endregion


                    sysEventDescp = System_Events.GetSysEventString(sessionValues, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(sessionValues.UserId), sessionValues.UserSchema);

                }
                #endregion

                #region For handling various broken route scenarios most of the sections are commented becaues all the unplanned broken routes are now blocked on the UI itself
                //if ((routeId > 0 && RouteFlag != 0) && isBroken == 1)
                //{
                //    routeId = Convert.ToInt64(PlannedRouteId);
                //    if (plannedRoutePath1.IsAutoReplan == 1 && IsRePlanned == 1)
                //        RouteManagerProvider.Instance.SetVerificationStatus((int)routeId, 0, 921002, sessionValues.userSchema);
                //    else if (plannedRoutePath1.IsAutoReplan == 0 && IsRePlanned == 1)
                //        RouteManagerProvider.Instance.SetVerificationStatus((int)routeId, 0, 921004, sessionValues.userSchema);
                //}
                #endregion

                if (IsAddToLibrary)
                    AddRouteToLibrary(routeId, plannedRoutePath1.RoutePartDetails.RouteType, "");

                return Json(new { value = routeId, routeSaveRes = res, VNENRturnRouteID = NENRturnRouteID, VNENMainRouteID = NENMainRouteID });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/SaveRoute, Exception:" + ex);
                return RedirectToAction(ViewBag.SavedRouteID);
            }
        }

        [AuthorizeUser(Roles = "40000,40001,40002,40003,40004")]
        public ActionResult SaveCompressedRoute(string compressedRoutePart, string PlannedRouteId, long? orgID, long RouteFlag = 0, string VR1ContentRefNo = "", int VR1VersionId = 0, long RouteRevisionId = 0, int Dock_Flag = 0, int NotificationId = 0, int isSimple = 0, bool IsNEN = false, bool IsAutoPlan = false, bool isNENReturnRoute = false, bool IsReturnRoute = false, bool IsAddToLibrary = false, bool IsNENApi = false)
        {
            RoutePart routePart = null;

            // Decode the base64 string
            byte[] compressedBytes = Convert.FromBase64String(compressedRoutePart);

            // Decompress the data
            using (MemoryStream compressedStream = new MemoryStream(compressedBytes))
            {
                using (MemoryStream decompressedStream = new MemoryStream())
                {
                    using (GZipStream decompressionStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedStream);
                    }

                    // Convert the decompressed data to a string
                    string jsonData = Encoding.UTF8.GetString(decompressedStream.ToArray());
                    
                    // Deserialize the JSON data into the RoutePart object
                    try
                    {
                        var routeData = JsonConvert.DeserializeObject<RoutePartMapper>(jsonData);
                        routePart = routeData.RoutePart;

                        return SaveRoute(routePart, PlannedRouteId, orgID, RouteFlag, VR1ContentRefNo, 
                            VR1VersionId, RouteRevisionId, Dock_Flag, NotificationId, isSimple, IsNEN, 
                            IsAutoPlan, isNENReturnRoute, IsReturnRoute, IsAddToLibrary,IsNENApi);
                    }
                    catch(Exception ex)
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/SaveRoute, Exception:" + ex);
                        return RedirectToAction(ViewBag.SavedRouteID);
                    }
                }
            }
        }

        [HttpPost]
        [AuthorizeUser(Roles = "40000,40001,40002,40003,40004")]
        public JsonResult GetPlannedRoute(long RouteID, string routeType = "planned", bool IsSortPortal = false, bool IsNEN = false, bool IsfromImp = false, bool IsPlanMovement = false, bool IsCandidateView = false, bool IsAuthorizeMovementGeneral = false,int IsHistoric=0)
        {
            UserInfo SessionInfo = new UserInfo();
            if (Session["UserInfo"] != null)
                SessionInfo = (UserInfo)Session["UserInfo"];
            if (Session["OnBackIsSORTFlag"] != null)
            {
                if ((bool)Session["OnBackIsSORTFlag"])
                    IsSortPortal = true;
                Session["OnBackIsSORTFlag"] = null;
            }

            string UserSchemaVal = IsSortPortal? UserSchema.Sort: SessionInfo.UserSchema;

            ViewBag.plannedRouteName = Request.QueryString["plannedRouteName"];
            RoutePart plannedRoutePath = null;
            if (IsAuthorizeMovementGeneral)
            {
                if (IsHistoric == 0)
                {
                    plannedRoutePath = routesService.GetApplicationRoute(RouteID, UserSchemaVal);
                }
                else
                {
                    plannedRoutePath= routesService.GetHistoricAppRoute(RouteID, UserSchemaVal);
                }
            }
            else if (IsPlanMovement || IsNEN)
            {
                if (Session["RouteFlag"] != null && Session["RouteFlag"].ToString() == "4")
                {
                    plannedRoutePath = routesService.GetApplicationRoute(RouteID, SessionInfo.UserSchema);
                    if (plannedRoutePath == null)
                        plannedRoutePath = routesService.GetCandidateOutlineRoute(RouteID, UserSchema.Sort);
                }
                else if (routeType == "outline")
                {
                    plannedRoutePath = routesService.GetApplicationRoutePartGeometry(RouteID, SessionInfo.UserSchema);
                    if (plannedRoutePath != null)
                        plannedRoutePath.RoutePartDetails.RouteType = "outline";
                }
                else if ((Session["RouteFlag"] != null && routeType == "planned") || IsNEN)
                {
                    if (IsSortPortal) //proposal,repropsal,agreed route
                        plannedRoutePath = routesService.GetApplicationRoute(RouteID, UserSchema.Sort);
                    else
                    {
                        if (IsfromImp)
                            plannedRoutePath = routesService.GetLibraryRoute(RouteID, SessionInfo.UserSchema);
                        else
                            plannedRoutePath = routesService.GetApplicationRoute(RouteID, SessionInfo.UserSchema);
                    }
                    if (plannedRoutePath != null)
                        plannedRoutePath.RoutePartDetails.RouteType = "planned";
                }
            }
            else if(IsCandidateView)
            {
                plannedRoutePath = routesService.GetApplicationRoute(RouteID, SessionInfo.UserSchema);
            }
            else
            {
                plannedRoutePath = routesService.GetLibraryRoute(RouteID, SessionInfo.UserSchema);
            }
            if (RouteID != 0 && plannedRoutePath != null)
            {
                Session["URouteID"] = plannedRoutePath.RoutePartDetails.RouteId;
                Session["URouteName"] = plannedRoutePath.RoutePartDetails.RouteName;
                Session["URouteDesc"] = plannedRoutePath.RoutePartDetails.RouteDescr;
                Session["UDockCaution"] = plannedRoutePath.RoutePartDetails.DockCaution;
                Session["UFlag"] = "U";
            }
           
            try
            {
                string rson = null;
                if (plannedRoutePath.RoutePartDetails.RouteId > 0)
                {
                    rson = JsonConvert.SerializeObject(plannedRoutePath);
                    RoutePartJson routePartJson1 = JsonConvert.DeserializeObject<RoutePartJson>(rson);
                    return Json(new { result = routePartJson1 });
                }
                else
                {
                    return Json(new { result = (string)null });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;

        }
        public RoutePartJson DoProcessRoutepart(RoutePart routePart)
        {
            RoutePartJson processedRoute = new RoutePartJson();

            try
            {
                processedRoute = JsonConvert.DeserializeObject<RoutePartJson>(JsonConvert.SerializeObject(routePart));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return processedRoute;

        }

        [AuthorizeUser(Roles = "40000,40001,40002,40003,40004")]
        public ActionResult enterRouteDetail(string mode = "S", string routeFlag = "0", string origin = " ", string IsNotification = "false", int routeid = 0)
        {
            if (Session["RouteFlag"] != null)
                routeFlag = Convert.ToString(Session["RouteFlag"]);

            if (mode == "U")
            {
                ViewBag.mode = "Edit";
            }
            else
            {
                ViewBag.mode = "Save";
            }
            bool Identifyreturnflag = false;
            if (routeid > 0)
            {
                var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                PlanMvmntPayLoad PlanMovementPayload = applicationNotificationManagement.GetPlanMvmtPayload();
                ReturnRouteMapping returnRouteRemove = null;
                if (PlanMovementPayload.ReturnRouteMappings.Count > 0)
                {
                    returnRouteRemove = PlanMovementPayload.ReturnRouteMappings.FirstOrDefault(r => r.MainRouteId == routeid || r.ReturnRouteId == routeid);
                    if (returnRouteRemove != null)
                    {
                        Identifyreturnflag = true;
                    }
                }
            }

            ViewBag.IsNotification = bool.Parse(IsNotification);
                ViewBag.ReturnRouteExist = Identifyreturnflag;

            ViewBag.RouteFLag = routeFlag;
            ViewBag.origin = origin;

            return PartialView("enterRouteDetail");
        }
        [HttpPost]
        public ActionResult ValidateApplicationRouteName(string ROUTE_NAME, int REVISION_ID = 0, string CONTENT_REF_NO = null, int ROUTE_FOR = 0)
        {
            var sessionValues = (UserInfo)Session["UserInfo"];
            int result = routesService.VerifyApplicatiponRouteNameValidation(ROUTE_NAME, REVISION_ID, CONTENT_REF_NO, ROUTE_FOR, sessionValues.UserSchema);
            return Json(result);
        }

        [HttpPost]
        public JsonResult CheckRouteName(string RouteName)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            int organisationId = (int)SessionInfo.OrganisationId;
            if (SessionInfo.UserTypeId == UserType.Sort)
                organisationId = (int)Session["SORTOrgID"];

            int result = routesService.CheckRouteName(RouteName, organisationId, SessionInfo.UserSchema);

            return Json(new { success = result });
        }
        public JsonResult AddRouteToLibrary(long routePartId, string routeType, string routeName)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            int organisationId = (int)SessionInfo.OrganisationId;
            if (SessionInfo.UserTypeId == UserType.Sort)
                organisationId = (int)Session["SORTOrgID"];
            long routeId = 0;
            string workFlowKey = new SessionData().Wf_An_ApplicationWorkflowId;
            PlanMvmntPayLoad mvmntPayLoad = new PlanMvmntPayLoad();
            var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
            if (workFlowKey != string.Empty || workFlowKey != WorkflowActivityConstants.Gn_Failed)
            {
                mvmntPayLoad = applicationNotificationManagement.GetPlanMvmtPayload();
            }
            int isRouteExist = 0;
            if (routeName != "")
                isRouteExist = routesService.CheckRouteName(routeName, organisationId, SessionInfo.UserSchema);
            if (isRouteExist == 0)
            {
                routeId = routesService.AddRouteToLibrary(routePartId, organisationId, routeType, SessionInfo.UserSchema);
                if (routeId > 0)
                {
                    string sysEventDescp;
                    MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                    movactiontype.UserName = SessionInfo.UserName;
                    movactiontype.RouteId = (int)routeId;
                    if (mvmntPayLoad.MvmntType.MovementType == (int)MovementType.special_order)
                        movactiontype.SystemEventType = SysEventType.Haulier_added_route_to_library_for_so_application;
                    else if (mvmntPayLoad.MvmntType.MovementType == (int)MovementType.vr_1)
                        movactiontype.SystemEventType = SysEventType.Haulier_addd_route_to_library_for_vr1_application;
                    else
                        movactiontype.SystemEventType = SysEventType.Haulier_addd_route_to_library_for_notification;

                    sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out string ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                }
            }

            return Json(new { RouteId = routeId, RouteExist = isRouteExist });
        }

        #region MovementRoute WFANStep4
        public ActionResult MovementRoute(MovementRouteCntrlModel movementRouteCntrlModel)
        {
            List<AppRouteList> routelist;
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
            var planMvmt = applicationNotificationManagement.GetPlanMvmtPayload();
            VehicleMovementType movementTypeClass = planMvmt.MvmntType;
            List<AppVehicleConfigList> vehicleList;
            if ((movementRouteCntrlModel.contRefNum == null || movementRouteCntrlModel.contRefNum == "") && movementRouteCntrlModel.apprevisionId == 0 && movementRouteCntrlModel.versionId == 0)
            {
                movementRouteCntrlModel.contRefNum = planMvmt.ContenRefNo;
                movementRouteCntrlModel.versionId = movementTypeClass.MovementType == (int)MovementType.vr_1 ? planMvmt.VersionId : 0;
                movementRouteCntrlModel.apprevisionId = planMvmt.RevisionId;
            }
            movementRouteCntrlModel.versionId = movementTypeClass.MovementType == (int)MovementType.vr_1 ? movementRouteCntrlModel.versionId : 0;
            routelist = movementTypeClass.MovementType == (int)MovementType.special_order
                ? routesService.GetSoAppRouteList(movementRouteCntrlModel.apprevisionId, SessionInfo.UserSchema)
                : routesService.NotifVR1RouteList(movementRouteCntrlModel.apprevisionId, movementRouteCntrlModel.contRefNum, movementRouteCntrlModel.versionId, SessionInfo.UserSchema);

            if (planMvmt.IsApp)
            {
                if (planMvmt.IsVr1App)//VR1        
                {
                    vehicleList = vehicleconfigService.AppVehicleConfigListVR1(0, planMvmt.VersionId, null, SessionInfo.UserSchema);
                    Session["pageflag"] = "1";
                    Session["RouteFlag"] = "1";
                    Session["AppFlag"] = "VR1App";
                }
                else //SO
                {
                    vehicleList = vehicleconfigService.AppVehicleConfigList(planMvmt.RevisionId, SessionInfo.UserSchema);
                    Session["pageflag"] = "2";
                    Session["RouteFlag"] = "2";
                    Session["AppFlag"] = "SOApp";
                }
                Session["IsRoute"] = true;
            }
            else //NOTIF
            {
                vehicleList = vehicleconfigService.AppVehicleConfigListVR1(0, 0, planMvmt.ContenRefNo, SessionInfo.UserSchema);
                Session["pageflag"] = "3";
                Session["RouteFlag"] = "3";
                Session["AppFlag"] = "Notif";
                Session["IsNotif"] = true;
            }
            if (planMvmt.ReturnRouteMappings.Count > 0 && movementTypeClass.MovementType == (int)MovementType.notification)
            {
                foreach (var (item, itemMap) in routelist.SelectMany(item => planMvmt.ReturnRouteMappings.Select(itemMap => (item, itemMap))))
                {
                    if (itemMap.MainRouteId == item.RouteID)//this route have return leg
                        item.HasReturnRoute = true;
                    else if (itemMap.ReturnRouteId == item.RouteID)//this route is a retun leg
                        item.IsReturnRoute = true;
                }
            }
            ViewBag.AutoAssignRouteVehicle = true;
            vehicleList = vehicleList.Where(x => x.ParentVehicleId == 0).ToList();
            if (routelist.Count > 1 && vehicleList.Count > 1)
                ViewBag.AutoAssignRouteVehicle = false;

            if (planMvmt.VehicleAssignmentList != null && planMvmt.VehicleAssignmentList.Select(x => x.RoutePartId).Count() != routelist.Count)
                planMvmt.IsRouteVehicleAssigned = false;

            if (WorkflowTaskFinder.FindNextTask("HaulierApplication", WorkflowActivityTypes.An_Activity_PlanRouteOnMap, out dynamic workflowPayload) != string.Empty)
            {
                dynamic dataPayload = new ExpandoObject();
                dataPayload.routelist = routelist;
                dataPayload.taskId = 4;
                planMvmt.ActionCompleted = planMvmt.ActionCompleted <= dataPayload.taskId ? dataPayload.taskId : planMvmt.ActionCompleted;
                planMvmt.IsRouteSummaryPage = movementRouteCntrlModel.IsRouteSummaryPage;
                dataPayload.PlanMvmntPayLoad = planMvmt;
                dataPayload.workflowActivityLog = applicationNotificationManagement.SetWorkflowLog(WorkflowActivityTypes.An_Activity_PlanRouteOnMap.ToString());
                WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                {
                    data = routelist.Count > 0 ? dataPayload : new object(),
                    workflowData = workflowPayload
                };
                applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);
            }

            ViewBag.PlanMvmntPayLoad = planMvmt;
            ViewBag.VehicleList = vehicleList;
            Session[SessionData.Ev_AN_VehicleList] = vehicleList;
            ViewBag.MovementRouteList = routelist;
            ViewBag.IsNotif = movementRouteCntrlModel.isNotif;
            if (planMvmt.IsNotif)
            {
                string assessmentFlag = "";
                if (Session["RouteAssessmentFlag"] != null)
                {
                    assessmentFlag = Session["RouteAssessmentFlag"].ToString();
                }
                ViewBag.AssessmentStatus = assessmentFlag;
            }

            return PartialView("MovementRoute");
        }
        #endregion
        public JsonResult GetFavouriteRoutes()
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            int organisationId = (int)SessionInfo.OrganisationId;
            if (SessionInfo.UserTypeId == UserType.Sort)
                organisationId = (int)Session["SORTOrgID"];
            List<RoutePartDetails> favoriteRoutes = routesService.GetFavouriteRoutes(organisationId, SessionInfo.UserSchema);
            return Json(new { data = favoriteRoutes });
        }

        public JsonResult DeleteApplicationRoute(long routeId, string routeType, bool Iscandi = false, long revisionId = 0)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
            PlanMvmntPayLoad PlanMovementPayload = applicationNotificationManagement.GetPlanMvmtPayload();
            VehicleMovementType movementTypeClass = PlanMovementPayload.MvmntType;
            bool status = false;
            int result = routesService.DeleteApplicationRoute(routeId, routeType, SessionInfo.UserSchema);
            if (result == 1)
            {
                string ErrMsg;
                string sysEventDescp;
                ReturnRouteMapping returnRouteRemove = null;
                VehicleAssignment vehicleAssignment = null;
                if (PlanMovementPayload.ReturnRouteMappings.Count > 0 && movementTypeClass.MovementType == (int)MovementType.notification)
                    returnRouteRemove = PlanMovementPayload.ReturnRouteMappings.Find(r => r.MainRouteId == routeId || r.ReturnRouteId == routeId);
                if (returnRouteRemove != null)
                    PlanMovementPayload.ReturnRouteMappings.Remove(returnRouteRemove);
                if (PlanMovementPayload.VehicleAssignmentList != null && PlanMovementPayload.VehicleAssignmentList.Count > 0)
                    vehicleAssignment = PlanMovementPayload.VehicleAssignmentList.Find(x => x.RoutePartId == routeId);
                if (vehicleAssignment != null)
                    PlanMovementPayload.VehicleAssignmentList.Remove(vehicleAssignment);
                PlanMovementPayload.IsRouteVehicleAssigned = false;
                ModifyWorkfLowPayload(PlanMovementPayload, applicationNotificationManagement);
                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                movactiontype.UserName = SessionInfo.UserName;
                movactiontype.RouteId = (int)routeId;
                if (Iscandi)
                {
                    //Need to impliment the logic for logging the candidate route sysevnevt logic
                    routeAssessmentService.ClearRouteAssessment(revisionId, SessionInfo.UserSchema);
                    movactiontype.SystemEventType = SysEventType.sort_deleted_candidate_route;
                    sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                }
                else
                {
                    if (movementTypeClass.MovementType == (int)MovementType.special_order)
                        movactiontype.SystemEventType = SysEventType.Haulier_deleted_route_for_so_application;
                    else if (movementTypeClass.MovementType == (int)MovementType.vr_1)
                        movactiontype.SystemEventType = SysEventType.Haulier_deleted_route_for_vr1_application;
                    else
                        movactiontype.SystemEventType = SysEventType.Haulier_deleted_route_for_notification;
                    sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                }

                status = true;
            }
            return Json(new { Success = status });
        }

        public ActionResult MovementRoutePlan()
        {
            return PartialView();
        }
        public ActionResult SetRouteDetails(AppRouteList route, bool isAgreedNotif = false, bool isNotif = false, int routeCount = 0)
        {
            ViewBag.IsAgreedNotif = isAgreedNotif;
            ViewBag.IsNotif = isNotif;
            ViewBag.RouteCount = routeCount;
            return PartialView("_RouteDetails", route);
        }
        public ActionResult MovementSelectRouteByImport(MovementSelectRouteByImportCntrlModel movementSelectRouteByImportCntrlModel)
        {
            ViewBag.ImportFrm = movementSelectRouteByImportCntrlModel.importFrm;
            ViewBag.IsFavourite = movementSelectRouteByImportCntrlModel.isFavourite;
            ViewBag.BackToMovementPreviousList = movementSelectRouteByImportCntrlModel.BackToMovementPreviousList;
            var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
            if (applicationNotificationManagement.IsThisMovementExist(movementSelectRouteByImportCntrlModel.NotificationId, movementSelectRouteByImportCntrlModel.ApplicationId, out string workflowKey))
            {
                if (WorkflowTaskFinder.FindNextTask(movementSelectRouteByImportCntrlModel.workflowProcess, movementSelectRouteByImportCntrlModel.importFrm == "library" ? WorkflowActivityTypes.An_Activity_ChooseFromRouteLibrary : WorkflowActivityTypes.An_Activity_ImportFromPreviousovement_Route, out dynamic workflowPayload) != string.Empty)
                {

                    dynamic dataPayload = new ExpandoObject();
                    dataPayload.workflowActivityLog = applicationNotificationManagement.SetWorkflowLog(movementSelectRouteByImportCntrlModel.importFrm == "library" ? WorkflowActivityTypes.An_Activity_ChooseFromRouteLibrary.ToString() : WorkflowActivityTypes.An_Activity_ImportFromPreviousovement_Route.ToString());
                    WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                    {
                        data = dataPayload,
                        workflowData = workflowPayload
                    };
                    applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);
                }
            }
            if (movementSelectRouteByImportCntrlModel.Iscandidiate)//return candidate partial view
            {
                return PartialView("~/Views/SORTApplication/_SORTSelectRouteByImport.cshtml");
            }
            else
            {
                return PartialView("_SelectRouteByImport");
            }

        }

        #region ImportFromLibrary
        public ActionResult ImportRouteFromLibrary(int routepartId, int AppRevId, string routetype, string CONTENT_REF = null, int VersionId = 0, int NotificationId = 0, bool isBackCall = false)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Routes/ImportRouteFromLibrary actionResult method started successfully,input parameters are routepartId: {0},AppRevId: {1},routetype: {2},CONTENT_REF: {3},VersionId: {4},NotificationId: {5}", routepartId, AppRevId, routetype, CONTENT_REF, VersionId, NotificationId));
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                string ErrMsg = string.Empty;
                string sysEventDescp = string.Empty;
                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                PlanMvmntPayLoad mvmntPayLoad = new PlanMvmntPayLoad();
                string workFlowKey = new SessionData().Wf_An_ApplicationWorkflowId;
                var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                if (workFlowKey != string.Empty || workFlowKey != WorkflowActivityConstants.Gn_Failed)
                {
                    mvmntPayLoad = applicationNotificationManagement.GetPlanMvmtPayload();
                }
                int Routetype = 615001;
                if (routetype == "planned")
                    Routetype = 615002;
                long result = 0;
                if (Routetype == 615001)
                {
                    result = routesService.SaveSOAppImportRoute(routepartId, AppRevId, Routetype, SessionInfo.UserSchema);

                    #region System Event Log - Haulier_imported_existing_route_from_library_for_notification
                    movactiontype.SystemEventType = SysEventType.Haulier_imported_existing_route_from_library;
                    #endregion
                }

                else if (Routetype == 615002)
                {
                    if (mvmntPayLoad.MvmntType.MovementType == (int)MovementType.vr_1)
                        AppRevId = 0;
                    result = routesService.ImportRouteFromLibrary(routepartId, VersionId, AppRevId, Routetype, CONTENT_REF, SessionInfo.UserSchema);

                    if (mvmntPayLoad.MvmntType.MovementType == (int)MovementType.special_order)
                    {
                        #region System Event Log - Haulier_chose_existing_route_from_library_for_so_application
                        if (SessionInfo.UserSchema == UserSchema.Portal)
                            movactiontype.SystemEventType = SysEventType.Haulier_imported_existing_route_from_library;
                        else if (SessionInfo.UserSchema == UserSchema.Sort)
                            movactiontype.SystemEventType = SysEventType.sort_chose_existing_route_from_library_for_so_application;
                        #endregion
                    }
                    else
                    {
                        #region System Event Log - Haulier_imported_route_from_library_for_notification_or_vr1_application
                        if (mvmntPayLoad.MvmntType.MovementType == (int)MovementType.notification)
                            movactiontype.SystemEventType = SysEventType.Haulier_imported_route_from_library;
                        else
                        {
                            if (SessionInfo.UserSchema == UserSchema.Portal)
                                movactiontype.SystemEventType = SysEventType.Haulier_chose_existing_route_from_library_for_vr1_application;
                            else if (SessionInfo.UserSchema == UserSchema.Sort)
                                movactiontype.SystemEventType = SysEventType.Sort_chose_existing_route_from_library_for_vr1_application;
                        }
                        #endregion
                    }
                }
                if (result != 0)
                {
                    movactiontype.UserName = SessionInfo.UserName;
                    movactiontype.LibRouteId = routepartId;
                    movactiontype.ContentRefNo = CONTENT_REF;
                    movactiontype.RevisionId = AppRevId;
                    movactiontype.NotificationID = NotificationId;
                    movactiontype.RouteId = (int)result;
                    sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);

                    mvmntPayLoad.IsRouteVehicleAssigned = false;
                    ModifyWorkfLowPayload(mvmntPayLoad, applicationNotificationManagement);
                    TempData["apprevisionId"] = AppRevId;
                }
                Session["UserSearchString"] = null;
                return Json(new { result = result });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Routes/ImportRouteFromLibrary, Exception: {0}", ex));
                return Json(0);
            }
        }
        #endregion

        #region ImportRouteFromPrevious
        public JsonResult ImportRouteFromPrevious(int routepartId, int AppRevId, string routeType, bool VR1route = false, int versionid = 0, string contentref = "", int NotificationId = 0, int SOVersionId = 0, string PrevMovEsdalRefNum = "", int ShowPrevMoveSortRoute = 0)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Routes/ImportRouteFromPrevious actionResult method started successfully,input parameters are routepartId: {0},AppRevId: {1},routeType: {2},VR1route: {3},versionid: {4},contentref: {5},NotificationId: {6}, SOVersionId: {7}, PrevMovEsdalRefNum: {8}", routepartId, AppRevId, routeType, VR1route, versionid, contentref, NotificationId, SOVersionId, PrevMovEsdalRefNum));
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                PlanMvmntPayLoad mvmntPayLoad = new PlanMvmntPayLoad();
                string workFlowKey = new SessionData().Wf_An_ApplicationWorkflowId;
                var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                if (workFlowKey != string.Empty || workFlowKey != WorkflowActivityConstants.Gn_Failed)
                {
                    mvmntPayLoad = applicationNotificationManagement.GetPlanMvmtPayload();
                }
                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                string ErrMsg = string.Empty;
                string sysEventDescp = string.Empty;
                long result = 0;
                string esdalref = "";
                if (ShowPrevMoveSortRoute == 2)
                {
                    Session["RouteFlag"] = 2;
                }
                if (mvmntPayLoad.MvmntType.MovementType == (int)MovementType.special_order)
                {
                    if (routeType == "planned")
                        result = routesService.SaveRouteInRouteParts(routepartId, AppRevId, 0, contentref, SessionInfo.UserSchema);
                    else
                        result = routesService.SaveRouteInAppParts(routepartId, AppRevId, SessionInfo.UserSchema);

                    #region System Event Log - Haulier_imported_route_from_previous_movement_for_so_application
                    if (SessionInfo.UserSchema == UserSchema.Portal)
                    {
                        if (SOVersionId != 0)
                        {
                            //esdalref = ApplicationProvider.Instance.GetEsdalRefNum(SOVersionId);
                        }
                        movactiontype.ESDALRef = esdalref;
                        movactiontype.SystemEventType = SysEventType.Haulier_imported_route_from_previous_movement_for_so_application;
                    }
                    else if (SessionInfo.UserSchema == UserSchema.Sort)
                    {
                        movactiontype.ESDALRef = PrevMovEsdalRefNum;
                        movactiontype.SystemEventType = SysEventType.Sort_imported_route_from_previous_movement_for_so_application;
                    }
                    sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                    #endregion
                }
                if (mvmntPayLoad.MvmntType.MovementType == (int)MovementType.vr_1 || mvmntPayLoad.MvmntType.MovementType == (int)MovementType.notification)
                {
                    result = routesService.SaveRouteInRouteParts(routepartId, 0, versionid, contentref, SessionInfo.UserSchema);

                    #region System Event Log - Haulier_imported_route_from_previous_movement
                    movactiontype.ESDALRef = PrevMovEsdalRefNum;
                    if (NotificationId != 0)
                        movactiontype.SystemEventType = SysEventType.Haulier_imported_route_from_previous_movement;
                    else
                    {
                        if (SessionInfo.UserSchema == UserSchema.Portal)
                            movactiontype.SystemEventType = SysEventType.Haulier_imported_route_from_previous_movement_for_vr1_application;
                        else if (SessionInfo.UserSchema == UserSchema.Sort)
                            movactiontype.SystemEventType = SysEventType.Sort_imported_route_from_previous_movement_for_vr1_application;
                    }
                    #endregion
                }
                if (result != 0)
                {
                    movactiontype.UserName = SessionInfo.UserName;
                    movactiontype.PrevMovRouteId = routepartId;
                    movactiontype.RevisionId = AppRevId;
                    movactiontype.NotificationID = NotificationId;
                    movactiontype.RouteId = (int)result;
                    sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                    mvmntPayLoad.IsRouteVehicleAssigned = false;
                    ModifyWorkfLowPayload(mvmntPayLoad, applicationNotificationManagement);
                    TempData["apprevisionId"] = AppRevId;
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Routes/ImportRouteFromPrevious, Exception: {0}", ex));
                return Json(0);
            }
        }
        #endregion

        [HttpPost]
        public JsonResult GetSoRouteDescMap(long plannedRouteId = 0, string routeType = "planned")
        {
            UserInfo SessionInfo = new UserInfo();
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }

            RoutePart rp;
            if (routeType == "outline")
                rp = plannedRouteId > 0 ? routesService.GetApplicationRoutePartGeometry(plannedRouteId, SessionInfo.UserSchema) : new RoutePart();
            else
            {
                rp = plannedRouteId > 0 ? routesService.GetApplicationRoute(plannedRouteId, SessionInfo.UserSchema) : new RoutePart();
            }

            RoutePartJson routePartJson1 = new RoutePartJson();
            try
            {
                string rson = JsonConvert.SerializeObject(rp);
                routePartJson1 = JsonConvert.DeserializeObject<RoutePartJson>(rson);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            ViewBag.rp = rp;
            return Json(new { result = routePartJson1 });
        }
        public ActionResult GetAgreedRoute(long routePartId)
        {
            UserInfo SessionInfo = new UserInfo();
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }
            RoutePart objDt = routesService.GetApplicationRoute(routePartId, SessionInfo.UserSchema);

            RoutePartJson routePartJson1 = new RoutePartJson();
            try
            {
                string rson = null;
                if (objDt.RoutePartDetails.RouteId > 0)
                {
                    rson = JsonConvert.SerializeObject(objDt);
                    routePartJson1 = JsonConvert.DeserializeObject<RoutePartJson>(rson);
                    return Json(routePartJson1);
                }
                else
                {
                    return Json(routePartJson1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(routePartJson1);
            }
        }
        public ActionResult GetRouteVehicleDetails(long routePartId, string routeType, int isHistory = 0)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            RoutePart routePart;
            if (routeType == "outline")
                routePart = routesService.GetApplicationRoutePartGeometry(routePartId, SessionInfo.UserSchema);
            else
            {
                if (isHistory == 0)
                {
                    routePart = routesService.GetApplicationRoute(routePartId, SessionInfo.UserSchema);
                }
                else
                {
                    routePart = routesService.GetHistoricAppRoute(routePartId, SessionInfo.UserSchema);
                }
            }            
            List<VehicleDetails> vehicleList = vehicleconfigService.GetVehicleList(routePartId, SessionInfo.UserSchema, isHistory);
            RoutePartJson routePartJson = new RoutePartJson();
            try
            {
                string rson = null;
                if (routePart.RoutePartDetails.RouteId > 0)
                {
                    rson = JsonConvert.SerializeObject(routePart);
                    routePartJson = JsonConvert.DeserializeObject<RoutePartJson>(rson);
                    return Json(new { routeDetails = routePartJson, vehicleDetails = vehicleList });
                }
                else
                {
                    return Json(new { routeDetails = routePartJson, vehicleDetails = vehicleList });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(new { routeDetails = routePartJson, vehicleDetails = vehicleList });
            }
        }
        public ActionResult ViewRoute(string plannedRouteName, long plannedRouteId = 0, string PageFlag = "S", string routeType = "planned", string ApplicationRevId = "0", bool IsTextualRouteType = false, bool IsStartAndEndPointOnly = false, int ShowReturnLeg = 0, string workflowProcess = "")
        {
            UserInfo SessionInfo = new UserInfo();
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }
            Session["plannedRouteId"] = plannedRouteId;
            if (ApplicationRevId == "0")
            {
                ViewBag.Origin = "Lib";
                long value = 0;
                if (Session["ApplicationRevId"] != null)
                    if (Session["RouteFlag"] != null && Session["RouteFlag"].ToString() != "3")
                    {
                        value = Convert.ToInt32(Session["ApplicationRevId"].ToString());
                    }

                if (value < 0)
                {
                    Session["ApplicationRevId"] = ApplicationRevId;
                }
                Session["IsLibrary"] = true;//session checks for indicating library route
            }
            else
            {
                ViewBag.Origin = "App";
                Session["ApplicationRevId"] = ApplicationRevId;
                Session["IsLibrary"] = false;
            }
            if (Session["RouteFlag"] == null)
                Session["ApplicationRevId"] = null;
            try
            {
                if (routeType == "outline")
                {
                    RoutePart rp;
                    if (ApplicationRevId == "0")
                        rp = plannedRouteId > 0 ? routesService.GetLibraryRoute(plannedRouteId, SessionInfo.UserSchema) : new RoutePart();
                    else // This is for so app and VR1 app , Notification.
                    {
                        if (Session["RouteFlag"].ToString() == "4")
                            rp = plannedRouteId > 0 ? routesService.GetCandidateOutlineRoute(plannedRouteId, UserSchema.Sort) : new RoutePart();
                        else if (Session["RouteFlag"].ToString() == "2")
                            rp = plannedRouteId > 0 ? routesService.GetApplicationRoutePartGeometry(plannedRouteId, SessionInfo.UserSchema) : new RoutePart();
                        else
                            rp = plannedRouteId > 0 ? routesService.GetApplicationRoutePartGeometry((int)plannedRouteId, SessionInfo.UserSchema) : new RoutePart();
                        ViewBag.rp = rp;
                    }
                    if (plannedRouteId == 0)
                    {
                        rp.RoutePartDetails.RouteId = 0;
                        RoutePath routeP = new RoutePath();
                        routeP.RoutePointList.Add(new RoutePoint());
                        routeP.RoutePointList.Add(new RoutePoint());
                        rp.RoutePathList.Add(routeP);//placeholder for start and end
                    }
                    else
                    {
                        rp.RoutePartDetails.RouteId = plannedRouteId;
                        if (isRoutePresentable(rp.RoutePathList[0].RoutePointList))
                        {
                            ViewBag.routeID = plannedRouteId;
                            ViewBag.plannedRouteName = Request.QueryString["plannedRouteName"];
                            ViewBag.PageFlag = PageFlag;
                            TempData["RouteID"] = plannedRouteId;
                            if (PageFlag == "updateOutline")
                                return PartialView("OutlineRouteDetails", rp);
                            else
                                return View();
                        }
                    }
                    if (PageFlag == "updateOutline")
                    {
                        return PartialView("OutlineRouteDetails", null);
                    }

                    ViewBag.IsTextualRouteType = IsTextualRouteType;
                    ViewBag.IsStartAndEndPointOnly = IsStartAndEndPointOnly;
                    return PartialView("OutlineRouteDetails", rp);
                }
                if (plannedRouteId == 0)
                {
                    ViewBag.IsNewRoute = true;
                }
                else
                {
                    ViewBag.IsNewRoute = false;
                }
                ViewBag.routeID = plannedRouteId;
                ViewBag.plannedRouteName = Request.QueryString["plannedRouteName"];
                ViewBag.PageFlag = PageFlag;
                TempData["RouteID"] = plannedRouteId;
                ViewBag.ShowReturnLeg = ShowReturnLeg;
                if (plannedRouteId == 0)
                {
                    RouteUpdateFlagSessionClear();
                }
                return PartialView("_ViewRoute");
            }
            catch (Exception)
            {
                return PartialView("_ViewRoute");
            }
        }
        #region Audit log recording for all notifications
        public void SaveAuditLogs(AuditActionType VAuditActionType, string routeName = "")
        {
            string EsdalReference = Convert.ToString(Session["NENesdal_ref"]);
            int InboxId = Convert.ToInt32(Session["NENINBOX_ITEM_ID"]);

            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }

            AuditLogIdentifiers auditLogType = new AuditLogIdentifiers();
            string ErrMsg = string.Empty;
            auditLogType.NENID = Convert.ToInt64(SessionInfo.UserId);
            if (SessionInfo.UserSchema == UserSchema.Portal)
            {
                //auditLogType.auditActionType = AuditActionType.soauser_set_user_for_scrutiny;
                auditLogType.AuditActionType = VAuditActionType;
                auditLogType.HelpDeskUserID = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                auditLogType.HelpDeskUsername = SessionInfo.HelpdeskUserName;
                auditLogType.NENNotificationNo = EsdalReference;
                auditLogType.ESDALNotificationNo = EsdalReference;
                auditLogType.InboxItemId = Convert.ToString(InboxId);
                auditLogType.NENToScrutinyUser = SessionInfo.UserName;
                auditLogType.DateTime = DateTime.Now.ToString();
                auditLogType.NENRouteName = routeName;
                string auditLogDescp = AuditLog.GetNENNotifAuditLog(SessionInfo, auditLogType, out ErrMsg);
                int user_ID = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                var orgId = SessionInfo.organisationId > 0 ? SessionInfo.organisationId : SessionInfo.OrganisationId;
                long auditLogResult = auditLogService.SaveNotifAuditLog(auditLogType, auditLogDescp, user_ID, orgId);
            }
        }
        #endregion
        #region Check if route is attached to a vehicle
        /// <summary>
        /// Method to check a route is attached to a vehicle
        /// </summary>
        /// <param name="routePartId"></param>
        /// <returns></returns>
        public JsonResult CheckRouteVehicleAttach(long routePartId)
        {
            int result = routesService.CheckRouteVehicleAttach(routePartId);

            return Json(new { success = result }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public JsonResult AddReturnRoute(long routePartId)
        {
            List<RoutePoint> routePoints = routesService.GetRoutePointsForReturnLeg(0, routePartId);
            RetunRouteObject retunRouteList = new RetunRouteObject();
            retunRouteList.RouteId = routePartId;
            for (var i = 0; i < routePoints.Count; i++)
            {
                if (routePoints[i].PointType == 239002)
                {
                    retunRouteList.StartDescr = routePoints[i].PointDescr;
                    retunRouteList.StartEasting = routePoints[i].PointGeom.sdo_point.X;
                    retunRouteList.StartNorthing = routePoints[i].PointGeom.sdo_point.Y;
                }
                if (routePoints[i].PointType == 239001)
                {
                    retunRouteList.EndDescr = routePoints[i].PointDescr;
                    retunRouteList.EndEasting = routePoints[i].PointGeom.sdo_point.X;
                    retunRouteList.EndNorthing = routePoints[i].PointGeom.sdo_point.Y;
                }
            }
            return Json(new { result = retunRouteList });
        }
        public JsonResult SetWorkFlowPayload(long mainRoutePartId, long returnRoutePartId)
        {
            var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
            PlanMvmntPayLoad planMovePayload = applicationNotificationManagement.GetPlanMvmtPayload();
            ReturnRouteMapping returnRouteMapping = new ReturnRouteMapping
            {
                MainRouteId = mainRoutePartId,
                ReturnRouteId = returnRoutePartId
            };
            planMovePayload.ReturnRouteMappings.Add(returnRouteMapping);
            ModifyWorkfLowPayload(planMovePayload, applicationNotificationManagement);
            return Json(new { result = true });
        }

        private void ModifyWorkfLowPayload(PlanMvmntPayLoad planMovePayload, ApplicationNotificationManagement applicationNotificationManagement)
        {
            if (applicationNotificationManagement.IsThisMovementExist(planMovePayload.NotificationId, planMovePayload.RevisionId, out string workflowKey)
                    && WorkflowTaskFinder.FindNextTask("HaulierApplication", WorkflowActivityTypes.An_Activity_ConfirmMovementType, out dynamic workflowPayload) != string.Empty)
            {
                dynamic dataPayload = new ExpandoObject();
                dataPayload.PlanMvmntPayLoad = planMovePayload;
                WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                {
                    data = dataPayload,
                    workflowData = workflowPayload
                };
                applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);
            }
        }

        public JsonResult ReOrderRoutePart(string routePartIds)
        {
            try
            {
                var sessionValues = (UserInfo)Session["UserInfo"];
                int result = routesService.ReOrderRoutePart(routePartIds, sessionValues.UserSchema);
                return Json(new { result = true, data = result });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(new { error=ex.Message.ToString() });
            }
        }

        #region ListInputRouteDescription
        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationidVal"></param>
        /// <param name="isNenViaPdf"></param>
        /// <param name="isHistoric"></param>
        /// <returns></returns>
        public ActionResult ListInputRouteDescription(string notificationidVal, int? isNenViaPdf, int? isHistoric)
        {
            List<RoutePartDetails> routePartDetails;
            try
            {
                var sessionValues = (UserInfo)Session["UserInfo"];
                routePartDetails = routesService.GetRoutePartDetails(notificationidVal, isNenViaPdf, isHistoric, (int)sessionValues.OrganisationId, UserSchema.Portal);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/ListRouteDescription, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
            return PartialView("_ListInputRouteDescription", routePartDetails);
        }
        #endregion
    }
}