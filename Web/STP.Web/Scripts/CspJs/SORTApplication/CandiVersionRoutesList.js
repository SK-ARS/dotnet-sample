    var route_id = 0;
    var rt_type = '';
    var rt_Name;
    $(document).ready(function () {

        $("#btnGotoRouteLibrary").on('click', gotoRouteLibrary);
        $("#btnBactoGen").on('click', BactoGen);
        $("#span-close").on('click', ClosePopUp);
        $("#span-help").on('click', help_poup);
        $("#span-help").on('click', help_poup);
        $("#span-help").on('click', help_poup);
        $("#span-help").on('click', help_poup);

        $('body').on('click', '#spanShowRouteDetails', function (e) {
            e.preventDefault();
            var routeId = $(this).data('routeId');
            var routeType = $(this).data('routeType');
            ShowRouteDetails(routeId, routeType);
        });
        $('body').on('click', '#btnImportRouteInApp', function (e) {
            e.preventDefault();
            var routeId = $(this).data('btnImportrouteId');
            var routeType = $(this).data('btnImportrouteType');
            Importrouteinapp(routeId, routeType);
        });
        $('body').on('click', '#btnImportRoute', function (e) {
            e.preventDefault();
            var routeId = $(this).data('btnImportRouterouteId');
            var routeName = $(this).data('btnImportRouterouteName');
            ImportRoute(routeId, routeName);
        });

    });
    function BactoGen() {
        $("#divCandiRouteDeatils").hide();
        $('#generalDetailDiv').show();
        $('#route').hide(); //hide if the route div is open
        $('#back_btn_Rt').hide();

    }
    function ShowRouteList() {
        if ($('#SortStatus').val() == "CreateSO")
            SelectedRouteFromLibraryForVR1();
        else {
            WarningCancelBtn();
            BindRouteParts();
        }
    }
    function Importrouteinapp(routeID, routetype) {
        WarningCancelBtn();
        var routename = $('#btnrouteimport_' + routeID).data('name');
        if (routename == null)
            routename = $("#RouteName1").text();
        var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
        var SOVersionID = $('#RevisionID').val(); //Previous Movement version id
        var PrevMovESDALRefNum = $('#PrevMovESDALRefNum').val();
        var ShowPrevMoveSortRoute = $("#ShowPrevMoveSortRT").val();
        var vr1contrefno = $('#VR1ContentRefNo').val();
        var vr1versionid = $('#VersionId').val();
        $("#divCurrentMovement").hide();
        $.ajax({
            url: '../Application/SaveRouteInAppParts',
            type: 'POST',
            async: false,
            cache: false,
            data: { routepartId: routeID, routeType: routetype, AppRevId: AppRevId, versionid: vr1versionid, contentref: vr1contrefno, SOVersionId: SOVersionID, PrevMovEsdalRefNum: PrevMovESDALRefNum, ShowPrevMoveSortRoute: ShowPrevMoveSortRoute },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                if (result != 0) {
                    var is_replan = 0;
                    is_replan = CheckIsBroken(result, 0, 0, 0, 0, 0);
                    if (is_replan[0].isBroken > 0) { //check in the existing route is broken
                        var msg = "";
                        var replan = false;

                        if (is_replan[0].isReplan > 1) {
                            $('.popup111 .message111').css({ 'height': '180px' });
                            $('.popup111').css({ 'height': '215px' });
                            if ($("#IsPreviousMvmnt").val() == 1) {
                                msg = 'Please be aware that due to the map upgrade the route(s) in this previous movement contains previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br> </br> When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.';
                            }
                            else {
                                msg = 'Please be aware that due to the map upgrade the route(s) in this current movement contains previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br> </br> When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.';
                            }
                        }
                        else {
                            replan = ReplanBrokenRoutes(is_replan[0].plannedRouteId, 0, false);
                            if (replan) {
                                if ($("#IsPreviousMvmnt").val() == 1) {
                                    msg = 'Please be aware that due to the map upgrade the route(s) in this previous movement version contain previous map data. However, the route has been automatically re-planned based on new map data.  Please check the route before proceeding.';
                                }
                                else {
                                    msg = 'Please be aware that due to the map upgrade the route(s) in this current movement version contain previous map data. However, the route has been automatically re-planned based on new map data.  Please check the route before proceeding.';
                                }
                            }
                            else {
                                $('.popup111 .message111').css({ 'height': '180px' });
                                $('.popup111').css({ 'height': '215px' });
                                if ($("#IsPreviousMvmnt").val() == 1) {
                                    msg = 'Please be aware that due to the map upgrade the route(s) in this previous movement version contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.';
                                }
                                else {
                                    msg = 'Please be aware that due to the map upgrade the route(s) in this current movement version contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.';
                                }
                            }
                        }
                        showWarningPopDialogBig(msg, 'Ok', '', 'ShowRouteList', '', 1, 'info');
                    }
                    else {
                        showWarningPopDialog('"' + routename + '" route imported for this application', 'Ok', '', 'ShowRouteList', '', 1, 'info');
                    }
                    $("#RoutePart").show();
                    $("#ShowDetail").hide();
                    $("#RouteMap").html('');
                    if ($('#UserTitle').html() == "SORT Portal")
                        CloneRoutesSort();
                    else
                        CloneRoutes();
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                location.reload();
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
