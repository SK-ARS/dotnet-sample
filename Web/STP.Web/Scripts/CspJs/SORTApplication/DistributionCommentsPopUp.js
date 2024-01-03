    //#region
    $(document).ready(function () {
        Resize_PopUp(440);
        $('#text').click(function () {
            $('#errorcomment').css("display", "none");
            $("#spanCloseGeneralPopup").on('click', CloseGeneralPopup);
            $("#btnSaveDistributionComment").on('click', SaveDistributionComment);
            $("#btnCloseGeneralPopup").on('click', CloseGeneralPopup);
        });
    });
    //#endregion

    //#region
    function ClosePopUp() {
        $("#dialogue").html('');
        $("#dialogue").hide();
        $("#overlay").hide();
        addscroll();
    }
    //#endregion

    //#region
    function validateDistributionComment() {
        var comment = $("#text").val();
        if (comment == "") {
            $("#errorcomment").show();
            return false;
        }
        else {
            SaveDistributionComment();
        }
    }
    function GetSortSORoute(pointOfCommunication, dataModelPassed) {
        var data = {
            module: 'SOP', pointOfCommunication: pointOfCommunication, dataModel: JSON.stringify(dataModelPassed)
        };
        return ExecutePostAjax("../Workflow/Index", data);
    }
    function SaveDistributionComment() {
        CloseGeneralPopup();
        var _verno = $('#versionid').val();
        var comment = $("#text").val();
        var esdal_ref_no = hauliermnemonic + "/" + esdalref + "/S" + _verno;
        var _versionid = $('#versionid').val();
        var mov_analysisid = $('#analysis_id').val();
        var cand_analysisid = $('#candAnalysisId').val();
        var hajobfileref = $('#HaJobFileRef').val();
        var _pstatuscode = $('#AppStatusCode').val();
        var _preverdistr = $('#PreVerDistr').val();        
        $.ajax({
            url: '../SORTApplication/SaveDistributionComments',
            type: 'POST',
            data: $("#CommentInfoForm").serialize() + "&EsdalReference=" + esdal_ref_no + "&HaulierMnemonic=" + hauliermnemonic + "&EsdalRef=" + esdalref + "&VersionNo=" + versionno + "&VersionId=" + _versionid + '&HaJobFileRef=' + hajobfileref + '&ProjectStatus=' + _pstatuscode + '&PreVersionDistr=' + _preverdistr,
            beforeSend: function () {               
                startAnimation();
            },
            success: function (data) {

                if (data.result == 1) {
                    ShowInfoPopup('Distribution to affected parties completed', 'Redirect_ProjectOverview');
                }
                else if (data.result == 2) {
                    ShowInfoPopup('Distribution is not completed, please contact helpdesk.', 'Redirect_ProjectOverview');
                }
                else {
                    ShowInfoPopup('No possible contact found for distribution.', 'Redirect_ProjectOverview');
                }
                ClosePopUp();
                stopAnimation();
            },
            error: function (xhr, textStatus, errorThrown) {
                stopAnimation();
            },
            complete: function () {               
                stopAnimation();
            }
        });
    }
           
