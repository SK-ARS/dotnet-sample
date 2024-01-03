using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.SecurityAndUsers
{
        public class ViewOrganizationByID
        {
            public double OrgId { get; set; }
            public string OrgName { get; set; }
            public string HAContact { get; set; }
            public string OrgType { get; set; }
            public string OrgCode { get; set; }
            public string LicenseNR { get; set; }
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string AddressLine3 { get; set; }
            public string AddressLine4 { get; set; }
            public string AddressLine5 { get; set; }
            public string PostCode { get; set; }
            public string CountryId { get; set; }
            public string Phone { get; set; }
            public string Web { get; set; }
            public string Fax { get; set; }
            public string EmailId { get; set; }
            public bool IsNENsReceive { get; set; }
            public bool AccessToALSAT { get; set; }
            public string AuthenticationKey { get; set; }
    }
    }
