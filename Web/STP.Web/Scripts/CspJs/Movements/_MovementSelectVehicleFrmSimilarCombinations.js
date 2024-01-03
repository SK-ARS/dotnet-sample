    var viewVehicleFlag = false;
    $(document).ready(function () {
        StepFlag = 1;
        SubStepFlag = 1.2;
        CurrentStep = "Select Vehicle";
        SetWorkflowProgress(1);
        $('#confirm_btn').hide();

        $('#RearOverhang').attr('disabled', true);
        $('#LeftOverhang').attr('disabled', true);
        $('#FrontOverhang').attr('disabled', true);
        $('#RightOverhang').attr('disabled', true);

        $('div#edit_btn_cntr').hide();
        $('div#confirm_btn_cntr').hide();
    });

    $("input[type=text]").change(function () {
        GetSimilarVehicles();
    });

    function GetSimilarVehicles() {
        var searchFlag = false;
        $("input[type=text]").each(function () {
            if ($(this).val()) {
                searchFlag = true;
            }
        });

        if (searchFlag) {
            $.ajax({
                url: '../Movements/GetFilteredCombinations',
                type: 'POST',
                cache: false,
                async: false,
                data: $("#VehicleConfigInfo").serialize(),
                beforeSend: function () {
                    startAnimation();
                },
                success: function (response) {
                    $('div#similar_vehicles_contnr').html(response);
                },
                complete: function () {
                    stopAnimation();
                }
            });
        }
    }

    function OnBackBtnClick() {
        if (viewVehicleFlag) {
            ShowSearchedCombinations()
        }
        else {
            @*if (@SelectedVehicles.Count > 0) {
                $('#select_vehicle_section').hide();
                $('#vehicle_details_section').show();
            }
            else {
                window.location = '../Movements/PlanMovement';
            }*@            
        }
    }

    function ShowSearchedCombinations() {
        viewVehicleFlag = false;
        $('div#vehicles').children().not(':first-child').remove();
        $(combinationLstCln).appendTo('div#vehicles');
    }

    function EditVehicle() {
        window.location = '../VehicleConfig/EditConfiguration' = EncodedQueryString('vehicleId=' + vehicleIdGbl);
    }
