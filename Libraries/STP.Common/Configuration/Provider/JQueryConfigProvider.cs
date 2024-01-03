#region

using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using STP.Common.Configuration.Interface;
using STP.Common.Extensions;
using STP.Common.Log;

#endregion

namespace STP.Common.Configuration.Provider
{
    public sealed class JQueryConfigProvider : IJQueryConfigProvider
    {
        #region Singleton

        private static volatile JQueryConfigProvider instance;
        // Lock synchronization object
        private static readonly object SyncLock = new object();

        private JQueryConfigProvider()
        {
        }

        internal static JQueryConfigProvider Instance
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
                            instance = new JQueryConfigProvider();
                        }
                    }
                }
                return instance;
            }
        }

        #region Logger instance

/*
        private const string PolicyName = "JQueryConfigProvider";
*/
        //        private static readonly LogWrapper log = new LogWrapper();

        #endregion

        #endregion

        private const string JQueryConfigurationSectionNameMappings = "JQueryConfigSection";
        private const string SourcePath = "/js/JQuery/{0}";
        private const string PlugginPath = "/js/JQuery/Pluggins/{0}";
        private const string Source10Path = "/js/JQuery/V103/{0}";
        private const string Pluggin10Path = "/js/JQuery/V103/Pluggins/{0}";

        #region Private Methods for JQueryConfigProvider

        #region JQuery

        private string GetJQuery(bool is10)
        {
            string fileName = string.Empty;
            try
            {
                if (is10)
                {
                    var mappingsSection = GetJQuerySection();
                    if (mappingsSection != null && mappingsSection.JQuery10RootItem.Count > 0)
                    {
                        fileName = mappingsSection.JQuery10RootItem[0].FileName;
                    }
                }
                else
                {
                    var mappingsSection = GetJQuerySection();
                    if (mappingsSection != null && mappingsSection.JQueryRootItem.Count > 0)
                    {
                        fileName = mappingsSection.JQueryRootItem[0].FileName;
                    }
                }
            }
            catch (Exception ex)
            {
                STPConfigurationManager.LogProvider.Log(ex);
            }
            return fileName;
        }

        private JQueryPlugginCollection GetJQueryPlugginCollection()
        {
            var mappingsSection = GetJQuerySection();
            if (mappingsSection != null)
            {
                return mappingsSection.JQueryPlugginItems;
            }
            return null; // OOPS!
        }

        private JQuery10PlugginCollection GetJQuery10PlugginCollection()
        {
            var mappingsSection = GetJQuerySection();
            if (mappingsSection != null)
            {
                return mappingsSection.JQuery10PlugginItems;
            }
            return null; // OOPS!
        }

        private bool IsJQueryPlugginEnabled(string fileName)
        {
            bool isValid = false;
            try
            {
                var mappingsSection = GetJQuerySection();
                if (mappingsSection != null)
                {
                    JQueryPlugginCollection configCollection = mappingsSection.JQueryPlugginItems;
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
                throw ex;
            }
            return isValid;
        }

        private int TotalJQueryPluggins()
        {
            int totalCount = 0;
            try
            {
                var mappingsSection = GetJQuerySection();
                if (mappingsSection != null)
                {
                    JQueryPlugginCollection commonConfigCollection = mappingsSection.JQueryPlugginItems;
                    totalCount = commonConfigCollection.Count;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return totalCount;
        }

        private string GetJQueryPluggin(string key)
        {
            string value = string.Empty;
            try
            {
                var mappingsSection = GetJQuerySection();
                if (mappingsSection != null)
                {
                    JQueryPlugginCollection commonConfigCollection = mappingsSection.JQueryPlugginItems;
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
                throw ex;
            }
            return value;
        }


        private bool IsJQueryEnabled()
        {
            bool isEnabled = false;
            try
            {
                var mappingsSection = GetJQuerySection();
                if (mappingsSection != null && mappingsSection.JQueryRootItem.Count > 0)
                {
                    bool b;
                    bool.TryParse(mappingsSection.JQueryRootItem[0].Enabled, out b);
                    isEnabled = b;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isEnabled;
        }

        private bool IsJQuery10Enabled()
        {
            bool isEnabled = false;
            try
            {
                var mappingsSection = GetJQuerySection();
                if (mappingsSection != null && mappingsSection.JQuery10RootItem.Count > 0)
                {
                    bool b;
                    bool.TryParse(mappingsSection.JQuery10RootItem[0].Enabled, out b);
                    isEnabled = b;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isEnabled;
        }

        #endregion

        #region Wijmo

        private string GetWijmo()
        {
            string fileName = string.Empty;
            try
            {
                var mappingsSection = GetJQuerySection();
                if (mappingsSection != null && mappingsSection.WijmoRootItem.Count > 0)
                {
                    fileName = mappingsSection.WijmoRootItem[0].FileName;
                }
            }
            catch (Exception ex)
            {
                STPConfigurationManager.LogProvider.Log(ex);
            }
            return fileName;
        }

        private WijmoJQueryPlugginCollection GetWizjoPlugginCollection()
        {
            var mappingsSection = GetJQuerySection();
            if (mappingsSection != null)
            {
                return mappingsSection.WijmoJQueryPlugginItems;
            }
            return null; // OOPS!
        }

        private bool IsWijmoPlugginEnabled(string fileName)
        {
            bool isValid = false;
            try
            {
                var mappingsSection = GetJQuerySection();
                if (mappingsSection != null)
                {
                    WijmoJQueryPlugginCollection configCollection = mappingsSection.WijmoJQueryPlugginItems;
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

        private int TotalWijmoPluggins()
        {
            int totalCount = 0;
            try
            {
                var mappingsSection = GetJQuerySection();
                if (mappingsSection != null)
                {
                    WijmoJQueryPlugginCollection commonConfigCollection = mappingsSection.WijmoJQueryPlugginItems;
                    totalCount = commonConfigCollection.Count;
                }
            }
            catch (Exception ex)
            {
                STPConfigurationManager.LogProvider.Log(ex);
            }
            return totalCount;
        }

        private string GetWijmoPluggin(string key)
        {
            string value = string.Empty;
            try
            {
                var mappingsSection = GetJQuerySection();
                if (mappingsSection != null)
                {
                    WijmoJQueryPlugginCollection commonConfigCollection = mappingsSection.WijmoJQueryPlugginItems;
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

        private bool IsWijmoEnabled()
        {
            bool isEnabled = false;
            try
            {
                var mappingsSection = GetJQuerySection();
                if (mappingsSection != null && mappingsSection.WijmoRootItem.Count > 0)
                {
                    bool b;
                    bool.TryParse(mappingsSection.WijmoRootItem[0].Enabled, out b);
                    isEnabled = b;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isEnabled;
        }

        #endregion

        #endregion

        #region Helper Methods

        private JQuerySection GetJQuerySection()
        {
            JQuerySection jQuerySection = null;
            try
            {
                jQuerySection =
                    (JQuerySection)
                        ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                            JQueryConfigurationSectionNameMappings));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jQuerySection;
        }

        private static bool IfFileExist(string filePath)
        {
            return File.Exists(HttpContext.Current.Server.MapPath(filePath));
        }

        #endregion

        #region Implementation of IJQueryConfigProvider

        /// <summary>
        ///     Register JQuery
        /// </summary>
        public void RegisterJQuery()
        {
            bool isJQueryEnabled = IsJQueryEnabled();
            if (isJQueryEnabled)
            {
                string jQueryFile = GetJQuery(false);
                string sPath = string.Format(SourcePath, jQueryFile.Trim());

                var page = (Page) HttpContext.Current.CurrentHandler;
                if (page != null && IfFileExist(sPath))
                {
                    var script = new HtmlGenericControl("script");
                    script.Attributes.Add("type", "text/javascript");
                    script.Attributes.Add("src", sPath);
                    page.Header.Controls.Add(script);
                    page.Header.Controls.Add(new LiteralControl(Environment.NewLine));
                }
            }
        }

        /// <summary>
        ///     Register JQuery
        /// </summary>
        public void RegisterJQuery10()
        {
            bool isJQueryEnabled = IsJQuery10Enabled();
            if (isJQueryEnabled)
            {
                string jQueryFile = GetJQuery(true);
                string sPath = string.Format(Source10Path, jQueryFile.Trim());

                var page = (Page) HttpContext.Current.CurrentHandler;
                if (page != null && IfFileExist(sPath))
                {
                    var script = new HtmlGenericControl("script");
                    script.Attributes.Add("type", "text/javascript");
                    script.Attributes.Add("src", sPath);
                    page.Header.Controls.Add(script);
                    page.Header.Controls.Add(new LiteralControl(Environment.NewLine));
                }
            }
        }

        /// <summary>
        ///     <para>Populate Selected JQuery Pluggins</para>
        ///     <para>Check the config files for the number you want to load the script</para>
        ///     <para>Usage: STPScriptManager.Provider.RegisterJQueryPluggins("3,23");</para>
        /// </summary>
        /// <param name="ids">Pluggin Ids from the config files which will load in runtime</param>
        public void RegisterJQueryPluggins(string ids)
        {
            string[] sArr = ids.Split(",".ToCharArray());
            if (sArr.Length > 0)
            {
                bool isJQueryEnabled = IsJQueryEnabled();
                if (isJQueryEnabled)
                {
                    JQueryPlugginCollection plugginCollection = GetJQueryPlugginCollection();
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
                                    foreach (JQueryPluggin pluggin in plugginCollection)
                                    {
                                        if (pluggin.ID.Trim() == iid.Trim())
                                        {
                                            string pPath = string.Format(PlugginPath, pluggin.FileName.Trim());
                                            if (pluggin.Enabled.ToBool() && IfFileExist(pPath))
                                            {
                                                var scriptReference = new ScriptReference(pPath);
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
        ///     <para>Populate Selected JQuery Pluggins for version 1.10</para>
        ///     <para>Check the config files for the number you want to load the script</para>
        ///     <para>Usage: STPScriptManager.Provider.RegisterJQueryPluggins("3,23");</para>
        /// </summary>
        /// <param name="ids">Pluggin Ids from the config files which will load in runtime</param>
        public void RegisterJQuery10Pluggins(string ids)
        {
            string[] sArr = ids.Split(",".ToCharArray());
            if (sArr.Length > 0)
            {
                bool isJQueryEnabled = IsJQuery10Enabled();
                if (isJQueryEnabled)
                {
                    JQuery10PlugginCollection plugginCollection = GetJQuery10PlugginCollection();
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
                                    foreach (JQuery10Pluggin pluggin in plugginCollection)
                                    {
                                        if (pluggin.ID.Trim() == iid.Trim())
                                        {
                                            string pPath = string.Format(Pluggin10Path, pluggin.FileName.Trim());
                                            if (pluggin.Enabled.ToBool() && IfFileExist(pPath))
                                            {
                                                var scriptReference = new ScriptReference(pPath);
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
        ///     Remove JQuery Pluggins
        /// </summary>
        public void UnRegisterJQueryPluggins(string ids)
        {
            string[] sArr = ids.Split(",".ToCharArray());
            if (sArr.Length > 0)
            {
                bool isJQueryEnabled = IsJQueryEnabled();
                if (isJQueryEnabled)
                {
                    JQueryPlugginCollection plugginCollection = GetJQueryPlugginCollection();
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
                                    foreach (JQueryPluggin pluggin in plugginCollection)
                                    {
                                        if (pluggin.ID.Trim() == iid.Trim())
                                        {
                                            string pPath = string.Format(PlugginPath, pluggin.FileName.Trim());
                                            if (pluggin.Enabled.ToBool() && IfFileExist(pPath))
                                            {
                                                var scriptReference = new ScriptReference(pPath);
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
        ///     Remove All JQuery Pluggins
        /// </summary>
        public void UnRegisterJQueryPluggins()
        {
            bool isJQueryEnabled = IsJQueryEnabled();
            if (isJQueryEnabled)
            {
                JQueryPlugginCollection plugginCollection = GetJQueryPlugginCollection();
                var page = (Page) HttpContext.Current.CurrentHandler;
                if (page != null)
                {
                    ScriptManager scriptManager = ScriptManager.GetCurrent(page);
                    if (scriptManager != null)
                    {
                        if (plugginCollection != null && plugginCollection.Count > 0)
                        {
                            foreach (JQueryPluggin pluggin in plugginCollection)
                            {
                                string pPath = string.Format(PlugginPath, pluggin.FileName.Trim());
                                if (pluggin.Enabled.ToBool() && IfFileExist(pPath))
                                {
                                    var scriptReference = new ScriptReference(pPath);
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
        ///     Populate All JQuery Pluggins from config
        /// </summary>
        public void RegisterJQueryPluggins()
        {
            bool isJQueryEnabled = IsJQueryEnabled();
            if (isJQueryEnabled)
            {
                JQueryPlugginCollection plugginCollection = GetJQueryPlugginCollection();
                var page = (Page) HttpContext.Current.CurrentHandler;
                if (page != null)
                {
                    ScriptManager scriptManager = ScriptManager.GetCurrent(page);
                    if (scriptManager != null)
                    {
                        if (plugginCollection != null && plugginCollection.Count > 0)
                        {
                            foreach (JQueryPluggin pluggin in plugginCollection)
                            {
                                string pPath = string.Format(PlugginPath, pluggin.FileName.Trim());
                                if (pluggin.Enabled.ToBool() && IfFileExist(pPath))
                                {
                                    var scriptReference = new ScriptReference(pPath);
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

    #region JQueryConfigCollection

    /// <summary>
    ///     JQueryPlugginCollection for JQueryConfigSectionMappings
    /// </summary>
    [ConfigurationCollection(typeof (JQueryPluggin))]
    public class JQueryPlugginCollection : ConfigurationElementCollection
    {
        public JQueryPluggin this[int idx]
        {
            get { return (JQueryPluggin) BaseGet(idx); }
        }

        public new JQueryPluggin this[string key]
        {
            get { return (JQueryPluggin) BaseGet(key); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new JQueryPluggin();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((JQueryPluggin) (element)).FileName;
        }
    }

    /// <summary>
    ///     JQueryRootCollection for JQueryConfigSectionMappings
    /// </summary>
    [ConfigurationCollection(typeof (JQuery))]
    public class JQueryCollection : ConfigurationElementCollection
    {
        public JQuery this[int idx]
        {
            get { return (JQuery) BaseGet(idx); }
        }

        public new JQuery this[string key]
        {
            get { return (JQuery) BaseGet(key); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new JQuery();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((JQuery) (element)).FileName;
        }
    }

    /// <summary>
    ///     JQueryPlugginCollection for JQueryConfigSectionMappings
    /// </summary>
    [ConfigurationCollection(typeof (JQuery10Pluggin))]
    public class JQuery10PlugginCollection : ConfigurationElementCollection
    {
        public JQuery10Pluggin this[int idx]
        {
            get { return (JQuery10Pluggin) BaseGet(idx); }
        }

        public new JQuery10Pluggin this[string key]
        {
            get { return (JQuery10Pluggin) BaseGet(key); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new JQuery10Pluggin();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((JQuery10Pluggin) (element)).FileName;
        }
    }

    /// <summary>
    ///     JQueryRootCollection for JQueryConfigSectionMappings
    /// </summary>
    [ConfigurationCollection(typeof (JQuery))]
    public class JQuery10Collection : ConfigurationElementCollection
    {
        public JQuery10 this[int idx]
        {
            get { return (JQuery10) BaseGet(idx); }
        }

        public new JQuery10 this[string key]
        {
            get { return (JQuery10) BaseGet(key); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new JQuery10();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((JQuery10) (element)).FileName;
        }
    }

    /// <summary>
    ///     WijmoJQueryPlugginCollection for JQueryConfigSectionMappings
    /// </summary>
    [ConfigurationCollection(typeof (WijmoJQueryPluggin))]
    public class WijmoJQueryPlugginCollection : ConfigurationElementCollection
    {
        public WijmoJQueryPluggin this[int idx]
        {
            get { return (WijmoJQueryPluggin) BaseGet(idx); }
        }

        public new WijmoJQueryPluggin this[string key]
        {
            get { return (WijmoJQueryPluggin) BaseGet(key); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new WijmoJQueryPluggin();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WijmoJQueryPluggin) (element)).FileName;
        }
    }

    /// <summary>
    ///     WijmoRootCollection for WijmoJQueryConfigSectionMappings
    /// </summary>
    [ConfigurationCollection(typeof (Wijmo))]
    public class WijmoCollection : ConfigurationElementCollection
    {
        public Wijmo this[int idx]
        {
            get { return (Wijmo) BaseGet(idx); }
        }

        public new Wijmo this[string key]
        {
            get { return (Wijmo) BaseGet(key); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Wijmo();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Wijmo) (element)).FileName;
        }
    }

    #endregion

    #region JQueryConfigSection

    /// <summary>
    ///     JQueryConfigSection
    /// </summary>
    internal class JQuerySection : ConfigurationSection
    {
        private const string JQueryPlugginsMappings = "JQueryPluggins";
        private const string JQueryRootMapping = "JQuery";
        private const string JQuery10PlugginsMappings = "JQuery10Pluggins";
        private const string JQuery10RootMapping = "JQuery10";

        private const string WijmoJQueryPlugginsMappings = "WijmoJQueryPluggins";
        private const string WijmoRootMapping = "Wijmo";


        [ConfigurationProperty(JQueryPlugginsMappings)]
        internal JQueryPlugginCollection JQueryPlugginItems
        {
            get { return ((JQueryPlugginCollection) (base[JQueryPlugginsMappings])); }
        }

        [ConfigurationProperty(JQueryRootMapping)]
        internal JQueryCollection JQueryRootItem
        {
            get { return ((JQueryCollection) (base[JQueryRootMapping])); }
        }

        [ConfigurationProperty(JQuery10PlugginsMappings)]
        internal JQuery10PlugginCollection JQuery10PlugginItems
        {
            get { return ((JQuery10PlugginCollection) (base[JQuery10PlugginsMappings])); }
        }

        [ConfigurationProperty(JQuery10RootMapping)]
        internal JQuery10Collection JQuery10RootItem
        {
            get { return ((JQuery10Collection) (base[JQuery10RootMapping])); }
        }


        [ConfigurationProperty(WijmoJQueryPlugginsMappings)]
        internal WijmoJQueryPlugginCollection WijmoJQueryPlugginItems
        {
            get { return ((WijmoJQueryPlugginCollection) (base[WijmoJQueryPlugginsMappings])); }
        }

        [ConfigurationProperty(WijmoRootMapping)]
        internal WijmoCollection WijmoRootItem
        {
            get { return ((WijmoCollection) (base[WijmoRootMapping])); }
        }
    }

    #endregion

    #region JQuery & JQueryPluggins

    /// <summary>
    ///     JQueryPluggins-JQueryConfigSectionMappings
    /// </summary>
    public class JQueryPluggin : ConfigurationElement
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
    ///     JQuery-JQueryConfigSectionMappings
    /// </summary>
    public class JQuery : ConfigurationElement
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

    /// <summary>
    ///     JQueryPluggins-JQueryConfigSectionMappings
    /// </summary>
    public class JQuery10Pluggin : ConfigurationElement
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
    ///     JQuery-JQueryConfigSectionMappings
    /// </summary>
    public class JQuery10 : ConfigurationElement
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

    #region Wijmo & WijmoJQueryPluggins

    /// <summary>
    ///     WijmoJQueryPluggins-JQueryConfigSectionMappings
    /// </summary>
    public class WijmoJQueryPluggin : ConfigurationElement
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
    ///     Wijmo-JQueryConfigSectionMappings
    /// </summary>
    public class Wijmo : ConfigurationElement
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