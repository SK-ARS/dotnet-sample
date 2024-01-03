using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.VehiclesAndFleets
{
    public class VehicleEnums
    {
        public enum ComponentType
        {
            [Description("ballast tractor")]
            BallastTractor = 234001,
            [Description("conventional tractor")]
            ConventionalTractor = 234002,
            [Description("rigid vehicle")]
            RigidVehicle = 234003,
            [Description("tracked vehicle")]
            Tracked = 234004,
            [Description("semi trailer")]
            SemiTrailer = 234005,
            [Description("drawbar trailer")]
            DrawbarTrailer = 234006,
            [Description("spmt")]
            SPMT = 234007,
            [Description("mobile crane")]
            MobileCrane = 234008,

            [Description("engineering plant")]
            EngineeringPlant = 234009,
            [Description("engineering plant–semi trailer")]
            EngineeringPlantSemiTrailer = 234010,
            [Description("engineering plant–drawbar trailer")]
            EngineeringPlantDrawbarTrailer = 234011,

            [Description("recovery vehicle")]
            RecoveryVehicle = 234012,

            [Description("girder set")]
            GirderSet = 234013
        }

        public enum ComponentSubType
        {
            [Description("ballast tractor")]
            BallastTractor = 224001,
            [Description("conventional tractor")]
            ConventionalTractor = 224002,
            [Description("other tractor")]
            OtherTractor = 224003,
            [Description("semi trailer")]
            SemiTrailer = 224004,
            [Description("semi low loader")]
            SemiLowLoader = 224005,
            [Description("trombone trailer")]
            TromboneTrailer = 224006,
            [Description("other semi trailer")]
            OtherSemiTrailer = 224007,
            [Description("drawbar trailer")]
            DrawbarTrailer = 224008,
            [Description("other drawbar trailer")]
            OtherDrawbarTrailer = 224009,
            [Description("bogie")]
            Bogie = 224010,
            [Description("twin bogies")]
            TwinBogies = 224011,
            [Description("tracked vehicle")]
            TrackedVehicle = 224012,
            [Description("rigid vehicle")]
            RigidVehicle = 224013,
            [Description("spmt")]
            SPMT = 224014,
            [Description("wheeled load")]
            WheeledLoad = 224016,
            
            [Description("recovered vehicle")]
            RecoveredVehicle = 224018,
            [Description("mobile crane")]
            MobileCrane = 224019,
            [Description("engineering plant")]
            EngineeringPlant = 224020,

            [Description("eng plant-conventional tractor")]
            EngPlantConventionalTractor = 224021,
            [Description("eng plant-rigid")]
            EngPlantRigid = 224022,
            [Description("eng plant-tracked")]
            EngPlantTracked = 224023,
            [Description("eng plant-ballast tractor")]
            EngPlantBallastTractor = 224024,


            [Description("engineering plant–semi trailer")]
            EngineeringPlantSemiTrailer = 224025,
            [Description("engineering plant–drawbar trailer")]
            EngineeringPlantDrawbarTrailer = 224026,

            [Description("recovery vehicle")]
            RecoveryVehicle = 224027,

            [Description("girder set")]
            GirderSet = 224028,

            [Description("clamp system")]
            ClampSystem = 224029
        }

        public enum CouplingType
        {
            [Description("None")]
            None = 201001,
            [Description("5th wheel")]
            FifthWheel = 201002,
            [Description("drawbar")]
            Drawbar = 201003,
            [Description("tow hitch")]
            TowHitch = 201004
        }

        public enum CouplingTypeNew
        {
            [Description("None")]
            None = 201001,
            [Description("5th wheel")]
            FifthWheel = 201002,
            [Description("Drawbar")]
            Drawbar = 201003,
            [Description("Tow hitch")]
            TowHitch = 201004,
            [Description("No Coupling")]
            NoCoupling = 0
        }

        public enum DimensionUnitSystem
        {
            [Description("imperial system")]
            Imperial = 208006,
            [Description("metric")]
            Metric = 208001
        }

        public enum WeightUnitSystem
        {
            [Description("imperial system")]
            Imperial = 240003,
            [Description("metric")]
            Metric = 240001
        }

        public enum SpeedUnitSystem
        {
            [Description("imperial system")]
            Imperial = 229001,
            [Description("metric")]
            Metric = 229002
        }

        public enum BitSystem
        {
            [Description("yes")]
            Yes = 1,
            [Description("no")]
            No = 0
        }

        public enum ConfigurationType
        {
            [Description("Drawbar trailer")]
            DrawbarTrailer = 244001,
            [Description("Semi trailer")]
            SemiTrailer = 244002,
            [Description("Rigid Vehicle")]
            Rigid = 244003,
            [Description("Tracked Vehicle")]
            Tracked = 244004,
            [Description("SPMT")]
            SPMT = 244005,
            [Description("other in line")]
            OtherInline = 244006,
            [Description("other side by side")]
            SidebySide = 244007,
            [Description("Boat Mast Exception")]
            BoatMast = 244008,
            [Description("Semi Trailer 3-8")]
            SemiTrailer_3_8 = 244009,
            [Description("DrawbarTrailer 3-8")]
            DrawbarTrailer_3_8 = 244010,
            [Description("Rigid And Drag")]
            RigidAndDrag = 244011,
            [Description("Crane")]
            Crane = 244012,
            [Description("Recovery Vehicle")]
            RecoveryVehicle = 244013
        }

        public enum VehiclePurpose
        {
            [Description("c and u")]
            WheeledConstructionAndUse = 270001,
            [Description("stgo ail")]
            Stgoail = 270002,
            [Description("stgo mobile crane")]
            StgoMobileCrane = 270003,
            [Description("stgo engineering plant wheeled")]
            StgoEngineeringPlantWheeled = 270004,
            [Description("stgo road recovery")]
            StgoRoadRecovery = 270005,
            [Description("special order")]
            SpecialOrder = 270006,
            [Description("vehicle special order")]
            VehicleSpecialOrder = 270007,
            [Description("tracked")]
            Tracked = 270008,
            [Description("Stgo-ail Cat1")]
            StgoailCat1 = 270009,
            [Description("Stgo-ail Cat2")]
            StgoailCat2 = 270010,
            [Description("Stgo-ail Cat3")]
            StgoailCat3 = 270011,
            [Description("stgo mobile crane cat a")]
            StgoMobileCraneCata = 270012,
            [Description("stgo mobile crane cat b")]
            StgoMobileCraneCatb = 270013,
            [Description("stgo mobile crane cat c")]
            StgoMobileCraneCatc = 270014,
            [Description("stgo engineering plant tracked")]
            StgoEngineeringPlantTracked = 270015,
            [Description("Unclassified")]
            Unclassified = 270101,
            [Description("No crane")]
            Crane = 270155
        }

        public enum VehicleClassificationType
        {
            [Description("Vehicle Special Order")]
            VehicleSpecialOrder = 241001,
            [Description("Special Order")]
            SpecialOrder = 241002,
            [Description("Stgo-ail Cat1")]
            StgoailCat1 = 241003,
            [Description("Stgo-ail Cat2")]
            StgoailCat2 = 241004,
            [Description("Stgo-ail Cat3")]
            StgoailCat3 = 241005,
            [Description("stgo mobile crane cat a")]
            StgoMobileCraneCata = 241006,
            [Description("stgo mobile crane cat b")]
            StgoMobileCraneCatb = 241007,
            [Description("stgo mobile crane cat c")]
            StgoMobileCraneCatc = 241008,
            [Description("stgo engineering plant")]
            StgoEngineeringPlant = 241009,
            [Description("stgo road recovery vehicle")]
            StgoRoadRecoveryVehicle = 241010,
            [Description("Wheeled construction and use")]
            WheeledConstructionAndUse = 241011,
            [Description("Tracked")]
            Tracked = 241012,
            [Description("stgo engineering plant wheeled")]
            StgoEngineeringPlantWheeled = 241013,
            [Description("stgo engineering plant tracked")]
            StgoEngineeringPlantTracked = 241014,
            [Description("No Vehicle Classification")]
            NoVehicleClassification = 241015,
            [Description("No Crane")]
            NoCrane = 241016,
            [Description("stgo engineering plant wheeled")]
            StgoCat1EngineeringPlantWheeled = 241017,
            [Description("stgo engineering plant wheeled")]
            StgoCat2EngineeringPlantWheeled = 241018,
            [Description("stgo engineering plant wheeled")]
            StgoCat3EngineeringPlantWheeled = 241019,
            [Description("stgo engineering plant tracked")]
            StgoCat1EngineeringPlantTracked = 241020,
            [Description("stgo engineering plant tracked")]
            StgoCat2EngineeringPlantTracked = 241021,
            [Description("stgo engineering plant tracked")]
            StgoCat3EngineeringPlantTracked = 241022,
            [Description("stgo road recovery")]
            StgoCat1RoadRecovery = 241023,
            [Description("stgo road recovery")]
            StgoCat2RoadRecovery = 241024,
            [Description("stgo road recovery")]
            StgoCat3RoadRecovery = 241025,
            [Description("stgo vr-1 engineering plant wheeled")]
            StgoCat1Vr1EngineeringPlantWheeled = 241026,
            [Description("stgo vr-1 engineering plant wheeled")]
            StgoCat2Vr1EngineeringPlantWheeled = 241027,
            [Description("stgo vr-1 engineering plant wheeled")]
            StgoCat3Vr1EngineeringPlantWheeled = 241028,
            [Description("stgo vr-1 engineering plant tracked")]
            StgoCat1Vr1EngineeringPlantTracked = 241029,
            [Description("stgo vr-1 engineering plant tracked")]
            StgoCat2Vr1EngineeringPlantTracked = 241030,
            [Description("stgo vr-1 engineering plant tracked")]
            StgoCat3Vr1EngineeringPlantTracked = 241031,
            [Description("stgo vr-1 road recovery")]
            StgoCat1Vr1RoadRecovery = 241032,
            [Description("stgo vr-1 road recovery")]
            StgoCat2Vr1RoadRecovery = 241033,
            [Description("stgo vr-1 road recovery")]
            StgoCat3Vr1RoadRecovery = 241034
        }

        public enum VehicleCategoryToMovementTypeHighLevelMapping
        {
            [Description("Wheeled construction and use")]
            WheeledConstructionAndUse = 270001,
            [Description("stgo engineering plant wheeled")]
            StgoEngineeringPlantWheeled = 270004,
            [Description("stgo road recovery vehicle")]
            StgoRoadRecoveryVehicle = 270005,
            [Description("Special Order")]
            SpecialOrder = 270006,
            [Description("Vehicle Special Order")]
            VehicleSpecialOrder = 270007,
            [Description("Tracked")]
            Tracked = 270008,
            [Description("Stgo-ail Cat1")]
            StgoailCat1 = 270009,
            [Description("Stgo-ail Cat2")]
            StgoailCat2 = 270010,
            [Description("Stgo-ail Cat3")]
            StgoailCat3 = 270011,
            [Description("stgo mobile crane cat a")]
            StgoMobileCraneCata = 270012,
            [Description("stgo mobile crane cat b")]
            StgoMobileCraneCatb = 270013,
            [Description("stgo mobile crane cat c")]
            StgoMobileCraneCatc = 270014,
            [Description("stgo engineering plant tracked")]
            StgoEngineeringPlantTracked = 270015,
            [Description("No Vehicle Classification")]
            NoVehicleClassification = 270017
        }

        public enum VehicleCategoryToMovementTypeMapping
        {
            [Description("Special Order")]
            SpecialOrder = 270006,
            [Description("No Notification")]
            NoNotification = 270101,
            [Description("C&U Notification (Police Notification - 2 clear working days)")]
            CandUNotification_Pol2 = 270102,
            [Description("STGO CAT 1 Notification (Police Notification - 2 clear working days)")]
            StgoailCat1_Pol2 = 270103,
            [Description("STGO CAT 1 Notification (SOA Notification - 2 clear working days)")]
            StgoailCat1_Soa2 = 270104,
            [Description("STGO CAT 1 Notification (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)")]
            StgoailCat1_Soa2_Pol2 = 270105,
            [Description("STGO CAT 2 Notification (SOA Notification - 2 clear working days)")]
            StgoailCat2_Soa2 = 270106,
            [Description("STGO CAT 2 Notification (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)")]
            StgoailCat2_Soa2_Pol2 = 270107,
            [Description("STGO CAT 3 Notification (SOA Notification - 2 clear working days)")]
            StgoailCat3_Soa2 = 270108,
            [Description("STGO CAT 3 Notification (SOA Notification - 5 clear working days, Police Notification - 2 clear working days)")]
            StgoailCat3_Soa5_Pol2 = 270109,
            [Description("STGO CAT 1 VR-1 (Police Notification - 2 clear working days)")]
            StgoailCat1Vr1_Pol2 = 270110,
            [Description("STGO CAT 2 VR-1 (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)")]
            StgoailCat2Vr1_Soa2_Pol2 = 270111,
            [Description("STGO CAT 3 VR-1 (SOA Notification - 5 clear working days, Police Notification - 2 clear working days)")]
            StgoailCat3Vr1_Soa5_Pol2 = 270112,
            [Description("VSO Notification")]
            VehicleSpecialOrder = 270113,
            [Description("SO Notification")]
            SONotification = 270114,
            [Description("Tracked")]
            Tracked = 270115,
            [Description("SO Application")]
            SOApplication = 270116,
            [Description("VR-1 Application")]
            VR1Application = 270117,
            [Description("STGO Mobile Crane CAT A (SOA Notification - 2 clear working days)")]
            StgoMobileCraneCatA_Soa2 = 270118,
            [Description("STGO Mobile Crane CAT A (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)")]
            StgoMobileCraneCatA_Soa2_Pol2 = 270119,
            [Description("STGO Mobile Crane CAT B (SOA Notification - 2 clear working days)")]
            StgoMobileCraneCatB_Soa2 = 270120,
            [Description("STGO Mobile Crane CAT B (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)")]
            StgoMobileCraneCatB_Soa2_Pol2 = 270121,
            [Description("STGO Mobile Crane CAT B (SOA Notification - 5 clear working days, Police Notification - 2 clear working days)")]
            StgoMobileCraneCatB_Soa5_Pol2 = 270122,
            [Description("STGO Mobile Crane CAT C (SOA Notification - 2 clear working days)")]
            StgoMobileCraneCatC_Soa2 = 270123,
            [Description("STGO Mobile Crane CAT C (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)")]
            StgoMobileCraneCatC_Soa2_Pol2 = 270124,
            [Description("STGO Mobile Crane CAT C (SOA Notification - 5 clear working days, Police Notification - 2 clear working days)")]
            StgoMobileCraneCatC_Soa5_Pol2 = 270125,
            [Description("STGO Engineering Plant - Tracked (Police Notification - 2 clear working days)")]
            StgoEngPlantTrackedCat1Notification_Pol2 = 270126,
            [Description("STGO Engineering Plant - Wheeled (Police Notification - 2 clear working days)")]
            StgoEngPlantWheeledCat1Notification_Pol2 = 270127,
            [Description("STGO Road Recovery (Police Notification - 2 clear working days)")]
            StgoRoadRecoveryCat1Notification_Pol2 = 270128,
            [Description("STGO tracked CAT 1 Notification (Police Notification - 2 clear working days)")]
            StgoTrackedCat1Notification_Pol2 = 270129,
            [Description("STGO Engineering Plant - Tracked (SOA Notification - 2 clear working days)")]
            StgoEngPlantTrackedCat1Notification_Soa2 = 270130,
            [Description("STGO Engineering Plant - Wheeled (SOA Notification - 2 clear working days)")]
            StgoEngPlantWheeledCat1Notification_Soa2 = 270131,
            [Description("STGO Road Recovery (SOA Notification - 2 clear working days)")]
            StgoRoadRecoveryCat1Notification_Soa2 = 270132,
            [Description("STGO tracked CAT 1 Notification (SOA Notification - 2 clear working days)")]
            StgoTrackedCat1Notification_Soa2 = 270133,
            [Description("STGO Engineering Plant - Tracked (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)")]
            StgoEngPlantTrackedCat1Notification_Soa2_Pol2 = 270134,
            [Description("STGO Engineering Plant - Wheeled (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)")]
            StgoEngPlantWheeledCat1Notification_Soa2_Pol2 = 270135,
            [Description("STGO Road Recovery (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)")]
            StgoRoadRecoveryCat1Notification_Soa2_Pol2 = 270136,
            [Description("STGO tracked CAT 1 Notification (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)")]
            StgoTrackedCat1Notification_Soa2_Pol2 = 270137,
            [Description("STGO Engineering Plant - Tracked (SOA Notification - 2 clear working days)")]
            StgoEngPlantTrackedCat2Notification_Soa2 = 270138,
            [Description("STGO Engineering Plant - Wheeled (SOA Notification - 2 clear working days)")]
            StgoEngPlantWheeledCat2Notification_Soa2 = 270139,
            [Description("STGO Road Recovery (SOA Notification - 2 clear working days)")]
            StgoRoadRecoveryCat2Notification_Soa2 = 270140,
            [Description("STGO tracked CAT-2 Notification (SOA Notification - 2 clear working days)")]
            StgoTrackedCat2Notification_Soa2 = 270141,
            [Description("STGO Engineering Plant - Tracked (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)")]
            StgoEngPlantTrackedCat2Notification_Soa2_Pol2 = 270142,
            [Description("STGO Engineering Plant - Wheeled (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)")]
            StgoEngPlantWheeledCat2Notification_Soa2_Pol2 = 270143,
            [Description("STGO Road Recovery (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)")]
            StgoRoadRecoveryCat2Notification_Soa2_Pol2 = 270144,
            [Description("STGO tracked CAT-2 Notification (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)")]
            StgoTrackedCat2Notification_Soa2_Pol2 = 270145,
            [Description("STGO Engineering Plant - Tracked (SOA Notification - 2 clear working days)")]
            StgoEngPlantTrackedCat3Notification_Soa2 = 270146,
            [Description("STGO Engineering Plant - Wheeled (SOA Notification - 2 clear working days)")]
            StgoEngPlantWheeledCat3Notification_Soa2 = 270147,
            [Description("STGO Road Recovery (SOA Notification - 2 clear working days)")]
            StgoRoadRecoveryCat3Notification_Soa2 = 270148,
            [Description("STGO tracked CAT-3 Notification (SOA Notification - 2 clear working days)")]
            StgoTrackedCat3Notification_Soa2 = 270149,
            [Description("STGO Engineering Plant - Tracked (SOA Notification - 5 clear working days, Police Notification - 2 clear working days)")]
            StgoEngPlantTrackedCat3Notification_Soa5_Pol2 = 270150,
            [Description("STGO Engineering Plant - Wheeled (SOA Notification - 5 clear working days, Police Notification - 2 clear working days)")]
            StgoEngPlantWheeledCat3Notification_Soa5_Pol2 = 270151,
            [Description("STGO Road Recovery (SOA Notification - 5 clear working days, Police Notification - 2 clear working days)")]
            StgoRoadRecoveryCat3Notification_Soa5_Pol2 = 270152,
            [Description("STGO tracked CAT-3 Notification (SOA Notification - 5 clear working days, Police Notification - 2 clear working days)")]
            StgoTrackedCat3Notification_Soa5_Pol2 = 270153,
            [Description("C&U No Notification")]
            CandUNoNotification = 270154,
            [Description("No Notification: Vehicle is not classified as a crane, run rigid vehicle checks")]
            NoCrane = 270155,
            [Description("STGO CAT 1 VR-1 (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)")]
            StgoailCat1Vr1_Soa2_Pol2 = 270156,
            [Description("STGO CAT 3 Notification (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)")]
            StgoailCat3_Soa2_Pol2 = 270157,
            [Description("STGO VR-1 Engineering Plant - Wheeled (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)")]
            StgoEngPlantWheeledCat1Vr1Notification_Soa2_Pol2 = 270158,
            [Description("STGO VR-1 Engineering Plant - Wheeled (SOA Notification - 2 clear working days)")]
            StgoEngPlantWheeledCat1Vr1Notification_Soa2 = 270159,
            [Description("STGO VR-1 Engineering Plant - Wheeled (Police Notification - 2 clear working days)")]
            StgoEngPlantWheeledCat1Vr1Notification_Pol2 = 270160,
            [Description("STGO VR-1 Engineering Plant - Wheeled (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)")]
            StgoEngPlantWheeledCat2Vr1Notification_Soa2_Pol2 = 270161,
            [Description("STGO VR-1 Engineering Plant - Wheeled (SOA Notification - 2 clear working days)")]
            StgoEngPlantWheeledCat2Vr1Notification_Soa2 = 270162,
            [Description("STGO VR-1 Engineering Plant - Wheeled (SOA Notification - 5 clear working days, Police Notification - 2 clear working days)")]
            StgoEngPlantWheeledCat3Vr1Notification_Soa5_Pol2 = 270163,
            [Description("STGO VR-1 Engineering Plant - Wheeled (SOA Notification - 2 clear working days)")]
            StgoEngPlantWheeledCat3Vr1Notification_Soa2 = 270164,
            [Description("STGO VR-1 Engineering Plant - Tracked (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)")]
            StgoEngPlantTrackedCat1Vr1Notification_Soa2_Pol2 = 270165,
            [Description("STGO VR-1 Engineering Plant - Tracked (SOA Notification - 2 clear working days)")]
            StgoEngPlantTrackedCat1Vr1Notification_Soa2 = 270166,
            [Description("STGO VR-1 Engineering Plant - Tracked (Police Notification - 2 clear working days)")]
            StgoEngPlantTrackedCat1Vr1Notification_Pol2 = 270167,
            [Description("STGO VR-1 Engineering Plant - Tracked (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)")]
            StgoEngPlantTrackedCat2Vr1Notification_Soa2_Pol2 = 270168,
            [Description("STGO VR-1 Engineering Plant - Tracked (SOA Notification - 2 clear working days)")]
            StgoEngPlantTrackedCat2Vr1Notification_Soa2 = 270169,
            [Description("STGO VR-1 Engineering Plant - Tracked (SOA Notification - 5 clear working days, Police Notification - 2 clear working days)")]
            StgoEngPlantTrackedCat3Vr1Notification_Soa5_Pol2 = 270170,
            [Description("STGO VR-1 Engineering Plant - Tracked (SOA Notification - 2 clear working days)")]
            StgoEngPlantTrackedCat3Vr1Notification_Soa2 = 270171,
            [Description("STGO VR-1 Road Recovery (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)")]
            StgoRoadRecoveryCat1Vr1Notification_Soa2_Pol2 = 270172,
            [Description("STGO VR-1 Road Recovery (SOA Notification - 2 clear working days)")]
            StgoRoadRecoveryCat1Vr1Notification_Soa2 = 270173,
            [Description("STGO VR-1 Road Recovery (Police Notification - 2 clear working days)")]
            StgoRoadRecoveryCat1Vr1Notification_Pol2 = 270174,
            [Description("STGO VR-1 Road Recovery (SOA Notification - 2 clear working days, Police Notification - 2 clear working days)")]
            StgoRoadRecoveryCat2Vr1Notification_Soa2_Pol2 = 270175,
            [Description("STGO VR-1 Road Recovery (SOA Notification - 2 clear working days)")]
            StgoRoadRecoveryCat2Vr1Notification_Soa2 = 270176,
            [Description("STGO VR-1 Road Recovery (SOA Notification - 5 clear working days, Police Notification - 2 clear working days)")]
            StgoRoadRecoveryCat3Vr1Notification_Soa5_Pol2 = 270177,
            [Description("STGO VR-1 Road Recovery (SOA Notification - 2 clear working days)")]
            StgoRoadRecoveryCat3Vr1Notification_Soa2 = 270178
        }

        public enum VehicleXmlMovementType
        {
            [Description("Wheeled construction and use")]
            WheeledConstructionAndUse = 270001,
            [Description("stgo engineering plant wheeled")]
            StgoEngineeringPlantWheeled = 270004,
            [Description("stgo road recovery vehicle")]
            StgoRoadRecoveryVehicle = 270005,
            [Description("Special Order")]
            SpecialOrder = 270006,
            [Description("Vehicle Special Order")]
            VehicleSpecialOrder = 270007,
            [Description("Tracked")]
            Tracked = 270008,
            [Description("Stgo-ail Cat1")]
            StgoailCat1 = 270009,
            [Description("Stgo-ail Cat2")]
            StgoailCat2 = 270010,
            [Description("Stgo-ail Cat3")]
            StgoailCat3 = 270011,
            [Description("stgo mobile crane cat a")]
            StgoMobileCraneCata = 270012,
            [Description("stgo mobile crane cat b")]
            StgoMobileCraneCatb = 270013,
            [Description("stgo mobile crane cat c")]
            StgoMobileCraneCatc = 270014,
            [Description("stgo engineering plant tracked")]
            StgoEngineeringPlantTracked = 270015,
            [Description("No Vehicle Classification")]
            NoVehicleClassification = 270017,
            [Description("stgo vr-1 engineering plant wheeled")]
            StgoVr1EngineeringPlantWheeled = 270018,
            [Description("stgo vr-1 road recovery vehicle")]
            StgoVr1RoadRecoveryVehicle = 270019,
            [Description("stgo vr-1 engineering plant tracked")]
            StgoVr1EngineeringPlantTracked = 270020,
            

            [Description("No Notification")]
            NoNotification = 270101,
            [Description("C&U Notification POL-2")]
            CandUNotification_Pol2 = 270102,
            [Description("STGO CAT 1 Notification POL-2")]
            StgoailCat1_Pol2 = 270103,
            [Description("STGO CAT 1 Notification SOA-2")]
            StgoailCat1_Soa2 = 270104,
            [Description("STGO CAT 1 Notification SOA-2 POL-2")]
            StgoailCat1_Soa2_Pol2 = 270105,
            [Description("STGO CAT 2 Notification SOA-2")]
            StgoailCat2_Soa2 = 270106,
            [Description("STGO CAT 2 Notification SOA-2 POL-2")]
            StgoailCat2_Soa2_Pol2 = 270107,
            [Description("STGO CAT 3 Notification SOA-2")]
            StgoailCat3_Soa2 = 270108,
            [Description("STGO CAT 3 Notification SOA-5 POL-2")]
            StgoailCat3_Soa5_Pol2 = 270109,
            [Description("STGO CAT 1 VR-1 POL-2")]
            StgoailCat1Vr1_Pol2 = 270110,
            [Description("STGO CAT 2 VR-1 SOA-2 POL-2")]
            StgoailCat2Vr1_Soa2_Pol2 = 270111,
            [Description("STGO CAT 3 VR-1 SOA-5 POL-2")]
            StgoailCat3Vr1_Soa5_Pol2 = 270112,
            [Description("VSO Notification")]
            VSONotification = 270113,
            [Description("SO Notification")]
            SONotification = 270114,
            [Description("Tracked Vehicles Notification")]
            TrackedVehicleNotification = 270115,
            [Description("SO Application")]
            SOApplication = 270116,
            [Description("VR-1 Notification")]
            VR1Application = 270117,
            [Description("STGO Mobile Crane CAT A SOA-2")]
            StgoMobileCraneCatA_Soa2 = 270118,
            [Description("STGO Mobile Crane CAT A SOA-2 POL-2")]
            StgoMobileCraneCatA_Soa2_Pol2 = 270119,
            [Description("STGO Mobile Crane CAT B SOA-2")]
            StgoMobileCraneCatB_Soa2 = 270120,
            [Description("STGO Mobile Crane CAT B SOA-2 POL-2")]
            StgoMobileCraneCatB_Soa2_Pol2 = 270121,
            [Description("STGO Mobile Crane CAT B SOA-5 POL-2")]
            StgoMobileCraneCatB_Soa5_Pol2 = 270122,
            [Description("STGO Mobile Crane CAT C SOA-2")]
            StgoMobileCraneCatC_Soa2 = 270123,
            [Description("STGO Mobile Crane CAT C SOA-2 POL-2")]
            StgoMobileCraneCatC_Soa2_Pol2 = 270124,
            [Description("STGO Mobile Crane CAT C SOA-5 POL-2")]
            StgoMobileCraneCatC_Soa5_Pol2 = 270125,
            [Description("STGO engineering plant tracked CAT 1 Notification (POL-2)")]
            StgoEngPlantTrackedCat1Notification_Pol2 = 270126,
            [Description("STGO engineering plant wheeled CAT 1 Notification (POL-2)")]
            StgoEngPlantWheeledCat1Notification_Pol2 = 270127,
            [Description("STGO road recovery vehicle CAT 1 Notification (POL-2)")]
            StgoRoadRecoveryCat1Notification_Pol2 = 270128,
            [Description("STGO tracked CAT 1 Notification (POL-2)")]
            StgoTrackedCat1Notification_Pol2 = 270129,
            [Description("STGO engineering plant tracked CAT 1 Notification (SOA – 2)")]
            StgoEngPlantTrackedCat1Notification_Soa2 = 270130,
            [Description("STGO engineering plant wheeled CAT 1 Notification (SOA – 2)")]
            StgoEngPlantWheeledCat1Notification_Soa2 = 270131,
            [Description("STGO road recovery vehicle CAT 1 Notification (SOA – 2)")]
            StgoRoadRecoveryCat1Notification_Soa2 = 270132,
            [Description("STGO tracked CAT 1 Notification (SOA – 2)")]
            StgoTrackedCat1Notification_Soa2 = 270133,
            [Description("STGO engineering plant tracked CAT 1 Notification (SOA – 2, POL-2)")]
            StgoEngPlantTrackedCat1Notification_Soa2_Pol2 = 270134,
            [Description("STGO engineering plant wheeled CAT 1 Notification (SOA – 2, POL-2)")]
            StgoEngPlantWheeledCat1Notification_Soa2_Pol2 = 270135,
            [Description("STGO road recovery vehicle CAT 1 Notification (SOA – 2, POL-2)")]
            StgoRoadRecoveryCat1Notification_Soa2_Pol2 = 270136,
            [Description("STGO tracked CAT 1 Notification (SOA – 2, POL-2)")]
            StgoTrackedCat1Notification_Soa2_Pol2 = 270137,
            [Description("STGO engineering plant tracked CAT-2 Notification (SOA-2)")]
            StgoEngPlantTrackedCat2Notification_Soa2 = 270138,
            [Description("STGO engineering plant wheeled CAT-2 Notification (SOA-2)")]
            StgoEngPlantWheeledCat2Notification_Soa2 = 270139,
            [Description("STGO road recovery vehicle CAT-2 Notification (SOA-2)")]
            StgoRoadRecoveryCat2Notification_Soa2 = 270140,
            [Description("STGO tracked CAT-2 Notification (SOA-2)")]
            StgoTrackedCat2Notification_Soa2 = 270141,
            [Description("STGO engineering plant tracked CAT-2 Notification (SOA – 2, POL-2)")]
            StgoEngPlantTrackedCat2Notification_Soa2_Pol2 = 270142,
            [Description("STGO engineering plant wheeled CAT-2 Notification (SOA – 2, POL-2)")]
            StgoEngPlantWheeledCat2Notification_Soa2_Pol2 = 270143,
            [Description("STGO road recovery vehicle CAT-2 Notification (SOA – 2, POL-2)")]
            StgoRoadRecoveryCat2Notification_Soa2_Pol2 = 270144,
            [Description("STGO tracked CAT-2 Notification (SOA – 2, POL-2)")]
            StgoTrackedCat2Notification_Soa2_Pol2 = 270145,
            [Description("STGO engineering plant tracked CAT-3 Notification (SOA – 2)")]
            StgoEngPlantTrackedCat3Notification_Soa2 = 270146,
            [Description("STGO engineering plant wheeled CAT-3 Notification (SOA – 2)")]
            StgoEngPlantWheeledCat3Notification_Soa2 = 270147,
            [Description("STGO road recovery vehicle CAT-3 Notification (SOA – 2)")]
            StgoRoadRecoveryCat3Notification_Soa2 = 270148,
            [Description("STGO tracked CAT-3 Notification (SOA – 2)")]
            StgoTrackedCat3Notification_Soa2 = 270149,
            [Description("STGO engineering plant tracked CAT-3 Notification (SOA – 5, POL-2)")]
            StgoEngPlantTrackedCat3Notification_Soa5_Pol2 = 270150,
            [Description("STGO engineering plant wheeled CAT-3 Notification (SOA – 5, POL-2)")]
            StgoEngPlantWheeledCat3Notification_Soa5_Pol2 = 270151,
            [Description("STGO road recovery vehicle CAT-3 Notification (SOA – 5, POL-2)")]
            StgoRoadRecoveryCat3Notification_Soa5_Pol2 = 270152,
            [Description("STGO tracked CAT-3 Notification (SOA – 5, POL-2)")]
            StgoTrackedCat3Notification_Soa5_Pol2 = 270153,
            [Description("C&U No Notification")]
            CandUNoNotification = 270154,
            [Description("STGO CAT 1 VR-1 SOA - 2, POL-2")]
            StgoailCat1Vr1_Soa2_Pol2 = 270156,
            [Description("STGO CAT 3 SOA-2 POL-2")]
            StgoailCat3_Soa2_Pol2 = 270157,
            [Description("STGO VR-1 Engineering Plant - Wheeled (SOA - 2, POL - 2)")]
            StgoEngPlantWheeledCat1Vr1Notification_Soa2_Pol2 = 270158,
            [Description("STGO VR-1 Engineering Plant - Wheeled (SOA - 2)")]
            StgoEngPlantWheeledCat1Vr1Notification_Soa2 = 270159,
            [Description("STGO VR-1 Engineering Plant - Wheeled (POL - 2)")]
            StgoEngPlantWheeledCat1Vr1Notification_Pol2 = 270160,
            [Description("STGO VR-1 Engineering Plant - Wheeled (SOA - 2, POL - 2)")]
            StgoEngPlantWheeledCat2Vr1Notification_Soa2_Pol2 = 270161,
            [Description("STGO VR-1 Engineering Plant - Wheeled (SOA - 2)")]
            StgoEngPlantWheeledCat2Vr1Notification_Soa2 = 270162,
            [Description("STGO VR-1 Engineering Plant - Wheeled (SOA - 5, POL - 2)")]
            StgoEngPlantWheeledCat3Vr1Notification_Soa5_Pol2 = 270163,
            [Description("STGO VR-1 Engineering Plant - Wheeled (SOA - 2)")]
            StgoEngPlantWheeledCat3Vr1Notification_Soa2 = 270164,
            [Description("STGO VR-1 Engineering Plant - Tracked (SOA - 2, POL - 2)")]
            StgoEngPlantTrackedCat1Vr1Notification_Soa2_Pol2 = 270165,
            [Description("STGO VR-1 Engineering Plant - Tracked (SOA - 2)")]
            StgoEngPlantTrackedCat1Vr1Notification_Soa2 = 270166,
            [Description("STGO VR-1 Engineering Plant - Tracked (POL - 2)")]
            StgoEngPlantTrackedCat1Vr1Notification_Pol2 = 270167,
            [Description("STGO VR-1 Engineering Plant - Tracked (SOA - 2, POL - 2)")]
            StgoEngPlantTrackedCat2Vr1Notification_Soa2_Pol2 = 270168,
            [Description("STGO VR-1 Engineering Plant - Tracked (SOA - 2)")]
            StgoEngPlantTrackedCat2Vr1Notification_Soa2 = 270169,
            [Description("STGO VR-1 Engineering Plant - Tracked (SOA - 5, POL - 2)")]
            StgoEngPlantTrackedCat3Vr1Notification_Soa5_Pol2 = 270170,
            [Description("STGO VR-1 Engineering Plant - Tracked (SOA - 2)")]
            StgoEngPlantTrackedCat3Vr1Notification_Soa2 = 270171,
            [Description("STGO VR-1 Engineering Plant - Tracked (SOA - 2, POL - 2)")]
            StgoRoadRecoveryCat1Vr1Notification_Soa2_Pol2 = 270172,
            [Description("STGO VR-1 Engineering Plant - Tracked (SOA - 2)")]
            StgoRoadRecoveryCat1Vr1Notification_Soa2 = 270173,
            [Description("STGO VR-1 Engineering Plant - Tracked (POL - 2)")]
            StgoRoadRecoveryCat1Vr1Notification_Pol2 = 270174,
            [Description("STGO VR-1 Engineering Plant - Tracked (SOA - 2, POL - 2)")]
            StgoRoadRecoveryCat2Vr1Notification_Soa2_Pol2 = 270175,
            [Description("STGO VR-1 Engineering Plant - Tracked (SOA - 2)")]
            StgoRoadRecoveryCat2Vr1Notification_Soa2 = 270176,
            [Description("STGO VR-1 Engineering Plant - Tracked (SOA - 5, POL - 2)")]
            StgoRoadRecoveryCat3Vr1Notification_Soa5_Pol2 = 270177,
            [Description("STGO VR-1 Engineering Plant - Tracked (SOA - 2)")]
            StgoRoadRecoveryCat3Vr1Notification_Soa2 = 270178
        }

        public enum VehicleMovementTypeMain
        {
            [Description("Wheeled construction and use")]
            WheeledConstructionAndUse = 201,
            [Description("Special Order")]
            SpecialOrder = 202,
            [Description("Vehicle Special Order")]
            VehicleSpecialOrder = 203,
            [Description("Tracked")]
            Tracked = 204,
            [Description("Stgo-vr1")]
            Stgovr1 = 205,
            [Description("Stgo")]
            Stgo = 206,
            [Description("stgo mobile crane")]
            StgoMobileCrane = 207

        }
    }
}
