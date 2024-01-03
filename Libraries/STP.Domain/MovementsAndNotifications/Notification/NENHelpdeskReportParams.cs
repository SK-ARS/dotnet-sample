using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    public class NENHelpdeskReportParams
    {
        public int Month { get; set; }

        public int Year { get; set; }

        public int NENReportCategoryAll { get; set; }

        public int NENEmailCategory { get; set; }

        public int NENInterCategory { get; set; }
    }
}