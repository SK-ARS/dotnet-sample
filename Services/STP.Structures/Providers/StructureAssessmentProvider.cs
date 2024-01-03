using STP.Domain.Structures;
using STP.Domain.Structures.StructureJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using STP.Structures.Persistance;
using STP.Structures.Interface;
using System.Diagnostics;

namespace STP.Structures.Providers
{
    public class StructureAssessmentProvider: IStructureAssessmentProvider
    {
        #region RouteManagerProvider Singleton

        private StructureAssessmentProvider()
        {
        }
        public static StructureAssessmentProvider Instance
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
            internal static readonly StructureAssessmentProvider instance = new StructureAssessmentProvider();
        }
        #endregion


        public EsdalStructureJson GetStructureAssessmentCount(string ESRN, long routePartId)
        {
            return StructureAssessmentDAO.GetStructureAssessmentCount(ESRN, routePartId);
        }
        public int PerformAssessment(List<StructuresToAssess> stuctureList, int notificationId, string movementReferenceNumber, int analysisId, int routeId)
        {
            return StructureAssessmentDAO.PerformAssessment(stuctureList, notificationId, movementReferenceNumber, analysisId, routeId);
        }
    }
}