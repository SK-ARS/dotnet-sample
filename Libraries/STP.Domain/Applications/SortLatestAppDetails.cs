using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class SORTLatestAppDetails
    {
        public long ProjectId { get; set; }
        public long ApplicationRevisionId { get; set; }
        public int ApplicationRevisionNo { get; set; }
        public long VersionId { get; set; }
        public int VersionNo { get; set; }
        public decimal MovIsDistributed { get; set; }
    }
}