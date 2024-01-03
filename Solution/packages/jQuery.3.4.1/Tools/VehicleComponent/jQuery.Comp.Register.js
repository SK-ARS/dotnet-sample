//global values for deleting registration&fleet id
var compId1 = '';
var IdNum1 = '';
var x = '';
var initCount = 0;
var vehicleTypeId = $('#vehicleTypeValue').val();
var isTractor = $('#Tractor').val();
   
$(function () {
    ButtonName();
    $(".page_help").attr('url', '../../Home/NotImplemented');
    //CloneRow();        
    //CloneRow(); 
    $('#PageNum').val(2);
});

//remove the fleetid - registration row 
function deleteRow(_this) {
    debugger;
    var compId = $('#Component_Id').val();
    var IdNum = $(_this).closest('td').find('input:hidden').val();
    compId1 = compId;
    IdNum1 = IdNum;
    x = _this;
    var Msg = 'Do you want to delete?';
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var isVR1 = $('#vr1appln').val();
    if (ApplicationRevId == 0 && !isVR1) {
        showWarningPopDialog(Msg, 'No', 'Yes', 'WarningCancelBtn', 'DeleteRegistration2', 1, 'warning');
    }
    else if (isVR1) {
        showWarningPopDialog(Msg, 'No', 'Yes', 'WarningCancelBtn', 'DeleteVR1CompRegistration2', 1, 'warning');
    }
    else {
        showWarningPopDialog(Msg, 'No', 'Yes', 'WarningCancelBtn', 'DeleteAppCompRegistration2', 1, 'warning');
    }
}

function DeleteRegistration2()
{
    DeleteRegistration(compId1, IdNum1, x);
    $('#pop-warning').hide();
        
}

function DeleteAppCompRegistration2() {
    DeleteAppcompRegistration(compId1, IdNum1, x);
    $('#pop-warning').hide();
}

function DeleteVR1CompRegistration2() {
    DeleteVR1compRegistration(compId1, IdNum1, x);
    $('#pop-warning').hide();
}

function CloneRow(_this) {
    //$('.btn_add_new').live('click', function () {
    //var _this = this;
    debugger;
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
        AddRegistration(_this);
    }
    else if (isVR1 == 'True' || isVR1 == 'true')
    {
        AddVR1CompRegistration(_this);
    }
    else {
        AddAppCompRegistration(_this);
    }

    //});

}



//add the fleetid and registrationid
function addfleetid(_this) {
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
function validatData(_this) {
    debugger;
    var isvalid = true;
    var this_regId = $(_this).closest('tr').find('.txt_register').val();
    var this_fleetId = $(_this).closest('tr').find('.txt_fleet').val();
        
        
    $('#div_reg_component_vehicle').find('.cls_regId').not(':last').each(function () {
        var prv_reg_txt = $(this).text();
        if (prv_reg_txt == this_regId && this_regId !='') {
            $('#div_reg_component_vehicle').find('#error_msg').text('Vehicle Registration already in use');
            isvalid = false;
        }
    });

    if (this_fleetId != '') {
        $('#div_reg_component_vehicle').find('.cls_fleetId').not(':last').each(function () {
            var prv_fleet_txt = $(this).text();
            if (prv_fleet_txt == this_fleetId) {
                $('#div_reg_component_vehicle').find('#error_msg').text('Fleet ID already in use');
                isvalid = false;
            }

                
        });
    }
    if (isTractor && (vehicleTypeId != 234001 && vehicleTypeId !=234002 && vehicleTypeId != 234003))
    {
        if (this_fleetId == '') {
            $('#div_reg_component_vehicle').find('#error_msg').text('Please enter a valid fleet id');
            isvalid = false;
        }
    }
        

    if (this_regId == '' && this_fleetId == '') {
        $('#div_reg_component_vehicle').find('#error_msg').text('Please select either registration plate or fleet id');
        isvalid = false;
    }
    return isvalid;
}

//clear error message
function ClearSpan() {
    $('#div_reg_component_vehicle').find('#error_msg').text('');
}


function AddRegistration(_this) {
    //$(_this).attr('disabled', 'disabled');
    var makeconfig = false;
    var vehicleConfigId = 0;
    if ($('#MakeConfig').is(':checked')) {
        makeconfig = true;
        vehicleConfigId = vehicleId;
    }
    //vehicleId
    var componentId = $('#Component_Id').val();

    var regVal = $(_this).closest('tr').find('.txt_register').val();
    var fleetId = $(_this).closest('tr').find('.txt_fleet').val();
    var result = false;
    var url = '../Vehicle/SaveRegistrationID';
    $.post(url, { compId: componentId, registrationId: regVal, fleetId: fleetId, vehicleConfigId: vehicleConfigId }, function (data) {
        debugger;
        if (data.Success != 0) {
            $(_this).closest('td').find('input:hidden').val(data.Success);
            $(_this).closest('tr').clone().find("input").each(function () {
                debugger;
                $(this).attr({
                    'value': ''
                });
                $(this).closest('td').find('input:text').val('');
                $(this).closest('tr').find('button:eq(0)').attr('disabled', false);
            }).end().appendTo(".tbl_registration");


            //addfleetid(_this);            
            $(_this).closest('tr').find('input:text').each(function () {
                debugger;
                var txt = $(this).val();
                $(this).hide();
                $(this).closest('td').find('span').text(txt);
                $(this).closest('tr').find('button:eq(1)').show();
            });
            $(_this).closest('tr').find('button:eq(1)').show();
            $(_this).hide();
        }
        else {
            $(_this).attr('disabled', false);
        }
    });
}

//AddAppCompRegistration for Application veh comp
function AddAppCompRegistration(_this) {
    //$(_this).attr('disabled', 'disabled');
    var componentId = $('#Component_Id').val();

    var regVal = $(_this).closest('tr').find('.txt_register').val();
    var fleetId = $(_this).closest('tr').find('.txt_fleet').val();
    var result = false;
    var url = '../Vehicle/SaveRegistrationID';
    $.post(url, { compId: componentId, registrationId: regVal, fleetId: fleetId, isApplication:true }, function (data) {
        if (data.Success != 0) {
            $(_this).closest('td').find('input:hidden').val(data.Success);
            $(_this).closest('tr').clone().find("input").each(function () {
                $(this).attr({
                    'value': ''
                });
                $(this).closest('tr').find('button:eq(0)').attr('disabled', false);
            }).end().appendTo(".tbl_registration");


            //addfleetid(_this);            
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

//AddVR1CompRegistration for Application veh comp
function AddVR1CompRegistration(_this) {
    //$(_this).attr('disabled', 'disabled');
    var componentId = $('#Component_Id').val();

    var regVal = $(_this).closest('tr').find('.txt_register').val();
    var fleetId = $(_this).closest('tr').find('.txt_fleet').val();
    var result = false;
    var url = '../Vehicle/SaveRegistrationID';
    $.post(url, { compId: componentId, registrationId: regVal, fleetId: fleetId, isApplication: true ,isVR1: true}, function (data) {
        if (data.Success != 0) {
            $(_this).closest('td').find('input:hidden').val(data.Success);
            $(_this).closest('tr').clone().find("input").each(function () {
                $(this).attr({
                    'value': ''
                });
                $(this).closest('tr').find('button:eq(0)').attr('disabled', false);
            }).end().appendTo(".tbl_registration");


            //addfleetid(_this);            
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

function DeleteRegistration(compId, IdNum, _this) {
    $.ajax({
        url: '../Vehicle/DeleteVehicleRegistration',
        type: 'POST',
        cache: false,
        data: { componentId: compId, IdNumber: IdNum, vehicleConfigId: vehicleId },
        success: function (result) {
            if (result.Success) {
                $(_this).closest('tr').remove();
            }
            else {
                alert("Error");
            }
        }
    });
}

//DeleteAppcompRegistration
function DeleteAppcompRegistration(compId, IdNum, _this) {
    $.ajax({
        url: '../Vehicle/DeleteVehicleRegistration',
        type: 'POST',
        cache: false,
        data: { componentId: compId, IdNumber: IdNum ,isApplication:true},
        success: function (result) {
            if (result.Success) {
                $(_this).closest('tr').remove();
            }
            else {
                alert("Error");
            }
        }
    });
}

//DeleteAppcompRegistration
function DeleteVR1compRegistration(compId, IdNum, _this) {
    $.ajax({
        url: '../Vehicle/DeleteVehicleRegistration',
        type: 'POST',
        cache: false,
        data: { componentId: compId, IdNumber: IdNum, isApplication: true ,isVR1: true},
        success: function (result) {
            if (result.Success) {
                $(_this).closest('tr').remove();
            }
            else {
                alert("Error");
            }
        }
    });
}

function ButtonName() {
    var configurableAxles = $('.AxleConfig').val();
    if (configurableAxles == 'True') {
        $('#btn_reg_finish').hide();
        $('#btn_reg_Next').show();           
    }
    else {
        $('#btn_reg_finish').show();
        $('#btn_reg_Next').hide();
        //HideFinishButton();
        if (typeof HideFinishButton == 'function') {
            HideFinishButton();
        }            
    }
}
   
// function to check whether any changes have been made in the Component Registration page
function CompareRegField() {
    var hasChange = false;      
    $('.tbl_registration').find('input:text').each(function () {
        var _txt = $(this).val();
        if (_txt != '') {
            hasChange = true;
            return false;
        }
    });
    return hasChange;
}
