using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.SecurityAndUsers.Models
{
        public class ViewOrganisationByID
        {
            public double OrgId { get; set; }
            public string OrgName { get; set; }
            public string HA_Contact { get; set; }
            public string OrgType { get; set; }
            public string OrgCode { get; set; }
            public string LICENSE_NR { get; set; }
            public string AddressLine_1 { get; set; }
            public string AddressLine_2 { get; set; }
            public string AddressLine_3 { get; set; }
            public string AddressLine_4 { get; set; }
            public string AddressLine_5 { get; set; }
            public string PostCode { get; set; }
            public string CountryID { get; set; }
            public string Phone { get; set; }
            public string Web { get; set; }
            public string Fax { get; set; }
            public string EmailId { get; set; }
            public bool IsNENsReceive { get; set; }
        }
    }
