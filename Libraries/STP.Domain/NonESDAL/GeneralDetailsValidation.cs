using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.Domain.ExternalAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using static STP.Common.Enums.ExternalApiEnums;

namespace STP.Domain.NonESDAL
{
    #region GeneralDetailsValidation
    public class GeneralDetailsValidation
    {
        public GeneralDetailsValidation()
        {
            ValidationFailure = LoadValidationFailure();
        }

        #region Validation Failure Message
        public Dictionary<string, string> ValidationFailure { get; set; }
        public Dictionary<string, string> LoadValidationFailure()
        {
            string validationFile = ConfigurationManager.AppSettings["GeneralValidationFailureFile"];
            string path = ConfigurationManager.AppSettings["ValidationFailurePath"] + validationFile;
            if (ConfigurationManager.AppSettings["Envrironment"] == "Debug")
                path = AppDomain.CurrentDomain.RelativeSearchPath + validationFile;
            var vehicleValidationFailure = System.IO.File.ReadAllText(path);
            Dictionary<string, string> keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(vehicleValidationFailure);
            return keyValuePairs;
        }
        #endregion

        #region Application General Details Validation 
        public NEAppGeneralDetails ValidateAppGeneralDetail(GeneralDetail generalDetail)
        {
            NEAppGeneralDetails appGeneralDetail = new NEAppGeneralDetails();
            int errorCount = 0;
            string errorMsg = ValidationFailure["GeneralApplicationError"];
            StringBuilder validationErrorBuilder = new StringBuilder();
            validationErrorBuilder.Append(errorMsg);

            #region Vehicle Classification
            if (string.IsNullOrWhiteSpace(generalDetail.Classification))
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["VehicleClassRequired"]);
            }
            else
            {
                try
                {
                    appGeneralDetail.Classification = (int)EnumExtensionMethods.GetValueFromDescription<ExternalApiGeneralClassificationType>(generalDetail.Classification.ToLower().Trim());
                }
                catch (Exception ex)
                {
                    appGeneralDetail.Classification = 0;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["InvalidVehicleClass"]);
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateAppGeneralDetail : Invalid Calssification Input:{ generalDetail.Classification} Exception: { ex.Message } StackTrace { ex.StackTrace}");
                }
                if (appGeneralDetail.Classification > 0 && appGeneralDetail.Classification != (int)ExternalApiGeneralClassificationType.GC002 && appGeneralDetail.Classification != (int)ExternalApiGeneralClassificationType.GC002)
                    appGeneralDetail.IsVR1 = true;
            }
            #endregion

            #region Haulier Details
            if (generalDetail.HaulierDetails != null)
            {
                var haulierDetails = generalDetail.HaulierDetails;

                #region Organsiation Name
                if (string.IsNullOrWhiteSpace(haulierDetails.HaulierOrgName))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["OrganisationName"]);
                }
                else
                    appGeneralDetail.HaulierOrgName = haulierDetails.HaulierOrgName.Trim();
                #endregion

                #region Haulier Address
                if (haulierDetails.HaulierAddress != null)
                {
                    var address = haulierDetails.HaulierAddress;

                    #region AddressLine1
                    if (string.IsNullOrWhiteSpace(address.AddressLine1))
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierAddress"]);
                    }
                    else if (address.AddressLine1.Trim().Length > 100)
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierAddress1Length"]);
                    }
                    else
                        appGeneralDetail.HaulierAddressLine1 = address.AddressLine1.Trim();
                    #endregion

                    #region AddressLine2
                    if (!string.IsNullOrWhiteSpace(address.AddressLine2) && address.AddressLine2.Trim().Length > 100)
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierAddress2Length"]);
                    }
                    else
                        appGeneralDetail.HaulierAddressLine2 = !string.IsNullOrWhiteSpace(address.AddressLine2) ? address.AddressLine2.Trim() : address.AddressLine2;
                    #endregion

                    #region AddressLine3
                    if (!string.IsNullOrWhiteSpace(address.AddressLine3) && address.AddressLine3.Trim().Length > 100)
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierAddress3Length"]);
                    }
                    else
                        appGeneralDetail.HaulierAddressLine3 = !string.IsNullOrWhiteSpace(address.AddressLine3) ? address.AddressLine3.Trim() : address.AddressLine3; 
                    #endregion

                    #region AddressLine4
                    if (!string.IsNullOrWhiteSpace(address.AddressLine4) && address.AddressLine4.Trim().Length > 100)
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierAddress4Length"]);
                    }
                    else
                        appGeneralDetail.HaulierAddressLine4 = !string.IsNullOrWhiteSpace(address.AddressLine4) ? address.AddressLine4.Trim() : address.AddressLine4; 
                    #endregion

                    #region AddressLine5
                    if (!string.IsNullOrWhiteSpace(address.AddressLine5) && address.AddressLine5.Trim().Length > 100)
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierAddress5Length"]);
                    }
                    else
                        appGeneralDetail.HaulierAddressLine5 = !string.IsNullOrWhiteSpace(address.AddressLine5) ? address.AddressLine5.Trim() : address.AddressLine5; 
                    #endregion

                    #region Post Code
                    if (string.IsNullOrWhiteSpace(address.PostCode))
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierPostCode"]);
                    }
                    else if (address.PostCode.Trim().Length > 20)
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierPostCodeSize"]);
                    }
                    else
                        appGeneralDetail.HaulierPostCode = address.PostCode.Trim();
                    #endregion

                    #region Country
                    if (string.IsNullOrWhiteSpace(address.Country))
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierCountry"]);
                    }
                    else
                    {
                        appGeneralDetail.HaulierCountry = Common.General.CountryDetails.GetCountryId(address.Country.Trim());
                        if (appGeneralDetail.HaulierCountry == 0)
                        {
                            errorCount++;
                            validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierCountryFormat"]);
                        }
                    }
                    #endregion
                }
                else
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierAddressMandate"]);
                }
                #endregion

                #region Contact Name
                if (string.IsNullOrWhiteSpace(haulierDetails.HaulierContact))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierContactName"]);
                }
                else if (haulierDetails.HaulierContact.Trim().Length > 100)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierContactNameSize"]);
                }
                else
                    appGeneralDetail.HaulierContact = haulierDetails.HaulierContact.Trim();
                #endregion

                #region Email
                if (string.IsNullOrWhiteSpace(haulierDetails.Email))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierEmail"]);
                }
                else if (haulierDetails.Email.Trim().Length > 255)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierEmailSize"]);
                }
                else if (!CheckIsMailValid(haulierDetails.Email.Trim()))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierEmailIncorrect"]);
                }
                else
                    appGeneralDetail.HaulierEmail = haulierDetails.Email.Trim();
                #endregion

                #region Phone Nuber
                if (string.IsNullOrWhiteSpace(haulierDetails.TelephoneNumber))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierTelephone"]);
                }
                else if (haulierDetails.TelephoneNumber.Trim().Length > 100)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierPhoneSize"]);
                }
                else if (!IsNumber(haulierDetails.TelephoneNumber.Trim()))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierPhoneNumber"]);
                }
                else
                    appGeneralDetail.HaulierTelephoneNumber = haulierDetails.TelephoneNumber.Trim();
                #endregion

                #region Fax Nuber
                if (string.IsNullOrWhiteSpace(haulierDetails.FaxNumber))
                    appGeneralDetail.HaulierFaxNumber = string.Empty;
                else if (haulierDetails.FaxNumber.Trim().Length > 100)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierFaxSize"]);
                }
                else if (!IsNumber(haulierDetails.FaxNumber.Trim()))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierFaxNumber"]);
                }
                else
                    appGeneralDetail.HaulierFaxNumber = haulierDetails.FaxNumber.Trim();
                #endregion

                #region Licence
                if (string.IsNullOrWhiteSpace(haulierDetails.Licence))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierLicence"]);
                }
                else if (haulierDetails.Licence.Trim().Length > 20)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierLicenceSize"]);
                }
                else
                    appGeneralDetail.HaulierLicence = haulierDetails.Licence;
                #endregion
            }
            else
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierDetailsMandate"]);
            }
            #endregion

            #region Load Details
            if (generalDetail.LoadDetails != null)
            {
                var loadDetails = generalDetail.LoadDetails;

                #region Load Description
                if (string.IsNullOrWhiteSpace(loadDetails.Description))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["LoadDescriptionSummary"]);
                }
                else if (loadDetails.Description.Trim().Length > 1000)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["LoadDescriptionSize"]);
                }
                else
                    appGeneralDetail.LoadDescription = loadDetails.Description.Trim();
                #endregion

                #region Total Moves
                if (loadDetails.TotalMoves > 0 && Math.Floor(Math.Log10(loadDetails.TotalMoves)) + 1 > 3)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["TotalMovesSize"]);
                }
                else
                    appGeneralDetail.TotalMoves = loadDetails.TotalMoves > 0 ? Convert.ToInt32(loadDetails.TotalMoves) : 1;
                #endregion

                #region Max pieces per move
                if (loadDetails.MaxPiecesPerMove > 0 && Math.Floor(Math.Log10(loadDetails.MaxPiecesPerMove)) + 1 > 3)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["MaxPiecesPerMoveSize"]);
                }
                else
                    appGeneralDetail.MaxPiecesPerMove = loadDetails.MaxPiecesPerMove > 0 ? Convert.ToInt32(loadDetails.MaxPiecesPerMove) : 1;
                #endregion
            }
            else
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["LoadDetailsMandate"]);
            }
            #endregion

            #region MovementDetails
            if (generalDetail.MovementDetails != null)
            {
                var movementDetails = generalDetail.MovementDetails;

                #region From Summary
                if (string.IsNullOrWhiteSpace(movementDetails.FromSummary))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["FromSummary"]);
                }
                else if (movementDetails.FromSummary.Trim().Length > 255)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["FromSummarySize"]);
                }
                else
                    appGeneralDetail.FromSummary = movementDetails.FromSummary.Trim();
                #endregion

                #region To Summary
                if (string.IsNullOrWhiteSpace(movementDetails.ToSummary))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["ToSummary"]);
                }
                else if (movementDetails.ToSummary.Trim().Length > 255)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["ToSummarySize"]);
                }
                else
                    appGeneralDetail.ToSummary = movementDetails.ToSummary.Trim();
                #endregion

                bool isValidStartDate;
                bool isValidStartTime;
                bool isValidEndDate;
                bool isValidEndTime;

                #region Start Date
                if (string.IsNullOrWhiteSpace(movementDetails.StartDate))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["MovementStartDate"]);
                }
                else
                {
                    isValidStartDate = IsValidDate(movementDetails.StartDate.Trim(), "dd/MM/yyyy");
                    if (!isValidStartDate)
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["MovementStartDateFormat"]);
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateNotifGeneralDetail : Invalid Start Input:{ movementDetails.StartDate}");
                    }

                    isValidStartTime = IsValidDate(movementDetails.StartTime.Trim(), "HH:mm");
                    if (!isValidStartTime)
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["MovementStartTimeFormat"]);
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateNotifGeneralDetail : Invalid Start Input:{ movementDetails.StartTime}");
                    }

                    if (isValidStartDate && isValidStartTime)
                    {
                        string moveStart = movementDetails.StartDate.Trim() + " " + movementDetails.StartTime.Trim();
                        try
                        {
                            appGeneralDetail.MovementStart = DateTime.ParseExact(moveStart, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            validationErrorBuilder.Append(errorCount + ValidationFailure["MovementStartDateFormat"]);
                            Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateAppGeneralDetail : Invalid Start Date Input:{ moveStart} Exception: { ex.Message } StackTrace { ex.StackTrace}");
                        }
                    }
                    else if (isValidStartDate && !isValidStartTime)
                        appGeneralDetail.MovementStart = DateTime.ParseExact(movementDetails.StartDate.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                #endregion

                #region End Date
                if (string.IsNullOrWhiteSpace(movementDetails.EndDate))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["MovementEndDate"]);
                }
                else
                {
                    isValidEndDate = IsValidDate(movementDetails.EndDate.Trim(), "dd/MM/yyyy");
                    if (!isValidEndDate)
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["MovementEndDateFormat"]);
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateNotifGeneralDetail : Invalid End Input:{ movementDetails.EndDate}");
                    }

                    isValidEndTime = IsValidDate(movementDetails.EndTime.Trim(), "HH:mm");
                    if (!isValidEndTime)
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["MovementEndTimeFormat"]);
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateNotifGeneralDetail : Invalid End Input:{ movementDetails.EndTime}");
                    }

                    if (isValidEndDate && isValidEndTime)
                    {
                        string moveEnd = movementDetails.EndDate.Trim() + " " + movementDetails.EndTime.Trim();
                        try
                        {
                            appGeneralDetail.MovementEnd = DateTime.ParseExact(moveEnd, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            validationErrorBuilder.Append(errorCount + ValidationFailure["MovementEndDateFormat"]);
                            Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateAppGeneralDetail : Invalid End Date Input:{ moveEnd} Exception: { ex.Message } StackTrace { ex.StackTrace}");
                        }
                    }
                    else if (isValidEndDate && !isValidEndTime)
                        appGeneralDetail.MovementStart = DateTime.ParseExact(movementDetails.StartDate.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                #endregion

                #region Date Validation
                if (appGeneralDetail.MovementStart < DateTime.Now)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["InvalidMovementStart"]);
                }
                if (appGeneralDetail.MovementEnd < appGeneralDetail.MovementStart || appGeneralDetail.MovementEnd < DateTime.Now)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["InvalidMovementEnd"]);
                }
                #endregion
            }
            else
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["MovementDetailsMandate"]);
            }
            #endregion

            #region Supplimentary Details validation
            if (generalDetail.SupplimentaryDetails == null)
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["SupplimentaryDetails"]);
            }
            else
            {
                var isValidSupplimentaryDetails = true;
                var supDet = generalDetail.SupplimentaryDetails;
                if (supDet.DistanceOfRoad == 0)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["TotalDistance"]);
                    isValidSupplimentaryDetails = false;
                }
                if (supDet.DistanceOfRoad > 0 && supDet.DistanceOfRoad.ToString().Trim().Length > 80)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["InvalidDistanceOfRoadSize"]);
                    isValidSupplimentaryDetails = false;
                }

                if (!string.IsNullOrWhiteSpace(supDet.ValueOfLoad) && supDet.ValueOfLoad.ToString().Trim().Length > 80)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["InvalidValueOfLoadSize"]);
                    isValidSupplimentaryDetails = false;
                }
                if (!string.IsNullOrWhiteSpace(supDet.DateOfAuthority) && supDet.DateOfAuthority.ToString().Trim().Length > 200)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["InvalidDateOfAuthoritySize"]);
                    isValidSupplimentaryDetails = false;
                }
                if (!string.IsNullOrWhiteSpace(supDet.AdditionalCost) && supDet.AdditionalCost.ToString().Trim().Length > 80)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["InvalidAdditionalCostSize"]);
                    isValidSupplimentaryDetails = false;
                }
                if (!string.IsNullOrWhiteSpace(supDet.RiskNature) && supDet.RiskNature.ToString().Trim().Length > 200)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["InvalidRiskNatureSize"]);
                    isValidSupplimentaryDetails = false;
                }
                if (!string.IsNullOrWhiteSpace(supDet.PortNames) && supDet.PortNames.ToString().Trim().Length > 80)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["InvalidPortNamesSize"]);
                    isValidSupplimentaryDetails = false;
                }
                if (!string.IsNullOrWhiteSpace(supDet.SeaQuotation) && supDet.SeaQuotation.ToString().Trim().Length > 200)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["InvalidSeaQuotationSize"]);
                    isValidSupplimentaryDetails = false;
                }
                if (!string.IsNullOrWhiteSpace(supDet.ProposedMovementDetails) && supDet.ProposedMovementDetails.ToString().Trim().Length > 200)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["InvalidProposedMovementDetailsSize"]);
                    isValidSupplimentaryDetails = false;
                }
                if (!string.IsNullOrWhiteSpace(supDet.CostOfMovement) && supDet.CostOfMovement.ToString().Trim().Length > 80)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["InvalidCostOfMovementSize"]);
                    isValidSupplimentaryDetails = false;
                }
                if (!string.IsNullOrWhiteSpace(supDet.AdditionalConsideration) && supDet.AdditionalConsideration.ToString().Trim().Length > 800)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["InvalidAdditionalConsiderationSize"]);
                    isValidSupplimentaryDetails = false;
                }

                if (isValidSupplimentaryDetails)
                {
                    appGeneralDetail.SupplimentaryInfo = new Applications.SupplimentaryInfo
                    {
                        TotalDistanceOfRoad = supDet.DistanceOfRoad.ToString(),
                        ApprValueOfLoad = supDet.ValueOfLoad,
                        DateOfAuthority = supDet.DateOfAuthority,
                        AdditionalCost = supDet.AdditionalCost,
                        RiskNature = supDet.RiskNature,
                        PortNames = supDet.PortNames,
                        SeaQuotation = supDet.SeaQuotation,
                        ProposedMoveDetails = supDet.ProposedMovementDetails,
                        ApprCostOfMovement = supDet.CostOfMovement,
                        AdditionalConsideration = supDet.AdditionalConsideration,
                        LoadDivision = string.IsNullOrWhiteSpace(supDet.AdditionalCost) && string.IsNullOrWhiteSpace(supDet.RiskNature) ? (int)ExternalApiBitSystem.No : (int)ExternalApiBitSystem.Yes,
                        Shipment = string.IsNullOrWhiteSpace(supDet.PortNames) && string.IsNullOrWhiteSpace(supDet.SeaQuotation) ? (int)ExternalApiBitSystem.No : (int)ExternalApiBitSystem.Yes,
                        Address = string.IsNullOrWhiteSpace(supDet.ProposedMovementDetails) ? (int)ExternalApiBitSystem.No : (int)ExternalApiBitSystem.Yes
                    };
                }
            }
            #endregion

            #region Notes on Escort
            if (!string.IsNullOrWhiteSpace(generalDetail.NotesOnEscort) && generalDetail.NotesOnEscort.Trim().Length > 100)
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["NotesOnEscortSize"]);
            }
            else
                appGeneralDetail.NotesOnEscort = !string.IsNullOrWhiteSpace(generalDetail.NotesOnEscort) ? generalDetail.NotesOnEscort.Trim() : string.Empty;
            #endregion

            #region Application Notes
            if (!string.IsNullOrWhiteSpace(generalDetail.Notes) && generalDetail.Notes.Trim().Length > 4000)
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["NotesSize"]);
            }
            else
                appGeneralDetail.ApplicationNotes = !string.IsNullOrWhiteSpace(generalDetail.Notes) ? generalDetail.Notes.Trim() : string.Empty;
            #endregion

            #region Haulier Reference
            if (!string.IsNullOrWhiteSpace(generalDetail.HaulierReference) && generalDetail.HaulierReference.Trim().Length > 35)
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierReferenceSize"]);
            }
            else
                appGeneralDetail.HauliersReference = !string.IsNullOrWhiteSpace(generalDetail.HaulierReference) ? generalDetail.HaulierReference.Trim() : string.Empty;
            #endregion

            #region Application Desc
            if (!string.IsNullOrWhiteSpace(generalDetail.ApplicationDesc) && generalDetail.ApplicationDesc.Trim().Length > 255)
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["AppDescSize"]);
            }
            else
                appGeneralDetail.ApplicationDesc = !string.IsNullOrWhiteSpace(generalDetail.ApplicationDesc) ? generalDetail.ApplicationDesc.Trim() : string.Empty;
            #endregion

            #region Agent Name
            if (!string.IsNullOrWhiteSpace(generalDetail.AgentName) && generalDetail.AgentName.Trim().Length > 70)
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["AgentNameSize"]);
            }
            else
                appGeneralDetail.AgentName = !string.IsNullOrWhiteSpace(generalDetail.AgentName) ? generalDetail.AgentName.Trim() : string.Empty;
            #endregion

            #region Client
            if (!string.IsNullOrWhiteSpace(generalDetail.Client) && generalDetail.Client.Trim().Length > 70)
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["ClientSize"]);
            }
            else
                appGeneralDetail.Client = !string.IsNullOrWhiteSpace(generalDetail.Client) ? generalDetail.Client.Trim() : string.Empty;
            #endregion

            #region Error Count
            if (errorCount > 0)
            {
                string errorMessage = validationErrorBuilder.ToString().Trim();
                List<string> generalDetailsErrorList = errorMessage.Split('~').ToList();
                generalDetailsErrorList.RemoveAt(generalDetailsErrorList.Count - 1);
                ValidationError validationError = new ValidationError
                {
                    ErrorCount = errorCount,
                    ErrorMessage = errorMessage,
                    ErrorList = generalDetailsErrorList
                };
                appGeneralDetail.GeneralDetailError = validationError;
            }
            #endregion

            return appGeneralDetail;
        }

        #endregion

        #region Notification General Details Validation
        public NENotifGeneralDetails ValidateNotifGeneralDetail(GeneralDetail generalDetail, ValidNERenotif validNERenotif)
        {
            NENotifGeneralDetails notifGeneralDetail = new NENotifGeneralDetails();
            int errorCount = 0;
            string errorMsg = ValidationFailure["GeneralNotificationError"];
            StringBuilder validationErrorBuilder = new StringBuilder();
            validationErrorBuilder.Append(errorMsg);

            #region Vehicle Classification
            if (string.IsNullOrWhiteSpace(generalDetail.Classification))
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["GeneralClassRequired"]);
            }
            else
            {
                try
                {
                    notifGeneralDetail.Classification = (int)EnumExtensionMethods.GetValueFromDescription<ExternalApiGeneralClassificationType>(generalDetail.Classification.ToLower().Trim());
                }
                catch (Exception ex)
                {
                    notifGeneralDetail.Classification = 0;
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["InvalidGeneralClass"]);
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateNotifGeneralDetail : Invalid Calssification Input:{ generalDetail.Classification} Exception: { ex.Message } StackTrace { ex.StackTrace}");
                }
            }
            #endregion

            #region Application Reference
            if (string.IsNullOrWhiteSpace(generalDetail.ApplicationReference) && notifGeneralDetail.Classification == (int)ExternalApiGeneralClassificationType.GC001)
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["AppReferenceVSO"]);
            }
            else if (!string.IsNullOrWhiteSpace(generalDetail.ApplicationReference) && generalDetail.ApplicationReference.Trim().Length > 20)
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["AppRefernceMaxSize"]);
            }
            else
                notifGeneralDetail.ApplicationReference = !string.IsNullOrWhiteSpace(generalDetail.ApplicationReference) ? generalDetail.ApplicationReference.Trim() : string.Empty;
            #endregion

            #region Haulier Details
            if (generalDetail.HaulierDetails != null)
            {
                var haulierDetails = generalDetail.HaulierDetails;

                #region Organsiation Name
                if (string.IsNullOrWhiteSpace(haulierDetails.HaulierOrgName))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["OrganisationName"]);
                }
                else
                    notifGeneralDetail.HaulierOrgName = haulierDetails.HaulierOrgName.Trim();
                #endregion

                #region Haulier Address 
                if (haulierDetails.HaulierAddress != null)
                {
                    var address = haulierDetails.HaulierAddress;
                    StringBuilder addressBuilder = new StringBuilder();

                    #region AddressLine1
                    if (string.IsNullOrWhiteSpace(address.AddressLine1))
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierAddress"]);
                    }
                    else if (address.AddressLine1.Trim().Length > 100)
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierAddress1Length"]);
                    }
                    else
                        addressBuilder.Append(address.AddressLine1.Trim());
                    #endregion

                    #region AddressLine2
                    if (!string.IsNullOrWhiteSpace(address.AddressLine2) && address.AddressLine2.Trim().Length > 100)
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierAddress2Length"]);
                    }
                    else
                        addressBuilder.Append(";" + (!string.IsNullOrWhiteSpace(address.AddressLine2) ? address.AddressLine2.Trim() : string.Empty));
                    #endregion

                    #region AddressLine3
                    if (!string.IsNullOrWhiteSpace(address.AddressLine3) && address.AddressLine3.Trim().Length > 100)
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierAddress3Length"]);
                    }
                    else
                        addressBuilder.Append(";" + (!string.IsNullOrWhiteSpace(address.AddressLine3) ? address.AddressLine3.Trim() : string.Empty));
                    #endregion

                    #region AddressLine4
                    if (!string.IsNullOrWhiteSpace(address.AddressLine4) && address.AddressLine4.Trim().Length > 100)
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierAddress4Length"]);
                    }
                    else
                        addressBuilder.Append(";" + (!string.IsNullOrWhiteSpace(address.AddressLine4) ? address.AddressLine4.Trim() : string.Empty));
                    #endregion

                    #region AddressLine5
                    if (!string.IsNullOrWhiteSpace(address.AddressLine5) && address.AddressLine5.Trim().Length > 100)
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierAddress5Length"]);
                    }
                    else
                        addressBuilder.Append(";" + (!string.IsNullOrWhiteSpace(address.AddressLine5) ? address.AddressLine5.Trim() : string.Empty));
                    #endregion

                    notifGeneralDetail.HaulierAddressLine = addressBuilder.ToString().Trim();

                    #region Post Code
                    if (string.IsNullOrWhiteSpace(address.PostCode))
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierPostCode"]);
                    }
                    else if (address.PostCode.Trim().Length > 20)
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierPostCodeSize"]);
                    }
                    else
                        notifGeneralDetail.HaulierPostCode = address.PostCode.Trim();
                    #endregion

                    #region Country
                    if (string.IsNullOrWhiteSpace(address.Country))
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierCountry"]);
                    }
                    else
                    {
                        notifGeneralDetail.HaulierCountry = Common.General.CountryDetails.GetCountryId(address.Country.Trim());
                        if (notifGeneralDetail.HaulierCountry == 0)
                        {
                            errorCount++;
                            validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierCountryFormat"]);
                        }
                    }
                    #endregion
                }
                else
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierAddressMandate"]);
                }
                #endregion

                #region Contact Name
                if (string.IsNullOrWhiteSpace(haulierDetails.HaulierContact))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierContactName"]);
                }
                else if (haulierDetails.HaulierContact.Trim().Length > 100)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierContactNameSize"]);
                }
                else
                    notifGeneralDetail.HaulierContact = haulierDetails.HaulierContact.Trim();
                #endregion

                #region Email
                if (string.IsNullOrWhiteSpace(haulierDetails.Email))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierEmail"]);
                }
                else if (haulierDetails.Email.Trim().Length > 255)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierEmailSize"]);
                }
                else if (!CheckIsMailValid(haulierDetails.Email.Trim()))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierEmailIncorrect"]);
                }
                else
                    notifGeneralDetail.HaulierEmail = haulierDetails.Email.Trim();
                #endregion

                #region Phone Nuber
                if (string.IsNullOrWhiteSpace(haulierDetails.TelephoneNumber))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierTelephone"]);
                }
                else if (haulierDetails.TelephoneNumber.Trim().Length > 100)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierPhoneSize"]);
                }
                else if (!IsNumber(haulierDetails.TelephoneNumber.Trim()))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierPhoneNumber"]);
                }
                else
                    notifGeneralDetail.HaulierTelephoneNumber = haulierDetails.TelephoneNumber.Trim();
                #endregion

                #region Fax Nuber
                if (string.IsNullOrWhiteSpace(haulierDetails.FaxNumber))
                    notifGeneralDetail.HaulierFaxNumber = haulierDetails.FaxNumber;
                else if (haulierDetails.FaxNumber.Trim().Length > 100)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierFaxSize"]);
                }
                else if (!IsNumber(haulierDetails.FaxNumber.Trim()))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierFaxNumber"]);
                }
                else
                    notifGeneralDetail.HaulierFaxNumber = haulierDetails.FaxNumber.Trim();
                #endregion

                #region Licence
                if (string.IsNullOrWhiteSpace(haulierDetails.Licence))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierLicence"]);
                }
                else if (haulierDetails.Licence.Trim().Length > 20)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierLicenceSize"]);
                }
                else
                    notifGeneralDetail.HaulierLicence = haulierDetails.Licence.Trim();
                #endregion
            }
            else
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierDetailsMandate"]);
            }
            #endregion

            #region Load Details
            if (generalDetail.LoadDetails != null)
            {
                var loadDetails = generalDetail.LoadDetails;

                #region Total Moves
                if (loadDetails.TotalMoves > 0 && Math.Floor(Math.Log10(loadDetails.TotalMoves)) + 1 > 3)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["TotalMovesSize"]);
                }
                else
                    notifGeneralDetail.TotalMoves = loadDetails.TotalMoves > 0 ? Convert.ToInt32(loadDetails.TotalMoves) : 1;
                #endregion

                #region Max pieces per move
                if (loadDetails.MaxPiecesPerMove > 0 && Math.Floor(Math.Log10(loadDetails.MaxPiecesPerMove)) + 1 > 3)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["MaxPiecesPerMoveSize"]);
                }
                else
                    notifGeneralDetail.MaxPiecesPerMove = loadDetails.MaxPiecesPerMove > 0 ? Convert.ToInt32(loadDetails.MaxPiecesPerMove) : 1;
                #endregion

                #region Load Description
                if (string.IsNullOrWhiteSpace(loadDetails.Description))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["LoadDescriptionSummary"]);
                }
                else if (loadDetails.Description.Trim().Length > 1000)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["LoadDescriptionSize"]);
                }
                else
                    notifGeneralDetail.LoadDescription = loadDetails.Description.Trim();
                #endregion
            }
            else
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["LoadDetailsMandate"]);
            }
            #endregion

            #region Movement Details
            if (generalDetail.MovementDetails != null)
            {
                var movementDetails = generalDetail.MovementDetails;

                #region From Summary
                if (string.IsNullOrWhiteSpace(movementDetails.FromSummary))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["FromSummary"]);
                }
                else if (movementDetails.FromSummary.Trim().Length > 255)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["FromSummarySize"]);
                }
                else
                    notifGeneralDetail.FromSummary = movementDetails.FromSummary.Trim();
                #endregion

                #region To Summary
                if (string.IsNullOrWhiteSpace(movementDetails.ToSummary))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["ToSummary"]);
                }
                else if (movementDetails.ToSummary.Trim().Length > 255)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["ToSummarySize"]);
                }
                else
                    notifGeneralDetail.ToSummary = movementDetails.ToSummary.Trim();
                #endregion

                bool isValidStartDate;
                bool isValidStartTime;
                bool isValidEndDate;
                bool isValidEndTime;

                #region Start Date
                if (string.IsNullOrWhiteSpace(movementDetails.StartDate))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["MovementStartDate"]);
                }
                else
                {
                    isValidStartDate = IsValidDate(movementDetails.StartDate.Trim(), "dd/MM/yyyy");
                    if (!isValidStartDate)
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["MovementStartDateFormat"]);
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateNotifGeneralDetail : Invalid Start Input:{ movementDetails.StartDate}");
                    }

                    isValidStartTime = IsValidDate(movementDetails.StartTime.Trim(), "HH:mm");
                    if (!isValidStartTime)
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["MovementStartTimeFormat"]);
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateNotifGeneralDetail : Invalid Start Input:{ movementDetails.StartTime}");
                    }

                    if (isValidStartDate && isValidStartTime)
                    {
                        string moveStart = movementDetails.StartDate.Trim() + " " + movementDetails.StartTime.Trim();
                        try
                        {
                            notifGeneralDetail.MovementStart = DateTime.ParseExact(moveStart, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            validationErrorBuilder.Append(errorCount + ValidationFailure["MovementStartDateFormat"]);
                            Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateNotifGeneralDetail : Invalid Start Input:{ moveStart} Exception: { ex.Message } StackTrace { ex.StackTrace}");
                        }
                    }
                    else if (isValidStartDate && !isValidStartTime)
                        notifGeneralDetail.MovementStart = DateTime.ParseExact(movementDetails.StartDate.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                #endregion

                #region End Date
                if (string.IsNullOrWhiteSpace(movementDetails.EndDate))
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["MovementEndDate"]);
                }
                else
                {
                    isValidEndDate = IsValidDate(movementDetails.EndDate.Trim(), "dd/MM/yyyy");
                    if (!isValidEndDate)
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["MovementEndDateFormat"]);
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateNotifGeneralDetail : Invalid End Input:{ movementDetails.EndDate}");
                    }

                    isValidEndTime = IsValidDate(movementDetails.EndTime.Trim(), "HH:mm");
                    if (!isValidEndTime)
                    {
                        errorCount++;
                        validationErrorBuilder.Append(errorCount + ValidationFailure["MovementEndTimeFormat"]);
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateNotifGeneralDetail : Invalid End Input:{ movementDetails.EndTime}");
                    }

                    if (isValidEndDate && isValidEndTime)
                    {
                        string moveEnd = movementDetails.EndDate.Trim() + " " + movementDetails.EndTime.Trim();
                        try
                        {
                            notifGeneralDetail.MovementEnd = DateTime.ParseExact(moveEnd, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            validationErrorBuilder.Append(errorCount + ValidationFailure["MovementEndDateFormat"]);
                            Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateNotifGeneralDetail : Invalid End Input:{ moveEnd} Exception: { ex.Message } StackTrace { ex.StackTrace}");
                        }
                    }
                    else if (isValidEndDate && !isValidEndTime)
                        notifGeneralDetail.MovementEnd = DateTime.ParseExact(movementDetails.EndDate.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                #endregion

                #region Date Validation
                if (notifGeneralDetail.MovementStart < DateTime.Now)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["InvalidMovementStart"]);
                }
                if (notifGeneralDetail.MovementEnd < notifGeneralDetail.MovementStart || notifGeneralDetail.MovementEnd < DateTime.Now)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["InvalidMovementEnd"]);
                }
                #endregion
            }
            else
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["MovementDetailsMandate"]);
            }
            #endregion

            #region Notes on Escort
            if (!string.IsNullOrWhiteSpace(generalDetail.NotesOnEscort) && generalDetail.NotesOnEscort.Trim().Length > 100)
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["NotesOnEscortSize"]);
            }
            else
                notifGeneralDetail.NotesOnEscort = !string.IsNullOrWhiteSpace(generalDetail.NotesOnEscort) ? generalDetail.NotesOnEscort.Trim() : string.Empty;
            #endregion

            #region Notes
            if (!string.IsNullOrWhiteSpace(generalDetail.Notes) && generalDetail.Notes.Trim().Length > 32767)
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["NotesSize"]);
            }
            else
                notifGeneralDetail.Notes = !string.IsNullOrWhiteSpace(generalDetail.Notes) ? generalDetail.Notes.Trim() : string.Empty;
            #endregion

            #region Haulier Reference
            if (!string.IsNullOrWhiteSpace(generalDetail.HaulierReference) && generalDetail.HaulierReference.Trim().Length > 35)
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["HaulierReferenceSize"]);
            }
            else
                notifGeneralDetail.HauliersReference = !string.IsNullOrWhiteSpace(generalDetail.HaulierReference) ? generalDetail.HaulierReference.Trim() : string.Empty;
            #endregion

            #region Client
            if (!string.IsNullOrWhiteSpace(generalDetail.Client) && generalDetail.Client.Trim().Length > 70)
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["ClientSize"]);
            }
            else
                notifGeneralDetail.Client = !string.IsNullOrWhiteSpace(generalDetail.Client) ? generalDetail.Client.Trim() : string.Empty;
            #endregion

            #region Indemntiy Check
            int indemntity = GetBitSystemValue(generalDetail.Indemnity);
            if (indemntity >= 0)
                notifGeneralDetail.Indemnity = indemntity;
            else
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["InvalidIndemnity"]);
            }
            #endregion

            #region Renotification Validation
            if (validNERenotif != null)
            {
                try
                {
                    var esdalRef = generalDetail.ESDALReferenceNumber.Split('/');
                    if (esdalRef[0] != "NEN")
                        validNERenotif.InValid = true;
                }
                catch
                {
                    validNERenotif.InValid = true;
                }
                if (validNERenotif.InValid || validNERenotif.IsNotExist)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["NotValidRenotifEsdalReference"]);
                }
                else if (validNERenotif.IsRenotified)
                {
                    errorCount++;
                    validationErrorBuilder.Append(errorCount + ValidationFailure["Renotifed"]);
                }
                else
                {
                    notifGeneralDetail.NonEsdalKeyId = validNERenotif.NonEsdalKeyId;
                    notifGeneralDetail.PrevEsdalRef = generalDetail.ESDALReferenceNumber;
                }
            }
            #endregion

            #region Error Count
            if (errorCount > 0)
            {
                string errorMessage = validationErrorBuilder.ToString().Trim();
                List<string> generalDetailsErrorList = errorMessage.Split('~').ToList();
                generalDetailsErrorList.RemoveAt(generalDetailsErrorList.Count - 1);
                ValidationError validationError = new ValidationError
                {
                    ErrorCount = errorCount,
                    ErrorMessage = string.Join("~", generalDetailsErrorList),
                    ErrorList = generalDetailsErrorList
                };
                notifGeneralDetail.GeneralDetailError = validationError;
            }
            #endregion

            return notifGeneralDetail;
        }
        #endregion

        #region Private Methods
        private bool IsValidDate(string value, string dateFormats)
        {
            DateTime tempDate;
            bool validDate = DateTime.TryParseExact(value, dateFormats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out tempDate);
            if (validDate)
                return true;
            else
                return false;
        }
        private bool CheckIsMailValid(string email)
        {
            if (email.Trim().EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new MailAddress(email.Trim());
                return addr.Address == email.Trim();
            }
            catch (FormatException ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateNotifGeneralDetail : Invalid Email Input:{ email} Exception: { ex.Message } StackTrace { ex.StackTrace}");
                return false;
            }
        }
        private static bool IsNumber(string number)
        {
            return Regex.Match(number, @"^(\+?( |-|\.)?\d{1,2}( |-|\.)?)?(\(?\d{3}\)?|\d{3})( |-|\.)?(\d{3}( |-|\.)?\d{4})*").Success;
        }

#pragma warning disable S1144 // Unused private types or members should be removed
        private static bool IsPostcode(string postcode)
#pragma warning restore S1144 // Unused private types or members should be removed
        {
            Regex regx = new Regex("^([Gg][Ii][Rr] 0[Aa]{2}|([A-Za-z][0-9]{1,2}|[A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2}|[A-Za-z][0-9][A-Za-z]|[A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]) {0,1}[0-9][A-Za-z]{2})$");
            return regx.Match(postcode).Success;
        }
        private int GetBitSystemValue(string word)
        {
            int key;
            try
            {
                key = string.IsNullOrWhiteSpace(word) ? (int)ExternalApiBitSystem.No :
                            (int)EnumExtensionMethods.GetValueFromDescription<ExternalApiBitSystem>(word.ToLower());
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateNotifGeneralDetail : Invalid Indemnity Input:{ word} Exception: { ex.Message } StackTrace { ex.StackTrace}");
                key = -1;
            }
            return key;
        }
        #endregion

        #region Reverse Mapping for GeneralClass
        public int GeneralClassReverseMapping(int vehicleClass)
        {
            switch (vehicleClass)
            {
                case (int)ExternalApiGeneralClassificationTypeMapping.GC017:
                case (int)ExternalApiGeneralClassificationTypeMapping.GC018:
                case (int)ExternalApiGeneralClassificationTypeMapping.GC019:
                case (int)ExternalApiGeneralClassificationTypeMapping.GC026:
                case (int)ExternalApiGeneralClassificationTypeMapping.GC027:
                case (int)ExternalApiGeneralClassificationTypeMapping.GC028:
                    vehicleClass = (int)ExternalApiGeneralClassificationType.GC012;
                    break;
                case (int)ExternalApiGeneralClassificationTypeMapping.GC020:
                case (int)ExternalApiGeneralClassificationTypeMapping.GC021:
                case (int)ExternalApiGeneralClassificationTypeMapping.GC022:
                case (int)ExternalApiGeneralClassificationTypeMapping.GC029:
                case (int)ExternalApiGeneralClassificationTypeMapping.GC030:
                case (int)ExternalApiGeneralClassificationTypeMapping.GC031:
                    vehicleClass = (int)ExternalApiGeneralClassificationType.GC013;
                    break;
                case (int)ExternalApiGeneralClassificationTypeMapping.GC023:
                case (int)ExternalApiGeneralClassificationTypeMapping.GC024:
                case (int)ExternalApiGeneralClassificationTypeMapping.GC025:
                case (int)ExternalApiGeneralClassificationTypeMapping.GC032:
                case (int)ExternalApiGeneralClassificationTypeMapping.GC033:
                case (int)ExternalApiGeneralClassificationTypeMapping.GC034:
                    vehicleClass = (int)ExternalApiGeneralClassificationType.GC009;
                    break;
            }
            return vehicleClass;
        }
        #endregion
    }
    #endregion

    #region Validation Error Class
    public class ValidationError
    {
        public int ErrorCount { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> ErrorList { get; set; }
        public ValidationError()
        {
            ErrorList = new List<string>();
        }
    }
    #endregion
}
