var soappVal = $('#hf_soapp').val();
var CountVal = $('#hf_Count').val();
var VrOneRouteVal = $('#hf_VrOneRoute').val();
var RouteFlagVal = $('#hf_RouteFlag').val();
var RouteTypeVal = $('#hf_RouteType').val();

function HaulierApplVehiclePartsInit() {
    soappVal = $('#hf_soapp').val();
    CountVal = $('#hf_Count').val();
    VrOneRouteVal = $('#hf_VrOneRoute').val();
    RouteFlagVal = $('#hf_RouteFlag').val();
    RouteTypeVal = $('#hf_RouteType').val();
    $('#Suggestedroute').hide();
    $('#Suggestedroute1').hide();
    var as = $('#hf_soapp').val();

    if ($('#hf_soap').val() == 'true') {
        $("#singelradiobtn").click();
    }
    if ($('#hf_VrOneRout').val() == 'true' && soappVal == 'true') {
        $("#singelradiobtn").click();
    }
    IfOneRecordHaulierApplVehicleParts();
}
$(document).ready(function () {
    $('body').off('click', '#btn_cancel');
    $('body').on('click', '#btn_cancel', function (e) {
        UsePreviousMovement(this);
    });
    $('body').off('click', '.btnrouteing');
    $('body').on('click', '.btnrouteing', function (e) {
        ImportrouteinnotificationHaulierApplVehicleParts(this);
    });
    $('body').off('click', '.getsoappHaulierApplVehicleParts');
    $('body').on('click', '.getsoappHaulierApplVehicleParts', function (e) {
        GetSOapplicationHaulierApplVehicleParts(this);
    });
    $('body').off('click', '.gettingHaulierApplVehicleParts');
    $('body').on('click', '.gettingHaulierApplVehicleParts', function (e) {
        GettingdetailsHaulierApplVehicleParts(this);
    });
    $('body').off('click', '#displaypopup');
    $('body').on('click', '#displaypopup', function (e) {
        displayPopupList(this);
    });
    $('body').off('click', '.displaysorouteHaulierApplVehicleParts');
    $('body').on('click', '.displaysorouteHaulierApplVehicleParts', function (e) {
        displaySoRouteDescMapforNotifyHaulierApplVehicleParts(this);
    });
    $('body').off('click', '.getvr1routesHaulierApplVehicleParts');
    $('body').on('click', '.getvr1routesHaulierApplVehicleParts', function (e) {
        GetVrOneRoutesHaulierApplVehicleParts(this);
    });
    $('body').off('click', '.getsortrouteHaulierApplVehicleParts');
    $('body').on('click', '.getsortrouteHaulierApplVehicleParts', function (e) {
        GetSORTVehRoutesHaulierApplVehicleParts(this);
    });

    $('body').off('click', '#back-link');
    $('body').on('click', '#back-link', function (e) {
        history.go(-1);
    });
    //----------------$(document).ready(function () { external script load function start here }-----------
    //  RouteDetailsReady();
    //----------------$(document).ready(function () { external script load function end here }-------------
    
});

function GetSOapplicationHaulierApplVehicleParts(e) {
    var arg1 = $(e).attr("arg1");
    var arg2 = $(e).attr("arg2");
    GetSOappHaulierApplVehicleParts(arg1, arg2);
}
function displaySoRouteDescMapforNotifyHaulierApplVehicleParts(e) {
    var arg1 = $(e).attr("arg1");
    var arg2 = $(e).attr("arg2");
    displaySoRouteDescMapforNotification(arg1, arg2);
}
function GetVrOneRoutesHaulierApplVehicleParts(e) {
    var arg1 = $(e).attr("arg1");
    GetVrOneRouteHaulierApplVehicleParts(arg1);
}
function GetSORTVehRoutesHaulierApplVehicleParts(e) {
    var arg1 = $(e).attr("arg1");
    var arg2 = $(e).attr("arg2");
    GetSORTVehRouteHaulierApplVehicleParts(arg1, arg2);
}
function GettingdetailsHaulierApplVehicleParts(e) {
    var arg1 = $(e).attr("arg1");
    var arg2 = $(e).attr("arg2");
    GetHaulierApplVehicleParts(arg1, arg2);
}
function ImportrouteinnotificationHaulierApplVehicleParts(e) {
    var arg1 = $(e).attr("arg1");
    var arg2 = $(e).attr("arg2");
    ImportrouteinnotifHaulierApplVehicleParts(arg1, arg2);
}
function IfOneRecordHaulierApplVehicleParts() {

    if (CountVal == 1) {
        var Vpartid = 0;
        var VRouteType = "";
        Vpartid = $('input.gettingHaulierApplVehicleParts[name="chk"]:checked').val();
        V_RouteType = $('input.gettingHaulierApplVehicleParts[name="chk"]:checked').data('type');

        if (CountVal == 1) {
            if ($('#hf_soapp').val() == 'True') {
                var rt_type = RouteType;
                GetSOappHaulierApplVehicleParts(Vpartid, rt_type);
            }
            if ($('#hf_approute').val() == 'True') {
                displaySoRouteDescMapforNotification(Vpartid, V_RouteType);
            }
            if ($('#hf_VrOneRoute').val() != 'Null' && VrOneRouteVal == 'True') {
                GetVrOneRouteHaulierApplVehicleParts(Vpartid);
            }
            else {
                GetSORTVehRouteHaulierApplVehicleParts(Vpartid, V_RouteType);
            }
        }
    }
    else {
        var Vpartid = 0;
        var VRouteType = "";
        Vpartid = $('input.gettingHaulierApplVehicleParts[name="chk"]:checked').val();
        V_RouteType = $('input.gettingHaulierApplVehicleParts[name="chk"]:checked').data('type');
        if ($('#hf_soapp').val() == 'True' || $('#hf_soapp').val() == 'true') {
            var rt_type = RouteType;
            GetSOappHaulierApplVehicleParts(Vpartid, rt_type);
        }
        if ($('#hf_approute').val() == 'True' || $('#hf_approute').val() == 'true') {

            displaySoRouteDescMapforNotification(Vpartid, V_RouteType);

        }
        if ($('#hf_VrOneRout').val() != 'Null' && (VrOneRouteVal == 'True' || VrOneRouteVal == 'true')) {
            GetVrOneRouteHaulierApplVehicleParts(Vpartid);
        }
        else {
            GetSORTVehRouteHaulierApplVehicleParts(Vpartid, V_RouteType);
        }
    }


}

function GetSOappHaulierApplVehicleParts(partid, rt_type) {

    var FlagAppVeh = 0;
    var isVR1 = $('#vr1appln').val();
    if (isVR1 == 'True') {
        FlagAppVeh = 1;//for VR1 application
    }
    else {
        FlagAppVeh = 2; //for SO application
    }
    //to check this is call from SO application to fetch vehicles list
    $.ajax
        ({
            url: '../Application/appvehconfigImport',
            type: 'POST',
            datatype: 'json',
            async: false,
            data: { partid: partid, FlagSOAppVeh: FlagAppVeh, RouteType: rt_type },
            beforeSend: function () {
                startAnimation();
            },
            success: function (page) {

                $('#routelist').html('');
                $('#routelist').show();

                $('#routelist').html($(page).find('#divappvehcongi1').html(), function () {
                });
            },
            error: function () {
                location.reload();
            },
            complete: function () {
                stopAnimation();
            }
        });
}
function GetHaulierApplVehicleParts(PartId, routeType) {

    if ($('#hf_NotifShowVeh').val() == 'True' || $('#hf_NotifShowVeh').val() == 'true') {
        $('#Vehicle_sort').html("");
        startAnimation();
        $.ajax({
            url: '../Application/ApplicationVehicle',
            type: 'POST',
            datatype: 'json',
            async: true,
            data: { PartId: PartId, IsVRVeh: true },
            success: function (page) {
                $('#ApplicationVehicle').show();
                $('#ApplicationVehicle').html(page);
            },
            error: function () {
                //location.reload();
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    else {
        var revisionId = $('#RevisionID').val();
        var version_id = $('#VersionID').val();
        $.ajax({
            url: '../Application/ApplicationPartDetails',
            type: 'POST',
            datatype: 'json',
            async: true,
            data: { partid: PartId, routeType: routeType, RevisionID: revisionId, Version_ID: version_id },
            success: function (page) {
                startAnimation();
                $('#ApplicationVehicle').show();
                $('#ApplicationVehicle').html(page);
            },
            error: function () {
                //location.reload();
            },
            complete: function () {
                stopAnimation();
            }
        });
        //  GetRoutePartIDForHuilierTab(PartId);
        NewGetHaulierApplVehicleParts(PartId, routeType);
        $('#divMap1').hide();
        $('#ShowDetail').hide();
        $('#PartDetail').hide();
        $('#Suggestedroute').hide();


    }
}
function GetRouteForHuilierTab(PartId) {
    $('#Suggestedroute').show();
    var revisionId = $('#RevisionID').val();
    var version_id = $('#VersionID').val();
    $('#tab_4').show();

    $('#PartDetail').load('../Application/ApplicationPartDetails?partid=' + PartId + '&RevisionID=' + revisionId + '&Version_ID=' + version_id);
    GetRoutePartIDForHuilierTab(PartId);

}

function GetRoutePartIDForHuilierTab(PartId) {

    $("#overlay").show();
    $("#dialogue").hide();
    $('.loading').show();
    $.ajax({
        type: 'POST',
        async: true,
        dataType: 'json',
        url: '../Routes/GetAgreedRoute',
        data: { routePartId: PartId },
        beforeSend: function (xhr) {
            startAnimation();
        },
        success: function (result) {
            if (result != null) {


                var count = result.routePathList[0].routePointList.length, strTr, flag = 0, Index = 0;

                for (var i = 0; i < count; i++) {
                    if (result.routePathList[0].routePointList[i].pointGeom != null || result.routePathList[0].routePointList[i].linkId != null)
                        flag = 1;
                }
                if (flag == 1 || RouteFlagVal == 1 || RouteFlagVal == 3) {
                    $('#Tabvia').html('');
                    $("#ShowDetail").show();
                    $("#div_Route").hide();

                    $("#RouteName").html(result.routePartDetails.routeName);
                    if (result.routePartDetails.routeDescr != null && result.routePartDetails.routeDescr != "")
                        $("#RouteDesc").html(result.routePartDetails.routeDescr);
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
                    $("#map").html('');
                    $("#tab_3 #map").load('../Routes/A2BPlanning?routeID=0', function () {
                        // $("#map").addClass("context-wrap olMap");
                        var listCountSeg = 0;
                        //if (result.routePathList[0].routeSegmentList != null)
                        //    listCountSeg = 1;
                        for (var i = 0; i < result.routePathList.Count; i++) {
                            listCountSeg = result.routePathList[i].routeSegmentList.Count;
                            if (listCountSeg > 0)
                                break;
                        }

                        if (listCountSeg == 0) {
                            if (result.routePathList[0].routeSegmentList != null)
                                listCountSeg = 1;
                        }

                        if (listCountSeg == 0)// if (RouteFlagVal == 2)
                        {
                            //loadmap(2);
                            //showSketchedRoute(result);
                            loadmap('DISPLAYONLY', result.result);
                        }
                        else {
                            loadmap('DISPLAYONLY', result);
                        }

                        A2BPlanningInit();
                    })
                }
                else {

                    $("#RouteMap").html('');
                    $("#ShowDetail").show();
                    $("#div_Route").hide();
                    $("#RouteName").html(result.routePartDetails.routeName);
                    $('#Tabvia').html('');
                    if (result.routePartDetails.routeDescr != null && result.routePartDetails.routeDescr != "")
                        $("#RouteDesc").html(result.routePartDetails.routeDescr);
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
                    $("#RouteMap").html('No visual representation of route available.');
                }
                if ($('#Starting').html() == '') {
                    $('#trRoute').hide();
                    $('#trStarting').hide();
                    $('#trVia').hide();
                    $('#trEnding').hide();
                }
                else {
                    $('#trRoute').show();
                    $('#trStarting').show();
                    $('#trVia').show();
                    $('#trEnding').show();
                }
                if ($("#RouteDesc").html() != "") {
                    $('#trHeaderDescription').show();
                    $('#trdesc').show();
                }
                else {
                    $('#trHeaderDescription').hide();
                    $('#trdesc').hide();
                }
                stopAnimation();

            }
            stopAnimation();
        }
    });
}

function GetRoutePartID(PartId, RouteType) {

    $("#overlay").show();
    $("#dialogue").hide();
    $('.loading').show();
    $.ajax({
        type: 'POST',
        dataType: 'json',
        // url: '../Routes/GetAgreedRoute',
        url: '../Routes/GetSoRouteDescMap',
        data: { plannedRouteId: PartId, routeType: RouteType },
        beforeSend: function (xhr) {
            startAnimation();
        },
        success: function (result) {
            $("#tab_3 #map").load('../Routes/A2BPlanning?routeID=0', function () {
                $("#map").addClass("context-wrap olMap");
                if (RouteType == "planned")
                    loadmap('A2BPLANNING', result.result);
                else {
                    loadmap('A2BPLANNING', result.result);
                    //  showOutlineRoute(result.result);
                }
                A2BPlanningInit();
            })
            stopAnimation();
        }
    });
}
function GetVrOneRouteHaulierApplVehicleParts(partId) {

    $('#part_Id').val(partId);
    $('#form_vrone_route_parts').submit();
}
function LoadOtherMovements(result) {

    $('#routelist').html('');
    $('#routelist').show();
    $('#AffectedStructures').find('#btn_cancel').remove();
    $('#routelist').html(result);
}

//SORT Mov Version route & vehicle shown here

function GetSORTVehRouteHaulierApplVehicleParts(PartId, routeType) {
    var version_id = $('#VersionID').val();
    $.ajax({
        url: '../Application/ApplicationPartDetails',
        type: 'POST',
        datatype: 'json',
        async: true,
        data: { partid: PartId, routeType: routeType, Version_ID: version_id },
        success: function (page) {
            startAnimation();
            $('#ApplicationVehicle').show();
            $('#ApplicationVehicle').html(page);
        },
        error: function () {

        },

    });

    $('#divMap1').hide();
    $('#ShowDetail').hide();
    $('#PartDetail').hide();
    $('#Suggestedroute').hide();
    $.ajax({
        url: '../Application/ApplicationVehicle',
        type: 'POST',
        datatype: 'json',
        async: false,
        data: { PartId: PartId, SORTMOV: true },
        success: function (page) {
            $('#Vehicle_sort').show();
            $('#Vehicle_sort').html(page);

        },
        error: function () {
            //location.reload();
        },
        complete: function () {
            stopAnimation();
        }

    });

    $('#TRbtnBackToRouteList1').hide();
}
function NewGetHaulierApplVehicleParts(RouteId, RouteType) {

    //$("#ShowDetail1").show();
    $('#Vehicle').show();
    //$("#map").html('');
    var str = '';
    if (RouteType != "")
        str = RouteType;
    var HfRouteType = $('#HfRouteType1').val();
    $.ajax({
        type: 'POST',
        dataType: 'json',
        async: false,
        url: '../Routes/GetPlannedRoute',
        data: { RouteID: RouteId, routeType: str, IsPlanMovement: $("#hf_IsPlanMovmentGlobal").length > 0, IsCandidateView: IsCandidateRouteView() },
        beforeSend: function (xhr) {
            startAnimation();
        },
        success: function (result) {
            if (result.result != null) {
                var PathListCount = 0;
                PathListCount = result.result.routePathList.length - 1;
                for (var x = 0; x < result.result.routePathList.length; x++) {
                    result.result.routePathList[x].routePointList.sort((a, b) => a.routePointId - b.routePointId);
                }
                var count = result.result.routePathList[0].routePointList.length, strTr, flag = 0, Index = 0;
                for (var i = 0; i < count; i++) {
                    if (result.result.routePathList[0].routePointList[i].pointGeom != null)//|| result.result.routePathList[0].routePointList[i].linkId != null
                        flag = 1;
                }
                $("#RouteDesc1").html('');
                $("#RouteName1").html('');
                //$('#Starting1').html('');
                //$('#Ending1').html('');
                //$('#Tabvia1').html('');
                if (flag == 1 || RouteFlagVal == 1 || RouteFlagVal == 3) {
                    $('#Tabvia1').html('');
                    //$("#ShowDetail1").show();
                    $('#Vehicle').show();
                    $("#div_Route1").hide();

                    $("#RouteName1").html(result.result.routePartDetails.routeName);
                    if (result.result.routePartDetails.routeDescr != null && result.result.routePartDetails.routeDescr != "")
                        $("#RouteDesc1").html(result.result.routePartDetails.routeDescr);
                }
                else {

                    $("#map").html('');
                    //$("#ShowDetail1").show();
                    $('#Vehicle').show();

                    $("#RouteName1").html(result.result.routePartDetails.routeName);
                    $('#Tabvia1').html('');
                    if (result.result.routePartDetails.routeDescr != null && result.result.routePartDetails.routeDescr != "")
                        $("#RouteDesc1").html(result.result.routePartDetails.routeDescr);
                    for (var i = 0; i < count; i++) {

                        if (result.result.routePathList[0].routePointList[i].pointType == 0)
                            $('#Starting1').html(result.result.routePathList[0].routePointList[0].pointDescr);
                        else if (result.result.routePathList[0].routePointList[i].pointType == 1)
                            $('#Ending1').html(result.result.routePathList[0].routePointList[1].pointDescr);

                        else if (result.result.routePathList[0].routePointList[i].pointType >= 2) {
                            Index = Index + 1;
                            strTr = "<tr ><td style='width:40px'>" + Index + "</td><td>" + result.result.routePathList[0].routePointList[i].pointDescr + "</td></tr>"
                            $('#Tabvia1').append(strTr);
                        }
                    }
                    $("#map").html('No visual representation of route available.');
                }
            }
            else
                $("#RouteName1").html('Route not available.');
            if ($('#Starting1').html() == '') {
                $('#trRoute1').hide();
                $('#trStarting1').hide();
                $('#trVia1').hide();
                $('#trEnding1').hide();
            }
            else {
                $('#trRoute1').show();
                $('#trStarting1').show();
                $('#trVia1').show();
                $('#trEnding1').show();
            }
            if ($("#RouteDesc1").html() != "") {
                $('#trHeaderDescription1').show();
                $('#trdesc1').show();
            }
            else {
                $('#trHeaderDescription1').hide();
                $('#trdesc1').hide();
            }


            stopAnimation();
        }
    });

    $("#tdBtn1").html('');

    $("#tdBtn1").html('<button class="btn_reg_back next btngrad btnrds btnbdr haul-app-vehicle-part-disp-popup" id="displaypopup"  aria-hidden="true" data-icon="" type="button" >Back</button>  <button data-routeid="' + RouteId + '" arg2="' + $("#RouteName").html() + '" class="tdbutton next btngrad btnrds btnbdr btnroutein haul-app-vehicle-part-import-route-notif"  aria-hidden="true" data-icon="&#xe0f4;" >Import</button>');


}
$('body').on('click', '.haul-app-vehicle-part-disp-popup', function (e) {
    displayPopupList(this);
});

$('body').on('click', '.haul-app-vehicle-part-import-route-notif', function (e) {
    var routeId = $(this).data("routeid");
    ImportrouteinnotifHaulierApplVehicleParts(routeId, $("#RouteName").html());
});
