    $(document).ready(function () {
        //ShowVehicleTab();
        $("#HaulierEmailAddress").val('@ViewBag.HauliEmail');

    });
        $("#verifyRoute").click(function () {
            $("#generalTab").removeClass('active-card');
            $("#routeTab").addClass('active-card');
        });
