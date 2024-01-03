using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.VehiclesAndFleets.Component
{
    public class ComponentIdList
    {
        public int ComponentId { get; set; }
    }
    public class ComponentTypeList
    {
        public int ComponentId { get; set; }
        public int ComponentTypeId { get; set; }
        public int ComponentSubTypeId { get; set; }
    }
}
