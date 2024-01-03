using STP.Domain.RouteAssessment.AssessmentInput;
using STP.Domain.RouteAssessment.AssessmentOutput;
using STP.Domain.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Alsat.Interface
{
    interface IAlsatAssessmentProvider
    {
        
        AssessmentResponse GetAssessment(int sequenceNumber);

        StructuresAssessment PutAssessmentResult(AssessmentOutput assessmentOutput);
    }
}
