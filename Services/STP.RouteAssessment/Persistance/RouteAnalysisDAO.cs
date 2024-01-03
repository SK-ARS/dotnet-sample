using NetSdoGeometry;
using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.DataAccess.SafeProcedure;
using STP.Domain.RouteAssessment;
using STP.Domain.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static STP.Domain.Routes.RouteModel;

namespace STP.RouteAssessment.Persistance
{
    public class RouteAnalysisDAO
    {
        #region FetchMovementStatus(int versionId,string ContentRefNo, string userSchema)
        /// <summary>
        /// portion to fetch movement status.
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        internal static long FetchMovementStatus(int versionId, string contentReferenceNo, string userSchema)
        {
            long result = 0;
           
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    result,
                    userSchema + ".SP_MOVEMENT_VER_STATUS_CHECK",
                    parameter =>
                    {
                        parameter.AddWithValue("P_VER_ID", versionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_CONT_REF_NO", contentReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            result = records.GetInt32OrDefault("VERSION_STATUS");
                        }
                );
          
            return result;
        }
        #endregion

        #region  GetAffectedStructureList(int routePartId,int routeType, RoutePart routePart, string userSchema=UserSchema.Portal)
        /// <summary>
        /// function to fetch affected structure instantly with their suitability this funtion is applicable for application side routes and vehicle's saved in route vehicle's table
        /// </summary>
        /// <param name="routePartId">Route Id over which vehicle's are saved in route vehicles / 0 for library route affected structure's</param>
        /// <param name="routePart">Route part object that has route related information</param>
        /// <param name="userSchema">Default user schema</param>
        /// <returns></returns>
        public static List<StructureInfo> GetAffectedStructureList(int routePartId, RoutePart routePart, string userSchema = UserSchema.Portal)
        {
            List<long?> routePointLinkId = new List<long?>();

            Dictionary<long, int[]> linkList = new System.Collections.Generic.Dictionary<long, int[]>();

            List<long?> routeLinkId = new List<long?>();


            int[] idInfo = null;

            foreach (RoutePath routePathObj in routePart.RoutePathList)
            {
                //Start 
                foreach (RoutePoint routePointObj in routePathObj.RoutePointList.Where(t => t.PointType == 0).ToList())
                {
                    routePointLinkId.Add(routePointObj.LinkId);

                    idInfo = new int[3];

                    idInfo[0] = (int)routePointObj.Lrs;      //LRS measure of point
                    idInfo[1] = (int)routePointObj.Direction; //direction of link Id
                    idInfo[2] = 0;  //point type 0 for start

                    linkList.Add(routePointObj.LinkId, idInfo);
                }

                //Middle portion
                foreach (RouteSegment routeSegObj in routePathObj.RouteSegmentList)
                {
                    foreach (RouteLink routeLinkObj in routeSegObj.RouteLinkList.OrderBy(t => t.LinkNo))
                    {
                        routeLinkId.Add(routeLinkObj.LinkId);
                    }
                }

                //End 
                foreach (RoutePoint routePointObj in routePathObj.RoutePointList.Where(t => t.PointType == 1).ToList())
                {
                    routePointLinkId.Add(routePointObj.LinkId);

                    idInfo = new int[3];

                    idInfo[0] = (int)routePointObj.Lrs; //LRS measure of point
                    idInfo[1] = (int)routePointObj.Direction;   //direction of link id
                    idInfo[2] = 1;  // point type 1 for end

                    linkList.Add(routePointObj.LinkId, idInfo);
                }

            }

            var linkId = routeLinkId.Except(routePointLinkId);

            routeLinkId = linkId.ToList();

            List<StructureInfo> structAtFlags = new List<StructureInfo>();

            OracleCommand cmd = new OracleCommand();

            #region Portion to check all structures at all other point's
            //creating associative array parameter to pass to stored procedure
            OracleParameter param = new OracleParameter(); // cmd.CreateParameter();
            param.OracleDbType = OracleDbType.Int32;
            param.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
            param.Value = routeLinkId.ToArray(); // change when the testing is completed
            param.Size = routeLinkId.ToArray().Length;

            List<StructureInfo> structInfoList = GetStructureInfoList(routePartId, param, userSchema);

            #endregion

            #region find structures at start and end point's
            //Portion to check whether the structure's at point are affected or not.
            OracleParameter pointParam = new OracleParameter();
            pointParam.OracleDbType = OracleDbType.Int32;
            pointParam.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
            pointParam.Value = linkList.Keys.ToArray();
            pointParam.Size = linkList.Keys.ToArray().Length;

            List<StructureInfo> structAtPoints = GetStructureInfoAtPoint(routePartId, pointParam, linkList, userSchema);

            #endregion

            if (structAtPoints.Count != 0)
            {
                structInfoList.AddRange(structAtPoints);
            }



            return structInfoList;
        }
        #endregion

        #region GetStructureInfoList(int routePartId, OracleParameter param, string userSchema)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="routePartId"></param>
        /// <param name="param"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        private static List<StructureInfo> GetStructureInfoList(int routePartId, OracleParameter param, string userSchema)
        {
            List<StructureInfo> tmpStructInfoList = new List<StructureInfo>();
            List<StructureInfo> structInfoList = new List<StructureInfo>();

            long tmpOrgId1 = 0, tmpOrgId2 = 0;
            long structId = 0, structIdTemp = 0, sectionId = 0, sectionIdTemp = 0, routeVar = 0, routeVarTmp = 0;
            int suit = 0, cbp = 0, margSuit = 0, unSuit = 0;
            string appraisalTemp = null;
            string str = null;

            StructureInfo StructInfo = null;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    tmpStructInfoList,
                    userSchema + ".STP_LINK_ID_ARRAY.SP_SELECT_STRUCT_INSTANT",
                    parameter =>
                    {
                        parameter.AddWithValue("P_ROUTE_ID", routePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.Add(param);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    (records, instance) =>
                    {
                        structId = records.GetLongOrDefault("STRUCTURE_ID");
                        sectionId = records.GetLongOrDefault("SECTION_ID");
                        tmpOrgId2 = records.GetLongOrDefault("OWNER_ID"); //owner of the structure

                        if (structId != structIdTemp)
                        {
                            StructInfo = new StructureInfo();

                            structInfoList.Add(StructInfo);

                            StructInfo.StructureId = structId; // saving structure id

                            StructInfo.SectionId = sectionId; //saving section id

                            StructInfo.SectionNo = records.GetLongOrDefault("SECTION_NO");

                            StructInfo.StructureName = records.GetStringOrDefault("STRUCTURE_NAME");

                            StructInfo.StructureCode = records.GetStringOrDefault("STRUCTURE_CODE");

                            StructInfo.StructureClass = records.GetStringOrDefault("SECTION_CLASS");

                            StructInfo.StructureDescr = records.GetStringOrDefault("DESCRIPTION");

                            StructInfo.PointGeometry = records.GetGeometryOrNull("POINT_GEOMETRY") as sdogeometry;

                            StructInfo.LineGeometry = records.GetGeometryOrNull("LINE_GEOMETRY") as sdogeometry;

                            str = StructInfo.PointGeometry.AsText;

                            StructInfo.Point = StructInfo.PointGeometry.sdo_point;

                            if (StructInfo.Suitability == null)
                            {
                                StructInfo.Suitability = new List<string>();
                            }
                            StructInfo.Suitability.Add(records.GetStringOrDefault("SECTION_SUITABILITY"));

                            tmpOrgId1 = tmpOrgId2;

                            structIdTemp = structId;

                        }
                        if (structId == structIdTemp && tmpOrgId1 != tmpOrgId2)
                        {
                            tmpOrgId1 = records.GetLongOrDefault("OWNER_ID");

                            appraisalTemp = records.GetStringOrDefault("SECTION_SUITABILITY");

                            if (StructInfo.Suitability == null)
                            {
                                StructInfo.Suitability = new List<string>();
                            }
                            StructInfo.Suitability.Add(appraisalTemp);
                        }

                    }
                );

            return structInfoList;
        }
        #endregion

        #region GetStructureInfoAtPoint(int routePartId, OracleParameter param, Dictionary<long, int[]> linkList , string userSchema)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="routePartId"></param>
        /// <param name="param"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        private static List<StructureInfo> GetStructureInfoAtPoint(int routePartId, OracleParameter param, Dictionary<long, int[]> linkList, string userSchema)
        {
            List<StructureInfo> tmpStructInfoList = new List<StructureInfo>();
            List<StructureInfo> structInfoList = new List<StructureInfo>();
            bool result = false;
            long tmpOrgId1 = 0, tmpOrgId2 = 0;
            long structId = 0, structIdTemp = 0, sectionId = 0, linkId = 0;
            int? dir, ptlrs, fLrs;
            int ptType = 0;

            string appraisalTemp = null;
            string str = null;

            StructureInfo StructInfo = null;


            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    tmpStructInfoList,
                    userSchema + ".STP_LINK_ID_ARRAY.SP_SELECT_STRUCT_INSTANT",
                    parameter =>
                    {
                        parameter.AddWithValue("P_ROUTE_ID", routePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.Add(param);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    (records, instance) =>
                    {
                        structId = records.GetLongOrDefault("STRUCTURE_ID");
                        sectionId = records.GetLongOrDefault("SECTION_ID");
                        tmpOrgId2 = records.GetLongOrDefault("OWNER_ID"); //owner of the structure

                        linkId = records.GetLongOrDefault("LINK_ID");
                        fLrs = records.GetInt32OrDefault("FROM_LINEAR_REF");
                        if (linkList.ContainsKey(linkId))
                        {
                            ptlrs = linkList[linkId][0];
                            dir = linkList[linkId][1];
                            ptType = linkList[linkId][2];

                            result = IsAffectedPortion(dir, ptlrs, fLrs, ptType);
                        }
                        if (result)
                        {
                            if (structId != structIdTemp)
                            {
                                StructInfo = new StructureInfo();

                                structInfoList.Add(StructInfo);

                                StructInfo.StructureId = structId; // saving structure id

                                StructInfo.SectionId = sectionId; //saving section id

                                StructInfo.SectionNo = records.GetLongOrDefault("SECTION_NO");

                                StructInfo.StructureName = records.GetStringOrDefault("STRUCTURE_NAME");

                                StructInfo.StructureCode = records.GetStringOrDefault("STRUCTURE_CODE");

                                StructInfo.StructureClass = records.GetStringOrDefault("SECTION_CLASS");

                                StructInfo.StructureDescr = records.GetStringOrDefault("DESCRIPTION");

                                StructInfo.PointGeometry = records.GetGeometryOrNull("POINT_GEOMETRY") as sdogeometry;

                                StructInfo.LineGeometry = records.GetGeometryOrNull("LINE_GEOMETRY") as sdogeometry;

                                str = StructInfo.PointGeometry.AsText;

                                StructInfo.Point = StructInfo.PointGeometry.sdo_point;

                                if (StructInfo.Suitability == null)
                                {
                                    StructInfo.Suitability = new List<string>();
                                }
                                StructInfo.Suitability.Add(records.GetStringOrDefault("SECTION_SUITABILITY"));

                                tmpOrgId1 = tmpOrgId2;

                                structIdTemp = structId;

                            }
                            if (structId == structIdTemp && tmpOrgId1 != tmpOrgId2)
                            {
                                tmpOrgId1 = records.GetLongOrDefault("OWNER_ID");

                                appraisalTemp = records.GetStringOrDefault("SECTION_SUITABILITY");

                                if (StructInfo.Suitability == null)
                                {
                                    StructInfo.Suitability = new List<string>();
                                }
                                StructInfo.Suitability.Add(appraisalTemp);
                            }
                        }

                    }
                );

            return structInfoList;
        }
        #endregion

        #region  IsAffectedPortion(int? direction, int? ptLRS, int? StrCnstrFLRS, int pointType)
        /// <summary>
        /// function to check whether the point has the constraint or strcture affected or not
        /// </summary>
        /// <param name="direction">direction of flag point link id</param>
        /// <param name="ptLRS">lrs measure at which the flag is placed</param>
        /// <param name="StrCnstrFLRS">from LRS / LRS of corresponding structure</param>
        /// <param name="pointType">type of point 239001 : start , 239002 : end</param>
        /// <returns></returns>
        private static bool IsAffectedPortion(int? direction, int? ptLRS, int? StrCnstrFLRS, int pointType)
        {
            if (direction == 0)
            {
                if (pointType == 0)
                {
                    if (ptLRS >= StrCnstrFLRS)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (ptLRS <= StrCnstrFLRS)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else if (direction == 1)
            {
                if (pointType == 0)
                {
                    if (ptLRS <= StrCnstrFLRS)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (ptLRS >= StrCnstrFLRS)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region  GetAffectedConstraintList(int routePartId, RoutePart routePart, string userSchema = UserSchema.Portal)
        /// <summary>
        /// function to fetch affected constraint's from table and show them
        /// </summary>
        /// <param name="routePartId">Route Id over which vehicle's are saved in route vehicles  / 0 for library route affected structure's</param>
        /// <param name="routePart">Route part object that has route related information</param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public static List<RouteConstraints> GetAffectedConstraintList(int routePartId, RoutePart routePart, string userSchema = UserSchema.Portal)
        {
            try
            {

                List<long?> routePointLinkId = new List<long?>();

                Dictionary<long, int[]> linkList = new System.Collections.Generic.Dictionary<long, int[]>();

                List<long?> routeLinkId = new List<long?>();

                long linkId = 0;
                int? dir, ptlrs, fLrs;
                int ptType = 0;

                bool result = false;

                int[] idInfo = null;

                foreach (RoutePath routePathObj in routePart.RoutePathList)
                {
                    //Start 
                    foreach (RoutePoint routePointObj in routePathObj.RoutePointList.Where(t => t.PointType == 0).ToList())
                    {
                        routePointLinkId.Add(routePointObj.LinkId);

                        idInfo = new int[3];

                        idInfo[0] = (int)routePointObj.Lrs;      //LRS measure of point
                        idInfo[1] = (int)routePointObj.Direction; //direction of link Id
                        idInfo[2] = 0;  //point type 0 for start

                        linkList.Add(routePointObj.LinkId, idInfo);
                    }

                    //Middle portion
                    foreach (RouteSegment routeSegObj in routePathObj.RouteSegmentList)
                    {
                        foreach (RouteLink routeLinkObj in routeSegObj.RouteLinkList.OrderBy(t => t.LinkNo))
                        {
                            routeLinkId.Add(routeLinkObj.LinkId);
                        }
                    }

                    //End 
                    foreach (RoutePoint routePointObj in routePathObj.RoutePointList.Where(t => t.PointType == 1).ToList())
                    {
                        routePointLinkId.Add(routePointObj.LinkId);

                        idInfo = new int[3];

                        idInfo[0] = (int)routePointObj.Lrs; //LRS measure of point
                        idInfo[1] = (int)routePointObj.Direction;   //direction of link id
                        idInfo[2] = 1;  // point type 1 for end

                        linkList.Add(routePointObj.LinkId, idInfo);
                    }

                }

                var tmpLinkId = routeLinkId.Except(routePointLinkId);

                routeLinkId = tmpLinkId.ToList();

                OracleCommand cmd = new OracleCommand();

                #region Portion to check all constraints at all other point's
                //creating associative array parameter to pass to stored procedure
                OracleParameter param = new OracleParameter(); // cmd.CreateParameter();
                param.OracleDbType = OracleDbType.Int32;
                param.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                param.Value = routeLinkId.ToArray(); // change when the testing is completed
                param.Size = routeLinkId.ToArray().Length;

                List<RouteConstraints> routeConst = GetConstraintInfoList(routePartId, param, userSchema);

                #endregion

                #region find constraints at start and end point's
                //Portion to check whether the structure's at point are affected or not.
                OracleParameter pointParam = new OracleParameter();
                pointParam.OracleDbType = OracleDbType.Int32;
                pointParam.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                pointParam.Value = linkList.Keys.ToArray();
                pointParam.Size = linkList.Keys.ToArray().Length;

                List<RouteConstraints> routePointConst = GetConstraintInfoList(routePartId, pointParam, userSchema);

                #endregion


                List<RouteConstraints> routePointConstList = new List<RouteConstraints>();

                foreach (RouteConstraints routeConstObj in routePointConst)
                {
                    foreach (ConstraintReferences constRef in routeConstObj.ConstraintRefrences)
                    {
                        linkId = constRef.constLink;
                        if (linkList.ContainsKey(linkId))
                        {
                            fLrs = (int)constRef.FromLinearRef;

                            ptlrs = linkList[linkId][0];
                            dir = linkList[linkId][1];
                            ptType = linkList[linkId][2];

                            result = IsAffectedPortion(dir, ptlrs, fLrs, ptType);

                            if (result)
                            {
                                routePointConstList.Add(routeConstObj);
                            }
                        }
                    }
                }

                if (routePointConstList.Count > 0)
                {
                    routeConst.AddRange(routePointConstList);
                }

                return routeConst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GtConstraintInfoList(int routePartId, OracleParameter param, string userSchema)
        /// <summary>
        /// function to fetch Constraint for affected route part
        /// </summary>
        /// <param name="routePartId"></param>
        /// <param name="param"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        private static List<RouteConstraints> GetConstraintInfoList(int routePartId, OracleParameter param, string userSchema)
        {

            List<RouteConstraints> routeConst = new List<RouteConstraints>();


            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                routeConst,
                userSchema + ".STP_LINK_ID_ARRAY.SP_SELECT_CNSTRT_INSTANT",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_ID", routePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.Add(param);
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
                        //retrieving geometric references related to constraint's
                        instance.ConstraintRefrences = new List<ConstraintReferences>();

                        instance.ConstraintRefrences = GetConstraintGeoDetails(instance.ConstraintId, userSchema);

                        instance.ConstraintGeometry = records.GetGeometryOrNull("GEOMETRY") as sdogeometry;

                        instance.ConstraintValue = new ConstraintValues();

                        instance.ConstraintSuitability = records.GetStringOrDefault("SUITABILITY"); //variable to store constraint suitability

                        instance.ConstraintValue.GrossWeight = (int)records.GetInt32OrDefault("GROSS_WEIGHT");
                        instance.ConstraintValue.AxleWeight = (int)records.GetInt32OrDefault("AXLE_WEIGHT");
                        instance.ConstraintValue.MaxHeight = records.GetSingleOrDefault("MAX_HEIGHT_MTRS");
                        instance.ConstraintValue.MaxLength = records.GetSingleOrDefault("MAX_LEN_MTRS");
                        instance.ConstraintValue.MaxWidth = records.GetSingleOrDefault("MAX_WIDTH_MTRS");

                        instance.CautionList = RouteAssessmentDAO.GetCautionList(instance.ConstraintId, userSchema);
                    }
            );

            return routeConst;
        }
        #endregion

        #region GetConstraintGeoDetails(long constId, string userSchema = UserSchema.Portal)
        /// <summary>
        /// function to fetch the constraint related geometric details
        /// </summary>
        /// <param name="constId"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        private static List<ConstraintReferences> GetConstraintGeoDetails(long constId, string userSchema = UserSchema.Portal)
        {
            int temp1 = 0;

            List<ConstraintReferences> routeConstraintRef = new List<ConstraintReferences>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                routeConstraintRef,
                userSchema + ".STP_ROUTE_ASSESSMENT.SP_R_GET_CONSTRAINT_LINKS",
                parameter =>
                {
                    parameter.AddWithValue("P_CONSTRAINT_ID", constId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        temp1 = (int)records.GetDecimalOrDefault("CONT_ONE");
                        if (temp1 != 0)
                        {
                            instance.constLink = records.GetLongOrDefault("LINK_ID");

                            instance.ToEasting = records.GetInt32OrDefault("TO_EASTING");

                            instance.ToNorthing = records.GetInt32OrDefault("TO_NORTHING");

                            instance.FromEasting = records.GetInt32OrDefault("FROM_EASTING");

                            instance.FromNorthing = records.GetInt32OrDefault("FROM_NORTHING");

                            instance.ToLinearRef = records.GetInt32OrDefault("TO_LINEAR_REF");

                            instance.FromLinearRef = records.GetInt32OrDefault("FROM_LINEAR_REF");

                            try
                            {
                                instance.IsPoint = records.GetInt16OrDefault("IS_POINT") == 1 ? true : false;
                            }
                            catch
                            {
                                instance.IsPoint = records.GetInt16Nullable("IS_POINT") == 1 ? true : false;
                            }

                            instance.Direction = records.GetInt16Nullable("DIRECTION");
                        }
                    }
            );

            return routeConstraintRef;

        }
        #endregion
    }
}