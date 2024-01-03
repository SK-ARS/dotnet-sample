
    function SetWorkflowProgress(currentStep) {        
        if (currentStep == '2') {
            $("#back_btn").prop('disabled', true);
        }
        else {
            $("#back_btn").prop('disabled', false);
        }

        if ($("#IsNotif").val() == 'true') {
            $('#step5 p').text('Route Assessment');
        }
        else {
            if (@userTypeID == 696008) {
                $('#step5 p').text('Route');
            }
            else {
                $('#step5 p').text('Supplementary Information');
            }
        }
        for (let i = 2; i < 8; i++) {
            $('#progressbar').find('li:nth-child(' + i + ')').removeClass('active');
            $('#progressbar').find('li#step' + i).removeClass('completed');
            $('#step' + i + ' p').css('cursor', 'default');
            $('#step' + i + ' p').css('text-decoration', 'none');
        }
        if (@userTypeID == 696008) {
            $('#progressbar').find('li:nth-child(' + (currentStep + 1) + ')').addClass('active');
            if (currentStep != 0) {
                for (let i = 1; i <= currentStep; i++) {
                    if ($('#IsSortRevise').val() == 'true' && i == 1) {
                        $('#progressbar').find('li:nth-child(' + i + ')').addClass('active');
                    }
                    else {
                       
                            $('#progressbar').find('li:nth-child(' + i + ')').addClass('active');
                            $('#progressbar').find('li#step' + i).addClass('completed').trigger('AddNavigationForWorkflowSeq', [i]);
                       
                    }

                }
            }
        }
        else {
            $('#progressbar').find('li:nth-child(' + currentStep + ')').addClass('active');

            for (let i = 1; i < currentStep; i++) {
                $('#progressbar').find('li:nth-child(' + i + ')').addClass('active');

                $('#progressbar').find('li#step' + i).addClass('completed').trigger('AddNavigationForWorkflowSeq', [i]);
            }
        }

    }

    $('#progressbar').on(
        "AddNavigationForWorkflowSeq", function (event, index) {
            $('#step' + index + ' p').css('cursor', 'pointer');
            $('#step' + index + ' p').css('text-decoration', 'underline');
            if (@userTypeID != 696008)
                $('#step' + index + ' p').attr("onclick", "NavigationForWrkflwSequence(" + index + ")");
            else
                $('#step' + index + ' p').attr("onclick", "NavigationForWrkflwSequence(" + (index - 1) + ")");
        });
