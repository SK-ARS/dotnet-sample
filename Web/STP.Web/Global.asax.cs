using STP.Common.Logger;
using STP.Domain.VehicleAndFleets.Component;
using STP.ServiceAccess;
using STP.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace STP.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityMvcActivator.Start();

            FilterProviders.Providers.Add(new AntiForgeryTokenValidate());

            ComponentConfiguration compConfig = new ComponentConfiguration();
            if (compConfig.LoadVehicleConfiguration())
            {
                Application["VehicleComponents"] = compConfig;
                //Application["Vehicle"] = compConfig;
            }
        }
        protected void Application_BeginRequest()
        {
            Response.Cache.SetNoStore();
        }

        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            string authcookie = FormsAuthentication.FormsCookieName;
            var RequestCookies = Request.Cookies;

            for (int i = 0; i < RequestCookies.Count; i++)
            {
                string sCookie = RequestCookies.GetKey(i);

                if (sCookie.Equals(authcookie)/* || sCookie.Equals("__RequestVerificationToken")*/)
                {
                    // Set the cookie to be httponly and secure.
                    // Browsers will send the cookie only to pages requested with https.
                    var httpCookie = Response.Cookies[sCookie];
                    if (httpCookie != null)
                    {
                        httpCookie.HttpOnly = true;
                        httpCookie.Secure = true;
                        httpCookie.SameSite = SameSiteMode.Lax;
                    }
                }
            }

            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            // do other error handling here as you need to
            Exception ex = Server.GetLastError();
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Application_Error {0} --- Exception {1} ", Context.Request.RawUrl, ex.Message.ToString()));
            if (ex is HttpAntiForgeryException)
            {
                Response.Clear();
                Server.ClearError(); //make sure you log the exception first
                Session.RemoveAll();
                Session.Abandon();
                Session.Clear();
                Response.Cookies.Clear();
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.MinValue;
                Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", "")
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.MinValue
                });
                FormsAuthentication.SignOut();
                Server.TransferRequest("/Account/RedirectToLogin", true);
            }
        }

        protected void Application_AcquireRequestState(Object sender, EventArgs e)
        {
            string _encryptedString = string.Empty;
            string _sessionIPAddress = string.Empty;
            string _sessionBrowserInfo = string.Empty;

            if (HttpContext.Current.Session != null)
            {
                _encryptedString = Convert.ToString(Session["encryptedSession"]);
                byte[] _encodedAsBytes = System.Convert.FromBase64String(_encryptedString);
                string _decrytedString = System.Text.ASCIIEncoding.ASCII.GetString(_encodedAsBytes);

                char[] _separator = new char[] { '^' };
                if (_decrytedString != string.Empty && _decrytedString != "" && _decrytedString != null)
                {
                    string[] _splitStrings = _decrytedString.Split(_separator);
                    if (_splitStrings.Count() > 0 && _splitStrings[2].Count() > 0)
                    {
                        string[] _userBrowserInfo = _splitStrings[2].Split('~');
                        if (_userBrowserInfo.Count() > 0)
                        {
                            _sessionBrowserInfo = _userBrowserInfo[0];
                            _sessionIPAddress = _userBrowserInfo[1];
                        }
                    }
                }
                
                string _currentBrowserInfo = Request.Browser.Browser +
                                    Request.Browser.Version +
                                    Request.UserAgent;

                if (_sessionBrowserInfo != string.Empty && _sessionBrowserInfo != "")
                {
                    if (_sessionBrowserInfo != _currentBrowserInfo)
                    {
                        Session.RemoveAll();
                        Session.Clear();
                        Session.Abandon();
                        Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.MinValue;
                        Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                        Response.Redirect("/Account/Logout");
                    }
                    else
                    {
                        string _currentUserIPAddress;
                        //if (string.IsNullOrEmpty(Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                        //{
                            _currentUserIPAddress = Request.ServerVariables["REMOTE_ADDR"];
                        //}
                        //else
                        //{
                        //    _currentUserIPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(",".ToCharArray(),
                        //                                                                             StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                        //}

                        System.Net.IPAddress result;
                        if (!System.Net.IPAddress.TryParse(_currentUserIPAddress, out result))
                        {
                            result = System.Net.IPAddress.None;
                        }

                        if (_sessionIPAddress != string.Empty && _sessionIPAddress != "")
                        {
                            if (_sessionIPAddress != _currentUserIPAddress)
                            {
                                Session.RemoveAll();
                                Session.Clear();
                                Session.Abandon();
                                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.MinValue;
                                Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                                Response.Redirect("/Account/Logout");
                            }
                            else
                            {
                                //valid user
                            }
                        }
                    }
                } 
            }
        }
    }
}
