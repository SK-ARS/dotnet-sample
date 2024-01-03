        $(document).ready(function () {            
            $("#map").html('');
            $("#map").load('@Url.Action("A2BPlanning", "Routes", new {routeID = 0})', function () {
                loadmap('SORTMAPFILTER_VIEWANDEDIT');              
            });
        });
