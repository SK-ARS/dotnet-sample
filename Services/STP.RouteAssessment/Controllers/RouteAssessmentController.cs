using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain.Custom;
using STP.Domain.RouteAssessment;
using STP.Domain.RouteAssessment.AssessmentOutput;
using STP.Domain.Structures;
using STP.RouteAssessment.Providers;
using STP.RouteAssessment.RouteAssessment;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using static STP.Domain.Routes.RouteModel;

namespace STP.RouteAssessment.Controllers
{
    public class RouteAssessmentController : ApiController
    {

        [HttpGet]
        [Route("RouteAssessment/GetLibraryNotes")]
        public IHttpActionResult GetLibraryNotes(int OrgId, int LibraryNoteId, int UserId)
        {
            try
            {
                List<LibraryNotes> result = RouteAssessmentProvider.Instance.GetLibraryNotes(OrgId, LibraryNoteId, UserId, UserSchema.Portal);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"RouteAssessment/GetLibraryNotes, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("RouteAssessment/InsertLibraryNotes")]
        public IHttpActionResult InsertLibraryNotes(LibraryNotes objCheckValidation)
        {
            try
            {
                int result = RouteAssessmentProvider.Instance.InsertLibraryNotes(objCheckValidation, UserSchema.Portal);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"RouteAssessment/InsertLibraryNotes, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("RouteAssessment/GetDriveInstructionsinfo")]
        public IHttpActionResult GetDriveInstructionsInfo(long analysisId, int analysisType, string userSchema, int? sortOrder = null, int? presetFilter = null)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"RouteAssessment/GetDriveInstructionsInfo_service, analysisId: ", analysisId+','+ analysisType + ',' + userSchema);
                RouteAssessmentModel result = RouteAssessmentProvider.Instance.GetDriveInstructionsInfo(analysisId, analysisType, userSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"RouteAssessment/GetDriveInstructionsInfo, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }


        [HttpPost]
        [Route("RouteAssessment/UpdateRouteAssessment")]
        public IHttpActionResult UpdateRouteAssessment(UpdateRouteAssessmentParam updateRouteAssessmentParam)
        {
            try
            {
                int result = RouteAssessmentProvider.Instance.UpdateRouteAssessment(updateRouteAssessmentParam.ContentReferenceNo, updateRouteAssessmentParam.RevisionId, updateRouteAssessmentParam.OrganisationId, updateRouteAssessmentParam.AnalysisId, updateRouteAssessmentParam.AnalysisType, updateRouteAssessmentParam.UserSchema);
                return Ok(result);
            }
            catch(Exception ex)
            {
				Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"RouteAssessment/UpdateRouteAssessment, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #region UpdateRouteAssessment 7 parameters
        [HttpPost]
        [Route("RouteAssessment/UpdateAlsatAssessment")]
        public IHttpActionResult UpdateAlsatAssessment(UpdateRouteAssessmentSevenParams updateRouteAssessmentSevenParams)
        {
            try
            {
                int status = RouteAssessmentProvider.Instance.UpdateAlsatAssessment(updateRouteAssessmentSevenParams.AnalysisId, updateRouteAssessmentSevenParams.RouteId, updateRouteAssessmentSevenParams.UserSchema, updateRouteAssessmentSevenParams.AssessmentResult);
                return Ok(status);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/UpdateRouteAssessment Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        [HttpGet]
        [Route("RouteAssessment/GetRouteDetailForAnalysis")]
        public IHttpActionResult GetRouteDetailForAnalysis(int analysisId, long versionId, string contentRefNo, int revisionId, string userSchema)
        {
            try
            {
                List<RoutePartDetails> result = RouteAssessmentProvider.Instance.GetRouteDetailForAnalysis(analysisId, versionId, contentRefNo, revisionId, userSchema);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteAssessmentDAO/GetRouteDetailForAnalysis, Exception : ​​​​​​​",ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("RouteAssessment/GetCountryId")]
        public IHttpActionResult GetCountryId(int routeID = 0)
        {
            try
            {
                List<int> result = RouteAssessmentProvider.Instance.GetCountryId(routeID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteAssessmentDAO/GetCountryId, Exception: ​​​​​​​",ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("RouteAssessment/RouteAssessmentDI")]
        public IHttpActionResult UpdateRouteAssessmentDI(RouteAssessmentDIParam routeAssessmentDIParam)
        {
            try
            {
                int result = RouteAssessmentProvider.Instance.UpdateRouteAssessment(routeAssessmentDIParam.Inputs, routeAssessmentDIParam.AnalType, routeAssessmentDIParam.UserSchema);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"RouteAssessment/RouteAssessmentDI, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("RouteAssessment/RouteAssessmentVR1")]
        public IHttpActionResult UpdateRouteAssessmentVR1(UpdateRouteAssessmentVrParam updateRouteAssessmentVrParam)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"RouteAssessment/UpdateRouteAssessmentVrParam_service, updateRouteAssessmentVrParam: ", updateRouteAssessmentVrParam.AnalysisId+','+ updateRouteAssessmentVrParam.AnalysisType + ',' + updateRouteAssessmentVrParam.OrganisationId + ',' + updateRouteAssessmentVrParam.UserSchema + ',' + updateRouteAssessmentVrParam.VersionId);
                int result = RouteAssessmentProvider.Instance.updateRouteAssessment(updateRouteAssessmentVrParam.VersionId, updateRouteAssessmentVrParam.OrganisationId, updateRouteAssessmentVrParam.AnalysisId, updateRouteAssessmentVrParam.AnalysisType, updateRouteAssessmentVrParam.UserSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"RouteAssessment/UpdateRouteAssessmentVR1, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("RouteAssessment/UpdateRouteAssessmentAll")]
        public IHttpActionResult UpdateRouteAssessmentAll(UpdateRouteAssessmentParam updateRouteAssessmentParam)
        {
            try
            {
                int result = RouteAssessmentProvider.Instance.updateRouteAssessment(updateRouteAssessmentParam.ContentReferenceNo, updateRouteAssessmentParam.RevisionId, updateRouteAssessmentParam.OrganisationId, updateRouteAssessmentParam.AnalysisId, updateRouteAssessmentParam.AnalysisType, updateRouteAssessmentParam.UserSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"RouteAssessment/UpdateRouteAssessmentAll, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("RouteAssessment/UpdateRouteAssessmentRoads")]
        public IHttpActionResult UpdateRouteAssessmentRoads(string contentRefNo, int notificationId, int revisionId, int versionId, int orgId, int analysisId, int analType, string userSchema, string VSOType = "")
        {
            try
            {
                int result = RouteAssessmentProvider.Instance.updateRouteAssessment(contentRefNo, notificationId, revisionId, versionId, orgId, analysisId, analType, userSchema, VSOType);
                return result != 0 ? Ok(result) : (IHttpActionResult)Content(HttpStatusCode.NotFound, StatusMessage.NotFound);
            }
            catch
            {
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("RouteAssessment/UpdateRouteAssessmentModel")]
        public IHttpActionResult UpdateRouteAssessmentModel(UpdateAssessmentModelParam updateAssessmentModelParam)
        {
            try
            {
                long result = RouteAssessmentProvider.Instance.updateRouteAssessment(updateAssessmentModelParam.RouteAssessmentModel, updateAssessmentModelParam.AnalysisId, updateAssessmentModelParam.UserSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RouteAssessment/UpdateRouteAssessmentModel, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
      
        [HttpGet]
        [Route("RouteAssessment/GetMovementStatus")]
        public IHttpActionResult GetMovementStatus(int versionId, string contentReferenceNo, string userSchema)
        {
            try
            {
                long result = RouteAssessmentProvider.Instance.GetMovementStatus(versionId, contentReferenceNo, userSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RouteAssessment/GetMovementStatus, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("RouteAssessment/FetchPreviousAffectedList")]
        public IHttpActionResult FetchPreviousAffectedList(int analysisId, int analysisType, string userSchema)
        {
            try
            {
                AnalysedRoute result = RouteAssessmentProvider.Instance.fetchPreviousAffectedParties(analysisId, analysisType, userSchema);
                return Ok(result);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RouteAssessment/FetchPreviousAffectedList, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("RouteAssessment/MovementClearRouteAssessment")]
        public IHttpActionResult MovementClearRouteAssessment(long revisionId, string userSchema)
        {
            try
            {
                bool result = RouteAssessmentProvider.Instance.MovementClearRouteAssessment(revisionId, userSchema);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/MovementClearRouteAssessment Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("RouteAssessment/GetInstantStructureAnalysis")]
        public IHttpActionResult GetInstantStructureAnalysis(RouteAssessmentParams assessmentParams)
        {
            try
            {
                List<StructureInfo> structureInfoList = RouteAssessmentProvider.Instance.GetInstantStructureAnalysis(assessmentParams.routePartId, assessmentParams.routePart, assessmentParams.userSchema);
                return Ok(structureInfoList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GetInstantStructureAnalysis Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("RouteAssessment/GetInstantConstraintAnalysis")]
        public IHttpActionResult GetInstantConstraintAnalysis(RouteAssessmentParams assessmentParams)
        {
            try
            {
                List<RouteConstraints> routeConstraints = RouteAssessmentProvider.Instance.GetInstantConstraintAnalysis(assessmentParams.routePartId, assessmentParams.routePart, assessmentParams.userSchema);
                return Ok(routeConstraints);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GetInstantStructureAnalysis Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #region GetAffectedConstraintInfoList
        [HttpGet]
        [Route("RouteAssessment/FetchConstraintList")]
        public IHttpActionResult FetchConstraintList(int routeId, int routeType, string userSchema)
        {
            try
            {
                List<RouteConstraints> routeConstraints = RouteAssessmentProvider.Instance.FetchConstraintList(routeId, routeType, userSchema);
                return Ok(routeConstraints);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/FetchConstraintList Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetAffectedStructureInfoList
        [HttpGet]
        [Route("RouteAssessment/FetchAffectedStructureInfoList")]
        public IHttpActionResult FetchAffectedStructureInfoList(int routeId, int routeType, string userSchema)
        {
            try
            {
                List<StructureInfo> structureInfoList = RouteAssessmentProvider.Instance.FetchAffectedStructureInfoList(routeId, routeType, userSchema);
                return Ok(structureInfoList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/FetchAffectedStructureInfoList Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetAffectedStructureInfoListSO
        [HttpGet]
        [Route("RouteAssessment/FetchAffectedStructureListSO")]
        public IHttpActionResult FetchAffectedStructureListSO(int routeId, int routeType, string userSchema)
        {
            try
            {
                List<StructureInfo> structureInfoList = RouteAssessmentProvider.Instance.FetchAffectedStructureListSO(routeId, routeType, userSchema);
                return Ok(structureInfoList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/FetchAffectedStructureInfoList Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        [HttpGet]
        [Route("RouteAssessment/AffectedStructureInfoList")]
        public IHttpActionResult AffectedStructureInfoList(int routeId)
        {
            try
            {
                List<StructureInfo> structureInfoList = RouteAssessmentProvider.Instance.AffectedStructureInfoList(routeId);
                return Ok(structureInfoList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/AffectedStructureInfoList Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("RouteAssessment/FetchAffectedStructureAtPoints")]
        public IHttpActionResult FetchAffectedStructureAtPoints(int routeId, long routeVar, string userSchema, int routeType)
        {
            try
            {
                List<StructureInfo> structureInfoList = RouteAssessmentProvider.Instance.FetchAffectedStructureAtPoints(routeId,routeVar, userSchema, routeType);
                return Ok(structureInfoList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/FetchAffectedStructureAtPoints Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #region GetAffectedGetConstraintId
        [HttpGet]
        [Route("RouteAssessment/GetConstraintId")]
        public IHttpActionResult GetConstraintId(string ConstraintCode)
        {
            try
            {
                decimal Constraintcode = RouteAssessmentProvider.Instance.GetConstraintId(ConstraintCode);
                return Ok(Constraintcode);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GetCautionList Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        [HttpPost]
        [Route("RouteAssessment/RetainManualAddedAndCompareForAllVersions")]
        public IHttpActionResult RetainManualAddedAndCompareForAllVersions(AffectedParties affectedParties)
        {
            try
            {
                bool result = StringExtractor.RetainManualAddedAndCompareForAllVersions(affectedParties.AnalysisId, affectedParties.DistributedMovAnalysisId, affectedParties.Inputs, affectedParties.AffectedPartie, affectedParties.userSchema);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/RetainManualAddedAndCompareForAllVersions Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("RouteAssessment/SortAffectedPartyBasedOnOrganisation")]
        public IHttpActionResult SortAffectedPartyBasedOnOrganisation(AffectedPartyBasedOnOrganisation affectedParty)
        {
            try
            {
                bool result = StringExtractor.SortAffectedPartyBasedOnOrganisation(affectedParty.RouteAssessmentModel, affectedParty.AnalysisId, affectedParty.UserSchema);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/SortAffectedPartyBasedOnOrganisation Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("RouteAssessment/xmlAffectedPartyExcludeDeserializer")]
        public IHttpActionResult xmlAffectedPartyExcludeDeserializer(string xml, int contactId, int organisationId, string organisationName)
        {
            try
            {
                string result = StringExtractor.xmlAffectedPartyExcludeDeserializer(xml,contactId,organisationId,organisationName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/xmlAffectedPartyExcludeDeserializer Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        
        [HttpGet]
        [Route("RouteAssessment/xmlAffectedPartyIncludeDeserializer")]
        public IHttpActionResult xmlAffectedPartyIncludeDeserializer(string xml, int contactId, int organisationId, string organisationName)
        {
            try
            {
                string result = StringExtractor.xmlAffectedPartyExcludeDeserializer(xml, contactId, organisationId, organisationName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/xmlAffectedPartyIncludeDeserializer Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("RouteAssessment/ModifyingDispStatusToInUse")]
        public IHttpActionResult ModifyingDispStatusToInUse(int NotificationID, Domain.RouteAssessment.XmlAffectedParties.AffectedPartiesStructure existingAfftdPartyObj, int AnalysisId, string userSchema)
        {
            try
            {
                int result = XMLModifier.ModifyingDispStatusToInUse(NotificationID, existingAfftdPartyObj, AnalysisId, userSchema);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/xmlAffectedPartyIncludeDeserializer Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #region SaveAndFetchContacts
        [HttpGet]
        [Route("RouteAssessment/SaveAndFetchContacts")]
        public IHttpActionResult SaveAndFetchContacts(int contactId, int notificationId, string userSchema)
        {
            try
            {
                List<AssessmentContacts> result = RouteAssessmentProvider.Instance.SaveAndFetchContacts(contactId, notificationId, userSchema);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/SaveAndFetchContacts Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region FetchContactDetails
        [HttpGet]
        [Route("RouteAssessment/FetchContactDetails")]
        public IHttpActionResult FetchContactDetails(int organisationId, int revisionId, string userSchema)
        {
            try
            {
                List<AssessmentContacts> result = RouteAssessmentProvider.Instance.FetchContactDetails(organisationId, revisionId, userSchema);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/SaveAndFetchContacts Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region XmlAffectedPartyDeserializer
        [HttpPost]
        [Route("RouteAssessment/XmlAffectedPartyDeserializer")]
        public IHttpActionResult XmlAffectedPartyDeserializer(StringExtractorParams stringExtractorParams)
        {
            try
            {
                string result = RouteAssessmentProvider.Instance.XmlAffectedPartyDeserializer(stringExtractorParams.XmlData,stringExtractorParams.ManualContactList);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/XmlAffectedPartyDeserializer Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region FetchContactDetails
        [HttpPost]
        [Route("RouteAssessment/XmlAffectedPartyDeleteFromXml")]
        public IHttpActionResult XmlAffectedPartyDeleteFromXml(DeleteAffectedParams deleteAffectedParams)
        {
            try
            {
                string result = RouteAssessmentProvider.Instance.xmlAffectedPartyDeleteFromXml(deleteAffectedParams.XmlData, deleteAffectedParams.OrganisationName, deleteAffectedParams.FullName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/XmlAffectedPartyDeleteFromXml Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        [HttpGet]
        [Route("RouteAssessment/GetAssessmentResult")]
        public IHttpActionResult GetAssessmentResult(int analysisId, string userSchema)
        {
            try
            {
                string assessmentNote = RouteAssessmentProvider.Instance.GetAssessmentResult(analysisId, userSchema);
                return Content(HttpStatusCode.OK, assessmentNote);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GetAssessmentResult Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        #region Generate Route Assessment

        [HttpPost]
        [Route("RouteAssessment/GenerateAffectedStructure")]
        public IHttpActionResult GenerateAffectedStructure(GenerateRouteAssessment generateAffectedStructure)
        {
            try
            {
                string affectedStructure = RouteAssessmentProvider.Instance.GenerateAffectedStructures(generateAffectedStructure.RoutePart , generateAffectedStructure.AnalysedStructures , generateAffectedStructure.OrganisationId, generateAffectedStructure.UserSchema);
                byte[] affectedStructures = !string.IsNullOrWhiteSpace(affectedStructure) ? StringExtraction.ZipAndBlob(affectedStructure) : null;
                return Content(HttpStatusCode.OK, affectedStructures);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteAssessment/GenerateAffectedStructure Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("RouteAssessment/GenerateAffectedParties")]
        public IHttpActionResult GenerateAffectedParties(GenerateRouteAssessment generateAffectedParty)
        {
            try
            {
                string affectedParty = RouteAssessmentProvider.Instance.GenerateAffectedParties(generateAffectedParty.RoutePart, generateAffectedParty.NotificationId, generateAffectedParty.OrganisationId, generateAffectedParty.UserSchema, generateAffectedParty.VSOType);
                byte[] affectedParties = !string.IsNullOrWhiteSpace(affectedParty) ? StringExtraction.ZipAndBlob(affectedParty) : null;
                return Content(HttpStatusCode.OK, affectedParties);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteAssessment/GenerateAffectedParties Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("RouteAssessment/GenerateAffectedCautions")]
        public IHttpActionResult GenerateAffectedCautions(GenerateRouteAssessment generateAffectedCaution)
        {
            try
            {
                string affectedCaution = RouteAssessmentProvider.Instance.GenerateAffectedCautions(generateAffectedCaution.RoutePart, generateAffectedCaution.OrganisationId, generateAffectedCaution.UserSchema);
                byte[] affectedCautions = !string.IsNullOrWhiteSpace(affectedCaution) ? StringExtraction.ZipAndBlob(affectedCaution) : null;
                return Content(HttpStatusCode.OK, affectedCautions);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteAssessment/GenerateAffectedCautions Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("RouteAssessment/GenerateAffectedConstraints")]
        public IHttpActionResult GenerateAffectedConstraints(GenerateRouteAssessment generateAffectedConstraints)
        {
            try
            {
                string affectedConstraint = RouteAssessmentProvider.Instance.GenerateAffectedConstraints(generateAffectedConstraints.RoutePart, generateAffectedConstraints.UserSchema);
                byte[] affectedConstraints = !string.IsNullOrWhiteSpace(affectedConstraint) ? StringExtraction.ZipAndBlob(affectedConstraint) : null;
                return Content(HttpStatusCode.OK, affectedConstraints);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteAssessment/GenerateAffectedConstraints Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("RouteAssessment/GenerateAffectedAnnotation")]
        public IHttpActionResult GenerateAffectedAnnotation(GenerateRouteAssessment generateAffectedAnnotation)
        {
            try
            {
                string affectedAnnotation = RouteAssessmentProvider.Instance.GenerateAffectedAnnotation(generateAffectedAnnotation.RoutePart, generateAffectedAnnotation.UserSchema);
                byte[] affectedAnnotations = !string.IsNullOrWhiteSpace(affectedAnnotation) ? StringExtraction.ZipAndBlob(affectedAnnotation) : null;
                return Content(HttpStatusCode.OK, affectedAnnotations);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteAssessment/GenerateAffectedAnnotation Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("RouteAssessment/GenerateAffectedRoads")]
        public IHttpActionResult GenerateAffectedRoads(GenerateRouteAssessment generateAffectedRoads)
        {
            try
            {
                string affectedRoad = RouteAssessmentProvider.Instance.GenerateAffectedRoads(generateAffectedRoads.RoutePart, generateAffectedRoads.UserSchema);
                byte[] affectedRoads = !string.IsNullOrWhiteSpace(affectedRoad) ? StringExtraction.ZipAndBlob(affectedRoad) : null;
                return Content(HttpStatusCode.OK, affectedRoads);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteAssessment/GenerateAffectedRoads Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("RouteAssessment/UpdateAnalysedRoute")]
        public IHttpActionResult UpdateAnalysedRoute(UpdateAnalysedRoutes updateAnalysedRoutes)
        {
            try
            {
                long  result = RouteAssessmentProvider.Instance.UpdateAnalysedRoute(updateAnalysedRoutes.RouteAssess, updateAnalysedRoutes.AnalysisId, updateAnalysedRoutes.UserSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteAssessment/UpdateAnalysedRoute Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        [HttpPost]
        [Route("RouteAssessment/UpdatedNenAssessmentDetails")]
        public IHttpActionResult UpdatedNenAssessmentDetails(NenRouteAssessmentParams nenRouteAssessment)
        {
            try
            {
                int result = RouteAssessmentProvider.Instance.UpdatedNenAssessmentDetails(nenRouteAssessment.NotificationId, nenRouteAssessment.InboxItemId, nenRouteAssessment.AnalysisType, nenRouteAssessment.OrganisationId, nenRouteAssessment.UserSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"RouteAssessment/UpdatedNenAssessmentDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("RouteAssessment/GeneratePrintablePDF")]
        public IHttpActionResult GeneratePrintablePDF(string outputString)
        {
            try
            {
                byte[] assessmentNote = RouteAssessmentProvider.Instance.GeneratePrintablePDF(outputString);
                return Content(HttpStatusCode.OK, assessmentNote);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GeneratePrintablePDF Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("RouteAssessment/GenerateInstrPDF")]
        public IHttpActionResult GenerateInstrPDF(string outputString)
        {
            try
            {
                byte[] assessmentNote = RouteAssessmentProvider.Instance.GenerateInstrPDF(outputString);
                return Content(HttpStatusCode.OK, assessmentNote);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GenerateInstrPDF Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("RouteAssessment/GetDispensationCount")]
        public IHttpActionResult GetDispensationCount(int grantee = 0, int grantor = 0)
        {
            try
            {
                int count = 0;
                List<long> dispensationList = RouteAssessmentProvider.Instance.GetDispensationCount(grantee, grantor);
                count = dispensationList.Count;
                return Content(HttpStatusCode.OK, count);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteAssessment/GetDispensationStatusType Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

    }
}
