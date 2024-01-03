using System;
using System.Collections.Generic;
using System.Web.Mvc;
using STP.Domain.Applications;
using STP.Domain.SecurityAndUsers;
using STP.ServiceAccess.Applications;
using STP.ServiceAccess.Routes;
using System.IO;
using STP.Web.General;
using STP.ServiceAccess.VehiclesAndFleets;
using STP.Common.Logger;
using STP.Domain.LoggingAndReporting;
using STP.ServiceAccess.LoggingAndReporting;
using STP.Common.Constants;
using STP.Web.WorkflowProvider;
using STP.Domain.Routes;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.VehicleAndFleets.Component;
using STP.ServiceAccess.MovementsAndNotifications;
using System.Globalization;
using System.Xml;
using System.Text;
using STP.Common.Enums;
using STP.ServiceAccess.Workflows.ApplicationsNotifications;
using System.Dynamic;
using STP.Domain.Workflow;
using STP.Common.SortHelper;
using STP.Domain.RouteAssessment;
using STP.Domain.Structures;
using STP.Domain.MovementsAndNotifications.Notification;
using static STP.Domain.VehiclesAndFleets.VehicleEnums;
using STP.ServiceAccess.SecurityAndUsers;
using UserType = STP.Common.Constants.UserType;
using STP.Domain.MovementsAndNotifications.Movements;
using System.Linq;
using STP.ServiceAccess.RouteAssessment;
using STP.Common.General;
using STP.Domain.Custom;
using STP.ServiceAccess.Workflows.SORTSOProcessing;

namespace STP.Web.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly IApplicationService applicationService;
        private readonly IVehicleConfigService vehicleConfigService;
        private readonly ILoggingService loggingService;
        private readonly IRoutesService routesService;
        private readonly ISORTApplicationService sortApplicationService;
        private readonly INotificationService notificationService;
        private readonly IMovementsService movementService;
        private readonly IApplicationNotificationWorkflowService applicationNotificationWorkflowService;
        private readonly IUserService userService;
        private readonly IOrganizationService organizationService;
        private readonly IRouteAssessmentService routeAssessmentService;
        private readonly ISORTSOProcessingService sortSOProcessingService;
        private readonly IRoutesService routeService;
        public ApplicationController(IApplicationService applicationService, IVehicleConfigService vehicleConfigService, ILoggingService loggingService, IRoutesService routesService, ISORTApplicationService sortApplicationService, INotificationService notificationService, IMovementsService movementService, IApplicationNotificationWorkflowService applicationNotificationWorkflowService, IUserService userService, IOrganizationService organizationService, IRouteAssessmentService routeAssessmentService, ISORTSOProcessingService sortSOProcessingService)
        {
            this.applicationService = applicationService;
            this.vehicleConfigService = vehicleConfigService;
            this.loggingService = loggingService;
            this.routesService = routesService;
            this.sortApplicationService = sortApplicationService;
            this.notificationService = notificationService;
            this.movementService = movementService;
            this.applicationNotificationWorkflowService = applicationNotificationWorkflowService;
            this.userService = userService;
            this.organizationService = organizationService;
            this.routeAssessmentService = routeAssessmentService;
            this.sortSOProcessingService = sortSOProcessingService;
        }

        #region  public ActionResult ViewContactDetails(decimal ContactId)
        /// <summary>
        /// actionresult to display SoContactDetails
        /// </summary>
        /// <param name="ContactId"></param>
        /// <returns></returns>
        public ActionResult ViewContactDetails(decimal ContactId = 0, string EsdalRefNo = "", string structOwner = "", string isClosed = "false", int ownerCnt = 0, bool showPopup = true)
        {
            if (ContactId != 0)
            {
                if (ownerCnt != 0)
                    ViewBag.OwnerCnt = ownerCnt;
                ViewBag.structOwner = structOwner;
                var haContactDetailObj = applicationService.GetHAContactDetails(ContactId);
                ViewBag.ShowXML = 0;
                if (showPopup)
                    return PartialView("ViewContactDetails", haContactDetailObj);
                else
                    return PartialView("PartialView/_ViewContactDetails", haContactDetailObj);
            }
            else
            {
                HAContact HAContactDet = new HAContact();
                string result = string.Empty;
                string errormsg = "No data found";
                string path = null;

                HAContactDet = applicationService.GetHAContDetFromInboundDoc(EsdalRefNo);
                if (showPopup)
                {
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\HAContactDetails.xslt");
                }
                else
                {
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\HAViewContactDetails.xslt");
                }
                result = STP.Common.General.XsltTransformer.Trafo(HAContactDet.ContactDetails, path, out errormsg);
                ViewBag.ContactDetails = result;
                ViewBag.ShowXML = 1;
                if (showPopup)
                    return PartialView("ViewContactDetails");
                else
                    return PartialView("PartialView/_ViewContactDetails");
            }
        }
        #endregion

        #region ViewVehicleDetails
        public ActionResult ViewVehicleDetails(string ESDALRef)
        {

            List<VehicleDetail> objVehicleDetail = vehicleConfigService.GetVehicleConfigByPartID(ESDALRef, 1);

            VehicleDetail objectVehicleDetail = new VehicleDetail();
            ApplicationGroupingFunction ApplicationGroupingFunctionObj = new ApplicationGroupingFunction();

            string savename = string.Empty, GrossWeight = string.Empty, MaxAcelWeight = string.Empty, Registration = string.Empty;

            objectVehicleDetail = ApplicationGroupingFunctionObj.ViewSummary(objVehicleDetail, out savename, out GrossWeight, out MaxAcelWeight, out Registration);
            ViewBag.savename = savename;

            ViewBag.GrossWeight = GrossWeight;
            ViewBag.MaxAcelWeight = MaxAcelWeight;

            if (Registration != "" && Registration != string.Empty)
            {
                ViewBag.Registration = Registration.Substring(0);
            }
            else
            {
                ViewBag.Registration = string.Empty;
            }

            return View(objectVehicleDetail);
        }
        #endregion

        #region  ActionResult ApplicationComponentList(int RoutePartId)
        /// <summary>
        /// public ActionResult ApplicationComponentList(int RoutePartId)
        /// </summary>
        /// <param name="RoutePartId"></param>
        /// <returns></returns>
        public ActionResult ApplicationComponentList(int RoutePartID)
        {
            //displays list of component          
            List<ComponentGroupingModel> componentObjList = new List<ComponentGroupingModel>();
            componentObjList = vehicleConfigService.ApplicationcomponentList(RoutePartID);
            List<ComponentObjListModelToreturn> componentObjListModelToreturn = new List<ComponentObjListModelToreturn>();
            ApplicationGroupingFunction ApplicationGroupingFunctionObj = new ApplicationGroupingFunction();
            componentObjListModelToreturn = ApplicationGroupingFunctionObj.Grouping(componentObjList);
            ViewBag.RoutePartID = RoutePartID;
            return PartialView(componentObjListModelToreturn);
        }

        #endregion

        #region ApplicationSupplimentaryInfo
        public ActionResult ApplicationSupplimentaryInfo(int appRevisionId = 0, string workflowProcess = "")
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Application/VR1EditSupplementaryDetails actionResult method started successfully"));
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                SupplimentaryInfo supplimentaryInfo;
                supplimentaryInfo = applicationService.VR1GetSupplementaryInfo(appRevisionId, SessionInfo.UserSchema, 0);
                var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                if (applicationNotificationManagement.IsThisMovementExist(0, appRevisionId, out string workflowKey)
                    && WorkflowTaskFinder.FindNextTask("HaulierApplication", WorkflowActivityTypes.An_Activity_SupplementaryDetails, out dynamic workflowPayload) != string.Empty)
                {
                    dynamic dataPayload = new ExpandoObject();
                    dataPayload.taskId = 5;
                    PlanMvmntPayLoad planMvmntPayLoad = applicationNotificationManagement.GetPlanMvmtPayload();
                    planMvmntPayLoad.ActionCompleted = planMvmntPayLoad.ActionCompleted <= dataPayload.taskId ? dataPayload.taskId : planMvmntPayLoad.ActionCompleted;
                    dataPayload.PlanMvmntPayLoad = planMvmntPayLoad;
                    dataPayload.supplimentaryInfo = supplimentaryInfo;
                    dataPayload.workflowActivityLog = applicationNotificationManagement.SetWorkflowLog(WorkflowActivityTypes.An_Activity_SupplementaryDetails.ToString());
                    WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                    {
                        data = dataPayload,
                        workflowData = workflowPayload
                    };
                    var currentActivity = applicationNotificationWorkflowService.GetCurrentTask(new SessionData().Wf_An_ApplicationWorkflowId);
                    if (currentActivity != "Activity_PlanRouteOnMap")
                    {
                        applicationNotificationManagement.ProcessWorkflowActivity(workflowProcess, workflowActivityPostModel);
                    }
                    applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);
                }
                return PartialView("PartialView/_ApplicationSupplimentaryInfo", supplimentaryInfo);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/VR1EditSupplementaryDetails, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region VR1SupplementaryInfo
        public ActionResult SaveSupplimentaryInfo(SupplimentaryInfo supplimentaryInfo, int appRevisionId = 0)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Application/VR1SupplementaryInfo actionResult method started successfully"));
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                bool result = applicationService.SOVR1SupplementaryInfo(supplimentaryInfo, SessionInfo.UserSchema, appRevisionId);
                var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                if (applicationNotificationManagement.IsThisMovementExist(0, appRevisionId, out string workflowKey)
                    && WorkflowTaskFinder.FindNextTask("HaulierApplication", WorkflowActivityTypes.An_Activity_SupplementaryDetails
                    , out dynamic workflowPayload, taskId: 0) != string.Empty)
                {
                    dynamic dataPayload = new ExpandoObject();
                    dataPayload.supplimentaryInfo = supplimentaryInfo;
                    PlanMvmntPayLoad planMvmntPayLoad = applicationNotificationManagement.GetPlanMvmtPayload();
                    planMvmntPayLoad.IsSupplimentarySaved = true;
                    dataPayload.taskId = 5;
                    planMvmntPayLoad.ActionCompleted = planMvmntPayLoad.ActionCompleted <= dataPayload.taskId ? dataPayload.taskId : planMvmntPayLoad.ActionCompleted;
                    dataPayload.PlanMvmntPayLoad = planMvmntPayLoad;
                    dataPayload.workflowActivityLog = applicationNotificationManagement.SetWorkflowLog(WorkflowActivityTypes.An_Activity_SupplementaryDetails.ToString());
                    WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                    {
                        data = dataPayload,
                        workflowData = workflowPayload
                    };
                    applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);

                }
                return Json(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/VR1SupplementaryInfo, Exception: {0}", ex));
                return Json("0");
            }
        }
        #endregion

        #region ApplicationOverview
        public ActionResult ApplicationOverview(int appRevisionId = 0, long versionId = 0, string hauliermnemonic = null, int esdalref = 0, int revisionno = 0, int versionno = 0, bool btnedit = false, string workflowProcess = "",string isVr1 = "")
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Application/ApplicationOverview actionResult method started successfully"));
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                SOApplication soapplication = new SOApplication();
                SessionInfo = (UserInfo)Session["UserInfo"];
                int organisationId = 0;
                if (SessionInfo.UserTypeId == UserType.Sort)
                    organisationId = (int)Session["SORTOrgID"];
                else
                    organisationId = (int)SessionInfo.OrganisationId;

                soapplication.ApplicationRevId = appRevisionId;
                soapplication.VersionId = versionId;
                soapplication = applicationService.GetSOGeneralWorkinProcessbyrevisionid(SessionInfo.UserSchema, appRevisionId, 0, organisationId);
                if (soapplication.ApplicationDate != "" && soapplication.ApplicationDate != null)
                {
                    DateTime appDate = DateTime.ParseExact(soapplication.ApplicationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    soapplication.ApplicationDate = appDate.ToString("dd-MMM-yyyy");
                }
                if (soapplication.ApplicationDueDate != "" && soapplication.ApplicationDueDate != null)
                {
                    DateTime appDueDate = DateTime.ParseExact(soapplication.ApplicationDueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    soapplication.ApplicationDueDate = appDueDate.ToString("dd-MMM-yyyy");
                }
                else
                {
                    soapplication.ApplicationDueDate = DateTime.Now.ToString("dd-MMM-yyyy");
                }
                if (soapplication.MovementDateFrom != "" && soapplication.MovementDateFrom != null)
                {
                    DateTime fromDate = DateTime.ParseExact(soapplication.MovementDateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(soApplication.MovementDateFrom);
                    soapplication.MovementDateFrom = fromDate.ToString("dd-MMM-yyyy");
                }
                if (soapplication.MovementDateTo != "" && soapplication.MovementDateTo != null)
                {
                    DateTime toDate = DateTime.ParseExact(soapplication.MovementDateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(soApplication.MovementDateTo);
                    soapplication.MovementDateTo = toDate.ToString("dd-MMM-yyyy");
                }
                soapplication.VersionId = versionId;

                ViewBag.ProjectID = soapplication.ProjectId;


                if (workflowProcess.Length > 0)
                {
                    var activityToChange = WorkflowTaskFinder.FindNextTask(workflowProcess, WorkflowActivityTypes.An_Activity_SubmitApplication, out dynamic workflowPayload);
                    if (activityToChange != string.Empty)
                    {
                        var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                        dynamic dataPayload = new ExpandoObject();
                        dataPayload.soapplication = soapplication;
                        dataPayload.taskId = 6;
                        PlanMvmntPayLoad planMvmntPayLoad = applicationNotificationManagement.GetPlanMvmtPayload();
                        planMvmntPayLoad.ActionCompleted = planMvmntPayLoad.ActionCompleted <= dataPayload.taskId ? dataPayload.taskId : planMvmntPayLoad.ActionCompleted;
                        dataPayload.PlanMvmntPayLoad = planMvmntPayLoad; 
                        dataPayload.workflowActivityLog = applicationNotificationManagement.SetWorkflowLog(WorkflowActivityTypes.An_Activity_SubmitApplication.ToString());
                        WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                        {
                            data = dataPayload,
                            workflowData = workflowPayload
                        };
                        applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);
                    }
                }
                ViewBag.IsVr1 = !string.IsNullOrEmpty(isVr1) && isVr1.ToLower()=="true"? 1:0;
                soapplication.CountryList = userService.GetCountryInfo();
                return PartialView("PartialView/_ApplicationOverview", soapplication);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/CreateGeneralApplication, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }

        }
        #endregion

        #region SaveAppGeneral
        [HttpPost]
        public ActionResult SaveAppGeneral(SOApplication soapplication, long ApplicationrevId = 0, string workflowProcess = "")
        {
            bool isSuccess = false;
            try
            {
                var _fromDate = DateTime.Parse(soapplication.MovementDateFrom);
                var _toDate = DateTime.Parse(soapplication.MovementDateTo);
                soapplication.MovementDateFrom = _fromDate.ToString("dd-MMM-yyyy");
                soapplication.MovementDateTo = _toDate.ToString("dd-MMM-yyyy");
                if (!string.IsNullOrEmpty(soapplication.ApplicationDate))
                {
                    var _applicationDate = DateTime.Parse(soapplication.ApplicationDate);
                    soapplication.ApplicationDate = _applicationDate.ToString("dd-MMM-yyyy");
                }
                if (!string.IsNullOrEmpty(soapplication.ApplicationDueDate))
                {
                    var _applicationDueDate = DateTime.Parse(soapplication.ApplicationDueDate);
                    soapplication.ApplicationDueDate = _applicationDueDate.ToString("dd-MMM-yyyy");
                }
                UserInfo SessionInfo = null;
                long result = 0;
                if (ModelState.IsValid)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"Application/SaveAppGeneral actionResult method started successfully");

                    if (Session["UserInfo"] == null)
                    {
                        return Json(result);
                    }
                    SessionInfo = (UserInfo)Session["UserInfo"];
                    var sessionValues = (UserInfo)Session["UserInfo"];
                    int organisationId = (int)SessionInfo.OrganisationId;
                    if (sessionValues.UserSchema == UserSchema.Sort)
                        organisationId = (int)soapplication.OrgId;
                    int userId = Convert.ToInt32(sessionValues.UserId);
                    string ErrMsg = string.Empty;
                    string sysEventDescp = string.Empty;
                    MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                    movactiontype.UserName = SessionInfo.UserName;
                    if (soapplication.ApplicationRevId != 0)
                    {
                        isSuccess = applicationService.SaveAppGeneral(soapplication, organisationId, userId, ApplicationrevId, sessionValues.UserSchema);
                        var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                        if (applicationNotificationManagement.IsThisMovementExist(0, ApplicationrevId, out string workflowKey)
                            && workflowProcess.Length > 0
                            && isSuccess
                            && WorkflowTaskFinder.FindNextTask(workflowProcess, WorkflowActivityTypes.An_Activity_SubmitApplication, out dynamic workflowPayload, taskId: 0) != string.Empty)
                        {
                            dynamic dataPayload = new ExpandoObject();
                            dataPayload.soapplication = soapplication;
                            dataPayload.taskId = 6.1;
                            PlanMvmntPayLoad planMvmntPayLoad = applicationNotificationManagement.GetPlanMvmtPayload();
                            planMvmntPayLoad.IsSoOverView = true;
                            planMvmntPayLoad.IsNotifGeneralSaved = false;
                            planMvmntPayLoad.ActionCompleted = planMvmntPayLoad.ActionCompleted <= dataPayload.taskId ? dataPayload.taskId : planMvmntPayLoad.ActionCompleted;
                            dataPayload.PlanMvmntPayLoad = planMvmntPayLoad;
                            dataPayload.displayOverView = true;
                            dataPayload.workflowActivityLog = applicationNotificationManagement.SetWorkflowLog(WorkflowActivityTypes.An_Activity_SubmitApplication.ToString());
                            WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                            {
                                data = dataPayload,
                                workflowData = workflowPayload
                            };
                            applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);
                        }
                        #region System Event Log - haulier_edited_so_application
                        movactiontype.RevisionId = (int)soapplication.ApplicationRevId;
                        if (sessionValues.UserSchema == "UserSchema.Portal")
                        {
                            movactiontype.SystemEventType = SysEventType.Haulier_saved_so_application;
                        }
                        else if (sessionValues.UserSchema == "UserSchema.Sort")
                        {
                            movactiontype.SystemEventType = SysEventType.sort_saved_so_application;
                            movactiontype.OrganisatioName = soapplication.OrgName;
                        }
                        sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                        loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, userId, SessionInfo.UserSchema);
                        #endregion

                        return Json(new { result = isSuccess });
                    }
                }
                return Json(new { result = isSuccess });
            }
            catch (Exception ex)
            {
                return Json(new { result = isSuccess });
            }
        }
        #endregion

        #region Private Methods
        private List<ProjectFolderModel> GetProjectFolderModel(int OrgId)
        {
            List<ProjectFolderModel> projectFolderModel = applicationService.GetProjectFolderList(OrgId);
            return projectFolderModel;

        }
        #endregion

        #region SubmitSoApplication()

        public JsonResult SubmitSoApplication(int apprevisionId, string workflowProcess = "")
        {
            try
            {
                UserInfo UserSessionValue = null; //--------object is used for stroing movement actions
                UserSessionValue = (UserInfo)Session["UserInfo"];
                int userId = Convert.ToInt32(UserSessionValue.UserId);
                //submit so application
                SOApplication soGeneralDetails = applicationService.SubmitSoApplication(apprevisionId, userId);
                string sysEventDescp = string.Empty;

                if (soGeneralDetails.ApplicationStatus == 1 && soGeneralDetails.ESDALReference != null)
                {
                    #region movement actions for this action method

                    MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                    movactiontype.MovementActionType = MovementnActionType.haulier_submit_appl;
                    movactiontype.ESDALRef = soGeneralDetails.ESDALReference;
                    string ErrMsg = string.Empty;
                    int versionNo = 0;
                    string MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                    long res = loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription,soGeneralDetails.ProjectId, soGeneralDetails.LastRevisionNo, soGeneralDetails.VersionNo, UserSessionValue.UserSchema);
                    #endregion

                    #region System Event Log - haulier_submitted_application
                    movactiontype.RevisionId = apprevisionId;
                    movactiontype.ESDALRef = soGeneralDetails.ESDALReference;
                    movactiontype.UserName = UserSessionValue.UserName;
                    movactiontype.SystemEventType = SysEventType.Haulier_submitted_so_application;
                    sysEventDescp = System_Events.GetSysEventString(UserSessionValue, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(UserSessionValue.UserId), UserSessionValue.UserSchema);
                    #endregion
                }
                else
                    return Json(new { Success = false });
                var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                var payload = applicationNotificationManagement.GetPlanMvmtPayload();
                vehicleConfigService.DeleteTempMovementVehicle(payload.VehicleMoveId, UserSessionValue.UserSchema);
                if (applicationNotificationManagement.IsThisMovementExist(0, apprevisionId, out string workflowKey))
                {
                    var currentActivity = applicationNotificationManagement.GetCurrentActivity();
                    if (currentActivity != "Activity_SubmitApplication" && workflowProcess.Length > 0 && WorkflowTaskFinder.FindNextTask(workflowProcess, WorkflowActivityTypes.An_Activity_SubmitApplication, out dynamic workflowPayloadActivity) != string.Empty)
                    {

                        dynamic dataPayload = new ExpandoObject();
                        dataPayload.soGeneralDetails = soGeneralDetails;
                        dataPayload.workflowActivityLog = applicationNotificationManagement.SetWorkflowLog(WorkflowActivityTypes.An_Activity_SubmitApplication.ToString());
                        WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                        {
                            data = dataPayload,
                            workflowData = workflowPayloadActivity
                        };

                        applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);


                    }
                    if (workflowProcess.Length > 0 && WorkflowTaskFinder.FindNextTask(workflowProcess, WorkflowActivityTypes.TheEnd, out dynamic workflowPayload) != string.Empty)
                    {

                        dynamic dataPayload = new ExpandoObject();
                        WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                        {
                            data = dataPayload,
                            workflowData = workflowPayload
                        };

                        applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);


                    }
                }

                string str = soGeneralDetails.ESDALReference.ToString();
                return Json(new { Success = str });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/SubmitSoApplication, Exception: {0}", ex));
                return Json(0);
            }
        }
        #endregion

        #region SubmitVR1Application()

        public JsonResult SubmitVR1Application(int apprevisionId, bool ReducedDet = false, string workflowProcess = "")
        {
            try
            {
                int rd = 0;
                int versionNo = 0;
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                string ErrMsg = string.Empty;
                string sysEventDescp = string.Empty;
                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                if (ReducedDet) { rd = 1; }
                ApplyForVR1 applyForVR1 = applicationService.SubmitVR1Application(apprevisionId, rd);
                if (applyForVR1.Status == 1 && applyForVR1.ESDALReference != null)
                {
                    #region movement actions for this action method
                    UserInfo UserSessionValue = null; //--------object is used for stroing movement actions
                    UserSessionValue = (UserInfo)Session["UserInfo"];
                    movactiontype.MovementActionType = MovementnActionType.haulier_submit_appl;
                    movactiontype.ESDALRef = applyForVR1.ESDALReference;
                    string MovementDescription = STP.Domain.LoggingAndReporting.MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                    long res = loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, applyForVR1.ProjectId, applyForVR1.RevisionNumber, applyForVR1.VersionNumber, UserSessionValue.UserSchema);
                    #endregion

                    #region movement actions for this action method
                    movactiontype.SystemEventType = SysEventType.Haulier_submitted_vr1_application;
                    movactiontype.ESDALRef = applyForVR1.ESDALReference;
                    sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                    #endregion
                }
                else
                    return Json(new { Success = false });

                if (workflowProcess.Length > 0 && WorkflowTaskFinder.FindNextTask(workflowProcess, WorkflowActivityTypes.An_Activity_SubmitApplication, out dynamic workflowPayload) != string.Empty)
                {
                    var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                    var payload = applicationNotificationManagement.GetPlanMvmtPayload();
                    vehicleConfigService.DeleteTempMovementVehicle(payload.VehicleMoveId, SessionInfo.UserSchema);
                    dynamic dataPayload = new ExpandoObject();
                    dataPayload.workflowActivityLog = applicationNotificationManagement.SetWorkflowLog(WorkflowActivityTypes.An_Activity_SubmitApplication.ToString());
                    WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                    {
                        data = dataPayload,
                        workflowData = workflowPayload
                    };

                    applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel, true);
                    //applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);

                }
                string str = applyForVR1.ESDALReference.ToString();
                return Json(new { Success = str });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/DeleteSelectedSOAppRoute, Exception: {0}", ex));
                return Json(0);
            }
        }

        #endregion

        #region SubmitSORTVR1Application()
        public JsonResult SubmitSORTVR1Application(int apprevisionId)
        {
            try
            {
                string messg = "SORTApplications/SubmitSORTVR1Application?apprevisionId=" + apprevisionId;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                SubmitSORTVR1Params submitSORTParams = new SubmitSORTVR1Params()
                {
                    ApplicationRevisionID = apprevisionId
                };
                ApplyForVR1 result = sortApplicationService.SubmitSORTVR1Application(submitSORTParams);
                string res = result.HaulierMnemonic + "/" + result.EsdalRefNo;
                string ErrorMsg = string.Empty;
                #region Saving submission of sort VR1 application sys_event
                if (result != null)
                {

                    #region movement actions for this action method
                    UserInfo UserSessionValue = null; //--------object is used for stroing movement actions
                    UserSessionValue = (UserInfo)Session["UserInfo"];
                    movactiontype.MovementActionType = MovementnActionType.sort_submit_VR1_appl;
                    movactiontype.ESDALRef = result.ESDALReference;
                    string MovementDescription = STP.Domain.LoggingAndReporting.MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrorMsg);
                    long reslt = loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, result.ProjectId, result.RevisionNumber, result.VersionNumber, UserSessionValue.UserSchema);
                    #endregion

                    movactiontype.ContactName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                    movactiontype.ESDALRef = result.ESDALReference;
                    movactiontype.RevisionId = apprevisionId;
                    movactiontype.UserName = SessionInfo.UserName;
                    string ErrMsg = string.Empty;
                    movactiontype.SystemEventType = SysEventType.Sort_submitted_vr1_application;
                    string sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                }
                #endregion

                return Json(new { Success = res });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/SubmitSORTVR1Application, Exception: {0}", ex));
                return Json(0);
            }
        }
        #endregion

        #region SubmitSORTSoApplication()
        public JsonResult SubmitSORTSoApplication(int apprevisionId, int EditFlag = 0, string workflowProcess = "")
        {
            try
            {
                string messg = "SORTApplications/SubmitSORTSoApplication?apprevisionId=" + apprevisionId + "EditFlag=" + EditFlag;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                int user_ID = Convert.ToInt32(SessionInfo.UserId);
                SORTSOApplicationParams sortSOApplicationParams = new SORTSOApplicationParams()
                {
                    ApplicationRevisionId = apprevisionId,
                    EditFlag = EditFlag
                };
                SOApplication result = sortApplicationService.SubmitSORTSoApplication(sortSOApplicationParams);
                if (result != null)
                {
                    
                    movactiontype.ContactName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                    movactiontype.ESDALRef = result.ESDALReference;
                    string MovementDescription = "";
                    string ErrMsg = string.Empty;
                    #region movement actions for this action method
                    movactiontype.MovementActionType = MovementnActionType.sort_desktop_submits_appl;
                    MovementDescription = MovementActions.GetMovementActionString(SessionInfo, movactiontype, out ErrMsg);
                    loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, result.ProjectId, result.LastRevisionNo, result.VersionNo, SessionInfo.UserSchema);

                    #endregion
                    #region Saving submission of sort application event
                    movactiontype.SystemEventType = SysEventType.Sort_submitted_so_application;
                    movactiontype.RevisionId = apprevisionId;
                    movactiontype.EditFlag = EditFlag;
                    movactiontype.UserName = SessionInfo.UserName;
                    string sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, SessionInfo.UserSchema);
                    #endregion

                    string res = result.HaulierMneu + "/" + result.EsdalRefNo;

                    if (workflowProcess.Length > 0 && WorkflowTaskFinder.FindNextTask(workflowProcess, WorkflowActivityTypes.An_Activity_SubmitApplication, out dynamic workflowPayload) != string.Empty)
                    {
                        var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                        var payload = applicationNotificationManagement.GetPlanMvmtPayload();
                        vehicleConfigService.DeleteTempMovementVehicle(payload.VehicleMoveId, SessionInfo.UserSchema);
                        dynamic dataPayload = new ExpandoObject();
                        dataPayload.workflowActivityLog = applicationNotificationManagement.SetWorkflowLog(WorkflowActivityTypes.An_Activity_SubmitApplication.ToString());
                        WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                        {
                            data = dataPayload,
                            workflowData = workflowPayload
                        };
                        applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);
                        applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);

                    }
                    return Json(new { Success = res });
                }
                else
                    return Json(new { Success = false });

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/SubmitSORTSoApplication, Exception: {0}", ex));
                return Json(0);

            }
        }
        #endregion

        #region ViewSupplimentaryApplication()
        public ActionResult ViewSupplimentaryApplication(int appRevisionId = 0, int isClone = 0, int isRevise = 0)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Application/ViewSupplimentaryApplication actionResult method started successfully"));
                UserInfo SessionInfo = null;

                SessionInfo = (UserInfo)Session["UserInfo"];
                // SupplimentaryInfo supplimentaryInfo = applicationService.VR1GetSupplementaryInfo(appRevisionId, SessionInfo.UserSchema);
                ViewBag.IsClone = isClone;
                ViewBag.IsRevise = isRevise;
                ViewBag.AppRevId = appRevisionId;
                return PartialView("PartialView/_ViewApplicationSupplimentary");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/ViewSupplimentaryApplication, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region ViewVehicleAssignmentList
        public ActionResult ViewVehicleAssignmentList(long versionId = 0, string cont_Ref_No = "", long appRevisionId = 0)
        {
            List<MovementVehicleList> routeVehicleList;
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            routeVehicleList = vehicleConfigService.GetRouteVehicleList(appRevisionId, versionId, cont_Ref_No, SessionInfo.UserSchema);
            ViewBag.MovementRouteVehicleList = routeVehicleList;
            return PartialView("PartialView/_ApplicationRoutes");
        }
        #endregion

        #region ListSOMovements
        public ActionResult ListSOMovements(string hauliermnemonic, int movementId = 0, int esdalref = 0, int revisionno = 0, int versionno = 0, long revisionId = 0, long versionId = 0, int cloneapprevid = 0, long apprevid = 0, int projecid = 0, bool VR1Applciation = false, bool reduceddetailed = false, long pageflag = 1, bool soveh = false, bool VR1Notify = false, bool isQuickLink = false, int isNotifyFlag = 0, int Ishistoric = 0) //pageflag 1 for VR1 app , 2 for SO app
        {
            try
            {
                Session["RouteFlag"] = null;
                Session["RouteFlag"] = pageflag;
                ViewBag.WichPage = Session["RouteFlag"];
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                int organisationId = (int)SessionInfo.OrganisationId;
                if (SessionInfo.HelpdeskRedirect == "true")
                    ViewBag.Helpdesk_redirect = SessionInfo.HelpdeskRedirect;
                ApplyForVR1 applyForVR1 = new ApplyForVR1();
                ViewBag.organisationId = organisationId;
                ViewBag.revisionId = revisionId;
                ViewBag.hauliermnemonic = hauliermnemonic;
                ViewBag.esdalref = esdalref;
                ViewBag.revisionno = revisionno;
                ViewBag.versionno = versionno;
                ViewBag.VR1Applciation = VR1Applciation;
                ViewBag.projectId = projecid;
                if (pageflag == 3)
                    ViewBag.VR1Applciation = true;
                if (VR1Applciation)
                {
                    ViewBag.reduceddetailed = reduceddetailed;
                    if (revisionId != 0)
                    {
                        applyForVR1 = applicationService.GetVR1General(SessionInfo.UserSchema, apprevid, versionId, organisationId, Ishistoric);
                        if (applyForVR1.SubMovementClass == 241003 || applyForVR1.SubMovementClass == 241004 || applyForVR1.SubMovementClass == 241005)
                        {
                            ViewBag.VR1Applciation = true;
                        }
                        ViewBag.analysis_id = applyForVR1.AnalysisId;
                        ViewBag.reduceddetailed = !(applyForVR1.ReducedDetails == 0);
                    }
                }
                ViewBag.soveh = soveh;
                Session["pageflag"] = pageflag;

                var soGeneralDetails = applicationService.GetSOApplicationTabDetails(revisionId, versionId, SessionInfo.UserSchema, Ishistoric);
                if (SessionInfo.HelpdeskRedirect != "true" && !isQuickLink)
                    notificationService.InsertQuickLink(Convert.ToInt32(SessionInfo.UserId), projecid, 0, (int)revisionId, (int)versionId);
                TempData["analysisID"] = soGeneralDetails.AnalysisId;

                ViewBag.AnalysisID = soGeneralDetails.AnalysisId;
                ViewBag.ESDAL_Reference = soGeneralDetails.ESDALReference;
                ViewBag.Version_Status = soGeneralDetails.VersionStatus;
                ViewBag.ApprevisionId = soGeneralDetails.ApplicationRevisionId;

                if (soGeneralDetails.ApplicationStatus == 0)
                {
                    ViewBag.ApplicationStatus = applyForVR1.VR1ApplicationStatus;
                    soGeneralDetails.ApplicationStatus = applyForVR1.VR1ApplicationStatus;
                }
                else
                {
                    ViewBag.ApplicationStatus = soGeneralDetails.ApplicationStatus;
                }

                // creates so
                ViewBag.generaldone = "false";
                ViewBag.ApplicationRevId = soGeneralDetails.ApplicationRevisionId;
                ViewBag.MovementId = movementId;

                //clone
                if (cloneapprevid != 0)
                {
                    soGeneralDetails.ApplicationRevisionId = cloneapprevid;
                    soGeneralDetails.ApplicationStatus = 308001;
                    ViewBag.ApplicationStatus = 308001;
                    TempData["cloneapprevid"] = cloneapprevid;
                    ViewBag.ApprevisionId = cloneapprevid;
                    ViewBag.revisionId = cloneapprevid;
                    ViewBag.ESDAL_Reference = null;
                    ViewBag.Version_Status = 0;
                }
                if (soGeneralDetails.ApplicationRevisionId == 0)
                {
                    soGeneralDetails.ApplicationRevisionId = apprevid;
                    ViewBag.ApplicationRevId = (int)apprevid;
                }
                ViewBag.VR1Notify = VR1Notify;
                if (VR1Applciation)
                {
                    ViewBag.IsDistributed = applyForVR1.IsDistributed;
                    if (isNotifyFlag == 5)
                    {
                        ViewBag.ESDAL_Reference = null;
                    }
                    else if (applyForVR1.VR1ApplicationStatus == 308001) { ViewBag.ESDAL_Reference = null; }
                    else { ViewBag.ESDAL_Reference = applyForVR1.ESDALReference; }
                }
                else
                {
                    ViewBag.IsDistributed = soGeneralDetails.IsDistributed;
                }

                if (hauliermnemonic == null && esdalref == 0)
                {
                    soGeneralDetails.ApplicationStatus = 308001;
                }
                int ContactId = movementService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));
                ViewBag.ContactId = ContactId;

                string OrderNumber = movementService.GetSpecialOrderNo(soGeneralDetails.ESDALReference);
                ViewBag.OrderNumber = OrderNumber;
                ViewBag.VR1ContentRefNo = applyForVR1.VR1ContentRefNo;
                ViewBag.isNotifyFlag = isNotifyFlag;
                if (versionId == 0)
                {
                    ViewBag.versionId = applyForVR1.VersionId;
                }
                else
                {
                    ViewBag.versionId = versionId;
                }
                if (SessionInfo.HelpdeskRedirect != "true")//checking needs to attention for helpdesk redirect as a haulier
                {
                    applicationService.ResetNeedAttention(projecid, revisionId, versionId);
                }
                else
                {
                    #region ------saving loggs for helpdesk redirect for another login--------------
                    if (SessionInfo.UserName != null)
                    {
                        #region sys_events for saving loggin info for helpdesk redirect Haulier
                        MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                        string ErrMsg = string.Empty;
                        if (SessionInfo.UserSchema == UserSchema.Portal)
                        {
                            movactiontype.SystemEventType = SysEventType.Check_as_Haulier;
                            movactiontype.UserId = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                            movactiontype.UserName = SessionInfo.HelpdeskUserName;
                            movactiontype.ESDALRef = hauliermnemonic + "/" + esdalref + "/" + versionno;
                            //string sysEventDescp = STP.Domain.Sys_Events.System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                            int userId = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                            //loggingService.SaveSysEventsMovement(movactiontype, sysEventDescp, user_ID, UserSchema.Portal);
                            string sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                            loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, userId, SessionInfo.UserSchema);
                        }

                        #endregion
                    }
                    #endregion ------end--------------
                }
                ViewBag.IsHistory = Ishistoric;
                return View("ListSOMovements", soGeneralDetails);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Application/ListSOMovements, Exception: " + ex);
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region SOGeneralDetails
        public ActionResult SOGeneralDetails(string hauliermnemonic, int esdalref = 0, int revisionno = 0, int versionno = 0, long revisionId = 0, long versionId = 0, int isHistory = 0)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            if (SessionInfo.HelpdeskRedirect == "true")
            {
                ViewBag.Helpdesk_redirect = SessionInfo.HelpdeskRedirect;
            }
            int organisationId = (int)SessionInfo.OrganisationId;
            SOApplication soGeneralDetails = applicationService.GetSOGeneralDetails(revisionId, versionId, SessionInfo.UserSchema, isHistory);
            ViewBag.IncorrectValue = soGeneralDetails.ProjectId == 0;
            ViewBag.ProjectID = soGeneralDetails.ProjectId;

            long ProjectID = soGeneralDetails.ProjectId;
            int status = applicationService.GetApplicationStatus(versionno, revisionno, ProjectID, SessionInfo.UserSchema, isHistory);
            ViewBag.ApplStatus = status;
            //retreive notes to haulier
            string errormsg;
            string result;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\SOGeneral.xslt");
            result = Common.General.XsltTransformer.Trafo(soGeneralDetails.ApplicationNotesToHA, path, out errormsg);
           
            TempData["analysisID"] = soGeneralDetails.AnalysisId;
            ViewBag.App_Notes_To_HA = result;
            ViewBag.ESDAL_Reference = soGeneralDetails.ESDALReference;
            ViewBag.Version_Status = soGeneralDetails.VersionStatus;

            //APPLY FOLDER 
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("SOAppGetFolderDetails , RevisionID= {0},ProjectId= {1}", soGeneralDetails.ApplicationRevId, soGeneralDetails.ProjectId));
            var sofolderDetails = GetSetCommonFolderDetails(0, 0, soGeneralDetails.ProjectId, null, 0, 0, soGeneralDetails.ApplicationRevId);
            soGeneralDetails.ProjectFolderList = GetProjectFolderModel(organisationId);
            ViewBag.FolderID = new SelectList(soGeneralDetails.ProjectFolderList, "FolderID", "FolderName", sofolderDetails.FolderId);

            //retrieve NH contact details
            HAContact HAContactDet;
            string result1;
            string path1;
            HAContactDet = applicationService.GetHAContDetFromInboundDoc(soGeneralDetails.ESDALReference);

            path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\HAContactName.xslt");
            result1 = Common.General.XsltTransformer.Trafo(HAContactDet.ContactDetails, path1, out string errormsg1);
            ViewBag.ContactName = result1;

            TempData["Version_Status"] = soGeneralDetails.VersionStatus;

            //retreive distribution notes
            string result2;
            string path2;
            string newDistributionData = string.Empty;

            path2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\DistributionNotes.xslt");
            result2 = STP.Common.General.XsltTransformer.Trafo(HAContactDet.ContactDetails, path2, out string errormsg2);
            XmlDocument Doc = new XmlDocument();
            try
            {
                Doc.LoadXml(result2);
                XmlNodeList parentNode = Doc.GetElementsByTagName("body");
                foreach (XmlNode node in parentNode)
                {
                    newDistributionData = node.InnerText;
                }
            }
            catch(Exception ex)
            {
                newDistributionData = "";
            }            
           
            ViewBag.Distrib_Notes = result2;
            ViewBag.NewDistributionData = newDistributionData;

            if (ViewBag.IncorrectValue)
                soGeneralDetails = null;
            ViewBag.NotificationEditFlag = 0;
            if (soGeneralDetails != null && (soGeneralDetails.VersionStatus == 305004 || soGeneralDetails.VersionStatus == 305005 || soGeneralDetails.VersionStatus == 305006))
                ViewBag.NotificationEditFlag = 1;

            MovementPrint movPrint = new MovementPrint()
            {
                ContactId = movementService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId)),
                ESDALReferenceNumber = soGeneralDetails.ESDALReference
            };

            if (soGeneralDetails.VersionStatus == 305004 || soGeneralDetails.VersionStatus == 305005 || soGeneralDetails.VersionStatus == 305006)
            {
                movPrint.OrderNumber = movementService.GetSpecialOrderNo(movPrint.ESDALReferenceNumber);
            }

            movPrint.OrganisationId = SessionInfo.organisationId;
            movPrint.VersionId = soGeneralDetails.VersionStatus;

            ViewBag.CONTACTID = movPrint.ContactId;
            ViewBag.OrderNumber = movPrint.OrderNumber;
            ViewBag.IsHistory = isHistory;
            soGeneralDetails.SpecialOrders = movementService.GetSpecialOrders(soGeneralDetails.ESDALReference);

            return PartialView("SOGeneralDetails", soGeneralDetails);
        }
        #endregion

        #region ViewApplication
        public ActionResult ViewApplication(long appRevisionId = 0, long appVersionId = 0, int movementType = 0, int vehicleType = 0) //pageflag 1 for VR1 app , 2 for SO app
        {
            try
            {
                ViewBag.RevisionId = appRevisionId;
                if (movementType == 207002)
                    ViewBag.VersionId = appVersionId;

                return View("ViewApplication");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/ListSOMovements, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region GetHaulierContactDetails
        public ActionResult GetHaulierContactDetails(long notificationNumber, int isNEN = 0)
        {
            HAContact HAContactDet;
            ViewBag.ShowXML = 0;
            HAContactDet = notificationService.GetHaulierDetails(notificationNumber);
            return PartialView("ViewContactDetails", HAContactDet);
        }
        #endregion

        #region appvehicle_movementlist
        /// <summary>
        /// appvehicle_movementlist
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <param name="apprevisionId"></param>
        /// <returns></returns>
        public ActionResult AppVehicle_MovementList(int vehicleId = 0, int apprevisionId = 0, int routepartid = 0, bool IsVR1 = false, string VehicleType = "", string PrevMovEsdalRefNum = "", int SOVersionId = 0)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Application/appvehicle_movementlist actionResult method started successfully,input parameters are vehicleId: {0},apprevisionId: {1},routepartid: {2},IsVR1: {3},VehicleType: {4},PrevMovEsdalRefNum: {5},SOVersionId: {6}", vehicleId, apprevisionId, routepartid, IsVR1, VehicleType, PrevMovEsdalRefNum, SOVersionId));
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];
                //int organisationId = (int)SessionInfo.organisationId;
                int result = 0;
                string esdalref = "";

                //int latestroutepartid = 0;


                if (VehicleType == "ApplVehicle")
                {
                    result = vehicleConfigService.ImportApplnVehicleFromPreMove(vehicleId, apprevisionId, routepartid, SessionInfo.UserSchema);
                    if (result == 0)
                        result = vehicleConfigService.ImportRouteVehicleToAppVehicle(vehicleId, apprevisionId, routepartid, SessionInfo.UserSchema);

                }
                else
                {
                    result = IsVR1 ? vehicleConfigService.VR1AppVehicleMovementList(vehicleId, apprevisionId, routepartid, SessionInfo.UserSchema)
                        : vehicleConfigService.AppVehicleMovementList(vehicleId, apprevisionId, routepartid, SessionInfo.UserSchema);
                }

                if (result > 0)// Sort_imported_vehicle_from_previous_movement_for_ SO and VR1 
                {
                    #region System events for imported_vehicle_from_previous_movement_for_SO and VR1
                    MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                    movactiontype.RevisionId = apprevisionId;
                    movactiontype.VehicleId = result;
                    movactiontype.PrevMovVehicleId = vehicleId;
                    movactiontype.UserName = SessionInfo.UserName;

                    string ErrMsg = string.Empty;
                    int user_ID = Convert.ToInt32(SessionInfo.UserId);

                    if (SessionInfo.UserSchema == UserSchema.Sort) // For SORT Sort_imported_vehicle_from_previous_movement_for_so_application
                    {
                        if (!IsVR1)//for SORT SO application
                        {
                            #region Saving Sort_imported_vehicle_from_previous_movement_for_so_application
                            movactiontype.ESDALRef = PrevMovEsdalRefNum;
                            movactiontype.SystemEventType = SysEventType.Sort_imported_vehicle_from_previous_movement_for_so_application;
                            #endregion
                        }
                        else               //for SORT VR1 application
                        {
                            #region Saving Sort_imported_vehicle_from_previous_movement_for_vr1_application
                            movactiontype.SystemEventType = SysEventType.Sort_imported_vehicle_from_previous_movement_for_vr1_application;
                            #endregion
                        }
                        string sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                        bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, SessionInfo.UserSchema);
                    }
                    else if (SessionInfo.UserSchema == UserSchema.Portal)
                    {
                        if (!IsVR1)//for Haulier SO application
                        {
                            if (SOVersionId != 0)
                            {
                                esdalref = applicationService.GetEsdalRefNum(SOVersionId);
                            }
                            #region Saving HAulier_imported_vehicle_from_previous_movement_for_so_application
                            movactiontype.SystemEventType = SysEventType.Haulier_imported_vehicle_from_previous_movement_for_so_application;
                            movactiontype.ESDALRef = esdalref;
                            #endregion
                        }
                        else               //for Haulier VR1 application
                        {
                            #region Saving HAulier_imported_vehicle_from_previous_movement_for_vr1_application
                            movactiontype.SystemEventType = SysEventType.Haulier_imported_vehicle_from_previous_movment;
                            movactiontype.ESDALRef = PrevMovEsdalRefNum;
                            #endregion
                        }
                        string sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                        bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, SessionInfo.UserSchema);
                    }
                    #endregion
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex.Message);
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/SaveExistingFleetConfiguration, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");

            }
        }
        #endregion

        #region GetHaulierContactDetailsForProposal
        public ActionResult GetHaulierContactDetailsForProposal(long versionId, long revisionId)
        {
            HAContact HAContactDet = new HAContact();
            SOHaulierApplication sOApplicationObj = sortApplicationService.GetSORTSOHaulierDetails(revisionId);
            HAContactDet = new HAContact
            {
                HAAddress1 = sOApplicationObj.HaulierApplicantAddress1,
                HAAddress2 = sOApplicationObj.HaulierApplicantAddress2,
                HAAddress3 = sOApplicationObj.HaulierApplicantAddress3,
                HAAddress4 = sOApplicationObj.HaulierApplicantAddress4,
                HAAddress5 = sOApplicationObj.HaulierApplicantAddress5,
                ContactName = sOApplicationObj.HaulierContactName,
                OrganisationName = sOApplicationObj.HaulierApplicantName,
                Telephone = sOApplicationObj.HaulierTelephone,
                Fax = sOApplicationObj.HaulierFaxNumber,
                Email = sOApplicationObj.HaulierEmailId,
                PostCode = sOApplicationObj.HaulierPostCode,
                Country = sOApplicationObj.HaulierCountry,
            };
            ViewBag.ShowXML = 0;

            return PartialView("ViewContactDetails", HAContactDet);
        }
        #endregion

        #region ListImportedVehicleConfiguration
        public ActionResult ListImportedVehicleConfiguration(int apprevisionId = 0, int routepartId = 0, bool VRAPP = false, string ContentRefNo = "", bool IsNotif = false, bool SORTView = false, string status = "", bool Update = false, int verId = 0, int AgreedSO = 0, bool IsNEN = false)
        {
            try
            {
                int AgreedSOCondn = 0;
                ViewBag.AgreedSOCondn = 0;
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }

                //int apprevisionId = TempData["apprevisionId"] == null ? 0 : Convert.ToInt32(   TempData["apprevisionId"]);
                List<AppVehicleConfigList> vehicleconfigurationlist = new List<AppVehicleConfigList>();
                ViewBag.IsNEN = IsNEN;
                ViewBag.routecount = 0;
                ViewBag.ApplRevId = apprevisionId;
                ViewBag.IsNotif = IsNotif;

                List<AppRouteList> ImportedRoutelist = new List<AppRouteList>();

                //if (apprevisionId != 0)
                if (apprevisionId != 0 && !VRAPP)
                {
                    ImportedRoutelist = routesService.GetSoAppRouteList(apprevisionId, SessionInfo.UserSchema);
                    ViewBag.routecount = ImportedRoutelist.Count;
                    ViewBag.update = Update;

                    ViewBag.RoutePartsType = new SelectList(ImportedRoutelist, "routeID", "routetype");
                    ViewBag.RouteParts = new SelectList(ImportedRoutelist, "routeID", "routeName");
                    //vehicleconfigurationlist = ApplicationProvider.Instance.AppVehicleConfigList(apprevisionId, SessionInfo.userSchema);
                    vehicleconfigurationlist = vehicleConfigService.AppVehicleConfigList(apprevisionId, SessionInfo.UserSchema);
                    if (ImportedRoutelist.Count == 1)
                    {
                        for (int i = 0; i < vehicleconfigurationlist.Count; i++)
                        {
                            vehicleconfigurationlist[i].RoutePart = ImportedRoutelist[0].RouteName;
                        }
                    }
                }
                else if (VRAPP || IsNotif || ContentRefNo != "")
                {
                    ImportedRoutelist = routesService.NotifVR1RouteList(apprevisionId, ContentRefNo, verId, SessionInfo.UserSchema);
                    ViewBag.routecount = ImportedRoutelist.Count;
                    ViewBag.update = Update;

                    if (VRAPP)
                    {
                        ViewBag.VR1Appl = VRAPP;
                        //vehicleconfigurationlist = ApplicationProvider.Instance.AppVehicleConfigListvr1(routepartId, verId, ContentRefNo);
                        vehicleconfigurationlist = vehicleConfigService.AppVehicleConfigListVR1(routepartId, verId, ContentRefNo, SessionInfo.UserSchema);
                        if (ImportedRoutelist.Count == 1)
                        {
                            for (int i = 0; i < vehicleconfigurationlist.Count; i++)
                            {
                                vehicleconfigurationlist[i].RoutePart = ImportedRoutelist[0].RouteName;
                            }
                        }
                    }
                    else if (IsNotif || ContentRefNo != "")
                    {
                        IsNotif = true;
                        ViewBag.IsNotif = IsNotif;
                        ViewBag.ContentRefNo = ContentRefNo;
                        if (IsNEN == true)
                            //vehicleconfigurationlist = STP.NEN.Persistance.NENNotificationDAO.GetNEN_VehicleList(routepartId);
                            vehicleconfigurationlist = vehicleConfigService.GetNenVehicleList(routepartId);
                        else
                            //vehicleconfigurationlist = ApplicationProvider.Instance.AppVehicleConfigListvr1(routepartId, apprevisionId, ContentRefNo);
                            vehicleconfigurationlist = vehicleConfigService.AppVehicleConfigListVR1(routepartId, verId, ContentRefNo, SessionInfo.UserSchema);
                        if (vehicleconfigurationlist.Count == 1 && AgreedSO == 1)
                        {
                            ViewBag.AgreedSOCondn = 1;              //can not edit or delete vehicle
                        }
                        else if (vehicleconfigurationlist.Count > 1 && AgreedSO == 1)
                        {
                            ViewBag.AgreedSOCondn = 2;             //can delete vehicle but not route
                        }
                        if (ImportedRoutelist.Count == 1)
                        {
                            for (int i = 0; i < vehicleconfigurationlist.Count; i++)
                            {
                                vehicleconfigurationlist[i].RoutePart = ImportedRoutelist[0].RouteName;
                            }

                        }
                    }
                    if (routepartId != 0)
                    {
                        apprevisionId = 0;
                    }
                    ViewBag.RoutePartsType = new SelectList(ImportedRoutelist, "routeID", "routetype");
                    ViewBag.RouteParts = new SelectList(ImportedRoutelist, "routeID", "routeName");
                }
                ViewBag.status = status;
                ViewBag.SORTView = SORTView;
                ViewBag.AgreedSO = AgreedSO;

                return PartialView(vehicleconfigurationlist);
            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex.Message);
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/ImportConfigurationFromExistingMovement, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");

            }
        }
        #endregion

        #region ViewIndemnityConfirmation
        [HttpPost]
        public ActionResult ViewIndemnityConfirmation(IndemnityConfirmation indemnityConfirmation)/*long NotificationId = 0*/
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

                indemnityConfirmation.OrganisationName = SessionInfo.OrganisationName;
                PlanMovementType planMovementType = applicationService.GetNotificationDetails(indemnityConfirmation.NotificationId, SessionInfo.UserSchema);
                indemnityConfirmation.HaulierContact = planMovementType.HaulierContact;
                indemnityConfirmation.HaulierName = planMovementType.HaulierName;
                indemnityConfirmation.SentDateTime = planMovementType.NotificationDate;
                if (planMovementType.HaulierOnBehalfOf != null && planMovementType.HaulierOnBehalfOf != "")
                    indemnityConfirmation.OnBehalfOf = planMovementType.HaulierOnBehalfOf;
                return PartialView("PartialView/_ViewIndemnityConfirmation", indemnityConfirmation);

            }
            catch (Exception ex)
            {

                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/ViewIndemnityConfirmation, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }

        }
        #endregion

        #region DeleteApplication()
        public JsonResult DeleteApplication(int apprevisionId)
        {
            try
            {
                bool result = false;
                var sessionValues = (UserInfo)Session["UserInfo"];
                result = applicationService.DeleteApplication(apprevisionId, sessionValues.UserSchema);
                #region System Event Log - haulier_deleted_so_application
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                string ErrMsg = string.Empty;
                string sysEventDescp = string.Empty;
                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                movactiontype.UserName = SessionInfo.UserName;
                movactiontype.RevisionId = apprevisionId;
                movactiontype.SystemEventType = SysEventType.haulier_deleted_application;

                sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                #endregion
                return Json(new { Success = result });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/DeleteSelectedSOAppRoute, Exception: {0}", ex));
                return Json(0);

            }
        }
        #endregion

        #region WithdrawApplication
        public JsonResult WithdrawApplication(long Project_ID = 0, string Doc_type = "", string EsdalRefNumber = "", long app_rev_id = 0)
        {
            try
            {
                ApplicationWithdraw withDrawApp = applicationService.WithdrawApplication(Project_ID, app_rev_id);
                if (withDrawApp.Result)
                {
                    #region movement actions for this action method
                    UserInfo UserSessionValue = (UserInfo)Session["UserInfo"];
                    string ErrMsg = string.Empty;
                    MovementActionIdentifiers movactiontype = new MovementActionIdentifiers()
                    {
                        MovementActionType = MovementnActionType.haulier_withdraw_appl,
                        DocType = Doc_type,
                        ESDALRef = EsdalRefNumber,
                        ContactName = UserSessionValue.FirstName + " " + UserSessionValue.LastName
                    };
                    int versionNo = 0;
                    int revisionNo = 0;
                    string MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                    loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, Project_ID, revisionNo, versionNo,UserSessionValue.UserSchema);
                    #endregion
                }
                return Json(new { Success = withDrawApp.Result });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/DeleteSelectedSOAppRoute, Exception: {0}", ex));
                return Json(0);

            }
        }
        #endregion

        #region CheckingLatestAppStatus
        public JsonResult CheckLatestAppStatus(long Project_ID = 0)
        {
            try
            {
                SOApplication soStatusDetails = applicationService.CheckLatestAppStatus(Project_ID);
                return Json(new { result = soStatusDetails });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/DeleteSelectedSOAppRoute, Exception: {0}", ex));
                return Json(0);
            }
        }
        #endregion

        #region  CloneSOApplication
        [HttpPost]
        public ActionResult CloneSOApplication(long apprevid = 0, string ESDALRefCode = "", int isHistory = 0)
        {
            try
            {
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                int organisationId = (int)SessionInfo.OrganisationId;
                int userId = Convert.ToInt32(SessionInfo.UserId);
                Session["AppFlag"] = "SOApp";
                Session["IsRoute"] = true;
                ViewBag.organisationId = organisationId;
                SOApplication sOApplication;
                if (isHistory == 1)
                {
                    sOApplication = applicationService.CloneSOHistoryApplication(apprevid, organisationId, userId, SessionInfo.UserSchema);
                    routesService.UpdateCloneHistoricRoute(null, sOApplication.ApplicationRevId, 0, SessionInfo.UserSchema);
                }
                else
                    sOApplication = applicationService.CloneSOApplication(apprevid, organisationId, userId);
                TempData["analysisID"] = null;
                ViewBag.generaldone = "false";

                #region movement actions for this action method
                if (SessionInfo.UserSchema == UserSchema.Portal)
                {
                    UserInfo UserSessionValue = null; //--------object is used for stroing movement actions
                    UserSessionValue = (UserInfo)Session["UserInfo"];
                    MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                    movactiontype.MovementActionType = MovementnActionType.haulier_clones_application;
                    movactiontype.ESDALRef = ESDALRefCode;
                    movactiontype.RevisionId = (int)apprevid;
                    movactiontype.NewRevisionId = (int)sOApplication.ApplicationRevId;
                    movactiontype.UserName = UserSessionValue.UserName;
                    string ErrMsg = string.Empty;
                    string sysEventDescp = string.Empty;
                    string MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                    int versionNo = 0;
                    loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, sOApplication.ProjectId, sOApplication.LastRevisionNo,versionNo, UserSessionValue.UserSchema);
                    movactiontype.SystemEventType = SysEventType.Haulier_cloned_so_app;
                    sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.MovementActionType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                }
                #endregion

                var activityName = WorkflowTaskFinder.GenerateWorkflowActivityName(WorkflowActivityTypes.An_Activity_VehicleDetails);
                var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                PlanMvmntPayLoad planMvmntPayLoad = new PlanMvmntPayLoad
                {
                    NextActivity = activityName,
                    OrgId = SessionInfo.organisationId,
                    OrgName = SessionInfo.OrganisationName,
                    MovementKey = sOApplication.ApplicationRevId,
                    RevisionId = sOApplication.ApplicationRevId,
                    IsApp = true,
                    IsSortApp = false,
                    VehicleMoveId = sOApplication.MovementId,
                    VersionId = sOApplication.VersionId,
                    IsAppClone = true,
                    MovementType = (int)MovementType.special_order,
                    PrevMovType = (int)MovementType.special_order,
                    VehicleClass = sOApplication.VehicleClassification,
                    IsRouteVehicleAssigned = true
                };
                if (!applicationNotificationManagement.IsThisMovementExist(0, sOApplication.ApplicationRevId, out string workflowKey))
                {
                    applicationNotificationManagement.StartWorkflow(planMvmntPayLoad, 2);
                    new SessionData().E4_AN_PlanMovement = planMvmntPayLoad;
                }
                return Json(sOApplication);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/ListSOMovements, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region  ReviseSOApplication
        [HttpPost]
        public ActionResult ReviseSOApplication(long apprevid = 0, string ESDALRefCode = "", int revisionno = 0, int versionno = 0)
        {
            try
            {
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                int organisationId = (int)SessionInfo.OrganisationId;

                Session["AppFlag"] = "SOApp";
                Session["IsRoute"] = true;

                ViewBag.organisationId = organisationId;


                SOApplication sOApplication = applicationService.ReviseSOApplication(apprevid, SessionInfo.UserSchema);
                TempData["analysisID"] = null;
                ViewBag.generaldone = "false";

                ViewBag.hauliermnemonic = Convert.ToString(TempData["hauliermnemonic"]);
                ViewBag.esdalref = Convert.ToInt32(TempData["esdalref"]);
                ViewBag.revisionno = Convert.ToInt32(TempData["revisionno"]);
                ViewBag.versionno = Convert.ToInt32(TempData["versionno"]);

                #region movement actions for this action method
                if (SessionInfo.UserSchema == UserSchema.Portal)
                {
                    UserInfo UserSessionValue = null; //--------object is used for stroing movement actions
                    UserSessionValue = (UserInfo)Session["UserInfo"];
                    MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                    movactiontype.MovementActionType = MovementnActionType.haulier_revises_application;
                    movactiontype.ESDALRef = ESDALRefCode;
                    movactiontype.NewRevisionId = (int)sOApplication.ApplicationRevId;
                    movactiontype.RevisionId = (int)apprevid;
                    movactiontype.UserName = UserSessionValue.UserName;
                    string ErrMsg = string.Empty;
                    string sysEventDescp = string.Empty;
                    string MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                    loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, sOApplication.ProjectId, revisionno, versionno, UserSessionValue.UserSchema);
                    movactiontype.SystemEventType = SysEventType.Haulier_revised_so_app;
                    sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.MovementActionType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
                }
                if (SessionInfo.UserSchema == UserSchema.Sort)
                {
                    MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                    movactiontype.MovementActionType = MovementnActionType.sort_desktop_revises_appl;

                    UserInfo UserSessionValue = null; //--------object is used for stroing movement actions
                    UserSessionValue = (UserInfo)Session["UserInfo"];
                    movactiontype.ContactName = UserSessionValue.FirstName + " " + UserSessionValue.LastName;
                    movactiontype.ESDALRef = ESDALRefCode;
                    string MovementDescription = "";
                    string ErrMsg = string.Empty;
                    #region movement actions for this action method

                    MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                    loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, sOApplication.ProjectId, revisionno, versionno, UserSessionValue.UserSchema);
                    #endregion
                }
                #endregion

                var activityName = WorkflowTaskFinder.GenerateWorkflowActivityName(WorkflowActivityTypes.An_Activity_VehicleDetails);
                var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                PlanMvmntPayLoad planMvmntPayLoad = new PlanMvmntPayLoad
                {
                    NextActivity = activityName,
                    OrgId = (int)SessionInfo.OrganisationId,
                    OrgName = SessionInfo.OrganisationName,
                    MovementKey = sOApplication.ApplicationRevId,
                    RevisionId = sOApplication.ApplicationRevId,
                    IsApp = true,
                    IsSortApp = SessionInfo.UserTypeId == UserType.Sort,
                    VehicleMoveId = sOApplication.MovementId,
                    VersionId = sOApplication.VersionId,
                    IsRevise = true,
                    MovementType = (int)MovementType.special_order,
                    PrevMovType = (int)MovementType.special_order,
                    VehicleClass = sOApplication.VehicleClassification,
                    IsRouteVehicleAssigned = true
                };
                if (!applicationNotificationManagement.IsThisMovementExist(0, sOApplication.ApplicationRevId, out string workflowKey))
                {
                    applicationNotificationManagement.StartWorkflow(planMvmntPayLoad, 2);
                    new SessionData().E4_AN_PlanMovement = planMvmntPayLoad;
                }
                return Json(sOApplication);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/ListSOMovements, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region ReviseVR1Application
        public ActionResult ReviseVR1Application(int Revision_id, int Reduced_det = 0, int Clone_app = 0, int VersionID = 0, string ESDALRefCode = "", int isHistory = 0, int revisionno = 0, int versionno = 0)
        {
            UserInfo UserSessionValue = (UserInfo)Session["UserInfo"];
            ApplyForVR1 obj;

            if (isHistory == 1)
            {
                obj = applicationService.CloneHistoryVR1Application(Revision_id, Reduced_det, Clone_app, VersionID, UserSessionValue.UserSchema);
                routesService.UpdateCloneHistoricRoute(null, 0, obj.VersionId, UserSessionValue.UserSchema);
            }
            else
                obj = applicationService.ReviseVR1Application(Revision_id, Reduced_det, Clone_app, VersionID, UserSessionValue.UserSchema);

            ViewBag.VR1Applciation = true;
            Session["AppFlag"] = "VR1App";
            Session["IsRoute"] = true;
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

            #region movement actions for this action method
            if (SessionInfo.UserSchema == UserSchema.Portal)
            {
                
                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                if (Clone_app == 1)
                {
                    movactiontype.MovementActionType = MovementnActionType.haulier_clones_application;
                    movactiontype.SystemEventType = SysEventType.Haulier_cloned_vr1_app;
                }
                else
                {
                    movactiontype.MovementActionType = MovementnActionType.haulier_revises_application;
                    movactiontype.SystemEventType = SysEventType.Haulier_revised_vr1_app;
                }
                movactiontype.ESDALRef = ESDALRefCode;
                movactiontype.RevisionId = Revision_id;
                movactiontype.UserName = UserSessionValue.UserName;
                movactiontype.NewRevisionId = (int)obj.ApplicationRevisionId;
                string ErrMsg;
                string sysEventDescp;
                sysEventDescp = System_Events.GetSysEventString(UserSessionValue, movactiontype, out ErrMsg);
                loggingService.SaveSysEventsMovement((int)movactiontype.MovementActionType, sysEventDescp, Convert.ToInt32(UserSessionValue.UserId), UserSessionValue.UserSchema);

                string MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, obj.ProjectId, revisionno, versionno, UserSessionValue.UserSchema);
            }
            if (SessionInfo.UserSchema == UserSchema.Sort)
            {
                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                movactiontype.MovementActionType = MovementnActionType.sort_desktop_revises_appl;

                movactiontype.ContactName = UserSessionValue.FirstName + " " + UserSessionValue.LastName;
                movactiontype.ESDALRef = ESDALRefCode;
                string MovementDescription = "";
                string ErrMsg = string.Empty;
                #region movement actions for this action method

                MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, obj.ProjectId, revisionno, versionno, UserSessionValue.UserSchema);
                #endregion
            }
            #endregion

            var activityName = WorkflowTaskFinder.GenerateWorkflowActivityName(WorkflowActivityTypes.An_Activity_VehicleDetails);
            var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
            PlanMvmntPayLoad planMvmntPayLoad = new PlanMvmntPayLoad
            {
                NextActivity = activityName,
                OrgId = obj.OrganisationId,
                OrgName = obj.HaulierOrgName,
                MovementKey = obj.ApplicationRevisionId,
                NotificationId = 0,
                RevisionId = obj.ApplicationRevisionId,
                AnalysisId = 0,
                IsVr1App = true,
                IsApp = true,
                IsSortApp = false,
                VehicleMoveId = obj.MovementId,
                VersionId = obj.VersionId,
                IsRevise = Clone_app != 1,
                IsAppClone = Clone_app == 1,
                MovementType = (int)MovementType.vr_1,
                PrevMovType = (int)MovementType.vr_1,
                VehicleClass = obj.SubMovementClass,
                IsRouteVehicleAssigned = true
            };
            if (!applicationNotificationManagement.IsThisMovementExist(0, obj.ApplicationRevisionId, out string workflowKey))
            {
                applicationNotificationManagement.StartWorkflow(planMvmntPayLoad, 2);
                new SessionData().E4_AN_PlanMovement = planMvmntPayLoad;
            }
            return Json(obj);
        }
        #endregion

        #region SOHaulierApplicationDetails
        public ActionResult ApplicationDetails(long appRevisionId = 0, long appVersionId = 0, int historic = 0)
        {
            ViewBag.RevisionId = appRevisionId;
            ViewBag.VersionId = appVersionId;
            ViewBag.IsHistoric = historic;
            return PartialView();
        }
        public ActionResult SOHaulierApplicationDetails(long revisionId = 0, long versionId = 0, int historic = 0)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            SOHaulierApplication soHaulDetails;
            if (SessionInfo.UserTypeId != UserType.Sort)
                soHaulDetails = applicationService.GetSOHaulierDetails(revisionId, versionId, historic);
            else
                soHaulDetails = sortApplicationService.GetSORTSOHaulierDetails(revisionId);
            if (soHaulDetails.SubMovementClass == (int)VehicleClassificationType.SpecialOrder)
            {
                Session["AppFlag"] = "SOApp";
                Session["pageflag"] = "2";
                Session["RouteFlag"] = "2";
            }
            else
            {
                Session["AppFlag"] = "VR1App";
                Session["pageflag"] = "1";
                Session["RouteFlag"] = "1";
            }
            Session["IsRoute"] = true;

            return PartialView(soHaulDetails);
        }
        public ActionResult ViewSupplimentaryInfo(int appRevisionId = 0, int historic = 0)
        {
            try
            {
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                SupplimentaryInfo supplimentaryInfo = applicationService.VR1GetSupplementaryInfo(appRevisionId, SessionInfo.UserSchema, historic);
                return PartialView("ViewSupplimentaryInfo", supplimentaryInfo);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/ViewSupplimentaryInfo, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult ViewRouteVehicleList(long versionId = 0, string cont_Ref_No = null, long appRevisionId = 0)
        {
            ViewBag.VersionId = versionId;
            ViewBag.RevisionId = appRevisionId;
            ViewBag.ContentRefNum = cont_Ref_No;
            return PartialView("ApplicationRouteVehicle");
        }

        public ActionResult ViewAppRouteList(long versionId = 0, long appRevisionId = 0, string cont_Ref_No = null)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            List<AppRouteList> appRouteLists;
            if (Convert.ToString(Session["AppFlag"]) == "VR1App" || !string.IsNullOrWhiteSpace(cont_Ref_No))
                appRouteLists = routesService.NotifVR1RouteList(appRevisionId, cont_Ref_No, versionId, SessionInfo.UserSchema);
            else
                appRouteLists = routesService.GetSoAppRouteList(appRevisionId, SessionInfo.UserSchema);

            ViewBag.RoutePardId = appRouteLists!=null && appRouteLists.Any() ? appRouteLists.First().RouteID:0;
            ViewBag.RouteType = appRouteLists != null && appRouteLists.Any() ? appRouteLists.First().RouteType:null;
            ViewBag.RouteCount = appRouteLists.Count;

            return PartialView("ApplicationRouteList", appRouteLists);
        }

        #endregion

        #region RouteAnalysisPanel
        public ActionResult RouteAnalysisPanel(int versionId = 0, long analysisId = 0, bool IsNotifRouteassessment = false, int RivisionId = 0, bool SORTflag = false, bool IsCandidate = false, long CheckerId = 0, int CheckerStatus = 0, bool IsCandLastVersion = false, long planneruserId = 0, int appStatusCode = 0, int MovVersionNo = 0, decimal IsDistributed = 0, string contentRefNo = "", string SONumber = "", bool IsVr1 = false, string SORTStatus = null)
        {
            ViewBag.versionId = versionId;
            TempData["analysisID"] = analysisId;
            ViewBag.AnalysisId = analysisId;
            ViewBag.RivisionId = RivisionId;
            ViewBag.SORTflag = SORTflag;
            ViewBag.IsNotifRouteassessment = IsNotifRouteassessment;
            ViewBag.CandidateRT = IsCandidate;
            ViewBag.AppStatus = appStatusCode;
            ViewBag.VR1ContentRefNo = contentRefNo; // added for vr-1 applications
            try
            {

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");
                else
                    SessionInfo = (UserInfo)Session["UserInfo"];

                if (SessionInfo.HelpdeskRedirect == "true")
                {
                    ViewBag.Helpdest_redirect = SessionInfo.HelpdeskRedirect;
                }

                if (IsCandLastVersion)
                {
                    SortActions sactions = new Common.SortHelper.SortActions();
                    sactions = CheckingProcess.SortSOActions(sactions, SessionInfo.SortUserId, planneruserId, CheckerId, SessionInfo.SORTCanAgreeAllSO, appStatusCode, CheckerStatus, IsDistributed, 11, MovVersionNo, 0);

                    ViewBag.IsCandidateRTCreate = sactions.EditCandidateVersion;
                    ViewBag.SendforChecking = sactions.SendforChecking;
                    ViewBag.CompleteChecking = sactions.CompleteChecking;
                    ViewBag.MovementCreation = sactions.CreateMovementVersion;
                    ViewBag.Agreed = sactions.Agree;
                    ViewBag.SendForQAChecking = sactions.SendForQAChecking;
                    ViewBag.CompleteQAChecking = sactions.CompleteQAChecking;
                    ViewBag.SendforSignoff = sactions.SendForSignOff;
                    ViewBag.SignOff = sactions.SignOff;
                    ViewBag.CreateRevisedApplication = sactions.CreateRevisedApplication;
                }
                else if (SORTStatus == "MoveVer" && !IsVr1)
                {
                    if (SessionInfo.SortUserId == planneruserId)
                    {
                        if ((appStatusCode == 307002 || appStatusCode == 307012) && CheckerStatus == 301003)
                            ViewBag.Distribution = true;
                        else if ((appStatusCode == 307005 || appStatusCode == 307007) && CheckerStatus == 301006)
                            ViewBag.Distribution = true;
                    }
                    ViewBag.SORTStatus = SORTStatus;
                }
                bool IsDIGenerate = false;
                RouteAssessmentModel objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisId, 9, SessionInfo.UserSchema);
                if (!IsVr1 && SORTflag && !IsCandidate)
                {
                    ViewBag.IsGenerate = false;
                    if (objRouteAssessmentModel.DriveInst == null)
                        IsDIGenerate = true;
                }
                ViewBag.IsDIGenerate = IsDIGenerate;
                if (IsNotifRouteassessment)
                {
                    int unsuitStructure = objRouteAssessmentModel.AffectedStructure != null ? GetUnsuitStructureCount(objRouteAssessmentModel.AffectedStructure, objRouteAssessmentModel.Cautions) : 0;
                    int unsuitConstraint = objRouteAssessmentModel.Constraints != null ? GetUnsuitConstraintCount(objRouteAssessmentModel.Constraints, objRouteAssessmentModel.Cautions) : 0;
                    ViewBag.UnsuitStruct = unsuitStructure;
                    ViewBag.UnsuitConstraint = unsuitConstraint;

                    var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                    PlanMvmntPayLoad planMvmntPayLoad = applicationNotificationManagement.GetPlanMvmtPayload();
                    
                    return PartialView("RouteAnalysisNotification");
                }
                else
                {
                    return PartialView("RouteAnalysisPanel");
                }
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "Exception:", ex);
                return RedirectToAction("Error", "Home");
            }
        }

        private int GetUnsuitStructureCount(byte[] affectedStructures, byte[] cautions)
        {
            int result = 0;
            //string xmlAffectedStructure = Encoding.UTF8.GetString(XsltTransformer.Trafo(affectedStructures));
            //var objAffectedStructures = StringExtraction.XmlDeserializerStructures(xmlAffectedStructure);

            string affectedStructuresxml = Encoding.UTF8.GetString(XsltTransformer.Trafo(affectedStructures));
            var newAnalysedStructures = XmlAffectedStructuresDeserializer.XmlAffectedStructuresDeserialize(affectedStructuresxml);

            string cautionsxml = Encoding.UTF8.GetString(XsltTransformer.Trafo(cautions));
            STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions newCautions = XmlDeserializerGeneric<STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions>.XmlDeserialize(cautionsxml);
            routeAssessmentService.GetUnsuitableStructuresWithCautions(false, false, ref newAnalysedStructures, ref newCautions);

            result = newAnalysedStructures.AnalysedStructuresPart.SelectMany(structure => structure.Structure.SelectMany
                    (appraisal => appraisal.Appraisal.Where(suitability => suitability.AppraisalSuitability.Value != null &&
                        (suitability.AppraisalSuitability.Value.ToLower().Contains("unsuitable") ||
                        suitability.AppraisalSuitability.Value.ToLower().Contains("not specified") 
                        || suitability.AppraisalSuitability.Value.ToLower().Contains("not structure specified"))).Select(item => new { }))).Count();
            return result;
        }

        private int GetUnsuitConstraintCount(byte[] affectedConstraints, byte[] cautions)
        {
            int result = 0;
            //string xmlAffectedConstraints = Encoding.UTF8.GetString(XsltTransformer.Trafo(affectedConstraints));
            //var objAffectedConstraint = StringExtraction.constraintDeserializer(xmlAffectedConstraints);
            
            string constraintsxml = Encoding.UTF8.GetString(XsltTransformer.Trafo(affectedConstraints));
            STP.Domain.RouteAssessment.XmlConstraints.AnalysedConstraints newConstraints = XmlDeserializerGeneric<STP.Domain.RouteAssessment.XmlConstraints.AnalysedConstraints>.XmlDeserialize(constraintsxml);

            string cautionsxml = Encoding.UTF8.GetString(XsltTransformer.Trafo(cautions));
            STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions newCautions = XmlDeserializerGeneric<STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions>.XmlDeserialize(cautionsxml);
            routeAssessmentService.GetUnsuitableConstraintsWithCautions(false, false, ref newConstraints, ref newCautions);

            result = newConstraints.AnalysedConstraintsPart.SelectMany(constraints => constraints.Constraint.Where
                    (suitability => suitability.Appraisal.Suitability.Value != null && (suitability.Appraisal.Suitability.Value.ToLower().Contains("unsuitable")||
                    suitability.Appraisal.Suitability.Value.ToLower().Contains("not specified"))).
                    Select(item => new { })).Count();
            return result;
        }

        private int GetUnsuitCautionCount(byte[] affectedCautions)
        {
            int result = 0;
            string xmlAffectedCaution = Encoding.UTF8.GetString(XsltTransformer.Trafo(affectedCautions));
            var objAffectedCautions = StringExtraction.XmlDeserializeCautions(xmlAffectedCaution);
            
            result = objAffectedCautions.AnalysedCautionsPart.Where(analCautions => analCautions.Caution.Count > 0).SelectMany
                (analCautions => analCautions.Caution.Where(item => item.Vehicle[0] != null && item.Vehicle[0].ToLower().Contains("unsuitable")).Select(item => new { })).Count();
            return result;
        }
        #endregion

        #region Updateroutepart
        public ActionResult Updateroutepart(string VehicleArray, string RouteArray, string RouteTypeArray, int arrlen, bool VR1Appl, bool Notif = false, bool Iscand=false)
        {
            int res = 1;
            int VehicleId = 0;
            int PartId = 0;
            string RType = "";

            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }

            if (arrlen != 0)
            {
                VehicleArray = VehicleArray.TrimEnd(',');
                RouteArray = RouteArray.TrimEnd(',');
                RouteTypeArray = RouteTypeArray.TrimEnd(',');

                string[] VehArr = VehicleArray.Split(',');
                string[] RouteArr = RouteArray.Split(',');
                string[] RouteTypeArr = RouteTypeArray.Split(',');

                for (int i = 0; i < arrlen; i++)
                {

                    VehicleId = Convert.ToInt32(VehArr[i]);
                    PartId = Convert.ToInt32(RouteArr[i]);
                    RType = RouteTypeArr[i].ToString();
                    res = applicationService.UpdatePartId(VehicleId, PartId, VR1Appl, Notif, RType, Iscand, SessionInfo.UserSchema);
                }
            }
            return Json(new { result = res });
        }
        #endregion

        #region DeleteSelectedVehicleComponent()
        public JsonResult DeleteSelectedVehicleComponent(int vehicleId, bool isVR1 = false, int appRevID = 0, bool isNotif = false, int NotificationId = 0)
        {
            try
            {
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }

                bool result = !isVR1
                    ? vehicleConfigService.DeleteSelectedVehicleComponent(vehicleId, SessionInfo.UserSchema)
                    : vehicleConfigService.DeleteSelectedVR1VehicleComponent(vehicleId, SessionInfo.UserSchema);

                #region System events for Sort_deleted_vehicle_for SO and VR1
                if (result)
                {
                    MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                    movactiontype.RevisionId = appRevID;
                    movactiontype.VehicleId = vehicleId;
                    movactiontype.UserName = SessionInfo.UserName;
                    movactiontype.NotificationID = NotificationId;
                    string ErrMsg = string.Empty;
                    int user_ID = Convert.ToInt32(SessionInfo.UserId);

                    if (SessionInfo.UserSchema == UserSchema.Sort) // For SORT Vehicle delete Log
                    {
                        if (!isVR1)//for SORT SO application
                        {
                            #region Saving sort_deleted_vehicle_for_so_application
                            movactiontype.SystemEventType = SysEventType.sort_deleted_vehicle_for_so_application;
                            #endregion
                        }
                        else               //for SORT VR1 application
                        {
                            #region Saving Sort_deleted_vehicle_for_vr1_application
                            movactiontype.SystemEventType = SysEventType.Sort_deleted_vehicle_for_vr1_application;
                            #endregion
                        }
                        string sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                        bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, SessionInfo.UserSchema);
                    }
                    else if (SessionInfo.UserSchema == UserSchema.Portal)
                    {
                        // For Haulier Vehicle creation Log
                        if (!isVR1)//for SO application
                        {
                            #region Saving Haulier_deleted_vehicle_for_so_application
                            movactiontype.SystemEventType = SysEventType.Haulier_deleted_vehicle_for_so_application;
                            #endregion
                        }
                        else               //for  Notification/VR1 application
                        {
                            #region Saving Haulier_deleted_vehicle_for_vr1_application/notification
                            if (isNotif && isVR1)
                            {
                                movactiontype.SystemEventType = SysEventType.Haulier_deleted_vehicle;
                            }
                            else if (!isNotif && isVR1)
                            {
                                movactiontype.SystemEventType = SysEventType.Haulier_deleted_vehicle_for_vr1_application;
                            }

                            #endregion
                        }
                        string sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                        bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, SessionInfo.UserSchema);

                    }
                }
                #endregion
                return Json(new { Success = result }); //View(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/DeleteSelectedVehicleComponent, Exception: {0}", ex));
                return Json(0);

            }
        }
        #endregion

        #region AffectedStructureDetails
        public ActionResult StructureGeneralDetails(string StructureCode, int SectionId)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Application/StructureGeneralDetails actionResult method started successfully"));

                List<AffStructureSectionList> objListStructureSection = new List<AffStructureSectionList>();
                objListStructureSection = applicationService.ViewAffStructureSections(StructureCode);
                ViewBag.ListStructureSections = objListStructureSection;
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                ViewBag.usrTypeId = SessionInfo.UserTypeId;
                #region Function to get structure general detail list

                List<AffStructureGeneralDetails> objDetailList = new List<AffStructureGeneralDetails>();

                objDetailList = applicationService.GetStructureDetailList(StructureCode, SectionId);

                #endregion

                return PartialView("StructureGeneralDetails", objDetailList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Home/Index, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion

        #region ViewSupplementary
        public ActionResult ViewSupplementary(int appRevisionId = 0, int isClone = 0, int isRevise = 0)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Application/ViewSupplementary actionResult method started successfully"));
                UserInfo SessionInfo = null;

                SessionInfo = (UserInfo)Session["UserInfo"];
                SupplimentaryInfo supplimentaryInfo = applicationService.VR1GetSupplementaryInfo(appRevisionId, SessionInfo.UserSchema, 0);
                ViewBag.IsClone = isClone;
                ViewBag.IsRevise = isRevise;
                var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                if (applicationNotificationManagement.IsThisMovementExist(0, appRevisionId, out string workflowKey)
                    && WorkflowTaskFinder.FindNextTask("HaulierApplication", WorkflowActivityTypes.An_Activity_ApplicationAttributes, out dynamic workflowPayload) != string.Empty)
                {

                    dynamic dataPayload = new ExpandoObject();
                    dataPayload.supplimentaryInfo = supplimentaryInfo;
                    dataPayload.workflowActivityLog = applicationNotificationManagement.SetWorkflowLog(WorkflowActivityTypes.An_Activity_ApplicationAttributes.ToString());
                    WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                    {
                        data = dataPayload,
                        workflowData = workflowPayload
                    };
                    applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);
                }
                return PartialView("PartialView/_ViewSupplementary", supplimentaryInfo);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/ViewSupplementary, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region HaulierApplRouteParts
        public ActionResult HaulierApplRouteParts(int RevisionId = 0, bool soapp = false, bool approute = false, int VersionId = 0, bool SORTVehRoute = false, int NotifID = 0, bool SubmitVR1 = false, bool vr1app = false, string VR1ContentRefNo = "", long AppStatus = 0, bool IsSort = false, int VersionStatus = 0, bool RouteVehFlag = false, string type = "")
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            int SortRouteVehFlag = 0;
            Session["RouteAssessmentFlag"] = "Completed";

            //----For regression issue reported mail that Haulier created SO route and vehicle is not shown to SORT portal------
            if (RouteVehFlag)
            {
                SortRouteVehFlag = 1;
            }
            //---------------------------
            ViewBag.RevisionId = RevisionId;
            ViewBag.soapp = soapp;
            ViewBag.VrOneRoute = vr1app;
            ViewBag.approute = approute;
            ViewBag.SORTVehRoute = SORTVehRoute;
            ViewBag.NotifShowVeh = false;
            ViewBag.VersionStatus = VersionStatus;
            List<AffectedStructures> ojbroute = new List<AffectedStructures>();
            
            if (VersionId != 0)
            {
                ojbroute = applicationService.GetSORTHaulierAppRouteParts(VersionId, VR1ContentRefNo, SessionInfo.UserSchema);
                ViewBag.VersionId = VersionId;
            }
            else if (NotifID != 0)
            {
                ojbroute = applicationService.GetNotifRouteParts(NotifID, 0);
                ViewBag.NotifShowVeh = true;
            }
            else
            {
                if (Session["RouteFlag"] != null && Session["RouteFlag"].ToString() == "1")
                    ojbroute = applicationService.GetHaulierApplRouteParts(RevisionId, "1", "0", SessionInfo.UserSchema);
                else if (AppStatus > 308001 || IsSort)
                    ojbroute = applicationService.GetHaulierApplRouteParts(RevisionId, "0", SortRouteVehFlag.ToString(), SessionInfo.UserSchema);
                else
                    ojbroute = applicationService.GetHaulierApplRouteParts(RevisionId, "2", "0", SessionInfo.UserSchema);
            }
            if (ojbroute.Count == 1)
            {
                ViewBag.Display = "1";
            }
            if (SubmitVR1)
            {
                ViewBag.NotifShowVeh = true;
            }
            if(ojbroute.Count == 1)
            {
                ViewBag.RoutePartId = ojbroute.FirstOrDefault().PartId;
            }
            if (type == "Vehicle")
            {
                return PartialView("~/Views/Application/PartialView/HaulierApplVehicleParts.cshtml", ojbroute);
            }
            else
            {
                return PartialView("~/Views/Application/PartialView/HaulierApplRouteParts.cshtml", ojbroute);
            }

        }
        #endregion

        #region ApplicationVehicle
        public ActionResult ApplicationVehicle(int PartId, int RevisionId = 0, bool SORTMOV = false, bool IsNotifRoutePart = false, bool IsVRVeh = false, int EditRouteVeh = 0)
        {

            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }

            //List<ApplicationVehicle> objVehicle = new List<ApplicationVehicle>();
            List<VehicleDetails> vehicleDetails = new List<VehicleDetails>();
            if (IsNotifRoutePart == false)
            {
                if (SORTMOV != false)
                {
                    ViewBag.SORTMOV = SORTMOV;
                    vehicleDetails = vehicleConfigService.GetSORTMovVehicle(PartId, UserSchema.Sort);
                }
                else
                {
                    vehicleDetails = vehicleConfigService.GetApplVehicle(PartId, RevisionId, IsVRVeh, SessionInfo.UserSchema);
                }
            }
            ViewBag.IsVRVeh = IsVRVeh;
            ViewBag.EditRouteVeh = EditRouteVeh;

            GetVehicleImage(vehicleDetails);
            ViewBag.VehicleList = vehicleDetails;

            return PartialView("~/Views/Application/PartialView/ApplicationVehicle.cshtml", vehicleDetails);
        }
        #endregion

        #region VR1GeneralDetails
        public ActionResult VR1GeneralDetails(int applicationrevid = 0, bool reduceddetailed = true, bool VR1Notify = false, long versionid = 0, bool hideflag = false,int isHistory = 0)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Application/ApplyVR1GeneralDetails actionResult method started successfully"));
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action"); //this.ControllerContext.RouteData.Values["action"];
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");
                    return RedirectToAction("Login", "Account");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];
                int organisationId = (int)SessionInfo.OrganisationId;
                if (SessionInfo.HelpdeskRedirect == "true")
                {
                    ViewBag.Helpdest_redirect = SessionInfo.HelpdeskRedirect;
                }
                ViewBag.reduceddetailed = reduceddetailed;
                ApplyForVR1 applyForVR1 = new ApplyForVR1();

                ViewBag.Iscreate = "true";
                if (applicationrevid != 0)
                    applyForVR1 = applicationService.GetVR1General(SessionInfo.UserSchema, applicationrevid, versionid, organisationId, isHistory);
                VR1VehicleDetailsParams vr1VehicleDetailsParams = new VR1VehicleDetailsParams()
                {
                    VR1Application = applyForVR1,
                    ContentNo = applyForVR1.VR1ContentRefNo,
                    UserSchema = SessionInfo.UserSchema,
                    Historic = isHistory
                };
                applyForVR1 = applicationService.GetVR1VehicleDEtails(vr1VehicleDetailsParams);

                ViewBag.ProjectID = applyForVR1.ProjectId;

                //APPLY FOLDER 
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("VR1AppGetFolderDetails , RevisionID= {0},ProjectId= {1}", applicationrevid, applyForVR1.ProjectId));
                var vr1folderDetails = GetSetCommonFolderDetails(0, 0, applyForVR1.ProjectId, null, 0, 0, applicationrevid);//fetching selected folder details for this project
                applyForVR1.ProjectFolderList = GetProjectFolderModel(organisationId);//fetching all folders for this organisation            
                ViewBag.FolderID = new SelectList(applyForVR1.ProjectFolderList, "FolderID", "FolderName", vr1folderDetails.FolderId);

                ViewBag.RevisionId = applicationrevid;
                ViewBag.VR1Notify = VR1Notify;
                ViewBag.Vr1ApplStatus = applyForVR1.VR1ApplicationStatus;
                ViewBag.versionid = versionid;
                ViewBag.hideflag = hideflag;

                if (applyForVR1.ProjectId == 0)
                {
                    applyForVR1 = null;
                }
                // if (applyForVR1.NotesWithAppl != null)
                //applyForVR1.NotesWithAppl = applyForVR1.NotesWithAppl.Replace("\r\n", "<br/>");
                applyForVR1.ApplicationRevisionId = applicationrevid;
                ViewBag.IsHistory = isHistory;
                return PartialView(applyForVR1);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/ApplyVR1GeneralDetails, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region AgreedRoutes
        public ActionResult AgreedRoutes(int VersionId = 0, string ContentRefNo = "0", bool IsNotifRouteVehicle = false, int revisionid = 0, bool VR1 = false, bool NotifPrevMoveImportVeh = false, int NotifId = 0, bool isSort = false, bool ViewRoute = false)//int RevisionId = 0, bool approute = false
        {
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }
            ViewBag.ViewRoute = ViewRoute;
            List<AffectedStructures> ojbroute = new List<AffectedStructures>();
            if (isSort == false)
            {
                //ContentRefNo == "0" && 
                if (NotifId == 0)
                {
                    ViewBag.VersionId = VersionId;
                    ViewBag.IsNotifRoute = false;
                    if (VR1)
                    {
                        ojbroute = applicationService.GetAgreedRouteParts(VersionId, 0, SessionInfo.UserSchema, ContentRefNo);
                    }
                    else
                    {
                        ojbroute = applicationService.GetAgreedRouteParts(VersionId, 0, SessionInfo.UserSchema);
                    }

                }
                else if (VersionId == 0)
                {
                    ViewBag.VersionId = VersionId;
                    ViewBag.NotifPrevMoveImportVeh = NotifPrevMoveImportVeh;

                    if (IsNotifRouteVehicle == true)
                    {
                        ViewBag.IsNotifRouteVehicle = true;
                    }
                    else
                    {
                        ViewBag.IsNotifRoute = true;
                    }
                    ojbroute = applicationService.GetNotifRouteParts(NotifId, 1);
                }
            }
            else
            {
                ViewBag.VersionId = VersionId;
                ViewBag.IsNotifRoute = false;
                ojbroute = sortApplicationService.GetAgreedRouteParts(revisionid, SessionInfo.UserSchema);
            }
            // ViewBag.RevisionId = RevisionId;
            //ViewBag.approute = approute;
            ViewBag.AppType = isSort;
            return PartialView("PartialView/AgreedRoutes", ojbroute);

        }
        #endregion
        
        #region GetSetCommonFolderDetails
        public ProjectFolderModel GetSetCommonFolderDetails(int flag = 0, long folderID = 0, long projectId = 0, string hauliermnemonic = null, int esdalref = 0, long notificationId = 0, long revisionID = 0)
        {
            ProjectFolderModel projectFolderModel = new ProjectFolderModel();
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("GetSetCommonFolderDetails , NotificationId= {0},ProjectId= {1}, RevisionId: {2}, Flag: {3}, FolderID: {4}", notificationId, projectId, revisionID, flag, folderID));
            projectFolderModel = applicationService.GetFolderDetails(flag, folderID, projectId, hauliermnemonic, esdalref, notificationId, revisionID);

            return projectFolderModel;

        }
        #endregion

        #region  public ActionResult RouteConfig(int versionId)
        /// <summary>
        /// ActionResult RouteConfig 
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>

        public ActionResult RouteConfig(int versionId = 0, int revisionId = 0, bool Vr1 = false, bool isSort = false)
        {
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }
            List<AffectedStructures> ojbAffstruct = new List<AffectedStructures>();
            ViewBag.VersionId = versionId;
            if (isSort == false)
            {
                if (Vr1)
                {
                    ojbAffstruct = applicationService.GetAgreedRouteParts(0, revisionId, SessionInfo.UserSchema);
                    ViewBag.vr1 = true;
                }
                else
                {
                    ViewBag.vr1 = true;
                    ojbAffstruct = applicationService.GetAgreedRouteParts(versionId, 0, SessionInfo.UserSchema);
                    Session["AppFlag"] = "VR1App";
                    Session["IsRoute"] = true;
                }
            }
            else
            {
                ojbAffstruct = sortApplicationService.GetAgreedRouteParts(revisionId, SessionInfo.UserSchema);
                ViewBag.vr1 = true;
            }
            Session["RouteAssessmentFlag"] = "Completed";
            ViewBag.MovementRouteList = ojbAffstruct;
            return PartialView("PartialView/RouteVehicle");
        }
        #endregion

        #region NotificationHistory
        public ActionResult NotificationHistory(int versionId = 0, int orgId = 0, int mode = 0, string esdalref = "", int versionno = 0, int notifhistory = 0)
        {
            List<InboxSubContent> objInbox = new List<InboxSubContent>();

            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            int organisationId = (int)SessionInfo.OrganisationId;
            if (mode != 2)
            {
                objInbox = notificationService.GetInboxSubContent(1, 10, versionId, organisationId, notifhistory);
            }
            if (esdalref != "")
            {
                objInbox = notificationService.GetSORTHistoryDetails(esdalref, versionno);
            }
            ViewBag.mode = mode;
            return PartialView("ViewNotificationHistory", objInbox);
            //return View(objInbox);
        }
        #endregion

        #region  ListImportedRouteFromLibrary
        /// <summary>
        /// ListImportedRouteFromLibrary
        /// </summary>
        /// <returns></returns>
        public ActionResult ListImportedRouteFromLibrary(int apprevisionId = 0, string CONT_REF_NUM = null, bool appParts = false, string AuthoMov_VersionID = "0", string AuthoMov_CONT_REF = "0", int VersionId = 0, int AgreedSo = 0, int NEN_ID = 0, bool IsNEN = false, bool IsNEN_authoMov = false, string nenInboxId = "")
        {
            try
            {
                int inboxId = 0;

                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                UserInfo SessionInfo = new UserInfo();
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                if (!String.IsNullOrEmpty(nenInboxId))
                {
                    inboxId = Convert.ToInt32(STP.Common.EncryptDecrypt.MD5EncryptDecrypt.DecryptDetails(nenInboxId));
                }
                else
                {
                    inboxId = Convert.ToInt32(Session["NENINBOX_ITEM_ID"]);
                }
                ViewBag.AuthoMov_VersionID = AuthoMov_VersionID;
                ViewBag.AuthoMov_CONT_REF = "0";
                ViewBag.AgreedSO = 0;
                ViewBag.IsNEN = IsNEN;
                Session["MovVersionID"] = AuthoMov_VersionID;

                List<AppRouteList> ImportedRoutelist = new List<AppRouteList>();

                if (AuthoMov_VersionID != "0" || AuthoMov_CONT_REF != "0")//This is for Authorized movemens
                {
                    if (AuthoMov_CONT_REF != "0")
                    {
                        ImportedRoutelist = routesService.NotifVR1RouteList(apprevisionId, AuthoMov_CONT_REF, 0, UserSchema.Portal);
                        ViewBag.AuthoMov_CONT_REF = AuthoMov_CONT_REF;
                        Session["RouteFlag"] = 3;
                    }
                    else
                    {
                        long version_id = Convert.ToInt32(AuthoMov_VersionID);
                        ImportedRoutelist = routesService.GetAuthorizedRoutePartList(version_id, SessionInfo.UserSchema);
                        ViewBag.AuthoMov_VersionID = AuthoMov_VersionID;
                        Session["RouteFlag"] = 2;
                    }
                }
                else if (IsNEN)
                {
                    Session["mainRouteDesc"] = ""; Session["mainReturnRouteDesc"] = "";
                    ViewBag.IsNEN_authoMov = IsNEN_authoMov;
                    int userId = Convert.ToInt32(SessionInfo.UserId);
                    int orgId = Convert.ToInt32(SessionInfo.OrganisationId);
                    long nenId = NEN_ID;
                    ImportedRoutelist = routesService.GetPlannedNenRouteList(nenId, userId, inboxId, orgId); // changed from taking this value from session
                    if (ImportedRoutelist.Count > 0)
                    {
                        ViewBag.IsreturnLeg = false;
                        int Nstatus = 0;
                        Nstatus = Convert.ToInt32(ImportedRoutelist[0].RouteType);
                        if (Nstatus == 911001 || Nstatus == 911005)
                            ImportedRoutelist[0].NENRouteStatus = "Unplanned";
                        else if (Nstatus == 911002 || Nstatus == 911010)
                            ImportedRoutelist[0].NENRouteStatus = "Planned";
                        else if (Nstatus == 911003)
                            ImportedRoutelist[0].NENRouteStatus = "Planning error";
                        else if (Nstatus == 911004 || Nstatus == 911011)
                            ImportedRoutelist[0].NENRouteStatus = "Replanned";

                        ViewBag.NENrouteStatus = Nstatus;
                        ViewBag.MainRouteDesc = ImportedRoutelist[0].RouteDescription;
                        ViewBag.hNENRoute_Id = ImportedRoutelist[0].RouteID;
                        if (ImportedRoutelist.Count > 1)
                        {
                            Nstatus = Convert.ToInt32(ImportedRoutelist[1].RouteType);
                            ViewBag.IsreturnLeg = true;
                            ViewBag.ReturnRouteDesc = ImportedRoutelist[1].RouteDescription;
                            if (Nstatus == 911001 || Nstatus == 911005)
                                ImportedRoutelist[1].NENRouteStatus = "Unplanned";
                            else if (Nstatus == 911002 || Nstatus == 911010)
                                ImportedRoutelist[1].NENRouteStatus = "Planned";
                            else if (Nstatus == 911003)
                                ImportedRoutelist[1].NENRouteStatus = "Planning error";
                            else if (Nstatus == 911004 || Nstatus == 911011)
                                ImportedRoutelist[1].NENRouteStatus = "Replanned";
                        }
                    }
                }
                else if (Session["RouteFlag"].ToString() == "2" || Session["RouteFlag"].ToString() == "1")//RouteFlag 1 for VR1 Applictaion 2 for so app
                {
                    if (CONT_REF_NUM != null && VersionId != 0)
                    {
                        ImportedRoutelist = routesService.NotifVR1RouteList(apprevisionId, CONT_REF_NUM, VersionId, UserSchema.Portal);
                    }
                    else
                    {
                        long revisionId = apprevisionId;
                        ImportedRoutelist = routesService.GetSoAppRouteList(revisionId, SessionInfo.UserSchema);
                    }
                }
                else if (Session["RouteFlag"] != null && Session["RouteFlag"].ToString() == "3")//RouteFlag 1 for VR1 Applictaion 2 for so app and 3 for NOtification.
                {
                    ImportedRoutelist = routesService.NotifVR1RouteList(apprevisionId, CONT_REF_NUM, 0, UserSchema.Portal);
                    if (ImportedRoutelist.Count == 1 && AgreedSo == 1)
                    {
                        ViewBag.AgreedSO = 1;         //can not edit or delete route
                    }
                    else if (ImportedRoutelist.Count > 1 && AgreedSo == 1)
                    {
                        ViewBag.AgreedSO = 2;        //can delete route but not edit
                    }
                    for (int i = 0; i < ImportedRoutelist.Count; i++)
                    {
                        if (ImportedRoutelist[i].RouteName == "")
                            ImportedRoutelist.RemoveAt(i);
                    }
                }
                return PartialView(ImportedRoutelist);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Application/ListImportedRouteFromLibrary, Exception:" + ex);
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region VR1SupplementaryDetails
        public ActionResult VR1SupplementaryDetails(int apprevisionId = 0, int historic = 0 )
        {

            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Application/VR1SupplementaryDetails actionResult method started successfully"));
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action"); //this.ControllerContext.RouteData.Values["action"];
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");
                    return RedirectToAction("Login", "Account");
                }
                SupplimentaryInfo supplimentaryInfo = new SupplimentaryInfo();
                supplimentaryInfo = applicationService.VR1GetSupplementaryInfo(apprevisionId, SessionInfo.UserSchema, historic);

                return PartialView(supplimentaryInfo);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/VR1SupplementaryDetails, Exception: {0}", ex));
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
                MovementClassificationConfig mvClassConfig = compConfigObj.GetMovementClassificationConfig(vehicle.VehiclePurpose);
                foreach (var vehComp in vehicle.VehicleCompList)
                {
                    MovementClassificationConfig moveClassConfigObj = compConfigObj.GetListOfVehicleComponents((int)vehComp.ComponentTypeId);
                    STP.Domain.VehicleAndFleets.Component.VehicleComponent vehclCompObj = moveClassConfigObj.GetVehicleComponent((int)vehComp.ComponentTypeId, (int)vehComp.ComponentSubTypeId);
                    vehicle.VehicleNameList.Add(vehclCompObj.vehicleComponentSubType.ImageName);
                }
            }
        }
        #endregion

        #region GetExtractedString
        private string GetExtractedString(string stringToExtract)
        {
            string extractedString = "";
            string[] stringSeparators = { "##**##" };
            extractedString = stringToExtract.Split(stringSeparators, StringSplitOptions.None)[0];
            return extractedString;
        }
        #endregion
    }
}
