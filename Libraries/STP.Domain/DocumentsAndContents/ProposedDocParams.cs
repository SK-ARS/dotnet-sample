using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.DocumentsAndContents
{
    public class ProposedDocParams
    {
        public string EsdalReferenceNo { get; set; }
        public int OrganisationId { get; set; }
        public int ContactId { get; set; }
        public string UserSchema { get; set; }
        public UserInfo SessionInfo { get; set; }
    }
}