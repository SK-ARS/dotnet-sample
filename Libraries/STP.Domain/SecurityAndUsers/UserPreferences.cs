using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.SecurityAndUsers
{
    public class UserPreferences
    {
        public int VehicleUnits { get; set; }
        public int RouteplanUnit { get; set; }
        public bool IsEnable { get; set; }
        public int MaxListItems { get; set; }
        public long CommonMethod { get; set; }
        public string EmailText { get; set; }
        public string FaxNumber { get; set; }
        public bool IsXMLAttached { get; set; }
        public long SubCommonMethod { get; set; } 
    }
    public class AutoMailResponse
    {
        public int ResponseId { get; set; }
        public int UserId { get; set; }
        public int OrganisationId { get; set; }
        public bool EnableAutoResponse { get; set; }
        public Byte[] ReplyMailText { get; set; }
        public string ReplyMailPdf { get; set; }
        public string EmailID { get; set; }
    }
}
