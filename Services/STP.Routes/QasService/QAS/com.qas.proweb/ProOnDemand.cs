﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.QasService.QAS.com.qas.proweb.soap
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "QASOnDemand", Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QASOnDemandIntermediary : System.Web.Services.Protocols.SoapHttpClientProtocol
    {
        private QAQueryHeader qAQueryHeaderValueField;
        private QAInformation qAInformationValueField;
        private System.Threading.SendOrPostCallback DoSearchOperationCompleted;
        private System.Threading.SendOrPostCallback DoRefineOperationCompleted;
        private System.Threading.SendOrPostCallback DoGetAddressOperationCompleted;
        private System.Threading.SendOrPostCallback DoGetDataOperationCompleted;
        private System.Threading.SendOrPostCallback DoGetDataMapDetailOperationCompleted;
        private System.Threading.SendOrPostCallback DoGetLicenseInfoOperationCompleted;
        private System.Threading.SendOrPostCallback DoGetSystemInfoOperationCompleted;
        private System.Threading.SendOrPostCallback DoGetExampleAddressesOperationCompleted;
        private System.Threading.SendOrPostCallback DoGetLayoutsOperationCompleted;
        private System.Threading.SendOrPostCallback DoGetPromptSetOperationCompleted;
        private System.Threading.SendOrPostCallback DoCanSearchOperationCompleted;
        public QASOnDemandIntermediary()
        {
            string urlSetting = System.Configuration.ConfigurationManager.AppSettings["NOTUSED"];
            if ((urlSetting != null))
            {
                this.Url = urlSetting;
            }
            else
            {
                this.Url = "https://ws.ondemand.qas.com/ProOnDemand/V3/ProOnDemandService.asmx";
            }
        }
        public QAQueryHeader QAQueryHeaderValue
        {
            get
            {
                return this.qAQueryHeaderValueField;
            }
            set
            {
                this.qAQueryHeaderValueField = value;
            }
        }
        public QAInformation QAInformationValue
        {
            get
            {
                return this.qAInformationValueField;
            }
            set
            {
                this.qAInformationValueField = value;
            }
        }
        public event DoSearchCompletedEventHandler DoSearchCompleted;
        public event DoRefineCompletedEventHandler DoRefineCompleted;
        public event DoGetAddressCompletedEventHandler DoGetAddressCompleted;
        public event DoGetDataCompletedEventHandler DoGetDataCompleted;
        public event DoGetDataMapDetailCompletedEventHandler DoGetDataMapDetailCompleted;
        public event DoGetLicenseInfoCompletedEventHandler DoGetLicenseInfoCompleted;
        public event DoGetSystemInfoCompletedEventHandler DoGetSystemInfoCompleted;
        public event DoGetExampleAddressesCompletedEventHandler DoGetExampleAddressesCompleted;
        public event DoGetLayoutsCompletedEventHandler DoGetLayoutsCompleted;
        public event DoGetPromptSetCompletedEventHandler DoGetPromptSetCompleted;
        public event DoCanSearchCompletedEventHandler DoCanSearchCompleted;
        
        [System.Web.Services.Protocols.SoapHeaderAttribute("QAQueryHeaderValue")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("QAInformationValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.qas.com/OnDemand-2011-03/DoSearch", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("QASearchResult", Namespace = "http://www.qas.com/OnDemand-2011-03")]
        public QASearchResult DoSearch([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")] QASearch QASearch)
        {
            object[] results = this.Invoke("DoSearch", new object[] {
                        QASearch});
            return ((QASearchResult)(results[0]));
        }
        public System.IAsyncResult BeginDoSearch(QASearch QASearch, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DoSearch", new object[] {
                        QASearch}, callback, asyncState);
        }
        public QASearchResult EndDoSearch(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((QASearchResult)(results[0]));
        }
        public void DoSearchAsync(QASearch QASearch)
        {
            this.DoSearchAsync(QASearch, null);
        }
        public void DoSearchAsync(QASearch QASearch, object userState)
        {
            if ((this.DoSearchOperationCompleted == null))
            {
                this.DoSearchOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDoSearchOperationCompleted);
            }
            this.InvokeAsync("DoSearch", new object[] {
                        QASearch}, this.DoSearchOperationCompleted, userState);
        }
        private void OnDoSearchOperationCompleted(object arg)
        {
            if ((this.DoSearchCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DoSearchCompleted(this, new DoSearchCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        [System.Web.Services.Protocols.SoapHeaderAttribute("QAQueryHeaderValue")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("QAInformationValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.qas.com/OnDemand-2011-03/DoRefine", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("Picklist", Namespace = "http://www.qas.com/OnDemand-2011-03")]
        public Picklist DoRefine([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")] QARefine QARefine)
        {
            object[] results = this.Invoke("DoRefine", new object[] {
                        QARefine});
            return ((Picklist)(results[0]));
        }
        public System.IAsyncResult BeginDoRefine(QARefine QARefine, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DoRefine", new object[] {
                        QARefine}, callback, asyncState);
        }
        public Picklist EndDoRefine(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((Picklist)(results[0]));
        }
        public void DoRefineAsync(QARefine QARefine)
        {
            this.DoRefineAsync(QARefine, null);
        }
        public void DoRefineAsync(QARefine QARefine, object userState)
        {
            if ((this.DoRefineOperationCompleted == null))
            {
                this.DoRefineOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDoRefineOperationCompleted);
            }
            this.InvokeAsync("DoRefine", new object[] {
                        QARefine}, this.DoRefineOperationCompleted, userState);
        }
        private void OnDoRefineOperationCompleted(object arg)
        {
            if ((this.DoRefineCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DoRefineCompleted(this, new DoRefineCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        [System.Web.Services.Protocols.SoapHeaderAttribute("QAQueryHeaderValue")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("QAInformationValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.qas.com/OnDemand-2011-03/DoGetAddress", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("Address", Namespace = "http://www.qas.com/OnDemand-2011-03")]
        public Address DoGetAddress([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")] QAGetAddress QAGetAddress)
        {
            object[] results = this.Invoke("DoGetAddress", new object[] {
                        QAGetAddress});
            return ((Address)(results[0]));
        }
        public System.IAsyncResult BeginDoGetAddress(QAGetAddress QAGetAddress, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DoGetAddress", new object[] {
                        QAGetAddress}, callback, asyncState);
        }
        public Address EndDoGetAddress(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((Address)(results[0]));
        }
        public void DoGetAddressAsync(QAGetAddress QAGetAddress)
        {
            this.DoGetAddressAsync(QAGetAddress, null);
        }
        public void DoGetAddressAsync(QAGetAddress QAGetAddress, object userState)
        {
            if ((this.DoGetAddressOperationCompleted == null))
            {
                this.DoGetAddressOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDoGetAddressOperationCompleted);
            }
            this.InvokeAsync("DoGetAddress", new object[] {
                        QAGetAddress}, this.DoGetAddressOperationCompleted, userState);
        }
        private void OnDoGetAddressOperationCompleted(object arg)
        {
            if ((this.DoGetAddressCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DoGetAddressCompleted(this, new DoGetAddressCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        [System.Web.Services.Protocols.SoapHeaderAttribute("QAQueryHeaderValue")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("QAInformationValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.qas.com/OnDemand-2011-03/DoGetData", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlArrayAttribute("QAData", Namespace = "http://www.qas.com/OnDemand-2011-03")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute("DataSet", IsNullable = false)]
        public QADataSet[] DoGetData([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")] QAGetData QAGetData)
        {
            object[] results = this.Invoke("DoGetData", new object[] {
                        QAGetData});
            return ((QADataSet[])(results[0]));
        }
        public System.IAsyncResult BeginDoGetData(QAGetData QAGetData, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DoGetData", new object[] {
                        QAGetData}, callback, asyncState);
        }
        public QADataSet[] EndDoGetData(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((QADataSet[])(results[0]));
        }
        public void DoGetDataAsync(QAGetData QAGetData)
        {
            this.DoGetDataAsync(QAGetData, null);
        }
        public void DoGetDataAsync(QAGetData QAGetData, object userState)
        {
            if ((this.DoGetDataOperationCompleted == null))
            {
                this.DoGetDataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDoGetDataOperationCompleted);
            }
            this.InvokeAsync("DoGetData", new object[] {
                        QAGetData}, this.DoGetDataOperationCompleted, userState);
        }
        private void OnDoGetDataOperationCompleted(object arg)
        {
            if ((this.DoGetDataCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DoGetDataCompleted(this, new DoGetDataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        [System.Web.Services.Protocols.SoapHeaderAttribute("QAQueryHeaderValue")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("QAInformationValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.qas.com/OnDemand-2011-03/DoGetDataMapDetail", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("QADataMapDetail", Namespace = "http://www.qas.com/OnDemand-2011-03")]
        public QADataMapDetail DoGetDataMapDetail([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")] QAGetDataMapDetail QAGetDataMapDetail)
        {
            object[] results = this.Invoke("DoGetDataMapDetail", new object[] {
                        QAGetDataMapDetail});
            return ((QADataMapDetail)(results[0]));
        }
        public System.IAsyncResult BeginDoGetDataMapDetail(QAGetDataMapDetail QAGetDataMapDetail, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DoGetDataMapDetail", new object[] {
                        QAGetDataMapDetail}, callback, asyncState);
        }
        public QADataMapDetail EndDoGetDataMapDetail(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((QADataMapDetail)(results[0]));
        }
        public void DoGetDataMapDetailAsync(QAGetDataMapDetail QAGetDataMapDetail)
        {
            this.DoGetDataMapDetailAsync(QAGetDataMapDetail, null);
        }
        public void DoGetDataMapDetailAsync(QAGetDataMapDetail QAGetDataMapDetail, object userState)
        {
            if ((this.DoGetDataMapDetailOperationCompleted == null))
            {
                this.DoGetDataMapDetailOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDoGetDataMapDetailOperationCompleted);
            }
            this.InvokeAsync("DoGetDataMapDetail", new object[] {
                        QAGetDataMapDetail}, this.DoGetDataMapDetailOperationCompleted, userState);
        }
        private void OnDoGetDataMapDetailOperationCompleted(object arg)
        {
            if ((this.DoGetDataMapDetailCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DoGetDataMapDetailCompleted(this, new DoGetDataMapDetailCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        [System.Web.Services.Protocols.SoapHeaderAttribute("QAQueryHeaderValue")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("QAInformationValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.qas.com/OnDemand-2011-03/DoGetLicenseInfo", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("QALicenceInfo", Namespace = "http://www.qas.com/OnDemand-2011-03")]
        public QALicenceInfo DoGetLicenseInfo([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")] QAGetLicenseInfo QAGetLicenseInfo)
        {
            object[] results = this.Invoke("DoGetLicenseInfo", new object[] {
                        QAGetLicenseInfo});
            return ((QALicenceInfo)(results[0]));
        }
        public System.IAsyncResult BeginDoGetLicenseInfo(QAGetLicenseInfo QAGetLicenseInfo, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DoGetLicenseInfo", new object[] {
                        QAGetLicenseInfo}, callback, asyncState);
        }
        public QALicenceInfo EndDoGetLicenseInfo(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((QALicenceInfo)(results[0]));
        }
        public void DoGetLicenseInfoAsync(QAGetLicenseInfo QAGetLicenseInfo)
        {
            this.DoGetLicenseInfoAsync(QAGetLicenseInfo, null);
        }
        public void DoGetLicenseInfoAsync(QAGetLicenseInfo QAGetLicenseInfo, object userState)
        {
            if ((this.DoGetLicenseInfoOperationCompleted == null))
            {
                this.DoGetLicenseInfoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDoGetLicenseInfoOperationCompleted);
            }
            this.InvokeAsync("DoGetLicenseInfo", new object[] {
                        QAGetLicenseInfo}, this.DoGetLicenseInfoOperationCompleted, userState);
        }
        private void OnDoGetLicenseInfoOperationCompleted(object arg)
        {
            if ((this.DoGetLicenseInfoCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DoGetLicenseInfoCompleted(this, new DoGetLicenseInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        [System.Web.Services.Protocols.SoapHeaderAttribute("QAQueryHeaderValue")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("QAInformationValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.qas.com/OnDemand-2011-03/DoGetSystemInfo", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlArrayAttribute("QASystemInfo", Namespace = "http://www.qas.com/OnDemand-2011-03")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute("SystemInfo", IsNullable = false)]
        public string[] DoGetSystemInfo([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")] QAGetSystemInfo QAGetSystemInfo)
        {
            object[] results = this.Invoke("DoGetSystemInfo", new object[] {
                        QAGetSystemInfo});
            return ((string[])(results[0]));
        }
        public System.IAsyncResult BeginDoGetSystemInfo(QAGetSystemInfo QAGetSystemInfo, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DoGetSystemInfo", new object[] {
                        QAGetSystemInfo}, callback, asyncState);
        }
        public string[] EndDoGetSystemInfo(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string[])(results[0]));
        }
        public void DoGetSystemInfoAsync(QAGetSystemInfo QAGetSystemInfo)
        {
            this.DoGetSystemInfoAsync(QAGetSystemInfo, null);
        }
        public void DoGetSystemInfoAsync(QAGetSystemInfo QAGetSystemInfo, object userState)
        {
            if ((this.DoGetSystemInfoOperationCompleted == null))
            {
                this.DoGetSystemInfoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDoGetSystemInfoOperationCompleted);
            }
            this.InvokeAsync("DoGetSystemInfo", new object[] {
                        QAGetSystemInfo}, this.DoGetSystemInfoOperationCompleted, userState);
        }
        private void OnDoGetSystemInfoOperationCompleted(object arg)
        {
            if ((this.DoGetSystemInfoCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DoGetSystemInfoCompleted(this, new DoGetSystemInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        [System.Web.Services.Protocols.SoapHeaderAttribute("QAQueryHeaderValue")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("QAInformationValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.qas.com/OnDemand-2011-03/DoGetExampleAddresses", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlArrayAttribute("QAExampleAddresses", Namespace = "http://www.qas.com/OnDemand-2011-03")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute("ExampleAddress", IsNullable = false)]
        public QAExampleAddress[] DoGetExampleAddresses([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")] QAGetExampleAddresses QAGetExampleAddresses)
        {
            object[] results = this.Invoke("DoGetExampleAddresses", new object[] {
                        QAGetExampleAddresses});
            return ((QAExampleAddress[])(results[0]));
        }
        public System.IAsyncResult BeginDoGetExampleAddresses(QAGetExampleAddresses QAGetExampleAddresses, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DoGetExampleAddresses", new object[] {
                        QAGetExampleAddresses}, callback, asyncState);
        }
        public QAExampleAddress[] EndDoGetExampleAddresses(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((QAExampleAddress[])(results[0]));
        }
        public void DoGetExampleAddressesAsync(QAGetExampleAddresses QAGetExampleAddresses)
        {
            this.DoGetExampleAddressesAsync(QAGetExampleAddresses, null);
        }
        public void DoGetExampleAddressesAsync(QAGetExampleAddresses QAGetExampleAddresses, object userState)
        {
            if ((this.DoGetExampleAddressesOperationCompleted == null))
            {
                this.DoGetExampleAddressesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDoGetExampleAddressesOperationCompleted);
            }
            this.InvokeAsync("DoGetExampleAddresses", new object[] {
                        QAGetExampleAddresses}, this.DoGetExampleAddressesOperationCompleted, userState);
        }
        private void OnDoGetExampleAddressesOperationCompleted(object arg)
        {
            if ((this.DoGetExampleAddressesCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DoGetExampleAddressesCompleted(this, new DoGetExampleAddressesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        [System.Web.Services.Protocols.SoapHeaderAttribute("QAQueryHeaderValue")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("QAInformationValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.qas.com/OnDemand-2011-03/DoGetLayouts", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlArrayAttribute("QALayouts", Namespace = "http://www.qas.com/OnDemand-2011-03")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute("Layout", IsNullable = false)]
        public QALayout[] DoGetLayouts([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")] QAGetLayouts QAGetLayouts)
        {
            object[] results = this.Invoke("DoGetLayouts", new object[] {
                        QAGetLayouts});
            return ((QALayout[])(results[0]));
        }
        public System.IAsyncResult BeginDoGetLayouts(QAGetLayouts QAGetLayouts, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DoGetLayouts", new object[] {
                        QAGetLayouts}, callback, asyncState);
        }
        public QALayout[] EndDoGetLayouts(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((QALayout[])(results[0]));
        }
        public void DoGetLayoutsAsync(QAGetLayouts QAGetLayouts)
        {
            this.DoGetLayoutsAsync(QAGetLayouts, null);
        }
        public void DoGetLayoutsAsync(QAGetLayouts QAGetLayouts, object userState)
        {
            if ((this.DoGetLayoutsOperationCompleted == null))
            {
                this.DoGetLayoutsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDoGetLayoutsOperationCompleted);
            }
            this.InvokeAsync("DoGetLayouts", new object[] {
                        QAGetLayouts}, this.DoGetLayoutsOperationCompleted, userState);
        }
        private void OnDoGetLayoutsOperationCompleted(object arg)
        {
            if ((this.DoGetLayoutsCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DoGetLayoutsCompleted(this, new DoGetLayoutsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        [System.Web.Services.Protocols.SoapHeaderAttribute("QAQueryHeaderValue")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("QAInformationValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.qas.com/OnDemand-2011-03/DoGetPromptSet", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("QAPromptSet", Namespace = "http://www.qas.com/OnDemand-2011-03")]
        public QAPromptSet DoGetPromptSet([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")] QAGetPromptSet QAGetPromptSet)
        {
            object[] results = this.Invoke("DoGetPromptSet", new object[] {
                        QAGetPromptSet});
            return ((QAPromptSet)(results[0]));
        }
        public System.IAsyncResult BeginDoGetPromptSet(QAGetPromptSet QAGetPromptSet, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DoGetPromptSet", new object[] {
                        QAGetPromptSet}, callback, asyncState);
        }
        public QAPromptSet EndDoGetPromptSet(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((QAPromptSet)(results[0]));
        }
        public void DoGetPromptSetAsync(QAGetPromptSet QAGetPromptSet)
        {
            this.DoGetPromptSetAsync(QAGetPromptSet, null);
        }
        public void DoGetPromptSetAsync(QAGetPromptSet QAGetPromptSet, object userState)
        {
            if ((this.DoGetPromptSetOperationCompleted == null))
            {
                this.DoGetPromptSetOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDoGetPromptSetOperationCompleted);
            }
            this.InvokeAsync("DoGetPromptSet", new object[] {
                        QAGetPromptSet}, this.DoGetPromptSetOperationCompleted, userState);
        }
        private void OnDoGetPromptSetOperationCompleted(object arg)
        {
            if ((this.DoGetPromptSetCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DoGetPromptSetCompleted(this, new DoGetPromptSetCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        [System.Web.Services.Protocols.SoapHeaderAttribute("QAQueryHeaderValue")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("QAInformationValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.qas.com/OnDemand-2011-03/DoCanSearch", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("QASearchOk", Namespace = "http://www.qas.com/OnDemand-2011-03")]
        public QASearchOk DoCanSearch([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")] QACanSearch QACanSearch)
        {
            object[] results = this.Invoke("DoCanSearch", new object[] {
                        QACanSearch});
            return ((QASearchOk)(results[0]));
        }
        public System.IAsyncResult BeginDoCanSearch(QACanSearch QACanSearch, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DoCanSearch", new object[] {
                        QACanSearch}, callback, asyncState);
        }
        public QASearchOk EndDoCanSearch(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((QASearchOk)(results[0]));
        }

        /// <remarks/>
        public void DoCanSearchAsync(QACanSearch QACanSearch)
        {
            this.DoCanSearchAsync(QACanSearch, null);
        }
        public void DoCanSearchAsync(QACanSearch QACanSearch, object userState)
        {
            if ((this.DoCanSearchOperationCompleted == null))
            {
                this.DoCanSearchOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDoCanSearchOperationCompleted);
            }
            this.InvokeAsync("DoCanSearch", new object[] {
                        QACanSearch}, this.DoCanSearchOperationCompleted, userState);
        }
        private void OnDoCanSearchOperationCompleted(object arg)
        {
            if ((this.DoCanSearchCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DoCanSearchCompleted(this, new DoCanSearchCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03", IsNullable = false)]
    public partial class QAQueryHeader : System.Web.Services.Protocols.SoapHeader
    {
        private QAAuthentication qAAuthenticationField;
        private SecurityHeaderType securityField;
        public QAAuthentication QAAuthentication
        {
            get
            {
                return this.qAAuthenticationField;
            }
            set
            {
                this.qAAuthenticationField = value;
            }
        }
        public SecurityHeaderType Security
        {
            get
            {
                return this.securityField;
            }
            set
            {
                this.securityField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QAAuthentication
    {
        private string usernameField;
        private string passwordField;
        public string Username
        {
            get
            {
                return this.usernameField;
            }
            set
            {
                this.usernameField = value;
            }
        }
        public string Password
        {
            get
            {
                return this.passwordField;
            }
            set
            {
                this.passwordField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class PromptLine
    {
        private string promptField;
        private string suggestedInputLengthField;
        private string exampleField;
        public string Prompt
        {
            get
            {
                return this.promptField;
            }
            set
            {
                this.promptField = value;
            }
        }
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string SuggestedInputLength
        {
            get
            {
                return this.suggestedInputLengthField;
            }
            set
            {
                this.suggestedInputLengthField = value;
            }
        }
        public string Example
        {
            get
            {
                return this.exampleField;
            }
            set
            {
                this.exampleField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QALayout
    {
        private string nameField;
        private string commentField;
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
        public string Comment
        {
            get
            {
                return this.commentField;
            }
            set
            {
                this.commentField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QAExampleAddress
    {
        private QAAddressType addressField;
        private string commentField;
        public QAAddressType Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }
        public string Comment
        {
            get
            {
                return this.commentField;
            }
            set
            {
                this.commentField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QAAddressType
    {
        private AddressLineType[] addressLineField;
        private bool overflowField;
        private bool truncatedField;
        private DPVStatusType dPVStatusField;
        private bool dPVStatusFieldSpecified;
        private bool missingSubPremiseField;
        public QAAddressType()
        {
            this.overflowField = false;
            this.truncatedField = false;
            this.missingSubPremiseField = false;
        }
        [System.Xml.Serialization.XmlElementAttribute("AddressLine")]
        public AddressLineType[] AddressLine
        {
            get
            {
                return this.addressLineField;
            }
            set
            {
                this.addressLineField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool Overflow
        {
            get
            {
                return this.overflowField;
            }
            set
            {
                this.overflowField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool Truncated
        {
            get
            {
                return this.truncatedField;
            }
            set
            {
                this.truncatedField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public DPVStatusType DPVStatus
        {
            get
            {
                return this.dPVStatusField;
            }
            set
            {
                this.dPVStatusField = value;
            }
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DPVStatusSpecified
        {
            get
            {
                return this.dPVStatusFieldSpecified;
            }
            set
            {
                this.dPVStatusFieldSpecified = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool MissingSubPremise
        {
            get
            {
                return this.missingSubPremiseField;
            }
            set
            {
                this.missingSubPremiseField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class AddressLineType
    {
        private string labelField;
        private string lineField;
        private DataplusGroupType[] dataplusGroupField;
        private LineContentType lineContentField;
        private bool overflowField;
        private bool truncatedField;
        public AddressLineType()
        {
            this.lineContentField = LineContentType.Address;
            this.overflowField = false;
            this.truncatedField = false;
        }
        public string Label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }
        public string Line
        {
            get
            {
                return this.lineField;
            }
            set
            {
                this.lineField = value;
            }
        }
        [System.Xml.Serialization.XmlElementAttribute("DataplusGroup")]
        public DataplusGroupType[] DataplusGroup
        {
            get
            {
                return this.dataplusGroupField;
            }
            set
            {
                this.dataplusGroupField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(LineContentType.Address)]
        public LineContentType LineContent
        {
            get
            {
                return this.lineContentField;
            }
            set
            {
                this.lineContentField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool Overflow
        {
            get
            {
                return this.overflowField;
            }
            set
            {
                this.overflowField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool Truncated
        {
            get
            {
                return this.truncatedField;
            }
            set
            {
                this.truncatedField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class DataplusGroupType
    {
        private string[] dataplusGroupItemField;
        private string groupNameField;
        [System.Xml.Serialization.XmlElementAttribute("DataplusGroupItem")]
        public string[] DataplusGroupItem
        {
            get
            {
                return this.dataplusGroupItemField;
            }
            set
            {
                this.dataplusGroupItemField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GroupName
        {
            get
            {
                return this.groupNameField;
            }
            set
            {
                this.groupNameField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public enum LineContentType
    {
        None,
        Address,
        Name,
        Ancillary,
        DataPlus,
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public enum DPVStatusType
    {
        DPVNotConfigured,
        DPVConfigured,
        DPVConfirmed,
        DPVConfirmedMissingSec,
        DPVNotConfirmed,
        DPVLocked,
        DPVSeedHit,
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QALicensedSet
    {
        private string idField;
        private string descriptionField;
        private string copyrightField;
        private string versionField;
        private string baseCountryField;
        private string statusField;
        private string serverField;
        private LicenceWarningLevel warningLevelField;
        private string daysLeftField;
        private string dataDaysLeftField;
        private string licenceDaysLeftField;
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
        public string Copyright
        {
            get
            {
                return this.copyrightField;
            }
            set
            {
                this.copyrightField = value;
            }
        }
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
        public string BaseCountry
        {
            get
            {
                return this.baseCountryField;
            }
            set
            {
                this.baseCountryField = value;
            }
        }
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
        public string Server
        {
            get
            {
                return this.serverField;
            }
            set
            {
                this.serverField = value;
            }
        }
        public LicenceWarningLevel WarningLevel
        {
            get
            {
                return this.warningLevelField;
            }
            set
            {
                this.warningLevelField = value;
            }
        }
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string DaysLeft
        {
            get
            {
                return this.daysLeftField;
            }
            set
            {
                this.daysLeftField = value;
            }
        }
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string DataDaysLeft
        {
            get
            {
                return this.dataDaysLeftField;
            }
            set
            {
                this.dataDaysLeftField = value;
            }
        }
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string LicenceDaysLeft
        {
            get
            {
                return this.licenceDaysLeftField;
            }
            set
            {
                this.licenceDaysLeftField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public enum LicenceWarningLevel
    {
        None,
        DataExpiring,
        LicenceExpiring,
        ClicksLow,
        Evaluation,
        NoClicks,
        DataExpired,
        EvalLicenceExpired,
        FullLicenceExpired,
        LicenceNotFound,
        DataUnreadable,
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QADataSet
    {
        private string idField;
        private string nameField;
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class VerificationFlagsType
    {
        private bool bldgFirmNameChangedField;
        private bool primaryNumberChangedField;
        private bool streetCorrectedField;
        private bool ruralRteHighwayContractMatchedField;
        private bool cityNameChangedField;
        private bool cityAliasMatchedField;
        private bool stateProvinceChangedField;
        private bool postCodeCorrectedField;
        private bool secondaryNumRetainedField;
        private bool idenPreStInfoRetainedField;
        private bool genPreStInfoRetainedField;
        private bool postStInfoRetainedField;
        public VerificationFlagsType()
        {
            this.bldgFirmNameChangedField = false;
            this.primaryNumberChangedField = false;
            this.streetCorrectedField = false;
            this.ruralRteHighwayContractMatchedField = false;
            this.cityNameChangedField = false;
            this.cityAliasMatchedField = false;
            this.stateProvinceChangedField = false;
            this.postCodeCorrectedField = false;
            this.secondaryNumRetainedField = false;
            this.idenPreStInfoRetainedField = false;
            this.genPreStInfoRetainedField = false;
            this.postStInfoRetainedField = false;
        }
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool BldgFirmNameChanged
        {
            get
            {
                return this.bldgFirmNameChangedField;
            }
            set
            {
                this.bldgFirmNameChangedField = value;
            }
        }
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool PrimaryNumberChanged
        {
            get
            {
                return this.primaryNumberChangedField;
            }
            set
            {
                this.primaryNumberChangedField = value;
            }
        }
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool StreetCorrected
        {
            get
            {
                return this.streetCorrectedField;
            }
            set
            {
                this.streetCorrectedField = value;
            }
        }
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool RuralRteHighwayContractMatched
        {
            get
            {
                return this.ruralRteHighwayContractMatchedField;
            }
            set
            {
                this.ruralRteHighwayContractMatchedField = value;
            }
        }
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool CityNameChanged
        {
            get
            {
                return this.cityNameChangedField;
            }
            set
            {
                this.cityNameChangedField = value;
            }
        }
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool CityAliasMatched
        {
            get
            {
                return this.cityAliasMatchedField;
            }
            set
            {
                this.cityAliasMatchedField = value;
            }
        }
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool StateProvinceChanged
        {
            get
            {
                return this.stateProvinceChangedField;
            }
            set
            {
                this.stateProvinceChangedField = value;
            }
        }
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool PostCodeCorrected
        {
            get
            {
                return this.postCodeCorrectedField;
            }
            set
            {
                this.postCodeCorrectedField = value;
            }
        }
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SecondaryNumRetained
        {
            get
            {
                return this.secondaryNumRetainedField;
            }
            set
            {
                this.secondaryNumRetainedField = value;
            }
        }
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool IdenPreStInfoRetained
        {
            get
            {
                return this.idenPreStInfoRetainedField;
            }
            set
            {
                this.idenPreStInfoRetainedField = value;
            }
        }
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool GenPreStInfoRetained
        {
            get
            {
                return this.genPreStInfoRetainedField;
            }
            set
            {
                this.genPreStInfoRetainedField = value;
            }
        }
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool PostStInfoRetained
        {
            get
            {
                return this.postStInfoRetainedField;
            }
            set
            {
                this.postStInfoRetainedField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class PicklistEntryType
    {
        private string monikerField;
        private string partialAddressField;
        private string picklistField;
        private string postcodeField;
        private string scoreField;
        private QAAddressType qAAddressField;
        private bool fullAddressField;
        private bool multiplesField;
        private bool canStepField;
        private bool aliasMatchField;
        private bool postcodeRecodedField;
        private bool crossBorderMatchField;
        private bool dummyPOBoxField;
        private bool nameField;
        private bool informationField;
        private bool warnInformationField;
        private bool incompleteAddrField;
        private bool unresolvableRangeField;
        private bool phantomPrimaryPointField;
        private bool subsidiaryDataField;
        private bool extendedDataField;
        private bool enhancedDataField;
        public PicklistEntryType()
        {
            this.fullAddressField = false;
            this.multiplesField = false;
            this.canStepField = false;
            this.aliasMatchField = false;
            this.postcodeRecodedField = false;
            this.crossBorderMatchField = false;
            this.dummyPOBoxField = false;
            this.nameField = false;
            this.informationField = false;
            this.warnInformationField = false;
            this.incompleteAddrField = false;
            this.unresolvableRangeField = false;
            this.phantomPrimaryPointField = false;
            this.subsidiaryDataField = false;
            this.extendedDataField = false;
            this.enhancedDataField = false;
        }
        public string Moniker
        {
            get
            {
                return this.monikerField;
            }
            set
            {
                this.monikerField = value;
            }
        }
        public string PartialAddress
        {
            get
            {
                return this.partialAddressField;
            }
            set
            {
                this.partialAddressField = value;
            }
        }
        public string Picklist
        {
            get
            {
                return this.picklistField;
            }
            set
            {
                this.picklistField = value;
            }
        }
        public string Postcode
        {
            get
            {
                return this.postcodeField;
            }
            set
            {
                this.postcodeField = value;
            }
        }
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string Score
        {
            get
            {
                return this.scoreField;
            }
            set
            {
                this.scoreField = value;
            }
        }
        public QAAddressType QAAddress
        {
            get
            {
                return this.qAAddressField;
            }
            set
            {
                this.qAAddressField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool FullAddress
        {
            get
            {
                return this.fullAddressField;
            }
            set
            {
                this.fullAddressField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool Multiples
        {
            get
            {
                return this.multiplesField;
            }
            set
            {
                this.multiplesField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool CanStep
        {
            get
            {
                return this.canStepField;
            }
            set
            {
                this.canStepField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool AliasMatch
        {
            get
            {
                return this.aliasMatchField;
            }
            set
            {
                this.aliasMatchField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool PostcodeRecoded
        {
            get
            {
                return this.postcodeRecodedField;
            }
            set
            {
                this.postcodeRecodedField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool CrossBorderMatch
        {
            get
            {
                return this.crossBorderMatchField;
            }
            set
            {
                this.crossBorderMatchField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool DummyPOBox
        {
            get
            {
                return this.dummyPOBoxField;
            }
            set
            {
                this.dummyPOBoxField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool Information
        {
            get
            {
                return this.informationField;
            }
            set
            {
                this.informationField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool WarnInformation
        {
            get
            {
                return this.warnInformationField;
            }
            set
            {
                this.warnInformationField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool IncompleteAddr
        {
            get
            {
                return this.incompleteAddrField;
            }
            set
            {
                this.incompleteAddrField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool UnresolvableRange
        {
            get
            {
                return this.unresolvableRangeField;
            }
            set
            {
                this.unresolvableRangeField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool PhantomPrimaryPoint
        {
            get
            {
                return this.phantomPrimaryPointField;
            }
            set
            {
                this.phantomPrimaryPointField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SubsidiaryData
        {
            get
            {
                return this.subsidiaryDataField;
            }
            set
            {
                this.subsidiaryDataField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool ExtendedData
        {
            get
            {
                return this.extendedDataField;
            }
            set
            {
                this.extendedDataField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool EnhancedData
        {
            get
            {
                return this.enhancedDataField;
            }
            set
            {
                this.enhancedDataField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QAPicklistType
    {
        private string fullPicklistMonikerField;
        private PicklistEntryType[] picklistEntryField;
        private string promptField;
        private string totalField;
        private bool autoFormatSafeField;
        private bool autoFormatPastCloseField;
        private bool autoStepinSafeField;
        private bool autoStepinPastCloseField;
        private bool largePotentialField;
        private bool maxMatchesField;
        private bool moreOtherMatchesField;
        private bool overThresholdField;
        private bool timeoutField;
        public QAPicklistType()
        {
            this.autoFormatSafeField = false;
            this.autoFormatPastCloseField = false;
            this.autoStepinSafeField = false;
            this.autoStepinPastCloseField = false;
            this.largePotentialField = false;
            this.maxMatchesField = false;
            this.moreOtherMatchesField = false;
            this.overThresholdField = false;
            this.timeoutField = false;
        }
        public string FullPicklistMoniker
        {
            get
            {
                return this.fullPicklistMonikerField;
            }
            set
            {
                this.fullPicklistMonikerField = value;
            }
        }
        [System.Xml.Serialization.XmlElementAttribute("PicklistEntry")]
        public PicklistEntryType[] PicklistEntry
        {
            get
            {
                return this.picklistEntryField;
            }
            set
            {
                this.picklistEntryField = value;
            }
        }
        public string Prompt
        {
            get
            {
                return this.promptField;
            }
            set
            {
                this.promptField = value;
            }
        }
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string Total
        {
            get
            {
                return this.totalField;
            }
            set
            {
                this.totalField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool AutoFormatSafe
        {
            get
            {
                return this.autoFormatSafeField;
            }
            set
            {
                this.autoFormatSafeField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool AutoFormatPastClose
        {
            get
            {
                return this.autoFormatPastCloseField;
            }
            set
            {
                this.autoFormatPastCloseField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool AutoStepinSafe
        {
            get
            {
                return this.autoStepinSafeField;
            }
            set
            {
                this.autoStepinSafeField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool AutoStepinPastClose
        {
            get
            {
                return this.autoStepinPastCloseField;
            }
            set
            {
                this.autoStepinPastCloseField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool LargePotential
        {
            get
            {
                return this.largePotentialField;
            }
            set
            {
                this.largePotentialField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool MaxMatches
        {
            get
            {
                return this.maxMatchesField;
            }
            set
            {
                this.maxMatchesField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool MoreOtherMatches
        {
            get
            {
                return this.moreOtherMatchesField;
            }
            set
            {
                this.moreOtherMatchesField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool OverThreshold
        {
            get
            {
                return this.overThresholdField;
            }
            set
            {
                this.overThresholdField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool Timeout
        {
            get
            {
                return this.timeoutField;
            }
            set
            {
                this.timeoutField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class EngineType
    {
        private bool flattenField;
        private bool flattenFieldSpecified;
        private EngineIntensityType intensityField;
        private bool intensityFieldSpecified;
        private PromptSetType promptSetField;
        private bool promptSetFieldSpecified;
        private string thresholdField;
        private string timeoutField;
        private EngineEnumType valueField;
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Flatten
        {
            get
            {
                return this.flattenField;
            }
            set
            {
                this.flattenField = value;
            }
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FlattenSpecified
        {
            get
            {
                return this.flattenFieldSpecified;
            }
            set
            {
                this.flattenFieldSpecified = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public EngineIntensityType Intensity
        {
            get
            {
                return this.intensityField;
            }
            set
            {
                this.intensityField = value;
            }
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IntensitySpecified
        {
            get
            {
                return this.intensityFieldSpecified;
            }
            set
            {
                this.intensityFieldSpecified = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public PromptSetType PromptSet
        {
            get
            {
                return this.promptSetField;
            }
            set
            {
                this.promptSetField = value;
            }
        }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PromptSetSpecified
        {
            get
            {
                return this.promptSetFieldSpecified;
            }
            set
            {
                this.promptSetFieldSpecified = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "positiveInteger")]
        public string Threshold
        {
            get
            {
                return this.thresholdField;
            }
            set
            {
                this.thresholdField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "nonNegativeInteger")]
        public string Timeout
        {
            get
            {
                return this.timeoutField;
            }
            set
            {
                this.timeoutField = value;
            }
        }
        [System.Xml.Serialization.XmlTextAttribute()]
        public EngineEnumType Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public enum EngineIntensityType
    {
        Exact,
        Close,
        Extensive,
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public enum PromptSetType
    {
        OneLine,
        Default,
        Generic,
        Optimal,
        Alternate,
        Alternate2,
        Alternate3,
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public enum EngineEnumType
    {
        Singleline,
        Typedown,
        Verification,
        Keyfinder,
        Intuitive,
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd" +
        "")]
    public partial class SecurityHeaderType
    {
        private System.Xml.XmlElement[] anyField;
        private System.Xml.XmlAttribute[] anyAttrField;
        [System.Xml.Serialization.XmlAnyElementAttribute()]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03", IsNullable = false)]
    public partial class QAInformation : System.Web.Services.Protocols.SoapHeader
    {
        private string stateTransitionField;
        private long creditsUsedField;
        public string StateTransition
        {
            get
            {
                return this.stateTransitionField;
            }
            set
            {
                this.stateTransitionField = value;
            }
        }
        public long CreditsUsed
        {
            get
            {
                return this.creditsUsedField;
            }
            set
            {
                this.creditsUsedField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QASearch
    {
        private string countryField;
        private EngineType engineField;
        private string layoutField;
        private string searchField;
        private bool formattedAddressInPicklistField;
        private string localisationField;
        private string requestTagField;
        public QASearch()
        {
            this.formattedAddressInPicklistField = false;
        }
        public string Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }
        public EngineType Engine
        {
            get
            {
                return this.engineField;
            }
            set
            {
                this.engineField = value;
            }
        }
        public string Layout
        {
            get
            {
                return this.layoutField;
            }
            set
            {
                this.layoutField = value;
            }
        }
        public string Search
        {
            get
            {
                return this.searchField;
            }
            set
            {
                this.searchField = value;
            }
        }
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool FormattedAddressInPicklist
        {
            get
            {
                return this.formattedAddressInPicklistField;
            }
            set
            {
                this.formattedAddressInPicklistField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Localisation
        {
            get
            {
                return this.localisationField;
            }
            set
            {
                this.localisationField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RequestTag
        {
            get
            {
                return this.requestTagField;
            }
            set
            {
                this.requestTagField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QASearchResult
    {
        private QAPicklistType qAPicklistField;
        private QAAddressType qAAddressField;
        private VerificationFlagsType verificationFlagsField;
        private VerifyLevelType verifyLevelField;
        public QASearchResult()
        {
            this.verifyLevelField = VerifyLevelType.None;
        }
        public QAPicklistType QAPicklist
        {
            get
            {
                return this.qAPicklistField;
            }
            set
            {
                this.qAPicklistField = value;
            }
        }
        public QAAddressType QAAddress
        {
            get
            {
                return this.qAAddressField;
            }
            set
            {
                this.qAAddressField = value;
            }
        }
        public VerificationFlagsType VerificationFlags
        {
            get
            {
                return this.verificationFlagsField;
            }
            set
            {
                this.verificationFlagsField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(VerifyLevelType.None)]
        public VerifyLevelType VerifyLevel
        {
            get
            {
                return this.verifyLevelField;
            }
            set
            {
                this.verifyLevelField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public enum VerifyLevelType
    {
        None,
        Verified,
        InteractionRequired,
        PremisesPartial,
        StreetPartial,
        Multiple,
        VerifiedPlace,
        VerifiedStreet,
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QARefine
    {
        private string monikerField;
        private string refinementField;
        private string layoutField;
        private bool formattedAddressInPicklistField;
        private string thresholdField;
        private string timeoutField;
        private string localisationField;
        private string requestTagField;
        public QARefine()
        {
            this.formattedAddressInPicklistField = false;
        }
        public string Moniker
        {
            get
            {
                return this.monikerField;
            }
            set
            {
                this.monikerField = value;
            }
        }
        public string Refinement
        {
            get
            {
                return this.refinementField;
            }
            set
            {
                this.refinementField = value;
            }
        }
        public string Layout
        {
            get
            {
                return this.layoutField;
            }
            set
            {
                this.layoutField = value;
            }
        }
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool FormattedAddressInPicklist
        {
            get
            {
                return this.formattedAddressInPicklistField;
            }
            set
            {
                this.formattedAddressInPicklistField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "positiveInteger")]
        public string Threshold
        {
            get
            {
                return this.thresholdField;
            }
            set
            {
                this.thresholdField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "nonNegativeInteger")]
        public string Timeout
        {
            get
            {
                return this.timeoutField;
            }
            set
            {
                this.timeoutField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Localisation
        {
            get
            {
                return this.localisationField;
            }
            set
            {
                this.localisationField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RequestTag
        {
            get
            {
                return this.requestTagField;
            }
            set
            {
                this.requestTagField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class Picklist
    {
        private QAPicklistType qAPicklistField;
        public QAPicklistType QAPicklist
        {
            get
            {
                return this.qAPicklistField;
            }
            set
            {
                this.qAPicklistField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QAGetAddress
    {
        private string layoutField;
        private string monikerField;
        private string localisationField;
        private string requestTagField;
        public string Layout
        {
            get
            {
                return this.layoutField;
            }
            set
            {
                this.layoutField = value;
            }
        }
        public string Moniker
        {
            get
            {
                return this.monikerField;
            }
            set
            {
                this.monikerField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Localisation
        {
            get
            {
                return this.localisationField;
            }
            set
            {
                this.localisationField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RequestTag
        {
            get
            {
                return this.requestTagField;
            }
            set
            {
                this.requestTagField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class Address
    {
        private QAAddressType qAAddressField;
        public QAAddressType QAAddress
        {
            get
            {
                return this.qAAddressField;
            }
            set
            {
                this.qAAddressField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QAGetData
    {
        private string localisationField;
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Localisation
        {
            get
            {
                return this.localisationField;
            }
            set
            {
                this.localisationField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QAGetDataMapDetail
    {
        private string dataMapField;
        private string localisationField;
        public string DataMap
        {
            get
            {
                return this.dataMapField;
            }
            set
            {
                this.dataMapField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Localisation
        {
            get
            {
                return this.localisationField;
            }
            set
            {
                this.localisationField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QADataMapDetail
    {
        private LicenceWarningLevel warningLevelField;
        private QALicensedSet[] licensedSetField;
        public LicenceWarningLevel WarningLevel
        {
            get
            {
                return this.warningLevelField;
            }
            set
            {
                this.warningLevelField = value;
            }
        }
        [System.Xml.Serialization.XmlElementAttribute("LicensedSet")]
        public QALicensedSet[] LicensedSet
        {
            get
            {
                return this.licensedSetField;
            }
            set
            {
                this.licensedSetField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QAGetLicenseInfo
    {
        private string localisationField;
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Localisation
        {
            get
            {
                return this.localisationField;
            }
            set
            {
                this.localisationField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QALicenceInfo
    {
        private LicenceWarningLevel warningLevelField;
        private QALicensedSet[] licensedSetField;
        public LicenceWarningLevel WarningLevel
        {
            get
            {
                return this.warningLevelField;
            }
            set
            {
                this.warningLevelField = value;
            }
        }
        [System.Xml.Serialization.XmlElementAttribute("LicensedSet")]
        public QALicensedSet[] LicensedSet
        {
            get
            {
                return this.licensedSetField;
            }
            set
            {
                this.licensedSetField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QAGetSystemInfo
    {
        private string localisationField;
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Localisation
        {
            get
            {
                return this.localisationField;
            }
            set
            {
                this.localisationField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QAGetExampleAddresses
    {
        private string countryField;
        private string layoutField;
        private string localisationField;
        private string requestTagField;
        public string Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }
        public string Layout
        {
            get
            {
                return this.layoutField;
            }
            set
            {
                this.layoutField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Localisation
        {
            get
            {
                return this.localisationField;
            }
            set
            {
                this.localisationField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RequestTag
        {
            get
            {
                return this.requestTagField;
            }
            set
            {
                this.requestTagField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QAGetLayouts
    {
        private string countryField;
        private string localisationField;
        public string Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Localisation
        {
            get
            {
                return this.localisationField;
            }
            set
            {
                this.localisationField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QAGetPromptSet
    {
        private string countryField;
        private EngineType engineField;
        private PromptSetType promptSetField;
        private string localisationField;
        public string Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }
        public EngineType Engine
        {
            get
            {
                return this.engineField;
            }
            set
            {
                this.engineField = value;
            }
        }
        public PromptSetType PromptSet
        {
            get
            {
                return this.promptSetField;
            }
            set
            {
                this.promptSetField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Localisation
        {
            get
            {
                return this.localisationField;
            }
            set
            {
                this.localisationField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QAPromptSet
    {
        private PromptLine[] lineField;
        private bool dynamicField;
        public QAPromptSet()
        {
            this.dynamicField = false;
        }
        [System.Xml.Serialization.XmlElementAttribute("Line")]
        public PromptLine[] Line
        {
            get
            {
                return this.lineField;
            }
            set
            {
                this.lineField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool Dynamic
        {
            get
            {
                return this.dynamicField;
            }
            set
            {
                this.dynamicField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QACanSearch
    {
        private string countryField;
        private EngineType engineField;
        private string layoutField;
        private string localisationField;
        public string Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }
        public EngineType Engine
        {
            get
            {
                return this.engineField;
            }
            set
            {
                this.engineField = value;
            }
        }
        public string Layout
        {
            get
            {
                return this.layoutField;
            }
            set
            {
                this.layoutField = value;
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Localisation
        {
            get
            {
                return this.localisationField;
            }
            set
            {
                this.localisationField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.qas.com/OnDemand-2011-03")]
    public partial class QASearchOk
    {
        private bool isOkField;
        private string errorCodeField;
        private string errorMessageField;
        private string[] errorDetailField;
        public bool IsOk
        {
            get
            {
                return this.isOkField;
            }
            set
            {
                this.isOkField = value;
            }
        }
        public string ErrorCode
        {
            get
            {
                return this.errorCodeField;
            }
            set
            {
                this.errorCodeField = value;
            }
        }
        public string ErrorMessage
        {
            get
            {
                return this.errorMessageField;
            }
            set
            {
                this.errorMessageField = value;
            }
        }
        [System.Xml.Serialization.XmlElementAttribute("ErrorDetail")]
        public string[] ErrorDetail
        {
            get
            {
                return this.errorDetailField;
            }
            set
            {
                this.errorDetailField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void DoSearchCompletedEventHandler(object sender, DoSearchCompletedEventArgs e);
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DoSearchCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        private object[] results;
        internal DoSearchCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }
        public QASearchResult Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((QASearchResult)(this.results[0]));
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void DoRefineCompletedEventHandler(object sender, DoRefineCompletedEventArgs e);
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DoRefineCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        private object[] results;
        internal DoRefineCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }
        public Picklist Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((Picklist)(this.results[0]));
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void DoGetAddressCompletedEventHandler(object sender, DoGetAddressCompletedEventArgs e);
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DoGetAddressCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        private object[] results;
        internal DoGetAddressCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }
        public Address Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((Address)(this.results[0]));
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void DoGetDataCompletedEventHandler(object sender, DoGetDataCompletedEventArgs e);
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DoGetDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        private object[] results;
        internal DoGetDataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }
        public QADataSet[] Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((QADataSet[])(this.results[0]));
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void DoGetDataMapDetailCompletedEventHandler(object sender, DoGetDataMapDetailCompletedEventArgs e);
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DoGetDataMapDetailCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        private object[] results;
        internal DoGetDataMapDetailCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }
        public QADataMapDetail Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((QADataMapDetail)(this.results[0]));
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void DoGetLicenseInfoCompletedEventHandler(object sender, DoGetLicenseInfoCompletedEventArgs e);
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DoGetLicenseInfoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        private object[] results;
        internal DoGetLicenseInfoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }
        public QALicenceInfo Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((QALicenceInfo)(this.results[0]));
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void DoGetSystemInfoCompletedEventHandler(object sender, DoGetSystemInfoCompletedEventArgs e);
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DoGetSystemInfoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        private object[] results;
        internal DoGetSystemInfoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }
        public string[] Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void DoGetExampleAddressesCompletedEventHandler(object sender, DoGetExampleAddressesCompletedEventArgs e);
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DoGetExampleAddressesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        private object[] results;
        internal DoGetExampleAddressesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }
        public QAExampleAddress[] Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((QAExampleAddress[])(this.results[0]));
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void DoGetLayoutsCompletedEventHandler(object sender, DoGetLayoutsCompletedEventArgs e);
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DoGetLayoutsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        private object[] results;
        internal DoGetLayoutsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }
        public QALayout[] Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((QALayout[])(this.results[0]));
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void DoGetPromptSetCompletedEventHandler(object sender, DoGetPromptSetCompletedEventArgs e);
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DoGetPromptSetCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        private object[] results;
        internal DoGetPromptSetCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }
        public QAPromptSet Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((QAPromptSet)(this.results[0]));
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void DoCanSearchCompletedEventHandler(object sender, DoCanSearchCompletedEventArgs e);
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DoCanSearchCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        private object[] results;
        internal DoCanSearchCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }
        public QASearchOk Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((QASearchOk)(this.results[0]));
            }
        }
    }
}