using STP.Routes.QasService.QAS.com.qas.proweb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.QasService.QAS.com.qas.prowebintegration
{
    public class HierBasePage : System.Web.UI.Page
    {
        protected HierBasePage StoredPage = null;
        public enum StepinWarnings
        {
            None,
            CloseMatches,
            CrossBorder,
            PostcodeRecode
        };
        private const string PAGE_BEGIN = "HierInput.aspx";
        private const string PAGE_SEARCH = "HierSearch.aspx";
        private const string PAGE_FORMAT = "HierAddress.aspx";
        virtual protected void Page_BaseLoad(object sender, System.EventArgs e)
        {
            if (!IsPostBack && Context.Handler is HierBasePage)
            {
                StoredPage = Context.Handler as HierBasePage;
            }
            else
            {
                StoredPage = this;
            }
        }
        protected string GetLayout()
        {
            string sLayout;
            string sDataID = StoredDataID;
            sLayout = System.Configuration.ConfigurationManager.AppSettings[Constants.KEY_LAYOUT + "." + sDataID];
            if (sLayout == null || sLayout == "")
            {
                sLayout = System.Configuration.ConfigurationManager.AppSettings[Constants.KEY_LAYOUT];
            }
            return sLayout;
        }
        protected void GoFirstPage()
        {
            Server.Transfer(PAGE_BEGIN);
        }
        protected void GoSearchPage()
        {
            Server.Transfer(PAGE_SEARCH);
        }
        protected void GoFormatPage(string sMoniker, StepinWarnings eWarn)
        {
            StoredPage.StoredRoute = Constants.Routes.Okay;
            StoredPage.StoredMoniker = sMoniker;
            StoredPage.StoredWarning = eWarn;
            Server.Transfer(PAGE_FORMAT);
        }
        protected void GoErrorPage(Constants.Routes route)
        {
            StoredPage.StoredRoute = route;
            Server.Transfer(PAGE_FORMAT);
        }
        protected void GoErrorPage(Constants.Routes route, string sMessage)
        {
            StoredPage.StoredRoute = route;
            StoredPage.StoredErrorInfo = sMessage;
            Server.Transfer(PAGE_FORMAT);
        }
        protected void GoErrorPage(Exception x)
        {
            StoredPage.StoredRoute = Constants.Routes.Failed;
            StoredPage.StoredErrorInfo = x.Message;
            Server.Transfer(PAGE_FORMAT);
        }
        protected void GoFinalPage()
        {
            Server.Transfer(Constants.PAGE_FINAL_ADDRESS);
        }
        public string StoredDataID
        {
            get
            {
                return (string)ViewState[Constants.FIELD_DATA_ID];
            }
            set
            {
                ViewState[Constants.FIELD_DATA_ID] = value;
            }
        }
        public string StoredCountryName
        {
            get
            {
                return (string)ViewState[Constants.FIELD_COUNTRY_NAME];
            }
            set
            {
                ViewState[Constants.FIELD_COUNTRY_NAME] = value;
            }
        }
        public string StoredUserInput
        {
            get
            {
                return (string)ViewState[Constants.FIELD_INPUT_LINES];
            }
            set
            {
                ViewState[Constants.FIELD_INPUT_LINES] = value;
            }
        }
        public Constants.Routes StoredRoute
        {
            get
            {
                object objValue = ViewState[Constants.FIELD_ROUTE];
                return (objValue != null) ? (Constants.Routes)objValue : Constants.Routes.Okay;
            }
            set
            {
                ViewState[Constants.FIELD_ROUTE] = value;
            }
        }
        public string StoredMoniker
        {
            get
            {
                return (string)ViewState[Constants.FIELD_MONIKER];
            }
            set
            {
                ViewState[Constants.FIELD_MONIKER] = value;
            }
        }
        public StepinWarnings StoredWarning
        {
            get
            {
                object objValue = ViewState["Warning"];
                return (objValue != null) ? (StepinWarnings)objValue : StepinWarnings.None;
            }
            set
            {
                ViewState["Warning"] = value;
            }
        }
        public HistoryStack GetStoredHistory()
        {
            object objValue = ViewState["History"];
            if (objValue is ArrayList list)
            {
                HistoryStack stack = new HistoryStack(list);
                return stack;
            }
            return new HistoryStack();
        }
        public void SetStoredHistory(HistoryStack vValue)
        {
            ViewState["History"] = vValue;
        }
        public string StoredErrorInfo
        {
            get
            {
                return (string)ViewState[Constants.FIELD_ERROR_INFO];
            }
            set
            {
                ViewState[Constants.FIELD_ERROR_INFO] = value;
            }
        }
        protected string GetRequestTag()
        {
            return Request.Form["RequestTag"];
        }
        [Serializable()]
        public class HistoryStack : ArrayList
        {
            public HistoryStack()
            {
            }
            public HistoryStack(ArrayList vValue)
            {
                foreach (object obj in vValue)
                {
                    Add((HistoryItem)obj);
                }
            }
            public HistoryItem Peek()
            {
                return this[Count - 1];
            }
            public HistoryItem Pop()
            {
                HistoryItem tail = Peek();
                RemoveAt(Count - 1);
                return tail;
            }
            public void Add(HistoryItem item)
            {
                if (Count == 0 || !Peek().Moniker.Equals(item.Moniker))
                {
                    base.Add(item);
                }
            }
            public void Push(string sMoniker, string sText, string sPostcode, string sScore)
            {
                HistoryItem item = new HistoryItem(sMoniker, sText, sPostcode, sScore);
                Add(item);
            }
            public void Push(PicklistItem item)
            {
                Push(item.Moniker, item.Text, item.Postcode, item.ScoreAsString);
            }
            public new HistoryItem this[int iIndex]
            {
                get
                {
                    return (HistoryItem)base[iIndex];
                }
                set
                {
                    base[iIndex] = value;
                }
            }
        }
        [Serializable()]
        public class HistoryItem
        {
            public string Moniker { get; set; }
            public string Text { get; set; }
            public string Postcode { get; set; }
            public string Score { get; set; }
            public HistoryItem(string sMonikerIn, string sTextIn, string sPostcodeIn, string sScoreIn)
            {
                Moniker = sMonikerIn;
                Text = sTextIn;
                Postcode = sPostcodeIn;
                Score = sScoreIn;
            }
        }
    }
}