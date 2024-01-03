using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Movements
{
    public class QuickLinksSOA
    {
        public int NotificationId { get; set; }
        public string ESDALReferenceNo { get; set; }
        public string Route { get; set; }
        public int InboxId { get; set; }

        public int ItemStatus { get; set; }
        public long NENId { get; set; }
    }
}