using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.NonESDAL
{
    public class InboxEditRouteParams
    {
        public int InboxId { get; set; }
        public long NENId { get; set; }
        public int NotificationId { get; set; }
        public int NewUserId { get; set; }
        public long EditedRouteId { get; set; }
        public long NewRouteId { get; set; }
        public long OrganisationId { get; set; }
    }
}
