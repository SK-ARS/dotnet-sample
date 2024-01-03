using STP.Domain;
using STP.Domain.HelpdeskTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.HelpdeskTools
{
    public interface IDistributionStatusService
    {
        List<DistributionAlerts> GetDistributionAlert(int pageNumber, int? pageSize, DistributionAlerts objDistributionAlert, int portalType,int? presetFilter=null,int? sortOrder=null);
    }
}
