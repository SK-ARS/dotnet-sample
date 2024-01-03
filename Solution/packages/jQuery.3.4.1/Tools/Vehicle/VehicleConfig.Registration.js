$(function () {
});
//global variables for deleting registration& fleet id 
var configId1 = '';
var IdNum1 = '';
var x = '';

//function for navigating back to general config page
function NavigateBackGeneralPage() {
    $('#div_General_Config').show();
    $("#div_Registration").html('');
}

function NavigateToSummaryPage() {
        $('#div_Registration').hide();
        $("#div_Summary").load('../VehicleConfig/SummariseConfig');
}

function AppNavigateToSummaryPage() {
    $('#div_Registration').hide();
    $("#div_Summary").load('../VehicleConfig/SummariseConfig?SummaryBtn=' + false + '');
}

function VR1NavigateToSummaryPage() {
    var notifId = $('#Notificationid').val();
    var configType = $('#VehicleTypeConfig').val();

    var registrationFound = validateRegistration();
    if (notifId == undefined || notifId == null || notifId == '' || configType == 244005) {
        registrationFound = 1;
    }
    if (registrationFound == 1) {
        $('#div_Registration').hide();
        $("#div_Summary").load('../VehicleConfig/SummariseConfig?SummaryBtn=' + false + '&isVR1=' + true);
    }
    else {
        $('#div_reg_config_vehicle').find('#error_msg_config').text('Please enter registration id to proceed further.');
    }
}

//remove the fleetid - registration row 
function deleteRowConfig(_this) {
    
    var configId = GetConfigurationId();
    var IdNum = $(_this).closest('td').find('input:hidden').val();
    configId1 = configId;
   
    IdNum1 = IdNum;
    x = _this;
    var Msg = 'Do you want to delete?';
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    console.log('ApplicationRevId : '+ApplicationRevId);
    var isVR1 = $('#vr1appln').val();
    console.log('isVR1 : ' + isVR1);
    var isNotify = $('#Notificationid').val();
    console.log('isNotify : ' + isNotify);
    if (isNotify != 0 && isNotify != null && isNotify != undefined && isNotify != '') {
        isVR1 = 'True';
    }
    var isAmend = $('#AmendVehicleId').val();
    console.log('isAmend : ' + isAmend);
    

    if (isAmend != null && isAmend != "" ) {
        configId1 = isAmend;
        showWarningPopDialog(Msg, 'No', 'Yes', 'WarningCancelBtn', 'DeleteVR1RegConfig1', 1, 'warning');
    }
    else if (ApplicationRevId == 0 && !isNotify) {
        showWarningPopDialog(Msg, 'No', 'Yes', 'WarningCancelBtn', 'DeleteRegistrationConfig1', 1, 'warning');
    }
    else if (isVR1 == 'True' || isVR1 == 'true') {
        showWarningPopDialog(Msg, 'No', 'Yes', 'WarningCancelBtn', 'DeleteVR1RegConfig1', 1, 'warning');
    }
    else {
        showWarningPopDialog(Msg, 'No', 'Yes', 'WarningCancelBtn', 'DeleteAppRegConfig1', 1, 'warning');
    }
}

function DeleteRegistrationConfig1() {
    DeleteRegistrationConfig(configId1, IdNum1, x);
    $('#pop-warning').hide();
}

function DeleteAppRegConfig1() {
    DeleteAppRegConfig(configId1, IdNum1, x);
    $('#pop-warning').hide();
}

function DeleteVR1RegConfig1() {
    DeleteVR1RegConfig(configId1, IdNum1, x);
    $('#pop-warning').hide();
}

function CloneRowConfig(_this) {
    
    //$('.btn_add_new').live('click', function () {
    //var _this = this;
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
    
    
    //for SO Application
    if (ApplicationRevId != 0 && isVR1 == 'False') {
        AddAppRegistrationConfig(_this, ApplicationRevId);
    }
    //for Vr1 application
    else if (isVR1 == 'True' || isVR1 == 'true') {
        AddVR1RegistrationConfig(_this);
    }
    else {
        AddRegistrationConfig(_this);
    }

    //});

}



//add the fleetid and registrationid
function addfleetConfigid(_this) {
    $(_this).hide();
    $(_this).next('button').show();
    $(_this).closest('tr').find('input').each(function () {
        var txt = $(this).val();
        $(this).hide();
        $(this).closest('td').find('span').text(txt);

    });
    return false;
}


//validate the fleetid and registrationid
function validateConfigData(_this) {
    var isvalid = true;
    var this_regId = $(_this).closest('tr').find('.txt_register_config').val();
    var this_fleetId = $(_this).closest('tr').find('.txt_fleet_config').val();

    $('.tbl_registration').find('.cls_regId').not(':last').each(function () {
        //$('.cls_regId_config').not(':last').each(function () {
        var prv_reg_txt = $(this).text();
        if (prv_reg_txt == this_regId && this_regId != '') {
            $('#div_reg_config_vehicle').find('#error_msg_config').text('Vehicle Registration already in use');
            //$('#error_msg_config').text('Vehicle Registration already in use');
            isvalid = false;
        }
    });

    if (this_fleetId != '') {
        $('.tbl_registration').find('.cls_fleetId').not(':last').each(function () {
            //$('.cls_fleetId_config').not(':last').each(function () {
            var prv_fleet_txt = $(this).text();
            if (prv_fleet_txt == this_fleetId) {
                $('#div_reg_config_vehicle').find('#error_msg_config').text('Fleet ID already in use');
                isvalid = false;
            }
        });
    }


    if (this_regId == '' && this_fleetId == '') {
        $('#div_reg_config_vehicle').find('#error_msg_config').text('Please enter either registration plate or fleet id');
        //$('#error_msg_config').text('Please enter either registration plate or fleet id');
        isvalid = false;
    }
    return isvalid;
}

//clear error message
function ClearSpanConfig() {
    $('#div_reg_config_vehicle').find('#error_msg_config').text('');
    //$('#error_msg_config').text('');
}

function AddRegistrationConfig(_this) {
    //$(_this).attr('disabled', 'disabled');
    var configId = GetConfigurationId();

    var regVal = $(_this).closest('tr').find('.txt_register_config').val();
    var fleetId = $(_this).closest('tr').find('.txt_fleet_config').val();
    var result = false;
    var url = "../VehicleConfig/SaveRegistrationID";
    $.post(url, { configId: configId, registrationId: regVal, fleetId: fleetId }, function (data) {
        if (data.Success != 0) {
            $(_this).closest('td').find('input:hidden').val(data.Success);
            $(_this).closest('tr').clone().find("input").each(function () {
                $(this).attr({
                    'value': ''
                });
                $(this).closest('tr').find('button:eq(0)').attr('disabled', false);
            }).end().appendTo(".tbl_registration");


            //addfleetConfigid(_this);            
            $(_this).closest('tr').find('input:text').each(function () {
                var txt = $(this).val();
                $(this).hide();
                $(this).closest('td').find('span').text(txt);
            });
            $(_this).closest('tr').find('button:eq(1)').show();
            $(_this).hide();
        }
        else {
            $(_this).attr('disabled', false);
        }
    });
}


function AddAppRegistrationConfig(_this, ApplicationRevId)
{
    
    //$(_this).attr('disabled', 'disabled');
    var configId = GetConfigurationId();

    var regVal = $(_this).closest('tr').find('.txt_register_config').val();
    var fleetId = $(_this).closest('tr').find('.txt_fleet_config').val();
    var result = false;
    var url = "../VehicleConfig/SaveAppVehicleRegistrationId";
    $.post(url, { configId: configId, registrationId: regVal, fleetId: fleetId , appRevId :ApplicationRevId}, function (data) {
        if (data.Success != 0) {
            $(_this).closest('td').find('input:hidden').val(data.Success);
            $(_this).closest('tr').clone().find("input").each(function () {
                $(this).attr({
                    'value': ''
                });
                $(this).closest('tr').find('button:eq(0)').attr('disabled', false);
            }).end().appendTo(".tbl_registration");


            //addfleetConfigid(_this);            
            $(_this).closest('tr').find('input:text').each(function () {
                var txt = $(this).val();
                $(this).hide();
                $(this).closest('td').find('span').text(txt);
            });
            $(_this).closest('tr').find('button:eq(1)').show();
            $(_this).hide();
        }
        else {
            $(_this).attr('disabled', false);
        }
    });
}


//function to save the VR1 vehicle registartion details
function AddVR1RegistrationConfig(_this) {
    //$(_this).attr('disabled', 'disabled');
    var configId = GetConfigurationId();
    if (configId == null || configId == undefined || configId == 0 || configId == '') {
        configId = $('#AmendVehicleId').val();
    }
    var regVal = $(_this).closest('tr').find('.txt_register_config').val();
    var fleetId = $(_this).closest('tr').find('.txt_fleet_config').val();
    var result = false;
    var url = "../VehicleConfig/SaveVR1VehicleRegistrationId";
    $.post(url, { configId: configId, registrationId: regVal, fleetId: fleetId}, function (data) {
        if (data.Success != 0) {
            $(_this).closest('td').find('input:hidden').val(data.Success);
            $(_this).closest('tr').clone().find("input").each(function () {
                $(this).attr({
                    'value': ''
                });
                $(this).closest('tr').find('button:eq(0)').attr('disabled', false);
            }).end().appendTo(".tbl_registration");


            //addfleetConfigid(_this);            
            $(_this).closest('tr').find('input:text').each(function () {
                var txt = $(this).val();
                $(this).hide();
                $(this).closest('td').find('span').text(txt);
            });
            $(_this).closest('tr').find('button:eq(1)').show();
            $(_this).hide();
        }
        else {
            $(_this).attr('disabled', false);
        }
    });
}

function DeleteRegistrationConfig(configId, IdNum, _this) {
    $.ajax({
        url: "../VehicleConfig/DeleteVehicleRegistration",
        type: 'POST',
        cache: false,
        data: { configId: configId, IdNumber: IdNum },
        success: function (result) {
            if (result.Success) {
                $(_this).closest('tr').remove();
            }
            else {
                alert("Error");
            }
        }
    });
    return false;
}


function DeleteAppRegConfig(configId, IdNum, _this) {
    $.ajax({
        url: "../VehicleConfig/DeleteAppRegConfig",
        type: 'POST',
        cache: false,
        data: { configId: configId, IdNumber: IdNum },
        success: function (result) {
            if (result.Success) {
                $(_this).closest('tr').remove();
            }
            else {
                alert("Error");
            }
        }
    });
    return false;
}

function DeleteVR1RegConfig(configId, IdNum, _this) {
    $.ajax({
        url: "../VehicleConfig/DeleteVR1RegConfig",
        type: 'POST',
        cache: false,
        data: { configId: configId, IdNumber: IdNum },
        success: function (result) {
            if (result.Success) {
                $(_this).closest('tr').remove();
            }
            else {
                alert("Error");
            }
        }
    });
    return false;
}

function validateRegistration() {
    var registrationFound = 0;
    var value;
    var isNotif = false;

    if ($('#DetailNotif').val == 1) {
        isNotif = true;
    }
    else if ($('#NotificatinId').val != null || $('#NotificatinId').val != 0 || $('#NotificatinId').val != undefined) {
        isNotif = true;
    }

    if (isNotif) {
        $('.tbl_registration').find('.cls_regId_config').each(function () {
            value = $(this).text();
            if (value != null && value != '') {
                registrationFound = 1;
            }
        });

        if (registrationFound == 0) {
            $('.tbl_registration').find('.cls_regId').each(function () {
                value = $(this).text();
                if (value != null && value != '') {
                    registrationFound = 1;
                }
            });
        }
    }
    else {
        registrationFound = 1;
    }
    return registrationFound;
}
function ClosePopUp() {
    $("#overlay").hide();
    $("#dialogue").hide();
    $('.error').text('');// added by Anlet on 16th Feb 2017 to resolve RM #6037(to clear the error messages on pop up close action)
    addscroll();
    var isAmend = $('#AmendVehicleId').val();
    if (isAmend != null && isAmend != undefined && isAmend != '') {
        NotifValidation();
    }
    $.ajax({
        url: "../Vehicle/ClearSession",
        type: 'POST',
        cache: false,
        async: false,
        success: function (result) {
        }
    });
    
}