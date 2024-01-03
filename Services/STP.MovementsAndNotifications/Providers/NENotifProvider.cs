using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.NonESDAL;
using STP.MovementsAndNotifications.Interface;
using STP.MovementsAndNotifications.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace STP.MovementsAndNotifications.Providers
{
    public class NENotifProvider : INENotif
    {
        #region NENNotificationProvider Singleton

        private NENotifProvider()
        {
        }
        public static NENotifProvider Instance
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
            internal static readonly NENotifProvider instance = new NENotifProvider();
        }
        #endregion

        public NotificationGeneralDetails SaveNENotification(NENotifGeneralDetails notifGeneralDetails)
        {
            return NENotifDao.SaveNENotification(notifGeneralDetails);
        }
        public string GenerateNENotifCode(long notificationId, int isIndemenity)
        {
            return NENotifDao.GenerateNENotifCode(notificationId, isIndemenity);
        }
        public int GetWorkingDays(DateTime moveStartDate, int countryId)
        {
            return NENotifDao.GetWorkingDays(moveStartDate, countryId);
        }
        public List<NENotificationStatusOutput> GetNENotificationStatus(string ESDALReferenceNumber)
        {
            return NENotifDao.GetNENotificationStatus(ESDALReferenceNumber);
        }

        public ValidNERenotif IsNenRenotified(string prevEsdalRef)
        {
            return NENotifDao.IsNenRenotified(prevEsdalRef);
        }
    }
}