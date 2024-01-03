using STP.Domain.ExternalAPI;
using STP.Domain.SecurityAndUsers;
using System.Collections.Generic;

namespace STP.SecurityAndUsers.Interface
{
    interface IUserProvider
    {
        int CheckNewPAssword(ChangePasswordInfo changePasswordInfo);
        UserInfo GetLoginInfo(string userId, string password);
        int SaveTermsAndConditions(string userId);
        UserInfo GetAnotherUser(string userId);
        List<MenuPrivileage> GetMenuInfo(int userId);
        List<UserRegistration> GetUserByID(string userTypeId, int userId, int contactId);
        decimal GetUserByName(string userName, int type, string mode, string userId);
        int UpdateUser(UserRegistration regDet, int userType, int userId);
        UserRegistration SetRegInfo(UserRegistration regDet, int userTypeId);
        int DeleteUser(int userId, int deleteVal);
        int DeleteContact(int contactId, int deleteVal);
        List<GetUserList> UserList(string userTypeId, string organisationId, int pageNumber, int pageSize, UserContactSearchItems userContactSearchItems, int sortOrder, int presetFilter, int sortFlag = 0);
        bool CheckToDisableUser(int userId);
        UserRegistration SetContactRegInfo(UserRegistration regDet, int userTypeId);
        int UpdateContact(UserRegistration regDet, int userType, int contactId);
        bool GetSearchUser(string value, string Type);
        List<ViewOrganizationByID> ViewOrganizationByID(int orgId);
        List<GetHaulContactByOrgID> GetHaulierContactByOrgID(int organisationId);
        List<UserDetailsModel> GetUsersByOrgID(int organisationId,int UserTypeId);
        UserPreferences GetUserPreferencesById(int userId);
        int GetAutoResponse(int organisationId);
        Organization EditOrganisation(Organization orgDetails);
        ContactModel GetContactInformation(long ContactId);
        int GetPasswords(string strpassword);
        AuthKeyValid GetOrgDetailsByAuthKey(string authenticationKey);
        List<GetUserList> SearchUserCriteria(string userTypeId, string organisationId, UserContactSearchItems userContactSearchItems);
        List<GetUserList> GetUserbyOrgId(string userTypeId, string organisationId);
        AuthorizedOrganisation GetAuthorizedUsers(string ESDALReferenceNumber, bool isApp);
    }
}
