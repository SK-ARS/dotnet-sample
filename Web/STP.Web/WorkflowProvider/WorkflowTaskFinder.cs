using STP.Common.Enums;
using STP.Domain.Routes;
using STP.Domain.Workflow;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace STP.Web.WorkflowProvider
{
    public static class WorkflowTaskFinder
    {
        public static string FindNextTask(string workflowProcess, WorkflowActivityTypes workflowNextTask, out dynamic workflowPayload, bool decideNextTask = true, int taskId = -1)
        {

            switch (workflowProcess)
            {
                case "FleetManagement":
                    return FleetManagement(workflowNextTask, out workflowPayload);

                case "HaulierApplication":
                    return HaulierApplicationNextTask(workflowNextTask, out workflowPayload, taskId);

                case "SAP":
                    return SORTSOApplicationNextTask(workflowNextTask, out workflowPayload, decideNextTask);

                case "SVR1P":
                    return SORTVR1ApplicationNextTask(workflowNextTask, out workflowPayload, decideNextTask);

                case "SOANotification":
                    return SOAPoliceProcessingNextTaskFinder(workflowNextTask, out workflowPayload, decideNextTask);

                default:
                    workflowPayload = null;
                    return string.Empty;


            }
        }
        public static string FindPreviousTask(string workflowProcess, int stepBack, double subStepFlag, out dynamic workflowPayload, out string activityName)
        {

            switch (workflowProcess)
            {
                case "HaulierApplicationBack":
                    return HaulierApplicationPreviousTask(stepBack, subStepFlag, out workflowPayload, out activityName);

                case "PlanMovementDirectRoute":
                    return PlanMovementDirectPreviousRoute(stepBack, out workflowPayload, out activityName);

                case "PlanMmtNotificationDirectPreviousRoute":
                    return PlanMmtNotificationDirectPreviousRoute(stepBack, out workflowPayload, out activityName);

                default:
                    workflowPayload = null;
                    activityName = string.Empty;
                    return string.Empty;


            }
        }
        public static string WorkflowSoaPoliceNotificationNumberBuilder(string notificationNumber, bool isPolice)
        {
            return isPolice ? $"police_{notificationNumber}" : $"soa_{notificationNumber}";
        }
        public static string WorkflowEsdalReferenceNumberBuilder(string esdalReferenceNumber, bool isVr1 = false)
        {
            if (esdalReferenceNumber == "_0" || esdalReferenceNumber.Length <= 2)
            {
                return "";
            }
            var resultEsdalReferenceNumber = esdalReferenceNumber.Replace("/", "_").Replace(" ", "");
            return isVr1 ? $"VR1_{resultEsdalReferenceNumber}" : resultEsdalReferenceNumber;
        }
        public static string GenerateEsdalReferenceNumber(string haulnemonic, string referenecNumber)
        {

            return $"{haulnemonic}_{referenecNumber}";
        }
        public static string SORTSOProcessingNextTaskFinder(string activity)
        {
            if (activity == GenerateWorkflowActivityName(WorkflowActivityTypes.Ap_Activity_AllocateApplication2RoutingOfficers) || activity == GenerateWorkflowActivityName(WorkflowActivityTypes.Ap_Activity_OpenSOApplication))
            {
                return "CREATE CANDIDATE ROUTE VERSION";
            }
            else if (activity == GenerateWorkflowActivityName(WorkflowActivityTypes.Ap_Activity_CreateAndModifyCandidateVersion))
            {
                return "SEND FOR CHECKING";
            }
            else if (activity == GenerateWorkflowActivityName(WorkflowActivityTypes.Ap_Activity_SendForChecking))
            {
                return "COMPLETE CHECKING";
            }
            else if (activity == GenerateWorkflowActivityName(WorkflowActivityTypes.Ap_Activity_VerifyCandidateVersion))
            {
                return "CREATE MOVEMENT VERSION";
            }
            else if (activity == GenerateWorkflowActivityName(WorkflowActivityTypes.Ap_Activity_CreateProposedMovementVersion))
            {
                return "DISTRUBUTE MOVEMENT";
            }
            else if (activity == GenerateWorkflowActivityName(WorkflowActivityTypes.Ap_Activity_DistributeMovement))
            {
                return "AGREE";
            }
            else if (activity == GenerateWorkflowActivityName(WorkflowActivityTypes.Ap_Activity_AgreeMovement))
            {
                return "SEND FOR QA CHECKING";
            }
            else if (activity == GenerateWorkflowActivityName(WorkflowActivityTypes.Ap_Activity_SendForQAChecking))
            {
                return "COMPLETE QA CHECKING";
            }
            else if (activity == GenerateWorkflowActivityName(WorkflowActivityTypes.Ap_Activity_VerifyAgreedMovement))
            {
                return "SEND FOR FINAL CHECKING";
            }
            else if (activity == GenerateWorkflowActivityName(WorkflowActivityTypes.Ap_Activity_SendForFinalChecking))
            {
                return "COMPLETE FINAL CHECKING";
            }
            else if (activity == GenerateWorkflowActivityName(WorkflowActivityTypes.Ap_Activity_VerifyAgreedMovement2))
            {
                return "CREATE SPECIAL ORDER";
            }
            else if (activity == GenerateWorkflowActivityName(WorkflowActivityTypes.Ap_Activity_GenerateSpecialOrderNumber))
            {
                return "DISTRIBUTE AGREED MOVEMENTS";
            }
            else if (activity == GenerateWorkflowActivityName(WorkflowActivityTypes.Ap_Activity_DistributeAgreedMovementVersion))
            {
                return "TASK COMPLETED";
            }
            return "";
        }
        public static string SORTVR1ProcessingNextTaskFinder(string activity)
        {
            if (activity == GenerateWorkflowActivityName(WorkflowActivityTypes.Vr_Activity_AllocateApplication2RoutingOfficers) || activity == GenerateWorkflowActivityName(WorkflowActivityTypes.Vr_Activity_OpenVR1Application))
            {
                return "SEND FOR CHECKING";
            }
            else if (activity == GenerateWorkflowActivityName(WorkflowActivityTypes.Vr_Activity_SendForChecking))
            {
                return "COMPLETE CHECKING";
            }
            else if (activity == GenerateWorkflowActivityName(WorkflowActivityTypes.Vr_Activity_ReviewMovementVersion2))
            {
                return "GENERATE VR1";
            }
            return "NF";
        }
        public static string SOAPoliceProcessingNextTaskFinder(string activity)
        {
            if (activity == GenerateWorkflowActivityName(WorkflowActivityTypes.Sp_Activity_SelectaNotification) || activity == GenerateWorkflowActivityName(WorkflowActivityTypes.Sp_Activity_OpenMovementInbox))
            {
                return "NOTIFICATION SELECTED";
            }
            else if (activity == GenerateWorkflowActivityName(WorkflowActivityTypes.Vr_Activity_SendForChecking))
            {
                return "COMPLETE CHECKING";
            }
            else if (activity == GenerateWorkflowActivityName(WorkflowActivityTypes.Vr_Activity_ReviewMovementVersion2))
            {
                return "GENERATE VR1";
            }
            return "NF";
        }
        public static WorkflowActivityTypes SORTSOSetActivityByStatus(int status)
        {
            switch (status)
            {
                case 301002:
                    return WorkflowActivityTypes.Ap_Activity_SendForChecking;
                case 301003:
                    return WorkflowActivityTypes.Ap_Activity_VerifyCandidateVersion;
                case 301005:
                    return WorkflowActivityTypes.Ap_Activity_SendForFinalChecking;
                case 301006:
                    return WorkflowActivityTypes.Ap_Activity_VerifyAgreedMovement2;
                case 301007:
                    return WorkflowActivityTypes.Ap_Activity_VerifyAgreedMovement;
                case 301008:
                    return WorkflowActivityTypes.Ap_Activity_SendForQAChecking;
                case 301009:
                    return WorkflowActivityTypes.Ap_Activity_VerifyAgreedMovement;
                case 301010:
                    return WorkflowActivityTypes.Ap_Activity_AgreeMovement;
            }

            return WorkflowActivityTypes.Gn_NotDecided;
        }
        public static WorkflowActivityTypes SORTVR1SetActivityByStatus(int status)
        {
            switch (status)
            {
                case 301002:
                    return WorkflowActivityTypes.Vr_Activity_SendForChecking;
                case 301003:
                    return WorkflowActivityTypes.Vr_Activity_ReviewMovementVersion2;
            }
            return WorkflowActivityTypes.Gn_NotDecided;
        }

        #region Fleet Management
        private static string FleetManagement(WorkflowActivityTypes workflowNextTask, out dynamic workflowPayload)
        {
            var workflowNextActivity = "Activity_ChooseVehicleConfigurationType";
            workflowPayload = new ExpandoObject();
            switch (workflowNextTask)
            {
                case WorkflowActivityTypes.Fm_ManualEntryComponent:
                    workflowNextActivity = "Activity_ChooseVehicleConfigurationType";
                    break;
                case WorkflowActivityTypes.Fm_Activity_ChooseVehicleConfigurationType:
                    workflowNextActivity = "Activity_ChooseVehicleConfigurationType";
                    break;
                case WorkflowActivityTypes.Fm_Activity_ChooseMovementType:
                    workflowNextActivity = "Activity_ChooseMovementType";
                    break;
                case WorkflowActivityTypes.Fm_Activity_VehicleDetailsEntry:
                    workflowNextActivity = "Activity_EnterVehicleDetails";
                    break;
            }
            return workflowNextActivity;
        }
        #endregion Fleet Management

        #region Application Submit
        private static string HaulierApplicationNextTask(WorkflowActivityTypes workflowNextTask, out dynamic workflowPayload, int taskId = -1)
        {
            string workflowActivityName;
            switch (workflowNextTask)
            {
                case WorkflowActivityTypes.An_Activity_ConfirmMovementType:
                    workflowActivityName = GenerateWorkflowActivityName(workflowNextTask);
                    workflowPayload = new ExpandoObject();
                    workflowPayload.decideNextTask = true;
                    workflowPayload.nextTaskKey = workflowActivityName;
                    workflowPayload.taskId = 3;
                    return workflowActivityName;

                case WorkflowActivityTypes.An_Activity_ChooseFromRouteLibrary:
                case WorkflowActivityTypes.An_Activity_PlanRouteOnMap:
                case WorkflowActivityTypes.An_Activity_ImportFromPreviousovement_Route:
                case WorkflowActivityTypes.An_Activity_VehicleAddUpdate:
                    workflowActivityName = GenerateWorkflowActivityName(workflowNextTask);
                    workflowPayload = new ExpandoObject();
                    workflowPayload.decideNextTask = true;
                    workflowPayload.nextTaskKey = workflowActivityName;
                    workflowPayload.assignRouteAuto = false;
                    return workflowActivityName;

                case WorkflowActivityTypes.An_Activity_ApplicationAttributes:
                case WorkflowActivityTypes.An_Activity_AdditionalAffectedPartiesManualEntry:
                    workflowActivityName = GenerateWorkflowActivityName(workflowNextTask);
                    workflowPayload = new ExpandoObject();
                    workflowPayload.decideNextTask = true;
                    workflowPayload.nextTaskKey = workflowActivityName;
                    return workflowActivityName;

                              
                case WorkflowActivityTypes.TheEnd:
                    workflowActivityName = GenerateWorkflowActivityName(workflowNextTask);
                    workflowPayload = new ExpandoObject();
                    workflowPayload.decideNextTask = false;
                    workflowPayload.nextTaskKey = workflowActivityName;
                    return workflowActivityName;

                case WorkflowActivityTypes.An_Activity_SupplementaryDetails:
                    workflowActivityName = GenerateWorkflowActivityName(workflowNextTask);
                    workflowPayload = new ExpandoObject();
                    workflowPayload.decideNextTask = true;                   
                    workflowPayload.taskId = taskId == -1 ? 5.0 : 5.1;
                    workflowPayload.IsSupplimentarySaved = taskId != -1;
                    workflowPayload.nextTaskKey = workflowActivityName;
                    return workflowActivityName;

                case WorkflowActivityTypes.An_Activity_SubmitApplication:
                    workflowActivityName = GenerateWorkflowActivityName(workflowNextTask);
                    workflowPayload = new ExpandoObject();
                    workflowPayload.decideNextTask = true;
                    workflowPayload.taskId = taskId == -1 ? 6.0 : 6.1;
                    workflowPayload.IsSoOverView = taskId != -1;
                    workflowPayload.nextTaskKey = workflowActivityName;
                    return workflowActivityName;

                case WorkflowActivityTypes.An_Activity_FillInNotificationAttributesAndLoadDetails:

                case WorkflowActivityTypes.An_Activity_AcceptTermsAndConditions:
                
                    workflowActivityName = GenerateWorkflowActivityName(workflowNextTask);
                    workflowPayload = new ExpandoObject();
                    workflowPayload.decideNextTask = true;
                    workflowPayload.nextTaskKey = workflowActivityName;
                    return workflowActivityName;

                default:
                    workflowPayload = null;
                    return string.Empty;
            }

        }
        private static string PlanMmtCommonPreviousRoute(int stepBack, out dynamic workflowPayload, out string activityName)
        {
            string workflowActivityName;
            switch (stepBack)
            {

                case 1:
                    var goToActivity_1 = GenerateWorkflowActivityName(WorkflowActivityTypes.An_Activity_VehicleAddUpdate);
                    workflowActivityName = goToActivity_1;
                    activityName = $"Back_{goToActivity_1}";
                    workflowPayload = new ExpandoObject();
                    workflowPayload.decideNextTask = true;
                    workflowPayload.nextTaskKey = workflowActivityName;
                    return workflowActivityName;
                case 2:
                    var goToActivity_2 = GenerateWorkflowActivityName(WorkflowActivityTypes.An_Activity_VehicleDetails);
                    workflowActivityName = goToActivity_2;
                    activityName = $"Back_{goToActivity_2}";
                    workflowPayload = new ExpandoObject();
                    workflowPayload.decideNextTask = true;
                    workflowPayload.nextTaskKey = workflowActivityName;
                    return workflowActivityName;

                case 3:
                    var goToActivity_3 = GenerateWorkflowActivityName(WorkflowActivityTypes.An_Activity_ConfirmMovementType);
                    workflowActivityName = goToActivity_3;
                    activityName = $"Back_{goToActivity_3}";
                    workflowPayload = new ExpandoObject();
                    workflowPayload.decideNextTask = true;
                    workflowPayload.nextTaskKey = workflowActivityName;
                    return workflowActivityName;


                //4 APPLn : Move to confirm movement type
                case 4:
                    var goToActivity_4 = GenerateWorkflowActivityName(WorkflowActivityTypes.An_Activity_PlanRouteOnMap);
                    workflowActivityName = goToActivity_4;
                    activityName = $"Back_{goToActivity_4}";
                    workflowPayload = new ExpandoObject();
                    workflowPayload.decideNextTask = true;
                    workflowPayload.nextTaskKey = workflowActivityName;
                    return workflowActivityName;

                default:
                    workflowPayload = null;
                    activityName = "Unknown Back Event";
                    return string.Empty;
            }

        }
        private static string PlanMovementDirectPreviousRoute(int stepBack, out dynamic workflowPayload, out string activityName)
        {
            string workflowActivityName;
            switch (stepBack)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    return PlanMmtCommonPreviousRoute(stepBack, out workflowPayload, out activityName);

                //5 APPLn : Move to Route selection screen
                case 5:
                    var goToActivity_5 = GenerateWorkflowActivityName(WorkflowActivityTypes.An_Activity_ApplicationAttributes);
                    workflowActivityName = goToActivity_5;
                    activityName = $"Back_{goToActivity_5}";
                    workflowPayload = new ExpandoObject();
                    workflowPayload.decideNextTask = true;
                    workflowPayload.nextTaskKey = workflowActivityName;
                    return workflowActivityName;
                default:
                    workflowPayload = null;
                    activityName = "Unknown Back Event";
                    return string.Empty;
            }

        }
        private static string PlanMmtNotificationDirectPreviousRoute(int stepBack, out dynamic workflowPayload, out string activityName)
        {
            string workflowActivityName;
            switch (stepBack)
            {

                case 1:
                case 2:
                case 3:
                case 4:
                    return PlanMmtCommonPreviousRoute(stepBack, out workflowPayload, out activityName);

                //5 APPLn : Move to Route selection screen
                case 5:
                    var goToActivity_5 = GenerateWorkflowActivityName(WorkflowActivityTypes.An_Activity_AdditionalAffectedPartiesManualEntry);
                    workflowActivityName = goToActivity_5;
                    activityName = $"Back_{goToActivity_5}";
                    workflowPayload = new ExpandoObject();
                    workflowPayload.decideNextTask = true;
                    workflowPayload.nextTaskKey = workflowActivityName;
                    return workflowActivityName;
                default:
                    workflowPayload = null;
                    activityName = "Unknown Back Event";
                    return string.Empty;
            }

        }
        private static string HaulierApplicationPreviousTask(int stepBack, double substepFlag, out dynamic workflowPayload, out string activityName)
        {
            string workflowActivityName;
            switch (stepBack)
            {
                case 2:
                    var goToActivity_2 = GenerateWorkflowActivityName(WorkflowActivityTypes.An_Activity_VehicleAddUpdate);
                    workflowActivityName = goToActivity_2;
                    activityName = $"Back_{goToActivity_2}";
                    workflowPayload = new ExpandoObject();
                    workflowPayload.decideNextTask = true;
                    workflowPayload.taskId = 1;
                    workflowPayload.nextTaskKey = workflowActivityName;
                    return workflowActivityName;

                case 3:
                    var goToActivity_3 = GenerateWorkflowActivityName(WorkflowActivityTypes.An_Activity_VehicleDetails);
                    workflowActivityName = goToActivity_3;
                    activityName = $"Back_{goToActivity_3}";
                    workflowPayload = new ExpandoObject();
                    workflowPayload.taskId = 2;
                    workflowPayload.decideNextTask = true;
                    workflowPayload.nextTaskKey = workflowActivityName;
                    return workflowActivityName;


                //4 APPLn : Move to confirm movement type
                case 4:
                    if (substepFlag == 4.1)
                    {
                        var goToActivity_4_1 = GenerateWorkflowActivityName(WorkflowActivityTypes.An_Activity_PlanRouteOnMap);
                        workflowActivityName = goToActivity_4_1;
                        activityName = $"Back_{goToActivity_4_1}";
                        workflowPayload = new ExpandoObject();
                        workflowPayload.decideNextTask = true;
                        workflowPayload.taskId = 4;
                        workflowPayload.nextTaskKey = workflowActivityName;
                        return workflowActivityName;
                    }
                    var goToActivity_4 = GenerateWorkflowActivityName(WorkflowActivityTypes.An_Activity_ConfirmMovementType);
                    workflowActivityName = goToActivity_4;
                    activityName = $"Back_{goToActivity_4}";
                    workflowPayload = new ExpandoObject();
                    workflowPayload.decideNextTask = true;
                    workflowPayload.taskId = 3;
                    workflowPayload.nextTaskKey = workflowActivityName;
                    return workflowActivityName;

                //5 APPLn : Move to Route selection screen
                case 5:
                    var goToActivity_5 = GenerateWorkflowActivityName(WorkflowActivityTypes.An_Activity_PlanRouteOnMap);
                    workflowActivityName = goToActivity_5;
                    activityName = $"Back_{goToActivity_5}";
                    workflowPayload = new ExpandoObject();
                    workflowPayload.decideNextTask = true;
                    workflowPayload.nextTaskKey = workflowActivityName;
                    workflowPayload.taskId = 4;
                    return workflowActivityName;


                //6 APPLn : Move to dislay of application overview page.
                case 6:
                    var goToActivity_6 = GenerateWorkflowActivityName(WorkflowActivityTypes.An_Activity_ApplicationAttributes);
                    workflowActivityName = goToActivity_6;
                    activityName = $"Back_{goToActivity_6}";
                    workflowPayload = new ExpandoObject();
                    workflowPayload.decideNextTask = true;
                    workflowPayload.nextTaskKey = workflowActivityName;
                    workflowPayload.taskId = 5.1;
                    return workflowActivityName;

                default:
                    workflowPayload = null;
                    activityName = "Unknown Back Event";
                    return string.Empty;
            }

        }
        #endregion Application Submit

        #region SORT SO Application Processing
        private static string SORTSOApplicationNextTask(WorkflowActivityTypes workflowNextTask, out dynamic workflowPayload, bool decideNextTask = true)
        {
            string workflowActivityName;
            switch (workflowNextTask)
            {
                case WorkflowActivityTypes.Ap_Activity_AddAnnotation:
                case WorkflowActivityTypes.Ap_Activity_AddAnnotation2:
                case WorkflowActivityTypes.Ap_Activity_AddAnnotation3:
                case WorkflowActivityTypes.Ap_Activity_AddEditNotes2Haulier:
                case WorkflowActivityTypes.Ap_Activity_AddEditNotesToHaulier:
                case WorkflowActivityTypes.Ap_Activity_AdditionalAffectedPartiesManualEntry:
                case WorkflowActivityTypes.Ap_Activity_AdditionalAffectedPartiesManualEntry2:
                case WorkflowActivityTypes.Ap_Activity_AdditionalAffectedPartiesManualEntry3:
                case WorkflowActivityTypes.Ap_Activity_AdditionalAffectedPartiesManualEntry4:
                case WorkflowActivityTypes.Ap_Activity_AdditionalAffectedPartiesManualEntry5:
                case WorkflowActivityTypes.Ap_Activity_AdditionalAffectedPartiesManualEntry6:
                case WorkflowActivityTypes.Ap_Activity_AgreeMovement:
                case WorkflowActivityTypes.Ap_Activity_AllocateApplication2RoutingOfficers:
                case WorkflowActivityTypes.Ap_Activity_CreateAndModifyCandidateVersion:
                case WorkflowActivityTypes.Ap_Activity_CreateProposedMovementVersion:
                case WorkflowActivityTypes.Ap_Activity_DigitalSigningOfSOdocument:
                case WorkflowActivityTypes.Ap_Activity_DistributeAgreedMovementVersion:
                case WorkflowActivityTypes.Ap_Activity_EditNotesToHaulier:
                case WorkflowActivityTypes.Ap_Activity_GenerateDrivingInstruction:
                case WorkflowActivityTypes.Ap_Activity_GenerateDrivingInstruction2:
                case WorkflowActivityTypes.Ap_Activity_GenerateDrivingInstruction3:
                case WorkflowActivityTypes.Ap_Activity_GenerateSpecialOrderNumber:
                case WorkflowActivityTypes.Ap_Activity_ModifyCandidateVersion:
                case WorkflowActivityTypes.Ap_Activity_ModifyCandidateVersion2:
                case WorkflowActivityTypes.Ap_Activity_OpenSOApplication:
                case WorkflowActivityTypes.Ap_Activity_RouteAssessment:
                case WorkflowActivityTypes.Ap_Activity_RouteAssessment2:
                case WorkflowActivityTypes.Ap_Activity_SendForChecking:
                case WorkflowActivityTypes.Ap_Activity_SendForFinalChecking:
                case WorkflowActivityTypes.Ap_Activity_SendForQAChecking:
                case WorkflowActivityTypes.Ap_Activity_StoreDigitallySignedSODocumentAsPDF:
                case WorkflowActivityTypes.Ap_Activity_Unagree:
                case WorkflowActivityTypes.Ap_Activity_VerifyAgreedMovement:
                case WorkflowActivityTypes.Ap_Activity_VerifyAgreedMovement2:
                case WorkflowActivityTypes.Ap_Activity_VerifyCandidateVersion:
                case WorkflowActivityTypes.Ap_Activity_ViewAndPrintRouteContactList:

                case WorkflowActivityTypes.TheEnd:
                    workflowActivityName = GenerateWorkflowActivityName(workflowNextTask);
                    workflowPayload = new ExpandoObject();
                    workflowPayload.decideNextTask = decideNextTask;
                    workflowPayload.nextTaskKey = workflowActivityName;
                    return workflowActivityName;
                default:
                    workflowPayload = null;
                    return string.Empty;
            }

        }
        #endregion SORT SO Application Processing

        #region SORT VR1 Application Processing
        private static string SORTVR1ApplicationNextTask(WorkflowActivityTypes workflowNextTask, out dynamic workflowPayload, bool decideNextTask = true)
        {
            string workflowActivityName;
            switch (workflowNextTask)
            {

                case WorkflowActivityTypes.Vr_Activity_AllocateApplication2RoutingOfficers:
                case WorkflowActivityTypes.Vr_Activity_SendForChecking:
                case WorkflowActivityTypes.Vr_Activity_ReviewMovementVersion2:
                case WorkflowActivityTypes.Vr_Activity_ApproveVR1:

                case WorkflowActivityTypes.TheEnd:

                    workflowActivityName = GenerateWorkflowActivityName(workflowNextTask);
                    workflowPayload = new ExpandoObject();
                    workflowPayload.decideNextTask = decideNextTask;
                    workflowPayload.nextTaskKey = workflowActivityName;
                    return workflowActivityName;

                default:
                    workflowPayload = null;
                    return string.Empty;
            }

        }

        #endregion SORT VR1 Application Processing

        #region SOA Police Notification Processing
        private static string SOAPoliceProcessingNextTaskFinder(WorkflowActivityTypes workflowNextTask, out dynamic workflowPayload, bool decideNextTask = true)
        {
            string workflowActivityName;
            switch (workflowNextTask)
            {

                case WorkflowActivityTypes.Sp_Activity_OpenMovementInbox:
                case WorkflowActivityTypes.Sp_Activity_SelectaNotification:
                case WorkflowActivityTypes.Sp_Activity_UnderAssessment:
                case WorkflowActivityTypes.Sp_Activity_Reject:
                case WorkflowActivityTypes.Sp_Activity_Accept:
                case WorkflowActivityTypes.Sp_Activity_AssigntoAnotherOperator:

                case WorkflowActivityTypes.TheEnd:

                    workflowActivityName = GenerateWorkflowActivityName(workflowNextTask);
                    workflowPayload = new ExpandoObject();
                    workflowPayload.decideNextTask = decideNextTask;
                    workflowPayload.nextTaskKey = workflowActivityName;
                    return workflowActivityName;

                default:
                    workflowPayload = null;
                    return string.Empty;
            }

        }
        #endregion SOA Police Notification Processing

        public static string GenerateWorkflowActivityName(WorkflowActivityTypes workflowNextTask)
        {
            var activityName = workflowNextTask.ToString();
            activityName = activityName.Substring(activityName.IndexOf('_') + 1, activityName.Length - (activityName.IndexOf('_') + 1));
            return activityName;
        }
    }
}