using Oracle.DataAccess.Client;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.Domain.DocumentsAndContents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.DocumentsAndContents.Persistance
{
    public static class FormVR1DAO
    {
        /// <summary>
        /// get form vr1 document
        /// </summary>
        /// <param name="haulierMnemonic">haulier mnemonic</param>
        /// <param name="esdalRefNumber">esdal ref number</param>
        /// <param name="Version_No">version no</param>
        /// <param name="userSchema">user schema</param>
        /// <returns></returns>
        public static VR1Structure GetFormVR1Details(string haulierMnemonic, string esdalRefNumber, int Version_No, string userSchema)
        {
            VR1Structure Vr1Stru = new VR1Structure();
            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    Vr1Stru,
                    userSchema + ".GET_VR1_DETAILS",
                    parameter =>
                    {
                        parameter.AddWithValue("P_HAULIER_MNEMONIC", haulierMnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ESDAL_REF_NUMBER", esdalRefNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_VERSION_NO", Version_No, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                       (records, instance) =>
                       {
                           #region ESDALRefrenceNumber
                           ESDALReferenceNumberStructure esdalrefnostru = new ESDALReferenceNumberStructure();
                           esdalrefnostru.Mnemonic = records.GetStringOrDefault("HAULIER_MNEMONIC");
                           esdalrefnostru.MovementProjectNumber = Convert.ToString(records.GetInt32OrDefault("esdal_ref_number"));

                           MovementVersionNumberStructure movvernostru = new MovementVersionNumberStructure();
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

                           #region HaulierDetails
                           HaulierDetailsStructure hds = new HaulierDetailsStructure();
                           hds.HaulierName = records.GetStringOrDefault("HAULIER_NAME");
                           AddressStructure AddStruHaulier = new AddressStructure();
                           string[] AddHaulier = new string[4];
                           AddHaulier[0] = records.GetStringOrDefault("HAULIER_ADDRESS_1");
                           AddHaulier[1] = records.GetStringOrDefault("HAULIER_ADDRESS_2");
                           AddHaulier[2] = records.GetStringOrDefault("HAULIER_ADDRESS_3");
                           AddHaulier[3] = records.GetStringOrDefault("HAULIER_ADDRESS_4");

                           AddStruHaulier.Line = AddHaulier;
                           AddStruHaulier.PostCode = records.GetStringOrDefault("haulier_post_code");
                           int country = records.GetInt32OrDefault("haulier_country");
                           if (country == (int)Country.england)
                           {
                               AddStruHaulier.Country = CountryType.england;
                               AddStruHaulier.CountrySpecified = true;
                           }
                           else if (country == (int)Country.northernireland)
                           {
                               AddStruHaulier.Country = CountryType.northernireland;
                               AddStruHaulier.CountrySpecified = true;
                           }
                           else if (country == (int)Country.scotland)
                           {
                               AddStruHaulier.Country = CountryType.scotland;
                               AddStruHaulier.CountrySpecified = true;
                           }
                           else if (country == (int)Country.wales)
                           {
                               AddStruHaulier.Country = CountryType.wales;
                               AddStruHaulier.CountrySpecified = true;
                           }
                           AddStruHaulier.CountrySpecified = true;
                           hds.HaulierAddress = AddStruHaulier;
                           hds.TelephoneNumber = records.GetStringOrDefault("HAULIER_TEL_NO");
                           hds.FaxNumber = records.GetStringOrDefault("HAULIER_FAX_NO");
                           hds.EmailAddress = records.GetStringOrDefault("HAULIER_EMAIL");
                           hds.OrganisationId = Convert.ToInt32(records.GetLongOrDefault("ORGANISATION_ID"));
                           hds.OrganisationIdSpecified = Convert.ToString(records.GetLongOrDefault("ORGANISATION_ID")) != string.Empty ? true : false;
                           hds.HaulierContact = records.GetStringOrDefault("HAULIER_CONTACT");

                           instance.Haulier = hds;

                           #endregion

                           # region AdHocDetails

                           AdhocContactReferenceStructure cs = new AdhocContactReferenceStructure();
                           cs.FullName = records.GetStringOrDefault("HCONTACT");
                           cs.OrganisationName = records.GetStringOrDefault("HNAME");
                           AddressStructure AddStruAddress = new AddressStructure();

                           string[] AddAddress = new string[5];

                           AddAddress[0] = records.GetStringOrDefault("HADDRESS1");
                           AddAddress[1] = records.GetStringOrDefault("HADDRESS2");
                           AddAddress[2] = records.GetStringOrDefault("HADDRESS3");
                           AddAddress[3] = records.GetStringOrDefault("HADDRESS4");
                           AddAddress[4] = records.GetStringOrDefault("HADDRESS5");

                           AddStruAddress.Line = AddAddress;

                           AddStruAddress.PostCode = records.GetStringOrDefault("HPOST_CODE");

                           int appcountry = records.GetInt32OrDefault("HCOUNTRY");

                           if (appcountry == (int)Country.england)
                           {
                               AddStruAddress.Country = CountryType.england;
                               AddStruAddress.CountrySpecified = true;
                           }
                           else if (appcountry == (int)Country.northernireland)
                           {
                               AddStruAddress.Country = CountryType.northernireland;
                               AddStruAddress.CountrySpecified = true;
                           }
                           else if (appcountry == (int)Country.scotland)
                           {
                               AddStruAddress.Country = CountryType.scotland;
                               AddStruAddress.CountrySpecified = true;
                           }
                           else if (appcountry == (int)Country.wales)
                           {
                               AddStruAddress.Country = CountryType.wales;
                               AddStruAddress.CountrySpecified = true;
                           }

                           AddStruAddress.CountrySpecified = true;

                           cs.Address = AddStruAddress;

                           cs.TelephoneNumber = records.GetStringOrDefault("H_TEL_NO");
                           cs.FaxNumber = records.GetStringOrDefault("H_FAX_NO");
                           cs.EmailAddress = records.GetStringOrDefault("HEMAIL");

                           instance.ContactStructure = cs;

                           # endregion

                           #region JourneyTiming
                           JourneyDateStructure jds = new JourneyDateStructure();
                           jds.FirstMoveDate = records.GetDateTimeOrDefault("MOVEMENT_START_DATE");
                           jds.LastMoveDate = records.GetDateTimeOrDefault("MOVEMENT_END_DATE");
                           jds.LastMoveDateSpecified = true;
                           instance.JourneyTiming = jds;
                           #endregion

                           #region LoadDetail
                           instance.LoadDescription = records.GetStringOrDefault("LOAD_DESCR");
                           instance.TotalMoves = records.GetShortOrDefault("TOTAL_MOVES");
                           instance.MaxPiecesPerMove = records.GetShortOrDefault("MAX_PARTS_PER_MOVE");
                           #endregion

                           instance.ApplicationDate = records.GetDateTimeOrDefault("APPLICATION_DATE");

                           instance.JourneyFrom = records.GetStringOrDefault("FROM_DESCR");
                           instance.JourneyTo = records.GetStringOrDefault("TO_DESCR");

                           instance.Height = records.GetStringOrDefault("MAX_HEIGHT");
                           instance.VR1Number = records.GetStringOrDefault("VR1_DETAILS");
                           instance.VehicleDescription = records.GetStringOrDefault("vehicle_desc");

                           instance.GrossWeight = records.GetStringOrDefault("GROSS_WEIGHT");
                           instance.Length = records.GetStringOrDefault("RIGID_LEN");
                           instance.Width = records.GetStringOrDefault("WIDTH");
                       }
                );

                
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - GetFormVR1Details , Exception:" + ex);
            }
            return Vr1Stru;
        }
    }
}