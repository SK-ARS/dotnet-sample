using NoLongerAffectedXSD;
using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.DataAccess.SafeProcedure;
using STP.Domain.DocumentsAndContents;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace STP.DocumentsAndContents.Persistance
{
    public static class ReProposalNoLongerAffectedDAO
    {
        /// <summary>
        /// get no longer affected details for notification
        /// </summary>
        /// <param name="ProjectID">project id</param>
        /// <param name="psPortalType">portal type</param>
        /// <param name="ContactID">contact id</param>
        /// <param name="versionNo">version no</param>
        /// <returns></returns>
        public static NoLongerAffectedStructure GetNoLongerAfftectedDetailsForNotification(int ProjectID, Enums.PortalType psPortalType, int ContactID, int versionNo, string userSchema = UserSchema.Sort)
        {
            NoLongerAffectedStructure odns = new NoLongerAffectedStructure();
            long analysisId = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                odns,
                userSchema + ".GET_REPROPONO_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("p_Project_ID", ProjectID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_Version_No", versionNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       NoLongerAffectedXSD.ESDALReferenceNumberStructure esdalrefnostru = new NoLongerAffectedXSD.ESDALReferenceNumberStructure();
                       esdalrefnostru.Mnemonic = records.GetStringOrDefault("HAULIER_MNEMONIC");

                       esdalrefnostru.MovementProjectNumber = Convert.ToString(records.GetInt32OrDefault("ESDAL_REF_NUMBER"));

                       NoLongerAffectedXSD.MovementVersionNumberStructure movvernostru = new NoLongerAffectedXSD.MovementVersionNumberStructure();
                       movvernostru.Value = records.GetShortOrDefault("VERSION_NO");
                       esdalrefnostru.MovementVersion = movvernostru;

                       esdalrefnostru.EnteredBySORT = records.GetInt16OrDefault("VERSIONED_BY_SORT") == 1;
                       esdalrefnostru.EnteredBySORTSpecified = true;

                       instance.EMRN = esdalrefnostru;

                           #region HaulierContact
                           ContactChoiceStructure ccstru = new ContactChoiceStructure();
                       ccstru.Item = GetHAcontactDetails(ContactID);
                       instance.Contact = ccstru;
                           #endregion

                           instance.SentDateTime = DateTime.Now;

                           #region JourneyFromToSummary
                           JourneyFromToSummaryStructure jftss = new JourneyFromToSummaryStructure();

                       jftss.From = GetSimplifiedRoutePointStart(ProjectID).Description;

                       jftss.To = GetSimplifiedRoutePointEnd(ProjectID).Description;

                       instance.JourneyFromToSummary = jftss;
                       #endregion

                       #region JourneyTiming
                       NoLongerAffectedXSD.JourneyDateStructure jts = new NoLongerAffectedXSD.JourneyDateStructure();
                       jts.FirstMoveDate = records.GetDateTimeOrDefault("MOVEMENT_START_DATE");
                       jts.LastMoveDate = records.GetDateTimeOrDefault("MOVEMENT_END_DATE");
                       jts.LastMoveDateSpecified = true;

                       instance.JourneyTiming = jts;
                           #endregion

                           instance.HauliersName = records.GetStringOrDefault("HAULIER_NAME");

                       instance.CommunicationType = NoLongerAffectedCommunicationType.soroute;

                           #region Recipients
                           analysisId = records.GetLongOrDefault("Analysis_ID");

                       ContactModel contactInfo = GetRecipientDetails(analysisId, userSchema);

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
                               if(childrenNode != null)
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

                                           contactList[contactList.Count - 1].DelegationId = DelegationId;
                                           contactList[contactList.Count - 1].DelegatorsContactId = DelegatorsContactId;
                                           contactList[contactList.Count - 1].DelegatorsOrganisationId = DelegatorsOrganisationId;
                                           contactList[contactList.Count - 1].RetainNotification = RetainNotification;
                                           contactList[contactList.Count - 1].WantsFailureAlert = WantsFailureAlert;
                                           contactList[contactList.Count - 1].DelegatorsOrganisationName = DelegatorsOrganisationName;
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
                                                   }
                                               }
                                           }
                                       }
                                   }
                               }
                               
                           }

                           List<NoLongerAffectedXSD.RecipientContactStructure> rcsclplist = new List<NoLongerAffectedXSD.RecipientContactStructure>();
                           NoLongerAffectedXSD.RecipientContactStructure rcs1;
                           foreach (ContactModel cont in contactList)
                           {
                               rcs1 = new NoLongerAffectedXSD.RecipientContactStructure();

                               rcs1.Reason = cont.Reason;
                               rcs1.ContactId = cont.ContactId;
                               rcs1.ContactIdSpecified = true;

                               string orgtype = GetOrgTypeDetails(cont.OrganisationId);
                               if (orgtype == "haulier")
                                   rcs1.IsHaulier = true;
                               else
                                   rcs1.IsHaulier = false;

                               if (orgtype == "police")
                                   rcs1.IsPolice = true;
                               else
                                   rcs1.IsPolice = false;
                               rcs1.IsRecipient = cont.IsRecipient;
                               rcs1.IsRetainedNotificationOnly = cont.IsRetainedNotificationOnly;
                               rcs1.OrganisationId = cont.OrganisationId;
                               rcs1.OrganisationIdSpecified = true;

                               rcs1.ContactName = cont.FullName;
                               rcs1.OrganisationName = cont.Organisation;
                               rcs1.Fax = cont.Fax;
                               rcs1.Email = cont.Email;

                               rcsclplist.Add(rcs1);
                           }

                           instance.Recipients = rcsclplist.ToArray();
                       }
                           #endregion
                       }
            );

            return odns;
        }

        /// <summary>
        /// get jouney from detail for no longer affected document
        /// </summary>
        /// <param name="ProjectID">project id</param>
        /// <returns></returns>
        public static NoLongerAffectedXSD.SimplifiedRoutePointStructure GetSimplifiedRoutePointStart(int ProjectID)
        {
            NoLongerAffectedXSD.SimplifiedRoutePointStructure srps = new NoLongerAffectedXSD.SimplifiedRoutePointStructure();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                srps,
                UserSchema.Portal + ".GET_REPROPONO_STARTPOINT",
                parameter =>
                {
                    parameter.AddWithValue("p_Project_ID", ProjectID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ROUTEPART_ID", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
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
        /// get jouney to detail for no longer affected document
        /// </summary>
        /// <param name="ProjectID">project id</param>
        /// <returns></returns>
        public static NoLongerAffectedXSD.SimplifiedRoutePointStructure GetSimplifiedRoutePointEnd(int ProjectID)
        {
            NoLongerAffectedXSD.SimplifiedRoutePointStructure srps = new NoLongerAffectedXSD.SimplifiedRoutePointStructure();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                srps,
                UserSchema.Portal + ".GET_REPROPONO_ENDPOINT",
                parameter =>
                {
                    parameter.AddWithValue("p_Project_ID", ProjectID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ROUTEPART_ID", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
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
        /// get recipient xml from the database
        /// </summary>
        /// <param name="AnalysisID">analysis id</param>
        /// <returns></returns>
        public static ContactModel GetRecipientDetails(long AnalysisID, string userSchema = UserSchema.Sort)
        {
            ContactModel contactDetail = new ContactModel();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                contactDetail,
                userSchema + ".GET_REPROPONO_RECIPIENTS_DTL",
                parameter =>
                {
                    parameter.AddWithValue("p_ANALYSIS_ID", AnalysisID, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
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
        /// get org type detail based on organisation id
        /// </summary>
        /// <param name="organisationID">organisation id</param>
        /// <returns></returns>
        public static string GetOrgTypeDetails(int organisationID)
        {
            ContactModel contactDetail = new ContactModel();
            string orgtype = string.Empty;
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
                     orgtype = records.GetStringOrDefault("orgtype").ToLower();
                 }
                );

            return orgtype;
        }

        /// <summary>
        /// get ha contact detail for no longer affected document
        /// </summary>
        /// <param name="ContactID">contact id</param>
        /// <returns></returns>
        public static HAContactStructure GetHAcontactDetails(int ContactID)
        {
            HAContactStructure hacs = new HAContactStructure();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                  hacs,
                  UserSchema.Portal + ".GET_REPROPOSAL_CONTACT_DTL",
                  parameter =>
                  {
                      parameter.AddWithValue("P_CONTACT_ID", ContactID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
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

                       hacs.Contact = contact;

                       NoLongerAffectedXSD.AddressStructure has = new NoLongerAffectedXSD.AddressStructure();

                       string[] Addstru = new string[5];
                       Addstru[0] = records.GetStringOrDefault("ADDRESSLINE_1");
                       Addstru[1] = records.GetStringOrDefault("ADDRESSLINE_2");
                       Addstru[2] = records.GetStringOrDefault("ADDRESSLINE_3");
                       Addstru[3] = records.GetStringOrDefault("ADDRESSLINE_4");
                       Addstru[4] = records.GetStringOrDefault("ADDRESSLINE_5");
                       has.Line = Addstru;
                       has.PostCode = records.GetStringOrDefault("POSTCODE");

                       int country = Convert.ToInt32(records.GetDecimalOrDefault("COUNTRY_ID"));
                       if (country == (int)Country.england)
                       {
                           has.Country = NoLongerAffectedXSD.CountryType.england;
                           has.CountrySpecified = true;
                       }
                       else if (country == (int)Country.northernireland)
                       {
                           has.Country = NoLongerAffectedXSD.CountryType.northernireland;
                           has.CountrySpecified = true;
                       }
                       else if (country == (int)Country.scotland)
                       {
                           has.Country = NoLongerAffectedXSD.CountryType.scotland;
                           has.CountrySpecified = true;
                       }
                       else if (country == (int)Country.wales)
                       {
                           has.Country = NoLongerAffectedXSD.CountryType.wales;
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
    }
}