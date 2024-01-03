using Oracle.DataAccess.Client;
using STP.Common.Enums;
using STP.DataAccess.SafeProcedure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.DocumentsAndContents.Persistance
{
    public static class RouteAssessmentDAO
    {
        #region fetchContactPreference(int contactId)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public static string[] FetchContactPreference(int contactId, string userSchema)
        {
            string[] contactDet = new string[6];
            int commnCode = 0;
            int userTypeId = 0;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               contactDet,
               userSchema + ".STP_ROUTE_ASSESSMENT.SP_FETCH_CONTACT_DETAILS",
               parameter =>
               {
                   parameter.AddWithValue("P_CONTACT_ID", contactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
               records =>
               {
                   commnCode = Convert.ToInt32(records.GetDecimalOrDefault("COMMUNICATION_METHOD"));
                   userTypeId = Convert.ToInt32(records.GetDecimalOrDefault("USER_TYPE_ID"));
                   switch (commnCode)
                   {
                       case 695001:
                           contactDet[0] = "Fax";
                           break;
                       case 695002:
                           contactDet[0] = "Email";
                           break;
                       case 695003:
                           contactDet[0] = "Email";
                           break;
                       case 695004:
                           contactDet[0] = "Online Inbox Only";
                           break;
                       case 695005:
                           contactDet[0] = "Online inbox plus email (HTML)";
                           break;
                       default:
                           contactDet[0] = "Email";
                           break;
                   }

                   contactDet[1] = records.GetStringOrDefault("EMAIL");
                   contactDet[2] = records.GetStringOrDefault("FAX");

                   switch (userTypeId)
                   {
                       case 696002:
                           contactDet[3] = "police";
                           break;
                       case 696007:
                           contactDet[3] = "soa";
                           break;
                   }

                   contactDet[4] = Convert.ToString(records.GetLongOrDefault("ORGANISATION_ID"));
                   contactDet[5] = Convert.ToString(records.GetDecimalOrDefault("ATTACH_XML"));
               });

            return contactDet;
        }
        #endregion
    }
}