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
using STP.Web.Filters;
using STP.Web.WorkflowProvider;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static STP.Common.Enums.ExternalApiEnums;
using static STP.Domain.VehiclesAndFleets.Configuration.VehicleGlobalConfig;
using static STP.Domain.VehiclesAndFleets.VehicleEnums;
using static STP.Common.Enums.ExternalApiEnums;

namespace STP.Web.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleComponentService vehicleComponentService;
        private readonly ILoggingService loggingService;
        private readonly IVehicleConfigService vehicleConfigService;
        private readonly IFleetManagementWorkflowService fleetManagementWorkflowService;


        #region public VehicleController()
        public VehicleController()
        {
            //componentConfig = new ComponentConfiguration();

        }
        #endregion public VehicleController()
        public VehicleController(IVehicleComponentService vehicleComponentService, ILoggingService loggingService, IVehicleConfigService vehicleConfigService, IFleetManagementWorkflowService fleetManagementWorkflowService)
        {
            this.vehicleComponentService = vehicleComponentService;
            this.loggingService = loggingService;
            this.vehicleConfigService = vehicleConfigService;
            this.fleetManagementWorkflowService = fleetManagementWorkflowService;
        }
        // GET: Vehicle
        public ActionResult Index()
        {
            return View();
        }

        #region Public Functions

        #region public ActionResult FleetComponent(int? page, int? pageSize, string searchString)
        /// <summary>
        /// List the Details of Fleet Component
        /// </summary>
        /// <returns>View</returns>
        [AuthorizeUser(Roles = "50000,50001,13003,13004,13005,13006")]
        public ActionResult FleetComponent(int? page, int? pageSize, string searchString,string searchVhclType=null,string searchVhclIntend=null, int isFromConfig = 0, int filterFavourites = 0, int? sortOrder = null, int? sortType = null, int IsFromMenu = 0)
        {
            try
            {
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                sortOrder = sortOrder != null ? (int)sortOrder : 0; //internal name
                int presetFilter = sortType != null ? (int)sortType : 0; // asc
                ViewBag.SortOrder = sortOrder;
                ViewBag.SortType = presetFilter;
                ViewBag.isFromConfig = isFromConfig;
                if (filterFavourites == 0)
                    ViewBag.filterFavourites = false;
                else
                    ViewBag.filterFavourites = true;

                if (IsFromMenu == 1)
                {
                    Session["g_VehicleComponentSearch"] = null;
                    Session["g_VehicleTypeSearch"] = null;
                    Session["g_VehicleIntendSearch"] = null;
                    Session["movementClassificationName"] = null;
                    Session["movementClassificationId"] = null;
                }

                #region search data

                if (!string.IsNullOrEmpty(searchString))
                {
                    TempData["SearchCompName"] = searchString;
                }
                else if (Session["g_VehicleComponentSearch"] != null)
                {
                    searchString = (string)Session["g_VehicleComponentSearch"];
                }

                if (!string.IsNullOrEmpty(searchVhclType))
                {
                    TempData["SearchVhclType"] = searchVhclType;
                }
                else if (Session["g_VehicleTypeSearch"] != null)
                {
                    searchVhclType = (string)Session["g_VehicleTypeSearch"];
                }

                if (!string.IsNullOrEmpty(searchVhclIntend))
                {
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
                else if (Session["g_VehicleIntendSearch"] != null)
                {
                    searchVhclIntend = (string)Session["g_VehicleIntendSearch"];
                }

                ViewBag.CompNameSearch = searchString;
                ViewBag.TypeSearch = searchVhclType;
                ViewBag.VhclIntendSearch = searchVhclIntend;
                if (ViewBag.VhclIntendSearch == "c and u")
                    ViewBag.VhclIntendSearch = "Construction and use";

                ViewBag.vehicleType = new SelectList(GetAllVehicleType(), ViewBag.TypeSearch);
                if (Session["movementClassificationName"] != null)
                    ViewBag.vehicleIntend = new SelectList(GetVehicleIntend(ViewBag.VhclIntendSearch, SessionInfo.UserTypeId), ViewBag.VhclIntendSearch);
                else
                    ViewBag.vehicleIntend = new SelectList(GetVehicleIntend("", SessionInfo.UserTypeId), ViewBag.VhclIntendSearch);

                if (isFromConfig == 1)
                {
                    if (Session["movementClassificationId"] != null)
                        AssignMovementClassification((int)Session["movementClassificationId"]);
                    if (Session["movementClassificationName"] != null)
                        searchVhclIntend = (string)Session["movementClassificationName"];
                    else
                        searchVhclIntend = (string)Session["g_VehicleIntendSearch"];
                    if (searchVhclIntend == "Construction and use")
                        searchVhclIntend = "c and u";
                    if (searchVhclIntend == "STGO AIL ( including VR-1s )")
                        searchVhclIntend = "STGO AIL";
                }
                else
                {
                    if (Session["g_VehicleIntendSearch"] != null)
                        searchVhclIntend = (string)Session["g_VehicleIntendSearch"];
                }


                switch (searchVhclIntend)
                {
                    case "STGO Road recovery operation":
                        searchVhclIntend = "stgo road recovery";
                        break;
                    case "STGO Engineering plant(not tracked)":
                    case "STGO Engineering plant ( not tracked )":
                        searchVhclIntend = "stgo engineering plant wheeled";
                        break;
                    case "STGO Mobile crane":
                        searchVhclIntend = "stgo mobile crane";
                        break;
                    default:
                        break;
                }
                #endregion search data

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/FleetComponent actionResult method started successfully, with page - {1}, pageSize - {2}, search string - {3}", Session.SessionID, page, pageSize, searchString));

                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");

                int organisationId;
                if (SessionInfo.UserTypeId == UserType.Sort)
                    organisationId = (int)Session["SORTOrgID"];
                else
                    organisationId = (int)SessionInfo.OrganisationId;
                int maxlist_item = SessionInfo.MaxListItem;

                if (Session["PageSize"] == null)
                    Session["PageSize"] = maxlist_item;

                if (pageSize == null)
                    pageSize = (Session["PageSize"] != null) ? (int)Session["PageSize"] : maxlist_item;
                else
                    Session["PageSize"] = pageSize;

                ViewBag.pageSize = pageSize;
                ViewBag.SearchValue = searchString;
                int pageNumber = (page ?? 1);
                ViewBag.page = pageNumber;

                List<ComponentGridList> GridListObj = new List<ComponentGridList>();
                if (sortOrder == null)
                    GridListObj = vehicleComponentService.GetComponentByOrganisationId(organisationId, pageNumber, (int)pageSize, "", searchVhclType, searchVhclIntend, filterFavourites, UserSchema.Portal, presetFilter, sortOrder).OrderBy(s => s.ComponentDescription).ToList();
                else
                    GridListObj = vehicleComponentService.GetComponentByOrganisationId(organisationId, pageNumber, (int)pageSize, "", searchVhclType, searchVhclIntend, filterFavourites, UserSchema.Portal, presetFilter, sortOrder).ToList();


                
                if (!string.IsNullOrEmpty(searchString))
                {
                    GridListObj = (from s in GridListObj
                                   where s.ComponentName.ToLower().Contains(searchString.ToLower())
                                                select s).ToList();
                }
                ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/FleetComponent actionResult method completed successfully", Session.SessionID));

                long totalCount = 0;
                if (GridListObj != null && GridListObj.Count > 0)
                    totalCount = GridListObj.Count;
                else
                    totalCount = 0;
                var componentObjPagedList = new StaticPagedList<ComponentGridList>(GridListObj, (int)pageNumber, (int)pageSize, (int)totalCount);

                ViewBag.movementClassificationName = Session["movementClassificationName"];
                return View(componentObjPagedList.ToPagedList(pageNumber, (int)pageSize));
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Vehicle/FleetComponent, Exception: {0}", ex));
                throw new HttpException(500, ex.Message);
            }

        }
        #endregion

        #region public ActionResult SaveSearchData(string searchString, string vehicleType, string vehicleIntend)
        public ActionResult SaveSearchData(string searchString, string vehicleType, string vehicleIntend, int filterFavourites = 0, int isFromConfig = 0, int? sortOrder = null, int? sortType = null,int? page=1, int? pageSize=10)
        {
            if (vehicleIntend == "Construction and use")
            {
                vehicleIntend = "c and u";
            }
            Session["g_VehicleComponentSearch"] = searchString;
            Session["g_VehicleTypeSearch"] = vehicleType;
            Session["g_VehicleIntendSearch"] = vehicleIntend;
            return RedirectToAction("FleetComponent", new
            {
                B7vy6imTleYsMr6Nlv7VQ =
                        STP.Web.Helpers.EncryptionUtility.Encrypt("isFromConfig=" + isFromConfig +
                        "&searchString=" + searchString +
                        "&searchVhclType=" + vehicleType +
                        "&searchVhclIntend=" + vehicleIntend +
                        "&filterFavourites=" + filterFavourites +
                        "&sortOrder=" + sortOrder +
                        "&sortType=" + sortType +
                        "&page=" + page +
                        "&pageSize=" + pageSize)
            });
        }
        #endregion        

        #region public ActionResult GeneralComponent(int vehicleSubTypeId, int vehicleTypeId, int movementId, int? componentId, bool isRoute=false)
        public ActionResult GeneralComponent(int? componentId, string mode = "")
        {
            try
            {
                ViewBag.componentId = componentId;
                ViewBag.mode = mode;
                return View("GeneralComponent");

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}],Vehicle/GeneralComponent, Exception: {1}", Session.SessionID, ex));
                throw ex;
            }
        }
        #endregion public ActionResult ViewComponent(int vehicleSubTypeId, int vehicleTypeId, int movementId, int? componentId, bool isRoute=false)

        #region public ActionResult GeneralVehicleComponent(int vehicleSubTypeId, int vehicleTypeId, int movementId, int? componentId, bool isRoute=false)
        public ActionResult GeneralVehicleComponent(int vehicleSubTypeId, int vehicleTypeId, int movementId, int? componentId, bool isRoute = false, int latPos = 0, int longPos = 0, bool IsNotif = false, bool isVR1 = false, bool movement = false, string mode = "", bool isLastComponent = false, int compNumber = 1, int configTypeId = 0, string flag = "")
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/ViewComponent ActionResult method started successfully, with parameters vehicleSubTypeId:{1}, vehicleTypeId:{2}, movementId:{3}, componentId:{4}", Session.SessionID, vehicleSubTypeId, vehicleTypeId, movementId, componentId));
                ViewBag.ComponentId = componentId;
                ViewBag.IsMovement = movement;
                ViewBag.IsRoute = isRoute;
                ViewBag.IsNotif = IsNotif;
                ViewBag.IsVR1 = isVR1;
                ViewBag.mode = mode;
                ViewBag.IsLastComp = isLastComponent;
                ViewBag.configTypeId = configTypeId;
                if (Session["AppFlag"] != null && flag.ToLower() != "candidatevehicle")
                    flag = Convert.ToString(Session["AppFlag"]);
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }

                VehicleComponent vehclCompObj = new VehicleComponent();
                ComponentModel VehicleComponentObj = null;
                List<VehicleRegistration> listVehclRegObj = null;
                List<Axle> axleList = null;
                ViewBag.AxleList = null;
                if (componentId != null)
                {
                    int compId = (int)componentId;
                    if (isRoute && !movement)
                    {
                        if (isVR1)
                        {
                            VehicleComponentObj = vehicleComponentService.GetVR1VehicleComponent(compId, SessionInfo.UserSchema);
                        }
                        else if (flag == "SOApp" || flag == "")
                        {
                            VehicleComponentObj = vehicleComponentService.GetRouteComponent(compId, SessionInfo.UserSchema);
                        }
                        else
                        {
                            VehicleComponentObj = vehicleComponentService.GetVR1VehicleComponent(compId, SessionInfo.UserSchema);
                        }
                    }
                    else if (IsNotif && !movement)
                    {
                        VehicleComponentObj = vehicleComponentService.GetVR1VehicleComponent(compId, SessionInfo.UserSchema);                        
                    }
                    else
                    {
                        if (movement)
                        {
                            VehicleComponentObj = vehicleComponentService.GetComponentTemp(compId, "", SessionInfo.UserSchema);
                        }
                        else
                        {
                            VehicleComponentObj = vehicleComponentService.GetVehicleComponent(compId);
                        }
                    }
                    if (VehicleComponentObj.ComponentType != 0)
                    {
                        vehicleTypeId = VehicleComponentObj.ComponentType;
                        vehicleSubTypeId = VehicleComponentObj.ComponentSubType;
                        if(movementId==0)
                            movementId = VehicleComponentObj.VehicleIntent;
                        if (isRoute && !movement)
                        {
                            if (isVR1)
                            {
                                listVehclRegObj = vehicleComponentService.GetVR1RegistrationDetails(compId, SessionInfo.UserSchema);
                                axleList = vehicleComponentService.ListVR1vehAxle(compId, SessionInfo.UserSchema);
                            }
                            else if (flag == "SOApp" || flag == "")
                            {
                                listVehclRegObj = vehicleComponentService.GetRouteComponentRegistrationDetails(compId, SessionInfo.UserSchema);
                                axleList = vehicleComponentService.ListRouteComponentAxle(compId, SessionInfo.UserSchema);
                            }
                            else if (IsNotif)
                            {
                                listVehclRegObj = vehicleComponentService.GetVR1RegistrationDetails(compId, SessionInfo.UserSchema);
                                axleList = vehicleComponentService.ListVR1vehAxle(compId, SessionInfo.UserSchema);
                            }
                        }
                        else if (IsNotif && !movement)
                        {
                            listVehclRegObj = vehicleComponentService.GetVR1RegistrationDetails(compId, SessionInfo.UserSchema);
                            axleList = vehicleComponentService.ListVR1vehAxle(compId, SessionInfo.UserSchema);

                        }
                        else
                        {
                            if (movement)
                            {
                                listVehclRegObj = vehicleComponentService.GetRegistrationTemp(compId, movement, SessionInfo.UserSchema);
                                axleList = vehicleComponentService.ListAxleTemp(compId, movement, SessionInfo.UserSchema);
                            }
                            else
                            {
                                listVehclRegObj = vehicleComponentService.GetRegistrationDetails(compId);
                                axleList = vehicleComponentService.ListAxle(compId);
                            }
                        }
                        bool tyreEmpty = false;
                        tyreEmpty = axleList.All(item => (item.TyreSize is null || item.TyreSize == "") && (item.TyreCenters is null || item.TyreCenters == ""));
                        ViewBag.tyreEmpty = tyreEmpty;
                        ViewBag.AxleList = axleList;
                        int MovementXmlTypeId = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleXmlMovementTypeMapping((VehicleXmlMovementType)movementId);

                        vehclCompObj = GetVehicleComponent(vehicleSubTypeId, vehicleTypeId, MovementXmlTypeId);
                        if (vehicleTypeId == (int)ComponentType.BallastTractor && (movementId == 270103 || movementId == 270104 
                            || movementId == 270105 || movementId == 270106 || movementId == 270107 || movementId == 270108 || movementId == 270109))
                        {
                            var itemToRemove1 = vehclCompObj.VehicleParamList.FirstOrDefault(r => r.ParamModel == "Outside Track");
                            vehclCompObj.VehicleParamList.Remove(itemToRemove1);
                            var itemToRemove2 = vehclCompObj.VehicleParamList.FirstOrDefault(r => r.ParamModel == "Wheelbase");
                            vehclCompObj.VehicleParamList.Remove(itemToRemove2);
                        }
                        if (configTypeId == (int)ConfigurationType.RigidAndDrag && vehicleTypeId == (int)ComponentType.RigidVehicle)
                        {
                            int index = vehclCompObj.VehicleParamList.FindIndex(s => s.ParamModel == "Coupling");
                            if (index != -1)
                                vehclCompObj.VehicleParamList[index].ParamValue = "Drawbar";
                        }
                        if (movement && SessionInfo.UserSchema != UserSchema.Sort)
                        {
                            var itemToRemove1 = vehclCompObj.VehicleParamList.FirstOrDefault(r => r.ParamModel == "Notes");
                            vehclCompObj.VehicleParamList.Remove(itemToRemove1);
                        }

                        
                        if(mode!="" && VehicleComponentObj.SpacingToFollowing!=null&& VehicleComponentObj.SpacingToFollowing != 0)
                        {
                            IFXProperty iFX = new IFXProperty();
                            iFX.DisplayString = "Axle Spacing To Following";
                            iFX.ParamModel = "Axle Spacing To Following";
                            iFX.ParamType = "float";
                            iFX.DropDownList = vehclCompObj.VehicleParamList[vehclCompObj.VehicleParamList.Count-1].DropDownList;
                            vehclCompObj.VehicleParamList.Add(iFX);
                        }
                        if (vehclCompObj != null)
                        {
                            if (VehicleComponentObj != null)
                                vehclCompObj.UpdateVehicleProperties(VehicleComponentObj);

                            if (listVehclRegObj != null && listVehclRegObj.Count != 0)
                            {
                                vehclCompObj.ListVehicleReg = new List<VehicleRegistration>();
                                vehclCompObj.ListVehicleReg = listVehclRegObj;
                            }
                            if (movementId == 270110 || movementId == 270111 || movementId == 270112 || movementId == 270156)
                                vehclCompObj.IsConfigTyreCentreSpacing = true;
                        }

                        ViewBag.LatPos = latPos;
                        ViewBag.LongPos = longPos;
                        ViewBag.CompNumber = compNumber;
                        ViewBag.VehicleTypeId = vehicleTypeId;
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/ViewComponent ActionResult method completed successfully"));
                        return PartialView("GeneralVehicleComponent", vehclCompObj);
                    }
                }
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}],Vehicle/ViewComponent, Exception: {1}", Session.SessionID, ex));
                throw ex;
            }
        }
        #endregion public ActionResult ViewComponent(int vehicleSubTypeId, int vehicleTypeId, int movementId, int? componentId, bool isRoute=false)

        #region public JsonResult DeleteVehicleComponent(int componentId)
        /// <summary>
        /// JsonResult method to delete Component details
        /// </summary>
        /// <param name="componentId">bool as json result</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteVehicleComponent(int componentId)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/DeleteVehicleComponent JsonResult method started successfully, with parameters componentId:{1}", Session.SessionID, componentId));

                bool result = vehicleComponentService.DeleteVehComponent(componentId);

                #region System Event Log - created_component_for_so_application
                UserInfo SessionInfo = new UserInfo();
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                string ErrMsg = string.Empty;
                string sysEventDescp = string.Empty;

                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                movactiontype.UserName = SessionInfo.UserName;
                movactiontype.FleetComponentId = Convert.ToInt32(componentId);
                movactiontype.SystemEventType = SysEventType.haulier_deleted_fleet_component;

                sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                #endregion

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/DeleteVehicleComponent JsonResult method completed successfully"));
                return Json(new { Success = true });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}],Vehicle/DeleteVehicleComponent, Exception: {1}", Session.SessionID, ex));
                throw ex;
            }
        }
        #endregion public JsonResult DeleteVehicleComponent(int componentId)

        #region public ActionResult CreateComponent()
        /// <summary>
        /// ActionResult to display CreateComponent
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]

        public ActionResult CreateComponent(bool isLastComponent = false, int isFromConfig = 0)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , HttpGet,VehicleController/CreateComponent , Create component ", Session.SessionID));

            #region session to find out last component
            isLastComponent = isFromConfig == 0 ? true : false;
            Session["is_lastComponent"] = isLastComponent;
            #endregion
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
            VehicleComponentDropDown(sessionValues.UserTypeId);
            ViewBag.IsAdmin = isAdmin;
            ViewBag.isFromConfig = isFromConfig;
            TempData["IsFromConfig"] = isFromConfig;
            if (isFromConfig == 0)
            {
                Session["movementClassificationId"] = null;
                Session["movementClassificationName"] = null;
            }
            return View();
        }
        #endregion

        #region public JsonResult CreateComponent(VehicleComponent vehicleComponent, int showComponent)
        /// <summary>
        /// JsonResult method to save Component details
        /// </summary>
        /// <param name="user">Object</param>
        /// <returns>bool as json result </returns>
        [HttpPost]
        //[ValidateAntiForgeryToken()]
        public JsonResult CreateComponent(VehicleComponent vehicleComponent, int showComponent, List<RegistrationParams> registrationParams, List<Axle> axleList, int vehicleConfigId = 0, int isFromConfig = 0, bool isMovement = false, int organisationId = 0)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/CreateComponent actionResult method started successfully with parameters VehicleComponent object and showComponent:{0}", showComponent));

            UserInfo SessionInfo = null;
            int portalType = 0;
            bool isAdmin = false;
            string sysEventDescp = null;
            string ErrMsg = null;

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
            string guid = "";
            try
            {
                if (isFromConfig == 1 && new SessionData().Wf_Fm_CurrentExecuted == WorkflowActivityTypes.Gn_NotDecided) //1 : From Vehicle Configuration .
                {
                    //New workflow for fleet management with manual entry fleet starts here.
                    StartFleetManagementWorkflow(SessionInfo.OrganisationName, false);
                }
                if (SessionInfo.UserTypeId != UserType.Sort)
                {
                    organisationId = (int)SessionInfo.OrganisationId;
                }

                ComponentModel componentObj = ConvertToComponent(vehicleComponent);
                componentObj.OrganisationId = organisationId;
                componentObj.ComponentType = vehicleComponent.vehicleCompType.ComponentTypeId;
                componentObj.ComponentSubType = vehicleComponent.VehicleComponentId;
                componentObj.VehicleIntent = vehicleComponent.moveClassification.ClassificationId;
                //component to show or not'
                componentObj.ShowComponent = showComponent;
                if (isFromConfig == 1)
                {
                    if (new SessionData().Wf_Fm_FleetManagementId.ToLower() != "failed")
                    {
                        componentObj.GUID = new SessionData().Wf_Fm_FleetManagementId;
                        guid = new SessionData().Wf_Fm_FleetManagementId;
                    }
                }

                ///make as configuration methods
                ///need to implement functionality
                string makeConfig = (from s in vehicleComponent.VehicleParamList
                                     where s.ParamModel == ComponentFields.MakeConfig
                                     select s.ParamValue).FirstOrDefault();
                ///check makeConfig not null and is "1" then check configuration exists
                ///if new config name create component then configuration and finally vehicle position
                if (!string.IsNullOrEmpty(makeConfig) && makeConfig == "1")
                {
                    ///check configuration name exists
                    ///if exists return json or value
                    string internalName = (from s in vehicleComponent.VehicleParamList
                                           where s.ParamModel == ComponentFields.InternalName
                                           select s.ParamValue).FirstOrDefault();

                    int configNameCount = vehicleComponentService.CheckConfigNameExists(internalName, organisationId);


                    ViewBag.MakeConfig = 1;
                    TempData["MakeConfig"] = ViewBag.MakeConfig;
                    if (configNameCount > 0)
                    {
                        return Json(new { Success = 0 });
                    }
                    if (isFromConfig == 1 && vehicleConfigId == 0)
                    {
                        componentId = vehicleComponentService.InsertComponentToTemp(componentObj);
                    }
                    else
                    {
                        componentId = vehicleComponentService.CreateComponent(componentObj);
                    }

                    #region System Event Log - Haulier created fleet component
                    movactiontype.UserName = SessionInfo.UserName;
                    movactiontype.FleetComponentId = (long)componentId;
                    movactiontype.FleetComponentName = componentObj.IntendedName;
                    movactiontype.SystemEventType = SysEventType.haulier_created_fleet_component;

                    sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                    #endregion

                    ///create this configuration
                    if (componentId > 0)
                    {
                        int vhclType = 0;
                        if (componentObj.ComponentType == 234003)
                        {
                            vhclType = 244003;
                        }
                        else if (componentObj.ComponentType == 234007)
                        {
                            vhclType = 244005;
                        }
                        VehicleConfiguration config = CastComponentToVehicle(vehicleComponent);
                        NewConfigurationModel vehicleConfiguration = ConvertToConfiguration(config);
                        vehicleConfiguration.OrganisationId = organisationId;
                        vehicleConfiguration.VehicleType = vhclType;
                        vehicleConfiguration.VehiclePurpose = componentObj.VehicleIntent;
                        vehicleConfiguration.SpeedUnit = vehicleComponent.TravellingSpeedUnit;
                        double configurationid = vehicleConfigService.CreateConfiguration(vehicleConfiguration);

                        ///insert the vehicle and component into vehicle position table with lat and long position as 1
                        VehicleConfigList ConfigPosn = new VehicleConfigList();
                        ConfigPosn.VehicleId = Convert.ToInt64(configurationid);
                        ConfigPosn.ComponentId = Convert.ToInt64(componentId);
                        ConfigPosn.LatPosn = 1;
                        ConfigPosn.LongPosn = 1;
                        ConfigPosn.SubType = componentObj.ComponentType;

                        var vehicleConfigList = vehicleComponentService.CreateConfPosnComponent(ConfigPosn);

                        componentId = vehicleConfigList.ComponentId;

                        #region System Event Log - haulier_added_fleet_component_as_config
                        movactiontype.FleetVehicleId = vehicleConfigList.VehicleId;
                        movactiontype.SystemEventType = SysEventType.haulier_added_fleet_component_as_config;

                        sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                        loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);

                        #endregion

                        return Json(new { Success = componentId, configId = vehicleConfigList.VehicleId });
                    }
                }
                else
                {
                    if ((isFromConfig == 1 && vehicleConfigId == 0) || (vehicleConfigId != 0 && isMovement == true))
                    {
                        if (componentObj.GUID != "")
                        {
                            componentId = vehicleComponentService.InsertComponentToTemp(componentObj);
                        }
                        else { componentId = -1; }
                    }
                    else if (vehicleConfigId != 0)
                    {
                        componentId = vehicleComponentService.CreateComponent(componentObj);
                        if (componentId > 0)
                        {
                            configposn = vehicleComponentService.InsertComponentConfigPosn((int)componentId, vehicleConfigId);
                        }
                    }
                    else
                    {
                        componentId = vehicleComponentService.CreateComponent(componentObj);
                    }

                    #region System Event Log - Haulier created fleet component
                    movactiontype.UserName = SessionInfo.UserName;
                    movactiontype.FleetComponentId = (long)componentId;
                    movactiontype.FleetComponentName = componentObj.IntendedName;
                    if (showComponent == 1 && vehicleConfigId == 0)
                    {
                        movactiontype.SystemEventType = SysEventType.haulier_created_fleet_component;
                    }
                    else if (vehicleConfigId > 0 && showComponent == 0)
                    {
                        movactiontype.FleetVehicleId = (long)vehicleConfigId;
                        movactiontype.SystemEventType = SysEventType.Haulier_created_component;
                    }

                    sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out sysEventDescp);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                    #endregion
                }

                if (componentId > 0)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/CreateComponent JsonResult method for registration details save for component started successfully, with parameters compId:{1}", Session.SessionID, componentId));

                    int IdNumber = 0;
                    if (registrationParams != null)
                    {
                        foreach (RegistrationParams registration in registrationParams)
                        {
                            if ((isFromConfig == 1 && vehicleConfigId == 0) || (vehicleConfigId != 0 && isMovement == true))
                            {
                                IdNumber = vehicleComponentService.CreateRegistrationTemp((int)componentId, registration.RegistrationValue, registration.FleetId, false, SessionInfo.UserSchema);
                            }
                            else
                            {
                                IdNumber = vehicleComponentService.CreateRegistration((int)componentId, registration.RegistrationValue, registration.FleetId);
                            }
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
                            if ((isFromConfig == 1 && vehicleConfigId == 0) || (vehicleConfigId != 0 && isMovement == true))
                            {
                                vehicleComponentService.InsertAxleDetailsTemp(axle, (int)componentId, false, SessionInfo.UserSchema);
                            }
                            else
                            {
                                vehicleComponentService.UpdateAxle(axle, (int)componentId);
                            }
                        }
                    }

                    movactiontype.SystemEventType = SysEventType.haulier_added_axle_details_for_fleet_component;

                    sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                    bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);

                    if (isMovement && vehicleConfigId != 0)
                    {
                        bool result = vehicleConfigService.InsertMovementConfigPosnTemp(guid, vehicleConfigId, SessionInfo.UserSchema);
                    }
                    //For saving component details at the time of vehicle save
                    componentObj.ComponentId = componentId;
                    if (Session["vehicleWorkFlowParams"] != null)
                    {
                        VehicleWorkFlowParams vehicleWorkFlow = (VehicleWorkFlowParams)Session["vehicleWorkFlowParams"];
                        vehicleWorkFlow.VehicleComponentsModels.Add(new VehicleComponentsModel { ComponentModel = componentObj, RegistrationDetails = registrationParams, AxleDetails = axleList });
                        Session["vehicleWorkFlowParams"] = vehicleWorkFlow;
                    }
                    else
                    {
                        VehicleWorkFlowParams vehicleWorkFlowParams = new VehicleWorkFlowParams();
                        vehicleWorkFlowParams.VehicleComponentsModels.Add(new VehicleComponentsModel { ComponentModel = componentObj, RegistrationDetails = registrationParams, AxleDetails = axleList });

                        Session["vehicleWorkFlowParams"] = vehicleWorkFlowParams;
                    }
                    ProcessWorkflowActivity(null);

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

            return Json(new { Success = componentId, Guid = guid });
        }
        #endregion

        #region public JsonResult FillVehicleSubType(int movementId, int vehicleTypeId)
        /// <summary>
        /// JsonResult method to fill vehicle type dropdown
        /// </summary>
        /// <param name="movementId">movementId</param>
        /// <returns>list of objacts as dropdown</returns>
        public JsonResult FillVehicleSubType(int vehicleTypeId)
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

                List<uint> subComponentId = new List<uint>();
                if (Session["movementClassificationId"] != null && Session["movementClassificationId"] != "")
                {
                    subComponentId = vehicleComponentService.VehicleSubComponentType((int)Session["movementClassificationId"], vehicleTypeId, UserSchema.Portal);

                    listVehicleSubCompObj = listVehicleSubCompObj.Where(x => subComponentId.Contains((uint)x.SubCompType)).ToList();
                }
                else if (SessionInfo.UserTypeId == UserType.Sort)
                {
                    subComponentId = vehicleComponentService.VehicleSubComponentType(0, vehicleTypeId,UserSchema.Sort);

                    listVehicleSubCompObj = listVehicleSubCompObj.Where(x => subComponentId.Contains((uint)x.SubCompType)).ToList();
                }
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/FillVehicleSubType JsonResult method completed successfully"));
                return Json(new { type = listVehicleSubCompObj, defaultType = defaultSubType });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}],Vehicle/FillVehicleSubType, Exception: {1}", Session.SessionID, ex.Message));
                throw ex;
            }
        }
        #endregion

        #region public ActionResult ComponentGeneralPage(int vehicleSubTypeId, int vehicleTypeId, int movementId, int? componentId, bool isComponent=false)
        /// <summary>
        /// ActionResult Method to populate General Component Page
        /// </summary>
        /// <param name="vehicleTypeId">vehicle typeId</param>
        /// <returns>partialView</returns>
        public ActionResult ComponentGeneralPage(int vehicleSubTypeId, int vehicleTypeId, int movementId, int? componentId, string GUID = "", bool isComponent = false, bool isLastComponent = false, int vehicleConfigId = 0, bool isNotify = false, int isFromConfig = 0, bool movement = false, bool isCandidate = false)
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
                VehicleComponent vehclCompObj = null;
                ComponentModel VehicleComponentObj = null;
                if (componentId != null)
                {
                    int compId = (int)componentId;

                    if (isFromConfig == 1 && vehicleConfigId == 0)
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
                //if (vehicleTypeId == 234008)
                //    movementId = 270003;
                //else
                    //movementId = 270001;
                vehclCompObj = GetVehicleComponent(vehicleSubTypeId, vehicleTypeId, movementId);

                //ViewBag.CompImageName = vehclCompObj.vehicleCompType.ImageName;
                ViewBag.CompImageName = vehclCompObj.vehicleComponentSubType.ImageName;

                if (VehicleComponentObj != null)
                {
                    vehclCompObj.UpdateVehicleProperties(VehicleComponentObj);
                }
                ViewBag.IsMovement = movement;
                ViewBag.IsCandidate = isCandidate;

                #region check last component in config
                if (Session["is_lastComponent"] != null)
                {
                    isLastComponent = (bool)Session["is_lastComponent"];
                }

                ViewBag.IsLastComp = isLastComponent;
                ViewBag.ImageName = vehclCompObj.vehicleComponentSubType.ImageName;

                //Session["is_lastComponent"] = null;
                #endregion

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/GeneralComponent ActionResult method completed successfully"));
                if (isFromConfig == 1)
                {
                    return PartialView("~/Views/VehicleConfig/VehicleComponentEdit.cshtml", vehclCompObj);
                }
                else
                {
                    return PartialView("ComponentGeneralPage", vehclCompObj);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}],Vehicle/GeneralComponent, Exception: {1}", Session.SessionID, ex.Message));
                throw ex;
            }
        }
        #endregion public ActionResult GeneralComponent(int vehicleSubTypeId, int vehicleTypeId, int movementId, int? componentId, bool isComponent=false)

        #region public ActionResult RegistrationComponent(int compId, bool isTractor, int? vehicleTypeId)
        public ActionResult RegistrationComponent(int compId, bool isTractor, int? vehicleTypeId, bool isApplication = false, bool isVR1 = false, int isFromConfig = 0, int vehicleConfigId = 0, bool movement = false, bool isCandidate = false) //vehicleTypeId added as a parameter to remove registration Id for rigid vehicle component type.
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/RegistrationComponent ActionResult method started successfully, with parameters compId:{1}, isTractor:{2}, vehicleTypeId:{3}", Session.SessionID, compId, isTractor, vehicleTypeId));
                ViewBag.IsTractor = isTractor;
                ViewBag.VehicleTypeId = vehicleTypeId;
                ViewBag.MakeConfig = TempData["MakeConfig"];
                //ViewBag.IsBtnFlage = isBtnFlage;
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                List<VehicleRegistration> listVehclRegObj = null;
                if (!isApplication && !isVR1)
                {
                    if (isFromConfig == 1 && vehicleConfigId == 0)
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
                return PartialView("RegistrationComponent", listVehclRegObj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}],Vehicle/RegistrationComponent, Exception: {1}", Session.SessionID, ex));
                throw ex;
            }
        }
        #endregion public ActionResult RegistrationComponent(int compId, bool isTractor, int? vehicleTypeId)

        #region public ActionResult ViewRegistrationComponent(int compId, bool isTractor, int? vehicleTypeId)
        public ActionResult ViewRegistrationComponent(int compId, bool isTractor, int? vehicleTypeId, bool isApplication = false, bool isVR1 = false, int isFromConfig = 0, bool movement = false, bool isRoute = false, bool isNotif = false) //vehicleTypeId added as a parameter to remove registration Id for rigid vehicle component type.
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

                if (isRoute && !movement)
                {
                    if (isVR1)
                    {
                        listVehclRegObj = vehicleComponentService.GetVR1RegistrationDetails(compId, SessionInfo.UserSchema);
                    }
                    else
                    {
                        listVehclRegObj = vehicleComponentService.GetRouteComponentRegistrationDetails(compId, SessionInfo.UserSchema);

                    }
                }
                else if (isNotif && !movement)
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

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/RegistrationComponent ActionResult method completed successfully"));
                return PartialView("ViewRegistrationComponent", listVehclRegObj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}],Vehicle/RegistrationComponent, Exception: {1}", Session.SessionID, ex));
                throw ex;
            }
        }
        #endregion public ActionResult RegistrationComponent(int compId, bool isTractor, int? vehicleTypeId)


        #region public JsonResult SaveRegistrationID(int compId, string registrationId, string fleetId)
        /// <summary>
        /// JsonResult method to save Registration details
        /// </summary>
        /// <returns>bool as json result</returns>
        [HttpPost]
        public JsonResult SaveRegistrationID(int compId, List<RegistrationParams> registrationParams, bool isApplication = false, bool isVR1 = false, int vehicleConfigId = 0, int isFromConfig = 0) //SaveRegistrationID(Name name)
        {/*string registrationId,string fleetId*/
            int IdNumber = 0;
            string ErrMsg = string.Empty;
            string sysEventDescp = string.Empty;
            try
            {
                UserInfo SessionInfo = new UserInfo();
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }

                var vehicleConfigList = vehicleComponentService.GetConfigForComponent(compId);
                vehicleConfigId = Convert.ToInt32(vehicleConfigList.VehicleId);

                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                movactiontype.UserName = SessionInfo.UserName;
                movactiontype.FleetComponentId = compId;

                if (!isApplication && !isVR1)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/SaveRegistrationID JsonResult method started successfully, with parameters compId:{1}", Session.SessionID, compId));
                    foreach (RegistrationParams registration in registrationParams)
                    {
                        IdNumber = vehicleComponentService.CreateRegistration(compId, registration.RegistrationValue, registration.FleetId);
                    }
                    if (vehicleConfigId > 0)
                    {
                        //update config registration also
                        movactiontype.FleetComponentId = vehicleConfigId;
                        foreach (RegistrationParams registration in registrationParams)
                        {
                            IdNumber = vehicleConfigService.CreateVehicleRegistration(vehicleConfigId, registration.RegistrationValue, registration.FleetId);
                        }
                    }


                    #region System Event Log - created_component_for_so_application
                    movactiontype.FleetComponentId = IdNumber;
                    movactiontype.SystemEventType = SysEventType.haulier_added_registration_for_fleet_component;

                    sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                    #endregion

                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/SaveRegistrationID JsonResult method completed successfully, Registration saved successfully", Session.SessionID));
                }
                else if (isVR1)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/SaveVR1CompRegistrationID JsonResult method started successfully, with parameters compId:{1}", Session.SessionID, compId));
                    foreach (RegistrationParams registration in registrationParams)
                    {
                        IdNumber = vehicleComponentService.CreateVR1CompRegistration(compId, registration.RegistrationValue, registration.FleetId, SessionInfo.UserSchema);
                    }
                    #region System Event Log - created_component_for_so_application
                    movactiontype.FleetVehicleId = compId;
                    movactiontype.FleetVehicleIdNo = IdNumber;
                    movactiontype.SystemEventType = SysEventType.haulier_added_registration_for_vr1_vehicle;

                    sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                    #endregion
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/SaveVR1CompRegistrationID JsonResult method completed successfully, Registration saved successfully"));
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/SaveAppCompRegistrationID JsonResult method started successfully, with parameters compId:{1}", Session.SessionID, compId));
                    foreach (RegistrationParams registration in registrationParams)
                    {
                        IdNumber = vehicleComponentService.CreateAppCompRegistration(compId, registration.RegistrationValue, registration.FleetId, SessionInfo.UserSchema);
                    }
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/SaveAppCompRegistrationID JsonResult method completed successfully, Registration saved successfully"));
                }

                //For saving component registration details at the time of vehicle save
                if (Session["vehicleWorkFlowParams"] != null)
                {
                    VehicleWorkFlowParams vehicleWorkFlow = (VehicleWorkFlowParams)Session["vehicleWorkFlowParams"];
                    vehicleWorkFlow.VehicleComponentsModels.Add(new VehicleComponentsModel { RegistrationDetails = registrationParams });

                    Session["vehicleWorkFlowParams"] = vehicleWorkFlow;
                }
                else
                {
                    VehicleWorkFlowParams vehicleWorkFlowParams = new VehicleWorkFlowParams();
                    vehicleWorkFlowParams.VehicleComponentsModels.Add(new VehicleComponentsModel { RegistrationDetails = registrationParams });

                    Session["vehicleWorkFlowParams"] = vehicleWorkFlowParams;
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}],Vehicle/SaveRegistrationID, Exception: {1}", Session.SessionID, ex));
                throw ex;
            }
            return Json(new { Success = IdNumber });
        }
        #endregion public JsonResult SaveRegistrationID(int compId, string registrationId, string fleetId)

        #region public JsonResult DeleteVehicleRegistration(int componentId, int IdNumber)
        [HttpPost]
        public JsonResult DeleteVehicleRegistration(int componentId, int IdNumber, bool isMovement = false, int isFromConfig = 0, int vehicleConfigId = 0)
        {
            try
            {
                UserInfo SessionInfo = new UserInfo();
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }

                string ErrMsg = string.Empty;
                string sysEventDescp = string.Empty;

                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                movactiontype.UserName = SessionInfo.UserName;
                movactiontype.FleetComponentId = componentId;
                movactiontype.FleetComponentIdNo = IdNumber;


                //if (!isApplication)
                //{
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/DeleteVehicleRegistration ActionResult method started successfully, with parameters componentId:{1}, IdNumber:{2}", Session.SessionID, componentId, IdNumber));
                if (isMovement)
                {
                    bool result = vehicleComponentService.DeleteComponentRegister(componentId, IdNumber, 2);
                }
                else if (isFromConfig == 1 && vehicleConfigId == 0)
                {
                    bool result = vehicleComponentService.DeleteComponentRegister(componentId, IdNumber, 1);
                }
                else
                {
                    bool result = vehicleComponentService.DeleteComponentRegister(componentId, IdNumber, 0);
                }
                movactiontype.SystemEventType = SysEventType.haulier_deleted_registration_for_fleet_component;
                //if (vehicleConfigId > 0)
                //{
                //    result = vehicleConfigService.DeletVehicleRegisterConfiguration(vehicleConfigId, IdNumber);
                //    movactiontype.SystemEventType = SysEventType.haulier_deleted_registration_for_fleet_vehicle;
                //    movactiontype.FleetVehicleId = vehicleConfigId;
                //    movactiontype.FleetVehicleIdNo = IdNumber;
                //    sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                //    bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);

                //    movactiontype.SystemEventType = 0;
                //}

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/DeleteVehicleRegistration ActionResult method completed successfully"));
                //}
                //else if (isVR1)
                //{
                //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/DeleteVehicleRegistration ActionResult method started successfully, with parameters componentId:{1}, IdNumber:{2}", Session.SessionID, componentId, IdNumber));
                //    bool result = vehicleComponentService.DeleteVR1VehComponentRegister(componentId, IdNumber, SessionInfo.UserSchema);
                //    if (SessionInfo.UserSchema == UserSchema.Sort)
                //        movactiontype.SystemEventType = SysEventType.sort_deleted_registration_for_vr1_component;
                //    else
                //        movactiontype.SystemEventType = SysEventType.haulier_deleted_registration_for_vr1_component;
                //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/DeleteVehicleRegistration ActionResult method completed successfully"));
                //}
                //else
                //{
                //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/DeleteVehicleRegistration ActionResult method started successfully, with parameters componentId:{1}, IdNumber:{2}", Session.SessionID, componentId, IdNumber));
                //    bool result = vehicleComponentService.DeleteAppVehComponentRegister(componentId, IdNumber, SessionInfo.UserSchema);
                //    if (SessionInfo.UserSchema == UserSchema.Sort)
                //        movactiontype.SystemEventType = SysEventType.sort_deleted_registration_for_so_component;
                //    else
                //        movactiontype.SystemEventType = SysEventType.haulier_deleted_registration_for_so_component;
                //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/DeleteVehicleRegistration ActionResult method completed successfully"));
                //}

                if (movactiontype.SystemEventType != 0)
                {
                    sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                    bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}],Vehicle/DeleteVehicleRegistration, Exception: {1}", Session.SessionID, ex));
                throw ex;
            }
            return Json(new { Success = true });
        }
        #endregion public JsonResult DeleteVehicleRegistration(int componentId, int IdNumber)

        #region public ActionResult AxleComponent(int axleCount, int componentId, int vehicleSubTypeId, int vehicleTypeId, int movementId, int? weight, bool IsEdit)
        public ActionResult AxleComponent(int axleCount, int componentId, int vehicleSubTypeId, int vehicleTypeId, int movementId, int? weight, bool IsEdit, bool isApplication = false, int appRevID = 0, long vehConfigID = 0, bool isVR1 = false, int isFromConfig = 0, int vehicleConfigId = 0, bool movement = false, bool isCandidate = false, bool isLastComponent = false)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/AxleComponent ActionResult method started successfully, with parameters axleCount:{1}, componentId:{2}, vehicleSubTypeId:{3}, vehicleTypeId:{4}, weight:{5}, IsEdit:{6}", Session.SessionID, axleCount, componentId, vehicleSubTypeId, vehicleTypeId, weight, IsEdit));
                ViewBag.AxleCount = axleCount;
                ViewBag.IsFromConfig = isFromConfig;
                ViewBag.IsLastComponent = true;
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                ViewBag.isvr1 = isVR1;
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
                VehicleComponent vhclCompObj = GetVehicleComponent(vehicleSubTypeId, vehicleTypeId, movementId);
                AxleValidator axleValidator = vhclCompObj.vehicleComponentSubType.axleValidator;
                axleValidator.IsConfigureTyreCentreSpacing = vhclCompObj.IsConfigTyreCentreSpacing;
                List<Axle> axleList = new List<Axle>();

                if (IsEdit)
                {
                    STP.Domain.LoggingAndReporting.MovementActionIdentifiers movactiontype = new STP.Domain.LoggingAndReporting.MovementActionIdentifiers();
                    movactiontype.ComponentId = componentId;
                    movactiontype.RevisionId = appRevID;
                    //movactiontype.vehicle_id = Convert.ToInt32(vehConfigID);
                    movactiontype.FleetComponentId = Convert.ToInt32(vehConfigID);
                    movactiontype.UserName = SessionInfo.UserName;
                    string ErrMsg = string.Empty;
                    int user_ID = Convert.ToInt32(SessionInfo.UserId);

                    if (!isApplication && !isVR1)
                    {
                        if (isFromConfig == 1 && vehicleConfigId == 0)
                        {
                            axleList = vehicleComponentService.ListAxleTemp(componentId, false, SessionInfo.UserSchema);
                        }
                        else if (isCandidate)
                        {
                            axleList = vehicleComponentService.ListVR1vehAxle(componentId, SessionInfo.UserSchema);
                        }
                        else if (movement)
                        {
                            axleList = vehicleComponentService.ListAxleTemp(componentId, movement, SessionInfo.UserSchema);
                        }
                        else
                        {
                            axleList = vehicleComponentService.ListAxle(componentId);
                        }
                        #region System events for edited_axle_details_for_fleet_component
                        if (axleList.Count > 0)
                        {
                            //movactiontype.systemEventType = SysEventType.haulier_edited_axle_details_for_fleet_component;
                        }
                        #endregion
                        ViewBag.AxleList = axleList;
                    }
                    else if (isVR1)
                    {
                        axleList = vehicleComponentService.ListVR1vehAxle(componentId, SessionInfo.UserSchema);
                        ViewBag.AxleList = axleList;

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
                        axleList = vehicleComponentService.ListAppvehAxle(componentId, SessionInfo.UserSchema);
                        ViewBag.AxleList = axleList;

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

                    ViewBag.AxleCount = axleList.Count;
                }
                else
                {
                    ViewBag.AxleList = null;
                }
                ViewBag.componentId = componentId;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/AxleComponent ActionResult method completed successfully"));
                return PartialView("AxleComponent", axleValidator);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}],Vehicle/AxleComponent, Exception: {1}", Session.SessionID, ex));
                throw ex;
            }
        }
        #endregion public ActionResult AxleComponent(int axleCount, int componentId, int vehicleSubTypeId, int vehicleTypeId, int movementId, int? weight, bool IsEdit)

        #region public JsonResult SaveAxles(List<Axle> axleList, int componentId, bool isApplication=false)
        [HttpPost]
        public JsonResult SaveAxles(List<Axle> axleList, int componentId, bool isApplication = false, bool isVR1 = false, int appRevID = 0, long vehicleId = 0, int NotificationID = 0, bool isEdit = false, int isFromConfig = 0)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/SaveAxles actionResult method started successfully with parameters List of Axles object and componentId:{1}", Session.SessionID, componentId));
            UserInfo SessionInfo = new UserInfo();
            bool success = false;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }
            int organisationId = (int)SessionInfo.OrganisationId;
            MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
            movactiontype.RevisionId = appRevID;
            movactiontype.VehicleId = Convert.ToInt32(vehicleId);
            movactiontype.UserName = SessionInfo.UserName;
            movactiontype.FleetComponentId = componentId;
            movactiontype.NotificationID = NotificationID;
            string ErrMsg = string.Empty;
            int user_ID = Convert.ToInt32(SessionInfo.UserId);
            try
            {
                if (!isApplication)
                {
                    if (axleList != null)
                    {
                        foreach (Axle axle in axleList)
                        {
                            vehicleComponentService.UpdateAxle(axle, componentId);
                        }
                    }
                    if (isEdit)
                    {
                        movactiontype.SystemEventType = SysEventType.haulier_edited_axle_details_for_fleet_component;
                    }
                    else
                    {
                        movactiontype.SystemEventType = SysEventType.haulier_added_axle_details_for_fleet_component;
                    }
                    string sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                    bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                }
                else if (isVR1)
                {
                    foreach (Axle axle in axleList)
                    {
                        vehicleComponentService.UpdateVR1Axle(axle, componentId, SessionInfo.UserSchema);
                    }

                    #region System events for Sort_created_axle_for_vr1_application
                    if (SessionInfo.UserSchema == UserSchema.Sort) // For SORT Axle creation Log
                    {
                        #region Saving Sort_created_axle_for_vr1_application
                        movactiontype.SystemEventType = SysEventType.Sort_created_axle_for_vr1_application;
                        #endregion
                    }
                    else if (SessionInfo.UserSchema == UserSchema.Portal)
                    {
                        if (NotificationID == 0)
                        {
                            if (!isEdit)
                            {
                                movactiontype.SystemEventType = SysEventType.Haulier_created_axle_for_vr1_application;
                            }
                            else
                            {
                                movactiontype.SystemEventType = SysEventType.Haulier_edited_axle_for_vr1_application;
                            }
                        }
                        else
                        {
                            if (!isEdit)
                            {
                                movactiontype.SystemEventType = SysEventType.Haulier_created_axle_for_notification;
                            }
                            else
                            {
                                movactiontype.SystemEventType = SysEventType.Haulier_edited_axle_for_notification;
                            }

                        }
                    }
                    string sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                    //bool sysEvntResult = STP.Business.Persistence.MovementActionDAO.SaveSysEvents(movactiontype, sysEventDescp, user_ID, SessionInfo.userSchema);
                    bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                    #endregion
                }
                else
                {
                    foreach (Axle axle in axleList)
                    {
                        vehicleComponentService.UpdateAppAxle(axle, componentId, SessionInfo.UserSchema);
                    }

                    #region System events for sort_created_axle_for_so_application
                    if (SessionInfo.UserSchema == UserSchema.Sort) // For SORT Axle creation Log
                    {
                        #region Saving created_new_vehicle_for_so_application
                        movactiontype.SystemEventType = SysEventType.sort_created_axle_for_so_application;
                        #endregion
                    }
                    else if (SessionInfo.UserSchema == UserSchema.Portal)
                    {
                        #region Saving haulier created_new_vehicle_for_so_application
                        if (!isEdit)
                        {
                            movactiontype.SystemEventType = SysEventType.Haulier_created_axle_for_so_application;
                        }
                        else
                        {
                            movactiontype.SystemEventType = SysEventType.Haulier_edited_axle_for_so_application;
                        }
                        #endregion
                    }
                    string sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                    bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                    #endregion
                }

                //For saving component registration details at the time of vehicle save
                if (Session["vehicleWorkFlowParams"] != null)
                {
                    VehicleWorkFlowParams vehicleWorkFlow = (VehicleWorkFlowParams)Session["vehicleWorkFlowParams"];
                    vehicleWorkFlow.VehicleComponentsModels.Add(new VehicleComponentsModel { AxleDetails = axleList });

                    Session["vehicleWorkFlowParams"] = vehicleWorkFlow;
                }
                else
                {
                    VehicleWorkFlowParams vehicleWorkFlowParams = new VehicleWorkFlowParams();
                    vehicleWorkFlowParams.VehicleComponentsModels.Add(new VehicleComponentsModel { AxleDetails = axleList });

                    Session["vehicleWorkFlowParams"] = vehicleWorkFlowParams;
                }
                success = true;

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/SaveAxles,  Axles added successfully"));

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}],Vehicle/SaveAxles, Exception: {1}", Session.SessionID, ex));
                throw ex;
            }
            return Json(new { success = success });
        }
        #endregion public JsonResult SaveAxles(List<Axle> axleList, int componentId, bool isApplication=false)

        #region public ActionResult UpdateComponent()
        public ActionResult UpdateComponent(int componentId)
        {
            ViewBag.ComponentId = componentId;
            bool isAdmin = false;

            if (Session["UserInfo"] != null)
            {
                var sessionValues = (UserInfo)Session["UserInfo"];
                int portalType = sessionValues.UserTypeId;
                if (portalType == 696006 || sessionValues.IsAdmin == 1)
                {
                    isAdmin = true;
                }
            }
            ViewBag.IsAdmin = isAdmin;

            VehicleComponent vehclCompObj = null;
            ComponentModel VehicleComponentObj = null;
            VehicleComponentObj = vehicleComponentService.GetVehicleComponent(componentId);

            ViewBag.VehicleTypeId = VehicleComponentObj.ComponentType;
            ViewBag.VehicleSubTypeId = VehicleComponentObj.ComponentSubType;
            ViewBag.MovementId = VehicleComponentObj.VehicleIntent;
            return View();
        }
        #endregion public ActionResult UpdateComponent()

        #region public JsonResult UpdateComponent(VehicleComponent vehicleComponent, int componentId)
        [HttpPost]
        public JsonResult UpdateComponent(VehicleComponent vehicleComponent, int componentId, List<RegistrationParams> registrationParams, List<Axle> axleList, bool isApplication = false, bool isVR1 = false, int vehicleConfigId = 0, int vehicleType = 0, int vehicleIntend = 0, int appRevID = 0, int NotificationID = 0, int IsFromConfig = 0, bool planMovement = false, bool isCandidate = false)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/UpdateComponent actionResult method started successfully with parameters VehicleComponent object and componentId:{1}", Session.SessionID, componentId));
            UserInfo SessionInfo = new UserInfo();
            bool success = false;
            string ErrMsg = string.Empty;
            string sysEventDescp = string.Empty;
            ViewBag.MakeConfig = 0;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }
            MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
            movactiontype.UserName = SessionInfo.UserName;

            int organisationId = (int)SessionInfo.OrganisationId;
            var isFromConfigFlag = TempData["ChkIsFromConfig"];
            ViewBag.IsFromConfig = isFromConfigFlag;

            string guid = "";

            try
            {
                if (!isApplication)
                {
                    ComponentModel componentObj = ConvertToComponent(vehicleComponent);
                    componentObj.OrganisationId = organisationId;
                    componentObj.ComponentId = componentId;

                    if (vehicleConfigId == 0)
                    {
                        ///make as configuration methods
                        ///need to implement functionality
                        string makeConfig = (from s in vehicleComponent.VehicleParamList
                                             where s.ParamModel == "MakeConfig"
                                             select s.ParamValue).FirstOrDefault();
                        ///check makeConfig not null and is "1" then check configuration exists
                        ///if new config name create component then configuration and finally vehicle position
                        if (!string.IsNullOrEmpty(makeConfig) && makeConfig == "1")
                        {
                            ///check configuration name exists
                            ///if exists return json or value
                            string internalName = (from s in vehicleComponent.VehicleParamList
                                                   where s.ParamModel == "Internal_Name"
                                                   select s.ParamValue).FirstOrDefault();

                            int configNameCount = vehicleComponentService.CheckConfigNameExists(internalName, organisationId);


                            ViewBag.MakeConfig = 1;
                            if (configNameCount > 0)
                            {
                                return Json(new { Success = 0 });
                            }

                            ///else
                            ///
                            if (IsFromConfig == 1)
                            {
                                if (new SessionData().Wf_Fm_FleetManagementId.ToLower() != "failed")
                                {
                                    componentObj.GUID = new SessionData().Wf_Fm_FleetManagementId;
                                }
                                success = vehicleComponentService.UpdateComponentTemp(componentObj);
                            }
                            else
                            {
                                success = vehicleComponentService.UpdateComponent(componentObj);
                            }


                            ///create this configuration
                            ///
                            if (componentId > 0)
                            {
                                int vhclType = 0;
                                if (vehicleType == 234003)
                                {
                                    vhclType = 244003;
                                }
                                else if (vehicleType == 234007)
                                {
                                    vhclType = 244005;
                                }
                                VehicleConfiguration config = CastComponentToVehicle(vehicleComponent);
                                NewConfigurationModel vehicleConfiguration = ConvertToConfiguration(config);
                                vehicleConfiguration.OrganisationId = organisationId;
                                vehicleConfiguration.VehicleType = vhclType;
                                vehicleConfiguration.VehiclePurpose = vehicleIntend;
                                double configurationid = vehicleConfigService.CreateConfiguration(vehicleConfiguration);

                                int cloneReg = vehicleComponentService.CreateVehicleRegFromCompReg(componentId, Convert.ToInt32(configurationid));


                                ///insert the vehicle and component into vehicle position table with lat and long position as 1
                                VehicleConfigList ConfigPosn = new VehicleConfigList();
                                ConfigPosn.VehicleId = Convert.ToInt64(configurationid);
                                ConfigPosn.ComponentId = Convert.ToInt64(componentId);
                                ConfigPosn.LatPosn = 1;
                                ConfigPosn.LongPosn = 1;
                                ConfigPosn.SubType = componentObj.ComponentType;

                                var vehicleConfigList = vehicleComponentService.CreateConfPosnComponent(ConfigPosn);

                                componentId = Convert.ToInt32(vehicleConfigList.VehicleId);
                                return Json(new { result = success, configId = vehicleConfigList.VehicleId });
                            }
                        }
                        else
                        {
                            if (IsFromConfig == 1 && vehicleConfigId == 0)
                            {
                                if (new SessionData().Wf_Fm_FleetManagementId.ToLower() != "failed")
                                {
                                    componentObj.GUID = new SessionData().Wf_Fm_FleetManagementId;
                                    guid = new SessionData().Wf_Fm_FleetManagementId;
                                }
                                success = vehicleComponentService.UpdateComponentTemp(componentObj);
                            }
                            else if (planMovement)
                            {
                                success = vehicleComponentService.UpdateMovementComponentTemp(componentObj, SessionInfo.UserSchema);
                            }
                            else
                            {
                                success = vehicleComponentService.UpdateComponent(componentObj);
                            }

                        }
                    }
                    else
                    {
                        if (IsFromConfig == 1 && vehicleConfigId == 0)
                        {
                            if (new SessionData().Wf_Fm_FleetManagementId.ToLower() != "failed")
                            {
                                componentObj.GUID = new SessionData().Wf_Fm_FleetManagementId;
                                guid = new SessionData().Wf_Fm_FleetManagementId;
                            }
                            success = vehicleComponentService.UpdateComponentTemp(componentObj);
                        }
                        else if (isCandidate)
                        {
                            success = vehicleComponentService.UpdateVR1VehComponent(componentObj, SessionInfo.UserSchema);
                        }
                        else if (planMovement)
                        {
                            success = vehicleComponentService.UpdateMovementComponentTemp(componentObj, SessionInfo.UserSchema);
                        }
                        else
                        {
                            success = vehicleComponentService.UpdateComponent(componentObj);
                        }

                    }
                    //success = true;
                    if (success)
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/UpdateComponent,  component updated successfully", Session.SessionID));

                        int IdNumber = 0;
                        if (registrationParams != null)
                        {
                            foreach (RegistrationParams registration in registrationParams)
                            {
                                if (IsFromConfig == 1 && vehicleConfigId == 0)
                                {
                                    IdNumber = vehicleComponentService.CreateRegistrationTemp(componentId, registration.RegistrationValue, registration.FleetId, false, SessionInfo.UserSchema);
                                }
                                else if (isCandidate)
                                {
                                    IdNumber = vehicleComponentService.CreateVR1CompRegistration(componentId, registration.RegistrationValue, registration.FleetId, SessionInfo.UserSchema);
                                }
                                else if (planMovement)
                                {
                                    IdNumber = vehicleComponentService.CreateRegistrationTemp(componentId, registration.RegistrationValue, registration.FleetId, planMovement, SessionInfo.UserSchema);
                                }
                                else
                                {
                                    IdNumber = vehicleComponentService.CreateRegistration(componentId, registration.RegistrationValue, registration.FleetId);
                                }
                            }
                        }

                        if (axleList != null)
                        {
                            foreach (Axle axle in axleList)
                            {
                                if (IsFromConfig == 1 && vehicleConfigId == 0)
                                {
                                    vehicleComponentService.InsertAxleDetailsTemp(axle, componentId, false, SessionInfo.UserSchema);
                                }
                                else if (isCandidate)
                                {
                                    vehicleComponentService.UpdateVR1Axle(axle, componentId, SessionInfo.UserSchema);
                                }
                                else if (planMovement)
                                {
                                    vehicleComponentService.InsertAxleDetailsTemp(axle, componentId, planMovement, SessionInfo.UserSchema);
                                }
                                else
                                {
                                    vehicleComponentService.UpdateAxle(axle, componentId);
                                }
                            }
                        }

                        movactiontype.FleetComponentName = componentObj.IntendedName;
                        movactiontype.FleetComponentId = (long)componentObj.ComponentId;
                        if (vehicleConfigId > 0)
                        {
                            movactiontype.FleetVehicleId = (long)vehicleConfigId;
                            movactiontype.SystemEventType = SysEventType.Haulier_edited_component;
                        }
                        else
                        {
                            movactiontype.SystemEventType = SysEventType.haulier_edited_fleet_component;
                        }
                        try
                        {
                            if (IsFromConfig == 1)
                            {
                                //Fm_Wf: Update Component
                                var componentList = GetPayloadComponents();
                                if (componentList.Count > 0)
                                {
                                    VehicleComponentsModel vehicleComponentsModel = new VehicleComponentsModel();
                                    vehicleComponentsModel = componentList.SingleOrDefault(x => x.ComponentModel.ComponentId.Equals(componentObj.ComponentId));
                                    componentList.RemoveAll(x => x.ComponentModel != null && x.ComponentModel.ComponentId.Equals(componentId));
                                    vehicleComponentsModel.ComponentModel = componentObj;
                                    componentList.Add(vehicleComponentsModel);
                                }

                                VehicleWorkFlowParams vehicleWorkFlow = (VehicleWorkFlowParams)Session["vehicleWorkFlowParams"];
                                if (vehicleWorkFlow != null)
                                {
                                    vehicleWorkFlow.VehicleComponentsModels = componentList;
                                    vehicleWorkFlow.TotalComponents = componentList.Count;
                                    Session["vehicleWorkFlowParams"] = vehicleWorkFlow;
                                    ProcessWorkflowActivity(componentList);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] ,Workflow: VehicleController/UpdateComponent, Exception: {1}", Session.SessionID, ex.Message));
                        }

                    }
                    else
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.WARNING, string.Format("[{0}],Vehicle/UpdateComponent,  component not updated, Internal Name already exists", Session.SessionID));
                    }
                }
                else if (isVR1)
                {

                    ComponentModel componentObj = ConvertToComponent(vehicleComponent);
                    componentObj.OrganisationId = organisationId;
                    componentObj.ComponentId = componentId;



                    success = vehicleComponentService.UpdateVR1VehComponent(componentObj, SessionInfo.UserSchema);
                    //success = true;
                    if (success)
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/UpdateVR1VehComponent,  component updated successfully", Session.SessionID));
                        movactiontype.VehicleId = vehicleConfigId;
                        movactiontype.ComponentId = (int)componentObj.ComponentId;
                        movactiontype.RevisionId = appRevID;
                        movactiontype.UserName = SessionInfo.UserName;
                        movactiontype.NotificationID = NotificationID;
                        if (SessionInfo.UserSchema == UserSchema.Sort)
                        {
                            movactiontype.SystemEventType = SysEventType.Sort_edited_component_for_vr1_application;
                        }
                        else if (SessionInfo.UserSchema == UserSchema.Portal)
                        {
                            if (NotificationID != 0)
                            {
                                movactiontype.SystemEventType = SysEventType.Haulier_edited_component_for_notification;
                            }
                            else
                            {
                                movactiontype.SystemEventType = SysEventType.Haulier_edited_component_for_vr1_application;
                            }
                        }

                    }
                    else
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.WARNING, string.Format("[{0}],Vehicle/UpdateVR1VehComponent,  component not updated, Internal Name already exists", Session.SessionID));
                    }
                }
                else
                {

                    ComponentModel componentObj = ConvertToComponent(vehicleComponent);
                    componentObj.OrganisationId = organisationId;
                    componentObj.ComponentId = componentId;

                    success = vehicleComponentService.UpdateAppVehComponent(componentObj, SessionInfo.UserSchema);
                    //success = true;

                    if (success)
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/UpdateAppVehComponent,  component updated successfully", Session.SessionID));
                        movactiontype.VehicleId = vehicleConfigId;
                        movactiontype.ComponentId = (int)componentObj.ComponentId;
                        movactiontype.RevisionId = appRevID;
                        movactiontype.UserName = SessionInfo.UserName;
                        if (SessionInfo.UserSchema == UserSchema.Sort)
                        {
                            movactiontype.SystemEventType = SysEventType.sort_edited_component_for_so_application;
                        }
                        else if (SessionInfo.UserSchema == UserSchema.Portal)
                        {
                            movactiontype.SystemEventType = SysEventType.Haulier_edited_component_for_so_application;
                        }
                    }
                    else
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.WARNING, string.Format("[{0}],Vehicle/UpdateAppVehComponent,  component not updated, Internal Name already exists", Session.SessionID));
                    }
                }
                if (movactiontype.SystemEventType != 0)
                {
                    sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);

                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}],Vehicle/UpdateComponent, Exception: {1}", Session.SessionID, ex));
                throw ex;
            }
            //return Json(new { result = success, configId = componentId, isFromConfigFlag = isFromConfigFlag });

            return Json(new { result = success, Guid = guid, configId = vehicleConfigId });

        }
        #endregion public JsonResult ImportComponent(VehicleComponent vehicleComponent, int componentId)

        [HttpPost]
        public JsonResult ImportComponent(int componentId, bool IsNotif = false, bool isVR1 = false, bool isRoute = false, bool isMovement = false, int vehicleId = 0)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/ViewComponent ActionResult method started successfully, with parameters componentId:{1}", Session.SessionID, componentId));
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

            if (new SessionData().Wf_Fm_FleetManagementId.Length == 0)
            {
                //Fm_Wf: Import from fleet process starts here.
                new SessionData().Wf_Fm_ImportFromFleet = true;
                StartFleetManagementWorkflow(SessionInfo.OrganisationName, true);
            }
            else
            {
                //Fm_Wf: Once process starts update activity.
                ProcessWorkflowActivity(null);
            }

            VehicleComponent vehclCompObj = new VehicleComponent();
            ComponentModel VehicleComponentObj = null;
            List<VehicleRegistration> listVehclRegObj = null;
            List<Axle> axleList = null;
            ViewBag.AxleList = null;

            string guid = "";

            int vehicleTypeId = 0;
            int vehicleSubTypeId = 0;
            int movementId = 0;

            if (componentId != null)
            {
                int compId = (int)componentId;
                if (isRoute)
                {
                    if (isVR1)
                    {
                        VehicleComponentObj = vehicleComponentService.GetVR1VehicleComponent(compId, SessionInfo.UserSchema);
                    }
                    else
                    {
                        VehicleComponentObj = vehicleComponentService.GetRouteComponent(compId, SessionInfo.UserSchema);
                    }
                }
                else if (IsNotif)
                {
                    VehicleComponentObj = vehicleComponentService.GetVR1VehicleComponent(compId, SessionInfo.UserSchema);
                    if (SessionInfo.UserSchema == UserSchema.Portal)
                    {
                        if (vehicleSubTypeId == 0) { VehicleComponentObj.ComponentSubType = 224002; }
                        if (vehicleTypeId == 0) { VehicleComponentObj.ComponentType = 234002; }
                    }
                }
                else
                {
                    VehicleComponentObj = vehicleComponentService.GetVehicleComponent(compId);

                }
                if (VehicleComponentObj.ComponentType != 0)
                {
                    vehicleTypeId = VehicleComponentObj.ComponentType;
                    vehicleSubTypeId = VehicleComponentObj.ComponentSubType;
                    movementId = VehicleComponentObj.VehicleIntent;

                    if (new SessionData().Wf_Fm_FleetManagementId.ToLower() != "failed")
                    {
                        VehicleComponentObj.GUID = new SessionData().Wf_Fm_FleetManagementId;
                        guid = new SessionData().Wf_Fm_FleetManagementId;
                    }

                    AssignMovementClassification(VehicleComponentObj.VehicleIntent);

                    if (isRoute)
                    {
                        if (isVR1)
                        {
                            listVehclRegObj = vehicleComponentService.GetVR1RegistrationDetails(compId, SessionInfo.UserSchema);
                            axleList = vehicleComponentService.ListVR1vehAxle(compId, SessionInfo.UserSchema);
                        }
                        else
                        {
                            listVehclRegObj = vehicleComponentService.GetRouteComponentRegistrationDetails(compId, SessionInfo.UserSchema);

                            axleList = vehicleComponentService.ListRouteComponentAxle(compId, SessionInfo.UserSchema);
                        }
                    }
                    else if (IsNotif)
                    {
                        listVehclRegObj = vehicleComponentService.GetVR1RegistrationDetails(compId, SessionInfo.UserSchema);
                        axleList = vehicleComponentService.ListVR1vehAxle(compId, SessionInfo.UserSchema);

                    }
                    else
                    {
                        listVehclRegObj = vehicleComponentService.GetRegistrationDetails(compId);
                        axleList = vehicleComponentService.ListAxle(compId);
                    }
                    ViewBag.AxleList = axleList;

                    vehclCompObj = GetVehicleComponent(vehicleSubTypeId, vehicleTypeId, movementId);

                    if (VehicleComponentObj != null)
                        vehclCompObj.UpdateVehicleProperties(VehicleComponentObj);

                    if (listVehclRegObj != null && listVehclRegObj.Count != 0)
                    {
                        vehclCompObj.ListVehicleReg = new List<VehicleRegistration>();
                        vehclCompObj.ListVehicleReg = listVehclRegObj;
                    }

                    //------ create component------------------------------
                    //newComponentId = vehicleComponentService.CreateComponent(VehicleComponentObj);

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
                                //IdNumber = vehicleComponentService.CreateRegistration((int)newComponentId, registration.RegistrationId, registration.FleetId);

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
                                //vehicleComponentService.UpdateAxle(axle, (int)newComponentId);

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
                        if (isMovement && vehicleId != 0)
                        {
                            bool result = vehicleConfigService.InsertMovementConfigPosnTemp(guid, vehicleId, SessionInfo.UserSchema);
                        }
                        VehicleComponentObj.ComponentId = newComponentId;
                        if (Session["vehicleWorkFlowParams"] != null)
                        {
                            VehicleWorkFlowParams vehicleWorkFlow = (VehicleWorkFlowParams)Session["vehicleWorkFlowParams"];
                            vehicleWorkFlow.VehicleComponentsModels.Add(new VehicleComponentsModel { ComponentModel = VehicleComponentObj, RegistrationDetails = registrationParams, AxleDetails = axleList });

                            Session["vehicleWorkFlowParams"] = vehicleWorkFlow;
                        }
                        else
                        {
                            VehicleWorkFlowParams vehicleWorkFlowParams = new VehicleWorkFlowParams();
                            vehicleWorkFlowParams.VehicleComponentsModels.Add(new VehicleComponentsModel { ComponentModel = VehicleComponentObj, RegistrationDetails = registrationParams, AxleDetails = axleList });

                            Session["vehicleWorkFlowParams"] = vehicleWorkFlowParams;
                        }

                    }
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/ViewComponent ActionResult method completed successfully"));

                }
            }

            return Json(new { ConfigId = vehicleId, Guid = guid });
        }
        [HttpPost]
        public JsonResult BackButtonToConfig()
        {
            string guid = "";
            if (new SessionData().Wf_Fm_FleetManagementId.ToLower() != "failed")
            {
                guid = new SessionData().Wf_Fm_FleetManagementId;
            }
            return Json(guid);
        }
        public ActionResult ComponentButton(int isFromConfig = 0)
        {
            ViewBag.IsFromConfig = isFromConfig;
            return PartialView("ComponentButton");
        }

        [HttpPost]
        public JsonResult AxleValidationCalculation(int? weight)
        {
            string weightRange = "";
            if (weight != null)
            {
                weightRange = AxleValidator.GetAxleToleranceRange((int)weight).getRangeString();
                ViewBag.AxleWeightTolerance = weightRange;
            }
            else
            {
                ViewBag.AxleWeightTolerance = "";
            }
            return Json(weightRange);
        }


        #region public ActionResult VehicleComponentDetails(int vehicleSubTypeId, int vehicleTypeId, int movementId, int? componentId, bool isRoute=false)
        public ActionResult VehicleComponentDetails(int vehicleSubTypeId, int vehicleTypeId, int movementId, int? componentId, bool isRoute = false, int latPos = 0, int longPos = 0, bool IsNotif = false, bool isVR1 = false, bool isLastComponent = false, int compNumber = 1,int configTypeId=0)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/VehicleComponentDetails ActionResult method started successfully, with parameters vehicleSubTypeId:{1}, vehicleTypeId:{2}, movementId:{3}, componentId:{4}", Session.SessionID, vehicleSubTypeId, vehicleTypeId, movementId, componentId));
                ViewBag.ComponentId = componentId;
                ViewBag.isNotif = IsNotif;
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }

                VehicleComponent vehclCompObj = new VehicleComponent();
                ComponentModel VehicleComponentObj = null;
                List<VehicleRegistration> listVehclRegObj = null;
                List<Axle> axleList = null;
                ViewBag.AxleList = null;
                if (componentId != null)
                {
                    int compId = (int)componentId;
                    if (isRoute)
                    {
                        if (isVR1)
                        {
                            VehicleComponentObj = vehicleComponentService.GetVR1VehicleComponent(compId, SessionInfo.UserSchema);
                        }
                        else
                        {
                            VehicleComponentObj = vehicleComponentService.GetRouteComponent(compId, SessionInfo.UserSchema);
                        }
                    }
                    else if (IsNotif)
                    {
                        VehicleComponentObj = vehicleComponentService.GetVR1VehicleComponent(compId, SessionInfo.UserSchema);
                        if (SessionInfo.UserSchema == UserSchema.Portal)
                        {
                            if (vehicleSubTypeId == 0) { VehicleComponentObj.ComponentSubType = 224002; }
                            if (vehicleTypeId == 0) { VehicleComponentObj.ComponentType = 234002; }
                        }
                    }
                    else
                    {
                        VehicleComponentObj = vehicleComponentService.GetVehicleComponent(compId);
                    }
                    if (VehicleComponentObj.ComponentType != 0)
                    {
                        vehicleTypeId = VehicleComponentObj.ComponentType;
                        vehicleSubTypeId = VehicleComponentObj.ComponentSubType;
                        if(VehicleComponentObj.VehicleIntent!=0)
                            movementId = VehicleComponentObj.VehicleIntent;
                        if (isRoute)
                        {
                            if (isVR1)
                            {
                                listVehclRegObj = vehicleComponentService.GetVR1RegistrationDetails(compId, SessionInfo.UserSchema);
                                axleList = vehicleComponentService.ListVR1vehAxle(compId, SessionInfo.UserSchema);
                            }
                            else
                            {
                                listVehclRegObj = vehicleComponentService.GetRouteComponentRegistrationDetails(compId, SessionInfo.UserSchema);

                                axleList = vehicleComponentService.ListRouteComponentAxle(compId, SessionInfo.UserSchema);
                            }
                        }
                        else if (IsNotif)
                        {
                            listVehclRegObj = vehicleComponentService.GetVR1RegistrationDetails(compId, SessionInfo.UserSchema);
                            axleList = vehicleComponentService.ListVR1vehAxle(compId, SessionInfo.UserSchema);

                        }
                        else
                        {
                            listVehclRegObj = vehicleComponentService.GetRegistrationDetails(compId);

                            axleList = vehicleComponentService.ListAxle(compId);
                        }
                        ViewBag.AxleList = axleList;
                        int MovementXmlTypeId = STP.Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleXmlMovementTypeMapping((VehicleXmlMovementType)movementId);

                        vehclCompObj = GetVehicleComponent(vehicleSubTypeId, vehicleTypeId, MovementXmlTypeId);
                        if (vehicleTypeId == (int)ComponentType.BallastTractor && (movementId == 270103 || movementId == 270104
                            || movementId == 270105 || movementId == 270106 || movementId == 270107 || movementId == 270108 || movementId == 270109))
                        {
                            var itemToRemove1 = vehclCompObj.VehicleParamList.FirstOrDefault(r => r.ParamModel == "Outside Track");
                            vehclCompObj.VehicleParamList.Remove(itemToRemove1);
                            var itemToRemove2 = vehclCompObj.VehicleParamList.FirstOrDefault(r => r.ParamModel == "Wheelbase");
                            vehclCompObj.VehicleParamList.Remove(itemToRemove2);
                        }
                        
                        if (vehclCompObj != null)
                        {
                            if (VehicleComponentObj != null)
                                vehclCompObj.UpdateVehicleProperties(VehicleComponentObj);

                            if (listVehclRegObj != null && listVehclRegObj.Count != 0)
                            {
                                vehclCompObj.ListVehicleReg = new List<VehicleRegistration>();
                                vehclCompObj.ListVehicleReg = listVehclRegObj;
                            }
                            if (movementId == 270110 || movementId == 270111 || movementId == 270112 || movementId == 270156)
                                vehclCompObj.IsConfigTyreCentreSpacing = true;
                        }
                        bool tyreEmpty = false;
                        tyreEmpty = axleList.All(item => (item.TyreSize is null || item.TyreSize == "") && (item.TyreCenters is null || item.TyreCenters == ""));
                        ViewBag.tyreEmpty = tyreEmpty;
                        ViewBag.LatPos = latPos;
                        ViewBag.LongPos = longPos;
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/VehicleComponentDetails ActionResult method completed successfully"));
                        return PartialView("VehicleComponentDetails", vehclCompObj);
                    }
                }
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}],Vehicle/VehicleComponentDetails, Exception: {1}", Session.SessionID, ex));
                throw ex;
            }
        }
        #endregion public ActionResult VehicleComponentDetails(int vehicleSubTypeId, int vehicleTypeId, int movementId, int? componentId, bool isRoute=false)

        #region public ActionResult ViewComponentRegistration(int compId, bool isTractor, int? vehicleTypeId)
        public ActionResult ViewComponentRegistration(int compId, bool isTractor, int? vehicleTypeId, bool isApplication = false, bool isVR1 = false, bool isNotif = false, int isFromConfig = 0) //vehicleTypeId added as a parameter to remove registration Id for rigid vehicle component type.
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/ViewComponentRegistration ActionResult method started successfully, with parameters compId:{1}, isTractor:{2}, vehicleTypeId:{3}", Session.SessionID, compId, isTractor, vehicleTypeId));
                ViewBag.IsTractor = isTractor;
                ViewBag.VehicleTypeId = vehicleTypeId;
                ViewBag.MakeConfig = TempData["MakeConfig"];
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                List<VehicleRegistration> listVehclRegObj = null;
                if (isNotif || isVR1)
                {
                    listVehclRegObj = vehicleComponentService.GetVR1RegistrationDetails(compId, SessionInfo.UserSchema);
                }
                else if (!isApplication && !isVR1)
                {
                    //listVehclRegObj = vehicleComponentService.GetRegistrationDetails(compId);
                    listVehclRegObj = vehicleComponentService.GetRouteComponentRegistrationDetails(compId, SessionInfo.UserSchema);
                }
                else
                {
                    listVehclRegObj = vehicleComponentService.GetApplRegistrationDetails(compId, SessionInfo.UserSchema);
                }

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/ViewComponentRegistration ActionResult method completed successfully"));
                return PartialView("ViewComponentRegistration", listVehclRegObj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}],Vehicle/ViewComponentRegistration, Exception: {1}", Session.SessionID, ex));
                throw ex;
            }
        }
        #endregion public ActionResult ViewComponentRegistration(int compId, bool isTractor, int? vehicleTypeId)

        public ActionResult GetVehicleComponentImage(int vehicleSubTypeId, int vehicleTypeId, int movementId, int? componentId, string GUID = "", bool isComponent = false, bool isLastComponent = false, int vehicleConfigId = 0, bool isNotify = false, int isFromConfig = 0, bool movement = false, bool isCandidate = false)
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

                VehicleComponent vehclCompObj = null;
                ComponentModel VehicleComponentObj = null;
                if (componentId != null)
                {
                    int compId = (int)componentId;

                    if (isFromConfig == 1 && vehicleConfigId == 0)
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
                    movementId = VehicleComponentObj.VehicleIntent;

                }

                ViewBag.VehicleSubTypeId = vehicleSubTypeId;
                ViewBag.VehicleTypeId = vehicleTypeId;
                ViewBag.MovementID = movementId;

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

                vehclCompObj = GetVehicleComponent(vehicleSubTypeId, vehicleTypeId, movementId);

                ViewBag.CompImageName = vehclCompObj.vehicleComponentSubType.ImageName;


                if (VehicleComponentObj != null)
                {
                    vehclCompObj.UpdateVehicleProperties(VehicleComponentObj);
                }

                #region check last component in config
                if (Session["is_lastComponent"] != null)
                {
                    isLastComponent = (bool)Session["is_lastComponent"];
                }

                ViewBag.IsLastComp = isLastComponent;
                ViewBag.ImageName = vehclCompObj.vehicleComponentSubType.ImageName;

                #endregion

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/GeneralComponent ActionResult method completed successfully"));
                if (isFromConfig == 1)
                {
                    ViewBag.ComponentId = componentId;
                    for (int i = 0; i < vehclCompObj.VehicleParamList.Count; i++)
                    {
                        if (vehclCompObj.VehicleParamList[i].ParamModel == "Internal Name")
                        {
                            ViewBag.CompName = vehclCompObj.VehicleParamList[i].ParamValue;
                        }
                    }
                    return PartialView("GetVehicleComponentImage");
                }
                else
                {
                    return new EmptyResult();
                }

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}],Vehicle/GeneralComponent, Exception: {1}", Session.SessionID, ex.Message));
                throw ex;
            }
        }

        public ActionResult SingleComponentAxle(int? componentId, bool isRoute = false, bool IsNotif = false, bool isVR1 = false, bool movement = false, string mode = "",bool isConfigTyreCentreSpacing=false)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/SingleComponentAxle ActionResult method started successfully, with parameters componentId:{1}", Session.SessionID, componentId));
                
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }

                ViewBag.mode = mode;
                List<Axle> axleList = null;
                ViewBag.AxleList = null;
                if (componentId != null)
                {
                    int compId = (int)componentId;
                    
                    if (isRoute && !movement)
                    {
                        if (isVR1)
                        {
                            axleList = vehicleComponentService.ListVR1vehAxle(compId, SessionInfo.UserSchema);
                        }
                        else
                        {                                
                            axleList = vehicleComponentService.ListRouteComponentAxle(compId, SessionInfo.UserSchema);
                        }
                    }
                    else if (IsNotif && !movement)
                    {
                        axleList = vehicleComponentService.ListVR1vehAxle(compId, SessionInfo.UserSchema);

                    }
                    else
                    {

                        if (movement)
                        {
                            axleList = vehicleComponentService.ListAxleTemp(compId, movement, SessionInfo.UserSchema);
                        }
                        else
                        {
                            axleList = vehicleComponentService.ListAxle(compId);
                        }
                    }
                    ViewBag.AxleList = axleList;
                    ViewBag.IsConfigTyreCentreSpacing = isConfigTyreCentreSpacing;
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/SingleComponentAxle ActionResult method completed successfully"));
                    return PartialView("_SingleComponentAxle");
                    
                }
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}],Vehicle/SingleComponentAxle, Exception: {1}", Session.SessionID, ex));
                throw ex;
            }
        }


        #endregion

        #region Private Functions

        #region private VehicleComponent GetVehicleComponent(int vehicleSubTypeId, int vehicleTypeId, int movementId)
        /// <summary>
        /// Finds the VehicleComponent Object with validated values from cache
        /// </summary>
        /// <param name="vehicleSubTypeId"></param>
        /// <param name="vehicleTypeId"></param>
        /// <param name="movementId"></param>
        /// <returns>VehicleComponent</returns>
        private VehicleComponent GetVehicleComponent(int vehicleSubTypeId, int vehicleTypeId, int movementId)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/GetVehicleComponent method started successfully, with parameters vehicleSubTypeId:{0}, vehicleTypeId:{1}", vehicleSubTypeId, vehicleTypeId));
                ComponentConfiguration compConfigObj = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
                //MovementClassificationConfig moveClassConfigObj = compConfigObj.GetMovementClassificationConfig(movementId);
                //VehicleComponent vehclCompObj = moveClassConfigObj.GetVehicleComponent(vehicleTypeId, vehicleSubTypeId);
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
        #endregion private VehicleComponent GetVehicleComponent(int vehicleSubTypeId, int vehicleTypeId, int movementId)

        #region private static List<string> GetAllVehicleType()
        private static List<string> GetAllVehicleType()
        {
            List<string> listVhclType = new List<string>();
            listVhclType.Add("Ballast tractor");
            listVhclType.Add("Conventional tractor");
            listVhclType.Add("Drawbar trailer");
            listVhclType.Add("Rigid vehicle");
            listVhclType.Add("Semi trailer");
            listVhclType.Add("SPMT");
            listVhclType.Add("Tracked vehicle");
            listVhclType.Add("Mobile crane");
            listVhclType.Add("Engineering plant");
            listVhclType.Add("Engineering plant–drawbar trailer");
            listVhclType.Add("Engineering plant–semi trailer");
            listVhclType.Add("Recovery vehicle");
            listVhclType.Add("Girder set");

            return listVhclType;
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

        #region private void VehicleComponentDropDown()
        /// <summary>
        /// Method to fill movement list to the dropdown
        /// </summary>
        private void VehicleComponentDropDown(int userType)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("MovementClsDropDown method started successfully"));

                List<VehicleComponentType> dropDownList = new List<VehicleComponentType>();
                List<uint> componentId = new List<uint>();

                ComponentConfiguration vehicleParams = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
                dropDownList = vehicleParams.GetListOfVehicleComponent();
                
                if (Session["movementClassificationId"] != null && Session["movementClassificationId"] != "")
                {
                    componentId = vehicleComponentService.VehicleComponentType((int)Session["movementClassificationId"], UserSchema.Portal);

                    dropDownList = dropDownList.Where(x => componentId.Contains((uint)x.ComponentTypeId)).ToList();

                }
                else  if (userType == UserType.Sort)
                {
                    componentId = vehicleComponentService.VehicleComponentType(0,UserSchema.Sort);
                    dropDownList = dropDownList.Where(x => componentId.Contains((uint)x.ComponentTypeId)).ToList();
                }
                ViewBag.ComponentType = new SelectList(dropDownList, "ComponentTypeId", "ComponentName");

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("MovementClsDropDown method completed successfully"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region private ComponentModel ConvertToComponent(VehicleComponent vehicleComponent)
        /// <summary>
        /// Method to convert the list of IFX propery to componentModel objects
        /// </summary>
        /// <param name="vehicleComponent"></param>
        /// <returns></returns>
        private ComponentModel ConvertToComponent(VehicleComponent vehicleComponent)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Vehicle/ConvertToComponent  method started successfully with parameters VehicleComponent object"));
            try
            {
                int unit = 1;
                int unitWeight = 1;
                Domain.VehiclesAndFleets.Vehicles.VehicleEnumConversions vehicleEnumConversions = new Domain.VehiclesAndFleets.Vehicles.VehicleEnumConversions();
                ComponentModel componentObj = new ComponentModel();
                
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
                        case "Number of Axles":
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
                                // componentObj.CouplingType = Convert.ToInt32(ifxProperty.ParamValue);
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
                                componentObj.SpacingToFollowing = Convert.ToDouble(ifxProperty.ParamValue);
                            componentObj.SpacingToFollowingUnit = unit;
                            break;
                        case "Rear_Steer":
                            if (!string.IsNullOrEmpty(ifxProperty.ParamValue))
                                componentObj.IsSteerable = Convert.ToInt32(ifxProperty.ParamValue);
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
        #endregion

        #region private int ConvertBooleanToInt(string value)
        private int ConvertBooleanToInt(string value)
        {
            if (value == "true")
                return 1;
            else
                return 0;
        }
        #endregion

        #region private VehicleConfiguration CastComponentToVehicle(VehicleComponent vehicleComponent)
        private VehicleConfiguration CastComponentToVehicle(VehicleComponent vehicleComponent)
        {
            if (vehicleComponent.TravellingSpeed != null)
            {
                List<string> dropString = new List<string>();
                IFXProperty objIfxProperty = new IFXProperty()
                {
                    ParamModel = "Speed",
                    ParamType = "double",
                    ParamValue = vehicleComponent.TravellingSpeed.ToString(),
                    DropDownList = dropString
                };
                vehicleComponent.VehicleParamList.Add(objIfxProperty);
            }
            VehicleConfiguration vehicleConfigObj = new VehicleConfiguration()
            {
                VehicleConfigParamList = vehicleComponent.VehicleParamList
            };
            return vehicleConfigObj;
        }
        #endregion

        #region private NewConfigurationModel ConvertToConfiguration(VehicleConfiguration vehicleConfigObj)
        /// <summary>
        /// Method to convert the list of IFX propery to componentModel objects
        /// </summary>
        /// <param name="vehicleConfigObj"></param>
        /// <returns></returns>
        private NewConfigurationModel ConvertToConfiguration(VehicleConfiguration vehicleConfigObj)
        {
            int unit = 1;
            NewConfigurationModel configModelObj = new NewConfigurationModel();
            foreach (var ifxProperty in vehicleConfigObj.VehicleConfigParamList)
            {
                switch (ifxProperty.ParamModel)
                {
                    case "Formal_Name":
                        configModelObj.VehicleIntDesc = ifxProperty.ParamValue;
                        break;
                    case "Internal_Name":
                        configModelObj.VehicleName = ifxProperty.ParamValue;
                        break;
                    //case "intendUse":
                    //    componentObj.VehicleIntent = Convert.ToInt32(ifxProperty.ParamValue);
                    //    break;
                    case "Notes":
                        configModelObj.VehicleDesc = ifxProperty.ParamValue;
                        break;
                    case "Length":
                        configModelObj.RigidLength = Convert.ToDouble(ifxProperty.ParamValue);
                        configModelObj.RigidLengthUnit = unit;
                        break;
                    case "Weight":
                        configModelObj.GrossWeight = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "OverallLength":
                        configModelObj.Length = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "Width":
                        configModelObj.Width = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "Wheelbase":
                        configModelObj.WheelBase = Convert.ToDouble(ifxProperty.ParamValue);
                        configModelObj.WheelBaseUnit = unit;
                        break;
                    case "Maximum Height":
                        configModelObj.MaxHeight = Convert.ToDouble(ifxProperty.ParamValue);
                        configModelObj.MaxHeightUnit = unit;
                        break;
                    case "AxleWeight":
                        configModelObj.MaxAxleWeight = Convert.ToDouble(ifxProperty.ParamValue);
                        configModelObj.MaxAxleWeightUnit = unit;
                        break;
                    case "Speed":
                        configModelObj.Speed = Convert.ToDouble(ifxProperty.ParamValue);
                        break;
                    case "Tyre_Spacing":
                        configModelObj.TyreSpacing = Convert.ToDouble(ifxProperty.ParamValue);
                        configModelObj.TyreSpacingUnit = unit;
                        break;

                    default:
                        break;
                }
            }
            return configModelObj;
        }
        #endregion private NewConfigurationModel ConvertToConfiguration(VehicleConfiguration vehicleConfigObj)

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
        private void StartFleetManagementWorkflow(string organizationName, bool isImportComponent)
        {
            var vehicleName = new SessionData().Wf_Fm_VehicleConfigurationId ?? string.Empty;
            if (vehicleName.Length == 0 && new SessionData().Wf_Fm_CurrentExecuted == WorkflowActivityTypes.Gn_NotDecided)
            {
                FleetManagement fleetManagement = new FleetManagement(fleetManagementWorkflowService);
                new SessionData().Wf_Fm_FleetManagementId = fleetManagement.StartWorkflow(isImportComponent, organizationName, vehicleName);
            }
            if (isImportComponent)
            {
                ProcessWorkflowActivity(null);
            }
        }

        private void ProcessWorkflowActivity(List<VehicleComponentsModel> vehicleComponentsList)
        {
            var fleetPayload = (VehicleWorkFlowParams)Session["vehicleWorkFlowParams"];
            FleetManagement fleetManagement = new FleetManagement(fleetManagementWorkflowService);
            fleetManagement.ProcessWorkflowActivity(fleetPayload, vehicleComponentsList, WorkflowFleetMgmtFlowTypes.Vehicle, Session.SessionID, true, false, false, false);
        }
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

        #region Get Movement classification and component parameters

        public ActionResult VehicleComponentMovementClassification(int componentTypeId, int componentSubTypeId)
        {
            List<uint> movementClassificationResult = new List<uint>();
            List<MovementClassification> mvmntList = new List<MovementClassification>();
            try
            {
                movementClassificationResult = vehicleComponentService.VehicleComponentMovementClassification(componentTypeId, componentSubTypeId);
                List<MovementClassification> mvmntdrpdwn = null;
                ComponentConfiguration vehicleParams = (ComponentConfiguration)HttpContext.Application["VehicleComponents"];
                mvmntdrpdwn = vehicleParams.GetMovementClassification();

                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                if (SessionInfo.UserTypeId == UserType.Sort)
                {
                    mvmntList = mvmntdrpdwn.Where(x => movementClassificationResult.Contains(270002) || movementClassificationResult.Contains(270006)).ToList();
                    if (mvmntList.Count > 0)
                    {
                        mvmntList = mvmntdrpdwn.Where(a => a.ClassificationId == 270002 || a.ClassificationId == 270006).ToList();
                    }
                }
                else
                    mvmntList = mvmntdrpdwn.Where(x => movementClassificationResult.Contains((uint)x.ClassificationId)).ToList();

                if (Session["movementClassificationId"] != null && Session["movementClassificationId"] != "")
                {
                    mvmntList = new List<MovementClassification>();
                    MovementClassification obj = new MovementClassification()
                    {
                        ClassificationId = (int)Session["movementClassificationId"],
                        ClassificationName = (string)Session["movementClassificationName"]
                    };
                    mvmntList.Add(obj);
                }

                //var MovementClassConfig = new SelectList(mvmntList, "ClassificationId", "ClassificationName");

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/VehicleComponentMovementClassification, Exception: {ex}");
            }
            return Json(new { movementClassificationResult = mvmntList });
        }

        public VehicleComponentConfiguration VehicleComponentValidation(int componentTypeId, int componentSubTypeId, int movementClassificationId)
        {
            VehicleComponentConfiguration vehicleComponent = new VehicleComponentConfiguration();
            try
            {
                vehicleComponent = vehicleComponentService.VehicleComponentValidation(componentTypeId, componentSubTypeId, movementClassificationId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/VehicleComponentConfiguration, Exception: {ex}");
            }
            return vehicleComponent;
        }

        #endregion
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
        #endregion

        #region-----Amend Axle For Semi Trailer Conventional Tractor------
        [HttpPost]
        public JsonResult UpdateConventionalTractorAxleCount(int axleCount,int vehicleId,int fromComponentId, int toComponentId)
        {
            try
            {
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                int res=vehicleComponentService.UpdateConventionalTractorAxleCount(axleCount, vehicleId, fromComponentId, toComponentId, SessionInfo.UserSchema);
                if (res > 0)
                    return Json(new { success = true });
                else
                    return Json(new { success = false, error = "Not updated." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false,error=ex.Message.ToString() });
            }
        }
        #endregion
        private VehicleComponent RemoveProjectionFields(VehicleComponent vehclCompObj, bool isLastComponent = false, int compNumber = 0)
        {
            if (compNumber == 1 && !isLastComponent)
            {
                var itemToRemove1 = vehclCompObj.VehicleParamList.FirstOrDefault(r => r.ParamModel == "Rear Overhang");
                if (itemToRemove1 != null)
                    vehclCompObj.VehicleParamList.Remove(itemToRemove1);
            }
            else if (compNumber > 1 && !isLastComponent)
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

    }
}