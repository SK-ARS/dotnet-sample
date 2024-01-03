using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class MovementHistory
    {
        public int NotificationVersionNumber { get; set; }
        public string NotificationDate { get; set; }
        public int LatestRevisionNumber { get; set; }
        public string ActionType { get; set; }       
        public string Description { get; set; }
        public int TotalCount { get; set; }
    }
}