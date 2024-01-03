using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class SupplimentaryInfoParams
    {
        public SupplimentaryInfo supplimentaryInfo { get; set; }

        public string UserSchema { get; set; }

        public int ApprevId { get; set; }
    }
}