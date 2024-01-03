#region

using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    public enum AgentBoardingDocumentCategory
    {
        [Description("Scanned Image")] ScannedImage = 0,

        [Description("Scanned Document")] ScannedDocument = 1,
    }
}