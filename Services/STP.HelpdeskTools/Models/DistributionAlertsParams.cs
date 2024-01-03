using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.HelpdeskTools.Models
{
    public class DistributionAlertsParams
    {
        public int PageNum { get; set; }

        public int PageSize { get; set; }

        public DistributionAlerts objDistributionAlert { get; set; }

        public int PortalType { get; set; }

    }
}