using STP.Domain.ExternalAPI;
using STP.Domain.SecurityAndUsers;
using System.Collections.Generic;

namespace STP.ServiceAccess.SecurityAndUsers
{
    public interface IAuthenticationService
    {
        UserInfo GetLoginInfo(UserInfo userLogin);
        int CheckNewPAssword(ChangePasswordInfo passwordInfo);
        PasswordExpiry GetPasswordExpiryInfo(int flag);        
        List<SecurityQuestionInfo> GetSecurityQuestion();
        List<MenuPrivileage> GetMenuInfo(int userTypeId);
        int SaveTermsAndConditions(string user_id);
        int UpdatePassword(ChangePasswordInfo passwordInfo);
        int UpdateExpiryPassword(ChangePasswordInfo passwordInfo);
        UserInfo GetAnotherLogin(string UserName);
        int GenerateOTP(OTPPasswordUpdation otppasswordUpdation);
        int GetUsername(string userEmailId);
        ValidateAuthentication ValidateAuthentication(string authenticationKey);
        int GetPasswords(string password);
        AuthKeyValid GetOrgDetailsByAuthKey(string authenticationKey);
        AuthorizedOrganisation GetAuthorizedUsers(string ESDALReferenceNumber, bool isApp);
    }
}
