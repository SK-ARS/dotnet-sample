using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class AllocateSORTUserInsertParams
    {
        public long ProjectID { get; set; }
        public long PlannerUserID { get; set; }
        public string DueDate { get; set; }
        public int RevisionNumber { get; set; }
    }
}