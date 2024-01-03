#region

using STP.Common.Configuration;
using STP.Common.Configuration.Interface;
using STP.Common.Configuration.Provider;
using STP.Common.Log;

#endregion

namespace STP.Common
{
    public sealed class STPConfigurationManager
    {
        #region Nested type: ConfigProvider

        /// <summary>
        ///     Provides all the methods available from Custome Configurations
        ///     Please contact pnayak@euronetworldwide.com for any queries
        /// </summary>
        public class ConfigProvider
        {
            /// <summary>
            ///     Gets All methods for PageSecurityConfigProvider
            /// </summary>
            public static IPageSecurityConfigProvider PageSecurityConfigProvider
            {
                get { return Configuration.Provider.PageSecurityConfigProvider.Instance; }
            }

            /// <summary>
            ///     Gets All methods for DatabaseConnection Provider
            /// </summary>
            public static IDatabaseConfigProvider DatabaseConfigProvider
            {
                get { return Configuration.Provider.DatabaseConfigProvider.Instance; }
            }


            /// <summary>
            ///     Gets All methods for CommonConfigProvider, it gets the Settings from
            ///     Apps\Web\STP\Configuration\FileUploadSettings.config file
            /// </summary>
            public static ICommonConfigProvider CommonConfigProvider
            {
                get { return Configuration.Provider.CommonConfigProvider.Instance; }
            }

            /// <summary>
            ///     Gets All methods for CommonConfigProvider, it gets the Settings from
            ///     Apps\Web\STP\Configuration\FileUploadSettings.config file
            /// </summary>
            public static ICommonSettingsProvider CommonSettingsProvider
            {
                get { return Configuration.Provider.CommonSettingsProvider.Instance; }
            }

            /// <summary>
            ///     Gets All methods for JQueryConfigProvider, it gets the Settings from Apps\Web\STP\Configuration\JQuery.config file
            /// </summary>
            public static IJQueryConfigProvider JQueryConfigProvider
            {
                get { return Configuration.Provider.JQueryConfigProvider.Instance; }
            }

            /// <summary>
            ///     Script Provider for all types of Framework and Pluggins to load in Header and Body of HTML
            ///     on runtime
            /// </summary>
            public static IFrameworkConfigProvider FrameworkProvider
            {
                get { return FrameworkConfigProvider.Instance; }
            }

            /// <summary>
            ///     Gets All methods for ScriptConfigProvider, it gets the Settings from Apps\Web\STP\Configuration\Script.config file
            /// </summary>
            public static IScriptConfigProvider ScriptConfigProvider
            {
                get { return Configuration.Provider.ScriptConfigProvider.Instance; }
            }

            /// <summary>
            ///     Gets All methods for EmailConfigProvider, it gets the Settings from Apps\Web\STP\Configuration\EmailConfig.config
            ///     file
            /// </summary>
            public static IEmailConfigProvider EmailConfigProvider
            {
                get { return Configuration.Provider.EmailConfigProvider.Instance; }
            }

            #region Nested type: ServiceProvider

            public sealed class ServiceProvider
            {
                /// <summary>
                ///     Build Provider
                /// </summary>
                public static IBuild BuildProvider
                {
                    get { return Build.Instance; }
                }
            }

            #endregion

            #region Nested type: RouteProvider

            public sealed class Routes
            {
                /// <summary>
                ///     Script Provider for all types of Common scripts to load in Body of HTML
                ///     on runtime
                /// </summary>
                public static IRouteConfigProvider RouteProvider
                {
                    get { return RouteConfigProvider.Instance; }
                }
            }

            #endregion
        }

        #endregion

        #region Logmanager

        /// <summary>
        ///     LogManager
        /// </summary>
        public static ILogWrapper LogProvider
        {
            get { return LogWrapper.Instance; }
        }

        #endregion

    }


}