using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using STP.Common.Constants;
using STP.Common.General;
using STP.RouteAssessment.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using STP.Domain.RouteAssessment.XmlAffectedParties;
using STP.Domain.RouteAssessment.XmlAnalysedCautions;
using STP.Domain.RouteAssessment.XmlAnalysedRoads;
using STP.Domain.RouteAssessment.XmlAnalysedStructures;
using STP.Domain.RouteAssessment.XmlConstraints;
using STP.Domain.RouteAssessment;
using STP.Domain.MovementsAndNotifications.Notification;

namespace STP.RouteAssessment.RouteAssessment
{
    #region StringExtractor Class
    /// <summary>
    /// The class encorporates function's to perform various xml string or xml document operations
    /// </summary>
    public class StringExtractor
    {
        #region  xmlStringExtractor(string xmlString, string nameSpace)
        /// <summary>
        /// Function to extract caution and constraint from xml input,
        /// the function returns the InnerText in the xml string obtained from Annotation and Cautions table fields
        /// The sample XML files from both the tables are commented below part of this Class library for reference
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static string xmlStringExtractor(string xmlString, string nameSpace)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(xmlString);

                var nsManager = new XmlNamespaceManager(xmlDoc.NameTable);

                switch (nameSpace)
                {

                    case "annotation":
                        nsManager.AddNamespace("annotation", xmlDoc.DocumentElement.NamespaceURI);
                        XmlNode anotNode = xmlDoc.DocumentElement.SelectSingleNode("/annotation:Text", nsManager);
                        return xmlDoc.InnerText;
                        break;
                    case "SpecificAction":
                        nsManager.AddNamespace("ns", xmlDoc.DocumentElement.NamespaceURI);
                        XmlNode specificAction = xmlDoc.DocumentElement.SelectSingleNode("/ns:SpecificAction", nsManager);
                        return specificAction.InnerText;
                        break;
                    case "route":
                        nsManager.AddNamespace("route", xmlDoc.DocumentElement.NamespaceURI);

                        XmlNode routeNode = xmlDoc.DocumentElement.SelectSingleNode("/route:Text", nsManager);

                        return xmlDoc.InnerText;
                        break;
                    default:
                        return null;
                        break;
                };
            }
            catch (Exception e)
            {
                return null;
            }
        }
        #endregion

        #region XmlDeserializer function
        /// <summary>
        /// Function to deserialize the Analysed constraint Xml Data
        /// </summary>
        public static void xmlDeserializer(string xml)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(AnalysedConstraints));

                StringReader stringReader = new StringReader(xml);
                XmlTextReader xmlReader = new XmlTextReader(stringReader);

                object obj = deserializer.Deserialize(xmlReader);

                AnalysedConstraints XmlData = (AnalysedConstraints)obj;

                xmlReader.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region XmlSerializer function
        /// <summary>
        ///Function to Serialize the Analysed constraint object to constraint xml
        /// </summary>
        public static string xmlSerializer(AnalysedConstraints XmlData)
        {
            try
            {

                XmlSerializer serializer = new XmlSerializer(typeof(AnalysedConstraints));

                StringWriter outStream = new Utf8StringWriter();    // The code generates UTF-8 encoded xml string 
                serializer.Serialize(outStream, XmlData);
                string str = outStream.ToString();

                return outStream.ToString(); // Output string
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region XmlDeserializerStructures function
        /// <summary>
        /// Function to deserialize the Analysed structure's Xml Data 
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static AnalysedStructures XmlDeserializerStructures(string xmlString)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AnalysedStructures));

                StringReader read = new StringReader(xmlString);

                using (XmlReader xmlReader = new XmlTextReader(read))
                {
                    Object obj = serializer.Deserialize(xmlReader);

                    AnalysedStructures structures = (AnalysedStructures)obj;

                    xmlReader.Close();

                    return structures;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion

        #region xmlStructureSerializer function
        /// <summary>
        /// Function to Serialize the Analysed structure Xml data
        /// </summary>
        /// <param name="XmlData"></param>
        /// <returns></returns>
        public static string xmlStructureSerializer(AnalysedStructures XmlData)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AnalysedStructures));

                StringWriter outStream = new Utf8StringWriter();    // The code generates UTF-8 encoded xml string 
                serializer.Serialize(outStream, XmlData);
                string str = outStream.ToString();

                return outStream.ToString(); // Output string 
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region XmlCautionSerializer function
        /// <summary>
        ///Function to Serialize the Analysed caution's Xml data
        /// </summary>
        public static string XmlCautionSerializer(AnalysedCautions XmlData)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AnalysedCautions));

                StringWriter outStream = new Utf8StringWriter();    // The code generates UTF-8 encoded xml string 
                serializer.Serialize(outStream, XmlData);
                string str = outStream.ToString();

                return outStream.ToString(); // Output string 
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region XmlDeserializeCautions function
        /// <summary>
        /// Function to deserialize the Analysed caution's Xml string
        /// </summary>
        public static AnalysedCautions XmlDeserializeCautions(string xmlString)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AnalysedCautions));

                StringReader read = new StringReader(xmlString);

                using (XmlReader xmlReader = new XmlTextReader(read))
                {
                    Object obj = serializer.Deserialize(xmlReader);

                    AnalysedCautions cautions = (AnalysedCautions)obj;

                    xmlReader.Close();

                    return cautions;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Utf8StringWriter sub class to overide UTF-16 encoding to UTF-8 encoding
        /// <summary>
        /// class to generate UTF8 encoded xml
        /// </summary>
        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }
        }
        #endregion

        #region  constraintListToXml(List<RouteConstraints> constrList)
        /// <summary>
        /// Function to convert constraint list to Xml Object and use serializer class to generate the string
        /// </summary>
        /// <param name="constrList"></param>
        public static string constraintListToXml(List<RouteConstraints> constrList)
        {
            int i = 0;
            AnalysedConstraints constr = new AnalysedConstraints();

            constr.AnalysedConstraintsPart = new List<AnalysedConstraintsPart>();

            AnalysedConstraintsPart analConstrPart = null;

            Constraint analConstr = null;

            foreach (RouteConstraints constrObj in constrList)
            {
                try
                {
                    //constr.AnalysedConstraintsPart = new List<AnalysedConstraintsPart>();
                    analConstrPart = new AnalysedConstraintsPart();
                    analConstrPart.Id = 546; // this should be the route-part-id 
                    analConstr = new Constraint();
                    //This should be the route name and not constraint name kept for test purpose
                    analConstrPart.AnalysedConstraintsPartName = constrObj.ConstraintName; // Should be replaced with constraint Part Name (route_part Name )

                    analConstr.ECRN = constrObj.ConstraintCode;
                    analConstr.ConstraintType = constrObj.ConstraintType;
                    analConstr.IsApplicable = true;
                    analConstr.ConstraintName = constrObj.ConstraintName;
                    analConstrPart.Constraint = new List<Constraint>();
                    analConstrPart.Constraint.Add(analConstr);

                    constr.AnalysedConstraintsPart.Add(analConstrPart);
                    i++;
                }
                catch (Exception ex)
                {
                    STP.Common.Logger.Logger.GetInstance().LogMessage(Common.Logger.Log_Priority.FATAL_ERROR, "Exception Occured : {0}", ex.StackTrace);
                    throw ex;
                }
            }
            string constraintXmlString = xmlSerializer(constr);

            return constraintXmlString;
        }
        #endregion

        #region constraintListToXml(List<Constraints> constrList, int routePartId, string routeName)
        /// <summary>
        /// Function to convert constraint list to Xml Object and use serializer class to generate the string
        /// </summary>
        /// <param name="constrList"></param>
        public static string constraintListToXml(List<RouteConstraints> constrList, int routePartId, string routeName)
        {
            int i = 0;
            AnalysedConstraints constr = new AnalysedConstraints();

            constr.AnalysedConstraintsPart = new List<AnalysedConstraintsPart>();

            AnalysedConstraintsPart analConstrPart = null;

            Constraint analConstr = null;

            analConstrPart = new AnalysedConstraintsPart();
            analConstrPart.Id = routePartId;
            analConstrPart.AnalysedConstraintsPartName = routeName; // constraint route_part Name 
            analConstrPart.Constraint = new List<Constraint>();

            foreach (RouteConstraints constrObj in constrList)
            {
                try
                {
                    analConstr = new Constraint();
                    analConstr.ECRN = constrObj.ConstraintCode;
                    analConstr.ConstraintType = constrObj.ConstraintType;
                    analConstr.IsApplicable = true;
                    analConstr.ConstraintName = constrObj.ConstraintName;
                    analConstrPart.Constraint.Add(analConstr);

                    i++;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            constr.AnalysedConstraintsPart.Add(analConstrPart);

            string constraintXmlString = xmlSerializer(constr);

            return constraintXmlString;
        }
        #endregion

        #region constraintListToXml(AnalysedConstraintsPart analConstrPart, List<RouteConstraints> constrList, int routePartId, string routeName)
        /// <summary>
        /// Function to retrieve xml from constraint List object for multiple route-part for a given analysis Id
        /// </summary>
        /// <param name="analConstrPart"></param>
        /// <param name="constrList"></param>
        /// <param name="routePartId"></param>
        /// <param name="routeName"></param>
        /// <returns></returns>
        public static AnalysedConstraintsPart constraintListToXml(AnalysedConstraintsPart analConstrPart, List<RouteConstraints> constrList, int routePartId, string routeName)
        {
            int i = 0;

            Constraint analConstr = null;

            analConstrPart = new AnalysedConstraintsPart();

            analConstrPart.Id = routePartId;

            analConstrPart.AnalysedConstraintsPartName = routeName; // constraint route_part Name 

            analConstrPart.Constraint = new List<Constraint>();

            foreach (RouteConstraints constrObj in constrList)
            {
                try
                {
                    analConstr = new Constraint();

                    analConstr.ECRN = constrObj.ConstraintCode;

                    analConstr.ConstraintType = constrObj.ConstraintType;

                    analConstr.IsApplicable = true; //need to differ dynamically

                    analConstr.ConstraintName = constrObj.ConstraintName;

                    //constraint  Suitability
                    analConstr.Appraisal = new Domain.RouteAssessment.XmlConstraints.Appraisal();

                    analConstr.Appraisal.Suitability = new Suitability();

                    analConstr.Appraisal.Suitability.Value = constrObj.ConstraintSuitability;

                    analConstrPart.Constraint.Add(analConstr);

                    i++;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return analConstrPart;
        }
        #endregion


        #region constraintDeserializer(string constraintXmlString)
        /// <summary>
        /// function to deserialize constraint xml string
        /// </summary>
        /// <param name="constraintXmlString"></param>
        /// <returns></returns>
        public static AnalysedConstraints constraintDeserializer(string constraintXmlString)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(AnalysedConstraints));

                StringReader stringReader = new StringReader(constraintXmlString);

                XmlTextReader xmlReader = new XmlTextReader(stringReader);

                object obj = deserializer.Deserialize(xmlReader);

                AnalysedConstraints XmlData = (AnalysedConstraints)obj;

                return XmlData;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region xmlAffectedPartyDeserializer(string xml)
        /// <summary>
        /// Function to deserialize the Affected Parties xml Data
        /// </summary>
        public static AffectedPartiesStructure xmlAffectedPartyDeserializer(string xml)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(AffectedPartiesStructure));

                StringReader stringReader = new StringReader(xml);

                XmlTextReader xmlReader = new XmlTextReader(stringReader);

                object obj = deserializer.Deserialize(xmlReader);

                AffectedPartiesStructure XmlData = (AffectedPartiesStructure)obj;

                xmlReader.Close();

                return XmlData;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region xmlAffectedPartyDeleteFromXml(string xml, string orgName, string fullName)
        /// <summary>
        /// Function to deserialize the Affected Parties xml Data for deleting the orgName and full name from the manual affected party list
        /// </summary>
        public static string xmlAffectedPartyDeleteFromXml(string xml, string orgName, string fullName)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(AffectedPartiesStructure));

            string tmpFullName = null, tmpOrgName = null;


            StringReader stringReader = new StringReader(xml);
            XmlTextReader xmlReader = new XmlTextReader(stringReader);

            object obj = deserializer.Deserialize(xmlReader);

            AffectedPartiesStructure XmlData = (AffectedPartiesStructure)obj;

            int i = 0;

            int listCnt = XmlData.ManualAffectedParties.Count();

            foreach (AffectedPartyStructure manualAffectedParty in XmlData.ManualAffectedParties.ToList())
            {
                try
                {
                    tmpFullName = manualAffectedParty.Contact.Contact.adhocContactRef.FullName;

                    tmpOrgName = manualAffectedParty.Contact.Contact.adhocContactRef.OrganisationName;
                }
                catch (Exception ex)
                {
                    tmpFullName = manualAffectedParty.Contact.Contact.simpleContactRef.FullName;

                    tmpOrgName = manualAffectedParty.Contact.Contact.simpleContactRef.OrganisationName;
                }


                if (tmpFullName == fullName && tmpOrgName == orgName)
                {

                    XmlData.ManualAffectedParties.Remove(manualAffectedParty);
                    //XmlData.ManualAffectedParties.RemoveAt(i);
                }
                else if (fullName == "" && orgName == "")
                {
                    XmlData.ManualAffectedParties.Remove(manualAffectedParty);
                }

                i++;
            }

            string newXml = xmlAffectedPartySerializer(XmlData);

            xmlReader.Close();

            return newXml;
        }
        #endregion

        #region xmlAffectedPartyDeserializer(string xml, List<AssessmentContacts> manualContactList)
        /// <summary>
        /// Function to deserialize the Affected Parties xml Data
        /// </summary>
        public static string XmlAffectedPartyDeserializer(string xml, List<AssessmentContacts> manualContactList)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(AffectedPartiesStructure));

            int count = 0;

            StringReader stringReader = new StringReader(xml);
            XmlTextReader xmlReader = new XmlTextReader(stringReader);
            AffectedPartiesStructure XmlData = null;
            try
            {
                object obj = deserializer.Deserialize(xmlReader);
                XmlData = (AffectedPartiesStructure)obj;
            }
            catch
            {
                if (xml == "")
                {
                    XmlData = new AffectedPartiesStructure();
                    XmlData.GeneratedAffectedParties = new List<AffectedPartyStructure>();
                }
            }

            if (XmlData.ManualAffectedParties == null)
            {
                XmlData.ManualAffectedParties = new List<AffectedPartyStructure>();
            }

            //List<AffectedPartyStructure> manualAffectedPartyList = null;

            foreach (AssessmentContacts manualContact in manualContactList)
            {

                AffectedPartyStructure manualAffectedParty = new AffectedPartyStructure();

                manualAffectedParty.Exclude = false;

                manualAffectedParty.ExclusionOutcome = AffectedPartyReasonExclusionOutcomeType.newlyaffected;

                manualAffectedParty.ExclusionOutcomeSpecified = true;

                manualAffectedParty.IsPolice = manualContact.OrganisationType == "police" ? true : false;

                manualAffectedParty.IsRetainedNotificationOnly = false;

                manualAffectedParty.Reason = AffectedPartyReasonType.newlyaffected;

                manualAffectedParty.ReasonSpecified = true;

                manualAffectedParty.Contact = new Domain.RouteAssessment.XmlAffectedParties.ContactReferenceStructure();

                manualAffectedParty.Contact.Contact = new Domain.RouteAssessment.XmlAffectedParties.ContactReferenceChoiceStructure();

                manualAffectedParty.Contact.Contact.adhocContactRef = new Domain.RouteAssessment.XmlAffectedParties.AdhocContactReferenceStructure();

                manualAffectedParty.Contact.Contact.adhocContactRef.FullName = manualContact.ContactName;

                manualAffectedParty.Contact.Contact.adhocContactRef.OrganisationName = manualContact.OrganisationName;

                manualAffectedParty.Contact.Contact.adhocContactRef.FaxNumber = manualContact.Fax;

                manualAffectedParty.Contact.Contact.adhocContactRef.EmailAddress = manualContact.Email;

                count = checkForDuplicateCont(manualAffectedParty, XmlData);
                if (count == 0)
                {
                    XmlData.ManualAffectedParties.Add(manualAffectedParty);
                }
            }

            if (count == 0)
            {
                string newXml = xmlAffectedPartySerializer(XmlData);
                xmlReader.Close();
                return newXml;
            }
            else if (count == 2)
            {
                string newXml = xmlAffectedPartySerializer(XmlData);
                xmlReader.Close();
                return newXml;
            }
            else
            {
                return "Contact already exist";
            }
        }
        #endregion

        #region checkForDuplicateCont(AffectedPartyStructure manualAffectedParty, AffectedPartiesStructure XmlData)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manualAffectedParty"></param>
        /// <param name="XmlData"></param>
        /// <returns></returns>
        private static int checkForDuplicateCont(AffectedPartyStructure manualAffectedParty, AffectedPartiesStructure XmlData)
        {
            int count = 0;

            string tmpNewFullName = manualAffectedParty.Contact.Contact.adhocContactRef.FullName;

            string tmpNewOrgName = manualAffectedParty.Contact.Contact.adhocContactRef.OrganisationName;
            string tmpNewEmail = manualAffectedParty.Contact.Contact.adhocContactRef.EmailAddress;

            string tmpFullName = null, tmpOrgName = null, tmpEmail = null;

            foreach (AffectedPartyStructure adhocContact in XmlData.ManualAffectedParties.ToList())
            {
                try
                {
                    tmpFullName = adhocContact.Contact.Contact.adhocContactRef.FullName;

                    tmpOrgName = adhocContact.Contact.Contact.adhocContactRef.OrganisationName;

                    tmpEmail = adhocContact.Contact.Contact.adhocContactRef.EmailAddress;

                }
                catch
                {
                    tmpFullName = adhocContact.Contact.Contact.simpleContactRef.FullName;

                    tmpFullName = adhocContact.Contact.Contact.simpleContactRef.FullName;
                }
                if (tmpFullName == tmpNewFullName && tmpOrgName == tmpNewOrgName)
                {
                    count++;
                    if (tmpEmail != tmpNewEmail)
                    {
                        count++;

                        adhocContact.Contact.Contact.adhocContactRef.EmailAddress = tmpNewEmail;
                    }
                }
            }

            return count;
        }
        #endregion

        #region  xmlAffectedPartySerializer(AffectedPartiesStructure XmlData)
        /// <summary>
        ///Function to Serialize the Analysed constraint object to constraint xml
        /// </summary>
        public static string xmlAffectedPartySerializer(AffectedPartiesStructure XmlData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AffectedPartiesStructure));

            StringWriter outStream = new Utf8StringWriter();    // The code generates UTF-8 encoded xml string 
            serializer.Serialize(outStream, XmlData);
            string str = outStream.ToString();

            return outStream.ToString(); // Output string 
        }
        #endregion

        #region ZipAndBlob
        /// <summary>
        /// function to zip the constraint
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ZipAndBlob(string value)
        {
            //Converting string to byte array
            byte[] XMLPage = System.Text.Encoding.UTF8.GetBytes(value);
            string result = string.Empty;

            string ErrMsg = "No Data Found";

            try
            {
                //compressing the byte array
                MemoryStream ms = new MemoryStream();
                Stream zipStream = new GZipStream(ms, CompressionMode.Compress);

                zipStream.Write(XMLPage, 0, XMLPage.Length);
                zipStream.Close();

                byte[] compressedData = (byte[])ms.ToArray();

                //returing the compressed data
                return compressedData;

            }
            catch (Exception ex)
            {
                ErrMsg = ex.StackTrace;
            }
            return null;
        }
        #endregion

        #region Notification
        #region xmlInboundNotifSerializer(InboundNotification XmlData)
        /// <summary>
        ///Function to Serialize the Analysed constraint object to constraint xml
        /// </summary>
        public static string xmlInboundNotifSerializer(InboundNotification XmlData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(InboundNotification));

            StringWriter outStream = new Utf8StringWriter();    // The code generates UTF-8 encoded xml string 
            serializer.Serialize(outStream, XmlData);
            string str = outStream.ToString();

            return outStream.ToString(); // Output string 
        }
        #endregion

        #region xmlInboundNotifDeserializer(string xml)
        /// <summary>
        /// Function to deserialize the Affected Parties xml Data
        /// </summary>
        public static InboundNotification xmlInboundNotifDeserializer(string xml)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(InboundNotification));

            StringReader stringReader = new StringReader(xml);
            XmlTextReader xmlReader = new XmlTextReader(stringReader);
            object obj = deserializer.Deserialize(xmlReader);

            InboundNotification XmlData = (InboundNotification)obj;

            xmlReader.Close();

            return XmlData;
        }
        #endregion
        #endregion

        #region checkAffectedPartiesDiff(AffectedPartiesStructure oldAffectedParties, AffectedPartiesStructure newAffectedParties,bool changeOfRoute = false)
        /// <summary>
        /// function to generated the serializable object for affected structure xml from provided input's.
        /// </summary>
        /// <param name="oldAffectedParties"></param>
        /// <param name="newAffectedParties"></param>
        /// <param name="changeOfRoute"></param>
        /// <returns></returns>
        public static AffectedPartiesStructure checkAffectedPartiesDiff(AffectedPartiesStructure oldAffectedParties, AffectedPartiesStructure newAffectedParties, bool changeOfRoute = false)
        {

            #region
            List<AffectedPartyStructure> newList = newAffectedParties.GeneratedAffectedParties;
            List<AffectedPartyStructure> oldList = oldAffectedParties.GeneratedAffectedParties;
            List<AffectedPartyStructure> updatedNewList = null;
            List<AffectedPartyStructure> updatedOldList = null;
            #endregion
            //Full Name, and corresponding object
            Dictionary<int, List<AffectedPartyStructure>> oldAffectedPartDict = new Dictionary<int, List<AffectedPartyStructure>>();
            Dictionary<int, List<AffectedPartyStructure>> newAffectedPartDict = new Dictionary<int, List<AffectedPartyStructure>>();

            int OrgId;

            List<int> oldOrgIdList = null;
            List<int> newOrgIdList = null;
            //generating current version of affected party list 

            foreach (AffectedPartyStructure affNewObj in newList)
            {
                OrgId = affNewObj.Contact.Contact.simpleContactRef.OrganisationId;
                try
                {
                    //new list is created
                    updatedNewList = new List<AffectedPartyStructure>();
                    //object added to the list object
                    updatedNewList.Add(affNewObj);
                    //dictionary is updated
                    newAffectedPartDict.Add(OrgId, updatedNewList);

                }
                catch (Exception e)
                {
                    //creating the list instance again
                    updatedNewList = AffectedPartyUpdatedList(affNewObj, newAffectedPartDict[OrgId]);

                    //updating the dictionary with the new list
                    newAffectedPartDict[OrgId] = updatedNewList;
                }
            }

            //generating previous version of affected party list

            foreach (AffectedPartyStructure affOldObj in oldList)
            {
                OrgId = affOldObj.Contact.Contact.simpleContactRef.OrganisationId;

                try
                {

                    updatedOldList = new List<AffectedPartyStructure>();

                    updatedOldList.Add(affOldObj);

                    oldAffectedPartDict.Add(OrgId, updatedOldList);

                }
                catch (Exception e)
                {

                    updatedOldList = AffectedPartyUpdatedList(affOldObj, oldAffectedPartDict[OrgId]);

                    //updating the dictionary with the new list
                    oldAffectedPartDict[OrgId] = updatedOldList;

                }

            }
            //
            oldOrgIdList = oldAffectedPartDict.Keys.ToList();
            //
            newOrgIdList = newAffectedPartDict.Keys.ToList();

            //new object list of the affected party structure. is created
            updatedNewList = new List<AffectedPartyStructure>();

            //checking for the affected contact name in the newly generated affected parties object list
            //if present the reason and exclusion outcome is updated 
            foreach (int orgId in oldOrgIdList)
            {
                try
                {
                    //getting the AffectedPartyStructure object from the dictionary value for a given key
                    //here key is the full name of the contact/
                    if (newAffectedPartDict.ContainsKey(orgId))
                    {
                        try
                        {
                            foreach (AffectedPartyStructure newAffectListObj in newAffectedPartDict[orgId])
                            {
                                int tmpOrgId = newAffectListObj.Contact.Contact.simpleContactRef.OrganisationId;

                                if (oldAffectedParties.GeneratedAffectedParties.Exists(x => x.Contact.Contact.simpleContactRef.OrganisationId == tmpOrgId))
                                {
                                    newAffectListObj.Reason = AffectedPartyReasonType.stillaffected;

                                    newAffectListObj.ReasonSpecified = true;

                                    newAffectListObj.ExclusionOutcome = AffectedPartyReasonExclusionOutcomeType.stillaffected;

                                    newAffectListObj.ExclusionOutcomeSpecified = true;

                                    updatedNewList.Add(newAffectListObj);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                foreach (AffectedPartyStructure newAffectListObj in newAffectedPartDict[orgId])
                                {
                                    newAffectListObj.Reason = AffectedPartyReasonType.stillaffected;

                                    newAffectListObj.ReasonSpecified = true;

                                    newAffectListObj.ExclusionOutcome = AffectedPartyReasonExclusionOutcomeType.stillaffected;

                                    newAffectListObj.ExclusionOutcomeSpecified = true;

                                    updatedNewList.Add(newAffectListObj);
                                }
                            }
                            catch (Exception e)
                            {
                                continue;
                            }
                        }
                    }
                    else if (oldAffectedPartDict.ContainsKey(orgId))
                    {

                        if (newAffectedParties.GeneratedAffectedParties.Exists(x => x.OnBehalfOf != null && x.OnBehalfOf.DelegatorsOrganisationId == orgId && x.OnBehalfOf.RetainNotification == true))
                        {//if the organisation exist in new list due to retainment.
                            foreach (AffectedPartyStructure affectedByRetainment in oldAffectedPartDict[orgId])
                            {
                                int tmpOrgId = affectedByRetainment.Contact.Contact.simpleContactRef.OrganisationId;

                                if (oldAffectedParties.GeneratedAffectedParties.Exists(x => x.Contact.Contact.simpleContactRef.OrganisationId == tmpOrgId))
                                {
                                    affectedByRetainment.Reason = AffectedPartyReasonType.affectedbychangeofroute;

                                    affectedByRetainment.ReasonSpecified = true;

                                    affectedByRetainment.ExclusionOutcome = AffectedPartyReasonExclusionOutcomeType.affectedbychangeofroute;

                                    affectedByRetainment.ExclusionOutcomeSpecified = true;

                                    updatedNewList.Add(affectedByRetainment);
                                }
                            }
                        }
                        else if (newAffectedParties.GeneratedAffectedParties.Exists(x => x.OnBehalfOf != null && x.OnBehalfOf.DelegatorsOrganisationId == orgId && x.OnBehalfOf.RetainNotification == false))
                        {
                            //ignore the organisation id to be listed as an affected party as it has been delegated but has not retained by the owner
                        }
                        else
                        {
                            //Exception occurs when the old affected party is not present in newly generated list this in turn is used to check whether the name is present in old list of affected parties
                            //if the list is present the object is extracted and updated with no longer affected reason and exclution type and added to the newly generated updated object list.
                            try
                            {
                                foreach (AffectedPartyStructure noLongerAffectdObj in oldAffectedPartDict[orgId])
                                {
                                    int tmpOrgId = noLongerAffectdObj.Contact.Contact.simpleContactRef.OrganisationId;

                                    if (oldAffectedParties.GeneratedAffectedParties.Exists(x => x.Contact.Contact.simpleContactRef.OrganisationId == tmpOrgId))
                                    {

                                        noLongerAffectdObj.Reason = AffectedPartyReasonType.nolongeraffected;

                                        noLongerAffectdObj.ReasonSpecified = true;

                                        noLongerAffectdObj.ExclusionOutcome = AffectedPartyReasonExclusionOutcomeType.nolongeraffected;

                                        noLongerAffectdObj.ExclusionOutcomeSpecified = true;

                                        updatedNewList.Add(noLongerAffectdObj);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    foreach (AffectedPartyStructure noLongerAffectdObj in oldAffectedPartDict[orgId])
                                    {
                                        noLongerAffectdObj.Reason = AffectedPartyReasonType.nolongeraffected;

                                        noLongerAffectdObj.ReasonSpecified = true;

                                        noLongerAffectdObj.ExclusionOutcome = AffectedPartyReasonExclusionOutcomeType.nolongeraffected;

                                        noLongerAffectdObj.ExclusionOutcomeSpecified = true;

                                        updatedNewList.Add(noLongerAffectdObj);
                                    }
                                }
                                catch (Exception e)
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    continue;
                }

            }

            //checking for the contact's from the newly generated contact list 
            foreach (int orgId in newOrgIdList)
            {
                try
                {
                    if (!oldAffectedPartDict.ContainsKey(orgId) && newAffectedPartDict.ContainsKey(orgId))
                    {
                        //checking whether old list of affected partie's have data for a contact from new list
                        //if the contact does not exist 
                        foreach (AffectedPartyStructure newAffectListObj in newAffectedPartDict[orgId])
                        {
                            newAffectListObj.Reason = AffectedPartyReasonType.affectedbychangeofroute;

                            newAffectListObj.ReasonSpecified = true;

                            newAffectListObj.ExclusionOutcome = AffectedPartyReasonExclusionOutcomeType.affectedbychangeofroute;

                            newAffectListObj.ExclusionOutcomeSpecified = true;

                            updatedNewList.Add(newAffectListObj);
                        }
                    }
                }
                catch (Exception e)
                {
                    continue;
                }
            }


            newAffectedParties.GeneratedAffectedParties = new List<AffectedPartyStructure>();

            newAffectedParties.GeneratedAffectedParties.AddRange(updatedNewList);

            return newAffectedParties;
        }
        #endregion

        #region AffectedPartyUpdatedList(AffectedPartyStructure affObj, List<AffectedPartyStructure> affectedPartyObjList)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="affObj"></param>
        /// <param name="affectedPartyObjList"></param>
        /// <returns></returns>
        public static List<AffectedPartyStructure> AffectedPartyUpdatedList(AffectedPartyStructure affObj, List<AffectedPartyStructure> affectedPartyObjList)
        {
            List<AffectedPartyStructure> updatedList = new List<AffectedPartyStructure>();

            foreach (AffectedPartyStructure afftdStruc in affectedPartyObjList)
            {
                if (afftdStruc.Contact.Contact.simpleContactRef.FullName.Length > affObj.Contact.Contact.simpleContactRef.FullName.Length)
                {
                    updatedList.Add(afftdStruc);
                }
                else
                {
                    //adding to the existing list
                    updatedList.Add(affObj);
                }
            }
            return updatedList;
        }
        #endregion

        #region AnalysedRoadsDeserializer(string xmlString)
        /// <summary>
        /// Function to deserialize the Analysed structure's Xml Data 
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static AnalysedRoadsRoute AnalysedRoadsDeserializer(string xmlString)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(AnalysedRoadsRoute));

            StringReader stringReader = new StringReader(xmlString);


            XmlTextReader xmlReader = new XmlTextReader(stringReader);

            object obj = deserializer.Deserialize(xmlReader);

            AnalysedRoadsRoute XmlData = (AnalysedRoadsRoute)obj;

            //AnalysedRoadsSerializer(XmlData);

            return XmlData;

        }
        #endregion

        #region AnalysedRoadsSerializer function
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static string AnalysedRoadsSerializer(AnalysedRoadsRoute XmlData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AnalysedRoadsRoute));

            StringWriter outStream = new Utf8StringWriter();    // The code generates UTF-8 encoded xml string 
            serializer.Serialize(outStream, XmlData);
            string str = outStream.ToString();

            return outStream.ToString(); // Output string 
        }
        #endregion

        #region public static string xmlAffectedPartyDeserializer(string xml, int contactId)
        public static string xmlAffectedPartyExcludeDeserializer(string xml, int contactId, int organisationId, string organisationName)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(AffectedPartiesStructure));

            StringReader stringReader = new StringReader(xml);
            XmlTextReader xmlReader = new XmlTextReader(stringReader);

            object obj = deserializer.Deserialize(xmlReader);

            AffectedPartiesStructure XmlData = (AffectedPartiesStructure)obj;

            if (XmlData.ManualAffectedParties == null)
            {
                XmlData.ManualAffectedParties = new List<AffectedPartyStructure>();
            }


            var generatedData = XmlData.GeneratedAffectedParties;

            (from s in generatedData
             where (s.Contact.Contact.simpleContactRef.ContactId == contactId && s.Contact.Contact.simpleContactRef.OrganisationId == organisationId && s.Contact.Contact.simpleContactRef.OrganisationName == organisationName)
             select s).ToList().ForEach(s => s.Exclude = true);

            XmlData.GeneratedAffectedParties = generatedData;

            string newXml = xmlAffectedPartySerializer(XmlData);
            xmlReader.Close();
            return newXml;
        }
        #endregion

        #region public static string xmlAffectedPartyIncludeDeserializer(string xml, int contactId)
        public static string xmlAffectedPartyIncludeDeserializer(string xml, int contactId, int organisationId, string organisationName)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(AffectedPartiesStructure));

            StringReader stringReader = new StringReader(xml);
            XmlTextReader xmlReader = new XmlTextReader(stringReader);

            object obj = deserializer.Deserialize(xmlReader);

            AffectedPartiesStructure XmlData = (AffectedPartiesStructure)obj;

            if (XmlData.ManualAffectedParties == null)
            {
                XmlData.ManualAffectedParties = new List<AffectedPartyStructure>();
            }


            var generatedData = XmlData.GeneratedAffectedParties;

            (from s in generatedData
             where s.Contact.Contact.simpleContactRef.ContactId == contactId && s.Contact.Contact.simpleContactRef.OrganisationId == organisationId && s.Contact.Contact.simpleContactRef.OrganisationName == organisationName
             select s).ToList().ForEach(s => s.Exclude = false);

            XmlData.GeneratedAffectedParties = generatedData;

            string newXml = xmlAffectedPartySerializer(XmlData);
            xmlReader.Close();
            return newXml;
        }
        #endregion

        #region xmlAffectedPartyIncludeDispensation(AffectedPartiesStructure XMLObj, string orgName, string fullName, int orgId, InboundDispensation inbndNotif)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="XMLObj"></param>
        /// <param name="orgName"></param>
        /// <param name="fullName"></param>
        /// <param name="orgId"></param>
        /// <param name="inbndNotif"></param>
        /// <returns></returns>
        public static string xmlAffectedPartyIncludeDispensation(AffectedPartiesStructure XMLObj, string orgName, string fullName, int orgId, InboundDispensation inbndNotif)
        {
            string newXml = null;

            if (XMLObj.GeneratedAffectedParties.Exists(x => x.Contact.Contact.simpleContactRef.OrganisationId == orgId) && XMLObj.GeneratedAffectedParties.Exists(x => x.Contact.Contact.simpleContactRef.OrganisationName == orgName))
            {
                var generatedData = XMLObj.GeneratedAffectedParties;

                (from s in generatedData
                 where s.Contact.Contact.simpleContactRef.OrganisationId == orgId && s.Contact.Contact.simpleContactRef.OrganisationName == orgName
                 select s).ToList().ForEach(s => s.DispensationStatus = DispensationStatusType.inuse);

                XMLObj.GeneratedAffectedParties = generatedData;

                newXml = xmlAffectedPartySerializer(XMLObj);
            }

            return newXml;
        }
        #endregion

        #region AnnotationSerializer(XmlAnalysedAnnotations.AnalysedAnnotations annotationXml)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="annotationXml"></param>
        /// <returns></returns>
        public static string AnnotationSerializer(Domain.RouteAssessment.XmlAnalysedAnnotations.AnalysedAnnotations annotationXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Domain.RouteAssessment.XmlAnalysedAnnotations.AnalysedAnnotations));

            StringWriter outStream = new Utf8StringWriter();    // The code generates UTF-8 encoded xml string 
            serializer.Serialize(outStream, annotationXml);

            return outStream.ToString(); // Output string 
        }
        #endregion

        #region AnnotationDeserializer(string xmlString)
        /// <summary>
        /// function to deserialize annotation xml string
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static Domain.RouteAssessment.XmlAnalysedAnnotations.AnalysedAnnotations AnnotationDeserializer(string xmlString)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(Domain.RouteAssessment.XmlAnalysedAnnotations.AnalysedAnnotations));

            StringReader stringReader = new StringReader(xmlString);

            XmlTextReader xmlReader = new XmlTextReader(stringReader);

            object obj = deserializer.Deserialize(xmlReader);

            Domain.RouteAssessment.XmlAnalysedAnnotations.AnalysedAnnotations XmlData = (Domain.RouteAssessment.XmlAnalysedAnnotations.AnalysedAnnotations)obj;

            return XmlData;
        }
        #endregion

        #region public static byte[] GenerateInstrPDF(string outputString)
        public static byte[] GenerateInstrPDF(string outputString)//Generate PDF for print functionality
        {
            int pageCount = GetNoofPages(outputString);

            StringReader sr = new StringReader(outputString.Replace("###Noofpages###", pageCount.ToString()));

            byte[] content = null;
            string savedfile = string.Empty;



            using (MemoryStream myMemoryStream = new MemoryStream())
            {

                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 40f, 10f, 40f, 36);

                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

                StyleSheet style = new StyleSheet();
                htmlparser.SetStyleSheet(style);
                PdfWriter myPDFWriter = PdfWriter.GetInstance(pdfDoc, myMemoryStream);

                pdfDoc.Open();

                htmlparser.Parse(sr);

                HTMLWorker htmlparser1 = new HTMLWorker(pdfDoc);
                StyleSheet style1 = new StyleSheet();
                htmlparser1.SetStyleSheet(style1);

                pdfDoc.Close();

                content = AddPageNumbers(myMemoryStream.ToArray());
            }

            return content;
        }
        public static byte[] GeneratePrintablePDF(string outputString)
        {
            int pageCount = GetNoofPages(outputString);
            StringReader stringReader = new StringReader(outputString.Replace("###Noofpages###", pageCount.ToString()));
            byte[] content = null;
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 40f, 10f, 40f, 36);
                PdfWriter myPDFWriter = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(myPDFWriter, pdfDoc, stringReader);
                pdfDoc.Close();
                content = AddPageNumbers(myMemoryStream.ToArray());
            }

            return content;
        }

        #endregion
        #region public static int GetNoofPages(String sw)
        public static int GetNoofPages(String sw)
        {
            int pageCount = 0;

            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                try
                {
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 40f, 10f, 40f, 36);

                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

                    PdfWriter myPDFWriter = PdfWriter.GetInstance(pdfDoc, myMemoryStream);

                    pdfDoc.Open();

                    StringReader sr = new StringReader(sw);

                    htmlparser.Parse(sr);

                    sr.Close();
                    pdfDoc.Close();

                    MemoryStream ms = new MemoryStream();
                    ms.Write(myMemoryStream.ToArray(), 0, myMemoryStream.ToArray().Length);
                    PdfReader reader = new PdfReader(myMemoryStream.ToArray());   // we create a reader for a certain document
                    pageCount = reader.NumberOfPages;            // we retrieve the total number of pages
                    reader.Close();
                    return pageCount;
                }
                catch (Exception e)
                {
                    return pageCount;
                }
            }

        }
        #endregion
        #region public static byte[] AddPageNumbers(byte[] pdf)
        public static byte[] AddPageNumbers(byte[] pdf)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(pdf, 0, pdf.Length);

            PdfReader reader = new PdfReader(pdf);   // we create a reader for a certain document

            int n = reader.NumberOfPages;            // we retrieve the total number of pages

            Rectangle psize = reader.GetPageSize(1);  // we retrieve the size of the first page

            iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 40f, 10f, 40f, 36);    // step 1: creation of a document-object

            PdfWriter writer = PdfWriter.GetInstance(document, ms);      // step 2: we create a writer that listens to the document

            document.Open();     // step 3: we open the document

            PdfContentByte cb = writer.DirectContent;  // step 4: we add content

            int p = 0;
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                document.NewPage();
                p++;

                PdfImportedPage importedPage = writer.GetImportedPage(reader, page);
                cb.AddTemplate(importedPage, 0, 0, false);

                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb.BeginText();
                cb.SetFontAndSize(bf, 10);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Page " + p + " of " + n, 525, 15, 0);
                cb.EndText();
            }
            document.Close();      // step 5: we close the document

            return ms.ToArray();
        }
        #endregion

        #region checkForDuplicateGenAffectdParty(List<AffectedPartyStructure> generatedAfftdPartyList, AffectedPartyStructure generatedAfftdParty)
        /// <summary>
        /// checking duplicates in generated affected party list
        /// </summary>
        /// <param name="generatedAfftdPartyList"></param>
        /// <param name="generatedAfftdParty"></param>
        /// <returns></returns>
        internal static int checkForDuplicateGenAffectdParty(List<AffectedPartyStructure> generatedAfftdPartyList, AffectedPartyStructure generatedAfftdParty)
        {
            int count = 0;

            string tmpNewFullName = generatedAfftdParty.Contact.Contact.simpleContactRef.FullName;

            string tmpNewOrgName = generatedAfftdParty.Contact.Contact.simpleContactRef.OrganisationName;

            if (generatedAfftdPartyList.Exists(x => x.Contact.Contact.simpleContactRef.FullName == tmpNewFullName) && generatedAfftdPartyList.Exists(x => x.Contact.Contact.simpleContactRef.OrganisationName == tmpNewOrgName))
            {
                count++;
            }

            return count;
        }
        #endregion

        #region SortAffectedPartyBasedOnOrganisation(RouteAssessmentModel objRouteAssessmentModel, int analysisId, string userSchema)
        /// <summary>
        /// function to sort the affected parties based on organisation name
        /// </summary>
        /// <param name="objRouteAssessmentModel"></param>
        /// <param name="analysisId"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public static bool SortAffectedPartyBasedOnOrganisation(RouteAssessmentModel objRouteAssessmentModel, int analysisId, string userSchema)
        {
            try
            {
                //sample object 1
                Domain.RouteAssessment.XmlAffectedParties.AffectedPartiesStructure existingAfftdPartyObj = null;

                //fetching affected party list into a string 
                string existingAffectedParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.AffectedParties));

                //Deserializing it into object and storing it 
                existingAfftdPartyObj = StringExtractor.xmlAffectedPartyDeserializer(existingAffectedParties);

                //sorting the generated affected party list based on organisation name
                if (existingAfftdPartyObj.GeneratedAffectedParties.Count != 0 || existingAfftdPartyObj.GeneratedAffectedParties != null)
                {
                    var affectedPartylist = existingAfftdPartyObj.GeneratedAffectedParties;

                    existingAfftdPartyObj.GeneratedAffectedParties = affectedPartylist.OrderBy(t => t.Contact.Contact.simpleContactRef.OrganisationName).ToList();
                }

                string result = StringExtractor.xmlAffectedPartySerializer(existingAfftdPartyObj);

                objRouteAssessmentModel.AffectedParties = StringExtractor.ZipAndBlob(result);

                long tmpResult = RouteAssessmentProvider.Instance.updateRouteAssessment(objRouteAssessmentModel, analysisId, userSchema);

                if (tmpResult == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #endregion

        #region RetainManualAddedAndCompareForAllVersions(int analysisId,int DistributedMovAnalysisId, RouteAssessmentInputs inputsForAssessment, byte[] affectedParties, string userSchema)
        /// <summary>
        /// This function is for candidate route on sort side for retaining manual added affected parties and 
        /// checking no longer / still affected / newly affected affected parties from previously distributed movement.
        /// </summary>
        /// <param name="analysisId">Unique Id for existing route analysis which is to be updated by comparisons and merging.</param>
        /// <param name="DistributedMovAnalysisId">Previously distributed movements key for locating freezed analysis</param>
        /// <param name="inputsForAssessment">Inputs to carry out assessment for candidate route versions</param>
        /// <param name="affectedParties">existing affected parties that needs to be updated with changes.</param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public static bool RetainManualAddedAndCompareForAllVersions(int analysisId, int DistributedMovAnalysisId, RouteAssessmentInputs inputsForAssessment, byte[] affectedParties, string userSchema)
        {
            try
            {
                #region retaining manually added parties from previous version of candidate route  after route edit / modified /  recreated
                //the below condition and code portion generates affected parties in every case it retains the user added manually added parties in case there is any
                //object for affected party of previous candidate route version
                AffectedPartiesStructure prevCandidateAffectedParty = null;
                //object for current candidate route this is a global variable
                AffectedPartiesStructure currentCandidateAffectedParty = null;

                //fetching affected party list into a string 
                string existingAffectedParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(affectedParties));

                //Deserializing it into object and storing it 
                prevCandidateAffectedParty = StringExtractor.xmlAffectedPartyDeserializer(existingAffectedParties);

                //generating new list of affected parties 
                int updated = RouteAssessmentProvider.Instance.UpdateRouteAssessment(inputsForAssessment, 7, userSchema);//To update affected parties for candidate route

                #region Checking with already distributed movement
                //condition's to update the newly generated affected parties list with the previously generated one's for a movement version
                //this comparison will bring out the still affected, newly affected and no longer affected list of affected parties.
                if (DistributedMovAnalysisId != 0)
                {
                    RouteAssessmentModel routeObjNew = new RouteAssessmentModel();

                    byte[] prevMovVerAfftdParty = null;

                    byte[] currCandRtAfftdParty = null;

                    //fetching previous movement version affected party list
                    prevMovVerAfftdParty = RouteAssessmentProvider.Instance.GetDriveInstructionsinfo(DistributedMovAnalysisId, 7, userSchema).AffectedParties;

                    string prevMoveVerAfftdXml = Encoding.UTF8.GetString(XsltTransformer.Trafo(prevMovVerAfftdParty));

                    AffectedPartiesStructure prevMoveVerAffectedParties = StringExtractor.xmlAffectedPartyDeserializer(prevMoveVerAfftdXml);

                    //fetching current candidate route version affected party list
                    currCandRtAfftdParty = RouteAssessmentProvider.Instance.GetDriveInstructionsinfo(analysisId, 7, userSchema).AffectedParties;

                    string currCandRtAfftdXml = Encoding.UTF8.GetString(XsltTransformer.Trafo(currCandRtAfftdParty));

                    AffectedPartiesStructure currCandRtAffectedParties = StringExtractor.xmlAffectedPartyDeserializer(currCandRtAfftdXml);


                    //comparing both list of affected parties
                    currCandRtAffectedParties = StringExtractor.checkAffectedPartiesDiff(prevMoveVerAffectedParties, currCandRtAffectedParties);

                    currCandRtAffectedParties = AffectedStatusInformer.SortMovementAffectedPartyComparison(analysisId, DistributedMovAnalysisId, inputsForAssessment, currCandRtAffectedParties, userSchema);

                    //sorting the generated affected party list based on organisation name
                    if (currCandRtAffectedParties.GeneratedAffectedParties.Count != 0 || currCandRtAffectedParties.GeneratedAffectedParties != null)
                    {
                        var affectedPartylist = currCandRtAffectedParties.GeneratedAffectedParties;

                        currCandRtAffectedParties.GeneratedAffectedParties = affectedPartylist.OrderBy(t => t.Contact.Contact.simpleContactRef.OrganisationName).ToList();
                    }

                    //adding previous movement manual added parties to current candidate route affected party list.
                    if (prevMoveVerAffectedParties.ManualAffectedParties.Count != 0 || prevMoveVerAffectedParties.ManualAffectedParties != null)
                    {
                        currCandRtAffectedParties.ManualAffectedParties = prevMoveVerAffectedParties.ManualAffectedParties;
                    }

                    string result = StringExtractor.xmlAffectedPartySerializer(currCandRtAffectedParties);

                    routeObjNew.AffectedParties = StringExtractor.ZipAndBlob(result);

                    long tmpResult = RouteAssessmentProvider.Instance.updateRouteAssessment(routeObjNew, analysisId, userSchema);
                }
                #endregion
                string latestAffectedParties = null;

                #region Checking for any manual added party in current list
                //checking whether there is any manually added affected party or not in the previous list if yes ?
                if (prevCandidateAffectedParty.ManualAffectedParties.Count() >= 0)
                {
                    //the object is emptied to store new list
                    affectedParties = null;

                    //once again the affected party in list is fetched this time its the newly generated
                    affectedParties = RouteAssessmentProvider.Instance.GetDriveInstructionsinfo(analysisId, 7, userSchema).AffectedParties;

                    //fetching latest affected party list into a string 
                    latestAffectedParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(affectedParties));

                    //deserializing the list
                    currentCandidateAffectedParty = StringExtractor.xmlAffectedPartyDeserializer(latestAffectedParties);

                    if (currentCandidateAffectedParty.ManualAffectedParties.Count != 0 || currentCandidateAffectedParty.ManualAffectedParties != null)
                    {
                        try
                        {
                            var firstNotSecond = prevCandidateAffectedParty.ManualAffectedParties.Except(currentCandidateAffectedParty.ManualAffectedParties).ToList();

                            currentCandidateAffectedParty.ManualAffectedParties = firstNotSecond;
                        }
                        catch (Exception ex)
                        {

                        }

                        #region ignore this code portion 
                        //foreach (AffectedPartyStructure tmpAfftdParty in prevCandidateAffectedParty.ManualAffectedParties)
                        //{
                        //    try
                        //    {
                        //        if (tmpAfftdParty.Contact.Contact.adhocContactRef != null)
                        //        {
                        //            try
                        //            {
                        //                if (!currentCandidateAffectedParty.ManualAffectedParties.Any(x => x.Contact.Contact.adhocContactRef.FullName == tmpAfftdParty.Contact.Contact.adhocContactRef.FullName)
                        //                    && !currentCandidateAffectedParty.ManualAffectedParties.Any(x => x.Contact.Contact.adhocContactRef.OrganisationName == tmpAfftdParty.Contact.Contact.adhocContactRef.OrganisationName)
                        //                    )
                        //                {
                        //                    currentCandidateAffectedParty.ManualAffectedParties.Add(tmpAfftdParty);
                        //                }
                        //            }
                        //            catch
                        //            {

                        //            }

                        //        }
                        //        else if (tmpAfftdParty.Contact.Contact.simpleContactRef != null)
                        //        {
                        //            try
                        //            {
                        //                if (!currentCandidateAffectedParty.ManualAffectedParties.Any(x => x.Contact.Contact.simpleContactRef.FullName == tmpAfftdParty.Contact.Contact.simpleContactRef.FullName)
                        //                    && !currentCandidateAffectedParty.ManualAffectedParties.Any(x => x.Contact.Contact.simpleContactRef.OrganisationName == tmpAfftdParty.Contact.Contact.simpleContactRef.OrganisationName)
                        //                    )
                        //                {
                        //                    currentCandidateAffectedParty.ManualAffectedParties.Add(tmpAfftdParty);
                        //                }
                        //            }
                        //            catch
                        //            {

                        //            }
                        //        }

                        //    }
                        //    catch (Exception ex)
                        //    {

                        //    }
                        //}
                        #endregion
                    }
                    else
                    {
                        //copying the old manually added affected party list into the newly generated affected party list
                        currentCandidateAffectedParty.ManualAffectedParties = prevCandidateAffectedParty.ManualAffectedParties;
                    }

                    //retaining the excluded and other status

                    if ((currentCandidateAffectedParty.GeneratedAffectedParties != null || currentCandidateAffectedParty.GeneratedAffectedParties.Count != 0) && (prevCandidateAffectedParty.GeneratedAffectedParties != null || prevCandidateAffectedParty.GeneratedAffectedParties.Count != 0))
                    {
                        var generatedData = currentCandidateAffectedParty.GeneratedAffectedParties;

                        foreach (AffectedPartyStructure tmpAfftdParty in prevCandidateAffectedParty.GeneratedAffectedParties)
                        {

                            string tmpOrgName = tmpAfftdParty.Contact.Contact.simpleContactRef.OrganisationName;
                            string tmpFullName = tmpAfftdParty.Contact.Contact.simpleContactRef.FullName;
                            int tmpContId = tmpAfftdParty.Contact.Contact.simpleContactRef.ContactId;
                            int tmpOrgId = tmpAfftdParty.Contact.Contact.simpleContactRef.OrganisationId;

                            //if current list has a name and organisation from previous list 
                            if (currentCandidateAffectedParty.GeneratedAffectedParties.Exists(x => x.Contact.Contact.simpleContactRef.OrganisationId == tmpOrgId)
                               && currentCandidateAffectedParty.GeneratedAffectedParties.Exists(x => x.Contact.Contact.simpleContactRef.ContactId == tmpContId)
                                && currentCandidateAffectedParty.GeneratedAffectedParties.Exists(x => x.Contact.Contact.simpleContactRef.OrganisationName == tmpOrgName)
                                )
                            {
                                try
                                {
                                    //inserting the excluded status
                                    (from s in generatedData
                                     where (s.Contact.Contact.simpleContactRef.ContactId == tmpContId && s.Contact.Contact.simpleContactRef.OrganisationId == tmpOrgId && s.Contact.Contact.simpleContactRef.OrganisationName == tmpOrgName)
                                     select s).ToList().ForEach(s => s.Exclude = tmpAfftdParty.Exclude);
                                    //exclusion outcome commented 
                                    //(from s in generatedData
                                    // where (s.Contact.Contact.simpleContactRef.ContactId == tmpContId && s.Contact.Contact.simpleContactRef.OrganisationId == tmpOrgId && s.Contact.Contact.simpleContactRef.OrganisationName == tmpOrgName)
                                    // select s).ToList().ForEach(s => s.ExclusionOutcome = tmpAfftdParty.ExclusionOutcome);
                                }
                                catch
                                {

                                }
                            }
                            //else
                            //{
                            //    tmpAfftdParty.ExclusionOutcome = AffectedPartyReasonExclusionOutcomeType.nolongeraffected;
                            //    tmpAfftdParty.ExclusionOutcomeSpecified = true;
                            //    tmpAfftdParty.Reason = AffectedPartyReasonType.nolongeraffected;
                            //    tmpAfftdParty.ReasonSpecified = true;
                            //    currentCandidateAffectedParty.GeneratedAffectedParties.Add(tmpAfftdParty);
                            //}
                        }
                    }

                    //sorting the generated affected party list based on organisation name
                    if (currentCandidateAffectedParty.GeneratedAffectedParties.Count != 0 || currentCandidateAffectedParty.GeneratedAffectedParties != null)
                    {
                        var affectedPartylist = currentCandidateAffectedParty.GeneratedAffectedParties;
                        currentCandidateAffectedParty.GeneratedAffectedParties = affectedPartylist.OrderBy(t => t.Contact.Contact.simpleContactRef.OrganisationName).ToList();
                    }

                    //serializing the affected party object into XML string 
                    latestAffectedParties = StringExtractor.xmlAffectedPartySerializer(currentCandidateAffectedParty);

                    //converting the XML string into a blob field
                    affectedParties = StringExtractor.ZipAndBlob(latestAffectedParties);

                    RouteAssessmentModel objRouteAssessmentModel = new RouteAssessmentModel();

                    objRouteAssessmentModel.AffectedParties = affectedParties;

                    //updating the analysed route's table with the new affected party list
                    long tempVar = RouteAssessmentProvider.Instance.updateRouteAssessment(objRouteAssessmentModel, analysisId, userSchema);

                }

                #endregion

                #endregion
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        #endregion
        #region xmlAffectedStructuresDeserializer(string xml)
        /// <summary>
        /// Function to deserialize the Affected structures xml Data
        /// </summary>
        public static AnalysedStructures XmlAffectedStructuresDeserializer(string xml)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(AnalysedStructures));

                StringReader stringReader = new StringReader(xml);

                XmlTextReader xmlReader = new XmlTextReader(stringReader);

                object obj = deserializer.Deserialize(xmlReader);

                AnalysedStructures XmlData = (AnalysedStructures)obj;

                xmlReader.Close();

                return XmlData;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

    }
    #endregion

    #region XMLModifier class
    public class XMLModifier
    {


        /// <summary>
        /// function that returns the inbound document with newly added dispensation into it
        /// </summary>
        /// <param name="inboundNotif"></param>
        /// <param name="DispInbnd"></param>
        /// <param name="InbndDisp"></param>
        /// <param name="analysisId"></param>
        /// <param name="notifId"></param>
        /// <param name="drnNumber"></param>
        /// <param name="grantorName"></param>
        /// <param name="grantorId"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public static string AddingDispensationToInbound(byte[] inboundNotif, Dispensations DispInbnd, InboundDispensation InbndDisp, int analysisId, int notifId, string drnNumber, string grantorName, int grantorId, string userSchema)
        {
            string message = "", newInboundXml = "";
            bool result = false;
            try
            {
                RouteAssessmentModel objRouteAssessmentModel = new RouteAssessmentModel();

                string xmlinboundNotif = Encoding.UTF8.GetString(XsltTransformer.Trafo(inboundNotif)); //converting Inbound to 

                XmlSerializer deserializer = new XmlSerializer(typeof(InboundNotification));

                StringReader stringReader = new StringReader(xmlinboundNotif);

                XmlTextReader xmlReader = new XmlTextReader(stringReader);

                object obj = deserializer.Deserialize(xmlReader);

                InboundNotification XmlData = (InboundNotification)obj;

                if (XmlData.Dispensation == null)
                {
                    XmlData.Dispensation = DispInbnd;

                    result = ModifyingDispStatusToInUse(analysisId, grantorName, grantorId, InbndDisp, userSchema);
                }
                else
                {
                    if (!XmlData.Dispensation.InboundDispensation.Exists(x => x.DRN == drnNumber))
                    {
                        XmlData.Dispensation.InboundDispensation.AddRange(DispInbnd.InboundDispensation);

                        result = ModifyingDispStatusToInUse(analysisId, grantorName, grantorId, InbndDisp, userSchema);
                    }
                    else
                    {
                        message = "Dispensation already included";

                        result = ModifyingDispStatusToInUse(analysisId, grantorName, grantorId, InbndDisp, userSchema);
                    }

                }

                newInboundXml = StringExtractor.xmlInboundNotifSerializer(XmlData);
            }
            catch
            {

            }

            if (!result)
            {
                message = "Error ESD#ADTI#001 : Contact helpdesk.";
            }

            return message != "" ? message : newInboundXml;
        }

        public static bool ModifyingDispStatusToInUse(int analysisId, string grantorName, int grantorId, InboundDispensation InbndDisp, string userSchema)
        {
            byte[] affectdParty = null;
            string xmlAffectedParty = null;
            bool result = false;

            RouteAssessmentModel objRouteAssessmentModel = new RouteAssessmentModel();

            affectdParty = RouteAssessmentProvider.Instance.GetDriveInstructionsinfo(analysisId, 7, UserSchema.Portal).AffectedParties;

            //sample object 1
            Domain.RouteAssessment.XmlAffectedParties.AffectedPartiesStructure existingAfftdPartyObj = null;

            //the below condition and code portion generates affected parties in every case it retains the user added manually added parties in case there is any
            if (affectdParty != null)
            {
                //fetching affected party list into a string 
                string existingAffectedParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(affectdParty));

                //Deserializing it into object and storing it 
                existingAfftdPartyObj = StringExtractor.xmlAffectedPartyDeserializer(existingAffectedParties);

                xmlAffectedParty = StringExtractor.xmlAffectedPartyIncludeDispensation(existingAfftdPartyObj, grantorName, "", grantorId, InbndDisp);

                result = true;
            }

            if (xmlAffectedParty != null)
            {
                objRouteAssessmentModel.AffectedParties = StringExtractor.ZipAndBlob(xmlAffectedParty);

                long status = RouteAssessmentProvider.Instance.updateRouteAssessment(objRouteAssessmentModel, analysisId, userSchema);

                result = true;
            }

            return result;
        }

        public static void RemovingDispensationFromInbound()
        {

        }

        /// <summary>
        /// function to modify the some matching status of dispensation in affected party list when the list is generated multiple times
        /// </summary>
        /// <param name="NotificationID"></param>
        /// <param name="existingAfftdPartyObj"></param>
        /// <param name="AnalysisId"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public static int ModifyingDispStatusToInUse(int NotificationID, AffectedPartiesStructure existingAfftdPartyObj, int AnalysisId, string userSchema)
        {
            //sample object 2
            AffectedPartiesStructure latestAfftdPartyObj = null;

            int orgId = 0;

            //the object is emptied to store new list
            RouteAssessmentModel objRouteAssessmentModel = new RouteAssessmentModel();

            //once again the affected party is list is fetched this time its the newly generated
            objRouteAssessmentModel = RouteAssessmentProvider.Instance.GetDriveInstructionsinfo(AnalysisId, 7, userSchema);

            //fetching latest affected party list into a string 
            string latestAffectedParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.AffectedParties));

            //deserializing the list
            latestAfftdPartyObj = StringExtractor.xmlAffectedPartyDeserializer(latestAffectedParties);

            var generatedData = latestAfftdPartyObj.GeneratedAffectedParties;

            foreach (AffectedPartyStructure affprtStruct in existingAfftdPartyObj.GeneratedAffectedParties)
            {
                orgId = affprtStruct.Contact.Contact.simpleContactRef.OrganisationId;

                if (affprtStruct.DispensationStatus == DispensationStatusType.inuse)
                {

                    (from s in generatedData
                     where s.DispensationStatus == DispensationStatusType.somematching && s.Contact.Contact.simpleContactRef.OrganisationId == orgId
                     select s).ToList().ForEach(s => s.DispensationStatus = DispensationStatusType.inuse);
                }
            }

            latestAfftdPartyObj.GeneratedAffectedParties = generatedData;


            //sorting the generated affected party list based on organisation name
            if (latestAfftdPartyObj.GeneratedAffectedParties.Count != 0 || latestAfftdPartyObj.GeneratedAffectedParties != null)
            {
                var affectedPartylist = latestAfftdPartyObj.GeneratedAffectedParties;

                latestAfftdPartyObj.GeneratedAffectedParties = affectedPartylist.OrderBy(t => t.Contact.Contact.simpleContactRef.OrganisationName).ToList();
            }

            //serializing the affected party object into XML string 
            latestAffectedParties = StringExtractor.xmlAffectedPartySerializer(latestAfftdPartyObj);

            //converting the XML string into a blob field
            objRouteAssessmentModel.AffectedParties = StringExtractor.ZipAndBlob(latestAffectedParties);

            //updating the analysed route's table with the new affected party list
            long tempVar = RouteAssessmentProvider.Instance.updateRouteAssessment(objRouteAssessmentModel, AnalysisId, userSchema);

            return (int)tempVar;
        }

        /// <summary>
        /// function to update the dispensation status while renotifying from previous version of affected party
        /// </summary>
        /// <param name="oldAffectedParty"></param>
        /// <param name="newAffectedParty"></param>
        /// <returns></returns>
        public static AffectedPartiesStructure ModifyingDispStatusToInUse(AffectedPartiesStructure oldAffectedParty, AffectedPartiesStructure newAffectedParty)
        {
            int orgId = 0;

            var generatedData = newAffectedParty.GeneratedAffectedParties;

            foreach (AffectedPartyStructure affprtStruct in oldAffectedParty.GeneratedAffectedParties)
            {
                orgId = affprtStruct.Contact.Contact.simpleContactRef.OrganisationId;

                if (affprtStruct.DispensationStatus == DispensationStatusType.inuse)
                {

                    (from s in generatedData
                     where s.DispensationStatus == DispensationStatusType.somematching && s.Contact.Contact.simpleContactRef.OrganisationId == orgId
                     select s).ToList().ForEach(s => s.DispensationStatus = DispensationStatusType.inuse);
                }
            }

            newAffectedParty.GeneratedAffectedParties = generatedData;
            newAffectedParty.ManualAffectedParties = oldAffectedParty.ManualAffectedParties;

            return newAffectedParty;
        }
    }
    #endregion

    #region RetransmitDocumentDomain
    public class RetransmitDocumentDomain
    {
        /// <summary>
        /// function to fetch reply to email address for retransmission from document
        /// </summary>
        /// <param name="Doc"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public static string GetHaHaulEmailAddressForRetransmit(XmlDocument Doc, string tagName)
        {
            try
            {
                string HaEmailAddress = "";
                string HaulEmailAddress = "";
                XmlNodeList HaulContactlist = null, HaContactlist = null;

                switch (tagName)
                {
                    case "HaulierDetails":
                        HaulContactlist = Doc.GetElementsByTagName(tagName);

                        foreach (XmlElement childrenNode in HaulContactlist)
                        {
                            foreach (XmlElement xmlElement in childrenNode)
                            {
                                if (xmlElement.Name == "EmailAddress")
                                {
                                    HaulEmailAddress = xmlElement.InnerText;
                                }
                            }
                        }

                        break;
                    case "HAContact":
                        HaContactlist = Doc.GetElementsByTagName(tagName);

                        foreach (XmlElement childrenNode in HaContactlist)
                        {
                            foreach (XmlElement xmlElement in childrenNode)
                            {
                                if (xmlElement.Name == "EmailAddress")
                                {
                                    HaEmailAddress = xmlElement.InnerText;
                                }
                            }
                        }


                        break;
                }
                return tagName == "HaulierDetails" ? HaulEmailAddress : HaEmailAddress;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
    #endregion

    #region AffectedStatusInformer
    public class AffectedStatusInformer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="candAnalysisId"></param>
        /// <param name="distrMoveAnalysisId"></param>
        /// <param name="inputsForAssessment"></param>
        /// <param name="currCandAffectedParty"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public static AffectedPartiesStructure SortMovementAffectedPartyComparison(int candAnalysisId, int distrMoveAnalysisId, RouteAssessmentInputs inputsForAssessment, AffectedPartiesStructure currCandAffectedParty, string userSchema = UserSchema.Sort)
        {
            try
            {
                int updated = 0;
                //fetching candidate route analysis info
                //#5228 : Fix to update the route assessment needed affected structures and , affected roads each time.
                // This is fix was needed because the assumption that user will reassess structures and roads before reassessing parties is not 
                // Possible.
                updated = RouteAssessmentProvider.Instance.UpdateRouteAssessment(inputsForAssessment, 3, userSchema);//To update affected structures
                updated = RouteAssessmentProvider.Instance.UpdateRouteAssessment(inputsForAssessment, 8, userSchema);//To update affected roads
                //#5228

                RouteAssessmentModel candidateAssessedInfo = RouteAssessmentProvider.Instance.GetDriveInstructionsinfo(candAnalysisId, 9, userSchema);
                //fetching distributed movement analysis info
                RouteAssessmentModel distrMoveAssessedInfo = RouteAssessmentProvider.Instance.GetDriveInstructionsinfo(distrMoveAnalysisId, 9, userSchema);


                #region Code commented for #5228
                
                #endregion
                //object for current candidate route this is a global variable
                RouteAnalysisXml candidateRouteAnalysisXml = ManagingTheAssessmentInfo.GetAssessmentXmlFromByteArray(candidateAssessedInfo);
                RouteAnalysisObject candidateRouteAnalysisObjFromXml = ManagingTheAssessmentInfo.GetAssessedObjectFromXml(candidateRouteAnalysisXml);

                RouteAnalysisXml distributedRouteAnalysisXml = ManagingTheAssessmentInfo.GetAssessmentXmlFromByteArray(distrMoveAssessedInfo);
                RouteAnalysisObject distributedRouteAnalysisObjFromXml = ManagingTheAssessmentInfo.GetAssessedObjectFromXml(distributedRouteAnalysisXml);

                currCandAffectedParty = AssessmentInfoComaparerForCandidateRoute(candidateRouteAnalysisObjFromXml, distributedRouteAnalysisObjFromXml, currCandAffectedParty);

                currCandAffectedParty = ManagingTheAssessmentInfo.CompareWithDistrMovementToFindNewlyAffected(currCandAffectedParty, distributedRouteAnalysisObjFromXml.AffectedPartyList);

                #region #5171 : SORT: Parties which are no longer affected should be removed from the affected parties page for subsequent versions
                currCandAffectedParty = ManagingTheAssessmentInfo.CompareAndRemoveNoLongerAffectedFromSubsequentVersions(currCandAffectedParty, distributedRouteAnalysisObjFromXml.AffectedPartyList);
                #endregion

                return currCandAffectedParty;
            }
            catch (Exception ex)
            {
                STP.Common.Logger.Logger.GetInstance().LogMessage(Common.Logger.Log_Priority.FATAL_ERROR, "Exception Occured affected party generation: {0}", ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="candidateRouteObj"></param>
        /// <param name="distributedMovementObj"></param>
        /// <param name="currCandAffectedParty"></param>
        /// <returns></returns>
        public static AffectedPartiesStructure AssessmentInfoComaparerForCandidateRoute(RouteAnalysisObject candidateRouteObj, RouteAnalysisObject distributedMovementObj, AffectedPartiesStructure currCandAffectedParty)
        {
            try
            {
                Dictionary<string, List<int>> roadNameOrgDictCandRoute = ManagingTheAssessmentInfo.GetRoadNameOrgIdDict(candidateRouteObj.AnalysedRoadList);

                List<RoadDistanceInfo> roadDistanceDictList = ManagingTheAssessmentInfo.GetRoadDistanceList(candidateRouteObj.AnalysedRoadList);

                Dictionary<string, List<int>> roadNameOrgDictDistrMovmnt = ManagingTheAssessmentInfo.GetRoadNameOrgIdDict(distributedMovementObj.AnalysedRoadList);

                List<RoadDistanceInfo> roadDistanceDistrDictList = ManagingTheAssessmentInfo.GetRoadDistanceList(distributedMovementObj.AnalysedRoadList);

                Dictionary<string, List<int>> structureOrgDictCandRoute = ManagingTheAssessmentInfo.GetStructureNameOrgIdDict(candidateRouteObj.AnalysedStructureList);

                Dictionary<string, List<int>> structureOrgDictDistrMovmnt = ManagingTheAssessmentInfo.GetStructureNameOrgIdDict(distributedMovementObj.AnalysedStructureList);

                Dictionary<string, List<int>> roadPartyStatusList = ManagingTheAssessmentInfo.GetRoadNameOrgIdDict(roadNameOrgDictCandRoute, roadNameOrgDictDistrMovmnt, roadDistanceDictList, roadDistanceDistrDictList);

                Dictionary<string, List<int>> structurePartyStatusList = ManagingTheAssessmentInfo.GetStructNameOrgIdDict(structureOrgDictCandRoute, structureOrgDictDistrMovmnt);

                Dictionary<string, List<int>> afftdPartyOrdIdStatusList = new Dictionary<string, List<int>>();

                if (currCandAffectedParty != null || currCandAffectedParty.GeneratedAffectedParties.Count != 0)
                {
                    afftdPartyOrdIdStatusList = ManagingTheAssessmentInfo.GetAffectedPartyObjectGrouped(roadPartyStatusList, structurePartyStatusList);

                    var generatedData = currCandAffectedParty.GeneratedAffectedParties;

                    foreach (int orgId in afftdPartyOrdIdStatusList["no longer affected"])
                    {
                        if (currCandAffectedParty.GeneratedAffectedParties.Exists(x => x.Contact.Contact.simpleContactRef.OrganisationId == orgId)) //check whether the organisation id is present in candidate routes affected party list
                        {
                            (from s in generatedData
                             where (s.Contact.Contact.simpleContactRef.OrganisationId == orgId)
                             select s).ToList().ForEach(s => s.Reason = AffectedPartyReasonType.nolongeraffected);

                            (from s in generatedData
                             where (s.Contact.Contact.simpleContactRef.OrganisationId == orgId)
                             select s).ToList().ForEach(s => s.ReasonSpecified = true);

                            (from s in generatedData
                             where (s.Contact.Contact.simpleContactRef.OrganisationId == orgId)
                             select s).ToList().ForEach(s => s.ExclusionOutcome = AffectedPartyReasonExclusionOutcomeType.nolongeraffected);

                            (from s in generatedData
                             where (s.Contact.Contact.simpleContactRef.OrganisationId == orgId)
                             select s).ToList().ForEach(s => s.ExclusionOutcomeSpecified = true);
                        }
                    }

                    foreach (int orgId in afftdPartyOrdIdStatusList["still affected"])
                    {
                        if (currCandAffectedParty.GeneratedAffectedParties.Exists(x => x.Contact.Contact.simpleContactRef.OrganisationId == orgId)) //check whether the organisation id is present in candidate routes affected party list
                        {
                            (from s in generatedData
                             where (s.Contact.Contact.simpleContactRef.OrganisationId == orgId)
                             select s).ToList().ForEach(s => s.Reason = AffectedPartyReasonType.stillaffected);

                            (from s in generatedData
                             where (s.Contact.Contact.simpleContactRef.OrganisationId == orgId)
                             select s).ToList().ForEach(s => s.ReasonSpecified = true);

                            (from s in generatedData
                             where (s.Contact.Contact.simpleContactRef.OrganisationId == orgId)
                             select s).ToList().ForEach(s => s.ExclusionOutcome = AffectedPartyReasonExclusionOutcomeType.stillaffected);

                            (from s in generatedData
                             where (s.Contact.Contact.simpleContactRef.OrganisationId == orgId)
                             select s).ToList().ForEach(s => s.ExclusionOutcomeSpecified = true);
                        }
                    }

                    foreach (int orgId in afftdPartyOrdIdStatusList["affected by change"])
                    {
                        if (currCandAffectedParty.GeneratedAffectedParties.Exists(x => x.Contact.Contact.simpleContactRef.OrganisationId == orgId)) //check whether the organisation id is present in candidate routes affected party list
                        {
                            (from s in generatedData
                             where (s.Contact.Contact.simpleContactRef.OrganisationId == orgId)
                             select s).ToList().ForEach(s => s.Reason = AffectedPartyReasonType.affectedbychangeofroute);

                            (from s in generatedData
                             where (s.Contact.Contact.simpleContactRef.OrganisationId == orgId)
                             select s).ToList().ForEach(s => s.ReasonSpecified = true);

                            (from s in generatedData
                             where (s.Contact.Contact.simpleContactRef.OrganisationId == orgId)
                             select s).ToList().ForEach(s => s.ExclusionOutcome = AffectedPartyReasonExclusionOutcomeType.affectedbychangeofroute);

                            (from s in generatedData
                             where (s.Contact.Contact.simpleContactRef.OrganisationId == orgId)
                             select s).ToList().ForEach(s => s.ExclusionOutcomeSpecified = true);
                        }
                    }

                    currCandAffectedParty.GeneratedAffectedParties = generatedData;
                }

                return currCandAffectedParty;
            }
            catch (Exception ex)
            {
                STP.Common.Logger.Logger.GetInstance().LogMessage(Common.Logger.Log_Priority.FATAL_ERROR, "Exception Occured affected party generation: {0}", ex.StackTrace);
                throw;
            }
        }
    }
    #endregion
}