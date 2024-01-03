#region

using System;
using System.Configuration;
using STP.Common.Configuration.Provider;

#endregion

namespace STP.Common.Configuration
{
    [CLSCompliant(false)]
    public class STPConfigurationSection : ConfigurationSection
    {
        [CLSCompliant(false)] 
        [ConfigurationProperty("AppId")]
        public ConfigNodeValue<int> AppId
        {
            get { return (ConfigNodeValue<int>) this["AppId"]; }
        }

        [CLSCompliant(false)]
        [ConfigurationProperty("SiteName")]
        public ConfigNodeValue<string> SiteName
        {
            get { return (ConfigNodeValue<string>) this["SiteName"]; }
        }


        [CLSCompliant(false)]
        [ConfigurationProperty("DefaultCulture")]
        public ConfigNodeValue<string> DefaultCulture
        {
            get { return (ConfigNodeValue<string>) this["DefaultCulture"]; }
        }

        [CLSCompliant(false)]
        [ConfigurationProperty("DefaultRecordsPerPage")]
        public ConfigNodeValue<int> DefaultRecordsPerPage
        {
            get { return (ConfigNodeValue<int>) this["DefaultRecordsPerPage"]; }
        }

        [CLSCompliant(false)]
        [ConfigurationProperty("EnableSSL")]
        public ConfigNodeValue<bool> EnableSSL
        {
            get { return (ConfigNodeValue<bool>) this["EnableSSL"]; }
        }

        [CLSCompliant(false)]
        [ConfigurationProperty("LogDirectoryRepository")]
        public ConfigNodeValue<string> LogDirectoryRepository
        {
            get { return (ConfigNodeValue<string>) this["LogDirectoryRepository"]; }
        }


        [CLSCompliant(false)]
        [ConfigurationProperty("MaxLogFileSize")]
        public ConfigNodeValue<string> MaxLogFileSize
        {
            get { return (ConfigNodeValue<string>) this["MaxLogFileSize"]; }
        }

        [CLSCompliant(false)]
        [ConfigurationProperty("EnableLog")]
        public ConfigNodeValue<string> EnableLog
        {
            get { return (ConfigNodeValue<string>) this["EnableLog"]; }
        }

        [CLSCompliant(false)]
        [ConfigurationProperty("IsDebugEnabled")]
        public ConfigNodeValue<string> IsDebugEnabled
        {
            get { return (ConfigNodeValue<string>) this["IsDebugEnabled"]; }
        }
    }
}