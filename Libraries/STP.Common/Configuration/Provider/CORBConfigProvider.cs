#region

using System;
using System.Configuration;
using System.Diagnostics;
using STP.Common.Configuration.Interface;

#endregion

namespace STP.Common.Configuration.Provider
{
    public sealed class CORBConfigProvider : ICORBConfigProvider
    {
        #region Singleton

        private static volatile CORBConfigProvider instance;
        // Lock synchronization object
        private static readonly object SyncLock = new object();

        private CORBConfigProvider()
        {
        }

        internal static CORBConfigProvider Instance
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
                            instance = new CORBConfigProvider();
                        }
                    }
                }
                return instance;
            }
        }

        #region Logger instance

        private const string PolicyName = "CORBConfigProvider";
        //        private static readonly LogWrapper log = new LogWrapper();

        #endregion

        #endregion

        #region Implementation of ICorrespondentOnBoardingProvider

        private const string MappingsCORBConfigurationSectionName = "CORBConfigSection";

        public CORBConfigCollection GetTheCollection()
        {
            var mappingsSection =
                (CORBConfigSection)
                    ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                        MappingsCORBConfigurationSectionName));
            if (mappingsSection != null)
            {
                return mappingsSection.CORBConfigItems;
            }
            return null; // OOPS!
        }

        /// <summary>
        ///     Get all the settings located at Apps/Web/STPApps\Web\STP\Configuration\CORBSettings.config
        /// </summary>
        /// <returns></returns>
        public int TotalSettings()
        {
            int totalCount = 0;
            try
            {
                var mappingsSection =
                    (CORBConfigSection)
                        ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                            MappingsCORBConfigurationSectionName));
                if (mappingsSection != null)
                {
                    CORBConfigCollection corbConfigCollection = mappingsSection.CORBConfigItems;
                    totalCount = corbConfigCollection.Count;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return totalCount;
        }


        /// <summary>
        ///     Get value of CORB Settings. pass setting name as parameter
        ///     settings are located at Apps\Web\STP\Configuration\CORBSettings.config
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            string value = string.Empty;
            try
            {
                var mappingsSection =
                    (CORBConfigSection)
                        ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                            MappingsCORBConfigurationSectionName));
                if (mappingsSection != null)
                {
                    CORBConfigCollection corbConfigCollection = mappingsSection.CORBConfigItems;
                    if (corbConfigCollection.Count > 0)
                    {
                        for (int x = 0; x < corbConfigCollection.Count; x++)
                        {
                            if (corbConfigCollection[x].Key.ToLower() == key.ToLower())
                            {
                                value = corbConfigCollection[x].Value;
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

    #region CORBConfigCollection

    /// <summary>
    ///     CORBConfigCollection for CORBConfigSectionMappings
    /// </summary>
    [ConfigurationCollection(typeof (CORBConfigSectionMappings))]
    public class CORBConfigCollection : ConfigurationElementCollection
    {
        public CORBConfigSectionMappings this[int idx]
        {
            get { return (CORBConfigSectionMappings) BaseGet(idx); }
        }

        public new CORBConfigSectionMappings this[string key]
        {
            get { return (CORBConfigSectionMappings) BaseGet(key); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new CORBConfigSectionMappings();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CORBConfigSectionMappings) (element)).Key;
        }
    }

    #endregion

    #region CORBConfigSection

    /// <summary>
    ///     CORBConfigSection
    /// </summary>
    internal class CORBConfigSection : ConfigurationSection
    {
        private const string CORBConfigSectionMappings = "CORBConfigSectionMappings";

        [ConfigurationProperty(CORBConfigSectionMappings)]
        internal CORBConfigCollection CORBConfigItems
        {
            get { return ((CORBConfigCollection) (base[CORBConfigSectionMappings])); }
        }
    }

    #endregion

    #region CORBConfigSectionMappings

    /// <summary>
    ///     CORBConfigSectionMappings
    /// </summary>
    public class CORBConfigSectionMappings : ConfigurationElement
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