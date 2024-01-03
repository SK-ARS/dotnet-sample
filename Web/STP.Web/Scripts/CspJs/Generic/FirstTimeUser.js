    $(document).ready(function () {
        $('#wrapper').html('');
        $('#div_change_pass').find('#span-close').click(function () {
            location.replace('@Url.Action("LogOut", "Account")');
        });
    });
