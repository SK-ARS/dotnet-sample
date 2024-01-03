using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using STP.Common.Logger;
using STP.Domain.LoggingAndReporting;

namespace STP.ServiceAccess.LoggingAndReporting
{
    public class ReportService : IReportService
    {
        private readonly HttpClient httpClient;
        public ReportService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        #region GetTotalSessionLengthHistoryTypewise
        public List<SessionLengthModel> GetTotalSessionLengthHistoryTypewise(int month, int year)
        {
            List<SessionLengthModel> lstSessionLengthModel = new List<SessionLengthModel>();
            try
            {
                string urlParameters = "?month=" + month + "&year=" + year;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["LoggingAndReporting"]}" +
                                               $"/Report/GetTotalSessionLengthHistoryTypewise{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    lstSessionLengthModel = response.Content.ReadAsAsync<List<SessionLengthModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/GetTotalSessionLengthHistoryTypewise, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/GetTotalSessionLengthHistoryTypewise, Exception: {ex}");
            }
            return lstSessionLengthModel;
        }
        #endregion

        #region GetPeakHourSessionInMonth
        public List<SessionLengthModel> GetPeakHourSessionInMonth(int month, int year)
        {
            List<SessionLengthModel> lstSessionLengthModel = new List<SessionLengthModel>();
            try
            {
                string urlParameters = "?month=" + month + "&year=" + year;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["LoggingAndReporting"]}" +
                                               $"/Report/GetPeakHourSessionInMonth{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    lstSessionLengthModel = response.Content.ReadAsAsync<List<SessionLengthModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/GetPeakHourSessionInMonth, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/GetPeakHourSessionInMonth, Exception: {ex}");
            }
            return lstSessionLengthModel;
        }
        #endregion

        #region GetAvgSessionLength
        public List<SessionLengthModel> GetAvgSessionLength(int month, int year)
        {
            List<SessionLengthModel> lstSessionLengthModel = new List<SessionLengthModel>();
            try
            {
                string urlParameters = "?month=" + month + "&year=" + year;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["LoggingAndReporting"]}" +
                                               $"/Report/GetAvgSessionLength{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    lstSessionLengthModel = response.Content.ReadAsAsync<List<SessionLengthModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/GetAvgSessionLength, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/GetAvgSessionLength, Exception: {ex}");
            }
            return lstSessionLengthModel;
        }
        #endregion

        #region GetSessionLengthHistory
        public List<SessionLengthModel> GetSessionLengthHistory(int month, int year, string userType)
        {
            List<SessionLengthModel> lstSessionLengthModel = new List<SessionLengthModel>();
            try
            {
                string urlParameters = "?month=" + month + "&year=" + year + "&userType=" + userType;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["LoggingAndReporting"]}" +
                                               $"/Report/GetSessionLengthHistory{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    lstSessionLengthModel = response.Content.ReadAsAsync<List<SessionLengthModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/GetSessionLengthHistory, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/GetSessionLengthHistory, Exception: {ex}");
            }
            return lstSessionLengthModel;
        }
        #endregion

        #region GetPeriodicSessionLengthHistory
        public List<SessionLengthModel> GetPeriodicSessionLengthHistory(int pageNumber, int pageSize, int month, int year, int userTypeId)
        {
            List<SessionLengthModel> lstSessionLengthModel = new List<SessionLengthModel>();
            try
            {
                string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&month=" + month + "&year=" + year + "&userTypeId=" + userTypeId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["LoggingAndReporting"]}" +
                                               $"/Report/GetPeriodicSessionLengthHistory{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    lstSessionLengthModel = response.Content.ReadAsAsync<List<SessionLengthModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/GetPeriodicSessionLengthHistory, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/GetPeriodicSessionLengthHistory, Exception: {ex}");
            }
            return lstSessionLengthModel;
        }
        #endregion

        #region GetAllPeriodicSessionLengthHistory
        public List<SessionLengthModel> GetAllPeriodicSessionLengthHistory(int month, int year, int userTypeId)
        {
            List<SessionLengthModel> lstSessionLengthModel = new List<SessionLengthModel>();
            try
            {
                string urlParameters = "?month=" + month + "&year=" + year + "&userTypeId=" + userTypeId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["LoggingAndReporting"]}" +
                                               $"/Report/GetAllPeriodicSessionLengthHistory{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    lstSessionLengthModel = response.Content.ReadAsAsync<List<SessionLengthModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/GetAllPeriodicSessionLengthHistory, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/GetAllPeriodicSessionLengthHistory, Exception: {ex}");
            }
            return lstSessionLengthModel;
        }
        #endregion

        #region GetPeakHourSessionInMonthOrganizationWise
        public List<SessionLengthModel> GetPeakHourSessionInMonthOrganizationWise(int month, int year)
        {
            List<SessionLengthModel> listPeakHrSessionMonth = new List<SessionLengthModel>();
            try
            {
                string urlParameters = "?month=" + month + "&year=" + year;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["LoggingAndReporting"]}" +
                                               $"/Report/GetPeakHourSessionInMonthOrganizationWise{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    listPeakHrSessionMonth = response.Content.ReadAsAsync<List<SessionLengthModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/GetPeakHourSessionInMonthOrganizationWise, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/GetPeakHourSessionInMonthOrganizationWise, Exception: {ex}");
            }
            return listPeakHrSessionMonth;
        }
        #endregion

        #region GetCommunicationHistory
        public List<CommunicationModel> GetCommunicationHistory(int startMonth, int startYear)
        {
            List<CommunicationModel> lstCommunicationModel = new List<CommunicationModel>();
            try
            {
                string urlParameters = "?startMonth=" + startMonth + "&startYear=" + startYear;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["LoggingAndReporting"]}" +
                                               $"/Report/GetCommunicationHistory{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    lstCommunicationModel = response.Content.ReadAsAsync<List<CommunicationModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/GetCommunicationHistory, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/GetCommunicationHistory, Exception: {ex}");
            }
            return lstCommunicationModel;
        }
        #endregion

        #region GetIndustryLiaisonHistory
        public IndustryLiaisonModel GetIndustryLiaisonHistory(int month, int year)
        {
            IndustryLiaisonModel industryLiaisonModel = new IndustryLiaisonModel();
            try
            {
                string urlParameters = "?month=" + month + "&year=" + year;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["LoggingAndReporting"]}" +
                                               $"/Report/GetIndustryLiaisonHistory{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    industryLiaisonModel = response.Content.ReadAsAsync<IndustryLiaisonModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/GetIndustryLiaisonHistory, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/GetIndustryLiaisonHistory, Exception: {ex}");
            }
            return industryLiaisonModel;
        }
        #endregion

        #region GetReportPerUserHistory
        public List<ReportPerUserModel> GetReportPerUserHistory(int month, int year, int userType)
        {
            List<ReportPerUserModel> reportPerUserList = new List<ReportPerUserModel>();
            try
            {
                string urlParameters = "?month=" + month + "&year=" + year + "&userType=" + userType;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["LoggingAndReporting"]}" +
                                               $"/Report/GetReportPerUserHistory{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    reportPerUserList = response.Content.ReadAsAsync<List<ReportPerUserModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/GetReportPerUserHistory, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/GetReportPerUserHistory, Exception: {ex}");
            }
            return reportPerUserList;
        }
        #endregion

        #region LogEvent
        public int LogEvent(int eventType, int userId, string description)
        {
            int result = 0;
            try
            {
                LogEventParam logEventParam = new LogEventParam()
                {
                    EventType = eventType,
                    UserId = userId,
                    Description = description
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["LoggingAndReporting"]}" +
               $"/Report/LogEvent", logEventParam).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/LogEvent, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/LogEvent, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region  SaveSessionLengthLog
        public int SaveSessionLengthLog(SessionLengthModel sessionLengthModel)
        {
            int result = 0;
            try
            {  
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["LoggingAndReporting"]}" +
               $"/Report/SaveSessionLengthLog", sessionLengthModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/SaveSessionLengthLog, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Report/SaveSessionLengthLog, Exception: {ex}");
            }
            return result;
        }
        #endregion
    }
}