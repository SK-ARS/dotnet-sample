using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.VehiclesAndFleets.Models
{
    public class VehicleEnumConversions
    {
        public double GetConvertedValues(int unit, double value)
        {
            double convertedValue;
            if (unit == 208006) // Feet
            {
                convertedValue = FeetToMeters(value);
            }
            else if (unit == 240003)// Pound
            {
                convertedValue = PoundToKilogram(value);
            }
            else if (unit == 229001)//Miles per hour
            {
                convertedValue = MphToKmph(value);
            }
            else
            {
                convertedValue = value;
            }
            return convertedValue;
        }

        #region Functions to convert Enums to corresponding values
        public string GetVehicleType(int VehicleType)
        {
            string vehicleType = "";
            switch (VehicleType)
            {
                case 244001:
                    return "Drawbar Vehicle";
                case 244002:
                    return "Semi Vehicle";
                case 244003:
                    return "Rigid Vehicle";
                case 244004:
                    return "Tracked Vehicle";
                case 244005:
                    return "SPMT";
                case 244006:
                    return "Other in line";
                case 244007:
                    return "Other side by side";
            }
            return vehicleType;
        }
        public string GetComponentType(int ComponentType)
        {
            string componentType = "";
            switch (ComponentType)
            {
                case 234001:
                    return "Ballast Tractor";
                case 234002:
                    return "Conventional Tractor";
                case 234003:
                    return "Rigid Vehicle";
                case 234004:
                    return "Tracked Vehicle";
                case 234005:
                    return "Semi Trailer";
                case 234006:
                    return "Drawbar Trailer";
                case 234007:
                    return "SPMT";
            }
            return componentType;
        }
        public string GetComponentSubType(int ComponentSubType)
        {
            string componentSubType = "";
            switch (ComponentSubType)
            {
                case 224001:
                    return "Ballast Tractor";
                case 224002:
                    return "Conventional Tractor";
                case 224003:
                    return "Other Tractor";
                case 224004:
                    return "Semi Trailer";
                case 224005:
                    return "Semi Low Loader";
                case 224006:
                    return "Trombone Trailer";
                case 224007:
                    return "Other Semi Trailer";
                case 224008:
                    return "Drawbar Trailer";
                case 224009:
                    return "Other Drawbar Trailer";
                case 224010:
                    return "Bogie";
                case 224011:
                    return "Twin Bogies";
                case 224012:
                    return "Tracked Vehicle";
                case 224013:
                    return "Rigid Vehicle";
                case 224014:
                    return "SPMT";
                case 224015:
                    return "Girder Set";
                case 224016:
                    return "Wheeled Load";
                case 224017:
                    return "Recovery Vehicle";
                case 224018:
                    return "Recovered Vehicle";
                case 224019:
                    return "Mobile Crane";
                case 224020:
                    return "Engineering Plant";
            }
            return componentSubType;
        }
        public string GetVehiclePurpose(int VehiclePurpose)
        {
            string vehiclePurpose = "";
            switch (VehiclePurpose)
            {
                case 270001:
                    return "C and U";
                case 270002:
                    return "STGO AIL";
                case 270003:
                    return "STGO Mobile Crane";
                case 270004:
                    return "STGO Engineering Plant Wheeled";
                case 270005:
                    return "STGO Road Recovery";
                case 270006:
                    return "Special Order";
                case 270007:
                    return "Vehicle Special Order";
                case 270008:
                    return "Tracked";
            }
            return vehiclePurpose;
        }
        public string GetVehicleDimensionUnit(int length)
        {
            string Length = "";
            switch (length)
            {
                case 208001:
                    return "Meter";
                case 208002:
                    return "Kilometer";
                case 208003:
                    return "Millimeter";
                case 208004:
                    return "Centimeter";
                case 208005:
                    return "Inch";
                case 208006:
                    return "Foot";
                case 208007:
                    return "Yard";
                case 208008:
                    return "Mile";
            }
            return Length;
        }
        public string GetWeightUnit(int weight)
        {
            string Weight = "";
            switch (weight)
            {
                case 240001:
                    return "Kilogram";
                case 240002:
                    return "Ton";
                case 240003:
                    return "Pound";
                case 240004:
                    return "HundredWeight";
                case 240005:
                    return "Tonne";
            }
            return Weight;
        }
        public string GetCouplingTypetUnit(int couplingType)
        {
            string CouplingType = "";
            switch (couplingType)
            {
                case 201001:
                    return "None";
                case 201002:
                    return "Fifth Wheel";
                case 201003:
                    return "Drawbar";
                case 201004:
                    return "Tow Hitch";
            }
            return CouplingType;
        }
        public string GetSpeedUnit(int speed)
        {
            string Speed = "";
            switch (speed)
            {
                case 229001:
                    return "Mph";
                case 229002:
                    return "Kph";
            }
            return Speed;
        }
        public string GetBoolResult(int? bitValue)
        {
            string boolValue = null;
            switch (bitValue)
            {
                case 0:
                    return "No";
                case 1:
                    return "Yes";
            }
            return boolValue;
        }
        #endregion

        #region Functions to convert values to corresponding Enums
        public int GetVehicleTypeId(string VehicleType)
        {
            int vehicleType = 0;
            switch (VehicleType)
            {
                case "drawbar vehicle":
                    return 244001;
                case "semi vehicle":
                    return 244002;
                case "rigid vehicle":
                    return 244003;
                case "tracked vehicle":
                    return 244004;
                case "spmt":
                    return 244005;
                case "other in line":
                    return 244006;
                case "other side by side":
                    return 244007;
            }
            return vehicleType;
        }
        public int GetComponentTypeId(string ComponentType)
        {
            int componentType = 0;
            switch (ComponentType)
            {
                case "ballast tractor":
                    return 234001;
                case "conventional tractor":
                    return 234002;
                case "rigid vehicle":
                    return 234003;
                case "tracked vehicle":
                    return 234004;
                case "semi trailer":
                    return 234005;
                case "drawbar trailer":
                    return 234006;
                case "spmt":
                    return 234007;
            }
            return componentType;
        }
        public int GetComponentSubTypeId(string ComponentSubType)
        {
            int componentSubType = 0;
            switch (ComponentSubType)
            {
                case "ballast tractor":
                    return 224001;
                case "conventional tractor":
                    return 224002;
                case "other tractor":
                    return 224003;
                case "semi trailer":
                    return 224004;
                case "semi low loader":
                    return 224005;
                case "trombone trailer":
                    return 224006;
                case "other semi trailer":
                    return 224007;
                case "drawbar trailer":
                    return 224008;
                case "other drawbar trailer":
                    return 224009;
                case "bogie":
                    return 224010;
                case "twin bogies":
                    return 224011;
                case "tracked vehicle":
                    return 224012;
                case "rigid vehicle":
                    return 224013;
                case "spmt":
                    return 224014;
                case "girder set":
                    return 224015;
                case "wheeled load":
                    return 224016;
                case "recovery vehicle":
                    return 224017;
                case "recovered vehicle":
                    return 224018;
                case "mobile crane":
                    return 224019;
                case "engineering plant":
                    return 224020;
            }
            return componentSubType;
        }
        public int GetMovementClassificationId(string MovementClassification)
        {
            int movementClassification = 0;
            switch (MovementClassification)
            {
                case "c and u":
                    return 270001;
                case "stgo ail":
                    return 270002;
                case "stgo mobile crane":
                    return 270003;
                case "stgo engineering plant wheeled":
                    return 270004;
                case "stgo road recovery":
                    return 270005;
                case "special order":
                    return 270006;
                case "vehicle special order":
                    return 270007;
                case "tracked":
                    return 270008;
            }
            return movementClassification;
        }
        public int GetCouplingTypetId(string couplingType)
        {
            int CouplingType = 0;
            switch (couplingType)
            {
                case "none":
                    return 201001;
                case "fifth wheel":
                    return 201002;
                case "drawbar":
                    return 201003;
                case "tow hitch":
                    return 201004;
            }
            return CouplingType;
        }
        public int GetBitResult(string boolValue)
        {
            int bitValue = 0;
            switch (boolValue)
            {
                case "no":
                    return 0;
                case "yes":
                    return 1;
            }
            return bitValue;
        }
        #endregion

        public int GetDimensionUnitId(string unit)
        {
            int Length;
            if (unit == "imperial system")
            {
                Length = 208006;
            }
            else
            {
                Length = 208001;
            }
            return Length;
        }
        public int GetSpeedUnitId(string unit)
        {
            int speed;
            if (unit == "imperial system")
            {
                speed = 229001;
            }
            else
            {
                speed = 229002;
            }
            return speed;
        }
        public int GetWeightUnitId(string unit)
        {
            int weight;
            if (unit == "imperial system")
            {
                weight = 240003;
            }
            else
            {
                weight = 240001;
            }
            return weight;
        }

        #region Function to convert Meters To Feet and vice versa
        public static double MetersToFeet(double Meters)
        {
            return Meters / 0.304800610;
        }

        public static double FeetToMeters(double Feet)
        {
            return Feet * 0.304800610;
        }
        #endregion

        #region Function to convert Km/hr to miles/hr and vice versa
        public static double KmphToMph(double kmph)
        {
            return 0.6214 * kmph;
        }
        public static double MphToKmph(double mph)
        {
            return mph * 1.60934;
        }
        #endregion

        #region Function to convert Kilogram to Pound and vice versa
        public static double KilogramToPound(double kilogram)
        {
            return 2.20462 * kilogram;
        }
        public static double PoundToKilogram(double pound)
        {
            return pound * 0.453592;
        }
        #endregion
    }
}