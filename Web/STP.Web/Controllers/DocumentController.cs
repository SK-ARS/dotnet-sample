using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Rotativa;
using STP.Common.Logger;
using STP.Domain.Applications;
using STP.Domain.DocumentsAndContents;
using STP.Domain.SecurityAndUsers;
using STP.ServiceAccess.CommunicationsInterface;
using STP.ServiceAccess.DocumentsAndContents;
using STP.ServiceAccess.MovementsAndNotifications;
using STP.Web.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace STP.Web.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IMovementsService movementsService;
        private readonly INotificationService notificationsService;
        private readonly INENNotificationService nenNotificationService;
        private readonly IDocumentService documentService;

        private readonly INotificationDocService notificationDocService;
        private readonly ICommunicationsInterfaceService communicationService;

        public DocumentController()
        {
        }

        public DocumentController(IMovementsService movementsService, INENNotificationService nenNotificationService, INotificationService notificationsService,
            INotificationDocService notificationDocService, ICommunicationsInterfaceService communicationService, IDocumentService documentService)
        {
            this.movementsService = movementsService;
            this.documentService = documentService;
            this.notificationsService = notificationsService;
            this.nenNotificationService = nenNotificationService;
            this.notificationDocService = notificationDocService;
            this.communicationService = communicationService;
        }

        /// <summary>
        /// print report based on documenttype and documentid
        /// </summary>
        /// <param name="Documentid">Documentid</param>
        /// <param name="DocumentType">Documenttype</param>
        /// <returns>Return true or false</returns>
        [HttpGet]
        public ActionResult PrintReport1(long notificationId, string esdalRefno, string userType, string DRN, int ISNENVal = 0, long NENInboxId = 0)
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                int contactId = movementsService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));
                string xmlInformation = movementsService.PrintReport(notificationId);


                //===============================================================================================
                xmlInformation = RemoveDispansationNode_of_Outboundxml(xmlInformation, DRN);
                //-----------------------------------------------------------------------------------------------

                xmlInformation = xmlInformation.Replace(">?<", ">\u2002<");
                xmlInformation = xmlInformation.Replace(">?##**##", ">##**##");

                xmlInformation = xmlInformation.Replace("<OnBehalfOf xmlns=\"http://www.esdal.com/schemas/core/movement\"> </OnBehalfOf>", "");
                xmlInformation = xmlInformation.Replace("<OnBehalfOf> </OnBehalfOf>", "");

                byte[] notificationDocument = null;
                string xsltPath = string.Empty;

                int organisationId = Convert.ToInt32(SessionInfo.OrganisationId);
                string HAReferenceNumber = string.Empty;

                if (Session["HAReferenceNo"] != null)
                {
                    HAReferenceNumber = Convert.ToString(Session["HAReferenceNo"]);
                }

                if (xmlInformation != string.Empty)
                {
                    //TODO for NEN project 
                    if (ISNENVal > 0)
                    {
                        xmlInformation = appendNENRouteDetails(xmlInformation, notificationId, NENInboxId, organisationId);
                    }
                    if (userType == "Police")
                    {
                        if (ISNENVal > 0)
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\NEN_Movement_Police.xslt";
                        }
                        else
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Notification_Movement_Police.xslt";
                        }
                        //TODO Chirag
                        xmlInformation = appendRouteDetails(xmlInformation, notificationId, ISNENVal, NENInboxId, organisationId); //added input parameter organisation Id changed for NEN R2 inbox table schema change.
                        //
                        notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", HAReferenceNumber, 692001, "PDF", null, userType);
                    }
                    else
                    {


                        //TODO Chirag
                        xmlInformation = appendRouteDetails(xmlInformation, notificationId, ISNENVal, NENInboxId, organisationId); //added input parameter organisation Id changed for NEN R2 inbox table schema change.

                        xmlInformation = GetLoggedInUserAffectedStructureDetails(xmlInformation, notificationId, NENInboxId, ISNENVal);

                        if (ISNENVal > 0)
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\NEN_Movement_SOA.xsl";
                        }
                        else
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Notification_Movement_SOA.xsl";
                        }

                        notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, SessionInfo.OrganisationName, HAReferenceNumber);
                    }
                }

                if (notificationDocument != null)
                {
                    string notificationDocument1 = documentService.GeneratePDF1(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, SessionInfo.OrganisationName, HAReferenceNumber);

                    string baseUrl = string.Empty;

                    StyleSheet styles = new StyleSheet();
                    styles.LoadTagStyle(iTextSharp.text.html.HtmlTags.H1, iTextSharp.text.html.HtmlTags.FONTSIZE, "16");
                    styles.LoadTagStyle(iTextSharp.text.html.HtmlTags.P, iTextSharp.text.html.HtmlTags.FONTSIZE, "10");
                    styles.LoadTagStyle(iTextSharp.text.html.HtmlTags.P, iTextSharp.text.html.HtmlTags.COLOR, "#ff0000");
                    styles.LoadTagStyle(iTextSharp.text.html.HtmlTags.UL, iTextSharp.text.html.HtmlTags.INDENT, "10");
                    styles.LoadTagStyle(iTextSharp.text.html.HtmlTags.LI, iTextSharp.text.html.HtmlTags.LEADING, "16");

                    //Convert the HTML string to PDF using Blink
                    StringReader sr = new StringReader(notificationDocument1.ToString());
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                        pdfDoc.Open();

                        List<IElement> objects = HTMLWorker.ParseToList(sr, styles);
                        foreach (IElement element in objects)
                        {
                            pdfDoc.Add(element);
                        }

                        //htmlparser.Parse(sr);
                        pdfDoc.Close();

                        byte[] bytes = memoryStream.ToArray();
                        memoryStream.Close();
                        return File(bytes, "application/pdf", "Blink.pdf");
                    }


                    //System.IO.MemoryStream pdfStream = new System.IO.MemoryStream(notificationDocument);

                    //return new FileStreamResult(pdfStream, "application/pdf");

                    //-----INTELLIZENZ PART FOR NEN NOTIFICATIONS AUDIT LOG SAVING-----------

                    //------END HERE---------------------------------------------------------

                }
                else
                {
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movement/PrintReport, Exception: {0}", ex));
                throw ex;
            }
        }


        /// <summary>
        /// Get Route Details 
        /// </summary>
        /// <param name="xmlInformation"></param>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        public string appendRouteDetails(string xmlInformation, double notificationId, int ISNEN = 0, long NENInboxId = 0, int organisationId = 0) //TODO chirag
        {
            //TODO chirag
            if (!string.IsNullOrEmpty(xmlInformation))
            {
                string xmlRouteDetails = "";
                string xmlRouteImperialDetails = "";
                if (ISNEN == 0)
                {
                    xmlRouteDetails = getRouteDetails(Convert.ToInt32(notificationId));//For existing ESDAL2 Notification
                }
                else
                {
                    try
                    {
                        if (NENInboxId > 0)
                        {
                            xmlRouteDetails = getNENRouteDetails(NENInboxId, organisationId, 1);//For NEN Notification this is modified to fetch the latest analysis for hte input inbox id and organisation id
                            xmlRouteImperialDetails = getNENRouteDetails(NENInboxId, organisationId, 2);//For NEN Notification this is modified to fetch the latest analysis for hte input inbox id and organisation id

                            XmlSerializer deserializer = new XmlSerializer(typeof(NotificationXSD.OutboundNotificationStructure));

                            StringReader stringReader = new StringReader(xmlInformation);
                            XmlTextReader xmlReader = new XmlTextReader(stringReader);
                            object obj = deserializer.Deserialize(xmlReader);

                            NotificationXSD.OutboundNotificationStructure XmlData = (NotificationXSD.OutboundNotificationStructure)obj;
                            XmlData.RouteParts[0].Route = xmlRouteDetails;
                            XmlData.RouteParts[0].RouteImperial = xmlRouteImperialDetails;

                            GenerateXML gxml = new GenerateXML();
                            XMLModel model = gxml.GenerateNotificationXML(0, XmlData, NotificationXSD.NotificationTypeType.police);
                            xmlInformation = model.ReturnXML;
                        }
                    }
                    catch (Exception ex)
                    {
                        return xmlRouteDetails;
                    }
                }

                xmlInformation = xmlInformation.Replace("</OutboundNotification>", "<RouteDescription>");

                xmlInformation += xmlRouteDetails;

                xmlInformation += "</RouteDescription></OutboundNotification>";

                xmlInformation = xmlInformation.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
            }
            return xmlInformation;

        }
        #region Below function is for NEN
        /// <summary>
        /// getNENRouteDetails for NEN project
        /// </summary>
        /// <param name="NENInboxId"></param>
        /// <param name="organisationId"></param>
        /// <returns></returns>
        public string getNENRouteDetails(long NENInboxId, int organisationId, int flag = 1)
        {
            string routedesc = string.Empty;
            string errormsg = "No data found";
            string result = string.Empty;
            Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
            //For RM#4311 Change
            string[] separators = { "Split" };
            string resultPart = string.Empty;
            string FinalResult = string.Empty;
            string[] versionSplit = new string[] { };
            StringBuilder sb = new StringBuilder();
            //End

            DrivingInstructionModel DrivingInstructionInfo = notificationDocService.GetNENRouteDescription(NENInboxId, organisationId);

            string path = "";
            if (flag == 1)
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"XSLT\RouteDetails.xslt");
            else
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"XSLT\RouteDetailsImperial.xslt");

            result = STP.Common.General.XsltTransformer.Trafo(DrivingInstructionInfo.DrivingInstructions, path, out errormsg);

            //For RM#4311
            if (result != null && result.Length > 0)
            {
                versionSplit = result.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            }


            foreach (string part in versionSplit)
            {
                resultPart = string.Empty;

                if (part != null && part.Length > 0 && part.IndexOf("Alternative end # 1") != -1 && part.IndexOf("Alternative end # 2") == -1)
                {
                    resultPart = part.Replace("arrive at destination", "<u><b>Alternative end # 2:</b></u> arrive at destination");
                    sb.Append(resultPart);
                }
                else
                {
                    resultPart = part;
                    sb.Append(resultPart);
                }
            }

            if (sb.ToString() != null || sb.ToString() != "")
            {
                FinalResult = sb.ToString();
            }
            //End

            FinalResult = FinalResult.Replace("<u>", "##us##");
            FinalResult = FinalResult.Replace("</u>", "##ue##");

            FinalResult = FinalResult.Replace("<b>", "#bst#");
            FinalResult = FinalResult.Replace("</b>", "#be#");

            FinalResult = _htmlRegex.Replace(FinalResult, string.Empty);

            FinalResult = FinalResult.Replace("Start of new part", "");

            return FinalResult;
        }
        #endregion
        /// <summary>
        /// generate routedescription node detail by processing routedescription xml
        /// </summary>
        /// <param name="NotificationID">notification id</param>
        /// <returns></returns>
        public string getRouteDetails(int NotificationID)
        {
            string messg = "OutboundDAO/getRouteDetails?NotificationID=" + NotificationID;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            string routedesc = string.Empty;
            string errormsg = "No data found";
            string result = string.Empty;
            Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
            //For RM#4311 Change
            string[] separators = { "Split" };
            string resultPart = string.Empty;
            string FinalResult = string.Empty;
            string[] versionSplit = new string[] { };
            StringBuilder sb = new StringBuilder();
            //End

            DrivingInstructionModel DrivingInstructionInfo = notificationDocService.GetRouteDescription(NotificationID);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"XSLT\RouteDetails.xslt");

            result = STP.Common.General.XsltTransformer.Trafo(DrivingInstructionInfo.DrivingInstructions, path, out errormsg);

            //For RM#4311
            if (result != null && result.Length > 0)
            {
                versionSplit = result.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            }


            foreach (string part in versionSplit)
            {
                resultPart = string.Empty;

                if (part != null && part.Length > 0 && part.IndexOf("Alternative end # 1") != -1 && part.IndexOf("Alternative end # 2") == -1)
                {
                    resultPart = part.Replace("arrive at destination", "<u><b>Alternative end # 2:</b></u> arrive at destination");
                    sb.Append(resultPart);
                }
                else
                {
                    resultPart = part;
                    sb.Append(resultPart);
                }
            }

            if (sb.ToString() != null || sb.ToString() != "")
            {
                FinalResult = sb.ToString();
            }
            //End

            //if (result != null && result.Length > 0 && result.IndexOf("Alternative end # 1") != -1 && result.IndexOf("Alternative end # 2") == -1)
            //{
            //    result = result.Replace("arrive at destination", "<u><b>Alternative end # 2:</b></u> arrive at destination");
            //}

            FinalResult = FinalResult.Replace("<u>", "##us##");
            FinalResult = FinalResult.Replace("</u>", "##ue##");

            FinalResult = FinalResult.Replace("<b>", "#bst#");
            FinalResult = FinalResult.Replace("</b>", "#be#");

            FinalResult = _htmlRegex.Replace(FinalResult, string.Empty);

            FinalResult = FinalResult.Replace("Start of new part", "");

            //result = result.Replace("Start\r\n", ": Start\r\n");

            //result = result.Replace("arrive at destination.", "arrive at destination.<hr width='100%'></hr>");

            //result = result.Remove(result.LastIndexOf("<hr width='100%'></hr>"));

            return FinalResult;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlInformation"></param>
        /// <returns></returns>
        public string GetLoggedInUserAffectedStructureDetails(string xmlInformation, long notificationId, long NENInboxID = 0, int NENIdVal = 0, long OrganisationId = 0, int UserTypeId = 0)
        {

            STP.Domain.RouteAssessment.StructuresModel struInfo = new STP.Domain.RouteAssessment.StructuresModel();
            if (NENInboxID != 0 && NENIdVal != 0)
            {
                DrivingInstructionModel DIInfo = notificationDocService.GetNENRouteDescription(NENInboxID, (int)OrganisationId);
                struInfo.AffectedStructures = DIInfo.AffectedStructures;
            }
            else
            {
                struInfo = notificationDocService.GetStructuresXML(Convert.ToInt32(notificationId));
            }
            int organisationId = Convert.ToInt32(OrganisationId);

            string recipientXMLInformation = string.Empty;
            int userTypeId = UserTypeId;
            string errormsg = "No data found";

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
                catch (System.Xml.XmlException XE)
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


        #region AppendNENRouteDetails
        /// <summary>
        /// appendNENRouteDetails
        /// </summary>
        /// <param name="xmlInformation"></param>
        /// <param name="notificationId"></param>
        /// <param name="NENInboxId"></param>
        /// <returns></returns>
        private string appendNENRouteDetails(string xmlInformation, long notificationId, long NENInboxId, int organisationId)
        {
            XmlDocument obj = new XmlDocument();
            obj.LoadXml(xmlInformation);
            string oldStartRoute = "", oldEndRoute = "", oldLegNum = "", oldName = "", oldStartPoint = "", oldEndPoint = "";
            List<STP.Domain.MovementsAndNotifications.Notification.NENUpdateRouteDet> NENLatestRouteObj = new List<STP.Domain.MovementsAndNotifications.Notification.NENUpdateRouteDet>();
            NENLatestRouteObj = nenNotificationService.GetNENRouteList(NENInboxId, organisationId);
            //retreive max no of pieces
            XmlNodeList parentNode = obj.GetElementsByTagName("JourneyFromTo");
            // loop through all AID nodes
            foreach (XmlNode node in parentNode)
            {
                try
                {
                    oldStartRoute = node["From"].InnerText;
                    oldEndRoute = node["To"].InnerText;
                }
                catch (Exception e)
                {
                    oldStartRoute = "";
                    oldEndRoute = "";
                }
            }
            if (NENLatestRouteObj != null)
            {
                if (oldStartRoute != "" && oldEndRoute != "")
                {
                    xmlInformation = xmlInformation.Replace(">" + oldStartRoute + "</From>", ">" + NENLatestRouteObj[0].FromAddress + "</From>");
                    xmlInformation = xmlInformation.Replace(">" + oldEndRoute + "</To>", ">" + NENLatestRouteObj[0].ToAddress + "</To>");
                }
            }
            parentNode = obj.GetElementsByTagName("RoutePartListPosition");
            // loop through all AID nodes
            foreach (XmlNode node in parentNode)
            {
                try
                {
                    if (node.ChildNodes.Count > 0)
                    {
                        try
                        {
                            oldLegNum = node.ChildNodes[0].ChildNodes[0].InnerText;//LegNumber
                        }
                        catch (Exception e)
                        {
                            oldLegNum = "";
                        }
                        try
                        {
                            oldName = node.ChildNodes[0].ChildNodes[1].InnerText;//Name
                        }
                        catch (Exception e)
                        {
                            oldName = "";
                        }
                        try
                        {
                            oldStartPoint = node.ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;//StartDescription
                        }
                        catch (Exception e)
                        {
                            oldStartPoint = "";
                        }
                        try
                        {
                            oldEndPoint = node.ChildNodes[0].ChildNodes[2].ChildNodes[1].ChildNodes[0].ChildNodes[0].InnerText;//EndDescription
                        }
                        catch (Exception e)
                        {
                            oldEndPoint = "";
                        }

                    }
                }
                catch (Exception e)
                {
                    oldLegNum = "";
                    oldName = "";
                    oldStartPoint = "";
                    oldEndPoint = "";
                }
                if (oldLegNum != "")
                {
                    if (oldLegNum == "1")
                    {
                        if (NENLatestRouteObj != null && oldName != "" && oldStartPoint != "" && oldEndPoint != "" && NENLatestRouteObj.Count > 0)
                        {
                            node.ChildNodes[0].ChildNodes[1].InnerText = NENLatestRouteObj[0].RouteName;
                            node.ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText = NENLatestRouteObj[0].FromAddress;
                            node.ChildNodes[0].ChildNodes[2].ChildNodes[1].ChildNodes[0].ChildNodes[0].InnerText = NENLatestRouteObj[0].ToAddress;
                        }
                    }
                    else if (oldLegNum == "2")
                    {
                        if (NENLatestRouteObj != null && oldName != "" && oldStartPoint != "" && oldEndPoint != "" && NENLatestRouteObj.Count > 1)
                        {
                            node.ChildNodes[0].ChildNodes[1].InnerText = NENLatestRouteObj[1].RouteName;
                            node.ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText = NENLatestRouteObj[1].FromAddress;
                            node.ChildNodes[0].ChildNodes[2].ChildNodes[1].ChildNodes[0].ChildNodes[0].InnerText = NENLatestRouteObj[1].ToAddress;
                        }
                    }
                }
            }

            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                obj.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                xmlInformation = stringWriter.GetStringBuilder().ToString();
            }

            return xmlInformation;
        }
        #endregion

        //===============================================================================================
        /// <summary>
        /// Function to remove Dispansation node from outbound xml string
        /// </summary>
        /// <param name="xmlInformation">xml string variable</param>
        /// <param name="DRN">Dispansation Registration Number</param>
        /// <returns>modified xml </returns>
        private string RemoveDispansationNode_of_Outboundxml(string xmlInformation, string DRN)
        {

            XmlDocument docOutboundNode = new XmlDocument();
            XDocument docOutBoundNodeLinq = new XDocument();
            try
            {
                if (string.IsNullOrEmpty(DRN) == false)
                {
                    XNamespace ns = "http://www.esdal.com/schemas/core/notification";

                    docOutBoundNodeLinq = XDocument.Parse(xmlInformation);

                    var DRNArray = DRN.Split(',');

                    var outBoundDispensations = (from d in docOutBoundNodeLinq.Descendants(ns + "Dispensations").Descendants(ns + "OutboundDispensation")
                                                 where DRNArray.Contains(d.Elements(ns + "DRN").ElementAtOrDefault(0).Value) == false
                                                 select d).ToList();


                    if (outBoundDispensations != null || outBoundDispensations.Count > 0)
                    {
                        foreach (var xItems in outBoundDispensations)
                        {
                            xItems.Remove();
                        }
                    }


                    return docOutBoundNodeLinq.ToString();
                }
                else
                {

                    XNamespace ns = "http://www.esdal.com/schemas/core/notification";

                    docOutBoundNodeLinq = XDocument.Parse(xmlInformation);

                    var outBoundDispensations = (from d in docOutBoundNodeLinq.Descendants(ns + "Dispensations")
                                                 select d).ToList();

                    if (outBoundDispensations != null || outBoundDispensations.Count > 0)
                    {
                        foreach (var xItems in outBoundDispensations)
                        {
                            xItems.Remove();
                        }
                    }

                    return docOutBoundNodeLinq.ToString();
                }

            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        //-----------------------------------------------------------------------------------------------



        public ActionResult AgreedRouteV1()
        {
            //read configuration xml
            XmlSerializer deserializer = new XmlSerializer(typeof(STP.Domain.MovementsAndNotifications.Folder.OutboundNotification));
            STP.Domain.MovementsAndNotifications.Folder.OutboundNotification agreedRoute = new STP.Domain.MovementsAndNotifications.Folder.OutboundNotification();

            string xsltPath1 = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\data3.xml";

            using (XmlReader reader = XmlReader.Create(xsltPath1))
            {
                agreedRoute = (STP.Domain.MovementsAndNotifications.Folder.OutboundNotification)deserializer.Deserialize(reader);
            }
            //string singleLine = ReplaceWhitespace(agreedRoute.RouteParts.RoutePartListPosition.Route, "");


            //agreedRoute.RouteParts.RoutePartListPosition.Route = singleLine;

            //string RouteImperial = ReplaceWhitespace(agreedRoute.RouteParts.RoutePartListPosition.RouteImperial, "");
            //agreedRoute.RouteParts.RoutePartListPosition.RouteImperial = RouteImperial;
            return new Rotativa.ViewAsPdf("AgreedRouteV1", agreedRoute)
            {


            };
        }

        public ActionResult PrintIndexV1()
        {
            AgreedRoute agreedRoute = new AgreedRoute();

            return new ActionAsPdf("AgreedRouteV1")
            {
                CustomSwitches = "--disable-smart-shrinking --no-outline",
            };
        }



        public ActionResult AgreedRouteV()
        {
            //read configuration xml
            XmlSerializer deserializer = new XmlSerializer(typeof(AgreedRoute));
            AgreedRoute agreedRoute;
            using (XmlReader reader = XmlReader.Create(Server.MapPath("~/Content/Data/AgreedRoute.xml")))
            {
                agreedRoute = (AgreedRoute)deserializer.Deserialize(reader);
            }

            return new Rotativa.ViewAsPdf("AgreedRouteV", agreedRoute)
            {


            };

            //return View(agreedRoute);
        }

        public ActionResult PrintIndexV()
        {
            return new ActionAsPdf("AgreedRouteV")
            {
                CustomSwitches = "--disable-smart-shrinking --no-outline"
            };
        }


    }
}