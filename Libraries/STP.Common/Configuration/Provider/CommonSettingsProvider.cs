#region

using System;
using System.Configuration;
using System.Diagnostics;
using STP.Common.Configuration.Interface;

#endregion

namespace STP.Common.Configuration.Provider
{
    public sealed class CommonSettingsProvider : ICommonSettingsProvider
    {
        #region Singleton

        private static volatile CommonSettingsProvider instance;
        // Lock synchronization object
        private static readonly object SyncLock = new object();

        private CommonSettingsProvider()
        {
        }

        internal static CommonSettingsProvider Instance
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
                            instance = new CommonSettingsProvider();
                        }
                    }
                }
                return instance;
            }
        }

        #region Logger instance

        private const string PolicyName = "CommonSettingsProvider";
        //        private static readonly LogWrapper log = new LogWrapper();

        #endregion

        #endregion

        #region Implementation of ICommonConfigProvider

        private const string MappingsCommonSettingsSectionName = "CommonSettingsSection";

        public CommonSettingsCollection GetTheCollection()
        {
            var mappingsSection =
                (CommonSettingsSection)
                    ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                        MappingsCommonSettingsSectionName));
            if (mappingsSection != null)
            {
                return mappingsSection.CommonSettingsItems;
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
                            MappingsCommonSettingsSectionName));
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
                    (CommonSettingsSection)
                        ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                            MappingsCommonSettingsSectionName));
                if (mappingsSection != null)
                {
                    CommonSettingsCollection commonSettingsCollection = mappingsSection.CommonSettingsItems;
                    if (commonSettingsCollection.Count > 0)
                    {
                        for (int x = 0; x < commonSettingsCollection.Count; x++)
                        {
                            if (commonSettingsCollection[x].Key.ToLower() == key.ToLower())
                            {
                                value = commonSettingsCollection[x].Value;
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
    [ConfigurationCollection(typeof (CommonSettingsSectionMappings))]
    public class CommonSettingsCollection : ConfigurationElementCollection
    {
        public CommonSettingsSectionMappings this[int idx]
        {
            get { return (CommonSettingsSectionMappings) BaseGet(idx); }
        }

        public new CommonSettingsSectionMappings this[string key]
        {
            get { return (CommonSettingsSectionMappings) BaseGet(key); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new CommonSettingsSectionMappings();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CommonSettingsSectionMappings) (element)).Key;
        }
    }

    #endregion

    #region CommonConfigSection

    /// <summary>
    ///     CommonConfigSection
    /// </summary>
    internal class CommonSettingsSection : ConfigurationSection
    {
        private const string CommonSettingsSectionMappings = "CommonSettingsSectionMappings";

        [ConfigurationProperty(CommonSettingsSectionMappings)]
        internal CommonSettingsCollection CommonSettingsItems
        {
            get { return ((CommonSettingsCollection) (base[CommonSettingsSectionMappings])); }
        }
    }

    #endregion

    #region CommonSettingsSectionMappings

    /// <summary>
    ///     CommonConfigSectionMappings
    /// </summary>
    public class CommonSettingsSectionMappings : ConfigurationElement
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