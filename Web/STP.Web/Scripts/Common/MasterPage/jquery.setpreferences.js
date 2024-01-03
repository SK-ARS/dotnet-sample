$(document).ready(function () {
    $('#80001').click(function () {
        startAnimation();
        $("#dialogue").html('');
        $("#dialogue").load('../Account/UserPreferences', function () {
            stopAnimation();

            removescroll();
            $("#dialogue").show();
            $("#overlay").show();
        });
        
        return false;
    });
    $('#80002').click(function () {
        startAnimation();
        $("#dialogue").html('');
        $("#dialogue").load('../Account/ChangePassword?needTerms=0', function () {
            stopAnimation();

            removescroll();
            $("#dialogue").show();

            $("#overlay").show();
        });

        return false;
    });
});