using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.VehiclesAndFleets
{
    public class AssessMovementTypeParams
    {
        public List<Int64> componentIds { get; set; }
        public int configurationType { get; set; }
        public bool IsMovement { get; set; }
        public string userSchema { get; set; }
    }
}
