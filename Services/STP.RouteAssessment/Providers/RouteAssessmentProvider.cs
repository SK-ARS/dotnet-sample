using STP.Domain.RouteAssessment;
using STP.RouteAssessment.Interface;
using STP.RouteAssessment.RouteAssessment;
using STP.RouteAssessment.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using static STP.Domain.Routes.RouteModel;
using STP.Domain.Structures;
using STP.Domain.RouteAssessment.AssessmentOutput;
using STP.Domain.RouteAssessment.XmlAnalysedStructures;
using STP.Domain.Routes.TempModelsMigrations;
using STP.Domain.RouteAssessment.XmlAnalysedCautions;
using STP.Domain.ExternalAPI;

namespace STP.RouteAssessment.Providers
{
    public class RouteAssessmentProvider: IRouteAssessment
    {
        #region RouteManagerProvider Singleton

        private RouteAssessmentProvider()
        {
        }
        public static RouteAssessmentProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }

        /// <summary>
        /// Not to be called while using logic
        /// </summary>
        internal static class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly RouteAssessmentProvider instance = new RouteAssessmentProvider();
        }
        #endregion

        public List<LibraryNotes> GetLibraryNotes(int OrgId, int LibraryNoteId, int UserId, string userSchema)
        {
            return RouteAssessmentDAO.GetLibraryNotes(OrgId, LibraryNoteId, UserId, userSchema);

        }
        public int InsertLibraryNotes(LibraryNotes objCheckValidation, string userSchema)
        {
            return RouteAssessmentDAO.InsertLibraryNotes(objCheckValidation, userSchema);

        }

        public RouteAssessmentModel GetDriveInstructionsInfo(long analysisId, int analysisType, string userSchema, int? sortOrder = null, int? presetFilter = null)
        {
            return RouteAssessmentDAO.GetDriveInstructionsInfo(analysisId, analysisType, userSchema);

        }
        
        #region GetDriveInstructionsinfo(long anal_id, int anal_type)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="anal_id"></param>
        /// <param name="anal_type"></param>
        /// <returns></returns>
        public RouteAssessmentModel GetDriveInstructionsinfo(long anal_id, int anal_type, string userSchema)
        {
            return RouteAssessmentDAO.GetDriveInstructionsinfo(anal_id, anal_type, userSchema);

        }
        #endregion
        #region updateRouteAssessment(RouteAssessmentModel routeAssess, int analysisId)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="routeAssess"></param>
        /// <param name="analysisId"></param>
        /// <returns></returns>
        public long updateRouteAssessment(RouteAssessmentModel routeAssess, int analysisId, string userSchema)
        {
            return RouteAssessmentDAO.UpdateAnalysedRoute(routeAssess, analysisId, userSchema);
        }
        #endregion
        public int UpdateRouteAssessment(RouteAssessmentInputs inputs, int analType, string userSchema)
        {
            return RouteAssessmentDAO.UpdateRouteAnalysis("", 0, (int)inputs.RevisionId, 0, (int)inputs.OrganisationId, (int)inputs.AnalysisId, analType, userSchema, inputs.IsCanditateRoute);
        }

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentRefNo"></param>
        /// <param name="revisionId"></param>
        /// <param name="orgId"></param>
        /// <param name="analysisId"></param>
        /// <param name="analType"></param>
        /// <returns></returns>
        public int UpdateRouteAssessment(string contentRefNo, int revisionId, int orgId, int analysisId, int analType, string userSchema)
        {
            return RouteAssessmentDAO.UpdateRouteAnalysis(contentRefNo, 0, revisionId, 0, orgId, analysisId, analType, userSchema);
        }

        #endregion

        #region GetRouteDetailForAnalysis
        public List<RoutePartDetails> GetRouteDetailForAnalysis(int analysisId, long versionId, string contentRefNo, int revisionId, string userSchema)
        {
            return RouteAssessmentDAO.GetRouteDetailForAnalysis(analysisId, versionId, contentRefNo, revisionId, userSchema);
        }
        #endregion

        public List<int> GetCountryId(int routeID = 0)
        {
            return RouteAssessmentDAO.GetCountryId(routeID);
        }

        public int updateRouteAssessment(int versionId, int orgId, int analysisId, int analType, string userSchema)
        {
            return RouteAssessmentDAO.RouteAnalysisUpdate("", 0, 0, versionId, orgId, analysisId, analType, userSchema);
        }
        public int updateRouteAssessment(string contentRefNo, int revisionId, int orgId, int analysisId, int analType, string userSchema)
        {
            return RouteAssessmentDAO.UpdateRouteAnalysis(contentRefNo, 0, revisionId, 0, orgId, analysisId, analType, userSchema);
        }

        public int updateRouteAssessment(string contentRefNo, int notificationId, int revisionId, int versionId, int orgId, int analysisId, int analType, string userSchema, string VSOType = "")
        {
            return RouteAssessmentDAO.UpdateRouteAnalysis(contentRefNo, notificationId, revisionId, versionId, orgId, analysisId, analType, userSchema, false, VSOType);
        }

        public int UpdateAlsatAssessment(int analysisId, int routeId, string userSchema, AssessmentOutput assessmentResult)
        {
            return RouteAssessmentDAO.UpdateAlsatAnalysis(analysisId, routeId, userSchema, assessmentResult);
        }

        public long GetMovementStatus(int versionId, string contentReferenceNo, string userSchema)
        {
            return RouteAnalysisDAO.FetchMovementStatus(versionId, contentReferenceNo, userSchema);
        }

        #region fetchPreviousAffectedParties(int analysisId)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="analysisId"></param>
        /// <param name="analType"></param>
        /// <returns></returns>
        public AnalysedRoute fetchPreviousAffectedParties(int analysisId, int analysisType, string userSchema)
        {
            return RouteAssessmentDAO.FetchPreviousAffectedList(analysisId, analysisType, userSchema);
        }

        public bool MovementClearRouteAssessment(long revisionId, string userSchema)
        {
            return RouteAssessmentDAO.MovementClearRouteAssessment(revisionId, userSchema);
        }
        #endregion
        public int UpdateRouteAssessment(string ContentRefNo, int orgId, int AnalysisId, int analType, string userSchema, int routeId, AssessmentOutput assessmentResult)
        {
            return RouteAssessmentDAO.UpdateRouteAnalysis(ContentRefNo, 0, 0, 0, orgId, AnalysisId, analType, userSchema, false, "", routeId, assessmentResult);
        }

        /// <summary>
        /// fetching affected structure's instantly without saving
        /// </summary>
        /// <param name="routePartId"> if 0 : is passed then library route related structure's will be fetched </param>
        /// <param name="routePart"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public List<StructureInfo> GetInstantStructureAnalysis(int routePartId, RoutePart routePart, string userSchema)
        {
            return RouteAnalysisDAO.GetAffectedStructureList(routePartId, routePart, userSchema);
        }

        /// <summary>
        /// function to fetch constraint instantly without saving
        /// </summary>
        /// <param name="routePartId">if 0 : is passed then library route related constraints will be fetched</param>
        /// <param name="routePart"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public List<RouteConstraints> GetInstantConstraintAnalysis(int routePartId, RoutePart routePart, string userSchema)
        {
            return RouteAnalysisDAO.GetAffectedConstraintList(routePartId, routePart, userSchema);
        }
       public  List<RouteConstraints> FetchConstraintList(int routeId, int routeType, string userSchema)
        {
            return RouteAssessmentDAO.FetchConstraintList(routeId, routeType, userSchema);
        }
        #region FetchAffectedStructureInfoList(int RouteID, int routeType,string userSchema)
        /// <summary>
        /// function to fetch route appraisal
        /// </summary>
        /// <param name="RouteID"></param>
        /// <param name="routeType"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public List<StructureInfo> FetchAffectedStructureInfoList(int routeId, int routeType, string userSchema)
        {
            return RouteAssessmentDAO.FetchAffectedStructureInfoList(routeId, routeType, userSchema);
        }
        #endregion
        #region FetchAffectedStructureListSO(int RouteID, int routeType,string userSchema)
        /// <summary>
        /// function to fetch route appraisal
        /// </summary>
        /// <param name="RouteID"></param>
        /// <param name="routeType"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public List<StructureInfo> FetchAffectedStructureListSO(int routeId, int routeType, string userSchema)
        {
            return RouteAssessmentDAO.FetchAffectedStructureListSO(routeId, routeType, userSchema);
        }
        #endregion
        public List<StructureInfo> AffectedStructureInfoList(int routeId)
        {
            return RouteAssessmentDAO.GetAffectedStructureInfoList(routeId);
        }
        #region FetchAffectedStructureAtPoints(int RouteID, long routeVar, string userSchema, int routeType)
        /// <summary>
        /// function to fetch affected structure at start and end point for planned route the route type for this function is by default set to 1
        /// </summary>
        /// <param name="RouteID"></param>
        /// <param name="routeVar"></param>
        /// <param name="userSchema"></param>
        /// <param name="routeType"></param>
        /// <returns></returns>
        public List<StructureInfo> FetchAffectedStructureAtPoints(int routeId, long routeVar, string userSchema, int routeType)
        {
            if (routeVar == 0)
            {
                return RouteAssessmentDAO.FetchAffectedStructureInfoAtPoints(routeId, 0, userSchema, 1);
            }
            else
            {
                throw new NotImplementedException();
            }

        }
        #endregion


        public decimal GetConstraintId(string ConstraintCode)
        {
            return RouteAssessmentDAO.GetConstraintId(ConstraintCode);
        }
        public List<AssessmentContacts> SaveAndFetchContacts(int contactId, int notificationId, string userSchema)
        {
            return RouteAssessmentDAO.SaveAndFetchContacts(contactId, notificationId, userSchema);
        }
        public List<AssessmentContacts> FetchContactDetails(int organisationId, int revisionId, string userSchema)
        {
            return RouteAssessmentDAO.FetchContactDetails(organisationId, revisionId, userSchema);
        }
        public string XmlAffectedPartyDeserializer(string xml, List<AssessmentContacts> manualContactList)
        {
            return StringExtractor.XmlAffectedPartyDeserializer(xml, manualContactList);
        }
        public string xmlAffectedPartyDeleteFromXml(string xml, string orgName, string fullName)
        {
            return StringExtractor.xmlAffectedPartyDeleteFromXml(xml,orgName,fullName);
        }
        public string GetAssessmentResult(long analysisID, string userSchema)
        {
            return RouteAssessmentDAO.GetAssessmentResult(analysisID, userSchema);
        }

        #region GnerateRouteAssessment
        public string GenerateAffectedStructures(List<RoutePartDetails> routePartDet, AnalysedStructures newAnalysedStructures, int orgId, string userSchema)
        {
            return RouteAssessDao.GenerateAffectedStructures(routePartDet, newAnalysedStructures, orgId, userSchema);
        }
        public string GenerateAffectedParties(List<RoutePartDetails> routePartDet, long notificationId, int orgId, string userSchema, int vSoType)
        {
            return RouteAssessDao.GenerateAffectedParties(routePartDet, notificationId, orgId, userSchema, vSoType);
        }
        public string GenerateAffectedCautions(List<RoutePartDetails> routePartDet, int orgId, string userSchema)
        {
            return RouteAssessDao.GenerateAffectedCautions(routePartDet, orgId, userSchema);
        }
        public string GenerateAffectedConstraints(List<RoutePartDetails> routePartDet, string userSchema)
        {
            return RouteAssessDao.GenerateAffectedConstraints(routePartDet, userSchema);
        }
        public string GenerateAffectedAnnotation(List<RoutePartDetails> routePartDet, string userSchema)
        {
            return RouteAssessDao.GenerateAffectedAnnotation(routePartDet, userSchema);
        }
        public string GenerateAffectedRoads(List<RoutePartDetails> routePartDet, string userSchema)
        {
            return RouteAssessDao.GenerateAffectedRoads(routePartDet, userSchema);
        }
        public long UpdateAnalysedRoute(RouteAssessmentModel routeAssess, long analysisId, string userSchema)
        {
            return RouteAssessDao.UpdateAnalysedRoute(routeAssess, analysisId, userSchema);
        }
        #endregion

        public int UpdatedNenAssessmentDetails(int notificationId, int inboxItemId, int analType, int organisationId, string userSchema)
        {
            return RouteAssessmentDAO.UpdatedNenAssessmentDetails(notificationId, inboxItemId, analType,organisationId,userSchema);
        }

        public byte[] GeneratePrintablePDF(string outputString)
        {
            return StringExtractor.GeneratePrintablePDF(outputString);
        }

        public byte[] GenerateInstrPDF(string outputString)
        {
            return StringExtractor.GenerateInstrPDF(outputString);
        }

        public List<RouteAnalysisModel> GetRouteAnalysisTemp(int PageNo, int PageSize, string UserSchema)
        {
            return RouteAssessDao.GetRouteAnalysisTemp(PageNo, PageSize, UserSchema);
        }

        public List<RoutePartDetails> GetRouteDetailForAnalysis(long versionId, string contentRefNo, long revisionId, int isCandidate, string userSchema)
        {
            return RouteAssessDao.GetRouteDetailForAnalysis(versionId, contentRefNo, revisionId, isCandidate, userSchema);
        }

        public AnalysedCautions GenerateAffectedCautionsTemp(List<RoutePartDetails> routePartDet, int orgId, string userSchema)
        {
            return RouteAssessDao.GenerateAffectedCautionsTemp(routePartDet, orgId, userSchema);
        }
        public long UpdateAnalysedRouteTemp(byte[] assessedData, int analysisId, int analType, string userSchema)
        {
            return RouteAssessmentDAO.UpdateAnalysedRoute(assessedData, analysisId, analType, userSchema);
        }

        public long UpdateProcessedRowInTempTable(int analysisId, string userSchema)
        {
            return RouteAssessDao.UpdateProcessedRowInTempTable(analysisId, userSchema);
        }
        public List<long> GetDispensationCount(int grantee, int grantor)
        {
            return RouteAssessDao.GetDispensationStatusType(grantee, grantor);
        }

        public DIList GetRouteDetailsForMovement(string EsdalReferenceNumber, int movementTypeFlag, long organisationId, string userSchema)
        {
            return RouteAssessDao.GetRouteDetailsForMovement(EsdalReferenceNumber, movementTypeFlag, organisationId, userSchema);
        }
    }
}