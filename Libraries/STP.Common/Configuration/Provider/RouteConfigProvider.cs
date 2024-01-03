#region

using System;
using System.Configuration;
using System.Diagnostics;
using STP.Common.Configuration.Interface;
using STP.Common.Log;

#endregion

namespace STP.Common.Configuration.Provider
{
    internal sealed class RouteConfigProvider : IRouteConfigProvider
    {
        #region Singleton

        private static volatile RouteConfigProvider instance;
        // Lock synchronization object
        private static readonly object SyncLock = new object();

        private RouteConfigProvider()
        {
        }

        internal static RouteConfigProvider Instance
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
                            instance = new RouteConfigProvider();
                        }
                    }
                }
                return instance;
            }
        }

        #region Logger instance

        private const string PolicyName = "RouteConfigProvider";
        //        private static readonly LogWrapper log = new LogWrapper();

        #endregion

        #endregion

        #region Private Methods for RouteConfigProvider

        private const string RouteConfigurationSectionName = "RouteConfigSection";

        #endregion

        #region Helper Methods

        private RouteConfigSection GetRouteSection()
        {
            RouteConfigSection routeSection = null;
            try
            {
                routeSection =
                    (RouteConfigSection)
                        ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
                            RouteConfigurationSectionName));
            }
            catch (Exception ex)
            {
                STPConfigurationManager.LogProvider.Log(ex);
            }
            return routeSection;
        }

        #endregion

        #region Implementation of IRouteConfigProvider

        public RouteCollection GetRouteCollection()
        {
            var mappingsSection = GetRouteSection();
            if (mappingsSection != null)
            {
                return mappingsSection.RoutesItems;
            }
            return null; // OOPS!
        }

        //public bool IsRoutEnabled(string routeName)
        //{
        //    bool isValid = false;
        //    try
        //    {
        //        var mappingsSection = GetRouteSection();
        //        if (mappingsSection != null)
        //        {
        //            RoutesCollection configCollection = mappingsSection.RoutesItems;
        //            if (configCollection.Count > 0)
        //            {
        //                for (int x = 0; x < configCollection.Count; x++)
        //                {
        //                    string a = configCollection[x].RouteName;
        //                    bool b;
        //                    bool.TryParse(configCollection[x].Enabled, out b);
        //                    if (routeName == a && b)
        //                    {
        //                        isValid = true;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        STPConfigurationManager.LogProvider.Log(ex);
        //    }
        //    return isValid;
        //}

        //public int TotalRoutes()
        //{
        //    int totalCount = 0;
        //    try
        //    {
        //        var mappingsSection = GetRouteSection();
        //        if (mappingsSection != null)
        //        {
        //            RoutesCollection commonConfigCollection = mappingsSection.RoutesItems;
        //            totalCount = commonConfigCollection.Count;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        STPConfigurationManager.LogProvider.Log(ex);
        //    }
        //    return totalCount;
        //}

        //public RoutesRoute GetRoute(string routeName)
        //{
        //    RoutesRoute value = null;
        //    try
        //    {
        //        var mappingsSection = GetRouteSection();
        //        if (mappingsSection != null)
        //        {
        //            RoutesCollection commonConfigCollection = mappingsSection.RoutesItems;
        //            if (commonConfigCollection.Count > 0)
        //            {
        //                for (int x = 0; x < commonConfigCollection.Count; x++)
        //                {
        //                    if (commonConfigCollection[x].ID.ToLower() == routeName.ToLower())
        //                    {
        //                        value = commonConfigCollection[x];
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        STPConfigurationManager.LogProvider.Log(ex);
        //    }
        //    return value;
        //}

        #endregion
    }

    #region RouteConfigCollection

    ///// <summary>
    /////   RoutesCollection for RouteConfigSectionMappings
    ///// </summary>
    //[ConfigurationCollection(typeof(RoutesRoute))]
    //public class RoutesCollection : ConfigurationElementCollection
    //{
    //    public RoutesRoute this[int idx]
    //    {
    //        get { return (RoutesRoute)BaseGet(idx); }
    //    }

    //    public new RoutesRoute this[string key]
    //    {
    //        get { return (RoutesRoute)BaseGet(key); }
    //    }

    //    protected override ConfigurationElement CreateNewElement()
    //    {
    //        return new RoutesRoute();
    //    }

    //    protected override object GetElementKey(ConfigurationElement element)
    //    {
    //        return ((RoutesRoute)(element)).RouteName;
    //    }
    //}

    #endregion

    #region Routes

    ///// <summary>
    /////   Routes-RouteConfigSectionMappings
    ///// </summary>
    //public class RoutesRoute : ConfigurationElement
    //{
    //    private const string IsEnabled = "Enabled";
    //    private const string IID = "ID";
    //    private const string RRouteName = "RouteName";
    //    private const string RRouteUrl = "RouteUrl";
    //    private const string PPhysicalFile = "PhysicalFile";
    //    private const string CCheckPhysicalUrlAccess = "CheckPhysicalUrlAccess";

    //    [ConfigurationProperty(IsEnabled, DefaultValue = "", IsKey = false, IsRequired = true)]
    //    public string Enabled
    //    {
    //        get { return ((string) (base[IsEnabled])); }
    //        set { base[IsEnabled] = value; }
    //    }

    //    [ConfigurationProperty(IID, DefaultValue = "", IsKey = true, IsRequired = true)]
    //    public string ID
    //    {
    //        get { return ((string) (base[IID])); }
    //        set { base[IID] = value; }
    //    }

    //    [ConfigurationProperty(RRouteName, DefaultValue = "", IsKey = true, IsRequired = true)]
    //    public string RouteName
    //    {
    //        get { return ((string) (base[RRouteName])); }
    //        set { base[RRouteName] = value; }
    //    }

    //    [ConfigurationProperty(RRouteUrl, DefaultValue = "", IsKey = true, IsRequired = true)]
    //    public string RouteUrl
    //    {
    //        get { return ((string) (base[RRouteUrl])); }
    //        set { base[RRouteUrl] = value; }
    //    }

    //    [ConfigurationProperty(PPhysicalFile, DefaultValue = "", IsKey = true, IsRequired = true)]
    //    public string PhysicalFile
    //    {
    //        get { return ((string) (base[PPhysicalFile])); }
    //        set { base[PPhysicalFile] = value; }
    //    }

    //    [ConfigurationProperty(CCheckPhysicalUrlAccess, DefaultValue = "", IsKey = true, IsRequired = true)]
    //    public string CheckPhysicalUrlAccess
    //    {
    //        get { return ((string) (base[CCheckPhysicalUrlAccess])); }
    //        set { base[CCheckPhysicalUrlAccess] = value; }
    //    }
    //}

    #endregion

    #region MURoute Section

    public class MURouteValueDictionary : ConfigurationElement
    {
        [ConfigurationProperty("Name")]
        public string Name
        {
            get { return (string) this["Name"]; }
        }

        [ConfigurationProperty("Key")]
        public string Key
        {
            get { return (string) this["Key"]; }
        }

        [ConfigurationProperty("Value")]
        public string Value
        {
            get { return (string) this["Value"]; }
        }

        [ConfigurationProperty("Enabled")]
        public bool Enabled
        {
            get { return (bool) this["Enabled"]; }
        }
    }

    public class MURouteCollection : ConfigurationElementCollection
    {
        //Constructor
        public MURouteCollection()
        {
            AddElementName = "MURouteValueDictionary";
        }

        [ConfigurationProperty("RouteName")]
        public string RouteName
        {
            get { return (string) this["RouteName"]; }
        }

        [ConfigurationProperty("RouteUrl")]
        public string RouteUrl
        {
            get { return (string) this["RouteUrl"]; }
        }

        [ConfigurationProperty("PhysicalFile")]
        public string PhysicalFile
        {
            get { return (string) this["PhysicalFile"]; }
        }

        [ConfigurationProperty("CheckPhysicalUrlAccess")]
        public bool CheckPhysicalUrlAccess
        {
            get { return (bool) this["CheckPhysicalUrlAccess"]; }
        }

        [ConfigurationProperty("ID")]
        public int ID
        {
            get { return (int) this["ID"]; }
        }

        [ConfigurationProperty("Enabled")]
        public bool Enabled
        {
            get { return (bool) this["Enabled"]; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new MURouteValueDictionary();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MURouteValueDictionary) element).Key;
        }
    }

    public class RouteCollection : ConfigurationElementCollection
    {
        public RouteCollection()
        {
            AddElementName = "MURoute";
        }

        //[ConfigurationProperty("name")]
        //public string Name
        //{
        //    get { return (string)this["name"]; }
        //}

        protected override ConfigurationElement CreateNewElement()
        {
            return new MURouteCollection();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MURouteCollection) element).ID;
        }
    }

    #region RouteConfigSectionReader

    /// <summary>
    ///     RouteConfigSection
    /// </summary>
    internal class RouteConfigSection : ConfigurationSection
    {
        private const string RoutesMappings = "RouteCollection";

        [ConfigurationProperty(RoutesMappings)]
        internal RouteCollection RoutesItems
        {
            get { return ((RouteCollection) (base[RoutesMappings])); }
        }
    }

    #endregion

    #endregion
}