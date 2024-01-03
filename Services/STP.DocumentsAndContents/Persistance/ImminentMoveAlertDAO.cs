using ImminentMoveAlert;
using Oracle.DataAccess.Client;
using STP.Common.Enums;
using STP.DataAccess.SafeProcedure;
using STP.Domain.VehiclesAndFleets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using STP.Common.Constants;

namespace STP.DocumentsAndContents.Persistance
{
    public static class ImminentMoveAlertDAO
    {
        /// <summary>
        /// get imminent move alert detail based on notification
        /// </summary>
        /// <param name="contactId">contact id</param>
        /// <param name="notificationId">notification id</param>
        /// <returns></returns>
        public static ImminentMoveAlertStructure GetImminentMovAlertDetails(int contactId, int notificationId, int loggedInContactId)
        {
            
                ImminentMoveAlertStructure stru = new ImminentMoveAlertStructure();

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    stru,
                     UserSchema.Portal + ".GET_IMMI_MOVE_DETAILS",
                    parameter =>
                    {
                        parameter.AddWithValue("P_CONTACT_ID", contactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_NOTIFICATION_ID", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                       (records, instance) =>
                       {
                           instance.Recipients = GetRecipientDetails(contactId);

                           instance.AlertGenerationTime = DateTime.Now;

                           #region ESDALRefrenceNumber

                           ESDALReferenceNumberStructure esdalrefnostru = new ESDALReferenceNumberStructure();
                           esdalrefnostru.EnteredBySORT = false;
                           esdalrefnostru.EnteredBySORTSpecified = false;
                           esdalrefnostru.Mnemonic = records.GetStringOrDefault("HAULIER_MNEMONIC");
                           string[] hauMnemonicArr = records.GetStringOrDefault("NOTIFICATION_CODE").Split("/".ToCharArray());
                           if (hauMnemonicArr.Length > 0)
                           {
                               esdalrefnostru.MovementProjectNumber = hauMnemonicArr[1];
                           }

                           MovementVersionNumberStructure movvernostru = new MovementVersionNumberStructure();
                           movvernostru.Value = records.GetShortOrDefault("NOTIFICATION_VERSION_NO");
                           esdalrefnostru.MovementVersion = movvernostru;
                           esdalrefnostru.NotificationNumber = records.GetShortOrDefault("NOTIFICATION_NO");
                           esdalrefnostru.NotificationNumberSpecified = true;
                           if (hauMnemonicArr.Length > 0)
                           {
                               string[] movVersionArr = hauMnemonicArr[2].Split("(".ToCharArray());
                               string movver = string.Empty;
                               if (movVersionArr.Length > 1)
                               {
                                   movver = Convert.ToString(movVersionArr[1]);
                                   movver = movver.Replace(")", "");
                                   esdalrefnostru.NotificationVersion = Convert.ToInt16(movver);
                                   esdalrefnostru.NotificationVersionSpecified = true;
                               }
                           }

                           instance.ESDALReferenceNumber = esdalrefnostru;
                           #endregion

                           string specialOrderESDALRefNo = esdalrefnostru.Mnemonic + "/" + esdalrefnostru.MovementProjectNumber + "/S" + movvernostru.Value;

                           instance.HauliersReference = records.GetStringOrDefault("HAULIERS_REF");
                           instance.InboxQuickReference = Convert.ToString(records.GetDecimalOrDefault("QUICK_REF"));

                           if (Convert.ToString(records.GetInt32OrDefault("VEHICLE_CLASSIFICATION")) == Convert.ToString((int)VehicleEnums.VehicleClassificationType.VehicleSpecialOrder))
                           {
                               instance.Classification = MovementClassificationType.vehiclespecialorder;
                           }
                           if (Convert.ToString(records.GetInt32OrDefault("VEHICLE_CLASSIFICATION")) == Convert.ToString((int)VehicleEnums.VehicleClassificationType.SpecialOrder))
                           {
                               instance.Classification = MovementClassificationType.specialorder;
                           }
                           if (Convert.ToString(records.GetInt32OrDefault("VEHICLE_CLASSIFICATION")) == Convert.ToString((int)VehicleEnums.VehicleClassificationType.StgoailCat1))
                           {
                               instance.Classification = MovementClassificationType.stgoailcat1;
                           }
                           if (Convert.ToString(records.GetInt32OrDefault("VEHICLE_CLASSIFICATION")) == Convert.ToString((int)VehicleEnums.VehicleClassificationType.StgoailCat2))
                           {
                               instance.Classification = MovementClassificationType.stgoailcat2;
                           }
                           if (Convert.ToString(records.GetInt32OrDefault("VEHICLE_CLASSIFICATION")) == Convert.ToString((int)VehicleEnums.VehicleClassificationType.StgoailCat3))
                           {
                               instance.Classification = MovementClassificationType.stgoailcat3;
                           }
                           if (Convert.ToString(records.GetInt32OrDefault("VEHICLE_CLASSIFICATION")) == Convert.ToString((int)VehicleEnums.VehicleClassificationType.StgoMobileCraneCata))
                           {
                               instance.Classification = MovementClassificationType.stgomobilecranecata;
                           }
                           if (Convert.ToString(records.GetInt32OrDefault("VEHICLE_CLASSIFICATION")) == Convert.ToString((int)VehicleEnums.VehicleClassificationType.StgoMobileCraneCatb))
                           {
                               instance.Classification = MovementClassificationType.stgomobilecranecatb;
                           }
                           if (Convert.ToString(records.GetInt32OrDefault("VEHICLE_CLASSIFICATION")) == Convert.ToString((int)VehicleEnums.VehicleClassificationType.StgoMobileCraneCatc))
                           {
                               instance.Classification = MovementClassificationType.stgomobilecranecatc;
                           }
                           if (Convert.ToString(records.GetInt32OrDefault("VEHICLE_CLASSIFICATION")) == Convert.ToString((int)VehicleEnums.VehicleClassificationType.StgoRoadRecoveryVehicle))
                           {
                               instance.Classification = MovementClassificationType.stgoroadrecoveryvehicle;
                           }
                           if (Convert.ToString(records.GetInt32OrDefault("VEHICLE_CLASSIFICATION")) == Convert.ToString((int)VehicleEnums.VehicleClassificationType.WheeledConstructionAndUse))
                           {
                               instance.Classification = MovementClassificationType.wheeledconstructionanduse;
                           }
                           if (Convert.ToString(records.GetInt32OrDefault("VEHICLE_CLASSIFICATION")) == Convert.ToString((int)VehicleEnums.VehicleClassificationType.Tracked))
                           {
                               instance.Classification = MovementClassificationType.tracked;
                           }
                           if (Convert.ToString(records.GetInt32OrDefault("VEHICLE_CLASSIFICATION")) == Convert.ToString((int)VehicleEnums.VehicleClassificationType.StgoEngineeringPlantWheeled))
                           {
                               instance.Classification = MovementClassificationType.stgoengineeringplantwheeled;
                           }
                           if (Convert.ToString(records.GetInt32OrDefault("VEHICLE_CLASSIFICATION")) == Convert.ToString((int)VehicleEnums.VehicleClassificationType.StgoEngineeringPlantTracked))
                           {
                               instance.Classification = MovementClassificationType.stgoengineeringplanttracked;
                           }
                           if (Convert.ToString(records.GetInt32OrDefault("VEHICLE_CLASSIFICATION")) == Convert.ToString((int)VehicleEnums.VehicleClassificationType.StgoCat1EngineeringPlantWheeled))
                           {
                               instance.Classification = MovementClassificationType.StgoCat1EngineeringPlantWheeled;
                           }
                           if (Convert.ToString(records.GetInt32OrDefault("VEHICLE_CLASSIFICATION")) == Convert.ToString((int)VehicleEnums.VehicleClassificationType.StgoCat2EngineeringPlantWheeled))
                           {
                               instance.Classification = MovementClassificationType.StgoCat2EngineeringPlantWheeled;
                           }
                           if (Convert.ToString(records.GetInt32OrDefault("VEHICLE_CLASSIFICATION")) == Convert.ToString((int)VehicleEnums.VehicleClassificationType.StgoCat3EngineeringPlantWheeled))
                           {
                               instance.Classification = MovementClassificationType.StgoCat3EngineeringPlantWheeled;
                           }

                           if (records.GetStringOrDefault("VR1_NUMBER") != string.Empty)
                           {
                               instance.VR1 = records.GetStringOrDefault("VR1_NUMBER");
                           }

                           instance.FirstMoveTime = records.GetDateTimeOrDefault("MOVE_START_DATE");
                           instance.HaulierName = records.GetStringOrDefault("HAULIER_NAME");

                           JourneyFromToSummaryStructure js = new JourneyFromToSummaryStructure();
                           js.From = records.GetStringOrDefault("FROM_DESCR");

                           js.To = records.GetStringOrDefault("TO_DESCR");

                           instance.JourneyFromToSummary = js;

                           instance.Load = records.GetStringOrDefault("LOAD_DESCR");

                           instance.JobFileReference = records.GetStringOrDefault("ha_job_file_ref");

                           instance.HAContact = GetHAContactDetails(notificationId, loggedInContactId);

                           instance.OrderSummary = GetSODetail(specialOrderESDALRefNo);

                           instance.ContactDetails = GetContactDetails(contactId);

                           Byte[] inboundNotificationArray = records.GetByteArrayOrNull("INBOUND_NOTIFICATION");

                           if (inboundNotificationArray != null)
                           {
                               string inboundNotification = Encoding.UTF8.GetString(inboundNotificationArray, 0, inboundNotificationArray.Length);
                               XmlDocument Doc = new XmlDocument();
                               try
                               {
                                   Doc.LoadXml(inboundNotification);
                               }
                               catch (System.Xml.XmlException xmlEx)
                               {
                                   inboundNotificationArray = STP.Common.General.XsltTransformer.Trafo(records.GetByteArrayOrNull("INBOUND_NOTIFICATION"));
                                   inboundNotification = Encoding.UTF8.GetString(inboundNotificationArray, 0, inboundNotificationArray.Length);
                                   Doc.LoadXml(inboundNotification);
                               }

                               instance.LicenceNumber = Doc.GetElementsByTagName("Licence") == null ? string.Empty : (Doc.GetElementsByTagName("Licence").Item(0) == null ? string.Empty : Doc.GetElementsByTagName("Licence").Item(0).InnerText);
                           }
                       }
                );

                return stru;
             
        }

        /// <summary>
        /// get recipient detail
        /// </summary>
        /// <param name="contactId">contact id</param>
        /// <returns></returns>
        public static Contact[] GetRecipientDetails(int contactId)
        {
            List<Contact> rcsclplist = new List<Contact>();

             
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    rcsclplist,
                     UserSchema.Portal + ".GET_DAILY_DIGEST_DETAILS",
                    parameter =>
                    {
                        parameter.AddWithValue("p_CONTACT_ID", contactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                       (records, instance) =>
                       {
                           instance.ContactId = contactId;
                           instance.ContactIdSpecified = true;
                           if (Convert.ToString(records.GetLongOrDefault("organisation_id")) != string.Empty)
                           {
                               instance.OrganisationId = Convert.ToInt32(records.GetLongOrDefault("organisation_id"));
                               instance.OrganisationIdSpecified = true;
                           }

                           string contact = string.Empty;
                           if (records.GetStringOrDefault("TITLE") != string.Empty)
                               contact = records.GetStringOrDefault("TITLE");
                           if (records.GetStringOrDefault("FIRST_NAME") != string.Empty)
                               contact = contact + " " + records.GetStringOrDefault("FIRST_NAME");
                           if (records.GetStringOrDefault("SUR_NAME") != string.Empty)
                               contact = contact + " " + records.GetStringOrDefault("SUR_NAME");
                           instance.ContactName = contact;
                           instance.OrganisationName = records.GetStringOrDefault("orgname");

                       }
                   );
           
            return rcsclplist.ToArray();
        }

        public static string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        public static HAContactStructure GetHAContactDetails(int notificationId, int contactId)
        {
            
                HAContactStructure hacs = new HAContactStructure();

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    hacs,
                     UserSchema.Portal + ".GET_HAULIERADRESS_DETAILS",
                    parameter =>
                    {
                        parameter.AddWithValue("P_NOTIFICATION_ID", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_CONTACT_ID", contactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                     (records, instance) =>
                     {
                         instance.Contact = FirstLetterToUpper(records.GetStringOrDefault("name"));

                         AddressStructure has = new AddressStructure();

                         string[] Addstru = new string[5];
                         Addstru[0] = records.GetStringOrDefault("ADDRESSLINE_1");
                         Addstru[1] = records.GetStringOrDefault("ADDRESSLINE_2");
                         Addstru[2] = records.GetStringOrDefault("ADDRESSLINE_3");
                         Addstru[3] = records.GetStringOrDefault("ADDRESSLINE_4");
                         Addstru[4] = records.GetStringOrDefault("ADDRESSLINE_5");
                         has.Line = Addstru;
                         has.PostCode = records.GetStringOrDefault("POSTCODE");

                         int countrycode = records.GetInt32OrDefault("COUNTRY_ID");
                         if (countrycode == (int)Country.england)
                         {
                             has.Country = CountryType.england;
                             has.CountrySpecified = true;
                         }
                         else if (countrycode == (int)Country.northernireland)
                         {
                             has.Country = CountryType.northernireland;
                             has.CountrySpecified = true;
                         }
                         else if (countrycode == (int)Country.scotland)
                         {
                             has.Country = CountryType.scotland;
                             has.CountrySpecified = true;
                         }
                         else if (countrycode == (int)Country.wales)
                         {
                             has.Country = CountryType.wales;
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

        public static Contact GetContactDetails(int contactId)
        {
            
                Contact cntDetails = new Contact();

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                cntDetails,
                 UserSchema.Portal + ".GET_CONTACT_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_contact_id", contactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 (records, instance) =>
                 {
                     instance.ContactName = records.GetStringOrDefault("First_name") + " " + records.GetStringOrDefault("Sur_name");
                     instance.OrganisationName = records.GetStringOrDefault("ORGNAME");
                 }

                );

                return cntDetails;
            

        }

        public static SignedOrderSummaryStructure GetSODetail(string specialOrderESDALRefNo)
        {
            
                SignedOrderSummaryStructure soss = new SignedOrderSummaryStructure();

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    soss,
                     UserSchema.Portal + ".GET_SO_DETAILS",
                    parameter =>
                    {
                        parameter.AddWithValue("P_ESDAL_REF_NUMBER", specialOrderESDALRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                       (records, instance) =>
                       {

                           instance.OrderNumber = records.GetStringOrDefault("ORDER_NO");
                           instance.SignedOn = records.GetDateTimeOrDefault("SIGNED_DATE");
                           instance.ExpiresOn = records.GetDateTimeOrDefault("EXPIRY_DATE");
                           instance.SignedBy = records.GetStringOrDefault("SIGNATORY");
                       }
                      );
                return soss;
            
        }


        #region public string GetImminentForCountries(int Orgid,string ImminentStatus)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Orgid"></param>
        /// <param name="ImminentStatus">status which contain country Id also for summited notification</param>
        /// <returns></returns>
        public static bool GetImminentForCountries(int Orgid, string ImminentStatus)
        {
            int countryIDByORG = 0;
            bool ImminentFlag = false;// Added for NEN project
            string[] arrayOfContry = new string[6];
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance
            (
                countryIDByORG,
                 UserSchema.Portal + ".SP_GET_COUNTRYID_BY_ORG",
                 parameter =>
                 {
                     parameter.AddWithValue("P_OrganisationId", Orgid, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                 },
                 records =>
                 {
                     countryIDByORG = records.GetInt32OrDefault("COUNTRY_ID");// get country Id for organisation
                 }
            );

            try
            {
                arrayOfContry = ImminentStatus.Split(',');
                string countryIDOrg = countryIDByORG.ToString();

                if (countryIDByORG != 0)
                {
                    foreach (string ImminentCountryId in arrayOfContry)
                    {
                        if (ImminentCountryId != "")
                        {
                            if (countryIDOrg.Contains(ImminentCountryId))
                            {
                                ImminentFlag = true;
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
                //do nothing
                ImminentFlag = false;
            }
            return ImminentFlag;
        }
        #endregion
    }
}