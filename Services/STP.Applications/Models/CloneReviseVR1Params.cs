using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class CloneReviseVR1Params
    {
        public long apprevId { get; set; }
        public int reducedDet { get; set; }
        public int cloneApp { get; set; }
        public int versionId { get; set; }
        public string userSchema { get; set; }
    }
}