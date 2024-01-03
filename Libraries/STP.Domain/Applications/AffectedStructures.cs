using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Applications
{
    public class AffectedStructures
    {
        public long RoutePartID { get; set; }
        public int RoutePartNo { get; set; }
        public string RoutePartName { get; set; }
        public int RoutePart { get; set; }
        public long StructureId { get; set; }
        public string StructureCode { get; set; }
        public string StructureName { get; set; }
        public string StructureType { get; set; }
        public string TypeSuitability { get; set; }
        public long PartId { get; set; }
        public int PartNo { get; set; }
        public string PartName { get; set; }
        public string PartNarrative { get; set; }
        public string RouteType { get; set; }
    }
}
