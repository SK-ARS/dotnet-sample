using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using STP.Domain;
using STP.Domain.HelpdeskTools;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.SecurityAndUsers;

namespace STP.MovementsAndNotifications.Interface
{
    public interface IMovement
    {
        List<SORTMovementList> GetSORTMovementRelatedToStructList(int organisationId, int pageNumber, int pageSize, long structID);
        List<SORTMovementList> GetSortMovementsList(int pageNumber, int pageSize, SORTMovementFilter sortMovementFilter, SortAdvancedMovementFilter SORTMovementFilterAdvanced, bool IsCreCandidateOrCreAppl, SortMapFilter SortObjMapFilter, bool planMovement, int sortOrder, int sortType);
        int EditInboxItemOpenStatus(long inboxId, long organisationId);
        MovementModel GetAuthorizeMovementGeneralProposed(MovementModelParams objMovementModelParams);
        List<VehicleConfigration> GetVehiclesList(string mnemonic, string ESDALReferenceNo, string version, long notificationId, int isSimplified);
        MovementModel GetHAAndHaulierContactIdByName(MovementModel movement);
        List<CollaborationNotes> GetCollaborationNotes(long DocumentId, long OrganisationId);
        MovementModel GetAuthorizeMovementGeneral(long notificationId, long inboxId, long contactId, string ESDALReferenceNo, long organisationId);
        MovementModel GetHaulierContactId(MovementModel movement);
        long UpdateInboxItemStatus(InboxItemStatusParams objInboxItemStatusParams);
        List<SpecialOrder> GetSpecialOrders(string notificationCode);
        List<VR1> GetVR1s(string VR1No);
        List<RelatedCommunication> GetNotificationDetailsByCode(string notificationCode, string route, long organisationId, long projectId);
        List<VehicleConfigration> GetNENVehicleList(long NENId, long inboxId, long organisationId);
        string PrintReport(long notificationId);
        DocumentInfo ViewMovementDocument(long documentId, long organisationId, string userSchema);
        MovementModel GetCollaborationStatus(long INBOX_ID);
        bool ManageCollaborationStatus(MovementModel movement);
        string PrintAgreedReport(string Notificationcode, long organisationId);
        string GetProposalOutboundDocsXML(long documentNumber);
        MovementModel GetInboxItemDetails(string esdalRefNumber, long organisationId);
        bool ManageNotesOnEscort(MovementModel movement);
        string GetHaulierUserId(string firstName, string surName, int organisationId);
        bool ManageInternalNotes(MovementModel movement);
        int GetContactDetailsForDefault(int organisationId);
        long CopyMovementSortToPortal(MovementCopyDetails movementCopyDetails, int movementCloneStatus, int versionID, string esdalReference, byte[] haContactBytes, int organizationID, string userSchema);
        List<MapStructLink> GetStructLinkId(SortMapFilter objSortMapFilterParams);

        MovementContactModel GetContactedPartiesDetail(long analysisId);
        int ReturnRouteAutoAssignVehicle(long movementId, int flag, long notificationId, long organisationId);
        DateTime CalcualteMovementDate(int noticePeriod, int vehicleClass, string userSchema);
        List<ContactModel> GetNENAffectedContactDetails(string esdalRefNumber, string userSchema);
    }
}