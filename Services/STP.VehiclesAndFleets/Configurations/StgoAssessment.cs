using STP.Common.Enums;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.VehiclesAndFleets.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static STP.Domain.VehiclesAndFleets.VehicleEnums;

namespace STP.VehiclesAndFleets.Configurations
{
    public class StgoAssessment
    {
        private enum AssessClass
        {
            NoNotification,
            CU,
            STGO,
            VR1,
            VSO,
            SO
        }

        private static bool no;        //No notification required
        private static bool cu2;       //Police only notification 2 days
        private static bool vr1;
        private static bool so;        //Special Order once obtained requires 5 days Police and 5 Days SOA
        private static bool vso;       //VSO are obtained from 
        //private static bool stgoax;    //variable to indicate lowest level STGO without police notfication likely and low axel weight
        //private static bool p2day;     //variable used to indicate police notify reqt for non C&U
        private static bool stgop2day; //variable to indicate an STGO with police notification
        private static bool stgo35;    //stgo3  5 day notice to SOA classification
        private static bool stgo32;    //stgo3  2 day notice to SOA classification
        private static bool stgo2;     //stgo2 classification
        private static bool stgo1;     //stgo1 classification
        //private static bool boatMast;  //variable to indicate boatMast mast exception

        private static void InitValues()
        {
            no = false;
            cu2 = false;
            vr1 = false;
            so = false;
            vso = false;
            stgop2day = false;
            stgo35 = false;
            stgo32 = false;
            stgo2 = false;
            stgo1 = false;
        }

        private static bool isRigid(ConfigurationModel configuration)
        {
            return (configuration.VehicleType == (int)ConfigurationType.Rigid ||
                configuration.VehicleType == (int)ConfigurationType.SPMT ||
                configuration.VehicleType == (int)ConfigurationType.Tracked ||
                configuration.VehicleType == (int)ConfigurationType.Crane);
        }

        private static bool isBoatMast(ConfigurationModel configuration)
        {
            return (configuration.VehicleType == (int)ConfigurationType.BoatMast);
        }

        private static bool isCU(ConfigurationModel configuration)
        {
            var Width = configuration.Width != null ? Math.Round(configuration.Width.Value, 2) : 0;
            var RigidLength = configuration.RigidLength != null ? Math.Round(configuration.RigidLength.Value, 2) : 0;
            var NotifLeftOverhang = configuration.NotifLeftOverhang != null ? Math.Round(configuration.NotifLeftOverhang.Value, 3) : 0;
            var NotifRightOverhang = configuration.NotifRightOverhang != null ? Math.Round(configuration.NotifRightOverhang.Value, 3) : 0;
            var NotifFrontOverhang = configuration.NotifFrontOverhang != null ? Math.Round(configuration.NotifFrontOverhang.Value, 2) : 0;
            var NotifRearOverhang = configuration.NotifRearOverhang != null ? Math.Round(configuration.NotifRearOverhang.Value, 2) : 0;
            var OverallLength = configuration.OverallLength != null ? Math.Round(configuration.OverallLength.Value, 2) : 0;
            return (Width > 2.9 ||
                RigidLength > 18.65 ||
                NotifLeftOverhang > 0.305 || NotifRightOverhang > 0.305 ||
                NotifFrontOverhang > 3.05 || NotifRearOverhang > 3.05 ||
                OverallLength > 25.9);
        }

        private static bool isSTGO(ConfigurationModel configuration, bool rigidVehicle)
        {
            var Width = configuration.Width != null ? Math.Round(configuration.Width.Value, 2) : 0;
            var RigidLength = configuration.RigidLength != null ? Math.Round(configuration.RigidLength.Value, 2) : 0;
            var GrossWeight = configuration.GrossWeight != null ? Math.Round(configuration.GrossWeight.Value, 2) : 0;
            return (Width > 4.3 ||
                RigidLength > 27.4 ||
                GrossWeight > 44000 ||
                rigidVehicle && GrossWeight > 32000 && configuration.AxleCount > 4) ||
                (!rigidVehicle && GrossWeight > 44000 && configuration.AxleCount >= 6) ||
                (!rigidVehicle && GrossWeight > 40000 && configuration.AxleCount == 5) ||
                (!rigidVehicle && GrossWeight > 38000 && configuration.AxleCount == 4) ||
                (GrossWeight > 26000 && configuration.AxleCount == 3) ||
                (GrossWeight > 18000 && configuration.AxleCount == 2) ||
                (configuration.AxleCount >= 6 && configuration.MaxAxleWeight > 11500);
        }

        private static bool isVR1(ConfigurationModel configuration)
        {
            var Width = configuration.Width != null ? Math.Round(configuration.Width.Value, 2) : 0;
            return (Width > 5);
        }

        private static bool isSO(ConfigurationModel configuration)
        {
            var Width = configuration.Width != null ? Math.Round(configuration.Width.Value, 2) : 0;
            var RigidLength = configuration.RigidLength != null ? Math.Round(configuration.RigidLength.Value, 2) : 0;
            var GrossWeight = configuration.GrossWeight != null ? Math.Round(configuration.GrossWeight.Value, 2) : 0;
            var MaxAxleWeight = configuration.MaxAxleWeight != null ? Math.Round(configuration.MaxAxleWeight.Value, 2) : 0;
            return (Width > 6.1 ||
                RigidLength > 30 ||
                GrossWeight > 150000 ||
                MaxAxleWeight > 16500);
        }

        private static bool isVSO(ConfigurationModel configuration, bool rigidVehicle)
        {
            var GrossWeight = configuration.GrossWeight != null ? Math.Round(configuration.GrossWeight.Value, 2) : 0;
            var MaxAxleWeight = configuration.MaxAxleWeight != null ? Math.Round(configuration.MaxAxleWeight.Value, 2) : 0;

            return ((configuration.AxleCount == 2 && GrossWeight > 18000) ||
                (configuration.AxleCount == 3 && GrossWeight > 26000) ||
                (rigidVehicle && configuration.AxleCount == 4 && GrossWeight > 32000) ||
                (!rigidVehicle && configuration.AxleCount == 4 && GrossWeight > 38000) ||
                (configuration.AxleCount == 5 && GrossWeight > 46000) ||
                ((configuration.AxleCount >= 2 && configuration.AxleCount <= 5) && MaxAxleWeight > 11500));
        }

        private static VehicleMovementType NoNotif()
        {
            no = true;
            return new VehicleMovementType()
            {
                VehicleClass = (int)VehicleClassificationType.NoVehicleClassification,
                MovementType = (int)MovementType.no_movement,
                Message = "Based on the information entered your vehicle does not meet the requirements for a notification. However, further details may be required for a more accurate assessment. If you are unsure, please complete the remaining details on the form or contact the helpdesk on 0300 470 3733."
            };
        }

        private static VehicleMovementType CUClass(bool forceApplication = false)
        {
            cu2 = true;
            if (!forceApplication)
            {
                return new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.WheeledConstructionAndUse,
                    MovementType = (int)MovementType.notification,
                    SOANoticePeriod = 0,
                    PoliceNoticePeriod = 2,
                    Message = "Oversize construction and use vehicle (Police Notification - 2 clear working days)."
                };
            }
            else
            {
                return new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.NoVehicleClassification,
                    MovementType = (int)MovementType.no_movement,
                    Message = "Vehicle has not been categorised."
                };
            }
        }

        private static VehicleMovementType STGOClass(ConfigurationModel configuration, bool forceApplication = false, bool rigidVehicle = false)
        {
            if (!forceApplication)
            {
                stgop2day = (configuration.Width > 3 ||
                configuration.RigidLength > 18.75 || configuration.OverallLength > 25.9 ||
                configuration.NotifFrontOverhang > 3.05 || configuration.NotifRearOverhang > 3.05 ||
                configuration.NotifLeftOverhang > 0.305 || configuration.NotifRightOverhang > 0.305 ||
                configuration.GrossWeight > 80000);    //include Police

                if (configuration.AxleCount >= 6)
                {
                    if (configuration.GrossWeight > 80000)
                    {
                        stgo35 = true;
                        return new VehicleMovementType()
                        {
                            VehicleClass = (int)VehicleClassificationType.StgoailCat3,
                            MovementType = (int)MovementType.notification,
                            SOANoticePeriod = 5,
                            PoliceNoticePeriod = stgop2day ? 2 : 0,
                            Message = stgop2day ? "STGO CAT 3 Notification (SOA Notification - 5 clear working days), STGO with Police Notification Non C&U (Police Notification - 2 clear working days)"
                                        : "STGO CAT 3 Notification (SOA Notification - 5 clear working days)."
                        };
                    }
                    else if (configuration.MaxAxleWeight > 12500)
                    {
                        stgo32 = true;
                        return new VehicleMovementType()
                        {
                            VehicleClass = (int)VehicleClassificationType.StgoailCat3,
                            MovementType = (int)MovementType.notification,
                            SOANoticePeriod = 2,
                            PoliceNoticePeriod = stgop2day ? 2 : 0,
                            Message = stgop2day ? "STGO CAT 3 Notification (SOA Notification - 2 clear working days), STGO with Police Notification Non C&U (Police Notification - 2 clear working days)"
                                        : "STGO CAT 3 Notification (SOA Notification - 2 clear working days)."
                        };
                    }
                    else if (configuration.GrossWeight > 50000 || configuration.MaxAxleWeight > 11500)
                    {
                        stgo2 = true;
                        return new VehicleMovementType()
                        {
                            VehicleClass = (int)VehicleClassificationType.StgoailCat2,
                            MovementType = (int)MovementType.notification,
                            SOANoticePeriod = 2,
                            PoliceNoticePeriod = stgop2day ? 2 : 0,
                            Message = stgop2day ? "STGO CAT 2 Notification (SOA Notification - 2 clear working days), STGO with Police Notification Non C&U (Police Notification - 2 clear working days)"
                                        : "STGO CAT 2 Notification (SOA Notification - 2 clear working days)."
                        };
                    }
                    else if (configuration.GrossWeight > 44000 || ((rigidVehicle && configuration.AxleCount > 4 && configuration.GrossWeight > 32000)))
                    {
                        stgo1 = true;
                        return new VehicleMovementType()
                        {
                            VehicleClass = (int)VehicleClassificationType.StgoailCat1,
                            MovementType = (int)MovementType.notification,
                            SOANoticePeriod = 2,
                            PoliceNoticePeriod = stgop2day ? 2 : 0,
                            Message = stgop2day ? "STGO CAT 1 Notification (SOA Notification - 2 clear working days), STGO with Police Notification Non C&U (Police Notification - 2 clear working days)"
                                        : "STGO CAT 1 Notification (SOA Notification - 2 clear working days)."
                        };
                    }
                }
                else if (configuration.AxleCount == 5 && ((configuration.GrossWeight > 40000) || (rigidVehicle && configuration.GrossWeight > 32000)))
                {
                    stgo1 = true;
                    return new VehicleMovementType()
                    {
                        VehicleClass = (int)VehicleClassificationType.StgoailCat1,
                        MovementType = (int)MovementType.notification,
                        SOANoticePeriod = 2,
                        PoliceNoticePeriod = stgop2day ? 2 : 0,
                        Message = stgop2day ? "STGO CAT 1 Notification (SOA Notification - 2 clear working days), STGO with Police Notification Non C&U (Police Notification - 2 clear working days)"
                                    : "STGO CAT 1 Notification (SOA Notification - 2 clear working days)."
                    };
                }
                else if (configuration.GrossWeight > 44000 && configuration.GrossWeight <= 50000)
                {
                    stgo1 = true;
                    return new VehicleMovementType()
                    {
                        VehicleClass = (int)VehicleClassificationType.StgoailCat1,
                        MovementType = (int)MovementType.notification,
                        SOANoticePeriod = 2,
                        PoliceNoticePeriod = stgop2day ? 2 : 0,
                        Message = stgop2day ? "STGO CAT 1 Notification (SOA Notification - 2 clear working days), STGO with Police Notification Non C&U (Police Notification - 2 clear working days)"
                                    : "STGO CAT 1 Notification (SOA Notification - 2 clear working days)."
                    };
                }
                else if (configuration.GrossWeight > 50000 && configuration.GrossWeight <= 80000)
                {
                    stgo2 = true;
                    return new VehicleMovementType()
                    {
                        VehicleClass = (int)VehicleClassificationType.StgoailCat2,
                        MovementType = (int)MovementType.notification,
                        SOANoticePeriod = 2,
                        PoliceNoticePeriod = stgop2day ? 2 : 0,
                        Message = stgop2day ? "STGO CAT 2 Notification (SOA Notification - 2 clear working days), STGO with Police Notification Non C&U (Police Notification - 2 clear working days)"
                                    : "STGO CAT 2 Notification (SOA Notification - 2 clear working days)."
                    };
                }
                else if (configuration.GrossWeight > 80000)
                {
                    stgo35 = true;
                    return new VehicleMovementType()
                    {
                        VehicleClass = (int)VehicleClassificationType.StgoailCat3,
                        MovementType = (int)MovementType.notification,
                        SOANoticePeriod = 5,
                        PoliceNoticePeriod = stgop2day ? 2 : 0,
                        Message = stgop2day ? "STGO CAT 2 Notification (SOA Notification - 5 clear working days), STGO with Police Notification Non C&U (Police Notification - 2 clear working days)"
                                    : "STGO CAT 2 Notification (SOA Notification - 5 clear working days)."
                    };
                }
                if (stgop2day)
                {
                    stgo1 = true;
                    return new VehicleMovementType()
                    {
                        VehicleClass = (int)VehicleClassificationType.StgoailCat1,
                        MovementType = (int)MovementType.notification,
                        PoliceNoticePeriod = 2,
                        Message = "STGO with Police Notification Non C&U (Police Notification - 2 clear working days)."
                    };
                }
                else
                {
                    return new VehicleMovementType()
                    {
                        VehicleClass = (int)VehicleClassificationType.NoVehicleClassification,
                        MovementType = (int)MovementType.no_movement,
                        Message = "Vehicle has not been categorised."
                    };
                }
            }
            else
            {
                return new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.NoVehicleClassification,
                    MovementType = (int)MovementType.no_movement,
                    Message = "Vehicle has not been categorised."
                };
            }
        }

        private static VehicleMovementType VR1Class(ConfigurationModel configuration, bool forceApplication = false, bool rigidVehicle = false)
        {
            VehicleMovementType movementType = STGOClass(configuration, forceApplication, rigidVehicle);
            vr1 = true;
            if (stgo1)
            {
                movementType.VehicleClass = (int)VehicleClassificationType.StgoailCat1;
            }
            else if (stgo2)
            {
                movementType.VehicleClass = (int)VehicleClassificationType.StgoailCat2;
            }
            else if (stgo32 || stgo35)
            {
                movementType.VehicleClass = (int)VehicleClassificationType.StgoailCat3;
            }

            movementType.PoliceNoticePeriod = movementType.PoliceNoticePeriod > 0 ? movementType.PoliceNoticePeriod : 2;
            movementType.Message = movementType.Message + " VR-1 Application Required";
            movementType.MovementType = (int)MovementType.vr_1;

            return movementType;
        }

        private static VehicleMovementType SOClass()
        {
            so = true;
            return new VehicleMovementType()
            {
                VehicleClass = (int)VehicleClassificationType.SpecialOrder,
                MovementType = (int)MovementType.special_order,
                SOANoticePeriod = 5,
                PoliceNoticePeriod = 5,
                Message = "Special Order."
            };
        }

        private static VehicleMovementType VSOClass(ConfigurationModel configuration, bool forceApplication = false)
        {
            vso = true;
            if (configuration.MaxAxleWeight > 11500)
            {
                if (!forceApplication)
                {
                    return new VehicleMovementType()
                    {
                        VehicleClass = (int)VehicleClassificationType.VehicleSpecialOrder,
                        MovementType = (int)MovementType.notification,
                        Message = "Please apply for a VSO, Vehicle has a too heavy axle weight " + configuration.MaxAxleWeight + " MAW."
                    };
                }
                else
                {
                    return new VehicleMovementType()
                    {
                        VehicleClass = (int)VehicleClassificationType.NoVehicleClassification,
                        MovementType = (int)MovementType.no_movement,
                        Message = "Vehicle has not been categorised."
                    };
                }
            }
            else
            {
                if (!forceApplication)
                {
                    return new VehicleMovementType()
                    {
                        VehicleClass = (int)VehicleClassificationType.VehicleSpecialOrder,
                        MovementType = (int)MovementType.notification,
                        Message = "Please apply for a VSO, Vehicle is overweight for a " + configuration.AxleCount + " axle vehicle."
                    };
                }
                else
                {
                    return new VehicleMovementType()
                    {
                        VehicleClass = (int)VehicleClassificationType.NoVehicleClassification,
                        MovementType = (int)MovementType.no_movement,
                        Message = "Vehicle has not been categorised."
                    };
                }
            }
        }

        private static AssessClass BoatMastClass(ConfigurationModel configuration, bool forceApplication = false, bool rigidVehicle = false)
        {
            if (configuration.RigidLength > 27.4 &&
                configuration.GrossWeight <= 12000 && configuration.GrossWeight != 0 &&
                configuration.TrailerWeight <= 10000 && configuration.TrailerWeight != 0)
            {
                return AssessClass.SO;
            }
            //else if (configuration.RigidLength > 27.4 &&
            //    (configuration.GrossWeight > 12000 || configuration.TrailerWeight > 10000))
            //{
            //    return AssessClass.STGO;
            //}
            else if (configuration.RigidLength <= 27.4 && configuration.RigidLength != 0)
            {
                return AssessClass.CU;
            }
            else
            {
                return AssessClass.NoNotification;
            }
        }

        public static void RefineVehicleClassification(ConfigurationModel configuration, ref VehicleMovementType movementType)
        {
            if (configuration.LeadingComponentType != 0)
            {
                movementType.VehicleClass = 
                    VehicleCategoryToMovementType.VehicleClassMappingByLeadingComponent(configuration.LeadingComponentType, movementType.VehicleClass);
            }
            else if (configuration.VehiclePurpose != 0 && configuration.VehiclePurpose != null)
            {
                int movementClassification = VehicleCategoryToMovementType.VehicleMovementTypeMapping((VehicleXmlMovementType)configuration.VehiclePurpose);
                movementType.VehicleClass =
                    VehicleCategoryToMovementType.VehicleClassMappingByPurpose(movementClassification, (int)configuration.VehiclePurpose, movementType.VehicleClass);
            }
        }

        public static VehicleMovementType AssessMovementTypeLogic(ConfigurationModel configuration, bool forceApplication = false)
        {
            InitValues();
            VehicleMovementType movementType = null;

            #region Empty value check
            if ((configuration.GrossWeight == 0 || configuration.GrossWeight == null) &&
                (configuration.Width == 0 || configuration.Width == null) &&
                (configuration.OverallLength == 0 || configuration.OverallLength == null) &&
                (configuration.RigidLength == 0 || configuration.RigidLength == null) &&
                (configuration.MaxAxleWeight == 0 || configuration.MaxAxleWeight == null) &&
                (configuration.AxleCount == 0 || configuration.AxleCount == null) &&
                (configuration.NotifLeftOverhang == 0 || configuration.NotifLeftOverhang == null) &&
                (configuration.NotifRightOverhang == 0 || configuration.NotifRightOverhang == null) &&
                (configuration.NotifFrontOverhang == 0 || configuration.NotifFrontOverhang == null) &&
                (configuration.NotifRearOverhang == 0 || configuration.NotifRearOverhang == null))
            {
                return movementType = new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.NoVehicleClassification,
                    MovementType = (int)MovementType.no_movement,
                    Message = "Vehicle has not been categorised."
                };
            }
            #endregion

            bool rigidVehicle = isRigid(configuration);
            bool crane = configuration.VehicleType == (int)ConfigurationType.Crane ? true : false;

            //Check to see what kind of notification is needed.
            AssessClass foundClass = AssessClass.NoNotification;
            if (isCU(configuration))
            {
                foundClass = AssessClass.CU;
            }
            if (!crane && isSTGO(configuration, rigidVehicle))
            {
                foundClass = AssessClass.STGO;
            }
            if (!crane && isVR1(configuration))
            {
                foundClass = AssessClass.VR1;
            }
            if (!crane && isVSO(configuration, rigidVehicle))
            {
                foundClass = AssessClass.VSO;
            }
            if (!crane && isSO(configuration))
            {
                foundClass = AssessClass.SO;
            }
            if (!crane && isBoatMast(configuration))
            {
                AssessClass boatMastClass = BoatMastClass(configuration, forceApplication, rigidVehicle);
                foundClass = boatMastClass > 0 ? boatMastClass : foundClass;
            }

            //Once identified call the function to provide the correct message and update the correct global boolean to true
            switch (foundClass)
            {
                case AssessClass.CU:
                    movementType = CUClass(forceApplication);
                    break;
                case AssessClass.STGO:
                    movementType = STGOClass(configuration, forceApplication, rigidVehicle);
                    RefineVehicleClassification(configuration, ref movementType);
                    break;
                case AssessClass.VR1:
                    movementType = VR1Class(configuration, forceApplication, rigidVehicle);
                    RefineVehicleClassification(configuration, ref movementType);
                    break;
                case AssessClass.VSO:
                    movementType = VSOClass(configuration, forceApplication);
                    break;
                case AssessClass.SO:
                    movementType = SOClass();
                    break;
                default:
                    movementType = NoNotif();
                    break;
            }

            return movementType;
        }
    }
}