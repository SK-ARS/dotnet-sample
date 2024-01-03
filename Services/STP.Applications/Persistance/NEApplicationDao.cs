using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.DataAccess.SafeProcedure;
using STP.Domain.NonESDAL;

namespace STP.Applications.Persistance
{
    public static class NEApplicationDao
    {
        private static readonly string nePackage = ".STP_NE_GENERAL.";

        #region SaveNEApplication
        public static long SaveNEApplication(NEAppGeneralDetails generalDetails)
        {
            string neaAppProc = "SP_INSERT_NE_APPLICATION";
            long revId = 0;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                revId,
               UserSchema.Portal + nePackage + neaAppProc,
                parameter =>
                {
                    parameter.AddWithValue("P_CLIENT", generalDetails.Client, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_APP_DESC", generalDetails.ApplicationDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NOTES", generalDetails.ApplicationNotes, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                    parameter.AddWithValue("P_HAUL_REF", generalDetails.HauliersReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NOTES_ON_ESCORT", generalDetails.NotesOnEscort, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                    parameter.AddWithValue("P_AGENTNAME", generalDetails.AgentName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_NON_ESDAL_KEY", generalDetails.NonEsdalKeyId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_ORG", generalDetails.HaulierOrgName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_CONT", generalDetails.HaulierContact, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_EMAIL", generalDetails.HaulierEmail, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_TEL", generalDetails.HaulierTelephoneNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_FAX", generalDetails.HaulierFaxNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_LIC", generalDetails.HaulierLicence, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_HAUL_ADDRESS1", generalDetails.HaulierAddressLine1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_ADDRESS2", generalDetails.HaulierAddressLine2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_ADDRESS3", generalDetails.HaulierAddressLine3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_ADDRESS4", generalDetails.HaulierAddressLine4, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_ADDRESS5", generalDetails.HaulierAddressLine5, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_POSTCODE", generalDetails.HaulierPostCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_COUNTRY", generalDetails.HaulierCountry, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_LOAD_DESC ", generalDetails.LoadDescription, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NO_OF_MOV", generalDetails.TotalMoves, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NO_OF_PIECES", generalDetails.MaxPiecesPerMove, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_FROM_DESCR", generalDetails.FromSummary, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_TO_DESCR ", generalDetails.ToSummary, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MOV_START", generalDetails.MovementStart, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MOV_END", generalDetails.MovementEnd, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_VEHICLE_CLASS", generalDetails.Classification, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        
                    parameter.AddWithValue("P_DISTANCE_BY_ROAD", generalDetails.SupplimentaryInfo.TotalDistanceOfRoad, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VALUE_OF_LOAD", generalDetails.SupplimentaryInfo.ApprValueOfLoad, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIMILAR_MOVEMENT", generalDetails.SupplimentaryInfo.DateOfAuthority, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_LOAD_DIVISIBLE", generalDetails.SupplimentaryInfo.LoadDivision, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_COST_OF_ROAD_MOVE", generalDetails.SupplimentaryInfo.ApprCostOfMovement, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_COST_OF_DIVISION", generalDetails.SupplimentaryInfo.AdditionalCost, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RISK_OF_DIVISION", generalDetails.SupplimentaryInfo.RiskNature, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_OTHER_CONSIDERATIONS", generalDetails.SupplimentaryInfo.AdditionalConsideration, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_IS_FINAL_ADDRESS", generalDetails.SupplimentaryInfo.Address, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_FURTHER_MOVES", generalDetails.SupplimentaryInfo.ProposedMoveDetails, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SEA_QUOTATION", generalDetails.SupplimentaryInfo.SeaQuotation, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_BETWEEN_WHICH_PORTS", generalDetails.SupplimentaryInfo.PortNames, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SEA_CONSIDERED", generalDetails.SupplimentaryInfo.Shipment, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     revId = record.GetLongOrDefault("REVISION_ID");
                 }
            );
            return revId;
        }

        public static string SubmitNEApplication(long appRevId,bool isVr1)
        {
            string esdalRefNumber = null;
            string neaAppProc = "SP_SUBMIT_NE_SO";
            if (isVr1)
                neaAppProc = "SP_SUBMIT_NE_VR1";
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                esdalRefNumber,
               UserSchema.Portal + nePackage + neaAppProc,
                parameter =>
                {
                    parameter.AddWithValue("P_REV_ID", appRevId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     esdalRefNumber = record.GetStringOrDefault("esdal_ref_no");
                 }
            );
            return esdalRefNumber;
        }

        public static string GetNEApplicationStatus(string ESDALReferenceNumber)
        {
            string status=string.Empty;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                status,
               UserSchema.Portal + ".STP_EXPORT_DETAILS.SP_GET_NE_APP_STATUS",
                parameter =>
                {
                    parameter.AddWithValue("P_ESDAL_REF_NO", ESDALReferenceNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 (records, instance) =>
                 {
                     status = records.GetStringOrDefault("STATUS");
                 }
            );

            return status;
        }

        #endregion
    }
}