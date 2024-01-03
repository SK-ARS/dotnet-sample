using Oracle.DataAccess.Client;
using SpecialOrderXSD;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.DocumentsAndContents.Common;
using STP.Domain;
using STP.Domain.Applications;
using STP.Domain.DocumentsAndContents;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace STP.DocumentsAndContents.Persistance
{
    public static class SpecialOrderDAO
    {

        /// <summary>
        /// for get ordinal detail for day from the datetime
        /// </summary>
        /// <param name="num">day</param>
        /// <returns></returns>
        public static string AddOrdinal(int num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }

        }
        public static SORTSpecialOrder GetSORTSpecialOrder(string specialOrderId)
        {
            SORTSpecialOrder sporder = new SORTSpecialOrder();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                        sporder,
                        UserSchema.Sort + ".SP_SORT_SPEC_ORDER",
                        parameter =>
                        {
                            parameter.AddWithValue("P_ORDER_NO", specialOrderId, OracleDbType.NVarchar2, ParameterDirectionWrap.Input);
                            parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                        },
                         record =>
                         {
                             sporder.SONumber = record.GetStringOrDefault("ORDER_NO");
                             sporder.Signatory = record.GetStringOrDefault("SIGNATORY");
                             sporder.ExpiryDate = record.GetDateTimeOrDefault("EXPIRY_DATE").ToString("dd/MM/yyyy");
                             sporder.SignatoryRole = record.GetStringOrDefault("SIGNATORY_ROLE");
                             sporder.SOCreateDate = record.GetDateTimeOrDefault("CREATED_DATE").ToString("dd/MM/yyyy");
                             sporder.Template = record.GetStringOrDefault("TEMPLATE");
                             sporder.State = record.GetStringOrDefault("STATE");
                             sporder.SignDate = record.GetDateTimeOrDefault("SIGNED_DATE").ToString("dd/MM/yyyy");
                             sporder.Applicability = record.GetStringOrDefault("APPLICABILITY").ToString();
                             sporder.ESDALNo = record.GetStringOrDefault("ESDAL_REF").ToString();
                         }

                    );
            return sporder;
        }

        //Special order coverage grid

        public static List<SOCRouteParts> GetRouteVehicles(int movementVersionId,string vehicleId)
        {
            List<SOCRouteParts> lstsoroutparts = new List<SOCRouteParts>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                lstsoroutparts,
                UserSchema.Sort + ".SP_GET_SOROUTES_VEHICLES",
                parameter =>
                {
                    parameter.AddWithValue("P_VERSION_ID", movementVersionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VEHICLE_ID", vehicleId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.RoutePartId = records.GetLongOrDefault("ROUTE_PART_ID");
                        instance.RPName = records.GetStringOrDefault("PART_NAME");
                        instance.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                        instance.RVName = records.GetStringOrDefault("VEHICLE_DESC");
                        instance.RecomTemplate = records.GetStringOrDefault("RECOM_TEMPLATE");
                    }
          );
            return lstsoroutparts;
        }

        public static List<SOCoverageDetails> GetSpecialOrderCoverages(int projectid, int state)
        {
            List<SOCoverageDetails> lstcoverages = new List<SOCoverageDetails>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                lstcoverages,
                UserSchema.Sort + ".SP_GET_SO_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_PROJECT_ID", projectid, OracleDbType.Int32, ParameterDirectionWrap.Input);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.OrderNo = records.GetStringOrDefault("ORDER_NO");
                        instance.Applicability = records.GetStringOrDefault("APPLICABILITY");
                    }
          );
            return lstcoverages;
        }

        //Delete special order
        internal static int DeleteSpecialOrder(string specialOrder, string userSchema = UserSchema.Sort)
        {
            int result=0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                userSchema + ".SP_DELETE_SPECIAL_ORDER",
                parameter =>
                {
                    parameter.AddWithValue("SO_ORDER_NO", specialOrder, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        result = records.GetInt32("p_AFFECTED_ROWS");
                    }
          );
            return result;
        }

        //Save new special order
        public static string SaveNewSortSpecialOrder(SORTSpecialOrder model, List<string> removedCovrg)
        {
            DateTime signdate = DateTime.ParseExact(model.SignDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime expdate = DateTime.ParseExact(model.ExpiryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime SOCreateDate = DateTime.ParseExact(model.SOCreateDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //DateTime signdate = DateTime.Parse(model.SignDate);
            //DateTime expdate = DateTime.Parse(model.ExpiryDate);
            int state = int.Parse(model.State);
            int template = int.Parse(model.Template);

            string soNumber = "";
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                      model,
                      UserSchema.Sort + ".SP_INSERT_SORT_SPECIAL_ORDER",
                      parameter =>
                      {
                          parameter.AddWithValue("SO_ESDAL_REF", model.ESDALNo, OracleDbType.NVarchar2, ParameterDirectionWrap.Input);
                          parameter.AddWithValue("SO_STATE", state, OracleDbType.Int32, ParameterDirectionWrap.Input);
                          parameter.AddWithValue("SO_CREATED_DATE", SOCreateDate, OracleDbType.Date, ParameterDirectionWrap.Input);
                          parameter.AddWithValue("SO_SIGNED_DATE", signdate, OracleDbType.Date, ParameterDirectionWrap.Input);
                          parameter.AddWithValue("SO_EXPIRY_DATE", expdate, OracleDbType.Date, ParameterDirectionWrap.Input);
                          parameter.AddWithValue("SO_SIGNATORY", model.Signatory, OracleDbType.NVarchar2, ParameterDirectionWrap.Input);
                          parameter.AddWithValue("SO_SIGNATORY_ROLE", model.SignatoryRole, OracleDbType.NVarchar2, ParameterDirectionWrap.Input);
                          parameter.AddWithValue("SO_TEMPLATE", template, OracleDbType.Int32, ParameterDirectionWrap.Input);
                          parameter.AddWithValue("SO_YEAR", model.Year, OracleDbType.Int32, ParameterDirectionWrap.Input);
                          parameter.AddWithValue("SO_PROJECT_ID", model.ProjectID, OracleDbType.Int32, ParameterDirectionWrap.Input);
                          parameter.AddWithValue("SO_VERSION_ID", model.VersionID, OracleDbType.Int32, ParameterDirectionWrap.Input);
                          parameter.AddWithValue("SO_APPLICABILITY", model.Applicability, OracleDbType.NVarchar2, ParameterDirectionWrap.Input);
                          parameter.AddWithValue("SO_COVARGEGRID", model.CoverageStatus, OracleDbType.Int32, ParameterDirectionWrap.Input);
                          parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                      },
                      (record, instance) =>
                      {
                          soNumber = record.GetStringOrDefault("ORDER_NO");
                      }
                 );
            //Update the Removed Applicabilities.

            UpdateSOApplicability(removedCovrg);
            return soNumber;
        }
        public static void UpdateSOApplicability(List<string> removedCovrg)
        {
            if (removedCovrg.Count != 0)
            {
                foreach (string coverage in removedCovrg)
                {
                    if (coverage != "")
                    {
                        string[] covrgapplicability = coverage.Split('#');
                        if (covrgapplicability.Count() > 1)
                        {
                            string applicability = covrgapplicability[0].ToString();
                            string specialorderno = covrgapplicability[1].ToString();
                            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                                      removedCovrg,
                                      UserSchema.Sort + ".SP_SO_UPDATE_APPLICABILITY",
                                      parameter =>
                                      {
                                          parameter.AddWithValue("SO_ORDER_NO", specialorderno, OracleDbType.NVarchar2, ParameterDirectionWrap.Input);
                                          parameter.AddWithValue("SO_APPLICABILITY", applicability, OracleDbType.NVarchar2, ParameterDirectionWrap.Input);
                                      },
                                      (record, instance) =>
                                      {
                                      }
                                 );
                        }
                    }
                }
            }
        }

        //Update special order
        public static string UpdateSortSpecialOrder(SORTSpecialOrder model, List<string> removedCovrg)
        {
            model.SignDate = model.SignDate.Replace('/', '-');
            model.ExpiryDate = model.ExpiryDate.Replace('/', '-');
            model.SOCreateDate = model.SOCreateDate.Replace('/', '-');
            DateTime signdate = DateTime.ParseExact(model.SignDate,"dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime expdate = DateTime.ParseExact(model.ExpiryDate,"dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime SOCreateDate = DateTime.ParseExact(model.SOCreateDate,"dd-MM-yyyy", CultureInfo.InvariantCulture);
            int state = int.Parse(model.State);
            int template = int.Parse(model.Template);

            string soNumber = "";
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                      model,
                      UserSchema.Sort + ".SP_UPDATE_SORT_SPECIAL_ORDER",
                      parameter =>
                      {
                          parameter.AddWithValue("SO_ORDER_NO", model.SONumber, OracleDbType.NVarchar2, ParameterDirectionWrap.Input);
                          parameter.AddWithValue("SO_CREATED_DATE", SOCreateDate, OracleDbType.Date, ParameterDirectionWrap.Input);
                          parameter.AddWithValue("SO_SIGNED_DATE", signdate, OracleDbType.Date, ParameterDirectionWrap.Input);
                          parameter.AddWithValue("SO_EXPIRY_DATE", expdate, OracleDbType.Date, ParameterDirectionWrap.Input);
                          parameter.AddWithValue("SO_SIGNATORY", model.Signatory, OracleDbType.NVarchar2, ParameterDirectionWrap.Input);
                          parameter.AddWithValue("SO_SIGNATORY_ROLE", model.SignatoryRole, OracleDbType.NVarchar2, ParameterDirectionWrap.Input);
                          parameter.AddWithValue("SO_STATE", state, OracleDbType.Int32, ParameterDirectionWrap.Input);
                          parameter.AddWithValue("SO_TEMPLATE", template, OracleDbType.Int32, ParameterDirectionWrap.Input);
                          parameter.AddWithValue("SO_APPLICABILITY", model.Applicability, OracleDbType.NVarchar2, ParameterDirectionWrap.Input);
                          parameter.AddWithValue("SO_COVARGEGRID", model.CoverageStatus, OracleDbType.Int32, ParameterDirectionWrap.Input);
                      },
                      (record, instance) =>
                      {
                          soNumber = record.GetStringOrDefault("ORDER_NO");
                      }
                 );
            //Update the Removed Applicabilities.
            UpdateSOApplicability(removedCovrg);
            return soNumber;
        }

        /// <summary>
        /// get special order detail
        /// </summary>
        /// <param name="orderNumber">order number</param>
        /// <param name="esDAlRefNo">esdal ref number</param>
        /// <param name="soTemplateType">so template type</param>
        /// <param name="userSchema">user schema</param>
        /// <returns></returns>
        public static SpecialOrderStructure GetSpecialOrderDetails(string orderNumber, string esDAlRefNo, Enums.SOTemplateType soTemplateType, string userSchema)
        {
            SpecialOrderStructure sos = new SpecialOrderStructure();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                sos,
                userSchema + ".GET_SPECIALORDER_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("p_ORDER_NUMBER", orderNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ESDAL_REF_NUMBER", esDAlRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       orderNumber = records.GetStringOrDefault("ORDER_NO");
                       if (soTemplateType == Enums.SOTemplateType.SO2D1)
                           instance.OrderTemplate = SpecialOrderXSD.SpecialOrderTemplateType.Item2d1;
                       if (soTemplateType == Enums.SOTemplateType.SO2D4)
                           instance.OrderTemplate = SpecialOrderXSD.SpecialOrderTemplateType.Item2d4;
                       if (soTemplateType == Enums.SOTemplateType.SO2D4a)
                           instance.OrderTemplate = SpecialOrderXSD.SpecialOrderTemplateType.Item2d4;
                       if (soTemplateType == Enums.SOTemplateType.SO2D4b)
                           instance.OrderTemplate = SpecialOrderXSD.SpecialOrderTemplateType.Item2d4;
                       if (soTemplateType == Enums.SOTemplateType.SO2D5)
                           instance.OrderTemplate = SpecialOrderXSD.SpecialOrderTemplateType.Item2d5;
                       if (soTemplateType == Enums.SOTemplateType.SO2D7)
                           instance.OrderTemplate = SpecialOrderXSD.SpecialOrderTemplateType.Item2d7;
                       if (soTemplateType == Enums.SOTemplateType.SO2D7a)
                           instance.OrderTemplate = SpecialOrderXSD.SpecialOrderTemplateType.Item2d7;
                       if (soTemplateType == Enums.SOTemplateType.SO2D8)
                           instance.OrderTemplate = SpecialOrderXSD.SpecialOrderTemplateType.Item2d8;
                       if (soTemplateType == Enums.SOTemplateType.SO2D9)
                           instance.OrderTemplate = SpecialOrderXSD.SpecialOrderTemplateType.Item2d9;

                       instance.OrderNumber = orderNumber;

                           #region ESDALRefrenceNumber
                           SpecialOrderXSD.ESDALReferenceNumberStructure esdalrefnostru = new SpecialOrderXSD.ESDALReferenceNumberStructure();
                       esdalrefnostru.Mnemonic = records.GetStringOrDefault("HAULIER_MNEMONIC");
                       string[] hauMnemonicArr = records.GetStringOrDefault("ESDAL_REF").Split("/".ToCharArray());
                       if (hauMnemonicArr.Length > 0)
                       {
                           esdalrefnostru.MovementProjectNumber = hauMnemonicArr[1];
                       }

                       SpecialOrderXSD.MovementVersionNumberStructure movvernostru = new SpecialOrderXSD.MovementVersionNumberStructure();
                       movvernostru.Value = records.GetShortOrDefault("VERSION_NO");
                       esdalrefnostru.MovementVersion = movvernostru;
                       if (records.GetInt16OrDefault("ENTERED_BY_SORT") == 1)
                       {
                           esdalrefnostru.EnteredBySORT = true;
                           esdalrefnostru.EnteredBySORTSpecified = true;
                       }
                       else
                       {
                           esdalrefnostru.EnteredBySORT = false;
                           esdalrefnostru.EnteredBySORTSpecified = false;
                       }
                       instance.ESDALReferenceNumber = esdalrefnostru;
                           #endregion
                           instance.JobFileReferenceNumber = records.GetStringOrDefault("HA_JOB_FILE_REF");

                       instance.Load = records.GetStringOrDefault("LOAD_DESCR");

                       instance.Wide = null;
                       instance.RigidLength = Convert.ToDecimal(records.GetFloatOrDefault("RIGID_LEN"));
                       if (Convert.ToDouble(instance.RigidLength) >= 30)
                       {
                           instance.NonSTGOLength = true;
                       }
                       else
                       {
                           instance.NonSTGOLength = false;
                       }
                       instance.Width = Convert.ToDecimal(records.GetFloatOrDefault("WIDTH"));
                       if (Convert.ToDouble(instance.NonSTGOWidth) >= 6.1)
                       {
                           instance.NonSTGOWidth = true;
                       }
                       else
                       {
                           instance.NonSTGOWidth = false;
                       }
                       instance.MinAxles = Convert.ToString(records.GetInt16OrDefault("Axle_count"));
                       instance.HaulierName = records.GetStringOrDefault("HAULIER_NAME");

                       #region HaulierAddress
                       SpecialOrderXSD.AddressStructure has = new SpecialOrderXSD.AddressStructure();
                       string[] Addstru = new string[5];
                       Addstru[0] = records.GetStringOrDefault("HAULIER_ADDRESS_1");
                       Addstru[1] = records.GetStringOrDefault("HAULIER_ADDRESS_2");
                       Addstru[2] = records.GetStringOrDefault("HAULIER_ADDRESS_3");
                       Addstru[3] = records.GetStringOrDefault("HAULIER_ADDRESS_4");
                       Addstru[4] = records.GetStringOrDefault("HAULIER_ADDRESS_5");
                       has.Line = Addstru;
                       has.PostCode = records.GetStringOrDefault("haulier_post_code");

                       int country = records.GetInt32OrDefault("haulier_country");
                       if (country == (int)Country.england)
                       {
                           has.Country = SpecialOrderXSD.CountryType.england;
                           has.CountrySpecified = true;
                       }
                       else if (country == (int)Country.northernireland)
                       {
                           has.Country = SpecialOrderXSD.CountryType.northernireland;
                           has.CountrySpecified = true;
                       }
                       else if (country == (int)Country.scotland)
                       {
                           has.Country = SpecialOrderXSD.CountryType.scotland;
                           has.CountrySpecified = true;
                       }
                       else if (country == (int)Country.wales)
                       {
                           has.Country = SpecialOrderXSD.CountryType.wales;
                           has.CountrySpecified = true;
                       }

                       instance.HaulierAddress = has;
                           #endregion

                           instance.NoOfJourneys = records.GetShortOrDefault("TOTAL_MOVES");
                       instance.NoOfJourneysInWord = CommonMethods.NumberToWords(records.GetShortOrDefault("TOTAL_MOVES"));

                           #region SigningDetails
                           SigningDetailsStructure sosigndetail = new SigningDetailsStructure();

                       sosigndetail.SigningDate = records.GetDateTimeOrDefault("SIGNED_DATE");

                       int dayNo = sosigndetail.SigningDate.Day;

                       sosigndetail.CustomSigningDate = string.Format("{0} {1} {2}",
                                   AddOrdinal(dayNo), sosigndetail.SigningDate.ToString("MMMM"), sosigndetail.SigningDate.Year);

                       sosigndetail.Signatory = records.GetStringOrDefault("SIGNATORY");
                       sosigndetail.SignatoryRole = records.GetStringOrDefault("SIGNATORY_ROLE");

                       instance.SigningDetails = sosigndetail;
                           #endregion

                           instance.ExpiryDate = records.GetDateTimeOrDefault("EXPIRY_DATE");

                       dayNo = instance.ExpiryDate.Day;

                       instance.CustomFormatExpiryDate = string.Format("{0} {1} {2}",
                                   AddOrdinal(dayNo), instance.ExpiryDate.ToString("MMMM"), instance.ExpiryDate.Year);

                       SpecialOrderXSD.SimplifiedRoutePointStructure srps;

                       instance.StartLocation = records.GetStringOrDefault("FROM_DESCR");

                       instance.EndLocation = records.GetStringOrDefault("TO_DESCR");

                       srps = GetSimplifiedRoutePointVia(orderNumber, esDAlRefNo, userSchema);

                       instance.ViaLocation = srps.Description;

                       long analysisID = records.GetLongOrDefault("ANALYSIS_ID");

                       ContactModel contactDetail = ProposalDAO.GetRecipientDetail(analysisID, userSchema);

                       string recipientXMLInformation = string.Empty;

                       Byte[] affectedPartiesArray = contactDetail.AffectedParties;

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

                       string organisationID = string.Empty;

                       XmlNodeList parentNode = null;
                       if (xmlDoc.GetElementsByTagName("AffectedParty").Item(0) == null)
                       {
                           parentNode = xmlDoc.GetElementsByTagName("movement:AffectedParty");
                       }
                       else
                       {
                           parentNode = xmlDoc.GetElementsByTagName("AffectedParty");
                       }

                       foreach (XmlElement childrenNode in parentNode)
                       {
                           foreach (XmlElement xmlElement in childrenNode)
                           {
                               foreach (XmlElement xmlElement1 in xmlElement.ChildNodes)
                               {
                                   foreach (XmlNode childNode in xmlElement1)
                                   {
                                       if (childNode.Name == "SimpleReference")
                                       {

                                           int orgId = 0;

                                           XmlElement simpleReference = childNode as XmlElement;

                                           if ((simpleReference != null) && simpleReference.HasAttribute("OrganisationId"))
                                           {
                                               orgId = Convert.ToInt32(childNode.Attributes["OrganisationId"].InnerText);
                                           }

                                           organisationID += orgId + ",";
                                       }
                                   }
                               }
                           }
                       }

                       if (organisationID != string.Empty)
                       {
                           organisationID = organisationID.Remove(organisationID.LastIndexOf(","));
                       }

                       List<MovementExclusionStructure> movementexclustruList = GetBankHolidayExclusion(sosigndetail.SigningDate, instance.ExpiryDate, userSchema, organisationID);

                       instance.BankHolidayExclusion = ArrangeBankHolidayExclusionStartEndDate(movementexclustruList).ToArray();

                       List<MovementExclusionStructure> summerexclustruList = GetSummerExclusion(sosigndetail.SigningDate, instance.ExpiryDate, userSchema);

                       instance.SummerExclusion = ArrangeBankHolidayExclusionStartEndDate(summerexclustruList).ToArray();

                       List<VehicleScheduleStructure> vsslist = GetVehicleScheduleStructure(orderNumber, esDAlRefNo, soTemplateType, userSchema);
                       instance.VehiclesSchedule = vsslist.ToArray();

                       foreach (VehicleScheduleStructure vss in vsslist)
                       {
                           if (vss.GrossWeight != string.Empty)
                           {
                               if (Convert.ToDouble(vss.GrossWeight) >= 150.0)
                                   instance.NonSTGOWeight = true;
                               else
                                   instance.NonSTGOWeight = false;
                           }
                       }
                   }
            );

            return sos;
        }

        /// <summary>
        /// get journey via location for special order document
        /// </summary>
        /// <param name="orderNumber">order number</param>
        /// <param name="esDAlRefNo">esdal ref no</param>
        /// <param name="userSchema">user schema</param>
        /// <returns></returns>
        public static SpecialOrderXSD.SimplifiedRoutePointStructure GetSimplifiedRoutePointVia(string orderNumber, string esDAlRefNo, string userSchema)
        {
            SpecialOrderXSD.SimplifiedRoutePointStructure srps = new SpecialOrderXSD.SimplifiedRoutePointStructure();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                srps,
                userSchema + ".GET_VIAPOINT",
                parameter =>
                {
                    parameter.AddWithValue("p_ORDER_NUMBER", orderNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ESDAL_REF_NUMBER", esDAlRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
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
        /// get bank holiday detail for specifid duration
        /// </summary>
        /// <param name="startdate">start date</param>
        /// <param name="enddate">end date</param>
        /// <param name="userSchema">user schema</param>
        /// <returns></returns>
        public static List<MovementExclusionStructure> GetBankHolidayExclusion(DateTime startdate, DateTime enddate, string userSchema, string organisationId)
        {
            List<MovementExclusionStructure> bankholidayexclusionList = new List<MovementExclusionStructure>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                bankholidayexclusionList,
                userSchema + ".GET_HOLIDAY_DETAIL",
                parameter =>
                {
                    parameter.AddWithValue("p_START_DATE", startdate.ToString("dd-MMMM-yy"), OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_END_DATE", enddate.ToString("dd-MMMM-yy"), OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGANISATION_ID", organisationId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 (records, instance) =>
                 {
                     TimeDayDateStructure start = new TimeDayDateStructure();
                     TimeDayDateStructure end = new TimeDayDateStructure();

                     DateTime dt = records.GetDateTimeOrDefault("WHEN");

                     start.Date = dt;
                     start.Time = TimeOfDayType.noon;
                     start.TimeSpecified = true;
                     instance.Start = start; if (dt.ToString("dddd") == "Monday")

                         if (dt.ToString("dddd") == "Monday")
                             start.Day = DayOfWeekType.monday;
                     if (dt.ToString("dddd") == "Tuesday")
                         start.Day = DayOfWeekType.tuesday;
                     if (dt.ToString("dddd") == "Wednesday")
                         start.Day = DayOfWeekType.wednesday;
                     if (dt.ToString("dddd") == "Thursday")
                         start.Day = DayOfWeekType.thursday;
                     if (dt.ToString("dddd") == "Friday")
                         start.Day = DayOfWeekType.friday;
                     if (dt.ToString("dddd") == "Saturday")
                         start.Day = DayOfWeekType.saturday;
                     if (dt.ToString("dddd") == "Sunday")
                         start.Day = DayOfWeekType.sunday;


                     end.Day = DayOfWeekType.monday;
                     if (dt.ToString("dddd") == "Tuesday")
                         end.Day = DayOfWeekType.tuesday;
                     if (dt.ToString("dddd") == "Wednesday")
                         end.Day = DayOfWeekType.wednesday;
                     if (dt.ToString("dddd") == "Thursday")
                         end.Day = DayOfWeekType.thursday;
                     if (dt.ToString("dddd") == "Friday")
                         end.Day = DayOfWeekType.friday;
                     if (dt.ToString("dddd") == "Saturday")
                         end.Day = DayOfWeekType.saturday;
                     if (dt.ToString("dddd") == "Sunday")
                         end.Day = DayOfWeekType.sunday;
                     end.Date = dt;
                     end.Time = TimeOfDayType.noon;
                     end.TimeSpecified = true;

                     instance.End = end;
                 }
                );

            return bankholidayexclusionList;
        }

        /// <summary>
        /// arrange holiday in sequence if holiday comes in sequence 
        /// </summary>
        /// <param name="holidayexclusionList">MovementExclusionStructure</param>
        /// <returns></returns>
        public static List<MovementExclusionStructure> ArrangeBankHolidayExclusionStartEndDate(List<MovementExclusionStructure> holidayexclusionList)
        {
            List<MovementExclusionStructure> updatedmovementexclustruList = new List<MovementExclusionStructure>();

            foreach (MovementExclusionStructure day in holidayexclusionList)
            {
                MovementExclusionStructure newstru = new MovementExclusionStructure();
                newstru.Start = day.Start;
                newstru.End = day.End;

                bool status = false;
                foreach (MovementExclusionStructure updateddays in updatedmovementexclustruList)
                {
                    if (updateddays.Start.Date <= day.Start.Date && updateddays.End.Date >= day.Start.Date)
                    {
                        status = true;
                        break;
                    }
                }
                if (status)
                    continue;

                DateTime dt = day.Start.Date.AddDays(1);

            outer:
                int count = 0;
                foreach (MovementExclusionStructure days in holidayexclusionList)
                {

                    if (dt == days.Start.Date)
                    {
                        count += 1;
                        if (count == 1)
                        {
                            dt = dt.AddDays(1);
                            goto outer;
                        }
                    }
                }

                dt = dt.AddDays(-1);

                newstru.End.Date = dt;

                newstru.Start.Date = newstru.Start.Date.AddDays(-1);

                dt = newstru.Start.Date;

                if (dt.ToString("dddd") == "Monday")
                    newstru.Start.Day = DayOfWeekType.monday;
                if (dt.ToString("dddd") == "Tuesday")
                    newstru.Start.Day = DayOfWeekType.tuesday;
                if (dt.ToString("dddd") == "Wednesday")
                    newstru.Start.Day = DayOfWeekType.wednesday;
                if (dt.ToString("dddd") == "Thursday")
                    newstru.Start.Day = DayOfWeekType.thursday;
                if (dt.ToString("dddd") == "Friday")
                    newstru.Start.Day = DayOfWeekType.friday;
                if (dt.ToString("dddd") == "Saturday")
                    newstru.Start.Day = DayOfWeekType.saturday;
                if (dt.ToString("dddd") == "Sunday")
                    newstru.Start.Day = DayOfWeekType.sunday;

                newstru.End.Date = newstru.End.Date.AddDays(1);

                dt = newstru.End.Date;

                if (dt.ToString("dddd") == "Monday")
                    newstru.End.Day = DayOfWeekType.monday;
                if (dt.ToString("dddd") == "Tuesday")
                    newstru.End.Day = DayOfWeekType.tuesday;
                if (dt.ToString("dddd") == "Wednesday")
                    newstru.End.Day = DayOfWeekType.wednesday;
                if (dt.ToString("dddd") == "Thursday")
                    newstru.End.Day = DayOfWeekType.thursday;
                if (dt.ToString("dddd") == "Friday")
                    newstru.End.Day = DayOfWeekType.friday;
                if (dt.ToString("dddd") == "Saturday")
                    newstru.End.Day = DayOfWeekType.saturday;
                if (dt.ToString("dddd") == "Sunday")
                    newstru.End.Day = DayOfWeekType.sunday;

                newstru.End.Time = TimeOfDayType.noon;

                int dayNo = newstru.Start.Date.Day;

                newstru.Start.CustomStartDate = string.Format("{0} {1} {2}",
                                       AddOrdinal(dayNo), newstru.Start.Date.ToString("MMMM"), newstru.Start.Date.Year);

                dayNo = newstru.End.Date.Day;

                newstru.End.CustomEndDate = string.Format("{0} {1} {2}",
                                       AddOrdinal(dayNo), newstru.End.Date.ToString("MMMM"), newstru.End.Date.Year);

                updatedmovementexclustruList.Add(newstru);
            }
            return updatedmovementexclustruList;
        }

        /// <summary>
        /// get summer holiday detail for specifid duration
        /// </summary>
        /// <param name="startdate">start date</param>
        /// <param name="enddate">end date</param>
        /// <param name="userSchema">user schema</param>
        /// <returns></returns>
        public static List<MovementExclusionStructure> GetSummerExclusion(DateTime startdate, DateTime enddate, string userSchema)
        {
            List<MovementExclusionStructure> summerexclusionList = new List<MovementExclusionStructure>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                summerexclusionList,
                userSchema + ".GET_SUMMER_HOLIDAY_DETAIL",
                parameter =>
                {
                    parameter.AddWithValue("p_START_DATE", startdate.ToString("dd-MMMM-yy"), OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_END_DATE", enddate.ToString("dd-MMMM-yy"), OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 (records, instance) =>
                 {
                     TimeDayDateStructure start = new TimeDayDateStructure();
                     TimeDayDateStructure end = new TimeDayDateStructure();

                     DateTime dt = records.GetDateTimeOrDefault("WHEN");

                     start.Date = dt;
                     start.Time = TimeOfDayType.noon;
                     start.TimeSpecified = true;
                     instance.Start = start; if (dt.ToString("dddd") == "Monday")

                         if (dt.ToString("dddd") == "Monday")
                             start.Day = DayOfWeekType.monday;
                     if (dt.ToString("dddd") == "Tuesday")
                         start.Day = DayOfWeekType.tuesday;
                     if (dt.ToString("dddd") == "Wednesday")
                         start.Day = DayOfWeekType.wednesday;
                     if (dt.ToString("dddd") == "Thursday")
                         start.Day = DayOfWeekType.thursday;
                     if (dt.ToString("dddd") == "Friday")
                         start.Day = DayOfWeekType.friday;
                     if (dt.ToString("dddd") == "Saturday")
                         start.Day = DayOfWeekType.saturday;
                     if (dt.ToString("dddd") == "Sunday")
                         start.Day = DayOfWeekType.sunday;


                     end.Day = DayOfWeekType.monday;
                     if (dt.ToString("dddd") == "Tuesday")
                         end.Day = DayOfWeekType.tuesday;
                     if (dt.ToString("dddd") == "Wednesday")
                         end.Day = DayOfWeekType.wednesday;
                     if (dt.ToString("dddd") == "Thursday")
                         end.Day = DayOfWeekType.thursday;
                     if (dt.ToString("dddd") == "Friday")
                         end.Day = DayOfWeekType.friday;
                     if (dt.ToString("dddd") == "Saturday")
                         end.Day = DayOfWeekType.saturday;
                     if (dt.ToString("dddd") == "Sunday")
                         end.Day = DayOfWeekType.sunday;
                     end.Date = dt;
                     end.Time = TimeOfDayType.noon;
                     end.TimeSpecified = true;

                     instance.End = end;
                 }
                );

            return summerexclusionList;
        }

        /// <summary>
        /// get vehicle schedule details
        /// </summary>
        /// <param name="orderNumber">order number</param>
        /// <param name="esDAlRefNo">esdal ref no</param>
        /// <param name="soTemplateType">so template type</param>
        /// <param name="userSchema">user schema</param>
        /// <returns></returns>

        public static List<VehicleScheduleStructure> GetVehicleScheduleStructure(string orderNumber, string esDAlRefNo, Enums.SOTemplateType soTemplateType, string userSchema)
        {
            List<VehicleScheduleStructure> vsslist = new List<VehicleScheduleStructure>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
             vsslist,
             userSchema + ".GET_VEHICLE_SCHEDULE_DETAILS",
             parameter =>
             {
                 parameter.AddWithValue("p_ORDER_NUMBER", orderNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                 parameter.AddWithValue("p_ESDAL_REF_NUMBER", esDAlRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                 parameter.AddWithValue("p_VEHICLE_TYPE", soTemplateType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                 parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
             },
                (records, instance) =>
                {

                    instance.GrossWeight = Convert.ToString(records.GetFloatOrDefault("Gross_Weight")); ;

                    instance.Summary = records.GetStringOrDefault("Vehicle_desc");

                    instance.NumberOfAxles = Convert.ToString(records.GetInt16OrDefault("Axle_count"));

                    instance.CustomNumberOfAxles = CommonMethods.NumberToWords(records.GetInt16OrDefault("Axle_count"));
                }
         );
            return vsslist;
        }
    }
}