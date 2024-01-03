var compName;
var vehicleId;
var view = false;
//$(document).ready(function () {
//    var url = '../VehicleConfig/VehicleConfigBtn';
//    loadLeftSearchPanel(url);
//    LoadQuickLinks();
//    //load_leftpanel();//method for loading partial view of createcomponent button in left panel
//    selectedmenu('Fleet');
//    view = false;
//    fillPageSizeSelect();
//    closeModal();
//});

//function defenition for loading partial view of createconfiguration button in left panel
function load_leftpanel() {
    //console.log();
    startAnimation();
    $('#leftpanel').load('../VehicleConfig/VehicleConfigBtn', function () {
        CheckSessionTimeOut();

        LoadQuickLinks();
        stopAnimation();
    });
}

function LoadQuickLinks() {
    startAnimation();
    $('#leftpanel_quickmenu').load('../Home/QuickLinks', function () {
        CheckSessionTimeOut();

        stopAnimation();
    });
}

function Edit(id) {

    $("#dialogue").load('../VehicleConfig/PartialViewLayout', function () {
        $.ajax({
            url: '../VehicleConfig/CreateConfiguration',
            data: { vhclClassification: id, isApplicationVehicle: false, isVR1: false },
            type: 'GET',
            beforeSend: function () {
                startAnimation();
                $(".dyntitleConfig").html("Edit configuration");
                removescroll();
                DisableBackButton();
            },
            success: function (result) {
                $("#Config-body").html(result);

                CheckSessionTimeOut();
                EditConfigurationLoad(id);
            },

            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                location.reload();
            },
            complete: function () {

                stopAnimation();
                // $("#dialogue").show();
                //$("#overlay").show();

            }
            //removescroll();
        });
    });
}

//for application edit vehicle config
function EditApplication(id) {
    resetdialogue();
    //id = 692169;
    var isVR1 = $('#vr1appln').val();
    var isNotify = $('#ISNotif').val();
    $('#IsEdit').val(true);
    $('#IsEditVehicle').val(true);
    if (isNotify == 'True' || isNotify == 'true') {
        isVR1 = true;
    }
    $("#dialogue").load('../VehicleConfig/PartialViewLayout', function () {

        $.ajax({
            url: '../VehicleConfig/CreateConfiguration',
            data: { vhclClassification: id, isApplicationVehicle: true, isVR1: isVR1 },
            type: 'GET',
            beforeSend: function () {
                startAnimation();

            },
            success: function (result) {
                $("#Config-body").html(result);

                CheckSessionTimeOut();
                if (isVR1 == 'False') {
                    EditConfigurationLoadApplication(id);
                }
                else {
                    EditConfigurationLoadVR1(id, isNotify);
                }
            },

            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                location.reload();
            },
            complete: function () {

                stopAnimation();
                $(".dyntitleConfig").html("Edit configuration");
                removescroll();
                DisableBackButton();
                // $("#dialogue").show();
                //$("#overlay").show();

            }
            //removescroll();
        });
    });
}



function viewConfig(id) {
    view = true;
    ViewConfiguration(id);
}


function Delete(_this, id) {
    compName = $(_this).attr('name');
    vehicleId = id;
  
    var Msg = "Do you want to delete '" + "" + "'" + compName + "'" + "" + "' ?";
    showWarningPopDialog(Msg, 'No', 'Yes', 'WarningCancelBtn', 'DeleteData', 1, 'warning');
    DisableBackButton();
}

//function Create() {
//    startAnimation();
//    $("#dialogue").load('../VehicleConfig/PartialViewLayout', function () {
//        DynamicTitle('Create configuration');

//        $("#Config-body").load('../VehicleConfig/CreateConfiguration', function () {
//            stopAnimation(); 
//            $("#dialogue").show();
//            $("#overlay").show();
//            removescroll();
//            DisableBackButton();
//        });

//    });
//}

function Create() {

    $("#dialogue").load('../VehicleConfig/PartialViewLayout', function () {

        $.ajax({
            url: '../VehicleConfig/CreateConfiguration',
            data: { vhclClassification: 0, isApplicationVehicle: false, isVR1: false },
            type: 'GET',
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                $("#Config-body").html(result);
                CheckSessionTimeOut();
            },

            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                location.reload();
            },
            complete: function () {
                stopAnimation();
                $("#dialogue").show();
                $("#overlay").show();
                $(".dyntitleConfig").html("Create configuration");
                removescroll();
                DisableBackButton();
            }
           
        });
    });
}

function EditComponent(id) {
   
    $("#dialogue").load('../VehicleConfig/ViewConfiguration');
   
    $.ajax({
        url: '../VehicleConfig/ViewConfiguration',
        type: 'GET',
        cache: false,
        data: { vehicleSubTypeId: 0, vehicleTypeId: 0, movementId: 0, componentId: id },
        beforeSend: function () {
            $("#overlay").show();
            startAnimation();
        },
        success: function (result) {
            $('#Update_Component').html(result);
            CheckSessionTimeOut();
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            stopAnimation();
            DisableBackButton();
            $("#dialogue").show();
            removescroll();
            $("#overlay").show();
            $("#Component_Id").val(id);
            $(".dyntitle").html("Edit configuration");
        }
    });
}


function ViewConfiguration(id) {
    //$("#overlay").show();
    //$("#dialogue").show();
    $("#dialogue").load('../VehicleConfig/PartialViewLayout', function () {
        DynamicTitle('View configuration');

        $.ajax({
            url: '../VehicleConfig/ViewConfiguration',
            type: 'GET',
            cache: false,
            data: { vehicleID: id },
            beforeSend: function () {
                $("#overlay").show();
                $("#dialogue").hide();
                $('.loading').show();
            },
            success: function (result) {
                $('#Config-body').html(result);
                CheckSessionTimeOut();
            },
            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                location.reload();
            },
            complete: function () {
                Resize_PopUp(900);
                $("#dialogue").show();
                removescroll();
                $("#overlay").show();
                //$("#Component_Id").val(id);
                $('.loading').hide();
                DisableBackButton();
            }
        });

    });
}

function ClosePopUp() {
    //if (!view) {
    //    //showWarningDialog(message, btn1_txt, btn2_txt, btn1Action, btn2Action)
    //    //location.reload();
    //    showWarningPopDialog('Closing the window may end up in losing data. Continue?', 'Cancel', 'Ok', 'WarningCancelBtn', 'ReloadLocation', 1, 'warning');
    //}
    //else {
    //    //location.reload();
    //    //$("#content").removeClass("contentscroll");
    //    addscroll();
    //    EnableBackButton();
    //}
    addscroll();
    EnableBackButton(); 
}

//function for closing the edit configuration popup;
function EditconfigPopUPclose() {
    $('#overlay').hide();
    WarningCancelBtn();
    addscroll();
    EnableBackButton();
}

function closeModal() {
    //var pageSize = $('#pageSizeVal').val();
    //var pageNum = $('#pageNum').val();        
    $('#span-close').live('click', function () {
        addscroll();
        if (!view) {
            //showWarningDialog(message, btn1_txt, btn2_txt, btn1Action, btn2Action)
        }
        else {
            //location.reload();
            //$("#content").removeClass("contentscroll");
            EnableBackButton();
        }
    });
}


function changePageSize(_this) {
    var pageSize = $(_this).val();
    var isVR1 = $('#vr1appln').val();
    var movclassification = '';
    if ($('#vr1appln').length > 0) {
        if (isVR1 == 'True') {
            movclassification = "VR1";
        }
        else {
            movclassification = "Special Order"
        }
    }

    $.ajax({
        url: '../VehicleConfig/VehicleConfigList',
        type: 'GET',
        cache: false,
        async: false,
        data: { pageSize: pageSize, movclassification: movclassification },
        beforeSend: function () {
            $("#overlay").show();
            $('.loading').show();
        },
        success: function (result) {           
            $('#div_fleet').html($(result).find('#div_fleet').html());

            CheckSessionTimeOut();
            //location.reload();
            $('#pageSizeVal').val(pageSize);
            $('#pageSizeSelect').val(pageSize);
            var x = fix_tableheader();
            if (x == 1) $('#tableheader').show();
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            $("#overlay").hide();
            $('.loading').hide();
        }
    });


}

function fillPageSizeSelect() {
    var selectedVal = $('#pageSizeVal').val();
    $('#pageSizeSelect').val(selectedVal);
}


function DeleteData() {
    $.ajax({
        url: '../VehicleConfig/DeleteConfiguration',
        type: 'POST',
        cache: false,
        data: { vehicleID: vehicleId, vehicleName: compName },
        success: function (result) {
            if (result.success) {
                $('#pop-warning').hide();                
                showWarningPopDialog('Configuration  "' + compName + '" is deleted successfully', 'Ok', '', 'ReloadLocation', '', 1, 'info');
                //location.reload();
            }
            else {
                showWarningPopDialog('Configuration  "' + compName + '" is not deleted successfully', 'Ok', '', 'WarningCancelBtn', '', 1, 'error');
                location.reload();
            }
        }
    });
}

