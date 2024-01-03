using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    public class NENSOAReportModel
    {
        public string Month { get; set; }

        public int Year { get; set; }

        public int ReportRecieved { get; set; }

        public int ReportAccepted { get; set; }

        public int ReportRejected { get; set; }

        public int SentforFurtherAssessment { get; set; }

        public int NoActionTaken { get; set; }

        public decimal TotalRecordCount { get; set; }
    }
}