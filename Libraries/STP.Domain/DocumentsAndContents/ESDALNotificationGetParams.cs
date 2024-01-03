using AggreedRouteXSD;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.DocumentsAndContents
{
    public class ESDALNotificationGetParams
    {
        public XMLModel XMLModel { get; set; }
        public XMLModel XMLModelPolice { get; set; }
        public string XsltSOAPath { get; set; }
        public string XsltPolicePath { get; set; }
        public string XsltPath { get; set; }
        public OutboundDocuments outboundDocuments { get; set; }
    }
    public class GenerateEmailgetParams
    {
        public string HtmlContent { get; set; }
        public byte[] AttachmentData { get; set; }
    }
    public class RetransmitEmailgetParams
    {
        public byte[] Content { get; set; }
        public byte[] AttachmentData { get; set; }
        public int XmlAttached { get; set; }
        public bool IsImminent { get; set; }
    }
    public class NotifDistibutionParams
    {
        public int NotificationId { get; set; }
        public int ContactId { get; set; }
        public Dictionary<int, int> ICAStatusDictionary { get; set; }
        public string ImminentMovestatus { get; set; }
        public UserInfo SessionInfo { get; set; }
        public List<ContactModel> ContactModel { get; set; }
        public bool IsNen { get; set; }
        public bool IsRenotify { get; set; }
        public long ProjectId { get; set; }
    }
    public class SODistributionParams
    {
        public string EsdalReferenceNum { get; set; }
        public string DistribComments { get; set; }
        public int Versionid { get; set; }
        public Dictionary<int, int> IcaStatusDictionary { get; set; }
        public string EsdalReference { get; set; }
        public HAContact HaContact { get; set; }
        public AgreedRouteStructure AgreedRoute { get; set; }
        public long ProjectStatus { get; set; }
        public int VersionNo { get; set; }
        public MovementPrint Moveprint { get; set; }
        public decimal PreVersionDistr { get; set; }
        public UserInfo SessionInfo { get; set; }
        public long ProjectId { get; set; }
        public int LastRevNo { get; set; }
    }
}
