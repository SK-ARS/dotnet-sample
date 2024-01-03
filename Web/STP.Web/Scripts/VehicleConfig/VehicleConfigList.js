var ViewBagObject = JSON.parse($('#ViewBagObject').val());
SelectMenu(5);

function clearConfigSearch() {
    $(':input', '#filters')
        .not(':button, :submit, :reset, :hidden')
        .val('')
        .removeAttr('selected');
    $("#FilterFavouritesVehConfig").prop("checked", false);
    SearchVehicle();
}

function SearchVehicle() {
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
        async: false,
        beforeSend: function () {
            startAnimation();
        },
        data: {
            searchString: searchString, vehicleIntend: vehicleIntend, vehicleType: vehicleType, importFlag: ViewBagObject.importFlag, filterFavouritesVehConfig: filterFavourites
        },
        success: function (response) {

            stopAnimation();
            if (ViewBagObject.importFlag == 'True') {
                $('section#banner_list').html(response);
            }
            else {
                var result = $(response).find('section#banner_list').html();
                $('section#banner_list').html(result);
            }

        }
    });
}

$('.dropdown-toggle').dropdown();

function openFilters() {
    document.getElementById("filters").style.width = "350px";
    document.getElementById("banner_list").style.filter = "brightness(0.5)";
    document.getElementById("banner_list").style.background = "white";
    document.getElementById("navbar").style.filter = "brightness(0.5)";
    document.getElementById("navbar").style.background = "white";
    function myFunction(x) {
        if (x.matches) { // If media query matches
            document.getElementById("filters").style.width = "200px";
        }
    }
    var x = window.matchMedia("(max-width: 770px)")
    myFunction(x) // Call listener function at run time
    x.addListener(myFunction)
}
function closeFilters() {
    document.getElementById("filters").style.width = "0";
    document.getElementById("banner_list").style.filter = "unset"
    document.getElementById("navbar").style.filter = "unset";

}
// Attach listener function on state changes
function Delete(_this, id) {
    startAnimation();
    compName = $(_this).attr('name');
    vehicleId = id;
    vehicleName = compName;
    var Msg = "Do you want to delete '" + "" + "'" + compName + "'" + "" + "' ?";
    ShowWarningPopup(Msg, "DeleteConfiguration");
    stopAnimation();
}
function DeleteConfiguration() {
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
                ShowModalPopup("'" + vehicleName + "'" + " " + "deleted successfully.");
            }
            else if (result.success == false) {
                ShowErrorPopup("Deletion failed");
            }
        },
        error: function (result) {
            stopAnimation();
            ShowErrorPopup("Error on the page.");
        }
    });
}


function CreateConfiguration() {
    //var url = "../VehicleConfig/CreateConfiguration";
    var url = "../VehicleConfig/CreateVehicle";
    window.location.href = url;
}
