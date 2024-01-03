using STP.Routes.QasService.QAS.com.qas.proweb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.QasService.QAS.com.qas.prowebintegration
{
    public class KeyBasePage : System.Web.UI.Page
    {
        protected const string PAGE_BEGIN = "KeyCountry.aspx";
        protected const string PAGE_INPUT = "KeyPrompt.aspx";
        protected const string PAGE_SEARCH = "KeySearch.aspx";
        protected const string PAGE_FORMAT = "KeyAddress.aspx";
        protected const string FIELD_PROMPTSET = "PromptSet";
        protected const string FIELD_PICKLIST_MONIKER = "PicklistMoniker";
        protected const string FIELD_REFINE_MONIKER = "RefineMoniker";
        public KeyBasePage()
        {
        }
        protected void GoFirstPage()
        {
            Server.Transfer(PAGE_BEGIN);
        }
        protected void GoInputPage()
        {
            Server.Transfer(PAGE_INPUT);
        }
        protected void GoSearchPage()
        {
            Server.Transfer(PAGE_SEARCH);
        }
        protected void GoFormatPage(string sMoniker)
        {
            if (sMoniker != null)
            {
                SetMoniker(sMoniker);
            }
            SetRoute(Constants.Routes.Okay);
            Server.Transfer(PAGE_FORMAT);
        }
        protected void GoErrorPage(Constants.Routes route)
        {
            SetRoute(route);
            Server.Transfer(PAGE_FORMAT);
        }
        protected void GoErrorPage(Constants.Routes route, string sMessage)
        {
            SetRoute(route);
            SetErrorInfo(sMessage);
            Server.Transfer(PAGE_FORMAT);
        }
        protected void GoErrorPage(Exception x)
        {
            SetRoute(Constants.Routes.Failed);
            SetErrorInfo(x.Message);
            Server.Transfer(PAGE_FORMAT);
        }
        protected void GoFinalPage()
        {
            Server.Transfer(Constants.PAGE_FINAL_ADDRESS);
        }
        protected void RenderRequestString(string sKey)
        {
            string sValue = Request[sKey];
            RenderHiddenField(sKey, sValue);
        }
        protected void RenderRequestArray(string sKey)
        {
            string[] asValues = Request.Params.GetValues(sKey);
            if (asValues != null)
            {
                foreach (string sValue in asValues)
                {
                    RenderHiddenField(sKey, sValue);
                }
                if (asValues.Length == 1)
                {
                    RenderHiddenField(sKey, null);
                }
            }
        }
        protected void RenderHiddenField(string sKey, string sValue)
        {
            Response.Write("<input type=\"hidden\" name=\"");
            Response.Write(sKey);
            if (sValue != null)
            {
                Response.Write("\" value=\"");
                Response.Write(HttpUtility.HtmlEncode(sValue));
            }
            Response.Write("\" />\n");
        }
        protected void RenderHiddenField(string sKey, bool bValue)
        {
            Response.Write("<input type=\"hidden\" name=\"");
            Response.Write(sKey);
            if (bValue) // Only write a value if it is True
            {
                Response.Write("\" value=\"");
                Response.Write(true.ToString());
            }
            Response.Write("\" />\n");
        }
        protected string GetDataID()
        {
            return Request[Constants.FIELD_DATA_ID];
        }
        protected string GetCountryName()
        {
            return Request[Constants.FIELD_COUNTRY_NAME];
        }
        protected string GetLayout()
        {
            string sLayout;
            string sDataID = GetDataID();
            sLayout = System.Configuration.ConfigurationManager.AppSettings[Constants.KEY_LAYOUT + "." + sDataID];
            if (String.IsNullOrEmpty(sLayout))
            {
                sLayout = System.Configuration.ConfigurationManager.AppSettings[Constants.KEY_LAYOUT];
            }
            return sLayout;
        }
        protected PromptSet.Types GetPromptSet()
        {
            string sValue = Request[FIELD_PROMPTSET];
            if (sValue != null)
            {
                return (PromptSet.Types)Enum.Parse(typeof(PromptSet.Types), sValue);
            }
            else
            {
                return PromptSet.Types.Optimal;
            }
        }
        protected void SetPromptSet(PromptSet.Types ePromptSet)
        {
            Request.Cookies.Set(new HttpCookie(FIELD_PROMPTSET, ePromptSet.ToString()));
        }
        protected string[] GetInputLines()
        {
            string[] asValues = Request.Params.GetValues(Constants.FIELD_INPUT_LINES);
            if (asValues != null)
            {
                return asValues;
            }
            else
            {
                return new string[0];
            }
        }
        protected Constants.Routes GetRoute()
        {
            string sValue = Request[Constants.FIELD_ROUTE];
            if (sValue != null)
            {
                return (Constants.Routes)Enum.Parse(typeof(Constants.Routes), sValue);
            }
            else
            {
                return Constants.Routes.Undefined;
            }
        }
        private void SetRoute(Constants.Routes eRoute)
        {
            Request.Cookies.Set(new HttpCookie(Constants.FIELD_ROUTE, eRoute.ToString()));
        }
        protected string GetErrorInfo()
        {
            return Request[Constants.FIELD_ERROR_INFO];
        }
        protected void SetErrorInfo(string sErrorInfo)
        {
            Request.Cookies.Set(new HttpCookie(Constants.FIELD_ERROR_INFO, sErrorInfo));
        }
        protected string GetMoniker()
        {
            return Request[Constants.FIELD_MONIKER];
        }
        private void SetMoniker(string sMoniker)
        {
            Request.Cookies.Set(new HttpCookie(Constants.FIELD_MONIKER, sMoniker));
        }
        protected string GetPicklistMoniker()
        {
            return Request[FIELD_PICKLIST_MONIKER];
        }
        protected void SetPicklistMoniker(string sMoniker)
        {
            Request.Cookies.Set(new HttpCookie(FIELD_PICKLIST_MONIKER, sMoniker));
        }
        protected string GetRequestTag()
        {
            return Request.Form["RequestTag"];
        }
    }
}