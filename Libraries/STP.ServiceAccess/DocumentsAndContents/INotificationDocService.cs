using NotificationXSD;
using STP.Domain;
using STP.Domain.DocumentsAndContents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STP.Domain.SecurityAndUsers;
using STP.Domain.RouteAssessment;
using STP.Domain.LoggingAndReporting;

namespace STP.ServiceAccess.DocumentsAndContents
{
    public interface INotificationDocService
    {
        DrivingInstructionModel GetRouteDescription(int NotificationID);
        DrivingInstructionModel GetNENRouteDescription(long NENInboxId, int organisationId);
        RouteAnalysisModel GetApiRouteAssessmentDetails(int notificationID, int organisationId, int isNen);
        StructuresModel GetStructuresXML(int NotificationID);
        byte[] GenerateHaulierNotificationDocument(int notificationId, Enums.DocumentType doctype, int contactId, UserInfo SessionInfo = null);
        ContactModel GetContactDetails(int contactId);

        #region Commented Code by Mahzeer 13/07/2023
        //int GetVehicleUnits(int ContactId, Int32 OrgId);
        //long AddManageDocument(OutboundDocuments obdc, string userSchema);
        //bool GenerateMovementAction(UserInfo UserSessionValue, string EsdalRef, MovementActionIdentifiers movActionItem, int movFlagVar = 0, NotificationContacts objContact = null);
        //int InsertCollaboration(OutboundDocuments obdc, long documentId, string userSchema, int status = 0);
        //StructuresModel GetStructuresXMLByESDALReference(string esdalReference, string userSchema);
        #endregion

    }
}
