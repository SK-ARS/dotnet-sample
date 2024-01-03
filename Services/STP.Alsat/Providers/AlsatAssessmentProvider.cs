using STP.Alsat.Interface;
using STP.Alsat.Persistance;
using STP.Domain.RouteAssessment.AssessmentInput;
using STP.Domain.RouteAssessment.AssessmentOutput;
using STP.Domain.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace STP.Alsat.Providers
{
    public class AlsatAssessmentProvider:IAlsatAssessmentProvider
    {
        #region Singleton
        private AlsatAssessmentProvider()
        {

        }

        public static AlsatAssessmentProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }

        /// <summary>
        /// Not to be called while using logic
        /// </summary>
        internal static class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly AlsatAssessmentProvider instance = new AlsatAssessmentProvider();
        }

        #region Logger instance

        private const string PolicyName = "UserProvider";

        #endregion
        #endregion

        public AssessmentResponse GetAssessment(int sequenceNumber)
        {
            return AlsatAssessmentDAO.GetAssessment(sequenceNumber);
        }
        public StructuresAssessment PutAssessmentResult(AssessmentOutput assessmentOutput)
        {
            return AlsatAssessmentDAO.PutAssessmentResult(assessmentOutput);
        }

    }
}