using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.General;
using STP.Common.Logger;
using STP.Common.SortHelper;
using STP.Common.StringExtractor;
using STP.Domain.Applications;
using STP.Domain.DocumentsAndContents;
using STP.Domain.HelpdeskTools;
using STP.Domain.LoggingAndReporting;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.RouteAssessment;
using STP.Domain.Routes;
using STP.Domain.SecurityAndUsers;
using STP.Domain.Structures;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.Workflow;
using STP.Domain.Workflow.Models;
using STP.ServiceAccess.Applications;
using STP.ServiceAccess.CommunicationsInterface;
using STP.ServiceAccess.DocumentsAndContents;
using STP.ServiceAccess.LoggingAndReporting;
using STP.ServiceAccess.MovementsAndNotifications;
using STP.ServiceAccess.RouteAssessment;
using STP.ServiceAccess.Routes;
using STP.ServiceAccess.VehiclesAndFleets;
using STP.ServiceAccess.Workflows.SORTSOProcessing;
using STP.ServiceAccess.Workflows.SORTVR1Processing;
using STP.Web.Filters;
using STP.Web.WorkflowProvider;

using System.Configuration;
using System.Dynamic;
using System.Globalization;
using System.IO;

using System.Text;
using System.Text.RegularExpressions;

using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using UserType = STP.Common.Constants.UserType;
using Rotativa;
namespace STP.Web.Controllers
{
    public class ReportDocumentController : Controller
    {

        #region Constructor
        private readonly ISORTDocumentService sortDocumentService;
        private readonly IMovementsService movementsService;
        private readonly INotificationDocService notificationDocService;
        private readonly IDocumentService documentService;
        private readonly ISORTApplicationService sortApplicationService;
        private readonly IRoutesService routeService;
        private readonly IRouteAssessmentService routeAssessmentService;
        private readonly ILoggingService loggingService;
        private readonly IApplicationService applicationService;
        private readonly IVehicleConfigService vehicleconfigService;
        private readonly ISORTSOProcessingService sortSOProcessingService;
        private readonly ICommunicationsInterfaceService communicationService;
        private readonly ISORTVR1ProcessingService sortVR1ProcessingService;

        public ReportDocumentController(ISORTDocumentService sortDocumentService, IMovementsService movements, INotificationDocService notificationDocService, IDocumentService documentService, ISORTApplicationService sortApplicationService, IRouteAssessmentService routeAssessmentService, ILoggingService loggingService, IApplicationService applicationService, IRoutesService routeService, IVehicleConfigService vehicleconfigService, ICommunicationsInterfaceService communicationService, ISORTSOProcessingService sortSOProcessingService, ISORTVR1ProcessingService sortVR1ProcessingService)
        {
            this.sortDocumentService = sortDocumentService;
            this.movementsService = movements;
            this.notificationDocService = notificationDocService;
            this.documentService = documentService;
            this.sortApplicationService = sortApplicationService;
            this.routeAssessmentService = routeAssessmentService;
            this.loggingService = loggingService;
            this.applicationService = applicationService;
            this.routeService = routeService;
            this.vehicleconfigService = vehicleconfigService;
            this.sortSOProcessingService = sortSOProcessingService;
            this.sortVR1ProcessingService = sortVR1ProcessingService;
            this.communicationService = communicationService;
        }

        #endregion
        // GET: ReportDocument
        public ActionResult Index()
        {
            return View();
        }

        #region SORT Proposal
        public ActionResult ViewProposedReport(string esdalRefno = "", int trans_id = 0, int OrgId = 0, int contactId = 0, int flagpoint = 0)
        {
            //try
            //{
            //    string messg = "ReportDocument/ViewProposedReport?esdalRefno=" + esdalRefno + "trans_id=" + trans_id + "OrgId=" + OrgId + "contactId=" + contactId;
            //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            //    string userType = "Police";  //by default userTyoe is Police

            if (Session["UserInfo"] == null)
            {

                return RedirectToAction("Login", "Account");
            }

            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

           

            if (SessionInfo.UserTypeId == UserType.Sort && OrgId == 0 && contactId == 0)
            {
                OrgId = (int)Session["SORTOrgID"];
                contactId = movementsService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));
            }
            else if (SessionInfo.UserTypeId == UserType.PoliceALO)
            {
                if (contactId == 0)
                {
                    contactId = -1;
                }
                if (OrgId == 0)
                {
                    OrgId = -1;
                }
            }

            //    #region Function to fetch outbound document/ user type based on input parameters Trans_id and OrgId
            //    //Function Call to fetch xml document from outbound document for trans_id aka document id and usertype based on organisation id
            //    DocumentInfo XmlOutboundDoc = movementsService.ViewMovementDocument(trans_id, OrgId, SessionInfo.UserSchema);

            //    string xmlInformation = XmlOutboundDoc.XMLDocument;

            //    //if OrgId is 0 then by default police is selected else based on the output from stored procedure the usertype is considered.
            //    if (OrgId != 0 && XmlOutboundDoc.UserType != String.Empty)
            //    {
            //        userType = XmlOutboundDoc.UserType;
            //    }
            //    #endregion

            //    if (xmlInformation != string.Empty)
            //    {
            //        xmlInformation = xmlInformation.Replace(">?<", ">\u2002<");
            //        xmlInformation = xmlInformation.Replace(">?##**##", ">##**##");
            //    }

            //    string notificationDocument = null;
            //    string xsltPath = string.Empty;

            //    if (xmlInformation != string.Empty)
            //    {
            //        if (userType == "Police")
            //        {
            //            xsltPath = "XSLT\\ProposalForPolice.xslt";

            //            //RM#4965
            //            List<TransmissionModel> transmissionList = sortDocumentService.GetTransmissionType(trans_id, "310001,310009,310008,310002,310007,310003,310005", 7, UserSchema.Portal);

            //            if (transmissionList.Any() && !string.IsNullOrEmpty(transmissionList[0].Fax))
            //            {
            //                notificationDocument = documentService.GenerateHTMLPDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
            //            }
            //            else
            //            {
            //                if (flagpoint == 1)
            //                {
            //                    ViewBag.HtmlStr = ViewSpecialOrderEmail(xmlInformation, xsltPath, contactId); //RM#4965                            
            //                    return View("../DistributionStatus/EmailTransmissionView");
            //                }
            //                else
            //                {
            //                    notificationDocument = documentService.GenerateHTMLPDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
            //                }
            //            }

            //        }
            //        else
            //        {
            //            string[] esdalRefPro = esdalRefno.Split('/');
            //            string haulierMnemonic = string.Empty;
            //            string esdalrefnum = string.Empty;
            //            int versionNo = 0;

            //            if (esdalRefPro.Length > 0)

            //            {
            //                haulierMnemonic = Convert.ToString(esdalRefPro[0]);
            //                esdalrefnum = Convert.ToString(esdalRefPro[1].ToUpper().Replace("S", ""));
            //                versionNo = Convert.ToInt32(esdalRefPro[2].ToUpper().Replace("S", ""));
            //            }

            //            xmlInformation = documentService.GetLoggedInUserAffectedStructureDetailsByESDALReference(xmlInformation, esdalRefno, SessionInfo, UserSchema.Sort, "Proposal", OrgId);

            //            xsltPath = "XSLT\\ProposalFaxSOA.xslt";

            //            //RM#4965
            //            List<TransmissionModel> transmissionList = sortDocumentService.GetTransmissionType(trans_id, "310001,310009,310008,310002,310007,310003,310005", 7, UserSchema.Portal);

            //            if (transmissionList.Any() && !string.IsNullOrEmpty(transmissionList[0].Fax))
            //            {
            //                notificationDocument = documentService.GenerateHTMLPDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
            //            }
            //            else
            //            {
            //                if (flagpoint == 1)
            //                {
            //                    ViewBag.HtmlStr = ViewSpecialOrderEmail(xmlInformation, xsltPath, contactId); //RM#4965                            
            //                    return View("../DistributionStatus/EmailTransmissionView");
            //                }
            //                else
            //                {
            //                    notificationDocument = documentService.GenerateHTMLPDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
            //                }

            //            }

            //        }
            //    }

            //    if (notificationDocument != null)
            //    {
            //        notificationDocument = notificationDocument.Replace("<newdate>", "");
            //        notificationDocument = notificationDocument.Replace("</newdate>", "");
            //        using (MemoryStream stream = new System.IO.MemoryStream())
            //        {
            //            StringReader sr = new StringReader(notificationDocument);
            //            iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 100f, 0f);
            //            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
            //            pdfDoc.Open();
            //            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
            //            pdfDoc.Close();
            //            return File(stream.ToArray(), "application/pdf");
            //        }
            //    }
            //    else
            //    {
            //        return Content("<script language='javascript' type='text/javascript'>alert('Data is not valid for current movement therefore PDF document will not be generated.');</script>");
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/ViewProposedReport, Exception: {0}", ex));
            //    throw ex;
            //}



            return new ActionAsPdf("ProposalSOA", new
            {
                esdalRefno = esdalRefno,
                trans_id = trans_id,
                OrgId = OrgId,
                contactId = contactId,
                flagpoint = flagpoint,
                UserTypeId = SessionInfo.UserTypeId,
                UserId = SessionInfo.UserId,
                OrganisationId = SessionInfo.OrganisationId,
                OrganisationName = SessionInfo.OrganisationName,
                UserName = SessionInfo.UserName,
                userSchema = SessionInfo.UserSchema
            });
        }


        [HttpGet]
        public ActionResult ProposalSOA(string esdalRefno = "", int trans_id = 0, int OrgId = 0, int contactId = 0, int flagpoint = 0,
            int UserTypeId = 0, string UserId = "", long OrganisationId = 0, string OrganisationName = "", string UserName = "", string userSchema = "")
        {

            try
            {
                UserInfo SessionInfo = new UserInfo();
                SessionInfo.UserTypeId = UserTypeId;
                SessionInfo.UserId = UserId;
                SessionInfo.OrganisationId = OrganisationId;
                SessionInfo.OrganisationName = OrganisationName;
                SessionInfo.UserName = UserName;
                string messg = "ReportDocument/ViewProposedReport?esdalRefno=" + esdalRefno + "trans_id=" + trans_id + "OrgId=" + OrgId + "contactId=" + contactId;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                string userType = "Police";  //by default userTyoe is Police


                #region Function to fetch outbound document/ user type based on input parameters Trans_id and OrgId
                //Function Call to fetch xml document from outbound document for trans_id aka document id and usertype based on organisation id
                DocumentInfo XmlOutboundDoc = movementsService.ViewMovementDocument(trans_id, OrgId, userSchema);

                string xmlInformation = XmlOutboundDoc.XMLDocument;

                //if OrgId is 0 then by default police is selected else based on the output from stored procedure the usertype is considered.
                if (OrgId != 0 && XmlOutboundDoc.UserType != String.Empty)
                {
                    userType = XmlOutboundDoc.UserType;
                }
                #endregion

                if (xmlInformation != string.Empty)
                {
                    xmlInformation = xmlInformation.Replace(">?<", ">\u2002<");
                    xmlInformation = xmlInformation.Replace(">?##**##", ">##**##");
                }

                //string notificationDocument = null;
                string xsltPath = string.Empty;

                if (xmlInformation != string.Empty)
                {
                    if (userType == "Police")
                    {
                        xsltPath = "XSLT\\ProposalForPolice.xslt";

                        //RM#4965
                        List<TransmissionModel> transmissionList = sortDocumentService.GetTransmissionType(trans_id, "310001,310009,310008,310002,310007,310003,310005", 7, UserSchema.Portal);

                        if (transmissionList.Any() && !string.IsNullOrEmpty(transmissionList[0].Fax))
                        {
                            //notificationDocument = documentService.GenerateHTMLPDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
                        }
                        else
                        {
                            if (flagpoint == 1)
                            {
                               // ViewBag.HtmlStr = ViewSpecialOrderEmail(xmlInformation, xsltPath, contactId); //RM#4965                            
                                return View("../DistributionStatus/EmailTransmissionView");
                            }
                            else
                            {
                                // notificationDocument = documentService.GenerateHTMLPDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
                            }
                        }

                    }
                    else
                    {
                        string[] esdalRefPro = esdalRefno.Split('/');
                        string haulierMnemonic = string.Empty;
                        string esdalrefnum = string.Empty;
                        int versionNo = 0;

                        if (esdalRefPro.Length > 0)

                        {
                            haulierMnemonic = Convert.ToString(esdalRefPro[0]);
                            esdalrefnum = Convert.ToString(esdalRefPro[1].ToUpper().Replace("S", ""));
                            versionNo = Convert.ToInt32(esdalRefPro[2].ToUpper().Replace("S", ""));
                        }

                        xmlInformation = documentService.GetLoggedInUserAffectedStructureDetailsByESDALReference(xmlInformation, esdalRefno, SessionInfo, UserSchema.Sort, "Proposal", OrgId);

                        //xsltPath = "XSLT\\ProposalFaxSOA.xslt";

                        //RM#4965
                        List<TransmissionModel> transmissionList = sortDocumentService.GetTransmissionType(trans_id, "310001,310009,310008,310002,310007,310003,310005", 7, UserSchema.Portal);

                        if (transmissionList.Any() && !string.IsNullOrEmpty(transmissionList[0].Fax))
                        {
                            //notificationDocument = documentService.GenerateHTMLPDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
                        }
                        else
                        {
                            if (flagpoint == 1)
                            {
                               // ViewBag.HtmlStr = ViewSpecialOrderEmail(xmlInformation, xsltPath, contactId); //RM#4965                            
                                return View("../DistributionStatus/EmailTransmissionView");
                            }
                            else
                            {
                                //notificationDocument = documentService.GenerateHTMLPDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
                            }

                        }

                    }
                }

                byte[] byteArray = Encoding.UTF8.GetBytes(xmlInformation);

                MemoryStream stream = new MemoryStream(byteArray);



                //read configuration xml
                XmlSerializer deserializer = new XmlSerializer(typeof(ProposalParams.Proposal));
                ProposalParams.Proposal agreedRoute = new ProposalParams.Proposal();

                using (XmlReader reader = XmlReader.Create(stream))
                {
                    agreedRoute = (ProposalParams.Proposal)deserializer.Deserialize(reader);
                }
            

                return View("~/Views/Document/ProposalSOA.cshtml ", agreedRoute);



            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/ViewProposedReport, Exception: {0}", ex));
                throw ex;
            }


            return View("");

        }
        public static string ReplaceWhitespace(string input, string replacement)
        {
            string resultString = Regex.Replace(input, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);

            string pattern = "   ";
            string replace = "";

            string result = Regex.Replace(resultString, pattern, replace);
            String singleLine = result.Replace("\n", " ");
            return singleLine;
        }

        #endregion

        #region SORT Reproposal
        public ActionResult ViewReProposedReport(string esdalRefno = "", int trans_id = 0, int OrgId = 0, int contactId = 0, int flagpoint = 0)
        {
          
                string messg = "ReportDocument/ViewReProposedReport?esdalRefno=" + esdalRefno + "trans_id=" + trans_id + "OrgId=" + OrgId + "contactId=" + contactId;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            if (Session["UserInfo"] == null)
            {

                return RedirectToAction("Login", "Account");
            }

            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];   

               if (SessionInfo.UserTypeId == UserType.Sort && OrgId == 0 && contactId == 0)
                {
                    OrgId = (int)Session["SORTOrgID"];
                    contactId = movementsService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));
                }
                else if (SessionInfo.UserTypeId == UserType.PoliceALO)
                {
                    if (contactId == 0)
                    {
                        contactId = -1;
                    }
                    if (OrgId == 0)
                    {
                        OrgId = -1;
                    }
                }

               
            return new ActionAsPdf("ReProposalSOA", new
            {
                esdalRefno = esdalRefno,
                trans_id = trans_id,
                OrgId = OrgId,
                contactId = contactId,
                flagpoint = flagpoint,
                UserTypeId = SessionInfo.UserTypeId,
                UserId = SessionInfo.UserId,
                OrganisationId = SessionInfo.OrganisationId,
                OrganisationName = SessionInfo.OrganisationName,
                UserName = SessionInfo.UserName,
                userSchema = SessionInfo.UserSchema
            });
        }


        [HttpGet]
        public ActionResult ReProposalSOA(string esdalRefno = "", int trans_id = 0, int OrgId = 0, int contactId = 0, int flagpoint = 0,
            int UserTypeId = 0, string UserId = "", long OrganisationId = 0, string OrganisationName = "", string UserName = "", string userSchema = "")
        {
            try
            {
                UserInfo SessionInfo = new UserInfo();
                SessionInfo.UserSchema = userSchema;
               
                string userType = "Police";  //by default userTyoe is Police
                #region Function to fetch outbound document/ user type based on input parameters Trans_id and OrgId
                //Function Call to fetch xml document from outbound document for trans_id aka document id and usertype based on organisation id
                DocumentInfo XmlOutboundDoc = movementsService.ViewMovementDocument(trans_id, OrgId, userSchema);

                string xmlInformation = XmlOutboundDoc.XMLDocument;

                //if OrgId is 0 then by default police is selected else based on the output from stored procedure the usertype is considered.
                if (OrgId != 0 && XmlOutboundDoc.UserType != String.Empty)
                {
                    userType = XmlOutboundDoc.UserType;
                }
                #endregion

                if (xmlInformation != string.Empty)
                {
                    xmlInformation = xmlInformation.Replace(">?<", ">\u2002<");
                    xmlInformation = xmlInformation.Replace(">?##**##", ">##**##");
                }

                String notificationDocument = null;
                string xsltPath = string.Empty;
                TransmittingDocumentDetails transmittingDetail = new TransmittingDocumentDetails();
                transmittingDetail = documentService.SortSideCheckDoctype(trans_id, SessionInfo.UserSchema);
                List<TransmissionModel> transmissionList = sortDocumentService.GetTransmissionType(trans_id, "310001,310009,310008,310002,310007,310003,310005", 7, UserSchema.Portal);
                if (xmlInformation != string.Empty)
                {
                    if (userType == "Police")
                    {
                        if (transmittingDetail.DocumentType == "no longer affected")
                        {
                            xsltPath = "XSLT\\ReProposalNoLongerFAXPolice.xslt";
                        }
                        else
                        {
                            xsltPath = "XSLT\\ReProposalstillaffected_Fax_Police.xslt";
                        }

                        if (transmissionList.Any() && !string.IsNullOrEmpty(transmissionList[0].Fax))
                        {
                        //    notificationDocument = documentService.GenerateHTMLPDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
                        }
                        else
                        {
                            if (flagpoint == 1)
                            {
                              //  ViewBag.HtmlStr = ViewSpecialOrderEmail(xmlInformation, xsltPath, contactId); //RM#4965                            
                                return View("../DistributionStatus/EmailTransmissionView");
                            }
                            else
                            {
                              //  notificationDocument = documentService.GenerateHTMLPDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
                            }
                        }


                    }
                    else
                    {
                        if (transmittingDetail.DocumentType == "no longer affected")
                        {
                          //  xsltPath = "XSLT\\Re_Proposal_No_Longer_FAX_SOA.xsl";
                        }
                        else
                        {
                            //STP.Document.GenerateDocument genDocument = new Document.GenerateDocument();
                            xmlInformation = documentService.GetLoggedInUserAffectedStructureDetailsByESDALReference(xmlInformation, esdalRefno, SessionInfo, UserSchema.Sort, "Proposal", OrgId);
                            //xsltPath = "XSLT\\ReProposalStillAffectedFax.xslt";
                        }
                        if (transmissionList.Any() && !string.IsNullOrEmpty(transmissionList[0].Fax))
                        {
                           // notificationDocument = documentService.GenerateHTMLPDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "MovementPDF", SessionInfo);
                        }
                        else
                        {
                            if (flagpoint == 1)
                            {
                               // ViewBag.HtmlStr = ViewSpecialOrderEmail(xmlInformation, xsltPath, contactId); //RM#4965                            
                                return View("../DistributionStatus/EmailTransmissionView");
                            }
                            else
                            {
                                //notificationDocument = documentService.GenerateHTMLPDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "MovementPDF", SessionInfo);
                            }
                        }


                    }
                }

                if (xmlInformation != null)
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(xmlInformation);

                    MemoryStream stream = new MemoryStream(byteArray);



                    //read configuration xml
                    XmlSerializer deserializer = new XmlSerializer(typeof(ReProposalParams.ProposalStructure));
                    ReProposalParams.ProposalStructure reproposedRoute = new ReProposalParams.ProposalStructure();

                    using (XmlReader reader = XmlReader.Create(stream))
                    {
                        reproposedRoute = (ReProposalParams.ProposalStructure)deserializer.Deserialize(reader);
                    }


                    return View("~/Views/Document/ReProposalSOA.cshtml ", reproposedRoute);

                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data is not valid for current movement therefore PDF document will not be generated.');</script>");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ReportDocument/ViewReProposedReport, Exception: {0}", ex));
                throw ex;
            }
        }

        #endregion
        #region Agree Report
        public ActionResult ViewAgreedReport(string esdalRefno = "", int trans_id = 0, int OrgId = 0, int contactId = 0, int flagpoint = 0)
        {
           
                string messg = "ReportDocument/ViewAgreedReport?esdalRefno=" + esdalRefno + "trans_id=" + trans_id + "OrgId=" + OrgId + "contactId=" + contactId;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                string userType = "Police"; //by default userTyoe is Police

                if (Session["UserInfo"] == null)
                {
                   
                    return RedirectToAction("Login", "Account");
                }

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                if (SessionInfo.UserTypeId == UserType.Sort && OrgId == 0 && contactId == 0)
                {
                    OrgId = (int)Session["SORTOrgID"];
                    contactId = movementsService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));
                }
                else if (SessionInfo.UserTypeId == UserType.PoliceALO)
                {
                    if (contactId == 0)
                    {
                        contactId = -1;
                    }
                    if (OrgId == 0)
                    {
                        OrgId = -1;
                    }
                }



          
            return new ActionAsPdf("SortViewAgree", new
            {
                EsdalRefno = esdalRefno,
                Trans_id = trans_id,
                OrgId = OrgId,
                contactId = contactId,
                flagpoint = flagpoint,
                UserTypeId = SessionInfo.UserTypeId,
                UserId = SessionInfo.UserId,
                OrganisationId = SessionInfo.OrganisationId,
                OrganisationName = SessionInfo.OrganisationName,
                UserName = SessionInfo.UserName,
                userSchema = SessionInfo.UserSchema
            });
        }

        [HttpGet]
        public ActionResult SortViewAgree(string EsdalRefno = "", int Trans_id = 0, int OrgId = 0, int contactId = 0, int flagpoint = 0,
           int UserTypeId = 0, string UserId = "", long OrganisationId = 0, string OrganisationName = "", string UserName = "", string userSchema = "")
        {
            try
            {
                string userType = "Police"; //by default userTyoe is Police
                #region Function to fetch outbound document/ user type based on input parameters Trans_id and OrgId
                //Function Call to fetch xml document from outbound document for trans_id aka document id and usertype based on organisation id
                DocumentInfo XmlOutboundDoc = movementsService.ViewMovementDocument(Trans_id, OrgId, userSchema);
                UserInfo SessionInfo = new UserInfo();
                SessionInfo.UserSchema = userSchema;
                SessionInfo.UserId = UserId;
                SessionInfo.OrganisationId = OrganisationId;
                SessionInfo.OrganisationName = OrganisationName;
                SessionInfo.UserName = UserName;

                string xmlInformation = XmlOutboundDoc.XMLDocument;

                //if OrgId is 0 then by default police is selected else based on the output from stored procedure the usertype is considered.
                if (OrgId != 0 && XmlOutboundDoc.UserType != String.Empty)
                {
                    userType = XmlOutboundDoc.UserType;
                }
                #endregion

                if (xmlInformation != string.Empty)
                {
                    xmlInformation = xmlInformation.Replace(">?<", ">\u2002<");
                    xmlInformation = xmlInformation.Replace(">?##**##", ">##**##");
                }

                //string notificationDocument = null;
                string xsltPath = string.Empty;
                List<TransmissionModel> transmissionList = sortDocumentService.GetTransmissionType(Trans_id, "310001,310009,310008,310002,310007,310003,310005", 7, UserSchema.Portal);
                if (xmlInformation != string.Empty)
                {
                    if (userType == "Police")
                    {
                        xsltPath = "XSLT\\SpecialOrderAgreedRoute.xslt";

                        if (transmissionList.Any() && !string.IsNullOrEmpty(transmissionList[0].Fax))
                        {
                           // notificationDocument = documentService.GenerateHTMLPDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
                        }
                        else
                        {
                            if (flagpoint == 1)
                            {
                              //  ViewBag.HtmlStr = ViewSpecialOrderEmail(xmlInformation, xsltPath, contactId); //RM#4965                            
                                return View("../DistributionStatus/EmailTransmissionView");
                            }
                            else
                            {
                                //notificationDocument = documentService.GenerateHTMLPDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
                            }
                        }


                    }
                    else
                    {
                        //STP.Document.GenerateDocument genDocument = new Document.GenerateDocument();
                        xmlInformation = documentService.GetLoggedInUserAffectedStructureDetailsByESDALReference(xmlInformation, EsdalRefno, SessionInfo, UserSchema.Sort, "agreed", OrgId);

                        xsltPath = "XSLT\\SpecialOrderAgreedRoute.xslt";
                        if (transmissionList.Any() && !string.IsNullOrEmpty(transmissionList[0].Fax))
                        {
                           // notificationDocument = documentService.GenerateHTMLPDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "MovementPDF", SessionInfo);
                        }
                        else
                        {
                            if (flagpoint == 1)
                            {
                               // ViewBag.HtmlStr = ViewSpecialOrderEmail(xmlInformation, xsltPath, contactId); //RM#4965                            
                                return View("../DistributionStatus/EmailTransmissionView");
                            }
                            else
                            {
                               // notificationDocument = documentService.GenerateHTMLPDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "MovementPDF", SessionInfo);
                            }
                        }

                    }
                }

                if (xmlInformation != null)
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(xmlInformation);

                    MemoryStream stream = new MemoryStream(byteArray);

                    if(xmlInformation.Contains("<LegNumber>2</LegNumber>"))
                    {
                        //read configuration xml
                        XmlSerializer deserializer = new XmlSerializer(typeof(SortAgreeRouteParams.AgreedRoute));
                        SortAgreeRouteParams.AgreedRoute sortagreedRoute = null;

                        using (XmlReader reader = XmlReader.Create(stream))
                        {
                            sortagreedRoute = (SortAgreeRouteParams.AgreedRoute)deserializer.Deserialize(reader);
                        }

                        return View("~/Views/Document/SortAgreeRoute.cshtml ", sortagreedRoute);
                    }
                    else
                    {
                        // single route

                        XmlSerializer deserializer = new XmlSerializer(typeof(SortSingleAgreeRouteParam.AgreedRoute));
                        SortSingleAgreeRouteParam.AgreedRoute sortagreedRoute = null;

                        using (XmlReader reader = XmlReader.Create(stream))
                        {
                            sortagreedRoute = (SortSingleAgreeRouteParam.AgreedRoute)deserializer.Deserialize(reader);
                        }

                        return View("~/Views/Document/SortSingleAgreeRoute.cshtml ", sortagreedRoute);
                    }




                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data is not valid for current movement therefore PDF document will not be generated.');</script>");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ReportDocument/ViewAgreedReport, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion

        #region View Agree Revised
        public ActionResult ViewAmendmentToAgreementReport(string esdalRefno = "", int trans_id = 0, int OrgId = 0, int contactId = 0, int flagpoint = 0)
        {
            
                string messg = "ReportDocument/ViewAmendmentToAgreementReport?esdalRefno=" + esdalRefno + "trans_id=" + trans_id + "OrgId=" + OrgId + "contactId=" + contactId;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                string userType = "Police"; //by default userTyoe is Police
                

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                if (SessionInfo.UserTypeId == UserType.Sort && OrgId == 0 && contactId == 0)
                {
                    OrgId = (int)Session["SORTOrgID"];
                    contactId = movementsService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));
                }
                else if (SessionInfo.UserTypeId == UserType.PoliceALO)
                {
                    if (contactId == 0)
                    {
                        contactId = -1;
                    }
                    if (OrgId == 0)
                    {
                        OrgId = -1;
                    }
                }
          return new ActionAsPdf           
                ("SortViewReAgree", new
            {
                EsdalRefno = esdalRefno,
                Trans_id = trans_id,
                OrgId = OrgId,
                contactId = contactId,
                flagpoint = flagpoint,
                UserTypeId = SessionInfo.UserTypeId,
                UserId = SessionInfo.UserId,
                OrganisationId = SessionInfo.OrganisationId,
                OrganisationName = SessionInfo.OrganisationName,
                UserName = SessionInfo.UserName,
                userSchema = SessionInfo.UserSchema
            });

        }

   

        [HttpGet]
        public ActionResult SortViewReAgree(string EsdalRefno = "", int Trans_id = 0, int OrgId = 0, int contactId = 0, int flagpoint = 0,
          int UserTypeId = 0, string UserId = "", long OrganisationId = 0, string OrganisationName = "", string UserName = "", string userSchema = "")
        {
            try
            {
                string userType = "Police"; //by default userTyoe is Police
                #region Function to fetch outbound document/ user type based on input parameters Trans_id and OrgId
                //Function Call to fetch xml document from outbound document for trans_id aka document id and usertype based on organisation id
                DocumentInfo XmlOutboundDoc = movementsService.ViewMovementDocument(Trans_id, OrgId, userSchema);

                    string xmlInformation = XmlOutboundDoc.XMLDocument;

                    //if OrgId is 0 then by default police is selected else based on the output from stored procedure the usertype is considered.
                    if (OrgId != 0 && XmlOutboundDoc.UserType != String.Empty)
                    {
                        userType = XmlOutboundDoc.UserType;
                    }
                    #endregion

                    if (xmlInformation != string.Empty)
                    {
                        xmlInformation = xmlInformation.Replace(">?<", ">\u2002<");
                        xmlInformation = xmlInformation.Replace(">?##**##", ">##**##");
                    }

                    string notificationDocument = null;
                    string xsltPath = string.Empty;
                //RM#4965
                UserInfo SessionInfo = new UserInfo();
                SessionInfo.UserSchema = userSchema;
                SessionInfo.UserId = UserId;
                SessionInfo.OrganisationId = OrganisationId;
                SessionInfo.OrganisationName = OrganisationName;
                SessionInfo.UserName = UserName;
                List<TransmissionModel> transmissionList = sortDocumentService.GetTransmissionType(Trans_id, "310001,310009,310008,310002,310007,310003,310005", 7, UserSchema.Portal);
                    TransmittingDocumentDetails transmittingDetail = new TransmittingDocumentDetails();
                    transmittingDetail = documentService.SortSideCheckDoctype(Trans_id, SessionInfo.UserSchema);
                    if (xmlInformation != string.Empty)
                    {
                        if (userType == "Police")
                        {
                            if (transmittingDetail.DocumentType == "no longer affected")
                            {
                                xsltPath = "XSLT\\ReProposalNoLongerFAXPolice.xslt";
                            }
                            else
                            {
                                xsltPath = "XSLT\\RevisedAgreementFaxPolice.xslt";
                            }

                            if (transmissionList.Any() && !string.IsNullOrEmpty(transmissionList[0].Fax))
                            {
                               // notificationDocument = documentService.GenerateHTMLPDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
                            }
                            else
                            {
                                if (flagpoint == 1)
                                {
                                    //ViewBag.HtmlStr = ViewSpecialOrderEmail(xmlInformation, xsltPath, contactId); //RM#4965                            
                                    return View("../DistributionStatus/EmailTransmissionView");
                                }
                                else
                                {
                                   // notificationDocument = documentService.GenerateHTMLPDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
                                }
                            }


                        }
                        else
                        {
                            if (transmittingDetail.DocumentType == "no longer affected")
                            {
                              //  xsltPath = "XSLT\\Re_Proposal_No_Longer_FAX_SOA.xsl";
                            }
                            else
                            {
                                //STP.Document.GenerateDocument genDocument = new Document.GenerateDocument();
                                xmlInformation = documentService.GetLoggedInUserAffectedStructureDetailsByESDALReference(xmlInformation, EsdalRefno, SessionInfo, UserSchema.Sort, "agreed", OrgId);

                               xsltPath = "XSLT\\RevisedAgreement.xslt";
                            }
                            if (transmissionList.Any() && !string.IsNullOrEmpty(transmissionList[0].Fax))
                            {
                                //notificationDocument = documentService.GenerateHTMLPDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "MovementPDF", SessionInfo);
                            }
                            else
                            {
                                if (flagpoint == 1)
                                {
                                   // ViewBag.HtmlStr = ViewSpecialOrderEmail(xmlInformation, xsltPath, contactId); //RM#4965                            
                                    return View("../DistributionStatus/EmailTransmissionView");
                                }
                                else
                                {
                                    //notificationDocument = documentService.GenerateHTMLPDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "MovementPDF", SessionInfo);
                                }
                            }

                        }
                    }

                if (xmlInformation != null)
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(xmlInformation);

                    MemoryStream stream = new MemoryStream(byteArray);

                    ////read configuration xml
                    XmlSerializer deserializer = new XmlSerializer(typeof(SortAgreeRevisedParams.AgreedRoute));
                    SortAgreeRevisedParams.AgreedRoute sortagreedRevised = new SortAgreeRevisedParams.AgreedRoute();

                    using (XmlReader reader = XmlReader.Create(stream))
                    {
                        sortagreedRevised = (SortAgreeRevisedParams.AgreedRoute)deserializer.Deserialize(reader);
                    }
                    return View("~/Views/Document/SortAgreeRevised.cshtml ", sortagreedRevised);
                    
                }
                else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Data is not valid for current movement therefore PDF document will not be generated.');</script>");
                    }
                }
                catch (System.Exception ex)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ReportDocument/ViewAmendmentToAgreementReport, Exception: {0}", ex));
                    throw ex;
                }
            }

        #endregion


        public ActionResult ViewNotifDoc(string ESDALREf = null, int transmission_id = 0, long OrganisationId = 0, long ContactId = 0, string orgtype = null, string Is_manually_added = null, string fromOrgName = null)//byte[] Document = null, string org_type = null, string NotifType = null)
        {
            ESDALREf = ESDALREf.Replace("*", "#");
            DistributionAlerts DistributionObj = new DistributionAlerts();
            AccountController ObjAC = new AccountController();
            string NotifType = "s";
            List<string> arrlist = new List<string>();
            string xsltPath = string.Empty;
            byte[] notificationDocument = null;
            string xmlInformation = null;
            string DOCType = "";
            UserInfo SessionInfo = null;
            SessionInfo = (UserInfo)Session["UserInfo"];
            try
            {
                Enums.DocumentType doctype = new Enums.DocumentType();
                //DOCStatus = "Notification";
                //arrlist = ObjAC.Get_splitESDAL(ESDALREf);
                arrlist = ObjAC.GetSplitESDAL(ESDALREf);
                DistributionObj = documentService.GetNotifDetails(ESDALREf, transmission_id);
                if (DistributionObj.OutboundDocument == null)
                {
                    ViewBag.ErrorMessage = "Doc Error Occurred.";
                    return RedirectToAction("ViewDistributionStatus", "DistributionStatus", new { page = 1, pageSize = 10, ErrorMessage = ViewBag.ErrorMessage });
                }
                if (arrlist[2] == "1")
                {
                    NotifType = "Notif";
                }
                else
                {
                    NotifType = "Re-Notif";
                }

                try
                {
                    byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(DistributionObj.OutboundDocument);

                    xmlInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);
                }
                catch (System.Xml.XmlException XE)
                {
                    xmlInformation = Encoding.UTF8.GetString(DistributionObj.OutboundDocument, 0, DistributionObj.OutboundDocument.Length);
                }
                if (xmlInformation != string.Empty)
                {
                    xmlInformation = xmlInformation.Replace(">?<", ">\u2002<");
                    xmlInformation = xmlInformation.Replace(">?##**##", ">##**##");
                }
                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");
                    return RedirectToAction("Login", "Account");
                }

                if (orgtype == "SOA" && NotifType == "Notif")
                {
                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Notification_FAX_SOA_PDF.xsl";
                }
                else if (orgtype == "SOA" && NotifType == "Re-Notif")
                {
                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ReNotification.xslt";
                }
                else if (orgtype == "Police" && NotifType == "Notif")
                {
                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Notification_Fax_Police.xslt";
                }
                else if (orgtype == "Police" && NotifType == "Re-Notif")
                {
                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Re_Notification_Fax_Police.xslt";
                }
                if (Is_manually_added == "true")
                {
                    if (NotifType == "Notif")
                    {
                        xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Notification_Fax_Police.xslt";
                    }
                    else
                    {
                        xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Re_Notification_Fax_Police.xslt";
                    }

                }

                List<TransmissionModel> transmissionList = sortDocumentService.GetTransmissionType(transmission_id, "310001,310009,310008,310002,310007,310003,310005", 7, UserSchema.Portal);

                if (!string.IsNullOrEmpty(transmissionList[0].Fax))
                {
                    if (Is_manually_added == "1")
                    {
                        notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, 0, "", false, "", "", SessionInfo.VehicleUnits, "PDF");
                    }
                    else
                    {
                        notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", OrganisationId, (int)ContactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF");
                    }
                }
                else
                {
                    #region For Email View
                    string xmlinfo = xmlInformation;
                    string userType = "";
                    string documentType = "EMAIL";
                    string organisationName = "";
                    string HAReference = "";

                    xmlInformation = xmlInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\"><Underscore xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##bus##");
                    xmlInformation = xmlInformation.Replace("</Underscore></Bold>", "##bue##");

                    xmlInformation = xmlInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "#bst#");
                    xmlInformation = xmlInformation.Replace("</Bold>", "#be#");

                    xmlInformation = xmlInformation.Replace("<Underscore xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##us##");
                    xmlInformation = xmlInformation.Replace("<Underscore>", "##us##"); // RM#5232 changes
                    xmlInformation = xmlInformation.Replace("</Underscore>", "##ue##");
                    xmlInformation = xmlInformation.Replace("<Italic xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##is##");
                    xmlInformation = xmlInformation.Replace("<Italic>", "##is##");// RM#5232 changes
                    xmlInformation = xmlInformation.Replace("</Italic>", "##ie##");

                    xmlInformation = xmlInformation.Replace("<NotesForHaulier xmlns=\"http://www.esdal.com/schemas/core/movement\">##bs####us##", "<NotesForHaulier xmlns=\"http://www.esdal.com/schemas/core/movement\">##bssbr####us##");

                    xmlInformation = xmlInformation.Replace("<Underscore>", "");

                    xmlInformation = xmlInformation.Replace("<OnBehalfOf xmlns=\"http://www.esdal.com/schemas/core/movement\"> </OnBehalfOf>", "");
                    xmlInformation = xmlInformation.Replace("<OnBehalfOf> </OnBehalfOf>", "");

                    xmlInformation = xmlInformation.Replace("<Para xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##ps##");
                    xmlInformation = xmlInformation.Replace("</Para>", "##pe##");

                    xmlInformation = xmlInformation.Replace("<BulletedText xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##bts##");
                    xmlInformation = xmlInformation.Replace("</BulletedText>", "##bte##");

                    //Start For Bug #4319 </Br> tag
                    xmlInformation = xmlInformation.Replace("<Br />", "##br##").Replace("<Br></Br>", "##br##");
                    //End For Bug #4319 </Br> tag

                    xmlInformation = ConvertCountryLettersToSymbol(xmlInformation);

                    StringReader stringReader = new StringReader(xmlInformation);
                    XmlReader xmlReader = XmlReader.Create(stringReader);

                    XslCompiledTransform xslt = new XslCompiledTransform();
                    xslt.Load(xsltPath);

                    StringWriter sw = new StringWriter();
                    XmlTextWriter writer = new XmlTextWriter(sw);

                    XsltArgumentList argsList = new XsltArgumentList();

                    if (Is_manually_added == "1")
                    {
                        argsList.AddParam("Contact_ID", "", 0);
                    }
                    else
                    {
                        argsList.AddParam("Contact_ID", "", (int)ContactId);
                    }

                    if (SessionInfo.VehicleUnits == 692002 && userType == "Police" && xmlInformation.IndexOf("RouteImperial") != -1 && xmlInformation.IndexOf("OutboundNotification") != -1)
                    {
                        argsList.AddParam("UnitType", "", 692002);
                    }
                    else
                    {
                        argsList.AddParam("UnitType", "", SessionInfo.VehicleUnits);
                    }

                    argsList.AddParam("DocType", "", documentType);
                    argsList.AddParam("OrganisationName", "", organisationName);
                    argsList.AddParam("HAReferenceNumber", "", HAReference);

                    xslt.Transform(xmlReader, argsList, writer, null);

                    writer.Close();
                    writer = null;

                    string Attr = "id=\"hdr_img\"";
                    string ImgFilePath = Attr + " src=\"" + System.Configuration.ConfigurationSettings.AppSettings["DocumentImagePath"].ToString() + "\"";

                    string outputString = Convert.ToString(sw);

                    outputString = outputString.Replace(Attr, ImgFilePath);

                    outputString = outputString.Replace("##bus##", "<br/><br/><b><u>");
                    outputString = outputString.Replace("##bue##", "</u></b> ");

                    outputString = outputString.Replace("##bss##", "<b>");
                    outputString = outputString.Replace("##bssbr##", "<b>");

                    outputString = outputString.Replace("#bst#", "<b>");
                    outputString = outputString.Replace("#be#", "</b>");

                    outputString = outputString.Replace("##is##", "<i>");
                    outputString = outputString.Replace("##ie##", "</i>");
                    outputString = outputString.Replace("##us##", "<u>");
                    outputString = outputString.Replace("##ue##", "</u> ");

                    outputString = outputString.Replace("&amp;nbsp;", " ");

                    outputString = outputString.Replace("##ps##", "<p>");
                    outputString = outputString.Replace("##pe##", "</p>");

                    outputString = outputString.Replace("##bts##", "<ul><li>");
                    outputString = outputString.Replace("##bte##", "</li></ul>");

                    //Start For Bug #4319 </Br> tag
                    outputString = outputString.Replace("##br##", "<Br />");
                    //End For Bug #4319 </Br> tag

                    outputString = outputString.Replace("FACSIMILE MESSAGE", "Mail");

                    outputString = outputString.Replace("<b />", "");
                    outputString = outputString.Replace("#b#", "<b>");

                    outputString = outputString.Replace("?", " ");

                    //int n = STP.Document.Common.CommonMethods.GetNoofPages(outputString);

                    //outputString = outputString.Replace("###Noofpages###", n.ToString());

                    ViewBag.HtmlStr = outputString;

                    return View("../DistributionStatus/EmailTransmissionView");

                    #endregion

                }





                #region sys_events for saving loggin info for helpdesk redirect SOA, POLICE
                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                string ErrMsg = string.Empty;

                //if (SessionInfo.userSchema == UserSchema.Portal)
                // {

                //movactiontype.systemEventType = SysEventType.View;
                movactiontype.SystemEventType = SysEventType.View;
                movactiontype.UserId = Convert.ToInt32(SessionInfo.UserId);
                movactiontype.UserName = SessionInfo.UserName;
                movactiontype.ESDALRef = ESDALREf;
                movactiontype.TransmissionId = transmission_id;
                movactiontype.OrganisationNameSender = fromOrgName;
                movactiontype.OrganisationIdReciver = OrganisationId;
                movactiontype.ContactIdReciver = ContactId;

                if (NotifType == "Notif")
                    DOCType = "Notification";
                else if (NotifType == "Re-Notif")
                    DOCType = "Re-notification";
                else if (Is_manually_added == "1" && NotifType == "Notif")
                    DOCType = "Manually added notification";
                else if (Is_manually_added == "1" && NotifType == "Re-Notif")
                    DOCType = "Manually added re-notification";
                else
                    DOCType = "No doctype found";

                movactiontype.DocType = DOCType;
                string sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                int user_ID = Convert.ToInt32(SessionInfo.UserId);
                bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, SessionInfo.UserSchema);
                // }

                #endregion
                if (DistributionObj.OutboundDocument != null)
                {
                    System.IO.MemoryStream pdfStream = new System.IO.MemoryStream(notificationDocument);

                    return new FileStreamResult(pdfStream, "application/pdf");
                }
                else
                {

                    return Content("<script language='javascript' type='text/javascript'>alert('Data is not valid for current movement therefore PDF document will not be generated.');</script>");
                }


            }
            catch (System.Exception ex)
            {
                //Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("DistributionStatus/ViewNotifDoc, Exception: {0}", ex));
                //throw ex;
                ViewBag.ErrorMessage = "Generate Error Occurred.";
                return RedirectToAction("ViewDistributionStatus", "DistributionStatus", new { page = 1, pageSize = 10, ErrorMessage = ViewBag.ErrorMessage });
            }

        }


        #region  private static string ConvertCountryLettersToSymbol(string xmlDocument)
        private static string ConvertCountryLettersToSymbol(string xmlDocument)
        {
            xmlDocument = xmlDocument.Replace("#@GBP@#", "£");
            xmlDocument = xmlDocument.Replace("#@Pound@#", "€");
            xmlDocument = xmlDocument.Replace("#@GEL@#", "₾");
            xmlDocument = xmlDocument.Replace("#@BGN@#", "лв");
            xmlDocument = xmlDocument.Replace("#@Turkey@#", "₺");

            return xmlDocument;
        }
        #endregion
    }
}