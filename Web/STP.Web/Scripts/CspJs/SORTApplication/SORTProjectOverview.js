    var ProjectID = $('#projectid').val();
    var OwnerName = $('#OwnerName').val();
    var plannruserid = $('#PlannrUserId').val();
    var checkingstatus = $('#CheckerStatus').val();
    var _isvr1 = $('#VR1Applciation').val();
    var Enter_BY_SORT = $('#EnterBySort').val();
    var i = 0;
    $(document).ready(function () {
        $("#active-button").on('click', onEdit);
        $("#valid-det").on('click', validDetails);
        $("#edit-can").on('click', editcancel);

        $('#versionId').val('@VersionId');
        versionId = '@VersionId';
        $('#versionno').val('@VersionNo');
        versionno = '@VersionNo';
        $('#IsMovDistributed').val('@MovIdDistributed');
        if (esdalref != '@ViewBag.esdalref') {
            $('#esdalref').val(@ViewBag.esdalref);
    esdalref  = $('#hf_esdalref').val(); 
            }
    if (hauliermnemonic != '@ViewBag.hauliermnemonic') {
        $('#hauliermnemonic').val('@ViewBag.hauliermnemonic');
        hauliermnemonic  = $('#hf_hauliermnemonic').val(); 
        }
    ApplRevisions();
    if (_isvr1 == "False") {
        Candidate_route_version();
        Special_order();
    }
    Movement_Version();

    ApprevId  = $('#hf_AppRevisionId').val(); 
    revisionno  = $('#hf_AppRevisionNo').val(); 
    revisionId  = $('#hf_AppRevisionId').val(); 
    $('#ApprevId').val(ApprevId);
    $('#revisionId').val(revisionId);
    $('#revisionno').val(revisionno);

    var appstatuscode = $('#AppStatusCode').val();
    var plannruserid = $('#PlannrUserId').val();
    var sortcheckerid = $('#CheckerId').val();
    var candidateid = $('#LastCandRouteId').val();

    var movversionno = $('#versionno').val();
    var _vr1number = '@VR1Number';
    var movisdisted = $('#IsMovDistributed').val();
    var sonumber = $('#SONumber').val();
    var sortentered = $('#EnteredBySort').val();
    var isVR1 = $('#VR1Applciation').val();
    $("#leftpanel").html('');
        $('#leftpanel').load('../SORTApplication/SORTLeftPanel?Display=ProjOverview&pageflag=' + Pageflag + '&Project_ID=' + ProjectID + '&Rev_ID=' + revisionId+ '&OwnerName=' + OwnerName.replace(/ /g, '%20') + '&PlannerId=' + plannruserid + '&CheckingStatus=' + checkingstatus + '&AppStatus=' + appstatuscode + '&CheckerId=' + sortcheckerid + '&CandidateId=' + candidateid + '&MoveVersiono=' + movversionno + '+&IsVR1=' + isVR1 + '&VR1Number=' + _vr1number + '&MovIsDistrbted=' + movisdisted + '&SONumber=' + sonumber + '&Enter_BY_SORT=' + sortentered, function () {
            $("#leftpanel").show();
    });

    //added by ajit
    $('#Owner').val($('#OwnerName').val());
    var sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
    var checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
    var proj_status = $('#AppStatusCode').val() ? $('#AppStatusCode').val() : 0;
    if ((checkingstatus == 301002 || checkingstatus == 301008 || checkingstatus == 301005) && (sort_user_id != checker_id) && $('#VR1Applciation').val() == "False") {
        $('#active-button').hide();
    }
    if (checkingstatus == 301006 && $('#VR1Applciation').val() == "False") {
        if (proj_status == 307014 || proj_status == 307011)
            $('#active-button').show();
        else
            $('#active-button').hide();
    }
        });

    function ApplRevisions() {
        startAnimation();
        Checker = Checker.replace(/ /g, '%20');
        var _pstatus = $('#AppStatusCode').val();
       
        $('#ApplRevisions').load('../SORTApplication/SORTApplRevisions?ProjectID=' + ProjectID + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&Checker=' + Checker + '&PlannerId=' + plannruserid + '&OwnerName=' + OwnerName.replace(/ /g, '%20') + '&ProjectStatus=' + _pstatus, {},
            function () {
                i++;
                if (_isvr1 == "False") {
                    if (i == 4) {
                        stopAnimation();
                    }
                }
                else {
                    if (i == 2) {
                        stopAnimation();
                    }
                }
            });
    }
    function Movement_Version() {       
        Checker = Checker.replace(/ /g, '%20');
        $('#Movement_Version').load('../SORTApplication/SORTAppMovementVersion?ProjectID=' + ProjectID + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&EnterBySORT=' + Enter_BY_SORT + '&IsVR1App=' + _isvr1 + '&Checker=' + Checker + '&OwnerName=' + OwnerName.replace(/ /g, '%20'), {},
            function () {               
                i++;
                if (_isvr1 == "False") {
                    if (i == 4) {
                        stopAnimation();
                    }
                }
                else {
                    if (i == 2) {
                        stopAnimation();
                    }
                }
            });
    }
    function Special_order() {
        //startAnimation();
        $('#Special_order').load('../SORTApplication/SORTSpecialOrderView?ProjectID=' + projectid, {},
            function () {
                i++;
                if (_isvr1 == "False") {
                    if (i == 4) {
                        stopAnimation();
                    }
                }
                else {
                    if (i == 2) {
                        stopAnimation();
                    }
                }
            });
    }
    function Candidate_route_version() {
       // startAnimation();
        var wstatus  = $('#hf_PStatus').val(); 
        $('#Candidate_route_version').load('../SORTApplication/SORTAppCandidateRouteVersion?AnalysisId=' + analysis_id + '&Prj_Status=' + encodeURIComponent(wstatus) + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&projectid=' + projectid + '&IsCandPermision=' + '@ViewBag.CandidatePermission', {},
            function () {
                i++;
                if (_isvr1 == "False") {
                    if (i == 4) {
                        stopAnimation();
                    }
                }
                else {
                    if (i == 2) {
                        stopAnimation();
                    }
                }
            });
    }

    function ProjectPopUp() {
        $("#overlay").show();
        $('.loading').show();
        $("#dialogue").html('');
        $("#dialogue").load('../SORTApplication/SORTMovProjDetail?ProjectID=' + ProjectID, function (responseText, textStatus, req) {
            $('.loading').hide();
            if (textStatus == "error") {
                location.reload();
            }
            $('#Txt_Mov_Name').val($('#spn_mov_name').text().substring(0, 35));
            // $('#Txt_Mov_Name_From').val($('#spn_start_add').text());
            // $('#Txt_Mov_Name_To').val($('#spn_end_add').text());
            $('#Txt_HA_Job_Ref').val($('#spn_ha_ref').text().substring(0, 20));
            $('#Txt_start_add').val($('#spn_start_add').text().substring(0, 100));
            $('#Txt_end_add').val($('#spn_end_add').text().substring(0, 100));
            $('#Txt_Load_summ').val($('#spn_load_summ').text().substring(0, 50));
        });
        removescroll();
        resetdialogue();
        $("#overlay").show();
        $("#dialogue").show();
    }

    function onEdit() {
        $('#Txt_Mov_Name').val($('#spn_mov_name').text().substring(0, 35));
        $('#Txt_HA_Job_Ref').val($('#spn_ha_ref').text().substring(0, 20));
        $('#Txt_start_add').val($('#spn_start_add').text().substring(0, 100));
        $('#Txt_end_add').val($('#spn_end_add').text().substring(0, 100));
        $('#Txt_Load_summ').val($('#spn_load_summ').text().substring(0, 50));
        $("#afterEdit").css("display", "none");
        $("#beforeEdit").css("display", "block");
    }
    function editcancel() {
        $("#afterEdit").css("display", "block");
        $("#beforeEdit").css("display", "none");
    }

    var v_Proj_ID = $('#projectid').val();
    var VR1Applciation = $('#VR1Applciation').val();
    var start_add = null;
    var end_add = null;
    var title = null;
    var movname = null;
    var hajobref = null;

    $('#Txt_start_add').change(function () {
        $('#sortstarterror').text('');
    });
    $('#Txt_end_add').change(function () {
        $('#sortenderror').text('');
    });
    $('#Txt_Load_summ').change(function () {
        $('#sortloaderror').text('');
    });
    $('#Txt_Mov_Name').change(function () {
        $('#sortmoverror').text('');
    });
    $('#Txt_HA_Job_Ref').change(function () {
        $('#sortreferror').text('');
    });

    function validDetails() {
        if (validateProjectDetails()) {
            saveMovProjDetails();
        }
    }

    function validateProjectDetails() {
        movname = $('#Txt_Mov_Name').val();
        hajobref = $('#Txt_HA_Job_Ref').val();
        start_add = $('#Txt_start_add').val();
        end_add = $('#Txt_end_add').val();
        load = $('#Txt_Load_summ').val();
        var flag = 0;
        if (start_add == "" || end_add == "" || load == "" || movname == "" || hajobref == "") {
            if (load == "") {
                $('#sortloaderror').text('Load summary is required');
                $("#sortloaderror").show();
                flag = 1;
            }
            else {
                $('#sortloaderror').text('');
            }
            if (start_add == "") {
                $('#sortstarterror').text('Start address is required');
                $("#sortstarterror").show();
                flag = 1;
            }
            else {
                $('#sortstarterror').text('');
            }
            if (end_add == "") {
                $('#sortenderror').text('End address is required');
                $("#sortenderror").show();
                flag = 1;
            }
            else {
                $('#sortenderror').text('');
            }
            if (movname == "") {
                if (VR1Applciation != "True") {
                    $('#sortmoverror').text('Movement name is required');
                    $("#sortmoverror").show();
                    flag = 1;
                } else {
                    $('#sortmoverror').text('');
                }
            }
            else {
                $('#sortmoverror').text('');
            }
            if (hajobref == "") {
                if (VR1Applciation != "True") {
                    $('#sortreferror').text('NH job reference number is required');
                    $("#sortreferror").show();
                    flag = 1;
                } else {
                    $('#sortreferror').text('');
                }
            }
            else {
                $('#sortreferror').text('');
            }
        }
        if (flag == 1) {
            return false;
        }
        else {
            return true;
        }

    }

    function saveMovProjDetails() {
        $.ajax({
            url: "../SORTApplication/SaveMovementProjDetails",
            type: 'post',
            data: { projectId: v_Proj_ID, Mov_Name: movname, From_Add: start_add, To_Add: end_add, Load: load, HA_Job_Ref: hajobref },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                stopAnimation();
                if (result == "1") {
                    ClosePopUp();
                    ShowInfoPopup('Movement project details saved successfully. ', 'Redirect_ProjectOverview');
                    //showWarningPopDialog('Movement project details saved successfully. ', 'Ok', '', 'Redirect_ProjectOverview', '', 1, 'info');
                } else {
                    ShowWarningPopup('The movement project details not saved.', 'WarningCancelBtn');
                    //showWarningPopDialog('The movement project details not saved.', 'Ok', '', 'WarningCancelBtn', '', 1, 'error');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                stopAnimation();
                showWarningPopDialog('The movement project details not saved.', 'Ok', '', 'WarningCancelBtn', '', 1, 'error');
            }
        });
    }
