var userTypeID = $('#hf_UserTypeID').val();
var actionCompleted = 0;
function SetWorkflowProgress(currentStep) {
    var NextActionValTemp = parseInt($('#hf_NextAction').val()) || currentStep;
    $('#hf_NextAction').remove();    

    actionCompleted = parseInt($('#hf_ActionCompleted').val()) || actionCompleted;
    $('#hf_ActionCompleted').remove();
    if (actionCompleted <= StepFlag) {
        actionCompleted = StepFlag;
        currentStep = StepFlag;
    }
    else if (actionCompleted != 0 && parseInt(currentStep) < actionCompleted) {
        currentStep = actionCompleted;
    }
    

    if (currentStep == '2') {
        $("#back_btn").hide();
    }
    else {
        $("#back_btn").show();
        $("#back_btn").prop('disabled', false);
    }

    if ($("#IsNotif").val() == 'true') {
        $('#step5 p').text('Route Assessment');
    }
    else {
        if (userTypeID == 696008) {
            $('#step5 p').text('Route');
        }
        else {
            $('#step5 p').text('Supplementary Information');
        }
    }
   /* for (let i = 2; i < 8; i++) {
        $('#progressbar').find('li:nth-child(' + i + ')').removeClass('active');
        $('#progressbar').find('li#step' + i).removeClass('completed');
        $('#step' + i + ' p').css('cursor', 'default');
        $('#step' + i + ' p').css('text-decoration', 'none');
    }*/
    $('#progressbar').find('li').removeClass('active');
    $('#progressbar').find('li').removeClass('completed');
    if (userTypeID == 696008) {
        $('#progressbar').find('li p').css('text-decoration', 'none');
        $('#step' + (StepFlag + 1) + ' p').css('text-decoration', 'underline');

        $('#progressbar').find('li:nth-child(' + (currentStep + 1) + ')').addClass('active');
        
        if (currentStep != 0) {
            for (let i = 1; i <= currentStep; i++) {
                if ($('#IsSortRevise').val() == 'true' && i == 1) {
                    $('#progressbar').find('li:nth-child(' + i + ')').addClass('active');
                }
                else {
                    $('#progressbar').find('li:nth-child(' + i + ')').addClass('active');
                    if (i <= NextActionValTemp) {
                        $('#progressbar').find('li#step' + i).addClass('completed').trigger('AddNavigationForWorkflowSeq', [i]);
                    }
                }
            }
        }
    }
    else {
        $('#progressbar').find('li p').css('text-decoration', 'none');
        $('#step' + StepFlag + ' p').css('text-decoration', 'underline');
        $('#progressbar').find('li:nth-child(' + currentStep + ')').addClass('active');
        $('#progressbar').find('li#step' + currentStep).removeClass('completed');
        for (let i = 1; i < currentStep; i++) {
            $('#progressbar').find('li:nth-child(' + i + ')').addClass('active');
            if (i < NextActionValTemp) {
                $('#progressbar').find('li#step' + i).addClass('completed');
            }
            $('#progressbar').find('li#step' + i).trigger('AddNavigationForWorkflowSeq', [i]);
        }
    }

}

$('body').off('click', '#progressbar li.active');
$('body').on('click', '#progressbar li.active', function (e) {
    e.preventDefault();
    var seqIndex = $(this).data('step');
    if ($('#PortalType').val() == "696008") {//FOR SORT-To Handle Haulier step
        seqIndex = parseInt(seqIndex) - 1;
    }
    if (seqIndex > StepFlag) {
        if ($('.imgAmendComponentAxle').length > 0 && $('.imgAmendComponentAxle').is(':visible')) {
            showToastMessage({
                message: "Please amend axles for tractor before proceeding further.",
                type: "error"
            });
            return;
        }
    }
    PlanMoveNavigateFlow(seqIndex, false,true);
    //NavigationForWorkflowSequenceOnTopNavClick(index);
});

//var seqIndex = 0;
//$('#progressbar').on(
//    "AddNavigationForWorkflowSeq", function (event, index) {
//        //$('#step' + index + ' p').css('cursor', 'pointer');
//        //$('#step' + index + ' p').css('text-decoration', 'underline');
//        seqIndex = (userTypeID != 696008) ? index : index - 1;
//    });
