#region

using System;
using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    public enum Source
    {
        [Description("Register New Agent")] RNA = 1,

        [Description("Register with PDF")] RegisterWithPDF = 2,

        [Description("Register New Agent using Long Form")] RNAUsingLongForm = 3,
    }
}
