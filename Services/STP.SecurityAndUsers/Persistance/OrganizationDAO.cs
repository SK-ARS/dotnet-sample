using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.DataAccess.SafeProcedure;
using STP.Domain.ExternalAPI;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace STP.SecurityAndUsers.Persistance
{
    public static class OrganizationDAO
    {
        #region GetOrganisationInfo
        internal static List<OrganizationGridList> GetOrganizationInformation(string criteria,int pageNumber, int pageSize, int userTypeId,string searchOrgCode, int sortOrder, int presetFilter)
        {
            if (criteria != null)
            {
                criteria = criteria.Trim();
            }
            List<OrganizationGridList> componentGridObj = new List<OrganizationGridList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                componentGridObj,
               UserSchema.Portal + ".STP_LOGIN_PKG.GetAllOrganisations",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_CODE", searchOrgCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("p_ORG_NAME", criteria, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("p_PAGENUMBER", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PAGESIZE", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_UserTypeId", userTypeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("SORT_ORDER", sortOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("PRESET_FILTER", presetFilter, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.OrgId = records.GetLongOrDefault("ORGANISATION_ID");
                    instance.OrgName = records.GetStringOrDefault("ORGNAME");
                    instance.OrgType = records.GetStringOrDefault("NAME");
                    instance.AddressLine1 = records.GetStringOrDefault("ADDRESSLINE_1");
                    instance.AddressLine2 = records.GetStringOrDefault("ADDRESSLINE_2");
                    instance.AddressLine3 = records.GetStringOrDefault("ADDRESSLINE_3");
                    instance.AddressLine4 = records.GetStringOrDefault("ADDRESSLINE_4");
                    instance.AddressLine5 = records.GetStringOrDefault("ADDRESSLINE_5");
                    instance.Phone = records.GetStringOrDefault("PHONENUMBER");
                    instance.Web = records.GetStringOrDefault("ORG_URL");
                    instance.OrgCode = records.GetStringOrDefault("ORG_CODE");
                    instance.AuthenticationKey = records.GetStringOrDefault("AUTHENTICATION_KEY");
                    instance.TotalRecordCount = (long)Convert.ToInt32(records["TOTALRECORDCOUNT"]);
                }
                );
            return componentGridObj;
        }

        
        #endregion
        #region GetOrganisationInformation
        internal static List<OrganizationGridList> GetOrganizationInformation(int pageNumber, int pageSize, int userTypeId, int sortOrder, int presetFilter)
        {
            List<OrganizationGridList> componentGridObj = new List<OrganizationGridList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                componentGridObj,
               UserSchema.Portal + ".STP_LOGIN_PKG.GetAllOrganisations",
                parameter =>
                {
                    parameter.AddWithValue("p_PAGENUMBER", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PAGESIZE", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_UserTypeId", userTypeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SORT_ORDER", sortOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PRESET_FILTER", presetFilter, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.OrgId = records.GetLongOrDefault("ORGANISATION_ID");
                    instance.OrgName = records.GetStringOrDefault("ORGNAME");
                    instance.OrgType = records.GetStringOrDefault("NAME");
                    instance.AddressLine1 = records.GetStringOrDefault("ADDRESSLINE_1");
                    instance.AddressLine2 = records.GetStringOrDefault("ADDRESSLINE_2");
                    instance.AddressLine3 = records.GetStringOrDefault("ADDRESSLINE_3");
                    instance.AddressLine4 = records.GetStringOrDefault("ADDRESSLINE_4");
                    instance.AddressLine5 = records.GetStringOrDefault("ADDRESSLINE_5");
                    instance.Phone = records.GetStringOrDefault("PHONENUMBER");
                    instance.Web = records.GetStringOrDefault("ORG_URL");
                    instance.OrgCode = records.GetStringOrDefault("ORG_CODE");
                    instance.AuthenticationKey = records.GetStringOrDefault("AUTHENTICATION_KEY");
                    instance.TotalRecordCount = (long)Convert.ToInt32(records["TOTALRECORDCOUNT"]);
                }
                );
            return componentGridObj;
        }
        #endregion
        #region OrganizationTypeList
        internal static List<OrganizationTypeList> GetOrganizationTypeList()
        {
            List<OrganizationTypeList> OrgTypeListObj = new List<OrganizationTypeList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                OrgTypeListObj,
               UserSchema.Portal + ".STP_LOGIN_PKG.GetAllOrganisationType",
                parameter =>
                {
                    parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.OrgTypeCode = records.GetInt32OrDefault("CODE");
                    instance.OrgTypeName = records.GetStringOrDefault("NAME");
                }
                );
            return OrgTypeListObj;
        }
        #endregion
        #region Save organisation
        internal static int SaveOrganization(Organization orgDet)
        {
            int result = 0;
            int IsreceiveNEN = 0;
            int AccessToALSAT = 0;
            if (orgDet.IsNENsReceive)
            {
                IsreceiveNEN = 1;
            }
            else
            {
                IsreceiveNEN = 0;
            }
            if (orgDet.AccessToALSAT)
            {
                AccessToALSAT = 1;
            }
            else
            {
                AccessToALSAT = 0;
            }

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                orgDet,
               UserSchema.Portal + ".STP_LOGIN_PKG.InsertOrganisation",
                parameter =>
                {
                    parameter.AddWithValue("p_ORGNAME", orgDet.OrgName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGTYPE", orgDet.OrgType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORG_CODE", orgDet.OrgCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LICENSE_NR", orgDet.Licence_NR, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_1", orgDet.AddressLine1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_2", orgDet.AddressLine2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_3", orgDet.AddressLine3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_4", orgDet.AddressLine4, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE_5", orgDet.AddressLine5, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_POSTCODE", orgDet.PostCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COUNTRY_ID", orgDet.CountryID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PHONENUMBER", orgDet.Phone, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORG_URL", orgDet.Web, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AUTHENTICATION_KEY", orgDet.AuthenticationKey, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RECEIVE_NEN", IsreceiveNEN, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_Access_To_ALSAT", AccessToALSAT, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                record =>
                { 
                    result = Convert.ToInt32(record.GetDecimalOrDefault("ORG_ID"));
                }
            );           
            return result;
        }

        
        #endregion
        #region Edit Organization
        internal static int EditOrganization(Organization orgDet)
        {
            int result = 0;
            int IsreceiveNEN = 0;
            int AccessToALSAT = 0;

            if (orgDet.IsNENsReceive)
            {
                IsreceiveNEN = 1;
            }
            else
            {
                IsreceiveNEN = 0;
            }
            if (orgDet.AccessToALSAT)
            {
                AccessToALSAT = 1;
            }
            else
            {
                AccessToALSAT = 0;
            }

            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
               UserSchema.Portal + ".STP_LOGIN_PKG.EditOrganisation",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_ID", orgDet.OrgID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGNAME", orgDet.OrgName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGTYPE", orgDet.OrgType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORG_CODE", orgDet.OrgCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LICENSE_NR", orgDet.Licence_NR, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE1", orgDet.AddressLine1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE2", orgDet.AddressLine2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE3", orgDet.AddressLine3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE4", orgDet.AddressLine4, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESSLINE5", orgDet.AddressLine5, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_POSTCODE", orgDet.PostCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COUNTRY_ID", orgDet.CountryID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PHONENUMBER", orgDet.Phone, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORG_URL", orgDet.Web, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AUTHENTICATION_KEY", orgDet.AuthenticationKey, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RECEIVE_NEN", IsreceiveNEN, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_Access_To_ALSAT", AccessToALSAT, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = (records.GetInt32("P_AFFECTED_ROWS"));
                }
            );            
            return result;
        }
        #endregion

        internal static decimal SearchOrganisationByName(string orgName, int type, string mode, string organisationId)
        {
            decimal result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                 UserSchema.Portal + ".STP_LOGIN_PKG.sp_IsOrganisationExists",
                parameter =>
                {
                    parameter.AddWithValue("p_ORGNAME", orgName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TYPE", type, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MODE", mode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGID", organisationId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = records.GetDecimalOrDefault("COUNT");
                }
                );
            return result;
        }
        #region Validate Authentication 
        public static ValidateAuthentication ValidateAuthentication(string authenticationKey)
        {
            ValidateAuthentication authentication = new ValidateAuthentication();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                authentication,
                 UserSchema.Portal + ".SP_VALIDATE_AUTHENTICATION",
                parameter =>
                {
                    parameter.AddWithValue("p_Authentication_Key", authenticationKey, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records,instance) =>
                {
                    instance.OrganisationId = (int)records.GetDecimalOrDefault("ORG_ID");
                    instance.UserTypeId = (int)records.GetDecimalOrDefault("USER_TYPE_ID");
                });
            return authentication;
        }
        #endregion

        public static List<ViewOrganizationByID> ViewOrganisationByIDForSORT(int RevisionId)
        {
            List<ViewOrganizationByID> OrgTypeListObj = new List<ViewOrganizationByID>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                OrgTypeListObj,
                "STP_SORT.SP_SORT_SO_ORG_APP_REVISIONID",
                parameter =>
                {
                    parameter.AddWithValue("P_REVISION_ID", RevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.OrgId = records.GetLongOrDefault("ORGANISATION_ID");
                    instance.OrgName = records.GetStringOrDefault("HAULIER_NAME");
                    instance.HAContact = records.GetStringOrDefault("HAULIER_CONTACT");
                    // instance.OrgType = Convert.ToString(records.GetInt32OrDefault("ORGTYPE"));
                    instance.OrgCode = records.GetStringOrDefault("ORG_CODE");
                    instance.LicenseNR = records.GetStringOrDefault("LICENSE_NR");
                    instance.AddressLine1 = records.GetStringOrDefault("HAULIER_ADDRESS_1");
                    instance.AddressLine2 = records.GetStringOrDefault("HAULIER_ADDRESS_2");
                    instance.AddressLine3 = records.GetStringOrDefault("HAULIER_ADDRESS_3");
                    instance.AddressLine4 = records.GetStringOrDefault("HAULIER_ADDRESS_4");
                    instance.AddressLine5 = records.GetStringOrDefault("HAULIER_ADDRESS_5");
                    instance.PostCode = records.GetStringOrDefault("HAULIER_POST_CODE");
                    instance.CountryId = Convert.ToString(records.GetInt32OrDefault("HAULIER_COUNTRY"));
                    instance.Phone = records.GetStringOrDefault("HAULIER_TEL_NO");
                    instance.Fax = records.GetStringOrDefault("HAULIER_FAX_NO");
                    instance.EmailId = records.GetStringOrDefault("HAULIER_EMAIL");
                }
                );
            return OrgTypeListObj;
        }

        #region GetOrgNameByAuthKey
        public static AuthKeyValid GetOrgDetailsByAuthKey(string authenticationKey)
        {
            AuthKeyValid orgDetails = new AuthKeyValid();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                orgDetails,
                UserSchema.Portal + ".SP_GET_ORG_DETAILS_BY_AUTHKEY",
                parameter =>
                {
                    parameter.AddWithValue("P_AUTH_KEY", authenticationKey, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.OrganisationId = records.GetLongOrDefault("v_org_id");
                    instance.OrganisationName = records.GetStringOrDefault("V_ORG_NAME");
                });
            
            return orgDetails;
        }
        #endregion

        #region NEN API GET INPUT AFFECTED PARTIES DETAILS 
        public static List<ContactModel> GetAffectedOrganisationDetails(string affectedParties, int affectedPartiesCount, string userSchema)
        {
            List<ContactModel> contacts = new List<ContactModel>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                contacts,
                UserSchema.Portal + ".SP_GET_AFFECT_ORG_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_AFFECTED_PARTIES", affectedParties, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECTED_PARTIES_COUNT", affectedPartiesCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records,instance) =>
                {
                    instance.ContactId = (int)records.GetDecimalOrDefault("CONTACT_ID");
                    instance.FullName = records.GetStringOrDefault("FULL_NAME");
                    instance.Email = records.GetStringOrDefault("EMAIL");
                    instance.Fax = records.GetStringOrDefault("FAX");
                    instance.Organisation = records.GetStringOrDefault("ORGNAME");
                    instance.ISPolice = records.GetStringOrDefault("ORG_TYPE").ToLower() == "police";
                    instance.OrganisationId = (int)records.GetLongOrDefault("ORG_ID");
                    instance.Reason = records.GetStringOrDefault("REASON");
                    instance.IsReceiveNen = records.GetInt16OrDefault("RECEIVE_NEN");
                });

            return contacts;
        }
        #endregion

        #region GetAuthorizedUsers
        public static AuthorizedOrganisation GetAuthorizedUsers(string ESDALReferenceNumber, bool isApp)
        {
            AuthorizedOrganisation orgDetails = new AuthorizedOrganisation();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                orgDetails,
                UserSchema.Portal + ".SP_GET_MOVE_AUTH_USERS",
                parameter =>
                {
                    parameter.AddWithValue("P_ESDAL_REF", ESDALReferenceNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_IS_APP", isApp ? 1 : 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    var validCount = records.GetDecimalOrDefault("COUNT");
                    var sortValid = records.GetDecimalOrDefault("SORT_COUNT");
                    if (validCount > 0 || sortValid > 0)
                    {
                        instance.ValidCount = (int)validCount;
                        instance.SortValid = (int)sortValid;
                        var receiverIds = records.GetStringOrDefault("RECEIVER");
                        var sortIds = records.GetStringOrDefault("SORT");
                        instance.Receivers = new List<long>();
                        instance.SortOrgIds = new List<long>();
                        if (!string.IsNullOrWhiteSpace(receiverIds))
                        {
                            var receivers = receiverIds.Split(',');
                            foreach (var item in receivers)
                            {
                                instance.Receivers.Add(Convert.ToInt32(item));
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(sortIds))
                        {
                            var sort = sortIds.Split(',');
                            foreach (var item in sort)
                            {
                                instance.SortOrgIds.Add(Convert.ToInt32(item));
                            }
                        }
                        instance.SenderId = (long)records.GetDecimalOrDefault("SENDER");
                    }
                });

            return orgDetails;
        }
        #endregion

        #region NEN PDF GET INPUT AFFECTED PARTIES DETAILS 
        public static List<ContactModel> GetNenAffectedOrganisationDetails(int inboxItemId)
        {
            List<ContactModel> contacts = new List<ContactModel>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                contacts,
                UserSchema.Portal + ".SP_GET_NEN_PDF_AFFECT_ORG_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_INBOX_ITEM_ID", inboxItemId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.ContactId = (int)records.GetDecimalOrDefault("CONTACT_ID");
                    instance.FullName = records.GetStringOrDefault("FULL_NAME");
                    instance.Email = records.GetStringOrDefault("EMAIL");
                    instance.Fax = records.GetStringOrDefault("FAX");
                    instance.Organisation = records.GetStringOrDefault("ORGNAME");
                    instance.ISPolice = records.GetStringOrDefault("ORG_TYPE").ToLower() == "police";
                    instance.OrganisationId = (int)records.GetLongOrDefault("ORG_ID");
                    instance.Reason = records.GetStringOrDefault("REASON");
                    instance.IsReceiveNen = records.GetInt16OrDefault("RECEIVE_NEN");
                });

            return contacts;
        }
        #endregion
    }
}
