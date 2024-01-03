using STP.Domain;
using STP.Domain.LoggingAndReporting;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.SecurityAndUsers;
using STP.LoggingAndReporting.Interface;
using STP.LoggingAndReporting.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace STP.LoggingAndReporting.Providers
{
    public class LoggingProvider : ILogging
    {
        #region LoggingProvider Singleton

        private LoggingProvider()
        {
        }
        public static LoggingProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }

        /// <summary>
        /// Not to be called while using logic
        /// </summary>
        internal static class Nested

        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly LoggingProvider instance = new LoggingProvider();
        }

        #region Logger instance
        private const string PolicyName = "LoggingProvider";
        #endregion


        #endregion

        /// <summary>
        ///To save SysEvents in movements.
        /// </summary>         
        /// <returns></returns>
        public void InsertTransmissionInfoToAction(NotificationContacts objcontact, UserInfo userInfo, long transmissionId, string esdalRef, int actionFlag, string errMessage, string docType)
        {
            LoggingDAO.InsertTransmissionInfoToAction(objcontact, userInfo, transmissionId, esdalRef, actionFlag, errMessage, docType);

        }

        /// <summary>
        ///To save SysEvents in movements.
        /// </summary>         
        /// <returns></returns>
        public bool SaveSysEvents(int systemEventType, string systemDescrp, int userId, string userSchema)
        {
            return LoggingDAO.SaveSysEvents(systemEventType, systemDescrp, userId, userSchema);

        }
        public long SaveMovementAction(string esdalRef, int movementActionType, string movementDescription,long projectId,int revisionNo,int versionNo, string userSchema)
        {
            return LoggingDAO.SaveMovementAction(esdalRef, movementActionType, movementDescription, projectId, revisionNo, versionNo, userSchema);

        }

        /// <summary>
        ///To save notification audit log.
        /// </summary>         
        /// <returns></returns>
        public long SaveNotifAuditLog(AuditLogIdentifiers auditLogType, string logMsg, int userId, long organisationId = 0)
        {
            return LoggingDAO.SaveNotifAuditLog( auditLogType, logMsg, userId, organisationId);
        }

        #region Get AuditList Search
        public List<NENAuditLogList> GetAuditListSearch(string searchString, int pageNo, int pageSize, int sortFlag, long organisationId, string searchType,int searchNotificationSource,int presetFilter, int? sortOrder = null)
        {
            return LoggingDAO.GetAuditListSearch(searchString, pageNo, pageSize, sortFlag, organisationId, searchType, searchNotificationSource, presetFilter, sortOrder);
        }
        #endregion

        #region Get NEN Auditlog
        public List<NENAuditGridList> GetAuditlogNEN(int? page, int? pageSize, string NENnotificationNo, long organisationId,int? sortOrder,int? sortType)
        {
            return LoggingDAO.ListNENAuditPerNotification(page, pageSize, NENnotificationNo, organisationId,sortOrder,sortType);
        }
        #endregion

        public List<NotificationHistory> GetNotificationHistory(int pageNumber, int pageSize, long notificationNo,int sortOrder,int sortType, int historic, int userType = 0, long projectId = 0)
        {
            return LoggingDAO.GetNotificationHistory(pageNumber, pageSize, notificationNo, sortOrder, sortType, historic,userType, projectId);
        }
    }
}