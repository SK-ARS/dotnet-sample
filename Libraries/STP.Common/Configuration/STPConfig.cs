#region

using System.Configuration;
using System.Web.Hosting;
using StringValidator = STP.Common.Validation.StringValidator;

#endregion

namespace STP.Common.Configuration
{
    public class STPConfig
    {
        private static STPConfig instance;
        private readonly STPConfigurationSection _STPConfigurationSection;


        /// <summary>
        ///     Only allow instantiation locally
        /// </summary>
        private STPConfig()
        {
            _STPConfigurationSection =
                ConfigurationManager.GetSection("STPConfigurationSection") as STPConfigurationSection;
        }

        public int ApplicationId
        {
            get { return _STPConfigurationSection.AppId.Value; }
        }

        public string SiteName
        {
            get { return _STPConfigurationSection.SiteName.Value; }
        }


        /// <summary>
        ///     This sets the default culture for the website. Culture is used to control the formatting of dates and other values.
        /// </summary>
        public string DefaultCulture
        {
            get { return _STPConfigurationSection.DefaultCulture.Value; }
        }

        public bool EnableSSL
        {
            get { return _STPConfigurationSection.EnableSSL.Value; }
        }

        public string LogDirectoryRepository
        {
            get
            {
                string logPath;
                if (ConfigurationManager.AppSettings["Envrironment"] == "Debug")
                    logPath = HostingEnvironment.ApplicationPhysicalPath;
                else
                    logPath = ConfigurationManager.AppSettings["ServerLogPhysicalPath"];
                return logPath + _STPConfigurationSection.LogDirectoryRepository.Value;
            }
        }

        public long MaxLogFileSize
        {
            get
            {
                long maxLogFileSize;
                long.TryParse(_STPConfigurationSection.MaxLogFileSize.Value, out maxLogFileSize);
                if (maxLogFileSize == 0)
                {
                    maxLogFileSize = 4096;
                }
                return maxLogFileSize;
            }
        }

        public bool EnableLog
        {
            get
            {
                bool enableLog;
                bool.TryParse(_STPConfigurationSection.EnableLog.Value, out enableLog);
                return enableLog;
            }
        }

        public bool IsDebugEnabled
        {
            get
            {
                bool isDebugEnabled;
                bool.TryParse(_STPConfigurationSection.IsDebugEnabled.Value, out isDebugEnabled);
                return isDebugEnabled;
            }
        }

        public static STPConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new STPConfig();
                    instance.Validate();
                }
                return instance;
            }
        }

        /// <summary>
        ///     This retrieves the default number of search results displayed by the pagination control
        /// </summary>
        public int DefaultRecordsPerPage
        {
            get { return _STPConfigurationSection.DefaultRecordsPerPage.Value; }
        }

        /// <summary>
        ///     Validates the config file
        /// </summary>
        /// <returns></returns>
        private void Validate()
        {
            //IdValidator.Validate(ApplicationId);
            //IdValidator.ValidateNotNull(ApplicationId);
            //StringValidator.ValidateNotNull(DefaultCulture);
        }
    }
}