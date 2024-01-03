    $(document).ready(function () {
        $("#ahrefSortCreateCandidateRoute").on('click', SortCreateCandidateRoute);
        $('body').on('click', '#li_edit_cndRt', function (e) {
            e.preventDefault();
            var CnRouteId = $(this).data('CnRouteId');
            EditCandiRouteName(CnRouteId);
        });

        $('body').on('click', '#ahrefVersinRevision', function (e) {
            e.preventDefault();
            var VersinRevisionId = $(this).data('VersinRevisionId');
            var VersinVrListType = $(this).data('VersinVrListType');
            CurreMovemenRouteList(VersinRevisionId, VersinVrListType);
        });

        $('body').on('click', '#ahrefShowCandidateRoutes', function (e) {
            e.preventDefault();
            var ReviosionId = $(this).data('ReviosionId');
            var candname = $(this).data('candname');
            var RevisionNo = $(this).data('RevisionNo');
            var RouteId = $(this).data('RouteId');
            var AnalysisId = $(this).data('AnalysisId');
            ShowCandidateRoutes(ReviosionId, candname, RevisionNo, RouteId, AnalysisId);
        });

        $('body').on('click', '#ahrefCurreMovemenVehicleList', function (e) {
            e.preventDefault();
            var ReviosionId = $(this).data('ReviosionId');
            var SecondParameter = $(this).data('SecondParameter');

            CurreMovemenVehicleList(ReviosionId, SecondParameter);
        });

        $('#LastCandRouteId').val('@CandidateRouteId');
        $('#LastCandVersion').val('@LastVersion');
        $('#LastCandRevisionId').val('@LastRevisionId');
        var chk_status = $('#CheckerStatus').val() ? $('#CheckerStatus').val() : 0;
        var sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
        var checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
        if ((chk_status == 301002 || chk_status == 301008 || chk_status == 301005) && (sort_user_id != checker_id)) {
            $('#li_edit_cndRt').hide();
        }
        if (chk_status == 301006) {
            $('#li_edit_cndRt').hide();
        }

        $('.tree li').each(function () {
            if ($(this).children('ol').length > 0) {
                $(this).addClass('parent');
            }
        });
        $('.tree li').each(function () {
            $(this).toggleClass('active');
            $(this).children('ol').slideToggle('fast');
        });
        $('.tree li.parent > a').click(function () {
            $(this).parent().toggleClass('active');
            $(this).parent().children('ol').slideToggle('fast');
        });
if($('#hf_IsVehicleCurrentMovenet').val() ==  'True' ||  $('#hf_IsCurrentMovenet1').val() ==  'True') {
            if ($('#IsMyStructure').val() == "Yes" || $('#StruRelatedMove').val() == "Yes") {
                StruApplRevisions_SelectCurrMovmt('@ViewBag.IsVehicleCurrentMovenet');
                StruMovementVersion__SelectCurrMovmt('@ViewBag.IsVehicleCurrentMovenet');
            }
            else {
                ApplRevisions_SelectCurrMovmt('@ViewBag.IsVehicleCurrentMovenet');
                MovementVersion__SelectCurrMovmt('@ViewBag.IsVehicleCurrentMovenet');
            }
        }
    });
        function ApplRevisions_SelectCurrMovmt(V_IsVehicleCurrentMovenet) {

            var ProjectID, OwnerName = '', plannruserid, checkingstatus, _isvr1, Enter_BY_SORT, PreMov_hauliermnemonic, PreMov_esdalref;
            ProjectID = $('#projectid').val();
            OwnerName = '';
            plannruserid = $('#PlannrUserId').val();
            checkingstatus = $('#CheckerStatus').val();
            _isvr1 = $('#VR1Applciation').val();
            Enter_BY_SORT = $('#EnterBySort').val();
            if ($("#IsPrevMoveOpion").val() == 'True' || $("#IsPrevMoveOpion").val() == 'true') {
                ProjectID = $('#PrevMove_projectid').val();
                PreMov_hauliermnemonic = $('#PrevMove_hauliermnemonic').val();
                PreMov_esdalref = $('#PrevMove_esdalref').val();
            }
            var hauliermnemonic = $('#hauliermnemonic').val();

            var esdalref = $('#esdalref').val();
            if (esdalref== 0) {
                esdalref = $('#esdalReferenceNumberHiddenValue').val();
            }

            var N_IsRouteCurrentMovenet = true;
            if (V_IsVehicleCurrentMovenet == 'True')
                N_IsRouteCurrentMovenet = false;
            startAnimation()
            Checker = "";//Checker.replace(/ /g, '%20');
            var _pstatus = $('#AppStatusCode').val();
            if ($("#IsPrevMoveOpion").val() == 'False' || $("#IsPrevMoveOpion").val() == 'false') {

                $('#SelectCurrentMovementsVehicle1').load('../SORTApplication/SORTApplRevisions?ProjectID=' + ProjectID + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&Checker=' + Checker + '&PlannerId=' + plannruserid + '&OwnerName=' + OwnerName.replace(/ /g, '%20') + '&ProjectStatus=' + _pstatus + '&IsChooseCurrMovmOption=' + N_IsRouteCurrentMovenet + '&IsVehicleCurrentMovenet=' + V_IsVehicleCurrentMovenet, {},
                    function () {
                        $('#SelectCurrentMovementsVehicle1').show();
                        //$('#General').show();
                        stopAnimation()
                    });
            }
            else {

                $('#SelectCurrentMovementsVehicle1').load('../SORTApplication/SORTApplRevisions?ProjectID=' + ProjectID + '&hauliermnemonic=' + PreMov_hauliermnemonic + '&esdalref=' + esdalref + '&Checker=' + Checker + '&PlannerId=' + plannruserid + '&OwnerName=' + OwnerName.replace(/ /g, '%20') + '&ProjectStatus=' + _pstatus + '&IsChooseCurrMovmOption=' + N_IsRouteCurrentMovenet + '&IsVehicleCurrentMovenet=' + V_IsVehicleCurrentMovenet, {},
                    function () {
                        $('#SelectCurrentMovementsVehicle1').show();
                        stopAnimation()
                    });

            }
        }
        function MovementVersion__SelectCurrMovmt(V_IsVehicleCurrentMovenet) {
            var ProjectID = $('#projectid').val();
            var OwnerName = '';
            var plannruserid = $('#PlannrUserId').val();
            var checkingstatus = $('#CheckerStatus').val();
            var _isvr1 = $('#VR1Applciation').val();
            var Enter_BY_SORT = $('#EnterBySort').val();

            var PreMov_hauliermnemonic, PreMov_esdalref;
            var N_IsRouteCurrentMovenet = true;
            if (V_IsVehicleCurrentMovenet == 'True')
                N_IsRouteCurrentMovenet = false;
            startAnimation()

            if ($("#IsPrevMoveOpion").val() == 'true' || $("#IsPrevMoveOpion").val() == 'True') {
                ProjectID = $('#PrevMove_projectid').val();
                PreMov_hauliermnemonic = $('#PrevMove_hauliermnemonic').val();
                PreMov_esdalref = $('#PrevMove_esdalref').val();
                $('#previousMovementImportBack').show();
                if (N_IsRouteCurrentMovenet) {
                    $('#previousMovementImportBack').hide();//hide back button for imprting a route from prev movment
                }
            }

            if (esdalref == 0) {
                esdalref = $('#esdalReferenceNumberHiddenValue').val();
            }
            Checker = Checker.replace(/ /g, '%20');
            if ($("#IsPrevMoveOpion").val() == 'False' || $("#IsPrevMoveOpion").val() == 'false') {
                $('#SelectCurrentMovementsVehicle2').load('../SORTApplication/SORTAppMovementVersion?ProjectID=' + ProjectID + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&EnterBySORT=' + Enter_BY_SORT + '&IsVR1App=' + _isvr1 + '&Checker=' + Checker + '&OwnerName=' + OwnerName.replace(/ /g, '%20') + '&IsChooseCurrMovmOption=' + N_IsRouteCurrentMovenet + '&IsVehicleCurrentMovenet=' + V_IsVehicleCurrentMovenet, {},
                    function () {
                        $('#SelectCurrentMovementsVehicle2').show();
                        stopAnimation()
                    });
            }
            else {
                $('#SelectCurrentMovementsVehicle2').load('../SORTApplication/SORTAppMovementVersion?ProjectID=' + ProjectID + '&hauliermnemonic=' + PreMov_hauliermnemonic + '&esdalref=' + esdalref + '&EnterBySORT=' + Enter_BY_SORT + '&IsVR1App=' + _isvr1 + '&Checker=' + Checker + '&OwnerName=' + OwnerName.replace(/ /g, '%20') + '&IsChooseCurrMovmOption=' + N_IsRouteCurrentMovenet + '&IsVehicleCurrentMovenet=' + V_IsVehicleCurrentMovenet, {},
                    function () {
                        $('#SelectCurrentMovementsVehicle2').show();
                        stopAnimation()
                    });
            }
        }
    function ShowRouteVer() {
        showWarningPopDialog('Functionality is not implemented', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
    }
    function CandRoute_Click(e) {
        var routeid = $(e).attr("data-rid");
        if ($('#' + routeid).css("display") == "none") {
            $('#' + routeid).fadeIn("slow", function () {
            });
        }
        else {
            $('#' + routeid).fadeOut("slow", function () {
            });
        }
    }
    function CandVersion_Click(e) {
        var versionId = $(e).attr("data-vid");
        if ($('#' + versionId).css("display") == "none") {
            $('#' + versionId).fadeIn("slow", function () {
            });
        }
        else {
            $('#' + versionId).fadeOut("slow", function () {
            });
        }
    }
    function SortCreateCandidateRoute() {
        var projectid = $('#projectid').val();
        var revisionId = $('#revisionId').val();
        startAnimation();
        $.post('/SORTApplication/CheckVehicleForApplication?PROJ_ID=' + projectid + '&Rev_Id=' + revisionId, function (data) {
            if (data == true) {
                stopAnimation();
                $("#overlay").show();
                $('.loading').show();
                var options = { "backdrop": "static", keyboard: true };
                $.ajax({
                    type: "GET",
                    url: "../SORTApplication/CreateCandidateRoute",
                    //contentType: "application/json; charset=utf-8",
                    data: { },
                    datatype: "json",
                    success: function (data) {
                        $('#generalPopupContent').html(data);
                        $('#generalPopup').modal(options);
                        $('#generalPopup').modal('show');
                        $('.loading').hide();
                    },
                    error: function () {

                    }
                });
                stopAnimation();
            }
            else {
                stopAnimation();
                ShowDialogWarningPop('Submit the edited application before proceeding', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
            }
        });
    }

    function ShowCandidateRoutes(rev_Id,candname, candversionno, routeId, analysisId) {
        startAnimation();
        if ('@LastRevisionId' == rev_Id)
            isLastVersion = true;
        else
            isLastVersion = false;
        var sowner = $('#Owner').val();
        sowner = sowner.replace(/ /g, '%20');
        window.location.href = '../SORTApplication/SORTListMovemnets' + EncodedQueryString('SORTStatus=CandidateRT&projecid=' + ProjectID + '&VR1Applciation=' + VR1Applciation + '&reduceddetailed=' + false + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&revisionId=' + rev_Id + '&movementId=' + movementId + '&apprevid=' + ApprevId + '&revisionno=' + revisionno + '&OrganisationId=' + OrgID + '&versionno=' + versionno + '&versionId=' + versionId + '&VR1Applciation=' + VR1Applciation + '&reduceddetailed=' + false + '&pageflag=' + Pageflag + '&esdal_history=' + esdal_history + '&candName=' + candname + '&candVersionno=' + candversionno + '&CandRouteId=' + routeId + '&LatestRevisionId=' + '@ViewBag.LatestRevisionId' + '&analysisId=' + analysisId + '&IsLastVersion=' + isLastVersion + '&EnterBySORT=' + Enter_BY_SORT + '&Owner=' + sowner);
        stopAnimation();
    }

    function EditCandiRouteName(CnRouteID)
    {
        removescroll();
       // $("#dialogue").html('');
        //$("#dialogue").load("../SORTApplication/SORTAppEditCandiRouteName?CnRouteID=" + CnRouteID);
       // $("#dialogue").show();
        $("#overlay").show();
        var options = { "backdrop": "static", keyboard: true };
        $.ajax({
            type: "GET",
            url: "../SORTApplication/SORTAppEditCandiRouteName",
            //contentType: "application/json; charset=utf-8",
            data: { CnRouteID: CnRouteID},
            datatype: "json",
            success: function (data) {
                $('#generalPopupContent').html(data);
                $('#generalPopup').modal(options);
                $('#generalPopup').modal('show');
                $('.loading').hide();
            },
            error: function () {
                alert("Dynamic content load failed.");
            }
        });
    }


