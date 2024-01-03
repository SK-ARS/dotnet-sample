// Author: Narendra Kosta
// Created Date: 01/02/2011
// Description: created Enum for AgentBoarding Fields to be displayed in Forms.

using System.ComponentModel;

namespace STP.Common.Enums
{
    public enum AgentApplicationBusinessSectionFour
    {
        [Description("Compliance Title")]
        ComplianceTitle = 1,

        [Description("Compliance Name")]
        ComplianceName = 2,

        [Description("Compliance Address")]
        ComplianceAddress = 3,

        [Description("Compliance Phone")]
        CompliancePhone = 4,

        [Description("Is Money Transfer Relationship Terminated?")]
        IsMoneyTransferRelationshipTerminated = 5,

        [Description("Reason For Money Transfer Relationship Terminated")]
        MoneyTransferRelationshipTerminatedReason = 6,

        [Description("Is Cash Check More Than 1000 Available?")]
        IsCashCheckMoreThan1000 = 7,

        [Description("Primary Corridor")]
        Corridor1 = 8,

        [Description("Primary Competitor")]
        Competitor1 = 9,

        [Description("Primary External/Internal")]
        ExternalInternal1 = 10,

        [Description("Primary Forecast with CES")]
        Forecast1WithCES = 11,

        [Description("Primary Credit Limit Suggested")]
        CreditLimit1Suggested = 12,

        [Description("Primary Fee")]
        Fee1 = 13,

        [Description("Primary Agent Commission")]
        AgentCommission1 = 14,

        [Description("Primary Todays Fx W/Competition")]
        TodaysFxWCompetition1 = 15,

        [Description("Primary TRX Potential")]
        TRXPotential1 = 16
    }
}
