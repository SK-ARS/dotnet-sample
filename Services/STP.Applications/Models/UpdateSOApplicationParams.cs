using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class UpdateSOApplicationParams
    {
        public SOApplication SOApplication { get; set; }
        public int OrganisationId { get; set; }
        public int UserId { get; set; }
        public long ApplicationRevId { get; set; }
    }
}