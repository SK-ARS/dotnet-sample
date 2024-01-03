#region

using System.Configuration;

#endregion

namespace STP.Common.Mail
{
    /// <summary>
    ///     Contains the properties required to connect to a SmtpServer
    /// </summary>
    public class SmtpServerConfigurationNode : ConfigurationElement
    {
        [ConfigurationProperty("host")]
        public string Host
        {
            get { return (string) this["host"]; }
        }

        [ConfigurationProperty("port")]
        public int Port
        {
            get { return (int) this["port"]; }
        }

        [ConfigurationProperty("enableSSL")]
        public bool EnableSSL
        {
            get
            {
                if (this["enableSSL"] != null)
                {
                    bool result;
                    result = bool.TryParse(this["enableSSL"].ToString(), out result) ? result : false;
                    return result;
                }
                return false;
            }
        }
    }
}