$(document).ready(function () {
    $('#menu-cros-search').click(function () {
        $('.left-panel-inner').show();
        $('#search-details-div').hide();
        $('#search-details-div').html("");
    });


    $('#nav').find('li :not(#menu-cros-search)').click(function () {
       
    });

});

function notImplemented() {
}