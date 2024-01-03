    $(document).ready(function () {
        
        $("#IDCreateRoute").on('click', CreateRoute);
        $(".EditRouteCLS").on('click', EditRoute1);
        $(".AddRouteLibCls").on('click', AddRouteToLibrary1);
        $(".DeleteRouteCls").on('click', DeleteRoute1);
        $(".ViewNotifgareedRoutesCls").on('click', ViewNotifgareedRoutes1);
    });
    function EditRoute1(){
        var r1 = e.currentTarget.dataset.DataRouteID;
        var r2 = e.currentTarget.dataset.DataRouteName;
        var r3 = e.currentTarget.dataset.DataRouteType;
        EditRoute(r1,r2,r3);
    }
    function ViewNotifgareedRoutes1() {
        var r1 = e.currentTarget.dataset.DataRouteID3;
        var r2 = e.currentTarget.dataset.dataRouteType3;
        var r3 = e.currentTarget.dataset.DataRtName3;
        ViewNotifgareedRoutes(r1,r3);
    }
    function DeleteRoute1() {
        var r1 = e.currentTarget.dataset.DataRouteID2;
        var r2 = e.currentTarget.dataset.dataRouteType2;
        var r3 = e.currentTarget.dataset.DataRtName2;
        DeleteRoute(r1, r2, r3);
    }
    function AddRouteToLibrary1() {
        var r1 = e.currentTarget.dataset.DataRouteID1;
        var r2 = e.currentTarget.dataset.dataRouteType1;
        var r3 = e.currentTarget.dataset.DataRtName;
        AddRouteToLibrary(r1, r2, r3);
    }
