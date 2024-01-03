using AggreedRouteXSD;
using STP.Common.Constants;
using STP.Common.Logger;
using STP.DocumentsAndContents.Common;
using STP.DocumentsAndContents.Persistance;
using STP.Domain.DocumentsAndContents;
using STP.Domain.LoggingAndReporting;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.RouteAssessment;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace STP.DocumentsAndContents.Document
{
    public class GenerateDocument
    {
        #region GenrateSODocument
        public byte[] GenrateSODocument(Enums.SOTemplateType templatetype, string esDALRefNo, string orderNumber, UserInfo userInfo = null, bool generateFlag = true)//added generateFlag flag for generate document in order to solve redmine #4959
        {
            string docFileName = GetFileName(templatetype, orderNumber);
            int docType = 322010;
            string xsltPath = GetXsltPath(templatetype);

            GenerateXML gxml = new GenerateXML();

            XMLModel model = gxml.GenerateSpecialOrderXML(templatetype, esDALRefNo, orderNumber, userInfo.UserSchema);

            OutboundDocuments outbounddocs = CommonMethods.GetOutboundDocMetaDataDetails(esDALRefNo, userInfo.UserSchema);

            //byte[] exportByteArrayData = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, docFileName, null, userInfo, 277001, false, false, 692001, generateFlag);//added generateFlag flag for generate document in order to solve redmine #4959
            GenerateEmailgetParams emailgetParams = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, docFileName, null, userInfo, 277001, false, false, 692001, generateFlag);//added generateFlag flag for generate document in order to solve redmine #4959
            byte[] exportByteArrayData = Encoding.UTF8.GetBytes(emailgetParams.HtmlContent);
            return exportByteArrayData;
        }

        #endregion

        #region Private Methods
        private string GetFileName(Enums.SOTemplateType templatetype, string orderNumber)
        {
            string docname = "";
            switch (templatetype)
            {
                case Enums.SOTemplateType.SO2D4:
                    docname = "2D4-SpecialOrder";
                    break;
                case Enums.SOTemplateType.SO2D4a:
                    docname = "2D4A-SpecialOrder";
                    break;
                case Enums.SOTemplateType.SO2D4b:
                    docname = "2D4-SpecialOrder";
                    break;
                case Enums.SOTemplateType.SO2D7:
                    docname = "2D7-SpecialOrder";
                    break;
                case Enums.SOTemplateType.SO2D7a:
                    docname = "2D7a-SpecialOrder";
                    break;
                case Enums.SOTemplateType.SO2D1:
                    docname = "2D1-SpecialOrder";
                    break;
                case Enums.SOTemplateType.SO2D5:
                    docname = "2D5-SpecialOrder";
                    break;
                case Enums.SOTemplateType.SO2D8:
                    docname = "2D8-SpecialOrder";
                    break;
                case Enums.SOTemplateType.SO2D9:
                    docname = "2D9-SpecialOrder";
                    break;
            }
            return docname + "_" + orderNumber;
        }

        private string GetXsltPath(Enums.SOTemplateType templatetype)
        {
            string xsltpath = null;
            switch (templatetype)
            {
                case Enums.SOTemplateType.SO2D4:
                    xsltpath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\2D4.xsl";
                    break;
                case Enums.SOTemplateType.SO2D4a:
                    xsltpath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\2D4A.xsl";
                    break;
                case Enums.SOTemplateType.SO2D4b:
                    xsltpath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\2D4B.xsl";
                    break;
                case Enums.SOTemplateType.SO2D7:
                    xsltpath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\2D7.xsl";
                    break;
                case Enums.SOTemplateType.SO2D7a:
                    xsltpath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\2D7a.xslt";
                    break;
                case Enums.SOTemplateType.SO2D1:
                    xsltpath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\2D1.xsl";
                    break;
                case Enums.SOTemplateType.SO2D5:
                    xsltpath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\2D5.xslt";
                    break;
                case Enums.SOTemplateType.SO2D8:
                    xsltpath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\2D8.xslt";
                    break;
                case Enums.SOTemplateType.SO2D9:
                    xsltpath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\2D9.xslt";
                    break;
            }
            return xsltpath;
        }

        private string ConvertSpecialCharactersToLetters(string xmlDocument)
        {
            xmlDocument = xmlDocument.Replace("£", "#@GBP@#");
            xmlDocument = xmlDocument.Replace("€", "#@Pound@#");
            xmlDocument = xmlDocument.Replace("₾", "#@GEL@#");
            xmlDocument = xmlDocument.Replace("лв", "#@BGN@#");
            xmlDocument = xmlDocument.Replace("₺", "#@Turkey@#");
            return xmlDocument;
        }

        #endregion

        #region Code Commented By Mahzeer on 12/07/2023
        /*
        public List<byte[]> OldGenerateEsdalReNotification(int notificationId, int contactId, Dictionary<int, int> icaStatusDictionary, string ImminentMovestatus = "No imminent movement", UserInfo userInfo = null)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("GenerateEsdalReNotification started successfully with parameters Notifid : {0}, ContactId : {1}", notificationId, contactId));
                string docFileName = "";

                int docType = 322003;

                int icaStatus = 277001;

                Byte[] outputValue = null;

                List<byte[]> docarray = new List<byte[]>();

                GenerateXML gxml = new GenerateXML();

                string xsltPath = "";

                #region using netwebs code portion to save outbound document

                XMLModel model = gxml.GenerateReNotificationFaxSOAXML(Enums.PortalType.SOA, notificationId, false, contactId);

                OutboundDocuments outbounddocs = CommonMethods.GetNotificationDetails(notificationId);

                CommonMethods.GetRecipientDetails(model.ReturnXML);

                model.ReturnXML = ConvertSpecialCharactersToLetters(model.ReturnXML);

                byte[] byteArrayData = Encoding.ASCII.GetBytes(model.ReturnXML);

                //Some data is stored in gzip format, so we need to unzip then load it.
                byte[] CompressData = STP.Common.General.XsltTransformer.CompressData(byteArrayData);

                #endregion

                #region Code part to generate SOA related Document and send mail/fax/online inbox

                string xsltSOAPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\ReNotification.xslt";

                List<ContactModel> contactSOAList = CommonMethods.GetRecipientDetails(model.ReturnXML);
                string[] imminentStateArr = ImminentMovestatus.Split(',');
                bool ImminentState = false;
                //Soa related code part
                foreach (ContactModel objcontact in contactSOAList)
                {

                    try
                    {
                        if (objcontact.Reason == "no longer affected")
                        {
                            continue;
                        }
                        docFileName = "RenotificationFaxSOA";

                        xsltPath = xsltSOAPath;//storing the police related XSLT path

                        string[] contactDet = null;
                        //GenerateReProposalNolongeraffectedFaxSOADocument
                        if (objcontact.ContactId != 0)
                        {
                            //function that returns contact's details in a string array
                            contactDet = RouteAssessmentDAO.FetchContactPreference(objcontact.ContactId, UserSchema.Portal);

                            objcontact.OrganisationId = objcontact.OrganisationId == 0 ? Convert.ToInt32(contactDet[4]) : objcontact.OrganisationId; // saving organisation Id into the NotificationContacts object

                            if (ImminentMovestatus != "No imminent movement" && ImminentMovestatus != null && ImminentMovestatus != "" && (imminentStateArr[0] == "Imminent movement for SOA." || imminentStateArr[0] == "Imminent movement for SOA and police." || imminentStateArr[0] == "Imminent movement."))
                            {
                                //Get imminent status to send mail to that organisation affected parties
                                ImminentState = ImminentMoveAlertDAO.GetImminentForCountries(objcontact.OrganisationId, ImminentMovestatus);
                            }
                            if (icaStatusDictionary != null)
                            {
                                if (icaStatusDictionary.ContainsKey(objcontact.OrganisationId))
                                {
                                    icaStatus = icaStatusDictionary[objcontact.OrganisationId];
                                }
                            }
                        }
                        else
                        {
                            contactDet = new string[6];
                            if (!objcontact.ISPolice) // For RM#4340
                            {

                                contactDet[3] = "soa"; // Condition - When Haulier with ContactId,OrganisationId = 0, ISPolice= false then SOA document.

                                contactDet[0] = "Email";

                                contactDet[5] = "0";

                                contactDet[1] = objcontact.Email;

                            }
                        }
                        if (contactDet[3] == "soa")
                        {
                            //condition to generate Email document and send for the following preferences
                            if (contactDet[0] == "Email" || contactDet[0] == "Online inbox plus email (HTML)")
                            {
                                objcontact.Email = contactDet[1];
                                if (contactDet[5] == "1")
                                {
                                    //outputValue = CommonMethods.GenerateEMAIL(notificationId, docType, model.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, objcontact, docFileName, true, userInfo, icaStatus, false, 1, ImminentState);
                                }
                                else
                                {
                                    //outputValue = CommonMethods.GenerateEMAIL(notificationId, docType, model.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, objcontact, docFileName, true, userInfo, icaStatus, false, 0, ImminentState);
                                }
                            }
                            else if (contactDet[0] == "Fax") //condition to generate Fax document and send for the following preferences
                            {
                                objcontact.Fax = contactDet[2];
                                //outputValue = CommonMethods.GenerateNotificationPDF(notificationId, docType, model.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, objcontact, docFileName, userInfo, icaStatus, false, ImminentState);
                            }

                            else if (contactDet[0] == "Online Inbox Only") //condition to generate document and save it on online inbox
                            {
                                //outputValue = CommonMethods.GenerateWord(notificationId, docType, model.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, docFileName, objcontact, userInfo, icaStatus, false, ImminentState);
                                //imminent movment alert
                                if (ImminentState)//!= "No imminent movement" && ImminentState != null && ImminentState != ""
                                {
                                    //disabling imminent movement alert.
                                }
                            }
                            docarray.Add(outputValue);
                            ImminentState = false;
                        }
                    }
                    catch (Exception e)
                    {
                        ImminentState = false;
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("GenerateDocument/GenerateEsdalReNotification SOA related code, Exception: {0}", e));
                    }
                }
                #endregion

                #region Code part to generate Police related notification Document and send mail/fax/online inbox

                string xsltPolicePath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Re_Notification_Fax_Police.xslt";

                XMLModel modelPolice = gxml.GenerateReNotificationFaxSOAXML(Enums.PortalType.POLICE, notificationId, false, contactId);

                List<ContactModel> contactPoliceList = CommonMethods.GetRecipientDetails(modelPolice.ReturnXML);
                //police related code part
                foreach (ContactModel objcontact in contactPoliceList)
                {

                    try
                    {
                        if (objcontact.Reason == "no longer affected")
                        {
                            continue;
                        }

                        docFileName = "RenotificationFaxPolice";

                        xsltPath = xsltPolicePath; //storing the police related XSLT path

                        string[] contactDet = null;

                        if (objcontact.ContactId != 0)
                        {
                            //function that returns contact's details in a string array
                            contactDet = RouteAssessmentDAO.FetchContactPreference(objcontact.ContactId, UserSchema.Portal);

                            objcontact.OrganisationId = objcontact.OrganisationId == 0 ? Convert.ToInt32(contactDet[4]) : objcontact.OrganisationId; // saving organisation Id into the NotificationContacts object
                            if (ImminentMovestatus != "No imminent movement" && ImminentMovestatus != null && ImminentMovestatus != "" && (imminentStateArr[0] == "Imminent movement for police." || imminentStateArr[0] == "Imminent movement for SOA and police." || imminentStateArr[0] == "Imminent movement."))
                            {
                                //Get imminent status to send mail to that organisation affected parties
                                ImminentState = ImminentMoveAlertDAO.GetImminentForCountries(objcontact.OrganisationId, ImminentMovestatus);
                            }
                        }
                        else // condition to fetch manually added parties and send police document
                        {
                            contactDet = new string[6];
                            if (objcontact.ISPolice) //For RM#4340 changes for If Condition only 
                            {
                                contactDet[3] = "police";// send the police document to any manually added party

                                contactDet[0] = (objcontact.Email != null && objcontact.Email != "") ? "Email" : "Fax";

                                contactDet[1] = objcontact.Email;

                                contactDet[2] = objcontact.Fax;
                            }
                        }

                        //condition to check whether the contact is police or not
                        if (contactDet[3] == "police")
                        {

                            if (contactDet[0] == "Email" || contactDet[0] == "Online inbox plus email (HTML)")
                            {
                                objcontact.Email = contactDet[1];
                                //outputValue = CommonMethods.GenerateEMAIL(notificationId, docType, modelPolice.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, objcontact, docFileName, true, userInfo, 277001, false, 0, ImminentState);
                            }
                            else if (contactDet[0] == "Fax")
                            {
                                objcontact.Fax = contactDet[2];
                                //outputValue = CommonMethods.GenerateNotificationPDF(notificationId, docType, modelPolice.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, objcontact, docFileName, userInfo, 277001, false, ImminentState);
                            }
                            else if (contactDet[0] == "Online Inbox Only")
                            {
                                //outputValue = CommonMethods.GenerateWord(notificationId, docType, modelPolice.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, docFileName, objcontact, userInfo, 277001, false, ImminentState);

                                //Imminent movement alert
                                if (ImminentState)
                                {
                                    //disabling imminent movement alert.
                                }
                            }
                            docarray.Add(outputValue);

                        }
                    }

                    catch (Exception e)
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("GenerateDocument/GenerateEsdalReNotification Police related code, Exception: {0}", e));
                    }
                }
                #endregion

                if (docarray != null)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "GenerateEsdalReNotification completed successfully");
                }
                return docarray;
            }
            catch (Exception e)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("GenerateDocument/GenerateEsdalReNotification, Exception: {0}", e));
                return null;
            }
        }

        public List<byte[]> GenerateSORevisedAgreementFAXDocument(Enums.DocumentType doctype, string esDALRefNo, string order_no, int contactId = 0)
        {
            CommonMethods commonMethods = new CommonMethods();

            string docFileName = "RevisedAgreement";
            int docType = 322002;

            Byte[] outputValue = null;

            List<byte[]> docarray = new List<byte[]>();

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateRevisedAgreementXML(Enums.PortalType.SOA, esDALRefNo, order_no, contactId);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\RevisedAgreement.xslt";

            OutboundDocuments outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo);

            List<ContactModel> contactList = CommonMethods.GetRecipientDetails(model.ReturnXML);

            foreach (ContactModel objcontact in contactList)
            {
                if (doctype == Enums.DocumentType.PDF)
                {
                    outputValue = commonMethods.GeneratePDF(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, objcontact.ContactId, docFileName);
                }
                else if (doctype == Enums.DocumentType.EMAIL)
                {
                    //outputValue = CommonMethods.GenerateEMAIL(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, objcontact, docFileName, false);
                }
                docarray.Add(outputValue);
            }

            return docarray;
        }

        public List<byte[]> GenerateSORevisedAgreementPoliceDocument(Enums.DocumentType doctype, string esDALRefNo, string order_no, int contactId = 0)
        {
            CommonMethods commonMethods = new CommonMethods();
            string docFileName = "RevisedAgreementPolice";
            int docType = 322002;

            Byte[] outputValue = null;

            List<byte[]> docarray = new List<byte[]>();

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateRevisedAgreementXML(Enums.PortalType.POLICE, esDALRefNo, order_no, contactId);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\RevisedAgreementFaxPolice.xslt";

            OutboundDocuments outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo);

            List<ContactModel> contactList = CommonMethods.GetRecipientDetails(model.ReturnXML);

            foreach (ContactModel objcontact in contactList)
            {
                if (doctype == Enums.DocumentType.PDF)
                {
                    outputValue = commonMethods.GeneratePDF(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, objcontact.ContactId, docFileName);
                }
                else if (doctype == Enums.DocumentType.EMAIL)
                {
                    //outputValue = CommonMethods.GenerateEMAIL(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, objcontact, docFileName, false);
                }

                docarray.Add(outputValue);
            }

            return docarray;
        }

        public List<byte[]> GenerateSOReclearedAgreementFAXPoliceDocument(Enums.DocumentType doctype, string esDALRefNo, string order_no, int contactId = 0)
        {
            string docFileName = "ReclearedAgreementFaxPolice";
            int docType = 322002;
            CommonMethods commonMethods = new CommonMethods();

            Byte[] outputValue = null;

            List<byte[]> docarray = new List<byte[]>();

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateAgreedRoutetXML(Enums.PortalType.POLICE, esDALRefNo, order_no, contactId);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ReClearedAgreementFaxPolice.xslt";

            OutboundDocuments outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo);

            List<ContactModel> contactList = CommonMethods.GetRecipientDetails(model.ReturnXML);

            foreach (ContactModel objcontact in contactList)
            {
                if (doctype == Enums.DocumentType.PDF)
                {
                    outputValue = commonMethods.GeneratePDF(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, objcontact.ContactId, docFileName);
                }
                else if (doctype == Enums.DocumentType.EMAIL)
                {
                    //outputValue = CommonMethods.GenerateEMAIL(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, objcontact, docFileName, false);
                }

                docarray.Add(outputValue);
            }

            return docarray;
        }

        public List<byte[]> GenerateSOReclearedAgreementFaxSOADocument(Enums.DocumentType doctype, string esDALRefNo, string order_no, int contactId = 0)
        {
            string docFileName = "ReclearedAgreementFaxSOA";
            int docType = 322002;
            CommonMethods commonMethods = new CommonMethods();
            Byte[] outputValue = null;

            List<byte[]> docarray = new List<byte[]>();

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateAgreedRoutetXML(Enums.PortalType.POLICE, esDALRefNo, order_no, contactId);


            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Re_ClearedAgreementFax_SOA.xslt";


            OutboundDocuments outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo);

            List<ContactModel> contactList = CommonMethods.GetRecipientDetails(model.ReturnXML);

            foreach (ContactModel objcontact in contactList)
            {
                if (doctype == Enums.DocumentType.PDF)
                {
                    outputValue = commonMethods.GeneratePDF(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, objcontact.ContactId, docFileName);
                }
                else if (doctype == Enums.DocumentType.EMAIL)
                {
                    //outputValue = CommonMethods.GenerateEMAIL(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, objcontact, docFileName, false);
                }

                docarray.Add(outputValue);
            }

            return docarray;
        }

        public List<byte[]> GenerateAgreedRouteSOADocument(Enums.DocumentType doctype, string esDALRefNo, string order_no, int contactId = 0)
        {
            string docFileName = "AgreedRouteSOA";
            int docType = 322010;
            CommonMethods commonMethods = new CommonMethods();

            Byte[] outputValue = null;

            List<byte[]> docarray = new List<byte[]>();

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateAgreedRoutetXML(Enums.PortalType.SOA, esDALRefNo, order_no, contactId);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\SpecialOrderAgreedRoute.xslt";

            OutboundDocuments outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo);

            List<ContactModel> contactList = CommonMethods.GetRecipientDetails(model.ReturnXML);

            foreach (ContactModel objcontact in contactList)
            {
                if (doctype == Enums.DocumentType.PDF)
                {
                    outputValue = commonMethods.GeneratePDF(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, objcontact.ContactId, docFileName);
                }
                else if (doctype == Enums.DocumentType.EMAIL)
                {
                    //outputValue = CommonMethods.GenerateEMAIL(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, objcontact, docFileName, false);
                    CommonMethods.GenerateEMAIL(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, objcontact, docFileName, false);
                }

                docarray.Add(outputValue);
            }
            return docarray;
        }

        public List<byte[]> GenerateDailyDigestFaxDocument(Enums.DocumentType doctype, int contactId, string notificationId)
        {
            string docFileName = "DailyDigest";

            int docType = 322010;

            Byte[] outputValue = null;

            List<byte[]> docarray = new List<byte[]>();
            CommonMethods commonMethods = new CommonMethods();

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateDailyDigestFAXXML(contactId, notificationId);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\DailyDigestFax.xslt";

            OutboundDocuments outbounddocs = CommonMethods.GetNotificationDetails(Convert.ToInt32(model.NotificationID));

            List<ContactModel> contactList = CommonMethods.GetRecipientDetails(model.ReturnXML);

            foreach (ContactModel objcontact in contactList)
            {
                if (doctype == Enums.DocumentType.EMAIL)
                {
                    //outputValue = CommonMethods.GenerateEMAIL(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, objcontact, docFileName, false);
                }
                else
                {
                    outputValue = commonMethods.GeneratePDF(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, objcontact.ContactId, docFileName);
                }

                docarray.Add(outputValue);
            }

            return docarray;
        }

        public List<byte[]> GenerateImminentMoveAlertDocument(int contactId, int notificationId, int loggedInContactId, bool ImminentMovestatus = true, UserInfo userInfo = null)
        {
            string docFileName = "ImmenientMoveAlertNotificationFax";

            int docType = 322006;

            Byte[] outputValue = null;

            List<byte[]> docarray = new List<byte[]>();

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateImminentMoveAlertXML(contactId, notificationId, loggedInContactId);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Notification_Alert_Fax.xslt";

            OutboundDocuments outbounddocs = CommonMethods.GetNotificationDetails(notificationId);

            List<ContactModel> contactList = CommonMethods.GetRecipientDetails(model.ReturnXML);

            foreach (ContactModel objcontact in contactList)
            {
                string[] contactDet = null;

                if (objcontact.ContactId != 0)
                {
                    //function that returns contact's details in a string array
                    contactDet = RouteAssessmentDAO.FetchContactPreference(objcontact.ContactId, UserSchema.Portal);

                    objcontact.OrganisationId = objcontact.OrganisationId == 0 ? Convert.ToInt32(contactDet[4]) : objcontact.OrganisationId; // saving organisation Id into the NotificationContacts object
                }
                else
                {
                    contactDet = new string[6];

                    contactDet[3] = "police";// send the police document to any manually added party

                    contactDet[0] = (objcontact.Email != null && objcontact.Email != "") ? "Email" : "Fax";

                    contactDet[1] = objcontact.Email;

                    contactDet[2] = objcontact.Fax;
                }


                if (contactDet[3] == null || contactDet[3] == "")
                {
                    contactDet[3] = "police";// send the police document to any manually added party 
                }

                try
                {

                    if (contactDet[3] == "soa")
                    {
                        //condition to generate Email document and send for the following preferences inbox preference is also considered as online inbox
                        if (contactDet[0] == "Online Inbox Only" && (contactDet[1] != "" || contactDet[1] != null))
                        {
                            objcontact.Email = contactDet[1];
                        }
                        else if (contactDet[0] == "Online Inbox Only" && (contactDet[2] != "" || contactDet[2] != null)) //condition to generate Fax document and send for the following preferences
                        {
                            objcontact.Fax = contactDet[2];
                        }
                    }
                    else if (contactDet[3] == "police")
                    {
                        if (contactDet[0] == "Online Inbox Only" && (contactDet[1] != "" || contactDet[1] != null))
                        {
                            objcontact.Email = contactDet[1];
                        }
                        else if (contactDet[0] == "Online Inbox Only" && (contactDet[2] != "" || contactDet[2] != null))
                        {
                            objcontact.Fax = contactDet[2];
                        }
                    }
                }
                catch
                {
                    // do nothing
                }
                docarray.Add(outputValue);
            }

            return docarray;
        }

        public string GetRouteDescriptionInformation(string orderNo, string esDAlRefNo)
        {
            string returnXML = string.Empty;

            byte[] byteXML = RevisedAgreementDAO.GetRouteDescription(orderNo, esDAlRefNo);


            string outboundXMLInformation = string.Empty;

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                outboundXMLInformation = Encoding.UTF8.GetString(byteXML, 0, byteXML.Length);

                xmlDoc.LoadXml(outboundXMLInformation);
            }
            catch
            {
                //Some data is stored in gzip format, so we need to unzip then load it.
                byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(byteXML);

                outboundXMLInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);

                xmlDoc.LoadXml(outboundXMLInformation);
            }

            returnXML = xmlDoc.OuterXml;

            return returnXML;
        }
       
        public List<byte[]> GenerateSOProposalFaxDocument(Enums.DocumentType doctype, string esDALRefNo, int organisationID, int contactId)
        {
            string docFileName = "SpecialOrderProposal";
            int docType = 322001;

            doctype = Enums.DocumentType.EMAIL;

            Byte[] outputValue = null;
            CommonMethods commonMethods = new CommonMethods();

            List<byte[]> docarray = new List<byte[]>();

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateProposalXML(esDALRefNo, organisationID, contactId);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\SpecialOrderProposal.xslt";

            OutboundDocuments outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo);

            List<ContactModel> contactList = CommonMethods.GetRecipientDetails(model.ReturnXML);

            foreach (ContactModel objcontact in contactList)
            {
                if (doctype == Enums.DocumentType.EMAIL)
                {
                    //outputValue = CommonMethods.GenerateEMAIL(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, objcontact, docFileName, false);
                    CommonMethods.GenerateEMAIL(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, objcontact, docFileName, false);
                }
                else if (doctype == Enums.DocumentType.PDF)
                {
                    outputValue = commonMethods.GeneratePDF(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, objcontact.ContactId, docFileName);
                }

                docarray.Add(outputValue);
            }

            return docarray;
        }

        public List<byte[]> GenerateSOProposalFaxPOLICEDocument(Enums.DocumentType doctype, string esDALRefNo, int organisationID, int contactId)
        {
            string docFileName = "ProposalFaxPolice";
            int docType = 322001;
            Byte[] outputValue = null;
            List<byte[]> docarray = new List<byte[]>();
            CommonMethods commonMethods = new CommonMethods();

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateProposalXML(esDALRefNo, organisationID, contactId);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ProposalForPolice.xslt";

            OutboundDocuments outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo);

            List<ContactModel> contactList = CommonMethods.GetRecipientDetails(model.ReturnXML);

            foreach (ContactModel objcontact in contactList)
            {
                if (doctype == Enums.DocumentType.EMAIL)
                {
                    //outputValue = CommonMethods.GenerateEMAIL(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, objcontact, docFileName, false);
                }
                else
                {
                    outputValue = commonMethods.GeneratePDF(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, objcontact.ContactId, docFileName);
                }

                docarray.Add(outputValue);
            }
            return docarray;
        }

        public List<byte[]> GenerateSOProposalFaxSOADocument(Enums.DocumentType doctype, string esDALRefNo, int organisationID, int contactId)
        {
            CommonMethods commonMethods = new CommonMethods();

            string docFileName = "ProposalFaxSOA";
            int docType = 322001;

            Byte[] outputValue = null;

            List<byte[]> docarray = new List<byte[]>();

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateProposalXML(esDALRefNo, organisationID, contactId);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ProposalFaxSOA.xslt";

            OutboundDocuments outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo);

            List<ContactModel> contactList = CommonMethods.GetRecipientDetails(model.ReturnXML);

            foreach (ContactModel objcontact in contactList)
            {
                if (doctype == Enums.DocumentType.EMAIL)
                {
                    //outputValue = CommonMethods.GenerateEMAIL(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, objcontact, docFileName, false);
                }
                else
                {
                    outputValue = commonMethods.GeneratePDF(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, objcontact.ContactId, docFileName);
                }

                docarray.Add(outputValue);
            }
            return docarray;
        }

        public List<byte[]> GenerateReProposalStillaffectedFaxSOADocument(int ProjectID, Enums.PortalType portalType, int contactId, int versionNo)
        {
            CommonMethods commonMethods = new CommonMethods();

            string docFileName = "ReProposalStillAffectedFaxSOA";
            int docType = 322001;

            Byte[] outputValue = null;

            List<byte[]> docarray = new List<byte[]>();

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateReProposalStillAffectedFAXSOAXML(ProjectID, portalType, contactId, versionNo);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ReProposalStillAffectedFax.xslt";

            OutboundDocuments outbounddocs = CommonMethods.GetNotificationDetails(Convert.ToInt32(model.NotificationID));

            List<ContactModel> contactList = CommonMethods.GetRecipientDetails(model.ReturnXML);

            foreach (ContactModel objcontact in contactList)
            {
                outputValue = commonMethods.GeneratePDF(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, objcontact.ContactId, docFileName);
                docarray.Add(outputValue);
            }

            return docarray;
        }

        public List<byte[]> GenerateReProposalStillaffectedFaxPoliceDocument(int ProjectID, Enums.PortalType portalType, int contactId, int versionNo)
        {
            string docFileName = "ReProposalStillAffectedFaxPolice";
            int docType = 322001;

            Byte[] outputValue = null;

            List<byte[]> docarray = new List<byte[]>();
            CommonMethods commonMethods = new CommonMethods();

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateReProposalStillAffectedFAXSOAXML(ProjectID, portalType, contactId, versionNo);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ReProposalstillaffected_Fax_Police.xslt";

            OutboundDocuments outbounddocs = CommonMethods.GetNotificationDetails(Convert.ToInt32(model.NotificationID));

            List<ContactModel> contactList = CommonMethods.GetRecipientDetails(model.ReturnXML);

            foreach (ContactModel objcontact in contactList)
            {
                outputValue = commonMethods.GeneratePDF(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, objcontact.ContactId, docFileName);
                docarray.Add(outputValue);
            }

            return docarray;
        }

        public List<byte[]> GenerateReProposalNolongeraffectedFaxSOADocument(int projectId, Enums.DocumentType doctype, int contactId, int versionNo, long organisationId)
        {
            string docFileName = "ReProposalNolongerAffectedFaxSOA";
            int docType = 322007;

            Byte[] outputValue = null;

            List<byte[]> docarray = new List<byte[]>();

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateReProposalNoLongerAffectedFAXSOAXML(projectId, Enums.PortalType.SOA, contactId, versionNo);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Re_Proposal_No_Longer_FAX_SOA.xsl";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(model.ReturnXML);
            XmlNodeList parentNode = xmlDoc.GetElementsByTagName("EMRN");
            CommonMethods commonMethods = new CommonMethods();

            List<ContactModel> contactList = CommonMethods.GetRecipientDetails(model.ReturnXML);
            string EsdalRefNumber = string.Empty;

            foreach (XmlNode childrenNode in parentNode)
            {
                EsdalRefNumber = childrenNode["Mnemonic"].InnerText + "/";
                EsdalRefNumber += childrenNode["MovementProjectNumber"].InnerText.ToUpper().Replace("S", "") + "/";
                EsdalRefNumber += "S";// For special order
                EsdalRefNumber += childrenNode["MovementVersion"].InnerText;
            }

            foreach (ContactModel objContact in contactList)
            {
                if (doctype == Enums.DocumentType.PDF)
                {
                    outputValue = commonMethods.GeneratePDF(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, EsdalRefNumber, organisationId, objContact.ContactId, docFileName);
                }
                else if (doctype == Enums.DocumentType.EMAIL)
                {
                    //outputValue = CommonMethods.GenerateEMAIL(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, EsdalRefNumber, organisationId, objContact, docFileName, false);
                }

                docarray.Add(outputValue);
            }

            return docarray;
        }

        public List<byte[]> GenerateReProposalNolongeraffectedFaxPoliceDocument(int projectId, Enums.DocumentType doctype, int contactId, int versionNo, long organisationId)
        {
            string docFileName = "ReProposalNolongerAffectedFaxPolice";
            int docType = 322007;

            Byte[] outputValue = null;

            List<byte[]> docarray = new List<byte[]>();

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateReProposalNoLongerAffectedFAXSOAXML(projectId, Enums.PortalType.POLICE, contactId, versionNo);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ReProposalNoLongerFAXPolice.xslt";
            CommonMethods commonMethods = new CommonMethods();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(model.ReturnXML);
            XmlNodeList parentNode = xmlDoc.GetElementsByTagName("EMRN");

            string EsdalRefNumber = string.Empty;

            foreach (XmlNode childrenNode in parentNode)
            {
                EsdalRefNumber = childrenNode["Mnemonic"].InnerText + "/";
                EsdalRefNumber += childrenNode["MovementProjectNumber"].InnerText.ToUpper().Replace("S", "") + "/";
                EsdalRefNumber += "S";// For special order
                EsdalRefNumber += childrenNode["MovementVersion"].InnerText;
            }

            List<ContactModel> contactList = CommonMethods.GetRecipientDetails(model.ReturnXML);

            foreach (ContactModel objcontact in contactList)
            {
                if (doctype == Enums.DocumentType.PDF)
                {
                    outputValue = commonMethods.GeneratePDF(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, EsdalRefNumber, organisationId, objcontact.ContactId, docFileName);
                }
                else if (doctype == Enums.DocumentType.EMAIL)
                {
                    //outputValue = CommonMethods.GenerateEMAIL(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, EsdalRefNumber, organisationId, objcontact, docFileName, false);
                }

                docarray.Add(outputValue);
            }

            return docarray;
        }

        public byte[] Generate2D4SODocument(Enums.SOTemplateType templatetype, string orderNumber, string esDALRefNo)
        {
            string docFileName = "2D4-SpecialOrder";
            int docType = 322010;
            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\2D4.xsl";

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateSpecialOrderXML(templatetype, esDALRefNo, orderNumber);

            OutboundDocuments outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo);

            //byte[] exportByteArrayData = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, docFileName);
            GenerateEmailgetParams emailgetParams = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, docFileName);
            byte[] exportByteArrayData = Encoding.UTF8.GetBytes(emailgetParams.HtmlContent);
            return exportByteArrayData;
        }

        public byte[] Generate2D4aSODocument(Enums.DocumentType doctype, string esDALRefNo)
        {
            string docFileName = "2D4A-SpecialOrder";
            int docType = 322010;
            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\2D4A.xsl";

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateSpecialOrderXML(Enums.SOTemplateType.SO2D4a, esDALRefNo);

            OutboundDocuments outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo);

            //byte[] exportByteArrayData = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, docFileName);
            GenerateEmailgetParams emailgetParams = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, docFileName);
            byte[] exportByteArrayData = Encoding.UTF8.GetBytes(emailgetParams.HtmlContent);
            return exportByteArrayData;
        }

        public byte[] Generate2D4bSODocument(Enums.DocumentType doctype, string esDALRefNo)
        {
            string docFileName = "2D4B-SpecialOrder";
            int docType = 322010;
            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\2D4B.xsl";

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateSpecialOrderXML(Enums.SOTemplateType.SO2D4b, esDALRefNo);

            OutboundDocuments outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo);

            //byte[] exportByteArrayData = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, docFileName);
            GenerateEmailgetParams emailgetParams = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, docFileName);
            byte[] exportByteArrayData = Encoding.UTF8.GetBytes(emailgetParams.HtmlContent);
            return exportByteArrayData;
        }

        public byte[] Generate2D7SODocument(Enums.DocumentType doctype, string esDALRefNo)
        {
            string docFileName = "2D7-SpecialOrder";
            int docType = 322010;
            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\2D7.xsl";

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateSpecialOrderXML(Enums.SOTemplateType.SO2D7, esDALRefNo);

            OutboundDocuments outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo);

            //byte[] exportByteArrayData = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, docFileName);
            GenerateEmailgetParams emailgetParams = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, docFileName);
            byte[] exportByteArrayData = Encoding.UTF8.GetBytes(emailgetParams.HtmlContent);
            return exportByteArrayData;
        }

        public byte[] Generate2D7aSODocument(Enums.DocumentType doctype, string esDALRefNo)
        {
            string docFileName = "2D7a-SpecialOrder";
            int docType = 322010;
            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\2D7a.xslt";

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateSpecialOrderXML(Enums.SOTemplateType.SO2D7a, esDALRefNo);

            OutboundDocuments outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo);

            //byte[] exportByteArrayData = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, docFileName);
            GenerateEmailgetParams emailgetParams = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, docFileName);
            byte[] exportByteArrayData = Encoding.UTF8.GetBytes(emailgetParams.HtmlContent);
            return exportByteArrayData;
        }

        public byte[] Generate2D1SODocument(Enums.DocumentType doctype, string esDALRefNo)
        {
            string docFileName = "2D1-SpecialOrder";
            int docType = 322010;
            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\2D1.xsl";

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateSpecialOrderXML(Enums.SOTemplateType.SO2D1, esDALRefNo);

            OutboundDocuments outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo);

            //byte[] exportByteArrayData = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, docFileName);
            GenerateEmailgetParams emailgetParams = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, docFileName);
            byte[] exportByteArrayData = Encoding.UTF8.GetBytes(emailgetParams.HtmlContent);
            return exportByteArrayData;
        }

        public byte[] Generate2D5SODocument(Enums.DocumentType doctype, string esDALRefNo)
        {
            string docFileName = "2D5-SpecialOrder";
            int docType = 322010;
            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\2D5.xslt";

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateSpecialOrderXML(Enums.SOTemplateType.SO2D5, esDALRefNo);

            OutboundDocuments outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo);

            //byte[] exportByteArrayData = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, docFileName);
            GenerateEmailgetParams emailgetParams = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, docFileName);
            byte[] exportByteArrayData = Encoding.UTF8.GetBytes(emailgetParams.HtmlContent);
            return exportByteArrayData;
        }

        public byte[] Generate2D8SODocument(Enums.DocumentType doctype, string esDALRefNo)
        {
            string docFileName = "2D8-SpecialOrder";
            int docType = 322010;
            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\2D8.xslt";

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateSpecialOrderXML(Enums.SOTemplateType.SO2D8, esDALRefNo);

            OutboundDocuments outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo);

            //byte[] exportByteArrayData = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, docFileName);
            GenerateEmailgetParams emailgetParams = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, docFileName);
            byte[] exportByteArrayData = Encoding.UTF8.GetBytes(emailgetParams.HtmlContent);
            return exportByteArrayData;
        }
        
        public List<byte[]> GenerateNotificationFAXSOADocument(int notificationId, Enums.DocumentType doctype, int contactId)
        {
            string docFileName = "NotificationFaxSOA";
            int docType = 322003;

            Byte[] outputValue = null;

            List<byte[]> docarray = new List<byte[]>();

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateNotificationFaxSOAXML(Enums.PortalType.SOA, notificationId, false, contactId);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Notification_FAX_SOA_PDF.xsl";

            OutboundDocuments outbounddocs = CommonMethods.GetNotificationDetails(notificationId);

            List<ContactModel> contactList = CommonMethods.GetRecipientDetails(model.ReturnXML);

            model.ReturnXML = ConvertSpecialCharactersToLetters(model.ReturnXML);

            byte[] byteArrayData = Encoding.ASCII.GetBytes(model.ReturnXML);

            //Some data is stored in gzip format, so we need to unzip then load it.
            byte[] CompressData = STP.Common.General.XsltTransformer.CompressData(byteArrayData);

            foreach (ContactModel objcontact in contactList)
            {
                if (doctype == Enums.DocumentType.PDF)
                {
                    //outputValue = CommonMethods.GenerateNotificationPDF(notificationId, docType, model.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, objcontact, docFileName);
                }
                else if (doctype == Enums.DocumentType.EMAIL)
                {
                    //outputValue = CommonMethods.GenerateEMAIL(notificationId, docType, model.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, objcontact, docFileName, true);
                }

                docarray.Add(outputValue);
            }

            return docarray;
        }

        public List<byte[]> GenerateNotificationFAXPoliceDocument(int notificationId, Enums.DocumentType doctype, int contactId)
        {
            string docFileName = "NotificationFaxPolice";
            int docType = 322003;

            Byte[] outputValue = null;

            List<byte[]> docarray = new List<byte[]>();

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateNotificationFaxSOAXML(Enums.PortalType.POLICE, notificationId, false, contactId);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Notification_Fax_Police.xslt";

            OutboundDocuments outbounddocs = CommonMethods.GetNotificationDetails(notificationId);

            List<ContactModel> contactList = CommonMethods.GetRecipientDetails(model.ReturnXML);

            model.ReturnXML = ConvertSpecialCharactersToLetters(model.ReturnXML);

            byte[] byteArrayData = Encoding.ASCII.GetBytes(model.ReturnXML);

            //Some data is stored in gzip format, so we need to unzip then load it.
            byte[] CompressData = STP.Common.General.XsltTransformer.CompressData(byteArrayData);

            foreach (ContactModel objcontact in contactList)
            {
                if (doctype == Enums.DocumentType.PDF)
                {
                    //outputValue = CommonMethods.GenerateNotificationPDF(notificationId, docType, model.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, objcontact, docFileName);
                }
                else if (doctype == Enums.DocumentType.EMAIL)
                {
                    //outputValue = CommonMethods.GenerateEMAIL(notificationId, docType, model.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, objcontact, docFileName, true);
                }

                docarray.Add(outputValue);
            }

            return docarray;
        }

        public List<byte[]> GenerateReNotificationFaxSOADocument(int notificationId, Enums.DocumentType doctype, int contactId)
        {
            string docFileName = "RenotificationFaxSOA";
            int docType = 322003;

            Byte[] outputValue = null;

            List<byte[]> docarray = new List<byte[]>();

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateReNotificationFaxSOAXML(Enums.PortalType.SOA, notificationId, false, contactId);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ReNotification.xslt";

            OutboundDocuments outbounddocs = CommonMethods.GetNotificationDetails(notificationId);

            List<ContactModel> contactList = CommonMethods.GetRecipientDetails(model.ReturnXML);

            model.ReturnXML = ConvertSpecialCharactersToLetters(model.ReturnXML);

            byte[] byteArrayData = Encoding.ASCII.GetBytes(model.ReturnXML);

            //Some data is stored in gzip format, so we need to unzip then load it.
            byte[] CompressData = STP.Common.General.XsltTransformer.CompressData(byteArrayData);


            foreach (ContactModel objcontact in contactList)
            {
                if (doctype == Enums.DocumentType.PDF)
                {
                    //outputValue = CommonMethods.GenerateNotificationPDF(notificationId, docType, model.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, objcontact, docFileName);
                }
                else if (doctype == Enums.DocumentType.EMAIL)
                {
                    //outputValue = CommonMethods.GenerateEMAIL(notificationId, docType, model.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, objcontact, docFileName, true);
                }

                docarray.Add(outputValue);
            }

            return docarray;
        }

        public List<byte[]> GenerateReNotificationFaxPoliceDocument(int notificationId, Enums.DocumentType doctype, int contactId)
        {
            string docFileName = "RenotificationFaxPolice";
            int docType = 322003;

            Byte[] outputValue = null;

            List<byte[]> docarray = new List<byte[]>();

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateReNotificationFaxSOAXML(Enums.PortalType.POLICE, notificationId, false, contactId);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Re_Notification_Fax_Police.xslt";

            OutboundDocuments outbounddocs = CommonMethods.GetNotificationDetails(notificationId);

            List<ContactModel> contactList = CommonMethods.GetRecipientDetails(model.ReturnXML);

            model.ReturnXML = ConvertSpecialCharactersToLetters(model.ReturnXML);

            byte[] byteArrayData = Encoding.ASCII.GetBytes(model.ReturnXML);

            //Some data is stored in gzip format, so we need to unzip then load it.
            byte[] CompressData = STP.Common.General.XsltTransformer.CompressData(byteArrayData);


            foreach (ContactModel objcontact in contactList)
            {
                if (doctype == Enums.DocumentType.PDF)
                {
                    //outputValue = CommonMethods.GenerateNotificationPDF(notificationId, docType, model.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, objcontact, docFileName);
                }
                else if (doctype == Enums.DocumentType.EMAIL)
                {
                    // outputValue = CommonMethods.GenerateEMAIL(notificationId, docType, model.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, objcontact, docFileName, true);
                }

                docarray.Add(outputValue);
            }
            return docarray;
        }

        public byte[] GenerateSTGOReducedNotificationSOADocument(int notificationId, Enums.DocumentType doctype)
        {
            CommonMethods commonMethods = new CommonMethods();

            string docFileName = "STGOReducedNotificationSOA";
            int docType = 322003;

            Byte[] outputValue = null;

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateNotificationFaxSOAXML(Enums.PortalType.SOA, notificationId, false, 0);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\STGOReducedNotificationSOA.xslt";

            OutboundDocuments outbounddocs = CommonMethods.GetNotificationDetails(notificationId);

            outputValue = commonMethods.GeneratePDF(notificationId, docType, model.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, 0, docFileName);

            return outputValue;
        }

        public byte[] GenerateSTGOReducedNotificationPoliceDocument(int notificationId, Enums.DocumentType doctype)
        {
            CommonMethods commonMethods = new CommonMethods();

            string docFileName = "STGOReducedNotificationPolice";
            int docType = 322003;

            Byte[] outputValue = null;

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateNotificationFaxSOAXML(Enums.PortalType.SOA, notificationId, false, 0);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\STGOReducedNotificationPolice.xslt";

            OutboundDocuments outbounddocs = CommonMethods.GetNotificationDetails(notificationId);

            outputValue = commonMethods.GeneratePDF(notificationId, docType, model.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, 0, docFileName);

            return outputValue;
        }

        public byte[] GenerateCAndUReducedNotificationPoliceDocument(int notificationId, Enums.DocumentType doctype)
        {
            CommonMethods commonMethods = new CommonMethods();

            string docFileName = "CAndUReducedNotificationPolice";
            int docType = 322003;

            Byte[] outputValue = null;

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateNotificationFaxSOAXML(Enums.PortalType.SOA, notificationId, false, 0);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\PoliceNotificationReducedCandU.xslt";

            OutboundDocuments outbounddocs = CommonMethods.GetNotificationDetails(notificationId);

            outputValue = commonMethods.GeneratePDF(notificationId, docType, model.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, 0, docFileName);

            return outputValue;
        }

        public byte[] GenerateNotificationForSpecialOrderDocument(int notificationId, Enums.DocumentType doctype, int contactId)
        {
            CommonMethods commonMethods = new CommonMethods();

            string docFileName = "NotificationForSpecialOrder";
            int docType = 322003;

            Byte[] outputValue = null;

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateNotificationFaxSOAXML(Enums.PortalType.SOA, notificationId, false, contactId);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\NotificationForSpecialOrder.xslt";

            OutboundDocuments outbounddocs = CommonMethods.GetNotificationDetails(notificationId);

            outputValue = commonMethods.GeneratePDF(notificationId, docType, model.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, 0, docFileName);

            return outputValue;
        }
        */
        #endregion

        #region GenerateFormVR1Document
        public byte[] GenerateFormVR1Document(string haulierMnemonic, string esdalRefNumber, int Version_No, bool generateFlag = true)
        {
            string docFileName = "FormVR1";
            int docType = 322010;

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateFormVR1XML(haulierMnemonic, esdalRefNumber, Version_No);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\FormVR1.xslt";

            CommonMethods.GetNotificationDetails(Convert.ToInt32(model.NotificationID));

            //byte[] exportByteArrayData = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, "", 0, docFileName, null, null, 277001, false, false, 692001, generateFlag);
            GenerateEmailgetParams emailgetParams = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, "", 0, docFileName, null, null, 277001, false, false, 692001, generateFlag);
            byte[] exportByteArrayData = Encoding.UTF8.GetBytes(emailgetParams.HtmlContent);
            return exportByteArrayData;
        }
        #endregion

        #region  GenerateEsdalNotification(int notificationId)
        public ESDALNotificationGetParams GenerateEsdalNotification(int notificationId, int contactId)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("GenerateEsdalNotification started successfully with parameters Notifid : {0}, ContactId : {1}", notificationId, contactId));

            GenerateXML gxml = new GenerateXML();

            NotificationXSD.OutboundNotificationStructure obns = gxml.GenerateOutboundNotificationStructureData(notificationId, false, contactId);

            XMLModel model = gxml.GenerateNotificationXML(notificationId, obns, NotificationXSD.NotificationTypeType.police);

            OutboundDocuments outbounddocs = CommonMethods.GetNotificationDetails(notificationId);

            string xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Notification_FAX_SOA_PDF.xsl";

            model.ReturnXML = ConvertSpecialCharactersToLetters(model.ReturnXML);

            XMLModel modelSOAPolice = gxml.GenerateNotificationXML(notificationId, obns, NotificationXSD.NotificationTypeType.soapolice);

            string xsltSOAPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Notification_FAX_SOA_PDF.xsl";
            string xsltPolicePath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Notification_Fax_Police.xslt";

            ESDALNotificationGetParams getParams = new ESDALNotificationGetParams
            {
                XMLModel = modelSOAPolice,
                XMLModelPolice = modelSOAPolice,
                XsltPolicePath = xsltPolicePath,
                XsltSOAPath = xsltSOAPath,
                XsltPath = xsltPath,
                outboundDocuments = outbounddocs
            };

            return getParams;
        }
        #endregion

        #region GetLoggedInUserAffectedStructureDetails
        public string GetLoggedInUserAffectedStructureDetails(string xmlInformation, long notificationId, UserInfo SessionInfo, int organisationId)
        {

            StructuresModel struInfo = OutBoundDAO.GetStructuresXML(Convert.ToInt32(notificationId));

            string recipientXMLInformation = string.Empty;
            int userTypeId = SessionInfo.UserTypeId;
            string errormsg;

            Byte[] affectedPartiesArray = struInfo.AffectedStructures;
            byte[] outBoundDecryptString = null;

            XmlDocument xmlDoc = new XmlDocument();

            if (affectedPartiesArray != null)
            {
                try
                {
                    recipientXMLInformation = Encoding.UTF8.GetString(affectedPartiesArray, 0, affectedPartiesArray.Length);

                    xmlDoc.LoadXml(recipientXMLInformation);
                }
                catch
                {
                    outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(affectedPartiesArray);
                }
            }

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\NotificationAffectedStructures.xslt");

            string result = STP.Common.General.XsltTransformer.Trafo(outBoundDecryptString, path, organisationId, userTypeId, out errormsg);

            xmlInformation = xmlInformation.Replace("</OutboundNotification>", "<StructureDetails>");

            xmlInformation += result;

            xmlInformation += "</StructureDetails></OutboundNotification>";

            xmlInformation = xmlInformation.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");

            return xmlInformation;
        }
        #endregion

        #region GetLoggedInUserAffectedStructureDetailsByESDALReference
        public string GetLoggedInUserAffectedStructureDetailsByESDALReference(string xmlInformation, string esDALRefNo, UserInfo SessionInfo, string userSchema, string type, int organisationId)
        {


            StructuresModel struInfo = OutBoundDAO.GetStructuresXMLByESDALReference(esDALRefNo, userSchema);

            string recipientXMLInformation = string.Empty;
            int userTypeId = SessionInfo.UserTypeId;
            string errormsg;

            Byte[] affectedPartiesArray = struInfo.AffectedStructures;
            byte[] outBoundDecryptString = null;

            XmlDocument xmlDoc = new XmlDocument();

            if (affectedPartiesArray != null)
            {
                try
                {
                    recipientXMLInformation = Encoding.UTF8.GetString(affectedPartiesArray, 0, affectedPartiesArray.Length);

                    xmlDoc.LoadXml(recipientXMLInformation);
                }
                catch
                {
                    outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(affectedPartiesArray);
                }
            }

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\NotificationAffectedStructures.xslt");

            string result = STP.Common.General.XsltTransformer.Trafo(outBoundDecryptString, path, organisationId, userTypeId, out errormsg);

            if (type == "agreed")
            {
                xmlInformation = xmlInformation.Replace("</AgreedRoute>", "<StructureDetails>");
            }
            else
            {
                xmlInformation = xmlInformation.Replace("</Proposal>", "<StructureDetails>");
            }


            xmlInformation += result;

            if (type == "agreed")
            {
                xmlInformation += "</StructureDetails></AgreedRoute>";
            }
            else
            {
                xmlInformation += "</StructureDetails></Proposal>";
            }

            xmlInformation = xmlInformation.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");

            return xmlInformation;
        }

        #endregion

        #region  GenerateEsdalReNotification(int notificationId)
        public ESDALNotificationGetParams GenerateEsdalReNotification(int notificationId, int contactId)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("GenerateEsdalReNotification started successfully with parameters Notifid : {0}, ContactId : {1}", notificationId, contactId));

                GenerateXML gxml = new GenerateXML();

                string xsltPath = string.Empty;

                XMLModel modelSoaPolice = gxml.GenerateReNotificationFaxSOAXML(notificationId, false, contactId);

                OutboundDocuments outbounddocs = CommonMethods.GetNotificationDetails(notificationId);

                modelSoaPolice.ReturnXML = ConvertSpecialCharactersToLetters(modelSoaPolice.ReturnXML);

                string xsltSOAPath = AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ReNotification.xslt";

                string xsltPolicePath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Re_Notification_Fax_Police.xslt";

                ESDALNotificationGetParams getParams = new ESDALNotificationGetParams
                {
                    XMLModel = modelSoaPolice,
                    XMLModelPolice = modelSoaPolice,
                    XsltPolicePath = xsltPolicePath,
                    XsltSOAPath = xsltSOAPath,
                    XsltPath = xsltPath,
                    outboundDocuments = outbounddocs
                };

                return getParams;
            }
            catch (Exception e)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("GenerateDocument/GenerateEsdalReNotification, Exception: {0}", e));
                return null;
            }
        }
        #endregion

        #region GenerateHaulierNotificationDocument
        public byte[] GenerateHaulierNotificationDocument(int notificationId, Enums.DocumentType doctype, int contactId, UserInfo sessionInfo = null)
        {
            string docFileName = "HaulierNotification";
            int docType = 322003;
            CommonMethods commonMethods = new CommonMethods();

            string outboundXMLInformation = string.Empty;            

            XMLModel model = new XMLModel();

            if (outboundXMLInformation == string.Empty)
            {
                GenerateXML gxml = new GenerateXML();
                model = gxml.GenerateNotificationFaxSOAXML(Enums.PortalType.SOA, notificationId, true, contactId);
            }
            else
            {
                model.ReturnXML = outboundXMLInformation;

                if (model.ReturnXML != string.Empty)
                {
                    model.ReturnXML = model.ReturnXML.Replace(">?<", ">\u2002<");
                    model.ReturnXML = model.ReturnXML.Replace(">?##**##", ">##**##");                    
                }
            }

            if (model != null && !string.IsNullOrEmpty(model.ReturnXML))
            {

                string xmlRouteDetails = OutBoundDAO.getRouteDetails(Convert.ToInt32(notificationId));

                model.ReturnXML = model.ReturnXML.Replace("</OutboundNotification>", "<RouteDescription>");

                model.ReturnXML += xmlRouteDetails;

                model.ReturnXML += "</RouteDescription></OutboundNotification>";

                model.ReturnXML = model.ReturnXML.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");

                model.ReturnXML = model.ReturnXML.Replace("andamp;", "&amp;");
            }

            string xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Haulier_Notification.xslt";

            OutboundDocuments outbounddocs = CommonMethods.GetNotificationDetails(notificationId);

            byte[] exportbyteArrayData = commonMethods.GeneratePDF(notificationId, docType, model.ReturnXML, xsltPath, outbounddocs.EsdalReference, outbounddocs.OrganisationID, 0, docFileName, true, "", "", 692001, "PDF", sessionInfo);

            return exportbyteArrayData;
        }
        #endregion

        #region GenerateHaulierProposedRouteDocument
        public byte[] GenerateHaulierProposedRouteDocument(string ESDALReferenceNo, int organisationId, int contactId, string userSchema = UserSchema.Portal, UserInfo sessionInfo = null)
        {
            string docFileName = "HaulierProposedRoute";
            int docType = 322001;
            CommonMethods commonMethods = new CommonMethods();

            //byte[] byteArrayData = null;

            //byteArrayData = CommonMethods.GetAgreedProposedNotificationXML("PROPOSED", ESDALReferenceNo, 0);

            string outboundXMLInformation = string.Empty;

            //if (byteArrayData != null && byteArrayData.Length > 0)
            //{
            //    //Some data is stored in gzip format, so we need to unzip then load it.
            //    byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(byteArrayData);

            //    outboundXMLInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);
            //}

            XMLModel model = new XMLModel();
            if (outboundXMLInformation == string.Empty)
            {
                GenerateXML gxml = new GenerateXML();
                model = gxml.GenerateProposalXML(ESDALReferenceNo, organisationId, contactId, userSchema);
            }
            else
            {
                model.ReturnXML = outboundXMLInformation;
            }

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Haulier_Proposed_Route.xslt";

            OutboundDocuments outbounddocs = CommonMethods.GetOrganisationDetails(ESDALReferenceNo);

            byte[] exportbyteArrayData = commonMethods.GeneratePDF(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, ESDALReferenceNo, outbounddocs.OrganisationID, contactId, docFileName, true, "", "", 692001, "PDF", sessionInfo);

            return exportbyteArrayData;
        }
        #endregion

        #region Generate proposal document
        /*
        public int GenerateSOProposalDocument(string esDALRefNo, int organisationID, int contactId, string distributionComments, int versionid, Dictionary<int, int> icaStatusDictionary, string EsdalReference, HAContact hacontact, AgreedRouteStructure agreedroute, string userSchema = UserSchema.Portal, int routePlanUnits = 692001, long ProjectStatus = 0, int versionNo = 0, MovementPrint moveprint = null, decimal PreVersionDistr = 0, UserInfo sessioninfo = null)
        {
            try
            {
                int revisionNo = 0;
                //status has been changed for planned applications(307014)
                long projectstatus = ProjectStatus;
                if (ProjectStatus == 307002)
                {
                    if (PreVersionDistr == 1)
                        projectstatus = 307004;
                    else
                        projectstatus = 307003;
                }
                else if (projectstatus == 307012)
                {
                    projectstatus = 307006;
                }
                string docFileName = "";
                int docType = 0;
                int icaStatus = 277001;

                string order_no = moveprint.OrderNumber;
                long projectId = moveprint.ProjectId;
                Byte[] outputValue = null;
                List<byte[]> docarray = new List<byte[]>();
                GenerateXML gxml = new GenerateXML();
                XMLModel model = new XMLModel();

                XMLModel modelStillAfftdSOA = null;
                XMLModel modelNoLongAfftdSOA = null;
                XMLModel modelSOA = null;
                XMLModel modelPolice = null;

                string xsltPath = "";

                OutboundDocuments outbounddocs = null;
                List<ContactModel> contactList = null;

                //-------------05-12-2014------by poonam------------------
                switch (projectstatus)
                {
                    case 307003:
                        //proposed
                        docFileName = "SpecialOrderProposal";
                        docType = 322001;
                        model = gxml.GenerateProposalXML(esDALRefNo, organisationID, contactId, distributionComments, userSchema);

                        xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\SpecialOrderProposal.xslt";
                        outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo, userSchema);
                        contactList = CommonMethods.GetRecipientDetails(model.ReturnXML);
                        break;
                    case 307004:
                        //re-proposed
                        //still affected SOA xml document
                        modelStillAfftdSOA = gxml.GenerateReProposalStillAffectedFAXSOAXML((int)projectId, Enums.PortalType.SOA, sessioninfo != null ? (sessioninfo.ContactId != null ? Convert.ToInt32(sessioninfo.ContactId) : contactId) : contactId, versionNo, userSchema, distributionComments);//RM#3646

                        //still affected POLICE xml document
                        //no longer affected POLICE xml document
                        modelNoLongAfftdSOA = gxml.GenerateReProposalNoLongerAffectedFAXSOAXML((int)projectId, Enums.PortalType.SOA, sessioninfo != null ? (sessioninfo.ContactId != null ? Convert.ToInt32(sessioninfo.ContactId) : contactId) : contactId, versionNo, userSchema);

                        //no longer affected POLICE xml document

                        contactList = CommonMethods.GetRecipientDetails(modelStillAfftdSOA.ReturnXML);

                        outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo, userSchema);

                        break;
                    case 307005:
                        //agreed
                        modelSOA = gxml.GenerateAgreedRoutetXML(Enums.PortalType.SOA, esDALRefNo, order_no, sessioninfo != null ? (sessioninfo.ContactId != null ? Convert.ToInt32(sessioninfo.ContactId) : contactId) : contactId, userSchema, distributionComments);//RM#3646

                        modelPolice = gxml.GenerateAgreedRoutetXML(Enums.PortalType.POLICE, esDALRefNo, order_no, sessioninfo != null ? (sessioninfo.ContactId != null ? Convert.ToInt32(sessioninfo.ContactId) : contactId) : contactId, userSchema, distributionComments);//RM#3646

                        contactList = CommonMethods.GetRecipientDetails(modelSOA.ReturnXML);

                        outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo, userSchema);

                        break;

                    case 307006:
                        //agreed revised
                        modelSOA = gxml.GenerateRevisedAgreementXML(Enums.PortalType.SOA, esDALRefNo, order_no, sessioninfo != null ? (sessioninfo.ContactId != null ? Convert.ToInt32(sessioninfo.ContactId) : contactId) : contactId, userSchema, distributionComments);//RM#3646

                        modelPolice = gxml.GenerateRevisedAgreementXML(Enums.PortalType.POLICE, esDALRefNo, order_no, sessioninfo != null ? (sessioninfo.ContactId != null ? Convert.ToInt32(sessioninfo.ContactId) : contactId) : contactId, userSchema, distributionComments);//RM#3646

                        modelNoLongAfftdSOA = gxml.GenerateReProposalNoLongerAffectedFAXSOAXML((int)projectId, Enums.PortalType.SOA, sessioninfo != null ? (sessioninfo.ContactId != null ? Convert.ToInt32(sessioninfo.ContactId) : contactId) : contactId, versionNo, userSchema);

                        contactList = CommonMethods.GetRecipientDetails(modelSOA.ReturnXML);

                        outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo, userSchema);

                        break;
                    case 307007:
                        //agreed recleared
                        modelSOA = gxml.GenerateAgreedRoutetXML(Enums.PortalType.SOA, esDALRefNo, order_no, sessioninfo != null ? (sessioninfo.ContactId != null ? Convert.ToInt32(sessioninfo.ContactId) : contactId) : contactId, userSchema, distributionComments);//RM#3646

                        modelPolice = gxml.GenerateAgreedRoutetXML(Enums.PortalType.POLICE, esDALRefNo, order_no, sessioninfo != null ? (sessioninfo.ContactId != null ? Convert.ToInt32(sessioninfo.ContactId) : contactId) : contactId, userSchema, distributionComments);//RM#3646

                        contactList = CommonMethods.GetRecipientDetails(modelSOA.ReturnXML);

                        outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo, userSchema);

                        break;
                }
                #region

                foreach (ContactModel objcontact in contactList)
                {
                    if (objcontact.Organisation != null && objcontact.FullName != null && objcontact.Organisation != string.Empty && objcontact.FullName != string.Empty)
                    {
                        string[] contactDet = null;

                        if (objcontact.ContactId != 0)
                        {
                            //function that returns contact's details in a string array
                            contactDet = RouteAssessmentDAO.FetchContactPreference(objcontact.ContactId, UserSchema.Portal);

                            objcontact.OrganisationId = objcontact.OrganisationId == 0 ? Convert.ToInt32(contactDet[4]) : objcontact.OrganisationId; // saving organisation Id into the NotificationContacts object

                            if (icaStatusDictionary != null)
                            {
                                if (icaStatusDictionary.ContainsKey(objcontact.OrganisationId))
                                {
                                    icaStatus = icaStatusDictionary[objcontact.OrganisationId];
                                }
                            }
                        }
                        else // condition to fetch manually added parties and send police document
                        {
                            contactDet = new string[6];

                            contactDet[3] = "police";// send the police document to any manually added party

                            contactDet[0] = (objcontact.Email != null && objcontact.Email != "") ? "Email" : "Fax";

                            contactDet[1] = objcontact.Email;

                            contactDet[2] = objcontact.Fax;
                        }

                        string latestReason = objcontact.Reason;
                        string[] stringSeparators = new string[] { "##**##" };
                        string finalReson = "";

                        try
                        {
                            if (latestReason != null && latestReason != string.Empty && latestReason.IndexOf("##**##") != -1)
                            {
                                string[] reasonArray = latestReason.Split(stringSeparators, StringSplitOptions.None);

                                if (reasonArray.Length > 1)
                                {
                                    finalReson = reasonArray[1];
                                }
                            }
                            else if (latestReason != null && latestReason != string.Empty && latestReason.IndexOf("##**##") == -1)
                            {
                                finalReson = latestReason;
                            }
                        }
                        catch
                        {
                            // do nothing
                        }

                        if (contactDet[3] == "soa")
                        {
                            #region check for reason

                            if (projectstatus == 307004 && finalReson == "no longer affected")
                            {
                                //no longer affected fax soa
                                docFileName = "ReProposalNolongerAffectedFaxSOA";
                                docType = 322007;
                                model = modelNoLongAfftdSOA;
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Re_Proposal_No_Longer_FAX_SOA.xsl";
                            }
                            else if (projectstatus == 307004 && (finalReson == "still affected" || finalReson == "newly affected" || finalReson == "affected by change of route"))
                            {
                                //still affected fax soa
                                docFileName = "ReProposalStillAffectedFaxSOA";
                                docType = 322001;
                                model = modelStillAfftdSOA; //still affected model SOA document is attached to the contact
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\ReProposalStillAffectedFax.xslt";
                            }
                            else if (projectstatus == 307005)
                            {
                                docFileName = "AgreedRouteSOA";
                                docType = 322010;
                                model = modelSOA;
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\SpecialOrderAgreedRoute.xslt";
                            }
                            else if (projectstatus == 307006 && finalReson == "no longer affected")
                            {
                                //no longer affected fax soa
                                docFileName = "ReProposalNolongerAffectedFaxSOA";
                                docType = 322007;
                                model = modelNoLongAfftdSOA;
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Re_Proposal_No_Longer_FAX_SOA.xsl";
                            }
                            else if (projectstatus == 307006)
                            {
                                //soa
                                docFileName = "RevisedAgreement";
                                docType = 322002;
                                model = modelSOA;
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\RevisedAgreement.xslt";
                            }
                            else if (projectstatus == 307007)
                            {
                                //soa
                                docFileName = "ReclearedAgreementFaxSOA";
                                docType = 322002;
                                model = modelSOA;
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Re_ClearedAgreementFax_SOA.xslt";
                            }
                            #endregion

                            //condition to generate Email document and send for the following preferences
                            if (contactDet[0] == "Email" || contactDet[0] == "Online inbox plus email (HTML)")
                            {
                                objcontact.Email = contactDet[1];
                                /// to send xml attchment in mail to SOA users if xmlattached user preference is set.
                                int varXmlAttached = 0;
                                if (contactDet[5] == "1")
                                {
                                    varXmlAttached = 1; //send 1 for xmlattach parameter to attach xml in mail.
                                }
                                //outputValue = CommonMethods.GenerateEMAIL(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, objcontact.OrganisationId, objcontact, docFileName, true, sessioninfo, icaStatus, false, varXmlAttached, false, routePlanUnits, projectstatus);

                                CommonMethods.GenerateEMAIL(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, objcontact, docFileName, false);
                            }
                            else if (contactDet[0] == "Fax") //condition to generate Fax document and send for the following preferences
                            {
                                objcontact.Fax = contactDet[2];
                                //outputValue = CommonMethods.GenerateNotificationPDF(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, objcontact.OrganisationId, objcontact, docFileName, sessioninfo, icaStatus, false, false, routePlanUnits);
                            }

                            else if (contactDet[0] == "Online Inbox Only") //condition to generate document and save it on online inbox
                            {
                                //outputValue = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, objcontact.OrganisationId, docFileName, objcontact, sessioninfo, icaStatus, false, false, routePlanUnits);
                            
                            }
                        }

                        else if (contactDet[3] == "police")
                        {
                            #region check for reason
                            if (projectstatus == 307004 && finalReson == "no longer affected")
                            {
                                //no longer affected fax police
                                docFileName = "ReProposalNolongerAffectedFaxPolice";
                                docType = 322007;
                                model = modelNoLongAfftdSOA; 
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\ReProposalNoLongerFAXPolice.xslt";
                            }
                            else if (projectstatus == 307004 && (finalReson == "still affected" || finalReson == "newly affected" || finalReson == "affected by change of route"))
                            {
                                //still affected fax police
                                docFileName = "ReProposalStillAffectedFaxPolice";
                                docType = 322001;
                                model = modelStillAfftdSOA; 
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\ReProposalstillaffected_Fax_Police.xslt";
                            }
                            else if (projectstatus == 307005)
                            {
                                docFileName = "AgreedRouteSOA";
                                docType = 322010;
                                model = modelPolice;
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\SpecialOrderAgreedRoute.xslt";
                            }
                            else if (projectstatus == 307006 && finalReson == "no longer affected")
                            {
                                //no longer affected fax soa
                                docFileName = "ReProposalNolongerAffectedFaxSOA";
                                docType = 322007;
                                model = modelNoLongAfftdSOA;
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Re_Proposal_No_Longer_FAX_SOA.xsl";
                            }
                            else if (projectstatus == 307006)
                            {
                                //police
                                docFileName = "RevisedAgreementPolice";
                                docType = 322002;
                                model = modelPolice;
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\RevisedAgreementFaxPolice.xslt";
                            }
                            else if (projectstatus == 307007)
                            {
                                //police
                                docFileName = "ReclearedAgreementFaxPolice";
                                docType = 322002;
                                model = modelPolice;
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\ReClearedAgreementFaxPolice.xslt";
                            }
                            #endregion

                            if (contactDet[0] == "Email" || contactDet[0] == "Online inbox plus email (HTML)")
                            {
                                objcontact.Email = contactDet[1];
                                //outputValue = CommonMethods.GenerateEMAIL(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, objcontact.OrganisationId, objcontact, docFileName, true, sessioninfo, 277001, false, 0, false, routePlanUnits);

                                CommonMethods.GenerateEMAIL(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, objcontact, docFileName, false);
                            }
                            else if (contactDet[0] == "Fax")
                            {
                                objcontact.Fax = contactDet[2];
                                //outputValue = CommonMethods.GenerateNotificationPDF(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, objcontact.OrganisationId, objcontact, docFileName, sessioninfo, 277001, false, false, routePlanUnits);
                            }
                            else if (contactDet[0] == "Online Inbox Only")
                            {
                                //outputValue = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, objcontact.OrganisationId, docFileName, objcontact, sessioninfo, 277001, false, false, routePlanUnits);
                            }
                        }

                        else
                        {
                            continue;
                        }

                        docarray.Add(outputValue);
                    }
                }
                if (contactList.Count > 0)
                {
                    #region Code part to clone movement details by extracting esdal reference code details
                    string esdalReference = esDALRefNo;
                    esdalReference = esdalReference.Replace("~", "#");
                    esdalReference = esdalReference.Replace("-", "/");
                    esdalReference = esdalReference.Replace("#", "/");
                    string[] esdalRefPro = esdalReference.Split('/');

                    MovementCopyDetails moveDetails = new MovementCopyDetails();
                    if (esdalRefPro.Length > 0)
                    {
                        moveDetails.HaulMnemonic = Convert.ToString(esdalRefPro[0]);
                        moveDetails.ESDALRefNo = Convert.ToString(esdalRefPro[1].ToUpper().Replace("S", ""));
                        moveDetails.VersionNo = Convert.ToInt32(esdalRefPro[2].ToUpper().Replace("S", ""));
                    }
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "STP.Document/GenerateDocuments/GenerateSOProposalDocument actionResult method successfully completed");
                    //DAO call to copy from sort movement to portal movement 
                    byte[] hacontactbytes = GetHAContactDetails(hacontact, agreedroute);
                    DocumentTransmissionDAO.CopyMovementSortToPortal(moveDetails, 0, versionid, EsdalReference, hacontactbytes, organisationID, userSchema);
                    #endregion
                }

                #region movement actions for this action method
                if (contactList.Count != 0)
                {

                    MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                    movactiontype.MovementActionType = MovementnActionType.sort_desk_distributes_movement_version;
                    string ErrMsg = string.Empty;
                    movactiontype.ContactName = sessioninfo.FirstName + " " + sessioninfo.LastName;
                    if (ProjectStatus == 307003)
                    {
                        movactiontype.ProjectStatus = "PROPOSED";
                    }
                    else if (ProjectStatus == 307004)
                    {
                        movactiontype.ProjectStatus = "REPROPOSED";
                    }
                    else if (ProjectStatus == 307005)
                    {
                        movactiontype.ProjectStatus = "AGREED";
                    }
                    else if (ProjectStatus == 307006)
                    {
                        movactiontype.ProjectStatus = "AGREED_REVISED";
                    }
                    else if (ProjectStatus == 307007)
                    {
                        movactiontype.ProjectStatus = "AGREED_RECLEARED";
                    }
                    string MovementDescription = MovementActions.GetMovementActionString(sessioninfo, movactiontype, out ErrMsg);
                    OutBoundDocumentDOA.SaveMovementActionForDistTrans(movactiontype, MovementDescription, moveprint.ProjectId,revisionNo, versionNo, userSchema);
                }
                #endregion

                return contactList.Count != 0 ? 1 : 0;
                //return 1;
                #endregion
            }
            catch(Exception ex)
            {
                return 2; //Some error occured while transmission please contact helpdesk
            }

        }
        
        private byte[] GetHAContactDetails(HAContact haContactDetailObj, AgreedRouteStructure agreedroute)
        {
            HAContactStructure objhacontact = new HAContactStructure();

            AggreedRouteXSD.AddressStructure sddrstructure = new AggreedRouteXSD.AddressStructure();
            objhacontact.TelephoneNumber = haContactDetailObj.Telephone;
            objhacontact.EmailAddress = haContactDetailObj.Email;
            objhacontact.Contact = haContactDetailObj.ContactName;
            objhacontact.FaxNumber = haContactDetailObj.Fax;
            string[] Addstru = new string[5];
            Addstru[0] = haContactDetailObj.HAAddress1;
            Addstru[1] = haContactDetailObj.HAAddress2;
            Addstru[2] = haContactDetailObj.HAAddress3;
            Addstru[3] = haContactDetailObj.HAAddress4;
            Addstru[4] = haContactDetailObj.HAAddress5;
            sddrstructure.Line = Addstru;
            objhacontact.Address = sddrstructure;
            switch (haContactDetailObj.Country)
            {
                case "England":
                    sddrstructure.Country = AggreedRouteXSD.CountryType.england;
                    sddrstructure.CountrySpecified = true;
                    break;

                case "Wales":
                    sddrstructure.Country = AggreedRouteXSD.CountryType.wales;
                    sddrstructure.CountrySpecified = true;
                    break;

                case "Scotland":
                    sddrstructure.Country = AggreedRouteXSD.CountryType.scotland;
                    sddrstructure.CountrySpecified = true;
                    break;

                case "Northern Ireland":
                    sddrstructure.Country = AggreedRouteXSD.CountryType.northernireland;
                    sddrstructure.CountrySpecified = true;
                    break;
            }
            sddrstructure.PostCode = haContactDetailObj.PostCode;
            agreedroute.HAContact = objhacontact;

            XmlSerializer serializer = new XmlSerializer(typeof(AgreedRouteStructure));

            StringWriter outStream = new StringExtractor.Utf8StringWriter();    // The code generates UTF-8 encoded xml string 
            serializer.Serialize(outStream, agreedroute);
            string str = outStream.ToString();

            byte[] hacont = StringExtractor.ZipAndBlob(str);
            return hacont;
        }
        */
        #endregion

        #region GenerateHaulierAgreedRouteDocument
        public byte[] GenerateHaulierAgreedRouteDocument(Enums.DocumentType doctype, string esDALRefNo, string order_no, int contactId = 0, UserInfo SessionInfo = null)
        {
            string docFileName = "HaulierAgreedRoute";
            int docType = 322010;
            CommonMethods commonMethods = new CommonMethods();

            XMLModel model = new XMLModel();
            //byte[] byteArrayData = null;

            //byteArrayData = CommonMethods.GetAgreedProposedNotificationXML("AGREED", esDALRefNo, 0);

            //string outboundXMLInformation = string.Empty;

            //if (byteArrayData != null && byteArrayData.Length > 0)
            //{
            //    try
            //    {
            //        //Some data is stored in gzip format, so we need to unzip then load it.
            //        byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(byteArrayData);

            //        outboundXMLInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);
            //        model.ReturnXML = outboundXMLInformation;
            //    }
            //    catch 
            //    {
            //        // do nothing
            //    }
            //}
            //else
            //{
                GenerateXML gxml = new GenerateXML();
                model = gxml.GenerateAgreedRoutetXML(Enums.PortalType.SOA, esDALRefNo, order_no, contactId, SessionInfo.UserSchema);
            //}

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Haulier_Agreed_Route.xslt";

            OutboundDocuments outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo);

            byte[] exportbyteArrayData = commonMethods.GeneratePDF(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, esDALRefNo, outbounddocs.OrganisationID, contactId, docFileName, true, "", "", 692001, "PDF", SessionInfo);

            return exportbyteArrayData;
        }
        #endregion

        #region GenerateAmendmentDocument
        public byte[] GenerateAmendmentDocument(string OrderNumber, Enums.DocumentType doctype, long OrganisationId, bool generateFlag = true)
        {
            string docFileName = "Amendment";
            int docType = 322010;

            GenerateXML gxml = new GenerateXML();
            XMLModel model = gxml.GenerateAmendmentOrderXML(OrderNumber);

            string xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\AmendmentOrder.xslt";

            CommonMethods.GetNotificationDetails(Convert.ToInt32(model.NotificationID));

            GenerateEmailgetParams emailgetParams = CommonMethods.GenerateWord(Convert.ToInt32(model.NotificationID), docType, model.ReturnXML, xsltPath, "", 0, docFileName, null, null, 277001, false, false, 692001, generateFlag);
            byte[] exportByteArrayData = Encoding.UTF8.GetBytes(emailgetParams.HtmlContent);
            return exportByteArrayData;
        }
        #endregion

        #region GetSOAPoliceContactList
        public List<ContactModel> GetSOAPoliceContactList(XMLModel modelSOAPolice)
        {
            List<ContactModel> contactSOAPoliceList = new List<ContactModel>();
            try
            {
                contactSOAPoliceList = CommonMethods.GetRecipientDetails(modelSOAPolice.ReturnXML);
                
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"Document/GetSOAPoliceContactList , Exception: {ex}", ex);
            }
            return contactSOAPoliceList;
        }
        #endregion

        #region GenerateSODistributeDocument
        public static SODistributeDocumentParams GenerateSODistributeDocument(string esDALRefNo, int organisationID, int contactId, string distributionComments, int versionid, Dictionary<int, int> icaStatusDictionary, string EsdalReference, HAContact hacontact, AgreedRouteStructure agreedroute, string userSchema = UserSchema.Portal, int routePlanUnits = 692001, long ProjectStatus = 0, int versionNo = 0, MovementPrint moveprint = null, decimal PreVersionDistr = 0, UserInfo sessioninfo = null)
        {
            try
            {
                //status has been changed for planned applications(307014)
                long projectstatus = ProjectStatus;
                if (ProjectStatus == (int)STP.Common.Enums.ProjectStatus.wip)// 307002
                {
                    if (PreVersionDistr == 1)
                        projectstatus = (int)STP.Common.Enums.ProjectStatus.reproposed;// 307004
                    else
                        projectstatus = (int)STP.Common.Enums.ProjectStatus.proposed;//307003
                }
                else if (projectstatus == (int)STP.Common.Enums.ProjectStatus.agreement_wip)//307012
                {
                    projectstatus = (int)STP.Common.Enums.ProjectStatus.agreed_revised;// 307006
                }
                string docFileName = "";
                int docType = 0;

                string order_no = moveprint.OrderNumber;
                long projectId = moveprint.ProjectId;
                GenerateXML gxml = new GenerateXML();
                XMLModel model = new XMLModel();

                XMLModel modelStillAfftdSOA = new XMLModel();
                XMLModel modelNoLongAfftdSOA = new XMLModel();
                XMLModel modelSOA = new XMLModel();
                XMLModel modelPolice = new XMLModel();

                string xsltPath = "";

                OutboundDocuments outbounddocs = null;
                List<ContactModel> contactList = null;

                //-------------05-12-2014------by poonam------------------
                switch (projectstatus)
                {
                    case (int)STP.Common.Enums.ProjectStatus.proposed:
                        //proposed
                        docFileName = "SpecialOrderProposal";
                        docType = 322001;
                        model = gxml.GenerateProposalXML(esDALRefNo, organisationID, contactId, distributionComments, userSchema);

                        xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\SpecialOrderProposal.xslt";
                        outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo, userSchema);
                        contactList = CommonMethods.GetRecipientDetails(model.ReturnXML);
                        break;
                    case (int)STP.Common.Enums.ProjectStatus.reproposed:
                        //re-proposed
                        //still affected SOA xml document
                        modelStillAfftdSOA = gxml.GenerateReProposalStillAffectedFAXSOAXML((int)projectId, Enums.PortalType.SOA, sessioninfo != null ? (sessioninfo.ContactId != null ? Convert.ToInt32(sessioninfo.ContactId) : contactId) : contactId, versionNo, userSchema, distributionComments);//RM#3646

                        //still affected POLICE xml document
                        //no longer affected POLICE xml document
                        modelNoLongAfftdSOA = gxml.GenerateReProposalNoLongerAffectedFAXSOAXML((int)projectId, Enums.PortalType.SOA, sessioninfo != null ? (sessioninfo.ContactId != null ? Convert.ToInt32(sessioninfo.ContactId) : contactId) : contactId, versionNo, userSchema);

                        //no longer affected POLICE xml document

                        contactList = CommonMethods.GetRecipientDetails(modelStillAfftdSOA.ReturnXML);

                        outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo, userSchema);

                        break;
                    case (int)STP.Common.Enums.ProjectStatus.agreed:
                    case (int)STP.Common.Enums.ProjectStatus.agreed_recleared:
                        //agreed
                        modelSOA = gxml.GenerateAgreedRoutetXML(Enums.PortalType.SOA, esDALRefNo, order_no, sessioninfo != null ? (sessioninfo.ContactId != null ? Convert.ToInt32(sessioninfo.ContactId) : contactId) : contactId, userSchema, distributionComments);//RM#3646

                        modelPolice = gxml.GenerateAgreedRoutetXML(Enums.PortalType.POLICE, esDALRefNo, order_no, sessioninfo != null ? (sessioninfo.ContactId != null ? Convert.ToInt32(sessioninfo.ContactId) : contactId) : contactId, userSchema, distributionComments);//RM#3646

                        contactList = CommonMethods.GetRecipientDetails(modelSOA.ReturnXML);

                        outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo, userSchema);

                        break;

                    case (int)STP.Common.Enums.ProjectStatus.agreed_revised:
                        //agreed revised
                        modelSOA = gxml.GenerateRevisedAgreementXML(Enums.PortalType.SOA, esDALRefNo, order_no, sessioninfo != null ? (sessioninfo.ContactId != null ? Convert.ToInt32(sessioninfo.ContactId) : contactId) : contactId, userSchema, distributionComments);//RM#3646

                        modelPolice = gxml.GenerateRevisedAgreementXML(Enums.PortalType.POLICE, esDALRefNo, order_no, sessioninfo != null ? (sessioninfo.ContactId != null ? Convert.ToInt32(sessioninfo.ContactId) : contactId) : contactId, userSchema, distributionComments);//RM#3646

                        modelNoLongAfftdSOA = gxml.GenerateReProposalNoLongerAffectedFAXSOAXML((int)projectId, Enums.PortalType.SOA, sessioninfo != null ? (sessioninfo.ContactId != null ? Convert.ToInt32(sessioninfo.ContactId) : contactId) : contactId, versionNo, userSchema);

                        contactList = CommonMethods.GetRecipientDetails(modelSOA.ReturnXML);

                        outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo, userSchema);

                        break;
                    /*case (int)STP.Common.Enums.ProjectStatus.agreed_recleared:
                        //agreed recleared
                        modelSOA = gxml.GenerateAgreedRoutetXML(Enums.PortalType.SOA, esDALRefNo, order_no, sessioninfo != null ? (sessioninfo.ContactId != null ? Convert.ToInt32(sessioninfo.ContactId) : contactId) : contactId, userSchema, distributionComments);//RM#3646

                        modelPolice = gxml.GenerateAgreedRoutetXML(Enums.PortalType.POLICE, esDALRefNo, order_no, sessioninfo != null ? (sessioninfo.ContactId != null ? Convert.ToInt32(sessioninfo.ContactId) : contactId) : contactId, userSchema, distributionComments);//RM#3646

                        contactList = CommonMethods.GetRecipientDetails(modelSOA.ReturnXML);

                        outbounddocs = CommonMethods.GetOrganisationDetails(esDALRefNo, userSchema);

                        break;*/
                }

                SODistributeDocumentParams getParams = new SODistributeDocumentParams
                {
                    XmlModel = model,
                    ModelStillAfftdSOA = modelStillAfftdSOA,
                    ModelNoLongAfftdSOA = modelNoLongAfftdSOA,
                    ModelSOA = modelSOA,
                    ModelPolice = modelPolice,
                    XsltPath = xsltPath,
                    DocFileName= docFileName,
                    DocType= docType,
                    OutboundDocuments= outbounddocs,
                    ContactList= contactList,
                    ProjectStatus=projectstatus
                };
                return getParams;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"Document/GenerateSODistributeDocument , Exception: {ex}", ex);
                return null; //Some error occured while transmission please contact helpdesk
            }

        }
        #endregion

        #region GetSoProposalXsltPath
        public static SOProposalXsltPath GetSoProposalXsltPath(string ContactType,long ProjectStatus,string FinalReson)
        {
            string xsltPath = "";
            if (ContactType == "soa")
            {
                #region check for reason

                if (ProjectStatus == 307004 && FinalReson == "no longer affected")
                {
                    //no longer affected fax soa
                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Re_Proposal_No_Longer_FAX_SOA.xsl";
                }
                else if (ProjectStatus == 307004 && (FinalReson == "still affected" || FinalReson == "newly affected" || FinalReson == "affected by change of route"))
                {
                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\ReProposalStillAffectedFax.xslt";
                }
                else if (ProjectStatus == 307005)
                {
                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\SpecialOrderAgreedRoute.xslt";
                }
                else if (ProjectStatus == 307006 && FinalReson == "no longer affected")
                {
                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Re_Proposal_No_Longer_FAX_SOA.xsl";
                }
                else if (ProjectStatus == 307006)
                {
                    //soa
                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\RevisedAgreement.xslt";
                }
                else if (ProjectStatus == 307007)
                {
                    //soa
                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Re_ClearedAgreementFax_SOA.xslt";
                }
                #endregion

                
            }

            else if (ContactType == "police")
            {
                #region check for reason
                if (ProjectStatus == 307004 && FinalReson == "no longer affected")
                {
                    //no longer affected fax police
                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\ReProposalNoLongerFAXPolice.xslt";
                }
                else if (ProjectStatus == 307004 && (FinalReson == "still affected" || FinalReson == "newly affected" || FinalReson == "affected by change of route"))
                {
                    //still affected fax police
                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\ReProposalstillaffected_Fax_Police.xslt";
                }
                else if (ProjectStatus == 307005)
                {
                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\SpecialOrderAgreedRoute.xslt";
                }
                else if (ProjectStatus == 307006 && FinalReson == "no longer affected")
                {
                    //no longer affected fax soa
                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Re_Proposal_No_Longer_FAX_SOA.xsl";
                }
                else if (ProjectStatus == 307006)
                {
                    //police
                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\RevisedAgreementFaxPolice.xslt";
                }
                else if (ProjectStatus == 307007)
                {
                    //police
                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\ReClearedAgreementFaxPolice.xslt";
                }
                #endregion

               
            }

            SOProposalXsltPath sOProposalXslt = new SOProposalXsltPath()
            {
                XSLTPath = xsltPath
            };
            return sOProposalXslt;
        }
        #endregion

    }
}