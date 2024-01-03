using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using STP.Common.Constants;
using STP.Common.General;
using STP.Domain.Applications;
using STP.Domain.Custom;
using STP.Domain.VehiclesAndFleets.Configuration;
using static STP.Common.Enums.ExternalApiEnums;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    public class NotificationGeneralDetails
    {
        public long NotificationId { get; set; }

        public long AnalysisId { get; set; }

        public long VersionId { get; set; }

        public long RevId { get; set; }

        public string MovementName { get; set; }

        public string ESDALReference { get; set; }

        public int UpdateRouteAnalysis { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Classification { get; set; }

        public byte[] NotesFromHaulier { get; set; }

        public DateTime MovementDateFrom { get; set; }

        public DateTime MovementDateTo { get; set; }

        public int NoOfMovements { get; set; }

        public int MaxPiecesPerLoad { get; set; }

        public int VehicleCode { get; set; }

        public string ContentReferenceNo { get; set; }

        public long RoutePartIdS { get; set; }

        public long LibRoutePartId { get; set; }

        public long VehicleId { get; set; }

        public int VehicleComponantId { get; set; }

        [RegularExpression(@"[0-9]+$", ErrorMessage = "Only numbers are allowed")]
        public int NoOfMoves { get; set; }

        [RegularExpression(@"[0-9]+$", ErrorMessage = "Only numbers are allowed")]
        public int MaximamPieces { get; set; }

        public int OrganisationId { get; set; }

        public int UserId { get; set; }

        public int VehicleCategory { get; set; }

        public int ProjectStatus { get; set; }

        public int NeedsAttention { get; set; }

        public int EnteredBySORT { get; set; }

        public int RequiresVR1 { get; set; }

        public int LatestRevisionNo { get; set; }

        public int IsNextNotification { get; set; }

        public int IsWithdrawn { get; set; }

        public int IsDeclined { get; set; }

        public int VersionNo { get; set; }

        public int VersionStatus { get; set; }

        public int IsWorkInProgress { get; set; }

        public int IsNotified { get; set; }

        public int IsMostRecent { get; set; }

        public int IsVersionedBySORT { get; set; }

        public int NotificationNo { get; set; }

        public int NotificationVersionNo { get; set; }

        public int NotYetAgreed { get; set; }

        public int? RevisionId { get; set; }

        public int UpdateRoute { get; set; }

        public int UpdateVehicle { get; set; }

        public int MaxVersion { get; set; }

        public int MaxVersion1 { get; set; }

        public int FleetId { get; set; }

        public string NotificationCode { get; set; }

        public string MyReference { get; set; }

        public string ClientName { get; set; }

        public string HaulierOprLicence { get; set; }

        public string FromSummary { get; set; }

        public string ToSummary { get; set; }

        public string FromAddress { get; set; }

        public string ToAddress { get; set; }

        public string LoadDescription { get; set; }

        public string RegisterNo { get; set; }

        public string FleetNos { get; set; }

        public string VehicleType { get; set; }

        public string Notes { get; set; }

        public string NotesOnEscort { get; set; }

        public string OrderingESDALReferenceNo { get; set; }

        public string HauliersReference { get; set; }

        public string NotificationDate { get; set; }

        public string VR1Number { get; set; }

        public string SONumbers { get; set; }

        public string VSONumber { get; set; }

        public int VSOType { get; set; }

        public byte[] InboundNotification { get; set; }

        public byte[] OutboundNotification { get; set; }

        public byte[] HauliersNotification { get; set; }

        public DateTime NeedsAttentionDate { get; set; }
        public DateTime NotificationCreationDate { get; set; }

        [Required(ErrorMessage = "Enter date time(to).")]
        public DateTime ToDateTime { get; set; }

        [Required(ErrorMessage = "Enter date time(from).")]
        public DateTime FromDateTime { get; set; }

        public decimal VehicleLength { get; set; }

        public decimal VehicleWidth { get; set; }

        public decimal? FrontProjection { get; set; }

        public decimal? RearProjection { get; set; }

        public decimal? LeftProjection { get; set; }

        public decimal? RightProjection { get; set; }

        public decimal MaximamHeight { get; set; }

        public decimal? ReducibleHeight { get; set; }

        public decimal RigidLength { get; set; }

        [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public int GrossWeight { get; set; }

        public int? AxelWeight { get; set; }

        public bool PlanRouteYes { get; set; }

        public bool PlanRouteNo { get; set; }

        public bool RetJourny { get; set; }

        public bool CollaborationStatus { get; set; }

        public bool IsAToB { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string NotificationCreateDate { get; set; }

        public string NotifDate { get; set; }

        public string OrganisationUser { get; set; }

        public string Username { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string SurName { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        //for checking validation
        public int CheckRoute { get; set; }

        public int CheckVehicle { get; set; }

        public int CheckVehicleConfig { get; set; }

        public int CheckRouteConfig { get; set; }

        public int CheckVehicleReg { get; set; }

        public int CheckVehicleImport { get; set; }

        public int CheckVehicleWeight { get; set; }

        public int CheckRouteImport { get; set; }

        //Show broken routes
        public int CheckBrokenRoutes { get; set; }

        public int IsSimplified { get; set; }

        public int ShowWarning { get; set; }

        public int PlanRouteOnMapId { get; set; }

        public int PlanRouteOnMapId1 { get; set; }

        public int AddingRouteBy { get; set; }

        public int AxleCounter { get; set; }

        public int IsIndemnity { get; set; }

        public string OnBehalfOf { get; set; }

        //parameters for planning route
        public decimal? StartEasting { get; set; }

        public decimal? StartNorthing { get; set; }

        public string StartDescription { get; set; }

        public decimal? EndEasting { get; set; }

        public decimal? EndNorthing { get; set; }

        public string EndDescription { get; set; }

        public string NotificationRouteName { get; set; }

        public string NotificationRetRouteName { get; set; }

        public long RetRoutePartId { get; set; }

        public int RouteCount { get; set; }

        public int CheckVehicleLength { get; set; }

        public int InValidVehicleCount { get; set; }

        public int CheckVehicleGrossWeight { get; set; }

        public int InValidWeightVehicleCount { get; set; }

        public int CheckVehicleWidth { get; set; }

        public int InValidWidthVehicleCount { get; set; }

        public int CheckVehicleAxleWeight { get; set; }

        public int InValidAxleWeightVehicleCount { get; set; }

        public int CheckVehicleRigidLength { get; set; }

        public int InValidRigidLengthVehicleCount { get; set; }

        public int CheckIsReplanCount { get; set; }

        public int CheckIsSpecialManourCount { get; set; }

        ////For Folder Showing DropDownList////

        public long FolderId { get; set; }

        public long ProjectId { get; set; }

        public List<ProjectFolderModel> FolderDomainList { get; set; }

        public List<NotifDispensations> NotificationDispensationList { get; set; }

        public List<NotifVehicleRegistration> NotificationVehicleRegisterList { get; set; }

        public List<NotifVehicleImport> NotificationVehicleImportList { get; set; }

        public List<NotifVehicleWeight> NotificationVehicleWeightList { get; set; }

        public List<NotifRouteImport> NotificationRouteImportList { get; set; }


        //Broken routes
        public List<NotifRouteImport> BrokenRoutes { get; set; }

        public List<NotifVehicleImport> NotificationVehicleLengthList { get; set; }

        public string[] NotifRouteImportList { get; set; }

        public List<AxleDetails> NotificationAxleDetails { get; set; }

        public List<NotifVehicleImport> NotificationVehicleGrossWeightList { get; set; }

        public List<NotifVehicleImport> NotificationVehicleWidthList { get; set; }

        public List<NotifVehicleImport> NotificationVehicleAxleWeightList { get; set; }

        public List<NotifVehicleImport> NotificationVehicleRLList { get; set; }

        // for the set of hidden fields
        public decimal MetricVehicleLength { get; set; }

        public decimal MetricVehicleWidth { get; set; }

        public decimal? MetricFrontProjection { get; set; }

        public decimal? MetricRearProjection { get; set; }

        public decimal? MetricLeftProjection { get; set; }

        public decimal? MetricRightProjection { get; set; }

        public decimal MetricMaximamHeight { get; set; }

        public decimal? MetricReducibleHeight { get; set; }

        public decimal MetricRigidLength { get; set; }

        public bool IndemnifyFlag { get; set; }

        public string ActingOnBehalfOf { get; set; }
        public long MovementId { get; set; }
        public string DispensationId { get; set; }

        public List<long> DispensationList { get; set; }
        public string  PreviousContactName { get; set; }
        public string ImminentMessage { get; set; }

        public NotificationGeneralDetails()
        {
            ProjectFolderModel sofolder = new ProjectFolderModel();
            sofolder.FolderId = 0;
            sofolder.FolderName = "--None--";
            FolderDomainList = new List<ProjectFolderModel>();
            DispensationList = new List<long>();
        }
    }

    public class AxleDetails
    {

        public int NoOfWheels { get; set; }

        public decimal AxleWeight { get; set; }

        public decimal AxleSpacing { get; set; }

        public int ComponentId { get; set; }

        public int AxleNumberId { get; set; }

        public string TyreCenters { get; set; }

        public string TyreSize { get; set; }

        public double DistanceToNextAxle { get; set; }

        public int AxleNo { get; set; }

        public decimal ComponentType { get; set; }
    }

    public class NotifDispensations
    {
        public string DRN { get; set; }

        public string Summary { get; set; }
        public int DispensationId { get; set; }
        public string GrantorName { get; set; }
        public int GrantorId { get; set; }
    }

    public class NotifVehicleRegistration
    {
        public long VehicleId { get; set; }

        public string VehicleName { get; set; }

        public string VehicleRegistration { get; set; }
    }

    public class NotifVehicleImport
    {
        public long VehicleId { get; set; }

        public string VehicleName { get; set; }

        public int VehicleClass { get; set; }
    }

    public class NotifVehicleWeight
    {
        public string VehicleName { get; set; }
    }

    public class NotifRouteImport
    {
        public string RouteName { get; set; }

        public int IsReplan { get; set; }
    }
    public class CommonNotifMethods
    {
        public int GetImminent(int workingDays, VehicleMovementType vehicleMovement)
        {
            //1 - Imminent movement.
            //2 - Imminent movement for police.
            //3 - Imminent movement for SOA.
            //4 - Imminent movement for SOA and police.
            //5 - No imminent movement.
            int imminent = 5;
            if (vehicleMovement.PoliceNoticePeriod == 0 && vehicleMovement.SOANoticePeriod == 0)
            {
                imminent = 1;
            }
            else
            {
                if (workingDays < vehicleMovement.SOANoticePeriod)
                {
                    imminent = 3;
                }
                if (workingDays < vehicleMovement.PoliceNoticePeriod)
                {
                    if (imminent != 5)
                    {
                        imminent = 4;
                    }
                    else
                    {
                        imminent = 2;
                    }
                }
            }
            return imminent;
        }
        public string GetImminentMessage(int imminentStat, string strContryID)
        {
            string message;
            if (imminentStat == 1)
            {
                message = "Imminent movement." + "," + strContryID;
            }
            else if (imminentStat == 2)
            {
                message = "Imminent movement for police." + "," + strContryID;
            }
            else if (imminentStat == 3)
            {
                message = "Imminent movement for SOA." + "," + strContryID;
            }
            else if (imminentStat == 4)
            {
                message = "Imminent movement for SOA and police." + "," + strContryID;
            }
            else
            {
                message = "No imminent movement";
            }
            return message;
        }
        public List<long> GetAffectedStructList(byte[] structures)
        {
            List<long> sectionList = new List<long>();
            if (structures != null)
            {
                // object containing the list of affected structures 
                Domain.RouteAssessment.XmlAnalysedStructures.AnalysedStructures structureListObj;
                string xmlaffectedStructures = Encoding.UTF8.GetString(XsltTransformer.Trafo(structures));
                //Deserializing the xml into object and storing it 
                structureListObj = StringExtraction.XmlDeserializerStructures(xmlaffectedStructures);

                // parent node of Affected structures xml
                foreach (Domain.RouteAssessment.XmlAnalysedStructures.AnalysedStructuresPart structureListPart in structureListObj.AnalysedStructuresPart)
                {
                    foreach (Domain.RouteAssessment.XmlAnalysedStructures.Structure structListObj in structureListPart.Structure)
                    {
                        sectionList.Add(structListObj.StructureSectionId);
                    }
                }
            }
            return sectionList;
        }
        public List<string> GetAffectedConstrList(byte[] constraints)
        {
            List<string> constarintList = new List<string>();
            if (constraints != null)
            {
                // object containing the list of affected Constraint 
                Domain.RouteAssessment.XmlConstraints.AnalysedConstraints ConstraintListObj;
                string xmlaffectedConstraint = Encoding.UTF8.GetString(XsltTransformer.Trafo(constraints));
                //Deserializing the xml into object and storing it for constraint
                ConstraintListObj = StringExtraction.constraintDeserializer(xmlaffectedConstraint);
                //For constraint
                foreach (Domain.RouteAssessment.XmlConstraints.AnalysedConstraintsPart constraintListPart in ConstraintListObj.AnalysedConstraintsPart)
                {
                    foreach (Domain.RouteAssessment.XmlConstraints.Constraint ConstrListObj in constraintListPart.Constraint)
                    {
                        constarintList.Add(ConstrListObj.ECRN);
                    }
                }
            }
            return constarintList;
        }
        public Dictionary<int, int> GetNotifICAstatus(string xmlaffectedStructures)
        {
            // object containing the list of affected structures 
            RouteAssessment.XmlAnalysedStructures.AnalysedStructures structureListObj;

            //Deserializing the xml into object and storing it 
            structureListObj = StringExtraction.XmlDeserializerStructures(xmlaffectedStructures);

            Dictionary<int, int> orgSuitablityDict = new Dictionary<int, int>();

            int orgId;

            // parent node of Affected structures xml
            foreach (RouteAssessment.XmlAnalysedStructures.AnalysedStructuresPart structureListPart in structureListObj.AnalysedStructuresPart)
            {
                foreach (RouteAssessment.XmlAnalysedStructures.Structure structListObj in structureListPart.Structure)
                {
                    // ICA is performed only for underbriges so only that status need to be updated
                    foreach (RouteAssessment.XmlAnalysedStructures.Appraisal apprais in structListObj.Appraisal)
                    {
                        orgId = apprais.OrganisationId;
                        /* 
                           277001 - unknown > No priority
                           277002 - suitable > Priority4
                           277003 - marginal > Priority3
                           277004 - unsuitable > Priority1
                           277005 - erroneous  > Priority2
                         */
                        string suitability = !string.IsNullOrWhiteSpace(apprais.AppraisalSuitability.Value) ? apprais.AppraisalSuitability.Value.ToLower() :
                            IcaSuitability.IcaDisabled;
                        switch (suitability)
                        {
                            case IcaSuitability.IcaDisabled:
                                if (!orgSuitablityDict.ContainsKey(orgId))
                                    orgSuitablityDict.Add(orgId, (int)ExternalApiSuitability.unknown); // unknown
                                break;

                            case IcaSuitability.Suitable:
                                if (orgSuitablityDict.ContainsKey(orgId))
                                {
                                    if (orgSuitablityDict[orgId] == (int)ExternalApiSuitability.unknown)
                                        orgSuitablityDict[orgId] = (int)ExternalApiSuitability.suitable;
                                }
                                else
                                    orgSuitablityDict.Add(orgId, (int)ExternalApiSuitability.suitable);
                                break;

                            case IcaSuitability.MariginalSuitable:
                            case IcaSuitability.MariginalSuit:
                                if (orgSuitablityDict.ContainsKey(orgId))
                                {
                                    if (orgSuitablityDict[orgId] == (int)ExternalApiSuitability.unknown
                                        || orgSuitablityDict[orgId] == (int)ExternalApiSuitability.suitable)
                                    {
                                        orgSuitablityDict[orgId] = (int)ExternalApiSuitability.marginal;
                                    }
                                }
                                else
                                    orgSuitablityDict.Add(orgId, (int)ExternalApiSuitability.marginal);
                                break;

                            case IcaSuitability.NotApplicable:
                            case IcaSuitability.Sidebyside:
                            case IcaSuitability.MoreComponent:
                            case IcaSuitability.Minaxlespace:
                            case IcaSuitability.Svtrain:
                            case IcaSuitability.Stgocat1:
                            case IcaSuitability.Axleweight:
                            case IcaSuitability.Axleweigthscreening:
                            case IcaSuitability.Structurewidth:
                            case IcaSuitability.Grossweight:
                            case IcaSuitability.Grossweightscreen:
                            case IcaSuitability.NotSvVehicle:
                                if (orgSuitablityDict.ContainsKey(orgId))
                                {
                                    if (orgSuitablityDict[orgId] != (int)ExternalApiSuitability.unsuitable)
                                    {
                                        orgSuitablityDict[orgId] = (int)ExternalApiSuitability.erroneous;
                                    }
                                }
                                else
                                {
                                    orgSuitablityDict.Add(orgId, (int)ExternalApiSuitability.erroneous);
                                }
                                break;

                            case IcaSuitability.Unsuitable:
                                if (orgSuitablityDict.ContainsKey(orgId))
                                    orgSuitablityDict[orgId] = (int)ExternalApiSuitability.unsuitable;
                                else
                                    orgSuitablityDict.Add(orgId, (int)ExternalApiSuitability.unsuitable);
                                break;
                            default:
                                if (!orgSuitablityDict.ContainsKey(orgId))
                                    orgSuitablityDict.Add(orgId, (int)ExternalApiSuitability.unknown); // unknown
                                break;
                        }
                    }
                }
            }
            return orgSuitablityDict;
        }
    }
}