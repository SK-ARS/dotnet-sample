var CountVal = $('#hf_Count').val();
var RtpartidVal = $('#hf_Rtpartid').val();

function Sort_AgreedRouteInit() {
    CountVal = $('#hf_Count').val();
    RtpartidVal = $('#hf_Rtpartid').val();
    CheckOnlyRoutePart();
    $("#RoutePartsDescription").hide();

    if (CountVal == "1") {
        if ($('#hf_IsNotifRouteVehicle').val() == "True" || $('#hf_IsNotifRouteVehicle').val() == "true") {
            $('#rbtn1').attr('checked', true);
        }
        if ($('#hf_IsNotifRoute').val() == "True") {
            GetNotifRoutePartID(RtpartidVal);
            $('#rbtn').attr('checked', true);
        }
    } else {
        $("input.rbn_routepart-agreed-routes:radio:first").prop("checked", true).trigger("click");
    }
}
function GetAgreedRoutePartID(RoutePartId) {
    $("#RoutePartsDescription").show();
    startAnimation();
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '../Routes/GetAgreedRoute',
        data: { routePartId: RoutePartId },
        beforeSend: function (xhr) {
        },
        success: function (result) {
            $("#map").html('');
            $("#idstrDesc").html('');
            $("#idEndDesc").html('');
            $("#Tabvia").html('');

            $("#map").show();
            if (result.routePartDetails.routeDescr != null) {
                $('#trHeaderDescription1').show();
                $('#trdesc1').show();
                $('#RouteDesc1').html(result.routePartDetails.routeDescr);
            }
            $('#RoutePartsDescription').show();
            $("#map").load('../Routes/A2BPlanning?routeID=0', function () {
                $("#map").addClass("context-wrap olMap");
                loadmap('DISPLAYONLY', result);
                var count = -1, strTr, Index = 0;
                if (result != null)
                    count = result.routePathList[0].routePointList.length;
                for (var i = 0; i < count; i++) {
                    if (result.routePathList[0].routePointList[i].pointType == 0)
                        $('#idstrDesc').html(result.routePathList[0].routePointList[0].pointDescr);
                    else if (result.routePathList[0].routePointList[i].pointType == 1)
                        $('#idEndDesc').html(result.routePathList[0].routePointList[1].pointDescr);
                    else if (result.routePathList[0].routePointList[i].pointType >= 2) {
                        Index = Index + 1;
                        strTr = "<tr ><td style='width:40px'>" + Index + "</td><td>" + result.routePathList[0].routePointList[i].pointDescr + "</td></tr>"
                        $('#Tabvia').append(strTr);
                    }
                }
                A2BPlanningInit();
                stopAnimation();
            })
            
        }
    });
}
function GetDetailNotifRoutePartID(RoutePartId) {

    $("#RoutePartsDescription").show();

    $('#RoutePart').hide();
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '../Routes/GetAgreedRoute',
        data: { routePartId: RoutePartId },
        beforeSend: function (xhr) {
        },
        success: function (result) {
            $("#map").html('');
            $("#Starting").html('');
            $("#Ending").html('');
            $("#Tabvia").html('');

            $("#map").show();
            $('#RoutePartsDescription').show();
            $("#map").load('../Routes/A2BPlanning?routeID = 0', function () {
                $("#map").addClass("context-wrap olMap");
                loadmap('DISPLAYONLY', result);
                var count = -1, strTr, Index = 0;
                if (result != null)
                    count = result.routePathList[0].routePointList.length;
                for (var i = 0; i < count; i++) {
                    if (result.routePathList[0].routePointList[i].pointType == 0)
                        $('#Starting').html(result.routePathList[0].routePointList[0].pointDescr);
                    else if (result.routePathList[0].routePointList[i].pointType == 1)
                        $('#Ending').html(result.routePathList[0].routePointList[1].pointDescr);
                    else if (result.routePathList[0].routePointList[i].pointType >= 2) {
                        Index = Index + 1;
                        strTr = "<tr ><td style='width:40px'>" + Index + "</td><td>" + result.routePathList[0].routePointList[i].pointDescr + "</td></tr>"
                        $('#Tabvia').append(strTr);
                    }
                }
            })
            stopAnimation();
        }
    });
}
function GetNotifRoutePartID(Routepartid) {

    if ($('#hf_ViewRout').val() != 'True') {
        $('#TDaddButtons').html('<button class="btn_reg_back next btngrad btnrds btnbdr" id="btnBackToMovementList"  type="button" data-icon="" aria-hidden="true">Back</button>');
        $('#TDaddButtons').append('<button class="btn_reg_back next btngrad btnrds btnbdr" id="btnImpRouteInDetailNotif" data-id=' + Routepartid + '  type="button" data-icon="&#xe0f4;" aria-hidden="true">Import</button>');
    }
    GetRoutePartID(Routepartid);
}


function CheckOnlyRoutePart() {
    var len = $('.rbn_routepart-agreed-routes').length;
    if (len == 1) {
        $('.rbn_routepart-agreed-routes').prop('checked', 'checked');
        $('.rbn_routepart-agreed-routes').trigger('click');
    }
}

$(document).ready(function () {
    $('body').on('click', '.div-agreed-routes #btnNotifRoutePart', function (e) {
        var RoutePartID = $(this).data('routepartid');
        GetNotifRoutePartID(RoutePartID);
    });
    $('body').on('click', '.div-agreed-routes #btnNotifVehicleRoutePart', function (e) {
        var RoutePartID = $(this).data('routepartid');
        GetNotifVehicleRoutePartID(RoutePartID);
    });
    $('body').on('click', '.div-agreed-routes #btnRoutePart', function (e) {
        var RoutePartID = $(this).data('routepartid');
        GetAgreedRoutePartID(RoutePartID);
    });
    $('body').on('click', '.div-agreed-routes #btnBackToMovementList', function (e) {
        BackToMovementList(this);
    });

    $('body').on('click', '.div-agreed-routes #btnImpRouteInDetailNotif', function (e) {
        var id = $(this).data('id');
        ImpRouteInDetailNotif(id);
    });
});
