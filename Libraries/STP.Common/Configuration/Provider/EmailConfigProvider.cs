#region

using System;
using System.Configuration;
using System.Diagnostics;
using STP.Common.Configuration.Interface;

#endregion

namespace STP.Common.Configuration.Provider
{
    public sealed class EmailConfigProvider : IEmailConfigProvider
    {
        #region Singleton

        private static volatile EmailConfigProvider instance;
        // Lock synchronization object
        private static readonly object SyncLock = new object();

        private EmailConfigProvider()
        {
        }

        internal static EmailConfigProvider Instance
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
                            instance = new EmailConfigProvider();
                        }
                    }
                }
                return instance;
            }
        }

        #region Logger instance

        private const string PolicyName = "EmailConfigProvider";
        //        private static readonly LogWrapper log = new LogWrapper();

        #endregion

        #endregion

        #region Implementation of IEmailConfigProvider

        private const string MappingsEmailConfigSectionName = "EmailConfigSection";

        public EmailConfigCollection GetTheCollection()
        {
            var mappingsSection =
                (EmailConfigSection)
                    ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                        MappingsEmailConfigSectionName));
            if (mappingsSection != null)
            {
                return mappingsSection.EmailConfigItems;
            }
            return null; // OOPS!
        }

        public int TotalSettings()
        {
            int totalCount = 0;
            try
            {
                var mappingsSection =
                    (EmailConfigSection)
                        ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                            MappingsEmailConfigSectionName));
                if (mappingsSection != null)
                {
                    EmailConfigCollection EmailConfigCollection = mappingsSection.EmailConfigItems;
                    totalCount = EmailConfigCollection.Count;
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
                    (EmailConfigSection)
                        ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                            MappingsEmailConfigSectionName));
                if (mappingsSection != null)
                {
                    EmailConfigCollection EmailConfigCollection = mappingsSection.EmailConfigItems;
                    if (EmailConfigCollection.Count > 0)
                    {
                        for (int x = 0; x < EmailConfigCollection.Count; x++)
                        {
                            if (EmailConfigCollection[x].Key.ToLower() == key.ToLower())
                            {
                                value = EmailConfigCollection[x].Value;
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

    #region EmailConfigCollection

    /// <summary>
    ///     EmailConfigCollection for EmailConfigSectionMappings
    /// </summary>
    [ConfigurationCollection(typeof (EmailConfigSectionMappings))]
    public class EmailConfigCollection : ConfigurationElementCollection
    {
        public EmailConfigSectionMappings this[int idx]
        {
            get { return (EmailConfigSectionMappings) BaseGet(idx); }
        }

        public new EmailConfigSectionMappings this[string key]
        {
            get { return (EmailConfigSectionMappings) BaseGet(key); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new EmailConfigSectionMappings();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EmailConfigSectionMappings) (element)).Key;
        }
    }

    #endregion

    #region EmailConfigSection

    /// <summary>
    ///     EmailConfigSection
    /// </summary>
    internal class EmailConfigSection : ConfigurationSection
    {
        private const string EmailConfigSectionMappings = "EmailConfigSectionMappings";

        [ConfigurationProperty(EmailConfigSectionMappings)]
        internal EmailConfigCollection EmailConfigItems
        {
            get { return ((EmailConfigCollection) (base[EmailConfigSectionMappings])); }
        }
    }

    #endregion

    #region EmailConfigSectionMappings

    /// <summary>
    ///     EmailConfigSectionMappings
    /// </summary>
    public class EmailConfigSectionMappings : ConfigurationElement
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