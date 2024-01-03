using NotificationXSD;
using Oracle.DataAccess.Client;
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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using STP.Common.Constants;
using STP.Domain.RouteAssessment.XmlAnalysedCautions;
using STP.Common.General;
using System.Xml.Serialization;
using STP.Domain.Structures;
using STP.Domain.MovementsAndNotifications.Notification;

namespace STP.DocumentsAndContents.Persistance
{
    public static class OutBoundDAO
    {
        public static NotificationXSD.PredefinedCautionsDescriptionsStructure1 Pcds { get; set; }

        public static decimal TotalDistanceMetric { get; set; }

        public static decimal TotalDistanceImperial { get; set; }

        #region GetOutboundNotificationDetailsForNotification
        public static OutboundNotificationStructure GetOutboundNotificationDetailsForNotification(int NotificationID, bool isHaulier, long ContactID, int OrganisationId=0, int IsNen=0)
        {
            string messg = "OutboundDAO/GetOutboundNotificationDetailsForNotification?NotificationID=" + NotificationID + ", isHaulier=" + isHaulier + ", ContactID=" + ContactID;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));
            OutboundNotificationStructure odns = new OutboundNotificationStructure();
            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    odns,
                    UserSchema.Portal + ".GET_OUTBOUND_DETAILS",
                    parameter =>
                    {
                        parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_CONTACT_ID", ContactID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                       (records, instance) =>
                       {
                           NotificationXSD.ESDALReferenceNumberStructure esdalrefnostru = new NotificationXSD.ESDALReferenceNumberStructure();
                           NotificationVR1InformationStructure vr1Info = new NotificationVR1InformationStructure();
                           VR1NumbersStructure vr1Str = new VR1NumbersStructure();

                           esdalrefnostru.Mnemonic = records.GetStringOrDefault("HAULIER_MNEMONIC");
                           string[] hauMnemonicArr = records.GetStringOrDefault("NOTIFICATION_CODE").Split("/".ToCharArray());
                           if (hauMnemonicArr.Length > 0)
                           {
                               esdalrefnostru.MovementProjectNumber = hauMnemonicArr[1];
                           }
                           instance.NotificationOnEscort = records.GetStringOrDefault("NOTESONESCORT");

                           NotificationXSD.MovementVersionNumberStructure movvernostru = new NotificationXSD.MovementVersionNumberStructure();
                           movvernostru.Value = records.GetShortOrDefault("VERSION_NO");
                           esdalrefnostru.MovementVersion = movvernostru;

                           esdalrefnostru.NotificationNumber = records.GetShortOrDefault("NOTIFICATION_NO");
                           esdalrefnostru.NotificationNumberSpecified = true;
                           esdalrefnostru.ESDALReferenceNo = records.GetStringOrDefault("NOTIFICATION_CODE");
                           esdalrefnostru.VSONo = records.GetStringOrDefault("VSO_NUMBER");
                           instance.ESDALReferenceNumber = esdalrefnostru;

                           // Start for #3846
                           vr1Str.Scottish = records.GetStringOrDefault("VR1_NUMBER");
                           vr1Info.Numbers = vr1Str;
                           instance.VR1Information = vr1Info;
                           // End for #3846

                           #region Vehicle Classification
                           int vehicleClass = records.GetInt32OrDefault("vehicle_classification");
                           switch (vehicleClass)
                           {
                               case (int)VehicleEnums.VehicleClassificationType.VehicleSpecialOrder:
                                   instance.Classification = MovementClassificationType.vehiclespecialorder;
                                   break;
                               case (int)VehicleEnums.VehicleClassificationType.SpecialOrder:
                                   instance.Classification = MovementClassificationType.specialorder;
                                   break;
                               case (int)VehicleEnums.VehicleClassificationType.StgoailCat1:
                                   instance.Classification = MovementClassificationType.stgoailcat1;
                                   break;
                               case (int)VehicleEnums.VehicleClassificationType.StgoailCat2:
                                   instance.Classification = MovementClassificationType.stgoailcat2;
                                   break;
                               case (int)VehicleEnums.VehicleClassificationType.StgoailCat3:
                                   instance.Classification = MovementClassificationType.stgoailcat3;
                                   break;
                               case (int)VehicleEnums.VehicleClassificationType.StgoMobileCraneCata:
                                   instance.Classification = MovementClassificationType.stgomobilecranecata;
                                   break;
                               case (int)VehicleEnums.VehicleClassificationType.StgoMobileCraneCatb:
                                   instance.Classification = MovementClassificationType.stgomobilecranecatb;
                                   break;
                               case (int)VehicleEnums.VehicleClassificationType.StgoMobileCraneCatc:
                                   instance.Classification = MovementClassificationType.stgomobilecranecatc;
                                   break;
                               case (int)VehicleEnums.VehicleClassificationType.StgoRoadRecoveryVehicle:
                               case (int)VehicleEnums.VehicleClassificationType.StgoCat1RoadRecovery:
                               case (int)VehicleEnums.VehicleClassificationType.StgoCat2RoadRecovery:
                               case (int)VehicleEnums.VehicleClassificationType.StgoCat3RoadRecovery:
                               case (int)VehicleEnums.VehicleClassificationType.StgoCat1Vr1RoadRecovery:
                               case (int)VehicleEnums.VehicleClassificationType.StgoCat2Vr1RoadRecovery:
                               case (int)VehicleEnums.VehicleClassificationType.StgoCat3Vr1RoadRecovery:
                                   instance.Classification = MovementClassificationType.stgoroadrecoveryvehicle;
                                   break;
                               case (int)VehicleEnums.VehicleClassificationType.Tracked:
                                   instance.Classification = MovementClassificationType.tracked;
                                   break;
                               case (int)VehicleEnums.VehicleClassificationType.StgoEngineeringPlantWheeled:
                               case (int)VehicleEnums.VehicleClassificationType.StgoCat1EngineeringPlantWheeled:
                               case (int)VehicleEnums.VehicleClassificationType.StgoCat2EngineeringPlantWheeled:
                               case (int)VehicleEnums.VehicleClassificationType.StgoCat3EngineeringPlantWheeled:
                               case (int)VehicleEnums.VehicleClassificationType.StgoCat1Vr1EngineeringPlantWheeled:
                               case (int)VehicleEnums.VehicleClassificationType.StgoCat2Vr1EngineeringPlantWheeled:
                               case (int)VehicleEnums.VehicleClassificationType.StgoCat3Vr1EngineeringPlantWheeled:
                                   instance.Classification = MovementClassificationType.stgoengineeringplantwheeled;
                                   break;
                               case (int)VehicleEnums.VehicleClassificationType.StgoEngineeringPlantTracked:
                               case (int)VehicleEnums.VehicleClassificationType.StgoCat1EngineeringPlantTracked:
                               case (int)VehicleEnums.VehicleClassificationType.StgoCat2EngineeringPlantTracked:
                               case (int)VehicleEnums.VehicleClassificationType.StgoCat3EngineeringPlantTracked:
                               case (int)VehicleEnums.VehicleClassificationType.StgoCat1Vr1EngineeringPlantTracked:
                               case (int)VehicleEnums.VehicleClassificationType.StgoCat2Vr1EngineeringPlantTracked:
                               case (int)VehicleEnums.VehicleClassificationType.StgoCat3Vr1EngineeringPlantTracked:
                                   instance.Classification = MovementClassificationType.stgoengineeringplanttracked;
                                   break;
                                   case (int)VehicleEnums.VehicleClassificationType.WheeledConstructionAndUse:
                                   instance.Classification = MovementClassificationType.wheeledconstructionanduse;
                                   break;
                           }
                           #endregion

                           string specialOrderESDALRefNo = esdalrefnostru.Mnemonic + "/" + esdalrefnostru.MovementProjectNumber + "/S" + movvernostru.Value;
                           SignedOrderSummaryStructure soss;
                           soss = GetSpecialOrderNo(specialOrderESDALRefNo);

                           instance.DftReference = soss.OrderNumber == null ? records.GetStringOrDefault("SO_NUMBERS") : soss.OrderNumber;

                           instance.ClientName = records.GetStringOrDefault("Client_Descr");

                           instance.JobFileReference = records.GetStringOrDefault("HA_JOB_FILE_REF");

                           #region Haulier Details
                           NotificationXSD.HaulierDetailsStructure hds = new NotificationXSD.HaulierDetailsStructure();
                           instance.HauliersReference = records.GetStringOrDefault("hauliers_ref");
                           hds.HaulierContact = records.GetStringOrDefault("HAULIER_CONTACT");
                           hds.HaulierName = records.GetStringOrDefault("HAULIER_NAME");
                           hds.TelephoneNumber = records.GetStringOrDefault("HAULIER_TEL_NO");
                           hds.FaxNumber = records.GetStringOrDefault("HAULIER_Fax_NO");
                           hds.EmailAddress = records.GetStringOrDefault("HAULIER_Email");
                           hds.OrganisationId = records.GetLongOrDefault("organisation_id");
                           hds.OrganisationIdSpecified = records.GetLongOrDefault("organisation_id") > 0;
                           hds.Licence = records.GetStringOrDefault("HAULIER_LICENCE_NO");

                           NotificationXSD.AddressStructure has = new NotificationXSD.AddressStructure();

                           string[] Addstru = new string[5];
                           Addstru[0] = records.GetStringOrDefault("HAULIER_ADDRESS_1");
                           Addstru[1] = records.GetStringOrDefault("HAULIER_ADDRESS_2");
                           Addstru[2] = records.GetStringOrDefault("HAULIER_ADDRESS_3");
                           Addstru[3] = records.GetStringOrDefault("HAULIER_ADDRESS_4");
                           Addstru[4] = records.GetStringOrDefault("HAULIER_ADDRESS_5");
                           has.Line = Addstru;
                           has.PostCode = records.GetStringOrDefault("haulier_post_code");
                           int country = records.GetInt32OrDefault("COUNTRY");
                           if (country == (int)Country.england)
                           {
                               has.Country = NotificationXSD.CountryType.england;
                               has.CountrySpecified = true;
                           }
                           else if (country == (int)Country.northernireland)
                           {
                               has.Country = NotificationXSD.CountryType.northernireland;
                               has.CountrySpecified = true;
                           }
                           else if (country == (int)Country.scotland)
                           {
                               has.Country = NotificationXSD.CountryType.scotland;
                               has.CountrySpecified = true;
                           }
                           else if (country == (int)Country.wales)
                           {
                               has.Country = NotificationXSD.CountryType.wales;
                               has.CountrySpecified = true;
                           }
                           hds.HaulierAddress = has;
                           instance.HaulierDetails = hds;
                           #endregion

                           #region JourneyFromToSummary
                           JourneyFromToSummaryStructure jftss = new JourneyFromToSummaryStructure();

                           jftss.From = records.GetStringOrDefault("FROM_DESCR");

                           jftss.To = records.GetStringOrDefault("TO_DESCR");

                           instance.JourneyFromToSummary = jftss;
                           #endregion

                           #region JourneyFromTo
                           JourneyFromToStructure jfts = new JourneyFromToStructure();
                           jfts.From = records.GetStringOrDefault("FROM_DESCR");

                           jfts.To = records.GetStringOrDefault("TO_DESCR");

                           instance.JourneyFromTo = jfts;
                           #endregion

                           #region JourneyTiming
                           JourneyTimingStructure jts = new JourneyTimingStructure();
                           jts.FirstMoveDate = records.GetDateTimeOrDefault("movement_start_date");
                           jts.LastMoveDate = records.GetDateTimeOrDefault("movement_end_date");
                           jts.LastMoveDateSpecified = true;
                           jts.StartTime = Convert.ToString(jts.FirstMoveDate.TimeOfDay);
                           jts.EndTime = Convert.ToString(jts.LastMoveDate.TimeOfDay);
                           instance.JourneyTiming = jts;
                           #endregion

                           #region LoadDetails
                           LoadDetailsStructure lds = new LoadDetailsStructure();
                           lds.Description = records.GetStringOrDefault("load_descr");
                           lds.TotalMoves = Convert.ToString(records.GetInt16OrDefault("NO_OF_MOVES"));
                           lds.MaxPiecesPerMove = Convert.ToString(records.GetInt32OrDefault("max_pieces_per_move"));
                           lds.MaxPiecesPerMoveSpecified = true;
                           instance.LoadDetails = lds;
                           instance.NotificationNotesFromHaulier = records.GetStringOrDefault("HAUL_NOTES");
                           InboundNotificationStructure inBoundStructure = new InboundNotificationStructure();
                           inBoundStructure.OnBehalfOf = records.GetStringOrDefault("ON_BEHALF_OF");
                           instance.OnBehalfOf = inBoundStructure.OnBehalfOf;
                           #endregion

                           #region Indemnity Information

                           IndemnityStructure indemnityDetails = new IndemnityStructure();

                           bool isIndemnityPresent = records.GetInt16OrDefault("INDEMNITY_CONFIRMATION") > 0;

                           instance.IsIndemnityConfirmed = isIndemnityPresent;

                           short totalMoves = 0;

                           if (lds.TotalMoves != string.Empty)
                           {
                               totalMoves = Convert.ToInt16(lds.TotalMoves);
                           }
                           indemnityDetails.MultipleMoves = totalMoves > 1;

                           indemnityDetails.Confirmed = isIndemnityPresent;

                           indemnityDetails.OnBehalfOf = instance.OnBehalfOf;

                           indemnityDetails.Haulier = records.GetStringOrDefault("HAULIER_CONTACT");

                           indemnityDetails.SignedDate = DateTime.Today;

                           indemnityDetails.Signatory = records.GetStringOrDefault("HAULIER_CONTACT");

                           MovementTimingStructure movementStructure = new MovementTimingStructure();
                           MovementTimingStructureMovementDateRange movementTiming = new MovementTimingStructureMovementDateRange();

                           movementTiming.FromDate = jts.FirstMoveDate;
                           movementTiming.ToDate = jts.LastMoveDate;

                           movementStructure.Item = movementTiming;

                           indemnityDetails.Timing = movementStructure;

                           if (!isIndemnityPresent)
                               instance.IndemnityConfirmation = null;
                           else
                               instance.IndemnityConfirmation = indemnityDetails;

                           #endregion

                           #region Dispensation

                           List<NotifDispensations> notifDispensation = GetNotificationDispensation(NotificationID, 2);
                           if (notifDispensation != null && notifDispensation.Any())
                           {
                               List<OutboundDispensationStructure> dispensationDetails = new List<OutboundDispensationStructure>();
                               OutboundDispensationStructure dispensationDtl;
                               foreach (var item in notifDispensation)
                               {
                                   dispensationDtl = new OutboundDispensationStructure
                                   {
                                       DRN = item.DRN,
                                       Summary = item.Summary,
                                       GrantedBy = item.GrantorName
                                   };
                                   dispensationDetails.Add(dispensationDtl);
                               }
                               instance.Dispensations = dispensationDetails.ToArray();
                           }

                           #endregion

                           instance.SentDateTime = records.GetDateTimeOrEmpty("notification_date");

                           #region SOInformation
                           try
                           {
                               instance.SOInformation = GetSODetail(specialOrderESDALRefNo);
                           }
                           catch { }

                           if (instance.SOInformation != null && instance.SOInformation.Summary != null && instance.SOInformation.Summary[0].CurrentOrder != null && instance.SOInformation.Summary[0].CurrentOrder.OrderNumber != null)
                           {
                               instance.JobFileReference = instance.SOInformation.Summary[0].CurrentOrder.HAJobRefNumber;
                           }

                           #endregion

                           RouteAnalysisModel routeAnalysis = GetRouteAssessmentDetails(NotificationID, OrganisationId, IsNen);

                           #region Recipients
                           instance.Recipients = GetRecipientContactStructure(NotificationID, routeAnalysis.AffectedParties).ToArray();
                           #endregion

                           #region RouteParts
                           List<RoutePartsStructureRoutePartListPosition> rpsrplpList = new List<RoutePartsStructureRoutePartListPosition>();
                           rpsrplpList = GetRouteParts(NotificationID, isHaulier, OrganisationId, IsNen, routeAnalysis);
                           instance.RouteParts = rpsrplpList.ToArray();
                           if (instance.RouteParts != null)
                               instance.RouteParts = instance.RouteParts.Where(x => x.RoutePart != null).ToArray();

                           #endregion

                           instance.OldNotificationID = Convert.ToInt64(records.GetDecimalOrDefault("old_noti"));
                           instance.OrganisationName = records.GetStringOrDefault("orgname");
                       });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - GetOutboundNotificationDetailsForNotification,Exception:" + ex);
            }
            return odns;
        }
        #endregion

        #region GetRouteParts
        public static List<RoutePartsStructureRoutePartListPosition> GetRouteParts(int NotificationID, bool isHaulier, int OrganisationId, int IsNen=0, RouteAnalysisModel routeAnalysis = null)
        {
            string messg = "OutboundDAO/GetRouteParts?NotificationID=" + NotificationID + ", isHaulier=" + isHaulier;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            long routepartid = 0;
            int legNumber = 1;
            bool ReturnFlag = false; //RM#3659
            int counter = 0; //RM#3659
            string EsdalRefNumber = "1#";
            /// <summary>
            /// This sessions are showing an exception during testing of API. Below sessions are not used.
            /// Used for maintaining logs
            ///  if (HttpContext.Current.Session["ReturnFlag"] != null) //RM#3659
            ///  {
            ///  ReturnFlag = Convert.ToBoolean(HttpContext.Current.Session["ReturnFlag"]);
            /// 
            /// string messgForReturnFlag = "OutboundDAO/GetRouteParts?ReturnFlag=" + ReturnFlag;
            /// Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messgForReturnFlag + "; ReturnFlag"));
            /// }
            /// if (HttpContext.Current.Session["ESDALRefNumber"] != null) //RM#3659
            /// {
            ///  EsdalRefNumber = Convert.ToString(HttpContext.Current.Session["ESDALRefNumber"]);

            ///  string messgForRefNum = "OutboundDAO/GetRouteParts?EsdalRefNumber=" + EsdalRefNumber;
            ///  Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messgForRefNum + "; EsdalRefNumber"));
            ///  }
            /// </summary>
            /// 
            string spName = ".GET_MULTI_ROUTEVEHICLE_DETAILS";
            if (IsNen == 1)
                spName = ".STP_NON_ESDAL_DOCUMENT.SP_GET_NEN_PDF_DOCUMENT";
            else if (IsNen == 2)
                spName = ".STP_NON_ESDAL_DOCUMENT.SP_GET_NEN_API_DOCUMENT";
            List<OutBoundDoc> outBoundDoc = new List<OutBoundDoc>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               outBoundDoc,
              UserSchema.Portal + spName,
               parameter =>
               {
                   parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   if (IsNen > 0)
                       parameter.AddWithValue("P_ORGANISATION_ID", OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);

               },
                  (records, instance) =>
                  {
                      try
                      {
                          instance.RoutePartId = records.GetLongOrDefault("route_part_id");
                      }
                      catch (Exception ex)
                      {
                          Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                      }

                      try
                      {
                          instance.VehicleId = records.GetLongOrDefault("vehicle_id");
                      }
                      catch (Exception ex)
                      {
                          Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                      }

                      instance.VehicleDesc = records.GetStringOrDefault("vehicle_desc");
                      var componentType = records.GetStringOrDefault("COMPONENT_TYPE");
                      var vehicleType = records.GetStringOrDefault("vehicle_type");

                      try
                      {
                          if (vehicleType != null && vehicleType != string.Empty)
                          {
                              if(vehicleType.ToLower()== "rigid vehicle")
                              {
                                  try
                                  {
                                      instance.Length = (records["rigid_len"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("rigid_len");
                                  }
                                  catch (Exception ex)
                                  {
                                      Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                                  }
                              }
                              else
                              {
                                  instance.Length = (records["len"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("len");
                              }
                          }
                          else
                            instance.Length = (records["len"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("len");
                      }
                      catch (Exception ex)
                      {
                          Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                      }

                      try
                      {
                          instance.RearOverhang = (records["REAR_OVERHANG"]).ToString() == string.Empty ? 0 : records.GetFieldType("REAR_OVERHANG").Name == "Decimal" ? 
                          (double)records.GetDecimalOrDefault("REAR_OVERHANG") : records.GetDoubleOrDefault("REAR_OVERHANG");

                      }
                      catch (Exception ex)
                      {
                          Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                      }

                      try
                      {
                          if (vehicleType.ToLower() == "crane")
                          {
                              try
                              {
                                  instance.RigidLength = (records["len"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("len");
                              }
                              catch (Exception ex)
                              {
                                  Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                              }
                          }
                          else
                              instance.RigidLength = (records["rigid_len"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("rigid_len");
                      }
                      catch (Exception ex)
                      {
                          Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                      }

                      try
                      {
                          if ((records["front_overhang"]).ToString() == string.Empty)
                          {
                              instance.FrontOverhang = 0;
                          }
                          else
                          {
                              try
                              {
                                  instance.FrontOverhang = records.GetDoubleOrDefault("front_overhang");
                              }
                              catch
                              {
                                  instance.FrontOverhang = Convert.ToDouble(records.GetDecimalOrDefault("front_overhang"));
                              }
                          }
                      }
                      catch (Exception ex)
                      {
                          Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                      }

                      try
                      {
                          instance.Width = (records["width"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("width");
                      }
                      catch (Exception ex)
                      {
                          Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                      }

                      try
                      {
                          instance.MaximumHeight = (records["max_height"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("max_height");
                      }
                      catch (Exception ex)
                      {
                          Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                      }

                      try
                      {
                          instance.RedHeight = (records["Red_Height"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("Red_Height");
                      }
                      catch (Exception ex)
                      {
                          Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                      }

                      try
                      {
                          instance.GrossWeight = (records["gross_weight"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("gross_weight");
                      }
                      catch (Exception ex)
                      {
                          Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                      }

                      try
                      {
                          instance.MaximumAxleWeight = (records["MAX_AXLE_WEIGHT"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("MAX_AXLE_WEIGHT");
                      }
                      catch (Exception ex)
                      {
                          Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                      }

                      instance.PlannedContentRefNo = records.GetStringOrDefault("planned_content_ref_no");

                      try
                      {
                          instance.IsSteerableAtRear = (records["is_steerable_at_rear"]).ToString() == string.Empty ? 0 : records.GetDecimalOrDefault("is_steerable_at_rear");
                      }
                      catch (Exception ex)
                      {
                          Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                      }

                      try
                      {
                          instance.WheelBase = (records["wheelbase"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("wheelbase");
                      }
                      catch (Exception ex)
                      {
                          Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                      }

                      try
                      {
                          instance.GroundClearance = (records["ground_clearance"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("ground_clearance");
                      }
                      catch (Exception ex)
                      {
                          Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                      }

                      try
                      {
                          instance.OutsideTrack = (records["outside_track"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("outside_track");
                      }
                      catch (Exception ex)
                      {
                          Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                      }

                      //=====================================================
                      try
                      {
                          instance.ComparisonId = (records["comparison_id"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("comparison_id");
                      }
                      catch
                      {
                          try
                          {
                              instance.ComparisonId = (records["comparison_id"]).ToString() == string.Empty ? 0 : records.GetLongOrDefault("comparison_id");
                          }
                          catch (Exception ex)
                          {
                              Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                          }
                      }
                      //======================================================

                      try
                      {
                          instance.RoutePartNo = (records["route_part_no"]).ToString() == string.Empty ? 0 : records.GetDecimalOrDefault("route_part_no");
                      }
                      catch (Exception ex)
                      {
                          Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                      }


                      instance.PartDescr = records.GetStringOrDefault("part_descr");

                      try
                      {
                          instance.TransportMode = (records["transport_mode"]).ToString() == string.Empty ? 0 : records.GetInt32OrDefault("transport_mode");
                      }
                      catch (Exception ex)
                      {
                          Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                      }

                      instance.PartName = records.GetStringOrDefault("PART_NAME");
                      instance.Name = records.GetStringOrDefault("name");
                      instance.ComponentType = records.GetStringOrDefault("component_type");
                      instance.VehicleType = records.GetStringOrDefault("vehicle_type");
                      instance.ComponentSubtype = records.GetStringOrDefault("COMPONENT_SUBTYPE");

                      try
                      {
                          instance.SpaceToFollowing = (records["SPACE_TO_FOLLOWING"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("SPACE_TO_FOLLOWING");
                      }
                      catch (Exception ex)
                      {
                          Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                      }

                      try
                      {
                          instance.LeftOverhang = (records["LEFT_OVERHANG"]).ToString() == string.Empty ? 0 : records.GetFieldType("LEFT_OVERHANG").Name=="Decimal"?(double)records.GetDecimalOrDefault("LEFT_OVERHANG"): records.GetDoubleOrDefault("LEFT_OVERHANG");
                      }
                      catch (Exception ex)
                      {
                          Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                      }

                      try
                      {
                          instance.RightOverhang = (records["RIGHT_OVERHANG"]).ToString() == string.Empty ? 0 : records.GetFieldType("RIGHT_OVERHANG").Name == "Decimal" ? (double)records.GetDecimalOrDefault("RIGHT_OVERHANG") : records.GetDoubleOrDefault("RIGHT_OVERHANG");
                      }
                      catch (Exception ex)
                      {
                          Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                      }

                      try
                      {
                          instance.RedGroundClearance = (records["RED_GROUND_CLEARANCE"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("RED_GROUND_CLEARANCE");
                      }
                      catch (Exception ex)
                      {
                          Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                      }

                      instance.GrossTrainWeight = (records["TRAIN_WEIGHT"]).ToString() == string.Empty ? 0 : Convert.ToDouble(records["TRAIN_WEIGHT"]);

                  }
           );

            List<RoutePartsStructureRoutePartListPosition> rpsrplp = new List<RoutePartsStructureRoutePartListPosition>(); 
            
            spName = ".GET_VEHICLE_DETAILS";
            if (IsNen == 1)
                spName = ".STP_NON_ESDAL_DOCUMENT.SP_GET_NEN_PDF_VEHICLES";
            else if (IsNen == 2)
                spName = ".STP_NON_ESDAL_DOCUMENT.SP_GET_NEN_API_VEHICLES";
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                rpsrplp,
                UserSchema.Portal + spName,
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    if (IsNen > 0)
                        parameter.AddWithValue("P_ORGANISATION_ID", OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       PlannedRoutePartStructure2 prps = new PlannedRoutePartStructure2();
                       string RoutePartName = string.Empty;
                       prps.Id = Convert.ToInt32(records.GetLongOrDefault("route_part_id"));

                       prps.LegNumber = Convert.ToString(legNumber);

                       routepartid = records.GetLongOrDefault("route_part_id");

                       List<OutBoundDoc> outBoundDocVehicle = new List<OutBoundDoc>();
                       outBoundDocVehicle = GetVehicleMaxHeight(routepartid);

                       prps.Name = records.GetStringOrDefault("PART_NAME");

                       if (records.GetStringOrDefault("name") == "road")
                       {
                           prps.Mode = NotificationXSD.ModeOfTransportType.road;
                       }
                       else if (records.GetStringOrDefault("name") == "air")
                       {
                           prps.Mode = NotificationXSD.ModeOfTransportType.air;
                       }
                       else if (records.GetStringOrDefault("name") == "rail")
                       {
                           prps.Mode = NotificationXSD.ModeOfTransportType.rail;
                       }
                       else if (records.GetStringOrDefault("name") == "sea")
                       {
                           prps.Mode = NotificationXSD.ModeOfTransportType.sea;
                       }

                       PlannedRoadRoutePartStructure prp = new PlannedRoadRoutePartStructure();

                       #region StartPointListPosition
                       PlannedRoadRoutePartStructureStartPointListPosition[] prrpssplpList = new PlannedRoadRoutePartStructureStartPointListPosition[1];
                       PlannedRoadRoutePartStructureStartPointListPosition prrpssplp = new PlannedRoadRoutePartStructureStartPointListPosition();

                       prrpssplp.StartPoint = GetSimplifiedRoutePointStart(NotificationID, routepartid);
                       prrpssplpList[0] = prrpssplp;
                       prp.StartPointListPosition = prrpssplpList;
                       #endregion
                       #region EndPointListPosition
                       PlannedRoadRoutePartStructureEndPointListPosition[] prrpseplpList = new PlannedRoadRoutePartStructureEndPointListPosition[1];
                       PlannedRoadRoutePartStructureEndPointListPosition prrpseplp = new PlannedRoadRoutePartStructureEndPointListPosition();

                       prrpseplp.EndPoint = GetSimplifiedRoutePointEnd(NotificationID, routepartid);
                       prrpseplpList[0] = prrpseplp;
                       prp.EndPointListPosition = prrpseplpList;
                       #endregion

                       int recordcount = 0;
                       var outBoundDocVar = outBoundDoc.Where(r => r.RoutePartId.Equals(routepartid)).ToList();
                       NotificationXSD.VehiclesSummaryStructure vss = new NotificationXSD.VehiclesSummaryStructure();
                       outBoundDoc.RemoveAll(r => r.RoutePartId.Equals(routepartid));

                       if ((EsdalRefNumber.Contains("#") && ReturnFlag) || (!ReturnFlag && counter == 0)) //RM#3659 If condition added to show 1 vehicle details for simplified noti when return route is selected
                       {
                           if (ReturnFlag)
                           {
                               counter++;
                               ReturnFlag = false;
                           }

                           if (outBoundDocVar.Count > 0)
                           {
                               #region ConfigurationSummaryListPosition
                               NotificationXSD.VehiclesSummaryStructureConfigurationSummaryListPosition[] vscslpList = new NotificationXSD.VehiclesSummaryStructureConfigurationSummaryListPosition[outBoundDocVar.Count];
                               recordcount = 0;
                               foreach (var outbound in outBoundDocVar)
                               {
                                   NotificationXSD.VehiclesSummaryStructureConfigurationSummaryListPosition vscslp = new NotificationXSD.VehiclesSummaryStructureConfigurationSummaryListPosition();
                                   vscslp.ConfigurationSummary = outbound.VehicleDesc;
                                   vscslpList[recordcount] = vscslp;
                                   recordcount++;
                               }
                               vss.ConfigurationSummaryListPosition = vscslpList;
                               #endregion

                               #region OverallLengthListPosition
                               NotificationXSD.VehiclesSummaryStructureOverallLengthListPosition[] vssollpList = new NotificationXSD.VehiclesSummaryStructureOverallLengthListPosition[outBoundDocVar.Count];
                               recordcount = 0;
                               foreach (var outbound in outBoundDocVar)
                               {
                                   NotificationXSD.VehiclesSummaryStructureOverallLengthListPosition vssollp = new NotificationXSD.VehiclesSummaryStructureOverallLengthListPosition();
                                   NotificationXSD.SummaryLengthStructure sls = new NotificationXSD.SummaryLengthStructure();
                                   sls.IncludingProjections = NullableOrNotForDecimal(outbound.Length);

                                   sls.ExcludingProjections = sls.IncludingProjections - (NullableOrNotForDecimal(outbound.RearOverhang) + NullableOrNotForDecimal(outbound.FrontOverhang));

                                   if (sls.ExcludingProjections != null)
                                   {
                                       sls.ExcludingProjectionsSpecified = true;
                                   }
                                   else
                                   {
                                       sls.ExcludingProjectionsSpecified = false;
                                   }

                                   vssollp.OverallLength = sls;
                                   vssollpList[recordcount] = vssollp;
                                   recordcount++;
                               }
                               vss.OverallLengthListPosition = vssollpList;
                               #endregion

                               #region RigidLengthListPosition
                               NotificationXSD.VehiclesSummaryStructureRigidLengthListPosition[] vssrllpList = new NotificationXSD.VehiclesSummaryStructureRigidLengthListPosition[outBoundDocVar.Count];
                               recordcount = 0;
                               foreach (var outbound in outBoundDocVar)
                               {

                                   NotificationXSD.VehiclesSummaryStructureRigidLengthListPosition vssrllp = new NotificationXSD.VehiclesSummaryStructureRigidLengthListPosition();
                                   vssrllp.RigidLength = NullableOrNotForDecimal(outbound.RigidLength);

                                   if (vssrllp.RigidLength != null)
                                   {
                                       vssrllp.RigidLengthSpecified = true;
                                   }
                                   else
                                   {
                                       vssrllp.RigidLengthSpecified = false;
                                   }

                                   vssrllpList[recordcount] = vssrllp;
                                   recordcount++;
                               }
                               vss.RigidLengthListPosition = vssrllpList;
                               #endregion

                               #region RearOverhangListPosition
                               NotificationXSD.VehiclesSummaryStructureRearOverhangListPosition[] vssrolpList = new NotificationXSD.VehiclesSummaryStructureRearOverhangListPosition[outBoundDocVar.Count];
                               recordcount = 0;
                               foreach (var outbound in outBoundDocVar)
                               {
                                   NotificationXSD.VehiclesSummaryStructureRearOverhangListPosition vssrolp = new NotificationXSD.VehiclesSummaryStructureRearOverhangListPosition();
                                   vssrolp.RearOverhang = NullableOrNotForDecimal(outbound.RearOverhang);
                                   if (vssrolp.RearOverhang != null)
                                   {
                                       vssrolp.RearOverhangSpecified = true;
                                   }
                                   else
                                   {
                                       vssrolp.RearOverhangSpecified = false;
                                   }

                                   vssrolpList[recordcount] = vssrolp;
                                   recordcount++;
                               }
                               vss.RearOverhangListPosition = vssrolpList;
                               #endregion

                               #region FrontOverhangListPosition
                               NotificationXSD.VehiclesSummaryStructureFrontOverhangListPosition[] vssfolpList = new NotificationXSD.VehiclesSummaryStructureFrontOverhangListPosition[outBoundDocVar.Count];
                               recordcount = 0;
                               foreach (var outbound in outBoundDocVar)
                               {
                                   NotificationXSD.VehiclesSummaryStructureFrontOverhangListPosition vssfolp = new NotificationXSD.VehiclesSummaryStructureFrontOverhangListPosition();
                                   NotificationXSD.FrontOverhangStructure frontoverhangstru = new NotificationXSD.FrontOverhangStructure();
                                   frontoverhangstru.InfrontOfCab = false;
                                   frontoverhangstru.Value = NullableOrNotForDecimal(outbound.FrontOverhang);
                                   vssfolp.FrontOverhang = frontoverhangstru;
                                   if (frontoverhangstru.Value != null)
                                   {
                                       vssfolp.FrontOverhangSpecified = true;
                                   }
                                   else
                                   {
                                       vssfolp.FrontOverhangSpecified = false;
                                   }
                                   vssfolpList[recordcount] = vssfolp;
                                   recordcount++;
                               }
                               vss.FrontOverhangListPosition = vssfolpList;
                               #endregion

                               #region LeftOverhangListPosition
                               NotificationXSD.VehiclesSummaryStructureLeftOverhangListPosition[] vsslolpList = new NotificationXSD.VehiclesSummaryStructureLeftOverhangListPosition[outBoundDocVar.Count];
                               recordcount = 0;
                               foreach (var outbound in outBoundDocVar)
                               {
                                   NotificationXSD.VehiclesSummaryStructureLeftOverhangListPosition vsslolp = new NotificationXSD.VehiclesSummaryStructureLeftOverhangListPosition();
                                   NotificationXSD.LeftOverhangStructure leftoverhangstru = new NotificationXSD.LeftOverhangStructure();
                                   leftoverhangstru.InLeftOfCab = false;
                                   leftoverhangstru.Value = NullableOrNotForDecimal(outbound.LeftOverhang);
                                   vsslolp.LeftOverhang = leftoverhangstru;
                                   if (leftoverhangstru.Value != null)
                                   {
                                       vsslolp.LeftOverhangSpecified = true;
                                   }
                                   else
                                   {
                                       vsslolp.LeftOverhangSpecified = false;
                                   }
                                   vsslolpList[recordcount] = vsslolp;
                                   recordcount++;
                               }
                               vss.LeftOverhangListPosition = vsslolpList;
                               #endregion

                               #region RightOverhangListPosition
                               NotificationXSD.VehiclesSummaryStructureRightOverhangListPosition[] vssriolpList = new NotificationXSD.VehiclesSummaryStructureRightOverhangListPosition[outBoundDocVar.Count];
                               recordcount = 0;
                               foreach (var outbound in outBoundDocVar)
                               {
                                   NotificationXSD.VehiclesSummaryStructureRightOverhangListPosition vssrolp = new NotificationXSD.VehiclesSummaryStructureRightOverhangListPosition();
                                   NotificationXSD.RightOverhangStructure rightoverhangstru = new NotificationXSD.RightOverhangStructure();
                                   rightoverhangstru.InRightOfCab = false;
                                   rightoverhangstru.Value = NullableOrNotForDecimal(outbound.RightOverhang);
                                   vssrolp.RightOverhang = rightoverhangstru;
                                   if (rightoverhangstru.Value != null)
                                   {
                                       vssrolp.RightOverhangSpecified = true;
                                   }
                                   else
                                   {
                                       vssrolp.RightOverhangSpecified = false;
                                   }
                                   vssriolpList[recordcount] = vssrolp;
                                   recordcount++;
                               }
                               vss.RightOverhangListPosition = vssriolpList;
                               #endregion

                               #region OverallWidthListPosition
                               NotificationXSD.VehiclesSummaryStructureOverallWidthListPosition[] vssowlpList = new NotificationXSD.VehiclesSummaryStructureOverallWidthListPosition[outBoundDocVar.Count];
                               recordcount = 0;
                               foreach (var outbound in outBoundDocVar)
                               {

                                   NotificationXSD.VehiclesSummaryStructureOverallWidthListPosition vssowlp = new NotificationXSD.VehiclesSummaryStructureOverallWidthListPosition();
                                   vssowlp.OverallWidth = NullableOrNotForDecimal(outbound.Width);
                                   if (vssowlp.OverallWidth != null)
                                   {
                                       vssowlp.OverallWidthSpecified = true;
                                   }
                                   else
                                   {
                                       vssowlp.OverallWidthSpecified = false;
                                   }
                                   vssowlpList[recordcount] = vssowlp;
                                   recordcount++;
                               }
                               vss.OverallWidthListPosition = vssowlpList;
                               #endregion

                               #region OverallHeightListPosition
                               NotificationXSD.VehiclesSummaryStructureOverallHeightListPosition[] vssohlpList = new NotificationXSD.VehiclesSummaryStructureOverallHeightListPosition[outBoundDocVar.Count];
                               recordcount = 0;
                               foreach (var outbound in outBoundDocVar)
                               {
                                   NotificationXSD.VehiclesSummaryStructureOverallHeightListPosition vssohlp = new NotificationXSD.VehiclesSummaryStructureOverallHeightListPosition();
                                   NotificationXSD.SummaryHeightStructure shs = new NotificationXSD.SummaryHeightStructure();
                                   shs.MaxHeight = Convert.ToDecimal(outbound.MaximumHeight);
                                   //Removed old logic for RM 4581 -> 21-09-2023 - RM #28175 -The Reducible Height field shows the value entered for max height. It should show the reducible height value.
                                   shs.ReducibleHeight = NullableOrNotForDecimal(outbound.RedHeight);
                                   if (shs.ReducibleHeight != null)
                                   {
                                       shs.ReducibleHeightSpecified = true;
                                   }
                                   else
                                   {
                                       shs.ReducibleHeightSpecified = false;
                                   }
                                   vssohlp.OverallHeight = shs;
                                   vssohlpList[recordcount] = vssohlp;
                                   recordcount++;
                               }
                               vss.OverallHeightListPosition = vssohlpList;
                               #endregion

                               #region GrossTrainWeightListPosition
                               NotificationXSD.VehiclesSummaryStructureGrossTrainWeightListPosition[] vssgtlpList = new NotificationXSD.VehiclesSummaryStructureGrossTrainWeightListPosition[outBoundDocVar.Count];
                               recordcount = 0;
                               foreach (var outbound in outBoundDocVar)
                               {

                                   NotificationXSD.VehiclesSummaryStructureGrossTrainWeightListPosition vssgtlp = new NotificationXSD.VehiclesSummaryStructureGrossTrainWeightListPosition();
                                   vssgtlp.GrossTrainWeight = NullableOrNotForDecimal(outbound.GrossTrainWeight);
                                   if (vssgtlp.GrossTrainWeight != null)
                                   {
                                       vssgtlp.GrossTrainWeightSpecified = true;
                                   }
                                   else
                                   {
                                       vssgtlp.GrossTrainWeightSpecified = false;
                                   }
                                   vssgtlpList[recordcount] = vssgtlp;
                                   recordcount++;
                               }
                               vss.GrossTrainWeightListPosition = vssgtlpList;
                               #endregion

                               #region GrossWeightListPosition
                               NotificationXSD.VehiclesSummaryStructureGrossWeightListPosition[] vssgwlpList = new NotificationXSD.VehiclesSummaryStructureGrossWeightListPosition[outBoundDocVar.Count];
                               recordcount = 0;
                               foreach (var outbound in outBoundDocVar)
                               {
                                   NotificationXSD.VehiclesSummaryStructureGrossWeightListPosition vssgwlp = new NotificationXSD.VehiclesSummaryStructureGrossWeightListPosition();
                                   NotificationXSD.GrossWeightStructure gws = new NotificationXSD.GrossWeightStructure();
                                   gws.ExcludesTractors = false;
                                   gws.Item = Convert.ToString(outbound.GrossWeight);

                                   vssgwlp.GrossWeight = gws;
                                   vssgwlpList[recordcount] = vssgwlp;
                                   recordcount++;
                               }
                               #endregion

                               #region MaxAxleWeightListPosition
                               vss.GrossWeightListPosition = vssgwlpList;
                               NotificationXSD.VehiclesSummaryStructureMaxAxleWeightListPosition[] vssmawlpList = new NotificationXSD.VehiclesSummaryStructureMaxAxleWeightListPosition[outBoundDocVar.Count];
                               recordcount = 0;
                               foreach (var outbound in outBoundDocVar)
                               {
                                   NotificationXSD.VehiclesSummaryStructureMaxAxleWeightListPosition vssmawlp = new NotificationXSD.VehiclesSummaryStructureMaxAxleWeightListPosition();
                                   NotificationXSD.SummaryMaxAxleWeightStructure smaws = new NotificationXSD.SummaryMaxAxleWeightStructure();
                                   smaws.ItemElementName = NotificationXSD.ItemChoiceType2.Weight;
                                   smaws.Item = Convert.ToString(outbound.MaximumAxleWeight);
                                   vssmawlp.MaximumAxleWeight = smaws;
                                   vssmawlpList[recordcount] = vssmawlp;
                                   recordcount++;
                               }
                               vss.MaximumAxleWeightListPosition = vssmawlpList;
                               #endregion

                               #region ConfigurationIdentityListPosition

                               NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition[] vssvslpList = new NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition[outBoundDocVar.Count];

                               recordcount = 0;
                               int totaAlternativeId = 1;

                               foreach (var outbound in outBoundDocVar)
                               {

                                   List<VehicleConfigList> vehicleConfigurationList = GetVehicleConfigurationDetails(Convert.ToInt32(outbound.VehicleId), UserSchema.Portal);

                                   // Business logic is written for semi vehicle
                                   if (outbound.VehicleType.ToLower() == "semi trailer(3-8) vehicle" ||
                                   outbound.VehicleType.ToLower() == "boat mast exception" || outbound.VehicleType.ToLower() == "semi vehicle" || outbound.VehicleType.ToLower() == "conventional tractor"
                                       || outbound.VehicleType.ToLower() == "crane" || outbound.VehicleType.ToLower() == "mobile crane")
                                   {
                                       NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition vssvslp = new NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition();
                                       NotificationXSD.VehicleSummaryStructure vssList = new NotificationXSD.VehicleSummaryStructure();

                                       NotificationXSD.VehicleSummaryTypeChoiceStructure vstcs = new NotificationXSD.VehicleSummaryTypeChoiceStructure();
                                       NotificationXSD.SemiTrailerSummaryStructure stss = new NotificationXSD.SemiTrailerSummaryStructure();

                                       if (outbound.ComponentType != string.Empty)
                                       {
                                           stss.TractorSubType = CommonMethods.GetVehicleComponentSubTypes(outbound.ComponentType);
                                       }

                                       if (outbound.ComponentSubtype != string.Empty)
                                       {
                                           stss.TrailerSubType = CommonMethods.GetVehicleComponentSubTypes(outbound.ComponentSubtype);
                                       }

                                       stss.Summary = outbound.VehicleDesc;

                                       NotificationXSD.SummaryWeightStructure sws = new NotificationXSD.SummaryWeightStructure();
                                       sws.Item = Convert.ToString(outbound.GrossWeight);
                                       stss.GrossWeight = sws;

                                       stss.IsSteerableAtRear = outbound.IsSteerableAtRear == 1 ? true : false;
                                       stss.IsSteerableAtRearSpecified = true;

                                       NotificationXSD.SummaryMaxAxleWeightStructure smawsConfig = new NotificationXSD.SummaryMaxAxleWeightStructure();
                                       smawsConfig.ItemElementName = NotificationXSD.ItemChoiceType2.Weight;
                                       smawsConfig.Item = Convert.ToString(outbound.MaximumAxleWeight);
                                       stss.MaximumAxleWeight = smawsConfig;

                                       NotificationXSD.SummaryAxleStructure sas = new NotificationXSD.SummaryAxleStructure();

                                           List<VehComponentAxles> vehicleComponentAxlesList = GetVehicleComponentAxles(NotificationID, outbound.VehicleId);

                                           sas.NumberOfAxles = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.AxleCount));

                                           sas.NumberOfWheel = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.WheelCount));

                                           #region AxleWeightListPosition
                                           sas.AxleWeightListPosition = CommonMethods.GetAxleWeightListPosition(vehicleComponentAxlesList);
                                           #endregion

                                           #region WheelsPerAxleListPosition
                                           sas.WheelsPerAxleListPosition = CommonMethods.GetWheelsPerAxleListPosition(vehicleComponentAxlesList);
                                           #endregion

                                           #region AxleSpacingListPosition
                                           sas.AxleSpacingListPosition = CommonMethods.GetAxleSpacingListPositionAxleSpacing(vehicleComponentAxlesList);
                                           #endregion

                                           //Added RM#4386
                                           #region AxleSpacingToFollow
                                           sas.AxleSpacingToFollowing = CommonMethods.GetAxleSpacingToFollowListPositionAxleSpacing(vehicleComponentAxlesList, vehicleConfigurationList[0].SpaceToFollowing);
                                           #endregion

                                           #region TyreSizeListPosition
                                           sas.TyreSizeListPosition = CommonMethods.GetTyreSizeListPosition(vehicleComponentAxlesList);
                                           #endregion

                                           #region WheelSpacingListPosition
                                           sas.WheelSpacingListPosition = CommonMethods.GetWheelSpacingListPosition(vehicleComponentAxlesList);
                                           #endregion

                                           stss.AxleConfiguration = sas;

                                       stss.RigidLength = (decimal)outbound.RigidLength;
                                       stss.Width = (decimal)outbound.Width;
                                       NotificationXSD.SummaryHeightStructure shsVehi = new NotificationXSD.SummaryHeightStructure();
                                       shsVehi.MaxHeight = (decimal)outbound.MaximumHeight;

                                       shsVehi.ReducibleHeight = (decimal?)outbound.RedHeight;
                                       if (shsVehi.ReducibleHeight != null && shsVehi.ReducibleHeight > 0)
                                       {
                                           shsVehi.ReducibleHeightSpecified = true;
                                       }
                                       else
                                       {
                                           shsVehi.ReducibleHeightSpecified = false;
                                       }

                                       stss.Height = shsVehi;
                                       stss.Wheelbase = (decimal?)outbound.WheelBase;
                                       if (stss.Wheelbase != null && stss.Wheelbase > 0)
                                       {
                                           stss.WheelbaseSpecified = true;
                                       }
                                       else
                                       {
                                           stss.WheelbaseSpecified = false;
                                       }

                                       stss.GroundClearance = (decimal?)outbound.GroundClearance;
                                       if (stss.GroundClearance != null && stss.GroundClearance > 0)
                                       {
                                           stss.GroundClearanceSpecified = true;
                                       }
                                       else
                                       {
                                           stss.GroundClearanceSpecified = false;
                                       }

                                       stss.OutsideTrack = (decimal?)outbound.OutsideTrack;
                                       if (stss.OutsideTrack != null && stss.OutsideTrack > 0)
                                       {
                                           stss.OutsideTrackSpecified = true;
                                       }
                                       else
                                       {
                                           stss.OutsideTrackSpecified = false;
                                       }

                                       // new code added
                                       stss.RearOverhang = (decimal?)outbound.RearOverhang;

                                       stss.LeftOverhang = (decimal?)outbound.LeftOverhang;
                                       if (stss.LeftOverhang != null && stss.LeftOverhang > 0)
                                       {
                                           stss.LeftOverhangSpecified = true;
                                       }
                                       else
                                       {
                                           stss.LeftOverhangSpecified = false;
                                       }

                                       stss.RightOverhang = (decimal?)outbound.RightOverhang;
                                       if (stss.RightOverhang != null && stss.RightOverhang > 0)
                                       {
                                           stss.RightOverhangSpecified = true;
                                       }
                                       else
                                       {
                                           stss.RightOverhangSpecified = false;
                                       }

                                       stss.FrontOverhang = (decimal?)outbound.FrontOverhang;
                                       if (stss.RightOverhang != null && stss.RightOverhang > 0)
                                       {
                                           stss.FrontOverhangSpecified = true;
                                       }
                                       else
                                       {
                                           stss.FrontOverhangSpecified = false;
                                       }
                                       //new code ends here

                                       vstcs.Item = stss;

                                       vssList.Configuration = vstcs;
                                       if (outbound.VehicleType == "drawbar vehicle")
                                       {
                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.drawbarvehicle;
                                       }
                                       if (outbound.VehicleType == "semi vehicle"|| outbound.VehicleType == "semi trailer(3-8) vehicle")
                                       {
                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.semivehicle;
                                       }
                                       if (outbound.VehicleType == "rigid vehicle")
                                       {
                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.rigidvehicle;
                                       }
                                       if (outbound.VehicleType == "tracked vehicle")
                                       {
                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.trackedvehicle;
                                       }
                                       if (outbound.VehicleType == "other in line")
                                       {
                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.otherinline;
                                       }
                                       if (outbound.VehicleType == "other side by side")
                                       {
                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.othersidebyside;
                                       }
                                       //Added - Jan272014 
                                       if (outbound.VehicleType == "spmt")
                                       {
                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.spmt;
                                       }
                                       if (outbound.VehicleType == "crane" || outbound.VehicleType == "mobile crane")
                                       {
                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.crane;
                                       }
                                       if (outbound.VehicleType == "Recovery Vehicle")
                                       {
                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.recoveryvehicle;
                                       }
                                       //Added - Jan272014 - end
                                       vssList.ConfigurationIdentityListPosition = GetVehicleSummaryConfigurationIdentity(NotificationID, outbound.VehicleId);

                                       vssList.AlternativeId = Convert.ToString(totaAlternativeId);

                                       double grossWeight = Convert.ToDouble(outbound.GrossWeight);

                                       if (grossWeight >= 80000 && grossWeight <= 150000)
                                       {
                                           vssList.WeightConformance = NotificationXSD.SummaryWeightConformanceType.heavystgo;
                                       }
                                       else
                                       {
                                           vssList.WeightConformance = NotificationXSD.SummaryWeightConformanceType.other;
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
                                       || outbound.VehicleType.ToLower() == "other in line" || outbound.VehicleType.ToLower()=="recovery vehicle" || outbound.VehicleType.ToLower() =="rigid and drag")
                                   // Facing issues with "other in line" as this contains both Semi and Non Semi Vehicles
                                   {
                                       #region For Other Inline or other vehicle
                                       // Business logic is written for Non Semi Vehicle
                                       NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition vssvslp = new NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition();
                                       NotificationXSD.VehicleSummaryStructure vssList = new NotificationXSD.VehicleSummaryStructure();

                                       NotificationXSD.VehicleSummaryTypeChoiceStructure vstcs = new NotificationXSD.VehicleSummaryTypeChoiceStructure();
                                       NotificationXSD.NonSemiTrailerSummaryStructure stss = new NotificationXSD.NonSemiTrailerSummaryStructure();

                                       NotificationXSD.NonSemiTrailerSummaryStructureComponentListPosition[] nsclp = new NotificationXSD.NonSemiTrailerSummaryStructureComponentListPosition[vehicleConfigurationList.Count];

                                       int newRecordcount = 0;
                                       int longitudeIncrement = 1;
                                       int SpecialCaseIncrement = 0;
                                       int SpecialCaseFlag = 0;

                                       vssList.ConfigurationIdentityListPosition = GetVehicleSummaryConfigurationIdentity(NotificationID, outbound.VehicleId);

                                       foreach (var vehicleList in vehicleConfigurationList)
                                       {
                                           NotificationXSD.NonSemiTrailerSummaryStructureComponentListPosition nsclpObject = new NotificationXSD.NonSemiTrailerSummaryStructureComponentListPosition();

                                           NotificationXSD.ComponentSummaryStructure css = new NotificationXSD.ComponentSummaryStructure();

                                           css.Longitude = Convert.ToString(longitudeIncrement);

                                           if (vehicleList.ComponentType != string.Empty)
                                           {
                                               css.ComponentType = CommonMethods.GetVehicleComponentSubTypesNonSemiVehicles(vehicleList.ComponentType);
                                           }

                                           if (vehicleList.ComponentSubType != string.Empty)
                                           {
                                               css.ComponentSubType = CommonMethods.GetVehicleComponentSubTypes(vehicleList.ComponentSubType);
                                           }

                                           nsclpObject.Component = css;

                                           #region Non Semi Vehicle configuration

                                           List<string> allowedComponentTypeList = new List<string>();
                                           allowedComponentTypeList.Add("rigid vehicle");
                                           allowedComponentTypeList.Add("ballast tractor");
                                           allowedComponentTypeList.Add("drawbar trailer");
                                           allowedComponentTypeList.Add("spmt");

                                           if (outbound.VehicleType.ToLower() == "other in line" && vehicleConfigurationList.Count >= 2 && !vehicleConfigurationList.All(x => allowedComponentTypeList.Contains(x.ComponentType.ToLower())) && (vehicleConfigurationList[0].ComponentType.Trim().ToLower() == "rigid vehicle" || vehicleConfigurationList[0].ComponentType.Trim().ToLower() == "ballast tractor" || vehicleConfigurationList[0].ComponentType.Trim().ToLower() == "drawbar trailer" || vehicleConfigurationList[0].ComponentType.Trim().ToLower() == "spmt")) // Special condition when rigidvehicle + semitrailer
                                           {
                                               if (SpecialCaseIncrement >= 1)
                                               {
                                                   break;
                                               }
                                               #region For Special case when nonsemi is at top and semi-trailer is at bottom

                                               NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition vssvslp2 = new NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition();

                                               NotificationXSD.SemiTrailerSummaryStructure dtss = new NotificationXSD.SemiTrailerSummaryStructure();

                                               dtss.Summary = outbound.VehicleDesc;

                                               NotificationXSD.SummaryWeightStructure sws = new NotificationXSD.SummaryWeightStructure();
                                               sws.Item = Convert.ToString(outbound.GrossWeight);
                                               dtss.GrossWeight = sws;

                                               dtss.IsSteerableAtRear = outbound.IsSteerableAtRear == 1 ? true : false;
                                               dtss.IsSteerableAtRearSpecified = true;

                                               NotificationXSD.SummaryMaxAxleWeightStructure smawsConfig = new NotificationXSD.SummaryMaxAxleWeightStructure();
                                               smawsConfig.ItemElementName = NotificationXSD.ItemChoiceType2.Weight;
                                               smawsConfig.Item = Convert.ToString(outbound.MaximumAxleWeight);
                                               dtss.MaximumAxleWeight = smawsConfig;

                                               NotificationXSD.SummaryAxleStructure sas = new NotificationXSD.SummaryAxleStructure();

                                                   List<VehComponentAxles> vehicleComponentAxlesList = GetVehicleComponentAxles(NotificationID, outbound.VehicleId);

                                                   sas.NumberOfAxles = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.AxleCount));

                                                   sas.NumberOfWheel = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.WheelCount));

                                                   #region AxleWeightListPosition
                                                   sas.AxleWeightListPosition = CommonMethods.GetAxleWeightListPosition(vehicleComponentAxlesList);
                                                   #endregion

                                                   #region WheelsPerAxleListPosition
                                                   sas.WheelsPerAxleListPosition = CommonMethods.GetWheelsPerAxleListPosition(vehicleComponentAxlesList);
                                                   #endregion

                                                   #region AxleSpacingListPosition
                                                   sas.AxleSpacingListPosition = CommonMethods.GetAxleSpacingListPositionAxleSpacing(vehicleComponentAxlesList);
                                                   #endregion

                                                   //Added RM#4386
                                                   #region AxleSpacingToFollow
                                                   sas.AxleSpacingToFollowing = CommonMethods.GetAxleSpacingToFollowListPositionAxleSpacing(vehicleComponentAxlesList, vehicleConfigurationList[0].SpaceToFollowing);
                                                   #endregion

                                                   #region TyreSizeListPosition
                                                   sas.TyreSizeListPosition = CommonMethods.GetTyreSizeListPosition(vehicleComponentAxlesList);
                                                   #endregion

                                                   #region WheelSpacingListPosition
                                                   sas.WheelSpacingListPosition = CommonMethods.GetWheelSpacingListPosition(vehicleComponentAxlesList);
                                                   #endregion

                                                   dtss.AxleConfiguration = sas;

                                               dtss.RigidLength = (decimal)outbound.RigidLength;
                                               dtss.Width = (decimal)outbound.Width;

                                               NotificationXSD.SummaryHeightStructure shsVehi = new NotificationXSD.SummaryHeightStructure();
                                               shsVehi.MaxHeight = (decimal)outbound.MaximumHeight;

                                               shsVehi.ReducibleHeight = (decimal)outbound.RedHeight;
                                               if (shsVehi.ReducibleHeight != null && shsVehi.ReducibleHeight > 0)
                                               {
                                                   shsVehi.ReducibleHeightSpecified = true;
                                               }
                                               else
                                               {
                                                   shsVehi.ReducibleHeightSpecified = false;
                                               }

                                               dtss.Height = shsVehi;
                                               dtss.Wheelbase = (decimal?)outbound.WheelBase;
                                               if (dtss.Wheelbase != null && dtss.Wheelbase > 0)
                                               {
                                                   dtss.WheelbaseSpecified = true;
                                               }
                                               else
                                               {
                                                   dtss.WheelbaseSpecified = false;
                                               }

                                               dtss.GroundClearance = (decimal?)outbound.GroundClearance;
                                               if (dtss.GroundClearance != null && dtss.GroundClearance > 0)
                                               {
                                                   dtss.GroundClearanceSpecified = true;
                                               }
                                               else
                                               {
                                                   dtss.GroundClearanceSpecified = false;
                                               }

                                               dtss.OutsideTrack = (decimal?)outbound.OutsideTrack;
                                               if (dtss.OutsideTrack != null && dtss.OutsideTrack > 0)
                                               {
                                                   dtss.OutsideTrackSpecified = true;
                                               }
                                               else
                                               {
                                                   dtss.OutsideTrackSpecified = false;
                                               }

                                               // new code added

                                               dtss.RearOverhang = (decimal?)outbound.RearOverhang;

                                               dtss.LeftOverhang = (decimal?)outbound.LeftOverhang;
                                               if (dtss.LeftOverhang != null && dtss.LeftOverhang > 0)
                                               {
                                                   dtss.LeftOverhangSpecified = true;
                                               }
                                               else
                                               {
                                                   dtss.LeftOverhangSpecified = false;
                                               }

                                               dtss.RightOverhang = (decimal?)outbound.RightOverhang;
                                               if (dtss.RightOverhang != null && dtss.RightOverhang > 0)
                                               {
                                                   dtss.RightOverhangSpecified = true;
                                               }
                                               else
                                               {
                                                   dtss.RightOverhangSpecified = false;
                                               }

                                               dtss.FrontOverhang = (decimal?)outbound.FrontOverhang;
                                               if (dtss.FrontOverhang != null && dtss.FrontOverhang > 0)
                                               {
                                                   dtss.FrontOverhangSpecified = true;
                                               }
                                               else
                                               {
                                                   dtss.FrontOverhangSpecified = false;
                                               }

                                               //new code ends here

                                               if (outbound.VehicleType == "drawbar vehicle")
                                               {
                                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.drawbarvehicle;
                                               }
                                               if (outbound.VehicleType == "semi vehicle" || outbound.VehicleType == "semi trailer(3-8) vehicle")
                                               {
                                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.semivehicle;
                                               }
                                               if (outbound.VehicleType == "rigid vehicle")
                                               {
                                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.rigidvehicle;
                                               }
                                               if (outbound.VehicleType == "tracked vehicle")
                                               {
                                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.trackedvehicle;
                                               }
                                               if (outbound.VehicleType == "other in line")
                                               {
                                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.otherinline;
                                               }
                                               if (outbound.VehicleType == "other side by side")
                                               {
                                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.othersidebyside;
                                               }
                                               //Added - Jan272014 
                                               if (outbound.VehicleType == "spmt")
                                               {
                                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.spmt;
                                               }
                                               if (outbound.VehicleType == "Recovery Vehicle")
                                               {
                                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.recoveryvehicle;
                                               }

                                               vstcs.Item = dtss;
                                               vssList.Configuration = vstcs;
                                               vssvslp.VehicleSummary = vssList;

                                               if (outBoundDocVar[0].VehicleType.ToLower() == "semi vehicle" || outBoundDocVar[0].VehicleType.ToLower() == "conventional tractor")
                                               {
                                                   vssvslpList[recordcount] = vssvslp;
                                                   recordcount++;
                                               }
                                               else
                                               {
                                                   vssvslpList[newRecordcount] = vssvslp;
                                                   newRecordcount++;
                                               }

                                               SpecialCaseFlag++;
                                               SpecialCaseIncrement++;
                                               #endregion
                                           }
                                           else
                                           {

                                               if (nsclpObject.Component.ComponentType == NotificationXSD.VehicleComponentType.ballasttractor)
                                               {
                                                   #region For ballasttractor
                                                   NotificationXSD.DrawbarTractorSummaryStructure dtss = new NotificationXSD.DrawbarTractorSummaryStructure();

                                                   dtss.Summary = vehicleList.VehicleDescription;

                                                   dtss.Weight = Convert.ToString(vehicleList.GrossWeight);

                                                   NotificationXSD.SummaryMaxAxleWeightStructure smawsConfig = new NotificationXSD.SummaryMaxAxleWeightStructure();
                                                   smawsConfig.ItemElementName = NotificationXSD.ItemChoiceType2.Weight;
                                                   smawsConfig.Item = Convert.ToString(outbound.MaximumAxleWeight);

                                                   dtss.MaxAxleWeight = smawsConfig;

                                                   NotificationXSD.SummaryAxleStructure sas = new NotificationXSD.SummaryAxleStructure();

                                                       List<VehComponentAxles> vehicleComponentAxlesList = GetVehicleComponentAxlesByComponent(vehicleList.ComponentId, outbound.VehicleId, UserSchema.Portal);

                                                       sas.NumberOfAxles = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.AxleCount));

                                                       sas.NumberOfWheel = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.WheelCount));

                                                       #region AxleWeightListPosition
                                                       sas.AxleWeightListPosition = CommonMethods.GetAxleWeightListPosition(vehicleComponentAxlesList);
                                                       #endregion

                                                       #region WheelsPerAxleListPosition
                                                       sas.WheelsPerAxleListPosition = CommonMethods.GetWheelsPerAxleListPosition(vehicleComponentAxlesList);
                                                       #endregion

                                                       #region AxleSpacingListPosition
                                                       sas.AxleSpacingListPosition = CommonMethods.GetAxleSpacingListPosition(vehicleComponentAxlesList);
                                                       #endregion

                                                       //Added RM#4386
                                                       #region AxleSpacingToFollow
                                                       sas.AxleSpacingToFollowing = CommonMethods.GetAxleSpacingToFollowListPositionAxleSpacing(vehicleComponentAxlesList, vehicleConfigurationList[0].SpaceToFollowing);
                                                       #endregion

                                                       #region TyreSizeListPosition
                                                       sas.TyreSizeListPosition = CommonMethods.GetTyreSizeListPosition(vehicleComponentAxlesList);
                                                       #endregion

                                                       #region WheelSpacingListPosition
                                                       sas.WheelSpacingListPosition = CommonMethods.GetWheelSpacingListPosition(vehicleComponentAxlesList);
                                                       #endregion

                                                       dtss.AxleConfiguration = sas;

                                                   dtss.Length = (decimal?)outbound.RigidLength;
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

                                                   NotificationXSD.SummaryHeightStructure shsVehi = new NotificationXSD.SummaryHeightStructure();
                                                   shsVehi.MaxHeight = (decimal)outbound.MaximumHeight;

                                                   shsVehi.ReducibleHeight = (decimal?)vehicleList.RedHeight;
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
                                               if ((outbound.VehicleType.ToLower() == "recovery vehicle" && (nsclpObject.Component.ComponentType == NotificationXSD.VehicleComponentType.semitrailer || nsclpObject.Component.ComponentType == NotificationXSD.VehicleComponentType.conventionaltractor)))
                                               {
                                                   #region For ballasttractor
                                                   NotificationXSD.DrawbarTractorSummaryStructure dtss = new NotificationXSD.DrawbarTractorSummaryStructure();

                                                   dtss.Summary = vehicleList.VehicleDescription;

                                                   dtss.Weight = Convert.ToString(vehicleList.GrossWeight);

                                                   NotificationXSD.SummaryMaxAxleWeightStructure smawsConfig = new NotificationXSD.SummaryMaxAxleWeightStructure();
                                                   smawsConfig.ItemElementName = NotificationXSD.ItemChoiceType2.Weight;
                                                   smawsConfig.Item = Convert.ToString(outbound.MaximumAxleWeight);

                                                   dtss.MaxAxleWeight = smawsConfig;

                                                   NotificationXSD.SummaryAxleStructure sas = new NotificationXSD.SummaryAxleStructure();

                                                   List<VehComponentAxles> vehicleComponentAxlesList = GetVehicleComponentAxlesByComponent(vehicleList.ComponentId, outbound.VehicleId, UserSchema.Portal);

                                                   sas.NumberOfAxles = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.AxleCount));

                                                   sas.NumberOfWheel = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.WheelCount));

                                                   #region AxleWeightListPosition
                                                   sas.AxleWeightListPosition = CommonMethods.GetAxleWeightListPosition(vehicleComponentAxlesList);
                                                   #endregion

                                                   #region WheelsPerAxleListPosition
                                                   sas.WheelsPerAxleListPosition = CommonMethods.GetWheelsPerAxleListPosition(vehicleComponentAxlesList);
                                                   #endregion

                                                   #region AxleSpacingListPosition
                                                   sas.AxleSpacingListPosition = CommonMethods.GetAxleSpacingListPosition(vehicleComponentAxlesList);
                                                   #endregion

                                                   //Added RM#4386
                                                   #region AxleSpacingToFollow
                                                   sas.AxleSpacingToFollowing = CommonMethods.GetAxleSpacingToFollowListPositionAxleSpacing(vehicleComponentAxlesList, vehicleConfigurationList[0].SpaceToFollowing);
                                                   #endregion

                                                   #region TyreSizeListPosition
                                                   sas.TyreSizeListPosition = CommonMethods.GetTyreSizeListPosition(vehicleComponentAxlesList);
                                                   #endregion

                                                   #region WheelSpacingListPosition
                                                   sas.WheelSpacingListPosition = CommonMethods.GetWheelSpacingListPosition(vehicleComponentAxlesList);
                                                   #endregion

                                                   dtss.AxleConfiguration = sas;

                                                   dtss.Length = (decimal?)outbound.RigidLength;
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

                                                   NotificationXSD.SummaryHeightStructure shsVehi = new NotificationXSD.SummaryHeightStructure();
                                                   shsVehi.MaxHeight = (decimal)outbound.MaximumHeight;

                                                   shsVehi.ReducibleHeight = (decimal?)vehicleList.RedHeight;
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

                                               else if (nsclpObject.Component.ComponentType == NotificationXSD.VehicleComponentType.drawbartrailer
                                                   || nsclpObject.Component.ComponentType == NotificationXSD.VehicleComponentType.spmt
                                                   || nsclpObject.Component.ComponentType == NotificationXSD.VehicleComponentType.rigidvehicle)
                                               {
                                                   #region For rigidvehicle or spmt or drawbartrailer
                                                   NotificationXSD.LoadBearingSummaryStructure dtss = new NotificationXSD.LoadBearingSummaryStructure();

                                                   dtss.Summary = vehicleList.VehicleDescription;

                                                   dtss.Weight = Convert.ToString(vehicleList.GrossWeight);

                                                   dtss.IsSteerableAtRear = vehicleList.IsSteerableAtRear == 1;
                                                   dtss.IsSteerableAtRearSpecified = true;

                                                   NotificationXSD.SummaryMaxAxleWeightStructure smawsConfig = new NotificationXSD.SummaryMaxAxleWeightStructure();
                                                   smawsConfig.ItemElementName = NotificationXSD.ItemChoiceType2.Weight;
                                                   smawsConfig.Item = Convert.ToString(outbound.MaximumAxleWeight);
                                                   dtss.MaxAxleWeight = smawsConfig;

                                                   NotificationXSD.SummaryAxleStructure sas = new NotificationXSD.SummaryAxleStructure();

                                                       List<VehComponentAxles> vehicleComponentAxlesList = GetVehicleComponentAxlesByComponent(vehicleList.ComponentId, outbound.VehicleId, UserSchema.Portal);

                                                       sas.NumberOfAxles = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.AxleCount));

                                                       sas.NumberOfWheel = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.WheelCount));

                                                       #region AxleWeightListPosition
                                                       sas.AxleWeightListPosition = CommonMethods.GetAxleWeightListPosition(vehicleComponentAxlesList);
                                                       #endregion

                                                       #region WheelsPerAxleListPosition
                                                       sas.WheelsPerAxleListPosition = CommonMethods.GetWheelsPerAxleListPosition(vehicleComponentAxlesList);
                                                       #endregion

                                                       #region AxleSpacingListPosition
                                                       sas.AxleSpacingListPosition = CommonMethods.GetAxleSpacingListPosition(vehicleComponentAxlesList);
                                                       #endregion

                                                       //Added RM#4386
                                                       #region AxleSpacingToFollow
                                                       sas.AxleSpacingToFollowing = CommonMethods.GetAxleSpacingToFollowListPositionAxleSpacing(vehicleComponentAxlesList, vehicleConfigurationList[0].SpaceToFollowing);
                                                       #endregion

                                                       #region TyreSizeListPosition
                                                       sas.TyreSizeListPosition = CommonMethods.GetTyreSizeListPosition(vehicleComponentAxlesList);
                                                       #endregion

                                                       #region WheelSpacingListPosition
                                                       sas.WheelSpacingListPosition = CommonMethods.GetWheelSpacingListPosition(vehicleComponentAxlesList);
                                                       #endregion

                                                       dtss.AxleConfiguration = sas;

                                                   dtss.RigidLength = (decimal)outbound.RigidLength;
                                                   dtss.Width = (decimal)outbound.Width;

                                                   NotificationXSD.SummaryHeightStructure shsVehi = new NotificationXSD.SummaryHeightStructure();
                                                   shsVehi.MaxHeight = (decimal)outbound.MaximumHeight;

                                                   shsVehi.ReducibleHeight = (decimal?)vehicleList.RedHeight;
                                                   if (shsVehi.ReducibleHeight != null && shsVehi.ReducibleHeight > 0)
                                                   {
                                                       shsVehi.ReducibleHeightSpecified = true;
                                                   }
                                                   else
                                                   {
                                                       shsVehi.ReducibleHeightSpecified = false;
                                                   }


                                                   dtss.Height = shsVehi;
                                                   dtss.Wheelbase = (decimal?)vehicleList.WheelBase;
                                                   if (dtss.Wheelbase != null && dtss.Wheelbase > 0)
                                                   {
                                                       dtss.WheelbaseSpecified = true;
                                                   }
                                                   else
                                                   {
                                                       dtss.WheelbaseSpecified = false;
                                                   }

                                                   dtss.GroundClearance = (decimal?)vehicleList.GroundClearance;
                                                   if (dtss.GroundClearance != null && dtss.GroundClearance > 0)
                                                   {
                                                       dtss.GroundClearanceSpecified = true;
                                                   }
                                                   else
                                                   {
                                                       dtss.GroundClearanceSpecified = false;
                                                   }

                                                   dtss.OutsideTrack = (decimal?)vehicleList.OutsideTrack;
                                                   if (dtss.OutsideTrack != null && dtss.OutsideTrack > 0)
                                                   {
                                                       dtss.OutsideTrackSpecified = true;
                                                   }
                                                   else
                                                   {
                                                       dtss.OutsideTrackSpecified = false;
                                                   }

                                                   // new code added

                                                   dtss.RearOverhang = (decimal?)vehicleList.RearOverhang;

                                                   dtss.LeftOverhang = (decimal?)vehicleList.LeftOverhang;
                                                   if (dtss.LeftOverhang != null && dtss.LeftOverhang > 0)
                                                   {
                                                       dtss.LeftOverhangSpecified = true;
                                                   }
                                                   else
                                                   {
                                                       dtss.LeftOverhangSpecified = false;
                                                   }

                                                   dtss.RightOverhang = (decimal?)vehicleList.RightOverhang;
                                                   if (dtss.RightOverhang != null && dtss.RightOverhang > 0)
                                                   {
                                                       dtss.RightOverhangSpecified = true;
                                                   }
                                                   else
                                                   {
                                                       dtss.RightOverhangSpecified = false;
                                                   }

                                                   dtss.FrontOverhang = (decimal?)vehicleList.FrontOverhang;
                                                   if (dtss.FrontOverhang != null && dtss.FrontOverhang > 0)
                                                   {
                                                       dtss.FrontOverhangSpecified = true;
                                                   }
                                                   else
                                                   {
                                                       dtss.FrontOverhangSpecified = false;
                                                   }

                                                   dtss.AxleSpacingToFollowing = Convert.ToDecimal(vehicleList.SpaceToFollowing);
                                                   if (dtss.AxleSpacingToFollowing != null && dtss.AxleSpacingToFollowing > 0)
                                                   {
                                                       dtss.AxleSpacingToFollowingSpecified = true;
                                                   }
                                                   else
                                                   {
                                                       dtss.AxleSpacingToFollowingSpecified = false;
                                                   }

                                                   dtss.ReducedGroundClearance = (decimal?)vehicleList.RedGroundClearance;
                                                   if (dtss.ReducedGroundClearance != null && dtss.ReducedGroundClearance > 0)
                                                   {
                                                       dtss.ReducedGroundClearanceSpecified = true;
                                                   }
                                                   else
                                                   {
                                                       dtss.ReducedGroundClearanceSpecified = false;
                                                   }
                                                   //new code ends here

                                                   nsclpObject.Component.Item = dtss;

                                                   nsclp[newRecordcount] = nsclpObject;

                                                   newRecordcount++;
                                                   #endregion
                                               }

                                               else if (nsclpObject.Component.ComponentType == NotificationXSD.VehicleComponentType.semitrailer || nsclpObject.Component.ComponentType == NotificationXSD.VehicleComponentType.conventionaltractor)
                                               {
                                                   #region For semitrailer or conventionaltractor

                                                   NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition vssvslp2 = new NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition();

                                                   NotificationXSD.SemiTrailerSummaryStructure dtss = new NotificationXSD.SemiTrailerSummaryStructure();

                                                   dtss.Summary = outbound.VehicleDesc;

                                                   NotificationXSD.SummaryWeightStructure sws = new NotificationXSD.SummaryWeightStructure();
                                                   sws.Item = Convert.ToString(outbound.GrossWeight);
                                                   dtss.GrossWeight = sws;

                                                   dtss.IsSteerableAtRear = outbound.IsSteerableAtRear == 1;
                                                   dtss.IsSteerableAtRearSpecified = true;

                                                   NotificationXSD.SummaryMaxAxleWeightStructure smawsConfig = new NotificationXSD.SummaryMaxAxleWeightStructure();
                                                   smawsConfig.ItemElementName = NotificationXSD.ItemChoiceType2.Weight;
                                                   smawsConfig.Item = Convert.ToString(outbound.MaximumAxleWeight);
                                                   dtss.MaximumAxleWeight = smawsConfig;

                                                   NotificationXSD.SummaryAxleStructure sas = new NotificationXSD.SummaryAxleStructure();

                                                       List<VehComponentAxles> vehicleComponentAxlesList = GetVehicleComponentAxles(NotificationID, outbound.VehicleId);

                                                       sas.NumberOfAxles = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.AxleCount));

                                                       sas.NumberOfWheel = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.WheelCount));

                                                       #region AxleWeightListPosition
                                                       sas.AxleWeightListPosition = CommonMethods.GetAxleWeightListPosition(vehicleComponentAxlesList);
                                                       #endregion

                                                       #region WheelsPerAxleListPosition
                                                       sas.WheelsPerAxleListPosition = CommonMethods.GetWheelsPerAxleListPosition(vehicleComponentAxlesList);
                                                       #endregion

                                                       #region AxleSpacingListPosition
                                                       sas.AxleSpacingListPosition = CommonMethods.GetAxleSpacingListPositionAxleSpacing(vehicleComponentAxlesList);
                                                       #endregion

                                                       //Added RM#4386
                                                       #region AxleSpacingToFollow
                                                       sas.AxleSpacingToFollowing = CommonMethods.GetAxleSpacingToFollowListPositionAxleSpacing(vehicleComponentAxlesList, vehicleConfigurationList[0].SpaceToFollowing);
                                                       #endregion

                                                       #region TyreSizeListPosition
                                                       sas.TyreSizeListPosition = CommonMethods.GetTyreSizeListPosition(vehicleComponentAxlesList);
                                                       #endregion

                                                       #region WheelSpacingListPosition
                                                       sas.WheelSpacingListPosition = CommonMethods.GetWheelSpacingListPosition(vehicleComponentAxlesList);
                                                       #endregion

                                                       dtss.AxleConfiguration = sas;

                                                   dtss.RigidLength = (decimal)outbound.RigidLength;
                                                   dtss.Width = (decimal)outbound.Width;

                                                   NotificationXSD.SummaryHeightStructure shsVehi = new NotificationXSD.SummaryHeightStructure();
                                                   shsVehi.MaxHeight = (decimal)outbound.MaximumHeight;

                                                   shsVehi.ReducibleHeight = (decimal?)outbound.RedHeight;
                                                   if (shsVehi.ReducibleHeight != null && shsVehi.ReducibleHeight > 0)
                                                   {
                                                       shsVehi.ReducibleHeightSpecified = true;
                                                   }
                                                   else
                                                   {
                                                       shsVehi.ReducibleHeightSpecified = false;
                                                   }

                                                   dtss.Height = shsVehi;
                                                   dtss.Wheelbase = (decimal)outbound.WheelBase;
                                                   if (dtss.Wheelbase != null && dtss.Wheelbase > 0)
                                                   {
                                                       dtss.WheelbaseSpecified = true;
                                                   }
                                                   else
                                                   {
                                                       dtss.WheelbaseSpecified = false;
                                                   }

                                                   dtss.GroundClearance = (decimal)outbound.GroundClearance;
                                                   if (dtss.GroundClearance != null && dtss.GroundClearance > 0)
                                                   {
                                                       dtss.GroundClearanceSpecified = true;
                                                   }
                                                   else
                                                   {
                                                       dtss.GroundClearanceSpecified = false;
                                                   }

                                                   dtss.OutsideTrack = (decimal)outbound.OutsideTrack;
                                                   if (dtss.OutsideTrack != null && dtss.OutsideTrack > 0)
                                                   {
                                                       dtss.OutsideTrackSpecified = true;
                                                   }
                                                   else
                                                   {
                                                       dtss.OutsideTrackSpecified = false;
                                                   }

                                                   // new code added

                                                   dtss.RearOverhang = (decimal?)outbound.RearOverhang;

                                                   dtss.LeftOverhang = (decimal?)outbound.LeftOverhang;
                                                   if (dtss.LeftOverhang != null && dtss.LeftOverhang > 0)
                                                   {
                                                       dtss.LeftOverhangSpecified = true;
                                                   }
                                                   else
                                                   {
                                                       dtss.LeftOverhangSpecified = false;
                                                   }

                                                   dtss.RightOverhang = (decimal?)outbound.RightOverhang;
                                                   if (dtss.RightOverhang != null && dtss.RightOverhang > 0)
                                                   {
                                                       dtss.RightOverhangSpecified = true;
                                                   }
                                                   else
                                                   {
                                                       dtss.RightOverhangSpecified = false;
                                                   }

                                                   dtss.FrontOverhang = (decimal?)outbound.FrontOverhang;
                                                   if (dtss.FrontOverhang != null && dtss.FrontOverhang > 0)
                                                   {
                                                       dtss.FrontOverhangSpecified = true;
                                                   }
                                                   else
                                                   {
                                                       dtss.FrontOverhangSpecified = false;
                                                   }

                                                   //new code ends here

                                                   vstcs.Item = dtss;
                                                   vssList.Configuration = vstcs;
                                                   vssvslp.VehicleSummary = vssList;

                                                   newRecordcount++;
                                                   #endregion
                                               }
                                           }

                                           #endregion

                                           longitudeIncrement++;
                                       }

                                       if (SpecialCaseFlag == 0)
                                       {

                                           if (vehicleConfigurationList.Count > 0 && (vehicleConfigurationList[0].ComponentType != "semi trailer" && vehicleConfigurationList[0].ComponentType != "conventional tractor"))
                                           {
                                               stss.ComponentListPosition = nsclp;
                                           }

                                           if (outbound.VehicleType == "drawbar vehicle")
                                           {
                                               vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.drawbarvehicle;
                                           }
                                           if (outbound.VehicleType == "semi vehicle" || outbound.VehicleType == "semi trailer(3-8) vehicle")
                                           {
                                               vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.semivehicle;
                                           }
                                           if (outbound.VehicleType == "rigid vehicle")
                                           {
                                               vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.rigidvehicle;
                                           }
                                           if (outbound.VehicleType == "tracked vehicle")
                                           {
                                               vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.trackedvehicle;
                                           }
                                           if (outbound.VehicleType == "other in line")
                                           {
                                               vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.otherinline;
                                           }
                                           if (outbound.VehicleType == "other side by side")
                                           {
                                               vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.othersidebyside;
                                           }
                                           //Added - Jan272014 
                                           if (outbound.VehicleType == "spmt")
                                           {
                                               vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.spmt;
                                           }
                                           if (outbound.VehicleType == "crane" || outbound.VehicleType == "mobile crane")
                                           {
                                               vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.crane;
                                           }
                                           if (outbound.VehicleType == "Recovery Vehicle")
                                           {
                                               vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.recoveryvehicle;
                                           }
                                           //Added - Jan272014 - end

                                           if (vehicleConfigurationList.Count > 0 && (vehicleConfigurationList[0].ComponentType != "semi trailer" && vehicleConfigurationList[0].ComponentType != "conventional tractor"))
                                           {
                                               vstcs.Item = stss;
                                           }

                                           vssList.Configuration = vstcs;

                                           vssvslp.VehicleSummary = vssList;

                                           vssvslpList[recordcount] = vssvslp;
                                       }
                                       recordcount++;
                                       #endregion
                                   }
                                   else
                                   {
                                       NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition vssvslp = new NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition();
                                       NotificationXSD.VehicleSummaryStructure vssList = new NotificationXSD.VehicleSummaryStructure();

                                       NotificationXSD.VehicleSummaryTypeChoiceStructure vstcs = new NotificationXSD.VehicleSummaryTypeChoiceStructure();

                                       NotificationXSD.TrackedVehicleSummaryStructure dtss = new NotificationXSD.TrackedVehicleSummaryStructure();

                                       NotificationXSD.ComponentSummaryStructure css = new NotificationXSD.ComponentSummaryStructure();

                                       vssList.ConfigurationIdentityListPosition = GetVehicleSummaryConfigurationIdentity(NotificationID, outbound.VehicleId);

                                       dtss.Summary = outbound.VehicleDesc;

                                       NotificationXSD.SummaryWeightStructure sws = new NotificationXSD.SummaryWeightStructure();
                                       sws.Item = Convert.ToString(outbound.GrossWeight);
                                       dtss.GrossWeight = sws;

                                       dtss.RigidLength = (decimal)outbound.RigidLength;

                                       dtss.Width = (decimal)outbound.Width;

                                       if (outbound.VehicleType == "drawbar vehicle")
                                       {
                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.drawbarvehicle;
                                       }
                                       if (outbound.VehicleType == "semi vehicle" || outbound.VehicleType == "semi trailer(3-8) vehicle")
                                       {
                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.semivehicle;
                                       }
                                       if (outbound.VehicleType == "rigid vehicle")
                                       {
                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.rigidvehicle;
                                       }
                                       if (outbound.VehicleType == "tracked vehicle")
                                       {
                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.trackedvehicle;
                                       }
                                       if (outbound.VehicleType == "other in line")
                                       {
                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.otherinline;
                                       }
                                       if (outbound.VehicleType == "other side by side")
                                       {
                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.othersidebyside;
                                       }
                                       //Added - Jan272014 
                                       if (outbound.VehicleType == "spmt")
                                       {
                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.spmt;
                                       }
                                       if (outbound.VehicleType == "crane" || outbound.VehicleType == "mobile crane")
                                       {
                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.crane;
                                       }
                                       if (outbound.VehicleType == "Recovery Vehicle")
                                       {
                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.recoveryvehicle;
                                       }
                                       //Added - Jan272014 - end
                                       vstcs.Item = dtss;

                                       vssList.Configuration = vstcs;

                                       vssvslp.VehicleSummary = vssList;

                                       vssvslpList[recordcount] = vssvslp;

                                       recordcount++;
                                       totaAlternativeId++;
                                   }
                               }

                               vss.VehicleSummaryListPosition = vssvslpList;
                               #endregion

                           }

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

                       }
                       if (outBoundDocVar.Count > 0)
                       {
                           //=============================================================================================================================
                           // Date 11 Feb 2015 Ticket No 3537
                           DrivingInstructionModel DrivingInstructionInfo =new DrivingInstructionModel{ 
                               DrivingInstructions= routeAnalysis.DrivingInstructions
                           };
                           //GetDrivingInstructionStructures(NotificationID);

                           #region "Driving Instructions"
                           int incdoccaution = 0;
                           incdoccaution = records.GetShortOrDefault("include_dock_caution");
                           prp.DrivingInstructions = getDrivingDetails(DrivingInstructionInfo, incdoccaution);
                           #endregion

                           //=============================================================================================================================

                           #region Roads
                           prp.Roads = getRoadDetails(NotificationID, DrivingInstructionInfo);
                           #endregion

                           #region "Structures"
                           AffectedStructuresStructure affStructure = new AffectedStructuresStructure();
                           RoutePartName = records.GetStringOrDefault("PART_NAME");
                           affStructure = GetStructureDataDetails(NotificationID, RoutePartName, routeAnalysis.AffectedStructures);
                           prp.Structures = affStructure;
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

                           string routedescImperial = getRouteDetailsImperial(NotificationID,routeAnalysis.RouteDescription);

                           if (routedescImperial == string.Empty)
                           {
                               routedescImperial = "\u2002";
                           }

                           instance.RouteImperial = routedescImperial;

                           string routedesc = getRouteDetails(NotificationID, routeAnalysis.RouteDescription);

                           if (routedesc == string.Empty)
                           {
                               routedesc = "\u2002";
                           }

                           instance.Route = routedesc;

                           legNumber++;
                       }
                   }
            );
            return rpsrplp;
        }

        public static RouteAnalysisModel GetRouteAssessmentDetails(int notificationID, int organisationId, int isNen)
        {
            RouteAnalysisModel routeAnalysis = new RouteAnalysisModel();
            string spName = ".GET_OUTBOUND_DRIVINGINS_DETAIL";
            if (isNen == 1)
                spName = ".STP_NON_ESDAL_DOCUMENT.SP_GET_NEN_PDF_ANALYSED_ROUTES";
            else if (isNen == 2)
                spName = ".STP_NON_ESDAL_DOCUMENT.SP_GET_NEN_API_ANALYSED_ROUTES";

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                routeAnalysis,
                UserSchema.Portal + spName,
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIFICATION_ID", notificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    if (isNen > 0)
                        parameter.AddWithValue("P_ORGANISATION_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       instance.DrivingInstructions = records.GetByteArrayOrNull("DRIVING_INSTRUCTIONS");
                       instance.AffectedStructures = records.GetByteArrayOrNull("affected_structures");
                       instance.RouteDescription = records.GetByteArrayOrNull("route_description");
                       instance.AffectedParties = records.GetByteArrayOrNull("affected_parties");
                   }
            );
            return routeAnalysis;
        }
        #endregion

        #region GetVehicleComponentAxles
        public static List<VehComponentAxles> GetVehicleComponentAxles(int NotificationID, long VehicleID)
        {
            string messg = "OutboundDAO/GetVehicleComponentAxles?NotificationID=" + NotificationID + ", VehicleID=" + VehicleID;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            List<VehComponentAxles> componentAxleList = new List<VehComponentAxles>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                componentAxleList,
                  UserSchema.Portal + ".GET_OUTBOUND_AXLE_CONFIG",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_Vehicle_Id", VehicleID, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.AxleCount = records.GetInt16OrDefault("axle_count");
                        instance.ComponentId = records.GetLongOrDefault("Component_id");
                        instance.NextAxleDistNoti = records.GetDecimalOrDefault("NEXT_AXLE_DIST");
                        instance.TyreSize = records.GetStringOrDefault("tyre_size");

                        instance.Weight = records.GetFloatOrDefault("weight");
                        instance.WheelCount = records.GetInt16OrDefault("wheel_count");
                        instance.AxleNumber = records.GetInt16OrDefault("AXLE_NO");
                        instance.WheelSpacingList = records.GetStringOrDefault("wheel_spacing_list");
                        instance.AxleSpacingToFollowing = Convert.ToDouble(records.GetDecimalOrDefault("AXLE_SPACE_TO_FOLLOW"));
                    }
            );
            return componentAxleList;
        }
        #endregion

        #region GetVehicleComponentAxlesByComponent
        public static List<VehComponentAxles> GetVehicleComponentAxlesByComponent(long componentID, long VehicleID, string userSchema)
        {
            List<VehComponentAxles> componentAxleList = new List<VehComponentAxles>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                componentAxleList,
                  userSchema + ".GET_AXLE_CONFIG_NONSEMI",
                parameter =>
                {
                    parameter.AddWithValue("P_ComponentId", componentID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VehicleId", VehicleID, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.AxleCount = records.GetInt16OrDefault("axle_count");
                        instance.ComponentId = records.GetLongOrDefault("Component_Id");
                        instance.NextAxleDist = Convert.ToSingle(records["NEXT_AXLE_DIST"]);
                        instance.TyreSize = records.GetStringOrDefault("tyre_size");
                        instance.Weight = records.GetFloatOrDefault("weight");
                        instance.WheelCount = records.GetInt16OrDefault("wheel_count");
                        instance.AxleNumber = records.GetInt16OrDefault("AXLE_NO");
                        instance.WheelSpacingList = records.GetStringOrDefault("wheel_spacing_list");
                    }
            );
            return componentAxleList;
        }
        #endregion

        #region GetSimplifiedRoutePointStart
        public static NotificationXSD.SimplifiedRoutePointStructure GetSimplifiedRoutePointStart(int NotificationID, long routepartid)
        {
            string messg = "OutboundDAO/GetSimplifiedRoutePointStart?NotificationID=" + NotificationID + ", routepartid=" + routepartid;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            NotificationXSD.SimplifiedRoutePointStructure srps = new NotificationXSD.SimplifiedRoutePointStructure();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                srps,
                UserSchema.Portal + ".GET_OUTBOUND_STARTPOINT",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ROUTEPART_ID", routepartid, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
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
        #endregion

        #region GetSimplifiedRoutePointEnd
        public static NotificationXSD.SimplifiedRoutePointStructure GetSimplifiedRoutePointEnd(int NotificationID, long routepartid)
        {
            string messg = "OutboundDAO/GetSimplifiedRoutePointEnd?NotificationID=" + NotificationID + ", routepartid=" + routepartid;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            NotificationXSD.SimplifiedRoutePointStructure srps = new NotificationXSD.SimplifiedRoutePointStructure();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                srps,
                UserSchema.Portal + ".GET_OUTBOUND_ENDPOINT",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ROUTEPART_ID", routepartid, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
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
        #endregion

        #region GetVehicleSummaryConfigurationIdentity
        public static NotificationXSD.VehicleSummaryStructureConfigurationIdentityListPosition[] GetVehicleSummaryConfigurationIdentity(int NotificationID, long vehicleId)
        {
            string messg = "OutboundDAO/GetVehicleSummaryConfigurationIdentity?NotificationID=" + NotificationID;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));
            List<NotificationXSD.VehicleSummaryStructureConfigurationIdentityListPosition> vsscilpList = new List<NotificationXSD.VehicleSummaryStructureConfigurationIdentityListPosition>();


            try
            {
                NotificationXSD.VehicleSummaryStructure vssList = new NotificationXSD.VehicleSummaryStructure();

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    vsscilpList,
                    UserSchema.Portal + ".GET_CONFIGURATION_IDENTITY",
                    parameter =>
                    {
                        parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                     (records, instance) =>
                     {
                         NotificationXSD.SummaryVehicleIdentificationStructure svis = new NotificationXSD.SummaryVehicleIdentificationStructure();
                         svis.PlateNo = records.GetStringOrDefault("LICENSE_PLATE");
                         svis.FleetNumber = records.GetStringOrDefault("FLEET_NO");
                         instance.ConfigurationIdentity = svis;
                     }
                    );


            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - GetVehicleSummaryConfigurationIdentity,Exception:" + ex);
            }
            return vsscilpList.ToArray();
        }
        #endregion

        #region GetStructuresXML
        public static StructuresModel GetStructuresXML(int NotificationID)
        {
            StructuresModel structuresDetail = new StructuresModel();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                structuresDetail,
                UserSchema.Portal + ".GET_OUTBOUND_DRIVINGINS_DETAIL",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       instance.AffectedStructures = records.GetByteArrayOrNull("AFFECTED_STRUCTURES");
                   }
            );
            return structuresDetail;
        }
        #endregion

        #region GetCautionsXML
        public static RouteAssessmentModel GetCautionsXML(int NotificationID)
        {
            RouteAssessmentModel routeAssessmentModel = new RouteAssessmentModel();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                routeAssessmentModel,
                UserSchema.Portal + ".GET_OUTBOUND_DRIVINGINS_DETAIL",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       instance.Cautions = records.GetByteArrayOrNull("CAUTIONS");
                   }
            );
            return routeAssessmentModel;
        }
        #endregion

        #region GetStructuresXMLByESDALReference
        public static StructuresModel GetStructuresXMLByESDALReference(string esdalReference, string userSchema)
        {
            StructuresModel structuresDetail = new StructuresModel();

            esdalReference = esdalReference.Replace("~", "#");
            esdalReference = esdalReference.Replace("-", "/");
            esdalReference = esdalReference.Replace("#", "/");
            string[] esdalRefPro = esdalReference.Split('/');

            string haulierMnemonic = string.Empty;
            string esdalrefnum = string.Empty;
            int versionNo = 0;

            if (esdalRefPro.Length > 0)
            {
                haulierMnemonic = Convert.ToString(esdalRefPro[0]);
                esdalrefnum = Convert.ToString(esdalRefPro[1].ToUpper().Replace("S", ""));
                versionNo = Convert.ToInt32(esdalRefPro[2].ToUpper().Replace("S", ""));
            }

            if (esdalReference != null)
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                structuresDetail,
                userSchema + ".GET_STRUCTDET_ESDALREF",
                parameter =>
                {
                    parameter.AddWithValue("p_HAULIER_MNEMONIC", haulierMnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ESDAL_REF_NUMBER", esdalrefnum, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VERSION_NO", versionNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.AffectedStructures = records.GetByteArrayOrNull("AFFECTED_STRUCTURES");
                }
               );
            }

            return structuresDetail;
        }
        #endregion

        #region GetStructureDataDetails
        public static AffectedStructuresStructure GetStructureDataDetails(int NotificationID, string RoutePartName, byte[] affectedStructures)
        {
            string messg = "OutboundDAO/GetStructureDataDetails?NotificationID=" + NotificationID + ", RoutePartName=" + RoutePartName;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            AffectedStructuresStructure affStructures = new AffectedStructuresStructure();


            try
            {

                StructuresModel struInfo = new StructuresModel
                {
                    AffectedStructures = affectedStructures
                };
                    //GetStructuresXML(NotificationID);

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
                    catch
                    {
                        //Some data is stored in gzip format, so we need to unzip then load it.
                        byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(affectedPartiesArray);

                        recipientXMLInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);

                        xmlDoc.LoadXml(recipientXMLInformation);
                    }

                    List<StructuresModel> struList = new List<StructuresModel>();
                    XNamespace StructNS = "http://www.esdal.com/schemas/core/routeanalysis";

                    NotificationXSD.StructureSuitabilityStructure appraisalRecord = new NotificationXSD.StructureSuitabilityStructure();

                    XDocument StructXDocument = XDocument.Parse(xmlDoc.OuterXml); // Converting from XMLDocument to XDocument

                    XDocument SelectedStructures = new XDocument(new XElement("AddedElements", from p in StructXDocument.Root.Elements(StructNS + "AnalysedStructuresPart")
                                                                                               where p.Element(StructNS + "Name").Value == RoutePartName
                                                                                               select p.Elements(StructNS + "Structure"))); // Fetching structures in XDocument object

                    XmlDocument StructXMLDocument = new XmlDocument();
                    string StructModelString = string.Empty;
                    StructModelString = SelectedStructures.ToString();
                    StructXMLDocument.LoadXml(StructModelString); // Converting XDocument to XMLDocument

                    XmlNodeList parentNode = StructXMLDocument.GetElementsByTagName("Structure");

                    List<NotificationXSD.StructureSectionSuitabilityStructure> individaulSection = new List<NotificationXSD.StructureSectionSuitabilityStructure>();
                    NotificationXSD.StructureSectionSuitabilityStructure structureSection = new NotificationXSD.StructureSectionSuitabilityStructure();

                    List<NotificationXSD.SuitabilityResultStructure> resultStructureList = new List<NotificationXSD.SuitabilityResultStructure>();
                    NotificationXSD.SuitabilityResultStructure resultStructure = new NotificationXSD.SuitabilityResultStructure();

                    AffectedStructureStructure structure = new AffectedStructureStructure();

                    List<AffectedStructureStructure> affectedstructList = new List<AffectedStructureStructure>();

                    foreach (XmlElement childrenNode in parentNode)
                    {

                        bool isConstraint = false;
                        bool isInFailedDelegation = false;

                        bool isMyResponsibility = false;
                        bool isRetainNotification = false;

                        int structureSectionId = 0;
                        int orgId = 0;
                        string traversalType = string.Empty;

                        string structureCode = string.Empty;
                        string structureName = string.Empty;

                        if ((childrenNode != null) && childrenNode.HasAttribute("StructureSectionId"))
                        {
                            structureSectionId = Convert.ToInt32(childrenNode.Attributes["StructureSectionId"].InnerText);
                        }

                        if ((childrenNode != null) && childrenNode.HasAttribute("IsConstrained"))
                        {
                            isConstraint = Convert.ToBoolean(childrenNode.Attributes["IsConstrained"].InnerText);
                        }

                        if ((childrenNode != null) && childrenNode.HasAttribute("IsInFailedDelegation"))
                        {
                            isInFailedDelegation = Convert.ToBoolean(childrenNode.Attributes["IsInFailedDelegation"].InnerText);
                        }

                        if ((childrenNode != null) && childrenNode.HasAttribute("IsMyResponsibility"))
                        {
                            isMyResponsibility = Convert.ToBoolean(childrenNode.Attributes["IsMyResponsibility"].InnerText);
                        }

                        if ((childrenNode != null) && childrenNode.HasAttribute("IsRetainNotificationOnly"))
                        {
                            isRetainNotification = Convert.ToBoolean(childrenNode.Attributes["IsRetainNotificationOnly"].InnerText);
                        }

                        if ((childrenNode != null) && childrenNode.HasAttribute("TraversalType"))
                        {
                            traversalType = Convert.ToString(childrenNode.Attributes["TraversalType"].InnerText);
                        }
                        if (childrenNode != null)
                        {
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

                                if (xmlElement.Name == "StructureResponsibility")
                                {
                                    foreach (XmlElement xmlElement1 in xmlElement.ChildNodes)
                                    {
                                        if ((xmlElement1 != null) && xmlElement1.HasAttribute("OrganisationId"))
                                        {
                                            orgId = Convert.ToInt32(xmlElement1.Attributes["OrganisationId"].InnerText);
                                        }

                                    }
                                }

                                if (xmlElement.Name == "Appraisal")
                                {

                                    string suitability = string.Empty;
                                    string OrganisationName = string.Empty;

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
                                            OrganisationName = xmlElement1.InnerText;
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

                                                    resultStructure = new NotificationXSD.SuitabilityResultStructure
                                                    {
                                                        Suitability = GetSuitability(childSectionSuitability),
                                                        ResultDetails = childSectionResultDetails,
                                                        TestClass = childSectionTestClass == "dimensional constraint" ? NotificationXSD.SuitabilityTestClassType.dimensionalconstraint : NotificationXSD.SuitabilityTestClassType.ICA,
                                                        TestIdentity = childSectionTestIdentity
                                                    };

                                                    resultStructureList.Add(resultStructure);
                                                }

                                                structureSection = new NotificationXSD.StructureSectionSuitabilityStructure
                                                {
                                                    Suitability = sectionSuitability.ToLower() == "suitable" ? NotificationXSD.SuitabilityType.suitable : sectionSuitability.ToLower() == "marginal" ? NotificationXSD.SuitabilityType.marginal : sectionSuitability.ToLower() == "unsuitable" ? NotificationXSD.SuitabilityType.unsuitable : NotificationXSD.SuitabilityType.unknown,
                                                    SectionDescription = sectionDescription,
                                                    IndividualResult = resultStructureList.ToArray()
                                                };

                                                individaulSection.Add(structureSection);
                                            }
                                        }
                                    }

                                    appraisalRecord = new NotificationXSD.StructureSuitabilityStructure()
                                    {
                                        Suitability = GetSuitability(suitability),
                                        Organisation = OrganisationName,
                                        OrganisationId = orgId,
                                        IndividualSectionSuitability = individaulSection.ToArray()
                                    };
                                }
                            }
                        }


                        structure = new AffectedStructureStructure
                        {
                            StructureSectionId = structureSectionId,
                            IsConstrained = isConstraint,
                            IsInFailedDelegation = isInFailedDelegation,
                            IsRetainNotificationOnly = isRetainNotification,
                            IsMyResponsibility = isMyResponsibility,

                            TraversalType = traversalType.ToLower() == "overbridge" ? NotificationXSD.StructureTraversalType.overbridge : traversalType.ToLower() == "underbridge" ? NotificationXSD.StructureTraversalType.underbridge : traversalType.ToLower() == "levelcrossing" ? NotificationXSD.StructureTraversalType.levelcrossing : NotificationXSD.StructureTraversalType.archedoverbridge,

                            ESRN = structureCode,
                            Name = structureName,
                            Appraisal = appraisalRecord,

                        };

                        affectedstructList.Add(structure);
                    }

                    affStructures.Structure = affectedstructList.ToArray();
                }
                else
                {
                    affStructures.Structure = null;
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("OutBoundDAO/GetStructureDataDetails, Exception: {0}", ex));
            }
            return affStructures;
        }
        #endregion

        #region GetSuitability
        public static NotificationXSD.SuitabilityType GetSuitability(string suitability)
        {
            if (suitability.ToLower() == "suitable")
                return NotificationXSD.SuitabilityType.suitable;
            else if (suitability.ToLower() == "marginal")
                return NotificationXSD.SuitabilityType.marginal;
            else if (suitability.ToLower() == "unsuitable")
                return NotificationXSD.SuitabilityType.unsuitable;
            else
                return NotificationXSD.SuitabilityType.unknown;
        }
        #endregion

        #region GetCautionDataDetails
        public static AnalysedCautions GetCautionDataDetails(RouteAssessmentModel objRouteAssessmentModel)
        {
            AnalysedCautions existingAnalysedCautions = null;
            try
            {
                if (objRouteAssessmentModel != null && objRouteAssessmentModel.Cautions != null)
                {
                    string affectedCautionsxml = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.Cautions));
                    try
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(AnalysedCautions));

                        StringReader read = new StringReader(affectedCautionsxml);

                        using (XmlReader xmlReader = new XmlTextReader(read))
                        {
                            Object obj = serializer.Deserialize(xmlReader);

                            existingAnalysedCautions = (AnalysedCautions)obj;

                            xmlReader.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        return existingAnalysedCautions;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("OutBoundDAO/GetStructureDataDetails, Exception: {0}", ex));
            }
            return existingAnalysedCautions;
        }
        #endregion

        #region Commented Code By Mahzeer on 12/07/2023
        /*
        public static OutboundNotificationStructure GetOutboundNotificationDetailsForNotification(Enums.PortalType psPortalType, int NotificationID, bool isHaulier, int ContactId)
        {
            string messg = "OutboundDAO/GetOutboundNotificationDetailsForNotification?NotificationID=" + NotificationID + ", isHaulier=" + isHaulier + ", ContactId=" + ContactId;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            OutboundNotificationStructure odns = new OutboundNotificationStructure();
            NotificationVR1InformationStructure vr1Info = new NotificationVR1InformationStructure();
            VR1NumbersStructure vr1Str = new VR1NumbersStructure();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                odns,
                UserSchema.Portal + ".GET_OUTBOUND_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTACT_ID", ContactId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       NotificationXSD.ESDALReferenceNumberStructure esdalrefnostru = new NotificationXSD.ESDALReferenceNumberStructure();
                       esdalrefnostru.Mnemonic = records.GetStringOrDefault("HAULIER_MNEMONIC");
                       string[] hauMnemonicArr = records.GetStringOrDefault("NOTIFICATION_CODE").Split("/".ToCharArray());
                       if (hauMnemonicArr.Length > 0)
                       {
                           esdalrefnostru.MovementProjectNumber = hauMnemonicArr[1];
                       }

                       esdalrefnostru.ESDALReferenceNo = records.GetStringOrDefault("NOTIFICATION_CODE");
                       esdalrefnostru.VSONo = records.GetStringOrDefault("VSO_NUMBER");
                       instance.NotificationOnEscort = records.GetStringOrDefault("NOTESONESCORT");

                       NotificationXSD.MovementVersionNumberStructure movvernostru = new NotificationXSD.MovementVersionNumberStructure();
                       movvernostru.Value = records.GetShortOrDefault("VERSION_NO");
                       esdalrefnostru.MovementVersion = movvernostru;

                       esdalrefnostru.NotificationNumber = records.GetShortOrDefault("NOTIFICATION_NO");
                       esdalrefnostru.NotificationNumberSpecified = true;

                       instance.ESDALReferenceNumber = esdalrefnostru;                       

                       //HttpContext.Current.Session["ESDALRefNumber"] = esdalrefnostru.ESDALReferenceNo; //RM#3659

                       // Start for #3846
                       vr1Str.Scottish = records.GetStringOrDefault("VR1_NUMBER");
                       vr1Info.Numbers = vr1Str;
                       instance.VR1Information = vr1Info;
                       // End for #3846


                       if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.VehicleSpecialOrder)
                       {
                           instance.Classification = MovementClassificationType.vehiclespecialorder;
                       }
                       if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.SpecialOrder)
                       {
                           instance.Classification = MovementClassificationType.specialorder;
                       }
                       if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoailCat1)
                       {
                           instance.Classification = MovementClassificationType.stgoailcat1;
                       }
                       if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoailCat2)
                       {
                           instance.Classification = MovementClassificationType.stgoailcat2;
                       }
                       if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoailCat3)
                       {
                           instance.Classification = MovementClassificationType.stgoailcat3;
                       }
                       if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoailCat3)
                       {
                           instance.Classification = MovementClassificationType.stgoailcat3;
                       }
                       if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoMobileCraneCata)
                       {
                           instance.Classification = MovementClassificationType.stgomobilecranecata;
                       }
                       if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoMobileCraneCatb)
                       {
                           instance.Classification = MovementClassificationType.stgomobilecranecatb;
                       }
                       if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoMobileCraneCatc)
                       {
                           instance.Classification = MovementClassificationType.stgomobilecranecatc;
                       }
                       if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoRoadRecoveryVehicle)
                       {
                           instance.Classification = MovementClassificationType.stgoroadrecoveryvehicle;
                       }
                       if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.WheeledConstructionAndUse)
                       {
                           instance.Classification = MovementClassificationType.wheeledconstructionanduse;
                       }
                       if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.Tracked)
                       {
                           instance.Classification = MovementClassificationType.tracked;
                       }
                       if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoEngineeringPlantWheeled)
                       {
                           instance.Classification = MovementClassificationType.stgoengineeringplantwheeled;
                       }
                       if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoEngineeringPlantTracked)
                       {
                           instance.Classification = MovementClassificationType.stgoengineeringplanttracked;
                       }
                       else if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoCat1EngineeringPlantWheeled)
                       {
                           instance.Classification = MovementClassificationType.StgoCat1EngineeringPlantWheeled;
                       }
                       else if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoCat2EngineeringPlantWheeled)
                       {
                           instance.Classification = MovementClassificationType.StgoCat2EngineeringPlantWheeled;
                       }
                       else if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoCat3EngineeringPlantWheeled)
                       {
                           instance.Classification = MovementClassificationType.StgoCat3EngineeringPlantWheeled;
                       }

                       string specialOrderESDALRefNo = esdalrefnostru.Mnemonic + "/" + esdalrefnostru.MovementProjectNumber + "/S" + movvernostru.Value;
                       SignedOrderSummaryStructure soss;
                       soss = GetSpecialOrderNo(specialOrderESDALRefNo);

                       instance.DftReference = soss.OrderNumber == null ? records.GetStringOrDefault("SO_NUMBERS") : soss.OrderNumber;

                       instance.ClientName = records.GetStringOrDefault("Client_Descr");

                       instance.JobFileReference = records.GetStringOrDefault("HA_JOB_FILE_REF");

                       if (instance.JobFileReference == string.Empty)
                       {
                           instance.JobFileReference = "\u2002";
                       }

                       NotificationXSD.HaulierDetailsStructure hds = new NotificationXSD.HaulierDetailsStructure();
                       //string name = string.Empty;
                       //name = records.GetStringOrDefault("first_name");
                       //if (records.GetStringOrDefault("sur_name") != string.Empty)
                       //{
                       //    if (name != string.Empty)
                       //        name = name + " " + records.GetStringOrDefault("sur_name");
                       //}
                       instance.HauliersReference = records.GetStringOrDefault("hauliers_ref");
                       hds.HaulierContact = records.GetStringOrDefault("HAULIER_CONTACT");
                       hds.HaulierName = records.GetStringOrDefault("HAULIER_NAME").Trim();
                       hds.TelephoneNumber = records.GetStringOrDefault("HAULIER_TEL_NO");
                       hds.FaxNumber = records.GetStringOrDefault("HAULIER_Fax_NO");
                       hds.EmailAddress = records.GetStringOrDefault("HAULIER_Email");
                       hds.OrganisationId = records.GetLongOrDefault("organisation_id");
                       hds.OrganisationIdSpecified = Convert.ToString(records.GetLongOrDefault("organisation_id")) != string.Empty;
                       hds.Licence = records.GetStringOrDefault("HAULIER_LICENCE_NO");

                       NotificationXSD.AddressStructure has = new NotificationXSD.AddressStructure();

                       string[] Addstru = new string[5];
                       Addstru[0] = records.GetStringOrDefault("HAULIER_ADDRESS_1");
                       Addstru[1] = records.GetStringOrDefault("HAULIER_ADDRESS_2");
                       Addstru[2] = records.GetStringOrDefault("HAULIER_ADDRESS_3");
                       Addstru[3] = records.GetStringOrDefault("HAULIER_ADDRESS_4");
                       Addstru[4] = records.GetStringOrDefault("HAULIER_ADDRESS_5");
                       has.Line = Addstru;
                       has.PostCode = records.GetStringOrDefault("haulier_post_code");

                       int country = records.GetInt32OrDefault("COUNTRY");
                       if (country == (int)Country.england)
                       {
                           has.Country = NotificationXSD.CountryType.england;
                           has.CountrySpecified = true;
                       }
                       else if (country == (int)Country.northernireland)
                       {
                           has.Country = NotificationXSD.CountryType.northernireland;
                           has.CountrySpecified = true;
                       }
                       else if (country == (int)Country.scotland)
                       {
                           has.Country = NotificationXSD.CountryType.scotland;
                           has.CountrySpecified = true;
                       }
                       else if (country == (int)Country.wales)
                       {
                           has.Country = NotificationXSD.CountryType.wales;
                           has.CountrySpecified = true;
                       }
                       hds.HaulierAddress = has;
                       instance.HaulierDetails = hds;

                       #region JourneyFromToSummary
                       JourneyFromToSummaryStructure jftss = new JourneyFromToSummaryStructure();

                       jftss.From = records.GetStringOrDefault("FROM_DESCR");

                       jftss.To = records.GetStringOrDefault("TO_DESCR");

                       instance.JourneyFromToSummary = jftss;
                       #endregion

                       #region JourneyFromTo
                       JourneyFromToStructure jfts = new JourneyFromToStructure();
                       jfts.From = records.GetStringOrDefault("FROM_DESCR");

                       jfts.To = records.GetStringOrDefault("TO_DESCR");

                       instance.JourneyFromTo = jfts;
                       #endregion

                       #region JourneyTiming
                       JourneyTimingStructure jts = new JourneyTimingStructure();
                       jts.FirstMoveDate = records.GetDateTimeOrDefault("movement_start_date");
                       jts.LastMoveDate = records.GetDateTimeOrDefault("movement_end_date");
                       jts.LastMoveDateSpecified = true;
                       jts.StartTime = Convert.ToString(jts.FirstMoveDate.TimeOfDay);
                       jts.EndTime = Convert.ToString(jts.LastMoveDate.TimeOfDay);
                       instance.JourneyTiming = jts;
                       #endregion

                       #region LoadDetails
                       LoadDetailsStructure lds = new LoadDetailsStructure();
                       lds.Description = records.GetStringOrDefault("load_descr");
                       lds.TotalMoves = Convert.ToString(records.GetInt16OrDefault("NO_OF_MOVES"));
                       lds.MaxPiecesPerMove = Convert.ToString(records.GetInt32OrDefault("max_pieces_per_move"));
                       lds.MaxPiecesPerMoveSpecified = true;
                       instance.LoadDetails = lds;
                       instance.NotificationNotesFromHaulier = records.GetStringOrDefault("HAUL_NOTES");
                       InboundNotificationStructure inBoundStructure = new InboundNotificationStructure();
                       inBoundStructure.OnBehalfOf = records.GetStringOrDefault("ON_BEHALF_OF");
                       instance.OnBehalfOf = inBoundStructure.OnBehalfOf;
                       #endregion

                       #region Indemnity Information

                       IndemnityStructure indemnityDetails = new IndemnityStructure();

                       bool isIndemnityPresent = records.GetInt16OrDefault("INDEMNITY_CONFIRMATION") > 0;

                       short totalMoves = 0;

                       if (lds.TotalMoves != string.Empty)
                       {
                           totalMoves = Convert.ToInt16(lds.TotalMoves);
                       }
                       indemnityDetails.MultipleMoves = totalMoves > 1;

                       indemnityDetails.Confirmed = isIndemnityPresent;

                       indemnityDetails.OnBehalfOf = instance.OnBehalfOf;

                       indemnityDetails.Haulier = records.GetStringOrDefault("HAULIER_CONTACT");

                       indemnityDetails.SignedDate = DateTime.Today;

                       indemnityDetails.Signatory = records.GetStringOrDefault("HAULIER_CONTACT");

                       MovementTimingStructure movementStructure = new MovementTimingStructure();
                       MovementTimingStructureMovementDateRange movementTiming = new MovementTimingStructureMovementDateRange();

                       movementTiming.FromDate = jts.FirstMoveDate;
                       movementTiming.ToDate = jts.LastMoveDate;

                       movementStructure.Item = movementTiming;

                       indemnityDetails.Timing = movementStructure;

                       if (!isIndemnityPresent)
                           instance.IndemnityConfirmation = null;
                       else
                           instance.IndemnityConfirmation = indemnityDetails;

                       #endregion

                       #region Dispensation

                       //int dispensationCount = Convert.ToInt32(Doc.GetElementsByTagName("InboundDispensation").Count);

                       //List<OutboundDispensationStructure> dispensationDetails = GetDispensationList(NotificationID);

                       //instance.Dispensations = dispensationDetails.ToArray();

                       #endregion

                       instance.SentDateTime = records.GetDateTimeOrEmpty("notification_date");

                       #region SOInformation
                       try
                       {
                           instance.SOInformation = GetSODetail(specialOrderESDALRefNo);
                       }
                       catch { }

                       if (instance.SOInformation != null && instance.SOInformation.Summary != null && instance.SOInformation.Summary[0].CurrentOrder != null && instance.SOInformation.Summary[0].CurrentOrder.OrderNumber != null)
                       {
                           instance.JobFileReference = instance.SOInformation.Summary[0].CurrentOrder.HAJobRefNumber;
                       }

                       #endregion

                       #region Recipients
                       instance.Recipients = GetRecipientContactStructure(NotificationID).ToArray();
                       #endregion

                       #region RouteParts
                       List<RoutePartsStructureRoutePartListPosition> rpsrplpList = new List<RoutePartsStructureRoutePartListPosition>();
                       rpsrplpList = GetRouteParts(NotificationID, isHaulier);
                       instance.RouteParts = rpsrplpList.ToArray();
                       #endregion

                       instance.OldNotificationID = Convert.ToInt64(records.GetDecimalOrDefault("old_noti"));
                       instance.OrganisationName = records.GetStringOrDefault("orgname");
                   }
            );

            return odns;
        }
        
        public static List<AffStructureGeneralDetails> GetStructureGeneralDetailList(string structureCode, int sectionId)
        {

            List<AffStructureGeneralDetails> objstructures = new List<AffStructureGeneralDetails>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               objstructures,
                   UserSchema.Portal + ".SP_AFFECTED_STRUCT_DETAILS",
               parameter =>
               {

                   parameter.AddWithValue("P_STRUCT_CODE", structureCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_SECTION_ID", sectionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                   (records, instance) =>
                   {
                       instance.StructureChainNo = records.GetLongOrDefault("CHAIN_NO");
                       instance.StructurePosition = records.GetInt16OrDefault("POSITION");
                       instance.ESRN = records.GetStringOrDefault("STRUCTURE_CODE");
                       instance.StructureDescription = records.GetStringOrDefault("DESCRIPTION");
                       instance.StructureClass = records.GetStringOrDefault("STRUCTURE_CLASS");
                       instance.OSGR = records.GetLongOrDefault("NORTHING");
                       instance.Easting = records.GetLongOrDefault("EASTING");
                       instance.StructureType = records.GetStringOrDefault("STRUCT_TYPE");
                       instance.StructureType1 = records.GetStringOrDefault("STRUCT_TYPE1");
                       instance.StructureType2 = records.GetStringOrDefault("STRUCT_TYPE2");
                       instance.OwnerName = records.GetStringOrDefault("OWNER");
                       instance.AlternativeName = records.GetStringOrDefault("ALTERNATIVENAME");
                       instance.Notes = records.GetStringOrDefault("NOTES");
                       instance.ContactId = Convert.ToInt64(records["CONTACT_ID"]); //this function will convert the record object to Int64 #7197
                       instance.StructureId = records.GetLongOrDefault("STRUCTURE_ID");
                       instance.StructureName = records.GetStringOrDefault("STRUCTURE_NAME");
                       instance.StructureKey = records.GetStringOrDefault("STRUCT_KEY");
                       instance.OrganisationId = records.GetLongOrDefault("ORGANISATION_ID");
                       instance.StructureCategory = records.GetStringOrDefault("STRUCT_CATEGORY");
                       instance.StructureLength = records.GetDoubleOrDefault("STRUCTURE_LENGTH");
                       if (sectionId != 0)
                       {
                           //Structure Imposed Constraints detail
                           instance.SignedAxleGroupLength = records.GetDoubleOrDefault("SIGN_AXLE_GROUP_LEN");
                           instance.SignedAxleGroupWeight = records.GetInt32OrDefault("SIGN_AXLE_GROUP_WEIGHT");
                           instance.SignedDoubleAxleWeight = records.GetInt32OrDefault("SIGN_DOUBLE_AXLE_WEIGHT");
                           instance.SignedGrossWeight = records.GetInt32OrDefault("SIGN_GROSS_WEIGHT");
                           instance.SignedHeightInFeet = records.GetDoubleOrDefault("SIGN_HEIGHT_FEET");
                           instance.SignedHeightInMetres = records.GetDoubleOrDefault("SIGN_HEIGHT_METRES");
                           instance.SignedLengthInFeet = records.GetDoubleOrDefault("SIGN_LEN_FEET");
                           instance.SignedLengthInMetres = records.GetDoubleOrDefault("SIGN_LEN_METRES");
                           instance.SignedSingleAxleWeight = records.GetInt32OrDefault("SIGN_SINGLE_AXLE_WEIGHT");
                           instance.SignedTripleAxleWeight = records.GetInt32OrDefault("SIGN_TRIPLE_AXLE_WEIGHT");
                           instance.SignedWidthInFeet = records.GetDoubleOrDefault("SIGN_WIDTH_FEET");
                           instance.SignedWidthInMetres = records.GetDoubleOrDefault("SIGN_WIDTH_METRES");
                       }

                   }
              );
            return objstructures;
        }
        public static AffectedStructureStructure GetAffectedStructures(int struSectionId)
        {
            AffectedStructureStructure affectedstruct = new AffectedStructureStructure();
            affectedstruct.IsConstrained = false;
            affectedstruct.IsInFailedDelegation = false;
            affectedstruct.IsMyResponsibility = false;
            affectedstruct.IsRetainNotificationOnly = false;
            affectedstruct.StructureSectionId = struSectionId;
            affectedstruct.StructureSectionIdSpecified = true;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                affectedstruct,
                UserSchema.Portal + ".GET_AFFECTEDSTRUCTURE_DETAIL",
                parameter =>
                {
                    parameter.AddWithValue("p_STRUCTURE_SECTION_ID", struSectionId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       if (records.GetInt32OrDefault("structure_class") == 510001)
                       {
                           instance.TraversalType = NotificationXSD.StructureTraversalType.underbridge;
                       }
                       if (records.GetInt32OrDefault("structure_class") == 510002)
                       {
                           instance.TraversalType = NotificationXSD.StructureTraversalType.overbridge;
                       }
                       if (records.GetInt32OrDefault("structure_class") == 510003)
                       {
                           instance.TraversalType = NotificationXSD.StructureTraversalType.archedoverbridge;
                       }
                       if (records.GetInt32OrDefault("structure_class") == 510004)
                       {
                           instance.TraversalType = NotificationXSD.StructureTraversalType.levelcrossing;
                       }
                       instance.ESRN = records.GetStringOrDefault("structure_code");
                       instance.Name = records.GetStringOrDefault("structure_name");

                   }
            );
            return affectedstruct;
        }
        public static string getNENRouteDetails(long NENInboxId, int OrganisationId, int flag = 1)
        {
            string errormsg;
            string result = string.Empty;
            Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
            //For RM#4311 Change
            string[] separators = { "Split" };
            string resultPart = string.Empty;
            string FinalResult = string.Empty;
            string[] versionSplit = new string[] { };
            StringBuilder sb = new StringBuilder();
            //End

            DrivingInstructionModel DrivingInstructionInfo = GetNENRouteDescription(NENInboxId, OrganisationId);

            string path = "";
            if (flag == 1)
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\RouteDetails.xslt");
            else
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\RouteDetailsImperial.xslt");

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
        */
        #endregion

        #region GetSODetail
        public static NotificationSOInformationStructure GetSODetail(string esdalRefNumber)
        {
            string messg = "OutboundDAO/GetSODetail?esdalRefNumber=" + esdalRefNumber;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            NotificationSOInformationStructure soInformation = new NotificationSOInformationStructure();
            PossiblyReplacingSignedOrderSummaryStructure[] prsosList = new PossiblyReplacingSignedOrderSummaryStructure[1];
            PossiblyReplacingSignedOrderSummaryStructure prsos = new PossiblyReplacingSignedOrderSummaryStructure();


            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                soInformation,
                UserSchema.Portal + ".GET_SO_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_ESDAL_REF_NUMBER", esdalRefNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {

                       SignedOrderSummaryStructure soss = new SignedOrderSummaryStructure();

                       soss.IsAmendmentOrder = records.GetDecimalOrDefault("IS_AMENDMENT_ORDER") == 1 ? true : false;
                       soss.OrderNumber = records.GetStringOrDefault("ORDER_NO");
                       soss.SignedOn = records.GetDateTimeOrDefault("SIGNED_DATE");
                       soss.ExpiresOn = records.GetDateTimeOrDefault("EXPIRY_DATE");
                       soss.SignedBy = records.GetStringOrDefault("SIGNATORY");
                       soss.HAJobRefNumber = records.GetStringOrDefault("HA_JOB_FILE_REF");
                       prsos.CurrentOrder = soss;
                       prsosList[0] = prsos;
                       instance.Summary = prsosList;

                       instance.HAJobRefNumber = records.GetStringOrDefault("HA_JOB_FILE_REF");

                       decimal status = records.GetDecimalOrDefault("STATE");
                       instance.Status = SOStatusType.expired;
                       if (status == 230001)
                       {
                           instance.Status = SOStatusType.proposedrouteonly;
                       }
                       else if (status == 230002)
                       {
                           instance.Status = SOStatusType.expired;
                       }
                       else if (status == 230003)
                       {
                           instance.Status = SOStatusType.granted;
                       }
                       else if (status == 230004)
                       {
                           instance.Status = SOStatusType.externaltoesdal;
                       }
                       else if (status == 230005)
                       {
                           instance.Status = SOStatusType.reclearance;
                       }
                   }
                  );

            if (soInformation.Summary == null)
            {
                return null;
            }
            else if (soInformation.Summary != null && soInformation.Summary[0].CurrentOrder.OrderNumber == string.Empty)
            {
                return null;
            }
            else
            {
                return soInformation;
            }
        }
        #endregion

        #region GetRecipientDetails
        public static ContactModel GetRecipientDetails(int NotificationID)
        {
            ContactModel contactDetail = new ContactModel();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                contactDetail,
                UserSchema.Portal + ".GET_OUTBOUND_DRIVINGINS_DETAIL",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       instance.AffectedParties = records.GetByteArrayOrNull("AFFECTED_PARTIES");
                   }
            );
            return contactDetail;
        }
        #endregion

        #region GetDrivingInstructionStructures
        public static DrivingInstructionModel GetDrivingInstructionStructures(int NotificationID)
        {
            string messg = "OutboundDAO/GetDrivingInstructionStructures?NotificationID=" + NotificationID;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            DrivingInstructionModel structuresDetail = new DrivingInstructionModel();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                structuresDetail,
                UserSchema.Portal + ".GET_OUTBOUND_DRIVINGINS_DETAIL",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       instance.DrivingInstructions = records.GetByteArrayOrNull("DRIVING_INSTRUCTIONS");
                   }
            );
            return structuresDetail;
        }
        #endregion

        #region getRoadDetails
        public static AffectedRoadsStructure getRoadDetails(int NotificationID,DrivingInstructionModel DrivingInstructionInfo)
        {
            string messg = "OutboundDAO/getRoadDetails?NotificationID=" + NotificationID;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            AffectedRoadsStructure affRoadStruct = new AffectedRoadsStructure();
            //DrivingInstructionModel DrivingInstructionInfo = GetDrivingInstructionStructures(NotificationID);

            string recipientXMLInformation = string.Empty;

            Byte[] DrivingInstructionArray = DrivingInstructionInfo.DrivingInstructions;

            if (DrivingInstructionArray != null)
            {
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

                XmlNodeList parentNode = xmlDoc.GetElementsByTagName("Navigation");
                NavigationXML navigationXML;
                List<NavigationXML> navigationXMLs = new List<NavigationXML>();
                foreach (XmlNode childrenNode in parentNode)
                {
                    if (childrenNode.InnerText.ToLower().Contains("continue "))
                    {
                        navigationXML = new NavigationXML();
                        navigationXML.Instruction = childrenNode["Instruction"].InnerText;
                        navigationXML.MeasuredMetric = childrenNode.LastChild["MeasuredMetric"] == null ? 0 : Convert.ToDecimal(childrenNode.LastChild["MeasuredMetric"].InnerText);
                        navigationXML.DisplayMetric = childrenNode.LastChild["DisplayMetric"] == null ? 0 : Convert.ToDecimal(childrenNode.LastChild["DisplayMetric"].InnerText);
                        navigationXML.DisplayImperial = childrenNode.LastChild["DisplayImperial"] == null ? 0 : Convert.ToDecimal(childrenNode.LastChild["DisplayImperial"].InnerText);
                        navigationXMLs.Add(navigationXML);
                    }
                }
                List<NavigationXML> finalNavigationXMLs = new List<NavigationXML>();

                if (navigationXMLs.Count > 0)
                {
                    finalNavigationXMLs = navigationXMLs.GroupBy(x => x.Instruction).Select(g => new NavigationXML
                    {
                        Instruction = g.Key,
                        MeasuredMetric = g.Sum(x => x.MeasuredMetric),
                        DisplayMetric = g.Sum(x => x.DisplayMetric),
                        DisplayImperial = g.Sum(x => x.DisplayImperial)
                    }).ToList();
                }
                decimal totalsmeric = 0;
                decimal totalsimperial = 0;
                foreach (NavigationXML xml in finalNavigationXMLs)
                {
                    if (xml.DisplayImperial > 700)
                    {
                        xml.YardMiles = Math.Round(xml.DisplayImperial / 1760, 1) + " miles ";
                    }
                    else
                    {
                        xml.YardMiles = xml.DisplayImperial + " yards ";
                    }
                    if (xml.DisplayImperial > 0)
                    {
                        totalsimperial = totalsimperial + xml.DisplayImperial;
                    }

                    if (xml.DisplayMetric > 0)
                    {
                        totalsmeric = totalsmeric + xml.DisplayMetric;
                    }
                }

                TotalDistanceMetric = totalsmeric;

                TotalDistanceImperial = totalsimperial;


                affRoadStruct.IsBroken = false;

                List<AffectedRoadsStructureRouteSubPartListPosition> arsrslpList = new List<AffectedRoadsStructureRouteSubPartListPosition>();
                AffectedRoadsStructureRouteSubPartListPosition arsrslp = new AffectedRoadsStructureRouteSubPartListPosition();
                AffectedRoadsSubPartStructure arsps = new AffectedRoadsSubPartStructure();
                List<AffectedRoadsSubPartStructurePathListPosition> arspslpList = new List<AffectedRoadsSubPartStructurePathListPosition>();
                AffectedRoadsSubPartStructurePathListPosition arspslp = new AffectedRoadsSubPartStructurePathListPosition();
                List<AffectedRoadsPathStructureRoadTraversalListPosition> arpsrtlpList = new List<AffectedRoadsPathStructureRoadTraversalListPosition>();

                foreach (NavigationXML xml in finalNavigationXMLs)
                {
                    if (xml.Instruction != string.Empty || (xml.DisplayMetric != 0 && xml.YardMiles != string.Empty))
                    {
                        AffectedRoadsPathStructureRoadTraversalListPosition arpsrtlp1 = new AffectedRoadsPathStructureRoadTraversalListPosition();

                        AffectedRoadStructure ars1 = new AffectedRoadStructure();
                        ars1.IsMyResponsibility = false;
                        ars1.IsStartOfMyResponsibility = false;

                        List<AffectedRoadStructure> affrdconsStruList = new List<AffectedRoadStructure>();
                        NotificationXSD.RoadIdentificationStructure ris = new NotificationXSD.RoadIdentificationStructure();
                        if (xml.Instruction != string.Empty)
                        {
                            ris.Name = xml.Instruction;
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
                arsrslp.RouteSubPart = arsps;
                arsrslpList.Add(arsrslp);
                affRoadStruct.RouteSubPartListPosition = arsrslpList.ToArray();
            }
            return affRoadStruct;
        }
        #endregion

        #region GetRecipientContactStructure
        public static List<NotificationXSD.RecipientContactStructure> GetRecipientContactStructure(int NotificationID, byte[] affectedParties)
        {
            string messg = "OutboundDAO/Get NotificationXSD.RecipientContactStructure?NotificationID=" + NotificationID;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            List<NotificationXSD.RecipientContactStructure> rcsclplist = new List<NotificationXSD.RecipientContactStructure>();
            string contactInformationLog = string.Empty;
            try
            {


                ContactModel contactInfo = new ContactModel
                {
                    AffectedParties = affectedParties
                };
                //GetRecipientDetails(NotificationID);

                string recipientXMLInformation = string.Empty;

                Byte[] affectedPartiesArray = contactInfo.AffectedParties;

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
                        //Some data is stored in gzip format, so we need to unzip then load it.
                        byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(affectedPartiesArray);

                        recipientXMLInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);

                        xmlDoc.LoadXml(recipientXMLInformation);
                    }
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
                    bool isHaulier = false;
                    bool isRecepient = false;

                    string reason = string.Empty;

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

                                if ((!string.IsNullOrEmpty(reason) && reason.ToLower() != "no longer affected") && (string.IsNullOrEmpty(exclusionOutCome) || exclusionOutCome.ToLower() != "no longer affected")
                                                       && (string.IsNullOrEmpty(exclude) || exclude.ToLower() == "false"))
                                {
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

                                        ContactModel cntDetails = GetContactDetails(contactId);

                                        if ((!string.IsNullOrEmpty(reason) && reason.ToLower() != "no longer affected") && (string.IsNullOrEmpty(exclusionOutCome) || exclusionOutCome.ToLower() != "no longer affected")
                                                       && (string.IsNullOrEmpty(exclude) || exclude.ToLower() == "false"))
                                        {
                                            contactInfo = new ContactModel()
                                            {
                                                IsRecipient = isRecepient,
                                                IsHaulier = isHaulier,
                                                ContactId = contactId,
                                                OrganisationId = orgId,
                                                Fax = cntDetails.Fax,
                                                Email = cntDetails.Email,
                                                ISPolice = isPolice,
                                                IsRetainedNotificationOnly = isRetainNotificationOnly,
                                                Reason = reason,
                                                FullName = contactname,
                                                Organisation = orgname,
                                            };

                                            contactList.Add(contactInfo);

                                            contactInformationLog = string.Empty;

                                            contactInformationLog = "OutboundDAO/Get NotificationXSD.RecipientContactStructure?IsRecipient=" + contactInfo.IsRecipient + ", IsHaulier=" + contactInfo.IsHaulier + ", ContactId=" + contactInfo.ContactId + ", OrganisationID=" + contactInfo.OrganisationId + ", Fax=" + contactInfo.Fax + ", Email=" + contactInfo.Email + ", ISPolice=" + contactInfo.ISPolice + ", IsRetainedNotificationOnly=" + contactInfo.IsRetainedNotificationOnly + ", Reason=" + contactInfo.Reason + ", FullName=" + contactInfo.FullName + ", Organisation=" + contactInfo.Organisation;
                                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(contactInformationLog + "; SimpleReference contact Info details"));
                                        }
                                    }
                                    else if (childNode.Name.Contains("AdhocReference"))
                                    {

                                        string contactname = string.Empty;
                                        string orgname = string.Empty;
                                        string Email = string.Empty;
                                        string Fax = string.Empty;

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
                                            Email = childNode["contact:EmailAddress"].InnerText;
                                        }
                                        else
                                        {
                                            Email = childNode["EmailAddress"] != null ? childNode["EmailAddress"].InnerText : null;
                                        }

                                        if (childNode["contact:OrganisationName"] != null)
                                        {
                                            orgname = childNode["contact:OrganisationName"].InnerText;
                                        }
                                        else
                                        {
                                            orgname = childNode["OrganisationName"] != null ? childNode["OrganisationName"].InnerText : null;
                                        }
                                        //here Fax is also fetched
                                        if (childNode["contact:FaxNumber"] != null)
                                        {
                                            Fax = childNode["contact:FaxNumber"].InnerText;
                                        }
                                        else
                                        {
                                            Fax = childNode["FaxNumber"] != null ? childNode["FaxNumber"].InnerText : null;
                                        }
                                        if ((!string.IsNullOrEmpty(reason) && reason.ToLower() != "no longer affected") && (string.IsNullOrEmpty(exclusionOutCome) || exclusionOutCome.ToLower() != "no longer affected")
                                                           && (string.IsNullOrEmpty(exclude) || exclude.ToLower() == "false"))
                                        {
                                            contactInfo = new ContactModel
                                            {

                                                IsRecipient = isRecepient,
                                                IsHaulier = isHaulier,
                                                Fax = Fax,
                                                Email = Email,
                                                ISPolice = isPolice,
                                                IsRetainedNotificationOnly = isRetainNotificationOnly,
                                                Reason = reason,
                                                FullName = contactname,
                                                Organisation = orgname,
                                            };

                                            contactList.Add(contactInfo);

                                            contactInformationLog = string.Empty;

                                            contactInformationLog = "ProposalDAO/Get NotificationXSD.RecipientContactStructure?IsRecipient=" + contactInfo.IsRecipient + ", IsHaulier=" + contactInfo.IsHaulier + ", Fax=" + contactInfo.Fax + ", Email=" + contactInfo.Email + ", ISPolice=" + contactInfo.ISPolice + ", IsRetainedNotificationOnly=" + contactInfo.IsRetainedNotificationOnly + ", Reason=" + contactInfo.Reason + ", FullName=" + contactInfo.FullName + ", Organisation=" + contactInfo.Organisation;
                                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(contactInformationLog + "; Adhoc Reference contact Info details"));
                                        }
                                    }
                                }
                            }
                        }
                    }

                }

                NotificationXSD.RecipientContactStructure rcs1;
                foreach (ContactModel cont in contactList)
                {
                    rcs1 = new NotificationXSD.RecipientContactStructure();
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

                    NotificationXSD.OnBehalfOfStructure onbehalfofstru = new NotificationXSD.OnBehalfOfStructure();

                    onbehalfofstru.DelegationId = cont.DelegationId;
                    onbehalfofstru.DelegationIdSpecified = true;
                    onbehalfofstru.DelegatorsContactId = cont.DelegatorsContactId;
                    onbehalfofstru.DelegatorsContactIdSpecified = true;
                    onbehalfofstru.DelegatorsOrganisationId = cont.DelegatorsOrganisationId;
                    onbehalfofstru.DelegatorsOrganisationIdSpecified = true;
                    onbehalfofstru.DelegatorsOrganisationName = cont.DelegatorsOrganisationName;
                    onbehalfofstru.RetainNotification = cont.RetainNotification;
                    onbehalfofstru.WantsFailureAlert = cont.WantsFailureAlert;

                    if (onbehalfofstru.DelegationId > 0 && onbehalfofstru.DelegatorsContactId > 0)
                    {
                        rcs1.OnbehalfOf = onbehalfofstru;
                    }

                    rcsclplist.Add(rcs1);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
            }
            return rcsclplist;
        }
        #endregion

        #region GetContactDetails
        public static ContactModel GetContactDetails(int contactId)
        {
            ContactModel contactDetail = new ContactModel();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                contactDetail,
                UserSchema.Portal + ".GET_CONTACT_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("p_ContactId", contactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       instance.ContactId = contactId;
                       instance.Email = records.GetStringOrDefault("Email");
                       instance.Fax = records.GetStringOrDefault("Fax");
                       instance.FullName = records.GetStringOrDefault("FIRST_NAME");
                       instance.Organisation = records.GetStringOrDefault("ORGNAME");
                       instance.OrganisationId = (int)records.GetLongOrDefault("ORGANISATION_ID");
                       instance.PhoneNumber = records.GetStringOrDefault("PHONENUMBER");
                   }
            );
            return contactDetail;
        }
        #endregion

        #region GetOrgTypeDetails
        public static bool GetOrgTypeDetails(int OrganisationID)
        {
            ContactModel contactDetail = new ContactModel();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                contactDetail,
                UserSchema.Portal + ".GET_ORGTYPE_DETAIL",
                parameter =>
                {
                    parameter.AddWithValue("p_OrganisationID", OrganisationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
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
        #endregion

        #region GetSpecialOrderNo
        public static SignedOrderSummaryStructure GetSpecialOrderNo(string esDAlRefNo)
        {
            string messg = "OutboundDAO/GetSpecialOrderNo?esDAlRefNo=" + esDAlRefNo;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));
            SignedOrderSummaryStructure srps = new SignedOrderSummaryStructure();
            try
            {

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    srps,
                    UserSchema.Portal + ".GET_SPECIAL_ORDERS",
                    parameter =>
                    {
                        parameter.AddWithValue("p_ESDAL_REF_NUMBER", esDAlRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                     (records, instance) =>
                     {
                         instance.OrderNumber = records.GetStringOrDefault("order_no");
                         instance.SignedOn = records.GetDateTimeOrDefault("SIGNED_DATE");
                         instance.ExpiresOn = records.GetDateTimeOrDefault("EXPIRY_DATE");
                     }
                    );


            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - GetSpecialOrderNo,Exception:" + ex);

            }
            return srps;
        }
        #endregion

        #region GetRouteDescription
        public static DrivingInstructionModel GetRouteDescription(int NotificationID)
        {
            DrivingInstructionModel structuresDetail = new DrivingInstructionModel();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                structuresDetail,
               UserSchema.Portal + ".GET_OUTBOUND_DRIVINGINS_DETAIL",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       instance.DrivingInstructions = records.GetByteArrayOrNull("ROUTE_DESCRIPTION");
                   }
            );
            return structuresDetail;
        }
        #endregion

        #region getRouteDetails
        public static string getRouteDetails(int NotificationID, byte[] routeDescription=null)
        {
            string messg = "OutboundDAO/getRouteDetails?NotificationID=" + NotificationID;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            string errormsg;
            string result = string.Empty;
            Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
            //For RM#4311 Change
            string[] separators = { "Split" };
            string resultPart = string.Empty;
            string FinalResult = string.Empty;
            string[] versionSplit = new string[] { };
            StringBuilder sb = new StringBuilder();
            //End
            DrivingInstructionModel DrivingInstructionInfo = new DrivingInstructionModel();
            if (routeDescription == null)
            {
                DrivingInstructionInfo = GetRouteDescription(NotificationID);
            }
            else
            {
                DrivingInstructionInfo = new DrivingInstructionModel
                {
                    DrivingInstructions = routeDescription
                };
            }
            //GetRouteDescription(NotificationID);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\\XSLT\RouteDetails.xslt");

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

            FinalResult = FinalResult.Replace("<u>", "##us##");
            FinalResult = FinalResult.Replace("</u>", "##ue##");

            FinalResult = FinalResult.Replace("<b>", "#bst#");
            FinalResult = FinalResult.Replace("</b>", "#be#");

            FinalResult = _htmlRegex.Replace(FinalResult, string.Empty);

            FinalResult = FinalResult.Replace("Start of new part", "");

            return FinalResult;
        }
        #endregion

        #region getRouteDetailsImperial
        public static string getRouteDetailsImperial(int NotificationID, byte[] routeDescription)
        {
            string messg = "OutboundDAO/getRouteDetailsImperial?NotificationID=" + NotificationID;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            string errormsg;
            string result = string.Empty;
            Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
            //For RM#4311 Change
            string[] separators = { "Split" };
            string resultPart = string.Empty;
            string FinalResult = string.Empty;
            string[] versionSplit = new string[] { };
            StringBuilder sb = new StringBuilder();
            //End

            DrivingInstructionModel DrivingInstructionInfo = new DrivingInstructionModel
            {
                DrivingInstructions = routeDescription
            };
                //GetRouteDescription(NotificationID);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\RouteDetailsImperial.xslt");

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

            FinalResult = FinalResult.Replace("<u>", "##us##");
            FinalResult = FinalResult.Replace("</u>", "##ue##");

            FinalResult = FinalResult.Replace("<b>", "#bst#");
            FinalResult = FinalResult.Replace("</b>", "#be#");

            FinalResult = _htmlRegex.Replace(FinalResult, string.Empty);

            FinalResult = FinalResult.Replace("Start of new part", "");

            return FinalResult;
        }
        #endregion

        #region GetVehicleConfigurationDetails
        public static List<VehicleConfigList> GetVehicleConfigurationDetails(int vhclID, string userSchema)
        {
            string messg = "OutboundDAO/GetVehicleConfigurationDetails?vhclID=" + vhclID + ", userSchema=" + userSchema;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            List<VehicleConfigList> listVehclRegObj = new List<VehicleConfigList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listVehclRegObj,
                userSchema + ".GET_VEHICLE_CONFIG_POSN",
                parameter =>
                {
                    parameter.AddWithValue("p_VHCL_ID", vhclID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, result) =>
                {
                    result.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                    result.ComponentId = records.GetLongOrDefault("COMPONENT_ID");
                    result.ComponentSubType = records.GetStringOrDefault("SUB_TYPE");
                    result.ComponentType = records.GetStringOrDefault("TYPE");
                    result.LatPosn = records.GetInt16OrDefault("LONG_POSN");
                    result.LongPosn = records.GetInt16OrDefault("Lat_Posn");

                    result.Length = (records["len"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("len");
                    result.Width = (records["width"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("width");

                    result.RedGroundClearance = (records["RED_Ground_Clearance"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("RED_Ground_Clearance");
                    result.GroundClearance = (records["Ground_Clearance"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("Ground_Clearance");

                    result.RedHeight = (records["Red_Height"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("Red_Height");

                    try
                    {
                        result.WheelBase = (records["wheelbase"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("wheelbase");
                    }
                    catch
                    {

                        try
                        {
                            result.WheelBase = (records["wheelbase"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("wheelbase");
                        }
                        catch (Exception ex)
                        {
                            Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
                        }
                    }
                    result.OutsideTrack = (records["Outside_Track"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("Outside_Track");

                    result.RightOverhang = (records["Right_Overhang"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("Right_Overhang");
                    result.LeftOverhang = (records["Left_Overhang"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("Left_Overhang");

                    result.FrontOverhang = (records["Front_Overhang"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("Front_Overhang");
                    result.RearOverhang = (records["Rear_Overhang"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("Rear_Overhang");

                    result.IsSteerableAtRear = (records["Is_Steerable_At_Rear"]).ToString() == string.Empty ? 0 : records.GetDecimalOrDefault("Is_Steerable_At_Rear");
                    result.SpaceToFollowing = (records["Space_To_Following"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("Space_To_Following");
                    result.VehicleDescription = records.GetStringOrDefault("Vehicle_Desc");

                    result.GrossWeight = (records["Gross_Weight"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("Gross_Weight");

                    result.RigidLength = (records["RIGID_LEN"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("RIGID_LEN");
                }
            );
            return listVehclRegObj;
        }
        #endregion

        #region GetVehicleMaxHeight
        public static List<OutBoundDoc> GetVehicleMaxHeight(long RoutePart_Id)
        {
            string messg = "OutboundDAO/GetVehicleMaxHeight?RoutePart_Id=" + RoutePart_Id;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

            List<OutBoundDoc> outBoundDoc = new List<OutBoundDoc>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                outBoundDoc,
                UserSchema.Portal + ".GET_VEHICLE_MAX_HEIGHT",
                parameter =>
                {
                    parameter.AddWithValue("p_ROUTEPART_ID", RoutePart_Id, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, result) =>
                {
                    try
                    {
                        result.MaximumHeight = (records["max_height"]).ToString() == string.Empty ? 0 : Convert.ToDouble(records.GetDecimalOrDefault("max_height"));
                    }
                    catch
                    {

                        try
                        {
                            result.MaximumHeight = (records["max_height"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("max_height");
                        }
                        catch (Exception ex)
                        {
                            Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured ,Exception:" + ex);
                        }
                    }
                }
            );
            return outBoundDoc;

        }
        #endregion

        #region getDrivingDetails
        public static NotificationXSD.DrivingInstructionsStructure getDrivingDetails(DrivingInstructionModel DrivingInstructionInfo, int incdoccaution)
        {
            string messg = "OutboundDAO/getDrivingDetails?incdoccaution=" + incdoccaution;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));


            NotificationXSD.DrivingInstructionsStructure dis = new NotificationXSD.DrivingInstructionsStructure();
            string recipientXMLInformation = string.Empty;

            Byte[] DrivingInstructionArray = DrivingInstructionInfo.DrivingInstructions;

            if (DrivingInstructionArray != null)
            {
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    recipientXMLInformation = Encoding.UTF8.GetString(DrivingInstructionArray, 0, DrivingInstructionArray.Length);

                    recipientXMLInformation = recipientXMLInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "#b#");
                    recipientXMLInformation = recipientXMLInformation.Replace("</Bold>", "#be#");

                    xmlDoc.LoadXml(recipientXMLInformation);
                }
                catch
                {
                    //Some data is stored in gzip format, so we need to unzip then load it.
                    byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(DrivingInstructionArray);

                    recipientXMLInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);

                    recipientXMLInformation = recipientXMLInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "#b#");
                    recipientXMLInformation = recipientXMLInformation.Replace("</Bold>", "#be#");

                    xmlDoc.LoadXml(recipientXMLInformation);
                }

                List<DrivingInstructionModel> drivingInsList = new List<DrivingInstructionModel>();

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

                XmlNodeList node = xmlDoc.GetElementsByTagName("DrivingInstructions");
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
                            legDetailsMessg = string.Empty;
                            LegNumber = xmlElement1["LegNumber"].InnerText;
                            legDetailsMessg = "OutboundDAO/getDrivingDetails?LegNumber=" + LegNumber;
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(legDetailsMessg + "; In DrivingInstructions Element"));

                        }
                        if (xmlElement1["Name"] != null)
                        {
                            legDetailsMessg = string.Empty;
                            Name = xmlElement1["Name"].InnerText;
                            legDetailsMessg = "OutboundDAO/getDrivingDetails?Name=" + Name;
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(legDetailsMessg + "; In DrivingInstructions Element"));
                        }
                    }
                }


                XmlNodeList parentNode = xmlDoc.GetElementsByTagName("InstructionListPosition");

                foreach (XmlElement xmlElement in parentNode)  //InstructionListPosition
                {
                    foreach (XmlElement xmlElement1 in xmlElement.ChildNodes)//Instruction
                    {
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
                                                if (xmlElement5.Name == "MotorwayCaution")
                                                {
                                                    MoterwayCaution = true;
                                                }
                                            }
                                        }
                                        if (xmlElement4.Name == "GridReference")
                                        {
                                            foreach (XmlElement xmlElement5 in xmlElement4.ChildNodes)
                                            {
                                                if (xmlElement5.Name == "X")
                                                {
                                                    X = Convert.ToUInt64(xmlElement5.InnerText);
                                                }
                                                if (xmlElement5.Name == "Y")
                                                {
                                                    Y = Convert.ToUInt64(xmlElement5.InnerText);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
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

                    DrivingInstructionInfo = new DrivingInstructionModel
                    {
                        ComparisonId = ComparisonId,
                        Id = Id,
                        LegNumber = LegNumber,
                        Name = Name,
                        Instruction = instruction,
                        MeasuredMetricDistance = MeasuredMetric,
                        DisplayMetricDistance = DisplayMetric,
                        DisplayImperialDistance = DisplayImperial,
                        PointType = PointType,
                        Description = Description,
                        GridRefX = X,
                        GridRefY = Y,
                        MotorwayCaution = MoterwayCaution
                    };

                    drivingInsList.Add(DrivingInstructionInfo);

                    Pcds = new NotificationXSD.PredefinedCautionsDescriptionsStructure1();
                    Pcds.DockCautionDescription = incdoccaution;
                    Pcds.MotorwayCautionDescription = motorwayCautionDescription;

                    instruction = string.Empty;
                    MeasuredMetric = 0;
                    DisplayMetric = 0;
                    DisplayImperial = 0;
                    PointType = string.Empty;
                    Description = string.Empty;
                    X = 0;
                    Y = 0;
                    MoterwayCaution = false;
                }




                List<NotificationXSD.DrivingInstructionsStructureSubPartListPosition> disslpList = new List<NotificationXSD.DrivingInstructionsStructureSubPartListPosition>();

                foreach (DrivingInstructionModel drivinginsstru in drivingInsList)
                {

                    NotificationXSD.DrivingInstructionsStructureSubPartListPosition disslp1 = new NotificationXSD.DrivingInstructionsStructureSubPartListPosition();

                    List<NotificationXSD.DrivingInstructionSubPartStructureAlternativeListPosition> dispsalpList = new List<NotificationXSD.DrivingInstructionSubPartStructureAlternativeListPosition>();
                    NotificationXSD.DrivingInstructionSubPartStructureAlternativeListPosition dispsalp1 = new NotificationXSD.DrivingInstructionSubPartStructureAlternativeListPosition();


                    NotificationXSD.DrivingInstructionListStructure dils = new NotificationXSD.DrivingInstructionListStructure();

                    List<NotificationXSD.DrivingInstructionListStructureInstructionListPosition> dilsilpList = new List<NotificationXSD.DrivingInstructionListStructureInstructionListPosition>();
                    NotificationXSD.DrivingInstructionListStructureInstructionListPosition dilsilp1 = new NotificationXSD.DrivingInstructionListStructureInstructionListPosition();
                    //----------------------------------------------------
                    NotificationXSD.DrivingInstructionStructure drvIns1 = new NotificationXSD.DrivingInstructionStructure();

                    NotificationXSD.NavigationInstructionStructure nis1 = new NotificationXSD.NavigationInstructionStructure();
                    NotificationXSD.SimpleTextStructure sts1 = new NotificationXSD.SimpleTextStructure();
                    string[] str1 = new string[1];
                    str1[0] = drivinginsstru.Instruction;
                    sts1.Text = str1;
                    nis1.Instruction = sts1;

                    NotificationXSD.DrivingInstructionDistanceStructure dids = new NotificationXSD.DrivingInstructionDistanceStructure();
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

                    List<NotificationXSD.DrivingInstructionStructureNoteListPosition> disnlpList = new List<NotificationXSD.DrivingInstructionStructureNoteListPosition>();
                    NotificationXSD.DrivingInstructionStructureNoteListPosition disnlp1 = new NotificationXSD.DrivingInstructionStructureNoteListPosition();

                    NotificationXSD.DrivingInstructionNoteStructure dins1 = new NotificationXSD.DrivingInstructionNoteStructure();
                    NotificationXSD.NoteChoiceStructure ncs1 = new NotificationXSD.NoteChoiceStructure();

                    if (drivinginsstru.MotorwayCaution)
                    {

                        object motorWayCaution;

                        motorWayCaution = "Apply motorway caution";

                        ncs1.Item = motorWayCaution;

                        dins1.Content = ncs1;
                    }
                    else
                    {
                        NotificationXSD.RoutePointDescriptionStructure rps1 = new NotificationXSD.RoutePointDescriptionStructure();
                        if (drivinginsstru.PointType == "start")
                        {
                            rps1.PointType = NotificationXSD.RoutePointType.start;
                        }
                        if (drivinginsstru.PointType == "end")
                        {
                            rps1.PointType = NotificationXSD.RoutePointType.end;
                        }
                        if (drivinginsstru.PointType == "way")
                        {
                            rps1.PointType = NotificationXSD.RoutePointType.way;
                        }
                        if (drivinginsstru.PointType == "intermediate")
                        {
                            rps1.PointType = NotificationXSD.RoutePointType.intermediate;
                        }
                        rps1.Description = drivinginsstru.Description;
                        ncs1.Item = rps1;

                        if (rps1.Description != string.Empty)
                        {
                            dins1.Content = ncs1;
                        }
                    }



                    NotificationXSD.GridReferenceStructure grs1 = new NotificationXSD.GridReferenceStructure();
                    grs1.X = drivinginsstru.GridRefX;
                    grs1.Y = drivinginsstru.GridRefY;
                    if (grs1.X != 0 || grs1.Y != 0)
                    {
                        dins1.GridReference = grs1;
                    }

                    if (dins1.Content != null || dins1.GridReference != null)
                    {
                        disnlp1.Note = dins1;
                    }

                    disnlpList.Add(disnlp1);
                    drvIns1.NoteListPosition = disnlpList.ToArray();
                    dilsilp1.Instruction = drvIns1;
                    dilsilpList.Add(dilsilp1);
                    //---------------------------------------------------------
                    dils.InstructionListPosition = dilsilpList.ToArray();

                    dispsalp1.Alternative = dils;
                    dispsalpList.Add(dispsalp1);
                    disslp1.SubPart = dispsalpList.ToArray();

                    disslpList.Add(disslp1);

                    dis.ComparisonId = drivinginsstru.ComparisonId;
                    dis.Id = drivinginsstru.Id;
                    dis.LegNumber = drivinginsstru.LegNumber;
                    dis.Name = drivinginsstru.Name;
                }

                dis.SubPartListPosition = disslpList.ToArray();
            }

            return dis;
        }
        #endregion

        #region Below function is for NEN
        public static DrivingInstructionModel GetNENRouteDescription(long NENInboxId, int OrganisationId)
        {
            DrivingInstructionModel structuresDetail = new DrivingInstructionModel();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                structuresDetail,
                UserSchema.Portal + ".STP_NEN_NOTIFICATION.GET_NENOUTBOUND_DRIVINGINS_DET",
                parameter =>
                {
                    parameter.AddWithValue("P_INBOX_ID", NENInboxId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", OrganisationId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       instance.DrivingInstructions = records.GetByteArrayOrNull("ROUTE_DESCRIPTION");
                       instance.AffectedStructures = records.GetByteArrayOrNull("AFFECTED_STRUCTURES");
                   }
            );
            return structuresDetail;
        }
        #endregion

        #region NullableOrNotForDecimal
        public static decimal? NullableOrNotForDecimal(double? data)
        {
            if (data == null || data == 0)
            {
                return null;
            }
            else
            {
                return Convert.ToDecimal(data);
            }
        }
        #endregion

        #region GetStructureGeneralDetailListbyMultipleESRN
        public static List<AffStructureGeneralDetails> GetStructureGeneralDetailListbyMultipleESRN(string structureCode,int unSuitableStructuresCount)
        {
            List<AffStructureGeneralDetails> objstructures = new List<AffStructureGeneralDetails>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               objstructures,
                   UserSchema.Portal + ".SP_AFFECTED_STRUCT_DETAILS_BY_MULTIPLE_ESRN",
               parameter =>
               {
                   parameter.AddWithValue("P_STRUCT_CODE", structureCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_struct_count", unSuitableStructuresCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                   (records, instance) =>
                   {
                       try
                       {
                           instance.StructureChainNo = records.GetLongOrDefault("CHAIN_NO");
                           instance.StructurePosition = records.GetInt16OrDefault("POSITION");
                           instance.ESRN = records.GetStringOrDefault("STRUCTURE_CODE");
                           instance.StructureDescription = records.GetStringOrDefault("DESCRIPTION");
                           instance.StructureClass = records.GetStringOrDefault("STRUCTURE_CLASS");
                           instance.OSGR = records.GetLongOrDefault("NORTHING");
                           instance.Easting = records.GetLongOrDefault("EASTING");
                           instance.StructureType = records.GetStringOrDefault("STRUCT_TYPE");
                           instance.StructureType1 = records.GetStringOrDefault("STRUCT_TYPE1");
                           instance.StructureType2 = records.GetStringOrDefault("STRUCT_TYPE2");
                           instance.OwnerName = records.GetStringOrDefault("OWNER");
                           instance.AlternativeName = records.GetStringOrDefault("ALTERNATIVENAME");
                           instance.Notes = records.GetStringOrDefault("NOTES");
                           instance.ContactId = Convert.ToInt64(records["CONTACT_ID"]); //this function will convert the record object to Int64 #7197
                           instance.StructureId = records.GetLongOrDefault("STRUCTURE_ID");
                           instance.StructureName = records.GetStringOrDefault("STRUCTURE_NAME");
                           instance.StructureKey = records.GetStringOrDefault("STRUCT_KEY");
                           instance.OrganisationId = records.GetLongOrDefault("ORGANISATION_ID");
                           instance.StructureCategory = records.GetStringOrDefault("STRUCT_CATEGORY");
                           instance.StructureLength = records.GetFieldType("STRUCTURE_LENGTH").Name=="Decimal"? Convert.ToDouble(records.GetDecimalOrDefault("STRUCTURE_LENGTH"))
                                    : records.GetDoubleOrDefault("STRUCTURE_LENGTH");
                       }
                       catch(Exception ex)
                       {

                       }
                   }
              );
            return objstructures;
        }
        #endregion

        #region ViewUnsuitableStructSections
        public static List<StructureSectionList> ViewUnsuitableStructSections(long structureId, long route_part_id, long section_id)
        {
            List<StructureSectionList> structureSectionsObj = new List<StructureSectionList>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
          structureSectionsObj,
         UserSchema.Portal + ".SP_UNSUITABLE_SECTION",
          parameter =>
          {
              parameter.AddWithValue("P_STRUCT_ID", structureId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
              parameter.AddWithValue("P_SECT_ID", section_id, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
              parameter.AddWithValue("P_ROUTE_ID", route_part_id, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
              parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
          },
              (records, instance) =>
              {
                  instance.SectionId = records.GetLongOrDefault("SECTION_ID");
                  instance.StructureSections = records.GetStringOrDefault("STRUCTURE_SECTION");
                  instance.AffectFlag = (int)records.GetDecimalOrDefault("AFFECT_FLAG");
              }
           );

            return structureSectionsObj;

        }
        #endregion

        #region GetNotificationDispensation
        public static List<NotifDispensations> GetNotificationDispensation(long notificationId, int historic)
        {
            List<NotifDispensations> notifDispensationList = new List<NotifDispensations>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                notifDispensationList,
                UserSchema.Portal + ".SP_GET_NOTIF_DIPENSATION_DATA",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIFICATION_ID", notificationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HISTORIC", historic, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.DispensationId = (int)records.GetLongOrDefault("dispensation_id");
                        instance.Summary = records.GetStringOrDefault("SHORT_SUMMARY");
                        instance.DRN = records.GetStringOrDefault("DISPENSATION_REF");
                        instance.GrantorName = records.GetStringOrDefault("GRANTOR_NAME");
                        instance.GrantorId = (int)records.GetLongOrDefault("GRANTOR_ID");
                    }
            );
            return notifDispensationList;
        }
        #endregion
    }
}