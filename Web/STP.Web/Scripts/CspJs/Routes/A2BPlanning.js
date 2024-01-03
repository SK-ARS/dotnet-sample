    $(document).ready(function () {
        $(function () {           

            return false;
        });
        //---------------------------------->>> Method for setting css according to the screen fit
        //$(window).resize(function () { applysize(); });
        //---------------------------------->>> Method for setting css according to the screen fit
        var wrapper_width = $('#wrapper_center').width();
        var window_height = window.screen.availHeight;
        var min_height = parseInt(window_height - (184 + 100));

        //resizeMap(wrapper_width - 252 + "px", min_height + "px");

        createContextMenu();
    });   

