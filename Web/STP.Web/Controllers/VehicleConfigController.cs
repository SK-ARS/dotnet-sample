using Newtonsoft.Json;
using PagedList;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.Domain.LoggingAndReporting;
using STP.Domain.SecurityAndUsers;
using STP.Domain.Structures;
using STP.Domain.VehicleAndFleets.Component;
using STP.Domain.VehiclesAndFleets;
using STP.Domain.VehiclesAndFleets.Component;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.Workflow;
using STP.ServiceAccess.LoggingAndReporting;
using STP.ServiceAccess.VehiclesAndFleets;
using STP.ServiceAccess.Workflows;
using STP.ServiceAccess.Workflows.ApplicationsNotifications;
using STP.Web.Filters;
using STP.Web.WorkflowProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using static STP.Domain.VehiclesAndFleets.Configuration.VehicleModel;
using static STP.Domain.VehiclesAndFleets.VehicleEnums;
using static STP.Common.Enums.ExternalApiEnums;
namespace STP.Web.Controllers
{
    [AuthorizeUser(Roles = "50000,50002,13003,13004,13005,13006,300000")]
    public class VehicleConfigController : Controller
    {
        private readonly IVehicleConfigService vehicleconfigService;
        private readonly ILoggingService loggingService;
        private readonly IFleetManagementWorkflowService fleetManagementWorkflowService;
        private readonly IApplicationNotificationWorkflowService applicationNotificationWorkflowService;
        private readonly IVehicleComponentService vehicleComponentService;
        public VehicleConfigController()
        {
        }

        public VehicleConfigController(IVehicleConfigService vehicleconfigService, ILoggingService loggingService, IFleetManagementWorkflowService fleetManagementWorkflowService, IVehicleComponentService vehicleComponentService, IApplicationNotificationWorkflowService applicationNotificationWorkflowService)
        {
            this.vehicleconfigService = vehicleconfigService;
            this.loggingService = loggingService;
            this.fleetManagementWorkflowService = fleetManagementWorkflowService;
            this.applicationNotificationWorkflowService = applicationNotificationWorkflowService;
            this.vehicleComponentService = vehicleComponentService;
        }

        #region Old Create Vehicle functions
        public ActionResult vehicleworkflow(bool isApplicationVehicle)
        {
            return RedirectToAction("VehicleConfigList", new
            {
                isApplicationVehicle = isApplicationVehicle,
                pageSize = 5
            });
        }

        public ActionResult AddedVehicleList()
        {
            return View();
        }

        #region Public Methods
        #region public ActionResult VehicleConfigList(int? page, int? pageSize, string searchString, bool isApplicationVehicle)
        [AuthorizeUser(Roles = "50000,50002,13003,13004,13005,13006")]
        public ActionResult VehicleConfigList(int? page, int? pageSize, string searchString, string searchVhclIntend, string movclassification = "", bool isApplicationVehicle = false, int searchVhclType = 0, bool isNotifVehicle = false, int MovementClassCode = 0, int movetype1 = 0, string IsNotifMobileCrane = "", bool importFlag = false, int filterFavouritesVehConfig = 0,int? sortType=null,int? sortOrder=null)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/VehicleConfigList , Load Vehicle Configuration List", Session.SessionID));

            //sessoin checking
            UserInfo SessionInfo = null;
            Session["vehicleWorkFlowParams"] = null;
            sortOrder = sortOrder != null ? (int)sortOrder : 2; //configname
            int presetFilter = sortType != null ? (int)sortType : 0; // asc
            ViewBag.SortOrder = sortOrder;
            ViewBag.SortType = presetFilter;

            // Session["movementClassificationId"] = null;
            //Session["movementClassificationName"] = null;
            SessionInfo = (UserInfo)Session["UserInfo"];
            if (filterFavouritesVehConfig == 0)
                ViewBag.filterFavouritesVehConfig = false;
            else
                ViewBag.filterFavouritesVehConfig = true;
            if (IsNotifMobileCrane.Contains("MobileCrane"))
                Session["MovementType1"] = 0;
            int organisationId;
            if (SessionInfo.UserTypeId == UserType.Sort)
                organisationId = (int)Session["SORTOrgID"];
            else
                organisationId = (int)SessionInfo.OrganisationId;

            #region filter for application
            if (isApplicationVehicle)
            {
                searchVhclIntend = string.Empty;
            }
            #endregion filter for application

            ViewBag.isNotifVehicle = isNotifVehicle;
            ViewBag.MovementClassCode = MovementClassCode;
            var g_VehicleConfigSearch = importFlag ? "g_VehicleConfigSearch_Import" : "g_VehicleConfigSearch";
            var g_VehicleConfigTypeSearch = importFlag ? "g_VehicleConfigTypeSearch_Import" : "g_VehicleConfigTypeSearch";
            var g_VehicleConfigIntendSearch = importFlag ? "g_VehicleConfigIntendSearch_Import" : "g_VehicleConfigIntendSearch";
            if (Session[g_VehicleConfigSearch] != null)
            {
                ViewBag.ConfigNameSearch = Session[g_VehicleConfigSearch];
            }
            if (Session[g_VehicleConfigTypeSearch] != null)
            {
                ViewBag.TypeSearch = Session[g_VehicleConfigTypeSearch];
            }
            if ((Session["movementClassificationName"] != null))
            {
                if(Session[g_VehicleConfigIntendSearch] != null)
                    ViewBag.VhclIntendSearch = (string)Session["movementClassificationName"];
                AssignMovementClassification((int)Session["movementClassificationId"]);
            }
            else if (Session[g_VehicleConfigIntendSearch] != null)
            {
                ViewBag.VhclIntendSearch = Session[g_VehicleConfigIntendSearch];
            }

            if (ViewBag.VhclIntendSearch == "c and u")
            {
                ViewBag.VhclIntendSearch = "Construction and use";
            }

            #region search string

            if (!isApplicationVehicle && !isNotifVehicle)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(Session[g_VehicleConfigSearch])) && searchString == null)
                {
                    searchString = (string)Session[g_VehicleConfigSearch];
                }
                if (Session[g_VehicleConfigTypeSearch] != null && searchVhclType == 0)
                {
                    searchVhclType = (int)Session[g_VehicleConfigTypeSearch];
                }
                //if (Session["movementClassificationName"] != null)
                //{
                //    searchVhclIntend = (string)Session["movementClassificationName"];
                //}
                //else 
                if (Session[g_VehicleConfigIntendSearch] != null && searchVhclIntend == null)
                {
                    searchVhclIntend = (string)Session[g_VehicleConfigIntendSearch];
                }

            }

            switch (searchVhclIntend)
            {
                case "Construction and use":
                case "c and u":
                    searchVhclIntend = "c and u";
                    break;
                case "stgo ail":
                case "STGO AIL ( including VR-1s )":
                    searchVhclIntend = "STGO AIL";
                    break;
                case "STGO Road recovery operation":
                    searchVhclIntend = "stgo road recovery";
                    break;
                case "special order":
                    searchVhclIntend = "Special order";
                    break;
                case "stgo engineering plant wheeled":
                case "STGO Engineering plant ( not tracked )":
                case "STGO Engineering plant(not tracked)":
                    searchVhclIntend = "stgo engineering plant wheeled";
                    break;
                case "STGO Mobile crane":
                    searchVhclIntend = "stgo mobile crane";
                    break;
                case "vehicle special order":
                    searchVhclIntend = "Vehicle special order";
                    break;
                case "tracked":
                    searchVhclIntend = "Tracked";
                    break;
            }

            #endregion search string

            
            List<VehicleConfigurationGridList> VehicleConfigGridListObj = new List<VehicleConfigurationGridList>();

            int movementType = 0;
            if (MovementClassCode == 0)
            {
                switch (movclassification)
                {
                    case "C&U Semi Trailer":
                    case "C&U Rigid":
                        movementType = 270001;
                        searchString = "";
                        searchVhclIntend = "";
                        break;
                    case "STGO AIL-Semi Trailer":
                    case "STGO AIL-Rigid":
                    case "STGO AIL-Mobile Crane":
                        movementType = 270002;
                        searchString = "";
                        searchVhclIntend = "";
                        break;
                    case "241004":
                        movementType = 241004;
                        searchString = "";
                        searchVhclIntend = "";
                        break;
                    case "Special Order":
                        movementType = 270006;
                        break;
                    case "VR1":
                        movementType = 1;
                        break;
                    default:
                        movementType = 0;
                        break;
                }
            }
            else
            {
                movementType = MovementClassCode;
                ViewBag.MovementClassCode = MovementClassCode;
            }

            if (isApplicationVehicle)
                movementType = 270006;
            if (movclassification == "VR1")
                movementType = 1;


            if (isNotifVehicle && movetype1 == 0)
            {
                movetype1 = Convert.ToInt32(Session["MovementType1"]);
            }
            else
            {
                Session["MovementType1"] = movetype1;
            }


            if (Session["MovementClassId"] != null && importFlag)
            {
                movementType = (int)Session["MovementClassId"];
            }
            if(sortOrder == null)
            {
                VehicleConfigGridListObj = vehicleconfigService.GetConfigByOrganisationId(organisationId, movementType, movetype1, SessionInfo.UserSchema, filterFavouritesVehConfig, presetFilter, sortOrder).OrderBy(s => s.ConfigurationName).ToList();

            }
            else
            {
                VehicleConfigGridListObj = vehicleconfigService.GetConfigByOrganisationId(organisationId, movementType, movetype1, SessionInfo.UserSchema, filterFavouritesVehConfig, presetFilter, sortOrder).ToList();

            }

            if (isNotifVehicle)
            {
                VehicleConfigGridListObj = (from vhcl in VehicleConfigGridListObj
                                            where vhcl.VehicleType.ToString().Contains(movetype1.ToString())
                                            select vhcl).ToList();

            }

            if (!string.IsNullOrEmpty(searchString))
            {
                VehicleConfigGridListObj = (from s in VehicleConfigGridListObj
                                            where s.ConfigurationName.ToLower().Contains(searchString.ToLower())
                                            select s).ToList();

                TempData["SearchString"] = searchString;
            }

            if (SessionInfo.UserTypeId == UserType.Sort)
            {
                VehicleConfigGridListObj = (from s in VehicleConfigGridListObj
                                            where s.IndendedUse.ToLower() == ("special order") || s.IndendedUse.ToLower() == ("stgo ail")
                                            select s).ToList();
            }

            if (!string.IsNullOrEmpty(searchVhclIntend))
            {
                VehicleConfigGridListObj = GetVehicleList(searchVhclIntend, VehicleConfigGridListObj);

                switch (searchVhclIntend)
                {
                    case "stgo road recovery":
                        searchVhclIntend = "STGO Road recovery operation";
                        break;
                    case "stgo engineering plant wheeled":
                        searchVhclIntend = "STGO Engineering plant(not tracked)";
                        break;
                    case "stgo mobile crane":
                        searchVhclIntend = "STGO Mobile crane";
                        break;
                    default:
                        break;
                }
                TempData["SearchVhclIntend"] = searchVhclIntend;
            }

            if (searchVhclType != 0)
            {
                VehicleConfigGridListObj = (from s in VehicleConfigGridListObj
                                            where s.VehicleType == searchVhclType
                                            select s).ToList();

                TempData["SearchVhclType"] = searchVhclType;
            }

            if (Session["PageSize"] == null)
            {
                Session["PageSize"] = 10;
            }

            if (pageSize == null)
            {
                pageSize = (Session["PageSize"] != null) ? (int)Session["PageSize"] : 10;
            }
            else
            {
                Session["PageSize"] = pageSize;
            }
            int pageNumber;
            if (page == null && Session["PreviousVehicleListPage"] != null)
            {
                pageNumber = (int)Session["PreviousVehicleListPage"];
            }
            else
            {
                pageNumber = (page ?? 1);
            }
            ViewBag.isApplicationVehicle = isApplicationVehicle;
            ViewBag.isNotifVehicle = isNotifVehicle;
            //Setting Pagination
            ViewBag.pageSize = pageSize;
            Session["PreviousVehicleListPage"] = pageNumber;
            //for search string
            ViewBag.SearchValue = searchString;
            ViewBag.importFlag = importFlag;
            if (MovementClassCode != 0 && !isNotifVehicle)
            {
                ViewBag.isNotifVehicle = false;
                ViewBag.isApplicationVehicle = true;
            }

            if (new SessionData().Wf_Fm_FleetManagementId.Length > 1 && new SessionData().Wf_Fm_FleetManagementId != WorkflowActivityConstants.Gn_Failed)
            {
                TerminateWorkflowProcess(new SessionData().Wf_Fm_FleetManagementId);
                new SessionData().Wf_Fm_VehicleConfigurationId = string.Empty;
                new SessionData().Wf_Fm_CurrentExecuted = WorkflowActivityTypes.Gn_NotDecided;
                new SessionData().Wf_Fm_FleetManagementId = string.Empty;
            }
            if (!importFlag)
            {
                VehicleConfigGridListObj.ForEach(x => x.IndendedUse = x.IndendedUse.Replace(x.IndendedUse, STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleCategoryFleetMapping(x.VehiclePurpose)));
            }
            if (VehicleConfigGridListObj.Count <= 10 && (Convert.ToString(Session["UserSearchString"]) != searchString && searchString != null))
                pageNumber = 1;

            ViewBag.page = pageNumber;

            ViewBag.vehicleType = new SelectList(GetAllVehicleType(), "Id", "Value", ViewBag.TypeSearch);
            if (searchVhclIntend == "c and u")
                searchVhclIntend = "Construction and use";
            if (Session["movementClassificationName"] != null)
            {
                ViewBag.vehicleIntend = new SelectList(GetVehicleIntend(searchVhclIntend, SessionInfo.UserTypeId), searchVhclIntend);
            }
            else
            {
                ViewBag.vehicleIntend = new SelectList(GetVehicleIntend("", SessionInfo.UserTypeId), searchVhclIntend);
            }

            return View(VehicleConfigGridListObj.ToPagedList(pageNumber, (int)pageSize));
        }
        #endregion public ActionResult VehicleConfigList(int? page, int? pageSize, string searchString)

        #region public ActionResult SaveVehicleConfigSearch(string search)
        public ActionResult SaveVehicleConfigSearch(string searchString = null, string vehicleIntend = null, int vehicleType = 0, bool isNotifVehicle = false, int MovementClassCode = 0, bool isApplicationVehicle = false, bool importFlag = false, int filterFavouritesVehConfig = 0, int? sortOrder = null, int? sortType = null,int? page=1, int? pageSize=10)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , JsonResult,VehicleConfigController/SaveVehicleConfigSearch ", Session.SessionID));
            if (vehicleIntend == "Construction and use")
            {
                vehicleIntend = "c and u";
            }
            var g_VehicleConfigSearch = importFlag ? "g_VehicleConfigSearch_Import" : "g_VehicleConfigSearch";
            var g_VehicleConfigTypeSearch = importFlag ? "g_VehicleConfigTypeSearch_Import" : "g_VehicleConfigTypeSearch";
            var g_VehicleConfigIntendSearch = importFlag ? "g_VehicleConfigIntendSearch_Import" : "g_VehicleConfigIntendSearch";
            if (searchString == "")
            {
                searchString = null;
                Session[g_VehicleConfigSearch] = null;
            }
            else
            {
                Session[g_VehicleConfigSearch] = searchString;
            }
            if (vehicleIntend == "")
            {
                vehicleIntend = null;
            }
            if (vehicleType == 0)
            {
                Session[g_VehicleConfigTypeSearch] = null;
            }
            else
            {
                Session[g_VehicleConfigTypeSearch] = vehicleType;
            }
            Session[g_VehicleConfigIntendSearch] = vehicleIntend;
            if (MovementClassCode == 0)
            {
                Session["movementClassificationName"] = null;
                Session["movementClassificationId"] = null;
            }

            return RedirectToAction("VehicleConfigList", new
            {
                B7vy6imTleYsMr6Nlv7VQ =
                        STP.Web.Helpers.EncryptionUtility.Encrypt("isNotifVehicle=" + isNotifVehicle +
                        "&searchString=" + searchString +
                        "&searchVhclType=" + vehicleType +
                        "&searchVhclIntend=" + vehicleIntend +
                        "&isApplicationVehicle=" + isApplicationVehicle +
                        "&importFlag=" + importFlag +
                        "&filterFavouritesVehConfig=" + filterFavouritesVehConfig +
                        "&sortOrder=" + sortOrder +
                        "&sortType=" + sortType +
                        "&page=" + page +
                        "&pageSize=" + pageSize)
            });
        }
        #endregion

        #region public ActionResult CreateConfiguration()
        //Method for showing partial view of CreateConfiguration
        public ActionResult CreateConfiguration(int vhclClassification = 0, bool isApplicationVehicle = false, bool isVR1 = false, bool IsNotifVeh = false, string Guid = "", int isEdit = 0, bool isMovement = false, int vehicleConfigId = 0, List<VehicleConfigList> componentIdLists = null, bool isCandidate = false)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/CreateConfiguration , Load partial view of CreateConfiguration", Session.SessionID));
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }
            #region application vehicle
            if (isApplicationVehicle && !isVR1)
                ViewBag.newApplicationVehicle = true;
            if (isVR1)
                ViewBag.VR1appln = true;
            #endregion
            ViewBag.IsNotifVeh = IsNotifVeh;

            ViewBag.IsEdit = isEdit;
            ViewBag.IsMovement = isMovement;
            ViewBag.Guid = Guid;
            ViewBag.vehicleConfigId = vehicleConfigId;
            ViewBag.isCandidate = isCandidate;
            List<VehicleConfigList> componentIdList = new List<VehicleConfigList>();
            if (vehicleConfigId != 0)
            {
                componentIdList = componentIdLists;
                if (isMovement && !isCandidate)
                {
                    componentIdList = vehicleComponentService.GetComponentIdTemp(Guid, vehicleConfigId, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                }
                else if (isCandidate)
                {
                    componentIdList = vehicleconfigService.GetVR1VehicleConfigVhclID(vehicleConfigId, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                }
                else
                {
                    componentIdList = vehicleconfigService.GetVehicleConfigVhclID(vehicleConfigId, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                }

            }
            else
            {
                componentIdList = vehicleComponentService.GetComponentIdTemp(Guid, vehicleConfigId, SessionInfo.UserSchema).OrderBy(s => s.ComponentId).ToList();
            }

            if (componentIdList.Count == 0)
            {
                Session["ComponentIdDeleted"] = 1;
            }
            if (Guid == "" && vehicleConfigId == 0 && !isMovement)
            {
                Session.Remove("movementClassificationId");
                Session.Remove("movementClassificationName");
            }
            if (Guid == "")
            {
                Session.Remove("ComponentId");
                Session.Remove("ComponentIdDeleted");
                if (new SessionData().Wf_Fm_FleetManagementId.Length > 1 && new SessionData().Wf_Fm_FleetManagementId != WorkflowActivityConstants.Gn_Failed)
                {
                    TerminateWorkflowProcess(new SessionData().Wf_Fm_FleetManagementId);

                }
                new SessionData().Wf_Fm_VehicleConfigurationId = string.Empty;
                new SessionData().Wf_Fm_CurrentExecuted = WorkflowActivityTypes.Gn_NotDecided;
                new SessionData().Wf_Fm_FleetManagementId = string.Empty;
            }

            Session["g_VehicleComponentSearch"] = "";
            Session["g_VehicleTypeSearch"] = "";
            Session["g_VehicleIntendSearch"] = "";

            return View(componentIdList);
        }
        #endregion



        #region public ActionResult ConfigurationGeneralPage()
        //Method for showing  view of ConfigurationGeneralPage
        public ActionResult ConfigurationGeneralPage()
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/ConfigurationGeneralPage , Load view of ConfigurationGeneralPage", Session.SessionID));
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }
            string guid = "";
            if (new SessionData().Wf_Fm_FleetManagementId.ToLower() != "failed")
            {
                guid = new SessionData().Wf_Fm_FleetManagementId;
            }
            List<VehicleConfigList> componentIdList = new List<VehicleConfigList>();
            componentIdList = vehicleComponentService.GetComponentIdTemp(guid, 0, SessionInfo.UserSchema);
            List<int> compIds = new List<int>();
            foreach (var id in componentIdList)
            {
                compIds.Add((int)id.ComponentId);
            }

            List<uint> vehicleConfig = vehicleconfigService.AssessConfigurationType(compIds, false, UserSchema.Portal);

            List<VehicleConfigurationType> configdrpdwn = null;
            ComponentConfiguration vehicleParams = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
            configdrpdwn = vehicleParams.GetConfigType();
            List<VehicleConfigurationType> configList = configdrpdwn.Where(x => vehicleConfig.Contains((uint)x.ConfigurationTypeId)).ToList();


            ViewBag.VehicleTypeConfig = new SelectList(configList, "ConfigurationTypeId", "ConfigurationName");

            if (vehicleConfig.Count > 0)
            {
                bool moveToEnterVehicleDetails = false;
                if (vehicleConfig.Count == 1)
                    moveToEnterVehicleDetails = true;
                if (new SessionData().Wf_Fm_CurrentExecuted == WorkflowActivityTypes.Fm_ManualEntryComponent)
                {


                    new SessionData().Wf_Fm_CurrentExecuted = WorkflowActivityTypes.Fm_Activity_ChooseVehicleConfigurationType;
                    ProcessWorkflowActivity(null, true, new SessionData().Wf_Fm_ImportFromFleet, moveToEnterVehicleDetails, false);
                    if (new SessionData().Wf_Fm_ImportFromFleet)
                    {
                        new SessionData().Wf_Fm_ImportFromFleet = false;
                    }
                }
                if (new SessionData().Wf_Fm_ImportFromFleet)
                {
                    new SessionData().Wf_Fm_ImportFromFleet = false;
                    ProcessWorkflowActivity(null, true, true, moveToEnterVehicleDetails, false);
                }
            }
            return View();
        }
        #endregion

        #region public JsonResult CreateConfiguration(VehicleConfiguration vehicleConfigObj, int configTypeId)
        /// <summary>
        /// Method to create configuration
        /// </summary>
        /// <param name="vehicleConfigObj"></param>
        /// <param name="configTypeId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult OldCreateConfiguration(VehicleConfiguration vehicleConfigObj, int configTypeId, int? speedUnit = null, List<RegistrationParams> registrationParams = null, bool isMovement = false, int AplnMovemntId = 0, bool isCandidate = false, int CandRevisionId = 0)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , HttpPost,VehicleConfigController/CreateConfiguration , Create new vehicle config", Session.SessionID));

            UserInfo SessionInfo = null;
            int portalType = 0;
            bool isAdmin = false;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
                portalType = SessionInfo.UserTypeId;
                if (portalType == 696006 || SessionInfo.IsAdmin == 1)
                {
                    isAdmin = true;
                }
            }
            double configurationid = 0;
            double movementId = 0;
            try
            {

                int organisationId;
                if (SessionInfo.UserTypeId == 696008)
                {
                    organisationId = (int)Session["SORTOrgID"];
                }
                else
                {
                    organisationId = (int)SessionInfo.OrganisationId;
                }
                NewConfigurationModel vehicleConfiguration = ConvertToConfiguration(vehicleConfigObj);
                vehicleConfiguration.OrganisationId = organisationId;
                vehicleConfiguration.VehicleType = configTypeId;
                vehicleConfiguration.VehiclePurpose = vehicleConfigObj.moveClassification.ClassificationId;
                vehicleConfiguration.SpeedUnit = speedUnit;
                List<MovementVehicleConfig> movementVehicle = new List<MovementVehicleConfig>();
                if (isCandidate)
                {
                    vehicleConfiguration.CandRevisionId = CandRevisionId;
                    vehicleConfiguration = vehicleconfigService.InsertVR1VehicleConfiguration(vehicleConfiguration, SessionInfo.UserSchema);
                    configurationid = (double)vehicleConfiguration.VehicleId;
                }
                else if (isMovement)
                {
                    vehicleConfiguration.MovementId = AplnMovemntId;
                    movementVehicle = vehicleconfigService.InsertConfigurationTemp(vehicleConfiguration, SessionInfo.UserSchema);
                    configurationid = movementVehicle[0].VehicleId;
                    movementId = movementVehicle[0].MovementId;
                }
                else
                {
                    configurationid = vehicleconfigService.CreateConfiguration(vehicleConfiguration);
                }
                vehicleConfiguration.VehicleId = Convert.ToInt32(configurationid);
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , HttpPost,VehicleConfigController/CreateConfiguration , Created new vehicle config - {1}", Session.SessionID, configurationid));

                int IdNumber = 0;
                if (configurationid > 0)
                {
                    if (isCandidate)
                    {
                        if (registrationParams != null)
                        {
                            foreach (RegistrationParams registration in registrationParams)
                            {
                                IdNumber = vehicleconfigService.SaveVR1VehicleRegistration((int)configurationid, registration.RegistrationValue, registration.FleetId, SessionInfo.UserSchema);
                            }
                        }
                    }
                    else if (isMovement)
                    {
                        if (registrationParams != null)
                        {
                            foreach (RegistrationParams registration in registrationParams)
                            {
                                IdNumber = vehicleconfigService.CreateVehicleRegistrationTemp((int)configurationid, registration.RegistrationValue, registration.FleetId, SessionInfo.UserSchema);
                            }
                        }
                    }
                    else
                    {
                        if (registrationParams != null)
                        {
                            foreach (RegistrationParams registration in registrationParams)
                            {
                                IdNumber = vehicleconfigService.CreateVehicleRegistration((int)configurationid, registration.RegistrationValue, registration.FleetId);
                            }
                        }
                    }
                    if (new SessionData().Wf_Fm_FleetManagementId.ToLower() != "failed")
                    {
                        string guid = new SessionData().Wf_Fm_FleetManagementId;
                        if (isCandidate)
                        {
                            bool result = vehicleconfigService.InsertVehicleConfigPosnTemp(guid, (int)configurationid, UserSchema.Sort);
                            AssessMoveTypeParams moveTypeParams = new AssessMoveTypeParams
                            {
                                VehicleId = (int)vehicleConfiguration.VehicleId,
                                IsRoute = 1,
                                UserSchema = SessionInfo.UserSchema,
                                PreviousMovementType = null,
                                ForceApplication = SessionInfo.UserSchema == UserSchema.Sort,
                                configuration = null
                            };
                            VehicleMovementType vehicleMovementType = vehicleconfigService.AssessMovementType(moveTypeParams);
                            if (vehicleMovementType.MovementType != (int)MovementType.special_order)
                            {
                                bool dltresult = vehicleconfigService.DeleteSelectedVR1VehicleComponent((int)vehicleConfiguration.VehicleId, SessionInfo.UserSchema);
                                configurationid = 0;
                            }
                        }
                        else if (isMovement)
                        {
                            bool result = vehicleconfigService.InsertMovementConfigPosnTemp(guid, (int)configurationid, SessionInfo.UserSchema);
                        }
                        else
                        {
                            bool result = vehicleconfigService.InsertVehicleConfigPosnTemp(guid, (int)configurationid, UserSchema.Portal);
                        }
                    }

                }
                if (!isMovement)
                {
                    Session["movementClassificationName"] = null;
                    Session["movementClassificationId"] = null;
                }
                //For saving configuration details at the time of vehicle save
                if (Session["vehicleWorkFlowParams"] != null)
                {
                    VehicleWorkFlowParams vehicleWorkFlow = (VehicleWorkFlowParams)Session["vehicleWorkFlowParams"];
                    vehicleWorkFlow.VehicleConfigurationModels.ConfigurationModel = vehicleConfiguration;

                    Session["vehicleWorkFlowParams"] = vehicleWorkFlow;
                }
                else
                {
                    VehicleWorkFlowParams vehicleWorkFlowParams = new VehicleWorkFlowParams();
                    vehicleWorkFlowParams.VehicleConfigurationModels.ConfigurationModel = vehicleConfiguration;

                    Session["vehicleWorkFlowParams"] = vehicleWorkFlowParams;
                }

                //------------------------------------

                // Complete configuration creation.
                new SessionData().Wf_Fm_CurrentExecuted = WorkflowActivityTypes.Fm_ConfigurationCompleted;
                ProcessWorkflowActivity(null, true, false, false);
                new SessionData().Wf_Fm_CurrentExecuted = WorkflowActivityTypes.Gn_NotDecided;
                #region System Event Log - haulier_created_fleet_vehicle
                string ErrMsg = string.Empty;
                string sysEventDescp = string.Empty;

                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                movactiontype.UserName = SessionInfo.UserName;
                movactiontype.FleetVehicleId = (long)vehicleConfiguration.VehicleId;
                movactiontype.FleetVehicleName = vehicleConfiguration.VehicleName;
                movactiontype.SystemEventType = SysEventType.haulier_created_fleet_vehicle;

                sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                #endregion
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] , VehicleConfigController/CreateConfiguration, Exception: {1}", Session.SessionID, ex.Message));
                throw ex;
            }

            return Json(new { configId = configurationid, movementId = movementId });
        }
        #endregion

        #region public ActionResult GeneralPage(int movementId, int vehicleConfigId)
        public ActionResult OldGeneralPage(int movementId, int vehicleConfigId, bool ISNotifVeh = false)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/GeneralPage , Load Vehicle Config General Page movement id : {1} and config Id : {2}", Session.SessionID, movementId, vehicleConfigId));
                if (Session["UserInfo"] != null)
                {
                    var sessionValues = (UserInfo)Session["UserInfo"];
                    ViewBag.Units = sessionValues.VehicleUnits;
                }
                NewConfigurationModel vehicleconfig = new NewConfigurationModel();
                ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
                MovementClassificationConfig mvClassConfig = compConfigObj.GetMovementClassificationConfig(movementId);

                VehicleConfiguration vehicleConfigObj = mvClassConfig.GetVehicleConfiguration(vehicleConfigId);
                ViewBag.SpeedUnits = new SelectList(GetSpeedUnits(), "Id", "Value", vehicleconfig.SpeedUnit);
                //--------------------Added by Poonam 2 Aug 16------------------------------
                //#6151 code for making gross weight field mandetory for Notitifiction 
                if (ISNotifVeh && mvClassConfig.MovementClassification.ClassificationName == "Construction and use")
                {
                    for (int i = 0; i < vehicleConfigObj.VehicleConfigParamList.Count; i++)
                    {
                        if (vehicleConfigObj.VehicleConfigParamList[i].DisplayString == "Gross weight")
                        {
                            vehicleConfigObj.VehicleConfigParamList[i].IsRequired = 1;
                        }
                    }
                }
                //----------------------------------------------------------------------------
                string guid = "";
                if (new SessionData().Wf_Fm_FleetManagementId.ToLower() != "failed")
                {
                    guid = new SessionData().Wf_Fm_FleetManagementId;
                }
                ConfigurationModel configurationModel = new ConfigurationModel();

                configurationModel = vehicleconfigService.GetConfigDimensions(guid, vehicleConfigId, UserSchema.Portal);

                if (vehicleConfigObj != null)
                {
                    for (int i = 0; i < vehicleConfigObj.VehicleConfigParamList.Count; i++)
                    {
                        if (vehicleConfigObj.VehicleConfigParamList[i].InputType == "LABEL")
                        {
                            if (vehicleConfigObj.VehicleConfigParamList[i].ParamModel == "Weight")
                            {
                                vehicleConfigObj.VehicleConfigParamList[i].ParamValue = configurationModel.GrossWeight.ToString();
                            }
                            else if (vehicleConfigObj.VehicleConfigParamList[i].ParamModel == "Length")
                            {
                                vehicleConfigObj.VehicleConfigParamList[i].ParamValue = configurationModel.RigidLength.ToString();
                            }
                            else if (vehicleConfigObj.VehicleConfigParamList[i].ParamModel == "OverallLength")
                            {
                                vehicleConfigObj.VehicleConfigParamList[i].ParamValue = configurationModel.OverallLength.ToString();
                            }
                            else if (vehicleConfigObj.VehicleConfigParamList[i].ParamModel == "Width")
                            {
                                vehicleConfigObj.VehicleConfigParamList[i].ParamValue = configurationModel.Width.ToString();
                            }
                            else if (vehicleConfigObj.VehicleConfigParamList[i].ParamModel == "Maximum Height")
                            {
                                vehicleConfigObj.VehicleConfigParamList[i].ParamValue = configurationModel.MaxHeight.ToString();
                            }
                            else if (vehicleConfigObj.VehicleConfigParamList[i].ParamModel == "AxleWeight")
                            {
                                vehicleConfigObj.VehicleConfigParamList[i].ParamValue = configurationModel.MaxAxleWeight.ToString();
                            }
                            else if (vehicleConfigObj.VehicleConfigParamList[i].ParamModel == "WheelBase")
                            {
                                vehicleConfigObj.VehicleConfigParamList[i].ParamValue = configurationModel.WheelBase.ToString();
                            }
                        }
                    }
                }
                if (new SessionData().Wf_Fm_CurrentExecuted == WorkflowActivityTypes.Fm_Activity_ChooseMovementType)
                {
                    new SessionData().Wf_Fm_CurrentExecuted = WorkflowActivityTypes.Fm_Activity_VehicleDetailsEntry;
                    ProcessWorkflowActivity(null, true, false);
                }
                return PartialView("GeneralPage", vehicleConfigObj.VehicleConfigParamList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] , JsonResult,VehicleConfigController/GeneralPage , Exception - {1}", Session.SessionID, ex.Message));
                throw;
            }
        }
        #endregion

        #region public ActionResult ViewConfiguration(int vehicleID, bool isRoute=false)
        /// <summary>
        /// Viewing Vehicle configuration
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public ActionResult ViewConfiguration(int vehicleID, bool isRoute = false, int movementId = 0, bool isImportConfiguration = false, bool isNotif = false, string flag = "", bool isPolice = false, bool ImportBtn = false, bool isSort = false, bool IsNEN = false, bool importFlag = false)
        {
            try
            {
                ViewBag.IsNEN = IsNEN;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/ViewConfiguration , View Vehicle Configuration", Session.SessionID));
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                if (isSort)
                {
                    SessionInfo.UserSchema = UserSchema.Sort;
                }
                ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
                ConfigurationModel VehicleConfig = null;

                bool isVR1 = false;
                if (isRoute)
                {
                    if (flag == "SOApp" || flag == "")
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration , View SO Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));

                        VehicleConfig = vehicleconfigService.GetRouteConfigInfo(vehicleID, SessionInfo.UserSchema);
                        if (VehicleConfig.MovementClassificationId == 0)
                        {
                            VehicleConfig.MovementClassificationId = movementId;
                        }
                    }
                    else
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration , View VR1 Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                        VehicleConfig = vehicleconfigService.GetRouteConfigInfoForVR1(vehicleID, SessionInfo.UserSchema);
                        if (VehicleConfig.MovementClassificationId == 0)
                        {
                            VehicleConfig.MovementClassificationId = movementId;
                        }
                        isVR1 = true;
                    }
                }
                else if (isNotif || IsNEN)
                {
                    if (flag == "Notif" || IsNEN)
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration , View Notif Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                        VehicleConfig = vehicleconfigService.GetRouteConfigInfoForVR1(vehicleID, SessionInfo.UserSchema);
                    }
                    else
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration , View Notif Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                        VehicleConfig = vehicleconfigService.GetNotifVehicleConfigByID(vehicleID);
                        vehicleID = (int)VehicleConfig.ConfigurationId;
                    }
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration , View fleet Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                    //VehicleConfig = vehicleconfigService.GetConfigInfo(vehicleID, SessionInfo.UserSchema);
                    VehicleConfig = vehicleconfigService.GetVehicleDetails(vehicleID, false, UserSchema.Portal);
                }
                ViewBag.TravelSpeed = VehicleConfig.TravellingSpeedUnit;
                ViewBag.ConfigTypeId = VehicleConfig.ConfigurationTypeId;

                //MovementClassificationConfig mvClassConfig = compConfigObj.GetMovementClassificationConfig(VehicleConfig.MovementClassificationId);
                //VehicleConfiguration vehicleConfigObj = mvClassConfig.GetVehicleConfiguration(VehicleConfig.ConfigurationTypeId);

                MovementClassificationConfig mvClassConfig = new MovementClassificationConfig();
                VehicleConfiguration vehicleConfigObj = compConfigObj.GetVehicleConfiguration(VehicleConfig.ConfigurationTypeId);

                if (vehicleConfigObj != null)
                {
                    vehicleConfigObj.UpdateConfigProperties(VehicleConfig);
                }
                ComponentModel VehicleComponentObj = null;
                List<VehicleConfigList> vehicleConfigList=null;
                if (isRoute)
                {
                    if (flag == "SOApp" || flag == "")
                    {
                        vehicleConfigList = vehicleconfigService.GetRouteVehicleConfigVhclID(vehicleID, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                        if(vehicleConfigList.Count==1)
                            VehicleComponentObj = vehicleComponentService.GetRouteComponent((int)vehicleConfigList[0].ComponentId, SessionInfo.UserSchema);
                    }
                    else
                    {
                        vehicleConfigList = vehicleconfigService.GetVR1VehicleConfigVhclID(vehicleID, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                        if (vehicleConfigList.Count == 1)
                            VehicleComponentObj = vehicleComponentService.GetVR1VehicleComponent((int)vehicleConfigList[0].ComponentId, SessionInfo.UserSchema);
                    }
                }
                else if (isNotif)
                {
                    vehicleConfigList = vehicleconfigService.GetVR1VehicleConfigVhclID(vehicleID, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                    if (vehicleConfigList.Count == 1)
                        VehicleComponentObj = vehicleComponentService.GetVR1VehicleComponent((int)vehicleConfigList[0].ComponentId, SessionInfo.UserSchema);
                }
                else
                {
                    vehicleConfigList = vehicleconfigService.GetVehicleConfigVhclID(vehicleID, UserSchema.Portal).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                    if (vehicleConfigList.Count == 1)
                        VehicleComponentObj = vehicleComponentService.GetVehicleComponent((int)vehicleConfigList[0].ComponentId);
                }
                ViewBag.ComponentList= vehicleConfigList;
                ViewBag.IsRoute = isRoute;
                if (isRoute)
                {
                    if (flag == "SOApp" || flag == "")
                    {
                        vehicleConfigObj.VehicleRegList = vehicleconfigService.GetRouteVehicleRegistrationDetails(vehicleID, SessionInfo.UserSchema);
                    }
                    else
                    {
                        vehicleConfigObj.VehicleRegList = vehicleconfigService.GetVR1VehicleRegistrationDetails(vehicleID, SessionInfo.UserSchema);
                    }
                }
                else if (isNotif)
                {
                    vehicleConfigObj.VehicleRegList = vehicleconfigService.GetVR1VehicleRegistrationDetails(vehicleID, SessionInfo.UserSchema);
                }
                else
                {
                    vehicleConfigObj.VehicleRegList = vehicleconfigService.GetVehicleRegistrationDetails(vehicleID, UserSchema.Portal);
                }
                ViewBag.isImportConfiguration = isImportConfiguration;
                ViewBag.isNotif = isNotif;
                ViewBag.IsPolice = isPolice;
                ViewBag.ImportBtn = ImportBtn;
                ViewBag.vehicleID = vehicleID;
                ViewBag.movementTypeId = VehicleConfig.MovementClassificationId;

                ViewBag.isVR1 = isVR1;
                ViewBag.importFlag = importFlag;

                if (vehicleConfigList.Count == 1 && vehicleConfigObj.vehicleConfigType.ConfigurationTypeId == (int)ConfigurationType.RecoveryVehicle)
                {
                    var rigidlength = vehicleConfigObj.VehicleConfigParamList.FirstOrDefault(r => r.ParamModel == "Length");
                    if (rigidlength != null)
                        vehicleConfigObj.VehicleConfigParamList.Remove(rigidlength);
                }

                List<string> CompImage = new List<string>();
                int MovementXmlTypeId = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleXmlMovementTypeMapping((VehicleXmlMovementType)(int)VehicleConfig.MovementClassificationId);

                foreach (var component in ViewBag.ComponentList)
                {
                    VehicleComponent vehclCompObj = GetVehicleComponent((int)component.ComponentTypeId, (int)component.ComponentSubTypeId, MovementXmlTypeId);
                    CompImage.Add(vehclCompObj.vehicleComponentSubType.ImageName);

                    if (MovementXmlTypeId != 0)
                    {
                        if (MovementXmlTypeId == 270101)
                            ViewBag.IsConfigTyreCentreSpacing = false;
                        else
                            ViewBag.IsConfigTyreCentreSpacing = TyreDetailsRequired((int)component.ComponentTypeId, VehicleConfig.MovementClassificationId);

                    }

                    if (VehicleComponentObj != null && vehicleConfigList.Count == 1)
                    {
                        vehclCompObj.UpdateVehicleProperties(VehicleComponentObj);
                        vehicleConfigObj = ConvertComponentToConfig(vehclCompObj, vehicleConfigObj);
                    }
                }
                ViewBag.vehicleImage = CompImage;

                List<Axle> axles = GetAxleDetails(vehicleConfigList, isRoute, isNotif, false, isVR1);
                int isAxleDetails = axles.Count;
                ViewBag.isAxleDetails = isAxleDetails;

                return View(vehicleConfigObj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration, Exception: {1}", Session.SessionID, ex.Message));
                throw;
            }
        }
        #endregion public ActionResult ViewConfiguration(int vehicleID, bool isRoute=false)

        [HttpPost]
        public JsonResult ImportVehicleFromList(long vehicleId, int ApplnRevId = 0, bool isNotif = false, bool isVR1 = false, string ContentRefNo = "0", bool IsCandidate = false, int NotificationId = 0, string VersionType = "A")
        {
            long result = 0;
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , JsonResult,VehicleConfigController/CopyVehicleFromList ,Copy vehicle {1} rev id {2} content ref ", Session.SessionID, vehicleId, ApplnRevId, ContentRefNo));
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                int iscandidateveh = 0;
                if (IsCandidate)
                    iscandidateveh = 1;
                result = vehicleconfigService.ImportVehicleFromList(vehicleId, SessionInfo.UserSchema, ApplnRevId, isNotif, isVR1, ContentRefNo, iscandidateveh, VersionType);
                if (IsCandidate)
                {
                    AssessMoveTypeParams moveTypeParams = new AssessMoveTypeParams
                    {
                        VehicleId = (int)result,
                        IsRoute = 1,
                        UserSchema = SessionInfo.UserSchema,
                        PreviousMovementType = null,
                        ForceApplication = SessionInfo.UserSchema == UserSchema.Sort,
                        configuration = null
                    };
                    VehicleMovementType vehicleMovementType = vehicleconfigService.AssessMovementType(moveTypeParams);
                    if (vehicleMovementType.MovementType != (int)MovementType.special_order)
                    {
                        result = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] , VehicleConfigController/CopyVehicleFromList, Exception: {1}", Session.SessionID, ex.Message));
                throw ex;
            }


            return Json(new { Success = result });
        }

        #region public ActionResult ViewConfigurationGeneral(int vehicleID, bool isRoute=false)
        /// <summary>
        /// Viewing Vehicle configuration
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public ActionResult ViewConfigurationGeneral(int vehicleID, bool isRoute = false, int movementId = 0, bool isImportConfiguration = false, bool isNotif = false, string flag = "", bool isPolice = false, bool ImportBtn = false, bool isSort = false, bool IsNEN = false, bool isMovement = false, int NotificationEditFlag = 0, bool isOverviewDisplay = false)
        {
            try
            {
                ViewBag.IsNEN = IsNEN;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/ViewConfiguration , View Vehicle Configuration", Session.SessionID));
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                if (isSort)
                {
                    SessionInfo.UserSchema = UserSchema.Sort;
                }
                ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
                ConfigurationModel VehicleConfig = null;
                if (Session["AppFlag"] != null && flag.ToLower() != "candidatevehicle")
                    flag = Convert.ToString(Session["AppFlag"]);
                if (Session["IsRoute"] != null)
                    isRoute = Convert.ToBoolean(Session["IsRoute"]);
                if (Session["IsNotif"] != null)
                    isNotif = Convert.ToBoolean(Session["IsNotif"]);

                bool isVR1 = false;

                if (isRoute && !isMovement)
                {
                    if (flag == "SOApp" || flag == "")
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration , View SO Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));

                        VehicleConfig = vehicleconfigService.GetRouteConfigInfo(vehicleID, SessionInfo.UserSchema);
                        if (VehicleConfig.MovementClassificationId == 0)
                        {
                            VehicleConfig.MovementClassificationId = movementId;
                        }
                    }
                    else
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration , View VR1 Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                        VehicleConfig = vehicleconfigService.GetRouteConfigInfoForVR1(vehicleID, SessionInfo.UserSchema);
                        if (VehicleConfig.MovementClassificationId == 0)
                        {
                            VehicleConfig.MovementClassificationId = movementId;
                        }
                        if (flag.ToLower() == "candidatevehicle" || flag.ToLower() == "vr1app")
                            isVR1 = true;
                    }
                }
                else if ((isNotif || IsNEN) && !isMovement)
                {
                    if (flag == "Notif" || IsNEN)
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration , View Notif Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                        VehicleConfig = vehicleconfigService.GetRouteConfigInfoForVR1(vehicleID, SessionInfo.UserSchema);
                    }
                    else
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration , View Notif Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                        if (flag.ToLower() == "candidatevehicle")
                        {
                            VehicleConfig = vehicleconfigService.GetRouteConfigInfoForVR1(vehicleID, SessionInfo.UserSchema);
                            if (VehicleConfig.MovementClassificationId == 0)
                            {
                                VehicleConfig.MovementClassificationId = movementId;
                            }
                        }
                        else
                        {
                            VehicleConfig = vehicleconfigService.GetNotifVehicleConfigByID(vehicleID);
                            vehicleID = (int)VehicleConfig.ConfigurationId;
                        }
                    }
                }
                else if (isMovement)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration , View Movement Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                    //VehicleConfig = vehicleconfigService.GetMovementConfigInfo(vehicleID, SessionInfo.UserSchema);
                    VehicleConfig = vehicleconfigService.GetVehicleDetails(vehicleID, true, SessionInfo.UserSchema);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration , View fleet Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                    VehicleConfig = vehicleconfigService.GetVehicleDetails(vehicleID, false, SessionInfo.UserSchema);
                }
                ViewBag.TravelSpeed = VehicleConfig.TravellingSpeedUnit;

                //MovementClassificationConfig mvClassConfig = compConfigObj.GetMovementClassificationConfig(VehicleConfig.MovementClassificationId);
                //VehicleConfiguration vehicleConfigObj = mvClassConfig.GetVehicleConfiguration(VehicleConfig.ConfigurationTypeId);

                MovementClassificationConfig mvClassConfig = new MovementClassificationConfig();
                VehicleConfiguration vehicleConfigObj = compConfigObj.GetVehicleConfiguration(VehicleConfig.ConfigurationTypeId);
                if (vehicleConfigObj != null)
                {
                    vehicleConfigObj.UpdateConfigProperties(VehicleConfig);
                }
                if (isMovement && SessionInfo.UserSchema != UserSchema.Sort)
                {
                    var itemToRemove1 = vehicleConfigObj.VehicleConfigParamList.FirstOrDefault(r => r.ParamModel == "Notes");
                    vehicleConfigObj.VehicleConfigParamList.Remove(itemToRemove1);
                }
                ComponentModel VehicleComponentObj = null;
                List<VehicleConfigList> vehicleConfigList = null;
                if (isRoute && !isMovement)
                {
                    if (flag == "SOApp" || flag == "")
                    {
                        vehicleConfigList = vehicleconfigService.GetRouteVehicleConfigVhclID(vehicleID, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                        if (vehicleConfigList.Count == 1)
                            VehicleComponentObj = vehicleComponentService.GetRouteComponent((int)vehicleConfigList[0].ComponentId, SessionInfo.UserSchema);
                    }
                    else
                    {
                        vehicleConfigList = vehicleconfigService.GetVR1VehicleConfigVhclID(vehicleID, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                        if (vehicleConfigList.Count == 1)
                            VehicleComponentObj = vehicleComponentService.GetVR1VehicleComponent((int)vehicleConfigList[0].ComponentId, SessionInfo.UserSchema);
                    }
                }
                else if (isNotif && !isMovement)
                {
                    vehicleConfigList = vehicleconfigService.GetVR1VehicleConfigVhclID(vehicleID, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                    if (vehicleConfigList.Count == 1)
                        VehicleComponentObj = vehicleComponentService.GetVR1VehicleComponent((int)vehicleConfigList[0].ComponentId, SessionInfo.UserSchema);
                }
                else if (isMovement)
                {
                    vehicleConfigList = vehicleconfigService.GetMovementVehicleConfig(vehicleID, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                    if (vehicleConfigList.Count == 1)
                        VehicleComponentObj=vehicleComponentService.GetComponentTemp((int)vehicleConfigList[0].ComponentId, "", SessionInfo.UserSchema);
                }
                else
                {
                    vehicleConfigList = vehicleconfigService.GetVehicleConfigVhclID(vehicleID, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                    if (vehicleConfigList.Count == 1)
                        VehicleComponentObj = vehicleComponentService.GetVehicleComponent((int)vehicleConfigList[0].ComponentId);
                }
                ViewBag.ComponentList = vehicleConfigList;
                ViewBag.IsRoute = isRoute;

                if (isRoute && !isMovement)
                {
                    if (flag == "SOApp" || flag == "")
                    {
                        vehicleConfigObj.VehicleRegList = vehicleconfigService.GetRouteVehicleRegistrationDetails(vehicleID, SessionInfo.UserSchema);
                    }
                    else
                    {
                        vehicleConfigObj.VehicleRegList = vehicleconfigService.GetVR1VehicleRegistrationDetails(vehicleID, SessionInfo.UserSchema);
                    }
                }
                else if (isNotif && !isMovement)
                {
                    vehicleConfigObj.VehicleRegList = vehicleconfigService.GetVR1VehicleRegistrationDetails(vehicleID, SessionInfo.UserSchema);
                }
                else if (isMovement)
                {
                    vehicleConfigObj.VehicleRegList = vehicleconfigService.GetMovementVehicleRegDetails(vehicleID, SessionInfo.UserSchema);
                }
                else
                {
                    vehicleConfigObj.VehicleRegList = vehicleconfigService.GetVehicleRegistrationDetails(vehicleID, SessionInfo.UserSchema);
                }
                ViewBag.isImportConfiguration = isImportConfiguration;
                ViewBag.isNotif = isNotif;
                ViewBag.IsPolice = isPolice;
                ViewBag.ImportBtn = ImportBtn;
                ViewBag.vehicleID = vehicleID;
                ViewBag.IsRoute = isRoute;
                ViewBag.flag = flag;

                ViewBag.isVR1 = isVR1;
                ViewBag.IsMovement = isMovement;
                ViewBag.NotificationEditFlag = NotificationEditFlag;
                ViewBag.OverviewDisplayVehicleId = vehicleID;
                ViewBag.IsOverviewDisplay = isOverviewDisplay;
                ViewBag.movementTypeId = VehicleConfig.MovementClassificationId;
                ViewBag.configTypeId = vehicleConfigObj.vehicleConfigType.ConfigurationTypeId;

                if (vehicleConfigList.Count == 1 && vehicleConfigObj.vehicleConfigType.ConfigurationTypeId == (int)ConfigurationType.RecoveryVehicle)
                {
                    var rigidlength = vehicleConfigObj.VehicleConfigParamList.FirstOrDefault(r => r.ParamModel == "Length");
                    if (rigidlength != null)
                        vehicleConfigObj.VehicleConfigParamList.Remove(rigidlength);
                }

                int MovementXmlTypeId = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleXmlMovementTypeMapping((VehicleXmlMovementType)(int)VehicleConfig.MovementClassificationId);

                if (ViewBag.ComponentList != null)
                {
                    List<VehicleComponent> VehicleComponentList = new List<VehicleComponent>();
                    foreach (var component in ViewBag.ComponentList)
                    {
                        VehicleComponent vehclCompObj = GetVehicleComponent((int)component.ComponentTypeId, (int)component.ComponentSubTypeId, MovementXmlTypeId);
                        if (vehclCompObj != null)
                        {
                            if (MovementXmlTypeId != 0)
                            {
                                if (MovementXmlTypeId == 270101)
                                    ViewBag.IsConfigTyreCentreSpacing = false;
                                else
                                    ViewBag.IsConfigTyreCentreSpacing = TyreDetailsRequired((int)component.ComponentTypeId, VehicleConfig.MovementClassificationId);

                            }
                            if (VehicleComponentObj != null && vehicleConfigList.Count == 1)
                            {
                                vehclCompObj.UpdateVehicleProperties(VehicleComponentObj);
                                vehicleConfigObj = ConvertComponentToConfig(vehclCompObj, vehicleConfigObj);
                            }

                            //To Fix Image Issue For Engg Plant
                            VehicleComponentList.Add(vehclCompObj);
                        }
                    }
                    if (vehicleConfigList.Count == 1)
                    {
                        UpdateVehicleConfiguration(vehicleConfigObj, vehicleConfigObj.vehicleConfigType.ConfigurationTypeId, vehicleID, null, null, isMovement, isVR1, VehicleConfig.MovementClassificationId);
                    }
                    ViewBag.VehicleComponentList = VehicleComponentList;
                }

                List<Axle> axles = GetAxleDetails(vehicleConfigList, isRoute, isNotif, isMovement, isVR1);
                int isAxleDetails = axles.Count;
                ViewBag.isAxleDetails = isAxleDetails;
                return PartialView("ViewConfigurationGeneral", vehicleConfigObj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration, Exception: {1}", Session.SessionID, ex.Message));
                throw;
            }
        }
        #endregion public ActionResult ViewConfigurationGeneral(int vehicleID, bool isRoute=false)

        #region public ActionResult ViewConfigRegistration(List<VehicleRegistration> vehicleRegistrations)
        public ActionResult ViewConfigRegistration(List<VehicleRegistration> vehicleRegistrations)
        {
            return PartialView("~/Views/Vehicle/ViewRegistrationComponent.cshtml", vehicleRegistrations);
        }
        #endregion public ActionResult ViewConfigRegistration(List<VehicleRegistration> vehicleRegistrations)
        #region public ActionResult ViewConfigurationRegistration(List<VehicleRegistration> vehicleRegistrations)
        public ActionResult ViewConfigurationRegistration(List<VehicleRegistration> vehicleRegistrations)
        {
            return PartialView("~/Views/Vehicle/ViewComponentRegistration.cshtml", vehicleRegistrations);
        }
        #endregion public ActionResult ViewConfigurationRegistration(List<VehicleRegistration> vehicleRegistrations)

        #region public JsonResult DeleteConfiguration(int vehicleID)
        [HttpPost]
        public JsonResult DeleteConfiguration(int vehicleId, string vehicleName = "")
        {
            bool result = false;
            try
            {
                UserInfo SessionInfo = null;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , JsonResult,VehicleConfigController/DeleteConfiguration , Delete vehicle config - {1}", Session.SessionID, vehicleId));
                result = vehicleconfigService.DisableVehicleApi(vehicleId);
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                if (result)
                {
                    #region System Event Log - haulier_edited_fleet_vehicle
                    string ErrMsg = string.Empty;
                    string sysEventDescp = string.Empty;

                    MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                    movactiontype.UserName = SessionInfo.UserName;
                    movactiontype.FleetVehicleId = vehicleId;
                    movactiontype.FleetVehicleName = vehicleName;
                    movactiontype.SystemEventType = SysEventType.haulier_deleted_fleet_vehicle;

                    sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] , VehicleConfigController/DeleteConfiguration, Exception: {1}", Session.SessionID, ex.Message));
                throw;
            }
            return Json(new { success = result });
        }
        #endregion public JsonResult DeleteConfiguration(int vehicleID)

        #region public JsonResult ImportComponent(int vehicleConfigId, int componentId, int vehicleTypeId, int latitudePos, int longitudePos)
        /// <summary>
        /// Method to Save component details and position
        /// </summary>
        /// <param name="vehicleConfigId"></param>
        /// <param name="componentId"></param>
        /// <param name="vehicleTypeId"></param>
        /// <param name="latitudePos"></param>
        /// <param name="longitudePos"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult OldImportComponent(int vehicleConfigId, int componentId, int vehicleTypeId, int latitudePos, int longitudePos)
        {
            ViewBag.componentId = componentId;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , HttpPost,VehicleConfigController/ImportComponent , Import component with id - {1} for vehicle config with id - {2}", Session.SessionID, componentId, vehicleConfigId));
            //SP call must be from here
            bool success = true;
            try
            {
                VehicleConfigList ConfigPosn = new VehicleConfigList();
                ConfigPosn.VehicleId = vehicleConfigId;
                ConfigPosn.ComponentId = componentId;
                ConfigPosn.LatPosn = longitudePos;
                ConfigPosn.LongPosn = latitudePos;
                ConfigPosn.SubType = vehicleTypeId;

                var vehicleConfigList = vehicleconfigService.CreateConfigPosn(ConfigPosn);
                if (vehicleConfigList.ComponentId == 0)
                {
                    success = false;
                }
                else
                {
                    #region System Event Log - haulier_imported_component
                    string ErrMsg = string.Empty;
                    string sysEventDescp = string.Empty;

                    UserInfo SessionInfo = null;
                    if (Session["UserInfo"] != null)
                    {
                        SessionInfo = (UserInfo)Session["UserInfo"];
                    }

                    MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                    movactiontype.UserName = SessionInfo.UserName;
                    movactiontype.FleetVehicleId = vehicleConfigId;
                    movactiontype.FleetComponentId = (int)vehicleConfigList.ComponentId;
                    movactiontype.SystemEventType = SysEventType.haulier_imported_component;

                    sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                    #endregion
                }
                var userInfo = (UserInfo)Session["UserInfo"];

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] , VehicleConfigController/ImportComponent, Exception: {1}", Session.SessionID, ex.Message));
                throw ex;
            }
            return Json(new { Success = success });
        }
        #endregion public JsonResult ImportComponent(int vehicleConfigId, int componentId, int vehicleTypeId, int latitudePos, int longitudePos)

        #region public ActionResult RegistrationConfiguration(int vehicleId)
        public ActionResult OldRegistrationConfiguration(int vehicleId, bool RegBtn = true, bool isVR1 = false, bool isAmend = false, bool planMovement = false, bool isEditVehicleInSoProcessing = false)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/RegistrationConfiguration , Load Vehicle Config Registration Page", Session.SessionID));
            ViewBag.IsTractor = true;
            ViewBag.VehicleTypeId = 1;
            ViewBag.IsAmend = isAmend;
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }

            List<VehicleRegistration> listVehclRegObj = new List<VehicleRegistration>();
            try
            {
                if (RegBtn && !isVR1 && !isEditVehicleInSoProcessing)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/RegistrationConfiguration , Load fleet Vehicle Config Registration Page vehicle id : {1}", Session.SessionID, vehicleId));

                    if (planMovement)
                    {
                        listVehclRegObj = vehicleconfigService.GetVehicleRegistrationTemp(vehicleId, SessionInfo.UserSchema);
                    }
                    else
                    {
                        listVehclRegObj = vehicleconfigService.GetVehicleRegistrationDetails(vehicleId, SessionInfo.UserSchema);
                    }
                }
                else if (isVR1 || isEditVehicleInSoProcessing)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/RegistrationConfiguration , Load VR1 Vehicle Config Registration Page vehicle id : {1}", Session.SessionID, vehicleId));

                    listVehclRegObj = vehicleconfigService.GetVR1VehicleRegistrationDetails(vehicleId, SessionInfo.UserSchema);

                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/RegistrationConfiguration , Load SO Vehicle Config Registration Page vehicle id : {1}", Session.SessionID, vehicleId));

                    listVehclRegObj = vehicleconfigService.GetApplVehicleRegistrationDetails(vehicleId, SessionInfo.UserSchema);
                }

                ViewBag.RegBtn = RegBtn;
                ViewBag.VR1appln = isVR1;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] , VehicleConfigController/RegistrationConfiguration, Exception: {1}", Session.SessionID, ex.Message));
                throw ex;
            }
            return PartialView("RegistrationConfiguration", listVehclRegObj);
        }
        #endregion public ActionResult RegistrationConfiguration(int vehicleId)

        #region public JsonResult SaveRegistrationID(int configId, string registrationId, string fleetId)
        /// <summary>
        /// JsonResult method to save Registration details
        /// </summary>
        /// <returns>bool as json result</returns>
        //  [HttpPost]
        public JsonResult SaveRegistrationID(int configId, List<RegistrationParams> registrationParams) //SaveRegistrationID(Name name)
        {
            int IdNumber = 0;
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , JsonResult,VehicleConfigController/SaveRegistrationID , Save vehicle config registration details for id - {1}", Session.SessionID, configId));
                foreach (RegistrationParams registration in registrationParams)
                {
                    IdNumber = vehicleconfigService.CreateVehicleRegistration(configId, registration.RegistrationValue, registration.FleetId);
                }
                #region System Event Log - added_registration_for_fleet_vehicle
                string ErrMsg = string.Empty;
                string sysEventDescp = string.Empty;

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }

                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                movactiontype.UserName = SessionInfo.UserName;
                movactiontype.FleetVehicleId = configId;
                movactiontype.FleetVehicleIdNo = IdNumber;
                movactiontype.SystemEventType = SysEventType.haulier_added_registration_for_fleet_vehicle;

                sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                #endregion
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] , VehicleConfigController/SaveRegistrationID, Exception: {1}", Session.SessionID, ex.Message));
                throw ex;
            }
            return Json(new { Success = IdNumber });
        }
        #endregion public JsonResult SaveRegistrationID(int configId, string registrationId, string fleetId)

        #region public JsonResult DeleteComponentConfiguration(int componentId)
        [HttpPost]
        public JsonResult OldDeleteComponentConfiguration(int componentId, int vehicleId, bool isMovement = false)
        {
            string guid = "";
            List<VehicleConfigList> componentIdList = new List<VehicleConfigList>();
            try
            {
                UserInfo SessionInfo = null;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , JsonResult,VehicleConfigController/DeleteConfiguration , Delete vehicle config component- {1}", Session.SessionID, componentId));
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                int result = 0;
                if (new SessionData().Wf_Fm_FleetManagementId.ToLower() != "failed")
                {
                    guid = new SessionData().Wf_Fm_FleetManagementId;
                    if (isMovement)
                    {
                        componentIdList = vehicleComponentService.GetComponentIdTemp(guid, vehicleId, SessionInfo.UserSchema);
                    }
                    else
                    {
                        componentIdList = vehicleconfigService.GetVehicleConfigVhclID(vehicleId, UserSchema.Portal);
                    }
                }
                if (vehicleId != 0)
                {
                    result = vehicleComponentService.DeleteComponentConfig(componentId, vehicleId, isMovement, SessionInfo.UserSchema);
                }
                else
                {
                    result = vehicleComponentService.DeleteComponentTemp(componentId);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] , VehicleConfigController/DeleteConfiguration, Exception: {1}", Session.SessionID, ex.Message));
                throw;
            }
            try
            {
                //Fm_Wf: Delete Component
                var componentList = GetPayloadComponents();
                if (componentList.Count > 0)
                {
                    componentList.RemoveAll(x => x.ComponentModel != null && x.ComponentModel.ComponentId.Equals(componentId));
                }
                VehicleWorkFlowParams vehicleWorkFlow = (VehicleWorkFlowParams)Session["vehicleWorkFlowParams"];
                vehicleWorkFlow.VehicleComponentsModels = componentList;
                vehicleWorkFlow.TotalComponents = componentList.Count;
                Session["vehicleWorkFlowParams"] = vehicleWorkFlow;
                ProcessWorkflowActivity(componentList, false);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] ,Workflow: VehicleConfigController/DeleteConfiguration, Exception: {1}", Session.SessionID, ex.Message));
            }
            return Json(new { vehicleId = vehicleId, guid = guid, componentIdList = componentIdList });
        }
        #endregion public JsonResult DeleteConfiguration(int vehicleID)

        #region public JsonResult FillVehicleConfigType(int movementId)
        public JsonResult FillVehicleConfigType(int movementId)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , JsonResult,VehicleConfigController/FillVehicleConfigType , Fill vehicle config type dropdown for movement id - {1}", Session.SessionID, movementId));
                ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
                MovementClassificationConfig moveClassConfigObj = compConfigObj.GetMovementClassificationConfig(movementId);
                List<VehicleConfigurationType> vhclConfigTypeList = moveClassConfigObj.GetVehicleConfigList();

                return Json(new { result = vhclConfigTypeList });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] , JsonResult,VehicleConfigController/FillVehicleConfigType , Exception - {1}", Session.SessionID, ex.Message));
                throw ex;
            }
        }
        #endregion public JsonResult FillVehicleConfigType(int movementId)

        #region copy vehicle from list for notification,vr1 and so applications
        public JsonResult CopyVehicleFromList(long vehicleId, int ApplnRevId = 0, bool isNotif = false, bool isVR1 = false, string ContentRefNo = "0", bool IsCandidate = false, int NotificationId = 0)
        {
            long result = 0;
            try
            {

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , JsonResult,VehicleConfigController/CopyVehicleFromList ,Copy vehicle {1} rev id {2} content ref ", Session.SessionID, vehicleId, ApplnRevId, ContentRefNo));
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                int iscandidateveh = 0;
                if (IsCandidate)
                    iscandidateveh = 1;
                result = vehicleconfigService.CopyVehicleFromList(vehicleId, SessionInfo.UserSchema, ApplnRevId, isNotif, isVR1, ContentRefNo, iscandidateveh);

                #region System events for sort_copied_vehicle_for SO and Vr1
                if (result > 0)
                {
                    STP.Domain.LoggingAndReporting.MovementActionIdentifiers movactiontype = new STP.Domain.LoggingAndReporting.MovementActionIdentifiers();
                    movactiontype.RevisionId = ApplnRevId;
                    movactiontype.VehicleId = Convert.ToInt32(vehicleId);
                    movactiontype.UserName = SessionInfo.UserName;
                    movactiontype.ContentRefNo = ContentRefNo;
                    movactiontype.NotificationID = NotificationId;
                    string ErrMsg = string.Empty;
                    int user_ID = Convert.ToInt32(SessionInfo.UserId);

                    if (SessionInfo.UserSchema == UserSchema.Sort) // For SORT Vehicle copy Log
                    {
                        if (!isVR1)//for SORT SO application
                        {
                            #region Saving sort_copied_vehicle_for_so_application
                            movactiontype.SystemEventType = SysEventType.sort_copied_vehicle_for_so_application;
                            #endregion
                        }
                        else               //for SORT VR1 application
                        {
                            #region Saving Sort_copied_vehicle_for_vr1_application
                            movactiontype.SystemEventType = SysEventType.Sort_copied_vehicle_for_vr1_application;
                            #endregion
                        }
                        string sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                        bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, SessionInfo.UserSchema);
                    }
                    else if (SessionInfo.UserSchema == UserSchema.Portal)
                    {
                        // For Haulier Vehicle creation Log
                        if (!isVR1 && !isNotif)//for SO application
                        {
                            #region Saving Haulier_copied_vehicle_for_so_application
                            movactiontype.SystemEventType = SysEventType.Haulier_copied_vehicle_for_so_application;
                            #endregion
                        }
                        else if (isVR1)              //for VR1 application
                        {
                            #region Saving Haulier_copied_vehicle_for_vr1_application
                            movactiontype.SystemEventType = SysEventType.Haulier_copied_vehicle_for_vr1_application;
                            #endregion
                        }
                        else if (isNotif)              //for notification application
                        {
                            #region Saving Haulier_copied_vehicle_for_notification
                            movactiontype.ContentRefNo = ContentRefNo;
                            movactiontype.SystemEventType = SysEventType.Haulier_copied_vehicle;
                            #endregion
                        }
                        string sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                        bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, SessionInfo.UserSchema);

                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] , VehicleConfigController/CopyVehicleFromList, Exception: {1}", Session.SessionID, ex.Message));
                throw ex;
            }


            return Json(new { Success = result });
        }
        #endregion

        [HttpPost]
        public JsonResult ConfigTypeNext(int vehicleTypeId)
        {
            List<Int64> componentList = (List<Int64>)Session["ComponentId"];
            List<uint> movementClassification = vehicleconfigService.AssessMovementClassificationType(componentList, vehicleTypeId, false, UserSchema.Portal);

            List<MovementClassification> mvmntdrpdwn = null;
            ComponentConfiguration vehicleParams = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
            mvmntdrpdwn = vehicleParams.GetMovementClassification();
            List<MovementClassification> mvmntList = mvmntdrpdwn.Where(x => movementClassification.Contains((uint)x.ClassificationId)).ToList();

            ViewBag.MovementClassConfig = new SelectList(mvmntList, "ClassificationId", "ClassificationName");

            if (new SessionData().Wf_Fm_CurrentExecuted == WorkflowActivityTypes.Fm_Activity_ChooseVehicleConfigurationType)
            {
                new SessionData().Wf_Fm_CurrentExecuted = WorkflowActivityTypes.Fm_Activity_ChooseMovementType;
                ProcessWorkflowActivity(null, true, false);
            }
            return Json(new { type = mvmntList });
        }
        [HttpPost]
        public JsonResult MovementNext()
        {
            if (new SessionData().Wf_Fm_CurrentExecuted == WorkflowActivityTypes.Fm_Activity_ChooseVehicleConfigurationType)
            {
                new SessionData().Wf_Fm_CurrentExecuted = WorkflowActivityTypes.Fm_Activity_ChooseMovementType;
                ProcessWorkflowActivity(null, true, false);
            }
            return Json(true);
        }


        #region public ActionResult GetGeneralPageEdit(int vehicleId)
        public ActionResult GetGeneralPageEdit(int vehicleId, bool planMovement = false, bool isEditVehicleInSoProcessing = false)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , Get,VehicleConfigController/GetGeneralPageEdit , Load edit general details page for vehicle config - {1}", Session.SessionID, vehicleId));
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                var sessionValues = (UserInfo)Session["UserInfo"];
                ViewBag.Units = sessionValues.VehicleUnits;
            }
            SessionInfo = (UserInfo)Session["UserInfo"];
            ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];

            ConfigurationModel VehicleConfig;
            if (isEditVehicleInSoProcessing)
            {
                VehicleConfig = vehicleconfigService.GetRouteConfigInfoForVR1(vehicleId, SessionInfo.UserSchema, 1);
            }
            else
            {
                VehicleConfig = vehicleconfigService.GetVehicleDetails(vehicleId, planMovement, SessionInfo.UserSchema);
            }
            MovementClassificationConfig mvClassConfig = new MovementClassificationConfig();
            ViewBag.SpeedUnits = new SelectList(GetSpeedUnits(), "Id", "Value", VehicleConfig.TravellingSpeedUnit);
            ViewBag.TravelSpeed = VehicleConfig.TravellingSpeedUnit;
            if (ViewBag.TravelSpeed == null || ViewBag.TravelSpeed == 0)
            {
                ViewBag.TravelSpeed = 229001;
            }
            //VehicleConfiguration vehicleConfigObj = mvClassConfig.GetVehicleConfiguration(VehicleConfig.ConfigurationTypeId);
            VehicleConfiguration vehicleConfigObj = compConfigObj.GetVehicleConfiguration(VehicleConfig.ConfigurationTypeId);
            if (vehicleConfigObj != null)
            {
                vehicleConfigObj.UpdateConfigProperties(VehicleConfig);
            }

            if (!isEditVehicleInSoProcessing)
            {
                ConfigurationModel configurationModel = new ConfigurationModel();
                if (planMovement)
                {
                    configurationModel = vehicleconfigService.GetMovementConfigDimensions(vehicleId, SessionInfo.UserSchema);
                }
                else
                {
                    configurationModel = vehicleconfigService.GetVehicleDimensions(vehicleId, vehicleConfigObj.vehicleConfigType.ConfigurationTypeId, SessionInfo.UserSchema);
                }
                VehicleConfig = configurationModel;
            }
            if (vehicleConfigObj != null)
            {
                for (int i = 0; i < vehicleConfigObj.VehicleConfigParamList.Count; i++)
                {
                    if (vehicleConfigObj.VehicleConfigParamList[i].InputType == "LABEL")
                    {
                        if (vehicleConfigObj.VehicleConfigParamList[i].ParamModel == "Weight")
                        {
                            vehicleConfigObj.VehicleConfigParamList[i].ParamValue = VehicleConfig.GrossWeight.ToString();
                        }
                        else if (vehicleConfigObj.VehicleConfigParamList[i].ParamModel == "Length")
                        {
                            vehicleConfigObj.VehicleConfigParamList[i].ParamValue = VehicleConfig.RigidLength.ToString();
                        }
                        else if (vehicleConfigObj.VehicleConfigParamList[i].ParamModel == "OverallLength")
                        {
                            vehicleConfigObj.VehicleConfigParamList[i].ParamValue = VehicleConfig.OverallLength.ToString();
                        }
                        else if (vehicleConfigObj.VehicleConfigParamList[i].ParamModel == "Width")
                        {
                            vehicleConfigObj.VehicleConfigParamList[i].ParamValue = VehicleConfig.Width.ToString();
                        }
                        else if (vehicleConfigObj.VehicleConfigParamList[i].ParamModel == "Maximum Height")
                        {
                            vehicleConfigObj.VehicleConfigParamList[i].ParamValue = VehicleConfig.MaxHeight.ToString();
                        }
                        else if (vehicleConfigObj.VehicleConfigParamList[i].ParamModel == "AxleWeight")
                        {
                            vehicleConfigObj.VehicleConfigParamList[i].ParamValue = VehicleConfig.MaxAxleWeight.ToString();
                        }
                        else if (vehicleConfigObj.VehicleConfigParamList[i].ParamModel == "WheelBase")
                        {
                            vehicleConfigObj.VehicleConfigParamList[i].ParamValue = VehicleConfig.WheelBase.ToString();
                        }
                    }
                }
            }

            return PartialView("GeneralPage", vehicleConfigObj.VehicleConfigParamList);
        }
        #endregion public ActionResult GetGeneralPageEdit(int vehicleId)

        #region public ActionResult GeneralPageEdit(int vehicleId)
        public ActionResult GeneralPageEdit(int vehicleId, int MovementClassConfig, int VehicleTypeConfig, bool movement = false, bool isEditVehicleInSoProcessing = false, int grossWeight = 0)
        {
            ViewBag.VehicleId = vehicleId;
            ViewBag.MovementClassConfig = MovementClassConfig;
            ViewBag.VehicleTypeConfig = VehicleTypeConfig;
            ViewBag.PlanMovement = movement;
            ViewBag.isCandidate = isEditVehicleInSoProcessing;
            ViewBag.grossWeight = grossWeight;
            ProcessWorkflowActivity(null, true, false, isEditConfiguration: true);
            return View();
        }
        #endregion

        #region public JsonResult CheckFormalName(int componentid)
        public JsonResult CheckFormalName(int componentid, int vehicleConfigId = 0, bool isMovement = false, int organisationId = 0)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , JsonResult,VehicleConfigController/CheckFormalName , Check for internal name ", Session.SessionID));
            UserInfo SessionInfo = null;

            SessionInfo = (UserInfo)Session["UserInfo"];

            if (SessionInfo.UserTypeId != UserType.Sort)
            {
                organisationId = (int)SessionInfo.OrganisationId;
            }

            int result = 0;
            if (vehicleConfigId != 0)
            {
                if (isMovement)
                {
                    result = vehicleconfigService.MovementCheckFormalNameExists(componentid, organisationId, SessionInfo.UserSchema);
                }
                else
                {
                    result = vehicleconfigService.CheckFormalName(componentid, organisationId);
                }
            }
            else
            {
                result = vehicleconfigService.CheckFormalNameExistsTemp(componentid, organisationId);
            }
            //if (!isApplication)
            //{
            //    //here goes mirza's code
            //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , JsonResult,VehicleConfigController/CheckFormalName , Check for internal name for fleet vehicle - {1}", Session.SessionID, componentid));
            //    if (vehicleConfigId != 0)
            //    {
            //        result = vehicleconfigService.CheckFormalName(componentid, organisationId);
            //    }
            //    else
            //    {
            //        result = vehicleconfigService.CheckFormalNameExistsTemp(componentid, organisationId);
            //    }

            //}
            //else if (isVR1)
            //{
            //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , JsonResult,VehicleConfigController/CheckFormalName , Check for internal name for VR1 vehicle - {1}", Session.SessionID, componentid));
            //    result = vehicleconfigService.CheckVR1FormalName(componentid, organisationId, SessionInfo.UserSchema);
            //}
            //else
            //{
            //    //here goes my code
            //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , JsonResult,VehicleConfigController/CheckFormalName , Check for internal name for application vehicle - {1}", Session.SessionID, componentid));
            //    result = vehicleconfigService.CheckAppFormalName(componentid, organisationId, SessionInfo.UserSchema);
            //}


            return Json(new { success = result });
        }
        #endregion public JsonResult CheckFormalName(int componentid)

        #region public JsonResult AddComponentToFleet(int componentid)
        public JsonResult AddComponentToFleet(int componentid, int vehicleConfigId = 0, int flag = 0, bool isMovement = false, int organisationId = 0)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , JsonResult,VehicleConfigController/AddComponentToFleet , Add component to fleet ", Session.SessionID));
            UserInfo SessionInfo = null;

            SessionInfo = (UserInfo)Session["UserInfo"];
            if (SessionInfo.UserTypeId != UserType.Sort)
            {
                organisationId = (int)SessionInfo.OrganisationId;
            }

            MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
            movactiontype.ComponentId = componentid;
            //movactiontype.RevisionId = isAppRevId;
            movactiontype.VehicleId = vehicleConfigId;
            movactiontype.UserName = SessionInfo.UserName;
            //movactiontype.NotificationID = NotificationID;
            string ErrMsg = string.Empty;
            int user_ID = Convert.ToInt32(SessionInfo.UserId);

            int result = 0;
            string guid = null;
            //if (!isApplication)
            //{
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , JsonResult,VehicleConfigController/AddComponentToFleet , Add fleet config component {1} to fleet ", Session.SessionID, componentid));

            if (new SessionData().Wf_Fm_FleetManagementId.ToLower() != "failed")
            {
                guid = new SessionData().Wf_Fm_FleetManagementId;
            }
            if (vehicleConfigId != 0)
            {
                if (isMovement)
                {
                    result = vehicleconfigService.AddMovementComponentToFleet(componentid, organisationId, SessionInfo.UserSchema);
                }
                else
                {
                    result = vehicleconfigService.AddComponentToFleet(componentid, organisationId);
                }
            }
            else
            {
                //if (isMovement)
                //{
                //    result = vehicleconfigService.AddMovementComponentToFleet(componentid, organisationId);
                //}
                //else
                //{
                result = vehicleComponentService.AddToFleetTemp(guid, componentid, vehicleConfigId);
                //}
            }

            if (result > 0)
            {
                movactiontype.FleetVehicleId = vehicleConfigId;
                movactiontype.FleetComponentId = componentid;
                movactiontype.SystemEventType = SysEventType.haulier_added_new_fleet_component;
                string sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, SessionInfo.UserSchema);
            }
            //}
            //else if (isVR1)
            //{
            //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , JsonResult,VehicleConfigController/AddComponentToFleet , Add VR1 config component {1} to fleet ", Session.SessionID, componentid));
            //    result = vehicleconfigService.AddVR1ComponentToFleet(componentid, organisationId, flag, SessionInfo.UserSchema);

            //    if (result > 0)// added_component_to_fleet_for_vr1_application
            //    {
            //        #region System events for added_component_to_fleet_for_vr1_application
            //        if (SessionInfo.UserSchema == UserSchema.Sort) // For SORT sort_added_component_to_fleet_for_so_application
            //        {
            //            #region Saving Sort_added_component_to_fleet_for_vr1_application
            //            movactiontype.SystemEventType = SysEventType.Sort_added_component_to_fleet_for_vr1_application;
            //            #endregion
            //        }
            //        else if (SessionInfo.UserSchema == UserSchema.Portal)
            //        {
            //            #region Saving Haulier_added_component_to_fleet
            //            if (NotificationID != 0)
            //            {
            //                movactiontype.SystemEventType = SysEventType.Haulier_added_component_to_fleet_for_notification;
            //            }
            //            else
            //            {
            //                movactiontype.SystemEventType = SysEventType.Haulier_added_component_to_fleet_for_vr1_application;
            //            }
            //            #endregion
            //        }
            //        string sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
            //        bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, SessionInfo.UserSchema);
            //        #endregion
            //    }
            //}
            //else
            //{
            //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , JsonResult,VehicleConfigController/AddComponentToFleet , Add SO config component {1} to fleet ", Session.SessionID, componentid));
            //    result = vehicleconfigService.AddApplicationComponentToFleet(componentid, organisationId, flag, SessionInfo.UserSchema);

            //    if (result > 0)// added_component_to_fleet_for_so_application
            //    {
            //        #region System events for added_component_to_fleet_for_so_application
            //        if (SessionInfo.UserSchema == UserSchema.Sort) // For SORT sort_added_component_to_fleet_for_so_application
            //        {
            //            #region Saving sort_added_component_to_fleet_for_so_application
            //            movactiontype.SystemEventType = SysEventType.sort_added_component_to_fleet_for_so_application;
            //            #endregion
            //        }
            //        else if (SessionInfo.UserSchema == UserSchema.Portal)
            //        {
            //            #region Saving sort_added_component_to_fleet_for_so_application
            //            movactiontype.SystemEventType = SysEventType.Haulier_added_component_to_fleet_for_so_application;
            //            #endregion
            //        }
            //        string sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
            //        bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, SessionInfo.UserSchema);
            //        #endregion
            //    }
            //}

            return Json(new { success = result });
        }
        #endregion public JsonResult AddComponentToFleet(int componentid)

        #region Movement Vehicle
        public ActionResult InsertMovementVehicle(long movementId, long vehicleId, int flag,bool goToMovementPage=false)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            string workFlowKey = new SessionData().Wf_An_ApplicationWorkflowId;
            PlanMvmntPayLoad mvmntPayLoad = new PlanMvmntPayLoad();
            var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
            if (workFlowKey != string.Empty || workFlowKey != WorkflowActivityConstants.Gn_Failed)
                mvmntPayLoad = applicationNotificationManagement.GetPlanMvmtPayload();
            if (!goToMovementPage)
            {
                InsertMovementVehicle movementVehicle = new InsertMovementVehicle
                {
                    MovementId = movementId,
                    VehicleId = vehicleId,
                    Flag = flag,
                    NotificationId = mvmntPayLoad.NotificationId,
                    RevisionId = mvmntPayLoad.RevisionId,
                    IsVr1 = mvmntPayLoad.IsVr1App ? 1 : 0,
                    UserSchema = SessionInfo.UserSchema
                };
                List<MovementVehicleConfig> movementVehicleConfig = vehicleconfigService.InsertMovementVehicle(movementVehicle);
                AssignMovementClassification(movementVehicleConfig[0].VehiclePurpose);
                movementId = movementVehicleConfig[0].MovementId;
            }
            return RedirectToAction("MovementSelectedVehicles", "Movements", new
            {
                B7vy6imTleYsMr6Nlv7VQ =
                        Helpers.EncryptionUtility.Encrypt("movementId=" + movementId +
                        "&isVehicleModify=1")
            });
        }
        public JsonResult DeleteMovementVehicle(long movementId, long vehicleId, long mainVehicleId = 0)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            bool status = false;
            status = vehicleconfigService.DeleteMovementVehicle(movementId, vehicleId, SessionInfo.UserSchema);
            string workFlowKey = new SessionData().Wf_An_ApplicationWorkflowId;
            PlanMvmntPayLoad mvmntPayLoad = new PlanMvmntPayLoad();
            var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
            if (workFlowKey != string.Empty || workFlowKey != WorkflowActivityConstants.Gn_Failed)
                mvmntPayLoad = applicationNotificationManagement.GetPlanMvmtPayload();

            if(mainVehicleId != 0 && mvmntPayLoad.VehicleAssignmentList != null && mvmntPayLoad.VehicleAssignmentList.Count > 0)
            {
                List<VehicleAssignment> vehicleAssignments = mvmntPayLoad.VehicleAssignmentList.FindAll(item => item.VehicleIds.Contains(mainVehicleId));
                if (vehicleAssignments != null && vehicleAssignments.Count > 0)
                {
                    mvmntPayLoad.VehicleAssignmentList.ForEach(x => x.VehicleIds.Remove(mainVehicleId));
                }
                if (applicationNotificationManagement.IsThisMovementExist(mvmntPayLoad.NotificationId, mvmntPayLoad.RevisionId, out string workflowKey)
                    && WorkflowTaskFinder.FindNextTask("HaulierApplication", WorkflowActivityTypes.An_Activity_VehicleAddUpdate, out dynamic workflowPayload) != string.Empty)
                {
                    dynamic dataPayload = new ExpandoObject();
                    dataPayload.PlanMvmntPayLoad = mvmntPayLoad;
                    WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                    {
                        data = dataPayload,
                        workflowData = workflowPayload
                    };
                    applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);
                }
            }
            
            if (status)
            {
                List<MovementVehicleConfig> movementVehicleConfig = vehicleconfigService.SelectMovementVehicle(movementId, SessionInfo.UserSchema);
                Session["MovementSelectedVehicles"] = movementVehicleConfig;
            }
            return Json(new { Success = status });
        }
        public ActionResult GetPreviousMovementList(long versionId = 0, string cont_Ref_No = null, long appRevisionId = 0, bool isVehicleImport = false, string movementType = "",int isHistoric=0)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            List<MovementVehicleList> prevMovementVehicleLists = vehicleconfigService.GetRouteVehicleList(appRevisionId, versionId, cont_Ref_No, SessionInfo.UserSchema);

            OldGetVehicleImage(prevMovementVehicleLists);
            TempData["PreviousMovementVehicle"] = prevMovementVehicleLists;
            bool isNotifVehicle = false;
            if (cont_Ref_No != null && cont_Ref_No != "")
            {
                isNotifVehicle = true;
            }
            if ((isVehicleImport&&prevMovementVehicleLists.Any(i => i.VehicleList.Count == 0))|| prevMovementVehicleLists.Any(i => i.RoutePartId == null))
            {
                return Json(new { flag = true });
            }
            else
            {
                return RedirectToAction("MovementRouteVehicle", "Movements", new
                {
                    B7vy6imTleYsMr6Nlv7VQ =
                            STP.Web.Helpers.EncryptionUtility.Encrypt("isVehicleImport=" + isVehicleImport +
                            "&selectedMovementType=" + movementType +
                            "&isNotifVehicle=" + isNotifVehicle)
                });
            }
        }
        public JsonResult GetRouteVehicleList(long versionId = 0, string cont_Ref_No = "", long appRevisionId = 0)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            List<MovementVehicleList> routeVehicleList = vehicleconfigService.GetRouteVehicleList(appRevisionId, versionId, cont_Ref_No, SessionInfo.UserSchema);
            ViewBag.MovementRouteVehicleList = routeVehicleList;
            return Json(new { data = routeVehicleList });
        }
        #endregion

        public ActionResult OldSelectVehicleComponent(bool isCandidate = false)
        {
            ViewBag.IsCandidate = isCandidate;
            return PartialView("SelectVehicleComponent");
        }

        public ActionResult VehicleComponentDetails(string importFrm = "")
        {
            ViewBag.importFrm = importFrm;
            return PartialView("VehicleComponentDetails");
        }

        #region public ActionResult ViewVehicleConfiguration(int vehicleID, bool isRoute=false)
        /// <summary>
        /// Viewing Vehicle configuration
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public ActionResult ViewVehicleConfiguration(int vehicleID, bool isRoute = false, int movementId = 0, bool isImportConfiguration = false, bool isNotif = false, string flag = "", bool isPolice = false, bool ImportBtn = false, bool isSort = false, bool IsNEN = false, bool isMovement = false)
        {
            try
            {
                ViewBag.IsNEN = IsNEN;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/ViewConfiguration , View Vehicle Configuration", Session.SessionID));
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                if (isSort)
                {
                    SessionInfo.UserSchema = UserSchema.Sort;
                }
                ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
                ConfigurationModel VehicleConfig = null;

                bool isVR1 = false;
                if (isRoute)
                {
                    if (flag == "SOApp" || flag == "")
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration , View SO Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));

                        VehicleConfig = vehicleconfigService.GetRouteConfigInfo(vehicleID, SessionInfo.UserSchema);
                        if (VehicleConfig.MovementClassificationId == 0)
                        {
                            VehicleConfig.MovementClassificationId = movementId;
                        }
                    }
                    else
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration , View VR1 Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                        VehicleConfig = vehicleconfigService.GetRouteConfigInfoForVR1(vehicleID, SessionInfo.UserSchema);
                        if (VehicleConfig.MovementClassificationId == 0)
                        {
                            VehicleConfig.MovementClassificationId = movementId;
                        }
                        isVR1 = true;
                    }
                }
                else if (isNotif || IsNEN)
                {
                    if (flag == "Notif" || IsNEN)
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration , View Notif Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                        VehicleConfig = vehicleconfigService.GetRouteConfigInfoForVR1(vehicleID, SessionInfo.UserSchema);
                    }
                    else
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration , View Notif Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));

                        if (flag.ToLower() == "candidatevehicle")
                        {
                            VehicleConfig = vehicleconfigService.GetRouteConfigInfoForVR1(vehicleID, SessionInfo.UserSchema);
                            if (VehicleConfig.MovementClassificationId == 0)
                            {
                                VehicleConfig.MovementClassificationId = movementId;
                            }
                            vehicleID = (int)VehicleConfig.ConfigurationId;
                        }
                        else
                        {
                            VehicleConfig = vehicleconfigService.GetNotifVehicleConfigByID(vehicleID);
                            vehicleID = (int)VehicleConfig.ConfigurationId;
                        }
                        //VehicleConfig = vehicleconfigService.GetNotifVehicleConfigByID(vehicleID);
                        //vehicleID = (int)VehicleConfig.ConfigurationId;
                    }
                }
                else if (isMovement)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration , View Movement Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                    VehicleConfig = vehicleconfigService.GetMovementConfigInfo(vehicleID, SessionInfo.UserSchema);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration , View fleet Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                    //VehicleConfig = vehicleconfigService.GetConfigInfo(vehicleID, SessionInfo.UserSchema);

                    VehicleConfig = vehicleconfigService.GetVehicleDetails(vehicleID, false, SessionInfo.UserSchema);
                }
                ViewBag.TravelSpeed = VehicleConfig.TravellingSpeedUnit;

                //MovementClassificationConfig mvClassConfig = compConfigObj.GetMovementClassificationConfig(VehicleConfig.MovementClassificationId);
                //MovementClassificationConfig mvClassConfig = compConfigObj.GetMovementClassificationConfig(270001);

                //VehicleConfiguration vehicleConfigObj = mvClassConfig.GetVehicleConfiguration(VehicleConfig.ConfigurationTypeId);
                MovementClassificationConfig mvClassConfig = new MovementClassificationConfig();
                VehicleConfiguration vehicleConfigObj = compConfigObj.GetVehicleConfiguration(VehicleConfig.ConfigurationTypeId);
                if (vehicleConfigObj != null)
                {
                    vehicleConfigObj.UpdateConfigProperties(VehicleConfig);
                }

                ComponentModel VehicleComponentObj = null;
                List<VehicleConfigList> vehicleConfigList = null;
                if (isRoute)
                {
                    if (flag == "SOApp" || flag == "")
                    {
                        vehicleConfigList = vehicleconfigService.GetRouteVehicleConfigVhclID(vehicleID, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                        if (vehicleConfigList.Count == 1)
                            VehicleComponentObj = vehicleComponentService.GetRouteComponent((int)vehicleConfigList[0].ComponentId, SessionInfo.UserSchema);
                    }
                    else
                    {
                        vehicleConfigList = vehicleconfigService.GetVR1VehicleConfigVhclID(vehicleID, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                        if (vehicleConfigList.Count == 1)
                            VehicleComponentObj = vehicleComponentService.GetVR1VehicleComponent((int)vehicleConfigList[0].ComponentId, SessionInfo.UserSchema);
                    }
                }
                else if (isNotif)
                {
                    vehicleConfigList = vehicleconfigService.GetVR1VehicleConfigVhclID(vehicleID, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                    if (vehicleConfigList.Count == 1)
                        VehicleComponentObj = vehicleComponentService.GetVR1VehicleComponent((int)vehicleConfigList[0].ComponentId, SessionInfo.UserSchema);
                }
                else if (isMovement)
                {
                    vehicleConfigList = vehicleconfigService.GetMovementVehicleConfig(vehicleID, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                    if (vehicleConfigList.Count == 1)
                        VehicleComponentObj = vehicleComponentService.GetComponentTemp((int)vehicleConfigList[0].ComponentId, "", SessionInfo.UserSchema);
                }
                else
                {
                    vehicleConfigList = vehicleconfigService.GetVehicleConfigVhclID(vehicleID, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                    if (vehicleConfigList.Count == 1)
                        VehicleComponentObj = vehicleComponentService.GetVehicleComponent((int)vehicleConfigList[0].ComponentId);
                }
                ViewBag.IsRoute = isRoute;
                ViewBag.ComponentList = vehicleConfigList;

                if (isRoute)
                {
                    if (flag == "SOApp" || flag == "")
                    {
                        vehicleConfigObj.VehicleRegList = vehicleconfigService.GetRouteVehicleRegistrationDetails(vehicleID, SessionInfo.UserSchema);
                    }
                    else
                    {
                        vehicleConfigObj.VehicleRegList = vehicleconfigService.GetVR1VehicleRegistrationDetails(vehicleID, SessionInfo.UserSchema);
                    }
                }
                else if (isNotif)
                {
                    vehicleConfigObj.VehicleRegList = vehicleconfigService.GetVR1VehicleRegistrationDetails(vehicleID, SessionInfo.UserSchema);
                }
                else if (isMovement)
                {
                    vehicleConfigObj.VehicleRegList = vehicleconfigService.GetMovementVehicleRegDetails(vehicleID, SessionInfo.UserSchema);
                }
                else
                {
                    vehicleConfigObj.VehicleRegList = vehicleconfigService.GetVehicleRegistrationDetails(vehicleID, SessionInfo.UserSchema);
                }

                int MovementXmlTypeId = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleXmlMovementTypeMapping((VehicleXmlMovementType)(int)VehicleConfig.MovementClassificationId);

                if (ViewBag.ComponentList != null)
                {
                    List<VehicleComponent> VehicleComponentList = new List<VehicleComponent>();
                    foreach (var subType in ViewBag.ComponentList)
                    {
                        VehicleComponent vehclCompObj = GetVehicleComponent((int)subType.ComponentTypeId, (int)subType.ComponentSubTypeId, MovementXmlTypeId);
                        if (vehclCompObj != null)
                        {
                            ViewBag.IsConfigTyreCentreSpacing = vehclCompObj.IsConfigTyreCentreSpacing;
                            if (VehicleComponentObj != null && vehicleConfigList.Count == 1)
                            {
                                vehclCompObj.UpdateVehicleProperties(VehicleComponentObj);
                                vehicleConfigObj = ConvertComponentToConfig(vehclCompObj, vehicleConfigObj);
                            }

                            //To Fix Image Issue For Engg Plant
                            VehicleComponentList.Add(vehclCompObj);
                        }
                    }
                    ViewBag.VehicleComponentList = VehicleComponentList;
                }

                ViewBag.isImportConfiguration = isImportConfiguration;
                ViewBag.isNotif = isNotif;
                ViewBag.IsPolice = isPolice;
                ViewBag.ImportBtn = ImportBtn;
                ViewBag.vehicleID = vehicleID;

                ViewBag.isVR1 = isVR1;
                ViewBag.movementTypeId = movementId==0? MovementXmlTypeId : movementId;

                return PartialView("ViewVehicleConfiguration", vehicleConfigObj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration, Exception: {1}", Session.SessionID, ex.Message));
                throw;
            }
        }
        #endregion public ActionResult ViewConfiguration(int vehicleID, bool isRoute=false)

        #region public ActionResult ViewConfigDetails(int vehicleID, bool isRoute=false)
        /// <summary>
        /// Viewing Vehicle configuration
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public ActionResult ViewConfigDetails(int vehicleID, bool isRoute = false, int movementId = 0, bool isImportConfiguration = false, bool isNotif = false, string flag = "", bool isPolice = false, bool ImportBtn = false, bool isSort = false, bool IsNEN = false)
        {
            try
            {
                ViewBag.IsNEN = IsNEN;
                ViewBag.vehicleID = vehicleID;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/ViewConfigDetails , View Vehicle Configuration", Session.SessionID));
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                ViewBag.IsSORTRequest = false;
                if (isSort)
                {
                    SessionInfo.UserSchema = UserSchema.Sort;
                    ViewBag.IsSORTRequest = true;
                }
                ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
                ConfigurationModel VehicleConfig = null;

                bool isVR1 = false;
                if (isRoute)
                {
                    if (flag == "SOApp" || flag == "")
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfigDetails , View SO Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));

                        VehicleConfig = vehicleconfigService.GetRouteConfigInfo(vehicleID, SessionInfo.UserSchema);
                        if (VehicleConfig.MovementClassificationId == 0)
                        {
                            VehicleConfig.MovementClassificationId = movementId;
                        }
                    }
                    else
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfigDetails , View VR1 Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                        VehicleConfig = vehicleconfigService.GetRouteConfigInfoForVR1(vehicleID, SessionInfo.UserSchema);
                        if (VehicleConfig.MovementClassificationId == 0)
                        {
                            VehicleConfig.MovementClassificationId = movementId;
                        }
                        isVR1 = true;
                    }
                }
                else if (isNotif || IsNEN)
                {
                    if (flag == "Notif" || IsNEN)
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfigDetails , View Notif Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                        VehicleConfig = vehicleconfigService.GetRouteConfigInfoForVR1(vehicleID, SessionInfo.UserSchema);
                    }
                    else
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfigDetails , View Notif Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                        VehicleConfig = vehicleconfigService.GetNotifVehicleConfigByID(vehicleID);
                        vehicleID = (int)VehicleConfig.ConfigurationId;
                    }
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfigDetails , View fleet Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                    //VehicleConfig = vehicleconfigService.GetConfigInfo(vehicleID, SessionInfo.UserSchema);
                    VehicleConfig = vehicleconfigService.GetVehicleDetails(vehicleID, false, SessionInfo.UserSchema);
                }
                ViewBag.TravelSpeed = VehicleConfig.TravellingSpeedUnit;

               // MovementClassificationConfig mvClassConfig = compConfigObj.GetMovementClassificationConfig(VehicleConfig.MovementClassificationId);
                //VehicleConfiguration vehicleConfigObj = mvClassConfig.GetVehicleConfiguration(VehicleConfig.ConfigurationTypeId);

                MovementClassificationConfig mvClassConfig = new MovementClassificationConfig();
                VehicleConfiguration vehicleConfigObj = compConfigObj.GetVehicleConfiguration(VehicleConfig.ConfigurationTypeId);

                if (vehicleConfigObj != null)
                {
                    vehicleConfigObj.UpdateConfigProperties(VehicleConfig);
                }

                if (isRoute)
                {
                    if (flag == "SOApp" || flag == "")
                    {
                        ViewBag.ComponentList = vehicleconfigService.GetRouteVehicleConfigVhclID(vehicleID, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                    }
                    else
                    {
                        ViewBag.ComponentList = vehicleconfigService.GetVR1VehicleConfigVhclID(vehicleID, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                    }
                }
                else if (isNotif)
                {
                    ViewBag.ComponentList = vehicleconfigService.GetVR1VehicleConfigVhclID(vehicleID, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                }
                else
                {
                    ViewBag.ComponentList = vehicleconfigService.GetVehicleConfigVhclID(vehicleID, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                }

                ViewBag.IsRoute = isRoute;

                if (isRoute)
                {
                    if (flag == "SOApp" || flag == "")
                    {
                        vehicleConfigObj.VehicleRegList = vehicleconfigService.GetRouteVehicleRegistrationDetails(vehicleID, SessionInfo.UserSchema);
                    }
                    else
                    {
                        vehicleConfigObj.VehicleRegList = vehicleconfigService.GetVR1VehicleRegistrationDetails(vehicleID, SessionInfo.UserSchema);
                    }
                }
                else if (isNotif)
                {
                    vehicleConfigObj.VehicleRegList = vehicleconfigService.GetVR1VehicleRegistrationDetails(vehicleID, SessionInfo.UserSchema);
                }
                else
                {
                    vehicleConfigObj.VehicleRegList = vehicleconfigService.GetVehicleRegistrationDetails(vehicleID, SessionInfo.UserSchema);
                }
                ViewBag.isImportConfiguration = isImportConfiguration;
                ViewBag.isNotif = isNotif;
                ViewBag.IsPolice = isPolice;
                ViewBag.ImportBtn = ImportBtn;
                ViewBag.vehicleID = vehicleID;

                ViewBag.isVR1 = isVR1;
                ViewBag.movementType = VehicleConfig.MovementClassificationId;
                ViewBag.vehicleConfigType = VehicleConfig.ConfigurationTypeId;
                TempData.Keep("VehicleConfigModel");

                if (ViewBag.ComponentList != null)
                {
                    int MovementXmlTypeId = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleXmlMovementTypeMapping((VehicleXmlMovementType)(int)VehicleConfig.MovementClassificationId);
                    List<VehicleComponent> VehicleComponentList = new List<VehicleComponent>();
                    foreach (var subType in ViewBag.ComponentList)
                    {
                        VehicleComponent vehclCompObj = GetVehicleComponent((int)subType.ComponentTypeId, (int)subType.ComponentSubTypeId, MovementXmlTypeId);
                        if (vehclCompObj != null)
                        {
                            //To Fix Image Issue For Engg Plant
                            VehicleComponentList.Add(vehclCompObj);
                        }
                    }

                    ViewBag.VehicleComponentList = VehicleComponentList;
                }

                return View(vehicleConfigObj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] ,VehicleConfigController/ViewConfigDetails, Exception: {1}", Session.SessionID, ex.Message));
                throw;
            }
        }
        #endregion public ActionResult ViewConfigDetails(int vehicleID, bool isRoute=false)

        #region public JsonResult GetComponentFavourites
        [HttpPost]
        public JsonResult OldGetComponentFavourites()
        {
            List<ComponentGridList> componentIdList = new List<ComponentGridList>();
            try
            {
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                int organisationId = (int)SessionInfo.OrganisationId;
                int movementId = Session["movementClassificationId"] != null ? (int)Session["movementClassificationId"] : 0;
                componentIdList = vehicleComponentService.GetComponentFavourite(organisationId, movementId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] , VehicleConfigController/DeleteConfiguration, Exception: {1}", Session.SessionID, ex.Message));
                throw;
            }
            return Json(componentIdList);
        }
        #endregion

        #region public JsonResult AddConfigToFleet(int VehicleId,int flag)
        public JsonResult AddConfigToFleet(int VehicleId, int flag)
        {
            UserInfo SessionInfo = null;

            SessionInfo = (UserInfo)Session["UserInfo"];

            int organisationId;
            if (SessionInfo.UserTypeId == 696008)
            {
                organisationId = (int)Session["SORTOrgID"];
            }
            else
            {
                organisationId = (int)SessionInfo.OrganisationId;
            }

            int result = 0;
            result = vehicleconfigService.AddMovementVehicleToFleet(VehicleId, organisationId, flag, SessionInfo.UserSchema);


            #region System events for sort_added_new_vehicle_to_fleet_for SO and VR1
            if (result != 0)
            {

                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                movactiontype.VehicleId = VehicleId;
                movactiontype.UserName = SessionInfo.UserName;
                string ErrMsg = string.Empty;
                int user_ID = Convert.ToInt32(SessionInfo.UserId);

                if (SessionInfo.UserSchema == UserSchema.Sort) // For SORT Vehicle delete Log
                {
                    #region Saving sort_added_new_vehicle_to_fleet_for_so_application
                    movactiontype.SystemEventType = SysEventType.sort_added_new_vehicle_to_fleet_for_so_application;
                    #endregion
                }
                else if (SessionInfo.UserSchema == UserSchema.Portal)
                {
                    #region Saving sort_added_new_vehicle_to_fleet_for_so_application
                    movactiontype.SystemEventType = SysEventType.Haulier_added_vehicle_to_fleet;
                    #endregion

                }
            }
            #endregion

            return Json(new { success = result });
        }
        #endregion public JsonResult AddConfigToFleet(int VehicleId,int flag)

        #region public JsonResult DeleteVehicleRegistration(int configId, int IdNumber)
        /// <summary>
        /// JsonResult method to delete Registration
        /// </summary>
        /// <param name="configId"></param>
        /// <param name="IdNumber"></param>
        /// <returns></returns>
        //[HttpPost]
        public JsonResult DeleteVehicleRegistration(int configId, int IdNumber, bool isMovement = false, bool isCandidate = false)
        {
            try
            {
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , JsonResult,VehicleConfigController/DeleteVehicleRegistration , Delete vehicle config registration details for id - {1}", Session.SessionID, configId));
                if (isCandidate)
                {
                    vehicleconfigService.DeleteVR1RegConfig(configId, IdNumber, SessionInfo.UserSchema);
                }
                else if (isMovement)
                {
                    vehicleconfigService.DeletVehicleRegisterConfiguration(configId, IdNumber, isMovement);
                }
                else
                {
                    vehicleconfigService.DeletVehicleRegisterConfiguration(configId, IdNumber);
                }
                #region System Event Log - haulier_deleted_registration_for_fleet_vehicle
                string ErrMsg = string.Empty;
                string sysEventDescp = string.Empty;

                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                movactiontype.UserName = SessionInfo.UserName;
                movactiontype.FleetVehicleId = configId;
                movactiontype.FleetVehicleIdNo = IdNumber;
                movactiontype.SystemEventType = SysEventType.haulier_deleted_registration_for_fleet_vehicle;

                sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                #endregion

                if (Session["IsVehicleAmended"] == null)
                    Session["IsVehicleAmended"] = true;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] , VehicleConfigController/DeleteVehicleRegistration, Exception: {1}", Session.SessionID, ex.Message));
                throw ex;
            }


            return Json(new { Success = true });
        }
        #endregion public JsonResult DeleteVehicleRegistration(int configId, int IdNumber)

        #endregion

        #region Private Methods


        #region private void MovementClsDropDown()
        private void MovementClsDropDown(int vhclClassification, bool isApplicationVehicle = false)
        {

            List<MovementClassification> dropDownList = null;
            ComponentConfiguration vehicleParams = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
            dropDownList = vehicleParams.GetMovementClassification();
            if (isApplicationVehicle == true)
                ViewBag.MovementClassConfig = new SelectList(dropDownList, "ClassificationId", "ClassificationName", 270006);
            else
                ViewBag.MovementClassConfig = new SelectList(dropDownList, "ClassificationId", "ClassificationName");

            List<VehicleConfigurationType> configdrpdwn = null;
            configdrpdwn = vehicleParams.GetConfigType();
            ViewBag.VehicleTypeConfig = new SelectList(configdrpdwn, "ConfigurationTypeId", "ConfigurationName");
        }
        #endregion private void MovementClsDropDown()

        #region private NewConfigurationModel ConvertToConfiguration(VehicleConfiguration vehicleConfigObj)
        /// <summary>
        /// Method to convert the list of IFX propery to componentModel objects
        /// </summary>
        /// <param name="vehicleConfigObj"></param>
        /// <returns></returns>
        private NewConfigurationModel ConvertToConfiguration(VehicleConfiguration vehicleConfigObj)
        {
            int unit = 1;
            bool isRigid = false;
            if (vehicleConfigObj.vehicleConfigType!=null && vehicleConfigObj.vehicleConfigType.ConfigurationTypeId == (int)ConfigurationType.Rigid)
                isRigid = true;
            NewConfigurationModel configModelObj = new NewConfigurationModel();
            foreach (var ifxProperty in vehicleConfigObj.VehicleConfigParamList)
            {
                switch (ifxProperty.ParamModel.ToLower())
                {
                    case "formal_name":
                        configModelObj.VehicleIntDesc = ifxProperty.ParamValue;
                        break;
                    case "internal_name":
                    case "internal name":
                        configModelObj.VehicleName = ifxProperty.ParamValue;
                        break;
                    case "notes":
                        configModelObj.VehicleDesc = ifxProperty.ParamValue;
                        break;
                    case "length":
                        configModelObj.RigidLength = Convert.ToDouble(ifxProperty.ParamValue);
                        configModelObj.RigidLengthUnit = unit;
                        if (isRigid)
                        {
                            configModelObj.Length = Convert.ToDouble(ifxProperty.ParamValue);
                        }
                        break;
                    case "weight":
                        configModelObj.GrossWeight = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "overalllength":
                        configModelObj.Length = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "width":
                        configModelObj.Width = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "wheelbase":
                        if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                            configModelObj.WheelBase = Convert.ToDouble(ifxProperty.ParamValue);
                        configModelObj.WheelBaseUnit = unit;
                        break;
                    case "maximum_height":
                    case "maximum height":
                        if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                            configModelObj.MaxHeight = Convert.ToDouble(ifxProperty.ParamValue);
                        configModelObj.MaxHeightUnit = unit;
                        break;
                    case "axleweight":
                        if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                            configModelObj.MaxAxleWeight = Convert.ToDouble(ifxProperty.ParamValue);
                        configModelObj.MaxAxleWeightUnit = unit;
                        break;
                    case "speed":
                        configModelObj.Speed = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "tyre_spacing":
                        configModelObj.TyreSpacing = Convert.ToDouble(ifxProperty.ParamValue);
                        configModelObj.TyreSpacingUnit = unit;
                        break;
                    case "number of axles":
                        configModelObj.TractorAxleCount = Convert.ToInt32(ifxProperty.ParamValue);
                        break;
                    case "number of axles for trailer":
                        configModelObj.TrailerAxleCount = Convert.ToInt32(ifxProperty.ParamValue);
                        break;
                    case "reducable_height":
                    case "reducable height":
                        if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                            configModelObj.RedHeightMtr= Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    default:
                        break;
                }
            }
            return configModelObj;
        }
        #endregion

        #region private static List<string> GetVehicleIntend()
        private static List<string> GetVehicleIntend(string intend, int userType)
        {
            List<string> listVhclType = new List<string>();
            if (userType == UserType.Sort)
            {
                listVhclType.Add("STGO AIL");
                listVhclType.Add("Special order");
            }
            else
            {
                listVhclType.Add("Construction and use");
                listVhclType.Add("STGO AIL");
                listVhclType.Add("STGO Mobile crane");
                listVhclType.Add("STGO Engineering plant(not tracked)");
                listVhclType.Add("STGO Road recovery operation");
                listVhclType.Add("Special order");
                listVhclType.Add("Vehicle special order");
                listVhclType.Add("Tracked");
            }

            return listVhclType;
        }
        #endregion

        #region private static List<string> GetAllVehicleType()
        private static List<DropDown> GetAllVehicleType()
        {
            List<DropDown> listVhclType = new List<DropDown>();
            DropDown objDropDown = null;
            objDropDown = new DropDown()
            {
                Id = 244002,
                Value = "Semi trailer"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 244001,
                Value = "Drawbar trailer"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 244003,
                Value = "Rigid"
            };
            listVhclType.Add(objDropDown);


            objDropDown = new DropDown()
            {
                Id = 244005,
                Value = "SPMT"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 244006,
                Value = "Other inline"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 244007,
                Value = "Side by side"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 244004,
                Value = "Tracked"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 244012,
                Value = "Crane"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 244011,
                Value = "Rigid and Drag"
            };
            listVhclType.Add(objDropDown);

            return listVhclType;
        }
        #endregion

        #region private static List<string> GetSpeedUnits()
        private static List<DropDown> GetSpeedUnits()
        {
            List<DropDown> listSpeedUnit = new List<DropDown>();
            DropDown objDropDown = null;
            objDropDown = new DropDown()
            {
                Id = 229001,
                Value = "mph"
            };
            listSpeedUnit.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 229002,
                Value = "kph"
            };
            listSpeedUnit.Add(objDropDown);

            return listSpeedUnit;
        }
        #endregion

        #region Workflow Methods
        private void ProcessWorkflowActivity(List<VehicleComponentsModel> vehicleComponentsModels, bool decideNextTask, bool moveToEnterVehicleDetails = false, bool isImportFromFleet = false, bool isEditConfiguration = false)
        {
            FleetManagement fleetManagement = new FleetManagement(fleetManagementWorkflowService);
            fleetManagement.ProcessWorkflowActivity(null, vehicleComponentsModels, WorkflowFleetMgmtFlowTypes.VehicleConfig, Session.SessionID, false, decideNextTask, isImportFromFleet, moveToEnterVehicleDetails, isEditConfiguration);
        }
        private void TerminateWorkflowProcess(string applicationId)
        {
            FleetManagement fleetManagement = new FleetManagement(fleetManagementWorkflowService);
            fleetManagement.TerminateProcess(applicationId);

        }
        //private void StartEditFleetManagementWorkflow(string organizationName, bool isEditConfiguration)
        //{
        //    var vehicleName = new SessionData().Wf_Fm_VehicleConfigurationId ?? string.Empty;
        //    if ((new SessionData().Wf_Fm_CurrentExecuted == WorkflowActivityTypes.Gn_NotDecided && isEditConfiguration))
        //    {
        //        FleetManagement fleetManagement = new FleetManagement(fleetManagementWorkflowService);
        //        new SessionData().Wf_Fm_FleetManagementId = fleetManagement.StartWorkflow(false, organizationName, vehicleName);
        //    }


        //}

        private List<VehicleComponentsModel> GetPayloadComponents()
        {
            FleetManagement fleetManagement = new FleetManagement(fleetManagementWorkflowService);
            var vehicleWorkFlowParams = new VehicleWorkFlowParams();
            List<VehicleComponentsModel> vehicleComponentsModels = new List<VehicleComponentsModel>();
            var payloadData = fleetManagement.SearchPayloadItem(nameof(vehicleWorkFlowParams.VehicleComponentsModels));
            if (payloadData != null)
            {
                foreach (WorkflowVariableModel workflowVariableModel in payloadData)
                {
                    var tempVechicleComponentModel = JsonConvert.DeserializeObject<List<VehicleComponentsModel>>(workflowVariableModel.value);
                    vehicleComponentsModels.AddRange(tempVechicleComponentModel);

                }
            }
            return vehicleComponentsModels;
        }
        #endregion Workflow Methods

        #endregion


        #region private VehicleComponent GetVehicleComponent(int vehicleSubTypeId, int vehicleTypeId, int movementId)
        /// <summary>
        /// Finds the VehicleComponent Object with validated values from cache
        /// </summary>
        /// <param name="vehicleSubTypeId"></param>
        /// <param name="vehicleTypeId"></param>
        /// <param name="movementId"></param>
        /// <returns>VehicleComponent</returns>
        private VehicleComponent OldGetVehicleComponent(int vehicleSubTypeId, int vehicleTypeId)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/GetVehicleComponent method started successfully, with parameters vehicleSubTypeId:{0}, vehicleTypeId:{1}", vehicleSubTypeId, vehicleTypeId));
                ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
                //MovementClassificationConfig moveClassConfigObj = compConfigObj.GetMovementClassificationConfig(movementId);
                //VehicleComponent vehclCompObj = moveClassConfigObj.GetVehicleComponent(vehicleTypeId, vehicleSubTypeId);

                MovementClassificationConfig moveClassConfigObj = compConfigObj.GetListOfVehicleComponents(vehicleTypeId);
                VehicleComponent vehclCompObj = moveClassConfigObj.GetVehicleComponent(vehicleTypeId, vehicleSubTypeId);

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/GetVehicleComponent method completed successfully"));
                return vehclCompObj;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Vehicle/GetVehicleComponent, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion private VehicleComponent GetVehicleComponent(int vehicleSubTypeId, int vehicleTypeId, int movementId)

        private void OldGetVehicleImage(List<MovementVehicleList> movementVehicleList)
        {
            foreach (MovementVehicleList route in movementVehicleList)
            {
                foreach (VehicleDetails vehicle in route.VehicleList)
                {
                    ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
                    MovementClassificationConfig mvClassConfig = compConfigObj.GetMovementClassificationConfig(vehicle.VehiclePurpose);
                    //VehicleConfiguration vehicleConfigObj = mvClassConfig.GetVehicleConfiguration(vehicle.VehicleType);

                    foreach (var vehComp in vehicle.VehicleCompList)
                    {
                        MovementClassificationConfig moveClassConfigObj = compConfigObj.GetListOfVehicleComponents((int)vehComp.ComponentTypeId);
                        VehicleComponent vehclCompObj = moveClassConfigObj.GetVehicleComponent((int)vehComp.ComponentTypeId, (int)vehComp.ComponentSubTypeId);
                        vehicle.VehicleNameList.Add(vehclCompObj.vehicleComponentSubType.ImageName);
                    }

                    //foreach (VehicleConfigList component in vehicle.VehicleCompList)
                    //{
                    //    foreach (VehicleComponentType comType in vehicleConfigObj.vehicleConfigType.LstVehcCompTypes)
                    //    {
                    //        if (comType.ComponentTypeId == component.ComponentTypeId)
                    //        {
                    //            vehicle.VehicleNameList.Add(comType.ImageName);
                    //        }
                    //    }
                    //}
                }
            }
        }
        public void OldGetVehicleImage(dynamic movementVehicleList)
        {
            foreach (var vehicle in movementVehicleList)
            {
                ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
                MovementClassificationConfig mvClassConfig = compConfigObj.GetMovementClassificationConfig(vehicle.VehiclePurpose);
                //VehicleConfiguration vehicleConfigObj = mvClassConfig.GetVehicleConfiguration(vehicle.VehicleType);
                foreach (var vehComp in vehicle.VehicleCompList)
                {
                    MovementClassificationConfig moveClassConfigObj = compConfigObj.GetListOfVehicleComponents((int)vehComp.ComponentTypeId);
                    VehicleComponent vehclCompObj = moveClassConfigObj.GetVehicleComponent((int)vehComp.ComponentTypeId, (int)vehComp.ComponentSubTypeId);
                    vehicle.VehicleNameList.Add(vehclCompObj.vehicleComponentSubType.ImageName);
                }
                //foreach (VehicleConfigList component in vehicle.VehicleCompList)
                //{
                //    foreach (VehicleComponentType comType in vehicleConfigObj.vehicleConfigType.LstVehcCompTypes)
                //    {
                //        if (comType.ComponentTypeId == component.ComponentTypeId)
                //        {
                //            vehicle.VehicleNameList.Add(comType.ImageName);
                //        }
                //    }
                //}
            }
        }
        private void AssignMovementClassification(int movementTypeId)
        {
            Session["movementClassificationId"] = movementTypeId;
            List<MovementClassification> mvmntdrpdwn = null;
            ComponentConfiguration vehicleParams = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
            mvmntdrpdwn = vehicleParams.GetMovementClassification();
            movementTypeId = Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.GetVehicleIntentedUse((VehicleXmlMovementType)movementTypeId);
            string mvmntName = mvmntdrpdwn.Where(x => x.ClassificationId == movementTypeId).Select(x => x.ClassificationName).FirstOrDefault().ToString();
            Session["movementClassificationName"] = mvmntName;
        }
        public ActionResult ApplicationVehicles(List<VehicleDetails> VehicleList)
        {
            OldGetVehicleImage(VehicleList);
            ViewBag.VehicleList = VehicleList;
            return PartialView("_ApplicationVehicles");
        }

        #region public JsonResult CheckConfigFormalName(string VehicleName)
        [HttpPost]
        public JsonResult CheckConfigFormalName(string VehicleName)
        {
            UserInfo SessionInfo = null;

            SessionInfo = (UserInfo)Session["UserInfo"];
            //int organisationId = (int)SessionInfo.organisationId;
            int organisationId;
            if (SessionInfo.UserTypeId == 696008)
            {
                organisationId = (int)Session["SORTOrgID"];
            }
            else
            {
                organisationId = (int)SessionInfo.OrganisationId;
            }

            int result = 0;

            //result = ApplicationProvider.Instance.CheckFormalNameInApplicationVeh(VehicleName, organisationId, SessionInfo.userSchema);
            result = vehicleconfigService.CheckFormalNameInApplicationVeh(VehicleName, organisationId, SessionInfo.UserSchema);

            return Json(new { success = result });
        }
        #endregion public JsonResult CheckConfigFormalName(string VehicleName)

        public ActionResult ViewNENConfiguration(int vehicleID, bool isRoute = false, int movementId = 0, bool isImportConfiguration = false, bool isNotif = false, string flag = "", bool isPolice = false, bool ImportBtn = false, bool isSort = false, bool IsNEN = false, bool importFlag = false)
        {
            try
            {
                ViewBag.IsNEN = IsNEN;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/ViewConfiguration , View Vehicle Configuration", Session.SessionID));
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                if (isSort)
                {
                    SessionInfo.UserSchema = UserSchema.Sort;
                }
                ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
                ConfigurationModel VehicleConfig = null;

                bool isVR1 = false;
                if (isRoute)
                {
                    if (flag == "SOApp" || flag == "")
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration , View SO Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));

                        VehicleConfig = vehicleconfigService.GetRouteConfigInfo(vehicleID, SessionInfo.UserSchema);
                        if (VehicleConfig.MovementClassificationId == 0)
                        {
                            VehicleConfig.MovementClassificationId = movementId;
                        }
                    }
                    else
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration , View VR1 Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                        VehicleConfig = vehicleconfigService.GetRouteConfigInfoForVR1(vehicleID, SessionInfo.UserSchema);
                        if (VehicleConfig.MovementClassificationId == 0)
                        {
                            VehicleConfig.MovementClassificationId = movementId;
                        }
                        isVR1 = true;
                    }
                }
                else if (isNotif || IsNEN)
                {
                    if (flag == "Notif" || IsNEN)
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration , View Notif Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                        VehicleConfig = vehicleconfigService.GetRouteConfigInfoForVR1(vehicleID, SessionInfo.UserSchema);
                    }
                    else
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration , View Notif Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                        VehicleConfig = vehicleconfigService.GetNotifVehicleConfigByID(vehicleID);
                        vehicleID = (int)VehicleConfig.ConfigurationId;
                    }
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration , View fleet Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                    //VehicleConfig = vehicleconfigService.GetConfigInfo(vehicleID, SessionInfo.UserSchema);
                    VehicleConfig = vehicleconfigService.GetVehicleDetails(vehicleID, false, UserSchema.Portal);
                }
                ViewBag.TravelSpeed = VehicleConfig.TravellingSpeedUnit;

                //MovementClassificationConfig mvClassConfig = compConfigObj.GetMovementClassificationConfig(VehicleConfig.MovementClassificationId);

                //VehicleConfiguration vehicleConfigObj = mvClassConfig.GetVehicleConfiguration(VehicleConfig.ConfigurationTypeId);
                MovementClassificationConfig mvClassConfig = new MovementClassificationConfig();
                VehicleConfiguration vehicleConfigObj = compConfigObj.GetVehicleConfiguration(VehicleConfig.ConfigurationTypeId);

                if (vehicleConfigObj != null)
                {
                    vehicleConfigObj.UpdateConfigProperties(VehicleConfig);
                }

                if (isRoute)
                {
                    if (flag == "SOApp" || flag == "")
                    {
                        ViewBag.ComponentList = vehicleconfigService.GetRouteVehicleConfigVhclID(vehicleID, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                    }
                    else
                    {
                        ViewBag.ComponentList = vehicleconfigService.GetVR1VehicleConfigVhclID(vehicleID, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                    }
                }
                else if (isNotif)
                {
                    ViewBag.ComponentList = vehicleconfigService.GetVR1VehicleConfigVhclID(vehicleID, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                }
                else
                {
                    ViewBag.ComponentList = vehicleconfigService.GetVehicleConfigVhclID(vehicleID, UserSchema.Portal).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                }
                ViewBag.IsRoute = isRoute;

                if (isRoute)
                {
                    if (flag == "SOApp" || flag == "")
                    {
                        vehicleConfigObj.VehicleRegList = vehicleconfigService.GetRouteVehicleRegistrationDetails(vehicleID, SessionInfo.UserSchema);
                    }
                    else
                    {
                        vehicleConfigObj.VehicleRegList = vehicleconfigService.GetVR1VehicleRegistrationDetails(vehicleID, SessionInfo.UserSchema);
                    }
                }
                else if (isNotif)
                {
                    vehicleConfigObj.VehicleRegList = vehicleconfigService.GetVR1VehicleRegistrationDetails(vehicleID, SessionInfo.UserSchema);
                }
                else
                {
                    vehicleConfigObj.VehicleRegList = vehicleconfigService.GetVehicleRegistrationDetails(vehicleID, UserSchema.Portal);
                }
                int MovementXmlTypeId = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleXmlMovementTypeMapping((VehicleXmlMovementType)(int)VehicleConfig.MovementClassificationId);

                ViewBag.isImportConfiguration = isImportConfiguration;
                ViewBag.isNotif = isNotif;
                ViewBag.IsPolice = isPolice;
                ViewBag.ImportBtn = ImportBtn;
                ViewBag.vehicleID = vehicleID;

                ViewBag.isVR1 = isVR1;
                ViewBag.importFlag = importFlag;
                ViewBag.movementTypeId = movementId == 0 ? MovementXmlTypeId : movementId;

                return View(vehicleConfigObj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration, Exception: {1}", Session.SessionID, ex.Message));
                throw;
            }
        }

        [HttpPost]
        public JsonResult AmendVehicleConfiguration(AmendRegistarion amendRegistration)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/AmendVehicleConfiguration , Update Vehicle Configuration", Session.SessionID));
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                if (amendRegistration != null && amendRegistration.VehicleId > 0)
                {
                    //handle insert
                    if (amendRegistration.RegistrationDetails != null)
                    {
                        var amendItemsToBeInserted = amendRegistration.RegistrationDetails.Where(x => x.Id == 0).ToList();
                        if (amendItemsToBeInserted != null && amendItemsToBeInserted.Any())
                        {
                            if(Session["IsVehicleAmended"]==null)
                                Session["IsVehicleAmended"]= true;
                            foreach (var registration in amendItemsToBeInserted)
                            {
                                int i=vehicleconfigService.CreateVehicleRegistrationTemp(amendRegistration.VehicleId, registration.RegId, registration.FleetId, SessionInfo.UserSchema);
                            }
                        }
                    }
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, error = "Please enter valid vehicle details" });
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] ,VehicleConfigController/ViewConfiguration, Exception: {1}", Session.SessionID, ex.Message));
                throw;
            }
        }

        #endregion

        #region Create Vehicle New Work Flow

        public ActionResult CreateVehicle(int vhclClassification = 0, bool isApplicationVehicle = false, bool isVR1 = false, bool IsNotifVeh = false, string Guid = "", int isEdit = 0, bool isMovement = false, int vehicleConfigId = 0, List<VehicleConfigList> componentIdLists = null, bool isCandidate = false)
        {
            #region application vehicle
            if (isApplicationVehicle && !isVR1)
                ViewBag.newApplicationVehicle = true;
            if (isVR1)
                ViewBag.VR1appln = true;
            #endregion
            ViewBag.IsNotifVeh = IsNotifVeh;

            ViewBag.IsEdit = isEdit;
            ViewBag.IsMovement = isMovement;
            ViewBag.Guid = Guid;
            ViewBag.vehicleConfigId = vehicleConfigId;
            ViewBag.isCandidate = isCandidate;
            TempData["VehicleConfigModel"] = null;

            if (Guid == "" && vehicleConfigId == 0 && !isMovement)
            {
                Session.Remove("movementClassificationId");
                Session.Remove("movementClassificationName");
                Session["g_VehicleComponentSearch"] = "";
                Session["g_VehicleTypeSearch"] = "";
                Session["g_VehicleIntendSearch"] = "";
            }
            if (Session["VSOVehicleClassificationType"] != null && (int)Session["VSOVehicleClassificationType"] == (int)VehicleClassificationType.VehicleSpecialOrder)
            {
                VehicleMovementType MovementType = new VehicleMovementType
                {
                    MovementType = (int)Common.Enums.MovementType.notification,
                    SOANoticePeriod = 2,
                    PoliceNoticePeriod = 2,
                    VehicleClass = (int)VehicleClassificationType.VehicleSpecialOrder,
                    Message = "Vehicle Special Order."
                };
                new SessionData().E4_AN_MovemenTypeClass = MovementType;
            }
            if (isEdit == 1)
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/CreateConfiguration , Load partial view of CreateConfiguration", Session.SessionID));
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                List<VehicleConfigList> componentIdList = new List<VehicleConfigList>();
                if (vehicleConfigId != 0)
                {
                    componentIdList = componentIdLists;
                    if (isMovement && !isCandidate)
                    {
                        componentIdList = vehicleComponentService.GetComponentIdTemp(Guid, vehicleConfigId, SessionInfo.UserSchema).OrderBy(s => s.ComponentId).ToList();
                    }
                    else if (isCandidate)
                    {
                        componentIdList = vehicleconfigService.GetVR1VehicleConfigVhclID(vehicleConfigId, SessionInfo.UserSchema).OrderBy(s => s.ComponentId).ToList();
                    }
                    else
                    {
                        componentIdList = vehicleconfigService.GetVehicleConfigVhclID(vehicleConfigId, SessionInfo.UserSchema).OrderBy(s => s.ComponentId).ToList();
                    }

                }
                else
                {
                    componentIdList = vehicleComponentService.GetComponentIdTemp(Guid, vehicleConfigId, SessionInfo.UserSchema).OrderBy(s => s.ComponentId).ToList();
                }

                if (componentIdList.Count == 0)
                {
                    Session["ComponentIdDeleted"] = 1;
                }
                if (Guid == "")
                {
                    Session.Remove("ComponentId");
                    Session.Remove("ComponentIdDeleted");
                    if (new SessionData().Wf_Fm_FleetManagementId.Length > 1 && new SessionData().Wf_Fm_FleetManagementId != WorkflowActivityConstants.Gn_Failed)
                    {
                        TerminateWorkflowProcess(new SessionData().Wf_Fm_FleetManagementId);

                    }
                    new SessionData().Wf_Fm_VehicleConfigurationId = string.Empty;
                    new SessionData().Wf_Fm_CurrentExecuted = WorkflowActivityTypes.Gn_NotDecided;
                    new SessionData().Wf_Fm_FleetManagementId = string.Empty;
                }
                if (componentIdList.Count > 0)
                {
                    Session["ComponentId"] = componentIdList;
                }
                return View("~/Views/VehicleConfiguration/CreateVehicle.cshtml", componentIdList);
            }
            else
            {
                return View("~/Views/VehicleConfiguration/CreateVehicle.cshtml");
            }
        }

        [HttpPost]
        public JsonResult GetComponentFavourites(int OrganisationId=0)
        {
            List<ComponentGridList> componentIdList = new List<ComponentGridList>();
            try
            {
                if (OrganisationId == 0)
                {
                    UserInfo SessionInfo = null;
                    SessionInfo = (UserInfo)Session["UserInfo"];
                    OrganisationId = (int)SessionInfo.OrganisationId;
                }
                int movementId = Session["movementClassificationId"] != null ? (int)Session["movementClassificationId"] : 0;
                componentIdList = vehicleComponentService.GetComponentFavourite(OrganisationId, movementId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] , VehicleConfigController/DeleteConfiguration, Exception: {1}", Session.SessionID, ex.Message));
                throw;
            }
            return Json(componentIdList);
        }

        public ActionResult SelectVehicleComponent(bool isCandidate = false)
        {
            ViewBag.IsCandidate = isCandidate;
            return PartialView("~/Views/VehicleConfiguration/_ComponentSelection.cshtml");
        }

        [HttpGet]
        public ActionResult CreateComponent(int vehicleConfigId = 0, bool isMovement = false, string guid = "", bool isCandidate = false, int flag = 0, int previousComponentTypeId = 0, int previousComponentSubTypeId = 0, int componentCount = 0,bool isAddCompBtnClick=false,bool isDeleteButtonClicked=false,int isImportComponent=0, int firstComponetTypeId= 0)
        {
            int portalType = 0;
            bool isAdmin = false;

            UserInfo sessionValues = null;
            if (Session["UserInfo"] != null)
            {
                sessionValues = (UserInfo)Session["UserInfo"];
                portalType = sessionValues.UserTypeId;
                if (portalType == 696006 || sessionValues.IsAdmin == 1)
                {
                    isAdmin = true;
                }
            }
            ViewBag.IsAdmin = isAdmin;
            ViewBag.IsPrevTypeRecoveryVehicle = previousComponentTypeId == (int)VehicleEnums.ComponentType.RecoveryVehicle;

            var isRecoveryVehicle = firstComponetTypeId == (int)VehicleEnums.ComponentType.RecoveryVehicle;//For Default Component 
            ViewBag.IsRecoveryVehicle = isRecoveryVehicle;
            double componentId = 0;
            int organisationId = 0;
            var componentTypeId = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.DefaultComponentTypeForMoreThanTwo(previousComponentTypeId, previousComponentSubTypeId);
            var componentSubTypeId = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.DefaultComponentSubType(componentTypeId);
            if (isAddCompBtnClick && componentTypeId>0 && componentSubTypeId > 0)
            {
                ComponentModel componentObj = InsertComponent(vehicleConfigId, componentTypeId, componentSubTypeId, isMovement, ref organisationId, ref guid, sessionValues, ref componentId);
                flag = 1;
            }
            else if (componentTypeId == 0 && previousComponentTypeId == (int)VehicleEnums.ComponentType.RecoveryVehicle)
            {
                previousComponentTypeId = 0;
                previousComponentSubTypeId = 0;
            }

            List<VehicleConfigList> componentIdList = new List<VehicleConfigList>();
            if (vehicleConfigId == 0 || ((vehicleConfigId != 0 && isMovement) && !isCandidate))
            {
                componentIdList = vehicleComponentService.GetComponentIdTemp(guid, vehicleConfigId, sessionValues.UserSchema);
                if (componentIdList != null)
                    componentIdList = componentIdList.Where(x => x.ComponentTypeId != 0).ToList();
                componentCount = componentIdList.Count();
            }
            var componentIdOrderList = componentIdList.OrderBy(s => s.ComponentId).ToList();
            var imagename = new List<string>();

            foreach (var component in componentIdList)
            {
                GetComponentImage((int)component.ComponentTypeId, (int)component.ComponentSubTypeId).ToString();
                imagename.Add(TempData["CompImage"].ToString());
            }

            ViewBag.ComponentTypeList = componentIdOrderList;
            ViewBag.Guid = guid;
            ViewBag.VehicleConfigId = vehicleConfigId;
            ViewBag.IsMovement = isMovement;
            ViewBag.IsCandidate = isCandidate;
            ViewBag.VehicleImage = imagename;
            ViewBag.Flag = flag;
            ViewBag.ComponentCount = componentCount;
            ViewBag.PreviousComponentTypeId = previousComponentTypeId;
            ViewBag.PreviousComponentSubTypeId = previousComponentSubTypeId;
            ViewBag.isImportComponent = isImportComponent;
            return PartialView("~/Views/VehicleConfiguration/CreateComponent.cshtml");
        }

        [HttpGet]
        public ActionResult GetComponent(int componentId = 0, int componentTypeId = 0, int componentSubTypeId = 0,int TotalComponentCount=0, int? CurrentCount=null, int isImportComponent=0, bool isRecoveryVehicle = false, bool isRigidVehicle = false)
        {
            UserInfo sessionValues = null;
            if (Session["UserInfo"] != null)
            {
                sessionValues = (UserInfo)Session["UserInfo"];
            }
            
            VehicleComponentDropDown(sessionValues.UserTypeId, componentTypeId);
            if (TotalComponentCount == 0 || (CurrentCount !=null && CurrentCount <= 1))
            {
                var componentTypeList = (SelectList)ViewBag.ComponentType;
                if (componentTypeList != null)
                {
                    if (isImportComponent != 1)
                    {
                        ViewBag.ComponentType = componentTypeList.Where(x => Convert.ToInt32(x.Value) == (int)VehicleEnums.ComponentType.BallastTractor ||
                                                                Convert.ToInt32(x.Value) == (int)VehicleEnums.ComponentType.ConventionalTractor ||
                                                                Convert.ToInt32(x.Value) == (int)VehicleEnums.ComponentType.RigidVehicle ||
                                                                Convert.ToInt32(x.Value) == (int)VehicleEnums.ComponentType.SPMT ||
                                                                Convert.ToInt32(x.Value) == (int)VehicleEnums.ComponentType.EngineeringPlant ||
                                                                Convert.ToInt32(x.Value) == (int)VehicleEnums.ComponentType.Tracked ||
                                                                Convert.ToInt32(x.Value) == (int)VehicleEnums.ComponentType.MobileCrane ||
                                                                Convert.ToInt32(x.Value) == (int)VehicleEnums.ComponentType.RecoveryVehicle
                                                            ).ToList();
                    }
                    else
                    {
                        ViewBag.ComponentType = componentTypeList.Where(x => Convert.ToInt32(x.Value) == (int)VehicleEnums.ComponentType.BallastTractor ||
                                                                Convert.ToInt32(x.Value) == (int)VehicleEnums.ComponentType.ConventionalTractor ||
                                                                Convert.ToInt32(x.Value) == (int)VehicleEnums.ComponentType.RigidVehicle ||
                                                                Convert.ToInt32(x.Value) == (int)VehicleEnums.ComponentType.SPMT ||
                                                                Convert.ToInt32(x.Value) == (int)VehicleEnums.ComponentType.EngineeringPlant ||
                                                                Convert.ToInt32(x.Value) == (int)VehicleEnums.ComponentType.Tracked ||
                                                                Convert.ToInt32(x.Value) == (int)VehicleEnums.ComponentType.MobileCrane ||
                                                                Convert.ToInt32(x.Value) == (int)VehicleEnums.ComponentType.RecoveryVehicle ||
                                                                Convert.ToInt32(x.Value) == componentTypeId
                                                            ).ToList();
                    }

                }
            }
            else if(TotalComponentCount != 0 && CurrentCount > 1 && componentTypeId>0)
            {
                // Filter component items based on previous component type selection
                var componentTypeList = (SelectList)ViewBag.ComponentType;
                ViewBag.ComponentType = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.GetComponentTypes(componentTypeId,componentSubTypeId, componentTypeList, isRigidVehicle, isRecoveryVehicle);
            }
            if (componentTypeId != 0)
            {
                FillVehicleSubType(componentTypeId, componentSubTypeId);
                GetComponentImage(componentTypeId, componentSubTypeId).ToString();
                ViewBag.imageName = TempData["CompImage"];
            }
            ViewBag.componentTypeId = componentTypeId;
            ViewBag.componentId = componentId;
            ViewBag.componentSubTypeId = componentSubTypeId;
            ViewBag.ComponentCount = TotalComponentCount;
            return PartialView("~/Views/VehicleConfiguration/_CreateComponent.cshtml");
        }

        private void VehicleComponentDropDown(int userType, int componentType = 0)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("MovementClsDropDown method started successfully"));

                List<VehicleComponentType> dropDownList = new List<VehicleComponentType>();
                List<uint> componentId = new List<uint>();

                ComponentConfiguration vehicleParams = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
                dropDownList = vehicleParams.GetListOfVehicleComponent();
                ViewBag.ComponentType = new SelectList(dropDownList, "ComponentTypeId", "ComponentName");

                if (Session["movementClassificationId"] != null && Session["movementClassificationId"] != "" && componentType == 0)
                {
                    int MovementXmlTypeId = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleXmlMovementTypeMapping((VehicleXmlMovementType)(int)Session["movementClassificationId"]);
                    componentId = vehicleComponentService.VehicleComponentType(MovementXmlTypeId, UserSchema.Portal);
                    dropDownList = dropDownList.Where(x => componentId.Contains((uint)x.ComponentTypeId)).ToList();
                    ViewBag.ComponentType = new SelectList(dropDownList, "ComponentTypeId", "ComponentName");

                }
                else if (userType == UserType.Sort)
                {
                    componentId = vehicleComponentService.VehicleComponentType(0, UserSchema.Sort);
                    dropDownList = dropDownList.Where(x => componentId.Contains((uint)x.ComponentTypeId)).ToList();
                    if (componentType != 0)
                        ViewBag.ComponentType = new SelectList(dropDownList, "ComponentTypeId", "ComponentName", componentType);
                    else
                        ViewBag.ComponentType = new SelectList(dropDownList, "ComponentTypeId", "ComponentName");
                }
                else if (componentType != 0)
                {
                    ViewBag.ComponentType = new SelectList(dropDownList, "ComponentTypeId", "ComponentName", componentType);
                }

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("MovementClsDropDown method completed successfully"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult FillVehicleSubType(int vehicleTypeId, int componentSubType = 0)
        {
            try
            {
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/FillVehicleSubType JsonResult method started successfully, with parameter vehicleTypeId:{1}", Session.SessionID, vehicleTypeId));
                ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
                MovementClassificationConfig moveClassConfigObj = compConfigObj.GetListOfVehicleComponents(vehicleTypeId);
                List<VehicleCompSubType> listVehicleSubCompObj = moveClassConfigObj.GetListVehicleSubComponent(vehicleTypeId);
                listVehicleSubCompObj = listVehicleSubCompObj.GroupBy(x => x.SubCompType).Select(x => x.First()).ToList();
                int defaultSubType = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.DefaultComponentSubType(vehicleTypeId);

                ViewBag.VehicleSubType = new SelectList(listVehicleSubCompObj, "SubCompType", "SubCompName");
                List<uint> subComponentId = new List<uint>();
                if (Session["movementClassificationId"] != null && Session["movementClassificationId"] != "" && componentSubType == 0)
                {
                    int MovementXmlTypeId = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleXmlMovementTypeMapping((VehicleXmlMovementType)(int)Session["movementClassificationId"]);
                    subComponentId = vehicleComponentService.VehicleSubComponentType(MovementXmlTypeId, vehicleTypeId, UserSchema.Portal);
                    listVehicleSubCompObj = listVehicleSubCompObj.Where(x => subComponentId.Contains((uint)x.SubCompType)).ToList();
                    
                    ViewBag.VehicleSubType = new SelectList(listVehicleSubCompObj, "SubCompType", "SubCompName");
                }
                else if (componentSubType != 0)
                {
                    ViewBag.VehicleSubType = new SelectList(listVehicleSubCompObj, "SubCompType", "SubCompName", componentSubType);
                }
                else if (SessionInfo.UserTypeId == UserType.Sort)
                {
                    subComponentId = vehicleComponentService.VehicleSubComponentType(0, vehicleTypeId, UserSchema.Sort);
                    listVehicleSubCompObj = listVehicleSubCompObj.Where(x => subComponentId.Contains((uint)x.SubCompType)).ToList();

                    ViewBag.VehicleSubType = new SelectList(listVehicleSubCompObj, "SubCompType", "SubCompName");
                }
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/FillVehicleSubType JsonResult method completed successfully"));
                return Json(new { type = listVehicleSubCompObj,defaultType= defaultSubType });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}],Vehicle/FillVehicleSubType, Exception: {1}", Session.SessionID, ex.Message));
                throw ex;
            }
        }

        public JsonResult GetComponentImage(int ComponentTypeId, int ComponentSubTypeId)
        {
            ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
            MovementClassificationConfig moveClassConfigObj = compConfigObj.GetListOfVehicleComponents(ComponentTypeId);
            VehicleComponent vehclCompObj = moveClassConfigObj.GetVehicleComponent(ComponentTypeId, ComponentSubTypeId);
            string ImageName = vehclCompObj.vehicleComponentSubType.ImageName;
            TempData["CompImage"] = ImageName;
            return Json(new { result = ImageName });
        }
        public ActionResult GetVehicleComponentImage(int ComponentTypeId, int ComponentSubTypeId, int ComponentId)
        {
            ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
            MovementClassificationConfig moveClassConfigObj = compConfigObj.GetListOfVehicleComponents(ComponentTypeId);
            VehicleComponent vehclCompObj = moveClassConfigObj.GetVehicleComponent(ComponentTypeId, ComponentSubTypeId);
            ViewBag.CompImageName = vehclCompObj.vehicleComponentSubType.ImageName;
            ViewBag.ComponentId = ComponentId;
            return PartialView("~/Views/VehicleConfiguration/_ComponentShadow.cshtml");
        }

        public ActionResult GetVehicleComponentImageSub(int ComponentTypeId, int ComponentSubTypeId, int ComponentId, int Iteration=1)
        {
            ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
            MovementClassificationConfig moveClassConfigObj = compConfigObj.GetListOfVehicleComponents(ComponentTypeId);
            VehicleComponent vehclCompObj = moveClassConfigObj.GetVehicleComponent(ComponentTypeId, ComponentSubTypeId);
            ViewBag.CompImageName = vehclCompObj.vehicleComponentSubType.ImageName;
            ViewBag.ComponentId = ComponentId;
            ViewBag.Iteration = Iteration;
            return PartialView("~/Views/VehicleConfiguration/_ComponentShadowSub.cshtml");
        }

        public ActionResult GetVehicleImage(List<VehicleConfigList> componentIdList = null)
        {
            var imagename = new List<string>();
            if (componentIdList != null)
            {
                foreach (var component in componentIdList)
                {
                    GetComponentImage((int)component.ComponentTypeId, (int)component.ComponentSubTypeId).ToString();
                    imagename.Add(TempData["CompImage"].ToString());
                }
            }
            ViewBag.VehicleImage = imagename;
            return PartialView("~/Views/VehicleConfiguration/_VehicleShadow.cshtml");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken()]
        public JsonResult AddComponent(int vehicleConfigId = 0, int componentTypeId = 0, int componentSubTypeId = 0, bool isMovement = false, int organisationId = 0, string guId = "",int isSubTypeClick=0, double componentIdExisting =0)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("VehicleConfig/CreateComponent actionResult method started successfully with parameters VehicleComponent object"));

            UserInfo SessionInfo = null;
            int portalType = 0;
            bool isAdmin = false;
            string sysEventDescp = null;
            string ErrMsg = null;
            VehicleComponent vehicleComponent = new VehicleComponent();
            MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();

            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
                portalType = SessionInfo.UserTypeId;
                if (portalType == UserType.Admin || SessionInfo.IsAdmin == 1)
                {
                    isAdmin = true;
                }
            }
            ViewBag.IsAdmin = isAdmin;
            double componentId = 0;
            int configposn = 0;
            try
            {
                ComponentModel componentObj = InsertComponent(vehicleConfigId, componentTypeId, componentSubTypeId, isMovement, ref organisationId, ref guId, SessionInfo, ref componentId, isSubTypeClick,componentIdExisting);

                #region System Event Log - Haulier created fleet component
                movactiontype.UserName = SessionInfo.UserName;
                movactiontype.FleetComponentId = (long)componentId;
                movactiontype.FleetComponentName = componentObj.IntendedName;
                if (vehicleConfigId == 0)
                {
                    movactiontype.SystemEventType = SysEventType.haulier_created_fleet_component;
                }
                else if (vehicleConfigId > 0)
                {
                    movactiontype.FleetVehicleId = (long)vehicleConfigId;
                    movactiontype.SystemEventType = SysEventType.Haulier_created_component;
                }

                sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out sysEventDescp);
                loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                #endregion

                if (componentId > 0)
                {
                    //AssignMovementClassification(componentObj.VehicleIntent);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.WARNING, string.Format("[{0}],Vehicle/CreateComponent,  Fleet management Workflow not started as ComponentId is 0", Session.SessionID));
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/CreateComponent, Exception: {0}", Session.SessionID, ex.Message));
                throw;
            }
            Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Vehicle/CreateComponent actionResult method completed successfully"));

            return Json(new { componentId = componentId, guid = guId });
        }

        private ComponentModel InsertComponent(int vehicleConfigId, int componentTypeId, int componentSubTypeId, bool isMovement, ref int organisationId, ref string guId, UserInfo SessionInfo, ref double componentId,int isSubTypeClick=0,double componentIdExisting=0)
        {
            if (SessionInfo.UserTypeId != UserType.Sort)
            {
                organisationId = (int)SessionInfo.OrganisationId;
            }
            if (guId == "")
            {
                System.Guid guid = System.Guid.NewGuid();
                guId = guid.ToString();
            }

            ComponentModel componentObj = new ComponentModel();
            componentObj.OrganisationId = organisationId;
            componentObj.ComponentType = componentTypeId;
            componentObj.ComponentSubType = componentSubTypeId;
            componentObj.GUID = guId;

            if (vehicleConfigId == 0 || (vehicleConfigId != 0 && isMovement))
            {
                if (isSubTypeClick > 0)
                {
                    //Update subtype
                    componentObj.ComponentId = componentIdExisting;
                    componentId = componentIdExisting;
                    vehicleComponentService.UpdateComponentSubTypeToTemp(componentObj);
                }
                else
                {
                    componentId = vehicleComponentService.InsertComponentToTemp(componentObj);
                }
            }
            else if (vehicleConfigId != 0)
            {
                componentId = vehicleComponentService.CreateComponent(componentObj);
            }

            return componentObj;
        }

        [HttpPost]
        public JsonResult DeleteComponentConfiguration(int componentId, int vehicleId, bool isMovement = false)
        {
            int result = 0;
            try
            {
                UserInfo SessionInfo = null;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , JsonResult,VehicleConfigController/DeleteConfiguration , Delete vehicle config component- {1}", Session.SessionID, componentId));
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                result = vehicleComponentService.DeleteComponentTemp(componentId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] , VehicleConfigController/DeleteConfiguration, Exception: {1}", Session.SessionID, ex.Message));
                throw;
            }
            return Json(new { vehicleId = vehicleId, result = result });
        }

        [HttpPost]
        public JsonResult ImportComponent(int componentId, bool isMovement = false, int vehicleId = 0, string guId = "",bool isCandidate=false)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],VehicleConfig/ViewComponent ActionResult method started successfully, with parameters componentId:{1}", Session.SessionID, componentId));
            ViewBag.ComponentId = componentId;
            double newComponentId = 0;
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }

            string sysEventDescp = null;
            string ErrMsg = null;
            int organisationId = (int)SessionInfo.OrganisationId;
            MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();

            VehicleComponent vehclCompObj = new VehicleComponent();
            ComponentModel VehicleComponentObj = null;
            List<VehicleRegistration> listVehclRegObj = null;
            List<Axle> axleList = null;
            ViewBag.AxleList = null;

            if (guId == "")
            {
                System.Guid guid = System.Guid.NewGuid();
                guId = guid.ToString();
            }

            int vehicleTypeId = 0;
            int vehicleSubTypeId = 0;
            int movementId = 0;

            if (componentId != null)
            {
                if (isCandidate)
                {
                    VehicleComponentObj = vehicleComponentService.GetVR1VehicleComponent(componentId, SessionInfo.UserSchema);
                }
                else if (isMovement)
                {
                    VehicleComponentObj = vehicleComponentService.GetComponentTemp(componentId, "", SessionInfo.UserSchema);
                }
                else
                {
                    VehicleComponentObj = vehicleComponentService.GetVehicleComponent(componentId);
                }
                

                if (VehicleComponentObj.ComponentType != 0)
                {
                    vehicleTypeId = VehicleComponentObj.ComponentType;
                    vehicleSubTypeId = VehicleComponentObj.ComponentSubType;
                    movementId = VehicleComponentObj.VehicleIntent;
                    VehicleComponentObj.GUID = guId;
                    //AssignMovementClassification(VehicleComponentObj.VehicleIntent);
                    if (isCandidate)
                    {
                        listVehclRegObj = vehicleComponentService.GetVR1RegistrationDetails(componentId, SessionInfo.UserSchema);
                    }
                    else if (isMovement)
                    {
                        listVehclRegObj = vehicleComponentService.GetRegistrationTemp(componentId, isMovement, SessionInfo.UserSchema);
                    }
                    else
                    {
                        listVehclRegObj = vehicleComponentService.GetRegistrationDetails(componentId);

                    }
                    if (isCandidate)
                    {
                        axleList = vehicleComponentService.ListVR1vehAxle(componentId, SessionInfo.UserSchema);
                    }
                    else if (isMovement)
                    {
                        axleList = vehicleComponentService.ListAxleTemp(componentId, isMovement, SessionInfo.UserSchema);
                    }
                    else
                    {
                        axleList = vehicleComponentService.ListAxle(componentId);
                    }

                    ViewBag.AxleList = axleList;

                    vehclCompObj = GetVehicleComponent(vehicleTypeId, vehicleSubTypeId);

                    if (VehicleComponentObj != null)
                        vehclCompObj.UpdateVehicleProperties(VehicleComponentObj);

                    if (listVehclRegObj != null && listVehclRegObj.Count != 0)
                    {
                        vehclCompObj.ListVehicleReg = new List<VehicleRegistration>();
                        vehclCompObj.ListVehicleReg = listVehclRegObj;
                    }

                    newComponentId = vehicleComponentService.InsertComponentToTemp(VehicleComponentObj);

                    #region System Event Log - Haulier created fleet component
                    movactiontype.UserName = SessionInfo.UserName;
                    movactiontype.FleetComponentId = (long)componentId;
                    movactiontype.FleetComponentName = VehicleComponentObj.IntendedName;

                    movactiontype.SystemEventType = SysEventType.Haulier_created_component;


                    sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out sysEventDescp);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                    #endregion


                    if (newComponentId > 0)
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/CreateComponent JsonResult method for registration details save for component started successfully, with parameters compId:{1}", Session.SessionID, componentId));
                        int IdNumber = 0;
                        if (listVehclRegObj != null)
                        {
                            foreach (VehicleRegistration registration in listVehclRegObj)
                            {
                                IdNumber = vehicleComponentService.CreateRegistrationTemp((int)newComponentId, registration.RegistrationId, registration.FleetId, false, SessionInfo.UserSchema);

                            }
                        }

                        #region System Event Log - created_component_for_so_application
                        movactiontype.FleetComponentId = IdNumber;
                        movactiontype.SystemEventType = SysEventType.haulier_added_registration_for_fleet_component;

                        sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                        loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                        #endregion

                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/SaveRegistrationID JsonResult method completed successfully, Registration saved successfully", Session.SessionID));
                        if (axleList != null)
                        {
                            foreach (Axle axle in axleList)
                            {
                                vehicleComponentService.InsertAxleDetailsTemp(axle, (int)newComponentId, false, SessionInfo.UserSchema);
                            }
                        }

                        movactiontype.SystemEventType = SysEventType.haulier_added_axle_details_for_fleet_component;

                        sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                        bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);

                        //For saving component details at the time of vehicle save
                        List<RegistrationParams> registrationParams = listVehclRegObj.Select(s => new RegistrationParams
                        {
                            RegistrationValue = s.RegistrationId,
                            FleetId = s.FleetId
                        }).ToList();
                        //if (isMovement && vehicleId != 0)
                        //{
                        //    bool result = vehicleconfigService.InsertMovementConfigPosnTemp(guId, vehicleId, SessionInfo.UserSchema);
                        //}
                        VehicleComponentObj.ComponentId = newComponentId;

                    }
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/ViewComponent ActionResult method completed successfully"));

                }
            }

            return Json(new { ConfigId = vehicleId, Guid = guId,ComponentType= vehicleTypeId });
        }

        private VehicleComponent GetVehicleComponent(int ComponentTypeId, int ComponentSubTypeId, int movementId = 0)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("VehicleConfig/GetVehicleComponent method started successfully, with parameters ComponentTypeId:{0}, ComponentSubTypeId:{1}", ComponentTypeId, ComponentSubTypeId));
                ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
                VehicleComponent vehclCompObj;
                if (movementId != 0)
                {
                    MovementClassificationConfig moveClassConfigObj = compConfigObj.GetMovementClassificationConfig(movementId);
                    vehclCompObj = moveClassConfigObj.GetVehicleComponent(ComponentTypeId, ComponentSubTypeId);
                }
                else
                {
                    MovementClassificationConfig moveClassConfigObj = compConfigObj.GetListOfVehicleComponents(ComponentTypeId);
                    vehclCompObj = moveClassConfigObj.GetVehicleComponent(ComponentTypeId, ComponentSubTypeId);
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

        public ActionResult AssessConfigurationType(string guId = "", bool boatMastFlag = false)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/AssessConfigType , Load view of ConfigurationGeneralPage", Session.SessionID));
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }
            List<VehicleConfigList> componentIdList = new List<VehicleConfigList>();
            componentIdList = vehicleComponentService.GetComponentIdTemp(guId, 0, SessionInfo.UserSchema);
            List<int> compIds = new List<int>();
            foreach (var id in componentIdList)
            {
                compIds.Add((int)id.ComponentId);
            }
            var componentIdOrderList = componentIdList.OrderBy(s => s.ComponentId).ToList();

            List<uint> vehicleConfig = vehicleconfigService.AssessConfigurationType(compIds, boatMastFlag, UserSchema.Portal);
            if (vehicleConfig.Count > 0)
            {
                vehicleConfig.RemoveAll(x => (x.ToString() == "244007")||(x.ToString() == "244006"));

                List<VehicleConfigurationType> configdrpdwn = null;
                ComponentConfiguration vehicleParams = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
                configdrpdwn = vehicleParams.GetConfigType();
                List<VehicleConfigurationType> configList = configdrpdwn.Where(x => vehicleConfig.Contains((uint)x.ConfigurationTypeId)).ToList();
                ViewBag.VehicleTypeConfig = "";
                ViewBag.VehicleTypeConfigId = 0;
                ViewBag.MaxTractorCount = 0;
                ViewBag.MaxTrailerCount = 0;
                if (configList.Count > 0)
                {
                    ViewBag.VehicleTypeConfig = configList[0].ConfigurationName;
                    ViewBag.VehicleTypeConfigId = configList[0].ConfigurationTypeId;
                    ViewBag.MaxTractorCount = configList[0].MaxTractorCount;
                    ViewBag.MaxTrailerCount = configList[0].MaxTrailerCount;
                }
            }
            Session["componentIdList"] = componentIdOrderList;
            return PartialView("~/Views/VehicleConfiguration/_Assessment.cshtml");
        }

        public ActionResult VehicleConfiguration(int movementId, int vehicleConfigId, int vehicleId = 0, bool ISNotifVeh = false, bool planMovement = false, bool isEditVehicleInSoProcessing = false, string guId = "",bool VSONotificationVehicle=false,bool vehicleResetOnEdit=false)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/VehicleConfiguration , Load Vehicle Config General Page movement id : {1} and config Id : {2}", Session.SessionID, movementId, vehicleConfigId));

                UserInfo sessionValues = null;
                if (Session["UserInfo"] != null)
                {
                    sessionValues = (UserInfo)Session["UserInfo"];
                    ViewBag.Units = sessionValues.VehicleUnits;
                }
                ViewBag.movementTypeId = movementId;
                ViewBag.componentTypeList = Session["componentIdList"];
                NewConfigurationModel vehicleconfig = new NewConfigurationModel();
                ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];

                ConfigurationModel VehicleConfig=new ConfigurationModel();

                VehicleConfiguration vehicleConfigObj = null;
                if (vehicleResetOnEdit)
                {
                    vehicleId = 0;
                }
                if (TempData["VehicleConfigModel"] != null)
                {
                    vehicleConfigObj = (VehicleConfiguration)TempData["VehicleConfigModel"];
                    VehicleConfig = ConvertToConfigurationModel(vehicleConfigObj);
                }
                else
                {
                    if (isEditVehicleInSoProcessing)
                    {
                        VehicleConfig = vehicleconfigService.GetRouteConfigInfoForVR1(vehicleId, sessionValues.UserSchema, 0);
                    }
                    else
                    {
                        VehicleConfig = vehicleconfigService.GetVehicleDetails(vehicleId, planMovement, sessionValues.UserSchema);
                    }

                }
                MovementClassificationConfig mvClassConfig = new MovementClassificationConfig();
                vehicleConfigObj = compConfigObj.GetVehicleConfiguration(vehicleConfigId);

                if (vehicleConfigObj != null)
                {
                    vehicleConfigObj.UpdateConfigProperties(VehicleConfig);
                }

                ViewBag.configFieldsCount = vehicleConfigObj.VehicleConfigParamList.Count;
                if (planMovement && sessionValues.UserSchema != UserSchema.Sort)
                {
                    var itemToRemove1 = vehicleConfigObj.VehicleConfigParamList.FirstOrDefault(r => r.ParamModel == "Notes");
                    vehicleConfigObj.VehicleConfigParamList.Remove(itemToRemove1);
                    ViewBag.configFieldsCount = vehicleConfigObj.VehicleConfigParamList.Count - 1;
                }
                if(ViewBag.componentTypeList!=null && ViewBag.componentTypeList.Count==1&& vehicleConfigId == (int)ConfigurationType.RecoveryVehicle)
                {
                    var rigidlength = vehicleConfigObj.VehicleConfigParamList.FirstOrDefault(r => r.ParamModel == "Length");
                    if(rigidlength!=null)
                        vehicleConfigObj.VehicleConfigParamList.Remove(rigidlength);
                }
                TempData["VehicleConfigModel"] = null;
                TempData["ConfigModel"] = null;
                ViewBag.VSONotificationVehicle = VSONotificationVehicle;
                if (Session["VSOVehicleClassificationType"]!=null && (int)Session["VSOVehicleClassificationType"] == (int)VehicleClassificationType.VehicleSpecialOrder)
                {
                    ViewBag.VSONotificationVehicle = true;
                }
                ViewBag.PlanMovement = planMovement;
                ViewBag.Guid = guId;
                ViewBag.VehicleId = vehicleId;
                ViewBag.CandidateVehicle = isEditVehicleInSoProcessing;
                ViewBag.ConfigTypeId = vehicleConfigId;
                ViewBag.SpeedUnits = new SelectList(GetSpeedUnits(), "Id", "Value", null);
                return PartialView("~/Views/VehicleConfiguration/_VehicleConfiguration.cshtml", vehicleConfigObj.VehicleConfigParamList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] , JsonResult,VehicleConfigController/GeneralPage , Exception - {1}", Session.SessionID, ex.Message));
                throw;
            }
        }
        public ActionResult ConfigurationRegistration(int vehicleId, bool RegBtn = true, bool isVR1 = false, bool isAmend = false, bool planMovement = false, bool isEditVehicleInSoProcessing = false,bool vehicleResetOnEdit=false)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/RegistrationConfiguration , Load Vehicle Config Registration Page", Session.SessionID));
            ViewBag.IsTractor = true;
            ViewBag.VehicleTypeId = 1;
            ViewBag.IsAmend = isAmend;
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }

            List<VehicleRegistration> listVehclRegObj = new List<VehicleRegistration>();
            try
            {
                if (vehicleResetOnEdit)
                {
                    vehicleId = 0;
                }
                if (RegBtn && !isVR1 && !isEditVehicleInSoProcessing)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/RegistrationConfiguration , Load fleet Vehicle Config Registration Page vehicle id : {1}", Session.SessionID, vehicleId));

                    if (planMovement)
                    {
                        //listVehclRegObj = vehicleconfigService.GetVehicleRegistrationTemp(vehicleId, SessionInfo.UserSchema);
                        listVehclRegObj = vehicleconfigService.GetMovementVehicleRegDetails(vehicleId, SessionInfo.UserSchema);
                    }
                    else
                    {
                        listVehclRegObj = vehicleconfigService.GetVehicleRegistrationDetails(vehicleId, SessionInfo.UserSchema);
                    }
                }
                else if (isVR1 || isEditVehicleInSoProcessing)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/RegistrationConfiguration , Load VR1 Vehicle Config Registration Page vehicle id : {1}", Session.SessionID, vehicleId));

                    listVehclRegObj = vehicleconfigService.GetVR1VehicleRegistrationDetails(vehicleId, SessionInfo.UserSchema);

                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/RegistrationConfiguration , Load SO Vehicle Config Registration Page vehicle id : {1}", Session.SessionID, vehicleId));

                    listVehclRegObj = vehicleconfigService.GetApplVehicleRegistrationDetails(vehicleId, SessionInfo.UserSchema);
                }                

                ViewBag.RegBtn = RegBtn;
                ViewBag.VR1appln = isVR1;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] , VehicleConfigController/RegistrationConfiguration, Exception: {1}", Session.SessionID, ex.Message));
                throw ex;
            }
            return PartialView("~/Views/VehicleConfiguration/_ConfigRegistration.cshtml", listVehclRegObj);
        }

        [HttpPost]
        public JsonResult SaveConfiguration(VehicleConfiguration vehicleConfigObj, int configTypeId, int movementId, int? speedUnit = null, List<RegistrationParams> registrationParams = null, bool isMovement = false, int AplnMovemntId = 0, bool isCandidate = false, int CandRevisionId = 0, string guId = "")
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , HttpPost,VehicleConfigController/SaveConfiguration , Create new vehicle config", Session.SessionID));

            UserInfo SessionInfo = null;
            int portalType = 0;
            bool isAdmin = false;
            int candidateResult = 0;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
                portalType = SessionInfo.UserTypeId;
                if (portalType == 696006 || SessionInfo.IsAdmin == 1)
                {
                    isAdmin = true;
                }
            }
            double configurationid = 0;
            TempData["VehicleConfigModel"] = vehicleConfigObj;
            //double movementId = 0;
            try
            {

                int organisationId;
                if (SessionInfo.UserTypeId == 696008)
                {
                    organisationId = (int)Session["SORTOrgID"];
                }
                else
                {
                    organisationId = (int)SessionInfo.OrganisationId;
                }
                NewConfigurationModel vehicleConfiguration = ConvertToConfiguration(vehicleConfigObj);
                vehicleConfiguration.OrganisationId = organisationId;
                vehicleConfiguration.VehicleType = configTypeId;
                vehicleConfiguration.VehiclePurpose = movementId;
                vehicleConfiguration.SpeedUnit = speedUnit;
                List<MovementVehicleConfig> movementVehicle = new List<MovementVehicleConfig>();
                if (isCandidate)
                {
                    vehicleConfiguration.CandRevisionId = CandRevisionId;
                    vehicleConfiguration = vehicleconfigService.InsertVR1VehicleConfiguration(vehicleConfiguration, SessionInfo.UserSchema);
                    configurationid = (double)vehicleConfiguration.VehicleId;

                    ConfigurationModel configuration = new ConfigurationModel() ;
                    if (vehicleConfigObj != null)
                    {
                        configuration = ConvertToConfigurationModel(vehicleConfigObj);
                        configuration.VehicleType = configTypeId;
                    }
                    AssessMoveTypeParams moveTypeParams = new AssessMoveTypeParams
                    {
                        VehicleId = (int)configurationid,
                        IsRoute = 1,
                        UserSchema = SessionInfo.UserSchema,
                        PreviousMovementType = null,
                        ForceApplication = SessionInfo.UserSchema == UserSchema.Sort,
                        configuration = configuration
                    };
                    VehicleMovementType vehicleMovementType = vehicleconfigService.AssessMovementType(moveTypeParams);
                    if (vehicleMovementType.MovementType != (int)MovementType.special_order)
                    {
                        bool dltresult = vehicleconfigService.DeleteSelectedVR1VehicleComponent((int)configurationid, SessionInfo.UserSchema);
                        candidateResult = 1;
                        configurationid = 0;
                    }
                }
                else if (isMovement)
                {
                    vehicleConfiguration.MovementId = AplnMovemntId;
                    movementVehicle = vehicleconfigService.InsertConfigurationTemp(vehicleConfiguration, SessionInfo.UserSchema);
                    configurationid = movementVehicle[0].VehicleId;
                    movementId = (int)movementVehicle[0].MovementId;
                }
                else
                {
                    configurationid = vehicleconfigService.CreateConfiguration(vehicleConfiguration);
                }
                vehicleConfiguration.VehicleId = Convert.ToInt32(configurationid);
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , HttpPost,VehicleConfigController/CreateConfiguration , Created new vehicle config - {1}", Session.SessionID, configurationid));

                int IdNumber = 0;
                if (configurationid > 0)
                {
                    if (isCandidate)
                    {
                        if (registrationParams != null)
                        {
                            foreach (RegistrationParams registration in registrationParams)
                            {
                                IdNumber = vehicleconfigService.SaveVR1VehicleRegistration((int)configurationid, registration.RegistrationValue, registration.FleetId, SessionInfo.UserSchema);
                            }
                        }
                    }
                    else if (isMovement)
                    {
                        if (registrationParams != null)
                        {
                            foreach (RegistrationParams registration in registrationParams)
                            {
                                IdNumber = vehicleconfigService.CreateVehicleRegistrationTemp((int)configurationid, registration.RegistrationValue, registration.FleetId, SessionInfo.UserSchema);
                            }
                        }
                    }
                    else
                    {
                        if (registrationParams != null)
                        {
                            foreach (RegistrationParams registration in registrationParams)
                            {
                                IdNumber = vehicleconfigService.CreateVehicleRegistration((int)configurationid, registration.RegistrationValue, registration.FleetId);
                            }
                        }
                    }

                }
                if (!isMovement)
                {
                    Session["movementClassificationName"] = null;
                    Session["movementClassificationId"] = null;
                }
                TempData.Keep("VehicleConfigModel");

                #region System Event Log - haulier_created_fleet_vehicle
                string ErrMsg = string.Empty;
                string sysEventDescp = string.Empty;

                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                movactiontype.UserName = SessionInfo.UserName;
                movactiontype.FleetVehicleId = (long)vehicleConfiguration.VehicleId;
                movactiontype.FleetVehicleName = vehicleConfiguration.VehicleName;
                movactiontype.SystemEventType = SysEventType.haulier_created_fleet_vehicle;

                sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                #endregion
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] , VehicleConfigController/SaveConfiguration, Exception: {1}", Session.SessionID, ex.Message));
                throw ex;
            }

            return Json(new { configId = configurationid, movementId = movementId, candidateResult= candidateResult });
        }
        public ActionResult ComponentDetail(bool isApplicationVehicle = false, bool isVR1 = false, bool IsNotifVeh = false, string Guid = "", int isEdit = 0, bool isMovement = false, int vehicleConfigId = 0, bool isCandidate = false, int movementTypeId = 0,bool isEditMovement=false)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/ComponentDetail , Load partial view of CreateConfiguration", Session.SessionID));
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }
            #region application vehicle
            if (isApplicationVehicle && !isVR1)
                ViewBag.newApplicationVehicle = true;
            if (isVR1)
                ViewBag.VR1appln = true;
            #endregion
            ViewBag.IsNotifVeh = IsNotifVeh;

            ViewBag.IsEdit = isEdit;
            ViewBag.IsMovement = isMovement;
            ViewBag.Guid = Guid;
            ViewBag.vehicleConfigId = vehicleConfigId;
            ViewBag.isCandidate = isCandidate;
            ViewBag.movementTypeId = movementTypeId;
            ViewBag.isEditMovement = isEditMovement; 
            List<VehicleConfigList> componentIdList = new List<VehicleConfigList>();

            componentIdList = vehicleComponentService.GetComponentIdTemp(Guid, 0, SessionInfo.UserSchema).OrderBy(s => s.ComponentId).ToList();


            TempData.Keep("VehicleConfigModel");
            return PartialView("~/Views/VehicleConfiguration/_ComponentDetails.cshtml", componentIdList);
        }

        public ActionResult ComponentDetailSub(bool isApplicationVehicle = false, bool isVR1 = false, bool IsNotifVeh = false, string Guid = "", int isEdit = 0, bool isMovement = false, int vehicleConfigId = 0, bool isCandidate = false, int movementTypeId = 0,bool isEditMovement=false,int ConfigTypeId=0)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/ComponentDetailSub , Load partial view of CreateConfiguration", Session.SessionID));
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }
            #region application vehicle
            if (isApplicationVehicle && !isVR1)
                ViewBag.newApplicationVehicle = true;
            if (isVR1)
                ViewBag.VR1appln = true;
            #endregion
            ViewBag.IsNotifVeh = IsNotifVeh;

            ViewBag.IsEdit = isEdit;
            ViewBag.IsMovement = isMovement;
            ViewBag.Guid = Guid;
            ViewBag.vehicleConfigId = vehicleConfigId;
            ViewBag.configTypeId = ConfigTypeId;
            ViewBag.isCandidate = isCandidate;
            ViewBag.movementTypeId = movementTypeId;
            ViewBag.isEditMovement = isEditMovement; 

            List<VehicleConfigList> componentIdList = new List<VehicleConfigList>();

            componentIdList = vehicleComponentService.GetComponentIdTemp(Guid, 0, SessionInfo.UserSchema).OrderBy(s => s.ComponentId).ToList();

            ViewBag.componentCount = componentIdList.Count;
            TempData.Keep("VehicleConfigModel");
            if (componentIdList.Count == 1)
            {
                return PartialView("~/Views/VehicleConfiguration/_SingleComponent.cshtml", componentIdList);
            }
            else
            {
                return PartialView("~/Views/VehicleConfiguration/_ComponentDetailsSub.cshtml", componentIdList);
            }
        }
        public ActionResult ComponentGeneralPage(int vehicleSubTypeId, int vehicleTypeId, int movementId, int? componentId, string GUID = "", bool isComponent = false, bool isLastComponent = false, int vehicleConfigId = 0, bool isNotify = false, int isFromConfig = 0, bool movement = false, bool isCandidate = false, bool isEditMovement = false,int compNumber=1,int configTypeId=0)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/GeneralComponent ActionResult method started successfully, with parameters vehicleSubTypeId:{1}, vehicleTypeId:{2}, componentId:{3}", Session.SessionID, vehicleSubTypeId, vehicleTypeId, componentId));
                UserInfo sessionValues = null;
                if (Session["UserInfo"] != null)
                {
                    sessionValues = (UserInfo)Session["UserInfo"];
                    ViewBag.Units = sessionValues.VehicleUnits;
                }
                ViewBag.ComponentId = componentId;
                ViewBag.ShowComponent = false;

                ViewBag.IsComponent = isComponent;
                ViewBag.MakeComponent = 0;
                ViewBag.isFromConfig = isFromConfig;
                TempData["IsFromConfig"] = isFromConfig;
                VehicleComponent vehclCompObj = null;
                ComponentModel VehicleComponentObj = null;
                if (componentId != null)
                {
                    int compId = (int)componentId;

                    if (isFromConfig == 1 || isEditMovement)
                    {
                        VehicleComponentObj = vehicleComponentService.GetComponentTemp(compId, GUID, sessionValues.UserSchema);
                    }
                    else if (isCandidate)
                    {
                        VehicleComponentObj = vehicleComponentService.GetVR1VehicleComponent(compId, sessionValues.UserSchema);
                    }
                    else if (movement)
                    {
                        VehicleComponentObj = vehicleComponentService.GetComponentTemp(compId, "", sessionValues.UserSchema);
                    }
                    else
                    {
                        VehicleComponentObj = vehicleComponentService.GetVehicleComponent(compId);
                    }

                    vehicleTypeId = VehicleComponentObj.ComponentType;
                    vehicleSubTypeId = VehicleComponentObj.ComponentSubType;
                    if (movementId == 0)
                    {
                        movementId = VehicleComponentObj.VehicleIntent;
                    }
                }

                ViewBag.VehicleSubTypeId = vehicleSubTypeId;
                ViewBag.VehicleTypeId = vehicleTypeId;
                ViewBag.MovementID = movementId;
                ViewBag.IsMovement = movement;
                ViewBag.IsCandidate = isCandidate;
                ViewBag.configTypeId = configTypeId;

                ViewBag.VehicleConfigId = vehicleConfigId;
                ViewBag.SpeedUnits = new SelectList(GetSpeedUnits(), "Id", "Value", null);
                if (vehicleConfigId != 0 && isComponent == false)
                {
                    TempData["ChkIsFromConfig"] = 1;
                    ViewBag.ChkIsFromConfig = 1;
                }
                else
                {
                    TempData["ChkIsFromConfig"] = 0;
                    ViewBag.ChkIsFromConfig = 0;
                }

                int MovementXmlTypeId = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleXmlMovementTypeMapping((VehicleXmlMovementType)movementId);
                if (MovementXmlTypeId == 270101)
                    MovementXmlTypeId = 0;
                vehclCompObj = GetVehicleComponent(vehicleTypeId, vehicleSubTypeId, MovementXmlTypeId);
                if (vehclCompObj != null)
                    ViewBag.CompImageName = vehclCompObj.vehicleComponentSubType.ImageName;

                
                if (vehicleTypeId ==(int)ComponentType.BallastTractor &&( movementId ==  270103 || movementId == 270104 || movementId == 270105
                    || movementId == 270106 || movementId == 270107 || movementId == 270108 || movementId == 270109))
                {
                    var itemToRemove1 = vehclCompObj.VehicleParamList.FirstOrDefault(r => r.ParamModel == "Outside Track");
                    vehclCompObj.VehicleParamList.Remove(itemToRemove1);                    
                }
                if (movementId == 270110 || movementId == 270111 || movementId == 270112 || movementId == 270156)
                {
                    for( int i=0;i< vehclCompObj.VehicleParamList.Count;i++)
                    {
                        if (ComponentRequiredFieldForVR1(vehclCompObj.vehicleCompType.ComponentTypeId, vehclCompObj.VehicleParamList[i].ParamModel))
                        {
                            vehclCompObj.VehicleParamList[i].IsRequired = 1;
                        }
                    }
                }
                if (configTypeId == (int)ConfigurationType.RigidAndDrag && vehicleTypeId == (int)ComponentType.RigidVehicle)
                {
                    int index = vehclCompObj.VehicleParamList.FindIndex(s => s.ParamModel == "Coupling");
                    if (index != -1)
                        vehclCompObj.VehicleParamList[index].ParamValue = "Drawbar";
                }
                if (configTypeId == (int)ConfigurationType.BoatMast && vehicleTypeId == (int)ComponentType.SemiTrailer && movementId==270006)
                {
                    int index = vehclCompObj.VehicleParamList.FindIndex(s => s.ParamModel == "Weight");
                    if (index != -1)
                        vehclCompObj.VehicleParamList[index].IsRequired = 1;
                }

                if (movement && sessionValues.UserSchema != UserSchema.Sort)
                {
                    var itemToRemove1 = vehclCompObj.VehicleParamList.FirstOrDefault(r => r.ParamModel == "Notes");
                    if(itemToRemove1!=null)
                        vehclCompObj.VehicleParamList.Remove(itemToRemove1);
                }
                
                if (VehicleComponentObj != null)
                {
                    if (vehclCompObj != null)
                    {
                        vehclCompObj.UpdateVehicleProperties(VehicleComponentObj);
                    }
                    ViewBag.MakeConfig = 1;
                    TempData["MakeConfig"] = ViewBag.MakeConfig;
                }

                #region check last component in config
                
                ViewBag.IsLastComp = isLastComponent;
                if (vehclCompObj != null)
                {
                    ViewBag.ImageName = vehclCompObj.vehicleComponentSubType.ImageName;
                }
                //Session["is_lastComponent"] = null;
                #endregion
                ViewBag.CompNumber = compNumber;

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/GeneralComponent ActionResult method completed successfully"));

                return PartialView("~/Views/VehicleConfig/VehicleComponentEdit.cshtml", vehclCompObj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}],Vehicle/GeneralComponent, Exception: {1}", Session.SessionID, ex.Message));
                throw ex;
            }
        }
        public ActionResult AxleComponent(int axleCount, int componentId, string compIds, int vehicleSubTypeId, int vehicleTypeId, int movementId, int? weight, bool IsEdit, bool isApplication = false, int appRevID = 0, long vehConfigID = 0, bool isVR1 = false, int isFromConfig = 0, int vehicleConfigId = 0, bool movement = false, bool isCandidate = false, bool isEditMovement = false, bool isFromPopUp=false,List<Axle> axles=null, bool isLastComponent = false, string GUID = "")
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/AxleComponent ActionResult method started successfully, with parameters axleCount:{1}, componentId:{2}, vehicleSubTypeId:{3}, vehicleTypeId:{4}, weight:{5}, IsEdit:{6}", Session.SessionID, axleCount, componentId, vehicleSubTypeId, vehicleTypeId, weight, IsEdit));
                ViewBag.AxleCount = axleCount;
                ViewBag.IsFromConfig = isFromConfig;
                ViewBag.IsLastComponent = isLastComponent;
                ViewBag.movementtype = movementId;
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }

                if (weight != null)
                {
                    string weightRange = AxleValidator.GetAxleToleranceRange((int)weight).getRangeString();
                    ViewBag.AxleWeightTolerance = weightRange;
                }
                else
                {
                    ViewBag.AxleWeightTolerance = "";
                }
                //Get Vehicle Component object from cache with validation details
                VehicleComponent vhclCompObj;
                
                if (movementId != 0)
                {
                    int MovementXmlTypeId = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleXmlMovementTypeMapping((VehicleXmlMovementType)movementId);
                    vhclCompObj = GetVehicleComponent(vehicleTypeId, vehicleSubTypeId, MovementXmlTypeId);
                }
                else
                {
                    vhclCompObj = GetVehicleComponent(vehicleTypeId, vehicleSubTypeId);
                }

                AxleValidator axleValidator = new AxleValidator();
                if (vhclCompObj != null)
                {
                    axleValidator = vhclCompObj.vehicleComponentSubType.axleValidator;
                    if (movementId !=0)
                    {
                        if (movementId == 270101)
                            axleValidator.IsConfigureTyreCentreSpacing = false;
                        else
                            axleValidator.IsConfigureTyreCentreSpacing = TyreDetailsRequired(vehicleTypeId, movementId);

                        axleValidator.IsTyreCentreSpacingRequired = false;
                        axleValidator.IsTyreSizeRequired = false;

                        bool reuired = AxleRequiedFields(vehicleTypeId, movementId);
                        if (reuired)
                        {
                            axleValidator.IsTyreCentreSpacingRequired = true;
                            axleValidator.IsTyreSizeRequired = true;
                        }
                    }
                }
                else
                {
                    vhclCompObj = GetVehicleComponent(vehicleTypeId, vehicleSubTypeId);
                    axleValidator = vhclCompObj.vehicleComponentSubType.axleValidator;
                    axleValidator.IsConfigureTyreCentreSpacing = vhclCompObj.IsConfigTyreCentreSpacing;
                }
                List<Axle> axleList = new List<Axle>();

                //axleList - From database
                //axles - from Ajax

                bool tyreEmpty = false;
                axles = axles != null && axles.Count > 0 && axles[0] == null ? null : axles;

                if (axles != null && axles.Count > 0 && axles[0] != null)
                {
                    axles = axles.OrderBy(x => x.ComponentId).ToList();
                    if (string.IsNullOrEmpty(compIds) && axleCount != 0 && axleCount != axles.Count)
                    {
                        ViewBag.AxleCount = axleCount;
                        int cnt = axleCount - axles.Count;
                        int axlcnt = axles.Count;
                        if (axleCount > axlcnt)
                        {
                            for (int i = 1; i <= cnt; i++)
                            {
                                Axle axl = new Axle();
                                axl.AxleNumId = axlcnt + i;
                                axles.Add(axl);
                            }
                        }

                        else
                        {
                            axles = axles.Take(axleCount).ToList();
                        }
                    }
                    else
                    {
                        ViewBag.AxleCount = axles.Count;
                    }
                    ViewBag.AxleList = axles;
                    tyreEmpty = axles.All(item => (item.TyreSize is null || item.TyreSize == "") && (item.TyreCenters is null || item.TyreCenters == ""));

                }
                //If IsEdit=true - Get axles details from database
                if (IsEdit && (axles == null || axles.Count == 0 || (tyreEmpty&& axleValidator.IsTyreSizeRequired)))
                {
                    STP.Domain.LoggingAndReporting.MovementActionIdentifiers movactiontype = new STP.Domain.LoggingAndReporting.MovementActionIdentifiers();
                    movactiontype.ComponentId = componentId;
                    movactiontype.RevisionId = appRevID;
                    movactiontype.FleetComponentId = Convert.ToInt32(vehConfigID);
                    movactiontype.UserName = SessionInfo.UserName;
                    string ErrMsg = string.Empty;
                    int user_ID = Convert.ToInt32(SessionInfo.UserId);

                    if (!isApplication && !isVR1)
                    {
                        if (isFromConfig == 1 || isEditMovement)
                        {
                            if (!string.IsNullOrEmpty(compIds))
                            {
                                var compIdList = compIds.Split(',');
                                if (compIdList != null)
                                {
                                    foreach(var item in compIdList)
                                    {
                                        var axleListByCompId= vehicleComponentService.ListAxleTemp(Convert.ToInt32(item), false, SessionInfo.UserSchema);
                                        if (axleListByCompId != null)
                                            axleList.AddRange(axleListByCompId);
                                    }
                                }
                            }
                            else
                                axleList = vehicleComponentService.ListAxleTemp(componentId, false, SessionInfo.UserSchema);
                        }
                        else if (isCandidate)
                        {
                            if (!string.IsNullOrEmpty(compIds))
                            {
                                var compIdList = compIds.Split(',');
                                if (compIdList != null)
                                {
                                    foreach (var item in compIdList)
                                    {
                                        var axleListByCompId = vehicleComponentService.ListVR1vehAxle(Convert.ToInt32(item), SessionInfo.UserSchema);
                                        if (axleListByCompId != null)
                                            axleList.AddRange(axleListByCompId);
                                    }
                                }
                            }
                            else
                                axleList = vehicleComponentService.ListVR1vehAxle(componentId, SessionInfo.UserSchema);
                        }
                        else if (movement)
                        {
                            if (!string.IsNullOrEmpty(compIds))
                            {
                                var compIdList = compIds.Split(',');
                                if (compIdList != null)
                                {
                                    foreach (var item in compIdList)
                                    {
                                        var axleListByCompId = vehicleComponentService.ListAxleTemp(Convert.ToInt32(item), movement, SessionInfo.UserSchema);
                                        if (axleListByCompId != null)
                                            axleList.AddRange(axleListByCompId);
                                    }
                                }
                            }
                            else
                                axleList = vehicleComponentService.ListAxleTemp(componentId, movement, SessionInfo.UserSchema);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(compIds))
                            {
                                var compIdList = compIds.Split(',');
                                if (compIdList != null)
                                {
                                    foreach (var item in compIdList)
                                    {
                                        var axleListByCompId = vehicleComponentService.ListAxle(Convert.ToInt32(item));
                                        if (axleListByCompId != null)
                                            axleList.AddRange(axleListByCompId);
                                    }
                                }
                            }
                            else
                                axleList = vehicleComponentService.ListAxle(componentId);
                        }
                    }
                    else if (isVR1)
                    {
                        if (!string.IsNullOrEmpty(compIds))
                        {
                            var compIdList = compIds.Split(',');
                            if (compIdList != null)
                            {
                                foreach (var item in compIdList)
                                {
                                    var axleListByCompId = vehicleComponentService.ListVR1vehAxle(Convert.ToInt32(item), SessionInfo.UserSchema);
                                    if (axleListByCompId != null)
                                        axleList.AddRange(axleListByCompId);
                                }
                            }
                        }
                        else
                            axleList = vehicleComponentService.ListVR1vehAxle(componentId, SessionInfo.UserSchema);
                        

                        #region System events for edited_axle_details_for_vr1_component
                        if (axleList.Count > 0)
                        {
                            if (SessionInfo.UserSchema == UserSchema.Sort) // For SORT Vehicle Sort_edited_component_for_vr1_application Log
                            {
                                #region Saving Sort_created_new_vehicle_for_vr1_application
                                movactiontype.SystemEventType = STP.Domain.LoggingAndReporting.SysEventType.sort_edited_axle_details_for_vr1_component;
                                #endregion
                            }
                            else
                            {
                                movactiontype.SystemEventType = SysEventType.haulier_edited_axle_details_for_vr1_component;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(compIds))
                        {
                            var compIdList = compIds.Split(',');
                            if (compIdList != null)
                            {
                                foreach (var item in compIdList)
                                {
                                    var axleListByCompId = vehicleComponentService.ListAppvehAxle(Convert.ToInt32(item), SessionInfo.UserSchema);
                                    if (axleListByCompId != null)
                                        axleList.AddRange(axleListByCompId);
                                }
                            }
                        }
                        else
                            axleList = vehicleComponentService.ListAppvehAxle(componentId, SessionInfo.UserSchema);

                        #region System events for edited_axle_details_for_so_component
                        if (axleList.Count > 0)
                        {
                            if (SessionInfo.UserSchema == UserSchema.Sort) // For SORT Vehicle Sort_edited_component_for_so_application Log
                            {
                                #region Saving created_new_vehicle_for_so_application
                                movactiontype.SystemEventType = STP.Domain.LoggingAndReporting.SysEventType.sort_edited_axle_details_for_so_component;
                                #endregion
                            }
                            else
                            {
                                movactiontype.SystemEventType = SysEventType.haulier_edited_axle_details_for_so_component;
                            }
                        }
                        #endregion
                    }
                    if (movactiontype.SystemEventType != 0)
                    {
                        string sysEventDescp = STP.Domain.LoggingAndReporting.System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                        bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                    }

                    if (tyreEmpty && axleValidator.IsConfigureTyreCentreSpacing && axles != null && axles.Count > 0)
                    {
                        var query = from x in axleList
                                    join y in axles
                                        on x.AxleNumId equals y.AxleNumId
                                    select new { x, y };
                        foreach (var match in query)
                        {
                            if (match.y.AxleWeight != null && match.y.AxleWeight != match.x.AxleWeight)
                                match.x.AxleWeight = match.y.AxleWeight;
                            if (match.y.DistanceToNextAxle != null && match.y.DistanceToNextAxle != match.x.DistanceToNextAxle)
                                match.x.DistanceToNextAxle = match.y.DistanceToNextAxle;
                        }

                    }

                    ViewBag.AxleList = axleList;

                    if (isFromPopUp && axleList.Count != 0)
                    {
                        ViewBag.AxleCount = axleList.Count;
                    }
                    else if (!isFromPopUp && axleList.Count > 0)
                    {
                        ViewBag.AxleCount = axleList.Count;
                    }                   
                }
                else
                {
                    if (axles != null)
                        ViewBag.AxleList = axles;
                    else
                        ViewBag.AxleList = null;
                }

                if(IsEdit && (axles == null || axles.Count == 0 || (tyreEmpty && axleValidator.IsConfigureTyreCentreSpacing)) 
                    && (axleList==null || !axleList.Any()) && axles!=null && axles.Any())
                {
                    axleList = axles;
                    ViewBag.AxleList = axles;
                }

                //If axlelist empty, set default values - On Create New Vehicle
                axleList = GenerateAxleIfEmptyOnEdit(componentId, isFromConfig, movement, isCandidate, isEditMovement, GUID, SessionInfo, axleList);

                ViewBag.componentId = componentId;
                ViewBag.compIds = compIds;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/AxleComponent ActionResult method completed successfully"));
                return PartialView("~/Views/Vehicle/AxleComponent.cshtml", axleValidator);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}],Vehicle/AxleComponent, Exception: {1}", Session.SessionID, ex));
                throw ex;
            }
        }

        private List<Axle> GenerateAxleIfEmptyOnEdit(int componentId, int isFromConfig, bool movement, bool isCandidate, bool isEditMovement, string GUID, UserInfo SessionInfo, List<Axle> axleList)
        {
            if ((axleList == null || !axleList.Any())&&(ViewBag.AxleCount!=null && ViewBag.AxleCount==0))
            {
                if (componentId != 0)
                {
                    int compId = (int)componentId;
                    ComponentModel VehicleComponentObj = null;
                    if (isFromConfig == 1 || isEditMovement)
                    {
                        VehicleComponentObj = vehicleComponentService.GetComponentTemp(compId, GUID, SessionInfo.UserSchema);
                    }
                    else if (isCandidate)
                    {
                        VehicleComponentObj = vehicleComponentService.GetVR1VehicleComponent(compId, SessionInfo.UserSchema);
                    }
                    else if (movement)
                    {
                        VehicleComponentObj = vehicleComponentService.GetComponentTemp(compId, "", SessionInfo.UserSchema);
                    }
                    else
                    {
                        VehicleComponentObj = vehicleComponentService.GetVehicleComponent(compId);
                    }

                    if (VehicleComponentObj != null)
                    {
                        var noOfAxles = VehicleComponentObj.AxleCount;
                        if (noOfAxles > 0)
                        {
                            axleList = new List<Axle>();
                            for (int i = 0; i < noOfAxles; i++)
                            {
                                axleList.Add(new Axle() {AxleNumId=i+1 });
                            }
                            ViewBag.AxleCount = axleList.Count;
                            ViewBag.AxleList = axleList;
                        }
                    }
                }
            }

            return axleList;
        }

        public ActionResult RegistrationComponent(int compId, bool isTractor, int? vehicleTypeId, bool isApplication = false, bool isVR1 = false, int isFromConfig = 0, int vehicleConfigId = 0, bool movement = false, bool isCandidate = false, bool isEditMovement = false) //vehicleTypeId added as a parameter to remove registration Id for rigid vehicle component type.
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/RegistrationComponent ActionResult method started successfully, with parameters compId:{1}, isTractor:{2}, vehicleTypeId:{3}", Session.SessionID, compId, isTractor, vehicleTypeId));
                ViewBag.IsTractor = isTractor;
                ViewBag.VehicleTypeId = vehicleTypeId;
                ViewBag.MakeConfig = TempData["MakeConfig"];
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                List<VehicleRegistration> listVehclRegObj = null;
                if (!isApplication && !isVR1)
                {
                    if (isFromConfig == 1 || isEditMovement)
                    {
                        listVehclRegObj = vehicleComponentService.GetRegistrationTemp(compId, false, SessionInfo.UserSchema);
                    }
                    else if (isCandidate)
                    {
                        listVehclRegObj = vehicleComponentService.GetVR1RegistrationDetails(compId, SessionInfo.UserSchema);
                    }
                    else if (movement)
                    {
                        listVehclRegObj = vehicleComponentService.GetRegistrationTemp(compId, movement, SessionInfo.UserSchema);
                    }
                    else
                    {
                        listVehclRegObj = vehicleComponentService.GetRegistrationDetails(compId);
                    }
                }
                else if (isVR1)
                {
                    listVehclRegObj = vehicleComponentService.GetVR1RegistrationDetails(compId, SessionInfo.UserSchema);
                }
                else
                {
                    listVehclRegObj = vehicleComponentService.GetApplRegistrationDetails(compId, SessionInfo.UserSchema);
                }

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/RegistrationComponent ActionResult method completed successfully"));
                ViewBag.VehicleElementId = compId;
                return PartialView("~/Views/Vehicle/RegistrationComponent.cshtml", listVehclRegObj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}],Vehicle/RegistrationComponent, Exception: {1}", Session.SessionID, ex));
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult GetMovementAssessment(VehicleConfiguration vehicleConfigObj, int VehicleId = 0, ConfigurationModel configuration = null, int configTypeId = 0, bool isFleet = false, bool isNotifyVSO = false, int leadingComponentType = 0)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/GetMovementAssessment , Load view of ConfigurationGeneralPage", Session.SessionID));
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }
            VehicleMovementType vehicleMovementType = new SessionData().E4_AN_MovemenTypeClass;
            if (vehicleMovementType == null || (vehicleMovementType.VehicleClass != (int)VehicleClassificationType.VehicleSpecialOrder || !isNotifyVSO))
            {
                if (vehicleConfigObj != null)
                {
                    configuration = ConvertToConfigurationModel(vehicleConfigObj);
                }
                configuration.VehicleType = configTypeId;
                configuration.LeadingComponentType = leadingComponentType;

                if (configuration.TrailerAxleCount != null && configuration.TrailerAxleCount != 0)
                {
                    configuration.AxleCount = configuration.AxleCount + configuration.TrailerAxleCount;
                }
                AssessMoveTypeParams moveTypeParams = new AssessMoveTypeParams
                {
                    VehicleId = VehicleId,
                    IsRoute = 0,
                    UserSchema = SessionInfo.UserSchema,
                    PreviousMovementType = null,
                    ForceApplication = SessionInfo.UserSchema == UserSchema.Sort,
                    configuration = configuration
                };
                vehicleMovementType = vehicleconfigService.AssessMovementType(moveTypeParams);
            }
            ViewBag.MovementType = "";
            ViewBag.MovementTypeId = 0;
            if (vehicleMovementType.MovementType != 0)
            {
                VehicleClassificationType vehicleType = (VehicleClassificationType)vehicleMovementType.VehicleClass;
                string vehicleClassType = vehicleType.GetEnumDescription();
                VehicleCategoryToMovementTypeMapping movementType = (VehicleCategoryToMovementTypeMapping)STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleCategoryMapping(vehicleMovementType);
                int movmntTypeId = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleCategoryMapping(vehicleMovementType);

                string movementTypeDesc = "";
                if (isFleet|| SessionInfo.UserTypeId == UserType.Sort)
                {
                    movementTypeDesc = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleCategoryFleetMapping(movmntTypeId);
                }
                else
                {
                    movementTypeDesc = movementType.GetEnumDescription();
                }
                if (configuration.VehicleType == (int)ConfigurationType.BoatMast &&
                    (vehicleMovementType.MovementType == 207001 || vehicleMovementType.MovementType == 207004))
                {
                    movementTypeDesc = "Boat Mast Exception – Special Order Required";
                    movmntTypeId =270006; //special order
                }
                ViewBag.MovementType = movementTypeDesc;
                ViewBag.MovementTypeId = movmntTypeId;
                ViewBag.cvMovementType = vehicleMovementType.MovementType;
                ViewBag.cvVehicleClass = vehicleMovementType.VehicleClass;
            }
            return PartialView("~/Views/VehicleConfiguration/_Assessment.cshtml");
        }
        private ConfigurationModel ConvertToConfigurationModel(VehicleConfiguration vehicleConfigObj)
        {
            int unit = 1;
            ConfigurationModel configModelObj = new ConfigurationModel();
            foreach (var ifxProperty in vehicleConfigObj.VehicleConfigParamList)
            {
                switch (ifxProperty.ParamModel)
                {
                    case "Formal_Name":
                        configModelObj.FormalName = Convert.ToString(ifxProperty.ParamValue);
                        break;
                    case "Internal_Name":
                        configModelObj.InternalName = Convert.ToString(ifxProperty.ParamValue);
                        break;
                    case "Notes":
                        configModelObj.Description = Convert.ToString(ifxProperty.ParamValue);
                        break;
                    case "Length":
                        configModelObj.RigidLength = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "Weight":
                        configModelObj.GrossWeight = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "HeaviestComponentWeight":
                        if (configModelObj.GrossWeight == 0)
                        {
                            configModelObj.GrossWeight = Convert.ToDouble(ifxProperty.ParamValue);
                        }
                        break;
                    case "OverallLength":
                        configModelObj.OverallLength = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "Width":
                        configModelObj.Width = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "WheelBase":
                        configModelObj.WheelBase = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "Maximum Height":
                        configModelObj.MaxHeight = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "Maximum_Height":
                        configModelObj.MaxHeight = Convert.ToDouble(ifxProperty.ParamValue);
                        break;                        
                    case "AxleWeight":
                        configModelObj.MaxAxleWeight = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "Speed":
                        configModelObj.TravellingSpeed = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "Tyre_Spacing":
                        configModelObj.TyreSpacing = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "Left Overhang":
                    case "Left_Overhang":
                        configModelObj.NotifLeftOverhang = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "Right Overhang":
                    case "Right_Overhang":
                        configModelObj.NotifRightOverhang = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "Front Overhang":
                    case "Front_Overhang":
                        configModelObj.NotifFrontOverhang = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "Rear Overhang":
                    case "Rear_Overhang":
                        configModelObj.NotifRearOverhang = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "TractorWeight":
                        configModelObj.TractorWeight = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "TrailerWeight":
                        configModelObj.TrailerWeight = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "Number of Axles":
                        if (ifxProperty.ParamValue != "Select")
                        {
                            configModelObj.AxleCount = Convert.ToInt32(ifxProperty.ParamValue);
                        }
                        break;
                    case "Number of Axles for Trailer":
                        configModelObj.TrailerAxleCount = Convert.ToInt32(ifxProperty.ParamValue);
                        break;
                    case "Wheelbase":
                        configModelObj.WheelBase = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    default:
                        break;
                }
            }
            return configModelObj;
        }

        public ActionResult VehicleOverview(int vehicleId, string Guid = "", bool isMovement = false, int vehicleConfigId = 0, bool isCandidate = false, int movementTypeId = 0, int isEdit = 0, bool isEditMovement = false)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/VehicleOverview , Load partial view of CreateConfiguration", Session.SessionID));
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }

            ViewBag.VehicleId = vehicleId;
            ViewBag.IsEdit = isEdit;
            ViewBag.IsMovement = isMovement;
            ViewBag.Guid = Guid;
            ViewBag.vehicleConfigId = vehicleConfigId;
            ViewBag.isCandidate = isCandidate;
            ViewBag.movementTypeId = movementTypeId;
            ViewBag.isEditMovement = isEditMovement; 

            return PartialView("~/Views/VehicleConfiguration/_VehicleOverview.cshtml");
        }

        public ActionResult ViewConfigDetail(int vehicleID, bool isRoute = false, int movementId = 0, bool isImportConfiguration = false, bool isNotif = false, string flag = "", bool isPolice = false, bool ImportBtn = false, bool isSort = false, bool IsNEN = false, bool isMovement = false, int NotificationEditFlag = 0, bool isOverviewDisplay = false, bool isCandidate = false)
        {
            try
            {
                ViewBag.IsNEN = IsNEN;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/ViewConfigDetail , View Vehicle Configuration", Session.SessionID));
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                if (isSort)
                {
                    SessionInfo.UserSchema = UserSchema.Sort;
                }
                ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
                ConfigurationModel VehicleConfig = null;
                if (isCandidate)
                {
                    isNotif = true;
                    flag = "candidatevehicle";
                    isMovement = false;
                }
                if (Session["AppFlag"] != null && flag.ToLower() != "candidatevehicle")
                    flag = Convert.ToString(Session["AppFlag"]);
                if (Session["IsRoute"] != null)
                    isRoute = Convert.ToBoolean(Session["IsRoute"]);
                if (Session["IsNotif"] != null)
                    isNotif = Convert.ToBoolean(Session["IsNotif"]);

                bool isVR1 = false;
                VehicleConfig = GetVehicleDataById(vehicleID, isRoute, isMovement, flag, SessionInfo.UserSchema, isNotif, IsNEN, out isVR1);
                if (VehicleConfig.MovementClassificationId == 0)
                {
                    VehicleConfig.MovementClassificationId = movementId;
                }
                ViewBag.TravelSpeed = VehicleConfig.TravellingSpeedUnit;

                MovementClassificationConfig mvClassConfig = new MovementClassificationConfig();
                
                VehicleConfiguration vehicleConfigObj = compConfigObj.GetVehicleConfiguration(VehicleConfig.ConfigurationTypeId);

                if (vehicleConfigObj != null)
                {
                    vehicleConfigObj.UpdateConfigProperties(VehicleConfig);
                }

                ViewBag.IsRoute = isRoute;
                if (isRoute && !isMovement)
                {
                    if (flag == "SOApp" || flag == "")
                    {
                        vehicleConfigObj.VehicleRegList = vehicleconfigService.GetRouteVehicleRegistrationDetails(vehicleID, SessionInfo.UserSchema);
                    }
                    else
                    {
                        vehicleConfigObj.VehicleRegList = vehicleconfigService.GetVR1VehicleRegistrationDetails(vehicleID, SessionInfo.UserSchema);
                    }
                }
                else if (isNotif && !isMovement)
                {
                    vehicleConfigObj.VehicleRegList = vehicleconfigService.GetVR1VehicleRegistrationDetails(vehicleID, SessionInfo.UserSchema);
                }
                else if (isMovement)
                {
                    vehicleConfigObj.VehicleRegList = vehicleconfigService.GetMovementVehicleRegDetails(vehicleID, SessionInfo.UserSchema);
                }
                else
                {
                    vehicleConfigObj.VehicleRegList = vehicleconfigService.GetVehicleRegistrationDetails(vehicleID, SessionInfo.UserSchema);
                }
                ViewBag.isImportConfiguration = isImportConfiguration;
                ViewBag.isNotif = isNotif;
                ViewBag.IsPolice = isPolice;
                ViewBag.ImportBtn = ImportBtn;
                ViewBag.vehicleID = vehicleID;
                ViewBag.IsRoute = isRoute;

                ViewBag.isVR1 = isVR1;
                ViewBag.IsMovement = isMovement;
                ViewBag.NotificationEditFlag = NotificationEditFlag;
                ViewBag.OverviewDisplayVehicleId = vehicleID;
                ViewBag.IsOverviewDisplay = isOverviewDisplay;
                ViewBag.componentTypeList = Session["componentIdList"];
                ViewBag.IsCandidate = isCandidate;
                VehicleConfiguration vehicleConfigObjTempData = null;
                if (TempData["VehicleConfigModel"] != null)
                {
                    vehicleConfigObjTempData = (VehicleConfiguration)TempData["VehicleConfigModel"];
                }
                ViewBag.VehicleConfigObjTempData = vehicleConfigObjTempData;
                return PartialView("~/Views/VehicleConfiguration/_VehicleConfigDetail.cshtml", vehicleConfigObj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] ,VehicleConfigController/ViewConfigDetail, Exception: {1}", Session.SessionID, ex.Message));
                throw;
            }
        }
        public JsonResult SaveVehicleComponents(VehicleComponentDetail vehicleComponentDetail, bool isMovement = false, bool isCandidate = false, int isEdit = 0)
        {
            UserInfo SessionInfo = new UserInfo();
            long result = 0;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }
            int flag = 1; 
            if (isCandidate)
            {
                flag = 3;
            }
            else if (isMovement)
            {
                flag = 2;
            }
            int organisationId = (int)SessionInfo.OrganisationId;
            
            List<VehicleComponentModel> componentList = new List<VehicleComponentModel>();

            foreach (var compList in vehicleComponentDetail.ComponentDetailList)
            {
                VehicleComponentModel componentObj = new VehicleComponentModel();
                componentObj.ComponentId = compList.ComponentId;
                componentObj.ComponentType = compList.ComponentTypeId;
                componentObj.ComponentSubType = compList.ComponentSubTypeId;
                componentObj.GUID = compList.Guid;
                componentObj = ConvertToComponent(compList.vehicleComponent, componentObj);
                componentObj.ComponentAxleList = compList.ComponentAxleList;
                componentObj.ComponentRegistrationList = compList.ComponentRegistrationList;
                componentList.Add(componentObj);
            }

            VehicleConfigDetail vehicleConfigDetail = new VehicleConfigDetail()
            {
                ConfigurationId = vehicleComponentDetail.ConfigurationId,
                MovementTypeId = vehicleComponentDetail.MovemnetTypeId,
                Flag = flag,
                componentList = componentList,
                UserSchema = SessionInfo.UserSchema
            };

            result = isEdit == 1
                ? vehicleconfigService.UpdateVehicleComponents(vehicleConfigDetail)
                : vehicleconfigService.SaveVehicleComponents(vehicleConfigDetail);
            if (result == 1&&!isMovement)
            {
                Session["g_VehicleConfigSearch"] = null;
                Session["g_VehicleConfigTypeSearch"] = null;
                Session["movementClassificationName"] = null;
                Session["movementClassificationId"] = null;
            }
            TempData["VehicleConfigModel"] = null;
            return Json(new { result = result });
        }
        private VehicleComponentModel ConvertToComponent(VehicleComponent vehicleComponent, VehicleComponentModel componentObj)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/ConvertToComponent  method started successfully with parameters VehicleComponent object"));
            try
            {
                int unit = 1;
                int unitWeight = 1;
                Domain.VehiclesAndFleets.Vehicles.VehicleEnumConversions vehicleEnumConversions = new Domain.VehiclesAndFleets.Vehicles.VehicleEnumConversions();
                foreach (var ifxProperty in vehicleComponent.VehicleParamList)
                {
                    switch (ifxProperty.ParamModel)
                    {
                        case "Formal_Name":
                            componentObj.FormalName = ifxProperty.ParamValue;
                            break;
                        case "Internal_Name":
                            componentObj.IntendedName = ifxProperty.ParamValue;
                            break;
                        case "Notes":
                            componentObj.Description = ifxProperty.ParamValue;
                            break;
                        case "Component_Type":
                            if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                                componentObj.ComponentSubType = Convert.ToInt32(ifxProperty.ParamValue);
                            break;
                        case "Number_of_Axles":
                            if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                                if (ifxProperty.ParamValue == "Select")
                                { componentObj.AxleCount = null; }
                                else
                                {
                                    componentObj.AxleCount = Convert.ToInt16(ifxProperty.ParamValue);
                                }
                            break;
                        case "Coupling":
                            if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                                //componentObj.CouplingType = Convert.ToInt32(ifxProperty.ParamValue);
                                componentObj.CouplingType = vehicleEnumConversions.GetCouplingTypeNumber(ifxProperty.ParamValue);
                            break;
                        case "Maximum_Height":
                            if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                                componentObj.MaxHeight = Convert.ToDouble(ifxProperty.ParamValue);
                            componentObj.MaxHeightUnit = unit;
                            break;
                        case "Reducable_Height":
                            if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                                componentObj.ReducableHeight = Convert.ToDouble(ifxProperty.ParamValue);
                            componentObj.ReducableHeightUnit = unit;
                            break;
                        case "Ground_Clearance":
                            if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                                componentObj.GroundClearance = Convert.ToDouble(ifxProperty.ParamValue);
                            componentObj.GroundClearanceUnit = unit;
                            break;
                        case "Reduced_Ground_Clearance":
                            if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                                componentObj.RedGroundClearance = Convert.ToDouble(ifxProperty.ParamValue);
                            componentObj.RedGroundClearanceUnit = unit;
                            break;
                        case "Length":
                            if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                                componentObj.RigidLength = Convert.ToDouble(ifxProperty.ParamValue);
                            componentObj.RigidLengthUnit = unit;
                            break;
                        case "Width":
                            if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                                componentObj.Width = Convert.ToDouble(ifxProperty.ParamValue);
                            componentObj.WidthUnit = unit;
                            break;
                        case "Left_Overhang":
                            if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                                componentObj.LeftOverhang = Convert.ToDouble(ifxProperty.ParamValue);
                            componentObj.LeftOverhangUnit = unit;
                            break;
                        case "Right_Overhang":
                            if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                                componentObj.RightOverhang = Convert.ToDouble(ifxProperty.ParamValue);
                            componentObj.RightOverhangUnit = unit;
                            break;
                        case "Front_Overhang":
                            if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                                componentObj.FrontOverhang = Convert.ToDouble(ifxProperty.ParamValue);
                            componentObj.FrontOverhangUnit = unit;
                            break;
                        case "Rear_Overhang":
                            if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                                componentObj.RearOverhang = Convert.ToDouble(ifxProperty.ParamValue);
                            componentObj.RearOverhangUnit = unit;
                            break;
                        case "Outside_Track":
                            if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                                componentObj.OutsideTrack = Convert.ToDouble(ifxProperty.ParamValue);
                            componentObj.OutsideTrackUnit = unit;
                            break;
                        case "Wheelbase":
                            if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                                componentObj.WheelBase = Convert.ToDouble(ifxProperty.ParamValue);
                            componentObj.WheelBaseUnit = unit;
                            break;
                        case "conformsCU":
                            componentObj.StandardCU = ConvertBooleanToInt(ifxProperty.ParamValue);
                            break;
                        case "Weight":
                            if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                                componentObj.GrossWeight = Convert.ToDouble(ifxProperty.ParamValue);
                            componentObj.GrossWeightUnit = unitWeight;
                            break;
                        case "Axle_Spacing_To_Following":
                            if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                                componentObj.SpaceToFollowing = Convert.ToDouble(ifxProperty.ParamValue);
                            componentObj.SpaceToFollowingUnit = unit;
                            break;
                        case "Rear_Steer":
                            if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                                componentObj.IsSteerable = ConvertBooleanToInt(ifxProperty.ParamValue);
                            break;

                        default:
                            break;
                    }
                }

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/ConvertToComponent  method completed successfully"));
                return componentObj;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Vehicle/ConvertToComponent, Exception: {0}", ex));
                throw ex;
            }
        }
        private int ConvertBooleanToInt(string value)
        {
            if (value == "true")
                return 1;
            else
                return 0;
        }

        #region public ActionResult EditConfiguration()
        //Method for showing partial view of CreateConfiguration
        public ActionResult EditConfiguration(int vehicleId, bool isVR1 = false, bool isApplication = false, bool isEditVehicleInSoProcessing = false, bool isCandidate = false)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , JsonResult,VehicleConfigController/GetMovementId , Get movement id for vehicle - {1}", Session.SessionID, vehicleId));
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }
            ViewBag.IsApplication = isApplication;
            ConfigurationModel VehicleConfig;
            if (isEditVehicleInSoProcessing)
            {
                VehicleConfig = vehicleconfigService.GetRouteConfigInfoForVR1(vehicleId, SessionInfo.UserSchema);
            }
            else
            {
                VehicleConfig = vehicleconfigService.GetVehicleDetails(vehicleId, isApplication, SessionInfo.UserSchema);
            }


            ViewBag.movementId = VehicleConfig.MovementClassificationId;
            ViewBag.vehicleTypeId = VehicleConfig.ConfigurationTypeId;
            ViewBag.vehicleId = vehicleId;
            ViewBag.isCandidate = isCandidate;
            ViewBag.isEditMovement = isApplication;            

            AssignMovementClassification(VehicleConfig.MovementClassificationId);

            ViewBag.movementClassificationName = Session["movementClassificationName"];
            return View("~/Views/VehicleConfiguration/EditVehicleConfig.cshtml");
        }
        #endregion
        #region public JsonResult UpdateVehicleConfiguration(VehicleConfiguration vehicleConfigObj, int configTypeId, int vehicleId)
        public JsonResult UpdateVehicleConfiguration(VehicleConfiguration vehicleConfigObj, int configTypeId, int vehicleId, int? speedUnit = null, List<RegistrationParams> registrationParams = null, bool planMovement = false, bool isCandidate = false,int movementClassificationId=0)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , JsonResult,VehicleConfigController/UpdateVehicleConfiguration , Update vehicle config for id - {1}", Session.SessionID, vehicleId));
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }
            bool Success = false;
            TempData["VehicleConfigModel"] = vehicleConfigObj;
            try
            {
                int organisationId;
                if (SessionInfo.UserTypeId == 696008)
                {
                    organisationId = (int)Session["SORTOrgID"];
                }
                else
                {
                    organisationId = (int)SessionInfo.OrganisationId;
                }

                NewConfigurationModel vehicleConfiguration = ConvertToConfiguration(vehicleConfigObj);
                vehicleConfiguration.OrganisationId = organisationId;
                vehicleConfiguration.VehicleType = configTypeId;
                if (vehicleConfigObj.moveClassification != null)
                    vehicleConfiguration.VehiclePurpose = vehicleConfigObj.moveClassification.ClassificationId;
                else if(movementClassificationId!=0)
                    vehicleConfiguration.VehiclePurpose = movementClassificationId;
                vehicleConfiguration.VehicleId = vehicleId;
                vehicleConfiguration.SpeedUnit = speedUnit;
                if (isCandidate)
                {
                    Success = vehicleconfigService.UpdateVR1vehicleconfig(vehicleConfiguration, SessionInfo.UserSchema);
                }
                else if (planMovement)
                {
                    Success = vehicleconfigService.UpdateMovementVehicle(vehicleConfiguration, SessionInfo.UserSchema);
                }
                else
                {
                    Success = vehicleconfigService.Updatevehicleconfig(vehicleConfiguration);
                }
                if (Success)
                {
                    int IdNumber = 0;
                    if (vehicleId > 0)
                    {
                        if (registrationParams != null)
                        {
                            var listVehclRegObjExisting = (planMovement) ?
                                vehicleconfigService.GetVehicleRegistrationTemp(vehicleId, SessionInfo.UserSchema) :
                                vehicleconfigService.GetVehicleRegistrationDetails(vehicleId, SessionInfo.UserSchema);

                            foreach (RegistrationParams registration in registrationParams)
                            {
                                if (isCandidate)
                                {
                                    IdNumber = vehicleconfigService.SaveVR1VehicleRegistration(vehicleId, registration.RegistrationValue, registration.FleetId, SessionInfo.UserSchema);
                                }
                                else if (planMovement)
                                {
                                    IdNumber = vehicleconfigService.CreateVehicleRegistrationTemp(vehicleId, registration.RegistrationValue, registration.FleetId, SessionInfo.UserSchema);
                                }
                                else
                                {
                                    IdNumber = vehicleconfigService.CreateVehicleRegistration(vehicleId, registration.RegistrationValue, registration.FleetId);
                                }
                            }

                            //To Fix vehicle data lost scenario RegNo duplicate issues
                            if (listVehclRegObjExisting != null && listVehclRegObjExisting.Any() && registrationParams!=null)
                            {
                                foreach(var item in listVehclRegObjExisting)
                                {
                                    item.FleetId = item.FleetId == string.Empty ? null : item.FleetId;
                                    item.RegistrationId = item.RegistrationId == string.Empty ? null : item.RegistrationId;
                                    var isExistInNew = registrationParams.Any(x => x.FleetId == item.FleetId && x.RegistrationValue == item.RegistrationId);
                                    if (!isExistInNew)
                                    {
                                        if (isCandidate)
                                            vehicleconfigService.DeleteVR1RegConfig(vehicleId, item.IdNumber, SessionInfo.UserSchema);
                                        else if (planMovement)
                                            vehicleconfigService.DeletVehicleRegisterConfiguration(vehicleId, item.IdNumber, planMovement);
                                        else
                                            vehicleconfigService.DeletVehicleRegisterConfiguration(vehicleId, item.IdNumber);
                                    }
                                }
                            }

                        }
                    }
                    #region System Event Log - haulier_edited_fleet_vehicle
                    string ErrMsg = string.Empty;
                    string sysEventDescp = string.Empty;

                    MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                    movactiontype.UserName = SessionInfo.UserName;
                    movactiontype.FleetVehicleId = (long)vehicleConfiguration.VehicleId;
                    movactiontype.FleetVehicleName = vehicleConfiguration.VehicleName;
                    movactiontype.SystemEventType = SysEventType.haulier_edited_fleet_vehicle;

                    sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);

                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] , VehicleConfigController/UpdateVehicleConfiguration, Exception: {1}", Session.SessionID, ex.Message));
                throw ex;
            }
            //  return Json(new { configId = configurationid });
            return Json(new { Success });
        }
        #endregion public JsonResult UpdateVehicleConfiguration(VehicleConfiguration vehicleConfigObj, int configTypeId, int vehicleId)
        public List<VehicleConfigurationGridList> GetVehicleList(string intendedUse, List<VehicleConfigurationGridList> vehicleConfigurationGridLists)
        {

            List<VehicleConfigurationGridList> VehicleConfigGridListObj = new List<VehicleConfigurationGridList>();
            List<int> vehiclePurpose = new List<int>();
            if (intendedUse.ToLower() == "stgo ail")
            {
                vehiclePurpose = Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleIntentedUseMapping(VehiclePurpose.Stgoail);
            }
            else if (intendedUse.ToLower() == "c and u")
            {
                vehiclePurpose = Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleIntentedUseMapping(VehiclePurpose.WheeledConstructionAndUse);
            }
            else if (intendedUse.ToLower() == "stgo mobile crane")
            {
                vehiclePurpose = Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleIntentedUseMapping(VehiclePurpose.StgoMobileCrane);
            }
            else if (intendedUse.ToLower() == "special order")
            {
                vehiclePurpose = Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleIntentedUseMapping(VehiclePurpose.SpecialOrder);
            }
            else if (intendedUse.ToLower() == "vehicle special order")
            {
                vehiclePurpose = Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleIntentedUseMapping(VehiclePurpose.VehicleSpecialOrder);
            }
            else if (intendedUse.ToLower() == "tracked")
            {
                vehiclePurpose = Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleIntentedUseMapping(VehiclePurpose.Tracked);

            }
            else if (intendedUse.ToLower() == "stgo engineering plant wheeled" || intendedUse.ToLower() == "stgo road recovery")
            {
                VehicleConfigGridListObj = (from s in vehicleConfigurationGridLists
                                            where s.IndendedUse.ToLower() == (intendedUse.ToLower())
                                            select s).ToList();
            }
            if (vehiclePurpose.Count != 0)
            {
                VehicleConfigGridListObj = (from s in vehicleConfigurationGridLists
                                            where vehiclePurpose.Contains(s.VehiclePurpose)
                                            select s).ToList();
            }

            return VehicleConfigGridListObj;
        }
        private ConfigurationModel GetVehicleDataById(int vehicleID, bool isRoute, bool isMovement, string flag, string userSchema, bool isNotif, bool IsNEN, out bool isVR1)
        {
            ConfigurationModel VehicleConfig = null;
            isVR1 = false;
            if (isRoute && !isMovement)
            {
                if (flag.ToLower() == "soapp" || flag.Length == 0)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfigDetail , View SO Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));

                    VehicleConfig = vehicleconfigService.GetRouteConfigInfo(vehicleID, userSchema);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfigDetail , View VR1 Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                    VehicleConfig = vehicleconfigService.GetRouteConfigInfoForVR1(vehicleID, userSchema);
                    if (flag.ToLower() == "candidatevehicle" || flag.ToLower() == "vr1app")
                        isVR1 = true;
                }
            }
            else if ((isNotif || IsNEN) && !isMovement)
            {
                if (flag == "Notif" || IsNEN)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfigDetail , View Notif Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                    VehicleConfig = vehicleconfigService.GetRouteConfigInfoForVR1(vehicleID, userSchema);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfigDetail , View Notif Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                    if (flag.ToLower() == "candidatevehicle")
                    {
                        VehicleConfig = vehicleconfigService.GetRouteConfigInfoForVR1(vehicleID, userSchema);
                    }
                    else
                    {
                        VehicleConfig = vehicleconfigService.GetNotifVehicleConfigByID(vehicleID);
                        vehicleID = (int)VehicleConfig.ConfigurationId;
                    }
                }
            }
            else if (isMovement)
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfigDetail , View Movement Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                VehicleConfig = vehicleconfigService.GetMovementConfigInfo(vehicleID, userSchema);
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] ,VehicleConfigController/ViewConfigDetail , View fleet Vehicle Configuration with id - {1}", Session.SessionID, vehicleID));
                VehicleConfig = vehicleconfigService.GetVehicleDetails(vehicleID, false, userSchema);
            }
            return VehicleConfig;
        }
        [HttpPost]
        public JsonResult GenerateVehicleConfiguration(int vehicleID, VehicleComponentDetail vehicleComponentDetail, bool isRoute = false, bool isMovement = false, string flag = "", bool isNotif = false, bool IsNEN = false, bool isVR1 = false,bool isCandidate=false)
        {
            UserInfo SessionInfo = null;
            SessionInfo = (UserInfo)Session["UserInfo"];
            if (isCandidate)
            {
                isNotif = true;
                flag = "candidatevehicle";
                isMovement = false;
            }
            int configTypeId = 0;
            ConfigurationModel vehicleConfig = GetVehicleDataById(vehicleID, isRoute, isMovement, flag, SessionInfo.UserSchema, isNotif, IsNEN, out isVR1);
            var componemtModelList = vehicleComponentDetail.ComponentDetailList;
            List<VehicleComponentModel> vehicleComponents = new List<VehicleComponentModel>();
            configTypeId = vehicleConfig.ConfigurationTypeId;
            foreach (var item in componemtModelList)
            {
                if (item.vehicleComponent.VehicleParamList != null)
                {
                    VehicleComponentModel vehicleComponentModelTemp = new VehicleComponentModel();
                    VehicleComponentModel vehicleComponentModel = ConvertToComponent(item.vehicleComponent, vehicleComponentModelTemp);
                    vehicleComponents.Add(vehicleComponentModel);
                }
            }
            VehicleConfiguration vehicleConfigObj = null;
            if (TempData["VehicleConfigModel"] != null)
            {
                vehicleConfigObj = (VehicleConfiguration)TempData["VehicleConfigModel"];
                vehicleConfig = ConvertToConfigurationModel(vehicleConfigObj);
            }

            var maxAxleWeightComponent = vehicleComponents.Select(x => x.MaxAxleWeight).Max();
            var maxAxleWeightVehicle = vehicleConfig.MaxAxleWeight == null ? 0 : vehicleConfig.MaxAxleWeight;

            var axleCountComponent = vehicleComponents.Select(x => x.AxleCount).Sum();
            var axleCountVehicle = vehicleConfig.AxleCount == null ? 0 : vehicleConfig.AxleCount;

            if (configTypeId == (int)ConfigurationType.DrawbarTrailer || configTypeId == (int)ConfigurationType.DrawbarTrailer_3_8)
            {
                int heaviestComponentIndex = vehicleComponents.Select((x, index) => (x.GrossWeight, index)).Max().index;
                maxAxleWeightComponent = vehicleComponents[heaviestComponentIndex].MaxAxleWeight;
                axleCountComponent = vehicleComponents[heaviestComponentIndex].AxleCount;
                if(maxAxleWeightComponent!=null)
                    vehicleConfig.MaxAxleWeight = maxAxleWeightComponent;

                if (axleCountComponent != null) 
                    vehicleConfig.AxleCount = axleCountComponent ;
            }
            else
            {
                vehicleConfig.MaxAxleWeight = maxAxleWeightComponent > maxAxleWeightVehicle ? maxAxleWeightComponent : maxAxleWeightVehicle;
                vehicleConfig.AxleCount = axleCountComponent > axleCountVehicle ? axleCountComponent : axleCountVehicle;
            }

            var leftOverhangComponent = vehicleComponents.Select(x => x.LeftOverhang).Max();
            var leftOverhangVehicle = vehicleConfig.NotifLeftOverhang == null ? 0 : vehicleConfig.NotifLeftOverhang;
            vehicleConfig.NotifLeftOverhang = leftOverhangComponent > leftOverhangVehicle ? leftOverhangComponent : leftOverhangVehicle;
            var rightOverhangComponent = vehicleComponents.Select(x => x.RightOverhang).Max();
            var rightOverhangVehicle = vehicleConfig.NotifRightOverhang == null ? 0 : vehicleConfig.NotifRightOverhang;
            vehicleConfig.NotifRightOverhang = rightOverhangComponent > rightOverhangVehicle ? rightOverhangComponent : rightOverhangVehicle;
            var frontOverhangComponent = vehicleComponents.Select(x => x.FrontOverhang).Max();
            var frontOverhangVehicle = vehicleConfig.NotifFrontOverhang == null ? 0 : vehicleConfig.NotifFrontOverhang;
            vehicleConfig.NotifFrontOverhang = frontOverhangComponent > frontOverhangVehicle ? frontOverhangComponent : frontOverhangVehicle;
            var rearOverhangComponent = vehicleComponents.Select(x => x.RearOverhang).Max();
            var rearOverhangVehicle = vehicleConfig.NotifRearOverhang == null ? 0 : vehicleConfig.NotifRearOverhang;
            vehicleConfig.NotifRearOverhang = rearOverhangComponent > rearOverhangVehicle ? rearOverhangComponent : rearOverhangVehicle;

            var wieghtComponent = vehicleComponents.Select(x => x.GrossWeight).Max();
            var wieghtVehicle = vehicleConfig.GrossWeight == null ? 0 : vehicleConfig.GrossWeight;
            if(vehicleConfig.GrossWeight==null)
                vehicleConfig.GrossWeight = wieghtComponent > wieghtVehicle ? wieghtComponent : wieghtVehicle;
            var widthComponent = vehicleComponents.Select(x => x.Width).Max();
            var widthVehicle = vehicleConfig.Width == null ? 0 : vehicleConfig.Width;
            vehicleConfig.Width = widthComponent > widthVehicle ? widthComponent : widthVehicle;
            var heightComponent = vehicleComponents.Select(x => x.MaxHeight).Max();
            var heightVehicle = vehicleConfig.MaxHeight == null ? 0 : vehicleConfig.MaxHeight;
            vehicleConfig.MaxHeight = heightComponent > heightVehicle ? heightComponent : heightVehicle;

            var wheelbaseComponent = vehicleComponents.Select(x => x.WheelBase).Max();
            var wheelbaseVehicle = vehicleConfig.WheelBase == null ? 0 : vehicleConfig.WheelBase;
            vehicleConfig.WheelBase = wheelbaseComponent > wheelbaseVehicle ? wheelbaseComponent : wheelbaseVehicle;

            vehicleConfig.RigidLength = vehicleConfig.RigidLength == 0 ? null : vehicleConfig.RigidLength;

            var lengthComponent = vehicleComponents.Select(x => x.RigidLength).Sum();
            var lengthVehicle = vehicleConfig.OverallLength == null ? 0 : vehicleConfig.OverallLength;
            vehicleConfig.OverallLength = lengthComponent > lengthVehicle ? lengthComponent : lengthVehicle;

            if (configTypeId == 244008)
            {
                //vehicleConfig.TrailerWeight= vehicleComponents.Select(x => x.GrossWeight).Sum();
                vehicleConfig.TrailerWeight = vehicleComponents[1].GrossWeight;
            }
            //return RedirectToAction("GetMovementAssessment",new { VehicleId = vehicleID, configuration= vehicleConfig });
                return Json(new { VehicleId = vehicleID, configuration = vehicleConfig });

        }
        
        public JsonResult GetVehicleRegistration(int[] vehicleIds,int notificationId=0)
        {
            int count = 0;
            UserInfo SessionInfo = null;
            SessionInfo = (UserInfo)Session["UserInfo"];
            VehicleConfiguration vehicleConfigObj = new VehicleConfiguration();
            if (vehicleIds != null)
            {
                foreach (var vehicleId in vehicleIds)
                {
                    vehicleConfigObj.VehicleRegList = vehicleconfigService.GetMovementVehicleRegDetails(vehicleId, SessionInfo.UserSchema);
                    count = vehicleConfigObj.VehicleRegList.Count;
                    if (count == 0)
                    {
                        Session["AmendVehicle"] = true;
                        break;
                    }

                }
            }
            else if (notificationId != 0)
            {
                count = 1;
            }
            return Json(new { Count= count }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetNextDefaultComponent(int componentType, int subComponentType=0, int vehicleConfigId=0,bool isMovement=false,int organisationId=0,string guId="",int componentCount=0,int ConfigTypeId=0)
        {
            int defaultType = 0;
            if(componentCount == 0 || componentType == (int)ComponentType.GirderSet || 
                (
                   componentCount <= 2 && ConfigTypeId != (int)ConfigurationType.RecoveryVehicle && componentType == (int)ComponentType.EngineeringPlant && 
                    (subComponentType == (int)ComponentSubType.EngPlantConventionalTractor||
                    subComponentType == (int)ComponentSubType.EngPlantBallastTractor)
                    )
                )
                defaultType = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.DefaultComponentType(componentType, subComponentType, componentCount);
            if (defaultType != 0)
            {
                int defaultSubType = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.DefaultComponentSubType(defaultType);
                return AddComponent(vehicleConfigId, defaultType, defaultSubType, isMovement, organisationId, guId);
            }
            else
            {
                return null;
            }
        }

        public JsonResult CheckComponentName(string componentName, int organisationId = 0)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , JsonResult,VehicleConfigController/CheckComponentName , Check for internal name ", Session.SessionID));
            UserInfo SessionInfo = null;

            SessionInfo = (UserInfo)Session["UserInfo"];

            if (SessionInfo.UserTypeId != UserType.Sort)
            {
                organisationId = (int)SessionInfo.OrganisationId;
            }
            int result = vehicleComponentService.CheckComponentInternalnameExists(componentName, organisationId);

            return Json(new { success = result });
        }

        public JsonResult AddComponentToFleetLibrary(ComponentDetail componentDetail,int movementTypeId)
        {
            long result = 0;
            int organisationId = 0;
            UserInfo SessionInfo = null;
            SessionInfo = (UserInfo)Session["UserInfo"];
            if (SessionInfo.UserTypeId != UserType.Sort)
            {
                organisationId = (int)SessionInfo.OrganisationId;
            }
            VehicleComponentModel componentObj = new VehicleComponentModel();
            componentObj.OrganisationId = organisationId;
            componentObj.ComponentId = componentDetail.ComponentId;
            componentObj.GUID = componentDetail.Guid;
            componentObj.VehicleIntent = movementTypeId;
            componentObj.ShowComponent = 1;
            componentObj = ConvertToComponent(componentDetail.vehicleComponent, componentObj);
            List<VehicleComponentModel> componentList = new List<VehicleComponentModel>();
            componentList.Add(componentObj);
            result = vehicleconfigService.AddComponentToFleetLibrary(componentList);
            
            return Json(new { result = result });
        }

        public ActionResult GetVehicleFilteredCombinations(VehicleConfiguration vehicleConfigObj, int configTypeId=0,int movementTypeId=0)
        {
            List<VehicleConfigurationGridList> configList;
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            int organisationId = (int)SessionInfo.OrganisationId;
            if (SessionInfo.UserTypeId == UserType.Sort)
            {
                organisationId = (int)Session["SORTOrgID"];
            }

            ConfigurationModel VehicleConfig = ConvertToConfig(vehicleConfigObj);
            VehicleConfig.OrganisationId = organisationId;
            VehicleConfig.ConfigurationTypeId = configTypeId;
            VehicleConfig.VehicleType = configTypeId;
            //if (movementTypeId != 0)
            //{
            //    VehicleConfig.VehiclePurpose = movementTypeId;
            //}
            configList = vehicleconfigService.GetFilteredVehicleCombinations(VehicleConfig);
            ViewBag.FilteredVehicle = true;
            return PartialView("~/Views/Movements/_SimilarVehicleCombinations.cshtml", configList);
        }

        private ConfigurationModel ConvertToConfig(VehicleConfiguration vehicleConfigObj)
        {
            ConfigurationModel configModelObj = new ConfigurationModel();
            foreach (var ifxProperty in vehicleConfigObj.VehicleConfigParamList)
            {
                switch (ifxProperty.ParamModel)
                {
                    case "Formal_Name":
                        configModelObj.FormalName = Convert.ToString(ifxProperty.ParamValue);
                        break;
                    case "Internal_Name":
                        configModelObj.InternalName = Convert.ToString(ifxProperty.ParamValue);
                        break;
                    case "Notes":
                        configModelObj.Description = Convert.ToString(ifxProperty.ParamValue);
                        break;
                    case "Length":
                        if (ifxProperty.ParamValue != null)
                            configModelObj.RigidLength = Convert.ToDouble(ifxProperty.ParamValue);
                        else
                            configModelObj.RigidLength = null;
                        break;
                    case "Weight":
                        if (ifxProperty.ParamValue != null)
                            configModelObj.GrossWeight = Convert.ToDouble(ifxProperty.ParamValue);
                        else
                            configModelObj.GrossWeight = null;
                        break;
                    case "HeaviestComponentWeight":
                        if (configModelObj.GrossWeight == 0)
                        {
                            if (ifxProperty.ParamValue != null)
                                configModelObj.GrossWeight = Convert.ToDouble(ifxProperty.ParamValue);
                            else
                                configModelObj.GrossWeight = null;
                        }
                        break;
                    case "OverallLength":
                        if (ifxProperty.ParamValue != null)
                            configModelObj.OverallLength = Convert.ToDouble(ifxProperty.ParamValue);
                        else
                            configModelObj.OverallLength = null;
                        break;
                    case "Width":
                        if (ifxProperty.ParamValue != null)
                            configModelObj.Width = Convert.ToDouble(ifxProperty.ParamValue);
                        else
                            configModelObj.Width = null;
                        break;
                    case "WheelBase":
                        if (ifxProperty.ParamValue != null)
                            configModelObj.WheelBase = Convert.ToDouble(ifxProperty.ParamValue);
                        else
                            configModelObj.WheelBase = null;
                        break;
                    case "Maximum Height":
                        if (ifxProperty.ParamValue != null)
                            configModelObj.MaxHeight = Convert.ToDouble(ifxProperty.ParamValue);
                        else
                            configModelObj.MaxHeight = null;
                        break;
                    case "Maximum_Height":
                        if (ifxProperty.ParamValue != null)
                            configModelObj.MaxHeight = Convert.ToDouble(ifxProperty.ParamValue);
                        else
                            configModelObj.MaxHeight = null;
                        break;
                    case "AxleWeight":
                        if (ifxProperty.ParamValue != null)
                            configModelObj.MaxAxleWeight = Convert.ToDouble(ifxProperty.ParamValue);
                        else
                            configModelObj.MaxAxleWeight = null;
                        break;
                    case "Speed":
                        if (ifxProperty.ParamValue != null)
                            configModelObj.TravellingSpeed = Convert.ToDouble(ifxProperty.ParamValue);
                        else
                            configModelObj.TravellingSpeed = null;
                        break;
                    case "Tyre_Spacing":
                        if (ifxProperty.ParamValue != null)
                            configModelObj.TyreSpacing = Convert.ToDouble(ifxProperty.ParamValue);
                        else
                            configModelObj.TyreSpacing = null;
                        break;
                    case "Left_Overhang":
                        if (ifxProperty.ParamValue != null)
                            configModelObj.NotifLeftOverhang = Convert.ToDouble(ifxProperty.ParamValue);
                        else
                            configModelObj.NotifLeftOverhang = null;
                        break;
                    case "Right_Overhang":
                        if (ifxProperty.ParamValue != null)
                            configModelObj.NotifRightOverhang = Convert.ToDouble(ifxProperty.ParamValue);
                        else
                            configModelObj.NotifRightOverhang = null;
                        break;
                    case "Front_Overhang":
                        if (ifxProperty.ParamValue != null)
                            configModelObj.NotifFrontOverhang = Convert.ToDouble(ifxProperty.ParamValue);
                        else
                            configModelObj.NotifFrontOverhang = null;
                        break;
                    case "Rear_Overhang":
                        if (ifxProperty.ParamValue != null)
                            configModelObj.NotifRearOverhang = Convert.ToDouble(ifxProperty.ParamValue);
                        else
                            configModelObj.NotifRearOverhang = null;
                        break;
                    case "TractorWeight":
                        if (ifxProperty.ParamValue != null)
                            configModelObj.TractorWeight = Convert.ToDouble(ifxProperty.ParamValue);
                        else
                            configModelObj.TractorWeight = null;
                        break;
                    case "TrailerWeight":
                        if (ifxProperty.ParamValue != null)
                            configModelObj.TrailerWeight = Convert.ToDouble(ifxProperty.ParamValue);
                        else
                            configModelObj.TrailerWeight = null;
                        break;
                    case "Number of Axles":
                        if (ifxProperty.ParamValue != "Select")
                        {
                            if (ifxProperty.ParamValue != null)
                                configModelObj.AxleCount = Convert.ToInt32(ifxProperty.ParamValue);
                            else
                                configModelObj.AxleCount = null;
                        }
                        break;
                    case "Number of Axles for Trailer":
                        if (ifxProperty.ParamValue != null)
                            configModelObj.TrailerAxleCount = Convert.ToInt32(ifxProperty.ParamValue);
                        else
                            configModelObj.TrailerAxleCount = null;
                        break;
                    default:
                        break;
                }
            }
            return configModelObj;
        }

        public JsonResult ImportFleetVehicleToRoute(long vehicleId, int ApplnRevId = 0)
        {
            long result = 0;
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , JsonResult,VehicleConfigController/CopyVehicleFromList ,Copy vehicle {1} rev id {2} content ref ", Session.SessionID, vehicleId, ApplnRevId));
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                result = vehicleconfigService.ImportFleetVehicleToRoute(vehicleId, SessionInfo.UserSchema, ApplnRevId);
                
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] , VehicleConfigController/CopyVehicleFromList, Exception: {1}", Session.SessionID, ex.Message));
                throw;
            }
            return Json(new { Success = result });
        }

        public JsonResult ChekcVehicleIsValid(long vehicleId, int flag)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            long count = 0;
            count = vehicleconfigService.ChekcVehicleIsValid(vehicleId,flag, SessionInfo.UserSchema);
            if(count==0)
            {
                List<VehicleConfigList> vehicleConfigList = null;
                if (flag == 1|| flag == 5)
                    vehicleConfigList = vehicleconfigService.GetVR1VehicleConfigVhclID((int)vehicleId, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                else if (flag == 2)
                    vehicleConfigList = vehicleconfigService.GetVehicleConfigVhclID((int)vehicleId, UserSchema.Portal).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                else if (flag == 3)
                    vehicleConfigList = vehicleconfigService.GetRouteVehicleConfigVhclID((int)vehicleId, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                else if (flag == 4)
                    vehicleConfigList = vehicleconfigService.GetMovementVehicleConfig(vehicleId, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                List<int> compIds = new List<int>();
                foreach (var id in vehicleConfigList)
                {
                    compIds.Add((int)id.ComponentId);
                }

                List<uint> vehicleConfig = vehicleconfigService.AssessConfigurationType(compIds, false, SessionInfo.UserSchema, flag);
                if (vehicleConfig.Count == 0)
                    count = 2;
            }
            return Json(new { Count = count });
        }

        private VehicleConfiguration ConvertComponentToConfig(VehicleComponent vehclCompObj, VehicleConfiguration vehicleConfigObj)
        {
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
                }
            }

            //remove fields not avilable in components

            
            vehicleConfigObj.VehicleConfigParamList.RemoveAll(configField =>
            {
                return configField.ShowText == 1 && configField.ParamModel != "Speed" && configField.ParamModel != "AxleWeight" && configField.ParamModel != "Wheelbase"
                && vehclCompObj.VehicleParamList.FirstOrDefault(r => r.ParamModel == configField.ParamModel) == null;
            });

            return vehicleConfigObj;
        }

        /// <summary>
        /// Show axle details in popup
        /// </summary>
        /// <param name="axleCount"></param>
        /// <param name="componentId"></param>
        /// <param name="compIds">Will pass if full component need to be displayed</param>
        /// <param name="vehicleSubTypeId"></param>
        /// <param name="vehicleTypeId"></param>
        /// <param name="movementId"></param>
        /// <param name="weight"></param>
        /// <param name="IsEdit"></param>
        /// <param name="isApplication"></param>
        /// <param name="appRevID"></param>
        /// <param name="vehConfigID"></param>
        /// <param name="isVR1"></param>
        /// <param name="isFromConfig"></param>
        /// <param name="vehicleConfigId"></param>
        /// <param name="movement"></param>
        /// <param name="isCandidate"></param>
        /// <param name="isEditMovement"></param>
        /// <param name="axles"></param>
        /// <returns></returns>
        public ActionResult AxlePopUp(int axleCount,int componentId, string compIds, int vehicleSubTypeId, int vehicleTypeId, int movementId, int? weight, bool IsEdit, bool isApplication = false, int appRevID = 0, long vehConfigID = 0, bool isVR1 = false, int isFromConfig = 0, int vehicleConfigId = 0, bool movement = false, bool isCandidate = false, bool isEditMovement = false, List<Axle> axles = null)
        {
            try
            {
                ViewBag.axleCount = axleCount;
                ViewBag.componentId = componentId;
                ViewBag.compIds = compIds;
                ViewBag.vehicleSubTypeId = vehicleSubTypeId;
                ViewBag.vehicleTypeId = vehicleTypeId;
                ViewBag.movementId = movementId;
                ViewBag.movement = movement;
                ViewBag.isCandidate = isCandidate;
                ViewBag.isEditMovement = isEditMovement;
                ViewBag.isFromPopUp = true;
                ViewBag.axles = axles;
                
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("VehicleConfig/AxlePopUp ActionResult method completed successfully"));
                return PartialView("~/Views/VehicleConfiguration/_AxlePopUp.cshtml");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}],Vehicle/AxleComponent, Exception: {1}", Session.SessionID, ex));
                throw ex;
            }
        }

        public ActionResult AllAxleDetailsPopUp(int vehicleId, bool isRoute = false, int movementTypeId = 0, bool isNotif = false, string flag = "", bool isSort = false, bool isMovement = false, bool isCandidate = false,int isFleet=0)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , GET,VehicleConfigController/AllAxleDetailsPopUp , View all axle details", Session.SessionID));
            UserInfo SessionInfo = null;
            SessionInfo = (UserInfo)Session["UserInfo"];
            if (isSort)
            {
                SessionInfo.UserSchema = UserSchema.Sort;
            }
            List<VehicleConfigList> vehicleConfigList = null;
            if (isFleet == 0)
            {
                if (Session["AppFlag"] != null && flag.ToLower() != "candidatevehicle")
                    flag = Convert.ToString(Session["AppFlag"]);
                if (Session["IsRoute"] != null)
                    isRoute = Convert.ToBoolean(Session["IsRoute"]);
                if (Session["IsNotif"] != null)
                    isNotif = Convert.ToBoolean(Session["IsNotif"]);
            }
            bool isVR1 = false;
            if (isRoute && !isMovement)
            {
                if (flag == "SOApp" || flag == "")
                {
                    vehicleConfigList = vehicleconfigService.GetRouteVehicleConfigVhclID(vehicleId, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                }
                else
                {
                    vehicleConfigList = vehicleconfigService.GetVR1VehicleConfigVhclID(vehicleId, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                    if (flag.ToLower() == "candidatevehicle" || flag.ToLower() == "vr1app")
                        isVR1 = true;
                }
            }
            else if (isNotif && !isMovement)
            {
                vehicleConfigList = vehicleconfigService.GetVR1VehicleConfigVhclID(vehicleId, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
            }
            else if (isMovement&& isFleet==0)
            {
                vehicleConfigList = vehicleconfigService.GetMovementVehicleConfig(vehicleId, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                if (vehicleConfigList.Count == 0)
                {
                    if (flag == "SOApp" || flag == "")
                    {
                        vehicleConfigList = vehicleconfigService.GetRouteVehicleConfigVhclID(vehicleId, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                        isMovement = false;
                    }
                    else if (isNotif|| flag == "VR1App")
                    {
                        vehicleConfigList = vehicleconfigService.GetVR1VehicleConfigVhclID(vehicleId, SessionInfo.UserSchema).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
                        isVR1 = true;
                        isMovement = false;
                    }
                }
            }
            else
            {
                vehicleConfigList = vehicleconfigService.GetVehicleConfigVhclID(vehicleId, UserSchema.Portal).OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn).ToList();
            }
            ViewBag.ComponentList = vehicleConfigList;

            ViewBag.vehicleID = vehicleId;
            ViewBag.isRoute = isRoute;
            ViewBag.isNotif = isNotif;
            ViewBag.isVR1 = isVR1;
            ViewBag.isFleet = isFleet;

            ViewBag.movement = isMovement;
            ViewBag.movementTypeId = movementTypeId;
            ViewBag.isCandidate = isCandidate;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("VehicleConfig/AllAxleDetailsPopUp ActionResult method completed successfully"));
            return PartialView("~/Views/VehicleConfiguration/AllAxleDetailsPopUp.cshtml");
        }
        public ActionResult ViewAxleDetails(List<VehicleConfigList> vehicleConfigList, bool isRoute = false, int movementTypeId = 0, bool isNotif = false, bool isMovement = false,bool isVR1=false,int isFleet=0)
        {
            
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }
            VehicleComponent vehclCompObj = new VehicleComponent();
            List<Axle> axleList = new List<Axle>();
            List<Axle> allAxleList = new List<Axle>();
            ViewBag.AxleList = null;
            ViewBag.tyreEmpty = false;
            if (vehicleConfigList.Count > 0)
            {
                allAxleList = GetAxleDetails(vehicleConfigList, isRoute, isNotif, isMovement, isVR1, isFleet);

                bool tyreEmpty = false;
                tyreEmpty = allAxleList.All(item => (item.TyreSize is null || item.TyreSize == "") && (item.TyreCenters is null || item.TyreCenters == ""));
                ViewBag.AxleList = allAxleList;
                ViewBag.tyreEmpty = tyreEmpty;
                int MovementXmlTypeId = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleXmlMovementTypeMapping((VehicleXmlMovementType)movementTypeId);
                vehclCompObj = GetVehicleComponent((int)vehicleConfigList[0].ComponentTypeId, (int)vehicleConfigList[0].ComponentSubTypeId, MovementXmlTypeId);
                if (movementTypeId == 270110 || movementTypeId == 270111 || movementTypeId == 270112 || movementTypeId == 270156)
                    vehclCompObj.IsConfigTyreCentreSpacing = true;
                else
                    vehclCompObj.IsConfigTyreCentreSpacing = vehclCompObj.IsConfigTyreCentreSpacing;
            }

            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/ViewAxleDetails ActionResult method completed successfully"));
            return PartialView("~/Views/VehicleConfiguration/ViewAxleDetails.cshtml", vehclCompObj);
        }

        public List<Axle> GetAxleDetails(List<VehicleConfigList> vehicleConfigList, bool isRoute = false, bool isNotif = false, bool isMovement = false, bool isVR1 = false, int isFleet = 0)
        {
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }
            List<Axle> axleList = new List<Axle>();
            List<Axle> allAxleList = new List<Axle>();

            for (int i = 0; i < vehicleConfigList.Count; i++)
            {
                int compId = (int)vehicleConfigList[i].ComponentId;
                if (isRoute && !isMovement)
                {
                    if (isVR1)
                    {
                        axleList = vehicleComponentService.ListVR1vehAxle(compId, SessionInfo.UserSchema);
                    }
                    else if (isNotif)
                    {
                        axleList = vehicleComponentService.ListVR1vehAxle(compId, SessionInfo.UserSchema);
                    }
                    else
                    {
                        axleList = vehicleComponentService.ListRouteComponentAxle(compId, SessionInfo.UserSchema);
                    }
                }
                else if (isNotif && !isMovement)
                {
                    axleList = vehicleComponentService.ListVR1vehAxle(compId, SessionInfo.UserSchema);
                }
                else if (isMovement && isFleet == 0)
                {
                    axleList = vehicleComponentService.ListAxleTemp(compId, isMovement, SessionInfo.UserSchema);
                    if (axleList.Count == 0)
                    {
                        if (isNotif)
                        {
                            axleList = vehicleComponentService.ListVR1vehAxle(compId, SessionInfo.UserSchema);
                        }
                    }
                }
                else
                {
                    axleList = vehicleComponentService.ListAxle(compId);
                }
                allAxleList.AddRange(axleList);
            }

            return allAxleList;
        }


        private bool ComponentRequiredFieldForVR1(int ComponentTypeId, string Field)
        {
            bool isRequired = false;
            switch (ComponentTypeId)
            {
                case (int)ComponentType.ConventionalTractor:
                    if (Field == "Width" || Field == "Maximum Height")
                        isRequired = true;
                    break;
                case (int)ComponentType.SemiTrailer:
                case (int)ComponentType.EngineeringPlantSemiTrailer:
                    if (Field == "Width" || Field == "Maximum Height" || Field == "Length")
                        isRequired = true;
                    break;
                default:
                    break;
            }
            return isRequired;
        }
        private bool AxleRequiedFields(int ComponentTypeId,int movementTypeId)
        {
            int MvmntType= STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.MovementTypeHighLevelMapping((VehicleXmlMovementType)movementTypeId);

            bool isRequired = false;
            switch (ComponentTypeId)
            {
                case (int)ComponentType.BallastTractor:
                    if(MvmntType == (int)VehicleMovementTypeMain.SpecialOrder)
                        isRequired = true;
                    break;
                case (int)ComponentType.ConventionalTractor:
                    if (MvmntType == (int)VehicleMovementTypeMain.SpecialOrder )
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
                    if (MvmntType == (int)VehicleMovementTypeMain.SpecialOrder )
                        isRequired = true;
                    break;
                case (int)ComponentType.SemiTrailer:
                case (int)ComponentType.EngineeringPlantSemiTrailer:
                    if (MvmntType == (int)VehicleMovementTypeMain.SpecialOrder )
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
                    if (MvmntType == (int)VehicleMovementTypeMain.SpecialOrder|| 
                        MvmntType == (int)VehicleMovementTypeMain.Stgovr1 ||
                        MvmntType == (int)VehicleMovementTypeMain.VehicleSpecialOrder)
                        isRequired = true;
                    break;
                case (int)ComponentType.ConventionalTractor:
                    if (MvmntType == (int)VehicleMovementTypeMain.SpecialOrder ||
                        MvmntType == (int)VehicleMovementTypeMain.Stgovr1 ||
                        MvmntType == (int)VehicleMovementTypeMain.VehicleSpecialOrder)
                        isRequired = true;
                    break;
                case (int)ComponentType.DrawbarTrailer:
                case (int)ComponentType.EngineeringPlantDrawbarTrailer:
                case (int)ComponentType.GirderSet:
                    if ( MvmntType == (int)VehicleMovementTypeMain.SpecialOrder ||
                        MvmntType == (int)VehicleMovementTypeMain.Stgovr1 ||
                        MvmntType == (int)VehicleMovementTypeMain.VehicleSpecialOrder)
                        isRequired = true;
                    break;
                case (int)ComponentType.EngineeringPlant:
                case (int)ComponentType.RigidVehicle:
                    if (MvmntType == (int)VehicleMovementTypeMain.SpecialOrder ||
                        MvmntType == (int)VehicleMovementTypeMain.Stgovr1 ||
                        MvmntType == (int)VehicleMovementTypeMain.VehicleSpecialOrder)
                        isRequired = true;
                    break;
                case (int)ComponentType.SemiTrailer:
                case (int)ComponentType.EngineeringPlantSemiTrailer:
                    if (MvmntType == (int)VehicleMovementTypeMain.SpecialOrder ||
                        MvmntType == (int)VehicleMovementTypeMain.Stgovr1 ||
                        MvmntType == (int)VehicleMovementTypeMain.VehicleSpecialOrder)
                        isRequired = true;
                    break;
                case (int)ComponentType.SPMT:
                    if (MvmntType == (int)VehicleMovementTypeMain.SpecialOrder ||
                        MvmntType == (int)VehicleMovementTypeMain.Stgovr1 ||
                        MvmntType == (int)VehicleMovementTypeMain.VehicleSpecialOrder)
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
        
        private VehicleComponent RemoveProjectionFields(VehicleComponent vehclCompObj, bool isLastComponent = false, int compNumber=0)
        {
            if(compNumber==1&& !isLastComponent)
            {
                var itemToRemove1 = vehclCompObj.VehicleParamList.FirstOrDefault(r => r.ParamModel == "Rear Overhang");
                if (itemToRemove1 != null)
                    vehclCompObj.VehicleParamList.Remove(itemToRemove1);
            }
            else if(compNumber > 1 && !isLastComponent)
            {
                var itemToRemove1 = vehclCompObj.VehicleParamList.FirstOrDefault(r => r.ParamModel == "Front Overhang");
                if (itemToRemove1 != null)
                    vehclCompObj.VehicleParamList.Remove(itemToRemove1);

                var itemToRemove2 = vehclCompObj.VehicleParamList.FirstOrDefault(r => r.ParamModel == "Rear Overhang");
                if (itemToRemove2 != null)
                    vehclCompObj.VehicleParamList.Remove(itemToRemove2);

            }
            else if (isLastComponent)
            {
                var itemToRemove1 = vehclCompObj.VehicleParamList.FirstOrDefault(r => r.ParamModel == "Front Overhang");
                if (itemToRemove1 != null)
                    vehclCompObj.VehicleParamList.Remove(itemToRemove1);
            }
            return vehclCompObj;
        }
        #endregion

    }
}