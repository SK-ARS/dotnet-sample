using Oracle.DataAccess.Client;
using SpecialOrderXSD;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.DataAccess.SafeProcedure;
using STP.DocumentsAndContents.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.DocumentsAndContents.Persistance
{
    public static class AmendmentOrderDAO
    {
        /// <summary>
        /// get amendment order document detail
        /// </summary>
        /// <param name="orderNumber">order no</param>
        /// <returns></returns>
        public static SpecialOrderStructure GetAmendmentOrderDetails(string orderNumber, long organisationId)
        {
            SpecialOrderStructure sos = new SpecialOrderStructure();
            string esDAlRefNo = string.Empty;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                sos,
                UserSchema.Sort + ".GET_AMENDMENTORDER_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("p_ORDER_NUMBER", orderNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       esDAlRefNo = records.GetStringOrDefault("ESDAL_REF");
                       orderNumber = records.GetStringOrDefault("ORDER_NO");

                       instance.OrderTemplate = SpecialOrderXSD.SpecialOrderTemplateType.Item2d5;
                       instance.OrderNumber = orderNumber;

                           #region ESDALRefrenceNumber
                           ESDALReferenceNumberStructure esdalrefnostru = new ESDALReferenceNumberStructure();
                       esdalrefnostru.Mnemonic = records.GetStringOrDefault("HAULIER_MNEMONIC");
                       string[] hauMnemonicArr = records.GetStringOrDefault("ESDAL_REF").Split("/".ToCharArray());
                       if (hauMnemonicArr.Length > 0)
                       {
                           esdalrefnostru.MovementProjectNumber = hauMnemonicArr[1];
                       }

                       MovementVersionNumberStructure movvernostru = new MovementVersionNumberStructure();
                       movvernostru.Value = records.GetShortOrDefault("VERSION_NO");
                       esdalrefnostru.MovementVersion = movvernostru;
                       esdalrefnostru.EnteredBySORT = true;
                       esdalrefnostru.EnteredBySORTSpecified = true;
                       instance.ESDALReferenceNumber = esdalrefnostru;
                           #endregion
                           instance.JobFileReferenceNumber = records.GetStringOrDefault("HA_JOB_FILE_REF");

                       instance.Load = records.GetStringOrDefault("LOAD_DESCR");

                       instance.HaulierName = records.GetStringOrDefault("HAULIER_NAME");

                       instance.NoOfJourneysInWord = CommonMethods.NumberToWords(Convert.ToInt16(records.GetShortOrDefault("TOTAL_MOVES")));

                           #region SigningDetails
                           SigningDetailsStructure sosigndetail = new SigningDetailsStructure();

                       sosigndetail.SigningDate = records.GetDateTimeOrDefault("SIGNED_DATE");

                       int dayNo = sosigndetail.SigningDate.Day;

                       sosigndetail.CustomSigningDate = string.Format("{0} {1} {2}",
                                   AddOrdinal(dayNo), sosigndetail.SigningDate.ToString("MMMM"), sosigndetail.SigningDate.Year);

                       sosigndetail.Signatory = records.GetStringOrDefault("SIGNATORY");
                       sosigndetail.SignatoryRole = records.GetStringOrDefault("SIGNATORY_ROLE");

                       instance.SigningDetails = sosigndetail;
                           #endregion

                           instance.ExpiryDate = records.GetDateTimeOrDefault("EXPIRY_DATE");

                       instance.CustomFormatExpiryDate = string.Format("{0} {1} {2}",
                                   AddOrdinal(dayNo), instance.ExpiryDate.ToString("MMMM"), instance.ExpiryDate.Year);

                       List<MovementExclusionStructure> movementexclustruList = GetBankHolidayExclusion(sosigndetail.SigningDate, instance.ExpiryDate, organisationId);

                       instance.BankHolidayExclusion = ArrangeBankHolidayExclusionStartEndDate(movementexclustruList).ToArray();
                   }
            );

            return sos;
        }

        /// <summary>
        /// get bank holiday detail for specific duration
        /// </summary>
        /// <param name="startdate">start date</param>
        /// <param name="enddate">end date</param>
        /// <returns></returns>
        public static List<MovementExclusionStructure> GetBankHolidayExclusion(DateTime startdate, DateTime enddate, long organisationId, string userSchema = UserSchema.Sort)
        {
            List<MovementExclusionStructure> bankholidayexclusionList = new List<MovementExclusionStructure>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                bankholidayexclusionList,
                userSchema + ".GET_HOLIDAY_DETAIL",
                parameter =>
                {
                    parameter.AddWithValue("p_START_DATE", startdate.ToString("dd-MMMM-yy"), OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_END_DATE", enddate.ToString("dd-MMMM-yy"), OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGANISATION_ID", organisationId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 (records, instance) =>
                 {
                     TimeDayDateStructure start = new TimeDayDateStructure();
                     TimeDayDateStructure end = new TimeDayDateStructure();

                     DateTime dt = records.GetDateTimeOrDefault("WHEN");

                     start.Date = dt;
                     start.Time = TimeOfDayType.noon;
                     start.TimeSpecified = true;
                     instance.Start = start;


                     if (dt.ToString("dddd") == "Monday")
                         start.Day = DayOfWeekType.monday;
                     if (dt.ToString("dddd") == "Tuesday")
                         start.Day = DayOfWeekType.tuesday;
                     if (dt.ToString("dddd") == "Wednesday")
                         start.Day = DayOfWeekType.wednesday;
                     if (dt.ToString("dddd") == "Thursday")
                         start.Day = DayOfWeekType.thursday;
                     if (dt.ToString("dddd") == "Friday")
                         start.Day = DayOfWeekType.friday;
                     if (dt.ToString("dddd") == "Saturday")
                         start.Day = DayOfWeekType.saturday;
                     if (dt.ToString("dddd") == "Sunday")
                         start.Day = DayOfWeekType.sunday;


                     end.Day = DayOfWeekType.monday;
                     if (dt.ToString("dddd") == "Tuesday")
                         end.Day = DayOfWeekType.tuesday;
                     if (dt.ToString("dddd") == "Wednesday")
                         end.Day = DayOfWeekType.wednesday;
                     if (dt.ToString("dddd") == "Thursday")
                         end.Day = DayOfWeekType.thursday;
                     if (dt.ToString("dddd") == "Friday")
                         end.Day = DayOfWeekType.friday;
                     if (dt.ToString("dddd") == "Saturday")
                         end.Day = DayOfWeekType.saturday;
                     if (dt.ToString("dddd") == "Sunday")
                         end.Day = DayOfWeekType.sunday;
                     end.Date = dt;
                     end.Time = TimeOfDayType.noon;
                     end.TimeSpecified = true;

                     instance.End = end;
                 }
                );

            return bankholidayexclusionList;
        }

        /// <summary>
        /// get summer holiday detail for specific duration
        /// </summary>
        /// <param name="startdate">start date</param>
        /// <param name="enddate">end date</param>
        /// <returns></returns>
        public static List<MovementExclusionStructure> GetSummerExclusion(DateTime startdate, DateTime enddate)
        {
            List<MovementExclusionStructure> summerexclusionList = new List<MovementExclusionStructure>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                summerexclusionList,
                 UserSchema.Portal + ".GET_SUMMER_HOLIDAY_DETAIL",
                parameter =>
                {
                    parameter.AddWithValue("p_START_DATE", startdate.ToString("dd-MMMM-yy"), OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_END_DATE", enddate.ToString("dd-MMMM-yy"), OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 (records, instance) =>
                 {
                     TimeDayDateStructure start = new TimeDayDateStructure();
                     TimeDayDateStructure end = new TimeDayDateStructure();

                     DateTime dt = records.GetDateTimeOrDefault("WHEN");

                     start.Date = dt;
                     start.Time = TimeOfDayType.noon;
                     start.TimeSpecified = true;
                     instance.Start = start;

                     if (dt.ToString("dddd") == "Monday")
                         start.Day = DayOfWeekType.monday;
                     if (dt.ToString("dddd") == "Tuesday")
                         start.Day = DayOfWeekType.tuesday;
                     if (dt.ToString("dddd") == "Wednesday")
                         start.Day = DayOfWeekType.wednesday;
                     if (dt.ToString("dddd") == "Thursday")
                         start.Day = DayOfWeekType.thursday;
                     if (dt.ToString("dddd") == "Friday")
                         start.Day = DayOfWeekType.friday;
                     if (dt.ToString("dddd") == "Saturday")
                         start.Day = DayOfWeekType.saturday;
                     if (dt.ToString("dddd") == "Sunday")
                         start.Day = DayOfWeekType.sunday;


                     end.Day = DayOfWeekType.monday;
                     if (dt.ToString("dddd") == "Tuesday")
                         end.Day = DayOfWeekType.tuesday;
                     if (dt.ToString("dddd") == "Wednesday")
                         end.Day = DayOfWeekType.wednesday;
                     if (dt.ToString("dddd") == "Thursday")
                         end.Day = DayOfWeekType.thursday;
                     if (dt.ToString("dddd") == "Friday")
                         end.Day = DayOfWeekType.friday;
                     if (dt.ToString("dddd") == "Saturday")
                         end.Day = DayOfWeekType.saturday;
                     if (dt.ToString("dddd") == "Sunday")
                         end.Day = DayOfWeekType.sunday;
                     end.Date = dt;
                     end.Time = TimeOfDayType.noon;
                     end.TimeSpecified = true;

                     instance.End = end;
                 }
                );

            return summerexclusionList;
        }

        /// <summary>
        /// arrange date in sequence in case mutiple holiday comes in sequence between specific duration
        /// </summary>
        /// <param name="holidayexclusionList">holiday exclusion list</param>
        /// <returns></returns>
        public static List<MovementExclusionStructure> ArrangeBankHolidayExclusionStartEndDate(List<MovementExclusionStructure> holidayexclusionList)
        {
            List<MovementExclusionStructure> updatedmovementexclustruList = new List<MovementExclusionStructure>();

            foreach (MovementExclusionStructure day in holidayexclusionList)
            {
                MovementExclusionStructure newstru = new MovementExclusionStructure();
                newstru.Start = day.Start;
                newstru.End = day.End;

                bool status = false;
                foreach (MovementExclusionStructure updateddays in updatedmovementexclustruList)
                {
                    if (updateddays.Start.Date <= day.Start.Date && updateddays.End.Date >= day.Start.Date)
                    {
                        status = true;
                        break;
                    }
                }
                if (status)
                    continue;

                DateTime dt = day.Start.Date.AddDays(1);

            outer:
                int count = 0;
                foreach (MovementExclusionStructure days in holidayexclusionList)
                {

                    if (dt == days.Start.Date)
                    {
                        count += 1;
                        if (count == 1)
                        {
                            dt = dt.AddDays(1);
                            goto outer;
                        }
                    }
                }

                dt = dt.AddDays(-1);

                newstru.End.Date = dt;

                newstru.Start.Date = newstru.Start.Date.AddDays(-1);

                dt = newstru.Start.Date;

                if (dt.ToString("dddd") == "Monday")
                    newstru.Start.Day = DayOfWeekType.monday;
                if (dt.ToString("dddd") == "Tuesday")
                    newstru.Start.Day = DayOfWeekType.tuesday;
                if (dt.ToString("dddd") == "Wednesday")
                    newstru.Start.Day = DayOfWeekType.wednesday;
                if (dt.ToString("dddd") == "Thursday")
                    newstru.Start.Day = DayOfWeekType.thursday;
                if (dt.ToString("dddd") == "Friday")
                    newstru.Start.Day = DayOfWeekType.friday;
                if (dt.ToString("dddd") == "Saturday")
                    newstru.Start.Day = DayOfWeekType.saturday;
                if (dt.ToString("dddd") == "Sunday")
                    newstru.Start.Day = DayOfWeekType.sunday;

                newstru.End.Date = newstru.End.Date.AddDays(1);

                dt = newstru.End.Date;

                if (dt.ToString("dddd") == "Monday")
                    newstru.End.Day = DayOfWeekType.monday;
                if (dt.ToString("dddd") == "Tuesday")
                    newstru.End.Day = DayOfWeekType.tuesday;
                if (dt.ToString("dddd") == "Wednesday")
                    newstru.End.Day = DayOfWeekType.wednesday;
                if (dt.ToString("dddd") == "Thursday")
                    newstru.End.Day = DayOfWeekType.thursday;
                if (dt.ToString("dddd") == "Friday")
                    newstru.End.Day = DayOfWeekType.friday;
                if (dt.ToString("dddd") == "Saturday")
                    newstru.End.Day = DayOfWeekType.saturday;
                if (dt.ToString("dddd") == "Sunday")
                    newstru.End.Day = DayOfWeekType.sunday;

                newstru.End.Time = TimeOfDayType.noon;

                int dayNo = newstru.Start.Date.Day;

                newstru.Start.CustomStartDate = string.Format("{0} {1} {2}",
                                       AddOrdinal(dayNo), newstru.Start.Date.ToString("MMMM"), newstru.Start.Date.Year);

                dayNo = newstru.End.Date.Day;

                newstru.End.CustomEndDate = string.Format("{0} {1} {2}",
                                       AddOrdinal(dayNo), newstru.End.Date.ToString("MMMM"), newstru.End.Date.Year);

                updatedmovementexclustruList.Add(newstru);
            }
            return updatedmovementexclustruList;
        }

        /// <summary>
        /// for get ordinal detail for day from the datetime
        /// </summary>
        /// <param name="num">day</param>
        /// <returns></returns>
        public static string AddOrdinal(int num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }

        }
    }
}