

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.DataAccess;
using STP.DataAccess.SafeProcedure;
using STP.Common.Enums;
using Oracle.DataAccess.Client;
using STP.Common.Constants;
using Newtonsoft.Json;
using STP.Domain;
using STP.Domain.DocumentsAndContents;

namespace STP.DocumentsAndContents.Persistance
{
    public static class FeedbackDAO
    {

       
        #region InsertFeedback
        public static int InsertFeedbackDetails(InsertFeedbackDomain insertFeedbackDomain)
        {

            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
             UserSchema.Portal + ".ADD_FEEDBACK",
                parameter =>
                {
                    parameter.AddWithValue("p_FeedBackType", insertFeedbackDomain.FeedbackType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_FEEDBACK", insertFeedbackDomain.FeedBack, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_UserId", insertFeedbackDomain.UserId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    rowsAffected = record.GetInt32("p_AFFECTED_ROWS");
                }
            );
            return rowsAffected;
        }

        #endregion
        public static List<FeedbackDomain> GetFeedbackSearchInfo(int pageNum, int pageSize, int flag, string searchType, string searchString, int sortOrder, int presetFilter)
        {
            string FType = null, data = null;
            string[] description = null;
            List<FeedbackDomain> fdbDaoObj = new List<FeedbackDomain>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                fdbDaoObj,
                 UserSchema.Portal + ".SP_R_LIST_FEEDBACK",
                 parameter =>
                 {
                     parameter.AddWithValue("searchType", searchType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("flag", flag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("pageNumber", pageNum, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("searchString", searchString, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_SORT_ORDER", sortOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_PRESET_FILTER", presetFilter, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                 },
                (records, instance) =>
                {
                    instance.ListCount = (int)records.GetDecimalOrDefault("TOTALRECORDCOUNT");

                    FType = records.GetStringOrDefault("FEEDBACKTYPE");
                    switch (FType)
                    {
                        case "1":
                            instance.FeedbackTypeName = "Complaint";
                            break;
                        case "2":
                            instance.FeedbackTypeName = "Suggestion";
                            break;
                        case "3":
                            instance.FeedbackTypeName = "General complaint";
                            break;
                        case "4":
                            instance.FeedbackTypeName = "Fault";
                            break;
                    }
                    instance.Description = records.GetStringOrDefault("DESCRIPTION");

                    instance.FeedBackId = records.GetLongOrDefault("ID");
                    try
                    {
                        long contactId = 0;
                        instance.FullName = records.GetStringOrDefault("FULLNAME");
                          SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                            contactId,
                             UserSchema.Portal + ".STP_LOGIN_PKG.SP_LOGINASOTHERUSER",
                            parameter =>
                            {
                                parameter.AddWithValue("p_UserName", instance.FullName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                                parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                            },
                            userrecords =>
                            {

                                contactId = userrecords.GetLongOrDefault("CONTACT_ID");
                                instance.ContactId = contactId;
                            }
                            );

                        string sDate = records.GetDateTimeOrDefault("SHOWDATE").ToString();
                        if (sDate == null || sDate == "01/01/0001 00:00:00")
                        {
                            instance.ShowDateTime = "";
                        }
                        else
                        {
                            instance.ShowDateTime = records.GetDateTimeOrDefault("SHOWDATE").ToString();
                        }
                        instance.OpenCheck = records.GetInt16OrDefault("CHECKOPEN");

                    }
                    catch (Exception e)
                    {
                        instance.OpenCheck = 0;
                    }
                }
             );

            return fdbDaoObj;
        }

        public static int DeleteFeedbackInfo(int feedBackId)
        {
            FeedbackDomain feedbackObj = new FeedbackDomain();
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(

                 UserSchema.Portal + ".SP_DELETE_FEEDBACK",
                 parameter =>
                 {
                     parameter.AddWithValue("feedBackId", feedBackId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                 },
                 records =>
                 {

                     result = records.GetInt32("p_AFFECTED_ROWS");
                 }
              );
            return result;
        }

        public static FeedbackDomain GetFeedbackdetails(long feedBackId, int openChk = 0)
        {
            string FType = null;
            FeedbackDomain fdbDaoObj = new FeedbackDomain();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                fdbDaoObj,
                 UserSchema.Portal + ".GET_FEEDBACKDETAILS",
                 parameter =>
                 {
                     parameter.AddWithValue("FeedbackId", feedBackId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("openChk", openChk, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                 },
                 (records, instance) =>
                 {

                     fdbDaoObj.FeedBackId = records.GetLongOrDefault("ID");
                     fdbDaoObj.FullName = records.GetStringOrDefault("USERID");
                     fdbDaoObj.SearchType = records.GetStringOrDefault("TYPE");
                     FType = records.GetStringOrDefault("FEEDBACKTYPE");
                     switch (FType)
                     {
                         case "1":
                             instance.FeedbackTypeName = "Complaint";
                             break;
                         case "2":
                             instance.FeedbackTypeName = "Suggestion";
                             break;
                         case "3":
                             instance.FeedbackTypeName = "General Complaint";
                             break;
                         case "4":
                             instance.FeedbackTypeName = "Fault";
                             break;
                     }

                     fdbDaoObj.Description = records.GetStringOrDefault("DESCRIPTION");
                     fdbDaoObj.OpenCheck = records.GetInt16OrDefault("CHECKOPEN");
                 }
              );
            return fdbDaoObj;
        }
    }
}