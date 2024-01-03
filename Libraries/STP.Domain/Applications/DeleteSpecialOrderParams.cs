using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class DeleteSpecialOrderParams
    {
        public string OrderNumber { get; set; }
        public string UserSchema { get; set; }
    }
}