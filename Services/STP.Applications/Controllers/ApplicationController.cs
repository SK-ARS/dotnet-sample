using STP.Applications.Providers;
using STP.Common.Constants;
using STP.Common.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using STP.Domain.Routes;
using STP.Domain.Structures;
using STP.Domain.SecurityAndUsers;
using STP.Domain.Applications;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.MovementsAndNotifications.Movements;

namespace STP.Applications.Controllers
{
    public class ApplicationController : ApiController
    {
        #region Get VR1 General
        [HttpGet]
        [Route("Application/GetVR1General")]
        public IHttpActionResult GetVR1General(string userSchema, long revisionId, long versionId, long organisationId, int historic)
        {
            ApplyForVR1 data = new ApplyForVR1();
            try
            {
                data = ApplicationProvider.Instance.GetVR1General(userSchema, revisionId, versionId, organisationId, historic);
                return Content(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/GetVR1General, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get SO Application Details
        [HttpGet]
        [Route("Application/GetSOApplicationTabDetails")]
        public IHttpActionResult GetSOApplicationTabDetails(long revisionId, long versionId, string userSchema, int historic)
        {
            try
            {
                SOApplicationTabs data = ApplicationProvider.Instance.GetSOApplicationTabDetails(revisionId, versionId, userSchema, historic);
                return Content(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/GetSOApplicationTabDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Reset Need Attention
        [HttpGet]
        [Route("Application/ResetNeedAttention")]
        public IHttpActionResult ResetNeedAttention(long projectID, long revisionID, long versionID)
        {
            int result = 0;
            try
            {
                result = ApplicationProvider.Instance.ResetNeedAttention(projectID, revisionID, versionID);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/ResetNeedAttention, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Update Needs Attention
        [HttpPost]
        [Route("Application/UpdateNeedsAttention")]
        public IHttpActionResult UpdateNeedsAttention(UpdateNeedsAttentionModel updateNeedsAttentionModel)
        {
            try
            {
                int result = ApplicationProvider.Instance.UpdateNeedsAttention(updateNeedsAttentionModel.NotificationId, updateNeedsAttentionModel.RevisionId, updateNeedsAttentionModel.NAFlag);
                if (result >= 0)
                {
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.UpdationFailed);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/UpdateNeedsAttention, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get Project Folder Details
        [HttpPost]
        [Route("Application/GetProjectFolderDetails")]
        public IHttpActionResult GetProjectFolderDetails(ProjectFolderModelParams objProjectFolderModelParams)
        {
            try
            {
                ProjectFolderModel objProjectFolder = ApplicationProvider.Instance.GetFolderDetails(objProjectFolderModelParams);
                return Content(HttpStatusCode.OK, objProjectFolder);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/GetProjectFolderDetails, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get Project Folder List
        [HttpGet]
        [Route("Application/GetProjectFolderList")]
        public IHttpActionResult GetProjectFolderList(int organisationId)
        {
            try
            {
              List<ProjectFolderModel>  list = ApplicationProvider.Instance.GetProjectFolderList(organisationId);
                return Content(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/GetProjectFolderList, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetStructureDetails
        /// <summary>
        /// getting structure general detail list from route assessment of affected structure's
        /// </summary>
        /// <param name="structureCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Application/GetStructureDetailList")]
        public IHttpActionResult GetStructureDetailList(string structureCode, int sectionId = 0)
        {
            try
            {
                List<AffStructureGeneralDetails> structureDetailList = ApplicationProvider.Instance.GetStructureDetailList(structureCode, sectionId);
                return Ok(structureDetailList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/GetStructureDetailList, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }



        #endregion

        #region GetHAContactDetails
        [HttpGet]
        [Route("Application/GetHAContactDetails")]
        public IHttpActionResult GetHAContactDetails(decimal contactId)
        {
            try
            {
                HAContact haContact = ApplicationProvider.Instance.GetHAContactDetails(contactId);
                return Ok(haContact);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/GetHAContactDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }

        }
        #endregion

        #region viewAffStructureSections
        /// <summary>
        /// getting affected struvture list
        /// </summary>
        /// <param name="StructureCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Application/ViewAffStructureSections")]
        public IHttpActionResult ViewAffStructureSections(string structureCode)
        {
            try
            {
                List<AffStructureSectionList> affStructureSectionLists = ApplicationProvider.Instance.ViewAffStructureSections(structureCode);
                return Ok(affStructureSectionLists);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/ViewAffStructureSections, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }

        }



        #endregion

        #region SOVR1SupplementaryInfo
        [HttpPost]
        [Route("Application/SOVR1SupplementaryInfo")]
        public IHttpActionResult AddSOVR1SupplementaryInfo(SupplimentaryInfoParams objSupplimentaryInfoParams)
        {
            bool result = false;
            try
            {
                result = ApplicationProvider.Instance.VR1SupplementaryInfo(objSupplimentaryInfoParams);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/SOVR1SupplementaryInfo, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region UpdatePartId
        [HttpPost]
        [Route("Application/UpdatePartId")]
        public IHttpActionResult UpdatePartId(UpdatePartIdInputParams updatePartIdInputParams)
        {
            try
            {
                int result = ApplicationProvider.Instance.UpdatePartId(updatePartIdInputParams);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/SOVR1SupplementaryInfo, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region UpdateVR1Application
        [HttpPost]
        [Route("Application/UpdateVR1Application")]
        public IHttpActionResult UpdateVR1Application(ApplyForVR1 vr1Application)
        {
            bool result = false;
            try
            {
                result = ApplicationProvider.Instance.UpdateVR1Application(vr1Application, vr1Application.OrganisationId, vr1Application.UserId, vr1Application.AppRevId);
                return Content(HttpStatusCode.Created, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/UpdateVR1Application, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region SaveVR1Application
        [HttpPost]
        [Route("Application/SaveVR1Application")]
        public IHttpActionResult SaveVR1Application(ApplyForVR1 vr1Application)
        {
            try
            {
                ApplyForVR1 applyForVR1 = ApplicationProvider.Instance.SaveVR1Application(vr1Application, vr1Application.OrganisationId, vr1Application.UserId);
                return Content(HttpStatusCode.Created, applyForVR1);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/SaveVR1Application, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetSOGeneralWorkinProcessbyrevisionid
        [HttpGet]
        [Route("Application/GetSOGeneralWorkinProcessbyrevisionid")]
        public IHttpActionResult GetSOGeneralWorkInProcessByRevisionId(string userSchema = UserSchema.Portal, long revisionId = 0, long versionId = 0, int Org_id = 0)
        {
            try
            {
                SOApplication response = ApplicationProvider.Instance.GetSOGeneralWorkInProcessByRevisionId(userSchema, revisionId, versionId, Org_id);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/GetSOGeneralWorkinProcessbyrevisionid,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);

            }
        }
        #endregion

        #region GetSOGeneralDetails
        [HttpGet]
        [Route("Application/GetSOGeneralDetails")]
        public IHttpActionResult GetSOGeneralDetails(long revisionId, long versionId, string userSchema, int historic)
        {
            try
            {
                SOApplication response = ApplicationProvider.Instance.GetSOGeneralDetails(revisionId, versionId, userSchema, historic);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/GetSOGeneralDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetSONumberStatus
        [HttpGet]
        [Route("Application/GetSONumberStatus")]
        public IHttpActionResult GetSONumberStatus(int projectId, string userSchema = UserSchema.Portal)
        {
            try
            {
                string response = ApplicationProvider.Instance.GetSONumberStatus(projectId, userSchema);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/GetSONumberStatus, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Delete Application
        [HttpDelete]
        [Route("Application/DeleteApplication")]
        public IHttpActionResult DeleteApplication(long appRevisionId, string userSchema)
        {
            int result;
            try
            {
                result = ApplicationProvider.Instance.DeleteApplication(appRevisionId, userSchema);
                return result >= 0 ? Content(HttpStatusCode.OK, result) : (IHttpActionResult)Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/DeleteApplication, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Withdraw Application
        [HttpGet]
        [Route("Application/WithdrawApplication")]
        public IHttpActionResult WithdrawApplication(long projectId, long appRevisionId)
        {
            ApplicationWithdraw withDrawApp;
            try
            {
                withDrawApp = ApplicationProvider.Instance.WithdrawApplication(projectId, appRevisionId);                
                return Content(HttpStatusCode.OK, withDrawApp);                
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/WithdrawApplication, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Check Latest Application Status
        [HttpGet]
        [Route("Application/CheckLatestAppStatus")]
        public IHttpActionResult CheckLatestAppStatus(long projectId)
        {
            SOApplication sOApplicationObj;
            try
            {
                sOApplicationObj = ApplicationProvider.Instance.CheckLatestAppStatus(projectId);
                if (sOApplicationObj.ApplicationRevId > 0)
                    return Content(HttpStatusCode.OK, sOApplicationObj);
                else
                    return Content(HttpStatusCode.NotFound, StatusMessage.NotFound);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/CheckLatestAppStatus, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Revise SO Application
        [HttpGet]
        [Route("Application/ReviseSOApplication")]
        public IHttpActionResult ReviseSOApplication(long apprevId, string userSchema)
        {
            try
            {
                SOApplication sOApplication = ApplicationProvider.Instance.ReviseSOApplication(apprevId, userSchema);
                if (sOApplication.ApplicationRevId > 0)
                    return Content(HttpStatusCode.OK, sOApplication);
                else
                    return Content(HttpStatusCode.NotFound, StatusMessage.NotFound);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/ReviseSOApplication, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Clone SO Application
        [HttpGet]
        [Route("Application/CloneSOApplication")]
        public IHttpActionResult CloneSOApplication(long appRevId, int organisationId, int userId)
        {
            try
            {
                SOApplication sOApplication = ApplicationProvider.Instance.CloneSOApplication(appRevId, organisationId, userId);
                if (sOApplication.ApplicationRevId > 0)
                    return Content(HttpStatusCode.OK, sOApplication);
                else
                    return Content(HttpStatusCode.NotFound, StatusMessage.NotFound);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/CloneSOApplication, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Clone SO History Application
        [HttpGet]
        [Route("Application/CloneSOHistoryApplication")]
        public IHttpActionResult CloneSOHistoryApplication(long appRevId, int organisationId, int userId, string userSchema)
        {
            try
            {
                SOApplication sOApplication = ApplicationProvider.Instance.CloneSOHistoryApplication(appRevId, organisationId, userId, userSchema);
                if (sOApplication.ApplicationRevId > 0)
                    return Content(HttpStatusCode.OK, sOApplication);
                else
                    return Content(HttpStatusCode.NotFound, StatusMessage.NotFound);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/CloneSOHistoryApplication, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Revise/Clone VR1 Application
        [HttpPost]
        [Route("Application/ReviseVR1Application")]
        public IHttpActionResult ReviseVR1Application(CloneReviseVR1Params cloneReviseVR1)
        {
            try
            {
                ApplyForVR1 objReviseVR1 = ApplicationProvider.Instance.ReviseVR1Application(cloneReviseVR1.ApplicationRevisionId, cloneReviseVR1.ReducedDet, cloneReviseVR1.CloneApp, cloneReviseVR1.VersionId, cloneReviseVR1.UserSchema);
                return Content(HttpStatusCode.OK, objReviseVR1);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/ReviseVR1Application, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Clone History VR1 Application
        [HttpPost]
        [Route("Application/CloneHistoryVR1Application")]
        public IHttpActionResult CloneHistoryVR1Application(CloneReviseVR1Params cloneReviseVR1)
        {
            try
            {
                ApplyForVR1 objReviseVR1 = ApplicationProvider.Instance.CloneHistoryVR1Application(cloneReviseVR1.ApplicationRevisionId, cloneReviseVR1.ReducedDet, cloneReviseVR1.CloneApp, cloneReviseVR1.VersionId, cloneReviseVR1.UserSchema);
                return Content(HttpStatusCode.OK, objReviseVR1);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/CloneHistoryVR1Application, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region SaveAppGeneral
        [HttpPost]
        [Route("Application/SaveAppGeneral")]
        public IHttpActionResult SaveAppGeneral(UpdateSOApplicationParams updateSOApplicationParams)
        {
            bool result;
            try
            {
                result = ApplicationProvider.Instance.SaveAppGeneral(updateSOApplicationParams.SOApplication, updateSOApplicationParams.OrganisationId, updateSOApplicationParams.UserId, updateSOApplicationParams.userSchema, updateSOApplicationParams.ApplicationRevisionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/SaveAppGeneral, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region SaveSOApplication
        [HttpPost]
        [Route("Application/SaveSOApplication")]
        public IHttpActionResult SaveSOApplication(UpdateSOApplicationParams updateSOApplicationParams)
        {
            try
            {
                long result = ApplicationProvider.Instance.SaveSOApplication(updateSOApplicationParams.SOApplication, updateSOApplicationParams.OrganisationId, updateSOApplicationParams.UserId);
                return Content(HttpStatusCode.Created, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/SaveSOApplication, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region CheckSOValidation
        [HttpGet]
        [Route("Application/CheckSOValidation")]
        public IHttpActionResult CheckSOValidation(int appRevisionId, string userSchema = UserSchema.Portal)
        {
            ApplyForVR1 objApplyForVR1 = new ApplyForVR1();
            try
            {
                objApplyForVR1 = ApplicationProvider.Instance.CheckSOValidation(appRevisionId, userSchema);
                return Ok(objApplyForVR1);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/CheckSOValidation, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetApplicationStatus
        [HttpGet]
        [Route("Application/GetApplicationStatus")]
        public IHttpActionResult GetApplicationStatus(int versionNo, int revisionNo, long projectId, string userSchema, int historic)
        {
            int result = 0;
            try
            {
                result = ApplicationProvider.Instance.GetApplicationStatus(versionNo, revisionNo, projectId, userSchema, historic);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/GetApplicationStatus, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetHAContDetFromInboundDoc
        [HttpGet]
        [Route("Application/GetHAContDetFromInboundDoc")]
        public IHttpActionResult GetHAContDetFromInboundDoc(string esdalReference)
        {
            HAContact ObjContactDet = new HAContact();
            try
            {
                ObjContactDet = ApplicationProvider.Instance.GetHAContDetFromInboundDoc(esdalReference);
                return Ok(ObjContactDet);
            }
            catch
            {
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region CheckVR1Validation
        [HttpGet]
        [Route("Application/CheckVR1Validation")]
        public IHttpActionResult CheckVR1Validation(int versionId, int showVehicle, string contentRef, int appRevisionId, string userSchema = UserSchema.Portal)
        {
            ApplyForVR1 objCheckValidation = new ApplyForVR1();
            try
            {
                objCheckValidation = ApplicationProvider.Instance.CheckVR1Validation(versionId, showVehicle, contentRef, appRevisionId, userSchema);
                return Ok(objCheckValidation);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/CheckVR1Validation, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region ListVR1RouteDetails
        [HttpGet]
        [Route("Application/ListVR1RouteDetails")]
        public IHttpActionResult ListVR1RouteDetails(string contentReference)
        {
            List<VR1RouteImport> objlistrt = new List<VR1RouteImport>();
            try
            {
                objlistrt = ApplicationProvider.Instance.ListVR1RouteDetails(contentReference);
                return Ok(objlistrt);
            }
            catch
            {
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region SubmitSoApplication
        [HttpPost]
        [Route("Application/SubmitSoApplication")]
        public IHttpActionResult SubmitSoApplication(SubmitSoParams submitSoParams, string workflowProcess = "")
        {
            SOApplication SOGeneralDetails;
            try
            {
                SOGeneralDetails = ApplicationProvider.Instance.SubmitSoApplication(submitSoParams.ApplicationRevisionId, submitSoParams.UserId);
                return Content(HttpStatusCode.Created, SOGeneralDetails);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/SubmitSoApplication, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region SubmitVR1Application
        [HttpPost]
        [Route("Application/SubmitVR1Application")]
        public IHttpActionResult SubmitVR1Application(SubmitVR1Params submitVR1Params, string workflowProcess = "")
        {
            ApplyForVR1 applyForVR1;
            try
            {
                applyForVR1 = ApplicationProvider.Instance.SubmitVR1Application(submitVR1Params.ApplicationRevisionId, submitVR1Params.ReducedDet);
                return Content(HttpStatusCode.Created, applyForVR1);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/SubmitVR1Application, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetVR1VehicleDetails
        [HttpPost]
        [Route("Application/GetVR1VehicleDetails")]
        public IHttpActionResult GetVR1VehicleDetails(VR1VehicleDetailsParams vr1VehicleDetailsParams)
        {
            ApplyForVR1 applyForVR1 = new ApplyForVR1();
            try
            {
                applyForVR1 = ApplicationProvider.Instance.GetVR1VehicleDetails(vr1VehicleDetailsParams);
                return Content(HttpStatusCode.OK, applyForVR1);
            }
            catch
            {
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetSOHaulierDetails
        [HttpGet]
        [Route("Application/GetSOHaulierDetails")]
        public IHttpActionResult GetSOHaulierDetails(long revisionId, long versionId, int historic)
        {
            try
            {
                SOHaulierApplication sOApplicationObj = ApplicationProvider.Instance.GetSOHaulierDetails(revisionId, versionId, historic);
                return Content(HttpStatusCode.OK,sOApplicationObj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/GetSOHaulierDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetHaulierApplRouteParts
        [HttpGet]
        [Route("Application/GetHaulierApplRouteParts")]
        public IHttpActionResult GetHaulierApplRouteParts(int revisionID, int appFlag = 0, int sortRouteVehicleFlag = 0, string userSchema = UserSchema.Portal)
        {
            try
            {
                List<AffectedStructures> affectedStructures = ApplicationProvider.Instance.GetHaulierApplRouteParts(revisionID, appFlag, sortRouteVehicleFlag, userSchema);
                return Ok(affectedStructures);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/GetHAContactDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetSORTHaulierAppRouteParts
        [HttpGet]
        [Route("Application/GetSORTHaulierAppRouteParts")]
        public IHttpActionResult GetSORTHaulierAppRouteParts(int versionID, string vr1ContentRefNo, string userSchema = UserSchema.Portal)
        {
            try
            {
                List<AffectedStructures> affectedStructures = ApplicationProvider.Instance.GetSORTHaulierAppRouteParts(versionID, vr1ContentRefNo, userSchema);
                return Ok(affectedStructures);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/GetHAContactDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }

        }
        #endregion

        #region InsertApplicationType
        [HttpPost]
        [Route("Application/InsertApplicationType")]
        public IHttpActionResult InsertApplicationType(PlanMovementType saveAppType)
        {
            try
            {
                AppGeneralDetails appGeneral = ApplicationProvider.Instance.InsertApplicationType(saveAppType);
                return Content(HttpStatusCode.Created, appGeneral);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/InsertApplicationType, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("Application/UpdateApplicationType")]
        public IHttpActionResult UpdateApplicationType(PlanMovementType updateAppType)
        {
            try
            {
                AppGeneralDetails appGeneral = ApplicationProvider.Instance.UpdateApplicationType(updateAppType);
                return Content(HttpStatusCode.Created, appGeneral);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/UpdateApplicationType, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region VR1GetSupplementaryInfo
        [HttpGet]
        [Route("Application/VR1GetSupplementaryInfo")]
        public IHttpActionResult VR1GetSupplementaryInfo(int apprevisionId = 0, string userSchema = UserSchema.Portal, int historic = 0)
        { 
            try
            {
                SupplimentaryInfo response = ApplicationProvider.Instance.VR1GetSupplementaryInfo(apprevisionId, userSchema, historic);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/VR1GetSupplementaryInfo, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetNotifRouteParts
        [HttpGet]
        [Route("Application/GetNotifRouteParts")]
        public IHttpActionResult GetNotifRouteParts(int notificationId, int rpFlag)
        {
            try
            {
                List<AffectedStructures> response = ApplicationProvider.Instance.GetNotifRouteParts(notificationId, rpFlag);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/GetNotifRouteParts, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region IMP_CondidateRoue
        [HttpGet]
        [Route("Application/IMP_CondidateRoue")]
        public IHttpActionResult IMP_CondidateRoue(int RoutePartId, int AppRevId, int VersionId, string ContentRef, string userSchema)
        {
            try
            {
                long result = ApplicationProvider.Instance.IMP_CondidateRoue(RoutePartId, AppRevId, VersionId, ContentRef, userSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/IMP_CondidateRoue,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);

            }
        }
        #endregion

        #region EsdalRefNum
        [HttpGet]
        [Route("Application/EsdalRefNum")]
        public IHttpActionResult EsdalRefNum(int SOVersionID)
        {
            try
            {
                string result = ApplicationProvider.Instance.EsdalRefNum(SOVersionID);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/EsdalRefNum,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);

            }
        }
        #endregion

        #region GetAgreedRouteParts
        [HttpGet]
        [Route("Application/GetAgreedRouteParts")]
        public IHttpActionResult GetAgreedRouteParts(int VersionId, int revisionid, string userSchema, string ContentRefNo = "")
        {
            List<AffectedStructures> list = new List<AffectedStructures>();
            try
            {
                list = ApplicationProvider.Instance.GetAgreedRoutePart(VersionId, revisionid, userSchema, ContentRefNo);
                return Content(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/GetAgreedRouteParts,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);

            }
        }

        #endregion

        #region GetApplicationDetails
        [HttpGet]
        [Route("Application/GetApplicationDetails")]
        public IHttpActionResult GetApplicationDetails(long revisionId, string userSchema)
        {
            try
            {
                PlanMovementType response = ApplicationProvider.Instance.GetApplicationDetails(revisionId, userSchema);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/GetApplicationDetails,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);

            }
        }
        #endregion

        #region GetNotificationDetails
        [HttpGet]
        [Route("Application/GetNotificationDetails")]
        public IHttpActionResult GetNotificationDetails(long notificationId, string userSchema)
        {
            try
            {
                PlanMovementType response = ApplicationProvider.Instance.GetNotificationDetails(notificationId, userSchema);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/GetApplicationDetails,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);

            }
        }
        #endregion
    }
}
