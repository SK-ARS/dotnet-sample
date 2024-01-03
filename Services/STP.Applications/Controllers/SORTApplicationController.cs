using STP.Applications.Providers;
using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain;
using STP.Domain.Applications;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.Routes;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace STP.Applications.Controllers
{
    public class SORTApplicationController : ApiController
    {
        const string m_RouteName = " -SORTApplication";

        [HttpGet]
        [Route("SORTApplication/ListSortUser")]
        public IHttpActionResult ListSORTUser(long userTypeID, int checkerType = 0)
        {
            try
            {
                List<SORTUserList> result = SORTApplicationProvider.Instance.ListSORTUser(userTypeID, checkerType);
                return  Ok(result) ;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/ListSORTUser , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("SORTApplication/GetHaulierAppRevision")]
        public IHttpActionResult GetHaulierAppRevision(long projectID)
        {
            try
            {
                List<SORTMovementList> result = SORTApplicationProvider.Instance.GetHaulierAppRevision(projectID);
                return  Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/GetHaulierAppRevision , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("SORTApplication/GetMovmentVersion")]
        public IHttpActionResult GetMovmentVersion(long projectID)
        {
            try
            {
                List<SORTMovementList> result = SORTApplicationProvider.Instance.GetMovmentVersion(projectID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/GetMovmentVersion , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("SORTApplication/GetRelatedMovement")]
        public IHttpActionResult GetRelatedMovement(long applicationID, string type)
        {
            try
            {
                SOApplicationRelatedMov result = SORTApplicationProvider.Instance.GetRelatedMovement(applicationID, type);
                return  Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/GetRelatedMovement , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("SORTApplication/CandidateRouteVehicleConfiguration")]
        public IHttpActionResult CandidateRouteVehicleConfiguration(int revisionID, string userSchema = ApplicationConstants.DbUserSchemaSTPSORT, char rListType = ApplicationConstants.RListTypeC)
        {
            try
            {
                List<AppVehicleConfigList> result = SORTApplicationProvider.Instance.CandidateRouteVehicleConfiguration(revisionID, userSchema, rListType);
                return Ok(result) ;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/CandidateRouteVehicleConfiguration , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("SORTApplication/GetSortProjectDetails")]
        public IHttpActionResult GetSortProjectDetails(long projectID)
        {
            try
            {
                SORTLatestAppDetails result = SORTApplicationProvider.Instance.GetSortProjectDetails(projectID);
                return  Ok(result) ;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/GetSortProjectDetails , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("SORTApplication/GetSORTNotifiCode")]
        public IHttpActionResult GetSORTNotifiCode(int revisionID)
        {
            try
            {
                string result = SORTApplicationProvider.Instance.GetSORTNotifiCode(revisionID);
                return Ok(result) ;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/GetSORTNotifiCode , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("SORTApplication/GetMovHaulierNotes")]
        public IHttpActionResult GetMovHaulierNotes(long movementVersionID, string userSchema = ApplicationConstants.DbUserSchemaSTPSORT)
        {
            try
            {
                byte[] result = SORTApplicationProvider.Instance.GetMovHaulierNotes(movementVersionID, userSchema);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/GetMovHaulierNotes , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("SORTApplication/GetMovementHistory")]
        public IHttpActionResult GetMovementHistory(int pageNumber, int pageSize, string haulierNumber, int esdalReference, int versionNumber, long projectID, int? sortOrder = null, int? sortType = null)
        {
            try
            {
                List<MovementHistory> result = SORTApplicationProvider.Instance.GetMovementHistory(pageNumber, pageSize, haulierNumber, esdalReference, versionNumber, projectID,sortOrder,sortType);
                return Ok(result) ;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/GetMovementHistory , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("SORTApplication/GetVR1ApprovalDate")]
        public IHttpActionResult GetVR1ApprovalDate(long projectID)
        {
            try
            {
                string result = SORTApplicationProvider.Instance.GetVR1ApprovalDate(projectID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/GetVR1ApprovalDate , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("SORTApplication/GetRevIDFromApplication")]
        public IHttpActionResult GetRevIDFromApplication(long projectID)
        {
            try
            {
                long result = SORTApplicationProvider.Instance.GetRevIDFromApplication(projectID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/GetRevIDFromApplication , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("SORTApplication/GetCandidateRouteNM")]
        public IHttpActionResult GetCandidateRouteNM(long candidateRouteID)
        {
            try
            {
                string result = SORTApplicationProvider.Instance.GetCandidateRouteNM(candidateRouteID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/GetCandidateRouteNM , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("SORTApplication/GetSpecialOrderList")]
        public IHttpActionResult GetSpecialOrderList(long projectID)
        {
            try
            {
                List<SORTMovementList> result = SORTApplicationProvider.Instance.GetSpecialOrderList(projectID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/GetSpecialOrderList , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("SORTApplication/GetCandRouteVehicleDetails")]
        public IHttpActionResult GetCandRouteVehicleDetails(long routeRevisionID, string userSchema)
        {
            try
            {
                bool result = SORTApplicationProvider.Instance.GetCandRouteVehicleDetails(routeRevisionID, userSchema);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/GetCandRouteVehicleDetails , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("SORTApplication/CheckCandIsModified")]
        public IHttpActionResult CheckCandIsModified(int analysisID)
        {
            try
            {
                int result = SORTApplicationProvider.Instance.CheckCandIsModified(analysisID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/CheckCandIsModified , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("SORTApplication/CandRouteList")]
        public IHttpActionResult CandRouteList(int routeRevisionID, string userSchema = ApplicationConstants.DbUserSchemaSTPSORT, char rListType = ApplicationConstants.RListTypeC)
        {
            try
            {
                List<AppRouteList> result = SORTApplicationProvider.Instance.CandRouteList(routeRevisionID, userSchema, rListType);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/CandRouteList , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("SORTApplication/GetCandidateRTDetails")]
        public IHttpActionResult GetCandidateRTDetails(long projectID)
        {
            try
            {
                List<CandidateRTModel> result = SORTApplicationProvider.Instance.GetCandidateRTDetails(projectID);
                return Ok(result) ;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/GetCandidateRTDetails , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/SORTAppWithdrawandDecline")]
        public IHttpActionResult SORTAppWithdrawandDecline(SORTAppWithdrawAndDeclineParams sortApplnWithdrawandDeclineParams)
        {
            try
            {
                int response = SORTApplicationProvider.Instance.SORTAppWithdrawandDecline(sortApplnWithdrawandDeclineParams);
                return  Content(HttpStatusCode.OK, response) ;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/SORTAppWithdrawandDecline , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [Route("SORTApplication/SORTUnwithdraw")]
        public IHttpActionResult SORTUnwithdraw(SORTAppWithdrawAndDeclineParams sortApplnWithdrawandDeclineParams)
        {
            try
            {
                int response = SORTApplicationProvider.Instance.SORTUnwithdraw(sortApplnWithdrawandDeclineParams);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/SORTUnwithdraw , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        //
        [HttpPost]
        [Route("SORTApplication/SubmitSORTSoApplication")]
        public IHttpActionResult SubmitSORTSoApplication(SORTSOApplicationParams sortSoApplicationParams)
        {
            try
            {
                SOApplication response = SORTApplicationProvider.Instance.SubmitSORTSoApplication(sortSoApplicationParams);
                return Content(HttpStatusCode.Created, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/SubmitSORTSoApplication , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/UpdateSortSpecialOrder")]
        public IHttpActionResult UpdateSortSpecialOrder(UpdateSORTSpecialOrderParams updateSortSpecialOrderParams)
        {
            try
            {
                string response = SORTApplicationProvider.Instance.UpdateSortSpecialOrder(updateSortSpecialOrderParams.SORTSpecialOrder, updateSortSpecialOrderParams.RemovedCoverages);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/UpdateSortSpecialOrder , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }

        }
        [HttpDelete]
        [Route("SORTApplication/DeleteSpecialOrder")]
        public IHttpActionResult DeleteSpecialOrder(DeleteSpecialOrderParams deleteSpecialOrderParams)
        {
            try
            {
                int result = SORTApplicationProvider.Instance.DeleteSpecialOrder(deleteSpecialOrderParams.OrderNumber, deleteSpecialOrderParams.UserSchema);
                return result >= 0 ? Content(HttpStatusCode.OK, result) : (IHttpActionResult)Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);               
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/DeleteSpecialOrder , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }

        }
        [HttpGet]
        [Route("SORTApplication/Deletequicklinks")]
        public IHttpActionResult DeleteQuickLinks(long ProjectId)
        {
            try
            {
                int response = SORTApplicationProvider.Instance.DeleteQuickLinks(ProjectId);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/Deletequicklinks , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/SaveCandidateRoute")]
        public IHttpActionResult SaveCandidateRoute(CandidateRouteInsertParams candidateRouteSaveParams)
        {
            try
            {
                object response = SORTApplicationProvider.Instance.SaveCandidateRoute(candidateRouteSaveParams);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/SaveCandidateRoute , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/SaveSOApplication")]
        public IHttpActionResult SaveSOApplication(SOApplication soApplication)
        {
            try
            {
                SOApplication response = SORTApplicationProvider.Instance.SaveSOApplication(soApplication);
                return Content(HttpStatusCode.Created, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/SaveSOApplication , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/SaveSORTAllocateUser")]
        public IHttpActionResult SaveSORTAllocateUser(AllocateSORTUserInsertParams allocateSortUserParams)
        {
            try
            {
                List<string> response = SORTApplicationProvider.Instance.SaveSORTAllocateUser(allocateSortUserParams);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/SaveSORTAllocateUser , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/SaveSORTMovProjDetail")]
        public IHttpActionResult SaveSORTMovProjDetail(SORTMvmtProjectDetailsInsertParams sortMvmtProjectDetailsInsertParams)
        {
            try
            {
                int response = SORTApplicationProvider.Instance.SaveSORTMovProjDetail(sortMvmtProjectDetailsInsertParams);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/SaveSORTMovProjDetail , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/SubmitSORTVR1Application")]
        public IHttpActionResult SubmitSORTVR1Application(SubmitSORTVR1Params submitSORTParams )
        {
            try
            {
                ApplyForVR1 response = SORTApplicationProvider.Instance.SubmitSORTVR1Application(submitSORTParams);
                return Content(HttpStatusCode.Created, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/SubmitSORTVR1Application , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/SaveVR1Approval")]
        public IHttpActionResult SaveVR1Approval(VR1ApprovalInsertParams vr1ApprovalInsertParams)
        {
            try
            {
                int response = SORTApplicationProvider.Instance.SaveVR1Approval(vr1ApprovalInsertParams);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/SaveVR1Approval , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/SaveSORTVR1Application")]
        public IHttpActionResult SaveSORTVR1Application(SORTVR1ApplicationInsertParams sortVR1ApplicationInsertParams)
        {
            try
            {
                ApplyForVR1 response = SORTApplicationProvider.Instance.SaveSORTVR1Application(sortVR1ApplicationInsertParams);
                return Content(HttpStatusCode.Created, response);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - SORTApplication/SaveSORTVR1Application,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/SaveMovHaulierNotes")]
        public IHttpActionResult SaveMovHaulierNotes(HaulierMovNotesInsertParams haulierMovNotesInsertParams)
        {
            try
            {
                bool response = SORTApplicationProvider.Instance.SaveMovHaulierNotes(haulierMovNotesInsertParams);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/SaveMovHaulierNotes , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/ReviseVR1Application")]
        public IHttpActionResult ReviseVR1Application(ReviseVR1Params reviseVR1Params)
        {
            try
            {
                ApplyForVR1 objReviseVR1 = SORTApplicationProvider.Instance.ReviseVR1Application(reviseVR1Params);
                return Content(HttpStatusCode.OK, objReviseVR1);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/ReviseVR1Application , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/UpdateCandIsModified")]
        public IHttpActionResult UpdateCandIsModified(int analysisID)
        {
            try
            {
                SORTApplicationProvider.Instance.UpdateCandIsModified(analysisID);
                return Content(HttpStatusCode.OK, StatusMessage.Created);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/UpdateCandIsModified , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/UpdateProjectDetails")]
        public IHttpActionResult UpdateProjectDetails(UpdateProjectDetailsInsertParams updateProjectDetailsInsertParams)
        {
            try
            {
                decimal response = SORTApplicationProvider.Instance.UpdateProjectDetails(updateProjectDetailsInsertParams);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/UpdateProjectDetails , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/UpdateCollaborationView")]
        public IHttpActionResult UpdateCollaborationView(int documentID)
        {
            try
            {
                bool response = SORTApplicationProvider.Instance.UpdateCollaborationView(documentID);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/UpdateCollaborationView , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/UpdateSpecialOrder")]
        public IHttpActionResult UpdateSpecialOrder(UpdateSpecialOrderInsertParams updateSpecialOrderInsertParams)
        {
            try
            {
                int response = SORTApplicationProvider.Instance.UpdateSpecialOrder(updateSpecialOrderInsertParams);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/UpdateSpecialOrder , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }        
        [HttpPost]
        [Route("SORTApplication/CloneApplicationParts")]
        public IHttpActionResult CloneApplicationParts(CloneApplicationRTPartsInsertParams cloneApplicationRTPartsInsertParams)
        {
            try
            {
                SORTApplicationProvider.Instance.CloneApplicationParts(cloneApplicationRTPartsInsertParams);
                return Content(HttpStatusCode.OK, StatusMessage.Created);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/CloneApplicationParts , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/MovementVersionAgreeUnagreeWith")]
        public IHttpActionResult MovementVersionAgreeUnagreeWith(MovementVersionAgreeUnagreeWithInsertParams movementVersionAgreeUnagreeWithInsertParams)
        {
            try
            {
               int result= SORTApplicationProvider.Instance.MovementVersionAgreeUnagreeWith(movementVersionAgreeUnagreeWithInsertParams);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/MovementVersionAgreeUnagreeWith , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/InsertMovementVersion")]
        public IHttpActionResult InsertMovementVersion(InsertMovementVersionInsertParams insertMovementVersionInsertParams)
        {
            try
            {
                object result = SORTApplicationProvider.Instance.InsertMovementVersion(insertMovementVersionInsertParams);               
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/InsertMovementVersion , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/SpecialOrderUpdation")]
        public IHttpActionResult SpecialOrderUpdation(SpecialOrderUpdationInsertParams specialOrderUpdationInsertParams)
        {
            try
            {
                bool response = SORTApplicationProvider.Instance.SpecialOrderUpdation(specialOrderUpdationInsertParams);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/SpecialOrderUpdation , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/CheckerDetailsUpdation")]
        public IHttpActionResult CheckerDetailsUpdation(UpdateCheckerDetailsInsertParams updateCheckerDetailsInsertParams)
        {
            try
            {
                bool response = SORTApplicationProvider.Instance.UpdateCheckerDetails(updateCheckerDetailsInsertParams);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/CheckerDetailsUpdation , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/CloneRouteParts")]
        public IHttpActionResult CloneRouteParts(CloneRTPartsInsertParams cloneRTPartsInsertParams)
        {
            try
            {
                SORTApplicationProvider.Instance.CloneRouteParts(cloneRTPartsInsertParams);
                return Content(HttpStatusCode.OK, StatusMessage.Created);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/CloneRouteParts , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/SaveRouteRevision")]
        public IHttpActionResult SaveRouteRevision(RouteRevisionInsertParams routeRevisionInsertParams)
        {
            try
            {
                RouteRevision response = SORTApplicationProvider.Instance.SaveRouteRevision(routeRevisionInsertParams);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/SaveRouteRevision , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/AllocateSORTUser")]
        public IHttpActionResult AllocateSORTUser(AllocateSORTUserInsertParams allocateSortUserParams)
        {
            try
            {
                List<string> response = SORTApplicationProvider.Instance.SaveSORTAllocateUser(allocateSortUserParams);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/AllocateSORTUser , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("SORTApplication/GetCandRouteVehicleAssignDetails")]
        public IHttpActionResult GetCandRouteVehicleAssignDetails(long routeRevisionID, string userSchema)
        {
            try
            {
                bool result = SORTApplicationProvider.Instance.GetCandRouteVehicleAssignDetails(routeRevisionID, userSchema);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/GetCandRouteVehicleAssignDetails , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("SORTApplication/GetRouteType")]
        public IHttpActionResult GetRouteType(int revisionID, string userSchema)
        {
            try
            {
                List<CRDetails> result = SORTApplicationProvider.Instance.GetRouteType(revisionID, userSchema);
                return  Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/GetRouteType , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("SORTApplication/GetProjOverviewDetails")]
        public IHttpActionResult GetProjOverviewDetails(long revisionID)
        {
            try
            {
                SOApplication result = SORTApplicationProvider.Instance.GetProjOverviewDetails(revisionID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/GetProjOverviewDetails , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/SaveVR1Number")]
        public IHttpActionResult SaveVR1Number(VR1NumberInsertParams vr1NumberInsertParams)
        {
            try
            {
                string response = SORTApplicationProvider.Instance.SaveVR1Number(vr1NumberInsertParams);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/SaveVR1Number , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("SORTApplication/UpdateCandidateRoute")]
        public IHttpActionResult UpdateCandidateRoute(UpdateCandidateRouteNMInsertParams updateCandidateRouteNMInsertParams)
        {
            try
            {
                long response = SORTApplicationProvider.Instance.UpdateCandidateRouteNM(updateCandidateRouteNMInsertParams);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/UpdateCandidateRoute , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("SORTApplication/CheckVehicleOnRoute")]
        public IHttpActionResult CheckVehicleOnRoute(int projectID, int revisionID)
        {
            try
            {
                bool result = SORTApplicationProvider.Instance.CheckVehicleOnRoute(projectID, revisionID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"{m_RouteName}/CheckVehicleOnRoute , Exception: {ex}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("SORTApplication/GetSORTVR1GeneralDetails")]
        public IHttpActionResult GetSORTVR1GeneralDetails(int ProjectID, string userSchema)
        {
            try
            {
                ApplyForVR1 response = ApplicationProvider.Instance.GetSORTVR1GeneralDetails(ProjectID, userSchema);
                return Content(HttpStatusCode.OK, response);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - SORTApplication/GetSORTVR1GeneralDetails,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("SORTApplication/SaveResponseMessage")]
        public IHttpActionResult SaveResponseMessage(SaveResponseMessageParams saveResponseMessageParams)
        {
            try
            {
                bool result = SORTApplicationProvider.Instance.SaveResponseMessage(saveResponseMessageParams.UserId, saveResponseMessageParams.AutoResponseId, saveResponseMessageParams.OrganisationId, saveResponseMessageParams.ResponseMessage, saveResponseMessageParams.ResponsePdf);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - SORTApplication/SaveResponseMessage,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        #region GetSORTSOHaulierDetails
        [HttpGet]
        [Route("SORTApplication/GetSORTSOHaulierDetails")]
        public IHttpActionResult GetSORTSOHaulierDetails(long revisionId)
        {
            try
            {
                SOHaulierApplication sOApplicationObj = SORTApplicationProvider.Instance.GetSORTSOHaulierDetails(revisionId);
                return Content(HttpStatusCode.OK, sOApplicationObj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"SORTApplication/GetSORTSOHaulierDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        //Candidate route parts view
        #region GetAgreedRouteParts
        [HttpGet]
        [Route("SORTApplication/GetAgreedRouteParts")]
        public IHttpActionResult GetAgreedRouteParts(int revisionid, string userschema)
        {
            List<AffectedStructures> list = new List<AffectedStructures>();
            try
            {
                list = SORTApplicationProvider.Instance.GetAgreedRouteParts(revisionid, userschema);
                return Content(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"SORTApplication/GetAgreedRouteParts, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
    }
}
