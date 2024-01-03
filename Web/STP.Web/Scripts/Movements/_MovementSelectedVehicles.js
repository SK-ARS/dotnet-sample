var MovementIdVal;
var isVr1Val;
var isNotifVal;
var isSoAppVal;
var IsAddVehicleErrorVal;
var IsVehicleListNull;
var vehicleIdsVal;
var VehicleIdAmend = 0;
var planMovement = false;
var isValidationPopUpRequired = false;//If user not amend axle count and click close/cancel or enter axle details & save, the validation popup should appear.

function MovementSelectedVehiclesInit(isValidationPopUpRequiredFlag = true) {
    StepFlag = 2;
    SubStepFlag = 0;
    CurrentStep = "Vehicle Details";
    SetWorkflowProgress(2);
    if ($('#plan_movement_hdng').text() == '')
        $('#plan_movement_hdng').text("PLAN MOVEMENT");
    $('#current_step').text(CurrentStep);

    MovementIdVal = $('#hf_MovementId').val();
    isVr1Val = $('#IsVR1').val() != '' && $('#IsVR1').val() != undefined ? $('#IsVR1').val() : $('#hf_isVr1').val();
    isNotifVal =$('#IsNotif').val()!=''&&$('#IsNotif').val()!=undefined?$('#IsNotif').val(): $('#hf_isNotif').val();
    isSoAppVal = $('#IsSoApp').val() != '' && $('#IsSoApp').val() != undefined ? $('#IsSoApp').val() : $('#hf_isSoApp').val();
    $('#hf_VehicleAssignedGlobal').val($('#hf_IsVehicleRouteAssigned').val());
    IsAddVehicleErrorVal = $('#vehicle_details_section #hf_IsAddVehicleError').val();
    IsVehicleListNull = $('#hf_VehicleListNull').val();
    $('#pm_vehicleClass').val($('#hf_pmvehicleclass').val());
    $('#pm_movementType').val($('#hf_pmmovetype').val());
    var noVehicle = false;
    if (IsVehicleListNull == "True" || IsVehicleListNull == "true") {
        noVehicle = true;
    }
    vehicleIdsVal = $('#hf_vehicleIds').val();
    $('#save_btn').hide();
    $('#apply_btn').hide();

    $('#IsVR1').val(isVr1Val);
    $('#IsNotif').val(isNotifVal);
    $('#IsSoApp').val(isSoAppVal);
    $('#MovementId').val(MovementIdVal);

    if (vehicleIdsVal != undefined && vehicleIdsVal != null) {
        vehicleIdsVal = JSON.parse(vehicleIdsVal);
        for (var i = 0; i < vehicleIdsVal.length; i++) {
            var id = vehicleIdsVal[i];
            LoadSelectedVehicle(id);
        }
    }
    else {
        noVehicle = true;
    }

    if (isTopNavClicked) {
        existingVehicleIds = JSON.parse(JSON.stringify(SelectedVehicles));
        isTopNavClicked = false;
    }
    //Show Axle Amend button For SemiTrailer, if axle count for tractor is zero
    //Is Blank Tractor Axle Elem Exist & Validation exist   
    if ($('.IsBlankTractorAxleElemExist.ConventionalTractor').length > 0) {// && $('.IsBlankTractorAxleElemExist').length == 1
        $('.IsBlankTractorAxleElemExist').each(function (i, elem) {
            var componentId = $(this).data("componentid");
            if (i == 0) {
                var VehicleId = $(elem).closest('.new-vehicle').find('.amend-axle .imgAmendComponentAxle').data("vehicleid");
                var viewAllAxle = $('#view-allAxle.view-all-axle-' + VehicleId);//Get from vehicle details
                var movementTypeId = viewAllAxle.data("movementtypeid");
                var isFleet = viewAllAxle.data("isfleet");
                isValidationPopUpRequired = IsAddVehicleErrorVal > 0;
                ShowAxleAmendPopupFromVehicleDetails(VehicleId, componentId, movementTypeId, isFleet);//If
            }
            $(elem).closest('.new-vehicle').find('.amend-axle .imgAmendComponentAxle').attr('data-componentid', componentId);
            //The AMEND axle button show will call only after axle popup popup load and axle count exist
        });
        
    } else if (isValidationPopUpRequiredFlag){//Is Blank Tractor Axle Elem Not Exist & Validation exist
        showValidationPopupIfErrorExist();
    }

    
    $('#IsAddVehicleError').val(IsAddVehicleErrorVal);
    if (noVehicle) {
        LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', { contactId: $("#SOHndContactID").val() }, '#select_vehicle_section', '', function () {
            MovementSelectVehicleInit();
        });
    }
    else {
        $('#backbutton').show();
        if (IsAddVehicleErrorVal == 1) {
            $('#confirm_btn').addClass('blur-button');
            $('#confirm_btn').attr('disabled', true);
            localStorage.setItem("isVehicleHasChanges", true);
        }
        else {
            $('#confirm_btn').removeClass('blur-button');
            $('#confirm_btn').attr('disabled', false);
        }
        $('#confirm_btn').show();
        if ($('#hf_IsSortApp').val() != "True")
            $('#back_btn').hide();
    }
    if ($('#IsMovement').val() == "True" || $('#isMovement').val() == "true" || $('#IsMovement').val() === 'true') {
        planMovement = true;
    }
    ShowHideHeaderTyreSpaceMovement();
    IterateThroughTextboxMovement();
    ShowInFeetMovement();
    ShowAxleInFeetMovement();
}

function showValidationPopupIfErrorExist() {
    if (IsAddVehicleErrorVal == 3 ) {
        $('.btnAddVehicle').removeAttr('data-bs-toggle');
        var msg = $('#hf_IsAddVehicleErrorMsg').val();
        ShowDialogWarningPop(msg, 'Ok', '', 'CloseWarningPopupDialog');
    }
    else if ($('#hf_IsAddVehicleErrorMsg').val() != "" && $('#hf_IsAddVehicleErrorMsg').val() !=undefined) {
        var msg = $('#hf_IsAddVehicleErrorMsg').val();
        ShowDialogWarningPop(msg, 'Ok', '', 'CloseWarningPopupDialog');
    }
    else if (IsAddVehicleErrorVal == 4) {
        ShowErrorPopup("Total axle distance exceeds vehicle length for the vehicle. Please edit the vehicle and proceed.", 'CloseErrorPopup');
    }
    else if (IsAddVehicleErrorVal == 5) {
        ShowErrorPopup("Based on the vehicle details you entered, system assess the movement as a Notification and thus cannot proceed further. Please edit the vehicle dimensions to match with an application.", 'CloseErrorPopup');
    }
    else if (IsAddVehicleErrorVal == 6) {
        if (IsSortAppVal == "True") {
            ShowErrorPopup("One or more of your vehicles does not require a SO/VR1. Please remove or edit the vehicle before proceeding.", 'CloseErrorPopup');
        }
        else {
            ShowErrorPopup("One or more of your vehicles does not require a notification. Please remove or edit the vehicle before proceeding. If you need assistance please contact the helpdesk on 0300 470 3733.", 'CloseErrorPopup');
        }
    }
    else if (IsAddVehicleErrorVal == 7) {
        if (IsSortAppVal == "True") {
            ShowErrorPopup("One or more of your vehicles does not require a SO/VR1. Please remove or edit the vehicle before proceeding.", 'CloseErrorPopup');
        }
        else {
            ShowErrorPopup("One or more of your vehicles does not require a notification. Please remove or edit the vehicle before proceeding. If you need assistance please contact the helpdesk on 0300 470 3733.", 'CloseErrorPopup');
        }
    }
}

function ValidationMessage() {
    if (IsAddVehicleErrorVal == 3) {
        var msg = $('#hf_IsAddVehicleErrorMsg').val();
        ShowDialogWarningPop(msg, 'Ok', '', 'CloseWarningPopupDialog');
    }
    else if (IsAddVehicleErrorVal == 4) {
        ShowErrorPopup("Total axle distance exceeds vehicle length for the vehicle. Please edit the vehicle and proceed.", 'CloseErrorPopup');
    }
    else if (IsAddVehicleErrorVal == 1) {
        $('#vehicle_details_section .dropdown-menu').removeClass('show');
        showToastMessage({
            message: $('#vehicleassesserror').text(),
            type: "error"
        });
    }
}

function LoadSelectedVehicle(vehicleId) {
    if (!SelectedVehicles.includes(vehicleId)) {
        SelectedVehicles.push(vehicleId);
    }
}

//-------------------------------AMEND START
$(document).ready(function () {
    $('body').on('click', '.create_vehicle', function () { window['CreateNewConfiguration'](); $('#back_btn').show(); });
    $('body').on('click', '.choose_from_fleet', function () { window['ImportVehicle']('fleet'); $('#back_btn').show(); });
    $('body').on('click', '.choose_from_prvmovement', function () { window['ImportVehicle']('prevMov'); $('#back_btn').show(); });
    $('body').on('click', '.choosefromsimilarcombinations', function () { window['ChooseFromSimilarCombinations'](); $('#back_btn').show(); });
    $('body').on('click', '#spnclose', closeAmendmentPopup);
    $('body').on('click', '#btnclose', closeAmendmentPopup);
    $('body').on('click', '#btnSaveAmend', function (e) {
        e.preventDefault();
        //get vehicle details from modal
        var listObj = { VehicleId: VehicleIdAmend, RegistrationDetails: [] };
        var allTrs = $("#amendmentPopup table.tbl_registration > tbody > tr");
        var loader = $("#amendmentPopup .modal-footer #AmendLoader");
        var buttons = $("#amendmentPopup .modal-footer .btn");
        buttons.hide();
        loader.show();
        var isRegValid = true;
        allTrs.each(function (index, value) {
            var isLastElement = index == allTrs.length - 1;
            if (allTrs.length > 1)
                isLastElement = false;
            var regId = $(this).find('.cls_regId').length > 0 ? $(this).find('.cls_regId').text() : $(this).find('td .txt_register_config').val();
            var fleetId = $(this).find('.cls_fleetId').length > 0 ? $(this).find('.cls_fleetId').text() : $(this).find('td .txt_fleet_config ').val();
            
            var id = $(this).find('td .hdId').length > 0 ? $(this).find('td .hdId').val() : 0;
            if (regId != "" || fleetId != "") {
                var obj = { RegId: regId, FleetId: fleetId, Id: id };
                listObj.RegistrationDetails.push(obj);
            }
            else if (isLastElement && regId == "" && fleetId == "") {
                $('#div_reg_config_vehicle').find('#error_msg').html("Please enter at least one registration plate or fleet id");
                isRegValid = false;
            }
        }).promise().done(function () {
            console.log('listObj', listObj);
            if (isRegValid) {
                $.ajax({
                    url: '../VehicleConfig/AmendVehicleConfiguration',
                    type: 'POST',
                    cache: false,
                    async: false,
                    data: { amendRegistration: listObj },
                    beforeSend: function () {
                        //startAnimation();
                    },
                    success: function (response) {
                        buttons.show();
                        loader.hide();
                        if (response.success) {
                            $('#amendmentPopup').modal('hide');
                            showToastMessage({
                                message: "Successfully updated!",
                                type: "success"
                            })
                            ReplaceMovementSelectedVehiclesAllSections(isValidationPopUpRequired);
                            isValidationPopUpRequired = false;
                        } else {
                            if (response.error && response.error != '')
                                ShowErrorPopup(response.error);
                        }
                    },
                    complete: function () {
                        //stopAnimation();
                    },
                    error: function () {
                        buttons.show();
                        loader.hide();
                    }
                });
            }
            else {
                buttons.show();
                loader.hide();
            }
        });
    });
    $('body').on('click', '.vehicleformovement', function (e) {
        var vehicleId = $(this).data("vehicleid");
        var flag = $(this).data("flag");
        UseVehicleForMovement(vehicleId, flag);
    });
    $('body').on('click', '.validationmsg', function (e) {
        ValidationMessage();
    });
});

function AmendVehicle(VehicleId) {

    VehicleIdAmend = VehicleId;
    $('#vehicleConfigId').val(VehicleIdAmend);
    $.ajax({
        url: '../VehicleConfig/ConfigurationRegistration',
        type: 'GET',
        cache: false,
        async: false,
        data: { vehicleId: VehicleId, RegBtn: true, planMovement: planMovement },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            $('#amendmentPopup').modal('show');
            $('#amendmentPopup .modal-body').html(response);
        },
        complete: function () {
            stopAnimation();
        }
    });

}

function ReplaceUpdatedSection(vehicleId) {
    if (vehicleId == null || vehicleId == undefined || vehicleId == 0)
        vehicleId = VehicleIdAmend;

    CloseInfoPopup('SuccessPopupAction');
    $.ajax({
        url: '../VehicleConfig/ViewConfigurationGeneral',
        type: 'GET',
        cache: false,
        async: false,
        data: { vehicleId: vehicleId, isMovement: planMovement },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            $('#viewcomponentdetails_' + VehicleIdAmend).html(response);
            ViewConfigurationGeneralInit();
        },
        complete: function () {
            stopAnimation();
        }
    });
}

//After Amend Axle / Update replace all Movement Selected Vehicles sections
function ReplaceMovementSelectedVehiclesAllSections(isValidationPopUpRequired) {
    var VehicleMovementIdVal = $('#MovementId').val();
    var type = "POST";
    var url = '../Movements/MovementSelectedVehicles';
    var data = { movementId: VehicleMovementIdVal };
    var div = '#vehicle_details_section';
    var initMethods = function () {
        MovementSelectedVehiclesInit(isValidationPopUpRequired);
        VehicleDetailsInit();
        ViewConfigurationGeneralInit();
        GeneralVehicCompInit();
        MovementAssessDetailsInit();
    };

    CloseInfoPopup('SuccessPopupAction');
    LoadContentForAjaxCalls(type, url, data, div, '', initMethods);   
}

function closeAmendmentPopup() {
    ReplaceUpdatedSection(VehicleIdAmend);
    VehicleIdAmend = 0;
    $('#vehicleConfigId').val(VehicleIdAmend);
    $('#amendmentPopup .modal-body').html('');
    $('#amendmentPopup').modal('hide');
}


function ShowHideHeaderTyreSpaceMovement() {
    $('.new-vehicle').each(function () {
        if ($(this).find('.wheel_space').length == 0) {
            $(this).find('.headgrad2').hide();
        }
        else {
            $(this).find('.headgrad2').show();
            $(this).find('.sub1').show();
        }

        if ($(this).find('.tyre_size').length == 0) {
            $(this).find('.headgrad_tyreSize').hide();
        }
        else {
            $(this).find('.headgrad_tyreSize').show();
        }
    });

}

function IterateThroughTextboxMovement() {
    $('#vehicle_details_section .new-vehicle input:text').each(function () {
        if (IsLengthFields(this)) {
            ConvertRangeToFeets(this);
        }
    });
}

function ShowInFeetMovement() {

    $('#vehicle_details_section .new-vehicle input:text').each(function () {

        if (IsPreference()) {

            if (IsLengthFields(this)) {
                var data = $(this).val();
                data = ConvertToFeets(data);
                if (data != "0\'0\"") {
                    $(this).val(data);
                }
                else {
                    $(this).val(null);
                }

            }
        }
    });

}

function ShowAxleInFeetMovement() {
    var unitvalue = $('#UnitValue').val();

    if (unitvalue == 692002) {
        $('.tblAxle tbody tr').each(function () {
            var distanceToNxtAxl = $(this).find('.disttonext').text();
            if (distanceToNxtAxl != undefined && distanceToNxtAxl.indexOf('\'') === -1) {
                distanceToNxtAxl = ConvertToFeet(distanceToNxtAxl);
                $(this).find('.disttonext').text(distanceToNxtAxl);
            }
            var tyreSpace = null;
            $(this).find('.cstable input:text').each(function () {
                var _thistxt = $(this).val();

                if (tyreSpace != null) {
                    tyreSpace = tyreSpace + "," + _thistxt;
                }
                else {
                    tyreSpace = _thistxt;
                }

            });
        });
    }
}