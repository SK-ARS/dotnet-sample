    let combinationLstCln;
    var vehicleIdGbl;
    function GetVehicleDetails(vehicleId) {
        vehicleIdGbl = vehicleId;
        $.ajax({
            url: '../VehicleConfig/ViewConfiguration',
            type: 'POST',
            cache: false,
            async: false,
            data: { vehicleID: vehicleId },
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {
                viewVehicleFlag = true;
                combinationLstCln = $("div[id^='combinationlst_cntr']").clone(true, true);
                let pageBottomtCln = $("div[id^='page-bottom']").clone(true, true);

                $('div#combinationlst_cntr').hide();

                var result = $(response).find('div#vehicles').html();
                $(result).appendTo('div#vehicles');
                $(pageBottomtCln).appendTo('div#vehicles');
                $('div#edit_btn_cntr').show();
                $('div#confirm_btn_cntr').show();
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    $(document).ready(function () {
        $('body').on('click','.btn_createnewcombination', function() { window['CreateNewConfiguration'](); });

    });
    $('body').on('click', '.div_vehiclefoemovement', function (e) {
         var ConfigurationId = $(this).data("configurationid");
        var flag = $(this).data("flag");
     

         UseVehicleForMovement(ConfigurationId, flag);
    });
