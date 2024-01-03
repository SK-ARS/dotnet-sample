using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class CloneRTPartsInsertParams
    {       
        public long OldRevisionID { get; set; }
        public long RtRevisionID { get; set; }
        public int Flag { get; set; }
    }
}