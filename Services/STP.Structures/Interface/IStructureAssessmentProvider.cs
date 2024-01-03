using STP.Domain.Structures;
using STP.Domain.Structures.StructureJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Structures.Interface
{
    interface IStructureAssessmentProvider
    {
        EsdalStructureJson GetStructureAssessmentCount(string ESRN, long routePartId);
        int PerformAssessment(List<StructuresToAssess> stuctureList, int notificationId, string movementReferenceNumber, int analysisId, int routeId);

    }
}
