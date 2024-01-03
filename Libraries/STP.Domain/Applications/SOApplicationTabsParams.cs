using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class SOApplicationTabsParams
    {
        public string HaulierMnemonic { get; set; }

        public int ESDALReference { get; set; }

        public int RevisionNo { get; set; }

        public int VersionNo { get; set; }

        public string UserSchema { get; set; }
    }
}