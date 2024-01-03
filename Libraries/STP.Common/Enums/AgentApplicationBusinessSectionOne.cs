// Author: Narendra Kosta
// Created Date: 01/02/2011
// Description: created Enum for AgentBoarding Fields to be displayed in Forms.

using System.ComponentModel;

namespace STP.Common.Enums
{
    public enum AgentApplicationBusinessSectionOne
    {
        [Description("Application Type")]
        ApplicationType = 1,

        [Description("Agent Number")]
        AgentNumber = 2,

        [Description("Is Language Spoken English")]
        IsLanguageSpokenEnglish = 3,

        [Description("Is Language Spoken Spanish")]
        IsLanguageSpokenSpanish = 4,

        [Description("Is Language Spoken Other")]
        IsLanguageSpokenOther = 5,

        [Description("Language Spoken Other Language")]
        LanguageSpokenOtherLanguage = 6

    }
}
