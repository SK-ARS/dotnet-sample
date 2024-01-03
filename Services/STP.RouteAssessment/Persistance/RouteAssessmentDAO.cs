using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.General;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.RouteAssessment.RouteAssessment;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using STP.Domain.RouteAssessment.XmlAffectedParties;
using STP.Domain.RouteAssessment.XmlAnalysedAnnotations;
using STP.Domain.RouteAssessment.XmlAnalysedCautions;
using STP.Domain.RouteAssessment.XmlAnalysedRoads;
using STP.Domain.RouteAssessment.XmlAnalysedStructures;
using STP.Domain.RouteAssessment.XmlConstraints;
using STP.Domain.RouteAssessment;
using STP.Domain.DrivingInstructionsInterface;
using static STP.Domain.Routes.RouteModel;
using STP.Domain.Structures;
using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;
using STP.Domain.RouteAssessment.AssessmentOutput;
using STP.ServiceAccess.RouteAssessment;
using NetSdoGeometry;

namespace STP.RouteAssessment.Persistance
{
    public class RouteAssessmentDAO
    {
          public RouteAssessmentDAO()
        {
            
        }

        static bool bCandidateRouteFlag = false;
        static bool m_CandidateRouteFlag = false;
        public static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        #region GetLibraryNotes
        public static List<LibraryNotes> GetLibraryNotes(int OrgId,int? LibraryNoteId, int UserId, string userSchema = UserSchema.Portal)
        {
            List<LibraryNotes> objCheckValidation = new List<LibraryNotes>();
            if(LibraryNoteId==0)
                LibraryNoteId = null;

            //Setup Procedure SP_GET_LIBRARY_NOTES
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                objCheckValidation,
                userSchema + ".SP_GET_LIBRARY_NOTES",
                parameter =>
                {
                    parameter.AddWithValue("p_LIB_ID", LibraryNoteId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORG_ID", OrgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_USER_ID", UserId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.LibraryNotesId = (int)records.GetLongOrDefault("LIBRARY_NOTE_ID");
                        instance.Notes = records.GetStringOrDefault("COLLABORATION_NOTES");
                        instance.OrganisationId = records.GetLongOrDefault("ORGANISATION_ID");
                        instance.UserId = records.GetLongOrDefault("USER_ID");
                    }
           );
            return objCheckValidation;
        }
        #endregion

        #region InsertLibraryNotes
        public static int InsertLibraryNotes(LibraryNotes objCheckValidation, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                      UserSchema.Portal + ".SP_INSERT_LIBRARY_NOTES",
                    parameter =>
                    {
                        parameter.AddWithValue("p_DATE_CREATED", DateTime.Now, OracleDbType.Date, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_COLLABORATION_NOTES", objCheckValidation.Notes, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ORG_ID", objCheckValidation.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_USER_ID", objCheckValidation.UserId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);

                    },
                    records =>
                    {
                        result = records.GetInt32("P_AFFECTED_ROWS");
                    }
                );
            return result;
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="anal_id"></param>
        /// <param name="anal_type"></param>
        /// 1 : Driving Instruction
        /// 2 : Route Description
        /// 3 : Affected Structures
        /// 4 : Cautions
        /// 5 : Affected constraints
        /// 6 : Annotations
        /// 7 : Affected parties
        /// <param name="userSchema"></param>
        /// <returns></returns>
        /// 
        public static RouteAssessmentModel GetDriveInstructionsInfo(long analysisId, int analysisType, string userSchema, int? sortOrder = null, int? presetFilter = null)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, Logger.LogInstance + "Executing sp for generating driving instruction...");
            RouteAssessmentModel routeAssessmentModel = new RouteAssessmentModel();
                          
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    routeAssessmentModel,
                    userSchema + ".STP_ROUTE_ASSESSMENT.SP_AGREED_ROUTE",
                    parameter =>
                    {
                        parameter.AddWithValue("P_ANAL_ID", analysisId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ANAL_TYPE", analysisType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);

                    },
                    record =>
                    {
                        switch (analysisType)
                        {
                            case 1:
                                routeAssessmentModel.DriveInst = record.GetByteArrayOrNull("DRIVING_INSTRUCTIONS");
                                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, Logger.LogInstance +"Driving instruction data for analysis id : "+ analysisId +" and analysis type : "+ analysisType +" is : "+ routeAssessmentModel.DriveInst);
                                break;

                            case 2:
                                routeAssessmentModel.RouteDescription = record.GetByteArrayOrNull("ROUTE_DESCRIPTION");
                                break;

                            case 3:
                                routeAssessmentModel.AffectedStructure = record.GetByteArrayOrNull("AFFECTED_STRUCTURES");
                                routeAssessmentModel.Cautions = record.GetByteArrayOrNull("CAUTIONS");
                                break;

                            case 4:
                                routeAssessmentModel.Cautions = record.GetByteArrayOrNull("CAUTIONS");
                                break;

                            case 5:
                                routeAssessmentModel.Constraints = record.GetByteArrayOrNull("AFFECTED_CONSTRAINTS");
                                routeAssessmentModel.Cautions = record.GetByteArrayOrNull("CAUTIONS");
                                break;

                            case 6:
                                routeAssessmentModel.Annotation = record.GetByteArrayOrNull("ANNOTATIONS");
                                break;

                            case 7:
                                routeAssessmentModel.AffectedParties = record.GetByteArrayOrNull("AFFECTED_PARTIES");
                                break;

                            case 8:
                                routeAssessmentModel.AffectedRoads = record.GetByteArrayOrNull("AFFECTED_ROADS");
                                break;
                            case 9:
                                routeAssessmentModel.DriveInst = record.GetByteArrayOrNull("DRIVING_INSTRUCTIONS");
                                routeAssessmentModel.RouteDescription = record.GetByteArrayOrNull("ROUTE_DESCRIPTION");
                                routeAssessmentModel.AffectedStructure = record.GetByteArrayOrNull("AFFECTED_STRUCTURES");
                                routeAssessmentModel.Cautions = record.GetByteArrayOrNull("CAUTIONS");
                                routeAssessmentModel.Constraints = record.GetByteArrayOrNull("AFFECTED_CONSTRAINTS");
                                routeAssessmentModel.Annotation = record.GetByteArrayOrNull("ANNOTATIONS");
                                routeAssessmentModel.AffectedParties = record.GetByteArrayOrNull("AFFECTED_PARTIES");
                                routeAssessmentModel.AffectedRoads = record.GetByteArrayOrNull("AFFECTED_ROADS");
                                break;

                        }
                    });               
          
            return routeAssessmentModel;

        }
        #region GetDriveInstructionsinfo(long anal_id, int anal_type)
        /// <summary>
        /// according to the Anal_Type data will be fetched
        /// </summary>
        /// <param name="anal_id"></param>
        /// <param name="anal_type">
        /// 1 : Driving Instruction
        /// 2 : Route Description
        /// 3 : Affected Structures
        /// 4 : Cautions
        /// 5 : Affected constraints
        /// 6 : Annotations
        /// 7 : Affected parties
        /// </param>
        /// <returns></returns>
        public static RouteAssessmentModel GetDriveInstructionsinfo(long anal_id, int anal_type, string userSchema)
        {
            RouteAssessmentModel driveinstgridobj = new RouteAssessmentModel();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                driveinstgridobj,
                userSchema + ".STP_ROUTE_ASSESSMENT.SP_AGREED_ROUTE",
                parameter =>
                {
                    parameter.AddWithValue("P_ANAL_ID", anal_id, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ANAL_TYPE", anal_type, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);

                },
                record =>
                {
                    switch (anal_type)
                    {

                        case 1:
                            driveinstgridobj.DriveInst = record.GetByteArrayOrNull("DRIVING_INSTRUCTIONS");
                            break;

                        case 2:
                            driveinstgridobj.RouteDescription = record.GetByteArrayOrNull("ROUTE_DESCRIPTION");
                            break;

                        case 3:
                            driveinstgridobj.AffectedStructure = record.GetByteArrayOrNull("AFFECTED_STRUCTURES");
                            break;

                        case 4:
                            driveinstgridobj.Cautions = record.GetByteArrayOrNull("CAUTIONS");
                            break;

                        case 5:
                            driveinstgridobj.Constraints = record.GetByteArrayOrNull("AFFECTED_CONSTRAINTS");
                            break;

                        case 6:
                            driveinstgridobj.Annotation = record.GetByteArrayOrNull("ANNOTATIONS");
                            break;

                        case 7:
                            driveinstgridobj.AffectedParties = record.GetByteArrayOrNull("AFFECTED_PARTIES");
                            break;

                        case 8:
                            driveinstgridobj.AffectedRoads = record.GetByteArrayOrNull("AFFECTED_ROADS");
                            break;
                        case 9:
                            driveinstgridobj.DriveInst = record.GetByteArrayOrNull("DRIVING_INSTRUCTIONS");
                            driveinstgridobj.RouteDescription = record.GetByteArrayOrNull("ROUTE_DESCRIPTION");
                            driveinstgridobj.AffectedStructure = record.GetByteArrayOrNull("AFFECTED_STRUCTURES");
                            driveinstgridobj.Cautions = record.GetByteArrayOrNull("CAUTIONS");
                            driveinstgridobj.Constraints = record.GetByteArrayOrNull("AFFECTED_CONSTRAINTS");
                            driveinstgridobj.Annotation = record.GetByteArrayOrNull("ANNOTATIONS");
                            driveinstgridobj.AffectedParties = record.GetByteArrayOrNull("AFFECTED_PARTIES");
                            driveinstgridobj.AffectedRoads = record.GetByteArrayOrNull("AFFECTED_ROADS");
                            break;

                    }
                });
            return driveinstgridobj;
        }
        #endregion
        #region updateRouteAnalysis(string contentRefNo, int notificationId, int revisionId,int versionId, int orgId, int analysisId, int analType,string userSchema)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentRefNo"></param>
        /// <param name="notificationId"></param>
        /// <param name="revisionId"></param>
        /// <param name="versionId"></param>
        /// <param name="orgId"></param>
        /// <param name="analysisId"></param>
        /// <param name="analType"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public static int RouteAnalysisUpdate(string contentRefNo, int notificationId, int revisionId, int versionId, int orgId, int analysisId, int analType, string userSchema, bool isCandidateRoute = false, string VSOType = "")
        {
            RouteAnalysisXml assessedXmlStrings = new RouteAnalysisXml();
            RouteAssessmentModel routeAssess = new RouteAssessmentModel();

            //assigning candidate route boolean value into global flag variable
            m_CandidateRouteFlag = isCandidateRoute;

            long status = 0;
            Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("VR1 DI ServiceLog, analType: {0},analysisId:{1},contentRefNo:{2},versionId:{3},revisionId:{4},userSchema:{5}", analType, analysisId, contentRefNo, versionId, revisionId, userSchema));
            switch (analType)
            {
                case 1:

                    status = GenerateDrivingInstructions(analysisId, contentRefNo, versionId, revisionId, userSchema);
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("VR1 DI ServiceLog1, status: {0}", status));
                    break;

                case 2://To be filled
                    break;

                case 3:
                    assessedXmlStrings.XmlAnalysedStructure = GetStructureForAnalysis(0, contentRefNo, orgId, versionId, revisionId, userSchema);
                    if (assessedXmlStrings.XmlAnalysedStructure == null)
                    {

                        routeAssess.Constraints = null;

                        status = UpdateAnalysedRoute(routeAssess.AffectedStructure, analysisId, 3, userSchema);

                        break;
                    }
                    routeAssess.AffectedStructure = StringExtractor.ZipAndBlob(assessedXmlStrings.XmlAnalysedStructure);

                    status = UpdateAnalysedRoute(routeAssess, analysisId, userSchema); //Updating 

                    break;

                case 4://To be filled for cautions
                    assessedXmlStrings.XmlAnalysedCautions = CautionGenerator(0, contentRefNo, orgId, versionId, revisionId, userSchema);

                    if (assessedXmlStrings.XmlAnalysedCautions == null)
                    {
                        routeAssess.Cautions = null;
                        status = UpdateAnalysedRoute(routeAssess.Cautions, analysisId, 4, userSchema);
                        break;
                    }
                    else
                    {
                        routeAssess.Cautions = StringExtractor.ZipAndBlob(assessedXmlStrings.XmlAnalysedCautions);

                        status = UpdateAnalysedRoute(routeAssess, analysisId, userSchema);
                    }
                    break;

                case 5: //case for affected constraint's
                    assessedXmlStrings.XmlAnalysedConstraints = ConstraintSaver(contentRefNo, versionId, 0, revisionId, userSchema);

                    if (assessedXmlStrings.XmlAnalysedConstraints == null)
                    {

                        routeAssess.Constraints = null;

                        status = UpdateAnalysedRoute(routeAssess.Constraints, analysisId, 5, userSchema);
                        break;
                    }
                    else
                    {
                        routeAssess.Constraints = StringExtractor.ZipAndBlob(assessedXmlStrings.XmlAnalysedConstraints);

                        status = UpdateAnalysedRoute(routeAssess, analysisId, userSchema);
                    }
                    break;
                case 6:
                    assessedXmlStrings.XmlAnalysedAnnotations = GetAnnotationXML(revisionId, versionId, contentRefNo, 0, userSchema);

                    if (assessedXmlStrings.XmlAnalysedAnnotations == null)
                    {
                        routeAssess.Annotation = null;
                        status = UpdateAnalysedRoute(routeAssess.Annotation, analysisId, 5, userSchema);
                        break;
                    }
                    else
                    {
                        routeAssess.Annotation = StringExtractor.ZipAndBlob(assessedXmlStrings.XmlAnalysedAnnotations);
                        status = UpdateAnalysedRoute(routeAssess, analysisId, userSchema);
                    }
                    break;

                case 7: //case for affected partie's
                    //getAffected parties function generates affected parties for a given route the list includes both SOA, Police and NH
                    assessedXmlStrings.XmlAffectedParties = GetAffectedParties(0, notificationId, contentRefNo, versionId, revisionId, orgId, userSchema, VSOType);

                    routeAssess.AffectedParties = StringExtractor.ZipAndBlob(assessedXmlStrings.XmlAffectedParties);

                    status = UpdateAnalysedRoute(routeAssess, analysisId, userSchema);
                    break;

                case 8: //case for affected road's
                    //condition to generate affected roads for notification
                    if (revisionId == 0 && versionId == 0 && contentRefNo != "")
                    {
                        assessedXmlStrings.XmlAffectedRoads = FetchAffectedRoadXml(0, 0, contentRefNo, 0, userSchema);
                    }
                    else if (revisionId != 0 && versionId == 0)
                    {
                        assessedXmlStrings.XmlAffectedRoads = FetchAffectedRoadXml(revisionId, 0, "", 0, userSchema);
                    }
                    else if (versionId != 0)
                    {
                        assessedXmlStrings.XmlAffectedRoads = FetchAffectedRoadXml(0, versionId, "", 0, userSchema); //VR -1 
                    }

                    routeAssess.AffectedRoads = StringExtractor.ZipAndBlob(assessedXmlStrings.XmlAffectedRoads);
                    status = UpdateAnalysedRoute(routeAssess, analysisId, userSchema);
                    break;
            }

            return (int)status;
        }

        public static int UpdatedNenAssessmentDetails(int notificationId, int inboxItemId, int analType, int organisationId, string userSchema)
        {
            RouteAnalysisXml assessedXmlStrings = new RouteAnalysisXml();
            RouteAssessmentModel routeAssess = new RouteAssessmentModel();
            List<RoutePartDetails> routePartDet = GetRouteDetailForAnalysis(notificationId, inboxItemId, organisationId, userSchema);
            long status = 0;
            if (routePartDet.Count > 0)
            {
                switch (analType)
                {
                    case 1:

                        status = GenerateDrivingInstructions(routePartDet, userSchema);

                        break;

                    case 2://To be filled
                        break;

                    case 3:
                        assessedXmlStrings.XmlAnalysedStructure = GetStructureForAnalysis(routePartDet, userSchema);
                        if (assessedXmlStrings.XmlAnalysedStructure == null)
                        {
                            routeAssess.Constraints = null;

                            status = UpdateAnalysedRoute(routeAssess.AffectedStructure,routePartDet[0].AnalysisId, 3, userSchema);

                            break;
                        }
                        routeAssess.AffectedStructure = StringExtractor.ZipAndBlob(assessedXmlStrings.XmlAnalysedStructure);

                        status = UpdateAnalysedRoute(routeAssess, routePartDet[0].AnalysisId, userSchema); //Updating 
                        
                        break;

                    case 4://To be filled for cautions
                        assessedXmlStrings.XmlAnalysedCautions = CautionGenerator(routePartDet, userSchema);

                        if (assessedXmlStrings.XmlAnalysedCautions == null)
                        {
                            routeAssess.Cautions = null;
                            status = UpdateAnalysedRoute(routeAssess.Cautions, routePartDet[0].AnalysisId, 4, userSchema);
                            break;
                        }
                        else
                        {
                            routeAssess.Cautions = StringExtractor.ZipAndBlob(assessedXmlStrings.XmlAnalysedCautions);

                            status = UpdateAnalysedRoute(routeAssess, routePartDet[0].AnalysisId, userSchema);
                        }
                        break;

                    case 5: //case for affected constraint's
                        assessedXmlStrings.XmlAnalysedConstraints = ConstraintSaver(routePartDet, userSchema);

                        if (assessedXmlStrings.XmlAnalysedConstraints == null)
                        {
                            
                            routeAssess.Constraints = null;

                            status = UpdateAnalysedRoute(routeAssess.Constraints, routePartDet[0].AnalysisId, 5, userSchema);
                            break;
                        }
                        else
                        {
                            routeAssess.Constraints = StringExtractor.ZipAndBlob(assessedXmlStrings.XmlAnalysedConstraints);

                            status = UpdateAnalysedRoute(routeAssess, routePartDet[0].AnalysisId, userSchema);
                        }
                        break;
                    case 6:
                        assessedXmlStrings.XmlAnalysedAnnotations = GetAnnotationXML(routePartDet, userSchema);

                        if (assessedXmlStrings.XmlAnalysedAnnotations == null)
                        {
                            routeAssess.Annotation = null;
                            status = UpdateAnalysedRoute(routeAssess.Annotation, routePartDet[0].AnalysisId, 5, userSchema);
                            break;
                        }
                        else
                        {
                            routeAssess.Annotation = StringExtractor.ZipAndBlob(assessedXmlStrings.XmlAnalysedAnnotations);
                            status = UpdateAnalysedRoute(routeAssess, routePartDet[0].AnalysisId, userSchema);
                        }
                        break;

                    case 7: //case for affected partie's

                        break;

                    case 8: //case for affected road's

                        assessedXmlStrings.XmlAffectedRoads = FetchAffectedRoadXml(routePartDet, userSchema); //VR -1 

                        routeAssess.AffectedRoads = StringExtractor.ZipAndBlob(assessedXmlStrings.XmlAffectedRoads);
                        status = UpdateAnalysedRoute(routeAssess, routePartDet[0].AnalysisId, userSchema);
                        break;
                }
            }
            return (int)status;
        }

        private static string ConstraintSaver(List<RoutePartDetails> routePartDet, string userSchema)
        {

            AnalysedConstraints constr = new AnalysedConstraints();

            constr.AnalysedConstraintsPart = new List<AnalysedConstraintsPart>();

            //More condition's can be added to fetch route details 
            foreach (RoutePartDetails routePart in routePartDet)
            {
                List<RouteConstraints> routeConstr;

                routeConstr = FetchConstraintList((int)routePart.RouteId, 1, userSchema); //For Route_part related table route type is set to 1 in case of constraints 
                if (routeConstr.Count != 0)
                {
                    AnalysedConstraintsPart analConstrPart = null;

                    //Function called to generate object containing constraint list for a part of route as single route-part is passed 
                    analConstrPart = StringExtractor.constraintListToXml(analConstrPart, routeConstr, (int)routePart.RouteId, routePart.RouteName);

                    constr.AnalysedConstraintsPart.Add(analConstrPart);
                }
                
            }
            //condition to generate affected constraint's
            if (routePartDet.Count > 0 && constr.AnalysedConstraintsPart.Count > 0)
            {
                string constraintXmlString = StringExtractor.xmlSerializer(constr); //function to generate constraint XML

                return constraintXmlString; //returning generated driving 
            }
            else
            {
                return null; //returning null if there aren't any route being found for given input's
            }
        }
        #endregion

        #region generateDrivingInstructions(int analysisId, string contentRefNo, int versionId, int revisionId, string userSchema)
        /// <summary>
        /// function to generate driving instruction and store in analysed route's table
        /// </summary>
        /// <param name="analysisId"></param>
        /// <param name="contentRefNo"></param>
        /// <param name="versionId"></param>
        /// <param name="revisionId"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        private static long GenerateDrivingInstructions(int analysisId, string contentRefNo, int versionId, int revisionId, string userSchema)
        {
            List<RoutePartDetails> routePartDet = new List<RoutePartDetails>();
            
            long errorcode = 0;
            //fetching route part details based on content reference number or analysis id
            if (contentRefNo != "" && revisionId == 0 && versionId == 0)
            {
                routePartDet = GetRouteDetailForAnalysis(0, 0, contentRefNo, 0, userSchema);
            }

            else if (contentRefNo == "" && revisionId != 0 && versionId == 0)
            {
                routePartDet = GetRouteDetailForAnalysis(0, 0, null, revisionId, userSchema);
            }

            else if (contentRefNo == "" && revisionId == 0 && versionId != 0)
            {
                routePartDet = GetRouteDetailForAnalysis(0, versionId, null, 0, userSchema);
            }
            Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("VR1 DI ServiceLog2, routePartDetcount: {0},analysisId:{1}", routePartDet.Count, analysisId));
            //checking whether the any route parts are fetched from previous thress checks if no then it checks based on analysisId
            if (routePartDet.Count == 0 && analysisId != 0)
            {
                routePartDet = GetRouteDetailForAnalysis(analysisId, 0, "", 0, userSchema);
            }

            
            DrivingInsReq drivingInsReq = new DrivingInsReq();

            drivingInsReq.AnalysisID = (ulong)analysisId;

            
            if (routePartDet.Count != 0)
            {
                foreach (RoutePartDetails routePart in routePartDet)
                {
                    drivingInsReq.ListRouteParts.Add((ulong)routePart.RouteId);
                }
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("VR1 DI ServiceLog3, AnalysisID: {0},analysisId:{1}", drivingInsReq.AnalysisID, drivingInsReq.ListRouteParts.Count));
                RouteAssessmentDAO routeAssessmentDAO = new RouteAssessmentDAO();
                errorcode = routeAssessmentDAO.GenerateRouteDescApi(drivingInsReq, userSchema);
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("VR1 DI ServiceLog4, errorcode: {0}", errorcode));
                if (errorcode == 5)
                {
                    //check the driving instruction is updated in database
                    errorcode = CheckDrivingInstruction(analysisId, 1, userSchema); //Check driving instruction in database.                      
                }
            }
            else
            {
                errorcode = 6;
            }
            Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("VR1 DI ServiceLog_errorcode, errorcode: {0}", errorcode));
            return errorcode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationId"></param>
        /// <param name="inboxItemId"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        private static long GenerateDrivingInstructions(List<RoutePartDetails> routePartDet, string userSchema = UserSchema.Portal)
        {

            long errorcode = 0;

            DrivingInsReq drivingInsReq = new DrivingInsReq();

            if (routePartDet.Count != 0)
            {
                drivingInsReq.AnalysisID = (ulong)routePartDet[0].AnalysisId;

                foreach (RoutePartDetails routePart in routePartDet)
                {
                    drivingInsReq.ListRouteParts.Add((ulong)routePart.RouteId);
                }
                RouteAssessmentDAO routeAssessmentDAO = new RouteAssessmentDAO();
                if (userSchema == UserSchema.Portal)
                {
                    
                    errorcode = routeAssessmentDAO.GenerateRouteDescApi(drivingInsReq, userSchema);
                }
                if (errorcode == 5)
                {
                    //check the driving instruction is updated in database
                    errorcode = CheckDrivingInstruction(routePartDet[0].AnalysisId, 1, userSchema); //Check driving instruction in database.                      
                }
            }
            else
            {
                errorcode = 6;
            }

            return errorcode;
        }

        private static List<RoutePartDetails> GetRouteDetailForAnalysis(int notificationId, int inboxItemId, int organisationId, string userSchema)
        {
            List<RoutePartDetails> routeList = new List<RoutePartDetails>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                     routeList,
                     userSchema + ".STP_NEN_INWARD_PROC.SP_FETCH_NEN_ROUTE",
                     parameter =>
                     {
                         parameter.AddWithValue("P_NOTIFICATION_ID", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_INBOX_ITEM_ID", inboxItemId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                     },
                     (records, instance) =>
                     {
                         instance.RouteId = Convert.ToInt64(records["ROUTE_PART_ID"]);
                         instance.RouteName = records.GetStringOrDefault("PART_NAME");
                         instance.RouteType = records.GetStringOrDefault("TRANSPORT_MODE");
                         instance.AnalysisId = Convert.ToInt32(records["ANALYSIS_ID"]);
                     }
               );

            return routeList;
        }
        #endregion
        #region GetStructureForAnalysis(int analysisId, string contentRefNo, int orgId, long versionId)
        /// <summary>
        /// Function to fetch AnalysedStructure class object for xml generation 
        /// </summary>
        /// <param name="analysisId"></param>
        /// <param name="contentRefNo"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public static string GetStructureForAnalysis(int analysisId, string contentRefNo, int orgId, long versionId, int revisionId, string userSchema)
        {
            AnalysedStructures structures = new AnalysedStructures();

            List<RoutePartDetails> routePartDet = new List<RoutePartDetails>();
            //fetching route part details based on content reference number or analysis id
            if (analysisId == 0 && contentRefNo != "" && versionId == 0 && revisionId == 0)
            {
                routePartDet = GetRouteDetailForAnalysis(0, 0, contentRefNo, 0, userSchema); //Function to retrieve based on content reference number
            }
            else if (analysisId == 0 && contentRefNo == "" && versionId != 0 && revisionId == 0)
            {
                routePartDet = GetRouteDetailForAnalysis(0, versionId, null, 0, userSchema); //Function to retrieve based on version Id
            }

            else if (analysisId != 0 && contentRefNo == "" && versionId == 0 && revisionId == 0)
            {
                routePartDet = GetRouteDetailForAnalysis(analysisId, 0, null, 0, userSchema); //Function to retrieve based on analysis id
            }

            else if (analysisId == 0 && contentRefNo == "" && versionId == 0 && revisionId != 0)
            {
                routePartDet = GetRouteDetailForAnalysis(0, 0, null, revisionId, userSchema); //Function to retrieve based on revision id
            }

            //More condition's can be added to fetch route details 

            structures.AnalysedStructuresPart = new List<AnalysedStructuresPart>();

            foreach (RoutePartDetails routePart in routePartDet)
            {
                //Getting affected structure list for each individual route part
                AnalysedStructuresPart analstruct = new AnalysedStructuresPart();

                analstruct.ComparisonId = 1123; //Sample comparison id need to be updated with new comparison id

                analstruct.Id = (int)routePart.RouteId; //Route id for route routeId

                analstruct.AnalysedStructuresPartName = routePart.RouteName; //Route Name routeName

                analstruct.Structure = new List<Structure>();
                //getting structure list for given set of structure's
                analstruct.Structure = GetStructureListForAnalysis(analstruct.Id, 0, orgId, userSchema);

                //Commenting the conditions to save empty analysis even if their are no structures generated for some empty routes
                //storing route part that has structures                
                structures.AnalysedStructuresPart.Add(analstruct);
                
            }
            
            string structureXmlString = StringExtractor.xmlStructureSerializer(structures);

            return structureXmlString; //returns structureXmlString after it's generated
        }

        public static string GetStructureForAnalysis(List<RoutePartDetails> routePartDet, string userSchema = UserSchema.Portal)
        {
            AnalysedStructures structures = new AnalysedStructures();

            //More condition's can be added to fetch route details 

            structures.AnalysedStructuresPart = new List<AnalysedStructuresPart>();

            foreach (RoutePartDetails routePart in routePartDet)
            {
                //Getting affected structure list for each individual route part
                AnalysedStructuresPart analstruct = new AnalysedStructuresPart();

                analstruct.ComparisonId = 1123; //Sample comparison id need to be updated with new comparison id

                analstruct.Id = (int)routePart.RouteId; //Route id for route routeId

                analstruct.AnalysedStructuresPartName = routePart.RouteName; //Route Name routeName

                analstruct.Structure = new List<Structure>();
                //getting structure list for given set of structure's
                analstruct.Structure = GetStructureListForAnalysis(analstruct.Id, 3, 0, userSchema);//route type 3 for NEN

                structures.AnalysedStructuresPart.Add(analstruct);
            }

            string structureXmlString = StringExtractor.xmlStructureSerializer(structures);

            return structureXmlString;
        }
        #endregion
        #region getRouteDetailForAnalysis(int analysisId,int versionId, string contentRefNo,int revisionId)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="analysisId"></param>
        /// <param name="versionId"></param>
        /// <param name="contentRefNo"></param>
        /// <returns></returns>
        public static List<RoutePartDetails> GetRouteDetailForAnalysis(int analysisId, long versionId, string contentRefNo, int revisionId, string userSchema)
        {
            int candidateTmpVar = 0;

            if ((revisionId != 0 && bCandidateRouteFlag) || (revisionId != 0 && m_CandidateRouteFlag))
            {
                candidateTmpVar = 1;
            }
            else
            {
                candidateTmpVar = 0;
            }

            
                List<RoutePartDetails> routeList = new List<RoutePartDetails>();
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                     routeList,
                     userSchema + ".STP_ROUTE_ASSESSMENT.SP_R_GET_ROUTE_FOR_ANALYSIS",
                     parameter =>
                     {
                         parameter.AddWithValue("P_ANALYSIS_ID", analysisId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_VERSION_ID", versionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_CONTENT_REF", contentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_REVISION_ID", revisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_CAND_FLAG", candidateTmpVar, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_ROUTE_ID", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                     },
                     (records, instance) =>
                     {
                         instance.RouteId = records.GetLongOrDefault("ROUTE_PART_ID");
                         instance.RouteName = records.GetStringOrDefault("PART_NAME");
                         instance.RouteType = records.GetStringOrDefault("TRANSPORT_MODE");
                     }
               );
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, Logger.LogInstance + "Route list count : "+ routeList.Count);

            return routeList;

             
        }
        #endregion

        #region getStructureListForAnalysis(int routeId, int routeType, int orgId,string userSchema)
        /// <summary>
        /// Function to fetch structure list for structure analysis
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public static List<Structure> GetStructureListForAnalysis(int routeId, int routeType, int orgId, string userSchema)
        {
            long structId = 0, structIdTemp = 0, sectionId = 0, sectionIdTemp = 0, arrangementId = 0;
            long orgIdToCheck = 0, contIdToCheck = 0, tmpOrgId = 0, tmpContactId = 0;
            List<long> sectionIdList = new List<long>();
            int strPresentAt = -1, currntSectCnt = 0;
            bool addTheSection = false;

            List<Structure> structList = new List<Structure>();

            List<Structure> structTempList = new List<Structure>(); // Temporary structure list to avoid duplicating list's variable's 

            Structure structure = null;

            StructureResponsibility structResponse = null;
            StructureResponsibleParty structResponsibleParty = null;

            Domain.RouteAssessment.XmlAnalysedStructures.Appraisal apprObj = null;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                structTempList,
                userSchema + ".STP_ICA_CALCULATOR.SP_S_STRUCTURE_SECTION_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_ID", routeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ROUTE_TYPE", routeType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                            #region
                            sectionId = records.GetLongOrDefault("SECTION_ID");
                        structId = records.GetLongOrDefault("STRUCTURE_ID");

                        structResponsibleParty = new StructureResponsibleParty();

                        if (sectionId != sectionIdTemp)
                        {
                            strPresentAt = sectionIdList.LastIndexOf(sectionId);
                            if (strPresentAt != -1) //which confirms the presence of section in the list
                                {
                                currntSectCnt = sectionIdList.Count - strPresentAt; //checks the difference between current count of the section id list and the last location the section appeared.
                                    if (currntSectCnt > 25) // if this difference is more than 25 then the section can be included as a repeated section
                                    {
                                    addTheSection = true; // this section is suitable to be added as the a single link id contained maximum of 25 section's in current database as of 19-06-2015
                                    }
                                else
                                {
                                    addTheSection = false; // else it can be excluded
                                    }
                            }
                            else
                            {
                                addTheSection = true; //if section is not part of the section id list this structure can be added to the xml.
                                }

                            if (addTheSection) //checking whether the section id is already added to the list.
                                {
                                sectionIdList.Add(sectionId);

                                structure = new Structure();

                                structure.Constraints = new Constraints();

                                structure.Constraints.UnsignedSpatialConstraint = new UnsignedSpatialConstraint();

                                structList.Add(structure);

                                structure.ESRN = records.GetStringOrDefault("STRUCTURE_CODE");

                                structure.StructureName = records.GetStringOrDefault("STRUCTURE_NAME");

                                structure.StructureSectionId = records.GetLongOrDefault("SECTION_ID");

                                structure.TraversalType = records.GetStringOrDefault("SECTION_CLASS");

                                structure.Appraisal = new List<Domain.RouteAssessment.XmlAnalysedStructures.Appraisal>();      //Appraisal from AnalysedStructure's

                                    structResponse = new StructureResponsibility();

                                structResponse.StructureResponsibleParty = new List<StructureResponsibleParty>();

                                structIdTemp = structId;

                                sectionIdTemp = sectionId;
                                    //initializing the temporary details of structure organisations.
                                    tmpOrgId = 0;
                                tmpContactId = 0;
                            }
                        }

                            //variable to store orgid and contact id of structure organisation
                            orgIdToCheck = (int)records.GetLongOrDefault("ORGANISATION_ID");
                        contIdToCheck = (int)records.GetLongOrDefault("CONTACT_ID");

                        if (structId == structIdTemp && sectionIdTemp == sectionId)
                        {
                            if (contIdToCheck != tmpContactId || orgIdToCheck != tmpOrgId) // condition to exclude same organisation being added as a responsible party.
                                {
                                tmpContactId = contIdToCheck;
                                tmpOrgId = orgIdToCheck;

                                arrangementId = records.GetLongOrDefault("ARRANGEMENT_ID");

                                structResponsibleParty.ContactId = (int)records.GetLongOrDefault("CONTACT_ID");

                                structResponsibleParty.OrganisationId = (int)records.GetLongOrDefault("ORGANISATION_ID");

                                structResponsibleParty.StructureResponsiblePartyOrganisationName = records.GetStringOrDefault("ORGANISATION_NAME");

                                if (arrangementId != 0)
                                {
                                    StructureResponsiblePartyOnBehalfOf OnBehalfOfParty = new StructureResponsiblePartyOnBehalfOf();

                                    OnBehalfOfParty.ContactId = (int)records.GetLongOrDefault("CONTACT_ID");

                                    OnBehalfOfParty.OrganisationId = (int)records.GetLongOrDefault("OWNER_ID");

                                    OnBehalfOfParty.DelegationId = (int)records.GetLongOrDefault("ARRANGEMENT_ID");

                                    OnBehalfOfParty.OrganisationName = records.GetStringOrDefault("OWNER_NAME");

                                    OnBehalfOfParty.RetainNotification = records.GetInt16OrDefault("RETAIN_NOTIFICATION") == 1;

                                    OnBehalfOfParty.WantsFailureAlert = records.GetInt16OrDefault("RECEIVE_FAILURES") == 1;

                                    structResponsibleParty.StructureResponsiblePartyOnBehalfOf = OnBehalfOfParty;

                                }

                                orgId = structResponsibleParty.OrganisationId;

                                apprObj = new Domain.RouteAssessment.XmlAnalysedStructures.Appraisal();

                                apprObj.AppraisalSuitability = new AppraisalSuitability();

                                apprObj.Organisation = new Domain.RouteAssessment.XmlAnalysedStructures.Organisation();

                                    //Getting the structure section suitability for a given structure
                                    apprObj.AppraisalSuitability.Value = records.GetStringOrDefault("SECTION_SUITABILITY");

                                if (apprObj.AppraisalSuitability.Value == "Unsuitable")
                                {
                                    structure.Constraints.SignedSpatialConstraints = new SignedSpatialConstraints();
                                    structure.Constraints.SignedSpatialConstraints.SignedSpatialConstraintsHeight = new SignedSpatialConstraintsHeight();
                                    structure.Constraints.SignedSpatialConstraints.SignedSpatialConstraintsHeight.SignedDistanceValue = new SignedDistanceValue();
                                    structure.Constraints.SignedSpatialConstraints.SignedSpatialConstraintsHeight.SignedDistanceValue.Metres = (decimal)records.GetDoubleOrDefault("SIGN_HEIGHT_METRES");

                                    structure.Constraints.UnsignedSpatialConstraint.Height = (decimal)records.GetDoubleOrDefault("MAX_HEIGHT_METRES");
                                }
                                    //Getting the organisation id
                                    apprObj.OrganisationId = orgId;

                                    //Getting the structure responsility party
                                    apprObj.Organisation.Value = structResponsibleParty.StructureResponsiblePartyOrganisationName;

                                structure.Appraisal.Add(apprObj);

                                structResponse.StructureResponsibleParty.Add(structResponsibleParty);

                                structure.StructureResponsibility = structResponse;
                            }
                        }

                            #endregion
                        }
            );

            return structList;
        }

        #endregion
        #region updateAnalysedRoute(RouteAssessmentModel routeAssess, int analysisId)
        /// <summary>
        /// updating individual analysis
        /// </summary>
        /// <param name="assessedData"></param>
        /// <param name="analysisId"></param>
        /// <param name="analType"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public static long UpdateAnalysedRoute(byte[] assessedData, int analysisId, int analType, string userSchema)
        {
            long result = 0;

            //fetching the compressed data
            //Saving the compressed Data
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".STP_ROUTE_ASSESSMENT.SP_R_UPDATE_ROUTE_ANALYSIS",
                 parameter =>
                 {
                     parameter.AddWithValue("P_ANALYSIS_ID", analysisId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_ANAL_TYPE", analType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_ANALYSED_DATA", assessedData, OracleDbType.Blob, ParameterDirectionWrap.Input, 100000000);
                     parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                 },
                 records =>
                 {
                     result = (long)records.GetDecimalOrDefault("STATUS_1");
                 });

            return result;
        }
        #endregion
        #region updateAnalysedRoute(RouteAssessmentModel routeAssess, int analysisId)
        /// <summary>
        /// updating analysed route table with updated constraints
        /// </summary>
        /// <param name="analysedConstraint">constraint list in xml</param>
        /// <param name="analysisId">analysis id to be updated for</param>
        public static long UpdateAnalysedRoute(RouteAssessmentModel routeAssess, int analysisId, string userSchema)
        {
            long result = 0;
            var obj = JsonConvert.SerializeObject(routeAssess);
            //fetching the compressed data
            //Saving the compressed Data
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".STP_ROUTE_ASSESSMENT.SP_R_UPDATE_ROUTE_ANALYSIS",
                 parameter =>
                 {
                     parameter.AddWithValue("P_ANALYSIS_ID", analysisId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_CONSTRAINTS", routeAssess.Constraints, OracleDbType.Blob, ParameterDirectionWrap.Input, 100000000);
                     parameter.AddWithValue("P_STRUCTURES", routeAssess.AffectedStructure, OracleDbType.Blob, ParameterDirectionWrap.Input, 100000000);
                     parameter.AddWithValue("P_DRIVINGINST", null, OracleDbType.Blob, ParameterDirectionWrap.Input, 100000000);
                     parameter.AddWithValue("P_CAUTIONS", routeAssess.Cautions, OracleDbType.Blob, ParameterDirectionWrap.Input, 100000000);
                     parameter.AddWithValue("P_AFFECTED_PARTIES", routeAssess.AffectedParties, OracleDbType.Blob, ParameterDirectionWrap.Input, 100000000);
                     parameter.AddWithValue("P_AFFECTED_ROADS", routeAssess.AffectedRoads, OracleDbType.Blob, ParameterDirectionWrap.Input, 100000000);
                     parameter.AddWithValue("P_ANNOTATIONS", routeAssess.Annotation, OracleDbType.Blob, ParameterDirectionWrap.Input, 100000000);
                     parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                 },
                 records =>
                 {
                     result = (long)records.GetDecimalOrDefault("STATUS_1");
                 });

            return result;
        }
        #endregion
        #region cautionGenerator(int analysisId, string contentRefNo, int orgId, long versionId)
        /// <summary>
        /// Function to generate AnalysedCautions Object for route assessment
        /// </summary>
        /// <param name="cautionObj"></param>
        /// <returns></returns>
        public static string CautionGenerator(int analysisId, string contentRefNo, int orgId, long versionId, int revisionId, string userSchema)
        {
            try
            {
                AnalysedCautions newCautionObj = new AnalysedCautions();

                bool cautExist = false;

                newCautionObj.AnalysedCautionsPart = new List<AnalysedCautionsPart>();

                List<RoutePartDetails> routePartDet = new List<RoutePartDetails>();

                //fetching route part details based on content reference number or analysis id
                if (analysisId == 0 && contentRefNo != "" && versionId == 0 && revisionId == 0)
                {
                    routePartDet = GetRouteDetailForAnalysis(0, 0, contentRefNo, 0, userSchema); //Function to retrieve based on content reference number
                }
                else if (analysisId == 0 && contentRefNo == "" && versionId != 0 && revisionId == 0)
                {
                    routePartDet = GetRouteDetailForAnalysis(0, versionId, null, 0, userSchema);
                }

                else if (analysisId != 0 && contentRefNo == "" && versionId == 0 && revisionId == 0)
                {
                    routePartDet = GetRouteDetailForAnalysis(analysisId, 0, null, 0, userSchema); //Function to retrieve based on analysis id
                }

                else if (analysisId == 0 && contentRefNo == "" && versionId == 0 && revisionId != 0)
                {
                    routePartDet = GetRouteDetailForAnalysis(0, 0, null, revisionId, userSchema); //Function to retrieve based on revision id
                }

                AnalysedCautionsPart newAnalCautPart = null;

                #region Analysed caution Part adding

                if (routePartDet.Count == 0)
                {
                    return null;
                }

                foreach (RoutePartDetails routePart in routePartDet)
                {
                    newAnalCautPart = new AnalysedCautionsPart();

                    newAnalCautPart.ComparisonId = 1123; //dummy comparison id

                    newAnalCautPart.Id = (int)routePart.RouteId;

                    #region route part type switch case

                    switch (routePart.RouteType)
                    {
                        case "road":
                            newAnalCautPart.ModeOfTransport = Domain.RouteAssessment.XmlAnalysedCautions.ModeOfTransportType.road;
                            break;

                        case "rail":
                            newAnalCautPart.ModeOfTransport = Domain.RouteAssessment.XmlAnalysedCautions.ModeOfTransportType.rail;
                            break;

                        case "sea":
                            newAnalCautPart.ModeOfTransport = Domain.RouteAssessment.XmlAnalysedCautions.ModeOfTransportType.sea;
                            break;

                        case "air":
                            newAnalCautPart.ModeOfTransport = Domain.RouteAssessment.XmlAnalysedCautions.ModeOfTransportType.air;
                            break;

                        default:
                            newAnalCautPart.ModeOfTransport = Domain.RouteAssessment.XmlAnalysedCautions.ModeOfTransportType.road;
                            break;

                    }

                    #endregion

                    newAnalCautPart.Name = routePart.RouteName;

                    newAnalCautPart.Caution = new List<AnalysedCautionStructure>();

                    AnalysedCautionStructure newAnalCautStruct = null;

                    List<RouteCautions> routeCautionList = GetCautionList(newAnalCautPart.Id, 0, userSchema);

                    #region Route Caution part being added

                    //breaking if no caution's exist

                    if (routeCautionList.Count == 0)
                    {
                        newCautionObj.AnalysedCautionsPart.Add(newAnalCautPart);
                        continue;//if their are no caution's available for route.
                    }
                    #region road cautions
                    if (routeCautionList.Count != 0)
                    {
                        foreach (RouteCautions routeCaution in routeCautionList)
                        {
                            if (routeCaution.CautionType == 0)
                            {
                                newAnalCautStruct = GetStructureCaution(routeCaution);
                            }
                            else
                            {
                                newAnalCautStruct = GetRoadCaution(routeCaution, userSchema);
                            }
                            newAnalCautPart.Caution.Add(newAnalCautStruct);
                        }
                    }
                    #endregion

                    if (newAnalCautPart.Caution.Count == 0)
                    {
                        newCautionObj.AnalysedCautionsPart = null;
                    }
                    else
                    {
                        if (newCautionObj.AnalysedCautionsPart == null)
                        {
                            newCautionObj.AnalysedCautionsPart = new List<AnalysedCautionsPart>();
                        }
                        newCautionObj.AnalysedCautionsPart.Add(newAnalCautPart);
                    }
                    #endregion
                }

                #endregion
                if (newCautionObj.AnalysedCautionsPart != null)
                {
                    foreach (AnalysedCautionsPart cautList in newCautionObj.AnalysedCautionsPart)
                    {
                        if (cautList.Caution.Count > 0)
                        {
                            cautExist = true; //condition to check whether any route part has cautions or not.
                        }
                    }

                    if (cautExist) //if caution exist for any of the route part only then an xml is generated.
                    {
                        if (newCautionObj.AnalysedCautionsPart.Count != 0)
                        {
                            //function to serialize the object xml string
                            string cautionXmlString = StringExtractor.XmlCautionSerializer(newCautionObj);
                            return cautionXmlString;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Caution cannot be generated, Exception: {0}", e));
                return null;
            }
        }

        public static string CautionGenerator(List<RoutePartDetails> routePartDet, string userSchema)
        {
            try
            {
                AnalysedCautions newCautionObj = new AnalysedCautions();

                bool cautExist = false;

                newCautionObj.AnalysedCautionsPart = new List<AnalysedCautionsPart>();


                AnalysedCautionsPart newAnalCautPart = null;

                #region Analysed caution Part adding

                if (routePartDet.Count == 0)
                {
                    return null;
                }

                foreach (RoutePartDetails routePart in routePartDet)
                {
                    newAnalCautPart = new AnalysedCautionsPart();

                    newAnalCautPart.ComparisonId = 1123; //dummy comparison id

                    newAnalCautPart.Id = (int)routePart.RouteId;

                    #region route part type switch case

                    switch (routePart.RouteType)
                    {
                        case "road":
                            newAnalCautPart.ModeOfTransport = Domain.RouteAssessment.XmlAnalysedCautions.ModeOfTransportType.road;
                            break;

                        case "rail":
                            newAnalCautPart.ModeOfTransport = Domain.RouteAssessment.XmlAnalysedCautions.ModeOfTransportType.rail;
                            break;

                        case "sea":
                            newAnalCautPart.ModeOfTransport = Domain.RouteAssessment.XmlAnalysedCautions.ModeOfTransportType.sea;
                            break;

                        case "air":
                            newAnalCautPart.ModeOfTransport = Domain.RouteAssessment.XmlAnalysedCautions.ModeOfTransportType.air;
                            break;

                        default:
                            newAnalCautPart.ModeOfTransport = Domain.RouteAssessment.XmlAnalysedCautions.ModeOfTransportType.road;
                            break;

                    }

                    #endregion

                    newAnalCautPart.Name = routePart.RouteName;

                    newAnalCautPart.Caution = new List<AnalysedCautionStructure>();

                    AnalysedCautionStructure newAnalCautStruct = null;

                    List<RouteCautions> routeCautionList = GetCautionList(newAnalCautPart.Id, 0, userSchema);

                    #region Route Caution part being added

                    //breaking if no caution's exist

                    if (routeCautionList.Count == 0)
                    {
                        newCautionObj.AnalysedCautionsPart.Add(newAnalCautPart);
                        continue;//if their are no caution's available for route.
                    }
                    #region road cautions
                    if (routeCautionList.Count != 0)
                    {
                        foreach (RouteCautions routeCaution in routeCautionList)
                        {
                            if (routeCaution.CautionType == 0)
                            {
                                newAnalCautStruct = GetStructureCaution(routeCaution);
                            }
                            else
                            {
                                newAnalCautStruct = GetRoadCaution(routeCaution, userSchema);
                            }
                            newAnalCautPart.Caution.Add(newAnalCautStruct);
                        }
                    }
                    #endregion

                    if (newAnalCautPart.Caution.Count == 0)
                    {
                        newCautionObj.AnalysedCautionsPart = null;
                    }
                    else
                    {
                        if (newCautionObj.AnalysedCautionsPart == null)
                        {
                            newCautionObj.AnalysedCautionsPart = new List<AnalysedCautionsPart>();
                        }
                        newCautionObj.AnalysedCautionsPart.Add(newAnalCautPart);
                    }
                    #endregion
                }

                #endregion
                if (newCautionObj.AnalysedCautionsPart != null)
                {
                    foreach (AnalysedCautionsPart cautList in newCautionObj.AnalysedCautionsPart)
                    {
                        if (cautList.Caution.Count > 0)
                        {
                            cautExist = true; //condition to check whether any route part has cautions or not.
                        }
                    }

                    if (cautExist) //if caution exist for any of the route part only then an xml is generated.
                    {
                        if (newCautionObj.AnalysedCautionsPart.Count != 0)
                        {
                            //function to serialize the object xml string
                            string cautionXmlString = StringExtractor.XmlCautionSerializer(newCautionObj);
                            return cautionXmlString;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Caution cannot be generated, Exception: {0}", e));
                return null;
            }
        }
        #endregion
        
        #region getStructureCaution(List<RouteCautions> structureCautionList, AnalysedCautionsPart newAnalCautPart, string userSchema = UserSchema.Portal)
        /// <summary>
        /// function to fetch structure cautions and their contacts
        /// </summary>
        /// <param name="structureCautionList"></param>
        /// <param name="newAnalCautPart"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        private static AnalysedCautionStructure GetStructureCaution(RouteCautions routeCaution)
        {
            //breaking if no caution's exist
            AnalysedCautionStructure newAnalCautStruct = new AnalysedCautionStructure();

            newAnalCautStruct.CautionedEntity1 = new AnalysedCautionChoiceStructure();

            newAnalCautStruct.CautionedEntity1.AnalysedCautionStructureStructure = new AnalysedCautionStructureStructure();

            newAnalCautStruct.CautionedEntity1.AnalysedCautionStructureStructure.Annotation = new List<ResolvedAnnotationStructure>();

            newAnalCautStruct.Name = routeCaution.CautionName; //name of caution being generated

            newAnalCautStruct.CautionedEntity1.AnalysedCautionStructureStructure.ESRN = routeCaution.CautionConstraintCode;

            newAnalCautStruct.CautionedEntity1.AnalysedCautionStructureStructure.Name = routeCaution.CautionConstraintName;

            #region routeConstrType Switch statement
            try
            {
                newAnalCautStruct.ConstrainingAttribute = new List<CautionConditionType>();

                if (routeCaution.CautionConstraintValue.GrossWeight != 0)
                {
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.grossweight);
                }

                if (routeCaution.CautionConstraintValue.AxleWeight != 0)
                {
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.axleweight);
                }

                if (routeCaution.CautionConstraintValue.MaxHeight != 0)
                {
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.height);
                }

                if (routeCaution.CautionConstraintValue.MaxLength != 0)
                {
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.overalllength);
                }

                if (routeCaution.CautionConstraintValue.MaxWidth != 0)
                {
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.width);
                }

                if (routeCaution.CautionConstraintValue.MinSpeed != 0)
                {
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.speed);
                }

                if (routeCaution.CautionConstraintType == "underbridge")
                {
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionStructureStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.StructureType.underbridge;
                }

                else if (routeCaution.CautionConstraintType == "overbridge")
                {
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionStructureStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.StructureType.overbridge;
                }

                else if (routeCaution.CautionConstraintType == "level crossing")
                {
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionStructureStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.StructureType.levelcrossing;
                }

                else
                {
                    //do nothing
                }

                if (newAnalCautStruct.ConstrainingAttribute.Count == 0)
                {
                    newAnalCautStruct.ConstrainingAttribute = null;
                }
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Exception occurred, Exception: "+ex;
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, exceptionMessage);
            }
            #endregion

            #region Road details
            RoadIdentificationStructure roadIdentification = new RoadIdentificationStructure();

            roadIdentification.Name = routeCaution.RoadName;

            newAnalCautStruct.Road = roadIdentification;
            #endregion

            newAnalCautStruct.CautionId = (int)routeCaution.CautionId;

            newAnalCautStruct.CautionedEntity = null;
            try
            {
                newAnalCautStruct.Vehicle = new string[1];

                newAnalCautStruct.Vehicle[0] = routeCaution.Suitability;
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Exception occurred, Exception: " + ex;
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, exceptionMessage);
            }
            newAnalCautStruct.IsApplicable = true;

            #region Caution conditions
            // Condition's include Max weight, max height, max width etc.
            newAnalCautStruct.Conditions = new CautionConditionStructure();

            newAnalCautStruct.Conditions.MaxAxleWeight = new WeightStructure();

            newAnalCautStruct.Conditions.MaxAxleWeight.Value = routeCaution.CautionConstraintValue.AxleWeight; //tonnes to kilograms

            newAnalCautStruct.Conditions.MaxGrossWeight = new WeightStructure();

            newAnalCautStruct.Conditions.MaxGrossWeight.Value = routeCaution.CautionConstraintValue.GrossWeight;

            newAnalCautStruct.Conditions.MaxHeight = new DistanceStructure();

            newAnalCautStruct.Conditions.MaxHeight.Value = (decimal)routeCaution.CautionConstraintValue.MaxHeight;

            newAnalCautStruct.Conditions.MaxOverallLength = new DistanceStructure();

            newAnalCautStruct.Conditions.MaxOverallLength.Value = (decimal)routeCaution.CautionConstraintValue.MaxLength;

            newAnalCautStruct.Conditions.MaxWidth = new DistanceStructure();

            newAnalCautStruct.Conditions.MaxWidth.Value = (decimal)routeCaution.CautionConstraintValue.MaxWidth;

            newAnalCautStruct.Conditions.MinSpeed = new SpeedStructure();

            newAnalCautStruct.Conditions.MinSpeed.Value = (decimal)routeCaution.CautionConstraintValue.MinSpeed;

            #endregion

            //New Object creation to save the specific action tag value
            newAnalCautStruct.Action = new CautionActionStructure();

            newAnalCautStruct.Action.SpecificAction = routeCaution.cautDescription;  // specific action performed for cautions

            newAnalCautStruct.Contact = new List<ResolvedContactStructure>();

            #region Caution contact part being added
            //Entering the contact details.
            foreach (AssessmentContacts cautContactObj in routeCaution.CautionContactList)
            {
                ResolvedContactStructure newContactObj = new ResolvedContactStructure();

                newContactObj.Address = new Domain.RouteAssessment.XmlAnalysedCautions.AddressStructure();

                newContactObj.Address.Line = new List<string>();

                newContactObj.Address.Line = cautContactObj.AddressLine;

                newContactObj.Address.CountrySpecified = cautContactObj.Country != null;

                #region getting country switch case

                switch (cautContactObj.Country)
                {
                    case "england":
                        newContactObj.Address.Country = Domain.RouteAssessment.XmlAnalysedCautions.CountryType.england;
                        break;

                    case "wales":
                        newContactObj.Address.Country = Domain.RouteAssessment.XmlAnalysedCautions.CountryType.wales;
                        break;

                    case "scotland":
                        newContactObj.Address.Country = Domain.RouteAssessment.XmlAnalysedCautions.CountryType.scotland;
                        break;

                    case "northern ireland":
                        newContactObj.Address.Country = Domain.RouteAssessment.XmlAnalysedCautions.CountryType.northernireland;
                        break;
                }

                #endregion

                newContactObj.Address.PostCode = cautContactObj.PostCode;

                newContactObj.ContactId = (int)cautContactObj.ContactId;

                newContactObj.ContactIdSpecified = cautContactObj.ContactId != 0;

                newContactObj.FullName = cautContactObj.ContactName;

                newContactObj.OrganisationId = (int)cautContactObj.OrganisationId;

                newContactObj.OrganisationName = cautContactObj.OrganisationName;

                newContactObj.TelephoneNumber = cautContactObj.Mobile;

                newContactObj.FaxNumber = cautContactObj.Fax;

                newContactObj.Role = cautContactObj.RoleName;

                newContactObj.RoleSpecified = cautContactObj.RoleName != null;

                newContactObj.EmailAddress = cautContactObj.Email;

                newAnalCautStruct.Contact.Add(newContactObj);
            }

            #endregion

            return newAnalCautStruct;
        }
        #endregion
        private static AnalysedCautionStructure GetRoadCaution(RouteCautions routeCaution, string userSchema = UserSchema.Portal)
        {
            AnalysedCautionStructure newAnalCautStruct = new AnalysedCautionStructure();

            newAnalCautStruct.CautionedEntity1 = new AnalysedCautionChoiceStructure();

            newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure = new AnalysedCautionConstraintStructure();

            newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Annotation = new List<ResolvedAnnotationStructure>();

            newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.ECRN = routeCaution.CautionConstraintCode;

            newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Name = routeCaution.CautionConstraintName;

            try
            {
                newAnalCautStruct.Name = routeCaution.CautionName; //name of caution being generated
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Exception occurred, Exception: " + ex;
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, exceptionMessage);
            }

            #region routeConstrType Switch statement

            switch (routeCaution.CautionConstraintType)
            {
                case "generic":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.generic;
                    break;

                case "height":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.height;
                    break;

                case "width":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.width;
                    break;

                case "length":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.length;
                    break;

                case "weight":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.weight;
                    break;

                case "oneway":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.oneway;
                    break;

                case "roadworks":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.roadworks;
                    break;

                case "incline":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.incline;
                    break;

                case "tram":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.tram;
                    break;

                case "tight bend":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.tightbend;
                    break;

                case "event":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.@event;
                    break;

                case "risk of grounding":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.riskofgrounding;
                    break;

                case "unmade":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.unmade;
                    break;

                case "natural void":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.naturalvoid;
                    break;

                case "manmade void":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.manmadevoid;
                    break;

                case "tunnel":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.tunnel;
                    break;

                case "tunnel void":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.tunnelvoid;
                    break;

                case "pipes and ducts":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.pipesandducts;
                    break;

                case "retaining wall":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.retainingwall;
                    break;

                case "traffic calming":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.trafficcalming;
                    break;

                case "overhead building":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.overheadbuilding;
                    break;

                case "overhead pipes and utilities":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.overheadpipesandutilities;
                    break;

                case "adjacent retaining wall":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.adjacentretainingwall;
                    break;

                case "power cable":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.powercable;
                    break;

                case "telecomms cable":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type =Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.telecommscable;
                    break;

                case "gantry road furniture":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type =Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.gantryroadfurniture;
                    break;

                case "cantilever road furniture":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type =Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.cantileverroadfurniture;
                    break;

                case "catenary road furniture":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type =Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.catenaryroadfurniture;
                    break;

                case "electrification cable":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type =Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.electrificationcable;
                    break;

                case "bollard":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type =Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.bollard;
                    break;

                case "removable bollard":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type =Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.removablebollard;
                    break;
            }
            #endregion

            newAnalCautStruct.Road = GetRoadDetails(routeCaution.CautionConstraintId, userSchema);

            newAnalCautStruct.CautionId = (int)routeCaution.CautionId;

            newAnalCautStruct.CautionedEntity = null;

            try
            {
                newAnalCautStruct.Vehicle = new string[1];

                newAnalCautStruct.Vehicle[0] = routeCaution.Suitability;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured, Exception: " + ex​​​​);
            }
            newAnalCautStruct.IsApplicable = true;

            // Condition's include Max weight, max height, max width etc.
            newAnalCautStruct.Conditions = new CautionConditionStructure();

            newAnalCautStruct.Conditions.MaxAxleWeight = new WeightStructure();

            newAnalCautStruct.Conditions.MaxAxleWeight.Value = routeCaution.CautionConstraintValue.AxleWeight;

            newAnalCautStruct.Conditions.MaxGrossWeight = new WeightStructure();

            newAnalCautStruct.Conditions.MaxGrossWeight.Value = routeCaution.CautionConstraintValue.GrossWeight;

            newAnalCautStruct.Conditions.MaxHeight = new DistanceStructure();

            newAnalCautStruct.Conditions.MaxHeight.Value = (decimal)routeCaution.CautionConstraintValue.MaxHeight;

            newAnalCautStruct.Conditions.MaxOverallLength = new DistanceStructure();

            newAnalCautStruct.Conditions.MaxOverallLength.Value = (decimal)routeCaution.CautionConstraintValue.MaxLength;

            newAnalCautStruct.Conditions.MaxWidth = new DistanceStructure();

            newAnalCautStruct.Conditions.MaxWidth.Value = (decimal)routeCaution.CautionConstraintValue.MaxWidth;

            newAnalCautStruct.Conditions.MinSpeed = new SpeedStructure();

            newAnalCautStruct.Conditions.MinSpeed.Value = (decimal)routeCaution.CautionConstraintValue.MinSpeed;

            #region routeConstrType Switch statement
            try
            {
                newAnalCautStruct.ConstrainingAttribute = new List<CautionConditionType>();

                if (routeCaution.CautionConstraintValue.GrossWeight != 0)
                {
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.grossweight);
                }

                if (routeCaution.CautionConstraintValue.AxleWeight != 0)
                {
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.axleweight);
                }

                if (routeCaution.CautionConstraintValue.MaxHeight != 0)
                {
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.height);
                }

                if (routeCaution.CautionConstraintValue.MaxLength != 0)
                {
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.overalllength);
                }

                if (routeCaution.CautionConstraintValue.MaxWidth != 0)
                {
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.width);
                }

                if (routeCaution.CautionConstraintValue.MinSpeed != 0)
                {
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.speed);
                }

                if (newAnalCautStruct.ConstrainingAttribute.Count == 0)
                {
                    newAnalCautStruct.ConstrainingAttribute = null;
                }
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Exception occurred, Exception: " + ex;
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, exceptionMessage);
            }
            #endregion

            //New Object creation to save the specific action tag value
            newAnalCautStruct.Action = new CautionActionStructure();

            newAnalCautStruct.Action.SpecificAction = routeCaution.cautDescription; // specific action performed for cautions

            newAnalCautStruct.Contact = new List<ResolvedContactStructure>();

            #region Caution contact part being added
            //Entering the contact details.
            foreach (AssessmentContacts cautContactObj in routeCaution.CautionContactList)
            {
                ResolvedContactStructure newContactObj = new ResolvedContactStructure();

                newContactObj.Address = new Domain.RouteAssessment.XmlAnalysedCautions.AddressStructure();

                newContactObj.Address.Line = new List<string>();

                newContactObj.Address.Line = cautContactObj.AddressLine;

                newContactObj.Address.CountrySpecified = cautContactObj.Country != null;

                #region getting country switch case

                switch (cautContactObj.Country)
                {
                    case "england":
                        newContactObj.Address.Country =Domain.RouteAssessment.XmlAnalysedCautions.CountryType.england;
                        break;

                    case "wales":
                        newContactObj.Address.Country =Domain.RouteAssessment.XmlAnalysedCautions.CountryType.wales;
                        break;

                    case "scotland":
                        newContactObj.Address.Country =Domain.RouteAssessment.XmlAnalysedCautions.CountryType.scotland;
                        break;

                    case "northern ireland":
                        newContactObj.Address.Country =Domain.RouteAssessment.XmlAnalysedCautions.CountryType.northernireland;
                        break;
                }

                #endregion

                newContactObj.Address.PostCode = cautContactObj.PostCode;

                newContactObj.ContactId = (int)cautContactObj.ContactId;

                newContactObj.ContactIdSpecified = cautContactObj.ContactId != 0;

                newContactObj.FullName = cautContactObj.ContactName;

                newContactObj.OrganisationId = (int)cautContactObj.OrganisationId;

                newContactObj.OrganisationName = cautContactObj.OrganisationName;

                newContactObj.TelephoneNumber = cautContactObj.Mobile;

                newContactObj.FaxNumber = cautContactObj.Fax;

                newContactObj.Role = cautContactObj.RoleName;

                newContactObj.RoleSpecified = cautContactObj.RoleName != null;

                newContactObj.EmailAddress = cautContactObj.Email;

                newAnalCautStruct.Contact.Add(newContactObj);
            }
            #endregion

            return newAnalCautStruct;
        }
        #region getCautionContact(long ownerOrgId)
        /// <summary>
        /// Function to generate caution contact list from given owner organisation id for a caution
        /// </summary>
        /// <param name="ownerOrgId"></param>
        /// <returns></returns>
        private static List<AssessmentContacts> GetCautionContact(long ownerOrgId, string userSchema)
        {
            List<AssessmentContacts> cautContList = new List<AssessmentContacts>();

            string roleTypeName = null;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    cautContList,
                    userSchema + ".STP_ROUTE_ASSESSMENT.SP_R_GET_NOTIFICATION_CONTACT",
                    parameter =>
                    {
                        parameter.AddWithValue("P_ORG_ID", ownerOrgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    (records, instance) =>
                    {
                        instance.ContactId = (int)records.GetLongOrDefault("CONTACT_ID");

                        instance.OrganisationId = (int)ownerOrgId;

                        instance.OrganisationName = records.GetStringOrDefault("ORGNAME");

                        instance.ContactName = records.GetStringOrDefault("FULL_NAME");

                        instance.AddressLine = new List<string>();
                        //getting the address line into a string list 
                        instance.AddressLine.Add(records.GetStringOrDefault("ADDRESSLINE_1"));

                        instance.AddressLine.Add(records.GetStringOrDefault("ADDRESSLINE_2"));

                        instance.AddressLine.Add(records.GetStringOrDefault("ADDRESSLINE_3"));

                        instance.AddressLine.Add(records.GetStringOrDefault("ADDRESSLINE_4"));

                        instance.AddressLine.Add(records.GetStringOrDefault("ADDRESSLINE_5"));

                        instance.PostCode = records.GetStringOrDefault("POSTCODE");

                        instance.Fax = records.GetStringOrDefault("FAX");

                        instance.Mobile = records.GetStringOrDefault("PHONENUMBER");

                        //code to read phone number if mobile entry is empty
                        if (instance.Mobile == "")
                        {
                            instance.Mobile = records.GetStringOrDefault("PHONENUMBER");
                        }

                        roleTypeName = records.GetStringOrDefault("ROLE_NAME");

                        switch (roleTypeName)
                        {
                            case "data holder":
                                instance.RoleName = Domain.RouteAssessment.XmlAnalysedCautions.RoleType.dataholder;
                                break;

                            case "notification contact":
                                instance.RoleName = Domain.RouteAssessment.XmlAnalysedCautions.RoleType.notificationcontact;
                                break;

                            case "official contact":
                                instance.RoleName = Domain.RouteAssessment.XmlAnalysedCautions.RoleType.officialcontact;
                                break;

                            case "police alo":
                                instance.RoleName = Domain.RouteAssessment.XmlAnalysedCautions.RoleType.policealo;
                                break;

                            case "haulier":
                                instance.RoleName = Domain.RouteAssessment.XmlAnalysedCautions.RoleType.haulier;
                                break;

                            case "it contact":
                                instance.RoleName = Domain.RouteAssessment.XmlAnalysedCautions.RoleType.itcontact;
                                break;

                            case "data owner":
                                instance.RoleName = Domain.RouteAssessment.XmlAnalysedCautions.RoleType.dataowner;
                                break;
                            //case "default contact"
                            default:
                                instance.RoleName = Domain.RouteAssessment.XmlAnalysedCautions.RoleType.defaultcontact;
                                break;
                        }

                        instance.Email = records.GetStringOrDefault("EMAIL");

                        instance.Country = records.GetStringOrDefault("COUNTRY");

                    });

            return cautContList;
        }
        #endregion
        #region getRoadDetails(long contrId)
        /// <summary>
        /// function to get road number road name based on the constraint id
        /// </summary>
        /// <param name="contrId"></param>
        /// <returns></returns>
        private static RoadIdentificationStructure GetRoadDetails(long contrId, string userSchema)
        {

            RoadIdentificationStructure roadIdentification = new RoadIdentificationStructure();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                  roadIdentification,
                  userSchema + ".STP_ROUTE_ASSESSMENT.SP_R_GET_ROAD_DESCRIPTION",
                  parameter =>
                  {
                      parameter.AddWithValue("P_CONSTR_ID", contrId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                  },
                      (records) =>
                      {
                          roadIdentification.Number = records.GetStringOrDefault("ROAD_IDS");
                      }
              );

            return roadIdentification;

        }
        #endregion
        #region constraintSaver(string contentRefNo, long versionId, int analysisId) returning Constraints as XML String
        /// <summary>
        /// Function that generates Constraint XML
        /// </summary>
        /// <param name="contentRefNo"></param>
        /// <returns>constraintXmlString</returns>
        public static string ConstraintSaver(string contentRefNo, long versionId, int analysisId, int revisionId, string userSchema)
        {

            AnalysedConstraints constr = new AnalysedConstraints();

            constr.AnalysedConstraintsPart = new List<AnalysedConstraintsPart>();

            List<RoutePartDetails> routePartDet = new List<RoutePartDetails>();
            //fetching route part details based on content reference number or analysis id
            if (analysisId == 0 && contentRefNo != "" && versionId == 0 && revisionId == 0)
            {
                routePartDet = GetRouteDetailForAnalysis(0, 0, contentRefNo, 0, userSchema); //Function to retrieve based on content reference number
            }
            else if (analysisId == 0 && contentRefNo == "" && versionId != 0 && revisionId == 0)
            {
                routePartDet = GetRouteDetailForAnalysis(0, versionId, null, 0, userSchema);
            }

            else if (analysisId != 0 && contentRefNo == "" && versionId == 0 && revisionId == 0)
            {
                routePartDet = GetRouteDetailForAnalysis(analysisId, 0, null, 0, userSchema); //Function to retrieve based on analysis id
            }

            else if (analysisId == 0 && contentRefNo == "" && versionId == 0 && revisionId != 0)
            {
                routePartDet = GetRouteDetailForAnalysis(0, 0, null, revisionId, userSchema); //Function to retrieve based on revision id
            }
            //More condition's can be added to fetch route details 
            foreach (RoutePartDetails routePart in routePartDet)
            {
                List<RouteConstraints> routeConstr;

                routeConstr = FetchConstraintList((int)routePart.RouteId, 1, userSchema); //For Route_part related table route type is set to 1 in case of constraints 
                if (routeConstr.Count != 0)
                {
                    AnalysedConstraintsPart analConstrPart = null;

                    //Function called to generate object containing constraint list for a part of route as single route-part is passed 
                    analConstrPart = StringExtractor.constraintListToXml(analConstrPart, routeConstr, (int)routePart.RouteId, routePart.RouteName);

                    constr.AnalysedConstraintsPart.Add(analConstrPart);
                }
                
            }
            //condition to generate affected constraint's
            if (routePartDet.Count > 0 && constr.AnalysedConstraintsPart.Count > 0)
            {
                string constraintXmlString = StringExtractor.xmlSerializer(constr); //function to generate constraint XML

                return constraintXmlString; //returning generated driving 
            }
            else
            {
                return null; //returning null if there aren't any route being found for given input's
            }

        }
        #endregion
       
        #region  getConstraintGeoDetails(long constId)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        public static List<ConstraintReferences> GetConstraintGeoDetails(long constId, string userSchema)
        {
            int temp1 = 0, temp2 = 0;

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
                        temp2 = (int)records.GetDecimalOrDefault("CONT_TWO");

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

                                instance.IsPoint = records.GetInt16OrDefault("IS_POINT") == 1;
                            }
                            catch
                            {
                                instance.IsPoint = records.GetInt16Nullable("IS_POINT") == 1;
                            }
                            instance.Direction = records.GetInt16Nullable("DIRECTION");

                        }
                        else if (temp2 != 0)
                        {
                            instance.constLink = records.GetLongOrDefault("LINK_ID");

                            instance.LinearRef = records.GetInt32OrDefault("LINEAR_REF");

                            instance.Easting = records.GetInt32OrDefault("EASTING");

                            instance.Northing = records.GetInt32OrDefault("NORTHING");

                            instance.ToLinearRef = records.GetInt32OrDefault("TO_LINEAR_REF");

                            instance.FromLinearRef = records.GetInt32OrDefault("FROM_LINEAR_REF");

                        }
                    }
            );

            return routeConstraintRef;

        }
        #endregion
        #region getCautionList(int routeId, int routeType)
        /// <summary>
        /// function to get constraint list for a routeId from both routeLibrary (routeType = 1)  and application route part (routeType = 0) 
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="routeType">1 for planned route  , 0 for Application route</param>
        /// <returns></returns>
        public static List<RouteCautions> GetCautionList(int routeId, int routeType, string userSchema)
        {
            List<RouteCautions> routeCaution = new List<RouteCautions>();

            long roadConstId = 0, structConstId = 0;
            string cautionSuitability = "";

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                routeCaution,
                userSchema + ".STP_ROUTE_ASSESSMENT.SP_R_GET_CAUTION_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_PART_ID", routeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ROUTE_TYPE", routeType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        #region structure caution

                        roadConstId = records.GetLongOrDefault("CONSTRAINT_ID"); //fetching constraint id 
                        structConstId = records.GetLongOrDefault("STRUCTURE_ID"); // fetching structure id

                        if (structConstId != 0 && roadConstId == 0) //filling structure caution details
                        {
                            instance.CautionStructureId = structConstId;

                            instance.CautionType = 0;

                            instance.CautionConstraintName = records.GetStringOrDefault("STRUCTURE_NAME");

                            instance.CautionConstraintCode = records.GetStringOrDefault("STRUCTURE_CODE");

                            instance.CautionConstraintType = records.GetStringOrDefault("STRUCTURE_TYPE");
                        }
                        if (structConstId == 0 && roadConstId != 0) //filling constraint caution details
                        {
                            instance.CautionConstraintId = roadConstId;

                            instance.CautionType = 1;

                            instance.CautionConstraintName = records.GetStringOrDefault("CONSTRAINT_NAME");

                            instance.CautionConstraintCode = records.GetStringOrDefault("CONSTRAINT_CODE");

                            instance.CautionConstraintType = records.GetStringOrDefault("CONSTRAINT_TYPE");
                        }

                        cautionSuitability = records.GetStringOrDefault("SUITABILITY");

                        instance.Suitability = cautionSuitability.TrimEnd(',');

                        instance.CautionId = records.GetLongOrDefault("CAUTION_ID");

                        instance.CautionName = records.GetStringOrDefault("CAUTION_NAME");

                        instance.cautDescription = StringExtractor.xmlStringExtractor(records.GetStringOrDefault("SPECIFIC_ACTION"), "SpecificAction");

                        instance.OwnerOrgId = records.GetLongOrDefault("ORGANISATION_ID");

                        instance.CautionContactList = GetCautionContact(instance.OwnerOrgId, userSchema);

                        instance.RoadName = records.GetStringOrDefault("ROAD_NAME");

                        instance.CautionConstraintValue = new ConstraintValues();

                        try
                        {
                            instance.CautionConstraintValue.GrossWeight = (long)records.GetDoubleOrDefault("GROSS_WEIGHT");
                            instance.CautionConstraintValue.AxleWeight = (long)records.GetDoubleOrDefault("AXLE_WEIGHT");
                            instance.CautionConstraintValue.MaxHeight = (Single)records.GetDoubleOrDefault("MAX_HEIGHT");
                            instance.CautionConstraintValue.MaxLength = (Single)records.GetDoubleOrDefault("MAX_LENGTH");
                            instance.CautionConstraintValue.MaxWidth = (Single)records.GetDoubleOrDefault("MAX_WIDTH");
                            instance.CautionConstraintValue.MinSpeed = records.GetSingleOrDefault("MIN_SPEED");
                        }
                        catch
                        {
                            instance.CautionConstraintValue.GrossWeight = (long)records.GetInt32OrDefault("GROSS_WEIGHT");
                            instance.CautionConstraintValue.AxleWeight = (long)records.GetInt32OrDefault("AXLE_WEIGHT");
                            instance.CautionConstraintValue.MaxHeight = records.GetSingleOrDefault("MAX_HEIGHT");
                            instance.CautionConstraintValue.MaxLength = records.GetSingleOrDefault("MAX_LENGTH");
                            instance.CautionConstraintValue.MaxWidth = records.GetSingleOrDefault("MAX_WIDTH");
                            instance.CautionConstraintValue.MinSpeed = records.GetSingleOrDefault("MIN_SPEED");
                        }

                        #endregion
                    }
            );

            return routeCaution;
        }
        #endregion
        #region  getCautionList(long constraintId)
        /// <summary>
        /// Function to get caution list for a given constraintId
        /// </summary>
        /// <param name="constraintId"></param>
        /// <returns></returns>
        public static List<RouteCautions> GetCautionList(long constraintId, string userSchema)
        {
            List<RouteCautions> routeCaution = new List<RouteCautions>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                routeCaution,
                userSchema + ".STP_ROUTE_ASSESSMENT.SP_R_GET_CAUTIONS",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_PART_ID", null, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONSTRAINT_ID", constraintId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ROUTE_TYPE", null, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.CautionId = records.GetLongOrDefault("CAUTION_ID");
                        instance.CautionConstraintId = records.GetLongOrDefault("CONSTRAINT_ID");
                        instance.CautionName = records.GetStringOrDefault("CAUTION_NAME");
                        instance.cautDescription = StringExtractor.xmlStringExtractor(records.GetStringOrDefault("SPECIFIC_ACTION"), "SpecificAction");

                        instance.CautionConstraintValue = new ConstraintValues();

                        instance.CautionConstraintValue.GrossWeight = (long)records.GetInt32OrDefault("GROSS_WEIGHT");
                        instance.CautionConstraintValue.AxleWeight = (long)records.GetInt32OrDefault("AXLE_WEIGHT");
                        instance.CautionConstraintValue.MaxHeight = records.GetSingleOrDefault("MAX_HEIGHT");
                        instance.CautionConstraintValue.MaxLength = records.GetSingleOrDefault("MAX_LENGTH");
                        instance.CautionConstraintValue.MaxWidth = records.GetSingleOrDefault("MAX_WIDTH");
                        instance.CautionConstraintValue.MinSpeed = records.GetSingleOrDefault("MIN_SPEED");
                    }
            );

            return routeCaution;
        }
        #endregion
        #region getAnnotationXML(int revisionId,int versionId,string contentRefNo,int analysisId,string userSchema = UserSchema.Portal)
        /// <summary>
        /// function to return annotation XML
        /// </summary>
        /// <param name="revisionId"></param>
        /// <param name="versionId"></param>
        /// <param name="contentRefNo"></param>
        /// <param name="analysisId"></param>
        /// <param name="userSchema"></param>
        /// <returns>Annotation XML string</returns>
        public static string GetAnnotationXML(int revisionId, int versionId, string contentRefNo, int analysisId, string userSchema = UserSchema.Portal)
        {
            string annotationXml = null;
            List<RoutePartDetails> routePartDet = new List<RoutePartDetails>();

            //fetching route part details based on content reference number or analysis id
            if (analysisId == 0 && contentRefNo != "" && versionId == 0 && revisionId == 0)
            {
                routePartDet = GetRouteDetailForAnalysis(0, 0, contentRefNo, 0, userSchema); //Function to retrieve based on content reference number
            }
            else if (analysisId == 0 && contentRefNo == "" && versionId != 0 && revisionId == 0)
            {
                routePartDet = GetRouteDetailForAnalysis(0, versionId, null, 0, userSchema);
            }

            else if (analysisId != 0 && contentRefNo == "" && versionId == 0 && revisionId == 0)
            {
                routePartDet = GetRouteDetailForAnalysis(analysisId, 0, null, 0, userSchema); //Function to retrieve based on analysis id
            }

            else if (analysisId == 0 && contentRefNo == "" && versionId == 0 && revisionId != 0)
            {
                routePartDet = GetRouteDetailForAnalysis(0, 0, null, revisionId, userSchema); //Function to retrieve based on revision id
            }


            AnalysedAnnotations routeAnnot = new AnalysedAnnotations();

            routeAnnot.AnalysedAnnotationsPart = new List<AnalysedAnnotationsPart>();

            AnalysedAnnotationsPart analysedAnnotationPart = null;

            //More condition's can be added to fetch route details 
            foreach (RoutePartDetails routePart in routePartDet)
            {
                analysedAnnotationPart = new AnalysedAnnotationsPart();
                analysedAnnotationPart.Id = (int)routePart.RouteId;
                analysedAnnotationPart.Name = routePart.RouteName; // constraint route_part Name 
                analysedAnnotationPart.Annotation = GetAnnotationList(analysedAnnotationPart, userSchema);

                if (analysedAnnotationPart.Annotation != null || analysedAnnotationPart.Annotation.Count > 0)
                {
                    routeAnnot.AnalysedAnnotationsPart.Add(analysedAnnotationPart);
                }
            }

            if (routeAnnot.AnalysedAnnotationsPart.Count > 0 && routeAnnot.AnalysedAnnotationsPart != null)
            {
                annotationXml = StringExtractor.AnnotationSerializer(routeAnnot);
            }
            else
            {
                annotationXml = null;
            }

            return annotationXml;
        }

        public static string GetAnnotationXML(List<RoutePartDetails> routePartDet, string userSchema = UserSchema.Portal)
        {
            string annotationXml = null;

            AnalysedAnnotations routeAnnot = new AnalysedAnnotations();

            routeAnnot.AnalysedAnnotationsPart = new List<AnalysedAnnotationsPart>();

            AnalysedAnnotationsPart analysedAnnotationPart = null;

            //More condition's can be added to fetch route details 
            foreach (RoutePartDetails routePart in routePartDet)
            {
                analysedAnnotationPart = new AnalysedAnnotationsPart();
                analysedAnnotationPart.Id = (int)routePart.RouteId;
                analysedAnnotationPart.Name = routePart.RouteName; // constraint route_part Name 
                analysedAnnotationPart.Annotation = GetAnnotationList(analysedAnnotationPart, userSchema);

                if (analysedAnnotationPart.Annotation != null || analysedAnnotationPart.Annotation.Count > 0)
                {
                    routeAnnot.AnalysedAnnotationsPart.Add(analysedAnnotationPart);
                }
            }

            if (routeAnnot.AnalysedAnnotationsPart.Count > 0 && routeAnnot.AnalysedAnnotationsPart != null)
            {
                annotationXml = StringExtractor.AnnotationSerializer(routeAnnot);
            }
            else
            {
                annotationXml = null;
            }

            return annotationXml;
        }
        #endregion
        #region getAnnotationList(AnalysedAnnotationsPart analysedAnnotationPart, string userSchema = UserSchema.Portal)
        /// <summary>
        /// function to fetch annotation list
        /// </summary>
        /// <param name="analysedAnnotationPart"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public static List<Annotation> GetAnnotationList(AnalysedAnnotationsPart analysedAnnotationPart, string userSchema = UserSchema.Portal)
        {
            List<Annotation> annotationList = new List<Annotation>();

            List<Annotation> tmpList = new List<Annotation>();

            Annotation annotObj = null;

            string annotText = "";

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                tmpList,
                userSchema + ".STP_LINK_ID_ARRAY.SP_R_GET_ANNOTATION_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_ID", analysedAnnotationPart.Id, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        annotObj = new Annotation();
                        annotObj.AnnotationId = (int)records.GetLongOrDefault("ANNOTATION_ID");
                        annotObj.AnnotationType = records.GetStringOrDefault("TYPE_ANNOT");
                        annotObj.Road = new AnnotationRoad();
                        annotObj.Road.Name = records.GetStringOrDefault("ROAD_NAME");
                        annotObj.Text = new Text();
                        annotObj.Text.TextValue = new List<string>();
                        annotText = StringExtractor.xmlStringExtractor(records.GetStringOrDefault("ANNOT_TEXT"), "annotation");
                        annotObj.Text.TextValue.Add(annotText);
                        annotObj.AnnotatedEntity = new AnnotatedEntity();
                        annotObj.AnnotatedEntity.Road = new AnnotedEntityRoad();
                        annotObj.AnnotatedEntity.Road.OSGridRef = records.GetStringOrDefault("GRID_REF");
                        annotationList.Add(annotObj);
                    }
            );

            return annotationList;
        }
        #endregion
        #region GetAffectedParties(int analysisId, int notificationId, string contentRefNo, int versionId, int revisionId,int orgId)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="analysisId"></param>
        /// <param name="notificationId"></param>
        /// <param name="contentRefNo"></param>
        /// <param name="versionId"></param>
        /// <param name="revisionId"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public static string GetAffectedParties(int analysisId, int notificationId, string contentRefNo, int versionId, int revisionId, int orgId, string userSchema, string VSOType = "")
        {

            int count = 0;

            List<RoutePartDetails> routePartDet = new List<RoutePartDetails>();

            //fetching route part details based on content reference number or analysis id
            if (analysisId == 0 && contentRefNo != "" && versionId == 0 && revisionId == 0)
            {
                routePartDet = GetRouteDetailForAnalysis(0, 0, contentRefNo, 0, userSchema); //Function to retrieve based on content reference number
            }
            else if (analysisId == 0 && contentRefNo == "" && versionId != 0 && revisionId == 0)
            {
                routePartDet = GetRouteDetailForAnalysis(0, versionId, null, 0, userSchema);// function to retrieve based on version id
            }

            else if (analysisId != 0 && contentRefNo == "" && versionId == 0 && revisionId == 0)
            {
                routePartDet = GetRouteDetailForAnalysis(analysisId, 0, null, 0, userSchema); //Function to retrieve based on analysis id
            }

            else if (analysisId == 0 && contentRefNo == "" && versionId == 0 && revisionId != 0)
            {
                routePartDet = GetRouteDetailForAnalysis(0, 0, null, revisionId, userSchema); //Function to retrieve based on revision id
            }

            AffectedPartiesStructure affectedParties = new AffectedPartiesStructure();

            affectedParties.GeneratedAffectedParties = new List<AffectedPartyStructure>();

            AffectedPartyStructure affectedPartyStruct = null;

            foreach (RoutePartDetails routePart in routePartDet)
            {

                List<AssessmentContacts> affectedCont = GetAffectedContact(routePart.RouteId, 1, notificationId, orgId, userSchema);

                foreach (AssessmentContacts newContact in affectedCont)
                {

                    affectedPartyStruct = new AffectedPartyStructure();

                    affectedPartyStruct.Exclude = false;

                    affectedPartyStruct.ExclusionOutcome = newContact.AffectedExclusionOutcome;

                    affectedPartyStruct.ExclusionOutcomeSpecified = true;

                    affectedPartyStruct.Reason = newContact.AffectedReasonType;

                    affectedPartyStruct.ReasonSpecified = true;

                    affectedPartyStruct.IsPolice = newContact.OrganisationType == "police";

                    affectedPartyStruct.IsRetainedNotificationOnly = false; //hard coded 

                    affectedPartyStruct.Contact = new Domain.RouteAssessment.XmlAffectedParties.ContactReferenceStructure();

                    affectedPartyStruct.Contact.Contact = new Domain.RouteAssessment.XmlAffectedParties.ContactReferenceChoiceStructure();

                    affectedPartyStruct.DispensationStatus = newContact.DispensationStatus;

                    Domain.RouteAssessment.XmlAffectedParties.ContactReferenceChoiceStructure contRef = new Domain.RouteAssessment.XmlAffectedParties.ContactReferenceChoiceStructure();

                    contRef.simpleContactRef = new Domain.RouteAssessment.XmlAffectedParties.SimpleContactReferenceStructure();

                    contRef.simpleContactRef.ContactId = (int)newContact.ContactId;
                    contRef.simpleContactRef.OrganisationId = (int)newContact.OrganisationId;
                    contRef.simpleContactRef.FullName = newContact.ContactName;
                    contRef.simpleContactRef.OrganisationName = newContact.OrganisationName;

                    affectedPartyStruct.Contact.Contact = contRef;

                    if (newContact.OwnerArrangementId != 0)
                    {

                        affectedPartyStruct.OnBehalfOf = new OnBehalfOfStructure();

                        affectedPartyStruct.OnBehalfOf.DelegationId = (int)newContact.OwnerArrangementId;

                        affectedPartyStruct.OnBehalfOf.DelegationIdSpecified = true;

                        affectedPartyStruct.OnBehalfOf.DelegatorsContactId = (int)newContact.ContactId;

                        affectedPartyStruct.OnBehalfOf.DelegatorsContactIdSpecified = true;

                        affectedPartyStruct.OnBehalfOf.DelegatorsOrganisationId = (int)newContact.OwnerOrgId;

                        affectedPartyStruct.OnBehalfOf.DelegatorsOrganisationIdSpecified = true;

                        affectedPartyStruct.OnBehalfOf.DelegatorsOrganisationName = newContact.OwnerOrgName;

                        affectedPartyStruct.OnBehalfOf.RetainNotification = newContact.RetainNotification;

                        affectedPartyStruct.OnBehalfOf.WantsFailureAlert = newContact.RecieveFailures;
                    }

                    try
                    {
                        //checking whether the generated affected party is already present in the list or not
                        count = StringExtractor.checkForDuplicateGenAffectdParty(affectedParties.GeneratedAffectedParties, affectedPartyStruct);

                        if (count == 0)
                        {
                            affectedParties.GeneratedAffectedParties.Add(affectedPartyStruct);
                        }
                    }
                    catch (Exception ex)
                    {
                        string exceptionMessage = $"Exception occurred, Exception: " + ex;
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, exceptionMessage);
                    }
                }
            }

            //This code portion is used to remove affected parties which are police / soa in case the owner opts for VSO type of notification and opts to notify police or soa or both 
            if (VSOType == "police")
            {
                affectedParties.GeneratedAffectedParties.RemoveAll(x => !x.IsPolice);
            }
            else if (VSOType == "soa")
            {
                affectedParties.GeneratedAffectedParties.RemoveAll(x => x.IsPolice);
            }
            else
            {
                //do nothing
            }

            //sorting the generated affected party list based on organisation name
            if (affectedParties.GeneratedAffectedParties.Count != 0 || affectedParties.GeneratedAffectedParties != null)
            {
                var affectedPartylist = affectedParties.GeneratedAffectedParties;

                affectedParties.GeneratedAffectedParties = affectedPartylist.OrderBy(t => t.Contact.Contact.simpleContactRef.OrganisationName).ToList();
            }

            string result = StringExtractor.xmlAffectedPartySerializer(affectedParties);

            return result;
        }

        #endregion
        #region  GetAffectedContact(long? routeId, int routeType, int notifId, int orgId)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="routeType"></param>
        /// <param name="notifId"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public static List<AssessmentContacts> GetAffectedContact(long? routeId, int routeType, int notifId, int orgId, string userSchema = UserSchema.Portal, int revId = 0)
        {
            int isCandidateRt = 0;

            if (m_CandidateRouteFlag||bCandidateRouteFlag) //condition to check whether the call is for candidate route or not
            {
                isCandidateRt = 1;
            }

            List<AssessmentContacts> affectedContacts = new List<AssessmentContacts>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
           affectedContacts,
              userSchema + ".STP_ROUTE_ASSESSMENT.SP_GET_AFFECTED_PARTIES",
              parameter =>
              {
                  parameter.AddWithValue("P_ROUTE_ID", routeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_ROUTE_TYPE", routeType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_NOTIF_ID", notifId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_REV_ID", revId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_CAND_RT", isCandidateRt, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
              },
               (records, instance) =>
               {


                   instance.TotalRecords = (long)records.GetDecimalOrDefault("TOTALRECORDCOUNT");

                   instance.OrganisationId = records.GetLongOrDefault("ORG_ID");

                   instance.OwnerOrgId = records.GetLongOrDefault("OWNER_ORG_ID");

                   instance.OwnerArrangementId = records.GetLongOrDefault("ARRANGMENT_ID");

                   instance.OwnerOrgName = records.GetStringOrDefault("OWNER_ORG_NAME");

                   instance.RetainNotification = records.GetInt16OrDefault("RETAIN_NOTIF") == 1;

                   instance.RecieveFailures = records.GetInt16OrDefault("RECIEVE_FAILURE") == 1;

                   instance.OrganisationName = records.GetStringOrDefault("ORGNAME");

                   instance.ContactName = records.GetStringOrDefault("FULL_NAME");

                   instance.ContactId = (long)records.GetDecimalOrDefault("CONTACT_ID");

                   instance.AddressLine = new List<string>();

                   //getting the address line into a string list 
                   instance.AddressLine.Add(records.GetStringOrDefault("ADDRESSLINE_1"));

                   instance.AddressLine.Add(records.GetStringOrDefault("ADDRESSLINE_2"));

                   instance.AddressLine.Add(records.GetStringOrDefault("ADDRESSLINE_3"));

                   instance.AddressLine.Add(records.GetStringOrDefault("ADDRESSLINE_4"));

                   instance.AddressLine.Add(records.GetStringOrDefault("ADDRESSLINE_5"));

                   instance.PostCode = records.GetStringOrDefault("POSTCODE");

                   instance.Fax = records.GetStringOrDefault("FAX");

                   instance.Mobile = records.GetStringOrDefault("TELEPHONE");

                   instance.Email = records.GetStringOrDefault("EMAIL");

                   instance.Country = records.GetStringOrDefault("COUNTRY");

                   instance.AffectedReasonType = AffectedPartyReasonType.newlyaffected;

                   instance.AffectedExclusionOutcome = AffectedPartyReasonExclusionOutcomeType.newlyaffected;

                   //fetching list of dispensation 

                   instance.DispensationId = GetDispensationStatusType(orgId, (int)instance.OrganisationId);
                   if (instance.DispensationId.Count >= 1)
                   {
                       instance.DispensationStatus = DispensationStatusType.somematching;
                       instance.DispensationStatusType = "Some Matching";
                   }

                   else
                   {
                       instance.DispensationId.Add(0);
                   }

                   instance.OrganisationType = records.GetStringOrDefault("ORG_TYPE");

                   instance.UserAddedAffectedContact = 1; // 
               }
               );

            return affectedContacts;
        }
        #endregion
        #region getDispensationStatusType(int grantee, int grantor)
        /// <summary>
        /// retrieving dispensation status type.
        /// </summary>
        /// <param name="grantee">haulier Id</param>
        /// <param name="grantor">soa,police id</param>
        /// <returns></returns>
        public static List<long> GetDispensationStatusType(int grantee, int grantor)
        {

            List<long> dispensationId = new List<long>();
            List<long> result = new List<long>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               result,
                 UserSchema.Portal + ".STP_ROUTE_ASSESSMENT.SP_GET_DISPENSATION_STATUS",
                  parameter =>
                  {
                      parameter.AddWithValue("GRANTEE_ORG", grantee, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("GRANTOR_ORG", grantor, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767); // default for application route's route parts
                      parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                  },
                   (records, instance) =>
                   {

                       dispensationId.Add(records.GetLongOrDefault("DISPENSATION_ID"));
                   });

            return dispensationId;
        }
        #endregion
        #region  Business logic to generate the Xml string of affected road's
        /*
         * Business logic to generate the Xml string of affected road's
         * 
         * */
        #region fetchAffectedRoadXml(int revisionId, int versionId, string contentRefNo, int analysisId)
        /// <summary>
        /// function to generate Analysed Roads XML string 
        /// </summary>
        /// <param name="revisionId"></param>
        /// <param name="versionId"></param>
        /// <param name="contentRefNo"></param>
        /// <param name="analysisId"></param>
        /// <returns></returns>
        public static string FetchAffectedRoadXml(int revisionId, int versionId, string contentRefNo, int analysisId, string userSchema)
        {

            List<RoutePartDetails> routePartDet = new List<RoutePartDetails>();
            //fetching route part details based on content reference number or analysis id
            if (analysisId == 0 && contentRefNo != "" && versionId == 0 && revisionId == 0)
            {
                routePartDet = GetRouteDetailForAnalysis(0, 0, contentRefNo, 0, userSchema); //Function to retrieve based on content reference number
            }
            else if (analysisId == 0 && contentRefNo == "" && versionId != 0 && revisionId == 0)
            {
                routePartDet = GetRouteDetailForAnalysis(0, versionId, null, 0, userSchema);
            }

            else if (analysisId != 0 && contentRefNo == "" && versionId == 0 && revisionId == 0)
            {
                routePartDet = GetRouteDetailForAnalysis(analysisId, 0, null, 0, userSchema); //Function to retrieve based on analysis id
            }

            else if (analysisId == 0 && contentRefNo == "" && versionId == 0 && revisionId != 0)
            {
                routePartDet = GetRouteDetailForAnalysis(0, 0, null, revisionId, userSchema); //Function to retrieve based on revision id
            }

            AnalysedRoadsRoute analysedRoads = new AnalysedRoadsRoute();

            analysedRoads.AnalysedRoadsPart = new List<AnalysedRoadsPart>();

            foreach (RoutePartDetails routePart in routePartDet)
            {
                analysedRoads.AnalysedRoadsPart.Add(FetchAffectedRoads((int)routePart.RouteId, routePart.RouteName, userSchema));
            }

            string XmlString = StringExtractor.AnalysedRoadsSerializer(analysedRoads);

            return XmlString;
        }

        public static string FetchAffectedRoadXml(List<RoutePartDetails> routePartDet, string userSchema)
        {

            AnalysedRoadsRoute analysedRoads = new AnalysedRoadsRoute();

            analysedRoads.AnalysedRoadsPart = new List<AnalysedRoadsPart>();

            foreach (RoutePartDetails routePart in routePartDet)
            {
                analysedRoads.AnalysedRoadsPart.Add(FetchAffectedRoads((int)routePart.RouteId, routePart.RouteName, userSchema));
            }

            string XmlString = StringExtractor.AnalysedRoadsSerializer(analysedRoads);

            return XmlString;
        }
        #endregion

        #region  fetchAffectedRoads(int routeId, string routeName)
        /// <summary>
        /// function to call stored procedure's and fetch analysed road's
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="routeName"></param>
        /// <returns></returns>
        public static AnalysedRoadsPart FetchAffectedRoads(int routeId, string routeName, string userSchema)
        {
            //new list used to store the result from database for further manipulation and calculating distance.
            List<AffectedRoadDetail> affectedRoads = new List<AffectedRoadDetail>();

            string tmpRoadName1 = null, tmpRoadName2 = null;

            string strTmpPoliceName1 = null;

            string strTmpMgrName1 = null;

            long dist = 0;

            //List to store the affected roads and the total calculated distance.
            List<AffectedRoadDetail> affectedRoadDistList = new List<AffectedRoadDetail>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               affectedRoads,
               userSchema + ".STP_ROUTE_ASSESSMENT.SP_FETCH_AFFECTED_ROADS",
               parameter =>
               {
                   parameter.AddWithValue("P_ROUTE_ID", routeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);

               },
               (records, instance) =>
               {
                   instance.RoadName = records.GetStringOrDefault("ROADNAME");
                   instance.LinkNo = Convert.ToInt16(records["LINK_NO"]);
                   try
                   {
                       instance.LinkId = records.GetLongOrDefault("LINK_ID");
                   }
                   catch
                   {
                       //Some time the type of link id coming as decimal
                       instance.LinkId = long.Parse(records.GetDecimalOrDefault("LINK_ID").ToString());
                   }
                   instance.Distance = (long)records.GetDecimalOrDefault("DISTANCE");

                   #region Excluded till migration is completed !! do not delete the comented portions
                   instance.PoliceContactId = (long)records.GetDecimalOrDefault("POLICE_CONTACT_ID");
                   instance.PoliceOrgId = (long)records.GetDecimalOrDefault("POLICE_FORCE_ID");
                   instance.PoliceForceName = records.GetStringOrDefault("POLICE_FORCE_NAME");
                   instance.ManagerContactId = (long)records.GetDecimalOrDefault("MANAGER_CONTACT_ID");
                   instance.ManagerOrgId = (long)records.GetDecimalOrDefault("MANAGER_ID");
                   instance.ManagerName = records.GetStringOrDefault("MANAGER_NAME");
                   instance.OnBehalOfContId = (long)records.GetDecimalOrDefault("DELEGATING_CONTACT_ID");
                   instance.OnBehalOfArrangId = (long)records.GetDecimalOrDefault("ARRANGEMENT_ID");
                   instance.OnBehalOfOrgId = (long)records.GetDecimalOrDefault("DELEGATING_ID");
                   try
                   {
                       instance.RetainNotification = records.GetInt16OrDefault("RETAIN_NOTIFICATION") != 0;
                   }
                   catch
                   {
                       instance.RetainNotification = (long)records.GetDecimalOrDefault("RETAIN_NOTIFICATION") != 0;
                   }
                   try
                   {
                       instance.WantFailure = records.GetInt16OrDefault("ACCEPT_FAILURES") != 0;
                   }
                   catch
                   {
                       instance.WantFailure = (long)records.GetDecimalOrDefault("ACCEPT_FAILURES") != 0;
                   }
                   instance.DelegOrgName = records.GetStringOrDefault("DELEGATING_NAME");
                   #endregion
               });

            if (affectedRoads.Count > 0) //condition to check whether there are any affected roads or not
            {
                affectedRoads[0].StartPointLink = affectedRoads[0].LinkId;

                affectedRoads[affectedRoads.Count - 1].EndPointLink = affectedRoads[affectedRoads.Count - 1].LinkId;

                AffectedRoadDetail afftdRod = null;

                AffectedRoadResponsibleContact affectdRoadResponsible = null;

                AffectedRoadResponsibleDelegation affectdRoadResponsibleDeleg = null;

                List<string> roadOwnerList = null;
                long temp_Link_no = -1;
                long temp_Link_id = 0;
                foreach (AffectedRoadDetail affectdRoad in affectedRoads)
                {

                    tmpRoadName1 = affectdRoad.RoadName;

                    if (tmpRoadName1 != tmpRoadName2) //check to avoid duplicate road names being appearing in the list simultaneously
                    {
                        //new instance of object created for saving the value's
                        afftdRod = new AffectedRoadDetail();

                        affectedRoadDistList.Add(afftdRod);

                        afftdRod.RoadName = tmpRoadName1; //inserting the road name

                        tmpRoadName2 = tmpRoadName1;

                        roadOwnerList = new List<string>(); //emptying the list

                        dist = 0; //uncomment if total distance of each road is to be shown else comment it for displaying total distance travelled upto each road point
                    }
                    if (tmpRoadName1 == tmpRoadName2)
                    {

                        #region

                        //saving the police organisation name to a temp variable
                        strTmpPoliceName1 = affectdRoad.PoliceForceName;
                        //saving manager organisation name to a temp variable
                        strTmpMgrName1 = affectdRoad.ManagerName;

                        //police contacts
                        if (strTmpPoliceName1 != null && strTmpPoliceName1 != "" && roadOwnerList != null)
                        {
                            if (!roadOwnerList.Contains(strTmpPoliceName1)) // checking whether the police organisation is present in the list if not then the organisation is added as the road responsible party
                            {
                                affectdRoadResponsible = new AffectedRoadResponsibleContact();

                                affectdRoadResponsible.RespContId = affectdRoad.PoliceContactId;
                                affectdRoadResponsible.RespOrgId = affectdRoad.PoliceOrgId;
                                affectdRoadResponsible.RespOrgName = affectdRoad.PoliceForceName;

                                affectdRoadResponsibleDeleg = new AffectedRoadResponsibleDelegation();

                                affectdRoadResponsibleDeleg.DelegOrgName = affectdRoad.DelegOrgName;
                                affectdRoadResponsibleDeleg.RespOnBehalOfContId = affectdRoad.OnBehalOfContId;
                                affectdRoadResponsibleDeleg.RespOnBehalOfOrgId = affectdRoad.OnBehalOfOrgId;
                                affectdRoadResponsibleDeleg.RespOnBehalOfArrangId = affectdRoad.OnBehalOfArrangId;
                                affectdRoadResponsibleDeleg.RetainNotification = affectdRoad.RetainNotification;
                                affectdRoadResponsibleDeleg.WantFailure = affectdRoad.WantFailure;

                                affectdRoadResponsible.AffectedRoadDelegation.Add(affectdRoadResponsibleDeleg);

                                afftdRod.AffectedRoadContactList.Add(affectdRoadResponsible);

                                roadOwnerList.Add(strTmpPoliceName1); // adding the police organisation inside the list
                            }
                        }

                        if (strTmpMgrName1 != null && strTmpMgrName1 != "" && roadOwnerList != null)
                        {
                            if (!roadOwnerList.Contains(strTmpMgrName1)) //checking whether the soa organisation is present in the list if not then the organisation is added as the road responsible party 
                            {
                                affectdRoadResponsible = new AffectedRoadResponsibleContact()
                                {
                                    RespContId = affectdRoad.ManagerContactId,
                                    RespOrgId = affectdRoad.ManagerOrgId,
                                    RespOrgName = affectdRoad.ManagerName
                                };                                

                                affectdRoadResponsibleDeleg = new AffectedRoadResponsibleDelegation()
                                {
                                    DelegOrgName = affectdRoad.DelegOrgName,
                                    RespOnBehalOfContId = affectdRoad.OnBehalOfContId,
                                    RespOnBehalOfOrgId = affectdRoad.OnBehalOfOrgId,
                                    RespOnBehalOfArrangId = affectdRoad.OnBehalOfArrangId,
                                    RetainNotification = affectdRoad.RetainNotification,
                                    WantFailure = affectdRoad.WantFailure
                                };

                                affectdRoadResponsible.AffectedRoadDelegation.Add(affectdRoadResponsibleDeleg);

                                afftdRod.AffectedRoadContactList.Add(affectdRoadResponsible);

                                roadOwnerList.Add(strTmpMgrName1); // adding the soa organisation inside the list
                            }
                        }

                        #endregion
                        //check here the link no whether same as the previous link number if no then go for distance calculation

                        //distance is calculated.
                        if (temp_Link_no != affectdRoad.LinkNo)
                        {
                            dist = dist + affectdRoad.Distance;

                        }
                        if (affectdRoad.LinkNo == 0 && temp_Link_id != affectdRoad.LinkId)
                        {
                            temp_Link_no = -1;//a single Link can form a route path and the link no can be 0 another path tha immidiately follows 
                            //will have start link and the link no will be 0 to consider this case except the Link no having 0 value for different link id
                        }
                        else
                        {
                            temp_Link_no = affectdRoad.LinkNo;
                        }
                        temp_Link_id = affectdRoad.LinkId;
                        //calculated distance from object added to new object
                        if (afftdRod != null)
                        {
                            afftdRod.Distance = dist;
                        }
                        
                    }
                }

                List<AffectedRoadPointDet> affectdRoadPointList = new List<AffectedRoadPointDet>();

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                   affectdRoadPointList,
                   userSchema + ".STP_ROUTE_ASSESSMENT.SP_FETCH_AFFCTD_ROAD_POINT_DET",
                   parameter =>
                   {
                       parameter.AddWithValue("P_ROUTE_ID", routeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_START_LINK", affectedRoads[0].StartPointLink, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_END_LINK", affectedRoads[affectedRoads.Count - 1].EndPointLink, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);

                   },
                   (records, instance) =>
                   {
                       instance.RoutePointType = records.GetInt32OrDefault("ROUTE_POINT_TYPE");
                       instance.TruePointGeom = records.GetGeometryOrNull("TRUE_POINT_GEOMETRY");
                       instance.Description = records.GetStringOrDefault("DESCR");
                       //generating grid ref from SDO_POINT X , Y
                       //instance.GridRef = Convert.ToString(instance.TruePointGeom.sdo_point.X) + "," + Convert.ToString(instance.TruePointGeom.sdo_point.Y);
                   });


                AnalysedRoadsPart analysedRoadsPart;

                analysedRoadsPart = GenerateAffectedRoadsXml(affectedRoadDistList, affectdRoadPointList, routeId, routeName);

                return analysedRoadsPart;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region generateAffectedRoadsXml(List<AffectedRoadDetail> affectedRoadDistList, List<AffectedRoadPointDet> affectdRoadPointList, long routeId, string partName)
        /// <summary>
        /// function to generate xml serializable object
        /// </summary>
        /// <param name="affectedRoadDistList"></param>
        /// <param name="affectdRoadPointList"></param>
        /// <param name="routeId"></param>
        /// <param name="partName"></param>
        /// <returns></returns>
        public static AnalysedRoadsPart GenerateAffectedRoadsXml(List<AffectedRoadDetail> affectedRoadDistList, List<AffectedRoadPointDet> affectdRoadPointList, long routeId, string partName)
        {
            AnalysedRoadsPart analysedRoadsPart = new AnalysedRoadsPart(); // RoadsPart Object creation

            analysedRoadsPart.ComparisonId = 0; //setting comparison id 0

            analysedRoadsPart.Id = routeId;

            analysedRoadsPart.Name = partName;

            analysedRoadsPart.SubPart = new List<SubPart>(); //Creating new instance of sub part object

            SubPart analysedRoadsSubPart = new SubPart();

            List<PathRoadsPathSegment> analysedRoadsPath = new List<PathRoadsPathSegment>(); //creating new instance

            List<List<PathRoadsPathSegment>> roadsListObj = new List<List<PathRoadsPathSegment>>(); //creating new instance

            analysedRoadsSubPart.Roads = new List<List<PathRoadsPathSegment>>(); // creating new instance roads list class

            PathRoadsPathSegment analysedRoadsPathObj = null;  //setting null to the PathRoadsPathSegment

            //fetching affected road details from affected Road List and saving it into the fields.
            foreach (AffectedRoadDetail afftdRoadDet in affectedRoadDistList)
            {
                analysedRoadsPathObj = new PathRoadsPathSegment();

                Domain.RouteAssessment.XmlAnalysedRoads.Road RoadObj = new Domain.RouteAssessment.XmlAnalysedRoads.Road();

                RoadIdentity roadIdentity = new RoadIdentity();

                //
                if (afftdRoadDet.RoadName != "Unclassified")
                {
                    roadIdentity.Name = afftdRoadDet.RoadName;
                }
                //when the road name is not identifiable the following field is set to true
                else
                {
                    roadIdentity.Name = afftdRoadDet.RoadName;
                    roadIdentity.Unidentified = true;
                }

                RoadObj.RoadIdentity = roadIdentity;

                RoadObj.Distance = new Distance();

                RoadObj.Distance.Unit = "metre";

                RoadObj.Distance.Value = afftdRoadDet.Distance;

                analysedRoadsPathObj.Road = RoadObj;

                List<RoadResponsibleParty> roadResponsibleList = new List<RoadResponsibleParty>();

                RoadResponsibleParty roadResponsibleContact = null;

                foreach (AffectedRoadResponsibleContact afftdContct in afftdRoadDet.AffectedRoadContactList)
                {
                    roadResponsibleContact = new RoadResponsibleParty();
                    roadResponsibleContact.ContactId = (int)afftdContct.RespContId;
                    roadResponsibleContact.OrganisationId = (int)afftdContct.RespOrgId;
                    roadResponsibleContact.OrganisationName = afftdContct.RespOrgName;
                    roadResponsibleList.Add(roadResponsibleContact);
                }

                if (roadResponsibleList.Count > 0)
                {
                    analysedRoadsPathObj.Road.RoadResponsibility = roadResponsibleList;
                }
                analysedRoadsPath.Add(analysedRoadsPathObj);
            }

            //Fetching AffectedRoad point details from list and inserting into existing analysed roads list.
            foreach (AffectedRoadPointDet affectedPoint in affectdRoadPointList)
            {
                PathRoadsPathSegment analysedRoadsPointPathObj = new PathRoadsPathSegment();

                RoutePointStructure routePoint = new RoutePointStructure();

                Point pnt = new Point();

                if (affectedPoint.RoutePointType == 239001)
                {

                    routePoint.PointType = RoutePointType.start;

                    routePoint.IsBroken = false;

                    pnt.GridRef = affectedPoint.GridRef;

                    pnt.Description = affectedPoint.Description;

                    routePoint.Point = pnt;

                    analysedRoadsPointPathObj.RoutePoint = routePoint;

                    analysedRoadsPath.Insert(0, analysedRoadsPointPathObj);
                }

                if (affectedPoint.RoutePointType == 239002)
                {
                    routePoint.PointType = RoutePointType.end;

                    routePoint.IsBroken = false;

                    pnt.GridRef = affectedPoint.GridRef;

                    pnt.Description = affectedPoint.Description;

                    routePoint.Point = pnt;

                    analysedRoadsPointPathObj.RoutePoint = routePoint;

                    analysedRoadsPath.Insert(analysedRoadsPath.Count, analysedRoadsPointPathObj);
                }
            }


            roadsListObj.Add(analysedRoadsPath);

            analysedRoadsSubPart.Roads = roadsListObj;

            analysedRoadsPart.SubPart.Add(analysedRoadsSubPart);

            return analysedRoadsPart;
        }

        #endregion

        #endregion
        #region
        /// <summary>
        /// check the driving instruction is updated in database or not after successful generation of service
        /// </summary>
        /// <param name="anal_id"></param>
        /// <param name="anal_type"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public static long CheckDrivingInstruction(long anal_id, int anal_type, string userSchema)
        {
            long errorcode = 5;
            RouteAssessmentModel objRouteAssessmentModel;
            objRouteAssessmentModel = GetDriveInstructionsinfo(anal_id, anal_type, userSchema);
            if (objRouteAssessmentModel.DriveInst == null)
            {
                errorcode = 7;
            }
            return errorcode;
        }
        #endregion

        #region call to DrivingInstructionsInterface
        public long GenerateRouteDescApi(DrivingInsReq drivingInsReq,string userSchema)
        {
            long errorcode =0;
            try
            {
                //api call to new service   
                DrivInstParams drivInstParams = new DrivInstParams()
                {
                    DrivingInstructionReq = drivingInsReq,
                    UserSchema = userSchema
                };
                HttpResponseMessage response = new HttpClient().PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["APIGatewayUrl"]}" +
                    $"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
                    $"/DrivingInstructor/GenerateDI",
                    drivInstParams).Result;
                
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    errorcode = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"DrivingInstructor/GenerateDI, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"DrivingInstructor/GenerateDI, Exception: {0}", ex);
            }
            return errorcode;
        }
        #endregion
        

        #region static List<int> getCountryId(int routeID = 0)
        public static List<int> GetCountryId(int routeID = 0)
        {
            List<int> result = new List<int>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
              result,
             UserSchema.Portal + ".SP_GET_COUNTRYNAME",
               parameter =>
               {
                   parameter.AddWithValue("P_ROUTE_ID", routeID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
               (records, instance) =>
               {

                   result.Add(records.GetInt32OrDefault("COUNTRY_ID"));
               }
            );
            return result;
        }
        #endregion

        #region fetchPreviousAffectedList(int analysisId, int analType)
        /// <summary>
        /// function to fetch affected List from previous version of movement.
        /// </summary>
        /// <param name="analysisId">newly generated AnalysisId</param>
        /// <returns>return's the previous analysis id of the movement along with the Affected parties xml </returns>
        public static AnalysedRoute FetchPreviousAffectedList(int analysisId, int analysisType, string userSchema)
        {
            long prevAnalId = 0;

            string xmlAffectedParties = null;

            AnalysedRoute analysedRouteInfo = new AnalysedRoute();

            RouteAssessmentModel driveinstgridobj = new RouteAssessmentModel();
            
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                driveinstgridobj,
                userSchema + ".STP_ROUTE_ASSESSMENT.SP_FETCH_MOVEMENT_INFO",
                parameter =>
                {
                    parameter.AddWithValue("P_ANALYSIS_ID", analysisId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ANAL_TYPE", analysisType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);

                },
                record =>
                {
                    switch (analysisType)
                    {
                        case 1:
                            driveinstgridobj.DriveInst = record.GetByteArrayOrNull("DRIVING_INSTRUCTIONS");
                            prevAnalId = record.GetLongOrDefault("ANALYSIS_ID");
                            break;

                        case 2:
                            driveinstgridobj.RouteDescription = record.GetByteArrayOrNull("ROUTE_DESCRIPTION");
                            prevAnalId = record.GetLongOrDefault("ANALYSIS_ID");
                            break;

                        case 3:
                            driveinstgridobj.AffectedStructure = record.GetByteArrayOrNull("AFFECTED_STRUCTURES");
                            prevAnalId = record.GetLongOrDefault("ANALYSIS_ID");
                            break;

                        case 4:
                            driveinstgridobj.Cautions = record.GetByteArrayOrNull("CAUTIONS");
                            prevAnalId = record.GetLongOrDefault("ANALYSIS_ID");
                            break;

                        case 5:
                            driveinstgridobj.Constraints = record.GetByteArrayOrNull("AFFECTED_CONSTRAINTS");
                            prevAnalId = record.GetLongOrDefault("ANALYSIS_ID");
                            break;

                        case 6:
                            driveinstgridobj.Annotation = record.GetByteArrayOrNull("ANNOTATIONS");
                            prevAnalId = record.GetLongOrDefault("ANALYSIS_ID");
                            break;

                        case 7:
                            driveinstgridobj.AffectedParties = record.GetByteArrayOrNull("AFFECTED_PARTIES");
                            prevAnalId = record.GetLongOrDefault("ANALYSIS_ID");
                            break;

                    }


                });

            if (driveinstgridobj.AffectedParties != null)
            {
                xmlAffectedParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(driveinstgridobj.AffectedParties));

                analysedRouteInfo.NewAnalysisId = analysisId;
                //the below contents are from previous route analysis
                analysedRouteInfo.PreviousAnalysisId = prevAnalId;
                analysedRouteInfo.RouteAnalysisXml = xmlAffectedParties;
                analysedRouteInfo.RouteAnalysisBlob = driveinstgridobj.AffectedParties;
                analysedRouteInfo.AnalysisType = AnalysisType.affectedparties;
                return analysedRouteInfo;
            }
            else
            {
                return null;
            }

        }
        #endregion
        #region MovementClearRouteAssessment
        public static bool MovementClearRouteAssessment(long revisionId, string userSchema)
        {
            decimal result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
            result,
              userSchema + ".SP_CLEAR_MVMNT_RT_ASMNT",
                parameter =>
                {
                    parameter.AddWithValue("R_REVISION_ID", revisionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("R_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
               (records, instance) =>
               {
                   result = records.GetDecimalOrDefault("P_RESULT");
               }
            );
            if (result == 0)
                return false;
            else
                return true;
        }
        #endregion
        #region UpdateRouteAssessmentAlsat
        public static int UpdateRouteAnalysis(string contentRefNo, int notificationId, int revisionId, int versionId, int orgId, int analysisId, int analType, string userSchema, bool isCandidateRoute = false, string VSOType = "", int routeId = 0, AssessmentOutput assessmentResult = null)
        {
            RouteAnalysisXml assessedXmlStrings = new RouteAnalysisXml();
            RouteAssessmentModel routeAssess = new RouteAssessmentModel();

            //assigning candidate route boolean value into global flag variable
            bCandidateRouteFlag = isCandidateRoute;

            long status = 0;

            switch (analType)
            {
                case 1:

                    status = GenerateDrivingInstructions(analysisId, contentRefNo, versionId, revisionId, userSchema);

                    break;

                case 2://To be filled
                    break;

                case 3:
                    assessedXmlStrings.XmlAnalysedStructure = GetStructureForAnalysis(analysisId, contentRefNo, orgId, versionId, revisionId, userSchema, routeId, assessmentResult);
                    if (assessedXmlStrings.XmlAnalysedStructure == null)
                    {
                        

                        routeAssess.Constraints = null;

                        status = UpdateAnalysedRoute(routeAssess.AffectedStructure, analysisId, 3, userSchema);

                        break;
                    }
                    routeAssess.AffectedStructure = StringExtractor.ZipAndBlob(assessedXmlStrings.XmlAnalysedStructure);

                    status = UpdateAnalysedRoute(routeAssess, analysisId, userSchema); //Updating 

                    break;

                case 4://To be filled for cautions
                    assessedXmlStrings.XmlAnalysedCautions = CautionGenerator(0, contentRefNo, orgId, versionId, revisionId, userSchema);

                    if (assessedXmlStrings.XmlAnalysedCautions == null)
                    {
                        status = 0;
                        routeAssess.Cautions = null;
                        status = UpdateAnalysedRoute(routeAssess.Cautions, analysisId, 4, userSchema);
                        break;
                    }
                    else
                    {
                        routeAssess.Cautions = StringExtractor.ZipAndBlob(assessedXmlStrings.XmlAnalysedCautions);

                        status = UpdateAnalysedRoute(routeAssess, analysisId, userSchema);
                    }
                    break;

                case 5: //case for affected constraint's
                    assessedXmlStrings.XmlAnalysedConstraints = ConstraintSaver(contentRefNo, versionId, 0, revisionId, userSchema);

                    if (assessedXmlStrings.XmlAnalysedConstraints == null)
                    {
                        status = 0;

                        routeAssess.Constraints = null;

                        status = UpdateAnalysedRoute(routeAssess.Constraints, analysisId, 5, userSchema);
                        break;
                    }
                    else
                    {
                        routeAssess.Constraints = StringExtractor.ZipAndBlob(assessedXmlStrings.XmlAnalysedConstraints);

                        status = UpdateAnalysedRoute(routeAssess, analysisId, userSchema);
                    }
                    break;
                case 6:
                    assessedXmlStrings.XmlAnalysedAnnotations = GetAnnotationXML(revisionId, versionId, contentRefNo, 0, userSchema);

                    if (assessedXmlStrings.XmlAnalysedAnnotations == null)
                    {
                        status = 0;
                        routeAssess.Annotation = null;
                        status = UpdateAnalysedRoute(routeAssess.Annotation, analysisId, 5, userSchema);
                        break;
                    }
                    else
                    {
                        routeAssess.Annotation = StringExtractor.ZipAndBlob(assessedXmlStrings.XmlAnalysedAnnotations);
                        status = UpdateAnalysedRoute(routeAssess, analysisId, userSchema);
                    }
                    break;

                case 7: //case for affected partie's
                    //getAffected parties function generates affected parties for a given route the list includes both SOA, Police and NH
                    assessedXmlStrings.XmlAffectedParties = GetAffectedParties(0, notificationId, contentRefNo, versionId, revisionId, orgId, userSchema, VSOType);

                    routeAssess.AffectedParties = StringExtractor.ZipAndBlob(assessedXmlStrings.XmlAffectedParties);

                    status = UpdateAnalysedRoute(routeAssess, analysisId, userSchema);
                    break;

                case 8: //case for affected road's
                    //condition to generate affected roads for notification
                    if (revisionId == 0 && versionId == 0 && contentRefNo != "")
                    {
                        assessedXmlStrings.XmlAffectedRoads = FetchAffectedRoadXml(0, 0, contentRefNo, 0, userSchema);
                    }
                    else if (revisionId != 0 && versionId == 0)
                    {
                        assessedXmlStrings.XmlAffectedRoads = FetchAffectedRoadXml(revisionId, 0, "", 0, userSchema);
                    }
                    else if (versionId != 0)
                    {
                        assessedXmlStrings.XmlAffectedRoads = FetchAffectedRoadXml(0, versionId, "", 0, userSchema); //VR -1 
                    }

                    routeAssess.AffectedRoads = StringExtractor.ZipAndBlob(assessedXmlStrings.XmlAffectedRoads);
                    status = UpdateAnalysedRoute(routeAssess, analysisId, userSchema);
                    break;
            }

            return (int)status;
        }
        #endregion

        public static int UpdateAlsatAnalysis(int analysisId, int routeId, string userSchema, AssessmentOutput assessmentResult = null)
        {
            RouteAnalysisXml assessedXmlStrings = new RouteAnalysisXml();
            RouteAssessmentModel routeAssess = new RouteAssessmentModel();

            long status = 0;

            assessedXmlStrings.XmlAnalysedStructure = GetStructureForAlsatAnalysis(analysisId, routeId, userSchema, assessmentResult);
            if (assessedXmlStrings.XmlAnalysedStructure == null)
            {
                status = 0;
                routeAssess.Constraints = null;
                status = UpdateAnalysedRoute(routeAssess.AffectedStructure, analysisId, 3, userSchema);
            }
            routeAssess.AffectedStructure = StringExtractor.ZipAndBlob(assessedXmlStrings.XmlAnalysedStructure);

            status = UpdateAnalysedRoute(routeAssess, analysisId, userSchema); //Updating 

            return (int)status;
        }

        public static string GetStructureForAnalysis(int analysisId, string contentRefNo, int orgId, long versionId, int revisionId, string userSchema, int routeId = 0, AssessmentOutput assessmentResult = null)
        {
            AnalysedStructures structures = new AnalysedStructures();

            List<RoutePartDetails> routePartDet = new List<RoutePartDetails>();
            //fetching route part details based on content reference number or analysis id
            if (contentRefNo != "" && routeId == 0)
            {
                routePartDet = GetRouteDetailForAnalysis(0, 0, contentRefNo, 0, userSchema); //Function to retrieve based on content reference number
            }
            else if (versionId != 0 && routeId == 0)
            {
                routePartDet = GetRouteDetailForAnalysis(0, versionId, null, 0, userSchema); //Function to retrieve based on version Id
            }
            else if (analysisId != 0 && routeId == 0)
            {
                routePartDet = GetRouteDetailForAnalysis(analysisId, 0, null, revisionId, userSchema); //Function to retrieve based on analysis id
            }

            else if (revisionId != 0 && routeId == 0)
            {
                routePartDet = GetRouteDetailForAnalysis(0, 0, null, revisionId, userSchema); //Function to retrieve based on revision id
            }
            else if (analysisId != 0 && routeId != 0)
            {
                routePartDet = GetRouteDetailForAnalysis(analysisId, 0, null, 0, userSchema, routeId); //Function to retrieve based on route id
            }

            //More condition's can be added to fetch route details 

            structures.AnalysedStructuresPart = new List<AnalysedStructuresPart>();

            //getting existing ALSAT assessment values - part 1
            RouteAssessmentModel objRouteAssessmentModel = new RouteAssessmentModel();
            objRouteAssessmentModel = GetDriveInstructionsinfo(analysisId, 3, userSchema);

            AnalysedStructures newAnalysedStructures = null;
            if (objRouteAssessmentModel.AffectedStructure != null)
            {
                string affectedStructuresxml = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.AffectedStructure));
                newAnalysedStructures = StringExtractor.XmlAffectedStructuresDeserializer(affectedStructuresxml);
            }

            foreach (RoutePartDetails routePart in routePartDet)
            {
                //getting existing ALSAT assessment values - part 2
                if (newAnalysedStructures != null && newAnalysedStructures.AnalysedStructuresPart.Count > 0)
                {
                    if (assessmentResult == null)
                    {
                        assessmentResult = new AssessmentOutput();
                        assessmentResult.Properties = new Properties();
                        assessmentResult.Properties.EsdalStructure = new List<EsdalStructure>();
                    }

                    foreach (var analysedStructuresPart in newAnalysedStructures.AnalysedStructuresPart)
                    {
                        if (analysedStructuresPart.Id == routePart.RouteId)
                        {
                            foreach (var structure in analysedStructuresPart.Structure)
                            {
                                if (structure.AlsatAppraisal != null && structure.AlsatAppraisal.StructureKey != null)
                                {
                                    //check if newly available in assessment result
                                    bool existFlag = false;

                                    foreach (var result in assessmentResult.Properties.EsdalStructure)
                                    {
                                        if (result.Esrn == structure.AlsatAppraisal.ESRN && result.RouteId == analysedStructuresPart.Id)
                                        {
                                            existFlag = true;
                                            break;
                                        }
                                    }

                                    if (!existFlag)
                                    {
                                        assessmentResult.Properties.EsdalStructure.Add(new EsdalStructure
                                        {
                                            Esrn = structure.ESRN,
                                            StructureKey =structure.AlsatAppraisal.StructureKey,
                                            StructureCalculationType = structure.AlsatAppraisal.StructureCalculationType,
                                            ResultStructure = structure.AlsatAppraisal.ResultStructure,
                                            Sf = structure.AlsatAppraisal.Sf,
                                            CommentsForHaulier = structure.AlsatAppraisal.CommentsForHaulier,
                                            AssessmentComments = EnumExtensions.ToEnum<AssessmentComments>(structure.AlsatAppraisal.AssessmentComments),
                                            RouteId = analysedStructuresPart.Id
                                        });
                                    }
                                }
                            }
                        }
                    }
                }

                //Getting affected structure list for each individual route part
                AnalysedStructuresPart analstruct = new AnalysedStructuresPart();

                analstruct.ComparisonId = 1123; //Sample comparison id need to be updated with new comparison id

                analstruct.Id = (int)routePart.RouteId; //Route id for route routeId

                analstruct.AnalysedStructuresPartName = routePart.RouteName; //Route Name routeName

                analstruct.Structure = new List<Structure>();
                //getting structure list for given set of structure's
                analstruct.Structure = GetStructureListForAnalysis(analstruct.Id, 0, orgId, userSchema, assessmentResult);

                //Commenting the conditions to save empty analysis even if their are no structures generated for some empty routes
                //storing route part that has structures
                structures.AnalysedStructuresPart.Add(analstruct);
            }
            string structureXmlString = StringExtractor.xmlStructureSerializer(structures);

            return structureXmlString; //returns structureXmlString after it's generated
        }

        public static string GetStructureForAlsatAnalysis(int analysisId, int routeId, string userSchema, AssessmentOutput assessmentResult = null)
        {
            AnalysedStructures structures = new AnalysedStructures();
            List<RoutePartDetails> routePartDet = GetRouteDetailForAnalysis(analysisId, 0, null, 0, userSchema, routeId); //Function to retrieve based on analysis id and route id

            //More condition's can be added to fetch route details 
            structures.AnalysedStructuresPart = new List<AnalysedStructuresPart>();

            //getting existing ALSAT assessment values - part 1
            RouteAssessmentModel objRouteAssessmentModel = GetDriveInstructionsinfo(analysisId, 3, userSchema);

            AnalysedStructures newAnalysedStructures = null;
            if (objRouteAssessmentModel.AffectedStructure != null)
            {
                string affectedStructuresxml = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.AffectedStructure));
                newAnalysedStructures = StringExtractor.XmlAffectedStructuresDeserializer(affectedStructuresxml);
            }

            foreach (RoutePartDetails routePart in routePartDet)
            {
                //getting existing ALSAT assessment values - part 2
                if (newAnalysedStructures != null && newAnalysedStructures.AnalysedStructuresPart.Count > 0 && assessmentResult != null)
                {
                    foreach (var analysedStructuresPart in newAnalysedStructures.AnalysedStructuresPart)
                    {
                        if (analysedStructuresPart.Id == routeId)
                        {
                            foreach (var structure in analysedStructuresPart.Structure)
                            {
                                foreach (EsdalStructure esdalStructure in assessmentResult.Properties.EsdalStructure)
                                {
                                    if (esdalStructure.Esrn.Equals(structure.ESRN) && esdalStructure.RouteId == routeId && structure.TraversalType!= "overbridge")
                                    {
                                        if (structure.AlsatAppraisal == null)
                                        {
                                            structure.AlsatAppraisal = new AlsatAppraisal();
                                        }

                                        structure.AlsatAppraisal.ESRN = esdalStructure.Esrn;
                                        structure.AlsatAppraisal.StructureKey = esdalStructure.StructureKey.ToString();
                                        structure.AlsatAppraisal.StructureCalculationType = esdalStructure.StructureCalculationType;
                                        structure.AlsatAppraisal.ResultStructure = esdalStructure.ResultStructure;
                                        structure.AlsatAppraisal.Sf = esdalStructure.Sf;
                                        structure.AlsatAppraisal.CommentsForHaulier = esdalStructure.CommentsForHaulier;
                                        structure.AlsatAppraisal.AssessmentComments = EnumExtensions.GetString(esdalStructure.AssessmentComments);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            string structureXmlString = StringExtractor.xmlStructureSerializer(newAnalysedStructures);

            return structureXmlString; //returns structureXmlString after it's generated
        }

        public static List<RoutePartDetails> GetRouteDetailForAnalysis(int analysisId, long versionId, string contentRefNo, int revisionId, string userSchema, int routeId = 0)
        {
            int candidateTmpVar = 0;

            if (revisionId != 0 && bCandidateRouteFlag == true)
            {
                candidateTmpVar = 1;
            }
            else
            {
                candidateTmpVar = 0;
            }

            try
            {
                List<RoutePartDetails> routeList = new List<RoutePartDetails>();
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                     routeList,
                     userSchema + ".STP_ROUTE_ASSESSMENT.SP_R_GET_ROUTE_FOR_ANALYSIS",
                     parameter =>
                     {
                         parameter.AddWithValue("P_ANALYSIS_ID", analysisId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_VERSION_ID", versionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_CONTENT_REF", contentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_REVISION_ID", revisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_CAND_FLAG", candidateTmpVar, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_ROUTE_ID", routeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                     },
                     (records, instance) =>
                     {
                         instance.RouteId = records.GetLongOrDefault("ROUTE_PART_ID");
                         //instance.routeNo = records.GetInt16OrDefault("ROUTE_PART_NO");
                         instance.RouteName = records.GetStringOrDefault("PART_NAME");
                         instance.RouteType = records.GetStringOrDefault("TRANSPORT_MODE");
                     }
               );

                return routeList;

            }
            catch (Exception ex)
            {
                throw ex;
                return null;
            }
            
        }
        public static List<Structure> GetStructureListForAnalysis(int routeId, int routeType, int orgId, string userSchema, AssessmentOutput assessmentResult = null)
        {
            try
            {
                List<RoutePoint> routePointList = null;

                long structId = 0, structIdTemp = 0, sectionId = 0, sectionIdTemp = 0, arrangementId = 0, routeVar = 0, routeVarTmp = 0;
                long orgIdToCheck = 0, contIdToCheck = 0, tmpOrgId = 0, tmpContactId = 0;
                List<long> sectionIdList = new List<long>();
                int strPresentAt = -1, currntSectCnt = 0, margSuit = 0, unSuit = 0;
                bool addTheSection = false;

                AnalysedStructuresPart strucPart = null; // new AnalysedStructuresPart();

                List<Structure> structList = new List<Structure>();

                List<Structure> StructMainList = new List<Structure>();

                List<Structure> structTempList = new List<Structure>(); // Temporary structure list to avoid duplicating list's variable's 

                Structure structure = null; // new Structure();

                StructureResponsibility structResponse = null;
                StructureResponsibleParty structResponsibleParty = null;

                Domain.RouteAssessment.XmlAnalysedStructures.Appraisal apprObj = null;

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    structTempList,
                    userSchema + ".STP_ICA_CALCULATOR.SP_S_STRUCTURE_SECTION_LIST",
                    parameter =>
                    {
                        parameter.AddWithValue("P_ROUTE_ID", routeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ROUTE_TYPE", routeType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            #region
                            sectionId = records.GetLongOrDefault("SECTION_ID");
                            structId = records.GetLongOrDefault("STRUCTURE_ID");

                            structResponsibleParty = new StructureResponsibleParty();

                            if (sectionId != sectionIdTemp)
                            {
                                strPresentAt = sectionIdList.LastIndexOf(sectionId);
                                if (strPresentAt != -1) //which confirms the presence of section in the list
                                {
                                    currntSectCnt = sectionIdList.Count() - strPresentAt; //checks the difference between current count of the section id list and the last location the section appeared.
                                    if (currntSectCnt > 25) // if this difference is more than 25 then the section can be included as a repeated section
                                    {
                                        addTheSection = true; // this section is suitable to be added as the a single link id contained maximum of 25 section's in current database as of 19-06-2015
                                    }
                                    else
                                    {
                                        addTheSection = false; // else it can be excluded
                                    }
                                }
                                else
                                {
                                    addTheSection = true; //if section is not part of the section id list this structure can be added to the xml.
                                }

                                if (addTheSection) //checking whether the section id is already added to the list.
                                {
                                    sectionIdList.Add(sectionId);

                                    structure = new Structure();

                                    structure.Constraints = new Constraints();

                                    structure.Constraints.UnsignedSpatialConstraint = new UnsignedSpatialConstraint();

                                    structList.Add(structure);

                                    structure.ESRN = records.GetStringOrDefault("STRUCTURE_CODE");

                                        //store ALSAT appraisal result
                                        AlsatAppraisal alsatAppraisal = new AlsatAppraisal();
                                    if (assessmentResult != null)
                                    {
                                        foreach (EsdalStructure esdalStructure in assessmentResult.Properties.EsdalStructure)
                                        {
                                            if (esdalStructure.Esrn.Equals(structure.ESRN) && esdalStructure.RouteId == routeId)
                                            {
                                                alsatAppraisal.ESRN = esdalStructure.Esrn;
                                                alsatAppraisal.StructureKey = esdalStructure.StructureKey.ToString();
                                                alsatAppraisal.StructureCalculationType = esdalStructure.StructureCalculationType;
                                                alsatAppraisal.ResultStructure = esdalStructure.ResultStructure;
                                                alsatAppraisal.Sf = esdalStructure.Sf;
                                                alsatAppraisal.CommentsForHaulier = esdalStructure.CommentsForHaulier;
                                                alsatAppraisal.AssessmentComments = EnumExtensions.GetString(esdalStructure.AssessmentComments);
                                            }
                                        }
                                    }

                                    structure.AlsatAppraisal = alsatAppraisal;

                                    structure.StructureName = records.GetStringOrDefault("STRUCTURE_NAME");

                                    structure.StructureSectionId = records.GetLongOrDefault("SECTION_ID");

                                    structure.TraversalType = records.GetStringOrDefault("SECTION_CLASS");

                                    structure.Appraisal = new List<Domain.RouteAssessment.XmlAnalysedStructures.Appraisal>();      //Appraisal from AnalysedStructure's

                                    structResponse = new StructureResponsibility();

                                    structResponse.StructureResponsibleParty = new List<StructureResponsibleParty>();

                                    structIdTemp = structId;

                                    sectionIdTemp = sectionId;
                                        //initializing the temporary details of structure organisations.
                                        tmpOrgId = 0;
                                    tmpContactId = 0;
                                }
                            }

                                //variable to store orgid and contact id of structure organisation
                                orgIdToCheck = (int)records.GetLongOrDefault("ORGANISATION_ID");
                            contIdToCheck = (int)records.GetLongOrDefault("CONTACT_ID");

                            if (structId == structIdTemp && sectionIdTemp == sectionId)
                            {
                                if (contIdToCheck != tmpContactId || orgIdToCheck != tmpOrgId) // condition to exclude same organisation being added as a responsible party.
                                {
                                    tmpContactId = contIdToCheck;
                                    tmpOrgId = orgIdToCheck;

                                    arrangementId = records.GetLongOrDefault("ARRANGEMENT_ID");

                                    structResponsibleParty.ContactId = (int)records.GetLongOrDefault("CONTACT_ID");

                                    structResponsibleParty.OrganisationId = (int)records.GetLongOrDefault("ORGANISATION_ID");

                                    structResponsibleParty.StructureResponsiblePartyOrganisationName = records.GetStringOrDefault("ORGANISATION_NAME");

                                    if (arrangementId != 0)
                                    {
                                        StructureResponsiblePartyOnBehalfOf OnBehalfOfParty = new StructureResponsiblePartyOnBehalfOf();

                                        OnBehalfOfParty.ContactId = (int)records.GetLongOrDefault("CONTACT_ID");

                                        OnBehalfOfParty.OrganisationId = (int)records.GetLongOrDefault("OWNER_ID");

                                        OnBehalfOfParty.DelegationId = (int)records.GetLongOrDefault("ARRANGEMENT_ID");

                                        OnBehalfOfParty.OrganisationName = records.GetStringOrDefault("OWNER_NAME");

                                        OnBehalfOfParty.RetainNotification = records.GetInt16OrDefault("RETAIN_NOTIFICATION") == 1 ? true : false;

                                        OnBehalfOfParty.WantsFailureAlert = records.GetInt16OrDefault("RECEIVE_FAILURES") == 1 ? true : false;

                                        structResponsibleParty.StructureResponsiblePartyOnBehalfOf = OnBehalfOfParty;

                                    }

                                    orgId = structResponsibleParty.OrganisationId;

                                    apprObj = new Domain.RouteAssessment.XmlAnalysedStructures.Appraisal();

                                    apprObj.AppraisalSuitability = new AppraisalSuitability();

                                    apprObj.Organisation = new Domain.RouteAssessment.XmlAnalysedStructures.Organisation();

                                        //Getting the structure section suitability for a given structure
                                        apprObj.AppraisalSuitability.Value = records.GetStringOrDefault("SECTION_SUITABILITY");

                                    if (apprObj.AppraisalSuitability.Value == "Unsuitable")
                                    {
                                        structure.Constraints.SignedSpatialConstraints = new SignedSpatialConstraints();
                                        structure.Constraints.SignedSpatialConstraints.SignedSpatialConstraintsHeight = new SignedSpatialConstraintsHeight();
                                        structure.Constraints.SignedSpatialConstraints.SignedSpatialConstraintsHeight.SignedDistanceValue = new SignedDistanceValue();
                                        structure.Constraints.SignedSpatialConstraints.SignedSpatialConstraintsHeight.SignedDistanceValue.Metres = (decimal)records.GetDoubleOrDefault("SIGN_HEIGHT_METRES");

                                        structure.Constraints.UnsignedSpatialConstraint.Height = (decimal)records.GetDoubleOrDefault("MAX_HEIGHT_METRES");
                                    }
                                        //Getting the organisation id
                                        apprObj.OrganisationId = orgId;

                                        //Getting the structure responsility party
                                        apprObj.Organisation.Value = structResponsibleParty.StructureResponsiblePartyOrganisationName;

                                    structure.Appraisal.Add(apprObj);

                                    structResponse.StructureResponsibleParty.Add(structResponsibleParty);

                                    structure.StructureResponsibility = structResponse;
                                }
                            }

                                #endregion
                            }
                );

                return structList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region GetConstraintList(int routeId, int routeType)
        /// <summary>
        /// function to get constraint list for a routeId from both routeLibrary and application route part library
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="routeType">1 for Application route (routepart, route links) , 0 for planned route (planned route , planned route links)</param>
        /// <returns></returns>
        public static List<RouteConstraints> FetchConstraintList(int routeId, int routeType, string userSchema)
        {
           
                List<RouteConstraints> routeConst = new List<RouteConstraints>();

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    routeConst,
                    userSchema + ".STP_ROUTE_ASSESSMENT.SP_R_GET_CONSTRAINTS",
                    parameter =>
                    {
                        parameter.AddWithValue("P_ROUTE_PART_ID", routeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_LINK_ID", 0, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ROUTE_TYPE", routeType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
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

                            //instance.ConstraintRefrences = GetConstraintGeoDetails(instance.ConstraintId, userSchema);

                            instance.ConstraintGeometry = records.GetGeometryOrNull("GEOMETRY") as sdogeometry;

                            instance.ConstraintValue = new ConstraintValues();

                            instance.ConstraintSuitability = records.GetStringOrDefault("SUITABILITY"); //variable to store constraint suitability

                            instance.ConstraintValue.GrossWeight =records.GetInt32OrDefault("GROSS_WEIGHT");
                            instance.ConstraintValue.AxleWeight =records.GetInt32OrDefault("AXLE_WEIGHT");
                            instance.ConstraintValue.MaxHeight = records.GetSingleOrDefault("MAX_HEIGHT_MTRS");
                            instance.ConstraintValue.MaxLength = records.GetSingleOrDefault("MAX_LEN_MTRS");
                            instance.ConstraintValue.MaxWidth = records.GetSingleOrDefault("MAX_WIDTH_MTRS");

                            instance.CautionList = GetCautionList(instance.ConstraintId, userSchema);
                        }
                );

                return routeConst;
        }
        #endregion
        #region  fetchAffectedStructureListSO(int RouteID, int routeType, string userSchema)
        /// <summary>
        /// function to fetch route appraisal on application map's
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="routeType"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public static List<StructureInfo> FetchAffectedStructureListSO(int routeId, int routeType, string userSchema)
        {

            List<StructureInfo> tmpStructInfoList = new List<StructureInfo>();
            List<StructureInfo> structInfoList = new List<StructureInfo>();

            long tmpOrgId2 = 0;
            long structId = 0, structIdTemp = 0, sectionId = 0;
            string str = null;
            StructureInfo StructInfo = null;


            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                tmpStructInfoList,
                userSchema + ".STP_ICA_CALCULATOR.SP_S_STRUCTURE_INFO_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_ID", routeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ROUTE_TYPE", routeType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
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
                            

                        }
                        

                    }
            );

            return structInfoList;
        }
        #endregion

        #region  fetchAffectedStructureInfoList(int RouteID, int routeType, string userSchema)
        /// <summary>
        /// function to fetch route appraisal on application map's
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="routeType"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public static List<StructureInfo> FetchAffectedStructureInfoList(int routeId, int routeType, string userSchema)
        {

            List<StructureInfo> tmpStructInfoList = new List<StructureInfo>();
            List<StructureInfo> structInfoList = new List<StructureInfo>();

            long tmpOrgId1 = 0, tmpOrgId2 = 0;
            long structId = 0, structIdTemp = 0, sectionId = 0, sectionIdTemp = 0, routeVar = 0, routeVarTmp = 0;
            int suit = 0, cbp = 0, margSuit = 0, unSuit = 0;
            string appraisalTemp = null;
            string suitabilityTemp = null;
            string str = null;
            StructureInfo StructInfo = null;
            

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                tmpStructInfoList,
                userSchema + ".STP_ICA_CALCULATOR.SP_S_STRUCTURE_INFO_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_ID", routeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ROUTE_TYPE", routeType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
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
                            StructInfo.Suitability = new List<string>();
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
                            suitabilityTemp = records.GetStringOrDefault("SECTION_SUITABILITY");
                             
                            StructInfo.Suitability.Add(suitabilityTemp);

                            tmpOrgId1 = tmpOrgId2;

                            structIdTemp = structId;

                        }
                        if (structIdTemp == structId && tmpOrgId1 != tmpOrgId2)
                        {
                            tmpOrgId1 = records.GetLongOrDefault("OWNER_ID");

                            appraisalTemp = records.GetStringOrDefault("SECTION_SUITABILITY");

                            StructInfo.Suitability.Add(appraisalTemp);
                        }

                    }
            );

        return structInfoList;
        }
        #endregion
        #region GetAffectedStructureInfoList(int routeId)
        public static List<StructureInfo> GetAffectedStructureInfoList(int routeID)
        {

            List<StructureInfo> structureInfoList = new List<StructureInfo>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                structureInfoList,
               UserSchema.Portal + ".SP_S_GET_STRUCTURE_INFO",
                parameter =>
                {
                    parameter.AddWithValue("P_Route_ID", routeID, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {

                    instance.StructureName = records.GetStringOrDefault("structure_name");
                    instance.StructureDescr = records.GetStringOrDefault("description");
                    instance.StructureClass = records.GetStringOrDefault("name");
                    instance.StructureCode = records.GetStringOrDefault("structure_code");
                    instance.StructureId = records.GetLongOrDefault("structure_id");
                    instance.SectionId = records.GetLongOrDefault("section_id");
                    instance.SectionNo = records.GetLongOrDefault("section_no");
                    instance.PointGeometry = records.GetGeometryOrNull("point_geometry") as sdogeometry;
                    instance.LineGeometry = records.GetGeometryOrNull("line_geometry") as sdogeometry;
                    instance.Point = instance.PointGeometry.sdo_point;
                }
                );
            return structureInfoList;
        }

        #endregion
        #region FetchAffectedStructureInfoAtPoints(int RouteID, long routeVar, string userSchema, int routeType = 0)
        /// <summary>
        /// function to fetch route point affected structures for showing them on map. By default the route type is set to 0 for application related route's. Do not change this without changing the Stored procedure 
        /// used by this function
        /// </summary>
        /// <param name="RouteID"></param>
        /// <param name="routeVar"></param>
        /// <param name="userSchema"></param>
        /// <param name="routeType">route type is set to 0 by default for application(route-parts) related routes this variable is 1 for call that comes from map. As
        /// the logic for route assessment need to be uniform this variable is hardcoded for route assessment related maps. Where map related functionalities consider library routes as route type 0 and
        /// application related as route type 1</param>
        /// <returns></returns>
        public static List<StructureInfo> FetchAffectedStructureInfoAtPoints(int routeId, long routeVar, string userSchema, int routeType = 0)
        {
            List<StructureInfo> tmpStructInfoList = new List<StructureInfo>();
            List<StructureInfo> structAtFlags = new List<StructureInfo>();

            long tmpOrgId1 = 0, tmpOrgId2 = 0;
            long structId = 0, structIdTemp = 0, sectionId = 0, sectionIdTemp = 0;
            int suit = 0, cbp = 0, margSuit = 0, unSuit = 0;
            string appraisalTemp = null;
            string str = null;
            StructureInfo StructInfo = null;

            #region
            if (routeVar != 0 && routeType == 0)
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    tmpStructInfoList,
                    userSchema + ".STP_ICA_CALCULATOR.SP_STRUCTURE_IN_POINTS",
                        parameter =>
                        {
                            parameter.AddWithValue("P_ROUTE_ID", routeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_ROUTE_VAR", routeVar, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_ROUTE_TYPE", routeType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767); // In this case route type is 0
                            parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                        },
                        (records, instance) =>
                        {
                            structId = records.GetLongOrDefault("STRUCTURE_ID");
                            sectionId = records.GetLongOrDefault("SECTION_ID");
                            tmpOrgId2 = records.GetLongOrDefault("OWNER_ID"); //owner of the structure

                            if (structId != structIdTemp)
                            {
                                StructInfo = new StructureInfo();

                                structAtFlags.Add(StructInfo);

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

                                StructInfo.Suitability.Add(records.GetStringOrDefault("SECTION_SUITABILITY"));

                                tmpOrgId1 = tmpOrgId2;

                                structIdTemp = structId;

                            }
                            if (structId == structIdTemp && tmpOrgId1 != tmpOrgId2)
                            {
                                tmpOrgId1 = records.GetLongOrDefault("OWNER_ID");

                                appraisalTemp = records.GetStringOrDefault("SECTION_SUITABILITY");

                                StructInfo.Suitability.Add(appraisalTemp);
                            }

                        }
                );
            }
            else
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    structAtFlags,
                    userSchema + ".STP_ICA_CALCULATOR.SP_STRUCTURE_IN_POINTS",
                        parameter =>
                        {
                            parameter.AddWithValue("P_ROUTE_ID", routeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_ROUTE_VAR", routeVar, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_ROUTE_TYPE", routeType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767); // In this case the route type 1
                            parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                        },
                        (records, instance) =>
                        {

                            instance.StructureName = records.GetStringOrDefault("structure_name");
                            instance.StructureDescr = records.GetStringOrDefault("description");
                            instance.StructureClass = records.GetStringOrDefault("name");
                            instance.StructureCode = records.GetStringOrDefault("structure_code");
                            instance.StructureId = records.GetLongOrDefault("structure_id");
                            instance.SectionId = records.GetLongOrDefault("section_id");
                            instance.SectionNo = records.GetLongOrDefault("section_no");
                            instance.PointGeometry = records.GetGeometryOrNull("point_geometry") as sdogeometry;
                            instance.LineGeometry = records.GetGeometryOrNull("line_geometry") as sdogeometry;
                            str = instance.PointGeometry.AsText;
                            instance.Point = instance.PointGeometry.sdo_point;

                        }
                );
            }
            #endregion

            return structAtFlags;
        }
        #endregion

        #region static decimal GetConstraintId(string ConstraintCode)

        public static decimal GetConstraintId(string ConstraintCode)
        {
            decimal result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                 UserSchema.Portal + ".SP_GET_CONSTRAINTID",
                 parameter =>
                 {
                     parameter.AddWithValue("P_ConstraintCode", ConstraintCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                 },
                 records =>
                 {
                     result = records.GetLongOrDefault("CNT");
                 }
              );
            return result;
        }
        #endregion

        #region SaveAndFetchContacts
        public static List<AssessmentContacts> SaveAndFetchContacts(int contactId, int notificationId, string userSchema)
        {
            List<AssessmentContacts> affectedContList = new List<AssessmentContacts>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
           affectedContList,
              userSchema + ".STP_ROUTE_ASSESSMENT.SP_SAVE_AND_FETCH_CONTACTS",
              parameter =>
              {
                  parameter.AddWithValue("P_CONTACT_ID", contactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_NOTIF_ID", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767); // default for application route's route parts
                  parameter.AddWithValue("P_FLAG", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767); // variable to delete the temporary table in case there's change in route part or vehicle part 
                  parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
              },
               (records, instance) =>
               {

                   instance.OrganisationName = records.GetStringOrDefault("ORGNAME");

                   instance.ContactName = records.GetStringOrDefault("FULL_NAME");

                   instance.ContactId = (long)records.GetDecimalOrDefault("CONTACT_ID");

                   instance.OrganisationId = records.GetLongOrDefault("ORG_ID");

                   instance.OrganisationType = records.GetStringOrDefault("ORG_TYPE");

                   instance.Email = records.GetStringOrDefault("EMAIL");

                   instance.Fax = records.GetStringOrDefault("FAX");

               });

            return affectedContList;
        }
        #endregion
        public static List<AssessmentContacts> FetchContactDetails(int organisationId, int revisionId, string userSchema)
        {
            List<AssessmentContacts> affectedContList = new List<AssessmentContacts>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
           affectedContList,
              userSchema + ".SP_FETCH_CONTACT_DETAIL",
              parameter =>
              {
                  parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_REV_ID", revisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767); // revision id for sort applications
                  parameter.AddWithValue("P_FLAG", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767); // variable to delete the temporary table in case there's change in route part or vehicle part 
                  parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
              },
               (records, instance) =>
               {

                   instance.OrganisationName = records.GetStringOrDefault("ORGNAME");

                   instance.ContactName = records.GetStringOrDefault("FULL_NAME");

                   instance.OrganisationId = records.GetLongOrDefault("ORG_ID");

                   instance.Email = records.GetStringOrDefault("EMAIL");

                   instance.Fax = records.GetStringOrDefault("FAX");

               });

            return affectedContList;
        }
        /// <summary>
        /// function to import assessment notes
        /// </summary>
        /// <param name="analysisID"></param>
        /// <param name="userSchema"></param>

        public static string GetAssessmentResult(long analysisID, string userSchema)
        {

            RouteAssessmentModel objRouteAssessmentModel = new RouteAssessmentModel();
            objRouteAssessmentModel = GetDriveInstructionsinfo(analysisID, 3, userSchema);

            AnalysedStructures newAnalysedStructures = null;
            if (objRouteAssessmentModel.AffectedStructure != null)
            {
                string affectedStructuresxml = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.AffectedStructure));
                newAnalysedStructures = StringExtractor.XmlAffectedStructuresDeserializer(affectedStructuresxml);
            }

            StringBuilder assessmentNote = new StringBuilder();
            if (newAnalysedStructures != null)
            {
                int i = 1;
                foreach (var item in newAnalysedStructures.AnalysedStructuresPart)
                {

                    foreach (var structure in item.Structure)
                    {
                        if (structure.AlsatAppraisal != null && structure.AlsatAppraisal.CommentsForHaulier != null && structure.AlsatAppraisal.CommentsForHaulier != "")
                        {
                            assessmentNote = assessmentNote.Append("Route part" + " " + i + "-" + " " + item.AnalysedStructuresPartName + ":" + " ");
                            assessmentNote = assessmentNote.Append(structure.ESRN + "-" + structure.StructureName + ":" + " " + structure.AlsatAppraisal.CommentsForHaulier + ";" + " ");
                        }
                    }
                    i++;
                }
            }
            return assessmentNote.ToString();
        }
    }
}