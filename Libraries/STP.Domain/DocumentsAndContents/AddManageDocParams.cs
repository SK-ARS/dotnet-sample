using STP.Domain.LoggingAndReporting;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.DocumentsAndContents
{
    public class AddManageDocParams
    {
        public OutboundDocuments OutboundDocuments { get; set; }
        public string UserSchema { get; set; }
    }

    public class GenerateMovementParams
    {
        public UserInfo UserInfo { get; set; }
        public string ESDALReference { get; set; }
        public MovementActionIdentifiers Movactiontype { get; set; }
        public int MovFlagVar { get; set; }
        public NotificationContacts NotificationContacts { get; set; }
    }

    public class InsertCollabParams
    {
        public OutboundDocuments OutboundDocuments { get; set; }
        public long DocumentId { get; set; }
        public string UserSchema { get; set; }
        public int Status { get; set; }
    }

    public class GetOutboundParams
    {
        public Enums.PortalType PortalType { get; set; }
        public int NotificationID { get; set; }
        public bool IsHaulier { get; set; }
        public int ContactID { get; set; }
    }
}