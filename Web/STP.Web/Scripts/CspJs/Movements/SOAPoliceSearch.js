    $(document).ready(function () {
   
        $("#MovementFromDate").datepicker({
            dateFormat: "dd-M-yy",
            changeYear: true,
            changeMonth: true,
            onSelect: function (selected) {
                ;
                var dt = new Date(selected);
                var endDate = $("#MovementToDate").datepicker('getDate');
                dt.setDate(dt.getDate() + 1);
                $("#MovementToDate").datepicker("option", "minDate", dt);
                if (dt > endDate) {
                    $("#MovementToDate").datepicker("setDate", dt);
                }
            }
        });
        $("#MovementToDate").datepicker({
            dateFormat: "dd-M-yy",
            changeYear: true,
            changeMonth: true,
            onSelect: function (selected) {
                var dt = new Date(selected);
                dt.setDate(dt.getDate() - 1);
                $("#MovementFromDate").datepicker("option", "maxDate", dt);
            }
        });

        $("#FromReceiptDateOfCommn").datepicker({
            dateFormat: "dd-M-yy",
            changeYear: true,
            changeMonth: true,
            onSelect: function (selected) {
                var dt = new Date(selected);
                var endDate = $("#ToReceiptDateOfCommn").datepicker('getDate');
                dt.setDate(dt.getDate() + 1);
                $("#ToReceiptDateOfCommn").datepicker("option", "minDate", dt);
                if (dt > endDate) {
                    $("#ToReceiptDateOfCommn").datepicker("setDate", dt);
                }
            }
        });
        $("#ToReceiptDateOfCommn").datepicker({
            dateFormat: "dd-M-yy",
            changeYear: true,
            changeMonth: true,
            onSelect: function (selected) {
                var dt = new Date(selected);
                dt.setDate(dt.getDate() - 1);
                $("#FromReceiptDateOfCommn").datepicker("option", "maxDate", dt);
            }
        });
        
           
        $('#UnderAssessmentbyOtherUser').change(function () {
       
            if ($('#UnderAssessmentbyOtherUser').is(':checked')) {
                $('#DDselectotheruser').removeAttr('disabled');
            }
            else {
                $('#DDselectotheruser')[0].selectedIndex = 0;
                $('#DDselectotheruser').attr('disabled', 'disabled');
            }
        });

    });
    $('#MovementDate').click(function () {
    if ($('#MovementDate').is(':checked')) {
        $('#MovementFromDate').attr('disabled', false);
        $('#MovementToDate').attr('disabled', false);
        $('#MovementFromDate').val('@DateTime.Now.ToString("dd-MMM-yyyy")');
        $('#MovementToDate').val('@DateTime.Now.ToString("dd-MMM-yyyy")');
    } else {
        $('#MovementFromDate').attr('disabled', 'disabled');
        $('#MovementFromDate').val('');
        $('#MovementToDate').attr('disabled', 'disabled');
        $('#MovementToDate').val('');
    }
        });
    $('#ReceiveDate').click(function () {
    if ($('#ReceiveDate').is(':checked')) {
        $('#FromReceiptDateOfCommn').attr('disabled', false);
        $('#ToReceiptDateOfCommn').attr('disabled', false);
        $('#FromReceiptDateOfCommn').val('@DateTime.Now.ToString("dd-MMM-yyyy")');
        $('#ToReceiptDateOfCommn').val('@DateTime.Now.ToString("dd-MMM-yyyy")');
    } else {
        $('#FromReceiptDateOfCommn').attr('disabled', 'disabled');
        $('#FromReceiptDateOfCommn').val('');
        $('#ToReceiptDateOfCommn').attr('disabled', 'disabled');
        $('#ToReceiptDateOfCommn').val('');
    }
    });
    function togglecheckbox(_this) {
        if (_this.is(':checked')) {
            _this.closest('.row').find('input:text').attr('disabled', false);
        }
        else {
            _this.closest('.row').find('input:text').attr('disabled', true);
            _this.closest('.row').find('input:text').val("");
        }
    }
    function EnableDisableDatePicker() {
        $.each($("#viewMovement input:checkbox"), function () {
            togglecheckbox($(this));
        });
    }
    $(function () {
        EnableDisableDatePicker();
    });
