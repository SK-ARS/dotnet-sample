using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static STP.Domain.Routes.RouteModel;

namespace STP.Domain.Routes
{
    public class GetBrokenRouteList
    {
        public long OrganisationId { get; set; }
        public long RoutePartId { get; set; }
        public long VersionId { get; set; }
        public long AppRevisonId { get; set; }
        public long LibraryRouteId { get; set; }
        public string ConteRefNo { get; set; }
        public long CandRevisionId { get; set; }
        public string UserSchema { get; set; }
        public bool IsReplanRequired { get; set; }
        public bool IsSort { get; set; }
        public byte IsNen { get; set; }
        public bool IsViewOnly { get; set; }
    }

    public class BrokenRouteResponseModel
    {
        public bool Result { get; set; }

        public RoutePart RoutePart { get; set; }
    }
    public class BrokenRouteOutputModel
    {
        public List<BrokenRouteList> Result { get; set; }
        public int brokenRouteCount { get; set; }
        public int specialManouer { get; set; }
        public int autoReplanSuccess { get; set; }
        public int autoReplanFail { get; set; }
    }
}
