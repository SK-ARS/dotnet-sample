using Oracle.DataAccess.Client;
using STP.Domain.Applications;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Globalization;

namespace STP.Applications.Persistance
{
    public class SpecialOrderDAO
    {
        const string m_RouteName = "SpecialOrder";
        /// <summary>
        /// SaveVR1Approval
        /// </summary>
        /// <param name="vr1ApprovalInsertParams"></param>
        /// <returns></returns>
        public static int SaveVR1Approval(VR1ApprovalInsertParams vr1ApprovalInsertParams)
        {
            int result = 0;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                  result,
                  UserSchema.Sort + ".SP_SORT_VR1_APPROVAL",
                  parameter =>
                  {
                      parameter.AddWithValue("p_PROJECT_ID", vr1ApprovalInsertParams.ProjectID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("p_REV_NO", vr1ApprovalInsertParams.RevisionNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                  },
                  record =>
                  {

                      result = int.Parse(record.GetDecimalOrDefault("PROJ_CNT").ToString());

                  }
             );

            return result;
        }
        /// <summary>
        /// SaveSORTAllocateUser
        /// </summary>
        /// <param name="allocateSORTUserParams"></param>
        /// <returns></returns>
        public static List<string> SaveSORTAllocateUser(AllocateSORTUserInsertParams allocateSORTUserParams)
        {
            int result = 0;
            DateTime workDueDate = new DateTime();
            if (allocateSORTUserParams.PlannerUserID != 0)
            {
                workDueDate = DateTime.ParseExact(allocateSORTUserParams.DueDate, ApplicationConstants.DateFormatDDMMYYY, CultureInfo.InvariantCulture);
            }
            List<string> responseList = new List<string>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                  result,
                  UserSchema.Sort + ".SP_SORT_ALLOCATE_USER",
                  parameter =>
                  {
                      parameter.AddWithValue("P_PROJ_ID", allocateSORTUserParams.ProjectID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("P_PLANNER_ID", allocateSORTUserParams.PlannerUserID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                      if (allocateSORTUserParams.PlannerUserID != 0)
                      {
                          parameter.AddWithValue("P_DUE_DATE", TimeZoneInfo.ConvertTimeToUtc(workDueDate), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                      }
                      else
                      {
                          parameter.AddWithValue("P_DUE_DATE", null, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                      }
                      parameter.AddWithValue("P_REVISION_NO", allocateSORTUserParams.RevisionNumber, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                  },
                  record =>
                  {
                      if (allocateSORTUserParams.PlannerUserID != 0)
                      {
                          responseList.Add(Convert.ToString((int)record.GetDecimalOrDefault("PROJ_CNT")));
                      }
                      else
                      {
                          responseList.Add(record.GetStringOrDefault("USERNAME"));
                          responseList.Add(record.GetStringOrDefault("DUE_DATE"));
                          responseList.Add(Convert.ToString(record.GetLongOrDefault("PLANNER_USER_ID")));
                      }
                  }
             );

            return responseList;
        }
        /// <summary>
        /// SaveNewSortSpecialOrder
        /// </summary>
        /// <param name="sortSpecialOrder"></param>
        /// <param name="removedCoverage"></param>
        /// <returns></returns>
        public static string SaveNewSortSpecialOrder(SORTSpecialOrder sortSpecialOrder, List<string> removedCoverage)
        {
            DateTime signDate = DateTime.Parse(sortSpecialOrder.SignDate);
            DateTime expiryDate = DateTime.Parse(sortSpecialOrder.ExpiryDate);

            int state = int.Parse(sortSpecialOrder.State);
            int template = int.Parse(sortSpecialOrder.Template);
            string soNumber = "";
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                  sortSpecialOrder,
                  UserSchema.Sort + ".SP_INSERT_SORT_SPECIAL_ORDER",
                  parameter =>
                  {
                      parameter.AddWithValue("SO_ESDAL_REF", sortSpecialOrder.ESDALNo, OracleDbType.NVarchar2, ParameterDirectionWrap.Input);
                      parameter.AddWithValue("SO_STATE", state, OracleDbType.Int32, ParameterDirectionWrap.Input);
                      parameter.AddWithValue("SO_SIGNED_DATE", signDate, OracleDbType.Date, ParameterDirectionWrap.Input);
                      parameter.AddWithValue("SO_EXPIRY_DATE", expiryDate, OracleDbType.Date, ParameterDirectionWrap.Input);
                      parameter.AddWithValue("SO_SIGNATORY", sortSpecialOrder.Signatory, OracleDbType.NVarchar2, ParameterDirectionWrap.Input);
                      parameter.AddWithValue("SO_SIGNATORY_ROLE", sortSpecialOrder.SignatoryRole, OracleDbType.NVarchar2, ParameterDirectionWrap.Input);
                      parameter.AddWithValue("SO_TEMPLATE", template, OracleDbType.Int32, ParameterDirectionWrap.Input);
                      parameter.AddWithValue("SO_YEAR", sortSpecialOrder.Year, OracleDbType.Int32, ParameterDirectionWrap.Input);
                      parameter.AddWithValue("SO_PROJECT_ID", sortSpecialOrder.ProjectID, OracleDbType.Int32, ParameterDirectionWrap.Input);
                      parameter.AddWithValue("SO_VERSION_ID", sortSpecialOrder.VersionID, OracleDbType.Int32, ParameterDirectionWrap.Input);
                      parameter.AddWithValue("SO_APPLICABILITY", sortSpecialOrder.Applicability, OracleDbType.NVarchar2, ParameterDirectionWrap.Input);
                      parameter.AddWithValue("SO_COVARGEGRID", sortSpecialOrder.CoverageStatus, OracleDbType.Int32, ParameterDirectionWrap.Input);
                      parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                  },
                  (record, instance) =>
                  {
                      soNumber = record.GetStringOrDefault("ORDER_NO");
                  }
             );
            //Update the Removed Applicabilities.
            UpdateSOApplicability(removedCoverage);
            return soNumber;

        }
        /// <summary>
        /// UpdateSOApplicability
        /// </summary>
        /// <param name="removedCoverage"></param>
        public static void UpdateSOApplicability(List<string> removedCoverage)
        {
            if (removedCoverage.Count != 0)
            {
                foreach (string coverage in removedCoverage)
                {
                    if (coverage != "")
                    {
                        string[] coverageApplicability = coverage.Split('#');
                        if (coverageApplicability.Count() > 1)
                        {
                            string applicability = coverageApplicability[0].ToString();
                            string specialorderno = coverageApplicability[1].ToString();
                            
                                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                                      removedCoverage,
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
        /// <summary>
        /// UpdateSortSpecialOrder
        /// </summary>
        /// <param name="sortSpecialOrder"></param>
        /// <param name="removedCoverage"></param>
        /// <returns></returns>
        public static string UpdateSortSpecialOrder(SORTSpecialOrder sortSpecialOrder, List<string> removedCoverage)
        {
            DateTime signDate = DateTime.Parse(sortSpecialOrder.SignDate);
            DateTime expiryDate = DateTime.Parse(sortSpecialOrder.ExpiryDate);
            int state = int.Parse(sortSpecialOrder.State);
            int template = int.Parse(sortSpecialOrder.Template);
            string soNumber = "";

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                  sortSpecialOrder,
                  UserSchema.Sort + ".SP_UPDATE_SORT_SPECIAL_ORDER",
                  parameter =>
                  {
                      parameter.AddWithValue("SO_ORDER_NO", sortSpecialOrder.SONumber, OracleDbType.NVarchar2, ParameterDirectionWrap.Input);
                      parameter.AddWithValue("SO_SIGNED_DATE", signDate, OracleDbType.Date, ParameterDirectionWrap.Input);
                      parameter.AddWithValue("SO_EXPIRY_DATE", expiryDate, OracleDbType.Date, ParameterDirectionWrap.Input);
                      parameter.AddWithValue("SO_SIGNATORY", sortSpecialOrder.Signatory, OracleDbType.NVarchar2, ParameterDirectionWrap.Input);
                      parameter.AddWithValue("SO_SIGNATORY_ROLE", sortSpecialOrder.SignatoryRole, OracleDbType.NVarchar2, ParameterDirectionWrap.Input);
                      parameter.AddWithValue("SO_STATE", state, OracleDbType.Int32, ParameterDirectionWrap.Input);
                      parameter.AddWithValue("SO_TEMPLATE", template, OracleDbType.Int32, ParameterDirectionWrap.Input);
                      parameter.AddWithValue("SO_APPLICABILITY", sortSpecialOrder.Applicability, OracleDbType.NVarchar2, ParameterDirectionWrap.Input);
                      parameter.AddWithValue("SO_COVARGEGRID", sortSpecialOrder.CoverageStatus, OracleDbType.Int32, ParameterDirectionWrap.Input);
                  },
                  (record, instance) =>
                  {
                      soNumber = record.GetStringOrDefault("ORDER_NO");
                  }
             );
            //Update the Removed Applicabilities.
            UpdateSOApplicability(removedCoverage);
            return soNumber;

        }
        /// <summary>
        /// DeleteSpecialOrder
        /// </summary>
        /// <param name="specialOrder"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        internal static int DeleteSpecialOrder(string specialOrder, string userSchema = ApplicationConstants.DbUserSchemaSTPSORT)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(                
                userSchema + ".SP_DELETE_SPECIAL_ORDER",
                parameter =>
                {
                    parameter.AddWithValue("SO_ORDER_NO", specialOrder, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        result = Convert.ToInt32("p_AFFECTED_ROWS");
                    }
          );
            return result;


        }
        /// <summary>
        /// MovementVersionAgreeUnagreeWith
        /// </summary>
        /// <param name="movementVersionAgreeUnagreeWithInsertParams"></param>
        /// <returns></returns>
        public static int MovementVersionAgreeUnagreeWith(MovementVersionAgreeUnagreeWithInsertParams movementVersionAgreeUnagreeWithInsertParams)
        {
            int result = 0;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                  result,
                  UserSchema.Sort + ".SORT_MOVEMENT_AGREE",
                  parameter =>
                  {
                      parameter.AddWithValue("p_VERSION_ID", movementVersionAgreeUnagreeWithInsertParams.VersionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("p_Flag_ID", movementVersionAgreeUnagreeWithInsertParams.Flag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                  },
                  record =>
                  {
                      result = 1;
                  }
             );
            return result;

        }
        /// <summary>
        /// SaveVR1Number
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public static string SaveVR1Number(VR1NumberInsertParams vr1NumberInsertParams)
        {
            string result = string.Empty;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                  result,
                  UserSchema.Sort + ".SP_INSERT_VR1S",
                  parameter =>
                  {
                      parameter.AddWithValue("p_PROJECT_ID", vr1NumberInsertParams.ProjectId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("p_CONATCT_ID", vr1NumberInsertParams.ContactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                  },
                  record =>
                  {
                      result = record.GetStringOrDefault("VRNO");
                  }
             );
            return result;
        }
    }
}