#region

using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.UI;
using STP.Common.Configuration.Interface;
using STP.Common.Extensions;
using STP.Common.Log;

#endregion

namespace STP.Common.Configuration.Provider
{
    public sealed class ScriptConfigProvider : IScriptConfigProvider
    {
        #region Singleton

        private static volatile ScriptConfigProvider instance;
        // Lock synchronization object
        private static readonly object syncLock = new object();

        private ScriptConfigProvider()
        {
        }

        internal static ScriptConfigProvider Instance
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
                            instance = new ScriptConfigProvider();
                        }
                    }
                }
                return instance;
            }
        }

        #region Logger instance

        private const string PolicyName = "ScriptConfigProvider";
        //        private static readonly LogWrapper log = new LogWrapper();

        #endregion

        #endregion

        #region Private Methds  for IScriptConfigProvider

        private const string ScriptConfigurationSectionMappings = "ScriptConfigSection";
        private const string ScriptPath = "/js/Scripts/{0}";
        //private const string FrameWorkPath = "/js/Framework/{0}";
        private const string ResourcePath = "/js/Resources/{0}";


        private ScriptConfigCollection GetScriptCollection()
        {
            var mappingsSection = GetScriptSection();
            if (mappingsSection != null)
            {
                return mappingsSection.ScriptConfigItems;
            }
            return null; // OOPS!
        }

        private ResourceConfigCollection GetResourceConfigCollection()
        {
            var mappingsSection = GetScriptSection();
            if (mappingsSection != null)
            {
                return mappingsSection.ResourceConfigItems;
            }
            return null; // OOPS!
        }

        private bool IsResourceEnabled(string fileName)
        {
            bool isValid = false;
            try
            {
                var mappingsSection =
                    (ScriptSection)
                        ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                            ScriptConfigurationSectionMappings));
                if (mappingsSection != null)
                {
                    var configCollection = mappingsSection.ResourceConfigItems;
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

        private bool IsScriptEnabled(string fileName)
        {
            bool isValid = false;
            try
            {
                var mappingsSection =
                    (ScriptSection)
                        ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                            ScriptConfigurationSectionMappings));
                if (mappingsSection != null)
                {
                    ScriptConfigCollection configCollection = mappingsSection.ScriptConfigItems;
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

        private int TotalScripts()
        {
            int totalCount = 0;
            try
            {
                var mappingsSection = GetScriptSection();
                if (mappingsSection != null)
                {
                    ScriptConfigCollection commonConfigCollection = mappingsSection.ScriptConfigItems;
                    totalCount = commonConfigCollection.Count;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return totalCount;
        }

        private string GetScript(string key)
        {
            string value = string.Empty;
            try
            {
                var mappingsSection = GetScriptSection();
                if (mappingsSection != null)
                {
                    ScriptConfigCollection commonConfigCollection = mappingsSection.ScriptConfigItems;
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

        private string GetScriptFramework()
        {
            string fileName = string.Empty;
            try
            {
                var mappingsSection = GetScriptSection();
                if (mappingsSection != null && mappingsSection.ScriptFrameworkItem.Count > 0)
                {
                    fileName = mappingsSection.ScriptFrameworkItem[0].FileName;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return fileName;
        }

        private bool IsFrameworkEnabled()
        {
            bool isEnabled = false;
            try
            {
                var mappingsSection = GetScriptSection();
                if (mappingsSection != null && mappingsSection.ScriptFrameworkItem.Count > 0)
                {
                    bool b;
                    bool.TryParse(mappingsSection.ScriptFrameworkItem[0].Enabled, out b);
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

        private ScriptSection GetScriptSection()
        {
            ScriptSection scriptSection = null;
            try
            {
                scriptSection =
                    (ScriptSection)
                        ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                            ScriptConfigurationSectionMappings));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return scriptSection;
        }

        #region Helper Method

        private static bool IfFileExist(string filePath)
        {
            return File.Exists(HttpContext.Current.Server.MapPath(filePath));
        }

        #endregion

        #region Implementation of IScriptConfigProvider

        //// <summary>
        ////   Populate JQuery
        //// </summary>
        //public void RegisterKnockout()
        //{
        //    bool isFrameworkEnabled = IsFrameworkEnabled();
        //    if (isFrameworkEnabled)
        //    {
        //        string scriptFramework = GetScriptFramework();
        //        string fWorkPath = string.Format(FrameWorkPath, scriptFramework.Trim());
        //        var page = (Page) HttpContext.Current.CurrentHandler;
        //        if (page != null && IfFileExist(fWorkPath))
        //        {
        //            var script = new HtmlGenericControl("script");
        //            script.Attributes.Add("type", "text/javascript");
        //            script.Attributes.Add("src", fWorkPath);
        //            page.Header.Controls.Add(script);
        //            page.Header.Controls.Add(new LiteralControl(Environment.NewLine));
        //        }
        //    }
        //}

        /// <summary>
        ///     <para>Populate Selected Scripts from Scripts Folders</para>
        ///     <para>Check the config files for the number you want to load the script</para>
        ///     <para>Usage  STPScriptManager.Provider.RegisterScripts("1,5,8");</para>
        /// </summary>
        /// <param name="ids">Script Ids from the config files which will load in runtime</param>
        public void RegisterScripts(string ids)
        {
            string[] sArr = ids.Split(",".ToCharArray());
            if (sArr.Length > 0)
            {
                ScriptConfigCollection scriptCollection = GetScriptCollection();
                var page = (Page) HttpContext.Current.CurrentHandler;
                if (page != null)
                {
                    ScriptManager scriptManager = ScriptManager.GetCurrent(page);
                    if (scriptManager != null)
                    {
                        if (scriptCollection != null && scriptCollection.Count > 0)
                        {
                            foreach (string iid in sArr)
                            {
                                foreach (Scripts script in scriptCollection)
                                {
                                    if (script.ID.Trim() == iid.Trim())
                                    {
                                        string spath = string.Format(ScriptPath, script.FileName.Trim());
                                        if (script.Enabled.ToBool() && IfFileExist(spath))
                                        {
                                            var scriptReference = new ScriptReference(spath);
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

        /// <summary>
        ///     Remove All Scripts
        /// </summary>
        public void UnRegisterScripts(string ids)
        {
            string[] sArr = ids.Split(",".ToCharArray());
            if (sArr.Length > 0)
            {
                ScriptConfigCollection scriptCollection = GetScriptCollection();
                var page = (Page) HttpContext.Current.CurrentHandler;
                if (page != null)
                {
                    ScriptManager scriptManager = ScriptManager.GetCurrent(page);
                    if (scriptManager != null)
                    {
                        if (scriptCollection != null && scriptCollection.Count > 0)
                        {
                            foreach (string iid in sArr)
                            {
                                foreach (Scripts script in scriptCollection)
                                {
                                    if (script.ID.Trim() == iid.Trim())
                                    {
                                        string sPath = string.Format(ScriptPath, script.FileName.Trim());
                                        if (script.Enabled.ToBool() && IfFileExist(ScriptPath))
                                        {
                                            var scriptReference = new ScriptReference(sPath);
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

        /// <summary>
        ///     Remove Scripts
        /// </summary>
        public void UnRegisterScripts()
        {
            ScriptConfigCollection scriptCollection = GetScriptCollection();
            var page = (Page) HttpContext.Current.CurrentHandler;
            if (page != null)
            {
                ScriptManager scriptManager = ScriptManager.GetCurrent(page);
                if (scriptManager != null)
                {
                    if (scriptCollection != null && scriptCollection.Count > 0)
                    {
                        foreach (Scripts script in scriptCollection)
                        {
                            string sPath = string.Format(ScriptPath, script.FileName.Trim());
                            if (script.Enabled.ToBool() && IfFileExist(sPath))
                            {
                                var scriptReference = new ScriptReference(sPath);
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

        /// <summary>
        ///     Register Resources
        /// </summary>
        public void RegisterResourceScripts()
        {
            var scriptCollection = GetResourceConfigCollection();
            var page = (Page) HttpContext.Current.CurrentHandler;
            if (page != null)
            {
                ScriptManager scriptManager = ScriptManager.GetCurrent(page);
                if (scriptManager != null)
                {
                    if (scriptCollection != null && scriptCollection.Count > 0)
                    {
                        foreach (Resources resources in scriptCollection)
                        {
                            string rPath = string.Format(ResourcePath, resources.FileName.Trim());
                            if (resources.Enabled.ToBool() && IfFileExist(rPath))
                            {
                                var scriptReference = new ScriptReference(rPath);
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

        /// <summary>
        ///     Populate All Scripts
        /// </summary>
        public void RegisterScripts()
        {
            ScriptConfigCollection scriptCollection = GetScriptCollection();
            var page = (Page) HttpContext.Current.CurrentHandler;
            if (page != null)
            {
                ScriptManager scriptManager = ScriptManager.GetCurrent(page);
                if (scriptManager != null)
                {
                    if (scriptCollection != null && scriptCollection.Count > 0)
                    {
                        foreach (Scripts script in scriptCollection)
                        {
                            string sPath = string.Format(ScriptPath, script.FileName.Trim());
                            if (script.Enabled.ToBool() && IfFileExist(sPath))
                            {
                                var scriptReference = new ScriptReference(sPath);
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

        #endregion

        #region Created for Vijay Warpe for Multi form Scripts

        /// <summary>
        ///     Poplulate MultiForm Scripts
        /// </summary>
        public void RegisterMultiFormScripts()
        {
            //const string sourcePath = "/js/MultiForm/{0}";
            //ScriptFileStructure scriptFileStructure = GetScriptFileStructure(MULTIFORM_PATH, sourcePath);
            //if (scriptFileStructure != null && scriptFileStructure.ScriptIncludes.Count > 0)
            //{
            //    RegisterScriptReference(scriptFileStructure);
            //}


            //var scriptCollection = GetResourceConfigCollection();
            //var page = (Page)HttpContext.Current.CurrentHandler;
            //if (page != null)
            //{
            //    ScriptManager scriptManager = ScriptManager.GetCurrent(page);
            //    if (scriptManager != null)
            //    {
            //        if (scriptCollection != null && scriptCollection.Count > 0)
            //        {
            //            foreach (Resources resources in scriptCollection)
            //            {
            //                string rPath = string.Format(ResourcePath, resources.FileName.Trim());
            //                if (resources.Enabled.ToBool() && IfFileExist(rPath))
            //                {
            //                    var scriptReference = new ScriptReference(rPath);
            //                    if (!scriptManager.Scripts.Contains(scriptReference))
            //                    {
            //                        scriptManager.Scripts.Add(scriptReference);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }

        #endregion
    }

    #region ScriptConfigCollection

    /// <summary>
    ///     ScriptConfigCollection for ScriptConfigSectionMappings
    /// </summary>
    [ConfigurationCollection(typeof (Scripts))]
    public class ScriptConfigCollection : ConfigurationElementCollection
    {
        public Scripts this[int idx]
        {
            get { return (Scripts) BaseGet(idx); }
        }

        public new Scripts this[string key]
        {
            get { return (Scripts) BaseGet(key); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Scripts();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Scripts) (element)).FileName;
        }
    }

    /// <summary>
    ///     ScriptFrameworkCollection for ScriptFrameworkMappings
    /// </summary>
    [ConfigurationCollection(typeof (ScriptFramework))]
    public class ScriptFrameworkCollection : ConfigurationElementCollection
    {
        public ScriptFramework this[int idx]
        {
            get { return (ScriptFramework) BaseGet(idx); }
        }

        public new ScriptFramework this[string key]
        {
            get { return (ScriptFramework) BaseGet(key); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ScriptFramework();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ScriptFramework) (element)).FileName;
        }
    }

    /// <summary>
    ///     ResourceConfigCollection for ScriptConfigSectionMappings
    /// </summary>
    [ConfigurationCollection(typeof (Resources))]
    public class ResourceConfigCollection : ConfigurationElementCollection
    {
        public Resources this[int idx]
        {
            get { return (Resources) BaseGet(idx); }
        }

        public new Resources this[string key]
        {
            get { return (Resources) BaseGet(key); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Resources();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Resources) (element)).FileName;
        }
    }

    #endregion

    #region ScriptConfigSection

    /// <summary>
    ///     JQueryConfigSection
    /// </summary>
    internal class ScriptSection : ConfigurationSection
    {
        private const string ScriptsMappings = "Scripts";
        private const string ScriptFrameworkMapping = "ScriptFramework";
        private const string ResourceMappings = "Resources";

        [ConfigurationProperty(ScriptsMappings)]
        internal ScriptConfigCollection ScriptConfigItems
        {
            get { return ((ScriptConfigCollection) (base[ScriptsMappings])); }
        }

        [ConfigurationProperty(ScriptFrameworkMapping)]
        internal ScriptFrameworkCollection ScriptFrameworkItem
        {
            get { return ((ScriptFrameworkCollection) (base[ScriptFrameworkMapping])); }
        }

        [ConfigurationProperty(ResourceMappings)]
        internal ResourceConfigCollection ResourceConfigItems
        {
            get { return ((ResourceConfigCollection) (base[ResourceMappings])); }
        }
    }

    #endregion

    #region FrameWork & Scripts

    /// <summary>
    ///     Scripts-ScriptConfigSectionMappings
    /// </summary>
    public class Scripts : ConfigurationElement
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
    ///     ScriptFramework-ScriptConfigSectionMappings
    /// </summary>
    public class ScriptFramework : ConfigurationElement
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
    ///     Resource-ResourceSectionMappings
    /// </summary>
    public class Resources : ConfigurationElement
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

    #endregion
}