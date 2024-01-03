using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.VehiclesAndFleets.External
{
    public class VehicleListDetails
    {
        public  int TotalRecords { get; set; }
        public int NumberOfPages { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<Vehicle> Vehicles { get; set; }
    }
    public class Vehicle
    {
        public long VehicleId { get; set; }
        public string Name { get; set; }
        public string MovementClassification { get; set; }
        public string VehicleType { get; set; }

    }
}
