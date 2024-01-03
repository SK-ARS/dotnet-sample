
    var ApprevId = $('#ApprevId').val();
    var VR1_Applciation = "";
    var esdalref = $('#esdalref').val();
    var revisionno = $('#revisionno').val();
    var revision_no = $('#arev_no').val();
    var versionno = $('#versionno').val();
    var versionId = $('#versionId').val();
    var projectid = $('#projectid').val();
    var Work_status = $('#hdnWork_Status').val();
    let movement_Type = 207001;

    var OrgID = $('#OrganisationId').val();

    $(document).ready(function () {
        var chk_status = $('#CheckerStatus').val() ? $('#CheckerStatus').val() : 0;
        var sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
        var checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
        var proj_status = $('#AppStatusCode').val() ? $('#AppStatusCode').val() : 0;

        if ((chk_status == 301002 || chk_status == 301008 || chk_status == 301005) && (sort_user_id != checker_id) && $('#VR1Applciation').val() == "False") {
            $('#tr_decline').hide();
            $('#inactive-button-candidate').hide();
        }

        if (chk_status == 301006 && $('#VR1Applciation').val() == "False") {
            if (proj_status == 307011) {
                $('#inactive-button-candidate').show();
                $('#tr_decline').hide();
                $('#tr_withdraw').hide();
            }
            else {
                $('#tr_decline').hide();
                $('#inactive-button-candidate').hide();
                $('#tr_qa_check').hide();
            }
        }

        if ($('#VR1Applciation').val() === "True") {
            $.ajax({
                url: "../Workflow/GetSORTVR1ProcessingNextTask", /*Expected route : ../SORTApplication/GetSORTVR1ProcessingNextTask */
                type: 'GET',
                data: {
                    esdalReference: hauliermnemonic + '_' + esdalref
                },
                dataType: "json",
                beforeSend: function () {

                },
                success: function (result) {
                    if (result.nextTask.length > 1) {
                        console.log('NEXT TASK :' + result.nextTask);
                    }

                },
                error: function (jqXHR, textStatus, errorThrown) {

                }
            });
        }
        else {
            $.ajax({
                url: "../Workflow/GetSORTSOProcessingNextTask", /*Expected route : ../SORTApplication/GetSORTSOProcessingNextTask */
                type: 'GET',
                data: {
                    esdalReference: hauliermnemonic + '_' + esdalref
                },
                dataType: "json",
                beforeSend: function () {

                },
                success: function (result) {
                    if (result.nextTask.length > 1) {
                        console.log('NEXT TASK :' + result.nextTask);
                    }

                },
                error: function (jqXHR, textStatus, errorThrown) {

                }
            });
        }

        $("#inactive-button-allocate").on('click', Select_AllocatePOP);
        $("#tr_decline").on('click', SortDecline);
        $("#tr_withdraw").on('click', SortWithdraw);
        $("#tr_unwithdraw").on('click', SortUnWithdraw);
        $("#btnMovAgreefuntchck").on('click', Mov_Agree_funt_chck);
        $("#btnMovUnagreefunt").on('click', Mov_Unagree_funt);
        $("#btnCandidateWorkAllocation").on('click', CandidateWorkAllocation);
        $("#btnCreatemovementversionClick").on('click', Createmovementversion_Click);
        $("#btnDistributeMovement").on('click', DistributeMovement);
        $("#btnDistributeAgreedRoute").on('click', DistributeMovement);
        $("#btnCreateAmendementOrder").on('click', CreateAmendementOrder);
        $("#inactive-button-candidate").on('click', CandidateCreateversion_Click);
        $("#imgCreateNew").on('click', createnew);
        $("#btnVR1approvalChck").on('click', VR1approvalChck);
        $("#btnGenerateVR1Number").on('click', GenerateVR1Number);
        $("#td_VR1_doc").on('click', GenerateVR1Document);
        $("#btnAcknowledgementClick").on('click', Acknowledgement_Click);

        $('body').on('click', '#btnCandidateWorkAllocation', function (e) {
            e.preventDefault();
            var checking = $(this).data('Checking');
            var sChecking = $(this).data('SChecking');
            CandidateWorkAllocation(checking, sChecking);
        });

        $('body').on('click', '#tr_qa_check', function (e) {
            e.preventDefault();
            var checking = $(this).data('QAChecking');
            CandidateWorkAllocation(checking, checking);
        });

        $('body').on('click', '#btnSignOffVr1Application', function (e) {
            e.preventDefault();
            var SignOff = $(this).data('SignOff');
            var FChecking = $(this).data('FChecking');
            CandidateWorkAllocation(SignOff, FChecking);
        });

        $('body').on('click', '#btnCompleteChecking', function (e) {
            e.preventDefault();
            var CompleteChecking = $(this).data('CompleteChecking');
            var CSChecking = $(this).data('CSChecking');
            CandidateWorkAllocation(CSChecking, CSChecking);
        });

        $('body').on('click', '#btnCompleteQAChecking', function (e) {
            e.preventDefault();
            var QAChecking = $(this).data('QAChecking');
            var CQAChecking = $(this).data('CQAChecking');
            CandidateWorkAllocation(QAChecking, CQAChecking);
        });

        $('body').on('click', '#btnSignOff', function (e) {
            e.preventDefault();
            var SignOff = $(this).data('SignOff');
            var CFChecking = $(this).data('CFChecking');
            CandidateWorkAllocation(SignOff, CFChecking);
        });

    });


    $('#CreateNewRev').click(function () {
            var hauliermnemonic = $('#hauliermnemonic').val();
            esdalref = $('#esdalref').val();
            var ESDAL_Reference = hauliermnemonic + "/" + esdalref;
            if (VR1_Applciation == "True") {
                ShowWarningPopup('Click Ok to create a new version of "' + ESDAL_Reference + '" for editing.', "ReviseVR1('" + ESDAL_Reference +"')");
            } else {
                var isenterdsort  = $('#hf_EnterdbySort').val(); 
                if (isenterdsort == 1) {
                    ShowWarningPopup('Click Ok to create a new version of "' + ESDAL_Reference + '" for editing.', "ReviseSO('" + ESDAL_Reference +"')");
                    movement_Type = 207001;
                }
                else {
                    ShowErrorPopup('Haulier created application cannot be revised from sort!');
                    movement_Type = 207002;

                }
            }
        });
    function ReviseSO(ESDAL_Reference) {
            CloseWarningPopup();
            var is_replan = 0;
            $.ajax({
                url: "../Application/ReviseSOApplication",
                type: 'post',
                data: { apprevid: ApprevId, ESDALRefCode: EsdalrefCode },
                success: function (data) {
                    if (VR1_Applciation == "True") {
                        movement_Type = 207002;
                        var url = '../Movements/PlanMovement' + EncodedQueryString('appRevisionId=' + data.ApplicationRevisionId + '&cloneRevise=2&appVersionId=' + data.VersionId + '&vehicleType=' + data.SubMovementClass + '&movementId=' + data.MovementId + '&movementType=' + movement_Type);
                        window.location = url;
                    }
                    else {
                        movement_Type = 207001;
                        var url = '../Movements/PlanMovement' + EncodedQueryString('appRevisionId=' + data.ApplicationRevId + '&cloneRevise=2&appVersionId=' + versionId + '&vehicleType=' + data.VehicleClassification + '&movementId=' + data.MovementId + '&movementType=' + movement_Type);
                        window.location = url;
                    }
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

            $("#overlay").show();
            $('.loading').show();
            $.ajax({
                url: "../SORTApplication/ReviseVR1Application",
                type: 'post',
                data: { apprevid: ApprevId, ESDALRefCode: EsdalrefCode },
                success: function (data) {
                    if (data != 0) {
                        window.location.href = "../SORTApplication/SORTListMovemnets" + EncodedQueryString("SORTStatus=CreateVR1&cloneapprevid=" + data + "&revisionId=" + data + '&versionId=' + versionId + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&revisionno=' + revisionno + '&versionno=' + versionno + '&OrganisationId=' + OrgID + '&projecid=' + projectid + '&arev_no=' + revision_no + '&apprevid=' + data + '&VR1Applciation=' + VR1_Applciation + '&pageflag=2' + '&EditRev=true' + '&WorkStatus=' + Work_status + '&Checker=' + Checker + '&Owner=' + Owner.replace(/ /g, '%20'));
                    }
                    $("#overlay").hide();
                    $('.loading').hide();
                },
                error: function (jqXHR, textStatus, errorThrown) {

                }
            });
        }
