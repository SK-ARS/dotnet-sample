using System;
using System.Collections.Generic;
using System.Linq;
using STP.DataAccess.SafeProcedure;
using STP.Domain.LoggingAndReporting;
using Oracle.DataAccess.Client;
using STP.Common.Enums;
using STP.Common.Constants;

namespace STP.LoggingAndReporting.Persistance
{
    public static class ReportDAO
    {
        #region GetTotalSessionLengthHistoryTypewise
        public static List<SessionLengthModel> GetTotalSessionLengthHistoryTypewise(int month, int year)
        {
            List<SessionLengthModel> listSessionLength = new List<SessionLengthModel>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listSessionLength,
               UserSchema.Portal + ".GET_SESS_LEN_DTLS_TYPEWISE",
                parameter =>
                {
                    parameter.AddWithValue("MON", month, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("YR", year, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.UserType = records.GetStringOrDefault("USERTYPE");
                    instance.TotalSessionInMonth = records.GetDecimalOrDefault("TOTAL_SESSIONS");
                    instance.MostSessionInDay = records.GetDecimalOrDefault("MAX_SESSION_TIME");
                }
            );
            return listSessionLength;
        }
        #endregion

        #region GetAvgSessionLength
        public static List<SessionLengthModel> GetAvgSessionLength(int month, int year)
        {
            List<SessionLengthModel> listSessionLength = new List<SessionLengthModel>();
            DateTime startdt = DateTime.Now;
            DateTime enddt = DateTime.Now;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listSessionLength,
               UserSchema.Portal + ".GET_AVG_SESS_LENGTH",
                parameter =>
                {
                    parameter.AddWithValue("MON", month, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("YR", year, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.UserTypeID = records.GetInt32OrDefault("User_Type_ID");
                    instance.LoginTime = records.GetDateTimeOrDefault("login_time");
                    instance.LogOutTime = records.GetDateTimeOrDefault("logout_time");

                    if (records.GetDateTimeOrDefault("login_time").ToString() != string.Empty)
                        startdt = records.GetDateTimeOrDefault("login_time");
                    if (records.GetDateTimeOrDefault("logout_time").ToString() != string.Empty)
                        enddt = records.GetDateTimeOrDefault("logout_time");
                    TimeSpan ts = enddt - startdt;
                    instance.SessionDuration = ts.TotalMinutes;
                }
            );
            return listSessionLength;
        }
        #endregion

        #region GetPeakHourSessionInMonth
        public static List<SessionLengthModel> GetPeakHourSessionInMonth(int month, int year)
        {
            List<SessionLengthModel> listSessionLength = new List<SessionLengthModel>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listSessionLength,
               UserSchema.Portal + ".GET_PEAK_HOUR_SESS_MONTH",
                parameter =>
                {
                    parameter.AddWithValue("MON", month, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("YR", year, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.UserType = records.GetStringOrDefault("usertype");
                    instance.NoofSessionInPeakDay = records.GetDecimalOrDefault("totalsession");
                }
            );
            return listSessionLength;
        }

        #endregion

        #region GetSessionLengthHistory
        public static List<SessionLengthModel> GetSessionLengthHistory(int month, int year, string userType)
        {
            int userTypeID = 0;

            if (userType == "HAULIER")
                userTypeID = 696001;
            else if (userType == "SOA")
                userTypeID = 696007;
            else if (userType == "POLICE")
                userTypeID = 696002;

            List<SessionLengthModel> listSessionLength = new List<SessionLengthModel>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listSessionLength,
               UserSchema.Portal + ".GET_SESS_LEN_HISTORYDETAILS",
                parameter =>
                {
                    parameter.AddWithValue("MON", month, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("YR", year, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_userTypeId", userTypeID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.StartDate = records.GetDateTimeOrDefault("START_DATE");
                    instance.TotalRecordCount = records.GetDecimalOrDefault("TOTALRECORDCOUNT");
                    if (userTypeID == 696001)
                        instance.HaulierSessionCount = records.GetDecimalOrDefault("hauliercount");
                    else if (userTypeID == 696002)
                        instance.PoliceSessionCount = records.GetDecimalOrDefault("policecount");
                    else if (userTypeID == 696007)
                        instance.SOASessionCount = records.GetDecimalOrDefault("soacount");
                    else
                    {
                        if (records.GetStringOrDefault("UserType") == "HAULIER")
                            instance.HaulierSessionCount = records.GetDecimalOrDefault("totalcount");
                        if (records.GetStringOrDefault("UserType") == "POLICE")
                            instance.PoliceSessionCount = records.GetDecimalOrDefault("totalcount");
                        if (records.GetStringOrDefault("UserType") == "SOA")
                            instance.SOASessionCount = records.GetDecimalOrDefault("totalcount");
                    }
                }
            );

            if (userTypeID == 0)
            {
                List<DateTime> DateList = listSessionLength.Select(x => x.StartDate).Distinct().ToList();

                List<SessionLengthModel> lstNewSessionLengthModel = new List<SessionLengthModel>();
                foreach (DateTime date in DateList)
                {
                    DateTime dates = DateTime.Now;
                    decimal SOASessionCount = 0;
                    decimal PoliceSessionCount = 0;
                    decimal HaulierSessionCount = 0;
                    foreach (SessionLengthModel sessionLength in listSessionLength)
                    {
                        if (sessionLength.StartDate == date)
                        {
                            dates = sessionLength.StartDate;
                            if (sessionLength.SOASessionCount != 0)
                                SOASessionCount = sessionLength.SOASessionCount;
                            if (sessionLength.PoliceSessionCount != 0)
                                PoliceSessionCount = sessionLength.PoliceSessionCount;
                            if (sessionLength.HaulierSessionCount != 0)
                                HaulierSessionCount = sessionLength.HaulierSessionCount;
                        }

                    }
                    SessionLengthModel sessModel = new SessionLengthModel
                    {
                        StartDate = dates,
                        SOASessionCount = SOASessionCount,
                        PoliceSessionCount = PoliceSessionCount,
                        HaulierSessionCount = HaulierSessionCount
                    };
                    lstNewSessionLengthModel.Add(sessModel);
                }
                return lstNewSessionLengthModel;
            }
            else
                return listSessionLength;
        }
        #endregion

        #region GetPeriodicSessionLengthHistory
        public static List<SessionLengthModel> GetPeriodicSessionLengthHistory(int pageNumber, int pageSize, int month, int year, int userTypeId)
        {
            List<SessionLengthModel> listSessionLength = new List<SessionLengthModel>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listSessionLength,
               UserSchema.Portal + ".GET_PERIODIC_SESS_LEN_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("MON", month, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("YR", year, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USER_TYPE_ID", userTypeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.Organisation = records.GetStringOrDefault("orgname");
                    instance.TotalRecordCount = records.GetDecimalOrDefault("TOTALRECORDCOUNT");
                    instance.TotalSessionInMonth = records.GetDecimalOrDefault("totalsesion");
                }
            );

            List<SessionLengthModel> listPeakHrSessionMonth = GetPeakHourSessionInMonthOrganizationWise(month, year);

            if ((listSessionLength.Count > 0) && (listPeakHrSessionMonth != null))
            {
                for (var sessCount = 0; sessCount < listSessionLength.Count; sessCount++)
                {

                    var PeakSessionList = (from x in listPeakHrSessionMonth where x.UserTypeID == userTypeId && x.Organisation == listSessionLength[sessCount].Organisation orderby x.NoofSessionInPeakDay descending select x).ToList();
                    if ((PeakSessionList.Count > 0) && (PeakSessionList.Any()))
                    {
                        var PeakSessionDetails = PeakSessionList.FirstOrDefault();
                        listSessionLength[sessCount].NoofSessionInPeakDay = (PeakSessionDetails != null) ? PeakSessionDetails.NoofSessionInPeakDay : 0;
                    }
                }
            }

            return listSessionLength;
        }
        #endregion

        #region GetPeakHourSessionInMonthOrganizationWise
        public static List<SessionLengthModel> GetPeakHourSessionInMonthOrganizationWise(int month, int year)
        {

            List<SessionLengthModel> listSessionLength = new List<SessionLengthModel>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listSessionLength,
               UserSchema.Portal + ".GET_PEAK_HOUR_SESS_MONTH_ORG",
                parameter =>
                {
                    parameter.AddWithValue("MON", month, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("YR", year, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.Organisation = records.GetStringOrDefault("ORGNAME");
                        instance.UserTypeID = records.GetInt32OrDefault("user_type_id");
                        instance.SessionDuration = (double)records.GetDecimalOrDefault("Hrs");
                        instance.NoofSessionInPeakDay = records.GetDecimalOrDefault("totalsession");
                    }
            );
            return listSessionLength;
        }
        #endregion

        #region GetAllPeriodicSessionLengthHistory
        public static List<SessionLengthModel> GetAllPeriodicSessionLengthHistory(int month, int year, int userTypeId)
        {
            List<SessionLengthModel> listSessionLength = new List<SessionLengthModel>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listSessionLength,
               UserSchema.Portal + ".GET_ALL_PERIODIC_SESS_LEN_DTLS",
                parameter =>
                {
                    parameter.AddWithValue("MON", month, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("YR", year, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USER_TYPE_ID", userTypeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.Organisation = records.GetStringOrDefault("orgname");
                    instance.TotalRecordCount = records.GetDecimalOrDefault("TOTALRECORDCOUNT");
                    instance.TotalSessionInMonth = records.GetDecimalOrDefault("totalsesion");
                }
            );
            return listSessionLength;
        }
        #endregion

        #region GetCommunicationHistory
        public static List<CommunicationModel> GetCommunicationHistory(int startMonth, int startYear)
        {
            List<CommunicationModel> listCommunication = new List<CommunicationModel>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listCommunication,
               UserSchema.Portal + ".GET_COMM_HISTORYDETAILS",
                parameter =>
                {
                    parameter.AddWithValue("MON", startMonth, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("YR", startYear, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.StartDate = records.GetDateTimeOrDefault("START_DATE");
                        if (records.GetStringOrDefault("COMMTYPE") == "EMAIL")
                            instance.EmailSent = records.GetDecimalOrDefault("totalcount");
                        if (records.GetStringOrDefault("COMMTYPE") == "FAX")
                            instance.FaxSent = records.GetDecimalOrDefault("totalcount");
                        if (records.GetStringOrDefault("COMMTYPE") == "INBOX")
                            instance.InboxSent = records.GetDecimalOrDefault("totalcount");
                    }
            );
            List<DateTime> DateList = listCommunication.Select(x => x.StartDate).Distinct().ToList();

            List<CommunicationModel> lstCommunicationModel = new List<CommunicationModel>();
            foreach (DateTime date in DateList)
            {
                DateTime dates = DateTime.Now;
                decimal EmailCount = 0;
                decimal FaxCount = 0;
                decimal InobxCount = 0;
                foreach (CommunicationModel comm in listCommunication)
                {
                    if (comm.StartDate == date)
                    {
                        dates = comm.StartDate;
                        if (comm.EmailSent != 0)
                            EmailCount = comm.EmailSent;
                        if (comm.FaxSent != 0)
                            FaxCount = comm.FaxSent;
                        if (comm.InboxSent != 0)
                            InobxCount = comm.InboxSent;
                    }

                }
                CommunicationModel commModel = new CommunicationModel();
                commModel.StartDate = dates;
                commModel.EmailSent = EmailCount;
                commModel.FaxSent = FaxCount;
                commModel.InboxSent = InobxCount;
                lstCommunicationModel.Add(commModel);
            }
            return lstCommunicationModel;
        }
        #endregion

        #region GetIndustryLiaisonHistory
        public static IndustryLiaisonModel GetIndustryLiaisonHistory(int startMonth, int startYear)
        {
            IndustryLiaisonModel industryLiaison = new IndustryLiaisonModel();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                industryLiaison,
               UserSchema.Portal + ".GET_INDUSTRYLIAISON_HISTORY",
                parameter =>
                {
                    parameter.AddWithValue("MON", startMonth, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("YR", startYear, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {

                        instance.NoofRegEnabledHauAcc = records.GetDecimalOrDefault("REG_ENA_HAU_ACC_COUNT");
                        instance.NoofRegHaulierOrganisation = records.GetDecimalOrDefault("REG_HAU_ORG_COUNT");

                        instance.NoofHaulierSession = records.GetDecimalOrDefault("HAU_AC_COUNT");

                        instance.NoofPlanRouteSO = records.GetDecimalOrDefault("PLAN_ROU_SO_COUNT");
                        instance.NoofPlanRouteVR1 = records.GetDecimalOrDefault("PLAN_ROU_VR1_COUNT");
                        instance.NoofPlanRouteNotification = records.GetDecimalOrDefault("PLAN_ROU_NOTI_COUNT");

                        instance.NoofRegEnabledPoliceAcc = records.GetDecimalOrDefault("REG_ENA_POL_ACC_COUNT");
                        instance.NoofRegPoliceOrg = records.GetDecimalOrDefault("REG_POL_ORG_COUNT");
                        instance.NoofPoliceSession = records.GetDecimalOrDefault("POLICE_AC_COUNT");
                        instance.NoPolicePrefInboxOnly = records.GetDecimalOrDefault("POL_PREF_INB_COUNT");

                        instance.NoofRegSOAAcc = records.GetDecimalOrDefault("REG_SOA_ACC_COUNT");
                        instance.NoofRegSOAOrg = records.GetDecimalOrDefault("REG_SOA_ORG_COUNT");
                        instance.NoofSOASession = records.GetDecimalOrDefault("SOA_AC_COUNT");
                        instance.NoSOAPrefInboxOnly = records.GetDecimalOrDefault("SOA_PREF_INB_COUNT");

                        instance.SOAppSubEnterByHauForSO = records.GetDecimalOrDefault("SO_APP_SUB_ENTERBYHAUFORSO");
                        instance.SOAppSubEnterByHauForVR1 = records.GetDecimalOrDefault("SO_APP_SUB_ENTERBYHAUFORVR1");
                        instance.SOAppSubEnterBySortForSO = records.GetDecimalOrDefault("SO_APP_SUB_ENTERBYSORTFORSO");
                        instance.SOAppSubEnterBySortForVR1 = records.GetDecimalOrDefault("SO_APP_SUB_ENTERBYSORTFORVR1");

                        instance.SOProcssedAgreed = records.GetDecimalOrDefault("SO_PROCESSED_AGREED");
                        instance.SOProcssedDeclined = records.GetDecimalOrDefault("SO_PROCESSED_DECLINED");
                        instance.SOProcssedWithdrawn = records.GetDecimalOrDefault("SO_PROCESSED_WITHDRAWN");

                        instance.NoPropoAgreedSentEmail = records.GetDecimalOrDefault("NO_PROPO_AGREED_EMAIL");
                        instance.NoPropoAgreedSentFax = records.GetDecimalOrDefault("NO_PROPO_AGREED_FAX");
                        instance.NoPropoAgreedSentInbox = records.GetDecimalOrDefault("No_PROPO_AGREED_INBOX");

                        instance.SpecialOrders = records.GetDecimalOrDefault("SPECIAL_ORDERS");

                        instance.STGOCategory1 = records.GetDecimalOrDefault("STGO_CATEGORY1");
                        instance.STGOCategory2 = records.GetDecimalOrDefault("STGO_CATEGORY2");
                        instance.STGOCategory3 = records.GetDecimalOrDefault("STGO_CATEGORY3");

                        instance.MobileCraneCategoryA1 = records.GetDecimalOrDefault("MOBILE_CRANE_CATEGORY1");
                        instance.MobileCraneCategoryA2 = records.GetDecimalOrDefault("MOBILE_CRANE_CATEGORY2");
                        instance.MobileCraneCategoryA3 = records.GetDecimalOrDefault("MOBILE_CRANE_CATEGORY3");

                        instance.CAndUNotification = records.GetDecimalOrDefault("CANDU_NOTIFICATION");
                        instance.OtherNotification = records.GetDecimalOrDefault("OTHER_NOTIFICATION");

                        instance.TotalNumberOfSOANotificationToDisplay = records.GetDecimalOrDefault("TOTAL_NO_SOA_NOTI_TOSHOW");
                        instance.TotalNumberOfPoliceNotificationToDisplay = records.GetDecimalOrDefault("TOTAL_NO_POLICE_NOTI_TOSHOW");

                        instance.TotalNumberOfSOANotificationToShow = records.GetDecimalOrDefault("TOTAL_NO_SOA_NOTI_TODISPLAY");
                        instance.TotalNumberOfPoliceNotificationToShow = records.GetDecimalOrDefault("TOTAL_NO_POLICE_NOTI_TODISPLAY");

                        instance.EmailNotification = records.GetDecimalOrDefault("EMAIL_NOTIFICATION");
                        instance.FaxNotification = records.GetDecimalOrDefault("FAX_NOTIFICATION");
                        instance.InBoxNotification = records.GetDecimalOrDefault("INBOX_NOTIFICATION");

                        instance.TotalNumberOfSOANotificationOpenToDisplay = records.GetDecimalOrDefault("TOTAL_SOA_NOTI_OPEN_DISPLAY");
                        instance.TotalNumberOfSOANotificationOpenToShow = records.GetDecimalOrDefault("TOTAL_SOA_NOTI_OPEN_SHOW");

                        instance.TotalNumberOfPoliceNotificationOpenToDisplay = records.GetDecimalOrDefault("TOTAL_POLICE_NOTI_OPEN_DISPLAY");
                        instance.TotalNumberOfPoliceNotificationOpenToShow = records.GetDecimalOrDefault("TOTAL_POLICE_NOTI_OPEN_SHOW");

                        instance.STGOOtherDetails = records.GetDecimalOrDefault("STGO_OTHER_DETAILS");
                    }
            );
            return industryLiaison;
        }
        #endregion

        #region GetReportPerUserHistory
        public static List<ReportPerUserModel> GetReportPerUserHistory(int startMonth, int startYear, int userType)
        {

            List<ReportPerUserModel> reportPerUserList = new List<ReportPerUserModel>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                reportPerUserList,
               UserSchema.Portal + ".GET_REPORTPERUSER_HISTORY",
                parameter =>
                {
                    parameter.AddWithValue("MON", startMonth, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("YR", startYear, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("USERTYPE", userType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.OrganisationName = records.GetStringOrDefault("ORGNAME");
                        instance.TotalSession = records.GetDecimalOrDefault("TOTALSESSION");
                        instance.TotalNotification = records.GetDecimalOrDefault("TOTALNOTIFICATION");
                    }
            );
            return reportPerUserList;
        }
        #endregion

        #region AddSessionLog
        public static int AddSessionLog(SessionLengthModel sessionLengthLogsModel)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                sessionLengthLogsModel,
               UserSchema.Portal + ".MANAGE_SESSION_LENGTH_LOGS",
                parameter =>
                {
                    parameter.AddWithValue("p_EVENT_TYPE", sessionLengthLogsModel.EventType, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LOGIN_ID", sessionLengthLogsModel.LoginId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_START_DATE", TimeZoneInfo.ConvertTimeToUtc(sessionLengthLogsModel.StartDate), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MACHINE_NAME", sessionLengthLogsModel.MachineName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_DESCRIPTION", sessionLengthLogsModel.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (record) =>
                    {
                        result = Convert.ToInt32(record.GetDecimalOrDefault("EVENTID"));
                    }
            );
            return result;
        }
        #endregion
    }
}