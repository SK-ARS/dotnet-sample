using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class ApplyForVR1Params
    {
        public string UserSchema { get; set; }

        public long RevisionId { get; set; }

        public long VersionId { get; set; }

        public long OrganisationId { get; set; }
    }
}