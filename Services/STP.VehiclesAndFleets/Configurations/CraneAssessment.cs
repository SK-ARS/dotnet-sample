using STP.Common.Enums;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static STP.Domain.VehiclesAndFleets.VehicleEnums;

namespace STP.VehiclesAndFleets.Configurations
{
    public class CraneAssessment
    {
        private enum AssessClass
        {
            CraneSO,
            CatA,
            CatB,
            CatC,
            NoCrane
        }

        private static bool isNoCrane(ConfigurationModel configuration)
        {
            return (configuration.GrossWeight <= 12000);
        }

        private static bool isCatA(ConfigurationModel configuration)
        {
            return (configuration.AxleCount < 5 &&
                configuration.MaxAxleWeight <= 11500 &&
                (configuration.GrossWeight <= ((9 * configuration.AxleCount) + (4 - configuration.AxleCount) + (2 * (configuration.AxleCount % 2))) * 1000) &&
                (configuration.WheelBase >= 1.5 * configuration.AxleCount + 0.5 * (configuration.AxleCount % 2)));
        }

        private static bool isCatB(ConfigurationModel configuration)
        {
            return (configuration.MaxAxleWeight <= 12500 && configuration.GrossWeight <= 12500 * configuration.AxleCount && configuration.GrossWeight <= 150000);
        }

        private static bool oversize(ConfigurationModel configuration)
        {
            var Width = configuration.Width != null ? Math.Round(configuration.Width.Value, 2) : 0;
            var NotifLeftOverhang = configuration.NotifLeftOverhang != null ? Math.Round(configuration.NotifLeftOverhang.Value, 3) : 0;
            var NotifRightOverhang = configuration.NotifRightOverhang != null ? Math.Round(configuration.NotifRightOverhang.Value, 3) : 0;
            var NotifFrontOverhang = configuration.NotifFrontOverhang != null ? Math.Round(configuration.NotifFrontOverhang.Value, 2) : 0;
            var NotifRearOverhang = configuration.NotifRearOverhang != null ? Math.Round(configuration.NotifRearOverhang.Value, 2) : 0;
            var RigidLength = configuration.RigidLength != null ? Math.Round(configuration.RigidLength.Value, 2) : 0;
            return (Width > 3
                || NotifLeftOverhang > 0.305 || NotifRightOverhang > 0.305
                || NotifFrontOverhang > 3.05 || NotifRearOverhang > 3.05
                || RigidLength > 18.75);
        }

        private static bool isCatC(ConfigurationModel configuration)
        {
            return (configuration.MaxAxleWeight <= 16500 && configuration.GrossWeight <= 150000);
        }

        private static VehicleMovementType NoCrane()
        {
            return new VehicleMovementType()
            {
                VehicleClass = (int)VehicleClassificationType.NoCrane,
                MovementType = (int)MovementType.no_movement,
                Message = "Vehicle is not classified as a crane run Rigid Vehicle checks."
            };
        }

        private static VehicleMovementType CatA(ConfigurationModel configuration)
        {
            if (oversize(configuration))
            {
                return new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.StgoMobileCraneCata,
                    MovementType = (int)MovementType.notification,
                    SOANoticePeriod = 2,
                    PoliceNoticePeriod = 2,
                    Message = "STGO CAT A (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)"
                };
            }
            else
            {
                return new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.StgoMobileCraneCata,
                    MovementType = (int)MovementType.notification,
                    SOANoticePeriod = 2,
                    PoliceNoticePeriod = 0,
                    Message = "STGO CAT A (SOA Notification - 2 clear working days)"
                };
            }
        }

        private static VehicleMovementType CatB(ConfigurationModel configuration)
        {
            if (configuration.GrossWeight > 80000 && configuration.GrossWeight <= 150000)
            {
                return new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.StgoMobileCraneCatb,
                    MovementType = (int)MovementType.notification,
                    SOANoticePeriod = 5,
                    PoliceNoticePeriod = 2,
                    Message = "STGO CAT B (SOA Notification - 5 clear working days, Police Notification - 2 clear working days)"
                };
            }
            if (oversize(configuration))
            {
                return new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.StgoMobileCraneCatb,
                    MovementType = (int)MovementType.notification,
                    SOANoticePeriod = 2,
                    PoliceNoticePeriod = 2,
                    Message = "STGO CAT B (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)"
                };
            }
            else
            {
                return new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.StgoMobileCraneCatb,
                    MovementType = (int)MovementType.notification,
                    SOANoticePeriod = 2,
                    PoliceNoticePeriod = 0,
                    Message = "STGO CAT B (SOA Notification - 2 clear working days)"
                };
            }
        }

        private static VehicleMovementType CatC(ConfigurationModel configuration)
        {
            if (configuration.GrossWeight > 80000 && configuration.GrossWeight <= 150000)
            {
                return new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.StgoMobileCraneCatc,
                    MovementType = (int)MovementType.notification,
                    SOANoticePeriod = 5,
                    PoliceNoticePeriod = 2,
                    Message = "STGO CAT C (SOA Notification - 5 clear working days, Police Notification - 2 clear working days)"
                };
            }
            if (oversize(configuration))
            {
                return new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.StgoMobileCraneCatc,
                    MovementType = (int)MovementType.notification,
                    SOANoticePeriod = 2,
                    PoliceNoticePeriod = 2,
                    Message = "STGO CAT C (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)"
                };
            }
            else
            {
                return new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.StgoMobileCraneCatc,
                    MovementType = (int)MovementType.notification,
                    SOANoticePeriod = 2,
                    PoliceNoticePeriod = 0,
                    Message = "STGO CAT C (SOA Notification - 2 clear working days)"
                };
            }
        }

        private static VehicleMovementType CraneSO(ConfigurationModel configuration)
        {
            return new VehicleMovementType()
            {
                VehicleClass = (int)VehicleClassificationType.SpecialOrder,
                MovementType = (int)MovementType.special_order,
                SOANoticePeriod = 5,
                PoliceNoticePeriod = 2,
                Message = "Apply for SO."
            };
        }

        public static VehicleMovementType AssessMobileCraneLogic(ConfigurationModel configuration, bool forceApplication = false, VehicleMovementType previousMovementType = null)
        {
            VehicleMovementType movementType = null;

            //Check to see which class is found. Multiplied by the priority. Where more than one class
            //is determined, the highest priority class is selected
            AssessClass foundClass = AssessClass.CraneSO;
            if (isCatC(configuration))
            {
                foundClass = AssessClass.CatC;
            }
            if (isCatB(configuration))
            {
                foundClass = AssessClass.CatB;
            }
            if (isCatA(configuration))
            {
                foundClass = AssessClass.CatA;
            }
            if (isNoCrane(configuration))
            {
                foundClass = AssessClass.NoCrane;
            }

            //Call the correct message based on the found class
            switch (foundClass)
            {
                case AssessClass.CraneSO:
                    movementType = CraneSO(configuration);
                    break;
                case AssessClass.NoCrane:
                    movementType = NoCrane();
                    break;
                case AssessClass.CatA:
                    movementType = CatA(configuration);
                    break;
                case AssessClass.CatB:
                    movementType = CatB(configuration);
                    break;
                case AssessClass.CatC:
                    movementType = CatC(configuration);
                    break;
            }

            return movementType;
        }
    }
}