using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain.LoggingAndReporting;
using STP.LoggingAndReporting.Providers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace STP.LoggingAndReporting.Controllers
{
    public class ReportController : ApiController
    {
        #region GetTotalSessionLengthHistoryTypewise
        [HttpGet]
        [Route("Report/GetTotalSessionLengthHistoryTypewise")]
        public IHttpActionResult GetTotalSessionLengthHistoryTypewise(int month, int year)
        {
            try
            {
                List<SessionLengthModel> lstSessionLengthModel = ReportProvider.Instance.GetTotalSessionLengthHistoryTypewise(month, year);
                return Ok(lstSessionLengthModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Report/GetTotalSessionLengthHistoryTypewise, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetPeakHourSessionInMonth
        [HttpGet]
        [Route("Report/GetPeakHourSessionInMonth")]
        public IHttpActionResult GetPeakHourSessionInMonth(int month, int year)
        {
            try
            {
                List<SessionLengthModel> lstSessionLengthModel = ReportProvider.Instance.GetPeakHourSessionInMonth(month, year);
                return Ok(lstSessionLengthModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Report/GetPeakHourSessionInMonth, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetAvgSessionLength
        [HttpGet]
        [Route("Report/GetAvgSessionLength")]
        public IHttpActionResult GetAvgSessionLength(int month, int year)
        {
            try
            {
                List<SessionLengthModel> lstSessionLengthModel = ReportProvider.Instance.GetAvgSessionLength(month, year);
                return Ok(lstSessionLengthModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Report/GetAvgSessionLength, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetSessionLengthHistory
        [HttpGet]
        [Route("Report/GetSessionLengthHistory")]
        public IHttpActionResult GetSessionLengthHistory(int month, int year,string userType)
        {
            try
            {
                List<SessionLengthModel> lstSessionLengthModel = ReportProvider.Instance.GetSessionLengthHistory(month, year, userType);
                return Ok(lstSessionLengthModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Report/GetSessionLengthHistory, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetPeriodicSessionLengthHistory
        [HttpGet]
        [Route("Report/GetPeriodicSessionLengthHistory")]
        public IHttpActionResult GetPeriodicSessionLengthHistory(int pageNumber,int pageSize,int month, int year, int userTypeId)
        {
            try
            {
                List<SessionLengthModel> lstSessionLengthModel = ReportProvider.Instance.GetPeriodicSessionLengthHistory(pageNumber, pageSize, month, year, userTypeId);
                return Ok(lstSessionLengthModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Report/GetPeriodicSessionLengthHistory, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetAllPeriodicSessionLengthHistory
        [HttpGet]
        [Route("Report/GetAllPeriodicSessionLengthHistory")]
        public IHttpActionResult GetAllPeriodicSessionLengthHistory(int month, int year, int userTypeId)
        {
            try
            {
                List<SessionLengthModel> lstSessionLengthModel = ReportProvider.Instance.GetAllPeriodicSessionLengthHistory(month, year, userTypeId);
                return Ok(lstSessionLengthModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Report/GetAllPeriodicSessionLengthHistory, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetPeakHourSessionInMonthOrganizationWise
        [HttpGet]
        [Route("Report/GetPeakHourSessionInMonthOrganizationWise")]
        public IHttpActionResult GetPeakHourSessionInMonthOrganizationWise(int month, int year)
        {
            try
            {
                List<SessionLengthModel> listPeakHrSessionMonth = ReportProvider.Instance.GetPeakHourSessionInMonthOrganizationWise(month, year);
                return Ok(listPeakHrSessionMonth);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Report/GetPeakHourSessionInMonthOrganizationWise, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetCommunicationHistory
        [HttpGet]
        [Route("Report/GetCommunicationHistory")]
        public IHttpActionResult GetCommunicationHistory( int startMonth,  int startYear)
        {
            try
            {
                List<CommunicationModel> lstCommunicationModel = ReportProvider.Instance.GetCommunicationHistory(startMonth, startYear);
                return Ok(lstCommunicationModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Report/GetCommunicationHistory, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetIndustryLiaisonHistory
        [HttpGet]
        [Route("Report/GetIndustryLiaisonHistory")]
        public IHttpActionResult GetIndustryLiaisonHistory(int month, int year)
        {
            try
            {
                IndustryLiaisonModel industryLiaisonModel = ReportProvider.Instance.GetIndustryLiaisonHistory(month, year);
                return Ok(industryLiaisonModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Report/GetIndustryLiaisonHistory, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetReportPerUserHistory
        [HttpGet]
        [Route("Report/GetReportPerUserHistory")]
        public IHttpActionResult GetReportPerUserHistory(int month, int year, int userType)
        {
            try
            {
                List<ReportPerUserModel> reportPerUserList = ReportProvider.Instance.GetReportPerUserHistory(month, year, userType);
                return Ok(reportPerUserList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Report/GetReportPerUserHistory, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region LogEvent
        [HttpPost]
        [Route("Report/LogEvent")]
        public IHttpActionResult LogEvent(LogEventParam logEventParam)
        {
            try
            {
                int output = ReportProvider.Instance.LogEvent(logEventParam.EventType, logEventParam.UserId, logEventParam.Description);
                if (output > 0)
                {
                    return Content(HttpStatusCode.Created, output);
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.InsertionFailed);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Report/LogEvent, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region SaveSessionLengthLog
        [HttpPost]
        [Route("Report/SaveSessionLengthLog")]
        public IHttpActionResult SaveSessionLengthLog(SessionLengthModel sessionLengthModel)
        {
            try
            {
                int output = ReportProvider.Instance.SaveSessionLengthLog(sessionLengthModel);
                if (output > 0)
                {
                    return Content(HttpStatusCode.Created, output);
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.InsertionFailed);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Report/SaveSessionLengthLog, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
    }
}