using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.DocumentsAndContents
{
    public class GeneratePdfParams
    {
        public int NotificationID { get; set; }
        public int DocType { get; set; }
        public string XMLInformation { get; set; }
        public string FileName { get; set; }
        public string ESDALReferenceNo { get; set; }
        public long OrganisationID { get; set; }
        public int ContactID { get; set; }
        public string DocumentFileName { get; set; }
        public bool IsHaulier { get; set; }
        public string OrganisationName { get; set; }
        public string HAReference { get; set; }
        public int RoutePlanUnits { get; set; }
        public string DocumentType { get; set; }
        public UserInfo UserInfo { get; set; }
        public string UserType { get; set; }
    }
}