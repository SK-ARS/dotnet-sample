using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    public class UpdateNotificationParam
    {
        public NotificationGeneralDetails NotificationGeneralDetails{get;set;}
        public int OrganisationId { get; set; }
        public int UserId { get; set; }
        public long NotificationId { get; set; }
    }
}