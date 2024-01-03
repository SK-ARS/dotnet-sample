        SelectMenu(8);
        $(document).ready(function () {
            ManageOrganisationList();
       
    });
    function CreateOrganisation() {*@

        var isAccessible = $('#EditForAdmin').val();
        if (isAccessible == 'False') {
            ShowErrorPopup('You are not authorized to create');
        }
        else {
            $("#manage-user").remove();
            startAnimation();
            $("#createUser").load('@Url.Action("CreateOrganisation", "Organisation", new { mode = "Save" })', function () {
                stopAnimation();
            });
            $("#manage-user").hide();
        }
    }
    function BackToManageOrg() {
        startAnimation();
        location.reload();
    }

