using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.DataAccess.SafeProcedure;
using System;

namespace STP.MovementsAndNotifications.Persistance
{
    public static class ManageImminentDAO
    {
        #region static int showImminentMovementDAO(int vehicleclass, string moveStartDate)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vehicleclass"></param>
        /// <param name="moveStartDate"></param>
        /// <returns></returns>
        public static int showImminentMovementDAO(string moveStartDate, string countryId, int countryIdCount, int vehicleClass)
        {
            int result = 0;
            DateTime now = DateTime.Now;
            string Current_date = now.ToString("dd/MM/yyyy");
            if (moveStartDate.Equals(Current_date))
            {
                result = 0;
            }
            else
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance
                (
                    result,
                   UserSchema.Portal + ".SP_SHOW_IMMINENTMOVEMENT",
                     parameter =>
                     {
                         parameter.AddWithValue("P_Current_date", Current_date, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_moveStartDate", moveStartDate, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_countryId", countryId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_countryIdCount", countryIdCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_VEHICLE_CLASS", vehicleClass, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                     },
                     records =>
                     {
                         result = (int)records.GetDecimalOrDefault("VAL_CNT");
                     }
                );
            }
            return result;
        }
        #endregion

        #region Removed Unwanted code by Mahzeer on 04-12-2023

        #region public static GetImminentChkDetailsDomain getDetailsToChkImminent(long notificationId, long revisionId, string UserSchema)
        //public static GetImminentChkDetailsDomain getDetailsToChkImminent(long notificationId, string contentReferenceNo, long revisionId, string userSchema)
        //{
        //    GetImminentChkDetailsDomain objImminent = new GetImminentChkDetailsDomain();
        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
        //    objImminent,
        //    userSchema + ".SP_DETAILS_TO_CHK_IMMINENT",
        //     parameter =>
        //     {
        //         parameter.AddWithValue("P_NOTIF_ID", notificationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
        //         parameter.AddWithValue("P_CONTENT_REF_NO", contentReferenceNo, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
        //         parameter.AddWithValue("P_Revision_id", revisionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
        //         parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //     },
        //     (records, instance) =>
        //     {
        //         instance.VehicleCode = (int)records.GetDecimalOrDefault("vehicle_classification");
        //         instance.MovementStartDate = Convert.ToString(records.GetDateTimeOrDefault("MOVE_START_DATE"));
        //         instance.VehicleLength = (decimal)records.GetDoubleOrDefault("LEN_MAX_MTR");
        //         instance.RigidLength = (decimal)records.GetDoubleOrDefault("RIGID_LEN_MAX_MTR");
        //         instance.VehicleWidth = (decimal)records.GetDoubleOrDefault("WIDTH_MAX_MTR");
        //         instance.GrossWeight = (decimal)records.GetDoubleOrDefault("GROSS_WEIGHT");
        //         instance.Width = (decimal)records.GetDoubleOrDefault("WIDTH");
        //         instance.Length = (decimal)records.GetDoubleOrDefault("LEN");
        //         instance.MaximamAxleWeight = (decimal)records.GetDoubleOrDefault("MAX_AXLE_WEIGHT");
        //         instance.AxleCount = (int)records.GetDecimalOrDefault("AXLE_COUNT");
        //         instance.LeftPRJ = records.GetDecimalOrDefault("LEFT_OVERHANG");
        //         instance.RightPRJ = records.GetDecimalOrDefault("RIGHT_OVERHANG");
        //         instance.FrontPRJ = records.GetDecimalOrDefault("FRONT_OVERHANG");
        //         instance.RearPRJ = records.GetDecimalOrDefault("REAR_OVERHANG");
        //     }
        //  );
        //    return objImminent;
        //}
        #endregion

        #region public static int CheckImminent(int vehicleclass, decimal vehiWidth, decimal vehiLength, decimal rigidLength, int WorkingDays)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vehicleclass"></param>
        /// <param name="vehiWidth"></param>
        /// <param name="vehiLength"></param>
        /// <param name="rigidLength"></param>
        /// <param name="WorkingDays"></param>
        /// <returns></returns>
        //public static int CheckImminent(int vehicleclass, decimal vehiWidth, decimal vehiLength, decimal rigidLength, decimal GrossWeight, int WorkingDays, decimal FrontPRJ, decimal RearPRJ, decimal LeftPRJ, decimal RightPRJ, GetImminentChkDetailsDomain objImminent, string Notif_type = null)
        //{

        //    //1 - Imminent movement.
        //    //2 - Imminent movement for police.
        //    //3 - Imminent movement for SOA.
        //    //4 - Imminent movement for SOA and police.
        //    //5 - No imminent movement.
        //    int Imminent;
        //    int axleCount = objImminent.AxleCount;
        //    decimal maxAxleWeight = objImminent.MaximamAxleWeight;
        //    if (Notif_type == "Simple")
        //    {
        //        objImminent = new GetImminentChkDetailsDomain();
        //        objImminent.VehicleWidth = vehiWidth;
        //        objImminent.VehicleLength = vehiLength;
        //        objImminent.RigidLength = rigidLength;
        //        objImminent.GrossWeight = GrossWeight;
        //        objImminent.FrontPRJ = FrontPRJ;
        //        objImminent.RearPRJ = RearPRJ;
        //        objImminent.LeftPRJ = LeftPRJ;
        //        objImminent.RightPRJ = RightPRJ;
        //        objImminent.MaximamAxleWeight = maxAxleWeight;
        //        objImminent.AxleCount = axleCount;
        //    }
        //    #region
        //    if (objImminent.GrossWeight <= 44000 && vehicleclass != 241006 && vehicleclass != 241007 && vehicleclass != 241008)//c&u
        //    {
        //        if (WorkingDays < 2)
        //        {
        //            if (objImminent.VehicleWidth <= (decimal)(2.9) && objImminent.VehicleLength <= (decimal)(25.9) && objImminent.RigidLength <= (decimal)(18.65))
        //            {
        //                if (objImminent.FrontPRJ <= (decimal)3.05 && objImminent.RearPRJ <= (decimal)3.05 && objImminent.LeftPRJ <= (decimal)0.305 && objImminent.RightPRJ <= (decimal)0.305)
        //                {
        //                    Imminent = 1;
        //                }
        //                else
        //                {
        //                    Imminent = 2;
        //                }
        //            }
        //            else if (objImminent.VehicleWidth <= (decimal)(5) && objImminent.RigidLength <= (decimal)(30))
        //            {
        //                Imminent = 2;
        //            }
        //            else if (objImminent.VehicleWidth <= (decimal)(6.1) && objImminent.RigidLength <= (decimal)(30))
        //            {
        //                Imminent = 2;
        //            }
        //            else
        //            {
        //                Imminent = 4;
        //            }
        //        }
        //        else if (WorkingDays < 5)
        //        {
        //            if (objImminent.VehicleWidth > (decimal)(6.1) || objImminent.RigidLength > (decimal)(30))
        //            {
        //                Imminent = 4;
        //            }
        //            else
        //            {
        //                Imminent = 5;
        //            }
        //        }
        //        else
        //        {
        //            Imminent = 5;
        //        }
        //    }
        //    else if (objImminent.GrossWeight <= 46000 && objImminent.AxleCount == 5)//cat1
        //    {
        //        if (WorkingDays < 2)
        //        {
        //            if (objImminent.VehicleWidth <= (3) && objImminent.VehicleLength <= (decimal)(25.9) && objImminent.RigidLength <= (decimal)(18.75))
        //            {
        //                if (objImminent.FrontPRJ <= (decimal)3.05 && objImminent.RearPRJ <= (decimal)3.05 && objImminent.LeftPRJ <= (decimal)0.305 && objImminent.RightPRJ <= (decimal)0.305)
        //                {
        //                    Imminent = 3;
        //                }
        //                else
        //                {
        //                    Imminent = 4;
        //                }
        //            }
        //            else
        //            {
        //                Imminent = 4;
        //            }
        //        }
        //        else if (WorkingDays < 5)
        //        {
        //            if (objImminent.VehicleWidth > (decimal)(6.1) || objImminent.RigidLength > (decimal)(30))
        //            {
        //                Imminent = 4;
        //            }
        //            else
        //            {
        //                Imminent = 5;
        //            }
        //        }
        //        else
        //        {
        //            Imminent = 5;
        //        }
        //    }
        //    else if (objImminent.GrossWeight <= 50000 && objImminent.AxleCount >= 6)//cat1
        //    {
        //        if (WorkingDays < 2)
        //        {
        //            if (objImminent.VehicleWidth <= (3) && objImminent.VehicleLength <= (decimal)(25.9) && objImminent.RigidLength <= (decimal)(18.75))
        //            {
        //                if (objImminent.FrontPRJ <= (decimal)3.05 && objImminent.RearPRJ <= (decimal)3.05 && objImminent.LeftPRJ <= (decimal)0.305 && objImminent.RightPRJ <= (decimal)0.305)
        //                {
        //                    Imminent = 3;
        //                }
        //                else
        //                {
        //                    Imminent = 4;
        //                }
        //            }
        //            else
        //            {
        //                Imminent = 4;
        //            }
        //        }
        //        else if (WorkingDays < 5)
        //        {
        //            if (objImminent.VehicleWidth > (decimal)(6.1) || objImminent.RigidLength > (decimal)(30))
        //            {
        //                Imminent = 4;
        //            }
        //            else
        //            {
        //                Imminent = 5;
        //            }
        //        }
        //        else
        //        {
        //            Imminent = 5;
        //        }
        //    }
        //    else if (objImminent.GrossWeight <= 80000 && objImminent.MaximamAxleWeight <= 16500)//cat2 and cat3
        //    {
        //        if (objImminent.MaximamAxleWeight <= 12500)//cat2
        //        {
        //            if (vehicleclass == 241007) //cat B
        //            {
        //                if (objImminent.VehicleWidth <= (3) && objImminent.VehicleLength <= (decimal)(25.9) && objImminent.RigidLength <= (decimal)(18.75))
        //                {
        //                    if (objImminent.FrontPRJ <= (decimal)3.05 && objImminent.RearPRJ <= (decimal)3.05 && objImminent.LeftPRJ <= (decimal)0.305 && objImminent.RightPRJ <= (decimal)0.305)
        //                    {
        //                        if (WorkingDays >= 2)
        //                        {
        //                            Imminent = 5;
        //                        }
        //                        else
        //                        {
        //                            Imminent = 3;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (WorkingDays >= 2)
        //                        {
        //                            Imminent = 5;
        //                        }
        //                        else
        //                        {
        //                            Imminent = 4;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (WorkingDays >= 2)
        //                    {
        //                        Imminent = 5;
        //                    }
        //                    else
        //                    {
        //                        Imminent = 4;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (WorkingDays < 2)
        //                {
        //                    if (objImminent.VehicleWidth <= (3) && objImminent.VehicleLength <= (decimal)(25.9) && objImminent.RigidLength <= (decimal)(18.75))
        //                    {
        //                        if (objImminent.FrontPRJ <= (decimal)3.05 && objImminent.RearPRJ <= (decimal)3.05 && objImminent.LeftPRJ <= (decimal)0.305 && objImminent.RightPRJ <= (decimal)0.305)
        //                        {
        //                            Imminent = 3;
        //                        }
        //                        else
        //                        {
        //                            Imminent = 4;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Imminent = 4;
        //                    }
        //                }
        //                else if (WorkingDays < 5)
        //                {
        //                    //commented this section to fix RM#19344 on 10 Nov 2020 by Poonam
        //                    //added to fix regression issue of stgo cat 3
        //                    //if (vehicleclass == 241005)
        //                    //{
        //                    //    if (WorkingDays < 2)
        //                    //    {
        //                    //        //msg = "Imminent movement for SOA and police.";
        //                    //        Imminent = 4;
        //                    //    }
        //                    //    else if (WorkingDays >= 2 && WorkingDays < 5)
        //                    //    {
        //                    //        //msg = "Imminent movement for SOA.";
        //                    //        Imminent = 3;
        //                    //    }
        //                    //    else
        //                    //    {
        //                    //        //msg = "No imminent movement";
        //                    //        Imminent = 5;
        //                    //    }
        //                    //}
        //                    //else
        //                    //{
        //                    if (objImminent.VehicleWidth > (decimal)(6.1) || objImminent.RigidLength > (decimal)(30))
        //                    {
        //                        Imminent = 4;
        //                    }
        //                    else
        //                    {
        //                        Imminent = 5;
        //                    }
        //                    //}
        //                }
        //                else
        //                {
        //                    Imminent = 5;
        //                }
        //            }
        //        }
        //        else//cat3
        //        {
        //            if (vehicleclass == 241008) //cat C
        //            {
        //                if (objImminent.VehicleWidth <= (3) && objImminent.VehicleLength <= (decimal)(25.9) && objImminent.RigidLength <= (decimal)(18.75))
        //                {
        //                    if (objImminent.FrontPRJ <= (decimal)3.05 && objImminent.RearPRJ <= (decimal)3.05 && objImminent.LeftPRJ <= (decimal)0.305 && objImminent.RightPRJ <= (decimal)0.305)
        //                    {
        //                        if (WorkingDays >= 2)
        //                        {
        //                            Imminent = 5;
        //                        }
        //                        else
        //                        {
        //                            Imminent = 3;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (WorkingDays >= 2)
        //                        {
        //                            Imminent = 5;
        //                        }
        //                        else
        //                        {
        //                            Imminent = 4;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (WorkingDays >= 2)
        //                    {
        //                        Imminent = 5;
        //                    }
        //                    else
        //                    {
        //                        Imminent = 4;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (WorkingDays < 5)
        //                {
        //                    if (objImminent.VehicleWidth <= (decimal)(3) && objImminent.VehicleLength <= (decimal)(25.9) && objImminent.RigidLength <= (decimal)(18.75))
        //                    {
        //                        if (objImminent.FrontPRJ <= (decimal)3.05 && objImminent.RearPRJ <= (decimal)3.05 && objImminent.LeftPRJ <= (decimal)0.305 && objImminent.RightPRJ <= (decimal)0.305)
        //                        {
        //                            if (WorkingDays < 5)
        //                            {
        //                                Imminent = 3;
        //                            }
        //                            else
        //                            {
        //                                Imminent = 5;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (WorkingDays >= 2 && WorkingDays < 5)
        //                            {
        //                                Imminent = 3;
        //                            }
        //                            else
        //                            {
        //                                Imminent = 4;
        //                            }
        //                        }
        //                    }
        //                    else if (objImminent.VehicleWidth <= (decimal)(5) && objImminent.RigidLength <= (decimal)(30))
        //                    {
        //                        if (WorkingDays >= 2 && WorkingDays < 5)
        //                        {
        //                            Imminent = 3;
        //                        }
        //                        else
        //                        {
        //                            Imminent = 4;
        //                        }
        //                    }
        //                    else if (objImminent.VehicleWidth <= (decimal)(6.1) && objImminent.RigidLength <= (decimal)(30))
        //                    {
        //                        if (WorkingDays >= 2 && WorkingDays < 5)
        //                        {
        //                            Imminent = 3;
        //                        }
        //                        else
        //                        {
        //                            Imminent = 4;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Imminent = 4;
        //                    }
        //                }
        //                else
        //                {
        //                    Imminent = 5;
        //                }
        //            }
        //        }
        //    }
        //    else if (objImminent.GrossWeight <= 150000 && objImminent.MaximamAxleWeight <= 16500)
        //    {
        //        if (WorkingDays < 5)
        //        {
        //            if (objImminent.GrossWeight > 80000 && (vehicleclass == 241007 || vehicleclass == 241008))
        //            {
        //                if (WorkingDays >= 2 && WorkingDays < 5)
        //                {
        //                    Imminent = 3;
        //                }
        //                else if (WorkingDays < 2)
        //                {
        //                    Imminent = 4;
        //                }
        //                else
        //                {
        //                    Imminent = 5;
        //                }
        //            }
        //            else if (objImminent.VehicleWidth <= (decimal)(3) && objImminent.VehicleLength <= (decimal)(25.9) && objImminent.RigidLength <= (decimal)(18.75))
        //            {
        //                //added to fix regression issue of stgo cat 3
        //                if (vehicleclass == 241005)
        //                {
        //                    if (WorkingDays < 2)
        //                    {
        //                        Imminent = 4;
        //                    }
        //                    else if (WorkingDays >= 2 && WorkingDays < 5)
        //                    {
        //                        Imminent = 3;
        //                    }
        //                    else
        //                    {
        //                        Imminent = 5;
        //                    }
        //                }
        //                else
        //                {

        //                    if (WorkingDays < 2)
        //                    {
        //                        Imminent = 4;
        //                    }
        //                    else
        //                    {
        //                        Imminent = 5;
        //                    }
        //                }
        //            }
        //            else if (objImminent.VehicleWidth <= (decimal)(5) && objImminent.RigidLength <= (decimal)(30))
        //            {
        //                if (WorkingDays >= 2 && WorkingDays < 5)
        //                {
        //                    Imminent = 3;
        //                }
        //                else
        //                {
        //                    Imminent = 4;
        //                }
        //            }
        //            else if (objImminent.VehicleWidth <= (decimal)(6.1) && objImminent.RigidLength <= (decimal)(30))
        //            {
        //                if (WorkingDays >= 2 && WorkingDays < 5)
        //                {
        //                    Imminent = 3;
        //                }
        //                else
        //                {
        //                    Imminent = 4;
        //                }
        //            }
        //            else
        //            {
        //                Imminent = 4;
        //            }
        //        }
        //        else
        //        {
        //            Imminent = 5;
        //        }
        //    }
        //    else //For SO
        //    {
        //        if (WorkingDays < 5)
        //        {
        //            Imminent = 4;
        //        }
        //        else
        //        {
        //            Imminent = 5;
        //        }
        //    }
        //    #endregion

        //    return Imminent;
        //}
        #endregion

        #endregion
    }
}