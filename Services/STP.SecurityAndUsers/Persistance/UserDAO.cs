using NetSdoGeometry;
using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.Provider;
using STP.DataAccess.SafeProcedure;
using STP.Domain;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
namespace STP.SecurityAndUsers.Persistance
{
    public static class UserDAO
    {
        internal static List<SecurityQuestionInfo> GetSecurityQuestions()
        {
            var securityQuestions = new List<SecurityQuestionInfo>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
            securityQuestions,
             UserSchema.Portal + ".STP_LOGIN_PKG.GetAllSecurityQuestions",
             parameter =>
             {
                 parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
             },
            (record, instance) =>
            {
                instance.QuestionId = record.GetInt32OrDefault("CODE");
                instance.SecurityQuestion = record.GetStringOrEmpty("NAME");
            }
            );
            return securityQuestions;
        }
        internal static int CheckNewPAssword(ChangePasswordInfo changePasswordInfo)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                changePasswordInfo,
                 UserSchema.Portal + ".STP_LOGIN_PKG.SP_PASSWORD_CHECK",
                parameter =>
                {
                    parameter.AddWithValue("P_USER_ID", changePasswordInfo.UserId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NEW_PWD", changePasswordInfo.NewPassword, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = (int)records.GetDecimalOrDefault("P_CNT"); 
                }
            );
            return result;
        }
        internal static UserInfo GetLogin(string userId, string password)
        {
            var info = new UserInfo();
            string termsAcceptedFlag;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                info,
                 UserSchema.Portal + ".STP_LOGIN_PKG.GetUser",
                parameter =>
                {
                    parameter.AddWithValue("p_UserName", userId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PASSWORD", password, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                info.UserName = records.GetStringOrDefault("USERNAME");
                info.UserTypeId = records.GetInt32OrDefault("USER_TYPE_ID");
                info.OrganisationId = records.GetLongOrDefault("ORGANISATIONID");
                info.IsAdmin = Convert.ToInt32(records.GetStringOrDefault("IS_ADMINISTRATOR"));
                info.UserId = Convert.ToString(records.GetDecimalOrDefault("USER_ID"));
                info.LastLogin = records.GetDateTimeOrDefault("PASSWORD_START");
                info.VehicleUnits = records.GetInt32OrDefault("VEHICLE_UNITS");
                info.RoutePlanUnits = records.GetInt32OrDefault("ROUTEPLAN_UNITS_1");
                info.FirstName = records.GetStringOrDefault("FIRST_NAME");
                info.LastName = records.GetStringOrDefault("SUR_NAME");
                info.Email = records.GetStringOrDefault("EMAIL");
                info.NumOfAttempts = records.GetShortOrDefault("ATTEMPTS");
                info.Password = records.GetStringOrDefault("password_key");
                info.IsEnabled = Convert.ToInt32(records.GetStringOrDefault("enabled"));
                short listItems = records.GetInt16OrDefault("MAX_LIST_ITEMS");
                if (listItems != 0) info.MaxListItem = listItems;
                info.OrganisationName = records.GetStringOrDefault("ORGNAME");
                info.AccessToALSAT = Convert.ToInt32(records.GetDecimalOrDefault("ALSAT_ACCESS"));
                 termsAcceptedFlag = records.GetStringOrDefault("IS_TERMS_ACCEPTED");
                info.IsTermsAccepted = String.IsNullOrEmpty(termsAcceptedFlag) ? 0 : int.Parse(termsAcceptedFlag);
                info.SORTCreateJob = records.GetShortOrDefault("CAN_ENTER_APPS") == 1 ? "1" : "0";
                info.SORTAllocateJob = records.GetShortOrDefault("CAN_MANAGE_APPS") == 1 ? "1" : "0";
                info.SORTCanApproveSignVR1 = records.GetShortOrDefault("CAN_APPROVE_VR1S") == 1 ? "1" : "0";
                info.SORTCanAgreeUpto150 = records.GetShortOrDefault("CAN_AGREE_SO_UPTO150") == 1 ? "1" : "0";
                info.SORTCanAgreeAllSO = records.GetShortOrDefault("CAN_AGREE_ALL_SO") == 1 ? "1" : "0";
                info.ContactId = records.GetLongOrDefault("CONTACT_ID");
                info.SortUserId = records.GetLongOrDefault("SORT_USER_ID");
                info.GeoRegion = records.GetGeometryOrNull("GEO_REGION") as sdogeometry;
                info.PasswordStatus = records.GetInt32OrDefault("PASSWORD_EXPIRY");
                info.PasswordUpdatedOn = records.GetDateTimeOrDefault("PASSWORD_START");
                info.SecurityQuestion = records.GetStringOrDefault("SECURITY_QUESTION"); 
                info.SecurityAnswer = records.GetStringOrDefault("SECURITY_ANSWER"); 
                if (info.UserTypeId == 696001 || info.UserTypeId == 696002 || info.UserTypeId == 696007)
                {
                        if(!string.IsNullOrEmpty(records.GetStringOrDefault("LOGGED_IN")))
                            info.LoggedIn = Convert.ToInt32(records.GetStringOrDefault("LOGGED_IN"));
                        else
                            info.LoggedIn = 0;
                 }           
                    
                }
                 

                );
            return info;
        }
        #region
        internal static int SaveTermsAndConditions(string userId)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                 UserSchema.Portal + ".SP_SAVE_TERMS_CONDITIONS",
                 parameter =>
                 {
                     parameter.AddWithValue("P_USER_ID", userId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                 },
                 records =>
                 {
                     result = Convert.ToInt32(records.GetStringOrDefault("ACCEPTED_TERMS"));
                 }
              );
            return result;
        }
        #endregion
        internal static UserInfo GetAnotherUser(string userId)
        {
            var info = new UserInfo();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                info,
                 UserSchema.Portal + ".STP_LOGIN_PKG.SP_LOGINASOTHERUSER",
                parameter =>
                {
                    parameter.AddWithValue("p_UserName", userId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    info.UserName = records.GetStringOrDefault("USERNAME");
                    info.UserTypeId = records.GetInt32OrDefault("USER_TYPE_ID");
                    info.OrganisationId = records.GetLongOrDefault("ORGANISATIONID");
                    info.IsAdmin = Convert.ToInt32(records.GetStringOrDefault("IS_ADMINISTRATOR"));
                    info.UserId = Convert.ToString(records.GetDecimalOrDefault("USER_ID"));
                    info.LastLogin = records.GetDateTimeOrDefault("PASSWORD_START");
                    info.VehicleUnits = records.GetInt32OrDefault("VEHICLE_UNITS");
                    info.RoutePlanUnits = records.GetInt32OrDefault("ROUTEPLAN_UNITS_1");
                    info.FirstName = records.GetStringOrDefault("FIRST_NAME");
                    info.LastName = records.GetStringOrDefault("SUR_NAME");
                    info.Email = records.GetStringOrDefault("EMAIL");
                    info.NumOfAttempts = records.GetShortOrDefault("ATTEMPTS");
                    info.Password = records.GetStringOrDefault("password_key");
                    info.IsEnabled = Convert.ToInt32(records.GetStringOrDefault("enabled"));
                    short listItems = records.GetInt16OrDefault("MAX_LIST_ITEMS");
                    if (listItems != 0) info.MaxListItem = listItems;
                    info.OrganisationName = records.GetStringOrDefault("ORGNAME");
                    info.SortUserId = records.GetLongOrDefault("SORT_USER_ID");
                    info.SORTCreateJob = records.GetShortOrDefault("CAN_ENTER_APPS") == 1 ? "1" : "0";
                    info.SORTAllocateJob = records.GetShortOrDefault("CAN_MANAGE_APPS") == 1 ? "1" : "0";
                    info.SORTCanApproveSignVR1 = records.GetShortOrDefault("CAN_APPROVE_VR1S") == 1 ? "1" : "0";
                    info.SORTCanAgreeUpto150 = records.GetShortOrDefault("CAN_AGREE_SO_UPTO150") == 1 ? "1" : "0";
                    info.SORTCanAgreeAllSO = records.GetShortOrDefault("CAN_AGREE_ALL_SO") == 1 ? "1" : "0";
                    info.ContactId = records.GetLongOrDefault("CONTACT_ID");
                    info.AccessToALSAT = Convert.ToInt32(records["ALSAT_ACCESS"]);
                }
                );
            return info;
        }
        #region GetUserByID      
        internal static List<UserRegistration> GetUserByID(string userTypeId, int userId, int contactId)
        {
            string str = "";
            int count = 0;
            List<UserRegistration> componentGridObj = new List<UserRegistration>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                componentGridObj,
                 UserSchema.Portal + ".STP_LOGIN_PKG.GET_USER_BYID",
                parameter =>
                {
                    parameter.AddWithValue("p_user_id", userId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_CONTACT_ID", contactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    if (userId != 0)
                    {
                        var isAdmin = records.GetStringOrDefault("IS_ADMINISTRATOR");
                        instance.UserTypeName = records.GetStringOrDefault("USER_TYPE_NAME");
                        instance.UserType = records.GetInt32OrDefault("USER_TYPE_ID");
                        instance.UserName = records.GetStringOrDefault("USERNAME");
                        instance.password = records.GetStringOrDefault("PASSWORD_KEY");
                        instance.SecurityQuestion = records.GetStringOrDefault("SECURITY_QUESTION");
                        instance.SecurityAnswer = records.GetStringOrDefault("SECURITY_ANSWER");
                        if (isAdmin == "1")
                            instance.IsAdministrator = true;
                        else
                            instance.IsAdministrator = false;
                        instance.SORTCreateJob = records.GetShortOrDefault("CAN_ENTER_APPS") == 1 ? "1" : "0";
                        instance.SORTAllocateJob = records.GetShortOrDefault("CAN_MANAGE_APPS") == 1 ? "1" : "0";
                        instance.SORTCanApproveSignVR1 = records.GetShortOrDefault("CAN_APPROVE_VR1S") == 1 ? "1" : "0";
                        instance.SORTCanAgreeUpto150 = records.GetShortOrDefault("CAN_AGREE_SO_UPTO150") == 1 ? "1" : "0";
                        instance.SORTCanAgreeAllSO = records.GetShortOrDefault("CAN_AGREE_ALL_SO") == 1 ? "1" : "0";
                        instance.AdminSelectedOrganisationId = records.GetLongOrDefault("organisationid");
                    }
                    else
                    {
                        instance.Selectorg_Id = records.GetLongOrDefault("organisation_id");
                        int cntprefEmail = records.GetInt32OrDefault("primary_communication");
                        int cntprefFax = records.GetInt32OrDefault("supplementary_communication");
                        if (cntprefEmail == 695002)
                        {
                            instance.ContactPref = "1";
                        }
                        if (cntprefFax == 695001)
                        {
                            instance.ContactPref = "2";
                        }
                    }
                    var exte = records.GetStringOrDefault("EXTENSION");
                    str = str + records.GetInt32OrDefault("role_type").ToString() + ",";
                    instance.Roletype = str;
                    count = count + 1;
                    instance.OrgUser = records.GetStringOrDefault("org_name");
                    instance.Title = records.GetStringOrDefault("TITLE");
                    instance.Email = records.GetStringOrDefault("EMAIL");
                    instance.Notes = records.GetStringOrDefault("COMMENTS");
                    if (exte != "" && exte != null)
                    {
                        instance.Extension = Convert.ToInt32(exte);
                    }
                  
                    instance.Country = Convert.ToString(records.GetDecimalOrDefault("COUNTRY_ID"));
                    instance.CountryName = Convert.ToString(records.GetStringOrDefault("COUNTRY"));
                    instance.PostCode = records.GetStringOrDefault("POSTCODE");
                    instance.Fax = records.GetStringOrDefault("FAX");
                    instance.Telephone = records.GetStringOrDefault("PHONENUMBER");
                    instance.Mobile = records.GetStringOrDefault("MOBILE");
                    instance.FirstName = records.GetStringOrDefault("FIRST_NAME");
                    instance.SurName = records.GetStringOrDefault("SUR_NAME");
                    instance.AddressLine1 = records.GetStringOrDefault("ADDRESSLINE_1");
                    instance.AddressLine2 = records.GetStringOrDefault("ADDRESSLINE_2");
                    instance.AddressLine3 = records.GetStringOrDefault("ADDRESSLINE_3");
                    instance.AddressLine4 = records.GetStringOrDefault("ADDRESSLINE_4");
                    instance.ContactId = Convert.ToString(records.GetDecimalOrDefault("Contact_id"));
                    instance.AddressLine5 = records.GetStringOrDefault("ADDRESSLINE_5");
                }
                );
            componentGridObj[0].Roletype = componentGridObj[count - 1].Roletype;
            return componentGridObj;
        }
        #endregion
        #region SearchUserByName
        internal static decimal SearchUserByName(string userName, int type, string mode, string userId)
        {
            decimal result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                 UserSchema.Portal + ".STP_LOGIN_PKG.sp_IsUserExists",
                parameter =>
                {
                    parameter.AddWithValue("p_USERNAME", userName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TYPE", type, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_mode", mode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_UserId", userId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_user_email",null, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_resultset", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = records.GetDecimalOrDefault("COUNT");
                }
                );
            return result;
        }
        #endregion
        #region UpdateRegInfo
        internal static int UpdateRegInfo(UserRegistration regDet, int userTypeId, int userId)
        {
            string isAdmin = "";
            int SORTCreateJob = 0, SORTAllocateJob = 0, SORTCanApproveSignVR1 = 0, SORTCanAgreeUpto150 = 0, SORTCanAgreeAllSO = 0;
            int flag_PW = 0;
            if (regDet.IsResetPW)
                flag_PW = 1;
            if (regDet.IsAdministrator)
            {
                isAdmin = "1";
            }
            else
            {
                isAdmin = "0";
            }
            if (regDet.SecurityQuestion == "1")
            {
                regDet.SecurityQuestion = "699001";
            }
            else if (regDet.SecurityQuestion == "2")
            {
                regDet.SecurityQuestion = "699002";
            }
            else if (regDet.SecurityQuestion == "3")
            {
                regDet.SecurityQuestion = "699003";
            }
            else
            {
                regDet.SecurityQuestion = null;
                regDet.SecurityAnswer = null;
            }
            SORTCreateJob = regDet.SORTCreateJob == "1" ? 1 : 0;
            SORTAllocateJob = regDet.SORTAllocateJob == "1" ? 1 : 0;
            SORTCanApproveSignVR1 = regDet.SORTCanApproveSignVR1 == "1" ? 1 : 0;
            SORTCanAgreeUpto150 = regDet.SORTCanAgreeUpto150 == "1" ? 1 : 0;
            SORTCanAgreeAllSO = regDet.SORTCanAgreeAllSO == "1" ? 1 : 0;
            int output = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                 UserSchema.Portal + ".STP_LOGIN_PKG.EDIT_USER",
                parameter =>
                {
                    parameter.AddWithValue("p_USERID", userId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_USERNAME", regDet.UserName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_Usertype", regDet.UserType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);//regDet.UserType
                    parameter.AddWithValue("p_PASSWORD", regDet.password, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SECURITY_QUESTION_ID", regDet.SecurityQuestion, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SECURITY_ANSWER", regDet.SecurityAnswer, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ISADMINISTRATOR", isAdmin, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_CONTACTID", regDet.ContactId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FIRSTNAME", regDet.FirstName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SURNAME", regDet.SurName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TITLE", regDet.Title, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_1", regDet.AddressLine1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_2", regDet.AddressLine2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_3", regDet.AddressLine3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_4", regDet.AddressLine4, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_5", regDet.AddressLine5, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_POSTCODE", regDet.PostCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COUNTRYID", regDet.CountryId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PHONENUMBER", regDet.Telephone, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_EXTENSION", regDet.Extension, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MOBILE", regDet.Mobile, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FAX", regDet.Fax, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_EMAIL", regDet.Email, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_DELETED", '0', OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);//regDet.IsDeleted
                    if (userTypeId == 696006)
                    {
                        parameter.AddWithValue("p_ORGANISATIONID", regDet.Selectorg_Id, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);//regDet.OrganisationID
                    }
                    else
                    {
                        parameter.AddWithValue("p_ORGANISATIONID", regDet.OrganisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);//regDet.OrganisationID
                    }
                    parameter.AddWithValue("p_COMMENTS", regDet.Notes, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ROLE_TYPE", regDet.Roletype, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_Flags", flag_PW, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SORT_CREATE_JOB", SORTCreateJob, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SORT_ALLOCATE_JOB", SORTAllocateJob, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SORT_CAN_APPROVE_SIGN_VR1", SORTCanApproveSignVR1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SORT_CAN_AGREEUPTO150", SORTCanAgreeUpto150, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SORT_CAN_AGREEALLSO", SORTCanAgreeAllSO, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
            records =>
            {
                output = records.GetInt32("P_AFFECTED_ROWS");
            }
        );
            return output;

        }
        #endregion
        #region SetRegInfo
        internal static UserRegistration SetRegInfo(UserRegistration regDet, int userTypeId)
        {
            UserRegistration userobj = new UserRegistration();
            string isAdmin = "";
            int SORTCreateJob = 0, SORTAllocateJob = 0, SORTCanApproveSignVR1 = 0, SORTCanAgreeUpto150 = 0, SORTCanAgreeAllSO = 0;
            if (regDet.IsAdministrator)
            {
                isAdmin = "1";
            }
            else
            {
                isAdmin = "0";
            }
            if (regDet.UserType == null)
            {
                regDet.UserType = userTypeId;
            }
            SORTCreateJob = regDet.SORTCreateJob == "1" ? 1 : 0;
            SORTAllocateJob = regDet.SORTAllocateJob == "1" ? 1 : 0;
            SORTCanApproveSignVR1 = regDet.SORTCanApproveSignVR1 == "1" ? 1 : 0;
            SORTCanAgreeUpto150 = regDet.SORTCanAgreeUpto150 == "1" ? 1 : 0;
            SORTCanAgreeAllSO = regDet.SORTCanAgreeAllSO == "1" ? 1 : 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                userobj,
                  UserSchema.Portal + ".STP_LOGIN_PKG.INSERT_USER",
                parameter =>
                {
                    parameter.AddWithValue("p_USERNAME", regDet.UserName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_USERTYPE", regDet.UserType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);//regDet.UserType
                    parameter.AddWithValue("p_PASSWORD", regDet.password, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SECURITY_QUESTION_ID", regDet.SecurityQuestion, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SECURITY_ANSWER", regDet.SecurityAnswer, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ISADMINISTRATOR", isAdmin, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FIRSTNAME", regDet.FirstName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SURNAME", regDet.SurName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TITLE", regDet.Title, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_1", regDet.AddressLine1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_2", regDet.AddressLine2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_3", regDet.AddressLine3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_4", regDet.AddressLine4, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_5", regDet.AddressLine5, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_POSTCODE", regDet.PostCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COUNTRYID", regDet.CountryId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PHONENUMBER", regDet.Telephone, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_EXTENSION", regDet.Extension, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MOBILE", regDet.Mobile, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FAX", regDet.Fax, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_EMAIL", regDet.Email, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_DELETED", '0', OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);//regDet.IsDeleted
                    parameter.AddWithValue("p_NOTE", regDet.Notes, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    if (userTypeId == 696006)
                    {
                        parameter.AddWithValue("p_ORGANISATIONID", regDet.Selectorg_Id, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);//regDet.OrganisationID
                    }
                    else
                    {
                        parameter.AddWithValue("p_ORGANISATIONID", regDet.OrganisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);//regDet.OrganisationID
                    }
                    parameter.AddWithValue("P_ROLE_TYPE", regDet.Roletype, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_PRIMARYCOMM", regDet.ContactPref, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SUPPLEMENTRYCOMM", regDet.ContactPref, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SORT_CREATE_JOB", SORTCreateJob, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SORT_ALLOCATE_JOB", SORTAllocateJob, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SORT_CAN_APPROVE_SIGN_VR1", SORTCanApproveSignVR1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SORT_CAN_AGREEUPTO150", SORTCanAgreeUpto150, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SORT_CAN_AGREEALLSO", SORTCanAgreeAllSO, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.UserName = regDet.UserName;
                }
            );
            return userobj;
        }
        #endregion
        #region DeleteUser
        public static int DeleteUser(int userId, int deleteVal)
        {
            int affectedRows = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                 UserSchema.Portal + ".STP_LOGIN_PKG.DELETEUSER",
                parameter =>
                {
                    parameter.AddWithValue("P_USER_ID", userId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_deleteVal", deleteVal, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECTEDROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    affectedRows = record.GetInt32("P_AFFECTEDROWS");
                }
            );
            return affectedRows;
        }
        #endregion
        #region DeleteContact
        public static int DeleteContact(int contactId, int deleteVal)
        {
            int affectedRows = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                 UserSchema.Portal + ".STP_LOGIN_PKG.DELETECONTUSER",
                parameter =>
                {
                    parameter.AddWithValue("P_CONTACT_ID", contactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_deleteVal", deleteVal, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECTEDROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    affectedRows = record.GetInt32("P_AFFECTEDROWS");
                }
            );
            return affectedRows;
        }
        #endregion
        #region GetUserListInfo
        internal static List<GetUserList> GetUserListInfo(string userTypeID, string organisationId, int pageNumber, int pageSize,UserContactSearchItems userContactSearchItems, int sortOrder=1, int presetFilter=1)
        {
            List<GetUserList> componentGridObj = new List<GetUserList>();             
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                componentGridObj,
                 UserSchema.Portal + ".STP_LOGIN_PKG.ListUser",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_ID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_UserTypeID", userTypeID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_userDisableFlag", userContactSearchItems.DisabledUsersFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_Contact_Flag", userContactSearchItems.ShowContactsFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ContactDisableFlag", userContactSearchItems.DisabledContactsFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_UserName", userContactSearchItems.UserName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FirstName", userContactSearchItems.FirstName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SurName", userContactSearchItems.SurName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OrganisationName", userContactSearchItems.OrganisationName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OrganisationCode", userContactSearchItems.OrganisationCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PAGENUMBER", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PAGESIZE", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("SORT_ORDER", sortOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("PRESET_FILTER", presetFilter, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    if (userContactSearchItems.ShowContacts || userContactSearchItems.DisabledContacts)
                    {
                        if (userContactSearchItems.UserName == null|| userContactSearchItems.UserName =="")
                            instance.ContactId = Convert.ToString(records.GetDecimalOrDefault("CONTACT_ID"));
                        else
                        {
                            instance.UserID = Convert.ToString(records.GetDecimalOrDefault("USER_ID"));
                            instance.UserName = records.GetStringOrDefault("USERNAME");
                            instance.UserTypeId = Convert.ToString(records.GetInt32OrDefault("USER_TYPE_ID"));
                        }
                    }
                    else
                    {
                        instance.UserID = Convert.ToString(records.GetDecimalOrDefault("USER_ID"));
                        instance.UserName = records.GetStringOrDefault("USERNAME");
                        instance.UserTypeId = Convert.ToString(records.GetInt32OrDefault("USER_TYPE_ID"));
                    }
                    instance.FirstName = records.GetStringOrDefault("FIRST_NAME");
                    instance.SurName = records.GetStringOrDefault("SUR_NAME");
                    instance.PhoneNumber = records.GetStringOrDefault("PHONENUMBER");
                    instance.OrgName = records.GetStringOrDefault("ORGNAME");
                    instance.Email = records.GetStringOrDefault("email");
                    instance.TotalRecordCount = (long)Convert.ToInt32(records["TOTALRECORDCOUNT"]);
                    instance.OrganisationCode = records.GetStringOrDefault("orgcode");
                });
            return componentGridObj;
        }
        #endregion
        #region GetSORTUserListInfo
        internal static List<GetUserList> GetSORTUserListInfo(string userTypeId)
        {
            List<GetUserList> objSortUser = new List<GetUserList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               objSortUser,
               UserSchema.Sort + ".LIST_SORT_USER",
               parameter =>
               {
                   parameter.AddWithValue("p_USERTYPE_ID", userTypeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_CHCEKING_TYPE", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("SORT_ORDER", 1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("PRESET_FILTER", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                (records, instance) =>
                {
                    instance.UserID = Convert.ToString(records.GetLongOrDefault("user_id"));
                    instance.FirstName = records.GetStringOrDefault("SORT_USER");
                }
                );
            return objSortUser;
        }
        #endregion
        #region DelegationFailureAlertForDisabling
        internal static bool DelegationFailureAlertForDisabling(int userId)
        {
            bool result = true;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
            result,
             UserSchema.Portal + ".STP_ROAD_DELEGATION.SP_RD_FETCH_USER_STATUS",
            parameter =>
            {
                parameter.AddWithValue("P_USER_ID", userId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
            record =>
            {
                result = record.GetStringOrDefault("STATUS") != "FALSE";
            });
            return result;
        }
        #endregion
        #region SetContactRegInfo
        internal static UserRegistration SetContactRegInfo(UserRegistration regDet, int userTypeId)
        {
            UserRegistration contactuser = new UserRegistration();
            string isAdmin = "";
            int SORTCreateJob = 0, SORTAllocateJob = 0, SORTCanApproveSignVR1 = 0, SORTCanAgreeUpto150 = 0, SORTCanAgreeAllSO = 0;
            if (regDet.IsAdministrator)
            {
                isAdmin = "1";
            }
            else
            {
                isAdmin = "0";
            }
            SORTCreateJob = regDet.SORTCreateJob == "1" ? 1 : 0;
            SORTAllocateJob = regDet.SORTAllocateJob == "1" ? 1 : 0;
            SORTCanApproveSignVR1 = regDet.SORTCanApproveSignVR1 == "1" ? 1 : 0;
            SORTCanAgreeUpto150 = regDet.SORTCanAgreeUpto150 == "1" ? 1 : 0;
            SORTCanAgreeAllSO = regDet.SORTCanAgreeAllSO == "1" ? 1 : 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                contactuser,
                 UserSchema.Portal + ".STP_LOGIN_PKG.INSERT_USER",
                parameter =>
                {
                    parameter.AddWithValue("p_USERNAME", regDet.UserName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_USERTYPE", regDet.UserType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);//regDet.UserType
                    parameter.AddWithValue("p_PASSWORD", regDet.password, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SECURITY_QUESTION_ID", null, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SECURITY_ANSWER", null, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ISADMINISTRATOR", isAdmin, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FIRSTNAME", regDet.FirstName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SURNAME", regDet.SurName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TITLE", regDet.Title, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_1", regDet.AddressLine1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_2", regDet.AddressLine2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_3", regDet.AddressLine3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_4", regDet.AddressLine4, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_5", regDet.AddressLine5, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_POSTCODE", regDet.PostCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COUNTRYID", regDet.CountryId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PHONENUMBER", regDet.Telephone, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_EXTENSION", regDet.Extension, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MOBILE", regDet.Mobile, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FAX", regDet.Fax, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_EMAIL", regDet.Email, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_DELETED", '0', OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);//regDet.IsDeleted
                    parameter.AddWithValue("p_NOTE", regDet.Notes, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    if (userTypeId == 696006)
                    {
                        parameter.AddWithValue("p_ORGANISATIONID", regDet.Selectorg_Id, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);//regDet.OrganisationID
                    }
                    else
                    {
                        parameter.AddWithValue("p_ORGANISATIONID", regDet.OrganisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);//regDet.OrganisationID
                    }
                    parameter.AddWithValue("P_ROLE_TYPE", regDet.Roletype, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_PRIMARYCOMM", regDet.ContactPref, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SUPPLEMENTRYCOMM", regDet.ContactPref, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SORT_CREATE_JOB", SORTCreateJob, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SORT_ALLOCATE_JOB", SORTAllocateJob, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SORT_CAN_APPROVE_SIGN_VR1", SORTCanApproveSignVR1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SORT_CAN_AGREEUPTO150", SORTCanAgreeUpto150, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SORT_CAN_AGREEALLSO", SORTCanAgreeAllSO, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.FirstName = regDet.FirstName;
                }
            );
            return contactuser;
        }
        #endregion
        #region UpdateContact
        internal static int UpdateContact(UserRegistration regDet, int userTypeId, int contactId)
        {
            int output = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                 UserSchema.Portal + ".STP_LOGIN_PKG.EDIT_CONTACT_USER",
                parameter =>
                {
                    parameter.AddWithValue("p_CONTACTID", regDet.ContactId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FIRSTNAME", regDet.FirstName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SURNAME", regDet.SurName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TITLE", regDet.Title, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_1", regDet.AddressLine1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_2", regDet.AddressLine2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_3", regDet.AddressLine3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_4", regDet.AddressLine4, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_5", regDet.AddressLine5, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_POSTCODE", regDet.PostCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COUNTRYID", regDet.CountryId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PHONENUMBER", regDet.Telephone, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_EXTENSION", regDet.Extension, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MOBILE", regDet.Mobile, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FAX", regDet.Fax, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_EMAIL", regDet.Email, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_DELETED", '0', OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);//regDet.IsDeleted
                    if (userTypeId == 696006)
                    {
                        parameter.AddWithValue("p_ORGANISATIONID", regDet.Selectorg_Id, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);//regDet.OrganisationID
                    }
                    else
                    {
                        parameter.AddWithValue("p_ORGANISATIONID", regDet.OrganisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);//regDet.OrganisationID
                    }
                    parameter.AddWithValue("p_COMMENTS", regDet.Notes, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ROLE_TYPE", regDet.Roletype, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_PRIMARYCOMM", regDet.ContactPref, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SUPPLEMENTRYCOMM", regDet.ContactPref, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
            records =>
            {
                output = records.GetInt32("P_AFFECTED_ROWS");
            }
            );
            return output;
        }
        #endregion
        #region GetSearchUser
        public static bool GetSearchUser(string Value, string Type)
        {
            bool result = false;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                 UserSchema.Portal + ".STP_LOGIN_PKG.DELETEUSER",
                parameter =>
                {
                    parameter.AddWithValue("P_USER_ID", Value, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USER_ID", Type, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                },
                record =>
                {
                    result = true;
                }
            );
            return result;
        }
        #endregion
        #region GetCountryList
        internal static List<UserRegistration> GetListOfCountries()
        {
            var getList = new List<UserRegistration>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                getList,
               UserSchema.Portal + ".SP_GET_COUNTRY_LIST",
                parameter =>
                {
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.CountryId = records.GetInt32OrDefault("CODE").ToString();
                    instance.Country = records.GetStringOrDefault("COUNTRY_NAME");
                });
            return getList;
        }
        #endregion
        #region ViewOrganisationByID
        public static List<ViewOrganizationByID> ViewOrganisationByID(int orgId)
        {
            List<ViewOrganizationByID> OrgTypeListObj = new List<ViewOrganizationByID>();
            int IIsNENsReceive = 0;
            int AccessToALSAT = 0;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                OrgTypeListObj,
                 UserSchema.Portal + ".STP_LOGIN_PKG.GetOrganisationByID",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_ID", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.OrgId = records.GetLongOrDefault("ORGANISATION_ID");
                    instance.OrgName = records.GetStringOrDefault("ORGNAME");
                    instance.OrgType = Convert.ToString(records.GetInt32OrDefault("ORGTYPE"));
                    instance.OrgCode = records.GetStringOrDefault("ORG_CODE");
                    instance.LicenseNR = records.GetStringOrDefault("LICENSE_NR");
                    instance.AddressLine1 = records.GetStringOrDefault("ADDRESSLINE_1");
                    instance.AddressLine2 = records.GetStringOrDefault("ADDRESSLINE_2");
                    instance.AddressLine3 = records.GetStringOrDefault("ADDRESSLINE_3");
                    instance.AddressLine4 = records.GetStringOrDefault("ADDRESSLINE_4");
                    instance.AddressLine5 = records.GetStringOrDefault("ADDRESSLINE_5");
                    instance.PostCode = records.GetStringOrDefault("POSTCODE");
                    instance.AuthenticationKey = records.GetStringOrDefault("AUTHENTICATION_KEY");
                    IIsNENsReceive = Convert.ToInt32(records["RECEIVE_NEN"]);
                    AccessToALSAT = Convert.ToInt32(records["ALSAT_ACCESS"]);
                    if (IIsNENsReceive == 1)
                        instance.IsNENsReceive = true;
                    else
                        instance.IsNENsReceive = false;

                    if (AccessToALSAT == 1)
                        instance.AccessToALSAT = true;
                    else
                        instance.AccessToALSAT = false;
                    instance.CountryId = Convert.ToString(records.GetInt32OrDefault("COUNTRY_ID"));
                    instance.Phone = records.GetStringOrDefault("PHONENUMBER");
                    instance.Web = records.GetStringOrDefault("ORG_URL");
                }
                );

            return OrgTypeListObj;
        }
        #endregion
        #region GetUserType
        internal static List<UserRegistration> GetUserType()
        {
            var getList = new List<UserRegistration>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                getList,
               UserSchema.Portal + ".STP_LOGIN_PKG.GetAllUserTypes",
                parameter =>
                {
                    parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.UserType = records.GetInt32OrDefault("USER_TYPE_ID");
                    instance.Name = records.GetStringOrDefault("USER_TYPE_NAME");
                }
                );
            return getList;
        }
        #endregion
       
        #region Save organisation
        internal static Organization SaveOrganisation(Organization orgDet)
        {
            int IsreceiveNEN = 0;
            if (orgDet.IsNENsReceive)
                IsreceiveNEN = 1;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                orgDet,
               UserSchema.Portal + ".STP_LOGIN_PKG.InsertOrganisation",
                parameter =>
                {
                    parameter.AddWithValue("p_ORGNAME", orgDet.OrgName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGTYPE", orgDet.OrgType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORG_CODE", orgDet.OrgCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LICENSE_NR", orgDet.Licence_NR, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_1", orgDet.AddressLine1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_2", orgDet.AddressLine2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_3", orgDet.AddressLine3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_4", orgDet.AddressLine4, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_5", orgDet.AddressLine5, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_POSTCODE", orgDet.PostCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COUNTRY_ID", orgDet.CountryID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PHONENUMBER", orgDet.Phone, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORG_URL", orgDet.Web, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AUTHENTICATION_KEY", orgDet.AuthenticationKey, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    orgDet.OrgID = record.GetDecimalOrDefault("ORG_ID");
                }
            );
            // NEN
            if (IsreceiveNEN == 1)
                SafeProcedure.DBProvider.Oracle.Execute(
               UserSchema.Portal + ".STP_NEN_NOTIFICATION.EditOrganisation",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_ID", orgDet.OrgID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RECEIVE_NEN", IsreceiveNEN, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);//Added new field in Organization table name as 'IsNESReceive' to know weather the org has right to receive NEN notification or not 
                    }
            );
            return orgDet;
        }
        #endregion
        #region OrganisationType
        internal static List<OrganizationTypeList> GetOrganisationTypeList()
        {
            List<OrganizationTypeList> OrgTypeListObj = new List<OrganizationTypeList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                OrgTypeListObj,
               UserSchema.Portal + ".STP_LOGIN_PKG.GetAllOrganisationType",
                parameter =>
                {
                    parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.OrgTypeCode = records.GetInt32OrDefault("CODE");
                    instance.OrgTypeName = records.GetStringOrDefault("NAME");
                }
                );
            return OrgTypeListObj;
        }
        #endregion
        #region SetUserPreference   
        internal static bool SetUserPreference(UserPreferences userPreference, int userId, string emailUpdate, string faxNumber)
        {
            UserPreferencesDetails userPreferencesDetails = new UserPreferencesDetails();
            bool result = false;
            int isEnableFlag = 0;
            int IsXMLAttached = 0;
            if (userPreference.IsEnable)
            {
                isEnableFlag = 1;
            }
            if (userPreference.IsXMLAttached)
            {
                IsXMLAttached = 1;
            }            
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                UserSchema.Portal + ".SP_UPDATE_USERPREFERENCE", parameter =>
               {
                   parameter.AddWithValue("P_USER_ID", userId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_VEHICLE_UNITS", userPreference.VehicleUnits, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_ROUTEPLAN_UNITS", userPreference.RouteplanUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_ENABLE_WORKSPACES", isEnableFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_MAX_LIST", userPreference.MaxListItems, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_COMM_METHOD", userPreference.CommonMethod, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_EMAIL", emailUpdate, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_FAXNUMBER", faxNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_ENABLE_IsXMLAttached", IsXMLAttached, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);                
                   parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
            records =>
            {
                //userPreferencesDetails.AFFECTED_USERS = records.GetLong("USER_COUNT");
                //userPreferencesDetails.AFFECTED_CONTACTS = records.GetLong("CTACT_COUNT");
                result = true;
            });
            return result;
            
        }
        #endregion

        public static List<GetHaulContactByOrgID> GetHaulierContactByOrgID(int organisationId)
        {
            List<GetHaulContactByOrgID> HaulContactByOrgIDObj = new List<GetHaulContactByOrgID>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                HaulContactByOrgIDObj,
                 UserSchema.Portal + ".GET_HAULIERCONTACT_ORGID",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.ContactName = records.GetStringOrDefault("CONTACT_NAME");
                    instance.AddressLine_1 = records.GetStringOrDefault("addressline_1");
                    instance.AddressLine_2 = records.GetStringOrDefault("addressline_2");
                    instance.AddressLine_3 = records.GetStringOrDefault("addressline_3");
                    instance.AddressLine_4 = records.GetStringOrDefault("addressline_4");
                    instance.AddressLine_5 = records.GetStringOrDefault("addressline_5");
                    instance.CountryID = Convert.ToString(records.GetDecimalOrDefault("country_id"));
                    instance.PostCode = records.GetStringOrDefault("postcode");
                    instance.Phone = records.GetStringOrDefault("phonenumber");
                    instance.Fax = records.GetStringOrDefault("FAX");
                    instance.Email = records.GetStringOrDefault("EMAIL");
                    instance.contactId = records.GetDecimalOrDefault("contact_id");
                }
                );
            return HaulContactByOrgIDObj;
        }

        public static List<UserDetailsModel> GetUsersByOrgID(int organisationId,int UserTypeId)
        {
            List<UserDetailsModel> HaulContactByOrgIDObj = new List<UserDetailsModel>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                HaulContactByOrgIDObj,
                 UserSchema.Portal + ".SP_GET_USERS_BY_ORG_ID",
                parameter =>
                {
                    parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USERTYPE_ID", UserTypeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.Name = records.GetStringOrDefault("NAME");
                    instance.UserId = Convert.ToInt32(records.GetDecimalOrDefault("USER_ID"));
                    instance.UserName = records.GetStringOrDefault("USERNAME");
                    instance.UserTypeId = records.GetInt32OrDefault("USER_TYPE_ID");
                }
                );
            return HaulContactByOrgIDObj;
        }

        #region GetUserPreferencesById
        public static UserPreferences GetUserPreferencesById(int userId)
        {
            UserPreferences UserPreferences1 = new UserPreferences();
            string i,j,k;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                UserPreferences1,
                UserSchema.Portal + ".SP_GET_USERPREFERENCE",
                parameter =>
                {
                    parameter.AddWithValue("userId", userId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {

                    instance.VehicleUnits = records.GetInt32OrDefault("vehicle_units");
                    instance.RouteplanUnit = records.GetInt32OrDefault("routeplan_units_1");
                    i = Convert.ToString(records.GetInt16OrDefault("enable_workspaces"));

                    if (i == "1")
                    {
                        instance.IsEnable = true;
                    }
                    else
                    {
                        instance.IsEnable = false;
                    }
                    i = Convert.ToString(records.GetInt16OrDefault("IS_XML_ATTACHED"));
                    if (i == "1")
                    {
                        instance.IsXMLAttached = true;
                    }
                    else
                    {
                        instance.IsXMLAttached = false;
                    }
                    instance.MaxListItems = records.GetInt16OrDefault("max_list_items");
                    instance.CommonMethod = records.GetLongOrDefault("communication_method");
                    instance.EmailText = records.GetStringOrDefault("EMAIL");
                    instance.FaxNumber = records.GetStringOrDefault("FAX_NO"); 
                }
                );
            return UserPreferences1;
        }
        #endregion

        public static int GetAutoResponse(int organisationId)
        {
            int count = 1;            
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                     count,
                     "STP_USER_PREFERENCES" + ".SP_GET_AUTO_RESPONSE_STATUS",
                     parameter =>
                     {
                         parameter.AddWithValue("P_ORGANISATION_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                     },
                     records =>
                     {
                         count = records.GetInt16OrDefault("ENABLE_AUTO_RESPONSE");
                     });
            return count;
        }

        //Get Replay Mail PDF
        public static string GetReplyMailPDF(long OrganisationId, string userschema = "STP_USER_PREFERENCES")
        {

                string result = null;
                //decimal iscompressesfile = 0;
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    result,
                    userschema + ".SP_GET_REPLY_MAIL_PDF",
                    parameter =>
                    {
                        parameter.AddWithValue("P_ORGANISATION_ID", OrganisationId, OracleDbType.Long, ParameterDirectionWrap.Input);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        records =>
                        {
                            result = records.GetStringOrDefault("REPLY_MAIL_PDF");
                            //iscompressesfile = records.GetDecimalOrDefault("ISCOMPRESSED");
                        }
              );
                return result;

        }
        public static Organization EditOrganisation(Organization orgDetails)
        {
            
            int orgId = Decimal.ToInt32(orgDetails.OrgID);
            string organisationId = orgId.ToString();
            int countryId = Convert.ToInt32(orgDetails.CountryID);
            int organisationType = Convert.ToInt32(orgDetails.OrgType);
            int IsreceiveNEN = 0;
            int AccessToALSAT = 0;
            if (orgDetails.IsNENsReceive == true)
                IsreceiveNEN = 1;
            if (orgDetails.AccessToALSAT == true)
                AccessToALSAT = 1;
            SafeProcedure.DBProvider.Oracle.Execute(
               UserSchema.Portal + ".STP_LOGIN_PKG.EditOrganisation",
                parameter =>
                {
                    
                    parameter.AddWithValue("p_ORG_ID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGNAME", orgDetails.OrgName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGTYPE", organisationType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORG_CODE", orgDetails.OrgCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LICENSE_NR", orgDetails.Licence_NR, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_1", orgDetails.AddressLine1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_2", orgDetails.AddressLine2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_3", orgDetails.AddressLine3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_4", orgDetails.AddressLine4, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_5", orgDetails.AddressLine5, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_POSTCODE", orgDetails.PostCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COUNTRY_ID", countryId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PHONENUMBER", orgDetails.Phone, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORG_URL", orgDetails.Web, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AUTHENTICATION_KEY", orgDetails.AuthenticationKey, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RECEIVE_NEN", IsreceiveNEN, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);            
                    parameter.AddWithValue("P_Access_To_ALSAT", AccessToALSAT, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);


                }
            );
            if (IsreceiveNEN == 1)
                SafeProcedure.DBProvider.Oracle.Execute(
               UserSchema.Portal + ".STP_NEN_NOTIFICATION.EditOrganisation",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_ID", orgDetails.OrgID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RECEIVE_NEN", IsreceiveNEN, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);//Added new field in Organization table name as 'IsNESReceive' to know weather the org has right to receive NEN notification or not 

                    }
            );
            return orgDetails;
        }

        public static ContactModel GetContactInformation(long contactId)
        {
            ContactModel contactDetail = new ContactModel();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                contactDetail,
                UserSchema.Portal + ".GET_CONTACT_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("p_ContactId", contactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       //instance.ContactId = contactId;
                       instance.Email = records.GetStringOrDefault("Email");
                       instance.Fax = records.GetStringOrDefault("Fax");
                       instance.FullName = records.GetStringOrDefault("FIRST_NAME");
                       instance.Organisation = records.GetStringOrDefault("ORGNAME");
                       instance.OrganisationId = (int)records.GetLongOrDefault("ORGANISATION_ID");
                       instance.PhoneNumber = records.GetStringOrDefault("PHONENUMBER");
                   }
            );
            return contactDetail;
        }

        public static int GetPasswords(string strpassword)
        {
            int iCount = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                iCount,
                UserSchema.Portal + ".SP_Password_Exists",
                parameter =>
                {
                    parameter.AddWithValue("p_Password", strpassword, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       iCount = records.GetDataTypeName("CNT") == "Int16" ? records.GetInt16OrDefault("CNT") : (int)records.GetDecimalOrDefault("CNT");
                   }
            );
            return iCount;
        }

        internal static List<GetUserList> SearchUserCriteria(string userTypeId, string organisationId, UserContactSearchItems userContactSearchItems)
        {
            List<GetUserList> componentGridObj = new List<GetUserList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                componentGridObj,
                 UserSchema.Portal + ".STP_LOGIN_PKG.Search_User_Criteria",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_ID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_UserTypeID", userTypeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_userDisableFlag", userContactSearchItems.DisabledUsersFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_Contact_Flag", userContactSearchItems.ShowContactsFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ContactDisableFlag", userContactSearchItems.ShowContactsFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_UserName", userContactSearchItems.UserName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FirstName", userContactSearchItems.FirstName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SurName", userContactSearchItems.SurName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OrganisationName", userContactSearchItems.OrganisationName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OrganisationCode", userContactSearchItems.OrganisationCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                     
                        instance.UserName = records.GetStringOrDefault("USERNAME"); 
                    
                    instance.FirstName = records.GetStringOrDefault("FIRST_NAME");
                    instance.SurName = records.GetStringOrDefault("SUR_NAME");                   
                    instance.OrgName = records.GetStringOrDefault("ORGNAME");                   
                    instance.OrganisationCode = records.GetStringOrDefault("org_code");
                }
                );
            return componentGridObj;
        }
        internal static List<GetUserList> GetUserbyOrgId(string userTypeId, string organisationId)
        {
            long orgId = Convert.ToInt64(organisationId);
            
            int UsertypeID = Convert.ToInt32(userTypeId);
            List<GetUserList> componentGridObj = new List<GetUserList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                componentGridObj,
                 UserSchema.Portal + ".STP_LOGIN_PKG.ListAllUsers ",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_ID", orgId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_UserTypeID", UsertypeID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   
                   parameter.AddWithValue("p_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {

                    instance.UserName = records.GetStringOrDefault("USERNAME");
                    instance.UserID = Convert.ToString(records.GetDecimalOrDefault("USER_ID"));
                    instance.FirstName = records.GetStringOrDefault("FIRST_NAME");
                    instance.SurName = records.GetStringOrDefault("SUR_NAME");
                    instance.FullName = records.GetStringOrDefault("FULL_NAME");
                    instance.OrgName = records.GetStringOrDefault("ORGNAME");
                    instance.OrganisationCode = records.GetStringOrDefault("org_code");
                    
                }
                );
            return componentGridObj;
        }


    }
}