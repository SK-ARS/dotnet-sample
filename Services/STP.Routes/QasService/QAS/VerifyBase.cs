using STP.Routes.QasService.QAS.com.qas.proweb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace STP.Routes.QasService.QAS.com.qas.prowebintegration
{
    public class VerifyBase : System.Web.UI.Page
    {
        private QuickAddress m_searchService = null;
        private string[] m_asInputAddress;
        private const string PAGE_REFINE = "VerifyRefine.aspx";
        protected QuickAddress theQuickAddress
        {
            get
            {
                if (m_searchService == null)
                {
                    string sServerURL = System.Configuration.ConfigurationManager.AppSettings["OnDemandUrl"];
                    string sUsername = System.Configuration.ConfigurationManager.AppSettings["OnDemandUsername"];
                    string sPassword = System.Configuration.ConfigurationManager.AppSettings["OnDemandPassword"];
                    string sProxyAddress = null;
                    string sProxyUsername = null;
                    string sProxyPassword = null;
                    if (String.IsNullOrEmpty(sProxyAddress) != true)
                    {
                        IWebProxy proxy = new WebProxy(sProxyAddress, true);
                        NetworkCredential credentials = new NetworkCredential(sProxyUsername, sProxyPassword);
                        proxy.Credentials = credentials;
                        m_searchService = new QuickAddress(sServerURL, sUsername, sPassword, proxy);
                    }
                    else
                    {
                        m_searchService = new QuickAddress(sServerURL, sUsername, sPassword);
                    }
                }
                return m_searchService;
            }
        }
        protected string GetLayout()
        {
            string sLayout = "";
            string sDataID = Request[Constants.FIELD_DATA_ID];

            if (sDataID != null && sDataID != "")
            {
                sLayout = System.Configuration.ConfigurationManager.AppSettings[Constants.KEY_LAYOUT + "." + sDataID];
                if (sLayout == null || sLayout == "")
                {
                    sLayout = System.Configuration.ConfigurationManager.AppSettings[Constants.KEY_LAYOUT];
                }
            }
            return sLayout;
        }
        protected void GoFinalPage(string sMoniker)
        {
            FormatAddress(sMoniker);
            Server.Transfer(Constants.PAGE_FINAL_ADDRESS);
        }
        protected void GoFinalPage()
        {
            Server.Transfer(Constants.PAGE_FINAL_ADDRESS);
        }
        protected void GoRefinePage()
        {
            Server.Transfer(PAGE_REFINE);
        }
        protected void GoErrorPage(Exception x)
        {
            SetAddressResult(GetInputAddress);
            SetAddressInfo("address verification " + Constants.Routes.Failed + ", so the entered address has been used");
            SetErrorInfo(x.Message);
            Server.Transfer(Constants.PAGE_FINAL_ADDRESS);
        }
        protected void GoErrorPage(Constants.Routes route, string sReason)
        {
            SetAddressResult(GetInputAddress);
            SetAddressInfo("address verification " + route + ", so the entered address has been used");
            SetErrorInfo(sReason);
            Server.Transfer(Constants.PAGE_FINAL_ADDRESS);
        }
        protected void FormatAddress(string sMoniker)
        {
            try
            {
                FormattedAddress tAddressResult = theQuickAddress.GetFormattedAddress(sMoniker, GetLayout());
                SetDPVInfo((Constants.DPVStatus)Enum.Parse(typeof(Constants.DPVStatus), tAddressResult.DPVStatus.ToString()));
                SetAddressResult(tAddressResult);
            }
            catch (Exception x)
            {
                SetAddressResult(GetInputAddress);
                SetAddressInfo("address verification is not available, so the entered address has been used");
                SetErrorInfo(x.Message);
            }
        }
        protected void SetAddressResult(FormattedAddress tAddressResult)
        {
            Request.Cookies.Remove(Constants.FIELD_ADDRESS_LINES);
            foreach (AddressLine tLine in tAddressResult.AddressLines)
            {
                Request.Cookies.Add(new HttpCookie(Constants.FIELD_ADDRESS_LINES, tLine.Line));
            }
            AddAddressWarnings(tAddressResult);
        }
        protected void SetAddressResult(string[] asAddress)
        {
            Request.Cookies.Remove(Constants.FIELD_ADDRESS_LINES);
            foreach (string sLine in asAddress)
            {
                Request.Cookies.Add(new HttpCookie(Constants.FIELD_ADDRESS_LINES, sLine));
            }
        }
        protected string GetCountry()
        {
            return Request[Constants.FIELD_COUNTRY_NAME];
        }
        protected void SetCountry(string sCountry)
        {
            Request.Cookies.Set(new HttpCookie(Constants.FIELD_COUNTRY_NAME, sCountry));
        }
        protected void SetErrorInfo(string sErrorInfo)
        {
            Request.Cookies.Set(new HttpCookie(Constants.FIELD_ERROR_INFO, sErrorInfo));
        }
        protected string[] GetInputAddress
        {
            get
            {
                if (m_asInputAddress == null)
                {
                    m_asInputAddress = Request.Form.GetValues(Constants.FIELD_INPUT_LINES);
                }
                return m_asInputAddress;
            }
        }
        protected string GetMoniker()
        {
            return Request[Constants.FIELD_MONIKER];
        }
        protected string GetAddressInfo()
        {
            return Request[Constants.FIELD_ADDRESS_INFO];
        }
        protected string GetAddressInfoHTML()
        {
            return GetAddressInfo().Replace("\n", "<br />");
        }
        protected void SetAddressInfo(string sAddressInfo)
        {
            Request.Cookies.Set(new HttpCookie(Constants.FIELD_ADDRESS_INFO, sAddressInfo));
        }
        protected void SetDPVInfo(Constants.DPVStatus status)
        {
            string sDPVStatus = null;
            switch (status)
            {
                case Constants.DPVStatus.DPVConfirmed:
                    sDPVStatus = "DPV validated";
                    break;
                case Constants.DPVStatus.DPVNotConfirmed:
                    sDPVStatus = "WARNING - DPV not validated";
                    break;
                case Constants.DPVStatus.DPVConfirmedMissingSec:
                    sDPVStatus = "DPV validated but secondary number incorrect or missing";
                    break;
                case Constants.DPVStatus.DPVLocked:
                    sDPVStatus = "WARNING - DPV validation locked";
                    break;
                case Constants.DPVStatus.DPVSeedHit:
                    sDPVStatus = "WARNING - DPV - Seed address hit";
                    break;
                default:
                    sDPVStatus = "";
                    break;
            }
            Request.Cookies.Set(new HttpCookie(Constants.FIELD_DPVSTATUS, sDPVStatus));
        }
        protected void AddAddressWarnings(FormattedAddress tAddressResult)
        {
            if (tAddressResult.IsOverflow)
            {
                SetAddressInfo(GetAddressInfo() + "\nWarning: Address has overflowed the layout &#8211; elements lost");
            }
            if (tAddressResult.IsTruncated)
            {
                SetAddressInfo(GetAddressInfo() + "\nWarning: Address elements have been truncated");
            }
        }
        protected string GetRequestTag()
        {
            return Request.Form["RequestTag"];
        }
    }
}