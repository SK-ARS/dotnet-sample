using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class SORTVR1ApplicationInsertParams
    {        
        public ApplyForVR1 VR1Application { get; set; }
        public int OrganisationId { get; set; }
        public int UserId { get; set; }
        public long ApplicationRevisionId { get; set; }
    }
}