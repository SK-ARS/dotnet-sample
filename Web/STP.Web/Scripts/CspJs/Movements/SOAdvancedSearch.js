    $(document).ready(function () {
        $(".gross_weight1").hide();

         $("#MovementFromDate").datepicker({
             dateFormat: "dd-M-yy",
             changeYear: true,
             changeMonth: true,
             onSelect: function (selected) {
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

         $("#NotificationFromDate").datepicker({
             dateFormat: "dd-M-yy",
             changeYear: true,
             changeMonth: true,
             onSelect: function (selected) {
                 var dt = new Date(selected);
                 var endDate = $("#NotificationToDate").datepicker('getDate');
                 dt.setDate(dt.getDate() + 1);
                 $("#NotificationToDate").datepicker("option", "minDate", dt);
                 if (dt > endDate) {
                     $("#NotificationToDate").datepicker("setDate", dt);
                 }
             }
         });
         $("#NotificationToDate").datepicker({
             dateFormat: "dd-M-yy",
             changeYear: true,
             changeMonth: true,
             onSelect: function (selected) {
                 var dt = new Date(selected);
                 dt.setDate(dt.getDate() - 1);
                 $("#NotificationFromDate").datepicker("option", "maxDate", dt);
             }
         });
        $('body').on('click','.viewAdvhaulier', function() { window['viewAdvHaulier'](); }); 
        $('body').on('dragend', '.folder-item', function (e) {
            dragEnd(e);
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
    $('#ApplicationDate').click(function () {
        if ($('#ApplicationDate').is(':checked')) {
            $('#ApplicationFromDate').attr('disabled', false);
            $('#ApplicationToDate').attr('disabled', false);
            $('#ApplicationFromDate').val('@DateTime.Now.ToString("dd-MMM-yyyy")');
            $('#ApplicationToDate').val('@DateTime.Now.ToString("dd-MMM-yyyy")');
        } else {
            $('#ApplicationFromDate').attr('disabled', 'disabled');
            $('#ApplicationFromDate').val('');
            $('#ApplicationToDate').attr('disabled', 'disabled');
            $('#ApplicationToDate').val('');
        }
    });
    $('#NotifyDate').click(function () {
        if ($('#NotifyDate').is(':checked')) {
            $('#NotificationFromDate').attr('disabled', false);
            $('#NotificationToDate').attr('disabled', false);
            $('#NotificationFromDate').val('@DateTime.Now.ToString("dd-MMM-yyyy")');
            $('#NotificationToDate').val('@DateTime.Now.ToString("dd-MMM-yyyy")');
        } else {
            $('#NotificationFromDate').attr('disabled', 'disabled');
            $('#NotificationFromDate').val('');
            $('#NotificationToDate').attr('disabled', 'disabled');
            $('#NotificationToDate').val('');
        }
    });
    

    //function EnableDateFields() {
    //    $('#viewAdvHaulier input:checkbox').on('click', function () {
    //        if ($(this).is(':checked')) {
    //            $(this).closest('.row').find('input:text').attr('disabled', false);
    //        }
    //        else {
    //            $(this).closest('.row').find('input:text').attr('disabled', true);
    //            $(this).closest('.row').find('input:text').val("");
    //            $(this).closest('.row').find('.field-validation-error').html("");
    //        }
    //    });
    //}
    function AllowNumericOnly() {
        // isnumeric
        $('.isnumeric').keypress(function (evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
        });
    }

    $(function () {
        //EnableDateFields();
        AllowNumericOnly();
    });

