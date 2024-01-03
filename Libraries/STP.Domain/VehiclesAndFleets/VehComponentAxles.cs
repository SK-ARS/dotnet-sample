using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.VehiclesAndFleets
{
    public class VehComponentAxles
    {
        private long componentId;
        private Int16 axleCount;
        private Int16 wheelCount;
        private string tyreSize;
        private string wheelSpacingList;

        private float weight;
        private float nextAxleDist;

        private decimal nextAxleDistNoti;

        private double axleSpacingToFollowing { get; set; }

        private Int16 axleNumber;

        public long ComponentId
        {
            get { return componentId; }
            set { componentId = value; }
        }

        public Int16 AxleCount
        {
            get { return axleCount; }
            set { axleCount = value; }
        }

        public Int16 WheelCount
        {
            get { return wheelCount; }
            set { wheelCount = value; }
        }

        public string TyreSize
        {
            get { return tyreSize; }
            set { tyreSize = value; }
        }

        public string WheelSpacingList
        {
            get { return wheelSpacingList; }
            set { wheelSpacingList = value; }
        }

        public float Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public float NextAxleDist
        {
            get { return nextAxleDist; }
            set { nextAxleDist = value; }
        }

        public decimal NextAxleDistNoti
        {
            get { return nextAxleDistNoti; }
            set { nextAxleDistNoti = value; }
        }

        public Int16 AxleNumber
        {

            get { return axleNumber; }
            set { axleNumber = value; }
        }

        public double AxleSpacingToFollowing
        {
            get { return axleSpacingToFollowing; }
            set { axleSpacingToFollowing = value; }
        }
    }
}