using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.VehiclesAndFleets
{
    public class VehicleSummary
    {
        private string vehicleName;

        private float maxAxleWeight;

        private float grossWeight;

        private float rigidLength;

        private float width;

        private bool isSteerableAtRear;

        private float maxHeight;

        private float redHeight;

        private float wheelBase;

        private float rearOverhang;

        private float outsideTrack;


        public float MaxAxleWeight
        {
            get { return maxAxleWeight; }
            set { maxAxleWeight = value; }
        }

        public float GrossWeight
        {
            get { return grossWeight; }
            set { grossWeight = value; }
        }

        public string VehicleName
        {
            get { return vehicleName; }
            set { vehicleName = value; }
        }

        public float RigidLength
        {
            get { return rigidLength; }
            set { rigidLength = value; }
        }

        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        public bool IsSteerableAtRear
        {
            get { return isSteerableAtRear; }
            set { isSteerableAtRear = value; }
        }

        public float MaxHeight
        {
            get { return maxHeight; }
            set { maxHeight = value; }
        }

        public float RedHeight
        {
            get { return redHeight; }
            set { redHeight = value; }
        }

        public float WheelBase
        {
            get { return wheelBase; }
            set { wheelBase = value; }
        }

        public float RearOverhang
        {
            get { return rearOverhang; }
            set { rearOverhang = value; }
        }

        public float OutsideTrack
        {
            get { return outsideTrack; }
            set { outsideTrack = value; }
        }
    }
}