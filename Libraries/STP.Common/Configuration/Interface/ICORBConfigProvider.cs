#region

using STP.Common.Configuration.Provider;

#endregion

namespace STP.Common.Configuration.Interface
{
    /// <summary>
    ///     Interface contains methods to get settings for CORB
    /// </summary>
    public interface ICORBConfigProvider
    {
        CORBConfigCollection GetTheCollection();

        /// <summary>
        ///     Get all the settings located at Apps/Web/STPApps\Web\STP\Configuration\CORBSettings.config
        /// </summary>
        /// <returns></returns>
        int TotalSettings();

        /// <summary>
        ///     Get value of CORB Settings. pass setting name as parameter
        ///     settings are located at Apps\Web\STP\Configuration\CORBSettings.config
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetValue(string key);
    }
}