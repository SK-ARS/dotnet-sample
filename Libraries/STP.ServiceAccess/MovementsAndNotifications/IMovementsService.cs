using STP.Domain;
using STP.Domain.DocumentsAndContents;
using STP.Domain.HelpdeskTools;
using STP.Domain.LoggingAndReporting;
using STP.Domain.MovementsAndNotifications.Folder;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.MovementsAndNotifications
{
    public interface IMovementsService
    {
        //List<MovementsInbox> GetMovementInbox(int orgId, int pageNum, int pageSize, MovementsInboxFilter objMoveInboxFilter, MovementsInboxAdvancedFilter objMoveAdvInboxFilter, int userType, string userID, string userSchema);
        List<SORTMovementList> GetSORTMovementRelatedToStructList(int orgID, int pageNum, int pageSize, long structID);
        List<SORTMovementList> GetSORTMovementList(int organisationId, int pageNum, int pageSize, SORTMovementFilter objSORTMovementFilter, SortAdvancedMovementFilter objSORTMovementFilterAdvanced, bool IsCreCandidateOrCreAppl = false, SortMapFilter SortObjMapFilter=null, bool planMovement = false,int sortOrder=1,int sortType=0);
        int GetContactDetails(int UserId);
        int EditInboxItemOpenStatus(long inboxId, long organisationId);
        MovementModel GetAuthorizeMovementGeneralProposed(string route, string mnemonic, string esdalrefnum, string version, long inboxId, string esdal_ref, int contactId, long organisationId);
        string GetSpecialOrderNo(string ESDALReferenceNo);
        List<VehicleConfigration> GetVehiclesList(string mnemonic, string ESDALreferenceNo, string version, long notificationId, int IsSimplified);
        MovementModel GetHAAndHaulierContactIdByName(MovementModel movement);
        long GetDocumentID(string ESDALReferenceNo, long organisationId);
        List<CollaborationNotes> GetCollaborationNotes(long documentId, long organisationId);
        MovementModel GetHaulierContactId(MovementModel objMovement);
        MovementModel GetAuthorizeMovementGeneral(long notificationId, long inboxId, long contactId, string ESDALReference, long organisationId);
        List<SpecialOrder> GetSpecialOrders(string notificationCode);
        List<VR1> GetVR1s(string VR1No);
        List<RelatedCommunication> GetNotificationDetailsByCode(string notificationCode, string route, long organisationId, long ProjectId);
        long UpdateInboxItemStatus(long OrganisationId, string ESDALRef);
        List<VehicleConfigration> GetNENVehicleList(long NENId, long inboxId, long orgId);
       
        string GetContentReferenceNo(int notificationNo);
        int InsertQuickLinkSOA(int orgId, int inboxId, int userId);
        long SaveNotificationAuditLog(AuditLogIdentifiers auditLogType, string logMsg, int User_ID, long Org_ID = 0);
        List<DelegArrangeNameList> GetArrangementList(int orgId);
        List<QuickLinksSOA> GetQuickLinksSOAList(int UserId);
        List<MovementsList> GetMovementsList(int orgId, int pageNum, int pageSize, MovementsFilter movementFilter, MovementsAdvancedFilter advancedMovementFilter, int presetFilter, string userSchema, int ShowPrevSortRoute = 0, bool prevMovImport = false);
        List<FolderNameList> GetFolderList(long orgId, string userSchema);
        string PrintReport(long Notificationid);
        DocumentInfo ViewMovementDocument(long documentId, long organisationId, string userSchema);
        MovementModel GetCollaborationStatus(long INBOX_ID);
        bool ManageCollaborationStatus(MovementModel movement);
        string PrintAgreedReport(string Notificationcode, long organisationId);
        MovementModel GetInboxItemDetails(string esdalRefNumber, long organisationId);
        string GetProposalOutboundDocsXML(long documentNumber);
        bool ManageNotesOnEscort(MovementModel movement);
        string GetHaulierUserId(string FirstName, string Surname, int OrgId);
        bool ManageInternalNotes(MovementModel movement);
        int GetContactDetailsForDefault(int OrganisationId);
        List<MovementsList> GetPlanMovementList(int organisationId, int pageNumber, int pageSize, MovementsAdvancedFilter advancedMovementFilter, int presetFilter, int movementType, int vehicleClass, string userSchema);
        List<MapStructLink> GetStructLinkId(SortMapFilter objSortMapFilterParams);
        MovementContactModel GetContactedPartiesDetail(long analysisId);
        List<MovementsInbox> GetHomePageMovements(GetInboxMovementsParams inboxMovementsParams);
        int ReturnRouteAutoAssignVehicle(long movementId, int flag, long notificationId, long organisationId);
        XMLModel GetXmlDataForPrint(SOProposalDocumentParams documentParams);
        //List<MovementsInbox> GetMovementInbox(int organisationId, int page, int pageSize, MovementsInboxFilter objMovementsInboxFilter, MovementsInboxAdvancedFilter objMovementsInboxAdvancedFilter, int userType, string userId, string userSchema, int presetFilter);
        List<MovementsInbox> GetMovementInbox(GetInboxMovementsParams inboxMovementsParams);
        List<FolderTreeModel> GetFolders(long organisationId); 
        int InsertFolderInfo(InsertFolderParams model);
        int UpdateFolderInfo(EditFolderParams model);
        int DeleteFolderInfo(EditFolderParams model);
        int AddItemToFolder(List<AddItemFolderModel> model);
        int RemoveItemsFromFolder(List<AddItemFolderModel> model);
        int MoveFolderToFolder(FolderTreeModel model);
        DateTime CalcualteMovementDate(int noticePeriod, int vehicleClass, string userSchema);
        List<ContactModel> GetNENAffectedContactDetails(string esdalRefNumber, string userSchema);
    }
}
