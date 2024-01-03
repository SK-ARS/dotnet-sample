var componentCurrentval = {
    fieldId: "",
    fieldVal: ""
}
var axlesListPopUp = [];
var configurationCurrentval = {
    fieldId: "",
    fieldVal: ""
}
var configurationRevertCurrentval = {
    fieldId: "",
    fieldVal: ""
}
var configurationSumUpCurrentval = {
    fieldId: "",
    fieldVal: ""
}
var componentPreviousval = [];
var axlesListFromPopUp = [];

function ComponentsAutoFill(_this) {
    componentCurrentval = {
        fieldId: "",
        fieldVal: ""
    };
    var componentCount = $('#ComponentCount').val();
    var TotalComp = componentCount;
    if (TotalComp != undefined && TotalComp != "")
        TotalComp = parseInt(TotalComp);

    if (saveVehicleButtonClicked) {
        saveVehicleButtonClicked = false;
    }

    if (componentCount != 1 && componentCount != "1") {

        var configTypeId = $("#ConfigTypeId").val();
        var compCount = 0;
        var configFieldVal = $(_this).val();
        var configFieldId = $(_this).attr("id");
        var configFieldAxle = $(_this).attr("axleDropCount");
        var isLpConfig = false;
        if (configFieldId == 'txt_register_config' || configFieldId == 'txt_fleet_config') {
            configFieldId = configFieldId.replace("_config", "");//id is different in component section
            isLpConfig = true;
        }
        var widthTrailer;
        var widthTractor;
        $('.comp').each(function () {
            compCount++;
            var componentId = $(this).attr("id");
            var IsTractor = false;
            if ($('#' + componentId).find('#Tractor').val().toLowerCase() == "true") {
                IsTractor = true;
            }
            $('#' + componentId).find('.dynamic input,textarea:not(hidden,checkbox),#regDetailsConfig input:not(hidden)').each(function () {
                var compFieldVal = $(this).val();
                var compFieldId = $(this).attr("id");
                var compFieldAxle = $(this).attr("axleDropCount");
                var compElem = !isLpConfig ? $('#' + componentId + ' #' + compFieldId) : $('#div_reg_component_vehicle_' + componentId + ' #' + compFieldId);
                var weight = 0;

                switch (parseInt(configTypeId)) {
                    case VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER:
                        if (configFieldId == compFieldId) {
                            if ((compFieldId == "Length" || compFieldId == "Rear_Overhang"
                                || compFieldId == "Right_Overhang" || compFieldId == "Left_Overhang" || compFieldId == "Weight")
                                && !IsTractor) {
                                if (!saveVehicleButtonClicked)
                                    ShowUndoMessage(compElem, _this, compCount, componentId);
                                $(compElem).val(configFieldVal);
                                $(compElem).attr("previousvalue", configFieldVal);
                                if (compFieldId == "Weight") {
                                    weight = configFieldVal;
                                    IsGroundClearenceRequired(weight, componentId);
                                }
                            }
                            else if (compFieldId == "Width") {
                                if (!IsTractor) {
                                    widthTrailer = $(compElem).val() || 0;
                                    if (parseFloat(widthTractor) <= parseFloat(widthTrailer)
                                        || (parseFloat(configFieldVal) > parseFloat(widthTrailer) && parseFloat(configFieldVal) > parseFloat(widthTractor))) {
                                        if (widthTrailer != 0 && !saveVehicleButtonClicked)
                                            ShowUndoMessage(compElem, _this, compCount, componentId);
                                        $(compElem).val(configFieldVal);
                                    }
                                }
                                else {
                                    widthTractor = $(compElem).val() || 0;
                                    if (parseFloat(configFieldVal) < parseFloat(widthTractor)) {
                                        if (widthTractor != 0 && !saveVehicleButtonClicked)
                                            ShowUndoMessage(compElem, _this, compCount, componentId);
                                        $(compElem).val(configFieldVal);
                                    }
                                }
                            }
                            else if ((compFieldId == "Front_Overhang") && IsTractor) {
                                ShowUndoMessage(compElem, _this, compCount, componentId);
                                $(compElem).val(configFieldVal);
                            }
                            else if ((compFieldId == 'txt_register' || compFieldId == 'txt_fleet') && IsTractor) {
                                ShowUndoMessage(compElem, _this, compCount, componentId);
                                $(compElem).val(configFieldVal);
                            }
                            else if (compFieldId == 'Number_of_Axles' && compFieldAxle == configFieldAxle) {
                                ShowUndoMessage(compElem, _this, compCount, componentId);
                                $(compElem).val(configFieldVal);
                                compElem.trigger('change');
                            }
                        }
                        break;
                    case VEHICLE_CONFIGURATION_TYPE_CONFIG.SEMI_TRAILER:
                        if (configFieldId == compFieldId) {
                            if ((compFieldId == "Length" || compFieldId == "Maximum_Height"
                                || compFieldId == "Rear_Overhang" || compFieldId == "Right_Overhang" || compFieldId == "Left_Overhang"
                                || compFieldId == "Reducable_Height")
                                && !IsTractor) {
                                ShowUndoMessage(compElem, _this, compCount, componentId);
                                $(compElem).val(configFieldVal);
                            }
                            else if (compFieldId == "Width") {
                                if (!IsTractor) {
                                    widthTrailer  = $(compElem).val()||0;
                                    if (parseFloat(widthTractor) <= parseFloat(widthTrailer)
                                        || (parseFloat(configFieldVal) >= parseFloat(widthTrailer) && parseFloat(configFieldVal) > parseFloat(widthTractor))) {
                                        if (widthTrailer != 0)
                                            ShowUndoMessage(compElem, _this, compCount, componentId);
                                        $(compElem).val(configFieldVal);
                                    }
                                }
                                else {
                                    widthTractor= $(compElem).val()||0;
                                    if (parseFloat(configFieldVal) < parseFloat(widthTractor)) {
                                        if (widthTractor != 0)
                                            ShowUndoMessage(compElem, _this, compCount, componentId);
                                        $(compElem).val(configFieldVal);
                                    }
                                }
                            }
                            else if (compFieldId == "Front_Overhang" && IsTractor) {
                                ShowUndoMessage(compElem, _this, compCount, componentId);
                                $(compElem).val(configFieldVal);
                            }
                            else if ((compFieldId == 'txt_register' || compFieldId == 'txt_fleet') && IsTractor) {
                                ShowUndoMessage(compElem, _this, compCount, componentId);
                                $(compElem).val(configFieldVal);
                            }
                            else if (compFieldId == 'Number_of_Axles' && compFieldAxle == configFieldAxle) {
                                ShowUndoMessage(compElem, _this, compCount, componentId);
                                $(compElem).val(configFieldVal);
                                compElem.trigger('change');
                            }
                        }
                        break;
                    case VEHICLE_CONFIGURATION_TYPE_CONFIG.BOAT_MAST_EXCEPTION:
                        var compType = $('#' + componentId).find('#vehicleTypeValue').val();
                        if (configFieldId == compFieldId) {
                            if (compType == TypeConfiguration.CONVENTIONAL_TRACTOR || compType == TypeConfiguration.SEMI_TRAILER) {
                                if ((compFieldId == "Length" || compFieldId == "Maximum_Height"
                                    || compFieldId == "Rear_Overhang" || compFieldId == "Right_Overhang" || compFieldId == "Left_Overhang"
                                    || compFieldId == "Reducable_Height")
                                    && !IsTractor) {
                                    ShowUndoMessage(compElem, _this, compCount, componentId);
                                    $(compElem).val(configFieldVal);
                                }
                                else if (compFieldId == "Width") {
                                    if (!IsTractor) {
                                        widthTrailer = $(compElem).val() || 0;
                                        if (parseFloat(widthTractor) <= parseFloat(widthTrailer)
                                            || (parseFloat(configFieldVal) >= parseFloat(widthTrailer) && parseFloat(configFieldVal) > parseFloat(widthTractor))) {
                                            if (widthTrailer != 0)
                                                ShowUndoMessage(compElem, _this, compCount, componentId);
                                            $(compElem).val(configFieldVal);
                                        }
                                    }
                                    else {
                                        widthTractor = $(compElem).val() || 0;
                                        if (parseFloat(configFieldVal) < parseFloat(widthTractor)) {
                                            if (widthTractor != 0)
                                                ShowUndoMessage(compElem, _this, compCount, componentId);
                                            $(compElem).val(configFieldVal);
                                        }
                                    }
                                }
                                else if (compFieldId == "Front_Overhang" && IsTractor) {
                                    ShowUndoMessage(compElem, _this, compCount, componentId);
                                    $(compElem).val(configFieldVal);
                                }
                                else if ((compFieldId == 'txt_register' || compFieldId == 'txt_fleet') && IsTractor) {
                                    ShowUndoMessage(compElem, _this, compCount, componentId);
                                    $(compElem).val(configFieldVal);
                                }
                                else if (compFieldId == 'Number_of_Axles' && compFieldAxle == configFieldAxle) {
                                    ShowUndoMessage(compElem, _this, compCount, componentId);
                                    $(compElem).val(configFieldVal);
                                    compElem.trigger('change');
                                }
                            }
                            else {
                                if (compFieldId == "Width") {
                                    widthTractor = $(compElem).val() || 0;
                                    if (parseFloat(configFieldVal) < parseFloat(widthTractor)) {
                                        ShowUndoMessage(compElem, _this, compCount, componentId);
                                        $(compElem).val(configFieldVal);
                                    }
                                }
                                if (compFieldId == "Rear_Overhang" && compCount == TotalComp) {
                                    ShowUndoMessage(compElem, _this, compCount, componentId);
                                    $(compElem).val(configFieldVal);
                                }
                                else if (compFieldId == "Front_Overhang" && compCount == 1) {
                                    ShowUndoMessage(compElem, _this, compCount, componentId);
                                    $(compElem).val(configFieldVal);
                                }
                                else if ((compFieldId == 'txt_register' || compFieldId == 'txt_fleet') && IsTractor) {
                                    ShowUndoMessage(compElem, _this, compCount, componentId);
                                    $(compElem).val(configFieldVal);
                                }
                                else if (compFieldId == 'Number_of_Axles' && compFieldAxle == configFieldAxle) {
                                    ShowUndoMessage(compElem, _this, compCount, componentId);
                                    $(compElem).val(configFieldVal);
                                    compElem.trigger('change');
                                }
                                else if ((parseInt(configTypeId) == VEHICLE_CONFIGURATION_TYPE_CONFIG.RIGID_AND_DRAG ||
                                    parseInt(configTypeId) == VEHICLE_CONFIGURATION_TYPE_CONFIG.BOAT_MAST_EXCEPTION) &&
                                    (compFieldId == "Left_Overhang" || compFieldId == "Right_Overhang") && compCount == TotalComp) {
                                    ShowUndoMessage(compElem, _this, compCount, componentId);
                                    $(compElem).val(configFieldVal);
                                }
                            }
                        }
                        break;
                    case VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER_3_TO_8:
                    case VEHICLE_CONFIGURATION_TYPE_CONFIG.SEMI_TRAILER_3_TO_8:
                    case VEHICLE_CONFIGURATION_TYPE_CONFIG.RIGID_AND_DRAG:
                    case VEHICLE_CONFIGURATION_TYPE_CONFIG.RECOVER_VEHICLE:
                    case VEHICLE_CONFIGURATION_TYPE_CONFIG.SPMT:
                        if (configFieldId == compFieldId) {
                            if (compFieldId == "Width") {
                                widthTractor = $(compElem).val() || 0;                             
                                if (parseFloat(configFieldVal) < parseFloat(widthTractor)) {
                                    ShowUndoMessage(compElem, _this, compCount, componentId);
                                    $(compElem).val(configFieldVal);
                                }                                
                            }
                            if (compFieldId == "Rear_Overhang" && compCount == TotalComp) {
                                ShowUndoMessage(compElem, _this, compCount, componentId);
                                $(compElem).val(configFieldVal);
                            }
                            else if (compFieldId == "Front_Overhang" && compCount==1) {
                                ShowUndoMessage(compElem, _this, compCount, componentId);
                                $(compElem).val(configFieldVal);
                            }
                            else if ((compFieldId == 'txt_register' || compFieldId == 'txt_fleet') && IsTractor) {
                                ShowUndoMessage(compElem, _this, compCount, componentId);
                                $(compElem).val(configFieldVal);
                            }
                            else if (compFieldId == 'Number_of_Axles' && compFieldAxle == configFieldAxle) {
                                ShowUndoMessage(compElem, _this, compCount, componentId);
                                $(compElem).val(configFieldVal);
                                compElem.trigger('change');
                            }
                            else if ((parseInt(configTypeId) == VEHICLE_CONFIGURATION_TYPE_CONFIG.RIGID_AND_DRAG ||
                                parseInt(configTypeId) == VEHICLE_CONFIGURATION_TYPE_CONFIG.BOAT_MAST_EXCEPTION )&&
                                (compFieldId == "Left_Overhang" || compFieldId == "Right_Overhang") && compCount == TotalComp) {
                                ShowUndoMessage(compElem, _this, compCount, componentId);
                                $(compElem).val(configFieldVal);
                            }
                        }
                        break;
                    default:
                        break;
                }
            });
        });
    }
}

function ConfigAutoFill(_this) {
    configurationCurrentval = {
        fieldId: "",
        fieldVal: ""
    }
    //configurationSumUpCurrentval = {
    //    fieldId: "",
    //    fieldVal: ""
    //}
    componentCurrentval = {
        fieldId: "",
        fieldVal: ""
    }
    var configTypeId = $("#ConfigTypeId").val();
    var componentId = $(_this).parent().closest('.comp').attr('id');
    var IsTractor = false;

    var compFieldVal = $(_this).val();
    var compFieldId = $(_this).attr("id");
    var configElem = $('#div_config_general #' + compFieldId);
    var configFieldId = $(configElem).attr("id");
    if (configElem.length == 0) {
        configElem = $('#config_registration_section #' + compFieldId + "_config");
        configFieldId = compFieldId;
    }
    var configFieldAxle = $(configElem).attr("axleDropCount");
    var VSONotificationVehicle = $('#IsVSO').val();
    var mvmntTypeId = $('#MovementTypeId').val();

    if (compFieldId == configFieldId) {
        var weight = 0;
        var compWeight = 0;
        var overallLength = 0;
        var overallLengthMtr = 0;
        var compCount = 0;
        var isNoValue = 0;
        var maxVal = 0;
        var maxValMtr = 0;
        var wyt1 = 0;
        var wyt2 = 0;
        var TotalComp = $('#ComponentCount').val();
        if (TotalComp != undefined && TotalComp != "")
            TotalComp = parseInt(TotalComp);
        var compValuesObj = {};
        $('.comp').each(function () {
            var compId = $(this).attr("id");
            compCount++;

            var configFieldVal = $(configElem).val();
            var compElemVal = $('#' + compId + " #" + compFieldId).val()||'';
            if ($('#' + compId).find('#Tractor').val().toLowerCase() == "true") {
                IsTractor = true;
            }
            else {
                IsTractor = false;
            }
            //$('#' + compId).find('#' + compFieldId).each(function () {
            if (compValuesObj[compFieldId + "-" + TotalComp] == undefined)
                compValuesObj[compFieldId + "-" + TotalComp] = { fieldVal: 0};

            switch (parseInt(configTypeId)) {
                case VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER:
                case VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER_3_TO_8:
                    if (compFieldId == "Weight") {
                        var wyt = compElemVal != "" ? compElemVal : 0;
                        if (compWeight < parseFloat(wyt))
                            compWeight = parseFloat(wyt);

                        var wyt = compElemVal != "" ? compElemVal : 0;
                        weight = parseFloat(weight) + parseFloat(wyt);
                        if (parseFloat(wyt) == 0)
                            isNoValue = 1;
                        compValuesObj[compFieldId + "-" + TotalComp].fieldVal = wyt;

                        if (compCount == TotalComp && isNoValue == 0) {
                            ShowUndoMessageForConfiguration(configElem,_this, compWeight, $('#TrainWeight'), weight);
                            $('#TrainWeight').val(weight != 0 ? weight : "");
                            $(configElem).val(compWeight != 0 ? compWeight : "");

                            if (VSONotificationVehicle != "True" && VSONotificationVehicle != "true") {
                                MovementAssessment();
                            }
                        }
                        IsGroundClearenceRequired(wyt, compId);
                    }
                    else if (compFieldId == "Maximum_Height" || compFieldId == "Reducable_Height" || compFieldId == "Width"
                        || compFieldId == "Left_Overhang" || compFieldId == "Right_Overhang" ) {
                        var currVal = compElemVal != "" ? compElemVal : 0;
                        var currValNew = currVal != 0 ? currVal : "";
                        maxValMtr = maxVal;
                        if ($('#UnitValue').val() == 692002) {
                            currVal = ConverteFeetToMetre(currVal);
                            configFieldVal = ConverteFeetToMetre(configFieldVal);
                            maxValMtr = ConverteFeetToMetre(maxVal);
                        }
                        if (parseFloat(currVal) == 0)
                            isNoValue = 1;
                        compValuesObj[compFieldId + "-" + TotalComp].fieldVal = currVal;

                        if (parseFloat(currVal) > parseFloat(maxValMtr))
                            maxVal = currValNew;

                        if (compCount == TotalComp && isNoValue == 0) {
                            ShowUndoMessageForConfiguration(configElem, _this, maxVal);
                            $(configElem).val(maxVal != 0 ? maxVal : "");
                        }

                    }
                    else if (compFieldId == "Length") {
                        var len = compElemVal != "" ? compElemVal : 0;
                        var lenMtr = len;
                        maxValMtr = maxVal;
                        if ($('#UnitValue').val() == 692002) {
                            //overallLengthMtr = ConverteFeetToMetre(overallLength);
                            lenMtr = ConverteFeetToMetre(lenMtr);
                            configFieldVal = ConverteFeetToMetre(configFieldVal);
                            //overallLengthMtr = parseFloat(overallLengthMtr) + parseFloat(lenMtr);
                            //overallLength = ConvertMetreToFeet(overallLengthMtr);
                            maxValMtr = ConverteFeetToMetre(maxVal);
                        }
                        else {
                            //overallLength = parseFloat(overallLength) + parseFloat(len);
                        }
                        if (parseFloat(lenMtr) == 0)
                            isNoValue = 1;
                                                
                        if (parseFloat(lenMtr) > maxValMtr)
                            maxVal = len;

                        if (compCount == TotalComp && isNoValue == 0) {
                            ShowUndoMessageForConfiguration(configElem, _this, maxVal);
                            //$('#OverallLength').val(overallLength != 0 ? overallLength : "");
                            $(configElem).val(maxVal != 0 ? maxVal : "");
                        }
                    }
                    else if (compFieldId == "Front_Overhang") {
                        if ($(configElem).val() != compElemVal) {
                            if (compCount == 1) {
                                ShowUndoMessageForConfiguration(configElem, _this, compElemVal);
                                $(configElem).val(compElemVal);
                            }
                        }
                    }
                    else if (compFieldId == "Rear_Overhang") {
                        if ($(configElem).val() != compElemVal) {
                            if (compCount == TotalComp) {
                                ShowUndoMessageForConfiguration(configElem, _this, compElemVal);
                                $(configElem).val(compElemVal);
                            }
                        }
                    }
                    else if (compFieldId == 'Number_of_Axles') {
                        if ($(configElem).hasClass("axledropVehicle_" + compCount) && $(".axledropVehicle_" + compCount).val() != compElemVal) {
                            ShowUndoMessageForConfiguration($(".axledropVehicle_" + compCount), _this, compElemVal);
                            $(".axledropVehicle_" + compCount).val(compElemVal);
                        }
                    }
                    else if ((compFieldId == 'txt_register' || compFieldId == 'txt_fleet') && IsTractor) {
                        if (compElemVal == "")
                            $(configElem).val("");
                    }
                    break;
                case VEHICLE_CONFIGURATION_TYPE_CONFIG.SEMI_TRAILER:
                case VEHICLE_CONFIGURATION_TYPE_CONFIG.SEMI_TRAILER_3_TO_8:
                case VEHICLE_CONFIGURATION_TYPE_CONFIG.BOAT_MAST_EXCEPTION:
                    var compType = $('#' + compId).find('#vehicleTypeValue').val();
                    if (compFieldId == "Weight" && compType != TypeConfiguration.CONVENTIONAL_TRACTOR && compType != TypeConfiguration.SEMI_TRAILER) {
                        var wyt = compElemVal != "" ? compElemVal : 0;
                        if (compWeight < parseFloat(wyt))
                            compWeight = parseFloat(wyt);

                        weight = parseFloat(weight) + parseFloat(wyt);
                        if (parseFloat(wyt) == 0)
                            isNoValue = 1;
                        if (compCount == TotalComp && isNoValue == 0) {
                            ShowUndoMessageForConfiguration(null, _this, null, configElem, weight);
                            $(configElem).val(weight != 0 ? weight : "");

                            if (VSONotificationVehicle != "True" && VSONotificationVehicle != "true") {
                                MovementAssessment();
                            }
                        }
                        IsGroundClearenceRequired(wyt, compId);
                    }
                    else if (compFieldId == "Weight") {
                        var wyt = compElemVal != "" ? compElemVal : 0;
                        IsGroundClearenceRequired(wyt, compId);
                    }
                    if (compFieldId == "Maximum_Height" || compFieldId == "Reducable_Height" || compFieldId == "Width"
                        || compFieldId == "Left_Overhang" || compFieldId == "Right_Overhang") {
                        var currVal = compElemVal != "" ? compElemVal : 0;
                        var currValNew = currVal != 0 ? currVal : "";
                        maxValMtr = maxVal;
                        if ($('#UnitValue').val() == 692002) {
                            currVal = ConverteFeetToMetre(currVal);
                            configFieldVal = ConverteFeetToMetre(configFieldVal);
                            maxValMtr = ConverteFeetToMetre(maxVal);
                        }
                        if (parseFloat(currVal) > parseFloat(maxValMtr))
                            maxVal = currValNew;

                        if (parseFloat(currVal) == 0) {
                            isNoValue = 1;
                            if (parseInt(configTypeId) == VEHICLE_CONFIGURATION_TYPE_CONFIG.SEMI_TRAILER && compType == TypeConfiguration.SEMI_TRAILER &&
                                (compFieldId == "Left_Overhang" || compFieldId == "Right_Overhang")) {
                                isNoValue = 0;
                                maxVal = currValNew;
                            }
                        }
                        else if (isNoValue == 1) {
                            if (parseInt(configTypeId) == VEHICLE_CONFIGURATION_TYPE_CONFIG.SEMI_TRAILER && compType == TypeConfiguration.SEMI_TRAILER &&
                                (compFieldId == "Left_Overhang" || compFieldId == "Right_Overhang")) {
                                isNoValue = 0;
                                maxVal = currValNew;
                            }
                        }

                        if (compCount == TotalComp && isNoValue == 0) {
                            ShowUndoMessageForConfiguration(configElem, _this, maxVal);
                            $(configElem).val(maxVal != 0 ? maxVal : "");
                        }
                    }
                    else if ((compType == TypeConfiguration.RIGID_VEHICLE || compType == TypeConfiguration.DRAWBAR_TRAILER) && compFieldId == "Length") {
                        var len = compElemVal != "" ? compElemVal : 0;
                        var lenMtr = len;
                        maxValMtr = maxVal;
                        if ($('#UnitValue').val() == 692002) {
                            //overallLengthMtr = ConverteFeetToMetre(overallLength);
                            lenMtr = ConverteFeetToMetre(lenMtr);
                            configFieldVal = ConverteFeetToMetre(configFieldVal);
                            //overallLengthMtr = parseFloat(overallLengthMtr) + parseFloat(lenMtr);
                            //overallLength = ConvertMetreToFeet(overallLengthMtr);
                            maxValMtr = ConverteFeetToMetre(maxVal);
                        }
                        //else {
                        //    overallLength = parseFloat(overallLength) + parseFloat(len);
                        //}
                        if (parseFloat(lenMtr) == 0)
                            isNoValue = 1;

                        if (parseFloat(lenMtr) > maxValMtr)
                            maxVal = len;

                        if (compCount == TotalComp && isNoValue == 0) {
                            ShowUndoMessageForConfiguration(configElem, _this, maxVal);
                            $(configElem).val(maxVal != 0 ? maxVal : "");
                            //$('#OverallLength').val(overallLength != 0 ? overallLength : "");
                        }
                    }
                    else if ((compType == TypeConfiguration.CONVENTIONAL_TRACTOR || compType == TypeConfiguration.SEMI_TRAILER) && compFieldId == "Length") {
                        var currVal = compElemVal != "" ? compElemVal : 0;
                        var currValNew = currVal != 0 ? currVal : "";
                        maxValMtr = maxVal;
                        if ($('#UnitValue').val() == 692002) {
                            currVal = ConverteFeetToMetre(currVal);
                            configFieldVal = ConverteFeetToMetre(configFieldVal);
                            maxValMtr = ConverteFeetToMetre(maxVal);
                        }
                        if (parseFloat(currVal) > parseFloat(maxValMtr))
                            maxVal = currValNew;

                        if (parseFloat(currVal) > configFieldVal)
                            $(configElem).val(currValNew);
                        else if (currVal!=0) {
                            ShowUndoMessageForConfiguration(configElem, _this, maxVal);
                            $(configElem).val(maxVal != 0 ? maxVal : "");
                        }
                    }
                    else if (compFieldId == "Front_Overhang") {
                        if ($(configElem).val() != compElemVal) {
                            if (compCount == 1) {
                                ShowUndoMessageForConfiguration(configElem, _this, compElemVal);
                                $(configElem).val(compElemVal);
                            }
                        }
                    }
                    else if (compFieldId == "Rear_Overhang") {
                        if ($(configElem).val() != compElemVal) {
                            if (compCount == TotalComp) {
                                ShowUndoMessageForConfiguration(configElem, _this, compElemVal);
                                $(configElem).val(compElemVal);
                            }
                        }
                    }
                    else if (compFieldId == 'Number_of_Axles') {
                        if ($(configElem).hasClass("axledropVehicle_" + compCount)) {
                            ShowUndoMessageForConfiguration($(".axledropVehicle_" + compCount), _this, compElemVal);
                            $(".axledropVehicle_" + compCount).val(compElemVal);
                        }
                    }
                    else if ((compFieldId == 'txt_register' || compFieldId == 'txt_fleet') && IsTractor) {
                        if (compElemVal == "")
                            $(configElem).val("");
                    }
                    break;
                case VEHICLE_CONFIGURATION_TYPE_CONFIG.RIGID_AND_DRAG:
                case VEHICLE_CONFIGURATION_TYPE_CONFIG.RECOVER_VEHICLE:
                case VEHICLE_CONFIGURATION_TYPE_CONFIG.SPMT:
                    
                    var compType = $('#' + compId).find('#vehicleTypeValue').val();
                    if (compFieldId == "Weight" && compType != TypeConfiguration.CONVENTIONAL_TRACTOR && compType != TypeConfiguration.SEMI_TRAILER) {
                        var wyt = compElemVal != "" ? compElemVal : 0;
                        if (compWeight < parseFloat(wyt))
                            compWeight = parseFloat(wyt);

                        weight = parseFloat(weight) + parseFloat(wyt);
                        if (parseFloat(wyt) == 0)
                            isNoValue = 1;
                        if (compCount == TotalComp && isNoValue == 0) {
                            ShowUndoMessageForConfiguration(null, _this, null, configElem, weight);
                            $(configElem).val(weight != 0 ? weight : "");

                            if (VSONotificationVehicle != "True" && VSONotificationVehicle != "true") {
                                MovementAssessment();
                            }
                        }
                        IsGroundClearenceRequired(wyt, compId);

                    }
                    else if (compFieldId == "Maximum_Height" || compFieldId == "Reducable_Height" || compFieldId == "Width"
                        || compFieldId == "Left_Overhang" || compFieldId == "Right_Overhang") {
                        var currVal = compElemVal != "" ? compElemVal : 0;
                        var currValNew = currVal != 0 ? currVal : "";
                        maxValMtr = maxVal;
                        if ($('#UnitValue').val() == 692002) {
                            currVal = ConverteFeetToMetre(currVal);
                            configFieldVal = ConverteFeetToMetre(configFieldVal);
                            maxValMtr = ConverteFeetToMetre(maxVal);
                        }
                        if (parseFloat(currVal) > parseFloat(maxValMtr))
                            maxVal = currValNew;

                        if (parseFloat(currVal) == 0)
                            isNoValue = 1;
                        if (compCount == TotalComp && isNoValue == 0) {
                            ShowUndoMessageForConfiguration(configElem, _this, maxVal);
                            $(configElem).val(maxVal != 0 ? maxVal : "");
                        }
                    }
                    else if (compFieldId == "Length") {
                        var len = compElemVal != "" ? compElemVal : 0;
                        var lenMtr = len;
                        maxValMtr = maxVal;
                        if ($('#UnitValue').val() == 692002) {
                            //overallLengthMtr = ConverteFeetToMetre(overallLength);
                            lenMtr = ConverteFeetToMetre(lenMtr);
                            configFieldVal = ConverteFeetToMetre(configFieldVal);
                            //overallLengthMtr = parseFloat(overallLengthMtr) + parseFloat(lenMtr);
                            //overallLength = ConvertMetreToFeet(overallLengthMtr);
                            maxValMtr = ConverteFeetToMetre(maxVal);
                        }
                        //else {
                        //    overallLength = parseFloat(overallLength) + parseFloat(len);
                        //}

                        if (parseFloat(lenMtr) > parseFloat(maxValMtr))
                            maxVal = len;

                        if (parseFloat(lenMtr) == 0)
                            isNoValue = 1;

                        if (compCount == TotalComp && isNoValue == 0) {
                            ShowUndoMessageForConfiguration(configElem, _this, maxVal);
                            //$('#OverallLength').val(overallLength != 0 ? overallLength : "");
                            $(configElem).val(maxVal != 0 ? maxVal : "");
                        }
                    }
                    else if (compFieldId == "Front_Overhang") {
                        if ($(configElem).val() != compElemVal) {
                            if (compCount == 1) {
                                ShowUndoMessageForConfiguration(configElem, _this, compElemVal);
                                $(configElem).val(compElemVal);
                            }
                        }
                    }
                    else if (compFieldId == "Rear_Overhang") {
                        if ($(configElem).val() != compElemVal) {
                            if (compCount == TotalComp) {
                                ShowUndoMessageForConfiguration(configElem, _this, compElemVal);
                                $(configElem).val(compElemVal);
                            }
                        }
                    }
                    else if (compFieldId == 'Number_of_Axles') {
                        if ($(configElem).hasClass("axledropVehicle_" + compCount)) {
                            ShowUndoMessageForConfiguration($(".axledropVehicle_" + compCount), _this, compElemVal);
                            $(".axledropVehicle_" + compCount).val(compElemVal);
                        }
                    }
                    break;
                default:
                    break;
            }
            //});
        });
    }
}

function ShowToastMessage(_this) {
    var configTypeId = $("#ConfigTypeId").val();
    var configFieldId = $(_this).attr("id");
    var configFieldAxle = $(_this).attr("axleDropCount");
    var isLpConfig = false;
    if (configFieldId == 'txt_register_config' || configFieldId == 'txt_fleet_config') {
        configFieldId = configFieldId.replace("_config", "");//id is different in component section
        isLpConfig = true;
    }
    $('.comp').each(function () {
        var componentId = $(this).attr("id");
        var IsTractor = false;
        if ($('#' + componentId).find('#Tractor').val().toLowerCase() == "true") {
            IsTractor = true;
        }
        $('#' + componentId).find('.dynamic input,textarea:not(hidden,checkbox),#regDetailsConfig input:not(hidden)').each(function () {
            var compFieldId = $(this).attr("id");
            var compFieldAxle = $(this).attr("axleDropCount");
            var compElem = !isLpConfig ? $('#' + componentId + ' #' + compFieldId) : $('#div_reg_component_vehicle_' + componentId + ' #' + compFieldId);

            switch (parseInt(configTypeId)) {
                case VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER:
                    if (configFieldId == compFieldId) {
                        if ((compFieldId == "Length" || compFieldId == "Width" || compFieldId == "Rear_Overhang"
                            || compFieldId == "Right_Overhang" || compFieldId == "Left_Overhang" || compFieldId == "Weight")
                            && !IsTractor) {
                            ShowUndoMessage(compElem, _this);
                        }
                        else if ((compFieldId == "Front_Overhang") && IsTractor) {
                            ShowUndoMessage(compElem, _this);
                        }
                        else if ((compFieldId == 'txt_register' || compFieldId == 'txt_fleet') && IsTractor) {
                            ShowUndoMessage(compElem, _this);
                        }
                        else if (compFieldId == 'Number_of_Axles' && compFieldAxle == configFieldAxle) {
                            ShowUndoMessage(compElem, _this);
                        }
                    }
                    break;
                case VEHICLE_CONFIGURATION_TYPE_CONFIG.SEMI_TRAILER:
                    if (configFieldId == compFieldId) {
                        if ((compFieldId == "Length" || compFieldId == "Width" || compFieldId == "Maximum_Height"
                            || compFieldId == "Rear_Overhang" || compFieldId == "Right_Overhang" || compFieldId == "Left_Overhang"
                            || compFieldId == "Reducable_Height")
                            && !IsTractor) {
                            ShowUndoMessage(compElem, _this);
                        }
                        else if (compFieldId == "Front_Overhang" && IsTractor) {
                            ShowUndoMessage(compElem, _this);
                        }
                        else if ((compFieldId == 'txt_register' || compFieldId == 'txt_fleet') && IsTractor) {
                            ShowUndoMessage(compElem, _this);
                        }
                        else if (compFieldId == 'Number_of_Axles' && compFieldAxle == configFieldAxle) {
                            ShowUndoMessage(compElem, _this);
                        }
                    }
                    break;
                case VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER_3_TO_8:
                case VEHICLE_CONFIGURATION_TYPE_CONFIG.SEMI_TRAILER_3_TO_8:
                case VEHICLE_CONFIGURATION_TYPE_CONFIG.RIGID_AND_DRAG:
                case VEHICLE_CONFIGURATION_TYPE_CONFIG.BOAT_MAST_EXCEPTION:
                    if (configFieldId == compFieldId) {
                        if (compFieldId == "Rear_Overhang" && !IsTractor) {
                            ShowUndoMessage(compElem, _this);
                        }
                        else if (compFieldId == "Front_Overhang" && IsTractor) {
                            ShowUndoMessage(compElem, _this);
                        }
                        else if ((compFieldId == 'txt_register' || compFieldId == 'txt_fleet') && IsTractor) {
                            ShowUndoMessage(compElem, _this);
                        }
                        else if (compFieldId == 'Number_of_Axles' && compFieldAxle == configFieldAxle) {
                            ShowUndoMessage(compElem, _this);
                        }
                    }
                    break;
                default:
                    break;
            }
        });
    });
}

function ShowUndoMessage(compElem, _this, compCount, componentId) { // when components value got update by configuration value

    if (componentCurrentval[componentId] == undefined)
        componentCurrentval[componentId] = { fieldId: "", fieldVal: "" };
    if (componentCurrentval[componentId].fieldId != "Number_of_Axles") {
        componentCurrentval[componentId].fieldId = $(compElem).attr("id");
        componentCurrentval[componentId].fieldVal = $(compElem).val();
    }
    else {
        $(compElem).val(componentCurrentval.fieldVal);
    }
    if ($(compElem).val() != "" && $(compElem).val() != undefined && $(compElem).val() != $(_this).val()) {
        var compName = $(compElem).attr("name");
        var configName = $(_this).attr("name");
        if (compName == "Length") {
            compName = "Rigid Length";
            configName = "Rigid Length";
        }
        else if (compName == "Width") {
            configName = "Overall Width";
        }
        else if (compName == "Reducable Height") {
            compName = "Reducible Height";
            configName = "Reducible Height";
        }
        else if (compName == "Front Overhang") {
            compName = "Front Projection";
            configName = "Front Projection";
        }
        else if (compName == "Rear Overhang") {
            compName = "Rear Projection";
            configName = "Rear Projection";
        }
        else if (compName == undefined && componentCurrentval.fieldId == "txt_register") {
            compName = "Registration";
            configName = "Registration";
        }
        else if (compName == undefined && componentCurrentval.fieldId == "txt_fleet") {
            compName = "Fleet ID";
            configName = "Fleet ID";
        }
        if (parseInt($('#ConfigTypeId').val()) == VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER && configName == "Weight") {
            configName = "Heaviest Component Weight";
        }
        showMultiToastMessage({
            message: "Component" + compCount + "  " + compName + " field has been updated by the configuration " + configName + " field." + "<a id='undoCompChanges_" + compCount+"' class='undoCompChanges btn toast-btn btn-sm' href='#'>Undo</a>",
            type: "warning"
        });
        $('body').off('click', "#undoCompChanges_" + compCount);
        $('body').on('click', '#undoCompChanges_' + compCount, function (e) {
            RevertComponentChange(compElem);
        });
    }
}
function RevertComponentChange(compElem) {
    if (AxleTableMethods.AxlesItemsArrUndo && AxleTableMethods.AxlesItemsArrUndo.length > 0) {
        var componentId = AxleTableMethods.AxlesItemsArrUndo[0].componentId;
        for (var i = 0; i < AxleTableMethods.AxlesArr.length; i++) {
            var axleItem = AxleTableMethods.AxlesArr[i];
            if (axleItem.componentId == componentId) {
                AxleTableMethods.AxlesArr[i].axleItems=AxleTableMethods.AxlesArr[i].axleItems.concat(AxleTableMethods.AxlesItemsArrUndo);
                AxleTableMethods.AxlesItemsArrUndo = [];
                
            }
        }
    }
    var compId = $(compElem).closest('.comp').attr('id');
    var field = $(compElem).attr('id');
    $("#" + compId + " #" + field).val(componentCurrentval[compId].fieldVal);
    if (field == 'Number_of_Axles') {
        var axledropcount = $(compElem).attr('axledropcount');
        $("#VehicleConfigInfo .axledropVehicle[axledropcount='" + axledropcount + "']").val(configurationCurrentval.fieldVal);
    }
    else {
        $("#VehicleConfigInfo #" + field).val(configurationCurrentval.fieldVal);
    }
    AxleTableMethods.OnAxleTextBoxChange(compElem);
    $('#esdalToast').removeClass('show');//close toast
}

function CloseAxlePopUp() {
    $('#axleDetails').modal('hide');
    $("#overlay").hide();
}

function SetHeaderHeightForConfig(compId) {
    $('.comp').each(function () {
        var componentId = $(this).attr("id");
        var height = $('#' + componentId).find('.individualComponentAxle #tbl_Axle thead th:eq(0)').height();
        var paddingtop = $('#' + componentId).find('.individualComponentAxle #tbl_Axle thead th:eq(0)').css('padding-top') || "0px";
        var paddingbottom = $('#' + componentId).find('.individualComponentAxle #tbl_Axle thead th:eq(0)').css('padding-bottom') || "0px";
        var paddingtopvalue = paddingtop.replace('px', '');
        var paddingbottomvalue = paddingbottom.replace('px', '');
        var totalheight = parseFloat(height) + parseFloat(paddingtopvalue) + parseFloat(paddingbottomvalue);
        if (totalheight != 0) {
            $('.popUpAxleDetails .headgradBtn').attr('style', 'height:' + totalheight + 'px;color:none;');
            $('.popUpAxleDetails .headgrad').attr('style', 'height:' + totalheight + 'px;color:none;');
        }
    });

}

function ShowCurrentComponent(compId) {
    var itemToDIsplay = $('#compHeader-' + compId).data('elemtodisplay');
    $('.divComponentDataItem').removeClass('active');
    $('.component-item').hide();
    $('#compHeader-' + compId).addClass('active');
    $('#' + itemToDIsplay).fadeIn();
}

function ConfigAutoFillOnEdit(fromSave = false) {

    var isValid = true;
    var configTypeId = $("#ConfigTypeId").val();
    var IsTractor = false;
    var weight = 0;
    var compWeight = 0;
    var overallLength = 0;
    var overallLengthMtr = 0;
    var compCount = 0;
    var maxValMtr = 0;
    var wyt1 = 0;
    var wyt2 = 0;
    var isNoWeight = 0;
    var TotalComp = $('#ComponentCount').val();
    if (TotalComp != undefined && TotalComp != "")
        TotalComp = parseInt(TotalComp);
    if (TotalComp > 1) {
        var compValuesObj = {};
        $('.comp').each(function () {
            var compId = $(this).attr("id");
            compCount++;

            var isNoValue = 0;
            if ($('#' + compId).find('#Tractor').val().toLowerCase() == "true") {
                IsTractor = true;
            }
            else {
                IsTractor = false;
            }

            $('#' + compId).find('.dynamic input,textarea:not(hidden,checkbox),#regDetailsConfig input,.component_axle input').each(function () {
                var compFieldId = $(this).attr("id");
                var configElem = $('#div_config_general #' + compFieldId);
                var compElemVal = $('#' + compId + " #" + compFieldId).val() || '';
                var configFieldId = $(configElem).attr("id");
                var configFieldName = $(configElem).attr("name");
                if (configElem.length == 0) {
                    configElem = $('#config_registration_section #' + compFieldId + "_config");
                    configFieldId = compFieldId;
                }
                var configFieldVal = $(configElem).val();
                var maxVal = configFieldVal == "" ? 0 : configFieldVal;
                if (compValuesObj[compFieldId] == undefined)
                    compValuesObj[compFieldId] = { MaxValue: 0, IsBlankExist: false, OverallLength: 0 };
                if (compFieldId == configFieldId) {

                    switch (parseInt(configTypeId)) {
                        case VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER:
                        case VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER_3_TO_8:
                            if (compFieldId == "Weight") {
                                var wyt = $(this).val() != "" ? $(this).val() : 0;
                                if (compWeight < parseFloat(wyt))
                                    compWeight = parseFloat(wyt);


                                var wyt = $(this).val() != "" ? $(this).val() : 0;
                                weight = parseFloat(weight) + parseFloat(wyt);
                                if (parseFloat(wyt) == 0)
                                    compValuesObj[compFieldId].IsBlankExist = true;
                                if (compCount == TotalComp && compValuesObj[compFieldId].IsBlankExist == false) {
                                    $(configElem).val(compWeight != 0 ? compWeight : "");
                                    var oldTrainWeight = $('#TrainWeight').val();
                                    if (fromSave && oldTrainWeight != weight) {
                                        showMultiToastMessage({
                                            message: "Gross train weight doesn’t match with the sum of components weight values!",
                                            type: "error"
                                        });
                                        isValid = false;
                                        return;
                                    }
                                    else {
                                        $('#TrainWeight').val(weight != 0 ? weight : "")
                                    }
                                }
                            }
                            else if (compFieldId == "Maximum_Height" || compFieldId == "Reducable_Height" || compFieldId == "Width"
                                || compFieldId == "Left_Overhang" || compFieldId == "Right_Overhang") {
                                var configWidthVal = $('#Width').val();
                                var currVal = $(this).val() != "" ? $(this).val() : 0;
                                var currValNew = currVal != 0 ? currVal : "";
                                var ValInMtr = currValNew;
                                if ($('#UnitValue').val() == 692002) {
                                    currVal = ConverteFeetToMetre(currVal);
                                    configFieldVal = ConverteFeetToMetre(configFieldVal);
                                    ValInMtr = ConverteFeetToMetre(maxVal);
                                    ValInFeet = maxVal;
                                }
                                if (parseFloat(currVal) == 0)
                                    compValuesObj[compFieldId].IsBlankExist = true;

                                if (parseFloat(ValInMtr) > parseFloat(compValuesObj[compFieldId].MaxValue)) {
                                    compValuesObj[compFieldId].MaxValue = parseFloat(ValInMtr);
                                    
                                }

                                if (compFieldId == "Width" && configWidthVal != "") {
                                    compValuesObj[compFieldId].IsBlankExist = true;
                                }
                                if (compCount == TotalComp && compValuesObj[compFieldId].IsBlankExist == false) {
                                    var txtVal = compValuesObj[compFieldId].MaxValue != 0 ? compValuesObj[compFieldId].MaxValue : "";
                                    if ($('#UnitValue').val() == 692002) {
                                        txtVal = ConvertToFeet(txtVal);
                                    }
                                    if (fromSave && $(configElem).val() != "" && $(configElem).val() != txtVal) {
                                        configFieldUpdatedOnSave = true;
                                        updatedConfigFieldName = configFieldName;
                                    }
                                    $(configElem).val(txtVal);
                                }


                            }
                            else if (compFieldId == "Length") {
                                var len = $(this).val() != "" ? $(this).val() : 0;
                                var lenMtr = len;
                                var LengthValMtr = len;
                                if ($('#UnitValue').val() == 692002) {
                                    lenMtr = ConverteFeetToMetre(lenMtr);
                                    configFieldVal = ConverteFeetToMetre(configFieldVal);
                                    LengthValMtr = ConverteFeetToMetre(maxVal);
                                    LengthValFeet = maxVal;
                                }

                                if (parseFloat(LengthValMtr) > parseFloat(compValuesObj[compFieldId].MaxValue)) {
                                    compValuesObj[compFieldId].MaxValue = parseFloat(LengthValMtr);
                                    
                                }

                                if (parseFloat(lenMtr) == 0)
                                    compValuesObj[compFieldId].IsBlankExist = true;

                                if (compCount == TotalComp && compValuesObj[compFieldId].IsBlankExist == false) {
                                    if (fromSave && $(configElem).val() != "" && $(configElem).val() != compValuesObj[compFieldId].MaxValue) {
                                        configFieldUpdatedOnSave = true;
                                        updatedConfigFieldName = configFieldName;
                                    }
                                    var txtVal = compValuesObj[compFieldId].MaxValue != 0 ? compValuesObj[compFieldId].MaxValue : "";
                                    if ($('#UnitValue').val() == 692002) {
                                        txtVal = ConvertToFeet(txtVal);
                                    }
                                    $(configElem).val(txtVal);
                                    
                                }

                            }
                            else if (compFieldId == "Front_Overhang") {
                                if ($(configElem).val() != compElemVal && compElemVal != "0" && compCount == 1) {
                                    if (fromSave) {
                                        configFieldUpdatedOnSave = true;
                                        updatedConfigFieldName = configFieldName;
                                    }
                                    $(configElem).val(compElemVal);
                                }
                            }
                            else if (compFieldId == "Rear_Overhang") {
                                if ($(configElem).val() != compElemVal && compElemVal != "0" && compCount == TotalComp) {
                                    if (fromSave) {
                                        configFieldUpdatedOnSave = true;
                                        updatedConfigFieldName = configFieldName;
                                    }
                                    $(configElem).val(compElemVal);
                                }
                            }
                            else if (compFieldId == 'Number_of_Axles') {
                                if ($(configElem).hasClass("axledropVehicle_" + compCount)) {
                                    if (fromSave && $(".axledropVehicle_" + compCount).val() != $(this).val()) {
                                        configFieldUpdatedOnSave = true;
                                        updatedConfigFieldName = configFieldName;
                                    }
                                    $(".axledropVehicle_" + compCount).val($(this).val());
                                }
                            }
                            
                            break;
                        case VEHICLE_CONFIGURATION_TYPE_CONFIG.SEMI_TRAILER:
                        case VEHICLE_CONFIGURATION_TYPE_CONFIG.SEMI_TRAILER_3_TO_8:
                        case VEHICLE_CONFIGURATION_TYPE_CONFIG.BOAT_MAST_EXCEPTION:
                            var compType = $('#' + compId).find('#vehicleTypeValue').val();

                            if (compFieldId == "Weight" && compType != TypeConfiguration.CONVENTIONAL_TRACTOR && compType != TypeConfiguration.SEMI_TRAILER
                                && compType != TypeConfiguration.ENGINEERING_PLANT && compType != TypeConfiguration.ENGINEERING_PLANT_SEMI_TRAILER                            ) {
                                var wyt = $(this).val() != "" ? $(this).val() : 0;
                                if (compWeight < parseFloat(wyt))
                                    compWeight = parseFloat(wyt);

                                weight = parseFloat(weight) + parseFloat(wyt);
                                if (parseFloat(wyt) == 0)
                                    compValuesObj[compFieldId].IsBlankExist = true;
                                if (compCount == TotalComp && compValuesObj[compFieldId].IsBlankExist == false) {
                                    var oldWeight = $(configElem).val();
                                    if (fromSave && oldWeight != weight) {
                                        showMultiToastMessage({
                                            message: "Gross weight doesn’t match with the sum of components weight values!",
                                            type: "error"
                                        });
                                        isValid = false;
                                        return;
                                    }
                                    else {
                                        $(configElem).val(weight != 0 ? weight : "");
                                    }
                                }
                            }
                            else if (compFieldId == "Maximum_Height" || compFieldId == "Reducable_Height" || compFieldId == "Width"
                                || compFieldId == "Left_Overhang" || compFieldId == "Right_Overhang") {
                                var configWidthVal = $('#Width').val();
                                var currVal = $(this).val() != "" ? $(this).val() : 0;
                                var currValNew = currVal != 0 ? currVal : "";
                                var ValInMtr = currValNew;
                                if ($('#UnitValue').val() == 692002) {
                                    currVal = ConverteFeetToMetre(currVal);
                                    configFieldVal = ConverteFeetToMetre(configFieldVal);
                                    ValInMtr = ConverteFeetToMetre(currValNew);
                                    ValInFeet = currValNew;
                                }
                                if (parseFloat(ValInMtr) > parseFloat(compValuesObj[compFieldId].MaxValue)) {                                    
                                        compValuesObj[compFieldId].MaxValue = parseFloat(ValInMtr);
                                } 

                                if (parseFloat(currVal) == 0) {
                                    compValuesObj[compFieldId].IsBlankExist = true;
                                    if (parseInt(configTypeId) == VEHICLE_CONFIGURATION_TYPE_CONFIG.SEMI_TRAILER && compType == TypeConfiguration.SEMI_TRAILER &&
                                        (compFieldId == "Left_Overhang" || compFieldId == "Right_Overhang")) {
                                        compValuesObj[compFieldId].IsBlankExist = false;
                                    }
                                }
                                else if (compValuesObj[compFieldId].IsBlankExist == true) {
                                    if (parseInt(configTypeId) == VEHICLE_CONFIGURATION_TYPE_CONFIG.SEMI_TRAILER && compType == TypeConfiguration.SEMI_TRAILER &&
                                        (compFieldId == "Left_Overhang" || compFieldId == "Right_Overhang")) {
                                        compValuesObj[compFieldId].IsBlankExist = false;
                                    }
                                }
                                if (compFieldId == "Width" && configWidthVal != "") {
                                    compValuesObj[compFieldId].IsBlankExist = true;
                                }

                                if (compCount == TotalComp && compValuesObj[compFieldId].IsBlankExist == false) {
                                    var txtVal = compValuesObj[compFieldId].MaxValue != 0 ? compValuesObj[compFieldId].MaxValue : "";
                                    if ($('#UnitValue').val() == 692002) {
                                        txtVal = ConvertToFeet(txtVal);
                                    }
                                    if (fromSave && $(configElem).val() != "" && $(configElem).val() != txtVal) {
                                        configFieldUpdatedOnSave = true;
                                        updatedConfigFieldName = configFieldName;
                                    }
                                    $(configElem).val(txtVal);
                                }

                            }
                            else if ((compType == TypeConfiguration.RIGID_VEHICLE || compType == TypeConfiguration.DRAWBAR_TRAILER) && compFieldId == "Length") {
                                var len = $(this).val() != "" ? $(this).val() : 0;
                                var lenMtr = len;
                                var LengthValMtr = len;
                                if ($('#UnitValue').val() == 692002) {
                                    lenMtr = ConverteFeetToMetre(lenMtr);
                                    configFieldVal = ConverteFeetToMetre(configFieldVal);
                                    LengthValMtr = ConverteFeetToMetre(maxVal);
                                    LengthValFeet = maxVal;
                                }

                                if (parseFloat(lenMtr) == 0)
                                    compValuesObj[compFieldId].IsBlankExist = true;

                                if (parseFloat(LengthValMtr) > parseFloat(compValuesObj[compFieldId].MaxValue)) {
                                    compValuesObj[compFieldId].MaxValue = parseFloat(LengthValMtr);
                                    
                                }

                                if (compCount == TotalComp && compValuesObj[compFieldId].IsBlankExist == false) {
                                    if (fromSave && $(configElem).val() != "" && $(configElem).val() != compValuesObj[compFieldId].MaxValue) {
                                        configFieldUpdatedOnSave = true;
                                        updatedConfigFieldName = configFieldName;
                                    }
                                    var txtVal = compValuesObj[compFieldId].MaxValue != 0 ? compValuesObj[compFieldId].MaxValue : "";
                                    if ($('#UnitValue').val() == 692002) {
                                        txtVal = ConvertToFeet(txtVal);
                                    }
                                    $(configElem).val(txtVal);
                                    
                                }

                            }
                            else if ((compType == TypeConfiguration.CONVENTIONAL_TRACTOR || compType == TypeConfiguration.SEMI_TRAILER) && compFieldId == "Length") {
                                var len = $(this).val() != "" ? $(this).val() : 0;
                                var LengthValMtr = len;
                                if ($('#UnitValue').val() == 692002) {
                                    len = ConverteFeetToMetre(len);
                                    LengthValMtr = ConverteFeetToMetre(maxVal);
                                    LengthValFeet = maxVal;
                                }

                                if (parseFloat(LengthValMtr) > parseFloat(compValuesObj[compFieldId].MaxValue)) {
                                    compValuesObj[compFieldId].MaxValue = parseFloat(LengthValMtr);
                                    
                                }

                                if (parseFloat(LengthValMtr) == 0)
                                    compValuesObj[compFieldId].IsBlankExist = true;

                                if (compCount == TotalComp && compValuesObj[compFieldId].IsBlankExist == false) {
                                    if (compValuesObj[compFieldId].MaxValue != 0) {
                                        if (fromSave && $(configElem).val() != "" && $(configElem).val() != compValuesObj[compFieldId].MaxValue) {
                                            configFieldUpdatedOnSave = true;
                                            updatedConfigFieldName = configFieldName;
                                        }
                                        var txtVal = compValuesObj[compFieldId].MaxValue != 0 ? compValuesObj[compFieldId].MaxValue : "";
                                        if ($('#UnitValue').val() == 692002) {
                                            txtVal = ConvertToFeet(txtVal);
                                        }
                                        $(configElem).val(txtVal);
                                    }
                                }
                            }
                            else if (compFieldId == "Front_Overhang") {
                                if ($(this).val() != "0" && $(configElem).val() != $(this).val() && compCount == 1) {
                                    if (fromSave && $(configElem).val() != "" && $(configElem).val() != $(this).val()) {
                                        configFieldUpdatedOnSave = true;
                                        updatedConfigFieldName = configFieldName;
                                    }
                                    $(configElem).val($(this).val());
                                }
                            }
                            else if (compFieldId == "Rear_Overhang") {
                                if ($(this).val() != "0" && $(configElem).val() != $(this).val() && compCount == TotalComp) {
                                    if (fromSave && $(configElem).val() != "" && $(configElem).val() != $(this).val()) {
                                        configFieldUpdatedOnSave = true;
                                        updatedConfigFieldName = configFieldName;
                                    }
                                    $(configElem).val($(this).val());
                                }
                            }
                            else if (compFieldId == 'Number_of_Axles') {
                                if ($(configElem).hasClass("axledropVehicle_" + compCount)) {
                                    if (fromSave && $(".axledropVehicle_" + compCount).val() != $(this).val()) {
                                        configFieldUpdatedOnSave = true;
                                        updatedConfigFieldName = configFieldName;
                                    }
                                    $(".axledropVehicle_" + compCount).val($(this).val());
                                }
                            }
                            break;
                        case VEHICLE_CONFIGURATION_TYPE_CONFIG.RIGID_AND_DRAG:
                        case VEHICLE_CONFIGURATION_TYPE_CONFIG.RECOVER_VEHICLE:
                        case VEHICLE_CONFIGURATION_TYPE_CONFIG.SPMT:
                            var compType = $('#' + compId).find('#vehicleTypeValue').val();
                            if (compFieldId == "Weight" && compType != TypeConfiguration.CONVENTIONAL_TRACTOR && compType != TypeConfiguration.SEMI_TRAILER) {
                                var wyt = $(this).val() != "" ? $(this).val() : 0;
                                if (compWeight < parseFloat(wyt))
                                    compWeight = parseFloat(wyt);

                                var wyt = $(this).val() != "" ? $(this).val() : 0;
                                weight = parseFloat(weight) + parseFloat(wyt);
                                if (parseFloat(wyt) == 0)
                                    compValuesObj[compFieldId].IsBlankExist = true;
                                if (compCount == TotalComp && compValuesObj[compFieldId].IsBlankExist == false) {
                                    var oldWeight = $(configElem).val();
                                    if (fromSave && oldWeight != weight) {
                                        showMultiToastMessage({
                                            message: "Gross weight doesn’t match with the sum of components weight values!",
                                            type: "error"
                                        });
                                        isValid = false;
                                        return;
                                    }
                                    else {
                                        $('#div_config_general #Weight').val(weight != 0 ? weight : "");
                                    }
                                }
                            }
                            else if (compFieldId == "Maximum_Height" || compFieldId == "Reducable_Height" || compFieldId == "Width"
                                || compFieldId == "Left_Overhang" || compFieldId == "Right_Overhang") {
                                var configWidthVal = $('#Width').val();
                                var currVal = $(this).val() != "" ? $(this).val() : 0;
                                var currValNew = currVal != 0 ? currVal : "";
                                var ValInMtr = currValNew;
                                if ($('#UnitValue').val() == 692002) {
                                    currVal = ConverteFeetToMetre(currVal);
                                    configFieldVal = ConverteFeetToMetre(configFieldVal);
                                    ValInMtr = ConverteFeetToMetre(currValNew);
                                    ValInFeet = currValNew;
                                }
                                if (parseFloat(ValInMtr) > parseFloat(compValuesObj[compFieldId].MaxValue)) {
                                    compValuesObj[compFieldId].MaxValue = parseFloat(ValInMtr);
                                    
                                }

                                if (parseFloat(currVal) == 0)
                                    compValuesObj[compFieldId].IsBlankExist = true;
                                if (compFieldId == "Width" && configWidthVal != "") {
                                    compValuesObj[compFieldId].IsBlankExist = true;
                                }
                                if (compCount == TotalComp && compValuesObj[compFieldId].IsBlankExist == false) {
                                    var txtVal = compValuesObj[compFieldId].MaxValue != 0 ? compValuesObj[compFieldId].MaxValue : "";
                                    if (fromSave && $(configElem).val() != "" && $(configElem).val() != txtVal) {
                                        configFieldUpdatedOnSave = true;
                                        updatedConfigFieldName = configFieldName;
                                    }
                                    if ($('#UnitValue').val() == 692002) {
                                        txtVal = ConvertToFeet(txtVal);
                                    }
                                    $(configElem).val(txtVal);
                                }

                            }
                            else if (compFieldId == "Length") {
                                var len = $(this).val() != "" ? $(this).val() : 0;
                                var lenMtr = len;
                                var LengthValMtr = len;
                                if ($('#UnitValue').val() == 692002) {
                                    lenMtr = ConverteFeetToMetre(lenMtr);
                                    LengthValMtr = ConverteFeetToMetre(len);
                                    LengthValFeet = len;
                                }

                                if (parseFloat(LengthValMtr) > parseFloat(compValuesObj[compFieldId].MaxValue)) {
                                    compValuesObj[compFieldId].MaxValue = parseFloat(LengthValMtr);
                                    
                                }

                                if (parseFloat(lenMtr) == 0)
                                    compValuesObj[compFieldId].IsBlankExist = true;

                                if (compCount == TotalComp && compValuesObj[compFieldId].IsBlankExist == false) {
                                    if (fromSave && $(configElem).val() != "" && $(configElem).val() != compValuesObj[compFieldId].MaxValue) {
                                        configFieldUpdatedOnSave = true;
                                        updatedConfigFieldName = configFieldName;
                                    }
                                    var txtVal = compValuesObj[compFieldId].MaxValue != 0 ? compValuesObj[compFieldId].MaxValue : "";
                                    if ($('#UnitValue').val() == 692002) {
                                        txtVal = ConvertToFeet(txtVal);
                                    }
                                    $(configElem).val(txtVal);
                                    
                                }
                            }
                            else if (compFieldId == "Front_Overhang") {
                                if ($(this).val() != "0" && $(configElem).val() != $(this).val() && compCount == 1) {
                                    if (fromSave && $(configElem).val() != "" && $(configElem).val() != $(this).val()) {
                                        configFieldUpdatedOnSave = true;
                                        updatedConfigFieldName = configFieldName;
                                    }
                                    $(configElem).val($(this).val());
                                }
                            }
                            else if ($(this).val() != "0" && compFieldId == "Rear_Overhang") {
                                if ($(configElem).val() != $(this).val() && compCount == TotalComp) {
                                    if (fromSave && $(configElem).val() != "" && $(configElem).val() != $(this).val()) {
                                        configFieldUpdatedOnSave = true;
                                        updatedConfigFieldName = configFieldName;
                                    }
                                    $(configElem).val($(this).val());
                                }
                            }
                            else if (compFieldId == 'Number_of_Axles') {
                                if ($(configElem).hasClass("axledropVehicle_" + compCount)) {
                                    if (fromSave && $(".axledropVehicle_" + compCount).val() != $(this).val()) {
                                        configFieldUpdatedOnSave = true;
                                        updatedConfigFieldName = configFieldName;
                                    }
                                    $(".axledropVehicle_" + compCount).val($(this).val());
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            });
        });
    }
    return isValid;
}


function ShowUndoMessageForConfiguration(configElem,_this, updateVal, sumUpField, sumUpVal) { // when configuration value got update by components value
    
    configurationRevertCurrentval.fieldId = $(configElem).attr("id");
    configurationRevertCurrentval.fieldVal = $(configElem).val();
    configurationSumUpCurrentval.fieldId = $(sumUpField).attr("id");
    configurationSumUpCurrentval.fieldVal = $(sumUpField).val();

    if ($(configElem).val() != "" && $(configElem).val() != undefined && updateVal != null && $(configElem).val() != updateVal) {
        $('.multitoast').html('');
        var compName = $(_this).attr("name");
        var configName = $(configElem).attr("name");
        
        showMultiToastMessage({
            message: "Configuration " + configName + " field has been updated by component " + compName + " fields." + "<a class='undoConfigChanges btn toast-btn btn-sm' href='#'>Undo</a>",
            //message: "Configuration " + configName + " field has been updated by component " + compName + " fields value.",
            type: "warning"
        });
        $('body').off('click', ".undoConfigChanges");
        $('body').on('click', '.undoConfigChanges', function (e) {
            RevertConfigurationChange(configElem, _this, null);
        });
    }
    if ($(sumUpField).val() !="" && $(sumUpField).val() != undefined && $(sumUpField).val() != sumUpVal) {
        var compName = $(_this).attr("name");
        var configName = $(sumUpField).attr("name");

        showMultiToastMessage({
            message: "Configuration " + configName + " field has been updated by component " + compName + " fields." + "<a class='undoSumConfigChanges btn toast-btn btn-sm' href='#'>Undo</a>",
            //message: "Configuration " + configName + " field has been updated by sum of components " + compName + " field value.",
            type: "warning"
        });
        $('body').off('click', ".undoSumConfigChanges");
        $('body').on('click', '.undoSumConfigChanges', function (e) {
            RevertConfigurationChange(null, _this, sumUpField);
        });
    }
}

function RevertConfigurationChange(configElem, compElem, sumUpField) {
    if (AxleTableMethods.AxlesItemsArrUndo && AxleTableMethods.AxlesItemsArrUndo.length > 0) {
        var componentId = AxleTableMethods.AxlesItemsArrUndo[0].componentId;
        for (var i = 0; i < AxleTableMethods.AxlesArr.length; i++) {
            var axleItem = AxleTableMethods.AxlesArr[i];
            if (axleItem.componentId == componentId) {
                AxleTableMethods.AxlesArr[i].axleItems = AxleTableMethods.AxlesArr[i].axleItems.concat(AxleTableMethods.AxlesItemsArrUndo);
                AxleTableMethods.AxlesItemsArrUndo = [];

            }
        }
    }
    var compId = $(compElem).closest('.comp').attr('id');
    var field = $(compElem).attr('id');
    if (field == 'Number_of_Axles') {
        var axledropcount = $(compElem).attr('axledropcount');
        $("#VehicleConfigInfo .axledropVehicle[axledropcount='" + axledropcount + "']").val(configurationRevertCurrentval.fieldVal);
        $("#" + compId + " #" + field).val(configurationRevertCurrentval.fieldVal);
        $("#" + compId + " #" + field).attr("previousValue", configurationRevertCurrentval.fieldVal);
    }
    else {
        if (configElem != null)
            $(configElem).val(configurationRevertCurrentval.fieldVal);
        if (sumUpField != null)
            $(sumUpField).val(configurationSumUpCurrentval.fieldVal);

        var compfield = componentPreviousval.find((fieldId) => fieldId = field);
        var compVal = "";
        if (compfield != undefined)
            compVal = compfield.fieldVal;
        $("#" + compId + " #" + field).val(compVal);
        $("#" + compId + " #" + field).attr("previousValue", compVal);

    }

    AxleTableMethods.OnAxleTextBoxChange(compElem);
    $('#esdalToast').removeClass('show');//close toast
}

function ComponentsAutoFillOnEdit(fromSave = false) {
    componentCurrentval = {
        fieldId: "",
        fieldVal: ""
    };
    var componentCount = $('#ComponentCount').val();
    var TotalComp = componentCount;
    if (TotalComp != undefined && TotalComp != "")
        TotalComp = parseInt(TotalComp);
    if (componentCount != 1 && componentCount != "1") {

        var configTypeId = $("#ConfigTypeId").val();
        var compCount = 0;
        
        var widthTrailer;
        var widthTractor;
        $('.comp').each(function () {
            compCount++;
            var componentId = $(this).attr("id");
            var IsTractor = false;
            if ($('#' + componentId).find('#Tractor').val().toLowerCase() == "true") {
                IsTractor = true;
            }
            $('#' + componentId).find('.dynamic input,textarea:not(hidden,checkbox),#regDetailsConfig input:not(hidden)').each(function () {
                var compFieldVal = $(this).val();
                var compFieldId = $(this).attr("id");
                var compFieldAxle = $(this).attr("axleDropCount");
                var compFieldName = $(this).attr("name");
                var compElem = $('#' + componentId + ' #' + compFieldId) ;
                var weight = 0;

                var configElem = $('#div_config_general #' + compFieldId);
                var configFieldId = $(configElem).attr("id");
                var configFieldName = $(configElem).attr("name");
                var configFieldAxle = $('.axledropVehicle_' + compCount).attr("axleDropCount");
                if (configElem.length == 0) {
                    configElem = $('#config_registration_section #' + compFieldId + "_config");
                    configFieldId = compFieldId;
                }
                var configFieldVal = $(configElem).val();

                switch (parseInt(configTypeId)) {
                    case VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER:
                        if (configFieldId == compFieldId) {
                            if ((compFieldId == "Length" || compFieldId == "Rear_Overhang"
                                || compFieldId == "Right_Overhang" || compFieldId == "Left_Overhang" )
                                && !IsTractor) {
                                $(compElem).val(configFieldVal);
                            }
                            else if (compFieldId == "Weight") {
                                weight = configFieldVal;
                                IsGroundClearenceRequired(weight, componentId);
                            }
                            else if (compFieldId == "Width") {
                                if (!IsTractor) {
                                    widthTrailer = $(compElem).val() || 0;
                                    if (parseFloat(widthTractor) <= parseFloat(widthTrailer)
                                        || (parseFloat(configFieldVal) > parseFloat(widthTrailer) && parseFloat(configFieldVal) > parseFloat(widthTractor))) {
                                        if (widthTrailer != 0 && configFieldVal != widthTrailer) {
                                            compFieldUpdatedOnSave = true;
                                            updatedCompFieldName = "Component " + compCount + " " + compFieldName;
                                        }
                                        $(compElem).val(configFieldVal);
                                    }
                                }
                                else {
                                    widthTractor = $(compElem).val() || 0;
                                    if (parseFloat(configFieldVal) < parseFloat(widthTractor)) {
                                        if (widthTractor != 0) {
                                            compFieldUpdatedOnSave = true;
                                            updatedCompFieldName = "Component " + compCount + " " + compFieldName;
                                        }
                                        $(compElem).val(configFieldVal);
                                    }
                                }
                            }
                            else if ((compFieldId == "Front_Overhang") && IsTractor) {
                                $(compElem).val(configFieldVal);
                            }
                        }
                        break;
                    case VEHICLE_CONFIGURATION_TYPE_CONFIG.SEMI_TRAILER:
                        if (configFieldId == compFieldId) {
                            if ((compFieldId == "Length" || compFieldId == "Maximum_Height"
                                || compFieldId == "Rear_Overhang" || compFieldId == "Right_Overhang" || compFieldId == "Left_Overhang"
                                || compFieldId == "Reducable_Height")
                                && !IsTractor) {
                                $(compElem).val(configFieldVal);
                            }
                            else if (compFieldId == "Width") {
                                if (!IsTractor) {
                                    widthTrailer = $(compElem).val() || 0;
                                    if (parseFloat(widthTractor) <= parseFloat(widthTrailer)
                                        || (parseFloat(configFieldVal) >= parseFloat(widthTrailer) && parseFloat(configFieldVal) > parseFloat(widthTractor))) {
                                        if (widthTrailer != 0)
                                        $(compElem).val(configFieldVal);
                                    }
                                }
                                else {
                                    widthTractor = $(compElem).val() || 0;
                                    if (parseFloat(configFieldVal) < parseFloat(widthTractor)) {
                                        if (widthTractor != 0)
                                            ShowUndoMessage(compElem, _this, compCount, componentId);
                                        $(compElem).val(configFieldVal);
                                    }
                                }
                            }
                            else if (compFieldId == "Front_Overhang" && IsTractor) {
                                $(compElem).val(configFieldVal);
                            }
                        }
                        break;
                    case VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER_3_TO_8:
                    case VEHICLE_CONFIGURATION_TYPE_CONFIG.SEMI_TRAILER_3_TO_8:
                    case VEHICLE_CONFIGURATION_TYPE_CONFIG.RIGID_AND_DRAG:
                    case VEHICLE_CONFIGURATION_TYPE_CONFIG.BOAT_MAST_EXCEPTION:
                    case VEHICLE_CONFIGURATION_TYPE_CONFIG.RECOVER_VEHICLE:
                    case VEHICLE_CONFIGURATION_TYPE_CONFIG.SPMT:
                        if (configFieldId == compFieldId) {
                            if (compFieldId == "Width") {
                                widthTractor = $(compElem).val() || 0;
                                if (parseFloat(configFieldVal) < parseFloat(widthTractor)) {
                                    $(compElem).val(configFieldVal);
                                }
                            }
                            if (compFieldId == "Rear_Overhang" && compCount == TotalComp) {
                                $(compElem).val(configFieldVal);
                            }
                            else if (compFieldId == "Front_Overhang" && compCount == 1) {
                                $(compElem).val(configFieldVal);
                            }
                            else if ((parseInt(configTypeId) == VEHICLE_CONFIGURATION_TYPE_CONFIG.RIGID_AND_DRAG ||
                                parseInt(configTypeId) == VEHICLE_CONFIGURATION_TYPE_CONFIG.BOAT_MAST_EXCEPTION) &&
                                (compFieldId == "Left_Overhang" || compFieldId == "Right_Overhang") && compCount == TotalComp) {
                                $(compElem).val(configFieldVal);
                            }
                        }
                        break;
                    default:
                        break;
                }
            });
        });
    }
}