using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.Domain;
using STP.Domain.Communications;
using STP.Domain.SecurityAndUsers;
using STP.ServiceAccess.CommunicationsInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace STP.SecurityAndUsers.Persistance
{
    internal static class PasswordExpiryDAO
    {
        private static ICommunicationsInterfaceService communicationService;

        internal static PasswordExpiry GetPasswordExpiryInfo(int Flag = 0)
        {
            var info = new PasswordExpiry();
               SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               info,
                UserSchema.Portal + ".STP_LOGIN_PKG.GetExpiryPeriod",
               parameter =>
               {
                   parameter.AddWithValue("p_Flag", Flag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
               },
               records =>
               {
                   info.PasswordLife = records.GetStringOrDefault("NAME1"); 
                   info.ReminderPeriod = records.GetStringOrDefault("NAME2");
               }
               );
            return info;
        }
        internal static int ChangePassword(ChangePasswordInfo changePasswordInfo)
        {
            int result = 0;
                SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                 UserSchema.Portal + ".STP_LOGIN_PKG.Update_UserPassword",
                parameter =>
                {
                    parameter.AddWithValue("p_USERID", changePasswordInfo.UserId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PASSWORD", changePasswordInfo.NewPassword, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SECURITY_QUESTION_ID", changePasswordInfo.SecurityQuestion, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SECURITY_ANSWER", changePasswordInfo.SecurityAnswer, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ACCEPTED_TERMS", changePasswordInfo.AcceptedTerms, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ACCEPTED_TERMS_ID", changePasswordInfo.AcceptedTermId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REQUIRED_TERMS", changePasswordInfo.RequiredTerm, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = records.GetInt32("P_AFFECTED_ROWS");
                }
            );
            return result;
        }
        internal static int ChangeExpiryPassword(ChangePasswordInfo changePasswordInfo)
        {
            int output = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
             UserSchema.Portal + ".STP_LOGIN_PKG.Update_ExpiryUserPassword",
            parameter =>
            {
                parameter.AddWithValue("p_USERID", changePasswordInfo.UserId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("p_PASSWORD", changePasswordInfo.NewPassword, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("p_FLAG", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
            },
            records =>
            {
                output = records.GetInt32("P_AFFECTED_ROWS");
            }
        );
            return output;
         
        }

        

        public static int UpdateForgotPassword(OTPPasswordUpdation otppasswordUpdation)
        { 
            int result = 0; 
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
             UserSchema.Portal + ".STP_LOGIN_PKG.Update_ExpiryUserPassword",
            parameter =>
            {
                parameter.AddWithValue("p_USERID", otppasswordUpdation.UserId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("p_PASSWORD", otppasswordUpdation.OTPPassword, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue(" P_FLAG", 1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
            },
            records =>
            {
                result = records.GetInt32("P_AFFECTED_ROWS");
            }

        );
            return result;
        }

        public static int GenerateOTP(OTPPasswordUpdation otppasswordUpdation)
        {
           // int output = 0;
           // UserInfo info = new UserInfo();
           // CommunicationParams communicationParams = new CommunicationParams();
           // SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(info,
           //  UserSchema.Portal + ".STP_LOGIN_PKG.SP_LOGINASOTHERUSER",
           // parameter =>
           // {
           //     parameter.AddWithValue("p_UserName", otppasswordUpdation.UserName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
           //     parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
           // },
           // records =>
           // {
           //     info.Email = records.GetStringOrDefault("EMAIL");
           //     info.UserId = Convert.ToString(records.GetDecimalOrDefault("USER_ID"));
           // }
           //);
           int output = UpdateForgotPassword(otppasswordUpdation);
            //if (output > 0)
            //{

            //    string mailContent = "Hi \n" + info.Email + "You recently asked us to reset your ESDAL account password.\nThe One Time Password(OTP) for login to ESDAL is:" + output.ToString() + "\nClick the link below to sign into your account and choose a new password.\nThis OTP is only valid for the next 60 minutes.After this, you will need to request a new password.\nIf you did not request a password reset, kindly ignore this email.\nThanks,\nESDAL Helpdesk team\nContact : 0300 470 3733(8am - 6pm Mon - Fri excluding bank holidays)";
            //    string subject = "OTP for forget password";
            //    string Matter = mailContent + output.ToString();
            //    byte[] content = Encoding.ASCII.GetBytes(Matter);
            //    byte[] attachment = new byte[0];

            //    communicationParams.UserEmail = info.Email;
            //    communicationParams.Subject = subject;
            //    communicationParams.Content = content;
            //    communicationParams.Attachment = attachment;
            //    communicationParams.ESDALReference = null;
            //    //Sending Mail
            //    //STP.Communications.Communication.MessageTransmiter.SendGeneralmail(info.Email, subject, content, attachment,null);
            //}

            return output;

        }


        public static int GetUserName(string userEmail)
        {
            int output = 0;
            UserInfo info = new UserInfo();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(info,
             UserSchema.Portal + ".STP_LOGIN_PKG.Get_Email_User",
            parameter =>
            {
                parameter.AddWithValue("p_Email_Id", userEmail, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
            records =>
            {
                info.UserName = records.GetStringOrDefault("USERNAME");
               
               
                string subject = "To retrieve forget username";
                string Matter = " Usename accociated with the Email Id :" + info.Email + " is  : " + info.UserName;
                byte[] content = Encoding.ASCII.GetBytes(Matter);
                byte[] attachment = new byte[0];
                //Sending Mail
                //STP.Communications.Communication.MessageTransmiter.SendGeneralmail(info.Email, subject, content, attachment, null);

                output = 1;
            }
           );     

            return output;

        }

        public static int ValidateUser(string userName, string userEmail)
        {

            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                 UserSchema.Portal + ".STP_LOGIN_PKG.sp_IsUserExists",
                parameter =>
                {
                    parameter.AddWithValue("p_USERNAME", userName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TYPE", 2, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_mode", "", OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_UserId", "", OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_user_email", userEmail, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("p_resultset", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = Convert.ToInt32(records.GetDecimalOrDefault("User_ID"));
                }
                );
            return result;
        }
    }
}