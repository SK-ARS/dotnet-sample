using Newtonsoft.Json;
using STP.Domain.Workflow;
using STP.Domain.Workflow.Models;
using STP.ServiceAccess.Workflows.ApplicationsNotifications;
using STP.ServiceAccess.Workflows.SORTSOProcessing;
using STP.ServiceAccess.Workflows.SORTVR1Processing;
using STP.Web.WorkflowProvider;
using System.Web.Mvc;

namespace STP.Web.Controllers
{
    public class WorkflowController : Controller
    {

        private readonly ISORTSOProcessingService sortSOProcessingService;
        private readonly ISORTVR1ProcessingService sortVR1ProcessingService;
        private readonly IApplicationNotificationWorkflowService applicationNotificationWorkflowService;
        public WorkflowController(ISORTSOProcessingService sortSOProcessingService, ISORTVR1ProcessingService sortVR1ProcessingService, IApplicationNotificationWorkflowService applicationNotificationWorkflowService)
        {
            this.sortSOProcessingService = sortSOProcessingService;
            this.sortVR1ProcessingService = sortVR1ProcessingService;
            this.applicationNotificationWorkflowService = applicationNotificationWorkflowService;
        }
       
        [HttpPost]
        public JsonResult Index(WorkflowIndexPostModel workflowIndexPostModel)
        {
            return GenerateRoute(workflowIndexPostModel.module, workflowIndexPostModel.pointOfCommunication, workflowIndexPostModel.dataModel);
        }

        public JsonResult GetSORTSOProcessingNextTask(string esdalReference)
        {
            esdalReference = WorkflowTaskFinder.WorkflowEsdalReferenceNumberBuilder(esdalReference);
            if (esdalReference.Length > 0)
            {
                var sortSOApplicationManagement = new SORTSOApplicationManagement(sortSOProcessingService);
                if (sortSOApplicationManagement.CheckIfProcessExit(esdalReference, startProcess: false))
                {
                    var currentTask = sortSOProcessingService.GetCurrentTask(esdalReference);
                    var showMessage = WorkflowTaskFinder.SORTSOProcessingNextTaskFinder(currentTask);
                    return Json(new { nextTask = showMessage }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { nextTask = "" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSORTVR1ProcessingNextTask(string esdalReference)
        {
            esdalReference = WorkflowTaskFinder.WorkflowEsdalReferenceNumberBuilder(esdalReference, true);
            if (esdalReference.Length > 0)
            {
                var sortVR1ApplicationManagement = new SORTVR1ApplicationManagement(sortVR1ProcessingService);
                if (esdalReference.Length > 0 && sortVR1ApplicationManagement.CheckIfProcessExit(esdalReference, startProcess: false))
                {
                    var currentTask = sortVR1ProcessingService.GetCurrentTask(esdalReference);
                    var showMessage = WorkflowTaskFinder.SORTVR1ProcessingNextTaskFinder(currentTask);
                    return Json(new { nextTask = showMessage }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { nextTask = "" }, JsonRequestBehavior.AllowGet);
        }
        private JsonResult GenerateRoute(string module, int pointOfCommunication, string dataModel)
        {
            switch (module)
            {
                case "SOP":
                    return GenerateSoProcessingRouteWorkflow(pointOfCommunication, dataModel);
                case "SVR1":
                    return GenerateVR1ProcessingRouteWorkflow(pointOfCommunication, dataModel);
                case "APPLN":
                    return GenerateApplicationProcessingRouteWorkflow(pointOfCommunication, dataModel);
            }
            return Json(null);
        }
        private JsonResult GenerateSoProcessingRouteWorkflow(int pointOfCommunication, string dataModel)
        {
            var processName = "Process_SORTSOApplicationApprovalV08";
            WorkflowActivityRoute workflowActivityRoute;
            var sortSOApplicationManagement = new SORTSOApplicationManagement(sortSOProcessingService);
            string activityName;
            switch (pointOfCommunication)
            {
                case 1: //ALLOCATE USER
                    var deserializeModel = JsonConvert.DeserializeObject<AllocateSORTUserCntrlModel>(dataModel);
                    activityName = WorkflowTaskFinder.GenerateWorkflowActivityName(Common.Enums.WorkflowActivityTypes.Ap_Activity_AllocateApplication2RoutingOfficers);

                    workflowActivityRoute = sortSOApplicationManagement.GetWorkflowActivityRoute(activityName, processName);
                    return Json(new { route = workflowActivityRoute.routeUrl, dataJson = deserializeModel }, JsonRequestBehavior.AllowGet);

                case 2:
                    //Activity_CreateAndModifyCandidateVersion
                    var deserialize_SaveCandidateRouteCntrlModel = JsonConvert.DeserializeObject<SaveCandidateRouteCntrlModel>(dataModel);
                    activityName = WorkflowTaskFinder.GenerateWorkflowActivityName(Common.Enums.WorkflowActivityTypes.Ap_Activity_CreateAndModifyCandidateVersion);

                    workflowActivityRoute = sortSOApplicationManagement.GetWorkflowActivityRoute(activityName, processName);
                    return Json(new { route = workflowActivityRoute.routeUrl, dataJson = deserialize_SaveCandidateRouteCntrlModel }, JsonRequestBehavior.AllowGet);

                case 3: //SEND FOR CHECKING
                    var deserialize_CheckerUpdationCntrlModel = JsonConvert.DeserializeObject<CheckerUpdationCntrlModel>(dataModel);
                    activityName = WorkflowTaskFinder.GenerateWorkflowActivityName(Common.Enums.WorkflowActivityTypes.Ap_Activity_SendForChecking);

                    workflowActivityRoute = sortSOApplicationManagement.GetWorkflowActivityRoute(activityName, processName);
                    return Json(new { route = workflowActivityRoute.routeUrl, dataJson = deserialize_CheckerUpdationCntrlModel }, JsonRequestBehavior.AllowGet);

                case 4:// CREATE MOVEMENT VERSION
                    var deserialize_CreateMovementVersionCntrlModel = JsonConvert.DeserializeObject<CreateMovementVersionCntrlModel>(dataModel);
                    activityName = WorkflowTaskFinder.GenerateWorkflowActivityName(Common.Enums.WorkflowActivityTypes.Ap_Activity_CreateProposedMovementVersion);

                    workflowActivityRoute = sortSOApplicationManagement.GetWorkflowActivityRoute(activityName, processName);
                    return Json(new { route = workflowActivityRoute.routeUrl, dataJson = deserialize_CreateMovementVersionCntrlModel }, JsonRequestBehavior.AllowGet);

                case 6: //CREATE CANDIDATE VERSION
                    var deserialize_CreateCandidateVersionCntrlModel = JsonConvert.DeserializeObject<CreateCandidateVersionCntrlModel>(dataModel);
                    activityName = WorkflowTaskFinder.GenerateWorkflowActivityName(Common.Enums.WorkflowActivityTypes.Ap_Activity_ModifyCandidateVersion);

                    workflowActivityRoute = sortSOApplicationManagement.GetWorkflowActivityRoute(activityName, processName);
                    return Json(new { route = workflowActivityRoute.routeUrl, dataJson = deserialize_CreateCandidateVersionCntrlModel }, JsonRequestBehavior.AllowGet);

                case 7: //AGREE MOVEMENT
                    var deserialize_MovementAgreeUnagreeWithdrawCntrlModel = JsonConvert.DeserializeObject<MovementAgreeUnagreeWithdrawCntrlModel>(dataModel);
                    activityName = WorkflowTaskFinder.GenerateWorkflowActivityName(Common.Enums.WorkflowActivityTypes.Ap_Activity_AgreeMovement);

                    workflowActivityRoute = sortSOApplicationManagement.GetWorkflowActivityRoute(activityName, processName);
                    return Json(new { route = workflowActivityRoute.routeUrl, dataJson = deserialize_MovementAgreeUnagreeWithdrawCntrlModel }, JsonRequestBehavior.AllowGet);

                case 8: //COMPLETE FINAL CHECKING
                    var deserialize_CheckerUpdationCntrlModelCompleteFinal = JsonConvert.DeserializeObject<CheckerUpdationCntrlModel>(dataModel);
                    activityName = WorkflowTaskFinder.GenerateWorkflowActivityName(Common.Enums.WorkflowActivityTypes.Ap_Activity_VerifyAgreedMovement2);

                    workflowActivityRoute = sortSOApplicationManagement.GetWorkflowActivityRoute(activityName, processName);
                    return Json(new { route = workflowActivityRoute.routeUrl, dataJson = deserialize_CheckerUpdationCntrlModelCompleteFinal }, JsonRequestBehavior.AllowGet);


            }
            return Json(null);
        }
        private JsonResult GenerateVR1ProcessingRouteWorkflow(int pointOfCommunication, string dataModel)
        {
            var processName = "Process_SORTVR1ApplicationCreationV8";
            WorkflowActivityRoute workflowActivityRoute;
            var sortVr1ApplicationManagement = new SORTVR1ApplicationManagement(sortVR1ProcessingService);
            string activityName;
            switch (pointOfCommunication)
            {
                case 1: //SEND FOR CHECKING
                    var deserializeModel = JsonConvert.DeserializeObject<AllocateVr1CheckerUserCntrlModel>(dataModel);
                    activityName = WorkflowTaskFinder.GenerateWorkflowActivityName(Common.Enums.WorkflowActivityTypes.Vr_Activity_SendForChecking);

                    workflowActivityRoute = sortVr1ApplicationManagement.GetWorkflowActivityRoute(activityName, processName);
                    return Json(new { route = workflowActivityRoute.routeUrl, dataJson = deserializeModel }, JsonRequestBehavior.AllowGet);

                case 2://APPROVE VR1
                    var deserializeModel_ApproveVr1 = JsonConvert.DeserializeObject<ApproveVr1CntrlModel>(dataModel);
                    activityName = WorkflowTaskFinder.GenerateWorkflowActivityName(Common.Enums.WorkflowActivityTypes.Vr_Activity_ApproveVR1);

                    workflowActivityRoute = sortVr1ApplicationManagement.GetWorkflowActivityRoute(activityName, processName);
                    return Json(new { route = workflowActivityRoute.routeUrl, dataJson = deserializeModel_ApproveVr1 }, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }
        private JsonResult GenerateApplicationProcessingRouteWorkflow(int pointOfCommunication, string dataModel)
        {
            var processName = WorkflowProcessKey.Process_HauliersNotificationApplicationsCombined07.ToString(); //"Process_HauliersNotificationApplicationsCombined06";
            WorkflowActivityRoute workflowActivityRoute;
            var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
            string activityName;
            switch (pointOfCommunication)
            {
                case 1: //CONFIRM MOVEMENT TYPE [START PROCESS]
                    var deserializeModel = JsonConvert.DeserializeObject<InsertMovementTypeCntrlModel>(dataModel);
                    activityName = WorkflowTaskFinder.GenerateWorkflowActivityName(Common.Enums.WorkflowActivityTypes.An_Activity_ConfirmMovementType);

                    workflowActivityRoute = applicationNotificationManagement.GetWorkflowActivityRoute(activityName, processName);
                    return Json(new { route = workflowActivityRoute.routeUrl, dataJson = deserializeModel }, JsonRequestBehavior.AllowGet);

                case 2: //CHOOSE FROM ROUTE LIBRARY
                    var deserializeModel_MovementSelectRouteByImportCntrlModel = JsonConvert.DeserializeObject<MovementSelectRouteByImportCntrlModel>(dataModel);
                    activityName = WorkflowTaskFinder.GenerateWorkflowActivityName(Common.Enums.WorkflowActivityTypes.An_Activity_ChooseFromRouteLibrary);

                    workflowActivityRoute = applicationNotificationManagement.GetWorkflowActivityRoute(activityName, processName);
                    return Json(new { route = workflowActivityRoute.routeUrl, dataJson = deserializeModel_MovementSelectRouteByImportCntrlModel }, JsonRequestBehavior.AllowGet);
                //SetNotificationGeneralDetailsCntrlModel

                case 3: //NOTIFICATION GENERAL DETAILS
                    var deserializeModel_SetNotificationGeneralDetailsCntrlModel = JsonConvert.DeserializeObject<SetNotificationGeneralDetailsCntrlModel>(dataModel);
                    activityName = WorkflowTaskFinder.GenerateWorkflowActivityName(Common.Enums.WorkflowActivityTypes.An_Activity_FillInNotificationAttributesAndLoadDetails);

                    workflowActivityRoute = applicationNotificationManagement.GetWorkflowActivityRoute(activityName, processName);
                    return Json(new { route = workflowActivityRoute.routeUrl, dataJson = deserializeModel_SetNotificationGeneralDetailsCntrlModel }, JsonRequestBehavior.AllowGet);

                case 4: //THIS IS THE FIRST ACTIVITY. VEHICLE DETAILS
                    activityName = WorkflowTaskFinder.GenerateWorkflowActivityName(Common.Enums.WorkflowActivityTypes.An_Activity_VehicleDetails);

                    workflowActivityRoute = applicationNotificationManagement.GetWorkflowActivityRoute(activityName, processName);
                    return Json(new { route = workflowActivityRoute.routeUrl, dataJson = "" }, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

    }
}