using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    public class NeHaulierParams
    {
        public string AuthenticationKey { get; set; }

        public string HaulierName { get; set; }

        public string OrganisationName { get; set; }

        public long NeLimit { get; set; }

        public long KeyId { get; set; }
    }
}