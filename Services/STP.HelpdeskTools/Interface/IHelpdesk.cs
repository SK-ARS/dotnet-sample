using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using STP.Domain.HelpdeskTools;

namespace STP.HelpdeskTools.Interface
{
    public interface IHelpdesk
    {
        List<DistributionAlerts> GetSORTDistributionAlerts(int pageNum, int pageSize, DistributionAlerts objDistributionAlert, int portalType,int? presetFilter=null,int? sortOrder=null);
    }
}