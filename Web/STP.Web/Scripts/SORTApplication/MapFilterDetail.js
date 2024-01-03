$(document).ready(function () {
    $("#map").html('');
    $("#map").load('../Routes/A2BPlanning', {routeID : 0}, function () {
        loadmap('SORTMAPFILTER_VIEWANDEDIT');
    });
});
