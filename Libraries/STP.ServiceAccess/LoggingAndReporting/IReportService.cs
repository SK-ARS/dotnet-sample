using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STP.Domain.LoggingAndReporting;

namespace STP.ServiceAccess.LoggingAndReporting
{
    public interface IReportService
    {
        List<SessionLengthModel> GetTotalSessionLengthHistoryTypewise(int month, int year);
        List<SessionLengthModel> GetPeakHourSessionInMonth(int month, int year);
        List<SessionLengthModel> GetAvgSessionLength(int month, int year);
        List<SessionLengthModel> GetSessionLengthHistory(int month, int year, string userType);
        List<SessionLengthModel> GetPeriodicSessionLengthHistory(int pageNumber, int pageSize, int month, int year, int userTypeId);
        List<SessionLengthModel> GetAllPeriodicSessionLengthHistory(int month, int year, int userTypeId);
        List<SessionLengthModel> GetPeakHourSessionInMonthOrganizationWise(int month, int year);
        List<CommunicationModel> GetCommunicationHistory(int startMonth, int startYear);
        IndustryLiaisonModel GetIndustryLiaisonHistory(int month, int year);
        List<ReportPerUserModel> GetReportPerUserHistory(int month, int year, int userType);
        int LogEvent(int eventType, int userId, string description);
        int SaveSessionLengthLog(SessionLengthModel sessionLengthModel);
    }
}
