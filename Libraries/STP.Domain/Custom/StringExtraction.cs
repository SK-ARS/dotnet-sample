using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using STP.Common.Constants;
using STP.Common.General;
using STP.Common.Logger;
using STP.Domain.RouteAssessment;
using STP.Domain.RouteAssessment.XmlAffectedParties;
using STP.Domain.RouteAssessment.XmlAnalysedCautions;
using STP.Domain.RouteAssessment.XmlAnalysedRoads;
using STP.Domain.RouteAssessment.XmlAnalysedStructures;
using STP.Domain.RouteAssessment.XmlConstraints;
using STP.Domain.SecurityAndUsers;

namespace STP.Domain.Custom
{
    public static class StringExtraction
    {
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

        #region  XmlAffectedPartySerializer
        public static string XmlAffectedPartySerializer(AffectedPartiesStructure XmlData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AffectedPartiesStructure));

            StringWriter outStream = new Utf8StringWriter();    // The code generates UTF-8 encoded xml string 
            serializer.Serialize(outStream, XmlData);
            return outStream.ToString(); // Output string 
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

        /// <summary>
        /// function to update the dispensation status while renotifying from previous version of affected party
        /// </summary>
        /// <param name="oldAffectedParty"></param>
        /// <param name="newAffectedParty"></param>
        /// <returns></returns>
        public static AffectedPartiesStructure ModifyingDispStatusToInUse(AffectedPartiesStructure newAffectedParty, AffectedPartiesStructure oldAffectedParty)
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
            return newAffectedParty;
        }

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

        #region XmlStructureSerializer
        public static string XmlStructureSerializer(AnalysedStructures XmlData)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AnalysedStructures));
                StringWriter outStream = new Utf8StringWriter();    // The code generates UTF-8 encoded xml string 
                serializer.Serialize(outStream, XmlData);
                return outStream.ToString(); // Output string 
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"XmlStructureSerializer, Exception: " + ex);
                return null;
            }
        }
        #endregion

        #region CheckForDuplicateGenAffectdParty
        public static int CheckForDuplicateGenAffectdParty(List<AffectedPartyStructure> generatedAfftdPartyList, AffectedPartyStructure generatedAfftdParty)
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
                return outStream.ToString(); // Output string 
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"XmlCautionSerializer, Exception: " + ex);
                return null;
            }
        }
        #endregion

        #region  XmlStringExtractor
        public static string XmlStringExtractor(string xmlString, string nameSpace)
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
                        return xmlDoc.InnerText;
                    case "SpecificAction":
                        nsManager.AddNamespace("ns", xmlDoc.DocumentElement.NamespaceURI);
                        XmlNode specificAction = xmlDoc.DocumentElement.SelectSingleNode("/ns:SpecificAction", nsManager);
                        return specificAction.InnerText;
                    case "route":
                        nsManager.AddNamespace("route", xmlDoc.DocumentElement.NamespaceURI);
                        return xmlDoc.InnerText;
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"XmlStringExtractor, Exception: " + ex);
                return null;
            }
        }
        #endregion

        #region ConstraintListToXml
        public static AnalysedConstraintsPart ConstraintListToXml(AnalysedConstraintsPart analConstrPart, List<RouteConstraints> constrList, int routePartId, string routeName)
        {
            int i = 0;

            Constraint analConstr;

            analConstrPart = new AnalysedConstraintsPart
            {
                Id = routePartId,

                AnalysedConstraintsPartName = routeName, // constraint route_part Name 

                Constraint = new List<Constraint>()
            };

            foreach (RouteConstraints constrObj in constrList)
            {
                try
                {
                    analConstr = new Constraint
                    {
                        ECRN = constrObj.ConstraintCode,

                        ConstraintType = constrObj.ConstraintType,

                        IsApplicable = true, //need to differ dynamically

                        ConstraintName = constrObj.ConstraintName,

                        //constraint  Suitability
                        Appraisal = new Domain.RouteAssessment.XmlConstraints.Appraisal()
                    };

                    analConstr.Appraisal.Suitability = new Suitability
                    {
                        Value = constrObj.ConstraintSuitability
                    };

                    analConstrPart.Constraint.Add(analConstr);

                    i++;
                }
                catch (Exception ex)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"ConstraintListToXml, Exception: " + ex);
                    throw;
                }
            }
            return analConstrPart;
        }
        #endregion

        #region XmlConstraintSerializer
        /// <summary>
        ///Function to Serialize the Analysed constraint object to constraint xml
        /// </summary>
        public static string XmlConstraintSerializer(AnalysedConstraints XmlData)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AnalysedConstraints));
                StringWriter outStream = new Utf8StringWriter();    // The code generates UTF-8 encoded xml string 
                serializer.Serialize(outStream, XmlData);
                return outStream.ToString(); // Output string
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"ConstraintListToXml, Exception: " + ex);
                return null;
            }
        }
        #endregion

        #region ZipAndBlob
        public static byte[] ZipAndBlob(string value)
        {
            byte[] compressedData;
            try
            {
                byte[] XMLPage = Encoding.UTF8.GetBytes(value);//Converting string to byte array
                MemoryStream ms = new MemoryStream();//compressing the byte array
                Stream zipStream = new GZipStream(ms, CompressionMode.Compress);
                zipStream.Write(XMLPage, 0, XMLPage.Length);
                zipStream.Close();
                compressedData = ms.ToArray();
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"ZipAndBlob, Exception: " + ex);
                compressedData = null;
            }
            return compressedData;
        }
        #endregion

        #region AnnotationSerializer
        public static string AnnotationSerializer(RouteAssessment.XmlAnalysedAnnotations.AnalysedAnnotations annotationXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RouteAssessment.XmlAnalysedAnnotations.AnalysedAnnotations));

            StringWriter outStream = new Utf8StringWriter();    // The code generates UTF-8 encoded xml string 
            serializer.Serialize(outStream, annotationXml);

            return outStream.ToString(); // Output string 
        }
        #endregion

        #region AnalysedRoadsSerializer function
        public static string AnalysedRoadsSerializer(AnalysedRoadsRoute XmlData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AnalysedRoadsRoute));

            StringWriter outStream = new Utf8StringWriter();    // The code generates UTF-8 encoded xml string 
            serializer.Serialize(outStream, XmlData);
            return outStream.ToString(); // Output string 
        }
        #endregion

        #region GetRecipientDetails
        public static List<ContactModel> GetRecipientDetails(string recipientXMLInformation)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(recipientXMLInformation);

            ContactModel contactInfo;
            List<ContactModel> contactList = new List<ContactModel>();

            XmlNodeList parentNode = xmlDoc.GetElementsByTagName("Recipients");
            foreach (XmlElement childrenNode in parentNode)
            {
                foreach (XmlElement xmlElement in childrenNode)
                {
                    if (xmlElement.Name == "Contact")
                    {
                        XmlElement Contact = xmlElement;
                        if ((Contact != null) && Contact.HasAttribute("ContactId"))
                        {
                            int contactId = 0;
                            bool isPolice = false; // For RM#4340 
                            string orgname = "";
                            string contactName = string.Empty;
                            string reason = string.Empty;
                            string fax = string.Empty;
                            string email = string.Empty;
                            if (xmlElement.Attributes["ContactId"].InnerText.Contains("##**##"))
                                contactId = Convert.ToInt32(xmlElement.Attributes["ContactId"].InnerText.Split("##**##".ToCharArray())[6]);
                            else
                                contactId = Convert.ToInt32(xmlElement.Attributes["ContactId"].InnerText);

                            if (xmlElement.Attributes["IsPolice"].InnerText.Contains("##**##"))//Condition For RM#4340 
                            {
                                isPolice = Convert.ToBoolean(xmlElement.Attributes["IsPolice"].InnerText.Split("##**##".ToCharArray())[6]);
                            }
                            else
                            {
                                isPolice = Convert.ToBoolean(xmlElement.Attributes["IsPolice"].InnerText);
                            }

                            if (xmlElement.ChildNodes.Item(0).Name == "ContactName")
                            {
                                contactName = xmlElement.ChildNodes.Item(0).InnerText;
                            }
                            if (xmlElement.ChildNodes.Item(1).Name == "OrganisationName")
                            {
                                orgname = xmlElement.ChildNodes.Item(1).InnerText;
                            }
                            if (xmlElement.ChildNodes.Item(2) != null)
                            {
                                if (xmlElement.ChildNodes.Item(2).Name == "Fax")
                                {
                                    fax = xmlElement.ChildNodes.Item(2).InnerText;
                                }
                                else if (xmlElement.ChildNodes.Item(2).Name == "Email") // Checking for email in 2nd position if fax is not found 
                                {
                                    email = xmlElement.ChildNodes.Item(2).InnerText;
                                }
                            }
                            if (xmlElement.ChildNodes.Item(3) != null)
                            {
                                if (xmlElement.ChildNodes.Item(3).Name == "Email")
                                {
                                    email = xmlElement.ChildNodes.Item(3).InnerText;
                                }
                                else if (xmlElement.ChildNodes.Item(3).Name == "Fax") // Checking for fax in 3rd position if email is not found 
                                {
                                    fax = xmlElement.ChildNodes.Item(3).InnerText;
                                }
                            }
                            if ((Contact != null) && Contact.HasAttribute("Reason"))
                            {
                                reason = Convert.ToString(Contact.Attributes["Reason"].InnerText);
                            }
                            contactInfo = new ContactModel()
                            {
                                ContactId = contactId,
                                FullName = contactName,
                                Email = email, // fax,
                                Fax = fax, // email,
                                Reason = reason,
                                Organisation = orgname,
                                ISPolice = isPolice // For RM#4340
                            };
                            contactList.Add(contactInfo);
                        }
                    }
                }
            }
            return contactList;
        }
        #endregion

        #region GenerateNotificationXML
        public static STP.Domain.DocumentsAndContents.XMLModel GenerateNotificationXML(int notificationId, NotificationXSD.OutboundNotificationStructure obns, NotificationXSD.NotificationTypeType psPortalType)
        {
            string retunXML = string.Empty;
            STP.Domain.DocumentsAndContents.XMLModel model = new STP.Domain.DocumentsAndContents.XMLModel();
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

        #region GenerateNotificationXML
        public static string ConvertSpecialCharactersToLetters(string xmlDocument)
        {
            xmlDocument = xmlDocument.Replace("£", "#@GBP@#");
            xmlDocument = xmlDocument.Replace("€", "#@Pound@#");
            xmlDocument = xmlDocument.Replace("₾", "#@GEL@#");
            xmlDocument = xmlDocument.Replace("лв", "#@BGN@#");
            xmlDocument = xmlDocument.Replace("₺", "#@Turkey@#");
            return xmlDocument;
        }
        #endregion


        /// </summary>
        /// <param name="candAnalysisId"></param>
        /// <param name="distrMoveAnalysisId"></param>
        /// <param name="inputsForAssessment"></param>
        /// <param name="currCandAffectedParty"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public static AffectedPartiesStructure SortMovementAffectedPartyComparison(RouteAssessmentModel candidateAssessedInfo, RouteAssessmentModel distrMoveAssessedInfo, AffectedPartiesStructure currCandAffectedParty)
        {
            try
            {
                //object for current candidate route this is a global variable
                RouteAnalysisXml candidateRouteAnalysisXml = GetAssessmentXmlFromByteArray(candidateAssessedInfo);
                RouteAnalysisObject candidateRouteAnalysisObjFromXml = GetAssessedObjectFromXml(candidateRouteAnalysisXml);

                RouteAnalysisXml distributedRouteAnalysisXml = GetAssessmentXmlFromByteArray(distrMoveAssessedInfo);
                RouteAnalysisObject distributedRouteAnalysisObjFromXml = GetAssessedObjectFromXml(distributedRouteAnalysisXml);

                currCandAffectedParty = AssessmentInfoComaparerForCandidateRoute(candidateRouteAnalysisObjFromXml, distributedRouteAnalysisObjFromXml, currCandAffectedParty);

                currCandAffectedParty = CompareWithDistrMovementToFindNewlyAffected(currCandAffectedParty, distributedRouteAnalysisObjFromXml.AffectedPartyList);

                currCandAffectedParty = CompareAndRemoveNoLongerAffectedFromSubsequentVersions(currCandAffectedParty, distributedRouteAnalysisObjFromXml.AffectedPartyList);

                return currCandAffectedParty;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Common.Logger.Log_Priority.FATAL_ERROR, "Exception Occured affected party generation: {0}", ex.StackTrace);
                throw ex;
            }
        }
        #region GetAssessmentXmlFromByteArray(RouteAssessmentModel inputAssessmentByteInfo)
        /// <summary>
        /// Function to get xml string from assessed byte array
        /// </summary>
        /// <param name="inputAssessmentByteInfo"></param>
        /// <returns></returns>
        public static RouteAnalysisXml GetAssessmentXmlFromByteArray(RouteAssessmentModel inputAssessmentByteInfo)
        {
            RouteAnalysisXml outputRouteAssessedXml = new RouteAnalysisXml();
            try
            {
                //structures
                outputRouteAssessedXml.XmlAnalysedStructure = inputAssessmentByteInfo.AffectedStructure != null ? Encoding.UTF8.GetString(XsltTransformer.Trafo(inputAssessmentByteInfo.AffectedStructure)) : null;
                //affected parties
                outputRouteAssessedXml.XmlAffectedParties = inputAssessmentByteInfo.AffectedParties != null ? Encoding.UTF8.GetString(XsltTransformer.Trafo(inputAssessmentByteInfo.AffectedParties)) : null;
                //affected roads
                outputRouteAssessedXml.XmlAffectedRoads = inputAssessmentByteInfo.AffectedRoads != null ? Encoding.UTF8.GetString(XsltTransformer.Trafo(inputAssessmentByteInfo.AffectedRoads)) : null;
                //annotations
                outputRouteAssessedXml.XmlAnalysedAnnotations = inputAssessmentByteInfo.Annotation != null ? Encoding.UTF8.GetString(XsltTransformer.Trafo(inputAssessmentByteInfo.Annotation)) : null;
                //cautions
                outputRouteAssessedXml.XmlAnalysedCautions = inputAssessmentByteInfo.Cautions != null ? Encoding.UTF8.GetString(XsltTransformer.Trafo(inputAssessmentByteInfo.Cautions)) : null;
                //constraints
                outputRouteAssessedXml.XmlAnalysedConstraints = inputAssessmentByteInfo.Constraints != null ? Encoding.UTF8.GetString(XsltTransformer.Trafo(inputAssessmentByteInfo.Constraints)) : null;

            }
            catch (Exception ex)
            {
                STP.Common.Logger.Logger.GetInstance().LogMessage(Common.Logger.Log_Priority.FATAL_ERROR, "Exception Occured affected party generation: {0}", ex.StackTrace);
                throw;
            }
            return outputRouteAssessedXml;
        }
        #endregion
        #region GetAssessedObjectFromXml(RouteAnalysisXml inputAssessedXmlStringInfo)
        /// <summary>
        /// Function to get assessed object from assessed xml string
        /// </summary>
        /// <param name="inputAssessedXmlStringInfo"></param>
        /// <returns></returns>
        public static RouteAnalysisObject GetAssessedObjectFromXml(RouteAnalysisXml inputAssessedXmlStringInfo)
        {
            RouteAnalysisObject outputRouteAssessedObj = new RouteAnalysisObject();

            try
            {
                //structures
                outputRouteAssessedObj.AnalysedStructureList = inputAssessedXmlStringInfo.XmlAnalysedStructure != null ? XmlDeserializerStructures(inputAssessedXmlStringInfo.XmlAnalysedStructure) : null;
                //affected parties
                outputRouteAssessedObj.AffectedPartyList = inputAssessedXmlStringInfo.XmlAffectedParties != null ? xmlAffectedPartyDeserializer(inputAssessedXmlStringInfo.XmlAffectedParties) : null;
                //affected roads
                outputRouteAssessedObj.AnalysedRoadList = inputAssessedXmlStringInfo.XmlAffectedRoads != null ? AnalysedRoadsDeserializer(inputAssessedXmlStringInfo.XmlAffectedRoads) : null;
                //annotations
                outputRouteAssessedObj.AnnotationList = inputAssessedXmlStringInfo.XmlAnalysedAnnotations != null ? AnnotationDeserializer(inputAssessedXmlStringInfo.XmlAnalysedAnnotations) : null;
                //cautions
                outputRouteAssessedObj.AnalysedCautionList = inputAssessedXmlStringInfo.XmlAnalysedCautions != null ? XmlDeserializeCautions(inputAssessedXmlStringInfo.XmlAnalysedCautions) : null;
                //constraints
                outputRouteAssessedObj.AnalysedConstraintList = inputAssessedXmlStringInfo.XmlAnalysedConstraints != null ? constraintDeserializer(inputAssessedXmlStringInfo.XmlAnalysedConstraints) : null;

            }
            catch (Exception)
            {
                throw;
            }
            return outputRouteAssessedObj;
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


        /// </summary>
        /// <param name="candidateRouteObj"></param>
        /// <param name="distributedMovementObj"></param>
        /// <param name="currCandAffectedParty"></param>
        /// <returns></returns>
        public static AffectedPartiesStructure AssessmentInfoComaparerForCandidateRoute(RouteAnalysisObject candidateRouteObj, RouteAnalysisObject distributedMovementObj, AffectedPartiesStructure currCandAffectedParty)
        {
            try
            {
                Dictionary<string, List<int>> roadNameOrgDictCandRoute = GetRoadNameOrgIdDict(candidateRouteObj.AnalysedRoadList);

                List<RoadDistanceInfo> roadDistanceDictList = GetRoadDistanceList(candidateRouteObj.AnalysedRoadList);

                Dictionary<string, List<int>> roadNameOrgDictDistrMovmnt = GetRoadNameOrgIdDict(distributedMovementObj.AnalysedRoadList);

                List<RoadDistanceInfo> roadDistanceDistrDictList = GetRoadDistanceList(distributedMovementObj.AnalysedRoadList);

                Dictionary<string, List<int>> structureOrgDictCandRoute = GetStructureNameOrgIdDict(candidateRouteObj.AnalysedStructureList);

                Dictionary<string, List<int>> structureOrgDictDistrMovmnt = GetStructureNameOrgIdDict(distributedMovementObj.AnalysedStructureList);

                Dictionary<string, List<int>> roadPartyStatusList = GetRoadNameOrgIdDict(roadNameOrgDictCandRoute, roadNameOrgDictDistrMovmnt, roadDistanceDictList, roadDistanceDistrDictList);

                Dictionary<string, List<int>> structurePartyStatusList = GetStructNameOrgIdDict(structureOrgDictCandRoute, structureOrgDictDistrMovmnt);

                Dictionary<string, List<int>> afftdPartyOrdIdStatusList = new Dictionary<string, List<int>>();

                if (currCandAffectedParty != null || currCandAffectedParty.GeneratedAffectedParties.Count != 0)
                {
                    afftdPartyOrdIdStatusList = GetAffectedPartyObjectGrouped(roadPartyStatusList, structurePartyStatusList);

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

        #region GetAffectedPartyObjectGrouped(Dictionary<string, List<int>> roadPartyStatusList, Dictionary<string, List<int>> structurePartyStatusList)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roadPartyStatusList"></param>
        /// <param name="structurePartyStatusList"></param>
        /// <returns></returns>
        public static Dictionary<string, List<int>> GetAffectedPartyObjectGrouped(Dictionary<string, List<int>> roadPartyStatusList, Dictionary<string, List<int>> structurePartyStatusList)
        {
            List<int> stillAffectedOrgId = new List<int>();
            List<int> affectedByChangeOrgId = new List<int>();
            List<int> noLongerOrgId = new List<int>();

            Dictionary<string, List<int>> afftdPartyOrdIdStatusList = new Dictionary<string, List<int>>();

            stillAffectedOrgId = roadPartyStatusList["still affected"].Union(structurePartyStatusList["still affected"]).ToList();
            affectedByChangeOrgId = roadPartyStatusList["affected by change"].Union(structurePartyStatusList["affected by change"]).ToList();
            noLongerOrgId = roadPartyStatusList["no longer affected"].Union(structurePartyStatusList["no longer affected"]).ToList();


            afftdPartyOrdIdStatusList.Add("still affected", stillAffectedOrgId);
            afftdPartyOrdIdStatusList.Add("no longer affected", noLongerOrgId);
            afftdPartyOrdIdStatusList.Add("affected by change", affectedByChangeOrgId);

            return afftdPartyOrdIdStatusList;
        }
        #endregion
        internal static List<RoadDistanceInfo> GetRoadDistanceList(AnalysedRoadsRoute analysedRoadsRoute)
        {

            try
            {
                string roadName = null;
                int distance = 0;
                List<int> OrgIdList = null;

                List<RoadDistanceInfo> roadDistList = new List<RoadDistanceInfo>();

                RoadDistanceInfo roadDistObj = null;

                foreach (AnalysedRoadsPart analysedRoadsPart in analysedRoadsRoute.AnalysedRoadsPart)
                {
                    foreach (SubPart analysedSubPart in analysedRoadsPart.SubPart)
                    {
                        foreach (List<PathRoadsPathSegment> analysedRoadsPath in analysedSubPart.Roads)
                        {
                            foreach (PathRoadsPathSegment analysedPath in analysedRoadsPath)
                            {
                                if (analysedPath.Road != null)
                                {
                                    roadName = analysedPath.Road.RoadIdentity.Name != null && analysedPath.Road.RoadIdentity.Name != "" ? analysedPath.Road.RoadIdentity.Name : analysedPath.Road.RoadIdentity.Number;

                                    OrgIdList = new List<int>();

                                    foreach (RoadResponsibleParty roadResponsibleParty in analysedPath.Road.RoadResponsibility)
                                    {
                                        OrgIdList.Add(roadResponsibleParty.OrganisationId);

                                        if (roadResponsibleParty.OnBehalfOf != null && roadResponsibleParty.OnBehalfOf.OrganisationId != 0)
                                        {
                                            OrgIdList.Add((int)roadResponsibleParty.OnBehalfOf.OrganisationId);

                                            if (roadResponsibleParty.OnBehalfOf.OnBehalfOf != null && roadResponsibleParty.OnBehalfOf.OnBehalfOf.OrganisationId != 0)
                                            {
                                                OrgIdList.Add((int)roadResponsibleParty.OnBehalfOf.OnBehalfOf.OrganisationId);
                                            }
                                        }
                                    }
                                    distance = (int)analysedPath.Road.Distance.Value;

                                    foreach (int orgId in OrgIdList.Distinct().ToList())
                                    {
                                        var generatedData = roadDistList;

                                        if ((from s in generatedData
                                             where (s.OrgId == orgId && s.RoadName == roadName)
                                             select s).ToList().Count() > 0)
                                        {
                                            (from s in generatedData
                                             where (s.OrgId == orgId && s.RoadName == roadName)
                                             select s).ToList().ForEach(s => s.Distance = s.Distance + distance);

                                            roadDistList = generatedData;
                                        }
                                        else
                                        {
                                            roadDistObj = new RoadDistanceInfo();
                                            roadDistObj.RoadName = roadName;
                                            roadDistObj.OrgId = orgId;
                                            roadDistObj.Distance = distance;
                                            roadDistList.Add(roadDistObj);
                                        }
                                    }

                                }
                            }
                        }
                    }
                }

                return roadDistList;
            }
            catch (Exception ex)
            {
                STP.Common.Logger.Logger.GetInstance().LogMessage(Common.Logger.Log_Priority.FATAL_ERROR, "Exception Occured at GetRoadDistanceList: {0}", ex.StackTrace);
                throw;
            }
        }

        #region GetRoadNameOrgIdDict(AnalysedRoadsRoute analysedRoadsRoute)
        /// <summary>
        /// Function to get Dictionary of key (road name) and value(organisation id) of affected roads
        /// </summary>
        /// <param name="analysedRoadsRoute"></param>
        /// <returns></returns>
        public static Dictionary<string, List<int>> GetRoadNameOrgIdDict(AnalysedRoadsRoute analysedRoadsRoute)
        {
            try
            {
                string roadName = null;

                Dictionary<string, List<int>> roadNameOrgDictionary = new Dictionary<string, List<int>>();

                foreach (AnalysedRoadsPart analysedRoadsPart in analysedRoadsRoute.AnalysedRoadsPart)
                {
                    foreach (SubPart analysedSubPart in analysedRoadsPart.SubPart)
                    {
                        foreach (List<PathRoadsPathSegment> analysedRoadsPath in analysedSubPart.Roads)
                        {
                            foreach (PathRoadsPathSegment analysedPath in analysedRoadsPath)
                            {
                                if (analysedPath.Road != null)
                                {
                                    roadName = analysedPath.Road.RoadIdentity.Name != null && analysedPath.Road.RoadIdentity.Name != "" ? analysedPath.Road.RoadIdentity.Name : analysedPath.Road.RoadIdentity.Number;

                                    if (!roadNameOrgDictionary.ContainsKey(roadName))
                                    {
                                        roadNameOrgDictionary.Add(roadName, new List<int>());
                                    }
                                    try
                                    {
                                        foreach (RoadResponsibleParty roadResponsibleParty in analysedPath.Road.RoadResponsibility)
                                        {

                                            roadNameOrgDictionary[roadName].Add(roadResponsibleParty.OrganisationId);

                                            if (roadResponsibleParty.OnBehalfOf != null && roadResponsibleParty.OnBehalfOf.OrganisationId != 0)
                                            {
                                                roadNameOrgDictionary[roadName].Add((int)roadResponsibleParty.OnBehalfOf.OrganisationId);

                                                if (roadResponsibleParty.OnBehalfOf.OnBehalfOf != null && roadResponsibleParty.OnBehalfOf.OnBehalfOf.OrganisationId != 0)
                                                {
                                                    roadNameOrgDictionary[roadName].Add((int)roadResponsibleParty.OnBehalfOf.OnBehalfOf.OrganisationId);
                                                }

                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        STP.Common.Logger.Logger.GetInstance().LogMessage(Common.Logger.Log_Priority.FATAL_ERROR, "Exception Occured affected party generation: {0}", ex.StackTrace);
                                        throw;
                                    }
                                }
                            }
                        }
                    }
                }

                return roadNameOrgDictionary;
            }
            catch (Exception ex)
            {
                STP.Common.Logger.Logger.GetInstance().LogMessage(Common.Logger.Log_Priority.FATAL_ERROR, "Exception Occured affected party generation: {0}", ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region GetStructureNameOrgIdDict(AnalysedStructures analysedStructures)
        /// <summary>
        /// Function to get Dictionary of key (structure code) and value(organisation id) of affected structures
        /// </summary>
        /// <param name="analysedStructures"></param>
        /// <returns></returns>
        public static Dictionary<string, List<int>> GetStructureNameOrgIdDict(AnalysedStructures analysedStructures)
        {
            try
            {
                string structureCode = null;
                Dictionary<string, List<int>> structureNameOrgDictionary = new Dictionary<string, List<int>>();
                foreach (AnalysedStructuresPart analysedStructurePart in analysedStructures.AnalysedStructuresPart)
                {
                    foreach (Structure structure in analysedStructurePart.Structure)
                    {
                        structureCode = structure.ESRN;

                        if (!structureNameOrgDictionary.ContainsKey(structureCode))
                        {
                            structureNameOrgDictionary.Add(structureCode, new List<int>());
                        }

                        try
                        {
                            foreach (StructureResponsibleParty structResponsibleParty in structure.StructureResponsibility.StructureResponsibleParty)
                            {
                                structureNameOrgDictionary[structureCode].Add(structResponsibleParty.OrganisationId);

                                if (structResponsibleParty.StructureResponsiblePartyOnBehalfOf != null && structResponsibleParty.StructureResponsiblePartyOnBehalfOf.OrganisationId != 0)
                                {
                                    structureNameOrgDictionary[structureCode].Add(structResponsibleParty.StructureResponsiblePartyOnBehalfOf.OrganisationId);
                                    if (structResponsibleParty.StructureResponsiblePartyOnBehalfOf.OnBehalfOfOnBehalfOf != null && structResponsibleParty.StructureResponsiblePartyOnBehalfOf.OnBehalfOfOnBehalfOf.OrganisationId != 0)
                                    {
                                        structureNameOrgDictionary[structureCode].Add(structResponsibleParty.StructureResponsiblePartyOnBehalfOf.OnBehalfOfOnBehalfOf.OrganisationId);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            STP.Common.Logger.Logger.GetInstance().LogMessage(Common.Logger.Log_Priority.FATAL_ERROR, "Exception Occured affected party generation: {0}", ex.StackTrace);
                            throw;
                        }
                    }
                }

                return structureNameOrgDictionary;
            }
            catch (Exception ex)
            {
                STP.Common.Logger.Logger.GetInstance().LogMessage(Common.Logger.Log_Priority.FATAL_ERROR, "Exception Occured affected party generation: {0}", ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region GetRoadNameOrgIdDict(Dictionary<string, List<int>> roadNameOrgDictCandRoute, Dictionary<string, List<int>> roadNameOrgDictDistrMovmnt)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roadNameOrgDictCandRoute"></param>
        /// <param name="roadNameOrgDictDistrMovmnt"></param>
        /// <returns></returns>
        public static Dictionary<string, List<int>> GetRoadNameOrgIdDict(Dictionary<string, List<int>> roadNameOrgDictCandRoute, Dictionary<string, List<int>> roadNameOrgDictDistrMovmnt, List<RoadDistanceInfo> roadDistanceCandTable, List<RoadDistanceInfo> roadDistanceDistrTable)
        {
            Dictionary<string, List<int>> orgIdListAffectedStatus = new Dictionary<string, List<int>>();
            List<int> stillAffectedOrgId = new List<int>();
            List<int> affectedByChangeOrgId = new List<int>();
            List<int> noLongerOrgId = new List<int>();

            List<int> tempOrgList = null;

            #region Redmine 4607
            Dictionary<int, List<string>> orgIdDictWithNameListForDistr = SortOrgIdWithANameList(roadNameOrgDictDistrMovmnt);
            Dictionary<int, List<string>> orgIdDictWithNameListForCand = SortOrgIdWithANameList(roadNameOrgDictCandRoute);
            #endregion

            foreach (string roadName in roadNameOrgDictCandRoute.Keys) // road name list of affected candidate roads current list
            {
                tempOrgList = new List<int>();

                if (roadNameOrgDictDistrMovmnt.ContainsKey(roadName)) //if the candidate road name is present in distributed road name list then
                {
                    foreach (int orgId in roadNameOrgDictDistrMovmnt[roadName]) //foreach org id in for the given road name in distributed movement
                    {
                        if (roadNameOrgDictCandRoute[roadName].Exists(x => x == orgId)) // check whether its present in candidate route.
                        {
                            //#Redmine 4607
                            if (orgIdDictWithNameListForCand[orgId].Count() == orgIdDictWithNameListForDistr[orgId].Count()) //if the number of roads count is equal in both candidate and movement version 
                            {
                                if (CheckDifferentNameCount(orgIdDictWithNameListForDistr, orgIdDictWithNameListForCand, orgId)) // even if the count is same but candidate org have a new different road then 
                                {
                                    //checking difference in distances of similar road's within an organisations.
                                    if (!CheckDifferenceInDistanceOfSimilarRoads(roadDistanceCandTable, roadDistanceDistrTable, orgId, roadName))
                                    {
                                        tempOrgList.Add(orgId); //if its present its considered as still affected and maintained in a temporary organisation list.
                                    }
                                }
                            }
                            //#Redmine 4607
                        }
                    }
                    //the remaining organisations which may be new to the already affected road will be considered as affected by change, and these will be added to affected by change org id list.
                    foreach (int orgId in roadNameOrgDictCandRoute[roadName]) //foreach org id in for the given road name in candidate route
                    {
                        if (!tempOrgList.Exists(x => x == orgId))
                        {
                            affectedByChangeOrgId.Add(orgId);
                        }
                    }
                    stillAffectedOrgId.AddRange(tempOrgList); //if its present its considered as still affected.
                }
                else if (!roadNameOrgDictDistrMovmnt.ContainsKey(roadName)) // if the provided road name is not present in distributed movement affected road then
                {
                    foreach (int orgId in roadNameOrgDictCandRoute[roadName]) //foreach org id in for the given road name in candidate road
                    {
                        affectedByChangeOrgId.Add(orgId);
                    }
                }
            }

            foreach (string roadName in roadNameOrgDictDistrMovmnt.Keys)
            {
                if (!roadNameOrgDictCandRoute.ContainsKey(roadName))
                {
                    foreach (int orgId in roadNameOrgDictDistrMovmnt[roadName]) //foreach org id in for the given road name in candidate road
                    {
                        if (!stillAffectedOrgId.Contains(orgId) && !affectedByChangeOrgId.Contains(orgId)) // if the organisations are not present in still affected or affected by change list of organisations then
                        {
                            noLongerOrgId.Add(orgId);
                        }
                    }
                }
            }

            orgIdListAffectedStatus.Add("still affected", stillAffectedOrgId.Distinct().ToList());
            orgIdListAffectedStatus.Add("no longer affected", noLongerOrgId.Distinct().ToList());
            orgIdListAffectedStatus.Add("affected by change", affectedByChangeOrgId.Distinct().ToList());

            return orgIdListAffectedStatus;
        }

        private static bool CheckDifferenceInDistanceOfSimilarRoads(List<RoadDistanceInfo> roadDistanceCandTable, List<RoadDistanceInfo> roadDistanceDistrTable, int orgId, string roadName)
        {
            int distDistr = 0, distCand = 0;
            bool affctdByChange = false;
            var candidateRouteList = from rdInfo in roadDistanceCandTable where rdInfo.OrgId == orgId && rdInfo.RoadName == roadName select rdInfo;
            var distributedRouteList = from rdInfo in roadDistanceDistrTable where rdInfo.OrgId == orgId && rdInfo.RoadName == roadName select rdInfo;

            foreach (var item in candidateRouteList)
            {
                distCand = item.Distance;

                foreach (var ite in distributedRouteList)
                {
                    distDistr = ite.Distance;
                    if (distCand != distDistr)
                    {
                        affctdByChange = true;
                    }
                }
            }
            return affctdByChange;
        }

        #endregion

        #region GetStructNameOrgIdDict(Dictionary<string, List<int>> structureOrgDictCandRoute, Dictionary<string, List<int>> structureOrgDictDistrMovmnt)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="structureOrgDictCandRoute"></param>
        /// <param name="structureOrgDictDistrMovmnt"></param>
        /// <returns></returns>
        public static Dictionary<string, List<int>> GetStructNameOrgIdDict(Dictionary<string, List<int>> structureOrgDictCandRoute, Dictionary<string, List<int>> structureOrgDictDistrMovmnt)
        {
            Dictionary<string, List<int>> orgIdListAffectedStatus = new Dictionary<string, List<int>>();
            List<int> stillAffectedOrgId = new List<int>();
            List<int> affectedByChangeOrgId = new List<int>();
            List<int> noLongerOrgId = new List<int>();

            List<int> tempOrgList = null;

            #region Redmine 4607
            Dictionary<int, List<string>> orgIdDictWithNameListForDistr = SortOrgIdWithANameList(structureOrgDictDistrMovmnt);
            Dictionary<int, List<string>> orgIdDictWithNameListForCand = SortOrgIdWithANameList(structureOrgDictCandRoute);
            #endregion

            foreach (string ESRN in structureOrgDictCandRoute.Keys) // road name list of affected candidate affected structure current list
            {
                tempOrgList = new List<int>();

                if (structureOrgDictDistrMovmnt.ContainsKey(ESRN)) //if the candidate road name is present in distributed affected structure list then
                {
                    foreach (int orgId in structureOrgDictDistrMovmnt[ESRN]) //foreach org id in for the given affected structure
                    {
                        if (structureOrgDictCandRoute[ESRN].Exists(x => x == orgId))
                        {
                            //#Redmine 4607
                            if (orgIdDictWithNameListForCand[orgId].Count() == orgIdDictWithNameListForDistr[orgId].Count()) //if the number of roads count is equal in both candidate and movement version 
                            {
                                if (CheckDifferentNameCount(orgIdDictWithNameListForDistr, orgIdDictWithNameListForCand, orgId)) // even if the count is same but candidate org have a new different road then 
                                {
                                    tempOrgList.Add(orgId); //if its present its considered as still affected and maintained in a temporary organisation list.
                                }
                            }
                            //#Redmine 4607
                        }
                    }

                    foreach (int orgId in structureOrgDictCandRoute[ESRN])
                    {
                        if (!tempOrgList.Exists(x => x == orgId))
                        {
                            affectedByChangeOrgId.Add(orgId);
                        }
                    }
                    stillAffectedOrgId.AddRange(tempOrgList);
                }
                else if (!structureOrgDictDistrMovmnt.ContainsKey(ESRN)) //if the candidate structure name is not present in distributed affected structure list then
                {
                    foreach (int orgId in structureOrgDictCandRoute[ESRN]) //foreach org id in for the given affected structure in candidate road
                    {
                        affectedByChangeOrgId.Add(orgId);
                    }
                }
            }

            foreach (string ESRN in structureOrgDictDistrMovmnt.Keys)
            {
                if (!structureOrgDictCandRoute.ContainsKey(ESRN))
                {
                    foreach (int orgId in structureOrgDictDistrMovmnt[ESRN]) //foreach org id in for the given structure code in candidate road
                    {
                        if (!stillAffectedOrgId.Contains(orgId) && !affectedByChangeOrgId.Contains(orgId))
                        {
                            noLongerOrgId.Add(orgId);
                        }
                    }
                }
            }

            orgIdListAffectedStatus.Add("still affected", stillAffectedOrgId.Distinct().ToList());
            orgIdListAffectedStatus.Add("no longer affected", noLongerOrgId.Distinct().ToList());
            orgIdListAffectedStatus.Add("affected by change", affectedByChangeOrgId.Distinct().ToList());

            return orgIdListAffectedStatus;
        }
        #endregion
        private static bool CheckDifferentNameCount(Dictionary<int, List<string>> orgIdDictWithNameListForDistr, Dictionary<int, List<string>> orgIdDictWithNameListForCand, int orgId)
        {
            return orgIdDictWithNameListForCand[orgId].Except(orgIdDictWithNameListForDistr[orgId]).ToList().Count() == 0;
        }
        private static Dictionary<int, List<string>> SortOrgIdWithANameList(Dictionary<string, List<int>> orgIdListAsNamedDict)
        {

            Dictionary<int, List<string>> tmpListForOrgId = new Dictionary<int, List<string>>();
            Dictionary<int, List<string>> OrgIdNamedDict = new Dictionary<int, List<string>>();

            foreach (string name in orgIdListAsNamedDict.Keys)
            {

                foreach (int id in orgIdListAsNamedDict[name])
                {
                    if (!tmpListForOrgId.ContainsKey(id))
                    {
                        tmpListForOrgId.Add(id, new List<string>());
                        tmpListForOrgId[id].Add(name);
                    }
                    else
                    {
                        tmpListForOrgId[id].Add(name);
                    }
                }
            }

            foreach (int id in tmpListForOrgId.Keys)
            {
                if (!OrgIdNamedDict.ContainsKey(id))
                {
                    OrgIdNamedDict.Add(id, new List<string>());
                    OrgIdNamedDict[id] = tmpListForOrgId[id].Distinct().ToList();
                }
            }
            return OrgIdNamedDict;
        }

        #region CompareWithDistrMovementToFindNewlyAffected(AffectedPartiesStructure currCandAffectedParty, AffectedPartiesStructure distributedPartiesStructure)
        /// <summary>
        /// function to compare previously distributed movement version and current candidate route version to find out a newly affected parties.
        /// </summary>
        /// <param name="currCandAffectedParty"></param>
        /// <param name="affectedPartiesStructure"></param>
        public static AffectedPartiesStructure CompareWithDistrMovementToFindNewlyAffected(AffectedPartiesStructure currCandAffectedParty, AffectedPartiesStructure distributedPartiesStructure)
        {
            var generatedData = currCandAffectedParty.GeneratedAffectedParties;
            int orgId = 0;
            foreach (AffectedPartyStructure afftdPartyObj in currCandAffectedParty.GeneratedAffectedParties)
            {
                orgId = afftdPartyObj.Contact.Contact.simpleContactRef.OrganisationId; // getting orgId from currently generated affected party list

                if (!distributedPartiesStructure.GeneratedAffectedParties.Exists(x => x.Contact.Contact.simpleContactRef.OrganisationId == orgId)) //check whether the organisation id is not present in distributed routes affected party
                {
                    (from s in generatedData
                     where (s.Contact.Contact.simpleContactRef.OrganisationId == orgId)
                     select s).ToList().ForEach(s => s.Reason = AffectedPartyReasonType.newlyaffected);

                    (from s in generatedData
                     where (s.Contact.Contact.simpleContactRef.OrganisationId == orgId)
                     select s).ToList().ForEach(s => s.ReasonSpecified = true);

                    (from s in generatedData
                     where (s.Contact.Contact.simpleContactRef.OrganisationId == orgId)
                     select s).ToList().ForEach(s => s.ExclusionOutcome = AffectedPartyReasonExclusionOutcomeType.newlyaffected);

                    (from s in generatedData
                     where (s.Contact.Contact.simpleContactRef.OrganisationId == orgId)
                     select s).ToList().ForEach(s => s.ExclusionOutcomeSpecified = true);
                }
            }
            currCandAffectedParty.GeneratedAffectedParties = generatedData;
            return currCandAffectedParty;
        }
        #endregion

        /// <summary>
        ///  #5171 : SORT: Parties which are no longer affected should be removed from the affected parties page for subsequent versions
        /// </summary>
        /// <param name="currCandAffectedParty"></param>
        /// <param name="distributedPartiesStructure"></param>
        /// <returns></returns>
        internal static AffectedPartiesStructure CompareAndRemoveNoLongerAffectedFromSubsequentVersions(AffectedPartiesStructure currCandAffectedParty, AffectedPartiesStructure distributedPartiesStructure)
        {
            int orgId = 0;

            var generatedAffectedParty = currCandAffectedParty;

            try
            {
                STP.Common.Logger.Logger.GetInstance().LogMessage(Common.Logger.Log_Priority.INFORMATIONAL, "Removing no longer affected from subsequent version");

                for (int i = currCandAffectedParty.GeneratedAffectedParties.Count - 1; i >= 0; i--)
                {
                    AffectedPartyStructure afftdPartyObj = currCandAffectedParty.GeneratedAffectedParties[i];

                    orgId = afftdPartyObj.Contact.Contact.simpleContactRef.OrganisationId; // getting orgId from currently generated affected party list

                    if (distributedPartiesStructure.GeneratedAffectedParties.Exists(x => x.Contact.Contact.simpleContactRef.OrganisationId == orgId && x.Reason == AffectedPartyReasonType.nolongeraffected)) //check whether the organisation id is not present in distributed routes affected party
                    {
                        if (generatedAffectedParty.GeneratedAffectedParties.Exists(x => x.Contact.Contact.simpleContactRef.OrganisationId == orgId && x.Reason == AffectedPartyReasonType.nolongeraffected))
                        {
                            currCandAffectedParty.GeneratedAffectedParties.Remove(afftdPartyObj); // removing the affected parties object that is matching and the reason is not "no longer affected"
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                STP.Common.Logger.Logger.GetInstance().LogMessage(Common.Logger.Log_Priority.FATAL_ERROR, "Exception Occured at CompareAndRemoveNoLongerAffectedFromSubsequentVersions: {0}", ex.StackTrace);
                currCandAffectedParty = generatedAffectedParty;
            }
            return currCandAffectedParty;
        }

        #region xmlAffectedPartyDeserializer(string xml, List<AssessmentContacts> manualContactList)
        /// <summary>
        /// Function to deserialize the Affected Parties xml Data
        /// </summary>
        public static string XmlAffectedPartyDeserializer(string xml, List<AssessmentContacts> manualContactList, bool isHaullier = false)
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
                    XmlData = new AffectedPartiesStructure
                    {
                        GeneratedAffectedParties = new List<AffectedPartyStructure>()
                    };
                }
            }

            if (XmlData.ManualAffectedParties == null)
                XmlData.ManualAffectedParties = new List<AffectedPartyStructure>();

            foreach (AssessmentContacts manualContact in manualContactList)
            {

                AffectedPartyStructure manualAffectedParty = new AffectedPartyStructure
                {
                    Exclude = false,
                    ExclusionOutcome = AffectedPartyReasonExclusionOutcomeType.newlyaffected,
                    ExclusionOutcomeSpecified = true,
                    IsPolice = manualContact.OrganisationType == "police",
                    IsRetainedNotificationOnly = false,
                    Reason = AffectedPartyReasonType.newlyaffected,
                    ReasonSpecified = true,
                    Contact = new RouteAssessment.XmlAffectedParties.ContactReferenceStructure
                    {
                        Contact = new RouteAssessment.XmlAffectedParties.ContactReferenceChoiceStructure
                        {
                            adhocContactRef = new RouteAssessment.XmlAffectedParties.AdhocContactReferenceStructure
                            {
                                FullName = manualContact.ContactName,
                                OrganisationName = manualContact.OrganisationName,
                                FaxNumber = manualContact.Fax,
                                EmailAddress = manualContact.Email
                            }
                        }
                    }
                };

                count = CheckForDuplicateCont(manualAffectedParty, XmlData);
                if (count == 0)
                {
                    if (isHaullier)
                        XmlData.ManualAffectedParties.Insert(0, manualAffectedParty);
                    else
                        XmlData.ManualAffectedParties.Add(manualAffectedParty);
                }
            }

            if (count == 0 || count == 2)
            {
                string newXml = XmlAffectedPartySerializer(XmlData);
                xmlReader.Close();
                return newXml;
            }
            else
                return "Contact already exist";
        }
        #endregion

        #region checkForDuplicateCont(AffectedPartyStructure manualAffectedParty, AffectedPartiesStructure XmlData)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manualAffectedParty"></param>
        /// <param name="XmlData"></param>
        /// <returns></returns>
        private static int CheckForDuplicateCont(AffectedPartyStructure manualAffectedParty, AffectedPartiesStructure XmlData)
        {
            int count = 0;

            string tmpNewFullName = manualAffectedParty.Contact.Contact.adhocContactRef.FullName;
            string tmpNewOrgName = manualAffectedParty.Contact.Contact.adhocContactRef.OrganisationName;
            string tmpNewEmail = manualAffectedParty.Contact.Contact.adhocContactRef.EmailAddress;

            string tmpFullName, tmpOrgName = null, tmpEmail = null;

            foreach (var item in XmlData.ManualAffectedParties.Select(x => x.Contact.Contact).ToList())
            {
                try
                {
                    tmpFullName = item.adhocContactRef.FullName;
                    tmpOrgName = item.adhocContactRef.OrganisationName;
                    tmpEmail = item.adhocContactRef.EmailAddress;
                }
                catch
                {
                    tmpFullName = item.simpleContactRef.FullName;
                    tmpFullName = item.simpleContactRef.FullName;
                }
                if (tmpFullName == tmpNewFullName && tmpOrgName == tmpNewOrgName)
                {
                    count++;
                    if (tmpEmail != tmpNewEmail)
                    {
                        count++;
                        item.adhocContactRef.EmailAddress = tmpNewEmail;
                    }
                }
            }
            return count;
        }
        #endregion

        public static string ModifyingDispensationStatusToInUse(string xmlAffectedParties, string orgName, int orgId)
        {
            var newAffectedParty = xmlAffectedPartyDeserializer(xmlAffectedParties);
            if (newAffectedParty.GeneratedAffectedParties.Exists(x => x.Contact.Contact.simpleContactRef.OrganisationId == orgId)
                && newAffectedParty.GeneratedAffectedParties.Exists(x => x.Contact.Contact.simpleContactRef.OrganisationName == orgName))
            {
                var generatedData = newAffectedParty.GeneratedAffectedParties;

                (from s in generatedData
                 where s.Contact.Contact.simpleContactRef.OrganisationId == orgId && s.Contact.Contact.simpleContactRef.OrganisationName == orgName
                 select s).ToList().ForEach(s => s.DispensationStatus = DispensationStatusType.inuse);

                newAffectedParty.GeneratedAffectedParties = generatedData;
            }
            string updatedXmlAffectedParty = XmlAffectedPartySerializer(newAffectedParty);
            return updatedXmlAffectedParty;
        }
        public static decimal ConvertExponentialValueToDecimal(double val)
        {
            try
            {
                decimal value = decimal.Parse(Convert.ToString(val), System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowExponent |
                    System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowLeadingSign,
                    System.Globalization.CultureInfo.InvariantCulture);
                return value;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ConvertExponentialValueToDecimal : Exception: { ex.Message } StackTrace { ex.StackTrace}");
                return 0;
            }
        }
        public static string Base64Encode(string plainText)
        {
            try
            {
                if (plainText != RouteExportError.ExportError)
                {
                    var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                    return Convert.ToBase64String(plainTextBytes);
                }
                else
                    return plainText;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Base64Encode : Exception: { ex.Message } StackTrace { ex.StackTrace}");
                return string.Empty;
            }
        }
        public static string Base64Decode(string base64EncodedData)
        {
            try
            {
                var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
                return Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Base64Decode : Exception: { ex.Message } StackTrace { ex.StackTrace}");
                return string.Empty;
            }
        }
    }
}
