#region

using System.Configuration;
using System.Diagnostics;
using STP.Common.Configuration.Interface;
using STP.Common.Enums;

#endregion

namespace STP.Common.Configuration.Provider
{
    public sealed class DatabaseConfigProvider : IDatabaseConfigProvider
    {
        #region Singleton

        private static volatile DatabaseConfigProvider instance;
        // Lock synchronization object
        private static readonly object SyncLock = new object();

        private DatabaseConfigProvider()
        {
        }

        internal static DatabaseConfigProvider Instance
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
                            instance = new DatabaseConfigProvider();
                        }
                    }
                }
                return instance;
            }
        }

        #region Logger instance

        private const string PolicyName = "DatabaseConfigProvider";
        //        private static readonly LogWrapper log = new LogWrapper();

        #endregion

        #endregion

        #region Implementation of IDatabaseConfigProvider

        private const string MappingsDataAccessConfigurationSection = "dataAccessConfigurationSection";

        public string ConnectionString(string server, string subDataBase)
        {
            var mappingsSection =
                (DataAccessConfigurationSection)
                    ConfigurationManager.GetSection(string.Format("{0}", MappingsDataAccessConfigurationSection));
            if (mappingsSection != null)
            {
                return mappingsSection.DatabaseGroups[server].Databases[subDataBase].ConnectionString;
            }
            return string.Empty;
        }

        public int CommandTimeout
        {
            get
            {
                int commandTimeOut = 90;
                var mappingsSection =
                    (DataAccessConfigurationSection)
                        ConfigurationManager.GetSection(string.Format("{0}", MappingsDataAccessConfigurationSection));
                if (mappingsSection != null)
                {
                    commandTimeOut = mappingsSection.CommandTimeout.Value;
                }
                return commandTimeOut; // OOPS!
            }
        }

        public int VarcharMax
        {
            get
            {
                int varcharMax = 8000;
                var mappingsSection =
                    (DataAccessConfigurationSection)
                        ConfigurationManager.GetSection(string.Format("{0}", MappingsDataAccessConfigurationSection));
                if (mappingsSection != null)
                {
                    varcharMax = mappingsSection.VarcharMax.Value;
                }
                return varcharMax; // OOPS
            }
        }

        //int IDatabaseConfigProvider.VarcharMax
        //{
        //    get
        //    {
        //        int varcharMax = 8000;
        //        var mappingsSection =
        //            (DataAccessConfigurationSection)
        //            ConfigurationManager.GetSection(string.Format("{0}", MappingsDataAccessConfigurationSection));
        //        if (mappingsSection != null)
        //        {
        //            varcharMax = mappingsSection.VarcharMax.Value;
        //        }
        //        return varcharMax; // OOPS!
        //    }
        //}

        public DatabaseGroupCollection DatabaseGroupCollectionItems
        {
            get
            {
                var dataAccessConfigurationSection =
                    (DataAccessConfigurationSection)
                        ConfigurationManager.GetSection(string.Format("{0}", MappingsDataAccessConfigurationSection));
                DatabaseGroupCollection databaseGroupCollection = null;
                if (dataAccessConfigurationSection != null)
                {
                    databaseGroupCollection = dataAccessConfigurationSection.DatabaseGroups;
                }
                return databaseGroupCollection;
            }
        }

        public DatabaseGroupNode.DatabaseNodeCollection DatabaseNode(string server)
        {
            var mappingsSection =
                (DataAccessConfigurationSection)
                    ConfigurationManager.GetSection(string.Format("{0}", MappingsDataAccessConfigurationSection));
            if (mappingsSection != null)
            {
                return mappingsSection.DatabaseGroups[server].Databases;
            }
            return null; // OOPS!
        }


        //public int TotalSettings()
        //{
        //    int totalCount = 0;
        //    try
        //    {
        //        var mappingsSection =
        //            (AOBConfigSection)
        //            ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
        //                                                          MappingsAOBConfigurationSectionName));
        //        if (mappingsSection != null)
        //        {
        //            AOBConfigCollection aobConfigCollection = mappingsSection.AOBConfigItems;
        //            totalCount = aobConfigCollection.Count;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        STPConfigurationManager.LogProvider.Log(ex);
        //    }
        //    return totalCount;
        //}

        //public string GetValue(string key)
        //{
        //    string value = string.Empty;
        //    try
        //    {
        //        var mappingsSection =
        //            (AOBConfigSection)
        //            ConfigurationManager.GetSection(string.Format("{0}/{1}", "STPOnlineConfigGroup",
        //                                                          MappingsAOBConfigurationSectionName));
        //        if (mappingsSection != null)
        //        {
        //            AOBConfigCollection aobConfigCollection = mappingsSection.AOBConfigItems;
        //            if (aobConfigCollection.Count > 0)
        //            {
        //                for (int x = 0; x < aobConfigCollection.Count; x++)
        //                {
        //                    if (aobConfigCollection[x].Key.ToLower() == key.ToLower())
        //                    {
        //                        value = aobConfigCollection[x].Value;
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

    #region DataAccessConfigurationSection

    /// <summary>
    ///     Reads the configuarion file and returns the value of the xml node
    /// </summary>
    public sealed class DataAccessConfigurationSection : ConfigurationSection
    {
        /// <summary>
        ///     Gets a collection of database nodes
        /// </summary>
        [ConfigurationProperty("databaseGroups")]
        public DatabaseGroupCollection DatabaseGroups
        {
            get { return (DatabaseGroupCollection) this["databaseGroups"]; }
        }

        /// <summary>
        ///     Gets a collection of database nodes
        /// </summary>
        [ConfigurationProperty("STPDbInitInfo")]
        public STPDbNode STPDbInitInfo
        {
            get { return (STPDbNode) this["STPDbInitInfo"]; }
        }

        /// <summary>
        ///     Reads the command timeout node value from the config
        /// </summary>
        [ConfigurationProperty("commandTimeout")]
        public ConfigNodeValue<int> CommandTimeout
        {
            get
            {
                object timeout = this["commandTimeout"] ?? "90";

                return (ConfigNodeValue<int>) timeout;
            }
        }

        /// <summary>
        ///     The value of VARCHAR(MAX) for the current version of sql
        /// </summary>
        [ConfigurationProperty("varcharMax")]
        public ConfigNodeValue<int> VarcharMax
        {
            get { return (ConfigNodeValue<int>) this["varcharMax"]; }
        }

        /// <summary>
        ///     FrontEnd DatabaseId
        /// </summary>
        [ConfigurationProperty("FrontEndDatabaseId")]
        public ConfigNodeValue<int> FrontEndDatabaseId
        {
            get { return (ConfigNodeValue<int>) this["FrontEndDatabaseId"]; }
        }
    }

    #endregion

    #region DatabaseGroupCollection

    /// <Summary>
    ///     RepreseSTP the DatabaseGroup Node under DatabaseGroups node
    /// </Summary>
    [ConfigurationCollection(typeof (DataAccessConfigurationSection))]
    public sealed class DatabaseGroupCollection : GenericConfigurationCollection<DatabaseGroupNode>
    {
        protected override string ElementName
        {
            get { return "databaseGroup"; }
        }
    }

    #endregion

    #region Database GroupNode

    /// <Summary>
    ///     Database Group Node
    /// </Summary>
    public sealed class DatabaseGroupNode : ConfigurationElement, IConfigurationElementKey
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return (string) base["name"]; }
        }

        [ConfigurationProperty("connectionSelectionPolicy", DefaultValue = "TopMost")]
        public ConnectionSelectionPolicy ConnectionSelectionPolicy
        {
            get { return (ConnectionSelectionPolicy) base["connectionSelectionPolicy"]; }
        }

        [ConfigurationProperty("databases")]
        [ConfigurationCollection(typeof (DatabaseNodeCollection))]
        public DatabaseNodeCollection Databases
        {
            get { return (DatabaseNodeCollection) this["databases"]; }
        }

        #region IConfigurationElementKey Members

        public object Key
        {
            get { return Name; }
        }

        #endregion

        #region DatabaseNodeCollection

        public sealed class DatabaseNodeCollection : GenericConfigurationCollection<DatabaseNode>
        {
            protected override string ElementName
            {
                get { return "database"; }
            }
        }

        #endregion
    }

    #endregion

    #region DatabaseNode

    /// <Summary>
    ///     Configuration Database Node
    /// </Summary>
    public sealed class DatabaseNode : ConfigurationElement, IConfigurationElementKey
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return (string) base["name"]; }
        }

        [ConfigurationProperty("connectionString")]
        public string ConnectionString
        {
            get { return (string) base["connectionString"]; }
        }

        #region IConfigurationElementKey Members

        public object Key
        {
            get { return Name; }
        }

        #endregion
    }

    #endregion

    #region GenericConfigurationCollection

    /// <summary>
    ///     A Generic ConfigurationCollection to simplify use.
    /// </summary>
    public class GenericConfigurationCollection<T> : ConfigurationElementCollection
        where T : ConfigurationElement, IConfigurationElementKey, new()
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        public new T this[string name]
        {
            get { return (T) BaseGet(name); }
        }

        public T this[int index]
        {
            get { return (T) BaseGet(index); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((T) element).Key;
        }

        public void Add(T node)
        {
            BaseAdd(node);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }
    }

    #endregion

    #region ConfigNodeValue Type

    public class ConfigNodeValue<T> : ConfigurationElement
    {
        [ConfigurationProperty("value")]
        public T Value
        {
            get { return (T) this["value"]; }
        }
    }

    #endregion

    #region Interface IConfigurationElementKey

    /// <summary>
    ///     Get a key that uniquely identifies an ConfigurationELement
    /// </summary>
    public interface IConfigurationElementKey
    {
        object Key { get; }
    }

    #endregion
}