var isSpecialOrderOrVr1;
function MovementAssessDetailsInit() {
    isSpecialOrderOrVr1 = $('#hf_isSpecialOrderOrVr1').val();
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
            if (isSpecialOrderOrVr1 == "True") {
                $('.eff').hide();
            }
        }
        var isSort = $('#IsSortUser').val();
        var SORTCreateJob = $('#hf_SORTCreateJob').val();
        var SORTAllocateJob = $('#hf_SORTAllocateJob').val();
        if (isSort == 'True' && SORTAllocateJob=="1") {
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
                        SortAllocateUserInit();
                    });
            }
        }
        $("#FromDate").datepicker({
            dateFormat: "dd-mm-yy",
            changeYear: true,
            changeMonth: true,
            minDate: new Date(),
            onSelect: function (selected) {
                var startDate = selected.split("-").reverse().join("/");
                var fromDate = new Date(startDate);
                var toDate = $("#ToDate").datepicker('getDate');
                if (fromDate > toDate) {
                    $("#ToDate").datepicker("setDate", fromDate);
                }
                if (typeof showImminentMovementBanner != 'undefined') {
                    showImminentMovementBanner(selected,'#movement_type_confirmation ');
                }
            },
            beforeShow: function (textbox, instance) {
                var rect = textbox.getBoundingClientRect();
                setTimeout(function () {
                    var scrollTop = $("body").scrollTop();
                    instance.dpDiv.css({ top: rect.top + textbox.offsetHeight + scrollTop });
                }, 0);
                $("body").addClass('date-picker-pos-cl');
            },
            onClose: function () {
                setTimeout(function () {
                    $("body").removeClass('date-picker-pos-cl');
                }, 500)
            }
        });
        $("#ToDate").datepicker({
            dateFormat: "dd-mm-yy",
            changeYear: true,
            changeMonth: true,
            minDate: 0,
            beforeShow: function (textbox, instance) {
                var rect = textbox.getBoundingClientRect();
                setTimeout(function () {
                    var scrollTop = $("body").scrollTop();
                    instance.dpDiv.css({ top: rect.top + textbox.offsetHeight + scrollTop });
                }, 0);
                var startDate = $("#FromDate").datepicker('getDate');
                $("#ToDate").datepicker("option", "minDate", startDate);
                $("body").addClass('date-picker-pos-cl');
            },
            onClose: function () {
                setTimeout(function () {
                    $("body").removeClass('date-picker-pos-cl');
                },500)
            }
        });
        GetLocalObject(StepFlag);

        if (document.getElementById('NotificationRadio').checked) {
            notif();
        }
    }
    else {
        $('#movementGeneralDetails').html('');
        $('#movementGeneralDetails').hide();
        $("#allocate_btn").hide();
    }
    stopAnimation();
    if (isTopNavClicked) {
        isTopNavClicked = false;
        CreateLocalObject(StepFlag);//It will be used for top nav
    }

    if ($('#pm_vehicleClass').val() == VEHICLE_CLASSIFICATION_TYPE_CONFIG.VEHICLE_SPECIAL_ORDER) {
        $('#IsVSO').val(true);
    }
    else {
        $('#IsVSO').val(false);
    }
}