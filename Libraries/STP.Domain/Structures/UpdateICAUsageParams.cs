using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Structures
{
    public class UpdateICAUsageParams
    {
       public ManageStructureICA ICAUsage { get; set; }
       public int OrganisationId { get; set; }
       public int StructureId { get; set; }
       public int SectionId { get; set; }
    }
}
