using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.DocumentsAndContents
{
    public class SortAgreeRevisedParams
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

            private string sentDateTimeField;

            private HAContact hAContactField;

            private RecipientsContact[] recipientsField;

            private HaulierDetails haulierDetailsField;

            private string hauliersReferenceField;

            private JourneyFromToSummary journeyFromToSummaryField;

            private JourneyFromTo journeyFromToField;

            private JourneyTiming journeyTimingField;

            private object loadSummaryField;

            private LoadDetails loadDetailsField;

            private RoutePartsRoutePartListPosition[] routePartsField;

            private PredefinedCautions predefinedCautionsField;

            private uint projectIDField;

            private string versionIDField;

            private string versionStatusField;

            private AgreedRouteOrderSummary orderSummaryField;

            private string statusField;

            private NotesForHaulier notesForHaulierField;

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
            public string SentDateTime
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
            [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
            [System.Xml.Serialization.XmlArrayItemAttribute("RoutePartListPosition", IsNullable = false)]
            public RoutePartsRoutePartListPosition[] RouteParts
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
            public string VersionID
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
            public string VersionStatus
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

            private string movementVersionField;

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
            public string MovementVersion
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

            private string emailField;

            private RecipientsContactOnbehalfOf onbehalfOfField;

            private string contactIdField;

            private ushort organisationIdField;

            private bool isRecipientField;

            private bool isPoliceField;

            private bool isHaulierField;

            private bool isRetainedNotificationOnlyField;

            private string reasonField;

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
            public string ContactId
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
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RecipientsContactOnbehalfOf
        {

            private string delegatorsOrganisationNameField;

            private ushort delegatorsOrganisationIdField;

            private bool retainNotificationField;

            private bool wantsFailureAlertField;

            private ushort delegationIdField;

            private string delegatorsContactIdField;

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
            public bool RetainNotification
            {
                get
                {
                    return this.retainNotificationField;
                }
                set
                {
                    this.retainNotificationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool WantsFailureAlert
            {
                get
                {
                    return this.wantsFailureAlertField;
                }
                set
                {
                    this.wantsFailureAlertField = value;
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

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string DelegatorsContactId
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

            private object licenceField;

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
            public object Licence
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

            private string firstMoveDateField;

            private string lastMoveDateField;

            /// <remarks/>
            public string FirstMoveDate
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
            public string LastMoveDate
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

            private string idField;

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

            private RoutePartsRoutePartListPositionRoutePartRoadPartRouteSubPartListPosition[] roadsField;

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
            [System.Xml.Serialization.XmlArrayItemAttribute("RouteSubPartListPosition", IsNullable = false)]
            public RoutePartsRoutePartListPositionRoutePartRoadPartRouteSubPartListPosition[] Roads
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

            private string distanceField;

            /// <remarks/>
            public string Distance
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

            private string distanceField;

            /// <remarks/>
            public string Distance
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

            private RigidLengthListPosition rigidLengthListPositionField;

            private RearOverhangListPosition rearOverhangListPositionField;

            private GroundClearancegListPosition groundClearancegListPositionField;

            private ReducedGroundClearanceListPosition reducedGroundClearanceListPositionField;

            private LeftOverhangListPosition leftOverhangListPositionField;

            private RightOverhangListPosition rightOverhangListPositionField;

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
            public RigidLengthListPosition RigidLengthListPosition
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
            public GroundClearancegListPosition GroundClearancegListPosition
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
        public partial class RigidLengthListPosition
        {

            private byte rigidLengthField;

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

            private byte groundClearanceField;

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
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle", IsNullable = false)]
        public partial class ReducedGroundClearanceListPosition
        {

            private byte reducedGroundClearanceField;

            /// <remarks/>
            public byte ReducedGroundClearance
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

            private decimal maxHeightField;

            private byte reducibleHeightField;

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

            private byte alternativeIdField;

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

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte AlternativeId
            {
                get
                {
                    return this.alternativeIdField;
                }
                set
                {
                    this.alternativeIdField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfiguration
        {

            private VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicle semiVehicleField;

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicle SemiVehicle
            {
                get
                {
                    return this.semiVehicleField;
                }
                set
                {
                    this.semiVehicleField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicle
        {

            private string summaryField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleGrossWeight grossWeightField;

            private bool isSteerableAtRearField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleMaxAxleWeight maxAxleWeightField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfiguration axleConfigurationField;

            private byte rigidLengthField;

            private byte widthField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleHeight heightField;

            private decimal wheelbaseField;

            private byte leftOverhangField;

            private byte rightOverhangField;

            private byte frontOverhangField;

            private byte rearOverhangField;

            private byte groundClearanceField;

            private byte reducedGroundClearanceField;

            private byte outsideTrackField;

            private string tractorSubTypeField;

            private string trailerSubTypeField;

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
            public VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleGrossWeight GrossWeight
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
            public VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleMaxAxleWeight MaxAxleWeight
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
            public VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfiguration AxleConfiguration
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
            public VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleHeight Height
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
            public byte ReducedGroundClearance
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
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string TractorSubType
            {
                get
                {
                    return this.tractorSubTypeField;
                }
                set
                {
                    this.tractorSubTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string TrailerSubType
            {
                get
                {
                    return this.trailerSubTypeField;
                }
                set
                {
                    this.trailerSubTypeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleGrossWeight
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
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleMaxAxleWeight
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
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfiguration
        {

            private byte numberOfAxlesField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationAxleWeightListPosition[] axleWeightListPositionField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationWheelsPerAxleListPosition[] wheelsPerAxleListPositionField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationAxleSpacingListPosition[] axleSpacingListPositionField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationTyreSizeListPosition[] tyreSizeListPositionField;

            private VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationWheelSpacingListPosition[] wheelSpacingListPositionField;

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
            public VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationAxleWeightListPosition[] AxleWeightListPosition
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
            public VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationWheelsPerAxleListPosition[] WheelsPerAxleListPosition
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
            public VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationAxleSpacingListPosition[] AxleSpacingListPosition
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
            public VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationTyreSizeListPosition[] TyreSizeListPosition
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
            public VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationWheelSpacingListPosition[] WheelSpacingListPosition
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
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationAxleWeightListPosition
        {

            private VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationAxleWeightListPositionAxleWeight axleWeightField;

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationAxleWeightListPositionAxleWeight AxleWeight
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
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationAxleWeightListPositionAxleWeight
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
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationWheelsPerAxleListPosition
        {

            private VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationWheelsPerAxleListPositionWheelsPerAxle wheelsPerAxleField;

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationWheelsPerAxleListPositionWheelsPerAxle WheelsPerAxle
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
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationWheelsPerAxleListPositionWheelsPerAxle
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
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationAxleSpacingListPosition
        {

            private VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationAxleSpacingListPositionAxleSpacing axleSpacingField;

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationAxleSpacingListPositionAxleSpacing AxleSpacing
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
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationAxleSpacingListPositionAxleSpacing
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
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationTyreSizeListPosition
        {

            private VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationTyreSizeListPositionTyreSize tyreSizeField;

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationTyreSizeListPositionTyreSize TyreSize
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
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationTyreSizeListPositionTyreSize
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
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationWheelSpacingListPosition
        {

            private VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationWheelSpacingListPositionWheelSpacing wheelSpacingField;

            /// <remarks/>
            public VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationWheelSpacingListPositionWheelSpacing WheelSpacing
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
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationWheelSpacingListPositionWheelSpacing
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
        public partial class VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleHeight
        {

            private decimal maxHeightField;

            private byte reducibleHeightField;

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
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRouteSubPartListPosition
        {

            private RoutePartsRoutePartListPositionRoutePartRoadPartRouteSubPartListPositionRouteSubPart routeSubPartField;

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePartRoadPartRouteSubPartListPositionRouteSubPart RouteSubPart
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
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRouteSubPartListPositionRouteSubPart
        {

            private RoutePartsRoutePartListPositionRoutePartRoadPartRouteSubPartListPositionRouteSubPartPathListPosition pathListPositionField;

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePartRoadPartRouteSubPartListPositionRouteSubPartPathListPosition PathListPosition
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
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRouteSubPartListPositionRouteSubPartPathListPosition
        {

            private RoutePartsRoutePartListPositionRoutePartRoadPartRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPosition[] pathField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("RoadTraversalListPosition", IsNullable = false)]
            public RoutePartsRoutePartListPositionRoutePartRoadPartRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPosition[] Path
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
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPosition
        {

            private RoutePartsRoutePartListPositionRoutePartRoadPartRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversal roadTraversalField;

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePartRoadPartRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversal RoadTraversal
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
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversal
        {

            private RoutePartsRoutePartListPositionRoutePartRoadPartRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversalRoadIdentity roadIdentityField;

            private RoutePartsRoutePartListPositionRoutePartRoadPartRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversalDistance distanceField;

            /// <remarks/>
            public RoutePartsRoutePartListPositionRoutePartRoadPartRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversalRoadIdentity RoadIdentity
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
            public RoutePartsRoutePartListPositionRoutePartRoadPartRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversalDistance Distance
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
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversalRoadIdentity
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
        public partial class RoutePartsRoutePartListPositionRoutePartRoadPartRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversalDistance
        {

            private string metricField;

            private string imperialField;

            /// <remarks/>
            public string Metric
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
            public string Imperial
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

            private DrivingInstructionsSubPartListPosition[] subPartListPositionField;

            private byte idField;

            private byte comparisonIdField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("SubPartListPosition")]
            public DrivingInstructionsSubPartListPosition[] SubPartListPosition
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

            private DrivingInstructionsSubPartListPositionAlternativeListPosition[] subPartField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("AlternativeListPosition", IsNullable = false)]
            public DrivingInstructionsSubPartListPositionAlternativeListPosition[] SubPart
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
        public partial class DrivingInstructionsSubPartListPositionAlternativeListPosition
        {

            private DrivingInstructionsSubPartListPositionAlternativeListPositionAlternative alternativeField;

            /// <remarks/>
            public DrivingInstructionsSubPartListPositionAlternativeListPositionAlternative Alternative
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
        public partial class DrivingInstructionsSubPartListPositionAlternativeListPositionAlternative
        {

            private DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeAlternativeDescription alternativeDescriptionField;

            private DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPosition[] instructionListPositionField;

            /// <remarks/>
            public DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeAlternativeDescription AlternativeDescription
            {
                get
                {
                    return this.alternativeDescriptionField;
                }
                set
                {
                    this.alternativeDescriptionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("InstructionListPosition")]
            public DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPosition[] InstructionListPosition
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
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeAlternativeDescription
        {

            private byte alternativeNoField;

            private string descriptionField;

            /// <remarks/>
            public byte AlternativeNo
            {
                get
                {
                    return this.alternativeNoField;
                }
                set
                {
                    this.alternativeNoField = value;
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
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPosition
        {

            private DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstruction instructionField;

            /// <remarks/>
            public DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstruction Instruction
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
        public partial class DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstruction
        {

            private DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNavigation navigationField;

            private DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPosition noteListPositionField;

            /// <remarks/>
            public DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNavigation Navigation
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
            public DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPosition NoteListPosition
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
        public partial class DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNavigation
        {

            private string instructionField;

            private DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNavigationDistance distanceField;

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
            public DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNavigationDistance Distance
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
        public partial class DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNavigationDistance
        {

            private string measuredMetricField;

            private string displayMetricField;

            private string displayImperialField;

            /// <remarks/>
            public string MeasuredMetric
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
            public string DisplayMetric
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
            public string DisplayImperial
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
        public partial class DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPosition
        {

            private DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNote noteField;

            /// <remarks/>
            public DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNote Note
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
        public partial class DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNote
        {

            private DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNoteContent contentField;

            private DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNoteGridReference gridReferenceField;

            private DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNoteEncounteredAt encounteredAtField;

        
            /// <remarks/>
            public DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNoteContent Content
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
            public DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNoteGridReference GridReference
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
            public DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNoteEncounteredAt EncounteredAt
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
        public partial class DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNoteContent
        {

            private DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNoteContentAnnotation annotationField;

            private DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNoteContentRoutePoint routePointField;

           
            /// <remarks/>
            public DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNoteContentAnnotation Annotation
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
            public DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNoteContentRoutePoint RoutePoint
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
        public partial class DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNoteContentAnnotation
        {

            private string textField;

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
        public partial class DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNoteContentRoutePoint
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
        public partial class DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNoteGridReference
        {

            private string xField;

            private string yField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.govtalk.gov.uk/people/bs7666")]
            public string X
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
            public string Y
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
        public partial class DrivingInstructionsSubPartListPositionAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNoteEncounteredAt
        {

            private string measuredMetricField;

            private string displayMetricField;

            private string displayImperialField;

            /// <remarks/>
            public string MeasuredMetric
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
            public string DisplayMetric
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
            public string DisplayImperial
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

            private object signedField;

            /// <remarks/>
            public object Signed
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
        public partial class NotesForHaulier
        {

            private object[] itemsField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Para", typeof(Para), Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
            [System.Xml.Serialization.XmlElementAttribute("Br", typeof(object))]
            public object[] Items
            {
                get
                {
                    return this.itemsField;
                }
                set
                {
                    this.itemsField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext", IsNullable = false)]
        public partial class Para
        {

            private object[] itemsField;

            private string[] textField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Bold", typeof(ParaBold))]
            [System.Xml.Serialization.XmlElementAttribute("Br", typeof(object))]
            public object[] Items
            {
                get
                {
                    return this.itemsField;
                }
                set
                {
                    this.itemsField = value;
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
        public partial class ParaBold
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