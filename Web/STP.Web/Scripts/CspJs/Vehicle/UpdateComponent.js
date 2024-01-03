    $(document).ready(function () {
        $('#btn_comp_edit').click(function () {
            
            var componentId = $('#Component_Id').val();

            var component = $('#' + componentId);
            var unit = $('#UnitValue').val();
            var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
            var isVR1 = $('#vr1appln').val();
            var isNotify = $('#ISNotif').val();
            if (isNotify == 'True' || isNotify == 'true') {
                isVR1 = 'True';
            }
            var vd = validation(componentId);

            if (vd) {
                if (ApplicationRevId != 0 && isVR1 == 'False') {
                    if (componentId == '') {

                        Saveappvehcomponent(this);

                    }
                    else {
                        //method
                        Updateappvehcomponent(this);
                        //  UpdateData(this);
                    }

                }
                else if (isVR1 == 'True' || isVR1 == 'true') {
                    if (componentId == '') {

                        SaveVR1vehcomponent(this);

                    }
                    else {

                        UpdateVR1vehcomponent(this);
                    }
                }
                else {

                    if (componentId == '') {
                        SaveComponent(this);

                    }
                    else {
                        UpdateData(component);

                    }
                }

            }
            return false;
        });
    });
    $(function () {
        $(".axledrop").change(function () {
          
            $("#btn_cancel").show();
            $('#componentBtn').show();
            var numberOfAxles = $('#div_component_general_page').find('.axledrop').val();
            var configurableAxles = $('#div_component_general_page').find('.AxleConfig').val();
            if (configurableAxles == 'True') {

                loadAxles(numberOfAxles);

            }
        });
    });
