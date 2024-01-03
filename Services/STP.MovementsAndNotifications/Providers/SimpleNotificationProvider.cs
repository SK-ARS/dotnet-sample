using STP.Common.Constants;
using STP.MovementsAndNotifications.Interface;
using STP.MovementsAndNotifications.Persistance;
using System.Diagnostics;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.Communications;

namespace STP.MovementsAndNotifications.Providers
{
    public sealed class SimpleNotificationProvider : ISimpleNotification
    {
        #region SimpleNotificationProvider Singleton
        private SimpleNotificationProvider()
        {
        }
        public static SimpleNotificationProvider Instance
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
            internal static readonly SimpleNotificationProvider instance = new SimpleNotificationProvider();
        }
        #endregion

        #region Get Notification General Details
        public NotificationGeneralDetails GetNotificationGeneralDetail(long notificationId, int historic)
        {
            return SimpleNotificationDAO.GetNotifGeneralDetails(notificationId, historic);
        }
        #endregion

        #region GetNotificationAffectedStructures
        public byte[] GetNotificationAffectedStructures(int notificationId, string esdalReferenceNumber, string haulierMnemonic, string versionNumber, string userSchema = UserSchema.Portal)
        {
            return SimpleNotificationDAO.GetNotificationAffectedStructures(notificationId, esdalReferenceNumber, haulierMnemonic, versionNumber, userSchema);
        }
        #endregion

        #region GetOrderNoProjectId
        public MovementPrint GetOrderNoProjectId(int versionId)
        {
            return SimpleNotificationDAO.GetOrderNoProjectId(versionId);
        }
        #endregion

        #region GetProjectIdByEsdalReferenceNo
        public MovementPrint GetProjectIdByEsdalReferenceNo(string EsdalRefNo)
        {
            return SimpleNotificationDAO.GetProjectIdByEsdalReferenceNo(EsdalRefNo);
        }
        #endregion
        
        #region DeleteNotification
        public int DeleteNotification(int notificationId)
        {
            return SimpleNotificationDAO.DeleteNotification(notificationId);
        }
        #endregion

        #region GetAffectedParties
        public byte[] GetAffectedParties(int notificationId, string userSchema = UserSchema.Portal)
        {
            return SimpleNotificationDAO.GetNotifAffectedParties(notificationId, userSchema);
        }
        #endregion

        #region GetResponseMailDetails
        public MailResponse GetResponseMailDetails(int organisationId, string userSchema = UserSchema.Portal)
        {
            return SimpleNotificationDAO.GetResponseMailDetails(organisationId, userSchema);
        }
        #endregion

        #region SaveAffectedNotificationDetails
        public bool SaveAffectedNotificationDetails(AffectedStructConstrParam affectedParam)
        {
            return SimpleNotificationDAO.SaveAffectedNotificationDetails(affectedParam);
        }
        #endregion

        #region UpdateNotification
        public int UpdateNotification(NotificationGeneralDetails notificationGeneralDetails)
        {
            return SimpleNotificationDAO.UpdateNotification(notificationGeneralDetails);
        }
        #endregion

        #region Notify Application
        public NotificationGeneralDetails NotifyApplication(long versionId)
        {
            return SimpleNotificationDAO.NotifyApplication(versionId);
        }
        #endregion

        #region CheckNotificationVer
        public int CheckNotificationVersion(int notificationId)
        {
            return SimpleNotificationDAO.FetchNotifVersion(notificationId);
        }
        #endregion

        #region GenerateNotificationCode
        public string GenerateNotificationCode(int organisationId, long notificationId, int detail)
        {
            return SimpleNotificationDAO.GenerateNotifCode(organisationId, notificationId, detail);
        }
        #endregion
        
        #region ReNotifyNotification
        public NotificationGeneralDetails RenotifyNotification(int notificationId, int VR1)
        {
            return SimpleNotificationDAO.RenotifyNotification(notificationId, VR1);
        }
        #endregion

        #region CloneNotification
        public NotificationGeneralDetails CloneNotification(int notificationId)
        {
            return SimpleNotificationDAO.CloneNotifications(notificationId);
        }

        public NotificationGeneralDetails CloneHistoricNotification(int notificationId)
        {
            return SimpleNotificationDAO.CloneHistoricNotification(notificationId);
        }
        #endregion

        #region Removed Unwanted code by Mahzeer on 04-12-2023
        //public long GetVehicleTypeForNotification(int notifId)
        //{
        //    return SimpleNotificationDAO.GetSimpleNotifVhclType(notifId);
        //}
        //public int IsNotifSubmitCheck(long notificationId)
        //{
        //    return SimpleNotificationDAO.IsNotifSubmitCheck(notificationId);
        //}
        //public NotificationGeneralDetails CheckNotifValidation(string contentReferenceNo)
        //{
        //    return SimpleNotificationDAO.CheckNotifValidation(contentReferenceNo);
        //}
        //public NotificationGeneralDetails SaveNotifGeneralDetails(NotificationGeneralDetails notificationGeneralDetails)
        //{
        //    return SimpleNotificationDAO.SaveNotifGeneralDetails(notificationGeneralDetails);
        //}
        //public UserRegistration GetNotifHaulierDetails(int userId, int notificationId = 0, int vehicleClassCode = 0)
        //{
        //    return SimpleNotificationDAO.GetNotifHaulierDetails(userId, vehicleClassCode, notificationId);
        //}
        //public bool SetNotificationVRNum(int notificationId)
        //{
        //    return SimpleNotificationDAO.SetNotificationVRNum(notificationId);
        //}
        //public int UpdateInboundNotif(int notificationId, byte[] inboundNotif)
        //{
        //    return SimpleNotificationDAO.UpdateInboundNotif(notificationId, inboundNotif);
        //}
        //public List<AxleDetails> ListCloneAxelDetails(int VehicleID)
        //{
        //    return SimpleNotificationDAO.ListCloneAxelDetails(VehicleID);
        //}
        //public List<AxleDetails> ListAxelDetails(int VehicleID)
        //{
        //    return SimpleNotificationDAO.ListAxelDetails(VehicleID);
        //}
        //public int GetGrossWeight(int notificationId)
        //{
        //    return SimpleNotificationDAO.GetGrossWeight(notificationId);
        //}
        //public byte[] GetInboundNotification(int notificationId)
        //{
        //    return SimpleNotificationDAO.GetInboundNotification(notificationId);
        //}
        //public decimal GetMaxReduciableHeight(int notificationId)
        //{
        //    return SimpleNotificationDAO.GetMaxReducibleHeight(notificationId);
        //}
        #endregion
    }
}