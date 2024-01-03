// Author: Narendra Kosta
// Created Date: 01/02/2011
// Description: created Enum for AgentBoarding Fields to be displayed in Forms.

using System.ComponentModel;

namespace STP.Common.Enums
{
    public enum AgentApplicationBusinessSectionThree
    {
        [Description("Type Of Business")]
        TypeOfBusiness = 1,

        [Description("Major Product Or Service Offered")]
        MajorProductOrServiceOffered = 2,

        [Description("Month And Year Business Was Purchased")]
        MonthAndYearBusinessWasPurchased = 3,

        [Description("Do You Own This Property")]
        DoYouOwnThisProperty = 4,

        [Description("Years At This Location")]
        YearsAtThisLocation = 5,

        [Description("Terms Of Lease")]
        TermsOfLease = 6,

        [Description("Landlords Name")]
        LandlordsName = 7,

        [Description("Landlords Phone")]
        LandlordsPhone = 8,

        [Description("Landlords Address")]
        LandlordsAddress = 9,

        [Description("Property Down Payment")]
        PropertyDownPayment = 10   ,

        [Description("Property Amount Financed")]
        PropertyAmountFinanced = 11

    }
}
