    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref = $('#esdalref').val();
    var revisionno = $('#revisionno').val();
    var revision_no = $('#arev_no').val();
    var versionno = $('#versionno').val();
    var versionId = $('#versionId').val();
    var revisionId = $('#revisionId').val();
    var ApprevId = $('#ApprevId').val();
    var projectid = $('#projectid').val();
    var Pageflag = $('#Pageflag').val();
    var OrgID = $('#OrganisationId').val();
    var AppEdit = $('#AppEdit').val();
    var Proj_Status = $('#Proj_Status').val();
    var AppStatusCode = $('#AppStatusCode').val();
    var Work_status = $('#hdnWork_Status').val();
    var VR1_Applciation = $('#VR1Applciation').val();
    var chk_status = $('#CheckerStatus').val() ? $('#CheckerStatus').val() : 0;
    var sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
    var checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
    var newUrl = '';
    $(document).ready(function () {
        if (Proj_Status == 'Withdrawn' || Proj_Status == 'Declined') {
            $('#menu-buttons').hide();
            $('#tr_edit_rev').hide();
        }
        if ((chk_status == 301002 || chk_status == 301008 || chk_status == 301005) && (sort_user_id != checker_id) && $('#VR1Applciation').val() == "False") {
            $('#menu-buttons').hide();
            $('#tr_edit_rev').hide();
        }
        if (chk_status == 301006 && $('#VR1Applciation').val() == "False") {
            if (AppStatusCode == 307014 || AppStatusCode == 307011) {
                $('#menu-buttons').show();
                $('#tr_edit_rev').show();
            }
            else {
                $('#menu-buttons').hide();
                $('#tr_edit_rev').hide();
            }
        }
    });
    $('#EditCurrRev').click(function () {
        if (VR1_Applciation == "True") {
            window.location.href = "../SORTApplication/SORTListMovemnets" + EncodedQueryString("SORTStatus=CreateVR1&revisionId=" + revisionId + '&versionId=' + versionId + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&revisionno=' + revisionno + '&versionno=' + versionno + '&OrganisationId=' + OrgID + '&projecid=' + projectid + '&apprevid=' + ApprevId + '&VR1Applciation=' + VR1Applciation + '&pageflag=' + Pageflag + '&EditRev=true' + '&WorkStatus=' + Work_status + '&EditFlag=1' + '&Checker=' + Checker + '&Owner=' + Owner.replace(/ /g, '%20'));

        } else {
            window.location.href = "../SORTApplication/SORTListMovemnets" + EncodedQueryString("SORTStatus=CreateSO&revisionId=" + revisionId + '&versionId=' + versionId + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&revisionno=' + revisionno + '&versionno=' + versionno + '&OrganisationId=' + OrgID + '&projecid=' + projectid + '&apprevid=' + ApprevId + '&pageflag=' + Pageflag + '&EditRev=true' + '&WorkStatus=' + Work_status + '&EditFlag=1');
        }
    });

    $('#CreateNewRev').click(function () {

            var hauliermnemonic = $('#hauliermnemonic').val();
        esdalref = $('#esdalref').val();
        var newesdalref = $('#ESDALReferenceSORT').val();       
            var ESDAL_Reference = hauliermnemonic + "/" + esdalref;
        if (newesdalref != null || newesdalref != "") {
            ESDAL_Reference = newesdalref;
        }
            if (VR1_Applciation == "True") {
                ShowWarningPopup('Click Yes to create a new version of "' + ESDAL_Reference + '" for editing.', "ReviseVR1('" + ESDAL_Reference+"')");
            } else {
                var isenterdsort  = $('#hf_EnterdbySort').val(); 
                if (isenterdsort == 1) {

                    ShowWarningPopup('Click Yes to create a new version of "' + ESDAL_Reference + '" for editing.', "ReviseSO('" + ESDAL_Reference +"')");
                    movement_Type = 207001;
                }
                else {
                    ShowErrorPopup('Haulier created application cannot be revised from sort!');
                    movement_Type = 207002;

                }
            }
        });
         function ReviseSO(EsdalrefCode) {
            CloseWarningPopup();
            var is_replan = 0;
            $.ajax({
                url: "../Application/ReviseSOApplication",
                type: 'post',
                data: { apprevid: ApprevId, ESDALRefCode: EsdalrefCode },
                success: function (data) {                   
                    var redirectData = EncodedQueryString("revisionId=" + data.ApplicationRevisionId);
                    var redirectUrl = '../Movements/OpenMovement';
                    window.location = redirectUrl + redirectData;
                    $("#overlay").hide();
                    $('.loading').hide();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                }
            });
        }

        function WarningBrokenPopUp() {
            CloseWarningPopup();

            window.location = newUrl;
        }

    function ReviseVR1(EsdalrefCode) {
            CloseWarningPopup();
            $.ajax({
                url: "../SORTApplication/ReviseVR1Application",
                type: 'post',
                data: { apprevid: ApprevId, ESDALRefCode: EsdalrefCode },
                success: function (data) {
                    var redirectData = EncodedQueryString("revisionId=" + data.ApplicationRevisionId);
                    var redirectUrl = '../Movements/OpenMovement';
                    window.location = redirectUrl + redirectData;
                },
                error: function (jqXHR, textStatus, errorThrown) {
                }
            });
        }
