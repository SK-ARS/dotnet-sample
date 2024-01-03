using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    public class InsertQuickLinkParams
    {
        public int UserId { get; set; }

        public int ProjectId { get; set; }

        public int NotificationId { get; set; }

        public int RevisionId { get; set; }

        public int VersionId { get; set; }
    }
}