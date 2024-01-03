using NetSdoGeometry;
using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.Domain.RoadNetwork.Constraint;
using STP.Domain.RouteAssessment;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace STP.RoadNetwork.Persistance
{
    public static class ConstraintDAO
    {
        public static List<ConstraintModel> GetConstraintHistory(int pageNumber, int pageSize, long constraintId)
        {
            List<ConstraintModel> listConstraint = new List<ConstraintModel>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    listConstraint,
                    UserSchema.Portal + ".GET_CONSTRAINT_HISTORYDETAILS",
                    parameter =>
                    {
                        parameter.AddWithValue("P_CONSTRAINT_ID", constraintId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (DataAccess.Delegates.RecordMapper<ConstraintModel>)((records, instance) =>
                        {
                            instance.OccurredTime = records.GetDateTimeOrDefault("OCCURRED_TIME");
                            instance.HistoryDetails = records.GetStringOrDefault("AMENDMENT_1");
                            instance.FirstName = records.GetStringOrDefault("AUTHOR");
                            instance.TotalRecordCount = records.GetDecimalOrDefault("TOTALRECORDCOUNT");
                        })
                );

            return listConstraint;
        }

        public static long UpdateConstraint(ConstraintModel constraintModel, int userID)
        {

            bool result = false;

            long resultConstraintID = 0;

            DateTime? Start = new DateTime();

            DateTime? End = new DateTime();
            CultureInfo provider = CultureInfo.InvariantCulture;

            try
            {
                if (constraintModel.StartDateString != "01/01/0001" && constraintModel.StartDateString != null)
                {
                    if (constraintModel.StartDateString.Contains("/"))
                    {
                        DateTime date = DateTime.ParseExact(constraintModel.StartDateString, "dd/MM/yyyy", provider);
                        Start = DateTime.ParseExact(constraintModel.StartDateString, "dd/MM/yyyy", provider);
                    }
                    else
                    {
                        DateTime date = DateTime.ParseExact(constraintModel.StartDateString, "dd-MM-yyyy", provider);
                        Start = DateTime.ParseExact(constraintModel.StartDateString, "dd-MM-yyyy", provider);
                    }
                }
                else
                {
                    Start = null;
                }

                if (constraintModel.EndDateString != "01/01/0001" && constraintModel.EndDateString != null)
                {

                    if (constraintModel.EndDateString.Contains("/"))
                    {
                        DateTime date = DateTime.ParseExact(constraintModel.EndDateString, "dd/MM/yyyy", provider);
                        End = DateTime.ParseExact(constraintModel.EndDateString, "dd/MM/yyyy", provider);
                    }
                    else
                    {
                        DateTime date = DateTime.ParseExact(constraintModel.EndDateString, "dd-MM-yyyy", provider);
                        End = DateTime.ParseExact(constraintModel.EndDateString, "dd-MM-yyyy", provider);
                    }
                }
                else
                {
                    End = null;
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/UpdateConstraint, Exception: ", ex);

            }


            if (constraintModel.OwnerIsContactFlag == true)
            {
                constraintModel.OwnerIsContact = 1;
            }
            else
            {
                constraintModel.OwnerIsContact = 0;
            }

            if (constraintModel.IsNodeConstraintFlag == true)
            {
                constraintModel.TraversalType = 252001;
            }
            else
            {
                constraintModel.TraversalType = 252002;
            }

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               UserSchema.Portal + ".UPDATE_CONSTRAINT",
                parameter =>
                {
                    parameter.AddWithValue("P_CONSTRAINT_ID",constraintModel.ConstraintId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONSTRAINT_NAME",constraintModel.ConstraintName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONSTRAINT_TYPE",constraintModel.ConstraintTypeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_TOPOLOGY_TYPE",constraintModel.DirectionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_START_DATE", Start, OracleDbType.Date, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_END_DATE", End, OracleDbType.Date, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_HEIGHT_MTRS",constraintModel.MaxHeightMtrs, OracleDbType.Single, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_WIDTH_MTRS",constraintModel.MaxWidthMeters, OracleDbType.Single, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_LEN_MTRS",constraintModel.MaxLengthMeters, OracleDbType.Single, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_MAX_HEIGHT_FT",constraintModel.MaxHeightFT, OracleDbType.Single, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_WIDTH_FT",constraintModel.MaxWidthFeet, OracleDbType.Single, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_LEN_FT",constraintModel.MaxLengthFeet, OracleDbType.Single, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_MAX_GROSS_WEIGHT_KGS",constraintModel.MaxGrossWeightKgs, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT_KGS",constraintModel.MaxAxleWeightKgs, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_OWNER_IS_CONTACT",constraintModel.OwnerIsContact, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_TRAVERSAL_TYPE",constraintModel.TraversalType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USER_ID", userID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        resultConstraintID = records.GetLongOrDefault("CONSTRAINT_ID");
                    }
            );

            return resultConstraintID;
        }

        /// <summary>
        /// Get Constraint list
        /// </summary>
        /// <param name="pageNumber">Page</param>
        /// <param name="pageSize"> size of page</param>
        /// <param name="ConstraintID">Constraint Id </param>
        /// <returns>Return list of caution list</returns>
        public static List<ConstraintModel> GetCautionList(int pageNumber, int pageSize, long ConstraintID)
        {
            //Creating new object for GetCautionList
            List<ConstraintModel> listConstraint = new List<ConstraintModel>();

                //Setup Procedure LIST_Infromation
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    listConstraint,
                   UserSchema.Portal + ".GET_CAUTIONS_LIST",
                    parameter =>
                    {
                        parameter.AddWithValue("P_pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_CONSTRAINT_ID", ConstraintID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.CautionId = records.GetLongOrDefault("CAUTION_ID");
                            instance.CautionName = records.GetStringOrDefault("CAUTION_NAME");
                            instance.SpecificAction = records.GetStringOrDefault("SPECIFIC_ACTION");
                            instance.OrganisationName = records.GetStringOrDefault("ORGNAME");
                            instance.SpecificActionXML = records.GetStringOrDefault("SPECIFIC_ACTION");
                            instance.OwnerOrganisationId = records.GetLongOrDefault("OWNER_ORG_ID");
                            instance.TotalRecordCount = records.GetDecimalOrDefault("TotalRecordCount");
                        }
                );

            return listConstraint;
        }

        public static long DeleteConstraint(long ConstraintID, string UserName)
        {
            bool result = false;
            long ResultConstraintID = 0;

            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
               UserSchema.Portal + ".DELETE_CONSTRAINT_CAUTION",
                parameter =>
                {
                    parameter.AddWithValue("P_ConstraintID", ConstraintID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CautionID", null, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_UserName", UserName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);

                },
                    record =>
                    {
                        ResultConstraintID = record.GetInt32("p_AFFECTED_ROWS");
                    }
            );

            return ResultConstraintID;
        }
        /// <summary>
        /// Delete Caution
        /// </summary>
        /// <param name="CAUTION_ID">Caution id</param>
        /// <returns>ture/false</returns>
        public static int DeleteCaution(long cautionId, string userName)
        {
            int affectedRows = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
               UserSchema.Portal + ".DELETE_CONSTRAINT_CAUTION",
                parameter =>
                {
                    parameter.AddWithValue("P_CONSTRAINT_ID", null, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CautionID", cautionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_UserName", userName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECTEDROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);

                },
                record =>
                {
                    affectedRows = record.GetInt32("P_AFFECTEDROWS");
                }
            );
            return affectedRows;
        }

        public static bool SaveLinkDetails(long constraintId, List<ConstraintReferences> constRefrences)
        {
            if (constRefrences == null)
                return false;
            long i = 1;
            foreach (ConstraintReferences constRef in constRefrences)
            {
                SaveLinkToTempTable(constraintId, (long)constRef.constLink, i++);
            }

            return true;
        }

        #region GetConstraints()
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<RouteConstraints> GetConstraints()
        {
            List<RouteConstraints> constModel = new List<RouteConstraints>();
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    constModel,
                   UserSchema.Portal + ".STP_ROUTE_ASSESSMENT.SP_GET_ALL_CONSTRAINTS",
                    parameter =>
                    {
                        parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {

                            instance.ConstraintId = records.GetLongOrDefault("CONSTRAINT_ID");
                            instance.ConstraintCode = records.GetStringOrDefault("CONSTRAINT_CODE");
                            instance.ConstraintName = records.GetStringOrDefault("CONSTRAINT_NAME");
                            instance.ConstraintType = records.GetStringOrDefault("CONSTRAINT_TYPE");
                            instance.TopologyType = records.GetStringOrDefault("TOPOLOGY_TYPE");
                            instance.TraversalType = records.GetStringOrDefault("TRAVERSAL_TYPE");

                            ////Constraint Geometry
                            instance.ConstraintGeometry = records.GetGeometryOrNull("GEOMETRY") as sdogeometry;
                            instance.ConstraintValue = new ConstraintValues();
                            instance.ConstraintValue.GrossWeight = (int)records.GetInt32OrDefault("GROSS_WEIGHT");
                            instance.ConstraintValue.AxleWeight = (int)records.GetInt32OrDefault("AXLE_WEIGHT");
                            instance.ConstraintValue.MaxHeight = records.GetSingleOrDefault("MAX_HEIGHT_MTRS");
                            instance.ConstraintValue.MaxLength = records.GetSingleOrDefault("MAX_LEN_MTRS");
                            instance.ConstraintValue.MaxWidth = records.GetSingleOrDefault("MAX_WIDTH_MTRS");

                        }
                );

                return constModel;

        }
        #endregion
        #region GetConstraints()
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<ConstraintModel> GetConstraintList(int OrgId, int pageNumber, int pageSize, SearchConstraintsFilter objSearchConstraints)
        {
            int OwnerContact = 0;
            int isvalidConstraint = 0;
            if (objSearchConstraints.IsOwnerContact)
            {
                OwnerContact = 1;
            }
            if (objSearchConstraints.IsValid)
            {
                isvalidConstraint = 1;
            }
            List<ConstraintModel> constModel = new List<ConstraintModel>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                constModel,
               UserSchema.Portal + ".SP_CONSTRAINT_LIST_SEARCH",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_ID", OrgId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONSTRAINT_CODE", objSearchConstraints.ConstraintCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONSTRAINT_NAME", objSearchConstraints.ConstraintName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_OWNER_IS_CONTACT", OwnerContact, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_IsValid", isvalidConstraint, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CntType", objSearchConstraints.ConstraintType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    parameter.AddWithValue("SORT_ORDER", objSearchConstraints.sortOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("PRESET_FILTER", objSearchConstraints.presetFilter, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                },
                    (records, instance) =>
                    {

                        instance.ConstraintId = records.GetLongOrDefault("CONSTRAINT_ID");
                        instance.ConstraintCode = records.GetStringOrDefault("CONSTRAINT_CODE");
                        instance.ConstraintName = records.GetStringOrDefault("CONSTRAINT_NAME");
                        instance.DirectionName= records.GetStringOrDefault("DIRECTIONNAME");
                        instance.ConstraintTypeName= records.GetStringOrDefault("CONSTRAINTTYPENAME");
                        instance.OwnerIsContact = records.GetInt16OrDefault("OWNER_IS_CONTACT");
                        instance.vaildconst = records.GetDecimalOrDefault("IS_VALID");
                        instance.FEasting = records.GetInt32OrDefault("FROM_EASTING");
                        instance.FNorthing = records.GetInt32OrDefault("FROM_NORTHING");
                       // instance.Geometry = records.GetGeometryOrNull("GEOMETRY");
                        instance.TotalRecordCount = records.GetDecimalOrDefault("TOTAL_ROWS");

                    }
            );

            return constModel;

        }
        #endregion
        public static bool FindLinksOfAreaConstraint(sdogeometry polygonGeometry, int organisationId, int userType)
        {
            decimal result = 0;
            #region
            OracleCommand cmd = new OracleCommand();
            OracleParameter oracleGeo = cmd.CreateParameter();
            oracleGeo.OracleDbType = OracleDbType.Object;
            oracleGeo.UdtTypeName = "MDSYS.SDO_GEOMETRY";
            oracleGeo.Value = polygonGeometry;
            #endregion
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
              result,
               UserSchema.Portal + ".SP_FIND_LINKOF_GEOM",
                parameter =>
                {
                    parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USER_TYPE", userType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.Add(oracleGeo);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        result = records.GetDecimalOrDefault("P_CNT");
                    }

            );
            if (result > 0)
                return true;
            else
                return false;

        }

        public static void SaveLinkToTempTable(long CONSTRAINT_ID, long linkID, long linkNo)
        {
            SafeProcedure.DBProvider.Oracle.Execute(
               UserSchema.Portal + ".SP_INSERT_LINKDETAILS",
                parameter =>
                {
                    parameter.AddWithValue("p_CONSTRAINT_ID", CONSTRAINT_ID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LINK_ID", linkID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_CONSTRAINT_LINK_NO", (int)linkNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                }
            );
        }

        public static bool CheckLinkOwnerShip(int organisationId, List<int> linkIDs, bool allLinks)
        {
            if (allLinks)
            {
                foreach (int linkID in linkIDs)
                {
                    if (!CheckLinkOwnerShip(linkID, organisationId))
                        return false;
                }
                return true;
            }
            else
            {
                foreach (int linkID in linkIDs)
                {
                    if (CheckLinkOwnerShip(linkID, organisationId))
                        return true;
                }
                return false;
            }
        }
        public static bool CheckLinkOwnerShip(int organisationId, List<ConstraintReferences> constRefrences, bool allLinks)
        {
            if (constRefrences == null)
                return false;
            if (allLinks)
            {
                foreach (ConstraintReferences constRef in constRefrences)
                {
                    if (!CheckLinkOwnerShip(organisationId, (int)constRef.constLink))
                        return false;
                }
                return true;
            }
            else
            {
                foreach (ConstraintReferences constRef in constRefrences)
                {
                    if (CheckLinkOwnerShip(organisationId, (int)constRef.constLink))
                        return true;
                }
                return false;
            }
        }

        public static bool CheckLinkOwnerShip(int orgID, long linkID)
        {
            bool result = false;

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".SP_CHECK_LINK_ID_OWNERSHIP",
                parameter =>
                {
                    parameter.AddWithValue("P_ORG_ID", orgID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_LINK_ID", linkID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        long cnt = (long)Convert.ToInt32(records["RECCOUNT"]);
                        result = cnt > 0 ? true : false;
                    }
            );
            return result;
        }

        public static bool CheckLinkOwnerShipForPolice(int organisationId, List<ConstraintReferences> constRefrences, bool allLinks)
        {
            if (allLinks)
            {
                foreach (ConstraintReferences constRef in constRefrences)
                {
                    if (!CheckLinkOwnerShipForPolice(organisationId, (int)constRef.constLink))
                        return false;
                }
                return true;
            }
            else
            {
                foreach (ConstraintReferences constRef in constRefrences)
                {
                    if (CheckLinkOwnerShipForPolice(organisationId, (int)constRef.constLink))
                        return true;
                }
                return false;
            }
        }

        public static bool CheckLinkOwnerShipForPolice(int orgID, long linkID)
        {
            bool result = false;
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".SP_CHECK_POLICE_OWNERSHIP",
                parameter =>
                {
                    parameter.AddWithValue("P_ORG_ID", orgID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_LINK_ID", linkID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        long cnt = (long)Convert.ToInt32(records["RECCOUNT"]);
                        result = cnt > 0 ? true : false;
                    }
            );
            return result;
        }

        #region SaveConstraint(ConstraintModel CM , int userID)
        /// <summary>
        /// function to save the constraints in constraints table
        /// </summary>
        /// <param name="constraintModel">Constraint Model class object</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static long SaveConstraint(ConstraintModel constraintModel, int userId)
        {
            bool result = false;
            long resultConstraintID = 0;
            DateTime? start = new DateTime();
            DateTime? end = new DateTime();
            DateTime? end1 = DateTime.Now;
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {

            
            if (constraintModel.StartDateString != "01/01/0001" && constraintModel.StartDateString != null)
            {
                    if (constraintModel.StartDateString.Contains("/"))
                    {
                        DateTime date = DateTime.ParseExact(constraintModel.StartDateString, "dd/MM/yyyy", provider);
                        start = DateTime.ParseExact(constraintModel.StartDateString, "dd/MM/yyyy", provider);
                    }
                    else
                    {
                        DateTime date = DateTime.ParseExact(constraintModel.StartDateString, "dd-MM-yyyy", provider);
                        start = DateTime.ParseExact(constraintModel.StartDateString, "dd-MM-yyyy", provider);
                    }
            }
            else
            {
                start = null;
            }

            if (constraintModel.EndDateString != "01/01/0001" && constraintModel.EndDateString != null)
            {
                
                    if (constraintModel.EndDateString.Contains("/"))
                    {
                        DateTime date = DateTime.ParseExact(constraintModel.EndDateString, "dd/MM/yyyy", provider);
                        end = DateTime.ParseExact(constraintModel.EndDateString, "dd/MM/yyyy", provider);
                    }
                    else
                    {
                        DateTime date = DateTime.ParseExact(constraintModel.EndDateString, "dd-MM-yyyy", provider);
                        end = DateTime.ParseExact(constraintModel.EndDateString, "dd-MM-yyyy", provider);
                    }
                }
            else
            {
                end = null;
            }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/UpdateConstraint, Exception: ", ex);

            }
            if (constraintModel.OwnerIsContactFlag == true)
            {
                constraintModel.OwnerIsContact = 1;
            }
            else
            {
                constraintModel.OwnerIsContact = 0;
            }

            if (constraintModel.IsNodeConstraintFlag == true)
            {
                constraintModel.TraversalType = 252001;
            }
            else
            {
                constraintModel.TraversalType = 252002;
            }

            ///<summary>
            /// code region to generate oracle parameter variable to save SDO_GEOMETRY
            ///</summary>
            #region
            OracleCommand cmd = new OracleCommand();
            OracleParameter oraclePointGeo = cmd.CreateParameter();
            oraclePointGeo.OracleDbType = OracleDbType.Object;
            oraclePointGeo.UdtTypeName = "MDSYS.SDO_GEOMETRY";
            oraclePointGeo.Value = constraintModel.Geometry; //Saving Route point geometry 
            #endregion

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               UserSchema.Portal + ".STP_ROUTE_ASSESSMENT.SP_CREATE_CONSTRAINT",
                parameter =>
                {
                    parameter.AddWithValue("P_CONSTRAINT_NAME", constraintModel.ConstraintName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONSTRAINT_CODE", constraintModel.ConstraintCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONSTRAINT_TYPE", constraintModel.ConstraintTypeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_TOPOLOGY_TYPE", constraintModel.TopologyType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_START_DATE", start, OracleDbType.Date, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_END_DATE", end, OracleDbType.Date, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_HEIGHT_MTRS", constraintModel.MaxHeightMtrs, OracleDbType.Single, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_HEIGHT_FT", constraintModel.MaxHeightFT, OracleDbType.Single, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_WIDTH_MTRS", constraintModel.MaxWidthMeters, OracleDbType.Single, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_WIDTH_FT", constraintModel.MaxWidthFeet, OracleDbType.Single, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_LEN_MTRS", constraintModel.MaxLengthMeters, OracleDbType.Single, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_LEN_FT", constraintModel.MaxLengthFeet, OracleDbType.Single, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_GROSS_WEIGHT_KGS", constraintModel.MaxGrossWeightKgs, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT_KGS", constraintModel.MaxAxleWeightKgs, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_OWNER_IS_CONTACT", constraintModel.OwnerIsContact, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_TRAVERSAL_TYPE", constraintModel.TraversalType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USER_ID", userId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_DIRECTION", constraintModel.Direction, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", constraintModel.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.Add(oraclePointGeo);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        resultConstraintID = records.GetLongOrDefault("CONSTRAINT_ID");
                    }
            );
            //saving constraint related contact details
            foreach (AssessmentContacts assCont in constraintModel.ConstraintContact)
            {
                SaveConstraintContacts(assCont, resultConstraintID);
            }
            //saving constraint references (links) and geometric references

            if (constraintModel.TopologyType == 248003)
            {
                int userType;
                constraintModel.Geometry.ElemArray = new decimal[3] { 1, 1003, 1 };
                constraintModel.Geometry.sdo_gtype = 2003;
                constraintModel.Geometry.sdo_srid = 27700;

                if (constraintModel.UserType == 696002)
                    userType = 2;
                else if (constraintModel.UserType == 696008)
                    userType = 0;
                else
                    userType = 1;

                SaveAreaConstraintLinkReferences(constraintModel.Geometry, resultConstraintID, constraintModel.OrganisationId, userType);
            }
            else
            {
                var i = 1;
                foreach (ConstraintReferences constrRef in constraintModel.ConstraintReferences)
                {
                    constrRef.ConstraintLinkNo = i;    // initialise Constraint link numbers for each Link id in a constraint like 1,2,3...etc
                    i++;
                    SaveConstraintLinkReferences(constrRef, resultConstraintID, constraintModel.IsNodeConstraint);

                }
            }

            return resultConstraintID;

        }
        #endregion

        public static ConstraintModel GetConstraintDetails(int ConstraintId)
        {
            ConstraintModel constraintDetail = new ConstraintModel();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                constraintDetail,
               UserSchema.Portal + ".GET_CONSTRAINT_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_CONSTRAINT_ID", ConstraintId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       instance.ConstraintId = records.GetLongOrDefault("CONSTRAINT_ID");
                       instance.OrganisationName = records.GetStringOrDefault("ORGNAME");
                       instance.OwnerOrganisationId = records.GetLongOrDefault("ORGANISATION_ID");

                       instance.ConstraintName = records.GetStringOrDefault("CONSTRAINT_NAME");
                       instance.ConstraintCode = records.GetStringOrDefault("CONSTRAINT_CODE");
                       instance.ConstraintTypeName = records.GetStringOrDefault("ConstraintTypeName");
                       instance.DirectionName = records.GetStringOrDefault("DIRECTIONNAME");
                       instance.StartDate = records.GetDateTimeOrDefault("START_DATE");
                       instance.EndDate = records.GetDateTimeOrDefault("END_DATE");

                       instance.MaxHeightMtrs = records.GetDataTypeName("MAX_HEIGHT_MTRS") == "Single" ? records.GetSingleOrDefault("MAX_HEIGHT_MTRS") : Convert.ToSingle(records.GetDecimalOrDefault("MAX_HEIGHT_MTRS"));

                       instance.MaxHeightFT = records.GetDataTypeName("MAX_HEIGHT_FT") == "Single" ? records.GetSingleOrDefault("MAX_HEIGHT_FT") : Convert.ToSingle(records.GetDecimalOrDefault("MAX_HEIGHT_FT"));


                       instance.MaxWidthMeters = records.GetDataTypeName("MAX_WIDTH_MTRS") == "Single" ? records.GetSingleOrDefault("MAX_WIDTH_MTRS") : Convert.ToSingle(records.GetDecimalOrDefault("MAX_WIDTH_MTRS"));

                       instance.MaxWidthFeet = records.GetDataTypeName("MAX_WIDTH_FT") == "Single" ? records.GetSingleOrDefault("MAX_WIDTH_FT") : Convert.ToSingle(records.GetDecimalOrDefault("MAX_WIDTH_FT"));


                       instance.MaxLengthMeters = records.GetDataTypeName("MAX_LEN_MTRS") == "Single" ? records.GetSingleOrDefault("MAX_LEN_MTRS") : Convert.ToSingle(records.GetDecimalOrDefault("MAX_LEN_MTRS"));

                       instance.MaxLengthFeet = records.GetDataTypeName("MAX_LEN_FT") == "Single" ? records.GetSingleOrDefault("MAX_LEN_FT") : Convert.ToSingle(records.GetDecimalOrDefault("MAX_LEN_FT"));

                       instance.MaxGrossWeightKgs = records.GetInt32OrDefault("MAX_GROSS_WEIGHT_KGS");
                       instance.MaxAxleWeightKgs = records.GetInt32OrDefault("MAX_AXLE_WEIGHT_KGS");

                       instance.OwnerIsContact = records.GetInt16OrDefault("OWNER_IS_CONTACT");
                       instance.IsNodeConstraint = records.GetInt32OrDefault("TRAVERSAL_TYPE");

                       instance.DirectionId = records.GetInt32OrDefault("TOPOLOGY_TYPE");
                       instance.ConstraintTypeId = records.GetInt32OrDefault("CONSTRAINT_TYPE");

                       instance.HdnConstraintName = instance.ConstraintName;
                       instance.HdnConstraintTypeId = instance.ConstraintTypeId;
                       instance.HdnDirectionId = instance.DirectionId;

                       instance.HdnStartDate = instance.StartDate;
                       instance.HdnEndDate = instance.EndDate;

                       instance.HdnMaxHeightMtrs = instance.MaxHeightMtrs;
                       instance.HdnMaxWidthMeters = instance.MaxWidthMeters;
                       instance.HdnMaxLenMeters = instance.MaxLengthMeters;

                       instance.HdnMaxGrossWeightKgs = instance.MaxGrossWeightKgs;
                       instance.HdnMaxAxleWeightKgs = instance.MaxAxleWeightKgs;

                       instance.HdnOwnerIsContactFlag = instance.OwnerIsContactFlag;
                       instance.HdnIsNodeConstraintFlag = instance.IsNodeConstraintFlag;

                       List<ConstraintContactModel> contactsList = GetConstraintContactList(1, int.MaxValue, ConstraintId,0);
                       if (contactsList!=null&& contactsList.Any())
                       {
                           instance.ConstraintContact = contactsList.Select(x => new AssessmentContacts()
                           {
                               ContactName = x.FullName,
                               OrganisationName = x.OrganisationName,
                               Country = x.CountryName,
                               Mobile = x.Mobile,
                               Telephone = x.Telephone,
                               Email = x.Email,
                               AddressLine = new List<string>() { x.AddressLine1 }
                           }).ToList();
                       }
                   }
            );

            if (constraintDetail.OwnerIsContact == 1)
            {
                constraintDetail.OwnerIsContactFlag = true;
                constraintDetail.HdnOwnerIsContactFlag = true;
            }

            if (constraintDetail.IsNodeConstraint == 252001)
            {
                constraintDetail.IsNodeConstraintFlag = true;
                constraintDetail.HdnIsNodeConstraintFlag = true;
            }

            if (constraintDetail.StartDate.ToShortDateString() != "01/01/0001")
            {
                constraintDetail.StartDateString = constraintDetail.StartDate.ToString("dd/MM/yyyy");
                constraintDetail.HdnStartDateString = constraintDetail.StartDate.ToString("dd/MM/yyyy");
            }

            if (constraintDetail.EndDate.ToShortDateString() != "01/01/0001")
            {
                constraintDetail.EndDateString = constraintDetail.EndDate.ToString("dd/MM/yyyy");
                constraintDetail.HdnEndDateString = constraintDetail.EndDate.ToString("dd/MM/yyyy");
            }
            return constraintDetail;
        }

        #region SaveConstraintContacts(AssessmentContacts contact, long constrId)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="constrId"></param>
        /// <returns></returns>
        public static long SaveConstraintContacts(AssessmentContacts contact, long constrId)
        {
            long result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               UserSchema.Portal + ".STP_ROUTE_ASSESSMENT.SP_CREATE_CONSTRAINT",
                parameter =>
                {
                    parameter.AddWithValue("P_CONSTRAINT_ID", constrId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTACT_ID", contact.ContactId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", contact.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ROLE_TYPE", contact.RoleTypeCode, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_DESCR", contact.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AD_HOC", contact.IsAdHoc, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_FULL_NAME", contact.ContactName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_NAME", contact.OrganisationName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ADDR_1", contact.AddressLine[0], OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ADDR_2", contact.AddressLine[1], OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ADDR_3", contact.AddressLine[2], OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ADDR_4", contact.AddressLine[3], OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ADDR_5", contact.AddressLine[4], OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_COUNTRY", contact.CountryCode, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_POSTCODE", contact.PostCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_TELEPHONE", contact.Telephone, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_EXTENSION", contact.Extension, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MOBILE", contact.Mobile, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_FAX", contact.Fax, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_EMAIL", contact.Email, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_EMAIL_PREFERENCES", null, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        result = records.GetLongOrDefault("CONSTRAINT_ID");
                    }
            );
            return result;
        }
        #endregion

        #region SaveAreaConstraintLinkReferences(sdogeometry polygonGeometry, long constrId, long orgId, int userType)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="polygonGeometry"></param>
        /// <param name="constrId"></param>
        /// <param name="organisationId"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static bool SaveAreaConstraintLinkReferences(sdogeometry polygonGeometry, long constrId, long organisationId, int userType)
        {
            bool result = false;
            
            #region
            OracleCommand cmd = new OracleCommand();
            OracleParameter oracleGeo = cmd.CreateParameter();
            oracleGeo.OracleDbType = OracleDbType.Object;
            oracleGeo.UdtTypeName = "MDSYS.SDO_GEOMETRY";
            oracleGeo.Value = polygonGeometry;
            #endregion
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
              result,
             UserSchema.Portal + ".SP_INSERT_AREA_CONSTRAINT",
              parameter =>
              {
                  parameter.AddWithValue("P_CONSTRAINT_ID", constrId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_ORGID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_USER_TYPE", userType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                  parameter.Add(oracleGeo);
                  parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
              },
                  (records, instance) =>
                  {
                      //result = records.GetLongOrDefault("P_RESULTSET");
                      if (records.FieldCount > 0)
                      {
                          result = true;
                      }

                  }
          );
            return result;
        }
        #endregion

        #region SaveConstraintLinkReferences(ConstraintReferences constrRef, long constrId,int isNode)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="constrReference"></param>
        /// <param name="constrId"></param>
        /// <param name="isNode"></param>
        /// <returns></returns>
        public static long SaveConstraintLinkReferences(ConstraintReferences constrReference, long constrId, int isNode)
        {
            long result = 0;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
              result,
             UserSchema.Portal + ".STP_ROUTE_ASSESSMENT.SP_CREATE_CONSTR_REFERENCE",
              parameter =>
              {
                  parameter.AddWithValue("P_CONSTRAINT_ID", constrId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_LINK_ID", constrReference.constLink, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_EASTING", constrReference.Easting, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_NORTHING", constrReference.Northing, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_LINEAR_REF", constrReference.LinearRef, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_FROM_LINEAR_REF", constrReference.FromLinearRef, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_TO_LINEAR_REF", constrReference.ToLinearRef, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_FROM_EASTING", constrReference.FromEasting, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_FROM_NORTHING", constrReference.FromNorthing, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_TO_EASTING", constrReference.ToEasting, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_TO_NORTHING", constrReference.ToNorthing, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_ISPOINT", constrReference.IsPoint, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_DIRECTION", constrReference.Direction, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_NODE_FLAG", isNode, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("CONSTR_LINK_NO", constrReference.ConstraintLinkNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
              },
                  (records, instance) =>
                  {
                      result = records.GetLongOrDefault("CONSTRAINT_ID");
                  }
          );

            return result;
        }
        #endregion

        public static ConstraintModel GetCautionDetails(long cautionID)
        {
            ConstraintModel cautionDetail = new ConstraintModel();
            long contactId = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                cautionDetail,
               UserSchema.Portal + ".GET_CAUTIONS_DETAIL",
                parameter =>
                {
                    parameter.AddWithValue("P_Caution_ID", cautionID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       instance.ConstraintId = records.GetLongOrDefault("CONSTRAINT_ID");
                       instance.ConstraintCode = records.GetStringOrDefault("CONSTRAINT_CODE");
                       instance.ConstraintName = records.GetStringOrDefault("CONSTRAINT_NAME");
                       instance.OrganisationName = records.GetStringOrDefault("ORGNAME");
                       instance.OwnerOrganisationId = records.GetLongOrDefault("OWNER_ORG_ID");

                       instance.CautionId = records.GetLongOrDefault("CAUTION_ID");
                       instance.CautionName = records.GetStringOrDefault("CAUTION_NAME");

                       instance.SpecificAction = records.GetStringOrDefault("SPECIFIC_ACTION");

                       instance.MaxHeightMtrs = records.GetSingleOrDefault("MAX_HEIGHT_MTRS");
                       instance.MaxWidthMeters = records.GetSingleOrDefault("MAX_WIDTH_MTRS");
                       instance.MaxLengthMeters = records.GetSingleOrDefault("MAX_LENGTH_MTRS");

                       instance.MaxHeight = records.GetFloatOrDefault("MAX_HEIGHT");
                       instance.MaxWidth = records.GetFloatOrDefault("MAX_WIDTH");
                       instance.MaxLength = records.GetFloatOrDefault("MAX_LENGTH");
                       //-----------------------------------------------------------------------------
                       instance.MaxGrossWeightKgs = records.GetInt32OrDefault("MAX_GROSS_WEIGHT_KGS");
                       instance.MaxAxleWeightKgs = records.GetInt32OrDefault("MAX_AXLE_WEIGHT_KGS");
                       instance.MinSpeedKph = records.GetSingleOrDefault("MIN_SPEED_KPH");

                       instance.MaxGrossWeight = records.GetDoubleOrDefault("MAX_GROSS_WEIGHT");
                       instance.MaxAxleWeight = records.GetDoubleOrDefault("MAX_AXLE_WEIGHT");
                       instance.MinSpeed = records.GetSingleOrDefault("MIN_SPEED");
                       //----------------------------------------------------------------------------------------------
                       instance.MaxLengthMeters = records.GetSingleOrDefault("MAX_LENGTH_MTRS");
                       contactId = records.GetLongOrDefault("contact_id");

                   }
            );
            if (contactId != 0)
            {
                cautionDetail.OwnerIsContactFlag = true;
            }
            return cautionDetail;

        }

        /// <summary>
        /// Save cautions
        /// </summary>
        /// <param name="constraintModel">ConstraintModel</param>
        /// <returns>Save cautions</returns>
        public static bool SaveCautions(ConstraintModel constraintModel)
        {
            bool result = false;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               UserSchema.Portal + ".MANAGE_CAUTION",
                parameter =>
                {
                    parameter.AddWithValue("P_CAUTION_ID", constraintModel.CautionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CAUTION_NAME", constraintModel.CautionName, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONSTRAINT_ID", constraintModel.ConstraintId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_OWNER_ORG_ID", constraintModel.OwnerOrganisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SPECIFIC_ACTION", constraintModel.SpecificAction, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_GROSS_WEIGHT_KGS", (double)constraintModel.MaxGrossWeightKgs, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_MAX_GROSS_WEIGHT", constraintModel.MaxGrossWeight, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_GROSS_WEIGHT_UNIT", constraintModel.MaxGrossWeightUnit, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT_KGS", (double)constraintModel.MaxAxleWeightKgs, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT", constraintModel.MaxAxleWeight, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT_UNIT", constraintModel.MaxAxleWeightUnit, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_MAX_HEIGHT_MTRS", constraintModel.MaxHeightMtrs, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_HEIGHT", constraintModel.MaxHeight, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_HEIGHT_UNIT", constraintModel.MaxHeightUnit, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_MAX_LENGTH_MTRS", constraintModel.MaxLengthMeters, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_LENGTH", constraintModel.MaxLength, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_LENGTH_UNIT", constraintModel.MaxLengthUnit, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_MAX_WIDTH_MTRS", constraintModel.MaxWidthMeters, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_WIDTH", constraintModel.MaxWidth, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_WIDTH_UNIT", constraintModel.MaxWidthUnit, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_MIN_SPEED_KPH", constraintModel.MinSpeedKph, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MIN_SPEED", constraintModel.MinSpeed, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MIN_SPEED_UNIT", constraintModel.MinSpeedUnit, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_OWNER_IS_CONTACT", constraintModel.OwnerIsContact, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MODE", constraintModel.Mode, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        constraintModel.CautionId = Convert.ToInt64(records.GetDecimalOrDefault("CAUTION_ID"));
                        result = true;
                    }
            );
            return result;
        }

        /// <summary>
        /// Update constraint log
        /// </summary>
        /// <param name="constraintLogsModel">list of ConstraintLogModel</param>
        /// <returns>Update modification in Constraint_log table</returns>
        public static bool UpdateConstraintLog(List<ConstraintLogModel> constraintLogsModel)
        {

                bool result = false;
                foreach (ConstraintLogModel constraintLogModel in constraintLogsModel)
                {
                    result = false;
                    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                        result,
                       UserSchema.Portal + ".MANAGE_CONSTRAINT_LOGS",
                        parameter =>
                        {
                            parameter.AddWithValue("p_AUTHOR", constraintLogModel.Author, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("p_AMENDMENT_1", constraintLogModel.Amendment1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("p_AMENDMENT_3", constraintLogModel.Amendment3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("p_CONSTRAINT_CODE", constraintLogModel.ConstraintCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("p_AMENDMENT_2", constraintLogModel.Amendment2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        },
                            (records, instance) =>
                            {
                                result = true;
                            }
                    );
                }
                return result;
        }

        /// <summary>
        /// Review contacts list
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="constraintId">Constraint id</param>
        /// <param name="contactNo">Contact no</param>
        /// <returns>Get contact list</returns>
        public static List<ConstraintContactModel> GetConstraintContactList(int pageNumber, int pageSize, long constraintId, short contactNo)
        {
            //Creating new object for GetCautionList
            List<ConstraintContactModel> listConstraintContact = new List<ConstraintContactModel>();

                //Setup Procedure LIST_Infromation
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    listConstraintContact,
                   UserSchema.Portal + ".GET_CONSTRAINT_CONTACT_LIST",
                    parameter =>
                    {
                        parameter.AddWithValue("P_pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_CONSTRAINT_ID", constraintId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_CONTACT_NO", contactNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.ConstraintId = records.GetLongOrDefault("CONSTRAINT_ID");
                            instance.ContactNo = records.GetInt16OrDefault("CONTACT_NO");
                            instance.Description = records.GetStringOrDefault("DESCRIPTION");
                            instance.FullName = records.GetStringOrDefault("FULL_NAME");
                            instance.OrganisationName = records.GetStringOrDefault("ORG_NAME");
                            instance.AddressLine1 = records.GetStringOrDefault("ADDRESS_LINE_1");
                            instance.AddressLine2 = records.GetStringOrDefault("ADDRESS_LINE_2");
                            instance.AddressLine3 = records.GetStringOrDefault("ADDRESS_LINE_3");
                            instance.AddressLine4 = records.GetStringOrDefault("ADDRESS_LINE_4");
                            instance.AddressLine5 = records.GetStringOrDefault("ADDRESS_LINE_5");
                            instance.PostCode = records.GetStringOrDefault("POST_CODE");
                            instance.CountryId = records.GetInt32OrDefault("COUNTRY");
                            instance.Telephone = records.GetStringOrDefault("TELEPHONE");
                            instance.Extension = records.GetStringOrDefault("EXTENSION");
                            instance.Mobile = records.GetStringOrDefault("MOBILE");
                            instance.Fax = records.GetStringOrDefault("FAX");
                            instance.Email = records.GetStringOrDefault("EMAIL");
                            instance.EmailPreference = records.GetStringOrDefault("EMAIL_PREFERENCE");
                            instance.TotalRecordCount = records.GetDecimalOrDefault("TotalRecordCount");
                        }
                );
            return listConstraintContact;

        }

        /// <summary>
        /// Get Constraint list
        /// </summary>
        /// <param name="pageNumber">Page</param>
        /// <param name="pageSize"> size of page</param>
        /// <param name="ConstraintID">Constraint Id </param>
        /// <returns>Return list of caution list</returns>
        public static List<ConstraintModel> GetNotificationExceedingConstraint(int pageNumber, int pageSize, long ConstraintID, int UserID)
        {
            List<ConstraintModel> notificationExceedConstraint = new List<ConstraintModel>();

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    notificationExceedConstraint,
                   UserSchema.Portal + ".GET_NOTI_EXCEEDING_CONSTR",
                    parameter =>
                    {
                        parameter.AddWithValue("P_CONSTRAINT_ID", ConstraintID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_USER_ID", UserID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.RevisionId = records.GetLongOrDefault("REVISION_ID");
                            instance.RevisionNo = Convert.ToInt16(records.GetInt32OrDefault("REVISION_NO"));
                            instance.ReceivedDate = records.GetDateTimeOrDefault("NOTIFICATION_DATE");
                            instance.MovementStartDate = records.GetDateTimeOrDefault("MOVE_START_DATE");
                            instance.MovementEndDate = records.GetDateTimeOrDefault("MOVE_END_DATE");
                            instance.HaulierName = records.GetStringOrDefault("HAULIER_NAME");
                            instance.ESDALReference = records.GetStringOrDefault("ESDAL_REFERENCE");
                            instance.CountRecord = records.GetDecimalOrDefault("COUNTREC");
                            instance.ApplicationStatus = records.GetInt32OrDefault("ICA_STATUS");
                            instance.VersionId = records.GetLongOrDefault("VERSION_ID");
                        }
                );
                return notificationExceedConstraint;
        }

        #region getConstraintListForOrg(int orgId)
        /// <summary>
        /// function to get constraint list for a organisationid 
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="routeType"></param>
        /// <returns></returns>
        public static List<RouteConstraints> getConstraintListForOrg(int organisationId, string userSchema, int otherOrganisation, int left, int right, int bottom, int top)
        {
           List<RouteConstraints> routeConst = new List<RouteConstraints>();

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                        routeConst,
                        userSchema + ".SP_R_LIST_CONSTRAINT_ORG",
                        parameter =>
                        {
                            parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_SHOW_OTHER", otherOrganisation, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_FROM_EASTING", left, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_FROM_NORTHING", top, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_TO_EASTING", right, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_TO_NORTHING", bottom, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                        },
                            (records, instance) =>
                            {
                                instance.ConstraintId = records.GetLongOrDefault("CONSTRAINT_ID");
                                instance.ConstraintCode = records.GetStringOrDefault("CONSTRAINT_CODE");
                                instance.ConstraintName = records.GetStringOrDefault("CONSTRAINT_NAME");
                                instance.ConstraintType = records.GetStringOrDefault("CONSTRAINT_TYPE");
                                instance.TopologyType = records.GetStringOrDefault("TOPOLOGY_TYPE");
                                instance.TraversalType = records.GetStringOrDefault("TRAVERSAL_TYPE");
                                instance.ConstraintGeometry = records.GetGeometryOrNull("GEOMETRY") as sdogeometry;
                                instance.ConstraintValue = new ConstraintValues();
                                instance.ConstraintValue.GrossWeight = (int)records.GetInt32OrDefault("GROSS_WEIGHT");
                                instance.ConstraintValue.AxleWeight = (int)records.GetInt32OrDefault("AXLE_WEIGHT");
                                instance.ConstraintValue.MaxHeight = records.GetSingleOrDefault("MAX_HEIGHT_MTRS");
                                instance.ConstraintValue.MaxLength = records.GetSingleOrDefault("MAX_LEN_MTRS");
                                instance.ConstraintValue.MaxWidth = records.GetSingleOrDefault("MAX_WIDTH_MTRS");

                            }
                    );
            return routeConst;
        }
        #endregion

        /// <summary>
        /// Save constraint contact
        /// </summary>
        /// <param name="constraintContact">Save constraint contact</param>
        /// <returns>Save constraint contact</returns>
        public static bool SaveConstraintContact(ConstraintContactModel constraintContact)
        {
            bool result = false;

            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
               UserSchema.Portal + ".MANAGE_CONSTRAINT_CONTACT",
                parameter =>
                {
                    parameter.AddWithValue("p_FULL_NAME", constraintContact.FullName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_DESCRIPTION", constraintContact.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORG_NAME", constraintContact.OrganisationName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COUNTRY", constraintContact.CountryId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESS_LINE_5", constraintContact.AddressLine5, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    //parameter.AddWithValue("p_USER_SCHEMA", constraintContact.UserSchema, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESS_LINE_4", constraintContact.AddressLine4, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_CONTACT_NO", constraintContact.ContactNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_CONTACT_ID", constraintContact.ContactId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESS_LINE_3", constraintContact.AddressLine3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TELEPHONE", constraintContact.Telephone, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESS_LINE_2", constraintContact.AddressLine2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESS_LINE_1", constraintContact.AddressLine1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGANISATION_ID", constraintContact.OrganisationId, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ROLE_TYPE", constraintContact.RoleType, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MOBILE", constraintContact.Mobile, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_POST_CODE", constraintContact.PostCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_EMAIL", constraintContact.Email, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_EMAIL_PREFERENCE", constraintContact.EmailPreference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_EXTENSION", constraintContact.Extension, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FAX", constraintContact.Fax, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_CONSTRAINT_ID", constraintContact.ConstraintId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_AD_HOC", constraintContact.IsAdHoc, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_Affected_Rows", null, OracleDbType.Int32, ParameterDirectionWrap.Output);

                },
                    record =>
                    {
                        result = true;

                    }
            );
            return result;
        }

        /// <summary>
        /// Deleter constraint contact
        /// </summary>
        /// <param name="contactNo">Contact no</param>
        /// <param name="constraintId">Constraint id</param>
        /// <returns>Delete constrain contact </returns>
        public static int DeleteContact(short contactNo, long constraintId)
        {
            int affectedRows = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
               UserSchema.Portal + ".DELETE_CONSTRAINT_CONTACT",
                parameter =>
                {
                    parameter.AddWithValue("P_CONSTRAINT_ID", constraintId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTACT_NO", contactNo, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECTEDROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);

                },
                record =>
                {
                    affectedRows = record.GetInt32("P_AFFECTEDROWS");
                }
            );
            return affectedRows;
        }

        #region GetNotifAffectedStructuresConstraint
        public static RouteAssessmentModel GetNotifAffectedStructuresConstraint(int notificationId, string esdalReferenceNo, string haulierMnemonic, string versionNo, string userSchema = UserSchema.Portal, int inboxId = 0)
        {
            RouteAssessmentModel assessmentModelObj = new RouteAssessmentModel();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    assessmentModelObj,
                    userSchema + ".SP_GET_AFFECTED_STUCT_CONSTR",
                    parameter =>
                    {
                        parameter.AddWithValue("P_NOTIF_ID", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ESDAL_REF_NO", Convert.ToInt32(esdalReferenceNo), OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_HAULIER_MNEMONIC", haulierMnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_VERSION_NO", Convert.ToInt32(versionNo), OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_INBOX_ID", inboxId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);//FOR NEN PROJECT
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                    },
                    record =>
                    {
                        assessmentModelObj.AffectedStructure = record.GetByteArrayOrNull("AFFECTED_STRUCTURES");
                        assessmentModelObj.Constraints = record.GetByteArrayOrNull("AFFECTED_CONSTRAINTS");
                    }
                );
            return assessmentModelObj;
        }
        /// <summary>
        /// NEN R2 overriding procedure call
        /// </summary>
        /// <param name="inboxId"></param>
        /// <param name="organisationId"></param>
        /// <returns></returns>
        public static RouteAssessmentModel GetNotifAffectedStructuresConstraint(int inboxId, int organisationId)
        {
            RouteAssessmentModel assessmentModelObj = new RouteAssessmentModel();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    assessmentModelObj,
                   UserSchema.Portal + ".STP_NEN_INWARD_PROC.SP_GET_AFFECTED_STUCT_CONSTR",
                    parameter =>
                    {
                        parameter.AddWithValue("P_INBOX_ID", inboxId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);//FOR NEN PROJECT
                        parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                    },
                    record =>
                    {
                        assessmentModelObj.AffectedStructure = record.GetByteArrayOrNull("AFFECTED_STRUCTURES");
                        assessmentModelObj.Constraints = record.GetByteArrayOrNull("AFFECTED_CONSTRAINTS");
                    }
                );
            return assessmentModelObj;
        }
        #endregion

    }
}