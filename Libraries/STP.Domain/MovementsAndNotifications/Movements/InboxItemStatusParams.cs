using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.MovementsAndNotifications.Movements
{
    public class InboxItemStatusParams
    {
        public long OrganisationId { get; set; }

        public string ESDALRef { get; set; }
    }
}
