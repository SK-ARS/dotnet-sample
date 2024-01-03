using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.LoggingAndReporting
{
    public class ReportPerUserModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalRecordCount { get; set; }
        public int NumericMonth { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public int NumericUserType { get; set; }
        public string UserType { get; set; }
        public string OrganisationName { get; set; }
        public decimal TotalSession { get; set; }
        public decimal TotalNotification { get; set; }
    }
}
