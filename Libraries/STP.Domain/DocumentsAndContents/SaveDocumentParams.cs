using STP.Domain.HelpdeskTools;
using STP.Domain.LoggingAndReporting;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.DocumentsAndContents
{
    public class SaveDocumentParams
    {
        public int NotificationId { get; set; }
        public int DocType { get; set; }
        public long OrganisationId { get; set; }
        public string ESDALReferenceNo { get; set; }
        public int ContactId { get; set; }
        public byte[] ExportByteArrayData { get; set; }
        public string UserSchema { get; set; }
        public UserInfo UserInfo { get; set; }
        public NotificationContacts Contact { get; set; }
        public long ProjectId { get; set; }
        public int RevisionNo { get; set; }
        public int VersionNo { get; set; }
    }
    public class SaveInboxParams
    {
        public int NotificationId { get; set; }
        public long DocumentId { get; set; }
        public int OrganisationId { get; set; }
        public string ESDALReferenceNo { get; set; }
        public string UserSchema { get; set; }
        public int IcaStatus { get; set; }
        public bool ImminentMovestatus { get; set; }
    }
    public class SaveDistributionStatusParams
    {
        public NotificationContacts NotificationContacts { get; set; }
        public int Status { get; set; }
        public int InboxOnly { get; set; }
        public string EsdalReference { get; set; }
        public long TransmissionId { get; set; }
        public bool IsImminent { get; set; }
        public string Username { get; set; }

    }
    public class GenerateMovementActionParams
    {
        public UserInfo UserInfo { get; set; }
        public string EsdalReference { get; set; }
        public MovementActionIdentifiers MovementActionIdentifier { get; set; }
        public int MovementFlag { get; set; }
        public NotificationContacts NotificationContacts { get; set; }
        public string MovementDesc { get; set; }
        public long projectId { get; set; }
        public int revisionNo { get; set; }
        public int versionNo { get; set; }
        public string UserSchema { get; set; }

    }
    public class GetRetransmitDocumentParams
    {
        public TransmittingDocumentDetails TransmittingDetail { get; set; }
        public RetransmitDetails RetransmitDetails { get; set; }
        public int TransmissionId { get; set; }
        public UserInfo UserInfo { get; set; }
        public string UserSchema { get; set; }

    }
    public class SaveOutboundNotificationParams
    {
        public long NotificationId { get; set; }
        public byte[] CompressData { get; set; }
    }
    }
