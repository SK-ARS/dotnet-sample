#region

using System;
using System.Configuration;
using System.Diagnostics;
using STP.Common.Configuration.Interface;

#endregion

namespace STP.Common.Configuration.Provider
{
    public sealed class PageSecurityConfigProvider : IPageSecurityConfigProvider
    {
        #region Singleton

        private static volatile PageSecurityConfigProvider instance;
        // Lock synchronization object
        private static readonly object SyncLock = new object();

        private PageSecurityConfigProvider()
        {
        }

        internal static PageSecurityConfigProvider Instance
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
                            instance = new PageSecurityConfigProvider();
                        }
                    }
                }
                return instance;
            }
        }

        #region Logger instance

        private const string PolicyName = "PageSecurityConfigProvider";
        //        private static readonly LogWrapper log = new LogWrapper();

        #endregion

        #endregion

        #region Private Constant Variables

        private const string MappingsPageSecuritySectionName = "PageSecuritySection";

        #endregion

        #region Implementation of IPageSecurityConfigProvider

        public PageSecurityCollection GetTheCollection()
        {
            var mappingsSection =
                (PageSecurityConfigSection)
                    ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                        MappingsPageSecuritySectionName));
            if (mappingsSection != null)
            {
                return mappingsSection.PageSecurityItems;
            }
            return null; // OOPS!
        }

        public bool IsPageAllowed(string pageName, string securityCode)
        {
            bool isValid = false;
            try
            {
                var mappingsSection =
                    (PageSecurityConfigSection)
                        ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                            MappingsPageSecuritySectionName));
                if (mappingsSection != null)
                {
                    PageSecurityCollection pageSecurityCollection = mappingsSection.PageSecurityItems;
                    if (pageSecurityCollection.Count > 0)
                    {
                        for (int x = 0; x < pageSecurityCollection.Count; x++)
                        {
                            string a = pageSecurityCollection[x].PageName;
                            string b = pageSecurityCollection[x].SecurityCode;
                            if (pageName == a && b == securityCode)
                            {
                                isValid = true;
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
            return isValid;
        }

        public int GetSecurityCode(string pageName)
        {
            int securityCode = 0;
            try
            {
                var mappingsSection =
                    (PageSecurityConfigSection)
                        ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                            MappingsPageSecuritySectionName));
                if (mappingsSection != null)
                {
                    PageSecurityCollection pageSecurityCollection = mappingsSection.PageSecurityItems;
                    if (pageSecurityCollection.Count > 0)
                    {
                        for (int x = 0; x < pageSecurityCollection.Count; x++)
                        {
                            if (pageSecurityCollection[x].PageName.ToLower() == pageName.ToLower())
                            {
                                int.TryParse(pageSecurityCollection[x].SecurityCode, out securityCode);
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
            return securityCode;
        }

        public bool IsPageLevelSecurityEnabled(string pageName)
        {
            bool enableSecurity = false;
            try
            {
                var mappingsSection =
                    (PageSecurityConfigSection)
                        ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                            MappingsPageSecuritySectionName));
                if (mappingsSection != null)
                {
                    PageSecurityCollection pageSecurityCollection = mappingsSection.PageSecurityItems;
                    if (pageSecurityCollection.Count > 0)
                    {
                        for (int x = 0; x < pageSecurityCollection.Count; x++)
                        {
                            if (pageSecurityCollection[x].PageName.ToLower() == pageName.ToLower())
                            {
                                bool.TryParse(pageSecurityCollection[x].EnableSecurity, out enableSecurity);
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
            return enableSecurity;
        }

        public int TotalSettings()
        {
            int totalCount = 0;
            try
            {
                var mappingsSection =
                    (PageSecurityConfigSection)
                        ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                            MappingsPageSecuritySectionName));
                if (mappingsSection != null)
                {
                    PageSecurityCollection pageSecurityConfigCollection = mappingsSection.PageSecurityItems;
                    totalCount = pageSecurityConfigCollection.Count;
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
                    (PageSecurityConfigSection)
                        ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                            MappingsPageSecuritySectionName));
                if (mappingsSection != null)
                {
                    PageSecurityCollection pageSecurityConfigCollection = mappingsSection.PageSecurityItems;
                    if (pageSecurityConfigCollection.Count > 0)
                    {
                        for (int x = 0; x < pageSecurityConfigCollection.Count; x++)
                        {
                            if (pageSecurityConfigCollection[x].PageName.ToLower() == key.ToLower())
                            {
                                value = pageSecurityConfigCollection[x].SecurityCode;
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

    #region PageSecurityCollection

    /// <summary>
    ///     PageSecurityConfigCollection for PageSecurityConfigSectionMappings
    /// </summary>
    [ConfigurationCollection(typeof (PageSecuritySubSectionMappings))]
    public class PageSecurityCollection : ConfigurationElementCollection
    {
        public PageSecuritySubSectionMappings this[int idx]
        {
            get { return (PageSecuritySubSectionMappings) BaseGet(idx); }
        }

        public new PageSecuritySubSectionMappings this[string key]
        {
            get { return (PageSecuritySubSectionMappings) BaseGet(key); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new PageSecuritySubSectionMappings();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PageSecuritySubSectionMappings) (element)).PageName;
        }
    }

    #endregion

    #region PageSecurityConfigSection

    /// <summary>
    ///     PageSecurityConfigSection
    /// </summary>
    internal class PageSecurityConfigSection : ConfigurationSection
    {
        private const string PageSecurityMappings = "PageSecuritySubSectionMappings";

        [ConfigurationProperty(PageSecurityMappings)]
        public PageSecurityCollection PageSecurityItems
        {
            get { return ((PageSecurityCollection) (base[PageSecurityMappings])); }
        }
    }

    #endregion

    #region PageSecuritySectionMappings

    /// <summary>
    ///     PageSecuritySubSectionMappings
    /// </summary>
    public class PageSecuritySubSectionMappings : ConfigurationElement
    {
        private const string PAGE_NAME = "PageName";
        private const string ActivationCode = "SecurityCode";
        private const string IsSecurityEnabled = "EnableSecurity";


        [ConfigurationProperty(PAGE_NAME, DefaultValue = "", IsKey = false, IsRequired = true)]
        public string PageName
        {
            get { return ((string) (base[PAGE_NAME])); }
            set { base[PAGE_NAME] = value; }
        }

        [ConfigurationProperty(ActivationCode, DefaultValue = "", IsKey = true, IsRequired = true)]
        public string SecurityCode
        {
            get { return ((string) (base[ActivationCode])); }
            set { base[ActivationCode] = value; }
        }

        [ConfigurationProperty(IsSecurityEnabled, DefaultValue = "", IsKey = true, IsRequired = true)]
        public string EnableSecurity
        {
            get { return ((string) (base[IsSecurityEnabled])); }
            set { base[IsSecurityEnabled] = value; }
        }
    }

    #endregion
}