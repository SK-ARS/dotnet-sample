function SORTSORouting(pointOfCommunication, dataModelPassed) {

    var data = {
        module: 'SOP', pointOfCommunication: pointOfCommunication, dataModel: JSON.stringify(dataModelPassed)
    };
    return ExecutePostAjax("../Workflow/Index", data);
}
function SORTVR1Routing(pointOfCommunication, dataModelPassed) {

    var data = {
        module: 'SVR1', pointOfCommunication: pointOfCommunication, dataModel: JSON.stringify(dataModelPassed)
    };
    return ExecutePostAjax("../Workflow/Index", data);
}
function ApplicationRouting(pointOfCommunication, dataModelPassed) {
    var data = {
        module: 'APPLN', pointOfCommunication: pointOfCommunication, dataModel: JSON.stringify(dataModelPassed)
    };
    return ExecutePostAjax("../Workflow/Index", data);
}
function ExecutePostAjax(urlToExecute, dataToPass) {
    var routeUrl = "NOROUTE";
    $.ajax({
        async: false,
        url: urlToExecute,
        type: 'POST',
        data: dataToPass,
        beforeSend: function () {
            /*startAnimation();*/
        },
        success: function (result) {
            //console.log(result);
            /*stopAnimation();*/
            routeUrl = result;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            stopAnimation();
            console.log(jqXHR);
            console.log(textStatus);
            console.log(errorThrown);
        }
    });
    return routeUrl;
}