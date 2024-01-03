using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class CloneApplicationRTPartsInsertParams
    {        
        public long OldRevisionID { get; set; }
        public long RTRevisionID { get; set; }
    }
}