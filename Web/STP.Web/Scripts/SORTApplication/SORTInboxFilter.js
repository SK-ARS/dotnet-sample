
var DateTime = $('#hf_DateTime').val();
$(document).ready(function () {
    if (mapaddresssearch != undefined && mapaddresssearch != "") {
        $("#txtAddressSearch").val(mapaddresssearch);
    }

    $("#SOSignFromDate").datepicker({
        dateFormat: "dd-M-yy",
        changeYear: true,
        changeMonth: true,
        onSelect: function (selected) {
            var dt = new Date(selected);
            var endDate = $("#SOSignToDate").datepicker('getDate');
            dt.setDate(dt.getDate() + 1);
            $("#SOSignToDate").datepicker("option", "minDate", dt);
            if (dt > endDate) {
                $("#SOSignToDate").datepicker("setDate", dt);
            }
        }
    });
    $("#SOSignToDate").datepicker({
        dateFormat: "dd-M-yy",
        changeYear: true,
        changeMonth: true,
        onSelect: function (selected) {
            var dt = new Date(selected);
            dt.setDate(dt.getDate() - 1);
            $("#SOSignFromDate").datepicker("option", "maxDate", dt);
        }
    });

    $("#ApplicationFromDate").datepicker({
        dateFormat: "dd-M-yy",
        changeYear: true,
        changeMonth: true,
        onSelect: function (selected) {
            var dt = new Date(selected);
            var endDate = $("#ApplicationToDate").datepicker('getDate');
            dt.setDate(dt.getDate() + 1);
            $("#ApplicationToDate").datepicker("option", "minDate", dt);
            if (dt > endDate) {
                $("#ApplicationToDate").datepicker("setDate", dt);
            }
        }
    });
    $("#ApplicationToDate").datepicker({
        dateFormat: "dd-M-yy",
        changeYear: true,
        changeMonth: true,
        onSelect: function (selected) {
            var dt = new Date(selected);
            dt.setDate(dt.getDate() - 1);
            $("#ApplicationFromDate").datepicker("option", "maxDate", dt);
        }
    });

    $("#MovFromDate").datepicker({
        dateFormat: "dd-M-yy",
        changeYear: true,
        changeMonth: true,
        onSelect: function (selected) {
            var dt = new Date(selected);
            var endDate = $("#MovToDate").datepicker('getDate');
            dt.setDate(dt.getDate() + 1);
            $("#MovToDate").datepicker("option", "minDate", dt);
            if (dt > endDate) {
                $("#MovToDate").datepicker("setDate", dt);
            }
        }
    });
    $("#MovToDate").datepicker({
        dateFormat: "dd-M-yy",
        changeYear: true,
        changeMonth: true,
        onSelect: function (selected) {
            var dt = new Date(selected);
            dt.setDate(dt.getDate() - 1);
            $("#MovFromDate").datepicker("option", "maxDate", dt);
        }
    });

    $("#WorkDueFromDate").datepicker({
        dateFormat: "dd-M-yy",
        changeYear: true,
        changeMonth: true,
        onSelect: function (selected) {
            var dt = new Date(selected);
            var endDate = $("#WorkDueToDate").datepicker('getDate');
            dt.setDate(dt.getDate() + 1);
            $("#WorkDueToDate").datepicker("option", "minDate", dt);
            if (dt > endDate) {
                $("#WorkDueToDate").datepicker("setDate", dt);
            }
        }
    });
    $("#WorkDueToDate").datepicker({
        dateFormat: "dd-M-yy",
        changeYear: true,
        changeMonth: true,
        onSelect: function (selected) {
            var dt = new Date(selected);
            dt.setDate(dt.getDate() - 1);
            $("#WorkDueFromDate").datepicker("option", "maxDate", dt);
        }
    });
});
function EnableDateFields() {
    $('#viewotheroptions input:checkbox').on('click', function () {
        togglecheckbox($(this));
    });
}
$(function () {
    //EnableDateFields();
    EnableDisableDatePicker();
});
$('#SOSignDateCheck').click(function () {
    if ($('#SOSignDateCheck').is(':checked')) {
        $('#SOSignFromDate').attr('disabled', false);
        $('#SOSignToDate').attr('disabled', false);
        $('#SOSignFromDate').val(DateTime);
        $('#SOSignToDate').val(DateTime);
    } else {
        $('#SOSignFromDate').attr('disabled', 'disabled');
        $('#SOSignFromDate').val('');
        $('#SOSignToDate').attr('disabled', 'disabled');
        $('#SOSignToDate').val('');
    }
});
$('#ApplicationDateCheck').click(function () {
    if ($('#ApplicationDateCheck').is(':checked')) {
        $('#ApplicationFromDate').attr('disabled', false);
        $('#ApplicationToDate').attr('disabled', false);
        $('#ApplicationFromDate').val(DateTime);
        $('#ApplicationToDate').val(DateTime);
    } else {
        $('#ApplicationFromDate').attr('disabled', 'disabled');
        $('#ApplicationFromDate').val('');
        $('#ApplicationToDate').attr('disabled', 'disabled');
        $('#ApplicationToDate').val('');
    }
});
$('#MovDateCheck').click(function () {
    if ($('#MovDateCheck').is(':checked')) {
        $('#MovFromDate').attr('disabled', false);
        $('#MovToDate').attr('disabled', false);
        $('#MovFromDate').val(DateTime);
        $('#MovToDate').val(DateTime);
    } else {
        $('#MovFromDate').attr('disabled', 'disabled');
        $('#MovFromDate').val('');
        $('#MovToDate').attr('disabled', 'disabled');
        $('#MovToDate').val('');
    }
});
$('#WorkDueDateCheck').click(function () {
    if ($('#WorkDueDateCheck').is(':checked')) {
        $('#WorkDueFromDate').attr('disabled', false);
        $('#WorkDueToDate').attr('disabled', false);
        $('#WorkDueFromDate').val(DateTime);
        $('#WorkDueToDate').val(DateTime);
    } else {
        $('#WorkDueFromDate').attr('disabled', 'disabled');
        $('#WorkDueFromDate').val('');
        $('#WorkDueToDate').attr('disabled', 'disabled');
        $('#WorkDueToDate').val('');
    }
});
