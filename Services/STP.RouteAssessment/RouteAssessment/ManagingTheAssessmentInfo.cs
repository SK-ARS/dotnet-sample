using STP.Common.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using STP.Domain.RouteAssessment.XmlAffectedParties;
using STP.Domain.RouteAssessment.XmlAnalysedRoads;
using STP.Domain.RouteAssessment.XmlAnalysedStructures;
using STP.Domain.RouteAssessment;

namespace STP.RouteAssessment.RouteAssessment
{
    public class RoadDistanceInfo
    {
        public string roadName { get; set; }
        public int orgId { get; set; }
        public int distance { get; set; }
    }

    #region ManagingTheAssessmentInfo
    /// <summary>
    /// 
    /// </summary>
    public class ManagingTheAssessmentInfo
    {

        public ManagingTheAssessmentInfo()
        {

        }

        #region GetAssessmentXmlFromByteArray(RouteAssessmentModel inputAssessmentByteInfo)
        /// <summary>
        /// Function to get xml string from assessed byte array
        /// </summary>
        /// <param name="inputAssessmentByteInfo"></param>
        /// <returns></returns>
        public static RouteAnalysisXml GetAssessmentXmlFromByteArray(RouteAssessmentModel inputAssessmentByteInfo)
        {
            RouteAnalysisXml outputRouteAssessedXml = new RouteAnalysisXml();
            try
            {
                //structures
                outputRouteAssessedXml.XmlAnalysedStructure = inputAssessmentByteInfo.AffectedStructure != null ? Encoding.UTF8.GetString(XsltTransformer.Trafo(inputAssessmentByteInfo.AffectedStructure)) : null;
                //affected parties
                outputRouteAssessedXml.XmlAffectedParties = inputAssessmentByteInfo.AffectedParties != null ? Encoding.UTF8.GetString(XsltTransformer.Trafo(inputAssessmentByteInfo.AffectedParties)) : null;
                //affected roads
                outputRouteAssessedXml.XmlAffectedRoads = inputAssessmentByteInfo.AffectedRoads != null ? Encoding.UTF8.GetString(XsltTransformer.Trafo(inputAssessmentByteInfo.AffectedRoads)) : null;
                //annotations
                outputRouteAssessedXml.XmlAnalysedAnnotations = inputAssessmentByteInfo.Annotation != null ? Encoding.UTF8.GetString(XsltTransformer.Trafo(inputAssessmentByteInfo.Annotation)) : null;
                //cautions
                outputRouteAssessedXml.XmlAnalysedCautions = inputAssessmentByteInfo.Cautions != null ? Encoding.UTF8.GetString(XsltTransformer.Trafo(inputAssessmentByteInfo.Cautions)) : null;
                //constraints
                outputRouteAssessedXml.XmlAnalysedConstraints = inputAssessmentByteInfo.Constraints != null ? Encoding.UTF8.GetString(XsltTransformer.Trafo(inputAssessmentByteInfo.Constraints)) : null;

            }
            catch (Exception ex)
            {
                STP.Common.Logger.Logger.GetInstance().LogMessage(Common.Logger.Log_Priority.FATAL_ERROR, "Exception Occured affected party generation: {0}", ex.StackTrace);
                throw;
            }
            return outputRouteAssessedXml;
        }
        #endregion

        #region GetAssessedObjectFromXml(RouteAnalysisXml inputAssessedXmlStringInfo)
        /// <summary>
        /// Function to get assessed object from assessed xml string
        /// </summary>
        /// <param name="inputAssessedXmlStringInfo"></param>
        /// <returns></returns>
        public static RouteAnalysisObject GetAssessedObjectFromXml(RouteAnalysisXml inputAssessedXmlStringInfo)
        {
            RouteAnalysisObject outputRouteAssessedObj = new RouteAnalysisObject();

            try
            {
                //structures
                outputRouteAssessedObj.AnalysedStructureList = inputAssessedXmlStringInfo.XmlAnalysedStructure != null ? StringExtractor.XmlDeserializerStructures(inputAssessedXmlStringInfo.XmlAnalysedStructure) : null;
                //affected parties
                outputRouteAssessedObj.AffectedPartyList = inputAssessedXmlStringInfo.XmlAffectedParties != null ? StringExtractor.xmlAffectedPartyDeserializer(inputAssessedXmlStringInfo.XmlAffectedParties) : null;
                //affected roads
                outputRouteAssessedObj.AnalysedRoadList = inputAssessedXmlStringInfo.XmlAffectedRoads != null ? StringExtractor.AnalysedRoadsDeserializer(inputAssessedXmlStringInfo.XmlAffectedRoads) : null;
                //annotations
                outputRouteAssessedObj.AnnotationList = inputAssessedXmlStringInfo.XmlAnalysedAnnotations != null ? StringExtractor.AnnotationDeserializer(inputAssessedXmlStringInfo.XmlAnalysedAnnotations) : null;
                //cautions
                outputRouteAssessedObj.AnalysedCautionList = inputAssessedXmlStringInfo.XmlAnalysedCautions != null ? StringExtractor.XmlDeserializeCautions(inputAssessedXmlStringInfo.XmlAnalysedCautions) : null;
                //constraints
                outputRouteAssessedObj.AnalysedConstraintList = inputAssessedXmlStringInfo.XmlAnalysedConstraints != null ? StringExtractor.constraintDeserializer(inputAssessedXmlStringInfo.XmlAnalysedConstraints) : null;

            }
            catch (Exception)
            {
                throw;
            }
            return outputRouteAssessedObj;
        }
        #endregion

        #region GetRoadNameOrgIdDict(AnalysedRoadsRoute analysedRoadsRoute)
        /// <summary>
        /// Function to get Dictionary of key (road name) and value(organisation id) of affected roads
        /// </summary>
        /// <param name="analysedRoadsRoute"></param>
        /// <returns></returns>
        public static Dictionary<string, List<int>> GetRoadNameOrgIdDict(AnalysedRoadsRoute analysedRoadsRoute)
        {
            try
            {
                string roadName = null;

                Dictionary<string, List<int>> roadNameOrgDictionary = new Dictionary<string, List<int>>();

                foreach (AnalysedRoadsPart analysedRoadsPart in analysedRoadsRoute.AnalysedRoadsPart)
                {
                    foreach (SubPart analysedSubPart in analysedRoadsPart.SubPart)
                    {
                        foreach (List<PathRoadsPathSegment> analysedRoadsPath in analysedSubPart.Roads)
                        {
                            foreach (PathRoadsPathSegment analysedPath in analysedRoadsPath)
                            {
                                if (analysedPath.Road != null)
                                {
                                    roadName = analysedPath.Road.RoadIdentity.Name != null && analysedPath.Road.RoadIdentity.Name != "" ? analysedPath.Road.RoadIdentity.Name : analysedPath.Road.RoadIdentity.Number;

                                    if (!roadNameOrgDictionary.ContainsKey(roadName))
                                    {
                                        roadNameOrgDictionary.Add(roadName, new List<int>());
                                    }
                                    try
                                    {
                                        foreach (RoadResponsibleParty roadResponsibleParty in analysedPath.Road.RoadResponsibility)
                                        {

                                            roadNameOrgDictionary[roadName].Add(roadResponsibleParty.OrganisationId);

                                            if (roadResponsibleParty.OnBehalfOf != null && roadResponsibleParty.OnBehalfOf.OrganisationId != 0)
                                            {
                                                roadNameOrgDictionary[roadName].Add((int)roadResponsibleParty.OnBehalfOf.OrganisationId);

                                                if (roadResponsibleParty.OnBehalfOf.OnBehalfOf != null && roadResponsibleParty.OnBehalfOf.OnBehalfOf.OrganisationId != 0)
                                                {
                                                    roadNameOrgDictionary[roadName].Add((int)roadResponsibleParty.OnBehalfOf.OnBehalfOf.OrganisationId);
                                                }

                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        STP.Common.Logger.Logger.GetInstance().LogMessage(Common.Logger.Log_Priority.FATAL_ERROR, "Exception Occured affected party generation: {0}", ex.StackTrace);
                                        throw;
                                    }
                                }
                            }
                        }
                    }
                }

                return roadNameOrgDictionary;
            }
            catch (Exception ex)
            {
                STP.Common.Logger.Logger.GetInstance().LogMessage(Common.Logger.Log_Priority.FATAL_ERROR, "Exception Occured affected party generation: {0}", ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region GetStructureNameOrgIdDict(AnalysedStructures analysedStructures)
        /// <summary>
        /// Function to get Dictionary of key (structure code) and value(organisation id) of affected structures
        /// </summary>
        /// <param name="analysedStructures"></param>
        /// <returns></returns>
        public static Dictionary<string, List<int>> GetStructureNameOrgIdDict(AnalysedStructures analysedStructures)
        {
            try
            {
                string structureCode = null;
                Dictionary<string, List<int>> structureNameOrgDictionary = new Dictionary<string, List<int>>();
                foreach (AnalysedStructuresPart analysedStructurePart in analysedStructures.AnalysedStructuresPart)
                {
                    foreach (Structure structure in analysedStructurePart.Structure)
                    {
                        structureCode = structure.ESRN;

                        if (!structureNameOrgDictionary.ContainsKey(structureCode))
                        {
                            structureNameOrgDictionary.Add(structureCode, new List<int>());
                        }

                        try
                        {
                            foreach (StructureResponsibleParty structResponsibleParty in structure.StructureResponsibility.StructureResponsibleParty)
                            {
                                structureNameOrgDictionary[structureCode].Add(structResponsibleParty.OrganisationId);

                                if (structResponsibleParty.StructureResponsiblePartyOnBehalfOf != null && structResponsibleParty.StructureResponsiblePartyOnBehalfOf.OrganisationId != 0)
                                {
                                    structureNameOrgDictionary[structureCode].Add(structResponsibleParty.StructureResponsiblePartyOnBehalfOf.OrganisationId);
                                    if (structResponsibleParty.StructureResponsiblePartyOnBehalfOf.OnBehalfOfOnBehalfOf != null && structResponsibleParty.StructureResponsiblePartyOnBehalfOf.OnBehalfOfOnBehalfOf.OrganisationId != 0)
                                    {
                                        structureNameOrgDictionary[structureCode].Add(structResponsibleParty.StructureResponsiblePartyOnBehalfOf.OnBehalfOfOnBehalfOf.OrganisationId);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            STP.Common.Logger.Logger.GetInstance().LogMessage(Common.Logger.Log_Priority.FATAL_ERROR, "Exception Occured affected party generation: {0}", ex.StackTrace);
                            throw;
                        }
                    }
                }

                return structureNameOrgDictionary;
            }
            catch (Exception ex)
            {
                STP.Common.Logger.Logger.GetInstance().LogMessage(Common.Logger.Log_Priority.FATAL_ERROR, "Exception Occured affected party generation: {0}", ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region GetRoadNameOrgIdDict(Dictionary<string, List<int>> roadNameOrgDictCandRoute, Dictionary<string, List<int>> roadNameOrgDictDistrMovmnt)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roadNameOrgDictCandRoute"></param>
        /// <param name="roadNameOrgDictDistrMovmnt"></param>
        /// <returns></returns>
        public static Dictionary<string, List<int>> GetRoadNameOrgIdDict(Dictionary<string, List<int>> roadNameOrgDictCandRoute, Dictionary<string, List<int>> roadNameOrgDictDistrMovmnt, List<RoadDistanceInfo> roadDistanceCandTable, List<RoadDistanceInfo> roadDistanceDistrTable)
        {
            Dictionary<string, List<int>> orgIdListAffectedStatus = new Dictionary<string, List<int>>();
            List<int> stillAffectedOrgId = new List<int>();
            List<int> affectedByChangeOrgId = new List<int>();
            List<int> noLongerOrgId = new List<int>();

            List<int> tempOrgList = null;

            #region Redmine 4607
            Dictionary<int, List<string>> orgIdDictWithNameListForDistr = SortOrgIdWithANameList(roadNameOrgDictDistrMovmnt);
            Dictionary<int, List<string>> orgIdDictWithNameListForCand = SortOrgIdWithANameList(roadNameOrgDictCandRoute);
            #endregion

            foreach (string roadName in roadNameOrgDictCandRoute.Keys) // road name list of affected candidate roads current list
            {
                tempOrgList = new List<int>();

                if (roadNameOrgDictDistrMovmnt.ContainsKey(roadName)) //if the candidate road name is present in distributed road name list then
                {
                    foreach (int orgId in roadNameOrgDictDistrMovmnt[roadName]) //foreach org id in for the given road name in distributed movement
                    {
                        if (roadNameOrgDictCandRoute[roadName].Exists(x => x == orgId)) // check whether its present in candidate route.
                        {
                            //#Redmine 4607
                            if (orgIdDictWithNameListForCand[orgId].Count() == orgIdDictWithNameListForDistr[orgId].Count()) //if the number of roads count is equal in both candidate and movement version 
                            {
                                if (CheckDifferentNameCount(orgIdDictWithNameListForDistr, orgIdDictWithNameListForCand, orgId)) // even if the count is same but candidate org have a new different road then 
                                {
                                    //checking difference in distances of similar road's within an organisations.
                                    if (!CheckDifferenceInDistanceOfSimilarRoads(roadDistanceCandTable, roadDistanceDistrTable, orgId, roadName))
                                    {
                                        tempOrgList.Add(orgId); //if its present its considered as still affected and maintained in a temporary organisation list.
                                    }
                                }
                            }
                            //#Redmine 4607
                        }
                    }
                    //the remaining organisations which may be new to the already affected road will be considered as affected by change, and these will be added to affected by change org id list.
                    foreach (int orgId in roadNameOrgDictCandRoute[roadName]) //foreach org id in for the given road name in candidate route
                    {
                        if (!tempOrgList.Exists(x => x == orgId))
                        {
                            affectedByChangeOrgId.Add(orgId);
                        }
                    }
                    stillAffectedOrgId.AddRange(tempOrgList); //if its present its considered as still affected.
                }
                else if (!roadNameOrgDictDistrMovmnt.ContainsKey(roadName)) // if the provided road name is not present in distributed movement affected road then
                {
                    foreach (int orgId in roadNameOrgDictCandRoute[roadName]) //foreach org id in for the given road name in candidate road
                    {
                        affectedByChangeOrgId.Add(orgId);
                    }
                }
            }

            foreach (string roadName in roadNameOrgDictDistrMovmnt.Keys)
            {
                if (!roadNameOrgDictCandRoute.ContainsKey(roadName))
                {
                    foreach (int orgId in roadNameOrgDictDistrMovmnt[roadName]) //foreach org id in for the given road name in candidate road
                    {
                        if (!stillAffectedOrgId.Contains(orgId) && !affectedByChangeOrgId.Contains(orgId)) // if the organisations are not present in still affected or affected by change list of organisations then
                        {
                            noLongerOrgId.Add(orgId);
                        }
                    }
                }
            }

            orgIdListAffectedStatus.Add("still affected", stillAffectedOrgId.Distinct().ToList());
            orgIdListAffectedStatus.Add("no longer affected", noLongerOrgId.Distinct().ToList());
            orgIdListAffectedStatus.Add("affected by change", affectedByChangeOrgId.Distinct().ToList());

            return orgIdListAffectedStatus;
        }

        private static bool CheckDifferenceInDistanceOfSimilarRoads(List<RoadDistanceInfo> roadDistanceCandTable, List<RoadDistanceInfo> roadDistanceDistrTable, int orgId, string roadName)
        {
            int distDistr = 0, distCand = 0;
            bool affctdByChange = false;
            var candidateRouteList = from rdInfo in roadDistanceCandTable where rdInfo.orgId == orgId && rdInfo.roadName == roadName select rdInfo;
            var distributedRouteList = from rdInfo in roadDistanceDistrTable where rdInfo.orgId == orgId && rdInfo.roadName == roadName select rdInfo;

            foreach (var item in candidateRouteList)
            {
                distCand = item.distance;

                foreach (var ite in distributedRouteList)
                {
                    distDistr = ite.distance;
                    if (distCand != distDistr)
                    {
                        affctdByChange = true;
                    }
                }
            }

            return affctdByChange;
        }

        #endregion

        #region GetStructNameOrgIdDict(Dictionary<string, List<int>> structureOrgDictCandRoute, Dictionary<string, List<int>> structureOrgDictDistrMovmnt)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="structureOrgDictCandRoute"></param>
        /// <param name="structureOrgDictDistrMovmnt"></param>
        /// <returns></returns>
        public static Dictionary<string, List<int>> GetStructNameOrgIdDict(Dictionary<string, List<int>> structureOrgDictCandRoute, Dictionary<string, List<int>> structureOrgDictDistrMovmnt)
        {
            Dictionary<string, List<int>> orgIdListAffectedStatus = new Dictionary<string, List<int>>();
            List<int> stillAffectedOrgId = new List<int>();
            List<int> affectedByChangeOrgId = new List<int>();
            List<int> noLongerOrgId = new List<int>();

            List<int> tempOrgList = null;

            #region Redmine 4607
            Dictionary<int, List<string>> orgIdDictWithNameListForDistr = SortOrgIdWithANameList(structureOrgDictDistrMovmnt);
            Dictionary<int, List<string>> orgIdDictWithNameListForCand = SortOrgIdWithANameList(structureOrgDictCandRoute);
            #endregion

            foreach (string ESRN in structureOrgDictCandRoute.Keys) // road name list of affected candidate affected structure current list
            {
                tempOrgList = new List<int>();

                if (structureOrgDictDistrMovmnt.ContainsKey(ESRN)) //if the candidate road name is present in distributed affected structure list then
                {
                    foreach (int orgId in structureOrgDictDistrMovmnt[ESRN]) //foreach org id in for the given affected structure
                    {
                        if (structureOrgDictCandRoute[ESRN].Exists(x => x == orgId))
                        {
                            //#Redmine 4607
                            if (orgIdDictWithNameListForCand[orgId].Count() == orgIdDictWithNameListForDistr[orgId].Count()) //if the number of roads count is equal in both candidate and movement version 
                            {
                                if (CheckDifferentNameCount(orgIdDictWithNameListForDistr, orgIdDictWithNameListForCand, orgId)) // even if the count is same but candidate org have a new different road then 
                                {
                                    tempOrgList.Add(orgId); //if its present its considered as still affected and maintained in a temporary organisation list.
                                }
                            }
                            //#Redmine 4607
                        }
                    }

                    foreach (int orgId in structureOrgDictCandRoute[ESRN])
                    {
                        if (!tempOrgList.Exists(x => x == orgId))
                        {
                            affectedByChangeOrgId.Add(orgId);
                        }
                    }
                    stillAffectedOrgId.AddRange(tempOrgList);
                }
                else if (!structureOrgDictDistrMovmnt.ContainsKey(ESRN)) //if the candidate structure name is not present in distributed affected structure list then
                {
                    foreach (int orgId in structureOrgDictCandRoute[ESRN]) //foreach org id in for the given affected structure in candidate road
                    {
                        affectedByChangeOrgId.Add(orgId);
                    }
                }
            }

            foreach (string ESRN in structureOrgDictDistrMovmnt.Keys)
            {
                if (!structureOrgDictCandRoute.ContainsKey(ESRN))
                {
                    foreach (int orgId in structureOrgDictDistrMovmnt[ESRN]) //foreach org id in for the given structure code in candidate road
                    {
                        if (!stillAffectedOrgId.Contains(orgId) && !affectedByChangeOrgId.Contains(orgId))
                        {
                            noLongerOrgId.Add(orgId);
                        }
                    }
                }
            }

            orgIdListAffectedStatus.Add("still affected", stillAffectedOrgId.Distinct().ToList());
            orgIdListAffectedStatus.Add("no longer affected", noLongerOrgId.Distinct().ToList());
            orgIdListAffectedStatus.Add("affected by change", affectedByChangeOrgId.Distinct().ToList());

            return orgIdListAffectedStatus;
        }
        #endregion

        #region Redmine 4607

        private static bool CheckDifferentNameCount(Dictionary<int, List<string>> orgIdDictWithNameListForDistr, Dictionary<int, List<string>> orgIdDictWithNameListForCand, int orgId)
        {
            return orgIdDictWithNameListForCand[orgId].Except(orgIdDictWithNameListForDistr[orgId]).ToList().Count() == 0;
        }

        private static Dictionary<int, List<string>> SortOrgIdWithANameList(Dictionary<string, List<int>> orgIdListAsNamedDict)
        {

            Dictionary<int, List<string>> tmpListForOrgId = new Dictionary<int, List<string>>();
            Dictionary<int, List<string>> OrgIdNamedDict = new Dictionary<int, List<string>>();

            foreach (string name in orgIdListAsNamedDict.Keys)
            {

                foreach (int id in orgIdListAsNamedDict[name])
                {
                    if (!tmpListForOrgId.ContainsKey(id))
                    {
                        tmpListForOrgId.Add(id, new List<string>());
                        tmpListForOrgId[id].Add(name);
                    }
                    else
                    {
                        tmpListForOrgId[id].Add(name);
                    }
                }
            }

            foreach (int id in tmpListForOrgId.Keys)
            {
                if (!OrgIdNamedDict.ContainsKey(id))
                {
                    OrgIdNamedDict.Add(id, new List<string>());
                    OrgIdNamedDict[id] = tmpListForOrgId[id].Distinct().ToList();
                }
            }
            return OrgIdNamedDict;
        }

        #endregion

        #region GetAffectedPartyObjectGrouped(Dictionary<string, List<int>> roadPartyStatusList, Dictionary<string, List<int>> structurePartyStatusList)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roadPartyStatusList"></param>
        /// <param name="structurePartyStatusList"></param>
        /// <returns></returns>
        public static Dictionary<string, List<int>> GetAffectedPartyObjectGrouped(Dictionary<string, List<int>> roadPartyStatusList, Dictionary<string, List<int>> structurePartyStatusList)
        {
            List<int> stillAffectedOrgId = new List<int>();
            List<int> affectedByChangeOrgId = new List<int>();
            List<int> noLongerOrgId = new List<int>();

            Dictionary<string, List<int>> afftdPartyOrdIdStatusList = new Dictionary<string, List<int>>();

            stillAffectedOrgId = roadPartyStatusList["still affected"].Union(structurePartyStatusList["still affected"]).ToList();
            affectedByChangeOrgId = roadPartyStatusList["affected by change"].Union(structurePartyStatusList["affected by change"]).ToList();
            noLongerOrgId = roadPartyStatusList["no longer affected"].Union(structurePartyStatusList["no longer affected"]).ToList();


            afftdPartyOrdIdStatusList.Add("still affected", stillAffectedOrgId);
            afftdPartyOrdIdStatusList.Add("no longer affected", noLongerOrgId);
            afftdPartyOrdIdStatusList.Add("affected by change", affectedByChangeOrgId);

            return afftdPartyOrdIdStatusList;
        }
        #endregion

        #region CompareWithDistrMovementToFindNewlyAffected(AffectedPartiesStructure currCandAffectedParty, AffectedPartiesStructure distributedPartiesStructure)
        /// <summary>
        /// function to compare previously distributed movement version and current candidate route version to find out a newly affected parties.
        /// </summary>
        /// <param name="currCandAffectedParty"></param>
        /// <param name="affectedPartiesStructure"></param>
        public static AffectedPartiesStructure CompareWithDistrMovementToFindNewlyAffected(AffectedPartiesStructure currCandAffectedParty, AffectedPartiesStructure distributedPartiesStructure)
        {
            var generatedData = currCandAffectedParty.GeneratedAffectedParties;
            int orgId = 0;
            foreach (AffectedPartyStructure afftdPartyObj in currCandAffectedParty.GeneratedAffectedParties)
            {
                orgId = afftdPartyObj.Contact.Contact.simpleContactRef.OrganisationId; // getting orgId from currently generated affected party list

                if (!distributedPartiesStructure.GeneratedAffectedParties.Exists(x => x.Contact.Contact.simpleContactRef.OrganisationId == orgId)) //check whether the organisation id is not present in distributed routes affected party
                {
                    (from s in generatedData
                     where (s.Contact.Contact.simpleContactRef.OrganisationId == orgId)
                     select s).ToList().ForEach(s => s.Reason = AffectedPartyReasonType.newlyaffected);

                    (from s in generatedData
                     where (s.Contact.Contact.simpleContactRef.OrganisationId == orgId)
                     select s).ToList().ForEach(s => s.ReasonSpecified = true);

                    (from s in generatedData
                     where (s.Contact.Contact.simpleContactRef.OrganisationId == orgId)
                     select s).ToList().ForEach(s => s.ExclusionOutcome = AffectedPartyReasonExclusionOutcomeType.newlyaffected);

                    (from s in generatedData
                     where (s.Contact.Contact.simpleContactRef.OrganisationId == orgId)
                     select s).ToList().ForEach(s => s.ExclusionOutcomeSpecified = true);
                }
            }
            currCandAffectedParty.GeneratedAffectedParties = generatedData;
            return currCandAffectedParty;
        }
        #endregion

        internal static List<RoadDistanceInfo> GetRoadDistanceList(AnalysedRoadsRoute analysedRoadsRoute)
        {

            try
            {
                string roadName = null;
                int distance = 0;
                List<int> OrgIdList = null;

                List<RoadDistanceInfo> roadDistList = new List<RoadDistanceInfo>();

                RoadDistanceInfo roadDistObj = null;

                foreach (AnalysedRoadsPart analysedRoadsPart in analysedRoadsRoute.AnalysedRoadsPart)
                {
                    foreach (SubPart analysedSubPart in analysedRoadsPart.SubPart)
                    {
                        foreach (List<PathRoadsPathSegment> analysedRoadsPath in analysedSubPart.Roads)
                        {
                            foreach (PathRoadsPathSegment analysedPath in analysedRoadsPath)
                            {
                                if (analysedPath.Road != null)
                                {
                                    roadName = analysedPath.Road.RoadIdentity.Name != null && analysedPath.Road.RoadIdentity.Name != "" ? analysedPath.Road.RoadIdentity.Name : analysedPath.Road.RoadIdentity.Number;

                                    OrgIdList = new List<int>();

                                    foreach (RoadResponsibleParty roadResponsibleParty in analysedPath.Road.RoadResponsibility)
                                    {
                                        OrgIdList.Add(roadResponsibleParty.OrganisationId);

                                        if (roadResponsibleParty.OnBehalfOf != null && roadResponsibleParty.OnBehalfOf.OrganisationId != 0)
                                        {
                                            OrgIdList.Add((int)roadResponsibleParty.OnBehalfOf.OrganisationId);

                                            if (roadResponsibleParty.OnBehalfOf.OnBehalfOf != null && roadResponsibleParty.OnBehalfOf.OnBehalfOf.OrganisationId != 0)
                                            {
                                                OrgIdList.Add((int)roadResponsibleParty.OnBehalfOf.OnBehalfOf.OrganisationId);
                                            }
                                        }
                                    }
                                    distance = (int)analysedPath.Road.Distance.Value;

                                    foreach (int orgId in OrgIdList.Distinct().ToList())
                                    {
                                        var generatedData = roadDistList;

                                        if ((from s in generatedData
                                             where (s.orgId == orgId && s.roadName == roadName)
                                             select s).ToList().Count() > 0)
                                        {
                                            (from s in generatedData
                                             where (s.orgId == orgId && s.roadName == roadName)
                                             select s).ToList().ForEach(s => s.distance = s.distance + distance);

                                            roadDistList = generatedData;
                                        }
                                        else
                                        {
                                            roadDistObj = new RoadDistanceInfo();
                                            roadDistObj.roadName = roadName;
                                            roadDistObj.orgId = orgId;
                                            roadDistObj.distance = distance;
                                            roadDistList.Add(roadDistObj);
                                        }
                                    }

                                }
                            }
                        }
                    }
                }


                return roadDistList;
            }
            catch (Exception ex)
            {
                STP.Common.Logger.Logger.GetInstance().LogMessage(Common.Logger.Log_Priority.FATAL_ERROR, "Exception Occured at GetRoadDistanceList: {0}", ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        ///  #5171 : SORT: Parties which are no longer affected should be removed from the affected parties page for subsequent versions
        /// </summary>
        /// <param name="currCandAffectedParty"></param>
        /// <param name="distributedPartiesStructure"></param>
        /// <returns></returns>
        internal static AffectedPartiesStructure CompareAndRemoveNoLongerAffectedFromSubsequentVersions(AffectedPartiesStructure currCandAffectedParty, AffectedPartiesStructure distributedPartiesStructure)
        {
            int orgId = 0;

            var generatedAffectedParty = currCandAffectedParty;

            try
            {
                STP.Common.Logger.Logger.GetInstance().LogMessage(Common.Logger.Log_Priority.INFORMATIONAL, "Removing no longer affected from subsequent version");

                for (int i = currCandAffectedParty.GeneratedAffectedParties.Count - 1; i >= 0; i--)
                {
                    AffectedPartyStructure afftdPartyObj = currCandAffectedParty.GeneratedAffectedParties[i];

                    orgId = afftdPartyObj.Contact.Contact.simpleContactRef.OrganisationId; // getting orgId from currently generated affected party list

                    if (distributedPartiesStructure.GeneratedAffectedParties.Exists(x => x.Contact.Contact.simpleContactRef.OrganisationId == orgId && x.Reason == AffectedPartyReasonType.nolongeraffected)) //check whether the organisation id is not present in distributed routes affected party
                    {
                        if (generatedAffectedParty.GeneratedAffectedParties.Exists(x => x.Contact.Contact.simpleContactRef.OrganisationId == orgId && x.Reason == AffectedPartyReasonType.nolongeraffected))
                        {
                            currCandAffectedParty.GeneratedAffectedParties.Remove(afftdPartyObj); // removing the affected parties object that is matching and the reason is not "no longer affected"
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                STP.Common.Logger.Logger.GetInstance().LogMessage(Common.Logger.Log_Priority.FATAL_ERROR, "Exception Occured at CompareAndRemoveNoLongerAffectedFromSubsequentVersions: {0}", ex.StackTrace);
                currCandAffectedParty = generatedAffectedParty;
            }
            return currCandAffectedParty;
        }
    }

    #endregion
}