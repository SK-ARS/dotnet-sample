var componentId;
var compName;
var isComponent = true;
$(function () {
    selectedmenu('Fleet');
    $(".page_help").attr('url', '../../Home/NotImplemented');
    view = false;
    fillPageSizeSelect();
});
//function defenition for loading partial view of createcomponent button in left panel
function load_leftpanel() {
    
    var url = '../Vehicle/VehicleComponentBtn';
    loadLeftSearchPanel(url);

}
function LoadQuickLinks() {
    startAnimation();
    $('#leftpanel_quickmenu').load('../Home/QuickLinks', function () {
        stopAnimation();
    });
}
function Edit(id) {
    EditComponent(id);

}
function View(id) {
    ViewComponent(id);
    view = true;
}
function Delete(_this, PComponentDescription) {
    componentId = $(_this).attr('id');
    compName = $(_this).attr('name');
    var Msg = "Do you want to delete '" + "" + "'" + PComponentDescription + "'" + "" + "' ?";
    showWarningPopDialog(Msg, 'No', 'Yes', 'WarningCancelBtn', 'DeleteData', 1, 'warning');
    DisableBackButton();
}
function Create() {
  
    $.ajax({
        url: '../Vehicle/CreateComponent',
        type: 'GET',
        cache: false,
        //data: { vehicleSubTypeId: 224002, vehicleTypeId: 234002, movementId: 270001, componentId: id },
        beforeSend: function () {
            $("#overlay").show();
            $("#dialogue").hide();
            $('.loading').show();
        },
        success: function (result) {
            $('#dialogue').html(result);
            isComponent = true;
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            $("#dialogue").show();
            removescroll();
            $('.loading').hide();
            DisableBackButton();
        }
    });
}
function EditComponent(id) {   
   
    $("#dialogue").load('../Vehicle/UpdateComponent', function () {
        $.ajax({
            url: '../Vehicle/GeneralComponent',
            type: 'GET',
            cache: false,
            data: { vehicleSubTypeId: 0, vehicleTypeId: 0, movementId: 0, componentId: id, isComponent: true },
            beforeSend: function () {
                $("#overlay").show();
                $("#dialogue").hide();
                $('.loading').show();
            },
            success: function (result) {
                $('#Update_Component').html(result);
            },
            error: function (xhr, textStatus, errorThrown) {
                location.reload();
            },
            complete: function () {
                $("#dialogue").show();
                removescroll();
                $("#overlay").show();
                $("#Component_Id").val(id);
                $("#IsEdit").val(true);
                $('.loading').hide();

                DisableBackButton();
            }
        });
    });
        
}
function ViewComponent(id) {
    
    //$("#edit-dialog").dialog("open");
    //$("#dialogue").load('@Url.Action("UpdateComponent", "Vehicle")');
    $("#dialogue").load('../Vehicle/UpdateComponent', function () {
        $.ajax({
            url: '../Vehicle/ViewComponent',
            type: 'GET',
            cache: false,
            data: { vehicleSubTypeId: 0, vehicleTypeId: 0, movementId: 0, componentId: id },
            beforeSend: function () {
                $("#overlay").show();
                $("#dialogue").hide();
                $('.loading').show();
                //$('.dyntitle').html('View component');
            },
            success: function (result) {                
                $('#Update_Component').html(result);
                    
            },
            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                location.reload();
            },
            complete: function () {
                $("#dialogue").show();
                $(".page_help").hide();
                removescroll();
                $("#overlay").show();
                $("#Component_Id").val(id);
                   
                $('.loading').hide();
                DisableBackButton();
            }
        });
    });
}
function closeModal() {

    //var pageSize = $('#pageSizeVal').val();
    //var pageNum = $('#pageNum').val();

    //$('#span-close').live('click', function () {
    var page = $('#PageNum').val();
    var hasChanged = false;

    //checking on which page we are presently in
    if (page == 1) {
        if (typeof CompareField == 'function') {
            //function_name is a function
            hasChanged = CompareField();
        }

    }
    else if (page == 2) {
        if (typeof CompareRegField == 'function') {
            //function_name is a function
            hasChanged = CompareRegField();
        }

    }
    else if (page == 3) {
        if (typeof CompareData == 'function') {
            //function_name is a function
            hasChanged = CompareData();
        }

    }

    // if changes have been made the warning has to be displayed otherwise the window may be closed without warning the user.
    if (!view && hasChanged) {
        showWarningPopDialog('Closing the window may end up in losing data. Continue?', 'Cancel', 'Ok', 'WarningCancelBtn', 'ReloadLocation', 1, 'warning');
    }
    else {                
        //location.reload();
        $('#overlay').hide();
        addscroll();
        EnableBackButton();
        //$("#content").removeClass("contentscroll");
    }

    //});
}
function changePageSize(_this) {
    var pageSize = $(_this).val();


    $.ajax({
        url: '../Vehicle/FleetComponent',
        type: 'GET',
        cache: false,
        async: false,
        data: { pageSize: pageSize },
        beforeSend: function () {
            $("#overlay").show();
            $('.loading').show();
        },
        success: function (result) {
            
            $('#div_fleet').html($(result).find('#div_fleet').html());
            //location.reload();
            $('#pageSizeVal').val(pageSize);
            $('#pageSizeSelect').val(pageSize);
            $('#pagesize').val(pageSize);
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
function ClosePopUp() {
    $('#popup_Cancle').live('click', function () {
        showWarningPopDialog('Closing the window may end up in losing data. Continue?', 'Cancel', 'Ok', 'WarningCancelBtn', 'ReloadLocation', 1, 'warning');
    });    
}
function DeleteData() {        
    $.ajax({
        url: '../Vehicle/DeleteVehicleComponent',
        type: 'POST',
        cache: false,
        data: { componentId: componentId },
        success: function (result) {
            if (result.Success) {
                $('#pop-warning').hide();
                showWarningPopDialog('Component  "' + compName + '"  deleted successfully', 'Ok', '', 'ReloadLocation', '', 1, 'info');
            }
            else {
                alert("Error");
            }
        }
    });
}
function CheckIsComponent() {
    return isComponent;
}