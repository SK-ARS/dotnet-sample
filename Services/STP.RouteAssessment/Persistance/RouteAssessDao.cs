using Oracle.DataAccess.Client;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.Domain.Custom;
using STP.Domain.RouteAssessment.AssessmentOutput;
using STP.Domain.RouteAssessment.XmlAnalysedStructures;
using STP.Domain.RouteAssessment.XmlAffectedParties;
using System;
using System.Collections.Generic;
using System.Linq;
using static STP.Domain.Routes.RouteModel;
using STP.Domain.RouteAssessment;
using STP.Common.Constants;
using STP.Domain.RouteAssessment.XmlAnalysedCautions;
using STP.Domain.RouteAssessment.XmlConstraints;
using STP.Domain.RouteAssessment.XmlAnalysedAnnotations;
using STP.Domain.RouteAssessment.XmlAnalysedRoads;
using STP.Domain.Routes.TempModelsMigrations;
using STP.Common.General;
using System.Text;
using static STP.Common.Enums.ExternalApiEnums;

namespace STP.RouteAssessment.Persistance
{
    public static class RouteAssessDao
    {
        #region Generate Route Assessment

        #region Affected Structures

        #region GenerateAffectedStructures
        public static string GenerateAffectedStructures(List<RoutePartDetails> routePartDet, AnalysedStructures newAnalysedStructures, int orgId, string userSchema)
        {
            AnalysedStructures structures = new AnalysedStructures();

            //More condition's can be added to fetch route details 
            structures.AnalysedStructuresPart = new List<AnalysedStructuresPart>();
            AssessmentOutput assessmentResult = null;
            foreach (RoutePartDetails routePart in routePartDet)
            {
                //getting existing ALSAT assessment values - part 2
                if (newAnalysedStructures != null && newAnalysedStructures.AnalysedStructuresPart.Count > 0)
                {
                    if (assessmentResult == null)
                    {
                        assessmentResult = new AssessmentOutput
                        {
                            Properties = new Properties()
                        };
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
                                            StructureKey = structure.AlsatAppraisal.StructureKey,
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
                AnalysedStructuresPart analstruct = new AnalysedStructuresPart
                {
                    ComparisonId = 1123, //Sample comparison id need to be updated with new comparison id

                    Id = (int)routePart.RouteId, //Route id for route routeId

                    AnalysedStructuresPartName = routePart.RouteName, //Route Name routeName

                    Structure = new List<Structure>()
                };
                //getting structure list for given set of structure's
                analstruct.Structure = GetStructureListForAnalysis(analstruct.Id, 0, orgId, userSchema, assessmentResult);

                structures.AnalysedStructuresPart.Add(analstruct);
            }

            string structureXmlString = StringExtraction.XmlStructureSerializer(structures);

            return structureXmlString; //returns structureXmlString after it's generated
        }
        #endregion

        #region GetStructureListForAnalysis
        public static List<Structure> GetStructureListForAnalysis(int routeId, int routeType, int orgId, string userSchema, AssessmentOutput assessmentResult = null)
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
                                addTheSection = true; // this section is suitable to be added as the a single link id contained maximum of 25 section's in current database as of 19-06-2015
                            else
                                addTheSection = false; // else it can be excluded
                        }
                        else
                            addTheSection = true; //if section is not part of the section id list this structure can be added to the xml.

                        if (addTheSection) //checking whether the section id is already added to the list.
                        {
                            sectionIdList.Add(sectionId);

                            structure = new Structure
                            {
                                Constraints = new Constraints
                                {
                                    UnsignedSpatialConstraint = new UnsignedSpatialConstraint()
                                }
                            };

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
                });
            return structList;
        }
        #endregion

        #endregion

        #region Affected Parties

        #region GenerateAffectedParties
        public static string GenerateAffectedParties(List<RoutePartDetails> routePartDet, long notificationId, int orgId, string userSchema, int vSoType)
        {
            int count = 0;

            AffectedPartiesStructure affectedParties = new AffectedPartiesStructure
            {
                GeneratedAffectedParties = new List<AffectedPartyStructure>()
            };

            AffectedPartyStructure affectedPartyStruct = null;

            foreach (RoutePartDetails routePart in routePartDet)
            {

                List<AssessmentContacts> affectedCont = GetAffectedContact(routePart.RouteId, 1, notificationId, orgId, userSchema);

                foreach (AssessmentContacts newContact in affectedCont)
                {

                    affectedPartyStruct = new AffectedPartyStructure
                    {
                        Exclude = false,
                        ExclusionOutcome = newContact.AffectedExclusionOutcome,
                        ExclusionOutcomeSpecified = true,
                        Reason = newContact.AffectedReasonType,
                        ReasonSpecified = true,
                        IsPolice = newContact.OrganisationType == "police",
                        IsRetainedNotificationOnly = false, //hard coded 
                        Contact = new Domain.RouteAssessment.XmlAffectedParties.ContactReferenceStructure(),
                        DispensationStatus = newContact.DispensationStatus
                    };

                    affectedPartyStruct.Contact.Contact = new Domain.RouteAssessment.XmlAffectedParties.ContactReferenceChoiceStructure();

                    Domain.RouteAssessment.XmlAffectedParties.ContactReferenceChoiceStructure contRef = new Domain.RouteAssessment.XmlAffectedParties.ContactReferenceChoiceStructure
                    {
                        simpleContactRef = new Domain.RouteAssessment.XmlAffectedParties.SimpleContactReferenceStructure()
                    };

                    contRef.simpleContactRef.ContactId = (int)newContact.ContactId;
                    contRef.simpleContactRef.OrganisationId = (int)newContact.OrganisationId;
                    contRef.simpleContactRef.FullName = newContact.ContactName;
                    contRef.simpleContactRef.OrganisationName = newContact.OrganisationName;

                    affectedPartyStruct.Contact.Contact = contRef;

                    if (newContact.OwnerArrangementId != 0)
                    {
                        affectedPartyStruct.OnBehalfOf = new OnBehalfOfStructure
                        {
                            DelegationId = (int)newContact.OwnerArrangementId,

                            DelegationIdSpecified = true,

                            DelegatorsContactId = (int)newContact.ContactId,

                            DelegatorsContactIdSpecified = true,

                            DelegatorsOrganisationId = (int)newContact.OwnerOrgId,

                            DelegatorsOrganisationIdSpecified = true,

                            DelegatorsOrganisationName = newContact.OwnerOrgName,

                            RetainNotification = newContact.RetainNotification,

                            WantsFailureAlert = newContact.RecieveFailures
                        };
                    }

                    try
                    {
                        //checking whether the generated affected party is already present in the list or not
                        count = StringExtraction.CheckForDuplicateGenAffectdParty(affectedParties.GeneratedAffectedParties, affectedPartyStruct);

                        if (count == 0)
                            affectedParties.GeneratedAffectedParties.Add(affectedPartyStruct);
                    }
                    catch (Exception ex)
                    {
                        string exceptionMessage = $"Exception occurred, Exception: " + ex;
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, exceptionMessage);
                    }
                }
            }

            //This code portion is used to remove affected parties which are police / soa in case the owner opts for VSO type of notification and opts to notify police or soa or both 
            if (vSoType == (int)VSOType.police)
                affectedParties.GeneratedAffectedParties.RemoveAll(x => !x.IsPolice);
            else if (vSoType == (int)VSOType.soa)
                affectedParties.GeneratedAffectedParties.RemoveAll(x => x.IsPolice);

            //sorting the generated affected party list based on organisation name
            if (affectedParties.GeneratedAffectedParties.Count != 0 || affectedParties.GeneratedAffectedParties != null)
            {
                var affectedPartylist = affectedParties.GeneratedAffectedParties;

                affectedParties.GeneratedAffectedParties = affectedPartylist.OrderBy(t => t.Contact.Contact.simpleContactRef.OrganisationName).ToList();
            }

            string result = StringExtraction.XmlAffectedPartySerializer(affectedParties);

            return result;
        }
        #endregion

        #region GetAffectedContact
        public static List<AssessmentContacts> GetAffectedContact(long? routeId, int routeType, long notifId, int orgId, string userSchema = UserSchema.Portal, int revId = 0)
        {
            int isCandidateRt = 0;

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

        #region GetDispensationStatusType
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

        #endregion

        #region Affected Cautions

        #region GenerateAffectedCautions
        public static string GenerateAffectedCautions(List<RoutePartDetails> routePartDet, int orgId, string userSchema)
        {
            string cautionXmlString = null;
            try
            {
                AnalysedCautions newCautionObj = new AnalysedCautions
                {
                    AnalysedCautionsPart = new List<AnalysedCautionsPart>()
                };
                bool cautExist = false;

                AnalysedCautionsPart newAnalCautPart = null;

                #region Analysed caution Part adding

                if (routePartDet.Count == 0)
                    return null;

                foreach (RoutePartDetails routePart in routePartDet)
                {
                    newAnalCautPart = new AnalysedCautionsPart
                    {
                        ComparisonId = 1123, //dummy comparison id

                        Id = (int)routePart.RouteId
                    };

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
                    //if (routeCautionList.Count != 0)
                    //{
                        foreach (RouteCautions routeCaution in routeCautionList)
                        {
                            if (routeCaution.CautionType == 0)
                                newAnalCautStruct = GetStructureCaution(routeCaution);
                            else
                                newAnalCautStruct = GetRoadCaution(routeCaution, userSchema);
                            newAnalCautPart.Caution.Add(newAnalCautStruct);
                        }
                    //}
                    #endregion

                    //if (newAnalCautPart.Caution.Count == 0)
                    //    newCautionObj.AnalysedCautionsPart = null;
                    //else
                    //{
                    //    if (newCautionObj.AnalysedCautionsPart == null)
                            
                        newCautionObj.AnalysedCautionsPart.Add(newAnalCautPart);
                    //}
                    #endregion
                }
                #endregion

                if (newCautionObj.AnalysedCautionsPart != null)
                {
                    foreach (AnalysedCautionsPart cautList in newCautionObj.AnalysedCautionsPart)
                    {
                        if (cautList.Caution.Count > 0)
                            cautExist = true; //condition to check whether any route part has cautions or not.
                    }

                   // if (cautExist && newCautionObj.AnalysedCautionsPart.Count != 0) //if caution exist for any of the route part only then an xml is generated.
                        cautionXmlString = StringExtraction.XmlCautionSerializer(newCautionObj);
                }
            }
            catch (Exception e)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Caution cannot be generated, Exception: " + e);
            }
            return cautionXmlString;
        }
        #endregion

        #region GetCautionList
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
                    roadConstId = records.GetLongOrDefault("CONSTRAINT_ID"); //fetching constraint id 
                    structConstId = records.GetLongOrDefault("STRUCTURE_ID"); // fetching structure id

                    if (structConstId != 0 && roadConstId == 0) //filling structure caution details
                    {
                        instance.CautionStructureId = structConstId;

                        instance.CautionType = 0;

                        instance.CautionConstraintName = records.GetStringOrDefault("STRUCTURE_NAME");

                        instance.CautionConstraintCode = records.GetStringOrDefault("STRUCTURE_CODE");

                        instance.CautionConstraintType = records.GetStringOrDefault("STRUCTURE_TYPE");

                        instance.CautionSectionId = records.GetLongOrDefault("SECTION_ID");
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

                    instance.cautDescription = StringExtraction.XmlStringExtractor(records.GetStringOrDefault("SPECIFIC_ACTION"), "SpecificAction");

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
                });
            return routeCaution;
        }
        #endregion

        #region GetCautionContact
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
                        instance.ContactId = records.GetDataTypeName("CONTACT_ID") == "Decimal" ? (int)records.GetDecimalOrDefault("CONTACT_ID") : (int)records.GetLongOrDefault("CONTACT_ID"); 

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

        #region GetStructureCaution
        private static AnalysedCautionStructure GetStructureCaution(RouteCautions routeCaution)
        {
            //breaking if no caution's exist
            AnalysedCautionStructure newAnalCautStruct = new AnalysedCautionStructure
            {
                CautionedEntity1 = new AnalysedCautionChoiceStructure
                {
                    AnalysedCautionStructureStructure = new AnalysedCautionStructureStructure
                    {
                        Annotation = new List<ResolvedAnnotationStructure>(),

                        ESRN = routeCaution.CautionConstraintCode,

                        Name = routeCaution.CautionConstraintName,

                        SECTION_ID = routeCaution.CautionSectionId

                    }
                },

                Name = routeCaution.CautionName //name of caution being generated
            };

            #region routeConstrType Switch statement
            try
            {
                newAnalCautStruct.ConstrainingAttribute = new List<CautionConditionType>();

                if (routeCaution.CautionConstraintValue.GrossWeight != 0)
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.grossweight);

                if (routeCaution.CautionConstraintValue.AxleWeight != 0)
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.axleweight);

                if (routeCaution.CautionConstraintValue.MaxHeight != 0)
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.height);

                if (routeCaution.CautionConstraintValue.MaxLength != 0)
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.overalllength);

                if (routeCaution.CautionConstraintValue.MaxWidth != 0)
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.width);

                if (routeCaution.CautionConstraintValue.MinSpeed != 0)
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.speed);

                if (routeCaution.CautionConstraintType == "underbridge")
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionStructureStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.StructureType.underbridge;

                else if (routeCaution.CautionConstraintType == "overbridge")
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionStructureStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.StructureType.overbridge;

                else if (routeCaution.CautionConstraintType == "level crossing")
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionStructureStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.StructureType.levelcrossing;

                if (newAnalCautStruct.ConstrainingAttribute.Count == 0)
                    newAnalCautStruct.ConstrainingAttribute = null;
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Exception occurred, Exception: " + ex;
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, exceptionMessage);
            }


            #region Road details
            RoadIdentificationStructure roadIdentification = new RoadIdentificationStructure
            {
                Name = routeCaution.RoadName
            };

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
            newAnalCautStruct.Conditions = new CautionConditionStructure
            {
                MaxAxleWeight = new WeightStructure
                {
                    Value = routeCaution.CautionConstraintValue.AxleWeight //tonnes to kilograms
                },

                MaxGrossWeight = new WeightStructure
                {
                    Value = routeCaution.CautionConstraintValue.GrossWeight
                },

                MaxHeight = new DistanceStructure
                {
                    Value = (decimal)routeCaution.CautionConstraintValue.MaxHeight
                },

                MaxOverallLength = new DistanceStructure
                {
                    Value = (decimal)routeCaution.CautionConstraintValue.MaxLength
                },

                MaxWidth = new DistanceStructure
                {
                    Value = (decimal)routeCaution.CautionConstraintValue.MaxWidth
                },

                MinSpeed = new SpeedStructure
                {
                    Value = (decimal)routeCaution.CautionConstraintValue.MinSpeed
                }
            };

            #endregion

            //New Object creation to save the specific action tag value
            newAnalCautStruct.Action = new CautionActionStructure
            {
                SpecificAction = routeCaution.cautDescription  // specific action performed for cautions
            };

            newAnalCautStruct.Contact = new List<ResolvedContactStructure>();

            #region Caution contact part being added
            //Entering the contact details.
            foreach (AssessmentContacts cautContactObj in routeCaution.CautionContactList)
            {
                ResolvedContactStructure newContactObj = new ResolvedContactStructure();

                newContactObj.Address = new Domain.RouteAssessment.XmlAnalysedCautions.AddressStructure
                {
                    Line = new List<string>()
                };

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
        #endregion

        #region GetRoadCaution
        private static AnalysedCautionStructure GetRoadCaution(RouteCautions routeCaution, string userSchema = UserSchema.Portal)
        {
            AnalysedCautionStructure newAnalCautStruct = new AnalysedCautionStructure
            {
                CautionedEntity1 = new AnalysedCautionChoiceStructure
                {
                    AnalysedCautionConstraintStructure = new AnalysedCautionConstraintStructure
                    {
                        Annotation = new List<ResolvedAnnotationStructure>(),

                        ECRN = routeCaution.CautionConstraintCode,

                        Name = routeCaution.CautionConstraintName
                    }
                }
            };

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
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.telecommscable;
                    break;

                case "gantry road furniture":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.gantryroadfurniture;
                    break;

                case "cantilever road furniture":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.cantileverroadfurniture;
                    break;

                case "catenary road furniture":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.catenaryroadfurniture;
                    break;

                case "electrification cable":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.electrificationcable;
                    break;

                case "bollard":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.bollard;
                    break;

                case "removable bollard":
                    newAnalCautStruct.CautionedEntity1.AnalysedCautionConstraintStructure.Type = Domain.RouteAssessment.XmlAnalysedCautions.ConstraintType.removablebollard;
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
            newAnalCautStruct.Conditions = new CautionConditionStructure
            {
                MaxAxleWeight = new WeightStructure
                {
                    Value = routeCaution.CautionConstraintValue.AxleWeight
                },

                MaxGrossWeight = new WeightStructure
                {
                    Value = routeCaution.CautionConstraintValue.GrossWeight
                },

                MaxHeight = new DistanceStructure
                {
                    Value = (decimal)routeCaution.CautionConstraintValue.MaxHeight
                },

                MaxOverallLength = new DistanceStructure
                {
                    Value = (decimal)routeCaution.CautionConstraintValue.MaxLength
                },

                MaxWidth = new DistanceStructure
                {
                    Value = (decimal)routeCaution.CautionConstraintValue.MaxWidth
                },

                MinSpeed = new SpeedStructure
                {
                    Value = (decimal)routeCaution.CautionConstraintValue.MinSpeed
                }
            };

            #region routeConstrType Switch statement
            try
            {
                newAnalCautStruct.ConstrainingAttribute = new List<CautionConditionType>();

                if (routeCaution.CautionConstraintValue.GrossWeight != 0)
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.grossweight);

                if (routeCaution.CautionConstraintValue.AxleWeight != 0)
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.axleweight);

                if (routeCaution.CautionConstraintValue.MaxHeight != 0)
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.height);

                if (routeCaution.CautionConstraintValue.MaxLength != 0)
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.overalllength);

                if (routeCaution.CautionConstraintValue.MaxWidth != 0)
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.width);

                if (routeCaution.CautionConstraintValue.MinSpeed != 0)
                    newAnalCautStruct.ConstrainingAttribute.Add(CautionConditionType.speed);

                if (newAnalCautStruct.ConstrainingAttribute.Count == 0)
                    newAnalCautStruct.ConstrainingAttribute = null;
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Exception occurred, Exception: " + ex;
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, exceptionMessage);
            }
            #endregion

            //New Object creation to save the specific action tag value
            newAnalCautStruct.Action = new CautionActionStructure
            {
                SpecificAction = routeCaution.cautDescription // specific action performed for cautions
            };

            newAnalCautStruct.Contact = new List<ResolvedContactStructure>();

            #region Caution contact part being added
            //Entering the contact details.
            foreach (AssessmentContacts cautContactObj in routeCaution.CautionContactList)
            {
                ResolvedContactStructure newContactObj = new ResolvedContactStructure
                {
                    Address = new Domain.RouteAssessment.XmlAnalysedCautions.AddressStructure()
                };

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

        #region GetRoadDetails
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
                });
            return roadIdentification;
        }
        #endregion
        
        #endregion

        #region Affected Constraints

        #region GenerateAffectedConstraints
        public static string GenerateAffectedConstraints(List<RoutePartDetails> routePartDet, string userSchema)
        {
            string constraintXmlString = null;
            AnalysedConstraints constr = new AnalysedConstraints
            {
                AnalysedConstraintsPart = new List<AnalysedConstraintsPart>()
            };
            AnalysedConstraintsPart analConstrPart;
            foreach (RoutePartDetails routePart in routePartDet)
            {
                analConstrPart = new AnalysedConstraintsPart();
                analConstrPart.Id = (int)routePart.RouteId;
                analConstrPart.AnalysedConstraintsPartName = routePart.RouteName; // constraint route_part Name 

                List<RouteConstraints> routeConstr;

                routeConstr = FetchConstraintList((int)routePart.RouteId, 1, userSchema); //For Route_part related table route type is set to 1 in case of constraints 
                //analConstrPart.Constraint = routeConstr;
                if (routeConstr != null || routeConstr.Count > 0)
                {
                   // routeAnnot.AnalysedAnnotationsPart.Add(analysedAnnotationPart);
                
                //if (routeConstr.Count != 0)
                //{
                    //Function called to generate object containing constraint list for a part of route as single route-part is passed 
                    //AnalysedConstraintsPart analConstrPart = new AnalysedConstraintsPart();
                    analConstrPart = StringExtraction.ConstraintListToXml(analConstrPart, routeConstr, (int)routePart.RouteId, routePart.RouteName);

                    constr.AnalysedConstraintsPart.Add(analConstrPart);
                }
            }
            //condition to generate affected constraint's
            if (routePartDet.Count > 0 && constr.AnalysedConstraintsPart.Count > 0)
                constraintXmlString = StringExtraction.XmlConstraintSerializer(constr); //function to generate constraint XML

            return constraintXmlString; //returning generated driving 
        }
        #endregion

        #region FetchConstraintList
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
                    instance.ConstraintGeometry = records.GetGeometryOrNull("GEOMETRY");
                    instance.ConstraintSuitability = records.GetStringOrDefault("SUITABILITY"); //variable to store constraint suitability
                    instance.ConstraintValue = new ConstraintValues
                    {
                        GrossWeight = records.GetInt32OrDefault("GROSS_WEIGHT"),
                        AxleWeight = records.GetInt32OrDefault("AXLE_WEIGHT"),
                        MaxHeight = records.GetSingleOrDefault("MAX_HEIGHT_MTRS"),
                        MaxLength = records.GetSingleOrDefault("MAX_LEN_MTRS"),
                        MaxWidth = records.GetSingleOrDefault("MAX_WIDTH_MTRS")
                    };
                    instance.CautionList = GetConstraintCautionList(instance.ConstraintId, userSchema);
                });
            return routeConst;
        }
        #endregion

        #region GetConstraintGeoDetails
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
                });
            return routeConstraintRef;
        }
        #endregion

        #region GetConstraintCautionList
        public static List<RouteCautions> GetConstraintCautionList(long constraintId, string userSchema)
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
                    instance.cautDescription = StringExtraction.XmlStringExtractor(records.GetStringOrDefault("SPECIFIC_ACTION"), "SpecificAction");
                    instance.CautionConstraintValue = new ConstraintValues
                    {
                        GrossWeight = (long)records.GetInt32OrDefault("GROSS_WEIGHT"),
                        AxleWeight = (long)records.GetInt32OrDefault("AXLE_WEIGHT"),
                        MaxHeight = records.GetSingleOrDefault("MAX_HEIGHT"),
                        MaxLength = records.GetSingleOrDefault("MAX_LENGTH"),
                        MaxWidth = records.GetSingleOrDefault("MAX_WIDTH"),
                        MinSpeed = records.GetSingleOrDefault("MIN_SPEED")
                    };
                });
            return routeCaution;
        }
        #endregion

        #endregion

        #region Affected Annotation

        #region GenerateAffectedAnnotation
        public static string GenerateAffectedAnnotation(List<RoutePartDetails> routePartDet, string userSchema)
        {
            string annotationXml = null;

            AnalysedAnnotations routeAnnot = new AnalysedAnnotations
            {
                AnalysedAnnotationsPart = new List<AnalysedAnnotationsPart>()
            };

            AnalysedAnnotationsPart analysedAnnotationPart;

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
                annotationXml = StringExtraction.AnnotationSerializer(routeAnnot);

            return annotationXml;
        }
        #endregion

        #region GetAnnotationList
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
                    annotObj = new Annotation
                    {
                        AnnotationId = (int)records.GetLongOrDefault("ANNOTATION_ID"),
                        AnnotationType = records.GetStringOrDefault("TYPE_ANNOT"),
                        Road = new AnnotationRoad()
                    };
                    annotObj.Road.Name = records.GetStringOrDefault("ROAD_NAME");
                    annotObj.Text = new Text
                    {
                        TextValue = new List<string>()
                    };
                    annotText = StringExtraction.XmlStringExtractor(records.GetStringOrDefault("ANNOT_TEXT"), "annotation");
                    annotObj.Text.TextValue.Add(annotText);
                    annotObj.AnnotatedEntity = new AnnotatedEntity
                    {
                        Road = new AnnotedEntityRoad
                        {
                            OSGridRef = records.GetStringOrDefault("GRID_REF")
                        }
                    };
                    annotationList.Add(annotObj);
                });
            return annotationList;
        }
        #endregion

        #endregion

        #region Affected Roads

        #region GenerateAffectedRoads
        public static string GenerateAffectedRoads(List<RoutePartDetails> routePartDet, string userSchema)
        {
            AnalysedRoadsRoute analysedRoads = new AnalysedRoadsRoute();

            analysedRoads.AnalysedRoadsPart = new List<AnalysedRoadsPart>();

            foreach (RoutePartDetails routePart in routePartDet)
            {
                analysedRoads.AnalysedRoadsPart.Add(FetchAffectedRoads((int)routePart.RouteId, routePart.RouteName, userSchema));
            }

            string XmlString = StringExtraction.AnalysedRoadsSerializer(analysedRoads);

            return XmlString;
        }
        #endregion

        #region FetchAffectedRoads
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
                        if (strTmpPoliceName1 != null && strTmpPoliceName1 != "" && roadOwnerList != null && !roadOwnerList.Contains(strTmpPoliceName1))
                        {
                            affectdRoadResponsible = new AffectedRoadResponsibleContact
                            {
                                RespContId = affectdRoad.PoliceContactId,
                                RespOrgId = affectdRoad.PoliceOrgId,
                                RespOrgName = affectdRoad.PoliceForceName
                            };

                            affectdRoadResponsibleDeleg = new AffectedRoadResponsibleDelegation
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

                            roadOwnerList.Add(strTmpPoliceName1); // adding the police organisation inside the list
                        }

                        if (strTmpMgrName1 != null && strTmpMgrName1 != "" && roadOwnerList != null && !roadOwnerList.Contains(strTmpMgrName1))
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

                        #endregion
                        //check here the link no whether same as the previous link number if no then go for distance calculation

                        //distance is calculated.
                        if (temp_Link_no != affectdRoad.LinkNo)
                            dist += affectdRoad.Distance;
                        //a single Link can form a route path and the link no can be 0 another path tha immidiately follows 
                        //will have start link and the link no will be 0 to consider this case except the Link no having 0 value for different link id
                        if (affectdRoad.LinkNo == 0 && temp_Link_id != affectdRoad.LinkId)
                            temp_Link_no = -1;
                        else
                            temp_Link_no = affectdRoad.LinkNo;
                        temp_Link_id = affectdRoad.LinkId;
                        //calculated distance from object added to new object
                        if (afftdRod != null)
                            afftdRod.Distance = dist;
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

        #region GenerateAffectedRoadsXml
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

        #region UpdateAnalysedRoute
        public static long UpdateAnalysedRoute(RouteAssessmentModel routeAssess, long analysisId, string userSchema)
        {
            long result = 0;
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

        #endregion

        #region CautionsBlobMigration Temp
        public static List<RouteAnalysisModel> GetRouteAnalysisTemp(int PageNo, int PageSize, string UserSchema)
        {

            List<RouteAnalysisModel> routeAnalysisListObj = new List<RouteAnalysisModel>();
            try
            {
                string procName = ".GET_ROUTE_ANALYSIS_TEMP";

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    routeAnalysisListObj,
                    UserSchema + procName,
                    parameter =>
                    {
                        parameter.AddWithValue("pageNumber", PageNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("pageSize", PageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.NotificationId = records.GetDataTypeName("NOTIFICATION_ID") == "Int64" ? Convert.ToInt32(records.GetLongOrDefault("NOTIFICATION_ID")) : Convert.ToInt32(records.GetDecimalOrDefault("NOTIFICATION_ID"));
                            instance.AnalysisId = records.GetDataTypeName("ANALYSIS_ID") == "Int64" ? Convert.ToInt32(records.GetLongOrDefault("ANALYSIS_ID")) : Convert.ToInt32(records.GetDecimalOrDefault("ANALYSIS_ID"));
                            instance.VersionId = records.GetDataTypeName("VERSION_ID") == "Int64" ? Convert.ToInt32(records.GetLongOrDefault("VERSION_ID")) : Convert.ToInt32(records.GetDecimalOrDefault("VERSION_ID"));
                            instance.RevisionId = records.GetDataTypeName("REVISION_ID") == "Int64" ? Convert.ToInt32(records.GetLongOrDefault("REVISION_ID")) : Convert.ToInt32(records.GetDecimalOrDefault("REVISION_ID"));
                            instance.Reference = records.GetStringOrDefault("PLANNED_CONTENT_REF_NO");
                        }

                );
                return routeAnalysisListObj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<RoutePartDetails> GetRouteDetailForAnalysis(long versionId, string contentRefNo, long revisionId, int isCandidate, string userSchema)
        {
            List<RoutePartDetails> routeList = new List<RoutePartDetails>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                routeList,
                userSchema + ".STP_ROUTE_ASSESSMENT.SP_GET_ROUTE_FOR_ANALYSIS",
                parameter =>
                {
                    parameter.AddWithValue("P_VERSION_ID", versionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTENT_REF", contentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REVISION_ID", revisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CAND_FLAG", isCandidate, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ROUTE_ID", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.RouteId = records.GetLongOrDefault("ROUTE_PART_ID");
                    instance.RouteName = records.GetStringOrDefault("PART_NAME");
                    instance.RouteType = records.GetStringOrDefault("TRANSPORT_MODE");
                });
            return routeList;
        }

        public static AnalysedCautions GenerateAffectedCautionsTemp(List<RoutePartDetails> routePartDet, int orgId, string userSchema)
        {
            AnalysedCautions newCautionObj = new AnalysedCautions
            {
                AnalysedCautionsPart = new List<AnalysedCautionsPart>()
            };
            try
            {

                bool cautExist = false;

                AnalysedCautionsPart newAnalCautPart = null;

                #region Analysed caution Part adding

                if (routePartDet.Count == 0)
                    return null;

                foreach (RoutePartDetails routePart in routePartDet)
                {
                    newAnalCautPart = new AnalysedCautionsPart
                    {
                        ComparisonId = 1123, //dummy comparison id

                        Id = (int)routePart.RouteId
                    };

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
                    //if (routeCautionList.Count != 0)
                    //{
                    foreach (RouteCautions routeCaution in routeCautionList)
                    {
                        if (routeCaution.CautionType == 0)
                            newAnalCautStruct = GetStructureCaution(routeCaution);
                        else
                            newAnalCautStruct = GetRoadCaution(routeCaution, userSchema);
                        newAnalCautPart.Caution.Add(newAnalCautStruct);
                    }
                    //}
                    #endregion

                    //if (newAnalCautPart.Caution.Count == 0)
                    //    newCautionObj.AnalysedCautionsPart = null;
                    //else
                    //{
                    //    if (newCautionObj.AnalysedCautionsPart == null)

                    newCautionObj.AnalysedCautionsPart.Add(newAnalCautPart);
                    //}
                    #endregion
                }
                #endregion

                if (newCautionObj.AnalysedCautionsPart != null)
                {
                    foreach (AnalysedCautionsPart cautList in newCautionObj.AnalysedCautionsPart)
                    {
                        if (cautList.Caution.Count > 0)
                            cautExist = true; //condition to check whether any route part has cautions or not.
                    }
                }
            }
            catch (Exception e)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Caution cannot be generated, Exception: " + e);
            }
            return newCautionObj;
        }

        public static long UpdateProcessedRowInTempTable(long analysisId, string userSchema)
        {
            long result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".SP_UPDATE_ROUTE_ANALYSIS_TEMP",
                 parameter =>
                 {
                     parameter.AddWithValue("P_ANALYSIS_ID", analysisId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                 },
                 records =>
                 {
                 });

            return result;
        }
        #endregion

        #region GetRoutesDetailsForMovement
        public static Domain.ExternalAPI.DIList GetRouteDetailsForMovement(string ESDALReferenceNumber, int movementTypeFlag, long organisationId, string userSchema)
        {
            Domain.ExternalAPI.DIList dIList = new Domain.ExternalAPI.DIList();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                dIList,
                userSchema + ".STP_EXPORT_DETAILS.SP_GET_DRIVING_INSTRUCTIONS",
                parameter =>
                {
                    parameter.AddWithValue("p_ESDAL_REF_NO", ESDALReferenceNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MOVEMENT_TYPE", movementTypeFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.MovementStatus = records.GetStringOrDefault("VERSION_STATUS");
                    instance.AnalysisId = (long)records.GetDecimalOrDefault("ANALYSIS_ID");
                    if (movementTypeFlag == 1)
                    {
                        instance.ContentRefNum = records.GetStringOrDefault("CONTENT_REF_NUM");
                        instance.NotificationType = (int)records.GetDecimalOrDefault("NOTIF_TYPE");
                    }
                    if(movementTypeFlag == 2)
                        instance.VersionId = records.GetLongOrDefault("VERSION_ID");
                }
                );
            return dIList;
        }
        #endregion

    }
}