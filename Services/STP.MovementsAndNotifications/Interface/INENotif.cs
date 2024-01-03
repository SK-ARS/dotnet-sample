using STP.Domain.ExternalAPI;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.NonESDAL;
using System;
using System.Collections.Generic;

namespace STP.MovementsAndNotifications.Interface
{
    interface INENotif
    {
        NotificationGeneralDetails SaveNENotification(NENotifGeneralDetails notifGeneralDetails);
        int GetWorkingDays(DateTime moveStartDate, int countryId);
        string GenerateNENotifCode(long notificationId, int isIndemenity);
        List<NENotificationStatusOutput> GetNENotificationStatus(string ESDALReferenceNumber);
        ValidNERenotif IsNenRenotified(string prevEsdalRef);
    }
}
