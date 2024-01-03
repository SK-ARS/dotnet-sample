// Author: Narendra Kosta
// Created Date: 01/02/2011
// Description: created Enum for AgentBoarding Fields to be displayed in Forms.

using System.ComponentModel;

namespace STP.Common.Enums
{
    public enum AgentApplicationPersonalSectionTwo
    {
        [Description("Spouse Title")]
        SpouseTitle = 1,

        [Description("Spouse Name")]
        SpouseName = 2,

        [Description("Personal First Reference Name")]
        PersonalFirstReferenceName = 3,

        [Description("Personal First Reference Address")]
        PersonalFirstReferenceAddress = 4,

        [Description("Personal First Reference Phone")]
        PersonalFirstReferencePhone = 5,

        [Description("Personal First Reference Relationship")]
        PrimaryPersonalFirstReferenceRelationship = 6,

        [Description("Primary Bank Name")]
        PrimaryBankName = 7,

        [Description("Primary Bank Account Number")]
        PrimaryBankAccountNumber = 8,

        [Description("Primary Bank Cash In Account")]
        PrimaryBankCashInAccount = 9,

        [Description("Primary Bank Sort Code")]
        PrimaryBankSortCode = 10,

        [Description("Personal First Reference City")]
        PersonalFirstReferenceCity = 11,

        [Description("Personal First Reference State")]
        PersonalFirstReferenceState = 12,

        [Description("Personal First Reference Zip Code")]
        PersonalFirstReferenceZipCode = 13,

        [Description("Spouse Middle Name")]
        SpouseMiddleName = 14,

        [Description("Spouse Last Name")]
        SpouseLastName = 15,

        [Description("Spouse Ownership(%)")]
        SpouseOwnership = 16,

        [Description("Spouse Gender")]
        SpouseGender = 17,

        [Description("Spouse Home Address")]
        SpouseHomeAddress = 18,

        [Description("Spouse Country")]
        SpouseCountry = 19,

        [Description("Spouse State")]
        SpouseState = 20,

        [Description("Spouse City")]
        SpouseCity = 21,

        [Description("Spouse Postal Code")]
        SpousePostalCode = 22,

        [Description("Spouse Cell Phone")]
        SpouseCellPhone = 23,

        [Description("Spouse Home Phone")]
        SpouseHomePhone = 24,

        [Description("Spouse Email")]
        SpouseEmail = 25,

        [Description("Does your spouse own this property?")]
        DoesYourSpouseOwnThisProperty = 26,

        [Description("Spouse at this property in years")]
        SpouseAtThisPropertyInYears = 27,

        [Description("Spouse at this property in months")]
        SpouseAtThisPropertyInMonths = 28,

        [Description("Spouse Date Of Birth")]
        SpouseDateOfBirth = 29,

        [Description("Spouse ID Type")]
        SpouseIDType = 30,

        [Description("Spouse ID Number")]
        SpouseIDNumber = 31,

        [Description("Spouse ID Expiry Date")]
        SpouseIDExpiryDate = 32,

        [Description("Spouse SSN/ITIN Number")]
        SpouseSSNNumber  = 33
    }
}
