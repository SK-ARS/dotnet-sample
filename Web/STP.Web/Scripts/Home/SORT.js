$(document).ready(function () {
    SelectMenu(1);
    var createAlertMsg = $('#CreateAlert').val();
    var SetPreference = $('#SetPreferenceMessage').val();
    if (createAlertMsg == "True") {
        ShowDialogWarningPop('Password changed successfully', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
    }
    if (SetPreference == "True") {
        ShowDialogWarningPop('User preferences saved succesfully', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
    }

    $('body').on('click', '.selectprevmovmntveh', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var ProjectStatus = $(this).data('projectstatus');
        var hauliermnemonic = $(this).data('hauliermnemonic');
        var ProjectEsdalReference = $(this).data('projectesdalreference');
        var ProjectID = $(this).data('projectid');
        SelectPrevitMovementsVehicle(id, ProjectStatus, hauliermnemonic, ProjectEsdalReference, ProjectID);
    });
    $('body').on('click', '.selectCrntMvntsrt', function (e) {

        e.preventDefault();
        var id = $(this).data('id');
        var ProjectStatus = $(this).data('projectstatus');
        var hauliermnemonic = $(this).data('hauliermnemonic');
        var ProjectEsdalReference = $(this).data('projectesdalreference');
        var ProjectID = $(this).data('projectid');
        SelectCurrentMovementsRoute(id, ProjectStatus, hauliermnemonic, ProjectEsdalReference, ProjectID);
    });

});
