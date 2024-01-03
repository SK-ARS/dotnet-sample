
using STP.Domain;
using STP.Structures.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using STP.Common.Constants;
using STP.Common.Logger;
using Newtonsoft.Json;
using STP.Domain.Structures;
namespace STP.Structures.Controllers
{
    public class StructureDeligationController : ApiController
    {
        /// <summary>
        /// Returns deletegation arrangment list
        /// </summary>
        /// <param name="delegationList">StructureDelegationListParams modal class is passed as serialized json string</param>       
        /// <returns></returns>
        [HttpGet]
        [Route("StructureDeligation/GetDelegationArrangements")]
        public IHttpActionResult GetDelegationArrangements(long organisationId, int pageNumber, int pageSize, string searchType, string searchValue, int presetFilter, int? sortOrder)
        {
            try
            {
                
                List<DelegationList> delegArrangList = DelegationArrangementProvider.Instance.GetDelegArrangList(organisationId, pageNumber, pageSize, searchType, searchValue, presetFilter, sortOrder);
                return Ok(delegArrangList);               
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/GetDelegationArrangements,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }           
        }
        /// <summary>
        /// Returns deletegation arrangment 
        /// </summary>
        /// <param name="arrangId">Arrangement Id</param>
        /// <param name="organization">Organization Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("StructureDeligation/GetArrangement")]
        public IHttpActionResult GetArrangement(long arrangementId, int organizationId)
        {
            try
            {
                DelegationList delegationList = DelegationArrangementProvider.Instance.GetArrangement(arrangementId, organizationId);
                return Content(HttpStatusCode.OK, delegationList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/GetArrangement,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }          
        }
        /// <summary>
        /// Returns organisation list
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="organizationName">Organisation name</param>
        /// <returns></returns>
        [HttpGet]
        [Route("StructureDeligation/GetorganisationList")]
        public IHttpActionResult GetorganisationList(int pageNumber, int pageSize, string organizationName)
        {
            try
            {
                List<DelegationList> delegArrangList = DelegationArrangementProvider.Instance.GetOrganisationList(pageNumber, pageSize, organizationName);
                return Content(HttpStatusCode.OK, delegArrangList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/GetOrganisationList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }           
        }
        /// <summary>
        /// Returns deligation list
        /// </summary>
        /// <param name="organizationId">Organization Id</param>
        /// <param name="delegationName">Delegation Name</param>       
        /// <returns></returns>
        [HttpGet]
        [Route("StructureDeligation/GetDelegationAutoFill")]
        public IHttpActionResult GetDelegationAutoFill(int organizationId, string delegationName)
        {
            try
            {
                List<DropDown> delegationList = DelegationArrangementProvider.Instance.GetDelegationAutoFill(organizationId, delegationName);
                return Content(HttpStatusCode.OK, delegationList);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/GetDelegationAutoFill,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }             
        }
        /// <summary>
        /// Returns deletegation arrangment list
        /// </summary>
        /// <param name="organizationId">organizationId Id</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="arrangementName">Arrangement Name </param>        
        /// <returns></returns>
        [HttpGet]
        [Route("StructureDeligation/GetDelegateArrangementList")]
        public IHttpActionResult GetDelegateArrangementList(int organizationId, int pageNumber, int pageSize, string arrangementName)
        {
            try
            {
                List<DelegationList> delegArrangList = DelegationArrangementProvider.Instance.GetDelegateArrangList(organizationId, pageNumber, pageSize, arrangementName);
                return Content(HttpStatusCode.OK, delegArrangList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/GetDelegateArrangList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }          
        }
        /// <summary.
        /// Returns Structures in Delegation list 
        /// </summary>
        /// <param name="arrangementId">Arrangement  Id</param>
        /// <param name="organizationId">Organisation Id</param>
        /// <param name="pageNumber">pageNum</param>  
        /// <param name="pageSize">pageSize</param>         
        /// <returns></returns>
        [HttpGet]
        [Route("StructureDeligation/GetStructuresInDelegation")]
        public IHttpActionResult GetStructuresInDelegation(long arrangementId, long organizationId, int? pageNumber, int? pageSize)
        {
            try
            {
                List<StructureInDelegationList> arrangList = DelegationArrangementProvider.Instance.GetStructuresInDeleg(arrangementId, organizationId, pageNumber, pageSize);
                return Content(HttpStatusCode.OK, arrangList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/GetStructuresInDelegation,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }           
        }
        /// <summary>
        /// Returns contact list
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="contactName">Contactname name</param>
        /// <param name="organizationId">Organisation Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("StructureDeligation/GetContactList")]
        public IHttpActionResult GetContactList(int pageNumber, int pageSize, string contactName, int organizationId)
        {
            try
            {
                List<DelegationList> contactList = DelegationArrangementProvider.Instance.GetContactList(pageNumber, pageSize, contactName, organizationId);
                return Content(HttpStatusCode.OK, contactList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/GetContactList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }                 
        }
        /// <summary>
        /// Returns the List of Road Delegation Arrengement
        /// </summary>
        /// <param name="pageNumber">page number</param>
        /// <param name="pageSize">page size</param>
        /// <param name="OrganisationId">Organization Id</param>
        /// <returns>RoadDelegationList</returns>
        [HttpGet]
        [Route("StructureDeligation/GetRoadDelegationList")]
        public IHttpActionResult GetRoadDelegationList(int pageNumber, int pageSize, long organisationId, int presetFilter, int? sortOrder)
        {
            try
            {
                List<RoadDelegationList> roaddelegationList = DelegationArrangementProvider.Instance.GetRoadDelegationList(pageNumber, pageSize, organisationId,presetFilter,sortOrder);
                return Content(HttpStatusCode.OK, roaddelegationList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/GetRoadDelegationList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }           
        }
        /// <summary>
        /// Returns Structure In Delegation list
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="structureCodes">combine structure code</param>
        /// <param name="OrganisationId">Organization Id</param>
        /// <param name="structurecodeCount">count of structure code </param>
        /// <returns>StructureInDelegationList</returns>        
        [HttpGet]
        [Route("StructureDeligation/GetStructureInDelegationList")]
        public IHttpActionResult GetStructureInDelegationList(int pageNumber, int? pageSize, string structureCodes, int organizationId, int structurecodeCount)
        {
            try
            {
                List<StructureInDelegationList> structuredelegationList = DelegationArrangementProvider.Instance.GetStructureInDelegationList(pageNumber, pageSize, structureCodes, organizationId, structurecodeCount);
               return Content(HttpStatusCode.OK, structuredelegationList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/GetStructureInDelegationList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        /// <summary>
        /// Returns Structure In Delegation list by overriding function to take input of array of structure codes that are delegated/checked by user
        /// </summary>
        /// <param name="structureCodes">structurecodes stored in an ArrayList  and is passed as Json Serialized String</param>
        /// <param name="organizationId">Organisation Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("StructureDeligation/GetStructureInDeleList")]
        public IHttpActionResult GetStructureInDeleList(string structureCodes , int organisationId )
        {          
            try
            {
                string[] structurecodes1 = JsonConvert.DeserializeObject<string []>(structureCodes.ToString());                
                List<StructureInDelegationList> structuredelegationList = DelegationArrangementProvider.Instance.GetStructureInDelegationList(structurecodes1, organisationId);
                return Content(HttpStatusCode.OK, structuredelegationList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/GetStructureInDeleList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        /// <summary>
        /// Save Structures
        /// </summary>
        /// <param name="delegationList">DelegationList model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("StructureDeligation/ManageStructureDelegation")]
        public IHttpActionResult ManageStructureDelegation(DelegationList delegationList)
        {
            try
            {
                bool result = DelegationArrangementProvider.Instance.ManageStructureDelegation(delegationList);
                return Content(HttpStatusCode.Created, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/ManageStructureDelegation,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        /// <summary>
        /// Saves structure contact details while adding delegation arrangements
        /// </summary>
        /// <param name="structureContact">structure contact list</param>
        /// <returns>boolean value indicating the success / failure of structure contact details</returns>
        [HttpPost]
        [Route("StructureDeligation/ManageDelegationStructureContact")]
        public IHttpActionResult ManageDelegationStructureContact(DelegationList structureContact)
        {
            try
            {
                bool result = DelegationArrangementProvider.Instance.ManageDelegationStructureContact(structureContact);
                return Content(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            { 
            Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/ManageDelegationStructureContact,Exception:" + ex);
            return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        /// <summary>
        /// Check sub delegation details before adding delegation arrangements
        /// </summary>
        /// <param name="structureID"></param>
        /// <param name="organisationID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("StructureDeligation/CheckSubDelegationList")]
        public IHttpActionResult CheckSubDelegationList(long structureID, long organisationId)
        {
            try
            {
                int result = DelegationArrangementProvider.Instance.CheckSubDelegationList(structureID, organisationId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/CheckSubDelegationList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        /// <summary>
        /// delete structureedit in delegation arrangement
        /// </summary>        
        /// <param name="arrangId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("StructureDeligation/DeleteStructureEdit")]
        public IHttpActionResult DeleteStructureEdit(long arrangementId)
        {           
            try
            {
                int result = DelegationArrangementProvider.Instance.DeleteStructureEdit(arrangementId);

                if (result < 0)
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
                }
                else if (result == 0)
                {
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {
                    return Content(HttpStatusCode.OK, result);
                }
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/DeleteStructureEdit,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        /// <summary>
        /// Get structure contact list based upon arrangement Id
        /// </summary>
        /// <param name="arrangementId">ArrangementId of the respective structure contact</param>
        /// <returns>List of structure contact</returns>
        [HttpGet]
        [Route("StructureDeligation/GetStructureContactList")]
        public IHttpActionResult GetStructureContactList(long arrangementId)
        {
            try
            {
                List<StructureContactsList> structureContacts = DelegationArrangementProvider.Instance.GetStructureContactList(arrangementId);
                return Content(HttpStatusCode.OK, structureContacts);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/GetStructureContactList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        /// <summary>
        /// delete structure in delegation arrangement
        /// </summary>
        /// <param name="structId"></param>
        /// <param name="arrangId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("StructureDeligation/DeleteStructureInDelegation")]
        public IHttpActionResult DeleteStructureInDelegation(long structId, long arrangementId)
        {          
            try
            {
                int result = DelegationArrangementProvider.Instance.DeleteStructInDelegation(structId, arrangementId);
                if (result < 0)
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
                }
                else if (result == 0)
                {
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {
                    return Content(HttpStatusCode.OK, result);
                }
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/DeleteStructureInDelegation,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        /// <summary>
        /// view  delegation arrangement
        /// </summary>
        /// <param name="organisationId"></param>        
        /// <returns></returns>
        [HttpGet]
        [Route("StructureDeligation/ViewDelegationArrangment")]
        public IHttpActionResult ViewDelegationArrangment(long organisationId)
        {           
            try
            {
                List<DelegationArrangment> delegationArrangments = GetStructureDetailsModel.Instance.viewDelegationArrangment(organisationId);
                return Content(HttpStatusCode.OK, delegationArrangments);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/viewDelegationArrangment,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        /// <summary>
        /// view  viewUnsuitableStructSections
        /// </summary>
        /// <param name="structureId">structure id</param>      
        /// <param name="routePartId"> route part id</param>     
        /// <param name="section_id">section id   
        /// <param name="countReferenceNo">count reference number</param>     
        /// <returns></returns>
        [HttpGet]
        [Route("StructureDeligation/ViewUnsuitableStructSections")]
        public IHttpActionResult ViewUnsuitableStructSections(long structureId, long routePartId, long sectionId, string countReferenceNo)
        {
           try
            {
                List<StructureSectionList> structureSectionLists = GetStructureDetailsModel.Instance.viewUnsuitableStructSections(structureId, routePartId, sectionId, countReferenceNo);
                return Content(HttpStatusCode.OK, structureSectionLists);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/ViewUnsuitableStructSections,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        /// <summary>
        /// Returns structure id
        /// </summary>
        /// <param name="structureCode"></param>      
        /// <returns></returns>
        [HttpGet]
        [Route("StructureDeligation/GetStructureId")]       
        public IHttpActionResult GetStructureId(string structureCode)
        {
            try
            {
                long result = GetStructureDetailsModel.Instance.GetStructureId(structureCode);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/GetStructureId,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        /// <summary>
        /// Save delegation arrangement
        /// </summary>
        /// <param name="savedelegation">DelegationList model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("StructureDeligation/ManageDelegationArrangement")]
        public IHttpActionResult ManageDelegationArrangement(DelegationList saveDelegation, long organisationId)  
        {
            try
            {
                DelegationList delegationLists = DelegationArrangementProvider.Instance.ManageDelegationArrangement(saveDelegation, organisationId);
                return Content(HttpStatusCode.OK, delegationLists);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/ManageDelegationArrangement,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        /// <summary>
        /// Delete structure contact
        /// </summary>
        /// <param name="contactNO">Contact no</param>
        /// <param name="cautionId">Structure id</param>
        /// <returns>Delete structure contact </returns>
        [HttpDelete]
        [Route("StructureDeligation/DeleteStructureContact")]
        public IHttpActionResult DeleteStructureContact(short contactNO, long cautionId)//StructureProvider
        { 
            try
            {
                int  result = StructureProvider.Instance.DeleteStructureContact(contactNO, cautionId);
                if (result < 0)
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
                }
                else if (result == 0)
                {
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {
                    return Content(HttpStatusCode.OK, result);
                }
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/DeleteStructureContact,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        /// <summary>
        /// Delete structure contact
        /// </summary>

        /// <param name="structContactList">Parameters are stored in StructureContactsList model class and is passed in the form of a  json string</param>
        /// <returns>Delete structure contact </returns>
        [HttpDelete]
        [Route("StructureDeligation/DeleteStructureContact")]//DeligateArrangementProvider
        public IHttpActionResult DeleteStructureContact(long structureId, string structureCode, long arrangementId, long ownerID)//DeligateArrangementProvider

        {
            try
            {
                int result = DelegationArrangementProvider.Instance.DeleteStructureContact(structureId, structureCode, arrangementId, ownerID);

                if (result < 0)
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
                }
                else if (result == 0)
                {
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {
                    return Content(HttpStatusCode.OK, result);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/DeleteStructureContact,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        /// <summary>
        /// delete delegation arrangement
        /// </summary>
        /// <param name="arrangementId">arrangement Id</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("StructureDeligation/DeleteDelegationArrangement")]
        public IHttpActionResult DeleteDelegationArrangement(long arrangementId)
        {           
            try
            {
                int result = DelegationArrangementProvider.Instance.DeleteDelegationArrangement(arrangementId);
                if (result < 0)
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
                }
                else if (result == 0)
                {                    
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {                   
                    return Content(HttpStatusCode.OK, result);
                }                
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/DeleteDelegationArrangement,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        /// <summary>
        /// Returns undeligated structure list
        /// </summary>
        /// <param name="delegationList">StructureDelegationListParams modal class is passed as serialized json string</param>       
        /// <returns></returns>
        [HttpPost]
        [Route("StructureDeligation/GetNotDelegatedStructureListSearch")]
        public IHttpActionResult GetNotDelegatedStructureListSearch(StructureListParams structureListParam)
        {
            try
            {

                List<StructureSummary> delegList = DelegationArrangementProvider.Instance.GetNotDelegatedStructureListSearch(structureListParam.OrganisationId, structureListParam.PageNumber, structureListParam.PageSize, structureListParam.ObjSearchStructure,structureListParam.sortOrder,structureListParam.presetFilter);
                return Ok(delegList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/GetNotDelegatedStructureListSearch,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }


        [HttpGet]
        [Route("StructureDeligation/GetStructureDeleArrg")]
        public IHttpActionResult  GetStructureDeleArrg(string structureCode)
        {
            try
            {
                List<StructureDeleArrList> result = DelegationArrangementProvider.Instance.GetStructureDeleArrg(structureCode);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDeligation/GetStructureDeleArrg,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
    }
}
