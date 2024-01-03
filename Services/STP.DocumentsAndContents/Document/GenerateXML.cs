using Microsoft.XmlDiffPatch;
using STP.Common.Constants;
using STP.Common.Logger;
using STP.DocumentsAndContents.Persistance;
using STP.Domain.DocumentsAndContents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace STP.DocumentsAndContents.Document
{
    public class GenerateXML
    {

        XmlDocument OldXMLFile = new XmlDocument();
        XmlDocument NewXMLFile = new XmlDocument();

        #region GenerateSpecialOrderXML
        public XMLModel GenerateSpecialOrderXML(Enums.SOTemplateType soTemplateType, string esDAlRefNo, string orderNo = "0", string userSchema = UserSchema.Portal)
        {
            string retunXML = string.Empty;
            XMLModel model = new XMLModel();
            SpecialOrderXSD.SpecialOrderStructure sos = new SpecialOrderXSD.SpecialOrderStructure();

            sos = SpecialOrderDAO.GetSpecialOrderDetails(orderNo, esDAlRefNo, soTemplateType, userSchema);

            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(SpecialOrderXSD.SpecialOrderStructure));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, sos);
                    }
                    retunXML = textWriter.ToString(); //This is the output as a string
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            model.ReturnXML = retunXML;
            model.NotificationID = sos.NotificationID;
            return model;
        }
        #endregion

        #region GenerateProposalXML
        public XMLModel GenerateProposalXML(string esDAlRefNo, int organisationID, int contactId, string userSchema = UserSchema.Portal)
        {
            string retunXML = string.Empty;
            XMLModel model = new XMLModel();
            ProposedRouteXSD.ProposalStructure ps = new ProposedRouteXSD.ProposalStructure();

            ps = ProposalDAO.GetProposalRouteDetails(esDAlRefNo, organisationID, contactId, userSchema);

            ps.IsFailedDelegationAlert = false;
            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ProposedRouteXSD.ProposalStructure));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, ps);
                    }
                    retunXML = textWriter.ToString(); //This is the output as a string

                    StringReader stringReader = new StringReader(retunXML);
                    XmlReader xmlReader = XmlReader.Create(stringReader);

                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(xmlReader);

                    XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmldoc.NameTable);
                    nsMgr.AddNamespace("ns1", "http://www.esdal.com/schemas/core/proposedroute");
                    nsMgr.AddNamespace("ns2", "http://www.esdal.com/schemas/core/movement");

                    XmlNode root = xmldoc.SelectSingleNode("/ns1:Proposal", nsMgr);
                    XmlNode NotesForHaulier = xmldoc.CreateNode(XmlNodeType.Element, "NotesForHaulier", "http://www.esdal.com/schemas/core/movement");
                    try
                    {
                        string xml = GetProposalNotesforHaulier("", esDAlRefNo, userSchema);
                        xml = xml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");
                        xml = xml.Replace("<movementversion:NotesForHaulier xmlns:movementversion=\"http://www.esdal.com/schemas/common/movementversion\">", "");
                        xml = xml.Replace("</movementversion:NotesForHaulier>", "");

                        NotesForHaulier.InnerXml = xml;
                    }
                    catch (Exception ex)
                    {
                    }
                    root.AppendChild(NotesForHaulier);


                    retunXML = xmldoc.OuterXml.ToString();

                }
            }
            catch (Exception ex)
            {
            }

            model.ReturnXML = retunXML;
            model.NotificationID = ps.NotificationID;
            return model;
        }
        #endregion

        #region GenerateProposalXML
        public XMLModel GenerateProposalXML(string esDAlRefNo, int organisationID, int contactId, string distributionComments, string userSchema)
        {
            string retunXML = string.Empty;
            XMLModel model = new XMLModel();
            ProposedRouteXSD.ProposalStructure ps = new ProposedRouteXSD.ProposalStructure();

            ps = ProposalDAO.GetProposalRouteDetails(esDAlRefNo, organisationID, contactId, userSchema);

            if(!string.IsNullOrEmpty(distributionComments))
                ps.DistributionComments = distributionComments; //adding distribution comment's

            ps.IsFailedDelegationAlert = false;

            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ProposedRouteXSD.ProposalStructure));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, ps);
                    }
                    retunXML = textWriter.ToString(); //This is the output as a string

                    StringReader stringReader = new StringReader(retunXML);
                    XmlReader xmlReader = XmlReader.Create(stringReader);

                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(xmlReader);

                    XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmldoc.NameTable);
                    nsMgr.AddNamespace("ns1", "http://www.esdal.com/schemas/core/proposedroute");
                    nsMgr.AddNamespace("ns2", "http://www.esdal.com/schemas/core/movement");

                    XmlNode root = xmldoc.SelectSingleNode("/ns1:Proposal", nsMgr);
                    XmlNode NotesForHaulier = xmldoc.CreateNode(XmlNodeType.Element, "NotesForHaulier", "http://www.esdal.com/schemas/core/movement");
                    try
                    {
                        string xml = GetProposalNotesforHaulier("", esDAlRefNo, userSchema);
                        xml = xml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");
                        xml = xml.Replace("<movementversion:NotesForHaulier xmlns:movementversion=\"http://www.esdal.com/schemas/common/movementversion\">", "");
                        xml = xml.Replace("</movementversion:NotesForHaulier>", "");

                        NotesForHaulier.InnerXml = xml;
                    }
                    catch (Exception ex)
                    {
                    }
                    root.AppendChild(NotesForHaulier);


                    retunXML = xmldoc.OuterXml.ToString();

                }
            }
            catch (Exception ex)
            {
            }

            model.ReturnXML = retunXML;
            model.NotificationID = ps.NotificationID;
            return model;
        }
        #endregion

        #region GenerateRevisedAgreementXML
        public XMLModel GenerateRevisedAgreementXML(Enums.PortalType psPortalType, string esDAlRefNo, string orderNo = "0", int contactId = 0, string userSchema = UserSchema.Sort, string distributionComments = "")
        {
            string retunXML = string.Empty;
            XMLModel model = new XMLModel();
            AggreedRouteXSD.AgreedRouteStructure ars = new AggreedRouteXSD.AgreedRouteStructure();

            ars = RevisedAgreementDAO.GetRevisedAgreementDetails(orderNo, esDAlRefNo, contactId, userSchema);
            if (!string.IsNullOrEmpty(distributionComments))
                ars.DistributionComments = distributionComments;//RM#3646 
            ars.IsFailedDelegationAlert = false;
            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(AggreedRouteXSD.AgreedRouteStructure));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, ars);
                    }
                    retunXML = textWriter.ToString(); //This is the output as a string

                    StringReader stringReader = new StringReader(retunXML);
                    XmlReader xmlReader = XmlReader.Create(stringReader);

                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(xmlReader);

                    XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmldoc.NameTable);
                    nsMgr.AddNamespace("ns1", "http://www.esdal.com/schemas/core/agreedroute");
                    nsMgr.AddNamespace("ns2", "http://www.esdal.com/schemas/core/movement");

                    XmlNode root = xmldoc.SelectSingleNode("/ns1:AgreedRoute", nsMgr);
                    XmlNode NotesForHaulier = xmldoc.CreateNode(XmlNodeType.Element, "NotesForHaulier", "http://www.esdal.com/schemas/core/movement");
                    try
                    {
                        string xml = GetRevisedNotesforHaulier(ars.VersionID, userSchema);
                        xml = xml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");
                        xml = xml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>", "");
                        xml = xml.Replace("<movementversion:NotesForHaulier xmlns:movementversion=\"http://www.esdal.com/schemas/common/movementversion\">", "");
                        xml = xml.Replace("</movementversion:NotesForHaulier>", "");

                        NotesForHaulier.InnerXml = xml;
                    }
                    catch (Exception ex)
                    {
                    }

                    root.AppendChild(NotesForHaulier);


                    XmlDocument xmldoc1 = new XmlDocument();
                    xmldoc1 = xmldoc;

                    XmlNode root1 = xmldoc1.SelectSingleNode("/ns1:AgreedRoute/ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart", nsMgr);

                    XmlNode RouteDescription = xmldoc1.CreateNode(XmlNodeType.Element, "RouteDescription", "ns2");

                    try
                    {
                        string xml = GetRouteDescriptionInformation(orderNo, esDAlRefNo, userSchema);
                        xml = xml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");
                        xml = xml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>", "");
                        xml = xml.Replace("xmlns=\"http://www.esdal.com/schemas/common/routeappraisal\"", "");
                        xml = xml.Replace("xmlns:common=\"http://www.esdal.com/schemas/core/esdalcommontypes\"", "");
                        xml = xml.Replace("xmlns:route=\"http://www.esdal.com/schemas/core/routedescription\"", "");
                        xml = xml.Replace("<RoutePartsDescription   >", "");
                        xml = xml.Replace("route:", "");
                        xml = xml.Replace("common:", "");
                        xml = xml.Replace("<RoutePartsDescription>", "");
                        xml = xml.Replace("<RoutePartDescription>", "");
                        xml = xml.Replace("</RoutePartDescription>", "");
                        xml = xml.Replace("</RoutePartsDescription>", "");
                        RouteDescription.InnerXml = xml;
                    }
                    catch (Exception ex)
                    {
                    }

                    root1.AppendChild(RouteDescription);

                    retunXML = xmldoc1.OuterXml.ToString();

                    //Start Code Changes for RM#4574 and RM#4576
                    if (retunXML != null && retunXML != string.Empty)
                    {
                        retunXML = retunXML.Replace("<Br xmlns=\"\">", "<Br>");
                    }
                    //End Code Changes for RM#4574 and RM#4576

                    retunXML = retunXML.Replace("#bst#", "");
                    retunXML = retunXML.Replace("#be#", "");
                }
            }
            catch (Exception ex)
            {
            }

            Regex regexObj = new Regex(@">\s*<");
            retunXML = regexObj.Replace(retunXML, "><");

            model.ReturnXML = retunXML;

            long projectID = ars.ProjectID;
            long versionID = ars.VersionID;
            int versionStatus = ars.VersionStatus;

            //string ordernoesdalref = RevisedAgreementDAO.GetPreviousOrderDetails(projectID, versionID, versionStatus, userSchema);

            string referenceNumber = esDAlRefNo;

            string[] esdalRefNumber = esDAlRefNo.Split('/');

            string haulierMnemonic = string.Empty;
            string projectNumber = string.Empty;
            string versionNumber = string.Empty;

            if (esdalRefNumber.Length > 0)
            {
                haulierMnemonic = esdalRefNumber[0];
                projectNumber = esdalRefNumber[1].Replace("S", "");
                versionNumber = esdalRefNumber[2].Replace("S", "");

                versionNumber = Convert.ToString(Convert.ToInt32(versionNumber) - 1);
            }

            if (haulierMnemonic.IndexOf("SORT") == -1)
            {
                referenceNumber = haulierMnemonic + "/" + projectNumber + "/S" + versionNumber;
            }
            else
            {
                referenceNumber = haulierMnemonic + "/S" + projectNumber + "/S" + versionNumber;
            }

            retunXML = string.Empty;
            ars = RevisedAgreementDAO.GetRevisedAgreementDetails("", referenceNumber, contactId, userSchema);
            if (!string.IsNullOrEmpty(distributionComments))
                ars.DistributionComments = distributionComments;//RM#3646 
            ars.IsFailedDelegationAlert = false;
            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(AggreedRouteXSD.AgreedRouteStructure));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, ars);
                    }
                    retunXML = textWriter.ToString(); //This is the output as a string

                    StringReader stringReader = new StringReader(retunXML);
                    XmlReader xmlReader = XmlReader.Create(stringReader);

                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(xmlReader);

                    XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmldoc.NameTable);
                    nsMgr.AddNamespace("ns1", "http://www.esdal.com/schemas/core/agreedroute");
                    nsMgr.AddNamespace("ns2", "http://www.esdal.com/schemas/core/movement");

                    XmlNode root = xmldoc.SelectSingleNode("/ns1:AgreedRoute", nsMgr);
                    XmlNode NotesForHaulier = xmldoc.CreateNode(XmlNodeType.Element, "NotesForHaulier", "http://www.esdal.com/schemas/core/movement");
                    try
                    {
                        //long verID = RevisedAgreementDAO.GetPreviousOrderDetails(projectID, versionID, versionStatus);
                        string xml = GetRevisedNotesforHaulier(versionID, userSchema);
                        xml = xml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");
                        xml = xml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>", "");
                        xml = xml.Replace("xmlns=\"http://www.esdal.com/schemas/common/routeappraisal\"", "");
                        xml = xml.Replace("xmlns:common=\"http://www.esdal.com/schemas/core/esdalcommontypes\"", "");
                        xml = xml.Replace("xmlns:route=\"http://www.esdal.com/schemas/core/routedescription\"", "");
                        xml = xml.Replace("<movementversion:NotesForHaulier xmlns:movementversion=\"http://www.esdal.com/schemas/common/movementversion\">", "");
                        xml = xml.Replace("</movementversion:NotesForHaulier>", "");

                        NotesForHaulier.InnerXml = xml;
                    }
                    catch (Exception ex)
                    {
                    }

                    root.AppendChild(NotesForHaulier);


                    XmlDocument xmldoc1 = new XmlDocument();
                    xmldoc1 = xmldoc;

                    XmlNode root1 = xmldoc1.SelectSingleNode("/ns1:AgreedRoute/ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart", nsMgr);

                    XmlNode RouteDescription = xmldoc1.CreateNode(XmlNodeType.Element, "RouteDescription", "ns2");

                    try
                    {
                        string xml = GetRouteDescriptionInformation(orderNo, esDAlRefNo, userSchema);
                        xml = xml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");
                        xml = xml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>", "");
                        xml = xml.Replace("xmlns=\"http://www.esdal.com/schemas/common/routeappraisal\"", "");
                        xml = xml.Replace("xmlns:common=\"http://www.esdal.com/schemas/core/esdalcommontypes\"", "");
                        xml = xml.Replace("xmlns:route=\"http://www.esdal.com/schemas/core/routedescription\"", "");
                        xml = xml.Replace("<RoutePartsDescription   >", "");
                        xml = xml.Replace("route:", "");
                        xml = xml.Replace("common:", "");
                        xml = xml.Replace("<RoutePartsDescription>", "");
                        xml = xml.Replace("<RoutePartDescription>", "");
                        xml = xml.Replace("</RoutePartDescription>", "");
                        xml = xml.Replace("</RoutePartsDescription>", "");
                        RouteDescription.InnerXml = xml;
                    }
                    catch (Exception ex)
                    {
                    }

                    root1.AppendChild(RouteDescription);


                    retunXML = xmldoc1.OuterXml.ToString();


                    //Start Code Changes for RM#4574 and RM#4576
                    if (retunXML != null && retunXML != string.Empty)
                    {
                        retunXML = retunXML.Replace("<Br xmlns=\"\">", "<Br>");
                    }

                    retunXML = retunXML.Replace("#bst#", "");
                    retunXML = retunXML.Replace("#be#", "");

                    //End Code Changes for RM#4574 and RM#4576


                }
            }
            catch (Exception ex)
            {
            }

            retunXML = regexObj.Replace(retunXML, "><");
            model.OLDXML = retunXML;

            // dummy code started
            //var oldFileDocument = XDocument.Load(@"D:\ESDAL2\CloudeForge\STP_ProdLatest\STP.Web\Content\XML\2015-06-19-17-11-03-911_O.xml");
            //model.OLDXML = oldFileDocument.ToString();

            //retunXML = regexObj.Replace(model.OLDXML, "><");
            //retunXML = retunXML.Trim();
            //model.OLDXML = retunXML;

            //var newFileDocument = XDocument.Load(@"D:\ESDAL2\CloudeForge\STP_ProdLatest\STP.Web\Content\XML\2015-06-19-17-11-04-394_N.xml");
            //model.ReturnXML = newFileDocument.ToString();

            //retunXML = regexObj.Replace(model.ReturnXML, "><");
            //retunXML = retunXML.Trim();
            //model.ReturnXML = retunXML;
            // dummy code ended

            //Find number of Contacted Parties Changes for RM#4574 and RM#4576
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(model.OLDXML);
            XmlNodeList oldNode = xmlDoc.GetElementsByTagName("Contact");
            int totalOlderContacts = oldNode.Count;

            xmlDoc.LoadXml(model.ReturnXML);
            XmlNodeList newNode = xmlDoc.GetElementsByTagName("Contact");
            int totalNewContacts = newNode.Count;

            int totalDifference = 0;
            string xmlContent = string.Empty;

            if (totalOlderContacts != totalNewContacts)
            {
                // Changes to be done in the newer file
                if (totalOlderContacts > totalNewContacts)
                {
                    totalDifference = totalOlderContacts - totalNewContacts;

                    for (int totalRecords = 0; totalRecords < totalDifference; totalRecords++)
                    {
                        xmlContent = xmlContent + "<Contact ContactId=\"0\" OrganisationId=\"0\" Reason=\"newly affected\" IsRecipient=\"false\" IsPolice=\"false\" IsHaulier=\"false\" IsRetainedNotificationOnly=\"false\"><ContactName></ContactName><OrganisationName></OrganisationName><Fax /><Email></Email></Contact>";
                    }

                    if (xmlContent != string.Empty)
                    {
                        xmlContent = xmlContent + "</Recipients>";
                    }
                    model.ReturnXML = model.ReturnXML.Replace("</Recipients>", xmlContent);
                }
            }
            // Code ends here

            OldXMLFile.LoadXml(model.OLDXML);
            NewXMLFile.LoadXml(model.ReturnXML);

            string OldFilePath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XML\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff", System.Globalization.CultureInfo.InvariantCulture) + "_O.XML";
            string NewFilePath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XML\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff", System.Globalization.CultureInfo.InvariantCulture) + "_N.XML";

            OldXMLFile.Save(OldFilePath);
            NewXMLFile.Save(NewFilePath);

            string DifferenceFilePath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XML\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff", System.Globalization.CultureInfo.InvariantCulture) + "_D.XML";
            string MergeFilePath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XML\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff", System.Globalization.CultureInfo.InvariantCulture) + "_M.XML";

            model.ReturnXML = GenerateDiffGram(OldFilePath, NewFilePath, DifferenceFilePath, MergeFilePath);

            if (model.ReturnXML != null && model.ReturnXML != string.Empty)
            {
                model.ReturnXML = model.ReturnXML.ToString().Replace(@"<Contact ContactId=""0"" OrganisationId=""0"" Reason=""newly affected"" IsRecipient=""false"" IsPolice=""false"" IsHaulier=""false"" IsRetainedNotificationOnly=""false""><ContactName /><OrganisationName /><Fax /><Email /></Contact>", "");
                model.ReturnXML = model.ReturnXML.ToString().Replace(@"<Contact ContactId=""0"" OrganisationId=""0"" Reason=""newly affected"" IsRecipient=""false"" IsPolice=""false"" IsHaulier=""false"" IsRetainedNotificationOnly=""false""><ContactName></ContactName><OrganisationName></OrganisationName><Fax></Fax><Email></Email></Contact>", "");

                model.ReturnXML = model.ReturnXML.ToString().Replace(@"<Contact ContactId=""0"" IsHaulier=""false"" IsPolice=""false"" IsRecipient=""false"" IsRetainedNotificationOnly=""false"" OrganisationId=""0"" Reason=""newly affected""><ContactName /><OrganisationName /><Fax /><Email /></Contact>", "");
                model.ReturnXML = model.ReturnXML.ToString().Replace(@"<Contact ContactId=""0"" OrganisationId=""0"" Reason=""newly affected"" IsRecipient=""false"" IsPolice=""false"" IsHaulier=""false"" IsRetainedNotificationOnly=""false""><ContactName></ContactName><OrganisationName></OrganisationName><Fax /><Email></Email></Contact>", "");
            }

            if (System.IO.File.Exists(OldFilePath))
            {
                System.IO.File.Delete(OldFilePath);
            }

            if (System.IO.File.Exists(NewFilePath))
            {
                System.IO.File.Delete(NewFilePath);
            }

            if (System.IO.File.Exists(MergeFilePath))
            {
                System.IO.File.Delete(MergeFilePath);
            }

            if (System.IO.File.Exists(DifferenceFilePath))
            {
                System.IO.File.Delete(DifferenceFilePath);
            }

            model.NotificationID = ars.NotificationID;
            return model;
        }
        #endregion

        #region GenerateAgreedRoutetXML
        public XMLModel GenerateAgreedRoutetXML(Enums.PortalType psPortalType, string esDAlRefNo, string orderNo = "0", int contactId = 0, string userSchema = UserSchema.Sort, string distributionComments = "")//RM#3646
        
        {
            string retunXML = string.Empty;
            XMLModel model = new XMLModel();
            AggreedRouteXSD.AgreedRouteStructure ars = new AggreedRouteXSD.AgreedRouteStructure();

            ars = RevisedAgreementDAO.GetRevisedAgreementDetails(orderNo, esDAlRefNo, contactId, userSchema);
            if(!string.IsNullOrEmpty(distributionComments))
                ars.DistributionComments = distributionComments;//RM#3646
            ars.IsFailedDelegationAlert = false;
            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(AggreedRouteXSD.AgreedRouteStructure));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, ars);
                    }
                    retunXML = textWriter.ToString(); //This is the output as a string

                    StringReader stringReader = new StringReader(retunXML);
                    XmlReader xmlReader = XmlReader.Create(stringReader);

                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(xmlReader);

                    XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmldoc.NameTable);
                    nsMgr.AddNamespace("ns1", "http://www.esdal.com/schemas/core/agreedroute");
                    nsMgr.AddNamespace("ns2", "http://www.esdal.com/schemas/core/movement");

                    XmlNode root = xmldoc.SelectSingleNode("/ns1:AgreedRoute", nsMgr);
                    XmlNode NotesForHaulier = xmldoc.CreateNode(XmlNodeType.Element, "NotesForHaulier", "http://www.esdal.com/schemas/core/movement");
                    try
                    {
                        string xml = GetRevisedNotesforHaulier(ars.VersionID);
                        xml = xml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");
                        xml = xml.Replace("<movementversion:NotesForHaulier xmlns:movementversion=\"http://www.esdal.com/schemas/common/movementversion\">", "");
                        xml = xml.Replace("</movementversion:NotesForHaulier>", "");

                        NotesForHaulier.InnerXml = xml;
                    }
                    catch (Exception ex)
                    {
                    }

                    root.AppendChild(NotesForHaulier);


                    XmlDocument xmldoc1 = new XmlDocument();
                    xmldoc1 = xmldoc;

                    XmlNode root1 = xmldoc1.SelectSingleNode("/ns1:AgreedRoute/ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart", nsMgr);

                    XmlNode RouteDescription = xmldoc1.CreateNode(XmlNodeType.Element, "RouteDescription", "ns2");

                    try
                    {
                        string xml = GetRouteDescriptionInformation(orderNo, esDAlRefNo);
                        xml = xml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");
                        xml = xml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>", "");
                        xml = xml.Replace("xmlns=\"http://www.esdal.com/schemas/common/routeappraisal\"", "");
                        xml = xml.Replace("xmlns:common=\"http://www.esdal.com/schemas/core/esdalcommontypes\"", "");
                        xml = xml.Replace("xmlns:route=\"http://www.esdal.com/schemas/core/routedescription\"", "");
                        xml = xml.Replace("<RoutePartsDescription   >", "");
                        xml = xml.Replace("route:", "");
                        xml = xml.Replace("common:", "");
                        xml = xml.Replace("<RoutePartDescription>", "");
                        xml = xml.Replace("</RoutePartDescription>", "");
                        xml = xml.Replace("</RoutePartsDescription>", "");
                        RouteDescription.InnerXml = xml;
                    }
                    catch (Exception ex)
                    {
                    }

                    root1.AppendChild(RouteDescription);

                    retunXML = xmldoc1.OuterXml.ToString();
                }
            }
            catch (Exception ex)
            {
            }

            model.ReturnXML = retunXML;
            model.NotificationID = ars.NotificationID;
            return model;
        }
        #endregion

        #region GenerateNotificationFaxSOAXML
        public XMLModel GenerateNotificationFaxSOAXML(Enums.PortalType psPortalType, int NotificationID, bool isHaulier, int ContactId)
        {
            NotificationXSD.OutboundNotificationStructure obns = OutBoundDAO.GetOutboundNotificationDetailsForNotification(NotificationID, isHaulier, ContactId);
            obns.IsFailedDelegationAlert = false;
            obns.Type = psPortalType == Enums.PortalType.POLICE ? NotificationXSD.NotificationTypeType.police : NotificationXSD.NotificationTypeType.soa;
            XMLModel model = GenerateNotificationXML(NotificationID, obns, NotificationXSD.NotificationTypeType.police);
            model.NotificationID = NotificationID;
            GenerateUnSuitableStructureAndCautionsXmlByNotificationId(NotificationID, model, obns);

            return model;
        }
        #endregion

        #region GenerateUnSuitableStructureAndCautionsXmlByNotificationId
        private static void GenerateUnSuitableStructureAndCautionsXmlByNotificationId(int NotificationID, XMLModel model, NotificationXSD.OutboundNotificationStructure obns)
        {
            //loop through structure and get data
            if (obns != null && !string.IsNullOrEmpty(model.ReturnXML) && obns.RouteParts != null)
            {
                var structureXml = new StringBuilder();
                var cautionXml = new StringBuilder();

                var objRouteAssessmentModel = OutBoundDAO.GetCautionsXML(NotificationID);
                var cautionsObj = OutBoundDAO.GetCautionDataDetails(objRouteAssessmentModel);

                foreach (var item in obns.RouteParts)
                {
                    // get unsuitable structures items
                    var RoutePart = item.RoutePart;
                    if (RoutePart != null)
                    {
                        structureXml.Append(@"<Route>" +
                                       "<LegNumber>" + RoutePart.LegNumber + "</LegNumber> " +
                                       "<RoutePartName>" + RoutePart.Name + "</RoutePartName> ");
                        if (RoutePart.RoadPart != null && RoutePart.RoadPart.Structures != null && RoutePart.RoadPart.Structures.Structure != null)
                        {

                            var newAnalysedStructures = RoutePart.RoadPart.Structures;
                            var newCautions = cautionsObj;

                            GetUnsuitableStructuresWithCautions(false, false, ref newAnalysedStructures, ref newCautions);


                            var unSuitableStructures = newAnalysedStructures.Structure.Where(x => x.Appraisal != null &&
                            x.Appraisal.Suitability == NotificationXSD.SuitabilityType.unsuitable).ToList();
                            if (unSuitableStructures != null && unSuitableStructures.Any())
                            {
                                var unSuitableStructuresTotal = unSuitableStructures.Count;
                                var limit = 50;
                                var listunSuitableStructures = unSuitableStructures.Select(x => x.ESRN).Distinct().ToList();
                                var listunSuitableStructuresCount = listunSuitableStructures.Count;
                                var esrnListString = string.Join(",", listunSuitableStructures);
                                var structDetailsList = OutBoundDAO.GetStructureGeneralDetailListbyMultipleESRN(esrnListString, listunSuitableStructuresCount);
                                foreach (var structureDetails in structDetailsList)
                                {
                                    var additionalDetails = new StringBuilder();
                                    if (structureDetails != null)
                                    {
                                        //get reason
                                        string reason="";
                                        if (unSuitableStructuresTotal < limit)//to avoid timeout issue
                                        {
                                            reason = GetReasonByStructureIdAndRoutePart(RoutePart.Id, structureDetails.StructureId);
                                        }

                                        additionalDetails.Append(
                                           @" <StructureName>" + structureDetails.StructureName + "</StructureName> " +
                                           " <StructureKey>" + structureDetails.StructureKey + "</StructureKey> " +
                                           " <StructureDescription>" + structureDetails.StructureDescription + "</StructureDescription> " +
                                           " <StructureClass>" + structureDetails.StructureClass + "</StructureClass> " +
                                           " <OSGR>" + structureDetails.OSGR + "</OSGR> " +
                                           " <Easting>" + structureDetails.Easting + "</Easting> " +
                                           " <OwnerName>" + structureDetails.OwnerName + "</OwnerName> " +
                                           " <StructureCategory>" + structureDetails.StructureCategory + "</StructureCategory> " +
                                           " <StructureLength>" + structureDetails.StructureLength + "</StructureLength> " +
                                           "" + reason + ""
                                           );
                                    }
                                    var str = unSuitableStructures.Where(x => x.ESRN == structureDetails.ESRN).FirstOrDefault();
                                    if (str != null)
                                    {
                                        structureXml.Append(@"<Structure>" +
                                               "<ESRN>" + str.ESRN + "</ESRN> " +
                                               "<Name>" + str.Name + "</Name>" +
                                               "" + additionalDetails.ToString() + "" +
                                               "<Suitability>" + (str.Appraisal != null ? str.Appraisal.Suitability : NotificationXSD.SuitabilityType.unsuitable) + "</Suitability>" +
                                               "<Organisation>" + (str.Appraisal != null ? str.Appraisal.Organisation : "") + "</Organisation>" +
                                            "</Structure>");
                                    }
                                }
                            }
                        }
                        structureXml.Append(@"</Route>");

                        // get cautions
                        if (RoutePart != null && RoutePart.RoadPart != null && cautionsObj != null)
                        {
                            var caution = cautionsObj.AnalysedCautionsPart.Where(x => x.Id == item.RoutePart.Id).ToList();
                            if (caution != null && caution.Any())
                            {
                                string xmlVal = StringExtractor.XmlCautionSerializer(caution);//create xml
                                if (!string.IsNullOrEmpty(xmlVal))
                                    cautionXml.Append(xmlVal);
                            }

                        }
                    }
                }
                if (structureXml != null && structureXml.Length > 0)
                {
                    model.ReturnXML = model.ReturnXML.Replace("</OutboundNotification>", "<UnSuitableStructures>");

                    structureXml = structureXml.Replace("&", "and");
                    model.ReturnXML += structureXml.ToString();

                    model.ReturnXML += "</UnSuitableStructures></OutboundNotification>";
                }
                if (cautionXml != null && cautionXml.Length > 0)
                {
                    model.ReturnXML = model.ReturnXML.Replace("</OutboundNotification>", "<Cautions>");

                    cautionXml = cautionXml.Replace("&", "and");
                    cautionXml = cautionXml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                    cautionXml = cautionXml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
                    model.ReturnXML += cautionXml.ToString();

                    model.ReturnXML += "</Cautions></OutboundNotification>";
                }
            }
        }
        #endregion

        #region GetUnsuitableStructuresWithCautions
        public static void GetUnsuitableStructuresWithCautions(bool UnSuitableShowAllStructures, bool UnSuitableShowAllCautions, ref NotificationXSD.AffectedStructuresStructure newAnalysedStructures, ref Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions newCautions)
        {
            try
            {
                newCautions = GetUnsuitableCautions(newCautions, UnSuitableShowAllCautions);

                //foreach (var item in newAnalysedStructures.Structure)
                //{
                    List<NotificationXSD.AffectedStructuresStructure> unsuitStr = new List<NotificationXSD.AffectedStructuresStructure>();
                    foreach (var structure in newAnalysedStructures.Structure)
                    {
                        var ESRN = structure.ESRN;

                        if (newCautions != null && newCautions.AnalysedCautionsPart != null && newCautions.AnalysedCautionsPart.Any())
                        {
                            foreach (var itemCaution in newCautions.AnalysedCautionsPart)
                            {
                                if (itemCaution.Caution != null && itemCaution.Caution.Any())
                                {
                                    var analysedCautionStructures = itemCaution.Caution.Where(x => x.CautionedEntity1 != null && x.CautionedEntity1.AnalysedCautionStructureStructure != null
                                    && x.CautionedEntity1.AnalysedCautionStructureStructure.ESRN == ESRN).ToList();
                                    if (analysedCautionStructures != null)
                                    {
                                        //structure.AnalysedCautions = analysedCautionStructures;

                                        //if caution is unsuitable, set structure as unsuitable
                                        var isUnsuitableCautionExist = analysedCautionStructures.Where(x => x.Vehicle != null &&
                                                        x.Vehicle.Any(y => y.StartsWith("Unsuitable") || y.StartsWith("Not Specified") || y.ToLower() == "unsuitable" || y.ToLower() == "not specified")).Any();
                                        if (isUnsuitableCautionExist && structure.Appraisal != null)
                                        {
                                            structure.Appraisal.Suitability = NotificationXSD.SuitabilityType.unsuitable;
                                            //foreach (var itemapp in structure.Appraisal)
                                            //{
                                            //    itemapp.AppraisalSuitability.Value = "Unsuitable";
                                            //}
                                        }


                                    }
                                }
                            }
                        }
                    }
                //}

                if (UnSuitableShowAllStructures == false)
                    newAnalysedStructures = GetUnsuitableStructure(newAnalysedStructures);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "GetUnsuitableStructuresWithCautions:" + ex);
            }

        }
        #endregion

        #region GetUnsuitableStructure
        public static NotificationXSD.AffectedStructuresStructure GetUnsuitableStructure(NotificationXSD.AffectedStructuresStructure affectedStructures)
        {
            try
            {
                if (affectedStructures != null)
                {
                   var unSuitableStructures = affectedStructures.Structure.Where(x => x.Appraisal != null &&
                            x.Appraisal.Suitability == NotificationXSD.SuitabilityType.unsuitable|| x.Appraisal.Suitability == NotificationXSD.SuitabilityType.unknown).ToArray();
                   affectedStructures.Structure = unSuitableStructures;
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "GetUnsuitableStructure:" + ex);
            }
            return affectedStructures;
        }
        #endregion

        #region GetUnsuitableCautions
        public static STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions GetUnsuitableCautions(STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions analysedCautions, bool UnSuitableShowAllCautions)
        {
            try
            {
                if (analysedCautions != null)
                {
                    foreach (var item in analysedCautions.AnalysedCautionsPart)
                    {
                        if (UnSuitableShowAllCautions == false)
                        {
                            List<STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautionStructure> unsuitStr = new List<STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautionStructure>();
                            foreach (var cautionStructure in item.Caution)
                            {
                                var ve = cautionStructure.Vehicle;
                                var isSunsuitable = false;
                                if ((cautionStructure.Vehicle != null && cautionStructure.Vehicle.Where(x => x.StartsWith("Unsuitable")).Any()))
                                {
                                    unsuitStr.Add(cautionStructure);
                                    isSunsuitable = true;
                                }

                                //Generic caution
                                if (!isSunsuitable && (cautionStructure.ConstrainingAttribute == null || cautionStructure.ConstrainingAttribute.Count == 0))
                                {
                                    cautionStructure.Vehicle[0] = "Not Specified";// "Unsuitable (Generic caution)";
                                    unsuitStr.Add(cautionStructure);
                                }
                            }
                            item.Caution = unsuitStr;
                        }
                        else
                        {
                            //For Generic cautions
                            List<STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautionStructure> cautionStrList = new List<STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautionStructure>();
                            foreach (var cautionStructure in item.Caution)
                            {
                                //Generic caution
                                if (cautionStructure.ConstrainingAttribute == null || cautionStructure.ConstrainingAttribute.Count == 0)
                                    cautionStructure.Vehicle[0] = "Not Specified"; //"Unsuitable (Generic caution)";
                                cautionStrList.Add(cautionStructure);
                            }
                            item.Caution = cautionStrList;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "GetUnsuitableCautions:" + ex);
            }

            return analysedCautions;
        }
        #endregion

        #region GetReasonByStructureIdAndRoutePart
        private static string GetReasonByStructureIdAndRoutePart(int routePartId, long structureId)
        {
            var reason = OutBoundDAO.ViewUnsuitableStructSections(structureId, routePartId, 0);
            var msg = "";
            if (reason != null)
            {
                foreach (var item in reason)
                {
                    string sectionType = item.StructureSections;
                    if (item.AffectFlag > 0)
                    {
                        if (item.AffectFlag == 1)
                            msg += ""+ sectionType + " - Vehicle gross weight exceeds structure's gross weight, so this structure is unsuitable";
                        else if (item.AffectFlag == 2)
                            msg += "" + sectionType + " - Vehicle gross weight exceeds structure's signed gross weight, so this structure is unsuitable";
                        else if (item.AffectFlag == 3)
                            msg += ""+ sectionType + " - Vehicle maximum axle weight exceeds structure's axle weight, so this structure is unsuitable";
                        else if (item.AffectFlag == 4)
                            msg += ""+ sectionType + " - Vehicle maximum axle weight exceeds structure's signed axle weight, so this structure is unsuitable";
                        else if (item.AffectFlag == 5)
                            msg += ""+ sectionType + " - Vehicle height exceeds structure's height, so this structure is unsuitable";
                        else if (item.AffectFlag == 6)
                            msg += ""+ sectionType + " - Vehicle height exceeds structure's signed height, so this structure is unsuitable";
                        else if (item.AffectFlag == 7)
                            msg += ""+ sectionType + " - Vehicle width exceeds structure's width, so this structure is unsuitable";
                        else if (item.AffectFlag == 8)
                            msg += ""+ sectionType + " - Vehicle width exceeds structure's signed width, so this structure is unsuitable";
                        else if (item.AffectFlag == 9)
                            msg += ""+ sectionType + " - Vehicle length exceeds structure's length, so this structure is unsuitable";
                        else if (item.AffectFlag == 10)
                            msg += ""+ sectionType + " - Vehicle length exceeds structure's signed length, so this structure is unsuitable";
                        else if (item.AffectFlag == 11)
                            msg += ""+ sectionType + " - Vehicle’s axle weights exceeds structure’s maximum weight over distance, so this structure is unsuitable";
                        else if (item.AffectFlag == 12)
                            msg += ""+ sectionType + " - Vehicle gross weight doesn't satisfy SV screening, so this structure is unsuitable";

                        if (!string.IsNullOrEmpty(msg))
                            msg = msg + ",";
                    }
                }
            }
            msg = msg.TrimEnd(',');
            msg = "<Reason>"+ msg + "</Reason>";
            return msg;
        }
        #endregion

        #region GenerateOutboundNotificationStructureData
        public NotificationXSD.OutboundNotificationStructure GenerateOutboundNotificationStructureData(int NotificationId, bool isHaulier, long ContactId)
        {
            NotificationXSD.OutboundNotificationStructure obns = OutBoundDAO.GetOutboundNotificationDetailsForNotification(NotificationId, isHaulier, ContactId);
            obns.IsFailedDelegationAlert = false;
            return obns;
        }
        #endregion

        #region GenerateNotificationXML
        public XMLModel GenerateNotificationXML(int notificationId, NotificationXSD.OutboundNotificationStructure obns, NotificationXSD.NotificationTypeType psPortalType)
        {
            string retunXML = string.Empty;
            XMLModel model = new XMLModel();
            obns.Type = psPortalType;

            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(NotificationXSD.OutboundNotificationStructure));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, obns);
                    }
                    retunXML = textWriter.ToString(); //This is the output as a string
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            model.ReturnXML = retunXML;
            model.NotificationID = notificationId;
            return model;
        }
        #endregion

        #region GenerateReNotificationFaxSOAXML
        public XMLModel GenerateReNotificationFaxSOAXML(int NotificationID, bool isHaulier, int contactId)
        {
            NotificationXSD.OutboundNotificationStructure obns = OutBoundDAO.GetOutboundNotificationDetailsForNotification(NotificationID, isHaulier, contactId);
            obns.IsFailedDelegationAlert = false;
            obns.Type = NotificationXSD.NotificationTypeType.police;
            XMLModel model = GenerateNotificationXML(NotificationID, obns, NotificationXSD.NotificationTypeType.police);
            
            obns = OutBoundDAO.GetOutboundNotificationDetailsForNotification(Convert.ToInt32(obns.OldNotificationID), isHaulier, contactId);
            obns.IsFailedDelegationAlert = false;
            obns.Type = NotificationXSD.NotificationTypeType.police;

            XMLModel modelOld = GenerateNotificationXML(Convert.ToInt32(obns.OldNotificationID), obns, NotificationXSD.NotificationTypeType.police);

            model.OLDXML = modelOld.ReturnXML;

            OldXMLFile.LoadXml(model.OLDXML);
            NewXMLFile.LoadXml(model.ReturnXML);

            string OldFilePath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XML\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff", System.Globalization.CultureInfo.InvariantCulture) + "_O.XML";
            string NewFilePath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XML\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff", System.Globalization.CultureInfo.InvariantCulture) + "_N.XML";

            OldXMLFile.Save(OldFilePath);
            NewXMLFile.Save(NewFilePath);

            string DifferenceFilePath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XML\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff", System.Globalization.CultureInfo.InvariantCulture) + "_D.XML";
            string MergeFilePath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XML\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff", System.Globalization.CultureInfo.InvariantCulture) + "_M.XML";


            model.ReturnXML = GenerateDiffGram(OldFilePath, NewFilePath, DifferenceFilePath, MergeFilePath);

            if (File.Exists(OldFilePath))
                File.Delete(OldFilePath);

            if (File.Exists(NewFilePath))
                File.Delete(NewFilePath);

            if (File.Exists(MergeFilePath))
                File.Delete(MergeFilePath);

            if (File.Exists(DifferenceFilePath))
                File.Delete(DifferenceFilePath);

            model.NotificationID = NotificationID;
            return model;
        }
        #endregion

        #region GenerateReProposalStillAffectedFAXSOAXML
        public XMLModel GenerateReProposalStillAffectedFAXSOAXML(int ProjectID, Enums.PortalType psPortalType, int ContactID, int versionNo, string userSchema = UserSchema.Sort, string DistributionComments = "")
        {
            string retunXML = string.Empty;
            XMLModel model = new XMLModel();
            ProposedRouteXSD.ProposalStructure ps = new ProposedRouteXSD.ProposalStructure();

            ps = ReProposalStillAffectedDAO.GetProposalRouteDetails(ProjectID, psPortalType, ContactID, versionNo, userSchema);
            if(!string.IsNullOrEmpty(DistributionComments))
                ps.DistributionComments = DistributionComments;//RM#3646 - adding distribution comment's
            
            ps.IsFailedDelegationAlert = false;
            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ProposedRouteXSD.ProposalStructure));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, ps);
                    }
                    retunXML = textWriter.ToString(); //This is the output as a string

                    StringReader stringReader = new StringReader(retunXML);
                    XmlReader xmlReader = XmlReader.Create(stringReader);

                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(xmlReader);

                    XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmldoc.NameTable);
                    nsMgr.AddNamespace("ns1", "http://www.esdal.com/schemas/core/proposedroute");
                    nsMgr.AddNamespace("ns2", "http://www.esdal.com/schemas/core/movement");

                    XmlNode root = xmldoc.SelectSingleNode("/ns1:Proposal", nsMgr);
                    XmlNode NotesForHaulier = xmldoc.CreateNode(XmlNodeType.Element, "NotesForHaulier", "http://www.esdal.com/schemas/core/movement");
                    try
                    {
                        string xml = GetReProposalNotesforHaulier(ProjectID, versionNo, userSchema);
                        xml = xml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");
                        xml = xml.Replace("<movementversion:NotesForHaulier xmlns:movementversion=\"http://www.esdal.com/schemas/common/movementversion\">", "");
                        xml = xml.Replace("</movementversion:NotesForHaulier>", "");

                        NotesForHaulier.InnerXml = xml;
                    }
                    catch (Exception ex)
                    {
                    }
                    root.AppendChild(NotesForHaulier);

                    retunXML = xmldoc.OuterXml.ToString();

                    //Start Code Changes for RM#4574 and RM#4576
                    if (retunXML != null && retunXML != string.Empty)
                    {
                        retunXML = retunXML.Replace("<Br xmlns=\"\">", "<Br>");
                    }
                    //End Code Changes for RM#4574 and RM#4576
                }
            }
            catch (Exception ex)
            {
            }

            Regex regexObj = new Regex(@">\s*<");
            retunXML = regexObj.Replace(retunXML, "><");

            model.ReturnXML = retunXML;

            retunXML = string.Empty;
            ps = ReProposalStillAffectedDAO.GetProposalRouteDetails(ProjectID, psPortalType, ContactID, versionNo - 1, userSchema);
            if(!string.IsNullOrEmpty(DistributionComments))
                ps.DistributionComments = DistributionComments;//RM#3646
            ps.IsFailedDelegationAlert = false;
            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ProposedRouteXSD.ProposalStructure));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, ps);
                    }
                    retunXML = textWriter.ToString(); //This is the output as a string

                    StringReader stringReader = new StringReader(retunXML);
                    XmlReader xmlReader = XmlReader.Create(stringReader);

                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(xmlReader);

                    XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmldoc.NameTable);
                    nsMgr.AddNamespace("ns1", "http://www.esdal.com/schemas/core/proposedroute");
                    nsMgr.AddNamespace("ns2", "http://www.esdal.com/schemas/core/movement");

                    XmlNode root = xmldoc.SelectSingleNode("/ns1:Proposal", nsMgr);
                    XmlNode NotesForHaulier = xmldoc.CreateNode(XmlNodeType.Element, "NotesForHaulier", "http://www.esdal.com/schemas/core/movement");
                    try
                    {
                        string xml = GetReProposalNotesforHaulier(ProjectID, versionNo, userSchema);
                        xml = xml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");
                        xml = xml.Replace("<movementversion:NotesForHaulier xmlns:movementversion=\"http://www.esdal.com/schemas/common/movementversion\">", "");
                        xml = xml.Replace("</movementversion:NotesForHaulier>", "");

                        NotesForHaulier.InnerXml = xml;
                    }
                    catch (Exception ex)
                    {
                    }
                    root.AppendChild(NotesForHaulier);


                    retunXML = xmldoc.OuterXml.ToString();


                    //Start Code Changes for RM#4574 and RM#4576
                    if (retunXML != null && retunXML != string.Empty)
                    {
                        retunXML = retunXML.Replace("<Br xmlns=\"\">", "<Br>");
                    }
                    //End Code Changes for RM#4574 and RM#4576
                }
            }
            catch (Exception ex)
            {
            }

            retunXML = regexObj.Replace(retunXML, "><");

            retunXML = retunXML.Trim();

            model.OLDXML = retunXML;

            // dummy code started
            //var oldFileDocument = XDocument.Load(@"D:\ESDAL2\CloudeForge\STP_ProdLatest\STP.Web\Content\XML\2015-06-19-17-11-03-911_O.xml");
            //model.OLDXML = oldFileDocument.ToString();

            //retunXML = regexObj.Replace(model.OLDXML, "><");
            //retunXML = retunXML.Trim();
            //model.OLDXML = retunXML;

            //var newFileDocument = XDocument.Load(@"D:\ESDAL2\CloudeForge\STP_ProdLatest\STP.Web\Content\XML\2015-06-19-17-11-04-394_N.xml");
            //model.ReturnXML = newFileDocument.ToString();

            //retunXML = regexObj.Replace(model.ReturnXML, "><");
            //retunXML = retunXML.Trim();
            //model.ReturnXML = retunXML;
            // dummy code ended

            //Find number of Contacted Parties

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(model.OLDXML);
            XmlNodeList oldNode = xmlDoc.GetElementsByTagName("Contact");
            int totalOlderContacts = oldNode.Count;

            xmlDoc.LoadXml(model.ReturnXML);
            XmlNodeList newNode = xmlDoc.GetElementsByTagName("Contact");
            int totalNewContacts = newNode.Count;

            int totalDifference = 0;
            string xmlContent = string.Empty;

            if (totalOlderContacts != totalNewContacts)
            {
                // Changes to be done in the newer file
                if (totalOlderContacts > totalNewContacts)
                {
                    totalDifference = totalOlderContacts - totalNewContacts;

                    for (int totalRecords = 0; totalRecords < totalDifference; totalRecords++)
                    {
                        xmlContent = xmlContent + "<Contact ContactId=\"0\" OrganisationId=\"0\" Reason=\"newly affected\" IsRecipient=\"false\" IsPolice=\"false\" IsHaulier=\"false\" IsRetainedNotificationOnly=\"false\"><ContactName></ContactName><OrganisationName></OrganisationName><Fax /><Email></Email></Contact>";
                    }

                    if (xmlContent != string.Empty)
                    {
                        xmlContent = xmlContent + "</Recipients>";
                    }
                    model.ReturnXML = model.ReturnXML.Replace("</Recipients>", xmlContent);
                }
            }

            // Code ends here

            OldXMLFile.LoadXml(model.OLDXML);
            NewXMLFile.LoadXml(model.ReturnXML);

            string OldFilePath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XML\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff", System.Globalization.CultureInfo.InvariantCulture) + "_O.XML";
            string NewFilePath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XML\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff", System.Globalization.CultureInfo.InvariantCulture) + "_N.XML";

            OldXMLFile.Save(OldFilePath);
            NewXMLFile.Save(NewFilePath);

            string DifferenceFilePath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XML\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff", System.Globalization.CultureInfo.InvariantCulture) + "_D.XML";
            string MergeFilePath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XML\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff", System.Globalization.CultureInfo.InvariantCulture) + "_M.XML";

            model.ReturnXML = GenerateDiffGram(OldFilePath, NewFilePath, DifferenceFilePath, MergeFilePath);

            if (model.ReturnXML != null && model.ReturnXML != string.Empty)
            {
                model.ReturnXML = model.ReturnXML.ToString().Replace(@"<Contact ContactId=""0"" OrganisationId=""0"" Reason=""newly affected"" IsRecipient=""false"" IsPolice=""false"" IsHaulier=""false"" IsRetainedNotificationOnly=""false""><ContactName /><OrganisationName /><Fax /><Email /></Contact>", "");
                model.ReturnXML = model.ReturnXML.ToString().Replace(@"<Contact ContactId=""0"" OrganisationId=""0"" Reason=""newly affected"" IsRecipient=""false"" IsPolice=""false"" IsHaulier=""false"" IsRetainedNotificationOnly=""false""><ContactName></ContactName><OrganisationName></OrganisationName><Fax></Fax><Email></Email></Contact>", "");

                model.ReturnXML = model.ReturnXML.ToString().Replace(@"<Contact ContactId=""0"" IsHaulier=""false"" IsPolice=""false"" IsRecipient=""false"" IsRetainedNotificationOnly=""false"" OrganisationId=""0"" Reason=""newly affected""><ContactName /><OrganisationName /><Fax /><Email /></Contact>", "");
                model.ReturnXML = model.ReturnXML.ToString().Replace(@"<Contact ContactId=""0"" OrganisationId=""0"" Reason=""newly affected"" IsRecipient=""false"" IsPolice=""false"" IsHaulier=""false"" IsRetainedNotificationOnly=""false""><ContactName></ContactName><OrganisationName></OrganisationName><Fax /><Email></Email></Contact>", "");
            }

            if (System.IO.File.Exists(OldFilePath))
            {
                System.IO.File.Delete(OldFilePath);
            }

            if (System.IO.File.Exists(NewFilePath))
            {
                System.IO.File.Delete(NewFilePath);
            }

            if (System.IO.File.Exists(MergeFilePath))
            {
                System.IO.File.Delete(MergeFilePath);
            }

            if (System.IO.File.Exists(DifferenceFilePath))
            {
                System.IO.File.Delete(DifferenceFilePath);
            }

            return model;
        }
        #endregion

        #region GenerateReProposalNoLongerAffectedFAXSOAXML
        public XMLModel GenerateReProposalNoLongerAffectedFAXSOAXML(int ProjectID, Enums.PortalType psPortalType, int ContactID, int versionNo, string userSchema = UserSchema.Sort)
        {
            string retunXML = string.Empty;
            XMLModel model = new XMLModel();
            NoLongerAffectedXSD.NoLongerAffectedStructure obns = new NoLongerAffectedXSD.NoLongerAffectedStructure();

            obns = ReProposalNoLongerAffectedDAO.GetNoLongerAfftectedDetailsForNotification(ProjectID, psPortalType, ContactID, versionNo, userSchema);

            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(NoLongerAffectedXSD.NoLongerAffectedStructure));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, obns);
                    }
                    retunXML = textWriter.ToString(); //This is the output as a string
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            model.ReturnXML = retunXML;
            return model;
        }
        #endregion

        #region GenerateAmendmentOrderXML
        public XMLModel GenerateAmendmentOrderXML(string orderNo)
        {
            string retunXML = string.Empty;
            XMLModel model = new XMLModel();
            SpecialOrderXSD.SpecialOrderStructure sos = new SpecialOrderXSD.SpecialOrderStructure();

            sos = AmendmentOrderDAO.GetAmendmentOrderDetails(orderNo, 0);

            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(SpecialOrderXSD.SpecialOrderStructure));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, sos);
                    }
                    retunXML = textWriter.ToString(); //This is the output as a string
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"GenerateAmendmentOrderXML, Exception: ", ex);

            }
            model.ReturnXML = retunXML;
            model.NotificationID = sos.NotificationID;
            return model;
        }
        #endregion

        #region Code Commented by Mahzeer on 12/07/2023
        /*
        public XMLModel GenerateDailyDigestFAXXML(int ContactID, string NotificationID)
        {
            string retunXML = string.Empty;
            XMLModel model = new XMLModel();
            DailyDigestXSD.DigestStructure dds = new DailyDigestXSD.DigestStructure();

            dds = DailyDigestDAO.GetDailyDigestDetails(ContactID, NotificationID);

            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(DailyDigestXSD.DigestStructure));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, dds);
                    }
                    retunXML = textWriter.ToString(); //This is the output as a string
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            model.ReturnXML = retunXML;
            model.NotificationID = 0;
            return model;
        }

        public XMLModel GenerateImminentMoveAlertXML(int ContactID, int NotificationID, int LoggedInOrganisation)
        {
            string retunXML = string.Empty;
            XMLModel model = new XMLModel();
            ImminentMoveAlert.ImminentMoveAlertStructure imas = new ImminentMoveAlert.ImminentMoveAlertStructure();

            imas = ImminentMoveAlertDAO.GetImminentMovAlertDetails(ContactID, NotificationID, LoggedInOrganisation);

            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ImminentMoveAlert.ImminentMoveAlertStructure));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, imas);
                    }
                    retunXML = textWriter.ToString(); //This is the output as a string
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            model.ReturnXML = retunXML;
            model.NotificationID = 0;
            return model;
        }
        */
        #endregion

        #region GenerateFormVR1XML
        public XMLModel GenerateFormVR1XML(string haulierMnemonic, string esdalRefNumber, int Version_No, string userSchema = UserSchema.Sort)
        {
            string retunXML = string.Empty;
            XMLModel model = new XMLModel();
            VR1Structure formVr1Stru = new VR1Structure();

            formVr1Stru = FormVR1DAO.GetFormVR1Details(haulierMnemonic, esdalRefNumber, Version_No, userSchema);

            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(VR1Structure));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, formVr1Stru);
                    }
                    retunXML = textWriter.ToString(); //This is the output as a string
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            model.ReturnXML = retunXML;
            model.NotificationID = 0;
            return model;
        }
        #endregion

        #region GetProposalNotesforHaulier
        public string GetProposalNotesforHaulier(string orderNo, string esDAlRefNo, string schema)
        {
            string returnXML = string.Empty;

            byte[] byteXML = RevisedAgreementDAO.GetNotesForHaulier(orderNo, esDAlRefNo, schema);

            if (byteXML != null)
            {
                string outboundXMLInformation = string.Empty;

                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    outboundXMLInformation = Encoding.UTF8.GetString(byteXML, 0, byteXML.Length);

                    xmlDoc.LoadXml(outboundXMLInformation);
                }
                catch (System.Xml.XmlException XE)
                {
                    //Some data is stored in gzip format, so we need to unzip then load it.
                    byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(byteXML);

                    outboundXMLInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);

                    xmlDoc.LoadXml(outboundXMLInformation);
                }

                returnXML = xmlDoc.OuterXml;
            }

            return returnXML;
        }
        #endregion

        #region GetRevisedNotesforHaulier
        public string GetRevisedNotesforHaulier(long VersionID, string userSchema = UserSchema.Sort)
        {
            string returnXML = string.Empty;

            byte[] byteXML = RevisedAgreementDAO.GetNotesForHaulier(VersionID, userSchema);

            if (byteXML != null)
            {
                string outboundXMLInformation = string.Empty;

                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    outboundXMLInformation = Encoding.UTF8.GetString(byteXML, 0, byteXML.Length);

                    xmlDoc.LoadXml(outboundXMLInformation);
                }
                catch (System.Xml.XmlException XE)
                {
                    //Some data is stored in gzip format, so we need to unzip then load it.
                    byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(byteXML);

                    outboundXMLInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);

                    xmlDoc.LoadXml(outboundXMLInformation);
                }

                returnXML = xmlDoc.OuterXml;
            }

            return returnXML;
        }
        #endregion

        #region GetReProposalNotesforHaulier
        public string GetReProposalNotesforHaulier(int ProjectID, int versionNo, string userSchema)
        {
            string returnXML = string.Empty;

            byte[] byteXML = ReProposalStillAffectedDAO.GetNotesForHaulier(ProjectID, versionNo, userSchema);

            if (byteXML != null)
            {
                string outboundXMLInformation = string.Empty;

                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    outboundXMLInformation = Encoding.UTF8.GetString(byteXML, 0, byteXML.Length);

                    xmlDoc.LoadXml(outboundXMLInformation);
                }
                catch (System.Xml.XmlException XE)
                {
                    //Some data is stored in gzip format, so we need to unzip then load it.
                    byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(byteXML);

                    outboundXMLInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);

                    xmlDoc.LoadXml(outboundXMLInformation);
                }

                returnXML = xmlDoc.OuterXml;
            }

            return returnXML;
        }
        #endregion

        #region GetRouteDescriptionInformation
        public string GetRouteDescriptionInformation(string orderNo, string esDAlRefNo, string userSchema = UserSchema.Sort)
        {
            string returnXML = string.Empty;

            byte[] byteXML = RevisedAgreementDAO.GetRouteDescription(orderNo, esDAlRefNo, userSchema);

            if (byteXML != null)
            {
                string outboundXMLInformation = string.Empty;

                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    outboundXMLInformation = Encoding.UTF8.GetString(byteXML, 0, byteXML.Length);

                    xmlDoc.LoadXml(outboundXMLInformation);
                }
                catch (System.Xml.XmlException XE)
                {
                    //Some data is stored in gzip format, so we need to unzip then load it.
                    byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(byteXML);

                    outboundXMLInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);

                    xmlDoc.LoadXml(outboundXMLInformation);
                }

                returnXML = xmlDoc.OuterXml;
            }

            return returnXML;
        }
        #endregion

        #region GenerateDiffGram
        public string GenerateDiffGram(string OldFilePath, string NewFilePath, string DifferenceFilePath, string MergeFilePath)
        {
            XmlWriter diffGramWriter = XmlWriter.Create(DifferenceFilePath);
            XmlDiff xmldiff = new XmlDiff();
            string returnXML = string.Empty;
            try
            {
                bool bIdentical = xmldiff.Compare(OldFilePath, NewFilePath, true, diffGramWriter);
                diffGramWriter.Close();

                if (!bIdentical)
                {
                    returnXML = PatchUp(OldFilePath, DifferenceFilePath, MergeFilePath);
                }
            }
            catch (Exception ex)
            { throw ex; }
            return returnXML;
        }
        #endregion

        #region PatchUp
        public string PatchUp(string OldFile, String DifferenceFile, String MergeFile)
        {
            XmlDocument sourceDoc = new XmlDocument(new NameTable());

            try
            {
                sourceDoc.Load(OldFile);
                XmlTextReader diffgramReader = new XmlTextReader(DifferenceFile);

                XmlPatchNew xmlpatch = new XmlPatchNew();
                xmlpatch.Patch(sourceDoc, diffgramReader);

                XmlTextWriter output = new XmlTextWriter(MergeFile, Encoding.Unicode);
                sourceDoc.Save(output);
                output.Close();
                diffgramReader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return sourceDoc.InnerXml;
        }
        #endregion
    }
}