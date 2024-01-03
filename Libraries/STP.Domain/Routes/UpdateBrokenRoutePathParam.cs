using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static STP.Domain.Routes.RouteModel;

namespace STP.Domain.Routes
{
    public class UpdateBrokenRoutePathParam
    {
        public List<RoutePath> RoutePathList { get; set; }
        public int IsLib { get; set; }
        public string UserSchema { get; set; }
    }
}
