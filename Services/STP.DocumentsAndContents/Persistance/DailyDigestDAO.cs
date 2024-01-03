using DailyDigestXSD;
using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.DataAccess.SafeProcedure;
using STP.Domain.VehiclesAndFleets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.DocumentsAndContents.Persistance
{
    public static class DailyDigestDAO
    {
        /// <summary>
        /// get daily digest detail
        /// </summary>
        /// <param name="contactId">contact id</param>
        /// <param name="notificationId">notification id</param>
        /// <returns></returns>
        public static DigestStructure GetDailyDigestDetails(int contactId, string notificationId)
        {
            DigestStructure stru = new DigestStructure();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                stru,
                 UserSchema.Portal + ".GET_DAILY_DIGEST_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("p_CONTACT_ID", contactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       List<RecipientContactStructure> rcsclplist = new List<RecipientContactStructure>();

                       RecipientContactStructure rcs1 = new RecipientContactStructure();

                       rcs1.ContactId = contactId;
                       rcs1.ContactIdSpecified = true;

                       string contact = string.Empty;
                       if (records.GetStringOrDefault("TITLE") != string.Empty)
                           contact = records.GetStringOrDefault("TITLE");
                       if (records.GetStringOrDefault("FIRST_NAME") != string.Empty)
                           contact = contact + " " + records.GetStringOrDefault("FIRST_NAME");
                       if (records.GetStringOrDefault("SUR_NAME") != string.Empty)
                           contact = contact + " " + records.GetStringOrDefault("SUR_NAME");
                       rcs1.ContactName = contact;

                       if (Convert.ToString(records.GetLongOrDefault("organisation_id")) != string.Empty)
                       {
                           rcs1.OrganisationId = Convert.ToInt32(records.GetLongOrDefault("organisation_id"));
                           rcs1.OrganisationIdSpecified = true;
                       }

                       rcs1.OrganisationName = records.GetStringOrDefault("orgname");

                       rcsclplist.Add(rcs1);

                       instance.Recipients = rcsclplist.ToArray();

                       instance.DigestGeneration = DateTime.Now;

                       instance.Item = GetItemDetails(notificationId, rcs1.OrganisationId);
                   }
            );

            return stru;
        }

        /// <summary>
        /// get item detail for specif notification
        /// </summary>
        /// <param name="notificationId">notification id</param>
        /// <param name="organisationId">organisation id</param>
        /// <returns></returns>
        public static DigestItemStructure[] GetItemDetails(string notificationId, int organisationId)
        {
            string[] notificationidArr = notificationId.Split(",".ToCharArray());

            List<DigestItemStructure> digestItemstruList = new List<DigestItemStructure>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                digestItemstruList,
                 UserSchema.Portal + ".GET_ITEM_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIFICATION_ID", notificationId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ITEMS_COUNT", notificationidArr.Length, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORGANISATION_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                           #region ESDALRefrenceNumber

                           ESDALReferenceNumberStructure esdalrefnostru = new ESDALReferenceNumberStructure();
                       esdalrefnostru.Mnemonic = records.GetStringOrDefault("HAULIER_MNEMONIC");
                       string[] hauMnemonicArr = records.GetStringOrDefault("NOTIFICATION_CODE").Split("/".ToCharArray());
                       if (hauMnemonicArr.Length > 0)
                       {
                           esdalrefnostru.MovementProjectNumber = hauMnemonicArr[1];
                       }

                       MovementVersionNumberStructure movvernostru = new MovementVersionNumberStructure();
                       movvernostru.Value = records.GetShortOrDefault("NOTIFICATION_VERSION_NO");
                       esdalrefnostru.MovementVersion = movvernostru;
                       esdalrefnostru.EnteredBySORT = false;
                       esdalrefnostru.EnteredBySORTSpecified = false;
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
                           if (Convert.ToString(records.GetDecimalOrDefault("ITEM_TYPE")) == "312001")
                       {
                           instance.Type = ItemType.soproposedroute;
                       }
                       else if (Convert.ToString(records.GetDecimalOrDefault("ITEM_TYPE")) == "312002")
                       {
                           instance.Type = ItemType.soreproposedroute;
                       }
                       else if (Convert.ToString(records.GetDecimalOrDefault("ITEM_TYPE")) == "312005")
                       {
                           instance.Type = ItemType.soagreedroute;
                       }
                       else if (Convert.ToString(records.GetDecimalOrDefault("ITEM_TYPE")) == "312007")
                       {
                           instance.Type = ItemType.soreclearedagreedroute;
                       }
                       else if (Convert.ToString(records.GetDecimalOrDefault("ITEM_TYPE")) == "312008")
                       {
                           instance.Type = ItemType.nolongeraffected;
                       }
                       else if (Convert.ToString(records.GetDecimalOrDefault("ITEM_TYPE")) == "312009")
                       {
                           instance.Type = ItemType.notification;
                       }
                       else if (Convert.ToString(records.GetDecimalOrDefault("ITEM_TYPE")) == "312010")
                       {
                           instance.Type = ItemType.renotification;
                       }
                       else
                       {
                           instance.Type = ItemType.notification;
                       }

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

                       instance.FirstMoveDate = records.GetDateTimeOrDefault("MOVE_START_DATE");
                       instance.HaulierName = records.GetStringOrDefault("HAULIER_NAME");

                       if (records.GetDecimalOrDefault("UNOPENED") == 1)
                       {
                           instance.Reason = InclusionReasonType.unopened;
                       }
                       else
                       {
                           instance.Reason = InclusionReasonType.imminentmove;
                       }
                   }
            );

            return digestItemstruList.ToArray();
        }
    }
}