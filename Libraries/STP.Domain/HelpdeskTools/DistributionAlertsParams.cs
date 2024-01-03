using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.HelpdeskTools
{
    public class DistributionAlertsParams
    {
        public int PageNo { get; set; }

        public int PageSize { get; set; }

        public DistributionAlerts ObjDistributionAlert { get; set; }

        public int PortalType { get; set; }
        public int PresetFilter { get; set; }
        public int SortOrder { get; set; }

    }
}