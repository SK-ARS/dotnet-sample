using STP.Domain.Routes;
using STP.Domain.Applications;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STP.Domain.SecurityAndUsers;
using AggreedRouteXSD;
using STP.Domain.MovementsAndNotifications.Notification;


namespace STP.ServiceAccess.Applications
{
   public interface ISORTApplicationService
    {
        List<SORTMovementList> GetHaulierAppRevision(long projectID);
        List<SORTUserList> ListSORTUser(long userTypeID, int checkerType = 0);
        List<string> SaveSORTAllocateUser(AllocateSORTUserInsertParams allocateSortUserParams);
        List<SORTMovementList> GetMovmentVersion(long projectID);
        List<CandidateRTModel> GetCandidateRTDetails(long projectID);
        SOApplicationRelatedMov GetRelatedMovement(long applicationID, string type);       
        bool CheckVehicleOnRoute(int projectID, int revisionID);
        long GetRevIDFromApplication(long projectID);
        List<MovementHistory> GetMovementHistory(int pageNumber, int pageSize, string haulierNumber, int esdalReference, int versionNumber, long projectID, int? sortOrder = null, int? sortType = null);
        bool GetCandRouteVehicleAssignDetails(long routeRevisionID, string userSchema);
        string GetCandidateRouteNM(long candidateRouteID);
        string GetVR1ApprovalDate(long projectID);
        SOApplication GetProjOverviewDetails(long revisionID);
        string GetSORTNotifiCode(int revisionID);
        List<CRDetails> GetRouteType(int revisionID, string userSchema);
        List<AppVehicleConfigList> CandidateRouteVehicleConfiguration(int revisionID, string userSchema, char rListType = ApplicationConstants.RListTypeC);
        int SORTAppWithdrawandDecline(SORTAppWithdrawAndDeclineParams sortApplnWithdrawandDeclineParams);
        int SORTUnwithdraw(SORTAppWithdrawAndDeclineParams sortApplnWithdrawandDeclineParams);
        int CheckCandIsModified(int analysisID);
        SORTLatestAppDetails GetSortProjectDetails(long projectID);
        List<AppRouteList> CandRouteList(int routeRevisionID, string userSchema, char rListType = ApplicationConstants.RListTypeC);
        SOApplication SubmitSORTSoApplication(SORTSOApplicationParams sortSoApplicationParams);
        CandidateRouteInsertResponse SaveCandidateRoute(CandidateRouteInsertParams candidateRouteSaveParams);
        RouteRevision SaveRouteRevision(RouteRevisionInsertParams routeRevisionInsertParams);
        int SaveSORTMovProjDetail(SORTMvmtProjectDetailsInsertParams sortMvmtProjectDetailsInsertParams);
        ApplyForVR1 SubmitSORTVR1Application(SubmitSORTVR1Params submitSORTParams);
        int SaveVR1Approval(VR1ApprovalInsertParams vr1ApprovalInsertParams);
        string SaveVR1Number(VR1NumberInsertParams vr1NumberInsertParams);
        bool SaveMovHaulierNotes(HaulierMovNotesInsertParams haulierMovNotesInsertParams);
        ApplyForVR1 ReviseVR1Application(ReviseVR1Params reviseVR1Params);
        long UpdateCandidateRouteNM(UpdateCandidateRouteNMInsertParams updateCandidateRouteNMInsertParams);
        decimal UpdateProjectDetails(UpdateProjectDetailsInsertParams updateProjectDetailsInsertParams);
        bool UpdateCollaborationView(int documentID);
        int UpdateSpecialOrder(UpdateSpecialOrderInsertParams updateSpecialOrderInsertParams);
        bool SpecialOrderUpdation(SpecialOrderUpdationInsertParams specialOrderUpdationInsertParams);
        bool Deletequicklinks(long projectID);
        void CloneRouteParts(CloneRTPartsInsertParams cloneRTPartsInsertParams);
        bool CheckerDetailsUpdation(UpdateCheckerDetailsInsertParams updateCheckerDetailsInsertParams);
        void CloneApplicationParts(CloneApplicationRTPartsInsertParams cloneApplicationRTPartsInsertParams);
        object SaveMovementVersion(InsertMovementVersionInsertParams insertMovementVersionInsertParams);
        bool SaveResponseMessage(SaveResponseMessageParams saveResponseMessageParams);
        byte[] GetMovHaulierNotes(long VersionId, string UserSchema);
        SOHaulierApplication GetSORTSOHaulierDetails(long revisionId);
        void UpdateCandIsModified(int analysisID);
        int MovementVersionAgreeUnagreeWith(MovementVersionAgreeUnagreeWithInsertParams movementVersionAgreeUnagreeWithInsertParams);
        byte[] GenerateFormVR1Document(string haulierMnemonic, string esdalRefNumber, int Version_No, bool generateFlag = true);
        byte[] GenerateHaulierAgreedRouteDocument(string esDALRefNo = "GCS1/25/2", string order_no = "P21/2012", int contactId = 8866, int UserTypeId = 0);
        Dictionary<int, int> GetNotifICAstatus(string xmlaffectedStructures);
        byte[] GetAffectedStructures(int notificationId, string esdalReferenceNumber, string haulierMnemonic, string versionNumber, string userSchema);
        MovementPrint GetOrderNoProjectId(int versionId);
        MovementPrint GetProjectIdByEsdalReferenceNo(string EsdalRefNo);
        byte[] GenerateAmendmentDocument(string SOnumber, STP.Domain.DocumentsAndContents.Enums.DocumentType doctype, int organisationId, bool generateFlag);
        List<AffectedStructures> GetAgreedRouteParts(int revisionid, string userschema);
		bool SaveAffectedMovementDetails(AffectedStructConstrParam affectedParam);

        #region Commeneted Code By Mahzeer on 13/07/2023
        /*
        bool GetCandRouteVehicleDetails(long routeRevisionID, string userSchema);
        string UpdateSortSpecialOrder(UpdateSORTSpecialOrderParams updateSortSpecialOrderParams);
        bool DeleteSpecialOrder(DeleteSpecialOrderParams deleteSpecialOrderParams);
        int GenerateSOProposalDocument(string EsdalReference, int OrganisationId, int ContactId, string DistributionComments, int VersionId, Dictionary<int, int> ICAStatusDictionary, string Esdalreference, HAContact HaContactDetail, AgreedRouteStructure Agreedroute, string UserSchema, int RoutePlanUnits, long ProjectStatus, int VersionNo, MovementPrint Moveprint, decimal PreVersionDistr, UserInfo SessionInfo);
        ApplyForVR1 SaveSORTVR1Application(SORTVR1ApplicationInsertParams sortVR1ApplicationInsertParams);
        SOApplication SaveSOApplication(SOApplication soApplication);
        */
        #endregion
    }
}
