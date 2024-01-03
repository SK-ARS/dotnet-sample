function SORTHaulierDetailsInit(){
    StepFlag = 0;
    SubStepFlag = 0;
    CurrentStep = "Haulier Details";
    $('#plan_movement_hdng').text("PLAN MOVEMENT");
    $('#current_step').text(CurrentStep);
    SetWorkflowProgress(0);
}
$(document).ready(function () {
    $('body').on('click', '#btn-org', function (e) {
        GoToOrganisationList();
    });
});
function GoToOrganisationList() {
   
    $('#ImportFrom').find('OrganisationImport');
    $.ajax({
        url: '../Organisation/ListOrganisation',
        data: { SORT: 'SORTSO' },
        type: 'GET',
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
           
            $('#banner-container').find('div#filters').remove();
            $('div#filters.organisation-filter').remove();
            document.getElementById("vehicles").style.filter = "unset";
            $("#existingOrganisationList").html(result);

            var filters = $('#existingOrganisationList').find('div#filters');
            $(filters).insertAfter('#banner');
            initAutoComplete();

        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
            stopAnimation();
            $("#createOrganisation").hide();
            $("#Go_To_Organisations").hide();
            $("#existingOrganisationList").show();
            $('#list_heading').text("Select Existing Organisations");
            $("#viewExistingOrganisation").hide();
            $('#save_btn').hide();
            $('#confirm_btn').hide();
        }
    });
}
