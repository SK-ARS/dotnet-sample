using STP.Domain;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.SecurityAndUsers
{
  public  interface IUserService
    {
        List<UserRegistration> GetUserByID(string userTypeId, int userId, int contactId);
        decimal GetUserByName(string userName, int type, string mode, string userId);
        int UpdateUser(UserParams updateUserParams);
        UserRegistration SetRegInfo(UserParams userParams);
        int DeleteUser(int UserId, int deleteVal);
        int DeleteContact(int ContactId, int deleteVal);
        List<GetUserList> UserList(string userTypeID, string organisationId, int pageNumber, int pageSize, UserContactSearchItems userContactSearchItems, int presetFilter, int sortOrder);
        bool CheckToDisableUser(int userId);
        UserRegistration SetContactRegInfo(UserParams updateUserParams);
        int UpdateContact(UserParams updateParams);
        bool GetSearchUser(string value, string Type);       
        List<UserRegistration> GetUserType();        
        List<UserRegistration> GetCountryInfo();
        List<OrganizationTypeList> GetOrganisationTypeList();
        List<GetHaulContactByOrgID> GetHaulierContactByOrgID(int organisationId);
        List<UserDetailsModel> GetUsersByOrgID(long organisationId,int UserTypeId);
        UserPreferences GetUserPreferencesById(int userId);
        int GetAutoResponse(int organisationId);
        bool SetUserPreference(UserPreferenceParams userPreferenceParams);
        string GetReplyMailPDF(long OrganisationId, string userschema);
        Organization EditOrganisation(string orgID, Organization orgDet);
        ContactModel GetContactInformation(long ContactId);
        List<GetUserList> SearchUserCriteria(string userTypeId, string organisationId, UserContactSearchItems userContactSearchItems);
        List<GetUserList> GetUserbyOrgId(string userTypeId, string organisationId);

    }
}
