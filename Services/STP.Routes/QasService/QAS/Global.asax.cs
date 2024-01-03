using STP.Common.Logger;
using STP.Routes.QasService.QAS.com.qas.proweb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace STP.Routes.QasService.QAS.com.qas.prowebintegration
{
	public class Global : System.Web.HttpApplication
	{
		public Global()
		{
			InitializeComponent();
		}
		public static QuickAddress NewQuickAddress()
		{
			try
			{
				// Retrieve server URL from web.config
				string sServerURL = System.Configuration.ConfigurationManager.AppSettings["OnDemandUrl"];
				// Retrieve Username from web.config
				string sUsername = System.Configuration.ConfigurationManager.AppSettings["OnDemandUsername"];
				// Retrieve Password from web.config
				string sPassword = System.Configuration.ConfigurationManager.AppSettings["OnDemandPassword"];
				// Retrieve proxy address Value from web.config
				string sProxyAddress = null; 
				// Retrieve proxy username Value from web.config
				string sProxyUsername = null;
				// Retrieve proxy password Value from web.config
				string sProxyPassword = null; 
				// Create QuickAddress search object
				if (String.IsNullOrEmpty(sProxyAddress) != true)
				{
					IWebProxy proxy = new WebProxy(sProxyAddress, true);
					NetworkCredential credentials = new NetworkCredential(sProxyUsername, sProxyPassword);
					proxy.Credentials = credentials;
					// Create QuickAddress search object with proxy server
					return new QuickAddress(sServerURL, sUsername, sPassword, proxy);
				}
				return new QuickAddress(sServerURL, sUsername, sPassword);
			}
			catch (Exception ex)
			{
				Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"New Quick Address,  Exception: {ex}");
				return null;
			}
		}
		#region Web Form Designer generated code
		private void InitializeComponent()
        {
            // Method intentionally left empty.
        }
        #endregion
    }
}