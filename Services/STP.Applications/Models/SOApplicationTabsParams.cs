using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class SOApplicationTabsParams
    {
        public string HaulierMnemonic { get; set; }

        public int EsdalRef { get; set; }

        public int RevisionNo { get; set; }

        public int VersionNo { get; set; }

        public string UserSchema { get; set; }
    }
}