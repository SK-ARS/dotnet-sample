using STP.Common.Constants;
using System.Collections.Generic;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.SecurityAndUsers;
using STP.Domain.Communications;
using STP.Domain.MovementsAndNotifications.Movements;

namespace STP.MovementsAndNotifications.Interface
{
    public interface ISimpleNotification
    {
        NotificationGeneralDetails GetNotificationGeneralDetail(long notificationId, int historic);
        int DeleteNotification(int notificationId);
        byte[] GetAffectedParties(int notificationId, string userSchema = UserSchema.Portal);
        MailResponse GetResponseMailDetails(int organisationId, string userSchema = UserSchema.Portal);
        bool SaveAffectedNotificationDetails(AffectedStructConstrParam affectedParam);
        int UpdateNotification(NotificationGeneralDetails notificationGeneralDetails);
        NotificationGeneralDetails NotifyApplication(long versionId);
        int CheckNotificationVersion(int notificationId);
        string GenerateNotificationCode(int organisationId, long notificationId, int detail);
        NotificationGeneralDetails RenotifyNotification(int notificationId, int VR1);
        NotificationGeneralDetails CloneNotification(int notificationId);
        NotificationGeneralDetails CloneHistoricNotification(int notificationId);
        byte[] GetNotificationAffectedStructures(int notificationId, string esdalReferenceNumber, string haulierMnemonic, string versionNumber, string userSchema = UserSchema.Portal);
        MovementPrint GetOrderNoProjectId(int versionId);
        MovementPrint GetProjectIdByEsdalReferenceNo(string EsdalRefNo);

        #region Removed Unwanted code by Mahzeer on 04-12-2023
        //int UpdateInboundNotif(int notificationId, byte[] inboundNotif);
        //List<AxleDetails> ListCloneAxelDetails(int VehicleID);
        //List<AxleDetails> ListAxelDetails(int VehicleID);
        //int GetGrossWeight(int notificationId);
        //byte[] GetInboundNotification(int notificationId);
        //decimal GetMaxReduciableHeight(int notificationId);
        //bool SetNotificationVRNum(int notificationId);
        //UserRegistration GetNotifHaulierDetails(int userId, int notificationId = 0, int vehicleClassCode = 0);
        //NotificationGeneralDetails SaveNotifGeneralDetails(NotificationGeneralDetails notificationGeneralDetails);
        //NotificationGeneralDetails CheckNotifValidation(string contentReferenceNo);
        //int IsNotifSubmitCheck(long notificationId);
        //long GetVehicleTypeForNotification(int notifId)
        #endregion

    }
}