var viewVehicleFlag = false;
function MovementSelectVehicleCombInit() {
    StepFlag = 1;
    SubStepFlag = 1.2;
    CurrentStep = "Select Vehicle";
    SetWorkflowProgress(1);
    $('#confirm_btn').hide();

    $('div#edit_btn_cntr').hide();
    $('div#confirm_btn_cntr').hide();
    $('body').on('keyup', '#combinationlst_cntr input[type=text]', function (e) {
        e.preventDefault();
        GetSimilarVehicles(this);
    });
    stopAnimation();
}

function GetSimilarVehicles() {
    var searchFlag = false;
    $("input[type=text]").each(function () {
        if ($(this).val()) {
            searchFlag = true;
        }
    });

    if (searchFlag) {
        $.ajax({
            url: '../Movements/GetFilteredCombinations',
            type: 'POST',
            cache: false,
            async: false,
            data: $("#VehicleConfigInfo").serialize(),
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {
                $('div#similar_vehicles_contnr').html(response);
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
}

function OnBackBtnClick() {
    if (viewVehicleFlag) {
        ShowSearchedCombinations()
    }
}

function ShowSearchedCombinations() {
    viewVehicleFlag = false;
    $('div#vehicles').children().not(':first-child').remove();
    $(combinationLstCln).appendTo('div#vehicles');
}

function EditVehicle() {
    window.location = '../VehicleConfig/EditConfiguration' + EncodedQueryString('vehicleId=' + vehicleIdGbl);
}
