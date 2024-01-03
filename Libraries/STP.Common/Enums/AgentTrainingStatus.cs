#region

using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    public enum AgentTrainingStatus
    {
        [Description("Pending")] Pending = 1,

        [Description("In Process")] InProcess = 2,

        [Description("Completed")] Completed = 3,
    }
}
