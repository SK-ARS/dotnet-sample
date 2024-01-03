using STP.Common.Enums;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.Workflow;
using System.Collections.Generic;
using System.Web;

namespace STP.Web.WorkflowProvider
{
    public class SessionData
    {
        public SessionData()
        {
        }

        const string VehicleConfigurationId = "Wf_VehicleConfigurationId";
        const string FleetManagementId = "Wf_FleetManagementId";
        const string FleetManagementCurrentAction = "Wf_FleetManagementCurrentAction";
        const string FleetManagementImportFromFleet = "Wf_ImportFromFleet";

        const string ApplicationNotificationStarted = "Wf_ApplicationNotificationNew";
        const string ApplicationId = "Wf_ApplicationId";
        const string ApplicationCurrentAction = "Wf_ApplicationCurrentAction";

        const string SortSoProcessingStarted = "Wf_SortSoProcessing";
        const string SortSoProcessingCurrentAction = "Wf_SortSoProcessingCurrentAction";

        const string SortVR1ProcessingStarted = "Wf_SortVr1Processing";
        const string SortVR1ProcessingCurrentAction = "Wf_SortVr1ProcessingCurrentAction";

        const string SoaPoliceProcessingStarted = "Wf_SoaProcessing";
        const string Ev_PlanMvmntPayLoad = "Ev_PlanMvmntPayLoad";
        const string Ev_MoveTypeClass = "Ev_MoveTypeClass";


        public const string Ev_AN_VehicleList = "VehicleList";
        public const string Ev_UserInfo = "UserInfo";

        public string Wf_Fm_VehicleConfigurationId
        {
            get { return HttpContext.Current.Session[VehicleConfigurationId] != null ? (string)HttpContext.Current.Session[VehicleConfigurationId] : string.Empty; }
            set { HttpContext.Current.Session[VehicleConfigurationId] = value; }
        }
        public string Wf_Fm_FleetManagementId
        {
            get { return HttpContext.Current.Session[FleetManagementId] != null ? (string)HttpContext.Current.Session[FleetManagementId] : string.Empty; }
            set { HttpContext.Current.Session[FleetManagementId] = value; }
        }
        public WorkflowActivityTypes Wf_Fm_CurrentExecuted
        {
            get { return HttpContext.Current.Session[FleetManagementCurrentAction] != null ? (WorkflowActivityTypes)HttpContext.Current.Session[FleetManagementCurrentAction] : WorkflowActivityTypes.Gn_NotDecided; }
            set { HttpContext.Current.Session[FleetManagementCurrentAction] = value; }
        }
        public bool Wf_Fm_ImportFromFleet
        {
            get { return HttpContext.Current.Session[FleetManagementImportFromFleet] != null && (bool)HttpContext.Current.Session[FleetManagementImportFromFleet]; }
            set { HttpContext.Current.Session[FleetManagementImportFromFleet] = value; }
        }

        public bool Wf_An_ApplicationNotificationStarted
        {
            get { return HttpContext.Current.Session[ApplicationNotificationStarted] != null && (bool)HttpContext.Current.Session[ApplicationNotificationStarted]; }
            set { HttpContext.Current.Session[ApplicationNotificationStarted] = value; }
        }
        public string Wf_An_ApplicationWorkflowId
        {
            get { return HttpContext.Current.Session[ApplicationId] != null ? (string)HttpContext.Current.Session[ApplicationId] : string.Empty; }
            set { HttpContext.Current.Session[ApplicationId] = value; }
        }
        public WorkflowActivityTypes Wf_An_CurrentExecuted
        {
            get { return HttpContext.Current.Session[ApplicationCurrentAction] != null ? (WorkflowActivityTypes)HttpContext.Current.Session[ApplicationCurrentAction] : WorkflowActivityTypes.Gn_NotDecided; }
            set { HttpContext.Current.Session[ApplicationCurrentAction] = value; }
        }
        public string Wf_Ap_SortSoProcessingWorkflowId
        {
            get { return HttpContext.Current.Session[SortSoProcessingStarted] != null ? (string)HttpContext.Current.Session[SortSoProcessingStarted] : string.Empty; }
            set { HttpContext.Current.Session[SortSoProcessingStarted] = value; }
        }       
        public WorkflowActivityTypes Wf_Ap_CurrentExecuted
        {
            get { return HttpContext.Current.Session[SortSoProcessingCurrentAction] != null ? (WorkflowActivityTypes)HttpContext.Current.Session[SortSoProcessingCurrentAction] : WorkflowActivityTypes.Gn_NotDecided; }
            set { HttpContext.Current.Session[SortSoProcessingCurrentAction] = value; }
        }
        public string Wf_Ap_SOANotificationWorkflowId
        {
            get { return HttpContext.Current.Session[SoaPoliceProcessingStarted] != null ? (string)HttpContext.Current.Session[SoaPoliceProcessingStarted] : string.Empty; }
            set { HttpContext.Current.Session[SoaPoliceProcessingStarted] = value; }
        }
        public string Wf_Ap_SortVR1ProcessingWorkflowId
        {
            get { return HttpContext.Current.Session[SortVR1ProcessingStarted] != null ? (string)HttpContext.Current.Session[SortVR1ProcessingStarted] : string.Empty; }
            set { HttpContext.Current.Session[SortVR1ProcessingStarted] = value; }
        }
        public WorkflowActivityTypes Wf_Ap_VR1CurrentExecuted
        {
            get { return HttpContext.Current.Session[SortVR1ProcessingCurrentAction] != null ? (WorkflowActivityTypes)HttpContext.Current.Session[SortVR1ProcessingCurrentAction] : WorkflowActivityTypes.Gn_NotDecided; }
            set { HttpContext.Current.Session[SortVR1ProcessingCurrentAction] = value; }
        }

        public PlanMvmntPayLoad E4_AN_PlanMovement
        {
            get { return HttpContext.Current.Session[Ev_PlanMvmntPayLoad] != null ? (PlanMvmntPayLoad)HttpContext.Current.Session[Ev_PlanMvmntPayLoad] : null; }
            set { HttpContext.Current.Session[Ev_PlanMvmntPayLoad] = value; }
        }

        public VehicleMovementType E4_AN_MovemenTypeClass
        {
            get { return HttpContext.Current.Session[Ev_MoveTypeClass] != null ? (VehicleMovementType)HttpContext.Current.Session[Ev_MoveTypeClass] : null; }
            set { HttpContext.Current.Session[Ev_MoveTypeClass] = value; }
        }

    }
}