using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Common.Enums
{
    public enum WorkflowActivityTypes
    {
        [Description("Gn_NotDecided")] Gn_NotDecided, // Before starting a new vehicle configuration creation
        TheEnd,
        #region Fleet Management
        [Description("Fm_ManualEntryComponent")] Fm_ManualEntryComponent, // When a new configuration is started with manual entry.         
        [Description("Fm_Activity_ChooseVehicleConfigurationType")] Fm_Activity_ChooseVehicleConfigurationType,
        [Description("Fm_Activity_ChooseMovementType")] Fm_Activity_ChooseMovementType,
        [Description("Fm_Activity_VehicleDetailsEntry")] Fm_Activity_VehicleDetailsEntry,
        [Description("Fm_ConfigurationCompleted")] Fm_ConfigurationCompleted,
        #endregion Fleet Management

        #region Haulier Application & Notifications
        An_Activity_VehicleAddUpdate,
        An_Activity_VehicleDetails,
        An_Activity_ConfirmMovementType,
        An_Activity_PlanRouteOnMap,
        An_Activity_ChooseFromRouteLibrary,
        An_Activity_ImportFromPreviousovement_Route,

        #region ApplicationOnly
        An_Activity_SupplementaryDetails,
        An_Activity_ApplicationAttributes,
        An_Activity_SubmitApplication,
        #endregion ApplicationOnly

        #region Notifications Only       
        An_Activity_AdditionalAffectedPartiesManualEntry,
        An_Activity_FillInNotificationAttributesAndLoadDetails,
        An_Activity_AcceptTermsAndConditions,
        #endregion Notifications Only

        #endregion Haulier Application & Notifications

        #region SORT SO Application Processing
        Ap_Activity_VerifyAgreedMovement2,
        Ap_Activity_DigitalSigningOfSOdocument,
        Ap_Activity_StoreDigitallySignedSODocumentAsPDF,
        Ap_Activity_GenerateSpecialOrderNumber,
        Ap_Activity_DistributeAgreedMovementVersion,
        Ap_Activity_ViewAndPrintRouteContactList,
        Ap_Activity_AddEditNotesToHaulier,
        Ap_Activity_VerifyAgreedMovement,
        Ap_Activity_SendForFinalChecking,
        Ap_Activity_AdditionalAffectedPartiesManualEntry2,
        Ap_Activity_ModifyCandidateVersion,
        Ap_Activity_CreateAndModifyCandidateVersion,
        Ap_Activity_OpenSOApplication,
        Ap_Activity_AllocateApplication2RoutingOfficers,
        Ap_Activity_DistributeMovement,
        Ap_Activity_ModifyCandidateVersion2,
        Ap_Activity_VerifyCandidateVersion,
        Ap_Activity_SendForChecking,
        Ap_Activity_AgreeMovement,
        Ap_Activity_CreateProposedMovementVersion,
        Ap_Activity_Unagree,
        Ap_Activity_AdditionalAffectedPartiesManualEntry,
        Ap_Activity_AdditionalAffectedPartiesManualEntry3,
        Ap_Activity_GenerateDrivingInstruction,
        Ap_Activity_AdditionalAffectedPartiesManualEntry5,
        Ap_Activity_AddEditNotes2Haulier,
        Ap_Activity_AdditionalAffectedPartiesManualEntry6,
        Ap_Activity_EditNotesToHaulier,
        Ap_Activity_GenerateDrivingInstruction3,
        Ap_Activity_AddAnnotation3,
        Ap_Activity_SendForQAChecking,
        Ap_Activity_AddAnnotation,
        Ap_Activity_AdditionalAffectedPartiesManualEntry4,
        Ap_Activity_GenerateDrivingInstruction2,
        Ap_Activity_AddAnnotation2,
        Ap_Activity_RouteAssessment,
        Ap_Activity_RouteAssessment2,
        #endregion SORT SO Application Processing

        #region SORT VR1 Application Processing
        Vr_Activity_AllocateApplication2RoutingOfficers,
        Vr_Activity_OpenVR1Application,
        Vr_Activity_GenerateMovementVersion,
        Vr_Activity_SendForChecking,
        Vr_Activity_ReviewMovementVersion2,
        Vr_Activity_ApproveVR1,
        Vr_Activity_GenerateVR1document,
        Vr_Activity_DistributeVR1Movement,
        #endregion SORT VR1 Application Processing

        #region SOA Police Notification
        Sp_Activity_SendautoresponsetoHaulierEmail,
        Sp_Activity_ValidateNotification,
        Sp_Activity_OpenMovementInbox,
        Sp_Activity_SelectaNotification,
        Sp_Activity_UnderAssessment,
        Sp_Activity_Reject,
        Sp_Activity_Accept,
        Sp_Activity_SelectNotesfromLibrary,
        Sp_Activity_AddCollaborationNotes,
        Sp_Activity_AssigntoAnotherOperator
        #endregion SOA Police Notification

    }
    public enum WorkflowMaster
    {
        HaulierApplication,
        FleetManagement,
        SORTSOProcessing
    }
    public enum WorkflowFleetMgmtFlowTypes
    {
        Vehicle,
        VehicleConfig
    }
}
