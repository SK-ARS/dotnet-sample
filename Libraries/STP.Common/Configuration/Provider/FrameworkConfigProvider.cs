#region

using System;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using STP.Common.Configuration.Interface;
using STP.Common.Extensions;
using STP.Common.Log;

#endregion

namespace STP.Common.Configuration.Provider
{
    internal sealed class FrameworkConfigProvider : IFrameworkConfigProvider
    {
        #region Singleton

        private static volatile FrameworkConfigProvider instance;
        // Lock synchronization object
        private static readonly object syncLock = new object();

        private FrameworkConfigProvider()
        {
        }

        internal static FrameworkConfigProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                // Support multithreaded applications through 'Double checked locking' pattern which (once
                // the instance exists) avoids locking each time the method is invoked
                if (instance == null)
                {
                    lock (syncLock)
                    {
                        if (instance == null)
                        {
                            instance = new FrameworkConfigProvider();
                        }
                    }
                }
                return instance;
            }
        }

        #region Logger instance

        private const string PolicyName = "FrameworkConfigProvider";
        //        private static readonly LogWrapper log = new LogWrapper();

        #endregion

        #endregion

        #region Private Internal Methods

        private const string FrameworkConfigurationSectionNameMappings = "FrameworkConfigSection";
        private const string PlugginPath = "/Scripts/Framework/Pluggins/{0}";
        private string scriptPath = "/Js/Framework/{0}";

        private FrameworkPlugginCollection GetPlugginCollection()
        {
            var mappingsSection = GetFrameworkSection();
            if (mappingsSection != null)
            {
                return mappingsSection.FrameworkPlugginItems;
            }
            return null; // OOPS!
        }

        private bool IsPlugginEnabled(string fileName)
        {
            bool isValid = false;
            try
            {
                var mappingsSection = GetFrameworkSection();
                if (mappingsSection != null)
                {
                    FrameworkPlugginCollection configCollection = mappingsSection.FrameworkPlugginItems;
                    if (configCollection.Count > 0)
                    {
                        for (int x = 0; x < configCollection.Count; x++)
                        {
                            string a = configCollection[x].FileName;
                            bool b;
                            bool.TryParse(configCollection[x].Enabled, out b);
                            if (fileName == a && b)
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
                STPConfigurationManager.LogProvider.Log(ex);
            }
            return isValid;
        }

        private int TotalPluggins()
        {
            int totalCount = 0;
            try
            {
                var mappingsSection = GetFrameworkSection();
                if (mappingsSection != null)
                {
                    FrameworkPlugginCollection commonConfigCollection = mappingsSection.FrameworkPlugginItems;
                    totalCount = commonConfigCollection.Count;
                }
            }
            catch (Exception ex)
            {
                STPConfigurationManager.LogProvider.Log(ex);
            }
            return totalCount;
        }

        private string GetPluggin(string key)
        {
            string value = string.Empty;
            try
            {
                var mappingsSection = GetFrameworkSection();
                if (mappingsSection != null)
                {
                    FrameworkPlugginCollection commonConfigCollection = mappingsSection.FrameworkPlugginItems;
                    if (commonConfigCollection.Count > 0)
                    {
                        for (int x = 0; x < commonConfigCollection.Count; x++)
                        {
                            if (commonConfigCollection[x].ID.ToLower() == key.ToLower())
                            {
                                value = commonConfigCollection[x].FileName;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                STPConfigurationManager.LogProvider.Log(ex);
            }
            return value;
        }

        private string GetFramework()
        {
            string fileName = string.Empty;
            try
            {
                var mappingsSection = GetFrameworkSection();
                if (mappingsSection != null && mappingsSection.FrameworkRootItem.Count > 0)
                {
                    fileName = mappingsSection.FrameworkRootItem[0].FileName;
                }
            }
            catch (Exception ex)
            {
                STPConfigurationManager.LogProvider.Log(ex);
            }
            return fileName;
        }

        private bool IsFrameworkEnabled()
        {
            bool isEnabled = false;
            try
            {
                var mappingsSection = GetFrameworkSection();
                if (mappingsSection != null && mappingsSection.FrameworkRootItem.Count > 0)
                {
                    bool b;
                    bool.TryParse(mappingsSection.FrameworkRootItem[0].Enabled, out b);
                    isEnabled = b;
                }
            }
            catch (Exception ex)
            {
                STPConfigurationManager.LogProvider.Log(ex);
            }
            return isEnabled;
        }

        #endregion

        #region Helper Methods

        private FrameworkSection GetFrameworkSection()
        {
            FrameworkSection frameworkSection = null;
            try
            {
                frameworkSection =
                    (FrameworkSection)
                        ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                            FrameworkConfigurationSectionNameMappings));
            }
            catch (Exception ex)
            {
                STPConfigurationManager.LogProvider.Log(ex);
            }
            return frameworkSection;
        }

        #endregion

        #region Implementation of IFrameworkConfigProvider

        /// <summary>
        ///     Register Framework
        /// </summary>
        public void RegisterFramework()
        {
            bool isFrameworkEnabled = IsFrameworkEnabled();
            if (isFrameworkEnabled)
            {
                string scriptFramework = GetFramework();
                scriptPath = string.Format(scriptPath, scriptFramework.Trim());
                var page = (Page) HttpContext.Current.CurrentHandler;
                if (page != null && scriptPath.IfFileExist())
                {
                    var script = new HtmlGenericControl("script");
                    script.Attributes.Add("type", "text/javascript");
                    script.Attributes.Add("src", scriptPath);
                    page.Header.Controls.Add(script);
                    page.Header.Controls.Add(new LiteralControl(Environment.NewLine));

                    //ClieSTPcriptProxy.Current.RegisterClieSTPcriptInclude(page, page.GetType(), scriptPath,
                    //                                                      ScriptRenderMode.HeaderTop);
                }
            }
        }

        /// <summary>
        ///     Populate Selected Framework Pluggins
        /// </summary>
        public void RegisterFrameworkPluggins(string ids)
        {
            string[] sArr = ids.Split(",".ToCharArray());
            if (sArr.Length > 0)
            {
                bool isFrameworkEnabled = IsFrameworkEnabled();
                if (isFrameworkEnabled)
                {
                    FrameworkPlugginCollection plugginCollection = GetPlugginCollection();
                    var page = (Page) HttpContext.Current.CurrentHandler;
                    if (page != null)
                    {
                        ScriptManager scriptManager = ScriptManager.GetCurrent(page);
                        if (scriptManager != null)
                        {
                            if (plugginCollection != null && plugginCollection.Count > 0)
                            {
                                foreach (string iid in sArr)
                                {
                                    foreach (FrameworkPluggin pluggin in plugginCollection)
                                    {
                                        if (pluggin.ID.Trim() == iid.Trim())
                                        {
                                            string plugginp = string.Format(PlugginPath, pluggin.FileName.Trim());
                                            if (pluggin.Enabled.ToBool() && plugginp.IfFileExist())
                                            {
                                                var scriptReference = new ScriptReference(plugginp);
                                                if (!scriptManager.Scripts.Contains(scriptReference))
                                                {
                                                    scriptManager.Scripts.Add(scriptReference);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Remove Selected Framework Pluggins
        /// </summary>
        public void UnRegisterFrameworkPluggins(string ids)
        {
            string[] sArr = ids.Split(",".ToCharArray());
            if (sArr.Length > 0)
            {
                bool isFrameworkEnabled = IsFrameworkEnabled();
                if (isFrameworkEnabled)
                {
                    FrameworkPlugginCollection plugginCollection = GetPlugginCollection();
                    var page = (Page) HttpContext.Current.CurrentHandler;
                    if (page != null)
                    {
                        ScriptManager scriptManager = ScriptManager.GetCurrent(page);
                        if (scriptManager != null)
                        {
                            if (plugginCollection != null && plugginCollection.Count > 0)
                            {
                                foreach (string iid in sArr)
                                {
                                    foreach (FrameworkPluggin pluggin in plugginCollection)
                                    {
                                        if (pluggin.ID.Trim() == iid.Trim())
                                        {
                                            string plugginp = string.Format(PlugginPath, pluggin.FileName.Trim());
                                            if (pluggin.Enabled.ToBool() && plugginp.IfFileExist())
                                            {
                                                var scriptReference = new ScriptReference(plugginp);
                                                if (!scriptManager.Scripts.Contains(scriptReference))
                                                {
                                                    scriptManager.Scripts.Remove(scriptReference);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Remove All Framework Pluggins
        /// </summary>
        public void UnRegisterFrameworkPluggins()
        {
            bool isFrameworkEnabled = IsFrameworkEnabled();
            if (isFrameworkEnabled)
            {
                FrameworkPlugginCollection plugginCollection = GetPlugginCollection();
                var page = (Page) HttpContext.Current.CurrentHandler;
                if (page != null)
                {
                    ScriptManager scriptManager = ScriptManager.GetCurrent(page);
                    if (scriptManager != null)
                    {
                        if (plugginCollection != null && plugginCollection.Count > 0)
                        {
                            foreach (FrameworkPluggin pluggin in plugginCollection)
                            {
                                string plugginp = string.Format(PlugginPath, pluggin.FileName.Trim());
                                if (pluggin.Enabled.ToBool() && plugginp.IfFileExist())
                                {
                                    var scriptReference = new ScriptReference(plugginp);
                                    if (!scriptManager.Scripts.Contains(scriptReference))
                                    {
                                        scriptManager.Scripts.Remove(scriptReference);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Populate All Framework Pluggins
        /// </summary>
        public void RegisterFrameworkPluggins()
        {
            bool isFrameworkEnabled = IsFrameworkEnabled();
            if (isFrameworkEnabled)
            {
                FrameworkPlugginCollection plugginCollection = GetPlugginCollection();
                var page = (Page) HttpContext.Current.CurrentHandler;
                if (page != null)
                {
                    ScriptManager scriptManager = ScriptManager.GetCurrent(page);
                    if (scriptManager != null)
                    {
                        if (plugginCollection != null && plugginCollection.Count > 0)
                        {
                            foreach (FrameworkPluggin pluggin in plugginCollection)
                            {
                                string plugginp = string.Format(PlugginPath, pluggin.FileName.Trim());
                                if (pluggin.Enabled.ToBool() && plugginp.IfFileExist())
                                {
                                    var scriptReference = new ScriptReference(plugginp);
                                    if (!scriptManager.Scripts.Contains(scriptReference))
                                    {
                                        scriptManager.Scripts.Add(scriptReference);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }

    #region FrameworkConfigCollection

    /// <summary>
    ///     FrameworkPlugginCollection for FrameworkConfigSectionMappings
    /// </summary>
    [ConfigurationCollection(typeof (FrameworkPluggin))]
    internal class FrameworkPlugginCollection : ConfigurationElementCollection
    {
        public FrameworkPluggin this[int idx]
        {
            get { return (FrameworkPluggin) BaseGet(idx); }
        }

        public new FrameworkPluggin this[string key]
        {
            get { return (FrameworkPluggin) BaseGet(key); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FrameworkPluggin();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FrameworkPluggin) (element)).FileName;
        }
    }

    /// <summary>
    ///     FrameworkRootCollection for FrameworkConfigSectionMappings
    /// </summary>
    [ConfigurationCollection(typeof (Framework))]
    internal class FrameworkCollection : ConfigurationElementCollection
    {
        public Framework this[int idx]
        {
            get { return (Framework) BaseGet(idx); }
        }

        public new Framework this[string key]
        {
            get { return (Framework) BaseGet(key); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Framework();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Framework) (element)).FileName;
        }
    }

    #endregion

    #region FrameworkConfigSection

    /// <summary>
    ///     FrameworkConfigSection
    /// </summary>
    internal class FrameworkSection : ConfigurationSection
    {
        private const string FrameworkPlugginsMappings = "Pluggins";
        private const string FrameworkRootMapping = "Framework";

        [ConfigurationProperty(FrameworkPlugginsMappings)]
        internal FrameworkPlugginCollection FrameworkPlugginItems
        {
            get { return ((FrameworkPlugginCollection) (base[FrameworkPlugginsMappings])); }
        }

        [ConfigurationProperty(FrameworkRootMapping)]
        internal FrameworkCollection FrameworkRootItem
        {
            get { return ((FrameworkCollection) (base[FrameworkRootMapping])); }
        }
    }

    #endregion

    #region Framework & FrameworkPluggins

    /// <summary>
    ///     FrameworkPluggins-FrameworkConfigSectionMappings
    /// </summary>
    internal class FrameworkPluggin : ConfigurationElement
    {
        private const string IsEnabled = "Enabled";
        private const string IID = "ID";
        private const string FFileName = "FileName";

        [ConfigurationProperty(IsEnabled, DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Enabled
        {
            get { return ((string) (base[IsEnabled])); }
            set { base[IsEnabled] = value; }
        }

        [ConfigurationProperty(IID, DefaultValue = "", IsKey = true, IsRequired = true)]
        public string ID
        {
            get { return ((string) (base[IID])); }
            set { base[IID] = value; }
        }

        [ConfigurationProperty(FFileName, DefaultValue = "", IsKey = true, IsRequired = true)]
        public string FileName
        {
            get { return ((string) (base[FFileName])); }
            set { base[FFileName] = value; }
        }
    }

    /// <summary>
    ///     Framework-FrameworkConfigSectionMappings
    /// </summary>
    internal class Framework : ConfigurationElement
    {
        private const string IsEnabled = "Enabled";
        private const string FFileName = "FileName";

        [ConfigurationProperty(IsEnabled, DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Enabled
        {
            get { return ((string) (base[IsEnabled])); }
            set { base[IsEnabled] = value; }
        }

        [ConfigurationProperty(FFileName, DefaultValue = "", IsKey = true, IsRequired = true)]
        public string FileName
        {
            get { return ((string) (base[FFileName])); }
            set { base[FFileName] = value; }
        }
    }

    #endregion
}