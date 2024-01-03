using STP.Routes.QasService.QAS.com.qas.proweb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace STP.Routes.QasService.QAS.com.qas.prowebintegration
{
    public class RapidBasePage : System.Web.UI.Page
    {
        private QuickAddress s_searchService = null;
        private RapidBasePage StoredPage = null;
        protected HistoryStack m_aHistory = null;
        protected Dataset[] m_atDatasets = null;
        protected enum Commands
        {
            StepIn,									// Step in to sub-picklist
            ForceFormat,							// Force-accept an unrecognised address
            Format,									// Format into final address
            HaltRange,								// User must enter a value within the range shown
            HaltIncomplete,							// User must enter premise details
            None									// No hyperlink action - self-explanatory informational
        };
        protected enum Types
        {
            Alias,									// Picklist item is an alias (synonym)
            Info,									// Picklist item is an informational
            InfoWarn,								// Picklist item is a warning informational
            Name,									// Picklist item is a name/person 
            NameAlias,								// Picklist item is a name alias (i.e. forename synonym)
            POBox,									// Picklist item is a PO Box grouping
            Standard								// Picklist item is standard
        };
        protected enum StepinWarnings
        {
            None,									// No warning
            CloseMatches,							// Auto-stepped past close matches
            CrossBorder,							// Stepped into cross-border match
            ForceAccept,							// Force-format step-in performed
            Info,									// Stepped into informational item (i.e. 'Click to Show All')
            Overflow,								// Address elements have overflowed the layout
            PostcodeRecode,							// Stepped into postcode recode
            Truncate,								// Address elements have been truncated by the layout
            DpvStatusConf,							// Delivery point validation Status : DPV Confirmed
            DpvStatusUnConf,						// DPV UnConfirmed
            DpvStatusConfMisSec,					// DPV Confirmed but missing secondary
            DpvLocked,								// DPV Locked
            DpvSeedHit								// DPV Seed Address was Hit
        };
        private const string PAGE_SEARCH = "RapidSearch.aspx";
        private const string PAGE_FORMAT = "RapidAddress.aspx";
        protected const string FIELD_CALLBACK = "Callback";
        private const string FIELD_ENGINE = "Engine";
        private const string FIELD_HISTORY = "History";
        private const string FIELD_WARNING = "Warning";
        private const string FIELD_DATALIST = "Datalist";
        protected const int MAX_DATAMAP_NAME_LENGTH = 26;
        protected const string SELECT_WIDTH = "16em";
        virtual protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack && (Context.Handler is RapidBasePage))
            {
                StoredPage = Context.Handler as RapidBasePage;
                StoredCallback = StoredPage.StoredCallback;
                StoredDataID = StoredPage.StoredDataID;
                StoredSearchEngine = StoredPage.StoredSearchEngine;
                StoredErrorInfo = StoredPage.StoredErrorInfo;
                StoredMoniker = StoredPage.StoredMoniker;
                StoredRoute = StoredPage.StoredRoute;
                StoredWarning = StoredPage.StoredWarning;
                StoredDataMapList = StoredPage.StoredDataMapList;
            }
            else
            {
                StoredPage = this;
            }
            m_aHistory = StoredPage.GetStoredHistory();
        }
        protected override object SaveViewState()
        {
            SetStoredHistory(m_aHistory);
            return base.SaveViewState();
        }
        protected QuickAddress theQuickAddress
        {
            get
            {
                if (s_searchService == null)
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
                        s_searchService = new QuickAddress(sServerURL, sUsername, sPassword, proxy);
                    }
                    else
                    {
                        s_searchService = new QuickAddress(sServerURL, sUsername, sPassword);
                    }
                }
                return s_searchService;
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
        protected void GoSearchPage(string sDataID, QuickAddress.EngineTypes eEngine)
        {
            StoredPage.StoredDataID = sDataID;
            StoredPage.SetStoredHistory(m_aHistory);
            StoredPage.StoredSearchEngine = eEngine;
            Server.Transfer(PAGE_SEARCH);
        }
        protected void GoFormatPage(string sDataID, QuickAddress.EngineTypes eEngine, string sMoniker, StepinWarnings eWarn)
        {
            StoredPage.StoredDataID = sDataID;
            StoredPage.StoredMoniker = sMoniker;
            StoredPage.SetStoredHistory(m_aHistory);
            StoredPage.StoredRoute = Constants.Routes.Okay;
            StoredPage.StoredSearchEngine = eEngine;
            StoredPage.StoredWarning = eWarn;
            Server.Transfer(PAGE_FORMAT);
        }
        protected void GoErrorPage(Constants.Routes route, string sReason)
        {
            StoredPage.StoredErrorInfo = sReason;
            StoredPage.StoredRoute = route;
            Server.Transfer(PAGE_FORMAT);
        }
        protected void GoErrorPage(Exception x)
        {
            StoredPage.StoredErrorInfo = x.Message;
            StoredPage.StoredRoute = Constants.Routes.Failed;
            Server.Transfer(PAGE_FORMAT);
        }
        protected void RenderPicklistData(Picklist picklist, string sDepth)
        {
            if (picklist == null)
            {
                Response.Write("var sPicklistHTML = '';\n");
                Response.Write("var asActions = new Array();");
            }
            else
            {
                StringBuilder sActions = new StringBuilder();
                Response.Write("var sPicklistHTML = \"<table class='picklist indent" + sDepth + "'>\\\n");
                for (int i = 0; i < picklist.Length; ++i)
                {
                    PicklistItem item = picklist.Items[i];
                    StepinWarnings eWarn = StepinWarnings.None;
                    if (item.IsCrossBorderMatch)
                    {
                        eWarn = StepinWarnings.CrossBorder;
                    }
                    else if (item.IsPostcodeRecoded)
                    {
                        eWarn = StepinWarnings.PostcodeRecode;
                    }
                    Commands eCmd = Commands.None;
                    if (item.CanStep)
                    {
                        eCmd = Commands.StepIn;
                    }
                    else if (item.IsFullAddress)
                    {
                        eCmd = (item.IsInformation) ? Commands.ForceFormat : Commands.Format;
                    }
                    else if (item.IsUnresolvableRange)
                    {
                        eCmd = Commands.HaltRange;
                    }
                    else if (item.IsIncompleteAddress)
                    {
                        eCmd = Commands.HaltIncomplete;
                    }
                    Types eType = Types.Standard;
                    if (item.IsInformation)
                    {
                        eType = (item.IsWarnInformation) ? Types.InfoWarn : Types.Info;
                        eWarn = StepinWarnings.Info;
                    }
                    else if (item.IsDummyPOBox)
                    {
                        eType = Types.POBox;
                    }
                    else if (item.IsName)
                    {
                        eType = (item.IsAliasMatch) ? Types.NameAlias : Types.Name;
                    }
                    else if (item.IsAliasMatch || item.IsCrossBorderMatch || item.IsPostcodeRecoded)
                    {
                        eType = Types.Alias;
                    }
                    string sClass = "stop";
                    if (eCmd == Commands.StepIn)
                    {
                        if (eType == Types.Alias)
                        {
                            sClass = "aliasStep";
                        }
                        else if (eType == Types.Info)
                        {
                            sClass = "infoStep";
                        }
                        else if (eType == Types.POBox)
                        {
                            sClass = "pobox";
                        }
                        else
                        {
                            sClass = "stepIn";
                        }
                    }
                    else if (eCmd == Commands.Format)
                    {
                        if (eType == Types.Alias)
                        {
                            sClass = "alias";
                        }
                        else if (eType == Types.Name)
                        {
                            sClass = "name";
                        }
                        else if (eType == Types.NameAlias)
                        {
                            sClass = "nameAlias";
                        }
                        else
                        {
                            sClass = "format";
                        }
                    }
                    else if ((eCmd == Commands.HaltIncomplete) || (eCmd == Commands.HaltRange))
                    {
                        sClass = "halt";
                    }
                    else if (eType == Types.Info)
                    {
                        sClass = "info";
                    }

                    if (i == 0)
                    {
                        sClass += " first";
                    }
                    string sAnchorStart = "", sAnchorEnd = "";
                    if (eCmd != Commands.None)
                    {
                        sAnchorStart = "<a href='javascript:action(" + i.ToString() + ");' "
                            + "tabindex='" + (i + 1) + "' "
                            + "title=\\\"" + JavascriptEncode(item.PartialAddress) + "\\\">";
                        sAnchorEnd = "</a>";
                    }
                    string sScore = (item.Score > 0) ? item.Score + "%" : "";
                    Response.Write("<tr>");
                    Response.Write("<td class='pickitem " + sClass + "'>" + sAnchorStart + "<div>");
                    Response.Write(JavascriptEncode(Server.HtmlEncode(item.Text)) + "</div>" + sAnchorEnd + "</td>");
                    Response.Write("<td class='postcode'>" + JavascriptEncode(Server.HtmlEncode(item.Postcode)) + "</td>");
                    Response.Write("<td class='score'>" + sScore + "</td>");
                    Response.Write("</tr>\\\n");
                    sActions.Append("'" + (eCmd != Commands.None ? eCmd.ToString() : ""));
                    switch (eCmd)
                    {
                        case Commands.StepIn:
                            sActions.Append("(\"" + item.Moniker + "\",\"" + JavascriptEncode(Server.HtmlEncode(item.Text)) + "\",");
                            sActions.Append("\"" + item.Postcode + "\",\"" + item.ScoreAsString + "\",\"" + eWarn.ToString() + "\")");
                            break;
                        case Commands.Format:
                            sActions.Append("(\"" + item.Moniker + "\",\"" + eWarn.ToString() + "\")");
                            break;
                        case Commands.ForceFormat:
                            sActions.Append("(\"" + item.Moniker + "\")");
                            break;
                        case Commands.HaltIncomplete:
                        case Commands.HaltRange:
                            sActions.Append("()");
                            break;
                    }
                    sActions.Append("',");
                }
                Response.Write("</table>\";\n");
                Response.Write("var asActions = new Array(");
                Response.Write(sActions.ToString());
                Response.Write("'');\n");
            }
        }
        protected void RenderPicklistData(Picklist picklist)
        {
            RenderPicklistData(picklist, m_aHistory.Count.ToString());
        }
        protected string JavascriptEncode(string str)
        {
            return str.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\"", "\\\"");
        }
        protected string StoredCallback
        {
            get
            {
                return (string)ViewState[FIELD_CALLBACK];
            }
            set
            {
                ViewState[FIELD_CALLBACK] = value;
            }
        }
        protected string StoredDataID
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
        protected Dataset[] StoredDataMapList
        {
            get
            {
                return (Dataset[])ViewState[FIELD_DATALIST];
            }
            set
            {
                ViewState[FIELD_DATALIST] = value;
            }
        }
        protected string StoredErrorInfo
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
        protected string StoredMoniker
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
        protected Constants.Routes StoredRoute
        {
            get
            {
                object objValue = ViewState[Constants.FIELD_ROUTE];
                return (objValue != null) ? (Constants.Routes)objValue : Constants.Routes.Undefined;
            }
            set
            {
                ViewState[Constants.FIELD_ROUTE] = value;
            }
        }
        protected QuickAddress.EngineTypes StoredSearchEngine
        {
            get
            {
                object objValue = ViewState[FIELD_ENGINE];
                return (objValue != null) ? (QuickAddress.EngineTypes)objValue : QuickAddress.EngineTypes.Typedown;
            }
            set
            {
                ViewState[FIELD_ENGINE] = value;
            }
        }
        protected StepinWarnings StoredWarning
        {
            get
            {
                object objValue = ViewState[FIELD_WARNING];
                return (objValue != null) ? (StepinWarnings)objValue : StepinWarnings.None;
            }
            set
            {
                ViewState[FIELD_WARNING] = value;
            }
        }
        protected HistoryStack GetStoredHistory()
        {
            object objValue = ViewState[FIELD_HISTORY];
            if (objValue is ArrayList list)
            {
                HistoryStack stack = new HistoryStack(list);
                return stack;
            }
            return new HistoryStack();
        }
        protected void SetStoredHistory(HistoryStack value)
        {
            ViewState[FIELD_HISTORY] = value;
        }
        [Serializable()]
        protected class HistoryStack : ArrayList
        {
            public HistoryStack()
            {
            }
            public HistoryStack(ArrayList vValue)
            {
                foreach (object obj in vValue)
                {
                    base.Add((HistoryItem)obj);
                }
            }
            public void Add(HistoryItem item)
            {
                if (Count == 0 || !Peek().Moniker.Equals(item.Moniker))
                {
                    base.Add(item);
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
            public void Push(string sMoniker, string sText, string sPostcode, string sScore, string sRefine)
            {
                HistoryItem item = new HistoryItem(sMoniker, sText, sPostcode, sScore, sRefine);
                Add(item);
            }
            public void Push(PicklistItem item)
            {
                Push(item.Moniker, item.Text, item.Postcode, item.ScoreAsString, "");
            }
            public void Truncate(int iCount)
            {
                if (Count > iCount)
                {
                    RemoveRange(iCount, Count - iCount);
                }
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
        protected class HistoryItem
        {
            public string Moniker { get; set; }
            public string Text { get; set; }
            public string Postcode { get; set; }
            public string Score { get; set; }
            public string Refine { get; set; }

            public HistoryItem(string sMonikerIn, string sTextIn, string sPostcodeIn, string sScoreIn, string sRefineIn)
            {
                Moniker = sMonikerIn;
                Text = sTextIn;
                Postcode = sPostcodeIn;
                Score = sScoreIn;
                Refine = sRefineIn;
            }
        }
    }
}