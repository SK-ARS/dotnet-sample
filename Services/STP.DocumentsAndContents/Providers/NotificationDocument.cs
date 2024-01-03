using STP.DocumentsAndContents.Document;
using STP.DocumentsAndContents.Interface;
using STP.DocumentsAndContents.Persistance;
using STP.Domain.DocumentsAndContents;
using STP.Domain.RouteAssessment;
using STP.Domain.SecurityAndUsers;
using System.Diagnostics;

namespace STP.DocumentsAndContents.Providers
{
    public class NotificationDocument : INotificationDocument
    {
        #region ListMovement Singleton

        private NotificationDocument()
        {
        }
        public static NotificationDocument Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }

        internal static class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly NotificationDocument instance = new NotificationDocument();
        }
        #endregion

        public DrivingInstructionModel GetRouteDescription(int notificationId)
        {
            return OutBoundDAO.GetRouteDescription(notificationId);
        }
        
        public byte[] GenerateHaulierNotificationDocument(int notificationId, Enums.DocumentType docType, int contactId, UserInfo sessionInfo = null)
        {
            GenerateDocument generateDocument = new GenerateDocument();
            return generateDocument.GenerateHaulierNotificationDocument(notificationId, docType, contactId, sessionInfo);
        }
        public ContactModel GetContactDetails(int contactId)
        {
            return OutBoundDAO.GetContactDetails(contactId);
        }
        public DrivingInstructionModel GetNENRouteDescription(long NENInboxId, int organisationId)
        {
            return OutBoundDAO.GetNENRouteDescription(NENInboxId, organisationId);
        }
        public RouteAnalysisModel GetApiRouteAssessmentDetails(int notificationID, int organisationId, int isNen)
        {
            return OutBoundDAO.GetRouteAssessmentDetails(notificationID, organisationId, isNen);
        }
        public StructuresModel GetStructuresXML(int notificationId)
        {
            return OutBoundDAO.GetStructuresXML(notificationId);
        }

        #region Commented Code By Mahzeer On 12/07/2023
        /*
        public int GetVehicleUnits(int contactId, Int32 organisationId)
        {
            return CommonMethods.GetVehicleUnits(contactId, organisationId);
        }

        public long AddManageDocument(OutboundDocuments obdc, string userSchema)
        {
            return OutBoundDocumentDOA.AddManageDocument(obdc, userSchema);
        }
        public bool GenerateMovementAction(UserInfo userSessionValue, string EsdalRef, MovementActionIdentifiers movActionItem, int movFlagVar = 0, NotificationContacts objContact = null)
        {
            return OutBoundDocumentDOA.GenerateMovementAction(userSessionValue, EsdalRef, movActionItem,0,0,0, movFlagVar, objContact);
        }

        public int InsertCollaboration(OutboundDocuments obdc, long documentId, string userSchema, int status = 0)
        {
            return OutBoundDocumentDOA.InsertCollaboration(obdc, documentId, userSchema, status);
        }

        public StructuresModel GetStructuresXMLByESDALReference(string ESDALReference, string userSchema)
        {
            return OutBoundDAO.GetStructuresXMLByESDALReference(ESDALReference, userSchema);
        }
        public OutboundDocuments GetNotificationDetails(int notificationId)
        {
            return CommonMethods.GetNotificationDetails(notificationId);
        }

        public OutboundNotificationStructure GetOutboundNotificationDetailsForNotification(Enums.PortalType psPortalType, int NotificationID, bool isHaulier, int ContactID)
        {
            return OutBoundDAO.GetOutboundNotificationDetailsForNotification(NotificationID, isHaulier, ContactID);
        }*/
        #endregion

    }
}