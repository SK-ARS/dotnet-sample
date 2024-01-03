using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class UpdateSOApplicationParams
    {
        public SOApplication SOApplication { get; set; }
        public int OrganisationId { get; set; }
        public int UserId { get; set; }
        public long ApplicationRevisionId { get; set; }
        public string userSchema { get; set; }
    }
}