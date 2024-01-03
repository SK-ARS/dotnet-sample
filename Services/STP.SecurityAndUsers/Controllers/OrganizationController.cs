using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain.SecurityAndUsers;
using STP.SecurityAndUsers.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace STP.SecurityAndUsers.Controllers
{
    public class OrganizationController : ApiController
    {
        #region ViewOrganizationByID
        [HttpGet]
        [Route("Organization/ViewOrganizationByID")]
        public IHttpActionResult ViewOrganizationByID(int orgId)
        {     
            try
            {
                List<ViewOrganizationByID> result = UserProvider.Instance.ViewOrganizationByID(orgId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Organisation/ViewOrganizationByID,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion            
        #region GetOrganizationInformationByCriteria
        [HttpGet]
        [Route("Organization/GetOrganizationInformation")]
        public IHttpActionResult GetOrganizationInformation(string searchString, int pageNumber, int pageSize, int userTypeId,string searchOrgCode, int sortOrder, int presetFilter)
        {
            try
            {
                List<OrganizationGridList> result = OrganizationProvider.Instance.GetOrganizationInformation(searchString, pageNumber, pageSize, userTypeId, searchOrgCode, sortOrder, presetFilter);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Organisation/GetOrganizationInformationByCriteria,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetOrganizationInformation
        [HttpGet]
        [Route("Organization/GetOrganizationInformation")]
        public IHttpActionResult GetOrganizationInformation(int pageNumber, int pageSize,int userTypeId, int sortOrder, int presetFilter)
        {
            try
            {
                List<OrganizationGridList> result = OrganizationProvider.Instance.GetOrganizationInformation(pageNumber, pageSize, userTypeId, sortOrder, presetFilter);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Organisation/GetOrganizationInformation,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Save Organization
        [HttpPost]
        [Route("Organization/SaveOrganization")]
        public IHttpActionResult SaveOrganization(Organization orgDetails)
        {
            try
            {
                int  output = OrganizationProvider.Instance.SaveOrganization(orgDetails);
                if (output > 0)
                {
                    return Content(HttpStatusCode.Created, output);
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.InsertionFailed);
                }        
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Organization/SaveOrganization,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Edit Organization
        [HttpPost]
        [Route("Organization/EditOrganization")]
        public IHttpActionResult EditOrganization(Organization orgDetails)
        {
            try
            {
                int output = OrganizationProvider.Instance.EditOrganization(orgDetails);
                if (output > 0)
                {
                    return Content(HttpStatusCode.OK, output);
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.UpdationFailed);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Organization/EditOrganization,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetOrganizationTypeList
        [HttpGet]
        [Route("Organization/GetOrganizationTypeList")]
        public IHttpActionResult GetOrganizationTypeList()
        {
            try
            {
                List<OrganizationTypeList> organizationTypes = OrganizationProvider.Instance.GetOrganizationTypeList();
                return Content(HttpStatusCode.OK, organizationTypes);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Organization/GetOrganisationTypeList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        [HttpPost]
        [Route("Organization/GetOrganizationByName")]
        public IHttpActionResult GetOrganizationByName(CheckOrganisationExists checkOrganisationExists)
        {
            try
            {
                decimal output = OrganizationProvider.Instance.GetOrganizationByName(checkOrganisationExists.OrganisationName, checkOrganisationExists.Type, checkOrganisationExists.Mode, checkOrganisationExists.OrganisationId);
                return Content(HttpStatusCode.OK, output);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Organization/GetOrganizationByName,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("Organization/ViewOrganisationByIDForSORT")]
        public IHttpActionResult ViewOrganisationByIDForSORT(int RevisionId)
        {
            try
            {
                List<ViewOrganizationByID> result = OrganizationProvider.Instance.ViewOrganisationByIDForSORT(RevisionId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Organisation/ViewOrganisationByIDForSORT,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        #region NEN API GET INPUT AFFECTED PARTIES DETAILS 
        [HttpGet]
        [Route("Organization/GetAffectedOrganisationDetails")]
        public IHttpActionResult GetAffectedOrganisationDetails(string affectedParties, int affectedPartiesCount, string userSchema)
        {
            try
            {
                List<ContactModel> contacts = OrganizationProvider.Instance.GetAffectedOrganisationDetails(affectedParties, affectedPartiesCount, userSchema);
                return Content(HttpStatusCode.OK, contacts);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Organisation/GetAffectedOrganisationDetails,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region NEN PDF GET INPUT AFFECTED PARTIES DETAILS 
        [HttpGet]
        [Route("Organization/GetNenAffectedOrganisationDetails")]
        public IHttpActionResult GetNenAffectedOrganisationDetails(int inboxItemId)
        {
            try
            {
                List<ContactModel> contacts = OrganizationProvider.Instance.GetNenAffectedOrganisationDetails(inboxItemId);
                return Content(HttpStatusCode.OK, contacts);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Organisation/GetNenAffectedOrganisationDetails,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
    }
}
