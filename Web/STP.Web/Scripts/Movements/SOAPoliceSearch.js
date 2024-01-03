var DateTimeVal = $('#hf_DateTime').val();
$(document).ready(function () {
    $("#MovementFromDate").datepicker({
        dateFormat: "dd-M-yy",
        changeYear: true,
        changeMonth: true,
        onSelect: function (selected) {
            var dt = new Date(selected);
            var endDate = $("#MovementToDate").datepicker('getDate');
            dt.setDate(dt.getDate());
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
            dt.setDate(dt.getDate());
            //$("#MovementFromDate").datepicker("option", "maxDate", dt);
        }
    });

    $("#FromReceiptDateOfCommn").datepicker({
        dateFormat: "dd-M-yy",
        changeYear: true,
        changeMonth: true,
        onSelect: function (selected) {
            var dt = new Date(selected);
            var endDate = $("#ToReceiptDateOfCommn").datepicker('getDate');
            dt.setDate(dt.getDate());
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
            dt.setDate(dt.getDate());
            //$("#FromReceiptDateOfCommn").datepicker("option", "maxDate", dt);
        }
    });
    $('#UnderAssessmentbyOtherUser').change(function () {
        if ($('#UnderAssessmentbyOtherUser').is(':checked')) {
            $('#UserID').removeAttr('disabled');
        }
        else {
            $('#UserID')[0].selectedIndex = 0;
            $('#UserID').attr('disabled', 'disabled');
        }
    });

});
$('#MovementDate').click(function () {
    if ($('#MovementDate').is(':checked')) {
        $('#MovementFromDate').attr('disabled', false);
        $('#MovementToDate').attr('disabled', false);
        $('#MovementFromDate').val(DateTimeVal);
        $('#MovementToDate').val(DateTimeVal);
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
        $('#FromReceiptDateOfCommn').val(DateTimeVal);
        $('#ToReceiptDateOfCommn').val(DateTimeVal);
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
