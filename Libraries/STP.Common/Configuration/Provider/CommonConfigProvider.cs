#region

using System;
using System.Configuration;
using System.Diagnostics;
using STP.Common.Configuration.Interface;

#endregion

namespace STP.Common.Configuration.Provider
{
    public sealed class CommonConfigProvider : ICommonConfigProvider
    {
        #region Singleton

        private static volatile CommonConfigProvider instance;
        // Lock synchronization object
        private static readonly object SyncLock = new object();

        private CommonConfigProvider()
        {
        }

        internal static CommonConfigProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                // Support multithreaded applications through 'Double checked locking' pattern which (once
                // the instance exists) avoids locking each time the method is invoked
                if (instance == null)
                {
                    lock (SyncLock)
                    {
                        if (instance == null)
                        {
                            instance = new CommonConfigProvider();
                        }
                    }
                }
                return instance;
            }
        }

        #region Logger instance

        private const string PolicyName = "CommonConfigProvider";
     

        #endregion

        #endregion

        #region Implementation of ICommonConfigProvider

        private const string MappingsCommonConfigurationSectionName = "CommonConfigSection";

        public CommonConfigCollection GetTheCollection()
        {
            var mappingsSection =
                (CommonConfigSection)
                    ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                        MappingsCommonConfigurationSectionName));
            if (mappingsSection != null)
            {
                return mappingsSection.CommonConfigItems;
            }
            return null; // OOPS!
        }

        public int TotalSettings()
        {
            int totalCount = 0;
            try
            {
                var mappingsSection =
                    (CommonConfigSection)
                        ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                            MappingsCommonConfigurationSectionName));
                if (mappingsSection != null)
                {
                    CommonConfigCollection commonConfigCollection = mappingsSection.CommonConfigItems;
                    totalCount = commonConfigCollection.Count;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return totalCount;
        }

        public string GetValue(string key)
        {
            string value = string.Empty;
            try
            {
                var mappingsSection =
                    (CommonConfigSection)
                        ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                            MappingsCommonConfigurationSectionName));
                if (mappingsSection != null)
                {
                    CommonConfigCollection commonConfigCollection = mappingsSection.CommonConfigItems;
                    if (commonConfigCollection.Count > 0)
                    {
                        for (int x = 0; x < commonConfigCollection.Count; x++)
                        {
                            if (commonConfigCollection[x].Key.ToLower() == key.ToLower())
                            {
                                value = commonConfigCollection[x].Value;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        #endregion
    }

    #region CommonConfigCollection

    /// <summary>
    ///     CommonConfigCollection for CommonConfigSectionMappings
    /// </summary>
    [ConfigurationCollection(typeof (CommonConfigSectionMappings))]
    public class CommonConfigCollection : ConfigurationElementCollection
    {
        public CommonConfigSectionMappings this[int idx]
        {
            get { return (CommonConfigSectionMappings) BaseGet(idx); }
        }

        public new CommonConfigSectionMappings this[string key]
        {
            get { return (CommonConfigSectionMappings) BaseGet(key); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new CommonConfigSectionMappings();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CommonConfigSectionMappings) (element)).Key;
        }
    }

    #endregion

    #region CommonConfigSection

    /// <summary>
    ///     CommonConfigSection
    /// </summary>
    internal class CommonConfigSection : ConfigurationSection
    {
        private const string CommonConfigSectionMappings = "CommonConfigSectionMappings";

        [ConfigurationProperty(CommonConfigSectionMappings)]
        internal CommonConfigCollection CommonConfigItems
        {
            get { return ((CommonConfigCollection) (base[CommonConfigSectionMappings])); }
        }
    }

    #endregion

    #region CommonConfigSectionMappings

    /// <summary>
    ///     CommonConfigSectionMappings
    /// </summary>
    public class CommonConfigSectionMappings : ConfigurationElement
    {
        private const string KEY = "key";
        private const string VALUE = "value";

        [ConfigurationProperty(KEY, DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Key
        {
            get { return ((string) (base[KEY])); }
            set { base[KEY] = value; }
        }

        [ConfigurationProperty(VALUE, DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Value
        {
            get { return ((string) (base[VALUE])); }
            set { base[VALUE] = value; }
        }
    }

    #endregion
}