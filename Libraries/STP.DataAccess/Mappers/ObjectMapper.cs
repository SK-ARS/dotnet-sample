#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using STP.Common;
using STP.Common.Log;

#endregion

namespace STP.DataAccess.Mappers
{
    public sealed class ObjectMapper
    {
        #region Private Intialization

        private static readonly IDictionary<string, PropertyInfo[]> PropertiesCache =
            new Dictionary<string, PropertyInfo[]>();

        // Help with locking
        private static readonly ReaderWriterLockSlim PropertiesCacheLock = new ReaderWriterLockSlim();

        #endregion

        #region Public Methods

        /// <summary>
        ///     Return the current row in the reader as an object
        /// </summary>
        /// <param name="reader"> The Reader </param>
        /// <param name="objectToReturnType"> The type of object to return </param>
        /// <returns> Object </returns>
        public static Object GetObject(IDataReader reader, Type objectToReturnType)
        {
            // Create a new Object
            Object newObjectToReturn = Activator.CreateInstance(objectToReturnType);

            try
            {
                // Get all the properties in our Object
                PropertyInfo[] props = objectToReturnType.GetProperties();
                // For each property get the data from the reader to the object
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        foreach (PropertyInfo t in props)
                        {
                            if (ColumnExists(reader, t.Name))
                            {
                                if (reader[t.Name] != DBNull.Value)
                                {
                                    switch (t.PropertyType.Name.ToLower())
                                    {
                                        case "xmlelement":

                                            objectToReturnType.InvokeMember(t.Name, BindingFlags.SetProperty, null,
                                                newObjectToReturn,
                                                new[]
                                                {
                                                    GetXMLElement(
                                                        reader[t.Name].ToString())
                                                });
                                            break;

                                        default:
                                            objectToReturnType.InvokeMember(t.Name, BindingFlags.SetProperty, null,
                                                newObjectToReturn, new[] {reader[t.Name]});
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                STPConfigurationManager.LogProvider.Log(new ApplicationException(
                    string.Format("Load Failure of the Attribute ({0}) for {1}. Exception:{2}",
                        ((XmlElementAttribute) newObjectToReturn).ElementName,
                        newObjectToReturn,
                        exception.Message)));
            }
            return newObjectToReturn;
        }

        /// <summary>
        ///     Gets a collection of Objects from IDataReader
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="reader"> </param>
        /// <returns> </returns>
        public static List<T> GetList<T>(IDataReader reader)
        {
            // For each property get the data from the reader to the object
            var tList = new List<T>();
            if (reader != null)
            {
                while (reader.Read())
                {
                    // Create a new Object
                    var newObjectToReturn = Activator.CreateInstance<T>();

                    try
                    {
                        // Get all the properties in our Object
                        IEnumerable<PropertyInfo> props = GetCachedProperties<T>();
                        foreach (PropertyInfo t in props)
                        {
                            List<string> columnList = GetColumnList(reader);
                            if (columnList.Contains(t.Name.ToLower()) && reader[t.Name] != DBNull.Value)
                            {
                                switch (t.PropertyType.Name.ToLower())
                                {
                                    case "xmlelement":

                                        typeof (T).InvokeMember(t.Name, BindingFlags.SetProperty, null,
                                            newObjectToReturn,
                                            new[] {GetXMLElement(reader[t.Name].ToString())});
                                        break;

                                    default:
                                        typeof (T).InvokeMember(t.Name, BindingFlags.SetProperty, null,
                                            newObjectToReturn, new[] {reader[t.Name]});
                                        break;
                                }
                            }
                        }
                        tList.Add(newObjectToReturn);
                    }
                    catch (Exception exception)
                    {
                        STPConfigurationManager.LogProvider.Log(new ApplicationException(
                            string.Format(
                                "Load Failure of the Attribute ({0}) for {1}. Exception:{2}",
                                typeof (T).Name, typeof (T).FullName, exception.Message)));
                    }
                }
            }
            return tList;
        }

        #endregion

        #region Helper Mehods

        /// <summary>
        ///     Get an array of PropertyInfo for this type
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <returns> PropertyInfo[] for this type </returns>
        private static IEnumerable<PropertyInfo> GetCachedProperties<T>()
        {
            if (PropertiesCacheLock.TryEnterUpgradeableReadLock(100))
            {
                var props = new PropertyInfo[] {};
                try
                {
                    var fullName = typeof (T).FullName;
                    if (fullName != null && !PropertiesCache.TryGetValue(fullName, out props))
                    {
                        props = typeof (T).GetProperties();
                        if (PropertiesCacheLock.TryEnterWriteLock(100))
                        {
                            try
                            {
                                var name = typeof (T).FullName;
                                if (name != null) PropertiesCache.Add(name, props);
                            }
                            finally
                            {
                                PropertiesCacheLock.ExitWriteLock();
                            }
                        }
                    }
                }
                finally
                {
                    PropertiesCacheLock.ExitUpgradeableReadLock();
                }
                return props;
            }
            return typeof (T).GetProperties();
        }

        /// <summary>
        ///     Gets the list of Columns from a Row
        /// </summary>
        /// <param name="reader"> </param>
        /// <returns> </returns>
        private static List<string> GetColumnList(IDataReader reader)
        {
            var columnList = new List<string>();
            DataTable readerSchema = reader.GetSchemaTable();
            for (int i = 0; i < readerSchema.Rows.Count; i++)
                columnList.Add(readerSchema.Rows[i]["ColumnName"].ToString().ToLower());
            return columnList;
        }


        /// <summary>
        ///     Check if an SqlDataReader contains a field
        /// </summary>
        /// <param name="reader"> The reader </param>
        /// <param name="columnName"> The column name </param>
        /// <returns> </returns>
        private static bool ColumnExists(IDataReader reader, string columnName)
        {
            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName= '" + columnName + "'";
            return (reader.GetSchemaTable().DefaultView.Count > 0);
        }

        /// <summary>
        ///     Gets the RootElement of the XmlDocument
        /// </summary>
        /// <param name="name"> </param>
        /// <returns> </returns>
        private static XmlElement GetXMLElement(string name)
        {
            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(name);
                XmlElement xmlElement = xmlDocument.DocumentElement;
                return xmlElement;
            }
            catch (Exception exception)
            {
                STPConfigurationManager.LogProvider.Log(exception);
            }
            return null;
        }

        #endregion
    }
}