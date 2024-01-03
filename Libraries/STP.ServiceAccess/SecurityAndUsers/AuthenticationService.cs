using STP.Common.Logger;
using STP.Domain.ExternalAPI;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
namespace STP.ServiceAccess.SecurityAndUsers
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly HttpClient httpClient;
       
        public AuthenticationService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        #region private Object GetLoginInfo()
        public UserInfo GetLoginInfo(UserInfo userLogin)
        {
            HttpResponseMessage response = httpClient.PostAsJsonAsync(
            $"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
            $"/Authentication/GetLoginInfo",
            userLogin).Result;
            //api call to new service
            UserInfo userInfo = new UserInfo();
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                userInfo = response.Content.ReadAsAsync<UserInfo>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"AuthenticationService/GetLoginInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                var contents = response.Content.ReadAsStringAsync();
                userInfo.ResponseContent = contents.Result;
            }
            return userInfo;
        }
        #endregion
        #region public Object CheckNewPAssword()
        public int CheckNewPAssword(ChangePasswordInfo passwordInfo)
        {
            HttpResponseMessage response = httpClient.PostAsJsonAsync(
            $"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
            $"/Authentication/CheckNewPAssword",
            passwordInfo).Result;
            //api call to new service
            int result =-1 ;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                result = response.Content.ReadAsAsync<int>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"AuthenticationService/CheckNewPAssword, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return result;
        }
        #endregion
        #region private Object GetPasswordExpiryInfo()
        public PasswordExpiry GetPasswordExpiryInfo(int flag)
        {
            HttpResponseMessage response = httpClient.GetAsync(
            $"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
            $"/Authentication/GetPasswordExpiryInfo?Flag=" + flag).Result;
            //api call to new service
            PasswordExpiry passwordExpiry = null;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                passwordExpiry = response.Content.ReadAsAsync<PasswordExpiry>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"AuthenticationService/GetPasswordExpiryInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return passwordExpiry;
        }
        #endregion
        #region public Object GetSecurityQuestion()
        public List<SecurityQuestionInfo> GetSecurityQuestion()
        {
            HttpResponseMessage response = httpClient.GetAsync(
            $"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
            $"/Authentication/GetSecurityQuestion").Result;
            //api call to new service
            List<SecurityQuestionInfo> securityQuestionInfos = null;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                securityQuestionInfos = response.Content.ReadAsAsync<List<SecurityQuestionInfo>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"AuthenticationService/GetSecurityQuestion, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return securityQuestionInfos;
        }
        #endregion
        #region public Object GetMenuInfo()
        public List<MenuPrivileage> GetMenuInfo(int userTypeId)
        {
            List<MenuPrivileage> menuPrivileages = new List<MenuPrivileage>();
            try
            {
                string urlParameters = "?userId=" + userTypeId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
                $"/Authentication/GetMenuInfo{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    menuPrivileages = response.Content.ReadAsAsync<List<MenuPrivileage>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"AuthenticationService/GetMenuInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"AuthenticationService/GetMenuInfo, Exception: {ex}");
            }
            return menuPrivileages;
        }
        #endregion
        #region public Object GetMenuInfo()
        public int SaveTermsAndConditions(string user_id)
        {
            int result = 0;
            try
            {
                string urlParameters = "?userId=" + user_id;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
                $"/Authentication/SetTermsAndConditions{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"AuthenticationService/SaveTermsAndConditions, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"AuthenticationService/SaveTermsAndConditions, Exception: {ex}");
            }
            return result;
        }
        #endregion
        #region public Object UpdatePassword()
        public int UpdatePassword(ChangePasswordInfo passwordInfo)
        {
            passwordInfo.ConfirmPassword = "";
            HttpResponseMessage response = httpClient.PostAsJsonAsync(
            $"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
            $"/Authentication/UpdatePassword",
            passwordInfo).Result;
            //api call to new service
            //1- update sucessfully, 0- Norecords found, -1= updation failed due to some error.
            int result = -1;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                result = response.Content.ReadAsAsync<int>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"AuthenticationService/UpdatePassword, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return result;
        }
        #endregion
        #region public Object UpdateExpiryPassword()
        public int UpdateExpiryPassword(ChangePasswordInfo passwordInfo)
        {
            passwordInfo.ConfirmPassword = "";
            HttpResponseMessage response = httpClient.PostAsJsonAsync(
            $"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
            $"/Authentication/UpdateExpiryPassword",
            passwordInfo).Result;
            //api call to new service
            //1- update sucessfully, 0- Norecords found, -1= updation failed due to some error.
            int result = -1;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                result = response.Content.ReadAsAsync<int>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"AuthenticationService/UpdateExpiryPassword, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return result;
        }
        #endregion
        #region public  Object GetAnotherLogin()
        public UserInfo GetAnotherLogin(string UserName)
        {
            UserInfo objUserInfo = new UserInfo();
            try
            {
                string urlParameters = "?userName=" + UserName;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
             $"/Authentication/GetAnotherLogin{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objUserInfo = response.Content.ReadAsAsync<UserInfo>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Account/GetAnotherLogin, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Account/GetAnotherLogin, Exception: {ex}");
            }
            return objUserInfo;
        }
        #endregion
        #region ForgetPassword
        public int GenerateOTP(OTPPasswordUpdation otppasswordUpdation)
        {
            int result = 0;
            try
            {
                // string urlParameters = "?userName=" + userName + "&userEmail=" + EmailId;
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
                $"/Authentication/GenerateOTP", otppasswordUpdation).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Authentication/GenerateOTP, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $" AuthenticationService/GenerateOTP, Exception: {ex}");
            }
            return result;
        }
        #endregion
        #region GetUsername
        public int GetUsername(string userEmailId)
        {
            int result = 0;
            try
            {
                string urlParameters = "?userEmail=" + userEmailId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
             $"/Authentication/GetUsername{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Authentication/GetUsername, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"AuthenticationService/GetUsername, Exception: {ex}");
            }
            return result;
        }
        #endregion
        #region Validate Authentication Key
        public ValidateAuthentication ValidateAuthentication(string authenticationKey)
        {
            ValidateAuthentication authentication = new ValidateAuthentication();
            try
            {
                string urlParameters = "?authenticationKey=" + authenticationKey;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
                $"/Authentication/ValidateAuthentication{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    authentication = response.Content.ReadAsAsync<ValidateAuthentication>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"AuthenticationService/ValidateAuthentication, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"AuthenticationService/ValidateAuthentication, Exception: {ex}");
            }
            return authentication;
        }
        #endregion

        public int GetPasswords(string password)
        {
            int iCount = 0;
            string strUrlParam = "?password=" + password;
            HttpResponseMessage response = httpClient.GetAsync(
            $"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
            $"/Authentication/GetPasswords"+ strUrlParam).Result;           
            if (response.IsSuccessStatusCode)
            {
                iCount = response.Content.ReadAsAsync<int>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"AuthenticationService/GetPasswords, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return iCount;
        }

        #region GetOrgDetailsByAuthKey
        public AuthKeyValid GetOrgDetailsByAuthKey(string authenticationKey)
        {
            AuthKeyValid orgDetails = new AuthKeyValid();
            try
            {
                string urlParameters = "?authenticationKey=" + authenticationKey;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
                $"/Authentication/GetOrgDetailsByAuthKey{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    orgDetails = response.Content.ReadAsAsync<AuthKeyValid>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"AuthenticationService/GetOrgDetailsByAuthKey, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"AuthenticationService/GetOrgDetailsByAuthKey, Exception: {ex}");
            }
            return orgDetails;
        }
        #endregion

        #region GetOrgDetailsByAuthKey
        public AuthorizedOrganisation GetAuthorizedUsers(string ESDALReferenceNumber, bool isApp)
        {
            AuthorizedOrganisation orgDetails = new AuthorizedOrganisation();
            try
            {
                string urlParameters = "?ESDALReferenceNumber=" + Uri.EscapeDataString(ESDALReferenceNumber) + "&isApp="+ isApp;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
                $"/Authentication/GetAuthorizedUsers{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    orgDetails = response.Content.ReadAsAsync<AuthorizedOrganisation>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"AuthenticationService/GetAuthorizedUsers, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"AuthenticationService/GetAuthorizedUsers, Exception: {ex}");
            }
            return orgDetails;
        }
        #endregion
    }
}
