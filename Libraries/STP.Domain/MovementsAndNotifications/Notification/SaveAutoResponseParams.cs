using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    public class SaveAutoResponseParams
    {
        public string LogMessage { get; set; }
        public int UserId { get; set; }
        public long OrganisationId { get; set; }
    }
}
