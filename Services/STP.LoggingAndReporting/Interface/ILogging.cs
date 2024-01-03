using STP.Domain;
using STP.Domain.LoggingAndReporting;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.LoggingAndReporting.Interface
{
    public interface ILogging
    {
        void InsertTransmissionInfoToAction(NotificationContacts objcontact, UserInfo userInfo, long transmissionId, string esdalRef, int actionFlag, string errMessage, string docType);
        bool SaveSysEvents(int systemEventType, string systemDescrp, int userId, string userSchema);
       
        long SaveNotifAuditLog(AuditLogIdentifiers auditLogType, string logMsg, int userId, long organisationId = 0);
        List<NENAuditLogList> GetAuditListSearch(string searchString, int pageNo, int pageSize, int sortFlag, long organisationId, string searchType,int searchNotificationSource,int presetFilter,int? sortOrder=null);
        List<NENAuditGridList> GetAuditlogNEN(int? page, int? pageSize, string NENnotificationNo, long organisationId,int? sortOrder,int? sortType);
        List<NotificationHistory> GetNotificationHistory(int pageNumber, int pageSize, long notificationNo,int sortOrder,int sortType, int historic, int userType=0, long projectId = 0);
    }
}
