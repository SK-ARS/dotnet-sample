#region

using STP.Common.Configuration.Provider;

#endregion

namespace STP.Common.Configuration.Interface
{
    public interface IPageSecurityConfigProvider
    {
        PageSecurityCollection GetTheCollection();
        bool IsPageAllowed(string pageName, string securityCode);
        int GetSecurityCode(string pageName);
        bool IsPageLevelSecurityEnabled(string pageName);
        int TotalSettings();
        string GetValue(string key);
    }
}