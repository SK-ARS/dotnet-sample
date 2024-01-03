$(document).ready(function () {
    $('.route_selectcau').eq(0).attr({ "checked": true });
    $('.routedivclasscau').hide();
    $('.routedivclasscau').eq(0).show();
    $('body').on('change', '.route_selectcau', function (e) {
        e.preventDefault();
        select_route(this);
    });
});

function select_route(x) {
    $('.routedivclasscau').hide();
    $('#' + x.value).show();
}