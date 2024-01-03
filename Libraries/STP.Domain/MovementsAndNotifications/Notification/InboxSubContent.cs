using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    public class InboxSubContent
    {
        //For Client Description
        public string Description { get; set; }

        //For Start and End Location
        public string FromSummary { get; set; }
        public string ToSummary { get; set; }

        //For SubContentList
        public int NotificationVersionNum { get; set; }
        public int VersionNum { get; set; }
        public int NotificationStatus { get; set; }
        public string ESDALReference { get; set; }
        public string NotificationDate { get; set; }
        public int? CollaborationStatus { get; set; }
        public int LatestRevisionNum { get; set; }
        public string actiontype { get; set; }
        public long NotificationId { get; set; }

    }
}
