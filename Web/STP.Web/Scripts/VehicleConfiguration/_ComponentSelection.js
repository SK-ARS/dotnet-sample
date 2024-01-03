function ComponentSelectionInit() {
    FillComponentFav();
}
$(document).ready(function () {
    $('body').on('click', '.a_createComponent', function (e) {
        e.preventDefault();
        var isComponentExist = $('.CreateComponent').length > 0||false;
        CreateComponentForVehicle(undefined,isComponentExist);
    });
    $('body').on('click', '.a_selectComponent', function (e) {
        e.preventDefault();
        SelectVehiclecomponentFromFleet(this);
    });
    $('body').on('click', '#BoatMastException', function (e) {
        //e.preventDefault();
        AssessConfigType(this);
    });
}); 