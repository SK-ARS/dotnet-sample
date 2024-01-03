using PagedList;
using STP.Common.Logger;
using STP.Domain.DocumentsAndContents;
using STP.Domain.RouteAssessment;
using STP.Domain.LoggingAndReporting;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.SecurityAndUsers;
using STP.ServiceAccess.Applications;
using STP.ServiceAccess.DocumentsAndContents;
using STP.ServiceAccess.LoggingAndReporting;
using STP.ServiceAccess.MovementsAndNotifications;
using STP.ServiceAccess.RoadNetwork;
using STP.ServiceAccess.RouteAssessment;
using STP.ServiceAccess.Routes;
using STP.ServiceAccess.Structures;
using STP.ServiceAccess.VehiclesAndFleets;
using STP.Web.WorkflowProvider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using static STP.Domain.Routes.RouteModel;
using STP.Domain.VehiclesAndFleets.Configuration;
using System.Text;
using STP.Common.General;
using STP.Domain.Custom;
using STP.Domain.Applications;
using System.Globalization;
using STP.Domain.MovementsAndNotifications.Notification.StringExtractor;
using STP.Common.Enums;
using System.Dynamic;
using STP.Domain.Workflow;
using STP.ServiceAccess.Workflows.ApplicationsNotifications;
using STP.Common.Constants;
using STP.ServiceAccess.CommunicationsInterface;
using STP.Domain.Workflow.Models;
using STP.Domain.Communications;
using static STP.Domain.VehiclesAndFleets.VehicleEnums;
using STP.Common.EncryptDecrypt;
using Newtonsoft.Json;
using STP.Domain.MovementsAndNotifications.Movements;

namespace STP.Web.Controllers
{

    public class NotificationController : Controller
    {
        private readonly IMovementsService movementsService;
        private readonly INotificationService notificationsService;
        private readonly IApplicationService applicationService;
        private readonly ILoggingService loggingService;
        private readonly IVehicleConfigService vehicleconfigService;
        private readonly IVehicleComponentService vehicleComponentService;
        private readonly IRoutesService routeService;
        private readonly IStructuresService structuresService;
        private readonly IRouteAssessmentService routeAssessmentService;
        private readonly IDocumentService documentService;
        private readonly ISORTApplicationService sortApplicationService;
        private readonly IConstraintService constraintService;
        private readonly IApplicationNotificationWorkflowService applicationNotificationWorkflowService;
        private readonly INotificationDocService notificationDocService;
        private readonly ICommunicationsInterfaceService communicationService;

        public NotificationController()
        {
        }
        public NotificationController(IMovementsService movementsService, INotificationService notificationsService, IApplicationService applicationService, ILoggingService loggingService, IVehicleConfigService vehicleconfigService, IVehicleComponentService vehicleComponentService, IRoutesService routeService, IRouteAssessmentService routeAssessmentService, IStructuresService structuresService, IDocumentService documentService, IConstraintService constraintService, ISORTApplicationService sortApplicationService, IApplicationNotificationWorkflowService applicationNotificationWorkflowService, INotificationDocService notificationDocService, ICommunicationsInterfaceService communicationService)
        {
            this.movementsService = movementsService;
            this.notificationsService = notificationsService;
            this.applicationService = applicationService;
            this.loggingService = loggingService;
            this.vehicleconfigService = vehicleconfigService;
            this.vehicleComponentService = vehicleComponentService;
            this.routeService = routeService;
            this.structuresService = structuresService;
            this.routeAssessmentService = routeAssessmentService;
            this.documentService = documentService;
            this.sortApplicationService = sortApplicationService;
            this.constraintService = constraintService;
            this.applicationNotificationWorkflowService = applicationNotificationWorkflowService;
            this.notificationDocService = notificationDocService;
            this.communicationService = communicationService;
        }

        /// <summary>
        /// Code to fetch the transmission status from db
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="TMF"></param> This will contain the items to be filtered
        /// <param name="Notification_Code"></param> ESDAL2 reference number for filter
        /// <returns></returns>

        public ActionResult TransmissionStatusList(int? page, int? pageSize, bool? PageView, TransmissionModelFilter TMF, string Notification_Code, bool showtrans = false, int SortStatus = 0, int historic = 0)
        {
            try
            {

                string Status = "";
                String[] statusItems = null;
                int statusItemsCount = 0;
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                if (SessionInfo.HelpdeskRedirect == "true")
                {
                    ViewBag.Helpdest_redirect = SessionInfo.HelpdeskRedirect;
                }
                if ((SortStatus == 0 || SortStatus == -1) && showtrans == false)
                {
                    showtrans = true; //for notification and renotifications
                    //making all true in case any of the conditions are false for notifications or renotifications
                    if (!(TMF.All) || !(TMF.Delivered) || !(TMF.Failed) || !(TMF.Pending) || !(TMF.Sent))
                    {
                        TMF.All = true;
                        TMF.Delivered = true;
                        TMF.Failed = true;
                        TMF.Pending = true;
                        TMF.Sent = true;
                    }
                }

                #region Session Check


                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                //Put appropriate page access checking code here
                //if (!PageAccess.GetPageAccess("60001"))
                //{
                //    return RedirectToAction("Error", "Home");
                //}

                #endregion


                if (!string.IsNullOrEmpty(Notification_Code))
                {
                    if (Notification_Code.Contains("~"))
                    {
                        Notification_Code = Notification_Code.Replace("~", "#");
                    }
                }



                if (pageSize == null)
                {
                    if (page == null && TMF.Delivered == false && TMF.Failed == false && TMF.Pending == false && TMF.Sent == false)
                    {
                        //first time page is loaded, "page" parameter will be null and model will also be null                        
                        TempData["Model"] = null;
                    }
                    else if (page == null && (TMF.Delivered == true || TMF.Failed == true || TMF.Pending == true || TMF.Sent == true))
                    {
                        //search button is clicked, page parameter is null and model is supplied
                        //so save these values in the temp data                
                        TempData["Model"] = TMF;
                    }
                    else if (page != null && TMF.Delivered == false && TMF.Failed == false && TMF.Pending == false && TMF.Sent == false)
                    {
                        //during page number click, the page parameter will not be null but the model will be null
                        //so put the tempdata values back into the model
                        if (TempData["Model"] != null)
                        {
                            TMF = (TransmissionModelFilter)TempData["Model"];
                        }
                    }
                }
                else
                {
                    if (TempData["Model"] != null)
                    {
                        TMF = (TransmissionModelFilter)TempData["Model"];
                    }
                }

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

                if (TMF.Delivered == true)
                {
                    Status = "310001,310009,310008,";
                }

                if (TMF.Failed == true)
                {
                    Status += "310002,";
                }

                if (TMF.Sent == true)
                {
                    Status += "310007,";
                }

                if (TMF.Pending == true)
                {
                    Status += "310003,";
                }

                if (PageView == true || TMF.All == true)
                {
                    Status = "310001,310009,310008,310002,310007,310003,310005,";
                    ViewBag.PageView = true;
                    TempData["PageView"] = true;
                }

                statusItems = Status.Split(',');
                statusItemsCount = statusItems.Length - 1;

                if (!string.IsNullOrEmpty(Notification_Code))
                {
                    TempData["RefNo"] = Notification_Code;
                }
                else
                {
                    Notification_Code = TempData["RefNo"].ToString();
                }
                GetTransmissionListParams getTransmissionList = new GetTransmissionListParams
                {
                    PageNum = pageNumber,
                    PageSize = (int)pageSize,
                    ESDALRefNo = Notification_Code,
                    Status = Status,
                    StatusItemCount = statusItemsCount,
                    IsHistoric = historic,
                    UserSchema = SessionInfo.UserSchema
                };
                List<TransmissionModel> transmissionList = notificationsService.GetTransmissionList(getTransmissionList);

                if (transmissionList != null && transmissionList.Count > 0)
                {
                    ViewBag.TotalCount = transmissionList[0].RecordCount;
                }
                else
                {
                    ViewBag.TotalCount = 0;
                }

                TempData.Keep("RefNo");
                TempData.Keep("Model");
                TempData.Keep("PageView");
                var transmissionlistPagedList = new StaticPagedList<TransmissionModel>(transmissionList, pageNumber, (int)pageSize, (int)ViewBag.TotalCount);
                ViewBag.showtrans = showtrans;
                ViewBag.SortStatus = SortStatus;
                return PartialView(transmissionlistPagedList);
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Notification/TransmissionStatusList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        /// Collaboration status list
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">size of page</param>
        /// <param name="RefNo">Status code</param>
        /// <returns>List of collaboration status list</returns>
        public ActionResult CollaborationStatusList(int? page, int? pageSize, string RefNo = "", bool SORTCollab = false, bool NotifCollab = false)
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

                #region Page access
                //if (!(PageAccess.GetPageAccess("0") || PageAccess.GetPageAccess("1") || PageAccess.GetPageAccess("2") || PageAccess.GetPageAccess("3")))
                //{
                //    return RedirectToAction("Error", "Home");
                //}
                #endregion

                #region Paging Part
                int pageNumber = (page ?? 1);
                int tempPageCount = 0;
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

                RefNo = RefNo.Replace("~", "#");

                if (!string.IsNullOrEmpty(RefNo))
                {
                    TempData["StatusCode"] = RefNo;

                }
                else
                {
                    RefNo = Convert.ToString(TempData["StatusCode"]);
                }
                TempData.Keep("StatusCode");
                List<NotificationStatusModel> notificationStatuslist;
                notificationStatuslist = notificationsService.GetNotificationStatusList(pageNumber, (int)pageSize, RefNo, SessionInfo.UserSchema);

                if (notificationStatuslist.Count > 0)
                {
                    ViewBag.TotalCount = Convert.ToInt32(notificationStatuslist[0].TotalRecordCount);
                }
                else
                {
                    ViewBag.TotalCount = 0;
                }

                var notificationStatuslistFinal = new StaticPagedList<NotificationStatusModel>(notificationStatuslist, pageNumber, (int)pageSize, ViewBag.TotalCount);
                ViewBag.SORTCollab = SORTCollab;
                ViewBag.NotifCollab = NotifCollab;
                return PartialView(notificationStatuslistFinal);
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Notification/CollaborationStatusList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        /// Create internal collaboration
        /// </summary>
        /// <returns>Return data for internal collaboraton</returns>
        public ActionResult CreateInternalCollaboration(bool SORTSetCollab = false)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Notification/CreateInternalCollaboration actionResult method started successfully"));
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
                NotificationStatusModel notificationStatusModel = new NotificationStatusModel();

                notificationStatusModel.DOCUMENT_ID = Convert.ToInt32(TempData["DOCUMENT_ID"]);
                notificationStatusModel.COLLABORATION_NO = Convert.ToInt32(TempData["COLLABORATION_NO"]);

                //
                notificationStatusModel = notificationsService.GetInternalCollaboration(notificationStatusModel, SessionInfo.UserSchema);

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Notification/CreateInternalCollaboration actionResult method ented successfully"));
                TempData.Keep("DOCUMENT_ID");
                TempData.Keep("COLLABORATION_NO");
                TempData.Keep("StatusCode");
                ViewBag.SORTSetCollab = SORTSetCollab;
                notificationStatusModel.DOCUMENT_ID = Convert.ToInt32(TempData["DOCUMENT_ID"]);
                notificationStatusModel.COLLABORATION_NO = Convert.ToInt32(TempData["COLLABORATION_NO"]);

                return PartialView("CreateInternalCollaboration", notificationStatusModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Notification/CreateInternalCollaboration, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        /// Manage collaboration internal status
        /// </summary>
        /// <param name="movement">MovementModel object</param>
        /// <returns>Return true or false</returns>
        [HttpPost]
        public JsonResult ManageCollaborationInternal(NotificationStatusModel notificationStatu)
        {
            try
            {
                int projectId = 0;
                int versionNo = 0;
                int revisionNo = 0;
                #region Session Check

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }

                #endregion

                TempData.Keep("StatusCode");
                #region Page access check
                #endregion
                bool result = notificationsService.ManageCollaborationInternal(notificationStatu, SessionInfo.UserSchema);

                if (result == false)
                {
                    MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                    movactiontype.MovementActionType = MovementnActionType.int_collab_details_amended;

                    movactiontype.CollaborationStatus = notificationStatu.STATUS;
                    movactiontype.ESDALRef = TempData["StatusCode"] != null ? Convert.ToString(TempData["StatusCode"]) : "";
                    movactiontype.ContactName = notificationStatu.FIRST_NAME + " " + notificationStatu.SUR_NAME;

                    string ErrMsg = string.Empty;

                    string MovementDescription = MovementActions.GetMovementActionString(SessionInfo, movactiontype, out ErrMsg);
                    long res = loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, projectId,revisionNo,versionNo, SessionInfo.UserSchema);
                }

                return Json(result);
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/UpdateMovementStatus, Exception: {0}", ex));
                throw ex;
            }
        }


        /// <summary>
        /// Display collaboration history list
        /// </summary>
        /// <param name="page">Page </param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="DocumentId">Document id</param>
        /// <returns>Return collaboration history list</returns>
        public ActionResult DisplayCollaborationHistoryList(int? page, int? pageSize, int DocumentId, bool SORTCollab = false)
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
                ViewBag.DocumentId = DocumentId;
                TempData.Keep("StatusCode");
                List<CollaborationModel> externalCollaborationList = notificationsService.GetExternalCollaboration(pageNumber, (int)pageSize, DocumentId, SessionInfo.UserSchema);
                if (externalCollaborationList.Count > 0)
                    ViewBag.TotalCount = Convert.ToInt32(externalCollaborationList[0].TotalRecordCount);
                else
                    ViewBag.TotalCount = 0;

                var CollaborationListPaged = new StaticPagedList<CollaborationModel>(externalCollaborationList, pageNumber, (int)pageSize, ViewBag.TotalCount);
                ViewBag.SORTCollab = SORTCollab;
                return PartialView("DisplayCollaborationHistoryList", CollaborationListPaged);
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Notification/DisplayCollaborationHistoryList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        /// Store  Notifiaction status data
        /// </summary>
        /// <param name="notificationStatusModel">object of NotificationStatusModel</param>
        /// <returns>Store data</returns>
        [HttpPost]
        public JsonResult StoreInternalCollaboration(NotificationStatusModel notificationStatusModel)
        {
            TempData["DOCUMENT_ID"] = notificationStatusModel.DOCUMENT_ID;
            TempData["COLLABORATION_NO"] = notificationStatusModel.COLLABORATION_NO;

            TempData.Keep("DOCUMENT_ID");
            TempData.Keep("COLLABORATION_NO");
            TempData.Keep("StatusCode");

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #region CollabHistoryPopList
        public ActionResult CollabHistoryPopList(int DocumentId = 0, int randomNumber = 0, bool SORTCollab = false, bool SORTSetCollab = false)
        {
            try
            {
                string messg = "SORTApplications/CollabHistoryPopList?DocumentId=" + DocumentId + "randomNumber=" + randomNumber + "SORTCollab=" + SORTCollab + "SORTSetCollab=" + SORTSetCollab;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                // bool result = false;

                //result = SORTApplicationProvider.Instance.SubmitSORTSoApplication(apprevisionId);
                bool flag = sortApplicationService.UpdateCollaborationView(DocumentId);
                ViewBag.DocumentId = DocumentId;
                ViewBag.SORTCollab = SORTCollab;
                ViewBag.SORTSetCollab = SORTSetCollab;
                if (SORTCollab)
                {
                    if (DocumentId == 1011210465)
                    {
                        ViewBag.LibraryNotes = true;
                    }
                }


                return PartialView("CollabHistoryPopList");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/CollabHistoryPopList, Exception: {0}", ex));
                return Json(0);

            }
        }
        #endregion

        public ActionResult SearchTransmissionStatusPanel()
        {

            #region Session Check


            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            //Put appropriate page access checking code here
            //if (!PageAccess.GetPageAccess("60001"))
            //{
            //    return RedirectToAction("Error", "Home");
            //}

            #endregion

            TransmissionModelFilter search = new TransmissionModelFilter();

            if (TempData["Model"] != null)
            {
                search = (TransmissionModelFilter)TempData["Model"];
            }
            else
            {
                //To show all the checkbox as checked when the page is first opened.
                if (TempData["PageView"] != null)
                {
                    search.Delivered = true;
                    search.Failed = true;
                    search.Pending = true;
                    search.Sent = true;
                    search.All = true;
                }
            }
            TempData.Keep("Model");
            return PartialView(search);
        }

        public ActionResult SetNotificationGeneralDetails(SetNotificationGeneralDetailsCntrlModel notificationGeneralDetailsCntrlModel)
        {
            NotificationGeneralDetails objDetailNotifGeneralDetails;
            Session["AppFlag"] = "Notif";
            Session["IsNotif"] = true;
            objDetailNotifGeneralDetails = notificationsService.GetNotificationGeneralDetail(notificationGeneralDetailsCntrlModel.notificationId, 0);
            var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
            PlanMvmntPayLoad planMvmntPayLoad = applicationNotificationManagement.GetPlanMvmtPayload();
            if (applicationNotificationManagement.IsThisMovementExist(notificationGeneralDetailsCntrlModel.notificationId, 0, out string workflowKey))
            {
                if (notificationGeneralDetailsCntrlModel.workflowProcess.Length > 0
                    && WorkflowTaskFinder.FindNextTask(notificationGeneralDetailsCntrlModel.workflowProcess, WorkflowActivityTypes.An_Activity_FillInNotificationAttributesAndLoadDetails, out dynamic workflowPayload, false) != string.Empty)
                {

                    dynamic dataPayload = new ExpandoObject();
                    dataPayload.notificationId = notificationGeneralDetailsCntrlModel.notificationId;
                    dataPayload.taskId = 6;
                    planMvmntPayLoad.ActionCompleted = planMvmntPayLoad.ActionCompleted <= dataPayload.taskId ? dataPayload.taskId : planMvmntPayLoad.ActionCompleted;
                    dataPayload.PlanMvmntPayLoad = planMvmntPayLoad;
                    dataPayload.workflowActivityLog = applicationNotificationManagement.SetWorkflowLog(WorkflowActivityTypes.An_Activity_FillInNotificationAttributesAndLoadDetails.ToString());
                    WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                    {
                        data = dataPayload,
                        workflowData = workflowPayload
                    };
                    applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);
                }
                ViewBag.PlanMvmntPayLoad = planMvmntPayLoad;
            }
            return PartialView("_NotificationGeneralDetails", objDetailNotifGeneralDetails);
        }


        [HttpPost]
        public JsonResult UpdateTermsAndConditions(bool status)
        {
            try
            {
                var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                PlanMvmntPayLoad planMovePayload = applicationNotificationManagement.GetPlanMvmtPayload();
                if (applicationNotificationManagement.IsThisMovementExist(planMovePayload.NotificationId, planMovePayload.RevisionId, out string workflowKey)
                        && WorkflowTaskFinder.FindNextTask("HaulierApplication", WorkflowActivityTypes.An_Activity_FillInNotificationAttributesAndLoadDetails, out dynamic workflowPayload) != string.Empty)
                {
                    planMovePayload.IsTermsAndConditionsAccepted = status;
                    dynamic dataPayload = new ExpandoObject();
                    dataPayload.PlanMvmntPayLoad = planMovePayload;
                    WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                    {
                        data = dataPayload,
                        workflowData = workflowPayload
                    };
                    applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/UpdateTermsAndConditions, Exception: {0}", ex));
                return Json(new { success = false, errorMessage = ex.Message.ToString() });
            }
        }

        public ActionResult SaveNotificationGeneralDetails(NotificationGeneralDetails objNotifDetail, long notificationId,bool backflag=false)
        {
            bool isSuccess = false;
            try
            {
                var dateFromReceived = objNotifDetail.FromDateTime.Date;
                var timeFromReceived = objNotifDetail.FromDate.TimeOfDay;
                objNotifDetail.FromDateTime = dateFromReceived + timeFromReceived;
                var dateToReceived = objNotifDetail.ToDateTime.Date;
                var timeToReceived = objNotifDetail.ToDate.TimeOfDay;
                objNotifDetail.ToDateTime = dateToReceived + timeToReceived;
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                objNotifDetail.OrganisationId = (int)SessionInfo.OrganisationId;
                objNotifDetail.UserId = Convert.ToInt32(SessionInfo.UserId);
                objNotifDetail.NotificationId = notificationId;
                var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                var mvmntPayLoad = applicationNotificationManagement.GetPlanMvmtPayload();
                objNotifDetail.RequiresVR1 = mvmntPayLoad.RequireVr1 ? 1 : 0;
                List<long> dispensationList = new List<long>();
                foreach (var item in mvmntPayLoad.DipesnsationList)
                {
                    dispensationList.Add(item.DispensationId);
                }
                objNotifDetail.DispensationList = dispensationList;

                int result = notificationsService.UpdateNotification(objNotifDetail);
                if (backflag == false)
                {
                    if (result > 0)
                    {
                        isSuccess = true;
                        if (applicationNotificationManagement.IsThisMovementExist(notificationId, 0, out string workflowKey))
                        {
                            if (WorkflowTaskFinder.FindNextTask("HaulierApplication", WorkflowActivityTypes.An_Activity_AcceptTermsAndConditions, out dynamic workflowPayload) != string.Empty)
                            {
                                mvmntPayLoad.ImminentMessage = objNotifDetail.ImminentMessage;
                                dynamic dataPayload = new ExpandoObject();
                                dataPayload.PlanMvmntPayLoad = mvmntPayLoad;
                                dataPayload.workflowActivityLog = applicationNotificationManagement.SetWorkflowLog(WorkflowActivityTypes.An_Activity_AcceptTermsAndConditions.ToString());
                                WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                                {
                                    data = dataPayload,
                                    workflowData = workflowPayload
                                };
                                new ApplicationNotificationManagement(applicationNotificationWorkflowService).ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);
                            }
                        }
                    }
                }
                else
                {
                    if (WorkflowTaskFinder.FindNextTask("HaulierApplication", WorkflowActivityTypes.An_Activity_AcceptTermsAndConditions, out dynamic workflowPayload) != string.Empty)
                    {
                        dynamic dataPayload = new ExpandoObject();
                        mvmntPayLoad.ImminentMessage = objNotifDetail.ImminentMessage;
                        dataPayload.PlanMvmntPayLoad = mvmntPayLoad;
                        WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                        {
                            data = dataPayload,
                            workflowData = workflowPayload
                        };
                        applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel);
                    }
                }
                return Json(new { result = isSuccess });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Notification/SaveNotificationGeneralDetails, Exception: {0}", ex));
                return Json(new { result = isSuccess });
            }
        }

        #region JsonResult ShowImminentMovement(int vehicleclass, string moveStartDate)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vehicleclass"></param>
        /// <param name="moveStartDate"></param>
        /// <returns></returns>
        public JsonResult ShowImminentMovement(string moveStartDate = null, string contentRefNo = "", long notificationId = 0, int vehicleClass = 0)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            if (string.IsNullOrWhiteSpace(moveStartDate))
            {
                PlanMovementType planMovementType = applicationService.GetNotificationDetails(notificationId, SessionInfo.UserSchema);
                if(planMovementType!=null)
                    moveStartDate= planMovementType.MovementStart.ToString("dd-MM-yyyy");
            }
            if (vehicleClass == 0)
            {
                var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                var payloadMovement = applicationNotificationManagement.GetPlanMvmtPayload();
                vehicleClass = payloadMovement.VehicleClass;
            }
            string[] dateStr = new string[2];
            dateStr = moveStartDate.Split(' ');
            moveStartDate = dateStr[0].ToString();
            int imminentStatus;
            List<int> CountryIDList = new List<int>();
            List<int> WorkingDaysList = new List<int>();
            int WorkingDays;
            string strContryID = "";
            try
            {
                int result = 0;

                List<RoutePartDetails> routePartDet = new List<RoutePartDetails>();
                GetImminentChkDetailsDomain objImminent = new GetImminentChkDetailsDomain();
                //fetching route part details based on content reference number or analysis id
                if (contentRefNo != "")
                {
                    routePartDet = routeAssessmentService.GetRouteDetailForAnalysis(0, 0, contentRefNo, 0, SessionInfo.UserSchema); //Function to retrieve based on content reference number

                    foreach (RoutePartDetails routePart in routePartDet)
                    {
                        var countryIDByRouteId = routeAssessmentService.GetCountryId((int)routePart.RouteId);
                        if (countryIDByRouteId != null)
                            CountryIDList.AddRange(countryIDByRouteId);
                    }
                    CountryIDList = CountryIDList.Distinct().ToList();
                }
                if (CountryIDList.Count == 0)
                {
                    result = result + notificationsService.ShowImminentMovement(moveStartDate, null,0, vehicleClass);
                    WorkingDays = result;
                }
                else
                {
                    CountryIDList = CountryIDList.Where(x => x != 0).ToList();
                    var cnt = CountryIDList.Count();
                    strContryID = string.Join(",",CountryIDList);
                    WorkingDaysList.Add(notificationsService.ShowImminentMovement(moveStartDate, strContryID, cnt, vehicleClass));
                    WorkingDays = WorkingDaysList.Min();
                }

                imminentStatus = GetImminent(WorkingDays);

                if (strContryID == "" || strContryID == null)
                {
                    strContryID = "0";
                }
                var jSonVar = new { check = result, imminentStatus = imminentStatus, strContryID = strContryID };

                return Json(new { result = jSonVar });
            }
            catch (Exception ex)
            {
                var jSonVar = new { check = 0 };

                return Json(new { result = jSonVar });

            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workingDays"></param>
        /// <returns>
        //1 - Imminent movement.
        //2 - Imminent movement for police.
        //3 - Imminent movement for SOA.
        //4 - Imminent movement for SOA and police.
        //5 - No imminent movement.</returns>
        private int GetImminent(int workingDays)
        {
            int imminent = 5;
            var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
            var payloadMovement = applicationNotificationManagement.GetPlanMvmtPayload();

            int SOANoticePeriod = 0;
            int PoliceNoticePeriod = 0;
            if(payloadMovement!=null && payloadMovement.MvmntType != null)
            {
                SOANoticePeriod = payloadMovement.MvmntType.SOANoticePeriod;
                PoliceNoticePeriod = payloadMovement.MvmntType.PoliceNoticePeriod;
            }
            else if(new SessionData().E4_AN_MovemenTypeClass != null)
            {
                SOANoticePeriod = new SessionData().E4_AN_MovemenTypeClass.SOANoticePeriod;
                PoliceNoticePeriod = new SessionData().E4_AN_MovemenTypeClass.PoliceNoticePeriod;
            }
            if (PoliceNoticePeriod == 0 && SOANoticePeriod == 0)
                imminent = 1;
            else
            {
                workingDays = workingDays < 0 ? 0 : workingDays;
                if (workingDays < SOANoticePeriod)
                    imminent = 3;
                if (workingDays < PoliceNoticePeriod)
                {
                    imminent = imminent != 5 ? 4 : 2;
                }
            }
            return imminent;
        }

        public ActionResult NotificationRouteAssessment(string workflowProcess = "")
        {
            if (workflowProcess.Length > 0
                && WorkflowTaskFinder.FindNextTask(workflowProcess, WorkflowActivityTypes.An_Activity_AdditionalAffectedPartiesManualEntry, out dynamic workflowPayload) != string.Empty)
            {
                var currentActivity = applicationNotificationWorkflowService.GetCurrentTask(new SessionData().Wf_An_ApplicationWorkflowId);

                var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                dynamic dataPayload = new ExpandoObject();
                dataPayload.taskId = 5;
                PlanMvmntPayLoad planMvmntPayLoad = applicationNotificationManagement.GetPlanMvmtPayload();
                planMvmntPayLoad.IsRouteAssessmentDone = true;
                planMvmntPayLoad.ActionCompleted = planMvmntPayLoad.ActionCompleted <= dataPayload.taskId ? dataPayload.taskId : planMvmntPayLoad.ActionCompleted;
                dataPayload.PlanMvmntPayLoad = planMvmntPayLoad;
                dataPayload.workflowActivityLog = applicationNotificationManagement.SetWorkflowLog(WorkflowActivityTypes.An_Activity_AdditionalAffectedPartiesManualEntry.ToString());
                WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                {
                    data = dataPayload,
                    workflowData = workflowPayload
                };
                ViewBag.payloadData = planMvmntPayLoad;

                if (currentActivity != "Activity_PlanRouteOnMap")
                {
                    applicationNotificationManagement.ProcessWorkflowActivity(workflowProcess, workflowActivityPostModel);
                }
                applicationNotificationManagement.ProcessWorkflowActivity(workflowProcess, workflowActivityPostModel);
            }
            return PartialView("_NotificationRouteAssessment");
        }
        public ActionResult NotificationTermsAndConditions(bool IsTermsAndConditionsAccepted)
        {
            ViewBag.IsTermsAndConditionsAccepted = IsTermsAndConditionsAccepted;
            return PartialView("_NotificationTermsAndConditions");
        }

        #region AffectedParties_SimpleNotif
        public ActionResult NotifAffectedParties(int NotificationID = 0, string ContentRefNo = "", int AnalysisId = 0, string VSOType = "soapolice", int AgreedSOAffParty = 0)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , NotificationController/AffectedParties_SimpleNotif ,Affected parties generated: AnalysisId{1}", Session.SessionID, AnalysisId));
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            int orgId = (int)SessionInfo.OrganisationId;

            //Saving logged in users Name in viewbag for storing as a hidden variable
            ViewBag.FullName = SessionInfo.FirstName + " " + SessionInfo.LastName;
            ViewBag.OrgName = SessionInfo.OrganisationName;

            RouteAssessmentModel objRouteAssessmentModel;

            ViewBag.NotificationID = NotificationID;
            ViewBag.ContentRefNo = ContentRefNo;
            ViewBag.AnalysisId = AnalysisId;
            ViewBag.VSOType = VSOType;

            objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(AnalysisId, 7, SessionInfo.UserSchema);

            if (AgreedSOAffParty == 0)
            {

                //the below condition and code portion generates affected parties in every case it retains the user added manually added parties in case there is any
                if (objRouteAssessmentModel.AffectedParties != null)
                {
                    //sample object 1
                    Domain.RouteAssessment.XmlAffectedParties.AffectedPartiesStructure existingAfftdPartyObj = null;
                    //sample object 2
                    Domain.RouteAssessment.XmlAffectedParties.AffectedPartiesStructure latestAfftdPartyObj = null;

                    //fetching affected party list into a string 
                    string existingAffectedParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.AffectedParties));

                    //Deserializing it into object and storing it 
                    existingAfftdPartyObj = StringExtraction.xmlAffectedPartyDeserializer(existingAffectedParties);

                    //generating new list of affected parties 
                    routeAssessmentService.updateRouteAssessment(ContentRefNo, NotificationID, 0, 0, orgId, AnalysisId, 7, SessionInfo.UserSchema, VSOType);


                    //checking whether there is any manually added affected party or not in the previous list if yes ?
                    if (existingAfftdPartyObj.ManualAffectedParties.Count() > 0)
                    {
                        //once again the affected party is list is fetched this time its the newly generated
                        objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(AnalysisId, 7, SessionInfo.UserSchema);

                        //fetching latest affected party list into a string 
                        string latestAffectedParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.AffectedParties));

                        //deserializing the list
                        latestAfftdPartyObj = StringExtraction.xmlAffectedPartyDeserializer(latestAffectedParties);

                        //copying the old manually added affected party list into the newly generated affected party list
                        latestAfftdPartyObj.ManualAffectedParties = existingAfftdPartyObj.ManualAffectedParties;

                        //serializing the affected party object into XML string 
                        latestAffectedParties = StringExtraction.XmlAffectedPartySerializer(latestAfftdPartyObj);

                        //converting the XML string into a blob field
                        objRouteAssessmentModel.AffectedParties = StringExtraction.ZipAndBlob(latestAffectedParties);

                        //updating the analysed route's table with the new affected party list
                        long tempVar = routeAssessmentService.updateRouteAssessment(objRouteAssessmentModel, AnalysisId, SessionInfo.UserSchema);

                    }
                    //fetching inbound document from xml for updating the dispensation from some matching to in use list
                    // need to add service call
                    //  XMLModifier.ModifyingDispStatusToInUse(NotificationID, existingAfftdPartyObj, AnalysisId, SessionInfo.UserSchema);
                }
                else
                    routeAssessmentService.updateRouteAssessment(ContentRefNo, NotificationID, 0, 0, orgId, AnalysisId, 7, SessionInfo.UserSchema, VSOType);

                #region code part to compare two affected parties related xml and updated the latest xml W.R.T changes.

                AnalysedRoute routeObj;

                routeObj = routeAssessmentService.fetchPreviousAffectedList(AnalysisId, 7, SessionInfo.UserSchema);

                //condition's to update the newly generated affected parties list with the previously generated one's for a movement
                if (routeObj != null && routeObj.PreviousAnalysisId > 0)
                {

                    RouteAssessmentModel routeObjNew = new RouteAssessmentModel();

                    RouteAssessmentModel routeObjOld = new RouteAssessmentModel();

                    Domain.RouteAssessment.XmlAffectedParties.AffectedPartiesStructure oldAffectedParties = StringExtraction.xmlAffectedPartyDeserializer(routeObj.RouteAnalysisXml);

                    routeObjNew.AffectedParties = routeAssessmentService.GetDriveInstructionsinfo(AnalysisId, 7, SessionInfo.UserSchema).AffectedParties;

                    string newXml = Encoding.UTF8.GetString(XsltTransformer.Trafo(routeObjNew.AffectedParties));

                    Domain.RouteAssessment.XmlAffectedParties.AffectedPartiesStructure newAffectedParties = StringExtraction.xmlAffectedPartyDeserializer(newXml);

                    newAffectedParties = StringExtraction.checkAffectedPartiesDiff(oldAffectedParties, newAffectedParties);

                    //updating the dispensation from some matching to in use by comparing existing affected party list and then using them
                    newAffectedParties = StringExtraction.ModifyingDispStatusToInUse(oldAffectedParties, newAffectedParties);

                    //sorting the generated affected party list based on organisation name
                    if (newAffectedParties.GeneratedAffectedParties.Count != 0 || newAffectedParties.GeneratedAffectedParties != null)
                    {
                        var affectedPartylist = newAffectedParties.GeneratedAffectedParties;

                        newAffectedParties.GeneratedAffectedParties = affectedPartylist.OrderBy(t => t.Contact.Contact.simpleContactRef.OrganisationName).ToList();
                    }

                    string result = StringExtraction.XmlAffectedPartySerializer(newAffectedParties);

                    routeObjNew.AffectedParties = StringExtraction.ZipAndBlob(result);

                    long tmpResult = routeAssessmentService.updateRouteAssessment(routeObjNew, AnalysisId, SessionInfo.UserSchema);
                }

                #endregion
            }
            return PartialView("_NotifAffectedParties", "Notification");
        }

        public ActionResult GetAffectedParties(int NotificationID = 0, string ContentRefNo = "", int AnalysisId = 0, string VSOType = "soapolice", int AgreedSOAffParty = 0)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , NotificationController/AffectedParties_SimpleNotif ,Affected parties generated: AnalysisId{1}", Session.SessionID, AnalysisId));
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

            //Saving logged in users Name in viewbag for storing as a hidden variable
            ViewBag.FullName = SessionInfo.FirstName + " " + SessionInfo.LastName;
            ViewBag.OrgName = SessionInfo.OrganisationName;

            ViewBag.NotificationID = NotificationID;
            ViewBag.ContentRefNo = ContentRefNo;
            ViewBag.AnalysisId = AnalysisId;
            ViewBag.VSOType = VSOType;

            return PartialView("_NotifAffectedParties", "Notification");
        }
        #endregion
        public ActionResult NotifyApplication(long versionId = 0, int MaxPieces = 0, string ApplNotes = "", string MoveStartDate = "", string MoveEndDate = "", int ApplrevisionId = 0, int isVR1 = 0, int versionStatus = 0)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            NotificationGeneralDetails objNotifyVR1;
            objNotifyVR1 = notificationsService.NotifyApplication(versionId);

            #region movement actions for this action method
            MovementActionIdentifiers movactiontype = new MovementActionIdentifiers
            {
                MovementActionType = MovementnActionType.movement_notified,
                RevisionId = ApplrevisionId,
                NotificationID = objNotifyVR1.NotificationId,
                UserName = SessionInfo.UserName
            };
            string sysEventDescp;
            string ErrMsg;
            sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
            loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
            #endregion

            var activityName = WorkflowTaskFinder.GenerateWorkflowActivityName(WorkflowActivityTypes.An_Activity_VehicleDetails);
            var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
            var isAgreedApplication = false;
            if (versionStatus != 305002 && versionStatus != 305003 && versionStatus > 0 && isVR1 != 1)
            {
                isAgreedApplication = true;
            }

            PlanMvmntPayLoad planMvmntPayLoad = new PlanMvmntPayLoad
            {
                NextActivity = activityName,
                OrgId = SessionInfo.organisationId,
                OrgName = SessionInfo.OrganisationName,
                MovementKey = objNotifyVR1.NotificationId,
                NotificationId = objNotifyVR1.NotificationId,
                IsNotif = true,
                VehicleMoveId = objNotifyVR1.MovementId,
                VersionId = objNotifyVR1.VersionId,
                IsNotifyApp = true,
                ContenRefNo = objNotifyVR1.ContentReferenceNo,
                MovementType = (int)MovementType.notification,
                PrevMovType = isVR1 == 1 ? (int)MovementType.vr_1 : (int)MovementType.special_order,
                VehicleClass = objNotifyVR1.VehicleCode,
                IsAgreedNotified = isAgreedApplication,
                IsApproveNotified = isVR1 == 1 && versionStatus == (int)VersionStatus.approved,
                AnalysisId = objNotifyVR1.AnalysisId,
                IsRouteVehicleAssigned = true,
                PreviousContactName = objNotifyVR1.PreviousContactName
            };
            if (!applicationNotificationManagement.IsThisMovementExist(objNotifyVR1.NotificationId, 0, out string workflowKey))
            {
                applicationNotificationManagement.StartWorkflow(planMvmntPayLoad, 2);
                new SessionData().E4_AN_PlanMovement = planMvmntPayLoad;
            }
            return (Json(new { result = objNotifyVR1 }));
        }
        #region SendReplyMailToHaulier
        public bool SendReplyMailToHaulier(string esdalReference, int notificationId, string haulierEmailAddress)
        {

            bool ReplyMail = false;
            byte[] affectedParties = null;
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Send auto response mail method initiated for EsdalRef : {0}", esdalReference));

                //get affected parties list
                Domain.RouteAssessment.XmlAffectedParties.AffectedPartiesStructure existingAfftdPartyObj = null;

                affectedParties = notificationsService.GetAffectedParties(notificationId, UserSchema.Portal);
                if (affectedParties != null)
                {
                    //fetching affected party list into a string 
                    string existingAffectedParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(affectedParties));
                    //Deserializing it into object and storing it 
                    existingAfftdPartyObj = StringExtraction.xmlAffectedPartyDeserializer(existingAffectedParties);
                }
                //get the contact info
                if (existingAfftdPartyObj != null)
                {
                    for (int i = 0; i < existingAfftdPartyObj.GeneratedAffectedParties.Count; i++)
                    {
                        int organisationId = existingAfftdPartyObj.GeneratedAffectedParties[i].Contact.Contact.simpleContactRef.OrganisationId;
                        string organisationName = existingAfftdPartyObj.GeneratedAffectedParties[i].Contact.Contact.simpleContactRef.OrganisationName;

                        //get response mail details
                        MailResponse responseMailDetails = notificationsService.GetResponseMailDetails(organisationId);

                        //send auto response mail only if its enabled on SOA/Poice portal
                        if (responseMailDetails.EmailID != null && responseMailDetails.EmailID != "")
                        {
                            ReplyMail = communicationService.SendAutoResponseMail(esdalReference, haulierEmailAddress, organisationName, responseMailDetails);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SendReplyMailToHaulier exception : {0} ", ex.Message));
                ReplyMail = false;
            }
            return ReplyMail;
        }
        #endregion
        #region GenerateNotification

        public ActionResult GenerateNotification(int NotificationId = 0, int GenerateCode = 0, int Detail = 0, string ImminentMovestatus = "No imminent movement", long ProjectId = 0, string workflowProcess = "")
        {
            try
            {
                int versionNo = 0;
                int revisionNo = 0;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}], NotificationController/GenerateNotification ,Generate Notification Started, NotificationId: {1}", Session.SessionID, NotificationId));
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                int organisationId = (int)SessionInfo.OrganisationId;
                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                string ErrMsg = string.Empty;
                string sysEventDescp = string.Empty;
                string Esdalref = string.Empty;
                if (GenerateCode == 1)
                {
                    int iNotifVer = notificationsService.CheckNotificationVersion(NotificationId);
                    //check if notification submitted already/
                    string notifcode = "";
                    if (iNotifVer == 1)
                        notifcode = notificationsService.CheckIfNotificationSubmitted(NotificationId);
                    if (notifcode == "0" || notifcode == "")
                    {
                        notificationsService.GenerateNotificationCode(organisationId, NotificationId, Detail);

                        // Code to fetch ContactID based upon UserID
                        int contactID = Convert.ToInt32(SessionInfo.ContactId);
                        RouteAssessmentModel objRouteAssessmentModel = constraintService.GetAffectedStructuresConstraints(NotificationId, null, null, null, SessionInfo.UserSchema, 0);//Added last parameter by default 0 for ESDAL2 notification else will work for NEN

                        //save affected constraint and affected structure notifications
                        SaveAffectedSrtuctConstrNotif(objRouteAssessmentModel, NotificationId);

                        Dictionary<int, int> icaStatusDictionary = null;
                        CommonNotifMethods commonNotif = new CommonNotifMethods();
                        if (objRouteAssessmentModel.AffectedStructure != null)
                        {
                            string xmlaffectedStructures = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.AffectedStructure));

                            icaStatusDictionary = commonNotif.GetNotifICAstatus(xmlaffectedStructures);
                        }
                        NotifDistibutionParams distibution = new NotifDistibutionParams
                        {
                            NotificationId = NotificationId,
                            ProjectId = ProjectId,
                            ContactId = contactID,
                            ICAStatusDictionary = icaStatusDictionary,
                            ImminentMovestatus = ImminentMovestatus,
                            SessionInfo = SessionInfo,
                            ContactModel = null,
                            IsNen = false,
                            IsRenotify = iNotifVer > 1
                        };
                        //Sending mail, fax or saving inbox only
                        documentService.DistributeNotification(distibution);
                    }
                    string notifcode1 = notificationsService.CheckIfNotificationSubmitted(NotificationId);
                    Esdalref = notifcode1;
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}], NotificationController/GenerateNotification ,Generate Notification Completed Successfully, NotificationId: {1}", Session.SessionID, NotificationId));
                    try
                    {
                        //send auto response mail to hauliers from affected parties
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("SendReplyMailToHaulier() started for, NotificationId: {0}", NotificationId));
                        SendReplyMailToHaulier(notifcode1, NotificationId, SessionInfo.Email);
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("SendReplyMailToHaulier() end for, NotificationId: {0}", NotificationId));
                    }
                    catch (Exception ex)
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Exception while executing SendReplyMailToHaulier() for, NotificationId: {0}, Exception: {1}", NotificationId, ex.Message));
                    }

                    #region System Event Log - Haulier_submitted_notification
                    movactiontype.UserName = SessionInfo.UserName;
                    movactiontype.NotificationID = NotificationId;
                    movactiontype.NotificationCode = notifcode1;
                    movactiontype.ProjectId = ProjectId;
                    movactiontype.SystemEventType = SysEventType.Haulier_submitted_notification;

                    sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);


                    #endregion
                }
                var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                var payload = applicationNotificationManagement.GetPlanMvmtPayload();
                vehicleconfigService.DeleteTempMovementVehicle(payload.VehicleMoveId, SessionInfo.UserSchema);
                if (applicationNotificationManagement.IsThisMovementExist(NotificationId, 0, out string workflowKey)
                    && workflowProcess.Length > 0
                    && WorkflowTaskFinder.FindNextTask(workflowProcess, WorkflowActivityTypes.TheEnd, out dynamic workflowPayload, true) != string.Empty)
                {
                    dynamic dataPayload = new ExpandoObject();
                    dataPayload.workflowActivityLog = applicationNotificationManagement.SetWorkflowLog(WorkflowActivityTypes.TheEnd.ToString());
                    WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                    {
                        data = dataPayload,
                        workflowData = workflowPayload
                    };
                    applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel, true);
                }

                //Insert into MovementActionTable
                MovementActionIdentifiers movtype = new MovementActionIdentifiers();
                movtype.MovementActionType = MovementnActionType.notification_process_completed;
                movtype.ESDALRef = Esdalref;
                movtype.NotificationID = NotificationId;
                movtype.ProjectId = ProjectId;
                movtype.UserName = SessionInfo.UserName;
                movtype.NotificationCode = Esdalref;
                string ErrMsg1 = string.Empty;
                string MovementDescription = MovementActions.GetMovementActionString(SessionInfo, movtype, out ErrMsg1);
                loggingService.SaveMovementAction(movtype.ESDALRef, (int)movtype.MovementActionType, MovementDescription, ProjectId, revisionNo,versionNo, SessionInfo.UserSchema);


                return Json(new { result = NotificationId, EsdalRefNo = Esdalref });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Notification/GenerateNotification, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }

        }
        #endregion

        #region
        public bool SaveAffectedSrtuctConstrNotif(RouteAssessmentModel objRouteAssessmentModel, int NotificationId)
        {
            bool update = false;

            try
            {
                CommonNotifMethods commonNotif = new CommonNotifMethods();
                AffectedStructConstrParam affectedParam = new AffectedStructConstrParam
                {
                    AffectedConstraints = commonNotif.GetAffectedConstrList(objRouteAssessmentModel.Constraints),
                    AffectedSections = commonNotif.GetAffectedStructList(objRouteAssessmentModel.AffectedStructure),
                    NotificationId = NotificationId
                };
                if (affectedParam.AffectedSections.Count > 0 || affectedParam.AffectedConstraints.Count > 0)
                    update = notificationsService.SaveAffectedNotificationDetails(affectedParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return update;
        }
        #endregion

       

        public JsonResult IsRouteAnalysisComplete(long analysisId = 0)
        {
            bool completeFlag = true;
            string message = string.Empty;

            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            RouteAssessmentModel objRouteAssessmentModel = new RouteAssessmentModel();
            objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisId, 9, SessionInfo.UserSchema);

            if (objRouteAssessmentModel.DriveInst == null
                && objRouteAssessmentModel.AffectedStructure == null
                && objRouteAssessmentModel.AffectedParties == null)
            {
                completeFlag = false;
                message = "Please generate driving instructions, affected parties and affected structures.";
            }
            else
            {
                
                if (objRouteAssessmentModel.DriveInst == null)
                {
                    completeFlag = false;
                    message = "Please perform route assessment.";
                }
                if (objRouteAssessmentModel.AffectedStructure == null)
                {
                    completeFlag = false;
                    if (message != string.Empty)
                        message += ", affected structures";
                    else
                        //message += "Please generate affected structures";
                        message = "Please perform route assessment";
                }
                if (objRouteAssessmentModel.AffectedParties == null)
                {
                    completeFlag = false;
                    if (message != string.Empty)
                        message += ", affected parties";
                    else
                        //message += "Please generate affected parties
                        message = "Please perform route assessment";

                }
                //if (message != string.Empty)
                //    message += ".";

            }
            return Json(new { result = completeFlag, message = message });
        }
        public ActionResult DisplayNotification(long notificationId, string notificationCode, int notifStatus = 0, int Ishistoric = 0)
        {
            ViewBag.NotificationId = notificationId;
            ViewBag.NotificationCode = notificationCode;
            ViewBag.IsHistory = Ishistoric;
            Session["AppFlag"] = "Notif";
            Session["IsNotif"] = true;
            UserInfo SessionInfo = null;

            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            SessionInfo = (UserInfo)Session["UserInfo"];
            if (SessionInfo.HelpdeskRedirect == "true")
            {
                ViewBag.Helpdesk_redirect = SessionInfo.HelpdeskRedirect;
            }

            switch (notifStatus)
            {
                case 1:
                    ViewBag.VersionStatus = "Work in progress";
                    break;
                case 0:
                    ViewBag.VersionStatus = "Notification";
                    break;
            }

            MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
            string ErrMsg = string.Empty;
            string sysEventDescp = string.Empty;
            if (SessionInfo.HelpdeskRedirect == "true")
            {
                //decrypt esdal ref.
                notificationCode = notificationCode.Replace(" ", "+");
                notificationCode = MD5EncryptDecrypt.DecryptDetails(notificationCode);


                #region ------saving loggs for helpdesk redirect for another login--------------
                if (SessionInfo.UserName != null)
                {
                    #region sys_events for saving loggin info for helpdesk redirect Haulier
                    if (SessionInfo.UserSchema == UserSchema.Portal)
                    {
                        movactiontype.SystemEventType = SysEventType.Check_as_Haulier;
                        movactiontype.UserId = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                        movactiontype.UserName = SessionInfo.HelpdeskUserName;
                        movactiontype.ESDALRef = notificationCode;
                        sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                        int user_ID = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                        loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, UserSchema.Portal);
                    }
                    #endregion
                }
                #endregion ------end--------------
            }
            bool isAcknowledged = notificationsService.IsAcknowledged(notificationCode, Ishistoric);
            ViewBag.IsAcknowledged = isAcknowledged;

            // Date 12 Feb 2015 Ticket No 3723
            if (!isAcknowledged)
            {
                CollaborationModel collaboration = notificationsService.GetUnacknowledgedCollaboration(notificationCode, Ishistoric);

                string huallierNotes = string.Empty;
                if (!string.IsNullOrEmpty(collaboration.Notes))
                {
                    huallierNotes = collaboration.Notes.PadLeft(10);

                }
                ViewBag.huallierNotes = huallierNotes;
            }

            return View("DisplayNotification");
        }
        public ActionResult SaveAcknowledgement(long DocID, int ColNo, string esdalRefNumber, int historic = 0)
        {
            try
            {
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                bool Status = notificationsService.UpdateCollborationAck(DocID, ColNo, Convert.ToInt32(SessionInfo.UserId), null, historic);

                bool isAcknowledged = notificationsService.IsAcknowledged(esdalRefNumber, historic);

                return Json(isAcknowledged);
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Notification/DisplayCollaborationList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        public ActionResult DisplayGeneralDetails(long notificationId, int isHistory = 0)
        {
            NotificationGeneralDetails objNotifGeneralDetails;
            UserInfo SessionInfo = null;

            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            SessionInfo = (UserInfo)Session["UserInfo"];

            objNotifGeneralDetails = notificationsService.GetNotificationGeneralDetail(notificationId, isHistory);
            if (objNotifGeneralDetails != null && objNotifGeneralDetails.DispensationId != null && objNotifGeneralDetails.DispensationId.Length > 0)
            {
                var dispensationIds = JsonConvert.DeserializeObject<List<long>>(objNotifGeneralDetails.DispensationId);
                if (dispensationIds != null && dispensationIds.Any())
                {
                    objNotifGeneralDetails.NotificationDispensationList = documentService.GetNotificationDispensation(objNotifGeneralDetails.NotificationId, isHistory);
                }
            }
            bool isRenotify = objNotifGeneralDetails.IsMostRecent == 1;
            ViewBag.ReNotify = isRenotify;
            ViewBag.ShowWarning = false;
            ViewBag.IsHistory = isHistory;
            if (objNotifGeneralDetails.ShowWarning == 1)
            {
                ViewBag.ShowWarning = true;
            }
            int contactId = movementsService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));
            ViewBag.contactId = contactId;
            return PartialView("_DisplayNotificationGeneralDetails", objNotifGeneralDetails);
        }

        /// <summary>
        /// generate haulier notification document
        /// </summary>
        /// <param name="notificationId">notification id</param>
        /// <param name="contactId">contact id</param>
        /// As per my review, hard code valeu is not required , by 15/07/2022
        /// <returns></returns>
        [HttpGet]
        public ActionResult HaulierNotificationDocument(int notificationId = 257, int contactId = 9418)
        {
            try
            {
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];

                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }

                byte[] haulierNotificationDocument = notificationDocService.GenerateHaulierNotificationDocument(notificationId, Enums.DocumentType.PDF, contactId, SessionInfo);

                if (haulierNotificationDocument != null)
                {
                    
                    MemoryStream docStream = new MemoryStream();
                    docStream.Write(haulierNotificationDocument, 0, haulierNotificationDocument.Length);
                    docStream.Position = 0;
                    return new FileStreamResult(docStream, "application/pdf");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data is not valid for current movement therefore PDF document will not be generated.');</script>");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("DocumentConsole/HaulierNotificationDocument, Exception: {0}", ex));
                throw ex;
            }
        }

        /// <summary>
        /// generate proposed route document for haulier
        /// </summary>
        /// <param name="esdalRefNo">esdal ref no</param>
        /// <param name="organisationId">organisation id</param>
        /// <param name="contactId">contact id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HaulierProposedRouteDocument(string esdalRefNo = "ALE1/10/1", int organisationId = 3023, int contactId = 4057)
        {
            try
            {
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];

                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }


                byte[] haulierProposedDocument = documentService.GenerateHaulierProposedRouteDocument(esdalRefNo, organisationId, contactId, UserSchema.Portal, SessionInfo);

                if (haulierProposedDocument != null)
                {
                    MemoryStream docStream = new MemoryStream();

                    docStream.Write(haulierProposedDocument, 0, haulierProposedDocument.Length);

                    docStream.Position = 0;

                    return new FileStreamResult(docStream, "application/pdf");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data is not valid for current movement therefore PDF document will not be generated.');</script>");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("DocumentConsole/HaulierProposedRouteDocument, Exception: {0}", ex));
                throw ex;
            }
        }

        /// <summary>
        /// generate haulier agreed route document
        /// </summary>
        /// <param name="esDALRefNo">esdal ref no</param>
        /// <param name="order_no">order no</param>
        /// <param name="contactId">contact id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HaulierAgreedRouteDocument(string esDALRefNo = "GCS1/25/2", string order_no = "P21/2012", int contactId = 8866)
        {
            try
            {
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];

                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }


                byte[] haulierAgreedDocument = sortApplicationService.GenerateHaulierAgreedRouteDocument(esDALRefNo, order_no, contactId, SessionInfo.UserTypeId);

                if (haulierAgreedDocument != null)
                {
                    MemoryStream docStream = new MemoryStream();

                    docStream.Write(haulierAgreedDocument, 0, haulierAgreedDocument.Length);

                    docStream.Position = 0;

                    return new FileStreamResult(docStream, "application/pdf");
                }
                else
                {
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("DocumentConsole/HaulierAgreedRouteDocument, Exception: {0}", ex));
                throw ex;
            }
        }

        public ActionResult DisplayRouteVehicle(int notificationId, int isHistory = 0)
        {
            Session["RouteAssessmentFlag"] = "";
            List<AffectedStructures> ojbroute = new List<AffectedStructures>();
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            //routeVehicleList = vehicleconfigService.GetRouteVehicleList(0, 0, contentRefNo, SessionInfo.UserSchema);
            ojbroute = applicationService.GetNotifRouteParts(notificationId, 1);
            ViewBag.MovementRouteList = ojbroute;
            ViewBag.IsHistory = isHistory;
            Session["RouteAssessmentFlag"] = "Completed";
            return PartialView("_DisplayNotificationRouteVehicle");
        }

        #region PopUpAddressBook
        public ActionResult PopUpAddressBook(int? pageNum, int? pageSize, int NotifID = 0, int analysisId = 0, string searchValue = " ", string origin = " ", int count = 0, bool fromSimplified = false, bool fromSort = false, bool fromAnnotation = false)
        {
            try
            {
                //AddressBook\
                UserInfo SessionInfo = null;

                SessionInfo = (UserInfo)Session["UserInfo"];

                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (!PageAccess.GetPageAccess("60001"))
                {
                    return RedirectToAction("Error", "Home");
                }


                int pageNumber = 0;




                pageNumber = (pageNum ?? 1);

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
                ViewBag.searchValue = searchValue;
                ViewBag.originImp = origin;
                ViewBag.pageNum = pageNumber;
                ViewBag.pageSize = pageSize;
                ViewBag.NotifID = NotifID;
                ViewBag.analysisId = analysisId;

                ViewBag.IsSimplifiedNotif = fromSimplified;

                if (fromSort == true)
                {
                    ViewBag.IsSortSideCall = true;
                    ViewBag.FlagToCreateNonEsdalSORTCon = true;//to check condition this button will show in SORT
                }

                if (origin == "annotation")
                {
                    ViewBag.count = count;
                }
                ViewBag.FromAnnotation = fromAnnotation;
                Session["Iscontact"] = 0;
                return PartialView("PopUpAddressBook");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Notification/PopUpAddressBook, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }

        }
        #endregion

        #region DeleteXmlContact(string orgName = "", string contactName = "", int analysisId =0, int notifId=0)
        public ActionResult DeleteXmlContact(string orgName = "", string contactName = "", int analysisId = 0, int notifId = 0, int appRevisionId = 0)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            
            RouteAssessmentModel objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisId, 7, SessionInfo.UserSchema);

            string xmlAffectedParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.AffectedParties));

            string newXmlAffectedParties = routeAssessmentService.XmlAffectedPartyDeleteFromXml(xmlAffectedParties, orgName, contactName);

            objRouteAssessmentModel.AffectedParties = StringExtractor.ZipAndBlob(newXmlAffectedParties);

            routeAssessmentService.updateRouteAssessment(objRouteAssessmentModel, analysisId, SessionInfo.UserSchema);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\AffectedParties.xslt");

            string result = XsltTransformer.Trafo(objRouteAssessmentModel.AffectedParties, path, out string errormsg);

            ViewBag.AffectedParties = result;
            int haulierAffected = 1;
            if (SessionInfo.UserTypeId == Common.Constants.UserType.Sort)
            {
                var contactList = routeAssessmentService.FetchContactDetails(Convert.ToInt32(Session["SORTOrgID"]), appRevisionId, SessionInfo.UserSchema);
                if (contactList!=null && contactList.Any() && contactList[0].ContactName == contactName && contactList[0].OrganisationName == orgName)
                    haulierAffected = 0;
            }
            var jsonObj = new { NotifID = notifId, analysisId = analysisId, haulierAffected = haulierAffected };

            return Json(new { result = jsonObj });
        }

        #endregion

        #region ManualAddedParties
        public ActionResult ManualAddedParties(int analysisId = 0, bool isSORT = false, bool isVR1Sort = false, int flag = 0, int revisionId = 0, int haulierOrgId = 0)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            int orgId = (int)SessionInfo.OrganisationId;
            RouteAssessmentModel objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisId, 7, SessionInfo.UserSchema);
            string xmlAffectedParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.AffectedParties));
            var affectedParty = StringExtraction.xmlAffectedPartyDeserializer(xmlAffectedParties);
            if (affectedParty != null)
            {
                foreach (var item in affectedParty.GeneratedAffectedParties)
                {
                    if (item.DispensationStatus == Domain.RouteAssessment.XmlAffectedParties.DispensationStatusType.nonematching)
                    {
                        var cnt = routeAssessmentService.GetDispensationCount(orgId, item.Contact.Contact.simpleContactRef.OrganisationId);
                        if (cnt > 0)
                        {
                            item.DispensationStatus = Domain.RouteAssessment.XmlAffectedParties.DispensationStatusType.somematching;
                        }
                    }
                }
                int affectContact = 0;
                string contactName;
                string seesionContactName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                if (SessionInfo.UserTypeId == Common.Constants.UserType.Sort)
                {
                    var contactList = routeAssessmentService.FetchContactDetails(haulierOrgId, revisionId, SessionInfo.UserSchema);
                    seesionContactName = contactList[0].ContactName;
                }
                foreach (var item in affectedParty.ManualAffectedParties)
                {
                    contactName = item.Contact.Contact.adhocContactRef.FullName;
                    if (contactName.ToLower() == seesionContactName.ToLower())
                    {
                        affectContact = 1;
                    }
                }

                string updatedXmlAffectedParty = StringExtraction.XmlAffectedPartySerializer(affectedParty);
                objRouteAssessmentModel.AffectedParties = StringExtraction.ZipAndBlob(updatedXmlAffectedParty);
                string result;
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\AffectedParties.xslt");

                string errormsg;
                if (!isSORT)
                {
                    result = XsltTransformer.Trafo(objRouteAssessmentModel.AffectedParties, path, out errormsg);
                }
                else
                {
                    result = XsltTransformer.TrafoSORT(objRouteAssessmentModel.AffectedParties, path, out errormsg);
                    ViewBag.IsSortSide = "SortSideCall";
                    ViewBag.AnalysisIdManual = analysisId;
                    ViewBag.isVR1Sort = isVR1Sort;
                }
                ViewBag.AnalysisIdManual = analysisId;
                ViewBag.AffectedParties = result;
                ViewBag.Flag = flag;
                ViewBag.AffectFlag = affectContact;
            }
            return PartialView();
        }

        #endregion

        #region GetSortAffectedParties
        public ActionResult GetSortAffectedParties(int analysisId = 0, bool isSORT = false, bool isVR1Sort = false)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

            RouteAssessmentModel objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisId, 7, SessionInfo.UserSchema);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\AffectedParties.xslt");

            if (isSORT == true)
            {
                ViewBag.IsSortSide = "SortSideCall";
                ViewBag.AnalysisIdManual = analysisId;
                ViewBag.isVR1Sort = isVR1Sort;
            }
            string result = XsltTransformer.TrafoSORT(objRouteAssessmentModel.AffectedParties, path, out string errormsg);

            return Json(result);
        }
        #endregion

        #region Clone Notification
        public ActionResult CloneNotification(int notificationId, string notificationCode,int isHisto=0)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            NotificationGeneralDetails ObjClone;
            if (isHisto==0)
            {
                ObjClone = notificationsService.CloneNotification(notificationId);
            }
            else
            {
                ObjClone = notificationsService.CloneHistoricNotification(notificationId);
                routeService.UpdateCloneHistoricRoute(ObjClone.ContentReferenceNo, 0, 0, SessionInfo.UserSchema);
            }

            #region System Event Log - Haulier_notification_cloned
            string ErrMsg = string.Empty;
            string sysEventDescp = string.Empty;
            MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
            movactiontype.UserName = SessionInfo.UserName;
            movactiontype.NotificationID = notificationId;
            movactiontype.NewNotificationID = ObjClone.NotificationId;
            movactiontype.SystemEventType = SysEventType.Haulier_notification_cloned;

            sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
            loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);

            //Insert into MovementActionTable
            MovementActionIdentifiers movtype = new MovementActionIdentifiers();
            movtype.MovementActionType = MovementnActionType.haulier_notification_cloned;
            movtype.ESDALRef = notificationCode;
            movtype.NotificationID = notificationId;
            movtype.NewNotificationID = ObjClone.NotificationId;
            movtype.NewNotificationCode = ObjClone.NotificationCode;
            movtype.UserName = SessionInfo.UserName;
            string ErrMsg1 = string.Empty;
            string MovementDescription = MovementActions.GetMovementActionString(SessionInfo, movtype, out ErrMsg1);
            loggingService.SaveMovementAction(movtype.ESDALRef, (int)movtype.MovementActionType, MovementDescription, ObjClone.ProjectId, 0,0, SessionInfo.UserSchema);

            #endregion

            var activityName = WorkflowTaskFinder.GenerateWorkflowActivityName(WorkflowActivityTypes.An_Activity_VehicleDetails);
            var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
            PlanMvmntPayLoad planMvmntPayLoad = new PlanMvmntPayLoad
            {
                NextActivity = activityName,
                OrgId = SessionInfo.organisationId,
                OrgName = SessionInfo.OrganisationName,
                MovementKey = ObjClone.NotificationId,
                NotificationId = ObjClone.NotificationId,
                IsNotif = true,
                VehicleMoveId = ObjClone.MovementId,
                VersionId = ObjClone.VersionId,
                IsNotifClone = true,
                ContenRefNo = ObjClone.ContentReferenceNo,
                MovementType = (int)MovementType.notification,
                PrevMovType = (int)MovementType.notification,
                VehicleClass = ObjClone.VehicleCode,
                AnalysisId = ObjClone.AnalysisId,
                IsRouteVehicleAssigned = true,
                PreviousContactName = ObjClone.PreviousContactName
            };

            if (ObjClone.VehicleCode == (int)VehicleClassificationType.VehicleSpecialOrder)
            {
                planMvmntPayLoad.MvmntType = new VehicleMovementType
                {
                    MovementType = (int)MovementType.notification,
                    SOANoticePeriod = ObjClone.VSOType == (int)VSOType.soa || ObjClone.VSOType == (int)VSOType.soapolice ? 2 : 0,
                    PoliceNoticePeriod = ObjClone.VSOType == (int)VSOType.police || ObjClone.VSOType == (int)VSOType.soapolice ? 2 : 0,
                    VehicleClass = (int)VehicleClassificationType.VehicleSpecialOrder,
                    Message = "Vehicle Special Order."
                };
            }

            if (!applicationNotificationManagement.IsThisMovementExist(ObjClone.NotificationId, 0, out string workflowKey))
            {
                applicationNotificationManagement.StartWorkflow(planMvmntPayLoad, 2);
                new SessionData().E4_AN_PlanMovement = planMvmntPayLoad;
            }

            return Json(new { result = ObjClone });
        }
        #endregion
        public ActionResult DisplayNotificationHistory(int? page, int? pageSize, long notificationId = 0, int ?sortOrder = null, int ?sortType = null, int historic = 0,long projectId=0)
        {
            //viewbag for pagination
            int pageNumber = (page ?? 1);
            if (pageSize == null)
            {
                pageSize = (Session["PageSize"] != null) ? (int)Session["PageSize"] : 10;
            }
            else
            {
                Session["PageSize"] = pageSize;
            }
            ViewBag.pageSize = pageSize;
            ViewBag.page = pageNumber;
            ViewBag.NotificationId = notificationId;
            ViewBag.ProjectId = projectId;
            sortOrder = sortOrder != null ? (int)sortOrder : 1;
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];                                                                     //presetFilter = sortType != null ? (int)sortType : presetFilter;
            sortType = sortType != null ? (int)sortType : 1; // desc
            ViewBag.SortOrder = sortOrder;
            ViewBag.SortType = sortType;
            List<NotificationHistory> notifHistory = new List<NotificationHistory>();
            int userTypeId = SessionInfo.UserTypeId == 696001 ? 696001 : 0;
            notifHistory = loggingService.GetNotificationHistory(pageNumber, (int)pageSize, notificationId,(int)sortOrder,(int)sortType, historic, userTypeId, projectId);

            long totalCount = 0;
            if (notifHistory != null && notifHistory.Count > 0)
            {
                ViewBag.TotalCount = Convert.ToInt32(notifHistory[0].TotalCount);
                totalCount = notifHistory[0].TotalCount;
            }
            else
            {
                ViewBag.TotalCount = 0;
                totalCount = 0;
            }
            var notificatinPagedList = new StaticPagedList<NotificationHistory>(notifHistory, pageNumber, (int)pageSize, (int)totalCount);
            return PartialView("_DisplayNotificationHistory", notificatinPagedList);
        }
        /// <summary>
        /// The display collaboration list will fetch the records as per the version id and where notes have been entered        
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="VersionID"></param>
        /// <returns></returns>

        public ActionResult DisplayCollaborationList(int? page, int? pageSize, string notificationCode, int notificationId, int historic = 0)
        {

            string input = notificationCode;
            int index = input.LastIndexOf("#");
            if (index >= 0)
                notificationCode = input.Substring(0, index); // or index + 1 to keep slash

            try
            {
                #region Session Check
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                SessionInfo = (UserInfo)Session["UserInfo"];
                if (SessionInfo.HelpdeskRedirect == "true")
                {
                    ViewBag.Helpdest_redirect = SessionInfo.HelpdeskRedirect;
                }
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

                ViewBag.NotificationId = notificationId;
                if (!string.IsNullOrEmpty(notificationCode))
                {
                    if (notificationCode.Contains("~"))
                    {
                        notificationCode = notificationCode.Replace("~", "#");
                    }

                    string[] splitNotificationCode = notificationCode.Split('/');

                    if (splitNotificationCode.Length > 1)
                        notificationCode = splitNotificationCode[0] + "/" + splitNotificationCode[1] + "/" + splitNotificationCode[2];

                    // Changes added by NetWeb for listing all the Collaboration details for that specific version
                    if (splitNotificationCode.Length == 3)
                    {
                        if (splitNotificationCode[2] != null && splitNotificationCode[2].IndexOf("S") != -1)
                        {
                            string[] notificationVersion = splitNotificationCode[2].Split('(');

                            if (notificationVersion.Length >= 1)
                            {
                                notificationCode += notificationVersion[0];
                            }
                        }
                    }
                }

                List<CollaborationModel> collaborationList = notificationsService.GetCollaborationList(pageNumber, (int)pageSize, notificationCode, notificationId, historic);
                if (collaborationList != null && collaborationList.Count > 0)
                {
                    ViewBag.TotalCount = collaborationList[0].RecordCount;
                }
                else
                {
                    ViewBag.TotalCount = 0;
                }
                var CollaborationlistPagedList = new StaticPagedList<CollaborationModel>(collaborationList, pageNumber, (int)pageSize, (int)ViewBag.TotalCount);

                //#5945 added by Poonam on 11 Nov 2016
                //Update needs attention flag
                //int Needs_att = ApplicationProvider.Instance.Update_Needs_Attention(Notification_Id, 0, 4);

                return PartialView("_DisplayCollaborationList", CollaborationlistPagedList);
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Notification/DisplayCollaborationList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        #region ReNotify
        public ActionResult ReNotify(int NotificationID, int VR1_ReNotify = 0, string notifcode = null, int versionStatus = 0)
        {
            int projectId = 0;
            int revisionNo = 0;
            int versionNo = 0;
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            NotificationGeneralDetails objRenotify = notificationsService.RenotifyNotification(NotificationID, VR1_ReNotify);

            #region System Event Log - Haulier_notification_renotified
            MovementActionIdentifiers movactiontype = new MovementActionIdentifiers
            {
                UserName = SessionInfo.UserName,
                NotificationID = NotificationID,
                NewNotificationID = objRenotify.NotificationId,
                ESDALRef = notifcode
            };
            if (VR1_ReNotify == 2)
                movactiontype.SystemEventType = SysEventType.Haulier_notifcation_revised;
            else
                movactiontype.SystemEventType = SysEventType.Haulier_notification_renotified;

            string sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out string ErrMsg);
            loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);

            //Insert into MovementActionTable
            movactiontype = new MovementActionIdentifiers
            {
                MovementActionType = MovementnActionType.haulier_notification_renotified,
                ESDALRef = notifcode,
                NotificationID = NotificationID,
                NewNotificationID = objRenotify.NotificationId,
                NewNotificationCode = objRenotify.NotificationCode,
                UserName = SessionInfo.UserName
            };
            string MovementDescription = MovementActions.GetMovementActionString(SessionInfo, movactiontype, out string ErrMsg1);
            loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, objRenotify.ProjectId, revisionNo,versionNo, SessionInfo.UserSchema);

            #endregion

            bool isagreedRenotify = versionStatus == (int)VersionStatus.agreed || versionStatus == (int)VersionStatus.agreed_recleared;
            var activityName = WorkflowTaskFinder.GenerateWorkflowActivityName(WorkflowActivityTypes.An_Activity_VehicleDetails);
            var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
            PlanMvmntPayLoad planMvmntPayLoad = new PlanMvmntPayLoad
            {
                NextActivity = activityName,
                OrgId = SessionInfo.organisationId,
                OrgName = SessionInfo.OrganisationName,
                MovementKey = objRenotify.NotificationId,
                NotificationId = objRenotify.NotificationId,
                IsNotif = true,
                VehicleMoveId = objRenotify.MovementId,
                VersionId = objRenotify.VersionId,
                IsRenotify = true,
                IsAgreedNotified = isagreedRenotify,//set true for when renotify a notified agreed application
                ContenRefNo = objRenotify.ContentReferenceNo,
                MovementType = (int)MovementType.notification,
                PrevMovType = (int)MovementType.notification,
                VehicleClass = objRenotify.VehicleCode,
                AnalysisId = objRenotify.AnalysisId,
                IsRouteVehicleAssigned = true,
                PreviousContactName = objRenotify.PreviousContactName
            };

            if (objRenotify.VehicleCode == (int)VehicleClassificationType.VehicleSpecialOrder)
            {
                planMvmntPayLoad.MvmntType = new VehicleMovementType
                {
                    MovementType = (int)MovementType.notification,
                    SOANoticePeriod = objRenotify.VSOType == (int)VSOType.soa || objRenotify.VSOType == (int)VSOType.soapolice ? 2 : 0,
                    PoliceNoticePeriod = objRenotify.VSOType == (int)VSOType.police || objRenotify.VSOType == (int)VSOType.soapolice ? 2 : 0,
                    VehicleClass = (int)VehicleClassificationType.VehicleSpecialOrder,
                    Message = "Vehicle Special Order."
                };
            }

            if (!applicationNotificationManagement.IsThisMovementExist(objRenotify.NotificationId, 0, out string workflowKey))
            {
                applicationNotificationManagement.StartWorkflow(planMvmntPayLoad, 2);
                new SessionData().E4_AN_PlanMovement = planMvmntPayLoad;
            }
            return Json(new { result = objRenotify });
        }
        #endregion

        #region CheckLoginUserCount
        public ActionResult CheckLoginUserCount()
        {
            bool count = false;
            int result = 0;
            int UserID = 0;

            if (Session["UserInfo"] == null)
            {
                string actionName = Request.RequestContext.RouteData.GetRequiredString("action"); //this.ControllerContext.RouteData.Values["action"];
                string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                return RedirectToAction("Login", "Account");
            }

            UserInfo SessionInfo = null;

            SessionInfo = (UserInfo)Session["UserInfo"];
            UserID = Convert.ToInt32(SessionInfo.UserId);
            int viewMapFlag = 0;
            if (Session["ViewMap"] != null)
                viewMapFlag = (int)Session["ViewMap"];
            if (viewMapFlag != 2)
            {
                count = true;
                viewMapFlag = 2;
                Session["ViewMap"] = (int)viewMapFlag;
                result = notificationsService.SetLoginStatus(UserID, 1);
            }
            else
            {
                count = false;
            }

            return Json(new { Status = count }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult NotificationDispensationPopUp(int organisationId = 0, int analysisId = 0, string grantorName = "", bool showRestriction = false, int notifid = 0)
        {
            ViewBag.Org_Id = organisationId;
            ViewBag.analysisId = analysisId;
            ViewBag.Grantor_name = grantorName;
            ViewBag.showRestriction = showRestriction;
            ViewBag.notifid = notifid;
            return PartialView();
        }
        
        public ActionResult DeleteNotification(int notificationId)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            bool status = false;

            int iResult = notificationsService.DeleteNotification(notificationId);
            if (iResult > 0)
                status = true;

            #region System Event Log - Haulier_notifcation_deleted
            string ErrMsg = string.Empty;
            string sysEventDescp = string.Empty;
            MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
            movactiontype.UserName = SessionInfo.UserName;
            movactiontype.NotificationID = notificationId;
            movactiontype.SystemEventType = SysEventType.Haulier_notifcation_deleted;

            sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
            loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(SessionInfo.UserId), SessionInfo.UserSchema);
            #endregion

            return Json(new { result = status });
        }

        public ActionResult ViewUnacknowledgedCollaboration(string notificationCode, int historic = 0)
        {
            try
            {
                #region Session Check
                UserInfo SessionInfo = null;
                notificationCode = notificationCode.Replace("~", "#");

                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];
                ViewBag.Helpdesk_redirect = "false";
                if (SessionInfo.HelpdeskRedirect == "true")
                {
                    ViewBag.Helpdesk_redirect = SessionInfo.HelpdeskRedirect;
                }
                #endregion

                ViewBag.notificationCode = notificationCode;

                CollaborationModel collaboration = notificationsService.GetUnacknowledgedCollaboration(notificationCode, historic);
                return PartialView(collaboration);
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Notification/ViewUnacknowledgedCollaboration, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        public JsonResult DateTimeValidate(string startDateTime = null, string endDateTime = null, string currentDateTime = null)
        {
            int result = 0;
            DateTime start = DateTime.ParseExact(startDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(endDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            DateTime current = DateTime.ParseExact(currentDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            if (start < current)
            {
                result = 1;
            }
            if(end < start)
            {
                result = 2;
            }
            return Json(new { res = result });
        }

        public ActionResult DisplayNotifiedparties(int page = 0, int pageSize = 0, long analysisId = 0, long notificationId = 0)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            #region Paging Part
            int pageNumber = page == 0 ? 1 : page;
            if (pageSize != 0)
            {
                Session["PageSize"] = pageSize;
            }
            else
            {
                if (Session["PageSize"] == null)
                {
                    Session["PageSize"] = 10;
                }
                pageSize = (int)Session["PageSize"];
            }

            ViewBag.pageSize = pageSize;
            ViewBag.page = pageNumber;
            #endregion
            RouteAssessmentModel objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisId, 7, SessionInfo.UserSchema);
            string xmlAffectedParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.AffectedParties));
            var affectedParty = StringExtraction.xmlAffectedPartyDeserializer(xmlAffectedParties);
            List<ContactModel> contactList = new List<ContactModel>();
            ContactModel contactObj;

            foreach (var genratedAffectedParty in affectedParty.GeneratedAffectedParties)
            {
                contactObj = new ContactModel
                {
                    ContactId = genratedAffectedParty.Contact.Contact.simpleContactRef.ContactId,
                    FullName = genratedAffectedParty.Contact.Contact.simpleContactRef.FullName,
                    Organisation = genratedAffectedParty.Contact.Contact.simpleContactRef.OrganisationName,
                    OrganisationId = genratedAffectedParty.Contact.Contact.simpleContactRef.OrganisationId,
                    ContactType = genratedAffectedParty.IsPolice ? "Police alo" : "SOA",
                    Email = string.Empty,
                    DelegatorsOrganisationName = genratedAffectedParty.OnBehalfOf != null ? genratedAffectedParty.OnBehalfOf.DelegatorsOrganisationName : string.Empty,
                    DelegatorsContactId = genratedAffectedParty.OnBehalfOf != null ? genratedAffectedParty.OnBehalfOf.DelegatorsContactId : 0,
                    DelegatorsOrganisationId = genratedAffectedParty.OnBehalfOf != null ? genratedAffectedParty.OnBehalfOf.DelegatorsOrganisationId : 0,
                    DelegationId = genratedAffectedParty.OnBehalfOf != null ? genratedAffectedParty.OnBehalfOf.DelegationId : 0
                };
                contactList.Add(contactObj);
            }
            foreach (var manualAddedParty in affectedParty.ManualAffectedParties.Select(item => item.Contact.Contact.adhocContactRef))
            {
                contactObj = new ContactModel
                {
                    ContactId = 0,
                    OrganisationId = 0,
                    FullName = manualAddedParty.FullName,
                    Organisation = manualAddedParty.OrganisationName,
                    Email = manualAddedParty.EmailAddress,
                    ContactType = "Interested party"
                };
                contactList.Add(contactObj);
            }
            int totalCount = contactList.Count;
            var sortedList = contactList.OrderBy(m => m.ContactType).ThenBy(m => m.FullName).ToList();

            int skipPage = 0;

            if ((pageNumber - 1) == 0)
            {
                skipPage = 0;
            }
            else
            {
                skipPage = (pageNumber - 1) * pageSize;
            }
            sortedList = sortedList.Skip(skipPage).Take(pageSize).ToList();
            var contactPagedList = new StaticPagedList<ContactModel>(sortedList, pageNumber, pageSize, totalCount);
            ViewBag.TotalCount = totalCount;
            ViewBag.AnalysisId = analysisId;
            return PartialView(contactPagedList);
        }

        public JsonResult AddDispensationDetail(int dispensationId = 0, long notificationId = 0, long analysisId = 0, int grantorId = 0, string grantorName = null, string dispensationRefNo = null, string dispSummary = null)
        {
            int success = 0;
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            try
            {
                var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                var planMvmt = applicationNotificationManagement.GetPlanMvmtPayload();
                List<NotifDispensations> dispensationList = new List<NotifDispensations>();
                NotifDispensations dispensation = new NotifDispensations
                {
                    DispensationId = dispensationId,
                    DRN = dispensationRefNo,
                    Summary = dispSummary,
                    GrantorId = grantorId,
                    GrantorName = grantorName
                };
                if (planMvmt.DipesnsationList.Count > 0)
                {
                    dispensationList = planMvmt.DipesnsationList;
                    foreach (var item in planMvmt.DipesnsationList)
                    {
                        if (item.DispensationId == dispensationId)
                        {
                            success = 2;
                            break;
                        }
                        else
                        {
                            success = 1;
                        }
                    }
                }
                else
                {
                    success = 1;
                }
                if (success == 1)
                {
                    dispensationList.Add(dispensation);
                    byte[] affectedParties = routeAssessmentService.GetDriveInstructionsinfo(analysisId, 7, SessionInfo.UserSchema).AffectedParties;
                    string xmlAffectedParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(affectedParties));
                    string updatedXmlAffectedParty = StringExtraction.ModifyingDispensationStatusToInUse(xmlAffectedParties, grantorName, grantorId);
                    RouteAssessmentModel routeAssessmentModel = new RouteAssessmentModel
                    {
                        AffectedParties = StringExtraction.ZipAndBlob(updatedXmlAffectedParty)
                    };
                    routeAssessmentService.UpdateAnalysedRoute(routeAssessmentModel, analysisId, SessionInfo.UserSchema);
                    planMvmt.DipesnsationList = dispensationList;
                    if (applicationNotificationManagement.IsThisMovementExist(planMvmt.NotificationId, planMvmt.RevisionId, out string workflowKey)
                            && WorkflowTaskFinder.FindNextTask("HaulierApplication", WorkflowActivityTypes.An_Activity_AdditionalAffectedPartiesManualEntry, out dynamic workflowPayload) != string.Empty)
                    {
                        dynamic dataPayload = new ExpandoObject();
                        dataPayload.PlanMvmntPayLoad = planMvmt;
                        WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                        {
                            data = dataPayload,
                            workflowData = workflowPayload
                        };
                        applicationNotificationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel, false);
                    }
                }
            }
            catch
            {
                success = 0;
            }
            return Json(new { result = success, notifId = notificationId, analysisId = analysisId });
        }
        #region  AuditLog list
        /// <summary>
        /// NEN AuditLog list
        /// </summary>
        /// <returns></returns>


  
        [ActionName("AuditLog")]
        public ActionResult NENAuditLog(string ESDAL_ref_num = null, int sortFlag = 0, int pageNumber = 1, int pageSize = 10, int searchNotificationSource = 0, string searchCriteria = "0", int? sortOrder = null, int? sortType = null,bool IsClear=false)
        {
            searchNotificationSource = 0;
            UserInfo SessionInfo = null;
            long org_ID = 0;// To show All organisation's NEN to Helpdesk portal inside Audit log menu
            string searchItem = null;
            sortOrder = sortOrder != null && sortOrder != 0 ? (int)sortOrder : 1; //date
            sortType = sortType != null ? (int)sortType : 1; // asc
            ViewBag.SortOrder = sortOrder;
            ViewBag.SortType = sortType;

            try
            {
                if (IsClear)
                {
                    Session["ESDAL_ref_numAL"] = null;
                    Session["SearchCriteriaAL"] = null;
                }
                if(string.IsNullOrEmpty(ESDAL_ref_num) && Session["ESDAL_ref_numAL"]!=null)
                    ESDAL_ref_num = Session["ESDAL_ref_numAL"].ToString();
                if (string.IsNullOrEmpty(searchCriteria) && Session["SearchCriteriaAL"] != null)
                    searchCriteria = Session["SearchCriteriaAL"].ToString(); ;
                SessionInfo = (UserInfo)Session["UserInfo"];
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (SessionInfo.UserTypeId == 696002 || SessionInfo.UserTypeId == 696007)// To show only same organisation's NEN to SOA and Police portal inside Audit log menu
                {
                    org_ID = SessionInfo.OrganisationId;
                }


                //if (Session["PageSize"] == null)
                //{
                //    Session["PageSize"] = 10;
                //}

                if (pageSize == null)
                {
                    pageSize = (int)Session["PageSize"];
                }
                else if(pageSize!=null && ESDAL_ref_num == null && ESDAL_ref_num =="")
                {
                    pageSize = pageSize;
                }
                else if (pageSize != null && ESDAL_ref_num == "")
                {
                    pageSize = pageSize;
                }
                else if (pageSize != null && ESDAL_ref_num == null)
                {
                    pageSize = (int)Session["PageSize"];
                }
                else if(pageSize != null && ESDAL_ref_num != null)
                {
                    pageSize = pageSize;
                }
                else
                {
                    pageSize = (int)Session["PageSize"];
                }
                if (pageNumber == 1 && ESDAL_ref_num == null)
                {
                    //first time page is loaded page parameter will be null and search column will also be null
                    ESDAL_ref_num = null;
                    sortFlag = 0;
                    searchCriteria = "0";
                    TempData["ESDAL_ref_num"] = null;
                    TempData["sortFlag"] = 0;
                    TempData["searchCriteria"] = "0";
                }
                else if (pageNumber == 1 && ESDAL_ref_num != null && ESDAL_ref_num != "")
                {
                    //search button is clicked, page parameter is null and search column is supplied
                    //so save these values in the temp data                

                    if (TempData["ESDAL_ref_num"] != null)
                    {
                        sortFlag = Convert.ToInt32(TempData["sortFlag"]);
                        if (searchCriteria == "0")
                            searchCriteria = Convert.ToString(TempData["searchCriteria"]);
                    }
                    else
                    {
                        TempData["ESDAL_ref_num"] = ESDAL_ref_num;
                        if (sortFlag == null || sortFlag == 0)
                        {
                            TempData["sortFlag"] = 0;
                            sortFlag = 0;
                        }
                        else
                            TempData["sortFlag"] = sortFlag;
                        if (String.IsNullOrEmpty(searchCriteria))
                        {
                            TempData["searchCriteria"] = "0";
                            searchCriteria = "0";
                        }
                        else
                            TempData["searchCriteria"] = searchCriteria;
                    }
                }
                else if (pageNumber != 1 && ESDAL_ref_num == null)
                {
                    //during page number click, the page parameter will not be null but the model will be null
                    //so put the tempdata values back into the  contactSearch

                    if (TempData["ESDAL_ref_num"] == null)
                    {
                        ESDAL_ref_num = null;
                    }
                    else
                    {
                        ESDAL_ref_num = Convert.ToString(TempData["ESDAL_ref_num"]);
                    }

                    sortFlag = Convert.ToInt32(TempData["sortFlag"]);
                    searchCriteria = Convert.ToString(TempData["searchCriteria"]);
                }

                if (ESDAL_ref_num != null)
                {
                    ViewBag.SearchESDAL_num = ESDAL_ref_num;
                    searchItem = ESDAL_ref_num;
                }
                else
                {
                    ESDAL_ref_num = ViewBag.SearchESDAL_num;
                }
                if (ESDAL_ref_num == null)
                {
                    sortFlag = 0;
                    searchCriteria = "0";
                }
                ViewBag.sort_Flag = sortFlag;
                ViewBag.page_Number = pageNumber;
                ViewBag.page_Size = pageSize;
                ViewBag.searchItem = searchCriteria;
                List<NENAuditLogList> auditLogObjList;
                searchCriteria = sortFlag == 0 ? "0" : searchCriteria;
                auditLogObjList = loggingService.GetAuditListSearch(searchItem, pageNumber, pageSize, sortFlag, org_ID, searchNotificationSource, searchCriteria, sortType.Value, sortOrder);
                // int totalCount = 20000000;
                int totalCount = 0;
                if (auditLogObjList != null && auditLogObjList.Count != 0)
                {
                    //int.TryParse(auditLogObjList[0].RecordCount.ToString(), out tempPageCount);
                    totalCount = Convert.ToInt32(auditLogObjList[0].RecordCount);
                    //auditLogObjList[0].RecordCount = tempPageCount;
                }
                ViewBag.TotalCount = totalCount;
                var AuditLogPageList = new StaticPagedList<NENAuditLogList>(auditLogObjList, pageNumber, (int)pageSize, totalCount);

                Session["SearchCriteriaAL"] = null;
                Session["ESDAL_ref_numAL"] = null;
                if (!string.IsNullOrEmpty(ESDAL_ref_num))
                {
                    Session["SearchCriteriaAL"] = searchCriteria;
                    Session["ESDAL_ref_numAL"] = ESDAL_ref_num;
                }
                ViewBag.SearchESDAL_num = ESDAL_ref_num;
                return View("~/Views/NENNotification/NENAuditLog.cshtml", AuditLogPageList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Notification/AuditLog, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion
    }
}