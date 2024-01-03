using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using STP.DataAccess.SafeProcedure;
using STP.DataAccess.Interface;
using STP.Common.Enums;
using Oracle.DataAccess.Client;
using System.Runtime.Serialization.Json;
using NetSdoGeometry;
using STP.Common.Constants;
using System.Web;
using STP.Domain;
using STP.Domain.RoadNetwork.RoadOwnership;
using STP.Common.Logger;
using System.Configuration;
using STP.Domain.Structures;
namespace STP.Structures.Persistance
{   
    public static class StructureManager
    {
        
        #region List<StructureInfo> GetMyStructureInfoList(int orgId, int other, int left, int right, int bottom, int top) New Class
        public static List<StructureInfo> GetMyStructureInfoList(int orgId, int other, int left, int right, int bottom, int top)
        {
            List<StructureInfo> structureInfoList = new List<StructureInfo>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                        structureInfoList,
                        UserSchema.Portal + ".SP_STRUCT_BOUNDING_BOX",
                        parameter =>
                        {
                            parameter.AddWithValue("P_ORG_ID", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_SHOW_OTHER", other, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_LEFT", left, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_RIGHT", right, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_BOTTOM", bottom, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_TOP", top, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                        },
                        (records, instance) =>
                        {
                            instance.StructureId = records.GetLongOrDefault("STRUCTURE_ID");
                            instance.StructureName = records.GetStringOrDefault("STRUCTURE_NAME");
                            instance.StructureDescr = records.GetStringOrDefault("DESCRIPTION");
                            instance.StructureClass = records.GetStringOrDefault("TYPE_NAME");
                            instance.StructureCode = records.GetStringOrDefault("STRUCTURE_CODE");

                            instance.Northing = records.GetLongOrDefault("NORTHING");
                            instance.Easting = records.GetLongOrDefault("EASTING");

                            instance.FromNorthing = records.GetInt32OrDefault("FROM_NORTHING");
                            instance.FromEasting = records.GetInt32OrDefault("FROM_EASTING");
                            instance.ToEasting = records.GetInt32OrDefault("TO_EASTING");
                            instance.ToNorthing = records.GetInt32OrDefault("TO_NORTHING");


                            instance.PointGeometry = records.GetGeometryOrNull("POINT_GEOMETRY");
                            instance.LineGeometry = records.GetGeometryOrNull("LINE_GEOMETRY");
                            instance.Point = instance.PointGeometry.sdo_point;

                        }
                        );
            return structureInfoList;
        }

        #endregion

        #region List<StructureContact> getMyStructureContactList(long structureId, string userSchema = UserSchema.Portal)
        public static List<StructureContact> GetMyStructureContactList(long structureId, string userSchema = UserSchema.Portal)
        {
            List<StructureContact> StructureContactList = new List<StructureContact>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               StructureContactList,
               userSchema + ".GET_STRUCT_CONTACT_INFO",
               parameter =>
               {
                   parameter.AddWithValue("p_STRUCTURE_ID", structureId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
               (records, instance) =>
               {

                   instance.OwnerName = records.GetStringOrDefault("OWNER_NAME");
                   instance.OrganisationId = records.GetLongOrDefault("ORGANISATION_ID");
                   instance.OrganisationName = records.GetStringOrDefault("ORGANISATION_NAME");
                   instance.ContactId = Convert.ToInt64(records["CONTACT_ID"]);//#7197
                    instance.Position = records.GetShortOrDefault("POSITION");
                   instance.ContactName = records.GetStringOrDefault("CONTACT_NAME");
                   instance.PostCode = records.GetStringOrDefault("POSTCODE");
                   instance.AddressLine1 = records.GetStringOrDefault("ADDRESSLINE_1");
                   instance.AddressLine2 = records.GetStringOrDefault("ADDRESSLINE_2");
                   instance.AddressLine3 = records.GetStringOrDefault("ADDRESSLINE_3");
                   instance.AddressLine4 = records.GetStringOrDefault("ADDRESSLINE_4");
                   instance.FullName = records.GetStringOrDefault("FIRST_NAME");
                   instance.Telephone = records.GetStringOrDefault("PHONENUMBER");
                   instance.Fax = records.GetStringOrDefault("FAX");
                   instance.Email = records.GetStringOrDefault("EMAIL");

               }
               );
            return StructureContactList;
        }
        #endregion

        #region List<RoadContactModal> getRoadContactList(long linkID, long length, string userSchema = UserSchema.Portal)
        public static List<RoadContactModal> GetRoadContactList(long linkId, long length, string userSchema = UserSchema.Portal)
        {
            List<RoadContactModal> RoadContactList = new List<RoadContactModal>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               RoadContactList,
               userSchema + ".SP_GET_ROAD_CONTACTS",
               parameter =>
               {
                   parameter.AddWithValue("P_LINK_ID", linkId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_LENGTH", length, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
               (records, instance) =>
               {
                   instance.ContactName = records.GetStringOrDefault("FULL_NAME");
                   instance.OrganisationName = records.GetStringOrDefault("NAME");
                   instance.OrganisationType = records.GetStringOrDefault("ORGTYPE");
                   instance.AddressLine1 = records.GetStringOrDefault("ADDRESS_1");
                   instance.AddressLine2 = records.GetStringOrDefault("ADDRESS_2");
                   instance.AddressLine3 = records.GetStringOrDefault("ADDRESS_3");
                   instance.AddressLine4 = records.GetStringOrDefault("ADDRESS_4");
                   instance.PostCode = records.GetStringOrDefault("POST_CODE");
                   instance.Telephone = records.GetStringOrDefault("TELEPHONE");
                   instance.Fax = records.GetStringOrDefault("FAX");
                   instance.EMail = records.GetStringOrDefault("EMAIL");

               }
               );

            return RoadContactList;
        }
        #endregion

        public static List<StructureInfo> getAgreedAppStructureInfo(string V_STRUCT_CODE)
        {
            List<StructureInfo> structureInfoList = new List<StructureInfo>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                structureInfoList,
               UserSchema.Portal + ".STP_STRUCTURE_LIST.SP_GET_STRUCTURE_ON_MAP",
                parameter =>
                {
                    parameter.AddWithValue("P_STRUCT_CODE", V_STRUCT_CODE, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.StructureId = records.GetLongOrDefault("STRUCTURE_ID");
                    instance.StructureName = records.GetStringOrDefault("STRUCTURE_NAME");
                    instance.StructureDescr = records.GetStringOrDefault("DESCRIPTION");
                    instance.StructureClass = records.GetStringOrDefault("TYPE_NAME");
                    instance.StructureCode = records.GetStringOrDefault("STRUCTURE_CODE");
                    instance.Northing = records.GetLongOrDefault("NORTHING");
                    instance.Easting = records.GetLongOrDefault("EASTING");
                    instance.PointGeometry = records.GetGeometryOrNull("POINT_GEOMETRY");
                    instance.LineGeometry = records.GetGeometryOrNull("LINE_GEOMETRY");
                    instance.Point = instance.PointGeometry.sdo_point;
                }
                );
            return structureInfoList;
        }
        #region GetStructureOwner
        public static int GetStructureOwner(long structId, long orgId)
        {
            int recCnt = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                recCnt,
               UserSchema.Portal + ".STP_STRUCTURE_LIST.SP_GET_STRUCTURE_OWNER",
                parameter =>
                {
                    parameter.AddWithValue("S_STRUCTURE_ID", structId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("S_ORG_ID", orgId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("S_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    record =>
                    {
                        recCnt = (int)record.GetDecimalOrDefault("S_COUNT");
                    }
                );

            return recCnt;
        }
        #endregion

        #region GetStructureId
        public static int GetStructureId(string structureCode)
        {
            int structId = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                structId,
               UserSchema.Portal + ".SP_GET_STRUCTURE_ID",
                parameter =>
                {
                    parameter.AddWithValue("S_STRUCTURE_CODE", structureCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("S_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    record =>
                    {
                        structId = (int)record.GetDecimalOrDefault("S_STRUCTURE_ID");
                    }
                );

            return structId;
        }
        #endregion

    }
}