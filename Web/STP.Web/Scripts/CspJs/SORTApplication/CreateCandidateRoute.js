    $(document).ready(function () {
        $('.dyntitleConfig').html('Create Candidate Route');
        Resize_PopUp(440);
        $('#txtcandidateroutename').focus();
        $('#btncandidatesave').click(function () {
            $('#generalPopup').modal('hide');
            var candidatroutename = $('#txtcandidateroutename').val();
            var routeType = $('#ddlroutes option:selected').val();
if($('#hf_LatestCandRevId').val() ==  0) {
                routeType = "NewRoute";
            }
            if (candidatroutename != "" && routeType != "0") {
                startAnimation();               
                var vr1Application = false;
                if ($('#VR1Applciation').val() === "True") {
                    vr1Application = true;
                }

                var dataModelPassed  = $('#hf_LatestCandRevId', AppRevisionId: ApprevId, EsdalReferenceWorkflow: hauliermnemonic + '_').val(); 

                var result = SORTSORouting(2, dataModelPassed);
                if (result == null || result != "NOROUTE") {
                    $.ajax({
                        url: result.route, 
                        type: 'POST',
                        async: true,
                        data: {
                            saveCandidateRouteCntrlModel: result.dataJson
                        },
                        beforeSend: function () {
                            startAnimation();
                        },
                        success: function (data) {
                            stopAnimation();
                            if (data != null) {
                                var result = JSON.parse(data);
                                $('#btncandidatesave').hide();
                                $('#candAnalysisId').val(result.analysisid);
                                $('#LastCandRouteId').val(result.routeid);
                                $('#LastCandRevisionId').val(result.candRevId);
                                var candRevisionId = $('#LastCandRevisionId').val();
                                var isBrokenData;// result return from isBroken check function
                                var autoReplanFail = 0;
                                var autoReplanSuccess = 0;
                                var brokenRouteCount = 0;
                                var spManourCnt = 0;
                                var Msg = "";
                                $('#RoutePart').hide();
                                if (brokenRouteCount != 0) {
                                    if (spManourCnt > 0) {//single route contains special manour
                                        $('.popup111 .message111').css({ 'height': '183px' });
                                        $('.popup111').css({ 'height': '221px' });
                                        Msg = "Please be aware that due to the map upgrade route(s) in the application version (" + revisionno + ") contain previous map data and will need to be re-planned. Please re-plan or ask the haulier to provide a new route before proceeding.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.";
                                    }
                                    else if (autoReplanFail > 0) {
                                        if (brokenRouteCount == 1) { //condition for single route
                                            $('.popup111 .message111').css({ 'height': '183px' });
                                            $('.popup111').css({ 'height': '221px' });
                                            Msg = "Please be aware that due to the map upgrade route(s) in the application version (" + revisionno + ") contain previous map data and will need to be re-planned.Please re-plan a new route before proceeding.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.";
                                        }
                                        else if (brokenRouteCount > 1) {//condition for multiple route
                                            $('.popup111 .message111').css({ 'height': '213px' });
                                            $('.popup111').css({ 'height': '251px' });
                                            Msg = "Please be aware that due to the map upgrade route(s) in the application version (" + revisionno + ") contain previous map data.  One or more route(s) have not been replanned automatically which may be due to legal restrictions or the presence of special manoeuvres or the presence of alternative paths on the route(s). Please re-plan your current route, import a new route or create a new route – You can re-enter start and end points or create a new route on the map.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.";
                                        }
                                    }
                                    else if (autoReplanSuccess > 0) { // if all routes is replanned
                                        Msg = "Please be aware that due to the map upgrade route(s) in the application version (" + revisionno + ") contain previous map data. However, the route(s) have been automatically re-planned based on new map data. Please check the route before proceeding.";
                                    }
                                    showWarningPopDialogBig(Msg, 'OK', '', 'WarningBrokenPopUp', '', 1, 'warning');
                                }
                                else {
                                    if (result.analysisid != 0 && result.routeid != 0)
                                        ShowInfoPopup('Candidate route saved successfully', 'Redirect_ProjectOverview');
                                    else
                                        showWarningPopDialog('The candidate route is not saved successfully.', '', 'OK', '', 'WarningCancelBtn', 1, 'error');
                                }
                            }
                            else {
                                showWarningPopDialog('The candidate route is not saved successfully.', '', 'OK', '', 'WarningCancelBtn', 1, 'error');
                            }
                        },
                        error: function () {
                            stopAnimation();
                            showWarningPopDialog('The candidate route is not saved successfully.', '', 'OK', '', 'WarningCancelBtn', 1, 'error');
                        }
                    });
                }
            }
            else {
                if (candidatroutename == "")
                    $('#trvalidcandname').css("display", "block");
                if (routeType == "0")
                    $('#trvalidcandrt').css("display", "block");
            }
        });
        $('#txtcandidateroutename').click(function () {
            $('#trvalidcandname').css("display", "none");
        });
        $('#ddlroutes').change(function () {
            $('#trvalidcandrt').css("display", "none");
        });

        $("#spanCloseGeneralPopup").on('click', CloseGeneralPopup);
        $("#btnCloseGeneralPopup").on('click', CloseGeneralPopup);
        
    });
    function WarningBrokenPopUp() {
        WarningCancelBtn();
        stopAnimation();
        var analysisid = $('#candAnalysisId').val();
        var routeid = $('#LastCandRouteId').val();
        if (analysisid != 0 && routeid != 0)
            showWarningPopDialog('Candidate route saved successfully', 'Ok', '', 'DisplayProjView', '', 1, 'info');
        else
            showWarningPopDialog('The candidate route is not saved successfully.', '', 'OK', '', 'WarningCancelBtn', 1, 'error');
    }
    function ClosePopUp() {
        $("#dialogue").html('');
        $("#dialogue").hide();
        $("#overlay").hide();
        addscroll();
    }
