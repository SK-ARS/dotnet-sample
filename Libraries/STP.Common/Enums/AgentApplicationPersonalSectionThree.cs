// Author: Narendra Kosta
// Created Date: 01/02/2011
// Description: created Enum for AgentBoarding Fields to be displayed in Forms.

using System.ComponentModel;

namespace STP.Common.Enums
{
    public enum AgentApplicationPersonalSectionThree
    {
        [Description("How long have you lived at current location?")]
        HowLongHaveYouLivedInCurrentLocation = 1,

        [Description("Do you own or rent?")]
        DoYouOwnOrRent = 2,

        [Description("Have you ever been arrested or so?")]
        HaveYouEverArrestedOrSo = 3,

        [Description("Arrested or so detailed explanation")]
        ArrestedOrSoDetailedExplanation = 4,

        [Description("Do you agree to above?")]
        DoYouAgreeToAbove = 5,

        [Description("If you agree type your initials")]
        IfAgreedTypeYourInitial = 6,

        [Description("Does your spouse agree?")]
        IsSpouseAgree = 7,

        [Description("If spouse agreed type your initials")]
        IfSpouseAgreedTypeYourInitial = 8,

        [Description("Primary Property title name")]
        FirstPropertyTitleName = 9,

        [Description("Primary Property address")]
        FirstPropertyAddress = 10,

        [Description("Primary Property purchase price")]
        FirstPropertyPurchasePrice = 11,

        [Description("Primary Property current value")]
        FirstPropertyCurrentValue = 12,

        [Description("Primary Property monthly payment")]
        FirstPropertyMonthlyPayment = 13,

        [Description("Primary Property amount financed")]
        FirstPropertyAmountFinanced = 14,

        [Description("Primary Property down payment")]
        FirstPropertyDownPayment = 15
    }
}
