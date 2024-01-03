
    $(document).ready(function () {
        debugger
        var x = $('#current_step').text();
        if (x == "Vehicle Details") {
            $('.effe').hide();
        }
        if (StepFlag == 3) {
            $('#movementGeneralDetails').show();
            var x = $('#current_step').text();

            if (x == "Movement Type") {
                $('.hiding').hide();
                $('.eff').show();
                if ("@isSpecialOrderOrVr1"=="True") {
                    $('.eff').hide();
                } 
            }

            var isSort = $('#IsSortUser').val();
            if (isSort == 'True') {
                $("#allocate_btn").show();
                $("#notifradio").hide();
                var allocate = $('#movement_type_confirmation').find('#movementGeneralDetails').find('#Revise_AllocateUserId');

                var allocate_id = allocate.val();
                if (allocate_id != '' && allocate_id != null && allocate_id != '0') {

                    startAnimation();
                    $('#AllocateUser').load('../SORTApplication/SORTAllocateUser', { pln_user_id: allocate_id },
                        function () {
                            $("#overlay").hide();
                            $('#AllocateUser').find('#table-head').remove();
                            $('#AllocateUser').find('.pb-3').remove();
                            $('#AllocateUser').show();
                            stopAnimation();
                        });
                }
            }

        }
        else {
            $('#movementGeneralDetails').html('');
            $('#movementGeneralDetails').hide();
            $("#allocate_btn").hide();
        }
        $("#FromDate").datepicker({
            dateFormat: "dd-mm-yy",
            changeYear: true,
            changeMonth: true,
            minDate: new Date(),
            onSelect: function (selected) {
                var frmdt = selected.split("-").reverse().join("/");
                var dt = new Date(frmdt);
                var endDate = $("#ToDate").val();
                var dtnewdate1 = endDate.split("-").reverse().join("/");
                dt.setDate(dt.getDate());// + 1
                //  $("#ToDate").datepicker("option", "minDate", dt || 1);
                if (dt > new Date(dtnewdate1)) {
                    $("#ToDate").datepicker("setDate", dt);
                }
            },
            beforeShow: function (textbox, instance) {
                var date = $("#ToDate").datepicker('getDate');
                if (date) {
                    date.setDate(date.getDate()); //- 1
                }
                // $('#FromDate').datepicker('option', 'maxDate', date);
                var rect = textbox.getBoundingClientRect();
                setTimeout(function () {
                    var scrollTop = $("body").scrollTop();
                    instance.dpDiv.css({ top: rect.top + textbox.offsetHeight + scrollTop });
                }, 0);
            }
        });
        $("#ToDate").datepicker({
            dateFormat: "dd-mm-yy",
            changeYear: true,
            changeMonth: true,
            minDate: new Date(),
            onSelect: function (selected) {

                var todate = selected.split("-").reverse().join("/");
                var dt = new Date(todate);
                dt.setDate(dt.getDate()); //+ 1
                $('#FromDate').datepicker("setDate", new Date(dt));
                //$("#FromDate").datepicker("option", "maxDate", new Date(dt))
            },

            beforeShow: function (textbox, instance) {
                var date = $("#FromDate").datepicker('getDate');
                if (date) {
                    date.setDate(date.getDate()); //- 1
                }
                //  $("#ToDate").datepicker("option", "minDate", date);
                var rect = textbox.getBoundingClientRect();
                setTimeout(function () {
                    var scrollTop = $("body").scrollTop();
                    instance.dpDiv.css({ top: rect.top + textbox.offsetHeight + scrollTop });
                }, 0);
            }
        });
        $('body').on('click', '#ApplicationRadio', appln);
        $('body').on('click', '#NotificationRadio', notif);
    });

    function AllocatePOP() {
        startAnimation();
        $('#AllocateUser').load('../SORTApplication/SORTAllocateUser', {},
            function () {
                $("#overlay").hide();
                $('#AllocateUser').find('#table-head').remove();
                $('#AllocateUser').find('.pb-3').remove();
                $('#AllocateUser').show();
                stopAnimation();
            });

    }
    function appln() {
        debugger
        $('.effe').hide();
        //notifassess
        //notifassessin
        $('.notifassess').hide();
        $('.notifassessin').show();
    }

    function notif() {
        debugger
       
        $('.effe').show();
        $('.eff').show();
        }
       

