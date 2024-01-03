using PagedList;
using STP.Common.Logger;
using STP.Domain.LoggingAndReporting;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.VehicleAndFleets.Component;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.SecurityAndUsers;
using STP.Domain.Structures;
using STP.Web.Filters;
using STP.ServiceAccess.DocumentsAndContents;
using STP.ServiceAccess.LoggingAndReporting;
using STP.ServiceAccess.MovementsAndNotifications;
using STP.ServiceAccess.Structures;
using STP.ServiceAccess.VehiclesAndFleets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using STP.Domain.Applications;
using STP.ServiceAccess.Applications;
using STP.Web.WorkflowProvider;
using STP.Domain.Routes;
using STP.ServiceAccess.Routes;
using STP.Common.Enums;
using static STP.Domain.VehiclesAndFleets.VehicleEnums;
using STP.Common.Constants;
using STP.Common.EncryptDecrypt;
using System.Xml;
using System.Text;
using STP.ServiceAccess.Workflows.ApplicationsNotifications;
using System.Dynamic;
using STP.Domain.Workflow;
using NetSdoGeometry;
using STP.Web.General;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.IO;
using STP.Domain.DocumentsAndContents;
using System.Xml.Serialization;
using STP.Web.Document;
using Rotativa;
using STP.ServiceAccess.RouteAssessment;
using STP.Domain.Workflow.Models;
using STP.ServiceAccess.Workflows;
using System.Globalization;
using STP.ServiceAccess.SecurityAndUsers;
using static STP.Common.Enums.ExternalApiEnums;
using STP.Domain.VehiclesAndFleets.Component;
using STP.Domain.VehiclesAndFleets;

namespace STP.Web.Controllers
{
    public class MovementsController : Controller
    {
        private readonly IMovementsService movementsService;
        private readonly IDocumentService documentService;
        private readonly IStructuresService structuresService;
        private readonly INENNotificationService nenNotificationService;
        private readonly INotificationDocService notificationDocService;
        private readonly ILoggingService loggingService;
        private readonly IAuditLogService auditlogService;
        private readonly IVehicleConfigService vehicleconfigService;
        private readonly IApplicationService applicationService;
        private readonly IRoutesService routesService;
        private readonly INotificationService notificationService;
        private readonly IApplicationNotificationWorkflowService applicationNotificationWorkflowService;
        private readonly IRouteAssessmentService routeAssessmentService;
        private readonly ISOAPoliceWorkflowService soaPoliceWorkflowService;
        private readonly ISORTApplicationService sortApplicationService;
        private readonly IUserService userService;
        private readonly IVehicleComponentService vehicleComponentService;
        public MovementsController()
        {
        }


        public MovementsController(IMovementsService movementsService, IDocumentService documentService, IStructuresService structuresService,
            INENNotificationService nenNotificationService, INotificationDocService notificationDocService, ILoggingService loggingService,
            IAuditLogService auditlogService, IVehicleConfigService vehicleconfigService, IApplicationService applicationService, IRoutesService routesService,
            IApplicationNotificationWorkflowService applicationNotificationWorkflowService, INotificationService notificationService,
            IRouteAssessmentService routeAssessmentService, ISOAPoliceWorkflowService soaPoliceWorkflowService, ISORTApplicationService sortApplicationService,
            IUserService userService, IVehicleComponentService vehicleComponentService)
        {
            this.movementsService = movementsService;
            this.documentService = documentService;
            this.structuresService = structuresService;
            this.nenNotificationService = nenNotificationService;
            this.notificationDocService = notificationDocService;
            this.loggingService = loggingService;
            this.auditlogService = auditlogService;
            this.vehicleconfigService = vehicleconfigService;
            this.applicationService = applicationService;
            this.routesService = routesService;
            this.notificationService = notificationService;
            this.applicationNotificationWorkflowService = applicationNotificationWorkflowService;
            this.routeAssessmentService = routeAssessmentService;
            this.soaPoliceWorkflowService = soaPoliceWorkflowService;
            this.sortApplicationService = sortApplicationService;
            this.userService = userService;
            this.vehicleComponentService = vehicleComponentService;
        }
        public static MovementActionIdentifiers movactiontype = null;


        #region Hauliers Inbox

        #region public ActionResult MovementList()
        // [AuthorizeUser(Roles = "13001,13003,13004,13005")]
        public ActionResult MovementList(int? page, int? pageSize, bool MovementListForSO = false, int showrtveh = 0, int OrgID = 0, bool VR1route = false, bool IsNotify = false, bool IsSOvehicle = false, bool IsSOroute = false, bool IsNotifyRoute = false, bool IsVR1PreMovroute = false, bool IsNotifPreMovRoute = false, bool IsRoutePrevMoveOpion = false, bool IsVehiclePrevMoveOpion = false, bool PrevMovImport = false, bool planMovement = false, int? sortType = null, int? sortOrder = null)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "Movements/MovementList actionResult method started successfully");
                UserInfo SessionInfo = null;
                if (page == null && Session["PreviousMovementListPage"] != null)
                {
                    page = 1;
                }
                //if (page == null && Session["PreviousMovementListPage"] != null &&(PrevMovImport=true))
                //        {
                //    page = 1;
                //}
                SessionInfo = (UserInfo)Session["UserInfo"];
                if (!PageAccess.GetPageAccess("13001") && (SessionInfo.UserTypeId != 696008))
                {
                    return RedirectToAction("Error", "Home");
                }

                int organisationId;
                if (SessionInfo.UserTypeId == 696008)
                {
                    organisationId = (int)Session["SORTOrgID"];
                    if (organisationId == 0) { organisationId = OrgID; }
                }
                else
                {
                    organisationId = (int)SessionInfo.OrganisationId;
                }


                int portalType = SessionInfo.UserTypeId;
                int maxlist_item = SessionInfo.MaxListItem;
                if (portalType != 696001)
                {
                    return RedirectToAction("Login", "Users");
                }
                //viewbag for pagination
                int pageNumber = (page ?? 1);
                if (Session["PageSize"] == null)
                {
                    Session["PageSize"] = maxlist_item;
                }

                if (pageSize == null)
                {
                    pageSize = ((Session["PageSize"] != null) && (Convert.ToString(Session["PageSize"]) != "0")) ? (int)Session["PageSize"] : 10;
                }
                else
                {
                    Session["PageSize"] = pageSize;
                }

                ViewBag.pageSize = pageSize;
                Session["PreviousMovementListPage"] = pageNumber;
                ViewBag.page = pageNumber;
                ViewBag.MovementListForSO = MovementListForSO;
                ViewBag.showrtveh = showrtveh;
                ViewBag.OrgID = organisationId;
                ViewBag.VR1route = VR1route;
                ViewBag.IsNotify = IsNotify;

                ViewBag.IsSOvehicle = IsSOvehicle;
                ViewBag.IsSOroute = IsSOroute;
                ViewBag.IsVR1PreMovroute = IsVR1PreMovroute;
                ViewBag.IsNotifyRoute = IsNotifyRoute;
                ViewBag.IsNotifPreMovRoute = IsNotifPreMovRoute;
                TempData["MovementListForSO"] = MovementListForSO;
                ViewBag.IsRoutePrevMoveOpion = IsRoutePrevMoveOpion;
                ViewBag.IsVehiclePrevMoveOpion = IsVehiclePrevMoveOpion;
                ViewBag.PlanMovement = planMovement;
                ViewBag.sortType = sortType;
                ViewBag.sortOrder = sortOrder;
                if (Session["g_SearchData"] != null)
                {
                    ViewBag.MovementFilter = Session["g_SearchData"];
                }

                ViewBag.PrevMovImport = PrevMovImport;

                GetPresetFilters();
                return View();
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movements/MovementList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region public ActionResult SOMovementList()
        public ActionResult SOMovementList(int? page, int? pageSize, bool MovementListForSO, int showrtveh = 0, int OrgID = 0, bool VR1route = false, bool IsNotify = false, bool IsSOvehicle = false, bool IsSOroute = false, bool IsNotifyRoute = false, bool IsVR1PreMovroute = false, bool IsNotifPreMovRoute = false, bool IsRoutePrevMoveOpion = false, bool IsVehiclePrevMoveOpion = false, bool prevMovImport = false, bool planMovement = false, int? sortType = null, int? sortOrder = null)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "Movements/SOMovementList actionResult method started successfully");

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                int organisationId = (int)SessionInfo.OrganisationId;
                int userTypeId = SessionInfo.UserTypeId;
                int maxlist_item = SessionInfo.MaxListItem;

                if (userTypeId == 696008)
                {
                    organisationId = OrgID;//3393;// OrgID;  // changed according to haulier org tab creation
                }
                //BL method goes here                
                int pageNumber = (page ?? 1);
                if (Session["PageSize"] == null)
                {
                    Session["PageSize"] = maxlist_item;
                }

                if (pageSize == null)
                {
                    pageSize = ((Session["PageSize"] != null) && (Convert.ToString(Session["PageSize"]) != "0")) ? (int)Session["PageSize"] : 10;
                }
                else
                {
                    Session["PageSize"] = pageSize;
                }

                ViewBag.pageSize = pageSize;
                ViewBag.page = pageNumber;
                ViewBag.organisationId = organisationId;
                ViewBag.MovementListForSO = MovementListForSO;
                ViewBag.showrtveh = showrtveh;
                ViewBag.VR1route = VR1route;
                ViewBag.IsNotify = IsNotify;
                ViewBag.IsNotifyRoute = IsNotifyRoute;

                ViewBag.IsSOvehicle = IsSOvehicle;
                ViewBag.IsSOroute = IsSOroute;
                ViewBag.IsVR1PreMovroute = IsVR1PreMovroute;
                ViewBag.IsNotifPreMovRoute = IsNotifPreMovRoute;
                ViewBag.IsRoutePrevMoveOpion = IsRoutePrevMoveOpion;
                ViewBag.IsVehiclePrevMoveOpion = IsVehiclePrevMoveOpion;
                MovementsAdvancedFilter objMovementsAdvancedFilter = new MovementsAdvancedFilter();
                MovementsFilter objMovementFilter = new MovementsFilter();
                objMovementFilter.NeedsAttention = true;
                Session["MovementFilter"] = objMovementFilter;

                int presetFilter = 0;
                objMovementsAdvancedFilter.SORTOrder = 1;
                if (Session["g_MovementPresetFilter"] != null)
                {
                    presetFilter = (int)Session["g_MovementPresetFilter"];
                }


                if (MovementListForSO) //for so
                {
                    setfilterforso(ref objMovementFilter, ref objMovementsAdvancedFilter);
                    if (Session["g_SearchData"] != null)
                    {
                        objMovementFilter = (MovementsFilter)Session["g_SearchData"];
                    }

                }
                else if (VR1route) //for so
                {
                    objMovementFilter.ApprovedVR1 = true;
                }
                else
                {
                    if (planMovement)
                    {
                        if (Session["movement_SearchData"] != null)
                        {
                            objMovementFilter = (MovementsFilter)Session["movement_SearchData"];
                        }
                    }
                    else
                    {
                        if (Session["g_SearchData"] != null)
                        {
                            objMovementFilter = (MovementsFilter)Session["g_SearchData"];
                            if (objMovementFilter.WorkInProgressAppNotif)
                            {
                                objMovementFilter.WorkInProgressApplication = true;
                                objMovementFilter.WorkInProgressNotification = true;
                            }
                            else
                            {
                                objMovementFilter.WorkInProgressApplication = false;
                                objMovementFilter.WorkInProgressNotification = false;
                            }
                        }
                    }
                }
                if (planMovement)
                {
                    if (Session["movement_AdvancedSearchData"] != null)
                    {
                        objMovementsAdvancedFilter = (MovementsAdvancedFilter)Session["movement_AdvancedSearchData"];
                    }
                }
                else
                {
                    if (Session["g_AdvancedSearchData"] != null)
                    {
                        objMovementsAdvancedFilter = (MovementsAdvancedFilter)Session["g_AdvancedSearchData"];
                    }
                }
                //sorting 
                objMovementsAdvancedFilter.SORTOrder = sortOrder != null && sortOrder != 0 ? (int)sortOrder : 11; //esdal_ref_no
                presetFilter = sortType != null ? (int)sortType : 0; // desc
                ViewBag.SortOrder = objMovementsAdvancedFilter.SORTOrder;
                ViewBag.SortType = presetFilter;
                //End sorting

                ViewBag.objMovementFilterHaul = objMovementFilter;
                int totalCount = 0;

                if (MovementListForSO)
                {
                    objMovementFilter.Agreed = true;
                    objMovementFilter.ProposedRoute = true;
                    objMovementFilter.ReceivedByHA = false;
                    objMovementFilter.Submitted = false;
                    objMovementFilter.IncludeHistoric = false;
                    objMovementFilter.WorkInProgressApplication = false;
                    objMovementFilter.SO = true;
                    if (showrtveh == 0)
                    {
                        objMovementsAdvancedFilter.MyReference = "";
                    }
                }

                if (VR1route)
                {
                    objMovementFilter.ReceivedByHA = false;
                    objMovementFilter.Submitted = false;
                    objMovementFilter.ApprovedVR1 = true;
                    objMovementFilter.IncludeHistoric = false;
                    objMovementFilter.NeedsAttention = false;
                    objMovementFilter.STGOVR1 = true;
                    objMovementsAdvancedFilter.MyReference = "";

                }

                //condition needs to be changed
                if (IsNotify)
                {
                    objMovementFilter.Agreed = false;
                    objMovementFilter.ProposedRoute = false;
                    objMovementFilter.ReceivedByHA = false;
                    objMovementFilter.Submitted = false;
                    objMovementFilter.WorkInProgressNotification = false;
                    objMovementFilter.NeedsAttention = false;
                    objMovementFilter.IncludeHistoric = false;
                    objMovementFilter.STGO = true;
                    objMovementFilter.Notifications = true;
                    objMovementsAdvancedFilter.MyReference = "";
                }

                //condition for detailed notification for vr1
                if (IsNotify && VR1route)
                {
                    objMovementFilter.NeedsAttention = false;
                    objMovementFilter.STGO = false;
                    objMovementFilter.ApprovedVR1 = false;
                    objMovementFilter.Agreed = false;
                    objMovementFilter.ProposedRoute = false;
                    objMovementFilter.ReceivedByHA = false;
                    objMovementFilter.Notifications = false;
                    objMovementFilter.WorkInProgressNotification = false;
                    objMovementFilter.WorkInProgressApplication = false;
                    objMovementFilter.Submitted = true;
                    objMovementFilter.STGOVR1 = true;

                    ViewBag.VR1route = false;
                    ViewBag.IsNotify = false;
                    ViewBag.VR1Notify = true;
                }

                if (IsNotify && MovementListForSO)
                {
                    objMovementFilter.Agreed = true;
                    objMovementFilter.SO = true;
                    objMovementFilter.Notifications = false;
                    objMovementFilter.STGOVR1 = false;
                    objMovementFilter.STGO = false;
                    objMovementFilter.IncludeHistoric = true;

                    ViewBag.VR1route = false;
                    ViewBag.IsNotify = true;
                    ViewBag.VR1Notify = false;
                    ViewBag.MovementListForSO = false;
                }
                ViewBag.PlanMovement = planMovement;
                List<MovementsList> objMovement;

                if (planMovement)
                {
                    objMovementFilter.NeedsAttention = false;
                    if (objMovementsAdvancedFilter.ApplicationType == 4)
                        ViewBag.IsExistSOVr1 = true;
                    else
                        objMovementsAdvancedFilter.ApplicationType = 3;

                    if (objMovementsAdvancedFilter.ApplicationFromDate != null && objMovementsAdvancedFilter.NotificationFromDate == null)
                        objMovementsAdvancedFilter.ApplicationType = 1;
                    else if (objMovementsAdvancedFilter.ApplicationFromDate == null && objMovementsAdvancedFilter.NotificationFromDate != null)
                        objMovementsAdvancedFilter.ApplicationType = 2;
                }
               

                objMovement = GetMovementList(organisationId, pageNumber, (int)pageSize, objMovementFilter, objMovementsAdvancedFilter, presetFilter, showrtveh, prevMovImport);
                if (objMovement.Count > 0)
                {
                    totalCount = objMovement[0].TotalRowCount;
                }

                var movementObjPagedList = new StaticPagedList<MovementsList>(objMovement, pageNumber, (int)pageSize, totalCount);
                ViewBag.TotalCount = totalCount;

                ViewBag.MovementFilter = objMovementFilter;
                ViewBag.MovementsAdvancedFilter = objMovementsAdvancedFilter;

                return PartialView(movementObjPagedList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movements/SOMovementList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region public ActionResult HaulierMovement()
        public ActionResult HaulierMovement(MovementsFilter objMovementsFilter, MovementsAdvancedFilter objMovementsAdvancedFilter)
        {
            objMovementsFilter.Agreed = true;
            objMovementsFilter.ApprovedVR1 = true;
            objMovementsFilter.NeedsAttention = false;
            objMovementsFilter.NotifyVSO = 1;

            var MovementFromDate = DateTime.Now.AddMonths(-6);
            objMovementsAdvancedFilter.MovementFromDate = MovementFromDate.ToString("dd-MMM-yyyy");
            objMovementsAdvancedFilter.MovementFrom = MovementFromDate;
            objMovementsAdvancedFilter.MovementDate = true;//Checkbox
            Session["g_SearchData"] = objMovementsFilter;
            Session["g_AdvancedSearchData"] = objMovementsAdvancedFilter;
            return RedirectToAction("MovementList", new
            {
                B7vy6imTleYsMr6Nlv7VQ =
                        STP.Web.Helpers.EncryptionUtility.Encrypt("sortType=0" +
                        "&sortOrder=12")
            });
        }
        #endregion

        #region public ActionResult SOMovementFilter()
        public ActionResult SOMovementFilter(bool IsNotify = false, bool VR1Appl = false)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "Movements/SOMovementFilter actionResult method started successfully");

                UserInfo SessionInfo = null;
                long organisationId = 0;
                string userSchema = "";
                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction(actionName, controllerName);
                }
                else
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                    organisationId = SessionInfo.OrganisationId;
                    userSchema = SessionInfo.UserSchema;
                }

                ViewBag.VR1Appl = VR1Appl;
                ViewBag.IsNotify = IsNotify;
                if (Session["g_ShowPrevSortRoute"] != null)
                {
                    ViewBag.ShowPrevSortRoute = Session["g_ShowPrevSortRoute"];
                }
                else
                {
                    ViewBag.ShowPrevSortRoute = 0;
                }

                var FolderList = movementsService.GetFolderList(organisationId, userSchema);
                SelectList FolderSelectList = new SelectList(FolderList, "FolderID", "FolderName");
                ViewBag.FolderNameDropDown = FolderSelectList;

                MovementsFilter objMovementFilter = new MovementsFilter();

                #region set accordian type
                int accordType = 1;
                if (Session["g_AccordianType"] != null)
                {
                    accordType = (int)Session["g_AccordianType"];
                }
                ViewBag.Accordian = accordType;
                #endregion set accordian type

                if (Session["g_SearchData"] != null)
                {
                    objMovementFilter = (MovementsFilter)Session["g_SearchData"];
                }
                else
                {
                    objMovementFilter.IncludeHistoric = true;
                    objMovementFilter.NeedsAttention = true;
                }
                return PartialView(objMovementFilter);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movements/SOMovementFilter, Exception: {0}", ex));
                throw;
            }
        }
        #endregion

        #region public ActionResult SOAdvancedSearch()
        public ActionResult SOAdvancedSearch(bool MovementListForSO = false, int ShowPrevSortRoute = 0, bool planMovement = false)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "Movements/SOAdvancedSearch actionResult method started successfully");

                MovementsAdvancedFilter objMovementsAdvancedFilter = new MovementsAdvancedFilter();
                if (planMovement)
                {
                    if (Session["movement_AdvancedSearchData"] != null)
                    {
                        objMovementsAdvancedFilter = (MovementsAdvancedFilter)Session["movement_AdvancedSearchData"];
                    }
                }
                else
                {
                    if (Session["g_AdvancedSearchData"] != null)
                    {
                        objMovementsAdvancedFilter = (MovementsAdvancedFilter)Session["g_AdvancedSearchData"];
                    }
                }
                GetComparisonDropDown();
                VehicleDimensionDropDown();
                ViewBag.MovementListForSO = MovementListForSO;
                ViewBag.ShowPrevSortRoute = ShowPrevSortRoute;
                ViewBag.PlanMovement = planMovement;
                Session["g_ShowPrevSortRoute"] = ShowPrevSortRoute;
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                ViewBag.UserType = SessionInfo.UserTypeId;
                if (SessionInfo.UserTypeId == 696008)
                {
                    ViewBag.SORTOrgId = (int)Session["SORTOrgID"];
                }
                return PartialView(objMovementsAdvancedFilter);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movements/SOAdvancedSearch, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region public ActionResult SetSearchData(MovementsFilter objMovementsFilter)
        public ActionResult SetSearchData(MovementsFilter objMovementsFilter, MovementsAdvancedFilter objMovementsAdvancedFilter, int ShowPrevSortRoute = 0, bool prevMovImport = false, bool planMovement = false, int isExistVR1SoClass = 0, int? page = null, int pageSize = 10, bool isclear = false)
        {
            if (planMovement)
                Session["movement_SearchData"] = objMovementsFilter;
            else
                Session["g_SearchData"] = objMovementsFilter;
            if (isclear)
            {
                //objMovementsFilter.NeedsAttention = true;
            }
            if (isExistVR1SoClass != 0)
            {
                objMovementsAdvancedFilter.VehicleClass = Convert.ToString(isExistVR1SoClass);
                objMovementsAdvancedFilter.ApplicationType = 4;
            }
            if (objMovementsFilter.BtnClearSORTOrder == 1)
            {
                objMovementsAdvancedFilter.SORTOrder = 1;
                objMovementsFilter.BtnClearSORTOrder = 0;
            }
            else
            {
                if (planMovement)
                    Session["movement_AdvancedSearchData"] = objMovementsAdvancedFilter;
                else
                    Session["g_AdvancedSearchData"] = objMovementsAdvancedFilter;
            }
            return RedirectToAction("MovementList", new
            {
                B7vy6imTleYsMr6Nlv7VQ =
                        STP.Web.Helpers.EncryptionUtility.Encrypt("planMovement=" + planMovement +
                        "&prevMovImport=" + prevMovImport +
                        "&page=" + page +
                        "&sortType=" + objMovementsFilter.SortTypeValue +
                        "&sortOrder=" + objMovementsFilter.SortOrderValue + "&pageSize=" + pageSize)
            });
        }
        #endregion

        #region public ActionResult SetAdvancedSearchData(MovementsFilter objMovementsFilter)
        public ActionResult SetAdvancedSearchData(MovementsAdvancedFilter objMovementsAdvancedFilter, MovementsFilter objMovementsFilter, bool MovementListForSO = false, bool isFromNotif = false, bool VR1route = false, int ShowPrevMoveSortRoute = 0, int SortOrgId = 0)
        {
            if (ModelState.IsValid)
            {
                if (objMovementsAdvancedFilter.StartPoint != null)
                {
                    objMovementsAdvancedFilter.StartOrEnd = objMovementsAdvancedFilter.StartPoint;
                }
                else if (objMovementsAdvancedFilter.EndPoint != null)
                {
                    objMovementsAdvancedFilter.StartOrEnd = objMovementsAdvancedFilter.EndPoint;
                }

                Session["g_AdvancedSearchData"] = objMovementsAdvancedFilter;
                if (!isFromNotif)
                    Session["g_SearchData"] = objMovementsFilter;

                if (MovementListForSO || VR1route || isFromNotif)
                    return RedirectToAction("MovementList", new
                    {
                        B7vy6imTleYsMr6Nlv7VQ =
                        STP.Web.Helpers.EncryptionUtility.Encrypt("MovementListForSO=" + MovementListForSO +
                        "&showrtveh=" + ShowPrevMoveSortRoute +
                        "&OrgID=" + SortOrgId +
                        "&VR1route=" + VR1route +
                        "&IsNotify=" + isFromNotif)
                    });
                else
                    return Json(1);
            }
            GetComparisonDropDown();
            ViewBag.MovementListForSO = false;
            return View("SOAdvancedSearch", objMovementsAdvancedFilter);
        }
        #endregion

        #region public ActionResult GetQuickLinks(int userId)
        public ActionResult GetQuickLinks()
        {
            int userId = 24977;
            return View();
        }
        #endregion

        #region public JsonResult SetPresetFilter(int presetFilter)
        public JsonResult SetPresetFilter(int presetFilter)
        {
            Session["g_MovementPresetFilter"] = presetFilter;
            ChangeFiltersPerPreset(presetFilter);
            return Json(new { data = true });
        }
        #endregion

        #region public JsonResult FolderNames(string folderName)
        public JsonResult FolderNames(string folderName)
        {
            List<DropDown> listFolderNames = new List<DropDown>();
            DropDown objDropDown = null;
            objDropDown = new DropDown();
            objDropDown.Id = 1;
            objDropDown.Value = "abc";
            listFolderNames.Add(objDropDown);
            objDropDown = new DropDown();
            objDropDown.Id = 2;
            objDropDown.Value = "folder";
            listFolderNames.Add(objDropDown);
            objDropDown = new DropDown();
            objDropDown.Id = 3;
            objDropDown.Value = "sample";
            listFolderNames.Add(objDropDown);


            listFolderNames = (from s in listFolderNames
                               where s.Value.ToLower().Contains(folderName.ToLower())
                               select s).ToList();

            return Json(listFolderNames, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region public JsonResult ClearSOAdvancedFilter()
        public JsonResult ClearSOAdvancedFilter()
        {
            Session["g_AdvancedSearchData"] = null;
            Session["movement_AdvancedSearchData"] = null;
            return Json(new { result = 1 });
        }
        public JsonResult ClearSOHaulierFilter()
        {
            MovementsFilter objMovementFilter = new MovementsFilter();
            objMovementFilter.NeedsAttention = true;
            Session["g_SearchData"] = objMovementFilter;
            Session["g_AdvancedSearchData"] = null;
            return Json(new { result = 1 });

        }


        public JsonResult ClearSOFilter()
        {
            MovementsInboxAdvancedFilter objMovementsInboxAdvancedFilter = new MovementsInboxAdvancedFilter();
            if (Session["g_moveInboxAdvanceFilter"] != null)
            {
                objMovementsInboxAdvancedFilter = (MovementsInboxAdvancedFilter)Session["g_moveInboxAdvanceFilter"];
            }
            return Json(new { result = 1 });
        }
        #endregion

        #endregion Hauliers Inbox

        #region SOA/Police Movement Inbox

        #region public ActionResult MovementInboxList(int? page, int? pageSize)
        [AuthorizeUser(Roles = "13003,13003,13004,13005,13006")]
        public ActionResult MovementInboxList(int? page, int? pageSize, int structureID = 0, int? sectionID = null, string structureNm = "", string ESRN = "", bool IsRelatedMov = false, int? sortType = null, int? sortOrder = null, int? isRelatedInboxList = null)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "Movements/MovementInbox actionResult method started successfully");

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                int organisationId = 0;
                int presetFilter = 0;
                organisationId = (int)SessionInfo.OrganisationId;
                int userType = SessionInfo.UserTypeId;
                int maxlist_item = SessionInfo.MaxListItem;
                ViewBag.UserType = userType;
                ViewBag.sortOrder = sortOrder == null ? 1 : sortOrder;

                int portalType = SessionInfo.UserTypeId;

                if (portalType != 696007 && portalType != 696002 && portalType != 696008)
                {
                    return RedirectToAction("Login", "Users");
                }
                //viewbag for pagination
                int pageNumber = (page ?? 1);
                if (Session["PageSize"] == null)
                {
                    Session["PageSize"] = maxlist_item;
                }
                presetFilter = sortType != null ? (int)sortType : presetFilter;

                if (pageSize == null)
                {
                    pageSize = ((Convert.ToString(Session["PageSize"]) != "0") && (Session["PageSize"] != null)) ? (int)Session["PageSize"] : 10;
                }
                else
                {
                    Session["PageSize"] = pageSize;
                }

                ViewBag.pageSize = pageSize;
                ViewBag.page = pageNumber;
                ViewBag.structureID = structureID;
                ViewBag.StructureNm = structureNm;
                ViewBag.ESRN = ESRN;

                ViewBag.IsRelatedMov = IsRelatedMov;
                if (sectionID != null)
                {
                    Session["checkFlag"] = "true";
                    Session["sectionID"] = sectionID;
                }
                Session["structureID"] = structureID;
                Session["structureNm"] = structureNm;
                Session["ESRN"] = ESRN;

                if (string.IsNullOrEmpty(Convert.ToString(Session["IsFirstTime"])))
                {
                    Session["IsFirstTime"] = "1";
                    Session["g_moveInboxFilter"] = null;
                    Session["g_moveInboxAdvanceFilter"] = null;
                }
                if (isRelatedInboxList == 1)
                {
                    Session["IsRelatedInboxFirstTime"] = "1";
                    Session["isRelatedInboxList"] = "1";
                }
                else if (!string.IsNullOrEmpty(Convert.ToString(Session["isRelatedInboxList"]))){
                    Session["g_moveInboxFilter"] = null;
                    Session["g_moveInboxAdvanceFilter"] = null;
                    Session.Remove("isRelatedInboxList");
                }
                int chkValidStructCnt = 0;

                TempData["TmpchkValidStructCnt"] = 0;
                if (structureID != 0)
                {
                    int orgCheck = structuresService.CheckStructureOrganisation(organisationId, (long)structureID); // to check the sturcutre belongs to same organisation id 8389 related fix
                    if (orgCheck == 0 && !IsRelatedMov)
                    {
                        return RedirectToAction("Error", "Home");
                    }
                }
                if (structureID != 0)
                {
                    Session["structureID"] = structureID;
                    chkValidStructCnt = structuresService.CheckStructureAgainstOrganisation(Convert.ToInt64(structureID), SessionInfo.OrganisationId);
                }
                if (chkValidStructCnt > 0)
                {
                    TempData["TmpchkValidStructCnt"] = chkValidStructCnt;
                }

                return View("MovementInboxList");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movements/MovementInbox, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region public ActionResult PoliceMovementInbox()
        public ActionResult PoliceMovementInbox(int page, int pageSize, int structureID = 0, string structureNm = "", string ESRN = "", bool IsRelatedMov = false, int? sortOrder = null, int? sortType = null)
        {
            try
            {
                ViewBag.pageSize = pageSize;
                ViewBag.structureID = 0;
                ViewBag.IsRelatedMov = IsRelatedMov;
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                int organisationId = (int)SessionInfo.OrganisationId;
                int userType = SessionInfo.UserTypeId;
                int userId = Convert.ToInt32(SessionInfo.UserId);

                FillDelegateArrange();

                MovementsInboxFilter objMovementsInboxFilter = new MovementsInboxFilter();
                MovementsInboxAdvancedFilter objMovementsInboxAdvancedFilter = new MovementsInboxAdvancedFilter();
                if (Convert.ToString(Session["IsRelatedInboxFirstTime"]) == "1")
                {
                    Session["IsRelatedInboxFirstTime"] = "0";
                    Session["g_moveInboxFilter"] = objMovementsInboxFilter;
                    Session["g_moveInboxAdvanceFilter"] = objMovementsInboxAdvancedFilter;
                }
                
                int presetFilter = 0;
                objMovementsInboxFilter.Unopened = true;
                if (Convert.ToString(Session["isRelatedInboxList"]) == "1")
                {
                    ViewBag.IsRelatedInboxList = 1;
                    objMovementsInboxFilter.Unopened = false;
                }
                if (Session["g_moveInboxFilter"] != null)
                {
                    objMovementsInboxFilter = (MovementsInboxFilter)Session["g_moveInboxFilter"];
                }
                if (Session["g_moveInboxAdvanceFilter"] != null)
                {
                    objMovementsInboxAdvancedFilter = (MovementsInboxAdvancedFilter)Session["g_moveInboxAdvanceFilter"];
                }

                //objMovementsAdvancedFilter.SORTOrder = sortOrder != null ? (int)sortOrder : objMovementsAdvancedFilter.SORTOrder;
                objMovementsInboxAdvancedFilter.SortOrder = sortOrder != null ? (int)sortOrder : 6; //recieveddate
                //presetFilter = sortType != null ? (int)sortType : presetFilter;
                presetFilter = sortType != null ? (int)sortType : 1; // desc
                ViewBag.SortOrder = objMovementsInboxAdvancedFilter.SortOrder;
                ViewBag.SortType = presetFilter;

                if (structureID != 0 && Convert.ToString(Session["IsFirstTime"]) == "1")
                {
                    objMovementsInboxFilter.Unopened = false;
                    ViewBag.ChkUnopened = "0";
                }
                else if (structureID == 0 && Convert.ToString(Session["IsFirstTime"]) == "1")
                {
                    objMovementsInboxFilter.Unopened = true;
                    ViewBag.ChkUnopened = "1";
                }
                else
                {
                    if (!objMovementsInboxFilter.Unopened)
                        ViewBag.ChkUnopened = "0";
                    else
                        ViewBag.ChkUnopened = "1";
                }

                Session["IsFirstTime"] = "0";

                ViewBag.V_objMovementsInboxFilter = objMovementsInboxFilter;

                if (structureID > 0)
                {
                    objMovementsInboxAdvancedFilter.StructureId = structureID;
                    ViewBag.structureID = structureID;
                    ViewBag.StructureNm = structureNm;
                    ViewBag.ESRN = ESRN;
                }
                GetInboxMovementsParams inboxMovementsParams = new GetInboxMovementsParams
                {
                    OrganisationId = organisationId,
                    PageNumber = page,
                    PageSize = pageSize,
                    InboxFilter = objMovementsInboxFilter,
                    InboxAdvancedFilter = objMovementsInboxAdvancedFilter,
                    UserType = userType,
                    UserId = Convert.ToInt32(SessionInfo.UserId),
                    UserSchema = SessionInfo.UserSchema,
                    presetFilter = presetFilter
                };
                List<MovementsInbox> objMovementInbox = movementsService.GetMovementInbox(inboxMovementsParams);

                int totalCount = 0;
                if (objMovementInbox.Count > 0)
                {
                    totalCount = objMovementInbox[0].TotalRecord;
                }

                var movementInboxObjPagedList = new StaticPagedList<MovementsInbox>(objMovementInbox, page, pageSize, totalCount);
                Session["PageSize"] = pageSize;

                ViewBag.MovementsInboxFilter = objMovementsInboxFilter;
                ViewBag.MovementsInboxAdvancedFilter = objMovementsInboxAdvancedFilter;
                ViewBag.pageSize = pageSize;
                ViewBag.pageNum = page;

                return PartialView(movementInboxObjPagedList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movements/PoliceMovementInbox, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region public ActionResult SOAPoliceSearch()
        public ActionResult SOAPoliceSearch()
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "Movements/SOAPoliceSearch actionResult method started successfully");

            #region set accordian type
            int accordType = 1;
            if (Session["g_AccordianType"] != null)
            {
                accordType = (int)Session["g_AccordianType"];
            }
            ViewBag.Accordian = accordType;
            #endregion set accordian type

            MovementsInboxFilter objMovementsFilter = new MovementsInboxFilter();
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            var OrganisationId = (int)SessionInfo.OrganisationId;
            var UserId = SessionInfo.UserId;
            var UserTypeId = (int)SessionInfo.UserTypeId;

            List<GetUserList> GridListObj = null;
            GridListObj = userService.GetUserbyOrgId(Convert.ToString(UserTypeId), Convert.ToString(OrganisationId));
            if (GridListObj != null)
            {
                GridListObj.ForEach(x => x.FullName = x.FirstName + " " + x.SurName);
                GridListObj = GridListObj.OrderBy(x => x.FullName).ToList();
                GridListObj = GridListObj.Where(x => x.UserID != UserId).ToList();
            }

            SelectList UserSelectList = new SelectList(GridListObj, "UserID", "FullName");
            ViewBag.SelectUserDropDown = UserSelectList;

            if (Session["g_moveInboxFilter"] != null)
            {
                objMovementsFilter = (MovementsInboxFilter)Session["g_moveInboxFilter"];
                if (!objMovementsFilter.Unopened)
                    ViewBag.ChkUnopened = "0";
                else
                    ViewBag.ChkUnopened = "1";
            }
            return PartialView(objMovementsFilter);
        }
        #endregion

        #region public ActionResult MoveInboxAdvancedSearch()
        public ActionResult MoveInboxAdvancedSearch(int? userType, bool IsStruRelMove = false)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "Movements/MoveInboxAdvancedSearch actionResult method started successfully");
            ViewBag.IsStruRelMove = IsStruRelMove;
            MovementsInboxAdvancedFilter objMovementsAdvancedFilter = new MovementsInboxAdvancedFilter();
            objMovementsAdvancedFilter.SortOrder = 1;
            if (Session["g_moveInboxAdvanceFilter"] != null)
            {
                objMovementsAdvancedFilter = (MovementsInboxAdvancedFilter)Session["g_moveInboxAdvanceFilter"];
            }
            ViewBag.UserType = userType;

            GetComparisonDropDownPolice();
            VehicleDimensionDropDown();
            UserInfo SessionInfo = null;
            SessionInfo = (UserInfo)Session["UserInfo"];
            int organisationId = (int)SessionInfo.OrganisationId;
            var DelegArrangList = movementsService.GetArrangementList(organisationId);
            SelectList ObjectDelegationList = new SelectList(DelegArrangList, "ArrangementId", "ArrangementName");
            ViewBag.ObjectDelegationList = ObjectDelegationList;
            return PartialView(objMovementsAdvancedFilter);
        }
        #endregion

        #endregion

        #region private void GetPresetFilters()
        private void GetPresetFilters()
        {
            List<DropDown> objListDropDown = new List<DropDown>();
            DropDown objDropDown = null;
            objDropDown = new DropDown();
            objDropDown.Id = 0;
            objDropDown.Value = "No preset filter in use";
            objListDropDown.Add(objDropDown);
            objDropDown = new DropDown();
            objDropDown.Id = 1;
            objDropDown.Value = "Most recently submitted applications";
            objListDropDown.Add(objDropDown);
            objDropDown = new DropDown();
            objDropDown.Id = 2;
            objDropDown.Value = "Most recently submitted notifications";
            objListDropDown.Add(objDropDown);
            objDropDown = new DropDown();
            objDropDown.Id = 4;
            objDropDown.Value = "Movements within the next week";
            objListDropDown.Add(objDropDown);
            objDropDown = new DropDown();
            objDropDown.Id = 5;
            objDropDown.Value = "Movements within the next 30 days";
            objListDropDown.Add(objDropDown);

            int selected = 0;
            if (Session["g_MovementPresetFilter"] != null)
            {
                selected = (int)Session["g_MovementPresetFilter"];
            }

            ViewBag.PresetFilter = new SelectList(objListDropDown, "Id", "Value", selected);
        }
        #endregion

        #region private void GetComparisonDropDown()
        private void GetComparisonDropDown()
        {
            List<DropDown> objListDropDown = new List<DropDown>();
            DropDown objDropDown = null;
            objDropDown = new DropDown();
            objDropDown.Id = 1;
            objDropDown.Value = "less than or equal to";
            objListDropDown.Add(objDropDown);
            objDropDown = new DropDown();
            objDropDown.Id = 0;
            objDropDown.Value = "greater than or equal to";
            objListDropDown.Add(objDropDown);
            objDropDown = new DropDown();
            objDropDown.Id = 2;
            objDropDown.Value = "Between";
            objListDropDown.Add(objDropDown);


            List<FilterDropDown> objListFilterDropDown = new List<FilterDropDown>();
            FilterDropDown objFilterDropDown = null;
            objFilterDropDown = new FilterDropDown();
            objFilterDropDown.Id = "<=";
            objFilterDropDown.Value = "less than or equal to";
            objListFilterDropDown.Add(objFilterDropDown);
            objFilterDropDown = new FilterDropDown();
            objFilterDropDown.Id = ">=";
            objFilterDropDown.Value = "greater than or equal to";
            objListFilterDropDown.Add(objFilterDropDown);
            objFilterDropDown = new FilterDropDown();
            objFilterDropDown.Id = "between";
            objFilterDropDown.Value = "Between";
            objListFilterDropDown.Add(objFilterDropDown);

            int weightCount = 1;
            int widthCount = 1;
            int lengthCount = 1;
            int heightCount = 1;
            int axleCount = 1;
            int operatorCount = 1;

            if (Session["g_AdvancedSearchData"] != null)
            {
                MovementsAdvancedFilter objMovementsAdvancedFilter = (MovementsAdvancedFilter)Session["g_AdvancedSearchData"];
                weightCount = objMovementsAdvancedFilter.WeightCount;
                widthCount = objMovementsAdvancedFilter.WidthCount;
                lengthCount = objMovementsAdvancedFilter.LengthCount;
                heightCount = objMovementsAdvancedFilter.HeightCount;
                axleCount = objMovementsAdvancedFilter.AxleCount;
            }

            ViewBag.WeightCount = new SelectList(objListDropDown, "Id", "Value", weightCount);
            ViewBag.WidthCount = new SelectList(objListDropDown, "Id", "Value", widthCount);
            ViewBag.LengthCount = new SelectList(objListDropDown, "Id", "Value", lengthCount);
            ViewBag.HeightCount = new SelectList(objListDropDown, "Id", "Value", heightCount);
            ViewBag.AxleCount = new SelectList(objListDropDown, "Id", "Value", axleCount);
            ViewBag.OperatorCount = new SelectList(objListFilterDropDown, "Id", "Value", operatorCount);
        }
        #endregion

        #region private void ChangeFiltersPerPreset(int presetFilter)
        private void ChangeFiltersPerPreset(int presetFilter)
        {
            MovementsFilter objMovementFilter = new MovementsFilter();
            MovementsAdvancedFilter objMovementsAdvancedFilter = new MovementsAdvancedFilter();
            switch (presetFilter)
            {
                case 1:
                    objMovementFilter.Submitted = true;
                    break;
                case 2:
                    objMovementFilter.Notifications = true;
                    break;
                case 4:
                    objMovementFilter.Submitted = true;
                    objMovementFilter.ReceivedByHA = true;
                    objMovementFilter.Agreed = true;
                    objMovementFilter.ProposedRoute = true;
                    objMovementFilter.Notifications = true;
                    objMovementFilter.ApprovedVR1 = true;
                    //default selection of movement dates
                    objMovementsAdvancedFilter.SORTOrder = 8;
                    objMovementsAdvancedFilter.MovementDate = true;
                    objMovementsAdvancedFilter.MovementFromDate = DateTime.Now.ToString("dd/MM/yyyy");
                    objMovementsAdvancedFilter.MovementToDate = DateTime.Now.AddDays(7).ToString("dd/MM/yyyy");

                    break;
                case 5:
                    objMovementFilter.Submitted = true;
                    objMovementFilter.ReceivedByHA = true;
                    objMovementFilter.Agreed = true;
                    objMovementFilter.ProposedRoute = true;
                    objMovementFilter.Notifications = true;
                    objMovementFilter.ApprovedVR1 = true;
                    //default selection of movement dates
                    objMovementsAdvancedFilter.SORTOrder = 8;
                    objMovementsAdvancedFilter.MovementDate = true;
                    objMovementsAdvancedFilter.MovementFromDate = DateTime.Now.ToString("dd/MM/yyyy");
                    objMovementsAdvancedFilter.MovementToDate = DateTime.Now.AddDays(30).ToString("dd/MM/yyyy");
                    break;
                default:
                    objMovementFilter.IncludeHistoric = true;
                    objMovementFilter.NeedsAttention = true;
                    objMovementsAdvancedFilter.SORTOrder = 1;
                    break;
            }

            Session["g_SearchData"] = objMovementFilter;
            Session["g_AdvancedSearchData"] = objMovementsAdvancedFilter;
        }
        #endregion

        #region clear session for so
        private void setfilterforso(ref MovementsFilter objMovementFilter, ref MovementsAdvancedFilter objMovementsAdvancedFilter)
        {
            objMovementFilter.WorkInProgress = false;
            objMovementFilter.WorkInProgressApplication = false; // for Application
            objMovementFilter.WorkInProgressNotification = false;//for Notification
            objMovementFilter.Submitted = true;
            objMovementFilter.ReceivedByHA = true;
            objMovementFilter.WithdrawnApplications = false;
            objMovementFilter.DeclinedApplications = false;

            objMovementFilter.ProposedRoute = true;
            objMovementFilter.Notifications = false;
            objMovementFilter.ApprovedVR1 = false;

            //For Other Options
            objMovementFilter.NeedsAttention = false;
            objMovementFilter.MostRecentVersion = false;
            objMovementFilter.IncludeHistoric = false;
            //For Movement Type
            objMovementFilter.SO = false;
            objMovementFilter.VSO = false;
            objMovementFilter.STGO = false;
            objMovementFilter.CandU = false;
            objMovementFilter.Tracked = false;
            objMovementFilter.STGOVR1 = false;
            //for so agreed
            objMovementFilter.SO = true;
            objMovementFilter.Agreed = true;

            objMovementsAdvancedFilter.ESDALReference = null;
            objMovementsAdvancedFilter.MyReference = null;
            objMovementsAdvancedFilter.StartOrEnd = null;
            objMovementsAdvancedFilter.FleetId = null;
            objMovementsAdvancedFilter.Keyword = null;
            objMovementsAdvancedFilter.Client = null;
            objMovementsAdvancedFilter.ReceiptOrganisation = null;
            objMovementsAdvancedFilter.VehicleRegistration = null;
            objMovementsAdvancedFilter.GrossWeight = null;
            objMovementsAdvancedFilter.OverallWidth = null;
            objMovementsAdvancedFilter.Length = null;
            objMovementsAdvancedFilter.Height = null;
            objMovementsAdvancedFilter.AxleWeight = null;
            objMovementsAdvancedFilter.MovementFromDate = null;
            objMovementsAdvancedFilter.MovementToDate = null;
            objMovementsAdvancedFilter.ApplicationFromDate = null;
            objMovementsAdvancedFilter.ApplicationToDate = null;
            objMovementsAdvancedFilter.NotificationFromDate = null;
            objMovementsAdvancedFilter.NotificationToDate = null;
            objMovementsAdvancedFilter.WeightCount = 1;
            objMovementsAdvancedFilter.WidthCount = 1;
            objMovementsAdvancedFilter.LengthCount = 1;
            objMovementsAdvancedFilter.HeightCount = 1;
            objMovementsAdvancedFilter.AxleCount = 1;

            //dates checkbox
            objMovementsAdvancedFilter.MovementDate = false;
            objMovementsAdvancedFilter.ApplicationDate = false;
            objMovementsAdvancedFilter.NotifyDate = false;

            //For Sorting Order
            objMovementsAdvancedFilter.SORTOrder = 1;

        }
        #endregion

        #region private  List<MovementsList> GetMovementList()
        private List<MovementsList> GetMovementList(int organisationId, int pageNum, int pageSize, MovementsFilter objMovementFilter, MovementsAdvancedFilter objMovementsAdvancedFilter, int presetFilter, int ShowPrevSortRoute = 0, bool prevMovImport = false)
        {
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }
            if (SessionInfo.UserTypeId != 696008)
            {
                ShowPrevSortRoute = 0;
            }
            List<MovementsList> objMovementsList = movementsService.GetMovementsList(organisationId, pageNum, pageSize, objMovementFilter, objMovementsAdvancedFilter, presetFilter, SessionInfo.UserSchema, ShowPrevSortRoute, prevMovImport);
            if (objMovementsList != null)
            {
                foreach (var item in objMovementsList)
                {
                    if ((item.ColloborationStatus == 2 || item.ColloborationStatus == 1) && item.NotificationId > 0)
                    {
                        item.NeedAttentionFilterFlag = true;
                    }
                    if (item.Attention == 1)
                    {
                        item.NeedAttentionFilterFlag = true;
                    }
                    if (item.Status == "withdrawn")
                    {
                        item.VersionStatus = 305007;
                    }
                }
            }

            return objMovementsList;
        }
        #endregion

        #region private void GetComparisonDropDownPolice()
        private void GetComparisonDropDownPolice()
        {
            List<DropDown> objListDropDown = new List<DropDown>();
            DropDown objDropDown = null;
            objDropDown = new DropDown();
            objDropDown.Id = 1;
            objDropDown.Value = "less than or equal to";
            objListDropDown.Add(objDropDown);
            objDropDown = new DropDown();
            objDropDown.Id = 0;
            objDropDown.Value = "greater than or equal to";
            objListDropDown.Add(objDropDown);
            objDropDown = new DropDown();
            objDropDown.Id = 2;
            objDropDown.Value = "Between";
            objListDropDown.Add(objDropDown);

            List<FilterDropDown> objListFilterDropDown = new List<FilterDropDown>();
            FilterDropDown objFilterDropDown = null;
            objFilterDropDown = new FilterDropDown();
            objFilterDropDown.Id = "<=";
            objFilterDropDown.Value = "less than or equal to";
            objListFilterDropDown.Add(objFilterDropDown);
            objFilterDropDown = new FilterDropDown();
            objFilterDropDown.Id = ">=";
            objFilterDropDown.Value = "greater than or equal to";
            objListFilterDropDown.Add(objFilterDropDown);
            objFilterDropDown = new FilterDropDown();
            objFilterDropDown.Id = "between";
            objFilterDropDown.Value = "Between";
            objListFilterDropDown.Add(objFilterDropDown);

            int weightCount = 1;
            int widthCount = 1;
            int lengthCount = 1;
            int heightCount = 1;
            int axleCount = 1;
            int rigidLength = 1;
            int operatorCount = 1;

            MovementsInboxAdvancedFilter objMovementsAdvancedFilter = new MovementsInboxAdvancedFilter();
            if (Session["g_moveInboxAdvanceFilter"] != null)
            {
                objMovementsAdvancedFilter = (MovementsInboxAdvancedFilter)Session["g_moveInboxAdvanceFilter"];
                weightCount = objMovementsAdvancedFilter.WeightCount;
                widthCount = objMovementsAdvancedFilter.WidthCount;
                lengthCount = objMovementsAdvancedFilter.LengthCount;
                heightCount = objMovementsAdvancedFilter.HeightCount;
                axleCount = objMovementsAdvancedFilter.AxleCount;
                rigidLength = objMovementsAdvancedFilter.RigidLengthCount;

            }

            ViewBag.WeightCount = new SelectList(objListDropDown, "Id", "Value", weightCount);
            ViewBag.WidthCount = new SelectList(objListDropDown, "Id", "Value", widthCount);
            ViewBag.LengthCount = new SelectList(objListDropDown, "Id", "Value", lengthCount);
            ViewBag.HeightCount = new SelectList(objListDropDown, "Id", "Value", heightCount);
            ViewBag.AxleCount = new SelectList(objListDropDown, "Id", "Value", axleCount);
            ViewBag.RigidLengthCount = new SelectList(objListDropDown, "Id", "Value", rigidLength);
            ViewBag.OperatorCount = new SelectList(objListFilterDropDown, "Id", "Value", operatorCount);
        }
        #endregion

        #region private void FillDelegateArrange()
        private void FillDelegateArrange()
        {
            UserInfo SessionInfo = null;
            SessionInfo = (UserInfo)Session["UserInfo"];
            int organisationId = (int)SessionInfo.OrganisationId;
            List<DropDown> listDelegateNames = structuresService.GetDelegationArrangement(organisationId, "");
            ViewBag.DelegateArrange = listDelegateNames;
        }
        #endregion

        #region public ActionResult FilterMoveInbox(MovementsInboxFilter objMoveInboxFilter)
        public ActionResult FilterMoveInbox(MovementsInboxFilter objMoveInboxFilter, MovementsInboxAdvancedFilter objMoveAdvInboxFilter, int structureID = 0, string structureNm = "", string ESRN = "", int page = 1, int pageSize = 10)
        {
            if (objMoveInboxFilter.QueryString == null)
            {
                Session["g_moveInboxFilter"] = null;
                Session["g_moveInboxAdvanceFilter"] = null;
            }
            if (objMoveAdvInboxFilter.SortOrder == 0)
                objMoveAdvInboxFilter.SortOrder = 1;

            Session["g_moveInboxFilter"] = objMoveInboxFilter;
            Session["g_moveInboxAdvanceFilter"] = objMoveAdvInboxFilter;

            pageSize = ((Session["PageSize"] != null) && (Convert.ToString(Session["PageSize"]) != "0")) ? pageSize : 10;

            return RedirectToAction("PoliceMovementInbox", new
            {
                B7vy6imTleYsMr6Nlv7VQ =
                        STP.Web.Helpers.EncryptionUtility.Encrypt("page=" + page +
                        "&pageSize=" + pageSize +
                        "&structureID=" + structureID +
                        "&structureNm=" + structureNm +
                        "&ESRN=" + ESRN +
                        "&sortType=" + objMoveInboxFilter.SortTypeValue +
                        "&sortOrder=" + objMoveInboxFilter.SortOrderValue)
            });
        }
        #endregion

        #region public JsonResult ClearInboxFilter()  
        [HttpPost]
        public JsonResult ClearInboxFilter()
        {
            Session["g_moveInboxFilter"] = null;
            Session["g_moveInboxAdvanceFilter"] = null;
            MovementsInboxFilter obj = new MovementsInboxFilter();
            obj.Unopened = true;
            Session["g_moveInboxFilter"] = obj;
            return Json(new { result = 1 });
        }
        #endregion

        #region AuthorizeMovementGeneral()
        [HttpGet]
        public ActionResult AuthorizeMovementGeneral(string Notificationid = "", string esdal_ref = "", string route = "", string inboxId = "", string inboxItemStat = "", int FromInboxflag = 0, int historic = 0)
        {
            try
            {
                #region Session Check
                UserInfo SessionInfo = null;
                Session["RouteAssessmentFlag"] = "Completed";
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                SessionInfo = (UserInfo)Session["UserInfo"];
                if (SessionInfo.HelpdeskRedirect == "true")
                {
                    ViewBag.Helpdesk_redirect = SessionInfo.HelpdeskRedirect;
                }
                #endregion
                Session["RouteAssessmentFlag"] = "Completed";
                long NEN_ID = 0; //For NEN Project
                ViewBag.FromInboxflag = FromInboxflag;
                if (ModelState.IsValid)
                {
                    #region Page access check
                    #endregion

                    TempData["EncryptNotiId"] = Notificationid;
                    TempData["EncryptRoute"] = route;

                    TempData["EncryptEsdalRef"] = esdal_ref;
                    TempData["InBoxId"] = inboxId;

                    Notificationid = Notificationid.Replace(" ", "+");

                    long notifId = Convert.ToInt64(MD5EncryptDecrypt.DecryptDetails(Notificationid));

                    var soaPoliceNotifictaionManagement = new SOAPoliceNotificationManagement(soaPoliceWorkflowService);
                    var soaNotificationWorkflowRefrenceNumber = WorkflowTaskFinder.WorkflowSoaPoliceNotificationNumberBuilder(notifId.ToString(), false);
                    if (soaNotificationWorkflowRefrenceNumber.Length > 0
                        && !soaPoliceNotifictaionManagement.CheckIfProcessExit(soaNotificationWorkflowRefrenceNumber, startProcess: false))
                    {
                        new SOAPoliceNotificationManagement(soaPoliceWorkflowService).StartWorkflow(SessionInfo.OrganisationId, SessionInfo.OrganisationName, soaNotificationWorkflowRefrenceNumber != "", soaNotificationWorkflowRefrenceNumber, notifId);
                        if (WorkflowTaskFinder.FindNextTask("SOANotification", WorkflowActivityTypes.Sp_Activity_SelectaNotification, out dynamic workflowPayload, false) != string.Empty)
                        {

                            dynamic dataPayload = new ExpandoObject();
                            dataPayload.workflowActivityLog = soaPoliceNotifictaionManagement.SetWorkflowLog(WorkflowActivityTypes.Sp_Activity_SelectaNotification.ToString());
                            WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                            {
                                data = dataPayload,
                                workflowData = workflowPayload
                            };
                            soaPoliceNotifictaionManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel, true);
                        }
                    }
                    if (inboxItemStat != "")   // The check is added  for user clicks relative communication from authorize movment page(for avoiding duplicate vehicleconfig)
                    {
                        route = inboxItemStat;
                    }
                    else
                    {
                        route = route.Replace(" ", "+");
                        route = MD5EncryptDecrypt.DecryptDetails(route);
                    }

                    esdal_ref = esdal_ref.Replace(" ", "+");
                    esdal_ref = MD5EncryptDecrypt.DecryptDetails(esdal_ref);

                    inboxId = inboxId.Replace(" ", "+");
                    inboxId = MD5EncryptDecrypt.DecryptDetails(inboxId);

                    ViewBag.InboxId = inboxId;

                    int contactId = movementsService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));

                    long organisationId = SessionInfo.OrganisationId;

                    MovementModel movement = new MovementModel();
                    movement.VehicleConfigurations = new List<VehicleConfigration>();
                    //**********Required code*********
                    if (SessionInfo.HelpdeskRedirect != "true")
                    {
                        movementsService.EditInboxItemOpenStatus(Convert.ToInt64(inboxId), organisationId);
                    }
                    //***********************************

                    if (route.ToLower() == "no longer affected" && esdal_ref.IndexOf("#") != -1)
                    {
                        route = "no longer affected notification";
                    }

                    #region Application Movement Version
                    if (route.ToLower() == "reproposal" || route.ToLower() == "proposal" || route.ToLower() == "amendment to agreement"
                        || route.ToLower() == "nolonger affected" || route.ToLower() == "no longer affected" || route.ToLower() == "recleared" || route.ToLower() == "agreement" || route.ToLower() == "vrl planned route")
                    {
                        ViewBag.SortMovement = true; //Indicate this movement need to be taken from SORT

                        esdal_ref = esdal_ref.Replace("~", "#");
                        esdal_ref = esdal_ref.Replace("#", "/");
                        string[] esdalRefPro = esdal_ref.Split('/');
                        string mnemonic = string.Empty;
                        string esdalrefnum = string.Empty;
                        string version = string.Empty;

                        if (esdalRefPro.Length > 0)
                        {
                            mnemonic = Convert.ToString(esdalRefPro[0]);
                            esdalrefnum = Convert.ToString(esdalRefPro[1].ToUpper().Replace("S", ""));
                            version = Convert.ToString(esdalRefPro[2].ToUpper().Replace("S", ""));
                        }

                        movement = movementsService.GetAuthorizeMovementGeneralProposed(route, mnemonic, esdalrefnum, version, Convert.ToInt64(inboxId), esdal_ref, contactId, organisationId);
                        MovementModel movementForStatus = movementsService.GetCollaborationStatus(movement.DocumentId);
                        movement.Status = movementForStatus.Status;
                        movement.Route = route;
                        movement.InboxId = Convert.ToInt64(inboxId);

                        movement.ESDALReference = esdal_ref;

                        movement.NotificationCode = mnemonic + "/" + esdalrefnum + "/S" + Convert.ToString(version);

                        if (route.ToLower() != "proposal")
                        {
                            movement.OrderNumber = movementsService.GetSpecialOrderNo(movement.ESDALReference);
                        }
                        movement.ContactId = contactId;

                        #region HaContact Details
                        if (movement.HAContactDetails != null)
                        {
                            byte[] HAContactDetails = movement.HAContactDetails;
                            string docApp = Encoding.UTF8.GetString(HAContactDetails, 0, HAContactDetails.Length);
                            XmlDocument Doc = new XmlDocument();
                            try
                            {
                                Doc.LoadXml(docApp);
                            }
                            catch (System.Xml.XmlException xmlEx)
                            {
                                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movements/AuthorizeMovementGeneral, Exception: {0}", xmlEx));
                                HAContactDetails = STP.Common.General.XsltTransformer.Trafo(HAContactDetails);
                                docApp = Encoding.UTF8.GetString(HAContactDetails, 0, HAContactDetails.Length);
                                Doc.LoadXml(docApp);
                            }
                            if (Doc.GetElementsByTagName("HAContact") != null &&
                                    Doc.GetElementsByTagName("HAContact").Item(0) != null &&
                                    Doc.GetElementsByTagName("HAContact").Item(0)["Contact"] != null)
                            {
                                movement.HAContact = Doc.GetElementsByTagName("HAContact").Item(0)["Contact"].InnerText;
                            }
                            if (Doc.GetElementsByTagName("DistributionComments") != null &&
                                    Doc.GetElementsByTagName("DistributionComments").Item(0) != null)
                            {
                                movement.Notes = Doc.GetElementsByTagName("DistributionComments").Item(0).InnerText;
                            }
                        }
                        #endregion

                        movement.VehicleConfigurations = movementsService.GetVehiclesList(mnemonic, esdalrefnum, version, notifId, 0);
                        // get contact id by haulier name
                        MovementModel newMovement = movementsService.GetHAAndHaulierContactIdByName(movement);
                        movement.HaulierContactId = newMovement.HaulierContactId;
                        movement.HAContactId = newMovement.HAContactId;
                        //Get contact id end
                        ViewBag.Notificationid = 0;

                        //get document id
                        if (movement.DocumentId == 0)
                        {
                            movement.DocumentId = movementsService.GetDocumentID(esdal_ref, SessionInfo.OrganisationId);
                        }
                        //get document id

                        //get collaboration notes based upon documentID
                        if (movement.CollaborationNotes == null || movement.CollaborationNotes.Count == 0)
                        {
                            movement.CollaborationNotes = movementsService.GetCollaborationNotes(movement.DocumentId, SessionInfo.OrganisationId);
                        }
                        movement.ESDALReference = esdal_ref;

                        if (movement.AuthenticationNotesToHaulier != null)
                        {
                            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\SOGeneral.xslt");
                            string result = Common.General.XsltTransformer.Trafo(movement.AuthenticationNotesToHaulier, path, out string errormsg);
                            result = Regex.Replace(result, @"<(\/)?(html|body)([^>]*)>", "");
                            movement.AuthenticationNotesToHaulierContent = result;
                        }
                    }
                    #endregion

                    #region Notification
                    else if (route != string.Empty)
                    {
                        if (route.ToLower() == "no longer affected notification")
                        {
                            route = "no longer affected";
                        }

                        ViewBag.Notificationid = notifId;
                        ViewBag.DecrpNotifiId = Notificationid;

                        //get main data
                        movement = movementsService.GetAuthorizeMovementGeneral(notifId, Convert.ToInt64(inboxId), contactId, esdal_ref, organisationId);
                        movement.Route = route;
                        movement.ContactId = contactId;
                        movement.NotificationId = notifId;
                        movement.ESDALReference = esdal_ref;
                        if (route.ToLower() == "ne notification api" || route.ToLower() == "ne renotification api")
                            movement.VehicleConfigurations = vehicleconfigService.GetNenApiVehiclesList(notifId, organisationId, UserSchema.Portal);
                        else
                            movement.VehicleConfigurations = movementsService.GetVehiclesList(null, null, null, notifId, 0);

                        if (movement.HAJobFileReference != null && movement.HAJobFileReference != string.Empty)
                        {
                            Session["HAReferenceNo"] = movement.HAJobFileReference;
                        }

                        #region Dispensation
                        if (movement.DispensationIds != null && movement.DispensationIds.Length > 0)
                        {
                            var dispensationIds = JsonConvert.DeserializeObject<List<long>>(movement.DispensationIds);
                            if (dispensationIds != null && dispensationIds.Any())
                            {
                                movement.DispensationList = new List<Dispensations>();
                                List<Domain.MovementsAndNotifications.Notification.NotifDispensations> notifDispensations = documentService.GetNotificationDispensation(notifId, historic);
                                Dispensations dispensations;
                                if (notifDispensations != null)
                                {
                                    notifDispensations = notifDispensations.Where(x => x.GrantorId == (int)SessionInfo.OrganisationId).ToList();
                                    foreach (var item in notifDispensations)
                                    {
                                        dispensations = new Dispensations
                                        {
                                            DRN = item.DRN,
                                            Summary = item.Summary
                                        };
                                        movement.DispensationList.Add(dispensations);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region For HAContactDetails
                        if (esdal_ref != null && esdal_ref.Contains("/S"))
                        {
                            byte[] HAContactDetails = movement.HAContactDetails;

                            if (HAContactDetails != null)
                            {
                                string docApp = Encoding.UTF8.GetString(HAContactDetails, 0, HAContactDetails.Length);

                                XmlDocument Doc = new XmlDocument();
                                try
                                {
                                    Doc.LoadXml(docApp);
                                }
                                catch (System.Xml.XmlException xmlEx)
                                {
                                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movements/AuthorizeMovementGeneral, Exception: {0}", xmlEx));
                                    HAContactDetails = STP.Common.General.XsltTransformer.Trafo(HAContactDetails);
                                    docApp = Encoding.UTF8.GetString(HAContactDetails, 0, HAContactDetails.Length);
                                    Doc.LoadXml(docApp);
                                }

                                if (Doc.GetElementsByTagName("HAContact") != null &&
                                    Doc.GetElementsByTagName("HAContact").Item(0) != null &&
                                    Doc.GetElementsByTagName("HAContact").Item(0)["Contact"] != null)
                                {
                                    movement.HAContact = Doc.GetElementsByTagName("HAContact").Item(0)["Contact"].InnerText;
                                }
                                movement.HAContactId = movementsService.GetHAAndHaulierContactIdByName(movement).HAContactId;
                            }
                            else
                            {
                                movement.HAContact = string.Empty;
                                movement.HAContactId = 0;
                            }
                        }
                        #endregion

                        movement.HaulierContactId = movementsService.GetHaulierContactId(movement).HaulierContactId;

                        movement.CollaborationNotes = movementsService.GetCollaborationNotes(movement.DocumentId, SessionInfo.OrganisationId);
                    }
                    #endregion

                    //RM#3919 - start
                    ViewBag.IsNotification = true;
                    //RM#3919 - end

                    // Business logic is to be added to update opened / unopened flag in Distribution status table
                    movementsService.UpdateInboxItemStatus(SessionInfo.OrganisationId, esdal_ref);
                    // Business logic ends here

                    //get Special Orders and related communication
                    if (movement.NotificationCode != null && movement.NotificationCode != string.Empty)
                    {
                        string[] esdalRef;

                        esdalRef = movement.NotificationCode.Split('#');

                        if (esdalRef.Length > 0)
                        {
                            if (!movement.NotificationCode.ToString().Contains("#")) // Condition kept to skip the GetSpecialOrders SP call when it is a notification movement
                            {
                                movement.SpecialOrders = movementsService.GetSpecialOrders(esdalRef[0]);
                            }

                            //get related communication by notification code
                            string[] notificationcode = esdalRef[0].Split('/');

                            movement.RelatedCommunications = movementsService.GetNotificationDetailsByCode(notificationcode[0] + "/" + notificationcode[1] + "/", route, SessionInfo.OrganisationId, movement.ProjectId);
                            //RM#3919 - start
                            var NotificationStatus = movement.RelatedCommunications.SkipWhile(x => x.NotificationCode != movement.NotificationCode).Skip(1).FirstOrDefault();

                            if (NotificationStatus != null
                                && (NotificationStatus.ItemStatus.ToLower() == "nolonger affected"))//|| NotificationStatus.ITEM_STATUS == string.Empty))
                            {
                                ViewBag.RouteNotification = true;
                                if (ViewBag.IsNotification != null && ViewBag.IsNotification)
                                {
                                    ViewBag.NextNoLongerAffected = true;
                                }
                            }
                            //RM#3919 - end
                            if (!movement.NotificationCode.Contains('#'))
                            {
                                movement.RelatedCommunications.RemoveAll(r => r.NotificationCode.Contains('#'));
                            }
                            movement.RelatedCommunications.RemoveAll(r => r.NotificationCode.Equals(movement.NotificationCode));

                            movement.RelatedCommunications.ForEach(r => r.EncrapNotificationId = (MD5EncryptDecrypt.EncryptDetails(Convert.ToString(r.NotificationId))));
                            movement.RelatedCommunications.ForEach(r => r.EncryptESDALReference = (MD5EncryptDecrypt.EncryptDetails(Convert.ToString(r.NotificationCode))));
                        }
                    }

                    if (movement.NotificationId == 0 && route.ToLower() != "vrl planned route")
                    {
                        if (movement.RelatedCommunications != null && movement.RelatedCommunications.Count > 0)
                        {
                            string[] splitNotificationCode = movement.NotificationCode.Split('/');

                            if (splitNotificationCode != null && splitNotificationCode[2] != null)
                            {
                                string versionDetails = splitNotificationCode[2];

                                string[] lastVersion = versionDetails.Split('S');

                                if (lastVersion != null && lastVersion.Length > 1 && lastVersion[1] != null)
                                {
                                    int number = Convert.ToInt32(lastVersion[1]) + 1;

                                    versionDetails = "S" + number;

                                    var objRelCommunication = movement.RelatedCommunications.Find(r => r.NotificationCode.Equals(splitNotificationCode[0] + "/" + splitNotificationCode[1] + "/" + versionDetails));

                                    if (objRelCommunication != null && objRelCommunication.NotificationCode != string.Empty)
                                    {
                                        movement.IsMostRecent = 0;
                                    }
                                    else
                                    {
                                        movement.IsMostRecent = 1;
                                    }
                                }
                            }
                        }
                        else if (movement.RelatedCommunications != null && movement.RelatedCommunications.Count == 0)
                        {
                            movement.IsMostRecent = 1;
                        }
                    }

                    //get Special Orders and related communication end

                    //Get VR1 data
                    if (!string.IsNullOrEmpty(movement.VR1Number))
                    {
                        movement.VR1s = movementsService.GetVR1s(movement.VR1Number);

                        if (movement.VR1s.Count == 0)
                        {
                            List<VR1> vr1 = new List<VR1>();

                            vr1.Add(new VR1() { VR1Number = movement.VR1Number, ScottishVR1Number = string.Empty, VR1Id = 0 });

                            movement.VR1s = vr1;
                        }
                    }

                    if (SessionInfo.HelpdeskRedirect != "true")
                    {
                        movementsService.InsertQuickLinkSOA(Convert.ToInt32(SessionInfo.OrganisationId), Convert.ToInt32(inboxId), Convert.ToInt32(SessionInfo.UserId));
                    }

                    //Get VR1 data end
                    ViewBag.Content_ref_no = "0";
                    if (movement.NotificationId != 0)
                    {
                        ViewBag.Content_ref_no = movementsService.GetContentReferenceNo((int)movement.NotificationId);
                        //------IZ NEN PART--------
                        NEN_ID = nenNotificationService.GetNENId((int)movement.NotificationId);
                        if (NEN_ID > 0)
                        {
                            ViewBag.Nen_Id = NEN_ID;
                            ViewBag.IsNen = true;
                            Session["NENINBOX_ITEM_ID"] = inboxId;
                        }
                        //-------------------------
                    }
                    #region-------Intellizenz part for release 2----------------
                    if (SessionInfo.HelpdeskRedirect == "true")
                    {
                        if (SessionInfo.UserName != null)
                        {
                            #region sys_events for saving loggin info for helpdesk redirect SOA, POLICE
                            MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                            string ErrMsg = string.Empty;
                            if (SessionInfo.UserSchema == UserSchema.Portal)
                            {
                                if (SessionInfo.UserTypeId == 696007)
                                {
                                    movactiontype.SystemEventType = SysEventType.Check_as_SOA;
                                }
                                else
                                {
                                    movactiontype.SystemEventType = SysEventType.Check_as_Police;
                                }
                                movactiontype.UserId = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                                movactiontype.UserName = SessionInfo.HelpdeskUserName;
                                movactiontype.ESDALRef = esdal_ref;
                                string sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                                int userId = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                                //loggingService.SaveSysEventsMovement(movactiontype, sysEventDescp, userId, UserSchema.Portal);
                            }

                            #endregion
                        }
                    }
                    #endregion---------------end--------------------------------

                    //-----INTELLIZENZ PART FOR NEN NOTIFICATIONS AUDIT LOG SAVING-----------
                    #region-------INTELLIZENZ PART FOR NEN NOTIFICATIONS AUDIT LOG SAVING----------------
                    if (SessionInfo.UserName != null && (route == "NE Notification" || route == "NE Renotification"))
                    {
                        #region AUDIT LOGS FOR SAVING WHEN OPEN ESDAL/NEN NOTIFICATIONS BY SOA OR POLICE OR THROUGH HELPDESK
                        AuditLogIdentifiers auditLogType = new AuditLogIdentifiers();
                        string ErrMsg = string.Empty;
                        auditLogType.NENID = NEN_ID > 0 ? NEN_ID : 0;
                        if (SessionInfo.UserSchema == UserSchema.Portal)
                        {
                            int confirmData = Session["ConfirmRouteVeh"] != null ? Convert.ToInt32(Session["ConfirmRouteVeh"]) : 0;// Checking ConfirmRouteVeh session value to avoid inserting duplicate confim route and vehicle logs
                            if (NEN_ID > 0 && (route == "NE Notification" || route == "NE Renotification") && confirmData == 1)
                            {
                                auditLogType.AuditActionType = AuditActionType.NEN_notification_confirmed_by_user;
                                auditLogType.HelpDeskUserID = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                                auditLogType.HelpDeskUsername = SessionInfo.HelpdeskUserName;
                                auditLogType.NENNotificationNo = esdal_ref;
                                auditLogType.ESDALNotificationNo = esdal_ref;
                                auditLogType.InboxItemId = inboxId;
                                Session["ConfirmRouteVeh"] = null;
                                auditLogType.DateTime = DateTime.Now.ToString();
                                string auditLogDescp = AuditLog.GetNENNotifAuditLog(SessionInfo, auditLogType, out ErrMsg);
                                int user_ID = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                                auditlogService.SaveNotifAuditLog(auditLogType, auditLogDescp, user_ID, SessionInfo.OrganisationId);

                            }
                        }

                        #endregion
                    }
                    #endregion---------------end--------------------------------
                    //------END HERE---------------------------------------------------------
                    if (route == "NE agreed notification" || route == "NE Notification" || route == "NE Renotification")
                    {
                        if (movement.NotificationId != 0)
                        {
                            //------IZ NEN PART--------
                            NEN_ID = nenNotificationService.GetNENId((int)movement.NotificationId);
                            movement.VehicleConfigurations = movementsService.GetNENVehicleList(NEN_ID, Convert.ToInt64(inboxId), SessionInfo.OrganisationId);
                            //movement.IndemnityConfirmation = Doc.GetElementsByTagName("IndemnityConfirmation") == null ? string.Empty : (Doc.GetElementsByTagName("IndemnityConfirmation")[0] == null ? string.Empty : (Doc.GetElementsByTagName("IndemnityConfirmation")[0].Attributes[0].Value == null || Doc.GetElementsByTagName("IndemnityConfirmation")[0].Attributes[0].Value == string.Empty ? "No" : (Doc.GetElementsByTagName("IndemnityConfirmation")[0].Attributes[0].Value == "true" ? "Yes" : "No")));
                        }
                    }

                    //RM#3919 - start
                    TempData.Keep("EncryptNotiId");
                    TempData.Keep("EncryptRoute");
                    TempData.Keep("EncryptEsdalRef");
                    TempData.Keep("InBoxId");
                    TempData.Keep("PageLoadSecondTime");
                    if (movement.VehicleConfigurations == null)
                    {
                        movement.VehicleConfigurations = new List<VehicleConfigration>();
                    }

                    //RM#3919 - end
                    foreach (var vehicle in movement.VehicleConfigurations)
                    {
                        vehicle.VehicleCompList = vehicle.VehicleCompList.OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                    }
                    GetVehicleImage(movement.VehicleConfigurations);
                    ViewBag.AuthorizeMovementGeneral = true;
                    ViewBag.IsHistoric = historic;
                    Session["NENINBOX_ITEM_ID"] = inboxId;
                    Session["NENesdal_ref"] = esdal_ref;

                    return View("AuthorizeMovementGeneral", movement);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movements/AuthorizeMovementGeneral, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movements/AuthorizeMovementGeneral, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region IndemnityConfirmation
        [HttpGet]
        public ActionResult IndemnityConfirmation(long notificationId, bool isView = false)
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");
                    return RedirectToAction("Login", "Account");
                }
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                IndemnityConfirmation indemnityConfirmation = new IndemnityConfirmation();
                indemnityConfirmation.OrganisationName = SessionInfo.OrganisationName;
                PlanMovementType planMovementType = applicationService.GetNotificationDetails(notificationId, SessionInfo.UserSchema);
                indemnityConfirmation.HaulierContact = planMovementType.HaulierContact;
                indemnityConfirmation.HaulierName = planMovementType.HaulierName;
                indemnityConfirmation.SentDateTime = planMovementType.NotificationDate;
                if (isView)
                    return View("PrintIndemnityConfirmation", indemnityConfirmation);
                else
                {
                    string html = RazorViewToString.RenderRazorViewToString(this, "PrintIndemnityConfirmation", indemnityConfirmation);
                    var model = new HtmlDocumentParams() { InputHtmlString = html };
                    var notificationDocument = documentService.GeneratePDFFromHtmlString(model);
                    if (notificationDocument != null)
                    {
                        System.IO.MemoryStream pdfStream = new System.IO.MemoryStream(notificationDocument);
                        return new FileStreamResult(pdfStream, "application/pdf");
                    }
                }
                //return new Rotativa.ViewAsPdf("PrintIndemnityConfirmation", indemnityConfirmation);
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/IndemnityConfirmation, Exception: {0}", ex));
                throw ex;
            }
            return null;

        }

        #endregion

        #region ViewCollaborationStatusAndNotes
        /// <summary>
        /// View collaboration status and notes
        /// </summary>
        /// <param name="Notificationid">Notification id</param>
        /// <returns>Details of collaboration</returns>
        [HttpGet]
        public ActionResult ViewCollaborationStatusAndNotes(long Notificationid, long documentid, long inBoxId, long analysisId, string EMail, string esdalRef, long contactId, string route, byte IS_MOST_RECENT, string routeOriginal = "", bool NextNoLongerAffected = false, int LibraryNotesId = 0, int WipNENCollab = 0, long NEN_ID = 0)//RM#3919 -- add routeOriginal and NextNoLongerAffected 
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Movements/ViewCollaborationStatusAndNotes actionResult method started successfully"));

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                if (ModelState.IsValid)
                {
                    EMail = Regex.Replace(EMail, @"\s+", " "); //replaced spaces in mail as it was giving error. here again added spaces.
                    esdalRef = esdalRef.Replace("~", "#");
                    ViewBag.Notificationid = Notificationid;
                    ViewBag.documentid = documentid;

                    MovementModel movement = movementsService.GetCollaborationStatus(documentid);
                    movement.DocumentId = documentid;
                    movement.NotificationId = Notificationid;
                    movement.InboxId = inBoxId;
                    movement.OrganisationId = SessionInfo.OrganisationId;
                    movement.ESDALReference = esdalRef;
                    movement.AnalysisId = analysisId;
                    //get notes by document id
                    movement.CollaborationNotes = movementsService.GetCollaborationNotes(movement.DocumentId, SessionInfo.OrganisationId);
                    movement.EmailAddress = EMail;
                    if (LibraryNotesId != 0)
                    {
                        List<STP.Domain.RouteAssessment.LibraryNotes> result = routeAssessmentService.GetLibraryNotes((int)SessionInfo.OrganisationId, LibraryNotesId, Convert.ToInt32(SessionInfo.UserId));
                        movement.Notes = result[0].Notes;
                    }
                    //send Haulier mail
                    ViewBag.Subject = "Collaboration note on movement " + esdalRef.Replace("#", "%23");
                    //send Haulier mail end
                    ViewBag.route = Convert.ToString(route).Replace(" ", "").Trim().ToLower();
                    ViewBag.latestversion = IS_MOST_RECENT == null ? 0 : IS_MOST_RECENT;
                    //RM#3919 - start
                    TempData["EncryptNotiId"] = MD5EncryptDecrypt.EncryptDetails(Convert.ToString(Notificationid));
                    TempData["EncryptRoute"] = MD5EncryptDecrypt.EncryptDetails(Convert.ToString(routeOriginal.Replace("_", " ")));
                    TempData["EncryptEsdalRef"] = MD5EncryptDecrypt.EncryptDetails(Convert.ToString(esdalRef));
                    TempData["InBoxId"] = MD5EncryptDecrypt.EncryptDetails(Convert.ToString(inBoxId));

                    TempData.Keep("EncryptNotiId");
                    TempData.Keep("EncryptRoute");
                    TempData.Keep("EncryptEsdalRef");
                    TempData.Keep("InBoxId");
                    ViewBag.NextNoLongerAffected = NextNoLongerAffected;
                    ViewBag.WipNENCollab = WipNENCollab;
                    //RM#3919 - end
                    //-----NEN part 11-09-17-------
                    //NEN_ID = nenNotificationService.GetNENId((int)Notificationid);

                    List<Domain.MovementsAndNotifications.Notification.OrganisationUser> UserList = nenNotificationService.GetOrg_UserList(SessionInfo.OrganisationId, SessionInfo.UserTypeId, inBoxId, NEN_ID);
                    SelectList GridListObjInfo = new SelectList(UserList, "OrganisationUserId", "OrganisationUserName");
                    ViewBag.UserListInfo = GridListObjInfo;

                    ViewBag.User_id = movement.UserId;

                    ViewBag.NEN_ID = NEN_ID;
                    //--------------------
                    return PartialView(movement);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movements/ViewCollaborationStatusAndNotes, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movements/ViewCollaborationStatusAndNotes, Exception: {0}", ex.Message));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region SaveToLibrary

        [HttpGet]
        public JsonResult SaveToLibrary(string Notes, string userSchema)
        {
            try
            {

                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];

                int result = 0;
                STP.Domain.RouteAssessment.LibraryNotes libraryNotes = new Domain.RouteAssessment.LibraryNotes();
                libraryNotes.Notes = Notes;
                libraryNotes.OrganisationId = SessionInfo.OrganisationId;
                libraryNotes.UserId = long.Parse(SessionInfo.UserId);
                if (Notes != "")
                {
                    result = routeAssessmentService.InsertLibraryNotes(libraryNotes);

                }
                return Json(result > 0 ? "true" : "false");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/SaveToLibrary, Exception: {0}", ex));
                throw ex;
            }

        }
        #endregion

        #region ManageMovementStatus
        /// <summary>
        /// Update movement status
        /// </summary>
        /// <param name="movement">MovementModel object</param>
        /// <returns>Return true or false</returns>
        [HttpPost]
        public JsonResult ManageMovementStatus(MovementModel movement)
        {
            try
            {
                int projectId = 0;
                int VersionNo = 0;
                int revisionNo = 0;
                UserInfo UserSessionValue = (UserInfo)Session["UserInfo"];

                bool result = false;
                if (movement.NenFlag)
                {
                    STP.Domain.RouteAssessment.LibraryNotes libraryNotes = new Domain.RouteAssessment.LibraryNotes();
                    libraryNotes.Notes = movement.Notes;
                    libraryNotes.OrganisationId = UserSessionValue.OrganisationId;
                    libraryNotes.UserId = long.Parse(UserSessionValue.UserId);
                    if (movement.Notes != null)
                    {
                        int output = routeAssessmentService.InsertLibraryNotes(libraryNotes);
                        if (output == 0)
                            return Json(result == true ? "true" : "false");
                    }
                }
                result = movementsService.ManageCollaborationStatus(movement);

                if (result == true)
                {
                    //-----NEN PART---------
                    if (movement.WIPNENProcess == 0 && movement.NenId > 0)//In case of collaborate accepted and rejected from NEN general tab without planning route then WIP_NEN_PROCESS value be 1, hense it should update in INBOX_REVISION table
                    {
                        movement.LoginUserId = Convert.ToInt64(UserSessionValue.UserId);
                        movement.OrganisationId = Convert.ToInt64(UserSessionValue.OrganisationId);
                        bool saveUser = nenNotificationService.SP_SAVE_NEN_USER_FOR_SCRUTINY(movement);
                    }
                    string ErrMsg = string.Empty;
                    //-----END HERE---------
                    if (!string.IsNullOrEmpty(movement.Notes))
                    {
                        MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                        movactiontype.MovementActionType = MovementnActionType.notification_collaboration_made;
                        movactiontype.CollaborationStatus = Convert.ToInt64(movement.Status);
                        movactiontype.CollaborationNotes = movement.Notes;
                        movactiontype.ESDALRef = movement.ESDALReference;
                        movactiontype.NotificationCode = movement.ESDALReference;
                        movactiontype.UserName = UserSessionValue.UserName;


                        string MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                        long res = loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, projectId, revisionNo, VersionNo, UserSessionValue.UserSchema);
                    }
                    //-----INTELLIZENZ PART FOR NEN NOTIFICATIONS AUDIT LOG SAVING-----------
                    #region-------INTELLIZENZ PART FOR NEN NOTIFICATIONS AUDIT LOG SAVING----------------
                    if (UserSessionValue.UserName != null)
                    {
                        #region AUDIT LOGS FOR SAVING Collaboration notes ESDAL/NEN NOTIFICATIONS BY SOA OR POLICE OR THROUGH HELPDESK
                        AuditLogIdentifiers auditLogType = new AuditLogIdentifiers();
                        int NEN_ID = 0;
                        auditLogType.NENID = movement.NenId > 0 ? movement.NenId : 0;
                        if (UserSessionValue.UserSchema == UserSchema.Portal)
                        {
                            if (movement.Status == 327001)
                            {
                                auditLogType.AuditActionType = AuditActionType.soauser_set_accepted_collab;//"ACCEPTED"
                            }
                            else if (movement.Status == 327002)
                            {
                                auditLogType.AuditActionType = AuditActionType.soauser_set_rejected_collab;//"REJECTED"
                            }
                            else if (movement.Status == 327003)
                            {
                                auditLogType.AuditActionType = AuditActionType.soauser_set_underasmt_collab_with_scrutiny;//"UNDER ASSESSMENT"
                                auditLogType.NENToScrutinyUser = movement.HaulierName;
                            }
                            else
                            {
                                auditLogType.AuditActionType = AuditActionType.soauser_set_noaction_collab;//"No action"
                            }
                            auditLogType.HelpDeskUserID = Convert.ToInt32(UserSessionValue.HelpdeskUserId);
                            auditLogType.HelpDeskUsername = UserSessionValue.HelpdeskUserName;
                            auditLogType.ESDALNotificationNo = movement.ESDALReference;
                            auditLogType.InboxItemId = movement.InboxId.ToString();
                            auditLogType.CollabrationNotes = movement.Notes;
                            var ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                            auditLogType.DateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, ukTimeZone).ToString("dd/MM/yyyy HH:mm:ss");
                            auditLogType.MailedCollabration = movement.MailedCollab;
                            string auditLogDescp = AuditLog.GetNENNotifAuditLog(UserSessionValue, auditLogType, out ErrMsg);
                            int user_ID = Convert.ToInt32(UserSessionValue.HelpdeskUserId);
                            long auditLogResult = auditlogService.SaveNotifAuditLog(auditLogType, auditLogDescp, user_ID, UserSessionValue.OrganisationId);
                        }

                        #endregion
                    }
                    #endregion---------------end--------------------------------
                    //------END HERE---------------------------------------------------------
                }
                //RM#3919 - start
                TempData["PageLoadSecondTime"] = true;
                TempData.Keep("PageLoadSecondTime");
                //RM#3919 - end
                return Json(result == true ? "true" : "false");
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/ManageMovementStatus, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion

        #region GetStructuresAndRoad
        public JsonResult GetStructuresAndRoad(int structureFlag, sdogeometry areaGeom)
        {
            SortMapFilter objFilterParams = new SortMapFilter();
            objFilterParams.Flag = structureFlag;
            objFilterParams.Geometry = areaGeom;

            List<MapStructLink> FilterResult = movementsService.GetStructLinkId(objFilterParams);
            var jsonResult = Json(FilterResult, JsonRequestBehavior.AllowGet);

            List<long> structures = new List<long>();
            for (int i = 0; i < FilterResult.Count; i++)
            {
                if (FilterResult[i].StructureId != 0)
                {
                    structures.Add(FilterResult[i].StructureId);
                }
            }
            structures = structures.Distinct().ToList();


            string Structurestring = "";
            int structurecount = 0;
            if (structureFlag == 1)
            {
                for (int i = 0; i < structures.Count; i++)
                {
                    //if (FilterResult[i].StructureId != 0)
                    //{
                    if (i < structures.Count - 1)
                    {
                        Structurestring = Structurestring + structures[i] + ",";
                    }
                    else
                    {
                        Structurestring = Structurestring + structures[i];

                    }
                    structurecount++;
                    //}
                }
                if (structurecount > 0)
                {
                    Session["SORTFilterStructCount"] = structurecount;
                }
                if (Structurestring != "")
                {
                    Session["SORTFilterStructures"] = Structurestring;

                }
            }
            return jsonResult;

        }
        #endregion

        #region Code Commetned By Mahzeer on 11/12/203
        /*
        public ActionResult AgreedRouteV(long notificationId, string esdalRefno, string classification, string userType, string DRN, int ISNENVal = 0,
            long NENInboxId = 0, long UserTypeId = 0, string UserId = "", int OrganisationId = 0, string OrganisationName = "")
        {

            try
            {
                int contactId = movementsService.GetContactDetails(Convert.ToInt32(UserId));

                string xmlInformation = string.Empty;

                xmlInformation = movementsService.PrintReport(notificationId);


                //===============================================================================================
                xmlInformation = RemoveDispansationNode_of_Outboundxml(xmlInformation, DRN);
                //-----------------------------------------------------------------------------------------------

                if (xmlInformation != string.Empty)
                {
                    xmlInformation = xmlInformation.Replace(">?<", ">\u2002<");
                    xmlInformation = xmlInformation.Replace(">?##**##", ">##**##");

                    xmlInformation = xmlInformation.Replace("<OnBehalfOf xmlns=\"http://www.esdal.com/schemas/core/movement\"> </OnBehalfOf>", "");
                    xmlInformation = xmlInformation.Replace("<OnBehalfOf> </OnBehalfOf>", "");
                }

                byte[] notificationDocument = null;
                string xsltPath = string.Empty;

                if (xmlInformation != string.Empty)
                {
                    //TODO for NEN project 
                    if (ISNENVal > 0)
                    {
                        xmlInformation = appendNENRouteDetails(xmlInformation, notificationId, NENInboxId, (int)OrganisationId, ISNENVal, string.Empty);
                    }
                    if (userType == "Police")
                    {
                        if (classification == "STGO")
                        {
                            if (ISNENVal > 0)
                            {
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\NEN_STGOReducedNotificationPolice.xslt";
                            }
                            else
                            {
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\STGOReducedNotificationPolice.xslt";
                            }

                            xmlInformation = appendRouteDetails(xmlInformation, notificationId, ISNENVal, NENInboxId, (int)OrganisationId);//TODO chirag
                            notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", 692001, "PDF", null, userType);
                        }
                        else if (classification == "CandU")
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\PoliceNotificationReducedCandU.xslt";
                            xmlInformation = appendRouteDetails(xmlInformation, notificationId, ISNENVal, NENInboxId, (int)OrganisationId);//TODO chirag
                            notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", 692001, "PDF", null, userType);
                        }
                        else
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\NotificationForSpecialOrderPolice.xslt";
                            xmlInformation = appendRouteDetails(xmlInformation, notificationId, ISNENVal, NENInboxId, (int)OrganisationId);//TODO chirag
                            notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", 692001, "PDF", null, userType);
                        }
                    }
                    else
                    {

                        if (classification == "STGO")
                        {
                            xmlInformation = GetLoggedInUserAffectedStructureDetails(xmlInformation, notificationId, NENInboxId, 0, OrganisationId, (int)UserTypeId);
                            if (ISNENVal > 0)
                            {
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\NEN_STGOReducedNotificationSOA.xslt";
                            }
                            else
                            {
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\STGOReducedNotificationSOA.xslt";
                            }
                            xmlInformation = appendRouteDetails(xmlInformation, notificationId, ISNENVal, NENInboxId, (int)OrganisationId);//TODO chirag
                            notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, OrganisationName);
                            string someString = Encoding.ASCII.GetString(notificationDocument);

                        }
                        else if (classification == "CandU")
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\PoliceNotificationReducedCandU.xslt";
                            xmlInformation = appendRouteDetails(xmlInformation, notificationId, ISNENVal, NENInboxId, (int)OrganisationId);//TODO chirag
                            notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "");
                        }
                        else
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\NotificationForSpecialOrder.xslt";
                            xmlInformation = appendRouteDetails(xmlInformation, notificationId, ISNENVal, NENInboxId, (int)OrganisationId);//TODO chirag
                            //notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "");
                        }
                    }
                }

                byte[] byteArray = Encoding.UTF8.GetBytes(xmlInformation);
                //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
                MemoryStream stream = new MemoryStream(byteArray);



                //read configuration xml
                XmlSerializer deserializer = new XmlSerializer(typeof(OutboundNotification));
                OutboundNotification agreedRoute = new OutboundNotification();

                string xsltPath1 = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\data2.xml";

                using (XmlReader reader = XmlReader.Create(stream))
                {
                    agreedRoute = (OutboundNotification)deserializer.Deserialize(reader);
                }
                if (agreedRoute.RouteParts != null)
                {
                    string singleLine = ReplaceWhitespace(agreedRoute.RouteParts.RoutePartListPosition.Route, "");

                    agreedRoute.RouteParts.RoutePartListPosition.Route = singleLine;

                    string RouteImperial = ReplaceWhitespace(agreedRoute.RouteParts.RoutePartListPosition.RouteImperial, "");
                    agreedRoute.RouteParts.RoutePartListPosition.RouteImperial = RouteImperial;
                }

                return View(agreedRoute);
            }
            catch (Exception ex)
            {

            }

            return View("");
        }
        
        public static string ReplaceWhitespace(string input, string replacement)
        {
            string resultString = Regex.Replace(input, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);

            string pattern = "   ";
            string replace = "";

            string result = Regex.Replace(resultString, pattern, replace);
            String singleLine = result.Replace("\n", " ");
            return singleLine;
        }
        
        public ActionResult PrintIndexV(long notificationId, string esdalRefno, string classification, string userType, string DRN, int ISNENVal = 0, long NENInboxId = 0)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

            if (Session["UserInfo"] == null)
            {
                string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                return RedirectToAction("Login", "Account");
            }

            return new ActionAsPdf("AgreedRouteV", new
            {
                notificationId = notificationId,
                esdalRefno = esdalRefno,
                classification = classification,
                userType = userType,
                DRN = DRN,
                ISNENVal = ISNENVal,
                NENInboxId = NENInboxId,
                UserTypeId = SessionInfo.UserTypeId,
                UserId = SessionInfo.UserId,
                OrganisationId = SessionInfo.OrganisationId,
                OrganisationName = SessionInfo.OrganisationName
            });

        }*/
        #endregion

        #region PrintReportPDF
        //Not using, need to remove later
        public ActionResult PrintReportPDF(long notificationId, string esdalRefno, string userType, string DRN, int ISNENVal = 0, long NENInboxId = 0)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

            if (Session["UserInfo"] == null)
            {
                string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                return RedirectToAction("Login", "Account");
            }

            return new ActionAsPdf("AgreedRouteV1", new
            {
                notificationId = notificationId,
                esdalRefno = esdalRefno,
                userType = userType,
                DRN = DRN,
                ISNENVal = ISNENVal,
                NENInboxId = NENInboxId,
                UserTypeId = SessionInfo.UserTypeId,
                UserId = SessionInfo.UserId,
                OrganisationId = SessionInfo.OrganisationId,
                OrganisationName = SessionInfo.OrganisationName,
                UserName = SessionInfo.UserName
            });

        }
        #endregion

        #region PrintReport
        /// <summary>
        /// print report based on documenttype and documentid
        /// </summary>
        /// <param name="Documentid">Documentid</param>
        /// <param name="DocumentType">Documenttype</param>
        /// <returns>Return true or false</returns>
        [HttpGet]
        public ActionResult PrintReport(long notificationId, string esdalRefno, string userType, string DRN, int ISNENVal = 0, long NENInboxId = 0, string docType = "", string contentRefNum = "")
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                int contactId = movementsService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));

                //>>>>>>>>>>>>>>>>>>>>>>BLOB REMOVAL>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                if (docType.ToLower() == "ne notification api" || docType.ToLower() == "ne agreed notification")
                    docType = "ne notification";
                else if (docType.ToLower() == "ne renotification api")
                    docType = "ne renotification";
                var ItemTypeStatus = (int)EnumExtensionMethods.GetValueFromDescription<ItemTypeStatus>(docType.ToLower());
                var NotificationType = userType.ToLower() == "police" ? NotificationXSD.NotificationTypeType.police : NotificationXSD.NotificationTypeType.soa;
                var documentParams = new SOProposalDocumentParams()
                {
                    ContactId = contactId,
                    NotificationId = (int)notificationId,
                    EsdalReferenceNo = esdalRefno,
                    SessionInfo = SessionInfo,
                    OrganisationId = Convert.ToInt32(SessionInfo.OrganisationId),
                    ItemTypeStatus = ItemTypeStatus,
                    NotificationType = NotificationType,
                    IsNen=ISNENVal
                };
                var model = movementsService.GetXmlDataForPrint(documentParams);
                if (model == null)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.WARNING, string.Format("Movement/PrintReport, data is null {0}", notificationId));
                    return null;
                }
                GenerateXML gxml = new GenerateXML();
                string xmlInformation = model.ReturnXML;
                //>>>>>>>>>>>>>>>>>>>>>>>>>> END BLOB REMOVAL>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                //string xmlInformation = movementsService.PrintReport(notificationId);

                //===============================================================================================
                xmlInformation = RemoveDispansationNode_of_Outboundxml(xmlInformation, DRN);
                //-----------------------------------------------------------------------------------------------

                xmlInformation = xmlInformation.Replace(">?<", ">\u2002<");
                xmlInformation = xmlInformation.Replace(">?##**##", ">##**##");

                xmlInformation = xmlInformation.Replace("<OnBehalfOf xmlns=\"http://www.esdal.com/schemas/core/movement\"> </OnBehalfOf>", "");
                xmlInformation = xmlInformation.Replace("<OnBehalfOf> </OnBehalfOf>", "");

                byte[] notificationDocument = null;
                string xsltPath = string.Empty;

                int organisationId = Convert.ToInt32(SessionInfo.OrganisationId);
                string HAReferenceNumber = string.Empty;

                if (Session["HAReferenceNo"] != null)
                {
                    HAReferenceNumber = Convert.ToString(Session["HAReferenceNo"]);
                }

                if (xmlInformation != string.Empty)
                {
                    //TODO for NEN project 
                    if (ISNENVal > 0)
                    {
                        xmlInformation = appendNENRouteDetails(xmlInformation, notificationId, NENInboxId, organisationId, ISNENVal, contentRefNum);
                    }
                    if (userType == "Police")
                    {
                        if (ISNENVal > 0)
                        {
                            xsltPath = AppDomain.CurrentDomain.BaseDirectory + "XSLT\\NEN_Movement_Police.xslt";
                        }
                        else
                        {
                            xsltPath = AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Notification_Movement_Police.xslt";
                        }
                        //TODO Chirag
                        xmlInformation = appendRouteDetails(xmlInformation, notificationId, ISNENVal, NENInboxId, organisationId); //added input parameter organisation Id changed for NEN R2 inbox table schema change.
                        //
                        notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", HAReferenceNumber, 692001, "PDF", null, userType);
                    }
                    else
                    {


                        //TODO Chirag
                        xmlInformation = appendRouteDetails(xmlInformation, notificationId, ISNENVal, NENInboxId, organisationId); //added input parameter organisation Id changed for NEN R2 inbox table schema change.

                        //organisationId added - HE-4795
                        xmlInformation = GetLoggedInUserAffectedStructureDetails(xmlInformation, notificationId, NENInboxId, ISNENVal, organisationId);

                        if (ISNENVal > 0)
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\NEN_Movement_SOA.xsl";
                        }
                        else
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Notification_Movement_SOA.xsl";
                        }

                        notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, SessionInfo.OrganisationName, HAReferenceNumber);
                    }
                }

                if (notificationDocument != null)
                {
                    System.IO.MemoryStream pdfStream = new System.IO.MemoryStream(notificationDocument);

                    return new FileStreamResult(pdfStream, "application/pdf");

                    //-----INTELLIZENZ PART FOR NEN NOTIFICATIONS AUDIT LOG SAVING-----------
                    #region-------INTELLIZENZ PART FOR NEN NOTIFICATIONS AUDIT LOG SAVING----------------
                    if (SessionInfo.UserName != null)
                    {
                        #region AUDIT LOGS FOR SAVING Print Document ESDAL/NEN NOTIFICATIONS BY SOA OR POLICE OR THROUGH HELPDESK
                        AuditLogIdentifiers auditLogType = new AuditLogIdentifiers();
                        string ErrMsg = string.Empty;
                        int NEN_ID = 0;
                        auditLogType.NENID = NEN_ID > 0 ? NEN_ID : 0;
                        if (SessionInfo.UserSchema == UserSchema.Portal)
                        {
                            auditLogType.AuditActionType = AuditActionType.soauser_download_nen_pdf;
                            auditLogType.HelpDeskUserID = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                            auditLogType.HelpDeskUsername = SessionInfo.HelpdeskUserName;
                            auditLogType.ESDALNotificationNo = esdalRefno;
                            auditLogType.NotificationID = notificationId;
                            auditLogType.DateTime = DateTime.Now.ToString();
                            string auditLogDescp = AuditLog.GetNENNotifAuditLog(SessionInfo, auditLogType, out ErrMsg);
                            int user_ID = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                            movementsService.SaveNotificationAuditLog(auditLogType, auditLogDescp, user_ID, SessionInfo.organisationId);
                        }

                        #endregion
                    }
                    #endregion---------------end--------------------------------
                    //------END HERE---------------------------------------------------------

                }
                else
                {
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/PrintReport, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion

        #region Code commented earlier added in a region by Mahzeer on 11/12/20223
        /// <summary>
        /// print report based on documenttype and documentid
        /// </summary>
        /// <param name="Documentid">Documentid</param>
        /// <param name="DocumentType">Documenttype</param>
        /// <returns>Return true or false</returns>
        //[HttpGet]
        //public ActionResult PrintReport1(long notificationId, string esdalRefno, string userType, string DRN, int ISNENVal = 0, long NENInboxId = 0)
        //{
        //    try
        //    {
        //        if (Session["UserInfo"] == null)
        //        {
        //            string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
        //            string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

        //            return RedirectToAction("Login", "Account");
        //        }

        //        UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

        //        int contactId = movementsService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));
        //        string xmlInformation = movementsService.PrintReport(notificationId);


        //        //===============================================================================================
        //        xmlInformation = RemoveDispansationNode_of_Outboundxml(xmlInformation, DRN);
        //        //-----------------------------------------------------------------------------------------------

        //        xmlInformation = xmlInformation.Replace(">?<", ">\u2002<");
        //        xmlInformation = xmlInformation.Replace(">?##**##", ">##**##");

        //        xmlInformation = xmlInformation.Replace("<OnBehalfOf xmlns=\"http://www.esdal.com/schemas/core/movement\"> </OnBehalfOf>", "");
        //        xmlInformation = xmlInformation.Replace("<OnBehalfOf> </OnBehalfOf>", "");

        //        byte[] notificationDocument = null;
        //        string xsltPath = string.Empty;

        //        int organisationId = Convert.ToInt32(SessionInfo.OrganisationId);
        //        string HAReferenceNumber = string.Empty;

        //        if (Session["HAReferenceNo"] != null)
        //        {
        //            HAReferenceNumber = Convert.ToString(Session["HAReferenceNo"]);
        //        }

        //        if (xmlInformation != string.Empty)
        //        {
        //            //TODO for NEN project 
        //            if (ISNENVal > 0)
        //            {
        //                xmlInformation = appendNENRouteDetails(xmlInformation, notificationId, NENInboxId, organisationId);
        //            }
        //            if (userType == "Police")
        //            {
        //                if (ISNENVal > 0)
        //                {
        //                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\NEN_Movement_Police.xslt";
        //                }
        //                else
        //                {
        //                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Notification_Movement_Police.xslt";
        //                }
        //                //TODO Chirag
        //                xmlInformation = appendRouteDetails(xmlInformation, notificationId, ISNENVal, NENInboxId, organisationId); //added input parameter organisation Id changed for NEN R2 inbox table schema change.
        //                //
        //                notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", HAReferenceNumber, 692001, "PDF", null, userType);
        //            }
        //            else
        //            {


        //                //TODO Chirag
        //                xmlInformation = appendRouteDetails(xmlInformation, notificationId, ISNENVal, NENInboxId, organisationId); //added input parameter organisation Id changed for NEN R2 inbox table schema change.

        //                xmlInformation = GetLoggedInUserAffectedStructureDetails(xmlInformation, notificationId, NENInboxId, ISNENVal);

        //                if (ISNENVal > 0)
        //                {
        //                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\NEN_Movement_SOA.xsl";
        //                }
        //                else
        //                {
        //                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Notification_Movement_SOA.xsl";
        //                }

        //                notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, SessionInfo.OrganisationName, HAReferenceNumber);
        //            }
        //        }

        //        if (notificationDocument != null)
        //        {
        //            string notificationDocument1 = documentService.GeneratePDF1(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, SessionInfo.OrganisationName, HAReferenceNumber);

        //            string baseUrl = string.Empty;

        //            //Convert the HTML string to PDF using Blink
        //            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);

        //            BlinkConverterSettings settings = new BlinkConverterSettings();
        //            settings.BlinkPath = Server.MapPath("~\\BlinkBinaries");

        //            //Assign Blink settings to HTML converter
        //            htmlConverter.ConverterSettings = settings;

        //            //Convert HTML string to PDF
        //            PdfDocument document = htmlConverter.Convert(notificationDocument1, baseUrl);

        //            MemoryStream stream = new MemoryStream();

        //            //Save and close the PDF document 
        //            document.Save(stream);
        //            document.Close(true);

        //            return File(stream.ToArray(), "application/pdf", "Blink.pdf");

        //            //System.IO.MemoryStream pdfStream = new System.IO.MemoryStream(notificationDocument);

        //            //return new FileStreamResult(pdfStream, "application/pdf");

        //            //-----INTELLIZENZ PART FOR NEN NOTIFICATIONS AUDIT LOG SAVING-----------
        //            #region-------INTELLIZENZ PART FOR NEN NOTIFICATIONS AUDIT LOG SAVING----------------
        //            if (SessionInfo.UserName != null)
        //            {
        //                #region AUDIT LOGS FOR SAVING Print Document ESDAL/NEN NOTIFICATIONS BY SOA OR POLICE OR THROUGH HELPDESK
        //                AuditLogIdentifiers auditLogType = new AuditLogIdentifiers();
        //                string ErrMsg = string.Empty;
        //                int NEN_ID = 0;
        //                auditLogType.NENID = NEN_ID > 0 ? NEN_ID : 0;
        //                if (SessionInfo.UserSchema == UserSchema.Portal)
        //                {
        //                    auditLogType.AuditActionType = AuditActionType.soauser_download_nen_pdf;
        //                    auditLogType.HelpDeskUserID = Convert.ToInt32(SessionInfo.HelpdeskUserId);
        //                    auditLogType.HelpDeskUsername = SessionInfo.HelpdeskUserName;
        //                    auditLogType.ESDALNotificationNo = esdalRefno;
        //                    auditLogType.NotificationID = notificationId;
        //                    auditLogType.DateTime = DateTime.Now.ToString();
        //                    string auditLogDescp = AuditLog.GetNENNotifAuditLog(SessionInfo, auditLogType, out ErrMsg);
        //                    int user_ID = Convert.ToInt32(SessionInfo.HelpdeskUserId);
        //                    movementsService.SaveNotificationAuditLog(auditLogType, auditLogDescp, user_ID, SessionInfo.organisationId);
        //                }

        //                #endregion
        //            }
        //            #endregion---------------end--------------------------------
        //            //------END HERE---------------------------------------------------------

        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/PrintReport, Exception: {0}", ex));
        //        throw ex;
        //    }
        //}

        //public ActionResult ConvertToPDF()
        //{
        //    string html = System.IO.File.ReadAllText(Server.MapPath("~\\test.html"));
        //    string baseUrl = string.Empty;

        //    //Convert the HTML string to PDF using Blink
        //    HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);

        //    BlinkConverterSettings settings = new BlinkConverterSettings();
        //    settings.BlinkPath = Server.MapPath("~\\BlinkBinaries");

        //    //Assign Blink settings to HTML converter
        //    htmlConverter.ConverterSettings = settings;

        //    //Convert HTML string to PDF
        //    PdfDocument document = htmlConverter.Convert(html, baseUrl);

        //    MemoryStream stream = new MemoryStream();

        //    //Save and close the PDF document 
        //    document.Save(stream);
        //    document.Close(true);

        //    return File(stream.ToArray(), "application/pdf", "Blink.pdf");
        //}

        //public void DownloadPDF()
        //{
        //    string HTMLContent = "Hello <b>World</b>";
        //    Response.Clear();
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-disposition", "attachment;filename=" + "PDFfile.pdf");
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.BinaryWrite(GetPDF(HTMLContent));
        //    Response.End();
        //}

        //public byte[] GetPDF(string pHTML)
        //{
        //    byte[] bPDF = null;

        //    MemoryStream ms = new MemoryStream();
        //    TextReader txtReader = new StringReader(pHTML);

        //    // 1: create object of a itextsharp document class  
        //    Document doc = new Document(PageSize.A4, 25, 25, 25, 25);

        //    // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file  
        //    PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);

        //    // 3: we create a worker parse the document  
        //    HTMLWorker htmlWorker = new HTMLWorker(doc);

        //    // 4: we open document and start the worker on the document  
        //    doc.Open();
        //    htmlWorker.StartDocument();


        //    // 5: parse the html into the document  
        //    htmlWorker.Parse(txtReader);

        //    // 6: close the document and the worker  
        //    htmlWorker.EndDocument();
        //    htmlWorker.Close();
        //    doc.Close();

        //    bPDF = ms.ToArray();

        //    return bPDF;
        //}

        #endregion

        #region AgreedRouteV1
        /// <summary>
        /// print report based on documenttype and documentid
        /// </summary>
        /// <param name="Documentid">Documentid</param>
        /// <param name="DocumentType">Documenttype</param>
        /// <returns>Return true or false</returns>
        [HttpGet]
        public ActionResult AgreedRouteV1(long notificationId, string esdalRefno, string userType, string DRN, int ISNENVal = 0, long NENInboxId = 0,
            int UserTypeId = 0, string UserId = "", long OrganisationId = 0, string OrganisationName = "", string UserName = "")
        {
            try
            {
                //if (Session["UserInfo"] == null)
                //{
                //    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                //    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                //    return RedirectToAction("Login", "Account");
                //}

                //UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                int contactId = movementsService.GetContactDetails(Convert.ToInt32(UserId));
                string xmlInformation = movementsService.PrintReport(notificationId);


                //===============================================================================================
                xmlInformation = RemoveDispansationNode_of_Outboundxml(xmlInformation, DRN);
                //-----------------------------------------------------------------------------------------------

                xmlInformation = xmlInformation.Replace(">?<", ">\u2002<");
                xmlInformation = xmlInformation.Replace(">?##**##", ">##**##");

                xmlInformation = xmlInformation.Replace("<OnBehalfOf xmlns=\"http://www.esdal.com/schemas/core/movement\"> </OnBehalfOf>", "");
                xmlInformation = xmlInformation.Replace("<OnBehalfOf> </OnBehalfOf>", "");

                byte[] notificationDocument = null;
                string xsltPath = string.Empty;

                int organisationId = Convert.ToInt32(OrganisationId);
                string HAReferenceNumber = string.Empty;

                if (Session["HAReferenceNo"] != null)
                {
                    HAReferenceNumber = Convert.ToString(Session["HAReferenceNo"]);
                }

                if (xmlInformation != string.Empty)
                {
                    //TODO for NEN project 
                    if (ISNENVal > 0)
                    {
                        xmlInformation = appendNENRouteDetails(xmlInformation, notificationId, NENInboxId, organisationId, ISNENVal, string.Empty);
                    }
                    if (userType == "Police")
                    {
                        if (ISNENVal > 0)
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\NEN_Movement_Police.xslt";
                        }
                        else
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Notification_Movement_Police.xslt";
                        }
                        //TODO Chirag
                        xmlInformation = appendRouteDetails(xmlInformation, notificationId, ISNENVal, NENInboxId, organisationId); //added input parameter organisation Id changed for NEN R2 inbox table schema change.
                        //
                        notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", HAReferenceNumber, 692001, "PDF", null, userType);
                    }
                    else
                    {


                        //TODO Chirag
                        xmlInformation = appendRouteDetails(xmlInformation, notificationId, ISNENVal, NENInboxId, organisationId); //added input parameter organisation Id changed for NEN R2 inbox table schema change.

                        xmlInformation = GetLoggedInUserAffectedStructureDetails(xmlInformation, notificationId, NENInboxId, ISNENVal, OrganisationId, UserTypeId);

                        if (ISNENVal > 0)
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\NEN_Movement_SOA.xsl";
                        }
                        else
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Notification_Movement_SOA.xsl";
                        }

                        notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, OrganisationName, HAReferenceNumber);
                    }
                }

                if (notificationDocument != null)
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(xmlInformation);
                    //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
                    MemoryStream stream = new MemoryStream(byteArray);

                    //read configuration xml
                    XmlSerializer deserializer = new XmlSerializer(typeof(STP.Domain.MovementsAndNotifications.Folder.OutboundNotification));
                    STP.Domain.MovementsAndNotifications.Folder.OutboundNotification agreedRoute = new STP.Domain.MovementsAndNotifications.Folder.OutboundNotification();

                    //string xsltPath1 = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\data3.xml";

                    using (XmlReader reader = XmlReader.Create(stream))
                    {
                        agreedRoute = (STP.Domain.MovementsAndNotifications.Folder.OutboundNotification)deserializer.Deserialize(reader);
                    }
                    //string singleLine = ReplaceWhitespace(agreedRoute.RouteParts.RoutePartListPosition.Route, "");


                    //agreedRoute.RouteParts.RoutePartListPosition.Route = singleLine;

                    //string RouteImperial = ReplaceWhitespace(agreedRoute.RouteParts.RoutePartListPosition.RouteImperial, "");
                    //agreedRoute.RouteParts.RoutePartListPosition.RouteImperial = RouteImperial;

                    return View(agreedRoute);


                    //System.IO.MemoryStream pdfStream = new System.IO.MemoryStream(notificationDocument);

                    //return new FileStreamResult(pdfStream, "application/pdf");

                    //-----INTELLIZENZ PART FOR NEN NOTIFICATIONS AUDIT LOG SAVING-----------

                    //------END HERE---------------------------------------------------------

                }
                else
                {
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/PrintReport, Exception: {0}", ex));
                throw ex;
            }
        }
        public ActionResult AgreedRouteV1()
        {
            //read configuration xml
            XmlSerializer deserializer = new XmlSerializer(typeof(STP.Domain.MovementsAndNotifications.Folder.OutboundNotification));
            STP.Domain.MovementsAndNotifications.Folder.OutboundNotification agreedRoute = new STP.Domain.MovementsAndNotifications.Folder.OutboundNotification();

            string xsltPath1 = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\data3.xml";

            using (XmlReader reader = XmlReader.Create(xsltPath1))
            {
                agreedRoute = (STP.Domain.MovementsAndNotifications.Folder.OutboundNotification)deserializer.Deserialize(reader);
            }
            //string singleLine = ReplaceWhitespace(agreedRoute.RouteParts.RoutePartListPosition.Route, "");


            //agreedRoute.RouteParts.RoutePartListPosition.Route = singleLine;

            //string RouteImperial = ReplaceWhitespace(agreedRoute.RouteParts.RoutePartListPosition.RouteImperial, "");
            //agreedRoute.RouteParts.RoutePartListPosition.RouteImperial = RouteImperial;

            return View(agreedRoute);
        }

        public ActionResult PrintIndexV1()
        {
            AgreedRoute agreedRoute = new AgreedRoute();

            return new ActionAsPdf("AgreedRouteV1")
            {
                CustomSwitches = "--disable-smart-shrinking --no-outline",
            };
        }
        #endregion

        #region PrintReducedReport
        /// <summary>
        /// print report based on documenttype and documentid
        /// </summary>
        /// <param name="Documentid">Documentid</param>
        /// <param name="DocumentType">Documenttype</param>
        /// <returns>Return true or false</returns>
        [HttpGet]
        public ActionResult PrintReducedReport(long notificationId, string esdalRefno, string classification, string userType, string DRN, int ISNENVal = 0, long NENInboxId = 0, string docType = "", string contentRefNum = "")
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                int contactId = movementsService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));
                //>>>>>>>>>>>>>>>>>>>>>>BLOB REMOVAL>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                if (docType.ToLower() == "ne notification api" || docType.ToLower() == "ne agreed notification")
                    docType = "ne notification";
                else if (docType.ToLower() == "ne renotification api")
                    docType = "ne renotification";

                var ItemTypeStatus = (int)EnumExtensionMethods.GetValueFromDescription<ItemTypeStatus>(docType.ToLower());
                var NotificationType = userType.ToLower() == "police" ? NotificationXSD.NotificationTypeType.police : NotificationXSD.NotificationTypeType.soa;
                var documentParams = new SOProposalDocumentParams()
                {
                    ContactId = contactId,
                    NotificationId = (int)notificationId,
                    EsdalReferenceNo = esdalRefno,
                    SessionInfo = SessionInfo,
                    OrganisationId = Convert.ToInt32(SessionInfo.OrganisationId),
                    ItemTypeStatus = ItemTypeStatus,
                    NotificationType = NotificationType,
                    IsNen = ISNENVal
                };
                var model = movementsService.GetXmlDataForPrint(documentParams);
                if (model == null)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.WARNING, string.Format("Movement/PrintReport, data is null {0}", notificationId));
                    return null;
                }
                GenerateXML gxml = new GenerateXML();
                string xmlInformation = model.ReturnXML;
                //>>>>>>>>>>>>>>>>>>>>>>>>>> END BLOB REMOVAL>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                //string xmlInformation = string.Empty;

                //if (SessionInfo != null)
                //{
                //    xmlInformation = movementsService.PrintReport(notificationId);
                //}


                //===============================================================================================
                xmlInformation = RemoveDispansationNode_of_Outboundxml(xmlInformation, DRN);
                //-----------------------------------------------------------------------------------------------

                if (xmlInformation != string.Empty)
                {
                    xmlInformation = xmlInformation.Replace(">?<", ">\u2002<");
                    xmlInformation = xmlInformation.Replace(">?##**##", ">##**##");

                    xmlInformation = xmlInformation.Replace("<OnBehalfOf xmlns=\"http://www.esdal.com/schemas/core/movement\"> </OnBehalfOf>", "");
                    xmlInformation = xmlInformation.Replace("<OnBehalfOf> </OnBehalfOf>", "");
                }

                byte[] notificationDocument = null;
                string xsltPath = string.Empty;

                if (xmlInformation != string.Empty)
                {
                    //TODO for NEN project 
                    if (ISNENVal > 0)
                    {
                        xmlInformation = appendNENRouteDetails(xmlInformation, notificationId, NENInboxId, (int)SessionInfo.OrganisationId, ISNENVal, contentRefNum);
                    }
                    if (userType == "Police")
                    {
                        if (classification == "STGO")
                        {
                            if (ISNENVal > 0)
                            {
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\NEN_STGOReducedNotificationPolice.xslt";
                            }
                            else
                            {
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\STGOReducedNotificationPolice.xslt";
                            }

                            xmlInformation = appendRouteDetails(xmlInformation, notificationId, ISNENVal, NENInboxId, (int)SessionInfo.OrganisationId);//TODO chirag
                            notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", 692001, "PDF", null, userType);
                        }
                        else if (classification == "CandU")
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\PoliceNotificationReducedCandU.xslt";
                            xmlInformation = appendRouteDetails(xmlInformation, notificationId, ISNENVal, NENInboxId, (int)SessionInfo.OrganisationId);//TODO chirag
                            notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", 692001, "PDF", null, userType);
                        }
                        else
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\NotificationForSpecialOrderPolice.xslt";
                            xmlInformation = appendRouteDetails(xmlInformation, notificationId, ISNENVal, NENInboxId, (int)SessionInfo.OrganisationId);//TODO chirag
                            notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", 692001, "PDF", null, userType);
                        }
                    }
                    else
                    {

                        if (classification == "STGO")
                        {
                            xmlInformation = GetLoggedInUserAffectedStructureDetails(xmlInformation, notificationId, NENInboxId, 0, SessionInfo.OrganisationId, SessionInfo.UserTypeId);
                            if (ISNENVal > 0)
                            {
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\NEN_STGOReducedNotificationSOA.xslt";
                            }
                            else
                            {
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\STGOReducedNotificationSOA.xslt";
                            }
                            xmlInformation = appendRouteDetails(xmlInformation, notificationId, ISNENVal, NENInboxId, (int)SessionInfo.OrganisationId);//TODO chirag
                            notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, SessionInfo.OrganisationName);
                            string someString = Encoding.ASCII.GetString(notificationDocument);

                        }
                        else if (classification == "CandU")
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\PoliceNotificationReducedCandU.xslt";
                            xmlInformation = appendRouteDetails(xmlInformation, notificationId, ISNENVal, NENInboxId, (int)SessionInfo.OrganisationId);//TODO chirag
                            notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "");
                        }
                        else
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\NotificationForSpecialOrder.xslt";
                            xmlInformation = appendRouteDetails(xmlInformation, notificationId, ISNENVal, NENInboxId, (int)SessionInfo.OrganisationId);//TODO chirag
                            notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "");
                        }
                    }
                }

                if (notificationDocument != null)
                {
                    System.IO.MemoryStream pdfStream = new System.IO.MemoryStream(notificationDocument);

                    return new FileStreamResult(pdfStream, "application/pdf");

                    //-----INTELLIZENZ PART FOR NEN NOTIFICATIONS AUDIT LOG SAVING-----------
                    #region-------INTELLIZENZ PART FOR NEN NOTIFICATIONS AUDIT LOG SAVING----------------
                    if (SessionInfo.UserName != null)
                    {
                        #region AUDIT LOGS FOR SAVING Print reduced Document ESDAL/NEN NOTIFICATIONS BY SOA OR POLICE OR THROUGH HELPDESK
                        AuditLogIdentifiers auditLogType = new AuditLogIdentifiers();
                        string ErrMsg = string.Empty;
                        int NEN_ID = 0;
                        auditLogType.NENID = NEN_ID > 0 ? NEN_ID : 0;
                        if (SessionInfo.UserSchema == UserSchema.Portal)
                        {
                            auditLogType.AuditActionType = AuditActionType.print_reduced_report_doc;
                            auditLogType.HelpDeskUserID = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                            auditLogType.HelpDeskUsername = SessionInfo.HelpdeskUserName;
                            auditLogType.ESDALNotificationNo = esdalRefno;
                            auditLogType.NotificationID = notificationId;
                            auditLogType.DateTime = DateTime.Now.ToString();
                            string auditLogDescp = AuditLog.GetNENNotifAuditLog(SessionInfo, auditLogType, out ErrMsg);
                            int user_ID = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                            movementsService.SaveNotificationAuditLog(auditLogType, auditLogDescp, user_ID, SessionInfo.organisationId);
                        }

                        #endregion
                    }
                    #endregion---------------end--------------------------------
                    //------END HERE---------------------------------------------------------
                }
                else
                {
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/PrintReducedReport, Exception: {0}", ex));
                throw ex;
            }
        }

        #endregion

        #region PrintNoLongerAffectedReport
        [HttpGet]
        public ActionResult PrintNoLongerAffectedReport(string esdalRefno, string userType, string DRN, string docType = "")
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                int contactId = movementsService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));

                //>>>>>>>>>>>>>>>>>>>>>>BLOB REMOVAL>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                int versionNo = 0;
                string[] esdalRefPro = esdalRefno.Split('/');
                string haulierMnemonic = string.Empty;
                string esdalrefnum = string.Empty;
                if (esdalRefPro.Length > 0)
                {
                    haulierMnemonic = Convert.ToString(esdalRefPro[0]);
                    esdalrefnum = Convert.ToString(esdalRefPro[1].ToUpper().Replace("S", ""));
                    versionNo = Convert.ToInt32(esdalRefPro[2].ToUpper().Replace("S", ""));
                }
                MovementPrint moveprint = sortApplicationService.GetProjectIdByEsdalReferenceNo(esdalRefno);
                var ItemTypeStatus = (int)EnumExtensionMethods.GetValueFromDescription<ItemTypeStatus>(docType.ToLower());
                var NotificationType = userType.ToLower() == "police" ? NotificationXSD.NotificationTypeType.police : NotificationXSD.NotificationTypeType.soa;
                var UserType = userType.ToLower() == "police" ? Enums.PortalType.POLICE : Enums.PortalType.SOA;
                var documentParams = new SOProposalDocumentParams()
                {
                    ContactId = contactId,
                    EsdalReferenceNo = esdalRefno,
                    SessionInfo = SessionInfo,
                    OrganisationId = Convert.ToInt32(SessionInfo.OrganisationId),
                    ItemTypeStatus = ItemTypeStatus,
                    NotificationType = NotificationType,
                    UserSchema = SessionInfo.UserSchema,
                    UserType = UserType,
                    Moveprint = moveprint,
                    VersionNo = versionNo
                };
                var model = movementsService.GetXmlDataForPrint(documentParams);
                if (model == null)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.WARNING, string.Format("Movement/PrintNoLongerAffectedReport, data is null {0}", esdalRefno));
                    return null;
                }
                GenerateXML gxml = new GenerateXML();
                string xmlInformation = model.ReturnXML;
                //>>>>>>>>>>>>>>>>>>>>>>>>>> END BLOB REMOVAL>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                //string xmlInformation = movementsService.PrintAgreedReport(esdalRefno, SessionInfo.OrganisationId);

                if (xmlInformation != string.Empty)
                {
                    xmlInformation = xmlInformation.Replace(">?<", ">\u2002<");
                    xmlInformation = xmlInformation.Replace(">?##**##", ">##**##");
                }


                byte[] notificationDocument = null;
                string xsltPath = string.Empty;

                if (xmlInformation != string.Empty)
                {
                    if (userType == "Police")
                    {
                        xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ReProposalNoLongerFAXPolice.xslt";
                        notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits);
                    }
                    else
                    {
                        xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Re_Proposal_No_Longer_FAX_SOA.xsl";
                        notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "MovementPDF");
                    }
                }

                if (notificationDocument != null)
                {
                    System.IO.MemoryStream pdfStream = new System.IO.MemoryStream(notificationDocument);

                    return new FileStreamResult(pdfStream, "application/pdf");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data is not valid for current movement therefore PDF document will not be generated.');</script>");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/PrintReducedReport, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion

        #region PrintReProposedReport
        public ActionResult PrintReProposedReport(string esdalRefno, string userType, string DRN, string docType = "", bool SORTflag = false)
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                int contactId = movementsService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));
                //>>>>>>>>>>>>>>>>>>>>>>BLOB REMOVAL>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                int versionNo = 0;
                string[] esdalRefPro = esdalRefno.Split('/');
                string haulierMnemonic = string.Empty;
                string esdalrefnum = string.Empty;
                if (esdalRefPro.Length > 0)
                {
                    haulierMnemonic = Convert.ToString(esdalRefPro[0]);
                    esdalrefnum = Convert.ToString(esdalRefPro[1].ToUpper().Replace("S", ""));
                    versionNo = Convert.ToInt32(esdalRefPro[2].ToUpper().Replace("S", ""));
                }
                MovementPrint moveprint = sortApplicationService.GetProjectIdByEsdalReferenceNo(esdalRefno);
                var ItemTypeStatus = (int)EnumExtensionMethods.GetValueFromDescription<ItemTypeStatus>(docType.ToLower());
                var NotificationType = userType.ToLower() == "police" ? NotificationXSD.NotificationTypeType.police : NotificationXSD.NotificationTypeType.soa;
                var UserType = userType.ToLower() == "police" ? Enums.PortalType.POLICE : Enums.PortalType.SOA;
                string schema = SORTflag ? UserSchema.Sort : SessionInfo.UserSchema;
                var documentParams = new SOProposalDocumentParams()
                {
                    ContactId = contactId,
                    EsdalReferenceNo = esdalRefno,
                    SessionInfo = SessionInfo,
                    OrganisationId = Convert.ToInt32(SessionInfo.OrganisationId),
                    ItemTypeStatus = ItemTypeStatus,
                    NotificationType = NotificationType,
                    UserSchema = schema,
                    UserType = UserType,
                    Moveprint = moveprint,
                    VersionNo = versionNo
                };
                var model = movementsService.GetXmlDataForPrint(documentParams);
                if (model == null)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.WARNING, string.Format("Movement/PrintProposedReport, data is null {0}", esdalRefno));
                    return null;
                }
                GenerateXML gxml = new GenerateXML();
                string xmlInformation = model.ReturnXML;
                //>>>>>>>>>>>>>>>>>>>>>>>>>> END BLOB REMOVAL>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                //string xmlInformation = movementsService.PrintAgreedReport(esdalRefno, SessionInfo.OrganisationId);

                if (xmlInformation != string.Empty)
                {
                    xmlInformation = xmlInformation.Replace(">?<", ">\u2002<");
                    xmlInformation = xmlInformation.Replace(">?##**##", ">##**##");
                }

                byte[] notificationDocument = null;
                string xsltPath = string.Empty;

                if (xmlInformation != string.Empty)
                {
                    if (userType == "Police")
                    {
                        xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ReProposalstillaffected_Fax_Police.xslt";
                        notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits);
                    }
                    else
                    {
                        xmlInformation = documentService.GetLoggedInUserAffectedStructureDetailsByESDALReference(xmlInformation, esdalRefno, SessionInfo, UserSchema.Sort, "Proposal", Convert.ToInt32(SessionInfo.OrganisationId));

                        xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ReProposalStillAffectedFax.xslt";
                        notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "MovementPDF");
                    }
                }

                if (notificationDocument != null)
                {
                    System.IO.MemoryStream pdfStream = new System.IO.MemoryStream(notificationDocument);

                    return new FileStreamResult(pdfStream, "application/pdf");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data is not valid for current movement therefore PDF document will not be generated.');</script>");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/PrintProposedReport, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion

        #region PrintProposedReport
        public ActionResult PrintProposedReport(string esdalRefno, string userType, string DRN, string docType = "")
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                int contactId = movementsService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));

                //>>>>>>>>>>>>>>>>>>>>>>BLOB REMOVAL>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                int versionNo = 0;
                string[] esdalRefPro = esdalRefno.Split('/');
                string haulierMnemonic = string.Empty;
                string esdalrefnum = string.Empty;
                if (esdalRefPro.Length > 0)
                {
                    haulierMnemonic = Convert.ToString(esdalRefPro[0]);
                    esdalrefnum = Convert.ToString(esdalRefPro[1].ToUpper().Replace("S", ""));
                    versionNo = Convert.ToInt32(esdalRefPro[2].ToUpper().Replace("S", ""));
                }
                var ItemTypeStatus = (int)EnumExtensionMethods.GetValueFromDescription<ItemTypeStatus>(docType.ToLower());
                var NotificationType = userType.ToLower() == "police" ? NotificationXSD.NotificationTypeType.police : NotificationXSD.NotificationTypeType.soa;
                var documentParams = new SOProposalDocumentParams()
                {
                    ContactId = contactId,
                    EsdalReferenceNo = esdalRefno,
                    SessionInfo = SessionInfo,
                    OrganisationId = Convert.ToInt32(SessionInfo.OrganisationId),
                    ItemTypeStatus = ItemTypeStatus,
                    NotificationType = NotificationType,
                    UserSchema = SessionInfo.UserSchema,
                    VersionNo = versionNo
                };
                var model = movementsService.GetXmlDataForPrint(documentParams);
                if (model == null)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.WARNING, string.Format("Movement/PrintProposedReport, data is null {0}", esdalRefno));
                    return null;
                }
                GenerateXML gxml = new GenerateXML();
                string xmlInformation = model.ReturnXML;
                //>>>>>>>>>>>>>>>>>>>>>>>>>> END BLOB REMOVAL>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                //string xmlInformation = movementsService.PrintAgreedReport(esdalRefno, SessionInfo.OrganisationId);

                if (xmlInformation != string.Empty)
                {
                    xmlInformation = xmlInformation.Replace(">?<", ">\u2002<");
                    xmlInformation = xmlInformation.Replace(">?##**##", ">##**##");
                }

                byte[] notificationDocument = null;
                string xsltPath = string.Empty;

                if (xmlInformation != string.Empty)
                {
                    if (userType == "Police")
                    {
                        xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ProposalForPolice.xslt";
                        notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits);
                    }
                    else
                    {
                        //STP.Document.GenerateDocument genDocument = new Document.GenerateDocument();
                        xmlInformation = documentService.GetLoggedInUserAffectedStructureDetailsByESDALReference(xmlInformation, esdalRefno, SessionInfo, UserSchema.Sort, "Proposal", Convert.ToInt32(SessionInfo.OrganisationId));

                        xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ProposalFaxSOA.xslt";
                        notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits);
                    }
                }

                if (notificationDocument != null)
                {
                    System.IO.MemoryStream pdfStream = new System.IO.MemoryStream(notificationDocument);

                    return new FileStreamResult(pdfStream, "application/pdf");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data is not valid for current movement therefore PDF document will not be generated.');</script>");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/PrintProposedReport, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion

        #region PrintAmendmentToAgreementReport
        public ActionResult PrintAmendmentToAgreementReport(string esdalRefno, string userType, string DRN, string docType = "", int VersionId = 0)
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                int contactId = movementsService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));
                //>>>>>>>>>>>>>>>>>>>>>>BLOB REMOVAL>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                int versionNo = 0;
                string[] esdalRefPro = esdalRefno.Split('/');
                string haulierMnemonic = string.Empty;
                string esdalrefnum = string.Empty;
                if (esdalRefPro.Length > 0)
                {
                    haulierMnemonic = Convert.ToString(esdalRefPro[0]);
                    esdalrefnum = Convert.ToString(esdalRefPro[1].ToUpper().Replace("S", ""));
                    versionNo = Convert.ToInt32(esdalRefPro[2].ToUpper().Replace("S", ""));
                }
                MovementPrint moveprint = sortApplicationService.GetOrderNoProjectId(VersionId);
                var ItemTypeStatus = (int)EnumExtensionMethods.GetValueFromDescription<ItemTypeStatus>(docType.ToLower());
                var NotificationType = userType.ToLower() == "police" ? NotificationXSD.NotificationTypeType.police : NotificationXSD.NotificationTypeType.soa;
                var UserType = userType.ToLower() == "police" ? Enums.PortalType.POLICE : Enums.PortalType.SOA;
                var documentParams = new SOProposalDocumentParams()
                {
                    ContactId = contactId,
                    EsdalReferenceNo = esdalRefno,
                    SessionInfo = SessionInfo,
                    OrganisationId = Convert.ToInt32(SessionInfo.OrganisationId),
                    ItemTypeStatus = ItemTypeStatus,
                    NotificationType = NotificationType,
                    UserSchema = SessionInfo.UserSchema,
                    UserType = UserType,
                    Moveprint = moveprint,
                    VersionNo = versionNo
                };
                var model = movementsService.GetXmlDataForPrint(documentParams);
                if (model == null)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.WARNING, string.Format("Movement/PrintAmendmentToAgreementReport, data is null {0}", esdalRefno));
                    return null;
                }
                GenerateXML gxml = new GenerateXML();
                string xmlInformation = model.ReturnXML;
                //>>>>>>>>>>>>>>>>>>>>>>>>>> END BLOB REMOVAL>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                //string xmlInformation = movementsService.PrintAgreedReport(esdalRefno, SessionInfo.OrganisationId);

                if (xmlInformation != string.Empty)
                {
                    xmlInformation = xmlInformation.Replace(">?<", ">\u2002<");
                    xmlInformation = xmlInformation.Replace(">?##**##", ">##**##");
                }

                byte[] notificationDocument = null;
                string xsltPath = string.Empty;

                if (xmlInformation != string.Empty)
                {
                    if (userType == "Police")
                    {
                        xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\RevisedAgreementFaxPolice.xslt";
                        notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits);
                    }
                    else
                    {
                        //STP.Document.GenerateDocument genDocument = new Document.GenerateDocument();
                        xmlInformation = documentService.GetLoggedInUserAffectedStructureDetailsByESDALReference(xmlInformation, esdalRefno, SessionInfo, UserSchema.Sort, "agreed", Convert.ToInt32(SessionInfo.OrganisationId));

                        xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\RevisedAgreement.xslt";
                        notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "MovementPDF");
                    }
                }

                if (notificationDocument != null)
                {
                    System.IO.MemoryStream pdfStream = new System.IO.MemoryStream(notificationDocument);

                    return new FileStreamResult(pdfStream, "application/pdf");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data is not valid for current movement therefore PDF document will not be generated.');</script>");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/PrintAgreedReport, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion

        #region PrintAgreedReport
        public ActionResult PrintAgreedReport(string esdalRefno, string userType, string DRN, string docType = "", int VersionId = 0)
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                int contactId = movementsService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));

                //>>>>>>>>>>>>>>>>>>>>>>BLOB REMOVAL>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                MovementPrint moveprint = sortApplicationService.GetOrderNoProjectId(VersionId);
                var ItemTypeStatus = (int)EnumExtensionMethods.GetValueFromDescription<ItemTypeStatus>(docType.ToLower());
                var NotificationType = userType.ToLower() == "police" ? NotificationXSD.NotificationTypeType.police : NotificationXSD.NotificationTypeType.soa;
                var UserType = userType.ToLower() == "police" ? Enums.PortalType.POLICE : Enums.PortalType.SOA;
                var documentParams = new SOProposalDocumentParams()
                {
                    ContactId = contactId,
                    EsdalReferenceNo = esdalRefno,
                    SessionInfo = SessionInfo,
                    OrganisationId = Convert.ToInt32(SessionInfo.OrganisationId),
                    ItemTypeStatus = ItemTypeStatus,
                    NotificationType = NotificationType,
                    UserSchema = SessionInfo.UserSchema,
                    UserType = UserType,
                    Moveprint = moveprint
                };
                var model = movementsService.GetXmlDataForPrint(documentParams);
                if (model == null)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.WARNING, string.Format("Movement/PrintProposedReport, data is null {0}", esdalRefno));
                    return null;
                }
                GenerateXML gxml = new GenerateXML();
                string xmlInformation = model.ReturnXML;
                //>>>>>>>>>>>>>>>>>>>>>>>>>> END BLOB REMOVAL>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                //string xmlInformation = movementsService.PrintAgreedReport(esdalRefno, SessionInfo.OrganisationId);

                if (xmlInformation != string.Empty)
                {
                    xmlInformation = xmlInformation.Replace(">?<", ">\u2002<");
                    xmlInformation = xmlInformation.Replace(">?##**##", ">##**##");
                }

                byte[] notificationDocument = null;
                string xsltPath = string.Empty;

                if (xmlInformation != string.Empty)
                {
                    if (userType == "Police")
                    {
                        xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\SpecialOrderAgreedRoute.xslt";
                        notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits);
                    }
                    else
                    {
                        xmlInformation = documentService.GetLoggedInUserAffectedStructureDetailsByESDALReference(xmlInformation, esdalRefno, SessionInfo, UserSchema.Sort, "agreed", Convert.ToInt32(SessionInfo.OrganisationId));

                        xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\SpecialOrderAgreedRoute.xslt";
                        notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "MovementPDF");
                    }
                }

                if (notificationDocument != null)
                {
                    System.IO.MemoryStream pdfStream = new System.IO.MemoryStream(notificationDocument);

                    return new FileStreamResult(pdfStream, "application/pdf");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data is not valid for current movement therefore PDF document will not be generated.');</script>");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/PrintAgreedReport, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion

        #region GetLoggedInUserAffectedStructureDetails
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlInformation"></param>
        /// <returns></returns>
        public string GetLoggedInUserAffectedStructureDetails(string xmlInformation, long notificationId, long NENInboxID = 0, int NENIdVal = 0, long OrganisationId = 0, int UserTypeId = 0)
        {

            STP.Domain.RouteAssessment.StructuresModel struInfo = new STP.Domain.RouteAssessment.StructuresModel();
            if (NENInboxID != 0 && NENIdVal != 0)
            {
                DrivingInstructionModel DIInfo = notificationDocService.GetNENRouteDescription(NENInboxID, (int)OrganisationId);
                struInfo.AffectedStructures = DIInfo.AffectedStructures;
            }
            else
            {
                struInfo = notificationDocService.GetStructuresXML(Convert.ToInt32(notificationId));
            }
            int organisationId = Convert.ToInt32(OrganisationId);

            string recipientXMLInformation = string.Empty;
            int userTypeId = UserTypeId;
            string errormsg = "No data found";

            Byte[] affectedPartiesArray = struInfo.AffectedStructures;
            byte[] outBoundDecryptString = null;

            XmlDocument xmlDoc = new XmlDocument();

            if (affectedPartiesArray != null)
            {
                try
                {
                    recipientXMLInformation = Encoding.UTF8.GetString(affectedPartiesArray, 0, affectedPartiesArray.Length);

                    xmlDoc.LoadXml(recipientXMLInformation);
                }
                catch (System.Xml.XmlException XE)
                {
                    outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(affectedPartiesArray);
                }
            }

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\NotificationAffectedStructures.xslt");

            string result = STP.Common.General.XsltTransformer.Trafo(outBoundDecryptString, path, organisationId, userTypeId, out errormsg);

            xmlInformation = xmlInformation.Replace("</OutboundNotification>", "<StructureDetails>");

            xmlInformation += result;

            xmlInformation += "</StructureDetails></OutboundNotification>";

            xmlInformation = xmlInformation.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");

            return xmlInformation;
        }
        #endregion

        #region appendRouteDetails
        /// <summary>
        /// Get Route Details 
        /// </summary>
        /// <param name="xmlInformation"></param>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        public string appendRouteDetails(string xmlInformation, double notificationId, int ISNEN = 0, long NENInboxId = 0, int organisationId = 0) //TODO chirag
        {
            //TODO chirag
            if (!string.IsNullOrEmpty(xmlInformation))
            {
                string xmlRouteDetails = "";
                string xmlRouteImperialDetails = "";
                if (ISNEN == 0)
                {
                    xmlRouteDetails = getRouteDetails(Convert.ToInt32(notificationId));//For existing ESDAL2 Notification
                }
                else
                {
                    try
                    {
                        if (NENInboxId > 0)
                        {
                            xmlRouteDetails = getNENRouteDetails(NENInboxId, organisationId, ISNEN, Convert.ToInt32(notificationId), 1);//For NEN Notification this is modified to fetch the latest analysis for hte input inbox id and organisation id
                            xmlRouteImperialDetails = getNENRouteDetails(NENInboxId, organisationId, ISNEN, Convert.ToInt32(notificationId), 2);//For NEN Notification this is modified to fetch the latest analysis for hte input inbox id and organisation id

                            XmlSerializer deserializer = new XmlSerializer(typeof(NotificationXSD.OutboundNotificationStructure));

                            StringReader stringReader = new StringReader(xmlInformation);
                            XmlTextReader xmlReader = new XmlTextReader(stringReader);
                            object obj = deserializer.Deserialize(xmlReader);

                            NotificationXSD.OutboundNotificationStructure XmlData = (NotificationXSD.OutboundNotificationStructure)obj;
                            XmlData.RouteParts[0].Route = xmlRouteDetails;
                            XmlData.RouteParts[0].RouteImperial = xmlRouteImperialDetails;

                            GenerateXML gxml = new GenerateXML();
                            XMLModel model = gxml.GenerateNotificationXML(0, XmlData, NotificationXSD.NotificationTypeType.police);
                            xmlInformation = model.ReturnXML;
                        }
                    }
                    catch (Exception ex)
                    {
                        return xmlRouteDetails;
                    }
                }

                xmlInformation = xmlInformation.Replace("</OutboundNotification>", "<RouteDescription>");

                xmlInformation += xmlRouteDetails;

                xmlInformation += "</RouteDescription></OutboundNotification>";

                xmlInformation = xmlInformation.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
            }
            return xmlInformation;

        }
        #endregion

        #region Below function is for NEN
        /// <summary>
        /// getNENRouteDetails for NEN project
        /// </summary>
        /// <param name="NENInboxId"></param>
        /// <param name="organisationId"></param>
        /// <returns></returns>
        public string getNENRouteDetails(long NENInboxId, int organisationId, int isNen , int notificationId, int flag = 1)
        {
            string routedesc = string.Empty;
            string errormsg = "No data found";
            string result = string.Empty;
            Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
            //For RM#4311 Change
            string[] separators = { "Split" };
            string resultPart = string.Empty;
            string FinalResult = string.Empty;
            string[] versionSplit = new string[] { };
            StringBuilder sb = new StringBuilder();
            //End

            DrivingInstructionModel DrivingInstructionInfo = new DrivingInstructionModel();

            if (isNen == 1)
            {
                DrivingInstructionInfo = notificationDocService.GetNENRouteDescription(NENInboxId, organisationId);
            }
            else if (isNen == 2)
            {
                RouteAnalysisModel routeAnalysisModel = notificationDocService.GetApiRouteAssessmentDetails(notificationId, organisationId, isNen);
                DrivingInstructionInfo.DrivingInstructions = routeAnalysisModel.DrivingInstructions;
            }


            string path = "";
            if (flag == 1)
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"XSLT\RouteDetails.xslt");
            else
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"XSLT\RouteDetailsImperial.xslt");

            result = STP.Common.General.XsltTransformer.Trafo(DrivingInstructionInfo.DrivingInstructions, path, out errormsg);

            //For RM#4311
            if (result != null && result.Length > 0)
            {
                versionSplit = result.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            }


            foreach (string part in versionSplit)
            {
                resultPart = string.Empty;

                if (part != null && part.Length > 0 && part.IndexOf("Alternative end # 1") != -1 && part.IndexOf("Alternative end # 2") == -1)
                {
                    resultPart = part.Replace("arrive at destination", "<u><b>Alternative end # 2:</b></u> arrive at destination");
                    sb.Append(resultPart);
                }
                else
                {
                    resultPart = part;
                    sb.Append(resultPart);
                }
            }

            if (sb.ToString() != null || sb.ToString() != "")
            {
                FinalResult = sb.ToString();
            }
            //End

            FinalResult = FinalResult.Replace("<u>", "##us##");
            FinalResult = FinalResult.Replace("</u>", "##ue##");

            FinalResult = FinalResult.Replace("<b>", "#bst#");
            FinalResult = FinalResult.Replace("</b>", "#be#");

            FinalResult = _htmlRegex.Replace(FinalResult, string.Empty);

            FinalResult = FinalResult.Replace("Start of new part", "");

            return FinalResult;
        }
        #endregion

        #region getRouteDetails
        /// <summary>
        /// generate routedescription node detail by processing routedescription xml
        /// </summary>
        /// <param name="NotificationID">notification id</param>
        /// <returns></returns>
        public string getRouteDetails(int NotificationID)
        {
            string messg = "OutboundDAO/getRouteDetails?NotificationID=" + NotificationID;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            string routedesc = string.Empty;
            string errormsg = "No data found";
            string result = string.Empty;
            Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
            //For RM#4311 Change
            string[] separators = { "Split" };
            string resultPart = string.Empty;
            string FinalResult = string.Empty;
            string[] versionSplit = new string[] { };
            StringBuilder sb = new StringBuilder();
            //End

            DrivingInstructionModel DrivingInstructionInfo = notificationDocService.GetRouteDescription(NotificationID);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"XSLT\RouteDetails.xslt");

            result = STP.Common.General.XsltTransformer.Trafo(DrivingInstructionInfo.DrivingInstructions, path, out errormsg);

            //For RM#4311
            if (result != null && result.Length > 0)
            {
                versionSplit = result.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            }


            foreach (string part in versionSplit)
            {
                resultPart = string.Empty;

                if (part != null && part.Length > 0 && part.IndexOf("Alternative end # 1") != -1 && part.IndexOf("Alternative end # 2") == -1)
                {
                    resultPart = part.Replace("arrive at destination", "<u><b>Alternative end # 2:</b></u> arrive at destination");
                    sb.Append(resultPart);
                }
                else
                {
                    resultPart = part;
                    sb.Append(resultPart);
                }
            }

            if (sb.ToString() != null || sb.ToString() != "")
            {
                FinalResult = sb.ToString();
            }
            //End

            //if (result != null && result.Length > 0 && result.IndexOf("Alternative end # 1") != -1 && result.IndexOf("Alternative end # 2") == -1)
            //{
            //    result = result.Replace("arrive at destination", "<u><b>Alternative end # 2:</b></u> arrive at destination");
            //}

            FinalResult = FinalResult.Replace("<u>", "##us##");
            FinalResult = FinalResult.Replace("</u>", "##ue##");

            FinalResult = FinalResult.Replace("<b>", "#bst#");
            FinalResult = FinalResult.Replace("</b>", "#be#");

            FinalResult = _htmlRegex.Replace(FinalResult, string.Empty);

            FinalResult = FinalResult.Replace("Start of new part", "");

            //result = result.Replace("Start\r\n", ": Start\r\n");

            //result = result.Replace("arrive at destination.", "arrive at destination.<hr width='100%'></hr>");

            //result = result.Remove(result.LastIndexOf("<hr width='100%'></hr>"));

            return FinalResult;
        }
        #endregion

        #region AppendNENRouteDetails
        /// <summary>
        /// appendNENRouteDetails
        /// </summary>
        /// <param name="xmlInformation"></param>
        /// <param name="notificationId"></param>
        /// <param name="NENInboxId"></param>
        /// <returns></returns>
        private string appendNENRouteDetails(string xmlInformation, long notificationId, long NENInboxId, int organisationId, int nenVal = 0, string contentRefNum = "")
        {
            XmlDocument obj = new XmlDocument();
            obj.LoadXml(xmlInformation);
            string oldStartRoute = "", oldEndRoute = "", oldLegNum = "", oldName = "", oldStartPoint = "", oldEndPoint = "";
            List<STP.Domain.MovementsAndNotifications.Notification.NENUpdateRouteDet> NENLatestRouteObj = new List<STP.Domain.MovementsAndNotifications.Notification.NENUpdateRouteDet>();

            if (nenVal == 1)
            {
                NENLatestRouteObj = nenNotificationService.GetNENRouteList(NENInboxId, organisationId);
            }

            else if (nenVal == 2)
            {
                Domain.MovementsAndNotifications.Notification.NENUpdateRouteDet nENUpdateRouteDet;
                List<AppRouteList> nenRouteList = routesService.GetPlannedNenAPIRouteList(contentRefNum, organisationId);
                foreach( var item in nenRouteList)
                {
                    nENUpdateRouteDet = new Domain.MovementsAndNotifications.Notification.NENUpdateRouteDet
                    {
                        RouteId = item.RouteID,
                        RouteName = item.RouteName,
                        PartNo = item.PartNo,
                        FromAddress = item.FromAddress,
                        ToAddress = item.ToAddress
                    };
                    NENLatestRouteObj.Add(nENUpdateRouteDet);
                }
            }
            //retreive max no of pieces
            XmlNodeList parentNode = obj.GetElementsByTagName("JourneyFromTo");
            // loop through all AID nodes
            foreach (XmlNode node in parentNode)
            {
                try
                {
                    oldStartRoute = node["From"].InnerText;
                    oldEndRoute = node["To"].InnerText;
                }
                catch (Exception e)
                {
                    oldStartRoute = "";
                    oldEndRoute = "";
                }
            }
            if (NENLatestRouteObj != null)
            {
                if (oldStartRoute != "" && oldEndRoute != "")
                {
                    xmlInformation = xmlInformation.Replace(">" + oldStartRoute + "</From>", ">" + NENLatestRouteObj[0].FromAddress + "</From>");
                    xmlInformation = xmlInformation.Replace(">" + oldEndRoute + "</To>", ">" + NENLatestRouteObj[0].ToAddress + "</To>");
                }
            }
            parentNode = obj.GetElementsByTagName("RoutePartListPosition");
            // loop through all AID nodes
            foreach (XmlNode node in parentNode)
            {
                try
                {
                    if (node.ChildNodes.Count > 0)
                    {
                        try
                        {
                            oldLegNum = node.ChildNodes[0].ChildNodes[0].InnerText;//LegNumber
                        }
                        catch (Exception e)
                        {
                            oldLegNum = "";
                        }
                        try
                        {
                            oldName = node.ChildNodes[0].ChildNodes[1].InnerText;//Name
                        }
                        catch (Exception e)
                        {
                            oldName = "";
                        }
                        try
                        {
                            oldStartPoint = node.ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;//StartDescription
                        }
                        catch (Exception e)
                        {
                            oldStartPoint = "";
                        }
                        try
                        {
                            oldEndPoint = node.ChildNodes[0].ChildNodes[2].ChildNodes[1].ChildNodes[0].ChildNodes[0].InnerText;//EndDescription
                        }
                        catch (Exception e)
                        {
                            oldEndPoint = "";
                        }

                    }
                }
                catch (Exception e)
                {
                    oldLegNum = "";
                    oldName = "";
                    oldStartPoint = "";
                    oldEndPoint = "";
                }
                if (oldLegNum != "")
                {
                    if (oldLegNum == "1")
                    {
                        if (NENLatestRouteObj != null && oldName != "" && oldStartPoint != "" && oldEndPoint != "" && NENLatestRouteObj.Count > 0)
                        {
                            node.ChildNodes[0].ChildNodes[1].InnerText = NENLatestRouteObj[0].RouteName;
                            node.ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText = NENLatestRouteObj[0].FromAddress;
                            node.ChildNodes[0].ChildNodes[2].ChildNodes[1].ChildNodes[0].ChildNodes[0].InnerText = NENLatestRouteObj[0].ToAddress;
                        }
                    }
                    else if (oldLegNum == "2")
                    {
                        if (NENLatestRouteObj != null && oldName != "" && oldStartPoint != "" && oldEndPoint != "" && NENLatestRouteObj.Count > 1)
                        {
                            node.ChildNodes[0].ChildNodes[1].InnerText = NENLatestRouteObj[1].RouteName;
                            node.ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText = NENLatestRouteObj[1].FromAddress;
                            node.ChildNodes[0].ChildNodes[2].ChildNodes[1].ChildNodes[0].ChildNodes[0].InnerText = NENLatestRouteObj[1].ToAddress;
                        }
                    }
                }
            }

            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                obj.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                xmlInformation = stringWriter.GetStringBuilder().ToString();
            }

            return xmlInformation;
        }
        #endregion

        #region RemoveDispansationNode_of_Outboundxml
        //===============================================================================================
        /// <summary>
        /// Function to remove Dispansation node from outbound xml string
        /// </summary>
        /// <param name="xmlInformation">xml string variable</param>
        /// <param name="DRN">Dispansation Registration Number</param>
        /// <returns>modified xml </returns>
        private string RemoveDispansationNode_of_Outboundxml(string xmlInformation, string DRN)
        {

            XmlDocument docOutboundNode = new XmlDocument();
            XDocument docOutBoundNodeLinq = new XDocument();
            try
            {
                if (string.IsNullOrEmpty(DRN) == false)
                {
                    XNamespace ns = "http://www.esdal.com/schemas/core/notification";

                    docOutBoundNodeLinq = XDocument.Parse(xmlInformation);

                    var DRNArray = DRN.Split(',');

                    var outBoundDispensations = (from d in docOutBoundNodeLinq.Descendants(ns + "Dispensations").Descendants(ns + "OutboundDispensation")
                                                 where DRNArray.Contains(d.Elements(ns + "DRN").ElementAtOrDefault(0).Value) == false
                                                 select d).ToList();


                    if (outBoundDispensations != null || outBoundDispensations.Count > 0)
                    {
                        foreach (var xItems in outBoundDispensations)
                        {
                            xItems.Remove();
                        }
                    }


                    return docOutBoundNodeLinq.ToString();
                }
                else
                {

                    XNamespace ns = "http://www.esdal.com/schemas/core/notification";

                    docOutBoundNodeLinq = XDocument.Parse(xmlInformation);

                    var outBoundDispensations = (from d in docOutBoundNodeLinq.Descendants(ns + "Dispensations")
                                                 select d).ToList();

                    if (outBoundDispensations != null || outBoundDispensations.Count > 0)
                    {
                        foreach (var xItems in outBoundDispensations)
                        {
                            xItems.Remove();
                        }
                    }

                    return docOutBoundNodeLinq.ToString();
                }

            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        //-----------------------------------------------------------------------------------------------
        #endregion

        #region LibraryNotesList
        /// <summary>
        /// Display collaboration history list
        /// </summary>
        /// <param name="page">Page </param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="DocumentId">Document id</param>
        /// <returns>Return collaboration history list</returns>
        public ActionResult LibraryNotesList(int? page, int? pageSize, int DocumentId, bool SORTCollab = false)
        {
            try
            {

                #region Session Check
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];
                #endregion
                #region Paging Part
                int pageNumber = (page ?? 1);

                if (pageSize == null)
                {
                    if (Session["PageSize"] == null)
                    {
                        Session["PageSize"] = 10;
                    }
                    pageSize = (int)Session["PageSize"];
                }
                else
                {
                    Session["PageSize"] = pageSize;
                }

                ViewBag.pageSize = pageSize;
                ViewBag.page = pageNumber;

                #endregion
                List<STP.Domain.RouteAssessment.LibraryNotes> result = routeAssessmentService.GetLibraryNotes((int)SessionInfo.OrganisationId, 0, Convert.ToInt32(SessionInfo.UserId));

                if (result.Count > 0)
                    ViewBag.TotalCount = Convert.ToInt32(result.Count);
                else
                    ViewBag.TotalCount = 0;

                var CollaborationListPaged = new StaticPagedList<STP.Domain.RouteAssessment.LibraryNotes>(result, pageNumber, (int)pageSize, ViewBag.TotalCount);
                ViewBag.SORTCollab = SORTCollab;
                return PartialView("LibraryNotesList", CollaborationListPaged);
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Notification/DisplayCollaborationHistoryList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region GetVehicleImage
        public void GetVehicleImage(dynamic movementVehicleList)
        {
            foreach (var vehicle in movementVehicleList)
            {
                ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
                int MovementXmlTypeId = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleXmlMovementTypeMapping((VehicleXmlMovementType)vehicle.VehiclePurpose);

                MovementClassificationConfig mvClassConfig = compConfigObj.GetMovementClassificationConfig(MovementXmlTypeId);
                //Domain.VehicleAndFleets.Component.VehicleConfiguration vehicleConfigObj = mvClassConfig.GetVehicleConfiguration(vehicle.VehicleType);
                vehicle.VehicleNameList.Clear();
                foreach (var vehComp in vehicle.VehicleCompList)
                {
                    MovementClassificationConfig moveClassConfigObj = compConfigObj.GetListOfVehicleComponents((int)vehComp.ComponentTypeId);
                    VehicleComponent vehclCompObj = moveClassConfigObj.GetVehicleComponent((int)vehComp.ComponentTypeId, (int)vehComp.ComponentSubTypeId);
                    if (vehclCompObj != null)
                        vehicle.VehicleNameList.Add(vehclCompObj.vehicleComponentSubType.ImageName);
                }

                //foreach (VehicleConfigList component in vehicle.VehicleCompList)
                //{
                //    foreach (STP.Domain.VehicleAndFleets.Component.VehicleComponentType comType in vehicleConfigObj.vehicleConfigType.LstVehcCompTypes)
                //    {
                //        if (comType.ComponentTypeId == component.ComponentTypeId)
                //        {
                //            vehicle.VehicleNameList.Add(comType.ImageName);
                //        }
                //    }
                //}
            }
        }
        #endregion

        #region ManageNotesOnEscort
        [HttpPost]
        public JsonResult ManageNotesOnEscort(MovementModel movement)
        {
            try
            {
                #region Session Check
                if (Session["UserInfo"] == null)
                {
                    return Json("expire");
                }
                #endregion
                #region Page access check
                #endregion

                bool result = movementsService.ManageNotesOnEscort(movement);


                //-----INTELLIZENZ PART FOR NEN NOTIFICATIONS AUDIT LOG SAVING-----------
                #region-------INTELLIZENZ PART FOR NEN NOTIFICATIONS AUDIT LOG SAVING----------------      
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                if (SessionInfo.UserName != null && result == true)
                {
                    #region AUDIT LOGS FOR SAVING Notes On Escort ESDAL/NEN NOTIFICATIONS BY SOA OR POLICE OR THROUGH HELPDESK
                    AuditLogIdentifiers auditLogType = new AuditLogIdentifiers();
                    string ErrMsg = string.Empty;
                    int NEN_ID = 0;
                    auditLogType.NENID = NEN_ID > 0 ? NEN_ID : 0;
                    if (SessionInfo.UserSchema == UserSchema.Portal)
                    {
                        auditLogType.AuditActionType = AuditActionType.Save_Notes_On_Escort;
                        auditLogType.NoteOnEscort = movement.NotesOnEscort;
                        auditLogType.HelpDeskUserID = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                        auditLogType.HelpDeskUsername = SessionInfo.HelpdeskUserName;
                        auditLogType.ESDALNotificationNo = movement.NotificationCode.Replace("~", "#"); ;
                        auditLogType.InboxItemId = movement.InboxId.ToString();
                        var ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                        auditLogType.DateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, ukTimeZone).ToString("dd/MM/yyyy HH:mm:ss"); 
                        string auditLogDescp = AuditLog.GetNENNotifAuditLog(SessionInfo, auditLogType, out ErrMsg);
                        int user_ID = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                        long auditLogResult = auditlogService.SaveNotifAuditLog(auditLogType, auditLogDescp, user_ID, SessionInfo.OrganisationId);
                    }

                    #endregion
                }
                #endregion---------------end--------------------------------
                //------END HERE---------------------------------------------------------
                return Json(result == true ? "true" : "false");
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/ManageNotesOnEscort, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion

        #region ManageInternalNotes
        [HttpPost]
        public JsonResult ManageInternalNotes(MovementModel movement)
        {
            try
            {
                #region Session Check
                if (Session["UserInfo"] == null)
                {
                    return Json("expire");
                }
                #endregion
                #region Page access check
                #endregion
                bool result = movementsService.ManageInternalNotes(movement);

                //-----INTELLIZENZ PART FOR NEN NOTIFICATIONS AUDIT LOG SAVING-----------
                #region-------INTELLIZENZ PART FOR NEN NOTIFICATIONS AUDIT LOG SAVING----------------
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                if (SessionInfo.UserName != null && result == true)
                {
                    #region AUDIT LOGS FOR SAVING Internal Notes ESDAL/NEN NOTIFICATIONS BY SOA OR POLICE OR THROUGH HELPDESK
                    AuditLogIdentifiers auditLogType = new AuditLogIdentifiers();
                    string ErrMsg = string.Empty;
                    int NEN_ID = 0;
                    auditLogType.NENID = NEN_ID > 0 ? NEN_ID : 0;
                    if (SessionInfo.UserSchema == UserSchema.Portal)
                    {
                        auditLogType.AuditActionType = AuditActionType.Save_Internal_Notes;
                        auditLogType.InternalNotes = movement.InternalNotes;
                        auditLogType.HelpDeskUserID = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                        auditLogType.HelpDeskUsername = SessionInfo.HelpdeskUserName;
                        auditLogType.ESDALNotificationNo = movement.NotificationCode.Replace("~", "#"); ;
                        auditLogType.InboxItemId = movement.InboxId.ToString();
                        var ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                        auditLogType.DateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, ukTimeZone).ToString("dd/MM/yyyy HH:mm:ss"); 
                        string auditLogDescp = AuditLog.GetNENNotifAuditLog(SessionInfo, auditLogType, out ErrMsg);
                        int user_ID = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                        long auditLogResult = auditlogService.SaveNotifAuditLog(auditLogType, auditLogDescp, user_ID, SessionInfo.OrganisationId);
                    }

                    #endregion
                }
                #endregion---------------end--------------------------------
                //------END HERE---------------------------------------------------------
                return Json(result == true ? "true" : "false");
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/ManageInternalNotes, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion

        #region GetInboxDetails
        [HttpGet]
        public JsonResult GetInboxDetails(string esdalRefNumber)
        {
            try
            {
                #region Session Check
                if (Session["UserInfo"] == null)
                {
                    return Json("expire");
                }
                #endregion

                #region Page access check

                #endregion

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                esdalRefNumber = esdalRefNumber.Replace("~", "#");

                MovementModel movement = movementsService.GetInboxItemDetails(esdalRefNumber, SessionInfo.OrganisationId);

                string encryptedInboxId = MD5EncryptDecrypt.EncryptDetails(Convert.ToString(movement.InboxId));

                string encryptStatusName = MD5EncryptDecrypt.EncryptDetails(movement.StatusName);
                string encryptStatus = movement.Route;
                string encryptStatusType = MD5EncryptDecrypt.EncryptDetails(Convert.ToString(movement.ItemStatus));
                string encryptNen_Id = MD5EncryptDecrypt.EncryptDetails(Convert.ToString(movement.NenId));
                string result = encryptedInboxId + "|" + encryptStatusName + "|" + encryptStatus + "|" + encryptNen_Id + "|" + encryptStatusType;


                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/ManageNotesOnEscort, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion

        #region Movement Map-Authorization Movement General Tab
        public ActionResult MovementMap(string contRefNum = null, long versionId = 0, long nenId = 0, bool isNen = false, bool isNenApi = false)
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                Session["MovVersionID"] = versionId;
                List<AppRouteList> ImportedRoutelist = new List<AppRouteList>();
                ViewBag.IsMoveVersion = false;
                Session["RouteFlag"] = 3;
                int orgId = Convert.ToInt32(SessionInfo.OrganisationId);
                if (isNen)
                {
                    int userId = Convert.ToInt32(SessionInfo.UserId);
                    int inboxId = Convert.ToInt32(Session["NENINBOX_ITEM_ID"]);
                    ImportedRoutelist = routesService.GetPlannedNenRouteList(nenId, userId, inboxId, orgId);
                }
                else if(isNenApi)
                {
                    ImportedRoutelist = routesService.GetPlannedNenAPIRouteList(contRefNum, orgId);
                }
                else
                {
                    ImportedRoutelist = routesService.NotifVR1RouteList(0, contRefNum, versionId, versionId > 0 ? UserSchema.Sort : SessionInfo.UserSchema);
                    if (versionId > 0)
                    {
                        ViewBag.IsMoveVersion = true;
                    }
                }
                ViewBag.HfRouteIdMap = ImportedRoutelist != null && ImportedRoutelist.Any() ? ImportedRoutelist.FirstOrDefault().RouteID : 0;
                return PartialView("_MovementMap", ImportedRoutelist);
            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex.Message);
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/ListImportedRouteFromLibrary, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");

            }

        }
        #endregion

        #region Plan Movement Functionality

        #region CreateMovement
        public ActionResult CreateMovement()
        {
            TempData["CreateApp"] = true;
            Session["movement_SearchData"] = null;
            Session["g_SearchData"] = null;
            Session["movement_AdvancedSearchData"] = null;
            Session["g_AdvancedSearchData"] = null;
            Session["notivso"] = null;
            Session["VSOVehicleClassificationType"] = null;
            Session["movementClassificationId"] = null;
            Session["movementClassificationName"] = null;
            return RedirectToAction("PlanMovement", "Movements");
        }
        #endregion

        #region CreateVSOMovement
        public ActionResult CreateVSOMovement()
        {
            TempData["CreateVSO"] = true;
            TempData["CreateApp"] = true;
            return RedirectToAction("PlanMovement", "Movements");
        }
        #endregion

        #region SelectVSOType
        public ActionResult SelectVSOType(long NotiVSO)
        {
            Session["notivso"] = NotiVSO;
            Session["VSOVehicleClassificationType"] = (int)VehicleClassificationType.VehicleSpecialOrder;
            Session["movementClassificationId"] = (int)VehiclePurpose.VehicleSpecialOrder;
            Session["movementClassificationName"] = "Vehicle special order";
            if (new SessionData().E4_AN_MovemenTypeClass != null)
            {
                if (NotiVSO == (int)VSOType.soa)
                {
                    new SessionData().E4_AN_MovemenTypeClass.PoliceNoticePeriod = 0;
                    new SessionData().E4_AN_MovemenTypeClass.SOANoticePeriod = 2;
                }
                else if (NotiVSO == (int)VSOType.police)
                {
                    new SessionData().E4_AN_MovemenTypeClass.PoliceNoticePeriod = 2;
                    new SessionData().E4_AN_MovemenTypeClass.SOANoticePeriod = 0;
                }
                else if (NotiVSO == (int)VSOType.soapolice)
                {
                    new SessionData().E4_AN_MovemenTypeClass.PoliceNoticePeriod = 2;
                    new SessionData().E4_AN_MovemenTypeClass.SOANoticePeriod = 2;
                }
            }
            return Json("Success");
        }
        #endregion

        #region OpenMovement
        public ActionResult OpenMovement(long revisionId = 0, long notifId = 0)
        {
            Session["notivso"] = null;
            Session["VSOVehicleClassificationType"] = null;
            Session["movementClassificationId"] = null;
            Session["movementClassificationName"] = null;
            TempData["NotifId"] = notifId;
            TempData["RevisionId"] = revisionId;
            new SessionData().Wf_An_ApplicationWorkflowId = null;
            return RedirectToAction("PlanMovement", "Movements");
        }
        #endregion

        #region Plan Movement

        public ActionResult PlanMovement()
        {
            Session["OrgSearchString"] = "";
            Session["OrgSearchCode"] = "";
            Session["RouteAssessmentFlag"] = "";
            new SessionData().E4_AN_MovemenTypeClass = null;
            bool isCreateApp = false;
            long notificationId = 0;
            long appRevisionId = 0;
            bool isVSO = false;
            if (TempData["CreateApp"] != null)
            {
                isCreateApp = Convert.ToBoolean(TempData["CreateApp"]);
                TempData["CreateApp"] = false;
            }

            if (TempData["NotifId"] != null)
            {
                notificationId = Convert.ToInt64(TempData["NotifId"]);
                TempData["NotifId"] = 0;
            }
            ViewBag.CreateVSO = false;
            if (TempData["CreateVSO"] != null)
            {
                ViewBag.CreateVSO = true;
                isVSO = true;
            }
            Session["movement_SORTFilter"] = null;
            Session["candidate_SORTFilter"] = null;
            Session["candidate_SORTFilterAdvanced"] = null;
            Session["g_VehicleConfigSearch"] = null;
            Session["g_VehicleConfigTypeSearch"] = null;
            Session["g_VehicleConfigIntendSearch"] = null;
            Session["movementClassificationId"] = null;
            Session["movementClassificationName"] = null;
            Session["AmendVehicle"] = null;
            Session["g_VehicleComponentSearch"] = null;
            Session["g_VehicleTypeSearch"] = null;

            if (TempData["RevisionId"] != null)
            {
                appRevisionId = Convert.ToInt64(TempData["RevisionId"]);
                TempData["RevisionId"] = 0;
            }

            if (isCreateApp)
            {
                new SessionData().Wf_An_ApplicationNotificationStarted = true;
                new SessionData().Wf_An_ApplicationWorkflowId = string.Empty;
                new SessionData().E4_AN_MovemenTypeClass = null;
                new SessionData().E4_AN_PlanMovement = null;
                if (isVSO)
                {
                    Session["VSOVehicleClassificationType"] = (int)VehicleClassificationType.VehicleSpecialOrder;
                    Session["movementClassificationId"] = (int)VehiclePurpose.VehicleSpecialOrder;
                    Session["movementClassificationName"] = "Vehicle special order";
                }
            }
            else
            {
                var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                if ((notificationId != 0 || appRevisionId != 0) && applicationNotificationManagement.IsThisMovementExist(notificationId, appRevisionId, out string workflowKey))
                    new SessionData().Wf_An_ApplicationWorkflowId = workflowKey;
                PlanMvmntPayLoad mvmntPayLoad = applicationNotificationManagement.GetPlanMvmtPayload();
                ViewBag.PlanMvmntPayLoad = mvmntPayLoad;
            }
            return View();
        }
        #endregion

        #region OnBackButtonClick
        public void OnBackButtonClick(int stepFlag, double subStepFlag)
        {
            Session["StepFlag"] = stepFlag;
            try
            {
                Session["PreviousVehicleListPage"] = null;
                var currentActivity = new ApplicationNotificationManagement(applicationNotificationWorkflowService).GetCurrentActivity();
                if (WorkflowTaskFinder.FindPreviousTask("HaulierApplicationBack", stepFlag, subStepFlag, out dynamic workflowPayload, out string activityLog) != string.Empty && currentActivity.Length > 0)
                {
                    var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                    dynamic dataPayload = new ExpandoObject();
                    dataPayload.workflowActivityLog = applicationNotificationManagement.SetWorkflowLog(activityLog);
                    WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                    {
                        data = dataPayload,
                        workflowData = workflowPayload
                    };
                    applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);
                }
            }
            catch
            {
                //error
            }
        }
        #endregion

        #region SelectVehicle WFANStep1
        public ActionResult SelectVehicle(int contactId = 0, int movementId = 0, int vehicleError = 0, bool IsVSO = false, int flag = 0, int organisationId = 0)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            if (organisationId == 0)
            {
                organisationId = (int)SessionInfo.OrganisationId;
            }
            if (SessionInfo.UserTypeId == UserType.Sort)
            {
                organisationId = (int)Session["SORTOrgID"];
                Session["SORTContactId"] = contactId;
            }

            ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
            List<STP.Domain.VehicleAndFleets.Component.VehicleComponentType> listVehicleCompObj = compConfigObj.GetListOfVehicleComponent();
            int iMovementId = movementId;
            if (flag == 3)
            {
                iMovementId = 0;
                new SessionData().E4_AN_MovemenTypeClass = null;
            }
            List<VehicleList> favVehicles = vehicleconfigService.GetFavouriteVehicles(organisationId, iMovementId, UserSchema.Portal);
            ViewBag.FavoriteVehicles = favVehicles;
            ViewBag.ComponentTypeList = listVehicleCompObj;
            ViewBag.VehicleError = vehicleError;
            return PartialView("_MovementSelectVehicle");
        }
        #endregion

        #region MovementSelectedVehicles WFANStep2
        public ActionResult MovementSelectedVehicles(long movementId = 0, int isVehicleModify = 0, bool isBackCall = false)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            int organisationId = SessionInfo.UserTypeId == UserType.Sort ? (int)Session["SORTOrgID"] : (int)SessionInfo.OrganisationId;
            string workFlowKey = new SessionData().Wf_An_ApplicationWorkflowId;
            PlanMvmntPayLoad mvmntPayLoad = new PlanMvmntPayLoad();
            var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
            if (workFlowKey != string.Empty || workFlowKey != WorkflowActivityConstants.Gn_Failed)
            {
                mvmntPayLoad = applicationNotificationManagement.GetPlanMvmtPayload();
                if (mvmntPayLoad.VehicleMoveId != 0)
                    mvmntPayLoad.VehicleMoveId = movementId;
            }

            PerformVehicelAssessment vehicelAssessment = new PerformVehicelAssessment();
            if (mvmntPayLoad.IsAppClone || mvmntPayLoad.IsRevise || mvmntPayLoad.IsNotifClone || mvmntPayLoad.IsRenotify)
            {
                movementId = mvmntPayLoad.VehicleMoveId;
            }

            int vsoVehicleClass = Session["VSOVehicleClassificationType"] != null ? (int)Session["VSOVehicleClassificationType"] : 0;
            int VehicleSpecialOrder = mvmntPayLoad.MovementKey > 0 ? mvmntPayLoad.VehicleClass : vsoVehicleClass;

            #region Not Back call
            if (!isBackCall)
            {
                if (mvmntPayLoad.IsAgreedNotified)
                    vehicelAssessment = PerformNotifyAppVehicleAssessment(movementId, SessionInfo.UserSchema, mvmntPayLoad);
                else if (VehicleSpecialOrder == (int)VehicleClassificationType.VehicleSpecialOrder)
                {
                    vehicelAssessment = PerformVSOVehicleAssessment(movementId, SessionInfo.UserSchema, mvmntPayLoad);
                    mvmntPayLoad.IsVSO = true;
                }
                else
                    vehicelAssessment = PerformVehicleAssessment(movementId, SessionInfo.UserSchema, mvmntPayLoad);

                if (vehicelAssessment.VehicleList!=null && vehicelAssessment.VehicleList.Count > 0)
                {
                    if (mvmntPayLoad.IsNotifyApp)
                        vehicelAssessment.MovementType.MovementType = mvmntPayLoad.MovementType;

                    mvmntPayLoad.CurrentMovementType = vehicelAssessment.MovementType.MovementType;
                    mvmntPayLoad.MvmntType = vehicelAssessment.MovementType;
                    mvmntPayLoad.VehicleClass = vehicelAssessment.VehicleError == 1 ? mvmntPayLoad.VehicleClass : vehicelAssessment.MovementType.VehicleClass;
                }
                else
                {
                    if (!mvmntPayLoad.IsNotifyApp && !mvmntPayLoad.IsNotifClone && !mvmntPayLoad.IsRenotify && !mvmntPayLoad.IsAppClone && !mvmntPayLoad.IsRevise && !mvmntPayLoad.IsVSO)
                        new SessionData().E4_AN_MovemenTypeClass = null;
                    if (!mvmntPayLoad.IsVSO)
                    {
                        Session["movementClassificationId"] = null;
                        Session["movementClassificationName"] = null;
                    }
                    return RedirectToAction("SelectVehicle", new
                    {
                        B7vy6imTleYsMr6Nlv7VQ =
                        Helpers.EncryptionUtility.Encrypt("movementId=" + movementId +
                        "&vehicleError=" + vehicelAssessment.VehicleError +
                        "&flag=" + isVehicleModify)
                    });
                }
            }
            #endregion

            #region Back call
            else
            {
                if (mvmntPayLoad.MovementKey == 0)
                {
                    vehicelAssessment.MovementType = (VehicleMovementType)TempData["MovementTypeClass"];
                    vehicelAssessment.VehicleList = (List<MovementVehicleConfig>)TempData["MovementSelectedVehicles"];
                }
                else
                {
                    vehicelAssessment.MovementType = mvmntPayLoad.MvmntType;
                    movementId = mvmntPayLoad.VehicleMoveId;
                    vehicelAssessment.VehicleList = vehicleconfigService.SelectMovementVehicle(mvmntPayLoad.VehicleMoveId, SessionInfo.UserSchema);
                }
                foreach (var item in vehicelAssessment.VehicleList)
                {
                    AssessedVehicleList assessedVehicle = new AssessedVehicleList();
                    if (mvmntPayLoad.MovementKey == 0)
                    {
                        assessedVehicle.MovementType = vehicelAssessment.MovementType.MovementType;
                        assessedVehicle.VehicleClass = vehicelAssessment.MovementType.VehicleClass;
                        assessedVehicle.VehiclePurpose = vehicelAssessment.MovementType.VehiclePurpose;
                    }
                    else
                    {
                        assessedVehicle.MovementType = mvmntPayLoad.MvmntType.MovementType;
                        assessedVehicle.VehicleClass = mvmntPayLoad.MvmntType.VehicleClass;
                        assessedVehicle.VehiclePurpose = mvmntPayLoad.MvmntType.VehiclePurpose;
                    }
                    assessedVehicle.VehicleId = item.VehicleId;
                    assessedVehicle.VehicleName = item.VehicleName;
                    vehicelAssessment.AssessedVehicles.Add(assessedVehicle);
                }
            }
            #endregion

            if (vehicelAssessment.VehicleList != null)
                vehicelAssessment.VehicleList.ForEach(item =>
                {
                    if (item.VehicleCompList != null)
                        item.VehicleCompList = item.VehicleCompList.OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                });

            ViewBag.PlanMvmntPayLoad = mvmntPayLoad;
            VehicleMovementType movementTypeClass = vehicelAssessment.MovementType;
            List<MovementVehicleConfig> vehicleDetails = vehicelAssessment.VehicleList;
            List<AssessedVehicleList> vehicleLists = vehicelAssessment.AssessedVehicles;

            #region Vehicle Highlight
            var vehicleClass = movementTypeClass.VehicleClass;
            var movementType = movementTypeClass.MovementType;
            if (vehicelAssessment.VehicleError == 1)
            {
                var isSoVr1 = mvmntPayLoad.IsNotifyApp || mvmntPayLoad.IsAppClone || mvmntPayLoad.IsRevise || ((mvmntPayLoad.IsNotifClone || mvmntPayLoad.IsRenotify) &&
                                                                                mvmntPayLoad.PrevMovType != (int)MovementType.notification);
                vehicleClass = mvmntPayLoad.MovementKey > 0 && isSoVr1 ? mvmntPayLoad.VehicleClass : movementTypeClass.VehicleClass;
                movementType = mvmntPayLoad.MovementKey > 0 && isSoVr1 ? mvmntPayLoad.PrevMovType : movementTypeClass.MovementType;
                foreach (var vehicleList in vehicleDetails.SelectMany(vehicleList => vehicleLists.Where(assessedList => assessedList.VehicleId == vehicleList.VehicleId
                && (vehicleClass != assessedList.VehicleClass || movementType != assessedList.MovementType || movementType == (int)MovementType.no_movement)).Select(assessedList => vehicleList)))
                {
                    vehicleList.HighlightVehicle = true;
                }
            }
            if (SessionInfo.UserSchema == UserSchema.Sort && vehicelAssessment.VehicleError == 10)
            {
                var isSoVr1 = mvmntPayLoad.IsNotifyApp || mvmntPayLoad.IsAppClone || mvmntPayLoad.IsRevise || ((mvmntPayLoad.IsNotifClone || mvmntPayLoad.IsRenotify) &&
                                                                               mvmntPayLoad.PrevMovType != (int)MovementType.notification);
                vehicleClass = mvmntPayLoad.MovementKey > 0 && isSoVr1 ? mvmntPayLoad.VehicleClass : movementTypeClass.VehicleClass;
                movementType = mvmntPayLoad.MovementKey > 0 && isSoVr1 ? mvmntPayLoad.PrevMovType : movementTypeClass.MovementType;
                vehicelAssessment.VehicleList[0].HighlightVehicle = true;
            }
            #endregion

            GetVehicleImage(vehicleDetails);

            if (vehicleDetails.Count > 0)
            {
                ViewBag.VehicleError = vehicelAssessment.VehicleError;
                TempData["VehicleError"] = vehicelAssessment.VehicleError;
                TempData["VehicleClassification"] = vehicleClass;
                TempData["VehicleMovementType"] = movementType;
                ViewBag.MovementSelectedVehicles = vehicleDetails;
                ViewBag.MovementTypeClass = movementTypeClass;
                TempData["MovementTypeClass"] = movementTypeClass;
                TempData["MovementSelectedVehicles"] = vehicleDetails;
                new SessionData().E4_AN_MovemenTypeClass = movementTypeClass;
                ViewBag.AssessedVehicleList = vehicleLists;
                ViewBag.MovementId = movementId;
                int vehiclePurpose;
                if (mvmntPayLoad != null && mvmntPayLoad.MvmntType != null)
                    vehiclePurpose = mvmntPayLoad.MvmntType.VehiclePurpose > 0 ? mvmntPayLoad.MvmntType.VehiclePurpose : vehicelAssessment.MovementType.VehiclePurpose;
                else
                    vehiclePurpose = vehicelAssessment.MovementType.VehiclePurpose;
                List<VehicleList> favVehicles = vehicleconfigService.GetFavouriteVehicles(organisationId, vehiclePurpose, UserSchema.Portal);
                ViewBag.FavoriteVehicles = favVehicles;
            }
            ViewBag.IsBackCall = isBackCall;

            #region Workflow Activity
            if (applicationNotificationManagement.IsThisMovementExist(mvmntPayLoad.NotificationId, mvmntPayLoad.RevisionId, out string workflowKey)
                && WorkflowTaskFinder.FindNextTask("HaulierApplication", WorkflowActivityTypes.An_Activity_VehicleAddUpdate, out dynamic workflowPayload) != string.Empty)
            {
                dynamic dataPayload = new ExpandoObject();

                if (mvmntPayLoad.VehicleAssignmentList == null)
                    mvmntPayLoad.VehicleAssignmentList = GetAssignedVehicleList(mvmntPayLoad, SessionInfo.UserSchema);
                if (isVehicleModify == 1)
                    mvmntPayLoad.IsRouteVehicleAssigned = false;
                dataPayload.PlanMvmntPayLoad = mvmntPayLoad;
                WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                {
                    data = dataPayload,
                    workflowData = workflowPayload
                };
                applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);
            }
            #endregion

            return PartialView("_MovementSelectedVehicles");
        }
        private PerformVehicelAssessment PerformVehicleAssessment(long movementId, string userSchema, PlanMvmntPayLoad movementPayLoad)
        {
            PerformVehicelAssessment vehicleAssessment = new PerformVehicelAssessment
            {
                VehicleList = vehicleconfigService.SelectMovementVehicle(movementId, userSchema)
            };
            if (vehicleAssessment.VehicleList!=null && vehicleAssessment.VehicleList.Count > 0)
            {
                List<AutoFillModel> autoFillModels = new List<AutoFillModel>();
                var autofillNeeded = true;
                if ((movementPayLoad.IsNotifyApp || movementPayLoad.IsRenotify) && !movementPayLoad.IsAgreedNotified)
                    autofillNeeded = false;
                if (autofillNeeded)
                    autoFillModels = AutofillVehicleDetails(userSchema, vehicleAssessment);

                List<VehicleMovementType> movementTypeList = new List<VehicleMovementType>();
                vehicleAssessment.VehicleList = vehicleAssessment.VehicleList.OrderBy(x => x.VehicleId).ToList();
                var vehicleCurrentPosition = 0;
                int k = 0;
                TempData["SortVr1"] = false;
                bool isConfigValid = true;

                #region Assessment
                foreach (var item in vehicleAssessment.VehicleList)
                {
                    AssessedVehicleList assessedVehicle = new AssessedVehicleList();
                    AssessMoveTypeParams moveTypeParams = new AssessMoveTypeParams
                    {
                        VehicleId = (int)item.VehicleId,
                        IsRoute = 0,
                        UserSchema = userSchema,
                        ForceApplication = userSchema == UserSchema.Sort,
                        PreviousMovementType = k == 0 ? null : movementTypeList[k - 1],
                        configuration = null
                    };
                    vehicleAssessment.MovementType = vehicleconfigService.AssessMovementType(moveTypeParams);
                    vehicleAssessment.MovementType.VehiclePurpose = item.VehiclePurpose;
                    assessedVehicle.MovementType = vehicleAssessment.MovementType.MovementType;
                    assessedVehicle.VehicleClass = vehicleAssessment.MovementType.VehicleClass;
                    assessedVehicle.VehiclePurpose = vehicleAssessment.MovementType.VehiclePurpose;
                    assessedVehicle.VehicleId = item.VehicleId;
                    assessedVehicle.VehicleName = item.VehicleName;
                    vehicleAssessment.AssessedVehicles.Add(assessedVehicle);

                    movementTypeList.Add(vehicleAssessment.MovementType);

                    if (vehicleAssessment.MovementType.MovementType == (int)MovementType.no_movement)
                    {
                        vehicleAssessment.VehicleError = vehicleAssessment.MovementType.VehicleClass == (int)VehicleClassificationType.NoVehicleClassification ? 7 : 6;
                        break;
                    }
                    if (vehicleAssessment.MovementType.MovementType == (int)MovementType.vr_1 && vehicleAssessment.MovementType.VehicleClass == (int)VehicleClassificationType.NoVehicleClassification)
                    {
                        vehicleAssessment.VehicleError = 10;
                        TempData["SortVr1"] = true;
                        break;
                    }
                    vehicleCurrentPosition++;
                    k++;
                }
                #endregion

                #region More than 1 vehicle
                int i = 0;
                if (movementTypeList.Count > 1)
                {
                    foreach (var item in movementTypeList)
                    {
                        int currentMovemntType = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleXmlMovementTypeMapping((VehicleXmlMovementType)(int)item.VehiclePurpose);
                        int previousMovemntType = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleXmlMovementTypeMapping((VehicleXmlMovementType)(int)vehicleAssessment.MovementType.VehiclePurpose);

                        if (i==0 || currentMovemntType == previousMovemntType)
                        {
                            if (i == 0)
                                vehicleAssessment.MovementType = item;
                            else
                            {
                                if (vehicleAssessment.MovementType.SOANoticePeriod < item.SOANoticePeriod)
                                {
                                    vehicleAssessment.MovementType.SOANoticePeriod = item.SOANoticePeriod;
                                    vehicleAssessment.MovementType.Message = item.Message;
                                }

                                if (vehicleAssessment.MovementType.PoliceNoticePeriod < item.PoliceNoticePeriod)
                                {
                                    vehicleAssessment.MovementType.PoliceNoticePeriod = item.PoliceNoticePeriod;
                                    vehicleAssessment.MovementType.Message = item.Message;
                                }
                            }
                        }

                        #region Clone/Renotify/Revise/Notify SO/VR1
                        if ((movementPayLoad.IsNotifyApp || movementPayLoad.IsNotifClone || movementPayLoad.IsRenotify || movementPayLoad.IsAppClone ||
                        movementPayLoad.IsRevise) && (item.MovementType != movementPayLoad.PrevMovType || item.VehicleClass != movementPayLoad.VehicleClass) &&
                        (movementPayLoad.PrevMovType != (int)MovementType.notification))
                        {
                            vehicleAssessment.VehicleError = 1;
                            break;
                        }
                        #endregion

                        #region Normal flow / Clone/Renotfy 
                        else if (i > 0)
                        {
                            if (vehicleAssessment.MovementType.VehicleClass != (int)VehicleEnums.VehicleClassificationType.VehicleSpecialOrder)
                            {
                                if (item.VehicleClass != vehicleAssessment.MovementType.VehicleClass || item.MovementType != vehicleAssessment.MovementType.MovementType)
                                {
                                    vehicleAssessment.VehicleError = 1;
                                    break;
                                }
                            }
                        }
                        #endregion

                        else
                        {
                            if(movementPayLoad.IsNotifClone || movementPayLoad.IsRenotify || movementPayLoad.IsAppClone || movementPayLoad.IsRevise)
                            {
                                isConfigValid=CheckVehicleConfigurationType(vehicleAssessment);
                            }
                            if (isConfigValid && CheckVehicleValidationNeeded(movementPayLoad))
                                VehicleValidation(userSchema, vehicleAssessment, item, autoFillModels);
                            else if(!isConfigValid)
                                vehicleAssessment.VehicleError = 3;
                        }
                        i++;
                    }
                }
                #endregion

                #region Only One Vehicle
                else if (movementPayLoad.MovementKey > 0)
                {
                    if ((movementPayLoad.IsNotifyApp || movementPayLoad.IsNotifClone || movementPayLoad.IsRenotify ||
                        movementPayLoad.IsAppClone || movementPayLoad.IsRevise) && (movementPayLoad.PrevMovType != (int)MovementType.notification) &&
                        (movementTypeList[i].MovementType != movementPayLoad.PrevMovType || movementTypeList[i].VehicleClass != movementPayLoad.VehicleClass))
                    {
                        vehicleAssessment.VehicleError = 1;
                    }
                    else if (vehicleAssessment.MovementType.MovementType == (int)MovementType.no_movement)
                    {
                        vehicleAssessment.VehicleError = 1;
                    }
                    else
                    {
                        if (movementPayLoad.IsNotifClone || movementPayLoad.IsRenotify || movementPayLoad.IsAppClone || movementPayLoad.IsRevise)
                        {
                            isConfigValid = CheckVehicleConfigurationType(vehicleAssessment);
                        }
                        if (isConfigValid && CheckVehicleValidationNeeded(movementPayLoad))
                            VehicleValidation(userSchema, vehicleAssessment, movementTypeList[i], autoFillModels);
                        else if (!isConfigValid)
                            vehicleAssessment.VehicleError = 3;
                    }
                }
                else
                {
                    if (movementPayLoad.IsNotifClone || movementPayLoad.IsRenotify || movementPayLoad.IsAppClone || movementPayLoad.IsRevise)
                    {
                        isConfigValid = CheckVehicleConfigurationType(vehicleAssessment);
                    }
                    if (isConfigValid && CheckVehicleValidationNeeded(movementPayLoad))
                        VehicleValidation(userSchema, vehicleAssessment, movementTypeList[i], autoFillModels);
                    else if (!isConfigValid)
                        vehicleAssessment.VehicleError = 3;
                }

                #endregion

                vehicleAssessment.VehicleList = vehicleconfigService.SelectMovementVehicle(movementId, userSchema);
            }
            return vehicleAssessment;
        }
        private PerformVehicelAssessment PerformNotifyAppVehicleAssessment(long movementId, string userSchema, PlanMvmntPayLoad movementPayLoad)
        {
            PerformVehicelAssessment vehicleAssessment;
            vehicleAssessment = new PerformVehicelAssessment
            {
                MovementType = new VehicleMovementType
                {
                    MovementType = movementPayLoad.MovementType,
                    SOANoticePeriod = 5,
                    PoliceNoticePeriod = 5,
                    VehicleClass = movementPayLoad.VehicleClass,
                    Message = "Special Order."
                },
                VehicleList = vehicleconfigService.SelectMovementVehicle(movementId, userSchema),
                VehicleError = 0,
                AssessedVehicles = new List<AssessedVehicleList>()
            };

            var msg = "";
            var vehicleError = 0;
            foreach (var item in vehicleAssessment.VehicleList)
            {
                AssessedVehicleList assessedVehicle = new AssessedVehicleList
                {
                    MovementType = vehicleAssessment.MovementType.MovementType,
                    VehicleClass = vehicleAssessment.MovementType.VehicleClass,
                    VehicleId = item.VehicleId,
                    VehicleName = item.VehicleName
                };
                vehicleAssessment.AssessedVehicles.Add(assessedVehicle);

                var VehicleRegList = vehicleconfigService.GetMovementVehicleRegDetails(item.VehicleId, userSchema);

                if (VehicleRegList.Count == 0 && movementId != 270110 && movementId != 270111 && movementId != 270101
                    && movementId != 270112 && movementId != 270156 && movementId != 270006)
                {
                    var errorMsg = $@"<div style='text-align:left;'>
                    <h4 style='font-size: 18px;text-decoration: underline;font-weight: 900;'>{item.VehicleName}</h4>
                    <ul><li> Registation details are missing : Configuration  </li></ul></div>";
                    msg = msg + errorMsg;
                    vehicleError = 3;
                }
            }
            if (!string.IsNullOrWhiteSpace(msg)) 
            {
               ViewBag.vehicleValidationError = msg;
               vehicleAssessment.VehicleError = vehicleError;
            }
            return vehicleAssessment;
        }
        private PerformVehicelAssessment PerformVSOVehicleAssessment(long movementId, string userSchema, PlanMvmntPayLoad movementPayLoad)
        {
            PerformVehicelAssessment vehicleAssessment;
            vehicleAssessment = new PerformVehicelAssessment
            {
                VehicleList = vehicleconfigService.SelectMovementVehicle(movementId, userSchema),
                VehicleError = 0,
                AssessedVehicles = new List<AssessedVehicleList>()
            };

            List<AutoFillModel> autoFillModels = AutofillVehicleDetails(userSchema, vehicleAssessment);
            List<VehicleMovementType> movementTypeList = new List<VehicleMovementType>();
            if (vehicleAssessment.VehicleList != null)
            { 
                foreach (var item in vehicleAssessment.VehicleList)
                {
                    int PoliceNoticePeriod = 0;
                    int SOANoticePeriod = 0;
                    long NotiVSO = Session["notivso"] != null ? (long)Session["notivso"] : 0;
                    if (NotiVSO == (int)VSOType.soa)
                    {
                        PoliceNoticePeriod = 0;
                        SOANoticePeriod = 2;
                    }
                    else if (NotiVSO == (int)VSOType.police)
                    {
                        PoliceNoticePeriod = 2;
                        SOANoticePeriod = 0;
                    }
                    else if (NotiVSO == (int)VSOType.soapolice)
                    {
                        PoliceNoticePeriod = 2;
                        SOANoticePeriod = 2;
                    }

                    vehicleAssessment.MovementType = new VehicleMovementType
                    {
                        MovementType = (int)MovementType.notification,
                        SOANoticePeriod = SOANoticePeriod,
                        PoliceNoticePeriod = PoliceNoticePeriod,
                        VehicleClass = (int)VehicleClassificationType.VehicleSpecialOrder,
                        VehiclePurpose = (int)VehiclePurpose.VehicleSpecialOrder,
                        Message = "Vehicle Special Order."
                    };
                    new SessionData().E4_AN_MovemenTypeClass = vehicleAssessment.MovementType;

                    AssessedVehicleList assessedVehicle = new AssessedVehicleList
                    {
                        MovementType = vehicleAssessment.MovementType.MovementType,
                        VehicleClass = vehicleAssessment.MovementType.VehicleClass,
                        VehicleId = item.VehicleId,
                        VehicleName = item.VehicleName
                    };
                    vehicleAssessment.AssessedVehicles.Add(assessedVehicle);
                }
            }
            int i = 0;
            if (movementTypeList.Count > 1)
            {
                if (CheckVehicleValidationNeeded(movementPayLoad))
                {
                    foreach (var item in movementTypeList)
                    {
                        VehicleValidation(userSchema, vehicleAssessment, item, autoFillModels);
                        i++;
                    }
                }
            }
            else
            {
                if (CheckVehicleValidationNeeded(movementPayLoad))
                    VehicleValidation(userSchema, vehicleAssessment, vehicleAssessment.MovementType, autoFillModels);
            }
            return vehicleAssessment;
        }

        private static bool CheckVehicleValidationNeeded(PlanMvmntPayLoad movementPayLoad)
        {
            var vehicleValidationNeeded = true;
            if ((movementPayLoad.IsNotifyApp || movementPayLoad.IsRenotify) && movementPayLoad.IsAgreedNotified)
                vehicleValidationNeeded = false;
            return vehicleValidationNeeded;
        }
        #endregion

        #region MovementSelectVehicleByImport
        public ActionResult MovementSelectVehicleByImport(bool isApplicationVehicle = false, string importFrm = "", bool backToPreviousList = false)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            ViewBag.IsSort = "False";
            Session["IsSortCreatedApplication"] = false;
            if (SessionInfo.UserTypeId == UserType.Sort)
            {
                ViewBag.IsSort = "True";
                Session["IsSortCreatedApplication"] = true;
            }

            ViewBag.isApplicationVehicle = isApplicationVehicle;
            ViewBag.importFrm = importFrm;
            ViewBag.backToPreviousList = backToPreviousList;

            Session["g_VehicleConfigSearch_Import"] = null;
            Session["g_VehicleConfigTypeSearch_Import"] = null;
            Session["g_VehicleConfigIntendSearch_Import"] = null;

            return PartialView("_MovementSelectVehicleByImport");
        }
        #endregion

        #region Additional
        public ActionResult SelectVehicleConfiguration()
        {
            SearchVehicleCombination configDimensions = new SearchVehicleCombination();
            return PartialView("_MovementSelectVehicleFrmSimilarCombinations", configDimensions);
        }
        public ActionResult GetFilteredCombinations(SearchVehicleCombination configDimensions)
        {
            List<VehicleConfigurationGridList> configList;
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            if (SessionInfo.UserTypeId == UserType.Sort)
            {
                configDimensions.OrganisationId = (int)Session["SORTOrgID"];
            }
            else
            {
                configDimensions.OrganisationId = (int)SessionInfo.OrganisationId;
                VehicleMovementType VSO = new SessionData().E4_AN_MovemenTypeClass;
                if (VSO != null && VSO.VehicleClass == (int)VehicleClassificationType.VehicleSpecialOrder)
                {
                    configDimensions.VehicleType = (int)VehicleClassificationType.VehicleSpecialOrder;
                }
            }
            configList = vehicleconfigService.GetSimilarVehicleCombinations(configDimensions);
            return PartialView("_SimilarVehicleCombinations", configList);
        }
        public ActionResult MovementWorkflowProgress()
        {
            return PartialView("_MovementWorkflowProgress");
        }
        public ActionResult GetSelectedVehicleDetails(MovementVehicleConfig vehicleDetails, List<VehicleList> favoriteVehicles, bool isAgreedNotif = false, int vehicleCount = 0)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            if (SessionInfo.UserTypeId == 696008 && vehicleDetails.VehiclePurpose != 270006)
            {
                vehicleDetails.HighlightVehicle = true;
            }
            ViewBag.SelectedVehicle = vehicleDetails;
            ViewBag.FavoriteVehicles = favoriteVehicles;
            ViewBag.IsAgreedNotif = isAgreedNotif;
            ViewBag.VehicleCount = vehicleCount;
            return PartialView("_VehicleDetails");
        }
        public ActionResult MovementRouteVehicle(bool isVehicleImport = false, string selectedMovementType = "", bool isNotifVehicle = false)
        {
            ViewBag.IsVehicleImport = isVehicleImport;
            ViewBag.SelectedMovementType = selectedMovementType;
            ViewBag.IsNotifVehicle = isNotifVehicle;
            List<MovementVehicleList> prevMovementVehicleLists = (List<MovementVehicleList>)TempData["PreviousMovementVehicle"];
            return PartialView("_MovementRouteVehicle", prevMovementVehicleLists);
        }
        private void ShowMovementTypeRadioButton(UserInfo SessionInfo, VehicleMovementType movementTypeClass, PlanMvmntPayLoad mvmntPayLoad)
        {
            if (SessionInfo.UserTypeId == UserType.Haulier)
            {
                if (mvmntPayLoad.MovementKey > 0)
                {
                    if (!mvmntPayLoad.IsAppClone && !mvmntPayLoad.IsRevise && !mvmntPayLoad.IsRenotify && !mvmntPayLoad.IsNotifClone &&
                        !mvmntPayLoad.IsNotifyApp && mvmntPayLoad.CurrentMovementType != (int)MovementType.notification)
                        ViewBag.ShowRadioButton = true;
                }
                else if (movementTypeClass.MovementType != (int)MovementType.notification)
                {
                    ViewBag.ShowRadioButton = true;
                }
            }
        }
        #endregion

        #region Movement Confirmation WFANStep3
        public ActionResult GetMovementTypeConfirmation(bool isBackCall = false)
        {
            TempData.Keep("MovementTypeClass");
            TempData.Keep("MovementSelectedVehicles");
            ViewBag.IsBackCall = isBackCall;
            Session["AmendVehicle"] = null;

            var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
            PlanMvmntPayLoad planMvmntPayLoad = applicationNotificationManagement.GetPlanMvmtPayload();
            ViewBag.ImminentMessage = planMvmntPayLoad != null && !string.IsNullOrEmpty(planMvmntPayLoad.ImminentMessage) ?
                planMvmntPayLoad.ImminentMessage : "";

            return PartialView("_MovementTypeConfirmation");
        }
        public ActionResult GetAssessmentDetails(bool isBackCall = false, int task = 0)
        {
            UserInfo SessionInfo = (UserInfo)Session[SessionData.Ev_UserInfo];
            string workFlowKey = new SessionData().Wf_An_ApplicationWorkflowId;
            long appRevisionId = 0;
            long notificationId = 0;
            VehicleMovementType movementTypeClass;
            List<MovementVehicleConfig> vehicleDetails;
            PlanMvmntPayLoad mvmntPayLoad = new PlanMvmntPayLoad();
            var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);

            if (workFlowKey != string.Empty || workFlowKey != WorkflowActivityConstants.Gn_Failed)
            {
                mvmntPayLoad = applicationNotificationManagement.GetPlanMvmtPayload();
                ViewBag.PayLoadMovementType = mvmntPayLoad.MovementType;//For new application, this will be zero
                ViewBag.PayLoadVehicleClass = mvmntPayLoad.VehicleClass;
                ViewBag.PrevMovementType = mvmntPayLoad.PrevMovType;
            }
            if (mvmntPayLoad == null || mvmntPayLoad.MovementKey == 0)
            {
                movementTypeClass = (VehicleMovementType)TempData["MovementTypeClass"];
                vehicleDetails = (List<MovementVehicleConfig>)TempData["MovementSelectedVehicles"];
            }
            else
            {
                appRevisionId = mvmntPayLoad.RevisionId;
                notificationId = mvmntPayLoad.NotificationId;
                movementTypeClass = mvmntPayLoad.MvmntType;
                vehicleDetails = vehicleconfigService.SelectMovementVehicle(mvmntPayLoad.VehicleMoveId, SessionInfo.UserSchema);
            }
            int boatMastCount = 0;
            vehicleDetails.ForEach(item =>
            {
                if (item.VehicleType == (int)ConfigurationType.BoatMast)
                    boatMastCount++;
            });

            TempData["MovementTypeClass"] = movementTypeClass;
            TempData["MovementSelectedVehicles"] = vehicleDetails;

            ViewBag.count = vehicleDetails.Count;
            var isSoVr1 = mvmntPayLoad.IsNotifyApp || mvmntPayLoad.IsAppClone || mvmntPayLoad.IsRevise || ((mvmntPayLoad.IsNotifClone || mvmntPayLoad.IsRenotify) &&
                                                                                mvmntPayLoad.PrevMovType != (int)MovementType.notification);
            VehicleClassificationType vehicleType = isSoVr1 ? (VehicleClassificationType)mvmntPayLoad.VehicleClass : (VehicleClassificationType)movementTypeClass.VehicleClass;
            MovementType movementType = isSoVr1 ? (MovementType)mvmntPayLoad.PrevMovType : (MovementType)movementTypeClass.MovementType;
            if (mvmntPayLoad.CurrentMovementType != 0)
                movementType = (MovementType)mvmntPayLoad.CurrentMovementType;
            string movementTypeDesc = movementType.GetEnumDescription();
            string vehicleClassType = vehicleType.GetEnumDescription();
            ViewBag.IsNotify = mvmntPayLoad.IsNotifyApp;

            int MovementXmlTypeId = Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleXmlMovementTypeMapping((VehicleXmlMovementType)vehicleDetails[0].VehiclePurpose);

            VehiclePurpose vehiclePurpose = (VehiclePurpose)MovementXmlTypeId;
            string movementClassificationName = vehiclePurpose.GetEnumDescription();
            int movementClassificationId = vehicleDetails[0].VehiclePurpose;
            if (mvmntPayLoad.PrevMovType != 0 && mvmntPayLoad.PrevMovType != (int)MovementType.notification
                && (mvmntPayLoad.IsRenotify || mvmntPayLoad.IsNotifClone || mvmntPayLoad.IsNotifyApp))
            {
                MovementType type = (MovementType)mvmntPayLoad.PrevMovType;
                string desc = type.GetEnumDescription();
                if (mvmntPayLoad.PrevMovType == (int)MovementType.vr_1)
                {
                    desc = desc + "(" + vehicleClassType.ToUpper() + ")";
                }
                ViewBag.NotifyMessage = "You are currently notifying a " + desc;
            }
            else
            {

                if (boatMastCount == vehicleDetails.Count && (int)movementType == (int)MovementType.special_order)
                    ViewBag.AssessedMovementType = "Boat Mast Exception – Special Order Required";
                else if ((int)movementType == (int)MovementType.special_order)
                    ViewBag.AssessedMovementType = movementTypeDesc;
                else if (movementTypeClass.MovementType == (int)MovementType.no_movement)
                    ViewBag.AssessedMovementType = "";
                else if (vehicleClassType != "No Vehicle Classification")
                    ViewBag.AssessedMovementType = movementTypeDesc + " (" + vehicleClassType + ")";
                else
                    ViewBag.AssessedMovementType = movementTypeDesc;
            }

            ViewBag.AssessedMovementMessage = movementTypeClass.Message;
            ViewBag.MovementType = movementTypeClass.MovementType;
            ViewBag.MoveClassName = movementClassificationName;
            ViewBag.MoveClassId = movementClassificationId;
            ViewBag.VehicleClass = movementTypeClass.VehicleClass;
            ViewBag.PoliceNotice = movementTypeClass.PoliceNoticePeriod;
            ViewBag.soa = movementTypeClass.SOANoticePeriod;
            string vehicleClassDesc = string.Empty;
            int vehicleError = 0;
            if (task == 2 && !isBackCall)
            {
                var vehicleClassification = TempData["VehicleClassification"];
                var vehicleMoveType = TempData["VehicleMovementType"];
                VehicleClassificationType vehicleClassificationType = (VehicleClassificationType)vehicleClassification;
                MovementType movement = (MovementType)vehicleMoveType;
                var classDesc = vehicleClassificationType.GetEnumDescription().ToUpper();
                var moveDesc = movement.GetEnumDescription().ToUpper();
                vehicleClassDesc = (int)movement == (int)MovementType.vr_1 ? moveDesc + "(" + classDesc + ")" : classDesc;
                vehicleError = (int)TempData["VehicleError"];
            }
            ViewBag.VehicleClassDesc = vehicleClassDesc;
            ViewBag.VehicleError = vehicleError;
            ViewBag.sortVr1 = TempData["SortVr1"];
            PlanMovementType planMovementType = new PlanMovementType();

            if (task == 3)
            {
                if (appRevisionId > 0 || notificationId > 0)
                    planMovementType = appRevisionId > 0 ? applicationService.GetApplicationDetails(appRevisionId, SessionInfo.UserSchema)
                        : applicationService.GetNotificationDetails(notificationId, SessionInfo.UserSchema);

                ShowMovementTypeRadioButton(SessionInfo, movementTypeClass, mvmntPayLoad);

                if (!mvmntPayLoad.IsRenotify && mvmntPayLoad.NoticePeriodFlag != 1 && !mvmntPayLoad.IsRevise &&
                    movementTypeClass.VehicleClass != (int)VehicleClassificationType.VehicleSpecialOrder && SessionInfo.UserTypeId == UserType.Haulier)
                {
                    int noticePeriod = movementTypeClass.SOANoticePeriod > movementTypeClass.PoliceNoticePeriod ? movementTypeClass.SOANoticePeriod : movementTypeClass.PoliceNoticePeriod;
                    DateTime dateTime = movementsService.CalcualteMovementDate(noticePeriod, movementTypeClass.VehicleClass, UserSchema.Portal);
                    planMovementType.MovementStart = dateTime;
                    planMovementType.MovementEnd = dateTime;
                }
            }

            if (applicationNotificationManagement.IsThisMovementExist(mvmntPayLoad.NotificationId, mvmntPayLoad.RevisionId, out string workflowKey)
                && WorkflowTaskFinder.FindNextTask("HaulierApplication", task == 2 ? WorkflowActivityTypes.An_Activity_VehicleAddUpdate : WorkflowActivityTypes.An_Activity_ConfirmMovementType, out dynamic workflowPayload) != string.Empty)
            {
                dynamic dataPayload = new ExpandoObject();
                dataPayload.taskId = task;
                mvmntPayLoad.ActionCompleted = mvmntPayLoad.ActionCompleted <= dataPayload.taskId ? dataPayload.taskId : mvmntPayLoad.ActionCompleted;
                dataPayload.PlanMvmntPayLoad = mvmntPayLoad;
                WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                {
                    data = dataPayload,
                    workflowData = workflowPayload
                };
                applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);
            }
            var infos = userService.GetUserByID(SessionInfo.UserTypeId.ToString(), Convert.ToInt32(SessionInfo.UserId), Convert.ToInt32(SessionInfo.ContactId)).FirstOrDefault();
            if (infos != null)
            {
                ViewBag.SORTCreateJob = infos.SORTCreateJob;
                ViewBag.SORTAllocateJob = infos.SORTAllocateJob;
            }
            return PartialView("_MovementAssessmentDetails", planMovementType);
        }
        #endregion

        #region Insert Movement Type
        public JsonResult InsertMovementType(InsertMovementTypeCntrlModel insertMovementTypeCntrlModel)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            int orgId = (int)SessionInfo.OrganisationId;
            long contactId = Convert.ToInt64(SessionInfo.ContactId);
            if (SessionInfo.UserTypeId == UserType.Sort)
            {
                orgId = Convert.ToInt32(Session["SortOrgId"]);
                contactId = Convert.ToInt64(Session["SORTContactId"]);
            }
            string moveStart = insertMovementTypeCntrlModel.fromDate + " " + insertMovementTypeCntrlModel.startTime;
            string moveEnd = insertMovementTypeCntrlModel.toDate + " " + insertMovementTypeCntrlModel.endTime;
            bool requireVr1 = false;
            bool requireSo = false;

            VehicleMovementType movementTypeClass = (VehicleMovementType)TempData["MovementTypeClass"];
            TempData.Keep("MovementTypeClass");

            if (insertMovementTypeCntrlModel.applicationType == (int)MovementType.notification)
            {
                requireSo = movementTypeClass.VehicleClass == (int)ExternalApiGeneralClassificationType.GC002;
                requireVr1 = movementTypeClass.Message.ToLower().Contains("vr-1");
            }
            var currentMoveType = movementTypeClass.MovementType;
            movementTypeClass.MovementType = insertMovementTypeCntrlModel.applicationType;
            PlanMovementType saveMovement = new PlanMovementType()
            {
                MovementId = insertMovementTypeCntrlModel.movementId,
                OrganisationId = orgId,
                ContactId = contactId,
                VehicleClass = movementTypeClass.VehicleClass,
                MoveType = movementTypeClass.MovementType,
                UserSchema = SessionInfo.UserSchema,
                MovementStart = DateTime.ParseExact(moveStart, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                MovementEnd = DateTime.ParseExact(moveEnd, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                FromDesc = insertMovementTypeCntrlModel.fromSummary,
                ToDesc = insertMovementTypeCntrlModel.toSummary,
                HaulierRef = insertMovementTypeCntrlModel.haulierRefNo,
                AllocateUserId = insertMovementTypeCntrlModel.allocateUserId,
                ApplicationType = insertMovementTypeCntrlModel.applicationType
            };
            Session["MovemntGeneralDetail"] = saveMovement;
            AppGeneralDetails appGeneral = new AppGeneralDetails();
            NotifGeneralDetails notifGeneral = new NotifGeneralDetails();
            string previousContactName = string.Empty;
            if (movementTypeClass.MovementType == (int)MovementType.special_order || movementTypeClass.MovementType == (int)MovementType.vr_1)
            {
                appGeneral = applicationService.InsertApplicationType(saveMovement);
                if (appGeneral.IsVr1)
                {
                    Session["pageflag"] = "1";
                    Session["RouteFlag"] = "1";
                    Session["AppFlag"] = "VR1App";
                }
                else
                {
                    Session["pageflag"] = "2";
                    Session["RouteFlag"] = "2";
                    Session["AppFlag"] = "SOApp";
                }
                Session["IsRoute"] = true;
            }
            else
            {
                previousContactName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                notifGeneral = notificationService.InsertNotificationType(saveMovement);
                Session["pageflag"] = "3";
                Session["RouteFlag"] = "3";
                Session["AppFlag"] = "Notif";
                Session["IsNotif"] = true;

            }
            var activityName = WorkflowTaskFinder.GenerateWorkflowActivityName(WorkflowActivityTypes.An_Activity_PlanRouteOnMap);
            //HAULIER APPLICATION NOTIFICATION WORKFLOW STARTS HERE FOR APPLICATIONS
            if (insertMovementTypeCntrlModel.startApplicationProcess)
            {
                var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                PlanMvmntPayLoad planMvmntPayLoad = new PlanMvmntPayLoad
                {
                    NextActivity = activityName,
                    OrgId = orgId,
                    OrgName = SessionInfo.OrganisationName,
                    MovementKey = appGeneral.RevisionId > 0 ? appGeneral.RevisionId : notifGeneral.NotificationId,
                    NotificationId = notifGeneral.NotificationId > 0 ? notifGeneral.NotificationId : 0,
                    RevisionId = appGeneral.RevisionId > 0 ? appGeneral.RevisionId : 0,
                    AnalysisId = notifGeneral.AnalysisId > 0 ? notifGeneral.AnalysisId : 0,
                    IsVr1App = appGeneral.IsVr1,
                    IsApp = appGeneral.RevisionId > 0,
                    IsNotif = notifGeneral.NotificationId > 0,
                    IsSortApp = SessionInfo.UserTypeId == UserType.Sort,
                    MvmntType = movementTypeClass,
                    ContenRefNo = notifGeneral.ContentRefNum,
                    VehicleMoveId = insertMovementTypeCntrlModel.movementId,
                    VersionId = appGeneral.RevisionId > 0 ? appGeneral.VersionId : notifGeneral.VersionId,
                    RequireSO = requireSo,
                    RequireVr1 = requireVr1,
                    PrevMovType = movementTypeClass.MovementType,
                    VehicleClass = movementTypeClass.VehicleClass,
                    IsVSO = true,
                    VehicleAssignmentList = new List<VehicleAssignment>(),
                    NoticePeriodFlag = 1,
                    CurrentMovementType = currentMoveType,
                    PreviousContactName = previousContactName,
                    ImminentMessage = insertMovementTypeCntrlModel.ImminentMessage

            };
                if (!applicationNotificationManagement.IsThisMovementExist(notifGeneral.NotificationId, appGeneral.RevisionId, out string workflowKey))
                {
                    applicationNotificationManagement.StartWorkflow(planMvmntPayLoad);
                    new SessionData().E4_AN_PlanMovement = planMvmntPayLoad;
                }
            }
            if (appGeneral.RevisionId > 0)
                return Json(new { data = appGeneral, movementType = movementTypeClass.MovementType });
            else
                return Json(new { data = notifGeneral, movementType = movementTypeClass.MovementType });
        }
        #endregion

        #region Update Movement Type
        public JsonResult UpdateMovementType(UpdateMovementTypeCntrlModel updateMovementTypeCntrlModel)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            int orgId = (int)SessionInfo.OrganisationId;
            long contactId = Convert.ToInt64(SessionInfo.ContactId);
            if (SessionInfo.UserTypeId == UserType.Sort)
            {
                orgId = Convert.ToInt32(Session["SortOrgId"]);
                contactId = Convert.ToInt64(Session["SORTContactId"]);
            }

            string moveStart = updateMovementTypeCntrlModel.fromDate + " " + updateMovementTypeCntrlModel.startTime;
            string moveEnd = updateMovementTypeCntrlModel.toDate + " " + updateMovementTypeCntrlModel.endTime;
            bool requireVr1 = false;
            bool requireSo = false;

            VehicleMovementType movementTypeClass = (VehicleMovementType)TempData["MovementTypeClass"];
            var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
            PlanMvmntPayLoad planMvmntPayLoad = applicationNotificationManagement.GetPlanMvmtPayload();

            var IsVehicleAmended = Session["IsVehicleAmended"] != null ? Convert.ToBoolean(Session["IsVehicleAmended"]) : false;
            IsVehicleAmended = !IsVehicleAmended && planMvmntPayLoad != null ? planMvmntPayLoad.IsVehicleAmended : IsVehicleAmended;

            if (updateMovementTypeCntrlModel.applicationType == (int)MovementType.notification)
            {
                requireSo = planMvmntPayLoad.MvmntType.VehicleClass == (int)ExternalApiGeneralClassificationType.GC002;
                requireVr1 = planMvmntPayLoad.MvmntType.Message.ToLower().Contains("vr-1");
            }
            var currentMovementType = movementTypeClass.MovementType;
            movementTypeClass.MovementType = updateMovementTypeCntrlModel.applicationType;

            #region=========CHECK VSO====Clone=======
            long NotiVSO = Session["notivso"] != null ? (long)Session["notivso"] : 0;
            if (NotiVSO > 0 && movementTypeClass != null)
            {
                if (NotiVSO == (int)VSOType.soa)
                {
                    movementTypeClass.PoliceNoticePeriod = 0;
                    movementTypeClass.SOANoticePeriod = 2;
                }
                else if (NotiVSO == (int)VSOType.police)
                {
                    movementTypeClass.PoliceNoticePeriod = 2;
                    movementTypeClass.SOANoticePeriod = 0;
                }
                else if (NotiVSO == (int)VSOType.soapolice)
                {
                    movementTypeClass.PoliceNoticePeriod = 2;
                    movementTypeClass.SOANoticePeriod = 2;
                }
                movementTypeClass.VehicleClass = (int)VehicleClassificationType.VehicleSpecialOrder;
                movementTypeClass.VehiclePurpose = (int)VehiclePurpose.VehicleSpecialOrder;
                movementTypeClass.Message = "Vehicle Special Order.";
            }
            #endregion

            PlanMovementType updateMovement = new PlanMovementType
            {
                MovementId = updateMovementTypeCntrlModel.movementId,
                OrganisationId = orgId,
                ContactId = contactId,
                VehicleClass = movementTypeClass.VehicleClass,
                MoveType = movementTypeClass.MovementType,
                RevisionId = updateMovementTypeCntrlModel.appRevisionId,
                NotificationId = updateMovementTypeCntrlModel.notificationId,
                UserSchema = SessionInfo.UserSchema,
                MovementStart = DateTime.ParseExact(moveStart, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                MovementEnd = DateTime.ParseExact(moveEnd, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                FromDesc = updateMovementTypeCntrlModel.fromSummary,
                ToDesc = updateMovementTypeCntrlModel.toSummary,
                HaulierRef = updateMovementTypeCntrlModel.haulierRefNo,
                AllocateUserId = updateMovementTypeCntrlModel.allocateUserId,
                ApplicationType = updateMovementTypeCntrlModel.applicationType,
                IsVehicleEdit = planMvmntPayLoad.VehicleAssignmentList != null && planMvmntPayLoad.VehicleAssignmentList.Count > 0 ? 0 : 1,
                IsVehicleAmended = IsVehicleAmended ? 1 : 0
            };

            AppGeneralDetails appGeneral = new AppGeneralDetails();
            NotifGeneralDetails notifGeneral = new NotifGeneralDetails();

            if (movementTypeClass.MovementType == (int)MovementType.special_order || movementTypeClass.MovementType == (int)MovementType.vr_1)
            {
                if (planMvmntPayLoad.PrevMovType != movementTypeClass.MovementType)
                {
                    appGeneral = applicationService.InsertApplicationType(updateMovement);
                    planMvmntPayLoad.PrevMovType = movementTypeClass.MovementType;
                    planMvmntPayLoad.CurrentMovementType = currentMovementType;
                }
                else
                {
                    appGeneral = applicationService.UpdateApplicationType(updateMovement);
                }
                if (appGeneral.IsVr1)
                {
                    Session["pageflag"] = "1";
                    Session["RouteFlag"] = "1";
                    Session["AppFlag"] = "VR1App";
                }
                else
                {
                    Session["pageflag"] = "2";
                    Session["RouteFlag"] = "2";
                    Session["AppFlag"] = "SOApp";
                }
                Session["IsRoute"] = true;
            }
            else
            {
                if (planMvmntPayLoad.PrevMovType != (int)MovementType.notification && !planMvmntPayLoad.IsNotifyApp && !planMvmntPayLoad.IsRenotify
                    && !planMvmntPayLoad.IsNotifClone)
                {
                    notifGeneral = notificationService.InsertNotificationType(updateMovement);
                    planMvmntPayLoad.PrevMovType = (int)MovementType.notification;
                    planMvmntPayLoad.CurrentMovementType = currentMovementType;
                }
                else
                {
                    notifGeneral = notificationService.UpdateNotificationType(updateMovement);
                }
                Session["pageflag"] = "3";
                Session["RouteFlag"] = "3";
                Session["AppFlag"] = "Notif";
                Session["IsNotif"] = true;
            }
            var activityName = WorkflowTaskFinder.GenerateWorkflowActivityName(WorkflowActivityTypes.An_Activity_PlanRouteOnMap);
            //HAULIER APPLICATION NOTIFICATION WORKFLOW STARTS HERE FOR APPLICATIONS
            if (updateMovementTypeCntrlModel.startApplicationProcess)
            {
                planMvmntPayLoad.NextActivity = activityName;
                planMvmntPayLoad.MovementKey = appGeneral.RevisionId > 0 ? appGeneral.RevisionId : notifGeneral.NotificationId;
                planMvmntPayLoad.IsVr1App = appGeneral.IsVr1;
                planMvmntPayLoad.IsApp = appGeneral.RevisionId > 0;
                planMvmntPayLoad.IsSortApp = SessionInfo.UserTypeId == UserType.Sort;
                planMvmntPayLoad.MvmntType = movementTypeClass;
                planMvmntPayLoad.ContenRefNo = notifGeneral.ContentRefNum;
                planMvmntPayLoad.VehicleMoveId = updateMovementTypeCntrlModel.movementId;
                planMvmntPayLoad.VersionId = appGeneral.RevisionId > 0 ? appGeneral.VersionId : notifGeneral.VersionId;
                planMvmntPayLoad.RevisionId = appGeneral.RevisionId;
                planMvmntPayLoad.NotificationId = notifGeneral.NotificationId;
                planMvmntPayLoad.RequireVr1 = requireVr1;
                planMvmntPayLoad.RequireSO = requireSo;
                planMvmntPayLoad.AnalysisId = notifGeneral.NotificationId > 0 ? notifGeneral.AnalysisId : 0;
                planMvmntPayLoad.IsNotif = notifGeneral.NotificationId > 0;
                planMvmntPayLoad.IsVehicleAmended = IsVehicleAmended;
                planMvmntPayLoad.NoticePeriodFlag = 1;
                planMvmntPayLoad.ImminentMessage = updateMovementTypeCntrlModel.ImminentMessage;
                if (applicationNotificationManagement.IsThisMovementExist(notifGeneral.NotificationId, appGeneral.RevisionId, out string workflowKey)
                    && WorkflowTaskFinder.FindNextTask("HaulierApplication", WorkflowActivityTypes.An_Activity_ConfirmMovementType, out dynamic workflowPayload) != string.Empty)
                {
                    dynamic dataPayload = new ExpandoObject();
                    dataPayload.PlanMvmntPayLoad = planMvmntPayLoad;
                    WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                    {
                        data = dataPayload,
                        workflowData = workflowPayload
                    };
                    applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);
                }
            }
            if (appGeneral.RevisionId > 0)
                return Json(new { data = appGeneral, movementType = movementTypeClass.MovementType });
            else
                return Json(new { data = notifGeneral, movementType = movementTypeClass.MovementType });
        }
        #endregion

        #region Vehicle Assignment

        #region GetVehicleAssignmentList
        public ActionResult GetVehicleAssignmentList(long appRevisionId = 0, long versionId = 0, string contRefNum = "", string workflowProcess = "")
        {
            List<AppRouteList> Routelist;
            List<AppVehicleConfigList> VehicleList;
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
            PlanMvmntPayLoad PlanMovementPayload = applicationNotificationManagement.GetPlanMvmtPayload();
            VehicleMovementType movementTypeClass = PlanMovementPayload.MvmntType;
            if (movementTypeClass.MovementType == (int)MovementType.special_order)
            {
                Routelist = routesService.GetSoAppRouteList(appRevisionId, SessionInfo.UserSchema);
                VehicleList = vehicleconfigService.AppVehicleConfigList((int)appRevisionId, SessionInfo.UserSchema);
            }
            else
            {
                Routelist = routesService.NotifVR1RouteList(appRevisionId, contRefNum, versionId, SessionInfo.UserSchema);
                VehicleList = vehicleconfigService.AppVehicleConfigListVR1((int)appRevisionId, (int)versionId, contRefNum, SessionInfo.UserSchema);
            }
            List<MovementVehicleList> routeVehcileList = vehicleconfigService.GetRouteVehicleList(appRevisionId, versionId, contRefNum, SessionInfo.UserSchema);
            if (PlanMovementPayload.VehicleAssignmentList == null || PlanMovementPayload.VehicleAssignmentList.Count == 0)
            {
                foreach (var route in routeVehcileList)
                {
                    route.VehicleList.Clear();
                }
            }
            foreach (var vehicle in VehicleList)
            {
                vehicle.VehicleCompList = vehicle.VehicleCompList.OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
            }
            GetVehicleImage(VehicleList);
            ViewBag.MovementSelectedVehicles = VehicleList;
            ViewBag.MovementRouteList = Routelist;
            ViewBag.RouteVehicleList = routeVehcileList;
            ViewBag.PlanMovementPayload = PlanMovementPayload;
            return PartialView("_MovementRouteVehicleAssignment");
        }
        #endregion

        #region AssignMovementVehicle
        public JsonResult AssignMovementVehicle(List<VehicleAssignment> vehicleAssignment, long revisionId, long versionId, string contRefNum = "", string workflowProcess = "")
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            bool result = vehicleconfigService.AssignMovementVehicle(vehicleAssignment, revisionId, versionId, contRefNum, SessionInfo.UserSchema);
            var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
            PlanMvmntPayLoad planMovePayload = applicationNotificationManagement.GetPlanMvmtPayload();
            if (applicationNotificationManagement.IsThisMovementExist(planMovePayload.NotificationId, planMovePayload.RevisionId, out string workflowKey)
                    && WorkflowTaskFinder.FindNextTask("HaulierApplication", WorkflowActivityTypes.An_Activity_ConfirmMovementType, out dynamic workflowPayload) != string.Empty)
            {
                dynamic dataPayload = new ExpandoObject();
                planMovePayload.VehicleAssignmentList = vehicleAssignment;
                planMovePayload.IsRouteVehicleAssigned = true;
                dataPayload.PlanMvmntPayLoad = planMovePayload;
                WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                {
                    data = dataPayload,
                    workflowData = workflowPayload
                };
                applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);
            }
            return Json(new { data = result });
        }
        #endregion

        #region Return route Assign
        [HttpPost]
        public ActionResult CopyMovementVehicle(long movementId, int flag, long notificationId)
        {
            bool isResult = false;
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
            var planMvmt = applicationNotificationManagement.GetPlanMvmtPayload();
            int icount = movementsService.ReturnRouteAutoAssignVehicle(movementId, flag, notificationId, SessionInfo.OrganisationId);
            planMvmt.VehicleAssignmentList = GetAssignedVehicleList(planMvmt, SessionInfo.UserSchema);
            if (icount >= 0)
            {
                isResult = true;
                if (applicationNotificationManagement.IsThisMovementExist(notificationId, 0, out string workflowKey)
                && WorkflowTaskFinder.FindNextTask("HaulierApplication", WorkflowActivityTypes.An_Activity_AdditionalAffectedPartiesManualEntry, out dynamic workflowPayload) != string.Empty)
                {
                    dynamic dataPayload = new ExpandoObject();
                    dataPayload.PlanMvmntPayLoad = planMvmt;
                    WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                    {
                        data = dataPayload,
                        workflowData = workflowPayload
                    };
                    applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);
                }
                List<AppVehicleConfigList> totalVehicles = vehicleconfigService.AppVehicleConfigListVR1(0, 0, planMvmt.ContenRefNo, SessionInfo.UserSchema);
                var VehicleIdArr = totalVehicles.Select(x => x.VehicleId).ToArray();
                return Json(new { data = isResult, VehicleIds = VehicleIdArr });
            }
            return Json(new { data = isResult });
        }
        #endregion

        #region GetAssignedVehicleList
        private List<VehicleAssignment> GetAssignedVehicleList(PlanMvmntPayLoad planMvmt, string userSchema)
        {
            List<VehicleAssignment> vehicleAssignments = new List<VehicleAssignment>();
            if (planMvmt.NotificationId > 0 || planMvmt.RevisionId > 0)
            {
                long versionId = planMvmt.MvmntType.MovementType == (int)MovementType.vr_1 ? planMvmt.VersionId : 0;
                List<MovementVehicleList> routeVehcileList = vehicleconfigService.GetRouteVehicleList(planMvmt.RevisionId, versionId, planMvmt.ContenRefNo, userSchema);
                VehicleAssignment vehicleAssignment;
                if (routeVehcileList.Count > 0)
                {
                    foreach (var item in routeVehcileList)
                    {
                        vehicleAssignment = new VehicleAssignment
                        {
                            RoutePartId = item.RoutePartId,
                            VehicleIds = item.VehicleList.Select(x => x.ParentVehicleId == 0 ? x.VehicleId : x.ParentVehicleId).ToList()
                        };
                        vehicleAssignments.Add(vehicleAssignment);
                    }
                }
            }
            return vehicleAssignments;
        }
        #endregion

        #endregion

        #endregion

        #region Folder functionality

        #region GetFolders
        [HttpGet]
        public JsonResult GetFolders()
        {
            try
            {
                if (Session["UserInfo"] == null)
                    return Json("expire");

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                var folders = movementsService.GetFolders(SessionInfo.OrganisationId);

                return Json(folders, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/GetFolders, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion

        #region SaveNewFolder
        [HttpPost]
        public JsonResult SaveNewFolder(STP.Domain.MovementsAndNotifications.Folder.InsertFolderParams model)
        {
            try
            {
                if (Session["UserInfo"] == null)
                    return Json("expire");

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                model.OrganisationId = (int)SessionInfo.OrganisationId;
                var result = movementsService.InsertFolderInfo(model);

                return Json(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/SaveNewFolder, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion

        #region UpdateFolderName
        [HttpPost]
        public JsonResult UpdateFolderName(STP.Domain.MovementsAndNotifications.Folder.EditFolderParams model)
        {
            try
            {
                if (Session["UserInfo"] == null)
                    return Json("expire");

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                model.OrganisationId = (int)SessionInfo.OrganisationId;
                var result = movementsService.UpdateFolderInfo(model);

                return Json(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/UpdateFolderName, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion

        #region DeleteFolder
        [HttpPost]
        public JsonResult DeleteFolder(STP.Domain.MovementsAndNotifications.Folder.EditFolderParams model)
        {
            try
            {
                if (Session["UserInfo"] == null)
                    return Json("expire");


                var result = movementsService.DeleteFolderInfo(model);
                if (result != 0)
                {
                    UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                    var folders = movementsService.GetFolders(SessionInfo.OrganisationId);
                    return Json(new { folders = folders, result = result });
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/DeleteFolder, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion

        #region AddItemToFolder
        [HttpPost]
        public JsonResult AddItemToFolder(List<STP.Domain.MovementsAndNotifications.Folder.AddItemFolderModel> model)
        {
            try
            {
                if (Session["UserInfo"] == null)
                    return Json("expire");
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                model.ForEach(x => x.UserSchema = SessionInfo.UserSchema);
                var result = movementsService.AddItemToFolder(model);

                return Json(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/AddItemToFolder, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion

        #region RemoveItemsFromFolder
        [HttpPost]
        public JsonResult RemoveItemsFromFolder(List<STP.Domain.MovementsAndNotifications.Folder.AddItemFolderModel> model)
        {
            try
            {
                if (Session["UserInfo"] == null)
                    return Json("expire");

                var result = movementsService.RemoveItemsFromFolder(model);

                return Json(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/RemoveItemsFromFolder, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion

        #region MoveFolderToFolder
        [HttpPost]
        public JsonResult MoveFolderToFolder(STP.Domain.MovementsAndNotifications.Folder.FolderTreeModel model)
        {
            try
            {
                if (Session["UserInfo"] == null)
                    return Json("expire");

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                model.OrganisationId = (int)SessionInfo.OrganisationId;
                var result = movementsService.MoveFolderToFolder(model);

                return Json(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/MoveFolderToFolder, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion

        #region UpdateFolderIconStatus
        [HttpPost]
        public JsonResult UpdateFolderIconStatus(bool isOpened)
        {
            try
            {

                Session["IsFolderOpened"] = isOpened;
                return Json("");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/UpdateFolderIconStatus, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion

        #endregion

        #region private void VehicleDimensionDropDown()
        private void VehicleDimensionDropDown()
        {
            List<FilterDropDown> objListDropDown = new List<FilterDropDown>();
            FilterDropDown objDropDown = null;
            objDropDown = new FilterDropDown();
            objDropDown.Id = "gross_weight_max_kg";
            objDropDown.Value = "Gross weight";
            objListDropDown.Add(objDropDown);
            objDropDown = new FilterDropDown();
            objDropDown.Id = "width_max_mtr";
            objDropDown.Value = "Width";
            objListDropDown.Add(objDropDown);
            objDropDown = new FilterDropDown();
            objDropDown.Id = "max_height_max_mtr";
            objDropDown.Value = "Height";
            objListDropDown.Add(objDropDown);
            objDropDown = new FilterDropDown();
            objDropDown.Id = "red_height_max_mtr";
            objDropDown.Value = "Reducible height";
            objListDropDown.Add(objDropDown);
            objDropDown = new FilterDropDown();
            objDropDown.Id = "len_max_mtr";
            objDropDown.Value = "Length";
            objListDropDown.Add(objDropDown);
            objDropDown = new FilterDropDown();
            objDropDown.Id = "rigid_len_max_mtr";
            objDropDown.Value = "Rigid length";
            objListDropDown.Add(objDropDown);
            objDropDown = new FilterDropDown();
            objDropDown.Id = "max_axle_weight_max_kg";
            objDropDown.Value = "Max Axle weight";
            objListDropDown.Add(objDropDown);

            int vehicleDimensionCount = 1;

            MovementsAdvancedFilter objMovementsAdvancedFilter = new MovementsAdvancedFilter();
            if (Session["g_AdvancedSearchData"] != null)
            {
                objMovementsAdvancedFilter = (MovementsAdvancedFilter)Session["g_AdvancedSearchData"];
                //weightCount = objMovementsAdvancedFilter.WeightCount;
            }

            ViewBag.VehicleDimensionCount = new SelectList(objListDropDown, "Id", "Value", vehicleDimensionCount);
        }
        #endregion

        #region Vehicle Validation Private Functions
        private void VehicleValidation(string userSchema, PerformVehicelAssessment vehicleAssessment, VehicleMovementType moveType, List<AutoFillModel> autoFillModels = null)
        {
            string errorMsg = "", vehicleName = "";
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            StringBuilder msgAutoFill = new StringBuilder();
            int movmntTypeId = Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleCategoryMapping(moveType);
            var VehicleListCount = vehicleAssessment != null && vehicleAssessment.VehicleList != null ? vehicleAssessment.VehicleList.Count : 0;
            if (VehicleListCount > 0)
            {
                string lengthUnit = (SessionInfo.VehicleUnits == 692001) ? "m" : "ft/in";
                string weightUnit = (SessionInfo.VehicleUnits == 692001) ? "kg" : "t";
                foreach (var vehicleObj in vehicleAssessment.VehicleList)
                {
                    msgAutoFill = new StringBuilder();
                    if (autoFillModels != null && autoFillModels.Any())
                    {
                        List<AutoFillModel> autoFillModel = autoFillModels.Where(x => x.VehicleId == vehicleObj.VehicleId).ToList();
                        if (autoFillModel != null && autoFillModel.Any())
                        {
                            var oldRecord = autoFillModel.FirstOrDefault(x => x.RecordType == "OLD");
                            var newRecord = autoFillModel.FirstOrDefault(x => x.RecordType == "NEW");
                            if (oldRecord != null && newRecord != null)
                            {
                                if (oldRecord.GrossWeight != newRecord.GrossWeight && (vehicleObj.VehicleType!=(int)ConfigurationType.SemiTrailer&& vehicleObj.VehicleType != (int)ConfigurationType.SemiTrailer_3_8))
                                    msgAutoFill.Append($"<li>Gross Weight : {newRecord.GrossWeight} {weightUnit}</li>");
                                if (oldRecord.WheelBase != newRecord.WheelBase)
                                    msgAutoFill.Append($"<li>Wheelbase : {newRecord.WheelBase} {lengthUnit}</li>");
                                if (oldRecord.RigidLength != newRecord.RigidLength)
                                    msgAutoFill.Append($"<li>Rigid Length : {newRecord.RigidLength} {lengthUnit}</li>");
                                if (oldRecord.OverallWidth != newRecord.OverallWidth)
                                    msgAutoFill.Append($"<li>Overall Width : {newRecord.OverallWidth} {lengthUnit}</li>");
                                if (oldRecord.MaxHeight != newRecord.MaxHeight)
                                    msgAutoFill.Append($"<li>Max Height : {newRecord.MaxHeight} {lengthUnit}</li>");
                            }
                        }
                    }

                    int result = CheckVehicleValidation(movmntTypeId, (int)vehicleObj.VehicleId);
                    if (result > 0)
                    {
                        vehicleAssessment.VehicleError = result;
                    }
                    else if (userSchema == UserSchema.Sort && moveType.MovementType == (int)MovementType.notification)
                    {
                        vehicleAssessment.VehicleError = 5;
                    }

                    vehicleName = vehicleObj.VehicleName;
                    var msgAutoFillString = msgAutoFill.ToString();

                    if (result > 0 || !string.IsNullOrWhiteSpace(msgAutoFillString))
                    {
                        errorMsg += "<div style='text-align:left;'>";
                        errorMsg += "<h4 style='font-size: 18px;text-decoration: underline;font-weight: 900;'>" + vehicleName + "</h4>";
                        if (!string.IsNullOrWhiteSpace(msgAutoFillString))
                            errorMsg += "<b>The following vehicle dimension(s) were auto updated.</b><ul>" + msgAutoFillString + "</ul>";

                        if (ViewBag.vehicleValidationError != "")
                            errorMsg += "<b>Please edit the vehicle to fix the below errors. " +
                                "<button class='btn btn-primary btnEditmovementFromValidationPopup' data-vehicleid='" + vehicleObj.VehicleId + "' style='padding: 3px 14px;font-size: 11px;margin-left: 5px;'>Edit Vehicle</button>" +
                                "</b><ul>" + ViewBag.vehicleValidationError + "</ul>";

                        errorMsg += "</div>";
                    }
                }
                ViewBag.vehicleValidationError = errorMsg;
            }
        }
        private int CheckVehicleValidation(long movementId, int vehicleId)
        {
            int vehicleError = 0;
            UserInfo SessionInfo = null;
            SessionInfo = (UserInfo)Session["UserInfo"];
            string msg = "", requiedMsg = "", AxleMsg = "", configMsg = "", componentsMsg = "", componentsRegMsg = "";
            ComponentModel VehicleComponentObj = null;
            List<VehicleConfigList> vehicleConfigList = null;
            VehicleComponent vehclCompObj = new VehicleComponent();
            ImportVehicleValidations vehicleValidations = new ImportVehicleValidations();
            ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
            ConfigurationModel VehicleConfig = null;
            vehicleValidations = vehicleconfigService.CheckVehicleValidations(vehicleId, SessionInfo.UserSchema);

            if (vehicleValidations.AxleLength == 1)
                msg = "<li>Total axle distance exceeds vehicle length : Configuration </li>";

            VehicleConfig = vehicleconfigService.GetVehicleDetails(vehicleId, true, SessionInfo.UserSchema);
            if (vehicleValidations.Weight == 1&& VehicleConfig.GrossWeight!=0)
                msg += "<li>Axle weight is more than gross weight : Configuration</li>";

            MovementClassificationConfig mvClassConfig = new MovementClassificationConfig();
            VehicleConfiguration vehicleConfigObj = compConfigObj.GetVehicleConfiguration(VehicleConfig.ConfigurationTypeId);
            if (vehicleConfigObj != null)
            {
                vehicleConfigObj.UpdateConfigProperties(VehicleConfig);
                vehicleConfigObj.VehicleRegList = vehicleconfigService.GetMovementVehicleRegDetails(vehicleId, SessionInfo.UserSchema);
            }

            int compCount = 0;
            vehicleConfigList = vehicleconfigService.GetMovementVehicleConfig(vehicleId, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
            bool singleComponent = false;
            int totalCompCount = vehicleConfigList.Count;
            foreach (var item in vehicleConfigList)
            {
                compCount++;
                VehicleComponentObj = vehicleComponentService.GetComponentTemp((int)item.ComponentId, "", SessionInfo.UserSchema);
                List<Axle> axleList = vehicleComponentService.ListAxleTemp((int)item.ComponentId, true, SessionInfo.UserSchema);
                List<VehicleRegistration> listVehclRegObj = vehicleComponentService.GetRegistrationTemp((int)item.ComponentId, true, SessionInfo.UserSchema);
                if (VehicleComponentObj != null)
                {
                    int MovementXmlTypeId = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleXmlMovementTypeMapping((VehicleXmlMovementType)movementId);

                    vehclCompObj = GetVehicleComponent(VehicleComponentObj.ComponentSubType, VehicleComponentObj.ComponentType, MovementXmlTypeId);
                    vehclCompObj.UpdateVehicleProperties(VehicleComponentObj);
                    vehclCompObj.ClassificationId = (int)movementId;
                    if (vehicleConfigList.Count == 1)
                    {
                        singleComponent = true;
                        vehclCompObj.ClassificationId = (int)movementId;
                        var query = from x in vehicleConfigObj.VehicleConfigParamList
                                    join y in vehclCompObj.VehicleParamList
                                        on x.ParamModel equals y.ParamModel
                                    select new { x, y };

                        foreach (var match in query)
                        {
                            if (match.y.ParamModel != "Internal Name")
                            {
                                if (match.y.ParamValue != null)
                                    match.x.ParamValue = match.y.ParamValue;
                                if (match.x.ParamValue == "0")
                                    match.x.ParamValue = match.y.ParamValue;
                                if (match.x.ParamValue != null)
                                    match.y.ParamValue = match.x.ParamValue;
                            }
                        }
                    }
                }
                if (compCount == 1)
                {
                    configMsg = CheckConfigRequiredFields(vehicleConfigObj, vehicleConfigObj.VehicleRegList, (int)movementId, vehclCompObj, singleComponent);
                }
                else
                {
                    //--- check wheelbase is required
                    if (axleList.Count > 0)
                    {
                        var wheelbase = vehclCompObj.VehicleParamList.FirstOrDefault(r => r.ParamModel == "Wheelbase");
                        if (wheelbase != null && (wheelbase.ParamValue == null || wheelbase.ParamValue == ""))
                            configMsg += "<li> Wheelbase is required : Configuration </li>";
                    }
                }
                string compMsg = "", compRegMsg = "";
                if (vehicleConfigList.Count > 1)
                {
                    compMsg = CheckComponentRequiredFields(vehclCompObj, axleList, listVehclRegObj, compCount);
                    //compRegMsg = CheckComponentRegistrationFields(vehclCompObj, listVehclRegObj, compCount);
                }
                string[] compAxleMsg = CheckComponentAxleFields(vehclCompObj, axleList, compCount, singleComponent, totalCompCount);

                //if (AxleMsg.Contains("Axle details") && compAxleMsg[0].Contains("Axle details"))
                //{
                //    compAxleMsg[0] = " and component " + compCount + "<br/>";
                //}
                if ((compMsg.Contains("Weight") || configMsg.Contains("Weight")) && vehicleValidations.Weight == 1)
                {
                    msg = msg.Replace("<li>Axle weight is more than gross weight for the vehicle</li>", " ");
                }
                //if (AxleMsg.Contains("Tyre sizes or Tyre center spacings") && compAxleMsg[1].Contains("Tyre sizes or Tyre center spacings"))
                //{
                //    compAxleMsg[1] = " and component " + compCount + "<br/>";
                //}
                if (configMsg.Contains("Registation details") && requiedMsg.Contains("Registation details"))
                {
                    compRegMsg = "";
                }
                //else if (requiedMsg.Contains("Registation details") && compRegMsg.Contains("Registation details"))
                //{
                //    compRegMsg = " and component " + compCount + "<br/>";
                //}
                else if (compRegMsg.Contains("Registation details"))
                {
                    compRegMsg += " <br/>";
                }
                componentsMsg += compMsg;
                AxleMsg += compAxleMsg[0];
                AxleMsg += compAxleMsg[1];
                componentsRegMsg += compRegMsg;
            }
            requiedMsg += configMsg;
            requiedMsg += componentsMsg;
            requiedMsg += componentsRegMsg;

            requiedMsg += AxleMsg;

            if (requiedMsg != "")
            {
                msg += requiedMsg;
            }

            if (vehicleValidations.AxleLength == 0 && vehicleValidations.Weight == 0 && requiedMsg == "")
                vehicleError = 0;
            else
                vehicleError = 3;

            ViewBag.vehicleValidationError = msg;
            return vehicleError;
        }
        private string CheckConfigRequiredFields(VehicleConfiguration vehicleConfigObj, List<VehicleRegistration> listComponentReg, int movementId, VehicleComponent vehclCompObj, bool singleComponent = false)
        {
            var msg = "";
            int count = 0;
            double RigidLength = 0;
            double OverallLength = 0;
            //--- check vehicle required fields

            foreach (IFXProperty ifxProperty in vehicleConfigObj.VehicleConfigParamList)
            {
                if (ifxProperty.IsRequired == 1 && (ifxProperty.ParamValue == null || ifxProperty.ParamValue == "0") && ifxProperty.ParamModel != "Wheelbase")
                {
                    count++;
                    msg += ifxProperty.DisplayString + ",";
                }
                else
                {
                    var compParam = vehclCompObj.VehicleParamList.FirstOrDefault(r => r.ParamModel == ifxProperty.ParamModel);
                    if (singleComponent && compParam != null && compParam.IsRequired == 1 && (compParam.ParamValue == null || compParam.ParamValue == "0" || compParam.ParamValue == "") && compParam.ParamModel != "Wheelbase")
                    {
                        count++;
                        msg += ifxProperty.DisplayString + ",";
                    }
                }
                if (ifxProperty.ParamModel == "Length")
                    RigidLength = Convert.ToDouble(ifxProperty.ParamValue);
                if (ifxProperty.ParamModel == "OverallLength")
                    OverallLength = Convert.ToDouble(ifxProperty.ParamValue);
            }
            msg = msg.TrimEnd(',');
            if (count > 1)
                msg = "<li>" + msg + " are required : Configuration </li>";
            else if (count == 1)
                msg = "<li>" + msg + " is required : Configuration </li>";

            if (!singleComponent && RigidLength != 0 && OverallLength != 0 && OverallLength < RigidLength)
                msg = msg + "<li>Rigid Length should be less than or equal to Overall Length : Configuration </li>";


            //--- check registration details for components
            if (listComponentReg.Count == 0
                && movementId != 270110 && movementId != 270111 && movementId != 270101
                    && movementId != 270112 && movementId != 270156 && movementId != 270006)
            {
                msg = msg + "<li> Registation details are missing : Configuration  </li>";
            }

            return msg;
        }

        private string CheckComponentRequiredFields(VehicleComponent vehclCompObj, List<Axle> axleList, List<VehicleRegistration> listComponentReg, int compCount = 1)
        {
            var msg = "";
            int count = 0;
            double weight = 0;
            //--- check wheelbase is required
            bool isWheelbaseRequired = false;
            if (axleList.Count > 0)
                isWheelbaseRequired = true;
            //--- check component required fields
            foreach (IFXProperty ifxProperty in vehclCompObj.VehicleParamList)
            {
                if ((ifxProperty.IsRequired == 1 && (ifxProperty.ParamValue == null || ifxProperty.ParamValue == "0"))
                    || (isWheelbaseRequired && ifxProperty.ParamModel == "Wheelbase" && (ifxProperty.ParamValue == null || ifxProperty.ParamValue == "0")))
                {
                    count++;
                    msg += ifxProperty.DisplayString + ",";
                }
                else if (ifxProperty.IsRequired == 0 && (ifxProperty.ParamValue == null || ifxProperty.ParamValue == "0")
                    && (vehclCompObj.ClassificationId == 270110 || vehclCompObj.ClassificationId == 270111
                    || vehclCompObj.ClassificationId == 270112 || vehclCompObj.ClassificationId == 270156))
                {
                    if (ComponentRequiredFieldForVR1(vehclCompObj.vehicleCompType.ComponentTypeId, ifxProperty.ParamModel))
                    {
                        count++;
                        msg += ifxProperty.DisplayString + ",";
                    }
                }
                if(ifxProperty.ParamModel == "Weight")
                {
                    weight = Convert.ToDouble(ifxProperty.ParamValue);
                }
                if(ifxProperty.IsRequired == 0 && ifxProperty.ParamModel == "Ground Clearance" && weight> 150000
                    && (ifxProperty.ParamValue == null || ifxProperty.ParamValue == "") && vehclCompObj.ClassificationId == 270006)
                {
                    count++;
                    msg += ifxProperty.DisplayString + ",";
                }
            }
            msg = msg.TrimEnd(',');
            if (count >= 2)
            {
                var lastComma = msg.LastIndexOf(',');
                if (lastComma != -1)
                    msg = msg.Remove(lastComma, 1).Insert(lastComma, " and ");
            }

            if (count > 1)
                msg = "<li>" + msg + " are required for component " + compCount + ": " + vehclCompObj.VehicleParamList[0].ParamValue + "</li>";
            else if (count == 1)
                msg = "<li>" + msg + " is required for component " + compCount + ": " + vehclCompObj.VehicleParamList[0].ParamValue + "</li>";

            return msg;
        }
        private VehicleComponent GetVehicleComponent(int vehicleSubTypeId, int vehicleTypeId, int movementId)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/GetVehicleComponent method started successfully, with parameters vehicleSubTypeId:{0}, vehicleTypeId:{1}", vehicleSubTypeId, vehicleTypeId));
                ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
                VehicleComponent vehclCompObj;
                if (movementId != 0)
                {
                    MovementClassificationConfig moveClassConfigObj = compConfigObj.GetMovementClassificationConfig(movementId);
                    vehclCompObj = moveClassConfigObj.GetVehicleComponent(vehicleTypeId, vehicleSubTypeId);
                }
                else
                {
                    MovementClassificationConfig moveClassConfigObj = compConfigObj.GetListOfVehicleComponents(vehicleTypeId);
                    vehclCompObj = moveClassConfigObj.GetVehicleComponent(vehicleTypeId, vehicleSubTypeId);
                }

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/GetVehicleComponent method completed successfully"));
                return vehclCompObj;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Vehicle/GetVehicleComponent, Exception: {0}", ex));
                throw ex;
            }
        }

        private bool ComponentRequiredFieldForVR1(int ComponentTypeId, string Field)
        {
            bool isRequired = false;
            switch (ComponentTypeId)
            {
                case (int)ComponentType.BallastTractor:
                    if (Field == "Wheelbase")
                        isRequired = true;
                    break;
                case (int)ComponentType.ConventionalTractor:
                    if (Field == "Width" || Field == "Maximum Height" || Field == "TyreCentreSpacing" || Field == "Wheelbase")
                        isRequired = true;
                    break;
                case (int)ComponentType.DrawbarTrailer:
                case (int)ComponentType.EngineeringPlantDrawbarTrailer:
                case (int)ComponentType.GirderSet:
                    if (Field == "Wheelbase")
                        isRequired = true;
                    break;
                case (int)ComponentType.EngineeringPlant:
                case (int)ComponentType.RigidVehicle:
                    if (Field == "Wheelbase")
                        isRequired = true;
                    break;
                case (int)ComponentType.SemiTrailer:
                case (int)ComponentType.EngineeringPlantSemiTrailer:
                    if (Field == "Width" || Field == "Maximum Height" || Field == "Length" || Field == "Wheelbase")
                        isRequired = true;
                    break;
                case (int)ComponentType.SPMT:
                    if (Field == "TyreCentreSpacing" || Field == "Wheelbase")
                        isRequired = true;
                    break;
                default:
                    break;
            }
            return isRequired;
        }
        private string[] CheckComponentAxleFields(VehicleComponent vehclCompObj, List<Axle> axleList, int compCount = 1, bool singleComponent = false, int totalCompCount = 0)
        {
            string[] msg;
            string axleMsg = "";
            string tyreMsg = "";

            //--- check component axle required fields
            if (axleList.Count == 0 && vehclCompObj.IsConfigAxle == true)
            {
                if (singleComponent)
                    axleMsg = "<li>" + axleMsg + " Axle details are missing : Configuration </li>";
                else
                    axleMsg = "<li>" + axleMsg + " Axle details are missing for component " + compCount + ": " + vehclCompObj.VehicleParamList[0].ParamValue + "</li>";
            }
            else
            {
                bool distanceToNextAxleEmpty = axleList.Any(item => item.DistanceToNextAxle is 0);
                if (distanceToNextAxleEmpty && compCount != totalCompCount)
                    axleMsg = "<li>" + axleMsg + " Distance to next axle is missing for component " + compCount + ": " + vehclCompObj.VehicleParamList[0].ParamValue + "</li>";
            }

            if (vehclCompObj.ClassificationId == 270101)
                vehclCompObj.IsConfigTyreCentreSpacing = false;
            else
                vehclCompObj.IsConfigTyreCentreSpacing = TyreDetailsRequired(vehclCompObj.vehicleCompType.ComponentTypeId, vehclCompObj.ClassificationId);


            bool reuired = AxleRequiedFields(vehclCompObj.vehicleCompType.ComponentTypeId, vehclCompObj.ClassificationId);
            if (reuired)
            {
                if (axleList.Count > 0)
                {
                    bool tyreSizeEmpty = false, tyreCenterEmpty = false;
                    tyreSizeEmpty = axleList.All(item => (item.TyreSize is null || item.TyreSize == ""));
                    tyreCenterEmpty = axleList.Any(item => (item.TyreCenters is null || item.TyreCenters == "" || item.TyreCenters == ",,"));

                    if (tyreSizeEmpty || tyreCenterEmpty)
                    {
                        if (singleComponent)
                            tyreMsg = "<li>" + tyreMsg + " Tyre sizes or Tyre center spacings details are missing : Configuration</li>";
                        else
                            tyreMsg = "<li>" + tyreMsg + " Tyre sizes or Tyre center spacings details are missing for component " + compCount + ": " + vehclCompObj.VehicleParamList[0].ParamValue + "</li>";
                    }
                }
            }
            msg = new string[2] { axleMsg, tyreMsg };

            return msg;
        }
        private string CheckComponentRegistrationFields(VehicleComponent vehclCompObj, List<VehicleRegistration> listComponentReg, int compCount = 1)
        {
            var msg = "";

            //--- check registration details for components
            if (listComponentReg.Count == 0
                && vehclCompObj.vehicleCompType.ComponentTypeId != (int)ComponentType.SemiTrailer
                && vehclCompObj.vehicleCompType.ComponentTypeId != (int)ComponentType.DrawbarTrailer)
            {
                msg = "<li>" + msg + " Registation details are missing for component " + compCount + ": " + vehclCompObj.VehicleParamList[0].ParamValue + "</li>";
            }

            return msg;
        }

        private bool AxleRequiedFields(int ComponentTypeId, int movementTypeId)
        {
            int MvmntType = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.MovementTypeHighLevelMapping((VehicleXmlMovementType)movementTypeId);

            bool isRequired = false;
            switch (ComponentTypeId)
            {
                case (int)ComponentType.BallastTractor:
                    if (MvmntType == (int)VehicleMovementTypeMain.SpecialOrder)
                        isRequired = true;
                    break;
                case (int)ComponentType.ConventionalTractor:
                    if (MvmntType == (int)VehicleMovementTypeMain.SpecialOrder)
                        isRequired = true;
                    break;
                case (int)ComponentType.DrawbarTrailer:
                case (int)ComponentType.EngineeringPlantDrawbarTrailer:
                case (int)ComponentType.GirderSet:
                    if (MvmntType == (int)VehicleMovementTypeMain.SpecialOrder )
                        isRequired = true;
                    break;
                case (int)ComponentType.EngineeringPlant:
                case (int)ComponentType.RigidVehicle:
                    if (MvmntType == (int)VehicleMovementTypeMain.SpecialOrder)
                        isRequired = true;
                    break;
                case (int)ComponentType.SemiTrailer:
                case (int)ComponentType.EngineeringPlantSemiTrailer:
                    if (MvmntType == (int)VehicleMovementTypeMain.SpecialOrder)
                        isRequired = true;
                    break;
                case (int)ComponentType.SPMT:
                    if (MvmntType == (int)VehicleMovementTypeMain.SpecialOrder)
                        isRequired = true;
                    break;
                case (int)ComponentType.MobileCrane:
                    if (MvmntType == (int)VehicleMovementTypeMain.SpecialOrder)
                        isRequired = true;
                    break;
                default:
                    break;
            }
            return isRequired;
        }
        private bool TyreDetailsRequired(int ComponentTypeId, int movementTypeId)
        {
            int MvmntType = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.MovementTypeHighLevelMapping((VehicleXmlMovementType)movementTypeId);

            bool isRequired = false;
            switch (ComponentTypeId)
            {
                case (int)ComponentType.BallastTractor:
                    if (MvmntType == (int)VehicleMovementTypeMain.Stgovr1
                        || MvmntType == (int)VehicleMovementTypeMain.SpecialOrder
                        || MvmntType == (int)VehicleMovementTypeMain.VehicleSpecialOrder)
                        isRequired = true;
                    break;
                case (int)ComponentType.ConventionalTractor:
                    if (MvmntType == (int)VehicleMovementTypeMain.Stgovr1
                        || MvmntType == (int)VehicleMovementTypeMain.SpecialOrder
                        || MvmntType == (int)VehicleMovementTypeMain.VehicleSpecialOrder)
                        isRequired = true;
                    break;
                case (int)ComponentType.DrawbarTrailer:
                case (int)ComponentType.EngineeringPlantDrawbarTrailer:
                case (int)ComponentType.GirderSet:
                    if (MvmntType == (int)VehicleMovementTypeMain.Stgovr1
                        || MvmntType == (int)VehicleMovementTypeMain.SpecialOrder
                        || MvmntType == (int)VehicleMovementTypeMain.VehicleSpecialOrder)
                        isRequired = true;
                    break;
                case (int)ComponentType.EngineeringPlant:
                case (int)ComponentType.RigidVehicle:
                    if (MvmntType == (int)VehicleMovementTypeMain.Stgovr1
                        || MvmntType == (int)VehicleMovementTypeMain.SpecialOrder
                        || MvmntType == (int)VehicleMovementTypeMain.VehicleSpecialOrder)
                        isRequired = true;
                    break;
                case (int)ComponentType.SemiTrailer:
                case (int)ComponentType.EngineeringPlantSemiTrailer:
                    if (MvmntType == (int)VehicleMovementTypeMain.Stgovr1
                        || MvmntType == (int)VehicleMovementTypeMain.SpecialOrder
                        || MvmntType == (int)VehicleMovementTypeMain.VehicleSpecialOrder)
                        isRequired = true;
                    break;
                case (int)ComponentType.SPMT:
                    if (MvmntType == (int)VehicleMovementTypeMain.Stgovr1
                        || MvmntType == (int)VehicleMovementTypeMain.SpecialOrder
                        || MvmntType == (int)VehicleMovementTypeMain.VehicleSpecialOrder)
                        isRequired = true;
                    break;
                case (int)ComponentType.MobileCrane:
                    if (MvmntType == (int)VehicleMovementTypeMain.SpecialOrder)
                        isRequired = true;
                    break;
                default:
                    break;
            }
            return isRequired;
        }

        private List<AutoFillModel> AutofillVehicleDetails(string userSchema, PerformVehicelAssessment vehicleAssessment)
        {
            List<AutoFillModel> autoFillModels = new List<AutoFillModel>();
            if (vehicleAssessment != null && vehicleAssessment.VehicleList != null)
            {
                List<long> vehilceList = vehicleAssessment.VehicleList.Select(x => x.VehicleId).ToList();
                string vehicleIds = string.Join(",", vehilceList);
                autoFillModels = vehicleconfigService.AutoFillVehicles(vehicleIds, vehilceList.Count, userSchema);
            }
            return autoFillModels;
        }
        
        private bool CheckVehicleConfigurationType(PerformVehicelAssessment vehicleAssessment)
        {
            bool isConfigValid = true;
            UserInfo SessionInfo = null;
            SessionInfo = (UserInfo)Session["UserInfo"];
            List<VehicleConfigList> vehicleConfigList = null;
            var VehicleListCount = vehicleAssessment != null && vehicleAssessment.VehicleList != null ? vehicleAssessment.VehicleList.Count : 0;
            if (VehicleListCount > 0)
            {
                foreach (var vehicleObj in vehicleAssessment.VehicleList)
                {
                    vehicleConfigList = vehicleconfigService.GetMovementVehicleConfig(vehicleObj.VehicleId, SessionInfo.UserSchema);
                    List<int> compIds = new List<int>();
                    foreach (var id in vehicleConfigList)
                    {
                        compIds.Add((int)id.ComponentId);
                    }

                    List<uint> vehicleConfig = vehicleconfigService.AssessConfigurationType(compIds, false, SessionInfo.UserSchema, 4);
                    if (vehicleConfig.Count > 0)
                    {
                        vehicleConfig.RemoveAll(x => (x == (uint)ConfigurationType.OtherInline) || (x == (uint)ConfigurationType.SidebySide));
                    }
                    if (vehicleConfig.Count == 0)
                    {
                        isConfigValid = false;
                        ViewBag.vehicleValidationError = "The vehicle "+ vehicleObj.VehicleName + " is not compatible with ESDAL4. Please delete the vehicle and add a new vehicle.";
                    }
                    else {
                        isConfigValid = true;
                        ViewBag.vehicleValidationError = "";
                    }
                }
            }

            return isConfigValid;
        }
        #endregion

        #region GetMovementLinkByRefNo
        public JsonResult GetMovementLinkByRefNo(string ESDALReference)
        {
            try
            {
                int organisationId;
                MovementsFilter objMovementFilter = new MovementsFilter() { };
                MovementsAdvancedFilter objMovementsAdvancedFilter = new MovementsAdvancedFilter { ESDALReference = ESDALReference,IncludeHistoricalData=true };
                int presetFilter = 0; int ShowPrevSortRoute = 0;
                bool prevMovImport = false;
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                if (SessionInfo.UserTypeId != 696008)
                {
                    ShowPrevSortRoute = 0;
                }
                organisationId = (int)SessionInfo.OrganisationId;
                List<MovementsList> objMovementsList = movementsService.GetMovementsList(organisationId, 1, int.MaxValue, objMovementFilter, objMovementsAdvancedFilter, presetFilter, SessionInfo.UserSchema, ShowPrevSortRoute, prevMovImport);
                var link = "";
                if (objMovementsList != null && objMovementsList.Any())
                {
                    var firstElem = objMovementsList.Where(x => x.ESDALReference == ESDALReference).FirstOrDefault();
                    if (firstElem != null)
                    {
                        int movementId = 0;
                        string movementType = firstElem.MovementType;
                        if (!string.IsNullOrEmpty(movementType))
                        {
                            movementType = movementType.ToLower();
                        }
                        switch (movementType)
                        {
                            case "c and u":
                                movementId = 270001;
                                break;
                            case "stgo ail":
                                movementId = 270002;
                                break;
                            case "stgo mobile crane":
                                movementId = 270003;
                                break;
                            case "vehicle special order":
                                movementId = 270007;
                                break;
                            case "stgo ail cat 1":
                                movementId = 241003;
                                break;
                            case "stgo ail cat 2":
                                movementId = 241004;
                                break;
                            case "stgo ail cat 3":
                                movementId = 241005;
                                break;
                            case "special order":
                                movementId = 241002;
                                break;
                            case "stgo mobile crane cat a":
                                movementId = 241006;
                                break;
                            case "stgo mobile crane cat b":
                                movementId = 241007;
                                break;
                            case "stgo mobile crane cat c":
                                movementId = 241008;
                                break;
                            case "stgo engineering plant":
                                movementId = 241009;
                                break;
                            case "stgo road recovery":
                                movementId = 270005;
                                break;
                            case "wheeled construction and use":
                                movementId = 241011;
                                break;
                            case "tracked":
                                movementId = 270008;
                                break;
                            case "stgo engineering plant wheeled":
                                movementId = 270004;
                                break;
                            case "stgo engineering plant tracked":
                                movementId = 241014;
                                break;
                            default:
                                movementId = 270002;
                                break;
                        }
                        if ((int)firstElem.NotificationId == 0)
                        {
                            link = Url.Action("ListSOMovements", "Application",
                                                            new
                                                            {
                                                                B7vy6imTleYsMr6Nlv7VQ =
                                                            STP.Web.Helpers.EncryptionUtility.Encrypt("revisionId=" + firstElem.MovementRevisionId +
                                                                "&movementId=" + movementId +
                                                                "&versionId=" + firstElem.MovementVersionId +
                                                                "&hauliermnemonic=" + firstElem.HaulierMnemonic +
                                                                "&esdalref=" + firstElem.ProjectESDALReference +
                                                                "&revisionno=" + firstElem.ApplicationRevisionNo +
                                                                "&versionno=" + firstElem.MovementVersionNumber +
                                                                "&apprevid=" + firstElem.ApplicationRevisionId +
                                                                "&projecid=" + firstElem.ProjectId +
                                                                "&pageflag=1" +
                                                                "&Ishistoric=" + firstElem.IsHistoric)
                                                            });
                        }
                        else
                        {
                            link = Url.Action("DisplayNotification", "Notification",
                                                            new
                                                            {
                                                                B7vy6imTleYsMr6Nlv7VQ =
                                                            STP.Web.Helpers.EncryptionUtility.Encrypt("notificationId=" + firstElem.NotificationId +
                                                            "&notificationCode=" + firstElem.NotificationCode +
                                                            "&Ishistoric=" + firstElem.IsHistoric)
                                                            });
                        }
                    }

                }
                return  Json(new { data=link,success=true},JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { data = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
