using System;
using System.Collections.Generic;
using System.Diagnostics;
using STP.Domain.LoggingAndReporting;
using STP.LoggingAndReporting.Interface;
using STP.LoggingAndReporting.Persistance;

namespace STP.LoggingAndReporting.Providers
{
    public class ReportProvider : IReport
    {
        #region ReportProvider Singleton
        public static ReportProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }
        internal static class Nested
        {
            static Nested()
            {
            }
            internal static readonly ReportProvider instance = new ReportProvider();
        }
        #endregion

        #region GetTotalSessionLengthHistoryTypewise
        public List<SessionLengthModel> GetTotalSessionLengthHistoryTypewise(int month, int year)
        {
            return ReportDAO.GetTotalSessionLengthHistoryTypewise(month, year);
        }
        #endregion

        #region GetPeakHourSessionInMonth
        public List<SessionLengthModel> GetPeakHourSessionInMonth(int month, int year)
        {
            return ReportDAO.GetPeakHourSessionInMonth(month, year);
        }
        #endregion

        #region GetAvgSessionLength
        public List<SessionLengthModel> GetAvgSessionLength(int month, int year)
        {
            return ReportDAO.GetAvgSessionLength(month, year);
        }
        #endregion

        #region GetSessionLengthHistory
        public List<SessionLengthModel> GetSessionLengthHistory(int month, int year, string userType)
        {
            return ReportDAO.GetSessionLengthHistory(month, year, userType);
        }
        #endregion

        #region GetPeriodicSessionLengthHistory
        public List<SessionLengthModel> GetPeriodicSessionLengthHistory(int pageNumber, int pageSize, int month, int year, int userTypeId)
        {
            return ReportDAO.GetPeriodicSessionLengthHistory(pageNumber, pageSize, month, year, userTypeId);
        }
        #endregion

        #region GetAllPeriodicSessionLengthHistory
        public List<SessionLengthModel> GetAllPeriodicSessionLengthHistory(int month, int year, int userTypeId)
        {
            return ReportDAO.GetAllPeriodicSessionLengthHistory(month, year, userTypeId);
        }
        #endregion

        #region GetPeakHourSessionInMonthOrganizationWise
        public List<SessionLengthModel> GetPeakHourSessionInMonthOrganizationWise(int month, int year)
        {
            return ReportDAO.GetPeakHourSessionInMonthOrganizationWise(month, year);
        }
        #endregion

        #region GetCommunicationHistory
        public List<CommunicationModel> GetCommunicationHistory(int startMonth, int startYear)
        {
            return ReportDAO.GetCommunicationHistory(startMonth, startYear);
        }
        #endregion

        #region GetIndustryLiaisonHistory
        public IndustryLiaisonModel GetIndustryLiaisonHistory(int startMonth, int startYear)
        {
            return ReportDAO.GetIndustryLiaisonHistory(startMonth, startYear);
        }
        #endregion

        #region GetReportPerUserHistory
        public List<ReportPerUserModel> GetReportPerUserHistory(int startMonth, int startYear, int userType)
        {
            return ReportDAO.GetReportPerUserHistory(startMonth, startYear, userType);
        }
        #endregion

        #region LogEvent
        public int LogEvent(int eventType, int userId, string description)
        {
            SessionLengthModel sessionLengthModel = new SessionLengthModel()
            {
                EventType = eventType,
                StartDate = DateTime.Now,
                LoginId = userId,
                MachineName = System.Environment.MachineName,
                Description = description
            };
            return ReportDAO.AddSessionLog(sessionLengthModel);
        }
        #endregion

        #region SaveSessionLengthLog
        public int SaveSessionLengthLog(SessionLengthModel sessionLengthModel)
        {
            return ReportDAO.AddSessionLog(sessionLengthModel);
        }
        #endregion
    }
}