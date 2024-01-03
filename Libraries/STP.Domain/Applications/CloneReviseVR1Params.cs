using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class CloneReviseVR1Params
    {
        public long ApplicationRevisionId { get; set; }
        public int ReducedDet { get; set; }
        public int CloneApp { get; set; }
        public int VersionId { get; set; }
        public string UserSchema { get; set; }
    }
}