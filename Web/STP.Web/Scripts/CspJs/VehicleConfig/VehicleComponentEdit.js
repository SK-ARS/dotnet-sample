    $(document).ready(function () {
        
        vehicleId  = $('#hf_VehicleConfigId').val(); 
        var weight;
        $(".comp").each(function () {
            var compId = $(this).attr('id');
            weight = $('#' + compId).find('#Weight').val();

            if (weight == "") {
                weight = null;
            }
            $.ajax({
                url: '../Vehicle/AxleValidationCalculation',
                type: 'POST',
                data: { "weight": weight },
                success: function (response) {
                    $('#' + compId).find('#axleweightRange').val(response);
                },
                error: function (response) {

                }
            });
        });
        $('#btn_comp_finish').click(function () {

            var componentId = $('#Component_Id').val();

            var unit = $('#UnitValue').val();
            var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
            var isVR1 = $('#vr1appln').val();
            var isNotify = $('#ISNotif').val();
            if (isNotify == 'True' || isNotify == 'true') {
                isVR1 = 'True';
            }
            var vd = validation();

            if (vd) {


                    if (componentId == '') {
                        SaveComponent(this);

                    }
                    else {
                        UpdateData(this);

                    }
                //}

                // call registration function
            }
            return false;
        });


        $(".axledrop").change(function () {
            
            var div = $(this).parent().closest('.comp').attr('id');
            $("#btn_cancel").show();
            $('#componentBtn').show();
            var numberOfAxles = $(this).parent().closest('#div_component_general_page').find('.axledrop').val();
            var configurableAxles = $(this).parent().closest('#div_component_general_page').find('.AxleConfig').val();
            if (configurableAxles == 'True') {
                var compWeight = $('#div_general').find('#Weight').val();
                var componentId = $('#Component_Id').val();
                var isFromConfig = $('#HiddenFromConfig').val();

                var componentTypeId = parseInt($('#vehicleTypeValue').val());
                var compSubId = parseInt($('#vehicleSubTypeValue').val());
                var movementId = $('#movementTypeId').val();

                if (componentId == "") { componentId = 0; }
                if (componentTypeId == null) { componentTypeId = 0; }
                if (compSubId == null) { compSubId = 0; }

                var isEdit = $('#IsEdit').val();
                if (isEdit == '') {
                    isEdit = false;
                }
                var data = {
                    axleCount: numberOfAxles, componentId: componentId,
                    vehicleSubTypeId: compSubId, vehicleTypeId: componentTypeId, movementId: movementId,
                    weight: compWeight, IsEdit: isEdit, IsFromConfig: isFromConfig
                };
                var url = '../Vehicle/AxleComponent';
                if (@IsFromConfig== 1) {
                    url = '../VehicleConfig/AxleComponent';
                }
                $.ajax({
                    url: url,
                    data: data,
                    type: 'GET',
                    //contentType: 'application/json; charset=utf-8',
                    async: false,
                    success: function (response) {
                        $('#'+div+' #axlePage').html(response);
                        HeaderHeight();
                        $('.dyntitle').text('Edit axle');
                    },
                    error: function () {

                    },
                    complete: function () {

                    }
                });
            }
        });

        $(document).ready(function () {
            $(".pageKeyUpValidation").keyup(function () {
                keyUpValidationFn(this);
            });
            $(".pageKeyUpSpeedValidation").keyup(function () {
                validationSpeedFn(this);
            });
            
        });
    });


    $(function () {
        $('#Weight').change(function () {
            var weight = $('#Weight').val();
            if (weight == "") {
                weight = null;
            }
            $.ajax({
                url: '../Vehicle/AxleValidationCalculation',
                type: 'POST',
                data: { "weight": weight },
                success: function (response) {
                    $('#axleweightRange').val(response) ;
                },
                error: function (response) {

                }
            });
        })
    });

    function keyUpValidationFn(e) {
        keyUpValidation(e);
    }
    function validationSpeedFn(e) {
        validationSpeed(e);
    }
