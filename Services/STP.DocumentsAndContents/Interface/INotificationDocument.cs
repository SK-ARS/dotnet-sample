using STP.Domain.DocumentsAndContents;
using STP.Domain.RouteAssessment;
using STP.Domain.SecurityAndUsers;

namespace STP.DocumentsAndContents.Interface
{
    public interface INotificationDocument
    {
        DrivingInstructionModel GetRouteDescription(int notificationId);
        DrivingInstructionModel GetNENRouteDescription(long NENInboxId, int organisationId);
        StructuresModel GetStructuresXML(int notificationId);
        byte[] GenerateHaulierNotificationDocument(int notificationId, Enums.DocumentType docType, int contactId, UserInfo sessionInfo = null);
        ContactModel GetContactDetails(int contactId);
        RouteAnalysisModel GetApiRouteAssessmentDetails(int notificationID, int organisationId, int isNen);
        #region Commented Code By Mahzeer On 12/07/2023
        //OutboundDocuments GetNotificationDetails(int notificationId);
        //OutboundNotificationStructure GetOutboundNotificationDetailsForNotification(Enums.PortalType psPortalType, int NotificationID, bool isHaulier, int ContactID);
        //int GetVehicleUnits(int contactId, Int32 organisationId);
        //long AddManageDocument(OutboundDocuments obdc, string userSchema);
        //bool GenerateMovementAction(UserInfo userSessionValue, string EsdalRef, MovementActionIdentifiers movActionItem, int movFlagVar = 0, NotificationContacts objContact = null);
        //int InsertCollaboration(OutboundDocuments obdc, long documentId, string userSchema, int status = 0);
        //StructuresModel GetStructuresXMLByESDALReference(string ESDALReference, string userSchema);
        #endregion
    }
}
