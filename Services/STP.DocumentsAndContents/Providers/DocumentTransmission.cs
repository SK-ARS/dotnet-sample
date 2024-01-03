using STP.DocumentsAndContents.Interface;
using STP.DocumentsAndContents.Persistance;
using STP.Domain.DocumentsAndContents;
using STP.Domain.HelpdeskTools;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.SecurityAndUsers;
using STP.Domain.VehiclesAndFleets;
using STP.Common.Constants;
using System.Collections.Generic;
using System.Diagnostics;
using STP.Domain.MovementsAndNotifications.Notification;

namespace STP.DocumentsAndContents.Providers
{
    public sealed class DocumentTransmission : IDocumentTransmission
    {
        #region DocumentTransmission Singleton

        private DocumentTransmission()
        {
        }
        public static DocumentTransmission Instance
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
            internal static readonly DocumentTransmission instance = new DocumentTransmission();
        }
        #endregion

        #region Get UserDetails for Notification
        public UserInfo GetUserDetailsForNotification(string ESDALReference)
        {
            return DocumentTransmissionDAO.GetUserDetailsForNotification(ESDALReference);
        }
        #endregion

        #region Get UserDetails for Haulier
        public UserInfo GetUserDetailsForHaulier(string mnemonic, string ESDALReference)
        {
            return DocumentTransmissionDAO.GetUserDetailsForHaulier(mnemonic, ESDALReference);
        }
        #endregion

        #region Get UserName
        public UserInfo GetUserName(long orgId, long contactId)
        {
            return DocumentTransmissionDAO.GetUserName(orgId, contactId);
        }
        #endregion

        #region Get SOA,Police Details
        public DistributionAlerts GetSOAPoliceDetails(string ESDALReference, int transmissionId)
        {
            return DocumentTransmissionDAO.GetSOAPoliceDetails(ESDALReference, transmissionId);
        }
        #endregion

        #region Get Notifcation Details
        public DistributionAlerts GetNotifDetails(string ESDALReference, int transmissionId)
        {
            return DocumentTransmissionDAO.GetNotifDetails(ESDALReference, transmissionId);
        }
        #endregion

        #region Get Haulier Details
        public DistributionAlerts GetHaulierDetails(string mnemonic, string ESDALReference, string versionNo)
        {
            return DocumentTransmissionDAO.GetHaulierDetails(mnemonic, ESDALReference, versionNo);
        }
        #endregion

        #region Get Agreed proposed and notification details XML for Haulier
        public byte[] GetAgreedProposedNotificationXML(string docType, string ESDALReference, int notificationId)
        {
            return DocumentTransmissionDAO.GetAgreedProposedNotificationXML(docType, ESDALReference, notificationId);
        }
        #endregion

        #region Save DistributionStatus
        public long SaveDistributionStatus(SaveDistributionStatusParams saveDistributionStatus)
        {
            return DocumentTransmissionDAO.SaveDistributionStatus(saveDistributionStatus);
        }
        #endregion

        #region Save ActiveTransmission
        public long SaveActiveTransmission(NotificationContacts objContact, string EsdalReference, long transmissionId, int inboxOnly)
        {
            return DocumentTransmissionDAO.SaveActiveTransmission(objContact, EsdalReference, transmissionId, inboxOnly);
        }
        #endregion

        #region Save GetTransmissionType
       public List<TransmissionModel> GetTransmissionType(long TransId, string Status, int StatusItemCount, string userSchema)
        {
            return DocumentTransmissionDAO.GetTransmissionType(TransId, "310001,310009,310008,310002,310007,310003,310005", 7, UserSchema.Portal);
        }
        #endregion

        #region SortSideCheckDoctype
        public TransmittingDocumentDetails SortSideCheckDoctype(int transmissionId, string userSchema)
        {
            return DocumentTransmissionDAO.SortSideCheckDoctype(transmissionId , userSchema);
        }
        #endregion

        #region SortSideRetransmitApplication Commented By Mahzeer on 17/08/2023
        /*public int SortSideRetransmitApplication(int transmissionId, RetransmitDetails retransmitDetails, UserInfo userInfo)
        {
            return DocumentTransmissionDAO.SortSideRetransmitApplication(transmissionId, retransmitDetails, userInfo);
        }*/
        #endregion

        #region CopyMovementSortToPortal
        public long CopyMovementSortToPortal(MovementCopyDetails moveCopyDet, int movementCloneStatus, int versionid = 0, string EsdalReference = "", byte[] hacontactbytes = null, int organizationid = 0, string userSchema = UserSchema.Portal)
        {
            return DocumentTransmissionDAO.CopyMovementSortToPortal(moveCopyDet, movementCloneStatus, versionid, EsdalReference, hacontactbytes, organizationid, userSchema);
        }
        #endregion

        #region GetVehicleComponentAxles
        public List<VehComponentAxles> GetVehicleComponentAxles(int notificationId, long vehicleId)
        {
          return OutBoundDAO.GetVehicleComponentAxles(notificationId, vehicleId);
        }
        #endregion

        #region GetRetransmitDetails
        public RetransmitDetails GetRetransmitDetails(long transmissionId, string userSchema)
        {
            return DocumentTransmissionDAO.GetRetransmitDetails(transmissionId, userSchema);
        }
        #endregion

        #region GetNewInsertedTransForDist
        public long GetNewInsertedTransForDist(TransmittingDocumentDetails transDetails)
        {
            return DocumentTransmissionDAO.getNewInsertedTransForDist(transDetails, (int)transDetails.TransmissionId);
        }
        #endregion

        #region GetRetransmitDocument
        public RetransmitEmailgetParams GetRetransmitDocument(TransmittingDocumentDetails transmittingDetail, RetransmitDetails retransmitDetails, int transmissionId, UserInfo userInfo, string userSchema)
        {
            return DocumentTransmissionDAO.GetRetransmitDocument(transmittingDetail,retransmitDetails,transmissionId,userInfo,userSchema);
        }
        #endregion

        #region GetNotificationDispensation
        public List<NotifDispensations> GetNotificationDispensation(long notificationId, int historic)
        {
            return OutBoundDAO.GetNotificationDispensation(notificationId, historic);
        }
        #endregion

        #region  GetDocument
        public XMLModel GetDocument(SOProposalDocumentParams documentParams)
        {
            return DocumentTransmissionDAO.GetDocument(documentParams);
        }
        #endregion
    }
}