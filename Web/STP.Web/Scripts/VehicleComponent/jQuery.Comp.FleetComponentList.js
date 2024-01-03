$(document).ready(function () {
    closeFilters();
    SelectMenu(5);
});
function clearSearch() {

    $(':input', '#filters')
        .not(':button, :submit, :reset, :hidden')
        .val('')
        .removeAttr('selected');
    $("#FilterFavourites").prop("checked", false);
    SearchVehicleComponent();
}
$('body').on('change', '.FleetComponent-Pag #pageSizeSelect', function () {
    var page = getUrlParameterByName("page", this.href);
    $('#pageNum').val(page);
    var pageSize = $(this).val();
    $('#pageSizeVal').val(pageSize);
    SearchVehicleComponent(isSort = true);
});
$('body').off('click', ".fleet-comp-container .fleet-comp-pagination .pagination li a");
$('body').on('click', ".fleet-comp-container .fleet-comp-pagination .pagination li a", function (e) {
    e.preventDefault();
    var pageNum = getUrlParameterByName("page", this.href);
    $('#pageNum').val(pageNum);
    SearchVehicleComponent(isSort = true);
});
function SearchVehicleComponent(isSort = false) {
    var searchString = $('#searchText').val();
    var vehicleIntend = $('#Indend').val();
    var vehicleType = $('#VehType').val();

    var IsFromConfig = $('#IsFromConfig').val();
    var searchFavourites = $('#FilterFavourites').is(":checked");
    var filterFavourites = 0;
    if (searchFavourites) {
        filterFavourites = 1;
    }
    else {
        filterFavourites = 0;
    }
    closeFilters();
    $.ajax({
        url: '../Vehicle/SaveSearchData',
        type: 'POST',
        cache: false,
        async: false,
        data: {
            searchString: searchString, vehicleIntend: vehicleIntend, vehicleType: vehicleType, filterFavourites: filterFavourites, isFromConfig: IsFromConfig,
            sortType: $('#filters #SortTypeValue').val(), sortOrder: $('#filters #SortOrderValue').val(),
            page: isSort ? $('#pageNum').val() : 1, pageSize: $('#pageSizeVal').val()
        },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {

            stopAnimation();
            var result = $(response).find('section#banner').html();
            $('section#banner').html(result);
        }
    });
}
//jQuery(document).ready(function ($) {
//    $(document).on('click', '.bs-canvas-overlay', function () {
//        $('.bs-canvas-overlay').remove();
//        document.getElementById("filters").style.margin = "0 -400px 0 0 ";
//        return false;
//    });
//});

function DeleteConfirmation(componentId,compName) {
    startAnimation();
    var Msg = "Do you want to delete '" + "" + "'" + compName + "'" + "" + "' ?";
    ShowWarningPopup(Msg, "DeleteFleetComponent", 'CloseWarningPopupRef', componentId, compName);
    stopAnimation();

}
// Attach listener function on state changes
function DeleteFleetComponent(componentId, compName) {
    CloseWarningPopup();
    $.ajax({
        async: false,
        type: "POST",
        url: '../Vehicle/DeleteVehicleComponent',
        dataType: "json",
        //contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ componentId: componentId }),
        beforeSend: function () {
            startAnimation();
        },
        processdata: true,
        success: function (result) {
            stopAnimation();
            if (result.Success == true) {
                ShowModalPopup("'" + compName + "'" + " " + "deleted successfully");
            }
            else if (result.Success == false) {
                ShowErrorPopup("Deletion failed");
            }
        },
        error: function (result) {
            stopAnimation();
            ShowErrorPopup("Error on the page");
        }
    });

}
function GoToCreateComponentPage() {
    var url = "../Vehicle/CreateComponent";
    window.location.href = url;
}
function Backbutton() {
    $.ajax({
        url: '../Vehicle/BackButtonToConfig',
        type: 'POST',
        success: function (response) {
            if (response != "") {
                window.location = "../vehicleConfig/CreateConfiguration" + EncodedQueryString("Guid=" + response);
            }
            else {
                window.location = "../vehicleConfig/CreateConfiguration";
            }
        }
    });
}
function ViewComponentDetail(componentId, mode) {
    window.location = "../Vehicle/GeneralComponent" + EncodedQueryString("componentId=" + componentId + "&mode=" + mode);
        //encodeURIComponent(String(EncryptedData("componentId=" + componentId + "&mode=" + mode)));
    //window.location = "../Vehicle/GeneralComponent?componentId=" + componentId + "&mode=" + mode;
}
var sortTypeGlobal = 0;//0-asc
var sortOrderGlobal = 1;//type
function VehicleCompSort(event, param) {

    sortOrderGlobal = param;
    sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;
    $('#filters #SortTypeValue').val(sortTypeGlobal);
    $('#filters #SortOrderValue').val(sortOrderGlobal);
    SearchVehicleComponent(isSort = true);
}
