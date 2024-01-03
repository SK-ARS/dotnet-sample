using STP.Domain.MovementsAndNotifications.Movements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    public class UpdateDispensationParams
    {
        public DispensationGridList RegisterDispensation { get; set; }
        public int UserTypeId { get; set; }
    }
}
