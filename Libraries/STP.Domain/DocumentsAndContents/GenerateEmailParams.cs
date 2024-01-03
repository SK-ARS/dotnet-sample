using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.DocumentsAndContents
{
    public class GenerateEmailParams
    {
        public int NotificationId { get; set; }
        public int DocType { get; set; }
        public string XmlInformation { get; set; }
        public string XsltPath { get; set; }
        public string FileName { get; set; }
        public string ESDALReferenceNo { get; set; }
        public long OrganisationId { get; set; }
        public string DocumentFileName { get; set; }
        public bool TransmitMethodCallReq { get; set; }
        public ContactModel Contact { get; set; }
        public UserInfo UserInfo { get; set; }
        public int IcaStatus { get; set; }
        public bool Indemnity { get; set; }
        public int xmlAttach { get; set; }
        public bool ImminentMovestatus { get; set; }
        public int RoutePlanUnits { get; set; }
        public long Projectstatus { get; set; }
        public bool GenerateFlag { get; set; }
        public long Projectid { get; set; }
        public int RevisionNo { get; set; }
        public int VersionNo { get; set; }
    }
    public class GeneratePdfParam 
    {
        public int NotificationId { get; set; }
        public int DocType { get; set; }
        public string ReturnXML { get; set; }
        public string EsdalReference { get; set; }
        public string XSLTPath { get; set; }
        public ContactModel Contact { get; set; }
        public string DocFileName { get; set; }
        public int IcaStatus { get; set; }
        public bool ImminentState { get; set; }
        public UserInfo SessionInfo { get; set; }
        public long OrganisationId { get; set; }
    }
}
