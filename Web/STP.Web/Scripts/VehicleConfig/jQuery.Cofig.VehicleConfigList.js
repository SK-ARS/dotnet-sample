var sortTypeGlobal = 0;//0-asc
var sortOrderGlobal = 1;//type
$(document).ready(function () {
    closeFilters();
    if ($('#hf_IsPlanMovmentGlobal').length == 0)
        SelectMenu(5);
    $('body').on('change', '#pageSizeSelectvehicles', function () {
    var page = getUrlParameterByName("page", this.href);
    $('#pageNum').val(page);
    var pageSize = $(this).val();
    $('#pageSizeVal').val(pageSize);
    SearchVehicle(isSort = true, page);
});
    $('body').on('click', '.btnvehicle', function (e) {
        e.preventDefault();
        CreateConfiguration(this);
    });
    $('body').on('click', '.imgVehicleCloseFilter', function (e) {
        e.preventDefault();
        clearConfigSearch(this);
    });
    $('body').on('click', '.hrefViewConfig', function (e) {
        e.preventDefault();
        ViewConfiguration_PlanMovement(this);
    });
    $('body').on('click', '.hrefUseVehicle', function (e) {
        e.preventDefault();
        UseVehicleForMovementFn(this);
    });
    $('body').on('click', '.importVehicle', function (e) {
        e.preventDefault();
        ImportVehicleForNotif(this);
    });
    $('body').on('click', '.hrefDeleteVehicle', function (e) {
        e.preventDefault();
        DeleteConfiguration(this);
    });
    $('body').on('click', '.manageVehicleFav', function (e) {
        e.preventDefault();
        ManageVehicleFavourites(this);
    });
    $('body').on('click', '.btnClear', function (e) {
        e.preventDefault();
        clearConfigSearch(this);
    });
    $('body').on('click', '.btnSearch', function (e) {
        e.preventDefault();
        SearchVehicle(this);
    });

    $('body').off('click', "#vehiclepaginator .pagination li a");
    $('body').on('click', "#vehiclepaginator .pagination li a", function (e) {
        if ($('#hf_IsPlanMovmentGlobal').length == 0) {
            e.preventDefault();
            var pageNum = getUrlParameterByName("page", this.href);
            SearchVehicle(true, pageNum);
        }
    });
});
function clearConfigSearch() {

    $(':input', '#filters')
        .not(':button, :submit, :reset, :hidden')
        .val('')
        .removeAttr('selected');

    $("#FilterFavouritesVehConfig").prop("checked", false);
    SearchVehicle();
}

function SearchVehicle(isSort = false,page) {
    if ($('div#filters.vehicle-config-list-filter').length > 1) {
        $('div#filters.vehicle-config-list-filter').not(':last').remove();
    }
    var searchString = $('#searchText').val();
    var vehicleIntend = $('#Indend').val();
    var vehicleType = $('#VehType').val();


    var searchFavourites = $('#FilterFavouritesVehConfig').is(":checked");
    var filterFavourites = 0;
    if (searchFavourites) {
        filterFavourites = 1;
    }
    else {
        filterFavourites = 0;
    }
    closeFilters();
    $.ajax({
        url: '../VehicleConfig/SaveVehicleConfigSearch',
        type: 'POST',
        cache: false,
        beforeSend: function () {
            startAnimation();
        },
        data: {
            searchString: searchString, vehicleIntend: vehicleIntend, vehicleType: vehicleType, importFlag: $('#importFlag').val(), filterFavouritesVehConfig: filterFavourites,
            sortType: $('#filters #SortTypeValue').val(), sortOrder: $('#filters #SortOrderValue').val(),
            page: isSort ? page : 1, pageSize: $('#pageSizeVal').val()
        },
        success: function (response) {

            stopAnimation();
            if ($('#importFlag').val() == 'True' || $('#importFlag').val() == 'true') {
                //$('section#banner_list').html(response);
                SetResponse(response);
            }
            else {
                var result = $(response).find('section#banner_list').html();
                $('section#banner_list').html(result);
            }
        }
    });
}
function Delete(_this, id) {
    startAnimation();
    compName = $(_this).attr('name');
    vehicleId = id;
    vehicleName = compName;
    var Msg = "Do you want to delete '" + "" + "'" + compName + "'" + "" + "' ?";
    ShowWarningPopup(Msg, "DeleteConfigurationFn");
    stopAnimation();
}
function DeleteConfigurationFn() {
    CloseWarningPopup();
    $.ajax({
        type: "POST",
        url: '../VehicleConfig/DeleteConfiguration',
        dataType: "json",
        data: { vehicleId: vehicleId, vehicleName: vehicleName },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            stopAnimation();
            if (result.success == true) {
                ShowModalPopup("'" + vehicleName + "'" + " " + "deleted successfully");
            }
            else if (result.success == false) {
                ShowErrorPopup("Deletion failed");
            }
        },
        error: function (result) {
            stopAnimation();
            ShowErrorPopup("Error on the page");
        }
    });
}
function CreateConfiguration() {
    //var url = "../VehicleConfig/CreateConfiguration";
    var url = "../VehicleConfig/CreateVehicle";
    window.location.href = url;
}
function VehicleConfigSort(event, param) {
    sortOrderGlobal = param;
    sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;
    $('#filters #SortTypeValue').val(sortTypeGlobal);
    $('#filters #SortOrderValue').val(sortOrderGlobal);
    var page = $('#pageNumVal').val();
    SearchVehicle(isSort = true,page);
}
function ViewConfiguration_PlanMovement(e) {
    var configurationId = $(e).attr("configurationid");
    ViewConfigurationLoad(configurationId);
}
function UseVehicleForMovementFn(e) {
    var configurationId = $(e).attr("configurationid");
    UseVehicleForMovement(configurationId, 2);
}
function ImportVehicleForNotif(e) {
    var configurationId = $(e).attr("configurationid");
    var vehicleName = $(e).attr("vehiclename");
    ImportForNotif(configurationId, vehicleName);
}
function DeleteConfiguration(e) {
    var configurationId = $(e).attr("configurationid");
    Delete(e, configurationId);
}
function ManageVehicleFavourites(e) {
    var configurationId = $(e).attr("configurationid");
    var arg1 = $(e).attr("arg1");
    var arg2 = $(e).attr("arg2");
    ManageFavourites(configurationId, arg1, arg2);
}