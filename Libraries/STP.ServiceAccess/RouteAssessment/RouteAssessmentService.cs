using Newtonsoft.Json;
using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain;
using STP.Domain.Custom;
using STP.Domain.DrivingInstructionsInterface;
using STP.Domain.RouteAssessment;
using STP.Domain.RouteAssessment.AssessmentOutput;
using STP.Domain.RouteAssessment.XmlAnalysedStructures;
using STP.Domain.SecurityAndUsers;
using STP.Domain.Structures;
using STP.Domain.Structures.StructureJSON;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static STP.Domain.Routes.RouteModel;

namespace STP.ServiceAccess.RouteAssessment
{
    public class RouteAssessmentService : IRouteAssessmentService
    {
        private readonly HttpClient httpClient;
        const string m_RouteName = " -RouteAssessment";
        public RouteAssessmentService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public int InsertLibraryNotes(LibraryNotes libraryNotes)
        {
            int responseData = 0;


            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/InsertLibraryNotes", libraryNotes).Result;
            if (response.IsSuccessStatusCode)
            {
                responseData = response.Content.ReadAsAsync<int>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/ InsertLibraryNotes. StatusCode: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");

            }
            return responseData;
        }

        public List<LibraryNotes> GetLibraryNotes(int OrgId, int LibraryNoteId, int UserId)
        {
            List < LibraryNotes> responseData = new List<LibraryNotes>();
            string urlParameters = $"?OrgId={OrgId}&LibraryNoteId={LibraryNoteId}&UserId={UserId}";
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/GetLibraryNotes{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseData = response.Content.ReadAsAsync<List<LibraryNotes>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/GetLibraryNotes. StatusCode: {(int)response.StatusCode} - {response.ReasonPhrase}");

            }
            return responseData;
        }

        public RouteAssessmentModel GetDriveInstructionsinfo(long analysisID, int analysisType, string userSchema,int? sortOrder=null,int? presetFilter=null)
        {
            RouteAssessmentModel responseData = new RouteAssessmentModel();
            try
            {
                string urlParameters = $"?analysisId={analysisID}&analysisType={analysisType}&userSchema={userSchema}&sortOrder={sortOrder}&presetFilter={presetFilter}";
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
                $"/RouteAssessment/GetDriveInstructionsinfo{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<RouteAssessmentModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/GetDriveInstructionsinfo. StatusCode: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "GetDriveInstructionsinfo:" + ex);
            }
            return responseData;
        }

        public AnalysedStructures GetUnsuitableStructure(AnalysedStructures affectedStructures)
        {
            try
            {
                if (affectedStructures != null)
                {
                    foreach (var item in affectedStructures.AnalysedStructuresPart)
                    {
                        List<Structure> unsuitStr = new List<Structure>();
                        foreach (var structure in item.Structure)
                        {
                            var IsUnsuitableExist = structure.Appraisal != null && structure.Appraisal.Where(x => x.AppraisalSuitability != null
                            && x.AppraisalSuitability.Value != null && (x.AppraisalSuitability.Value.ToLower() == "unsuitable" || x.AppraisalSuitability.Value.ToLower()=="not specified" || x.AppraisalSuitability.Value.ToLower() == "not structure specified")).Any();
                            if (IsUnsuitableExist)
                                unsuitStr.Add(structure);
                        }
                        item.Structure = unsuitStr;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "GetUnsuitableStructure:" + ex);
            }
            return affectedStructures;
        }
        public STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions GetUnsuitableCautions(STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions analysedCautions, bool UnSuitableShowAllCautions)
        {
            try
            {
                if (analysedCautions != null)
                {
                    foreach (var item in analysedCautions.AnalysedCautionsPart)
                    {
                        if (UnSuitableShowAllCautions == false)
                        {
                            List<STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautionStructure> unsuitStr = new List<STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautionStructure>();
                            foreach (var cautionStructure in item.Caution)
                            {
                                var ve = cautionStructure.Vehicle;
                                var isSunsuitable = false;
                                if ((cautionStructure.Vehicle != null && cautionStructure.Vehicle.Where(x => x.StartsWith("Unsuitable")).Any()))
                                {
                                    unsuitStr.Add(cautionStructure);
                                    isSunsuitable = true;
                                }

                                //Generic caution
                                if (!isSunsuitable && (cautionStructure.ConstrainingAttribute == null || cautionStructure.ConstrainingAttribute.Count == 0))
                                {
                                    cautionStructure.Vehicle[0] = "Not Specified";// "Unsuitable (Generic caution)";
                                    unsuitStr.Add(cautionStructure);
                                }
                            }
                            item.Caution = unsuitStr;
                        }
                        else
                        {
                            //For Generic cautions
                            List<STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautionStructure> cautionStrList = new List<STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautionStructure>();
                            foreach (var cautionStructure in item.Caution)
                            {
                                //Generic caution
                                if (cautionStructure.ConstrainingAttribute == null || cautionStructure.ConstrainingAttribute.Count == 0)
                                    cautionStructure.Vehicle[0] = "Not Specified"; //"Unsuitable (Generic caution)";
                                cautionStrList.Add(cautionStructure);
                            }
                            item.Caution = cautionStrList;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "GetUnsuitableCautions:" + ex);
            }
            
            return analysedCautions;
        }
        public Domain.RouteAssessment.XmlConstraints.AnalysedConstraints GetUnsuitableConstraints(Domain.RouteAssessment.XmlConstraints.AnalysedConstraints affectedConstraints)
        {
            try
            {
                if (affectedConstraints != null)
                {
                    foreach (var item in affectedConstraints.AnalysedConstraintsPart)
                    {
                        List<Domain.RouteAssessment.XmlConstraints.Constraint> unsuitConstr = new List<Domain.RouteAssessment.XmlConstraints.Constraint>();
                        foreach (var constraint in item.Constraint)
                        {
                            if (constraint.Appraisal.Suitability != null && (constraint.Appraisal.Suitability.Value.ToLower() == "unsuitable"
                                || constraint.Appraisal.Suitability.Value.ToLower() == "not specified"))
                            {
                                unsuitConstr.Add(constraint);
                            }
                        }
                        item.Constraint = unsuitConstr;
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "GetUnsuitableConstraints:" + ex);
            }
            
            return affectedConstraints;
        }
        public void GetUnsuitableStructuresWithCautions(bool UnSuitableShowAllStructures, bool UnSuitableShowAllCautions, ref AnalysedStructures newAnalysedStructures, ref Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions newCautions)
        {
            try
            {
                newCautions = GetUnsuitableCautions(newCautions, UnSuitableShowAllCautions);
                int AnalysedStructuresPartcount = 0;
                int structurecount = 0;
                foreach (var item in newAnalysedStructures.AnalysedStructuresPart)
                {
                    structurecount = 0;
                    List<Structure> unsuitStr = new List<Structure>();
                    foreach (var structure in item.Structure)
                    {
                        var sectionId = structure.StructureSectionId;

                        if (newCautions != null && newCautions.AnalysedCautionsPart != null && newCautions.AnalysedCautionsPart.Any())
                        {
                            foreach (var itemCaution in newCautions.AnalysedCautionsPart)
                            {
                                if (itemCaution.Caution != null && itemCaution.Caution.Any())
                                {
                                    var analysedCautionStructures = itemCaution.Caution.Where(x => x.CautionedEntity1 != null && x.CautionedEntity1.AnalysedCautionStructureStructure != null
                                    && x.CautionedEntity1.AnalysedCautionStructureStructure.SECTION_ID == sectionId).ToList();
                                    if (analysedCautionStructures != null && analysedCautionStructures.Count>0)
                                    {
                                        newAnalysedStructures.AnalysedStructuresPart[AnalysedStructuresPartcount].Structure[structurecount].AnalysedCautions = analysedCautionStructures;
                                        //structure.AnalysedCautions = analysedCautionStructures;

                                        //if caution is unsuitable, set structure as unsuitable
                                        var isUnsuitableCautionExist = analysedCautionStructures.Where(x => x.Vehicle != null &&
                                                        x.Vehicle.Any(y => y.StartsWith("Unsuitable")  || y.ToLower()=="unsuitable" )).Any();
                                        if (isUnsuitableCautionExist && structure.Appraisal != null && structure.Appraisal.Any())
                                        {
                                            foreach (var itemapp in structure.Appraisal)
                                            {
                                                itemapp.AppraisalSuitability.Value = "Unsuitable";
                                            }
                                        }
                                        var isNotSpecifiedCautionExist = analysedCautionStructures.Where(x => x.Vehicle != null &&
                                                        x.Vehicle.Any(y => y.StartsWith("Not Specified") || y.ToLower() == "not specified")).Any();
                                        if (isNotSpecifiedCautionExist && structure.Appraisal != null && structure.Appraisal.Any())
                                        {
                                            foreach (var itemapp in structure.Appraisal)
                                            {
                                                if (itemapp.AppraisalSuitability.Value != "Suitable" && itemapp.AppraisalSuitability.Value != "Unsuitable")
                                                {
                                                    itemapp.AppraisalSuitability.Value = "Not Structure Specified";
                                                }
                                                if (itemapp.AppraisalSuitability.Value == "Suitable")
                                                {
                                                    itemapp.AppraisalSuitability.Value = "Not Specified";
                                                }
                                            }
                                        }


                                    }
                                }
                            }
                        }
                        structurecount = structurecount + 1;
                    }
                    AnalysedStructuresPartcount = AnalysedStructuresPartcount + 1;
                }

                if (UnSuitableShowAllStructures == false)
                    newAnalysedStructures = GetUnsuitableStructure(newAnalysedStructures);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "GetUnsuitableStructuresWithCautions:" + ex);
            }
            
        }
        public void GetUnsuitableConstraintsWithCautions(bool UnSuitableShowAllConstraint, bool UnSuitableShowAllCautions, ref Domain.RouteAssessment.XmlConstraints.AnalysedConstraints newConstraints, ref Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions newCautions)
        {
            try
            {
                newCautions = GetUnsuitableCautions(newCautions, UnSuitableShowAllCautions);
                int AnalysedConstraintPartcount = 0;
                int Constraintcount = 0;
                foreach (var item in newConstraints.AnalysedConstraintsPart)
                {
                    Constraintcount = 0;
                    List<Structure> unsuitStr = new List<Structure>();
                    foreach (var constraint in item.Constraint)
                    {
                        var ECRN = constraint.ECRN;
                        if (newCautions != null && newCautions.AnalysedCautionsPart != null && newCautions.AnalysedCautionsPart.Any())
                        {
                            foreach (var itemCaution in newCautions.AnalysedCautionsPart)
                            {
                                if (itemCaution.Caution != null && itemCaution.Caution.Any())
                                {
                                    var analysedCautionConstraints = itemCaution.Caution.Where(x => x.CautionedEntity1 != null && x.CautionedEntity1.AnalysedCautionConstraintStructure != null
                                    && x.CautionedEntity1.AnalysedCautionConstraintStructure.ECRN == ECRN).ToList();

                                    if (analysedCautionConstraints != null && analysedCautionConstraints.Count > 0)
                                    {
                                        newConstraints.AnalysedConstraintsPart[AnalysedConstraintPartcount].Constraint[Constraintcount].AnalysedCautions = analysedCautionConstraints;
                                        //constraint.AnalysedCautions = analysedCautionConstraints;
                                        var isConstraintUnsuitable = "Suitable";
                                        if (constraint.Appraisal != null)
                                        {
                                             isConstraintUnsuitable = constraint.Appraisal.Suitability.Value;
                                        }
                                        //if caution is unsuitable, set constraint   as unsuitable
                                        var isUnsuitableCautionExist = analysedCautionConstraints.Where(x => x.Vehicle != null &&
                                                        x.Vehicle.Any(y => y.StartsWith("Unsuitable")  || y.ToLower() == "unsuitable" )).Any();
                                        if (isUnsuitableCautionExist && constraint.Appraisal != null)
                                        {
                                            constraint.Appraisal.Suitability.Value = "Unsuitable";
                                        }
                                        if (isConstraintUnsuitable != "Unsuitable")
                                        {
                                            var isNotSpecifiedCautionExist = analysedCautionConstraints.Where(x => x.Vehicle != null &&
                                                            x.Vehicle.Any(y => y.StartsWith("Not Specified") || y.ToLower() == "not specified")).Any();
                                            if (isNotSpecifiedCautionExist && constraint.Appraisal != null)
                                            {
                                                constraint.Appraisal.Suitability.Value = "Not Specified";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        Constraintcount = Constraintcount + 1;
                    }
                    AnalysedConstraintPartcount = AnalysedConstraintPartcount + 1;
                }

                if (UnSuitableShowAllConstraint == false)
                    newConstraints = GetUnsuitableConstraints(newConstraints);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "GetUnsuitableStructure:" + ex);
            }
            
        }


        public int UpdateRouteAssessment(string contentRefNo, int revisionId, int orgId, int analysisId, int analType, string userSchema)
        {
            int responseData = 0;
            UpdateRouteAssessmentParam updateRouteAssessmentParam = new UpdateRouteAssessmentParam()
            {
                ContentReferenceNo = contentRefNo,
                RevisionId = revisionId,
                OrganisationId = orgId,
                AnalysisId = analysisId,
                AnalysisType = analType,
                UserSchema = userSchema
            };

            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/UpdateRouteAssessment", updateRouteAssessmentParam).Result;
            if (response.IsSuccessStatusCode)
            {
                responseData = response.Content.ReadAsAsync<int>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/ UpdateRouteAssessment. StatusCode: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");

            }
            return responseData;
        }

        public bool ClearRouteAssessment(long revisionId, string userSchema)
        {
            bool result = false;
            try
            {
                string urlParameter = "?revisionId=" + revisionId + "&userSchema=" + userSchema;

                //api call to new service   
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
                $"/RouteAssessment/MovementClearRouteAssessment{urlParameter}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/MovementClearRouteAssessment, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/MovementClearRouteAssessment, Exception: {0}", ex);
            }
            return result;
        }

        public List<RoutePartDetails> GetRouteDetailForAnalysis(int analysisId, long versionId, string contentRefNo, int revisionId, string userSchema)
        {
            string urlParameter = "?analysisId=" + analysisId + "&versionId=" + versionId + "&contentRefNo=" + contentRefNo + "&revisionId=" + revisionId + "&userSchema=" + userSchema;
            List<RoutePartDetails> responseData = new List<RoutePartDetails>();
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/GetRouteDetailForAnalysis{urlParameter}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseData = response.Content.ReadAsAsync<List<RoutePartDetails>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/ GetRouteDetailForAnalysis. StatusCode: {(int)response.StatusCode} -​​​​​​​ {response.ReasonPhrase}");

            }
            return responseData;
        }

        public List<int> GetCountryId(int routeID = 0)
        {
            string urlParameter = "?routeID=" + routeID;
            List<int> result = new List<int>();
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/GetCountryId{urlParameter}").Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<List<int>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/ GetCountryId. StatusCode: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");

            }
            return result;
        }

        #region updateRouteAssessment(inputsForAssessment, anal_type, SessionInfo.userSchema)        
        public int updateRouteAssessment(RouteAssessmentInputs inputs, int analType, string userSchema)
        {
            int responseData = 0;
            RouteAssessmentDIParam routeAssessmentDIParam = new RouteAssessmentDIParam()
            {
                Inputs = inputs,
                AnalType = analType,
                UserSchema = userSchema
            };
            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/RouteAssessmentDI", routeAssessmentDIParam).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    responseData = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/RouteAssessmentDI, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/RouteAssessmentDI, Exception: {0}", ex);
            }
            return responseData;
        }
        #endregion

        #region updateRouteAssessment(versionId, orgId, (int)analysisID, anal_type, SessionInfo.userSchema)        
        public int updateRouteAssessment(int versionId, int orgId, int analysisId, int analType, string userSchema)
        {
            int result = 0;

            UpdateRouteAssessmentVrParam updateRouteAssessmentVrParam = new UpdateRouteAssessmentVrParam()
            {
                VersionId = versionId,
                OrganisationId = orgId,
                AnalysisId = analysisId,
                AnalysisType = analType,
                UserSchema = userSchema
            };
            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/RouteAssessmentVR1", updateRouteAssessmentVrParam).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/RouteAssessmentVR1, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/RouteAssessmentVR1, Exception: {0}", ex);
            }
            return result;
        }
        #endregion

        #region int updateRouteAssessment(string contentRefNo, int revisionId, int orgId, int analysisId, int analType,string userSchema)        
        public int updateRouteAssessment(string contentRefNo, int revisionId, int orgId, int analysisId, int analType, string userSchema)
        {
            int result = 0;

            UpdateRouteAssessmentParam updateRouteAssessmentParam = new UpdateRouteAssessmentParam()
            {
                ContentReferenceNo = contentRefNo,
                RevisionId = revisionId,
                OrganisationId = orgId,
                AnalysisId = analysisId,
                AnalysisType = analType,
                UserSchema = userSchema
            };
            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/UpdateRouteAssessmentAll", updateRouteAssessmentParam).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/UpdateRouteAssessmentAll, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/UpdateRouteAssessmentAll, Exception: {0}", ex);
            }
            return result;
        }
        #endregion

        #region GetMovementStatus(versionId, null, SessionInfo.userSchema)        
        public long GetMovementStatus(int versionId, string contentRefNo, string userSchema)
        {
            string urlParameter = "?versionId=" + versionId + "&contentReferenceNo=" + contentRefNo + "&userSchema=" + userSchema;
            long result = 0;

            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/GetMovementStatus{urlParameter}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GetMovementStatus, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GetMovementStatus, Exception: {0}", ex);
            }
            return result;
        }
        #endregion
        #region updateRouteAssessment(string contentRefNo, int notificationId, int revisionId,int versionId, int orgId, int analysisId, int analType,string userSchema,string VSOType = "")        
        public int updateRouteAssessment(string contentRefNo, int notificationId, int revisionId, int versionId, int orgId, int analysisId, int analType, string userSchema, string VSOType = "")
        {
            string urlParameter = "?contentRefNo=" + contentRefNo + "&notificationId=" + notificationId + "&revisionId=" + revisionId + "&versionId=" + versionId + "&orgId=" + orgId + "&analysisId=" + analysisId + "&analType=" + analType + "&userSchema=" + userSchema + "&VSOType=" + VSOType;
            int result = 0;

            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/UpdateRouteAssessmentRoads{urlParameter}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/UpdateRouteAssessmentRoads, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/UpdateRouteAssessmentRoads, Exception: {0}", ex);
            }
            return result;
        }
        #endregion

        #region updateRouteAssessment(routeAssess, analysisId, SessionInfo.userSchema)        
        public long updateRouteAssessment(RouteAssessmentModel routeAssess, int analysisId, string userSchema)
        {
            long responseData = 0;
            try
            {
                UpdateAssessmentModelParam updateAssessmentModelParam = new UpdateAssessmentModelParam
                {
                    RouteAssessmentModel = routeAssess,
                    AnalysisId = analysisId,
                    UserSchema = userSchema
                };

                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/UpdateRouteAssessmentModel", updateAssessmentModelParam).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    responseData = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/UpdateRouteAssessmentModel, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/UpdateRouteAssessmentModel, Exception: {0}", ex);
            }
            return responseData;
        }
        #endregion

        #region fetchPreviousAffectedList(analysisId, analType, SessionInfo.userSchema)        
        public AnalysedRoute fetchPreviousAffectedList(int analysisId, int analType, string userSchema)
        {
            AnalysedRoute responseData = new AnalysedRoute();

            try
            {
                string urlParameter = "?analysisId=" + analysisId + "&analysisType=" + analType + "&userSchema=" + userSchema;

                //api call to new service   
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/FetchPreviousAffectedList{urlParameter}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    responseData = response.Content.ReadAsAsync<AnalysedRoute>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/FetchPreviousAffectedList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/FetchPreviousAffectedList, Exception: {0}", ex);
            }
            return responseData;
        }
        #endregion
        #region GetStructureAssessmentCount
        public EsdalStructureJson GetStructureAssessmentCount(string ESRN, long routePartId)
        {
            EsdalStructureJson esdalStructureJson = new EsdalStructureJson();
            try
            {

                //api call to new service
                string urlParameters = "?ESRN=" + ESRN + "&routePartId=" + routePartId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/StructureAssessment/GetStructureAssessmentCount{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    esdalStructureJson = response.Content.ReadAsAsync<EsdalStructureJson>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GetStructureAssessmentCount, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GetStructureAssessmentCount, Exception: {0}", ex);
            }
            return esdalStructureJson;
        }
        #endregion
        #region GetAssessmentResult
        public string GetAssessmentResult(int analysisId, string userSchema)
        {
            string assessmentNote ="";
            try
            {

                //api call to new service
                string urlParameters = "?analysisId=" + analysisId + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
                                                 $"/RouteAssessment/GetAssessmentResult{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    assessmentNote = response.Content.ReadAsAsync<string>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GetAssessmentResult, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GetAssessmentResult, Exception: {0}", ex);
            }
            return assessmentNote;
        }
        #endregion
        #region To Perform Structure Assessment
        public int PerformAssessment(List<StructuresToAssess> stuctureList, int notificationId, string movementReferenceNumber, int analysisId, int routeId)
        {
            int success = 0;
            try
            {
                PerformStructureAssessmentParams performStructureAssessmentParams = new PerformStructureAssessmentParams()
                {
                    StructureList = stuctureList,
                    NotificationId = notificationId,
                    MovementReferenceNumber = movementReferenceNumber,
                    AnalysisId = analysisId,
                    RouteId = routeId
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                                        $"{ConfigurationManager.AppSettings["Structures"]}" +
                                        $"/StructureAssessment/PerformAssessment", performStructureAssessmentParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    success = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureAssessment/PerformAssessment, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return success;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureAssessment/PerformAssessment, Exception: {ex}");
                throw;
            }
        }

        #endregion
        #region call to Route Assessment Controller update route assessment 7 params
        public int UpdateRouteAssessment(string contentRefNo, int orgId, int analysisId, int analType, string userSchema, int routeId, AssessmentOutput assessmentResult)
        {
            int status = 0;
            try
            {
                //api call to new service   
                UpdateRouteAssessmentSevenParams updateParams = new UpdateRouteAssessmentSevenParams()
                {
                    ContentRefNo = contentRefNo,
                    OrgId = orgId,
                    AnalysisId = analysisId,
                    AnalType = analType,
                    UserSchema = userSchema,
                    RouteId = routeId,
                    AssessmentResult = assessmentResult
                };

                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
                    $"/RouteAssessment/UpdateAlsatAssessment",
                    updateParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    status = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/UpdateRouteAssessment, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/UpdateRouteAssessment, Exception: {0}", ex);
            }
            return status;
        }

        #endregion

        public List<StructureInfo> GetInstantStructureAnalysis(int routePartId, RoutePart routePart, string userSchema)
        {
            List<StructureInfo> structureInfo = new List<StructureInfo>();
            try
            {
                RouteAssessmentParams assessmentParams = new RouteAssessmentParams()
                {
                    routePartId = routePartId,
                    routePart = routePart,
                    userSchema = userSchema
                };

                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
                    $"/RouteAssessment/GetInstantStructureAnalysis",
                    assessmentParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    structureInfo = response.Content.ReadAsAsync<List<StructureInfo>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GetInstantStructureAnalysis, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GetInstantStructureAnalysis, Exception: {0}", ex);
            }
            return structureInfo;
        }

        public List<RouteConstraints> GetInstantConstraintAnalysis(int routePartId, RoutePart routePart, string userSchema)
        {
            List<RouteConstraints> constraintsInfo = new List<RouteConstraints>();
            try
            {
                RouteAssessmentParams assessmentParams = new RouteAssessmentParams()
                {
                    routePartId = routePartId,
                    routePart = routePart,
                    userSchema = userSchema
                };

                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
                    $"/RouteAssessment/GetInstantConstraintAnalysis",
                    assessmentParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    constraintsInfo = response.Content.ReadAsAsync<List<RouteConstraints>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GetInstantStructureAnalysis, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GetInstantStructureAnalysis, Exception: {0}", ex);
            }
            return constraintsInfo;
        }
        #region GetAffectedConstraintInfoList
        public List<RouteConstraints> FetchConstraintList(int routeId, int routeType, string userSchema)
        {
            string urlParameter = "?routeId=" + routeId + "&routeType=" + routeType + "&userSchema=" + userSchema;
            List<RouteConstraints> constraintsInfo = new List<RouteConstraints>();

            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/FetchConstraintList{urlParameter}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    constraintsInfo = response.Content.ReadAsAsync<List<RouteConstraints>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/FetchConstraintList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/FetchConstraintList, Exception: {0}", ex);
            }
            return constraintsInfo;
        }
        #endregion
        #region GetAffectedStructureInfoList
        public List<StructureInfo> FetchAffectedStructureInfoList(int routeId, int routeType, string userSchema)
        {
            string urlParameter = "?routeId=" + routeId + "&routeType=" + routeType + "&userSchema=" + userSchema;
            List<StructureInfo> structureInfoList = new List<StructureInfo>();

            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/FetchAffectedStructureInfoList{urlParameter}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    structureInfoList = response.Content.ReadAsAsync<List<StructureInfo>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/FetchAffectedStructureInfoList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/FetchAffectedStructureInfoList, Exception: {0}", ex);
            }
            return structureInfoList;
        }
        public List<StructureInfo> FetchAffectedStructureListSO(int routeId, int routeType, string userSchema)
        {
            string urlParameter = "?routeId=" + routeId + "&routeType=" + routeType + "&userSchema=" + userSchema;
            List<StructureInfo> structureInfoList = new List<StructureInfo>();

            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/FetchAffectedStructureListSO{urlParameter}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    structureInfoList = response.Content.ReadAsAsync<List<StructureInfo>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/FetchAffectedStructureListSO, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/FetchAffectedStructureListSO, Exception: {0}", ex);
            }
            return structureInfoList;
        }
        public List<StructureInfo> AffectedStructureInfoList(int routeId)
        {
            string urlParameter = "?routeId=" + routeId;
            List<StructureInfo> affStructureInfoList = new List<StructureInfo>();

            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/AffectedStructureInfoList{urlParameter}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    affStructureInfoList = response.Content.ReadAsAsync<List<StructureInfo>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/AffectedStructureInfoList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/AffectedStructureInfoList, Exception: {0}", ex);
            }
            return affStructureInfoList;
        }
      public  List<StructureInfo> FetchAffectedStructureAtPoints(int routeId, long routeVar, string userSchema, int routeType)
        {
         
                string urlParameter = "?routeId=" + routeId + "&routeVar=" + routeVar + "&userSchema=" + userSchema+"&routeType="+routeType;
                List<StructureInfo> structureInfoAtPointsList = new List<StructureInfo>();

                try
                {
                    //api call to new service   
                    HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
                $"/RouteAssessment/FetchAffectedStructureAtPoints{urlParameter}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                    // Parse the response body.
                    structureInfoAtPointsList = response.Content.ReadAsAsync<List<StructureInfo>>().Result;
                    }
                    else
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/FetchAffectedStructureAtPoints, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/FetchAffectedStructureAtPoints, Exception: {0}", ex);
                }
                return structureInfoAtPointsList;
          
        }
        #endregion
        #region GetAffectedRouteConstraint GetConstraintId
        public decimal GetConstraintId(string ConstraintCode)
        {
            
            decimal ConstraintId = 0;

            try
            {
                string urlParameters = $"?ConstraintCode={ConstraintCode}";
                //api call to new service   
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/GetConstraintId{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    ConstraintId = response.Content.ReadAsAsync<decimal>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GetConstraintId, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GetConstraintId, Exception: {0}", ex);
            }
            return ConstraintId;
        }
        #endregion

		public bool RetainManualAddedAndCompareForAllVersions(int analysisId, int DistributedMovAnalysisId, RouteAssessmentInputs inputsForAssessment, byte[] affectedParties, string userSchema)
        {
            bool result = false;
            try
            {
                AffectedParties objAffectedParties = new AffectedParties()
                {
                    AnalysisId= analysisId,
                    DistributedMovAnalysisId=DistributedMovAnalysisId,
                    Inputs=inputsForAssessment,
                    AffectedPartie=affectedParties,
                    userSchema=userSchema
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
                    $"/RouteAssessment/RetainManualAddedAndCompareForAllVersions",
                    objAffectedParties).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/RetainManualAddedAndCompareForAllVersions, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/RetainManualAddedAndCompareForAllVersions, Exception: {0}", ex);
            }
            return result;
        }
        public bool SortAffectedPartyBasedOnOrganisation(RouteAssessmentModel objRouteAssessmentModel, int analysisId, string userSchema)
        {
            bool result = false;
            try
            {
                AffectedPartyBasedOnOrganisation affectedParty = new AffectedPartyBasedOnOrganisation()
                {
                    AnalysisId = analysisId,
                    RouteAssessmentModel = objRouteAssessmentModel,
                    UserSchema = userSchema
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
                    $"/RouteAssessment/SortAffectedPartyBasedOnOrganisation",
                    affectedParty).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/SortAffectedPartyBasedOnOrganisation, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/SortAffectedPartyBasedOnOrganisation, Exception: {0}", ex);
            }
            return result;
        }
        public string xmlAffectedPartyExcludeDeserializer(string xml, int contactId, int organisationId, string organisationName)
        {
            string result = "";
            try
            {
                string urlParameter = "?xml=" + xml+ "&contactId="+ contactId+ "&organisationId="+ organisationId+ "&organisationName="+ organisationName;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
                $"/RouteAssessment/xmlAffectedPartyExcludeDeserializer{urlParameter}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<string>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/xmlAffectedPartyExcludeDeserializer, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/xmlAffectedPartyExcludeDeserializer, Exception: {0}", ex);
            }
            return result;
        }

        public string xmlAffectedPartyIncludeDeserializer(string xml, int contactId, int organisationId, string organisationName)
        {
            string result = "";
            try
            {
                string urlParameter = "?xml=" + xml + "&contactId=" + contactId + "&organisationId=" + organisationId + "&organisationName=" + organisationName;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
                $"/RouteAssessment/xmlAffectedPartyIncludeDeserializer{urlParameter}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<string>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/xmlAffectedPartyIncludeDeserializer, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/xmlAffectedPartyIncludeDeserializer, Exception: {0}", ex);
            }
            return result;
        }
        public List<AssessmentContacts> SaveAndFetchContacts(int contactId, int notificationId, string userSchema)
        {
            List<AssessmentContacts> responseData = new List<AssessmentContacts>();
            string urlParameters = "?contactId=" + contactId + "&notificationId=" + notificationId + "&userSchema=" + userSchema;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/SaveAndFetchContacts{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseData = response.Content.ReadAsAsync<List<AssessmentContacts>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/SaveAndFetchContacts. StatusCode: {(int)response.StatusCode} - {response.ReasonPhrase}");

            }
            return responseData;
        }

        public List<AssessmentContacts> FetchContactDetails(int organisationId, int revisionId, string userSchema)
        {
            List<AssessmentContacts> responseData = new List<AssessmentContacts>();
            string urlParameters = "?organisationId=" + organisationId + "&revisionId=" + revisionId + "&userSchema=" + userSchema;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/FetchContactDetails{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseData = response.Content.ReadAsAsync<List<AssessmentContacts>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/FetchContactDetails. StatusCode: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return responseData;
        }
        public string XmlAffectedPartyDeserializer(string xml, List<AssessmentContacts> manualContactList)
        {
            string responseData = "";
            StringExtractorParams stringExtractorParams = new StringExtractorParams()
            {
                XmlData = xml,
                ManualContactList = manualContactList
            };

            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/XmlAffectedPartyDeserializer", stringExtractorParams).Result;
            if (response.IsSuccessStatusCode)
            {
                responseData = response.Content.ReadAsAsync<string>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/ XmlAffectedPartyDeserializer. StatusCode: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
            }
            return responseData;
        }

        public string XmlAffectedPartyDeleteFromXml(string xml, string orgName, string fullName)
        {
            string responseData = "";
            DeleteAffectedParams deleteAffectedParams = new DeleteAffectedParams()
            {
                XmlData = xml,
                OrganisationName = orgName,
                FullName= fullName
            };

            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/XmlAffectedPartyDeleteFromXml", deleteAffectedParams).Result;
            if (response.IsSuccessStatusCode)
            {
                responseData = response.Content.ReadAsAsync<string>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/ XmlAffectedPartyDeleteFromXml. StatusCode: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
            }
            return responseData;
        }

        #region Generate Route Assessment
        public byte[] GenerateAffectedStructures(List<RoutePartDetails> routePartDet, AnalysedStructures newAnalysedStructures, int orgId, string userSchema)
        {
            
            byte[] affectedStructure = null;
            GenerateRouteAssessment routeAssessment = new GenerateRouteAssessment
            {
                RoutePart = routePartDet,
                AnalysedStructures = newAnalysedStructures,
                OrganisationId = orgId,
                UserSchema = userSchema
            };
            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/GenerateAffectedStructure", routeAssessment).Result;
            if (response.IsSuccessStatusCode)
            {
                affectedStructure = response.Content.ReadAsAsync<byte[]>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteAssessment/GenerateAffectedStructures. StatusCode:" +(int)response.StatusCode + "ReasonPhrase: " + response.ReasonPhrase);
            }
            return affectedStructure;
        }

        public byte[] GenerateAffectedParties(List<RoutePartDetails> routePartDet, long notificationId, int orgId, string userSchema, int vSoType)
        {
            byte[] affectedParties = null;
            GenerateRouteAssessment routeAssessment = new GenerateRouteAssessment
            {
                RoutePart = routePartDet,
                NotificationId = notificationId,
                OrganisationId = orgId,
                UserSchema = userSchema,
                VSOType= vSoType
            };
            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/GenerateAffectedParties", routeAssessment).Result;
            if (response.IsSuccessStatusCode)
            {
                affectedParties = response.Content.ReadAsAsync<byte[]>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteAssessment/GenerateAffectedParties. StatusCode:" + (int)response.StatusCode + "ReasonPhrase: " + response.ReasonPhrase);
            }
            return affectedParties;
        }
        public byte[] GenerateAffectedCautions(List<RoutePartDetails> routePartDet, int orgId, string userSchema)
        {
            byte[] affectedCautions = null;
            GenerateRouteAssessment routeAssessment = new GenerateRouteAssessment
            {
                RoutePart = routePartDet,
                OrganisationId = orgId,
                UserSchema = userSchema
            };
            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/GenerateAffectedCautions", routeAssessment).Result;
            if (response.IsSuccessStatusCode)
            {
                affectedCautions = response.Content.ReadAsAsync<byte[]>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteAssessment/GenerateAffectedCautions. StatusCode:" + (int)response.StatusCode + "ReasonPhrase: " + response.ReasonPhrase);
            }
            return affectedCautions;
        }

        public byte[] GenerateAffectedConstraints(List<RoutePartDetails> routePartDet, string userSchema)
        {
            byte[] affectedConstraints = null;
            GenerateRouteAssessment routeAssessment = new GenerateRouteAssessment
            {
                RoutePart = routePartDet,
                UserSchema = userSchema
            };
            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/GenerateAffectedConstraints", routeAssessment).Result;
            if (response.IsSuccessStatusCode)
            {
                affectedConstraints = response.Content.ReadAsAsync<byte[]>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteAssessment/GenerateAffectedConstraints. StatusCode:" + (int)response.StatusCode + "ReasonPhrase: " + response.ReasonPhrase);
            }
            return affectedConstraints;
        }
        public byte[] GenerateAffectedAnnotation(List<RoutePartDetails> routePartDet, string userSchema)
        {
            byte[] affectedAnnotations = null;
            GenerateRouteAssessment routeAssessment = new GenerateRouteAssessment
            {
                RoutePart = routePartDet,
                UserSchema = userSchema
            };
            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/GenerateAffectedAnnotation", routeAssessment).Result;
            if (response.IsSuccessStatusCode)
            {
                affectedAnnotations = response.Content.ReadAsAsync<byte[]>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteAssessment/GenerateAffectedAnnotation. StatusCode:" + (int)response.StatusCode + "ReasonPhrase: " + response.ReasonPhrase);
            }
            return affectedAnnotations;
        }
        public byte[] GenerateAffectedRoads(List<RoutePartDetails> routePartDet, string userSchema)
        {
            byte[] affectedRoads = null;
            GenerateRouteAssessment routeAssessment = new GenerateRouteAssessment
            {
                RoutePart = routePartDet,
                UserSchema = userSchema
            };
            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
            $"/RouteAssessment/GenerateAffectedRoads", routeAssessment).Result;
            if (response.IsSuccessStatusCode)
            {
                affectedRoads = response.Content.ReadAsAsync<byte[]>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteAssessment/GenerateAffectedRoads. StatusCode:" + (int)response.StatusCode + "ReasonPhrase: " + response.ReasonPhrase);
            }
            return affectedRoads;
        }
        public long UpdateAnalysedRoute(RouteAssessmentModel routeAssess, long analysisId, string userSchema)
        {
            long result;
            UpdateAnalysedRoutes updateAnalysedRoute = new UpdateAnalysedRoutes
            {
                RouteAssess = routeAssess,
                AnalysisId = analysisId,
                UserSchema = userSchema
            };
            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" + 
                $"/RouteAssessment/UpdateAnalysedRoute", updateAnalysedRoute).Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<long>().Result;
            }
            else
            {
                result = 0;
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance, @"- RouteAssessment/UpdateAnalysedRoute, Error:" + (int)response.StatusCode + "ReasonPhrase:" + response.ReasonPhrase);
            }
            return result;
        }

        public long GenerateDrivingInstnRouteDesc(List<RoutePartDetails> routePartDet, long analysisId, string userSchema)
        {
            long result;
            UInt64 routePartId;
            List<UInt64> routePartIds = new List<UInt64>();
            foreach (var routeId in routePartDet)
            {
                routePartId = (UInt64)routeId.RouteId;
                routePartIds.Add(routePartId);
            }

            DrivInstParams drivInstParams = new DrivInstParams
            {
                DrivingInstructionReq = new DrivingInsReq
                {
                    ListRouteParts = routePartIds,
                    AnalysisID = (ulong)analysisId
                },
                UserSchema = userSchema
            };
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"{m_RouteName }/ GenerateDI. Start Time: {DateTime.Now}, AnalysisId:{analysisId}");
            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
                $"/DrivingInstructor/GenerateDI", drivInstParams).Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<long>().Result;
            }
            else
            {
                result = 0;
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance, @"- RouteAssessment/UpdateAnalysedRoute, Error:" + (int)response.StatusCode + "ReasonPhrase:" + response.ReasonPhrase);
            }
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"{m_RouteName }/ GenerateDI. End Time: {DateTime.Now}, AnalysisId:{analysisId}");
            return result;
        }
        public byte[] GenerateNENAffectedParties(List<ContactModel> contactModel)
        {
            string affectedParty;
            byte[] affectParties;
            Domain.RouteAssessment.XmlAffectedParties.AffectedPartiesStructure affectedParties = new Domain.RouteAssessment.XmlAffectedParties.AffectedPartiesStructure
            {
                GeneratedAffectedParties = new List<Domain.RouteAssessment.XmlAffectedParties.AffectedPartyStructure>()
            };
            Domain.RouteAssessment.XmlAffectedParties.AffectedPartyStructure affectedPartyStruct;
            foreach (var item in contactModel)
            {
                affectedPartyStruct = new Domain.RouteAssessment.XmlAffectedParties.AffectedPartyStructure
                {
                    Exclude = false,
                    ExclusionOutcome = Domain.RouteAssessment.XmlAffectedParties.AffectedPartyReasonExclusionOutcomeType.newlyaffected,
                    ExclusionOutcomeSpecified = true,
                    Reason = Domain.RouteAssessment.XmlAffectedParties.AffectedPartyReasonType.newlyaffected,
                    ReasonSpecified = true,
                    IsPolice = item.ISPolice,
                    IsRetainedNotificationOnly = false, //hard coded 
                    Contact = new Domain.RouteAssessment.XmlAffectedParties.ContactReferenceStructure()
                };
                affectedPartyStruct.Contact.Contact = new Domain.RouteAssessment.XmlAffectedParties.ContactReferenceChoiceStructure();

                Domain.RouteAssessment.XmlAffectedParties.ContactReferenceChoiceStructure contRef = new Domain.RouteAssessment.XmlAffectedParties.ContactReferenceChoiceStructure
                {
                    simpleContactRef = new Domain.RouteAssessment.XmlAffectedParties.SimpleContactReferenceStructure()
                };
                contRef.simpleContactRef.ContactId = item.ContactId;
                contRef.simpleContactRef.OrganisationId = item.OrganisationId;
                contRef.simpleContactRef.FullName = item.FullName;
                contRef.simpleContactRef.OrganisationName = item.Organisation;
                affectedPartyStruct.Contact.Contact = contRef;
                affectedParties.GeneratedAffectedParties.Add(affectedPartyStruct);
            }
            affectedParty = StringExtraction.XmlAffectedPartySerializer(affectedParties);
            affectParties = StringExtraction.ZipAndBlob(affectedParty);
            return affectParties;
        }
        #endregion

        #region UpdatedNenAssessmentDetails
        public int UpdatedNenAssessmentDetails(int notificationId, int inboxItemId, int analType, int organisationId, string userSchema)
        {
            int status = 0;
            try
            {
                //api call to new service   
                NenRouteAssessmentParams updateParams = new NenRouteAssessmentParams()
                {
                    NotificationId = notificationId,
                    InboxItemId = inboxItemId,
                    AnalysisType = analType,
                    OrganisationId = organisationId,
                    UserSchema = userSchema
                };

                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
                    $"/RouteAssessment/UpdatedNenAssessmentDetails",
                    updateParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    status = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/UpdatedNenAssessmentDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/UpdatedNenAssessmentDetails, Exception: {0}", ex);
            }
            return status;
        }
        #endregion

        #region GenerateInstrPDF
        public byte[] GenerateInstrPDF(string outputString)
        {
            byte[] assessmentNote = null;
            try
            {

                //api call to new service
                string urlParameters = "?outputString=" + outputString;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
                                                 $"/RouteAssessment/GenerateInstrPDF{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    assessmentNote = response.Content.ReadAsAsync<byte[]>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GenerateInstrPDF, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GenerateInstrPDF, Exception: {0}", ex);
            }
            return assessmentNote;
        }
        #endregion


        #region GeneratePrintablePDF
        public byte[] GeneratePrintablePDF(string outputString)
        {
            byte[] assessmentNote = null;
            try
            {

                //api call to new service
                string urlParameters = "?outputString=" + outputString;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
                                                 $"/RouteAssessment/GeneratePrintablePDF{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    assessmentNote = response.Content.ReadAsAsync<byte[]>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GeneratePrintablePDF, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GeneratePrintablePDF, Exception: {0}", ex);
            }
            return assessmentNote;
        }
        #endregion

        #region 
        public int GetDispensationCount(int grantee, int grantor)
        {
            int dispcount = 0;
            try
            {
                //api call to new service
                string urlParameters = "?grantee=" + grantee + "&grantor=" + grantor;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["RouteAssessment"]}" +
                                                 $"/RouteAssessment/GetDispensationCount{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    dispcount = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GetDispensationCount, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GetDispensationCount, Exception: {0}", ex);
            }
            return dispcount;
        }
        #endregion

        public RouteAssessmentModel GenerateNenRouteAssessment(List<RoutePartDetails> routePartDet, long analysisId, List<ContactModel> contactModelFiltered, bool generateAffectedParty)
        {
            RouteAssessmentModel routeAssessmentModel = new RouteAssessmentModel();
            Parallel.Invoke(
                    () => routeAssessmentModel.AffectedStructure = GenerateAffectedStructures(routePartDet, null, 0, UserSchema.Portal),
                    () => routeAssessmentModel.AffectedParties = generateAffectedParty ? GenerateNENAffectedParties(contactModelFiltered) : null,
                    () => routeAssessmentModel.AffectedRoads = GenerateAffectedRoads(routePartDet, UserSchema.Portal),
                    () => routeAssessmentModel.Annotation = GenerateAffectedAnnotation(routePartDet, UserSchema.Portal),
                    () => routeAssessmentModel.Cautions = GenerateAffectedCautions(routePartDet, 0, UserSchema.Portal),
                    () => routeAssessmentModel.Constraints = GenerateAffectedConstraints(routePartDet, UserSchema.Portal)
                    );
            UpdateAnalysedRoute(routeAssessmentModel, analysisId, UserSchema.Portal);
            GenerateDrivingInstnRouteDesc(routePartDet, analysisId, UserSchema.Portal);
            return routeAssessmentModel;
        }

    }
}

