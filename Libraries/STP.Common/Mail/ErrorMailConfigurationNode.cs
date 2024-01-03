#region

using System.Configuration;

#endregion

namespace STP.Common.Mail
{
    public class ErrorMailConfigurationNode : ConfigurationElement
    {
        [ConfigurationProperty("enabled")]
        public bool Enabled
        {
            get { return (bool) this["enabled"]; }
        }

        [ConfigurationProperty("senderAddress")]
        public string SenderAddress
        {
            get { return (string) this["senderAddress"]; }
        }

        [ConfigurationProperty("recipientAddress")]
        public string RecipientAddress
        {
            get { return (string) this["recipientAddress"]; }
        }
    }
}