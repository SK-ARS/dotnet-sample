using STP.Domain;
using STP.Domain.Communications;
using STP.Domain.SecurityAndUsers;
using STP.SecurityAndUsers.Interface;
using STP.SecurityAndUsers.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace STP.SecurityAndUsers.Providers
{
    public class ExpiryProvider : IExpiryProvider
    {
        #region ExpiryProvider Singleton
        private ExpiryProvider()
        {
        }
        internal static ExpiryProvider Instance
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
            internal static readonly ExpiryProvider instance = new ExpiryProvider();
        }

        #region Logger instance

        private const string PolicyName = "UserProvider";

        #endregion

        #endregion
        public PasswordExpiry GetPasswordExpiryInfo(int Flag = 0)
        {
            return PasswordExpiryDAO.GetPasswordExpiryInfo();
        }

        public int UpdatePassword(ChangePasswordInfo changePasswordInfo)
        {
            return PasswordExpiryDAO.ChangePassword(changePasswordInfo);
        }
        public int UpdateExpiryPassword(ChangePasswordInfo changePasswordInfo)
        {
            return PasswordExpiryDAO.ChangeExpiryPassword(changePasswordInfo);
        }

        #region Forgot Password
        public int GenerateOTP(OTPPasswordUpdation otppasswordUpdation)
        {
            return PasswordExpiryDAO.GenerateOTP(otppasswordUpdation);
        }

        public int UpdateForgotPassword(OTPPasswordUpdation otppasswordUpdation)
        {
            return PasswordExpiryDAO.UpdateForgotPassword(otppasswordUpdation);
        }
        #endregion

        #region Get Username
        public int GetUserName(string userEmail)
        {
            return PasswordExpiryDAO.GetUserName(userEmail);
        }

        #endregion


        public int ValidateUser(string userName, string userEmail)
        {
            return PasswordExpiryDAO.ValidateUser(userName, userEmail);
        }
    }
}