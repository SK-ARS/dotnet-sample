using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Structures
{
    public class PerformStructureAssessmentParams
    {
        public List<StructuresToAssess> StructureList { get; set; }
        public int NotificationId { get; set; }
        public string MovementReferenceNumber { get; set; }
        public int AnalysisId { get; set; }
        public int RouteId { get; set; }
    }
}
