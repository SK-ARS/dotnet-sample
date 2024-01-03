using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.VehiclesAndFleets
{
    public class VehicleEnums
    {
        public enum ComponentType
        {
            BallastTractor = 234001,
            ConventionalTractor = 234002,
            RigidVehicle = 234003,
            Tracked = 234004,
            SemiTrailer = 234005,
            DrawbarTrailer = 234006,
            SPMT = 234007
        }

        public enum ComponentSubType
        {
            BallastTractor = 224001,
            ConventionalTractor = 224002,
            OtherTractor = 224003,
            SemiTrailer = 224004,
            SemiLowLoader = 224005,
            TromboneTrailer = 224006,
            OtherSemiTrailer = 224007,
            DrawbarTrailer = 224008,
            OtherDrawbarTrailer = 224009,
            Bogie = 224010,
            TwinBogies = 224011,
            TrackedVehicle = 224012,
            RigidVehicle = 224013,
            SPMT = 224014,
            GirderSet = 224015,
            WheeledLoad = 224016,
            RecoveryVehicle = 224017,
            RecoveredVehicle = 224018,
            MobileCrane = 224019,
            EngineeingPlant = 224020
        }

        public enum CouplingType
        {
            None = 201001,
            FifthWheel = 201002,
            Drawbar = 201003,
            TowHitch = 201004
        }

        public enum ConfigurationType
        {
            DrawbarTrailer = 244001,
            SemiTrailer = 244002,
            Rigid = 244003,
            Tracked = 244004,
            SPMT = 244005,
            OtherInline = 244006,
            SidebySide = 244007
        }
    }
}
