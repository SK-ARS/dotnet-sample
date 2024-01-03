using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.RouteAssessment
{
    public class StructuresModel
    {
        public bool IsConstrained { get; set; }

        public bool IsInFailedDelegation { get; set; }

        public bool IsMyResponsibility { get; set; }

        public bool IsRetainNotificationOnly { get; set; }

        public int StructureSectionId { get; set; }

        public string TraversalType { get; set; }

        public int OrganisationId { get; set; }

        public string OrganisationName { get; set; }

        public string ESRN { get; set; }

        public string Name { get; set; }

        public Byte[] AffectedStructures { get; set; }

        public List<Appraisal> ListAppraisal;
    }

    public class Appraisal
    {
        public int OrganisationId { get; set; }

        public string Suitability { get; set; }

        public string Organisation { get; set; }

        public string SectionSuitability { get; set; }

        public string ChildSectionSuitability { get; set; }

        public string ChildSectionTestClass { get; set; }

        public string ChildSectionTestIdentity { get; set; }

        public string ChildSectionResultDetails { get; set; }
    }
}