    $('body').on('click', '#btn-importroute', function (e) {
        e.preventDefault();
        var RouteID = $(this).data('RouteID');
        var RouteName = $(this).data('RouteName');
        ImportRoute(RouteID, RouteName);
    });
    function ImportRoute(RouteId, RNam) {
        var RevId = $('#revisionId').val();
        $.ajax({
            url: "../SORTApplication/ImportRouret",
            type: 'post',
            async: false,
            data: { routepartId: RouteId, AppRevId: RevId, routeType: "planned" },
            success: function (result) {
                /*Region commented for ESDAL4 need to be integrate later stage   uncomment below 2 lines for reverting the mapupgrade functionality */
                var is_replan = 0;
                //is_replan = CheckIsBroken(result, 0, 0, 0, 0, 0);
                // if (is_replan[0].isBroken > 0) { //check in the existing route is broken
                if (1 == 2) {
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
                        showWarningPopDialogBig(msg, 'Ok', '', 'ShowRouteList', '', 1, 'info');
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
                            showWarningPopDialogBig(msg, 'Ok', '', 'ShowRouteList', '', 1, 'info');
                        }
                        else {
                            $('.popup111 .message111').css({ 'height': '180px' });
                            $('.popup111').css({ 'height': '215px' });
                            if ($("#IsPreRouteIDouteIDouteIDeIDousMvmnt").val() == 1) {
                                msg = 'Please be aware that due to the map upgrade the route(s) in this previous movement version contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or RouteTypeouteTypeypereRouteTypete a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.';
                            }
                            else {
                                msg = 'Please be aware that due to the map upgrade the route(s) in this current movement version contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.';
                            }
                            showWarningPopDialogBig(msg, 'Ok', '', 'ShowRouteList', '', 1, 'info');
                        }
                    }
                }
                else {
                    ShowSuccessModalPopup("'" + RNam + "' route imported successfully.", "ShowRouteList");
                }

            },
            error: function () {

            }
        });

    }
    function ShowRouteList() {
        if ($('#SortStatus').val() == "CreateSO")
            SelectedRouteFromLibraryForVR1();
        else {           
            BindRouteParts();
        }
    }
    //Candidate route binding.
    function BindRouteParts() {
        CloseSuccessModalPopup();
        $('#StruRelatedMov').hide();
        $('#RoutePart').show();
        $('#divCurrentMovement').hide();
        $("#back_currentmovmnt").hide();
        $('#SelectCurrentMovements1').hide();
        $('#SelectCurrentMovements2').hide();
        $("#divCandiRouteDeatils").html('');
        $('#route').hide(); //hide map
        $('#back_btn_Rt').hide();
        startAnimation();
        $("li[id='4']").show();
        $("li[id='4']").addClass('t');
        var rtrevisionId = $('#revisionId').val();
        var iscandlastversion = $('#IsCandVersion').val();
        var plannruserid = $('#PlannrUserId').val();
        var appstatuscode = $('#AppStatusCode').val();
        var movversionno = $('#versionno').val();
        var movdistributed = $('#IsMovDistributed').val();
        var sonumber = $('#SONumber').val();
        var hauliermnemonic = $('#hauliermnemonic').val();
        var esdalref = $('#esdalref').val();
        var prjstatus = $('#Proj_Status').val();

        //_checkerid = 15;
        if (rtrevisionId == 0) {
            $('#leftpanel').hide();
        }
        else {

            $(".tab_content1").each(function () {
                $(this).hide();
            });
            $.ajax({
                url: "../SORTApplication/ShowCandidateRoutes",
                type: 'post',
                async: false,
                data: { routerevision_id: rtrevisionId, CheckerId: _checkerid, CheckerStatus: _checkerstatus, IsCandLastVersion: iscandlastversion, planneruserId: plannruserid, appStatusCode: appstatuscode, SONumber: sonumber },
                success: function (data) {
                    $('#tab_4').show();
                    $('#RoutePart').html('');
                    $('#RoutePart').html(data);

                    //$("#leftpanel_quickmenu").html('');
                    $('#leftpanel').html('');
                    $('#leftpanel').hide();
                    $('#sort11').hide();
                    $('#information').hide();
                    $('#btn_cancel').hide();
                    stopAnimation();
                    var prjheader  = $('#hf_CandName' + " - Version " + '@ViewBag.CandVersionNo' + ") - Route').val(); 
                    $('div#pageheader').html('');
                    $('div#pageheader').append('<h3>' + prjheader + '</h3>');

                },
                error: function () {
                }
            });
        }
    }
