using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class SortLatestAppDetails
    {
        public long ProjectID { get; set; }
        public long AppRevisionID { get; set; }
        public int AppRevisionNo { get; set; }
        public long VersionID { get; set; }
        public int VersionNo { get; set; }
        public decimal MovIDDistributed { get; set; }
    }
}