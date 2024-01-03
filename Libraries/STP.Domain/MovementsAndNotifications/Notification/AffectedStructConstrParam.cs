using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    public class AffectedStructConstrParam
    {
        public List<long> AffectedSections { get; set; }
        public List<string> AffectedConstraints { get; set; }
        public long NotificationId { get; set; }
    }
}