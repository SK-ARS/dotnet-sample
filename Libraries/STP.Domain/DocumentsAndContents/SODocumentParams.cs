using AggreedRouteXSD;
using NotificationXSD;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.DocumentsAndContents
{
    public class SODocumentParams
    {
        public Enums.SOTemplateType TemplateType { get; set; }
        public string EsdalReferenceNo { get; set; }
        public string OrderNumber { get; set; }
        public UserInfo UserInfo { get; set; }
        public bool GenerateFlag { get; set; }
    }
    public class SOProposalDocumentParams
    {
        public string EsdalReferenceNo { get; set; }
        public int OrganisationId { get; set; }
        public int ContactId { get; set; }
        public string DistributionComments { get; set; }
        public int VersionId { get; set; }
        public Dictionary<int, int> ICAStatusDictionary { get; set; }
        public string Esdalreference { get; set; }
        public HAContact HaContactDetail { get; set; }
        public AgreedRouteStructure Agreedroute { get; set; }
        public string UserSchema { get; set; }
        public int RoutePlanUnits { get; set; }
        public long ProjectStatus { get; set; }
        public int VersionNo { get; set; }
        public MovementPrint Moveprint { get; set; }
        public decimal PreVersionDistr { get; set; }
        public UserInfo SessionInfo { get; set; }
        public int NotificationId { get; set; }
        public int ItemTypeStatus { get; set; }
        public NotificationTypeType NotificationType { get; set; }
        public Enums.PortalType UserType { get; set; }
        public int IsNen { get; set; }
    }
    public class NotificationICAstatusParans
    {
        public string XmlaffectedStructures { get; set; }
    }
    public class SODistributeDocumentParams
    {
        public XMLModel XmlModel { get; set; }
        public XMLModel ModelStillAfftdSOA { get; set; }
        public XMLModel ModelNoLongAfftdSOA { get; set; }
        public XMLModel ModelSOA { get; set; }
        public XMLModel ModelPolice { get; set; }
        public string XsltPath { get; set; }
        public string DocFileName { get; set; }
        public int DocType { get; set; }
        public long ProjectStatus { get; set; }
        public OutboundDocuments OutboundDocuments { get; set; }
        public List<ContactModel> ContactList { get; set; }
    }
    public class SOProposalXsltPath
    {
        public string XSLTPath { get; set; }
    }

    public class HtmlDocumentParams
    {
        public string InputHtmlString { get; set; }
    }
}