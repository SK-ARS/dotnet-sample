using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.VehiclesAndFleets.Component
{
    public class ComponentIdParams
    {
        public List<int> componentIds { get; set; }

        public string userSchema { get; set; }
        public bool boatMastFlag { get; set; }
        public int flag { get; set; }
    }
}
