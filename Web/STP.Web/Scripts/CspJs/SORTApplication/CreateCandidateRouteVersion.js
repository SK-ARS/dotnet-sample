    var getValue = 0;
    $(document).ready(function () { 
        $("#ddlroutes").change(function () {
            getValue = $(this).val();

            if (getValue != 0) {
                $('#trvalidcandrt').hide();
            }
        });
        $('#txtcandidateroutename').click(function () {
            $('#trvalidcandname').css("display", "none");
        });
        $("#btnValidate").on('click', validate);
        $("#btnRemoveCandidateButton").on('click', removeCandidateButton);
    });
    function ClosePopUp() {
        $("#dialogue").html('');
        $("#dialogue").hide();
        $("#overlay").hide();
        addscroll();
    }

    function validate() {
        var routeType = $('#ddlroutes option:selected').val();
        if (routeType == "0") {
            $("#trvalidcandrt").show();
            return false;
        }
        else {
            Createnewversion();
        }
    }


    function Createnewversion() {
        
        var lastcandrouteid = $('#LastCandRouteId').val();
        var lastcandrtreversionid = $('#LastCandRevisionId').val();
        var projectid = $('#projectid').val();
        var revisionId = $('#revisionId').val();
        var esdalref = hauliermnemonic + "/" + $('#esdalref').val() + "-" + revisionno;
        var esdalReferenceWorkflow = hauliermnemonic + "_" + $('#esdalref').val()
        var isBrokenData;// result return from isBroken check function
        var autoReplanFail = 0;
        var autoReplanSuccess = 0;
        var brokenRouteCount = 0;
        var spManourCnt = 0;
        var Msg = "";
        var replan = false;
        if (getValue == "application") {
            $.post('/SORTApplication/CheckVehicleForApplication?PROJ_ID=' + projectid + '&Rev_Id=' + revisionId, function (data) {
               
                if (data == true) {
                    startAnimation();
                    $.post('/SORTApplication/CreateCandidateVersion?CandRouteId=' + lastcandrouteid + '&CandRevisionId=' + lastcandrtreversionid + '&CloneType=' + getValue + '&AppRevId=' + ApprevId + '&EsdalRef=' + esdalref, function (data) {
                        if (data != null) {
                            var result = JSON.parse(data);                            
                            if (result.newrivisionId != 0 && result.newversionNo != 0 && result.analysisid != 0) {
                                $('#candAnalysisId').val(result.analysisid);
                                $('#LastCandRevisionId').val(result.newrivisionId);
                                $('#LastCandVersion').val(result.newversionNo);
                                var candRevisionId = $('#LastCandRevisionId').val();
                                
                                if (brokenRouteCount != 0) {
                                    if (spManourCnt > 0) {//single route contains special manour
                                        $('.popup111 .message111').css({ 'height': '183px' });
                                        $('.popup111').css({ 'height': '221px' });
                                        Msg = "Please be aware that due to the map upgrade route(s) in the application version (" + revisionno + ") contain previous map data and will need to be re-planned. Please re-plan or ask the haulier to provide a new route before proceeding.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.";
                                    }
                                    else if (autoReplanFail > 0) { //if auto replan fails
                                        if (brokenRouteCount == 1) { //single broken route
                                            $('.popup111 .message111').css({ 'height': '183px' });
                                            $('.popup111').css({ 'height': '221px' });
                                            Msg = "Please be aware that due to the map upgrade route(s) in the application version (" + revisionno + ") contain previous map data and will need to be re-planned.Please re-plan a new route before proceeding.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.";
                                        }
                                        else if (brokenRouteCount > 1) { //multiple broken route
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
                                    stopAnimation();
                                    if (getValue == "application")
                                        ShowInfoPopup('New candidate route version is successfully created from application routes', 'Redirect_ProjectOverview');
                                    else if (getValue == "lastcandidate")
                                        ShowInfoPopup('New candidate route version is successfully created from last candidate route version', 'Redirect_ProjectOverview');
                                }
                            }
                            else {
                                stopAnimation();
                                showWarningPopDialog('The new version is not created.', 'Ok', '', 'WarningCancelBtn', '', 1, 'error');
                            }
                        }
                    });
                }
                else {
                    stopAnimation();
                    showWarningPopDialog('Submit the edited application before proceeding', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                }
            });
        }
        else {
            var dataModelPassed = { CandRouteId: lastcandrouteid, CandRevisionId: lastcandrtreversionid, CloneType: getValue, AppRevId: ApprevId, EsdalRef: esdalref, EsdalReferenceWorkflow: esdalReferenceWorkflow };
            var result = SORTSORouting(6, dataModelPassed);            
            if (result != undefined || result != "NOROUTE") {
                $.ajax({
                    url: result.route, //"",
                    type: 'POST',
                    async: true,
                    data: {
                        createCandidateVersionCntrlModel: result.dataJson
                    },
                    beforeSend: function () {

                    },
                    success: function (data) {
                        if (data != null) {
                            startAnimation();
                            var result = JSON.parse(data);
                            if (result.newrivisionId != 0 && result.newversionNo != 0 && result.analysisid != 0) {
                                $('#candAnalysisId').val(result.analysisid);
                                $('#LastCandRevisionId').val(result.newrivisionId);
                                $('#LastCandVersion').val(result.newversionNo);
                                var lastcandrtreversionid = $('#LastCandRevisionId').val();
                                var LastCandVersion = $('#LastCandVersion').val();

                                if (brokenRouteCount != 0) {
                                    if (spManourCnt > 0) {//single route contains special manour
                                        $('.popup111 .message111').css({ 'height': '183px' });
                                        $('.popup111').css({ 'height': '221px' });
                                        Msg = "Please be aware that due to the map upgrade route(s) in the candidate route version (" + LastCandVersion + ") contain previous map data and will need to be re-planned. Please re-plan or ask the haulier to provide a new route before proceeding.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.";
                                    }
                                    else if (autoReplanFail > 0) { //condition for single route
                                        if (brokenRouteCount == 1) { //single route failed to replan
                                            $('.popup111 .message111').css({ 'height': '183px' });
                                            $('.popup111').css({ 'height': '221px' });
                                            Msg = "Please be aware that due to the map upgrade route(s) in the candidate route version (" + LastCandVersion + ") contain previous map data and will need to be re-planned.Please re-plan or ask the haulier to provide a new route before proceeding.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.";
                                        }
                                        else if (brokenRouteCount > 1) { //single route failed to replan
                                            $('.popup111 .message111').css({ 'height': '213px' });
                                            $('.popup111').css({ 'height': '251px' });
                                            Msg = "Please be aware that due to the map upgrade route(s) in the candidate route version (" + LastCandVersion + ") contain previous map data.  One or more route(s) have not been replanned automatically which may be due to legal restrictions or the presence of special manoeuvres or the presence of alternative paths on the route(s). Please re-plan your current route, import a new route or create a new route – You can re-enter start and end points or create a new route on the map.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.";
                                        }
                                    }
                                    else if (autoReplanSuccess > 0) { // if all routes is replanned
                                        Msg = "Please be aware that due to the map upgrade route(s) in the candidate route version (" + LastCandVersion + ") contain previous map data. However, the route(s) have been automatically re-planned based on new map data. Please check the route before proceeding.";
                                    }
                                    //end code
                                    showWarningPopDialogBig(Msg, 'OK', '', 'WarningBrokenPopUp', '', 1, 'warning');
                                }
                                else {
                                    stopAnimation();
                                    if (getValue == "application")
                                        ShowInfoPopup('New candidate route version is successfully created from application routes', 'Redirect_ProjectOverview');
                                    else if (getValue == "lastcandidate")
                                        ShowInfoPopup('New candidate route version is successfully created from last candidate route version', 'Redirect_ProjectOverview');
                                }
                            }
                            else {
                                stopAnimation();
                                showWarningPopDialog('The new version is not created.', 'Ok', '', 'WarningCancelBtn', '', 1, 'error');
                            }
                        }
                    },
                    error: function () {

                    }
                });
            }           
        }
    }
    function WarningBrokenPopUp() {
        WarningCancelBtn();
        stopAnimation();
        var res = false;
        res = UpdateRouteAnalysis();
        if (getValue == "application")
            showWarningPopDialog('New candidate route version is successfully created from application routes', 'Ok', '', 'Redirect_ProjectOverview', '', 1, 'info');
        else if (getValue == "lastcandidate")
            showWarningPopDialog('New candidate route version is successfully created from last candidate route version', 'Ok', '', 'Redirect_ProjectOverview', '', 1, 'info');
    }

    function UpdateRouteAnalysis() {
        var rtRevision = $('#LastCandRevisionId').val();
        var res = false;
        $.ajax({
            url: '../SORTApplication/ClearRouteAssessment',
            type: 'POST',
            async: false,
            data: { RouteRevisionId: rtRevision },
            success: function (data) {
                res = data.result;
            }
        });
        return res;
    }
