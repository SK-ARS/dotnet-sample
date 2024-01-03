using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.LoggingAndReporting
{
    public class AuditLogSearchParams
    {
        public string SearchString { get; set; }

        public int PageNo { get; set; }

        public int PageSize { get; set; }

        public int SortFlag { get; set; }

        public long OrganisationId { get; set; }

        public string SearchType { get; set; }
    }
}