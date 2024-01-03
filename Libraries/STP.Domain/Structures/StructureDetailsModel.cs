using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Structures
{
    public class StructureDetailsModel
    {

        public StructureGeneralDetails StructureGeneral { get; set; }
        public StructureICA StrucureICA { get; set; }
        public StructureSummary StrucutrSummary { get; set; }
        public ImposedConstraints ImposedConstraints { get; set; }
        public DimensionConstruction DimentionConstruction { get; set; }

        public StructureICA2 SVStrucureICA { get; set; }

        public SVDataWithLoadModel svDataWithLoad { get; set; }
        public SVDataWithoutLoadModel svDataWithoutLoad { get; set; }

        public ManageStructureICA ManageICA { get; set; }
    }
}
