var VSONotificationVehicle;
var isEdit;
function VehicleConfig_VehicleConfigurationInit() {
    isEdit = $("#IsVehicleConfigEdit").val();
    VSONotificationVehicle = $('#IsVSO').val();
    if (VSONotificationVehicle == "True" || VSONotificationVehicle == "true") {
        MovementAssessment();
    }

    $('body').off('focusin', '#div_config_general input#OverallLength,#div_config_general input');
    $('body').on('focusin', '#div_config_general input#OverallLength,#div_config_general input', function (e) {
        $(this).data('val', $(this).val());
    });
    $('body').off('keyup', '#div_config_general input#OverallLength,#div_config_general input');
    $('body').on('keyup', '#div_config_general input#OverallLength,#div_config_general input', function (e) {
        configurationCurrentval.fieldId = $(this).attr("id");;
        configurationCurrentval.fieldVal = $(this).data('val');;
    });

    //else {
        $('body').off('keyup', '#div_config_general input#OverallLength,#div_config_general input#Length');
        $('body').on('keyup', '#div_config_general input#OverallLength,#div_config_general input#Length', function (e) {
            ValidateLengthConfigOnKeyUp();
        });

        $('body').off('blur', '#div_config_general input[type=text],#config_registration_section input');
        $('body').on('blur', '#div_config_general input[type=text],#config_registration_section input', function (e) {
            var isValid = ValidateLengthConfigOnKeyUp(isRigidValidationRequired = false);
            var isFeetValid = ComponentKeyUpValidation(this);
            if (isValid && isFeetValid && $(this).is(':visible'))
                MovementAssessmentApiCall(this);
        });

        $('body').off('keyup', '#div_config_general input[type=text],#config_registration_section input');
        $('body').on('keyup', '#div_config_general input[type=text],#config_registration_section input', function (event) {
            var id = event.which || event.keyCode || event.key || 0;
            if (id == 13 || id == "Enter") {
                var isValid = ValidateLengthConfigOnKeyUp(isRigidValidationRequired = false);
                if (isValid)
                    MovementAssessmentApiCall(this);
            }
        });

        //============COMPONENT INPUT ASSESSMENT==========START
        $('body').off('blur', '.configComponent input[type=text]');
        $('body').on('blur', '.configComponent input[type=text]', function (e) {
            if ($(this).is(':visible'))
                ComponentAssesment(this);
        });
        $('body').off('keyup', '.configComponent input[type=text]');
        $('body').on('keyup', '.configComponent input[type=text]', function (event) {
            var id = event.which || event.keyCode || event.key || 0;
            if (id == 13 || id == "Enter") {
                ComponentAssesment(this)
            }
        });
        //============COMPONENT INPUT ASSESSMENT==========END
    //}
    if (isEdit == 1) {
        GenerateVehicleConfiguration(1);
    }
}

function MovementAssessmentApiCall(_this) {
    var _thisObj = _this;
    _this = $(_this);
    var configFieldVal = _this.val();
    var attribId = _this.attr('id');
    var isInput = _this.is('input');
    var compCount = 0;
    var noAxle = 0;
    if (attribId == "Internal_Name") {
        $('.comp').each(function () {
            compCount++;
            var componentId = $(this).attr("id");
            var compElem = $('#' + componentId + ' #Internal_Name');
            if ($(compElem).val() == "" && configFieldVal != "") {
                var compName = configFieldVal + "-component" + compCount;
                ShowUndoMessage(compElem, _thisObj);
                $(compElem).val(compName);
            }
        });
    }
    else if (attribId == "Number_of_Axles") {
        $('#VehicleConfigInfo #Number_of_Axles').each(function () {
            if ($(this).val() == "")
                noAxle = 1;
        });
        if (noAxle == 0)
            MovementAssessment();
    }
    else if (attribId != "txt_register_config" && attribId != "txt_fleet_config" && isInput) {
        if (saveVehicleButtonClicked) {
            saveVehicleButtonClicked = false;
        }
        else {
            ComponentsAutoFill(_thisObj);
        }
        if (VSONotificationVehicle != "True" && VSONotificationVehicle != "true") {
            MovementAssessment();
        }
    }
    else {
        if (saveVehicleButtonClicked) {
            saveVehicleButtonClicked = false;
        }
        else {
            ComponentsAutoFill(_thisObj);
        }
    }
}

function ComponentAssesment(_this) {
    _this = $(_this);
    var isValid = ValidateLengthConfigOnKeyUp();
    var attribId = _this.attr('id');
    var noAxle = 0;
    var ignoreIds = ["Internal_Name", "txt_register", "txt_fleet", "wheels", "distancetoaxle", "tyresize"]
    if (attribId == "Number_of_Axles") {
        $('#VehicleConfigInfo #Number_of_Axles').each(function () {
            if ($(this).val() == "")
                noAxle = 1;
        });
    }
    if (isValid && ignoreIds.indexOf(attribId) < 0) {
        if (VSONotificationVehicle != "True" && VSONotificationVehicle != "true") {
            if (noAxle == 0)
                MovementAssessment();
        }
    }
}

$(document).ready(function () {
    //VehicleConfig_VehicleConfigurationInit();
    $('body').off('click', ".axlePopUp");
    VehicleConfigurationInit();
    $('body').on('click', '#vehicle_save_btn', function (e) {
        e.preventDefault();
        saveVehicleButtonClicked = true;
        SaveVehicleConfigurationV1(this);
    });
    
    $('body').on('click', '.clearValidation', function (e) {
        e.preventDefault();
        ClearValidationMessageFn(this);
    });

    $('body').on('click', '.axlePopUp,.btnOpenAxlePopUp', function (e) {
        e.preventDefault();
        AxleTableMethods.OpenAxlePopupFromConfigSection(this);
    });

    $('body').on('click', '#btnOpenAllAxlePopup', function (e) {
        e.preventDefault();
        AxleTableMethods.OpenAxlePopupFromConfigSection(this,loadFull=true);
    });

    $('body').on('click', '.btn_AddAxleDetails', function (e) {
        e.preventDefault();
        AxleTableMethods.SaveFromPopupAndAddAxleToComponent(this);
        CloseAxlePopUp();
    });

    $('body').on('click', '.view-axle-modal-close,.btn_ClearAxleDetails', function (e) {
        e.preventDefault();
        
        CloseAxlePopUp();
    });
});

function ClearValidationMessageFn(_this) {
    ClearValidationMessage(_this);
}

