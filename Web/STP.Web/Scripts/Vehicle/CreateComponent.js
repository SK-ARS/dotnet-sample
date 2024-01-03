$(document).ready(function () {
    SelectMenu(5);
    $('body').on('change', '#ComponentType', function (e) {
        e.preventDefault();
        VehicleTypeChange(this);
    });
    $('body').on('change', '#VehicleSubType', function (e) {
        e.preventDefault();
        VehicleSubTypeChange(this);
    });
    $('body').on('click', '#movementTypeId', function (e) {
        e.preventDefault();
        VehicleClassificationChange(this);
    });
});