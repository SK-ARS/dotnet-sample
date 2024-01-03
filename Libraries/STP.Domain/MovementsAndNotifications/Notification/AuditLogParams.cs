using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    public class AuditLogParams
    {
        public int? Page { get; set; }

        public int? PageSize { get; set; }

        public string NENNotificationNo { get; set; }

        public long OrganisationId { get; set; }
         public int? sortOrder { get; set; }
        public int? sortType { get; set; }
    }
}
