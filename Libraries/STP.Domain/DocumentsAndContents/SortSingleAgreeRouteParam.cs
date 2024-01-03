using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.DocumentsAndContents
{
    public class SortSingleAgreeRouteParam
    {

        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/agreedroute")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/agreedroute", IsNullable = false)]
        public partial class AgreedRoute
        {
            private string distributionCommentsField;

            private ESDALReferenceNumber eSDALReferenceNumberField;

            private byte jobFileReferenceField;

            private System.DateTime sentDateTimeField;

            private HAContact hAContactField;

            private RecipientsContact[] recipientsField;

            private HaulierDetails haulierDetailsField;

            private string hauliersReferenceField;

            private JourneyFromToSummary journeyFromToSummaryField;

            private JourneyFromTo journeyFromToField;

            private JourneyTiming journeyTimingField;

            private object loadSummaryField;

            private LoadDetails loadDetailsField;

            private RouteParts routePartsField;

            private PredefinedCautions predefinedCautionsField;

            private uint projectIDField;

            private ushort versionIDField;

            private uint versionStatusField;

            private AgreedRouteOrderSummary orderSummaryField;

            private string statusField;

            private object notesForHaulierField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
            public string DistributionComments
            {
                get
                {
                    return this.distributionCommentsField;
                }
                set
                {
                    this.distributionCommentsField = value;
                }

            }
            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
            public ESDALReferenceNumber ESDALReferenceNumber
            {
                get
                {
                    return this.eSDALReferenceNumberField;
                }
                set
                {
                    this.eSDALReferenceNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
            public byte JobFileReference
            {
                get
                {
                    return this.jobFileReferenceField;
                }
                set
                {
                    this.jobFileReferenceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
            public System.DateTime SentDateTime
            {
                get
                {
                    return this.sentDateTimeField;
                }
                set
                {
                    this.sentDateTimeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
            public HAContact HAContact
            {
                get
                {
                    return this.hAContactField;
                }
                set
                {
                    this.hAContactField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
            [System.Xml.Serialization.XmlArrayItemAttribute("Contact", IsNullable = false)]
            public RecipientsContact[] Recipients
            {
                get
                {
                    return this.recipientsField;
                }
                set
                {
                    this.recipientsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
            public HaulierDetails HaulierDetails
            {
                get
                {
                    return this.haulierDetailsField;
                }
                set
                {
                    this.haulierDetailsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
            public string HauliersReference
            {
                get
                {
                    return this.hauliersReferenceField;
                }
                set
                {
                    this.hauliersReferenceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
            public JourneyFromToSummary JourneyFromToSummary
            {
                get
                {
                    return this.journeyFromToSummaryField;
                }
                set
                {
                    this.journeyFromToSummaryField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
            public JourneyFromTo JourneyFromTo
            {
                get
                {
                    return this.journeyFromToField;
                }
                set
                {
                    this.journeyFromToField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
            public JourneyTiming JourneyTiming
            {
                get
                {
                    return this.journeyTimingField;
                }
                set
                {
                    this.journeyTimingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
            public object LoadSummary
            {
                get
                {
                    return this.loadSummaryField;
                }
                set
                {
                    this.loadSummaryField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
            public LoadDetails LoadDetails
            {
                get
                {
                    return this.loadDetailsField;
                }
                set
                {
                    this.loadDetailsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
            public RouteParts RouteParts
            {
                get
                {
                    return this.routePartsField;
                }
                set
                {
                    this.routePartsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
            public PredefinedCautions PredefinedCautions
            {
                get
                {
                    return this.predefinedCautionsField;
                }
                set
                {
                    this.predefinedCautionsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
            public uint ProjectID
            {
                get
                {
                    return this.projectIDField;
                }
                set
                {
                    this.projectIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
            public ushort VersionID
            {
                get
                {
                    return this.versionIDField;
                }
                set
                {
                    this.versionIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
            public uint VersionStatus
            {
                get
                {
                    return this.versionStatusField;
                }
                set
                {
                    this.versionStatusField = value;
                }
            }

            /// <remarks/>
            public AgreedRouteOrderSummary OrderSummary
            {
                get
                {
                    return this.orderSummaryField;
                }
                set
                {
                    this.orderSummaryField = value;
                }
            }

            /// <remarks/>
            public string Status
            {
                get
                {
                    return this.statusField;
                }
                set
                {
                    this.statusField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
            public object NotesForHaulier
            {
                get
                {
                    return this.notesForHaulierField;
                }
                set
                {
                    this.notesForHaulierField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
        public partial class ESDALReferenceNumber
        {

            private string mnemonicField;

            private ushort movementProjectNumberField;

            private byte movementVersionField;

            /// <remarks/>
            public string Mnemonic
            {
                get
                {
                    return this.mnemonicField;
                }
                set
                {
                    this.mnemonicField = value;
                }
            }

            /// <remarks/>
            public ushort MovementProjectNumber
            {
                get
                {
                    return this.movementProjectNumberField;
                }
                set
                {
                    this.movementProjectNumberField = value;
                }
            }

            /// <remarks/>
            public byte MovementVersion
            {
                get
                {
                    return this.movementVersionField;
                }
                set
                {
                    this.movementVersionField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
        public partial class OrderSummary
        {

            private OrderSummaryCurrentOrder currentOrderField;

            /// <remarks/>
            public OrderSummaryCurrentOrder CurrentOrder
            {
                get
                {
                    return this.currentOrderField;
                }
                set
                {
                    this.currentOrderField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class OrderSummaryCurrentOrder
        {

            private string orderNumberField;

            private System.DateTime signedOnField;

            private System.DateTime expiresOnField;

            private string signedByField;

            /// <remarks/>
            public string OrderNumber
            {
                get
                {
                    return this.orderNumberField;
                }
                set
                {
                    this.orderNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public System.DateTime SignedOn
            {
                get
                {
                    return this.signedOnField;
                }
                set
                {
                    this.signedOnField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public System.DateTime ExpiresOn
            {
                get
                {
                    return this.expiresOnField;
                }
                set
                {
                    this.expiresOnField = value;
                }
            }

            /// <remarks/>
            public string SignedBy
            {
                get
                {
                    return this.signedByField;
                }
                set
                {
                    this.signedByField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
        public partial class HAContact
        {

            private string contactField;

            private HAContactAddress addressField;

            private string telephoneNumberField;

            private object faxNumberField;

            private string emailAddressField;

            /// <remarks/>
            public string Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }

            /// <remarks/>
            public HAContactAddress Address
            {
                get
                {
                    return this.addressField;
                }
                set
                {
                    this.addressField = value;
                }
            }

            /// <remarks/>
            public string TelephoneNumber
            {
                get
                {
                    return this.telephoneNumberField;
                }
                set
                {
                    this.telephoneNumberField = value;
                }
            }

            /// <remarks/>
            public object FaxNumber
            {
                get
                {
                    return this.faxNumberField;
                }
                set
                {
                    this.faxNumberField = value;
                }
            }

            /// <remarks/>
            public string EmailAddress
            {
                get
                {
                    return this.emailAddressField;
                }
                set
                {
                    this.emailAddressField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class HAContactAddress
        {

            private string[] lineField;

            private string postCodeField;

            private string countryField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Line", Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
            public string[] Line
            {
                get
                {
                    return this.lineField;
                }
                set
                {
                    this.lineField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
            public string PostCode
            {
                get
                {
                    return this.postCodeField;
                }
                set
                {
                    this.postCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
            public string Country
            {
                get
                {
                    return this.countryField;
                }
                set
                {
                    this.countryField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RecipientsContact
        {

            private string contactNameField;

            private string organisationNameField;

            private object faxField;

            private object emailField;

            private RecipientsContactOnbehalfOf onbehalfOfField;

            private ushort contactIdField;

            private ushort organisationIdField;

            private string reasonField;

            private bool isRecipientField;

            private bool isPoliceField;

            private bool isHaulierField;

            private bool isRetainedNotificationOnlyField;

            /// <remarks/>
            public string ContactName
            {
                get
                {
                    return this.contactNameField;
                }
                set
                {
                    this.contactNameField = value;
                }
            }

            /// <remarks/>
            public string OrganisationName
            {
                get
                {
                    return this.organisationNameField;
                }
                set
                {
                    this.organisationNameField = value;
                }
            }

            /// <remarks/>
            public object Fax
            {
                get
                {
                    return this.faxField;
                }
                set
                {
                    this.faxField = value;
                }
            }

            /// <remarks/>
            public object Email
            {
                get
                {
                    return this.emailField;
                }
                set
                {
                    this.emailField = value;
                }
            }

            /// <remarks/>
            public RecipientsContactOnbehalfOf OnbehalfOf
            {
                get
                {
                    return this.onbehalfOfField;
                }
                set
                {
                    this.onbehalfOfField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort ContactId
            {
                get
                {
                    return this.contactIdField;
                }
                set
                {
                    this.contactIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort OrganisationId
            {
                get
                {
                    return this.organisationIdField;
                }
                set
                {
                    this.organisationIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Reason
            {
                get
                {
                    return this.reasonField;
                }
                set
                {
                    this.reasonField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsRecipient
            {
                get
                {
                    return this.isRecipientField;
                }
                set
                {
                    this.isRecipientField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsPolice
            {
                get
                {
                    return this.isPoliceField;
                }
                set
                {
                    this.isPoliceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsHaulier
            {
                get
                {
                    return this.isHaulierField;
                }
                set
                {
                    this.isHaulierField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsRetainedNotificationOnly
            {
                get
                {
                    return this.isRetainedNotificationOnlyField;
                }
                set
                {
                    this.isRetainedNotificationOnlyField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RecipientsContactOnbehalfOf
        {

            private string delegatorsOrganisationNameField;

            private ushort delegatorsOrganisationIdField;

            private ushort delegatorsContactIdField;

            private ushort delegationIdField;

            /// <remarks/>
            public string DelegatorsOrganisationName
            {
                get
                {
                    return this.delegatorsOrganisationNameField;
                }
                set
                {
                    this.delegatorsOrganisationNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort DelegatorsOrganisationId
            {
                get
                {
                    return this.delegatorsOrganisationIdField;
                }
                set
                {
                    this.delegatorsOrganisationIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort DelegatorsContactId
            {
                get
                {
                    return this.delegatorsContactIdField;
                }
                set
                {
                    this.delegatorsContactIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort DelegationId
            {
                get
                {
                    return this.delegationIdField;
                }
                set
                {
                    this.delegationIdField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
        public partial class HaulierDetails
        {

            private string haulierContactField;

            private string haulierNameField;

            private HaulierDetailsHaulierAddress haulierAddressField;

            private string telephoneNumberField;

            private object faxNumberField;

            private string emailAddressField;

            private ushort organisationIdField;

            /// <remarks/>
            public string HaulierContact
            {
                get
                {
                    return this.haulierContactField;
                }
                set
                {
                    this.haulierContactField = value;
                }
            }

            /// <remarks/>
            public string HaulierName
            {
                get
                {
                    return this.haulierNameField;
                }
                set
                {
                    this.haulierNameField = value;
                }
            }

            /// <remarks/>
            public HaulierDetailsHaulierAddress HaulierAddress
            {
                get
                {
                    return this.haulierAddressField;
                }
                set
                {
                    this.haulierAddressField = value;
                }
            }

            /// <remarks/>
            public string TelephoneNumber
            {
                get
                {
                    return this.telephoneNumberField;
                }
                set
                {
                    this.telephoneNumberField = value;
                }
            }

            /// <remarks/>
            public object FaxNumber
            {
                get
                {
                    return this.faxNumberField;
                }
                set
                {
                    this.faxNumberField = value;
                }
            }

            /// <remarks/>
            public string EmailAddress
            {
                get
                {
                    return this.emailAddressField;
                }
                set
                {
                    this.emailAddressField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort OrganisationId
            {
                get
                {
                    return this.organisationIdField;
                }
                set
                {
                    this.organisationIdField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class HaulierDetailsHaulierAddress
        {

            private string[] lineField;

            private string postCodeField;

            private string licenceField;

            private string countryField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Line", Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
            public string[] Line
            {
                get
                {
                    return this.lineField;
                }
                set
                {
                    this.lineField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
            public string PostCode
            {
                get
                {
                    return this.postCodeField;
                }
                set
                {
                    this.postCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
            public string Licence
            {
                get
                {
                    return this.licenceField;
                }
                set
                {
                    this.licenceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
            public string Country
            {
                get
                {
                    return this.countryField;
                }
                set
                {
                    this.countryField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
        public partial class JourneyFromToSummary
        {

            private string fromField;

            private string toField;

            /// <remarks/>
            public string From
            {
                get
                {
                    return this.fromField;
                }
                set
                {
                    this.fromField = value;
                }
            }

            /// <remarks/>
            public string To
            {
                get
                {
                    return this.toField;
                }
                set
                {
                    this.toField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
        public partial class JourneyFromTo
        {

            private string fromField;

            private string toField;

            /// <remarks/>
            public string From
            {
                get
                {
                    return this.fromField;
                }
                set
                {
                    this.fromField = value;
                }
            }

            /// <remarks/>
            public string To
            {
                get
                {
                    return this.toField;
                }
                set
                {
                    this.toField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
        public partial class JourneyTiming
        {

            private System.DateTime firstMoveDateField;

            private System.DateTime lastMoveDateField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public System.DateTime FirstMoveDate
            {
                get
                {
                    return this.firstMoveDateField;
                }
                set
                {
                    this.firstMoveDateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public System.DateTime LastMoveDate
            {
                get
                {
                    return this.lastMoveDateField;
                }
                set
                {
                    this.lastMoveDateField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
        public partial class LoadDetails
        {

            private byte descriptionField;

            private byte totalMovesField;

            private byte maxPiecesPerMoveField;

            /// <remarks/>
            public byte Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            public byte TotalMoves
            {
                get
                {
                    return this.totalMovesField;
                }
                set
                {
                    this.totalMovesField = value;
                }
            }

            /// <remarks/>
            public byte MaxPiecesPerMove
            {
                get
                {
                    return this.maxPiecesPerMoveField;
                }
                set
                {
                    this.maxPiecesPerMoveField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
        public partial class RouteParts
        {

            private RoutePartsRoutePartListPosition routePartListPositionField;

            /// <remarks/>
            public RoutePartsRoutePartListPosition RoutePartListPosition
            {
                get
                {
                    return this.routePartListPositionField;
                }
                set
                {
                    this.routePartListPositionField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPosition
        {

            private RoutePartsRoutePartListPositionRoutePart routePartField;

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePart RoutePart
            {
                get
                {
                    return this.routePartField;
                }
                set
                {
                    this.routePartField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPositionRoutePart
        {

            private string legNumberField;

            private string nameField;

            private RoutePartsRoutePartListPositionRoutePartRoadPart roadPartField;

            private string modeField;

            private string idField;

            /// <remarks/>
            public string LegNumber
            {
                get
                {
                    return this.legNumberField;
                }
                set
                {
                    this.legNumberField = value;
                }
            }

            /// <remarks/>
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePartRoadPart RoadPart
            {
                get
                {
                    return this.roadPartField;
                }
                set
                {
                    this.roadPartField = value;
                }
            }

            /// <remarks/>
            public string Mode
            {
                get
                {
                    return this.modeField;
                }
                set
                {
                    this.modeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPart
        {

            private RoutePartsRoutePartListPositionRoutePartRoadPartStartPointListPosition startPointListPositionField;

            private RoutePartsRoutePartListPositionRoutePartRoadPartEndPointListPosition endPointListPositionField;

            private RoutePartsRoutePartListPositionRoutePartRoadPartDistance distanceField;

            private RoutePartsRoutePartListPositionRoutePartRoadPartVehicles vehiclesField;

            private RoutePartsRoutePartListPositionRoutePartRoadPartRoads roadsField;

            private RoutePartsRoutePartListPositionRoutePartRoadPartStructures structuresField;

            private DrivingInstructions drivingInstructionsField;

            private object routeDescriptionField;

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePartRoadPartStartPointListPosition StartPointListPosition
            {
                get
                {
                    return this.startPointListPositionField;
                }
                set
                {
                    this.startPointListPositionField = value;
                }
            }

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePartRoadPartEndPointListPosition EndPointListPosition
            {
                get
                {
                    return this.endPointListPositionField;
                }
                set
                {
                    this.endPointListPositionField = value;
                }
            }

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePartRoadPartDistance Distance
            {
                get
                {
                    return this.distanceField;
                }
                set
                {
                    this.distanceField = value;
                }
            }

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePartRoadPartVehicles Vehicles
            {
                get
                {
                    return this.vehiclesField;
                }
                set
                {
                    this.vehiclesField = value;
                }
            }

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePartRoadPartRoads Roads
            {
                get
                {
                    return this.roadsField;
                }
                set
                {
                    this.roadsField = value;
                }
            }

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePartRoadPartStructures Structures
            {
                get
                {
                    return this.structuresField;
                }
                set
                {
                    this.structuresField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
            public DrivingInstructions DrivingInstructions
            {
                get
                {
                    return this.drivingInstructionsField;
                }
                set
                {
                    this.drivingInstructionsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "ns2")]
            public object RouteDescription
            {
                get
                {
                    return this.routeDescriptionField;
                }
                set
                {
                    this.routeDescriptionField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartStartPointListPosition
        {

            private RoutePartsRoutePartListPositionRoutePartRoadPartStartPointListPositionStartPoint startPointField;

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePartRoadPartStartPointListPositionStartPoint StartPoint
            {
                get
                {
                    return this.startPointField;
                }
                set
                {
                    this.startPointField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartStartPointListPositionStartPoint
        {

            private string descriptionField;

            private string gridRefField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
            public string GridRef
            {
                get
                {
                    return this.gridRefField;
                }
                set
                {
                    this.gridRefField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartEndPointListPosition
        {

            private RoutePartsRoutePartListPositionRoutePartRoadPartEndPointListPositionEndPoint endPointField;

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePartRoadPartEndPointListPositionEndPoint EndPoint
            {
                get
                {
                    return this.endPointField;
                }
                set
                {
                    this.endPointField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartEndPointListPositionEndPoint
        {

            private string descriptionField;

            private string gridRefField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
            public string GridRef
            {
                get
                {
                    return this.gridRefField;
                }
                set
                {
                    this.gridRefField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartDistance
        {

            private RoutePartsRoutePartListPositionRoutePartRoadPartDistanceMetric metricField;

            private RoutePartsRoutePartListPositionRoutePartRoadPartDistanceImperial imperialField;

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePartRoadPartDistanceMetric Metric
            {
                get
                {
                    return this.metricField;
                }
                set
                {
                    this.metricField = value;
                }
            }

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePartRoadPartDistanceImperial Imperial
            {
                get
                {
                    return this.imperialField;
                }
                set
                {
                    this.imperialField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartDistanceMetric
        {

            private uint distanceField;

            /// <remarks/>
            public uint Distance
            {
                get
                {
                    return this.distanceField;
                }
                set
                {
                    this.distanceField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartDistanceImperial
        {

            private uint distanceField;

            /// <remarks/>
            public uint Distance
            {
                get
                {
                    return this.distanceField;
                }
                set
                {
                    this.distanceField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartVehicles
        {

            private ConfigurationSummaryListPosition[] configurationSummaryListPositionField;

            private OverallLengthListPosition[] overallLengthListPositionField;

            private RigidLengthListPosition[] rigidLengthListPositionField;

            private RearOverhangListPosition[] rearOverhangListPositionField;

            private GroundClearancegListPosition[] groundClearancegListPositionField;

            private ReducedGroundClearanceListPosition reducedGroundClearanceListPositionField;

            private LeftOverhangListPosition leftOverhangListPositionField;

            private RightOverhangListPosition rightOverhangListPositionField;

            private FrontOverhangListPosition[] frontOverhangListPositionField;

            private OverallWidthListPosition[] overallWidthListPositionField;

            private OverallHeightListPosition[] overallHeightListPositionField;

            private GrossWeightListPosition[] grossWeightListPositionField;

            private MaxAxleWeightListPosition[] maxAxleWeightListPositionField;

            private VehicleSummaryListPosition[] vehicleSummaryListPositionField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ConfigurationSummaryListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
            public ConfigurationSummaryListPosition[] ConfigurationSummaryListPosition
            {
                get
                {
                    return this.configurationSummaryListPositionField;
                }
                set
                {
                    this.configurationSummaryListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("OverallLengthListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
            public OverallLengthListPosition[] OverallLengthListPosition
            {
                get
                {
                    return this.overallLengthListPositionField;
                }
                set
                {
                    this.overallLengthListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("RigidLengthListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
            public RigidLengthListPosition[] RigidLengthListPosition
            {
                get
                {
                    return this.rigidLengthListPositionField;
                }
                set
                {
                    this.rigidLengthListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("RearOverhangListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
            public RearOverhangListPosition[] RearOverhangListPosition
            {
                get
                {
                    return this.rearOverhangListPositionField;
                }
                set
                {
                    this.rearOverhangListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("GroundClearancegListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
            public GroundClearancegListPosition[] GroundClearancegListPosition
            {
                get
                {
                    return this.groundClearancegListPositionField;
                }
                set
                {
                    this.groundClearancegListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
            public ReducedGroundClearanceListPosition ReducedGroundClearanceListPosition
            {
                get
                {
                    return this.reducedGroundClearanceListPositionField;
                }
                set
                {
                    this.reducedGroundClearanceListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
            public LeftOverhangListPosition LeftOverhangListPosition
            {
                get
                {
                    return this.leftOverhangListPositionField;
                }
                set
                {
                    this.leftOverhangListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
            public RightOverhangListPosition RightOverhangListPosition
            {
                get
                {
                    return this.rightOverhangListPositionField;
                }
                set
                {
                    this.rightOverhangListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("FrontOverhangListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
            public FrontOverhangListPosition[] FrontOverhangListPosition
            {
                get
                {
                    return this.frontOverhangListPositionField;
                }
                set
                {
                    this.frontOverhangListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("OverallWidthListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
            public OverallWidthListPosition[] OverallWidthListPosition
            {
                get
                {
                    return this.overallWidthListPositionField;
                }
                set
                {
                    this.overallWidthListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("OverallHeightListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
            public OverallHeightListPosition[] OverallHeightListPosition
            {
                get
                {
                    return this.overallHeightListPositionField;
                }
                set
                {
                    this.overallHeightListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("GrossWeightListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
            public GrossWeightListPosition[] GrossWeightListPosition
            {
                get
                {
                    return this.grossWeightListPositionField;
                }
                set
                {
                    this.grossWeightListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("MaxAxleWeightListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
            public MaxAxleWeightListPosition[] MaxAxleWeightListPosition
            {
                get
                {
                    return this.maxAxleWeightListPositionField;
                }
                set
                {
                    this.maxAxleWeightListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("VehicleSummaryListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
            public VehicleSummaryListPosition[] VehicleSummaryListPosition
            {
                get
                {
                    return this.vehicleSummaryListPositionField;
                }
                set
                {
                    this.vehicleSummaryListPositionField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle", IsNullable = false)]
        public partial class ConfigurationSummaryListPosition
        {

            private string configurationSummaryField;

            /// <remarks/>
            public string ConfigurationSummary
            {
                get
                {
                    return this.configurationSummaryField;
                }
                set
                {
                    this.configurationSummaryField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle", IsNullable = false)]
        public partial class OverallLengthListPosition
        {

            private OverallLengthListPositionOverallLength overallLengthField;

            /// <remarks/>
            public OverallLengthListPositionOverallLength OverallLength
            {
                get
                {
                    return this.overallLengthField;
                }
                set
                {
                    this.overallLengthField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class OverallLengthListPositionOverallLength
        {

            private decimal includingProjectionsField;

            private decimal excludingProjectionsField;

            /// <remarks/>
            public decimal IncludingProjections
            {
                get
                {
                    return this.includingProjectionsField;
                }
                set
                {
                    this.includingProjectionsField = value;
                }
            }

            /// <remarks/>
            public decimal ExcludingProjections
            {
                get
                {
                    return this.excludingProjectionsField;
                }
                set
                {
                    this.excludingProjectionsField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle", IsNullable = false)]
        public partial class RigidLengthListPosition
        {

            private decimal rigidLengthField;

            /// <remarks/>
            public decimal RigidLength
            {
                get
                {
                    return this.rigidLengthField;
                }
                set
                {
                    this.rigidLengthField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle", IsNullable = false)]
        public partial class RearOverhangListPosition
        {

            private byte rearOverhangField;

            /// <remarks/>
            public byte RearOverhang
            {
                get
                {
                    return this.rearOverhangField;
                }
                set
                {
                    this.rearOverhangField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle", IsNullable = false)]
        public partial class GroundClearancegListPosition
        {

            private decimal groundClearanceField;

            /// <remarks/>
            public decimal GroundClearance
            {
                get
                {
                    return this.groundClearanceField;
                }
                set
                {
                    this.groundClearanceField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle", IsNullable = false)]
        public partial class ReducedGroundClearanceListPosition
        {

            private decimal reducedGroundClearanceField;

            /// <remarks/>
            public decimal ReducedGroundClearance
            {
                get
                {
                    return this.reducedGroundClearanceField;
                }
                set
                {
                    this.reducedGroundClearanceField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle", IsNullable = false)]
        public partial class LeftOverhangListPosition
        {

            private byte leftOverhangField;

            /// <remarks/>
            public byte LeftOverhang
            {
                get
                {
                    return this.leftOverhangField;
                }
                set
                {
                    this.leftOverhangField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle", IsNullable = false)]
        public partial class RightOverhangListPosition
        {

            private byte rightOverhangField;

            /// <remarks/>
            public byte RightOverhang
            {
                get
                {
                    return this.rightOverhangField;
                }
                set
                {
                    this.rightOverhangField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle", IsNullable = false)]
        public partial class FrontOverhangListPosition
        {

            private byte frontOverhangField;

            private byte frontOverhangFieldField;

            /// <remarks/>
            public byte FrontOverhang
            {
                get
                {
                    return this.frontOverhangField;
                }
                set
                {
                    this.frontOverhangField = value;
                }
            }

            /// <remarks/>
            public byte FrontOverhangField
            {
                get
                {
                    return this.frontOverhangFieldField;
                }
                set
                {
                    this.frontOverhangFieldField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle", IsNullable = false)]
        public partial class OverallWidthListPosition
        {

            private decimal overallWidthField;

            /// <remarks/>
            public decimal OverallWidth
            {
                get
                {
                    return this.overallWidthField;
                }
                set
                {
                    this.overallWidthField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle", IsNullable = false)]
        public partial class OverallHeightListPosition
        {

            private OverallHeightListPositionOverallHeight overallHeightField;

            /// <remarks/>
            public OverallHeightListPositionOverallHeight OverallHeight
            {
                get
                {
                    return this.overallHeightField;
                }
                set
                {
                    this.overallHeightField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class OverallHeightListPositionOverallHeight
        {

            private decimal maxHeightField;

            private decimal reducibleHeightField;

            /// <remarks/>
            public decimal MaxHeight
            {
                get
                {
                    return this.maxHeightField;
                }
                set
                {
                    this.maxHeightField = value;
                }
            }

            /// <remarks/>
            public decimal ReducibleHeight
            {
                get
                {
                    return this.reducibleHeightField;
                }
                set
                {
                    this.reducibleHeightField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle", IsNullable = false)]
        public partial class GrossWeightListPosition
        {

            private GrossWeightListPositionGrossWeight grossWeightField;

            /// <remarks/>
            public GrossWeightListPositionGrossWeight GrossWeight
            {
                get
                {
                    return this.grossWeightField;
                }
                set
                {
                    this.grossWeightField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class GrossWeightListPositionGrossWeight
        {

            private uint weightField;

            /// <remarks/>
            public uint Weight
            {
                get
                {
                    return this.weightField;
                }
                set
                {
                    this.weightField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle", IsNullable = false)]
        public partial class MaxAxleWeightListPosition
        {

            private MaxAxleWeightListPositionMaxAxleWeight maxAxleWeightField;

            /// <remarks/>
            public MaxAxleWeightListPositionMaxAxleWeight MaxAxleWeight
            {
                get
                {
                    return this.maxAxleWeightField;
                }
                set
                {
                    this.maxAxleWeightField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class MaxAxleWeightListPositionMaxAxleWeight
        {

            private ushort weightField;

            /// <remarks/>
            public ushort Weight
            {
                get
                {
                    return this.weightField;
                }
                set
                {
                    this.weightField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle", IsNullable = false)]
        public partial class VehicleSummaryListPosition
        {

            private VehicleSummaryListPositionVehicleSummary vehicleSummaryField;

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummary VehicleSummary
            {
                get
                {
                    return this.vehicleSummaryField;
                }
                set
                {
                    this.vehicleSummaryField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummary
        {

            private string weightConformanceField;

            private VehicleSummaryListPositionVehicleSummaryConfiguration configurationField;

            private string configurationTypeField;

            /// <remarks/>
            public string WeightConformance
            {
                get
                {
                    return this.weightConformanceField;
                }
                set
                {
                    this.weightConformanceField = value;
                }
            }

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfiguration Configuration
            {
                get
                {
                    return this.configurationField;
                }
                set
                {
                    this.configurationField = value;
                }
            }

            /// <remarks/>
            public string ConfigurationType
            {
                get
                {
                    return this.configurationTypeField;
                }
                set
                {
                    this.configurationTypeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfiguration
        {

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPosition[] nonSemiVehicleField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("ComponentListPosition", IsNullable = false)]
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPosition[] NonSemiVehicle
            {
                get
                {
                    return this.nonSemiVehicleField;
                }
                set
                {
                    this.nonSemiVehicleField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPosition
        {

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponent componentField;

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponent Component
            {
                get
                {
                    return this.componentField;
                }
                set
                {
                    this.componentField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponent
        {

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearing loadBearingField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractor drawbarTractorField;

            private string componentTypeField;

            private string componentSubTypeField;

            private byte longitudeField;

            private bool longitudeFieldSpecified;

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearing LoadBearing
            {
                get
                {
                    return this.loadBearingField;
                }
                set
                {
                    this.loadBearingField = value;
                }
            }

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractor DrawbarTractor
            {
                get
                {
                    return this.drawbarTractorField;
                }
                set
                {
                    this.drawbarTractorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ComponentType
            {
                get
                {
                    return this.componentTypeField;
                }
                set
                {
                    this.componentTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ComponentSubType
            {
                get
                {
                    return this.componentSubTypeField;
                }
                set
                {
                    this.componentSubTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Longitude
            {
                get
                {
                    return this.longitudeField;
                }
                set
                {
                    this.longitudeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool LongitudeSpecified
            {
                get
                {
                    return this.longitudeFieldSpecified;
                }
                set
                {
                    this.longitudeFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearing
        {

            private string summaryField;

            private uint weightField;

            private bool isSteerableAtRearField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingMaxAxleWeight maxAxleWeightField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfiguration axleConfigurationField;

            private decimal rigidLengthField;

            private decimal widthField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingHeight heightField;

            private decimal wheelbaseField;

            private decimal leftOverhangField;

            private decimal rightOverhangField;

            private byte frontOverhangField;

            private byte rearOverhangField;

            private decimal groundClearanceField;

            private decimal reducedGroundClearanceField;

            private byte outsideTrackField;

            private byte axleSpacingToFollowingField;

            /// <remarks/>
            public string Summary
            {
                get
                {
                    return this.summaryField;
                }
                set
                {
                    this.summaryField = value;
                }
            }

            /// <remarks/>
            public uint Weight
            {
                get
                {
                    return this.weightField;
                }
                set
                {
                    this.weightField = value;
                }
            }

            /// <remarks/>
            public bool IsSteerableAtRear
            {
                get
                {
                    return this.isSteerableAtRearField;
                }
                set
                {
                    this.isSteerableAtRearField = value;
                }
            }

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingMaxAxleWeight MaxAxleWeight
            {
                get
                {
                    return this.maxAxleWeightField;
                }
                set
                {
                    this.maxAxleWeightField = value;
                }
            }

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfiguration AxleConfiguration
            {
                get
                {
                    return this.axleConfigurationField;
                }
                set
                {
                    this.axleConfigurationField = value;
                }
            }

            /// <remarks/>
            public decimal RigidLength
            {
                get
                {
                    return this.rigidLengthField;
                }
                set
                {
                    this.rigidLengthField = value;
                }
            }

            /// <remarks/>
            public decimal Width
            {
                get
                {
                    return this.widthField;
                }
                set
                {
                    this.widthField = value;
                }
            }

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingHeight Height
            {
                get
                {
                    return this.heightField;
                }
                set
                {
                    this.heightField = value;
                }
            }

            /// <remarks/>
            public decimal Wheelbase
            {
                get
                {
                    return this.wheelbaseField;
                }
                set
                {
                    this.wheelbaseField = value;
                }
            }

            /// <remarks/>
            public decimal LeftOverhang
            {
                get
                {
                    return this.leftOverhangField;
                }
                set
                {
                    this.leftOverhangField = value;
                }
            }

            /// <remarks/>
            public decimal RightOverhang
            {
                get
                {
                    return this.rightOverhangField;
                }
                set
                {
                    this.rightOverhangField = value;
                }
            }

            /// <remarks/>
            public byte FrontOverhang
            {
                get
                {
                    return this.frontOverhangField;
                }
                set
                {
                    this.frontOverhangField = value;
                }
            }

            /// <remarks/>
            public byte RearOverhang
            {
                get
                {
                    return this.rearOverhangField;
                }
                set
                {
                    this.rearOverhangField = value;
                }
            }

            /// <remarks/>
            public decimal GroundClearance
            {
                get
                {
                    return this.groundClearanceField;
                }
                set
                {
                    this.groundClearanceField = value;
                }
            }

            /// <remarks/>
            public decimal ReducedGroundClearance
            {
                get
                {
                    return this.reducedGroundClearanceField;
                }
                set
                {
                    this.reducedGroundClearanceField = value;
                }
            }

            /// <remarks/>
            public byte OutsideTrack
            {
                get
                {
                    return this.outsideTrackField;
                }
                set
                {
                    this.outsideTrackField = value;
                }
            }

            /// <remarks/>
            public byte AxleSpacingToFollowing
            {
                get
                {
                    return this.axleSpacingToFollowingField;
                }
                set
                {
                    this.axleSpacingToFollowingField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingMaxAxleWeight
        {

            private ushort weightField;

            /// <remarks/>
            public ushort Weight
            {
                get
                {
                    return this.weightField;
                }
                set
                {
                    this.weightField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfiguration
        {

            private byte numberOfAxlesField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationAxleWeightListPosition axleWeightListPositionField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationWheelsPerAxleListPosition wheelsPerAxleListPositionField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationAxleSpacingListPosition axleSpacingListPositionField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationTyreSizeListPosition tyreSizeListPositionField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationWheelSpacingListPosition wheelSpacingListPositionField;

            /// <remarks/>
            public byte NumberOfAxles
            {
                get
                {
                    return this.numberOfAxlesField;
                }
                set
                {
                    this.numberOfAxlesField = value;
                }
            }

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationAxleWeightListPosition AxleWeightListPosition
            {
                get
                {
                    return this.axleWeightListPositionField;
                }
                set
                {
                    this.axleWeightListPositionField = value;
                }
            }

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationWheelsPerAxleListPosition WheelsPerAxleListPosition
            {
                get
                {
                    return this.wheelsPerAxleListPositionField;
                }
                set
                {
                    this.wheelsPerAxleListPositionField = value;
                }
            }

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationAxleSpacingListPosition AxleSpacingListPosition
            {
                get
                {
                    return this.axleSpacingListPositionField;
                }
                set
                {
                    this.axleSpacingListPositionField = value;
                }
            }

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationTyreSizeListPosition TyreSizeListPosition
            {
                get
                {
                    return this.tyreSizeListPositionField;
                }
                set
                {
                    this.tyreSizeListPositionField = value;
                }
            }

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationWheelSpacingListPosition WheelSpacingListPosition
            {
                get
                {
                    return this.wheelSpacingListPositionField;
                }
                set
                {
                    this.wheelSpacingListPositionField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationAxleWeightListPosition
        {

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationAxleWeightListPositionAxleWeight axleWeightField;

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationAxleWeightListPositionAxleWeight AxleWeight
            {
                get
                {
                    return this.axleWeightField;
                }
                set
                {
                    this.axleWeightField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationAxleWeightListPositionAxleWeight
        {

            private byte axleCountField;

            private ushort valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte AxleCount
            {
                get
                {
                    return this.axleCountField;
                }
                set
                {
                    this.axleCountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public ushort Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationWheelsPerAxleListPosition
        {

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationWheelsPerAxleListPositionWheelsPerAxle wheelsPerAxleField;

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationWheelsPerAxleListPositionWheelsPerAxle WheelsPerAxle
            {
                get
                {
                    return this.wheelsPerAxleField;
                }
                set
                {
                    this.wheelsPerAxleField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationWheelsPerAxleListPositionWheelsPerAxle
        {

            private byte axleCountField;

            private byte valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte AxleCount
            {
                get
                {
                    return this.axleCountField;
                }
                set
                {
                    this.axleCountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public byte Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationAxleSpacingListPosition
        {

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationAxleSpacingListPositionAxleSpacing axleSpacingField;

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationAxleSpacingListPositionAxleSpacing AxleSpacing
            {
                get
                {
                    return this.axleSpacingField;
                }
                set
                {
                    this.axleSpacingField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationAxleSpacingListPositionAxleSpacing
        {

            private byte axleCountField;

            private decimal valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte AxleCount
            {
                get
                {
                    return this.axleCountField;
                }
                set
                {
                    this.axleCountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public decimal Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationTyreSizeListPosition
        {

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationTyreSizeListPositionTyreSize tyreSizeField;

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationTyreSizeListPositionTyreSize TyreSize
            {
                get
                {
                    return this.tyreSizeField;
                }
                set
                {
                    this.tyreSizeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationTyreSizeListPositionTyreSize
        {

            private byte axleCountField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte AxleCount
            {
                get
                {
                    return this.axleCountField;
                }
                set
                {
                    this.axleCountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationWheelSpacingListPosition
        {

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationWheelSpacingListPositionWheelSpacing wheelSpacingField;

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationWheelSpacingListPositionWheelSpacing WheelSpacing
            {
                get
                {
                    return this.wheelSpacingField;
                }
                set
                {
                    this.wheelSpacingField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingAxleConfigurationWheelSpacingListPositionWheelSpacing
        {

            private byte axleCountField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte AxleCount
            {
                get
                {
                    return this.axleCountField;
                }
                set
                {
                    this.axleCountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentLoadBearingHeight
        {

            private decimal maxHeightField;

            private decimal reducibleHeightField;

            /// <remarks/>
            public decimal MaxHeight
            {
                get
                {
                    return this.maxHeightField;
                }
                set
                {
                    this.maxHeightField = value;
                }
            }

            /// <remarks/>
            public decimal ReducibleHeight
            {
                get
                {
                    return this.reducibleHeightField;
                }
                set
                {
                    this.reducibleHeightField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractor
        {

            private string summaryField;

            private ushort weightField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorMaxAxleWeight maxAxleWeightField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfiguration axleConfigurationField;

            private decimal lengthField;

            private decimal axleSpacingToFollowingField;

            /// <remarks/>
            public string Summary
            {
                get
                {
                    return this.summaryField;
                }
                set
                {
                    this.summaryField = value;
                }
            }

            /// <remarks/>
            public ushort Weight
            {
                get
                {
                    return this.weightField;
                }
                set
                {
                    this.weightField = value;
                }
            }

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorMaxAxleWeight MaxAxleWeight
            {
                get
                {
                    return this.maxAxleWeightField;
                }
                set
                {
                    this.maxAxleWeightField = value;
                }
            }

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfiguration AxleConfiguration
            {
                get
                {
                    return this.axleConfigurationField;
                }
                set
                {
                    this.axleConfigurationField = value;
                }
            }

            /// <remarks/>
            public decimal Length
            {
                get
                {
                    return this.lengthField;
                }
                set
                {
                    this.lengthField = value;
                }
            }

            /// <remarks/>
            public decimal AxleSpacingToFollowing
            {
                get
                {
                    return this.axleSpacingToFollowingField;
                }
                set
                {
                    this.axleSpacingToFollowingField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorMaxAxleWeight
        {

            private ushort weightField;

            /// <remarks/>
            public ushort Weight
            {
                get
                {
                    return this.weightField;
                }
                set
                {
                    this.weightField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfiguration
        {

            private byte numberOfAxlesField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationAxleWeightListPosition[] axleWeightListPositionField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationWheelsPerAxleListPosition[] wheelsPerAxleListPositionField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationAxleSpacingListPosition[] axleSpacingListPositionField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationTyreSizeListPosition[] tyreSizeListPositionField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationWheelSpacingListPosition[] wheelSpacingListPositionField;

            /// <remarks/>
            public byte NumberOfAxles
            {
                get
                {
                    return this.numberOfAxlesField;
                }
                set
                {
                    this.numberOfAxlesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("AxleWeightListPosition")]
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationAxleWeightListPosition[] AxleWeightListPosition
            {
                get
                {
                    return this.axleWeightListPositionField;
                }
                set
                {
                    this.axleWeightListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("WheelsPerAxleListPosition")]
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationWheelsPerAxleListPosition[] WheelsPerAxleListPosition
            {
                get
                {
                    return this.wheelsPerAxleListPositionField;
                }
                set
                {
                    this.wheelsPerAxleListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("AxleSpacingListPosition")]
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationAxleSpacingListPosition[] AxleSpacingListPosition
            {
                get
                {
                    return this.axleSpacingListPositionField;
                }
                set
                {
                    this.axleSpacingListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("TyreSizeListPosition")]
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationTyreSizeListPosition[] TyreSizeListPosition
            {
                get
                {
                    return this.tyreSizeListPositionField;
                }
                set
                {
                    this.tyreSizeListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("WheelSpacingListPosition")]
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationWheelSpacingListPosition[] WheelSpacingListPosition
            {
                get
                {
                    return this.wheelSpacingListPositionField;
                }
                set
                {
                    this.wheelSpacingListPositionField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationAxleWeightListPosition
        {

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationAxleWeightListPositionAxleWeight axleWeightField;

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationAxleWeightListPositionAxleWeight AxleWeight
            {
                get
                {
                    return this.axleWeightField;
                }
                set
                {
                    this.axleWeightField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationAxleWeightListPositionAxleWeight
        {

            private byte axleCountField;

            private ushort valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte AxleCount
            {
                get
                {
                    return this.axleCountField;
                }
                set
                {
                    this.axleCountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public ushort Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationWheelsPerAxleListPosition
        {

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationWheelsPerAxleListPositionWheelsPerAxle wheelsPerAxleField;

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationWheelsPerAxleListPositionWheelsPerAxle WheelsPerAxle
            {
                get
                {
                    return this.wheelsPerAxleField;
                }
                set
                {
                    this.wheelsPerAxleField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationWheelsPerAxleListPositionWheelsPerAxle
        {

            private byte axleCountField;

            private byte valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte AxleCount
            {
                get
                {
                    return this.axleCountField;
                }
                set
                {
                    this.axleCountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public byte Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationAxleSpacingListPosition
        {

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationAxleSpacingListPositionAxleSpacing axleSpacingField;

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationAxleSpacingListPositionAxleSpacing AxleSpacing
            {
                get
                {
                    return this.axleSpacingField;
                }
                set
                {
                    this.axleSpacingField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationAxleSpacingListPositionAxleSpacing
        {

            private byte axleCountField;

            private decimal valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte AxleCount
            {
                get
                {
                    return this.axleCountField;
                }
                set
                {
                    this.axleCountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public decimal Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationTyreSizeListPosition
        {

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationTyreSizeListPositionTyreSize tyreSizeField;

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationTyreSizeListPositionTyreSize TyreSize
            {
                get
                {
                    return this.tyreSizeField;
                }
                set
                {
                    this.tyreSizeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationTyreSizeListPositionTyreSize
        {

            private byte axleCountField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte AxleCount
            {
                get
                {
                    return this.axleCountField;
                }
                set
                {
                    this.axleCountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationWheelSpacingListPosition
        {

            private VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationWheelSpacingListPositionWheelSpacing wheelSpacingField;

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationWheelSpacingListPositionWheelSpacing WheelSpacing
            {
                get
                {
                    return this.wheelSpacingField;
                }
                set
                {
                    this.wheelSpacingField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationComponentListPositionComponentDrawbarTractorAxleConfigurationWheelSpacingListPositionWheelSpacing
        {

            private byte axleCountField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte AxleCount
            {
                get
                {
                    return this.axleCountField;
                }
                set
                {
                    this.axleCountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRoads
        {

            private RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPosition routeSubPartListPositionField;

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPosition RouteSubPartListPosition
            {
                get
                {
                    return this.routeSubPartListPositionField;
                }
                set
                {
                    this.routeSubPartListPositionField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPosition
        {

            private RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPart routeSubPartField;

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPart RouteSubPart
            {
                get
                {
                    return this.routeSubPartField;
                }
                set
                {
                    this.routeSubPartField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPart
        {

            private RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPosition pathListPositionField;

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPosition PathListPosition
            {
                get
                {
                    return this.pathListPositionField;
                }
                set
                {
                    this.pathListPositionField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPosition
        {

            private RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPosition[] pathField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("RoadTraversalListPosition", IsNullable = false)]
            public RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPosition[] Path
            {
                get
                {
                    return this.pathField;
                }
                set
                {
                    this.pathField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPosition
        {

            private RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversal roadTraversalField;

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversal RoadTraversal
            {
                get
                {
                    return this.roadTraversalField;
                }
                set
                {
                    this.roadTraversalField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversal
        {

            private RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversalRoadIdentity roadIdentityField;

            private RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversalDistance distanceField;

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversalRoadIdentity RoadIdentity
            {
                get
                {
                    return this.roadIdentityField;
                }
                set
                {
                    this.roadIdentityField = value;
                }
            }

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversalDistance Distance
            {
                get
                {
                    return this.distanceField;
                }
                set
                {
                    this.distanceField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversalRoadIdentity
        {

            private string nameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversalDistance
        {

            private uint metricField;

            private uint imperialField;

            /// <remarks/>
            public uint Metric
            {
                get
                {
                    return this.metricField;
                }
                set
                {
                    this.metricField = value;
                }
            }

            /// <remarks/>
            public uint Imperial
            {
                get
                {
                    return this.imperialField;
                }
                set
                {
                    this.imperialField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartStructures
        {

            private RoutePartsRoutePartListPositionRoutePartRoadPartStructuresStructure[] structureField;

            private bool areMyResponsibilityOnlyField;

            private bool isStructureOwnerField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Structure")]
            public RoutePartsRoutePartListPositionRoutePartRoadPartStructuresStructure[] Structure
            {
                get
                {
                    return this.structureField;
                }
                set
                {
                    this.structureField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool AreMyResponsibilityOnly
            {
                get
                {
                    return this.areMyResponsibilityOnlyField;
                }
                set
                {
                    this.areMyResponsibilityOnlyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsStructureOwner
            {
                get
                {
                    return this.isStructureOwnerField;
                }
                set
                {
                    this.isStructureOwnerField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartStructuresStructure
        {

            private string eSRNField;

            private string nameField;

            private RoutePartsRoutePartListPositionRoutePartRoadPartStructuresStructureAppraisal appraisalField;

            private string traversalTypeField;

            /// <remarks/>
            public string ESRN
            {
                get
                {
                    return this.eSRNField;
                }
                set
                {
                    this.eSRNField = value;
                }
            }

            /// <remarks/>
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePartRoadPartStructuresStructureAppraisal Appraisal
            {
                get
                {
                    return this.appraisalField;
                }
                set
                {
                    this.appraisalField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string TraversalType
            {
                get
                {
                    return this.traversalTypeField;
                }
                set
                {
                    this.traversalTypeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartStructuresStructureAppraisal
        {

            private string suitabilityField;

            private string organisationField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
            public string Suitability
            {
                get
                {
                    return this.suitabilityField;
                }
                set
                {
                    this.suitabilityField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
            public string Organisation
            {
                get
                {
                    return this.organisationField;
                }
                set
                {
                    this.organisationField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/drivinginstruction", IsNullable = false)]
        public partial class DrivingInstructions
        {

            private DrivingInstructionsSubPartListPosition subPartListPositionField;

            private byte idField;

            private byte comparisonIdField;

            /// <remarks/>
            public DrivingInstructionsSubPartListPosition SubPartListPosition
            {
                get
                {
                    return this.subPartListPositionField;
                }
                set
                {
                    this.subPartListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte ComparisonId
            {
                get
                {
                    return this.comparisonIdField;
                }
                set
                {
                    this.comparisonIdField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionsSubPartListPosition
        {

            private DrivingInstructionsSubPartListPositionSubPart subPartField;

            /// <remarks/>
            public DrivingInstructionsSubPartListPositionSubPart SubPart
            {
                get
                {
                    return this.subPartField;
                }
                set
                {
                    this.subPartField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionsSubPartListPositionSubPart
        {

            private DrivingInstructionsSubPartListPositionSubPartAlternativeListPosition alternativeListPositionField;

            /// <remarks/>
            public DrivingInstructionsSubPartListPositionSubPartAlternativeListPosition AlternativeListPosition
            {
                get
                {
                    return this.alternativeListPositionField;
                }
                set
                {
                    this.alternativeListPositionField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPosition
        {

            private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPosition[] alternativeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("InstructionListPosition", IsNullable = false)]
            public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPosition[] Alternative
            {
                get
                {
                    return this.alternativeField;
                }
                set
                {
                    this.alternativeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPosition
        {

            private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstruction instructionField;

            /// <remarks/>
            public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstruction Instruction
            {
                get
                {
                    return this.instructionField;
                }
                set
                {
                    this.instructionField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstruction
        {

            private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNavigation navigationField;

            private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPosition[] noteListPositionField;

            /// <remarks/>
            public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNavigation Navigation
            {
                get
                {
                    return this.navigationField;
                }
                set
                {
                    this.navigationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("NoteListPosition")]
            public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPosition[] NoteListPosition
            {
                get
                {
                    return this.noteListPositionField;
                }
                set
                {
                    this.noteListPositionField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNavigation
        {

            private string instructionField;

            private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNavigationDistance distanceField;

            /// <remarks/>
            public string Instruction
            {
                get
                {
                    return this.instructionField;
                }
                set
                {
                    this.instructionField = value;
                }
            }

            /// <remarks/>
            public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNavigationDistance Distance
            {
                get
                {
                    return this.distanceField;
                }
                set
                {
                    this.distanceField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNavigationDistance
        {

            private ushort measuredMetricField;

            private ushort displayMetricField;

            private ushort displayImperialField;

            /// <remarks/>
            public ushort MeasuredMetric
            {
                get
                {
                    return this.measuredMetricField;
                }
                set
                {
                    this.measuredMetricField = value;
                }
            }

            /// <remarks/>
            public ushort DisplayMetric
            {
                get
                {
                    return this.displayMetricField;
                }
                set
                {
                    this.displayMetricField = value;
                }
            }

            /// <remarks/>
            public ushort DisplayImperial
            {
                get
                {
                    return this.displayImperialField;
                }
                set
                {
                    this.displayImperialField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPosition
        {

            private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNote noteField;

            /// <remarks/>
            public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNote Note
            {
                get
                {
                    return this.noteField;
                }
                set
                {
                    this.noteField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNote
        {

            private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNoteContent contentField;

            private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNoteGridReference gridReferenceField;

            private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNoteEncounteredAt encounteredAtField;

            /// <remarks/>
            public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNoteContent Content
            {
                get
                {
                    return this.contentField;
                }
                set
                {
                    this.contentField = value;
                }
            }

            /// <remarks/>
            public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNoteGridReference GridReference
            {
                get
                {
                    return this.gridReferenceField;
                }
                set
                {
                    this.gridReferenceField = value;
                }
            }

            /// <remarks/>
            public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNoteEncounteredAt EncounteredAt
            {
                get
                {
                    return this.encounteredAtField;
                }
                set
                {
                    this.encounteredAtField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNoteContent
        {

            private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNoteContentCaution cautionField;

            private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNoteContentMotorwayCaution motorwayCautionField;

            private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNoteContentRoutePoint routePointField;

            /// <remarks/>
            public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNoteContentCaution Caution
            {
                get
                {
                    return this.cautionField;
                }
                set
                {
                    this.cautionField = value;
                }
            }

            /// <remarks/>
            public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNoteContentMotorwayCaution MotorwayCaution
            {
                get
                {
                    return this.motorwayCautionField;
                }
                set
                {
                    this.motorwayCautionField = value;
                }
            }

            /// <remarks/>
            public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNoteContentRoutePoint RoutePoint
            {
                get
                {
                    return this.routePointField;
                }
                set
                {
                    this.routePointField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNoteContentCaution
        {

            private Action actionField;

            private CautionedEntity cautionedEntityField;

            private Contact contactField;

            private ushort cautionIdField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
            public Action Action
            {
                get
                {
                    return this.actionField;
                }
                set
                {
                    this.actionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
            public CautionedEntity CautionedEntity
            {
                get
                {
                    return this.cautionedEntityField;
                }
                set
                {
                    this.cautionedEntityField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
            public Contact Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort CautionId
            {
                get
                {
                    return this.cautionIdField;
                }
                set
                {
                    this.cautionIdField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/caution")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/caution", IsNullable = false)]
        public partial class Action
        {

            private string standardField;

            /// <remarks/>
            public string Standard
            {
                get
                {
                    return this.standardField;
                }
                set
                {
                    this.standardField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/caution")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/caution", IsNullable = false)]
        public partial class CautionedEntity
        {

            private CautionedEntityConstraint constraintField;

            private CautionedEntityStructure structureField;

            /// <remarks/>
            public CautionedEntityConstraint Constraint
            {
                get
                {
                    return this.constraintField;
                }
                set
                {
                    this.constraintField = value;
                }
            }

            /// <remarks/>
            public CautionedEntityStructure Structure
            {
                get
                {
                    return this.structureField;
                }
                set
                {
                    this.structureField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/caution")]
        public partial class CautionedEntityConstraint
        {

            private string eCRNField;

            private string typeField;

            private string constraintNameField;

            /// <remarks/>
            public string ECRN
            {
                get
                {
                    return this.eCRNField;
                }
                set
                {
                    this.eCRNField = value;
                }
            }

            /// <remarks/>
            public string Type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }

            /// <remarks/>
            public string ConstraintName
            {
                get
                {
                    return this.constraintNameField;
                }
                set
                {
                    this.constraintNameField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/caution")]
        public partial class CautionedEntityStructure
        {

            private string eSRNField;

            private string structureNameField;

            private byte sectionIdField;

            /// <remarks/>
            public string ESRN
            {
                get
                {
                    return this.eSRNField;
                }
                set
                {
                    this.eSRNField = value;
                }
            }

            /// <remarks/>
            public string StructureName
            {
                get
                {
                    return this.structureNameField;
                }
                set
                {
                    this.structureNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte SectionId
            {
                get
                {
                    return this.sectionIdField;
                }
                set
                {
                    this.sectionIdField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/caution")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/caution", IsNullable = false)]
        public partial class Contact
        {

            private object fullNameField;

            private object organisationNameField;

            private object telephoneNumberField;

            private object faxNumberField;

            private object emailAddressField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
            public object FullName
            {
                get
                {
                    return this.fullNameField;
                }
                set
                {
                    this.fullNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
            public object OrganisationName
            {
                get
                {
                    return this.organisationNameField;
                }
                set
                {
                    this.organisationNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
            public object TelephoneNumber
            {
                get
                {
                    return this.telephoneNumberField;
                }
                set
                {
                    this.telephoneNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
            public object FaxNumber
            {
                get
                {
                    return this.faxNumberField;
                }
                set
                {
                    this.faxNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
            public object EmailAddress
            {
                get
                {
                    return this.emailAddressField;
                }
                set
                {
                    this.emailAddressField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNoteContentMotorwayCaution
        {

            private string descriptionField;

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNoteContentRoutePoint
        {

            private string descriptionField;

            private string pointTypeField;

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string PointType
            {
                get
                {
                    return this.pointTypeField;
                }
                set
                {
                    this.pointTypeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNoteGridReference
        {

            private uint xField;

            private uint yField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.govtalk.gov.uk/people/bs7666")]
            public uint X
            {
                get
                {
                    return this.xField;
                }
                set
                {
                    this.xField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.govtalk.gov.uk/people/bs7666")]
            public uint Y
            {
                get
                {
                    return this.yField;
                }
                set
                {
                    this.yField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNoteEncounteredAt
        {

            private ushort measuredMetricField;

            private ushort displayMetricField;

            private ushort displayImperialField;

            /// <remarks/>
            public ushort MeasuredMetric
            {
                get
                {
                    return this.measuredMetricField;
                }
                set
                {
                    this.measuredMetricField = value;
                }
            }

            /// <remarks/>
            public ushort DisplayMetric
            {
                get
                {
                    return this.displayMetricField;
                }
                set
                {
                    this.displayMetricField = value;
                }
            }

            /// <remarks/>
            public ushort DisplayImperial
            {
                get
                {
                    return this.displayImperialField;
                }
                set
                {
                    this.displayImperialField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
        public partial class PredefinedCautions
        {

            private byte dockCautionDescriptionField;

            private object motorwayCautionDescriptionField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
            public byte DockCautionDescription
            {
                get
                {
                    return this.dockCautionDescriptionField;
                }
                set
                {
                    this.dockCautionDescriptionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
            public object MotorwayCautionDescription
            {
                get
                {
                    return this.motorwayCautionDescriptionField;
                }
                set
                {
                    this.motorwayCautionDescriptionField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/agreedroute")]
        public partial class AgreedRouteOrderSummary
        {

            private OrderSummary[] signedField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("OrderSummary", Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
            public OrderSummary[] Signed
            {
                get
                {
                    return this.signedField;
                }
                set
                {
                    this.signedField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
        public partial class Recipients
        {

            private RecipientsContact[] contactField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Contact")]
            public RecipientsContact[] Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }
        }


    }

}