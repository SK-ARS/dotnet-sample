var componentId;
var unit;
var ApplicationRevId;
var isVR1;
var isNotify;
$(document).ready(function () {
   
    vehicleId = $('#hf_VehicleConfigId').val();
    var weight;
    $(".comp").each(function () {
        var compId = $(this).attr('id');
        weight = $('#' + compId).find('#Weight').val();

        if (weight == "") {
            weight = null;
        }
        $.ajax({
            url: '../Vehicle/AxleValidationCalculation',
            type: 'POST',
            data: { "weight": weight },
            success: function (response) {
                $('#' + compId).find('#axleweightRange').val(response);
            },
            error: function (response) {
            }
        });
    });
    $('body').on('click', '#btn_comp_finish', function (e) {
        e.preventDefault();
        componentId = $('#Component_Id').val();
        unit = $('#UnitValue').val();
        ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
        isVR1 = $('#vr1appln').val();
        isNotify = $('#ISNotif').val();
        if (isNotify == 'True' || isNotify == 'true') {
            isVR1 = 'True';
        }
        var vd = validation();

        if (vd) {
            if (componentId == '') {
                SaveComponent(this);
            }
            else {
                UpdateData(this);
            }
        }
        return false;
    });
    $('body').on('blur', '.axledropVehicle', function (e) {//On config NoOfAxle texbox blur, the component NoOfAxle textbox change event will trigger
        var axleDropCount = $(this).attr('axledropcount') || 1;
        if ($(this).val() != "") {
            componentCurrentval.fieldId = "Number_of_Axles";
            componentCurrentval.fieldVal = $('.comp:eq(' + (parseInt(axleDropCount) - 1) + ') .axledrop').val();
        }
        $('.comp:eq(' + (parseInt(axleDropCount) - 1) + ') .axledrop').val($(this).val());
        $('.comp:eq(' + (parseInt(axleDropCount) - 1) + ') .axledrop').trigger('blur');
    });
    $('body').on('blur', '.axledrop', function (e) {
        AxleTableMethods.OnAxleTextBoxChange(this);
    });
    $('body').on('keyup', '.pageKeyUpValidation', function (e) {
        keyUpValidationFn(this);
    });
        $('body').on('keyup', '.pageKeyUpSpeedValidation', function (e) {
        validationSpeedFn(this);
        });
    $('body').on('click', '#btnbacktochoose', function (e) {
        e.preventDefault();
        BackToPreviousPage();
    });
    function BackToPreviousPage() {
        var flag = true;
        window.location = "../VehicleConfig/VehicleConfigList?flag="+flag;
    }

    $('body').on('change', '#Weight', function (e) {
        var weight = $('#Weight').val();
        if (weight == "") {
            weight = null;
        }
        $.ajax({
            url: '../Vehicle/AxleValidationCalculation',
            type: 'POST',
            data: { "weight": weight },
            success: function (response) {
                $('#axleweightRange').val(response);
            },
            error: function (response) {
            }
        });
    })
});

function keyUpValidationFn(e) {
    keyUpValidation(e);
}
function validationSpeedFn(e) {
    validationSpeed(e);
}
