using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Structures
{
    public class StructuresAssessment
    {
        public long SequenceNumber { get; set; }
        public string MovementReference { get; set; }
        public Nullable<long> RoutePartId { get; set; }
        public Nullable<System.DateTime> RequestTimestamp { get; set; }
        public Nullable<long> AnalysisId { get; set; }
        public byte[] Structures { get; set; }
        public byte[] Vehicles { get; set; }
        public Nullable<System.DateTime> ResponseTimestamp { get; set; }
        public Nullable<int> PortalSchema { get; set; }
        public byte[] EsdalStructure { get; set; }
    }
}
