    var onchangeflag = false;
        $(document).ready(function () {          
        $(".onchangefunctionVR1Suppl").on('click', onchangefunctionVR1Suppl);
        $("#CancelEditingSupplInfo").on('click', CancelEditingSupplInfo);
        if (($('#IsClone').val() != 1) && ($('#IsRevise').val() != 1)) {
            StepFlag = 5;
            SubStepFlag = 0;
            CurrentStep = "Supplementary Information";
            //$('#plan_movement_hdng').text("PLAN MOVEMENT");
            $('#current_step').text(CurrentStep);
            SetWorkflowProgress(5);
            $('#back_btn').show();
            $('#save_btn').show();
            $('#confirm_btn').hide();
            $('#apply_btn').hide();
        }

        if ($('#SupplInfoSaveFlag').val() == 'true') {
            $('#cancel_suppli_info_btn_cntr').show();
        }

        if('@Model.Shipment' == 1 )
        {
            btn = document.getElementById("partseatrue");
            btn.checked = true;
            $('#PortNames').attr('disabled', false);
            $('#SeaQuotation').attr('disabled', false);
        }
        else
        {
            btn = document.getElementById("partseafalse");
            btn.checked = true;
            $('#PortNames').attr('disabled', true);
            $('#SeaQuotation').attr('disabled', true);
        }

        if ('@Model.LoadDivision' == 1) {
            btn = document.getElementById("YesLoadDiv");
            btn.checked = true;

            $('#AdditionalCost').attr('disabled', false);
            $('#RiskNature').attr('disabled', false);
        }
        else {

            btn = document.getElementById("NoLoadDiv");
            btn.checked = true;
            $('#AdditionalCost').attr('disabled', true);
            $('#RiskNature').attr('disabled', true);
        }

        if ('@Model.TotalDistanceOfRoad' != "") {
            if ('@Model.Address' == 1) {
                btn = document.getElementById("YesAddress");
                btn.checked = true;
                $('#ProposedMoveDetails').attr('disabled', true);
            }
            else {
                btn = document.getElementById("NoAddress");
                btn.checked = true;

                $('#ProposedMoveDetails').attr('disabled', false);

            }
        }
        else {
            btn = document.getElementById("YesAddress");
            btn.checked = true;
            btn.value = 1;
            $('#ProposedMoveDetails').attr('disabled', true);
        }
    });

    $('input:radio[name="partsea"]').change(function () {
        var partsea = $(this).val();
        if(partsea==0)
        {
            $('#PortNames').attr('disabled', true);
            $('#SeaQuotation').attr('disabled', true);
            $("#Shipment").val(0);
        }
        else
        {
            $('#PortNames').attr('disabled', false);
            $('#SeaQuotation').attr('disabled', false);
            $("#Shipment").val(1);
        }
    });

    $('input:radio[name="Address"]').change(function () {
        var partsea = $(this).val();
        if (partsea == 0) {
            $('#ProposedMoveDetails').attr('disabled', false);
            $("#Address").val(0);
        }
        else {
            $('#ProposedMoveDetails').attr('disabled', true);
            $("#Address").val(1);
        }
    });

    $('input:radio[name="LoadDiv"]').change(function () {
        var partsea = $(this).val();
        if (partsea == 0) {
            $('#AdditionalCost').attr('disabled', true);
            $('#RiskNature').attr('disabled', true);
            $("#LoadDivision").val(0);
        }
        else {
            $('#AdditionalCost').attr('disabled', false);
            $('#RiskNature').attr('disabled', false);
            $("#LoadDivision").val(1);
        }
    });

    function checknoncomplusaryfield() {
        ;
        var isvalid = true;
        
        var decimalRegex = /^[1-9]\d*(\.\d+)?$/;

        if ($('#TotalDistanceOfRoad').val().trim().length == 0) {
            $('#spTotalDistOfRoad').html('Please enter Total distance of the road');
            isvalid = false;
        }
        else if (!decimalRegex.test($('#TotalDistanceOfRoad').val())) {
            $('#spTotalDistOfRoad').html('Please enter valid number');
            isvalid = false;
        }


        if ((!decimalRegex.test($('#ApprValueOfLoad').val())) && ($('#ApprValueOfLoad').val().length > 0)) {
            $('#spApprValueOfLoad').html('Please enter valid number');
            isvalid = false;
        }
        else {
            $('#spApprValueOfLoad').html('');

        }
        if ((!decimalRegex.test($('#AdditionalCost').val())) && ($('#AdditionalCost').val().length > 0)) {
            if ($('#AdditionalCost').val() != 0) {
                $('#spAdditionalCost').html('Please enter valid number');
                isvalid = false;
            }

        }
        else {
            $('#spAdditionalCost').html('');

        }
        if ((!decimalRegex.test($('#ApprCostOfMovement').val())) && ($('#ApprCostOfMovement').val().length > 0)) {
            $('#spApprCostOfMovement').html('Please enter valid number');
            isvalid = false;
        }
        else {
            $('#spApprCostOfMovement').html('');

        }
        return isvalid;
    }

    $('#TotalDistanceOfRoad').keyup(function () {
        $('#spTotalDistOfRoad').html('');
    });

    $('#ApprCostOfMovement').keyup(function () {
        $('#spApprCostOfMovement').html('');
    });

    function SaveSupplimentaryInfo() {
        var chk = checknoncomplusaryfield();
        if (chk == true) {
            var ApplicationRevId = $('#AppRevisionId').val() ? $('#AppRevisionId').val() : 0;          

            $.ajax({
                url: '/Application/SaveSupplimentaryInfo?appRevisionId=' + ApplicationRevId,
                dataType: 'json',
                type: 'POST',
                data: $("#supplimentaryinfo").serialize(),
                beforeSend: function () {
                    startAnimation();
                },
                success: function (result) {
                    $('#TextChangeFlagVR1').val(false);
                    $('#SupplInfoSaveFlag').val(true);
                    if (result == true) {
                        //showWarningPopDialog('', 'Ok', '', 'CloseSaveForSuppl', '', 1, 'info');
                        var Msg ='Supplementary Information saved successfully.'
                        ShowSuccessModalPopup(Msg, "NavigateToSupplConfirm");
                    }
                    else {
                        //showWarningPopDialog('', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                        var msg1 = 'Supplementary Information not saved';
                        ShowErrorPopup(msg1);
                    }
                    scrolltotop();
                },
                error: function (xhr, status) {
                    location.reload();
                },
                complete: function () {
                    stopAnimation();
                }

            });
        }
    }

    function CloseSaveForSuppl() {
        $('.pop-message').html('');
        $('.box_warningBtn1').html('');
        $('.box_warningBtn2').html('');
        $('#pop-warning').hide();

        $('.box_warningBtn1').unbind();
        $('.box_warningBtn2').unbind();
        $('#tab_4').hide();

    }

    function onchangefunctionVR1Suppl() {
        onchangeflag = true;
        $('#TextChangeFlagVR1').val(onchangeflag);
    } 
