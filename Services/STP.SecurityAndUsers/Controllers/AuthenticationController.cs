using STP.Common.Constants;
using STP.Common.Logger;
using STP.DataAccess.Provider;
using STP.Domain.ExternalAPI;
using STP.Domain.SecurityAndUsers;
using STP.SecurityAndUsers.Providers;
using STP.ServiceAccess.CommunicationsInterface;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
namespace STP.SecurityAndUsers.Controllers
{
    public class AuthenticationController : ApiController
    {
        private readonly ICommunicationsInterfaceService communicationService;
        public AuthenticationController(ICommunicationsInterfaceService communicationService)

        {
            this.communicationService = communicationService;
        }
        [HttpPost]
        [Route("Authentication/GetLoginInfo")]
        public IHttpActionResult GetLoginInfo(UserInfo userLogin)
        {
            UserInfo info = new UserInfo();
            try
            {
                string password = userLogin.Password;
                userLogin.Password = MD5Encryption(userLogin.Password);
                info = UserProvider.Instance.GetLoginInfo(userLogin.UserName, userLogin.Password);
                if (info.Password != userLogin.Password || info.UserId == null)
                {
                    return Content(HttpStatusCode.OK, info);
                }
                else
                {
                    info.Password = password;
                }
            }
            catch (DBConnectionFailedException dbEx)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Authentication/GetLogin, Exception: " + dbEx​​​​​​​);
                return Content(HttpStatusCode.NotFound, "Unable to establish connection to Database Server. Please try to login after some time. If the problem persists, contact helpdesk.");

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Authentication/GetLogin, Exception: " + ex​​​​);
                return Content(HttpStatusCode.NotFound, "Internal Exception occured in service");
            }

            return Content(HttpStatusCode.OK, info);
        }
        [HttpGet]
        [Route("Authentication/SetTermsAndConditions")]
        public IHttpActionResult SetTermsAndConditions(string userId)
        {
            int result = 0;
            try
            {
                result = UserProvider.Instance.SaveTermsAndConditions(userId);
                if (result >= 0)
                {
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Authentication/SetTermsAndConditions, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Authentication/GetAnotherLogin")]
        public IHttpActionResult GetAnotherLogin(string userName)
        {
            UserInfo objUserInfo = new UserInfo();
            try
            {
                objUserInfo = UserProvider.Instance.GetAnotherUser(userName);               
                return Content(HttpStatusCode.OK,objUserInfo);              
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Authentication/GetAnotherLogin, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Authentication/GetPasswordExpiryInfo")]
        public IHttpActionResult GetPasswordExpiryInfo(/*int Flag =0*/)
        {
            PasswordExpiry expiryInfo = new PasswordExpiry();
            try
            {
                expiryInfo = ExpiryProvider.Instance.GetPasswordExpiryInfo(/*Flag*/);
                return Content(HttpStatusCode.OK, expiryInfo);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Authentication/GetPasswordExpiryInfo, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("Authentication/UpdatePassword")]
        public IHttpActionResult UpdatePassword(ChangePasswordInfo changePasswordInfo)
        {
            int result = 0; 
            try
            {               
                result = ExpiryProvider.Instance.UpdatePassword(changePasswordInfo);
                if (result >=0)
                {
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.UpdationFailed);
                }
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Authentication/UpdatePassword,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }            
        }
        [HttpPost]
        [Route("Authentication/UpdateExpiryPassword")]
        public IHttpActionResult UpdateExpiryPassword(ChangePasswordInfo changePasswordInfo)
        {
           int result = 0;
            try
            {
                result = ExpiryProvider.Instance.UpdateExpiryPassword(changePasswordInfo);
                if (result >= 0)
                {
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.UpdationFailed);
                }
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Authentication/UpdateExpiryPassword,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }          
        }
        [HttpPost]
        [Route("Authentication/CheckNewPAssword")]
        public IHttpActionResult CheckNewPAssword(ChangePasswordInfo changePasswordInfo)
        {            
            try
            {
                int result= UserProvider.Instance.CheckNewPAssword(changePasswordInfo);
                return Content(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Authentication/CheckNewPAssword,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Authentication/GetMenuInfo")]
        public IHttpActionResult GetMenuInfo(int userId)
        {
            try
            {
                List<MenuPrivileage> menuPrivileages = UserProvider.Instance.GetMenuInfo(userId);
                return Content(HttpStatusCode.OK,menuPrivileages);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Authentication/GetMenuInfo, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Authentication/GetSecurityQuestion")]
        public IHttpActionResult GetSecurityQuestion()
        {
            try
            {
                List<SecurityQuestionInfo> expiryInfo = UserProvider.Instance.GetSecurityQuestion();
                    return Content(HttpStatusCode.OK,expiryInfo);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Authentication/GetSecurityQuestion, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        public string MD5Encryption(string passwordKey)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(passwordKey));
            // Build the final string by converting each byte
            // into hex and appending it to a StringBuilder
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }

        [HttpPost]
        [Route("Authentication/GenerateOTP")]
        public IHttpActionResult GenerateOTP(OTPPasswordUpdation otppasswordUpdation)
        {
            try
            {
                int userId = ExpiryProvider.Instance.ValidateUser(otppasswordUpdation.UserName, otppasswordUpdation.UserEmail);
                if (userId > 0)
                {
                    otppasswordUpdation.UserId = userId;
                    int result = ExpiryProvider.Instance.GenerateOTP(otppasswordUpdation);

                    return Content(HttpStatusCode.OK, result);

                }
                else
                {
                    return Content(HttpStatusCode.OK, -3);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Authentication/GenerateOTP,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }


        //Not Complete
        [HttpGet]
        [Route("Authentication/GetUsername")]
        public IHttpActionResult GetUsername(string userEmail)
        {
            try
            {
                int result = ExpiryProvider.Instance.GetUserName(userEmail);


                if (result <= 0)
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
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Authentication/GetUsername,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Authentication/ValidateAuthentication")]
        public IHttpActionResult ValidateAuthentication(string authenticationKey)
        {
            try
            {
                ValidateAuthentication authentication = OrganizationProvider.Instance.ValidateAuthentication(authenticationKey);
                return Content(HttpStatusCode.OK, authentication);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Authentication/ValidateAuthentication,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Authentication/GetPasswords")]
        public IHttpActionResult GetPasswords(string password)
        {
            try
            {
                int organisationId = UserProvider.Instance.GetPasswords(password);
                return Ok(organisationId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Authentication/GetPasswords,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("Authentication/GetOrgDetailsByAuthKey")]
        public IHttpActionResult GetOrgDetailsByAuthKey(string authenticationKey = null)
        {
            try
            {
                AuthKeyValid orgDetails = UserProvider.Instance.GetOrgDetailsByAuthKey(authenticationKey);
                return Content(HttpStatusCode.OK, orgDetails);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Authentication/ValidateNEAuthentication,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("Authentication/GetAuthorizedUsers")]
        public IHttpActionResult GetAuthorizedUsers(string ESDALReferenceNumber = null, bool isApp = false)
        {
            try
            {
                AuthorizedOrganisation orgDetails = UserProvider.Instance.GetAuthorizedUsers(ESDALReferenceNumber, isApp);
                return Content(HttpStatusCode.OK, orgDetails);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Authentication/ValidateNEAuthentication,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
    }
}
