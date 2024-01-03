using System.Collections.Generic;
using STP.Domain.DocumentsAndContents;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.SecurityAndUsers;

namespace STP.MovementsAndNotifications.Interface
{
    public interface INotificationProvider
    {
        bool IsAcknowledged(string esdalRefernce, int historic);
        CollaborationModel GetUnacknowledgedCollaboration(string notificationCode, int historic);
        string CheckNotificationSubmitted(int notificationId);
        bool UpdateCollborationAcknowledgement(long docId, int colNo, int userId, string acknowledgeAgainst, int historic);
        List<NotificationStatusModel> GetNotificationStatusList(int pageNumber, int pageSize, string NotificationCode, string userSchema);
        List<CollaborationModel> GetCollaborationList(int pageNumber, int pageSize, string notificationCode, int notificationId, int historic);
        List<TransmissionModel> GetTransmissionList(GetTransmissionListParams getTransmissionList);
        NotificationStatusModel GetInternalCollaboration(NotificationStatusModel notificationStatusModel, string userSchema);
        bool ManageCollaborationInternal(NotificationStatusModel notificationStatus, string userSchema);
        List<CollaborationModel> GetExternalCollaboration(int pageNumber, int pageSize, int Document_Id, string userSchema);
        NotifGeneralDetails InsertNotificationType(PlanMovementType saveNotifType);
        NotifGeneralDetails UpdateNotificationType(PlanMovementType updateNotifType);
        int SetLoginStatus(int UserId, int flag);
        Domain.ExternalAPI.ExportNotifGeneralDetails ExportNotifGeneralDetails(string esdalRefNumber);
        HAContact GetHaulierDetails(long notificationId);

        #region Removed Unwanted code by Mahzeer on 04-12-2023
        //string GetHaulierLicence(int organisationId);
        //List<ListRouteVehicleId> GetNotifcationRouteDetails(string contentReferenceNo);
        //List<CollaborationModel> PrintCollabrationList(string notificationCode, int notificationId);
        #endregion


    }
}