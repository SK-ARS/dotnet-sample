using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STP.Domain;
using STP.Domain.RouteAssessment;
using STP.Domain.RouteAssessment.AssessmentOutput;
using STP.Domain.RouteAssessment.XmlAnalysedStructures;
using STP.Domain.SecurityAndUsers;
using STP.Domain.Structures;
using STP.Domain.Structures.StructureJSON;
using static STP.Domain.Routes.RouteModel;

namespace STP.ServiceAccess.RouteAssessment
{
    public interface IRouteAssessmentService
    {
        int InsertLibraryNotes(LibraryNotes libraryNotes); 
        List<LibraryNotes> GetLibraryNotes(int OrgId, int LibraryNoteId, int UserId);
        RouteAssessmentModel GetDriveInstructionsinfo(long analysisID, int analysisType, string userSchema, int? sortOrder = null, int? presetFilter = null);
        AnalysedStructures GetUnsuitableStructure(AnalysedStructures affectedStructures);
        STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions GetUnsuitableCautions(STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions analysedCautions, bool UnSuitableShowAllCautions);
        Domain.RouteAssessment.XmlConstraints.AnalysedConstraints GetUnsuitableConstraints(Domain.RouteAssessment.XmlConstraints.AnalysedConstraints affectedConstraints);
        void GetUnsuitableStructuresWithCautions(bool UnSuitableShowAllStructures, bool UnSuitableShowAllCautions, ref AnalysedStructures newAnalysedStructures, ref Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions newCautions);
        void GetUnsuitableConstraintsWithCautions(bool UnSuitableShowAllConstraint, bool UnSuitableShowAllCautions, ref Domain.RouteAssessment.XmlConstraints.AnalysedConstraints newConstraints, ref Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions newCautions);
        int UpdateRouteAssessment(string contentRefNo, int revisionId, int orgId, int analysisId, int analType, string userSchema);
        List<RoutePartDetails> GetRouteDetailForAnalysis(int analysisId, long versionId, string contentRefNo, int revisionId, string userSchema);
        List<int> GetCountryId(int routeID = 0);
        int updateRouteAssessment(RouteAssessmentInputs inputs, int analType, string userSchema);
        int updateRouteAssessment(int versionId, int orgId, int analysisId, int analType, string userSchema);
        int updateRouteAssessment(string contentRefNo, int revisionId, int orgId, int analysisId, int analType, string userSchema);
        int updateRouteAssessment(string contentRefNo, int notificationId, int revisionId, int versionId, int orgId, int analysisId, int analType, string userSchema, string VSOType = "");
        bool ClearRouteAssessment(long revisionId, string userSchema);
        long GetMovementStatus(int versionId, string contentRefNo, string userSchema);
        long updateRouteAssessment(RouteAssessmentModel routeAssess, int analysisId, string userSchema);
        AnalysedRoute fetchPreviousAffectedList(int analysisId, int analType, string userSchema);
        EsdalStructureJson GetStructureAssessmentCount(string ESRN, long routePartId);
        string GetAssessmentResult(int analysisId, string userSchema);
        int PerformAssessment(List<StructuresToAssess> stuctureList, int notificationId, string movementReferenceNumber, int analysisId, int routeId);
        int UpdateRouteAssessment(string contentRefNo, int orgId, int analysisId, int analType, string userSchema, int routeId, AssessmentOutput assessmentResult);
        List<StructureInfo> GetInstantStructureAnalysis(int routePartId, RoutePart routePart, string userSchema);
        List<RouteConstraints> GetInstantConstraintAnalysis(int routePartId, RoutePart routePart, string userSchema);
        List<RouteConstraints> FetchConstraintList(int routeId, int routeType, string userSchema);
        List<StructureInfo> FetchAffectedStructureInfoList(int routeId, int routeType, string userSchema);
        List<StructureInfo> AffectedStructureInfoList(int routeId);
        List<StructureInfo> FetchAffectedStructureAtPoints(int routeId, long routeVar, string userSchema, int routeType);
        decimal GetConstraintId(string ConstraintCode);
        bool RetainManualAddedAndCompareForAllVersions(int analysisId, int DistributedMovAnalysisId, RouteAssessmentInputs inputsForAssessment, byte[] affectedParties, string userSchema);
        bool SortAffectedPartyBasedOnOrganisation(RouteAssessmentModel objRouteAssessmentModel, int analysisId, string userSchema);
        string xmlAffectedPartyExcludeDeserializer(string xml, int contactId, int organisationId, string organisationName);
        string xmlAffectedPartyIncludeDeserializer(string xml, int contactId, int organisationId, string organisationName);
        List<AssessmentContacts> SaveAndFetchContacts(int contactId, int notificationId, string userSchema);
        List<AssessmentContacts> FetchContactDetails(int organisationId, int revisionId, string userSchema);
        string XmlAffectedPartyDeleteFromXml(string xml, string orgName, string fullName);
        string XmlAffectedPartyDeserializer(string xml, List<AssessmentContacts> manualContactList);
        byte[] GenerateAffectedStructures(List<RoutePartDetails> routePartDet, AnalysedStructures newAnalysedStructures, int orgId, string userSchema);
        byte[] GenerateAffectedParties(List<RoutePartDetails> routePartDet, long notificationId, int orgId, string userSchema, int vSoType);
        byte[] GenerateAffectedCautions(List<RoutePartDetails> routePartDet, int orgId, string userSchema);
        byte[] GenerateAffectedConstraints(List<RoutePartDetails> routePartDet, string userSchema);
        byte[] GenerateAffectedAnnotation(List<RoutePartDetails> routePartDet, string userSchema);
        byte[] GenerateAffectedRoads(List<RoutePartDetails> routePartDet, string userSchema);
        long UpdateAnalysedRoute(RouteAssessmentModel routeAssess, long analysisId, string userSchema);
        long GenerateDrivingInstnRouteDesc(List<RoutePartDetails> routePartDet, long analysisId, string userSchema);
        int UpdatedNenAssessmentDetails(int notificationId, int inboxItemId, int analType, int organisationId, string userSchema);
        byte[] GenerateNENAffectedParties(List<ContactModel> contactModel);
        List<StructureInfo> FetchAffectedStructureListSO(int routeId, int routeType, string userSchema);
        byte[] GenerateInstrPDF(string outputString);
        byte[] GeneratePrintablePDF(string outputString);
        int GetDispensationCount(int grantee, int grantor);
        RouteAssessmentModel GenerateNenRouteAssessment(List<RoutePartDetails> routePartDet, long analysisId, List<ContactModel> contactModelFiltered, bool generateAffectedParty);
    }
}
