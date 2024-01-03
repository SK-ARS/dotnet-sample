using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STP.Domain;
using STP.Domain.MovementsAndNotifications.Movements;

namespace STP.ServiceAccess.MovementsAndNotifications
{
    public interface IGenericService
    {
        List<QuickLinks> GetQuickLinksList(int UserId);
    }
}
