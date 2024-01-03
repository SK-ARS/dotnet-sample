$(document).ready(function () {
    $('#route_plan').click(function () {
        GetPlan();
    });

    $('#btn_routeplan').live('click',function () {
        planRoute();
    });

});

function GetPlan() {
    var url = $('#RouteUrl').val();
    $.ajax({
        type: 'POST',
        dataType: 'html',
        //contentType: 'application/json; charset=utf-8',
        url: url,
        //data: '{ searchValue: "' + searchValue + '",searchType:"' + searchType + '" }',
        beforeSend: function (xhr) {
            $('.left-panel-inner').hide();
            $('#search-details-div').html("");
            $('#search-details-div').show();
            $('#search-details-div').append('<div id="wait-icon2"><img src="../../Content/Images/loading.gif" id="wait-image"/></div>');
            //xhr.overrideMimeType("text/plain; charset=x-user-defined");
        }
    }).done(function (Result) {
        if (Result != null) {
            $('.left-panel-inner').hide();
            $('#search-details-div').show();
            $('#search-details-div').html(Result);
        }
    }).fail(function (error, a, b) {
       
    }).always(function (xhr) {
        $('#wait-icon').remove();
    });
}