using Newtonsoft.Json;
using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using STP.Domain.RouteAssessment.AssessmentInput;
using STP.Domain.RouteAssessment.AssessmentOutput;
using Definitions = STP.Domain.RouteAssessment.AssessmentInput.Definitions;
using EsdalStructure = STP.Domain.RouteAssessment.AssessmentInput.EsdalStructure;
using Properties = STP.Domain.RouteAssessment.AssessmentInput.Properties;
using STP.Domain.Structures;
using System.Net.Http;
using System.Configuration;
using STP.ServiceAccess.RouteAssessment;

namespace STP.Alsat.Persistance
{
    public class AlsatAssessmentDAO
    {
        private readonly IRouteAssessmentService routeAssessmentService;
        public AlsatAssessmentDAO(IRouteAssessmentService routeAssessmentService)
        {
            this.routeAssessmentService = routeAssessmentService;
        }
        public AlsatAssessmentDAO()
        { }
        public static AssessmentResponse GetAssessment(int sequenceNumber)
        {
            AssessmentResponse assessmentResponse = new AssessmentResponse
            {
                AssessmentInput = null,
                ExceptionCode = ""
            };
            AssessmentInput assessmentInput = null;
            Properties assessmentProperties = null;
            TempProperties tempAssessmentProperties = new TempProperties();
            try
            {

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    tempAssessmentProperties,
                      UserSchema.Portal + ".GET_STRUCTURES_ASSESSMENT_ALSAT",
                    parameter =>
                    {
                        parameter.AddWithValue("P_SEQUENCE_NUMBER", sequenceNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.SequenceNumber = records.GetLongOrDefault("SEQUENCE_NO");
                            instance.Timestamp = records.GetDateTimeOrDefault("REQUEST_TIMESTAMP");
                            instance.MovementId = records.GetStringOrDefault("MOVEMENT_REF");
                            instance.Vehicles = records.GetByteArrayOrNull("VEHICLES");
                            instance.EsdalStructure = records.GetByteArrayOrNull("STRUCTURES");
                        }
                );
                if (tempAssessmentProperties.SequenceNumber == 0)
                {
                    assessmentResponse.ExceptionCode = "200";

                    return assessmentResponse;
                }

                byte[] decompressedVehicleByte = Decompress(tempAssessmentProperties.Vehicles);
                string vehicleJsonString = Encoding.UTF8.GetString(decompressedVehicleByte);
                TempVehicles tempVehicles = JsonConvert.DeserializeObject<TempVehicles>(vehicleJsonString);
                Vehicles vehicles = tempVehicles.Vehicles;

                byte[] decompressedStructureByte = Decompress(tempAssessmentProperties.EsdalStructure);
                string structureJsonString = Encoding.UTF8.GetString(decompressedStructureByte);
                TempStructures tempStructures = JsonConvert.DeserializeObject<TempStructures>(structureJsonString);
                List<EsdalStructure> structures = tempStructures.EsdalStructure;

                assessmentProperties = new Properties
                {
                    SequenceNumber = tempAssessmentProperties.SequenceNumber,
                    Timestamp = tempAssessmentProperties.Timestamp,
                    MovementId = tempAssessmentProperties.MovementId,
                    Vehicles = vehicles,
                    EsdalStructure = structures
                };

                assessmentInput = new AssessmentInput
                {
                    Id = new Uri("https://esdalurl/schemas/assessment.json"),
                    Title = "assessment",
                    Type = "object",
                    Definitions = new Definitions(),
                    Schema = new Uri("http://json-schema.org/draft-07/schema#"),
                    Properties = assessmentProperties
                };

                assessmentResponse.AssessmentInput = assessmentInput;
                assessmentResponse.ExceptionCode = "200";

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Request for sequence number {0} processed successfully.", sequenceNumber));
            }
            catch (Exception e)
            {
                switch (e.Message)
                {
                    case "Object reference not set to an instance of an object.":
                        assessmentResponse.ExceptionCode = "404";
                        break;
                    default:
                        assessmentResponse.ExceptionCode = "500";
                        break;
                }

                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/GetAssessment, Sequence number : {0} - Exception: {1}", sequenceNumber, e));
            }

            return assessmentResponse;
        }
        public static StructuresAssessment PutAssessmentResult(AssessmentOutput assessmentOutput)
        {
            long sequenceNumber = 0;
            sequenceNumber = assessmentOutput.Properties.SequenceNumber;

            StructuresAssessment objStructuresAssessment = new StructuresAssessment();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               objStructuresAssessment,
                 UserSchema.Portal + ".SELECT_STRUCTURE_ASSESSMENT_ALSAT",
               parameter =>
               {
                   parameter.AddWithValue("P_SEQUENCE_NUMBER", sequenceNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                   (records, instance) =>
                   {
                       instance.SequenceNumber = records.GetLongOrDefault("SEQUENCE_NO");
                       instance.MovementReference = records.GetStringOrDefault("MOVEMENT_REF");
                       instance.RoutePartId = records.GetLongOrDefault("ROUTE_PART_ID");
                       instance.RequestTimestamp = records.GetDateTimeOrDefault("REQUEST_TIMESTAMP");
                       instance.AnalysisId = records.GetLongOrDefault("ANALYSIS_ID");
                       instance.PortalSchema = records.GetInt16Nullable("PORTAL_SCHEMA");
                   }
           );

            return objStructuresAssessment;
        }

        public static byte[] Decompress(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }
        
        public int CallUpdateRouteAssessment(string contentRefNo, int orgId, int analysisId, int analType, string userSchema, int routeId, AssessmentOutput assessmentResult)
        {
            int status = routeAssessmentService.UpdateRouteAssessment(contentRefNo, orgId, analysisId, analType, userSchema, routeId, assessmentResult);
            return status;
        }
    }
}