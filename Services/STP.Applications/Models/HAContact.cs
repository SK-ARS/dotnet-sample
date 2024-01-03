using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class HAContact
    {
        public string ContactName { get; set; }
        public string HAAddress1 { get; set; }
        public string HAAddress2 { get; set; }
        public string HAAddress3 { get; set; }
        public string HAAddress4 { get; set; }
        public string HAAddress5 { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string OrgName { get; set; }
        public string Title { get; set; }

        public byte[] ContactDetails { get; set; }
    }
}