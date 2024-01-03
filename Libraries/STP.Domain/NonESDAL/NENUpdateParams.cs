using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.NonESDAL
{
    public partial class NENUpdateParams
    {
        public int InboxId { get; set; }
        public int UserId { get; set; }
        public int RouteId { get; set; }
        public int RouteStatus { get; set; }
        public long OrganisationId { get; set; }

    }
    public partial class UpdateNENICAStatusParams
    {
        public int InboxId { get; set; }
        public int IcaStatus { get; set; }
        public long OrganisationId { get; set; }
        public long NotificationId { get; set; }
        public string UserSchema { get; set; }

    }
}
