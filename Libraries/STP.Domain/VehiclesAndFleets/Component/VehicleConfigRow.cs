using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.VehicleAndFleets.Component
{
    public class VehicleConfigRow
    {
        private VehicleConfigRow oConfigRow;

        public VehicleConfigRow(VehicleConfigRow oConfigRow)
        {
            this.ListVehicleComp = new List<VehicleComponent>(oConfigRow.ListVehicleComp);
        }

        public List<VehicleComponent> ListVehicleComp { get; set; }
    }
}
