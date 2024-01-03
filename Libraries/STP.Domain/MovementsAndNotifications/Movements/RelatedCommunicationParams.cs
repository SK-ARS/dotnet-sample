using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Movements
{
    public class RelatedCommunicationParams
    {
        public string NotificationCode { get; set; }

        public string Route { get; set; }

        public long OrganisationId { get; set; }

        public long ProjectId { get; set; }
    }
}