using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.Domain;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.DocumentsAndContents.Persistance
{
    public static class ContactDAO
    {
        public static List<ContactListModel> GetContactListSearch(int organizationId, int pageNumber, int pageSize, int searchCriteria, string searchValue, int sortFlag, int presetFilter=1, int? sortOrder=null)
        {
            List<ContactListModel> contactList = new List<ContactListModel>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    contactList,
                     UserSchema.Portal + ".GET_CONTACT_LIST_SEARCH",
                    parameter =>
                    {
                        parameter.AddWithValue("ORG_ID", organizationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("Search_Criteria", searchCriteria, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("Search_Value", searchValue, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("sortFlag", sortFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("SORT_ORDER", sortOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("PRESET_FILTER", presetFilter, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.ContactId = int.Parse(records.GetDecimalOrDefault("CONTACT_ID").ToString());
                            instance.FirstName = records.GetStringOrDefault("NAMES");
                            instance.SurName = records.GetStringOrDefault("SUR_NAME");
                            instance.Organisation = records.GetStringOrDefault("ORGNAME");
                            instance.Title = records.GetStringOrDefault("TITLE");
                            instance.PhoneNumber = records.GetStringOrDefault("PHONENUMBER");
                            instance.Email = records.GetStringOrDefault("EMAIL");
                            instance.Fax = records.GetStringOrDefault("FAX");
                            instance.RecordCount = records.GetDecimalOrDefault("TOTALRECORDCOUNT");
                            instance.OtherOrganisation = records.GetStringOrDefault("OTHER_ORG");
                        }
                );
            return contactList;
        }

    }
}