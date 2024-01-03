#region

using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    public enum Title
    {
        [Description("Co-Signer")] CoSigner = 1,

        [Description("Director")] Director = 2,

        [Description("Manager")] Manager = 3,

        [Description("Member")] Member = 4,

        [Description("Partner")] Partner = 5,

        [Description("President")] President = 6,

        [Description("Secretary")] Secretary = 7,

        [Description("Sole Proprietor")] SoleProprietor = 8,

        [Description("Treasurer")] Treasurer = 9,

        [Description("Vice President")] VicePresident = 10,

        [Description("Other")] Other = 11,

    }
}



