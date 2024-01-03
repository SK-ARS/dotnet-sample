using Newtonsoft.Json;
using STP.Common.Constants;
using STP.Common.Logger;
using STP.DataAccess.Provider;
using STP.Domain;
using STP.Domain.Communications;
using STP.Domain.SecurityAndUsers;
using STP.SecurityAndUsers.Providers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Http;
namespace STP.SecurityAndUsers.Controllers
{
    public class UserController : ApiController
    {
        #region GetUserByID
        [HttpGet]
        [Route("User/GetUserByID")]
        public IHttpActionResult GetUserByID(string userTypeId, int userId, int contactId)
        {
            try
            {
                List<UserRegistration> result = UserProvider.Instance.GetUserByID(userTypeId, userId, contactId);
                return Content(HttpStatusCode.OK,result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/GetUserByID,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetUserByName
        [HttpGet]
        [Route("User/GetUserByName")]
        public IHttpActionResult GetUserByName(string userName, int type, string mode, string userId)
        {
            try
            {
                decimal result = UserProvider.Instance.GetUserByName(userName, type, mode, userId);
                return Content(HttpStatusCode.OK,result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/GetUserByName,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region UpdateUser
        [HttpPost]
        [Route("User/UpdateUser")]
        public IHttpActionResult UpdateUser(UserParams userparams)
        {
            try
            {
                int result = 0;
                result = UserProvider.Instance.UpdateUser(userparams.RegDet, userparams.UserTypeId, userparams.UserId.Value);
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
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/UpdateUser,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region SetRegInfo
        [HttpPost]
        [Route("User/SetRegInfo")]
        public IHttpActionResult SetRegInfo(UserParams userparams)
        {
            try
            {
                UserRegistration info = UserProvider.Instance.SetRegInfo(userparams.RegDet, userparams.UserTypeId);
                return Content(HttpStatusCode.OK,info);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/SetRegInfo,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region DeleteUser
        [HttpDelete]
        [Route("User/DeleteUser")]
        public IHttpActionResult DeleteUser(int UserId, int deleteVal)
        {
            try
            {
                int affectedRows = UserProvider.Instance.DeleteUser(UserId, deleteVal);
                if (affectedRows > -1)
                    return Content(HttpStatusCode.OK, affectedRows);
                else
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/DeleteUser,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region DeleteContact
        [HttpDelete]
        [Route("User/DeleteContact")]
        public IHttpActionResult DeleteContact(int contactId, int deleteVal)
        {
            try
            {
                int affectedRows = UserProvider.Instance.DeleteContact(contactId, deleteVal);
                if (affectedRows > -1)
                    return Content(HttpStatusCode.OK, affectedRows);
                else
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/DeleteContact,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region UserList
        [HttpPost]
        [Route("User/UserList")]
        public IHttpActionResult UserList(UserContactListParams userContactList)
        {
            try
            {
                List<GetUserList> result = UserProvider.Instance.UserList(userContactList.UserTypeId, userContactList.OrganisationId, userContactList.PageNumber, userContactList.PageSize, userContactList.UserContactSearchItems,userContactList.SortOrder,userContactList.PresetFilter);
                return Content(HttpStatusCode.OK,result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/UserList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region CheckToDisableUser
        [HttpGet]
        [Route("User/CheckToDisableUser")]
        public IHttpActionResult CheckToDisableUser(int userId)
        {
            try
            {
               bool result = UserProvider.Instance.CheckToDisableUser(userId);
                return Content(HttpStatusCode.OK,result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/CheckToDisableUser,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region SetContactRegInfo
        [HttpPost]
        [Route("User/SetContactRegInfo")]
        public IHttpActionResult SetContactRegInfo(UserParams userParams)
        {
            try
            {
                UserRegistration userRegistration = UserProvider.Instance.SetContactRegInfo(userParams.RegDet, userParams.UserTypeId);
                return Content(HttpStatusCode.OK,userRegistration);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/SetContactRegInfo,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region UpdateContact
        [HttpPost]
        [Route("User/UpdateContact")]
        public IHttpActionResult UpdateContact(UserParams userParams)
        {
            try
            {
                int result = UserProvider.Instance.UpdateContact(userParams.RegDet, userParams.UserTypeId, userParams.ContactId.Value);
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
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/UpdateContact,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetSearchUser
        [HttpGet]
        [Route("User/GetSearchUser")]
        public IHttpActionResult GetSearchUser(string value, string Type)
        {
            try
            {
                bool result = UserProvider.Instance.GetSearchUser(value, Type);
                return Content(HttpStatusCode.OK,result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/GetSearchUser,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        
        #region GetUserType
        [HttpGet]
        [Route("User/GetUserType")]
        public IHttpActionResult GetUserType()
        {
            try
            {
                List<UserRegistration> objUserType = UserProvider.Instance.GetUserType();
                return Content(HttpStatusCode.OK,objUserType);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/GetUserType,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetCountryInfo
        [HttpGet]
        [Route("User/GetCountryInfo")]
        public IHttpActionResult GetCountryInfo()
        {
            try
            {
                List<UserRegistration> objCountryInfo = UserProvider.Instance.GetCountryInfo();
                return Content(HttpStatusCode.OK,objCountryInfo);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/GetCountryInfo,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region SetUserPreference
        [HttpPost]
        [Route("User/SetUserPreference")]
        public IHttpActionResult SetUserPreference(UserPreferenceParams userPreferenceParams)
        {
            try
            {
                bool result = UserProvider.Instance.SetUserPreference(userPreferenceParams.ObjUserPreference, userPreferenceParams.UserId, userPreferenceParams.EmailUpdate, userPreferenceParams.FaxNumber);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/SetUserPreference,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        [HttpGet]
        [Route("User/GetOrganisationTypeList")]
        public IHttpActionResult GetOrganisationTypeList()
        {
            try
            {
                List<OrganizationTypeList> result = UserProvider.Instance.GetOrganisationTypeList();
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/UpdateContact,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("User/GetHaulierContactByOrgID")]
        public IHttpActionResult GetHaulierContactByOrgID(int organisationId)
        {
            try
            {
                List<GetHaulContactByOrgID> result = UserProvider.Instance.GetHaulierContactByOrgID(organisationId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/GetHaulierContactByOrgID,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("User/GetUsersByOrgID")]
        public IHttpActionResult GetUsersByOrgID(int organisationId,int UserTypeId)
        {
            try
            {
                List<UserDetailsModel> result = UserProvider.Instance.GetUsersByOrgID(organisationId, UserTypeId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/GetUserByOrgID,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("User/GetUserPreferencesById")]
        public IHttpActionResult GetUserPreferencesById(int userId)
        {
            try
            {
                UserPreferences result = UserProvider.Instance.GetUserPreferencesById(userId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/GetUserPreferencesById,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("User/GetAutoResponse")]
        public IHttpActionResult GetAutoResponse(int organisationId)
        {
            try
            {
                int result = UserProvider.Instance.GetAutoResponse(organisationId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/GetAutoResponse,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("User/GetReplyMailPDF")]
        public IHttpActionResult GetReplyMailPDF( long OrganisationId, string userschema)
        {
            try
            {
               string responseMsg = UserProvider.Instance.GetReplyMailPDF(OrganisationId, "STP_USER_PREFERENCES");
                return Content(HttpStatusCode.OK, responseMsg);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/GetReplyMailPDF,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [Route("User/GetContactInformation")]
        public IHttpActionResult GetContactInformation(long ContactId)
        {
            try
            {
                ContactModel obj = UserProvider.Instance.GetContactInformation(ContactId);
                return Content(HttpStatusCode.OK, obj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/GetReplyMailPDF,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #region EditOrganisation
        [HttpPost]
        [Route("User/EditOrganisation")]
        public IHttpActionResult EditOrganisation(Organization orgDetails)
        {
            try
            {
                Organization objOrgDetails = UserProvider.Instance.EditOrganisation(orgDetails);
                return Content(HttpStatusCode.OK, objOrgDetails);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/EditOrganisation,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        [HttpPost]
        [Route("User/SearchUserCriteria")]
        public IHttpActionResult SearchUserCriteria(UserContactListParams userContactList)
        {
            try
            {
                List<GetUserList> result = UserProvider.Instance.SearchUserCriteria(userContactList.UserTypeId, userContactList.OrganisationId, userContactList.UserContactSearchItems);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/SearchUserCriteria,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [Route("User/GetUserbyOrgId")]
        public IHttpActionResult GetUserbyOrgId(string UserTypeId, string OrganisationId)
        {
            try
            {
                List<GetUserList> result = UserProvider.Instance.GetUserbyOrgId(UserTypeId, OrganisationId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -User/GetUserbyOrgId,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
    }
}