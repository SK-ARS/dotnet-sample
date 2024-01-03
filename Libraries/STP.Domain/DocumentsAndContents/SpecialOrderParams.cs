using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using STP.Domain.Applications;

namespace STP.Domain.DocumentsAndContents
{
    public class SpecialOrderParams
    {
        public SORTSpecialOrder SpecialOrderModel { get; set; }

        public List<string> ListCoverages { get; set; }
    }
}