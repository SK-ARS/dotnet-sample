using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Routes.TempModelsMigrations
{
    public class RouteAnalysisModel
    {
        public int NotificationId { get; set; }
        public int RevisionId { get; set; }
        public int AnalysisId { get; set; }
        public string Reference { get; set; }
        public int VersionId { get; set; }
        public int IsCandidate { get; set; }
    }

    public class InputBlobModel
    {
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public string UserSchema { get; set; }
    }
}
