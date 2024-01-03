var vehicleId;
var isAgreedNotif;
var mainVehicleId;
function VehicleDetailsInit() {
    isAgreedNotif = $('#hf_isAgreedNotif').val();
}

function RemoveVehicle(VehicleId, mainVehicle) {
    vehicleId = VehicleId;
    mainVehicleId = mainVehicle;
    ShowWarningPopup("Do you want to remove the Vehicle?", 'RemoveMovementVehicle');
}
function RemoveMovementVehicle() {
    $.ajax({
        type: "POST",
        url: "../VehicleConfig/DeleteMovementVehicle",
        data: { movementId: $('#MovementId').val(), vehicleId: vehicleId, mainVehicleId: mainVehicleId },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            if (response.Success) {
                CloseWarningPopup();
                SelectedVehicles = $.grep(SelectedVehicles, function (value) {
                    return value != vehicleId;
                });
                var msg = 'The vehicle deleted successfully';
                var isRouteAvilable = false;
                if (routeIdArr != undefined && routeIdArr != null) {
                    if (routeIdArr.length > 0)
                        isRouteAvilable = true;
                }
                
                showToastMessage({
                    message: msg,
                    type: "success"
                });
                ReloadVehicleList(3);
            }
        },
        error: function (result) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function ReloadVehicleList(flag) {
    CloseSuccessModalPopup();
    if (flag == 2 || flag == 3) {
        $('#IsVehicleAutoAssigned_Flag').val(false);
    }
    LoadContentForAjaxCalls("POST", '../Movements/MovementSelectedVehicles', { movementId: $('#MovementId').val(), isVehicleModify: flag }, '#vehicle_details_section', '', function () {
        $('#confirm_btn').hide();
        MovementSelectedVehiclesInit();
        VehicleDetailsInit();
        ViewConfigurationGeneralInit();
        GeneralVehicCompInit();
        MovementAssessDetailsInit();
    });
}
function CopyVehicle(VehicleId, Flag) {
    if ($('#IsAddVehicleError').val() == 3) {
        var msg = $('#hf_IsAddVehicleErrorMsg').val();
        ShowDialogWarningPop(msg, 'Ok', '', 'CloseWarningPopupDialog');
        //ShowErrorPopup("Axle weight is more than gross weight for the vehicle. Please edit the vehicle and proceed.", 'CloseErrorPopup');
    }
    else if ($('#IsAddVehicleError').val() == 4) {
        ShowErrorPopup("Total axle distance exceeds vehicle length for the vehicle. Please edit the vehicle and proceed.", 'CloseErrorPopup');
    }
    else {
        if (!SelectedVehicles.includes(VehicleId)) {
            SelectedVehicles.push(VehicleId);
        }
        LoadContentForAjaxCalls("POST", '../VehicleConfig/InsertMovementVehicle', { movementId: $('#MovementId').val() || 0, vehicleId: VehicleId, flag: Flag }, '#vehicle_details_section', '', function () {
            MovementSelectedVehiclesInit();
            VehicleDetailsInit();
            ViewConfigurationGeneralInit();
            GeneralVehicCompInit();
            MovementAssessDetailsInit();
        });
    }
}

//function EditComponents(VehicleId) {
//    LoadContentForAjaxCalls("GET", '../VehicleConfig/EditConfiguration', { vehicleId: VehicleId, isApplication: true }, '#vehicle_Component_edit_section');
//}

function AddMovementVehicleToFleet(vehhId, vehName) {
    CheckFormalNameExist(vehName, vehhId);
}


function EditComponents(VehicleId) {
    $.ajax({
        url: '../VehicleConfig/EditConfiguration',
        type: 'GET',
        cache: false,
        async: false,
        data: { vehicleId: VehicleId, isApplication: true },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            SubStepFlag = 2.4;
            $('#haulier_details_section').hide();
            $('#select_vehicle_section').hide();
            $('#vehicle_details_section').hide();
            $('#movement_type_confirmation').hide();
            $('#select_route_section').hide();
            $('#route').html('');
            $('#route').hide();
            $('#route_vehicle_assign_section').hide();
            $('#supplimentary_info_section').hide();
            $('#overview_info_section').hide();
            $('#vehicle_edit_section').hide
            $('#vehicle_Component_edit_section').hide();
            $('#vehicle_Create_section').hide();

            $('#vehicle_Component_edit_section').show();
            $('#vehicle_Component_edit_section').html(response);
            $('.new-vehicle').unwrap('#banner-container');
            $('.new-vehicle').attr("style", "padding-left:0px !important");
            $('#confirm_btn').hide();
            $("#back_btn").prop('disabled', false);
            SelectMenu(2);
            VehicleConfigCreateVehicleInit();
            VehicleConfigurationAssessmentInit('config_assessment_section');
        },
        complete: function () {
            stopAnimation();
        }
    });
}

//function to check the formal name exists during add to fleet
function CheckFormalNameExist(vehName, vehhId) {

    var flag = 0;
    $.ajax({
        url: '../VehicleConfig/CheckConfigFormalName',
        type: 'POST',
        cache: false,
        async: false,
        data: { VehicleName: vehName },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {

            if (data.success > 0) {
                flag = 1;
                ShowWarningPopup("Configuration already exists. Do you want to over write? <br/><br/>  Note : If you want to save it as a new configuration, edit the internal name and then add to fleet.", 'AddVehicleToFleet', '', vehhId, flag);
            }
            else {
                flag = data.success;
                AddVehicleToFleet(vehhId, flag);
            }
        },
        complete: function () {
            stopAnimation();
        },
        error: function (data) {

        }
    });
}


//function to add to fleet
function AddVehicleToFleet(vehicleappId, flag) {


    $.ajax({
        url: '../VehicleConfig/AddConfigToFleet',
        type: 'POST',
        cache: false,
        async: false,
        data: { VehicleId: vehicleappId, flag: flag },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            if (result.success > 0) {
                CloseWarningPopup();
                ShowSuccessModalPopup('Configuration added to fleet', 'ReloadVehicleList')
            }
            else {
                ShowModalPopup('Not saved');
            }
        },
        complete: function () {
            stopAnimation();
        },
        error: function (data) {

        }
    });
}

$('body').on('click', '#imgcopyvehicle', function (e) {
    var vehicleId = $(this).data("vehicleid");
    var flag = $(this).data("flag");
    CopyVehicle(vehicleId, flag);
});
$('body').on('click', '#imgaddmovement', function (e) {
    var vehhId = $(this).data("vehicleid");
    var vehName = $(this).data("vehiclename");
    AddMovementVehicleToFleet(vehhId, vehName);
});
var isVehicleEditOnPlanMovement = false;
var isPlanMovementEditStarted = false;
$('body').on('click', '#imgeditmovement,.btnEditmovementFromValidationPopup', function (e) {
    var VehicleId = $(this).data("vehicleid");
    isVehicleEditOnPlanMovement = true;
    IsVehicleComponentTypeChanged = false;
    isPlanMovementEditStarted = true;
    if ($(this).hasClass('btnEditmovementFromValidationPopup')) {// from validation popup
        CloseWarningPopupDialog();
    }
    EditComponents(VehicleId);
});

$('body').on('click', '#imgdltmovement', function (e) {
    var VehicleId = $(this).data("vehicleid");
    var mainVehicleId = $(this).data("mainvehicleid");
    RemoveVehicle(VehicleId, mainVehicleId);
});
$('body').on('click', '#imgamendmovement', function (e) {
    var VehicleId = $(this).data("vehicleid");

    AmendVehicle(VehicleId);
});

$('body').on('click', '#spnviewvehicle', function (e) {
    var VehicleId = $(this).data("vehicleid");

    ViewVehicleDetails(VehicleId);
});

//=================AMEND AXLE FOR TRACTOR WITH No Axles
$('body').on('click', '.imgAmendComponentAxle', function (e) {
    var VehicleId = $(this).data("vehicleid");
    var componentId = $(this).data("componentid");
    var viewAllAxleBtnElem = $('#view-allAxle.view-all-axle-' + VehicleId);//Get from vehicle details
    var movementTypeId = viewAllAxleBtnElem.data("movementtypeid");
    var isFleet = viewAllAxleBtnElem.data("isfleet");
    ShowAxleAmendPopupFromVehicleDetails(VehicleId, componentId, movementTypeId, isFleet);
});

//This method will call on AmendAxle button(.imgAmendComponentAxle) click and on movement vehicle list init(if amend axle required)
function ShowAxleAmendPopupFromVehicleDetails(vehicleId, componentId, movementTypeId, isFleet) {
    $('#axleAmendmentPopup #hfVehicleIdForAxleEdit').val(vehicleId);
    $('#axleAmendmentPopup #hfCompIdForAxleEdit').val(componentId);

    var vehicleDetailElem = $('.vehicle-list-' + vehicleId);
    var vehcileName = vehicleDetailElem.data('vehiclename');
    $('#axleAmendmentPopup .vehicle-name-span').text(vehcileName);
    //SHOW ALL AXLE DETAILS
    $('#axleAmendmentPopup .allAxleDetailsAmendPopUpContainer').html('');
    if (typeof AllAxleDetailPopUp != 'undefined')
        AllAxleDetailPopUp(vehicleId, movementTypeId, isFleet, '#axleAmendmentPopup .allAxleDetailsAmendPopUpContainer');
}

$('body').on('click', '#btnCloseAxleAmendPopup,#spnCloseAxleAmend', function () {
    $('#axleAmendmentPopup #hfVehicleIdForAxleEdit,#axleAmendmentPopup #hfCompIdForAxleEdit').val('');
    $('#axleAmendmentPopup').modal('hide');
    showValidationPopupAfterAxleChanges();//Axle amend
});

$('body').on('click', '#btnSaveAxleAmend', function (e) {
    e.preventDefault();
    //get vehicle details from modal
    var vehicleId = $('#axleAmendmentPopup #hfVehicleIdForAxleEdit').val();
    var tractorCompId = $("#axleAmendmentPopup #hfCompIdForAxleEdit").val();
    var hf_ComponentIds = $('#veh_component_details_' + tractorCompId).closest('.new-vehicle').find('.hf_ComponentId');
    //veh_component_details_trailerCompId
    var trailerCompId = 0;
    hf_ComponentIds.each(function (i, obj) {
        var val = $(obj).val();
        if ((parseInt(val) != parseInt(tractorCompId)))
            trailerCompId = val;
    });

    //validate no
    var nextCompAxleCount = $('#veh_component_details_' + tractorCompId).closest('.new-vehicle').find('input.txtNoOfaxles').val();
    var axleCount = $("#axleAmendmentPopup #txtAxleNumber").val();
    if (axleCount == "" || axleCount=="0" || isNaN(axleCount)) {
        showToastMessage({
            message: "Please enter axle count",
            type: "error"
        });
        return false;
    }
    if (parseInt(axleCount) >= parseInt(nextCompAxleCount)) {
        showToastMessage({
            message: "Please enter axle count less than " + nextCompAxleCount,
            type: "error"
        });
        return false;
    }
    var loader = $("#axleAmendmentPopup .modal-footer #AxleAmendLoader");
    var buttons = $("#axleAmendmentPopup .modal-footer .btn");
    buttons.hide();
    loader.show();
    $.ajax({
        url: '../Vehicle/UpdateConventionalTractorAxleCount?axleCount=' + axleCount + '&vehicleId=' + vehicleId + '&fromComponentId=' + trailerCompId + '&toComponentId=' + tractorCompId +'',
        type: 'POST',
        cache: false,
        async: false,
        beforeSend: function () {
            //startAnimation();
        },
        success: function (response) {
            buttons.show();
            loader.hide();
            if (response.success) {
                $('#axleAmendmentPopup').modal('hide');
                showToastMessage({
                    message: "Successfully updated!",
                    type: "success"
                });
                VehicleIdAmend = vehicleId;
                ReplaceMovementSelectedVehiclesAllSections(isValidationPopUpRequired);
                isValidationPopUpRequired = false;
                $('.amend-axle-' + vehicleId).remove();

                //Show validation popup
                //showValidationPopupAfterAxleChanges();
            } else {
                if (response.error && response.error != '')
                    showToastMessage({
                        message: response.error,
                        type: "error"
                    });
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
});

function showValidationPopupAfterAxleChanges() {
    if (typeof isValidationPopUpRequired != 'undefined' && isValidationPopUpRequired) {
        if (typeof showValidationPopupIfErrorExist != 'undefined')
            showValidationPopupIfErrorExist();
        isValidationPopUpRequired = false;
    }
}

//=================AMEND AXLE FOR TRACTOR WITH No Axles