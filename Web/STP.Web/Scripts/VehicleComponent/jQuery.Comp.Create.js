var vehicleId = 0;
var isFromComponent = false;
function Comp_CreateInit() {
    ShowBackButton();
};
//Onchange function for vehicle type dropdownlist
function VehicleTypeChange(thisVal) {
    var vehicleTypeId = $(thisVal).val();
    $('#VehicleSubType option').remove();
    //$('#movementTypeId option').remove();

    $('#componentDetails').hide("Blind");
    $('#componentDetails').html("");
    if (vehicleTypeId == "") {
        $('#VehicleSubType').attr("disabled", "disabled");
        $('#movementTypeId').attr("disabled", "disabled");
    }
    else {
        FillVehicleSubType(vehicleTypeId);
    }
}
function VehicleSubTypeChange(thisVal) {
    
    var vehicleSubTypeId = $(thisVal).val();
    var vehicleTypeId = $('#ComponentType').val();
    //if (vehicleSubTypeId == "") {
    //    //$('#componentDetails').hide("Blind");
    //    //$('#componentDetails').html("");
    //}
    //else {        
    //    FillVehicleClassification(vehicleTypeId, vehicleSubTypeId)
    //}

    FillComponent(vehicleSubTypeId, vehicleTypeId, 0);
    $("#btn_cancel").show();
    //$("#ComponentType").attr("disabled", true);
    //$("#VehicleSubType").attr("disabled", true);
    $('#movementTypeId').attr("disabled", "disabled");
}
function FillVehicleClassification(vehicleTypeId,vehicleSubTypeId) {
    //$('#movementTypeId option:not(:first-child)').remove();
    $('#movementTypeId option').remove();
    var url = '../Vehicle/VehicleComponentMovementClassification';
    $.post(url, { componentTypeId: vehicleTypeId, componentSubTypeId: vehicleSubTypeId }, function (data) {
        
        var datalength = data.movementClassificationResult.length;
        $('#movementTypeId').removeAttr("disabled");
        if (datalength == 1) {
            //var vehicleTypeId = $('#ComponentType').val();
            //var vehicleSubTypeId = data.movementClassificationResult[0].SubCompType;
            $('#movementTypeId').append('<option value="' + data.movementClassificationResult[0].ClassificationId + '" selected="selected">' + data.movementClassificationResult[0].ClassificationName + '</option>');
            VehicleClassificationChange(data.movementClassificationResult[0].ClassificationId);
            //$('#VehicleSubType').attr("disabled", false);
            //FillGeneralData(vehicleSubTypeId, vehicleTypeId);
            //$("#btn_cancel").show();
            //$("#ComponentType").attr("disabled", true);
            //$("#VehicleSubType").attr("disabled", true);
        }

        else if (datalength > 1) {

            //$('#componentImage').attr("style", "display:none")
            $('#movementTypeId').append('<option value="0">Select Movement</option>');
            for (var i = 0; i < datalength; i++) {
                $('#movementTypeId').append('<option value="' + data.movementClassificationResult[i].ClassificationId + '">' + data.movementClassificationResult[i].ClassificationName + '</option>');
                //$('#movementTypeId').attr("disabled", false);

            }
            //if (isFromComponent) {

            //    $("#VehicleSubType option:contains(Wheeled load)").remove();
            //    $("#VehicleSubType option:contains(Recovered vehicle)").remove();
            //}
        }
        else {
            ShowInfoPopup("No Movement classification found");
        }

    });
}
function VehicleClassificationChange(thisVal) {
    
    var movementId = typeof thisVal == 'number' ? thisVal : $(thisVal).val();
    var vehicleSubTypeId = $('#VehicleSubType').val();
    var vehicleTypeId = $('#ComponentType').val();
    if (movementId == "" || movementId==0) {
        $('#componentDetails').hide("Blind");
        $('#componentDetails').html("");
    }
    else {
        
        if (CheckPreviousMovementId(movementId)) {
            FillComponent(vehicleSubTypeId, vehicleTypeId, movementId);
            $("#btn_cancel").show();
            $("#ComponentType").attr("disabled", true);
            $("#VehicleSubType").attr("disabled", true);
            $('#movementTypeId').attr("disabled", "disabled");
        }
        else {
            $('#movementTypeId').val(0);
            alert("Movement classification not match with the previous selected");
        }

    }
}
function FillComponent(vehicleSubType, vehicleTypeId, movementId) {

    var islast = false;
    var isNotify = $('#ISNotif').val();
    //if (!isFromComponent) {
    //    var configId = GetConfigurationId();

    //}
    var url = '../Vehicle/ComponentGeneralPage';
    $.post(url, { vehicleSubTypeId: vehicleSubType, vehicleTypeId: vehicleTypeId, movementId: movementId, isComponent: isFromComponent, isLastComponent:true, isNotify: isNotify }, function (data) {

        $('#componentDetails').html(data);
        $('#componentDetails').show("Blind");
        SetRangeForSpacing();
        suppressKey();
        IterateThroughText();
        $('#componentImage').attr("style", "display:block")
        var imgurl = "../Content/images/Common/MasterPage/componet_icons/" + $('#Imagename').val() + ".jpg";
        $('#componentImage img').attr("src", imgurl);
        $('#regDetails').show();

        //if ($(data).find('.AxleConfig').val() == 'False') {
            $('#componentBtn').show();
        //}
        if (vehicleTypeId == TypeConfiguration.DRAWBAR_TRAILER || vehicleTypeId== TypeConfiguration.SEMI_TRAILER)
            $('#regDetails').hide();
        else
            $('#regDetails').show();

        $('#axlePage').html('');
        $('#error_msg').text('');
    });

}
function CheckPreviousMovementId(movementId) {
    
    var prevMovementId = $('#prevMovementId').val();
    if (movementId != prevMovementId && prevMovementId != "") {
        return false
    }
    return true;
}
function FillVehicleType(movementId) {
  
    $('#VehicleType option:not(:first-child)').remove();
    var url = '../Vehicle/FillVehicleType';
    $.post(url, { movementId: movementId }, function (data) {
        var datalength = data.type.length;
        for (var i = 0; i < datalength; i++) {
            $('#VehicleType').append('<option value="' + data.type[i].ComponentTypeId + '">' + data.type[i].ComponentName + '</option>');
            $('#VehicleType').attr("disabled", false);
        }
        if (datalength == 1) {
            $('#VehicleType option:eq(1)').prop('selected', true);
            $('#VehicleType').change();
        }

        $('#regDetails').hide();
    });
}
function FillGeneralData(vehicleSubType, vehicleTypeId) {
    
    var islast = false;
    var isNotify = $('#ISNotif').val();
    //if (!isFromComponent) {
    //    var configId = GetConfigurationId();

    //}
    var url = '../Vehicle/ComponentGeneralPage';
    $.post(url, { vehicleSubTypeId: vehicleSubType, vehicleTypeId: vehicleTypeId,  isComponent: isFromComponent,  isNotify: isNotify }, function (data) {

        $('#componentDetails').html(data);
        $('#componentDetails').show("Blind");
        SetRangeForSpacing();
        if (isFromComponent == false) {
            //$('#div_general').find('#Speed').hide();
        }
        $('#componentImage').attr("style", "display:block")
        var imgurl = "../Content/images/Common/MasterPage/componet_icons/" + $('#Imagename').val() + ".jpg";
        $('#componentImage img').attr("src", imgurl);
        $('#regDetails').show();
        //$('#Axle_Spacing_To_Following').attr('range', '@ViewBag.minval,@ViewBag.maxval');
        //$('#selection').hide();
    });

}
function FillVehicleSubType(vehicleTypeId) {
    $('#VehicleSubType option:not(:first-child)').remove();

    $.ajax({
        async: false,
        type: "POST",
        url: '../Vehicle/FillVehicleSubType',
        dataType: "json",
        data: { vehicleTypeId: vehicleTypeId },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            var datalength = data.type.length;

            $('#componentImage').attr("style", "display:none")
            for (var i = 0; i < datalength; i++) {
                $('#VehicleSubType').append('<option value="' + data.type[i].SubCompType + '">' + data.type[i].SubCompName + '</option>');
                $('#VehicleSubType').attr("disabled", false);

            }
            if (isFromComponent) {
                $("#VehicleSubType option:contains(Wheeled load)").remove();
                $("#VehicleSubType option:contains(Recovered vehicle)").remove();
            }
            $('#VehicleSubType').val(data.defaultType);
            //$('#VehicleSubType').change();
            VehicleSubTypeChange($('#VehicleSubType'));

        },
        error: function (result) {
            stopAnimation();
        },
        complete: function () {
            stopAnimation();
        }
    });

    //var url = '../Vehicle/FillVehicleSubType';
    //$.post(url, { vehicleTypeId: vehicleTypeId }, function (data) {
    //    ;
    //    var datalength = data.type.length;

    //        $('#componentImage').attr("style", "display:none")
    //        $('#VehicleSubType').append('<option value="">Select</option>');
    //        for (var i = 0; i < datalength; i++) {
    //            $('#VehicleSubType').append('<option value="' + data.type[i].SubCompType + '">' + data.type[i].SubCompName + '</option>');
    //            $('#VehicleSubType').attr("disabled", false);
                       
    //        }
    //        if (isFromComponent) {

    //            $("#VehicleSubType option:contains(Wheeled load)").remove();
    //            $("#VehicleSubType option:contains(Recovered vehicle)").remove();
    //        }
            
    //});
}
function ShowBackButton() {
    if (typeof ShowBackBtnComponent == 'function') {
        ShowBackBtnComponent();
    }
}
//function to save the general data
function Save(_btn) {
    ;
    var _this = $(_btn);
    var _form = _this.closest("form");
    var _showComp = parseInt($('#ShowComponent').val());
    var configId = 0;

    var isFromConfig = $('#HiddenFromConfig').val();

    if (typeof IsConfigCreate == 'function') {
        //_showComp = IsConfigCreate();
        configId = GetConfigurationId();
    }
    var movementId = $('#Movement').val();
    var componentId = parseInt($('#ComponentType').val());
    var compSubId = parseInt($('#VehicleSubType').val());

    var componentName = $('#Internal_Name').val();
    var travelSpeed = $('#TravelSpeed').val();
    var speedUnit = $('#SpeedUnits').val();
    if (speedUnit == undefined || speedUnit == '') {
        speedUnit = null;
    }

    var myEntries = {
        moveClassification: { ClassificationId: movementId },
        vehicleCompType: { ComponentTypeId: componentId },
        VehicleComponentId: compSubId,
        VehicleParamList: [],
        TravellingSpeed: travelSpeed,
        TravellingSpeedUnit: speedUnit
    };
    var listArray = new Array();

    $('#' + componentId +' .dynamic input,textarea').not(':hidden,:checkbox').each(function () {
        var name = $(this).val();
        var model = $(this).attr("id");
        var datatype = $(this).attr("datatype");
        if ($('#UnitValue').val() == 692002) {
            if (IsLengthField(this)) {
                name = ConvertToMetres(name);
            }
        }
        myEntries.VehicleParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
    });


    var numberOfAxles = $('#' + componentId +' .axledrop').val();

    var configurableAxles = $('#' + componentId +' .AxleConfig').val();

    var axleModel = $('#' + componentId +' .axledrop').attr("id");
    var axleDatatype = "int";
    myEntries.VehicleParamList.push({ ParamModel: axleModel, ParamValue: numberOfAxles, ParamType: axleDatatype });

    $('#' + componentId + '.dynamic input:checkbox').each(function () {
        var name = $(this).val();
        var model = $(this).attr("id");
        var value = 0;

        if ($(this).is(':checked')) {
            value = 1;
        }
        var datatype = "bool";

        myEntries.VehicleParamList.push({ ParamModel: model, ParamValue: value, ParamType: datatype });
    });

    $.ajax({
        url: _form.attr("action"),
        data: '{"vehicleComponent":' + JSON.stringify(myEntries) + ',"showComponent":' + _showComp + ',"vehicleConfigId":' + configId + ',"isFromConfig":' + isFromConfig + '}',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        async: false,
        success: function (data) {
            ;
            if (data.configId != '') {
                vehicleId = data.configId; // check from here
            }
            if (data.Success != 0) {
                ;
                $('#Component_Id').val(data.Success);

                $('#chars_left').hide();
                $('#HiddenFormalName').val(componentName);

                SaveComponentRegistration(_form);
            }
            else {
                ;
                $("#chars_left").html("Internal Name already exist");
                $('#vehicles')[0].scrollIntoView();
                //var FN = document.getElementById("Internal_Name");
                //FN.focus();
            }
        },
        error: function (data) {
            ;
            //alert("error");
            ShowErrorPopup("error");
        }
    });
}
//function to update vehicle component
function UpdateData(_btn) {
   
    var _this = $(_btn);
    var _form = _this.closest("form");
    var isFromConfig = $('#IsFromConfig').val() == "" ? 0 : $('#IsFromConfig').val();
    var componentId = $(_btn)[0].id;

    var componentName = $(_btn).find('#Internal_Name').val();

    var configId = $('#vehicleConfigId').val() == "" ? 0 : $('#vehicleConfigId').val();

    var myEntries = {
        VehicleParamList: []

    };

    var listArray = new Array();

    var isvalid = true;
    $('#' + componentId + ' .dynamic input,textarea').not(':hidden,:checkbox').each(function () {
        isvalid = ComponentKeyUpValidation(this);
        if (!isvalid)
            return isvalid;

        var name = $(this).val();
        var model = $(this).attr("id");
        var datatype = $(this).attr("datatype");
        if ($('#UnitValue').val() == 692002) {
            if (IsLengthField(this)) {
                name = ConvertToMetres(name);
            }
        }
        myEntries.VehicleParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
    });
    if (isvalid) {
        var numberOfAxles = $('.axledrop').val();
        var configurableAxles = $('.AxleConfig').val();
        var axleModel = $('.axledrop').attr("id");
        var axleDatatype = "int";
        myEntries.VehicleParamList.push({ ParamModel: axleModel, ParamValue: numberOfAxles, ParamType: axleDatatype });

        $('#' + componentId + ' .dynamic input:checkbox').each(function () {

            var name = $(this).val();
            var model = $(this).attr("id");
            var value = 0;

            if ($(this).is(':checked')) {
                value = 1;
            }
            var datatype = "bool";

            myEntries.VehicleParamList.push({ ParamModel: model, ParamValue: value, ParamType: datatype });
        });

        //return validation();
        var vhclType = $('#vehicleTypeValue').val();
        var vhclIntend = $('#movementTypeId').val();
        vehicleId = $('#vehicleId').val();
        if (vehicleId == undefined) {
            vehicleId = null;
        }


        //-------- Registration details-------
        $(_this).attr('disabled', 'disabled');
        if (!validatData(_this)) {
            $(_this).attr('disabled', false);
            return false;
        }
        if ($(_this).closest('tr').next('tr').length != 0) {
            $(_this).attr('disabled', false);
            return false;
        }
        var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
        var isVR1 = $('#vr1appln').val();
        var isNotify = $('#ISNotif').val();


        var registrationParams = [];
        $('#' + componentId + ' .tbl_registration tbody .tr_Registration').each(function () {
            var regVal = $(this).find('.txt_register').val();
            if (regVal == "") { regVal = $(this).find('.txt_register').html(); }
            var fleetId = $(this).find('.txt_fleet').val();
            if (fleetId == "") { fleetId = $(this).find('.txt_fleet').html(); }
            if (fleetId != "" || regVal != "") {
                registrationParams.push({ RegistrationValue: regVal, FleetId: fleetId });
            }

        });

        //--------------- Axle Details -----------------

        AxleValidationCalculation($('#' + componentId).find('#Weight').val(), componentId);

        var axleweightsum = 0;
        $('#' + componentId + ' #Config-body').find('.axleweight').each(function () {
            var _thisVal = $(this).val();
            axleweightsum = axleweightsum + parseFloat(_thisVal);
        });

        if (axleweightsum == 0) {
            $('#' + componentId + ' #tbl_Axle').find('.axleweight').each(function () {
                var _thisVal = $(this).val();
                axleweightsum = axleweightsum + parseFloat(_thisVal);
            });
        }

        var result = checkaxle_weight(axleweightsum, componentId);

        var isNotify = $('#ISNotif').val();
        var vehicleClassCode = $('#MovementClassification').val();
        var excessAxleWgt = false;
        var axlesList = []; //Axle list
        //ValidateAxles() 
        if (result && !excessAxleWgt) {
            var i = 1;
            var unitvalue = $('#UnitValue').val();
            var wb = 0;
            axlesList = [];

            $('#' + componentId + ' #tbl_Axle tbody tr').each(function () {
                var axleNum = i;
                var noOfWheels = $(this).find('.nowheels').val();
                var axleWeight = $(this).find('.axleweight').val();
                var distanceToNxtAxl = $(this).find('.disttonext').val();

                //to check if distance tonext axle value is definedor not
                if (typeof distanceToNxtAxl !== "undefined") {
                    if (unitvalue == 692002) {
                        distanceToNxtAxl = ConvertToMetres(distanceToNxtAxl);
                    }
                    wb = parseFloat(wb) + parseFloat(distanceToNxtAxl);
                }

                var tyreSize = $(this).find('.tyresize').val();

                var tyreSpace = null;
                $(this).find('.cstable input:text').each(function () {
                    var _thistxt = $(this).val();
                    //if (_thistxt != undefined) {
                    //    if (unitvalue == 692002) {
                    //        _thistxt = ConvertToMetres(_thistxt);
                    //    }
                    //}
                    if (tyreSpace != null) {
                        tyreSpace = tyreSpace + "," + _thistxt;
                    }
                    else {
                        tyreSpace = _thistxt;
                    }
                });

                axlesList.push({ AxleNumId: axleNum, NoOfWheels: noOfWheels, AxleWeight: axleWeight, DistanceToNextAxle: distanceToNxtAxl, TyreSize: tyreSize, TyreCenters: tyreSpace });
                i++;
            });
            var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
            var isVR1 = $('#vr1appln').val();
            var isNotify = $('#ISNotif').val();
            var validwb = true;
            var axlecount = $('#' + componentId + ' .axledrop').val();  //added for crane workflow changes
            if (isNotify == 'True' || isNotify == 'true') {
                var vc = $('#' + componentId + ' #VehicleClass').val();
                var catAwb = 1.5 * axlecount + 0.5 * (axlecount % 2);
                if (vc == 241006) {
                    if (parseFloat(wb) < parseFloat(catAwb)) {
                        $('#' + componentId + ' .error').text('Wheelbase should be greater than or equal to ' + catAwb);
                        validwb = false;
                    }
                }
                isVR1 = 'True';
            }

            //--------------------------------------
            var vehicleEdit = false;
            var planMovement = false;
            var isCandidate = false;
            if ($('#IsMovement').val() == "True" || $('#isMovement').val() == "true") {
                planMovement = true;
            }
            if ($('#IsCandidate').val() == "True" || $('#IsCandidate').val() == "true") {
                isCandidate = true;
            }

            $.ajax({
                url: '../Vehicle/UpdateComponent',
                data: '{"vehicleComponent":' + JSON.stringify(myEntries) + ',"componentId":' + componentId + ',"registrationParams":' + JSON.stringify(registrationParams) + ',"axleList":' + JSON.stringify(axlesList) + ',"vehicleConfigId":' + vehicleId + ',"vehicleType":' + vhclType + ',"vehicleIntend":' + vhclIntend + ',"IsFromConfig":' + isFromConfig + ',"planMovement":' + planMovement + ',"isCandidate":' + isCandidate + '}',
                type: 'POST',
                //contentType: 'application/json; charset=utf-8',
                async: false,
                beforeSend: function () {
                    startAnimation();
                },
                success: function (data) {

                    if (data.configId != '') {
                        vehicleId = data.configId; // check from here
                        vehicleEdit = true;
                    }
                    if (data.result) {
                        isFromConfigFlag = data.isFromConfigFlag;
                        var isMovement = $('#IsMovement').val();

                        if (isCandidate) {
                            ShowSuccessModalPopup('Component  "' + componentName + '"  updated successfully', "EditComponents", configId);
                        }
                        else if ($('#IsFromConfig').val() == '1' || isMovement == "True") {
                            var guid = data.Guid;
                            if (guid == "") {
                                guid = $('#GUID').val();
                            }
                            var param1 = guid;
                            var param2 = vehicleEdit;
                            ShowSuccessModalPopup('Component  "' + componentName + '"  updated successfully', "FillComponentDetailsForConfig", param1, param2);
                        }
                        else {
                            ShowSuccessModalPopup('Component  "' + componentName + '"  saved successfully', "LoadVehicleComponentList");
                        }
                        $('#chars_left').hide();
                        $('#HiddenFormalName').val(componentName);
                    }
                    else {

                        $('#chars_left').show();
                        var IN = document.getElementById("Internal_Name");
                        IN.focus();
                        $('.err_formalName').text('Name already exists');
                    }
                },
                error: function (data) {
                    showWarningPopDialog('Component  "' + componentName + '"  not saved successfully', 'Ok', '', 'ReloadLocation', '', 1, 'error');
                },
                complete: function () {
                    stopAnimation();
                }
            });

        }
        else {
            $("#btn_comp_finish").removeAttr("disabled");

            var container = $('div');
            var scrollTo = $(".error:not(:contains(' '))");

            $('html, body').animate({
                scrollTop: scrollTo.offset().top - container.offset().top + container.scrollTop()
            });
        }
    }
}
//  save component registration details
function SaveComponentRegistration(_this) {
    ;
    $(_this).attr('disabled', 'disabled');
    if (!validatData(_this)) {
        $(_this).attr('disabled', false);
        return false;
    }
    if ($(_this).closest('tr').next('tr').length != 0) {
        $(_this).attr('disabled', false);
        return false;
    }
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var isVR1 = $('#vr1appln').val();
    var isNotify = $('#ISNotif').val();

    if (isNotify == 'True' || isNotify == 'true') {

        AddVR1CompRegistration(_this);
    }
    else if (ApplicationRevId == 0 && !isVR1) {
        SaveRegistration(_this);
    }
    else if (isVR1 == 'True' || isVR1 == 'true') {
        AddVR1CompRegistration(_this);
    }
    else {
        AddAppCompRegistration(_this);
    }

    //});

}
function SaveRegistration(_this) {

    var registrationParams = [];
    var makeconfig = false;
    var vehicleConfigId = 0;
    if ($('#MakeConfig').is(':checked')) {
        makeconfig = true;
        vehicleConfigId = vehicleId;
    }
    var result = false;
    //vehicleId
    var componentId = $('#Component_Id').val();
    var isFromConfig = $('#IsFromConfig').val();
    if (isFromConfig == "") { isFromConfig = 0; }
    registrationParams = [];
    $('#' + componentId + ' .tbl_registration tbody .tr_Registration').each(function () {
        ;
        var regVal = $(this).find('.txt_register').val();
        if (regVal == "") { regVal=$(this).find('.txt_register').html();}
        var fleetId = $(this).find('.txt_fleet').val();
        if (fleetId == "") { fleetId= $(this).find('.txt_fleet').html(); }
        if (fleetId != "" || regVal != "") {
            registrationParams.push({ RegistrationValue: regVal, FleetId: fleetId });
        }

    });
    ;

    if (registrationParams.length>0) {

        $.ajax({
            url: '../Vehicle/SaveRegistrationID',
            data: '{"compId":' + componentId + ',"registrationParams":' + JSON.stringify(registrationParams) + ',"vehicleConfigId":' + vehicleConfigId + ',"isFromConfig":' + isFromConfig + '}',
            type: 'POST',
            //contentType: 'application/json; charset=utf-8',
            async: false,
            success: function (data) {
                if (data.Success != 0) {
                    SaveComponentAxle(componentId);
                    }
            },
            error: function (data) {
                ;
            }
        });
    }
    else {
        SaveComponentAxle(componentId);
    }
}
function SaveComponentAxle(componentId) {
    ;
    var axleweightsum = 0;
    var configFlag = $("#hiddenConfigFlag").val();
    $('#Config-body').find('.axleweight').each(function () {
        var _thisVal = $(this).val();
        axleweightsum = axleweightsum + parseFloat(_thisVal);
    });

    if (axleweightsum == 0) {
        $('#'+componentId+' #div_component_general_page').find('.axleweight').each(function () {
            var _thisVal = $(this).val();
            axleweightsum = axleweightsum + parseFloat(_thisVal);
        });
    }
    var result = checkaxle_weight(axleweightsum);

    var isNotify = $('#ISNotif').val();
    var vehicleClassCode = $('#MovementClassification').val();
    var excessAxleWgt = false;
    if (result && isNotify) {
        $('#div_component_general_page').find('.axleweight').each(function () {
            var currentAxleWgt = $(this).val();

            // for STGO AIL Cat 1 notifcation && for stgo mobile crane cat a
            if ((vehicleClassCode == 241003 || vehicleClassCode == 241006) && parseFloat(currentAxleWgt) > 11500) {
                excessAxleWgt = true;
                $('.error').text('Max axle weight should be less than or equal to 11500 kg');
            }
            // for STGO AIL Cat 2 notifcation && for stgo mobile crane cat b
            else if ((vehicleClassCode == 241004 || vehicleClassCode == 241007) && parseFloat(currentAxleWgt) > 12500) {
                excessAxleWgt = true;
                $('.error').text('Max axle weight should be less than or equal to 12500 kg');
            }
            // for STGO AIL Cat 3 notifcation && for stgo mobile crane cat c
            else if ((vehicleClassCode == 241005 || vehicleClassCode == 241008) && parseFloat(currentAxleWgt) > 16500) {
                excessAxleWgt = true;
                $('.error').text('Max axle weight should be less than or equal to 16500 kg');
            }
        });
    }
    if (ValidateAxles() && result && !excessAxleWgt) {
        GetDataToSave();
    }
    else {
        var componentId = $('#Component_Id').val();
        var componentName = $('#Internal_Name').val();
        ShowModalPopup('Component  "' + componentName + '"  saved successfully');
        if (configFlag == 1) {
            ;
            ImportComponent(componentId);
        }
        else {
            location.reload();
        }
    }
}
function showNextButton(compId) {
    var comp = $('#ComponentCount').val();
    if (comp > 0) {
        $('#btn_Next').show();
        $("#componentImage").show();
    }
    $('.btn_comp_finish').hide();
    $("#btn_cancel_axle").hide();
    //var vars = [], hash;
    //var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    //for (var i = 0; i < hashes.length; i++) {
    //    hash = hashes[i].split('=');
    //    vars.push(hash[0]);
    //    vars[hash[0]] = hash[1];
    //}
    //$('.btn_comp_finish').hide();
    //$("#btn_cancel_axle").hide();
    //if (vars[0] == "componentId" || vars[0] == "vehicleId" || vars[0] == "Guid") {
    //    if (vars.Guid != -1 && compId!=0) {
    //        $('#btn_Next').show();
    //        $("#componentImage").show();
    //    }
    //}
}
function DeleteConfigComponent(compId) {    
    var compName = $("#" + compId).find("#Formal_Name").val();
    var msg = 'Do you want to delete ' + compName;
    ShowWarningPopup(msg, 'DeleteConfigConfirmComponent', '', compId);

}
function DeleteConfigConfirmComponent(compId) {
    
    var compName = $("#" + compId).find("#Formal_Name").val();
    var vchId = $('#vehicleConfigId').val() == "undefined" ? 0 : $('#vehicleConfigId').val();
    var planMvmt = false;
    if ($('#IsMovement').val() == "True" || $('#IsMovement').val() == "true" || $('#isMovement').val() == "true") {
        planMvmt = true;
    }
    $.ajax({
        async: false,
        type: "POST",
        url: '../VehicleConfig/DeleteComponentConfiguration',
        dataType: "json",
        //contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ componentId: compId, vehicleId: vchId, isMovement: planMvmt}),
        beforeSend: function () {
            CloseWarningPopup();
            startAnimation();
        },
        processdata: true,
        success: function (data) {
            if (planMvmt) {
                var params = data.guid + "','" + data.vehicleId;
                ShowSuccessModalPopup('Component deleted successfully', "FillComponentDetailsForConfig", params);
            }
            else {
                window.location = '/VehicleConfig/CreateConfiguration' + EncodedQueryString('Guid=' + data.guid + '&vehicleConfigId=' + vehicleId);
            }            
        },
        error: function (result) {            
            ShowErrorPopup("Deletion failed");
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function EditVehicleComponent(componentId) {
    
    var component = $('#' + componentId);   
    var componentId = componentId;
    var unit = $('#UnitValue').val();
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var isVR1 = $('#vr1appln').val();
    var isNotify = $('#ISNotif').val();
    if (isNotify == 'True' || isNotify == 'true') {
        isVR1 = 'True';
    }
    var vd = validation(componentId);
    if (vd) {
        if (componentId == '') {
            Save(this);
        }
        else {
            UpdateData(component);
        }       
    }
    return false;
}
function AxleValidationCalculation(weight,compId) {
    
    if (weight == "") {
        weight = null;
    }
    $.ajax({
        url: '../Vehicle/AxleValidationCalculation',
        type: 'POST',
        async: false,
        data: { "weight": weight },
        success: function (response) {
            $('#' + compId).find('#axleweightRange').val(response);
        },
        error: function (response) {
        }
    });
}