using STP.Common.Enums;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using static STP.Domain.VehiclesAndFleets.VehicleEnums;

namespace STP.Domain.VehiclesAndFleets.Vehicles
{
    public static class VehicleCategoryToMovementType
    {
        public static int VehicleCategoryMapping(VehicleMovementType vehicleMovementType, bool isNen = false, bool isNea = false)
        {
            switch (vehicleMovementType.VehicleClass)
            {
                case (int)VehicleClassificationType.VehicleSpecialOrder:
                    return (int)VehicleCategoryToMovementTypeMapping.VehicleSpecialOrder;
                case (int)VehicleClassificationType.SpecialOrder:
                    if (isNen)
                        return (int)VehicleCategoryToMovementTypeMapping.SONotification;
                    else if (isNea)
                        return (int)VehicleCategoryToMovementTypeMapping.SOApplication;
                    else
                        return (int)VehicleCategoryToMovementTypeMapping.SpecialOrder;
                case (int)VehicleClassificationType.StgoailCat1:
                    switch (vehicleMovementType.MovementType)
                    {
                        case (int)MovementType.vr_1:
                            return vehicleMovementType.SOANoticePeriod == 2 && vehicleMovementType.PoliceNoticePeriod == 2
                                ? (int)VehicleCategoryToMovementTypeMapping.StgoailCat1Vr1_Soa2_Pol2
                                : vehicleMovementType.PoliceNoticePeriod == 2
                                    ? (int)VehicleCategoryToMovementTypeMapping.StgoailCat1Vr1_Pol2
                                    : 0;
                        default:
                            return vehicleMovementType.SOANoticePeriod == 2 && vehicleMovementType.PoliceNoticePeriod == 2
                                ? (int)VehicleCategoryToMovementTypeMapping.StgoailCat1_Soa2_Pol2
                                : vehicleMovementType.SOANoticePeriod == 2
                                    ? (int)VehicleCategoryToMovementTypeMapping.StgoailCat1_Soa2
                                    : vehicleMovementType.PoliceNoticePeriod == 2
                                        ? (int)VehicleCategoryToMovementTypeMapping.StgoailCat1_Pol2
                                        : 0;
                    }
                case (int)VehicleClassificationType.StgoailCat2:
                    switch (vehicleMovementType.MovementType)
                    {
                        case (int)MovementType.vr_1:
                            return (int)VehicleCategoryToMovementTypeMapping.StgoailCat2Vr1_Soa2_Pol2;

                        default:
                            return vehicleMovementType.SOANoticePeriod == 2 && vehicleMovementType.PoliceNoticePeriod == 2
                                ? (int)VehicleCategoryToMovementTypeMapping.StgoailCat2_Soa2_Pol2
                                : vehicleMovementType.SOANoticePeriod == 2
                                    ? (int)VehicleCategoryToMovementTypeMapping.StgoailCat2_Soa2
                                    : 0;
                    }
                case (int)VehicleClassificationType.StgoailCat3:
                    switch (vehicleMovementType.MovementType)
                    {
                        case (int)MovementType.vr_1:
                            return (int)VehicleCategoryToMovementTypeMapping.StgoailCat3Vr1_Soa5_Pol2;
                        default:
                            return vehicleMovementType.SOANoticePeriod == 2
                                ? vehicleMovementType.PoliceNoticePeriod == 2
                                    ? (int)VehicleCategoryToMovementTypeMapping.StgoailCat3_Soa2_Pol2
                                    : (int)VehicleCategoryToMovementTypeMapping.StgoailCat3_Soa2
                                : vehicleMovementType.SOANoticePeriod == 5 && vehicleMovementType.PoliceNoticePeriod == 2
                                    ? (int)VehicleCategoryToMovementTypeMapping.StgoailCat3_Soa5_Pol2
                                    : 0;
                    }
                case (int)VehicleClassificationType.StgoMobileCraneCata:
                    return vehicleMovementType.SOANoticePeriod == 2
                        ? vehicleMovementType.PoliceNoticePeriod == 2
                            ? (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatA_Soa2_Pol2
                            : (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatA_Soa2
                        : 0;
                case (int)VehicleClassificationType.StgoMobileCraneCatb:
                    return vehicleMovementType.SOANoticePeriod == 2
                        ? vehicleMovementType.PoliceNoticePeriod == 2
                            ? (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatB_Soa2_Pol2
                            : (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatB_Soa2
                        : vehicleMovementType.SOANoticePeriod == 5 && vehicleMovementType.PoliceNoticePeriod == 2
                            ? (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatB_Soa5_Pol2
                            : 0;
                case (int)VehicleClassificationType.StgoMobileCraneCatc:
                    return vehicleMovementType.SOANoticePeriod == 2
                        ? vehicleMovementType.PoliceNoticePeriod == 2
                            ? (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatC_Soa2_Pol2
                            : (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatC_Soa2
                        : vehicleMovementType.SOANoticePeriod == 5 && vehicleMovementType.PoliceNoticePeriod == 2
                            ? (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatC_Soa5_Pol2
                            : 0;
                case (int)VehicleClassificationType.WheeledConstructionAndUse:
                    return vehicleMovementType.PoliceNoticePeriod == 2
                        ? (int)VehicleCategoryToMovementTypeMapping.CandUNotification_Pol2
                        : (int)VehicleCategoryToMovementTypeMapping.CandUNoNotification;
                case (int)VehicleClassificationType.Tracked:
                    return (int)VehicleCategoryToMovementTypeMapping.Tracked;
                case (int)VehicleClassificationType.NoCrane:
                    return (int)VehicleCategoryToMovementTypeMapping.NoCrane;
                case (int)VehicleClassificationType.StgoCat1EngineeringPlantWheeled:
                    switch (vehicleMovementType.MovementType)
                    {
                        case (int)MovementType.vr_1:
                            return vehicleMovementType.SOANoticePeriod == 2
                                    ? vehicleMovementType.PoliceNoticePeriod == 2
                                        ? (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Vr1Notification_Soa2_Pol2
                                        : (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Vr1Notification_Soa2
                                    : vehicleMovementType.PoliceNoticePeriod == 2
                                        ? (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Vr1Notification_Pol2
                                        : 0;
                        default:
                            return vehicleMovementType.SOANoticePeriod == 2
                                    ? vehicleMovementType.PoliceNoticePeriod == 2
                                        ? (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Notification_Soa2_Pol2
                                        : (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Notification_Soa2
                                    : vehicleMovementType.PoliceNoticePeriod == 2
                                        ? (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Notification_Pol2
                                        : 0;
                    }
                case (int)VehicleClassificationType.StgoCat2EngineeringPlantWheeled:
                    switch (vehicleMovementType.MovementType)
                    {
                        case (int)MovementType.vr_1:
                            return vehicleMovementType.SOANoticePeriod == 2
                                    ? vehicleMovementType.PoliceNoticePeriod == 2
                                        ? (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat2Vr1Notification_Soa2_Pol2
                                        : (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat2Vr1Notification_Soa2
                                    : 0;
                        default:
                            return vehicleMovementType.SOANoticePeriod == 2
                                    ? vehicleMovementType.PoliceNoticePeriod == 2
                                        ? (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat2Notification_Soa2_Pol2
                                        : (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat2Notification_Soa2
                                    : 0;
                    }
                case (int)VehicleClassificationType.StgoCat3EngineeringPlantWheeled:
                    switch (vehicleMovementType.MovementType)
                    {
                        case (int)MovementType.vr_1:
                            return vehicleMovementType.SOANoticePeriod == 5
                                ? vehicleMovementType.PoliceNoticePeriod == 2
                                    ? (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat3Vr1Notification_Soa5_Pol2
                                    : 0
                                : vehicleMovementType.SOANoticePeriod == 2
                                    ? (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat3Vr1Notification_Soa2
                                    : 0;
                        default:
                            return vehicleMovementType.SOANoticePeriod == 5
                                ? vehicleMovementType.PoliceNoticePeriod == 2
                                    ? (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat3Notification_Soa5_Pol2
                                    : 0
                                : vehicleMovementType.SOANoticePeriod == 2
                                    ? (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat3Notification_Soa2
                                    : 0;
                    }
                case (int)VehicleClassificationType.StgoCat1EngineeringPlantTracked:
                    switch (vehicleMovementType.MovementType)
                    {
                        case (int)MovementType.vr_1:
                            return vehicleMovementType.SOANoticePeriod == 2
                                    ? vehicleMovementType.PoliceNoticePeriod == 2
                                        ? (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Vr1Notification_Soa2_Pol2
                                        : (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Vr1Notification_Soa2
                                    : vehicleMovementType.PoliceNoticePeriod == 2
                                        ? (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Vr1Notification_Pol2
                                        : 0;
                        default:
                            return vehicleMovementType.SOANoticePeriod == 2
                                    ? vehicleMovementType.PoliceNoticePeriod == 2
                                        ? (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Notification_Soa2_Pol2
                                        : (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Notification_Soa2
                                    : vehicleMovementType.PoliceNoticePeriod == 2
                                        ? (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Notification_Pol2
                                        : 0;
                    }
                case (int)VehicleClassificationType.StgoCat2EngineeringPlantTracked:
                    switch (vehicleMovementType.MovementType)
                    {
                        case (int)MovementType.vr_1:
                            return vehicleMovementType.SOANoticePeriod == 2
                                    ? vehicleMovementType.PoliceNoticePeriod == 2
                                        ? (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat2Vr1Notification_Soa2_Pol2
                                        : (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat2Vr1Notification_Soa2
                                    : 0;
                        default:
                            return vehicleMovementType.SOANoticePeriod == 2
                                    ? vehicleMovementType.PoliceNoticePeriod == 2
                                        ? (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat2Notification_Soa2_Pol2
                                        : (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat2Notification_Soa2
                                    : 0;
                    }
                case (int)VehicleClassificationType.StgoCat3EngineeringPlantTracked:
                    switch (vehicleMovementType.MovementType)
                    {
                        case (int)MovementType.vr_1:
                            return vehicleMovementType.SOANoticePeriod == 5
                                ? vehicleMovementType.PoliceNoticePeriod == 2
                                    ? (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat3Vr1Notification_Soa5_Pol2
                                    : 0
                                : vehicleMovementType.SOANoticePeriod == 2
                                    ? (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat3Vr1Notification_Soa2
                                    : 0;
                        default:
                            return vehicleMovementType.SOANoticePeriod == 5
                                ? vehicleMovementType.PoliceNoticePeriod == 2
                                    ? (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat3Notification_Soa5_Pol2
                                    : 0
                                : vehicleMovementType.SOANoticePeriod == 2
                                    ? (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat3Notification_Soa2
                                    : 0;
                    }
                case (int)VehicleClassificationType.StgoCat1RoadRecovery:
                    switch (vehicleMovementType.MovementType)
                    {
                        case (int)MovementType.vr_1:
                            return vehicleMovementType.SOANoticePeriod == 2
                                    ? vehicleMovementType.PoliceNoticePeriod == 2
                                        ? (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Vr1Notification_Soa2_Pol2
                                        : (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Vr1Notification_Soa2
                                    : vehicleMovementType.PoliceNoticePeriod == 2
                                        ? (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Vr1Notification_Pol2
                                        : 0;
                        default:
                            return vehicleMovementType.SOANoticePeriod == 2
                                    ? vehicleMovementType.PoliceNoticePeriod == 2
                                        ? (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Notification_Soa2_Pol2
                                        : (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Notification_Soa2
                                    : vehicleMovementType.PoliceNoticePeriod == 2
                                        ? (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Notification_Pol2
                                        : 0;
                    }
                case (int)VehicleClassificationType.StgoCat2RoadRecovery:
                    switch (vehicleMovementType.MovementType)
                    {
                        case (int)MovementType.vr_1:
                            return vehicleMovementType.SOANoticePeriod == 2
                                    ? vehicleMovementType.PoliceNoticePeriod == 2
                                        ? (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat2Vr1Notification_Soa2_Pol2
                                        : (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat2Vr1Notification_Soa2
                                    : 0;
                        default:
                            return vehicleMovementType.SOANoticePeriod == 2
                                    ? vehicleMovementType.PoliceNoticePeriod == 2
                                        ? (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat2Notification_Soa2_Pol2
                                        : (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat2Notification_Soa2
                                    : 0;
                    }
                case (int)VehicleClassificationType.StgoCat3RoadRecovery:
                    switch (vehicleMovementType.MovementType)
                    {
                        case (int)MovementType.vr_1:
                            return vehicleMovementType.SOANoticePeriod == 5
                                ? vehicleMovementType.PoliceNoticePeriod == 2
                                    ? (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat3Vr1Notification_Soa5_Pol2
                                    : 0
                                : vehicleMovementType.SOANoticePeriod == 2
                                    ? (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat3Vr1Notification_Soa2
                                    : 0;
                        default:
                            return vehicleMovementType.SOANoticePeriod == 5
                                ? vehicleMovementType.PoliceNoticePeriod == 2
                                    ? (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat3Notification_Soa5_Pol2
                                    : 0
                                : vehicleMovementType.SOANoticePeriod == 2
                                    ? (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat3Notification_Soa2
                                    : 0;
                    }
                default: return (int)VehicleCategoryToMovementTypeMapping.NoNotification;
            }
        }

        public static int VehicleXmlMovementTypeMapping(VehicleXmlMovementType movementType)
        {
            switch (movementType)
            {
                case VehicleXmlMovementType.VehicleSpecialOrder:
                case VehicleXmlMovementType.VSONotification:
                    return (int)VehicleXmlMovementType.VehicleSpecialOrder;
                case VehicleXmlMovementType.SONotification:
                    return (int)VehicleXmlMovementType.SpecialOrder;
                case VehicleXmlMovementType.SOApplication:
                    return (int)VehicleXmlMovementType.SpecialOrder;
                case VehicleXmlMovementType.StgoailCat1Vr1_Pol2:
                case VehicleXmlMovementType.StgoailCat1Vr1_Soa2_Pol2:
                case VehicleXmlMovementType.StgoTrackedCat1Notification_Pol2:
                case VehicleXmlMovementType.StgoTrackedCat1Notification_Soa2:
                case VehicleXmlMovementType.StgoTrackedCat1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoailCat1_Pol2:
                case VehicleXmlMovementType.StgoailCat1_Soa2:
                case VehicleXmlMovementType.StgoailCat1_Soa2_Pol2:
                    return (int)VehicleXmlMovementType.StgoailCat1;
                case VehicleXmlMovementType.StgoTrackedCat2Notification_Soa2:
                case VehicleXmlMovementType.StgoTrackedCat2Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoailCat2Vr1_Soa2_Pol2:
                case VehicleXmlMovementType.StgoailCat2_Soa2:
                case VehicleXmlMovementType.StgoailCat2_Soa2_Pol2:
                    return (int)VehicleXmlMovementType.StgoailCat2;
                case VehicleXmlMovementType.StgoTrackedCat3Notification_Soa2:
                case VehicleXmlMovementType.StgoTrackedCat3Notification_Soa5_Pol2:
                case VehicleXmlMovementType.StgoailCat3Vr1_Soa5_Pol2:
                case VehicleXmlMovementType.StgoailCat3_Soa2:
                case VehicleXmlMovementType.StgoailCat3_Soa5_Pol2:
                case VehicleXmlMovementType.StgoailCat3_Soa2_Pol2:
                    return (int)VehicleXmlMovementType.StgoailCat3;
                case VehicleXmlMovementType.StgoMobileCraneCatA_Soa2:
                case VehicleXmlMovementType.StgoMobileCraneCatA_Soa2_Pol2:
                    return (int)VehicleXmlMovementType.StgoMobileCraneCata;
                case VehicleXmlMovementType.StgoMobileCraneCatB_Soa2:
                case VehicleXmlMovementType.StgoMobileCraneCatB_Soa2_Pol2:
                case VehicleXmlMovementType.StgoMobileCraneCatB_Soa5_Pol2:
                    return (int)VehicleXmlMovementType.StgoMobileCraneCatb;
                case VehicleXmlMovementType.StgoMobileCraneCatC_Soa2:
                case VehicleXmlMovementType.StgoMobileCraneCatC_Soa2_Pol2:
                case VehicleXmlMovementType.StgoMobileCraneCatC_Soa5_Pol2:
                    return (int)VehicleXmlMovementType.StgoMobileCraneCatc;
                case VehicleXmlMovementType.CandUNotification_Pol2:
                case VehicleXmlMovementType.CandUNoNotification:
                    return (int)VehicleXmlMovementType.WheeledConstructionAndUse;
                case VehicleXmlMovementType.Tracked:
                    return (int)VehicleXmlMovementType.Tracked;
                case VehicleXmlMovementType.StgoEngPlantTrackedCat1Notification_Pol2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat1Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat1Notification_Pol2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat1Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat1Notification_Pol2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat1Notification_Soa2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat1Vr1Notification_Pol2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat1Vr1Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat1Vr1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat1Vr1Notification_Pol2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat1Vr1Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat1Vr1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat1Vr1Notification_Pol2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat1Vr1Notification_Soa2_Pol2:
                    return (int)VehicleXmlMovementType.StgoailCat1;
                case VehicleXmlMovementType.StgoEngPlantTrackedCat2Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat2Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat2Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat2Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat2Notification_Soa2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat2Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat2Vr1Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat2Vr1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat2Vr1Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat2Vr1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat2Vr1Notification_Soa2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat2Vr1Notification_Soa2_Pol2:
                    return (int)VehicleXmlMovementType.StgoailCat2;
                case VehicleXmlMovementType.StgoEngPlantTrackedCat3Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat3Notification_Soa5_Pol2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat3Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat3Notification_Soa5_Pol2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat3Notification_Soa2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat3Notification_Soa5_Pol2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat3Vr1Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat3Vr1Notification_Soa5_Pol2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat3Vr1Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat3Vr1Notification_Soa5_Pol2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat3Vr1Notification_Soa2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat3Vr1Notification_Soa5_Pol2:
                    return (int)VehicleXmlMovementType.StgoailCat3;
                case VehicleXmlMovementType.NoNotification:
                    return 0;
                default: return (int)movementType;

            }
        }

        public static List<int> VehicleIntentedUseMapping(VehiclePurpose vehiclePurpose)
        {
            List<int> vehiclePurposeList = new List<int>();
            switch (vehiclePurpose)
            {
                case VehiclePurpose.Stgoail:
                    vehiclePurposeList.Add((int)VehiclePurpose.Stgoail);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoailCat1Vr1_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoailCat1Vr1_Soa2_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoailCat1_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoailCat1_Soa2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoailCat1_Soa2_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoailCat2Vr1_Soa2_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoailCat2_Soa2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoailCat2_Soa2_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoailCat3Vr1_Soa5_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoailCat3_Soa2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoailCat3_Soa5_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Notification_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Notification_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Notification_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoTrackedCat1Notification_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Notification_Soa2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Notification_Soa2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoTrackedCat1Notification_Soa2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Notification_Soa2_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Notification_Soa2_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Notification_Soa2_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoTrackedCat1Notification_Soa2_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat2Notification_Soa2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat2Notification_Soa2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat2Notification_Soa2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoTrackedCat2Notification_Soa2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat2Notification_Soa2_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat2Notification_Soa2_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat2Notification_Soa2_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoTrackedCat2Notification_Soa2_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat3Notification_Soa2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat3Notification_Soa2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat3Notification_Soa2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoTrackedCat3Notification_Soa2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat3Notification_Soa5_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat3Notification_Soa5_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat3Notification_Soa5_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoTrackedCat3Notification_Soa5_Pol2);
                    return vehiclePurposeList;
                case VehiclePurpose.SpecialOrder:
                    vehiclePurposeList.Add((int)VehicleXmlMovementType.SpecialOrder);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.SONotification);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.SOApplication);
                    return vehiclePurposeList;
                case VehiclePurpose.StgoMobileCrane:
                    vehiclePurposeList.Add((int)VehiclePurpose.StgoMobileCrane);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatA_Soa2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatA_Soa2_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatB_Soa2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatB_Soa5_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatC_Soa2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatC_Soa2_Pol2);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatC_Soa5_Pol2);
                    return vehiclePurposeList;
                case VehiclePurpose.Tracked:
                    vehiclePurposeList.Add((int)VehiclePurpose.Tracked);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.Tracked);
                    return vehiclePurposeList;
                case VehiclePurpose.WheeledConstructionAndUse:
                    vehiclePurposeList.Add((int)VehiclePurpose.WheeledConstructionAndUse);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.CandUNoNotification);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.CandUNotification_Pol2);
                    return vehiclePurposeList;
                case VehiclePurpose.VehicleSpecialOrder:
                    vehiclePurposeList.Add((int)VehiclePurpose.VehicleSpecialOrder);
                    vehiclePurposeList.Add((int)VehicleCategoryToMovementTypeMapping.VehicleSpecialOrder);
                    return vehiclePurposeList;
                default: return vehiclePurposeList;

            }


        }
        public static int GetVehicleIntentedUse(VehicleXmlMovementType movementType)
        {
            switch (movementType)
            {
                case VehicleXmlMovementType.VehicleSpecialOrder:
                    return (int)VehiclePurpose.VehicleSpecialOrder;
                case VehicleXmlMovementType.SONotification:
                case VehicleXmlMovementType.SOApplication:
                    return (int)VehiclePurpose.SpecialOrder;
                case VehicleXmlMovementType.StgoailCat1Vr1_Pol2:
                case VehicleXmlMovementType.StgoailCat1Vr1_Soa2_Pol2:
                case VehicleXmlMovementType.StgoTrackedCat1Notification_Pol2:
                case VehicleXmlMovementType.StgoTrackedCat1Notification_Soa2:
                case VehicleXmlMovementType.StgoTrackedCat1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoailCat1_Pol2:
                case VehicleXmlMovementType.StgoailCat1_Soa2:
                case VehicleXmlMovementType.StgoailCat1_Soa2_Pol2:
                case VehicleXmlMovementType.StgoTrackedCat2Notification_Soa2:
                case VehicleXmlMovementType.StgoTrackedCat2Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoailCat2Vr1_Soa2_Pol2:
                case VehicleXmlMovementType.StgoailCat2_Soa2:
                case VehicleXmlMovementType.StgoailCat2_Soa2_Pol2:
                case VehicleXmlMovementType.StgoTrackedCat3Notification_Soa2:
                case VehicleXmlMovementType.StgoTrackedCat3Notification_Soa5_Pol2:
                case VehicleXmlMovementType.StgoailCat3Vr1_Soa5_Pol2:
                case VehicleXmlMovementType.StgoailCat3_Soa2:
                case VehicleXmlMovementType.StgoailCat3_Soa5_Pol2:
                    return (int)VehiclePurpose.Stgoail;
                case VehicleXmlMovementType.StgoMobileCraneCatA_Soa2:
                case VehicleXmlMovementType.StgoMobileCraneCatA_Soa2_Pol2:
                case VehicleXmlMovementType.StgoMobileCraneCatB_Soa2:
                case VehicleXmlMovementType.StgoMobileCraneCatB_Soa2_Pol2:
                case VehicleXmlMovementType.StgoMobileCraneCatB_Soa5_Pol2:
                case VehicleXmlMovementType.StgoMobileCraneCatC_Soa2:
                case VehicleXmlMovementType.StgoMobileCraneCatC_Soa2_Pol2:
                case VehicleXmlMovementType.StgoMobileCraneCatC_Soa5_Pol2:
                    return (int)VehiclePurpose.StgoMobileCrane;
                case VehicleXmlMovementType.CandUNotification_Pol2:
                    return (int)VehiclePurpose.WheeledConstructionAndUse;
                case VehicleXmlMovementType.Tracked:
                    return (int)VehiclePurpose.Tracked;
                case VehicleXmlMovementType.StgoEngPlantWheeledCat1Notification_Pol2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat1Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat2Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat2Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat3Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat3Notification_Soa5_Pol2:
                    return (int)VehiclePurpose.StgoEngineeringPlantWheeled;
                case VehicleXmlMovementType.StgoEngPlantTrackedCat1Notification_Pol2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat1Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat2Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat2Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat3Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat3Notification_Soa5_Pol2:
                    return (int)VehiclePurpose.StgoEngineeringPlantTracked;
                case VehicleXmlMovementType.StgoRoadRecoveryCat1Notification_Pol2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat2Notification_Soa2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat2Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat3Notification_Soa2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat3Notification_Soa5_Pol2:
                    return (int)VehiclePurpose.StgoRoadRecovery;
                default:
                    return (int)movementType;

            }
        }

        public static int VehicleCategoryReverseMapping(int vehicleCategory)
        {
            switch (vehicleCategory)
            {
                case (int)VehicleCategoryToMovementTypeMapping.CandUNotification_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.CandUNoNotification:
                    return (int)VehicleCategoryToMovementTypeHighLevelMapping.WheeledConstructionAndUse;

                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat1_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat1_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat1_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat1Vr1_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat1Vr1_Soa2_Pol2:
                    return (int)VehicleCategoryToMovementTypeHighLevelMapping.StgoailCat1;

                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat2_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat2_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat2Vr1_Soa2_Pol2:
                    return (int)VehicleCategoryToMovementTypeHighLevelMapping.StgoailCat2;

                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat3_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat3_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat3_Soa5_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat3Vr1_Soa5_Pol2:
                    return (int)VehicleCategoryToMovementTypeHighLevelMapping.StgoailCat3;

                case (int)VehicleCategoryToMovementTypeMapping.VehicleSpecialOrder:
                    return (int)VehicleCategoryToMovementTypeHighLevelMapping.VehicleSpecialOrder;

                case (int)VehicleCategoryToMovementTypeMapping.SONotification:
                case (int)VehicleCategoryToMovementTypeMapping.SOApplication:
                case (int)VehicleCategoryToMovementTypeMapping.SpecialOrder:
                    return (int)VehicleCategoryToMovementTypeHighLevelMapping.SpecialOrder;

                case (int)VehicleCategoryToMovementTypeMapping.Tracked:
                case (int)VehicleCategoryToMovementTypeMapping.StgoTrackedCat1Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoTrackedCat2Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoTrackedCat2Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoTrackedCat3Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoTrackedCat3Notification_Soa5_Pol2:
                    return (int)VehicleCategoryToMovementTypeHighLevelMapping.Tracked;

                case (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatA_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatA_Soa2_Pol2:
                    return (int)VehicleCategoryToMovementTypeHighLevelMapping.StgoMobileCraneCata;

                case (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatB_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatB_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatB_Soa5_Pol2:
                    return (int)VehicleCategoryToMovementTypeHighLevelMapping.StgoMobileCraneCatb;

                case (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatC_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatC_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatC_Soa5_Pol2:
                    return (int)VehicleCategoryToMovementTypeHighLevelMapping.StgoMobileCraneCatc;

                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Notification_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat2Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat2Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat3Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat3Notification_Soa5_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Vr1Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Vr1Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Vr1Notification_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat2Vr1Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat2Vr1Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat3Vr1Notification_Soa5_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat3Vr1Notification_Soa2:
                    return (int)VehicleCategoryToMovementTypeHighLevelMapping.StgoEngineeringPlantTracked;

                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Notification_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat2Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat2Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat3Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat3Notification_Soa5_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Vr1Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Vr1Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Vr1Notification_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat2Vr1Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat2Vr1Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat3Vr1Notification_Soa5_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat3Vr1Notification_Soa2:
                    return (int)VehicleCategoryToMovementTypeHighLevelMapping.StgoEngineeringPlantWheeled;

                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Notification_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat2Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat2Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat3Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat3Notification_Soa5_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Vr1Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Vr1Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Vr1Notification_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat2Vr1Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat2Vr1Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat3Vr1Notification_Soa5_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat3Vr1Notification_Soa2:
                    return (int)VehicleCategoryToMovementTypeHighLevelMapping.StgoRoadRecoveryVehicle;
                default:
                    return (int)VehicleCategoryToMovementTypeHighLevelMapping.NoVehicleClassification;
            }
        }

        public static string VehicleCategoryFleetMapping(int vehicleCategory)
        {
            switch (vehicleCategory)
            {
                case (int)VehicleCategoryToMovementTypeMapping.CandUNotification_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.CandUNoNotification:
                    return "Wheeled construction and use";
                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat1_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat1_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat1_Soa2_Pol2:
                    return "STGO CAT 1 Notification";
                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat1Vr1_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat1Vr1_Soa2_Pol2:
                    return "STGO CAT 1 VR-1";
                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat2_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat2_Soa2_Pol2:
                    return "STGO CAT 2 Notification";
                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat2Vr1_Soa2_Pol2:
                    return "STGO CAT 2 VR-1";
                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat3_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat3_Soa5_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat3_Soa2_Pol2:
                    return "STGO CAT 3 Notification";
                case (int)VehicleCategoryToMovementTypeMapping.StgoailCat3Vr1_Soa5_Pol2:
                    return "STGO CAT 3 VR-1";
                case (int)VehicleCategoryToMovementTypeMapping.VehicleSpecialOrder:
                    return "VSO Notification";
                case (int)VehicleCategoryToMovementTypeMapping.SONotification:
                case (int)VehicleCategoryToMovementTypeMapping.SOApplication:
                case (int)VehicleCategoryToMovementTypeMapping.SpecialOrder:
                    return "Special Order";
                case (int)VehicleCategoryToMovementTypeMapping.Tracked:
                case (int)VehicleCategoryToMovementTypeMapping.StgoTrackedCat1Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoTrackedCat2Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoTrackedCat2Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoTrackedCat3Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoTrackedCat3Notification_Soa5_Pol2:
                    return "Tracked";
                case (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatA_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatA_Soa2_Pol2:
                    return "STGO Mobile Crane CAT A";
                case (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatB_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatB_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatB_Soa5_Pol2:
                    return "STGO Mobile Crane CAT B";
                case (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatC_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatC_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoMobileCraneCatC_Soa5_Pol2:
                    return "STGO Mobile Crane CAT C";
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Notification_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat2Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat2Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat3Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat3Notification_Soa5_Pol2:
                    return "STGO Engineering Plant - Tracked";
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Notification_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat2Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat2Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat3Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat3Notification_Soa5_Pol2:
                    return "STGO Engineering Plant - Wheeled";
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Notification_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat2Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat2Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat3Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat3Notification_Soa5_Pol2:
                    return "STGO Road Recovery";
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Vr1Notification_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Vr1Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Vr1Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat2Vr1Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat2Vr1Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat3Vr1Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat3Vr1Notification_Soa5_Pol2:
                    return "STGO VR-1 Engineering Plant - Tracked";
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Vr1Notification_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Vr1Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Vr1Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat2Vr1Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat2Vr1Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat3Vr1Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat3Vr1Notification_Soa5_Pol2:
                    return "STGO VR-1 Engineering Plant - Wheeled";
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Vr1Notification_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Vr1Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Vr1Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat2Vr1Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat2Vr1Notification_Soa2_Pol2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat3Vr1Notification_Soa2:
                case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat3Vr1Notification_Soa5_Pol2:
                    return "STGO VR-1 Road Recovery";
                default:
                    VehiclePurpose vehicleType = (VehiclePurpose)vehicleCategory;
                    return vehicleType.GetEnumDescription();
            }
        }

        public static int DefaultComponentSubType(int CompType)
        {
            switch (CompType)
            {
                case (int)ComponentType.BallastTractor:
                    return (int)ComponentSubType.BallastTractor;
                case (int)ComponentType.ConventionalTractor:
                    return (int)ComponentSubType.ConventionalTractor;
                case (int)ComponentType.MobileCrane:
                    return (int)ComponentSubType.MobileCrane;
                case (int)ComponentType.DrawbarTrailer:
                    return (int)ComponentSubType.DrawbarTrailer;
                case (int)ComponentType.RigidVehicle:
                    return (int)ComponentSubType.RigidVehicle;
                case (int)ComponentType.SemiTrailer:
                    return (int)ComponentSubType.SemiTrailer;
                case (int)ComponentType.SPMT:
                    return (int)ComponentSubType.SPMT;
                case (int)ComponentType.Tracked:
                    return (int)ComponentSubType.TrackedVehicle;
                case (int)ComponentType.EngineeringPlant:
                    return (int)ComponentSubType.EngPlantRigid;
                case (int)ComponentType.EngineeringPlantSemiTrailer:
                    return (int)ComponentSubType.EngineeringPlantSemiTrailer;
                case (int)ComponentType.EngineeringPlantDrawbarTrailer:
                    return (int)ComponentSubType.EngineeringPlantDrawbarTrailer;
                case (int)ComponentType.RecoveryVehicle:
                    return (int)ComponentSubType.RecoveryVehicle;
                case (int)ComponentType.GirderSet:
                    return (int)ComponentSubType.GirderSet;
                default:
                    return CompType;
            }
        }

        public static int DefaultComponentType(int CompType, int subCompType = 0, int componentCount = 0)
        {
            if (CompType == (int)ComponentType.BallastTractor)
                return (int)ComponentType.DrawbarTrailer;
            else if (CompType == (int)ComponentType.DrawbarTrailer && componentCount != 0)
                return (int)ComponentType.DrawbarTrailer;
            else if (CompType == (int)ComponentType.ConventionalTractor)
                return (int)ComponentType.SemiTrailer;
            else if (CompType == (int)ComponentType.EngineeringPlant && subCompType == (int)ComponentSubType.EngPlantConventionalTractor)
                return (int)ComponentType.EngineeringPlantSemiTrailer;
            else if (CompType == (int)ComponentType.EngineeringPlant && subCompType == (int)ComponentSubType.EngPlantBallastTractor)
                return (int)ComponentType.EngineeringPlantDrawbarTrailer;
            else if (CompType == (int)ComponentType.GirderSet)
                return (int)ComponentType.BallastTractor;
            else if (CompType == (int)ComponentType.SemiTrailer && componentCount != 0)
                return (int)ComponentType.SemiTrailer;
            else if (CompType == (int)ComponentType.EngineeringPlantSemiTrailer && componentCount != 0)
                return (int)ComponentType.EngineeringPlantSemiTrailer;
            else if (CompType == (int)ComponentType.EngineeringPlantDrawbarTrailer && componentCount != 0)
                return (int)ComponentType.EngineeringPlantDrawbarTrailer;
            else return 0;
        }

        public static int DefaultComponentTypeForMoreThanTwo(int CompType, int subCompType = 0)
        {
            if (CompType == (int)ComponentType.DrawbarTrailer || CompType == (int)ComponentType.RigidVehicle)
                return (int)ComponentType.DrawbarTrailer;
            else if (CompType == (int)ComponentType.ConventionalTractor)
                return (int)ComponentType.SemiTrailer;
            else if (CompType == (int)ComponentType.GirderSet)
                return (int)ComponentType.BallastTractor;
            else if (CompType == (int)ComponentType.BallastTractor)
                return (int)ComponentType.DrawbarTrailer;
            else if (CompType == (int)ComponentType.SemiTrailer)
                return (int)ComponentType.SemiTrailer;
            else if (CompType == (int)ComponentType.SPMT)
                return (int)ComponentType.SPMT;
            else if (CompType == (int)ComponentType.EngineeringPlant && subCompType == (int)ComponentSubType.EngPlantConventionalTractor)
                return (int)ComponentType.EngineeringPlantSemiTrailer;
            else if (CompType == (int)ComponentType.EngineeringPlant && subCompType == (int)ComponentSubType.EngPlantBallastTractor)
                return (int)ComponentType.EngineeringPlantDrawbarTrailer;
            else if (CompType == (int)ComponentType.EngineeringPlantSemiTrailer)
                return (int)ComponentType.EngineeringPlantSemiTrailer;
            else if (CompType == (int)ComponentType.EngineeringPlantDrawbarTrailer)
                return (int)ComponentType.EngineeringPlantDrawbarTrailer;
            else return 0;
        }

        public static List<System.Web.Mvc.SelectListItem> GetComponentTypes(int CompTypeId, int SubCompTypeId, System.Web.Mvc.SelectList ComponentTypeList, bool isRigidVehicle, bool isRecoveryVehicle)
        {
            var ComponentTypeListFiltered = new List<System.Web.Mvc.SelectListItem>();
            if (CompTypeId == (int)ComponentType.BallastTractor || CompTypeId == (int)ComponentType.DrawbarTrailer || CompTypeId == (int)ComponentType.GirderSet)
            {
                if (isRecoveryVehicle)
                {
                    if (CompTypeId == (int)ComponentType.BallastTractor)
                    {
                        ComponentTypeListFiltered = ComponentTypeList
                            .Where(x => Convert.ToInt32(x.Value) == (int)ComponentType.BallastTractor).ToList();
                    }
                    else
                    {
                        ComponentTypeListFiltered = ComponentTypeList
                            .Where(x => Convert.ToInt32(x.Value) == (int)ComponentType.DrawbarTrailer ||
                            Convert.ToInt32(x.Value) == (int)ComponentType.GirderSet ||
                            Convert.ToInt32(x.Value) == (int)ComponentType.BallastTractor).ToList();
                    }
                }
                else
                {
                    if (isRigidVehicle)
                    {
                        ComponentTypeListFiltered = ComponentTypeList
                        .Where(x => Convert.ToInt32(x.Value) == (int)ComponentType.DrawbarTrailer).ToList();
                    }
                    else
                    {
                        ComponentTypeListFiltered = ComponentTypeList
                        .Where(x => Convert.ToInt32(x.Value) == (int)ComponentType.DrawbarTrailer ||
                        Convert.ToInt32(x.Value) == (int)ComponentType.GirderSet ||
                        Convert.ToInt32(x.Value) == (int)ComponentType.BallastTractor).ToList();
                    }
                }

            }
            else if (CompTypeId == (int)ComponentType.ConventionalTractor || CompTypeId == (int)ComponentType.SemiTrailer)
            {
                if (isRecoveryVehicle)
                {
                    if (CompTypeId == (int)ComponentType.ConventionalTractor)
                    {
                        ComponentTypeListFiltered = ComponentTypeList
                            .Where(x => Convert.ToInt32(x.Value) == (int)ComponentType.ConventionalTractor).ToList();
                    }
                    else
                    {
                        ComponentTypeListFiltered = ComponentTypeList
                    .Where(x => Convert.ToInt32(x.Value) == (int)ComponentType.SemiTrailer).ToList();
                    }

                }
                else
                {
                    ComponentTypeListFiltered = ComponentTypeList
                    .Where(x => Convert.ToInt32(x.Value) == (int)ComponentType.SemiTrailer).ToList();
                }
            }
            else if (CompTypeId == (int)ComponentType.RigidVehicle)
            {
                if (isRecoveryVehicle)
                {
                    ComponentTypeListFiltered = ComponentTypeList
                    .Where(x => Convert.ToInt32(x.Value) == (int)ComponentType.DrawbarTrailer ||
                    Convert.ToInt32(x.Value) == (int)ComponentType.RigidVehicle).ToList();
                }
                else
                {
                    ComponentTypeListFiltered = ComponentTypeList
                    .Where(x => Convert.ToInt32(x.Value) == (int)ComponentType.DrawbarTrailer).ToList();
                }
            }
            else if (CompTypeId == (int)ComponentType.SPMT)
            {
                ComponentTypeListFiltered = ComponentTypeList
                    .Where(x => Convert.ToInt32(x.Value) == (int)ComponentType.SPMT).ToList();
            }
            else if (!isRecoveryVehicle && (CompTypeId == (int)ComponentType.EngineeringPlant &&
                SubCompTypeId == (int)ComponentSubType.EngPlantConventionalTractor) || (
                CompTypeId == (int)ComponentType.EngineeringPlantSemiTrailer))
            {
                ComponentTypeListFiltered = ComponentTypeList
                    .Where(x => Convert.ToInt32(x.Value) == (int)ComponentType.EngineeringPlantSemiTrailer).ToList();
            }
            else if (!isRecoveryVehicle && (CompTypeId == (int)ComponentType.EngineeringPlant &&
               SubCompTypeId == (int)ComponentSubType.EngPlantBallastTractor) || (
                CompTypeId == (int)ComponentType.EngineeringPlantDrawbarTrailer))
            {
                ComponentTypeListFiltered = ComponentTypeList
                    .Where(x => Convert.ToInt32(x.Value) == (int)ComponentType.EngineeringPlantDrawbarTrailer).ToList();
            }
            else
            {
                ComponentTypeListFiltered = ComponentTypeList.ToList();
            }
            return ComponentTypeListFiltered;
        }

        public static int MovementTypeHighLevelMapping(VehicleXmlMovementType movementType)
        {
            switch (movementType)
            {
                case VehicleXmlMovementType.VehicleSpecialOrder:
                case VehicleXmlMovementType.VSONotification:
                    return (int)VehicleMovementTypeMain.VehicleSpecialOrder;

                case VehicleXmlMovementType.SONotification:
                case VehicleXmlMovementType.SOApplication:
                case VehicleXmlMovementType.SpecialOrder:
                    return (int)VehicleMovementTypeMain.SpecialOrder;

                case VehicleXmlMovementType.StgoailCat1Vr1_Pol2:
                case VehicleXmlMovementType.StgoailCat1Vr1_Soa2_Pol2:
                case VehicleXmlMovementType.StgoailCat2Vr1_Soa2_Pol2:
                case VehicleXmlMovementType.StgoailCat3Vr1_Soa5_Pol2:
                    return (int)VehicleMovementTypeMain.Stgovr1;

                case VehicleXmlMovementType.StgoEngPlantTrackedCat1Notification_Pol2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat1Notification_Pol2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat1Notification_Pol2:
                case VehicleXmlMovementType.StgoTrackedCat1Notification_Pol2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat1Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat1Notification_Soa2:
                case VehicleXmlMovementType.StgoTrackedCat1Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoTrackedCat1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoailCat1_Pol2:
                case VehicleXmlMovementType.StgoailCat1_Soa2:
                case VehicleXmlMovementType.StgoailCat1_Soa2_Pol2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat2Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat2Notification_Soa2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat2Notification_Soa2:
                case VehicleXmlMovementType.StgoTrackedCat2Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat2Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat2Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat2Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoTrackedCat2Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoailCat2_Soa2:
                case VehicleXmlMovementType.StgoailCat2_Soa2_Pol2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat3Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat3Notification_Soa2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat3Notification_Soa2:
                case VehicleXmlMovementType.StgoTrackedCat3Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat3Notification_Soa5_Pol2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat3Notification_Soa5_Pol2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat3Notification_Soa5_Pol2:
                case VehicleXmlMovementType.StgoTrackedCat3Notification_Soa5_Pol2:
                case VehicleXmlMovementType.StgoailCat3_Soa2:
                case VehicleXmlMovementType.StgoailCat3_Soa5_Pol2:
                case VehicleXmlMovementType.StgoailCat3_Soa2_Pol2:
                case VehicleXmlMovementType.StgoailCat1:
                case VehicleXmlMovementType.StgoailCat2:
                case VehicleXmlMovementType.StgoailCat3:
                    return (int)VehicleMovementTypeMain.Stgo;

                case VehicleXmlMovementType.StgoMobileCraneCatA_Soa2:
                case VehicleXmlMovementType.StgoMobileCraneCatA_Soa2_Pol2:
                case VehicleXmlMovementType.StgoMobileCraneCatB_Soa2:
                case VehicleXmlMovementType.StgoMobileCraneCatB_Soa2_Pol2:
                case VehicleXmlMovementType.StgoMobileCraneCatB_Soa5_Pol2:
                case VehicleXmlMovementType.StgoMobileCraneCatC_Soa2:
                case VehicleXmlMovementType.StgoMobileCraneCatC_Soa2_Pol2:
                case VehicleXmlMovementType.StgoMobileCraneCatC_Soa5_Pol2:
                    return (int)VehicleMovementTypeMain.StgoMobileCrane;

                case VehicleXmlMovementType.CandUNotification_Pol2:
                case VehicleXmlMovementType.CandUNoNotification:
                    return (int)VehicleMovementTypeMain.WheeledConstructionAndUse;

                case VehicleXmlMovementType.Tracked:
                    return (int)VehicleMovementTypeMain.Tracked;

                case VehicleXmlMovementType.NoNotification:
                    return 0;
                default: return (int)movementType;
            }
        }

        public static int VehicleMovementTypeMapping(VehicleXmlMovementType movementType)
        {
            switch (movementType)
            {
                case VehicleXmlMovementType.VehicleSpecialOrder:
                case VehicleXmlMovementType.VSONotification:
                    return (int)VehicleXmlMovementType.VehicleSpecialOrder;
                case VehicleXmlMovementType.SONotification:
                    return (int)VehicleXmlMovementType.SpecialOrder;
                case VehicleXmlMovementType.SOApplication:
                    return (int)VehicleXmlMovementType.SpecialOrder;
                case VehicleXmlMovementType.StgoailCat1Vr1_Pol2:
                case VehicleXmlMovementType.StgoailCat1Vr1_Soa2_Pol2:
                case VehicleXmlMovementType.StgoTrackedCat1Notification_Pol2:
                case VehicleXmlMovementType.StgoTrackedCat1Notification_Soa2:
                case VehicleXmlMovementType.StgoTrackedCat1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoailCat1_Pol2:
                case VehicleXmlMovementType.StgoailCat1_Soa2:
                case VehicleXmlMovementType.StgoailCat1_Soa2_Pol2:
                    return (int)VehicleXmlMovementType.StgoailCat1;
                case VehicleXmlMovementType.StgoTrackedCat2Notification_Soa2:
                case VehicleXmlMovementType.StgoTrackedCat2Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoailCat2Vr1_Soa2_Pol2:
                case VehicleXmlMovementType.StgoailCat2_Soa2:
                case VehicleXmlMovementType.StgoailCat2_Soa2_Pol2:
                    return (int)VehicleXmlMovementType.StgoailCat2;
                case VehicleXmlMovementType.StgoTrackedCat3Notification_Soa2:
                case VehicleXmlMovementType.StgoTrackedCat3Notification_Soa5_Pol2:
                case VehicleXmlMovementType.StgoailCat3Vr1_Soa5_Pol2:
                case VehicleXmlMovementType.StgoailCat3_Soa2:
                case VehicleXmlMovementType.StgoailCat3_Soa5_Pol2:
                case VehicleXmlMovementType.StgoailCat3_Soa2_Pol2:
                    return (int)VehicleXmlMovementType.StgoailCat3;
                case VehicleXmlMovementType.StgoMobileCraneCatA_Soa2:
                case VehicleXmlMovementType.StgoMobileCraneCatA_Soa2_Pol2:
                    return (int)VehicleXmlMovementType.StgoMobileCraneCata;
                case VehicleXmlMovementType.StgoMobileCraneCatB_Soa2:
                case VehicleXmlMovementType.StgoMobileCraneCatB_Soa2_Pol2:
                case VehicleXmlMovementType.StgoMobileCraneCatB_Soa5_Pol2:
                    return (int)VehicleXmlMovementType.StgoMobileCraneCatb;
                case VehicleXmlMovementType.StgoMobileCraneCatC_Soa2:
                case VehicleXmlMovementType.StgoMobileCraneCatC_Soa2_Pol2:
                case VehicleXmlMovementType.StgoMobileCraneCatC_Soa5_Pol2:
                    return (int)VehicleXmlMovementType.StgoMobileCraneCatc;
                case VehicleXmlMovementType.CandUNotification_Pol2:
                case VehicleXmlMovementType.CandUNoNotification:
                    return (int)VehicleXmlMovementType.WheeledConstructionAndUse;
                case VehicleXmlMovementType.Tracked:
                    return (int)VehicleXmlMovementType.Tracked;
                case VehicleXmlMovementType.StgoEngPlantTrackedCat1Notification_Pol2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat1Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat2Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat2Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat3Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat3Notification_Soa5_Pol2:
                case VehicleXmlMovementType.StgoEngineeringPlantTracked:
                    return (int)VehicleXmlMovementType.StgoEngineeringPlantTracked;
                case VehicleXmlMovementType.StgoEngPlantWheeledCat1Notification_Pol2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat1Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat2Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat2Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat3Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat3Notification_Soa5_Pol2:
                case VehicleXmlMovementType.StgoEngineeringPlantWheeled:
                    return (int)VehicleXmlMovementType.StgoEngineeringPlantWheeled;
                case VehicleXmlMovementType.StgoRoadRecoveryCat1Notification_Pol2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat2Notification_Soa2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat2Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat3Notification_Soa2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat3Notification_Soa5_Pol2:
                case VehicleXmlMovementType.StgoRoadRecoveryVehicle:
                    return (int)VehicleXmlMovementType.StgoRoadRecoveryVehicle;
                case VehicleXmlMovementType.StgoEngPlantTrackedCat1Vr1Notification_Pol2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat1Vr1Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat1Vr1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat2Vr1Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat2Vr1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat3Vr1Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantTrackedCat3Vr1Notification_Soa5_Pol2:
                case VehicleXmlMovementType.StgoVr1EngineeringPlantTracked:
                    return (int)VehicleXmlMovementType.StgoVr1EngineeringPlantWheeled;
                case VehicleXmlMovementType.StgoEngPlantWheeledCat1Vr1Notification_Pol2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat1Vr1Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat1Vr1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat2Vr1Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat2Vr1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat3Vr1Notification_Soa2:
                case VehicleXmlMovementType.StgoEngPlantWheeledCat3Vr1Notification_Soa5_Pol2:
                case VehicleXmlMovementType.StgoVr1EngineeringPlantWheeled:
                    return (int)VehicleXmlMovementType.StgoVr1EngineeringPlantWheeled;
                case VehicleXmlMovementType.StgoRoadRecoveryCat1Vr1Notification_Pol2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat1Vr1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat2Vr1Notification_Soa2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat2Vr1Notification_Soa2_Pol2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat3Vr1Notification_Soa2:
                case VehicleXmlMovementType.StgoRoadRecoveryCat3Vr1Notification_Soa5_Pol2:
                case VehicleXmlMovementType.StgoVr1RoadRecoveryVehicle:
                    return (int)VehicleXmlMovementType.StgoVr1RoadRecoveryVehicle;
                case VehicleXmlMovementType.NoNotification:
                    return 0;
                default: return (int)movementType;

            }
        }

        public static int VehicleClassMappingByLeadingComponent(int componentSubType, int vehicleClass)
        {
            switch (componentSubType)
            {
                case (int)ComponentSubType.EngPlantConventionalTractor:
                case (int)ComponentSubType.EngPlantRigid:
                case (int)ComponentSubType.EngPlantBallastTractor:
                case (int)ComponentSubType.EngineeringPlantSemiTrailer:
                case (int)ComponentSubType.EngineeringPlantDrawbarTrailer:
                    switch (vehicleClass)
                    {
                        case (int)VehicleClassificationType.StgoailCat1:
                            return (int)VehicleClassificationType.StgoCat1EngineeringPlantWheeled;
                        case (int)VehicleClassificationType.StgoailCat2:
                            return (int)VehicleClassificationType.StgoCat2EngineeringPlantWheeled;
                        case (int)VehicleClassificationType.StgoailCat3:
                            return (int)VehicleClassificationType.StgoCat3EngineeringPlantWheeled;
                    }
                    break;
                case (int)ComponentSubType.EngPlantTracked:
                    switch (vehicleClass)
                    {
                        case (int)VehicleClassificationType.StgoailCat1:
                            return (int)VehicleClassificationType.StgoCat1EngineeringPlantTracked;
                        case (int)VehicleClassificationType.StgoailCat2:
                            return (int)VehicleClassificationType.StgoCat2EngineeringPlantTracked;
                        case (int)VehicleClassificationType.StgoailCat3:
                            return (int)VehicleClassificationType.StgoCat3EngineeringPlantTracked;
                    }
                    break;
                case (int)ComponentSubType.RecoveryVehicle:
                    switch (vehicleClass)
                    {
                        case (int)VehicleClassificationType.StgoailCat1:
                            return (int)VehicleClassificationType.StgoCat1RoadRecovery;
                        case (int)VehicleClassificationType.StgoailCat2:
                            return (int)VehicleClassificationType.StgoCat2RoadRecovery;
                        case (int)VehicleClassificationType.StgoailCat3:
                            return (int)VehicleClassificationType.StgoCat3RoadRecovery;
                    }
                    break;
            }
            return vehicleClass;
        }

        public static int VehicleClassMappingByPurpose(int movementClassification, int vehiclePurpose, int vehicleClass)
        {
            switch (movementClassification)
            {
                case (int)VehicleXmlMovementType.StgoEngineeringPlantWheeled:
                case (int)VehicleXmlMovementType.StgoVr1EngineeringPlantWheeled:
                    switch (vehiclePurpose)
                    {
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Vr1Notification_Soa2_Pol2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Vr1Notification_Soa2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Vr1Notification_Pol2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Notification_Soa2_Pol2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Notification_Soa2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat1Notification_Pol2:
                            return (int)VehicleClassificationType.StgoCat1EngineeringPlantWheeled;
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat2Vr1Notification_Soa2_Pol2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat2Vr1Notification_Soa2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat2Notification_Soa2_Pol2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat2Notification_Soa2:
                            return (int)VehicleClassificationType.StgoCat2EngineeringPlantWheeled;
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat3Vr1Notification_Soa5_Pol2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat3Vr1Notification_Soa2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat3Notification_Soa5_Pol2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantWheeledCat3Notification_Soa2:
                            return (int)VehicleClassificationType.StgoCat3EngineeringPlantWheeled;
                    }
                    break;
                case (int)VehicleXmlMovementType.StgoEngineeringPlantTracked:
                case (int)VehicleXmlMovementType.StgoVr1EngineeringPlantTracked:
                    switch (vehiclePurpose)
                    {
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Vr1Notification_Soa2_Pol2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Vr1Notification_Soa2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Vr1Notification_Pol2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Notification_Soa2_Pol2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Notification_Soa2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat1Notification_Pol2:
                            return (int)VehicleClassificationType.StgoCat1EngineeringPlantTracked;
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat2Vr1Notification_Soa2_Pol2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat2Vr1Notification_Soa2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat2Notification_Soa2_Pol2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat2Notification_Soa2:
                            return (int)VehicleClassificationType.StgoCat2EngineeringPlantTracked;
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat3Vr1Notification_Soa5_Pol2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat3Vr1Notification_Soa2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat3Notification_Soa5_Pol2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoEngPlantTrackedCat3Notification_Soa2:
                            return (int)VehicleClassificationType.StgoCat3EngineeringPlantTracked;
                    }
                    break;
                case (int)VehicleXmlMovementType.StgoRoadRecoveryVehicle:
                case (int)VehicleXmlMovementType.StgoVr1RoadRecoveryVehicle:
                    switch (vehiclePurpose)
                    {
                        case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Vr1Notification_Soa2_Pol2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Vr1Notification_Soa2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Vr1Notification_Pol2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Notification_Soa2_Pol2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Notification_Soa2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat1Notification_Pol2:
                            return (int)VehicleClassificationType.StgoCat1RoadRecovery;
                        case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat2Vr1Notification_Soa2_Pol2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat2Vr1Notification_Soa2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat2Notification_Soa2_Pol2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat2Notification_Soa2:
                            return (int)VehicleClassificationType.StgoCat2RoadRecovery;
                        case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat3Vr1Notification_Soa5_Pol2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat3Vr1Notification_Soa2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat3Notification_Soa5_Pol2:
                        case (int)VehicleCategoryToMovementTypeMapping.StgoRoadRecoveryCat3Notification_Soa2:
                            return (int)VehicleClassificationType.StgoCat3RoadRecovery;
                    }
                    break;
            }
            return vehicleClass;
        }
    }
}
