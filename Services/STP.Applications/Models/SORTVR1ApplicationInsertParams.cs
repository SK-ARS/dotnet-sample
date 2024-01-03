using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class SORTVR1ApplicationInsertParams
    {        
        public ApplyForVR1 VR1Application { get; set; }
        public int OrganisationID { get; set; }
        public int UserID { get; set; }
        public long AppRevID { get; set; }
    }
}