using STP.Domain;
using STP.Domain.Communications;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.SecurityAndUsers.Interface
{
    interface IExpiryProvider
    {
        /// Get Password expiry details
        /// </summary>
        /// <returns>PasswordExpiry object</returns>
        PasswordExpiry GetPasswordExpiryInfo(int Flag = 0);
        int UpdatePassword(ChangePasswordInfo changePasswordInfo);
        int UpdateExpiryPassword(ChangePasswordInfo changePasswordInfo);      
        int UpdateForgotPassword(OTPPasswordUpdation otppasswordUpdation);
        int GenerateOTP(OTPPasswordUpdation otppasswordUpdation);
        int GetUserName(string userEmail);
        int ValidateUser(string userName, string userEmail);
    }
}
