using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Structures
{
    public class StructureListParams
    {
        public int OrganisationId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public SearchStructures ObjSearchStructure { get; set; }
        public int presetFilter { get; set; }
        public int sortOrder { get; set; }

    }
    public class OnerousVehicleListParams
    {
        public string StructureId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchCriteria { get; set; }
        public string SearchStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int StatusCount { get; set; }
        public int Sort { get; set; }
        public long OrganisationId { get; set; }
    }
}