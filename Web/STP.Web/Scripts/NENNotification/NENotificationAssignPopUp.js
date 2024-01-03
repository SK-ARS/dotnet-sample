var hf_User_id = $('#hf_User_id').val();
$(document).ready(function () {
if($('#hf_User_id').val() !=  '') {
    $("#OrgUserId").val(hf_User_id);
            $('#hdnScrutinyUser').val(OrganisationUserId.options[OrganisationUserId.selectedIndex].text);
        }
        $(function () { $('#divContainer').draggable(); });
    });
    $("#OrgUserId").change(function () {
        $('#err_user_exists').text('');
    });
