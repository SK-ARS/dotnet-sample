using STP.Domain.RouteAssessment.XmlAnalysedStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace STP.Domain.Custom
{
    public class XmlAffectedStructuresDeserializer
    {
        #region xmlAffectedStructuresDeserializer(string xml)
        /// <summary>
        /// Function to deserialize the Affected structures xml Data
        /// </summary>
        public static AnalysedStructures XmlAffectedStructuresDeserialize(string xml)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(AnalysedStructures));

                StringReader stringReader = new StringReader(xml);

                XmlTextReader xmlReader = new XmlTextReader(stringReader);

                object obj = deserializer.Deserialize(xmlReader);

                AnalysedStructures XmlData = (AnalysedStructures)obj;

                xmlReader.Close();

                return XmlData;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

    }

    public class XmlDeserializerGeneric<T> where T : class, new()
    {
        #region XmlDeserializer(string xml)
        /// <summary>
        /// Function to deserialize the Affected structures xml Data
        /// </summary>
        public static T XmlDeserialize(string xml)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(T));

                StringReader stringReader = new StringReader(xml);

                XmlTextReader xmlReader = new XmlTextReader(stringReader);

                object obj = deserializer.Deserialize(xmlReader);

                T XmlData = (T)obj;

                xmlReader.Close();

                return XmlData;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

    }
}
