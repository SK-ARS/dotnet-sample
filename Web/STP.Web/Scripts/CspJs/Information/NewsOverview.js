    $(document).ready(function () {
        $("#newNewsIcon").css("display", "none");
        if ($('#haulierportal').val() == "True") {
          SelectMenu(7);
        }
        else {
         SelectMenu(5);
        }
        $("#btnbackprev").click(BackToPreviousPage);
    });
    function BackToPreviousPage() {
        window.history.back();
    }
