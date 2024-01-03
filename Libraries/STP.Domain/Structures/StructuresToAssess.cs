using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Structures
{
   public class StructuresToAssess
    {
        //constructor
        public StructuresToAssess()
        {
        }

        public string ESRN { get; set; }
        public int SectionId { get; set; }
        public long RouteId { get; set; }
        public int RoutePartNo { get; set; }
    }

}
