function AnnotationInit() {
    $('.route_selectannot').eq(0).attr({ "checked": true });
    $('.routedivclassannot').hide();
    $('.routedivclassannot').eq(0).show();
    $('body').on('change', '.route_selectannot', function (e) {
        e.preventDefault();
        select_route(this);
    });
}

function select_route(x) {
    $('.routedivclassannot').hide();
    $('#' + x.value).show();
}