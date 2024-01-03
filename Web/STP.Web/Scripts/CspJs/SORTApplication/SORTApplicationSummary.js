
    $(document).ready(function () {       
        var VR1App = $('#VR1Applciation').val();
        var plannruserid = $('#PlannrUserId').val();
if($('#hf_Status').val() ==  "MoveVer") {
            var Enter_BY_SORT = $('#EnterBySort').val();
            var prjstatus = $('#Proj_Status').val();
            prjstatus = prjstatus.replace(/ /g, '%20');
            var checkingstatus = $('#CheckerStatus').val();
            var sortcheckerid = $('#CheckerId').val();


            $("#leftpanel").html('');
            if ($('#ViewFlag').val() == 0) {
                $('#leftpanel').load('../SORTApplication/SORTLeftPanel?Display=MoveSumm&pageflag=' + Pageflag + '&Mov_btn_show=' + '@ViewBag.Mov_btn_show' + '&PlannerId=' + plannruserid + '&PrjStatus=' + prjstatus + '&CheckingStatus=' + checkingstatus + '&CheckerId=' + sortcheckerid + '&VR1APP=' + VR1App + '&Enter_BY_SORT=' + Enter_BY_SORT, function (data) {
                    $("#leftpanel").show();
                });
            }
        }
        else {
            $("#leftpanel").html('');
            if ($('#ViewFlag').val() == 0) {
                $('#leftpanel').load('../SORTApplication/SORTLeftPanel?Display=ApplSumm&pageflag=' + Pageflag + '&Project_ID=' + '@ViewBag.Project_id' + '&PlannerId=' + plannruserid + '&Rev_ID=' + '@ViewBag.RevisionId', function (data) {
                    $("#leftpanel").show();
                });
            }
        }
    });

    $('#EditCurrRev').click(function () {
        $('#ApplDate').attr("disabled", false);
    });

