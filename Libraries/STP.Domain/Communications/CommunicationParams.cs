using STP.Domain.SecurityAndUsers;

namespace STP.Domain.Communications
{
    public class CommunicationParams
    {
        public NotificationContacts ObjectContact { get; set; }
        public UserInfo UserInfo { get; set; }
        public long TransmissionId { get; set; }
        public byte[] Content { get; set; }
        public byte[] Attachment { get; set; }
        public string ESDALReference { get; set; }
        public int XMLAttach { get; set; }
        public bool IsImminent { get; set; }
        public string DocumentTypeName { get; set; }
        public string ReplyToEmail { get; set; }
        public string UserEmail { get; set; }
        public string Subject { get; set; }
        public int DocumentType { get; set; }
    }
}