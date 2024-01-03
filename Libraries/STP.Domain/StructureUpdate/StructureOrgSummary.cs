using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.StructureUpdate
{
   public class StructureOrgSummary
    {
        public long organisationId { get; set; }
        public string orgMultipleId { get; set; }

        public string organisationName { get; set; }

        public int totalRows { get; set; }
        public string totalMulRows { get; set; }
    }
}
