using STP.Common.Constants;
using STP.Domain.SecurityAndUsers;
using STP.Domain.MovementsAndNotifications.Notification;
using System.Collections.Generic;
using STP.Domain.Communications;
using STP.Domain.DocumentsAndContents;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.NonESDAL;

namespace STP.ServiceAccess.MovementsAndNotifications
{
    public interface INotificationService
    {
        int InsertQuickLink(int userId, int projectId, int notificationId, int revisionId, int versionId);
        bool IsAcknowledged(string esdalRefernce, int historic);
        string CheckIfNotificationSubmitted(int NotificationId);
        CollaborationModel GetUnacknowledgedCollaboration(string Notification_Code, int historic);
        NotificationGeneralDetails GetNotificationGeneralDetail(long notificationId, int historic);
        int DeleteNotification(int notificationId);
        byte[] GetAffectedParties(int notificationId, string userSchema = UserSchema.Portal);
        MailResponse GetResponseMailDetails(int orgId, string userSchema = UserSchema.Portal);
        int ShowImminentMovement(string moveStartDate, string countryId, int countryIdCount, int vehicleClass);
        bool SaveAffectedNotificationDetails(AffectedStructConstrParam affectedParam);
        int UpdateNotification(NotificationGeneralDetails notificationGeneralDetails);
        NotificationGeneralDetails NotifyApplication(long versionId);
        bool ManageCollaborationInternal(NotificationStatusModel notificationStatusModel, string userSchema);
        int CheckNotificationVersion(int NotificationId);
        string GenerateNotificationCode(int OrgId, long NotificationId, int Detail);
        bool UpdateCollborationAck(long docId, int colNo, int userId, string acknowledgeAgainst, int historic);
        List<CollaborationModel> GetCollaborationList(int pageNumber, int pageSize, string notificationCode, int notificationId, int historic);
        List<NotificationStatusModel> GetNotificationStatusList(int pageNumber, int pageSize, string NotificationCode, string userSchema);
        NotificationGeneralDetails RenotifyNotification(int notifId, int VR1);
        NotificationGeneralDetails CloneNotification(int notificationId);
        NotificationGeneralDetails CloneHistoricNotification(int notificationId);
        List<TransmissionModel> GetTransmissionList(GetTransmissionListParams getTransmissionList);
        List<InboxSubContent> GetInboxSubContent(int pageNumber, int pageSize,int versionId, int orgId, int notifhistory);
        List<InboxSubContent> GetSORTHistoryDetails(string esdalref, int versionno);
        List<CollaborationModel> GetExternalCollaboration(int pageNumber, int pageSize, int Document_Id, string userSchema);
        NotifGeneralDetails InsertNotificationType(PlanMovementType saveNotification);
        NotifGeneralDetails UpdateNotificationType(PlanMovementType updateNotification);
        HAContact GetHaulierDetails(long notificationId);
        int SetLoginStatus(int UserId, int flag);
        NotificationStatusModel GetInternalCollaboration(NotificationStatusModel notificationStatusModel, string userSchema);
        void UpdateNenApiIcaStatus(UpdateNENICAStatusParams updateNENICAStatusParams);

        #region Removed Unwanted code by Mahzeer on 04-12-2023
        /*int GetGrossWeight(int notifId);
        decimal GetMaxReduciableHeight(int NotificationId);
        List<ListRouteVehicleId> GetNotificationRouteDetails(string ContentRefNo);
        NotificationXSD.OutboundNotificationStructure GenerateOutboundNotificationStructureData(long NotificationId);
        OutboundDocuments GetOutboundDoc(int notificationID);
        long GetPreviousAnalysisId(long notificationId);
        List<CollaborationModel> PrintCollabrationList(string notificationCode, int notificationId);
        string GetHaulierLicence(int orgId);
        bool SetNotificationVRNum(int notificationId);
        UserRegistration GetNotifHaulierDetails(int userId, int notificationId = 0, int vehicleClassCode = 0);
        NotificationGeneralDetails SaveNotifGeneralDetails(NotificationGeneralDetails notificationGeneralDetails);
        NotificationGeneralDetails CheckNotifValidation(string contentReferenceNo);
        int IsNotifSubmitCheck(long notificationID);
        List<AxleDetails> ListCloneAxelDetails(int vehicleId);
        List<AxleDetails> ListAxelDetails(int vehicleId);
        int GetVehicleTypeForNotif(int notificationId);
        int UpdateInboundNotif(int notificationId, byte[] inboundNotif);
        GetImminentChkDetailsDomain GetDetailsToChkImminent(long notificationId, string Content_Ref_No, long revision_id, string UserSchema);
        int CheckImminent(int vehicleclass, decimal vehiWidth, decimal vehiLength, decimal rigidLength, decimal GrossWeight, int WorkingDays, decimal FrontPRJ, decimal RearPRJ, decimal LeftPRJ, decimal RightPRJ, GetImminentChkDetailsDomain objImminent, string Notif_type = null);
        byte[] GetInboundNotification(int NotificationId);*/
        #endregion
    }
}
