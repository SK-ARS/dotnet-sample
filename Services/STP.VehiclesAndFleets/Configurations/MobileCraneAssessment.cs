using STP.Common.Enums;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static STP.Domain.VehiclesAndFleets.VehicleEnums;

namespace STP.VehiclesAndFleets.Configurations
{
    public static class MobileCraneAssessment
    {
        public static VehicleMovementType AssessMobileCraneLogic(ConfigurationModel configuration, bool forceApplication = false, VehicleMovementType previousMovementType = null)
        {
            VehicleMovementType movementType = null;

            int gSOANoticePeriod = previousMovementType != null ? previousMovementType.SOANoticePeriod : 0;
            int gPoliceNoticePeriod = previousMovementType != null ? previousMovementType.PoliceNoticePeriod : 0;

            bool oversize = false;
            if(configuration.Width > 3 
                || configuration.NotifLeftOverhang > 0.305 || configuration.NotifRightOverhang > 0.305
                || configuration.NotifFrontOverhang > 3.05 || configuration.NotifRearOverhang > 3.05
                || configuration.RigidLength > 18.75)
            {
                oversize = true;
            }

            if (configuration.AxleCount < 5
                && configuration.MaxAxleWeight <= 11500
                && (configuration.GrossWeight <= ((9 * configuration.AxleCount) + (4 - configuration.AxleCount) + (2 * configuration.AxleCount % 2)) * 1000)
                && (configuration.WheelBase >= (1.5 * configuration.AxleCount + 0.5 * (configuration.AxleCount % 2)))
                && !oversize)
            {
                gSOANoticePeriod = gSOANoticePeriod > 2 ? gSOANoticePeriod : 2;
                return movementType = new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.StgoMobileCraneCata,
                    MovementType = (int)MovementType.notification,
                    SOANoticePeriod = gSOANoticePeriod,
                    PoliceNoticePeriod = gPoliceNoticePeriod,
                    Message = "STGO CAT A (SOA Notification - 2 clear working days)"
                };
            }
            else if (configuration.AxleCount < 5
                && configuration.MaxAxleWeight <= 11500
                && (configuration.GrossWeight <= ((9 * configuration.AxleCount) + (4 - configuration.AxleCount) + (2 * configuration.AxleCount % 2)) * 1000)
                && (configuration.WheelBase >= 1.5 * configuration.AxleCount + 0.5 * (configuration.AxleCount % 2))
                && oversize)
            {
                gSOANoticePeriod = gSOANoticePeriod > 2 ? gSOANoticePeriod : 2;
                gPoliceNoticePeriod = gPoliceNoticePeriod > 2 ? gPoliceNoticePeriod : 2;
                return movementType = new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.StgoMobileCraneCata,
                    MovementType = (int)MovementType.notification,
                    SOANoticePeriod = gSOANoticePeriod,
                    PoliceNoticePeriod = gPoliceNoticePeriod,
                    Message = "STGO CAT A (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)"
                };
            }
            else if (configuration.MaxAxleWeight <= 12500
                && configuration.GrossWeight <= 12500 * configuration.AxleCount
                && configuration.GrossWeight <= 80000
                && !oversize)
            {
                gSOANoticePeriod = gSOANoticePeriod > 2 ? gSOANoticePeriod : 2;
                return movementType = new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.StgoMobileCraneCatb,
                    MovementType = (int)MovementType.notification,
                    SOANoticePeriod = gSOANoticePeriod,
                    PoliceNoticePeriod = gPoliceNoticePeriod,
                    Message = "STGO CAT B (SOA Notification - 2 clear working days)"
                };
            }
            else if (configuration.MaxAxleWeight <= 12500
                && configuration.GrossWeight <= 12500 * configuration.AxleCount
                && configuration.GrossWeight <= 80000
                && oversize)
            {
                gSOANoticePeriod = gSOANoticePeriod > 2 ? gSOANoticePeriod : 2;
                gPoliceNoticePeriod = gPoliceNoticePeriod > 2 ? gPoliceNoticePeriod : 2;
                return movementType = new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.StgoMobileCraneCatb,
                    MovementType = (int)MovementType.notification,
                    SOANoticePeriod = gSOANoticePeriod,
                    PoliceNoticePeriod = gPoliceNoticePeriod,
                    Message = "STGO CAT B (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)"
                };
            }
            else if (configuration.MaxAxleWeight <= 12500
                && configuration.GrossWeight <= 12500 * configuration.AxleCount
                && (configuration.GrossWeight > 80000 && configuration.GrossWeight <= 150000))
            {
                gSOANoticePeriod = gSOANoticePeriod > 5 ? gSOANoticePeriod : 5;
                gPoliceNoticePeriod = gPoliceNoticePeriod > 2 ? gPoliceNoticePeriod : 2;
                return movementType = new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.StgoMobileCraneCatb,
                    MovementType = (int)MovementType.notification,
                    SOANoticePeriod = gSOANoticePeriod,
                    PoliceNoticePeriod = gPoliceNoticePeriod,
                    Message = "STGO CAT B (SOA Notification - 5 clear working days, Police Notification - 2 clear working days)"
                };
            }
            else if (configuration.MaxAxleWeight <= 16500
                && configuration.GrossWeight <= 80000
                && !oversize)
            {
                gSOANoticePeriod = gSOANoticePeriod > 2 ? gSOANoticePeriod : 2;
                return movementType = new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.StgoMobileCraneCatc,
                    MovementType = (int)MovementType.notification,
                    SOANoticePeriod = gSOANoticePeriod,
                    PoliceNoticePeriod = gPoliceNoticePeriod,
                    Message = "STGO CAT C (SOA Notification - 2 clear working days)"
                };
            }
            else if (configuration.MaxAxleWeight <= 16500
                && configuration.GrossWeight <= 80000
                && oversize)
            {
                gSOANoticePeriod = gSOANoticePeriod > 2 ? gSOANoticePeriod : 2;
                gPoliceNoticePeriod = gPoliceNoticePeriod > 2 ? gPoliceNoticePeriod : 2;
                return movementType = new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.StgoMobileCraneCatc,
                    MovementType = (int)MovementType.notification,
                    SOANoticePeriod = gSOANoticePeriod,
                    PoliceNoticePeriod = gPoliceNoticePeriod,
                    Message = "STGO CAT C (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)"
                };
            }
            else if (configuration.MaxAxleWeight <= 16500
               && configuration.GrossWeight <= 150000 && configuration.GrossWeight > 80000)
            {
                gSOANoticePeriod = gSOANoticePeriod > 5 ? gSOANoticePeriod : 5;
                gPoliceNoticePeriod = gPoliceNoticePeriod > 2 ? gPoliceNoticePeriod : 2;
                return movementType = new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.StgoMobileCraneCatc,
                    MovementType = (int)MovementType.notification,
                    SOANoticePeriod = gSOANoticePeriod,
                    PoliceNoticePeriod = gPoliceNoticePeriod,
                    Message = "STGO CAT C (SOA Notification - 5 clear working days, Police Notification - 2 clear working days)"
                };
            }
            else
            {
                return movementType = new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.SpecialOrder,
                    MovementType = (int)MovementType.special_order,
                    SOANoticePeriod = gSOANoticePeriod,
                    PoliceNoticePeriod = gPoliceNoticePeriod,
                    Message = "Apply for SO."
                };
            }
        }
    }
}