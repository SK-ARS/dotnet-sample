using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Diagnostics;
using System.CodeDom.Compiler;

namespace STP.Domain.RouteAssessment.XmlAffectedParties
{
    /// <remarks/>
    [GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
    [XmlRootAttribute("AffectedParties", Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
    public partial class AffectedPartiesStructure
    {

        /// <remarks/>
        [XmlArrayItemAttribute("AffectedParty", IsNullable = false)]
        public List<AffectedPartyStructure> GeneratedAffectedParties
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlArrayItemAttribute("AffectedParty", IsNullable = false)]
        public List<AffectedPartyStructure> ManualAffectedParties
        {
            get;
            set;
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
    public partial class AffectedPartyStructure
    {


        public AffectedPartyStructure()
        {
            DispensationStatus = DispensationStatusType.nonematching;
            this.Exclude = false;
            this.IsPolice = false;
            this.IsRetainedNotificationOnly = false;
        }

        /// <remarks/>
        public ContactReferenceStructure Contact
        {
            get;
            set;
        }

        /// <remarks/>
        public OnBehalfOfStructure OnBehalfOf
        {
            get;
            set;
        }

        /// <remarks/>
        public DelegatingToStructure DelegatingTo
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public DispensationStatusType DispensationStatus
        {
            get;
            set;
        }

        [XmlAttributeAttribute()]
        public bool Exclude
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public AffectedPartyReasonType Reason
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool ReasonSpecified
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public AffectedPartyReasonExclusionOutcomeType ExclusionOutcome
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool ExclusionOutcomeSpecified
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public bool IsPolice
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public bool IsRetainedNotificationOnly
        {
            get;
            set;
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
    public partial class ContactReferenceStructure
    {
        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string Description
        {
            get;
            set;
        }

        /// <remarks/>
        public ContactReferenceChoiceStructure Contact
        {
            get;
            set;
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
    public partial class OnBehalfOfStructure
    {

        public OnBehalfOfStructure()
        { }

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string DelegatorsOrganisationName
        {
            get;
            set;
        }

        /// <remarks/>
        public OnBehalfOfStructure OnBehalfOf
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int DelegatorsOrganisationId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool DelegatorsOrganisationIdSpecified
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int DelegatorsContactId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool DelegatorsContactIdSpecified
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool RetainNotification
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool WantsFailureAlert
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int DelegationId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool DelegationIdSpecified
        {
            get;
            set;
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
    public partial class DelegatingToStructure
    {
        /// <remarks/>
        public DelegatingToStructure DelegatingTo
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int DelegateesOrganisationId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool DelegateesOrganisationIdSpecified
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int DelegateesContactId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool DelegateesContactIdSpecified
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int DelegationId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool DelegationIdSpecified
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/dispensation")]
    public enum DispensationStatusType
    {

        /// <remarks/>
        [XmlEnumAttribute("in use")]
        inuse,

        /// <remarks/>
        [XmlEnumAttribute("none matching")]
        nonematching,

        /// <remarks/>
        [XmlEnumAttribute("some matching")]
        somematching,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
    public enum AffectedPartyReasonType
    {

        /// <remarks/>
        [XmlEnumAttribute("newly affected")]
        newlyaffected,

        /// <remarks/>
        [XmlEnumAttribute("no longer affected")]
        nolongeraffected,

        /// <remarks/>
        [XmlEnumAttribute("affected by change of route")]
        affectedbychangeofroute,

        /// <remarks/>
        [XmlEnumAttribute("still affected")]
        stillaffected,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
    public enum AffectedPartyReasonExclusionOutcomeType
    {

        /// <remarks/>
        [XmlEnumAttribute("newly affected")]
        newlyaffected,

        /// <remarks/>
        [XmlEnumAttribute("no longer affected")]
        nolongeraffected,

        /// <remarks/>
        [XmlEnumAttribute("affected by change of route")]
        affectedbychangeofroute,

        /// <remarks/>
        [XmlEnumAttribute("still affected")]
        stillaffected,

        /// <remarks/>
        [XmlEnumAttribute("not affected")]
        notaffected,
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
    public partial class ContactReferenceChoiceStructure
    {

        /// <remarks/>
        [XmlElementAttribute("AdhocReference", typeof(AdhocContactReferenceStructure))]
        public AdhocContactReferenceStructure adhocContactRef
        {
            get;
            set;
        }

        [XmlElementAttribute("RoleReference", typeof(RoleContactReferenceStructure))]
        public RoleContactReferenceStructure roleContactRef
        {
            get;
            set;
        }

        [XmlElementAttribute("SimpleReference", typeof(SimpleContactReferenceStructure))]
        public SimpleContactReferenceStructure simpleContactRef
        {
            get;
            set;
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
    public partial class AdhocContactReferenceStructure
    {

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string FullName
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string OrganisationName
        {
            get;
            set;
        }

        /// <remarks/>
        public AddressStructure Address
        {
            get;
            set;
        }

        /// <remarks/>
        public string TelephoneNumber
        {
            get;
            set;
        }

        /// <remarks/>
        public string TelephoneExtension
        {
            get;
            set;
        }

        /// <remarks/>
        public string MobileNumber
        {
            get;
            set;
        }

        /// <remarks/>
        public string FaxNumber
        {
            get;
            set;
        }

        /// <remarks/>
        public string EmailAddress
        {
            get;
            set;
        }

        /// <remarks/>
        public EmailFormatType EmailFormatPreference
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool EmailFormatPreferenceSpecified
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
    public enum EmailFormatType
    {

        /// <remarks/>
        html,

        /// <remarks/>
        text,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
    public partial class AddressStructure : UKPostalAddressStructure
    {
        /// <remarks/>
        public CountryType Country
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool CountrySpecified
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
    public enum CountryType
    {

        /// <remarks/>
        england,

        /// <remarks/>
        wales,

        /// <remarks/>
        scotland,

        /// <remarks/>
        [XmlEnumAttribute("northern ireland")]
        northernireland,
    }

    /// <remarks/>
    [XmlIncludeAttribute(typeof(AddressStructure))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.govtalk.gov.uk/people/AddressAndPersonalDetails")]
    public partial class UKPostalAddressStructure
    {

        /// <remarks/>
        [XmlElementAttribute("Line")]
        public List<string> Line
        {
            get;
            set;
        }

        /// <remarks/>
        public string PostCode
        {
            get;
            set;
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
    public partial class RoleContactReferenceStructure
    {

        /// <remarks/>
        public RoleType RoleWithinOganisation
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string OrganisationName
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string ResolvedFullName
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int OrganisationId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int ResolvedContactId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool ResolvedContactIdSpecified
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
    public enum RoleType
    {

        /// <remarks/>
        [XmlEnumAttribute("data holder")]
        dataholder,

        /// <remarks/>
        [XmlEnumAttribute("notification contact")]
        notificationcontact,

        /// <remarks/>
        [XmlEnumAttribute("official contact")]
        officialcontact,

        /// <remarks/>
        [XmlEnumAttribute("police alo")]
        policealo,

        /// <remarks/>
        haulier,

        /// <remarks/>
        [XmlEnumAttribute("it contact")]
        itcontact,

        /// <remarks/>
        [XmlEnumAttribute("default contact")]
        defaultcontact,

        /// <remarks/>
        [XmlEnumAttribute("data owner")]
        dataowner,
    }



    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
    public partial class SimpleContactReferenceStructure
    {

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string FullName
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string OrganisationName
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int ContactId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int OrganisationId
        {
            get;
            set;
        }
    }
}
