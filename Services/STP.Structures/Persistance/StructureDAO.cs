using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using STP.Domain.Structures;

namespace STP.Structures.Persistance
{
    public static class StructureDAO
    {
        #region All Db Call should be called here from SP
        #region         public static StructureModel GetCautionDetails(long cautionID)
        /// Get Caution Details
        /// </summary>
        /// <param name="cautionID"></param>
        /// <returns></returns>
        public static StructureModel GetCautionDetails(long cautionID)
        {
            StructureModel CautionDetail = new StructureModel();
            long ContactID = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                CautionDetail,
                 UserSchema.Portal + ".GET_STRUCTURE_CAUTIONS_DETAIL",
                parameter =>
                {
                    parameter.AddWithValue("P_Caution_ID", cautionID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (DataAccess.Delegates.RecordMapper<StructureModel>)((records, instance) =>
                   {
                       instance.StructureId = records.GetLongOrDefault("STRUCTURE_ID");
                       instance.StructureCode = records.GetStringOrDefault("STRUCTURE_CODE");
                       instance.StructureName = records.GetStringOrDefault("STRUCTURE_NAME");
                       instance.OrganizationName = records.GetStringOrDefault("ORGNAME");
                       instance.OrganisationId = records.GetLongOrDefault("ORGANISATION_ID");
                       instance.CautionId = records.GetLongOrDefault("CAUTION_ID");
                       instance.CautionName = records.GetStringOrDefault("CAUTION_NAME");
                       instance.SpecificAction = records.GetStringOrDefault("SPECIFIC_ACTION");
                       instance.MaxHeightMetres = records.GetSingleOrDefault("MAX_HEIGHT_MTRS");
                       instance.MaxWidthMetres = records.GetSingleOrDefault("MAX_WIDTH_MTRS");
                       instance.MaxLengthMetres = records.GetSingleOrDefault("MAX_LENGTH_MTRS");
                       instance.MaxHeight = records.GetFloatOrDefault("MAX_HEIGHT");
                       instance.MaxWidth = records.GetFloatOrDefault("MAX_WIDTH");
                       instance.MaxLength = records.GetFloatOrDefault("MAX_LENGTH");
                       instance.MaxGrossWeightKgs = records.GetInt32OrDefault("MAX_GROSS_WEIGHT_KGS");
                       instance.MaxAxleWeightKgs = records.GetInt32OrDefault("MAX_AXLE_WEIGHT_KGS");
                       instance.MinSpeedKmph = records.GetSingleOrDefault("MIN_SPEED_KPH");
                       instance.MaxGrossWeight = records.GetDoubleOrDefault("MAX_GROSS_WEIGHT");
                       instance.MaxAxleWeight = records.GetDoubleOrDefault("MAX_AXLE_WEIGHT");
                       instance.MinSpeed = records.GetSingleOrDefault("MIN_SPEED");
                       instance.MaxLengthInMetres = records.GetSingleOrDefault("MAX_LENGTH_MTRS");
                       ContactID = records.GetLongOrDefault("contact_id");
                   })
            );
            if (ContactID != 0)
            {
                CautionDetail.OwnerIsContactFlag = true;
            }
            return CautionDetail;
        }
        #endregion
        #region public static List<StructureModel> GetCautionList(int pageNumber, int pageSize, long structureID, long SectionID)
        public static List<StructureModel> GetCautionList(int pageNumber, int pageSize, long structureID, long SectionID)
        {
            List<StructureModel> listStructure = new List<StructureModel>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    listStructure,
                     UserSchema.Portal + ".GET_STRUCTURE_CAUTIONS_LIST",
                    parameter =>
                    {
                        parameter.AddWithValue("P_pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_STRUCTURE_ID", structureID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_SECTION_ID", SectionID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (DataAccess.Delegates.RecordMapper<StructureModel>)((records, instance) =>
                        {
                            instance.CautionId = records.GetLongOrDefault("CAUTION_ID");
                            instance.CautionName = records.GetStringOrDefault("CAUTION_NAME");
                            instance.SpecificAction = records.GetStringOrDefault("SPECIFIC_ACTION");
                            instance.OrganizationName = records.GetStringOrDefault("ORGNAME");
                            instance.SpecificActionXML = records.GetStringOrDefault("SPECIFIC_ACTION");
                            instance.OrganisationId = records.GetLongOrDefault("ORGANISATION_ID");
                            instance.TotalRecordCount = records.GetDecimalOrDefault("TotalRecordCount");
                        })
                );
            return listStructure;
        }
        #endregion
        #region public static List<StructureSummary> GetStructureList(int orgId, int pageNum, int pageSize, string structId, string structName)
        /// <summary>
        /// Get Structure list
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public static List<StructureSummary> GetStructureList(int orgId, int pageNum, int pageSize, SearchStructures objSearchStruct)
        {
            List<StructureSummary> structureSummaryObj = new List<StructureSummary>();
              if (objSearchStruct.StructureType == "Underbridge")
                {
                    objSearchStruct.StructureType = "510001";
                }
                if (objSearchStruct.StructureType == "Overbridge")
                {
                    objSearchStruct.StructureType = "510002";
                }
                if (objSearchStruct.StructureType == "Under and Over Bridge")
                {
                    objSearchStruct.StructureType = "510003";
                }
                if (objSearchStruct.StructureType == "Level Crossing")
                {
                    objSearchStruct.StructureType = "510004";
                }
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    structureSummaryObj,
                     UserSchema.Portal + ".SP_R_STRUCT_SEARCH",
                    parameter =>
                    {
                        parameter.AddWithValue("p_ORG_ID", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("pageNumber", pageNum, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_structure_code", objSearchStruct.SearchSummaryId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_struct_name", objSearchStruct.SearchSummaryName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_alternate_name", objSearchStruct.AlternateName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_description", objSearchStruct.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_struct_type", objSearchStruct.StructureType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_object_carried", objSearchStruct.Carries, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_object_crossed", objSearchStruct.Crosses, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_NAME", objSearchStruct.DelegateName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ica_option", objSearchStruct.ICAMethod, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.StructureId = records.GetLongOrDefault("STRUCTURE_ID");
                            instance.StructCode = records.GetStringOrDefault("STRUCTURE_CODE");
                            instance.StructureName = records.GetStringOrDefault("STRUCTURE_NAME");
                            instance.TotalCount = (int)records.GetDecimalOrDefault("TOTAL_ROWS");
                        }
                );
          
            return structureSummaryObj;
        }
        #endregion
        #region public static StructureGeneralDetails GetStructurebyStructId(string structureId)
        /// <summary>
        /// Get Structure General Details  
        /// </summary>
        /// <param name="structureId"></param>
        /// <returns></returns>
        public static StructureGeneralDetails GetStructurebyStructId(string structureId)
        {
            StructureGeneralDetails structureGeneralDetailsObj = new StructureGeneralDetails();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                structureGeneralDetailsObj,
                 UserSchema.Portal + ".SELECT_GENERAL_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("p_STRUCTURE_ID", structureId, OracleDbType.NVarchar2, ParameterDirectionWrap.Input, 32767);
                },
                    (records, instance) =>
                    {
                        instance.ESRN = records.GetStringOrDefault("STRUCTURE_NAME_ID");
                        instance.StructureClass = records.GetStringOrDefault("STRUCTURE_CLASS");
                        instance.OSGREast = records.GetLongOrDefault("OSGR");
                        instance.StructureOwner = records.GetStringOrDefault("STRUCTURE_OWNER");
                        instance.StructureName = records.GetStringOrDefault("STRUCTURE_NAME");
                        instance.StructureAlternateNameOne = records.GetStringOrDefault("STRUCTURE_ALTERNATE_NAME_ONE");
                        instance.StructureAlternateNameTwo = records.GetStringOrDefault("STRUCTURE_ALTERNATE_NAME_TWO");
                        instance.StructureKey = records.GetStringOrDefault("STRUCTURE_KEY");
                        instance.Description = records.GetStringOrDefault("DESCRIPTION");
                        instance.Notes = records.GetStringOrDefault("NOTES");
                        instance.Category = records.GetStringOrDefault("STRUCTURE_CATEGORY");
                        instance.Type = records.GetStringOrDefault("STRUCTURE_Type");
                        instance.SubType1 = records.GetInt32OrDefault("STRUCTURE_Type_SubTypeOne");
                        instance.SubType2 = records.GetInt32OrDefault("STRUCTURE_Type_SubTypeTwo");
                        instance.Length = records.GetInt32OrDefault("STRUCTURE_Length");
                    }
            );
            return structureGeneralDetailsObj;
        }
        #endregion
        #region public static DimentionConstruction dimentionConstructionByID(string structureId)
        ///<summary>
        /// Get dimention Construction by structure id  
        ///</summary>
        /// <param name="structureId"></param>
        /// <returns></returns>
        public static DimensionConstruction dimentionConstructionByID(string structureId)
        {
            DimensionConstruction dimentionConstructionObj = new DimensionConstruction();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               dimentionConstructionObj,
                UserSchema.Portal + ".SELECT_DIMENTIOIN_CONSTRUCTION",
               parameter =>
               {
                   parameter.AddWithValue("p_STRUCTURE_ID", structureId, OracleDbType.NVarchar2, ParameterDirectionWrap.Input, 32767);
               },
                   (records, instance) =>
                   {
                       instance.Desc = records.GetStringOrDefault("DESC");
                       instance.ObjectCarried = records.GetStringOrDefault("OBJECT_CARRIED");
                       instance.ObjectCrossed = records.GetStringOrDefault("OBJECT_CROSSED");
                       instance.SkewAngle = records.GetDoubleOrDefault("SKEWANGLE");
                       instance.Length = records.GetLongOrDefault("LENGTH");
                       instance.MaxLength = records.GetLongOrDefault("MAXLENGTH");
                       instance.SpansCount = records.GetLongOrDefault("SPANCOUNT");
                       instance.DecksCount = records.GetLongOrDefault("DECKSCOUNT");
                       instance.ConstructionType1 = records.GetStringOrDefault("CONSTRUCTIONTYPEONE");
                       instance.ConstructionType2 = records.GetStringOrDefault("CONSTRUCTIONTYPETWO");
                       instance.ConstructionType3 = records.GetStringOrDefault("CONSTRUCTIONTYPETHREE");
                       instance.DeckMaterial1 = records.GetStringOrDefault("DECKMATERIALONE");
                       instance.DeckMaterial2 = records.GetStringOrDefault("DECKMATERIALTWO");
                       instance.DeckMaterial3 = records.GetStringOrDefault("DECKMATERIALTHREE");
                       instance.BearingsType1 = records.GetStringOrDefault("BEARINGSTYPEONE");
                       instance.BearingsType2 = records.GetStringOrDefault("BEARINGSTYPETWO");
                       instance.BearingsType3 = records.GetStringOrDefault("BEARINGSTYPETHREE");
                       instance.FoundationType1 = records.GetStringOrDefault("FOUNDATIONTYPEONE");
                       instance.FoundationType2 = records.GetStringOrDefault("FOUNDATIONTYPETWO");
                       instance.FoundationType3 = records.GetStringOrDefault("FOUNDATIONTYPETHREE");
                       instance.CarrigeWayWidth = records.GetLongOrDefault("CARRIGEWAYWIDTH");
                       instance.DeckWidth = records.GetDoubleOrDefault("DECKWIDTH");
                   }
           );
            return dimentionConstructionObj;
        }
        #endregion
        #region public static ManageStructureICA GetStructureICAUsage(int orgId, int structureId,int sectionId)
        public static ManageStructureICA GetStructureICAUsage(int organisationId, int structureId, int sectionId)
        {
            ManageStructureICA ManageStructureICAObj = new ManageStructureICA();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               ManageStructureICAObj,
                UserSchema.Portal + ".SP_ICA_MANAGE_USAGE",
               parameter =>
               {
                   parameter.AddWithValue("p_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_STRUCT_ID", structureId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_SECTION_ID", sectionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                   (records, instance) =>
                   {
                       instance.EnableGrossWeight = records.GetDecimalNullable("VAR_EN_WEIGHT");
                       instance.EnableAxleWeight = records.GetDecimalNullable("VAR_EN_AXLE_WEIGHT");
                       instance.EnableWeightOverDist = records.GetDecimalNullable("VAR_EN_WEIGHT_OVR_DIST");
                       instance.EnableAWR = records.GetDecimalNullable("VAR_EN_WEIGHT_AWR");
                       instance.EnableSV80 = records.GetDecimalNullable("VAR_EN_SV_80");
                       instance.EnableSV100 = records.GetDecimalNullable("VAR_EN_SV_100");
                       instance.EnableSV150 = records.GetDecimalNullable("VAR_EN_SV_150");
                       instance.EnableSVTrain = records.GetDecimalNullable("VAR_EN_SV_TRAIN");
                       instance.DefaultWSBandLimitMin = (double)records.GetDecimalOrDefault("WEIGHT_LOWER_DEF");
                       instance.DefaultWSBandLimitMax = (double)records.GetDecimalOrDefault("WEIGHT_UPPER_DEF");
                       instance.DefaultSVBandLimitMin = (double)records.GetDecimalOrDefault("SV_LOWER_DEF");
                       instance.DefaultSVBandLimitMax = (double)records.GetDecimalOrDefault("SV_UPPER_DEF");
                       instance.CustomWSBandLimitMin = (double?)records.GetDecimalNullable("WEIGHT_LOWER");
                       instance.CustomWSBandLimitMax = (double?)records.GetDecimalNullable("WEIGHT_UPPER");
                       instance.CustomSVBandLimitMin = (double?)records.GetDecimalNullable("SV_LOWER");
                       instance.CustomSVBandLimitMax = (double?)records.GetDecimalNullable("SV_UPPER");
                   }
           );
            return ManageStructureICAObj;
        }
        #endregion
        #region public static DimentionConstruction viewDimensionConstruction(int structureId,int sectionId)
        public static DimensionConstruction viewDimensionConstruction(int structureId, int sectionId)
        {
            DimensionConstruction dimensionDetailsObj = new DimensionConstruction();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
           dimensionDetailsObj,
            UserSchema.Portal + ".SP_DIM_AND_CONSTRAINTS",
           parameter =>
           {
               parameter.AddWithValue("p_struct_id", structureId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
               parameter.AddWithValue("p_section_id", sectionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
               parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
           },
           (records, instance) =>
           {
               instance.Desc = records.GetStringOrDefault("SECTION_DESCRIPTION");
               instance.BearingsType1 = records.GetStringOrDefault("BEARING1");
               instance.BearingsType2 = records.GetStringOrDefault("BEARING2");
               instance.BearingsType3 = records.GetStringOrDefault("BEARING3");
               instance.ConstructionType1 = records.GetStringOrDefault("CONSTRUCTION_TYPE1");
               instance.ConstructionType2 = records.GetStringOrDefault("CONSTRUCTION_TYPE2");
               instance.ConstructionType3 = records.GetStringOrDefault("CONSTRUCTION_TYPE3");
               instance.DeckMaterial1 = records.GetStringOrDefault("DECK_MATERIAL1");
               instance.DeckMaterial2 = records.GetStringOrDefault("DECK_MATERIAL2");
               instance.DeckMaterial3 = records.GetStringOrDefault("DECK_MATERIAL3");
               instance.FoundationType1 = records.GetStringOrDefault("FOUNDATION1");
               instance.FoundationType2 = records.GetStringOrDefault("FOUNDATION2");
               instance.FoundationType3 = records.GetStringOrDefault("FOUNDATION3");
               instance.ObjectCrossed = records.GetStringOrDefault("OBJECT_CROSSED");
               instance.ObjectCarried = records.GetStringOrDefault("OBJECT_CARRIED");
               instance.SpansCount = records.GetLongOrNullable("NO_OF_SPANS");
               instance.DecksCount = records.GetLongOrNullable("NO_OF_DECKS");
               instance.SkewAngle = records.GetDoubleNullable("SKEW_ANGLE");
               instance.CarrigeWayWidth = records.GetDoubleNullable("CWAY_WIDTH");
               instance.CarrigeWayId = records.GetLongOrDefault("CARR_NO");
               instance.DeckWidth = records.GetDoubleNullable("DECK_WIDTH");
               instance.DeckId = records.GetLongOrDefault("STDECK_ID");
               instance.MaxLength = records.GetDoubleNullable("MAX_SPAN_LEN");
               instance.Desc = records.GetStringOrDefault("SECTION_DESCRIPTION");
               instance.Length = records.GetDoubleNullable("SECTION_LEN");
               instance.Chainage = records.GetStringOrDefault("ELR_CHAINAGE");
               instance.TxtBearingsType1 = records.GetStringOrDefault("BEARING1");
               instance.TxtBearingsType3 = records.GetStringOrDefault("BEARING2");
               instance.TxtBearingsType2 = records.GetStringOrDefault("BEARING3");
               instance.TxtConstructionType1 = records.GetStringOrDefault("CONSTRUCTION_TYPE1");
               instance.TxtConstructionType2 = records.GetStringOrDefault("CONSTRUCTION_TYPE2");
               instance.TxtConstructionType3 = records.GetStringOrDefault("CONSTRUCTION_TYPE3");
               instance.TxtDeckMaterial1 = records.GetStringOrDefault("DECK_MATERIAL1");
               instance.TxtDeckMaterial2 = records.GetStringOrDefault("DECK_MATERIAL2");
               instance.TxtDeckMaterial3 = records.GetStringOrDefault("DECK_MATERIAL3");
               instance.TxtFoundationType1 = records.GetStringOrDefault("FOUNDATION1");
               instance.TxtFoundationType2 = records.GetStringOrDefault("FOUNDATION2");
               instance.TxtFoundationType3 = records.GetStringOrDefault("FOUNDATION3");
           }
             );
            return dimensionDetailsObj;
        }
        #endregion
        #region public static List<SVData> ViewSVData(int structureId,int sectionId)
        public static List<SpanData> ViewSpanData(int structureId, int sectionId)
        {
            List<SpanData> SpanDataDetailsObj = new List<SpanData>();
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               SpanDataDetailsObj,
                UserSchema.Portal + ".SP_STRUCT_SPAN",
               parameter =>
               {
                   parameter.AddWithValue("p_struct_id", structureId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_section_id", sectionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
               (records, instance) =>
               {
                   instance.SpanNo = records.GetLongOrDefault("SPAN_NO");
                   instance.Sequence = records.GetShortOrDefault("SEQ_NO");
                   instance.Position = records.GetShortOrDefault("SEQ_POS");
                   instance.Length = records.GetDoubleOrDefault("LEN");
                   instance.Description = records.GetStringOrDefault("DESCRIPTION");
                   instance.StructType1 = records.GetStringOrDefault("STRUCT_TYPE1");
                   instance.StructType2 = records.GetStringOrDefault("STRUCT_TYPE2");
                   instance.StructType3 = records.GetStringOrDefault("STRUCT_TYPE3");
                   instance.ConstructionType1 = records.GetStringOrDefault("CONSTRUCTION_TYPE1");
                   instance.ConstructionType2 = records.GetStringOrDefault("CONSTRUCTION_TYPE2");
                   instance.ConstructionType3 = records.GetStringOrDefault("CONSTRUCTION_TYPE3");
                   instance.DeckMaterial1 = records.GetStringOrDefault("DECK_MATERIAL1");
                   instance.DeckMaterial2 = records.GetStringOrDefault("DECK_MATERIAL2");
                   instance.DeckMaterial3 = records.GetStringOrDefault("DECK_MATERIAL3");
                   instance.SpanBearing1 = records.GetStringOrDefault("BEARING1");
                   instance.SpanBearing2 = records.GetStringOrDefault("BEARING2");
                   instance.SpanBearing3 = records.GetStringOrDefault("BEARING3");
                   instance.SpanFoundation1 = records.GetStringOrDefault("FOUNDATION1");
                   instance.SpanFoundation2 = records.GetStringOrDefault("FOUNDATION2");
                   instance.SpanFoundation3 = records.GetStringOrDefault("FOUNDATION3");
                   instance.TxtStructType1 = records.GetStringOrDefault("STRUCT_TYPE1");
                   instance.TxtStructType2 = records.GetStringOrDefault("STRUCT_TYPE2");
                   instance.TxtStructType3 = records.GetStringOrDefault("STRUCT_TYPE3");
                   instance.TxtConstructionType1 = records.GetStringOrDefault("CONSTRUCTION_TYPE1");
                   instance.TxtConstructionType2 = records.GetStringOrDefault("CONSTRUCTION_TYPE2");
                   instance.TxtConstructionType3 = records.GetStringOrDefault("CONSTRUCTION_TYPE3");
                   instance.TxtDeckMaterial1 = records.GetStringOrDefault("DECK_MATERIAL1");
                   instance.TxtDeckMaterial2 = records.GetStringOrDefault("DECK_MATERIAL2");
                   instance.TxtDeckMaterial3 = records.GetStringOrDefault("DECK_MATERIAL3");
                   instance.TxtBearingsType1 = records.GetStringOrDefault("BEARING1");
                   instance.TxtBearingsType2 = records.GetStringOrDefault("BEARING2");
                   instance.TxtBearingsType3 = records.GetStringOrDefault("BEARING3");
                   instance.TxtFoundationType1 = records.GetStringOrDefault("FOUNDATION1");
                   instance.TxtFoundationType2 = records.GetStringOrDefault("FOUNDATION2");
                   instance.TxtFoundationType3 = records.GetStringOrDefault("FOUNDATION3");
               }
                 );
            
            return SpanDataDetailsObj;
        }
        #endregion
        #region public static List<SVData> ViewSVData(int structureId,int sectionId)
        public static SpanData ViewSpanDataByNo(long structureId, long sectionId, long? spanNo)
        {
            SpanData SpanDataDetailsObj = new SpanData();
            spanNo = spanNo == 0 ? null : spanNo;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
           SpanDataDetailsObj,
            UserSchema.Portal + ".SP_GET_STRUCT_SPAN_BY_NO",
           parameter =>
           {
               parameter.AddWithValue("p_struct_id", structureId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
               parameter.AddWithValue("p_section_id", sectionId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
               parameter.AddWithValue("P_SPAN_NO", spanNo, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
               parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
           },
           (records, instance) =>
           {
               instance.SpanNo = records.GetLongOrNullable("SPAN_NO");
               instance.Sequence = records.GetShortOrDefault("SEQ_NO");
               instance.Position = records.GetShortOrDefault("SEQ_POS");
               instance.Length = records.GetDoubleOrDefault("LEN");
               instance.Description = records.GetStringOrDefault("DESCRIPTION");
               instance.StructType1 = records.GetStringOrDefault("STRUCT_TYPE1");
               instance.StructType2 = records.GetStringOrDefault("STRUCT_TYPE2");
               instance.StructType3 = records.GetStringOrDefault("STRUCT_TYPE3");
               instance.ConstructionType1 = records.GetStringOrDefault("CONSTRUCTION_TYPE1");
               instance.ConstructionType2 = records.GetStringOrDefault("CONSTRUCTION_TYPE2");
               instance.ConstructionType3 = records.GetStringOrDefault("CONSTRUCTION_TYPE3");
               instance.DeckMaterial1 = records.GetStringOrDefault("DECK_MATERIAL1");
               instance.DeckMaterial2 = records.GetStringOrDefault("DECK_MATERIAL2");
               instance.DeckMaterial3 = records.GetStringOrDefault("DECK_MATERIAL3");
               instance.SpanBearing1 = records.GetStringOrDefault("BEARING1");
               instance.SpanBearing2 = records.GetStringOrDefault("BEARING2");
               instance.SpanBearing3 = records.GetStringOrDefault("BEARING3");
               instance.SpanFoundation1 = records.GetStringOrDefault("FOUNDATION1");
               instance.SpanFoundation2 = records.GetStringOrDefault("FOUNDATION2");
               instance.SpanFoundation3 = records.GetStringOrDefault("FOUNDATION3");
               instance.TxtStructType1 = records.GetStringOrDefault("STRUCT_TYPE1");
               instance.TxtStructType2 = records.GetStringOrDefault("STRUCT_TYPE2");
               instance.TxtStructType3 = records.GetStringOrDefault("STRUCT_TYPE3");
               instance.TxtConstructionType1 = records.GetStringOrDefault("CONSTRUCTION_TYPE1");
               instance.TxtConstructionType2 = records.GetStringOrDefault("CONSTRUCTION_TYPE2");
               instance.TxtConstructionType3 = records.GetStringOrDefault("CONSTRUCTION_TYPE3");
               instance.TxtDeckMaterial1 = records.GetStringOrDefault("DECK_MATERIAL1");
               instance.TxtDeckMaterial2 = records.GetStringOrDefault("DECK_MATERIAL2");
               instance.TxtDeckMaterial3 = records.GetStringOrDefault("DECK_MATERIAL3");
               instance.TxtBearingsType1 = records.GetStringOrDefault("BEARING1");
               instance.TxtBearingsType2 = records.GetStringOrDefault("BEARING2");
               instance.TxtBearingsType3 = records.GetStringOrDefault("BEARING3");
               instance.TxtFoundationType1 = records.GetStringOrDefault("FOUNDATION1");
               instance.TxtFoundationType2 = records.GetStringOrDefault("FOUNDATION2");
               instance.TxtFoundationType3 = records.GetStringOrDefault("FOUNDATION3");
           }
             );
            return SpanDataDetailsObj;
        }
        #endregion
        #region public static ImposedConstraints viewimposedConstruction(int structureId,int sectionId)
        public static ImposedConstraints ViewImposedConstruction(int structureId, int sectionId)
        {
            ImposedConstraints constructionDetailsObj = new ImposedConstraints();
            constructionDetailsObj.SignedAxleWeight = new SignedAxleWeight();
            constructionDetailsObj.SignedHeight = new SignedHeight();
            constructionDetailsObj.SignedLength = new SignedLength();
            constructionDetailsObj.SignedWidth = new SignedWidth();
            constructionDetailsObj.SignedGrossWeightObj = new SignedGrossWeight();
            constructionDetailsObj.VerticalAlignment = new VerticalAlignment();
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
               instance.GrossWeight = records.GetInt32Nullable("GROSS_WEIGHT");
               instance.SignedAxleWeight.AxleWeight = records.GetInt32Nullable("SIGN_SINGLE_AXLE_WEIGHT");
               instance.AxleWeight = records.GetInt32Nullable("SINGLE_AXLE_WEIGHT");
               instance.HighwaySkew = records.GetDoubleNullable("HIGHWAY_SKEW");
               instance.SignedGrossWeightObj.GrossWeight = records.GetInt32Nullable("SIGN_GROSS_WEIGHT");
               instance.HBWithLiveLoad = records.GetDoubleNullable("HB_RATING_WITH_LOAD");
               instance.HBWithoutLiveLoad = records.GetDoubleNullable("HB_RATING_WITHOUT_LOAD");
               instance.SignedHeight.HeightMeter = records.GetDoubleNullable("SIGN_HEIGHT_METRES");
               instance.SignedHeight.HeightFeet = records.GetDoubleNullable("SIGN_HEIGHT_FEET");
               instance.SignedLength.LengthMeter = records.GetDoubleNullable("SIGN_LEN_METRES");
               instance.SignedLength.LengthFeet = records.GetDoubleNullable("SIGN_LEN_FEET");
               instance.SignedWidth.WidthMeter = records.GetDoubleNullable("SIGN_WIDTH_METRES");
               instance.SignedWidth.WidthFeet = records.GetDoubleNullable("SIGN_WIDTH_FEET");
               instance.SignedHeightStatus = records.GetInt32OrDefault("SIGN_HEIGHT_STATUS");
               instance.SignedWidthStatus = records.GetInt32OrDefault("SIGN_WIDTH_STATUS");
               instance.SignedLengthStatus = records.GetInt32OrDefault("SIGN_LEN_STATUS");
               instance.SignedGrossWeightStatus = records.GetInt32OrDefault("SIGN_GROSS_WEIGHT_STATUS");
               instance.SignedAxleWeightStatus = records.GetInt32OrDefault("SIGN_SINGLE_AXLE_WEIGHT_STATUS");
               instance.HALoading = records.GetInt16Nullable("HA_ASSESSED");
               instance.Height = records.GetDoubleNullable("MAX_HEIGHT_METRES");
               instance.Length = records.GetDoubleNullable("MAX_LEN_METRES");
               instance.Width = records.GetDoubleNullable("MAX_WIDTH_METRES");
               instance.VerticalAlignment.EntryDistance = records.GetInt32Nullable("HORIZONTAL_OFFSET_1");
               instance.VerticalAlignment.EntryHeight = records.GetInt32Nullable("VERTICAL_OFFSET_1");
               instance.VerticalAlignment.MaxDistance = records.GetDoubleNullable("HORIZONTAL_OFFSET_2");
               instance.VerticalAlignment.MaxHeight = records.GetDoubleNullable("VERTICAL_OFFSET_2");
               instance.VerticalAlignment.ExitDistance = records.GetDoubleNullable("HORIZONTAL_OFFSET_3");
               instance.VerticalAlignment.ExitHeight = records.GetDoubleNullable("VERTICAL_OFFSET_3");
               instance.MaxWeightOverMinDistanceWeight = records.GetStringOrDefault("MULTI_AXLE_GROUP_WEIGHTS");
               instance.MaxWeightOverMinDistanceDistance = records.GetStringOrDefault("MULTI_AXLE_GROUP_LENS");
           }
             );
            return constructionDetailsObj;
        }
        #endregion           
        #region public static List<StructType> getStructType(int structureId)
        public static List<StructType> getStructType(int type)
        {
            List<StructType> structTypeObj = new List<StructType>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               structTypeObj,
                UserSchema.Portal + ".SP_STRUCT_CATEGORY",
               parameter =>
               {
                   parameter.AddWithValue("P_TYPE", type, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                (records, instance) =>
                {
                    instance.StructureTypeId = records.GetInt32OrDefault("CODE");
                    instance.StrustureType = records.GetStringOrDefault("NAME");
                }
                );
            return structTypeObj;
        }
        #endregion
        #region public static List<StructCategory> getStructCategory(int structureId)
        public static List<StructCategory> getStructCategory(int type)
        {
            List<StructCategory> structCategoryObj = new List<StructCategory>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               structCategoryObj,
                UserSchema.Portal + ".SP_STRUCT_CATEGORY",
               parameter =>
               {
                   parameter.AddWithValue("P_TYPE", type, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                (records, instance) =>
                {
                    instance.StructureCaegorytId = records.GetInt32OrDefault("CODE");
                    instance.StructureCategory = records.GetStringOrDefault("NAME");
                }
                );
            return structCategoryObj;
        }
        #endregion
        #region public static int SaveDelegation(DelegationList delobj)
        public static long SaveDelegation(DelegationList delobj, int organisationId, int contactID)
        {
            long result = 0;
            
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               result,
                UserSchema.Portal + ".SP_CREATE_DELEGATION",
               parameter =>
               {
                   parameter.AddWithValue("P_ORG_FROM_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_ORG_TO_ID", delobj.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_CONTACT_TO", contactID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_NAME", delobj.ArrangementName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   if (delobj.CopyNotification)
                   {
                       parameter.AddWithValue("RET_NOTIF", 1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   }
                   else
                   {
                       parameter.AddWithValue("RET_NOTIF", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   }
                   if (delobj.SubDelegation)
                   {
                       parameter.AddWithValue("ALLOW_SUBDELEG", 1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   }
                   else
                   {
                       parameter.AddWithValue("ALLOW_SUBDELEG", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   }
                   if (delobj.NotifyIfFails)
                   {
                       parameter.AddWithValue("ACCEPT_FAIL", 1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   }
                   else
                   {
                       parameter.AddWithValue("ACCEPT_FAIL", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   }
                   parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                (records, instance) =>
                {
                    result = records.GetLongOrDefault("ARRANGEMENT_ID");
                }
                );
            
            return result;
        }
        #endregion
        #region ConfigureBandings
        public static bool ConfigureBandings(ManageStructureICA Banding, int orgId)
        {
            bool result = false;
             
                int count = 0;
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                      count,
                     UserSchema.Portal + ".SP_EDIT_DEFAULT_BANDING",
                    parameter =>
                    {
                        parameter.AddWithValue("orgId", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_weight_screening_lower", Banding.CustomWSBandLimitMin, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_weight_screening_upper", Banding.CustomWSBandLimitMax, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_sv_screening_lower", Banding.CustomSVBandLimitMin, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_sv_screening_upper", Banding.CustomSVBandLimitMax, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    record =>
                    {
                        count = Convert.ToInt32(record.GetDecimalOrDefault("COUNT"));
                    }
                    );
                if (count > 0)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
           
            return result;
        }
        #endregion
        #region public static bool UpdateManageICAUsage(ManageStructureICA ICAUsage)
        public static int UpdateManageICAUsage(ManageStructureICA ICAUsage, int orgId, int structureId, int sectionId)
        {
          
                int count = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                  UserSchema.Portal + ".SP_ICA_MANAGE_USAGE_EDIT",
                    parameter =>
                    {
                        parameter.AddWithValue("p_ORG_ID", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_STRUCT_ID", structureId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_SECTION_ID", sectionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("GROSS_WEIGHT", ICAUsage.EnableGrossWeight, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("AXLE_WEIGHT", ICAUsage.EnableAxleWeight, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("WEIGHT_OVER_DIST", ICAUsage.EnableWeightOverDist, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("AWR_SCREE", ICAUsage.EnableAWR, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("SV_80", ICAUsage.EnableSV80, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("SV_100", ICAUsage.EnableSV100, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("SV_150", ICAUsage.EnableSV150, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("SV_TRAIN", ICAUsage.EnableSVTrain, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("WEIGHT_SC_LOW", ICAUsage.CustomWSBandLimitMin, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("WEIGHT_SC_UP", ICAUsage.CustomWSBandLimitMax, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("SV_SC_UP", ICAUsage.CustomSVBandLimitMax, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("SV_SC_LOW", ICAUsage.CustomSVBandLimitMin, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_USER_NAME", ICAUsage.UserName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                    },
                    record =>
                    {
                        count = record.GetInt32("P_AFFECTED_ROWS");
                    }
                    );
                
          
            return count;
        }
        #endregion
        #region public static List<StructureSummary> GetStructionSectionbyStructId(int structureId)
        public static List<StructureSummary> GetStructionSectionbyStructId(int structureId)
        {
            List<StructureSummary> structureSummaryObj = new List<StructureSummary>();
             
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    structureSummaryObj,
                     UserSchema.Portal + ".SP_R_STRUCT_SEARCH",
                    parameter =>
                    {
                        parameter.AddWithValue("structureId", structureId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                (records, instance) =>
                {
                    instance.SectionId = records.GetInt32OrDefault("sectionId");
                    instance.StructureClass = records.GetStringOrDefault("structureClass");
                }
                );
            
            return structureSummaryObj;
        }
        #endregion
        #region public static int summaryListCount(int orgId,SearchStruct objSearchStruct)
        public static int summaryListCount(int orgId, SearchStructures objSearchStruct)
        {
            int count = 0;
           
                if (objSearchStruct.StructureType == "Underbridge")
                {
                    objSearchStruct.StructureType = "510001";
                }
                if (objSearchStruct.StructureType == "Overbridge")
                {
                    objSearchStruct.StructureType = "510002";
                }
                if (objSearchStruct.StructureType == "Under and Over Bridge")
                {
                    objSearchStruct.StructureType = "510003";
                }
                if (objSearchStruct.StructureType == "Level Crossing")
                {
                    objSearchStruct.StructureType = "510004";
                }
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    count,
                     UserSchema.Portal + ".SP_R_STRUCT_LIST_COUNT",
                    parameter =>
                    {
                        parameter.AddWithValue("p_ORG_ID", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_STRUCTURE_CODE", objSearchStruct.SearchSummaryId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_struct_name", objSearchStruct.SearchSummaryName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_alternate_name", objSearchStruct.AlternateName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_description", objSearchStruct.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_struct_type", objSearchStruct.StructureType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_object_carried", objSearchStruct.Carries, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_name", objSearchStruct.DelegateName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_object_crossed", objSearchStruct.Crosses, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ica_option", objSearchStruct.ICAMethod, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records) =>
                        {
                            count = (int)records.GetDecimalOrDefault("COUNT");
                        }
                );
            
            return count;
        }
        #endregion
        #region DelegArrangListCount
        public static int DelegArrangListCount(int OrgId, string arrangName)
        {
            int count = 0;
            
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    count,
                     UserSchema.Portal + ".SP_R_MY_DELEG_COUNT",
                    parameter =>
                    {
                        parameter.AddWithValue("p_ORG_ID", OrgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_NAME", arrangName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records) =>
                        {
                            count = (int)records.GetDecimalOrDefault("count");
                        }
                );
           
            return count;
        }
        #endregion
        #region GetStructureHistoryCnt
        public static int GetStructureHistoryCnt(long structureId)
        {
            int count = 0;
           
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    count,
                     UserSchema.Portal + ".SP_R_HISTORY_CNT",
                    parameter =>
                    {
                        parameter.AddWithValue("p_STRUCT_ID", structureId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records) =>
                        {
                            count = (int)records.GetDecimalOrDefault("count");
                        }
                );
          
            return count;
        }
        #endregion
        #region GetStructHistoryById
        public static List<StructureHistoryList> GetStructHistoryById(long structId, int pageNum, int pageSize)
        {
            List<StructureHistoryList> structhistObj = new List<StructureHistoryList>();
            
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    structhistObj,
                     UserSchema.Portal + ".SP_R_STRUCT_HISTORY",
                    parameter =>
                    {
                        parameter.AddWithValue("p_STRUCT_ID", structId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("pageNumber", pageNum, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.StructureDateTime = records.GetDateTimeOrDefault("OCCURRED");
                            instance.StructureUserName = records.GetStringOrDefault("AUTHOR");
                            instance.StructureDetails = records.GetStringOrDefault("AMENDMENT_1");
                        }
                );
            
            return structhistObj;
        }
        #endregion
        #region GetDefaultConfigData
        public static ConfigBandModel GetDefaultConfigData(int OrgId, long structId, long sectionId)
        {
            ConfigBandModel objConfigBandModel = new ConfigBandModel();
            
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    objConfigBandModel,
                    "SP_R_DEFAULT_BANDING",
                    parameter =>
                    {
                        parameter.AddWithValue("p_ORG_ID", OrgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_STRUCT_ID", structId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_SECTION_ID", sectionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.OrgMinWeight = records.GetSingleNullable("ica_weight_screening_lower");
                            if (instance.OrgMinWeight != null)
                            {
                                instance.OrgMinWeight = Math.Round(Convert.ToDouble(instance.OrgMinWeight), 2);
                            }
                            instance.OrgMaxWeight = (double?)records.GetSingleNullable("ica_weight_screening_upper");
                            if (instance.OrgMaxWeight != null)
                            {
                                instance.OrgMaxWeight = Math.Round(Convert.ToDouble(instance.OrgMaxWeight), 2);
                            }
                            instance.OrgMinSV = (double?)records.GetSingleNullable("ica_sv_screening_lower");
                            if (instance.OrgMinSV != null)
                            {
                                instance.OrgMinSV = Math.Round(Convert.ToDouble(instance.OrgMinSV), 2);
                            }
                            instance.OrgMaxSV = (double?)records.GetSingleNullable("ica_sv_screening_upper");
                            if (instance.OrgMaxSV != null)
                            {
                                instance.OrgMaxSV = Math.Round(Convert.ToDouble(instance.OrgMaxSV), 2);
                            }
                            instance.DefaultMinWeight = (double)records.GetSingleOrDefault("WEIGHT_SCREENING_LOWER");
                            instance.DefaultMinWeight = Math.Round(Convert.ToDouble(instance.DefaultMinWeight), 2);
                            instance.DefaultMaxWeight = (double)records.GetSingleOrDefault("WEIGHT_SCREENING_UPPER");
                            instance.DefaultMaxWeight = Math.Round(Convert.ToDouble(instance.DefaultMaxWeight), 2);
                            instance.DefaultMinSV = (double)records.GetSingleOrDefault("sv_screening_lower");
                            instance.DefaultMinSV = Math.Round(Convert.ToDouble(instance.DefaultMinSV), 2);
                            instance.DefaultMaxSV = (double)records.GetSingleOrDefault("sv_screening_upper");
                            instance.DefaultMaxSV = Math.Round(Convert.ToDouble(instance.DefaultMaxSV), 2);
                        }
            );
            
            return objConfigBandModel;
        }
        #endregion
        #region public static bool SaveDefaultConfig(ConfigBandModel objConfigModel)
        public static bool SaveDefaultConfig(int OrgId, double? OrgMinWeight, double? OrgMaxWeight, double? OrgMinSV, double? OrgMaxSV, string userName)
        {
            bool isSaveSuccess = true;
            
                SafeProcedure.DBProvider.Oracle.Execute(
                    "SP_EDIT_DEFAULT_BANDING",
                    parameter =>
                    {
                        parameter.AddWithValue("p_org_id", OrgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_weight_screening_lower", OrgMinWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_weight_screening_upper", OrgMaxWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_sv_screening_lower", OrgMinSV, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_SV_SCREENING_UPPER", OrgMaxSV, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_USER_NAME", userName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    }
                );
           
            return isSaveSuccess;
        }
        #endregion
        #region public static bool StructureGeneralDetails EditGeneralDetails(StructureGeneralDetails GeneralDetails)
        public static bool EditGeneralDetails(StructureGeneralDetails structureGeneralDetails)
        {
            bool result = false;
            int count = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                  count,
                UserSchema.Portal+".SP_EDIT_STRUCT_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_STRUCT_NAME", structureGeneralDetails.StructureName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STRUCT_CODE", structureGeneralDetails.ESRN, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ALT_NAME", structureGeneralDetails.StructureAlternateNameOne, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_DESC", structureGeneralDetails.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STRUCT_CLASS", structureGeneralDetails.StructureClass, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STRUCT_CAT", structureGeneralDetails.Category, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STRUCT_TYPE", structureGeneralDetails.Type, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STRUCT_ALT_TYPE1", structureGeneralDetails.Type1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STRUCT_ALT_TYPE2", structureGeneralDetails.Type2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STRUCTURE_LENGTH", structureGeneralDetails.Length, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NOTES", structureGeneralDetails.Notes, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STRUCT_KEY", structureGeneralDetails.StructureKey, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STRUCT_ID", structureGeneralDetails.StructureId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USER_NAME", structureGeneralDetails.UserName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", structureGeneralDetails.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    count = Convert.ToInt32(record.GetDecimalOrDefault("STRUCT_ROW_CNT"));
                }
            );
            if (count > 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        #endregion
        //**code for ICA calculation**
        #region public static bool HbToSvConversion(int structureId, int sectionId,string sectionClass,SvReserveFactors objConversion)
        // public static List<SvReserveFactors> HbToSvConversion(int structureId, int sectionId, SvReserveFactors reserveFactorObj)
        public static List<SvReserveFactors> HbToSvConversion(long structureId, long sectionId, double? HBRatingLiveLoad, double? HBRatingNoLoad, int saveFlag, string userName)
        {
            List<SvReserveFactors> ReserveFactorsDataObj = new List<SvReserveFactors>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                ReserveFactorsDataObj,
                    UserSchema.Portal+".STP_HB_SV_CONVERSION.SP_HB_SV_CONVERSION",
                    parameter =>
                    {
                        parameter.AddWithValue("P_STRUCTURE_ID", structureId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_SECTION_ID", sectionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_HB_WITH_LOAD", HBRatingLiveLoad, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_HB_WITHOUT_LOAD", HBRatingNoLoad, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_SAVE_FLAG", saveFlag, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_USER_NAME", userName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                     (records, instance) =>
                     {
                         instance.ParamNumber = records.GetLongOrDefault("PARAMETER_NO");
                         instance.VehicleType = records.GetInt32OrDefault("VEHICLE_TYPE");
                         instance.SVLiveLoad = records.GetDoubleNullable("SV_RES_WITH_LOAD");
                         instance.SVNoLoad = records.GetDoubleNullable("SV_RES_MINUS_LOAD");
                         instance.ManualInputFactor = records.GetSingleNullable("MAN_HB_TO_SV_FACTOR");
                         instance.CalculatedFactor = records.GetSingleNullable("CALC_HB_TO_SV_FACTOR");
                         instance.SVDerivation = records.GetInt32OrDefault("SV_DERIVATION");
                     }
                    );
            return ReserveFactorsDataObj;
        }
        #endregion
        #region public static list GetHBRatings(int structureId, int sectionId,)
        public static List<double?> GetHBRatings(long structureId, long sectionId)
        {
            List<double?> lstHBRatingsObj = new List<double?>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                       lstHBRatingsObj,
                           UserSchema.Portal+".STP_HB_SV_CONVERSION.SP_GET_HB_RATING",
                           parameter =>
                           {
                               parameter.AddWithValue("P_STRUCTURE_ID", structureId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                               parameter.AddWithValue("P_SECTION_ID", sectionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                               parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                           },
                            (records, instance) =>
                            {
                                lstHBRatingsObj.Add(records.GetDoubleNullable("HB_RATING_WITH_LOAD"));
                                lstHBRatingsObj.Add(records.GetDoubleNullable("HB_RATING_WITHOUT_LOAD"));
                            }
                           );
            return lstHBRatingsObj;
        }
        #endregion
        #region public static int SVScreeningResult(int structureId, int sectionId,int sectionClass, SvReserveFactors objReserveFactor)
        public static ICACalculation SVScreeningResult(int structureId, int sectionId, int sectionClass, SvReserveFactors objReserveFactor)
        {
            ICACalculation objICAResult = new ICACalculation();
             
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    objICAResult,
                    "SPC_TESTPROCEDURE",
                    parameter =>
                    {
                        parameter.AddWithValue("structId", structureId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("sectionId", sectionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("sectionClass", sectionClass, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (record) =>
                        {
                            objICAResult.Status = record.GetInt32OrDefault("status");
                        }
            );
            
            return objICAResult;
        }
        #endregion
        #region Private Methods
        #endregion
        #endregion
        #region  DeleteCaution
        /// <summary>
        /// Delete Caution
        /// </summary>
        /// <param name="CAUTION_ID">Caution id</param>
        /// <returns>ture/false</returns>
        public static int DeleteCaution(long cautionId, string userName)
        {
            int deleteFlag = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(

                UserSchema.Portal + ".DELETE_STRUCTURE_CAUTION",
                parameter =>
                {
                    parameter.AddWithValue("P_STRUCTURE_ID", null, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CautionID", cautionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_UserName", userName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                   (records) =>
                   {
                       deleteFlag = records.GetInt32("P_AFFECTED_ROWS");
                   }
                   );

            return deleteFlag;
        }
        #endregion
        /// <summary>
        /// Get Structure History details
        /// </summary>
        /// <param name="structureId">Get details based upon structure id provided</param>
        /// <returns></returns>
        public static List<StructureModel> GetStructureHistory(int pageNumber, int pageSize, long structureId)
        {
            List<StructureModel> listStructure = new List<StructureModel>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listStructure,
                UserSchema.Portal+".GET_STRUCTURE_HISTORYDETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_STRUCTURE_ID", structureId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.OccuredTime = records.GetDateTimeOrDefault("OCCURRED");
                        instance.TotalRecordCount = records.GetDecimalOrDefault("TOTALRECORDCOUNT");
                        if (records.GetStringOrDefault("AMENDMENT_1") != string.Empty)
                        {
                            string history = records.GetStringOrDefault("AMENDMENT_1");
                            history = history.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?><SpecificAction xmlns=\"http://www.esdal.com/schemas/core/caution\">", "");
                            history = history.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "");
                            history = history.Replace("<Italic xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "");
                            history = history.Replace("<Underline xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "");
                            history = history.Replace("</Underline>", "");
                            history = history.Replace("</Italic>", "");
                            history = history.Replace("</Bold>", "");
                            history = history.Replace("</SpecificAction>", "");
                            history = history.Replace("<b><i><u>", "");
                            history = history.Replace("</u></i></b>", "");
                            instance.HistoryDetails = history;
                        }
                    }
            );
            return listStructure;
        }
        /// <summary>
        /// Update structure log
        /// </summary>
        /// <param name="structureLogsModel">list of StructureLogModel</param>
        /// <returns>Update modification in Structure_log table</returns>
        public static bool UpdateStructureLog(List<StructureLogModel> structureLogsModel)
        {
            bool result = false;
            Decimal maxAuditID = GetMaxStructureAuditID();
            maxAuditID += (structureLogsModel.Count - 1);
            foreach (StructureLogModel structureLogModel in structureLogsModel)
            {
                result = false;
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    result,
                    UserSchema.Portal + ".MANAGE_STRUCTURE_LOGS",
                    parameter =>
                    {
                        parameter.AddWithValue("p_AUDIT_ID", maxAuditID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_AUTHOR", structureLogModel.Author, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_AMENDMENT_1", structureLogModel.Amendment1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_AMENDMENT_3", structureLogModel.Amendment3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_STRUCTURE_ID", structureLogModel.StructureId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_AMENDMENT_2", structureLogModel.Amendment2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    },
                        (records, instance) =>
                        {
                            result = true;
                        }
                );
                maxAuditID++;
            }
            return result;
        }
        public static Decimal GetMaxStructureAuditID()
        {
            Decimal maxAuditID = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                maxAuditID,
                UserSchema.Portal + ".GET_MAX_STRUC_AUDIT_ID",
                parameter =>
                {
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        maxAuditID = records.GetDecimalOrDefault("AUDIT_ID");
                    }
            );
            return maxAuditID;
        }
        #region  public static bool SaveCautions(StructureModel Structuremodelalter)
        public static bool SaveCautions(StructureModel Structuremodelalter)
        {
            bool result = false;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               UserSchema.Portal + ".MANAGE_STRUCTURE_CAUTION",
                parameter =>
                {
                    parameter.AddWithValue("P_CAUTION_ID", Structuremodelalter.CautionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CAUTION_NAME", Structuremodelalter.CautionName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STRUCTURE_ID", Structuremodelalter.StructureId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SECTION_ID", Structuremodelalter.SectionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STRUCTURE_CODE", Structuremodelalter.StructureCode, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", Structuremodelalter.OrganisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SPECIFIC_ACTION", Structuremodelalter.SpecificAction, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_GROSS_WEIGHT_KGS", Structuremodelalter.MaxGrossWeightKgs, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_MAX_GROSS_WEIGHT", Structuremodelalter.MaxGrossWeight, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_GROSS_WEIGHT_UNIT", Structuremodelalter.MaxGrossWeightUnit, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT_KGS", Structuremodelalter.MaxAxleWeightKgs, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT", Structuremodelalter.MaxAxleWeight, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT_UNIT", Structuremodelalter.MaxAxleWeightUnit, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_MAX_HEIGHT_MTRS", Structuremodelalter.MaxHeightMetres, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_HEIGHT", Structuremodelalter.MaxHeight, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_HEIGHT_UNIT", Structuremodelalter.MaxHeightUnit, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_MAX_LENGTH_MTRS", Structuremodelalter.MaxLengthMetres, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_LENGTH", Structuremodelalter.MaxLength, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_LENGTH_UNIT", Structuremodelalter.MaxLengthUnit, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_MAX_WIDTH_MTRS", Structuremodelalter.MaxWidthMetres, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_WIDTH", Structuremodelalter.MaxWidth, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_WIDTH_UNIT", Structuremodelalter.MaxWidthUnit, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_MIN_SPEED_KPH", Structuremodelalter.MinSpeedKmph, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MIN_SPEED", Structuremodelalter.MinSpeed, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MIN_SPEED_UNIT", Structuremodelalter.MinSpeedUnit, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_OWNER_IS_CONTACT", Structuremodelalter.OwnerIsContact, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MODE", Structuremodelalter.Mode, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        Structuremodelalter.CautionId = Convert.ToInt64(records.GetDecimalOrDefault("CAUTION_ID"));
                        result = true;
                    }
            );

            return result;
        }
        #endregion
        #region GetSVData
        public static List<SVDataList> GetSVData(long structureId, long sectionId)
        {
            List<SVDataList> objsvdata = new List<SVDataList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    objsvdata,
                    UserSchema.Portal + ".SP_SV_DATA",
                    parameter =>
                    {
                        parameter.AddWithValue("P_STRUCT_ID", structureId, OracleDbType.Int64, ParameterDirectionWrap.Input);
                        parameter.AddWithValue("P_SECTION_ID", sectionId, OracleDbType.Int32, ParameterDirectionWrap.Input);
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
        #endregion
        #region Update_SVData
        public static List<SVDataList> UpdateSVData(UpdateSVParams objUpdateSVParams)
        {
            List<SVDataList> obj = new List<SVDataList>();
            SafeProcedure.DBProvider.Oracle.Execute(
                    UserSchema.Portal + ".SP_EDIT_SV_DATA", parameter =>
                    {
                        parameter.AddWithValue("P_SV_RES_WITH_LOAD", objUpdateSVParams.WithLoad, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_SV_RES_MINUS_LOAD", objUpdateSVParams.WithoutLoad, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_SV_DERIVATION", objUpdateSVParams.SVDerivation, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_SECTION_ID", objUpdateSVParams.SectionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_STRUCT_ID", objUpdateSVParams.StructId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_vehicle_type", objUpdateSVParams.VehicleType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_USER_NAME", objUpdateSVParams.UserName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_MANUAL_FLAG", objUpdateSVParams.ManualFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_HB_WITH_LOAD", objUpdateSVParams.HbWithLoad, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_HB_MINUS_LOAD", objUpdateSVParams.HbWithoutLoad, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                    }
                    );
            return obj;
        }
        #endregion
        #region public static List<DropDown> GetDelegationArrangementList(int organisationId, string delegationArrangement)
        public static List<DropDown> GetDelegationArrangementList(int organisationId, string delegationArrangement)
        {
            List<DropDown> dropDownObj = new List<DropDown>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                dropDownObj,
                UserSchema.Portal + ".SP_INBOX_DELEG",
                parameter =>
                {
                    parameter.AddWithValue("ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("STR", delegationArrangement, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.Value = records.GetStringOrDefault("NAME");
                    }
            );
            return dropDownObj;
        }
        #endregion

        public static int SaveStructureSpan(SpanData objSpanData, long StructureID, long SectionID, int editSaveFlag)
        {
            int saveFlag = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                saveFlag,
                "SP_SAVE_STRUCTURE_SPAN", parameter =>
                {
                    parameter.AddWithValue("P_SPAN_NO", objSpanData.SpanNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SEQ_NO", objSpanData.Sequence, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SEQ_POS", objSpanData.Position, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_DESC", objSpanData.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_LEN", objSpanData.Length, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    if (objSpanData.StructType1 == "Other")
                        parameter.AddWithValue("P_STRUCT_TYPE1", objSpanData.TxtStructType1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    else
                        parameter.AddWithValue("P_STRUCT_TYPE1", objSpanData.StructType1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    if (objSpanData.StructType2 == "Other")
                        parameter.AddWithValue("P_STRUCT_TYPE2", objSpanData.TxtStructType2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    else
                        parameter.AddWithValue("P_STRUCT_TYPE2", objSpanData.StructType2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    if (objSpanData.StructType3 == "Other")
                        parameter.AddWithValue("P_STRUCT_TYPE3", objSpanData.TxtStructType3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    else
                        parameter.AddWithValue("P_STRUCT_TYPE3", objSpanData.StructType3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    if (objSpanData.ConstructionType1 == "Other")
                        parameter.AddWithValue("P_CONSTRUCTION_TYPE1", objSpanData.TxtConstructionType1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    else
                        parameter.AddWithValue("P_CONSTRUCTION_TYPE1", objSpanData.ConstructionType1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    if (objSpanData.ConstructionType2 == "Other")
                        parameter.AddWithValue("P_CONSTRUCTION_TYPE2", objSpanData.TxtConstructionType2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    else
                        parameter.AddWithValue("P_CONSTRUCTION_TYPE2", objSpanData.ConstructionType2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    if (objSpanData.ConstructionType3 == "Other")
                        parameter.AddWithValue("P_CONSTRUCTION_TYPE3", objSpanData.TxtConstructionType3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    else
                        parameter.AddWithValue("P_CONSTRUCTION_TYPE3", objSpanData.ConstructionType3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    if (objSpanData.DeckMaterial1 == "Other")
                        parameter.AddWithValue("P_DECK_MATERIAL1", objSpanData.TxtDeckMaterial1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    else
                        parameter.AddWithValue("P_DECK_MATERIAL1", objSpanData.DeckMaterial1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    if (objSpanData.DeckMaterial2 == "Other")
                        parameter.AddWithValue("P_DECK_MATERIAL2", objSpanData.TxtDeckMaterial2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    else
                        parameter.AddWithValue("P_DECK_MATERIAL2", objSpanData.DeckMaterial2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    if (objSpanData.DeckMaterial3 == "Other")
                        parameter.AddWithValue("P_DECK_MATERIAL3", objSpanData.TxtDeckMaterial3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    else
                        parameter.AddWithValue("P_DECK_MATERIAL3", objSpanData.DeckMaterial3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    if (objSpanData.SpanFoundation1 == "Other")
                        parameter.AddWithValue("P_FOUNDATION1", objSpanData.TxtFoundationType1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    else
                        parameter.AddWithValue("P_FOUNDATION1", objSpanData.SpanFoundation1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    if (objSpanData.SpanFoundation2 == "Other")
                        parameter.AddWithValue("P_FOUNDATION2", objSpanData.TxtFoundationType2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    else
                        parameter.AddWithValue("P_FOUNDATION2", objSpanData.SpanFoundation2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    if (objSpanData.SpanFoundation3 == "Other")
                        parameter.AddWithValue("P_FOUNDATION3", objSpanData.TxtFoundationType3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    else
                        parameter.AddWithValue("P_FOUNDATION3", objSpanData.SpanFoundation3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    if (objSpanData.SpanBearing1 == "Other")
                        parameter.AddWithValue("P_BEARING1", objSpanData.TxtBearingsType1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    else
                        parameter.AddWithValue("P_BEARING1", objSpanData.SpanBearing1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    if (objSpanData.SpanBearing2 == "Other")
                        parameter.AddWithValue("P_BEARING2", objSpanData.TxtBearingsType2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    else
                        parameter.AddWithValue("P_BEARING2", objSpanData.SpanBearing2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    if (objSpanData.SpanBearing3 == "Other")
                        parameter.AddWithValue("P_BEARING3", objSpanData.TxtBearingsType3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    else
                        parameter.AddWithValue("P_BEARING3", objSpanData.SpanBearing3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STRUCT_ID", StructureID, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SECTION_ID", SectionID, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USER_NAME", objSpanData.UserName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SAVE_EDIT_FLAG", editSaveFlag, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                },
                 record =>
                 {
                     saveFlag = (int)record.GetDecimalOrDefault("SAVE_FLAG");
                 }
                );
            return saveFlag;
        }
        #region DeleteStructureSpan
        public static int DeleteStructureSpan(long StructureID, long SectionID, long spanNo, string userName)
        {
            int deleteFlag = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
              "SP_DELETE_STRUCT_SPAN", parameter =>
              {
                  parameter.AddWithValue("P_STRUCT_ID", StructureID, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_SECTION_ID", SectionID, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_SPAN_NO", spanNo, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_USER_NAME", userName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
              },
               (records) =>
               {
                   deleteFlag = records.GetInt32("P_AFFECTED_ROWS");
               }
               );
            return deleteFlag;
        }
        #endregion
        #region SP_STRUCT_DD
        public static List<StucDDList> STRUCT_DD(int type)
        {
            List<StucDDList> obj = new List<StucDDList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    obj,
                    UserSchema.Portal + ".SP_STRUCT_DD",
                    parameter =>
                    {
                        parameter.AddWithValue("P_TYPE", type, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                    },
                (records, instance) =>
                {
                    instance.Code = records.GetInt32OrDefault("CODE");
                    instance.Name = records.GetStringOrDefault("NAME");
                }
                    );
            var other = new StucDDList { Code = 0, Name = "Other" };
            obj.Add(other);
            return obj;
        }

        //SP_EDIT_DIM_AND_CONSTRAINTS
        public static bool EditDimensionConstraints(DimensionConstruction objStructureDimension, int StructureID, int SectionID)
        {
            SafeProcedure.DBProvider.Oracle.Execute(
                    "SP_EDIT_DIM_AND_CONSTRAINTS", parameter =>
                    {
                        parameter.AddWithValue("P_DESC", objStructureDimension.Desc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_OBJ_CARRIED", objStructureDimension.ObjectCarried, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_OBJ_CROSSED", objStructureDimension.ObjectCrossed, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_SKEW_ANGLE", objStructureDimension.SkewAngle, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_SECTION_LEN", objStructureDimension.Length, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_MAX_SPAN_LEN", objStructureDimension.MaxLength, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_NO_OF_SPANS", objStructureDimension.SpansCount, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_NO_OF_DECKS", objStructureDimension.DecksCount, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ELR_CHAINAGE", objStructureDimension.Chainage, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                        if (objStructureDimension.ConstructionType1 == "Other")
                            parameter.AddWithValue("P_CONSTRUCTION_TYPE1", objStructureDimension.TxtConstructionType1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        else
                            parameter.AddWithValue("P_CONSTRUCTION_TYPE1", objStructureDimension.ConstructionType1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                        if (objStructureDimension.ConstructionType2 == "Other")
                            parameter.AddWithValue("P_CONSTRUCTION_TYPE2", objStructureDimension.TxtConstructionType2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        else
                            parameter.AddWithValue("P_CONSTRUCTION_TYPE2", objStructureDimension.ConstructionType2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                        if (objStructureDimension.ConstructionType3 == "Other")
                            parameter.AddWithValue("P_CONSTRUCTION_TYPE3", objStructureDimension.TxtConstructionType3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        else
                            parameter.AddWithValue("P_CONSTRUCTION_TYPE3", objStructureDimension.ConstructionType3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                        if (objStructureDimension.DeckMaterial1 == "Other")
                            parameter.AddWithValue("P_DECK_MATERIAL1", objStructureDimension.TxtDeckMaterial1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        else
                            parameter.AddWithValue("P_DECK_MATERIAL1", objStructureDimension.DeckMaterial1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        if (objStructureDimension.DeckMaterial2 == "Other")
                            parameter.AddWithValue("P_DECK_MATERIAL2", objStructureDimension.TxtDeckMaterial2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        else
                            parameter.AddWithValue("P_DECK_MATERIAL2", objStructureDimension.DeckMaterial2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        if (objStructureDimension.DeckMaterial3 == "Other")
                            parameter.AddWithValue("P_DECK_MATERIAL3", objStructureDimension.TxtDeckMaterial3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        else
                            parameter.AddWithValue("P_DECK_MATERIAL3", objStructureDimension.DeckMaterial3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);


                        if (objStructureDimension.FoundationType1 == "Other")
                            parameter.AddWithValue("P_FOUNDATION1", objStructureDimension.TxtFoundationType1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        else
                            parameter.AddWithValue("P_FOUNDATION1", objStructureDimension.FoundationType1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        if (objStructureDimension.FoundationType2 == "Other")
                            parameter.AddWithValue("P_FOUNDATION2", objStructureDimension.TxtFoundationType2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        else
                            parameter.AddWithValue("P_FOUNDATION2", objStructureDimension.FoundationType2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        if (objStructureDimension.FoundationType3 == "Other")
                            parameter.AddWithValue("P_FOUNDATION3", objStructureDimension.TxtFoundationType3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        else
                            parameter.AddWithValue("P_FOUNDATION3", objStructureDimension.FoundationType3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                        if (objStructureDimension.BearingsType1 == "Other")
                            parameter.AddWithValue("P_BEARING1", objStructureDimension.TxtBearingsType1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        else
                            parameter.AddWithValue("P_BEARING1", objStructureDimension.BearingsType1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        if (objStructureDimension.BearingsType2 == "Other")
                            parameter.AddWithValue("P_BEARING2", objStructureDimension.TxtBearingsType2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        else
                            parameter.AddWithValue("P_BEARING2", objStructureDimension.BearingsType2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        if (objStructureDimension.BearingsType3 == "Other")
                            parameter.AddWithValue("P_BEARING3", objStructureDimension.TxtBearingsType3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        else
                            parameter.AddWithValue("P_BEARING3", objStructureDimension.BearingsType3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                        parameter.AddWithValue("P_WIDTH", objStructureDimension.CarrigeWayWidth, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_CARRID", objStructureDimension.CarrigeWayId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_DWIDTH", objStructureDimension.DeckWidth, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_DECID", objStructureDimension.DeckId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                        parameter.AddWithValue("P_STRUCT_ID", StructureID, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_SECTION_ID", SectionID, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_USER_NAME", objStructureDimension.UserName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                    }
                    );
            return true;
        }
        public static bool EditStructureImposed(ImposedConstraints objStructureImpoConst, int structureId, int sectionId, int structType)
        {
            if (structType == 2)
            {
                objStructureImpoConst.SignedGrossWeightObj = new SignedGrossWeight();
                objStructureImpoConst.VerticalAlignment = new VerticalAlignment();
                objStructureImpoConst.SignedAxleWeight = new SignedAxleWeight();
            }
            if (structType == 1)
            {
                objStructureImpoConst.VerticalAlignment = new VerticalAlignment();
            }
            SafeProcedure.DBProvider.Oracle.Execute(
                "SP_EDIT_STRUCT_IMPOSED", parameter =>
                {
                    parameter.AddWithValue("P_GROSS_WEIGHT", objStructureImpoConst.GrossWeight, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIGN_AXLE_GROUP_WEIGHT", null, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);  // Not confirm
                    parameter.AddWithValue("P_SINGLE_AXLE_WEIGHT", objStructureImpoConst.AxleWeight, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);  // Not confirm                        
                    parameter.AddWithValue("P_HIGHWAY_SKEW", objStructureImpoConst.HighwaySkew, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIGN_GROSS_WEIGHT", objStructureImpoConst.SignedGrossWeightObj.GrossWeight, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HB_RATING_WITH_LOAD", objStructureImpoConst.HBWithLiveLoad, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HB_RATING_WITHOUT_LOAD", objStructureImpoConst.HBWithoutLiveLoad, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AXLE_GROUP_WEIGHT", null, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);   // Not confirm
                    parameter.AddWithValue("P_SIGN_HEIGHT_METRES", objStructureImpoConst.SignedHeight.HeightMeter, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIGN_HEIGHT_FEET", objStructureImpoConst.SignedHeight.HeightFeet, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIGN_WIDTH_METRES", objStructureImpoConst.SignedWidth.WidthMeter, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIGN_WIDTH_FEET", objStructureImpoConst.SignedWidth.WidthFeet, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIGN_LEN_METRES", objStructureImpoConst.SignedLength.LengthMeter, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIGN_LEN_FEET", objStructureImpoConst.SignedLength.LengthFeet, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIGN_HEIGHT_STATUS", objStructureImpoConst.SignedHeightStatus, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIGN_WIDTH_STATUS", objStructureImpoConst.SignedWidthStatus, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIGN_LEN_STATUS", objStructureImpoConst.SignedLengthStatus, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIGN_GRO_WEI_STAT", objStructureImpoConst.SignedGrossWeightStatus, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIGN_SIN_AXLE_WEI_STAT", objStructureImpoConst.SignedAxleWeightStatus, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIGN_DOUB_AXLE_WEI_STAT", null, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_HEIGHT_METRES", objStructureImpoConst.Height, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767); // Height
                    parameter.AddWithValue("P_MAX_WIDTH_METRES", objStructureImpoConst.Width, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);       //Width
                    parameter.AddWithValue("P_MAX_LEN_METRES", objStructureImpoConst.Length, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);     //Length
                    parameter.AddWithValue("P_HA_ASSESSED", objStructureImpoConst.HALoading, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIGN_SINGLE_AXLE_WEIGHT", objStructureImpoConst.SignedAxleWeight.AxleWeight, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);   // Not confirm
                    parameter.AddWithValue("P_HORI_OFFSET1", objStructureImpoConst.VerticalAlignment.EntryDistance, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VERTI_OFFSET1", objStructureImpoConst.VerticalAlignment.EntryHeight, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HORI_OFFSET2", objStructureImpoConst.VerticalAlignment.MaxDistance, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VERTI_OFFSET2", objStructureImpoConst.VerticalAlignment.MaxHeight, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HORI_OFFSET3", objStructureImpoConst.VerticalAlignment.ExitDistance, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VERTI_OFFSET3", objStructureImpoConst.VerticalAlignment.ExitHeight, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MULTI_AXLE_GROUP_WEIGHTS", objStructureImpoConst.MaxWeightOverMinDistanceWeight, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MULTI_AXLE_GROUP_LENS", objStructureImpoConst.MaxWeightOverMinDistanceDistance, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SECTION_ID", sectionId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STRUCT_ID", structureId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USER_NAME", objStructureImpoConst.UserName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", objStructureImpoConst.OrgId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                }
                );
            return true;
        }
        #endregion
        # region Structure Notification
        public static List<StructureNotification> GetAllStructureNotification(string structureId, int pageNum, int pageSize)
        {
            List<StructureNotification> notificationList = new List<StructureNotification>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                notificationList,
                UserSchema.Portal + ".GET_STRUCTURE_NOTIFICATION",
                parameter =>
                {
                    parameter.AddWithValue("STRUCTURE_ID", structureId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageNumber", pageNum, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                },
                (records, instance) =>
                {
                    instance.RevisionId = records.GetLongOrDefault("REVISION_ID");
                    instance.RevisionNo = records.GetInt16OrDefault("REVISION_NO");
                    instance.ReceivedDate = records.GetDateTimeOrDefault("NOTIFICATION_DATE");
                    instance.MovementStartDate = records.GetDateTimeOrDefault("MOVE_START_DATE");
                    instance.MovementEndDate = records.GetDateTimeOrDefault("MOVE_END_DATE");
                    instance.HaulierName = records.GetStringOrDefault("HAULIER_NAME");
                    instance.ESDALReference = records.GetStringOrDefault("ESDAL_REFERENCE");
                    instance.CountRec = records.GetDecimalOrDefault("COUNTREC");
                    instance.ApplicationStatus = records.GetInt32OrDefault("APPLICATION_STATUS");
                }
                );
            return notificationList;
        }
        public static List<StructureNotification> GetStructureVehicles(long revisionID)
        {
            List<StructureNotification> structVehiclesDetails = new List<StructureNotification>();
           
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    structVehiclesDetails,
                    UserSchema.Portal + ".GET_STRUCTURE_VEHICLES",
                    parameter =>
                    {
                        parameter.AddWithValue("p_Revision_ID", revisionID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                    },
                    (records, instance) =>
                    {
                        instance.OverallLength = records.GetDoubleOrDefault("LEN");
                        instance.OverallWidth = records.GetDoubleOrDefault("WIDTH");
                        instance.OverallHeight = records.GetDoubleOrDefault("MAX_HEIGHT");
                        instance.ReducibleHeight = records.GetDoubleOrDefault("RED_HEIGHT_MTR");
                        instance.RigidLength = records.GetDoubleOrDefault("RIGID_LEN");
                        instance.GrossWeight = records.GetInt32OrDefault("GROSS_WEIGHT_KG");
                        instance.MaxAxleWeight = records.GetDoubleOrDefault("MAX_AXLE_WEIGHT");
                        instance.RearOverHang = records.GetDoubleOrDefault("REAR_OVERHANG");
                        instance.SemiGrossWeight = records.GetDoubleOrDefault("SEMI_GROSS_WEIGHT_KG");
                        instance.SemiMaxAxleWeight = records.GetDoubleOrDefault("SEMI_MAX_AXLE_WEIGHT");
                        instance.AxleComponent = records.GetStringOrDefault("AXLE_COMPONENT");
                        instance.WheelBase = records.GetDoubleOrDefault("WHEELBASE");
                        instance.GroundClearance = records.GetLongOrDefault("RED_GROUND_CLEARANCE");
                    }
                    );
           
            return structVehiclesDetails;
        }
        public static StructureNotification GetAffectedStructure(long analysisID)
        {
            StructureNotification affectedStructureList = new StructureNotification();
           
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    affectedStructureList,
                    UserSchema.Portal + ".GET_AFFECTED_STRUCTURE_DETAIL",
                    parameter =>
                    {
                        parameter.AddWithValue("p_Analysis_ID", analysisID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                    },
                    (records, instance) =>
                    {
                        instance.AffectedStucturesXML = records.GetByteArrayOrNull("AFFECTED_STRUCTURES");
                    }
                );
            
            return affectedStructureList;
        }
        # endregion     
        /// <summary>
        /// Get Structure list with search criteria
        /// </summary>
        /// <param name="orgId">Organisatin Id</param>
        /// <param name="pageNum">Page number</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="objSearchStruct">Search criteria object</param>
        /// <returns></returns>
        public static List<StructureSummary> GetStructureListSearch(int orgId, int pageNum, int pageSize, SearchStructures objSearchStruct)
        {
            List<StructureSummary> structureSummaryObj = new List<StructureSummary>();
            if (!string.IsNullOrEmpty(objSearchStruct.StructureType))
            {
                if (objSearchStruct.StructureType.ToLower() == "underbridge")
                {
                    objSearchStruct.StructureType = "510001";
                }
                if (objSearchStruct.StructureType.ToLower() == "overbridge")
                {
                    objSearchStruct.StructureType = "510002";
                }
                if (objSearchStruct.StructureType.ToLower() == "under and over bridge")
                {
                    objSearchStruct.StructureType = "510003";
                }
                if (objSearchStruct.StructureType.ToLower() == "level crossing")
                {
                    objSearchStruct.StructureType = "510004";
                }
            }
            int icaScreening = 0;
            if (objSearchStruct.ICAMethod == "Weight Screening")
            {
                icaScreening = 1;
            }
            else if (objSearchStruct.ICAMethod == "SV Screening")
            {
                icaScreening = 2;
            }
            //Setup Procedure LIST_STRUCTURE
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                structureSummaryObj,
                UserSchema.Portal + ".SP_R_STRUCT_SEARCH",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_ID", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageNumber", pageNum, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_structure_code", objSearchStruct.SearchSummaryId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_struct_name", objSearchStruct.SearchSummaryName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_alternate_name", objSearchStruct.AlternateName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_description", objSearchStruct.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_struct_type", objSearchStruct.StructureType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_object_carried", objSearchStruct.Carries, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_object_crossed", objSearchStruct.Crosses, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_name", objSearchStruct.DelegateName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("SORT_ORDER", objSearchStruct.sortOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("PRESET_FILTER", objSearchStruct.presetFilter, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ica_option", icaScreening, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.StructureId = records.GetLongOrDefault("STRUCTURE_ID");
                        instance.StructureName = records.GetStringOrDefault("STRUCTURE_NAMES");
                        instance.StructCode = records.GetStringOrDefault("STRUCTURE_CODE");
                        instance.StructureName = records.GetStringOrDefault("STRUCTURE_NAME");
                        instance.TotalRecordCount = records.GetDecimalOrDefault("TOTAL_ROWS");
                    }
            );
            return structureSummaryObj;
        }
        /// <summary>
        /// Get Structure list with search criteria
        /// </summary>
        /// <param name="orgId">Organisatin Id</param>
        /// <param name="pageNum">Page number</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="objSearchStruct">Search criteria object</param>
        /// <returns></returns>
        public static List<StructureSummary> GetNotDelegatedStructureListSearch(int orgId, int pageNum, int pageSize, SearchStructures objSearchStruct, int sortOrder, int sortType)
        {
            //Creating new object for GetStructureList
            List<StructureSummary> structureSummaryObj = new List<StructureSummary>();
             
                if (!string.IsNullOrEmpty(objSearchStruct.StructureType))
                {
                    if (objSearchStruct.StructureType.ToLower() == "underbridge")
                    {
                        objSearchStruct.StructureType = "510001";
                    }
                    if (objSearchStruct.StructureType.ToLower() == "overbridge")
                    {
                        objSearchStruct.StructureType = "510002";
                    }
                    if (objSearchStruct.StructureType.ToLower() == "under and over bridge")
                    {
                        objSearchStruct.StructureType = "510003";
                    }
                    if (objSearchStruct.StructureType.ToLower() == "level crossing")
                    {
                        objSearchStruct.StructureType = "510004";
                    }
                }
                //Setup Procedure LIST_STRUCTURE
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    structureSummaryObj,
                    UserSchema.Portal + ".SP_NOT_DELEGATED_STR_SRCH",
                    parameter =>
                    {
                        parameter.AddWithValue("p_ORG_ID", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("pageNumber", pageNum, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_structure_code", objSearchStruct.SearchSummaryId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_struct_name", objSearchStruct.SearchSummaryName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_alternate_name", objSearchStruct.AlternateName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_description", objSearchStruct.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_struct_type", objSearchStruct.StructureType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_object_carried", objSearchStruct.Carries, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_OBJECT_CROSSED", objSearchStruct.Crosses, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_NAME", objSearchStruct.DelegateName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("SORT_ORDER", sortOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("PRESET_FILTER", sortType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ica_option", objSearchStruct.ICAMethod, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.StructureId = records.GetLongOrDefault("STRUCTURE_ID");
                            instance.StructCode = records.GetStringOrDefault("STRUCTURE_CODE");
                            instance.StructureName = records.GetStringOrDefault("STRUCTURE_NAME");
                            instance.TotalRecordCount = records.GetDecimalOrDefault("TOTAL_ROWS");
                        }
                );
             
            return structureSummaryObj;
        }
        #region NetWeb code changes 
        #region GetStructInDelegListCnt
        public static int GetStructInDelegListCnt(long OrgId, long arrangId)
        {
            //Creating new object for GetStructureList
            int count = 0;
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    count,
                    UserSchema.Portal + ".SP_R_STRUCT_IN_DELEG_COUNT",
                    parameter =>
                    {
                        parameter.AddWithValue("p_ORG_ID", OrgId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_arr_id", arrangId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records) =>
                        {
                            count = (int)records.GetDecimalOrDefault("count");
                        }
                );
           
            return count;
        }
        #endregion       
        #region public static StructureGeneralDetails ViewGeneralDetails(int structureId)
        public static List<StructureGeneralDetails> ViewGeneralDetails(long structureId)
        {
            List<StructureGeneralDetails> generalDetailsObj = new List<StructureGeneralDetails>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
              generalDetailsObj,
              UserSchema.Portal + ".SP_STRUCT_GEN_DETAILS",
              parameter =>
              {
                  parameter.AddWithValue("p_struct_ID", structureId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                  parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
              },
                  (records, instance) =>
                  {
                      instance.OwnerId = records.GetLongOrDefault("OWNER_ID");
                      instance.StructureId = records.GetLongOrDefault("STRUCTURE_ID");
                      instance.ESRN = records.GetStringOrDefault("STRUCTURE_CODE");
                      instance.StructureName = records.GetStringOrDefault("STRUCTURE_NAME");
                      instance.StructureAlternateNameOne = records.GetStringOrDefault("ALTERNATIVE_NAMES");
                      instance.Description = records.GetStringOrDefault("DESCRIPTION");
                      instance.Type = records.GetStringOrDefault("STRUCTURE_TYPE");
                      instance.Type1 = records.GetStringOrDefault("STRUCTURE_TYPE1");
                      instance.Type2 = records.GetStringOrDefault("STRUCTURE_TYPE2");
                      instance.Category = records.GetStringOrDefault("STRUCTURE_CAT");
                      instance.StructureClass = records.GetStringOrDefault("STRUCTURE_CLASS");
                      instance.OSGRNorth = records.GetLongOrDefault("NORTHING");
                      instance.OSGREast = records.GetLongOrDefault("EASTING");
                      instance.OrganisationId = records.GetLongOrDefault("ORGANISATION_ID");
                      instance.StructureOwner = records.GetStringOrDefault("ORGANISATION_NAME");
                      instance.StructureKey = records.GetStringOrDefault("STRUCT_KEY");
                      instance.Notes = records.GetStringOrDefault("NOTES");
                      instance.Length = records.GetDoubleOrDefault("STRUCTURE_LENGTH");
                      instance.StructureAlternateNameOne = records.GetStringOrDefault("ALTERNATIVE_NAMES");
                      instance.ContactId = records.GetFieldType("CONTACT_ID") != null?Convert.ToInt64(records["CONTACT_ID"]):0; //this function will convert the record object to Int64 #7197
                  }
          );
            return generalDetailsObj;
        }
        #endregion
        #region public static StructureSectionList viewUnsuitableStructureSections(long structureId)
        #endregion
        public static List<StructureNotification> GetHaulierDetailsByESDALRef(string esdalReference)
        {
            List<StructureNotification> structNotification = new List<StructureNotification>();
           
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    structNotification,
                    UserSchema.Portal + ".GET_APPLICATION_HAULIERDETAILS",
                    parameter =>
                    {
                        parameter.AddWithValue("p_NOTIFICATION_CODE", esdalReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 100);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                    },
                    (records, instance) =>
                    {
                        instance.RevisionId = records.GetLongOrDefault("REVISION_ID");
                        instance.HaulierReference = records.GetStringOrDefault("HAULIERS_REF");
                        instance.HaulierContact = records.GetStringOrDefault("HAULIER_CONTACT");
                        instance.HaulierName = records.GetStringOrDefault("HAULIER_NAME");
                        instance.Address1 = records.GetStringOrDefault("HAULIER_ADDRESS_1");
                        instance.Address2 = records.GetStringOrDefault("HAULIER_ADDRESS_2");
                        instance.Address3 = records.GetStringOrDefault("HAULIER_ADDRESS_3");
                        instance.Address4 = records.GetStringOrDefault("HAULIER_ADDRESS_4");
                        instance.Address5 = records.GetStringOrDefault("HAULIER_ADDRESS_5");
                        instance.PostalCode = records.GetStringOrDefault("HAULIER_POST_CODE");
                        instance.HaulierCountry = records.GetStringOrDefault("HAULIER_COUNTRY");
                        instance.HaulierTelNumber = records.GetStringOrDefault("HAULIER_TEL_NO");
                        instance.HaulierFaxNumber = records.GetStringOrDefault("HAULIER_FAX_NO");
                        instance.HaulierEmail = records.GetStringOrDefault("HAULIER_EMAIL");
                        instance.HaulierLicenceNumber = records.GetStringOrDefault("HAULIER_LICENCE_NO");
                        instance.ESDALReference = records.GetStringOrDefault("ESDAL_REFERENCE");
                        instance.MovementStartDate = records.GetDateTimeOrDefault("MOVE_START_DATE");
                        instance.MovementEndDate = records.GetDateTimeOrDefault("MOVE_END_DATE");
                        instance.AnalysisId = records.GetLongOrDefault("ANALYSIS_ID");
                    }
                    );
          
            return structNotification;
        }
        public static string GetStructureName(string StructureID)
        {
            string StructureName = "";
            
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    StructureName,
                    UserSchema.Portal + ".GET_STRUCTURE_NAME",
                    parameter =>
                    {
                        parameter.AddWithValue("P_STRUCTURE_ID", StructureID, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 100);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                    },
                    (records) =>
                    {
                        StructureName = records.GetStringOrDefault("STRUCTURE_NAME");
                    }
                    );
             
            return StructureName;
        }
        #endregion
        //SP_CHECK_STRUCT_AGAINST_ORG
        #region public static StructureSectionList checkStructAgainstOrg(long StructID, long orgId)
        public static int CheckStructAgainstOrg(long structureId, long organisationId)
        {
            int recCnt = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                recCnt,
               UserSchema.Portal + ".SP_CHECK_STRUCT_AGAINST_ORG",
                parameter =>
                {
                    parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STRUCT_ID", structureId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                  (records, instance) =>
                  {
                      recCnt = (int)records.GetDecimalOrDefault("REC_CNT");
                  }
               );
            return recCnt;
        }
        #endregion
        public static int CheckStructOrg(long structureId, int organisationId)
        {
            int chkCnt = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                chkCnt,
               UserSchema.Portal + ".SP_CHECK_STRUCT_ORG",
                parameter =>
                {
                    parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STRUCT_ID", structureId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                  (records, instance) =>
                  {
                      chkCnt = (int)records.GetDecimalOrDefault("REC_CNT");
                  }
               );
            return chkCnt;
        }
        /// <summary>
        /// Save structure contact
        /// </summary>
        /// <param name="structureContact">Save structure contact</param>
        /// <returns>Save structure contact</returns>
        public static bool SaveStructureContact(StructureContactModel structureContact)
        {
            bool result = false;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".MANAGE_STRUCT_CAUTION_CONTACT",
                parameter =>
                {
                    parameter.AddWithValue("p_FULL_NAME", structureContact.FullName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_DESCRIPTION", structureContact.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORG_NAME", structureContact.OrganisationName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COUNTRY", structureContact.CountryId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESS_LINE_5", structureContact.AddressLine5, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    //parameter.AddWithValue("p_USER_SCHEMA", structureContact.UserSchema, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESS_LINE_4", structureContact.AddressLine4, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_CONTACT_NO", structureContact.ContactNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_CONTACT_ID", structureContact.ContactId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESS_LINE_3", structureContact.AddressLine3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TELEPHONE", structureContact.Telephone, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESS_LINE_2", structureContact.AddressLine2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ADDRESS_LINE_1", structureContact.AddressLine1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGANISATION_ID", structureContact.OrganisationId, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ROLE_TYPE", structureContact.RoleType, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MOBILE", structureContact.Mobile, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_POST_CODE", structureContact.PostCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_EMAIL", structureContact.Email, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_EMAIL_PREFERENCE", structureContact.EmailPreference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_EXTENSION", structureContact.Extension, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FAX", structureContact.Fax, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_CAUTION_ID", structureContact.CautionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_AD_HOC", structureContact.IsAdHoc, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                },
                    (records, instance) =>
                    {
                        result = true;
                    }
            );
            return result;
        }
        #region StructureDelegation
        public static List<DelegationList> GetDelegArrangList(long organisationId, int pageNumber, int pageSize, string searchType, string searchValue, int presetFilter = 1, int? sortOrder = null)
        {
            List<DelegationList> delegarrangObj = new List<DelegationList>();
            //Setup Procedure LIST_STRUCTURE
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                delegarrangObj,
                UserSchema.Portal + ".GET_DELEGATION_ARRANGMENT_LIST",
                parameter =>
                {
                    parameter.AddWithValue("p_organisationId", organisationId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_searchType", searchType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_searchValue", searchValue, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   
                    parameter.AddWithValue("PRESET_FILTER", presetFilter, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("SORT_ORDER", sortOrder, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ArrangementId = records.GetLongOrDefault("ARRANGEMENT_ID");
                        instance.ArrangementName = records.GetStringOrDefault("NAME");
                        instance.AllowSubDelegation = Convert.ToInt32(records.GetDecimalNullable("ALLOW_SUBDELEGATION"));
                        instance.OrganisationName = records.GetStringOrDefault("ORGNAME");
                        instance.OrganisationId = records.GetLongOrDefault("ORG_FROM_ID");
                        instance.OrgToId = records.GetLongOrDefault("ORG_TO_ID");
                        instance.TotalRecordCount = records.GetDecimalOrDefault("TotalRecordCount");
                        instance.IsRoadDelegation = records.GetShortOrDefault("IS_ROAD_DELEGATION");
                        instance.DelegatedOrganisationName = records.GetStringOrDefault("DELEGATED_ORG");
                    }
            );
            foreach (DelegationList item in delegarrangObj)
            {
                // Business logic starts here to show icon indicating sub-delegating structures
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    item,
                    UserSchema.Portal + ".GET_SUBDELEGATED_STRUCTURES",
                    parameter =>
                    {
                        parameter.AddWithValue("P_ARRANGEMENT_ID", item.ArrangementId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ORGANISATION_ID", organisationId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.AllowingSubDelegateStructures = records.GetDecimalOrDefault("ALLOWING_SUBDELEG_STRUCT");

                            //Code added by Susmitha--rework

                            if (instance.AllowingSubDelegateStructures > 0)
                            {
                                instance.SubDelegation = true;
                            }
                        }
                );
            }
            return delegarrangObj;
        }
        public static DelegationList GetArrangementDetails(long arrangId, int orgId)
        {
            DelegationList objdelegarrang = new DelegationList();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                objdelegarrang,
                UserSchema.Portal + ".SP_R_REVIEW_DELEG",
                parameter =>
                {
                    parameter.AddWithValue("p_arr_id", arrangId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORG_ID", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ArrangementName = records.GetStringOrDefault("NAME");
                        instance.OrganisationName = records.GetStringOrDefault("ORGNAME");
                        instance.ContactName = records.GetStringOrDefault("FIRST_NAME");
                        instance.AllowSubDelegation = records.GetInt16OrDefault("ALLOW_SUBDELEGATION");
                        instance.RetainNotification = records.GetInt16OrDefault("RETAIN_NOTIFICATION");
                        instance.AcceptFailure = records.GetInt16OrDefault("ACCEPT_FAILURES");
                        instance.OrgToId = records.GetLongOrDefault("ORG_TO_ID");
                        instance.ContactId = records.GetLongOrDefault("CONTACT_TO_ID");
                    }
            );
            return objdelegarrang;
        }
        public static List<DelegationList> GetOrganisationList(int pageNumber, int pageSize, string Organisationname)
        {
            List<DelegationList> dispOrgList = new List<DelegationList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                 dispOrgList,
                 UserSchema.Portal + ".GET_ORGANISATION_LIST_SEARCH",
                 parameter =>
                 {
                     parameter.AddWithValue("p_ORGNAME", Organisationname, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                 },
                 (records, instance) =>
                 {
                     instance.OrganisationId = records.GetLongOrDefault("ORGANISATION_ID");
                     instance.OrganisationName = records.GetStringOrDefault("ORGNAME");
                     instance.OrganisationCode = records.GetStringOrDefault("ORG_CODE");
                     instance.TotalRecordCount = Convert.ToInt32(records.GetDecimalOrDefault("TotalRecordCount"));

                 }
                 );
            return dispOrgList;
        }
        public static List<DelegationList> GetDelegArrangementList(int orgId, int pageNum, int pageSize, string arrangName)
        {
            //Creating new object for GetStructureList
            List<DelegationList> delegarrangObj = new List<DelegationList>();
            //Setup Procedure LIST_STRUCTURE
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                delegarrangObj,
                UserSchema.Portal + ".SP_R_MY_DELEG",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_ID", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_NAME", arrangName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageNumber", pageNum, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ArrangementId = records.GetLongOrDefault("ARRANGEMENT_ID");
                        instance.ArrangementName = records.GetStringOrDefault("NAME");
                        instance.AllowSubDelegation = records.GetInt16OrDefault("ALLOW_SUBDELEGATION");
                        instance.OrganisationName = records.GetStringOrDefault("ORGNAME");
                        instance.OrgToId = records.GetLongOrDefault("ORG_TO_ID");
                    }
            );
            return delegarrangObj;
        }
        public static List<DropDown> GetDelegationNameList(int organisationId, string delegationName)
        {
            //Creating new object for GetStructureList
            List<DropDown> dropDownObj = new List<DropDown>();
            //Setup Procedure LIST_STRUCTURE
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                dropDownObj,
                UserSchema.Portal + ".SP_R_DELEG_SEARCH",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_NAME", delegationName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.Value = records.GetStringOrDefault("NAME");
                    }
            );
            return dropDownObj;
        }
        public static List<StructureInDelegationList> GetStructuresInDelegation(long arrangId, long orgId, int? pageNum, int? pageSize)
        {
            List<StructureInDelegationList> objstructindeleg = new List<StructureInDelegationList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                objstructindeleg,
                UserSchema.Portal + ".SP_R_STRUCT_IN_DELEG",
                parameter =>
                {
                    parameter.AddWithValue("p_arr_id", arrangId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageNumber", pageNum, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORG_ID", orgId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.StructureId = records.GetLongOrDefault("structure_id");
                        instance.StructureName = records.GetStringOrDefault("structure_name");
                        instance.StructureReference = records.GetStringOrDefault("structure_code");
                        instance.StructureOwnedBy = records.GetStringOrDefault("owner_name");
                        instance.TotalRecordCount = records.GetDecimalOrDefault("TotalRecordCount");
                        instance.OwnerId = records.GetLongOrDefault("OWNER_ID");
                    }
            );
            return objstructindeleg;
        }
        public static List<DelegationList> GetContactList(int pageNumber, int pageSize, string Contactname, int orgID)
        {
            List<DelegationList> dispOrgList = new List<DelegationList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                dispOrgList,
                UserSchema.Portal + ".GET_CONTACTS_LIST_SEARCH",
                parameter =>
                {
                    parameter.AddWithValue("p_ContactName", Contactname, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OrgId", orgID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.ContactName = records.GetStringOrDefault("ContactName");
                    instance.ContactId = Convert.ToInt64(records.GetDecimalOrDefault("contact_id"));
                    instance.TotalRecordCount = Convert.ToInt32(records.GetDecimalOrDefault("TotalRecordCount"));
                }
             );
            return dispOrgList;
        }
        public static List<RoadDelegationList> GetRoadDelegationList(int pageNum, int pageSize, long OrganisationId,int presetFilter=1,int? sortOrder=null)
        {
            List<RoadDelegationList> roadDelegationListObj = new List<RoadDelegationList>();
            //Setup Procedure GET_ROAD_DELEG_ARRANGMENT_LIST
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                roadDelegationListObj,
                UserSchema.Portal + ".GET_ROAD_DELEG_ARRANGMENT_LIST",
                parameter =>
                {
                    parameter.AddWithValue("pageNumber", pageNum, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_OrganisationId", OrganisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("PRESET_FILTER", presetFilter, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("SORT_ORDER", sortOrder, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ArrangementId = records.GetLongOrDefault("ARRANGEMENT_ID");
                        instance.DelegatingSOAName = records.GetStringOrDefault("delgating_org");
                        instance.SOAName = records.GetStringOrDefault("orgname");
                        instance.ContactName = string.Format("{0} {1}", records.GetStringOrDefault("first_name"), records.GetStringOrDefault("sur_name"));
                        instance.DelegationName = records.GetStringOrDefault("name");
                        instance.RetainNotification = records.GetDecimalOrDefault("retain_notification") == 0 ? "false" : "true";
                        instance.CreatedDate = records.GetDateTimeOrDefault("creation_date") == DateTime.MinValue? null: records.GetDateTimeOrDefault("creation_date").ToString("dd/MM/yyyy");
                        instance.TotalRecordCount = records.GetDecimalOrDefault("TotalRecordCount");
                    }
            );
            return roadDelegationListObj;
        }
        internal static List<StructureInDelegationList> GetStructureInDelegationList(int pageNumber, int? pageSize, string structurecodes, int OrganisationId, int structurecodecount)
        {
            string[] StructureCodeArray = structurecodes.Split(',');
            List<StructureInDelegationList> StruInDelegList = new List<StructureInDelegationList>();
            List<string> StructureCodeList = new List<string>(structurecodes.Split(','));
            int divisor = StructureCodeArray.Length / 300;
            for (int i = 0; i <= divisor; i++)
            {
                List<StructureInDelegationList> StruInDelegInnerList = new List<StructureInDelegationList>();
                var StructureItems = StructureCodeList.Skip(i * 300).Take(300).ToList();
                string strucItm = string.Join(",", StructureItems);
                if (StructureItems != null && StructureItems.Count > 0)
                {
                    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    StruInDelegInnerList,
                    UserSchema.Portal + ".GET_STRUC_IN_DELEG_LIST",
                    parameter =>
                    {
                        parameter.AddWithValue("P_pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_Organisation_Id", OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_Structure_Codes", strucItm, OracleDbType.Clob, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_Structure_Code_Count", StructureItems.Count, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    (records, instance) =>
                    {
                        instance.StructureId = records.GetLongOrDefault("STRUCTURE_ID");
                        instance.StructureName = records.GetStringOrDefault("STRUCTURE_NAME");
                        instance.StructureReference = records.GetStringOrDefault("STRUCTURE_CODE");
                        instance.StructureOwnedBy = records.GetStringOrDefault("OWNER_NAME");
                        instance.OwnerId = records.GetLongOrDefault("OWNER_Id");
                        instance.TotalRecordCount = Convert.ToInt32(records.GetDecimalOrDefault("TotalRecordCount"));
                    }
                    );
                }
                StruInDelegList.AddRange(StruInDelegInnerList);
            }
            StruInDelegList = StruInDelegList.GroupBy(x => x.StructureId).Select(y => y.First()).ToList();
            return StruInDelegList;
        }
        internal static List<StructureInDelegationList> GetStructureInDelegationList(string[] structurecodes, int OrganisationId)
        {
            string[] StructureCodeArray = structurecodes;
            List<StructureInDelegationList> StruInDelegList = new List<StructureInDelegationList>();
            List<string> StructureCodeList = structurecodes.ToList();
            int divisor = StructureCodeArray.Length / 300;
            for (int i = 0; i <= divisor; i++)
            {
                List<StructureInDelegationList> StruInDelegInnerList = new List<StructureInDelegationList>();
                var StructureItems = StructureCodeList.Skip(i * 300).Take(300).ToList();
                string strucItm = string.Join(",", StructureItems);
                if (StructureItems != null && StructureItems.Count > 0)
                {
                    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                StruInDelegInnerList,
                UserSchema.Portal + ".GET_STRUC_IN_DELEG_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_pageSize", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_pageNumber", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_Organisation_Id", OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_Structure_Codes", strucItm, OracleDbType.Clob, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_Structure_Code_Count", StructureItems.Count, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.StructureId = records.GetLongOrDefault("STRUCTURE_ID");
                    instance.StructureName = records.GetStringOrDefault("STRUCTURE_NAME");
                    instance.StructureReference = records.GetStringOrDefault("STRUCTURE_CODE");
                    instance.StructureOwnedBy = records.GetStringOrDefault("OWNER_NAME");
                    instance.OwnerId = records.GetLongOrDefault("OWNER_Id");
                    instance.TotalRecordCount = Convert.ToInt32(records.GetDecimalOrDefault("TotalRecordCount"));
                }
                );
                }
                StruInDelegList.AddRange(StruInDelegInnerList);
            }
            StruInDelegList = StruInDelegList.GroupBy(x => x.StructureId).Select(y => y.First()).ToList();
            return StruInDelegList;
        }
        internal static bool ManageStructureDelegation(DelegationList delegationList)//Calling BulkInsertToStructureDelegation
        {
            bool result = false;
            result = BulkInsertToStructureDelegation(delegationList);
            if (!result)
            {
                Parallel.ForEach(delegationList.StructureInDelegations, item =>
                {
                    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                        delegationList,
                        UserSchema.Portal + ".MANAGE_STRUCTURE_DELEGATION",
                        parameter =>
                        {
                            parameter.AddWithValue("P_ARRANGEMENT_ID", delegationList.ArrangementId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_STRUCTURE_ID", item.StructureId, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                            parameter.AddWithValue("P_OWNER_ID", item.OwnerId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        },
                            records =>
                            {
                                result = true;
                            }
                    );
                });
            }
            return result;
        }
        internal static bool BulkInsertToStructureDelegation(DelegationList delegationList)
        {
            bool result = true;
            string query = @"insert into STRUCTURE_DELEGATIONS(arrangement_id, structure_id, owner_id) values (:arrng_id, :strctr_id, :ownrId)";
            int rows = SafeProcedure.DBProvider.Oracle.ExecuteBulkNonQuery(query,
                    parameter =>
                    {
                        parameter.AddWithValue(":arrng_id", delegationList.StructureInDelegations.Select(c => delegationList.ArrangementId).ToArray(), OracleDbType.Int64, ParameterDirectionWrap.Input);
                        parameter.AddWithValue(":strctr_id", delegationList.StructureInDelegations.Select(c => c.StructureId).ToArray(), OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                        parameter.AddWithValue(":ownrId", delegationList.StructureInDelegations.Select(c => c.OwnerId).ToArray(), OracleDbType.Long, ParameterDirectionWrap.Input);
                    }, delegationList.StructureInDelegations.Count
                );
            if (!rows.Equals(delegationList.StructureInDelegations.Count))
            {
                return false;
            }
            else
            {
                return result;
            }
        }
        internal static bool ManageDelegationStructureContact(DelegationList structureContact)
        {
            bool result = true;
            StructureContactsList contactList = new StructureContactsList();
            foreach (StructureContactsList item in structureContact.StructureInContactList)
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    structureContact,
                    UserSchema.Portal + ".MANAGE_STRUCTURE_CONTACTS",
                    parameter =>
                    {
                        parameter.AddWithValue("P_STRUCTURE_ID", item.StructureId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_STRUCTURE_CODE", item.StructureCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ORGANISATION_ID", structureContact.OrgToId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_CONTACT_ID", structureContact.ContactId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ARRANGEMENT_ID", structureContact.ArrangementId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_LAST_DELEGATION", 1, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RECEIVE_FAILURES", structureContact.AcceptFailure, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_STRUCTURE_ENABLED", 1, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RETAIN_NOTIFICATION", structureContact.RetainNotification, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_FROM_ORGANISATION_ID", structureContact.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        records =>
                        {
                            contactList.StructureId = records.GetLongOrDefault("STRUCTURE_ID");
                            contactList.ChainNo = records.GetLongOrDefault("CHAIN_NO");
                            contactList.OrganisationId = records.GetLongOrDefault("ORGANISATION_ID");
                            contactList.OrganisationName = records.GetStringOrDefault("ORGANISATION_NAME");
                        }
                );
            }
            return result;
        }
        public static int CheckSubDelegationList(long structureId, long organisationId)
        {
            int allowSubDelegation = 2;
            StructureContactsList contactList = new StructureContactsList();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    contactList,
                    UserSchema.Portal + ".GET_SUBDELEGATION",
                    parameter =>
                    {
                        parameter.AddWithValue("P_STRUCTURE_ID", structureId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ORGANISATION_ID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                    },
                    (records, instance) =>
                    {
                        contactList.AllowSubDelegation = records.GetInt16OrDefault("ALLOW_SUBDELEGATION");
                        contactList.OrganisationCount = records.GetDecimalOrDefault("P_CHECKORGANISATION");
                        contactList.DefaultContact = records.GetStringOrDefault("DEFAULTCONTACT");
                    }
            );
            if (contactList.DefaultContact != null && contactList.DefaultContact != string.Empty && contactList.OrganisationCount == 0)
            {
                allowSubDelegation = 0;
            }
            else if (contactList.DefaultContact != null && contactList.DefaultContact != string.Empty)
            {
                allowSubDelegation = contactList.AllowSubDelegation;
            }
            return allowSubDelegation;
        }
        public static int DeleteStructureEdit(long arrangementId)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                UserSchema.Portal + ".DELETE_STRUCTURE_EDIT",
                parameter =>
                {
                    parameter.AddWithValue("P_ARRANGEMENT_ID", arrangementId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                   (records) =>
                   {
                       result = records.GetInt32("P_AFFECTED_ROWS");
                   }
           );
            return result;
        }
        public static List<StructureContactsList> GetStructureContactList(long arrangementId)
        {
            List<StructureContactsList> structureContactList = new List<StructureContactsList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    structureContactList,
                    UserSchema.Portal + ".GET_STRUCTURE_CONTACTLIST",
                    parameter =>
                    {
                        parameter.AddWithValue("P_ARRANGEMENT_ID", arrangementId, OracleDbType.Long, ParameterDirectionWrap.Input, 100);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                    },
                    (records, instance) =>
                    {
                        instance.StructureId = records.GetLongOrDefault("STRUCTURE_ID");
                        instance.StructureCode = records.GetStringOrDefault("STRUCTURE_CODE");
                        instance.ArrangementId = records.GetLongOrDefault("ARRANGEMENT_ID");
                        instance.OwnerId = records.GetLongOrDefault("OWNER_ID");
                    }
            );
            return structureContactList;
        }
        public static List<StructureContactModel> GetStructureContactList(int pageNumber, int pageSize, long CautionID, short ContactNo)
        {
            //Creating new object for GetCautionList
            List<StructureContactModel> listStructureContact = new List<StructureContactModel>();
            //Setup Procedure LIST_Infromation
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listStructureContact,
                UserSchema.Portal + ".GET_STRU_CAUTION_CONTACT_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CAUTION_ID", CautionID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTACT_NO", ContactNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.CautionId = records.GetLongOrDefault("CAUTION_ID");
                        instance.ContactNo = records.GetInt16OrDefault("CONTACT_NO");
                        instance.Description = records.GetStringOrDefault("DESCRIPTION");
                        instance.FullName = records.GetStringOrDefault("FULL_NAME");
                        instance.OrganisationName = records.GetStringOrDefault("ORGANISATION_NAME");
                        instance.AddressLine1 = records.GetStringOrDefault("ADDRESS_LINE_1");
                        instance.AddressLine2 = records.GetStringOrDefault("ADDRESS_LINE_2");
                        instance.AddressLine3 = records.GetStringOrDefault("ADDRESS_LINE_3");
                        instance.AddressLine4 = records.GetStringOrDefault("ADDRESS_LINE_4");
                        instance.AddressLine5 = records.GetStringOrDefault("ADDRESS_LINE_5");
                        instance.PostCode = records.GetStringOrDefault("POST_CODE");
                        instance.CountryId = records.GetInt32OrDefault("COUNTRY");
                        instance.Telephone = records.GetStringOrDefault("TELEPHONE");
                        instance.Extension = records.GetStringOrDefault("EXTENSION");
                        instance.Mobile = records.GetStringOrDefault("MOBILE");
                        instance.Fax = records.GetStringOrDefault("FAX");
                        instance.Email = records.GetStringOrDefault("EMAIL");
                        instance.EmailPreference = records.GetStringOrDefault("EMAIL_PREFERENCE");
                        instance.TotalRecordCount = records.GetDecimalOrDefault("TotalRecordCount");
                    }
            );
            return listStructureContact;
        }
        public static int DeleteStructInDelegation(long structId, long arrangId)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(

               UserSchema.Portal + ".DELETE_STRUCT_IN_DELEGATION",
               parameter =>
               {
                   parameter.AddWithValue("P_STRUCT_ID", structId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_ARRANGEMENT_ID", arrangId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
               },
               (records) =>
               {
                   result = records.GetInt32("P_AFFECTED_ROWS");
               }
           );

            return result;
        }
        public static List<StructureSectionList> ViewStructureSections(long structureId)
        {
            List<StructureSectionList> structureSectionsObj = new List<StructureSectionList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
             structureSectionsObj,
             UserSchema.Portal + ".SP_STRUCT_SECTION",
             parameter =>
             {
                 parameter.AddWithValue("P_STRUCT_ID", structureId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                 parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
             },
                 (records, instance) =>
                 {
                     instance.SectionId = records.GetLongOrDefault("SECTION_ID");
                     instance.StructureSections = records.GetStringOrDefault("STRUCTURE_SECTION");
                 }
              );
            return structureSectionsObj;
        }
        public static ManageStructureICA viewEnabledICA(int structureId, int sectionId, long OrgID)
        {
            ManageStructureICA icaDetailsObj = new ManageStructureICA();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
           icaDetailsObj,
           UserSchema.Portal + ".SP_ICA_MANAGE_USAGE",
           parameter =>
           {
               parameter.AddWithValue("p_ORG_ID", OrgID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
               parameter.AddWithValue("p_STRUCT_ID", structureId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
               parameter.AddWithValue("p_SECTION_ID", sectionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
               parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
           },
            (records, instance) =>
            {
                instance.EnableGrossWeight = records.GetDecimalNullable("VAR_EN_WEIGHT");
                instance.EnableAxleWeight = records.GetDecimalNullable("VAR_EN_AXLE_WEIGHT");
                instance.EnableWeightOverDist = records.GetDecimalNullable("VAR_EN_WEIGHT_OVR_DIST");
                instance.EnableAWR = records.GetDecimalNullable("VAR_EN_WEIGHT_AWR");
                instance.EnableSV80 = records.GetDecimalNullable("VAR_EN_SV_80");
                instance.EnableSV100 = records.GetDecimalNullable("VAR_EN_SV_100");
                instance.EnableSV150 = records.GetDecimalNullable("VAR_EN_SV_150");
                instance.EnableSVTrain = records.GetDecimalNullable("VAR_EN_SV_TRAIN");
                instance.DefaultWSBandLimitMin = (double)records.GetDecimalOrDefault("WEIGHT_LOWER_DEF");
                instance.DefaultWSBandLimitMax = (double)records.GetDecimalOrDefault("WEIGHT_UPPER_DEF");
                instance.DefaultSVBandLimitMin = (double)records.GetDecimalOrDefault("SV_LOWER_DEF");
                instance.DefaultSVBandLimitMax = (double)records.GetDecimalOrDefault("SV_UPPER_DEF");
                instance.CustomWSBandLimitMin = (double?)records.GetDecimalNullable("WEIGHT_LOWER");
                instance.CustomWSBandLimitMax = (double?)records.GetDecimalNullable("WEIGHT_UPPER");
                instance.CustomSVBandLimitMin = (double?)records.GetDecimalNullable("SV_LOWER");
                instance.CustomSVBandLimitMax = (double?)records.GetDecimalNullable("SV_UPPER");
            }
            );
            return icaDetailsObj;
        }
        public static List<SvReserveFactors> viewSVData(int structureId, int sectionId)
        {
            List<SvReserveFactors> reserveFactorObj = new List<SvReserveFactors>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
             reserveFactorObj,
             UserSchema.Portal + ".SP_SV_DATA",
             parameter =>
             {
                 parameter.AddWithValue("P_STRUCT_ID", structureId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                 parameter.AddWithValue("P_SECTION_ID", sectionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                 parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
             },
              (records, instance) =>
              {
                  instance.SVDerivation = records.GetInt32OrDefault("SV_DERIVATION");
                  instance.VehicleType = records.GetInt32OrDefault("VEHICLE_TYPE");
                  instance.ParamNumber = records.GetLongOrDefault("PARAMETER_NO");
                  instance.SVLiveLoad = records.GetDoubleNullable("SV_RES_WITH_LOAD");
                  instance.SVNoLoad = records.GetDoubleNullable("SV_RES_MINUS_LOAD");
              }
              );
            return reserveFactorObj;
        }
        public static List<DelegationArrangment> viewDelegationArrangment(long orgId)
        {
            List<DelegationArrangment> structureSectionsObj = new List<DelegationArrangment>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
            structureSectionsObj,
            UserSchema.Portal + ".GET_DELEGATION_ARRANGMENT_LIST",
            parameter =>
            {
                parameter.AddWithValue("p_organisationId", orgId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("p_pageNumber", 1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("p_pageSize", 100, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("p_searchType", null, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("p_searchValue", null, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
                (records, instance) =>
                {
                    instance.ArrangmentId = records.GetLongOrDefault("ARRANGEMENT_ID");
                    instance.Name = records.GetStringOrDefault("NAME");
                }
             );
            return structureSectionsObj;
        }
        public static List<StructureSectionList> viewUnsuitableStructSections(long structureId, long route_part_id, long section_id, string cont_ref_num)
        {
            List<StructureSectionList> structureSectionsObj = new List<StructureSectionList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
          structureSectionsObj,
          UserSchema.Portal + ".SP_UNSUITABLE_SECTION",
          parameter =>
          {
              parameter.AddWithValue("P_STRUCT_ID", structureId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
              parameter.AddWithValue("P_SECT_ID", section_id, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
              parameter.AddWithValue("P_ROUTE_ID", route_part_id, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
              parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
          },
              (records, instance) =>
              {
                  instance.SectionId = records.GetLongOrDefault("SECTION_ID");
                  instance.StructureSections = records.GetStringOrDefault("STRUCTURE_SECTION");
                  instance.AffectFlag = (int)records.GetDecimalOrDefault("AFFECT_FLAG");
                  instance.VehicleName = records.GetStringOrDefault("VEHICLE_NAME");
              }
           );
            return structureSectionsObj;
        }
        public static long GetStructureId(string structureCode)
        {
            long struct_id = 0;
            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                struct_id,
                UserSchema.Portal + ".SP_GET_STRUCT_ID",
                parameter =>
                {
                    parameter.AddWithValue("P_STRUCT_CODE", structureCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                  (records, instance) =>
                  {
                      struct_id = records.GetLongOrDefault("STRUCTURE_ID");
                  }
               );
            }
            catch (Exception ex)
            {
                struct_id = -1;
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - StructureDAO/GetStructureId,Exception:" + ex);
            }
            return struct_id;
        }
        internal static DelegationList ManageDelegationArrangement(DelegationList savedelegation, long organisationId)
        {
            DelegationList delegationList = new DelegationList();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                delegationList,
                UserSchema.Portal + ".MANAGE_DELEGATION_ARRANGEMENT",
                parameter =>
                {
                    parameter.AddWithValue("P_ARRANGEMENT_ID", savedelegation.ArrangementId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NAME", savedelegation.ArrangementName, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_ORG_FROM_ID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_TO_ID", savedelegation.OrganisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTACT_TO_ID", savedelegation.ContactId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    if (savedelegation.CopyNotification)
                    {
                        parameter.AddWithValue("P_RETAIN_NOTIFICATION", 1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    }
                    else
                    {
                        parameter.AddWithValue("P_RETAIN_NOTIFICATION", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    }
                    if (savedelegation.SubDelegation)
                    {
                        parameter.AddWithValue("P_ALLOW_SUBDELEGATION", 1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    }
                    else
                    {
                        parameter.AddWithValue("P_ALLOW_SUBDELEGATION", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    }
                    parameter.AddWithValue("P_ACCEPT_FAILURES", savedelegation.AcceptFailure, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_DELEGATE_ALL", savedelegation.DelegateAll, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_IS_ROAD_DELEGATION", savedelegation.IsRoadDelegation, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_IS_AREA_DELEGATION", savedelegation.IsAreaDelegation, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        delegationList.ArrangementId = records.GetLongOrDefault("MAXID");
                        delegationList.OrgToId = records.GetLongOrDefault("ORG_TO_ID");
                        delegationList.ContactId = records.GetLongOrDefault("CONTACT_TO_ID");
                        delegationList.AcceptFailure = records.GetInt16OrDefault("ACCEPT_FAILURES");
                        delegationList.RetainNotification = records.GetInt16OrDefault("RETAIN_NOTIFICATION");
                    }
            );
            return delegationList;
        }
        /// <summary>
        /// Deleter structure contact
        /// </summary>
        /// <param name="CONTACT_NO">Contact no</param>
        /// <param name="STRUCTURE_ID">Structure id</param>
        /// <returns>Delete Structure contact </returns>
        public static int DeleteStructureContact(short CONTACT_NO, long CAUTION_ID)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(

                UserSchema.Portal + ".DELETE_STRUCT_CAUTION_CONTACT",
                parameter =>
                {
                    parameter.AddWithValue("P_CAUTION_ID", CAUTION_ID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTACT_NO", CONTACT_NO, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
               (records) =>
               {
                   result = records.GetInt32("P_AFFECTED_ROWS");
               }
            );
            return result;
        }
        public static int DeleteStructureContact(long StructureId, string StructureCode, long ArrangementId, long OwnerID)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(

                  UserSchema.Portal + ".DELETE_STRUCTURE_CONTACTS",
                 parameter =>
                 {
                     parameter.AddWithValue("P_ARRANGEMENT_ID", ArrangementId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_STRUCTURE_ID", StructureId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_STRUCTURE_CODE", StructureCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_OWNER_ID", OwnerID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                 },
                (records) =>
                {
                    result = records.GetInt32("P_AFFECTED_ROWS");
                }
             );
            return result;
        }
        /// <summary>
        /// Delete Delegation Arrangement
        /// </summary>
        /// <param name="delegId">delegationId id</param>
        /// <returns>ture/false</returns>
        public static int DeleteDelegationArrangement(long arrangId)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(

                  UserSchema.Portal + ".DELETE_DELEGATION_ARRANGEMENT",
                   parameter =>
                   {
                       parameter.AddWithValue("P_ARRANGEMENT_ID", arrangId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                   },
                   (records) =>
                   {
                       result = records.GetInt32("P_AFFECTED_ROWS");
                   }
               );

            return result;
        }
        #endregion

        #region Perform ICA analysis for given vehicle
        public static string GetVehicleICAResult(ICAVehicleModel objICAVehicleModel, ManageStructureICA objManageStructureICA, int movementClassConfig, int configType, int compNum, int? tractorType, int? trailerType, int structureId, int sectionId, int orgId)
        {
            string result = null;
            string tractrType = tractorType.ToString();
            string trailrType = trailerType.ToString();
            string componentType = String.Concat(tractrType, ',', trailrType);

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
            result,
           UserSchema.Portal + ".SP_CALCULATE_VEHICLE_ICA",
            parameter =>
            {
                parameter.AddWithValue("P_STRUCT_ID", structureId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_SECTION_ID", sectionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_ORG_ID", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);

                parameter.AddWithValue("P_VHCL_WEIGHT", objICAVehicleModel.VhclGrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_VHCL_LENGTH", objICAVehicleModel.VehicleLength, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_VHCL_WIDTH", objICAVehicleModel.VehicleWidth, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_VHCL_MAX_AXLE_WEIGHT", objICAVehicleModel.VehicleMaxAxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_VHCL_MIN_AXLE_SPACING", objICAVehicleModel.VehicleMinAxleSpacing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);

                parameter.AddWithValue("P_VHCL_CONFIG_TYPE", configType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_VHCL_CLASSIFICATION", movementClassConfig, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_VHCL_COMPONENT", componentType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_VHCL_COUNT", compNum, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);

                parameter.AddWithValue("P_TRAILR_GROSS_WEIGHT", objICAVehicleModel.TrailerGrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_TRACTR_GROSS_WEIGHT", objICAVehicleModel.TractorGrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);

                parameter.AddWithValue("P_TRAILR_MAX_AXLE_WEIGHT", objICAVehicleModel.TrailerMaxAxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_TRACTR_MAX_AXLE_WEIGHT", objICAVehicleModel.TractorMaxAxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);

                parameter.AddWithValue("P_TRAILR_MIN_AXLE_SPACING", objICAVehicleModel.TrailerMinAxleSpacing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_TRACTR_MIN_AXLE_SPACING", objICAVehicleModel.TractorMinAxleSpacing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);

                parameter.AddWithValue("P_TRACTOR_AXLE_COUNT", objICAVehicleModel.TractorAxleCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);

                parameter.AddWithValue("P_SV_SCREENING_UPPER_LIMIT", (objManageStructureICA.CustomSVBandLimitMax == null) ? objManageStructureICA.DefaultSVBandLimitMax : objManageStructureICA.CustomSVBandLimitMax, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_SV_SCREENING_LOWER_LIMIT", (objManageStructureICA.CustomSVBandLimitMin == null) ? objManageStructureICA.DefaultSVBandLimitMin : objManageStructureICA.CustomSVBandLimitMin, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);

                parameter.AddWithValue("P_WEIGHT_SCREENING_UPPER", (objManageStructureICA.CustomWSBandLimitMax == null) ? objManageStructureICA.DefaultWSBandLimitMax : objManageStructureICA.CustomWSBandLimitMax, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_WEIGHT_SCREENING_LOWER", (objManageStructureICA.CustomWSBandLimitMin == null) ? objManageStructureICA.DefaultWSBandLimitMin : objManageStructureICA.CustomWSBandLimitMin, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);

                parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
                (records) =>
                {
                    result = records.GetStringOrDefault("OUTPUT");
                }
            );

            return result;
        }
        #endregion

        #region Onerous Structure Vehicles

        public static List<StructureNotification> GetAllStructureOnerousVehicles(string structureId, int pageNumber, int pageSize, string searchCriteria, string searchStatus, DateTime? startDate, DateTime? endDate, int statusCount, int sort, long organisationId)
        {
            
                List<StructureNotification> notificationList = new List<StructureNotification>();

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    notificationList,
                    UserSchema.Portal + ".GET_STRUCTURE_ONEROUS_VEHICLE",
                    parameter =>
                    {
                        parameter.AddWithValue("STRUCT_ID", structureId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("Search_Criteria", searchCriteria, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("Status_Search", searchStatus, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("Start_Date", startDate, OracleDbType.Date, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("End_Date", endDate, OracleDbType.Date, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_STATUS_ITEMS_COUNT", statusCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_SORT", sort, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                    },

                    (records, instance) =>
                    {
                        instance.AnalysisId = records.GetLongOrDefault("ANALYSIS_ID");

                        instance.ReceivedDate = records.GetDateTimeOrDefault("NOTIFICATION_DATE");

                        instance.MovementStartDate = records.GetDateTimeOrDefault("MOVE_START_DATE");
                        instance.MovementEndDate = records.GetDateTimeOrDefault("MOVE_END_DATE");

                        instance.HaulierName = records.GetStringOrDefault("HAULIER_NAME");

                        instance.ESDALReference = records.GetStringOrDefault("ESDAL_REFERENCE");
                        instance.CountRec = records.GetDecimalOrDefault("COUNTREC");
                        instance.ApplicationStatus = records.GetInt32OrDefault("STATUS");
                        instance.GrossWeight = records.GetInt32OrDefault("GROSS_WEIGHT_MAX_KG");
                        instance.MaxAxleWeight = records.GetInt32OrDefault("MAX_AXLE_WEIGHT_MAX_KG");
                        instance.VersionId = records.GetLongOrDefault("VERSION_ID");
                        instance.ICA = records.GetInt32OrDefault("ICA");
                        //instance.RoutePartId = records.GetDecimalOrDefault("ROUTE_PART_ID");
                    }

                    );

                return notificationList;
           
        }

        #endregion

        public static List<StructureDeleArrList> GetStructureDeleArrg(string StructureCode)
        {
            List<StructureDeleArrList> arrgList = new List<StructureDeleArrList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList
                (
                    arrgList,
                    UserSchema.Portal + ".SP_GET_STRUCT_DEL_ARRG",
                    parameter =>
                    {
                        parameter.AddWithValue("p_STRUCT_CODE", StructureCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                    },
                    (records, instance) =>
                    {
                        instance.StructueId = Convert.ToInt32(records.GetLongOrDefault("STRUCTURE_ID"));
                        instance.StructureCode = records.GetStringOrDefault("structure_code");
                        instance.ArrangementId = Convert.ToInt32(records.GetLongOrDefault("ARRANGEMENT_ID"));
                        instance.OwnerId = Convert.ToInt32(records.GetLongOrDefault("OWNER_ID"));
                        instance.FromOrganisation = Convert.ToInt32(records.GetLongOrDefault("ORG_FROM_ID"));
                        instance.ToOrganisation = Convert.ToInt32(records.GetLongOrDefault("ORG_TO_ID"));
                        instance.AllowSubDelegation = (records.GetInt16OrDefault("ALLOW_SUBDELEGATION"));

                        instance.TotalCount = Convert.ToInt32(records.GetDecimalOrDefault("TotalCount"));
                    }
                 );
            return arrgList;

        }
    }

}