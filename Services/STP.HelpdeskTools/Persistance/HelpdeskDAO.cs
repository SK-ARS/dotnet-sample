using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.DataAccess.SafeProcedure;
using STP.Domain.HelpdeskTools;
using System;
using System.Collections.Generic;

namespace STP.HelpdeskTools.Persistance
{
    public static class HelpdeskDAO
    {
        #region Get Distribution Alerts
        public static List<DistributionAlerts> GetSORTDistributionAlerts(int pageNum, int pageSize, DistributionAlerts objDist, int portalType, int presetFilter, int? sortOrder = null)
        {
            List<DistributionAlerts> objDistributionAlert = new List<DistributionAlerts>();

            int showAllFailed = objDist.ShowAlert == "on" || objDist.ShowAlert == "true" ? 1 : 0;
            int mov_type = portalType == 696008 && Convert.ToInt32(objDist.MovementData) == 0 ? 2 : Convert.ToInt32(objDist.MovementData);
            string To_Org = objDist.ToOrganisationName?.ToUpper();
            objDist.EndDate = objDist.EndDate != null ? objDist.EndDate.Value.AddDays(1).AddSeconds(-1): objDist.EndDate;
            DateTime currentDateTime = DateTime.Now;
            DateTime yesterdayDateTime = DateTime.Now.AddDays(-1);
            objDist.StartDate = objDist.StartDate == null ? yesterdayDateTime : objDist.StartDate;
            objDist.EndDate = objDist.EndDate == null ? currentDateTime : objDist.EndDate;
            objDist.SortOrder = objDist.SearchFlag == 1 ? 1 : 0;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
            objDistributionAlert,
            UserSchema.Portal + ".SP_GET_DISTRIBUTION_ALERT",
            parameter =>
            {
                parameter.AddWithValue("P_ESDAL_REF", objDist.ESDALReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_MOVEMENT_TYPE", mov_type, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_SHOW_ALERT", showAllFailed, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                if (objDist.StartDate.HasValue)
                    parameter.AddWithValue("P_START_DATE", TimeZoneInfo.ConvertTimeToUtc(objDist.StartDate.Value), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                else
                    parameter.AddWithValue("P_START_DATE", objDist.StartDate, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                if (objDist.StartDate.HasValue)
                    parameter.AddWithValue("P_END_DATE", TimeZoneInfo.ConvertTimeToUtc(objDist.EndDate.Value), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                else
                    parameter.AddWithValue("P_END_DATE", objDist.EndDate, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_USER_TYPE", portalType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_TO_ORG", To_Org, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_SORT_ORDER", sortOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("pageNumber", pageNum, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("PRESET_FILTER", presetFilter, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
            (records, instance) =>
            {
                instance.TransmissionId = Convert.ToInt32(records.GetLongOrDefault("TRANSMISSION_ID"));
                instance.DateTime = Convert.ToString(records.GetDateTimeOrEmpty("INBOX_CREATION_TIME"));
                instance.InboxCreationTime = records.GetDateTimeOrEmpty("INBOX_CREATION_TIME");
                instance.AlertType = records.GetStringOrDefault("ALERT_TYPE");
                instance.ESDALReference = records.GetStringOrDefault("ESDAL_REFERENCE");
                instance.Details = records.GetStringOrDefault("REASON");
                instance.OrganisationId = (int)records.GetLongOrDefault("TO_ORGANISATION_ID");
                instance.ContactId = (int)records.GetLongOrDefault("TO_CONTACT_ID");
                instance.ToOrganisationName = records.GetStringOrDefault("TO_ORGNAME");
                instance.ToContactName = records.GetStringOrDefault("TO_FULLNAME");
                instance.OrganisationTypeName = records.GetStringOrDefault("ORG_TYPE_NAME");
                instance.FromOrganisationName = records.GetStringOrDefault("FROM_ORGANISATION");
                instance.IsManullyAdded = records.GetInt16OrDefault("IS_MANUALLY_ADDED");
                try
                {
                    instance.TotalCount = (int)records.GetDecimalOrDefault("TOTALRECORDCOUNT");
                }
                catch
                {
                    instance.TotalCount = (int)records.GetLongOrDefault("TOTALRECORDCOUNT");
                }
            });
            return objDistributionAlert;
        }
        #endregion
    }
}