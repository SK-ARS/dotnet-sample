using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class SORTUserList
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public long ContactID { get; set; }
        public string UserTypeID { get; set; }
    }
}