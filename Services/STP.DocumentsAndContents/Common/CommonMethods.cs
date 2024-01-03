using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using NotificationXSD;
using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.DocumentsAndContents.Document;
using STP.DocumentsAndContents.Persistance;
using STP.Domain.DocumentsAndContents;
using STP.Domain.SecurityAndUsers;
using STP.Domain.VehiclesAndFleets;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace STP.DocumentsAndContents.Common
{
    public class CommonMethods
    {
        #region GetAxleWeightListPosition
        public static NotificationXSD.SummaryAxleStructureAxleWeightListPosition[] GetAxleWeightListPosition(List<VehComponentAxles> vehicleComponentAxlesList)
        {
            List<NotificationXSD.SummaryAxleStructureAxleWeightListPosition> sasawlpList = new List<NotificationXSD.SummaryAxleStructureAxleWeightListPosition>();

            List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

            foreach (long component in componentList)
            {
                List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
                                                                            where apca.ComponentId == component
                                                                            orderby apca.AxleNumber ascending
                                                                            select apca).ToList();
                
                List<float> weightList = componentWiseAxleList.Select(x => x.Weight).ToList();

                int count = weightList.Count;
                int localCount = 0;

                float oldDummyweight = 0;
                int oldcountaAxles = 0;

                foreach (float weight in weightList)
                {
                    localCount = localCount + 1;

                    if (oldcountaAxles == 0)
                    {
                        oldDummyweight = weight;

                        oldcountaAxles = oldcountaAxles + 1;
                    }
                    else
                    {
                        if (weight == oldDummyweight)
                        {
                            oldcountaAxles = oldcountaAxles + 1;
                        }
                        else
                        {
                            NotificationXSD.SummaryAxleStructureAxleWeightListPosition sasawlp = new NotificationXSD.SummaryAxleStructureAxleWeightListPosition();
                            NotificationXSD.AxleWeightSummaryStructure awss = new NotificationXSD.AxleWeightSummaryStructure();

                            awss.Value = Convert.ToString(oldDummyweight);
                            awss.AxleCount = Convert.ToString(oldcountaAxles);

                            sasawlp.AxleWeight = awss;

                            sasawlpList.Add(sasawlp);

                            oldcountaAxles = 1;
                            oldDummyweight = weight;
                        }
                    }

                    if (localCount == count)
                    {
                        NotificationXSD.SummaryAxleStructureAxleWeightListPosition sasawlp = new NotificationXSD.SummaryAxleStructureAxleWeightListPosition();
                        NotificationXSD.AxleWeightSummaryStructure awss = new NotificationXSD.AxleWeightSummaryStructure();

                        awss.Value = Convert.ToString(oldDummyweight);
                        awss.AxleCount = Convert.ToString(oldcountaAxles);

                        sasawlp.AxleWeight = awss;

                        sasawlpList.Add(sasawlp);
                    }
                }


            }

            if (sasawlpList.ToArray().Length == 0)
            {
                NotificationXSD.SummaryAxleStructureAxleWeightListPosition sasawlp = new NotificationXSD.SummaryAxleStructureAxleWeightListPosition();
                NotificationXSD.AxleWeightSummaryStructure awss = new NotificationXSD.AxleWeightSummaryStructure();
                awss.Value = "0";
                sasawlp.AxleWeight = awss;
                sasawlpList.Add(sasawlp);
            }

            return sasawlpList.ToArray();
        }
        #endregion

        #region GetWheelsPerAxleListPosition
        public static NotificationXSD.SummaryAxleStructureWheelsPerAxleListPosition[] GetWheelsPerAxleListPosition(List<VehComponentAxles> vehicleComponentAxlesList)
        {
            List<NotificationXSD.SummaryAxleStructureWheelsPerAxleListPosition> saswpalpList = new List<NotificationXSD.SummaryAxleStructureWheelsPerAxleListPosition>();

            List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

            foreach (long component in componentList)
            {
                List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
                                                                            where apca.ComponentId == component
                                                                            orderby apca.AxleNumber ascending
                                                                            select apca).ToList();
                
                List<short> weightList = componentWiseAxleList.Select(x => x.WheelCount).ToList();

                int count = weightList.Count;
                int localCount = 0;

                short oldDummyweight = 0;
                int oldcountaAxles = 0;

                foreach (short weight in weightList)
                {
                    localCount = localCount + 1;

                    if (oldcountaAxles == 0)
                    {
                        oldDummyweight = weight;

                        oldcountaAxles = oldcountaAxles + 1;
                    }
                    else
                    {
                        if (weight == oldDummyweight)
                        {
                            oldcountaAxles = oldcountaAxles + 1;
                        }
                        else
                        {
                            NotificationXSD.SummaryAxleStructureWheelsPerAxleListPosition saswpalp = new NotificationXSD.SummaryAxleStructureWheelsPerAxleListPosition();
                            NotificationXSD.WheelsPerAxleSummaryStructure wpass = new NotificationXSD.WheelsPerAxleSummaryStructure();

                            wpass.Value = Convert.ToString(oldDummyweight);
                            wpass.AxleCount = Convert.ToString(oldcountaAxles);

                            saswpalp.WheelsPerAxle = wpass;

                            saswpalpList.Add(saswpalp);

                            oldcountaAxles = 1;
                            oldDummyweight = weight;
                        }
                    }

                    if (localCount == count)
                    {
                        NotificationXSD.SummaryAxleStructureWheelsPerAxleListPosition saswpalp = new NotificationXSD.SummaryAxleStructureWheelsPerAxleListPosition();
                        NotificationXSD.WheelsPerAxleSummaryStructure wpass = new NotificationXSD.WheelsPerAxleSummaryStructure();

                        wpass.Value = Convert.ToString(oldDummyweight);
                        wpass.AxleCount = Convert.ToString(oldcountaAxles);

                        saswpalp.WheelsPerAxle = wpass;

                        saswpalpList.Add(saswpalp);
                    }
                }
            }

            if (saswpalpList.ToArray().Length == 0)
            {
                NotificationXSD.SummaryAxleStructureWheelsPerAxleListPosition saswpalp = new NotificationXSD.SummaryAxleStructureWheelsPerAxleListPosition();
                NotificationXSD.WheelsPerAxleSummaryStructure wpass = new NotificationXSD.WheelsPerAxleSummaryStructure();
                wpass.Value = "0";
                saswpalp.WheelsPerAxle = wpass;
                saswpalpList.Add(saswpalp);
            }

            return saswpalpList.ToArray();
        }
        #endregion

        #region GetAxleSpacingListPositionAxleSpacing
        public static NotificationXSD.SummaryAxleStructureAxleSpacingListPosition[] GetAxleSpacingListPositionAxleSpacing(List<VehComponentAxles> vehicleComponentAxlesList)
        {
            List<NotificationXSD.SummaryAxleStructureAxleSpacingListPosition> sasaslpList = new List<NotificationXSD.SummaryAxleStructureAxleSpacingListPosition>();

            List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

            foreach (long component in componentList)
            {
                List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
                                                                            where apca.ComponentId == component
                                                                            orderby apca.AxleNumber ascending
                                                                            select apca).ToList();                

                List<decimal> axleSpacingList = componentWiseAxleList.Select(x => x.NextAxleDistNoti).ToList();

                int count = axleSpacingList.Count;
                int localCount = 0;

                decimal oldDummyweight = 0;
                int oldcountaAxles = 0;

                foreach (decimal axleSpacing in axleSpacingList)
                {
                    localCount = localCount + 1;

                    if (axleSpacing != 0)
                    {
                        if (oldcountaAxles == 0)
                        {
                            oldDummyweight = axleSpacing;

                            oldcountaAxles = oldcountaAxles + 1;
                        }
                        else
                        {
                            if (axleSpacing == oldDummyweight)
                            {
                                oldcountaAxles = oldcountaAxles + 1;
                            }
                            else
                            {
                                NotificationXSD.SummaryAxleStructureAxleSpacingListPosition sasaslp = new NotificationXSD.SummaryAxleStructureAxleSpacingListPosition();
                                NotificationXSD.AxleSpacingSummaryStructure ass = new NotificationXSD.AxleSpacingSummaryStructure();

                                ass.Value = oldDummyweight;
                                ass.AxleCount = Convert.ToString(oldcountaAxles);
                                sasaslp.AxleSpacing = ass;
                                sasaslpList.Add(sasaslp);

                                oldcountaAxles = 1;
                                oldDummyweight = axleSpacing;
                            }
                        }
                    }

                    if (localCount == count)
                    {
                        NotificationXSD.SummaryAxleStructureAxleSpacingListPosition sasaslp = new NotificationXSD.SummaryAxleStructureAxleSpacingListPosition();
                        NotificationXSD.AxleSpacingSummaryStructure ass = new NotificationXSD.AxleSpacingSummaryStructure();

                        ass.Value = oldDummyweight;
                        ass.AxleCount = Convert.ToString(oldcountaAxles);
                        sasaslp.AxleSpacing = ass;
                        sasaslpList.Add(sasaslp);
                    }
                }
            }

            if (sasaslpList.ToArray().Length == 0)
            {
                NotificationXSD.SummaryAxleStructureAxleSpacingListPosition sasaslp = new NotificationXSD.SummaryAxleStructureAxleSpacingListPosition();
                NotificationXSD.AxleSpacingSummaryStructure ass = new NotificationXSD.AxleSpacingSummaryStructure();
                ass.Value = 0;
                sasaslp.AxleSpacing = ass;
                sasaslpList.Add(sasaslp);
            }

            return sasaslpList.ToArray();
        }
        #endregion

        #region GetAxleSpacingToFollowListPositionAxleSpacing
        // Added RM#4386
        public static SummaryAxleStructureAxleSpacingToFollowListPosition[] GetAxleSpacingToFollowListPositionAxleSpacing(List<VehComponentAxles> vehicleComponentAxlesList, double FirstComponentAxleSpaceToFollow)
        {
            List<SummaryAxleStructureAxleSpacingToFollowListPosition> sasaslpList = new List<SummaryAxleStructureAxleSpacingToFollowListPosition>();

            if (FirstComponentAxleSpaceToFollow == 0)// RM#4386 Condition to show max axle value when the vehicle is manually added
            {
                SummaryAxleStructureAxleSpacingToFollowListPosition sasaslp = new SummaryAxleStructureAxleSpacingToFollowListPosition();
                AxleSpacingToFollowSummaryStructure ass = new AxleSpacingToFollowSummaryStructure();

                if (vehicleComponentAxlesList.ToArray().Length == 0)
                {
                    ass.Value = 0;
                    sasaslp.AxleSpacingToFollow = ass;
                    sasaslpList.Add(sasaslp);
                }
                else
                {
                    ass.Value = vehicleComponentAxlesList.Max(x => x.NextAxleDistNoti);
                    sasaslp.AxleSpacingToFollow = ass;
                    sasaslpList.Add(sasaslp);
                }

            }
            else
            {

                List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

                foreach (long component in componentList)
                {
                    List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
                                                                                where apca.ComponentId == component
                                                                                orderby apca.AxleNumber ascending
                                                                                select apca).ToList();
                    
                    List<decimal> axleSpacingList = componentWiseAxleList.Select(x => Convert.ToDecimal(x.AxleSpacingToFollowing)).ToList();

                    int count = axleSpacingList.Count;
                    int localCount = 0;

                    decimal oldDummyweight = 0;
                    int oldcountaAxles = 0;

                    foreach (decimal axleSpacing in axleSpacingList)
                    {
                        localCount = localCount + 1;

                        if (axleSpacing != 0)
                        {
                            if (oldcountaAxles == 0)
                            {
                                oldDummyweight = axleSpacing;

                                oldcountaAxles = oldcountaAxles + 1;
                            }
                            else
                            {
                                if (axleSpacing == oldDummyweight)
                                {
                                    oldcountaAxles = oldcountaAxles + 1;
                                }
                                else
                                {
                                    SummaryAxleStructureAxleSpacingToFollowListPosition sasaslp = new SummaryAxleStructureAxleSpacingToFollowListPosition();
                                    AxleSpacingToFollowSummaryStructure ass = new AxleSpacingToFollowSummaryStructure();

                                    ass.Value = oldDummyweight;
                                    ass.AxleCount = Convert.ToString(oldcountaAxles);
                                    sasaslp.AxleSpacingToFollow = ass;
                                    sasaslpList.Add(sasaslp);

                                    oldcountaAxles = 1;
                                    oldDummyweight = axleSpacing;
                                }
                            }
                        }

                        if (localCount == count)
                        {
                            SummaryAxleStructureAxleSpacingToFollowListPosition sasaslp = new SummaryAxleStructureAxleSpacingToFollowListPosition();
                            AxleSpacingToFollowSummaryStructure ass = new AxleSpacingToFollowSummaryStructure();

                            if (oldDummyweight > 0)
                            {
                                ass.Value = oldDummyweight;
                                ass.AxleCount = Convert.ToString(oldcountaAxles);
                                sasaslp.AxleSpacingToFollow = ass;
                                sasaslpList.Add(sasaslp);
                            }
                        }
                    }
                }

                if (sasaslpList.ToArray().Length == 0)
                {
                    SummaryAxleStructureAxleSpacingToFollowListPosition sasaslp = new SummaryAxleStructureAxleSpacingToFollowListPosition();
                    AxleSpacingToFollowSummaryStructure ass = new AxleSpacingToFollowSummaryStructure();
                    ass.Value = 0;
                    sasaslp.AxleSpacingToFollow = ass;
                    sasaslpList.Add(sasaslp);
                }
            }

            return sasaslpList.ToArray();
        }
        #endregion

        #region GetAxleSpacingListPosition
        public static NotificationXSD.SummaryAxleStructureAxleSpacingListPosition[] GetAxleSpacingListPosition(List<VehComponentAxles> vehicleComponentAxlesList)
        {
            List<NotificationXSD.SummaryAxleStructureAxleSpacingListPosition> sasaslpList = new List<NotificationXSD.SummaryAxleStructureAxleSpacingListPosition>();

            List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

            foreach (long component in componentList)
            {
                List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
                                                                            where apca.ComponentId == component
                                                                            select apca).ToList();


                List<float> axleSpacingList = componentWiseAxleList.Select(x => x.NextAxleDist).ToList();

                int count = axleSpacingList.Count;
                int localCount = 0;

                float oldDummyweight = 0;
                int oldcountaAxles = 0;

                foreach (float axleSpacing in axleSpacingList)
                {
                    localCount = localCount + 1;

                    if (axleSpacing != 0)
                    {
                        if (oldcountaAxles == 0)
                        {
                            oldDummyweight = axleSpacing;

                            oldcountaAxles = oldcountaAxles + 1;
                        }
                        else
                        {
                            if (axleSpacing == oldDummyweight)
                            {
                                oldcountaAxles = oldcountaAxles + 1;
                            }
                            else
                            {
                                NotificationXSD.SummaryAxleStructureAxleSpacingListPosition sasaslp = new NotificationXSD.SummaryAxleStructureAxleSpacingListPosition();
                                NotificationXSD.AxleSpacingSummaryStructure ass = new NotificationXSD.AxleSpacingSummaryStructure();

                                ass.Value = Convert.ToDecimal(oldDummyweight);
                                ass.AxleCount = Convert.ToString(oldcountaAxles);
                                sasaslp.AxleSpacing = ass;
                                sasaslpList.Add(sasaslp);

                                oldcountaAxles = 1;
                                oldDummyweight = axleSpacing;
                            }
                        }
                    }

                    if (localCount == count)
                    {
                        NotificationXSD.SummaryAxleStructureAxleSpacingListPosition sasaslp = new NotificationXSD.SummaryAxleStructureAxleSpacingListPosition();
                        NotificationXSD.AxleSpacingSummaryStructure ass = new NotificationXSD.AxleSpacingSummaryStructure();

                        ass.Value = Convert.ToDecimal(oldDummyweight);
                        ass.AxleCount = Convert.ToString(oldcountaAxles);
                        sasaslp.AxleSpacing = ass;
                        sasaslpList.Add(sasaslp);
                    }
                }
            }

            if (sasaslpList.ToArray().Length == 0)
            {
                NotificationXSD.SummaryAxleStructureAxleSpacingListPosition sasaslp = new NotificationXSD.SummaryAxleStructureAxleSpacingListPosition();
                NotificationXSD.AxleSpacingSummaryStructure ass = new NotificationXSD.AxleSpacingSummaryStructure();
                ass.Value = 0;
                sasaslp.AxleSpacing = ass;
                sasaslpList.Add(sasaslp);
            }

            return sasaslpList.ToArray();
        }
        #endregion

        #region GetTyreSizeListPosition
        public static NotificationXSD.SummaryAxleStructureTyreSizeListPosition[] GetTyreSizeListPosition(List<VehComponentAxles> vehicleComponentAxlesList)
        {
            List<NotificationXSD.SummaryAxleStructureTyreSizeListPosition> sastslpList = new List<NotificationXSD.SummaryAxleStructureTyreSizeListPosition>();

            List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

            foreach (long component in componentList)
            {
                List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
                                                                            where apca.ComponentId == component
                                                                            orderby apca.AxleNumber ascending
                                                                            select apca).ToList();
                
                List<string> tyreSizeList = componentWiseAxleList.Select(x => x.TyreSize).ToList();

                int count = tyreSizeList.Count;
                int localCount = 0;

                string oldDummyweight = "";
                int oldcountaAxles = 0;

                foreach (string tyreSize in tyreSizeList)
                {
                    localCount = localCount + 1;

                    if (oldcountaAxles == 0)
                    {
                        oldDummyweight = tyreSize;

                        oldcountaAxles = oldcountaAxles + 1;
                    }
                    else
                    {
                        if (tyreSize == oldDummyweight)
                        {
                            oldcountaAxles = oldcountaAxles + 1;
                        }
                        else
                        {
                            NotificationXSD.SummaryAxleStructureTyreSizeListPosition sastslp = new NotificationXSD.SummaryAxleStructureTyreSizeListPosition();
                            NotificationXSD.TyreSizeSummaryStructure tzss = new NotificationXSD.TyreSizeSummaryStructure();

                            tzss.Value = oldDummyweight;
                            tzss.AxleCount = Convert.ToString(oldcountaAxles);
                            sastslp.TyreSize = tzss;
                            sastslpList.Add(sastslp);

                            oldcountaAxles = 1;
                            oldDummyweight = tyreSize;
                        }
                    }

                    if (localCount == count)
                    {
                        NotificationXSD.SummaryAxleStructureTyreSizeListPosition sastslp = new NotificationXSD.SummaryAxleStructureTyreSizeListPosition();
                        NotificationXSD.TyreSizeSummaryStructure tzss = new NotificationXSD.TyreSizeSummaryStructure();

                        tzss.Value = oldDummyweight;
                        tzss.AxleCount = Convert.ToString(oldcountaAxles);
                        sastslp.TyreSize = tzss;
                        sastslpList.Add(sastslp);
                    }
                }
            }

            return sastslpList.ToArray();
        }
        #endregion

        #region GetWheelSpacingListPosition
        public static NotificationXSD.SummaryAxleStructureWheelSpacingListPosition[] GetWheelSpacingListPosition(List<VehComponentAxles> vehicleComponentAxlesList)
        {
            List<NotificationXSD.SummaryAxleStructureWheelSpacingListPosition> saswslp = new List<NotificationXSD.SummaryAxleStructureWheelSpacingListPosition>();

            List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

            foreach (long component in componentList)
            {
                List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
                                                                            where apca.ComponentId == component
                                                                            orderby apca.AxleNumber ascending
                                                                            select apca).ToList();
                
                List<string> wheelSpacingList = componentWiseAxleList.Select(x => x.WheelSpacingList).ToList();
                
                int count = wheelSpacingList.Count;
                int localCount = 0;

                string oldDummyweight = "";
                int oldcountaAxles = 0;

                foreach (string tyreSize in wheelSpacingList)
                {
                    localCount = localCount + 1;

                    if (oldcountaAxles == 0)
                    {
                        oldDummyweight = tyreSize;

                        oldcountaAxles = oldcountaAxles + 1;
                    }
                    else
                    {
                        if (tyreSize == oldDummyweight)
                        {
                            oldcountaAxles = oldcountaAxles + 1;
                        }
                        else
                        {
                            NotificationXSD.SummaryAxleStructureWheelSpacingListPosition saswsplp = new NotificationXSD.SummaryAxleStructureWheelSpacingListPosition();
                            NotificationXSD.WheelSpacingSummaryStructure wsss = new NotificationXSD.WheelSpacingSummaryStructure();

                            wsss.Value = oldDummyweight;
                            wsss.AxleCount = Convert.ToString(oldcountaAxles);
                            saswsplp.WheelSpacing = wsss;
                            saswslp.Add(saswsplp);

                            oldcountaAxles = 1;
                            oldDummyweight = tyreSize;
                        }
                    }

                    if (localCount == count)
                    {
                        NotificationXSD.SummaryAxleStructureWheelSpacingListPosition saswsplp = new NotificationXSD.SummaryAxleStructureWheelSpacingListPosition();
                        NotificationXSD.WheelSpacingSummaryStructure wsss = new NotificationXSD.WheelSpacingSummaryStructure();

                        wsss.Value = oldDummyweight;
                        wsss.AxleCount = Convert.ToString(oldcountaAxles);
                        saswsplp.WheelSpacing = wsss;
                        saswslp.Add(saswsplp);
                    }
                }
            }

            return saswslp.ToArray();
        }
        #endregion

        #region GetVehicleComponentSubTypes
        public static NotificationXSD.VehicleComponentSubType GetVehicleComponentSubTypes(String type)
        {
            NotificationXSD.VehicleComponentSubType vehtype = new NotificationXSD.VehicleComponentSubType();
            if (type == "ballast tractor")
            {
                vehtype = NotificationXSD.VehicleComponentSubType.ballasttractor;
            }
            else if (type == "conventional tractor")
            {
                vehtype = NotificationXSD.VehicleComponentSubType.conventionaltractor;
            }
            else if (type == "other tractor")
            {
                vehtype = NotificationXSD.VehicleComponentSubType.othertractor;
            }
            else if (type == "semi trailer")
            {
                vehtype = NotificationXSD.VehicleComponentSubType.semitrailer;
            }
            else if (type == "semi low loader")
            {
                vehtype = NotificationXSD.VehicleComponentSubType.semilowloader;
            }
            else if (type == "trombone trailer")
            {
                vehtype = NotificationXSD.VehicleComponentSubType.trombonetrailer;
            }
            else if (type == "other semi trailer")
            {
                vehtype = NotificationXSD.VehicleComponentSubType.othersemitrailer;
            }
            else if (type == "drawbar trailer")
            {
                vehtype = NotificationXSD.VehicleComponentSubType.drawbartrailer;
            }
            else if (type == "other drawbar trailer")
            {
                vehtype = NotificationXSD.VehicleComponentSubType.otherdrawbartrailer;
            }
            else if (type == "twin bogies")
            {
                vehtype = NotificationXSD.VehicleComponentSubType.twinbogies;
            }
            else if (type == "tracked vehicle")
            {
                vehtype = NotificationXSD.VehicleComponentSubType.trackedvehicle;
            }
            else if (type == "rigid vehicle")
            {
                vehtype = NotificationXSD.VehicleComponentSubType.rigidvehicle;
            }
            else if (type == "girder set")
            {
                vehtype = NotificationXSD.VehicleComponentSubType.girderset;
            }
            else if (type == "wheeled load")
            {
                vehtype = NotificationXSD.VehicleComponentSubType.wheeledload;
            }
            else if (type == "recovery vehicle")
            {
                vehtype = NotificationXSD.VehicleComponentSubType.recoveryvehicle;
            }
            else if (type == "recovered vehicle")
            {
                vehtype = NotificationXSD.VehicleComponentSubType.recoveredvehicle;
            }
            else if (type == "mobile crane"|| type == "Crane")
            {
                vehtype = NotificationXSD.VehicleComponentSubType.mobilecrane;
            }
            else if (type == "engineering plant")
            {
                vehtype = NotificationXSD.VehicleComponentSubType.engineeringplant;
            }
            return vehtype;
        }
        #endregion

        #region GetVehicleComponentSubTypesNonSemiVehicles
        public static NotificationXSD.VehicleComponentType GetVehicleComponentSubTypesNonSemiVehicles(String type)
        {
            NotificationXSD.VehicleComponentType vehtype = new NotificationXSD.VehicleComponentType();

            if (type == "ballast tractor")
            {
                vehtype = NotificationXSD.VehicleComponentType.ballasttractor;
            }
            else if (type == "conventional tractor")
            {
                vehtype = NotificationXSD.VehicleComponentType.conventionaltractor;
            }
            else if (type == "semi trailer")
            {
                vehtype = NotificationXSD.VehicleComponentType.semitrailer;
            }
            else if (type == "drawbar trailer")
            {
                vehtype = NotificationXSD.VehicleComponentType.drawbartrailer;
            }
            else if (type == "rigid vehicle")
            {
                vehtype = NotificationXSD.VehicleComponentType.rigidvehicle;
            }
            else if (type == "spmt")
            {
                vehtype = NotificationXSD.VehicleComponentType.spmt;
            }

            else if (type == "other drawbar trailer")
            {
                vehtype = NotificationXSD.VehicleComponentType.trackedvehicle;
            }

            return vehtype;
        }
        #endregion

        #region GetVehicleComponentSubTypes
        public static ProposedRouteXSD.VehicleComponentSubType GetProposedVehicleComponentSubTypes(String type)
        {
            ProposedRouteXSD.VehicleComponentSubType vehtype = new ProposedRouteXSD.VehicleComponentSubType();
            if (type == "ballast tractor")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.ballasttractor;
            }
            else if (type == "conventional tractor")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.conventionaltractor;
            }
            else if (type == "other tractor")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.othertractor;
            }
            else if (type == "semi trailer")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.semitrailer;
            }
            else if (type == "semi low loader")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.semilowloader;
            }
            else if (type == "trombone trailer")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.trombonetrailer;
            }
            else if (type == "other semi trailer")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.othersemitrailer;
            }
            else if (type == "drawbar trailer")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.drawbartrailer;
            }
            else if (type == "other drawbar trailer")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.otherdrawbartrailer;
            }
            else if (type == "twin bogies")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.twinbogies;
            }
            else if (type == "tracked vehicle")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.trackedvehicle;
            }
            else if (type == "rigid vehicle")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.rigidvehicle;
            }
            else if (type == "girder set")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.girderset;
            }
            else if (type == "wheeled load")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.wheeledload;
            }
            else if (type == "recovery vehicle")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.recoveryvehicle;
            }
            else if (type == "recovered vehicle")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.recoveredvehicle;
            }
            else if (type == "mobile crane" || type == "Crane")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.mobilecrane;
            }
            else if (type == "engineering plant")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.engineeringplant;
            }
            return vehtype;
        }
        #endregion

        #region GetVehicleComponentSubTypesNonSemiVehicles
        public static ProposedRouteXSD.VehicleComponentType GetProposedVehicleComponentSubTypesNonSemiVehicles(String type)
        {
            ProposedRouteXSD.VehicleComponentType vehtype = new ProposedRouteXSD.VehicleComponentType();

            if (type == "ballast tractor")
            {
                vehtype = ProposedRouteXSD.VehicleComponentType.ballasttractor;
            }
            else if (type == "conventional tractor")
            {
                vehtype = ProposedRouteXSD.VehicleComponentType.conventionaltractor;
            }
            else if (type == "semi trailer")
            {
                vehtype = ProposedRouteXSD.VehicleComponentType.semitrailer;
            }
            else if (type == "drawbar trailer")
            {
                vehtype = ProposedRouteXSD.VehicleComponentType.drawbartrailer;
            }
            else if (type == "rigid vehicle")
            {
                vehtype = ProposedRouteXSD.VehicleComponentType.rigidvehicle;
            }
            else if (type == "spmt")
            {
                vehtype = ProposedRouteXSD.VehicleComponentType.spmt;
            }

            else if (type == "other drawbar trailer")
            {
                vehtype = ProposedRouteXSD.VehicleComponentType.trackedvehicle;
            }

            return vehtype;
        }
        #endregion

        #region GetVehicleComponentSubTypes
        public static AggreedRouteXSD.VehicleComponentSubType GetAggreedVehicleComponentSubTypes(String type)
        {
            AggreedRouteXSD.VehicleComponentSubType vehtype = new AggreedRouteXSD.VehicleComponentSubType();
            if (type == "ballast tractor")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.ballasttractor;
            }
            else if (type == "conventional tractor")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.conventionaltractor;
            }
            else if (type == "other tractor")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.othertractor;
            }
            else if (type == "semi trailer")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.semitrailer;
            }
            else if (type == "semi low loader")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.semilowloader;
            }
            else if (type == "trombone trailer")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.trombonetrailer;
            }
            else if (type == "other semi trailer")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.othersemitrailer;
            }
            else if (type == "drawbar trailer")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.drawbartrailer;
            }
            else if (type == "other drawbar trailer")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.otherdrawbartrailer;
            }
            else if (type == "twin bogies")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.twinbogies;
            }
            else if (type == "tracked vehicle")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.trackedvehicle;
            }
            else if (type == "rigid vehicle")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.rigidvehicle;
            }
            else if (type == "girder set")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.girderset;
            }
            else if (type == "wheeled load")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.wheeledload;
            }
            else if (type == "recovery vehicle")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.recoveryvehicle;
            }
            else if (type == "recovered vehicle")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.recoveredvehicle;
            }
            else if (type == "mobile crane" || type == "Crane")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.mobilecrane;
            }
            else if (type == "engineering plant")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.engineeringplant;
            }
            return vehtype;
        }
        #endregion

        #region GetVehicleComponentSubTypesNonSemiVehicles
        public static AggreedRouteXSD.VehicleComponentType GetAggreedVehicleComponentSubTypesNonSemiVehicles(String type)
        {
            AggreedRouteXSD.VehicleComponentType vehtype = new AggreedRouteXSD.VehicleComponentType();

            if (type == "ballast tractor")
            {
                vehtype = AggreedRouteXSD.VehicleComponentType.ballasttractor;
            }
            else if (type == "conventional tractor")
            {
                vehtype = AggreedRouteXSD.VehicleComponentType.conventionaltractor;
            }
            else if (type == "semi trailer")
            {
                vehtype = AggreedRouteXSD.VehicleComponentType.semitrailer;
            }
            else if (type == "drawbar trailer")
            {
                vehtype = AggreedRouteXSD.VehicleComponentType.drawbartrailer;
            }
            else if (type == "rigid vehicle")
            {
                vehtype = AggreedRouteXSD.VehicleComponentType.rigidvehicle;
            }
            else if (type == "spmt")
            {
                vehtype = AggreedRouteXSD.VehicleComponentType.spmt;
            }

            else if (type == "other drawbar trailer")
            {
                vehtype = AggreedRouteXSD.VehicleComponentType.trackedvehicle;
            }

            return vehtype;
        }
        #endregion

        #region Commented Code By Mahzeer on 12/07/2023
        /*
        public static NotificationXSD.VehicleComponentType GetVehicleComponentTypeForNotification(String type)
        {
            NotificationXSD.VehicleComponentType vehtype = new NotificationXSD.VehicleComponentType();
            if (type == "ballast tractor")
            {
                vehtype = NotificationXSD.VehicleComponentType.ballasttractor;
            }
            else if (type == "conventional tractor")
            {
                vehtype = NotificationXSD.VehicleComponentType.conventionaltractor;
            }
            else if (type == "drawbar trailer")
            {
                vehtype = NotificationXSD.VehicleComponentType.drawbartrailer;
            }
            else if (type == "rigid vehicle")
            {
                vehtype = NotificationXSD.VehicleComponentType.rigidvehicle;
            }
            else if (type == "semi trailer")
            {
                vehtype = NotificationXSD.VehicleComponentType.semitrailer;
            }
            else if (type == "spmt")
            {
                vehtype = NotificationXSD.VehicleComponentType.spmt;
            }
            else if (type == "tracked vehicle")
            {
                vehtype = NotificationXSD.VehicleComponentType.trackedvehicle;
            }

            return vehtype;
        }

        public static string ReadXMLInformation(string xmlFileName)
        {
            string xmlString = string.Empty;
            XPathDocument document = new XPathDocument(xmlFileName);
            XPathNavigator navigator = document.CreateNavigator();

            xmlString = navigator.OuterXml;

            return xmlString;
        }
        public static AggreedRouteXSD.VehiclesSummaryStructureConfigurationSummaryListPosition[] GetRevisedAgreemetVehicleName(List<VehicleSummary> vehicleSummaryList)
        {
            List<AggreedRouteXSD.VehiclesSummaryStructureConfigurationSummaryListPosition> saswslp = new List<AggreedRouteXSD.VehiclesSummaryStructureConfigurationSummaryListPosition>();

            List<string> vehicleNameList = vehicleSummaryList.Select(x => x.VehicleName).Distinct().ToList();

            foreach (string vehicleName in vehicleNameList)
            {
                AggreedRouteXSD.VehiclesSummaryStructureConfigurationSummaryListPosition vssgwlp = new AggreedRouteXSD.VehiclesSummaryStructureConfigurationSummaryListPosition();
                vssgwlp.ConfigurationSummary = vehicleName;
                saswslp.Add(vssgwlp);
            }

            return saswslp.ToArray();
        }

        public static AggreedRouteXSD.VehiclesSummaryStructureGrossWeightListPosition[] GetRevisedAgreemetGrossWeight(List<VehicleSummary> vehicleSummaryList)
        {
            List<AggreedRouteXSD.VehiclesSummaryStructureGrossWeightListPosition> saswslp = new List<AggreedRouteXSD.VehiclesSummaryStructureGrossWeightListPosition>();

            List<float> grossWeightList = vehicleSummaryList.Select(x => x.GrossWeight).Distinct().ToList();

            foreach (float grossWeight in grossWeightList)
            {
                AggreedRouteXSD.VehiclesSummaryStructureGrossWeightListPosition vssgwlp = new AggreedRouteXSD.VehiclesSummaryStructureGrossWeightListPosition();

                AggreedRouteXSD.GrossWeightStructure gws = new AggreedRouteXSD.GrossWeightStructure();
                gws.ExcludesTractors = false;
                gws.Item = grossWeight.ToString();
                vssgwlp.GrossWeight = gws;

                saswslp.Add(vssgwlp);
            }

            return saswslp.ToArray();
        }

        public static AggreedRouteXSD.VehiclesSummaryStructureMaxAxleWeightListPosition[] GetRevisedAgreemetMaxAxleWeight(List<VehicleSummary> vehicleSummaryList)
        {
            List<AggreedRouteXSD.VehiclesSummaryStructureMaxAxleWeightListPosition> saswslp = new List<AggreedRouteXSD.VehiclesSummaryStructureMaxAxleWeightListPosition>();

            List<float> maxAxleWeightList = vehicleSummaryList.Select(x => x.MaxAxleWeight).Distinct().ToList();

            foreach (float maxAxleWeight in maxAxleWeightList)
            {
                AggreedRouteXSD.VehiclesSummaryStructureMaxAxleWeightListPosition vssgwlp = new AggreedRouteXSD.VehiclesSummaryStructureMaxAxleWeightListPosition();

                AggreedRouteXSD.SummaryMaxAxleWeightStructure gws = new AggreedRouteXSD.SummaryMaxAxleWeightStructure();
                gws.ItemElementName = AggreedRouteXSD.ItemChoiceType1.Weight;
                gws.Item = maxAxleWeight.ToString();
                vssgwlp.MaxAxleWeight = gws;

                saswslp.Add(vssgwlp);
            }

            return saswslp.ToArray();
        }
        public static void SaveDetailOutboundNotification(long notificationID, byte[] exportByteArrayData)
        {
            //OutboundDocuments outbounddocs = new OutboundDocuments();
            //outbounddocs.DocumentInBytes = exportByteArrayData;
            //outbounddocs.NotificationID = (int)notificationID;
            //OutBoundDocumentDOA.SaveXMLOutboundNotification(outbounddocs);
        }
        public static byte[] GetAgreedProposedNotificationXML(string docType, string ESDALRef, int notificationID)
        {
            byte[] docBytes = null;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                docBytes,
                 UserSchema.Portal + ".GET_AGREED_PROP_NOTI_XML",
                parameter =>
                {
                    parameter.AddWithValue("P_DOCTYPE", docType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ESDALREF", ESDALRef, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NOTIFICATION_ID", notificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       docBytes = records.GetByteArrayOrNull("document_xml");
                   }
            );
            return docBytes;
        }
        */
        #endregion

        #region GetProposalAxleWeightListPosition
        public static ProposedRouteXSD.SummaryAxleStructureAxleWeightListPosition[] GetProposalAxleWeightListPosition(List<VehComponentAxles> vehicleComponentAxlesList)
        {
            List<ProposedRouteXSD.SummaryAxleStructureAxleWeightListPosition> sasawlpList = new List<ProposedRouteXSD.SummaryAxleStructureAxleWeightListPosition>();

            List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

            foreach (long component in componentList)
            {
                List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
                                                                            where apca.ComponentId == component
                                                                            orderby apca.AxleNumber ascending
                                                                            select apca).ToList();

                List<float> weightList = componentWiseAxleList.Select(x => x.Weight).ToList();

                int count = weightList.Count;
                int localCount = 0;

                float oldDummyweight = 0;
                int oldcountaAxles = 0;

                foreach (float weight in weightList)
                {
                    localCount = localCount + 1;

                    if (oldcountaAxles == 0)
                    {
                        oldDummyweight = weight;

                        oldcountaAxles = oldcountaAxles + 1;
                    }
                    else
                    {
                        if (weight == oldDummyweight)
                        {
                            oldcountaAxles = oldcountaAxles + 1;
                        }
                        else
                        {
                            ProposedRouteXSD.SummaryAxleStructureAxleWeightListPosition sasawlp = new ProposedRouteXSD.SummaryAxleStructureAxleWeightListPosition();
                            ProposedRouteXSD.AxleWeightSummaryStructure awss = new ProposedRouteXSD.AxleWeightSummaryStructure();

                            awss.Value = Convert.ToString(oldDummyweight);
                            awss.AxleCount = Convert.ToString(oldcountaAxles);

                            sasawlp.AxleWeight = awss;

                            sasawlpList.Add(sasawlp);

                            oldcountaAxles = 1;
                            oldDummyweight = weight;
                        }
                    }

                    if (localCount == count)
                    {
                        ProposedRouteXSD.SummaryAxleStructureAxleWeightListPosition sasawlp = new ProposedRouteXSD.SummaryAxleStructureAxleWeightListPosition();
                        ProposedRouteXSD.AxleWeightSummaryStructure awss = new ProposedRouteXSD.AxleWeightSummaryStructure();

                        awss.Value = Convert.ToString(oldDummyweight);
                        awss.AxleCount = Convert.ToString(oldcountaAxles);

                        sasawlp.AxleWeight = awss;

                        sasawlpList.Add(sasawlp);
                    }
                }


            }

            if (sasawlpList.ToArray().Length == 0)
            {
                ProposedRouteXSD.SummaryAxleStructureAxleWeightListPosition sasawlp = new ProposedRouteXSD.SummaryAxleStructureAxleWeightListPosition();
                ProposedRouteXSD.AxleWeightSummaryStructure awss = new ProposedRouteXSD.AxleWeightSummaryStructure();
                awss.Value = "0";
                sasawlp.AxleWeight = awss;
                sasawlpList.Add(sasawlp);
            }

            return sasawlpList.ToArray();
        }
        #endregion

        #region GetProposalWheelsPerAxleListPosition
        public static ProposedRouteXSD.SummaryAxleStructureWheelsPerAxleListPosition[] GetProposalWheelsPerAxleListPosition(List<VehComponentAxles> vehicleComponentAxlesList)
        {
            List<ProposedRouteXSD.SummaryAxleStructureWheelsPerAxleListPosition> saswpalpList = new List<ProposedRouteXSD.SummaryAxleStructureWheelsPerAxleListPosition>();

            List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

            foreach (long component in componentList)
            {
                List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
                                                                            where apca.ComponentId == component
                                                                            orderby apca.AxleNumber ascending
                                                                            select apca).ToList();
                
                List<short> weightList = componentWiseAxleList.Select(x => x.WheelCount).ToList();

                int count = weightList.Count;
                int localCount = 0;

                short oldDummyweight = 0;
                int oldcountaAxles = 0;

                foreach (short weight in weightList)
                {
                    localCount = localCount + 1;

                    if (oldcountaAxles == 0)
                    {
                        oldDummyweight = weight;

                        oldcountaAxles = oldcountaAxles + 1;
                    }
                    else
                    {
                        if (weight == oldDummyweight)
                        {
                            oldcountaAxles = oldcountaAxles + 1;
                        }
                        else
                        {
                            ProposedRouteXSD.SummaryAxleStructureWheelsPerAxleListPosition saswpalp = new ProposedRouteXSD.SummaryAxleStructureWheelsPerAxleListPosition();
                            ProposedRouteXSD.WheelsPerAxleSummaryStructure wpass = new ProposedRouteXSD.WheelsPerAxleSummaryStructure();

                            wpass.Value = Convert.ToString(oldDummyweight);
                            wpass.AxleCount = Convert.ToString(oldcountaAxles);

                            saswpalp.WheelsPerAxle = wpass;

                            saswpalpList.Add(saswpalp);

                            oldcountaAxles = 1;
                            oldDummyweight = weight;
                        }
                    }

                    if (localCount == count)
                    {
                        ProposedRouteXSD.SummaryAxleStructureWheelsPerAxleListPosition saswpalp = new ProposedRouteXSD.SummaryAxleStructureWheelsPerAxleListPosition();
                        ProposedRouteXSD.WheelsPerAxleSummaryStructure wpass = new ProposedRouteXSD.WheelsPerAxleSummaryStructure();

                        wpass.Value = Convert.ToString(oldDummyweight);
                        wpass.AxleCount = Convert.ToString(oldcountaAxles);

                        saswpalp.WheelsPerAxle = wpass;

                        saswpalpList.Add(saswpalp);
                    }
                }
            }

            if (saswpalpList.ToArray().Length == 0)
            {
                ProposedRouteXSD.SummaryAxleStructureWheelsPerAxleListPosition saswpalp = new ProposedRouteXSD.SummaryAxleStructureWheelsPerAxleListPosition();
                ProposedRouteXSD.WheelsPerAxleSummaryStructure wpass = new ProposedRouteXSD.WheelsPerAxleSummaryStructure();

                wpass.Value = "0";
                saswpalp.WheelsPerAxle = wpass;
                saswpalpList.Add(saswpalp);
            }

            return saswpalpList.ToArray();
        }
        #endregion

        #region GetProposalAxleSpacingListPosition
        public static ProposedRouteXSD.SummaryAxleStructureAxleSpacingListPosition[] GetProposalAxleSpacingListPosition(List<VehComponentAxles> vehicleComponentAxlesList, bool isSemiVehicle = true)
        {
            List<ProposedRouteXSD.SummaryAxleStructureAxleSpacingListPosition> sasaslpList = new List<ProposedRouteXSD.SummaryAxleStructureAxleSpacingListPosition>();

            List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

            foreach (long component in componentList)
            {
                List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
                                                                            where apca.ComponentId == component
                                                                            select apca).ToList();

                List<float> axleSpacingList = componentWiseAxleList.Select(x => x.NextAxleDist).ToList();

                double axleSpacingToFollowing = componentWiseAxleList.Select(x => x.AxleSpacingToFollowing).Distinct().FirstOrDefault();

                int count = axleSpacingList.Count;
                int localCount = 0;

                float oldDummyweight = 0;
                int oldcountaAxles = 0;

                foreach (float axleSpacing in axleSpacingList)
                {
                    localCount = localCount + 1;

                    if (axleSpacing != 0)
                    {
                        if (oldcountaAxles == 0)
                        {
                            oldDummyweight = axleSpacing;

                            oldcountaAxles = oldcountaAxles + 1;
                        }
                        else
                        {
                            if (axleSpacing == oldDummyweight)
                            {
                                oldcountaAxles = oldcountaAxles + 1;
                            }
                            else
                            {
                                ProposedRouteXSD.SummaryAxleStructureAxleSpacingListPosition sasaslp = new ProposedRouteXSD.SummaryAxleStructureAxleSpacingListPosition();
                                ProposedRouteXSD.AxleSpacingSummaryStructure ass = new ProposedRouteXSD.AxleSpacingSummaryStructure();

                                ass.Value = Convert.ToDecimal(oldDummyweight);
                                ass.AxleCount = Convert.ToString(oldcountaAxles);
                                sasaslp.AxleSpacing = ass;
                                sasaslpList.Add(sasaslp);

                                oldcountaAxles = 1;
                                oldDummyweight = axleSpacing;
                            }
                        }
                    }

                    if (localCount == count)
                    {
                        ProposedRouteXSD.SummaryAxleStructureAxleSpacingListPosition sasaslp = new ProposedRouteXSD.SummaryAxleStructureAxleSpacingListPosition();
                        ProposedRouteXSD.AxleSpacingSummaryStructure ass = new ProposedRouteXSD.AxleSpacingSummaryStructure();

                        ass.Value = Convert.ToDecimal(oldDummyweight);
                        ass.AxleCount = Convert.ToString(oldcountaAxles);
                        sasaslp.AxleSpacing = ass;
                        sasaslpList.Add(sasaslp);
                    }
                }

                if (axleSpacingToFollowing != 0.0 && axleSpacingToFollowing > 0 && isSemiVehicle)
                {
                    ProposedRouteXSD.SummaryAxleStructureAxleSpacingListPosition sasaslp = new ProposedRouteXSD.SummaryAxleStructureAxleSpacingListPosition();
                    ProposedRouteXSD.AxleSpacingSummaryStructure ass = new ProposedRouteXSD.AxleSpacingSummaryStructure();

                    ass.Value = Convert.ToDecimal(axleSpacingToFollowing);
                    ass.AxleCount = "1";
                    sasaslp.AxleSpacing = ass;
                    sasaslpList.Add(sasaslp);
                }
            }

            if (sasaslpList.ToArray().Length == 0)
            {
                ProposedRouteXSD.SummaryAxleStructureAxleSpacingListPosition sasaslp = new ProposedRouteXSD.SummaryAxleStructureAxleSpacingListPosition();
                ProposedRouteXSD.AxleSpacingSummaryStructure ass = new ProposedRouteXSD.AxleSpacingSummaryStructure();

                ass.Value = 0;
                sasaslp.AxleSpacing = ass;
                sasaslpList.Add(sasaslp);
            }

            return sasaslpList.ToArray();
        }
        #endregion

        #region GetProposalTyreSizeListPosition
        public static ProposedRouteXSD.SummaryAxleStructureTyreSizeListPosition[] GetProposalTyreSizeListPosition(List<VehComponentAxles> vehicleComponentAxlesList)
        {
            List<ProposedRouteXSD.SummaryAxleStructureTyreSizeListPosition> sastslpList = new List<ProposedRouteXSD.SummaryAxleStructureTyreSizeListPosition>();

            List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

            foreach (long component in componentList)
            {
                List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
                                                                            where apca.ComponentId == component
                                                                            orderby apca.AxleNumber ascending
                                                                            select apca).ToList();


                List<string> tyreSizeList = componentWiseAxleList.Select(x => x.TyreSize).ToList();

                int count = tyreSizeList.Count;
                int localCount = 0;

                string oldDummyweight = "";
                int oldcountaAxles = 0;

                foreach (string tyreSize in tyreSizeList)
                {
                    localCount = localCount + 1;

                    if (oldcountaAxles == 0)
                    {
                        oldDummyweight = tyreSize;

                        oldcountaAxles = oldcountaAxles + 1;
                    }
                    else
                    {
                        if (tyreSize == oldDummyweight)
                        {
                            oldcountaAxles = oldcountaAxles + 1;
                        }
                        else
                        {
                            ProposedRouteXSD.SummaryAxleStructureTyreSizeListPosition sastslp = new ProposedRouteXSD.SummaryAxleStructureTyreSizeListPosition();
                            ProposedRouteXSD.TyreSizeSummaryStructure tzss = new ProposedRouteXSD.TyreSizeSummaryStructure();

                            tzss.Value = oldDummyweight;
                            tzss.AxleCount = Convert.ToString(oldcountaAxles);
                            sastslp.TyreSize = tzss;
                            sastslpList.Add(sastslp);

                            oldcountaAxles = 1;
                            oldDummyweight = tyreSize;
                        }
                    }

                    if (localCount == count)
                    {
                        ProposedRouteXSD.SummaryAxleStructureTyreSizeListPosition sastslp = new ProposedRouteXSD.SummaryAxleStructureTyreSizeListPosition();
                        ProposedRouteXSD.TyreSizeSummaryStructure tzss = new ProposedRouteXSD.TyreSizeSummaryStructure();

                        tzss.Value = oldDummyweight;
                        tzss.AxleCount = Convert.ToString(oldcountaAxles);
                        sastslp.TyreSize = tzss;
                        sastslpList.Add(sastslp);
                    }
                }
            }

            return sastslpList.ToArray();
        }
        #endregion

        #region GetProposalWheelSpacingListPosition
        public static ProposedRouteXSD.SummaryAxleStructureWheelSpacingListPosition[] GetProposalWheelSpacingListPosition(List<VehComponentAxles> vehicleComponentAxlesList)
        {
            List<ProposedRouteXSD.SummaryAxleStructureWheelSpacingListPosition> saswslp = new List<ProposedRouteXSD.SummaryAxleStructureWheelSpacingListPosition>();

            List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

            foreach (long component in componentList)
            {
                List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
                                                                            where apca.ComponentId == component
                                                                            orderby apca.AxleNumber ascending
                                                                            select apca).ToList();
                
                List<string> wheelSpacingList = componentWiseAxleList.Select(x => x.WheelSpacingList).ToList();

                int count = wheelSpacingList.Count;
                int localCount = 0;

                string oldDummyweight = "";
                int oldcountaAxles = 0;

                foreach (string tyreSize in wheelSpacingList)
                {
                    localCount = localCount + 1;

                    if (oldcountaAxles == 0)
                    {
                        oldDummyweight = tyreSize;

                        oldcountaAxles = oldcountaAxles + 1;
                    }
                    else
                    {
                        if (tyreSize == oldDummyweight)
                        {
                            oldcountaAxles = oldcountaAxles + 1;
                        }
                        else
                        {
                            ProposedRouteXSD.SummaryAxleStructureWheelSpacingListPosition saswsplp = new ProposedRouteXSD.SummaryAxleStructureWheelSpacingListPosition();
                            ProposedRouteXSD.WheelSpacingSummaryStructure wsss = new ProposedRouteXSD.WheelSpacingSummaryStructure();

                            wsss.Value = oldDummyweight;
                            wsss.AxleCount = Convert.ToString(oldcountaAxles);
                            saswsplp.WheelSpacing = wsss;
                            saswslp.Add(saswsplp);

                            oldcountaAxles = 1;
                            oldDummyweight = tyreSize;
                        }
                    }

                    if (localCount == count)
                    {
                        ProposedRouteXSD.SummaryAxleStructureWheelSpacingListPosition saswsplp = new ProposedRouteXSD.SummaryAxleStructureWheelSpacingListPosition();
                        ProposedRouteXSD.WheelSpacingSummaryStructure wsss = new ProposedRouteXSD.WheelSpacingSummaryStructure();

                        wsss.Value = oldDummyweight;
                        wsss.AxleCount = Convert.ToString(oldcountaAxles);
                        saswsplp.WheelSpacing = wsss;
                        saswslp.Add(saswsplp);
                    }
                }
            }

            return saswslp.ToArray();
        }
        #endregion

        #region GetVehicleComponentMainType
        public static ProposedRouteXSD.VehicleComponentType GetVehicleComponentMainType(String type)
        {
            ProposedRouteXSD.VehicleComponentType vehtype = new ProposedRouteXSD.VehicleComponentType();

            if (type == "ballast tractor")
            {
                vehtype = ProposedRouteXSD.VehicleComponentType.ballasttractor;
            }
            else if (type == "conventional tractor")
            {
                vehtype = ProposedRouteXSD.VehicleComponentType.conventionaltractor;
            }

            else if (type == "semi trailer")
            {
                vehtype = ProposedRouteXSD.VehicleComponentType.semitrailer;
            }


            else if (type == "drawbar trailer")
            {
                vehtype = ProposedRouteXSD.VehicleComponentType.drawbartrailer;
            }

            else if (type == "tracked vehicle")
            {
                vehtype = ProposedRouteXSD.VehicleComponentType.trackedvehicle;
            }
            else if (type == "rigid vehicle")
            {
                vehtype = ProposedRouteXSD.VehicleComponentType.rigidvehicle;
            }
            else if (type == "spmt")
            {
                vehtype = ProposedRouteXSD.VehicleComponentType.spmt;
            }
            return vehtype;
        }
        #endregion

        #region GetVehicleComponentType
        public static ProposedRouteXSD.VehicleComponentSubType GetVehicleComponentType(String type)
        {
            ProposedRouteXSD.VehicleComponentSubType vehtype = new ProposedRouteXSD.VehicleComponentSubType();
            if (type == "ballast tractor")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.ballasttractor;
            }
            else if (type == "conventional tractor")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.conventionaltractor;
            }
            else if (type == "other tractor")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.othertractor;
            }
            else if (type == "semi trailer")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.semitrailer;
            }
            else if (type == "semi low loader")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.semilowloader;
            }
            else if (type == "trombone trailer")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.trombonetrailer;
            }
            else if (type == "other semi trailer")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.othersemitrailer;
            }
            else if (type == "drawbar trailer")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.drawbartrailer;
            }
            else if (type == "other drawbar trailer")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.otherdrawbartrailer;
            }
            else if (type == "twin bogies")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.twinbogies;
            }
            else if (type == "tracked vehicle")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.trackedvehicle;
            }
            else if (type == "rigid vehicle")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.rigidvehicle;
            }
            else if (type == "girder set")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.girderset;
            }
            else if (type == "wheeled load")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.wheeledload;
            }
            else if (type == "recovery vehicle")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.recoveryvehicle;
            }
            else if (type == "recovered vehicle")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.recoveredvehicle;
            }
            else if (type == "mobile crane")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.mobilecrane;
            }
            else if (type == "engineering plant")
            {
                vehtype = ProposedRouteXSD.VehicleComponentSubType.engineeringplant;
            }
            return vehtype;
        }
        #endregion

        #region GenerateNotificationPDF
        public static GenerateEmailgetParams GenerateNotificationPDF(int notificationID, int docType, string xmlInformation, string fileName, string esDALRefNo, long organisationID, ContactModel objcontact, string docfileName, UserInfo userInfo = null, int icaStatus = 277001, bool indemnity = false, bool ImminentMovestatus = false, int routePlanUnits = 692001)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("GenerateNotificationPDF started successfully with parameters esdalRef : {0}, Notifid : {1}, OrgId : {2}, ICAStat : {3}", esDALRefNo, notificationID, organisationID, icaStatus));
            string userSchema = "";
            if (userInfo == null)
            {
                userInfo = new UserInfo();
                userSchema = UserSchema.Portal;
            }
            else
            {
                userSchema = userInfo.UserSchema;
            }

            byte[] content = null;
            string xmlinfo = xmlInformation;

            xmlInformation = xmlInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\"><Underscore xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##bus##");
            xmlInformation = xmlInformation.Replace("</Underscore></Bold>", "##bue##");

            xmlInformation = xmlInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "#bst#");
            xmlInformation = xmlInformation.Replace("</Bold>", "#be#");

            xmlInformation = xmlInformation.Replace("<Underscore xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##us##");
            xmlInformation = xmlInformation.Replace("</Underscore>", "##ue##");
            xmlInformation = xmlInformation.Replace("<Italic xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##is##");
            xmlInformation = xmlInformation.Replace("</Italic>", "##ie##");

            xmlInformation = xmlInformation.Replace("<NotesForHaulier xmlns=\"http://www.esdal.com/schemas/core/movement\">##bs####us##", "<NotesForHaulier xmlns=\"http://www.esdal.com/schemas/core/movement\">##bssbr####us##");

            xmlInformation = xmlInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\"><Underscore>", "##bs##");

            xmlInformation = xmlInformation.Replace("<OnBehalfOf xmlns=\"http://www.esdal.com/schemas/core/movement\"> </OnBehalfOf>", "");
            xmlInformation = xmlInformation.Replace("<OnBehalfOf> </OnBehalfOf>", "");

            xmlInformation = xmlInformation.Replace("<Para xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##ps##");
            xmlInformation = xmlInformation.Replace("</Para>", "##pe##");

            xmlInformation = xmlInformation.Replace("<BulletedText xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##bts##");
            xmlInformation = xmlInformation.Replace("</BulletedText>", "##bte##");

            //Start For Bug #4319 </Br> tag
            xmlInformation = xmlInformation.Replace("<Br />", "##br##").Replace("<Br></Br>", "##br##");
            //End For Bug #4319 </Br> tag

            xmlInformation = ConvertCountryLettersToSymbol(xmlInformation);

            StringReader stringReader = new StringReader(xmlInformation);
            XmlReader xmlReader = XmlReader.Create(stringReader);

            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(fileName);

            StringWriter sw = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(sw);

            XsltArgumentList argsList = new XsltArgumentList();

            argsList.AddParam("Contact_ID", "", objcontact == null ? 0 : objcontact.ContactId);
            argsList.AddParam("Organisation_ID", "", objcontact == null ? 0 : objcontact.OrganisationId);

            argsList.AddParam("DocType", "", "PDF");
            argsList.AddParam("UnitType", "", routePlanUnits);

            xslt.Transform(xmlReader, argsList, writer, null);

            writer.Close();

            string Attr = "id=\"hdr_img\"";

            string ImgFilePath = Attr + " src=\"" + Convert.ToString(ConfigurationSettings.AppSettings["DocumentImagePath"]) + "\"";

            string outputString = Convert.ToString(sw);

            outputString = outputString.Replace(Attr, ImgFilePath);

            outputString = outputString.Replace("##bus##", "<br/><br/><b><u>");
            outputString = outputString.Replace("##bue##", "</u></b> ");

            outputString = outputString.Replace("##bss##", "<b>");
            outputString = outputString.Replace("##bssbr##", "<b>");

            outputString = outputString.Replace("#bst#", "<b>");
            outputString = outputString.Replace("#be#", "</b>");

            outputString = outputString.Replace("##is##", "<i>");
            outputString = outputString.Replace("##ie##", "</i>");
            outputString = outputString.Replace("##us##", "<u>");
            outputString = outputString.Replace("##ue##", "</u> ");

            outputString = outputString.Replace("##ps##", "<p>");
            outputString = outputString.Replace("##pe##", "</p>");

            outputString = outputString.Replace("##bts##", "<ul><li>");
            outputString = outputString.Replace("##bte##", "</li></ul>");

            //Start For Bug #4319 </Br> tag
            outputString = outputString.Replace("##br##", "<Br />");
            //End For Bug #4319 </Br> tag

            outputString = outputString.Replace("&amp;nbsp;", " ");

            outputString = outputString.Replace("<b />", "");

            xmlinfo = xmlinfo.Replace("##bss##", "<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">");
            xmlinfo = xmlinfo.Replace("##be##", "</Bold>");

            xmlinfo = xmlinfo.Replace(">?<", ">\u2002<");
            xmlinfo = xmlinfo.Replace(">?##**##", ">##**##");

            byte[] XMLByteArrayData = Encoding.ASCII.GetBytes(xmlinfo);

            GenerateEmailgetParams emailgetParams = new GenerateEmailgetParams
            {
                HtmlContent = outputString,
                AttachmentData = XMLByteArrayData
            };
            
            return emailgetParams;
        }
        #endregion

        #region GenerateWord
        public static GenerateEmailgetParams GenerateWord(int notificationID, int docType, string xmlInformation, string fileName, string esDALRefNo, long organisationID, string docfileName, ContactModel objcontact = null, UserInfo userInfo = null, int icaStatus = 277001, bool indemnity = false, bool ImminentMovestatus = false, int routePlanUnits = 692001, bool generateFlag = true)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("GenerateWord started successfully with parameters esdalRef : {0}, Notifid : {1}, OrgId : {2}, ICAStat : {3}", esDALRefNo, notificationID, organisationID, icaStatus));
            string userSchema = "";
            if (userInfo == null)
            {
                userInfo = new UserInfo();
                userSchema = UserSchema.Portal;
            }
            else
            {
                userSchema = userInfo.UserSchema;
            }

            byte[] DocByteArrayData = null;
            string xmlinfo = xmlInformation;

            xmlInformation = xmlInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "#bst#");
            xmlInformation = xmlInformation.Replace("</Bold>", "#be#");

            xmlInformation = xmlInformation.Replace("<Underscore xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##us##");
            xmlInformation = xmlInformation.Replace("</Underscore>", "##ue##");
            xmlInformation = xmlInformation.Replace("<Italic xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##is##");
            xmlInformation = xmlInformation.Replace("</Italic>", "##ie##");

            xmlInformation = xmlInformation.Replace("<NotesForHaulier xmlns=\"http://www.esdal.com/schemas/core/movement\">##bs####us##", "<NotesForHaulier xmlns=\"http://www.esdal.com/schemas/core/movement\">##bssbr####us##");

            xmlInformation = xmlInformation.Replace("<OnBehalfOf xmlns=\"http://www.esdal.com/schemas/core/movement\"> </OnBehalfOf>", "");
            xmlInformation = xmlInformation.Replace("<OnBehalfOf> </OnBehalfOf>", "");

            xmlInformation = xmlInformation.Replace("<Para xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##ps##");
            xmlInformation = xmlInformation.Replace("</Para>", "##pe##");

            xmlInformation = xmlInformation.Replace("<BulletedText xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##bts##");
            xmlInformation = xmlInformation.Replace("</BulletedText>", "##bte##");

            xmlInformation = ConvertCountryLettersToSymbol(xmlInformation);

            StringReader stringReader = new StringReader(xmlInformation);

            XmlReader xmlReader = XmlReader.Create(stringReader);

            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(fileName);

            StringWriter sw = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(sw);

            if (docfileName.Contains("2D") || docfileName.Contains("Amendment") || docfileName.Contains("FormVR1"))
            {
                xslt.Transform(xmlReader, null, writer, null);
            }
            else
            {
                XsltArgumentList argsList = new XsltArgumentList();
                argsList.AddParam("Contact_ID", "", objcontact == null ? 0 : objcontact.ContactId);
                argsList.AddParam("Organisation_ID", "", objcontact == null ? 0 : objcontact.OrganisationId);
                argsList.AddParam("UnitType", "", routePlanUnits);
                xslt.Transform(xmlReader, argsList, writer, null);
            }

            writer.Close();
            
            string outputString = Convert.ToString(sw);

            string Attr = "id=\"hdr_img\"";
            string ImgFilePath = Attr + " src=\"" + Convert.ToString(ConfigurationSettings.AppSettings["DocumentImagePath"]) + "\"";

            outputString = outputString.Replace(Attr, ImgFilePath);

            outputString = outputString.Replace("##bss##", "<b>");
            outputString = outputString.Replace("##bssbr##", "<b>");

            outputString = outputString.Replace("#bst#", "<b>");
            outputString = outputString.Replace("#be#", "</b>");

            outputString = outputString.Replace("##is##", "<i>");
            outputString = outputString.Replace("##ie##", "</i>");
            outputString = outputString.Replace("##us##", "<u>");
            outputString = outputString.Replace("##ue##", "</u>: ");

            outputString = outputString.Replace("##ps##", "<p>");
            outputString = outputString.Replace("##pe##", "</p>");

            outputString = outputString.Replace("##bts##", "<ul><li>");
            outputString = outputString.Replace("##bte##", "</li></ul>");

            outputString = outputString.Replace("<b />", "");

            outputString = outputString.Replace("FACSIMILE MESSAGE", "Mail");
            outputString = outputString.Replace("?", "");

            outputString = outputString.Replace("&#xA0;", "");

            outputString = outputString.Replace("FACSIMILE MESSAGE", "Mail");
            outputString = outputString.Replace("?", "");

            xmlinfo = xmlinfo.Replace("##bss##", "<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">");
            xmlinfo = xmlinfo.Replace("##be##", "</Bold>");

            xmlinfo = xmlinfo.Replace(">?<", ">\u2002<");
            xmlinfo = xmlinfo.Replace(">?##**##", ">##**##");

            byte[] XMLByteArrayData = Encoding.ASCII.GetBytes(xmlinfo);

            GenerateEmailgetParams emailgetParams = new GenerateEmailgetParams
            {
                HtmlContent = outputString,
                AttachmentData = XMLByteArrayData
            };            

            return emailgetParams;
        }
        #endregion

        #region GenerateEMAIL
        public static GenerateEmailgetParams GenerateEMAIL(int notificationID, int docType, string xmlInformation, string fileName, string esDALRefNo, long organisationID, ContactModel objcontact, string docfileName, bool transmitMethodCallReq, UserInfo userInfo = null, int icaStatus = 277001, bool indemnity = false, int xmlAttach = 0, bool ImminentMovestatus = false, int routePlanUnits = 692001, long projectstatus = 307003)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("GenerateEMAIL started successfully with parameters esdalRef : {0}, Notifid : {1}, OrgId : {2}, ICAStat : {3}", esDALRefNo, notificationID, organisationID, icaStatus));
            string userSchema = "";
            int VehicleUnits = 0;
            if (userInfo == null)
            {
                userInfo = new UserInfo();
                userSchema = UserSchema.Portal;
            }
            else
            {
                userSchema = userInfo.UserSchema;
            }


            string xmlinfo = xmlInformation;

            xmlInformation = xmlInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\"><Underscore xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##bus##");
            xmlInformation = xmlInformation.Replace("</Underscore></Bold>", "##bue##");

            xmlInformation = xmlInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "#bst#");
            xmlInformation = xmlInformation.Replace("</Bold>", "#be#");

            xmlInformation = xmlInformation.Replace("<Underscore xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##us##");
            xmlInformation = xmlInformation.Replace("</Underscore>", "##ue##");
            xmlInformation = xmlInformation.Replace("<Italic xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##is##");
            xmlInformation = xmlInformation.Replace("</Italic>", "##ie##");

            xmlInformation = xmlInformation.Replace("<NotesForHaulier xmlns=\"http://www.esdal.com/schemas/core/movement\">##bs####us##", "<NotesForHaulier xmlns=\"http://www.esdal.com/schemas/core/movement\">##bssbr####us##");

            xmlInformation = xmlInformation.Replace("<OnBehalfOf xmlns=\"http://www.esdal.com/schemas/core/movement\"> </OnBehalfOf>", "");
            xmlInformation = xmlInformation.Replace("<OnBehalfOf> </OnBehalfOf>", "");

            xmlInformation = xmlInformation.Replace("<Para xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##ps##");
            xmlInformation = xmlInformation.Replace("</Para>", "##pe##");

            xmlInformation = xmlInformation.Replace("<BulletedText xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##bts##");
            xmlInformation = xmlInformation.Replace("</BulletedText>", "##bte##");
            //Start For Bug #4319 </Br> tag
            xmlInformation = xmlInformation.Replace("<Br />", "##br##").Replace("<Br></Br>", "##br##");
            //End For Bug #4319 </Br> tag

            xmlInformation = ConvertCountryLettersToSymbol(xmlInformation);

            // Add busines logic to show logged in user Affected Parties
            GenerateDocument genDocument = new GenerateDocument();

            if (xmlInformation != null && xmlInformation != string.Empty && notificationID == 0)
            {
                if (projectstatus == 307003 || projectstatus == 307004)
                {
                    if (xmlInformation.IndexOf("</Proposal>") != -1 && !objcontact.ISPolice)
                    {
                        xmlInformation = genDocument.GetLoggedInUserAffectedStructureDetailsByESDALReference(xmlInformation, esDALRefNo, userInfo, userSchema, "proposal", objcontact.OrganisationId);
                    }

                    else if (xmlInformation.IndexOf("</AgreedRoute>") != -1)
                    {
                        xmlInformation = genDocument.GetLoggedInUserAffectedStructureDetailsByESDALReference(xmlInformation, esDALRefNo, userInfo, userSchema, "agreed", objcontact.OrganisationId);
                    }
                }
                else if (projectstatus == 307005 || projectstatus == 307006 || projectstatus == 307007)
                {
                    if (xmlInformation.IndexOf("</AgreedRoute>") != -1)
                    {
                        xmlInformation = genDocument.GetLoggedInUserAffectedStructureDetailsByESDALReference(xmlInformation, esDALRefNo, userInfo, userSchema, "agreed", objcontact.OrganisationId);
                    }
                }
            }
            else if (xmlInformation != null && xmlInformation != string.Empty && notificationID > 0)
            {
                if (xmlInformation.IndexOf("</OutboundNotification>") != -1)
                {
                    xmlInformation = genDocument.GetLoggedInUserAffectedStructureDetails(xmlInformation, notificationID, userInfo, objcontact.OrganisationId);
                }
            }
            // Logic ends here

            StringReader stringReader = new StringReader(xmlInformation);
            XmlReader xmlReader = XmlReader.Create(stringReader);

            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(fileName);

            StringWriter sw = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(sw);

            XsltArgumentList argsList = new XsltArgumentList();

            if (objcontact.ContactId != 0)
            {
                VehicleUnits = GetVehicleUnits(objcontact.ContactId, objcontact.OrganisationId);
            }


            argsList.AddParam("Contact_ID", "", objcontact == null ? 0 : objcontact.ContactId);

            if (VehicleUnits != 0 && VehicleUnits != null)
            {
                argsList.AddParam("UnitType", "", VehicleUnits);
            }
            else
            {
                argsList.AddParam("UnitType", "", routePlanUnits);
            }

            argsList.AddParam("DocType", "", "EMAIL");

            argsList.AddParam("Organisation_ID", "", objcontact == null ? 0 : objcontact.OrganisationId);

            xslt.Transform(xmlReader, argsList, writer, null);

            writer.Close();
            
            string outputString = Convert.ToString(sw);

            string Attr = "id=\"hdr_img\"";
            string ImgFilePath = Attr + " src=\"" + System.Configuration.ConfigurationSettings.AppSettings["DocumentImagePath"].ToString() + "\"";

            outputString = outputString.Replace(Attr, ImgFilePath);

            outputString = outputString.Replace("##bus##", "<br/><br/><b><u>");
            outputString = outputString.Replace("##bue##", "</u></b> ");

            outputString = outputString.Replace("##bss##", "<b>");
            outputString = outputString.Replace("##bssbr##", "<b>");

            outputString = outputString.Replace("#bst#", "<b>");
            outputString = outputString.Replace("#be#", "</b>");

            outputString = outputString.Replace("##is##", "<i>");
            outputString = outputString.Replace("##ie##", "</i>");
            outputString = outputString.Replace("##us##", "<u>");
            outputString = outputString.Replace("##ue##", "</u> ");

            outputString = outputString.Replace("##ps##", "<p>");
            outputString = outputString.Replace("##pe##", "</p>");

            outputString = outputString.Replace("##bts##", "<ul><li>");
            outputString = outputString.Replace("##bte##", "</li></ul>");

            //Start For Bug #4319 </Br> tag
            outputString = outputString.Replace("##br##", "<Br />");
            //End For Bug #4319 </Br> tag

            outputString = outputString.Replace("FACSIMILE MESSAGE", "Mail");

            outputString = outputString.Replace("?", "");
            outputString = outputString.Replace("&#xA0;", "");
            outputString = outputString.Replace("&amp;nbsp;", " ");

            outputString = outputString.Replace("<b />", "");
            outputString = outputString.Replace("#b#", "<b>");

            NotificationContacts notiContacts = new NotificationContacts();

            if (transmitMethodCallReq)
            {
                notiContacts.ContactId = objcontact.ContactId;
                notiContacts.ContactName = objcontact.FullName;
                notiContacts.Email = objcontact.Email;
                notiContacts.Fax = objcontact.Fax;
                notiContacts.Reason = objcontact.Reason;
                notiContacts.ContactPreference = ContactPreference.emailHtml; // contactPreference.onlineInboxOnly; the contact preference is changed to send email from TransmitNotification
                notiContacts.OrganisationId = objcontact.OrganisationId;
                notiContacts.NotificationId = notificationID; //adding notification id to the notification contact object.
                notiContacts.OrganistationName = objcontact.Organisation; // organisation name of contact . Needed in case of a manually added contact.
            }
            
            xmlinfo = xmlinfo.Replace(">?<", ">\u2002<");
            xmlinfo = xmlinfo.Replace(">?##**##", ">##**##");

            byte[] XMLByteArrayData = Encoding.ASCII.GetBytes(xmlinfo);

            GenerateEmailgetParams emailgetParams = new GenerateEmailgetParams
            {
                HtmlContent= outputString,
                AttachmentData= XMLByteArrayData
            };
            

            
            byte[] DocByteArrayData = Encoding.UTF8.GetBytes(outputString);
            if (DocByteArrayData != null)
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "GenerateEMAIL completed successfully");
            }

            return emailgetParams;
        }
        #endregion

        #region ConvertCountryLettersToSymbol
        private static string ConvertCountryLettersToSymbol(string xmlDocument)
        {
            xmlDocument = xmlDocument.Replace("#@GBP@#", "£");
            xmlDocument = xmlDocument.Replace("#@Pound@#", "€");
            xmlDocument = xmlDocument.Replace("#@GEL@#", "₾");
            xmlDocument = xmlDocument.Replace("#@BGN@#", "лв");
            xmlDocument = xmlDocument.Replace("#@Turkey@#", "₺");

            return xmlDocument;
        }
        #endregion

        #region GeneratePDF
        public byte[] GeneratePDF(int notificationID, int docType, string xmlInformation, string fileName, string esDALRefNo, long organisationID, int contactID, string docfileName, bool isHaulier = false, string organisationName = "", string HAReference = "", int routePlanUnits = 692001, string documentType = "PDF", UserInfo userInfo = null, string userType = "")
        {
            byte[] content = null;
            int VehicleUnits = 0;
            ContactModel contactInfo;
            ContactModel contactDetails=new ContactModel();
            int struct_contactid;
            string xmlinfo = xmlInformation;
            if (xmlInformation != string.Empty)
            {
                XmlDocument Document = new XmlDocument();

                Document.LoadXml(xmlInformation);
                XmlNodeList parent = Document.GetElementsByTagName("Recipients");
                foreach (XmlNode children in parent)
                {
                    foreach (XmlNode childrenNode in children)
                    {
                        contactInfo = new ContactModel
                        {
                            Organisation = childrenNode.ChildNodes[1] == null ? string.Empty : childrenNode.ChildNodes[1].InnerText.Contains("##**##")?
                                childrenNode.ChildNodes[1].InnerText.Substring(childrenNode.ChildNodes[1].InnerText.LastIndexOf('#') + 1) :
                               childrenNode.ChildNodes[1].InnerText,
                            ContactId = childrenNode.Attributes["ContactId"] == null ? 0 : childrenNode.Attributes["ContactId"].Value.Contains("##**##")?
                                Convert.ToInt32(childrenNode.Attributes["ContactId"].Value.Substring(childrenNode.Attributes["ContactId"].Value.LastIndexOf('#') + 1)) :
                                Convert.ToInt32(childrenNode.Attributes["ContactId"].Value),
                        };
                        if(organisationName==contactInfo.Organisation)
                        {
                            struct_contactid = contactInfo.ContactId;
                            contactDetails = GetContactDetails(struct_contactid);
                            break;
                        }
                    }
                }
            }

            xmlInformation = xmlInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\"><Underscore xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##bus##");
            xmlInformation = xmlInformation.Replace("</Underscore></Bold>", "##bue##");

            xmlInformation = xmlInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "#bst#");
            xmlInformation = xmlInformation.Replace("</Bold>", "#be#");

            xmlInformation = xmlInformation.Replace("<Underscore xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##us##");
            xmlInformation = xmlInformation.Replace("<Underscore>", "##us##"); // RM#5232 changes
            xmlInformation = xmlInformation.Replace("</Underscore>", "##ue##");
            xmlInformation = xmlInformation.Replace("<Italic xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##is##");
            xmlInformation = xmlInformation.Replace("<Italic>", "##is##");// RM#5232 changes
            xmlInformation = xmlInformation.Replace("</Italic>", "##ie##");

            xmlInformation = xmlInformation.Replace("<NotesForHaulier xmlns=\"http://www.esdal.com/schemas/core/movement\">##bs####us##", "<NotesForHaulier xmlns=\"http://www.esdal.com/schemas/core/movement\">##bssbr####us##");

            xmlInformation = xmlInformation.Replace("<Underscore>", "");

            xmlInformation = xmlInformation.Replace("<OnBehalfOf xmlns=\"http://www.esdal.com/schemas/core/movement\"> </OnBehalfOf>", "");
            xmlInformation = xmlInformation.Replace("<OnBehalfOf> </OnBehalfOf>", "");

            xmlInformation = xmlInformation.Replace("<Para xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##ps##");
            xmlInformation = xmlInformation.Replace("</Para>", "##pe##");

            xmlInformation = xmlInformation.Replace("<BulletedText xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##bts##");
            xmlInformation = xmlInformation.Replace("</BulletedText>", "##bte##");

            //Start For Bug #4319 </Br> tag
            xmlInformation = xmlInformation.Replace("<Br />", "##br##").Replace("<Br></Br>", "##br##");
            //End For Bug #4319 </Br> tag

            xmlInformation = ConvertCountryLettersToSymbol(xmlInformation);

            StringReader stringReader = new StringReader(xmlInformation);
            XmlReader xmlReader = XmlReader.Create(stringReader);

            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(fileName);

            StringWriter sw = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(sw);

            XsltArgumentList argsList = new XsltArgumentList();

            VehicleUnits = GetVehicleUnits(contactID, (Int32)organisationID);

            argsList.AddParam("Contact_ID", "", contactID);

            if (VehicleUnits != 0)
            {
                argsList.AddParam("UnitType", "", VehicleUnits);
            }
            else
            {
                argsList.AddParam("UnitType", "", routePlanUnits);
            }

            argsList.AddParam("DocType", "", documentType);
            argsList.AddParam("OrganisationName", "", organisationName);
            argsList.AddParam("HAReferenceNumber", "", HAReference);
            argsList.AddParam("ContactPhoneNumber", "", contactDetails.PhoneNumber == null?"": contactDetails.PhoneNumber);
            argsList.AddParam("ContactEmail", "", contactDetails.Email==null?"": contactDetails.PhoneNumber);
            xslt.Transform(xmlReader, argsList, writer, null);

            writer.Close();

            string Attr = "id=\"hdr_img\"";
            string ImgFilePath = "";

            ImgFilePath = Attr + $" src='{ConfigurationManager.AppSettings["APIGatewayUrl"]}" +
                $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                $"{ConfigurationManager.AppSettings["DocumentImagePath"].Replace("~","")}'";

           
            string outputString = Convert.ToString(sw);

            outputString = outputString.Replace(Attr, ImgFilePath);

            outputString = outputString.Replace("##bus##", "<br/><br/><b><u>");
            outputString = outputString.Replace("##bue##", "</u></b> ");

            outputString = outputString.Replace("##bss##", "<b>");
            outputString = outputString.Replace("##bssbr##", "<b>");

            outputString = outputString.Replace("#bst#", "<b>");
            outputString = outputString.Replace("#be#", "</b>");

            outputString = outputString.Replace("##is##", "<i>");
            outputString = outputString.Replace("##ie##", "</i>");
            outputString = outputString.Replace("##us##", "<u>");
            outputString = outputString.Replace("##ue##", "</u> ");

            outputString = outputString.Replace("&amp;nbsp;", " ");

            outputString = outputString.Replace("##ps##", "<p>");
            outputString = outputString.Replace("##pe##", "</p>");

            outputString = outputString.Replace("##bts##", "<ul><li>");
            outputString = outputString.Replace("##bte##", "</li></ul>");

            //Start For Bug #4319 </Br> tag
            outputString = outputString.Replace("##br##", "<Br />");
            //End For Bug #4319 </Br> tag

            outputString = outputString.Replace("<b />", "");
            outputString = outputString.Replace("#b#", "<b>");

            outputString = outputString.Replace("?", " ");
            outputString = outputString.Replace("(on behalf of )", " ");

            int n = GetNoofPages(outputString);

            StringReader sr = new StringReader(outputString.Replace("###Noofpages###", n.ToString()));

            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 40f, 10f, 40f, 36);

                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

                StyleSheet style = new StyleSheet();

                htmlparser.SetStyleSheet(style);
                PdfWriter.GetInstance(pdfDoc, myMemoryStream);

                pdfDoc.Open();

                htmlparser.Parse(sr);

                HTMLWorker htmlparser1 = new HTMLWorker(pdfDoc);
                StyleSheet style1 = new StyleSheet();
                htmlparser1.SetStyleSheet(style1);

                pdfDoc.Close();

                content = AddPageNumbers(myMemoryStream.ToArray());
            }
            
            xmlinfo = xmlinfo.Replace(">?<", ">\u2002<");
            xmlinfo = xmlinfo.Replace(">?##**##", ">##**##");

            return content;
        }
        #endregion

        #region GeneratePDF1
        public string GeneratePDF1(int notificationID, int docType, string xmlInformation, string fileName, string esDALRefNo, long organisationID, int contactID, string docfileName, bool isHaulier = false, string organisationName = "", string HAReference = "", int routePlanUnits = 692001, string documentType = "PDF", UserInfo userInfo = null, string userType = "")
        {
            byte[] content = null;
            int VehicleUnits = 0;
            ContactModel contactInfo;
            ContactModel contactDetails = new ContactModel();
            int struct_contactid;
            string xmlinfo = xmlInformation;
            if (xmlInformation != string.Empty)
            {
                XmlDocument Document = new XmlDocument();

                Document.LoadXml(xmlInformation);
                XmlNodeList parent = Document.GetElementsByTagName("Recipients");
                foreach (XmlNode children in parent)
                {
                    foreach (XmlNode childrenNode in children)
                    {
                        contactInfo = new ContactModel
                        {
                            Organisation = childrenNode.ChildNodes[1] == null ? string.Empty : childrenNode.ChildNodes[1].InnerText.Contains("##**##") ?
                                childrenNode.ChildNodes[1].InnerText.Substring(childrenNode.ChildNodes[1].InnerText.LastIndexOf('#') + 1) :
                               childrenNode.ChildNodes[1].InnerText,
                            ContactId = childrenNode.Attributes["ContactId"] == null ? 0 : childrenNode.Attributes["ContactId"].Value.Contains("##**##") ?
                                Convert.ToInt32(childrenNode.Attributes["ContactId"].Value.Substring(childrenNode.Attributes["ContactId"].Value.LastIndexOf('#') + 1)) :
                                Convert.ToInt32(childrenNode.Attributes["ContactId"].Value),
                        };
                        if (organisationName == contactInfo.Organisation)
                        {
                            struct_contactid = contactInfo.ContactId;
                            contactDetails = GetContactDetails(struct_contactid);
                            break;
                        }
                    }
                }
            }

            xmlInformation = xmlInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\"><Underscore xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##bus##");
            xmlInformation = xmlInformation.Replace("</Underscore></Bold>", "##bue##");

            xmlInformation = xmlInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "#bst#");
            xmlInformation = xmlInformation.Replace("</Bold>", "#be#");

            xmlInformation = xmlInformation.Replace("<Underscore xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##us##");
            xmlInformation = xmlInformation.Replace("<Underscore>", "##us##"); // RM#5232 changes
            xmlInformation = xmlInformation.Replace("</Underscore>", "##ue##");
            xmlInformation = xmlInformation.Replace("<Italic xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##is##");
            xmlInformation = xmlInformation.Replace("<Italic>", "##is##");// RM#5232 changes
            xmlInformation = xmlInformation.Replace("</Italic>", "##ie##");

            xmlInformation = xmlInformation.Replace("<NotesForHaulier xmlns=\"http://www.esdal.com/schemas/core/movement\">##bs####us##", "<NotesForHaulier xmlns=\"http://www.esdal.com/schemas/core/movement\">##bssbr####us##");

            xmlInformation = xmlInformation.Replace("<Underscore>", "");

            xmlInformation = xmlInformation.Replace("<OnBehalfOf xmlns=\"http://www.esdal.com/schemas/core/movement\"> </OnBehalfOf>", "");
            xmlInformation = xmlInformation.Replace("<OnBehalfOf> </OnBehalfOf>", "");

            xmlInformation = xmlInformation.Replace("<Para xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##ps##");
            xmlInformation = xmlInformation.Replace("</Para>", "##pe##");

            xmlInformation = xmlInformation.Replace("<BulletedText xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##bts##");
            xmlInformation = xmlInformation.Replace("</BulletedText>", "##bte##");

            //Start For Bug #4319 </Br> tag
            xmlInformation = xmlInformation.Replace("<Br />", "##br##").Replace("<Br></Br>", "##br##");
            //End For Bug #4319 </Br> tag

            xmlInformation = ConvertCountryLettersToSymbol(xmlInformation);

            StringReader stringReader = new StringReader(xmlInformation);
            XmlReader xmlReader = XmlReader.Create(stringReader);

            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(fileName);

            StringWriter sw = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(sw);

            XsltArgumentList argsList = new XsltArgumentList();

            VehicleUnits = GetVehicleUnits(contactID, (Int32)organisationID);

            argsList.AddParam("Contact_ID", "", contactID);

            if (VehicleUnits != 0)
            {
                argsList.AddParam("UnitType", "", VehicleUnits);
            }
            else
            {
                argsList.AddParam("UnitType", "", routePlanUnits);
            }

            argsList.AddParam("DocType", "", documentType);
            argsList.AddParam("OrganisationName", "", organisationName);
            argsList.AddParam("HAReferenceNumber", "", HAReference);
            argsList.AddParam("ContactPhoneNumber", "", contactDetails.PhoneNumber == null ? "" : contactDetails.PhoneNumber);
            argsList.AddParam("ContactEmail", "", contactDetails.Email == null ? "" : contactDetails.PhoneNumber);
            xslt.Transform(xmlReader, argsList, writer, null);

            writer.Close();

            string Attr = "id=\"hdr_img\"";
            string ImgFilePath = "";
            if (userInfo != null)
            {
                if (userInfo.UserTypeId == 696008 || userInfo.UserTypeId == 696001)
                {
                    ImgFilePath = Attr + " src=\"" + System.AppDomain.CurrentDomain.BaseDirectory + "Content\\Images\\ESDAL 2 Logo_org_He_Branding.png" + "\"";
                }
                else
                {
                    ImgFilePath = Attr + " src=\"" + System.AppDomain.CurrentDomain.BaseDirectory + "Content\\Images\\ESDAL 2Logo.png" + "\"";
                }
            }
            else
            {
                ImgFilePath = Attr + " src=\"" + System.AppDomain.CurrentDomain.BaseDirectory + "Content\\Images\\logo.png" + "\"";
            }

            string outputString = Convert.ToString(sw);

            outputString = outputString.Replace(Attr, ImgFilePath);

            outputString = outputString.Replace("##bus##", "<br/><br/><b><u>");
            outputString = outputString.Replace("##bue##", "</u></b> ");

            outputString = outputString.Replace("##bss##", "<b>");
            outputString = outputString.Replace("##bssbr##", "<b>");

            outputString = outputString.Replace("#bst#", "<b>");
            outputString = outputString.Replace("#be#", "</b>");

            outputString = outputString.Replace("##is##", "<i>");
            outputString = outputString.Replace("##ie##", "</i>");
            outputString = outputString.Replace("##us##", "<u>");
            outputString = outputString.Replace("##ue##", "</u> ");

            outputString = outputString.Replace("&amp;nbsp;", " ");

            outputString = outputString.Replace("##ps##", "<p>");
            outputString = outputString.Replace("##pe##", "</p>");

            outputString = outputString.Replace("##bts##", "<ul><li>");
            outputString = outputString.Replace("##bte##", "</li></ul>");

            //Start For Bug #4319 </Br> tag
            outputString = outputString.Replace("##br##", "<Br />");
            //End For Bug #4319 </Br> tag

            outputString = outputString.Replace("<b />", "");
            outputString = outputString.Replace("#b#", "<b>");

            outputString = outputString.Replace("?", " ");

            int n = GetNoofPages(outputString);

            StringReader sr = new StringReader(outputString.Replace("###Noofpages###", n.ToString()));

            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 40f, 10f, 40f, 36);

                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

                StyleSheet style = new StyleSheet();

                htmlparser.SetStyleSheet(style);
                PdfWriter.GetInstance(pdfDoc, myMemoryStream);

                pdfDoc.Open();

                htmlparser.Parse(sr);

                HTMLWorker htmlparser1 = new HTMLWorker(pdfDoc);
                StyleSheet style1 = new StyleSheet();
                htmlparser1.SetStyleSheet(style1);

                pdfDoc.Close();

                content = AddPageNumbers(myMemoryStream.ToArray());
            }

            xmlinfo = xmlinfo.Replace(">?<", ">\u2002<");
            xmlinfo = xmlinfo.Replace(">?##**##", ">##**##");

            byte[] XMLByteArrayData = Encoding.ASCII.GetBytes(xmlinfo);

            //Some data is stored in gzip format, so we need to unzip then load it.
            byte[] CompressXMLString = STP.Common.General.XsltTransformer.CompressData(XMLByteArrayData);
            string userSchema = UserSchema.Portal;

            //if (!isHaulier)
            //{
            //    SaveDocument(notificationID, docType, organisationID, esDALRefNo, contactID, CompressXMLString, userSchema);
            //}

            return outputString.Replace("###Noofpages###", n.ToString());
        }
        #endregion

        #region AddPageNumbers
        public static byte[] AddPageNumbers(byte[] pdf)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(pdf, 0, pdf.Length);

            PdfReader reader = new PdfReader(pdf);   // we create a reader for a certain document

            int n = reader.NumberOfPages;            // we retrieve the total number of pages

            iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 40f, 10f, 40f, 36);    // step 1: creation of a document-object

            PdfWriter writer = PdfWriter.GetInstance(document, ms);      // step 2: we create a writer that listens to the document

            document.Open();     // step 3: we open the document

            PdfContentByte cb = writer.DirectContent;  // step 4: we add content

            int p = 0;
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                document.NewPage();
                p++;

                PdfImportedPage importedPage = writer.GetImportedPage(reader, page);

                cb.AddTemplate(importedPage, 0, 0, false);

                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb.BeginText();
                cb.SetFontAndSize(bf, 8);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Page " + p + " of " + n, 525, 15, 0);
                cb.EndText();
            }
            document.Close();      // step 5: we close the document

            return ms.ToArray();
        }
        #endregion

        #region GetNoofPages
        public static int GetNoofPages(String sw)
        {
            int n = 0;
            using (MemoryStream myMemoryStream = new MemoryStream())
            {

                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 40f, 10f, 40f, 36);

                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

                PdfWriter.GetInstance(pdfDoc, myMemoryStream);

                pdfDoc.Open();

                StringReader sr = new StringReader(sw);

                htmlparser.Parse(sr);
                sr.Close();
                pdfDoc.Close();

                MemoryStream ms = new MemoryStream();
                ms.Write(myMemoryStream.ToArray(), 0, myMemoryStream.ToArray().Length);
                PdfReader reader = new PdfReader(myMemoryStream.ToArray());   // we create a reader for a certain document
                n = reader.NumberOfPages;            // we retrieve the total number of pages
                reader.Close();
                return n;
            }

        }
        #endregion

        #region SaveDocument
        public static long SaveDocument(int notificationID, int documentType, long organisationID, string esDALRef, int contactID, byte[] exportByteArrayData, string userSchema = "", UserInfo userInfo = null, NotificationContacts objcontact = null,long projectId=0,int revisionNo=0,int versionno=0)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("SaveDocument started successfully with parameters esdalRef : {0}, Notifid : {1}, DocType : {2}, OrgId : {3}, contact : {4}", esDALRef, notificationID, documentType, organisationID, contactID));
            long documentId = long.MinValue;
            OutboundDocuments outbounddocs = new OutboundDocuments
            {
                DocType = documentType,
                DocumentInBytes = exportByteArrayData,
                NotificationID = notificationID,
                OrganisationID = organisationID,
                EsdalReference = esDALRef,
                ContactID = contactID
            };

            if (userSchema == "" && userInfo != null)
                userSchema = userInfo.UserSchema;
            documentId = OutBoundDocumentDOA.AddManageDocument(outbounddocs, userSchema);
            if (organisationID != 0) //No collaboration for manually added parties
                OutBoundDocumentDOA.InsertCollaboration(outbounddocs, documentId, userSchema);
            return documentId;
        }
        #endregion

        #region GetRevisedAgreementAxleWeightListPosition
        public static AggreedRouteXSD.SummaryAxleStructureAxleWeightListPosition[] GetRevisedAgreementAxleWeightListPosition(List<VehComponentAxles> vehicleComponentAxlesList)
        {
            List<AggreedRouteXSD.SummaryAxleStructureAxleWeightListPosition> sasawlpList = new List<AggreedRouteXSD.SummaryAxleStructureAxleWeightListPosition>();

            List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

            foreach (long component in componentList)
            {
                List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
                                                                            where apca.ComponentId == component
                                                                            orderby apca.AxleNumber ascending
                                                                            select apca).ToList();
                
                List<float> weightList = componentWiseAxleList.Select(x => x.Weight).ToList();

                int count = weightList.Count;
                int localCount = 0;

                float oldDummyweight = 0;
                int oldcountaAxles = 0;

                foreach (float weight in weightList)
                {
                    localCount = localCount + 1;

                    if (oldcountaAxles == 0)
                    {
                        oldDummyweight = weight;

                        oldcountaAxles = oldcountaAxles + 1;
                    }
                    else
                    {
                        if (weight == oldDummyweight)
                        {
                            oldcountaAxles = oldcountaAxles + 1;
                        }
                        else
                        {
                            AggreedRouteXSD.SummaryAxleStructureAxleWeightListPosition sasawlp = new AggreedRouteXSD.SummaryAxleStructureAxleWeightListPosition();
                            AggreedRouteXSD.AxleWeightSummaryStructure awss = new AggreedRouteXSD.AxleWeightSummaryStructure();

                            awss.Value = Convert.ToString(oldDummyweight);
                            awss.AxleCount = Convert.ToString(oldcountaAxles);

                            sasawlp.AxleWeight = awss;

                            sasawlpList.Add(sasawlp);

                            oldcountaAxles = 1;
                            oldDummyweight = weight;
                        }
                    }

                    if (localCount == count)
                    {
                        AggreedRouteXSD.SummaryAxleStructureAxleWeightListPosition sasawlp = new AggreedRouteXSD.SummaryAxleStructureAxleWeightListPosition();
                        AggreedRouteXSD.AxleWeightSummaryStructure awss = new AggreedRouteXSD.AxleWeightSummaryStructure();

                        awss.Value = Convert.ToString(oldDummyweight);
                        awss.AxleCount = Convert.ToString(oldcountaAxles);

                        sasawlp.AxleWeight = awss;

                        sasawlpList.Add(sasawlp);
                    }
                }


            }

            if (sasawlpList.ToArray().Length == 0)
            {
                AggreedRouteXSD.SummaryAxleStructureAxleWeightListPosition sasawlp = new AggreedRouteXSD.SummaryAxleStructureAxleWeightListPosition();
                AggreedRouteXSD.AxleWeightSummaryStructure awss = new AggreedRouteXSD.AxleWeightSummaryStructure();
                awss.Value = "0";
                sasawlp.AxleWeight = awss;
                sasawlpList.Add(sasawlp);
            }

            return sasawlpList.ToArray();
        }
        #endregion

        #region GetRevisedAgreementWheelsPerAxleListPosition
        public static AggreedRouteXSD.SummaryAxleStructureWheelsPerAxleListPosition[] GetRevisedAgreementWheelsPerAxleListPosition(List<VehComponentAxles> vehicleComponentAxlesList)
        {
            List<AggreedRouteXSD.SummaryAxleStructureWheelsPerAxleListPosition> saswpalpList = new List<AggreedRouteXSD.SummaryAxleStructureWheelsPerAxleListPosition>();

            List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

            foreach (long component in componentList)
            {
                List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
                                                                            where apca.ComponentId == component
                                                                            orderby apca.AxleNumber ascending
                                                                            select apca).ToList();
                
                List<short> weightList = componentWiseAxleList.Select(x => x.WheelCount).ToList();

                int count = weightList.Count;
                int localCount = 0;

                short oldDummyweight = 0;
                int oldcountaAxles = 0;

                foreach (short weight in weightList)
                {
                    localCount = localCount + 1;

                    if (oldcountaAxles == 0)
                    {
                        oldDummyweight = weight;

                        oldcountaAxles = oldcountaAxles + 1;
                    }
                    else
                    {
                        if (weight == oldDummyweight)
                        {
                            oldcountaAxles = oldcountaAxles + 1;
                        }
                        else
                        {
                            AggreedRouteXSD.SummaryAxleStructureWheelsPerAxleListPosition saswpalp = new AggreedRouteXSD.SummaryAxleStructureWheelsPerAxleListPosition();
                            AggreedRouteXSD.WheelsPerAxleSummaryStructure wpass = new AggreedRouteXSD.WheelsPerAxleSummaryStructure();

                            wpass.Value = Convert.ToString(oldDummyweight);
                            wpass.AxleCount = Convert.ToString(oldcountaAxles);

                            saswpalp.WheelsPerAxle = wpass;

                            saswpalpList.Add(saswpalp);

                            oldcountaAxles = 1;
                            oldDummyweight = weight;
                        }
                    }

                    if (localCount == count)
                    {
                        AggreedRouteXSD.SummaryAxleStructureWheelsPerAxleListPosition saswpalp = new AggreedRouteXSD.SummaryAxleStructureWheelsPerAxleListPosition();
                        AggreedRouteXSD.WheelsPerAxleSummaryStructure wpass = new AggreedRouteXSD.WheelsPerAxleSummaryStructure();

                        wpass.Value = Convert.ToString(oldDummyweight);
                        wpass.AxleCount = Convert.ToString(oldcountaAxles);

                        saswpalp.WheelsPerAxle = wpass;

                        saswpalpList.Add(saswpalp);
                    }
                }
            }

            if (saswpalpList.ToArray().Length == 0)
            {
                AggreedRouteXSD.SummaryAxleStructureWheelsPerAxleListPosition saswpalp = new AggreedRouteXSD.SummaryAxleStructureWheelsPerAxleListPosition();
                AggreedRouteXSD.WheelsPerAxleSummaryStructure wpass = new AggreedRouteXSD.WheelsPerAxleSummaryStructure();

                wpass.Value = "0";
                saswpalp.WheelsPerAxle = wpass;
                saswpalpList.Add(saswpalp);
            }

            return saswpalpList.ToArray();
        }
        #endregion

        #region GetRevisedAgreementAxleSpacingListPosition
        public static AggreedRouteXSD.SummaryAxleStructureAxleSpacingListPosition[] GetRevisedAgreementAxleSpacingListPosition(List<VehComponentAxles> vehicleComponentAxlesList, bool isSemiVehicle = true)
        {
            List<AggreedRouteXSD.SummaryAxleStructureAxleSpacingListPosition> sasaslpList = new List<AggreedRouteXSD.SummaryAxleStructureAxleSpacingListPosition>();

            List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

            foreach (long component in componentList)
            {
                List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
                                                                            where apca.ComponentId == component
                                                                            select apca).ToList();
                
                List<float> axleSpacingList = componentWiseAxleList.Select(x => x.NextAxleDist).ToList();

                double axleSpacingToFollowing = componentWiseAxleList.Select(x => x.AxleSpacingToFollowing).Distinct().FirstOrDefault();

                int count = axleSpacingList.Count;
                int localCount = 0;

                float oldDummyweight = 0;
                int oldcountaAxles = 0;

                foreach (float axleSpacing in axleSpacingList)
                {
                    localCount = localCount + 1;

                    if (axleSpacing != 0)
                    {
                        if (oldcountaAxles == 0)
                        {
                            oldDummyweight = axleSpacing;

                            oldcountaAxles = oldcountaAxles + 1;
                        }
                        else
                        {
                            if (axleSpacing == oldDummyweight)
                            {
                                oldcountaAxles = oldcountaAxles + 1;
                            }
                            else
                            {
                                AggreedRouteXSD.SummaryAxleStructureAxleSpacingListPosition sasaslp = new AggreedRouteXSD.SummaryAxleStructureAxleSpacingListPosition();
                                AggreedRouteXSD.AxleSpacingSummaryStructure ass = new AggreedRouteXSD.AxleSpacingSummaryStructure();

                                ass.Value = Convert.ToDecimal(oldDummyweight);
                                ass.AxleCount = Convert.ToString(oldcountaAxles);
                                sasaslp.AxleSpacing = ass;
                                sasaslpList.Add(sasaslp);

                                oldcountaAxles = 1;
                                oldDummyweight = axleSpacing;
                            }
                        }
                    }

                    if (localCount == count)
                    {
                        AggreedRouteXSD.SummaryAxleStructureAxleSpacingListPosition sasaslp = new AggreedRouteXSD.SummaryAxleStructureAxleSpacingListPosition();
                        AggreedRouteXSD.AxleSpacingSummaryStructure ass = new AggreedRouteXSD.AxleSpacingSummaryStructure();

                        ass.Value = Convert.ToDecimal(oldDummyweight);
                        ass.AxleCount = Convert.ToString(oldcountaAxles);
                        sasaslp.AxleSpacing = ass;
                        sasaslpList.Add(sasaslp);
                    }
                }

                if (axleSpacingToFollowing != 0.0 && axleSpacingToFollowing > 0 && isSemiVehicle)
                {
                    AggreedRouteXSD.SummaryAxleStructureAxleSpacingListPosition sasaslp = new AggreedRouteXSD.SummaryAxleStructureAxleSpacingListPosition();
                    AggreedRouteXSD.AxleSpacingSummaryStructure ass = new AggreedRouteXSD.AxleSpacingSummaryStructure();

                    ass.Value = Convert.ToDecimal(axleSpacingToFollowing);
                    ass.AxleCount = "1";
                    sasaslp.AxleSpacing = ass;
                    sasaslpList.Add(sasaslp);
                }
            }

            if (sasaslpList.ToArray().Length == 0)
            {
                AggreedRouteXSD.SummaryAxleStructureAxleSpacingListPosition sasaslp = new AggreedRouteXSD.SummaryAxleStructureAxleSpacingListPosition();
                AggreedRouteXSD.AxleSpacingSummaryStructure ass = new AggreedRouteXSD.AxleSpacingSummaryStructure();

                ass.Value = 0;
                sasaslp.AxleSpacing = ass;
                sasaslpList.Add(sasaslp);
            }

            return sasaslpList.ToArray();
        }
        #endregion

        #region GetRevisedAgreementTyreSizeListPosition
        public static AggreedRouteXSD.SummaryAxleStructureTyreSizeListPosition[] GetRevisedAgreementTyreSizeListPosition(List<VehComponentAxles> vehicleComponentAxlesList)
        {
            List<AggreedRouteXSD.SummaryAxleStructureTyreSizeListPosition> sastslpList = new List<AggreedRouteXSD.SummaryAxleStructureTyreSizeListPosition>();

            List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

            foreach (long component in componentList)
            {
                List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
                                                                            where apca.ComponentId == component
                                                                            orderby apca.AxleNumber ascending
                                                                            select apca).ToList();

                List<string> tyreSizeList = componentWiseAxleList.Select(x => x.TyreSize).ToList();

                int count = tyreSizeList.Count;
                int localCount = 0;

                string oldDummyweight = "";
                int oldcountaAxles = 0;

                foreach (string tyreSize in tyreSizeList)
                {
                    localCount = localCount + 1;

                    if (oldcountaAxles == 0)
                    {
                        oldDummyweight = tyreSize;

                        oldcountaAxles = oldcountaAxles + 1;
                    }
                    else
                    {
                        if (tyreSize == oldDummyweight)
                        {
                            oldcountaAxles = oldcountaAxles + 1;
                        }
                        else
                        {
                            AggreedRouteXSD.SummaryAxleStructureTyreSizeListPosition sastslp = new AggreedRouteXSD.SummaryAxleStructureTyreSizeListPosition();
                            AggreedRouteXSD.TyreSizeSummaryStructure tzss = new AggreedRouteXSD.TyreSizeSummaryStructure();

                            tzss.Value = oldDummyweight;
                            tzss.AxleCount = Convert.ToString(oldcountaAxles);
                            sastslp.TyreSize = tzss;
                            sastslpList.Add(sastslp);

                            oldcountaAxles = 1;
                            oldDummyweight = tyreSize;
                        }
                    }

                    if (localCount == count)
                    {
                        AggreedRouteXSD.SummaryAxleStructureTyreSizeListPosition sastslp = new AggreedRouteXSD.SummaryAxleStructureTyreSizeListPosition();
                        AggreedRouteXSD.TyreSizeSummaryStructure tzss = new AggreedRouteXSD.TyreSizeSummaryStructure();

                        tzss.Value = oldDummyweight;
                        tzss.AxleCount = Convert.ToString(oldcountaAxles);
                        sastslp.TyreSize = tzss;
                        sastslpList.Add(sastslp);
                    }
                }
            }

            return sastslpList.ToArray();
        }
        #endregion

        #region GetRevisedAgreementWheelSpacingListPosition
        public static AggreedRouteXSD.SummaryAxleStructureWheelSpacingListPosition[] GetRevisedAgreementWheelSpacingListPosition(List<VehComponentAxles> vehicleComponentAxlesList)
        {
            List<AggreedRouteXSD.SummaryAxleStructureWheelSpacingListPosition> saswslp = new List<AggreedRouteXSD.SummaryAxleStructureWheelSpacingListPosition>();

            List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

            foreach (long component in componentList)
            {
                List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
                                                                            where apca.ComponentId == component
                                                                            orderby apca.AxleNumber ascending
                                                                            select apca).ToList();
                
                List<string> wheelSpacingList = componentWiseAxleList.Select(x => x.WheelSpacingList).ToList();

                int count = wheelSpacingList.Count;
                int localCount = 0;

                string oldDummyweight = "";
                int oldcountaAxles = 0;

                foreach (string tyreSize in wheelSpacingList)
                {
                    localCount = localCount + 1;

                    if (oldcountaAxles == 0)
                    {
                        oldDummyweight = tyreSize;

                        oldcountaAxles = oldcountaAxles + 1;
                    }
                    else
                    {
                        if (tyreSize == oldDummyweight)
                        {
                            oldcountaAxles = oldcountaAxles + 1;
                        }
                        else
                        {
                            AggreedRouteXSD.SummaryAxleStructureWheelSpacingListPosition saswsplp = new AggreedRouteXSD.SummaryAxleStructureWheelSpacingListPosition();
                            AggreedRouteXSD.WheelSpacingSummaryStructure wsss = new AggreedRouteXSD.WheelSpacingSummaryStructure();

                            wsss.Value = oldDummyweight;
                            wsss.AxleCount = Convert.ToString(oldcountaAxles);
                            saswsplp.WheelSpacing = wsss;
                            saswslp.Add(saswsplp);

                            oldcountaAxles = 1;
                            oldDummyweight = tyreSize;
                        }
                    }

                    if (localCount == count)
                    {
                        AggreedRouteXSD.SummaryAxleStructureWheelSpacingListPosition saswsplp = new AggreedRouteXSD.SummaryAxleStructureWheelSpacingListPosition();
                        AggreedRouteXSD.WheelSpacingSummaryStructure wsss = new AggreedRouteXSD.WheelSpacingSummaryStructure();

                        wsss.Value = oldDummyweight;
                        wsss.AxleCount = Convert.ToString(oldcountaAxles);
                        saswsplp.WheelSpacing = wsss;
                        saswslp.Add(saswsplp);
                    }
                }
            }

            return saswslp.ToArray();
        }
        #endregion

        #region GetVehicleMainComponentsType
        public static AggreedRouteXSD.VehicleComponentType GetVehicleMainComponentsType(String Type)
        {
            AggreedRouteXSD.VehicleComponentType vehtype = new AggreedRouteXSD.VehicleComponentType();

            if (Type == "ballast tractor")
            {
                vehtype = AggreedRouteXSD.VehicleComponentType.ballasttractor;
            }
            else if (Type == "conventional tractor")
            {
                vehtype = AggreedRouteXSD.VehicleComponentType.conventionaltractor;
            }
            else if (Type == "semi trailer")
            {
                vehtype = AggreedRouteXSD.VehicleComponentType.semitrailer;
            }
            else if (Type == "drawbar trailer")
            {
                vehtype = AggreedRouteXSD.VehicleComponentType.drawbartrailer;
            }
            else if (Type == "tracked vehicle")
            {
                vehtype = AggreedRouteXSD.VehicleComponentType.trackedvehicle;
            }
            else if (Type == "rigid vehicle")
            {
                vehtype = AggreedRouteXSD.VehicleComponentType.rigidvehicle;
            }
            else if (Type == "spmt")
            {
                vehtype = AggreedRouteXSD.VehicleComponentType.spmt;
            }

            return vehtype;
        }
        #endregion

        #region GetVehicleComponentsType
        public static AggreedRouteXSD.VehicleComponentSubType GetVehicleComponentsType(String Type)
        {
            AggreedRouteXSD.VehicleComponentSubType vehtype = new AggreedRouteXSD.VehicleComponentSubType();
            if (Type == "ballast tractor")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.ballasttractor;
            }
            else if (Type == "conventional tractor")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.conventionaltractor;
            }
            else if (Type == "other tractor")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.othertractor;
            }
            else if (Type == "semi trailer")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.semitrailer;
            }
            else if (Type == "semi low loader")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.semilowloader;
            }
            else if (Type == "trombone trailer")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.trombonetrailer;
            }
            else if (Type == "other semi trailer")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.othersemitrailer;
            }
            else if (Type == "drawbar trailer")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.drawbartrailer;
            }
            else if (Type == "other drawbar trailer")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.otherdrawbartrailer;
            }
            else if (Type == "twin bogies")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.twinbogies;
            }
            else if (Type == "tracked vehicle")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.trackedvehicle;
            }
            else if (Type == "rigid vehicle")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.rigidvehicle;
            }
            else if (Type == "girder set")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.girderset;
            }
            else if (Type == "wheeled load")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.wheeledload;
            }
            else if (Type == "recovery vehicle")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.recoveryvehicle;
            }
            else if (Type == "recovered vehicle")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.recoveredvehicle;
            }
            else if (Type == "mobile crane")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.mobilecrane;
            }
            else if (Type == "engineering plant")
            {
                vehtype = AggreedRouteXSD.VehicleComponentSubType.engineeringplant;
            }
            return vehtype;
        }
        #endregion

        #region NumberToWords
        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }
        #endregion

        #region GetOrganisationDetails
        public static OutboundDocuments GetOrganisationDetails(string esDAlRefNo, string userSchema = UserSchema.Portal)
        {
            OutboundDocuments outbounddocs = new OutboundDocuments();
            outbounddocs.ContactID = 0;
            outbounddocs.OrganisationID = 0;
            userSchema = UserSchema.Portal; // checking from portal side only
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                outbounddocs,
                userSchema + ".GET_ORGANISATION_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("p_ESDAL_REF_NUMBER", esDAlRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 (records, instance) =>
                 {
                     instance.OrganisationID = records.GetLongOrDefault("organisation_id");
                 }
                );

            return outbounddocs;
        }
        #endregion

        #region GetNotificationDetails
        public static OutboundDocuments GetNotificationDetails(int notificationID)
        {
            OutboundDocuments outbounddocs = new OutboundDocuments();
            try
            {
                
                outbounddocs.ContactID = 0;
                outbounddocs.OrganisationID = 0;

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    outbounddocs,
                     UserSchema.Portal + ".GET_NOTIFICATION_DETAILS",
                    parameter =>
                    {
                        parameter.AddWithValue("P_NOTIFICATION_ID", notificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                     (records, instance) =>
                     {
                         instance.OrganisationID = records.GetLongOrDefault("organisation_id");
                         instance.EsdalReference = records.GetStringOrDefault("notification_code");
                     }
                    );  
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{ConfigurationManager.AppSettings["Instance"]} - NotificationDocument/GetNotificationDetails, Exception: {ex}");
                
            }
            return outbounddocs;
        }
        #endregion

        #region GetRecipientDetails
        public static List<ContactModel> GetRecipientDetails(string recipientXMLInformation)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(recipientXMLInformation);

            ContactModel contactInfo;
            List<ContactModel> contactList = new List<ContactModel>();

            XmlNodeList parentNode = xmlDoc.GetElementsByTagName("Recipients");
            foreach (XmlElement childrenNode in parentNode)
            {
                foreach (XmlElement xmlElement in childrenNode)
                {
                    if (xmlElement.Name == "Contact")
                    {
                        XmlElement Contact = xmlElement;
                        if ((Contact != null) && Contact.HasAttribute("ContactId"))
                        {
                            int contactId = 0;
                            bool isPolice = false; // For RM#4340 
                            string orgname = "";
                            string contactName = string.Empty;
                            string reason = string.Empty;
                            string fax = string.Empty;
                            string email = string.Empty;
                            if (xmlElement.Attributes["ContactId"].InnerText.Contains("##**##"))
                                contactId = Convert.ToInt32(xmlElement.Attributes["ContactId"].InnerText.Split("##**##".ToCharArray())[6]);
                            else
                                contactId = Convert.ToInt32(xmlElement.Attributes["ContactId"].InnerText);

                            if (xmlElement.Attributes["IsPolice"].InnerText.Contains("##**##"))//Condition For RM#4340 
                            {
                                isPolice = Convert.ToBoolean(xmlElement.Attributes["IsPolice"].InnerText.Split("##**##".ToCharArray())[6]);
                            }
                            else
                            {
                                isPolice = Convert.ToBoolean(xmlElement.Attributes["IsPolice"].InnerText);
                            }

                            if (xmlElement.ChildNodes.Item(0).Name == "ContactName")
                            {
                                contactName = xmlElement.ChildNodes.Item(0).InnerText;
                            }
                            if (xmlElement.ChildNodes.Item(1).Name == "OrganisationName")
                            {
                                orgname = xmlElement.ChildNodes.Item(1).InnerText;
                            }
                            if (xmlElement.ChildNodes.Item(2) != null)
                            {
                                if (xmlElement.ChildNodes.Item(2).Name == "Fax")
                                {
                                    fax = xmlElement.ChildNodes.Item(2).InnerText;
                                }
                                else if (xmlElement.ChildNodes.Item(2).Name == "Email") // Checking for email in 2nd position if fax is not found 
                                {
                                    email = xmlElement.ChildNodes.Item(2).InnerText;
                                }
                            }
                            if (xmlElement.ChildNodes.Item(3) != null)
                            {
                                if (xmlElement.ChildNodes.Item(3).Name == "Email")
                                {
                                    email = xmlElement.ChildNodes.Item(3).InnerText;
                                }
                                else if (xmlElement.ChildNodes.Item(3).Name == "Fax") // Checking for fax in 3rd position if email is not found 
                                {
                                    fax = xmlElement.ChildNodes.Item(3).InnerText;
                                }
                            }
                            if ((Contact != null) && Contact.HasAttribute("Reason"))
                            {
                                reason = Convert.ToString(Contact.Attributes["Reason"].InnerText);
                            }
                            contactInfo = new ContactModel()
                            {
                                ContactId = contactId,
                                FullName = contactName,
                                Email = email, // fax,
                                Fax = fax, // email,
                                Reason = reason,
                                Organisation = orgname,
                                ISPolice = isPolice // For RM#4340
                            };
                            contactList.Add(contactInfo);
                        }
                    }
                }
            }
            return contactList;
        }
        #endregion

        #region GetOutboundDocMetaDataDetails
        public static OutboundDocuments GetOutboundDocMetaDataDetails(string esDAlRefNo, string userSchema)
        {
            OutboundDocuments outbounddocs = new OutboundDocuments();
            outbounddocs.ContactID = 0;
            outbounddocs.OrganisationID = 0;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                outbounddocs,
                userSchema + ".GET_OUTDOC_METADATA_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("p_ESDAL_REF_NUMBER", esDAlRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 (records, instance) =>
                 {
                     instance.OrganisationID = records.GetLongOrDefault("organisation_id");
                 }
                );

            return outbounddocs;
        }
        #endregion

        #region RetransmissionDocument
        public static string RetransmissionDocument(string xmlInformation, long contactId, string xsltPath, string docType = "EMAIL", UserInfo userInfo = null)
        {
            try
            {
                int VehicleUnits = 0;

                xmlInformation = xmlInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\"><Underscore xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##bus##");
                xmlInformation = xmlInformation.Replace("</Underscore></Bold>", "##bue##");

                xmlInformation = xmlInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "#bst#");
                xmlInformation = xmlInformation.Replace("</Bold>", "#be#");

                xmlInformation = xmlInformation.Replace("<Underscore xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##us##");
                xmlInformation = xmlInformation.Replace("<Underscore>", "##us##"); // RM#5232 changes
                xmlInformation = xmlInformation.Replace("</Underscore>", "##ue##");
                xmlInformation = xmlInformation.Replace("<Italic xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##is##");
                xmlInformation = xmlInformation.Replace("<Italic>", "##is##"); // RM#5232 changes
                xmlInformation = xmlInformation.Replace("</Italic>", "##ie##");

                xmlInformation = xmlInformation.Replace("<NotesForHaulier xmlns=\"http://www.esdal.com/schemas/core/movement\">##bs####us##", "<NotesForHaulier xmlns=\"http://www.esdal.com/schemas/core/movement\">##bssbr####us##");

                xmlInformation = xmlInformation.Replace("<OnBehalfOf xmlns=\"http://www.esdal.com/schemas/core/movement\"> </OnBehalfOf>", "");
                xmlInformation = xmlInformation.Replace("<OnBehalfOf> </OnBehalfOf>", "");

                xmlInformation = xmlInformation.Replace("<Para xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##ps##");
                xmlInformation = xmlInformation.Replace("</Para>", "##pe##");

                xmlInformation = xmlInformation.Replace("<BulletedText xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##bts##");
                xmlInformation = xmlInformation.Replace("</BulletedText>", "##bte##");

                //Start For Bug #4319 </Br> tag
                xmlInformation = xmlInformation.Replace("<Br />", "##br##").Replace("<Br></Br>", "##br##");
                //End For Bug #4319 </Br> tag

                StringReader stringReader = new StringReader(xmlInformation);
                XmlReader xmlReader = XmlReader.Create(stringReader);

                XslCompiledTransform xslt = new XslCompiledTransform();
                xslt.Load(xsltPath);

                StringWriter sw = new StringWriter();
                XmlTextWriter writer = new XmlTextWriter(sw);

                XsltArgumentList argsList = new XsltArgumentList();

                VehicleUnits = GetVehicleUnits((int)contactId, 0);

                argsList.AddParam("Contact_ID", "", contactId);
                argsList.AddParam("DocType", "", docType);

                if (VehicleUnits != 0)
                {
                    argsList.AddParam("UnitType", "", VehicleUnits);
                }

                xslt.Transform(xmlReader, argsList, writer, null);

                writer.Close();

                string outputString = Convert.ToString(sw);

                string Attr = "id=\"hdr_img\"";
                string ImgFilePath = "";
                //if (userInfo != null)
                //{
                //    if (userInfo.UserTypeId == 696008 || userInfo.UserTypeId == 696001)
                //    {
                //        ImgFilePath = Attr + " src=\"" + ConfigurationSettings.AppSettings["HeBrandingDocumentImagePath"].ToString() + "\"";
                //    }
                //    else
                //    {
                //        ImgFilePath = Attr + " src=\"" + ConfigurationSettings.AppSettings["DocumentImagePath"].ToString() + "\"";
                //    }
                //}
                //else
                //{
                //    ImgFilePath = Attr + " src=\"" + ConfigurationSettings.AppSettings["DocumentImagePath"].ToString() + "\"";
                //}
                ImgFilePath = Attr + " src=\"" + ConfigurationSettings.AppSettings["DocumentImagePath"].ToString() + "\"";

                outputString = outputString.Replace(Attr, ImgFilePath);

                outputString = outputString.Replace("##bus##", "<br/><br/><b><u>");
                outputString = outputString.Replace("##bue##", "</u></b> ");

                outputString = outputString.Replace("##bss##", "<b>");
                outputString = outputString.Replace("##bssbr##", "<b>");

                outputString = outputString.Replace("#bst#", "<b>");
                outputString = outputString.Replace("#be#", "</b>");

                outputString = outputString.Replace("##is##", "<i>");
                outputString = outputString.Replace("##ie##", "</i>");
                outputString = outputString.Replace("##us##", "<u>");
                outputString = outputString.Replace("##ue##", "</u> ");

                outputString = outputString.Replace("##ps##", "<p>");
                outputString = outputString.Replace("##pe##", "</p>");

                outputString = outputString.Replace("##bts##", "<ul><li>");
                outputString = outputString.Replace("##bte##", "</li></ul>");

                //Start For Bug #4319 </Br> tag
                outputString = outputString.Replace("##br##", "<Br />");
                //End For Bug #4319 </Br> tag

                outputString = outputString.Replace("FACSIMILE MESSAGE", "Mail");

                outputString = outputString.Replace("?", "");
                outputString = outputString.Replace("&#xA0;", "");
                outputString = outputString.Replace("&amp;nbsp;", " ");

                outputString = outputString.Replace("<b />", "");
                outputString = outputString.Replace("#b#", "<b>");

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("GetRetransmitDocument, xsltPath{0},VehicleUnits{1},ImgFilePath{2}", xsltPath, VehicleUnits, ImgFilePath));

                return outputString;
            }
            catch(Exception ex)
            {
                return null;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("GetRetransmitDocument, Exception: {0},xsltPath{1}", ex, xsltPath));
            }
        }
        #endregion

        #region GetNotifICAstatus
        public Dictionary<int, int> GetNotifICAstatus(string xmlaffectedStructures)
        {

            // object containing the list of affected structures 
            Domain.RouteAssessment.XmlAnalysedStructures.AnalysedStructures structureListObj = null;

            //Deserializing the xml into object and storing it 
            structureListObj = StringExtractor.XmlDeserializerStructures(xmlaffectedStructures);

            Dictionary<int, int> orgSuitablityDict = new Dictionary<int, int>();

            int orgId = 0;

            // parent node of Affected structures xml
            foreach (Domain.RouteAssessment.XmlAnalysedStructures.AnalysedStructuresPart structureListPart in structureListObj.AnalysedStructuresPart)
            {
                foreach (Domain.RouteAssessment.XmlAnalysedStructures.Structure structListObj in structureListPart.Structure)
                {
                    // ICA is performed only for underbriges so only that status need to be updated
                    foreach (Domain.RouteAssessment.XmlAnalysedStructures.Appraisal apprais in structListObj.Appraisal)
                    {
                        orgId = apprais.OrganisationId;

                        /* 277001 - unknown 
                           277002 - suitable
                           277003 - marginal
                           277004 - unsuitable
                           277005 - erroneous */

                        switch (apprais.AppraisalSuitability.Value)
                        {
                            case "ICA options are disabled for structure":

                                if (orgSuitablityDict.ContainsKey(orgId))
                                {
                                    if (orgSuitablityDict[orgId] > 277001)
                                    {
                                        //do nothing
                                    }
                                    else
                                    {
                                        //replace with the new value
                                        orgSuitablityDict[orgId] = 277001;
                                    }
                                }
                                else
                                {
                                    orgSuitablityDict.Add(orgId, 277001); // erroneous
                                }
                                break;

                            case "Suitable":
                            case "suitable":

                                if (orgSuitablityDict.ContainsKey(orgId))
                                {
                                    if (orgSuitablityDict[orgId] > 277002)
                                    {
                                        //do nothing
                                    }
                                    else
                                    {
                                        //replace with the new value
                                        orgSuitablityDict[orgId] = 277002;
                                    }
                                }
                                else
                                {
                                    orgSuitablityDict.Add(orgId, 277002);
                                }
                                break;

                            case "Marginally suitable":
                            case "marginal":

                                if (orgSuitablityDict.ContainsKey(orgId))
                                {
                                    if (orgSuitablityDict[orgId] > 277003)
                                    {
                                        //do nothing
                                    }
                                    else
                                    {
                                        //replace with the new value
                                        orgSuitablityDict[orgId] = 277003;
                                    }
                                }
                                else
                                {
                                    orgSuitablityDict.Add(orgId, 277003);
                                }
                                break;

                            case "Assessment not applicable":
                            case "Cannot be performed for side-by-side configuration":
                            case "Cannot be performed for 3 or more components":
                            case "Minimum axle spacing not found":
                            case "Vehicle not suitable for SV Train":
                            case "Vehicle does not belong to STGO category 1":
                            case "Axle weight capacity of structure is not available":
                            case "Cannot perform axle weight screening":
                            case "Structure width or length is not available":
                            case "Gross weight capacity of structure is not available":
                            case "Cannot perform gross weight screening":
                            case "Not SV Vehicle":

                                if (orgSuitablityDict.ContainsKey(orgId))
                                {
                                    if (orgSuitablityDict[orgId] < 277005 && orgSuitablityDict[orgId] != 277004)
                                    {
                                        //do nothing
                                    }
                                    else if (orgSuitablityDict[orgId] == 277004)
                                    {
                                        //replace with the new value
                                        orgSuitablityDict[orgId] = 277004;
                                    }
                                    else
                                    {
                                        orgSuitablityDict[orgId] = 277005;
                                    }
                                }
                                else
                                {
                                    orgSuitablityDict.Add(orgId, 277005);
                                }
                                break;

                            case "Unsuitable":
                            case "unsuitable":

                                if (orgSuitablityDict.ContainsKey(orgId))
                                {
                                    //replace with the new value
                                    orgSuitablityDict[orgId] = 277004;
                                }
                                else
                                {
                                    orgSuitablityDict.Add(orgId, 277004);
                                }
                                break;
                        }
                    }
                }
            }
            return orgSuitablityDict;
        }
        #endregion

        #region GetVehicleUnits
        //For RM#5475
        public static int GetVehicleUnits(int ContactId, Int32 OrgId)
        {
            UserInfo UserInfo = new UserInfo();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                UserInfo,
                 UserSchema.Portal + ".GET_VEHICLE_UNITS",
                parameter =>
                {
                    parameter.AddWithValue("P_CONTACT_ID", ContactId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGID", OrgId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.VehicleUnits = records.GetInt32OrDefault("vehicle_units");
                    }
            );

            return UserInfo.VehicleUnits;
        }
        #endregion

        #region AttachAffecetedStructure
        public static string AttachAffecetedStructure(string xmlString, string esDAlRefNo, UserInfo userInfo, string userSchema, int orgId)
        {
            GenerateDocument genDocument = new GenerateDocument();
            xmlString = xmlString.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\"><Underscore xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##bus##");
            xmlString = xmlString.Replace("</Underscore></Bold>", "##bue##");
            xmlString = xmlString.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "#bst#");
            xmlString = xmlString.Replace("</Bold>", "#be#");
            xmlString = xmlString.Replace("<Underscore xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##us##");
            xmlString = xmlString.Replace("</Underscore>", "##ue##");
            xmlString = xmlString.Replace("<Italic xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##is##");
            xmlString = xmlString.Replace("</Italic>", "##ie##");
            xmlString = xmlString.Replace("<NotesForHaulier xmlns=\"http://www.esdal.com/schemas/core/movement\">##bs####us##", "<NotesForHaulier xmlns=\"http://www.esdal.com/schemas/core/movement\">##bssbr####us##");
            xmlString = xmlString.Replace("<OnBehalfOf xmlns=\"http://www.esdal.com/schemas/core/movement\"> </OnBehalfOf>", "");
            xmlString = xmlString.Replace("<OnBehalfOf> </OnBehalfOf>", "");
            xmlString = xmlString.Replace("<Para xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##ps##");
            xmlString = xmlString.Replace("</Para>", "##pe##");
            xmlString = xmlString.Replace("<BulletedText xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##bts##");
            xmlString = xmlString.Replace("</BulletedText>", "##bte##");
            xmlString = xmlString.Replace("<Br />", "##br##").Replace("<Br></Br>", "##br##");
            if (xmlString.IndexOf("</Proposal>") != -1)
            {
                xmlString = genDocument.GetLoggedInUserAffectedStructureDetailsByESDALReference(xmlString, esDAlRefNo, userInfo, userSchema, "proposal", orgId);
            }

            else if (xmlString.IndexOf("</AgreedRoute>") != -1)
            {
                xmlString = genDocument.GetLoggedInUserAffectedStructureDetailsByESDALReference(xmlString, esDAlRefNo, userInfo, userSchema, "agreed", orgId);
            }
            return xmlString;
        }
        #endregion

        #region GetContactDetails
        public static ContactModel GetContactDetails(int contactId)
        {
            ContactModel contactDetail = new ContactModel();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    contactDetail,
                     UserSchema.Portal + ".GET_CONTACT_DETAILS",
                    parameter =>
                    {
                        parameter.AddWithValue("p_CONTACT_ID", contactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                       (records, instance) =>
                       {
                           instance.PhoneNumber = records.GetStringOrDefault("PHONENUMBER");
                           instance.Email = records.GetStringOrDefault("EMAIL");
                       }
                );
            return contactDetail;

        }
        #endregion

        #region GenerateHTMLPDF
        public string GenerateHTMLPDF(int notificationID, int docType, string xmlInformation, string fileName, string esDALRefNo, long organisationID, int contactID, string docfileName, bool isHaulier = false, string organisationName = "", string HAReference = "", int routePlanUnits = 692001, string documentType = "PDF", UserInfo userInfo = null, string userType = "")
        {

          fileName = System.AppDomain.CurrentDomain.BaseDirectory + fileName;
            int VehicleUnits = 0;
            ContactModel contactInfo;
            ContactModel contactDetails = new ContactModel();
            int struct_contactid;
            string xmlinfo = xmlInformation;
            if (xmlInformation != string.Empty)
            {
                XmlDocument Document = new XmlDocument();

                Document.LoadXml(xmlInformation);
                XmlNodeList parent = Document.GetElementsByTagName("Recipients");
                foreach (XmlNode children in parent)
                {
                    foreach (XmlNode childrenNode in children)
                    {
                        contactInfo = new ContactModel
                        {
                            Organisation = childrenNode.ChildNodes[1] == null ? string.Empty : childrenNode.ChildNodes[1].InnerText.Contains("##**##") ?
                                childrenNode.ChildNodes[1].InnerText.Substring(childrenNode.ChildNodes[1].InnerText.LastIndexOf('#') + 1) :
                               childrenNode.ChildNodes[1].InnerText,
                            ContactId = childrenNode.Attributes["ContactId"] == null ? 0 : childrenNode.Attributes["ContactId"].Value.Contains("##**##") ?
                                Convert.ToInt32(childrenNode.Attributes["ContactId"].Value.Substring(childrenNode.Attributes["ContactId"].Value.LastIndexOf('#') + 1)) :
                                Convert.ToInt32(childrenNode.Attributes["ContactId"].Value),
                        };
                        if (organisationName == contactInfo.Organisation)
                        {
                            struct_contactid = contactInfo.ContactId;
                            contactDetails = GetContactDetails(struct_contactid);
                            break;
                        }
                    }
                }
            }

            xmlInformation = xmlInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\"><Underscore xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##bus##");
            xmlInformation = xmlInformation.Replace("</Underscore></Bold>", "##bue##");

            xmlInformation = xmlInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "#bst#");
            xmlInformation = xmlInformation.Replace("</Bold>", "#be#");

            xmlInformation = xmlInformation.Replace("<Underscore xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##us##");
            xmlInformation = xmlInformation.Replace("<Underscore>", "##us##"); // RM#5232 changes
            xmlInformation = xmlInformation.Replace("</Underscore>", "##ue##");
            xmlInformation = xmlInformation.Replace("<Italic xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##is##");
            xmlInformation = xmlInformation.Replace("<Italic>", "##is##");// RM#5232 changes
            xmlInformation = xmlInformation.Replace("</Italic>", "##ie##");

            xmlInformation = xmlInformation.Replace("<NotesForHaulier xmlns=\"http://www.esdal.com/schemas/core/movement\">##bs####us##", "<NotesForHaulier xmlns=\"http://www.esdal.com/schemas/core/movement\">##bssbr####us##");

            xmlInformation = xmlInformation.Replace("<Underscore>", "");

            xmlInformation = xmlInformation.Replace("<OnBehalfOf xmlns=\"http://www.esdal.com/schemas/core/movement\"> </OnBehalfOf>", "");
            xmlInformation = xmlInformation.Replace("<OnBehalfOf> </OnBehalfOf>", "");

            xmlInformation = xmlInformation.Replace("<Para xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##ps##");
            xmlInformation = xmlInformation.Replace("</Para>", "##pe##");

            xmlInformation = xmlInformation.Replace("<BulletedText xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##bts##");
            xmlInformation = xmlInformation.Replace("</BulletedText>", "##bte##");

            //Start For Bug #4319 </Br> tag
            xmlInformation = xmlInformation.Replace("<Br />", "##br##").Replace("<Br></Br>", "##br##");
            //End For Bug #4319 </Br> tag

            xmlInformation = ConvertCountryLettersToSymbol(xmlInformation);

            StringReader stringReader = new StringReader(xmlInformation);
            XmlReader xmlReader = XmlReader.Create(stringReader);

            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(fileName);

            StringWriter sw = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(sw);

            XsltArgumentList argsList = new XsltArgumentList();

            VehicleUnits = GetVehicleUnits(contactID, (Int32)organisationID);

            argsList.AddParam("Contact_ID", "", contactID);

            if (VehicleUnits != 0)
            {
                argsList.AddParam("UnitType", "", VehicleUnits);
            }
            else
            {
                argsList.AddParam("UnitType", "", routePlanUnits);
            }

            argsList.AddParam("DocType", "", documentType);
            argsList.AddParam("OrganisationName", "", organisationName);
            argsList.AddParam("HAReferenceNumber", "", HAReference);
            argsList.AddParam("ContactPhoneNumber", "", contactDetails.PhoneNumber == null ? "" : contactDetails.PhoneNumber);
            argsList.AddParam("ContactEmail", "", contactDetails.Email == null ? "" : contactDetails.PhoneNumber);
            xslt.Transform(xmlReader, argsList, writer, null);

            writer.Close();

            string Attr = "id=\"hdr_img\"";
            string ImgFilePath = "";
            if (userInfo != null)
            {
                if (userInfo.UserTypeId == 696008 || userInfo.UserTypeId == 696001)
                {
                    ImgFilePath = Attr + " src=\"" + System.AppDomain.CurrentDomain.BaseDirectory + "Content\\Images\\ESDAL 2 Logo_org_He_Branding.png" + "\"";
                }
                else
                {
                    ImgFilePath = Attr + " src=\"" + System.AppDomain.CurrentDomain.BaseDirectory + "Content\\Images\\ESDAL 2 Logo_org.png" + "\"";
                }
            }
            else
            {
                ImgFilePath = Attr + " src=\"" + System.AppDomain.CurrentDomain.BaseDirectory + "Content\\Images\\ESDAL 2 Logo_org.png" + "\"";
            }

            string outputString = Convert.ToString(sw);

            outputString = outputString.Replace(Attr, ImgFilePath);

            outputString = outputString.Replace("##bus##", "<br/><br/><b><u>");
            outputString = outputString.Replace("##bue##", "</u></b> ");

            outputString = outputString.Replace("##bss##", "<b>");
            outputString = outputString.Replace("##bssbr##", "<b>");

            outputString = outputString.Replace("#bst#", "<b>");
            outputString = outputString.Replace("#be#", "</b>");

            outputString = outputString.Replace("##is##", "<i>");
            outputString = outputString.Replace("##ie##", "</i>");
            outputString = outputString.Replace("##us##", "<u>");
            outputString = outputString.Replace("##ue##", "</u> ");

            outputString = outputString.Replace("&amp;nbsp;", " ");

            outputString = outputString.Replace("##ps##", "<p>");
            outputString = outputString.Replace("##pe##", "</p>");

            outputString = outputString.Replace("##bts##", "<ul><li>");
            outputString = outputString.Replace("##bte##", "</li></ul>");

            //Start For Bug #4319 </Br> tag
            outputString = outputString.Replace("##br##", "<Br />");
            //End For Bug #4319 </Br> tag

            outputString = outputString.Replace("<b />", "");
            outputString = outputString.Replace("#b#", "<b>");

            outputString = outputString.Replace("?", " ");

            int n = GetNoofPages(outputString);

            outputString = outputString.Replace("###Noofpages###", n.ToString());

            //using (MemoryStream myMemoryStream = new MemoryStream())
            //{
            //    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 40f, 10f, 40f, 36);

            //    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

            //    StyleSheet style = new StyleSheet();

            //    htmlparser.SetStyleSheet(style);
            //    PdfWriter.GetInstance(pdfDoc, myMemoryStream);

            //    pdfDoc.Open();

            //    htmlparser.Parse(sr);

            //    HTMLWorker htmlparser1 = new HTMLWorker(pdfDoc);
            //    StyleSheet style1 = new StyleSheet();
            //    htmlparser1.SetStyleSheet(style1);

            //    pdfDoc.Close();

            //    content = AddPageNumbers(myMemoryStream.ToArray());
            //}

            xmlinfo = xmlinfo.Replace(">?<", ">\u2002<");
            xmlinfo = xmlinfo.Replace(">?##**##", ">##**##");

            byte[] XMLByteArrayData = Encoding.ASCII.GetBytes(xmlinfo);

            ////Some data is stored in gzip format, so we need to unzip then load it.
            byte[] CompressXMLString = STP.Common.General.XsltTransformer.CompressData(XMLByteArrayData);
            string userSchema = UserSchema.Portal;

            //if (!isHaulier)
            //{
            //    SaveDocument(notificationID, docType, organisationID, esDALRefNo, contactID, CompressXMLString, userSchema);
            //}

            return outputString;
        }
        #endregion

        #region GeneratePDFFromHtmlString
        public byte[] GeneratePDFFromHtmlString(string htmlString)
        {
            byte[] content = null;
            htmlString = htmlString.Replace("##bus##", "<br/><br/><b><u>");
            htmlString = htmlString.Replace("##bue##", "</u></b> ");

            htmlString = htmlString.Replace("##bss##", "<b>");
            htmlString = htmlString.Replace("##bssbr##", "<b>");

            htmlString = htmlString.Replace("#bst#", "<b>");
            htmlString = htmlString.Replace("#be#", "</b>");

            htmlString = htmlString.Replace("##is##", "<i>");
            htmlString = htmlString.Replace("##ie##", "</i>");
            htmlString = htmlString.Replace("##us##", "<u>");
            htmlString = htmlString.Replace("##ue##", "</u> ");

            htmlString = htmlString.Replace("&amp;nbsp;", " ");

            htmlString = htmlString.Replace("##ps##", "<p>");
            htmlString = htmlString.Replace("##pe##", "</p>");

            htmlString = htmlString.Replace("##bts##", "<ul><li>");
            htmlString = htmlString.Replace("##bte##", "</li></ul>");

            htmlString = htmlString.Replace("##br##", "<Br />");

            htmlString = htmlString.Replace("<b />", "");
            htmlString = htmlString.Replace("#b#", "<b>");

            htmlString = htmlString.Replace("?", " ");
            htmlString = htmlString.Replace("(on behalf of )", " ");

            int n = GetNoofPages(htmlString);

            StringReader sr = new StringReader(htmlString.Replace("###Noofpages###", n.ToString()));

            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 40f, 10f, 40f, 36);

                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

                StyleSheet style = new StyleSheet();
                style.LoadTagStyle("p", "lineheight", "2");
                htmlparser.SetStyleSheet(style);
                PdfWriter.GetInstance(pdfDoc, myMemoryStream);

                pdfDoc.Open();

                htmlparser.Parse(sr);

                HTMLWorker htmlparser1 = new HTMLWorker(pdfDoc);
                StyleSheet style1 = new StyleSheet();
                htmlparser1.SetStyleSheet(style1);

                pdfDoc.Close();

                content = AddPageNumbers(myMemoryStream.ToArray());
            }
            return content;
        }
        #endregion
    }
}