var hf_Project_id;
var hf_Mov_btn_show;
var hf_RevisionId;
var VR1App;
var plannruserid;
var Enter_BY_SORT;
var prjstatus;
var checkingstatus;
var sortcheckerid;
function SortAppSummaryInit() {
    hf_Project_id = $('#hf_Project_id').val();
    hf_Mov_btn_show = $('#hf_Mov_btn_show').val();
    hf_RevisionId = $('#hf_RevisionId').val();
    VR1App = $('#VR1Applciation').val();
    plannruserid = $('#PlannrUserId').val();
    if ($('#hf_Status').val() == "MoveVer") {
        Enter_BY_SORT = $('#EnterBySort').val();
        prjstatus = $('#Proj_Status').val();
        prjstatus = prjstatus.replace(/ /g, '%20');
        checkingstatus = $('#CheckerStatus').val();
        sortcheckerid = $('#CheckerId').val();
        $("#leftpanel").html('');
        if ($('#ViewFlag').val() == 0) {
            $('#leftpanel').load('../SORTApplication/SORTLeftPanel?Display=MoveSumm&pageflag=' + Pageflag + '&Mov_btn_show=' + hf_Mov_btn_show + '&PlannerId=' + plannruserid + '&PrjStatus=' + prjstatus + '&CheckingStatus=' + checkingstatus + '&CheckerId=' + sortcheckerid + '&VR1APP=' + VR1App + '&Enter_BY_SORT=' + Enter_BY_SORT, function (data) {
                $("#leftpanel").show();
                SortLeftPanelInit();
                MovementSummaryLeftPanelInit();
            });
        }
    }
    else {
        $("#leftpanel").html('');
        if ($('#ViewFlag').val() == 0) {
            $('#leftpanel').load('../SORTApplication/SORTLeftPanel?Display=ApplSumm&pageflag=' + Pageflag + '&Project_ID=' + hf_Project_id + '&PlannerId=' + plannruserid + '&Rev_ID=' + hf_RevisionId, function (data) {
                $("#leftpanel").show();
                SortLeftPanelInit();
                ApplSummaryLeftPanelInit();
            });
        }
    }
}
$(document).ready(function () {
    $('body').on('click', '#EditCurrRev', function (e) {
        $('#ApplDate').attr("disabled", false);
    });
});


