using AggreedRouteXSD;
using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.DocumentsAndContents.Common;
using STP.Domain.DocumentsAndContents;
using STP.Domain.RouteAssessment;
using STP.Domain.SecurityAndUsers;
using STP.Domain.VehiclesAndFleets;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace STP.DocumentsAndContents.Persistance
{
    public static class RevisedAgreementDAO
    {

        public static AggreedRouteXSD.PredefinedCautionsDescriptionsStructure1 Pcds { get; set; }


        public static string TotalDistanceMetric { get; set; }

        public static string TotalDistanceImperial { get; set; }

        public static decimal CheckTotalDistanceMetric { get; set; }

        public static decimal CheckTotalDistanceImperial { get; set; }
        /// <summary>
        /// get revised agreement detail
        /// </summary>
        /// <param name="orderNumber">order number</param>
        /// <param name="esDAlRefNo">esdal ref no</param>
        /// <param name="contactId">contact id</param>
        /// <returns></returns>
        public static AgreedRouteStructure GetRevisedAgreementDetails(string orderNumber, string esDAlRefNo, int contactId, string userSchema = UserSchema.Sort)
        {
            int version_no = 0;
            esDAlRefNo = esDAlRefNo.Replace("-", "/S");

            string[] esdalRefNumber = esDAlRefNo.Split('/');

            string haulierMnemonic = string.Empty;
            string projectNumber = string.Empty;
            string versionNumber = string.Empty;

            if (esdalRefNumber.Length > 0)
            {
                haulierMnemonic = esdalRefNumber[0];
                projectNumber = esdalRefNumber[1].Replace("S", "");
                versionNumber = esdalRefNumber[2].Replace("S", "");
            }

            AgreedRouteStructure ps = new AgreedRouteStructure();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                ps,
                userSchema + ".GET_REVISE_AGREEMENT_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("p_ORDER_NUMBER", string.Empty, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_HAULIER_MNEMONIC", haulierMnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PROJECT_NUMBER", projectNumber, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VERSION_NUMBER", versionNumber, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       orderNumber = records.GetStringOrDefault("ORDER_NO");

                       ps.IsFailedDelegationAlert = true;

                       AggreedRouteXSD.ESDALReferenceNumberStructure erns = new AggreedRouteXSD.ESDALReferenceNumberStructure();

                       erns.Mnemonic = records.GetStringOrDefault("HAULIER_MNEMONIC");

                       string[] hauMnemonicArr = null;

                       if (orderNumber == string.Empty)
                       {
                           hauMnemonicArr = esDAlRefNo.Split("/".ToCharArray());
                           version_no = Convert.ToInt32(versionNumber);
                       }
                       else
                       {
                           hauMnemonicArr = records.GetStringOrDefault("ESDAL_REF").Split("/".ToCharArray());
                       }

                       if (hauMnemonicArr.Length > 0)
                       {
                           erns.MovementProjectNumber = hauMnemonicArr[1];

                           if (hauMnemonicArr[2] != null)
                               version_no = Convert.ToInt32(hauMnemonicArr[2].Replace("S", ""));
                       }

                       AggreedRouteXSD.MovementVersionNumberStructure mvns = new AggreedRouteXSD.MovementVersionNumberStructure();
                       mvns.Value = records.GetShortOrDefault("Version_No");
                       erns.MovementVersion = mvns;

                       instance.ESDALReferenceNumber = erns;

                       instance.JobFileReference = records.GetStringOrDefault("HA_JOB_FILE_REF");

                       var sentDateTime = records.GetDateTimeOrEmpty("distributed_date");
                       instance.SentDateTime = sentDateTime == null || sentDateTime == DateTime.MinValue ? DateTime.Now : sentDateTime;//DateTime.Now;

                       #region HAContact Detail
                       instance.HAContact = GetHAContactDetails(contactId);
                           #endregion

                           #region Recipients
                           instance.Recipients = GetRecipientContactStructure(orderNumber, haulierMnemonic, projectNumber, versionNumber, userSchema).ToArray();
                           #endregion

                           #region HaulierDetails
                           AggreedRouteXSD.HaulierDetailsStructure hds = new AggreedRouteXSD.HaulierDetailsStructure();
                       hds.OrganisationId = Convert.ToInt32(records.GetLongOrDefault("organisation_id"));
                       hds.OrganisationIdSpecified = Convert.ToString(records.GetLongOrDefault("organisation_id")) != string.Empty;
                       hds.HaulierContact = records.GetStringOrDefault("HAULIER_CONTACT");
                       hds.HaulierName = records.GetStringOrDefault("HAULIER_NAME");

                       AggreedRouteXSD.AddressStructure AddStruHaulier = new AggreedRouteXSD.AddressStructure();
                       string[] AddHaulier = new string[5];

                       AddHaulier[0] = records.GetStringOrDefault("HAULIER_ADDRESS_1");
                       AddHaulier[1] = records.GetStringOrDefault("HAULIER_ADDRESS_2");
                       AddHaulier[2] = records.GetStringOrDefault("HAULIER_ADDRESS_3");
                       AddHaulier[3] = records.GetStringOrDefault("HAULIER_ADDRESS_4");
                       AddHaulier[4] = records.GetStringOrDefault("HAULIER_ADDRESS_5");

                       AddStruHaulier.Line = AddHaulier;
                       AddStruHaulier.PostCode = records.GetStringOrDefault("haulier_post_code");
                       AddStruHaulier.Licence = records.GetStringOrDefault("HAULIER_LICENCE_NO");
                       int country = records.GetInt32OrDefault("haulier_country");
                       if (country == (int)Country.england)
                       {
                           AddStruHaulier.Country = AggreedRouteXSD.CountryType.england;
                           AddStruHaulier.CountrySpecified = true;
                       }
                       else if (country == (int)Country.northernireland)
                       {
                           AddStruHaulier.Country = AggreedRouteXSD.CountryType.northernireland;
                           AddStruHaulier.CountrySpecified = true;
                       }
                       else if (country == (int)Country.scotland)
                       {
                           AddStruHaulier.Country = AggreedRouteXSD.CountryType.scotland;
                           AddStruHaulier.CountrySpecified = true;
                       }
                       else if (country == (int)Country.wales)
                       {
                           AddStruHaulier.Country = AggreedRouteXSD.CountryType.wales;
                           AddStruHaulier.CountrySpecified = true;
                       }
                       AddStruHaulier.CountrySpecified = true;
                       hds.HaulierAddress = AddStruHaulier;
                       hds.TelephoneNumber = records.GetStringOrDefault("HAULIER_TEL_NO");
                       hds.FaxNumber = records.GetStringOrDefault("HAULIER_FAX_NO");
                       hds.EmailAddress = records.GetStringOrDefault("HAULIER_EMAIL");
                       instance.HaulierDetails = hds;

                       ps.HauliersReference = records.GetStringOrDefault("HAULIERS_REF");

                           #endregion

                           #region JourneyFromToSummary
                           JourneyFromToSummaryStructure jftss = new JourneyFromToSummaryStructure();

                       PlannedRoadRoutePartStructureStartPointListPosition prrpssplp = new PlannedRoadRoutePartStructureStartPointListPosition();

                       prrpssplp.StartPoint = GetSimplifiedRoutePointStart(orderNumber, haulierMnemonic, projectNumber, versionNumber, userSchema);

                       jftss.From = prrpssplp.StartPoint.Description;

                       PlannedRoadRoutePartStructureEndPointListPosition prrpseplp = new PlannedRoadRoutePartStructureEndPointListPosition();
                       prrpseplp.EndPoint = GetSimplifiedRoutePointEnd(orderNumber, haulierMnemonic, projectNumber, versionNumber, userSchema);

                       jftss.To = prrpseplp.EndPoint.Description;

                       instance.JourneyFromToSummary = jftss;
                           #endregion

                           #region JourneyFromTo
                           JourneyFromToStructure jfts = new JourneyFromToStructure();
                       jfts.From = records.GetStringOrDefault("FROM_DESCR");
                       jfts.To = records.GetStringOrDefault("TO_DESCR");
                       instance.JourneyFromTo = jfts;
                           #endregion

                           #region JourneyTiming
                           AggreedRouteXSD.JourneyDateStructure jds = new AggreedRouteXSD.JourneyDateStructure();
                       jds.FirstMoveDate = records.GetDateTimeOrDefault("MOVEMENT_START_DATE");
                       jds.LastMoveDate = records.GetDateTimeOrDefault("MOVEMENT_END_DATE");
                       jds.LastMoveDateSpecified = true;
                       instance.JourneyTiming = jds;
                           #endregion

                           instance.LoadSummary = "";

                           #region LoadDetail
                           LoadDetailsStructure lds = new LoadDetailsStructure();
                       lds.Description = records.GetStringOrDefault("LOAD_DESCR");
                       lds.TotalMoves = records.GetShortOrDefault("TOTAL_MOVES");
                       lds.MaxPiecesPerMove = records.GetShortOrDefault("MAX_PARTS_PER_MOVE");
                       lds.MaxPiecesPerMoveSpecified = true;
                       instance.LoadDetails = lds;
                           #endregion

                           #region RouteParts

                           instance.RouteParts = GetRouteParts(orderNumber, esDAlRefNo, version_no, haulierMnemonic, projectNumber, userSchema);

                       if (instance.RouteParts != null)
                           instance.RouteParts = instance.RouteParts.Where(x => x.RoutePart != null).ToArray();

                       #endregion

                       instance.PredefinedCautions = Pcds;

                           #region OrderSummary
                           instance.OrderSummary = GetSODetails(esDAlRefNo, userSchema);
                           #endregion

                           int status = 0;

                       if (orderNumber != string.Empty)
                       {
                           status = records.GetInt32OrDefault("STATE");
                       }

                       if (status == 258001)
                       {
                           instance.Status = AgreedRouteStatusType.original;
                       }
                       else if (status == 258002)
                       {
                           instance.Status = AgreedRouteStatusType.reclearance;
                       }
                       else if (status == 258003)
                       {
                           instance.Status = AgreedRouteStatusType.recleared;
                       }

                       instance.VersionID = records.GetLongOrDefault("VERSION_ID");
                       instance.ProjectID = records.GetLongOrDefault("PROJECT_ID");
                       instance.VersionStatus = records.GetInt32OrDefault("VERSION_STATUS");

                   //get distribution comments
                   Byte[] inboundNotificationArray = records.GetByteArrayOrNull("DOCUMENT");
                       if (inboundNotificationArray != null)
                       {
                           string inboundNotification = null;
                           XmlDocument Doc = new XmlDocument();
                           try
                           {
                               inboundNotification = Encoding.UTF8.GetString(inboundNotificationArray, 0, inboundNotificationArray.Length);
                               Doc.LoadXml(inboundNotification);
                           }
                           catch (System.Xml.XmlException xmlEx)
                           {
                               inboundNotificationArray = STP.Common.General.XsltTransformer.Trafo(records.GetByteArrayOrNull("DOCUMENT"));
                               inboundNotification = Encoding.UTF8.GetString(inboundNotificationArray, 0, inboundNotificationArray.Length);
                               Doc.LoadXml(inboundNotification);
                           }

                           instance.DistributionComments = Doc.GetElementsByTagName("DistributionComments") == null ? string.Empty : (Doc.GetElementsByTagName("DistributionComments").Item(0) == null ? string.Empty : Doc.GetElementsByTagName("DistributionComments").Item(0).InnerText);
                       }

                   }
            );

            return ps;
        }

        /// <summary>
        /// get ha contact detail for revised agreement document
        /// </summary>
        /// <param name="contactId">contact id</param>
        /// <returns></returns>
        public static HAContactStructure GetHAContactDetails(int contactId)
        {
            HAContactStructure hacs = new HAContactStructure();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                hacs,
                UserSchema.Portal + ".GET_HACONTACT_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("p_ContactId", contactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 (records, instance) =>
                 {
                     string contact = string.Empty;
                     if (records.GetStringOrDefault("TITLE") != string.Empty)
                         contact = records.GetStringOrDefault("TITLE");
                     if (records.GetStringOrDefault("FIRST_NAME") != string.Empty)
                         contact = contact + " " + records.GetStringOrDefault("FIRST_NAME");
                     if (records.GetStringOrDefault("SUR_NAME") != string.Empty)
                         contact = contact + " " + records.GetStringOrDefault("SUR_NAME");

                     instance.Contact = contact;

                     AggreedRouteXSD.AddressStructure has = new AggreedRouteXSD.AddressStructure();

                     string[] Addstru = new string[5];
                     Addstru[0] = records.GetStringOrDefault("ADDRESSLINE_1");
                     Addstru[1] = records.GetStringOrDefault("ADDRESSLINE_2");
                     Addstru[2] = records.GetStringOrDefault("ADDRESSLINE_3");
                     Addstru[3] = records.GetStringOrDefault("ADDRESSLINE_4");
                     Addstru[4] = records.GetStringOrDefault("ADDRESSLINE_5");
                     has.Line = Addstru;
                     has.PostCode = records.GetStringOrDefault("POSTCODE");

                     int countrycode = Convert.ToInt32(records.GetDecimalOrDefault("COUNTRY_ID"));
                     if (countrycode == (int)Country.england)
                     {
                         has.Country = AggreedRouteXSD.CountryType.england;
                         has.CountrySpecified = true;
                     }
                     else if (countrycode == (int)Country.northernireland)
                     {
                         has.Country = AggreedRouteXSD.CountryType.northernireland;
                         has.CountrySpecified = true;
                     }
                     else if (countrycode == (int)Country.scotland)
                     {
                         has.Country = AggreedRouteXSD.CountryType.scotland;
                         has.CountrySpecified = true;
                     }
                     else if (countrycode == (int)Country.wales)
                     {
                         has.Country = AggreedRouteXSD.CountryType.wales;
                         has.CountrySpecified = true;
                     }

                     instance.Address = has;
                     instance.TelephoneNumber = records.GetStringOrDefault("PHONENUMBER");
                     instance.FaxNumber = records.GetStringOrDefault("FAX");
                     instance.EmailAddress = records.GetStringOrDefault("EMAIL");
                 }
                );

            return hacs;

        }

        /// <summary>
        /// generate recipeint node detail based on xml
        /// </summary>
        /// <param name="orderNumber">order number</param>
        /// <param name="esDAlRefNo">esdal ref no</param>
        /// <returns></returns>
        public static List<AggreedRouteXSD.RecipientContactStructure> GetRecipientContactStructure(string orderNumber, string haulierMnemonic, string projectNumber, string versionNumber, string userSchema)
        {
            userSchema = UserSchema.Sort;
            string messg = "RevisedAgreementDAO/GetAggreedRouteXSD.RecipientContactStructure?orderNumber=" + orderNumber + ", haulierMnemonic=" + haulierMnemonic + ", projectNumber=" + projectNumber + ", versionNumber=" + versionNumber + ", userSchema=" + userSchema;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            string contactInformationLog = string.Empty;

            List<AggreedRouteXSD.RecipientContactStructure> rcsclplist = new List<AggreedRouteXSD.RecipientContactStructure>();

            ContactModel contactInfo = GetRecipientDetails(orderNumber, haulierMnemonic, projectNumber, versionNumber, userSchema);

            string recipientXMLInformation = string.Empty;

            Byte[] affectedPartiesArray = contactInfo.AffectedParties;
            if (affectedPartiesArray != null)
            {
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    recipientXMLInformation = Encoding.UTF8.GetString(affectedPartiesArray, 0, affectedPartiesArray.Length);

                    xmlDoc.LoadXml(recipientXMLInformation);
                }
                catch
                {
                    //Some data is stored in gzip format, so we need to unzip then load it.
                    byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(affectedPartiesArray);

                    recipientXMLInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);

                    xmlDoc.LoadXml(recipientXMLInformation);
                }

                List<ContactModel> contactList = new List<ContactModel>();

                XmlNodeList parentNode = null;
                if (xmlDoc.GetElementsByTagName("AffectedParty").Item(0) == null)
                {
                    parentNode = xmlDoc.GetElementsByTagName("movement:AffectedParty");
                }
                else
                {
                    parentNode = xmlDoc.GetElementsByTagName("AffectedParty");
                }
                foreach (XmlElement childrenNode in parentNode)                                   //AffectedParty
                {
                    bool isPolice = false;
                    bool isRetainNotificationOnly = false;
                    string reason = string.Empty;
                    bool isHaulier = false;
                    bool isRecepient = false;

                    string exclusionOutCome = string.Empty;
                    string exclude = string.Empty;
                    if ((childrenNode != null) && childrenNode.HasAttribute("IsPolice"))
                    {
                        isPolice = Convert.ToBoolean(childrenNode.Attributes["IsPolice"].InnerText);
                    }
                    if ((childrenNode != null) && childrenNode.HasAttribute("Exclude"))
                    {
                        exclude = Convert.ToString(childrenNode.Attributes["Exclude"].InnerText);
                    }
                    else if ((childrenNode != null) && childrenNode.HasAttribute("contact:Exclude"))
                    {
                        exclude = Convert.ToString(childrenNode.Attributes["contact:Exclude"].InnerText);
                    }

                    if ((childrenNode != null) && childrenNode.HasAttribute("IsRetainedNotificationOnly"))
                    {
                        isRetainNotificationOnly = Convert.ToBoolean(childrenNode.Attributes["IsRetainedNotificationOnly"].InnerText);
                    }
                    if ((childrenNode != null) && childrenNode.HasAttribute("Reason"))
                    {
                        reason = Convert.ToString(childrenNode.Attributes["Reason"].InnerText);
                    }
                    else if ((childrenNode != null) && childrenNode.HasAttribute("contact:Reason"))
                    {
                        reason = Convert.ToString(childrenNode.Attributes["contact:Reason"].InnerText);
                    }
                    if ((childrenNode != null) && childrenNode.HasAttribute("ExclusionOutcome"))
                    {
                        exclusionOutCome = Convert.ToString(childrenNode.Attributes["ExclusionOutcome"].InnerText);
                    }
                    else if ((childrenNode != null) && childrenNode.HasAttribute("contact:ExclusionOutcome"))
                    {
                        exclusionOutCome = Convert.ToString(childrenNode.Attributes["contact:ExclusionOutcome"].InnerText);
                    }
                    if (childrenNode != null && childrenNode.HasAttribute("IsHaulier"))
                    {
                        isHaulier = Convert.ToBoolean(childrenNode.Attributes["IsHaulier"].InnerText);
                    }

                    if (childrenNode != null && childrenNode.HasAttribute("IsRecipient"))
                    {
                        isRecepient = Convert.ToBoolean(childrenNode.Attributes["IsRecipient"].InnerText);
                    }
                    if (childrenNode != null)
                    {
                        foreach (XmlElement xmlElement in childrenNode)                             //Contact
                        {
                            int DelegationId = 0;
                            int DelegatorsContactId = 0;
                            int DelegatorsOrganisationId = 0;
                            bool RetainNotification = false;
                            bool WantsFailureAlert = false;
                            string DelegatorsOrganisationName = string.Empty;
                            if (xmlElement.Name == "OnBehalfOf")
                            {
                                if ((xmlElement != null) && xmlElement.HasAttribute("DelegationId"))
                                {
                                    DelegationId = Convert.ToInt32(xmlElement.Attributes["DelegationId"].InnerText);
                                }
                                if ((xmlElement != null) && xmlElement.HasAttribute("DelegatorsContactId"))
                                {
                                    DelegatorsContactId = Convert.ToInt32(xmlElement.Attributes["DelegatorsContactId"].InnerText);
                                }
                                if ((xmlElement != null) && xmlElement.HasAttribute("DelegatorsOrganisationId"))
                                {
                                    DelegatorsOrganisationId = Convert.ToInt32(xmlElement.Attributes["DelegatorsOrganisationId"].InnerText);
                                }
                                if ((xmlElement != null) && xmlElement.HasAttribute("RetainNotification"))
                                {
                                    RetainNotification = Convert.ToBoolean(xmlElement.Attributes["RetainNotification"].InnerText);
                                }
                                if ((xmlElement != null) && xmlElement.HasAttribute("WantsFailureAlert"))
                                {
                                    WantsFailureAlert = Convert.ToBoolean(xmlElement.Attributes["WantsFailureAlert"].InnerText);
                                }
                                if (xmlElement["DelegatorsOrganisationName"] != null)
                                {
                                    DelegatorsOrganisationName = xmlElement["DelegatorsOrganisationName"].InnerText;
                                }

                                if (contactList.Count > 0)
                                {
                                    contactList[contactList.Count - 1].DelegationId = DelegationId;
                                    contactList[contactList.Count - 1].DelegatorsContactId = DelegatorsContactId;
                                    contactList[contactList.Count - 1].DelegatorsOrganisationId = DelegatorsOrganisationId;
                                    contactList[contactList.Count - 1].RetainNotification = RetainNotification;
                                    contactList[contactList.Count - 1].WantsFailureAlert = WantsFailureAlert;
                                    contactList[contactList.Count - 1].DelegatorsOrganisationName = DelegatorsOrganisationName;
                                }
                            }
                            foreach (XmlElement xmlElement1 in xmlElement.ChildNodes)               //Contact
                            {
                                foreach (XmlNode childNode in xmlElement1)                          //SimpleReference
                                {
                                    if (childNode.Name == "SimpleReference")
                                    {
                                        int contactId = 0;
                                        int orgId = 0;
                                        string contactname = string.Empty;
                                        string orgname = string.Empty;
                                        string fax = string.Empty;
                                        string email = string.Empty;
                                        XmlElement simpleReference = childNode as XmlElement;
                                        if ((simpleReference != null) && simpleReference.HasAttribute("ContactId"))
                                        {
                                            contactId = Convert.ToInt32(childNode.Attributes["ContactId"].InnerText);
                                        }
                                        if ((simpleReference != null) && simpleReference.HasAttribute("OrganisationId"))
                                        {
                                            orgId = Convert.ToInt32(childNode.Attributes["OrganisationId"].InnerText);
                                        }

                                        if (childNode["FullName"] != null)
                                        {
                                            contactname = childNode["FullName"].InnerText;
                                        }
                                        if (childNode["OrganisationName"] != null)
                                        {
                                            orgname = childNode["OrganisationName"].InnerText;
                                        }
                                        if (childNode["FaxNumber"] != null)
                                        {
                                            fax = childNode["FaxNumber"].InnerText;
                                        }
                                        if (childNode["EmailAddress"] != null)
                                        {
                                            email = childNode["EmailAddress"].InnerText;
                                        }
                                        if ((!string.IsNullOrEmpty(reason)) && (!string.IsNullOrEmpty(exclusionOutCome))
                                                   && (string.IsNullOrEmpty(exclude) || exclude.ToLower() == "false"))
                                        {
                                            contactInfo = new ContactModel()
                                            {
                                                IsRecipient = isRecepient,
                                                IsHaulier = isHaulier,
                                                ContactId = contactId,
                                                OrganisationId = orgId,
                                                Fax = fax,
                                                Email = email,
                                                ISPolice = isPolice,
                                                IsRetainedNotificationOnly = isRetainNotificationOnly,
                                                Reason = reason,
                                                FullName = contactname,
                                                Organisation = orgname,
                                            };

                                            contactList.Add(contactInfo);

                                            contactInformationLog = "RevisedAgreementDAO/GetRecipientDetails?IsRecipient=" + contactInfo.IsRecipient + ", IsHaulier=" + contactInfo.IsHaulier + ", ContactId=" + contactInfo.ContactId + ", organisationID=" + contactInfo.OrganisationId + ", FAX=" + contactInfo.Fax + ", EMAIL=" + contactInfo.Email + ", ISPolice=" + contactInfo.ISPolice + ", IsRetainedNotificationOnly=" + contactInfo.IsRetainedNotificationOnly + ", Reason=" + contactInfo.Reason + ", FullName=" + contactInfo.FullName + ", ORGANISATION=" + contactInfo.Organisation;
                                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(contactInformationLog + "; SimpleReference contact Info details"));
                                        }
                                    }
                                    else if (childNode.Name.Contains("AdhocReference"))
                                    {

                                        string contactname = string.Empty;
                                        string orgname = string.Empty;
                                        string email = string.Empty;
                                        string fax = string.Empty;

                                        if (childNode["contact:FullName"] != null)
                                        {
                                            contactname = childNode["contact:FullName"].InnerText;
                                        }
                                        else
                                        {
                                            contactname = childNode["FullName"] != null ? childNode["FullName"].InnerText : null;
                                        }

                                        if (childNode["contact:EmailAddress"] != null)
                                        {
                                            email = childNode["contact:EmailAddress"].InnerText;
                                        }
                                        else
                                        {
                                            email = childNode["EmailAddress"] != null ? childNode["EmailAddress"].InnerText : null;
                                        }

                                        if (childNode["contact:OrganisationName"] != null)
                                        {
                                            orgname = childNode["contact:OrganisationName"].InnerText;
                                        }
                                        else
                                        {
                                            orgname = childNode["OrganisationName"] != null ? childNode["OrganisationName"].InnerText : null;
                                        }
                                        //here fax is also fetched
                                        if (childNode["contact:FaxNumber"] != null)
                                        {
                                            fax = childNode["contact:FaxNumber"].InnerText;
                                        }
                                        else
                                        {
                                            fax = childNode["FaxNumber"] != null ? childNode["FaxNumber"].InnerText : null;
                                        }
                                        if ((!string.IsNullOrEmpty(reason) && reason.ToLower() != "no longer affected") && (string.IsNullOrEmpty(exclusionOutCome) || exclusionOutCome.ToLower() != "no longer affected")
                                                           && (string.IsNullOrEmpty(exclude) || exclude.ToLower() == "false"))
                                        {
                                            contactInfo = new ContactModel
                                            {

                                                IsRecipient = isRecepient,
                                                IsHaulier = isHaulier,
                                                Fax = fax,
                                                Email = email,
                                                ISPolice = isPolice,
                                                IsRetainedNotificationOnly = isRetainNotificationOnly,
                                                Reason = reason,
                                                FullName = contactname,
                                                Organisation = orgname,
                                            };

                                            contactList.Add(contactInfo);

                                            contactInformationLog = "RevisedAgreementDAO/GetRecipientDetails?IsRecipient=" + contactInfo.IsRecipient + ", IsHaulier=" + contactInfo.IsHaulier + ", FAX=" + contactInfo.Fax + ", EMAIL=" + contactInfo.Email + ", ISPolice=" + contactInfo.ISPolice + ", IsRetainedNotificationOnly=" + contactInfo.IsRetainedNotificationOnly + ", Reason=" + contactInfo.Reason + ", FullName=" + contactInfo.FullName + ", ORGANISATION=" + contactInfo.Organisation;
                                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(contactInformationLog + "; Adhoc Reference contact Info details"));
                                        }
                                    }
                                }
                            }
                        }
                    }

                }

                AggreedRouteXSD.RecipientContactStructure rcs1;
                foreach (ContactModel cont in contactList)
                {
                    rcs1 = new AggreedRouteXSD.RecipientContactStructure();
                    rcs1.Reason = cont.Reason;
                    rcs1.ContactId = cont.ContactId;
                    rcs1.ContactIdSpecified = true;
                    rcs1.IsHaulier = GetOrgTypeDetails(cont.OrganisationId);
                    rcs1.IsPolice = cont.ISPolice;
                    rcs1.IsRecipient = cont.IsRecipient;
                    rcs1.IsRetainedNotificationOnly = cont.IsRetainedNotificationOnly;
                    rcs1.OrganisationId = cont.OrganisationId;
                    rcs1.OrganisationIdSpecified = true;

                    rcs1.ContactName = cont.FullName;
                    rcs1.OrganisationName = cont.Organisation;
                    rcs1.Fax = cont.Fax;
                    rcs1.Email = cont.Email;

                    AggreedRouteXSD.OnBehalfOfStructure onbehalfofstru = new AggreedRouteXSD.OnBehalfOfStructure();
                    if (cont.DelegationId > 0)
                    {
                        onbehalfofstru.DelegationId = cont.DelegationId;
                        onbehalfofstru.DelegationIdSpecified = true;
                        onbehalfofstru.DelegatorsContactId = cont.DelegatorsContactId;
                        onbehalfofstru.DelegatorsContactIdSpecified = true;
                        onbehalfofstru.DelegatorsOrganisationId = cont.DelegatorsOrganisationId;
                        onbehalfofstru.DelegatorsOrganisationIdSpecified = true;
                        onbehalfofstru.DelegatorsOrganisationName = cont.DelegatorsOrganisationName;
                        onbehalfofstru.RetainNotification = cont.RetainNotification;
                        onbehalfofstru.WantsFailureAlert = cont.WantsFailureAlert;

                        rcs1.OnbehalfOf = onbehalfofstru;
                    }

                    rcsclplist.Add(rcs1);
                }
            }
            return rcsclplist;
        }

        /// <summary>
        /// get recipient xml from the database
        /// </summary>
        /// <param name="orderNumber">order number</param>
        /// <param name="esDAlRefNo">esdal ref no</param>
        /// <returns></returns>
        public static ContactModel GetRecipientDetails(string orderNumber, string haulierMnemonic, string projectNumber, string versionNumber, string userSchema)
        {
            string messg = "RevisedAgreementDAO/GetRecipientDetails?orderNumber=" + orderNumber + ", haulierMnemonic=" + haulierMnemonic + ", projectNumber=" + projectNumber + ", versionNumber=" + versionNumber + ", userSchema=" + userSchema;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            ContactModel contactDetail = new ContactModel();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                contactDetail,
                userSchema + ".GET_RECIPIENTS_DETAIL",
                parameter =>
                {
                    parameter.AddWithValue("p_ORDER_NUMBER", string.Empty, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_HAULIER_MNEMONIC", haulierMnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PROJECT_NUMBER", projectNumber, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VERSION_NUMBER", versionNumber, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       instance.AffectedParties = records.GetByteArrayOrNull("AFFECTED_PARTIES");
                   }
            );
            return contactDetail;
        }

        /// <summary>
        /// get org type based on organisation id
        /// </summary>
        /// <param name="organisationID">organisation id</param>
        /// <returns></returns>
        public static bool GetOrgTypeDetails(int organisationID)
        {
            ContactModel contactDetail = new ContactModel();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                contactDetail,
                UserSchema.Portal + ".GET_ORGTYPE_DETAIL",
                parameter =>
                {
                    parameter.AddWithValue("p_ORGANISATIONID", organisationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 (records, instance) =>
                 {
                     if (records.GetStringOrDefault("orgtype").ToLower() == "haulier")
                         instance.IsHaulier = true;
                     else
                         instance.IsHaulier = false;
                 }
                );

            return contactDetail.IsHaulier;
        }

        /// <summary>
        /// get route part node detail for revised agreement document
        /// </summary>
        /// <param name="orderNumber">order number</param>
        /// <param name="esDAlRefNo">esdal ref no</param>
        /// <param name="version_no">version no</param>
        /// <returns></returns>
        public static RoutePartsStructureRoutePartListPosition[] GetRouteParts(string orderNumber, string esDAlRefNo, int version_no, string haulierMnemonic, string projectNumber, string userSchema)
        {
            string messg = "RevisedAgreementDAO/GetRouteParts?orderNumber=" + orderNumber + ", esDAlRefNo=" + esDAlRefNo + ", version_no=" + version_no + ", haulierMnemonic=" + haulierMnemonic + ", projectNumber=" + projectNumber + ", userSchema=" + userSchema;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            List<RoutePartsStructureRoutePartListPosition> rpsrplpList = new List<RoutePartsStructureRoutePartListPosition>();

            int legNumber = 1;
            long routepartid = 0;

            List<OutBoundDoc> outBoundDoc = new List<OutBoundDoc>();
            List<OutBoundDoc> allOutBoundDoc = new List<OutBoundDoc>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               allOutBoundDoc,
               userSchema + ".GET_MULTI_VEHICLE_AGREEMENT",
               parameter =>
               {
                   parameter.AddWithValue("p_ORDER_NUMBER", string.Empty, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_HAULIER_MNEMONIC", haulierMnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_PROJECT_NUMBER", projectNumber, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_VERSION_NUMBER", version_no, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                  (records, instance) =>
                  {
                      instance.RoutePartId = records.GetLongOrDefault("route_part_id");
                      instance.VehicleId = records.GetLongOrDefault("vehicle_id");
                      instance.VehicleDesc = records.GetStringOrDefault("vehicle_desc");
                      instance.Length = (records["len"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("len");
                      instance.RearOverhang = (records["REAR_OVERHANG"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("REAR_OVERHANG");
                      instance.RigidLength = (records["rigid_len"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("rigid_len");
                      instance.FrontOverhang = (records["front_overhang"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("front_overhang");
                      instance.Width = (records["width"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("width");
                      instance.MaximumHeight = (records["max_height"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("max_height");
                      instance.RedHeight = (records["red_height"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("red_height");
                      instance.GrossWeight = (records["gross_weight"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("gross_weight");
                      instance.MaximumAxleWeight = (records["MAX_AXLE_WEIGHT"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("MAX_AXLE_WEIGHT");
                      instance.LeftOverhang = (records["LEFT_OVERHANG"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("LEFT_OVERHANG");
                      instance.RightOverhang = (records["RIGHT_OVERHANG"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("RIGHT_OVERHANG");
                      instance.IsSteerableAtRear = (records["IS_STEERABLE_AT_REAR"]).ToString() == string.Empty ? 0 : records.GetDecimalOrDefault("IS_STEERABLE_AT_REAR");
                      try
                      {
                          instance.WheelBase = Convert.ToDouble((records["wheelbase"]).ToString() == string.Empty ? 0 : records.GetDecimalOrDefault("wheelbase"));
                      }
                      catch
                      {
                          instance.WheelBase = (records["wheelbase"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("wheelbase");
                      }

                      instance.GroundClearance = (records["ground_clearance"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("ground_clearance");
                      instance.OutsideTrack = (records["outside_track"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("outside_track");
                      instance.ComparisonId = (records["comparison_id"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("comparison_id");
                      instance.RoutePartNo = (records["route_part_no"]).ToString() == string.Empty ? 0 : records.GetDecimalOrDefault("route_part_no");
                      instance.PartDescr = records.GetStringOrDefault("part_descr");
                      instance.TransportMode = (records["transport_mode"]).ToString() == string.Empty ? 0 : records.GetInt32OrDefault("transport_mode");
                      instance.PartName = records.GetStringOrDefault("PART_NAME");
                      instance.Name = records.GetStringOrDefault("name");
                      instance.ComponentType = records.GetStringOrDefault("component_type");
                      instance.VehicleType = records.GetStringOrDefault("vehicle_type");
                      instance.ComponentSubtype = records.GetStringOrDefault("component_subtype");
                      instance.InclueDockWayCaution = records.GetShortOrDefault("include_dock_caution");
                      instance.RedGroundClearance = (records["red_ground_clearance"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("red_ground_clearance");
                      instance.LongPosn = records.GetShortOrDefault("long_posn");

                  }
           );

            var groupedProposalList = allOutBoundDoc
                                        .GroupBy(x => x.VehicleId)
                                        .Select(grp => grp.ToList())
                                        .ToList();

            foreach (var groupProposal in groupedProposalList)
            {
                if (groupProposal.Count > 1)
                {
                    outBoundDoc.AddRange(groupProposal.OrderBy(x => x.LongPosn).Skip(1).Take(1).ToList());
                }
                else
                {
                    outBoundDoc.AddRange(groupProposal);
                }
            }

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                rpsrplpList,
                userSchema + ".GET_VEH_DTLS_REVISE_AGREEMENT",
                parameter =>
                {
                    parameter.AddWithValue("p_ORDER_NUMBER", string.Empty, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_HAULIER_MNEMONIC", haulierMnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PROJECT_NUMBER", projectNumber, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VERSION_NUMBER", version_no, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       PlannedRoutePartStructure2 prps = new PlannedRoutePartStructure2();
                       prps.Id = Convert.ToInt32(records.GetLongOrDefault("route_part_id"));

                       routepartid = records.GetLongOrDefault("route_part_id");
                       int routePartNo = records.GetInt16OrDefault("ROUTE_PART_NO");
                       prps.LegNumber = Convert.ToString(legNumber);

                       prps.Name = records.GetStringOrDefault("PART_NAME");

                       if (records.GetStringOrDefault("name") == "road")
                       {
                           prps.Mode = AggreedRouteXSD.ModeOfTransportType.road;
                       }
                       else if (records.GetStringOrDefault("name") == "air")
                       {
                           prps.Mode = AggreedRouteXSD.ModeOfTransportType.air;
                       }
                       else if (records.GetStringOrDefault("name") == "rail")
                       {
                           prps.Mode = AggreedRouteXSD.ModeOfTransportType.rail;
                       }
                       else if (records.GetStringOrDefault("name") == "sea")
                       {
                           prps.Mode = AggreedRouteXSD.ModeOfTransportType.sea;
                       }

                       PlannedRoadRoutePartStructure prp = new PlannedRoadRoutePartStructure();

                           #region StartPointListPosition
                           PlannedRoadRoutePartStructureStartPointListPosition[] prrpssplpList = new PlannedRoadRoutePartStructureStartPointListPosition[1];
                       PlannedRoadRoutePartStructureStartPointListPosition prrpssplp = new PlannedRoadRoutePartStructureStartPointListPosition();

                       prrpssplp.StartPoint = GetSimplifiedRoutePointStart(orderNumber, haulierMnemonic, projectNumber, version_no.ToString(), userSchema, routepartid);
                       prrpssplpList[0] = prrpssplp;
                       prp.StartPointListPosition = prrpssplpList;
                           #endregion
                           #region EndPointListPosition
                           PlannedRoadRoutePartStructureEndPointListPosition[] prrpseplpList = new PlannedRoadRoutePartStructureEndPointListPosition[1];
                       PlannedRoadRoutePartStructureEndPointListPosition prrpseplp = new PlannedRoadRoutePartStructureEndPointListPosition();
                       prrpseplp.EndPoint = GetSimplifiedRoutePointEnd(orderNumber, haulierMnemonic, projectNumber, version_no.ToString(), userSchema, routepartid);
                       prrpseplpList[0] = prrpseplp;
                       prp.EndPointListPosition = prrpseplpList;
                           #endregion

                           int recordcount = 0;
                       var outBoundDocVar = outBoundDoc.Where(r => r.RoutePartId.Equals(routepartid)).ToList();
                       AggreedRouteXSD.VehiclesSummaryStructure vss = new AggreedRouteXSD.VehiclesSummaryStructure();
                       outBoundDoc.RemoveAll(r => r.RoutePartId.Equals(routepartid));

                       if (outBoundDocVar.Count > 0)
                       {
                           #region ConfigurationSummaryListPosition
                           AggreedRouteXSD.VehiclesSummaryStructureConfigurationSummaryListPosition[] vscslpList = new AggreedRouteXSD.VehiclesSummaryStructureConfigurationSummaryListPosition[outBoundDocVar.Count];
                           recordcount = 0;
                           foreach (var outbound in outBoundDocVar)
                           {
                               AggreedRouteXSD.VehiclesSummaryStructureConfigurationSummaryListPosition vscslp = new AggreedRouteXSD.VehiclesSummaryStructureConfigurationSummaryListPosition();
                               vscslp.ConfigurationSummary = outbound.VehicleDesc;
                               vscslpList[recordcount] = vscslp;
                               recordcount++;
                           }
                           vss.ConfigurationSummaryListPosition = vscslpList;
                           #endregion

                           #region OverallLengthListPosition
                           AggreedRouteXSD.VehiclesSummaryStructureOverallLengthListPosition[] vssollpList = new AggreedRouteXSD.VehiclesSummaryStructureOverallLengthListPosition[outBoundDocVar.Count];
                           recordcount = 0;
                           foreach (var outbound in outBoundDocVar)
                           {
                               AggreedRouteXSD.VehiclesSummaryStructureOverallLengthListPosition vssollp = new AggreedRouteXSD.VehiclesSummaryStructureOverallLengthListPosition();
                               AggreedRouteXSD.SummaryLengthStructure sls = new AggreedRouteXSD.SummaryLengthStructure();
                               sls.IncludingProjections = Convert.ToDecimal(outbound.Length);

                               sls.ExcludingProjections = sls.IncludingProjections - (Convert.ToDecimal(outbound.RearOverhang) + Convert.ToDecimal(outbound.FrontOverhang));
                               sls.ExcludingProjectionsSpecified = true;

                               vssollp.OverallLength = sls;
                               vssollpList[recordcount] = vssollp;
                               recordcount++;
                           }
                           vss.OverallLengthListPosition = vssollpList;
                           #endregion

                           #region RigidLengthListPosition
                           AggreedRouteXSD.VehiclesSummaryStructureRigidLengthListPosition[] vssrllpList = new AggreedRouteXSD.VehiclesSummaryStructureRigidLengthListPosition[outBoundDocVar.Count];
                           recordcount = 0;
                           foreach (var outbound in outBoundDocVar)
                           {

                               AggreedRouteXSD.VehiclesSummaryStructureRigidLengthListPosition vssrllp = new AggreedRouteXSD.VehiclesSummaryStructureRigidLengthListPosition();
                               vssrllp.RigidLength = Convert.ToDecimal(outbound.RigidLength);
                               vssrllp.RigidLengthSpecified = true;
                               vssrllpList[recordcount] = vssrllp;
                               recordcount++;
                           }
                           vss.RigidLengthListPosition = vssrllpList;
                           #endregion

                           #region RearOverhangListPosition
                           AggreedRouteXSD.VehiclesSummaryStructureRearOverhangListPosition[] vssrolpList = new AggreedRouteXSD.VehiclesSummaryStructureRearOverhangListPosition[outBoundDocVar.Count];
                           recordcount = 0;
                           foreach (var outbound in outBoundDocVar)
                           {
                               AggreedRouteXSD.VehiclesSummaryStructureRearOverhangListPosition vssrolp = new AggreedRouteXSD.VehiclesSummaryStructureRearOverhangListPosition();
                               vssrolp.RearOverhang = Convert.ToDecimal(outbound.RearOverhang);
                               vssrolp.RearOverhangSpecified = true;
                               vssrolpList[recordcount] = vssrolp;
                               recordcount++;
                           }
                           vss.RearOverhangListPosition = vssrolpList;
                           #endregion

                           #region FrontOverhangListPosition
                           AggreedRouteXSD.VehiclesSummaryStructureFrontOverhangListPosition[] vssfolpList = new AggreedRouteXSD.VehiclesSummaryStructureFrontOverhangListPosition[outBoundDocVar.Count];
                           recordcount = 0;
                           foreach (var outbound in outBoundDocVar)
                           {
                               AggreedRouteXSD.VehiclesSummaryStructureFrontOverhangListPosition vssfolp = new AggreedRouteXSD.VehiclesSummaryStructureFrontOverhangListPosition();
                               AggreedRouteXSD.FrontOverhangStructure frontoverhangstru = new AggreedRouteXSD.FrontOverhangStructure();
                               frontoverhangstru.InfrontOfCab = false;
                               frontoverhangstru.Value = Convert.ToDecimal(outbound.FrontOverhang);
                               vssfolp.FrontOverhang = frontoverhangstru;
                               vssfolp.FrontOverhangSpecified = true;
                               vssfolpList[recordcount] = vssfolp;
                               recordcount++;
                           }
                           vss.FrontOverhangListPosition = vssfolpList;
                           #endregion

                           #region OverallWidthListPosition
                           AggreedRouteXSD.VehiclesSummaryStructureOverallWidthListPosition[] vssowlpList = new AggreedRouteXSD.VehiclesSummaryStructureOverallWidthListPosition[outBoundDocVar.Count];
                           recordcount = 0;
                           foreach (var outbound in outBoundDocVar)
                           {

                               AggreedRouteXSD.VehiclesSummaryStructureOverallWidthListPosition vssowlp = new AggreedRouteXSD.VehiclesSummaryStructureOverallWidthListPosition();
                               vssowlp.OverallWidth = Convert.ToDecimal(outbound.Width);
                               vssowlp.OverallWidthSpecified = true;
                               vssowlpList[recordcount] = vssowlp;
                               recordcount++;
                           }
                           vss.OverallWidthListPosition = vssowlpList;
                           #endregion

                           #region OverallHeightListPosition
                           AggreedRouteXSD.VehiclesSummaryStructureOverallHeightListPosition[] vssohlpList = new AggreedRouteXSD.VehiclesSummaryStructureOverallHeightListPosition[outBoundDocVar.Count];
                           recordcount = 0;
                           foreach (var outbound in outBoundDocVar)
                           {
                               AggreedRouteXSD.VehiclesSummaryStructureOverallHeightListPosition vssohlp = new AggreedRouteXSD.VehiclesSummaryStructureOverallHeightListPosition();
                               AggreedRouteXSD.SummaryHeightStructure shs = new AggreedRouteXSD.SummaryHeightStructure();
                               shs.MaxHeight = Convert.ToDecimal(outbound.MaximumHeight);
                               shs.ReducibleHeight = Convert.ToDecimal(outbound.RedHeight);
                               shs.ReducibleHeightSpecified = true;
                               vssohlp.OverallHeight = shs;
                               vssohlpList[recordcount] = vssohlp;
                               recordcount++;
                           }
                           vss.OverallHeightListPosition = vssohlpList;
                           #endregion

                           #region GrossWeightListPosition
                           AggreedRouteXSD.VehiclesSummaryStructureGrossWeightListPosition[] vssgwlpList = new AggreedRouteXSD.VehiclesSummaryStructureGrossWeightListPosition[outBoundDocVar.Count];
                           recordcount = 0;
                           foreach (var outbound in outBoundDocVar)
                           {
                               AggreedRouteXSD.VehiclesSummaryStructureGrossWeightListPosition vssgwlp = new AggreedRouteXSD.VehiclesSummaryStructureGrossWeightListPosition();
                               AggreedRouteXSD.GrossWeightStructure gws = new AggreedRouteXSD.GrossWeightStructure();
                               gws.ExcludesTractors = false;
                               gws.Item = Convert.ToString(outbound.GrossWeight);

                               vssgwlp.GrossWeight = gws;
                               vssgwlpList[recordcount] = vssgwlp;
                               recordcount++;
                           }
                           #endregion

                           #region MaxAxleWeightListPosition
                           vss.GrossWeightListPosition = vssgwlpList;
                           AggreedRouteXSD.VehiclesSummaryStructureMaxAxleWeightListPosition[] vssmawlpList = new AggreedRouteXSD.VehiclesSummaryStructureMaxAxleWeightListPosition[outBoundDocVar.Count];
                           recordcount = 0;
                           foreach (var outbound in outBoundDocVar)
                           {
                               AggreedRouteXSD.VehiclesSummaryStructureMaxAxleWeightListPosition vssmawlp = new AggreedRouteXSD.VehiclesSummaryStructureMaxAxleWeightListPosition();
                               AggreedRouteXSD.SummaryMaxAxleWeightStructure smaws = new AggreedRouteXSD.SummaryMaxAxleWeightStructure();
                               smaws.ItemElementName = AggreedRouteXSD.ItemChoiceType1.Weight;
                               smaws.Item = Convert.ToString(outbound.MaximumAxleWeight);
                               vssmawlp.MaxAxleWeight = smaws;
                               vssmawlpList[recordcount] = vssmawlp;
                               recordcount++;
                           }
                           vss.MaxAxleWeightListPosition = vssmawlpList;
                           #endregion

                           #region GroundClearanceListPosition
                           AggreedRouteXSD.VehiclesSummaryStructureGroundClearanceListPosition[] vssgclpList = new AggreedRouteXSD.VehiclesSummaryStructureGroundClearanceListPosition[outBoundDocVar.Count];
                           recordcount = 0;
                           foreach (var outbound in outBoundDocVar)
                           {
                               AggreedRouteXSD.VehiclesSummaryStructureGroundClearanceListPosition vssgclp = new AggreedRouteXSD.VehiclesSummaryStructureGroundClearanceListPosition();
                               vssgclp.GroundClearance = Convert.ToDecimal(outbound.GroundClearance);
                               vssgclp.GroundClearanceSpecified = true;
                               vssgclpList[recordcount] = vssgclp;
                               recordcount++;
                           }

                           vss.GroundClearanceListPosition = vssgclpList;
                           #endregion

                           #region ReducedGroundClearanceListPosition

                           AggreedRouteXSD.VehiclesSummaryStructureReducedGroundClearanceListPosition[] vssrgclpList = new AggreedRouteXSD.VehiclesSummaryStructureReducedGroundClearanceListPosition[outBoundDocVar.Count];
                           recordcount = 0;
                           foreach (var outbound in outBoundDocVar)
                           {
                               AggreedRouteXSD.VehiclesSummaryStructureReducedGroundClearanceListPosition vssrgclp = new AggreedRouteXSD.VehiclesSummaryStructureReducedGroundClearanceListPosition();
                               vssrgclp.ReducedGroundClearance = Convert.ToDecimal(outbound.RedGroundClearance);
                               vssrgclp.ReducedGroundClearanceSpecified = true;
                               vssrgclpList[recordcount] = vssrgclp;
                           }

                           vss.ReducedGroundClearanceListPosition = vssrgclpList;

                           #endregion

                           #region LeftOverhangListPosition

                           AggreedRouteXSD.VehiclesSummaryStructureLeftOverhangListPosition[] vsslolpList = new AggreedRouteXSD.VehiclesSummaryStructureLeftOverhangListPosition[outBoundDocVar.Count];
                           recordcount = 0;
                           foreach (var outbound in outBoundDocVar)
                           {
                               AggreedRouteXSD.VehiclesSummaryStructureLeftOverhangListPosition vssloclp = new AggreedRouteXSD.VehiclesSummaryStructureLeftOverhangListPosition();
                               vssloclp.LeftOverhang = Convert.ToDecimal(outbound.LeftOverhang);
                               vssloclp.LeftOverhangSpecified = true;
                               vsslolpList[recordcount] = vssloclp;
                           }

                           vss.LeftOverhangListPosition = vsslolpList;

                           #endregion

                           #region RightOverhangListPosition
                           AggreedRouteXSD.VehiclesSummaryStructureRightOverhangListPosition[] vssrOvlpList = new AggreedRouteXSD.VehiclesSummaryStructureRightOverhangListPosition[outBoundDocVar.Count];
                           recordcount = 0;
                           foreach (var outbound in outBoundDocVar)
                           {
                               AggreedRouteXSD.VehiclesSummaryStructureRightOverhangListPosition vssrOvlp = new AggreedRouteXSD.VehiclesSummaryStructureRightOverhangListPosition();
                               vssrOvlp.RightOverhang = Convert.ToDecimal(outbound.RightOverhang);
                               vssrOvlp.RightOverhangSpecified = true;
                               vssrOvlpList[recordcount] = vssrOvlp;
                           }

                           vss.RightOverhangListPosition = vssrOvlpList;
                           #endregion


                           recordcount = 0;
                           int totaAlternativeId = 1;

                           AggreedRouteXSD.VehiclesSummaryStructureVehicleSummaryListPosition[] vssvslpList = new AggreedRouteXSD.VehiclesSummaryStructureVehicleSummaryListPosition[outBoundDocVar.Count];
                           foreach (var outbound in outBoundDocVar)
                           {

                               List<VehicleConfigList> vehicleConfigurationList = OutBoundDAO.GetVehicleConfigurationDetails(Convert.ToInt32(outbound.VehicleId), userSchema);

                               // Business logic is written for semi vehicle
                               if (outbound.VehicleType.ToLower() == "semi trailer(3-8) vehicle" ||
                                   outbound.VehicleType.ToLower() == "boat mast exception" || outbound.VehicleType.ToLower() == "semi vehicle" || outbound.VehicleType.ToLower() == "conventional tractor"
                                       || outbound.VehicleType.ToLower() == "crane" || outbound.VehicleType.ToLower() == "mobile crane")
                               {
                                   AggreedRouteXSD.VehiclesSummaryStructureVehicleSummaryListPosition vssvslp = new AggreedRouteXSD.VehiclesSummaryStructureVehicleSummaryListPosition();
                                   AggreedRouteXSD.VehicleSummaryStructure vssList = new AggreedRouteXSD.VehicleSummaryStructure();

                                   AggreedRouteXSD.VehicleSummaryTypeChoiceStructure vstcs = new AggreedRouteXSD.VehicleSummaryTypeChoiceStructure();
                                   AggreedRouteXSD.SemiTrailerSummaryStructure stss = new AggreedRouteXSD.SemiTrailerSummaryStructure();

                                   if (outbound.ComponentType != string.Empty)
                                   {
                                       //stss.TractorSubType = CommonMethods.GetVehicleComponentsType(outbound.ComponentType);
                                       stss.TractorSubType = CommonMethods.GetAggreedVehicleComponentSubTypes(outbound.ComponentType);
                                   }

                                   if (outbound.ComponentSubtype != string.Empty)
                                   {
                                       //stss.TrailerSubType = AggreedRouteXSD.VehicleComponentSubType.semitrailer;
                                       stss.TrailerSubType = CommonMethods.GetAggreedVehicleComponentSubTypes(outbound.ComponentSubtype);
                                   }

                                   stss.Summary = outbound.VehicleDesc;

                                   AggreedRouteXSD.SummaryWeightStructure sws = new AggreedRouteXSD.SummaryWeightStructure();
                                   sws.Item = Convert.ToString(outbound.GrossWeight);
                                   stss.GrossWeight = sws;

                                   stss.IsSteerableAtRear = outbound.IsSteerableAtRear == 1;
                                   stss.IsSteerableAtRearSpecified = true;

                                   AggreedRouteXSD.SummaryMaxAxleWeightStructure smawsConfig = new AggreedRouteXSD.SummaryMaxAxleWeightStructure();
                                   smawsConfig.ItemElementName = AggreedRouteXSD.ItemChoiceType1.Weight;
                                   smawsConfig.Item = Convert.ToString(outbound.MaximumAxleWeight);
                                   stss.MaxAxleWeight = smawsConfig;

                                   AggreedRouteXSD.SummaryAxleStructure sas = new AggreedRouteXSD.SummaryAxleStructure();
                                   List<VehComponentAxles> vehicleComponentAxlesList = GetVehicleComponentAxles(orderNumber, version_no, haulierMnemonic, projectNumber, outbound.VehicleId, userSchema);
                                   sas.NumberOfAxles = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.AxleCount));
                                   #region AxleWeightListPosition
                                   sas.AxleWeightListPosition = CommonMethods.GetRevisedAgreementAxleWeightListPosition(vehicleComponentAxlesList);
                                   #endregion
                                   #region WheelsPerAxleListPosition
                                   sas.WheelsPerAxleListPosition = CommonMethods.GetRevisedAgreementWheelsPerAxleListPosition(vehicleComponentAxlesList);
                                   #endregion
                                   #region AxleSpacingListPosition
                                   sas.AxleSpacingListPosition = CommonMethods.GetRevisedAgreementAxleSpacingListPosition(vehicleComponentAxlesList, true);
                                   #endregion
                                   #region TyreSizeListPosition
                                   sas.TyreSizeListPosition = CommonMethods.GetRevisedAgreementTyreSizeListPosition(vehicleComponentAxlesList);
                                   #endregion
                                   #region WheelSpacingListPosition
                                   sas.WheelSpacingListPosition = CommonMethods.GetRevisedAgreementWheelSpacingListPosition(vehicleComponentAxlesList);
                                   #endregion
                                   stss.AxleConfiguration = sas;

                                   stss.RigidLength = outbound.RigidLength != null ? Convert.ToDecimal(outbound.RigidLength) : 0;
                                   stss.Width = outbound.Width != null ? Convert.ToDecimal(outbound.Width) : 0;
                                   AggreedRouteXSD.SummaryHeightStructure shsVehi = new AggreedRouteXSD.SummaryHeightStructure();
                                   shsVehi.MaxHeight = outbound.MaximumHeight != null ? Convert.ToDecimal(outbound.MaximumHeight) : 0;
                                   shsVehi.ReducibleHeight = outbound.RedHeight != null ? Convert.ToDecimal(outbound.RedHeight) : 0;
                                   shsVehi.ReducibleHeightSpecified = true;
                                   stss.Height = shsVehi;

                                   stss.Wheelbase = outbound.WheelBase != null ? Convert.ToDecimal(outbound.WheelBase) : 0;
                                   stss.WheelbaseSpecified = true;

                                   stss.RearOverhang = outbound.RearOverhang != null ? Convert.ToDecimal(outbound.RearOverhang) : 0;

                                   stss.FrontOverhang = outbound.FrontOverhang != null ? Convert.ToDecimal(outbound.FrontOverhang) : 0;
                                   stss.FrontOverhangSpecified = true;

                                   stss.LeftOverhang = outbound.LeftOverhang != null ? Convert.ToDecimal(outbound.LeftOverhang) : 0;
                                   stss.LeftOverhangSpecified = true;

                                   stss.RightOverhang = outbound.RightOverhang != null ? Convert.ToDecimal(outbound.RightOverhang) : 0;
                                   stss.RightOverhangSpecified = true;

                                   stss.ReducedGroundClearance = outbound.RedGroundClearance != null ? Convert.ToDecimal(outbound.RedGroundClearance) : 0;
                                   stss.ReducedGroundClearanceSpecified = true;

                                   stss.GroundClearance = outbound.GroundClearance != null ? Convert.ToDecimal(outbound.GroundClearance) : 0;
                                   stss.GroundClearanceSpecified = true;
                                   stss.RightOverhang = outbound.RightOverhang != null ? Convert.ToDecimal(outbound.RightOverhang) : 0;
                                   stss.RightOverhangSpecified = true;
                                   stss.OutsideTrack = outbound.OutsideTrack != null ? (decimal)outbound.OutsideTrack : 0;
                                   stss.OutsideTrackSpecified = true;
                                   vstcs.Item = stss;
                                   vssList.Configuration = vstcs;

                                   if (outbound.VehicleType == "drawbar vehicle")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.drawbarvehicle;
                                   }
                                   if (outbound.VehicleType == "semi vehicle")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.semivehicle;
                                   }
                                   if (outbound.VehicleType == "rigid vehicle")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.rigidvehicle;
                                   }
                                   if (outbound.VehicleType == "tracked vehicle")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.trackedvehicle;
                                   }
                                   if (outbound.VehicleType == "other in line")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.otherinline;
                                   }
                                   if (outbound.VehicleType == "other side by side")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.othersidebyside;
                                   }
                                   if (outbound.VehicleType == "spmt")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.spmt;
                                   }
                                   if (outbound.VehicleType == "crane" || outbound.VehicleType == "mobile crane")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.crane;
                                   }
                                   if (outbound.VehicleType == "Recovery Vehicle")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.recoveryvehicle;
                                   }

                                   vssList.AlternativeId = Convert.ToString(totaAlternativeId);

                                   double grossWeight = Convert.ToDouble(outbound.GrossWeight);

                                   if (grossWeight >= 80000 && grossWeight <= 150000)
                                   {
                                       vssList.WeightConformance = AggreedRouteXSD.SummaryWeightConformanceType.heavystgo;
                                   }
                                   else
                                   {
                                       vssList.WeightConformance = AggreedRouteXSD.SummaryWeightConformanceType.other;
                                   }

                                   vssvslp.VehicleSummary = vssList;

                                   vssvslpList[recordcount] = vssvslp;

                                   recordcount++;
                                   totaAlternativeId++;
                               }
                               else if (outbound.VehicleType.ToLower() == "drawbar vehicle" ||
                                    outbound.VehicleType.ToLower() == "drawbar trailer(3-8) vehicle" ||
                                       outbound.VehicleType.ToLower() == "ballast tractor" ||
                                       outbound.VehicleType.ToLower() == "rigid vehicle" ||
                                       outbound.VehicleType.ToLower() == "spmt"
                                       || outbound.VehicleType.ToLower() == "other in line" || outbound.VehicleType.ToLower() == "recovery vehicle" || outbound.VehicleType.ToLower() == "rigid and drag")
                               {
                                   // Business logic is written for Non Semi Vehicle
                                   AggreedRouteXSD.VehiclesSummaryStructureVehicleSummaryListPosition vssvslp = new AggreedRouteXSD.VehiclesSummaryStructureVehicleSummaryListPosition();
                                   AggreedRouteXSD.VehicleSummaryStructure vssList = new AggreedRouteXSD.VehicleSummaryStructure();

                                   AggreedRouteXSD.VehicleSummaryTypeChoiceStructure vstcs = new AggreedRouteXSD.VehicleSummaryTypeChoiceStructure();
                                   AggreedRouteXSD.NonSemiTrailerSummaryStructure stss = new AggreedRouteXSD.NonSemiTrailerSummaryStructure();

                                   AggreedRouteXSD.NonSemiTrailerSummaryStructureComponentListPosition[] nsclp = new AggreedRouteXSD.NonSemiTrailerSummaryStructureComponentListPosition[vehicleConfigurationList.Count];

                                   int newRecordcount = 0;
                                   int longitudeIncrement = 1;

                                   foreach (var vehicleList in vehicleConfigurationList)
                                   {
                                       AggreedRouteXSD.NonSemiTrailerSummaryStructureComponentListPosition nsclpObject = new AggreedRouteXSD.NonSemiTrailerSummaryStructureComponentListPosition();

                                       AggreedRouteXSD.ComponentSummaryStructure css = new AggreedRouteXSD.ComponentSummaryStructure();

                                       css.Longitude = Convert.ToString(longitudeIncrement);

                                       if (vehicleList.ComponentType != string.Empty)
                                       {
                                           //css.ComponentType = CommonMethods.GetVehicleMainComponentsType(vehicleList.ComponentType);
                                           css.ComponentType = CommonMethods.GetAggreedVehicleComponentSubTypesNonSemiVehicles(vehicleList.ComponentType);
                                       }

                                       if (vehicleList.ComponentSubType != string.Empty)
                                       {
                                           //css.ComponentSubType = CommonMethods.GetVehicleComponentsType(vehicleList.ComponentSubType);
                                           css.ComponentSubType = CommonMethods.GetAggreedVehicleComponentSubTypes(vehicleList.ComponentSubType);
                                       }

                                       nsclpObject.Component = css;

                                       #region Non Semi Vehicle configuration

                                       if (nsclpObject.Component.ComponentType == AggreedRouteXSD.VehicleComponentType.ballasttractor)
                                       {

                                           AggreedRouteXSD.DrawbarTractorSummaryStructure dtss = new AggreedRouteXSD.DrawbarTractorSummaryStructure();

                                           dtss.Summary = vehicleList.VehicleDescription;
                                           dtss.Weight = Convert.ToString(vehicleList.GrossWeight);

                                           AggreedRouteXSD.SummaryMaxAxleWeightStructure smawsConfig = new AggreedRouteXSD.SummaryMaxAxleWeightStructure();
                                           smawsConfig.ItemElementName = AggreedRouteXSD.ItemChoiceType1.Weight;
                                           smawsConfig.Item = Convert.ToString(outbound.MaximumAxleWeight);
                                           dtss.MaxAxleWeight = smawsConfig;

                                           AggreedRouteXSD.SummaryAxleStructure sas = new AggreedRouteXSD.SummaryAxleStructure();

                                           List<VehComponentAxles> vehicleComponentAxlesList = OutBoundDAO.GetVehicleComponentAxlesByComponent(vehicleList.ComponentId, vehicleList.VehicleId, userSchema);

                                           sas.NumberOfAxles = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.AxleCount));

                                           #region AxleWeightListPosition
                                           sas.AxleWeightListPosition = CommonMethods.GetRevisedAgreementAxleWeightListPosition(vehicleComponentAxlesList);
                                           #endregion
                                           #region WheelsPerAxleListPosition
                                           sas.WheelsPerAxleListPosition = CommonMethods.GetRevisedAgreementWheelsPerAxleListPosition(vehicleComponentAxlesList);
                                           #endregion
                                           #region AxleSpacingListPosition
                                           sas.AxleSpacingListPosition = CommonMethods.GetRevisedAgreementAxleSpacingListPosition(vehicleComponentAxlesList, false);
                                           #endregion
                                           #region TyreSizeListPosition
                                           sas.TyreSizeListPosition = CommonMethods.GetRevisedAgreementTyreSizeListPosition(vehicleComponentAxlesList);
                                           #endregion
                                           #region WheelSpacingListPosition
                                           sas.WheelSpacingListPosition = CommonMethods.GetRevisedAgreementWheelSpacingListPosition(vehicleComponentAxlesList);
                                           #endregion

                                           dtss.AxleConfiguration = sas;

                                           dtss.Length = vehicleList.RigidLength != null ? (decimal)vehicleList.RigidLength : 0;
                                           dtss.LengthSpecified = true;

                                           dtss.AxleSpacingToFollowing = vehicleList.SpaceToFollowing != null ? (decimal)vehicleList.SpaceToFollowing : 0;
                                           dtss.AxleSpacingToFollowingSpecified = true;

                                           AggreedRouteXSD.SummaryHeightStructure shsVehi = new AggreedRouteXSD.SummaryHeightStructure();
                                           shsVehi.MaxHeight = outbound.MaximumHeight != null ? (decimal)outbound.MaximumHeight : 0;
                                           shsVehi.ReducibleHeight = vehicleList.RedHeight != null ? (decimal)vehicleList.RedHeight : 0;

                                           shsVehi.ReducibleHeightSpecified = true;

                                           nsclpObject.Component.Item = dtss;

                                           nsclp[newRecordcount] = nsclpObject;

                                           newRecordcount++;
                                       }

                                       if ((outbound.VehicleType.ToLower() == "recovery vehicle" && (nsclpObject.Component.ComponentType == AggreedRouteXSD.VehicleComponentType.semitrailer || nsclpObject.Component.ComponentType == AggreedRouteXSD.VehicleComponentType.conventionaltractor)))
                                       {
                                           #region For ballasttractor
                                           AggreedRouteXSD.DrawbarTractorSummaryStructure dtss = new AggreedRouteXSD.DrawbarTractorSummaryStructure();

                                           dtss.Summary = vehicleList.VehicleDescription;

                                           dtss.Weight = Convert.ToString(vehicleList.GrossWeight);

                                           AggreedRouteXSD.SummaryMaxAxleWeightStructure smawsConfig = new AggreedRouteXSD.SummaryMaxAxleWeightStructure();
                                           smawsConfig.ItemElementName = AggreedRouteXSD.ItemChoiceType1.Weight;
                                           smawsConfig.Item = Convert.ToString(outbound.MaximumAxleWeight);

                                           dtss.MaxAxleWeight = smawsConfig;

                                           AggreedRouteXSD.SummaryAxleStructure sas = new AggreedRouteXSD.SummaryAxleStructure();

                                           List<VehComponentAxles> vehicleComponentAxlesList = OutBoundDAO.GetVehicleComponentAxlesByComponent(vehicleList.ComponentId, outbound.VehicleId, UserSchema.Portal);

                                           sas.NumberOfAxles = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.AxleCount));

                                           //sas.NumberOfWheel = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.WheelCount));

                                           #region AxleWeightListPosition
                                           sas.AxleWeightListPosition = CommonMethods.GetRevisedAgreementAxleWeightListPosition(vehicleComponentAxlesList);
                                           #endregion

                                           #region WheelsPerAxleListPosition
                                           sas.WheelsPerAxleListPosition = CommonMethods.GetRevisedAgreementWheelsPerAxleListPosition(vehicleComponentAxlesList);
                                           #endregion

                                           #region AxleSpacingListPosition
                                           sas.AxleSpacingListPosition = CommonMethods.GetRevisedAgreementAxleSpacingListPosition(vehicleComponentAxlesList);
                                           #endregion

                                           //Added RM#4386
                                           #region AxleSpacingToFollow
                                           //sas.AxleSpacingToFollowing = CommonMethods.GetRevisedAgreementAxleSpacingToFollowListPositionAxleSpacing(vehicleComponentAxlesList, vehicleConfigurationList[0].SpaceToFollowing);
                                           #endregion

                                           #region TyreSizeListPosition
                                           sas.TyreSizeListPosition = CommonMethods.GetRevisedAgreementTyreSizeListPosition(vehicleComponentAxlesList);
                                           #endregion

                                           #region WheelSpacingListPosition
                                           sas.WheelSpacingListPosition = CommonMethods.GetRevisedAgreementWheelSpacingListPosition(vehicleComponentAxlesList);
                                           #endregion

                                           dtss.AxleConfiguration = sas;

                                           dtss.Length = vehicleList.RigidLength != null ? (decimal)vehicleList.RigidLength : 0;
                                           if (dtss.Length != null && dtss.Length > 0)
                                           {
                                               dtss.LengthSpecified = true;
                                           }
                                           else
                                           {
                                               dtss.LengthSpecified = false;
                                           }

                                           dtss.AxleSpacingToFollowing = (decimal)vehicleList.SpaceToFollowing;
                                           dtss.AxleSpacingToFollowingSpecified = true;

                                           AggreedRouteXSD.SummaryHeightStructure shsVehi = new AggreedRouteXSD.SummaryHeightStructure();
                                           shsVehi.MaxHeight = (decimal)outbound.MaximumHeight;

                                           shsVehi.ReducibleHeight = vehicleList.RedHeight != null ? (decimal)vehicleList.RedHeight : 0;
                                           if (shsVehi.ReducibleHeight != null && shsVehi.ReducibleHeight > 0)
                                           {
                                               shsVehi.ReducibleHeightSpecified = true;
                                           }
                                           else
                                           {
                                               shsVehi.ReducibleHeightSpecified = false;
                                           }

                                           nsclpObject.Component.Item = dtss;

                                           nsclp[newRecordcount] = nsclpObject;

                                           newRecordcount++;
                                           #endregion
                                       }


                                       else if (nsclpObject.Component.ComponentType == AggreedRouteXSD.VehicleComponentType.drawbartrailer
                                           || nsclpObject.Component.ComponentType == AggreedRouteXSD.VehicleComponentType.spmt
                                           || nsclpObject.Component.ComponentType == AggreedRouteXSD.VehicleComponentType.rigidvehicle)
                                       {
                                           AggreedRouteXSD.LoadBearingSummaryStructure dtss = new AggreedRouteXSD.LoadBearingSummaryStructure();

                                           dtss.Summary = vehicleList.VehicleDescription;

                                           dtss.Weight = Convert.ToString(vehicleList.GrossWeight);

                                           dtss.IsSteerableAtRear = vehicleList.IsSteerableAtRear == 1;
                                           dtss.IsSteerableAtRearSpecified = true;

                                           AggreedRouteXSD.SummaryMaxAxleWeightStructure smawsConfig = new AggreedRouteXSD.SummaryMaxAxleWeightStructure();
                                           smawsConfig.ItemElementName = AggreedRouteXSD.ItemChoiceType1.Weight;
                                           smawsConfig.Item = Convert.ToString(outbound.MaximumAxleWeight);
                                           dtss.MaxAxleWeight = smawsConfig;

                                           AggreedRouteXSD.SummaryAxleStructure sas = new AggreedRouteXSD.SummaryAxleStructure();

                                           List<VehComponentAxles> vehicleComponentAxlesList = OutBoundDAO.GetVehicleComponentAxlesByComponent(vehicleList.ComponentId, vehicleList.VehicleId, userSchema);

                                           sas.NumberOfAxles = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.AxleCount));

                                           #region AxleWeightListPosition
                                           sas.AxleWeightListPosition = CommonMethods.GetRevisedAgreementAxleWeightListPosition(vehicleComponentAxlesList);
                                           #endregion
                                           #region WheelsPerAxleListPosition
                                           sas.WheelsPerAxleListPosition = CommonMethods.GetRevisedAgreementWheelsPerAxleListPosition(vehicleComponentAxlesList);
                                           #endregion
                                           #region AxleSpacingListPosition
                                           sas.AxleSpacingListPosition = CommonMethods.GetRevisedAgreementAxleSpacingListPosition(vehicleComponentAxlesList, false);
                                           #endregion
                                           #region TyreSizeListPosition
                                           sas.TyreSizeListPosition = CommonMethods.GetRevisedAgreementTyreSizeListPosition(vehicleComponentAxlesList);
                                           #endregion
                                           #region WheelSpacingListPosition
                                           sas.WheelSpacingListPosition = CommonMethods.GetRevisedAgreementWheelSpacingListPosition(vehicleComponentAxlesList);
                                           #endregion

                                           dtss.AxleConfiguration = sas;

                                           dtss.RigidLength = outbound.RigidLength != null ? (decimal)outbound.RigidLength : 0;
                                           dtss.Width = outbound.Width != null ? (decimal)outbound.Width : 0;

                                           AggreedRouteXSD.SummaryHeightStructure shsVehi = new AggreedRouteXSD.SummaryHeightStructure();
                                           shsVehi.MaxHeight = outbound.MaximumHeight != null ? (decimal)outbound.MaximumHeight : 0;
                                           shsVehi.ReducibleHeight = vehicleList.RedHeight != null ? (decimal)vehicleList.RedHeight : 0;
                                           shsVehi.ReducibleHeightSpecified = true;

                                           dtss.Height = shsVehi;
                                           dtss.Wheelbase = outbound.WheelBase != null ? (decimal)outbound.WheelBase : 0;
                                           dtss.WheelbaseSpecified = true;
                                           dtss.RearOverhang = vehicleList.RearOverhang != null ? (decimal)vehicleList.RearOverhang : 0;
                                           dtss.GroundClearance = vehicleList.GroundClearance != null ? (decimal)vehicleList.GroundClearance : 0;
                                           dtss.GroundClearanceSpecified = true;
                                           dtss.OutsideTrack = vehicleList.OutsideTrack != null ? (decimal)vehicleList.OutsideTrack : 0;
                                           dtss.OutsideTrackSpecified = true;

                                           // new code added
                                           dtss.LeftOverhang = vehicleList.LeftOverhang != null ? (decimal)vehicleList.LeftOverhang : 0;
                                           dtss.LeftOverhangSpecified = true;

                                           dtss.RightOverhang = vehicleList.RightOverhang != null ? (decimal)vehicleList.RightOverhang : 0;
                                           dtss.RightOverhangSpecified = true;

                                           dtss.FrontOverhang = vehicleList.FrontOverhang != null ? (decimal)vehicleList.FrontOverhang : 0;
                                           dtss.FrontOverhangSpecified = true;

                                           dtss.AxleSpacingToFollowing = vehicleList.SpaceToFollowing != null ? Convert.ToDecimal(vehicleList.SpaceToFollowing) : 0;
                                           dtss.AxleSpacingToFollowingSpecified = true;

                                           dtss.ReducedGroundClearance = vehicleList.RedGroundClearance != null ? (decimal)vehicleList.RedGroundClearance : 0;
                                           dtss.ReducedGroundClearanceSpecified = true;
                                           //new code ends here

                                           nsclpObject.Component.Item = dtss;

                                           nsclp[newRecordcount] = nsclpObject;

                                           newRecordcount++;
                                       }
                                       else if (nsclpObject.Component.ComponentType == AggreedRouteXSD.VehicleComponentType.semitrailer || nsclpObject.Component.ComponentType == AggreedRouteXSD.VehicleComponentType.conventionaltractor)
                                       {
                                           AggreedRouteXSD.SemiTrailerSummaryStructure dtss = new AggreedRouteXSD.SemiTrailerSummaryStructure();

                                           dtss.Summary = outbound.VehicleDesc;

                                           AggreedRouteXSD.SummaryWeightStructure sws = new AggreedRouteXSD.SummaryWeightStructure();
                                           sws.Item = Convert.ToString(outbound.GrossWeight);
                                           dtss.GrossWeight = sws;

                                           dtss.IsSteerableAtRear = outbound.IsSteerableAtRear == 1;
                                           dtss.IsSteerableAtRearSpecified = true;

                                           AggreedRouteXSD.SummaryMaxAxleWeightStructure smawsConfig = new AggreedRouteXSD.SummaryMaxAxleWeightStructure();
                                           smawsConfig.ItemElementName = AggreedRouteXSD.ItemChoiceType1.Weight;
                                           smawsConfig.Item = Convert.ToString(outbound.MaximumAxleWeight);
                                           dtss.MaxAxleWeight = smawsConfig;

                                           AggreedRouteXSD.SummaryAxleStructure sas = new AggreedRouteXSD.SummaryAxleStructure();

                                           List<VehComponentAxles> vehicleComponentAxlesList = GetVehicleComponentAxles(orderNumber, version_no, haulierMnemonic, projectNumber, outbound.VehicleId, userSchema);

                                           sas.NumberOfAxles = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.AxleCount));

                                           #region AxleWeightListPosition
                                           sas.AxleWeightListPosition = CommonMethods.GetRevisedAgreementAxleWeightListPosition(vehicleComponentAxlesList);
                                           #endregion
                                           #region WheelsPerAxleListPosition
                                           sas.WheelsPerAxleListPosition = CommonMethods.GetRevisedAgreementWheelsPerAxleListPosition(vehicleComponentAxlesList);
                                           #endregion
                                           #region AxleSpacingListPosition
                                           sas.AxleSpacingListPosition = CommonMethods.GetRevisedAgreementAxleSpacingListPosition(vehicleComponentAxlesList, true);
                                           #endregion
                                           #region TyreSizeListPosition
                                           sas.TyreSizeListPosition = CommonMethods.GetRevisedAgreementTyreSizeListPosition(vehicleComponentAxlesList);
                                           #endregion
                                           #region WheelSpacingListPosition
                                           sas.WheelSpacingListPosition = CommonMethods.GetRevisedAgreementWheelSpacingListPosition(vehicleComponentAxlesList);
                                           #endregion

                                           dtss.AxleConfiguration = sas;

                                           dtss.RigidLength = outbound.RigidLength != null ? (decimal)outbound.RigidLength : 0;
                                           dtss.Width = outbound.Width != null ? (decimal)outbound.Width : 0;

                                           AggreedRouteXSD.SummaryHeightStructure shsVehi = new AggreedRouteXSD.SummaryHeightStructure();
                                           shsVehi.MaxHeight = outbound.MaximumHeight != null ? (decimal)outbound.MaximumHeight : 0;
                                           shsVehi.ReducibleHeight = outbound.RedHeight != null ? (decimal)outbound.RedHeight : 0;
                                           shsVehi.ReducibleHeightSpecified = true;

                                           dtss.Height = shsVehi;
                                           dtss.Wheelbase = outbound.WheelBase != null ? (decimal)outbound.WheelBase : 0;
                                           dtss.WheelbaseSpecified = true;
                                           dtss.RearOverhang = outbound.RearOverhang != null ? (decimal)outbound.RearOverhang : 0;
                                           dtss.GroundClearance = outbound.GroundClearance != null ? (decimal)outbound.GroundClearance : 0;
                                           dtss.GroundClearanceSpecified = true;
                                           dtss.OutsideTrack = outbound.OutsideTrack != null ? (decimal)outbound.OutsideTrack : 0;
                                           dtss.OutsideTrackSpecified = true;

                                           // new code added
                                           dtss.LeftOverhang = outbound.LeftOverhang != null ? (decimal)outbound.LeftOverhang : 0;
                                           dtss.LeftOverhangSpecified = true;

                                           dtss.RightOverhang = outbound.RightOverhang != null ? (decimal)outbound.RightOverhang : 0;
                                           dtss.RightOverhangSpecified = true;

                                           dtss.FrontOverhang = outbound.FrontOverhang != null ? (decimal)outbound.FrontOverhang : 0;
                                           dtss.FrontOverhangSpecified = true;
                                           //new code ends here

                                           vstcs.Item = dtss;
                                           vssList.Configuration = vstcs;
                                           vssvslp.VehicleSummary = vssList;

                                           newRecordcount++;
                                       }

                                       #endregion

                                       longitudeIncrement++;
                                   }

                                   if (vehicleConfigurationList.Count > 0 && (vehicleConfigurationList[0].ComponentType != "semi trailer" && vehicleConfigurationList[0].ComponentType != "conventional tractor"))
                                   {
                                       stss.ComponentListPosition = nsclp;
                                   }

                                   if (outbound.VehicleType == "drawbar vehicle")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.drawbarvehicle;
                                   }
                                   if (outbound.VehicleType == "semi vehicle")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.semivehicle;
                                   }
                                   if (outbound.VehicleType == "rigid vehicle")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.rigidvehicle;
                                   }
                                   if (outbound.VehicleType == "tracked vehicle")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.trackedvehicle;
                                   }
                                   if (outbound.VehicleType == "other in line")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.otherinline;
                                   }
                                   if (outbound.VehicleType == "other side by side")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.othersidebyside;
                                   }
                                   
                                   if (outbound.VehicleType == "spmt")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.spmt;
                                   }
                                   if (outbound.VehicleType == "crane" || outbound.VehicleType == "mobile crane")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.crane;
                                   }
                                   if (outbound.VehicleType == "Recovery Vehicle")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.recoveryvehicle;
                                   }

                                   if (vehicleConfigurationList.Count > 0 && (vehicleConfigurationList[0].ComponentType != "semi trailer" && vehicleConfigurationList[0].ComponentType != "conventional tractor"))
                                   {
                                       vstcs.Item = stss;
                                   }

                                   vssList.Configuration = vstcs;

                                   vssvslp.VehicleSummary = vssList;

                                   vssvslpList[recordcount] = vssvslp;

                                   recordcount++;
                               }
                               else
                               {
                                   AggreedRouteXSD.VehiclesSummaryStructureVehicleSummaryListPosition vssvslp = new AggreedRouteXSD.VehiclesSummaryStructureVehicleSummaryListPosition();
                                   AggreedRouteXSD.VehicleSummaryStructure vssList = new AggreedRouteXSD.VehicleSummaryStructure();

                                   AggreedRouteXSD.VehicleSummaryTypeChoiceStructure vstcs = new AggreedRouteXSD.VehicleSummaryTypeChoiceStructure();

                                   AggreedRouteXSD.TrackedVehicleSummaryStructure dtss = new AggreedRouteXSD.TrackedVehicleSummaryStructure();

                                   AggreedRouteXSD.ComponentSummaryStructure css = new AggreedRouteXSD.ComponentSummaryStructure();

                                   dtss.Summary = outbound.VehicleDesc;

                                   AggreedRouteXSD.SummaryWeightStructure sws = new AggreedRouteXSD.SummaryWeightStructure();
                                   sws.Item = Convert.ToString(outbound.GrossWeight);
                                   dtss.GrossWeight = sws;

                                   dtss.RigidLength = (decimal)outbound.RigidLength;

                                   dtss.Width = (decimal)outbound.Width;

                                   if (outbound.VehicleType == "drawbar vehicle")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.drawbarvehicle;
                                   }
                                   if (outbound.VehicleType == "semi vehicle")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.semivehicle;
                                   }
                                   if (outbound.VehicleType == "rigid vehicle")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.rigidvehicle;
                                   }
                                   if (outbound.VehicleType == "tracked vehicle")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.trackedvehicle;
                                   }
                                   if (outbound.VehicleType == "other in line")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.otherinline;
                                   }
                                   if (outbound.VehicleType == "other side by side")
                                   {
                                       vssList.ConfigurationType = AggreedRouteXSD.VehicleConfigurationType.othersidebyside;
                                   }

                                   vstcs.Item = dtss;

                                   vssList.Configuration = vstcs;

                                   vssvslp.VehicleSummary = vssList;

                                   vssvslpList[recordcount] = vssvslp;

                                   recordcount++;
                                   totaAlternativeId++;
                               }
                               // code ends here
                           }

                           vss.VehicleSummaryListPosition = vssvslpList;

                           if (vss != null && vss.ConfigurationSummaryListPosition != null && vss.ConfigurationSummaryListPosition.Any(x => x.ConfigurationSummary == ""))
                           {
                               var i = 0;
                               foreach (var s in vss.ConfigurationSummaryListPosition)
                               {
                                   if (s.ConfigurationSummary == "")
                                   {
                                       if (vss.VehicleSummaryListPosition != null && vss.VehicleSummaryListPosition[i] != null
                                       && vss.VehicleSummaryListPosition[i].VehicleSummary != null
                                       && vss.VehicleSummaryListPosition[i].VehicleSummary.Configuration != null
                                       && vss.VehicleSummaryListPosition[i].VehicleSummary.Configuration.Item != null)
                                       {
                                           if ((vss.VehicleSummaryListPosition[i].VehicleSummary.Configuration.Item).GetType() == typeof(ProposedRouteXSD.NonSemiTrailerSummaryStructure))
                                           {
                                               var data = (ProposedRouteXSD.NonSemiTrailerSummaryStructure)vss.VehicleSummaryListPosition[i].VehicleSummary.Configuration.Item;
                                               if ((data.ComponentListPosition[0].Component.Item).GetType() == typeof(ProposedRouteXSD.DrawbarTractorSummaryStructure))
                                               {
                                                   var data1 = (ProposedRouteXSD.DrawbarTractorSummaryStructure)data.ComponentListPosition[0].Component.Item;
                                                   s.ConfigurationSummary = data1.Summary;
                                               }
                                               if ((data.ComponentListPosition[0].Component.Item).GetType() == typeof(ProposedRouteXSD.LoadBearingSummaryStructure))
                                               {
                                                   var data1 = (ProposedRouteXSD.LoadBearingSummaryStructure)data.ComponentListPosition[0].Component.Item;
                                                   s.ConfigurationSummary = data1.Summary;
                                               }
                                           }
                                           else if ((vss.VehicleSummaryListPosition[i].VehicleSummary.Configuration.Item).GetType() == typeof(ProposedRouteXSD.SemiTrailerSummaryStructure))
                                           {
                                               var data = (ProposedRouteXSD.SemiTrailerSummaryStructure)vss.VehicleSummaryListPosition[i].VehicleSummary.Configuration.Item;
                                               s.ConfigurationSummary = data.Summary;
                                           }
                                           else if ((vss.VehicleSummaryListPosition[i].VehicleSummary.Configuration.Item).GetType() == typeof(ProposedRouteXSD.TrackedVehicleSummaryStructure))
                                           {
                                               var data = (ProposedRouteXSD.TrackedVehicleSummaryStructure)vss.VehicleSummaryListPosition[i].VehicleSummary.Configuration.Item;
                                               s.ConfigurationSummary = data.Summary.ToString();
                                           }
                                       }
                                   }
                                   i++;
                               }
                           }

                           prp.Vehicles = vss;

                           #region Commented Structures Details
                           StructuresModel struInfo = GetStructuresDetail(orderNumber, esDAlRefNo, userSchema);

                           string recipientXMLInformation = string.Empty;

                           Byte[] affectedPartiesArray = struInfo.AffectedStructures;

                           if (affectedPartiesArray != null)
                           {
                               XmlDocument xmlDoc = new XmlDocument();
                               try
                               {
                                   recipientXMLInformation = Encoding.UTF8.GetString(affectedPartiesArray, 0, affectedPartiesArray.Length);

                                   xmlDoc.LoadXml(recipientXMLInformation);
                               }
                               catch (System.Xml.XmlException XE)
                               {
                                   //Some data is stored in gzip format, so we need to unzip then load it.
                                   byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(affectedPartiesArray);

                                   recipientXMLInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);

                                   xmlDoc.LoadXml(recipientXMLInformation);
                               }

                               XNamespace StructNS = "http://www.esdal.com/schemas/core/routeanalysis";
                               AggreedRouteXSD.SuitabilityResultStructure resultStructure;
                               List<AggreedRouteXSD.SuitabilityResultStructure> resultStructureList = new List<AggreedRouteXSD.SuitabilityResultStructure>();
                               AggreedRouteXSD.StructureSectionSuitabilityStructure structureSection;
                               List<AggreedRouteXSD.StructureSectionSuitabilityStructure> individaulSection = new List<AggreedRouteXSD.StructureSectionSuitabilityStructure>();
                               AggreedRouteXSD.StructureSuitabilityStructure appraisalRecord = new AggreedRouteXSD.StructureSuitabilityStructure();
                               AggreedRouteXSD.AffectedStructureStructure1 structdetails = new AggreedRouteXSD.AffectedStructureStructure1();
                               List<AggreedRouteXSD.AffectedStructureStructure1> affectedstructList = new List<AggreedRouteXSD.AffectedStructureStructure1>();
                               AffectedStructuresStructure ass = new AffectedStructuresStructure();

                               XDocument StructXDocument = XDocument.Parse(xmlDoc.OuterXml); // Converting from XMLDocument to XDocument

                               XDocument SelectedStructures = new XDocument(new XElement("AddedElements", from p in StructXDocument.Root.Elements(StructNS + "AnalysedStructuresPart")
                                                                                                          where p.Element(StructNS + "Name").Value == records.GetStringOrDefault("PART_NAME")
                                                                                                          select p.Elements(StructNS + "Structure"))); // Fetching structures in XDocument object

                               XmlDocument StructXMLDocument = new XmlDocument();
                               string StructModelString = string.Empty;
                               StructModelString = SelectedStructures.ToString();
                               StructXMLDocument.LoadXml(StructModelString); // Converting XDocument to XMLDocument

                               XmlNodeList parentNode = StructXMLDocument.GetElementsByTagName("Structure");

                               foreach (XmlElement childrenNode in parentNode)
                               {
                                   int structureSectionId = 0;
                                   int orgId = 0;
                                   string orgName = string.Empty;

                                   string structureCode = string.Empty;
                                   string structureName = string.Empty;

                                   string traversalType = string.Empty;

                                   if ((childrenNode != null) && childrenNode.HasAttribute("StructureSectionId"))
                                   {
                                       structureSectionId = Convert.ToInt32(childrenNode.Attributes["StructureSectionId"].InnerText);
                                   }

                                   if ((childrenNode != null) && childrenNode.HasAttribute("TraversalType"))
                                   {
                                       traversalType = Convert.ToString(childrenNode.Attributes["TraversalType"].InnerText);
                                   }

                                   foreach (XmlElement xmlElement in childrenNode)
                                   {
                                       if (xmlElement.Name == "ESRN")
                                       {
                                           structureCode = xmlElement.InnerText;
                                       }

                                       if (xmlElement.Name == "Name")
                                       {
                                           structureName = xmlElement.InnerText;
                                       }

                                       if (xmlElement.Name == "Appraisal")
                                       {

                                           string suitability = string.Empty;
                                           string organisationName = string.Empty;
                                           string sectionSuitability = string.Empty;
                                           string sectionDescription = string.Empty;

                                           string childSectionSuitability = string.Empty;
                                           string childSectionTestClass = string.Empty;
                                           string childSectionTestIdentity = string.Empty;
                                           string childSectionResultDetails = string.Empty;

                                           foreach (XmlElement xmlElement1 in xmlElement.ChildNodes)
                                           {
                                               if (xmlElement1.Name == "Suitability")
                                               {
                                                   suitability = xmlElement1.InnerText;
                                               }

                                               if (xmlElement1.Name == "Organisation")
                                               {
                                                   organisationName = xmlElement1.InnerText;
                                               }

                                               if (xmlElement1.Name == "IndividualSectionSuitability")
                                               {
                                                   foreach (XmlElement individualSectionElement in xmlElement1.ChildNodes)
                                                   {
                                                       if (individualSectionElement.Name == "Suitability")
                                                       {
                                                           sectionSuitability = individualSectionElement.InnerText;
                                                       }

                                                       if (individualSectionElement.Name == "SectionDescription")
                                                       {
                                                           sectionDescription = individualSectionElement.InnerText;
                                                       }

                                                       foreach (XmlElement childIndividualSection in individualSectionElement.ChildNodes)
                                                       {
                                                           if (childIndividualSection.Name == "Suitability")
                                                           {
                                                               childSectionSuitability = childIndividualSection.InnerText;
                                                           }

                                                           if (childIndividualSection.InnerText == "TestClass")
                                                           {
                                                               childSectionTestClass = childIndividualSection.InnerText;
                                                           }

                                                           if (childIndividualSection.InnerText == "TestIdentity")
                                                           {
                                                               childSectionTestIdentity = childIndividualSection.InnerText;
                                                           }

                                                           if (childIndividualSection.InnerText == "ResultDetails")
                                                           {
                                                               childSectionResultDetails = childIndividualSection.InnerText;
                                                           }

                                                           resultStructure = new AggreedRouteXSD.SuitabilityResultStructure
                                                           {
                                                               Suitability = childSectionSuitability == "suitable" ? AggreedRouteXSD.SuitabilityType.suitable : childSectionSuitability == "marginal" ? AggreedRouteXSD.SuitabilityType.marginal : childSectionSuitability == "unsuitable" ? AggreedRouteXSD.SuitabilityType.unsuitable : AggreedRouteXSD.SuitabilityType.unknown,
                                                               ResultDetails = childSectionResultDetails,
                                                               TestClass = childSectionTestClass == "dimensional constraint" ? AggreedRouteXSD.SuitabilityTestClassType.dimensionalconstraint : AggreedRouteXSD.SuitabilityTestClassType.ICA,
                                                               TestIdentity = childSectionTestIdentity
                                                           };

                                                           resultStructureList.Add(resultStructure);
                                                       }

                                                       structureSection = new AggreedRouteXSD.StructureSectionSuitabilityStructure
                                                       {
                                                           Suitability = sectionSuitability == "suitable" ? AggreedRouteXSD.SuitabilityType.suitable : sectionSuitability == "marginal" ? AggreedRouteXSD.SuitabilityType.marginal : sectionSuitability == "unsuitable" ? AggreedRouteXSD.SuitabilityType.unsuitable : AggreedRouteXSD.SuitabilityType.unknown,
                                                           SectionDescription = sectionDescription,
                                                           IndividualResult = resultStructureList.ToArray()
                                                       };

                                                       individaulSection.Add(structureSection);
                                                   }
                                               }
                                           }

                                           appraisalRecord = new AggreedRouteXSD.StructureSuitabilityStructure()
                                           {
                                               Suitability = suitability == "suitable" ? AggreedRouteXSD.SuitabilityType.suitable : suitability == "marginal" ? AggreedRouteXSD.SuitabilityType.marginal : suitability == "unsuitable" ? AggreedRouteXSD.SuitabilityType.unsuitable : AggreedRouteXSD.SuitabilityType.unknown,
                                               Organisation = organisationName,
                                               OrganisationId = orgId,
                                               IndividualSectionSuitability = individaulSection.ToArray()
                                           };


                                       }

                                       structdetails = new AggreedRouteXSD.AffectedStructureStructure1
                                       {
                                           StructureSectionId = structureSectionId,
                                           ESRN = structureCode,
                                           TraversalType = traversalType.ToLower() == "overbridge" ? AggreedRouteXSD.StructureTraversalType.overbridge : traversalType.ToLower() == "underbridge" ? AggreedRouteXSD.StructureTraversalType.underbridge : traversalType.ToLower() == "levelcrossing" ? AggreedRouteXSD.StructureTraversalType.levelcrossing : AggreedRouteXSD.StructureTraversalType.archedoverbridge,
                                           Name = structureName,
                                           Appraisal = appraisalRecord

                                       };
                                   }

                                   affectedstructList.Add(structdetails);

                               }

                               ass.AreMyResponsibilityOnly = false;
                               ass.IsStructureOwner = false;
                               ass.Structure = affectedstructList.ToArray();
                               prp.Structures = ass;
                           }
                           #endregion

                           DrivingInstructionModel DrivingInstructionInfo = GetDrivingInstructionStructures(orderNumber, version_no, haulierMnemonic, projectNumber, userSchema);

                           #region Road
                           prp.Roads = getRoadDetails(DrivingInstructionInfo, routePartNo);
                           #endregion

                           #region DrivingInstructions
                           int incdoccaution = 0;
                           incdoccaution = records.GetShortOrDefault("include_dock_caution");

                           prp.DrivingInstructions = getDrivingDetails(DrivingInstructionInfo, incdoccaution, routePartNo);
                           #endregion

                           #region Distance
                           VariableMetricImperialDistancePairStructure vmdps = new VariableMetricImperialDistancePairStructure();

                           VariableMetricDistanceStructure vmds = new VariableMetricDistanceStructure();
                           vmds.Item = Convert.ToString(TotalDistanceMetric);
                           vmdps.Metric = vmds;

                           VariableImperialDistanceStructure vids = new VariableImperialDistanceStructure();
                           vids.Item = Convert.ToString(TotalDistanceImperial);
                           vmdps.Imperial = vids;

                           prp.Distance = vmdps;
                           #endregion

                           prps.RoadPart = prp;
                           instance.RoutePart = prps;

                           legNumber++;
                       }
                   }
            );
            return rpsrplpList.ToArray();
        }

        /// <summary>
        /// get journey from for revised agreement document
        /// </summary>
        /// <param name="orderNumber">order number</param>
        /// <param name="esDAlRefNo">esdal ref no</param>
        /// <returns></returns>
        public static AggreedRouteXSD.SimplifiedRoutePointStructure GetSimplifiedRoutePointStart(string orderNumber, string haulierMnemonic, string projectNumber, string versionNumber, string userSchema = UserSchema.Portal, long routePartId = 0)
        {
            string messg = "RevisedAgreementDAO/GetSimplifiedRoutePointStart?orderNumber=" + orderNumber + ", haulierMnemonic=" + haulierMnemonic + ", projectNumber=" + projectNumber + ", versionNumber=" + versionNumber + ", userSchema=" + userSchema + ", routePartId=" + routePartId;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            AggreedRouteXSD.SimplifiedRoutePointStructure srps = new AggreedRouteXSD.SimplifiedRoutePointStructure();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                srps,
                userSchema + ".GET_STARTPOINT",
                parameter =>
                {
                    parameter.AddWithValue("p_ORDER_NUMBER", string.Empty, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_HAULIER_MNEMONIC", haulierMnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PROJECT_NUMBER", projectNumber, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VERSION_NUMBER", versionNumber, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ROUTEPART_ID", routePartId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 (records, instance) =>
                 {
                     instance.Description = records.GetStringOrDefault("DESCR");
                     instance.GridRef = Convert.ToString(records.GetDecimalOrDefault("road_point_geometry.SDO_POINT.X")) + ',' + Convert.ToString(records.GetDecimalOrDefault("road_point_geometry.SDO_POINT.Y"));
                 }
                );

            return srps;
        }

        /// <summary>
        /// get journey to for revised agreement document
        /// </summary>
        /// <param name="orderNumber">order number</param>
        /// <param name="esDAlRefNo">esdal ref no</param>
        /// <returns></returns>
        public static AggreedRouteXSD.SimplifiedRoutePointStructure GetSimplifiedRoutePointEnd(string orderNumber, string haulierMnemonic, string projectNumber, string versionNumber, string userSchema = UserSchema.Portal, long routePartId = 0)
        {
            string messg = "RevisedAgreementDAO/GetSimplifiedRoutePointEnd?orderNumber=" + orderNumber + ", haulierMnemonic=" + haulierMnemonic + ", projectNumber=" + projectNumber + ", versionNumber=" + versionNumber + ", userSchema=" + userSchema + ", routePartId=" + routePartId;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));
            AggreedRouteXSD.SimplifiedRoutePointStructure srps = new AggreedRouteXSD.SimplifiedRoutePointStructure();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                srps,
                userSchema + ".GET_ENDPOINT",
                parameter =>
                {
                    parameter.AddWithValue("p_ORDER_NUMBER", string.Empty, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_HAULIER_MNEMONIC", haulierMnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PROJECT_NUMBER", projectNumber, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VERSION_NUMBER", versionNumber, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ROUTEPART_ID", routePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 (records, instance) =>
                 {
                     instance.Description = records.GetStringOrDefault("DESCR");
                     instance.GridRef = Convert.ToString(records.GetDecimalOrDefault("road_point_geometry.SDO_POINT.X")) + ',' + Convert.ToString(records.GetDecimalOrDefault("road_point_geometry.SDO_POINT.Y"));
                 }
                );

            return srps;
        }

        /// <summary>
        /// get vehicle component detail for revised agreement document
        /// </summary>
        /// <param name="orderNumber">order number</param>
        /// <param name="esDAlRefNo">esdal ref no</param>
        /// <param name="vehicleID">vehicle id</param>
        /// <returns></returns>
        public static List<VehComponentAxles> GetVehicleComponentAxles(string orderNumber, int version_no, string haulierMnemonic, string projectNumber, long vehicleID, string userSchema = UserSchema.Portal)
        {
            string messg = "RevisedAgreementDAO/GetVehicleComponentAxles?orderNumber=" + orderNumber + ", version_no=" + version_no + ", haulierMnemonic=" + haulierMnemonic + ", projectNumber=" + projectNumber + ", vehicleID=" + vehicleID + ", userSchema=" + userSchema;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            List<VehComponentAxles> componentAxleList = new List<VehComponentAxles>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                componentAxleList,
                  userSchema + ".GET_AXLE_CONFIGURATION",
                parameter =>
                {
                    parameter.AddWithValue("p_ORDER_NUMBER", string.Empty, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_HAULIER_MNEMONIC", haulierMnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PROJECT_NUMBER", projectNumber, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VERSION_NUMBER", version_no, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VEHICLE_ID", vehicleID, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.AxleCount = records.GetInt16OrDefault("axle_count");
                        instance.ComponentId = records.GetLongOrDefault("Component_Id");
                        instance.NextAxleDist = records.GetFloatOrDefault("NEXT_AXLE_DIST");
                        instance.TyreSize = records.GetStringOrDefault("tyre_size");
                        instance.Weight = records.GetFloatOrDefault("weight");
                        instance.WheelCount = records.GetInt16OrDefault("wheel_count");
                        instance.AxleNumber = records.GetInt16OrDefault("AXLE_NO");
                        instance.WheelSpacingList = records.GetStringOrDefault("wheel_spacing_list");
                        instance.AxleSpacingToFollowing = records.GetDoubleOrDefault("SPACE_TO_FOLLOWING");
                    }
            );
            return componentAxleList;
        }

        /// <summary>
        /// get structure xml from the database
        /// </summary>
        /// <param name="orderNumber">order number</param>
        /// <param name="esDAlRefNo">esdal ref no</param>
        /// <returns></returns>
        public static StructuresModel GetStructuresDetail(string orderNumber, string esDAlRefNo, string userSchema)
        {
            string messg = "RevisedAgreementDAO/GetStructuresDetail?orderNumber=" + orderNumber + ", esDAlRefNo=" + esDAlRefNo + ", userSchema=" + userSchema;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            StructuresModel structuresDetail = new StructuresModel();

            string[] esdalRefPro = esDAlRefNo.Split('/');
            string mnemonic = string.Empty;
            string esdalrefnum = string.Empty;
            string version = string.Empty;

            if (esdalRefPro.Length > 0)
            {
                mnemonic = Convert.ToString(esdalRefPro[0]);
                esdalrefnum = Convert.ToString(esdalRefPro[1].ToUpper().Replace("S", ""));
                version = Convert.ToString(esdalRefPro[2].ToUpper().Replace("S", ""));
            }

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                structuresDetail,
                userSchema + ".GET_DRIVINGINSTRUCTIONS_DETAIL",
                parameter =>
                {
                    parameter.AddWithValue("p_ORDER_NUMBER", string.Empty, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAULIER_MNEMONIC", mnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_PROJECT_NUMBER", esdalrefnum, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VERSION_NUMBER", version, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       instance.AffectedStructures = records.GetByteArrayOrNull("AFFECTED_STRUCTURES");
                   }
            );
            return structuresDetail;
        }

        /// <summary>
        /// get driving instruction xml from the database
        /// </summary>
        /// <param name="orderNumber">order number</param>
        /// <param name="esDAlRefNo">esdal ref no</param>
        /// <returns></returns>
        public static DrivingInstructionModel GetDrivingInstructionStructures(string orderNumber, int version_no, string haulierMnemonic, string projectNumber, string userSchema)
        {
            string messg = "RevisedAgreementDAO/GetDrivingInstructionStructures?orderNumber=" + orderNumber + ", version_no=" + version_no + ", haulierMnemonic=" + haulierMnemonic + ", projectNumber=" + projectNumber + ", userSchema=" + userSchema;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            DrivingInstructionModel structuresDetail = new DrivingInstructionModel();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                structuresDetail,
                userSchema + ".GET_DRIVINGINSTRUCTIONS_DETAIL",
                parameter =>
                {
                    parameter.AddWithValue("p_ORDER_NUMBER", string.Empty, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_HAULIER_MNEMONIC", haulierMnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PROJECT_NUMBER", projectNumber, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VERSION_NUMBER", version_no, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       instance.DrivingInstructions = records.GetByteArrayOrNull("DRIVING_INSTRUCTIONS");
                   }
            );
            return structuresDetail;
        }

        /// <summary>
        /// generate roads node detail based on xml
        /// </summary>
        /// <param name="DrivingInstructionInfo">DrivingInstructionModel</param>
        /// <returns></returns>
        public static AffectedRoadsStructure getRoadDetails(DrivingInstructionModel DrivingInstructionInfo, int legNumber)
        {
            string messg = "RevisedAgreementDAO/getRoadDetails?legNumber=" + legNumber;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            XNamespace di = "http://www.esdal.com/schemas/core/drivinginstruction";

            string recipientXMLInformation = string.Empty;

            Byte[] DrivingInstructionArray = DrivingInstructionInfo.DrivingInstructions;
            AffectedRoadsStructure affRoadStruct = new AffectedRoadsStructure();

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                recipientXMLInformation = Encoding.UTF8.GetString(DrivingInstructionArray, 0, DrivingInstructionArray.Length);

                xmlDoc.LoadXml(recipientXMLInformation);
            }
            catch 
            {
                //Some data is stored in gzip format, so we need to unzip then load it.
                byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(DrivingInstructionArray);

                recipientXMLInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);

                xmlDoc.LoadXml(recipientXMLInformation);
            }

            XmlNodeList rootNode = xmlDoc.GetElementsByTagName("DrivingInstructions");

            string latestXML = string.Empty;

            if (rootNode.Count >= 1)
            {
                latestXML = "<NavigationList xmlns=\"http://www.esdal.com/schemas/core/drivinginstruction\">" + rootNode[legNumber - 1].InnerXml + "</NavigationList>";

                xmlDoc.LoadXml(latestXML);
            }

            #region Code for Imperial and Metric distance RM#4998
            XDocument XMLData = XDocument.Parse(latestXML);

            var mainSubPart = (from el in XMLData.Root.Elements(di + "SubPartListPosition")
                               select el.Elements(di + "SubPart").Elements(di + "AlternativeListPosition")).ToList();

            var dataSubPart = from el in XMLData.Root.Elements(di + "SubPartListPosition")
                              select el.Elements(di + "SubPart").Elements(di + "AlternativeListPosition").Count();

            var lengthData = dataSubPart.ToList();

            string regx = "^";
            string startString = string.Empty;
            string endString = string.Empty;

            #region regular expression code
            for (int regxinit = 0; regxinit < lengthData.Count; regxinit++)
            {
                if (lengthData[regxinit] == 1)
                {
                    regx += "1";
                }
                else if (lengthData[regxinit] == 2)
                {
                    regx += "[1-2]";
                }

                startString += "1";
                endString += "2";
            }
            regx += "$";
            long startNumber = Int64.Parse(startString);
            long endNumber = Int64.Parse(endString);

            Regex regex = new Regex(regx);

            List<long> finalValue = new List<long>();
            for (long regxinit = startNumber; regxinit <= endNumber; regxinit++)
            {
                if (regex.IsMatch(regxinit.ToString()))
                {
                    finalValue.Add(regxinit);
                }
            }
            #endregion End


            List<decimal> listImperial = new List<decimal>();
            List<decimal> listMetric = new List<decimal>();
            string ImperialWithOR = String.Empty, MetricWithOR = String.Empty;

            for (int count = 0; count < finalValue.Count; count++)
            {
                decimal finalDisplayMetric = 0, finalDisplayImperial = 0;
                decimal TotalDisplayMetric = 0, TotalDisplayImperial = 0;
                long PathPosition = finalValue[count];
                int countData = lengthData.Count - 1;
                while (PathPosition % 10 != 0)
                {
                    int Partpos = Convert.ToInt32((PathPosition % 10)) - 1;


                    var CurrentData = mainSubPart[countData].ToList()[Partpos];

                    var AllDistance = (from el in CurrentData.Elements(di + "Alternative").Elements(di + "InstructionListPosition").Elements(di + "Instruction").Elements(di + "Navigation")
                                       where el.Elements(di + "Instruction").FirstOrDefault().Value.ToLower().Contains("continue ")
                                       select new NavigationXML
                                       {
                                           MeasuredMetric = el.Elements(di + "Distance") != null &&
                                                     el.Elements(di + "Distance").Elements(di + "MeasuredMetric") != null &&
                                                     el.Elements(di + "Distance").Elements(di + "MeasuredMetric").FirstOrDefault() != null ?
                                                     Convert.ToDecimal(el.Elements(di + "Distance").Elements(di + "MeasuredMetric").FirstOrDefault().Value) : 0,

                                           DisplayMetric = el.Elements(di + "Distance") != null &&
                                                            el.Elements(di + "Distance").Elements(di + "DisplayMetric") != null &&
                                                            el.Elements(di + "Distance").Elements(di + "DisplayMetric").FirstOrDefault() != null ?
                                                            Convert.ToDecimal(el.Elements(di + "Distance").Elements(di + "DisplayMetric").FirstOrDefault().Value) : 0,

                                           DisplayImperial = el.Elements(di + "Distance") != null &&
                                                            el.Elements(di + "Distance").Elements(di + "DisplayImperial") != null &&
                                                            el.Elements(di + "Distance").Elements(di + "DisplayImperial").FirstOrDefault() != null ?
                                                            Math.Round(Convert.ToDecimal(el.Elements(di + "Distance").Elements(di + "DisplayImperial").FirstOrDefault().Value), 2) : 0
                                       }).ToList();

                    TotalDisplayMetric = AllDistance.Sum(x => x.DisplayMetric);
                    TotalDisplayImperial = AllDistance.Sum(x => x.DisplayImperial);

                    finalDisplayMetric = finalDisplayMetric + TotalDisplayMetric;
                    finalDisplayImperial = finalDisplayImperial + TotalDisplayImperial;

                    PathPosition = PathPosition / 10;
                    countData--;
                }

                listImperial.Add(finalDisplayImperial);
                listMetric.Add(finalDisplayMetric);

            }


            for (int ImperInit = 0; ImperInit < listImperial.Count; ImperInit++)
            {
                if (ImperInit == 0)
                {
                    ImperialWithOR = listImperial[ImperInit].ToString();
                }
                else
                {
                    ImperialWithOR += " OR " + listImperial[ImperInit].ToString();
                }
            }

            for (int MetricInit = 0; MetricInit < listMetric.Count; MetricInit++)
            {
                if (MetricInit == 0)
                {
                    MetricWithOR = listMetric[MetricInit].ToString();
                }
                else
                {
                    MetricWithOR += " OR " + listMetric[MetricInit].ToString();
                }
            }
            TotalDistanceMetric = MetricWithOR;
            TotalDistanceImperial = ImperialWithOR;
            #endregion end code

            XmlNodeList parentNode = xmlDoc.GetElementsByTagName("SubPart");

            if (parentNode.Count >= 1)
            {
                List<AffectedRoadsStructureRouteSubPartListPosition> arsrslpList = new List<AffectedRoadsStructureRouteSubPartListPosition>();
                decimal totalsmeric = 0;
                decimal totalsimperial = 0;
                foreach (XmlElement element in parentNode) // For SubPart
                {
                    foreach (XmlElement alternativeElement in element) // For Navigation
                    {
                        NavigationXML navigationXML;
                        List<NavigationXML> navigationXMLs = new List<NavigationXML>();

                        AffectedRoadsSubPartStructure arsps = new AffectedRoadsSubPartStructure();
                        AffectedRoadsStructureRouteSubPartListPosition arsrslp = new AffectedRoadsStructureRouteSubPartListPosition();

                        foreach (XmlElement newNode in alternativeElement)
                        {
                            foreach (XmlElement childrenNode in newNode)
                            {
                                if (childrenNode.InnerText.ToLower().Contains("continue "))
                                {
                                    navigationXML = new NavigationXML();
                                    navigationXML.Instruction = childrenNode.ChildNodes[0] == null ? "" : childrenNode.ChildNodes[0].ChildNodes[0] == null ? "" : childrenNode.ChildNodes[0].ChildNodes[0].ChildNodes[0] == null ? "" : Convert.ToString(childrenNode.ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText);
                                    navigationXML.MeasuredMetric = childrenNode.ChildNodes[0] == null ? 0 : childrenNode.ChildNodes[0].ChildNodes[0] == null ? 0 : childrenNode.ChildNodes[0].ChildNodes[0].ChildNodes[1] == null ? 0 : childrenNode.ChildNodes[0].ChildNodes[0].ChildNodes[1].ChildNodes[0] == null ? 0 : Convert.ToDecimal(childrenNode.ChildNodes[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].InnerText);
                                    navigationXML.DisplayMetric = childrenNode.ChildNodes[0] == null ? 0 : childrenNode.ChildNodes[0].ChildNodes[0] == null ? 0 : childrenNode.ChildNodes[0].ChildNodes[0].ChildNodes[1] == null ? 0 : childrenNode.ChildNodes[0].ChildNodes[0].ChildNodes[1].ChildNodes[1] == null ? 0 : Convert.ToDecimal(childrenNode.ChildNodes[0].ChildNodes[0].ChildNodes[1].ChildNodes[1].InnerText);
                                    navigationXML.DisplayImperial = childrenNode.ChildNodes[0] == null ? 0 : childrenNode.ChildNodes[0].ChildNodes[0] == null ? 0 : childrenNode.ChildNodes[0].ChildNodes[0].ChildNodes[1] == null ? 0 : childrenNode.ChildNodes[0].ChildNodes[0].ChildNodes[1].ChildNodes[2] == null ? 0 : Convert.ToDecimal(childrenNode.ChildNodes[0].ChildNodes[0].ChildNodes[1].ChildNodes[2].InnerText);

                                    if (navigationXML.MeasuredMetric > 0 && navigationXML.DisplayImperial > 0 && navigationXML.DisplayMetric > 0)
                                    {
                                        navigationXMLs.Add(navigationXML);
                                    }
                                }
                            }

                            List<NavigationXML> finalNavigationXMLs = new List<NavigationXML>();

                            if (navigationXMLs.Count > 0)
                            {
                                
                                string previousInstruction = string.Empty;
                                string currentInstruction = string.Empty;
                                for (int count = 0; count < navigationXMLs.Count; count++)
                                {
                                    currentInstruction = navigationXMLs[count].Instruction;
                                    if (currentInstruction.ToLower() == previousInstruction.ToLower())
                                    {
                                        var matchNavigationElement = finalNavigationXMLs.Where(x => x.Instruction.ToLower() == currentInstruction.ToLower()).LastOrDefault();
                                        matchNavigationElement.MeasuredMetric = matchNavigationElement.MeasuredMetric + navigationXMLs[count].MeasuredMetric;
                                        matchNavigationElement.DisplayMetric = matchNavigationElement.DisplayMetric + navigationXMLs[count].DisplayMetric;
                                        matchNavigationElement.DisplayImperial = matchNavigationElement.DisplayImperial + navigationXMLs[count].DisplayImperial;
                                    }
                                    else
                                    {
                                        finalNavigationXMLs.Add(navigationXMLs[count]);
                                    }
                                    previousInstruction = currentInstruction;
                                }
                            }




                            foreach (NavigationXML xml in finalNavigationXMLs)
                            {
                                xml.YardMiles = Convert.ToString(Math.Round(xml.DisplayImperial, 2));

                                if (xml.DisplayImperial > 0)
                                {
                                    totalsimperial = totalsimperial + xml.DisplayImperial;
                                }

                                if (xml.DisplayMetric > 0)
                                {
                                    totalsmeric = totalsmeric + xml.DisplayMetric;
                                }
                            }

                            CheckTotalDistanceImperial = totalsmeric;

                            CheckTotalDistanceImperial = totalsimperial;

                            affRoadStruct.IsBroken = false;

                            List<AffectedRoadsSubPartStructurePathListPosition> arspslpList = new List<AffectedRoadsSubPartStructurePathListPosition>();
                            List<AffectedRoadsPathStructureRoadTraversalListPosition> arpsrtlpList = new List<AffectedRoadsPathStructureRoadTraversalListPosition>();

                            AffectedRoadsSubPartStructurePathListPosition arspslp = new AffectedRoadsSubPartStructurePathListPosition();

                            foreach (NavigationXML xml in finalNavigationXMLs)
                            {
                                if (xml.Instruction != string.Empty || (xml.DisplayMetric != 0 && xml.YardMiles != string.Empty))
                                {
                                    AffectedRoadsPathStructureRoadTraversalListPosition arpsrtlp1 = new AffectedRoadsPathStructureRoadTraversalListPosition();

                                    AffectedRoadStructure ars1 = new AffectedRoadStructure();
                                    ars1.IsMyResponsibility = false;
                                    ars1.IsStartOfMyResponsibility = false;

                                    List<AffectedRoadStructure> affrdconsStruList = new List<AffectedRoadStructure>();
                                    AggreedRouteXSD.RoadIdentificationStructure ris = new AggreedRouteXSD.RoadIdentificationStructure();
                                    if (xml.Instruction != string.Empty)
                                    {
                                        ris.Name = xml.Instruction.Replace("Continue", "").Replace("for", "");
                                        ars1.RoadIdentity = ris;
                                    }

                                    MetricImperialDistancePairStructure metimpstru = new MetricImperialDistancePairStructure();

                                    metimpstru.Metric = Convert.ToString(xml.DisplayMetric);
                                    metimpstru.Imperial = xml.YardMiles;

                                    if (xml.DisplayMetric != 0 && xml.YardMiles != string.Empty)
                                    {
                                        ars1.Distance = metimpstru;
                                    }

                                    arpsrtlp1.RoadTraversal = ars1;

                                    arpsrtlpList.Add(arpsrtlp1);
                                }
                            }

                            arspslp.Path = arpsrtlpList.ToArray();
                            arspslpList.Add(arspslp);
                            arsps.PathListPosition = arspslpList.ToArray();
                        }

                        arsrslp.RouteSubPart = arsps;
                        arsrslpList.Add(arsrslp);

                    }

                    affRoadStruct.RouteSubPartListPosition = arsrslpList.ToArray();
                }
            }

            return affRoadStruct;
        }

        /// <summary>
        /// generate driving instruction node detail based on xml
        /// </summary>
        /// <param name="DrivingInstructionInfo">DrivingInstructionModel</param>
        /// <param name="DocCaution">DocK Caution</param>
        /// <returns></returns>
        public static AggreedRouteXSD.DrivingInstructionsStructure getDrivingDetails(DrivingInstructionModel DrivingInstructionInfo, int incdoccaution, int legNumber)
        {
            string messg = "RevisedAgreementDAO/getDrivingDetails?incdoccaution=" + incdoccaution + ", legNumber=" + legNumber;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            AggreedRouteXSD.DrivingInstructionsStructure dis = new AggreedRouteXSD.DrivingInstructionsStructure();
            string recipientXMLInformation = string.Empty;

            Byte[] DrivingInstructionArray = DrivingInstructionInfo.DrivingInstructions;

            if (DrivingInstructionArray != null)
            {
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    recipientXMLInformation = Encoding.UTF8.GetString(DrivingInstructionArray, 0, DrivingInstructionArray.Length);

                    recipientXMLInformation = recipientXMLInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "#bst#");
                    recipientXMLInformation = recipientXMLInformation.Replace("</Bold>", "#be#");

                    xmlDoc.LoadXml(recipientXMLInformation);
                }
                catch 
                {
                    //Some data is stored in gzip format, so we need to unzip then load it.
                    byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(DrivingInstructionArray);

                    recipientXMLInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);

                    recipientXMLInformation = recipientXMLInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "#bst#");
                    recipientXMLInformation = recipientXMLInformation.Replace("</Bold>", "#be#");

                    xmlDoc.LoadXml(recipientXMLInformation);
                }

                string legDetailsMessg = string.Empty;
                string instruction = string.Empty;
                int MeasuredMetric = 0;
                int DisplayMetric = 0;
                int DisplayImperial = 0;
                string PointType = string.Empty;
                string Description = string.Empty;
                ulong X = 0;
                ulong Y = 0;
                int ComparisonId = 0;
                int Id = 0;
                string LegNumber = string.Empty;
                string Name = string.Empty;
                bool MoterwayCaution = false;
                //RM#3646 Start
                string AnnotationType = string.Empty;
                int AnnotationId = 0;
                string Text = string.Empty;
                bool isDrivingInstruction = false;
                //RM#3646 End
                //RM#3646 Start
                bool IsApplicable = false;
                string Action = string.Empty;
                string CautionedEntity = string.Empty;
                int CautionId = 0;
                bool CautionIdSpecified = false;
                string Conditions = string.Empty;
                string ConstrainingAttribute = string.Empty;
                string Contact = string.Empty;
                string ECRN = string.Empty;
                string ESRN = string.Empty;
                string Type = string.Empty;
                string ConstraintName = string.Empty;

                string OrganisationName = string.Empty;
                int OrganisationId = 0;

                string FullName = string.Empty;

                string TelephoneNumber = string.Empty;
                string FaxNumber = string.Empty;
                string EmailAddress = string.Empty;

                string StructureName = string.Empty;
                int SectionId = 0;

                decimal EncounteredMetric = 0;
                decimal EncounteredImperial = 0;
                decimal EncounterMeasureMetric = 0;

                //RM#3646 End
                XmlNodeList node = xmlDoc.GetElementsByTagName("DrivingInstructions");

                string latestXML = string.Empty;

                if (node.Count >= 1)
                {
                    latestXML = "<DrivingInstructions xmlns=\"http://www.esdal.com/schemas/core/drivinginstruction\">" + node[legNumber - 1].InnerXml + "</DrivingInstructions>";

                    xmlDoc.LoadXml(latestXML);
                }

                string motorwayCautionDescription = string.Empty;

                foreach (XmlElement xmlElements in node)//Instruction
                {
                    foreach (XmlElement xmlElement1 in xmlElements.ChildNodes)//Instruction
                    {
                        
                        if (xmlElement1.Name == "MotorwayCautionDescription")
                        {
                            motorwayCautionDescription = xmlElement1.InnerText;
                        }
                    }
                }

                foreach (XmlElement xmlElement1 in node)//Instruction
                {
                    if (xmlElement1.Name == "DrivingInstructions")
                    {
                        if ((xmlElement1 != null) && xmlElement1.HasAttribute("ComparisonId"))
                        {
                            ComparisonId = Convert.ToInt32(xmlElement1.Attributes["ComparisonId"].InnerText);
                        }
                        if ((xmlElement1 != null) && xmlElement1.HasAttribute("Id"))
                        {
                            Id = Convert.ToInt32(xmlElement1.Attributes["Id"].InnerText);
                        }
                        if (xmlElement1["LegNumber"] != null)
                        {

                            LegNumber = xmlElement1["LegNumber"].InnerText;
                            legDetailsMessg = "RevisedAgreementDAO/getDrivingDetails-LegNumber=" + LegNumber;
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(legDetailsMessg + "; In DrivingInstructions Element"));
                        }
                        if (xmlElement1["Name"] != null)
                        {

                            Name = xmlElement1["Name"].InnerText;
                            legDetailsMessg = "RevisedAgreementDAO/getDrivingDetails-Name=" + Name;
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(legDetailsMessg + "; In DrivingInstructions Element"));
                        }
                    }
                }

                node = xmlDoc.GetElementsByTagName("SubPart");

                if (node.Count >= 1)
                {
                    List<AggreedRouteXSD.DrivingInstructionsStructureSubPartListPosition> disslpList = new List<AggreedRouteXSD.DrivingInstructionsStructureSubPartListPosition>();

                    List<DrivingInstructionModel> lstDrivingInstruModel;

                    foreach (XmlElement newElement in node) // For SubPartListPosition
                    {

                        AggreedRouteXSD.DrivingInstructionSubPartStructureAlternativeListPosition dispsalp1 = new AggreedRouteXSD.DrivingInstructionSubPartStructureAlternativeListPosition();
                        List<AggreedRouteXSD.DrivingInstructionSubPartStructureAlternativeListPosition> dispsalpList = new List<AggreedRouteXSD.DrivingInstructionSubPartStructureAlternativeListPosition>();

                        AggreedRouteXSD.DrivingInstructionsStructureSubPartListPosition disslp1 = new AggreedRouteXSD.DrivingInstructionsStructureSubPartListPosition();

                        foreach (XmlElement element in newElement) // For SubPart
                        {
                            foreach (XmlElement alternativeElement in element) // For AlternativeListPosition
                            {
                                AggreedRouteXSD.DrivingInstructionListStructure dils = new AggreedRouteXSD.DrivingInstructionListStructure();
                                List<DrivingInstructionModel> drivingInsList = new List<DrivingInstructionModel>();

                                int alternativeNo = 0;
                                string alternativeName = string.Empty;

                                foreach (XmlElement parentName in alternativeElement) // For Alternative
                                {

                                    if (parentName.Name == "AlternativeDescription")
                                    {
                                        foreach (XmlElement alternativeNode in parentName.ChildNodes)
                                        {
                                            if (alternativeNode.Name == "AlternativeNo")
                                            {
                                                alternativeNo = Convert.ToInt32(alternativeNode.InnerText);
                                            }

                                            if (alternativeNode.Name == "Description")
                                            {
                                                alternativeName = Convert.ToString(alternativeNode.InnerText);
                                            }
                                        }
                                    }

                                    if (parentName.Name == "InstructionListPosition")
                                    {
                                        foreach (XmlElement xmlElement1 in parentName.ChildNodes)//Instruction
                                        {
                                            lstDrivingInstruModel = new List<DrivingInstructionModel>();

                                            foreach (XmlElement xmlElement2 in xmlElement1.ChildNodes)//Instruction
                                            {
                                                if (xmlElement2.Name == "Navigation")
                                                {
                                                    foreach (XmlElement xmlElement3 in xmlElement2.ChildNodes)
                                                    {
                                                        if (xmlElement3.Name == "Instruction")//Instruction
                                                            instruction = xmlElement3.InnerText;

                                                        if (xmlElement3.Name == "Distance")//Distance
                                                        {
                                                            foreach (XmlElement xmlElement4 in xmlElement3.ChildNodes)
                                                            {
                                                                if (xmlElement4.Name == "MeasuredMetric")
                                                                    MeasuredMetric = Convert.ToInt32(xmlElement4.InnerText);
                                                                if (xmlElement4.Name == "DisplayMetric")
                                                                    DisplayMetric = Convert.ToInt32(xmlElement4.InnerText);
                                                                if (xmlElement4.Name == "DisplayImperial")
                                                                    DisplayImperial = Convert.ToInt32(xmlElement4.InnerText);
                                                            }
                                                        }
                                                    }
                                                }

                                                if (xmlElement2.Name == "NoteListPosition")
                                                {
                                                    foreach (XmlElement xmlElement3 in xmlElement2.ChildNodes) //Note
                                                    {
                                                        foreach (XmlElement xmlElement4 in xmlElement3.ChildNodes) //Content
                                                        {
                                                            if (xmlElement4.Name == "Content")
                                                            {
                                                                foreach (XmlElement xmlElement5 in xmlElement4.ChildNodes)
                                                                {
                                                                    if (xmlElement5.Name == "RoutePoint")
                                                                    {
                                                                        if ((xmlElement5 != null) && xmlElement5.HasAttribute("PointType"))
                                                                        {
                                                                            PointType = xmlElement5.Attributes["PointType"].InnerText;
                                                                        }
                                                                        if (xmlElement5["Description"] != null)
                                                                            Description = xmlElement5["Description"].InnerText;
                                                                    }
                                                                    //RM#3646 - Start
                                                                    if (xmlElement5.Name == "Annotation")
                                                                    {
                                                                        if ((xmlElement5 != null) && xmlElement5.HasAttribute("AnnotationType"))
                                                                        {
                                                                            AnnotationType = xmlElement5.Attributes["AnnotationType"].InnerText;
                                                                        }

                                                                        if ((xmlElement5 != null) && xmlElement5.HasAttribute("AnnotationId"))
                                                                        {
                                                                            AnnotationId = Convert.ToInt32(xmlElement5.Attributes["AnnotationId"].InnerText);
                                                                        }

                                                                        if ((xmlElement5 != null) && xmlElement5.HasAttribute("IsDrivingInstructionAnnotation"))
                                                                        {
                                                                            isDrivingInstruction = Convert.ToBoolean(xmlElement5.Attributes["IsDrivingInstructionAnnotation"].InnerText);
                                                                        }

                                                                        if (xmlElement5 != null)
                                                                        {

                                                                            Text = xmlElement5.InnerText;
                                                                        }
                                                                        if (xmlElement5 != null && !string.IsNullOrEmpty(Text) && xmlElement5.ChildNodes[0] != null)
                                                                        {
                                                                            if (xmlElement5.ChildNodes[0].ChildNodes[0].Name.ToLower().Contains("bold"))
                                                                            {
                                                                                Text = "#bst#" + Text + "#be#";
                                                                            }
                                                                            else if (xmlElement5.ChildNodes[0].ChildNodes[0].Name.ToLower().Contains("italic"))
                                                                            {
                                                                                Text = "##is##" + Text + "##ie##";
                                                                            }
                                                                            else if (xmlElement5.ChildNodes[0].ChildNodes[0].Name.ToLower().Contains("underline"))
                                                                            {
                                                                                Text = "##us##" + Text + "##ue##";
                                                                            }
                                                                        }
                                                                    }
                                                                    //RM#3646 - End
                                                                    if (xmlElement5.Name == "MotorwayCaution")
                                                                    {
                                                                        MoterwayCaution = true;
                                                                    }
                                                                    //RM#3646 - Start
                                                                    if (xmlElement5 != null && xmlElement5.Name == "Caution")
                                                                    {
                                                                        if (xmlElement5.HasAttribute("IsApplicable"))
                                                                        {
                                                                            IsApplicable = Convert.ToBoolean(xmlElement5.Attributes["IsApplicable"].InnerText);
                                                                        }
                                                                        if (xmlElement5.HasAttribute("CautionId"))
                                                                        {
                                                                            CautionId = Convert.ToInt32(xmlElement5.Attributes["CautionId"].InnerText);
                                                                        }

                                                                        if (xmlElement5.GetElementsByTagName("caution:Action") != null && xmlElement5.GetElementsByTagName("caution:Action").Count > 0)
                                                                        {
                                                                            if (xmlElement5.GetElementsByTagName("caution:SpecificAction").Count > 0)
                                                                            {
                                                                                if (xmlElement5.GetElementsByTagName("caution:SpecificAction") != null && xmlElement5.GetElementsByTagName("caution:SpecificAction").Count > 0)
                                                                                {
                                                                                    Action = "SpecificAction";
                                                                                    Text = xmlElement5.GetElementsByTagName("caution:SpecificAction").Item(0).InnerText;
                                                                                    if (xmlElement5.GetElementsByTagName("caution:SpecificAction").Item(0).FirstChild != null && xmlElement5.GetElementsByTagName("caution:SpecificAction").Item(0).FirstChild.Name.ToLower().Contains("bold"))
                                                                                    {
                                                                                        Text = "#bst#" + Text + "#be#";
                                                                                    }
                                                                                    else if (xmlElement5.GetElementsByTagName("caution:SpecificAction").Item(0).FirstChild != null && xmlElement5.GetElementsByTagName("caution:SpecificAction").Item(0).FirstChild.Name.ToLower().Contains("italic"))
                                                                                    {
                                                                                        Text = "##is##" + Text + "##ie##";
                                                                                    }
                                                                                    else if (xmlElement5.GetElementsByTagName("caution:SpecificAction").Item(0).FirstChild != null && xmlElement5.GetElementsByTagName("caution:SpecificAction").Item(0).FirstChild.Name.ToLower().Contains("underline"))
                                                                                    {
                                                                                        Text = "##us##" + Text + "##ue##";
                                                                                    }

                                                                                }
                                                                                else if (xmlElement5.GetElementsByTagName("caution:standard") != null && xmlElement5.GetElementsByTagName("caution:standard").Count > 0)
                                                                                {
                                                                                    Action = "Standard";
                                                                                    Text = xmlElement5.GetElementsByTagName("caution:standard").Item(0).InnerText;
                                                                                }
                                                                            }
                                                                        }

                                                                        if (xmlElement5.GetElementsByTagName("caution:CautionedEntity") != null && xmlElement5.GetElementsByTagName("caution:CautionedEntity").Count > 0)
                                                                        {
                                                                            //caution:CautionedEntity
                                                                            if (xmlElement5.GetElementsByTagName("caution:Constraint") != null && xmlElement5.GetElementsByTagName("caution:Constraint").Count > 0)
                                                                            {
                                                                                //caution:Constraint
                                                                                if (xmlElement5.GetElementsByTagName("caution:ECRN") != null && xmlElement5.GetElementsByTagName("caution:ECRN").Count > 0)
                                                                                {
                                                                                    ECRN = xmlElement5.GetElementsByTagName("caution:ECRN").Item(0).InnerText;
                                                                                }
                                                                                //ESRN
                                                                                if (xmlElement5.GetElementsByTagName("caution:Type") != null && xmlElement5.GetElementsByTagName("caution:Type").Count > 0)
                                                                                {
                                                                                    Type = xmlElement5.GetElementsByTagName("caution:Type").Item(0).InnerText;
                                                                                }
                                                                                if (xmlElement5.GetElementsByTagName("caution:ConstraintName") != null && xmlElement5.GetElementsByTagName("caution:ConstraintName").Count > 0)
                                                                                {
                                                                                    ConstraintName = xmlElement5.GetElementsByTagName("caution:ConstraintName").Item(0).InnerText;
                                                                                }
                                                                            }

                                                                            if (xmlElement5.GetElementsByTagName("caution:Structure") != null && xmlElement5.GetElementsByTagName("caution:Structure").Count > 0)
                                                                            {
                                                                                //caution:structure
                                                                                if (xmlElement5.GetElementsByTagName("caution:ESRN") != null && xmlElement5.GetElementsByTagName("caution:ESRN").Count > 0)
                                                                                {
                                                                                    ESRN = xmlElement5.GetElementsByTagName("caution:ESRN").Item(0).InnerText;
                                                                                }
                                                                                if (xmlElement5.GetElementsByTagName("caution:SectionId") != null && xmlElement5.GetElementsByTagName("caution:SectionId").Count > 0)
                                                                                {
                                                                                    SectionId = Convert.ToInt32(xmlElement5.GetElementsByTagName("caution:SectionId").Item(0).InnerText);
                                                                                }
                                                                                if (xmlElement5.GetElementsByTagName("caution:StructureName") != null && xmlElement5.GetElementsByTagName("caution:StructureName").Count > 0)
                                                                                {
                                                                                    StructureName = xmlElement5.GetElementsByTagName("caution:StructureName").Item(0).InnerText;
                                                                                }
                                                                            }
                                                                        }
                                                                        if (xmlElement5.GetElementsByTagName("caution:Contact") != null && xmlElement5.GetElementsByTagName("caution:Contact").Count > 0)
                                                                        {//caution:Contact
                                                                            if (xmlElement5.GetElementsByTagName("caution:Contact").Item(0).Attributes["OrganisationId"] != null)
                                                                            {
                                                                                OrganisationId = Convert.ToInt32(xmlElement5.GetElementsByTagName("caution:Contact").Item(0).Attributes["OrganisationId"].InnerText);
                                                                            }

                                                                            if (xmlElement5.GetElementsByTagName("contact:OrganisationName") != null && xmlElement5.GetElementsByTagName("contact:OrganisationName").Count > 0)
                                                                            {
                                                                                OrganisationName = xmlElement5.GetElementsByTagName("contact:OrganisationName").Item(0).InnerText;
                                                                            }

                                                                            if (xmlElement5.GetElementsByTagName("contact:TelephoneNumber") != null && xmlElement5.GetElementsByTagName("contact:TelephoneNumber").Count > 0)
                                                                            {
                                                                                TelephoneNumber = xmlElement5.GetElementsByTagName("contact:TelephoneNumber").Item(0).InnerText;
                                                                            }

                                                                            if (xmlElement5.GetElementsByTagName("contact:FaxNumber") != null && xmlElement5.GetElementsByTagName("contact:FaxNumber").Count > 0)
                                                                            {
                                                                                FaxNumber = xmlElement5.GetElementsByTagName("contact:FaxNumber").Item(0).InnerText;
                                                                            }

                                                                            if (xmlElement5.GetElementsByTagName("contact:EmailAddress") != null && xmlElement5.GetElementsByTagName("contact:EmailAddress").Count > 0)
                                                                            {
                                                                                EmailAddress = xmlElement5.GetElementsByTagName("contact:EmailAddress").Item(0).InnerText;
                                                                            }

                                                                            if (xmlElement5.GetElementsByTagName("contact:FullName") != null && xmlElement5.GetElementsByTagName("contact:FullName").Count > 0)
                                                                            {
                                                                                FullName = xmlElement5.GetElementsByTagName("contact:FullName").Item(0).InnerText;
                                                                            }
                                                                        }
                                                                    }
                                                                    //RM#3646 - End
                                                                }
                                                            }
                                                            if (xmlElement4.Name == "GridReference")
                                                            {
                                                                foreach (XmlElement xmlElement5 in xmlElement4.ChildNodes)
                                                                {
                                                                    if (xmlElement5.Name == "X" || xmlElement5.LocalName == "X")
                                                                    {
                                                                        X = Convert.ToUInt64(xmlElement5.InnerText);
                                                                    }
                                                                    if (xmlElement5.Name == "Y" || xmlElement5.LocalName == "Y")
                                                                    {
                                                                        Y = Convert.ToUInt64(xmlElement5.InnerText);
                                                                    }
                                                                }
                                                            }
                                                            if (xmlElement4.Name == "EncounteredAt")
                                                            {
                                                                foreach (XmlElement xmlElement5 in xmlElement4.ChildNodes)
                                                                {
                                                                    if (xmlElement5.Name == "MeasuredMetric")
                                                                    {
                                                                        EncounterMeasureMetric = Convert.ToDecimal(xmlElement5.InnerText);
                                                                    }

                                                                    if (xmlElement5.Name == "DisplayMetric")
                                                                    {
                                                                        EncounteredMetric = Convert.ToDecimal(xmlElement5.InnerText);
                                                                    }

                                                                    if (xmlElement5.Name == "DisplayImperial")
                                                                    {
                                                                        EncounteredImperial = Convert.ToDecimal(xmlElement5.InnerText);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                DrivingInstructionModel drivingInstrMdl = new DrivingInstructionModel
                                                {
                                                    PointType = PointType,
                                                    Description = Description,
                                                    GridRefX = X,
                                                    GridRefY = Y,
                                                    MotorwayCaution = MoterwayCaution,
                                                    //RM#3646 - start
                                                    AnnotationId = AnnotationId,
                                                    Text = Text,
                                                    AnnotationType = AnnotationType,
                                                    IsDrivingInstructionAnnotation = isDrivingInstruction,
                                                    //RM#3646 - end
                                                    //RM#3646 - Start
                                                    IsApplicable = IsApplicable,
                                                    Action = Action,
                                                    CautionedEntity = CautionedEntity,
                                                    CautionId = CautionId,
                                                    CautionIdSpecified = CautionIdSpecified,
                                                    Conditions = Conditions,
                                                    ConstrainingAttribute = ConstrainingAttribute,
                                                    Contact = Contact,
                                                    ECRN = ECRN,
                                                    ESRN = ESRN,
                                                    Type = Type,
                                                    ConstraintName = ConstraintName,
                                                    OrganisationId = OrganisationId,
                                                    OrganisationName = OrganisationName,

                                                    FullName = FullName,
                                                    TelephoneNumber = TelephoneNumber,
                                                    FaxNumber = FaxNumber,
                                                    EmailAddress = EmailAddress,
                                                    SectionId = SectionId,
                                                    StructureName = StructureName,
                                                    EncounteredMetric = EncounteredMetric,
                                                    EncounteredImperial = EncounteredImperial,
                                                    EncounterMeasureMetric = EncounterMeasureMetric
                                                };

                                                lstDrivingInstruModel.Add(drivingInstrMdl);

                                                PointType = string.Empty;
                                                Description = string.Empty;
                                                X = 0;
                                                Y = 0;
                                                MoterwayCaution = false;
                                                //RM#3646 Start
                                                AnnotationType = string.Empty;
                                                AnnotationId = 0;
                                                Text = string.Empty;
                                                isDrivingInstruction = false;
                                                //RM#3646 End
                                                //RM#3646 Start
                                                IsApplicable = false;
                                                Action = string.Empty;
                                                CautionedEntity = string.Empty;
                                                CautionId = 0;
                                                CautionIdSpecified = false;
                                                Conditions = string.Empty;
                                                ConstrainingAttribute = string.Empty;
                                                Contact = string.Empty;
                                                ECRN = string.Empty;
                                                Type = string.Empty;
                                                ConstraintName = string.Empty;
                                                OrganisationId = 0;
                                                OrganisationName = string.Empty;

                                                FullName = string.Empty;
                                                TelephoneNumber = string.Empty;
                                                FaxNumber = string.Empty;
                                                EmailAddress = string.Empty;
                                                ESRN = string.Empty;
                                                SectionId = 0;
                                                StructureName = string.Empty;
                                                EncounteredImperial = 0;
                                                EncounteredMetric = 0;
                                                EncounterMeasureMetric = 0;
                                                //RM#3646 End
                                            }

                                            DrivingInstructionInfo = new DrivingInstructionModel
                                            {
                                                ComparisonId = ComparisonId,
                                                Id = Id,
                                                LegNumber= LegNumber,
                                                Name = Name,
                                                Instruction = instruction,
                                                MeasuredMetricDistance = MeasuredMetric,
                                                DisplayMetricDistance = DisplayMetric,
                                                DisplayImperialDistance = DisplayImperial,
                                                ListDrivingInstructionModel = lstDrivingInstruModel
                                            };

                                            drivingInsList.Add(DrivingInstructionInfo);

                                            Pcds = new AggreedRouteXSD.PredefinedCautionsDescriptionsStructure1();
                                            Pcds.DockCautionDescription = incdoccaution;
                                            Pcds.MotorwayCautionDescription = motorwayCautionDescription;

                                            instruction = string.Empty;
                                            MeasuredMetric = 0;
                                            DisplayMetric = 0;
                                            DisplayImperial = 0;
                                            lstDrivingInstruModel = null;
                                        }
                                    }
                                }

                                List<AggreedRouteXSD.DrivingInstructionListStructureInstructionListPosition> dilsilpList = new List<AggreedRouteXSD.DrivingInstructionListStructureInstructionListPosition>();

                                foreach (DrivingInstructionModel drivinginsstru in drivingInsList)
                                {
                                    List<AggreedRouteXSD.DrivingInstructionStructureNoteListPosition> disnlpList = new List<AggreedRouteXSD.DrivingInstructionStructureNoteListPosition>();

                                    AggreedRouteXSD.DrivingInstructionStructureNoteListPosition disnlp1 = new AggreedRouteXSD.DrivingInstructionStructureNoteListPosition();

                                    AggreedRouteXSD.DrivingInstructionStructure drvIns1 = new AggreedRouteXSD.DrivingInstructionStructure();

                                    AggreedRouteXSD.DrivingInstructionListStructureInstructionListPosition dilsilp1 = new AggreedRouteXSD.DrivingInstructionListStructureInstructionListPosition();

                                    AggreedRouteXSD.NavigationInstructionStructure nis1 = new AggreedRouteXSD.NavigationInstructionStructure();

                                    AggreedRouteXSD.SimpleTextStructure sts1 = new AggreedRouteXSD.SimpleTextStructure();

                                    AggreedRouteXSD.LevelTwoTextStructure[] ltsArray = new AggreedRouteXSD.LevelTwoTextStructure[1];

                                    string[] str1 = new string[1];
                                    str1[0] = drivinginsstru.Instruction;
                                    sts1.Text = str1;

                                    nis1.Instruction = sts1;

                                    AggreedRouteXSD.DrivingInstructionDistanceStructure dids = new AggreedRouteXSD.DrivingInstructionDistanceStructure();

                                    if (drivinginsstru.MeasuredMetricDistance != 0)
                                    {
                                        dids.MeasuredMetric = Convert.ToDecimal(drivinginsstru.MeasuredMetricDistance);
                                    }
                                    if (drivinginsstru.DisplayMetricDistance != 0)
                                    {
                                        dids.DisplayMetric = Convert.ToDecimal(drivinginsstru.DisplayMetricDistance);
                                    }
                                    if (drivinginsstru.DisplayImperialDistance != 0)
                                    {
                                        dids.DisplayImperial = Convert.ToDecimal(drivinginsstru.DisplayImperialDistance);
                                    }
                                    if (dids.MeasuredMetric != 0 || dids.DisplayMetric != 0 || dids.DisplayImperial != 0)
                                    {
                                        nis1.Distance = dids;
                                    }

                                    drvIns1.Navigation = nis1;

                                    for (int count = 0; count < drivinginsstru.ListDrivingInstructionModel.Count; count++)
                                    {
                                        AggreedRouteXSD.DrivingInstructionNoteStructure dins1 = new AggreedRouteXSD.DrivingInstructionNoteStructure();
                                        AggreedRouteXSD.NoteChoiceStructure ncs1 = new AggreedRouteXSD.NoteChoiceStructure();

                                        if (drivinginsstru.ListDrivingInstructionModel[count].MotorwayCaution)
                                        {
                                            MotorwayCautionStructure mwcaution = new MotorwayCautionStructure();
                                            mwcaution.Description = "Apply motorway caution";
                                            ncs1.Item = mwcaution;

                                            dins1.Content = ncs1;
                                        }

                                        //RM#3646 - start
                                        if (drivinginsstru.ListDrivingInstructionModel[count].AnnotationId != 0)
                                        {
                                            AggreedRouteXSD.ResolvedAnnotationStructure ras = new AggreedRouteXSD.ResolvedAnnotationStructure();

                                            string[] str2 = new string[1];
                                            str2[0] = drivinginsstru.ListDrivingInstructionModel[count].Text;
                                            ras.Text = new AggreedRouteXSD.SimpleTextStructure();
                                            ras.Text.Text = str2;

                                            ras.AnnotationId = drivinginsstru.ListDrivingInstructionModel[count].AnnotationId;

                                            ras.IsDrivingInstructionAnnotation = drivinginsstru.ListDrivingInstructionModel[count].IsDrivingInstructionAnnotation;


                                            switch (drivinginsstru.ListDrivingInstructionModel[count].AnnotationType.ToLower())
                                            {
                                                case "caution":
                                                    ras.AnnotationType = AggreedRouteXSD.AnnotationType.caution;
                                                    break;
                                                case "generic":
                                                    ras.AnnotationType = AggreedRouteXSD.AnnotationType.generic;
                                                    break;
                                                case "specialmanouevre":
                                                    ras.AnnotationType = AggreedRouteXSD.AnnotationType.specialmanouevre;
                                                    break;
                                                case "special_manouevre":
                                                    ras.AnnotationType = AggreedRouteXSD.AnnotationType.specialmanouevre;
                                                    break;
                                                default:
                                                    break;
                                            }

                                            ncs1.Item = ras;

                                            dins1.Content = ncs1;
                                        }
                                        //RM#3646 - end

                                        if (drivinginsstru.ListDrivingInstructionModel[count].PointType != string.Empty)
                                        {
                                            AggreedRouteXSD.RoutePointDescriptionStructure rps1 = new AggreedRouteXSD.RoutePointDescriptionStructure();
                                            if (drivinginsstru.ListDrivingInstructionModel[count].PointType == "start")
                                            {
                                                rps1.PointType = AggreedRouteXSD.RoutePointType.start;
                                            }
                                            if (drivinginsstru.ListDrivingInstructionModel[count].PointType == "end")
                                            {
                                                rps1.PointType = AggreedRouteXSD.RoutePointType.end;
                                            }
                                            if (drivinginsstru.ListDrivingInstructionModel[count].PointType == "way")
                                            {
                                                rps1.PointType = AggreedRouteXSD.RoutePointType.way;
                                            }
                                            if (drivinginsstru.ListDrivingInstructionModel[count].PointType == "intermediate")
                                            {
                                                rps1.PointType = AggreedRouteXSD.RoutePointType.intermediate;
                                            }
                                            rps1.Description = drivinginsstru.ListDrivingInstructionModel[count].Description;
                                            ncs1.Item = rps1;

                                            if (rps1.Description != string.Empty)
                                            {
                                                dins1.Content = ncs1;
                                            }
                                        }

                                        if (drivinginsstru.ListDrivingInstructionModel[count].CautionId > 0)
                                        {
                                            AggreedRouteXSD.ResolvedCautionStructure rcs = new AggreedRouteXSD.ResolvedCautionStructure();

                                            AggreedRouteXSD.CautionedEntityChoiceStructure cecs = new AggreedRouteXSD.CautionedEntityChoiceStructure();

                                            rcs.IsApplicable = drivinginsstru.ListDrivingInstructionModel[count].IsApplicable;
                                            rcs.CautionId = drivinginsstru.ListDrivingInstructionModel[count].CautionId;
                                            rcs.CautionIdSpecified = true;

                                            rcs.Name = drivinginsstru.ListDrivingInstructionModel[count].Name;

                                            AggreedRouteXSD.CautionActionStructure cas = new AggreedRouteXSD.CautionActionStructure();

                                            if (drivinginsstru.ListDrivingInstructionModel[count].Action == "SpecificAction")
                                            {
                                                string specificText = drivinginsstru.ListDrivingInstructionModel[count].Text;

                                                cas.Item = specificText;
                                            }
                                            else if (drivinginsstru.ListDrivingInstructionModel[count].Action == "Standard")
                                            {
                                                object standardText;

                                                standardText = drivinginsstru.ListDrivingInstructionModel[count].Text;

                                                cas.Item = standardText;
                                            }

                                            AggreedRouteXSD.CautionedEntityChoiceStructure cautionecs = new AggreedRouteXSD.CautionedEntityChoiceStructure();

                                            if (drivinginsstru.ListDrivingInstructionModel[count].ECRN != string.Empty)
                                            {
                                                AggreedRouteXSD.CautionedConstraintStructure ccs = new AggreedRouteXSD.CautionedConstraintStructure();

                                                ccs.ECRN = drivinginsstru.ListDrivingInstructionModel[count].ECRN;
                                                ccs.ConstraintName = drivinginsstru.ListDrivingInstructionModel[count].ConstraintName;
                                                cautionecs.Item = ccs;
                                            }

                                            if (drivinginsstru.ListDrivingInstructionModel[count].ESRN != string.Empty)
                                            {
                                                AggreedRouteXSD.CautionedStructureStructure css = new AggreedRouteXSD.CautionedStructureStructure();

                                                css.ESRN = drivinginsstru.ListDrivingInstructionModel[count].ESRN;
                                                css.SectionId = drivinginsstru.ListDrivingInstructionModel[count].SectionId;
                                                css.SectionIdSpecified = true;
                                                css.StructureName = drivinginsstru.ListDrivingInstructionModel[count].StructureName;

                                                cautionecs.Item = css;
                                            }

                                            AggreedRouteXSD.ResolvedContactStructure[] rconsArray = new AggreedRouteXSD.ResolvedContactStructure[1];

                                            AggreedRouteXSD.ResolvedContactStructure rcons = new AggreedRouteXSD.ResolvedContactStructure();

                                            rcons.FullName = drivinginsstru.ListDrivingInstructionModel[count].FullName;

                                            rcons.OrganisationId = drivinginsstru.ListDrivingInstructionModel[count].OrganisationId;
                                            rcons.OrganisationName = drivinginsstru.ListDrivingInstructionModel[count].OrganisationName;

                                            rcons.TelephoneNumber = drivinginsstru.ListDrivingInstructionModel[count].TelephoneNumber;
                                            rcons.FaxNumber = drivinginsstru.ListDrivingInstructionModel[count].FaxNumber;
                                            rcons.EmailAddress = drivinginsstru.ListDrivingInstructionModel[count].EmailAddress;

                                            rconsArray[0] = rcons;

                                            rcs.Contact = rconsArray;
                                            rcs.Action = cas;
                                            rcs.CautionedEntity = cautionecs;

                                            ncs1.Item = rcs;

                                            dins1.Content = ncs1;
                                        }

                                        if (drivinginsstru.ListDrivingInstructionModel[count].GridRefX > 0 || drivinginsstru.ListDrivingInstructionModel[count].GridRefY > 0)
                                        {
                                            AggreedRouteXSD.GridReferenceStructure grs1 = new AggreedRouteXSD.GridReferenceStructure();

                                            grs1.X = drivinginsstru.ListDrivingInstructionModel[count].GridRefX;
                                            grs1.Y = drivinginsstru.ListDrivingInstructionModel[count].GridRefY;

                                            dins1.GridReference = grs1;
                                        }

                                        if (drivinginsstru.ListDrivingInstructionModel[count].EncounteredImperial > 0 || drivinginsstru.ListDrivingInstructionModel[count].EncounteredMetric > 0)
                                        {
                                            AggreedRouteXSD.DrivingInstructionDistanceStructure dinstrds = new AggreedRouteXSD.DrivingInstructionDistanceStructure();

                                            dinstrds.MeasuredMetric = drivinginsstru.ListDrivingInstructionModel[count].EncounterMeasureMetric;
                                            dinstrds.DisplayImperial = drivinginsstru.ListDrivingInstructionModel[count].EncounteredImperial;
                                            dinstrds.DisplayMetric = drivinginsstru.ListDrivingInstructionModel[count].EncounteredMetric;

                                            dins1.EncounteredAt = dinstrds;
                                        }

                                        if (drivinginsstru.ListDrivingInstructionModel[count].PointType != "" || drivinginsstru.ListDrivingInstructionModel[count].MotorwayCaution
                                            || drivinginsstru.ListDrivingInstructionModel[count].AnnotationId != 0 || drivinginsstru.ListDrivingInstructionModel[count].CautionId > 0)
                                        {
                                            disnlp1.Note = dins1;

                                            disnlpList.Add(disnlp1);

                                        }

                                        disnlp1 = new AggreedRouteXSD.DrivingInstructionStructureNoteListPosition();

                                        //---------------------------------------------------------

                                        dis.ComparisonId = drivinginsstru.ListDrivingInstructionModel[count].ComparisonId;
                                        dis.Id = drivinginsstru.ListDrivingInstructionModel[count].Id;
                                        dis.LegNumber = drivinginsstru.ListDrivingInstructionModel[count].LegNumber;
                                        dis.Name = drivinginsstru.ListDrivingInstructionModel[count].Name;
                                    }

                                    drvIns1.NoteListPosition = disnlpList.ToArray();
                                    dilsilp1.Instruction = drvIns1;
                                    dilsilpList.Add(dilsilp1);

                                    disnlpList = null;
                                }

                                dils.InstructionListPosition = dilsilpList.ToArray();

                                if (alternativeNo > 0)
                                {
                                    AggreedRouteXSD.AlternativeDescriptionStructure ads = new AggreedRouteXSD.AlternativeDescriptionStructure();

                                    ads.AlternativeNo = Convert.ToString(alternativeNo);
                                    ads.Description = alternativeName;

                                    dils.AlternativeDescription = ads;
                                }

                                dispsalp1.Alternative = dils;

                                dispsalpList.Add(dispsalp1);

                                dispsalp1 = new AggreedRouteXSD.DrivingInstructionSubPartStructureAlternativeListPosition();

                                
                            }
                        }

                        disslp1.SubPart = dispsalpList.ToArray();
                        disslpList.Add(disslp1);
                    }

                    dis.SubPartListPosition = disslpList.ToArray();
                }
            }

            return dis;
        }

        /// <summary>
        /// get special order detail
        /// </summary>
        /// <param name="esDAlRefNo">esdal ref no</param>
        /// <returns></returns>
        public static OrderSummaryChoiceStructure GetSODetails(string esDAlRefNo, string userSchema)
        {
            OrderSummaryChoiceStructure oscs = new OrderSummaryChoiceStructure();
            SignedOrdersSummaryStructure signedorders = new SignedOrdersSummaryStructure();

            List<PossiblyReplacingSignedOrderSummaryStructure> prsossList = new List<PossiblyReplacingSignedOrderSummaryStructure>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               prsossList,
               userSchema + ".GET_SPECIAL_ORDER_DETAILS",
               parameter =>
               {
                   parameter.AddWithValue("p_ESDAL_REF_NUMBER", esDAlRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                  (records, instance) =>
                  {
                      PossiblyReplacingSignedOrderSummaryStructure prsoss = new PossiblyReplacingSignedOrderSummaryStructure();

                      SignedOrderSummaryStructure soss = new SignedOrderSummaryStructure();

                      soss.OrderNumber = records.GetStringOrDefault("ORDER_NO");
                      soss.SignedOn = records.GetDateTimeOrDefault("SIGNED_DATE");
                      soss.ExpiresOn = records.GetDateTimeOrDefault("EXPIRY_DATE");
                      soss.SignedBy = records.GetStringOrDefault("SIGNATORY");

                      prsoss.CurrentOrder = soss;

                      prsossList.Add(prsoss);
                  }
             );

            signedorders.OrderSummary = prsossList.ToArray();
            oscs.Item = signedorders;

            return oscs;
        }

        /// <summary>
        /// get notes for haulier xml from database
        /// </summary>
        /// <param name="orderNumber">order number</param>
        /// <param name="esDAlRefNo">esdal ref no</param>
        /// <returns></returns>
        public static Byte[] GetNotesForHaulier(string orderNumber, string esDAlRefNo, string userSchema = UserSchema.Portal)
        {
            Byte[] noteforhaulierArray = null;
            AgreedRouteStructure ps = new AgreedRouteStructure();

            string[] esdalRefNumber = esDAlRefNo.Split('/');

            string haulierMnemonic = string.Empty;
            string projectNumber = string.Empty;
            string versionNumber = string.Empty;

            if (esdalRefNumber.Length > 0)
            {
                haulierMnemonic = esdalRefNumber[0];
                projectNumber = esdalRefNumber[1].Replace("S", "");
                versionNumber = esdalRefNumber[2].Replace("S", "");
            }

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               ps,
               userSchema + ".GET_REVISE_AGREEMENT_DETAILS",
               parameter =>
               {
                   parameter.AddWithValue("p_ORDER_NUMBER", orderNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_HAULIER_MNEMONIC", haulierMnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_PROJECT_NUMBER", projectNumber, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_VERSION_NUMBER", versionNumber, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                  (records, instance) =>
                  {
                      noteforhaulierArray = records.GetByteArrayOrNull("notes_for_haulier");
                  }
           );
            return noteforhaulierArray;
        }

        /// <summary>
        ///  get notes for haulier xml from database
        /// </summary>
        /// <param name="versionID">version id</param>
        /// <returns></returns>
        public static Byte[] GetNotesForHaulier(long versionID, string userSchema = UserSchema.Sort)
        {
            Byte[] noteforhaulierArray = null;
            AgreedRouteStructure ps = new AgreedRouteStructure();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               ps,
               userSchema + ".GET_MOVEMENT_DETAILS",
               parameter =>
               {
                   parameter.AddWithValue("P_VERSION_ID", versionID, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                  (records, instance) =>
                  {
                      noteforhaulierArray = records.GetByteArrayOrNull("notes_for_haulier");
                  }
           );
            return noteforhaulierArray;
        }

        /// <summary>
        /// get route description node detail for revised agreement document
        /// </summary>
        /// <param name="orderNumber">order number</param>
        /// <param name="esDAlRefNo">esdal ref no</param>
        /// <returns></returns>
        public static byte[] GetRouteDescription(string orderNumber, string esDAlRefNo, string userSchema = UserSchema.Sort)
        {
            byte[] byteXML = null;

            string[] esdalRefNumber = esDAlRefNo.Split('/');

            string haulierMnemonic = string.Empty;
            string projectNumber = string.Empty;
            string versionNumber = string.Empty;

            if (esdalRefNumber.Length > 0)
            {
                haulierMnemonic = esdalRefNumber[0];
                projectNumber = esdalRefNumber[1].Replace("S", "");
                versionNumber = esdalRefNumber[2].Replace("S", "");
            }

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                byteXML,
                userSchema + ".GET_ROUTE_DESCRIPTION",
                parameter =>
                {
                    parameter.AddWithValue("p_ORDER_NUMBER", orderNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    if (userSchema == UserSchema.Portal)
                    {
                        parameter.AddWithValue("p_HAULIER_MNEMONIC", haulierMnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_PROJECT_NUMBER", projectNumber, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_VERSION_NUMBER", versionNumber, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    }
                    else
                    {
                        parameter.AddWithValue("p_ESDAL_REF_NUMBER", esDAlRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    }
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 (records, instance) =>
                 {
                     byteXML = records.GetByteArrayOrNull("route_description");
                 }
                );

            return byteXML;
        }
    }
}