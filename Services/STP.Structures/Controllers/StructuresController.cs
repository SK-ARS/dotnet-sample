using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain;
using STP.Domain.RoadNetwork.RoadOwnership;
using STP.Structures.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using STP.Domain.Structures;
using Newtonsoft.Json;

namespace STP.Structures.Controllers
{
    public class StructuresController : ApiController
    {
        [HttpGet]
        [Route("Structures/GetDelegationArrangementList")]
        public IHttpActionResult GetDelegationArrangementList(int organizationId, string delegationArrangement)
        {
            try
            {
                List<DropDown> dropDowns = GetStructureDetailsModel.Instance.GetDelegationArrangement(organizationId, delegationArrangement);
                return Ok(dropDowns);                
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Structures/GetDelegationArrangementList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Structures/CheckStructureAgainstOrganisation")]
        public IHttpActionResult CheckStructureAgainstOrganisation(long structureId, long organisationId)
        {
            try
            {
                int result = GetStructureDetailsModel.Instance.CheckStructAgainstOrg(structureId, organisationId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/CheckStructureAgainstOrganisation, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Structures/CheckStructureOrganisation")]
        public IHttpActionResult CheckStructureOrganisation(int organisationId, long structureId)
        {
            try
            {
                int orgCheck = GetStructureDetailsModel.Instance.CheckStructOrg(organisationId, structureId); // to check the sturcutre belongs to same organisation id 8389 related fix
                return Content(HttpStatusCode.OK, orgCheck);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/CheckStructureOrganisation, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Structures/ViewDimensionConstruction")]
        public IHttpActionResult ViewDimensionConstruction(int structureId, int sectionId)
        {          
            try
            {
                DimensionConstruction objDimentionConstruction = null;
                objDimentionConstruction = GetStructureDetailsModel.Instance.ViewDimensionConstruction(structureId, sectionId);
                return Ok(objDimentionConstruction);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/ViewDimensionConstruction, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Structures/ViewSpanData")]
        public IHttpActionResult ViewSpanData(int structureId, int sectionId)
        {
            try
            {

                List<SpanData> objListSpanData = GetStructureDetailsModel.Instance.ViewSpanData(structureId, sectionId);
                return Ok(objListSpanData);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/ViewSpanData, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
           
            
        }
        [HttpPost]
        [Route("Structures/GetStructureListSearch")]
        public IHttpActionResult GetStructureListSearch(StructureListParams structureListParam)
        {
            try
            {
                List<StructureSummary> summaryObjList = ListStructureSummary.Instance.GetStructureListSearch(structureListParam.OrganisationId, structureListParam.PageNumber, structureListParam.PageSize, structureListParam.ObjSearchStructure);
                return Ok(summaryObjList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Structures/GetStructureListSearch,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }            
        }
        [HttpGet]
        [Route("Structures/ViewGeneralDetails")]
        public IHttpActionResult ViewGeneralDetails(long structureId)
        {
            try
            {
                List<StructureGeneralDetails> objStructureGeneralDetails = GetStructureDetailsModel.Instance.ViewGeneralDetails(structureId);
                return Content(HttpStatusCode.OK, objStructureGeneralDetails);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/ViewGeneralDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Structures/GetStructureCategory")]
        public IHttpActionResult GetStructureCategory(int type)
        {
            List<StructCategory> dropdownObjList;
            try
            {
                dropdownObjList = GetStructureDetailsModel.Instance.getStructCategory(type);
                return Content(HttpStatusCode.OK, dropdownObjList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetStructureCategory, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Structures/GetStructureType")]
        public IHttpActionResult GetStructureType(int type)
        {
            List<StructType> dropdownObjList;
            try
            {
                dropdownObjList = GetStructureDetailsModel.Instance.getStructType(type);
                return Content(HttpStatusCode.OK, dropdownObjList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetStructureType, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("Structures/EditStructureGeneralDetails")]
        public IHttpActionResult EditStructureGeneralDetails(StructureGeneralDetails structureGeneralDetails)
        {
            try
            {
                bool saveFlag = EditGeneralDetails.Instance.EditStructureGeneralDetails(structureGeneralDetails);
                return Content(HttpStatusCode.OK, saveFlag);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/EditStructureGeneralDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Structures/ViewImposedConstruction")]
        public IHttpActionResult ViewImposedConstruction(int structureId, int sectionId)
        {
            try
            {
                ImposedConstraints objImpoConstr = GetStructureDetailsModel.Instance.ViewimposedConstruction(structureId, sectionId);
                return Content(HttpStatusCode.OK, objImpoConstr);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/ViewimposedConstruction, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Structures/ViewSpanDataByNo")]
        public IHttpActionResult ViewSpanDataByNo(long structureId, long sectionId, long? spanNo)
        {
            try
            {
                SpanData objListSpanData = GetStructureDetailsModel.Instance.ViewSpanDataByNo(structureId, sectionId, spanNo); 
                return Content(HttpStatusCode.OK, objListSpanData);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/ViewSpanDataByNo, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("Structures/SaveStructureSpan")]
        public IHttpActionResult SaveStructureSpan(StructureSpanParams SpanParams)
        {
            try
            {
                int saveFlag = StructureDimensinsProvider.SaveStructureSpan(SpanParams.ObjSpanData, SpanParams.StructureId, SpanParams.SectionId, SpanParams.EditSaveFlag);
                return Content(HttpStatusCode.OK, saveFlag);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/SaveStructureSpan, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Structures/GetSTRUCT_DD")]
        public IHttpActionResult GetSTRUCT_DD(int type)
        {
            try
            {
                List<StucDDList> objGetStruct = StructureDimensinsProvider.GetSTRUCT_DD(type);
                return Content(HttpStatusCode.OK, objGetStruct);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetSTRUCT_DD, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("Structures/GetEditStructureImposed")]
        public IHttpActionResult GetEditStructureImposed(StuctImposedParams stuctImposedParams)
        {
            try
            {
                bool flag = StructureDimensinsProvider.GetEditStructureImposed(stuctImposedParams.StructImposConstraints, stuctImposedParams.StructId, stuctImposedParams.SectionId, stuctImposedParams.StructType);
                return Content(HttpStatusCode.Created, flag);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetEditStructImposed, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Structures/GetSVData")]
        public IHttpActionResult GetSVData(long structureId, long sectionId)
        {
            try
            {
                List<SVDataList> objSVDataList = EditGeneralDetails.Instance.GetSVData(structureId, sectionId);
                return Content(HttpStatusCode.OK, objSVDataList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetSVData, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("Structures/UpdateSVData")]
        public IHttpActionResult UpdateSVData(UpdateSVParams objUpdateSVParams)
        {
            List<SVDataList> objSVDataList;
            try
            {
                objSVDataList = EditGeneralDetails.Instance.UpdateSVData(objUpdateSVParams);
                return Content(HttpStatusCode.OK, objSVDataList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/UpdateSVData, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Structures/GetHBRatings")]
        public IHttpActionResult GetHBRatings(long structureId, long sectionId)
        {
            List<double?> getHBresult;
            try
            {
                getHBresult = UpdateConversionFactor.Instance.GetHBRatings(structureId, sectionId);
                return Content(HttpStatusCode.OK, getHBresult);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetHBRatings, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Structures/GetCalculatedHBToSV")]
        public IHttpActionResult GetCalculatedHBToSV(long structureId, long sectionId, double? hbWithLoad, double? hbWithoutLoad, int saveFlag, string userName)
        {
            try
            {
                List<SvReserveFactors> objSvReserveFt = UpdateConversionFactor.Instance.GetCalculatedHBToSV(structureId, sectionId, hbWithLoad, hbWithoutLoad, saveFlag, userName);
                return Content(HttpStatusCode.OK, objSvReserveFt);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetCalculatedHBToSV, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("Structures/GetCautionList")]
        public IHttpActionResult GetCautionList(CautionListParams cautionListParams)
        {    
            try
            {
                List<StructureModel> structureList = StructureProvider.Instance.GetCautionList(cautionListParams.PageNumber, cautionListParams.PageSize, cautionListParams.StructureId, cautionListParams.SectionId);
                return Content(HttpStatusCode.OK, structureList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetCautionList, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Structures/GetCautionDetails")]
        public IHttpActionResult GetCautionDetails(long cautionId)
        {
            try
            {
                StructureModel StructuremodelBase = StructureProvider.Instance.GetCautionDetails(cautionId);
                return Content(HttpStatusCode.OK, StructuremodelBase);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetCautionDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Structures/GetRoadDelegationList")]
        public IHttpActionResult GetRoadDelegationList(int pageNumber, int pageSize, long organizationId)
        {
            try
            {
                List<RoadDelegationList> objRoadDelegationLst = StructureProvider.Instance.GetRoadDelegationList(pageNumber, pageSize, organizationId);
                    return Ok(objRoadDelegationLst);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetRoadDelegationList, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
                    }
        [HttpPost]
        [Route("Structures/SaveCautions")]
        public IHttpActionResult SaveCautions(StructureModel structuremodelalter)
          {
            try
            {
                bool result = StructureProvider.Instance.SaveCautions(structuremodelalter);
                return Content(HttpStatusCode.OK,result);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/SaveCautions, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpDelete] 
        [Route("Structures/DeleteCaution")]
        public IHttpActionResult  DeleteCaution(long cautionId, string userName)
        {
            try
            {
               int deleteFlag = StructureProvider.Instance.DeleteCaution(cautionId,userName);
                if (deleteFlag < 0)
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
                }

                else if (deleteFlag == 0)
                {
                    return Content(HttpStatusCode.OK, deleteFlag);

                }
                else
                {
                    return Content(HttpStatusCode.OK, deleteFlag);
                }
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/DeleteCaution, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("Structures/DeleteStructureSpan")]
        public IHttpActionResult DeleteStructureSpan(long structureId, long sectionId, long spanNo, string userName)
        {
            try
            {
                int deleteFlag = StructureDimensinsProvider.DeleteStructureSpan(structureId, sectionId, spanNo, userName);
                if (deleteFlag < 0)
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
                }

                else if (deleteFlag == 0)
                {
                    return Content(HttpStatusCode.OK,deleteFlag);

                }
                else
                {
                    return Content(HttpStatusCode.OK, deleteFlag);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/DeleteStructureSpan, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("Structures/UpdateStructureLog")]
        public IHttpActionResult UpdateStructureLog(List<StructureLogModel> structureLogsModel)
        {
            try
            {
                bool result = StructureProvider.Instance.UpdateStructureLog(structureLogsModel);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/UpdateStructureLog, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Structures/GetStructureHistory")]
        public IHttpActionResult GetStructureHistory(int pageNumber, int pageSize, long structureID)
        {
            try
            {
                List<StructureModel> lstStructureModel = StructureProvider.Instance.GetStructureHistory(pageNumber, pageSize, structureID);
                return Content(HttpStatusCode.OK, lstStructureModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetStructureHistory, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("Structures/GetStructureHistoryById")]
        public IHttpActionResult GetStructureHistoryById(int pageNumber, int pageSize, long structureID)
        {
            try
            {
                List<StructureHistoryList> lstStructureModel = StructureProvider.Instance.GetStructHistoryById(pageNumber, pageSize, structureID);
                return Content(HttpStatusCode.OK, lstStructureModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetStructureHistoryById, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }


        [HttpGet]
        [Route("Structures/GetStructureHistoryCount")]
        public IHttpActionResult GetStructureHistoryCount(long structureID)
        {
            try
            {
                int HistoryCount = StructureProvider.Instance.GetStructHistoryCount(structureID);
                return Content(HttpStatusCode.OK, HistoryCount);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetStructureHistoryCount, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }


        [HttpGet]
        [Route("Structures/StructureInfoList")]
        public IHttpActionResult StructureInfoList(int organisationId, int otherOrganisation, int left, int right, int bottom, int top)
        {
            List<StructureInfo> structureInfoList;
            try
            {
                structureInfoList = StructureManagerProvider.Instance.MyStructureInfoList(organisationId, otherOrganisation, left, right, bottom, top);
                return Ok(structureInfoList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/StructureInfoList, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        /// <summary>
        /// Review contacts list
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="StructureID">Structure id</param>
        /// <param name="contactNo">Contact no</param>
        /// <returns>Get contact list</returns>
        [HttpGet]
        [Route("Structures/GetStructureContactList")]
        public IHttpActionResult GetStructureContactList(int pageNumber, int pageSize, long cautionId, short contactNo = 0)
        {
            try
            {
                List<StructureContactModel> lstStructureModel = StructureProvider.Instance.GetStructureContactList(pageNumber, pageSize, cautionId, contactNo);
                return Ok(lstStructureModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetStructureContactList, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Structures/GetStructureContactList")]
        public IHttpActionResult GetStructureContactList(long structureId, string userSchema = UserSchema.Portal)
        {
            try
            {
                List<StructureContact> StructureContactList = StructureManagerProvider.Instance.GetStructureContactListInfo(structureId, userSchema);
                return Ok(StructureContactList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetStructureContactList, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        /// <summary>
        ///ViewStructureSections
        /// </summary>
        /// <param name="structureId">Page</param>
        /// <returns>Get structure section  list</returns>
        [HttpGet]
        [Route("Structures/ViewStructureSections")]
        public IHttpActionResult ViewStructureSections(long structureId)
        {
            try
            {
                List<StructureSectionList> lstStructureModel = GetStructureDetailsModel.Instance.ViewStructureSections(structureId);
                return Ok(lstStructureModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/ViewStructureSections, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        /// <summary>
        ///ViewEnabledICA
        /// </summary>
        /// <param name="structureId">Structure Id</param>
        /// <param name="sectionId">Section Id</param>
        /// <param name="organisationID">Organisation Id</param>
        /// <returns>Get structure section  list</returns>
        [HttpGet]
        [Route("Structures/ViewEnabledICA")]
        public IHttpActionResult ViewEnabledICA(int structureId, int sectionId, long organisationId)
        {
            try
            {
                ManageStructureICA manageStructure = GetStructureDetailsModel.Instance.ViewEnabledICA(structureId, sectionId, organisationId);
                return Ok(manageStructure);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/ViewEnabledICA, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Structures/ViewSVData")]
        public IHttpActionResult ViewSVData(int structureId, int sectionId)
        {
            try
            {
                List<SvReserveFactors> lstStructureModel = GetStructureDetailsModel.Instance.viewSVData(structureId, sectionId);
                return Content(HttpStatusCode.OK, lstStructureModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/viewSVData, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        /// <summary>
        /// Save Structures
        /// </summary>
        /// <param name="SaveStructureContact">SaveStructureContact model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Structures/SaveStructureContact")]
        public IHttpActionResult SaveStructureContact(StructureContactModel structureContact)
        {
            try
            {
                bool result = StructureProvider.Instance.SaveStructureContact(structureContact);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/SaveStructureContact, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpDelete]
        [Route("Structures/DeleteStructureContact")]
        public IHttpActionResult   StructureContact(short contactNo, long cautionId)
        {
            try
            {
                int deleteFlag = StructureProvider.Instance.DeleteStructureContact(contactNo, cautionId);

                if (deleteFlag < 0)
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
                }

                else if (deleteFlag == 0)
                {
                    return Content(HttpStatusCode.OK, deleteFlag);

                }
                else
                {
                    return Content(HttpStatusCode.OK, deleteFlag);
                }
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/DeleteStructureContact, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }        
        [HttpGet]
        [Route("Structures/GetRoadContactList")]
        public IHttpActionResult GetRoadContactList(long linkId, long length, string userSchema = UserSchema.Portal)
        {
            List<RoadContactModal> roadContactList;
            try
            {
                roadContactList = StructureManagerProvider.Instance.GetRoadContactList(linkId, length, userSchema);
                return Ok(roadContactList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetRoadContactList, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Structures/AgreedAppStructureInfo")]
        public IHttpActionResult AgreedAppStructureInfo(string StructureCode)
        {
            try
            {
                List<StructureInfo> result = StructureManagerProvider.Instance.AgreedAppStructureInfo(StructureCode);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/AgreedAppStructureInfo, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Structures/GetAllStructureNotification")]
        public IHttpActionResult GetAllStructureNotification(string structureId, int pageNum, int pageSize)
        {
            try
            {
                List<StructureNotification> result = StructureProvider.Instance.GetAllStructureNotification(structureId, pageNum, pageSize);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetAllStructureNotification, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("Structures/GetManageICAUsage")]
        public IHttpActionResult GetManageICAUsage(int organisationId, int structureId, int sectionId)
        {
            try
            {
                ManageStructureICA result = ManageICAVehicle.Instance.GetManageICAUsage(organisationId, structureId, sectionId); 
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetManageICAUsage, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("Structures/GetICAVehicleResult")]
        public IHttpActionResult GetICAVehicleResult(ICAVehicleResult obj)
        {
            try
            {
                string result = ManageICAVehicle.Instance.GetICAVehicleResult(obj.ObjICAVehicleModel, obj.ObjManageStructureICA, obj.MovementClassConfig, obj.ConfigType, obj.CompNum, obj.TractorType, obj.TrailerType,obj.StructureId,obj.SectionId,obj.OrgId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetICAVehicleResult, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("Structures/UpdateStructureICAUsage")]
        public IHttpActionResult UpdateStructureICAUsage(UpdateICAUsageParams ICAUsageparams)
        {
            try
            {
                int result = ManageICAVehicle.Instance.UpdateStructureICAUsage(ICAUsageparams.ICAUsage, ICAUsageparams.OrganisationId, ICAUsageparams.StructureId, ICAUsageparams.SectionId);
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
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/UpdateStructureICAUsage, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("Structures/UpdateDefaultBanding")]
        public IHttpActionResult UpdateDefaultBanding(SaveDefaultConfigParams configparams)
        {
            try
            {
                bool result = EditGeneralDetails.Instance.UpdateDefaultBanding(configparams.OrgId, configparams.OrgMinWeight, configparams.OrgMaxWeight, configparams.OrgMinSV, configparams.OrgMaxSV, configparams.UserName);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/UpdateDefaultBanding, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("Structures/GetDefaultBanding")]
        public IHttpActionResult GetDefaultBanding(int organisationId,int structureId, int sectionId)
        {
            try
            {
                ConfigBandModel objConfigBandModel = EditGeneralDetails.Instance.GetDefaultBanding(organisationId, structureId, sectionId);
                return Ok(objConfigBandModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetDefaultBanding, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("Structures/EditDimensionConstraints")]
        public IHttpActionResult EditDimensionConstraints(EditDimensionParams editDimensionParams)
        {
            try
            {
                bool result = StructureDimensinsProvider.EditDimensionConstraints(editDimensionParams.StructureDimension,editDimensionParams.StructureId,editDimensionParams.SectionId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/SaveCautions, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("Structures/GetAllStructureOnerousVehicles")]
        public IHttpActionResult GetAllStructureOnerousVehicles(OnerousVehicleListParams onerousVehicleListParams)
        {
            try
            {
                List<StructureNotification> result = StructureProvider.Instance.GetAllStructureOnerousVehicles(onerousVehicleListParams.StructureId, onerousVehicleListParams.PageNumber, onerousVehicleListParams.PageSize, onerousVehicleListParams.SearchCriteria, onerousVehicleListParams.SearchStatus, onerousVehicleListParams.StartDate, onerousVehicleListParams.EndDate, onerousVehicleListParams.StatusCount, onerousVehicleListParams.Sort, onerousVehicleListParams.OrganisationId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetAllStructureOnerousVehicles, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Structures/GetStructureOwner")]
        public IHttpActionResult GetStructureOwner(long structId, long orgId)
        {
            try
            {
                int result = StructureManagerProvider.Instance.GetStructureOwner(structId, orgId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetStructureOwner, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }


        [HttpGet]
        [Route("Structures/GetStructureId")]
        public IHttpActionResult GetStructureId(string structurecode)
        {
            try
            {
                int result = StructureManagerProvider.Instance.GetStructureId(structurecode);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Structures/GetStructureId, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
    }
}
