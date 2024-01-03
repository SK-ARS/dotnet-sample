using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.RouteAssessment
{
    public class StringExtractorParams
    {
        public string XmlData { get; set; }
        public List<AssessmentContacts> ManualContactList { get; set; }
    }
    public class DeleteAffectedParams
    {
        public string XmlData { get; set; }
        public string OrganisationName { get; set; }
        public string FullName { get; set; }
    }
}
