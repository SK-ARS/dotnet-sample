using STP.Common.Constants;
using STP.Domain.DocumentsAndContents;
using STP.Domain.HelpdeskTools;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace STP.DocumentsAndContents.Interface
{
    public interface IDocumentTransmission
    {
        UserInfo GetUserDetailsForNotification(string ESDALReference);

        UserInfo GetUserDetailsForHaulier(string mnemonic, string ESDALReference);

        UserInfo GetUserName(long orgId, long contactId);

        DistributionAlerts GetSOAPoliceDetails(string ESDALReference, int transmissionId);

        DistributionAlerts GetNotifDetails(string ESDALReference, int transmissionId);

        DistributionAlerts GetHaulierDetails(string mnemonic, string ESDALReference, string versionNo);

        byte[] GetAgreedProposedNotificationXML(string docType, string ESDALReference, int notificationId);

        long SaveDistributionStatus(SaveDistributionStatusParams saveDistributionStatus);
        long SaveActiveTransmission(NotificationContacts objContact, string EsdalReference, long transmissionId, int inboxOnly);
        List<TransmissionModel> GetTransmissionType(long TransId, string Status, int StatusItemCount, string userSchema);
        TransmittingDocumentDetails SortSideCheckDoctype(int transmissionId, string userSchema);
        //int SortSideRetransmitApplication(int transmissionId, RetransmitDetails retransmitDetails, UserInfo userInfo);
        RetransmitDetails GetRetransmitDetails(long transmissionId, string userSchema);
        long GetNewInsertedTransForDist(TransmittingDocumentDetails transDetails);
        RetransmitEmailgetParams GetRetransmitDocument(TransmittingDocumentDetails transmittingDetail,RetransmitDetails retransmitDetails, int transmissionId, UserInfo userInfo, string userSchema);
        long CopyMovementSortToPortal(MovementCopyDetails moveCopyDet, int movementCloneStatus, int versionid = 0, string EsdalReference = "", byte[] hacontactbytes = null, int organizationid = 0, string userSchema = UserSchema.Portal);
        List<NotifDispensations> GetNotificationDispensation(long notificationId, int historic);
    }
}