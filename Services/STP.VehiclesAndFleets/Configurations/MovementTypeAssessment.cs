using STP.Common.Enums;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static STP.Domain.VehiclesAndFleets.VehicleEnums;

namespace STP.VehiclesAndFleets.Configurations
{
    public static class MovementTypeAssessment
    {
        private static bool CuWeightLimits(bool rigidLength, double? grossWeight, int? axleCount)
        {
            return ((rigidLength && grossWeight <= 32000 && axleCount >= 4) ||
                (!rigidLength && grossWeight <= 44000 && axleCount >= 6) ||
                (!rigidLength && grossWeight <= 40000 && axleCount >= 5) ||
                (!rigidLength && grossWeight <= 38000 && axleCount >= 4) ||
                (grossWeight <= 26000 && axleCount >= 3) ||
                (grossWeight <= 18000 && axleCount >= 2));
        }

        public static VehicleMovementType AssessMovementTypeLogic(ConfigurationModel configuration, bool forceApplication = false, VehicleMovementType previousMovementType = null)
        {
            VehicleMovementType movementType = null;

            bool no = false;        //No notification required
            bool cu2 = false;       //Police only notification 2 days
            bool vr1 = false;
            bool so = false;        //Special Order once obtained requires 5 days Police and 5 Days SOA
            bool vso = false;       //VSO are obtained from 
            bool stgoax = false;    //variable to indicate lowest level STGO without police notfication likely and low axel weight
            bool p2day = false;     //variable used to indicate police notify reqt for non C&U
            bool stgop2day = false; //variable to indicate an STGO with police notification
            bool stgo35 = false;    //stgo3  5 day notice to SOA classification
            bool stgo32 = false;    //stgo3  2 day notice to SOA classification
            bool stgo2 = false;     //stgo2 classification
            bool stgo1 = false;     //stgo1 classification
            bool boatMast = false;  //variable to indicate boatMast mast exception

            int gSOANoticePeriod = previousMovementType != null ? previousMovementType.SOANoticePeriod : 0;
            int gPoliceNoticePeriod = previousMovementType != null ? previousMovementType.PoliceNoticePeriod : 0;

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

            #region Default value assignments
            // Assign '0' for null value dimensions
            if (configuration.GrossWeight == null)
            {
                configuration.GrossWeight = 0;
            }
            else
            {
                configuration.GrossWeight = Math.Round((double)configuration.GrossWeight, 2);
            }
            if (configuration.Width == null)
            {
                configuration.Width = 0;
            }
            else
            {
                configuration.Width = Math.Round((double)configuration.Width, 2);
            }
            if (configuration.OverallLength == null)
            {
                configuration.OverallLength = 0;
            }
            else
            {
                configuration.OverallLength = Math.Round((double)configuration.OverallLength, 2);
            }
            if (configuration.RigidLength == null)
            {
                if (configuration.OverallLength != 0)
                {
                    configuration.RigidLength = configuration.OverallLength;
                }
                else
                {
                    configuration.RigidLength = 0;
                }
            }
            else
            {
                configuration.RigidLength = Math.Round((double)configuration.RigidLength, 2);
            }
            if (configuration.MaxAxleWeight == null)
            {
                configuration.MaxAxleWeight = 0;
            }
            else
            {
                configuration.MaxAxleWeight = Math.Round((double)configuration.MaxAxleWeight, 2);
            }
            if (configuration.AxleCount == null)
            {
                configuration.AxleCount = 0;
            }
            if (configuration.NotifLeftOverhang == null)
            {
                configuration.NotifLeftOverhang = 0;
            }
            else
            {
                configuration.NotifLeftOverhang = Math.Round((double)configuration.NotifLeftOverhang, 3);
            }
            if (configuration.NotifRightOverhang == null)
            {
                configuration.NotifRightOverhang = 0;
            }
            else
            {
                configuration.NotifRightOverhang = Math.Round((double)configuration.NotifRightOverhang, 3);
            }
            if (configuration.NotifFrontOverhang == null)
            {
                configuration.NotifFrontOverhang = 0;
            }
            else
            {
                configuration.NotifFrontOverhang = Math.Round((double)configuration.NotifFrontOverhang, 3);
            }
            if (configuration.NotifRearOverhang == null)
            {
                configuration.NotifRearOverhang = 0;
            }
            else
            {
                configuration.NotifRearOverhang = Math.Round((double)configuration.NotifRearOverhang, 3);
            }
            #endregion

            bool rigidVehicle = configuration.OverallLength == configuration.RigidLength ? true : false;

            // =============================================================================
            // First Check for VSO based on gross weight and axel count.    
            // =============================================================================
            if ((configuration.AxleCount == 2 && configuration.GrossWeight > 18000) ||
                (configuration.AxleCount == 3 && configuration.GrossWeight > 26000) ||
                (rigidVehicle && configuration.AxleCount == 4 && configuration.GrossWeight > 32000) ||
                (!rigidVehicle && configuration.AxleCount == 4 && configuration.GrossWeight > 38000) ||
                (configuration.AxleCount == 5 && configuration.GrossWeight > 46000))
            {
                if (!forceApplication)
                {
                    return movementType = new VehicleMovementType()
                    {
                        VehicleClass = (int)VehicleClassificationType.VehicleSpecialOrder,
                        MovementType = (int)MovementType.notification,
                        SOANoticePeriod = gSOANoticePeriod,
                        PoliceNoticePeriod = gPoliceNoticePeriod,
                        Message = "Please apply for a VSO, Vehicle is overweight for a " + configuration.AxleCount + " axel vehicle."
                    };
                }
                else
                {
                    return movementType = new VehicleMovementType()
                    {
                        VehicleClass = (int)VehicleClassificationType.NoVehicleClassification,
                        MovementType = (int)MovementType.no_movement,
                        Message = "Vehicle has not been categorised."
                    };
                }
            }

            // =============================================================================
            // Check if the vehicle is construction and use.
            // =============================================================================
            if (configuration.Width <= 2.9
                && configuration.RigidLength <= 18.65
                && configuration.NotifLeftOverhang <= 0.305 && configuration.NotifRightOverhang <= 0.305
                && configuration.NotifFrontOverhang <= 3.05 && configuration.NotifRearOverhang <= 3.05
                && configuration.OverallLength <= 25.9
                && CuWeightLimits(rigidVehicle, configuration.GrossWeight, configuration.AxleCount))
            {
                no = true;
                return movementType = new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.WheeledConstructionAndUse,
                    MovementType = (int)MovementType.no_movement,
                    SOANoticePeriod = gSOANoticePeriod,
                    PoliceNoticePeriod = gPoliceNoticePeriod,
                    Message = "Construction and use vehicle. Does not need a notification."
                };
            }

            // =============================================================================
            // Check if the vehicle is oversize within C&U or is into STGO.
            // =============================================================================

            if (configuration.Width <= 4.3
                && configuration.RigidLength <= 27.4
                && CuWeightLimits(rigidVehicle, configuration.GrossWeight, configuration.AxleCount))
            {
                cu2 = true;
                gPoliceNoticePeriod = gPoliceNoticePeriod > 2 ? gPoliceNoticePeriod : 2;
                if (!forceApplication)
                {
                    int SOANoticePeriod = gSOANoticePeriod;
                    int PoliceNoticePeriod = gPoliceNoticePeriod;

                    return movementType = new VehicleMovementType()
                    {
                        VehicleClass = (int)VehicleClassificationType.WheeledConstructionAndUse,
                        MovementType = (int)MovementType.notification,
                        SOANoticePeriod = SOANoticePeriod,
                        PoliceNoticePeriod = PoliceNoticePeriod,
                        Message = "Oversize construction and use vehicle (Police Notification - 2 clear working days)."
                    };
                }
                else
                {
                    return movementType = new VehicleMovementType()
                    {
                        VehicleClass = (int)VehicleClassificationType.NoVehicleClassification,
                        MovementType = (int)MovementType.no_movement,
                        Message = "Vehicle has not been categorised."
                    };
                }
            }

            // =============================================================================
            // Enter STGO details (no requirement for tyre spacing data)
            // Enter STGO vehicle data including axel spacings but not tyre spacing data.
            // =============================================================================
            if (configuration.MaxAxleWeight > 16500
                || configuration.Width > 6.1
                || configuration.RigidLength > 30
                || configuration.GrossWeight > 150000)
            {
                so = true;
                gSOANoticePeriod = gSOANoticePeriod > 5 ? gSOANoticePeriod : 5;
                gPoliceNoticePeriod = gPoliceNoticePeriod > 5 ? gPoliceNoticePeriod : 5;
                if (!forceApplication)
                {
                    return movementType = new VehicleMovementType()
                    {
                        VehicleClass = (int)VehicleClassificationType.SpecialOrder,
                        MovementType = (int)MovementType.special_order,
                        SOANoticePeriod = gSOANoticePeriod,
                        PoliceNoticePeriod = gPoliceNoticePeriod,
                        Message = "Special Order."
                    };
                }
                else
                {
                    return movementType = new VehicleMovementType()
                    {
                        VehicleClass = (int)VehicleClassificationType.SpecialOrder,
                        MovementType = (int)MovementType.special_order,
                        Message = "Special Order."
                    };
                }
            }

            if (configuration.AxleCount >= 6)
            {
                if (configuration.GrossWeight > 80000)
                {
                    stgo35 = true;
                    gSOANoticePeriod = gSOANoticePeriod > 5 ? gSOANoticePeriod : 5;
                    if (!forceApplication)
                    {
                        int SOANoticePeriod = gSOANoticePeriod;
                        int PoliceNoticePeriod = gPoliceNoticePeriod;
                        movementType = new VehicleMovementType()
                        {
                            VehicleClass = (int)VehicleClassificationType.StgoailCat3,
                            MovementType = (int)MovementType.notification,
                            SOANoticePeriod = SOANoticePeriod,
                            PoliceNoticePeriod = PoliceNoticePeriod,
                            Message = "STGO CAT 3 Notification (SOA Notification - 5 clear working days)."
                        };
                    }
                }
                else if (configuration.MaxAxleWeight > 12500)
                {
                    stgo32 = true;
                    gSOANoticePeriod = gSOANoticePeriod > 2 ? gSOANoticePeriod : 2;
                    if (!forceApplication)
                    {
                        int SOANoticePeriod = gSOANoticePeriod;
                        int PoliceNoticePeriod = gPoliceNoticePeriod;
                        movementType = new VehicleMovementType()
                        {
                            VehicleClass = (int)VehicleClassificationType.StgoailCat3,
                            MovementType = (int)MovementType.notification,
                            SOANoticePeriod = SOANoticePeriod,
                            PoliceNoticePeriod = PoliceNoticePeriod,
                            Message = "STGO CAT 3 Notification (SOA Notification - 2 clear working days)."
                        };
                    }
                }
                else if (configuration.GrossWeight > 50000 || configuration.MaxAxleWeight > 11500)
                {
                    stgo2 = true;
                    gSOANoticePeriod = gSOANoticePeriod > 2 ? gSOANoticePeriod : 2;
                    if (!forceApplication)
                    {
                        int SOANoticePeriod = gSOANoticePeriod;
                        int PoliceNoticePeriod = gPoliceNoticePeriod;
                        movementType = new VehicleMovementType()
                        {
                            VehicleClass = (int)VehicleClassificationType.StgoailCat2,
                            MovementType = (int)MovementType.notification,
                            SOANoticePeriod = SOANoticePeriod,
                            PoliceNoticePeriod = PoliceNoticePeriod,
                            Message = "STGO CAT 2 Notification (SOA Notification - 2 clear working days)"
                        };
                    }
                }
                else if (configuration.GrossWeight > 44000)
                {
                    stgo1 = true;
                    gSOANoticePeriod = gSOANoticePeriod > 2 ? gSOANoticePeriod : 2;
                    if (!forceApplication)
                    {
                        int SOANoticePeriod = gSOANoticePeriod;
                        int PoliceNoticePeriod = gPoliceNoticePeriod;
                        movementType = new VehicleMovementType()
                        {
                            VehicleClass = (int)VehicleClassificationType.StgoailCat1,
                            MovementType = (int)MovementType.notification,
                            SOANoticePeriod = SOANoticePeriod,
                            PoliceNoticePeriod = PoliceNoticePeriod,
                            Message = "STGO CAT 1 Notification (SOA Notification - 2 clear working days)"
                        };
                    }
                }
                else if (rigidVehicle && configuration.AxleCount > 4 && configuration.GrossWeight > 32000)
                {
                    stgo1 = true;
                    gSOANoticePeriod = gSOANoticePeriod > 2 ? gSOANoticePeriod : 2;
                    if (!forceApplication)
                    {
                        int SOANoticePeriod = gSOANoticePeriod;
                        int PoliceNoticePeriod = gPoliceNoticePeriod;
                        movementType = new VehicleMovementType()
                        {
                            VehicleClass = (int)VehicleClassificationType.StgoailCat1,
                            MovementType = (int)MovementType.notification,
                            SOANoticePeriod = SOANoticePeriod,
                            PoliceNoticePeriod = PoliceNoticePeriod,
                            Message = "STGO CAT 1 Notification (SOA Notification - 2 clear working days)"
                        };
                    }
                }
                else
                {
                    stgo1 = true;
                    gSOANoticePeriod = gSOANoticePeriod > 2 ? gSOANoticePeriod : 2;
                    movementType = new VehicleMovementType()
                    {
                        VehicleClass = (int)VehicleClassificationType.StgoailCat1,
                        MovementType = (int)MovementType.vr_1,
                        SOANoticePeriod = gSOANoticePeriod,
                        PoliceNoticePeriod = gPoliceNoticePeriod,
                        Message = "Vehicle is not STGO plated, but treated as STGO."
                    };
                }
            }

            else if (configuration.AxleCount == 5)
            {
                if (configuration.MaxAxleWeight > 11500 || configuration.GrossWeight > 46000)
                {
                    vso = true;
                    if (!forceApplication)
                    {
                        return movementType = new VehicleMovementType()
                        {
                            VehicleClass = (int)VehicleClassificationType.VehicleSpecialOrder,
                            MovementType = (int)MovementType.notification,
                            Message = "The vehicle exceeds STGO. Outside the scope of ESDAL. May need a VSO."
                        };
                    }
                    else
                    {
                        return movementType = new VehicleMovementType()
                        {
                            VehicleClass = (int)VehicleClassificationType.NoVehicleClassification,
                            MovementType = (int)MovementType.no_movement,
                            Message = "Vehicle has not been categorised."
                        };
                    }
                }
                else if (configuration.GrossWeight > 40000)
                {
                    stgo1 = true;
                    gSOANoticePeriod = gSOANoticePeriod > 2 ? gSOANoticePeriod : 2;
                    if (!forceApplication)
                    {
                        int SOANoticePeriod = gSOANoticePeriod;
                        int PoliceNoticePeriod = gPoliceNoticePeriod;
                        movementType = new VehicleMovementType()
                        {
                            VehicleClass = (int)VehicleClassificationType.StgoailCat1,
                            MovementType = (int)MovementType.notification,
                            SOANoticePeriod = SOANoticePeriod,
                            PoliceNoticePeriod = PoliceNoticePeriod,
                            Message = "STGO CAT 1 Notification (SOA Notification - 2 clear working days)"
                        };
                    }
                }
                else if (rigidVehicle && configuration.GrossWeight > 32000)
                {
                    stgo1 = true;
                    gSOANoticePeriod = gSOANoticePeriod > 2 ? gSOANoticePeriod : 2;
                    if (!forceApplication)
                    {
                        int SOANoticePeriod = gSOANoticePeriod;
                        int PoliceNoticePeriod = gPoliceNoticePeriod;
                        movementType = new VehicleMovementType()
                        {
                            VehicleClass = (int)VehicleClassificationType.StgoailCat1,
                            MovementType = (int)MovementType.notification,
                            SOANoticePeriod = SOANoticePeriod,
                            PoliceNoticePeriod = PoliceNoticePeriod,
                            Message = "STGO CAT 1 Notification (SOA Notification - 2 clear working days)"
                        };
                    }
                }
            }

            else
            {
                stgo1 = true;
                gSOANoticePeriod = gSOANoticePeriod > 2 ? gSOANoticePeriod : 2;
                movementType = new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.StgoailCat1,
                    MovementType = (int)MovementType.vr_1,
                    SOANoticePeriod = gSOANoticePeriod,
                    PoliceNoticePeriod = gPoliceNoticePeriod,
                    Message = "Vehicle is not STGO plated, but treated as STGO."
                };
            }

            // =============================================================================
            // The following decides if it is STGO with or without 2 day Police Notification              
            // =============================================================================
            if (stgo1 || stgo2 || stgo32 || stgo35)
            {
                if (configuration.Width <= 3
                    && configuration.RigidLength <= 18.75
                    && configuration.OverallLength <= 25.9
                    && configuration.NotifLeftOverhang <= 0.305 && configuration.NotifRightOverhang <= 0.305
                    && configuration.NotifFrontOverhang <= 3.05 && configuration.NotifRearOverhang <= 3.05
                    && configuration.GrossWeight <= 80000)
                {
                    if (!forceApplication)
                    {
                        if (movementType == null)
                        {
                            movementType = new VehicleMovementType()
                            {
                                VehicleClass = (int)VehicleClassificationType.StgoailCat2,
                                MovementType = (int)MovementType.notification,
                                SOANoticePeriod = gSOANoticePeriod,
                                PoliceNoticePeriod = gPoliceNoticePeriod,
                                Message = "STGO without Police Notification weight only."
                            };
                        }
                        else
                        {
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
                            movementType.MovementType = (int)MovementType.notification;
                            movementType.SOANoticePeriod = gSOANoticePeriod;
                            movementType.PoliceNoticePeriod = gPoliceNoticePeriod;
                            movementType.Message += ";STGO without Police Notification weight only.";
                        }
                    }
                }
                else if (configuration.Width > 3
                    || configuration.RigidLength > 18.75
                    || configuration.OverallLength > 25.9
                    || configuration.NotifLeftOverhang > 0.305 || configuration.NotifRightOverhang > 0.305
                    || configuration.NotifFrontOverhang > 3.05 || configuration.NotifRearOverhang > 3.05
                    || configuration.GrossWeight > 80000)
                {
                    // =============================================================================
                    // Check need for a VR-1
                    // =============================================================================
                    if (configuration.Width > 6.1
                        || configuration.RigidLength > 30
                        || configuration.GrossWeight > 150000)
                    {
                        so = true;
                        gSOANoticePeriod = gSOANoticePeriod > 5 ? gSOANoticePeriod : 5;
                        gPoliceNoticePeriod = gPoliceNoticePeriod > 5 ? gPoliceNoticePeriod : 5;
                        return movementType = new VehicleMovementType()
                        {
                            VehicleClass = (int)VehicleClassificationType.SpecialOrder,
                            MovementType = (int)MovementType.special_order,
                            SOANoticePeriod = gSOANoticePeriod,
                            PoliceNoticePeriod = gPoliceNoticePeriod,
                            Message = "Special Order."
                        };
                    }

                    if (configuration.Width > 5)
                    {
                        vr1 = true;
                        gSOANoticePeriod = gSOANoticePeriod > 2 ? gSOANoticePeriod : 2;
                        if (movementType == null)
                        {
                            int SOANoticePeriod = gSOANoticePeriod;
                            int PoliceNoticePeriod = gPoliceNoticePeriod;
                            movementType = new VehicleMovementType()
                            {
                                VehicleClass = stgo1 ? (int)VehicleClassificationType.StgoailCat1 :
                                               (stgo2 ? (int)VehicleClassificationType.StgoailCat2 : (int)VehicleClassificationType.StgoailCat3),
                                MovementType = (int)MovementType.vr_1,
                                SOANoticePeriod = SOANoticePeriod,
                                PoliceNoticePeriod = PoliceNoticePeriod,
                                Message = "VR-1 Application 10 days Nat Highways 2 days Police."
                            };
                        }
                        else
                        {
                            gPoliceNoticePeriod = gPoliceNoticePeriod > 2 ? gPoliceNoticePeriod : 2;
                            int SOANoticePeriod = gSOANoticePeriod;
                            int PoliceNoticePeriod = gPoliceNoticePeriod;
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
                            movementType.MovementType = (int)MovementType.vr_1;
                            movementType.SOANoticePeriod = SOANoticePeriod;
                            movementType.PoliceNoticePeriod = PoliceNoticePeriod;
                            movementType.Message += ";VR-1 Application 10 days Nat Highways 2 days Police.";
                        }
                    }
                    else
                    {
                        stgop2day = true;
                        if (!forceApplication)
                        {
                            gPoliceNoticePeriod = gPoliceNoticePeriod > 2 ? gPoliceNoticePeriod : 2;
                            int SOANoticePeriod = gSOANoticePeriod;
                            int PoliceNoticePeriod = gPoliceNoticePeriod;
                            if (movementType == null)
                            {
                                movementType = new VehicleMovementType()
                                {
                                    VehicleClass = stgo1 ? (int)VehicleClassificationType.StgoailCat1 :
                                               (stgo2 ? (int)VehicleClassificationType.StgoailCat2 : (int)VehicleClassificationType.StgoailCat3),
                                    MovementType = (int)MovementType.notification,
                                    SOANoticePeriod = SOANoticePeriod,
                                    PoliceNoticePeriod = PoliceNoticePeriod,
                                    Message = "STGO with Police Notification Non C&U (Police Notification - 2 clear working days)"
                                };
                            }
                            else
                            {
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
                                movementType.MovementType = (int)MovementType.notification;
                                movementType.SOANoticePeriod = SOANoticePeriod;
                                movementType.PoliceNoticePeriod = PoliceNoticePeriod;
                                movementType.Message += ";STGO with Police Notification Non C&U (Police Notification - 2 clear working days)";
                            }
                        }
                    }
                }
                else
                {
                    if (!forceApplication)
                    {
                        if (movementType == null)
                        {
                            movementType = new VehicleMovementType()
                            {
                                VehicleClass = stgo1 ? (int)VehicleClassificationType.StgoailCat1 :
                                               (stgo2 ? (int)VehicleClassificationType.StgoailCat2 : (int)VehicleClassificationType.StgoailCat3),
                                MovementType = (int)MovementType.notification,
                                SOANoticePeriod = gSOANoticePeriod,
                                PoliceNoticePeriod = gPoliceNoticePeriod,
                                Message = "STGO applies without a police notification."
                            };
                        }
                        else
                        {
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
                            movementType.MovementType = (int)MovementType.notification;
                            movementType.SOANoticePeriod = gSOANoticePeriod;
                            movementType.PoliceNoticePeriod = gPoliceNoticePeriod;
                            movementType.Message += ";STGO applies without a police notification.";
                        }
                    }
                }
            }
            // ========================================================================================
            // The following finds the Boat Mast exception but has not been included in the code below.  
            // May need user input to identify this category 
            // ========================================================================================
            else if (configuration.OverallLength > 27.4
                && configuration.GrossWeight <= 12000
                && configuration.TrailerWeight <= 10000
                && !stgop2day && !stgo1 && !stgo2 && !stgo32 && !stgo35)
            {
                boatMast = true;
                so = true;
                return new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.SpecialOrder,
                    MovementType = (int)MovementType.special_order,
                    SOANoticePeriod = gSOANoticePeriod,
                    PoliceNoticePeriod = gPoliceNoticePeriod,
                    Message = "'Rigid and Drag' needs a Special Order."
                };
            }
            else if (configuration.OverallLength > 27.4
                && (configuration.GrossWeight > 12000 || configuration.TrailerWeight > 10000)
                && !stgop2day && !stgo1 && !stgo2 && !stgo32 && !stgo35)
            {
                vso = true;
                return new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.VehicleSpecialOrder,
                    MovementType = (int)MovementType.notification,
                    SOANoticePeriod = gSOANoticePeriod,
                    PoliceNoticePeriod = gPoliceNoticePeriod,
                    Message = "'Rigid and Drag' may need a VSO."
                };
            }
            else
            {
                movementType = new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.NoVehicleClassification,
                    MovementType = (int)MovementType.no_movement,
                    Message = "Vehicle has not been categorised."
                };
            }

            if (movementType == null)
            {
                movementType = new VehicleMovementType()
                {
                    VehicleClass = (int)VehicleClassificationType.NoVehicleClassification,
                    MovementType = (int)MovementType.no_movement,
                    Message = "Vehicle has not been categorised."
                };
            }

            return movementType;
        }
    }
}