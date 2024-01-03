    $(document).ready(function () {
        $("#Gross_text").keypress(function (evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
        });
        $("#Axle_text").keypress(function (evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
        });
        $("#Width_text").keypress(function (evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
        });
        $("#Height_text").keypress(function (evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
        });
        $("#Length_text").keypress(function (evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
        });

        //$("#Gross_text,#Axle_text,#Length_text,#Width_text,#Height_text").live("keypress", function (event) {
        //    if (event.which == 13) {
        //        SaveVehicleRestrications();
        //    }
        //});

        $("#Gross_text").change(function (e) {
            var a = $('#Gross_text').val();
            if (a > 99999999) {
                $('#errorChk').text("Gross weight exceeds the maximum allowed weight 99999999 kg");
                $('#Axle_text').attr('readonly', true);
                $('#Height_text').attr('readonly', true);
                $('#Width_text').attr('readonly', true);
                $('#Length_text').attr('readonly', true);
                $('#btn_update').attr('disabled', true);
                flag = 1;
            }
            else {
                $('#errorChk').text("");
                $('#Axle_text').attr('readonly', false);
                $('#Height_text').attr('readonly', false);
                $('#Width_text').attr('readonly', false);
                $('#Length_text').attr('readonly', false);
                $('#btn_update').attr('disabled', false);
                flag = 0;
            }
        });
        $("#Axle_text").change(function (e) {
            var a = $('#Axle_text').val();
            if (a > 99999999) {
                $('#errorChk').text("Axle weight exceeds the maximum allowed weight 99999999 kg");
                $('#Gross_text').attr('readonly', true);
                $('#Height_text').attr('readonly', true);
                $('#Width_text').attr('readonly', true);
                $('#Length_text').attr('readonly', true);
                $('#btn_update').attr('disabled', true);
                flag = 1;
            }
            else {
                $('#errorChk').text("");
                $('#Gross_text').attr('readonly', false);
                $('#Height_text').attr('readonly', false);
                $('#Width_text').attr('readonly', false);
                $('#Length_text').attr('readonly', false);
                $('#btn_update').attr('disabled', false);
                flag = 0;
            }
        });
        $("#Length_text").change(function (e) {
            var len = $("#Length_text").val();
            if (len > 999.99) {
                $('#errorChk').text("Length exceeds the maximum allowed length 999.999 m");
                $('#Axle_text').attr('readonly', true);
                $('#Height_text').attr('readonly', true);
                $('#Width_text').attr('readonly', true);
                $('#Gross_text').attr('readonly', true);
                $('#btn_update').attr('disabled', true);
                flag = 1;
            }
            else {
                $('#errorChk').text("");
                $('#Axle_text').attr('readonly', false);
                $('#Height_text').attr('readonly', false);
                $('#Width_text').attr('readonly', false);
                $('#Gross_text').attr('readonly', false);
                $('#btn_update').attr('disabled', false);
                flag = 0;
            }
        });
        $("#Width_text").change(function (e) {
            var len = $("#Width_text").val();
            if (len > 999.99) {
                $('#errorChk').text("Width exceeds the maximum allowed width 999.999 m");
                $('#Axle_text').attr('readonly', true);
                $('#Height_text').attr('readonly', true);
                $('#Length_text').attr('readonly', true);
                $('#Gross_text').attr('readonly', true);
                $('#btn_update').attr('disabled', true);
                flag = 1;
            }
            else {
                $('#errorChk').text("");
                $('#Axle_text').attr('readonly', false);
                $('#Height_text').attr('readonly', false);
                $('#Length_text').attr('readonly', false);
                $('#Gross_text').attr('readonly', false);
                $('#btn_update').attr('disabled', false);
                flag = 0;
            }
        });
        $("#Height_text").change(function (e) {
            var len = $("#Height_text").val();
            if (len > 999.99) {
                $('#errorChk').text("Height exceeds the maximum allowed Height 999.999 m");
                $('#Axle_text').attr('readonly', true);
                $('#Width_text').attr('readonly', true);
                $('#Length_text').attr('readonly', true);
                $('#Gross_text').attr('readonly', true);
                $('#btn_update').attr('disabled', true);
                flag = 1;
            }
            else {
                $('#errorChk').text("");
                $('#Axle_text').attr('readonly', false);
                $('#Width_text').attr('readonly', false);
                $('#Length_text').attr('readonly', false);
                $('#Gross_text').attr('readonly', false);
                $('#btn_update').attr('disabled', false);
                flag = 0;
            }
        });
    });
