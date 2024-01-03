using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

//Singleton Settings class
namespace STP.Common.Configuration
{
    public class Settings
    {
        private Settings()
        {
            bInitialized = false;
        }

        private static readonly Settings instance = new Settings();
        private static bool bInitialized;

        public static Settings GetInstance()
        {
            lock (instance)
            {
                if (!bInitialized)
                {
                    loadConfig();
                    bInitialized = true;
                }
            }
            return instance;
        }

        private static void loadConfig()
        {
            if (ConfigurationManager.AppSettings.Get("RoutePlannerIp") != null)
            {
                instance.RoutePlannerIP = (ConfigurationManager.AppSettings.Get("RoutePlannerIp") != null) ?
                    ConfigurationManager.AppSettings.Get("RoutePlannerIp") : "127.0.0.1";

                string port = (ConfigurationManager.AppSettings.Get("RoutePlannerport") != null) ?
                     ConfigurationManager.AppSettings.Get("RoutePlannerport") : "18001";

                string timedelay = (ConfigurationManager.AppSettings.Get("RouteRequestTimeDelay") != null) ?//added by Nithin 07/07/2015
                     ConfigurationManager.AppSettings.Get("RouteRequestTimeDelay") : "100000";

                string rpClientId = (ConfigurationManager.AppSettings.Get("RPClientId") != null) ?
                     ConfigurationManager.AppSettings.Get("RPClientId") : "1001";

                try
                {
                    instance.RoutePlannerport = Convert.ToInt32(port);
                }
                catch (FormatException)
                {
                    instance.RoutePlannerport = 18001;
                }

                try//Added by Nithin 07/07/2015
                {
                    instance.RouteRequestTimeDelay = Convert.ToInt32(timedelay);
                }
                catch (FormatException)
                {
                    instance.RouteRequestTimeDelay = 100000;
                }

                try
                {
                    instance.RPClientId = Convert.ToUInt16(rpClientId);
                }
                catch (FormatException)
                {
                    instance.RPClientId = 1001;
                }
                instance.UseExtendedMessage = GetConfigValueInt("UseExtendeRouteRequest", 0) != 0;
            }
            if (ConfigurationManager.AppSettings.Get("RouteInstructorIp") != null)
            {
                instance.RouteInstructorIP = GetConfigValueString("RouteInstructorIp", "127.0.0.1");
                instance.RouteInstructorport = GetConfigValueInt("RouteInstructorPort", 19000);
            }
        }

        #region RoutePlanner Configurations
        /// <summary>
        /// RoutePlanner Parameters
        /// </summary>
        public string RoutePlannerIP { get; set; }
        public int RoutePlannerport { get; set; }
        public int RouteRequestTimeDelay { get; set; }
        public UInt16 RPClientId { get; set; }
        public bool UseExtendedMessage { get; set; }
        #endregion

        #region Route Instructor Configuration
        public string RouteInstructorIP { get; set; }
        public int RouteInstructorport { get; set; }
        #endregion

        public string GetRoutePlannerIP()
        {
            return RoutePlannerIP;
        }

        public int GetRoutePlannerPort()
        {
            return RoutePlannerport;
        }


        // function to get value of RouteRequestTimeDelay
        //
        public int GetRouteRequestTimeDelay()
        {
            return RouteRequestTimeDelay;
        }

        //Function to get string value from configuration
        //@param keyname - the key name of the configuration
        //@paraam defaultvalue - default value to be returned in 
        //case key value pair is not found
        public static string GetConfigValueString(string keyname, string defaultvalue)
        {
            string value = (ConfigurationManager.AppSettings.Get(keyname) != null) ?
                     ConfigurationManager.AppSettings.Get(keyname) : defaultvalue;
            return value;
        }

        //Function to get int value from configuration
        //@param keyname - the key name of the configuration
        //@paraam defaultvalue - default value to be returned in 
        //case key value pair is not found or value is not integer
        public static int GetConfigValueInt(string keyname, int defaultvalue)
        {
            string strvalue = ConfigurationManager.AppSettings.Get(keyname);
            int nValue;
            try
            {
                nValue = (strvalue != null) ? Convert.ToInt32(strvalue) : defaultvalue;
            }
            catch (FormatException)
            {
                nValue = defaultvalue;
            }
            return nValue;
        }
    }
}