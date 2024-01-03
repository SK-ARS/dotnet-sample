using STP.Domain.Applications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Web.General
{
    public class ApplicationGroupingFunction
    {
        public List<ComponentObjListModelToreturn> Grouping(List<ComponentGroupingModel> componentObjList)
        {
            List<string> Names = componentObjList.Select(x => x.VehicleDesc).Distinct().ToList();
            List<ComponentObjListModelToreturn> componentObjListModelToreturn = new List<ComponentObjListModelToreturn>();
            for (int i = 0; i < Names.Count; i++)
            {
                componentObjListModelToreturn.Add(new ComponentObjListModelToreturn()
                {
                    VehicleDesc = Names[i],
                    GrossWeight = componentObjList.Where(x => x.VehicleDesc == Names[i]).First().GrossWeight,
                    MaxAxleWeight = componentObjList.Where(x => x.VehicleDesc == Names[i]).First().MaxAxleWeight,

                    AxleWeight = GetAxleWeight(componentObjList.Where(x => x.VehicleDesc == Names[i]).ToList()),
                    WheelsPerAxle = GetWheelsPerAxle(componentObjList.Where(x => x.VehicleDesc == Names[i]).ToList()),
                    AxleSpacing = GetAxleSpacing(componentObjList.Where(x => x.VehicleDesc == Names[i]).ToList()),
                    AxleSpacing2 = GetAxleSpacing2(componentObjList.Where(x => x.VehicleDesc == Names[i]).ToList()),
                    TyreSize = GetTyreSize(componentObjList.Where(x => x.VehicleDesc == Names[i]).ToList()),
                    TyreCentreSpacing = GetTyreCentreSpacing(componentObjList.Where(x => x.VehicleDesc == Names[i]).ToList()),

                    WheelBase = componentObjList.Where(x => x.VehicleDesc == Names[i]).First().Wheelbase,
                    RearOverhang = GetRearOverhang(componentObjList.Where(x => x.VehicleDesc == Names[i]).ToList()),
                    OutsideTrack = GetOutsideTrack(componentObjList.Where(x => x.VehicleDesc == Names[i]).ToList())
                });
            }
            return (componentObjListModelToreturn);
        }
        public string GetAxleWeight(List<ComponentGroupingModel> componentObjList)
        {
            string AxleWeight = string.Empty;
            var g = componentObjList.GroupBy(i => i.AxleWeight);
            foreach (var grp in g)
            {
                if (grp.Count() == 1)
                {
                    AxleWeight = AxleWeight + ", " + grp.Key.Value.ToString("#,##0.") + " kg";
                }
                else if (grp.Key == 0)
                {
                }
                else
                {
                    AxleWeight = AxleWeight + ", " + grp.Key.Value.ToString("#,##0.") + " kg" + " x " + grp.Count();
                }
            }
            return AxleWeight.Length > 1 ? AxleWeight.Substring(1) : AxleWeight;
        }
        public VehicleDetail ViewSummary(List<VehicleDetail> objVehicleDetail, out string savename, out string GrossWeight, out string MaxAcelWeight, out string Registration)
        {
            VehicleDetail objectVehicleDetail = new VehicleDetail();

            if (objVehicleDetail != null && objVehicleDetail.Count > 0)
            {
                objectVehicleDetail = objVehicleDetail[0];


                double temp = 0.0;
                foreach (var obj in objVehicleDetail)
                {
                    if (obj.Rear_Overhang != 0.0)
                    {
                        temp = obj.Rear_Overhang;
                    }
                }
                objectVehicleDetail.Rear_Overhang = temp;


                savename = string.Empty;
                List<string> Names = objVehicleDetail.Select(x => x.Vehicle_Name).Distinct().ToList();
                for (int i = 0; i < Names.Count; i++)
                {
                    savename += Names[i] + " or <br/>";
                }
                savename = savename.Length > 1 ? savename.Remove(savename.Length - 9, 9) : savename;
                List<double> GrossWeightList = objVehicleDetail.Select(x => x.Gross_Weight).Distinct().ToList();
                GrossWeight = string.Empty;
                for (int i = 0; i < GrossWeightList.Count; i++)
                {
                    GrossWeight += GrossWeightList[i] + " kg" + " or ";
                }
                GrossWeight = GrossWeight.Length > 1 ? GrossWeight.Remove(GrossWeight.Length - 3, 3) : GrossWeight;
                MaxAcelWeight = string.Empty;
                List<double> Max_Axle_WeightList = objVehicleDetail.Select(x => x.Max_Axle_Weight).Distinct().ToList();
                for (int i = 0; i < Max_Axle_WeightList.Count; i++)
                {
                    MaxAcelWeight += Max_Axle_WeightList[i] + " kg" + " or  ";
                }

                MaxAcelWeight = MaxAcelWeight.Remove(MaxAcelWeight.Length - 4, 4);

                Registration = string.Empty;
                List<string> RegistrationList = objVehicleDetail.Select(x => x.Registration).Distinct().ToList();
                for (int i = 0; i < RegistrationList.Count; i++)
                {
                    Registration += RegistrationList[i] + "  , ";
                }

                Registration = Registration.Remove(Registration.Length - 4, 4);
            }
            else
            {
                savename = string.Empty;
                GrossWeight = "";
                MaxAcelWeight = string.Empty;
                Registration = string.Empty;
            }

            return objectVehicleDetail;
        }
        #region  Private methods
        private string GetWheelsPerAxle(List<ComponentGroupingModel> componentObjList)
        {
            string WheelsPerAxle = string.Empty;
            var g = componentObjList.GroupBy(i => i.WheelsPerAxle);
            foreach (var grp in g)
            {
                if (grp.Count() == 1)
                {
                    WheelsPerAxle = Convert.ToString(grp.Key);
                }
                else if (grp.Key == 0)
                {
                }
                else
                {
                    WheelsPerAxle = WheelsPerAxle + ", " + grp.Key + " x " + grp.Count();
                }
            }
            if (WheelsPerAxle[0] == ',')
            {
                WheelsPerAxle = WheelsPerAxle.Remove(0, 1);
            }
            return WheelsPerAxle;

        }
        private string GetAxleSpacing(List<ComponentGroupingModel> componentObjList)
        {
            string AxleSpacing = string.Empty;
            var g = componentObjList.GroupBy(i => i.AxleSpacing);
            foreach (var grp in g)
            {
                if (grp.Count() == 1)
                {
                    AxleSpacing = AxleSpacing + ", " + grp.Key + " m";
                }
                else if (grp.Key == 0)
                {
                }
                else
                {
                    AxleSpacing = AxleSpacing + ", " + grp.Key + " m" + " x " + grp.Count();
                }
            }
            return AxleSpacing.Length > 1 ? AxleSpacing.Substring(1) : AxleSpacing;
        }


        private string GetAxleSpacing2(List<ComponentGroupingModel> componentObjList)
        {
            string AxleSpacing2 = string.Empty;
            var g = componentObjList.GroupBy(i => i.AxleSpacing2);
            foreach (var grp in g)
            {
                if (grp.Count() == 1 && grp.Key != 0.0)
                {
                    AxleSpacing2 = AxleSpacing2 + ", " + grp.Key + " m";
                }
                else if (grp.Key == 0)
                {
                }
                else
                {
                    AxleSpacing2 = AxleSpacing2 + ", " + grp.Key + " m";// +" x " + grp.Count();
                }
            }
            return AxleSpacing2.Length > 1 ? AxleSpacing2.Substring(1) : AxleSpacing2;
        }
        private string GetTyreSize(List<ComponentGroupingModel> componentObjList)
        {
            string TyreSize = string.Empty;
            var g = componentObjList.GroupBy(i => i.TyreSize);
            foreach (var grp in g)
            {
                if (grp.Count() == 1)
                {
                    TyreSize = TyreSize + ", " + grp.Key;
                }

                else
                {
                    TyreSize = TyreSize + ", " + grp.Key + " x " + grp.Count();
                }
            }
            return TyreSize.Length > 1 ? TyreSize.Substring(1) : TyreSize;

        }
        private string GetTyreCentreSpacing(List<ComponentGroupingModel> componentObjList)
        {
            string TyreCentreSpacing = string.Empty;
            var g = componentObjList.GroupBy(i => i.TyreCentreSpacing);
            foreach (var grp in g)
            {

                if (grp.Count() == 1)
                {
                    TyreCentreSpacing = TyreCentreSpacing + ", " + grp.Key + " m";
                }

                else
                {
                    TyreCentreSpacing = TyreCentreSpacing + ", " + grp.Key + " m" + " x " + grp.Count();
                }
            }
            return TyreCentreSpacing.Length > 1 ? TyreCentreSpacing.Substring(1) : TyreCentreSpacing;
        }
        private double? GetOutsideTrack(List<ComponentGroupingModel> componentObjList)
        {
            foreach (var obj in componentObjList)
            {
                if (obj.OutsideTrack != 0.0)
                {
                    return obj.OutsideTrack;
                }

            }
            return 0.0;
        }
        private double? GetRearOverhang(List<ComponentGroupingModel> componentObjList)
        {
            foreach (var obj in componentObjList)
            {
                if (obj.RearOverhang != 0.0)
                {
                    return obj.RearOverhang;
                }

            }
            return 0.0;
        }

        #endregion Private methods
    }
}