    @if (@VSONotificationVehicle) {
        @:MovementAssessment();
    }
    else{
        @:$("#div_config_general input[type=text]").not('textarea,#Internal_Name,#Formal_Name').keyup(function () {
            @:MovementAssessment();
        @:});
        @*@:$('.axledropVehicle').change(function () {
            @:MovementAssessment();
        @:});*@
    }
    var confirmAndContinueButton = $("#vehicle_movement_confirm_btn");
    confirmAndContinueButton.on("click", SaveVehicleConfigurationV1);
    $(document).ready(function () {
        var isEdit = $("#IsVehicleConfigEdit").val();
        if (isEdit == 1) {
            MovementAssessment();
        }
        $(".clearValidation").keyup(function () {
            ClearValidationMessageFn(this);
        });
    });

    function ClearValidationMessageFn(e) {
        ClearValidationMessage(e);
        }
