using NotificationXSD;
using STP.Common.Constants;
using STP.Domain.HelpdeskTools;
using STP.Domain.LoggingAndReporting;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.SecurityAndUsers;
using STP.Domain.VehiclesAndFleets;
using System.Collections.Generic;

namespace STP.ServiceAccess.DocumentsAndContents
{
    public interface IDocumentService
    {
        List<NotifDispensations> GetNotificationDispensation(long notificationId, int historic);
        UserInfo GetUserDetailsForNotification(string ESDALReference);
        UserInfo GetUserDetailsForHaulier(string mnemonic, string ESDALReference);
        UserInfo GetUserName(long orgId, long contactId);
        DistributionAlerts GetSOAPoliceDetails(string ESDALReference, int transmissionId);
        DistributionAlerts GetNotifDetails(string ESDALReference, int transmissionId);
        DistributionAlerts GetHaulierDetails(string mnemonic, string ESDALReference, string versionNo);
        TransmittingDocumentDetails SortSideCheckDoctype(int transmissionId, string userSchema);
        byte[] GenerateHaulierProposedRouteDocument(string esdalRefNo, int organisationId, int contactId, string userSchema = UserSchema.Portal, UserInfo sessionInfo = null);
        byte[] GeneratePDF(int notificationID, int docType, string xmlInformation, string fileName, string esDALRefNo, long organisationID, int contactID, string docfileName, bool isHaulier = false, string organisationName = "", string HAReference = "", int routePlanUnits = 692001, string documentType = "PDF", UserInfo userInfo = null, string userType = "");
        string GeneratePDF1(int notificationID, int docType, string xmlInformation, string fileName, string esDALRefNo, long organisationID, int contactID, string docfileName, bool isHaulier = false, string organisationName = "", string HAReference = "", int routePlanUnits = 692001, string documentType = "PDF", UserInfo userInfo = null, string userType = "");
        string GetLoggedInUserAffectedStructureDetailsByESDALReference(string xmlInformation, string esDALRefNo, UserInfo SessionInfo, string userSchema, string type, int organisationId);
        List<VehComponentAxles> GetVehicleComponentAxles(int notificationId, long vehicleId);
        SummaryAxleStructureAxleWeightListPosition[] GetAxleWeightListPositions(List<VehComponentAxles> vehicleComponentAxlesList);
        SummaryAxleStructureAxleSpacingListPosition[] GetAxleSpacingListPositionAxleSpacings(List<VehComponentAxles> vehicleComponentAxlesList);
        SummaryAxleStructureAxleSpacingToFollowListPosition[] GetAxleSpacingToFollowListPositionAxleSpacings(List<VehComponentAxles> vehicleComponentAxlesList, double firstComponentAxleSpaceToFollow);
        RetransmitDetails GetRetransmitDetails(long transmissionId, string userSchema);
        string[] FetchContactPreference(int contactId, string userSchema);
        long SaveDistributionStatus(Domain.DocumentsAndContents.SaveDistributionStatusParams saveDistributionStatus);
        long GetNewInsertedTransForDist(TransmittingDocumentDetails transDetails);
        Domain.DocumentsAndContents.RetransmitEmailgetParams GetRetransmitDocument(Domain.DocumentsAndContents.GetRetransmitDocumentParams getRetransmit);
        long SaveMovementActionForDistTrans(MovementActionIdentifiers movactiontype, string MovDescrp,long ProjectId, int RevisionNo,int VersionNo, string userSchema);
        byte[] GeneratePDFFromHtmlString(Domain.DocumentsAndContents.HtmlDocumentParams model);
        void DistributeNotification(Domain.DocumentsAndContents.NotifDistibutionParams distibutionParams);
        void DistributeSOMovement(Domain.DocumentsAndContents.SODistributionParams sODistributionParams, ref int status);
        //Domain.DocumentsAndContents.GenerateEmailgetParams GenerateMovementEMAIL(Domain.DocumentsAndContents.GenerateEmailParams emailParams);
        //Domain.DocumentsAndContents.GenerateEmailgetParams GenerateMovementPDF(Domain.DocumentsAndContents.GenerateEmailParams pdfParams);
        //Domain.DocumentsAndContents.GenerateEmailgetParams GenerateMovementWord(Domain.DocumentsAndContents.GenerateEmailParams wordParams);
        //Domain.DocumentsAndContents.ESDALNotificationGetParams GenerateEsdalNotification(Domain.DocumentsAndContents.GenerateEsdalNotificationParams esdalNotificationParams);
        //Domain.DocumentsAndContents.ESDALNotificationGetParams GenerateEsdalReNotification(Domain.DocumentsAndContents.GenerateEsdalNotificationParams esdalNotificationParams);
        //Domain.DocumentsAndContents.SODistributeDocumentParams GenerateSODistributeDocument(Domain.DocumentsAndContents.SOProposalDocumentParams sOProposalDocument);
        //long CopyMovementSortToPortal(MovementCopyDetails moveCopyDet, int movementCloneStatus, int versionid = 0, string EsdalReference = "", byte[] hacontactbytes = null, int organizationid = 0, string userSchema = UserSchema.Portal);
        //Domain.DocumentsAndContents.SOProposalXsltPath GetSoProposalXsltPath(string ContactType, long ProjectStatus, string FinalReson);
        //bool GetImminentForCountries(int Orgid, string ImminentStatus);
        //long SaveDocument(Domain.DocumentsAndContents.SaveDocumentParams saveDocumentParams);
        //byte[] TransmitNotification(Domain.DocumentsAndContents.TransmitNotificationParams transmitNotification);
        //long SaveInboxItems(int NotificationID, long documentId, int OrganisationID, string esDAlRefNo, string userSchema = UserSchema.Portal, int icaStatus = 277001, bool ImminentMovestatus = false);
        //void InsertTransmissionInfoToAction(Domain.DocumentsAndContents.TransmitNotificationParams transmitNotification);
        //bool GenerateMovementAction(UserInfo UserSessionValue, string EsdalRef, MovementActionIdentifiers movActionItem, int movFlagVar = 0, NotificationContacts objContact = null, long projectId = 0, int versioNo = 0, int revisionNo = 0);
        //void TransmissionInfoToAction(NotificationContacts objcontact, UserInfo userInfo, long transmissionId, string esdalRef, int actionFlag, string errMessage = "", string docType = "", long ProjectId = 0, int versionNo = 0, int revisionNo = 0);
    }
}
