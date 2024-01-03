using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Applications
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/agreedroute")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/agreedroute", IsNullable = false)]
    public partial class AgreedRoute
    {

        private ESDALReferenceNumber eSDALReferenceNumberField;

        private string distributionCommentsField;

        private string jobFileReferenceField;

        private System.DateTime sentDateTimeField;

        private HAContactDet hAContactField;

        private RecipientsContact[] recipientsField;

        private HaulierDetail haulierDetailsField;

        private JourneyFromToSummaryDet journeyFromToSummaryField;

        private JourneyFromToDet journeyFromToField;

        private JourneyTimingDet journeyTimingField;

        private string loadSummaryField;

        private LoadDetail loadDetailsField;

        private RouteParts routePartsField;

        private string notesFromHaulierField;

        private NotesForHaulier notesForHaulierField;

        private AgreedRouteOrderSummary orderSummaryField;

        private string statusField;

        private bool isFailedDelegationAlertField;

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
        public string JobFileReference
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
        public HAContactDet HAContact
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
        public HaulierDetail HaulierDetails
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
        public JourneyFromToSummaryDet JourneyFromToSummary
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
        public JourneyFromToDet JourneyFromTo
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
        public JourneyTimingDet JourneyTiming
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
        public string LoadSummary
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
        public LoadDetail LoadDetails
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
        public string NotesFromHaulier
        {
            get
            {
                return this.notesFromHaulierField;
            }
            set
            {
                this.notesFromHaulierField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public NotesForHaulier NotesForHaulier
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool IsFailedDelegationAlert
        {
            get
            {
                return this.isFailedDelegationAlertField;
            }
            set
            {
                this.isFailedDelegationAlertField = value;
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

        private byte movementProjectNumberField;

        private bool enteredBySORTField;

        private ESDALReferenceNumberMovementVersion movementVersionField;

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
        public byte MovementProjectNumber
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
        public bool EnteredBySORT
        {
            get
            {
                return this.enteredBySORTField;
            }
            set
            {
                this.enteredBySORTField = value;
            }
        }

        /// <remarks/>
        public ESDALReferenceNumberMovementVersion MovementVersion
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
    public partial class ESDALReferenceNumberMovementVersion
    {

        private bool createdBySortField;

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool CreatedBySort
        {
            get
            {
                return this.createdBySortField;
            }
            set
            {
                this.createdBySortField = value;
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
    public partial class OrderSummary
    {

        private OrderSummaryCurrentOrder currentOrderField;

        private OrderSummaryReplacedOrder replacedOrderField;

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

        /// <remarks/>
        public OrderSummaryReplacedOrder ReplacedOrder
        {
            get
            {
                return this.replacedOrderField;
            }
            set
            {
                this.replacedOrderField = value;
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

        private bool isAmendmentOrderField;

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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool IsAmendmentOrder
        {
            get
            {
                return this.isAmendmentOrderField;
            }
            set
            {
                this.isAmendmentOrderField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
    public partial class OrderSummaryReplacedOrder
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
    public partial class HAContactDet
    {

        private string contactField;

        private HAContactAddress addressField;

        private string telephoneNumberField;

        private string faxNumberField;

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
        public string FaxNumber
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

        private string faxField;

        private string emailField;

        private bool isPoliceField;

        private bool isPoliceFieldSpecified;

        private bool isRecipientField;

        private string reasonField;

        private bool isHaulierField;

        private bool isHaulierFieldSpecified;

        private ushort contactIdField;

        private bool contactIdFieldSpecified;

        private bool isRetainedNotificationOnlyField;

        private bool isRetainedNotificationOnlyFieldSpecified;

        private ushort organisationIdField;

        private bool organisationIdFieldSpecified;

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
        public string Fax
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
        public string Email
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsPoliceSpecified
        {
            get
            {
                return this.isPoliceFieldSpecified;
            }
            set
            {
                this.isPoliceFieldSpecified = value;
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsHaulierSpecified
        {
            get
            {
                return this.isHaulierFieldSpecified;
            }
            set
            {
                this.isHaulierFieldSpecified = value;
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ContactIdSpecified
        {
            get
            {
                return this.contactIdFieldSpecified;
            }
            set
            {
                this.contactIdFieldSpecified = value;
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

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsRetainedNotificationOnlySpecified
        {
            get
            {
                return this.isRetainedNotificationOnlyFieldSpecified;
            }
            set
            {
                this.isRetainedNotificationOnlyFieldSpecified = value;
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool OrganisationIdSpecified
        {
            get
            {
                return this.organisationIdFieldSpecified;
            }
            set
            {
                this.organisationIdFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
    public partial class HaulierDetail
    {

        private string haulierContactField;

        private string haulierNameField;

        private HaulierDetailsHaulierAddress haulierAddressField;

        private ulong telephoneNumberField;

        private string faxNumberField;

        private string emailAddressField;

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
        public ulong TelephoneNumber
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
        public string FaxNumber
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
    public partial class HaulierDetailsHaulierAddress
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
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
    public partial class JourneyFromToSummaryDet
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
    public partial class JourneyFromToDet
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
    public partial class JourneyTimingDet
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
    public partial class LoadDetail
    {

        private string descriptionField;

        private byte totalMovesField;

        private byte maxPiecesPerMoveField;

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

        private byte legNumberField;

        private string nameField;

        private RoutePartsRoutePartListPositionRoutePartRoadPart roadPartField;

        private string modeField;

        private ushort idField;

        /// <remarks/>
        public byte LegNumber
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
        public ushort Id
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

        private RoutePartsRoutePartListPositionRoutePartRoadPartRouteDescription routeDescriptionField;

        private RoutePartsRoutePartListPositionRoutePartRoadPartStructures structuresField;

        private DrivingInstructions drivingInstructionsField;

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
        public RoutePartsRoutePartListPositionRoutePartRoadPartRouteDescription RouteDescription
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

        private bool isBrokenField;

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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool IsBroken
        {
            get
            {
                return this.isBrokenField;
            }
            set
            {
                this.isBrokenField = value;
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

        private bool isBrokenField;

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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool IsBroken
        {
            get
            {
                return this.isBrokenField;
            }
            set
            {
                this.isBrokenField = value;
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

        private byte distanceField;

        /// <remarks/>
        public byte Distance
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

        private byte distanceField;

        /// <remarks/>
        public byte Distance
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

        private ConfigurationSummaryListPosition configurationSummaryListPositionField;

        private OverallLengthListPosition overallLengthListPositionField;

        private RearOverhangListPosition rearOverhangListPositionField;

        private FrontOverhangListPosition frontOverhangListPositionField;

        private OverallWidthListPosition overallWidthListPositionField;

        private OverallHeightListPosition overallHeightListPositionField;

        private GrossWeightListPosition grossWeightListPositionField;

        private MaxAxleWeightListPosition maxAxleWeightListPositionField;

        private VehicleSummaryListPosition vehicleSummaryListPositionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public ConfigurationSummaryListPosition ConfigurationSummaryListPosition
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
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public OverallLengthListPosition OverallLengthListPosition
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
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public RearOverhangListPosition RearOverhangListPosition
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
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public FrontOverhangListPosition FrontOverhangListPosition
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
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public OverallWidthListPosition OverallWidthListPosition
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
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public OverallHeightListPosition OverallHeightListPosition
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
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public GrossWeightListPosition GrossWeightListPosition
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
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public MaxAxleWeightListPosition MaxAxleWeightListPosition
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
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public VehicleSummaryListPosition VehicleSummaryListPosition
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

        private byte includingProjectionsField;

        private byte excludingProjectionsField;

        /// <remarks/>
        public byte IncludingProjections
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
        public byte ExcludingProjections
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
    public partial class FrontOverhangListPosition
    {

        private FrontOverhangListPositionFrontOverhang frontOverhangField;

        /// <remarks/>
        public FrontOverhangListPositionFrontOverhang FrontOverhang
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class FrontOverhangListPositionFrontOverhang
    {

        private bool infrontOfCabField;

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool InfrontOfCab
        {
            get
            {
                return this.infrontOfCabField;
            }
            set
            {
                this.infrontOfCabField = value;
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
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle", IsNullable = false)]
    public partial class OverallWidthListPosition
    {

        private byte overallWidthField;

        /// <remarks/>
        public byte OverallWidth
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

        private byte maxHeightField;

        private byte reducibleHeightField;

        /// <remarks/>
        public byte MaxHeight
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
        public byte ReducibleHeight
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

        private bool excludesTractorsField;

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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool ExcludesTractors
        {
            get
            {
                return this.excludesTractorsField;
            }
            set
            {
                this.excludesTractorsField = value;
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

        private VehicleSummaryListPositionVehicleSummaryConfiguration configurationField;

        private string configurationTypeField;

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

        private VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicle nonSemiVehicleField;

        /// <remarks/>
        public VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicle NonSemiVehicle
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
    public partial class VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicle
    {

        private VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPosition componentListPositionField;

        /// <remarks/>
        public VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPosition ComponentListPosition
        {
            get
            {
                return this.componentListPositionField;
            }
            set
            {
                this.componentListPositionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPosition
    {

        private VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponent componentField;

        /// <remarks/>
        public VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponent Component
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
    public partial class VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponent
    {

        private VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearing loadBearingField;

        private string componentSubTypeField;

        private string componentTypeField;

        private byte longitudeField;

        /// <remarks/>
        public VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearing LoadBearing
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearing
    {

        private string summaryField;

        private VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingWeight weightField;

        private bool isSteerableAtRearField;

        private VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfiguration axleConfigurationField;

        private byte rigidLengthField;

        private byte widthField;

        private VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingHeight heightField;

        private decimal wheelbaseField;

        private byte leftOverhangField;

        private byte rightOverhangField;

        private byte frontOverhangField;

        private byte rearOverhangField;

        private byte groundClearanceField;

        private byte outsideTrackField;

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
        public VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingWeight Weight
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
        public VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfiguration AxleConfiguration
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
        public byte RigidLength
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
        public byte Width
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
        public VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingHeight Height
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
        public byte GroundClearance
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingWeight
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
    public partial class VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfiguration
    {

        private byte numberOfAxlesField;

        private VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationAxleWeightListPosition axleWeightListPositionField;

        private VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationWheelsPerAxleListPosition wheelsPerAxleListPositionField;

        private VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationAxleSpacingListPosition[] axleSpacingListPositionField;

        private VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationTyreSizeListPosition tyreSizeListPositionField;

        private VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationWheelSpacingListPosition wheelSpacingListPositionField;

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
        public VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationAxleWeightListPosition AxleWeightListPosition
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
        public VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationWheelsPerAxleListPosition WheelsPerAxleListPosition
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
        public VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationAxleSpacingListPosition[] AxleSpacingListPosition
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
        public VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationTyreSizeListPosition TyreSizeListPosition
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
        public VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationWheelSpacingListPosition WheelSpacingListPosition
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
    public partial class VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationAxleWeightListPosition
    {

        private VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationAxleWeightListPositionAxleWeight axleWeightField;

        /// <remarks/>
        public VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationAxleWeightListPositionAxleWeight AxleWeight
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
    public partial class VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationAxleWeightListPositionAxleWeight
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
    public partial class VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationWheelsPerAxleListPosition
    {

        private VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationWheelsPerAxleListPositionWheelsPerAxle wheelsPerAxleField;

        /// <remarks/>
        public VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationWheelsPerAxleListPositionWheelsPerAxle WheelsPerAxle
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
    public partial class VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationWheelsPerAxleListPositionWheelsPerAxle
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
    public partial class VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationAxleSpacingListPosition
    {

        private VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationAxleSpacingListPositionAxleSpacing axleSpacingField;

        /// <remarks/>
        public VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationAxleSpacingListPositionAxleSpacing AxleSpacing
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
    public partial class VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationAxleSpacingListPositionAxleSpacing
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
    public partial class VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationTyreSizeListPosition
    {

        private VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationTyreSizeListPositionTyreSize tyreSizeField;

        /// <remarks/>
        public VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationTyreSizeListPositionTyreSize TyreSize
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
    public partial class VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationTyreSizeListPositionTyreSize
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
    public partial class VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationWheelSpacingListPosition
    {

        private VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationWheelSpacingListPositionWheelSpacing wheelSpacingField;

        /// <remarks/>
        public VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationWheelSpacingListPositionWheelSpacing WheelSpacing
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
    public partial class VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingAxleConfigurationWheelSpacingListPositionWheelSpacing
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
    public partial class VehicleSummaryListPositionVehicleSummaryConfigurationNonSemiVehicleComponentListPositionComponentLoadBearingHeight
    {

        private byte maxHeightField;

        /// <remarks/>
        public byte MaxHeight
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
    public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRoads
    {

        private RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPosition routeSubPartListPositionField;

        private bool isBrokenField;

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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool IsBroken
        {
            get
            {
                return this.isBrokenField;
            }
            set
            {
                this.isBrokenField = value;
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

        private RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionPath pathField;

        /// <remarks/>
        public RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionPath Path
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
    public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionPath
    {

        private RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionPathRoadTraversalListPosition roadTraversalListPositionField;

        /// <remarks/>
        public RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionPathRoadTraversalListPosition RoadTraversalListPosition
        {
            get
            {
                return this.roadTraversalListPositionField;
            }
            set
            {
                this.roadTraversalListPositionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
    public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionPathRoadTraversalListPosition
    {

        private RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionPathRoadTraversalListPositionRoadTraversal roadTraversalField;

        /// <remarks/>
        public RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionPathRoadTraversalListPositionRoadTraversal RoadTraversal
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
    public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionPathRoadTraversalListPositionRoadTraversal
    {

        private RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionPathRoadTraversalListPositionRoadTraversalRoadIdentity roadIdentityField;

        private RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionPathRoadTraversalListPositionRoadTraversalDistance distanceField;

        private RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionPathRoadTraversalListPositionRoadTraversalContiguousRoadDistance contiguousRoadDistanceField;

        private object constraintsField;

        /// <remarks/>
        public RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionPathRoadTraversalListPositionRoadTraversalRoadIdentity RoadIdentity
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
        public RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionPathRoadTraversalListPositionRoadTraversalDistance Distance
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
        public RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionPathRoadTraversalListPositionRoadTraversalContiguousRoadDistance ContiguousRoadDistance
        {
            get
            {
                return this.contiguousRoadDistanceField;
            }
            set
            {
                this.contiguousRoadDistanceField = value;
            }
        }

        /// <remarks/>
        public object Constraints
        {
            get
            {
                return this.constraintsField;
            }
            set
            {
                this.constraintsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
    public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionPathRoadTraversalListPositionRoadTraversalRoadIdentity
    {

        private string numberField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public string Number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
    public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionPathRoadTraversalListPositionRoadTraversalDistance
    {

        private byte metricField;

        private byte imperialField;

        /// <remarks/>
        public byte Metric
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
        public byte Imperial
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
    public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionPathRoadTraversalListPositionRoadTraversalContiguousRoadDistance
    {

        private byte metricField;

        private byte imperialField;

        /// <remarks/>
        public byte Metric
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
        public byte Imperial
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
    public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRouteDescription
    {

        private SubpartListPosition subpartListPositionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/routedescription")]
        public SubpartListPosition SubpartListPosition
        {
            get
            {
                return this.subpartListPositionField;
            }
            set
            {
                this.subpartListPositionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routedescription")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/routedescription", IsNullable = false)]
    public partial class SubpartListPosition
    {

        private SubpartListPositionSubpart subpartField;

        /// <remarks/>
        public SubpartListPositionSubpart Subpart
        {
            get
            {
                return this.subpartField;
            }
            set
            {
                this.subpartField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routedescription")]
    public partial class SubpartListPositionSubpart
    {

        private SubpartListPositionSubpartPathListPosition pathListPositionField;

        /// <remarks/>
        public SubpartListPositionSubpartPathListPosition PathListPosition
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routedescription")]
    public partial class SubpartListPositionSubpartPathListPosition
    {

        private SubpartListPositionSubpartPathListPositionPath pathField;

        /// <remarks/>
        public SubpartListPositionSubpartPathListPositionPath Path
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routedescription")]
    public partial class SubpartListPositionSubpartPathListPositionPath
    {

        private SubpartListPositionSubpartPathListPositionPathDescription descriptionField;

        private SubpartListPositionSubpartPathListPositionPathSegmentListPosition segmentListPositionField;

        /// <remarks/>
        public SubpartListPositionSubpartPathListPositionPathDescription Description
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
        public SubpartListPositionSubpartPathListPositionPathSegmentListPosition SegmentListPosition
        {
            get
            {
                return this.segmentListPositionField;
            }
            set
            {
                this.segmentListPositionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routedescription")]
    public partial class SubpartListPositionSubpartPathListPositionPathDescription
    {

        private byte alternativeNumberField;

        private string descriptionField;

        /// <remarks/>
        public byte AlternativeNumber
        {
            get
            {
                return this.alternativeNumberField;
            }
            set
            {
                this.alternativeNumberField = value;
            }
        }

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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routedescription")]
    public partial class SubpartListPositionSubpartPathListPositionPathSegmentListPosition
    {

        private SubpartListPositionSubpartPathListPositionPathSegmentListPositionSegment segmentField;

        /// <remarks/>
        public SubpartListPositionSubpartPathListPositionPathSegmentListPositionSegment Segment
        {
            get
            {
                return this.segmentField;
            }
            set
            {
                this.segmentField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routedescription")]
    public partial class SubpartListPositionSubpartPathListPositionPathSegmentListPositionSegment
    {

        private string typeField;

        private SubpartListPositionSubpartPathListPositionPathSegmentListPositionSegmentInstructionListPosition instructionListPositionField;

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
        public SubpartListPositionSubpartPathListPositionPathSegmentListPositionSegmentInstructionListPosition InstructionListPosition
        {
            get
            {
                return this.instructionListPositionField;
            }
            set
            {
                this.instructionListPositionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routedescription")]
    public partial class SubpartListPositionSubpartPathListPositionPathSegmentListPositionSegmentInstructionListPosition
    {

        private SubpartListPositionSubpartPathListPositionPathSegmentListPositionSegmentInstructionListPositionInstruction instructionField;

        /// <remarks/>
        public SubpartListPositionSubpartPathListPositionPathSegmentListPositionSegmentInstructionListPositionInstruction Instruction
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routedescription")]
    public partial class SubpartListPositionSubpartPathListPositionPathSegmentListPositionSegmentInstructionListPositionInstruction
    {

        private SubpartListPositionSubpartPathListPositionPathSegmentListPositionSegmentInstructionListPositionInstructionDirectionInstruction directionInstructionField;

        /// <remarks/>
        public SubpartListPositionSubpartPathListPositionPathSegmentListPositionSegmentInstructionListPositionInstructionDirectionInstruction DirectionInstruction
        {
            get
            {
                return this.directionInstructionField;
            }
            set
            {
                this.directionInstructionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routedescription")]
    public partial class SubpartListPositionSubpartPathListPositionPathSegmentListPositionSegmentInstructionListPositionInstructionDirectionInstruction
    {

        private SubpartListPositionSubpartPathListPositionPathSegmentListPositionSegmentInstructionListPositionInstructionDirectionInstructionRoadIdentity roadIdentityField;

        private SubpartListPositionSubpartPathListPositionPathSegmentListPositionSegmentInstructionListPositionInstructionDirectionInstructionDistance distanceField;

        private string directionField;

        /// <remarks/>
        public SubpartListPositionSubpartPathListPositionPathSegmentListPositionSegmentInstructionListPositionInstructionDirectionInstructionRoadIdentity RoadIdentity
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
        public SubpartListPositionSubpartPathListPositionPathSegmentListPositionSegmentInstructionListPositionInstructionDirectionInstructionDistance Distance
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
        public string Direction
        {
            get
            {
                return this.directionField;
            }
            set
            {
                this.directionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routedescription")]
    public partial class SubpartListPositionSubpartPathListPositionPathSegmentListPositionSegmentInstructionListPositionInstructionDirectionInstructionRoadIdentity
    {

        private string numberField;

        private bool unidentifiedField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public string Number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Unidentified
        {
            get
            {
                return this.unidentifiedField;
            }
            set
            {
                this.unidentifiedField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routedescription")]
    public partial class SubpartListPositionSubpartPathListPositionPathSegmentListPositionSegmentInstructionListPositionInstructionDirectionInstructionDistance
    {

        private byte measuredMetricField;

        private byte displayMetricField;

        private byte displayImperialField;

        /// <remarks/>
        public byte MeasuredMetric
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
        public byte DisplayMetric
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
        public byte DisplayImperial
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
    public partial class RoutePartsRoutePartListPositionRoutePartRoadPartStructures
    {

        private bool areMyResponsibilityOnlyField;

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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/drivinginstruction", IsNullable = false)]
    public partial class DrivingInstructions
    {

        private byte legNumberField;

        private string nameField;

        private DrivingInstructionsSubPartListPosition subPartListPositionField;

        private ushort comparisonIdField;

        private ushort idField;

        /// <remarks/>
        public byte LegNumber
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
        public ushort ComparisonId
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort Id
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

        private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNavigationInstruction instructionField;

        private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNavigationDistance distanceField;

        /// <remarks/>
        public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNavigationInstruction Instruction
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
    public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNavigationInstruction
    {

        private Bold boldField;

        private string[] textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
        public Bold Bold
        {
            get
            {
                return this.boldField;
            }
            set
            {
                this.boldField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext", IsNullable = false)]
    public partial class Bold
    {

        private string underscoreField;

        private string[] textField;

        /// <remarks/>
        public string Underscore
        {
            get
            {
                return this.underscoreField;
            }
            set
            {
                this.underscoreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
    public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNavigationDistance
    {

        private byte measuredMetricField;

        private byte displayMetricField;

        private byte displayImperialField;

        /// <remarks/>
        public byte MeasuredMetric
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
        public byte DisplayMetric
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
        public byte DisplayImperial
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

        private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNoteContentAnnotation annotationField;

        private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNoteContentRoutePoint routePointField;

        /// <remarks/>
        public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNoteContentAnnotation Annotation
        {
            get
            {
                return this.annotationField;
            }
            set
            {
                this.annotationField = value;
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
    public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionInstructionListPositionInstructionNoteListPositionNoteContentAnnotation
    {

        private string textField;

        private uint annotationIdField;

        private string annotationTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/annotation")]
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint AnnotationId
        {
            get
            {
                return this.annotationIdField;
            }
            set
            {
                this.annotationIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string AnnotationType
        {
            get
            {
                return this.annotationTypeField;
            }
            set
            {
                this.annotationTypeField = value;
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

        private byte measuredMetricField;

        private byte displayMetricField;

        private byte displayImperialField;

        /// <remarks/>
        public byte MeasuredMetric
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
        public byte DisplayMetric
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
        public byte DisplayImperial
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
    public partial class NotesForHaulier
    {

        private Bold[] boldField;

        private string[] textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Bold", Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
        public Bold[] Bold
        {
            get
            {
                return this.boldField;
            }
            set
            {
                this.boldField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
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


