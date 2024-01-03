using STP.Domain.ExternalAPI;
using STP.Domain.RouteAssessment;
using STP.Domain.RouteAssessment.XmlAnalysedStructures;
using STP.Domain.Structures;
using STP.RouteAssessment.RouteAssessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static STP.Domain.Routes.RouteModel;

namespace STP.RouteAssessment.Interface
{
   interface IRouteAssessment
    {
        List<LibraryNotes> GetLibraryNotes(int OrgId, int LibraryNoteId, int UserId, string userSchema);
        int InsertLibraryNotes(LibraryNotes objCheckValidation, string userSchema);
        RouteAssessmentModel GetDriveInstructionsInfo(long analysisId, int analysisType, string userSchema,int? sortOrder= null, int? presetFilter = null);
        int UpdateRouteAssessment(string contentRefNo, int revisionId, int orgId, int analysisId, int analType, string userSchema);
        List<RoutePartDetails> GetRouteDetailForAnalysis(int analysisId, long versionId, string contentRefNo, int revisionId, string userSchema);
        List<int> GetCountryId(int routeID = 0);
        long updateRouteAssessment(RouteAssessmentModel routeAssess, int analysisId, string userSchema);
        int UpdateRouteAssessment(RouteAssessmentInputs inputs, int analType, string userSchema);
        int updateRouteAssessment(int versionId, int orgId, int analysisId, int analType, string userSchema);
        int updateRouteAssessment(string contentRefNo, int revisionId, int orgId, int analysisId, int analType, string userSchema);
        int updateRouteAssessment(string contentRefNo, int notificationId, int revisionId, int versionId, int orgId, int analysisId, int analType, string userSchema, string VSOType = "");
        long GetMovementStatus(int versionId, string contentReferenceNo, string userSchema);
        AnalysedRoute fetchPreviousAffectedParties(int analysisId, int analType, string userSchema);
       
        bool MovementClearRouteAssessment(long revisionId, string userSchema);
        List<RouteConstraints> FetchConstraintList(int routeId, int routeType, string userSchema);
        List<StructureInfo> AffectedStructureInfoList(int routeId);
        List<AssessmentContacts> SaveAndFetchContacts(int contactId, int notificationId, string userSchema);
        List<AssessmentContacts> FetchContactDetails(int organisationId, int revisionId, string userSchema);
        string XmlAffectedPartyDeserializer(string xml, List<AssessmentContacts> manualContactList);
        string GetAssessmentResult(long analysisID, string userSchema);
        string GenerateAffectedStructures(List<RoutePartDetails> routePartDet, AnalysedStructures newAnalysedStructures, int orgId, string userSchema);
        string GenerateAffectedParties(List<RoutePartDetails> routePartDet, long notificationId, int orgId, string userSchema, int vSoType);
        string GenerateAffectedCautions(List<RoutePartDetails> routePartDet, int orgId, string userSchema);
        string GenerateAffectedConstraints(List<RoutePartDetails> routePartDet, string userSchema);
        string GenerateAffectedAnnotation(List<RoutePartDetails> routePartDet, string userSchema);
        string GenerateAffectedRoads(List<RoutePartDetails> routePartDet, string userSchema);
        long UpdateAnalysedRoute(RouteAssessmentModel routeAssess, long analysisId, string userSchema);
        int UpdatedNenAssessmentDetails(int notificationId, int inboxItemId, int analType, int organisationId, string userSchema);
        byte[] GeneratePrintablePDF(string outputString);
        byte[] GenerateInstrPDF(string outputString);
        List<long> GetDispensationCount(int grantee, int grantor);
        DIList GetRouteDetailsForMovement(string EsdalReferenceNumber, int movementTypeFlag, long organisationId, string userSchema);

    }
}
