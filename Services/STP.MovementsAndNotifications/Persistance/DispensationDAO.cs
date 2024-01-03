using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.Domain;
using STP.Domain.MovementsAndNotifications.Movements;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace STP.MovementsAndNotifications.Persistance
{
    public static class DispensationDAO
    {
        public static object P_SORT_ORDER { get; private set; }
        #region GetAffDispensationInfo(organisationId, granteeId, pageNumber, pageSize, userType);
        public static List<DispensationGridList> GetAffDispensationInfo(int organisationId, int granteeId, int pageNumber, int pageSize, int userType)
        {
            List<DispensationGridList> dispGridObj = new List<DispensationGridList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                dispGridObj,
                UserSchema.Portal + ".STP_ROUTE_ASSESSMENT.SP_AFFECTED_DISP_STATUS",
                parameter =>
                {
                    parameter.AddWithValue("GRANTEE_ORG", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("GRANTOR_ORG", granteeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.RecordCount = Convert.ToInt32(records.GetDecimalOrDefault("TOTALRECORDCOUNT"));
                    instance.DispensationId = records.GetLongOrDefault("DISPENSATION_ID");
                    instance.DispensationReferenceNo = records.GetStringOrDefault("DISPENSATION_REF");
                    instance.Summary = records.GetStringOrDefault("SHORT_SUMMARY");
                    instance.ToDate = ConvertDate(records.GetStringOrDefault("VALID_TO_DATE"));
                    if (userType == 696002 || userType == 696007)
                    {
                        instance.GrantedBy = records.GetStringOrDefault("GRANTEE_NAME");
                    }
                    if (userType == 696001)
                    {
                        instance.GrantedBy = records.GetStringOrDefault("GRANTOR_NAME");
                    }

                }
                );
            return dispGridObj;
        }
        #endregion

        #region ConvertDate
        private static string ConvertDate(string date)
        {
            string newDate = string.Empty;
            if (!string.IsNullOrEmpty(date))
            {
                newDate = date;
            }
            return newDate;
        }
        #endregion

        #region dispListCount
        public static int DispensationListCount(int organisationId, int userTypeId)
        {

            int P_USER_TYPE = 0;
            if (userTypeId == 696002 || userTypeId == 696007) //FOR POLICE AND SOA USER LIST DISPENSATION
            {
                P_USER_TYPE = 1;
            }
            else if (userTypeId == 696001)//FOR HAULIER USER LIST DISPENSATION
            {
                P_USER_TYPE = 0;
            }
            int count = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                count,
                UserSchema.Portal + ".SP_R_LIST_DISP_COUNT",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USER_TYPE", P_USER_TYPE, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records) =>
                    {
                        count = (int)records.GetDecimalOrDefault("COUNT");
                    }
            );
            return count;

        }
        #endregion

        #region GetDispensationInfo
        public static List<DispensationGridList> GetDispensationInfo(int organisationId, int pageNumber, int pageSize, int userType, int presetFilter = 1, int? sortOrder = null)
        //shoult add , int presetFilter = 1,int? sortOrder = null
        {
            int P_USER_TYPE = 0;
            if (userType == 696002 || userType == 696007) //FOR POLICE AND SOA USER LIST DISPENSATION
            {
                P_USER_TYPE = 1;
            }
            else if (userType == 696001)//FOR HAULIER USER LIST DISPENSATION
            {
                P_USER_TYPE = 0;
            }
            List<DispensationGridList> dispGridObj = new List<DispensationGridList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                dispGridObj,
                UserSchema.Portal + ".SP_R_LIST_DISPENSATION",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USER_TYPE", P_USER_TYPE, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("SORT_ORDER", sortOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("PRESET_FILTER", presetFilter, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.DispensationId = records.GetLongOrDefault("DISPENSATION_ID");
                    instance.DispensationReferenceNo = records.GetStringOrDefault("DISPENSATION_REF");
                    instance.Summary = records.GetStringOrDefault("SHORT_SUMMARY");
                    instance.ToDate = ConvertDate(records.GetStringOrDefault("VALID_TO_DATE"));
                    if (userType == 696002 || userType == 696007)
                    {
                        instance.GrantedBy = records.GetStringOrDefault("GRANTEE_NAME");
                    }
                    if (userType == 696001)
                    {
                        instance.GrantedBy = records.GetStringOrDefault("GRANTOR_NAME");
                    }

                }
                );
            return dispGridObj;
        }
        #endregion

        #region GetDispensationSearchInfo
        public static List<DispensationGridList> GetDispensationSearchList(int organisationId, int pageNumber, int pageSize, string DRefNo, string summary, string grantedBy, string description, int isValid, int chckcunt, int userType, int presetFilter = 1, int? sortOrder = null)
        {

            string grantee = null;
            string grantor = null;
            int P_USER_TYPE = 0;

            if (userType == 696002 || userType == 696007)
            {
                grantee = grantedBy;
                P_USER_TYPE = 1;
            }
            if (userType == 696001)
            {
                grantor = grantedBy;
                P_USER_TYPE = 0;
            }
            //Creating new object for GetStructureList
            List<DispensationGridList> DispSearchList = new List<DispensationGridList>();

            //Setup Procedure LIST_STRUCTURE
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                DispSearchList,
                UserSchema.Portal + ".SP_R_SEARCH_DISPENSATION",
                parameter =>
                {
                    parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REF_NUM", DRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SHRT_SUMMARY", summary, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_Notif_Auth", grantor, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_grantee_name", grantee, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_Description", description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_Expired", isValid, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("CHK_CNT", chckcunt, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USER_TYPE", P_USER_TYPE, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("SORT_ORDER", sortOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("PRESET_FILTER", presetFilter, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        if (chckcunt == 0)
                        {
                            instance.DispensationId = records.GetLongOrDefault("DISPENSATION_ID");
                            instance.DispensationReferenceNo = records.GetStringOrDefault("DISPENSATION_REF");
                            instance.Summary = records.GetStringOrDefault("SHORT_SUMMARY");
                            instance.Description = records.GetStringOrDefault("DESCRIPTION");
                            if (userType == 696002 || userType == 696007)
                            {
                                instance.GrantedBy = records.GetStringOrDefault("GRANTEE_NAME");
                            }
                            if (userType == 696001)
                            {
                                instance.GrantedBy = records.GetStringOrDefault("GRANTOR_NAME");
                            }
                            instance.ToDate = ConvertDate(records.GetStringOrDefault("VALID_TO_DATE"));
                        }
                        else
                        {
                            instance.ListCount = Convert.ToInt32(records.GetDecimalOrDefault("COUNT"));
                        }
                    }
            );

            return DispSearchList;


        }
        #endregion

        #region ViewDispensationInfoByDRN
        public static DispensationGridList ViewDispensationInfoByDRN(string DRN, int userTypeId)
        {
            DispensationGridList dispObjList = new DispensationGridList();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                dispObjList,
                UserSchema.Portal + ".SP_R_GET_DISPENSATION",
                parameter =>
                {
                    parameter.AddWithValue("p_DISP_ID", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_DISP_REF", DRN, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);

                },
                (records, instance) =>
                {
                    instance.DispensationReferenceNo = records.GetStringOrDefault("DISPENSATION_REF");
                    instance.Summary = records.GetStringOrDefault("SHORT_SUMMARY");
                    instance.Description = records.GetStringOrDefault("DESCRIPTION");
                    instance.GrossWeight = records.GetInt32Nullable("MAX_GROSS_WEIGHT_KG");
                    instance.SignedAxleWeight = records.GetInt32Nullable("MAX_AXLE_WEIGHT_KG");
                    instance.OverallLength = records.GetDoubleOrDefault("MAX_LEN_MTR");
                    instance.Width = records.GetDoubleOrDefault("MAX_WIDTH_MTR");
                    instance.Height = records.GetDoubleOrDefault("MAX_HEIGHT_MTR");
                    instance.FromDate = ConvertDate(records.GetStringOrDefault("VALID_FROM_DATE"));
                    instance.ToDate = ConvertDate(records.GetStringOrDefault("VALID_TO_DATE"));
                    if (userTypeId == 696007 || userTypeId == 696002)
                    {
                        instance.GrantedBy = records.GetStringOrDefault("GRANTEE_NAME");
                    }
                    else
                    {
                        instance.GrantedBy = records.GetStringOrDefault("GRANTOR_NAME");
                    }
                }

            );

            return dispObjList;
        }
        #endregion

        #region ViewDispensationInfo
        public static DispensationGridList ViewDispensationInfo(int dispensationId, int userTypeId)
        {
            DispensationGridList dispObjList = new DispensationGridList();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                dispObjList,
                UserSchema.Portal + ".SP_R_GET_DISPENSATION",
                parameter =>
                {
                    parameter.AddWithValue("p_DISP_ID", dispensationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_DISP_REF", null, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);

                },
                (records, instance) =>
                {
                    instance.DispensationReferenceNo = records.GetStringOrDefault("DISPENSATION_REF");
                    instance.Summary = records.GetStringOrDefault("SHORT_SUMMARY");
                    instance.Description = records.GetStringOrDefault("DESCRIPTION");
                    instance.GrossWeight = records.GetInt32OrDefault("MAX_GROSS_WEIGHT_KG");
                    instance.SignedAxleWeight = records.GetInt32OrDefault("MAX_AXLE_WEIGHT_KG");
                    instance.OverallLength = records.GetDoubleOrDefault("MAX_LEN_MTR");
                    instance.Width = records.GetDoubleOrDefault("MAX_WIDTH_MTR");
                    instance.Height = records.GetDoubleOrDefault("MAX_HEIGHT_MTR");
                    instance.FromDate = ConvertDate(records.GetStringOrDefault("VALID_FROM_DATE"));
                    instance.ToDate = ConvertDate(records.GetStringOrDefault("VALID_TO_DATE"));
                    if (userTypeId == 696007 || userTypeId == 696002)
                    {
                        instance.GrantedBy = records.GetStringOrDefault("GRANTEE_NAME");
                    }
                    else
                    {
                        instance.GrantedBy = records.GetStringOrDefault("GRANTOR_NAME");
                    }
                }

            );

            return dispObjList;
        }
        #endregion

        #region GetDispensationDetailsObjByID
        public static DispensationGridList GetDispensationDetailsObjByID(int dispensationId, int userTypeId)
        {
            DispensationGridList dispensationGridObj = new DispensationGridList();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                dispensationGridObj,
                UserSchema.Portal + ".SP_R_GET_DISPENSATION",
                parameter =>
                {
                    parameter.AddWithValue("p_DISP_ID", dispensationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_DISP_REF", null, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.DispensationId = records.GetLongOrDefault("DISPENSATION_ID");
                        instance.DispensationReferenceNo = records.GetStringOrDefault("DISPENSATION_REF");
                        instance.Summary = records.GetStringOrDefault("SHORT_SUMMARY");
                        instance.Description = records.GetStringOrDefault("DESCRIPTION");
                        instance.GrossWeight = records.GetInt32OrDefault("MAX_GROSS_WEIGHT_KG");
                        instance.SignedAxleWeight = records.GetInt32OrDefault("MAX_AXLE_WEIGHT_KG");
                        instance.OverallLength = records.GetDoubleOrDefault("MAX_LEN_MTR");
                        instance.Width = records.GetDoubleOrDefault("MAX_WIDTH_MTR");
                        instance.Height = records.GetDoubleOrDefault("MAX_HEIGHT_MTR");
                        instance.FromDate = ConvertDate(records.GetStringOrDefault("VALID_FROM_DATE"));
                        instance.ToDate = ConvertDate(records.GetStringOrDefault("VALID_TO_DATE"));
                        if (userTypeId == 696002 || userTypeId == 696007)
                        {
                            instance.GrantedBy = records.GetStringOrDefault("GRANTEE_NAME");
                            instance.SelectOrganisationId = Convert.ToInt32(records.GetLongOrDefault("GRANTEE_ID"));
                        }
                        if (userTypeId == 696001)
                        {
                            instance.GrantedBy = records.GetStringOrDefault("GRANTOR_NAME");
                            instance.SelectOrganisationId = Convert.ToInt32(records.GetLongOrDefault("GRANTOR_ID"));
                        }
                    }
            );
            return dispensationGridObj;
        }
        #endregion

        #region UpdateDispensation
        public static int UpdateDispensation(DispensationGridList regdisp, int userTypeId)
        {
            // DateTime fromDate = Convert.ToDateTime(regdisp.FromDate);
            // DateTime toDate = Convert.ToDateTime(regdisp.ToDate);

            //string moveStart = regdisp.FromDate + " " + DateTime.Now.ToLongTimeString();
            //string moveEnd = regdisp.ToDate + " " + DateTime.Now.ToLongTimeString();
            //DateTime fromDate = DateTime.ParseExact(moveStart, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            //DateTime toDate = DateTime.ParseExact(moveEnd, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            int? granteeid = null;
            int? grantorid = null;

            if (userTypeId == 696002 || userTypeId == 696007)
            {
                grantorid = regdisp.OrganisationId;
                granteeid = regdisp.SelectOrganisationId;
            }
            if (userTypeId == 696001)
            {
                granteeid = regdisp.OrganisationId;
                grantorid = regdisp.SelectOrganisationId;
            }

            int count = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                UserSchema.Portal + ".SP_R_EDIT_DISPENSATION",
                parameter =>
                {
                    parameter.AddWithValue("p_DISP_ID", regdisp.DispensationId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORG_ID", regdisp.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SHR_SUM", regdisp.Summary, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_DESC", regdisp.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VAL_FROM", TimeZoneInfo.ConvertTimeToUtc(regdisp.ValidFrom), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VAL_TO", TimeZoneInfo.ConvertTimeToUtc(regdisp.ValidTo), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_GROS", regdisp.GrossWeight, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE", regdisp.SignedAxleWeight, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_HEIGHT", regdisp.Height, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_WID", regdisp.Width, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_LEN", regdisp.OverallLength, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GRANTOR_ID", grantorid == 0 ? null : grantorid, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_GRANTEE_ID", granteeid == 0 ? null : granteeid, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_UserType_ID", userTypeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GrantedBy", regdisp.GrantedBy, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    count = record.GetInt32("P_AFFECTED_ROWS");
                }
            );
            return count;
        }
        #endregion

        #region DeleteDispensation
        public static int DeleteDispensation(int dispensationId)
        {
            int affectedRows = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                UserSchema.Portal + ".SP_R_DELETE_DISPENSATION",
                parameter =>
                {
                    parameter.AddWithValue("p_DISP_ID", dispensationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
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

        #region SaveDispensation
        public static bool SaveDispensation(DispensationGridList regdisp, int userTypeId)
        {
          //regdisp.FromDate = regdisp.FromDate.Replace("-", "/");
          //  regdisp.ToDate = regdisp.ToDate.Replace("-", "/");
          //  string moveStart = regdisp.FromDate;
          //  string moveEnd = regdisp.ToDate;  
          //  DateTime fromDate = DateTime.ParseExact(moveStart, "dd/MM/yyyy", CultureInfo.InvariantCulture);
          //  var fromTime = DateTime.Now;
          //  var fromHour = fromTime.Hour;
          //  var fromMinute = fromTime.Minute;
          //  var newFromDate = fromDate.ToString("dd/MM/yyy") + " " + fromHour + ":" + fromMinute + ":00";
          //  fromDate = Convert.ToDateTime(newFromDate);           
          //  DateTime toDate = DateTime.ParseExact(moveEnd, "dd/MM/yyyy", CultureInfo.InvariantCulture);
          //  var newToDate = toDate.ToString("dd/MM/yyy") + " " + fromHour + ":" + fromMinute + ":00";
          //  toDate = Convert.ToDateTime(newToDate);

            int granteeid = 0;
            int grantorid = 0;

            if (userTypeId == 696002 || userTypeId == 696007)
            {
                grantorid = regdisp.OrganisationId;
                granteeid = regdisp.SelectOrganisationId;
            }
            if (userTypeId == 696001)
            {
                granteeid = regdisp.OrganisationId;
                grantorid = regdisp.SelectOrganisationId;
            }
            bool result = false;
            int count = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".SP_R_ADD_DISPENSATION",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_ID", regdisp.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_DIS_REF", regdisp.DispensationReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FUZ_REF", regdisp.DispensationReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SHR_SUM", regdisp.Summary, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_DESC", regdisp.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VAL_FROM", TimeZoneInfo.ConvertTimeToUtc(regdisp.ValidFrom), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VAL_TO", TimeZoneInfo.ConvertTimeToUtc(regdisp.ValidTo), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_GROS", regdisp.GrossWeight, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE", regdisp.SignedAxleWeight, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_HGHT", regdisp.Height, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_WID", regdisp.Width, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_LEN", regdisp.OverallLength, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GRANTOR_ID", grantorid, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GRANTEE_ID", granteeid, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_UserType_ID", userTypeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GrantedBy", regdisp.GrantedBy, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);

                },
                 record =>
                 {
                     count = Convert.ToInt32(record.GetDecimalOrDefault("COUNT"));

                 }
            );
            if (count == 1)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }
        #endregion

        #region GetDispOrganisationInfo
        public static List<DispensationGridList> GetDispOrganisationInfo(string organisationName, int pageNumber, int pageSize, int chckcunt, int userType)
        {

            List<DispensationGridList> dispOrgList = new List<DispensationGridList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                dispOrgList,
                UserSchema.Portal + ".SP_Disp_Search_Organisation",
                parameter =>
                {

                    parameter.AddWithValue("p_ORGNAME", organisationName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_user_type", userType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("CHK_CNT", chckcunt, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    if (chckcunt == 0)
                    {
                        if (userType == 696006)
                        {

                            instance.OrganisationTypeId = records.GetInt32OrDefault("ORGTYPE");
                        }
                        instance.OrganisationId = (int)records.GetLongOrDefault("ORGANISATION_ID");
                        instance.OrganisationName = records.GetStringOrDefault("ORGNAME");
                        instance.ListCount = Convert.ToInt32(records.GetDecimalOrDefault("TOTAL_ROWS"));
                    }
                    else
                    {
                        instance.ListCount = Convert.ToInt32(records.GetDecimalOrDefault("COUNT"));
                    }
                }
                );
            return dispOrgList;

        }
        #endregion

        #region GetDispensationDetailsByID
        public static List<DispensationGridList> GetDispensationDetailsByID(int dispensationId)
        {
            List<DispensationGridList> dispensationGridObj = new List<DispensationGridList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                dispensationGridObj,
                UserSchema.Portal + ".SP_R_GET_DISPENSATION",
                parameter =>
                {
                    parameter.AddWithValue("p_DISP_ID", dispensationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_DISP_REF", null, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.DispensationReferenceNo = records.GetStringOrDefault("DISPENSATION_REF");
                        instance.DispensationReferenceNo = records.GetStringOrDefault("FUZZY_REF");
                        instance.Summary = records.GetStringOrDefault("SHORT_SUMMARY");
                        instance.Description = records.GetStringOrDefault("DESCRIPTION");
                        instance.GrossWeight = records.GetInt32OrDefault("MAX_GROSS_WEIGHT_KG");
                        instance.SignedAxleWeight = records.GetInt32OrDefault("MAX_AXLE_WEIGHT_KG");
                        instance.OverallLength = records.GetLongOrDefault("MAX_LEN_MTR");
                        instance.Width = records.GetLongOrDefault("MAX_WIDTH_MTR");
                        instance.Height = records.GetLongOrDefault("MAX_HEIGHT_MTR");
                        instance.FromDate = ConvertDate(records.GetStringOrDefault("VALID_FROM_DATE"));
                        instance.ToDate = ConvertDate(records.GetStringOrDefault("VALID_TO_DATE"));
                        instance.GrantedBy = records.GetStringOrDefault("GRANTOR_NAME");
                    }
            );
            return dispensationGridObj;
        }
        #endregion

        internal static decimal SearchDispensationReferenceNumber(string dispensationReferenceNo, int organisationId, string mode, long dispensationId)
        {
            decimal result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                  UserSchema.Portal + ".SP_IS_DISPENSATION_EXISTS",
                parameter =>
                {
                    parameter.AddWithValue("p_DIS_REF", dispensationReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MODE", mode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_DIS_ID", dispensationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_Count", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    result = (decimal)record.GetInt32("p_Count");
                }
                );
            return result;
        }
    }
}
