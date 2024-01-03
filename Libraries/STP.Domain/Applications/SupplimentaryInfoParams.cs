using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class SupplimentaryInfoParams
    {
        public SupplimentaryInfo SupplimentaryInfo { get; set; }

        public string UserSchema { get; set; }

        public int ApplicationRevisionId { get; set; }
    }
}