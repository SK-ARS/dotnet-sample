using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using STP.Domain;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.LoggingAndReporting;
using System.Globalization;
using STP.Domain.NonESDAL;

namespace STP.MovementsAndNotifications.Persistance
{
    public static class NENNotificationDAO
    {
        #region Get NEN ID
        public static long Get_SP_NEN_ID(int notificationNo)
        {
            long NENId = 0;
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    NENId,
                    UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_GET_NEN_ID",
                    parameter =>
                    {
                        parameter.AddWithValue("P_NOTIFICATION_ID", notificationNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    records =>
                    {
                        NENId = Convert.ToInt64(records["V_NENID"]);
                    }
                    );         
            return NENId;
        }
        #endregion

        #region Save Notification AuditLog
        public static long SaveNotifAuditLog(AuditLogIdentifiersParams objAuditLogIdentifiersParams)
        {
            long result = 0;
            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                        result,
                        UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_NEN_INSERT_ESDAL_NOTIF_LOG",
                        parameter =>
                        {
                            parameter.AddWithValue("P_ESDAL_REF_NO", objAuditLogIdentifiersParams.AuditLogType.ESDALNotificationNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_INBOX_ITEM_ID", objAuditLogIdentifiersParams.AuditLogType.InboxItemId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_LOG_MSG", objAuditLogIdentifiersParams.LogMsg, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_ORG_ID", objAuditLogIdentifiersParams.OrganisationId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                        },
                         (records, instance) =>
                         {
                             result = Convert.ToInt64(records["AUDIT_ID"]);
                         }
                );
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{ConfigurationManager.AppSettings["Instance"] } - NENNotificationDAO/SaveNotifAuditLog, Exception: {ex}");
                throw;
            }
            return result;
        }
        #endregion

        #region Get NEN Haulier
        public static List<NeHaulierList> GetNeHaulier(int pageNo, int pageSize, string searchString, int isVal = 0, int presetFilter = 1, int sortorder = 1)  
        {
            List<NeHaulierList> NehaulList = new List<NeHaulierList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    NehaulList,
                UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_GET_NE_HAULIERLIST",
                    parameter =>
                    {
                        parameter.AddWithValue("P_ISVAL", isVal, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_PAGENUMBER", pageNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_PAGESIZE", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_SEARCH_TEXT", searchString, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_SORT_ORDER", sortorder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_PRESET_FILTER", presetFilter, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    (records, instance) =>
                    {
                        instance.HaulierName = records.GetStringOrDefault("HAULIER_NAME");
                        instance.OrgniseName = records.GetStringOrDefault("ORGANISATION_NAME");
                        instance.AuthenticationKey = records.GetStringOrDefault("AUTHENTICATION_KEY");
                        instance.NeLimit = (long)records.GetDecimalOrDefault("NEN_LIMIT");
                        instance.NeAuthKey = Convert.ToInt32(records["KEY_ID"]);
                        instance.TotalCount = (long)Convert.ToInt32(records["TOTALRECORDCOUNT"]);
                    }
                );
            return NehaulList;
        }
        #endregion
        #region Edit Ne User
        public static List<NeHaulierList> EditNeUser(long authKeyId)   
        {
            List<NeHaulierList> NehaulList = new List<NeHaulierList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    NehaulList,
                UserSchema.Portal+".STP_NEN_NOTIFICATION.SP_GET_NE_HAULIER",
                    parameter =>
                    {
                        parameter.AddWithValue("P_AUTH_ID", authKeyId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    (records, instance) =>
                    {
                        instance.HaulierName = records.GetStringOrDefault("HAULIER_NAME");
                        instance.OrgniseName = records.GetStringOrDefault("ORGANISATION_NAME");
                        instance.AuthenticationKey = records.GetStringOrDefault("AUTHENTICATION_KEY");
                        instance.NeLimit = (long)records.GetDecimalOrDefault("NEN_LIMIT");
                        instance.NeAuthKey = Convert.ToInt32(records["KEY_ID"]);
                    }
                );
            return NehaulList;   
        }
        #endregion

        #region Save Ne User
        public static int SaveNeuse(NeHaulierParams objNeHaulierParams)
        {
            int flag = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                flag,
                UserSchema.Portal+".STP_NEN_NOTIFICATION.SP_SAVE_NE_USER",
                    parameter =>
                    {
                        parameter.AddWithValue("P_HAUL_NAME", objNeHaulierParams.HaulierName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_AUTH_KEY", objNeHaulierParams.AuthenticationKey, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ORG_NAME", objNeHaulierParams.OrganisationName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_NE_LIMIT", objNeHaulierParams.NeLimit, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_KEY_ID", objNeHaulierParams.KeyId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    (records, instance) =>
                    {
                        flag = Convert.ToInt32(records.GetDecimalOrDefault("V_RES"));
                    }
                );
            return flag;          
        }

        

        #endregion

        #region Enable/Disable User
        public static int EnableUser(string authKey, long keyId)
        {
            int flag = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                flag,
                UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_ENABLE_USER",
                    parameter =>
                    {
                        parameter.AddWithValue("P_AUTH_KEY", authKey, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_KEY_ID", keyId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    (records, instance) =>
                    {
                        flag = Convert.ToInt32(records.GetDecimalOrDefault("V_RES"));
                    }
                );
            return flag;          
        }
        #endregion

        #region Haulier Validation
        public static int HaulierValid(string haulierName, string organisationName)
        {
            int flag = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                flag,
                UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_VALID_HAULIER",
                    parameter =>
                    {
                        parameter.AddWithValue("P_HAUL_NAME", haulierName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ORG_NAME", organisationName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    (records, instance) =>
                    {
                        flag = Convert.ToInt32(records.GetDecimalOrDefault("V_RES"));
                    }
                );
            return flag;        
        }
        #endregion

        #region GetNENRouteList
        public static List<NENUpdateRouteDet> GetNENRouteList(long NENinboxId, int organisationId)
        {
            List<NENUpdateRouteDet> Route_List = new List<NENUpdateRouteDet>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                  Route_List,
                UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_GET_NEN_ROUTE_DET",
                  parameter =>
                  {
                      parameter.AddWithValue("P_INBOX_ID", NENinboxId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("P_RESULSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                  },
                  (records, instance) =>
                  {
                      instance.RouteId = records.GetLongOrDefault("ROUTE_PART_ID");
                      instance.RouteName = records.GetStringOrDefault("PART_NAME");
                      instance.PartNo = records.GetInt16OrDefault("ROUTE_PART_NO");
                      instance.FromAddress = records.GetStringOrDefault("FROM_ADDRESS");
                      instance.ToAddress = records.GetStringOrDefault("TO_ADDRESS");
                  }
             );
            return Route_List;
        }
        #endregion

       

       
        public static List<OrganisationUser> GetOrgUserList(long organisationId, int SOA_UserTypeID, long inBoxId = 0, long NEN_ID = 0)
        {
            List<OrganisationUser> OrgUserList = new List<OrganisationUser>();
          

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    OrgUserList,
                      UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_GET_USERS_OF_ORG",
                    parameter =>
                    {
                        parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_USER_TYPE_ID", SOA_UserTypeID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_INBOX_ITEAM_ID", inBoxId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_NEN_ID", NEN_ID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.OrganisationUserId = Convert.ToInt32(records.GetDecimalOrDefault("USER_ID"));
                            instance.OrganisationUserName = records.GetStringOrDefault("USERNAME");
                            instance.ScrutinyUserId = Convert.ToInt32(records.GetDecimalOrDefault("SCRUTINY_USER"));
                        }
                );             
           
            return OrgUserList;
        }

        public static bool SAVENENUSERFORSCRUTINY(MovementModel movement)
        {
            bool result = false;
            try
            {
               
                int flag = 0;
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    result,
                    UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_INSERT_USERID_FOR_SCRUTINY",
                    parameter =>
                    {
                        parameter.AddWithValue("P_INBOX_ID", movement.InboxId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_NEN_ID", movement.NenId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_NOTIF_ID", movement.NotificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_USER_ID", movement.UserId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_LOGUSER_ID", movement.LoginUserId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RTSTATUS_ID", movement.RouteStatus, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_NEN_PROCESS", movement.NenProcess, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ORG_ID", movement.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    records =>
                    {
                        flag = Convert.ToInt32(records.GetDecimalOrDefault("V_FLAG"));
                    }
                    );
                if (flag > 0)
                    result = true;
               
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotificationDAO/SP_SAVE_NEN_USER_FOR_SCRUTINY , Exception: ", ex);
            }

            return result;
        }

        #region GetNENSOAReportHistory
        public static List<NENSOAReportModel> GetNENSOAReportHistory(Int32 month, Int32 year, long organisationId)
        {
                string mon = month < 10 ? "0" + Convert.ToString(month) : Convert.ToString(month);
                string yer = Convert.ToString(year);
                List<NENSOAReportModel> NENSOAReportList = new List<NENSOAReportModel>();

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    NENSOAReportList,
                    UserSchema.Portal + ".SP_NEN_SOA_REPORT",
                    parameter =>
                    {
                        parameter.AddWithValue("P_MONTH", mon, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_YEAR", yer, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.ReportRecieved = records["RECEIEVED"] != DBNull.Value ? Convert.ToInt32(records["RECEIEVED"]) : 0;
                            instance.ReportAccepted = records["ACCEPTED"] != DBNull.Value ? Convert.ToInt32(records["ACCEPTED"]) : 0;
                            instance.ReportRejected = records["REJECTED"] != DBNull.Value ? Convert.ToInt32(records["REJECTED"]) : 0;
                            instance.SentforFurtherAssessment = records["SENT_FURTHER_ASSESSMENT"] != DBNull.Value ? Convert.ToInt32(records["SENT_FURTHER_ASSESSMENT"]) : 0;
                            instance.NoActionTaken = records["NO_ACTION_TAKEN"] != DBNull.Value ? Convert.ToInt32(records["NO_ACTION_TAKEN"]) : 0;
                        }
                );
                return NENSOAReportList;        
        }
        #endregion

        #region GetNENHelpdeskReportHistory
        public static List<NENHelpdeskReportModel> GetNENHelpdeskReportHistory(int month, int year, string vehicleCat, int requiresVr1, int vehicleCount)
        {
            List<NENHelpdeskReportModel> NENHelpdeskreportList = new List<NENHelpdeskReportModel>();
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                NENHelpdeskreportList,
                UserSchema.Portal + ".SP_GET_NEN_HELPDESK_REPORT",
                parameter =>
                {
                    parameter.AddWithValue("P_VEHICLE_CAT", vehicleCat, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VEHICLE_CAT_COUNT", vehicleCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REQUIRES_VR1", requiresVr1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MONTH", month, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_YEAR", year, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.Categories = Convert.ToString(records["VEHICLE_CATEGORY"]);
                    instance.NENFailures = records["NEN_FAILURE"] != DBNull.Value ? Convert.ToInt64(records["NEN_FAILURE"]) : 0;
                    instance.NENSentByEmail = records["NEN_MAILS"] != DBNull.Value ? Convert.ToInt64(records["NEN_MAILS"]) : 0;
                    instance.NENSentByApi = records["NEN_API"] != DBNull.Value ? Convert.ToInt64(records["NEN_API"]) : 0;
                    instance.TotalNENSubmitted = records["TOTAL"] != DBNull.Value ? Convert.ToInt64(records["TOTAL"]) : 0;
                }
                );
            return NENHelpdeskreportList;
        }
        #endregion

        #region GetNENRouteID
        public static long GetNENRouteID(int NENId, int inboxItemID, int organisationId, char returnVal = 'R')
        {
            long Lreturn_val = 0;
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    Lreturn_val,
                     UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_GET_NEN_ROUTEID",
                    parameter =>
                    {
                        parameter.AddWithValue("P_NEN_ID", NENId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_INBOX_ITEM_ID", inboxItemID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            if (returnVal == 'R')
                                Lreturn_val = records.GetLongOrDefault("ROUTE_PART_ID");
                            else
                                Lreturn_val = records.GetLongOrDefault("USER_ID");

                        }
                );
            return Lreturn_val;
        }
        #endregion

        #region VerifyRouteIdWithOtherOrg
        public static int VerifyRouteIdWithOtherOrg(int NENId, int organisationId, int routePartId)
        {
            int IsUsing = 0;
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    IsUsing,
                     UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_VERI_ROUTEID_OTHER_ORG",
                    parameter =>
                    {
                        parameter.AddWithValue("P_NEN_ID", NENId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ORGANISATION_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ROUTE_PART_ID", routePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            IsUsing = Convert.ToInt32(records.GetDecimalOrDefault("RCOUNT"));

                        }
                );
            return IsUsing;
        }
        #endregion

        #region SaveNotificationAutoResponseAuditLog
        public static long SaveNotificationAutoResponseAuditLog(string logMessage, int userId, long organisationId = 0)
        {
            long result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                        result,
                       UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_NEN_INSERT_LOG_AUTORESPONSE",
                        parameter =>
                        {
                            parameter.AddWithValue("P_LOG_MSG", logMessage, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                        },
                         (records, instance) =>
                         {
                             result = Convert.ToInt64(records["AUDIT_ID"]);
                         }
                );

            return result;
        }
        #endregion


        public static List<NENRouteStatusList> GetNENBothRouteStatus(long inboxId, int NENId, int userId, long organisationId)
        {

            List<NENRouteStatusList> Route_stat = new List<NENRouteStatusList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                  Route_stat,
                  UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_GET_NENBOTH_ROUTE_STAT",
                  parameter =>
                  {
                      parameter.AddWithValue("P_INBOX_ID", inboxId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("P_RESULSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                  },
                  (records, instance) =>
                  {
                      instance.RouteStatus = Convert.ToInt32(records["ROUTE_STATUS"]);
                  }
             );
            return Route_stat;
        }

        public static int GetRouteStatus(int inboxItemId, int NENId, int userId)
        {

            int RouteStatus = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
           RouteStatus,
              UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_GET_ROUTE_STATUS",
                parameter =>
                {

                    parameter.AddWithValue("R_INBOX_ITEM_ID", inboxItemId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("R_USER_ID", userId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("R_NEN_ID", NENId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("R_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        RouteStatus = records.GetInt32OrDefault("ROUTE_STATUS");
                    }
            );
            return RouteStatus;
        }

        public static List<NENGeneralDetails> GetNENRouteDescription(int NENId, long inboxId, long organisationId)
        {

            List<NENGeneralDetails> ObjRoutedescp = new List<NENGeneralDetails>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                ObjRoutedescp,
                  UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_GET_ROUTE_DESCRIPTION",
                parameter =>
                {
                    parameter.AddWithValue("P_NENID", NENId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_INBOX_ITEMID", inboxId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.RoutePartIdS = records.GetLongOrDefault("ROUTE_PART_ID");
                        instance.RoutePartNo = records.GetInt16OrDefault("ROUTE_PART_NO");
                        instance.GRouteDescription = records.GetStringOrDefault("PART_DESCR");
                    }
            );
            return ObjRoutedescp;


        }

        public static List<NENGeneralDetails> GetRouteFromAndToDescp(long routepartId)
        {

            List<NENGeneralDetails> ObjRoutedescp = new List<NENGeneralDetails>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
           ObjRoutedescp,
              UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_GET_ROUTE_FROM_TO_DESCP",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_PART_ID", routepartId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.RoutePartNo = records.GetInt16OrDefault("ROUTE_POINT_NO");
                        instance.From = records.GetStringOrDefault("DESCR");
                    }
            );
            return ObjRoutedescp;


        }

        public static NENHaulierRouteDesc GetHualierRouteDesc(int NENId, int inboxItemId)
        {

            NENHaulierRouteDesc ObjRoutedescp = new NENHaulierRouteDesc();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                ObjRoutedescp,
                  UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_GET_HUALIER_ROUTE_DESC",
                parameter =>
                {
                    parameter.AddWithValue("P_NEN_ID", NENId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_INBOX_ITEM_ID", inboxItemId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        ObjRoutedescp.HualierGRouteDescription = records.GetStringOrDefault("A");
                        ObjRoutedescp.HualierDescriptionReturnLeg = records.GetStringOrDefault("B");
                        ObjRoutedescp.MainStartAddress = records.GetStringOrDefault("SA");
                        ObjRoutedescp.MainEndAddress = records.GetStringOrDefault("EA");

                    }
            );
            return ObjRoutedescp;


        }

        internal static long GetNENDocumentIdFromInbox(long NENId, long inboxId, long organisationId)
        {
            long documetnId = 0;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
            documetnId,
                UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_GET_NENDOC_ID_INBOX",
                parameter =>
                {
                    parameter.AddWithValue("P_NENID", NENId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_INBOX_ITEM_ID", inboxId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);//new parameter added for NEN R2
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        documetnId = records.GetLongOrDefault("TRANSMISSION_ID");
                    }
            );
            return documetnId;
        }
        public static int UpdateInboxTypeFirstTime(long InboxId, long organisationId)
        {
            int UpdateStatus = 0;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
            UpdateStatus,
                UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_UPDATE_ITEM_TYPE",
                parameter =>
                {
                    parameter.AddWithValue("U_INBOX_ITEM_ID", InboxId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("U_ORG_ID", organisationId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767); // NEN R2
                    parameter.AddWithValue("U_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        UpdateStatus = 1;
                    }
            );
            return UpdateStatus;
        }
        public static NENGeneralDetails GetGeneralDetail(int NENId, int RouteId)
        {
            NENGeneralDetails GDetail = new NENGeneralDetails();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
           GDetail,
             UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_GET_GENERAL_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("p_NEN_ID", NENId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ROUTEID", RouteId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.From = records.GetStringOrDefault("FROM_DESCR");
                    instance.To = records.GetStringOrDefault("TO_DESCR");
                    instance.MovementName = records.GetStringOrDefault("MOVEMENT_NAME");
                    instance.ESDALReference = records.GetStringOrDefault("NOTIFICATION_CODE");
                    instance.MyReference = records.GetStringOrDefault("MY_REF");
                    instance.ClientName = records.GetStringOrDefault("CLIENT");
                    // instance.HaulierOprLicence = records.GetStringOrDefault("CLIENT");
                    instance.FromAddress = records.GetStringOrDefault("FROM_DESCR");
                    instance.ToAddress = records.GetStringOrDefault("TO_DESCR");
                    instance.Classification = records.GetStringOrDefault("NAME");
                    instance.FromDateTime = Convert.ToDateTime(records.GetStringOrDefault("START_DATE")).ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    instance.ToDateTime = Convert.ToDateTime(records.GetStringOrDefault("END_DATE")).ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    var MovesNoType = records.GetFieldType("NO_OF_MOVES");
                    if (MovesNoType == null)
                        instance.NoOfMovements = 0;
                    else
                        instance.NoOfMovements = records.GetInt16OrDefault("NO_OF_MOVES");
                    instance.LoadDescription = records.GetStringOrDefault("load_descr");
                    instance.NotesOnEscort = records.GetStringOrDefault("notesonescort");
                    try
                    {
                        instance.IsMostRecent = records.GetInt16OrDefault("IS_MOST_RECENT");
                    }
                    catch
                    {
                        instance.IsMostRecent = short.Parse(records.GetDecimalOrDefault("IS_MOST_RECENT").ToString());
                    }
                    instance.GRouteDescription = records.GetStringOrDefault("VROUTE_DESC");
                    instance.ReceivedOn = Convert.ToDateTime(records.GetDateTimeOrDefault("Received_On")).ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    instance.ContentRefNo = records.GetStringOrDefault("PLANNED_CONTENT_REF_NO");
                    instance.Email = records.GetStringOrDefault("EMAIL_ADDRESS");
                    instance.HaulierOprLicence = records.GetStringOrDefault("LICENSE");
                    instance.Telephone = records.GetStringOrDefault("TELEPHONE_NUMBER");
                    instance.IndemnityConfirmation = records.GetInt16OrDefault("INDEMNITY_CONFIRMATION");
                    instance.HaulierContactName = records.GetStringOrDefault("HAULIER_CONTACT");
                    instance.HaulierAddress1 = records.GetStringOrDefault("HAULIER_ADDRESS_1");
                    instance.HaulierAddress2 = records.GetStringOrDefault("HAULIER_ADDRESS_2");
                    instance.HaulierAddress3 = records.GetStringOrDefault("HAULIER_ADDRESS_3");
                    instance.HaulierAddress4 = records.GetStringOrDefault("HAULIER_ADDRESS_4");
                    instance.HaulierAddress5 = records.GetStringOrDefault("HAULIER_ADDRESS_5");
                    instance.OnBehalf = records.GetStringOrDefault("on_behalf_of");
                    instance.OtherContactDetails = records.GetStringOrDefault("other_contact_details");
                    instance.Notes = records.GetStringOrDefault("notification_notes_from_haulier");
                }
            );
            return GDetail;
        }
        internal static NotificationGeneralDetails GetNENNotifInboundDet(long NotifId, long NENId)
        {
            NotificationGeneralDetails objDetails = new NotificationGeneralDetails();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                objDetails,
                  UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_GET_NEN_NOTIF_INB_DOC",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIF_ID ", NotifId, OracleDbType.Int64, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_NENID ", NENId, OracleDbType.Int64, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.InboundNotification = records.GetByteArrayOrNull("INBOUND_NOTIFICATION");
                    }
            );
            return objDetails;
        }

        public static int UpdateRouteStatus(int InboxId, int UserId, int RouteId, int RouteStatus, long OrganisationId)
        {
            try
            {
                int UpdateStatus = 0;

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               UpdateStatus,
                 UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_UPDATE_ROUTE_STATUS",
                    parameter =>
                    {
                        parameter.AddWithValue("U_INBOX_ITEM_ID", InboxId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("U_USER_ID", UserId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("U_ROUTE_ID", RouteId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);//NEN R2
                        parameter.AddWithValue("U_ROUTE_STATUS", RouteStatus, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("U_ORD_ID", OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("U_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            UpdateStatus = 1;
                        }
                );
                return UpdateStatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertInboxEditRouteForNewUser(int InboxId, long NENId, int NotificationId, int NewUserId, long EditedRouteId, long NewRouteId, long OrganisationId)
        {
            bool result = false;
            int flag = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_INSERT_INBOX_EDITROUTE",
                parameter =>
                {
                    parameter.AddWithValue("P_INBOX_ID", InboxId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NEN_ID", NENId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NOTIF_ID", NotificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NEW_USER_ID", NewUserId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_EDITED_ROUTE_ID", EditedRouteId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NEW_ROUTE_ID", NewRouteId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    flag = Convert.ToInt32(records.GetDecimalOrDefault("VRETURN"));
                }
                );
            if (flag == 1)
                result = true;
            return result;
        }

        public static long GetNENReturnRouteID(int InboxItemId, int orgId)
        {
            long ReturnRouteID = 0;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                ReturnRouteID,
                 UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_GET_NEN_RETURN_ROUTEID",
                parameter =>
                {
                    parameter.AddWithValue("P_INBOX_ITEM_ID", InboxItemId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        ReturnRouteID = records.GetLongOrDefault("ROUTE_PART_ID");
                    }
            );
            return ReturnRouteID;
        }

        public static int UpdateNENICAStatusInboxItem(int InboxId, int IcaStatus, long OrganisationId)
        {
            int flag = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                flag,
                UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_UPDATE_ICA_STAT_INBX",
                parameter =>
                {
                    parameter.AddWithValue("P_INBOX_ID", InboxId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ICA_STATUS", IcaStatus, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    flag = Convert.ToInt32(records.GetDecimalOrDefault("V_RES"));
                }
                );
            return flag;
        }
        public static int UpdateNenApiIcaStatus(UpdateNENICAStatusParams updateNENICA)
        {
            int flag = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery
            (
                UserSchema.Portal + ".STP_NE_GENERAL.SP_UPDATE_API_ICA_STATUS",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIF_ID", updateNENICA.NotificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", updateNENICA.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ICA_STATUS", updateNENICA.IcaStatus, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    flag = records.GetInt32("P_AFFECTED_ROWS");
                }
            );
            return flag;
        }
    }
}