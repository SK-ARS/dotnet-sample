using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.VehiclesAndFleets.Models.Configuration
{
    public class ImportVehicleListModel
    {
        public long configId { get; set; }
        public string userSchema { get; set; }
        public int applnRev { get; set; }
        public bool isNotif { get; set; }
        public bool isVR1 { get; set; }
        public string ContentRefNo { get; set; }
        public int IsCandidate { get; set; }
        public string VersionType { get; set; }
     

    }
}