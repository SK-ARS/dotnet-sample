using System;
using System.Collections.Generic;
using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.DataAccess.SafeProcedure;
using System.Linq;
using System.Web;
using STP.Domain;
using STP.Domain.SecurityAndUsers;

namespace STP.DocumentsAndContents.Persistance
{
    public static class HaulierAddressDAO
    {

        /// <summary>
        /// Get Haulier Contact List
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<HaulierContactModel> GetHaulierContactList(int organizationId, int pageNumber, int pageSize, string searchCriteria, string searchValue, int presetFilter = 1, int? sortOrder = null)
        {


            List<HaulierContactModel> objHauliercontactList = new List<HaulierContactModel>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                objHauliercontactList,
               UserSchema.Portal + ".GET_HAULIERCONTACT_LIST_SEARCH",
                parameter =>
                {
                    parameter.AddWithValue("ORG_ID", organizationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("Search_Criteria", searchCriteria, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    
                   parameter.AddWithValue("Search_Value", searchValue, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("SORT_ORDER", sortOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("PRESET_FILTER", presetFilter, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.HaulierContactId = records.GetLongOrDefault("HAULIER_CONTACT_ID");
                        instance.Name = records.GetStringOrDefault("NAME");
                        instance.Email = records.GetStringOrDefault("EMAIL");
                        instance.Fax = records.GetStringOrDefault("FAX");
                        instance.CommunicationMethod = records.GetInt32OrDefault("COMMUNICATION_METHOD");
                        instance.CommunicationMethodName = records.GetStringOrDefault("COMMUNICATION_METHOD_NAME");
                        instance.OrganisationName = records.GetStringOrDefault("ORGANISATION_NAME");
                        instance.CommunicationMethodType = records.GetInt16OrDefault("COMMUNICATION_METHOD_TYPE");
                        instance.RecordCount = records.GetDecimalOrDefault("COUNTREC");
                    }

            );

            return objHauliercontactList;
        }

        /// <summary>
        /// Get Haulier contact detail by HaulierContactId
        /// </summary>
        /// <param name="haulierContactId"></param>
        /// <returns></returns>
        internal static HaulierContactModel GetHaulierContactById(double haulierContactId)
        {
            HaulierContactModel objHaulierContactList = new HaulierContactModel();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                objHaulierContactList,
               UserSchema.Portal + ".GET_HAULIERCONTACT_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_HAULIER_CONTACT_ID", haulierContactId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.HaulierContactId = records.GetLongOrDefault("HAULIER_CONTACT_ID");
                        instance.Name = records.GetStringOrDefault("NAME");
                        instance.Email = records.GetStringOrDefault("EMAIL");
                        instance.Fax = records.GetStringOrDefault("FAX");
                        instance.CommunicationMethod = records.GetInt32OrDefault("COMMUNICATION_METHOD");
                        instance.CommunicationMethodName = records.GetStringOrDefault("COMMUNICATION_METHOD_NAME");
                        instance.OrganisationName = records.GetStringOrDefault("ORGANISATION_NAME");
                        instance.OrganisationId = records.GetLongOrDefault("ORGANISATION_ID");
                        instance.CommunicationMethodType = records.GetInt16OrDefault("COMMUNICATION_METHOD_TYPE");
                    }
            );
            return objHaulierContactList;
        }

        /// <summary>
        /// Delete Haulier Contact by Id
        /// </summary>
        /// <param name="HAULIER_CONTACT_ID"></param>
        /// <returns></returns>
        internal static int DeleteHaulierContact(double haulierContactId)
        {
            
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
               UserSchema.Portal + ".DELETE_HAULIERCONTACT",
                parameter =>
                {
                    parameter.AddWithValue("P_HAULIER_CONTACT_ID", haulierContactId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    rowsAffected = record.GetInt32("p_AFFECTED_ROWS");

                });
            return rowsAffected;
        }

        /// <summary>
        /// Delete Haulier Contact by Id
        /// </summary>
        /// <param name="HAULIER_CONTACT_ID"></param>
        /// <returns></returns>
        internal static bool ManageHaulierContact(HaulierContactModel haulierContactModel)
        {

            bool result = false;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               UserSchema.Portal + ".MANAGE_HAULIERCONTACT",
                parameter =>
                {
                    parameter.AddWithValue("P_HAULIER_CONTACT_ID", haulierContactModel.HaulierContactId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORGANISATION_ID", haulierContactModel.OrganisationId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NAME", haulierContactModel.Name, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORGANISATION_NAME", haulierContactModel.OrganisationName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_FAX", haulierContactModel.Fax, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_EMAIL", haulierContactModel.Email, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_COMMUNICATION_METHOD", haulierContactModel.CommunicationMethod, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_COMMUNICATION_METHOD_NAME", haulierContactModel.CommunicationMethodName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_COMMUNICATION_METHOD_TYPE", haulierContactModel.CommunicationMethodType, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    result = true; 

                });
               
            return result;
        }
    }
}