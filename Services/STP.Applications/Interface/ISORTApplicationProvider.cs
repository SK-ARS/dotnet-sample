using STP.Domain.Applications;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.Routes;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;

namespace STP.Applications.Interface
{
    interface ISORTApplicationProvider
    {
        List<SORTMovementList> GetHaulierAppRevision(long projectID);
        List<SORTUserList> ListSORTUser(long userTypeID, int checkerType = 0);
        List<string> SaveSORTAllocateUser(AllocateSORTUserInsertParams allocateSORTUserParams);
        List<SORTMovementList> GetMovmentVersion(long projectID);
        List<CandidateRTModel> GetCandidateRTDetails(long projectID);
        SOApplicationRelatedMov GetRelatedMovement(long applicationID, string type);
        List<SORTUserList> ListSortUser(long userTypeID, int checkerType = 0);
        bool CheckVehicleOnRoute(int projectID, int revisionID);
        bool GetCandRouteVehicleDetails(long routeRevisionID, string userSchema);
        long GetRevIDFromApplication(long projectID);
        List<MovementHistory> GetMovementHistory(int pageNumber, int pageSize, string haulierNumber, int esdalReference, int versionNumber, long projectID, int? sortOrder = null, int? sortType = null);
        Byte[] GetMovHaulierNotes(long movementVersionID, string userSchema = ApplicationConstants.DbUserSchemaSTPSORT);
        bool GetCandRouteVehicleAssignDetails(long routeRevisionID, string userSchema);
        string GetCandidateRouteNM(long candidateRouteID);
        string GetVR1ApprovalDate(long projectID);
        SOApplication GetProjOverviewDetails(long revisionID);
        string GetSORTNotifiCode(int revisionID);
        List<CRDetails> GetRouteType(int revisionID, string userSchema);
        List<AppVehicleConfigList> CandidateRouteVehicleConfiguration(int revisionID, string userSchema = ApplicationConstants.DbUserSchemaSTPSORT, char rListType = ApplicationConstants.RListTypeC);
        int SORTAppWithdrawandDecline(SORTAppWithdrawAndDeclineParams sortApplnWithdrawAndDeclineParams);
        int CheckCandIsModified(int analysisID);
        SORTLatestAppDetails GetSortProjectDetails(long projectID);
        List<AppRouteList> CandRouteList(int routeRevisionID, string userSchema = ApplicationConstants.DbUserSchemaSTPSORT, char rListType = ApplicationConstants.RListTypeC);
        SOApplication SubmitSORTSoApplication(SORTSOApplicationParams sortSOApplicationParams);
        string UpdateSortSpecialOrder(SORTSpecialOrder sortSpecialOrder, List<string> removedCoverage);
        int DeleteSpecialOrder(string orderNumber, string userSchema);
        object SaveCandidateRoute(CandidateRouteInsertParams candidateRouteSaveParams);
        RouteRevision SaveRouteRevision(RouteRevisionInsertParams routeRevisionInsertParams);
        SOApplication SaveSOApplication(SOApplication soApplication);
        int SaveSORTMovProjDetail(SORTMvmtProjectDetailsInsertParams sortMvmtProjectDetailsInsertParams);
        ApplyForVR1 SubmitSORTVR1Application(SubmitSORTVR1Params submitSORTParams);
        int SaveVR1Approval(VR1ApprovalInsertParams vr1ApprovalInsertParams);
        ApplyForVR1 SaveSORTVR1Application(SORTVR1ApplicationInsertParams sortVR1ApplicationInsertParams);
        string SaveVR1Number(VR1NumberInsertParams vr1NumberInsertParams);
        bool SaveMovHaulierNotes(HaulierMovNotesInsertParams haulierMovNotesInsertParams);
        ApplyForVR1 ReviseVR1Application(ReviseVR1Params reviseVR1Params);
        long UpdateCandidateRouteNM(UpdateCandidateRouteNMInsertParams updateCandidateRouteNMInsertParams);
        void UpdateCandIsModified(int analysisID);
        decimal UpdateProjectDetails(UpdateProjectDetailsInsertParams updateProjectDetailsInsertParams);
        bool UpdateCollaborationView(int documentID);
        int UpdateSpecialOrder(UpdateSpecialOrderInsertParams updateSpecialOrderInsertParams);
        bool SpecialOrderUpdation(SpecialOrderUpdationInsertParams specialOrderUpdationInsertParams);
        int DeleteQuickLinks(long projectID);
        bool UpdateCheckerDetails(UpdateCheckerDetailsInsertParams updateCheckerDetailsInsertParams);
        void CloneRouteParts(CloneRTPartsInsertParams cloneRTPartsInsertParams);
        void CloneApplicationParts(CloneApplicationRTPartsInsertParams cloneApplicationRTPartsInsertParams);
        int MovementVersionAgreeUnagreeWith(MovementVersionAgreeUnagreeWithInsertParams movementVersionAgreeUnagreeWithInsertParams);
        object InsertMovementVersion(InsertMovementVersionInsertParams insertMovementVersionInsertParams);
        bool SaveResponseMessage(long userId, int autoResponseId, long organisationId, byte[] responseMessage, string responsePdf);
        SOHaulierApplication GetSORTSOHaulierDetails(long revisionId);
        //Candidate route parts view
        List<AffectedStructures> GetAgreedRouteParts(int revisionid, string userschema);
    }
}
