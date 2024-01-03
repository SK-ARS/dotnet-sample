using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.VehiclesAndFleets.Configuration
{
    public class ImportVehicleListModel
    {
        public long ConfigurationId { get; set; }
        public string UserSchema { get; set; }
        public int ApplicationRevisionId { get; set; }
        public bool IsNotif { get; set; }
        public bool IsVR1 { get; set; }
        public string ContentRefNo { get; set; }
        public int IsCandidate { get; set; }
        public string VersionType { get; set; }
    }
}