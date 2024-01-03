using Oracle.DataAccess.Client;
using STP.Common.Enums;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STP.DataAccess.SafeProcedure;
using NotificationXSD;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.IO.Compression;
using STP.Domain.Structures;
using STP.Structures.Persistance;
using STP.Domain.Structures.StructureJSON;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.Structures.VehicleJSON;
using STP.Domain.VehiclesAndFleets;
using System.Net.Http;
using STP.Common.Logger;
using STP.ServiceAccess.DocumentsAndContents;
using STP.Common.Constants;

namespace STP.Structures.Persistance
{
    public class StructureAssessmentDAO
    {
        private readonly IDocumentService documentService;
        public StructureAssessmentDAO(IDocumentService documentService)
        {
            this.documentService = documentService;
        }

        #region saving structure assessment input
        public static int PerformAssessment(List<StructuresToAssess> StuctureList, int NotificationID, string MovementRefNo, int AnalysisID, int RouteID)
        {
            int SequenceNo = 0;
            //int portal_schema =1;
            int portal_schema = 0;
            if (NotificationID != 0)
            {
                portal_schema = 1;
            }

            //get the sequence no route wise
            List<long> routeIdList = StuctureList.Select(x => x.RouteId).Distinct().ToList();
            // foreach (long routeid in routeIdList)
            //{
            List<StructuresToAssess> StuctureListRouteWise = (from a in StuctureList
                                                              where a.RouteId == RouteID
                                                              select a).ToList();

            EsdalStructureJson esdalJSON = new EsdalStructureJson();
            esdalJSON.EsdalStructure = getStructureList(StuctureListRouteWise);

            string structureJSON = Newtonsoft.Json.JsonConvert.SerializeObject(esdalJSON);

            byte[] structureByteArray = Zip(structureJSON);

            //vehicle byte array
            byte[] vehicleByteArray = PrepareVehicleJson(RouteID, NotificationID, MovementRefNo);

            try
            {
                //insert structure assessment details into database
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   SequenceNo,
                    UserSchema.Portal + ".STP_STRUCTURE_LIST.SP_PERFORM_STRUCTURE_ASSESSMENT",
                   parameter =>
                   {
                       parameter.AddWithValue("S_STRUCTURE_LIST", structureByteArray, OracleDbType.Blob, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("S_VEHICLE", vehicleByteArray, OracleDbType.Blob, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("S_ROUTE_PART_ID", RouteID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("S_MOVEMENT_REF_NO", MovementRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("S_ANALYSIS_ID", AnalysisID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("S_SCHEMA", portal_schema, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("S_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                      (records, instance) =>
                      {
                          SequenceNo = Convert.ToInt32(records.GetDecimalOrDefault("S_SEQUENCE_NO"));
                      }
               );
            }
            catch (Exception ex) { }

            return SequenceNo;
        }

        //fetch vehice details from the database and convert to json form
        public static byte[] PrepareVehicleJson(long RouteId, int NotificationID, string MovementRefNo)
        {
            string vehicleJson = null;
            byte[] vehicleByte = null;
            try
            {
                //get vehicle details
                NotifVehicleImport vehicleObj = GetVehicleDetails(RouteId, NotificationID, MovementRefNo);
                //get list of components
                List<VehicleConfigList> llistVehiComp = GetListOfVehicleComponents(vehicleObj);

                Vehicles vs = new Vehicles();
                ConfigurationSummaryListPosition cslp = new ConfigurationSummaryListPosition();
                cslp.ConfigurationSummary = vehicleObj.VehicleName;
                cslp.ConfigurationComponentsNo = llistVehiComp.Count();

                vs.ConfigurationSummaryListPosition = cslp;

                Configuration config = new Configuration();
                ComponentListPosition clp = new ComponentListPosition();
                List<Component> componentsList = new List<Component>();

                foreach (VehicleConfigList component in llistVehiComp)
                {
                    Component comp = new Component();
                    comp.ComponentType = component.ComponentType;
                    comp.ComponentSubType = component.ComponentSubType;
                    comp.Longitude = component.LongPosn;

                    AxleConfiguration axleConfig = new AxleConfiguration();
                    AxleSpacingListPosition aslp = new AxleSpacingListPosition();

                    List<VehComponentAxles> vehicleComponentAxlesList = new List<VehComponentAxles>();
                    vehicleComponentAxlesList = GetVehicleComponentAxlesList(vehicleObj.VehicleId, component.ComponentId);

                    axleConfig.NumberOfAxles = vehicleComponentAxlesList.Sum(x => x.AxleCount);
                    #region AxleSpacingToFollow
                    axleConfig.AxleSpacingToFollowing = component.SpaceToFollowing;
                    #endregion

                    #region AxleWeightListPosition
                    axleConfig.AxleWeightListPosition = GetAxleWeightListPosition(vehicleComponentAxlesList);
                    #endregion

                    #region AxleSpacingListPosition
                    axleConfig.AxleSpacingListPosition = GetAxleSpacingListPositionAxleSpacing(vehicleComponentAxlesList);
                    #endregion


                    comp.AxleConfiguration = axleConfig;
                    componentsList.Add(comp);
                }

                clp.Component = componentsList;
                config.ComponentListPosition = clp;

                vs.Configuration = config;
                vs.ConfigurationSummaryListPosition = cslp;

                EsdalVehiclesJSON vehicleJSON = new EsdalVehiclesJSON();
                vehicleJSON.Vehicles = vs;
                //convert vehicle object to json
                vehicleJson = Newtonsoft.Json.JsonConvert.SerializeObject(vehicleJSON);

                //getting the compressed blob value for saving
                vehicleByte = Zip(vehicleJson);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return vehicleByte;
        }

        private static void GetConfigurationType(NotifVehicleImport vehicleObj, VehicleSummaryStructure vssList)
        {
            if (vehicleObj.VehicleClass == 244001)
            {
                vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.drawbarvehicle;
            }
            if (vehicleObj.VehicleClass == 244002)
            {
                vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.semivehicle;
            }
            if (vehicleObj.VehicleClass == 244003)
            {
                vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.rigidvehicle;
            }
            if (vehicleObj.VehicleClass == 244004)
            {
                vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.trackedvehicle;
            }
            if (vehicleObj.VehicleClass == 244006)
            {
                vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.otherinline;
            }
            if (vehicleObj.VehicleClass == 244007)
            {
                vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.othersidebyside;
            }
            //Added - Jan272014 
            if (vehicleObj.VehicleClass == 244005)
            {
                vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.spmt;
            }
        }
        // this function has no references yet. so the service access calls not changed to the controller
        private SummaryAxleStructure GetAxleDetails(int NotificationID, NotifVehicleImport vehicleObj, List<VehicleConfigList> llistVehiComp)
        {
            SummaryAxleStructure sas = new SummaryAxleStructure();


            List<VehComponentAxles> vehicleComponentAxlesList = new List<VehComponentAxles>();
            if (NotificationID == 0)
            {
                vehicleComponentAxlesList = GetVehicleComponentAxlesList(vehicleObj.VehicleId, llistVehiComp[0].ComponentId);
            }
            else
            {
                vehicleComponentAxlesList = documentService.GetVehicleComponentAxles(NotificationID, vehicleObj.VehicleId);
            }
            sas.NumberOfAxles = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.AxleCount));

            sas.NumberOfWheel = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.WheelCount));

            #region AxleWeightListPosition
            sas.AxleWeightListPosition = documentService.GetAxleWeightListPositions(vehicleComponentAxlesList);
            #endregion

            #region AxleSpacingListPosition
            sas.AxleSpacingListPosition = documentService.GetAxleSpacingListPositionAxleSpacings(vehicleComponentAxlesList);
            #endregion

            #region AxleSpacingToFollow
            sas.AxleSpacingToFollowing = documentService.GetAxleSpacingToFollowListPositionAxleSpacings(vehicleComponentAxlesList, llistVehiComp[0].SpaceToFollowing);
            #endregion
            return sas;
        }

        private static List<VehicleConfigList> GetListOfVehicleComponents(NotifVehicleImport vehicleObj)
        {
            List<VehicleConfigList> llistVehiComp = new List<VehicleConfigList>();
            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                   llistVehiComp,
                    UserSchema.Portal + ".GET_VEHICLE_CONFIG_POSN",
                   parameter =>
                   {
                       parameter.AddWithValue("p_VHCL_ID", vehicleObj.VehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                   (records, result) =>
                   {
                       result.VehicleDescription = records.GetStringOrDefault("VEHICLE_DESC");
                       result.ComponentId = records.GetLongOrDefault("COMPONENT_ID");
                       result.LongPosn = records.GetShortOrDefault("LONG_POSN");
                       result.SpaceToFollowing = records["SPACE_TO_FOLLOWING"] == string.Empty ? 0 : records.GetDoubleOrDefault("SPACE_TO_FOLLOWING");
                       result.ComponentType = records.GetStringOrDefault("TYPE");
                       result.ComponentSubType = records.GetStringOrDefault("SUB_TYPE");

                   }
                   );
            }
            catch (Exception ex)
            {

            }
            return llistVehiComp;
        }

        private static NotifVehicleImport GetVehicleDetails(long RouteId, int NotificationID, string Movement_ref)
        {
            NotifVehicleImport vehicleObj = new NotifVehicleImport();
            if (NotificationID == 0)
            {
                //movement from sort
                //get hauier mnemonic and esdal ref
                string[] move_ref_arr = Movement_ref.Split('/');
                string mnemonic = move_ref_arr[0];
                string esdalrefnum = move_ref_arr[1];
                string ver_no = Convert.ToString(move_ref_arr[2].ToUpper().Replace("S", ""));

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   RouteId,
                    UserSchema.Portal + ".STP_STRUCTURE_LIST.SP_GET_SORT_MOVE_ROUTE_ID",
                   parameter =>
                   {
                       parameter.AddWithValue("P_MNEMONIC", mnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_ESDAL_REF", Convert.ToInt32(esdalrefnum), OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_VER_NO", Convert.ToInt32(ver_no), OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                      (records, instance) =>
                      {
                          RouteId = records.GetLongOrDefault("ROUTE_PART_ID");
                      }
               );
            }

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               vehicleObj,
                UserSchema.Portal + ".STP_STRUCTURE_LIST.SP_ROUTE_GET_VEHICLE_CONFIG",
               parameter =>
               {
                   parameter.AddWithValue("p_RoutePart_ID", RouteId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                  (records, instance) =>
                  {
                      vehicleObj.VehicleName = records.GetStringOrDefault("VEHICLE_NAME");
                      vehicleObj.VehicleId = Convert.ToInt64(records.GetLongOrDefault("VEHICLE_ID"));
                      vehicleObj.VehicleClass = Convert.ToInt32(records.GetInt32OrDefault("VEHICLE_TYPE"));
                  }
           );
            return vehicleObj;
        }

        public static List<VehComponentAxles> GetVehicleComponentAxlesList(long VehicleID, long ComponentID)
        {
            try
            {
                List<VehComponentAxles> componentAxleList = new List<VehComponentAxles>();

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    componentAxleList,
                       UserSchema.Portal + ".STP_STRUCTURE_LIST.SP_GET_SORT_MOVE_AXLES",
                    parameter =>
                    {
                        parameter.AddWithValue("A_VEHICLE_ID", VehicleID, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                        //parameter.AddWithValue("A_COMPONENT_ID", ComponentID, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("A_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.AxleCount = records.GetInt16OrDefault("axle_count");
                            instance.ComponentId = records.GetLongOrDefault("component_id");
                            instance.NextAxleDistNoti = records.GetDecimalOrDefault("NEXT_AXLE_DIST");
                            instance.TyreSize = records.GetStringOrDefault("tyre_size");

                            instance.Weight = records.GetFloatOrDefault("weight");
                            instance.WheelCount = records.GetInt16OrDefault("wheel_count");
                            instance.AxleNumber = records.GetInt16OrDefault("AXLE_NO");
                            instance.WheelSpacingList = records.GetStringOrDefault("wheel_spacing_list");
                            instance.AxleSpacingToFollowing = Convert.ToDouble(records.GetDecimalOrDefault("AXLE_SPACE_TO_FOLLOW"));
                        }
                );
                return componentAxleList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Structure Assessment List
        public static List<EsdalStructure> getStructureList(List<StructuresToAssess> StructureList)
        {
            List<EsdalStructure> essList = new List<EsdalStructure>();
            try
            {
                for (int i = 0; i < StructureList.Count; i++)
                {
                    EsdalStructure ess = getStructuresDetails(StructureList[i].ESRN, StructureList[i].SectionId);
                    essList.Add(ess);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return essList;
        }

        public static EsdalStructure getStructuresDetails(string structureCode, long SectionId)
        {
            EsdalStructure es = new EsdalStructure();
            try
            {
                es.ESRN = structureCode;

                AffStructureGeneralDetails listStructure = GetStructureGeneralDetails(structureCode, SectionId);
                es.StructureKey = listStructure.StructureKey;
                es.StructureType = listStructure.StructureType;

                long StructureId = listStructure.StructureId;
                int Structure_Id = Convert.ToInt32(listStructure.StructureId);
                int Section_Id = Convert.ToInt32(SectionId);


                UnderbridgeSections ubss = new UnderbridgeSections();
                UnderbridgeSection ubs = new UnderbridgeSection();

                ImposedConstraints constraints = ViewImposedConstruction(Structure_Id, Section_Id);

                ImposedConstraints ICAdata = SVICAData(Structure_Id, Section_Id);

                ubs.SkewAngle = Convert.ToInt32(constraints.SkewAngle);

                #region Load Rating
                LoadRating loadRating = new LoadRating();
                loadRating.HbRatingWithLoad = constraints.HBWithLiveLoad;
                loadRating.HbRatingWithoutLoad = constraints.HBWithoutLiveLoad;

                #region SV Parameters
                int ratingcount = 0;
                if (ICAdata.EnableSV_100 == 1 || ICAdata.EnableSV_150 == 1 || ICAdata.EnableSV_80 == 1 || ICAdata.EnableSV_Train == 1)
                {
                    ratingcount = 1;
                }
                #region SV Rating
                if (ratingcount >= 0)
                {


                    #region Vehicle Type
                    List<SVDataList> svParameters = Get_SV_Data(StructureId, SectionId);

                    List<SVParameters> svps = new List<SVParameters>();

                    for (int i = 0; i < svParameters.Count; i++)
                    {
                        SVParameters SVp = new SVParameters();
                        if (ICAdata.EnableSV_80 == 1 && svParameters[i].VehicleType == 340002)
                        {
                            SVp.VehicleType = VehicleType.sv80;
                            SVp.SVReserveWithLoad = Convert.ToDecimal(svParameters[i].WithLoad);
                            SVp.SVReserveWithOutLoad = Convert.ToDecimal(svParameters[i].WithoutLoad);
                            svps.Add(SVp);
                        }
                        if (ICAdata.EnableSV_100 == 1 && svParameters[i].VehicleType == 340003)
                        {
                            SVp.VehicleType = VehicleType.sv100;
                            SVp.SVReserveWithLoad = Convert.ToDecimal(svParameters[i].WithLoad);
                            SVp.SVReserveWithOutLoad = Convert.ToDecimal(svParameters[i].WithoutLoad);
                            svps.Add(SVp);
                        }
                        if (ICAdata.EnableSV_150 == 1 && svParameters[i].VehicleType == 340004)
                        {
                            SVp.VehicleType = VehicleType.sv150;
                            SVp.SVReserveWithLoad = Convert.ToDecimal(svParameters[i].WithLoad);
                            SVp.SVReserveWithOutLoad = Convert.ToDecimal(svParameters[i].WithoutLoad);
                            svps.Add(SVp);
                        }
                        if (ICAdata.EnableSV_Train == 1 && svParameters[i].VehicleType == 340005)
                        {
                            SVp.VehicleType = VehicleType.svtrain;
                            SVp.SVReserveWithLoad = Convert.ToDecimal(svParameters[i].WithLoad);
                            SVp.SVReserveWithOutLoad = Convert.ToDecimal(svParameters[i].WithoutLoad);
                            svps.Add(SVp);
                        }
                        if (svParameters[i].VehicleType == 340001)
                        {
                            SVp.VehicleType = VehicleType.svnone;
                            SVp.SVReserveWithLoad = Convert.ToDecimal(svParameters[i].WithLoad);
                            SVp.SVReserveWithOutLoad = Convert.ToDecimal(svParameters[i].WithoutLoad);
                            svps.Add(SVp);
                        }
                        if (svParameters[i].VehicleType == 340006)
                        {
                            SVp.VehicleType = VehicleType.svtt;
                            SVp.SVReserveWithLoad = Convert.ToDecimal(svParameters[i].WithLoad);
                            SVp.SVReserveWithOutLoad = Convert.ToDecimal(svParameters[i].WithoutLoad);
                            svps.Add(SVp);
                        }
                        if (svParameters[i].VehicleType == 340007)
                        {
                            SVp.VehicleType = VehicleType.unknown;
                            SVp.SVReserveWithLoad = Convert.ToDecimal(svParameters[i].WithLoad);
                            SVp.SVReserveWithOutLoad = Convert.ToDecimal(svParameters[i].WithoutLoad);
                            svps.Add(SVp);
                        }

                    }
                    SVRatings rating = new SVRatings();
                    rating.SVParameters = svps;
                    #endregion
                    loadRating.SVRatings = rating;
                }
                #endregion

                #endregion

                ubs.LoadRating = loadRating;

                #region Signed Weight Constraints
                SignedWeightConstraints swConstraints = new SignedWeightConstraints();
                swConstraints.GrossWeight = Convert.ToDecimal(constraints.SignedGrossWeightObj);
                swConstraints.AxleWeight = Convert.ToDecimal(constraints.SignedAxleWeight);
                swConstraints.DoubleAxleWeight = Convert.ToDecimal(constraints.SignedDoubleAxleWeight);
                swConstraints.TripleAxleWeight = Convert.ToDecimal(constraints.SignedTripleAxleWeight);
                swConstraints.AxleGroupWeight = Convert.ToDecimal(constraints.SignedAxleGroupWeight);

                #endregion

                ubs.SignedWeightConstraints = swConstraints;

                #region Span Details
                List<SpanData> spanDetails = StructureDAO.ViewSpanData(Structure_Id, Section_Id);
                List<Span> spanList = new List<Span>();
                for (int i = 0; i < spanDetails.Count; i++)
                {
                    Span span = new Span();
                    span.SpanNumber = (long)spanDetails[i].SpanNo;
                    span.Length = Convert.ToDouble(spanDetails[i].Length);

                    SpanPosition spanPosition = new SpanPosition();
                    spanPosition.SequencePosition = (int)spanDetails[i].Position;
                    spanPosition.SequenceNumber = (int)spanDetails[i].Sequence;
                    span.SpanPosition = spanPosition;
                    span.StructureType = spanDetails[i].StructType1;
                    span.Construction = spanDetails[i].ConstructionType1;
                    spanList.Add(span);
                }
                #endregion

                ubs.Span = spanList;
                ubss.UnderbridgeSection = ubs;
                es.UnderbridgeSections = ubss;
            }
            catch (Exception ex)
            {
                throw;
            }
            return es;
        }
        #endregion

        #region GetStructureGeneralDetails
        public static AffStructureGeneralDetails GetStructureGeneralDetails(string StructureCode, long SectionId)
        {
            try
            {
                AffStructureGeneralDetails objstructures = new AffStructureGeneralDetails();
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               objstructures,
                  "STP_STRUCTURE_LIST.SP_GET_STRUCTURE_DETAILS",
               parameter =>
               {

                   parameter.AddWithValue("P_STRUCT_CODE", StructureCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_SECTION_ID", SectionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                   (records, instance) =>
                   {
                       instance.StructureType = records.GetStringOrDefault("STRUCT_TYPE");
                       instance.StructureKey = records.GetStringOrDefault("STRUCTURE_KEY");
                       instance.StructureId = records.GetLongOrDefault("STRUCTURE_ID");
                   }
              );
                return objstructures;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region public static SVICAData (int structureId,int sectionId)
        public static ImposedConstraints SVICAData(int structureId, int sectionId)
        {
            try
            {
                ImposedConstraints icaDetailsObj = new ImposedConstraints();
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               icaDetailsObj,
               UserSchema.Portal + ".SP_ICA_SV_DATA",
               parameter =>
               {

                   parameter.AddWithValue("p_STRUCT_ID", structureId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_SECTION_ID", sectionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                (records, instance) =>
                {

                    var type = records.GetFieldType("ENABLE_SV_80");
                    instance.EnableSV_80 = records.GetInt16OrDefault("ENABLE_SV_80");
                    instance.EnableSV_100 = records.GetInt16OrDefault("ENABLE_SV_100");
                    instance.EnableSV_150 = records.GetInt16OrDefault("ENABLE_SV_150");
                    instance.EnableSV_Train = records.GetInt16OrDefault("ENABLE_SV_TRAIN");
                }
                );
                return icaDetailsObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region public static ImposedConstraints viewimposedConstruction(int structureId,int sectionId)
        public static ImposedConstraints ViewImposedConstruction(int structureId, int sectionId)
        {
            try
            {
                ImposedConstraints constructionDetailsObj = new ImposedConstraints();

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               constructionDetailsObj,
                UserSchema.Portal + ".SP_STRUCT_IMPOSED_CONSTRAINTS",
               parameter =>
               {
                   parameter.AddWithValue("p_struct_id", structureId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_section_id", sectionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
               (records, instance) =>
               {
                   //Imposed.
                   instance.SignedGrossWeight = records.GetInt32Nullable("SIGN_GROSS_WEIGHT");
                   instance.SignedSingleAxleWeight = records.GetInt32Nullable("SIGN_SINGLE_AXLE_WEIGHT");
                   instance.SignedDoubleAxleWeight = records.GetInt32Nullable("SIGN_DOUBLE_AXLE_WEIGHT");
                   instance.SignedTripleAxleWeight = records.GetInt32Nullable("SIGN_TRIPLE_AXLE_WEIGHT");
                   instance.SignedAxleGroupWeight = records.GetInt32Nullable("SIGN_AXLE_GROUP_WEIGHT");
                   instance.SkewAngle = records.GetDoubleNullable("SKEW_ANGLE");
                   instance.HBWithLiveLoad = records.GetDoubleNullable("HB_RATING_WITH_LOAD");
                   instance.HBWithoutLiveLoad = records.GetDoubleNullable("HB_RATING_WITHOUT_LOAD");
                   instance.SVRating = records.GetInt32Nullable("SV_RATING");
               }
                 );
                return constructionDetailsObj;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region GetAxleWeightListPosition
        public static AxleWeightListPosition GetAxleWeightListPosition(List<VehComponentAxles> vehicleComponentAxlesList)
        {
            AxleWeightListPosition awlp = new AxleWeightListPosition();

            List<Axles> axleWeight = new List<Axles>();

            //List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

            //foreach (long component in componentList)
            //{
            //    List<APPL_VEHICLE_COMPONENT_AXLES> componentWiseAxleList = (from apca in vehicleComponentAxlesList
            //                                                                where apca.ComponentId == component
            //                                                                orderby apca.AxleNumber ascending
            //                                                                select apca).ToList();

            VehComponentAxles a = new VehComponentAxles();

            List<float> weightList = vehicleComponentAxlesList.Select(x => x.Weight).ToList();

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
                        Axles axles = new Axles();
                        axles.Value = oldDummyweight;
                        axles.AxleCount = oldcountaAxles;

                        axleWeight.Add(axles);

                        oldcountaAxles = 1;
                        oldDummyweight = weight;
                    }
                }

                if (localCount == count)
                {
                    Axles axles = new Axles();
                    axles.Value = oldDummyweight;
                    axles.AxleCount = oldcountaAxles;

                    axleWeight.Add(axles);
                }
            }

            // }

            //if (componentList.Count == 0)
            //{
            //    Axles axles = new Axles();
            //    axles.Value = 0;
            //    axles.AxleCount = 0;

            //    axleWeight.Add(axles);
            //}
            awlp.AxleWeight = axleWeight;
            return awlp;
        }
        #endregion

        #region GetAxleSpacingListPositionAxleSpacing
        public static AxleSpacingListPosition GetAxleSpacingListPositionAxleSpacing(List<VehComponentAxles> vehicleComponentAxlesList)
        {
            AxleSpacingListPosition aslp = new AxleSpacingListPosition();
            List<Axles> axleSpace = new List<Axles>();
            //List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

            //foreach (long component in componentList)
            //{
            //    List<APPL_VEHICLE_COMPONENT_AXLES> componentWiseAxleList = (from apca in vehicleComponentAxlesList
            //                                                                where apca.ComponentId == component
            //                                                                orderby apca.AxleNumber ascending
            //                                                                select apca).ToList();

            VehComponentAxles a = new VehComponentAxles();

            List<decimal> axleSpacingList = vehicleComponentAxlesList.Select(x => x.NextAxleDistNoti).ToList();

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
                            Axles axles = new Axles();
                            axles.Value = Convert.ToDouble(oldDummyweight);
                            axles.AxleCount = oldcountaAxles;

                            axleSpace.Add(axles);

                            oldcountaAxles = 1;
                            oldDummyweight = axleSpacing;
                        }
                    }
                }

                if (localCount == count)
                {
                    Axles axles = new Axles();
                    axles.Value = Convert.ToDouble(oldDummyweight);
                    axles.AxleCount = oldcountaAxles;

                    axleSpace.Add(axles);
                }
            }
            //}

            //if (componentList.Count == 0)
            //{
            //    Axles axles = new Axles();
            //    axles.Value = 0;
            //    axles.AxleCount = 0;

            //    axleSpace.Add(axles);
            //}
            aslp.AxleSpacing = axleSpace;
            return aslp;
        }
        #endregion

        #region GetStructureAssessmentCount
        public static EsdalStructureJson GetStructureAssessmentCount(string ESRN, long RoutePartId)
        {
            EsdalStructureJson structureJSONMain = new EsdalStructureJson();
            try
            {
                List<StructureList> structureList = new List<StructureList>();
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    structureList,
                     UserSchema.Portal + ".STP_STRUCTURE_LIST.SP_GET_STRUCTURE_ASSESSMENT_COUNT",
                    parameter =>
                    {
                        parameter.AddWithValue("P_ESRN", ESRN, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ROUTE_PART_ID", RoutePartId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, result) =>
                        {
                            result.StructureBlobs = records.GetByteArrayOrNull("STRUCTURES");
                            result.SequenceNumber = records.GetLongOrDefault("SEQUENCE_NO");
                            result.Status = records.GetStringOrDefault("STATUS");
                        }
                    );
                structureJSONMain.EsdalStructure = new List<EsdalStructure>();
                string structureJsonString = "";
                if (structureList.Count() > 0)
                {
                    EsdalStructureJson structureJSON = null;
                    foreach (var item in structureList)
                    {

                        structureJsonString = Unzip(item.StructureBlobs);
                        structureJSON = JsonConvert.DeserializeObject<EsdalStructureJson>(structureJsonString);
                        structureJSON.EsdalStructure.ForEach(s => s.SeqNumber = item.SequenceNumber);
                        structureJSON.EsdalStructure.ForEach(s => s.Status = item.Status);
                        structureJSONMain.EsdalStructure.AddRange(structureJSON.EsdalStructure);
                    }
                }
                structureJSONMain.EsdalStructure = structureJSONMain.EsdalStructure.GroupBy(x => x.ESRN).Select(x => x.First()).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return structureJSONMain;
        }
        #endregion

        #region Get_SV_Data
        public static List<SVDataList> Get_SV_Data(long StructID, long SectionID)
        {
            try
            {
                List<SVDataList> objsvdata = new List<SVDataList>();
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    objsvdata,
                    "STP_STRUCTURE_LIST.SP_GET_SV_DATA",
                    parameter =>
                    {
                        parameter.AddWithValue("P_STRUCT_ID", StructID, OracleDbType.Int64, ParameterDirectionWrap.Input);
                        parameter.AddWithValue("P_SECTION_ID", SectionID, OracleDbType.Int64, ParameterDirectionWrap.Input);
                        //parameter.AddWithValue("P_VEHICLE_TYPE", VehicleType, OracleDbType.Int32, ParameterDirectionWrap.Input);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.VehicleType = records.GetInt32OrDefault("VEHICLE_TYPE");
                            instance.WithLoad = records.GetDoubleNullable("SV_RES_WITH_LOAD");
                            instance.WithoutLoad = records.GetDoubleNullable("SV_RES_MINUS_LOAD");
                            instance.SVDerivation = records.GetInt32OrDefault("SV_DERIVATION");
                            instance.CalculatedFactor = records.GetSingleNullable("CALC_HB_TO_SV_FACTOR");
                            instance.ManualInputFactor = records.GetSingleNullable("MAN_HB_TO_SV_FACTOR");
                        }
                );
                return objsvdata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Compressing and decompressing of blob file using GZip

        public static byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            //byte[] structureByteArray = Encoding.ASCII.GetBytes(structureJSON);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                    //CopyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }
        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }
        public static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                    //CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }
        #endregion

        public static int PrepareStructureAssessInput(bool p)
        {
            throw new NotImplementedException();
        }
    }
}
#endregion