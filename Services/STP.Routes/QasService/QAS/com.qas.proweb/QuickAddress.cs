using STP.Routes.QasService.QAS.com.qas.proweb.soap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace STP.Routes.QasService.QAS.com.qas.proweb
{
    public class QuickAddress
    {
        // -- Public Constants --
        public enum EngineTypes
        {
            Singleline = EngineEnumType.Singleline,
            Typedown = EngineEnumType.Typedown,
            Verification = EngineEnumType.Verification,
            Keyfinder = EngineEnumType.Keyfinder,
            Intuitive = EngineEnumType.Intuitive
        }
        public enum SearchingIntesityLevels
        {
            Exact = EngineIntensityType.Exact,
            Close = EngineIntensityType.Close,
            Extensive = EngineIntensityType.Extensive
        }
        /// Line separator - determined by a configuration setting on the server
        private const char cLINE_SEPARATOR = '|';
        // -- Private Members --
        // QuickAddress Pro Web search service
        private readonly QASOnDemandIntermediary m_Service;
        // Engine searching configuration settings (optional to override server defaults)
        private readonly EngineType m_Engine;
        // -- Public Construction --
        public QuickAddress(String sEndpointURL, String username, String password, IWebProxy proxy)
            : this(sEndpointURL, username, password)
        {
            if (proxy != null)
            {
                m_Service.Proxy = proxy;
            }
        }
        /// If you're integrating against the UK data centre: https://ws.ondemand.qas.com/ProOnDemand/V2/ProOnDemandService.asmx 
        /// If you're integrating against the US data centre: https://ws2.ondemand.qas.com/ProOnDemand/V2/ProOnDemandService.asmx 		 
        public QuickAddress(String sEndpointURL, String Username, String Password)
        {
            m_Service = new QASOnDemandIntermediary();
            m_Service.Url = sEndpointURL;
            QAAuthentication authentication = new QAAuthentication();
            authentication.Username = Username;
            authentication.Password = Password;
            QAQueryHeader header = new QAQueryHeader();
            header.QAAuthentication = authentication;
            m_Service.QAQueryHeaderValue = header;
            m_Engine = new EngineType();
        }
        // -- Public Properties --
        public EngineTypes Engine
        {
            get
            {
                return (EngineTypes)m_Engine.Value;
            }
            set
            {
                m_Engine.Value = (EngineEnumType)value;
            }
        }
        public void SetSearchingIntesity(SearchingIntesityLevels value)
        {
            m_Engine.Intensity = (EngineIntensityType)value;
            m_Engine.IntensitySpecified = true;
        }
        public void SetThreshold(int value)
        {
            m_Engine.Threshold = System.Convert.ToString(value);
        }
        public void SetTimeout(int value)
        {
            m_Engine.Timeout = System.Convert.ToString(value);
        }
        public void SetFlatten(bool value)
        {
            m_Engine.Flatten = value;
            m_Engine.FlattenSpecified = true;
        }
        // -- Public Methods - Searching Operations --
        public CanSearch CanSearch(string sDataID, string sLayout, PromptSet.Types tPromptSet)
        {
            QACanSearch param = new QACanSearch();
            param.Country = sDataID;
            param.Engine = m_Engine;
            param.Layout = sLayout;
            param.Engine.PromptSet = (PromptSetType)tPromptSet;
            param.Engine.PromptSetSpecified = true;
            CanSearch tResult = null;
            try
            {
                // Make the call to the server
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                QASearchOk cansearchResult = SearchService.DoCanSearch(param);
                tResult = new CanSearch(cansearchResult);
            }
            catch (Exception x)
            {
                MapException(x);
            }
            return tResult;
        }
        public CanSearch CanSearch(string sDataID, string sLayout)
        {
            return CanSearch(sDataID, sLayout, PromptSet.Types.Default);
        }
        public CanSearch CanSearch(string sDataID)
        {
            return CanSearch(sDataID, null);
        }
        public SearchResult Search(string sDataID, string[] asSearch, PromptSet.Types tPromptSet, string sLayout, string sRequestTag)
        {
            System.Diagnostics.Debug.Assert(asSearch != null && asSearch.GetLength(0) > 0);
            // Concatenate search terms
            StringBuilder sSearch = new StringBuilder(asSearch[0]);
            for (int i = 1; i < asSearch.GetLength(0); i++)
            {
                sSearch.Append(cLINE_SEPARATOR);
                sSearch.Append(asSearch[i]);
            }
            return Search(sDataID, sSearch.ToString(), tPromptSet, sLayout, sRequestTag);
        }
        public SearchResult Search(string sDataID, string[] asSearch, PromptSet.Types tPromptSet, string sLayout)
        {
            return Search(sDataID, asSearch, tPromptSet, sLayout, null);
        }
        public SearchResult Search(string sDataID, string[] asSearch, PromptSet.Types tPromptSet)
        {
            return Search(sDataID, asSearch, tPromptSet, null);
        }
        public SearchResult Search(string sDataID, string sSearch, PromptSet.Types tPromptSet, string sLayout, string sRequestTag)
        {
            System.Diagnostics.Debug.Assert(sDataID != null);
            System.Diagnostics.Debug.Assert(sSearch != null);
            // Set up the parameter for the SOAP call
            QASearch param = new QASearch();
            param.Country = sDataID;
            param.Engine = m_Engine;
            param.Engine.PromptSet = (PromptSetType)tPromptSet;
            param.Engine.PromptSetSpecified = true;
            param.Layout = sLayout;
            param.Search = sSearch;
            param.RequestTag = sRequestTag;
            SearchResult result = null;
            try
            {
                // Make the call to the server
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                QASearchResult searchResult = SearchService.DoSearch(param);
                result = new SearchResult(searchResult);
            }
            catch (Exception x)
            {
                MapException(x);
            }

            return result;
        }
        public SearchResult Search(string sDataID, string sSearch, PromptSet.Types tPromptSet, string sLayout)
        {
            return Search(sDataID, sSearch, tPromptSet, sLayout, null);
        }
        public SearchResult Search(string sDataID, string sSearch, PromptSet.Types tPromptSet)
        {
            return Search(sDataID, sSearch, tPromptSet, null);
        }
        public Picklist Refine(String sMoniker, String sRefinementText, String sRequestTag)
        {
            System.Diagnostics.Debug.Assert(sMoniker != null && sRefinementText != null);
            // Set up the parameter for the SOAP call
            QARefine param = new QARefine();
            param.Moniker = sMoniker;
            param.Refinement = sRefinementText;
            param.Threshold = m_Engine.Threshold;
            param.Timeout = m_Engine.Timeout;
            param.RequestTag = sRequestTag;

            Picklist result = null;
            try
            {
                // Make the call to the server
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                QAPicklistType picklist = SearchService.DoRefine(param).QAPicklist;

                result = new Picklist(picklist);
            }
            catch (Exception x)
            {
                MapException(x);
            }
            return result;
        }
        public Picklist Refine(String sMoniker, String sRefinementText)
        {
            return Refine(sMoniker, sRefinementText, null);
        }
        public Picklist StepIn(String sMoniker)
        {
            return Refine(sMoniker, "");
        }
        public FormattedAddress GetFormattedAddress(string sMoniker, string sLayout, string sRequestTag)
        {
            System.Diagnostics.Debug.Assert(sMoniker != null && sLayout != null);

            // Set up the parameter for the SOAP call
            QAGetAddress param = new QAGetAddress();
            param.Layout = sLayout;
            param.Moniker = sMoniker;
            param.RequestTag = sRequestTag;

            FormattedAddress result = null;
            try
            {
                // Make the call to the server
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                QAAddressType address = SearchService.DoGetAddress(param).QAAddress;

                result = new FormattedAddress(address);
            }
            catch (Exception x)
            {
                MapException(x);
            }
            return result;
        }
        public FormattedAddress GetFormattedAddress(string sMoniker, string sLayout)
        {
            return GetFormattedAddress(sMoniker, sLayout, null);
        }
        // -- Public Methods - Status Operations --
        public Dataset[] GetAllDatasets()
        {
            Dataset[] aResults = null;
            try
            {
                // Make the call to the server
                QAGetData getData = new QAGetData();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                QADataSet[] aDatasets = SearchService.DoGetData(getData);

                aResults = Dataset.CreateArray(aDatasets);
            }
            catch (Exception x)
            {
                MapException(x);
            }
            return aResults;
        }
        public LicensedSet[] GetDataMapDetail(string sID)
        {
            LicensedSet[] aDatasets = null;

            try
            {
                QAGetDataMapDetail tRequest = new QAGetDataMapDetail();
                tRequest.DataMap = sID;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                QADataMapDetail tMapDetail = SearchService.DoGetDataMapDetail(tRequest);
                aDatasets = LicensedSet.createArray(tMapDetail);
            }
            catch (Exception x)
            {
                MapException(x);
            }

            return aDatasets;
        }
        public Layout[] GetAllLayouts(string sDataID)
        {
            System.Diagnostics.Debug.Assert(sDataID != null);

            // Set up the parameter for the SOAP call
            QAGetLayouts param = new QAGetLayouts();
            param.Country = sDataID;

            Layout[] aResults = null;
            try
            {
                // Make the call to the server
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                QALayout[] aLayouts = SearchService.DoGetLayouts(param);
                aResults = Layout.CreateArray(aLayouts);
            }
            catch (Exception x)
            {
                MapException(x);
            }
            return aResults;
        }
        public ExampleAddress[] GetExampleAddresses(String sDataID, String sLayout, String sRequestTag)
        {
            // Set up the parameter for the SOAP call
            QAGetExampleAddresses param = new QAGetExampleAddresses();
            param.Country = sDataID;
            param.Layout = sLayout;
            param.RequestTag = sRequestTag;

            ExampleAddress[] aResults = null;
            try
            {
                // Make the call to the server
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                QAExampleAddress[] aAddresses = SearchService.DoGetExampleAddresses(param);
                aResults = ExampleAddress.createArray(aAddresses);
            }
            catch (Exception x)
            {
                MapException(x);
            }
            return aResults;
        }
        public ExampleAddress[] GetExampleAddresses(String sDataID, String sLayout)
        {
            return GetExampleAddresses(sDataID, sLayout, null);
        }
        public LicensedSet[] GetLicenceInfo()
        {
            LicensedSet[] aResults = null;
            try
            {
                // Make the call to the server
                QAGetLicenseInfo getLicenseInfo = new QAGetLicenseInfo();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                QALicenceInfo info = SearchService.DoGetLicenseInfo(getLicenseInfo);
                aResults = LicensedSet.createArray(info);
            }
            catch (Exception x)
            {
                MapException(x);
            }
            return aResults;
        }
        public PromptSet GetPromptSet(string sDataID, PromptSet.Types tType)
        {
            System.Diagnostics.Debug.Assert(sDataID != null);

            // Set up the parameter for the SOAP call
            QAGetPromptSet param = new QAGetPromptSet();
            param.Country = sDataID;
            param.Engine = m_Engine;
            param.PromptSet = (PromptSetType)tType;

            PromptSet result = null;
            try
            {
                // Make the call to the server
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                QAPromptSet tPromptSet = SearchService.DoGetPromptSet(param);
                result = new PromptSet(tPromptSet);
            }
            catch (Exception x)
            {
                MapException(x);
            }
            return result;
        }
        public String[] GetSystemInfo()
        {
            String[] aResults = null;
            try
            {
                // Make the call to the server
                QAGetSystemInfo getSystemInfo = new QAGetSystemInfo();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                aResults = SearchService.DoGetSystemInfo(getSystemInfo);
            }
            catch (Exception x)
            {
                MapException(x);
            }
            return aResults;
        }
        // -- Private Methods - Helpers --
        private QASOnDemandIntermediary SearchService
        {
            get
            {
                return m_Service;
            }
        }
        private void MapException(Exception x)
        {
            System.Diagnostics.Debugger.Log(0, "Error", x.ToString() + "\n");

            if (x is System.Web.Services.Protocols.SoapHeaderException)
            {
                System.Web.Services.Protocols.SoapHeaderException xHeader = x as System.Web.Services.Protocols.SoapHeaderException;
                throw xHeader;
            }
            else if (x is System.Web.Services.Protocols.SoapException)
            {
                // Parse out qas:QAFault string
                System.Web.Services.Protocols.SoapException xSoap = x as System.Web.Services.Protocols.SoapException;
                System.Xml.XmlNode xmlDetails = xSoap.Detail;

                string sMessage ;

                StringBuilder bld = new StringBuilder();

                foreach (System.Xml.XmlNode xmlDetail in xmlDetails.ChildNodes)
                {
                    string[] asDetail = xmlDetail.InnerText.Split('\n');
                    if (asDetail.Length == 2)
                    {
                        bld.Append(xmlDetail.Name + ": [" + asDetail[1].Trim() + " " + asDetail[0].Trim() + "] ");
                    }
                    else
                    {
                        bld.Append(xmlDetail.Name + ": [" + asDetail[0].Trim() + "] ");
                    }
                }
                sMessage = bld.ToString();
                Exception xThrow = new Exception(sMessage, xSoap);
                throw xThrow;
            }
            else
            {
                throw x;
            }
        }
    }
}