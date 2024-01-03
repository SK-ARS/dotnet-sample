using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.MovementsAndNotifications.Movements {

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/notification")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/notification", IsNullable = false)]
    public partial class OutboundNotification
    {

        private ESDALReferenceNumber eSDALReferenceNumberField;

        private string classificationField;

        private object dftReferenceField;

        private OutboundNotificationHaulierDetails haulierDetailsField;

        private string hauliersReferenceField;

        private string clientNameField;

        private OutboundNotificationJourneyFromToSummary journeyFromToSummaryField;

        private OutboundNotificationJourneyFromTo journeyFromToField;

        private OutboundNotificationJourneyTiming journeyTimingField;

        private OutboundNotificationLoadDetails loadDetailsField;

        private string notificationNotesFromHaulierField;

        private string notificationOnEscortField;

        private string oldNotificationIDField;

        private string jobFileReferenceField;

        private object organisationNameField;

        private string sentDateTimeField;

        private OutboundNotificationVR1Information vR1InformationField;

        private Contact[] recipientsField;

        private OutboundNotificationRouteParts routePartsField;

        private OutboundNotificationIndemnityConfirmation indemnityConfirmationField;

        private object structureDetailsField;

        private object routeDescriptionField;

        private string typeField;

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
        public string Classification
        {
            get
            {
                return this.classificationField;
            }
            set
            {
                this.classificationField = value;
            }
        }

        /// <remarks/>
        public object DftReference
        {
            get
            {
                return this.dftReferenceField;
            }
            set
            {
                this.dftReferenceField = value;
            }
        }

        /// <remarks/>
        public OutboundNotificationHaulierDetails HaulierDetails
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
        public string ClientName
        {
            get
            {
                return this.clientNameField;
            }
            set
            {
                this.clientNameField = value;
            }
        }

        /// <remarks/>
        public OutboundNotificationJourneyFromToSummary JourneyFromToSummary
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
        public OutboundNotificationJourneyFromTo JourneyFromTo
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
        public OutboundNotificationJourneyTiming JourneyTiming
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
        public OutboundNotificationLoadDetails LoadDetails
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
        public string NotificationNotesFromHaulier
        {
            get
            {
                return this.notificationNotesFromHaulierField;
            }
            set
            {
                this.notificationNotesFromHaulierField = value;
            }
        }

        /// <remarks/>
        public string NotificationOnEscort
        {
            get
            {
                return this.notificationOnEscortField;
            }
            set
            {
                this.notificationOnEscortField = value;
            }
        }

        /// <remarks/>
        public string OldNotificationID
        {
            get
            {
                return this.oldNotificationIDField;
            }
            set
            {
                this.oldNotificationIDField = value;
            }
        }

        /// <remarks/>
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
        public OutboundNotificationVR1Information VR1Information
        {
            get
            {
                return this.vR1InformationField;
            }
            set
            {
                this.vR1InformationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Contact", Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
        public Contact[] Recipients
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
        public OutboundNotificationRouteParts RouteParts
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
        public OutboundNotificationIndemnityConfirmation IndemnityConfirmation
        {
            get
            {
                return this.indemnityConfirmationField;
            }
            set
            {
                this.indemnityConfirmationField = value;
            }
        }

        /// <remarks/>
        public object StructureDetails
        {
            get
            {
                return this.structureDetailsField;
            }
            set
            {
                this.structureDetailsField = value;
            }
        }

        /// <remarks/>
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
    public partial class ESDALReferenceNumber
    {

        private string mnemonicField;

        private string eSDALReferenceNoField;

        private ushort movementProjectNumberField;

        private string movementVersionField;

        private byte notificationNumberField;

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
        public string ESDALReferenceNo
        {
            get
            {
                return this.eSDALReferenceNoField;
            }
            set
            {
                this.eSDALReferenceNoField = value;
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

        /// <remarks/>
        public byte NotificationNumber
        {
            get
            {
                return this.notificationNumberField;
            }
            set
            {
                this.notificationNumberField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
    public partial class Contact
    {

        private string contactNameField;

        private string organisationNameField;

        private string faxField;

        private string emailField;

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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/notification")]
    public partial class OutboundNotificationHaulierDetails
    {

        private string haulierContactField;

        private string haulierNameField;

        private HaulierAddress haulierAddressField;

        private string telephoneNumberField;

        private object faxNumberField;

        private string emailAddressField;

        private string licenceField;

        private ushort organisationIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
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
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
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
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public HaulierAddress HaulierAddress
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
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
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
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
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
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
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
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
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
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
    public partial class HaulierAddress
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/notification")]
    public partial class OutboundNotificationJourneyFromToSummary
    {

        private string fromField;

        private string toField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
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
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/notification")]
    public partial class OutboundNotificationJourneyFromTo
    {

        private string fromField;

        private string toField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
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
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/notification")]
    public partial class OutboundNotificationJourneyTiming
    {

        private System.DateTime firstMoveDateField;

        private System.DateTime lastMoveDateField;

        private string startTimeField;

        private string endTimeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", DataType = "date")]
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
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", DataType = "date")]
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

        /// <remarks/>
        public string StartTime
        {
            get
            {
                return this.startTimeField;
            }
            set
            {
                this.startTimeField = value;
            }
        }

        /// <remarks/>
        public string EndTime
        {
            get
            {
                return this.endTimeField;
            }
            set
            {
                this.endTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/notification")]
    public partial class OutboundNotificationLoadDetails
    {

        private string descriptionField;

        private byte totalMovesField;

        private byte maxPiecesPerMoveField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
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
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
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
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/notification")]
    public partial class OutboundNotificationVR1Information
    {

        private string statusField;

        private OutboundNotificationVR1InformationNumbers numbersField;

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
        public OutboundNotificationVR1InformationNumbers Numbers
        {
            get
            {
                return this.numbersField;
            }
            set
            {
                this.numbersField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/notification")]
    public partial class OutboundNotificationVR1InformationNumbers
    {

        private object scottishField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public object Scottish
        {
            get
            {
                return this.scottishField;
            }
            set
            {
                this.scottishField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/notification")]
    public partial class OutboundNotificationRouteParts
    {

        private RoutePartListPosition routePartListPositionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public RoutePartListPosition RoutePartListPosition
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
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
    public partial class RoutePartListPosition
    {

        private string routeField;

        private string routeImperialField;

        private RoutePartListPositionRoutePart routePartField;

        /// <remarks/>
        public string Route
        {
            get
            {
                return this.routeField;
            }
            set
            {
                this.routeField = value;
            }
        }

        /// <remarks/>
        public string RouteImperial
        {
            get
            {
                return this.routeImperialField;
            }
            set
            {
                this.routeImperialField = value;
            }
        }

        /// <remarks/>
        public RoutePartListPositionRoutePart RoutePart
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
    public partial class RoutePartListPositionRoutePart
    {

        private byte legNumberField;

        private string nameField;

        private RoutePartListPositionRoutePartRoadPart roadPartField;

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
        public RoutePartListPositionRoutePartRoadPart RoadPart
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
    public partial class RoutePartListPositionRoutePartRoadPart
    {

        private RoutePartListPositionRoutePartRoadPartStartPointListPosition startPointListPositionField;

        private RoutePartListPositionRoutePartRoadPartEndPointListPosition endPointListPositionField;

        private RoutePartListPositionRoutePartRoadPartDistance distanceField;

        private RoutePartListPositionRoutePartRoadPartVehicles vehiclesField;

        private RoutePartListPositionRoutePartRoadPartRoads roadsField;

        private RoutePartListPositionRoutePartRoadPartStructure[] structuresField;

        private DrivingInstructions drivingInstructionsField;

        /// <remarks/>
        public RoutePartListPositionRoutePartRoadPartStartPointListPosition StartPointListPosition
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
        public RoutePartListPositionRoutePartRoadPartEndPointListPosition EndPointListPosition
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
        public RoutePartListPositionRoutePartRoadPartDistance Distance
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
        public RoutePartListPositionRoutePartRoadPartVehicles Vehicles
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
        public RoutePartListPositionRoutePartRoadPartRoads Roads
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
        [System.Xml.Serialization.XmlArrayItemAttribute("Structure", IsNullable = false)]
        public RoutePartListPositionRoutePartRoadPartStructure[] Structures
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
    public partial class RoutePartListPositionRoutePartRoadPartStartPointListPosition
    {

        private RoutePartListPositionRoutePartRoadPartStartPointListPositionStartPoint startPointField;

        /// <remarks/>
        public RoutePartListPositionRoutePartRoadPartStartPointListPositionStartPoint StartPoint
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
    public partial class RoutePartListPositionRoutePartRoadPartStartPointListPositionStartPoint
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
    public partial class RoutePartListPositionRoutePartRoadPartEndPointListPosition
    {

        private RoutePartListPositionRoutePartRoadPartEndPointListPositionEndPoint endPointField;

        /// <remarks/>
        public RoutePartListPositionRoutePartRoadPartEndPointListPositionEndPoint EndPoint
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
    public partial class RoutePartListPositionRoutePartRoadPartEndPointListPositionEndPoint
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
    public partial class RoutePartListPositionRoutePartRoadPartDistance
    {

        private RoutePartListPositionRoutePartRoadPartDistanceMetric metricField;

        private RoutePartListPositionRoutePartRoadPartDistanceImperial imperialField;

        /// <remarks/>
        public RoutePartListPositionRoutePartRoadPartDistanceMetric Metric
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
        public RoutePartListPositionRoutePartRoadPartDistanceImperial Imperial
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
    public partial class RoutePartListPositionRoutePartRoadPartDistanceMetric
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
    public partial class RoutePartListPositionRoutePartRoadPartDistanceImperial
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
    public partial class RoutePartListPositionRoutePartRoadPartVehicles
    {

        private ConfigurationSummaryListPosition configurationSummaryListPositionField;

        private OverallLengthListPosition overallLengthListPositionField;

        private RigidLengthListPosition rigidLengthListPositionField;

        private object rearOverhangListPositionField;

        private object frontOverhangListPositionField;

        private object leftOverhangListPositionField;

        private object rightOverhangListPositionField;

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
        public object RearOverhangListPosition
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
        public object FrontOverhangListPosition
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
        public object LeftOverhangListPosition
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
        public object RightOverhangListPosition
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

        private decimal includingProjectionsField;

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

        private VehicleSummaryListPositionVehicleSummaryConfigurationIdentityListPosition configurationIdentityListPositionField;

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
        public VehicleSummaryListPositionVehicleSummaryConfigurationIdentityListPosition ConfigurationIdentityListPosition
        {
            get
            {
                return this.configurationIdentityListPositionField;
            }
            set
            {
                this.configurationIdentityListPositionField = value;
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
    public partial class VehicleSummaryListPositionVehicleSummaryConfigurationIdentityListPosition
    {

        private VehicleSummaryListPositionVehicleSummaryConfigurationIdentityListPositionConfigurationIdentity configurationIdentityField;

        /// <remarks/>
        public VehicleSummaryListPositionVehicleSummaryConfigurationIdentityListPositionConfigurationIdentity ConfigurationIdentity
        {
            get
            {
                return this.configurationIdentityField;
            }
            set
            {
                this.configurationIdentityField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleSummaryListPositionVehicleSummaryConfigurationIdentityListPositionConfigurationIdentity
    {

        private string plateNoField;

        private object fleetNoField;

        /// <remarks/>
        public string PlateNo
        {
            get
            {
                return this.plateNoField;
            }
            set
            {
                this.plateNoField = value;
            }
        }

        /// <remarks/>
        public object FleetNo
        {
            get
            {
                return this.fleetNoField;
            }
            set
            {
                this.fleetNoField = value;
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

        private decimal rigidLengthField;

        private decimal widthField;

        private VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleHeight heightField;

        private decimal wheelbaseField;

        private decimal rearOverhangField;

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
        public decimal RearOverhang
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

        private byte numberOfWheelField;

        private VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationAxleWeightListPosition[] axleWeightListPositionField;

        private VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationWheelsPerAxleListPosition[] wheelsPerAxleListPositionField;

        private VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationAxleSpacingListPosition[] axleSpacingListPositionField;

        private VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationAxleSpacingToFollowing axleSpacingToFollowingField;

        private VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationTyreSizeListPosition tyreSizeListPositionField;

        private VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationWheelSpacingListPosition wheelSpacingListPositionField;

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
        public byte NumberOfWheel
        {
            get
            {
                return this.numberOfWheelField;
            }
            set
            {
                this.numberOfWheelField = value;
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
        public VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationAxleSpacingToFollowing AxleSpacingToFollowing
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

        /// <remarks/>
        public VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationTyreSizeListPosition TyreSizeListPosition
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
        public VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationWheelSpacingListPosition WheelSpacingListPosition
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
    public partial class VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleAxleConfigurationAxleSpacingToFollowing
    {

        private decimal axleSpacingToFollowField;

        /// <remarks/>
        public decimal AxleSpacingToFollow
        {
            get
            {
                return this.axleSpacingToFollowField;
            }
            set
            {
                this.axleSpacingToFollowField = value;
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleSummaryListPositionVehicleSummaryConfigurationSemiVehicleHeight
    {

        private decimal maxHeightField;

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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
    public partial class RoutePartListPositionRoutePartRoadPartRoads
    {

        private RoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPosition routeSubPartListPositionField;

        /// <remarks/>
        public RoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPosition RouteSubPartListPosition
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
    public partial class RoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPosition
    {

        private RoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPart routeSubPartField;

        /// <remarks/>
        public RoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPart RouteSubPart
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
    public partial class RoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPart
    {

        private RoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPosition pathListPositionField;

        /// <remarks/>
        public RoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPosition PathListPosition
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
    public partial class RoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPosition
    {

        private RoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPosition[] pathField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("RoadTraversalListPosition", IsNullable = false)]
        public RoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPosition[] Path
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
    public partial class RoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPosition
    {

        private RoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversal roadTraversalField;

        /// <remarks/>
        public RoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversal RoadTraversal
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
    public partial class RoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversal
    {

        private RoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversalRoadIdentity roadIdentityField;

        private RoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversalDistance distanceField;

        /// <remarks/>
        public RoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversalRoadIdentity RoadIdentity
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
        public RoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversalDistance Distance
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
    public partial class RoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversalRoadIdentity
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
    public partial class RoutePartListPositionRoutePartRoadPartRoadsRouteSubPartListPositionRouteSubPartPathListPositionRoadTraversalListPositionRoadTraversalDistance
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
    public partial class RoutePartListPositionRoutePartRoadPartStructure
    {

        private string eSRNField;

        private string nameField;

        private RoutePartListPositionRoutePartRoadPartStructureAppraisal appraisalField;

        private bool isMyResponsibilityField;

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
        public RoutePartListPositionRoutePartRoadPartStructureAppraisal Appraisal
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
        public bool IsMyResponsibility
        {
            get
            {
                return this.isMyResponsibilityField;
            }
            set
            {
                this.isMyResponsibilityField = value;
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
    public partial class RoutePartListPositionRoutePartRoadPartStructureAppraisal
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

        private byte legNumberField;

        private string nameField;

        private DrivingInstructionsSubPartListPosition[] subPartListPositionField;

        private byte idField;

        private byte comparisonIdField;

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

        private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternative alternativeField;

        /// <remarks/>
        public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternative Alternative
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
    public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternative
    {

        private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPosition instructionListPositionField;

        /// <remarks/>
        public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPosition InstructionListPosition
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
    public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPosition
    {

        private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPositionInstruction instructionField;

        /// <remarks/>
        public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPositionInstruction Instruction
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
    public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPositionInstruction
    {

        private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPositionInstructionNavigation navigationField;

        private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPosition noteListPositionField;

        /// <remarks/>
        public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPositionInstructionNavigation Navigation
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
        public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPosition NoteListPosition
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
    public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPositionInstructionNavigation
    {

        private string instructionField;

        private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPositionInstructionNavigationDistance distanceField;

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
        public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPositionInstructionNavigationDistance Distance
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
    public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPositionInstructionNavigationDistance
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
    public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPosition
    {

        private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNote noteField;

        /// <remarks/>
        public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNote Note
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
    public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNote
    {

        private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNoteContent contentField;

        /// <remarks/>
        public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNoteContent Content
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
    public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNoteContent
    {

        private string motorwayCautionField;

        private DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNoteContentRoutePoint routePointField;

        /// <remarks/>
        public string MotorwayCaution
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
        public DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNoteContentRoutePoint RoutePoint
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
    public partial class DrivingInstructionsSubPartListPositionSubPartAlternativeListPositionAlternativeInstructionListPositionInstructionNoteListPositionNoteContentRoutePoint
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/notification")]
    public partial class OutboundNotificationIndemnityConfirmation
    {

        private string haulierField;

        private Timing timingField;

        private string signatoryField;

        private System.DateTime signedDateField;

        private bool confirmedField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public string Haulier
        {
            get
            {
                return this.haulierField;
            }
            set
            {
                this.haulierField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public Timing Timing
        {
            get
            {
                return this.timingField;
            }
            set
            {
                this.timingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public string Signatory
        {
            get
            {
                return this.signatoryField;
            }
            set
            {
                this.signatoryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", DataType = "date")]
        public System.DateTime SignedDate
        {
            get
            {
                return this.signedDateField;
            }
            set
            {
                this.signedDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Confirmed
        {
            get
            {
                return this.confirmedField;
            }
            set
            {
                this.confirmedField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
    public partial class Timing
    {

        private TimingMovementDateRange movementDateRangeField;

        /// <remarks/>
        public TimingMovementDateRange MovementDateRange
        {
            get
            {
                return this.movementDateRangeField;
            }
            set
            {
                this.movementDateRangeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
    public partial class TimingMovementDateRange
    {

        private System.DateTime fromDateField;

        private System.DateTime toDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime FromDate
        {
            get
            {
                return this.fromDateField;
            }
            set
            {
                this.fromDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime ToDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
            }
        }
    }


}
