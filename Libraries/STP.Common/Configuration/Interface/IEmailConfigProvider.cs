#region

using STP.Common.Configuration.Provider;

#endregion

namespace STP.Common.Configuration.Interface
{
    public interface IEmailConfigProvider
    {
        EmailConfigCollection GetTheCollection();
        int TotalSettings();
        string GetValue(string key);
    }
}