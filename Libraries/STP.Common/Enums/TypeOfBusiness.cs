#region

using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    public enum TypeOfBusiness
    {
        [Description("Individual/Sole Proprietorship")] Individual = 240911, //1,

        [Description("Partnership")] Partnership = 241111, //2,

        [Description("LLC")] LLC = 240811, // 3,

        [Description("LLP")] LLP = 241211,

        [Description("Corporation")] Corporation = 241011 // 4        
    }
}