using STP.Common.Logger;
using STP.Domain;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace STP.ServiceAccess.SecurityAndUsers
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;
        public UserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        #region GetUserByID
        public List<UserRegistration> GetUserByID(string userTypeId, int userId, int contactId)
        {
            List<UserRegistration> objUserInfo = new List<UserRegistration>();
            try
            {
                string urlParameters = "?userTypeId=" + userTypeId + "&userId=" + userId + "&contactId=" + contactId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/User/GetUserByID{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objUserInfo = response.Content.ReadAsAsync<List<UserRegistration>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/GetUserByID, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/GetUserByID, Exception: {ex}");
            }
            return objUserInfo;
        }
        #endregion
        #region GetUserByName
        public decimal GetUserByName(string userName, int type, string mode, string userId)
        {
            decimal result = 0;
            try
            {
                string urlParameters = "?userName=" + userName + "&type=" + type + "&mode=" + mode + "&userId=" + userId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/User/GetUserByName{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<decimal>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/GetUserByName, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/GetUserByName, Exception: {ex}");
            }
            return result;
        }
        #endregion
        #region UpdateUser
        public int UpdateUser(UserParams updateUserParams)
        {
            int result = 0;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/User/UpdateUser", updateUserParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/UpdateUser, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/UpdateUser, Exception: {ex}");
            }
            return result;
        }
        #endregion
        #region SetRegInfo
        public UserRegistration SetRegInfo(UserParams userParams)
        {
            UserRegistration result = new UserRegistration();
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/User/SetRegInfo", userParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<UserRegistration>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/SetRegInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/SetRegInfo, Exception: {ex}");
            }
            return result;
        }
        #endregion
        #region DeleteUser
        public int DeleteUser(int UserId, int deleteVal)
        {
            int result = 0;
            try
            {
                string urlParameters = "?UserId=" + UserId + "&deleteVal=" + deleteVal;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/User/DeleteUser{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/DeleteUser, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/DeleteUser, Exception: {ex}");
            }
            return result;
        }
        #endregion
        #region DeleteContact
        public int DeleteContact(int ContactId, int deleteVal)
        {
            int result = 0;
            try
            {
                string urlParameters = "?ContactId=" + ContactId + "&deleteVal=" + deleteVal;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/User/DeleteContact{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/DeleteContact, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/DeleteContact, Exception: {ex}");
            }
            return result;
        }
        #endregion
        #region UserList
        public List<GetUserList> UserList(string userTypeID, string organisationId, int pageNumber, int pageSize, UserContactSearchItems userContactSearchItems, int presetFilter, int sortOrder)
        {
            List<GetUserList> objUserInfo = new List<GetUserList>();
            try
            {
                UserContactListParams userContactListParams = new UserContactListParams()
                {
                    UserTypeId= userTypeID,
                    OrganisationId= organisationId,
                    PageNumber= pageNumber,
                    PageSize= pageSize,
                    UserContactSearchItems= userContactSearchItems,
                    PresetFilter=presetFilter,
                    SortOrder=sortOrder
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/User/UserList", userContactListParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    objUserInfo = response.Content.ReadAsAsync<List<GetUserList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/UserList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/UserList, Exception: {ex}");
            }
            return objUserInfo;
        }
        #endregion
        #region CheckToDisableUser
        public bool CheckToDisableUser(int userId)
        {
            bool result = false;
            try
            {
                string urlParameters = "?userId=" + userId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/User/CheckToDisableUser{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/CheckToDisableUser, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/CheckToDisableUser, Exception: {ex}");
            }
            return result;
        }
        #endregion
        #region SetContactRegInfo
        public UserRegistration SetContactRegInfo(UserParams updateUserParams)
        {
            UserRegistration userRegistration = new UserRegistration();
            try
            {    
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/User/SetContactRegInfo", updateUserParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    userRegistration = response.Content.ReadAsAsync<UserRegistration>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/SetContactRegInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/SetContactRegInfo, Exception: {ex}");
            }
            return userRegistration;
        }
        #endregion
        #region UpdateContact
        public int UpdateContact(UserParams updateParams)
        {
            int result = 0;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/User/UpdateContact", updateParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/UpdateContact, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/UpdateContact, Exception: {ex}");
            }
            return result;
        }
        #endregion
        #region GetSearchUser
        public bool GetSearchUser(string value, string Type)
        {
            bool result = false;
            try
            {
                string urlParameters = "?value=" + value + "&Type=" + Type;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/User/GetSearchUser{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/GetSearchUser, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/GetSearchUser, Exception: {ex}");
            }
            return result;
        }
        #endregion
        
        #region GetUserType
        public List<UserRegistration> GetUserType()
        {
            List<UserRegistration> userType = new List<UserRegistration>();
            HttpResponseMessage responseMessage = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/User/GetUserType").Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                userType = responseMessage.Content.ReadAsAsync<List<UserRegistration>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/GetUserType, Error: {(int)responseMessage.StatusCode} - {responseMessage.ReasonPhrase}");
            }
            return userType;
        }
        #endregion
        #region GetCountryInfoForUser
        public List<UserRegistration> GetCountryInfo()
        {
            List<UserRegistration> userCountryInfo = new List<UserRegistration>();
            HttpResponseMessage responseMessage = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/User/GetCountryInfo").Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                userCountryInfo = responseMessage.Content.ReadAsAsync<List<UserRegistration>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/GetCountryInfo, Error: {(int)responseMessage.StatusCode} - {responseMessage.ReasonPhrase}");
            }
            return userCountryInfo;
        }
        #endregion

        #region GetOrganisationTypeList
        public List<OrganizationTypeList> GetOrganisationTypeList()
        {
            List<OrganizationTypeList> organisatationInfo=new List<OrganizationTypeList>();
            HttpResponseMessage responseMessage = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/User/GetOrganisationTypeList").Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                organisatationInfo = responseMessage.Content.ReadAsAsync<List<OrganizationTypeList>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/GetOrganisationTypeList, Error: {(int)responseMessage.StatusCode} - {responseMessage.ReasonPhrase}");
            }
            return organisatationInfo;
        }
        #endregion

        public List<GetHaulContactByOrgID> GetHaulierContactByOrgID(int organisationId)
        {
            List<GetHaulContactByOrgID> haulierContactInfo = new List<GetHaulContactByOrgID>();
            HttpResponseMessage responseMessage = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/User/GetHaulierContactByOrgID?organisationId=" + organisationId).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                haulierContactInfo = responseMessage.Content.ReadAsAsync<List<GetHaulContactByOrgID>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/GetHaulierContactByOrgID, Error: {(int)responseMessage.StatusCode} - {responseMessage.ReasonPhrase}");
            }
            return haulierContactInfo;
        }

        public List<UserDetailsModel> GetUsersByOrgID(long organisationId, int UserTypeId)
        {
            List<UserDetailsModel> haulierContactInfo = new List<UserDetailsModel>();
            HttpResponseMessage responseMessage = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/User/GetUsersByOrgID?organisationId=" + organisationId+"&UserTypeId="+ UserTypeId).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                haulierContactInfo = responseMessage.Content.ReadAsAsync<List<UserDetailsModel>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/GetUsersByOrgID, Error: {(int)responseMessage.StatusCode} - {responseMessage.ReasonPhrase}");
            }
            return haulierContactInfo;
        }

        #region SetUserPreference
        public bool SetUserPreference(UserPreferenceParams userPreferenceParams)
        {
            bool flag = false;
            UserPreferencesDetails userPreferencesDetails = new UserPreferencesDetails();
            HttpResponseMessage responseMessage = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/User/SetUserPreference", userPreferenceParams).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                flag = responseMessage.Content.ReadAsAsync<bool>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/SetUserPreference, Error: {(int)responseMessage.StatusCode} - {responseMessage.ReasonPhrase}");
            }
            return flag;
        }
        #endregion

        public UserPreferences GetUserPreferencesById(int userId)
        {
            UserPreferences userPreferences = new UserPreferences();
            HttpResponseMessage responseMessage = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/User/GetUserPreferencesById?userId=" + userId).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                userPreferences = responseMessage.Content.ReadAsAsync<UserPreferences>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/GetUserPreferencesById, Error: {(int)responseMessage.StatusCode} - {responseMessage.ReasonPhrase}");
            }
            return userPreferences;
        }

        public int GetAutoResponse(int organisationId)
        {
            int iResponse = 1;
            var jsonInput = Newtonsoft.Json.JsonConvert.SerializeObject(organisationId);
            HttpResponseMessage responseMessage = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/User/GetAutoResponse?organisationId="+ organisationId).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                iResponse = responseMessage.Content.ReadAsAsync<int>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/GetAutoResponse, Error: {(int)responseMessage.StatusCode} - {responseMessage.ReasonPhrase}");
            }
            return iResponse;
        }

        public string GetReplyMailPDF(long OrganisationId, string userschema)
        {
            string result = string.Empty;
            string urlParameter = "?OrganisationId=" + OrganisationId + "&userschema=" + userschema;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
           $"/User/GetReplyMailPDF{urlParameter}").Result;

            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<string>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/GetReplyMailPDF, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return result;
        }
        #region EditOrganisation
        public Organization EditOrganisation(string orgID, Organization orgDet)
        {
            Organization objOrgDetails = null ;
            orgDet.OrgID = decimal.Parse(orgID);
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/User/EditOrganisation", orgDet).Result;
                if (response.IsSuccessStatusCode)
                {
                    objOrgDetails = response.Content.ReadAsAsync<Organization>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/EditOrganisation, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/EditOrganisation, Exception: {ex}");
            }
            return objOrgDetails;
        }
        #endregion

        public ContactModel GetContactInformation(long ContactId)
        {
            ContactModel UserInfo =new ContactModel();
            try
            {
                string urlParameters = "?ContactId=" + ContactId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
                             $"/User/GetContactInformation{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    UserInfo = response.Content.ReadAsAsync<ContactModel>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/GetContactInformation, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/GetContactInformation, Exception: {ex}");
            }
            return UserInfo;
        }
        public List<GetUserList> SearchUserCriteria(string userTypeId, string organisationId, UserContactSearchItems userContactSearchItems)
        {
            List<GetUserList> objUserInfo = new List<GetUserList>();
            try
            {
                UserContactListParams userContactListParams = new UserContactListParams()
                {
                    UserTypeId = userTypeId,
                    OrganisationId = organisationId, 
                    UserContactSearchItems = userContactSearchItems
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/User/SearchUserCriteria", userContactListParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    objUserInfo = response.Content.ReadAsAsync<List<GetUserList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/SearchUserCriteria, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/SearchUserCriteria, Exception: {ex}");
            }
            return objUserInfo;
        }
        public List<GetUserList> GetUserbyOrgId(string userTypeId, string organisationId)
        {
            List<GetUserList> objUserInfo = new List<GetUserList>();
            try
            {

              
                string urlParameters = "?UserTypeId=" + userTypeId+ "&OrganisationId=" + organisationId;
               
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/User/GetUserbyOrgId{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objUserInfo = response.Content.ReadAsAsync<List<GetUserList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/GetUserbyOrgId, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/SearchUserCriteria, Exception: {ex}");
            }
            return objUserInfo;
        }


    }
}
