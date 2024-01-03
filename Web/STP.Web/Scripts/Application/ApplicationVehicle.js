$(document).ready(function () {
    $('body').on('click', '.div-viewvehicle', function (e) {
        e.preventDefault();
        ViewVehicles(this);
    });
});
function routeDescription(id) {
    if (document.getElementById(id).style.display !== "none" && document.getElementById(id).style.display !== "") {
        document.getElementById(id).style.display = "none"
        document.getElementById('up-chevlon1').style.display = "none"
        document.getElementById('down-chevlon1').style.display = "block"
        $('#spnDetailStatus').text('Show Details');
    }
    else {
        document.getElementById(id).style.display = "block"
        document.getElementById('up-chevlon1').style.display = "block"
        document.getElementById('down-chevlon1').style.display = "none"
        $('#spnDetailStatus').text('Hide Details');
    }
}
function ViewVehicles(_this) {
    var param1 = $(_this).attr('arg1');
    ViewApplicationVehicle(param1, _this);
}
function ViewApplicationVehicle(id, $this) {
    var sortmovement = $('#movementId').val();
    var sortmov = $('#hf_SORTMOV').val();
    var vehicleid = $($this).attr("vehicle");
    var movementId = 0;
    var sort = false;
    var isRoute = false;
    if (sortmovement == null || sortmovement == 0) {
        movementId = $('#MoveId').val();
        isRoute = true;
    }
    else {
        if (sortmov == 'True') {
            var isNotif = true;
            var flag = "Notif";
            isRoute = false;
        }
        else {
            isRoute = true;
        }
        movementId = sortmovement;
        sort = true;
    }

    if ($('#hf_IsVRVeh').val() == 'True') {
        flag = 'VR1';
    }

    if ($('#' + vehicleid).html() == '') {
        startAnimation();
        $.ajax({
            url: '../VehicleConfig/ViewConfigDetails',
            type: 'POST',
            cache: false,
            async: false,
            data: { vehicleID: id, isRoute: isRoute, movementId: movementId, isNotif: isNotif, flag: "movement" },
            beforeSend: function () {
                //$("#overlay").show();
                //$("#dialogue").hide();
                //$('.loading').show();
            },
            success: function (response) {
                var result = $(response).find('div#vehicleconfig').html();
                $('#' + vehicleid).html(result);
                routeDescription(id);
                stopAnimation();
            },
            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                //location.reload();
                console.error(errorThrown);
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    else {
        routeDescription(id);
    }
}
// view vehicle configuration summary
function viewVehicleDetails(id, classCode) {
    $.ajax({
        url: '../VehicleConfig/ViewConfigDetails',
        type: 'POST',
        cache: false,
        async: false,
        beforeSend: function () {
            startAnimation();
        },
        data: {
            vehicleID: id, isRoute: isRoute, movementId: movementId, isNotif: isNotif, flag: flag
        },
        success: function (response) {
            stopAnimation();
            var result = $(response).find('div#vehicleconfig').html();
            $('div#vehicleconfig').html(result);
            vehicleInitialLoad = 0;
            $("#vehicleconfig").css("display", "block");
            $("#chevlon-up-icon1").css("display", "block");
            $("#chevlon-down-icon1").css("display", "none");
        }
    });
}