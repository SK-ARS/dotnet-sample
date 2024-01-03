using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static STP.Common.Enums.EnumDescriptor;

namespace STP.Common.Enums
{
    public enum RouteType
    {
        [Description("Normal Route")]
        normal_route = 922001,
        [Description("Return Route")]
        return_route = 922002,
    }
}
