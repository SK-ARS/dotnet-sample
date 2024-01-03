//vehicle Configuration Id to set
var vehicleConfigurationId = 0;
var ApplnMovementId = 0;
var vehicleName;
var appvehicleName;
var istractorfrnt;
var vehicleTypeId;
var movementId;
var numRows;
var vehcileCompNum;
//var vehicleComponentList = [{ComponentTypeId:null,ComponentName:null}];
var vehicleComponentList = [];
var vehcileCompTypeList = [];
var TotalData = [];
//global vehicle component list to maintain the prefilled data
var globalVehicleList = [];
//global variable to set lat-long pos
var vhclLatLongPos = [];
var fromsidebyside = false;
var routepartid = 0;
$(function () {    
    clearAllErrorSpan();
    $('body').on('click', '.btn-candidate-vehicle-back-button', function (e) {
        CandidateVehicleBackButton();
    });
});
//Onchange function for movement classification dropdownlist
function MovementChangeConfig(thisVal) {
    var movementId = $(thisVal).val();
   
    $('#VehicleSubType option').remove();
    $('#componentDetailsConfig').hide("Blind");
    $('#componentDetailsConfig').html("");

    //component number dropdown is hidden here
    $('#div_componentNum').hide();

    DisableSideBySideDrop();
    HideSideBySideDrop();
    DeselectSideBySide();
    HideSideBySideDiv();
    RemoveSideBySideTable();

    HideRowComponent();

    SetLongLatNull();

    fromsidebyside = false;

    if (movementId == "") {
        $('#VehicleTypeConfig option').not('option:eq(0)').remove();
        $('#VehicleTypeConfig').attr("disabled", "disabled");
        $('#VehicleSubType').attr("disabled", "disabled");
    }
    else {
        FillVehicleTypeConfig(movementId);
    }
}
//Onchange function for vehicle type dropdownlist
function VehicleTypeChangeConfig(thisVal) {
    vehicleTypeId = $(thisVal).val();

    movementId = $('#MovementClassConfig').val();

    var rangemin = $('option:selected', thisVal).attr('rangemin');
    var rangemax = $('option:selected', thisVal).attr('rangemax');

    istractorfrnt = $('option:selected', thisVal).attr('istractorfrnt');
    var numtractor = $('option:selected', thisVal).attr('numtractor');
    var numtrailer = $('option:selected', thisVal).attr('numtrailer');

    var sidebyside = $('option:selected', thisVal).attr('sidebyside');
    //to get the list of vehicle component
    vehcileCompNum = $('option:selected', thisVal).attr('listvhclcompnum');
    $('#ComponentNum').val(vehcileCompNum);

    DisableSideBySideDrop();
    HideSideBySideDrop();
    DeselectSideBySide();
    HideSideBySideDiv();
    RemoveSideBySideTable();

    HideRowComponent();

    SetLongLatNull();

    fromsidebyside = false;

    $('#div_componentNum').hide();

    if (vehicleTypeId == "") {
        //$('#VehicleSubType').attr("disabled", "disabled");                 
    }
    else {
        if (rangemin == rangemax) {
            FillImage();
            //loadGeneralPage(movementId, vehicleTypeId);
            $('#div_componentNum').hide();
        }
        else {
            if (sidebyside == 0) {
                fillVehicleCompNumSelect(rangemin, rangemax); //if no side by side required, component number is displayed
            }
            else {
                //side by side required
                FillVhclCompSidebySideSelect();
            }
            $('#componentDetailsConfig').html('');
        }
        GetGlobalVehicleList();
        //FillVehicleSubType(movementId, vehicleTypeId);
    }
    $('#div_RowComp_main').hide();
}
//Onchange function for vehicle subtype dropdownlist
function VehicleSubTypeChangeConfig(thisVal) {
    var vehicleSubTypeId = $(thisVal).val();
    var movementId = $('#MovementClassConfig').val();
    var vehicleTypeId = $('#VehicleTypeConfig').val();
    if (vehicleSubTypeId == "") {
        $('#componentDetailsConfig').hide("Blind");
        $('#componentDetailsConfig').html("");
    }
    else {
        //FillGeneralData(vehicleSubTypeId,vehicleTypeId, movementId);
    }
}
//function to fill vehicle type on movement classification dropdown change
function FillVehicleTypeConfig(movementId) {
    //$('#VehicleTypeConfig option:not(:first-child)').remove();
    //var url = '@Url.Action("FillVehicleConfigType", "VehicleConfig")';
    var url = '../VehicleConfig/FillVehicleConfigType';
    $.post(url, { movementId: movementId }, function (data) {
        TotalData = data.result;
        //console.log(TotalData);
        $('#VehicleTypeConfig option').remove();
        $('#VehicleTypeConfig').append('<option value="">Select Configuration Type</option>');
        vehcileCompTypeList = [];
        var datalength = data.result.length;
        for (var i = 0; i < datalength; i++) {
            var vehicleCompList = [];
            //exceptional cases add image
            if (movementId == 270003) {
                data.result[i].LstVehcCompTypes[0].ImageName = "crane";
            }
            else if (movementId == 270005) {
                data.result[i].LstVehcCompTypes[0].ImageName = "recoveryvehicle";
                data.result[i].LstVehcCompTypes[1].ImageName = "recoveredvehicle";
            }


            //-------------------------------
            var vhclCompTypeLength = data.result[i].LstVehcCompTypes.length;
            vehcileCompTypeList.push(data.result[i].LstVehcCompTypes);
            globalVehicleList.push(data.result[i].LstVehcCompTypes);
            //console.log(data.result[i]); 

            //console.log(data.result[5].LstVehcCompTypes);



            var numtractor = 0;
            var numtrailor = 0;
            //to get the number of tractors and trailors count
            for (var cnt = 0; cnt < vhclCompTypeLength; cnt++) {
                vehicleCompList.push({ ComponentTypeId: data.result[i].LstVehcCompTypes[cnt].ComponentTypeId, ComponentName: data.result[i].LstVehcCompTypes[cnt].ComponentName, IsTractor: data.result[i].LstVehcCompTypes[cnt].IsTractor, ImageName: data.result[i].LstVehcCompTypes[cnt].ImageName });
                //console.log(data.result[i].LstVehcCompTypes[cnt]);
                if (data.result[i].LstVehcCompTypes[cnt].IsTractor) {
                    numtractor++;
                }
                else {
                    numtrailor++;
                }
            }
            vehicleComponentList = vehicleCompList;
            //globalVehicleList = vehcileCompTypeList;
            $('#VehicleTypeConfig').append('<option value="' + data.result[i].ConfigurationTypeId + '" rangeMin="' + data.result[i].ComponentsRange.MinValue + '" rangeMax="' + data.result[i].ComponentsRange.MaxValue + '" istractorfrnt="' + data.result[i].IsTractorInfront + '" numtractor="' + numtractor + '" numtrailer="' + numtrailor + '" listVhclCompNum="' + i + '" sidebyside="' + data.result[i].SideBySideRows + '">' + data.result[i].ConfigurationName + '</option>');
            $('#VehicleTypeConfig').attr("disabled", false);

            
            //DisableSideBySideDrop();
            //HideSideBySideDrop();
        }
        
        //console.log(vehcileCompTypeList[5][0]);

        var isSideBy = $('#sovehothersidebyside').val();
        if (isSideBy == 'True') {
            //$('#VehicleTypeConfig option[value=244007]').attr("selected", 'selected');
            $('#VehicleTypeConfig').val('244007');

            var dropdownobj = document.getElementById('VehicleTypeConfig');
            VehicleTypeChangeConfig(dropdownobj)
            //$('#VehicleTypeConfig').select();  
            $('#VehicleTypeConfig').attr('disabled', 'disabled');
        }

        if (datalength == 1) {
            $('#VehicleTypeConfig option:eq(1)').prop('selected', true);
            $('#VehicleTypeConfig').change();
        }

    });
}
function componentNumChange(_this) {
    var _thisval = $(_this).val();
    if (_thisval != '') {
        numRows = _thisval;

        if (istractorfrnt == "true" && _thisval == 2) {
            FillImage();
            //loadGeneralPage(movementId, vehicleTypeId);
            //$('#div_componentNum').hide();
        }
        else {
            loadRowComponent();
            $('#componentDetailsConfig').html('');
        }
    }
}
//function to load generalP)age
function loadGeneralPage(movementId, vehicleTypeId) {
      var isnotifveh = $("#IsNotifVeh").val();
       $('#componentDetailsConfig').load("../VehicleConfig/GeneralPage?movementId=" + movementId + "&&vehicleConfigId=" + vehicleTypeId + "&ISNotifVeh=" + isnotifveh, function () {
        $('#componentDetailsConfig').show();
        $('#div_RowComp_main').hide();

        RemoveImages();

        $('#div_General_Config').show();
    });
}
//function to fill number of vehicle component slect dropdown
function fillVehicleCompNumSelect(rangemin, rangemax) {
    $('#componentNum option:not(:first-child)').remove();
    for (var i = rangemin; i <= rangemax; i++) {
        $('#componentNum').append('<option value="' + i + '">' + i + '</option>');
    }
    $('#componentNum').attr("disabled", false);
    $('#div_componentNum').show();
}
//function to fill define row component
function loadRowComponent() {
    fillRowComponent();
}
//ajax method to fill row component
function fillRowComponent() {
    var vhclCompSubList = FillSubDropDown();
    $('#div_RowComp').find('table').find('tr').remove();
    //var datalength = vehicleComponentList.length;
    var datalength = vhclCompSubList.length;
    //console.log(vhclCompSubList);
    for (var j = 1; j <= numRows; j++) {
        $('#div_RowComp').find('table').append('<tr><td>Row ' + j + '</td><td><select id="' + j + '"></select></td></tr>');

        for (var i = 0; i < datalength; i++) {
            //for the first row if vehicle type has tractor infront attribute else all data are taken
            if (istractorfrnt == "true" && j == 1) {
                //if (vehicleComponentList[i].IsTractor) {
                //    $('#div_RowComp').find('#' + j + '').append('<option value="' + vehicleComponentList[i].ComponentTypeId + '" compName="' + vehicleComponentList[i].ComponentName + '" compImg="' + vehicleComponentList[i].ImageName + '" istractor="' + vehicleComponentList[i].IsTractor + '">' + vehicleComponentList[i].ComponentName + '</option>');
                //}
                if (vhclCompSubList[i].IsTractor) {
                    $('#div_RowComp').find('#' + j + '').append('<option value="' + vhclCompSubList[i].ComponentTypeId + '" compName="' + vhclCompSubList[i].ComponentName + '" compImg="' + vhclCompSubList[i].ImageName + '" istractor="' + vhclCompSubList[i].IsTractor + '" latpos="' + j + '" longpos="1">' + vhclCompSubList[i].ComponentName + '</option>');
                }
            }
            else {
                //$('#div_RowComp').find('#' + j + '').append('<option value="' + vehicleComponentList[i].ComponentTypeId + '" compName="' + vehicleComponentList[i].ComponentName + '" compImg="' + vehicleComponentList[i].ImageName + '" istractor="' + vehicleComponentList[i].IsTractor + '">' + vehicleComponentList[i].ComponentName + '</option>');
                $('#div_RowComp').find('#' + j + '').append('<option value="' + vhclCompSubList[i].ComponentTypeId + '" compName="' + vhclCompSubList[i].ComponentName + '" compImg="' + vhclCompSubList[i].ImageName + '" istractor="' + vhclCompSubList[i].IsTractor + '" latpos="' + j + '" longpos="1">' + vhclCompSubList[i].ComponentName + '</option>');
            }
        }
        $('#div_RowComp_main').show();
    }
}
//function to get component list
function GetComponentList() {
    return vehcileCompTypeList;
}
//function to Navigate to Register page
function NavigateToRegister() {
  
    if (validationConfig()) {
        var isVr1 = $('#vr1appln').val();
        var isNotify = $('#ISNotif').val();
        var editveh = $('#editveh').val();
        var vehcls = $('#VehicleClass').val();

        var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
      
        var RoutePartId = $('#RoutePartId').val() ? $('#RoutePartId').val() : 0;
        
        var contentRefNo = $('#CRNo').val() ? $('#CRNo').val() : "";


        //saving or editing a VR1 vehicle config
        //Saving candidate route vehicles - added by Salih on 05-05-2015
        var iscandidatevehicles = $('#IsCandVehicle').val();
        if (iscandidatevehicles == 'True') {
            var candrevisionId = $('#revisionId').val();
            if (vehicleConfigurationId == 0) {
                SaveCandidateVehicleConfiguration(candrevisionId);
            }
            else {
                EditVR1VehicleConfiguration(RoutePartId);
            }
        }
        else if (isVr1 == 'True' || isVr1 == 'true') {
            if (vehicleConfigurationId == 0) {
                //added by poonam (13.8.14)
                var vr1contrefno = $('#VR1ContentRefNo').val()?$('#VR1ContentRefNo').val():"";
                var vr1versionid = $('#VersionId').val()?$('#VersionId').val():0;
                //-----------
                if (vr1versionid == 0) {
                    SaveNotifVehicleConfiguration(contentRefNo);
                }
                else {
                    SaveVR1VehicleConfiguration(ApplicationRevId, vr1contrefno, vr1versionid);
                }
            }
            else {
                if (editveh == 1 && vehcls!=270001) {
                    var gw = $('#Weight').val();
                    var ol = $('#OverallLength').val();
                    $('#GrossWeight').val(gw);
                    $('#VehLength').val(ol);
                }
                EditVR1VehicleConfiguration(RoutePartId);
            }
        }
        else if (isNotify == 'True' || isNotify == 'true') {
            if (vehicleConfigurationId == 0) {
                
                SaveNotifVehicleConfiguration(contentRefNo);
            }
            else {
                EditVR1VehicleConfiguration(RoutePartId);
            }
        }
        else{
            var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
            // method to save application configuration vehicle
            if (ApplicationRevId != 0) {

                if (vehicleConfigurationId == 0) {
                    SaveApplicationVehicleConfiguration(ApplicationRevId);
                }
                else {
                    EditAppVehicleConfiguration(ApplicationRevId);
                }
            }
                //saving or editing fleet vehicle config
            else if (vehicleConfigurationId == 0) {
                SaveConfiguration();

            }
            else {
                //method to update
                EditConfiguration();
            }
        }
    }
}
function GetCompNum() {
    return vehcileCompNum;
}
//function to validate data against the save
function validationConfig() {
    var isvalid = true;
    var movementId = $('#MovementClassConfig').val();
   
    $('#div_config_general input:text,textarea').each(function () {
        
        $(this).closest('div').find('.error').text('');

        var required = $(this).attr('isrequired');//is required field
        var type = $(this).attr('datatype');//datatype of the field
        var range = $(this).attr('range');//min and max range
        var field = $(this).attr('validationmsg');//textbox id/name field

        var text = $(this).val();//textbox value
        text = $.trim(text);

        var splitRange = ",";
        
        if (range != undefined) {
            splitRange = range.split(',');           
        }
        var minval = splitRange[0];
        var maxval = splitRange[1];



        var tyreSpace = $('#div_config_general').find('#Tyre_Spacing').val();
        var overWidth = $('#div_config_general').find('#Width').val();

    /******************************************************************************************************
    code block added by Mahzeer on 4th Apr 2017 to resolve 
    RM #6037 Detailed notification not giving the same warning as simplified

    gross weight max limit for each vehicle type is as follows:

    1. 241011 - C and U - Gross weight of vehicle should be less than or equal to 44000
    2. 241003 - stgo ail cat 1 - Gross weight of vehicle should be less than or equal to 50000
    3. 241006 - stgo mobile crane cat a - Gross weight of vehicle should be less than or equal to 50000
    4. 241004 - stgo ail cat 2 - Gross weight of vehicle should be less than or equal to 80000
    5. 241007 - stgo mobile crane cat b - Gross weight of vehicle should be less than or equal to 80000
    6. 241005 - stgo ail cat 3 - Gross weight of vehicle should be less than or equal to 150000
    7. 241008 - stgo mobile crane cat c - Gross weight of vehicle should be less than or equal to 150000
   *********************************************************************************************************/
        var movementClass = $('#MovementClassification').val(); // to get the vehicle class 

        if (field == 'Weight') {
            if (movementClass == 241011) {
                maxval = 44000;
            }
            else if (movementClass == 241003 || movementClass == 241006) {
                maxval = 50000;
            }
            else if (movementClass == 241004 || movementClass == 241007) {
                maxval = 80000;
            }
            else if (movementClass == 241005 || movementClass == 241008) {
                maxval = 150000;
            }

            if ($('#VehicleTypeConfig').val() == 244006) {
                if (parseFloat(text) < parseFloat($('#ComponentTotalWeight').val())) {
                    $(this).closest('div').find('.error').text(field + ' must be greater than or equal to ' + $('#ComponentTotalWeight').val());
                    isvalid = false;
                }
            }
        }

        if (required == 1 && text == '') {
            $(this).closest('div').find('.error').text(field + ' is required');
            isvalid = false;
        }
        else if (movementId == 270006 && (parseFloat(text) == 0) && (field != 'Internal Name' && field != 'Formal Name' && field != 'Notes')) {
            $(this).closest('div').find('.error').text(field + ' must be greater than 0');
             isvalid = false;
        }
        else if (movementId == 270001 && field == 'OverallLength'){
            if(parseFloat(text) > parseFloat(maxval)) {
                $(this).closest('div').find('.error').text(field + ' must be less than ' + maxval);
            isvalid = false;
        }
        }
        else if ((movementId != 270006) && (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval))) {
            $(this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
            isvalid = false;
        }
        else if (parseInt(overWidth) <= parseInt(tyreSpace)) {
            $('#div_config_general').find('#Tyre_Spacing').closest('div').find('.error').text('Side by side spacing should be less than overall width');
            isvalid = false;
        }
    });

    return isvalid;
}
var isTableValid = true;
function validateTable(_this, vd) {
    isTableValid = true;
    var form = $("#div_reg_config_vehicle");
    if (form.length > 0) {
        var tablevehicletable = form.find('#vehicle-reg-table.RegistrationComponent');
        if (tablevehicletable.length > 0) {
            console.log('vehicle-reg-table', true);
            var allTrs = $("#vehicle-reg-table.RegistrationComponent > tbody > tr");
            var registrationDetails = [];
            allTrs.each(function (index, value) {//loop thorugh all tbody>tr items and insert all itemsinto an array
                var isLastElement = index == allTrs.length - 1;
                if (!isLastElement) {
                    var regIdComp = $(this).find('td span.cls_regId_config').length > 0 ? $(this).find('td span.cls_regId_config') : $(this).find('td.cls_regId');
                    var fleetIdComp = $(this).find('td span.cls_fleetId_config').length > 0 ? $(this).find('td span.cls_regId_config') : $(this).find('td.cls_fleetId');
                    var regId = regIdComp.text();
                    var fleetId = fleetIdComp.text();
                    var id = $(this).find('td .hdId').length > 0 ? $(this).find('td .hdId').val() : 0;
                    if (regId || fleetId) {
                        var obj = { RegId: regId, FleetId: fleetId, Id: id };
                        registrationDetails.push(obj);
                    }
                }
            }).promise().done(function () { // after loop through all tbody tr items, we need to check the table has atleast one entry
                console.log('registrationDetails', registrationDetails);
                if (registrationDetails.length == 0) {
                    form.find('#error_msg').text('Please enter at least one registration plate or fleet id.');
                    setTimeout(function () {
                        form.find('#error_msg').text('');
                    }, 5000);
                    isTableValid = false;
                }
            });
        }
    }
}
//function to clear error span message
function clearAllErrorSpan() {
    $('input,textarea').focus(function () {
        $(this).closest('div').find('.error').text('');
    });
}
//function to validate data on keyup
function keyUpValidation(_this) {
   
    var movementId = $('#MovementClassConfig').val();
    $(_this).closest('div').find('.error').text('');
    var range = $(_this).attr('range');//min and max range
    var field = $(_this).attr('validationmsg');//textbox id/name field

    var text = $(_this).val();//textbox value
    
    var splitRange = range.split(',');
    var minval = splitRange[0];
    var maxval = splitRange[1];

    
    /******************************************************************************************************
     code block added by Anlet on 16th Feb 2017 to resolve 
     RM #6037 Detailed notification not giving the same warning as simplified

     gross weight max limit for each vehicle type is as follows:

     1. 241011 - C and U - Gross weight of vehicle should be less than or equal to 44000
     2. 241003 - stgo ail cat 1 - Gross weight of vehicle should be less than or equal to 50000
     3. 241006 - stgo mobile crane cat a - Gross weight of vehicle should be less than or equal to 50000
     4. 241004 - stgo ail cat 2 - Gross weight of vehicle should be less than or equal to 80000
     5. 241007 - stgo mobile crane cat b - Gross weight of vehicle should be less than or equal to 80000
     6.	241005 - stgo ail cat 3 - Gross weight of vehicle should be less than or equal to 150000
     7. 241008 - stgo mobile crane cat c - Gross weight of vehicle should be less than or equal to 150000
    *********************************************************************************************************/
    var movementClass = $('#MovementClassification').val(); // to get the vehicle class 
    
    if (field == 'Weight') {
        if (movementClass == 241011) {
            maxval = 44000;
        }
        else if (movementClass == 241003 || movementClass == 241006) {
            maxval = 50000;
        }
        else if (movementClass == 241004 || movementClass == 241007) {
            maxval = 80000;
        }
        else if (movementClass == 241005 || movementClass == 241008) {
            maxval = 150000;
        }
        if ($('#VehicleTypeConfig').val() == 244006) {
            if (parseFloat(text) < parseFloat($('#ComponentTotalWeight').val())) {
                $(_this).closest('div').find('.error').text(field + ' must be greater than or equal to ' + $('#ComponentTotalWeight').val());
            }
        }
    }
    /**********************************************************/

    if (text == '') {

    }
    else if (movementId == 270001 && field == 'OverallLength'){
        if (parseFloat(text) > parseFloat(maxval)) {
            $(_this).closest('div').find('.error').text(field + ' must be less than ' + maxval);
        }
    }
    else if (movementId == 270006 && (parseFloat(text) == 0)&&(field != 'Internal Name' && field != 'Formal Name' && field != 'Notes')) {
        $(_this).closest('div').find('.error').text(field + ' must be greater than 0 ');
    }
    else if (movementId != 270006 && (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval))) {
        $(_this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
    }
}
//function to update Application vehicle configuration
function EditAppVehicleConfiguration(ApplicationRevId) {
    var movementId = $('#MovementClassConfig').val();
    var configTypeId = parseInt($('#VehicleTypeConfig').val());
    var speedUnit = $('#SpeedUnits').val();

    if (speedUnit == undefined || speedUnit == '') {
        speedUnit = null;
    }

    var configList = {
        moveClassification: { ClassificationId: movementId },
        //vehicleConfigType: { ConfigurationTypeId: configTypeId },
        VehicleConfigParamList: []
    };

    // $('.div_config_general input:text,textarea').each(function () {
    $('#div_config_general input,textarea').not(':hidden,:checkbox').each(function () {
        var name = $(this).val();
        var model = $(this).attr("id");
        var datatype = $(this).attr("datatype");
        if ($('#UnitValue').val() == 692002) {
            if (IsLengthFields(this)) {
                name = ConvertToMetre(name);
            }
            //if (IsSpeedField(this)) {
            //    name = ConvertToKph(name);
            //}
        }
        configList.VehicleConfigParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
    });
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;//added for Log
    $.ajax({
        url: "../VehicleConfig/UpdateAppVehicleConfiguration",
        //data: '{"vehicleConfigObj":' + JSON.stringify(configList) + '}',
        data: '{"vehicleConfigObj":' + JSON.stringify(configList) + ',"configTypeId":' + configTypeId + ',"vehicleId":' + vehicleConfigurationId + ',"ApplicationRevId":' + ApplicationRevId + ',"speedUnit":'+ speedUnit +'}',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        async: false,
        beforeSend: function () {
            //$('#overlay_anim').show();
            $('.popoverlay').show();
            $('.config_body').hide();
            //$('#div_General_Config').hide();
        },
        success: function (data) {

            if (data.Success) {
                $('#div_General_Config').hide();
                $("#div_Registration").load('../VehicleConfig/RegistrationConfiguration?vehicleId=' + vehicleConfigurationId + '&RegBtn=' + false + '');
                $('.err_formalName').text('');

                $('#selection_Config').find('select').prop('disabled', 'disabled');

                vehicleName = $('#Internal_Name').val();//set vehicle name for display
            }
            else {

                $('.err_formalName').text('Name already exists');
                //formal name
            }
        },
        error: function () {
            location.reload();
        },
        complete: function () {
            $('.popoverlay').hide();
            $('.config_body').show();
        }
    });

}
//function to update VR-1 Application vehicle configuration
function EditVR1VehicleConfiguration(RoutePartId) {
    var notifid = $("#NotificatinId").val() ? $('#NotificatinId').val() : 0;
    var isNotif = false;
    if (notifid != 0) {
        isNotif = true;
    }
    var movementId = $('#MovementClassConfig').val();
    var configTypeId = parseInt($('#VehicleTypeConfig').val());
    var speedUnit = $('#SpeedUnits').val();

    if (speedUnit == undefined || speedUnit == '') {
        speedUnit = null;
    }
    
    var configList = {
        moveClassification: { ClassificationId: movementId },
        //vehicleConfigType: { ConfigurationTypeId: configTypeId },
        VehicleConfigParamList: []
    };

    // $('.div_config_general input:text,textarea').each(function () {
    $('#div_config_general input,textarea').not(':hidden,:checkbox').each(function () {
        var name = $(this).val();
        var model = $(this).attr("id");
        var datatype = $(this).attr("datatype");
        if ($('#UnitValue').val() == 692002) {
            if (IsLengthFields(this)) {
                name = ConvertToMetre(name);
            }
            //if (IsSpeedField(this)) {
                
            //    name = ConvertToKph(name);
                
            //}
        }
        configList.VehicleConfigParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
    });
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;//added for Log
    $.ajax({
        url: "../VehicleConfig/UpdateVR1VehicleConfiguration",
        //data: '{"vehicleConfigObj":' + JSON.stringify(configList) + '}',
        data: '{"vehicleConfigObj":' + JSON.stringify(configList) + ',"configTypeId":' + configTypeId + ',"vehicleId":' + vehicleConfigurationId + ',"ApplicationRevId":' + ApplicationRevId + ',"routePartId":' + RoutePartId + ',"speedUnit":' + speedUnit + ',"isNotif":' + isNotif + ',"NotificationId":' + notifid + '}',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        async: false,
        beforeSend: function () {
            //$('#overlay_anim').show();
            $('.popoverlay').show();
            $('.config_body').hide();
            //$('#div_General_Config').hide();
        },
        success: function (data) {
           
            if (data.Success) {
                $('#div_General_Config').hide();
                $("#div_Registration").load('../VehicleConfig/RegistrationConfiguration?vehicleId=' + vehicleConfigurationId + '&RegBtn=' + false + '&isVR1='+true+'');
                $('.err_formalName').text('');

                $('#selection_Config').find('select').prop('disabled', 'disabled');

                vehicleName = $('#Internal_Name').val();//set vehicle name for display
            }
            else {

                $('.err_formalName').text('Name already exists');
                //formal name
            }
        },
        error: function () {
            location.reload();
        },
        complete: function () {
            $('.popoverlay').hide();
            $('.config_body').show();
        }
    });

}
//function to Save Application Vehicle Configuration();
function SaveApplicationVehicleConfiguration(ApplicationRevId) {

    var movementId = $('#MovementClassConfig').val();
    var configTypeId = parseInt($('#VehicleTypeConfig').val());
    var speedUnit = $('#SpeedUnits').val();

    if (speedUnit == undefined || speedUnit == '') {
        speedUnit = null;
    }

    var configList = {
        moveClassification: { ClassificationId: movementId },
        //vehicleConfigType: { ConfigurationTypeId: configTypeId },
        VehicleConfigParamList: []
    };

    // $('.div_config_general input:text,textarea').each(function () {
    $('#div_config_general input,textarea').not(':hidden,:checkbox').each(function () {
        var name = $(this).val();
        var model = $(this).attr("id");
        var datatype = $(this).attr("datatype");
        if ($('#UnitValue').val() == 692002) {
            if (IsLengthFields(this)) {
                name = ConvertToMetre(name);
            }
            //if (IsSpeedField(this)) {
            //    name = ConvertToKph(name);
            //}
        }
        configList.VehicleConfigParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
    });
    $.ajax({
        url: "../VehicleConfig/CreateApplicationVehicleConfiguration",
        //data: '{"vehicleConfigObj":' + JSON.stringify(configList) + '}',
        data: '{"vehicleConfigObj":' + JSON.stringify(configList) + ',"configTypeId":' + configTypeId + ',"speedUnit":' + speedUnit +',"ApplicationRevId":' + ApplicationRevId + '}',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        async: false,
        beforeSend: function () {
            //$('#overlay_anim').show();
            $('.popoverlay').show();
            $('.config_body').hide();
        },
        success: function (data) {
            if (data.configId != 0) {
                vehicleConfigurationId = data.configId;

                vehicleName = $('#Internal_Name').val();//set vehicle name for display

                ImportComponents(vehicleConfigurationId);
            }
            else {
                $('.err_formalName').text('Name already exists');
                //formal name
            }
        },
        error: function () {
           
            location.reload();
        },
        complete: function () {
            //$('#overlay_anim').hide();
            $('.popoverlay').hide();
            $('.config_body').show();
        }
    });

}
//function to save VR1 application vehicle configurations
function SaveVR1VehicleConfiguration(RevisionId, contentref, versionid) {
    var contentReff = $('#CRNo').val() ? $('#CRNo').val() : "";
    if (contentReff == "") { contentReff = contentref;}
  
    var movementId = $('#MovementClassConfig').val();
    var configTypeId = parseInt($('#VehicleTypeConfig').val());

    var speedUnit = $('#SpeedUnits').val();

    if (speedUnit == undefined || speedUnit == '') {
        speedUnit = null;
    }

    var configList = {
        moveClassification: { ClassificationId: movementId },
        //vehicleConfigType: { ConfigurationTypeId: configTypeId },
        VehicleConfigParamList: []
    };

    // $('.div_config_general input:text,textarea').each(function () {
    $('#div_config_general input,textarea').not(':hidden,:checkbox').each(function () {
        var name = $(this).val();
        var model = $(this).attr("id");
        var datatype = $(this).attr("datatype");
        if ($('#UnitValue').val() == 692002) {
            if (IsLengthFields(this)) {
                name = ConvertToMetre(name);
            }
            //if (IsSpeedField(this)) {
            //    name = ConvertToKph(name);
            //}
        }
        configList.VehicleConfigParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
    });
    $.ajax({
        url: "../VehicleConfig/CreateVR1ApplicationVehicleConfiguration",
        //data: '{"vehicleConfigObj":' + JSON.stringify(configList) + '}',
        data: '{"vehicleConfigObj":' + JSON.stringify(configList) + ',"configTypeId":' + configTypeId + ',"ContentRefNo":' + JSON.stringify(contentReff) + ',"RevisionID":' + RevisionId + ' ,"VersionID":' + versionid + ',"speedUnit":' + speedUnit + '}',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        async: false,
        beforeSend: function () {
            //$('#overlay_anim').show();
            $('.popoverlay').show();
            $('.config_body').hide();
        },
        success: function (data) {
            if (data.configId != 0) {
                vehicleConfigurationId = data.configId;
                routepartid = data.routepartId;
                vehicleName = $('#Internal_Name').val();//set vehicle name for display
                $('#RoutePartId').val(routepartid);
                
                ImportComponents(vehicleConfigurationId);
            }
            else {
                $('.err_formalName').text('Name already exists');
                //formal name
            }
        },
        error: function () {
          
            location.reload();
        },
        complete: function () {
            //$('#overlay_anim').hide();
            $('.popoverlay').hide();
            $('.config_body').show();
        }
    });

}
//Save candidate route vehicles
function SaveCandidateVehicleConfiguration(Candrevisionid) {
    //var movementId = $('#MovementClassConfig').val();
    movementId = $('#movementId').val();
    var configTypeId = parseInt($('#VehicleTypeConfig').val());

    var speedUnit = $('#SpeedUnits').val();

    if (speedUnit == undefined || speedUnit == '') {
        speedUnit = null;
    }

    var configList = {
        moveClassification: { ClassificationId: movementId },
        //vehicleConfigType: { ConfigurationTypeId: configTypeId },
        VehicleConfigParamList: []
    };

    // $('.div_config_general input:text,textarea').each(function () {
    $('#div_config_general input,textarea').not(':hidden,:checkbox').each(function () {
        var name = $(this).val();
        var model = $(this).attr("id");
        var datatype = $(this).attr("datatype");
        if ($('#UnitValue').val() == 692002) {
            if (IsLengthFields(this)) {
                name = ConvertToMetre(name);
            }
            //if (IsSpeedField(this)) {
            //    name = ConvertToKph(name);
            //}
        }
        configList.VehicleConfigParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
    });
    $.ajax({
        url: "../VehicleConfig/CreateVR1ApplicationVehicleConfiguration",
        //data: '{"vehicleConfigObj":' + JSON.stringify(configList) + '}',
        data: '{"vehicleConfigObj":' + JSON.stringify(configList) + ',"configTypeId":' + configTypeId + ',"ContentRefNo":null' + ',"speedUnit":' + speedUnit + ',"CandRevisionId":' + Candrevisionid + '}',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        async: false,
        beforeSend: function () {
            //$('#overlay_anim').show();
            $('.popoverlay').show();
            $('.config_body').hide();
        },
        success: function (data) {
            if (data.configId != 0) {
                vehicleConfigurationId = data.configId;
                routepartid = data.routepartId;
                vehicleName = $('#Internal_Name').val();//set vehicle name for display
                $('#RoutePartId').val(routepartid);

                ImportComponents(vehicleConfigurationId);
            }
            else {
                $('.err_formalName').text('Name already exists');
                //formal name
            }
        },
        error: function () {           
            location.reload();
        },
        complete: function () {
            //$('#overlay_anim').hide();
            $('.popoverlay').hide();
            $('.config_body').show();
        }
    });

}
//function to create new vehicle for notification
function SaveNotifVehicleConfiguration(contentrefno) {

    
    var movementId = $('#MovementClassConfig').val();
    var notifid = $("#NotificatinId").val() ? $('#NotificatinId').val() : 0;
    var configTypeId = parseInt($('#VehicleTypeConfig').val());

    var speedUnit = $('#SpeedUnits').val();

    if (speedUnit == undefined || speedUnit == '') {
        speedUnit = null;
    }

  
    var configList = {
        moveClassification: { ClassificationId: movementId },
        //vehicleConfigType: { ConfigurationTypeId: configTypeId },
        VehicleConfigParamList: []
    };

    // $('.div_config_general input:text,textarea').each(function () {
    $('#div_config_general input,textarea').not(':hidden,:checkbox').each(function () {
        var name = $(this).val();
        var model = $(this).attr("id");
        var datatype = $(this).attr("datatype");
        if ($('#UnitValue').val() == 692002) {
            if (IsLengthFields(this)) {
                name = ConvertToMetre(name);
            }
            //if (IsSpeedField(this)) {
           
            //    name = ConvertToKph(name);
         
            //}
        }
        configList.VehicleConfigParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
    });

    
    //return false;
    $.ajax({
        url: "../VehicleConfig/CreateVR1ApplicationVehicleConfiguration",
        //data: '{"vehicleConfigObj":' + JSON.stringify(configList) + '}',
        data: '{"vehicleConfigObj":' + JSON.stringify(configList) + ',"configTypeId":' + configTypeId + ',"ContentRefNo":' + JSON.stringify(contentrefno) + ',"RevisionID":' + 0 + ' ,"VersionID":' + 0 + ',"NotificationID":' + notifid + '}',
        //data: '{"vehicleConfigObj":' + JSON.stringify(configList) + ',"configTypeId":' + configTypeId + ',"ContentRefNo":' + JSON.stringify(contentrefno) + '}',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        async: false,
        beforeSend: function () {
            //$('#overlay_anim').show();
            $('.popoverlay').show();
            $('.config_body').hide();
        },
        success: function (data) {
            if (data.configId != 0) {
                vehicleConfigurationId = data.configId;
                routepartid = data.routepartId;
                vehicleName = $('#Internal_Name').val();//set vehicle name for display
                $('#RoutePartId').val(routepartid);

                ImportComponents(vehicleConfigurationId);
            }
            else {
                $('.err_formalName').text('Name already exists');
                //formal name
            }
        },
        error: function () {
          
            location.reload();
        },
        complete: function () {
            //$('#overlay_anim').hide();
            $('.popoverlay').hide();
            $('.config_body').show();
        }
    });

}
//function to validate number of tractors against trailors
function ValidateNumTractors() {
    var isvalid = true;
    var numtractor = 0;
    var numtrailor = 0;
    var vhclCompList = [];

    vhclLatLongPos = [];
    $('#div_RowComp').find('select').each(function () {
        var thisVal = this;
        var istractor = $('option:selected', thisVal).attr('istractor');
        var imgName = $('option:selected', thisVal).attr('compImg');
        var compName = $('option:selected', thisVal).attr('compName');

        var latPosition = $('option:selected', thisVal).attr('latpos');
        var longPosition = $('option:selected', thisVal).attr('longpos');

        //var longPosition = $('option:selected', thisVal).attr('latpos');
        //var latPosition = $('option:selected', thisVal).attr('longpos');
        //mirza

        //ComponentId, LatPos, LongPos


        var compId = $(thisVal).val();
        vhclCompList.push({ ComponentTypeId: compId, ComponentName: compName, IsTractor: istractor, ImageName: imgName });
        vhclLatLongPos.push({ ComponentId: compId, LatPos: latPosition, LongPos: longPosition });

        if (istractor == "true") {
            numtractor++;
        }
        else {
            numtrailor++;
        }
    });
    if (numtractor > 4) {
        $('.comp_error').text('A maximum of 4 tractors are allowed');
        isvalid = false;
    }
    else if (numtrailor > 4) {
        $('.comp_error').text('A maximum of 4 trailers are allowed');
        isvalid = false;
    }
    else {
        $('.comp_error').text('');
        vehcileCompTypeList[vehcileCompNum] = vhclCompList;
    }
    return isvalid;

}
//function retrieve data from global vehicle list
function GetGlobalVehicleList() {
    vehcileCompTypeList = globalVehicleList;
    //console.log(vehcileCompTypeList);
}
//function to retrieve vehicle configuration Id
function GetConfigurationId() {
    return vehicleConfigurationId;
}
//function to Edit Configuration
function EditConfigurationLoad(vehicleID) {
    $.ajax({
        url: "../VehicleConfig/GetMovementId",
        type: 'POST',
        cache: false,
        data: { vehicleId: vehicleID },
        success: function (result) {
            var movementId = result.movementId;
            vehicleTypeId = result.vehicleTypeId;
            vehicleConfigurationId = vehicleID;

            $('#MovementClassConfig').val(movementId).change();
            $('#MovementClassConfig').attr('disabled', 'disabled');
            //SECOND DROPDOWN IS FILLED WITH A DELAY TO COMPLETE LOADING THE DATA AFTER FIRST DROPDOWN CHANGE IS TRIGGERED
            //setTimeout(TriggerVehicleTypeId, 100)
            //$('#VehicleTypeConfig').val(vehicleTypeId);

            GetGeneralPageForEdit(vehicleID);
        }
    });
}
//function to Get General Page for Edit
function GetGeneralPageForEdit(vehicleID) {
    $.ajax({
        url: "../VehicleConfig/GetGeneralPageEdit",
        type: 'POST',
        cache: false,
        data: { vehicleId: vehicleID },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            //console.log(result);   
            //$('#VehicleTypeConfig').val(vehicleTypeId).change();
            $('#VehicleTypeConfig').val(vehicleTypeId);
            //GetGlobalVehicleList();                

            $('#componentDetailsConfig').html(result);
            //setTimeout(ShowComponentConfig, 1000)
            //$('#componentDetailsConfig').show();
            //div_General_Config
            FillComponentId(vehicleID);

        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            vehcileCompNum = GetExactNum();
            $('#ComponentNum').val(vehcileCompNum);
           
        }
    });
}
//function to Edit Application Configuration
function EditConfigurationLoadApplication(vehicleID) {
    $.ajax({
        url: "../VehicleConfig/GetMovementIdApplication",
        type: 'POST',
        cache: false,
        data: { vehicleId: vehicleID },
        success: function (result) {
            var movementId = result.movementId;
            vehicleTypeId = result.vehicleTypeId;
            vehicleConfigurationId = vehicleID;

            $('#MovementClassConfig').val(movementId).change();
            $('#MovementClassConfig').attr('disabled', 'disabled');
            //SECOND DROPDOWN IS FILLED WITH A DELAY TO COMPLETE LOADING THE DATA AFTER FIRST DROPDOWN CHANGE IS TRIGGERED
            //setTimeout(TriggerVehicleTypeId, 100)
            //$('#VehicleTypeConfig').val(vehicleTypeId);

            GetGeneralPageForEditApplication(vehicleID);
        }
    });
}
//function to Edit VR1 Application Configuration
function EditConfigurationLoadVR1(vehicleID,IsNotifVehicle) {
   
    $.ajax({
        url: "../VehicleConfig/GetMovementIdVR1",
        type: 'POST',
        cache: false,
        data: { vehicleId: vehicleID },
        success: function (result) {
            var movementId = result.movementId;
            vehicleTypeId = result.vehicleTypeId;
            vehicleConfigurationId = vehicleID;

            $('#MovementClassConfig').val(movementId).change();
            $('#MovementClassConfig').attr('disabled', 'disabled');
            //SECOND DROPDOWN IS FILLED WITH A DELAY TO COMPLETE LOADING THE DATA AFTER FIRST DROPDOWN CHANGE IS TRIGGERED
            //setTimeout(TriggerVehicleTypeId, 100)
            //$('#VehicleTypeConfig').val(vehicleTypeId);

            GetGeneralPageForEditVR1(vehicleID, IsNotifVehicle);
        }
    });
}
function GetGeneralPageForEditApplication(vehicleID) {
    $.ajax({
        url: "../VehicleConfig/GetGeneralPageEditApplication",
        type: 'POST',
        cache: false,
        data: { vehicleId: vehicleID , AppNextBtn :true },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            //console.log(result);   
            //$('#VehicleTypeConfig').val(vehicleTypeId).change();
            $('#VehicleTypeConfig').val(vehicleTypeId);
            //GetGlobalVehicleList();                

            $('#componentDetailsConfig').html(result);
            //setTimeout(ShowComponentConfig, 1000)
            //$('#componentDetailsConfig').show();
            //div_General_Config
            FillAppComponentId(vehicleID);

        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            vehcileCompNum = GetExactNum();
            $('#ComponentNum').val(vehcileCompNum);
          
        }
    });
}
function GetGeneralPageForEditVR1(vehicleID,IsNotifVehicle) {
   
    $.ajax({
        url: "../VehicleConfig/GetGeneralPageEditVR1",
        type: 'POST',
        cache: false,
        data: { vehicleId: vehicleID, AppNextBtn: true ,isVR1: true,ISNotifVeh:IsNotifVehicle},
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            //console.log(result);   
            //$('#VehicleTypeConfig').val(vehicleTypeId).change();
            $('#VehicleTypeConfig').val(vehicleTypeId);
            //GetGlobalVehicleList();                

            $('#componentDetailsConfig').html(result);
            //setTimeout(ShowComponentConfig, 1000)
            //$('#componentDetailsConfig').show();
            //div_General_Config
            FillVR1ComponentId(vehicleID);

        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            vehcileCompNum = GetExactNum();
            $('#ComponentNum').val(vehcileCompNum);
            
        }
    });
}
//function to fill Image
function FillImage() {
    RemoveImages();
    //var vhclCompSubList = FillSubDropDown();
    var num = parseInt(vehcileCompNum);
    //console.log(num);
    //console.log(vehcileCompTypeList[num]);
    var totlength = vehcileCompTypeList[num].length;

    var title = '';
    //console.log(totlength);
    //var totlength = vhclCompSubList.length;
    for (var i = 0; i < totlength; i++) {
        title = vehcileCompTypeList[num][i].ComponentName;
        //exceptional cases
        var movementId = $('#MovementClassConfig').val();
        if (movementId == 270005) {
            if (i == 0) {
                vehcileCompTypeList[num][0].ImageName = "recoveryvehicle";
                title = 'recovery vehicle';
            }
            else {
                vehcileCompTypeList[num][i].ImageName = "recoveredvehicle";
                title = 'recovered vehicle';
            }
        }
        else if (movementId == 270003) {
            title = 'Mobile Crane';
        }

        //-----------------------------------------



        var latpostion = i + 1;
        var longposition = 1;
        if (fromsidebyside) {
            latpostion = vhclLatLongPos[i].LatPos;
            longposition = vhclLatLongPos[i].LongPos;
            //longposition = vhclLatLongPos[i].LatPos;
            //latpostion = vhclLatLongPos[i].LongPos;
        }
        $('#div_img_header').append('<div class="tooltip"  title="' + title + '" ><div class="div_img_line" id="div_' + i + '" compId="' + vehcileCompTypeList[num][i].ComponentTypeId + '" compTypeName="' + vehcileCompTypeList[num][i].ComponentName + '" number="' + i + '"><div class="vehiclecomp_div" ><div class="vehiclecomponent centre ' + vehcileCompTypeList[num][i].ImageName + '" title="' + title + '" latpos="' + latpostion + '" longpos="' + longposition + '"></div></div></div></div>');
        //console.log(i);
    }
    //div_image_portion
    $('#div_image_portion').show();
    //selection_Config
    $('#selection_Config').hide();

    AlignImageSideBySide();
}
//function Navigate to Select
function NavigateToSelect() {
    RemoveImages();
    $('#div_General_Config').show();
}
//function to navigate to general page from image
function NavigateToGeneralFromImg() {
    document.getElementById("VehicleConfignType").value = vehicleTypeId;
    if (movementId == "" || movementId == null || movementId == undefined) {
        movementId = $('#movementId').val();
    }
    loadGeneralPage(movementId, vehicleTypeId);

    if (fromsidebyside) {
        $('.filter').not('#div_componentNum').show();
    }
    else {
        $('.filter select:enabled').show();
    }
}
//functionto remove Images
function RemoveImages() {
    //div_img_header
    $('#div_image_portion').hide();
    $('#div_img_header').html('');
    $('#selection_Config').show();
    //div_General_Config
    $('#div_General_Config').hide();
}
//function to trigger vehicle type Id
function ShowComponentConfig() {
    $('#componentDetailsConfig').show();
    $("#selection_Config").hide();
    $("#dialogue").show();
    $("#overlay").show();

    //methods to hide dropdown and image, show general
    $('#div_image_portion').hide();
    $('#div_img_header').html('');
    $('#selection_Config').hide();
    $('#div_General_Config').show();

    //hide loading
    $('.loading').hide();
}
//function to fill sub  dropdown list value
function FillSubDropDown() {
    for (var i = 0; i < TotalData.length; i++) {
        if (TotalData[i].ConfigurationTypeId == vehicleTypeId) {
            //console.log(TotalData[i]);
            return TotalData[i].LstVehcCompTypes; //checks for the parameters for the selected vehicle Type Id
        }
    }
}
//function to get vehicle Name
function GetVehicleName() {
    return vehicleName;
}
function GetAppVehicleName() {
    return appvehicleName;
}
//function to get the exact number of the second dropdown selected
function GetExactNum() {
    var j;
    for (var i = 0; i < TotalData.length; i++) {
        if (TotalData[i].ConfigurationTypeId == vehicleTypeId) {
            j = i;
        }
    }
    ShowComponentConfig();
    return j;
}
//function to get the component image based on component Id
function FillComponentId(vehicleID) {
    $.ajax({
        url: "../VehicleConfig/GetComponentType",
        type: 'POST',
        cache: false,
        data: { vehicleId: vehicleID },
        success: function (result) {
            //var vhclImgList = [];
            //var selectedValue = GetDropDownNumSecond();

            //var datalen = result.components.length;
            //var vehicleTypelst = FillSubDropDown();//method to get the vehicleType list based on the vehicle type

            $.when(FillSubDropDown()).then(FillRemainingData(result));
            //var typeLen = vehicleTypelst.length;

            //vhclLatLongPos = [];
            //for (var j = 0; j < datalen; j++) {
            //    for (var i = 0; i < typeLen; i++) {                        
            //        if (vehicleTypelst[i].ComponentTypeId == result.components[j].ComponentId) {
            //            vhclImgList.push(vehicleTypelst[i]);
            //            //selectedValue = i;
            //            vhclLatLongPos.push(result.components[j]);

            //        }
            //    }                    
            //}
            //if (vhclImgList.length > 0) {
            //    vehcileCompTypeList[selectedValue] = vhclImgList;
            //}
            //);
        }
    });
}
//function to get the Application component image based on component Id
function FillAppComponentId(vehicleID) {
    $.ajax({
        url: "../VehicleConfig/GetAppComponentType",
        type: 'POST',
        cache: false,
        data: { vehicleId: vehicleID },
        success: function (result) {
            //var vhclImgList = [];
            //var selectedValue = GetDropDownNumSecond();

            //var datalen = result.components.length;
            //var vehicleTypelst = FillSubDropDown();//method to get the vehicleType list based on the vehicle type

            $.when(FillSubDropDown()).then(FillRemainingData(result));
            //var typeLen = vehicleTypelst.length;

            //vhclLatLongPos = [];
            //for (var j = 0; j < datalen; j++) {
            //    for (var i = 0; i < typeLen; i++) {                        
            //        if (vehicleTypelst[i].ComponentTypeId == result.components[j].ComponentId) {
            //            vhclImgList.push(vehicleTypelst[i]);
            //            //selectedValue = i;
            //            vhclLatLongPos.push(result.components[j]);

            //        }
            //    }                    
            //}
            //if (vhclImgList.length > 0) {
            //    vehcileCompTypeList[selectedValue] = vhclImgList;
            //}
            //);
        }
    });
}
//function to get the VR1 component image based on component Id
function FillVR1ComponentId(vehicleID) {
    
    $.ajax({
        url: "../VehicleConfig/GetVR1ComponentType",
        type: 'POST',
        cache: false,
        data: { vehicleId: vehicleID },
        success: function (result) {
            $.when(FillSubDropDown()).then(FillRemainingData(result));
        }
    });
}
function FillRemainingData(result) {
  
    var vhclImgList = [];
    var selectedValue = GetDropDownNumSecond();

    var datalen = result.components.length;

    var vehicleTypelst = FillSubDropDown();

    var typeLen = vehicleTypelst.length;

    vhclLatLongPos = [];
    for (var j = 0; j < datalen; j++) {
        for (var i = 0; i < typeLen; i++) {
            if (vehicleTypelst[i].ComponentTypeId == result.components[j].ComponentId) {
                vhclImgList.push(vehicleTypelst[i]);
                //selectedValue = i;
                vhclLatLongPos.push(result.components[j]);

            }
        }
    }
    if (vhclImgList.length > 0) {
        vehcileCompTypeList[selectedValue] = vhclImgList;
    }
}
//function to import config position on create
function ImportComponents(vehicleConfigId) {
    var Regbtn = true;
    //console.log(vehcileCompTypeList[vehcileCompNum]);
    var datalength = vehcileCompTypeList[vehcileCompNum].length;
    var isVR1 = $('#vr1appln').val();
    var isNotify = $('#ISNotif').val();

    var isNotify = $('#ISNotif').val();
    if (isNotify == 'True' || isNotify == 'true') {
        isVR1 = 'True';
    }

    for (var i = 0; i < datalength; i++) {
        var vhcleTypeId = vehcileCompTypeList[vehcileCompNum][i].ComponentTypeId;
        var latpostion = i + 1;
        var longposition = 1;
    
        if (fromsidebyside) {
            vhcleTypeId = vhclLatLongPos[i].ComponentId;
            latpostion = vhclLatLongPos[i].LatPos;
            longposition = vhclLatLongPos[i].LongPos;
        }
        //console.log(vehcileCompTypeList[vehcileCompNum][i].ComponentTypeId);

        var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
        
        if (isNotify == 'True' || isNotify == 'true') {
            ImportVR1VehicleComponentPos(vehicleConfigId, 0, vhcleTypeId, latpostion, longposition);
            RegBtn = false;
        }
        else if (ApplicationRevId == 0) {
            ImportComponentPos(vehicleConfigId, 0, vhcleTypeId, latpostion, longposition);
            RegBtn = true;
        }
        else if (isVR1 == 'True' || isVR1 == 'true') {
            ImportVR1VehicleComponentPos(vehicleConfigId, 0, vhcleTypeId, latpostion, longposition);
            RegBtn = false;
        }
        else {
            ImportAppVehicleComponentPos(vehicleConfigId, 0, vhcleTypeId, latpostion, longposition);
            RegBtn = false;
        }
    }

    $('#div_General_Config').hide();
    $("#div_Registration").load('../VehicleConfig/RegistrationConfiguration?vehicleId=' + vehicleConfigId + '&RegBtn='+RegBtn+'&isVR1='+isVR1+'');
    $('.err_formalName').text('');

    $('#selection_Config').find('select').prop('disabled', 'disabled');
}
//function to save config position
function ImportComponentPos(vehicleConfigId, compId, vhclTypeId, latPosition, longPosition) {
    $.ajax({
        url: "../VehicleConfig/ImportComponent",
        type: 'POST',
        cache: false,
        data: { vehicleConfigId: vehicleConfigId, componentId: compId, vehicleTypeId: vhclTypeId, latitudePos: latPosition, longitudePos: longPosition },
        success: function (result) {
            if (result.Success) {
                //method
                AjaxGetComponentDesc();
            }
            else {
                
            }
        }
    });
}
//function to save application config position
function ImportAppVehicleComponentPos(vehicleConfigId, compId, vhclTypeId, latPosition, longPosition) {
    $.ajax({
        url: "../VehicleConfig/ImportApplicationComponent",
        type: 'POST',
        cache: false,
        data: { vehicleConfigId: vehicleConfigId, componentId: compId, vehicleTypeId: vhclTypeId, latitudePos: latPosition, longitudePos: longPosition },
        success: function (result) {
            if (result.Success) {
                //method
                AjaxGetComponentDescFromApplication();
            }
            else {
                
            }
        }
    });
}
//function to save VR1 application vehicle config position
function ImportVR1VehicleComponentPos(vehicleConfigId, compId, vhclTypeId, latPosition, longPosition) {
    
    $.ajax({
        url: "../VehicleConfig/ImportVR1Component",
        type: 'POST',
        cache: false,
        data: { vehicleConfigId: vehicleConfigId, componentId: compId, vehicleTypeId: vhclTypeId, latitudePos: latPosition, longitudePos: longPosition},
        success: function (result) {
            if (result.Success) {
                //method
                AjaxGetComponentDescFromVR1();
            }
            else {
                
            }
        }
    });
}
//function get the number of selected dropdown during edit
function GetDropDownNumSecond() {
    var j;
    for (var i = 0; i < TotalData.length; i++) {
        if (TotalData[i].ConfigurationTypeId == vehicleTypeId) {
            j = i;
        }
    }
    return j;
}
//function to get lat-long pos based on given component Id
function GetLatLongbyId(componentId) {
    //console.log(vhclLatLongPos);
    for (var i = 0; i < vhclLatLongPos.length; i++) {
        if (vhclLatLongPos[i].ComponentId == componentId) {
            return vhclLatLongPos[i];
        }
    }
}
//function  to get lat-long positions
function GetLatLongPos() {
    return vhclLatLongPos;
}
//function to select number of side by side component
function FillVhclCompSidebySideSelect() {
    ShowSideBySideDrop();
    EnableSideBySideDrop();
}
//function to hide  sidebyside select dropdown
function HideSideBySideDrop() {
    $('#div_sidebysideSelect').hide();
}
//function to show  sidebyside select dropdown
function ShowSideBySideDrop() {
    $('#div_sidebysideSelect').show();
}
//function to disable  sidebyside select dropdown
function DisableSideBySideDrop() {
    $('#sidebyside').attr('disabled', 'disabled');
}
//function to enable  sidebyside select dropdown
function EnableSideBySideDrop() {
    $('#sidebyside').attr('disabled', false);
}
//function to deselect sidebyside
function DeselectSideBySide() {
    $('#sidebyside').val('');
}
//function change  side by side select
function sidebysideNumChange(_this) {
    var _thisValue = $(_this).val();
    if (_thisValue != '') {
        FillSideBySide(_thisValue);
    }
}
//function to fill selected side by side
function FillSideBySide(_thisVal) {
    RemoveSideBySideTable();
   
    $('#sidebysidetbl').append('<table style="width:100%;"><tr><td colspan="' + _thisVal + '">Please define the number of vehicle components in each row of the configuration.</td></tr><tr></tr><tr></tr></table>');
    for (var i = 1; i <= _thisVal; i++) {
        $('#sidebysidetbl table').find('tr').eq(1).append('<td>Row ' + i + '</td>');
        $('#sidebysidetbl table').find('tr').eq(2).append('<td><select class="selectRow"><option value="1">1</option><option value="2" selected="selected">2</option></select></td>');
    }
    ShowSideBySideDiv();

    $('#componentDetailsConfig').hide();
}
//function remove side by side table 
function RemoveSideBySideTable() {
    $('#sidebysidetbl table').remove();
}
//function Show Side by side 
function ShowSideBySideDiv() {
    $('#div_sidebyside_main').show();
}
//function Show Side by hide 
function HideSideBySideDiv() {
    $('#div_sidebyside_main').hide();
}
//function fill component select
function NavigateToComponentSelect() {
    var vhclCompSubList = FillSubDropDown();
    $('#div_RowComp').find('table').find('tr').remove();
    //var datalength = vehicleComponentList.length;
    var datalength = vhclCompSubList.length;

    var dataRows = $('#sidebyside').val();
    //console.log(dataRows);
    //console.log($('.selectRow').length);
    //check whether side by side selected
    var sidebyChecked = false;
    var totval = 0;
    $('.selectRow').each(function () {
        var sideByVal = $(this).val();
        if (sideByVal == 2) {
            sidebyChecked = true;
        }
        totval = parseInt(totval) + parseInt(sideByVal);
    });
    //console.log(totval);
    if (totval <= 8) {
        for (var j = 1; j <= dataRows; j++) {
            if (sidebyChecked) {
                var selectRowVal = $('.selectRow').eq(j - 1).val();
                if (selectRowVal == 1) {
                    $('#div_RowComp').find('table').append('<tr><td>Row ' + j + '</td><td><select id="' + j + '_1"></select></td><td></td></tr>');
                }
                else if (selectRowVal == 2) {
                    $('#div_RowComp').find('table').append('<tr><td>Row ' + j + '</td><td><select id="' + j + '_1"></select></td><td><select id="' + j + '_2"></select></td></tr>');
                }
            }
            else {
                $('#div_RowComp').find('table').append('<tr><td>Row ' + j + '</td><td><select id="' + j + '"></select></td></tr>');
            }
            for (var i = 0; i < datalength; i++) {
                if (sidebyChecked) {
                    var selectRowValue = $('.selectRow').eq(j - 1).val();
                    if (selectRowValue == 1) {
                        $('#div_RowComp').find('#' + j + '_1').append('<option value="' + vhclCompSubList[i].ComponentTypeId + '" compName="' + vhclCompSubList[i].ComponentName + '" compImg="' + vhclCompSubList[i].ImageName + '" istractor="' + vhclCompSubList[i].IsTractor + '" latpos="' + j + '" longpos="1">' + vhclCompSubList[i].ComponentName + '</option>');
                    }
                    else if (selectRowValue == 2) {
                        $('#div_RowComp').find('#' + j + '_1').append('<option value="' + vhclCompSubList[i].ComponentTypeId + '" compName="' + vhclCompSubList[i].ComponentName + '" compImg="' + vhclCompSubList[i].ImageName + '" istractor="' + vhclCompSubList[i].IsTractor + '" latpos="' + j + '" longpos="1">' + vhclCompSubList[i].ComponentName + '</option>');
                        $('#div_RowComp').find('#' + j + '_2').append('<option value="' + vhclCompSubList[i].ComponentTypeId + '" compName="' + vhclCompSubList[i].ComponentName + '" compImg="' + vhclCompSubList[i].ImageName + '" istractor="' + vhclCompSubList[i].IsTractor + '" latpos="' + j + '" longpos="2">' + vhclCompSubList[i].ComponentName + '</option>');
                    }
                }
                else {
                    $('#div_RowComp').find('#' + j + '').append('<option value="' + vhclCompSubList[i].ComponentTypeId + '" compName="' + vhclCompSubList[i].ComponentName + '" compImg="' + vhclCompSubList[i].ImageName + '" istractor="' + vhclCompSubList[i].IsTractor + '" latpos="' + j + '" longpos="1">' + vhclCompSubList[i].ComponentName + '</option>');
                }
            }
        }

        $('#div_RowComp_main').show();
        $('.btn_back_row').show();


        //filter hide
        $('.filter').hide();
        $('#div_sidebyside_main').hide();

        $('.comp_row_error').text('');

        fromsidebyside = true;
    }
    else {
        //validation message to be shown
        $('.comp_row_error').text('The configuration contained ' + totval + ' components, which exceeds the maximum of 8 imposed by ESDAL.');
    }
}
//function to show selection
function NavigateBackToSelect() {
    $('.filter').not('#div_componentNum').show();
    //$('.filter select:enabled').show();    
    //$('#div_componentNum').hide();
    $('#div_sidebyside_main').show();

    $('#div_RowComp_main').hide();
}
//function to align image
function AlignImageSideBySide() {
    var IsSidebySide = false;
    //console.log('latCurrent');
    var _this = $('.vehiclecomponent');
    var imgLength = $('.vehiclecomponent').length;
    for (var i = 0; i < imgLength; i++) {
        //console.log(_this.eq(i).attr('longpos'));
        var latPrev = _this.eq(i).attr('latpos');
        for (var j = i + 1; j < imgLength; j++) {
            //console.log(_this.eq(j));
            var latCurrent = _this.eq(j).attr('latpos');
            if (latPrev == latCurrent) {
                //$('[latpos=' + latPrev + ']').eq(0).parent().addClass('sidebysidetop');
                //$('[latpos=' + latPrev + ']').eq(1).parent().addClass('sidebysidebottom');
                //console.log(latCurrent);
                _this.eq(i).addClass('sidebysidetop');
                _this.eq(j).addClass('sidebysidebottom');
                //console.log(_this.eq(i).parent().html());
                //$('[latpos=' + latPrev + ']').parent().removeClass('centre');
                _this.eq(i).removeClass('centre');
                _this.eq(j).removeClass('centre');

                IsSidebySide = true;
            }
        }
    }

    if (!IsSidebySide) {
        $('.vehiclecomponent').removeClass('centre');
    }
    else {
        $('#div_img_header').addClass('hasSidebySide');
    }
}
//function set long lat pos null
function SetLongLatNull() {
    vhclLatLongPos = [];
}
//fucntiom hide component Select
function HideRowComponent() {
    $('#div_RowComp_main').hide();
}
//function to check whether the field is a length field or not for View Config
function IsLengthFields(_this) {
    var isLengthField = false;
    var LengthField = $(_this).find('.span_unit').text();
    if (LengthField == 'ft') {
        isLengthField = true;
    }
    return isLengthField;
}
//function to check whether the textbox is a speed field or not
function IsSpeedField(_this) {
    var isSpeedField = false;
    var speedField = $(_this).find('.span_unit').text();
    if (speedField == 'mph') {
        isSpeedField = true;
    }
    return isSpeedField;
}
//function to convert mph to kph
function ConvertToKph(_this) {
    var mph = _this;
    var kph = mph * 1.6093;
    kph = kph.toFixed(3);
    return kph;
}
//function to convert feet and inches to metres
function ConvertToMetre(_this) {

    var text = _this;
    var onlyInch = 0;
    var onlyFeet = 0;
    var feet;
    var inches;

    if (text == "0\'0\"") {
        return null;
    }

    //to check whether the value is entered only in feet
    if (text.indexOf('\'') === -1 && text.indexOf('\"') === -1) {
        text = text + '\'' + 0 + '\"';
    }
        //to check whether the value is entered only in inches
    else if (text.indexOf('\'') === -1) {
        onlyInch = 1;
    }

    text = text.replace('\"', '');
    if (onlyInch == 0) {
        var splitValue = text.split('\'');
        feet = splitValue[0];
        inches = splitValue[1];
        if (inches == undefined) {
            inches = 0;
        }
    }
    else {
        feet = 0;
        inches = text;
    }

    var totalInches = feet * 12;
    var Inches = totalInches + (inches * 1);
    //var metres = Inches / 39.370;
    var cm = Inches * 2.54;
    var metres = cm / 100;
    metres = metres.toFixed(4);
    return metres;
}
//function to navigate to general components
function NavigateToGeneral() {
    if (ValidateNumTractors()) {
        FillImage();
        //console.log(vhclLatLongPos);
    }
}
function AutoFillSideBy() {
    vehicleTypeId = 244007;
    var thisVal = $('#VehicleTypeConfig');
    movementId = $('#MovementClassConfig').val();
    var rangemin = $('option:selected', thisVal).attr('rangemin');
    var rangemax = $('option:selected', thisVal).attr('rangemax');

    istractorfrnt = $('option:selected', thisVal).attr('istractorfrnt');
    var numtractor = $('option:selected', thisVal).attr('numtractor');
    var numtrailer = $('option:selected', thisVal).attr('numtrailer');

    var sidebyside = $('option:selected', thisVal).attr('sidebyside');
    //to get the list of vehicle component
    vehcileCompNum = $('option:selected', thisVal).attr('listvhclcompnum');
    $('#ComponentNum').val(vehcileCompNum);

    DisableSideBySideDrop();
    HideSideBySideDrop();
    DeselectSideBySide();
    HideSideBySideDiv();
    RemoveSideBySideTable();

    HideRowComponent();

    SetLongLatNull();

    fromsidebyside = false;

    $('#div_componentNum').hide();

    if (vehicleTypeId == "") {
        //$('#VehicleSubType').attr("disabled", "disabled");                 
    }
    else {
        if (rangemin == rangemax) {
            FillImage();
            //loadGeneralPage(movementId, vehicleTypeId);
            $('#div_componentNum').hide();
        }
        else {
            if (sidebyside == 0) {
                fillVehicleCompNumSelect(rangemin, rangemax); //if no side by side required, component number is displayed
            }
            else {
                //side by side required
                FillVhclCompSidebySideSelect();
            }
            $('#componentDetailsConfig').html('');
        }
        GetGlobalVehicleList();
        //FillVehicleSubType(movementId, vehicleTypeId);
    }
    $('#div_RowComp_main').hide();
}
function LoadConfigDetails(_this) {
   
    var compList = [];
    //$("#divAllComponent form").each(function () {
    //    var componentId = $(this).find("#Component_Id").val();
    //    compList.push({ ComponentId: componentId });

    //});
    var ComponentTotalWeight = 0;
    $(".comp").each(function () {
        var compId = $(this).attr('id');
        compList.push({ ComponentId: compId });
        var get_textbox_value = $('#' + compId).find('#Weight').val();
        if ($.isNumeric(get_textbox_value)) {
            ComponentTotalWeight += parseFloat(get_textbox_value);
        }
    });
    var vd = false;
    $.each(compList, function (key, value) {
        
        vd = validation(value.ComponentId);
        if (!vd) {
            
            document.getElementById(value.ComponentId).style.display = "block"
            document.getElementById('chevlon-up-icon_' + value.ComponentId).style.display = "block"
            document.getElementById('chevlon-down-icon_' + value.ComponentId).style.display = "none"
            $('#spnDetailStatus_' + value.ComponentId).text("Hide Details");
            EnableEditComponent(value.ComponentId);

            //$("div .error:not(:contains(' '))").focus();

            var container = $('div');
            var scrollTo = $(".error:not(:contains(' '))");

            $('html, body').animate({
                scrollTop: scrollTo.offset().top - container.offset().top + container.scrollTop()
            });
            return vd;
        }
    });
    if (vd) {

        var vehicleId = $("#vehicleId").val();
        var MovementClassConfig = $("#movement_Id").val();
        var VehicleTypeConfig = $("#vehicleType_Id").val();
        if (vehicleId != undefined) {
            var isMovement = $('#IsMovement').val();
            if (isMovement == "true" || isMovement == "True") {
                var isCandidate = false;
                if ($('#IsCandidate').val() != undefined && $('#IsCandidate').val() != "False" && $('#IsCandidate').val() != "false") {
                    isCandidate = true;
                    CandidateBackSubFlag = 1.1;
                }
                $.ajax({
                    url: "../VehicleConfig/GeneralPageEdit",
                    type: 'GET',
                    data: { vehicleId: vehicleId, MovementClassConfig: MovementClassConfig, VehicleTypeConfig: VehicleTypeConfig, movement: true, isEditVehicleInSoProcessing: isCandidate, grossWeight: ComponentTotalWeight },
                    //contentType: 'application/json; charset=utf-8',
                    async: false,
                    beforeSend: function () {
                        startAnimation();
                    },
                    success: function (data) {
                        SubStepFlag = 2.5;
                        var GUID = $('#GUID').val();
                        var vehicleConfigId = $('#vehicleConfigId').val();
                        var ComponentCount = $('#ComponentCount').val();
                        $("#vehicle_Component_edit_section").hide();

                        if (isCandidate) {
                            $("#tab_2").hide();
                            $("#tab_7").html($(data).find('#vehicles'));
                            $("#tab_7").show();
                            $('<input>').attr({
                                type: 'hidden',
                                id: 'GUID',
                                value: GUID
                            }).appendTo('#tab_7');
                            $('<input>').attr({
                                type: 'hidden',
                                id: 'IsMovement',
                                value: isMovement
                            }).appendTo('#tab_7');
                            $('<input>').attr({
                                type: 'hidden',
                                id: 'vehicleConfigId',
                                value: vehicleConfigId
                            }).appendTo('#tab_7');
                            $('<input>').attr({
                                type: 'hidden',
                                id: 'ComponentCount',
                                value: ComponentCount
                            }).appendTo('#tab_7');
                            $('<input>').attr({
                                type: 'hidden',
                                id: 'IsMovement',
                                value: isMovement
                            }).appendTo('#tab_7');
                            $('<input>').attr({
                                type: 'hidden',
                                id: 'ComponentTotalWeight',
                                value: ComponentTotalWeight
                            }).appendTo('#tab_7');
                            $('#divbtn_candidateVehicleEdit').remove();
                            $('#tab_7').append('<div id="divbtn_candidateVehicleNext" class="row mt-4" style="float: right;"><button class="btn outline-btn-primary SOAButtonHelper mr-2 mb-2 btn-candidate-vehicle-back-button">BACK</button></div>');

                        }
                        else {
                            $("#vehicle_edit_section").show();
                            $("#vehicle_edit_section").html('');
                            $("#vehicle_edit_section").html($(data).find('#vehicles'));
                            $('<input>').attr({
                                type: 'hidden',
                                id: 'GUID',
                                value: GUID
                            }).appendTo('#vehicle_edit_section');
                            $('<input>').attr({
                                type: 'hidden',
                                id: 'IsMovement',
                                value: isMovement
                            }).appendTo('#vehicle_edit_section');
                            $('<input>').attr({
                                type: 'hidden',
                                id: 'vehicleConfigId',
                                value: vehicleConfigId
                            }).appendTo('#vehicle_edit_section');
                            $('<input>').attr({
                                type: 'hidden',
                                id: 'ComponentCount',
                                value: ComponentCount
                            }).appendTo('#vehicle_edit_section');
                            $('<input>').attr({
                                type: 'hidden',
                                id: 'IsMovement',
                                value: isMovement
                            }).appendTo('#vehicle_edit_section');
                            $('<input>').attr({
                                type: 'hidden',
                                id: 'ComponentTotalWeight',
                                value: ComponentTotalWeight
                            }).appendTo('#vehicle_edit_section');
                        }
                        $('#btn_back_config_edit').hide();
                    },
                    error: function (data) {

                    },
                    complete: function () {
                        stopAnimation();
                    }
                });
            }
            else {
                window.location.href = "/VehicleConfig/GeneralPageEdit" + EncodedQueryString("vehicleId=" + vehicleId + "&MovementClassConfig=" + MovementClassConfig + "&VehicleTypeConfig=" + VehicleTypeConfig + "&grossWeight=" + ComponentTotalWeight);
            }
        }
        else {

            $.ajax({
                url: "../VehicleConfig/ConfigurationGeneralPage",
                type: 'GET',
                //contentType: 'application/json; charset=utf-8',
                async: false,
                success: function (data) {
                    var isMovement = $('#IsMovement').val();
                    if (isMovement == "true" || isMovement == "True") {
                        SubStepFlag = 2.2;
                        CandidateBackSubFlag = 0.2;
                        var GUID = $('#GUID').val();
                        var vehicleConfigId = $('#vehicleConfigId').val();
                        var ComponentCount = $('#ComponentCount').val();

                        $("#vehicle_Create_section").html('');
                        $("#vehicle_Create_section").html($(data).find('#vehicles'));
                        $("#btn_back_config").hide();
                        $('<input>').attr({
                            type: 'hidden',
                            id: 'GUID',
                            value: GUID
                        }).appendTo('#vehicle_Create_section');
                        $('<input>').attr({
                            type: 'hidden',
                            id: 'IsMovement',
                            value: isMovement
                        }).appendTo('#vehicle_Create_section');
                        $('<input>').attr({
                            type: 'hidden',
                            id: 'vehicleConfigId',
                            value: vehicleConfigId
                        }).appendTo('#vehicle_Create_section');
                        $('<input>').attr({
                            type: 'hidden',
                            id: 'ComponentCount',
                            value: ComponentCount
                        }).appendTo('#vehicle_Create_section');
                        $('<input>').attr({
                            type: 'hidden',
                            id: 'ComponentTotalWeight',
                            value: ComponentTotalWeight
                        }).appendTo('#vehicle_Create_section');
                    }
                    else {
                        $("#banner-container").html('');
                        $("#banner-container").html($(data).find('#banner-container'));
                        $('<input>').attr({
                            type: 'hidden',
                            id: 'ComponentTotalWeight',
                            value: ComponentTotalWeight
                        }).appendTo('#vehicles');
                    }
                    //vehicle config type options count check
                    if ($("#VehicleTypeConfig option").length == 2) {
                        var config = $("#VehicleTypeConfig option:eq(1)").val();
                        $("#VehicleTypeConfig option:eq(0)").remove();
                        $('#VehicleTypeConfig option[value=' + config + ']').attr('selected', 'selected');
                        $('#VehicleTypeConfig').hide();
                        var configName = $("#VehicleTypeConfig option:selected").text();
                        var msg = "Based on the details you've filled in, the Configuration type is  ";
                        $('#spnconfigmessage').html(msg + '<b>' + configName + '</b>');
                        movementNext();
                    }
                    else if ($("#VehicleTypeConfig option").length > 2) {
                        $('#VehicleTypeConfig').show();
                        $('#spnconfigmessage').html("Based on the details you've filled in, there are multiple Configuration types identified by the system. Please select one from the list.");
                    }
                    else if ($("#VehicleTypeConfig option").length < 2) {
                        $('#VehicleTypeConfig').hide();
                        $('#spnconfigmessage').html("Based on the details you've filled in, the system cannot identify a Configuration type. Please go back and add/edit the components.");
                    }
                },
                error: function (data) {

                }
            });
        }
    }
    else {
        //$('html, body').animate({
        //    scrollTop: ($("#btn_Next").offset().top)
        //});
        //$("#Weight").css("border-color", "red");
        //$("#Weight").focus();
    }
}
//Save all details of configuration
//function to Navigate to Register page
function SaveVehicleConfiguration(_this) {
    if (validationConfig()) {
        validateTable(_this);//isTableValid variable will set on this call
        if (!isTableValid) {
            return false;
        }
        //var isVr1 = $('#vr1appln').val();
        //var isNotify = $('#ISNotif').val();
        var editveh = $('#editveh').val();
        var vehcls = $('#VehicleClass').val();

        //var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;

        //var RoutePartId = $('#RoutePartId').val() ? $('#RoutePartId').val() : 0;

        //var contentRefNo = $('#CRNo').val() ? $('#CRNo').val() : "";

        var vehicleConfigurationId = 0;
        if ($("#vehicleId").val() != undefined) {
            vehicleConfigurationId = $("#vehicleId").val();
        }

        if (vehicleConfigurationId == 0) {
            SaveConfiguration(_this);

        }
        else {
            //method to update
            EditConfiguration(_this);
        }

        //saving or editing a VR1 vehicle config
        //Saving candidate route vehicles - added by Salih on 05-05-2015
        //var iscandidatevehicles = $('#IsCandVehicle').val();
        //if (iscandidatevehicles == 'True') {
        //    var candrevisionId = $('#revisionId').val();
        //    if (vehicleConfigurationId == 0) {
        //        SaveCandidateVehicleConfiguration(candrevisionId);
        //    }
        //    else {
        //        EditVR1VehicleConfiguration(RoutePartId);
        //    }
        //}
        //else if (isVr1 == 'True' || isVr1 == 'true') {
        //    if (vehicleConfigurationId == 0) {
        //        //added by poonam (13.8.14)
        //        var vr1contrefno = $('#VR1ContentRefNo').val() ? $('#VR1ContentRefNo').val() : "";
        //        var vr1versionid = $('#VersionId').val() ? $('#VersionId').val() : 0;
        //        //-----------
        //        if (vr1versionid == 0) {
        //            SaveNotifVehicleConfiguration(contentRefNo);
        //        }
        //        else {
        //            SaveVR1VehicleConfiguration(ApplicationRevId, vr1contrefno, vr1versionid);
        //        }
        //    }
        //    else {
        //        if (editveh == 1 && vehcls != 270001) {
        //            var gw = $('#Weight').val();
        //            var ol = $('#OverallLength').val();
        //            $('#GrossWeight').val(gw);
        //            $('#VehLength').val(ol);
        //        }
        //        EditVR1VehicleConfiguration(RoutePartId);
        //    }
        //}
        //else if (isNotify == 'True' || isNotify == 'true') {
        //    if (vehicleConfigurationId == 0) {

        //        SaveNotifVehicleConfiguration(contentRefNo);
        //    }
        //    else {
        //        EditVR1VehicleConfiguration(RoutePartId);
        //    }
        //}
        //else {
        //    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
        //    // method to save application configuration vehicle
        //    if (ApplicationRevId != 0) {

        //        if (vehicleConfigurationId == 0) {
        //            SaveApplicationVehicleConfiguration(ApplicationRevId);
        //        }
        //        else {
        //            EditAppVehicleConfiguration(ApplicationRevId);
        //        }
        //    }
        //    //saving or editing fleet vehicle config
        //    else if (vehicleConfigurationId == 0) {
        //        SaveConfiguration(_this);

        //    }
        //    else {
        //        //method to update
        //        EditConfiguration(_this);
        //    }
        //}
    }
}
function LoadVehicleConfigList() {
    window.location = "/VehicleConfig/VehicleConfigList";
}
function LoadAppVehicle() {
    CloseSuccessModalPopup();
    if (!SelectedVehicles.includes(vehicleConfigurationId)) {
        SelectedVehicles.push(vehicleConfigurationId);
    }
    LoadContentForAjaxCalls("POST", '../VehicleConfig/InsertMovementVehicle', { movementId: ApplnMovementId, vehicleId: vehicleConfigurationId, flag: 5 }, '#vehicle_details_section', '', function () {
        MovementSelectedVehiclesInit();
        VehicleDetailsInit();
        ViewConfigurationGeneralInit();
        GeneralVehicCompInit();
        MovementAssessDetailsInit();
    });
}
//function to Save configuration
function SaveConfiguration(_this) {
    var movementId = $('#MovementClassConfig').val();
    var configTypeId = parseInt($('#VehicleTypeConfig').val());
    var speedUnit = $('#SpeedUnits').val();

    if (speedUnit == undefined || speedUnit == '') {
        speedUnit = null;
    }

    var configList = {
        moveClassification: { ClassificationId: movementId },
        //vehicleConfigType: { ConfigurationTypeId: configTypeId },
        VehicleConfigParamList: []
    };

    // $('.div_config_general input:text,textarea').each(function () {
    $('#div_config_general input,textarea,span.lblParam').not(':hidden,:checkbox').each(function () {
        var name;
        if ($(this)[0].localName == "span") {
            name = $(this)[0].innerText;
        }
        else {
            name = $(this).val();
        }
        var model = $(this).attr("id");
        var datatype = $(this).attr("datatype");
        if ($('#UnitValue').val() == 692002) {
            if (IsLengthFields(this)) {
                name = ConvertToMetre(name);
            }
            //if (IsSpeedField(this)) {
            //    name = ConvertToKph(name);
            //}
        }
        configList.VehicleConfigParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
    });

    $(_this).attr('disabled', 'disabled');
    if (!validateConfigData(_this)) {
        $(_this).attr('disabled', false);
        return false;
    }
    if ($(_this).closest('tr').next('tr').length != 0) {
        $(_this).attr('disabled', false);
        return false;
    }
    var isVR1 = $('#vr1appln').val();
    var isNotify = $('#ISNotif').val();
    if (isNotify == 'True' || isNotify == 'true') {
        isVR1 = 'True';
    }
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;

    var registrationParams = [];

    $('.tbl_registration tbody .tr_config_Registration').each(function () {
        var regVal = $(this).find('.txt_register_config').val();
        if (regVal == "") { regVal = $(this).find('.txt_register_config').html(); }
        var fleetId = $(this).find('.txt_fleet_config').val();
        if (fleetId == "") { fleetId = $(this).find('.txt_fleet_config').html(); }
        if (fleetId != "" || regVal != "") {
            registrationParams.push({ RegistrationValue: regVal, FleetId: fleetId });
        }

    });

    //var applnMovementId = 0;
    var isMovement = false;
    if ($('#IsMovement').val() != undefined) {
        var isMovement = $('#IsMovement').val() == "False" ? false : true;
        if (isMovement == true) {
            ApplnMovementId = $('#MovementId').val();
            if (ApplnMovementId == undefined) {
                ApplnMovementId = $('#movementId').val();
            }
        }
    }
    var isCandidateVehicle = false;
    if ($('#IsCandVersion').val() == "True") {
        isCandidateVehicle = true;
    }
    var candrevisionid = 0;
    if ($('#LastCandRevisionId').val() != undefined) {
        candrevisionid = $('#LastCandRevisionId').val();
    }
    $.ajax({
        url: "../VehicleConfig/CreateConfiguration",
        //data: '{"vehicleConfigObj":' + JSON.stringify(configList) + '}',
        data: '{"vehicleConfigObj":' + JSON.stringify(configList) + ',"configTypeId":' + configTypeId + ',"speedUnit":' + speedUnit + ',"registrationParams":' + JSON.stringify(registrationParams) + ',"isMovement":' + isMovement + ',"AplnMovemntId":' + ApplnMovementId + ',"isCandidate":' + isCandidateVehicle + ',"CandRevisionId":' + candrevisionid +'}',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        async: false,
        beforeSend: function () {
            //$('#overlay_anim').show();
            $('.popoverlay').show();
            $('.config_body').hide();
        },
        success: function (data) {
            if (data.configId != 0) {
                vehicleConfigurationId = data.configId;
                ApplnMovementId = data.movementId;
                vehicleName = $('#Internal_Name').val();//set vehicle name for display
                if (isCandidateVehicle) {
                    $('#vehicle_Create_section').html('');
                    $('#vehicle_Component_edit_section').html('');
                    $('#vehicle_edit_section').html('');
                    ShowSuccessModalPopup("Vehicle saved successfully", "Show_CandidateRTVehicles");
                }
                else if (isMovement) {
                    $('#vehicle_Create_section').html('');
                    $('#vehicle_Component_edit_section').html('');
                    $('#vehicle_edit_section').html('');
                    ShowSuccessModalPopup("Vehicle saved successfully", "LoadAppVehicle");                    
                    }
                else {
                    ShowSuccessModalPopup("Vehicle saved successfully", "LoadVehicleConfigList");
                }
            }
            else {
                if (isCandidateVehicle) {
                    ShowErrorPopup("The vehicle cannot be added, as it is not of special order", 'CloseErrorPopup');
                    $('#btn_saveConfig').removeAttr('disabled');
                }
                else {
                    $('.err_formalName').text('Name already exists');
                }
                //formal name
            }
        },
        error: function (data) {
            ;
            //location.reload();
        },
        complete: function () {
            //$('#overlay_anim').hide();
            $('.popoverlay').hide();
            $('.config_body').show();
        }
    });

}
//function to Save configuration
function EditConfiguration(_this) {
    var movementId = $('#MovementClassConfig').val();
    var configTypeId = parseInt($('#VehicleTypeConfig').val());
    var vehicleId = parseInt($('#vehicleId').val());
    var speedUnit = $('#SpeedUnits').val();

    if (speedUnit == undefined || speedUnit == '') {
        speedUnit = null;
    }

    var configList = {
        moveClassification: { ClassificationId: movementId },
        //vehicleConfigType: { ConfigurationTypeId: configTypeId },
        VehicleConfigParamList: []
    };

    // $('.div_config_general input:text,textarea').each(function () {
    $('#div_config_general input,#div_config_general textarea,#div_config_general span.lblParam').not(':hidden,:checkbox').each(function () {
        var name;
        if ($(this)[0].localName == "span") {
            name = $(this)[0].innerText;
        }
        else {
            name = $(this).val();
        }
        var model = $(this).attr("id");
        var datatype = $(this).attr("datatype");
        if ($('#UnitValue').val() == 692002) {
            if (IsLengthFields(this)) {
                name = ConvertToMetre(name);
            }
            //if (IsSpeedField(this)) {
            //    name = ConvertToKph(name);
            //}
        }
        configList.VehicleConfigParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
    });

    
    var registrationParams = [];
    $('.tbl_registration tbody .tr_config_Registration').each(function () {

        var regVal = $(this).find('.txt_register_config').val();
        if (regVal == "") { regVal = $(this).find('.txt_register_config').html(); }
        var fleetId = $(this).find('.txt_fleet_config').val();
        if (fleetId == "") { fleetId = $(this).find('.txt_fleet_config').html(); }
        if (fleetId != "" || regVal != "") {
            registrationParams.push({ RegistrationValue: regVal, FleetId: fleetId });
        }

    });
    var planMovement = false;
    if ($('#PlanMovement').val() == "True" || $('#isMovement').val() == "true") {
        planMovement = true;
    }
    var isCandidateVehicle = false;
    if ($('#IsCandidate').val() != undefined && $('#IsCandidate').val() != "false" && $('#IsCandidate').val() != "False") {
        isCandidateVehicle = true;
    }
    $.ajax({
        url: "../VehicleConfig/UpdateVehicleConfiguration",
        //data: '{"vehicleConfigObj":' + JSON.stringify(configList) + '}',
        data: '{"vehicleConfigObj":' + JSON.stringify(configList) + ',"configTypeId":' + configTypeId + ',"vehicleId":' + vehicleId + ',"speedUnit":' + speedUnit + ',"registrationParams":' + JSON.stringify(registrationParams) + ',"planMovement":' + planMovement + ',"isCandidate":' + isCandidateVehicle +'}',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        async: false,
        beforeSend: function () {
        },
        success: function (data) {
            
            if (data.Success) {
                if (isCandidateVehicle) {
                    ShowSuccessModalPopup("Vehicle Updated succesfully", "Show_CandidateRTVehicles");
                }
                else if (planMovement) {
                    $('#vehicle_edit_section').html('');
                    $('#vehicle_Create_section').html('');
                    $('#vehicle_Component_edit_section').html('');
                    vehicleConfigurationId = vehicleId;
                    ApplnMovementId = $('#MovementId').val();

                    ShowSuccessModalPopup("Vehicle Updated succesfully", "ReloadVehicleList", 2);
                }
                else {
                    ShowSuccessModalPopup("Vehicle Updated succesfully", "LoadVehicleConfigList");
                }
            }
            else {

                $('.err_formalName').text('Name already exists');
                //formal name
            }
        },
        error: function (data) {
            
            //location.reload();
        },
        complete: function () {
            //$('#overlay_anim').hide();
            $('.popoverlay').hide();
            $('.config_body').show();
        }
    });
}
function ShowGeneralPage() {
    $("#divGeneralpage").show();
}
function movementNext() {
    var configId = $("#VehicleTypeConfig").val();
    var mvmntType = $("#MovementClassConfig").val();
    if (mvmntType != "0" && configId!="") {
        $('#divGeneralpage').show();
        $('#divConfigGeneralPage').show();
        $('#divMovement').closest('div').find('.error').text('');
        $.ajax({
            url: '../VehicleConfig/GeneralPage',
            data: '{"movementId":' + mvmntType + ',"vehicleConfigId":' + configId + '}',
            type: 'POST',
            //contentType: 'application/json; charset=utf-8',
            async: false,
            success: function (data) {
                if (data) {
                    $('#divConfigGeneralPage').html(data);
                    $('#divConfigGeneralPage').show("Blind");
                    loadRegistartionPage();
                }
            },
            error: function (data) {               
            }
        });
    }
    else {
        $('#divConfigGeneralPage').hide();
        $('#divGeneralpage').hide();
        $('#divMovement').closest('div').find('.error').text('Please select configuration type');
    }
}
function loadRegistartionPage() {
    $.ajax({
        url: '../VehicleConfig/RegistrationConfiguration',
        data: '{"vehicleId":' + 0 + ',"RegBtn":' + true + ',"isVR1":' + false + '}',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        async: false,
        success: function (data) {
            if (data) {
                $('#regDetails').html(data);
                $('#regDetails').show("Blind");
                $("#btn_saveConfig").show();
            }
        },
        error: function (data) {
            alert("error");
        }
    });
}