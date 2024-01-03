using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.LoggingAndReporting
{
    public class CommunicationModel
    {
        public Decimal EmailSent { get; set; }
        public Decimal FaxSent { get; set; }
        public Decimal InboxSent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalRecordCount { get; set; }
        public int NumericMonth { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
    }
}
