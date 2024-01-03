var ProjectID;
var OwnerName;
var plannruserid;
var checkingstatus;
var _isvr1;
var Enter_BY_SORT;
var i = 0;
var vid;
var vno;
var movIdDistributed;
var esdalref;
var hmnemonic;
var VR1Number;
var CandidatePermission;
var appstatuscode;
var plannruserid;
var sortcheckerid;
var candidateid;
var movversionno;
var _vr1number;
var movisdisted;
var sonumber;
var sortentered;
var isVR1;
var sort_user_id;
var checker_id;
var proj_status;
var v_Proj_ID;
var VR1Applciation;
var start_add;
var end_add;
var title;
var movname;
var hajobref;
var isCandidateApiCallCompleted = false;
var isSpecialOrderApiCallCompleted = false;
function SORTProjectOverviewInit() {
    ProjectID = $('#projectid').val();
    OwnerName = $('#OwnerName').val();
    plannruserid = $('#PlannrUserId').val();
    checkingstatus = $('#CheckerStatus').val();
    _isvr1 = $('#VR1Applciation').val();
    Enter_BY_SORT = $('#EnterBySort').val();
    i = 0;
    vid = $('#hf_VersionId').val();
    vno = $('#hf_VersionNo').val();
    movIdDistributed = $('#hf_MovIdDistributed').val();
    esdalref = $('#hf_esdalref').val();
    hmnemonic = $('#hf_hauliermnemonic').val();
    VR1Number = $('#hf_VR1Number').val();
    CandidatePermission = $('#hf_CandidatePermission').val();
    v_Proj_ID = $('#projectid').val();
    VR1Applciation = $('#VR1Applciation').val();
    //------------------------------------------------------

    $('#versionId').val(vid);
    versionId = vid;
    $('#versionno').val(vno);
    versionno = vno;
    $('#IsMovDistributed').val(movIdDistributed);
    if (esdalref != esdalref) {
        $('#esdalref').val(esdalref);
        esdalref = $('#hf_esdalref').val();
    }
    if (hauliermnemonic != hmnemonic) {
        $('#hauliermnemonic').val(hmnemonic);
        hauliermnemonic = $('#hf_hauliermnemonic').val();
    }
    ApplRevisions();
    if (_isvr1 == "False") {
        Candidate_route_version();
        Special_order();
    } else {
        isCandidateApiCallCompleted = true;
        isSpecialOrderApiCallCompleted = true;
    }
    Movement_Version();
    ApprevId = $('#hf_AppRevisionId').val();
    revisionno = $('#hf_AppRevisionNo').val();
    revisionId = $('#hf_AppRevisionId').val();
    $('#ApprevId').val(ApprevId);
    $('#revisionId').val(revisionId);
    $('#revisionno').val(revisionno);
    appstatuscode = $('#AppStatusCode').val();
    plannruserid = $('#PlannrUserId').val();
    sortcheckerid = $('#CheckerId').val();
    candidateid = $('#LastCandRouteId').val();
    movversionno = $('#versionno').val();
    _vr1number = VR1Number;
    movisdisted = $('#IsMovDistributed').val();
    sonumber = $('#SONumber').val();
    sortentered = $('#EnteredBySort').val();
    isVR1 = $('#VR1Applciation').val();
    var intLeftPanel = setInterval(function () {
        if (isCandidateApiCallCompleted && isSpecialOrderApiCallCompleted) {
            clearInterval(intLeftPanel);
            $("#leftpanel").html('');
            $('#leftpanel').load('../SORTApplication/SORTLeftPanel?Display=ProjOverview&pageflag=' + Pageflag + '&Project_ID=' + ProjectID + '&Rev_ID=' + revisionId + '&OwnerName=' + OwnerName.replace(/ /g, '%20') + '&PlannerId=' + plannruserid + '&CheckingStatus=' + checkingstatus + '&AppStatus=' + appstatuscode + '&CheckerId=' + sortcheckerid + '&CandidateId=' + $('#LastCandRouteId').val() + '&MoveVersiono=' + movversionno + '+&IsVR1=' + isVR1 + '&VR1Number=' + _vr1number + '&MovIsDistrbted=' + movisdisted + '&SONumber=' + sonumber + '&Enter_BY_SORT=' + sortentered, function () {
                $("#leftpanel").show();
                ProjectOverviewLeftPanelInit();
            });
        }
    }, 100);
    
    //added by ajit
    $('#Owner').val($('#OwnerName').val());
    sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
    checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
    proj_status = $('#AppStatusCode').val() ? $('#AppStatusCode').val() : 0;
    if ((checkingstatus == 301002 || checkingstatus == 301008 || checkingstatus == 301005) && (sort_user_id != checker_id) && $('#VR1Applciation').val() == "False") {
        $('#active-button').hide();
    }
    if (checkingstatus == 301006 && $('#VR1Applciation').val() == "False") {
        if (proj_status == 307014 || proj_status == 307011)
            $('#active-button').show();
        else
            $('#active-button').hide();
    }
}
$(document).ready(function () {
    $('body').on('click', '#active-button', function (e) {
        e.preventDefault();
        onEdit(this);
    });
    $('body').on('click', '#valid-det', function (e) {
        e.preventDefault();
        validDetails(this);
    });
    $('body').on('click', '#edit-can', function (e) {
        e.preventDefault();
        editcancel(this);
    });
    $('body').on('click', '#edit-can', function (e) {
        $('#sortstarterror').text('');
    });
    $('body').on('change', '#Txt_end_add', function (e) {
        $('#sortenderror').text('');
    });
    $('body').on('change', '#Txt_Load_summ', function (e) {
        $('#sortloaderror').text('');
    });
    $('body').on('change', '#Txt_Mov_Name', function (e) {
        $('#sortmoverror').text('');
    });
    $('body').on('change', '#Txt_HA_Job_Ref', function (e) {
        $('#sortreferror').text('');
    });
});
function ApplRevisions() {
    openContentLoader('#ApplRevisions');//startAnimation();
    Checker = Checker.replace(/ /g, '%20');
    var _pstatus = $('#AppStatusCode').val();
    $('#ApplRevisions').load('../SORTApplication/SORTApplRevisions?ProjectID=' + ProjectID + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&Checker=' + Checker + '&PlannerId=' + plannruserid + '&OwnerName=' + OwnerName.replace(/ /g, '%20') + '&ProjectStatus=' + _pstatus, {},
        function () {
            i++;
            if (_isvr1 == "False") {
                if (i == 4) {
                    closeContentLoader('#ApplRevisions');//stopAnimation();
                }
            }
            else {
                if (i == 2) {
                    closeContentLoader('#ApplRevisions');//stopAnimation();
                }
            }
            SortAppRevisionInit();
        });
}
function Movement_Version() {
    openContentLoader('#Movement_Version');
    Checker = Checker.replace(/ /g, '%20');
    $('#Movement_Version').load('../SORTApplication/SORTAppMovementVersion?ProjectID=' + ProjectID + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&EnterBySORT=' + Enter_BY_SORT + '&IsVR1App=' + _isvr1 + '&Checker=' + Checker + '&OwnerName=' + OwnerName.replace(/ /g, '%20'), {},
        function () {
            i++;
            if (_isvr1 == "False") {
                if (i == 4) {
                    closeContentLoader('#Movement_Version');//stopAnimation();
                }
            }
            else {
                if (i == 2) {
                    closeContentLoader('#Movement_Version');//stopAnimation();
                }
            }
            SortAppMovementVersionInit();
        });
}
function Special_order() {
    //startAnimation();
    openContentLoader('#Special_order');
    $('#Special_order').load('../SORTApplication/SORTSpecialOrderView?ProjectID=' + projectid, {},
        function () {
            i++;
            if (_isvr1 == "False") {
                if (i == 4) {
                    closeContentLoader('#Special_order');//stopAnimation();
                }
            }
            else {
                if (i == 2) {
                    closeContentLoader('#Special_order');//stopAnimation();
                }
            }
            SORTSpecialOrderViewInit();
        });
}
function Candidate_route_version() {
    // startAnimation();
    openContentLoader('#Candidate_route_version');
    var wstatus = $('#hf_PStatus').val();
    $('#Candidate_route_version').load('../SORTApplication/SORTAppCandidateRouteVersion?AnalysisId=' + analysis_id + '&Prj_Status=' + encodeURIComponent(wstatus) + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&projectid=' + projectid + '&IsCandPermision=' + CandidatePermission, {},
        function () {
            i++;
            if (_isvr1 == "False") {
                if (i == 4) {
                    closeContentLoader('#Candidate_route_version');//stopAnimation();
                }
            }
            else {
                if (i == 2) {
                    closeContentLoader('#Candidate_route_version');//stopAnimation();
                }
            }
            SORTAppCandidateRouteVersionInit();
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
        $('#Txt_Load_summ').val($('#spn_load_summ').text().substring(0, 60));
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
    $('#Txt_Load_summ').val($('#spn_load_summ').text().substring(0, 60));
    $("#afterEdit").css("display", "none");
    $("#beforeEdit").css("display", "block");
}
function editcancel() {
    $("#afterEdit").css("display", "block");
    $("#beforeEdit").css("display", "none");
}
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
    //if (start_add == "" || end_add == "" || load == "" || movname == "" || hajobref == "") {
        if (load == "") {
            $('#sortloaderror').text('Load summary is required');
            $("#sortloaderror").show();
            flag = 1;
        }
        else {
            if (load.length > 60) {
                $('#sortloaderror').text('Maximum 60 characters allowed');
                $("#sortloaderror").show();
                flag = 1;
            } else {
                $('#sortloaderror').text('');
            }
        }
        if (start_add == "") {
            $('#sortstarterror').text('Start address is required');
            $("#sortstarterror").show();
            flag = 1;
        }
        else {
            if (start_add.length > 100) {
                $('#sortstarterror').text('Maximum 100 characters allowed');
                $("#sortstarterror").show();
                flag = 1;
            } else {
                $('#sortstarterror').text('');
            }
        }
        if (end_add == "") {
            $('#sortenderror').text('End address is required');
            $("#sortenderror").show();
            flag = 1;
        }
        else {
            if (end_add.length > 100) {
                $('#sortenderror').text('Maximum 100 characters allowed');
                $("#sortenderror").show();
                flag = 1;
            } else {
                $('#sortenderror').text('');
            }
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
            if (movname.length > 35) {
                $('#sortmoverror').text('Maximum 35 characters allowed');
                $("#sortmoverror").show();
                flag = 1;
            } else {
                $('#sortmoverror').text('');
            }
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
            if (hajobref.length > 20) {
                $('#sortreferror').text('Maximum 20 characters allowed');
                $("#sortreferror").show();
                flag = 1;
            } else {
                $('#sortreferror').text('');
            }
        }
    //}
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
                ShowSuccessModalPopup('Movement project details saved successfully ', 'Redirect_ProjectOverview');
                //showWarningPopDialog('Movement project details saved successfully. ', 'Ok', '', 'Redirect_ProjectOverview', '', 1, 'info');
            } else {
                ShowWarningPopup('The movement project details not saved', 'WarningCancelBtn');
                //showWarningPopDialog('The movement project details not saved.', 'Ok', '', 'WarningCancelBtn', '', 1, 'error');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            stopAnimation();
            showWarningPopDialog('The movement project details not saved', 'Ok', '', 'WarningCancelBtn', '', 1, 'error');
        }
    });
}