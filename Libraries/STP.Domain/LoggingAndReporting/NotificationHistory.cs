using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.LoggingAndReporting
{
    public class NotificationHistory
    {
        public int NotificationVersionNumber { get; set; }
        public string NotificationDate { get; set; }
        public string NotificationCode { get; set; }
        public string ActionType { get; set; }
        public string Description { get; set; }
        public int TotalCount { get; set; }
        public DateTime NotifDate { get; set; }
    }
}
