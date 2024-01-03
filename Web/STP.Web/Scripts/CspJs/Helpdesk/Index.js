        SelectMenu(1);
        $(document).ready(function () {
            var createAlertMsg = $('#CreateAlert').val();
            var SetPreference = $('#SetPreferenceMessage').val();
            if (createAlertMsg == "True") {
                ShowDialogWarningPop('Password changed successfully.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
            }
            if (SetPreference == "True") {
                ShowDialogWarningPop('User preferences saved succesfully.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');

            }
            $("#closeNav").on('click', closeNav);
        }); 

        
