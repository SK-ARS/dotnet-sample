using STP.Domain.ExternalAPI;
using STP.Domain.SecurityAndUsers;
using STP.SecurityAndUsers.Interface;
using STP.SecurityAndUsers.Persistance;
using System.Collections.Generic;
using System.Diagnostics;
namespace STP.SecurityAndUsers.Providers
{
    public sealed class UserProvider : IUserProvider
    {
        #region UserProvider Singleton
        private UserProvider()
        {
        }
        public static UserProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }
        /// <summary>
        /// Not to be called while using logic
        /// </summary>
        internal static class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly UserProvider instance = new UserProvider();
        }
        #region Logger instance
        private const string PolicyName = "UserProvider";
        #endregion
        #endregion
        public UserInfo GetLoginInfo(string userId, string password)
        {
            return UserDAO.GetLogin(userId, password);
        }
        public int CheckNewPAssword(ChangePasswordInfo changePasswordInfo)
        {
            return UserDAO.CheckNewPAssword(changePasswordInfo);
        }
        public int SaveTermsAndConditions(string userId)
        {
            return UserDAO.SaveTermsAndConditions(userId);
        }
        public UserInfo GetAnotherUser(string userId)
        {
            return UserDAO.GetAnotherUser(userId);
        }
        public List<MenuPrivileage> GetMenuInfo(int userId)
        {
            return MenuDAO.GetMenuInfo(userId);
        }
        public List<SecurityQuestionInfo> GetSecurityQuestion()
        {
            return UserDAO.GetSecurityQuestions();
        }
        public List<UserRegistration> GetUserType()
        {
            return UserDAO.GetUserType();
        }
       
        #region GetCountryInfo
        /// <summary>
        /// To obtain the list of country name and country code
        /// </summary>
        /// <returns></returns>
        public List<UserRegistration> GetCountryInfo()
        {
            return UserDAO.GetListOfCountries();
        }
        #endregion
        #region GetUserByID
        public List<UserRegistration> GetUserByID(string userTypeId, int userId, int contactId)
        {
            return UserDAO.GetUserByID(userTypeId, userId, contactId);
        }
        #endregion
        #region GetUserByName
        public decimal GetUserByName(string userName, int type, string mode, string userId)
        {
            return UserDAO.SearchUserByName(userName, type, mode, userId);
        }
        #endregion
        #region UpdateUser
        public int UpdateUser(UserRegistration regDet, int userType, int userId)
        {
            return UserDAO.UpdateRegInfo(regDet, userType, userId);
        }
        #endregion
        #region SetRegInfo
        public UserRegistration SetRegInfo(UserRegistration regDet, int userTypeId)
        {
            return UserDAO.SetRegInfo(regDet, userTypeId);
        }
        #endregion
        #region DeleteUser
        public int DeleteUser(int userId, int deleteVal)
        {
            return UserDAO.DeleteUser(userId, deleteVal);
        }
        #endregion
        #region DeleteContact
        public int DeleteContact(int contactId, int deleteVal)
        {
            return UserDAO.DeleteContact(contactId, deleteVal);
        }
        #endregion
        #region UserList
        public List<GetUserList> UserList(string userTypeId, string organisationId, int pageNumber, int pageSize, UserContactSearchItems userContactSearchItems, int sortOrder, int presetFilter, int sortFlag = 0)
        {
            if (sortFlag == 1)
            {
                return UserDAO.GetSORTUserListInfo(userTypeId);
            }
            else
            {
                return UserDAO.GetUserListInfo(userTypeId, organisationId, pageNumber, pageSize, userContactSearchItems,sortOrder,presetFilter);
            }
        }
        #endregion
        #region CheckToDisableUser
        public bool CheckToDisableUser(int userId)
        {
            return UserDAO.DelegationFailureAlertForDisabling(userId);
        }
        #endregion
        #region SetContactRegInfo
        public UserRegistration SetContactRegInfo(UserRegistration regDet, int userTypeId)
        {
            return UserDAO.SetContactRegInfo(regDet, userTypeId);
        }
        #endregion
        #region UpdateContact
        public int UpdateContact(UserRegistration regDet, int userType, int contactId)
        {
            return UserDAO.UpdateContact(regDet, userType, contactId);
        }
        #endregion
        #region GetSearchUser
        public bool GetSearchUser(string value, string Type)
        {
            return UserDAO.GetSearchUser(value, Type);
        }
        #endregion
        #region ViewOrganisationByID
        public List<ViewOrganizationByID> ViewOrganizationByID(int orgId)
        {
            return UserDAO.ViewOrganisationByID(orgId);
        }
        #endregion
      
        public List<OrganizationTypeList> GetOrganisationTypeList()      
        {
            return UserDAO.GetOrganisationTypeList();
        }
        #region SetUserPref
        public bool SetUserPreference(UserPreferences objUserPreference, int userId, string emailUpdate, string faxNumber)
        {
            return UserDAO.SetUserPreference(objUserPreference, userId, emailUpdate, faxNumber);
        }
        #endregion

        public List<GetHaulContactByOrgID> GetHaulierContactByOrgID(int organisationId)
        {
            return UserDAO.GetHaulierContactByOrgID(organisationId);
        }
        public List<UserDetailsModel> GetUsersByOrgID(int organisationId,int UserTypeId)
        {
            return UserDAO.GetUsersByOrgID(organisationId, UserTypeId);
        }
        public UserPreferences GetUserPreferencesById(int userId)
        {
             return UserDAO.GetUserPreferencesById(userId);
        }
        public int GetAutoResponse(int organisationId)
        {
            return UserDAO.GetAutoResponse(organisationId);
        }
        //Get Replay Mail PDF
        public string GetReplyMailPDF(long OrganisationId, string userschema = "STP_USER_PREFERENCES")
        {
            return UserDAO.GetReplyMailPDF(OrganisationId, userschema);
        }
        public Organization EditOrganisation(Organization orgDetails)
        {
            return UserDAO.EditOrganisation(orgDetails);
        }
        public ContactModel GetContactInformation(long ContactId)
        {
            return UserDAO.GetContactInformation(ContactId);
        }
        public int GetPasswords(string strpassword)
        {
            return UserDAO.GetPasswords(strpassword);
        }
        public AuthKeyValid GetOrgDetailsByAuthKey(string authenticationKey)
        {
            return OrganizationDAO.GetOrgDetailsByAuthKey(authenticationKey);
        }
        public List<GetUserList> SearchUserCriteria(string userTypeId, string organisationId, UserContactSearchItems userContactSearchItems)
        {
            return UserDAO.SearchUserCriteria(userTypeId, organisationId,userContactSearchItems);
        }
        public List<GetUserList> GetUserbyOrgId(string userTypeId, string organisationId)
        {
            return UserDAO.GetUserbyOrgId(userTypeId, organisationId);
        }

        public AuthorizedOrganisation GetAuthorizedUsers(string ESDALReferenceNumber, bool isApp)
        {
            return OrganizationDAO.GetAuthorizedUsers(ESDALReferenceNumber, isApp);
        }
    }
}