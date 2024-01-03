var modelCount = $('#hf_ModelCount').val();
var sessionRFlag = $('#hf_sessionRouteFlag').val();
function HaulierAppRoutePartInit() {
    modelCount = $('#hf_ModelCount').val();
    sessionRFlag = $('#hf_sessionRouteFlag').val();
    $('#Suggestedroute').hide();
    $('#Suggestedroute1').hide();
    IfOneRecord();
}
$(document).ready(function () {
    $('body').on('click', '.btnrouteing', function (e) {
        e.preventDefault();
        Importrouteinnotification(this);
    });
    $('body').on('click', '#displaypopup', function (e) {
        e.preventDefault();
        displayPopupList(this);
    });
    $('body').on('click', '#btn_cancel', function (e) {
        e.preventDefault();
        UsePreviousMovement(this);
    });
    $('body').on('click', '.getting', function (e) {
        e.preventDefault();
        Gettingdetails(this);
    });
    $('body').on('click', '.getsoapp', function (e) {
        e.preventDefault();
        GetSOapplication(this);
    });
    $('body').on('click', '#btnmovsaveannotation', function (e) {
        e.preventDefault();
        btn_save_annotation(this);
    });
    $('body').on('click', '.getsortroute', function (e) {
        GetSORTVehRoutes(this);
    });
    $('body').on('change', '.getvr1routes', function (e) {
        e.preventDefault();
        GetVrOneRoutes(this);
    });
    $('body').on('click', '.displaysoroute', function (e) {
        e.preventDefault();
        displaySoRouteDescMapforNotify(this);
    });
    $('body').on('click', '#back-link', function (e) {
        history.go(-1);
    });
});
function Gettingdetails(e) {
    var arg1 = $(e).attr("arg1");
    var arg2 = $(e).attr("arg2");
    Get(arg1, arg2);
}
function GetSOapplication(e) {
    var arg1 = $(e).attr("arg1");
    var arg2 = $(e).attr("arg2");
    GetSOapp(arg1, arg2);
}
function Importrouteinnotification(e) {
    var arg1 = $(e).attr("arg1");
    var arg2 = $(e).attr("arg2");
    Importrouteinnotif(arg1, arg2);
}
function GetVrOneRoutes(e) {
    var arg1 = $(e).attr("arg1");
    GetVrOneRoute(arg1);
}
function GetSORTVehRoutes(e) {
    var arg1 = $(e).attr("arg1");
    var arg2 = $(e).attr("arg2");
    
    GetSORTVehRoute(arg1, arg2);
}
function displaySoRouteDescMapforNotify(e) {
    var arg1 = $(e).attr("arg1");
    var arg2 = $(e).attr("arg2");
    displaySoRouteDescMapforNotification(arg1, arg2);
}
function IfOneRecord() {
    if (modelCount == 1) {
        var Vpartid = 0;
        var VRouteType = "";
        Vpartid = $('.getsortroute').val();
        V_RouteType = $('.getsortroute').data('type');

        if (modelCount == 1) {

            if ($('#hf_soapp').val() == 'True') {
                var rt_type = $('#hf_routeType').val();
                GetSOapp(Vpartid, rt_type);
            }
            if ($('#hf_approute').val() == 'True') {
                displaySoRouteDescMapforNotification(Vpartid, V_RouteType);
            }
            if ($('#hf_VrOneRout').val() == 'Null' && $('#hf_VrOneRout').val() == 'True') {
                GetVrOneRoute(Vpartid);
            }
            else {
                if ($('#hf_SORTVehRoute').val() == 'True') {
                    GetSORTVehRoute(Vpartid, V_RouteType);
                }
                else {
                    Get(Vpartid, V_RouteType);
                }
            }
        }
    }
}
function GetSOapp(partid, rt_type) {

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
function Get(PartId, routeType) {
    NewGet(PartId, routeType);
    $.ajax({
        url: '../Application/ApplicationVehicle',
        type: 'POST',
        datatype: 'json',
        async: true,
        data: { PartId: PartId, IsVRVeh: true },
        success: function (page) {
            startAnimation();
            $('#ApplicationParts').show();
            $('#ApplicationParts').html(page);
        },
        error: function () {
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
    $('#divMap1').hide();
    $('#ShowDetail').hide();
    $('#PartDetail').hide();
    $('#Suggestedroute').hide();
    
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
                if (flag == 1 || sessionRFlag == 1 || sessionRFlag == 3) {
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
                    $("#tab_3 #map").load('../Routes/A2BPlanning', { routeID : 0 }, function () {
                        var listCountSeg = 0;
                        for (var i = 0; i < result.routePathList.Count; i++) {
                            listCountSeg = result.routePathList[i].routeSegmentList.Count;
                            if (listCountSeg > 0)
                                break;
                        }
                        if (listCountSeg == 0) {
                            if (result.routePathList[0].routeSegmentList != null)
                                listCountSeg = 1;
                        }
                        if (listCountSeg == 0) {
                            loadmap('DISPLAYONLY', result.result);
                        }
                        else {
                            loadmap('DISPLAYONLY', result);
                        }
                    });
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
function GetRoute(PartId, RouteType) {

    $('#Suggestedroute1').show();
    var revisionId = $('#RevisionID').val();
    $('#tab_4').show();
    $('#PartDetail1').load('../Application/ApplicationPartDetails?partid=' + PartId + '&routeType=' + RouteType + '&RevisionID=' + revisionId + '&HideVehDet=' + true);
    GetRoutePartID(PartId, RouteType);
}
function GetRoutePartID(PartId, RouteType) {

    $("#overlay").show();
    $("#dialogue").hide();
    $('.loading').show();
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '../Routes/GetSoRouteDescMap',
        data: { plannedRouteId: PartId, routeType: RouteType },
        beforeSend: function (xhr) {
            startAnimation();
        },
        success: function (result) {
            $("#tab_3 #map").load('../Routes/A2BPlanning', { routeID : 0 }, function () {
                $("#map").addClass("context-wrap olMap");
                if (RouteType == "planned")
                    loadmap('A2BPLANNING', result.result);
                else {
                    loadmap('A2BPLANNING', result.result);
                }
            })
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function GetVrOneRoute(partId) {

    $('#part_Id').val(partId);
    $('#form_vrone_route_parts').submit();
}
function LoadOtherMovements(result) {

    $('#routelist').html('');
    $('#routelist').show();
    $('#AffectedStructures').find('#btn_cancel').remove();
    $('#routelist').html(result);
}
function GetSORTVehRoute(PartId, routeType) {
    NewGet(PartId, routeType);
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
        },
        complete: function () {
        }
    });

    $('#TRbtnBackToRouteList1').hide();
}
function btn_save_annotation() {
    var analysis_id = $('#analysis_id').val();
    var revisionId = $('#revisionId').val();
    var versionId = $('#versionId').val();
    startAnimation();
    $('#overlay_load').addClass("ZINdexMax");
    var rdetails = getRouteDetails();
    var annotationsexist = false;
    for (var i = 0; i < rdetails.routePathList.length; i++) {
        for (var j = 0; j < rdetails.routePathList[i].routeSegmentList.length; j++) {
            if (rdetails.routePathList[i].routeSegmentList[j].routeAnnotationsList.length > 0) {
                annotationsexist = true;
                break;
            }
        }
    }
    $.ajax({
        url: '/SORTApplication/SaveMovAnnotation',
        type: 'POST',
        processData: false,
        data: JSON.stringify({ plannedRoutePath1: rdetails, revisionId: revisionId, analysisId: analysis_id, versionId: versionId }),
        beforeSend: function () {
            startAnimation();
        },
        success: function (val) {
            $('#btnmovsaveannotation').hide();
            if (val == true) {
                ShowInfoPopup("Annotation(s) saved successfully");
            }
            else {
                if (annotationsexist == false) {
                    ShowInfoPopup("Annotation(s) saved successfully");
                }
                else {
                    ShowErrorPopup("Annotation(s) are not saved");
                }
            }
        },
        error: function (err) {
            ShowErrorPopup("Annotation(s) are not saved");
        },
        complete: function () {
            stopAnimation();
            $('#overlay_load').removeClass("ZINdexMax");
        }
    });
}
function NewGet(RouteId, RouteType) {
    $("#ShowDetail1").show();
    $('#Vehicle').show();
    $("#map").html('');
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
                for (var x = 0; x < result.result.routePathList.length; x++) {
                    result.result.routePathList[x].routePointList.sort((a, b) => a.routePointId - b.routePointId);
                }
                var PathListCount = 0;
                PathListCount = result.result.routePathList.length - 1;

                var count = result.result.routePathList[0].routePointList.length, strTr, flag = 0, Index = 0;
                for (var i = 0; i < count; i++) {
                    if (result.result.routePathList[0].routePointList[i].pointGeom != null)//|| result.result.routePathList[0].routePointList[i].linkId != null
                        flag = 1;
                }
                $("#RouteDesc1").html('');
                $("#RouteName1").html('');
                $('#Starting1').html('');
                $('#Ending1').html('');
                $('#Tabvia1').html('');
                var radioButton = '#radio_' + RouteId;
                $(radioButton).attr("checked", true);
                if (flag == 1 || sessionRFlag == 1 || sessionRFlag == 3) {
                    $('#Tabvia1').html('');
                    $("#ShowDetail1").show();
                    $("#ShowDetail1").show();
                    $('#Vehicle').show();
                    $("#div_Route1").hide();

                    $("#RouteName1").html(result.result.routePartDetails.routeName);
                    if (result.result.routePartDetails.routeDescr != null && result.result.routePartDetails.routeDescr != "")
                        $("#RouteDesc1").html(result.result.routePartDetails.routeDescr);

                    for (var i = 0; i < count; i++) {
                        if (result.result.routePathList[0].routePointList[i].pointType == 0)
                            $('#Starting1').html(result.result.routePathList[0].routePointList[i].pointDescr);
                        else if (result.result.routePathList[0].routePointList[i].pointType == 1)
                            $('#Ending1').html(result.result.routePathList[0].routePointList[i].pointDescr);

                        else if (result.result.routePathList[0].routePointList[i].pointType >= 2) {
                            Index = Index + 1;
                            strTr = "<tr ><td style='width:40px'>" + Index + "</td><td>" + result.result.routePathList[0].routePointList[i].pointDescr + "</td></tr>"
                            $('#Tabvia1').append(strTr);
                        }
                    }
                    $("#map").html('');
                    $("#tab_3 #map").load('../Routes/A2BPlanning', { routeID: 0 }, function () {
                        var listCountSeg = 0;
                        for (var i = 0; i < result.result.routePathList.Count; i++) {
                            listCountSeg = result.result.routePathList[i].routeSegmentList.Count;
                            if (listCountSeg > 0)
                                break;
                        }
                        if (listCountSeg == 0) {
                            if (result.result.routePathList[0].routeSegmentList != null)
                                listCountSeg = 1;
                        }
                        if (RouteType == 'outline') {
                            loadmap('DISPLAYONLY');
                            showSketchedRoute(result.result);
                        }
                        else {
                            var chk_status = $('#CheckerStatus').val() ? $('#CheckerStatus').val() : 0;
                            var sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
                            var checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
                            var MovLatestVer = $('#MovLatestVer').val();
                            var MovVersion = $('#versionno').val();
                            var proj_status = $('#AppStatusCode').val();
                            if ($('#hf_SORTVehRout').val() == 'False') {
                                loadmap('DISPLAYONLY', result.result, null, false);
                            }
                            else if (status != "MoveVer") {
                                loadmap('DISPLAYONLY', result.result);
                            }
                            else if (status == "MoveVer" && MovLatestVer == MovVersion) {
                                if ((chk_status == 301002 || chk_status == 301008 || chk_status == 301005) && (sort_user_id != checker_id))
                                    loadmap('DISPLAYONLY', result.result);
                                else if (chk_status == 301006)
                                    loadmap('DISPLAYONLY', result.result);
                                else if (proj_status == 307012 || proj_status == 307011)
                                    loadmap('DISPLAYONLY', result.result);
                                else
                                    loadmap('DISPLAYONLY_EDITANNOTATION', result.result);
                            }
                            else if (status == "MoveVer") {
                                loadmap('DISPLAYONLY', result.result);
                            }
                            else {
                                loadmap('DISPLAYONLY_EDITANNOTATION', result.result);
                            }
                        }
                    })
                }
                else {

                    $("#map").html('');
                    $("#ShowDetail1").show();
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

    $("#tdBtn1").html('<button class="btn_reg_back next btngrad btnrds btnbdr haul-app-route-part-disp-popup" id=displaypopup  aria-hidden="true" data-icon="" type="button">Back</button>  <button id="haulImportNotif "data-routeid="' + RouteId + '" arg2="' + $("#RouteName").html() + '" class="tdbutton next btngrad btnrds btnbdr btnroutein haul-app-route-part-import-route-notif"  aria-hidden="true" data-icon="&#xe0f4;" >Import</button>');
    if ($('#displaypopup').length > 0) {
        $('body').on('click', '.haul-app-route-part-disp-popup', function (e) {
            displayPopupList(this);
        });
    }
    if ($('#haulImportNotif').length > 0) {
        $('body').on('click', '.haul-app-route-part-import-route-notif"', function (e) {
            var routeId = $(this).data("routeid");
            Importrouteinnotif(routeId, $("#RouteName").html());
        });
    }
}