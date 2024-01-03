#region

using System.Configuration;

#endregion

namespace STP.Common.Configuration
{
    /// <Summary>
    ///     STP database node
    /// </Summary>
    public sealed class STPDbNode : ConfigurationElement
    {
        [ConfigurationProperty("webId")]
        public string WebId
        {
            get { return (string) base["webId"]; }
        }

        [ConfigurationProperty("connectionString")]
        public string ConnectionString
        {
            get { return (string) base["connectionString"]; }
        }
    }
}