using STP.Domain;
using STP.Domain.LoggingAndReporting;
using STP.Domain.MovementsAndNotifications.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.LoggingAndReporting
{
   public interface ILoggingService
    {
        bool SaveSysEventsMovement(int SystemEventType, string SysDescrp, int userid, string userschema);
        long SaveMovementAction(string esdalRef, int movementActionType, string movementDescription, long projectid,int revisionNo,int versionNo, string userSchema);
        List<NENAuditLogList> GetAuditListSearch(string searchString, int pageNum, int pageSize, int sortFlag, long organisationId, int searchNotificationSource, string searchType = "0", int presetFilter=1, int? sortOrder = null);
        List<NENAuditGridList> GetAuditlogNEN(int? page, int? pageSize, string NENnotificationNo, long organisationId,int? sortType=null,int? sortOrder=null);
        List<NotificationHistory> GetNotificationHistory(int pageNumber, int pageSize, long notificationNo, int sortOrder, int sortType, int historic,int userType, long projectId = 0);
    }
}
