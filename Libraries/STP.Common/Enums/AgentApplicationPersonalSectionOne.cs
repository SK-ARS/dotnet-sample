// Author: Narendra Kosta
// Created Date: 01/02/2011
// Description: created Enum for AgentBoarding Fields to be displayed in Forms.

using System.ComponentModel;

namespace STP.Common.Enums
{
    public enum AgentApplicationPersonalSectionOne
    {

        [Description("Partner Prefix")]
        PartnerPrefix = 1,

        [Description("Partner First Name")]
        PartnerFirstName = 2,

        [Description("Partner Middle Name")]
        PartnerMiddleName = 3,

        [Description("Partner Last Name")]
        PartnerLastName = 4,

        [Description("Partner Gender")]
        PartnerGender = 5,

        [Description("Partner Marital Status")]
        PartnerMaritalStatus = 6,

        [Description("Partner Home Address")]
        PartnerHomeAddress = 7,

        [Description("Partner Country")]
        PartnerCountry = 8,

        [Description("Partner State")]
        PartnerState = 9,

        [Description("Partner City")]
        PartnerCity = 10,

        [Description("Partner Postal Code")]
        PartnerPostalCode = 11,

        [Description("Partner Nationality")]
        PartnerNationality = 12,

        [Description("Partner Original Country")]
        PartnerOriginalCountry = 13,

        [Description("Partner Date Of Birth")]
        PartnerDateOfBirth = 14,

        [Description("Partner Email")]
        PartnerEmail = 15,

        [Description("Partner IDType")]
        PartnerIDType = 16,

        [Description("Partner IDNumber")]
        PartnerIDNumber = 17,

        [Description("Partner IDExpiryDate")]
        PartnerIDExpiryDate = 18,

        [Description("Partner HomePhone")]
        PartnerHomePhone = 19,

        [Description("Partner CellPhone")]
        PartnerCellPhone = 20
        
    }
}
