using STP.Applications.Interface;
using STP.Applications.Persistance;
using STP.Domain.Applications;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.Routes;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace STP.Applications.Providers
{
    public class SORTApplicationProvider : ISORTApplicationProvider
    {
        #region SORTApplicationProvider Singleton

        private SORTApplicationProvider()
        {
        }
        public static SORTApplicationProvider Instance
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
            internal static readonly SORTApplicationProvider instance = new SORTApplicationProvider();
        }
        #endregion

        /// <summary>
        /// ListSortUser
        /// </summary>
        /// <param name="userTypeID"></param>
        /// <param name="checkerType"></param>
        /// <returns></returns>
        public List<SORTUserList> ListSORTUser(long userTypeID, int checkerType = 0)
        {
            return SORTApplicationDAO.ListSortUserAppl(userTypeID, checkerType);
        }
        /// <summary>
        /// GetHaulierAppRevision
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public List<SORTMovementList> GetHaulierAppRevision(long projectID)
        {
            return SORTApplicationDAO.GetHaulierAppRevision(projectID);
        }
        /// <summary>
        /// GetMovmentVersion
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public List<SORTMovementList> GetMovmentVersion(long projectID)
        {
            return SORTApplicationDAO.GetMovmentVersion(projectID);
        }
        /// <summary>
        /// GetCandidateRTDetails
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public List<CandidateRTModel> GetCandidateRTDetails(long projectID)
        {
            return SORTApplicationDAO.GetCandidateRoutes(projectID);
        }
        /// <summary>
        /// GetRelatedMovement
        /// </summary>
        /// <param name="applicationID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public SOApplicationRelatedMov GetRelatedMovement(long applicationID, string type)
        {
            return SORTApplicationDAO.GetRelatedMovement(applicationID, type);
        }
        /// <summary>
        /// ListSortUser
        /// </summary>
        /// <param name="userTypeID"></param>
        /// <param name="checkerType"></param>
        /// <returns></returns>
        public List<SORTUserList> ListSortUser(long userTypeID, int checkerType = 0)
        {
            return SORTApplicationDAO.ListSortUserAppl(userTypeID, checkerType);
        }
        /// <summary>
        /// CandRouteList
        /// </summary>
        /// <param name="routeRevisionID"></param>
        /// <param name="userSchema"></param>
        /// <param name="rListType"></param>
        /// <returns></returns>
        public List<AppRouteList> CandRouteList(int routeRevisionID, string userSchema = ApplicationConstants.DbUserSchemaSTPSORT, char rListType = ApplicationConstants.RListTypeC)
        {
            return SORTApplicationDAO.CandidateRouteList(routeRevisionID, userSchema, rListType);
        }       
        /// <summary>
        /// CheckCandIsModified
        /// </summary>
        /// <param name="analysisID"></param>
        /// <returns></returns>
        public int CheckCandIsModified(int analysisID)
        {
            return SORTApplicationDAO.CheckCandIsModified(analysisID);
        }        
        /// <summary>
        /// GetCandRouteVehicleDetails
        /// </summary>
        /// <param name="routeRevisionID"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public bool GetCandRouteVehicleDetails(long routeRevisionID, string userSchema)
        {
            return SORTApplicationDAO.GetCandRouteVehicleDetails(routeRevisionID, userSchema);
        }
        /// <summary>
        /// GetCandRouteVehicleAssignDetails
        /// </summary>
        /// <param name="routeRevisionID"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public bool GetCandRouteVehicleAssignDetails(long routeRevisionID, string userSchema)
        {
            return SORTApplicationDAO.GetCandRouteVehicleAssignDetails(routeRevisionID, userSchema);
        }
        /// <summary>
        /// GetSpecialOrderList
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public List<SORTMovementList> GetSpecialOrderList(long projectID)
        {
            return SORTApplicationDAO.GetSpecialOrderList(projectID);
        }
        /// <summary>
        /// GetCandidateRouteNM
        /// </summary>
        /// <param name="candidateRouteID"></param>
        /// <returns></returns>
        public string GetCandidateRouteNM(long candidateRouteID)
        {
            return SORTApplicationDAO.GetCandidateRouteNM(candidateRouteID);
        }
        /// <summary>
        /// GetVR1ApprovalDate
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public string GetVR1ApprovalDate(long projectID)
        {
            return SORTApplicationDAO.GetVR1AppDate(projectID);
        }
        /// <summary>
        /// GetRevIDFromApplication
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public long GetRevIDFromApplication(long projectID)
        {
            return SORTApplicationDAO.GetRevIDFromApplication(projectID);
        }
        /// <summary>
        /// GetMovementHistory
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="haulierNumber"></param>
        /// <param name="esdalReference"></param>
        /// <param name="versionNumber"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public List<MovementHistory> GetMovementHistory(int pageNumber, int pageSize, string haulierNumber, int esdalReference, int versionNumber, long projectID,int? sortOrder=null,int? sortType=null)
        {
            return SORTApplicationDAO.GetMovementHistory(pageNumber, pageSize, haulierNumber, esdalReference, versionNumber, projectID,sortOrder,sortType);
        }
        /// <summary>
        /// GetSortProjectDetails
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public SORTLatestAppDetails GetSortProjectDetails(long projectID)
        {
            return SORTApplicationDAO.GetSortProjectDetails(projectID);
        }
        /// <summary>
        /// GetSORTNotifiCode
        /// </summary>
        /// <param name="revisionID"></param>
        /// <returns></returns>
        public string GetSORTNotifiCode(int revisionID)
        {
            return SORTApplicationDAO.GetSORTNotifiCode(revisionID);
        }
        /// <summary>
        /// GetMovHaulierNotes
        /// </summary>
        /// <param name="movementVersionID"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public Byte[] GetMovHaulierNotes(long movementVersionID, string userSchema = ApplicationConstants.DbUserSchemaSTPSORT)
        {
            return SORTApplicationDAO.GetMovHaulierNotes(movementVersionID, userSchema);
        }
        /// <summary>
        /// GetProjOverviewDetails
        /// </summary>
        /// <param name="revisionID"></param>
        /// <returns></returns>
        public SOApplication GetProjOverviewDetails(long revisionID)
        {
            return SORTApplicationDAO.GetProjOverviewDetails(revisionID);
        }
        /// <summary>
        /// CandidateRouteVehicleConfiguration
        /// </summary>
        /// <param name="revisionID"></param>
        /// <param name="userSchema"></param>
        /// <param name="rListType"></param>
        /// <returns></returns>
        public List<AppVehicleConfigList> CandidateRouteVehicleConfiguration(int revisionID, string userSchema = ApplicationConstants.DbUserSchemaSTPSORT, char rListType = ApplicationConstants.RListTypeC)
        {
            return SORTApplicationDAO.CandidateVehicleDetails(revisionID, userSchema, rListType);
        }
        /// <summary>
        /// SaveSORTAllocateUser
        /// </summary>
        /// <param name="allocateSORTUserParams"></param>
        /// <returns></returns>
        public List<string> SaveSORTAllocateUser(AllocateSORTUserInsertParams allocateSORTUserParams)
        {
            return SpecialOrderDAO.SaveSORTAllocateUser(allocateSORTUserParams);
        }
        /// <summary>
        /// SORTAppWithdrawandDecline
        /// </summary>
        /// <param name="sortApplnWithdrawandDeclineParams"></param>
        /// <returns></returns>
        public int SORTAppWithdrawandDecline(SORTAppWithdrawAndDeclineParams sortApplnWithdrawAndDeclineParams)
        {
            return SORTApplicationDAO.SORTAppWithdrawandDecline(sortApplnWithdrawAndDeclineParams);
        }
        /// <summary>
        /// SORTUnwithdraw
        /// </summary>
        /// <param name="sortApplnWithdrawandDeclineParams"></param>
        /// <returns></returns>
        public int SORTUnwithdraw(SORTAppWithdrawAndDeclineParams sortApplnWithdrawAndDeclineParams)
        {
            return SORTApplicationDAO.SORTUnwithdraw(sortApplnWithdrawAndDeclineParams);
        }
        //
        /// <summary>
        /// SubmitSORTSoApplication
        /// </summary>
        /// <param name="sortSOApplicationParams"></param>
        /// <returns></returns>
        public SOApplication SubmitSORTSoApplication(SORTSOApplicationParams sortSOApplicationParams)
        {
            return SORTApplicationDAO.SubmitSORTSOApplication(sortSOApplicationParams);
        }
        /// <summary>
        /// UpdateSortSpecialOrder
        /// </summary>
        /// <param name="sortSpecialOrder"></param>
        /// <param name="removedCoverage"></param>
        /// <returns></returns>
        public string UpdateSortSpecialOrder(SORTSpecialOrder sortSpecialOrder, List<string> removedCoverage)
        {
            return SpecialOrderDAO.UpdateSortSpecialOrder(sortSpecialOrder, removedCoverage);
        }
        /// <summary>
        /// DeleteSpecialOrder
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public int DeleteSpecialOrder(string orderNumber, string userSchema)
        {
            return SpecialOrderDAO.DeleteSpecialOrder(orderNumber, userSchema);
        }
        /// <summary>
        /// SaveCandidateRoute
        /// </summary>
        /// <param name="candidateRouteSaveParams"></param>
        /// <returns></returns>
        public object SaveCandidateRoute(CandidateRouteInsertParams candidateRouteSaveParams)
        {
            return SORTApplicationDAO.InsertCandidateRoute(candidateRouteSaveParams);
        }
        /// <summary>
        /// SaveRouteRevision
        /// </summary>
        /// <param name="routeRevisionInsertParams"></param>
        /// <returns></returns>
        public RouteRevision SaveRouteRevision(RouteRevisionInsertParams routeRevisionInsertParams)
        {
            return SORTApplicationDAO.InsertRouteRevision(routeRevisionInsertParams);
        }
        /// <summary>
        /// SaveSOApplication
        /// </summary>
        /// <param name="soApplication"></param>
        /// <returns></returns>
        public SOApplication SaveSOApplication(SOApplication soApplication)
        {
            return SORTApplicationDAO.SaveSOApplication(soApplication);
        }
        /// <summary>
        /// SaveSORTMovProjDetail
        /// </summary>
        /// <param name="sortMvmtProjectDetailsInsertParams"></param>
        /// <returns></returns>
        public int SaveSORTMovProjDetail(SORTMvmtProjectDetailsInsertParams sortMvmtProjectDetailsInsertParams)
        {
            return SORTApplicationDAO.SaveSORTMovProjDetail(sortMvmtProjectDetailsInsertParams);
        }
        /// <summary>
        /// SubmitSORTVR1Application
        /// </summary>
        /// <param name="appRevisionID"></param>
        /// <returns></returns>
        public ApplyForVR1 SubmitSORTVR1Application(SubmitSORTVR1Params submitSORTParams)
        {
            return SORTApplicationDAO.SubmitSORTVR1Application(submitSORTParams);
        }
        /// <summary>
        /// SaveVR1Approval
        /// </summary>
        /// <param name="vr1ApprovalInsertParams"></param>
        /// <returns></returns>
        public int SaveVR1Approval(VR1ApprovalInsertParams vr1ApprovalInsertParams)
        {
            return SpecialOrderDAO.SaveVR1Approval(vr1ApprovalInsertParams);
        }
        /// <summary>
        /// SaveSORTVR1Application
        /// </summary>
        /// <param name="sortVR1ApplicationInsertParams"></param>
        /// <returns></returns>
        public ApplyForVR1 SaveSORTVR1Application(SORTVR1ApplicationInsertParams sortVR1ApplicationInsertParams)
        {
            return SORTApplicationDAO.SaveSORTVR1Application(sortVR1ApplicationInsertParams);
        }
        /// <summary>
        /// SaveVR1Number
        /// </summary>
        /// <param name="vr1NumberInsertParams"></param>
        /// <returns></returns>
        public string SaveVR1Number(VR1NumberInsertParams vr1NumberInsertParams)
        {
            return SpecialOrderDAO.SaveVR1Number(vr1NumberInsertParams);
        }
        /// <summary>
        /// SaveMovHaulierNotes
        /// </summary>
        /// <param name="haulierMovNotesInsertParams"></param>
        /// <returns></returns>
        public bool SaveMovHaulierNotes(HaulierMovNotesInsertParams haulierMovNotesInsertParams)
        {
            return SORTApplicationDAO.InsertMovHaulierNotes(haulierMovNotesInsertParams);
        }
        /// <summary>
        /// ReviseVR1Application
        /// </summary>
        /// <param name="reviseVR1Params"></param>
        /// <returns></returns>
        public ApplyForVR1 ReviseVR1Application(ReviseVR1Params reviseVR1Params)
        {
            return SORTApplicationDAO.ReviseVR1Application(reviseVR1Params);
        }       
        /// <summary>
        /// UpdateCandIsModified
        /// </summary>
        /// <param name="analysisID"></param>
        public void UpdateCandIsModified(int analysisID)
        {
            SORTApplicationDAO.UpdateCandIsModified(analysisID);
        }
        /// <summary>
        /// UpdateProjectDetails
        /// </summary>
        /// <param name="updateProjectDetailsInsertParams"></param>
        /// <returns></returns>
        public decimal UpdateProjectDetails(UpdateProjectDetailsInsertParams updateProjectDetailsInsertParams)
        {
            return SORTApplicationDAO.UpdateProjectDetails(updateProjectDetailsInsertParams);
        }
        /// <summary>
        /// UpdateCollaborationView
        /// </summary>
        /// <param name="documentID"></param>
        /// <returns></returns>
        public bool UpdateCollaborationView(int documentID)
        {
            return SORTApplicationDAO.UpdateCollaborationView(documentID);
        }
        /// <summary>
        /// UpdateSpecialOrder
        /// </summary>
        /// <param name="updateSpecialOrderInsertParams"></param>
        /// <returns></returns>
        public int UpdateSpecialOrder(UpdateSpecialOrderInsertParams updateSpecialOrderInsertParams)
        {
            return SORTApplicationDAO.UpdateSpecialOrder(updateSpecialOrderInsertParams);
        }
        /// <summary>
        /// SpecialOrderUpdation
        /// </summary>
        /// <param name="specialOrderUpdationInsertParams"></param>
        /// <returns></returns>
        public bool SpecialOrderUpdation(SpecialOrderUpdationInsertParams specialOrderUpdationInsertParams)
        {
            return SORTApplicationDAO.SpecialOrderUpdation(specialOrderUpdationInsertParams);
        }
        /// <summary>
        /// Deletequicklinks
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public int DeleteQuickLinks(long projectID)
        {
            return SORTApplicationDAO.DeleteQuickLinks(projectID);
        }
        /// <summary>
        /// UpdateCheckerDetails
        /// </summary>
        /// <param name="updateCheckerDetailsInsertParams"></param>
        /// <returns></returns>
        public bool UpdateCheckerDetails(UpdateCheckerDetailsInsertParams updateCheckerDetailsInsertParams)
        {
            return SORTApplicationDAO.UpdateCheckerDetails(updateCheckerDetailsInsertParams);
        }
        /// <summary>
        /// CloneRTParts
        /// </summary>
        /// <param name="cloneRTPartsInsertParams"></param>
        public void CloneRouteParts(CloneRTPartsInsertParams cloneRTPartsInsertParams)
        {
            SORTApplicationDAO.CloneRTParts(cloneRTPartsInsertParams);
        }
        /// <summary>
        /// CloneApplicationRTParts
        /// </summary>
        /// <param name="cloneApplicationRTPartsInsertParams"></param>
        public void CloneApplicationParts(CloneApplicationRTPartsInsertParams cloneApplicationRTPartsInsertParams)
        {
            SORTApplicationDAO.CloneApplicationRTParts(cloneApplicationRTPartsInsertParams);
        }
        /// <summary>
        /// MovementVersionAgreeUnagreeWith
        /// </summary>
        /// <param name="movementVersionAgreeUnagreeWithInsertParams"></param>
        /// <returns></returns>
        public int MovementVersionAgreeUnagreeWith(MovementVersionAgreeUnagreeWithInsertParams movementVersionAgreeUnagreeWithInsertParams)
        {
            return SpecialOrderDAO.MovementVersionAgreeUnagreeWith(movementVersionAgreeUnagreeWithInsertParams);
        }
        /// <summary>
        /// InsertMovementVersion
        /// </summary>
        /// <param name="insertMovementVersionInsertParams"></param>
        /// <returns></returns>
        public object InsertMovementVersion(InsertMovementVersionInsertParams insertMovementVersionInsertParams)
        {
            return SORTApplicationDAO.InsertMovementVersion(insertMovementVersionInsertParams);
        }
        /// <summary>
        /// GetRouteType
        /// </summary>
        /// <param name="revisionID"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public List<CRDetails> GetRouteType(int revisionID, string userSchema)
        {
            return SORTApplicationDAO.GetRouteType(revisionID, userSchema);
        }
        /// <summary>
        /// UpdateCandidateRouteNM
        /// </summary>
        /// <param name="updateCandidateRouteNMInsertParams"></param>
        /// <returns></returns>
        public long UpdateCandidateRouteNM(UpdateCandidateRouteNMInsertParams updateCandidateRouteNMInsertParams)
        {
            return SORTApplicationDAO.UpdateCandidateRouteNM(updateCandidateRouteNMInsertParams);
        }
        /// <summary>
        /// CheckVehicleOnRoute
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="revisionID"></param>
        /// <returns></returns>
        public bool CheckVehicleOnRoute(int projectID, int revisionID)
        {
            return SORTApplicationDAO.CheckVehicleOnRoute(projectID, revisionID);
        }
        public bool SaveResponseMessage(long userId, int autoResponseId, long organisationId, byte[] responseMessage, string responsePdf)
        {
            return SORTApplicationDAO.InsertResponseMessageNotes(userId, autoResponseId, organisationId, responseMessage, responsePdf);
        }

        #region GetSORTSOHaulierDetails(RevisionID)
        public SOHaulierApplication GetSORTSOHaulierDetails(long revisionId)
        {
            return SORTApplicationDAO.GetSORTSOHaulApplDetails(revisionId);
        }
        #endregion

        //Candidate route parts view
        public List<AffectedStructures> GetAgreedRouteParts(int revisionid, string userschema)
        {
            return SORTApplicationDAO.GetCandidateAgreedRoutePart(revisionid, userschema);
        }
    }
}