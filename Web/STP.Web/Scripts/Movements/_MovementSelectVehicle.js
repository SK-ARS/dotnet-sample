var isAddVehicleError;
function MovementSelectVehicleInit() {
    localStorage.removeItem('ComponentTempData');
    hf_Vr1SoExistingPopUp = false;
    StepFlag = 1;
    SubStepFlag = 0;
    CurrentStep = "Select Vehicle";
    SetWorkflowProgress(1);
    if ($('#plan_movement_hdng').text() == '')
        $('#plan_movement_hdng').text("PLAN MOVEMENT");
    $('#current_step').text(CurrentStep);

    $('#save_btn').hide();
    $('#apply_btn').hide();

    if (SelectedVehicles.length > 0) {
        $('#confirm_btn').removeClass('blur-button');
        $('#confirm_btn').attr('disabled', false);
        $('#confirm_btn').show();
        $('#back_btn').show();
        $('#backbutton').show();
    }
    else {
        $('#confirm_btn').addClass('blur-button');
        $('#confirm_btn').attr('disabled', true);
        $('#confirm_btn').hide();
        if ($('#hf_IsSortApp').val() != "True")
            $('#back_btn').hide();
    }
    isAddVehicleError = $('#hf_IsAddVehicleError').val();

    if (isAddVehicleError == 1) {
        SelectedVehicles.pop();
        ShowErrorPopup("The vehicle is not added to the movement, as it is not compatible with the original movement type.", 'CloseErrorPopup');
    }
    else if (isAddVehicleError == 2) {
        SelectedVehicles.pop();
        ShowErrorPopup("The vehicles in this movement are in different category. So we are unable to proceed further. Please edit the vehicle and proceed.", 'CloseErrorPopup');
    }
    else if (isAddVehicleError == 3) {
        SelectedVehicles.pop();
        var msg = $('#hf_IsAddVehicleErrorMsg').val();
        ShowDialogWarningPop(msg, 'Ok', '', 'CloseWarningPopupDialog');
        //ShowErrorPopup("Axle weight is more than gross weight for the vehicle. So we are unable to proceed further. Please edit the vehicle and proceed.", 'CloseErrorPopup');

    }
    else if (isAddVehicleError == 4) {
        SelectedVehicles.pop();
        ShowErrorPopup("Total axle distance exceeds vehicle length for the vehicle. So we are unable to proceed further. Please edit the vehicle and proceed.", 'CloseErrorPopup');
    }
    else {
        $('#IsAddVehicleError').val(isAddVehicleError);
    }
    stopAnimation();
}

$(document).ready(function () {
    $('body').off('click', '.closeamendmentpopup');
    $('body').off('click', '.li_vehicleformovement');
    var cloneCount = 0;
    var componentIdList = [];

    //var clone = $("#add-component-template:last").clone(true, true);

    $('.card-image li.components').click(function () {
        var imageName = $(this).attr("value");
        var componentId = $(this).attr("id");
        var componentName = $(this).attr("name");
        var componentHolder = $(this).parents('div.card-image');
        var componentIndex = $(componentHolder).attr('indx');
        var imageHolder = $(componentHolder).find('div.card');
        var componentNameHolder = $(componentHolder).find('div.filters');
        let clone = $("div[id^='add-component-template']:last").clone(true, true);

        if (componentIdList.length == 0) {
            $("div[id^='add-component-template']:nth-child(2)").find('div.filters').removeClass('disabled');
        }

        componentIdList[componentIndex] = componentId;
        $(imageHolder).find('img').attr('src', '/Content/Images/Common/MasterPage/componet_icons/' + imageName + '.jpg');
        $(componentNameHolder).find('label').text(componentName);

        $(this).parent().find('a.dropdown-item').removeClass('dropdown-active');
        $(this).find('a.dropdown-item').addClass('dropdown-active');

        if (componentIdList.length == (2 + cloneCount)) {
            clone.find('div.filters').attr('aria-expanded', "false");
            clone.find('div.filters').removeClass('show');
            clone.find('ul').removeClass('show');
            clone.attr('indx', cloneCount + 2);
            clone.attr('id', 'add-component-template' + (++cloneCount))
                .appendTo("#component-container");
        }
    });

    $('body').on('click', '.btn_createvehicle', function () {
        localStorage.removeItem('ConfigTypeIdTemp');
        window['CreateNewConfiguration']();
        $('#back_btn').show(); });
    $('body').on('click', '.btn_fleetlibrary', function () {
        window['ImportVehicle']('fleet');
        $('#back_btn').show();});
    $('body').on('click', '.btn_prevmovement', function () {
        window['ImportVehicle']('prevMov');
        $('#back_btn').show();});
    $('body').on('click', '.closeamendmentpopup', function () { window['closeAmendmentPopup'](); });
    $('body').on('click', '.btn_choosefromsimilarcombination', function () { window['ChooseFromSimilarCombinations'](); $('#back_btn').show();});
    $('body').on('click', '.li_createvehicle', function () { window['CreateNewConfiguration'](); });
    $('body').on('click', '.li_fleetlibrary', function () { window['ImportVehicle']('fleet'); });
    $('body').on('click', '.li_prevmovement', function () { window['ImportVehicle']('prevMov'); });
    $('body').on('click', '.li_choosefromsimilarcombination', function () { window['ChooseFromSimilarCombinations'](); });

    $('body').on('click', '.li_vehicleformovement', function (e) {
        var vehicleId = $(this).data("vehicleid");
        var flag = $(this).data("flag");
        UseVehicleForMovement(vehicleId, flag);
        $('#back_btn').show();
    });
});
