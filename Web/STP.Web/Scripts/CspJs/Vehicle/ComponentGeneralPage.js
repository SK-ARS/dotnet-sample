    $(document).ready(function () {
        vehicleId  = $('#hf_VehicleConfigId').val(); 


        $('#btn_comp_finish').click(function () {           
            startAnimation();
            var componentId = $('#Component_Id').val();
            var unit = $('#UnitValue').val();
            var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
            var isVR1 = $('#vr1appln').val();
            var isNotify = $('#ISNotif').val();
            if (isNotify == 'True' || isNotify == 'true') {
                isVR1 = 'True';
            }
            var vd = validation();
            //validateTable(this, vd);//isTableValid variable will set on this call
            if (vd && isTableValid) {
                if (componentId == '') {
                    SaveComponent(this);
                }
                else {
                    UpdateData(this);
                }               
            }
            else { stopAnimation(); }
            return false;
        });

        var isTableValid = true;
        function validateTable(_this,vd) {
            isTableValid = true;
            var form = $(_this).closest("form");
            var tablevehicletable = form.find('#vehicle-table.RegistrationComponent');
            if (tablevehicletable.length > 0) {
                console.log('tablevehicletable', true);
                var allTrs = $("#vehicle-table.RegistrationComponent > tbody > tr");
                var registrationDetails = [];
                allTrs.each(function (index, value) {//loop thorugh all tbody>tr items and insert all itemsinto an array
                    var isLastElement = index == allTrs.length - 1;
                    if (!isLastElement) {
                        var regIdComp = $(this).find('td span.cls_regId_config').length > 0 ? $(this).find('td span.cls_regId_config') : $(this).find('td.cls_regId');
                        var fleetIdComp = $(this).find('td span.cls_fleetId_config').length > 0 ? $(this).find('td span.cls_regId_config') : $(this).find('td.cls_fleetId');
                        var regId = regIdComp.text();
                        var fleetId = fleetIdComp.text();
                        var id = $(this).find('td .hdId').length > 0 ? $(this).find('td .hdId').val() : 0;
                        if (regId || fleetId) {
                            var obj = { RegId: regId, FleetId: fleetId, Id: id };
                            registrationDetails.push(obj);
                        }
                    }
                }).promise().done(function () { // after loop through all tbody tr items, we need to check the table has atleast one entry
                    console.log('registrationDetails', registrationDetails);
                    if (registrationDetails.length == 0) {
                        form.find('#error_msg').text('Please enter atleast one registration plate or fleet id.');
                        setTimeout(function () {
                            form.find('#error_msg').text('');
                        }, 5000);
                        isTableValid= false;
                    }
                });
            }
        }
    });

    $(function () {
        $(".axledrop").change(function () {

            $("#btn_cancel").show();
            $('#componentBtn').show();
            //$("#btn_cancel_axle").show();
            //$('#btn_cancel_axle').css('display', 'block');
            var numberOfAxles = $('#div_component_general_page').find('.axledrop').val();
            var configurableAxles = $('#div_component_general_page').find('.AxleConfig').val();
            if (configurableAxles == 'True') {

                loadAxles(numberOfAxles);

                //var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
                //var isVR1 = $('#vr1appln').val();
                //var isNotify = $('#ISNotif').val();

                //if (isNotify == 'True' || isNotify == 'true') {
                //    loadVR1vehAxles(numberOfAxles);
                //}
                //else if (ApplicationRevId == 0 && !isVR1) {
                //    loadAxles(numberOfAxles);
                //}
                //else if (isVR1 == 'True') {
                //    loadVR1vehAxles(numberOfAxles);
                //}
                //else {
                //    loadAppvehAxles(numberOfAxles);
                //}
            }
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

