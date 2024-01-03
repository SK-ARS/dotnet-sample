$(document).ready(function () {
    SelectMenu(8);
    //ManageOrganisationList();
});
function CreateOrganisation() {
    let isAccessible = $('#EditForAdmin').val();
    if (isAccessible == 'False') {
        ShowErrorPopup('You are not authorized to create');
    }
    else {
        $("#manage-user").remove();
        startAnimation();
        $("#createUser").load('../Organisation/CreateOrganisation', { mode : "Save" }, function () {
            stopAnimation();            
            CreateOrganisationInit();
        });
        $("#manage-user").hide();
    }
}
function BackToManageOrg() {
    startAnimation();
    location.reload();
}

