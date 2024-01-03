using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.DocumentsAndContents
{
    public class AffectedStructureParams
    {
        public string XMLInformation { get; set; }
        public string ESDALReferenceNo { get; set; }
        public UserInfo SessionInfo { get; set; }
        public string UserSchema { get; set; }
        public string Type { get; set; }
        public int OrganisationId { get; set; }
    }
}