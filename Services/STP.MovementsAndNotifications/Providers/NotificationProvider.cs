using STP.MovementsAndNotifications.Interface;
using STP.MovementsAndNotifications.Persistance;
using System.Collections.Generic;
using System.Diagnostics;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.DocumentsAndContents;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.SecurityAndUsers;

namespace STP.MovementsAndNotifications.Providers
{
    public sealed class NotificationProvider : INotificationProvider
    {
        #region MovementProvider Singleton

        private NotificationProvider()
        {
        }
        public static NotificationProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }

        /// <summary>
        /// Not to be called while using logic
        /// </summary>
        internal static class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly NotificationProvider instance = new NotificationProvider();
        }
        #endregion

        #region Get IsAcknowledge
        public bool IsAcknowledged(string esdalRefernce, int historic)
        {
            return NotificationDAO.GetIsAcknowledge(esdalRefernce, historic);
        }
        #endregion

        #region Get unacknowledged Collaboration
        public CollaborationModel GetUnacknowledgedCollaboration(string notificationCode, int historic)
        {
            return NotificationDAO.GetUnacknowledgedCollaboration(notificationCode, historic);
        }
        #endregion

        #region Check Notification Submitted
        public string CheckNotificationSubmitted(int notificationId)
        {
            return NotificationDAO.CheckIfNotificationSubmitted(notificationId);
        }
        #endregion

        #region UpdateCollborationAcknoledgement
        public bool UpdateCollborationAcknowledgement(long docId, int colNo, int userId, string acknowledgeAgainst, int historic)
        {
            return NotificationDAO.UpdateCollborationAck(docId, colNo, userId, acknowledgeAgainst, historic);
        }

        public List<NotificationStatusModel> GetNotificationStatusList(int pageNumber, int pageSize, string NotificationCode, string userSchema)
        {
            return NotificationDAO.GetNotificationStatusList(pageNumber, pageSize, NotificationCode, userSchema);
        }

        #endregion

        #region GetCollaborationList
        public List<CollaborationModel> GetCollaborationList(int pageNumber, int pageSize, string notificationCode, int notificationId, int historic)
        {
            return NotificationDAO.GetCollaborationList(pageNumber, pageSize, notificationCode, notificationId, historic);
        }
        #endregion

        #region GetInternalCollaboration
        public NotificationStatusModel GetInternalCollaboration(NotificationStatusModel notificationStatusModel, string userSchema)
        {
            return NotificationDAO.GetInternalCollaboration(notificationStatusModel, userSchema);
        }
        #endregion

        #region ManageCollaborationInternal
        public bool ManageCollaborationInternal(NotificationStatusModel notificationStatus, string userSchema)
        {
            return NotificationDAO.ManageCollaborationInternal(notificationStatus, userSchema);
        }
        #endregion

        #region GetExternalCollaboration
        public List<CollaborationModel> GetExternalCollaboration(int pageNumber, int pageSize, int Document_Id, string userSchema)
        {
            return NotificationDAO.GetExternalCollaboration(pageNumber, pageSize, Document_Id, userSchema);
        }
        #endregion

        #region GetTransmissionList
        public List<TransmissionModel> GetTransmissionList(GetTransmissionListParams getTransmissionList)
        {
            return NotificationDAO.GetTransmissionList(getTransmissionList);
        }
        #endregion

        #region InsertNotificationType
        public NotifGeneralDetails InsertNotificationType(PlanMovementType saveNotifType)
        {
            return NotificationDAO.InsertNotificationType(saveNotifType);
        }
        #endregion

        #region UpdateNotificationType
        public NotifGeneralDetails UpdateNotificationType(PlanMovementType updateNotifType)
        {
            return NotificationDAO.UpdateNotificationType(updateNotifType);
        }
        #endregion

        #region SetLoginStatus
        public int SetLoginStatus(int UserId, int flag)
        {
            return NotificationDAO.SetLoginStatus(UserId,flag);
        }
        #endregion

        #region ExportNotifGeneralDetails

        public Domain.ExternalAPI.ExportNotifGeneralDetails ExportNotifGeneralDetails(string esdalRefNumber)
        {
            return NotificationDAO.ExportNotifGeneralDetails(esdalRefNumber);
        }
        #endregion

        #region GetHaulierDetails
        public HAContact GetHaulierDetails(long notificationId)
        {
            return NotificationDAO.GetHaulierDetails(notificationId);
        }
        #endregion

        #region Removed Unwanted code by Mahzeer on 04-12-2023

        #region Get Haulier Licence
        //public string GetHaulierLicence(int organisationId)
        //{
        //    return NotificationDAO.GETHAULIERLICENCEDAO(organisationId);
        //}
        #endregion

        #region Get Notifcation Route Details
        //public List<ListRouteVehicleId> GetNotifcationRouteDetails(string contentReferenceNo)
        //{
        //    return NotificationDAO.GetNotifRouteDetails(contentReferenceNo);
        //}

        #endregion

        #region PrintCollabrationList
        //public List<CollaborationModel> PrintCollabrationList(string notificationCode, int notificationId)
        //{
        //    return NotificationDAO.GetPrintCollabList(notificationCode, notificationId);
        //}

        #endregion

        #region GetPreviousAnalysisId
        //public long GetPreviousAnalysisId(long notificationId)
        //{
        //    return NotificationDAO.GetPreviousAnalysisId(notificationId);
        //}
        #endregion

        #endregion
    }
}