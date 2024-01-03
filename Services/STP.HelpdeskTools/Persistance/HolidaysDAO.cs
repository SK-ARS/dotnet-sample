using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.Domain.HelpdeskTools;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace STP.HelpdeskTools.Persistance
{
    public static class HolidaysDAO
    {
        #region Get All Holiday
        public static List<HolidaysDomain> GetAllHoliday(int pageNumber, int pageSize ,int flag, string monthYear,string searchType = "", int sortOrder=0, int presetFilter=0)
        {
            List<HolidaysDomain> ObjHoliday = new List<HolidaysDomain>();
            int number=0;
            number = searchType != null ? Convert.ToInt32(searchType) : 4;
            string[] TempDatearray = new string[1];
            string[] TempDatearrayHoliday = new string[1];
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                ObjHoliday,
                UserSchema.Portal + ".SP_LIST_HOLIDAYS",
                    parameter =>
                    {
                        parameter.AddWithValue("flag", flag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("Month", monthYear, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_COUNTRYID", number, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_PageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_PageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_SORT_ORDER", sortOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_PRESET_FILTER", presetFilter, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                (records, instance) =>
                {
                    instance.ListCount = (int)records.GetDecimalOrDefault("TOTALRECORDCOUNT");
                    instance.HolidayId = (long)records.GetDecimalOrDefault("HOLIDAY_ID");
                    instance.Descripton = records.GetStringOrDefault("DESCRIPTION");

                    instance.CountryName = records.GetStringOrDefault("COUNTRY_NAME");
                    instance.CountryId = records.GetInt32OrDefault("COUNTRY_ID");
                    
                    string TempData = records.GetDateTimeOrDefault("HolidayDate").ToString("dd/MM/yyyy");
                    TempDatearray = TempData.Split(' ');
                    instance.HolidayDate = TempDatearray[0].ToString();
                    string TempDataHoliday = records.GetDateTimeOrDefault("HolidayDate").ToString("dd");
                    TempDatearrayHoliday = TempDataHoliday.Split(' ');
                    instance.HighlightedDate = TempDatearrayHoliday[0].ToString();

                }
                );
            
            
            
            return ObjHoliday;
        }
        #endregion

        #region Insert Holiday Information

        public static int InsertHolidaysInfo(string holidayDate, string description, int countryId)
        {
            int result = 0;
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    result,
                    UserSchema.Portal + ".SP_INSERT_HOLIDAY",
                     parameter =>
                     {
                         parameter.AddWithValue("HolidayDate", holidayDate, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("DESCRIPTION", description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_CountryId", countryId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                     },
                     records =>
                     {
                         result = (int)records.GetDecimalOrDefault("CNT");
                     }
                  );           
            return result;
        }
        #endregion

        #region Delete Holiday Information
        public static int DeleteHolidayInfo(long holidayId)
        {
            int result = 0;
                SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                    UserSchema.Portal + ".SP_DELETE_HOLIDAY",
                     parameter =>
                     {
                         parameter.AddWithValue("HOLIDAY_ID", holidayId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                     },
                     records =>
                     {
                         result = records.GetInt32("p_AFFECTED_ROWS");
                     }
                  );  
            return result;
        }
        #endregion

        #region Edit Holiday Information
        public static int EditHolidaysInfo(HolidayParams objHolidayParams)
        {
            int result = 0;
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    result,
                    UserSchema.Portal + ".SP_EDIT_HOLIDAY",
                     parameter =>
                     {
                         parameter.AddWithValue("HOLIDAY_ID", objHolidayParams.HolidayId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("HolidayDate", objHolidayParams.HolidayDate, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("DESCRIPTION", objHolidayParams.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_COUNTRYID", objHolidayParams.CountryId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                     },
                     records =>
                     {
                         result = (int)records.GetDecimalOrDefault("CNT");
                     }
                  );            
            return result;
        }
        #endregion
    }
}