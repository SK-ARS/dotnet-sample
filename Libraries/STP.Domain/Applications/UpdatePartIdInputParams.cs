using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Applications
{
    public class UpdatePartIdInputParams
    {
        public int VehicleId { get; set; }
        public int PartId { get; set; }
        public bool VR1Appl { get; set; }
        public bool Notif { get; set; }
        public string RType { get; set; }
        public bool Iscand { get; set; }
        public string userSchema { get; set; }       
    }
}
