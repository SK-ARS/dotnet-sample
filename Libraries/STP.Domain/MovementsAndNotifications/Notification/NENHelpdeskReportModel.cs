using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    public class NENHelpdeskReportModel
    {
        public decimal TotalRecordCount { get; set; }

        public int NumericMonth { get; set; }

        public string Month { get; set; }

        public int Year { get; set; }

        public int NumericVehicleType { get; set; }

        public int NENReportCategoryAll { get; set; }

        public int NENEmailCategory { get; set; }

        public int NENInterCategory { get; set; }

        public string NotificationCategory { get; set; }

        public string Categories { get; set; }

        public long NENFailures { get; set; }

        public long NENSentByEmail { get; set; }

        public long NENThroughOpenInterface { get; set; }
        public long NENSentByApi { get; set; }

        public long TotalNENSubmitted { get; set; }

        public NENHelpdeskReportModel()
        {
            Categories = this.Categories;
            NENFailures = this.NENFailures;
            NENSentByEmail = this.NENSentByEmail;
            NENThroughOpenInterface = this.NENThroughOpenInterface;
            TotalNENSubmitted = this.TotalNENSubmitted;
        }
    }
}