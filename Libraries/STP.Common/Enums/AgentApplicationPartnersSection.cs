// Author: Narendra Kosta
// Created Date: 01/02/2011
// Description: created Enum for AgentBoarding Fields to be displayed in Forms.

using System.ComponentModel;

namespace STP.Common.Enums
{
    public enum AgentApplicationPartnersSection
    {
        [Description("Title")]
        Title = 1,

        [Description("Name")]
        Name = 2,

        [Description("Percentage")]
        Percentage = 3,

        [Description("Prefix")]
        Prefix = 4
    }
}
